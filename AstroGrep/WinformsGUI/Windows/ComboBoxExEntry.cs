using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroGrep.Windows
{
	/// <summary>
	/// A single entry for the <see cref="Controls.ComboBoxEx"/>.
	/// </summary>
	/// <remarks>
	///   AstroGrep File Searching Utility. Written by Theodore L. Ward
	///   Copyright (C) 2002 AstroComma Incorporated.
	///
	///   This program is free software; you can redistribute it and/or
	///   modify it under the terms of the GNU General Public License
	///   as published by the Free Software Foundation; either version 2
	///   of the License, or (at your option) any later version.
	///
	///   This program is distributed in the hope that it will be useful,
	///   but WITHOUT ANY WARRANTY; without even the implied warranty of
	///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	///   GNU General Public License for more details.
	///
	///   You should have received a copy of the GNU General Public License
	///   along with this program; if not, write to the Free Software
	///   Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
	///
	///   The author may be contacted at:
	///   ted@astrocomma.com or curtismbeard@gmail.com
	/// </remarks>
	/// <history>
	/// [Curtis_Beard]    09/16/2019	NEW: support drawing separators in the combo box drop down
	/// </history>
	public class ComboBoxExEntry : IEquatable<ComboBoxExEntry>
	{
		/// <summary>
		///
		/// </summary>
		public ComboBoxExEntry()
		{
			Display = string.Empty;
			Value = string.Empty;
			IsSystem = false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="display"></param>
		/// <param name="value"></param>
		public ComboBoxExEntry(string display, string value) : this()
		{
			Display = display;
			Value = value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="display"></param>
		/// <param name="value"></param>
		/// <param name="isSystem"></param>
		public ComboBoxExEntry(string display, string value, bool isSystem)
		   : this(display, value)
		{
			IsSystem = isSystem;
		}

		/// <summary></summary>
		public string Display { get; set; }

		/// <summary></summary>
		public bool IsSystem { get; set; }

		/// <summary></summary>
		public string Value { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ComboBoxExEntry other)
		{
			if (other == null)
				return false;

			return Display == other.Display && Value == other.Value && IsSystem == other.IsSystem;
		}
	}
}