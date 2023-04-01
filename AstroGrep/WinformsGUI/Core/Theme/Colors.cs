using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// Application theme colors.
	/// </summary>
	public class Colors
	{
		/// <summary>
		/// Current application accent color.
		/// </summary>
		public Color ApplicationAccentColor { get; set; }

		/// <summary>
		/// Back color.
		/// </summary>
		public Color BackColor { get; set; }

		/// <summary>
		/// Control color.
		/// </summary>
		public Color Control { get; set; }

		/// <summary>
		/// Control dark color.
		/// </summary>
		public Color ControlDark { get; set; }

		/// <summary>
		/// Control dark-dark color.
		/// </summary>
		public Color ControlDarkDark { get; set; }

		/// <summary>
		/// Control light color.
		/// </summary>
		public Color ControlLight { get; set; }

		/// <summary>
		/// Fore color.
		/// </summary>
		public Color ForeColor { get; set; }

		/// <summary>
		/// <see cref="System.Windows.Forms.LinkLabel"/> color.
		/// </summary>
		public Color LinkColor { get; set; }

		/// <summary>
		/// Window color.
		/// </summary>
		public Color Window { get; set; }
	}
}