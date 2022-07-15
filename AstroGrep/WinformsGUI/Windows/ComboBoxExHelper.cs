using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroGrep.Windows
{
	/// <summary>
	/// Helper methods around the <see cref="Controls.ComboBoxEx"/> selection.
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
	/// [Curtis_Beard]    09/16/2019	NEW: 130, Created to assist with custom system based file filters
	/// </history>
	public class ComboBoxExHelper
	{
		/// <summary>
		/// Current list of system entries for this implementation.
		/// </summary>
		private List<ComboBoxExEntry> systemEntries = new List<ComboBoxExEntry>();

		/// <summary>
		/// Creates the <see cref="ComboBoxExHelper"/> with the provided system entries.
		/// </summary>
		/// <param name="systemEntries">List of entries used by the system that are always present in the <see cref="Controls.ComboBoxEx"/></param>
		public ComboBoxExHelper(List<ComboBoxExEntry> systemEntries)
		{
			if (systemEntries != null)
			{
				this.systemEntries.AddRange(systemEntries);
			}
		}

		/// <summary>
		/// Add a user entered value to the <see cref="Controls.ComboBoxEx"/>.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to add value to</param>
		/// <param name="entry">The <see cref="ComboBoxExEntry"/> to add</param>
		/// <param name="maxEntries">The maximum number of entries</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void AddUserValue(Controls.ComboBoxEx combo, ComboBoxExEntry entry, int maxEntries)
		{
			if (!entry.IsSystem)
			{
				List<ComboBoxExEntry> userValues = GetUserValues(combo);

				if (userValues.Contains(entry))
				{
					userValues.Remove(entry);
				}

				// Add this path as the first item in the dropdown.
				userValues.Insert(0, entry);

				// Only store as many paths as has been set in options.
				//if (combo.Items.Count > Common.NUM_STORED_PATHS)
				if (userValues.Count > maxEntries)
				{
					// Remove the last item in the list.
					userValues.RemoveAt(userValues.Count - 1);
				}

				LoadUserValues(combo, userValues, true);
			}
		}

		/// <summary>
		/// Add a user entered value to the <see cref="Controls.ComboBoxEx"/>.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to add value to</param>
		/// <param name="value">The value to add</param>
		/// <param name="maxEntries">The maximum number of entries</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void AddUserValue(Controls.ComboBoxEx combo, string value, int maxEntries)
		{
			ComboBoxExEntry entry = new ComboBoxExEntry(value, value);

			AddUserValue(combo, entry, maxEntries);
		}

		/// <summary>
		/// Adjust the <see cref="Controls.ComboBoxEx"/> so that only <paramref name="maxEntries"/> is shown.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to adjust values stored</param>
		/// <param name="maxEntries">The maximum number of entries</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void AdjustUserValueMax(Controls.ComboBoxEx combo, int maxEntries)
		{
			int index = combo.SelectedIndex;
			List<ComboBoxExEntry> entries = GetUserValues(combo);
			while (entries.Count > maxEntries)
				entries.RemoveAt(entries.Count - 1);

			LoadUserValues(combo, entries, true);

			if (index > -1 && index < combo.Items.Count)
			{
				combo.SelectedIndex = index;
				combo.SelectionLength = 0;
			}
		}

		/// <summary>
		/// Clears all user entered values and only shows the system values.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to clear values</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void ClearUserValues(Controls.ComboBoxEx combo)
		{
			LoadUserValues(combo, new List<ComboBoxExEntry>(), true);
		}

		/// <summary>
		/// Get the selected <see cref="ComboBoxExEntry"/>.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to retrieve value from</param>
		/// <returns>The <see cref="ComboBoxExEntry"/> that is selected</returns>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public ComboBoxExEntry GetSelectedEntry(Controls.ComboBoxEx combo)
		{
			if (combo.SelectedItem != null && combo.SelectedItem is ComboBoxExEntry)
			{
				ComboBoxExEntry entry = combo.SelectedItem as ComboBoxExEntry;

				return entry;
			}
			else if (!string.IsNullOrEmpty(combo.Text))
			{
				return new ComboBoxExEntry(combo.Text, combo.Text);
			}

			return null;
		}

		/// <summary>
		/// Get the selected filter value.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to retrieve value from</param>
		/// <returns>The filter value that is selected</returns>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public string GetSelectedFilter(Controls.ComboBoxEx combo)
		{
			ComboBoxExEntry entry = GetSelectedEntry(combo);

			if (entry != null)
			{
				return entry.Value;
			}

			return string.Empty;
		}

		/// <summary>
		/// Get all the user entered <see cref="ComboBoxExEntry"/>s.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to retrieve values from</param>
		/// <returns>All user entered <see cref="ComboBoxExEntry"/>s</returns>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public List<ComboBoxExEntry> GetUserValues(Controls.ComboBoxEx combo)
		{
			List<string> userValues = new List<string>();
			foreach (object item in combo.Items)
			{
				if (item is ComboBoxExEntry)
				{
					ComboBoxExEntry entry = item as ComboBoxExEntry;

					if (!entry.IsSystem)
					{
						if (!userValues.Contains(entry.Value))
						{
							userValues.Add(entry.Value);
						}
					}
				}
			}

			return userValues.Select(v => new ComboBoxExEntry(v, v)).ToList();
		}

		/// <summary>
		/// Get all the user entered <see cref="ComboBoxExEntry"/>s as a string.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to retrieve values from</param>
		/// <returns>All user entered <see cref="ComboBoxExEntry"/>s as a string</returns>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public string GetUserValuesAsString(Controls.ComboBoxEx combo)
		{
			return string.Join(Constants.SEARCH_ENTRIES_SEPARATOR, GetUserValues(combo).Select(v => v.Value));
		}

		/// <summary>
		/// Load all the user entered <see cref="ComboBoxExEntry"/>s.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to load values to</param>
		/// <param name="entries">All user entered <see cref="ComboBoxExEntry"/>s</param>
		/// <param name="appendSystem">True to append system entries, False to exclude them</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void LoadUserValues(Controls.ComboBoxEx combo, List<ComboBoxExEntry> entries, bool appendSystem = true)
		{
			if (appendSystem)
			{
				foreach (ComboBoxExEntry entry in systemEntries)
				{
					entries.Add(entry);
				}
			}

			combo.DisplayMember = "Display";
			combo.ValueMember = "Value";
			combo.DataSource = entries;
		}

		/// <summary>
		/// Load all the user entered <see cref="ComboBoxExEntry"/>s from the string representation.
		/// </summary>
		/// <param name="combo">The <see cref="Controls.ComboBoxEx"/> to load values to</param>
		/// <param name="userValues">All user entered <see cref="ComboBoxExEntry"/>s as a string representation</param>
		/// <param name="appendSystem">True to append system entries, False to exclude them</param>
		/// <history>
		/// [Curtis_Beard]    09/16/2019	Initial
		/// </history>
		public void LoadUserValuesFromString(Controls.ComboBoxEx combo, string userValues, bool appendSystem = true)
		{
			string[] values = Core.Convertors.GetComboBoxEntriesFromString(userValues);

			List<ComboBoxExEntry> entries = values.Select(s => new ComboBoxExEntry(s, s)).ToList();

			LoadUserValues(combo, entries, appendSystem);
		}
	}
}