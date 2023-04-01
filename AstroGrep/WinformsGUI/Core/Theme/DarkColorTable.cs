using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroGrep.Core.Theme
{
	/// <summary>
	/// The dark theme color table for a <see cref="ToolStripProfessionalRenderer"/>.
	/// </summary>
	public class DarkColorTable : ProfessionalColorTable
	{
		/// <summary>
		/// Dark arrow color
		/// </summary>
		public static readonly Color ArrowColor = Color.FromArgb(255, 255, 255);

		/// <summary>
		/// Dark check pressed color
		/// </summary>
		public static readonly Color CheckPressedColor = Color.Gray;

		/// <summary>
		/// Dark menu back color
		/// </summary>
		public static readonly Color MenuBackColor = Color.FromArgb(43, 43, 43);

		/// <summary>
		/// Dark menu fore color
		/// </summary>
		public static readonly Color MenuForeColor = Color.FromArgb(255, 255, 255);

		/// <summary>
		/// Dark selection color
		/// </summary>
		public static readonly Color SelectionColor = Color.FromArgb(65, 65, 65);

		/// <summary>
		/// Dark separator color
		/// </summary>
		public static readonly Color SeparatorColor = Color.FromArgb(128, 128, 128);

		/// <inheritdoc/>
		public override Color ButtonCheckedGradientBegin => CheckPressedColor;
		/// <inheritdoc/>
		public override Color ButtonCheckedGradientEnd => CheckPressedColor;
		/// <inheritdoc/>
		public override Color ButtonCheckedGradientMiddle => CheckPressedColor;
		/// <inheritdoc/>
		public override Color ButtonSelectedGradientBegin => SelectionColor;
		/// <inheritdoc/>
		public override Color ButtonSelectedGradientEnd => SelectionColor;
		/// <inheritdoc/>
		public override Color ButtonSelectedGradientMiddle => SelectionColor;
		/// <inheritdoc/>
		public override Color CheckBackground => CheckPressedColor;
		/// <inheritdoc/>
		public override Color CheckPressedBackground => SelectionColor;
		/// <inheritdoc/>
		public override Color CheckSelectedBackground => MenuBackColor;
		/// <inheritdoc/>
		public override Color ImageMarginGradientBegin => MenuBackColor;
		/// <inheritdoc/>
		public override Color ImageMarginGradientEnd => MenuBackColor;
		/// <inheritdoc/>
		public override Color ImageMarginGradientMiddle => MenuBackColor;
		/// <inheritdoc/>
		public override Color MenuItemPressedGradientBegin => MenuBackColor;
		/// <inheritdoc/>
		public override Color MenuItemPressedGradientEnd => MenuBackColor;
		/// <inheritdoc/>
		public override Color MenuItemSelected => SelectionColor;
		/// <inheritdoc/>
		public override Color MenuItemSelectedGradientBegin => SelectionColor;
		/// <inheritdoc/>
		public override Color MenuItemSelectedGradientEnd => SelectionColor;
		/// <inheritdoc/>
		public override Color MenuStripGradientBegin => MenuBackColor;
		/// <inheritdoc/>
		public override Color MenuStripGradientEnd => MenuBackColor;
		/// <inheritdoc/>
		public override Color SeparatorDark => SeparatorColor;
		/// <inheritdoc/>
		public override Color ToolStripBorder => SeparatorColor;
		/// <inheritdoc/>
		public override Color ToolStripDropDownBackground => MenuBackColor;
		/// <inheritdoc/>
		public override Color ToolStripGradientBegin => MenuBackColor;
		/// <inheritdoc/>
		public override Color ToolStripGradientEnd => MenuBackColor;
		/// <inheritdoc/>
		public override Color ToolStripGradientMiddle => MenuBackColor;
	}
}