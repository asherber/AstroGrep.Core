using System.Drawing;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
	/// <summary>
	/// A separator control with custom dark theme support.
	/// </summary>
	public class ThemeSeparator : Control
	{
		#region Constructor Region

		/// <summary>
		/// Constructor, sets Dock and Size.
		/// </summary>
		public ThemeSeparator()
		{
			SetStyle(ControlStyles.Selectable, false);

			Dock = DockStyle.Top;
			Size = new Size(1, 2);
		}

		#endregion

		#region Paint Region

		/// <inheritdoc/>
		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;

			using (var p = new Pen(Core.Theme.ThemeProvider.Theme.Colors.ControlDark))
			{
				g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);
			}

			using (var p = new Pen(Core.Theme.ThemeProvider.Theme.Colors.ControlLight))
			{
				g.DrawLine(p, ClientRectangle.Left, 1, ClientRectangle.Right, 1);
			}
		}

		/// <inheritdoc/>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Absorb event
		}

		#endregion
	}
}
