using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AstroGrep.Core;
using libAstroGrep.Plugin;

namespace AstroGrep.Windows.Forms
{
	/// <summary>
	/// Plugins ordering/enabled selection screen.
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
	/// [Curtis_Beard]      09/10/2019	ADD: create new form for plugins
	/// </history>
	public partial class frmPlugins : BaseForm
	{
		/// <summary>
		/// Creates an instance of the <see cref="frmPlugins"/> class.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/10/2019	ADD: create new form for plugins
		/// </history>
		public frmPlugins()
		{
			InitializeComponent();

			API.ListViewExtensions.SetTheme(PluginsList);
		}

		/// <summary>
		/// Closes form.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		09/10/2019	Created
		/// </history>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Moves the selected plugin down in the list.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/10/2017	Pass new index to LoadPlugins
		/// [Curtis_Beard]		09/10/2019	Capture index before calling LoadPlugins
		/// </history>
		private void btnDown_Click(object sender, EventArgs e)
		{
			// move selected plugin down in list
			if (PluginsList.SelectedItems.Count > 0 && PluginsList.SelectedItems[0].Index != (PluginsList.Items.Count - 1))
			{
				int index = PluginsList.SelectedItems[0].Index;
				Core.PluginManager.Items.Reverse(index, 2);
				LoadPlugins(index + 1);
			}
		}

		/// <summary>
		/// Saves the changes and closes form.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		09/10/2019	Created
		/// </history>
		private void btnOK_Click(object sender, EventArgs e)
		{
			Core.PluginManager.Save();

			Close();
		}

		/// <summary>
		/// Moves the selected plugin up in the list.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/10/2017	Pass new index to LoadPlugins
		/// [Curtis_Beard]		09/10/2019	Capture index before calling LoadPlugins
		/// </history>
		private void btnUp_Click(object sender, EventArgs e)
		{
			// move selected plugin up in list
			if (PluginsList.SelectedItems.Count > 0 && PluginsList.SelectedItems[0].Index != 0)
			{
				int index = PluginsList.SelectedItems[0].Index;
				Core.PluginManager.Items.Reverse(index - 1, 2);
				LoadPlugins(index - 1);
			}
		}

		/// <summary>
		/// Clear the plugin details.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		09/05/2006	Created
		/// </history>
		private void ClearPluginDetails()
		{
			lblPluginName.Text = string.Empty;
			lblPluginVersion.Text = string.Empty;
			lblPluginAuthor.Text = string.Empty;
			lblPluginDescription.Text = string.Empty;
		}

		/// <summary>
		/// Handles setting the user specified options into the correct controls for display.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]      09/10/2019	ADD: create new form for plugins
		/// </history>
		private void frmPlugins_Load(object sender, EventArgs e)
		{
			LoadPlugins(0);

			//Language.GenerateXml(this, Application.StartupPath + "\\" + this.Name + ".xml");
			Language.ProcessForm(this);

			// set column text
			PluginsList.Columns[0].Text = Language.GetGenericText("PluginsColumnEnabled");
			PluginsList.Columns[1].Text = Language.GetGenericText("PluginsColumnName");
			PluginsList.Columns[2].Text = Language.GetGenericText("PluginsColumnExtensions");

			// set column widths
			PluginsList.Columns[0].Width = Constants.OPTIONS_PLUGINS_COLUMN_0_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100;
			PluginsList.Columns[1].Width = Constants.OPTIONS_PLUGINS_COLUMN_1_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100;
			PluginsList.Columns[2].Width = Constants.OPTIONS_PLUGINS_COLUMN_2_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100;
		}

		/// <summary>
		/// Display the plugin details.
		/// </summary>
		/// <param name="plugin">IAstroGrepPlugin to load</param>
		/// <history>
		/// [Curtis_Beard]		09/05/2006	Created
		/// </history>
		private void LoadPluginDetails(IAstroGrepPlugin plugin)
		{
			lblPluginName.Text = plugin.Name;
			lblPluginVersion.Text = plugin.Version;
			lblPluginAuthor.Text = plugin.Author;
			lblPluginDescription.Text = plugin.Description;
		}

		/// <summary>
		/// Load the plugins from the manager to the listview.
		/// </summary>
		/// <param name="selectedIndex">Set to index to show selected</param>
		/// <history>
		/// [Curtis_Beard]		07/28/2006	Created
		/// [Curtis_Beard]		05/10/2017	Allow selectedIndex parameter to be specified
		/// [Curtis_Beard]		09/10/2019	Apply selectedIndex after all items are available
		/// </history>
		private void LoadPlugins(int selectedIndex = -1)
		{
			PluginsList.Items.Clear();
			ListViewItem item;

			for (int i = 0; i < Core.PluginManager.Items.Count; i++)
			{
				item = new ListViewItem();
				item.Checked = Core.PluginManager.Items[i].Enabled;
				item.SubItems.Add(Core.PluginManager.Items[i].Plugin.Name);
				item.SubItems.Add(Core.PluginManager.Items[i].Plugin.Extensions);
				PluginsList.Items.Add(item);
			}

			if (selectedIndex > -1 && selectedIndex < PluginsList.Items.Count)
			{
				PluginsList.Items[selectedIndex].Selected = true;
			}
		}

		/// <summary>
		/// Enable or disable the selected plugin.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		09/05/2006	Created
		/// </history>
		private void PluginsList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if (e.Index > -1 && e.Index < PluginsList.Items.Count)
			{
				PluginsList.Items[e.Index].Selected = true;
				if (e.NewValue == CheckState.Checked)
					Core.PluginManager.Items[e.Index].Enabled = true;
				else
					Core.PluginManager.Items[e.Index].Enabled = false;
			}
		}

		/// <summary>
		/// Display the selected plugin details.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		09/05/2006	Created
		/// </history>
		private void PluginsList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (PluginsList.SelectedItems.Count > 0)
				LoadPluginDetails(Core.PluginManager.Items[PluginsList.SelectedItems[0].Index].Plugin);
			else
				ClearPluginDetails();
		}
	}
}