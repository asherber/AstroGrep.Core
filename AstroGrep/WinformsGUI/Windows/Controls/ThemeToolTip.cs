using System.Windows.Forms;

namespace AstroGrep.Windows.Controls
{
	/// <summary>
	/// A theme supported <see cref="ToolTip"/>.
	/// </summary>
	public class ThemeToolTip : ToolTip
	{
		/// <summary>
		/// Creates a new instance of this <see cref="ToolTip"/> with theme support.
		/// </summary>
		public ThemeToolTip() : base()
		{
			OwnerDraw = true;
			BackColor = Core.Theme.ThemeProvider.Theme.Colors.BackColor;
			ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;
			Draw += ThemeToolTip_Draw;
		}

		/// <summary>
		/// Creates a new instance of this <see cref="ToolTip"/> with theme support.
		/// </summary>
		/// <param name="cont"></param>
		public ThemeToolTip(System.ComponentModel.IContainer cont) : base(cont)
		{
			OwnerDraw = true;
			BackColor = Core.Theme.ThemeProvider.Theme.Colors.BackColor;
			ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;
			Draw += ThemeToolTip_Draw;
		}

		/// <summary>
		/// Reset the <see cref="ToolTip"/>'s theme colors.
		/// </summary>
		public void Reset()
		{
			BackColor = Core.Theme.ThemeProvider.Theme.Colors.BackColor;
			ForeColor = Core.Theme.ThemeProvider.Theme.Colors.ForeColor;
		}

		/// <summary>
		/// Handles custom drawing the tool tip to support theming.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ThemeToolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			e.DrawBackground();
			e.DrawBorder();
			e.DrawText(TextFormatFlags.Default | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
		}
	}
}
