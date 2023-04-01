using System.Windows.Forms;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// Defined theme interface for this application.
	/// </summary>
	public interface ITheme
	{
		/// <summary>
		/// The current theme colors.
		/// </summary>
		Colors Colors { get; }

		/// <summary>
		/// The current theme's <see cref="ToolStripRenderer"/> for a menu / context menu.
		/// </summary>
		ToolStripRenderer MenuRenderer { get; }

		/// <summary>
		/// The current theme's <see cref="ToolStripRenderer"/> for a tool strip.
		/// </summary>
		ToolStripRenderer ToolStripRenderer { get; }
	}
}