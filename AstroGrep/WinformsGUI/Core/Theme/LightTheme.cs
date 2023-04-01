using System.Drawing;
using System.Windows.Forms;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// The application light/default color theme.
	/// </summary>
	public class LightTheme : ITheme
	{
		/// <summary>
		/// Initialize this theme and its colors.
		/// </summary>
		public LightTheme()
		{
			Colors.BackColor = SystemColors.Window;
			Colors.ForeColor = SystemColors.ControlText;
			Colors.LinkColor = SystemColors.HotTrack;
			Colors.ApplicationAccentColor = Core.GeneralSettings.UseAstroGrepAccentColor ? Common.ProductInformation.ApplicationColor : SystemColors.ControlText;

			Colors.Control = SystemColors.Control;
			Colors.ControlDark = SystemColors.ControlDark;
			Colors.ControlDarkDark = SystemColors.ControlDarkDark;
			Colors.ControlLight = SystemColors.ControlLight;
			Colors.Window = SystemColors.Window;
		}

		/// <inheritdoc/>
		public Colors Colors { get; } = new Colors();

		/// <inheritdoc/>
		public ToolStripRenderer MenuRenderer { get; } = new ToolStripProfessionalRenderer();

		/// <inheritdoc/>
		public ToolStripRenderer ToolStripRenderer { get; } = new ToolStripProfessionalRenderer();
	}
}