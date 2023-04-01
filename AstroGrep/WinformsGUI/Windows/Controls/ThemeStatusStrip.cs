using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
	/// <summary>
	/// A <see cref="StatusStrip"/> with custom dark theme support.
	/// </summary>
	public class ThemeStatusStrip : StatusStrip
	{
		#region Constructor Region

		/// <summary>
		/// Constructor, sets back and fore colors
		/// </summary>
		public ThemeStatusStrip()
		{
			BackColor = Core.Theme.ThemeProvider.Theme.Colors.Control;
			ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;
		}

		#endregion

		/// <summary>
		/// Resets the back and fore colors and redraws itself
		/// </summary>
		public void Reset()
		{
			BackColor = Core.Theme.ThemeProvider.Theme.Colors.Control;
			ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;

			Invalidate();
		}

		#region Paint Region

		/// <inheritdoc/>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			var g = e.Graphics;

			using (var b = new SolidBrush(Core.Theme.ThemeProvider.Theme.Colors.Control))
			{
				g.FillRectangle(b, ClientRectangle);
			}

			using (var p = new Pen(Core.Theme.ThemeProvider.Theme.Colors.ControlDark))
			{
				g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);
			}

			using (var p = new Pen(Core.Theme.ThemeProvider.Theme.Colors.ControlLight))
			{
				g.DrawLine(p, ClientRectangle.Left, 1, ClientRectangle.Right, 1);
			}
		}

		#endregion
	}
}
