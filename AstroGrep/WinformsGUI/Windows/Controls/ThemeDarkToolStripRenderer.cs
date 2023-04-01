using System.Drawing;
using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
	/// <summary>
	/// A tool strip based <see cref="ToolStripProfessionalRenderer"/> with custom dark theme support.
	/// </summary>
	public class ThemeDarkToolStripRenderer : ToolStripProfessionalRenderer
	{
		/// <summary>
		/// Instructor
		/// </summary>
		public ThemeDarkToolStripRenderer() : base(new Core.Theme.DarkColorTable())
		{

		}

		/// <inheritdoc/>
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Core.Theme.DarkColorTable.MenuBackColor), e.AffectedBounds);
			base.OnRenderToolStripBackground(e);
		}

		/// <inheritdoc/>
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = Core.Theme.DarkColorTable.MenuForeColor;
			base.OnRenderItemText(e);
		}

		/// <inheritdoc/>
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			e.ArrowColor = Core.Theme.DarkColorTable.ArrowColor;
			base.OnRenderArrow(e);
		}

		/// <inheritdoc/>
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if (e.ToolStrip is MenuStrip || e.ToolStrip is ContextMenuStrip)
			{
				// draw the full line across the menu
				using (Pen forecolorpen = new Pen(new SolidBrush(Core.Theme.DarkColorTable.SeparatorColor), 1))
				{
					Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
					int startY = bounds.Height / 2;

					e.Graphics.DrawLine(forecolorpen, bounds.Left + 1, startY, bounds.Right - 1, startY);
				}
			}

			base.OnRenderSeparator(e);
		}
	}
}
