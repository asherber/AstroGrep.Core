using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
	/// <summary>
	/// Custom implemented ComboBox to allow a separating line.
	/// </summary>
	/// <remarks>
	///   AstroGrep File Searching Utility. Written by Theodore L. Ward
	///   Copyright (C) 2002 AstroComma Incorporated.
	///
	///   This program is free software; you can redistribute it and/or
	///   modify it under the terms of the GNU General Public License
	///   as published by the Free Software Foundation; either version 2
	///   of the License, or (at your option) any later version.
	///
	///   This program is distributed in the hope that it will be useful,
	///   but WITHOUT ANY WARRANTY; without even the implied warranty of
	///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	///   GNU General Public License for more details.
	///
	///   You should have received a copy of the GNU General Public License
	///   along with this program; if not, write to the Free Software
	///   Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
	///
	///   The author may be contacted at:
	///   ted@astrocomma.com or curtismbeard@gmail.com
	/// </remarks>
	/// <history>
	/// [Curtis_Beard]    09/16/2019	NEW: support drawing separators in the combo box drop down
	/// </history>
	public class ComboBoxEx : ComboBox
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct COMBOBOXINFO
		{
			public Int32 cbSize;
			public RECT rcItem;
			public RECT rcButton;
			public ComboBoxButtonState buttonState;
			public IntPtr hwndCombo;
			public IntPtr hwndEdit;
			public IntPtr hwndList;
		}

		[Serializable, StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			/// <summary>
			///
			/// </summary>
			/// <param name="rect"></param>
			public RECT(Rectangle rect)
			{
				Left = rect.Left;
				Top = rect.Top;
				Right = rect.Right;
				Bottom = rect.Bottom;
			}

			/// <summary>
			///
			/// </summary>
			public Rectangle Rect
			{
				get
				{
					return new Rectangle(Left, Top, Right - Left, Bottom - Top);
				}
			}

			/// <summary>
			///
			/// </summary>
			public Point Location
			{
				get
				{
					return new Point(Left, Top);
				}
			}

			/// <summary></summary>
			public int Left;

			/// <summary></summary>
			public int Top;

			/// <summary></summary>
			public int Right;

			/// <summary></summary>
			public int Bottom;
		}

		/// <summary>
		/// The predefined string value to signal the list to drawn a separator line.
		/// </summary>
		public const string Separator = "~-";

		private const int VK_DOWN = 0x28;
		private const int VK_END = 0x23;
		private const int VK_HOME = 0x24;
		private const int VK_LEFT = 0x25;
		private const int VK_NEXT = 0x22;
		private const int VK_PRIOR = 0x21;
		private const int VK_RIGHT = 0x27;
		private const int VK_UP = 0x26;

		private const int WM_KEYDOWN = 0x100;
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_MOUSEMOVE = 0x200;

		private bool __DownKey = false;
		private bool __UpKey = false;
		private bool dropDownOpen;
		private ToolTip toolTip;
		private bool toolTipVisible;

		/// <summary>
		///
		/// </summary>
		public ComboBoxEx()
		{
			DrawMode = DrawMode.OwnerDrawFixed;

			toolTip = new ToolTip
			{
				UseAnimation = false,
				UseFading = false
			};
		}

		private enum ComboBoxButtonState
		{
			StateSystemNone = 0,
			StateSystemInvisible = 0x00008000,
			StateSystemPressed = 0x00000008
		}

		/// <summary>
		///
		/// </summary>
		private IntPtr HwndCombo
		{
			get
			{
				COMBOBOXINFO pcbi = new COMBOBOXINFO();
				pcbi.cbSize = Marshal.SizeOf(pcbi);
				GetComboBoxInfo(Handle, ref pcbi);
				return pcbi.hwndCombo;
			}
		}

		/// <summary>
		///
		/// </summary>
		private IntPtr HwndDropDown
		{
			get
			{
				COMBOBOXINFO pcbi = new COMBOBOXINFO();
				pcbi.cbSize = Marshal.SizeOf(pcbi);
				GetComboBoxInfo(Handle, ref pcbi);
				return pcbi.hwndList;
			}
		}

		/// <summary>
		/// Handles drawing the radio button list.
		/// </summary>
		/// <param name="e">Draw event args</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0) return;

			if (Items.Count > 0)
			{
				var text = GetItemText(Items[e.Index]);
				Rectangle r = e.Bounds;
				var flags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
				var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

				var bc = selected ? (Enabled && IsItemEnabled(e.Index) ? SystemColors.Highlight :
				   SystemColors.InactiveBorder) : BackColor;
				var fc = selected ? (Enabled && IsItemEnabled(e.Index) ? SystemColors.HighlightText :
				   SystemColors.GrayText) : (IsItemEnabled(e.Index) ? ForeColor : SystemColors.GrayText);

				using (var b = new SolidBrush(bc))
				{
					e.Graphics.FillRectangle(b, e.Bounds);
				}

				// draw separator
				if (IsItemSeparator(e.Index))
				{
					using (var pen = new Pen(SystemColors.GrayText))
					{
						e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + (e.Bounds.Height / 2), e.Bounds.X + e.Bounds.Width, e.Bounds.Y + (e.Bounds.Height / 2));
					}
				}
				else
				{
					TextRenderer.DrawText(e.Graphics, text, Font, r, fc, bc, flags);
					e.DrawFocusRectangle();

					if (selected)
					{
						if (dropDownOpen)
						{
							GetWindowRect(HwndDropDown, out RECT rcDropDown);

							Size szText = TextRenderer.MeasureText(text, Font);
							if (szText.Width > rcDropDown.Rect.Width - SystemInformation.VerticalScrollBarWidth && !toolTipVisible)
							{
								GetWindowRect(HwndCombo, out RECT rcCombo);

								if (rcCombo.Top > rcDropDown.Top)
								{
									ShowToolTip(text, e.Bounds.X + rcDropDown.Rect.Width + 5, e.Bounds.Y - rcDropDown.Rect.Height - ItemHeight - 5);
								}
								else
								{
									ShowToolTip(text, e.Bounds.X + rcDropDown.Rect.Width + 5, e.Bounds.Y + ItemHeight);
								}
							}
						}
					}
					else
					{
						ResetToolTip();
					}
				}
			}

			base.OnDrawItem(e);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDropDown(EventArgs e)
		{
			dropDownOpen = true;
			base.OnDropDown(e);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDropDownClosed(EventArgs e)
		{
			dropDownOpen = false;
			ResetToolTip();
			base.OnDropDownClosed(e);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			__UpKey = false;
			__DownKey = false;
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.Up)
				__UpKey = true;
			else if (e.KeyCode == Keys.Down)
				__DownKey = true;
		}

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (IsItemSeparator(e.Index))
			{
				e.ItemHeight = 2; // 2 spaces on each side of 1 height line
			}
			else
			{
				base.OnMeasureItem(e);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			ResetToolTip();
			base.OnMouseLeave(e);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			string value = GetItemText(Items[SelectedIndex]);

			if (value.Equals(Separator))
			{
				if (__UpKey)
					if (this.SelectedIndex == 0)
						this.SelectedIndex += 1;
					else
						this.SelectedIndex -= 1;
				else if (__DownKey)
					if (this.SelectedIndex == this.Items.Count - 1)
						this.SelectedIndex -= 1;
					else
						this.SelectedIndex += 1;
				else
				   if (this.SelectedIndex == 0)
					this.SelectedIndex += 1;
				else
					this.SelectedIndex -= 1;
			}
			else
				base.OnSelectedIndexChanged(e);
		}

		[DllImport("user32.dll")]
		private static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

		/// <summary>
		/// Determines if an Item is enabled or disabled.
		/// </summary>
		/// <param name="index">Current Item index.</param>
		/// <returns>true if item is enabled, false if disabled</returns>
		private bool IsItemEnabled(int index)
		{
			// separators always disabled
			if (IsItemSeparator(index))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Determines if the given item is a separator.
		/// </summary>
		/// <param name="index">index of item to check</param>
		/// <returns>True if separator, False otherwise</returns>
		private bool IsItemSeparator(int index)
		{
			return GetItemText(index >= 0 && index < Items.Count ? Items[index] : "").Equals(Separator);
		}

		/// <summary>
		/// Gets the maximum number of items visible in the control.
		/// </summary>
		/// <returns>number of items visible</returns>
		private int NumVisibleItems()
		{
			return Height / ItemHeight;
		}

		/// <summary>
		///
		/// </summary>
		private void ResetToolTip()
		{
			if (toolTipVisible)
			{
				toolTip.SetToolTip(this, null);
				toolTipVisible = false;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void ShowToolTip(string text, int x, int y)
		{
			toolTip.Show(text, this, x, y);
			toolTipVisible = true;
		}
	}
}