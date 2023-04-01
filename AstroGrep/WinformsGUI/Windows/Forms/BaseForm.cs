using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AstroGrep.Windows.Forms
{
	/// <summary>
	/// Base form for all forms to inherit from.
	/// </summary>
	public class BaseForm : Form
	{
		/// <summary>
		/// Determines if the common logic for color changes is applied at the form level.
		/// </summary>
		public bool ProcessColorChange { get; set; } = true;

		/// <summary>
		/// Force a <see cref="Color"/> for the <paramref name="ctrl"/> and its children.
		/// </summary>
		/// <param name="ctrl"><see cref="Control"/> to set</param>
		/// <param name="backColor"><see cref="Color"/> to set</param>
		/// <param name="processChildren">true (default) to process children controls, false to only do given <paramref name="ctrl"/></param>
		public void ForceBackColor(Control ctrl, Color backColor, bool processChildren = true)
		{
			// set color
			ctrl.BackColor = backColor;

			// process the child controls
			if (processChildren && ctrl.HasChildren)
			{
				foreach (Control child in ctrl.Controls)
				{
					ForceBackColor(child, backColor);
				}
			}
		}

		/// <summary>
		/// Load the current them for the <paramref name="ctrl"/> and its children.
		/// </summary>
		/// <param name="ctrl"><see cref="Control"/> to set</param>
		public void LoadTheme(Control ctrl)
		{
			if (ProcessColorChange)
			{
				// Don't change these strip based controls here (they have their own rendering
				if (ctrl is ToolStrip || ctrl is MenuStrip || ctrl is StatusStrip)
				{
					return;
				}

				// Button
				if (ctrl is Button btn)
				{
					// special changes just for main form
					if (ctrl.FindForm() is frmMain)
					{
						btn.UseVisualStyleBackColor = true;
						btn.FlatStyle = FlatStyle.System;
					}
					else
					{
						return;
					}
				}

				// standard/normal control colors
				ctrl.ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;
				ctrl.BackColor = Core.Theme.ThemeProvider.Theme.Colors.Control;

				if (ctrl is TextBox || ctrl is ComboBox)
				{
					ctrl.BackColor = Core.Theme.ThemeProvider.Theme.Colors.Window;
				}

				if (ctrl.FindForm() is frmMain)
				{
					if (GetControlStyle(ctrl, ControlStyles.SupportsTransparentBackColor))
					{
						ctrl.BackColor = Color.Transparent;
					}
					else if (ctrl is Label || ctrl is ComboBox || ctrl is Controls.ComboBoxEx)
					{
						ctrl.BackColor = Core.Theme.ThemeProvider.Theme.Colors.Window;
					}
				}

				// CheckBox special changes (use standard style)
				if (ctrl is CheckBox chk)
				{
					if (chk.FlatStyle != FlatStyle.Standard)
					{
						chk.FlatStyle = FlatStyle.Standard;
					}

					if (GetControlStyle(ctrl, ControlStyles.SupportsTransparentBackColor))
					{
						ctrl.BackColor = Color.Transparent;
					}
				}

				if (ctrl is Label && ctrl.FindForm() is frmOptions)
				{
					if (GetControlStyle(ctrl, ControlStyles.SupportsTransparentBackColor))
					{
						ctrl.BackColor = Color.Transparent;
					}
				}

				// LinkLabel special changes
				if (ctrl is LinkLabel lnk)
				{
					lnk.ActiveLinkColor = Core.Theme.ThemeProvider.Theme.Colors.LinkColor;
					lnk.LinkColor = Core.Theme.ThemeProvider.Theme.Colors.LinkColor;
				}

				// ListView special changes
				if (ctrl is ListView lsv)
				{
					lsv.BackColor = Core.Theme.ThemeProvider.Theme.Colors.Window;
				}

				if (ctrl is Panel pnl)
				{
					if (pnl.FindForm() is frmMain)
					{
						pnl.BackColor = Core.Theme.ThemeProvider.Theme.Colors.Window;
					}
				}

				// process the child controls
				if (ctrl.HasChildren)
				{
					foreach (Control child in ctrl.Controls)
					{
						LoadTheme(child);
					}
				}
			}
		}

		/// <summary>
		/// Load event for all forms
		/// </summary>
		/// <param name="e">system parameter</param>
		protected override void OnLoad(EventArgs e)
		{
			if (!DesignMode)
			{
				LoadTheme(this);
			}

			base.OnLoad(e);
		}

		/// <summary>
		/// Determines if given <see cref="ControlStyles"/> <paramref name="flags"/> are available on the given <paramref name="control"/>.
		/// </summary>
		/// <param name="control"><see cref="Control"/> to check</param>
		/// <param name="flags"><see cref="ControlStyles"/> to check</param>
		/// <returns>true if found, false otherwise</returns>
		private bool GetControlStyle(Control control, ControlStyles flags)
		{
			Type type = control.GetType();
			BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
			MethodInfo method = type.GetMethod("GetStyle", bindingFlags);
			object[] param = { flags };
			return (bool)method.Invoke(control, param);
		}
	}
}