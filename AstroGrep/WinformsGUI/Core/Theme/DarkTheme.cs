using System.Drawing;
using System.Windows.Forms;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// The application dark theme.
	/// </summary>
	public class DarkTheme : ITheme
	{
		/// <summary>
		/// Initialize this theme and its colors.
		/// </summary>
		public DarkTheme()
		{
			Colors.BackColor = Color.FromArgb(51, 51, 51);
			Colors.ForeColor = Color.White;
			Colors.LinkColor = Color.White;
			Colors.ApplicationAccentColor = Core.GeneralSettings.UseAstroGrepAccentColor ? Common.ProductInformation.ApplicationColor : Color.White;

			Colors.Control = DarkColorTable.MenuBackColor;//43,43,43
			Colors.ControlDark = Color.FromArgb(51, 51, 51);
			Colors.ControlDarkDark = Color.FromArgb(25, 25, 25);
			Colors.ControlLight = Color.FromArgb(81, 81, 81);
			Colors.Window = Color.FromArgb(32, 32, 32);
		}

		/// <inheritdoc/>
		public Colors Colors { get; } = new Colors();

		/// <inheritdoc/>
		public ToolStripRenderer MenuRenderer { get; } = new Windows.Controls.ThemeDarkMenuRenderer();

		/// <inheritdoc/>
		public ToolStripRenderer ToolStripRenderer { get; } = new Windows.Controls.ThemeDarkToolStripRenderer();
	}
}