using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AstroGrep.Common;
using AstroGrep.Common.Logging;
using AstroGrep.Core;
using AstroGrep.Core.Theme;
using AstroGrep.Output;
using AstroGrep.Windows.Controls;
using libAstroGrep;
using libAstroGrep.EncodingDetection;

namespace AstroGrep.Windows.Forms
{
	/// <summary>
	/// Main Form
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
	/// [Theodore_Ward]     ??/??/????  Initial
	/// [Curtis_Beard]      01/11/2005  .Net Conversion/Comments/Option Strict
	/// [Curtis_Beard]      10/15/2005	CHG: Replace search procedures
	/// [Andrew_Radford]    17/08/2008	CHG: Moved Winforms designer stuff to a .designer file
	/// [Curtis_Beard]	    03/07/2012	ADD: 3131609, exclusions
	/// [Curtis_Beard]      09/26/2012	CHG: 3572487, move command line logic to program.cs and use property for value
	/// [Curtis_Beard]      01/16/2019	Use extension method to check for InvokeRequired instead of defining delegates
	/// [Curtis_Beard]		02/28/2020	CHG: .Net 4.5 cleanup
	/// </history>
	public partial class frmMain : BaseForm
	{
		private readonly LogItems LogItems = new LogItems();
		private readonly List<ICSharpCode.AvalonEdit.Document.TextAnchor> matchAnchors = new List<ICSharpCode.AvalonEdit.Document.TextAnchor>();
		private readonly RegistryMonitor registryMonitor;
		private readonly ContextMenuStrip TxtHitsContextMenuStrip = new ContextMenuStrip();
		private CommandLineProcessing.CommandLineArguments __CommandLineArgs = new CommandLineProcessing.CommandLineArguments();
		private List<FilterItem> __FilterItems = new List<FilterItem>();
		private Grep __Grep = null;
		private int __SortColumn = -1;
		private ComboBoxExHelper fileFilterHelper = null;
		private long StartingTime = 0;

		/// <summary>
		/// Creates an instance of the frmMain class.
		/// </summary>
		/// /// <history>
		/// [Theodore_Ward]     ??/??/????  Created
		/// [Curtis_Beard]      11/02/2006	CHG: Conversion to C#, setup event handlers
		/// [Curtis_Beard]      02/09/2012	FIX: 3486074, set modification date end to max value
		/// [Curtis_Beard]		02/24/2012	CHG: 3488321, ability to change results font
		/// [Curtis_Beard]		09/18/2013	ADD: 58, add drag/drop support for path
		/// [Curtis_Beard]		05/06/2015	CHG: remove events no longer used
		/// [Curtis_Beard]		05/14/2015	CHG: move event handlers to designer partial class
		/// [LinkNet]			04/24/2017	CHG: remove unused "ToolsMRUAll_Click" event handler
		/// [LinkNet]			04/24/2017	CHG: move enlarge label font code to Form Load Event
		/// </history>
		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// create file filter helper with entries
			CreateFileFilterHelper();

			API.ListViewExtensions.SetTheme(lstFileNames);

			// theme specific renderer
			ResultsToolStrip.Renderer = ThemeProvider.Theme.ToolStripRenderer;
			MainMenuStrip.Renderer = ThemeProvider.Theme.MenuRenderer;
			fileLstMnu.Renderer = ThemeProvider.Theme.MenuRenderer;
			TxtHitsContextMenuStrip.Renderer = ThemeProvider.Theme.MenuRenderer;

			// setup results display context menu actions
			TxtHitsContextMenuStrip.Opening += TxtHitsContextMenuStrip_Opening;
			txtHits.MouseRightButtonUp += TxtHits_MouseRightButtonUp;

			// create and setup the registry monitor to detect the Windows Theme setting
			registryMonitor = new RegistryMonitor(Registry.ThemePath)
			{
				RegChangeNotifyFilter = RegChangeNotifyFilter.Value
			};
			registryMonitor.RegChanged += RegistryMonitor_RegChanged;
			registryMonitor.Error += RegistryMonitor_Error;

			// turn off tool tips on tool strip but implement our own theme based tool tip instead
			ResultsToolStrip.ShowItemToolTips = false;
			foreach (ToolStripItem item in ResultsToolStrip.Items)
			{
				if (item is ToolStripButton || item is ToolStripComboBox)
				{
					item.MouseEnter += ResultsToolStripItem_MouseEnter;
					item.MouseLeave += ResultsToolStripItem_MouseLeave;
				}
			}
		}

		/// <summary>
		/// Gets/Sets the command line arguments
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		09/26/2012	Initial: 3572487
		/// </history>
		public CommandLineProcessing.CommandLineArguments CommandLineArgs
		{
			get { return __CommandLineArgs; }
			set { __CommandLineArgs = value; }
		}

		/// <summary>
		/// Allows Ctrl-F keyboard event to set focus to search text field.
		/// </summary>
		/// <param name="msg">system parameter for system message</param>
		/// <param name="keyData">system parameter for keys pressed</param>
		/// <returns>true if processed, false otherwise</returns>
		/// <history>
		/// [Curtis_Beard]	   10/27/2014	CHG: 87, set focus to search text field for ctrl-f
		/// [Curtis_Beard]	   05/11/2015	CHG: zoom for TextEditor control
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem and handle zooming key commands
		/// [Curtis_Beard]	   05/28/2015	CHG: remove zooming key commands since they are handled by ToolStripMenuItem
		/// </history>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.F))
			{
				cboSearchForText.Focus();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// Add an item to a combo box
		/// </summary>
		/// <param name="combo">Combo Box</param>
		/// <param name="item">Item to add</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   05/09/2007	CHG: check for a valid item
		/// [Ed_Jakubowski]	   05/26/2009	CHG: Added if Contains for testing combo item... this helps astrogrep run in mono 2.4
		/// </history>
		private static void AddComboSelection(ComboBox combo, string item)
		{
			if (item.Length > 0)
			{
				// If this path is already in the dropdown, remove it.
				if (combo.Items.Contains(item))
				{
					combo.Items.Remove(item);
				}

				// Add this path as the first item in the dropdown.
				combo.Items.Insert(0, item);

				// The combo text gets cleared by the AddItem.
				combo.Text = item;

				// Only store as many paths as has been set in options.
				//if (combo.Items.Count > Common.NUM_STORED_PATHS)
				if (combo.Items.Count > GeneralSettings.MaximumMRUPaths)
				{
					// Remove the last item in the list.
					combo.Items.RemoveAt(combo.Items.Count - 1);
				}
			}
		}

		/// <summary>
		/// Menu About Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]		??/??/????  Initial
		/// [Curtis_Beard]		01/11/2005	.Net Conversion
		/// [Curtis_Beard]		05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void AboutMenuItem_Click(object sender, EventArgs e)
		{
			var dlg = new frmAbout();
			dlg.ShowDialog(this);
		}

		/// <summary>
		/// Setup the context menu for the results area.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/10/2012  Initial: 3575509, show copy/select all context menu
		/// [Curtis_Beard]      04/08/2015  CHG: changes for CustomTextEditor
		/// </history>
		private void AddContextMenuForResults()
		{
			/*
			var menu = new System.Windows.Controls.ContextMenu();
			var item = new System.Windows.Controls.MenuItem();

			var bgMediaColor = System.Windows.Media.Color.FromArgb(ThemeProvider.Theme.Colors.BackColor.A, ThemeProvider.Theme.Colors.BackColor.R, ThemeProvider.Theme.Colors.BackColor.G, ThemeProvider.Theme.Colors.BackColor.B);
			var fgMediaColor = System.Windows.Media.Color.FromArgb(ThemeProvider.Theme.Colors.ForeColor.A, ThemeProvider.Theme.Colors.ForeColor.R, ThemeProvider.Theme.Colors.ForeColor.G, ThemeProvider.Theme.Colors.ForeColor.B);
			menu.Background = new System.Windows.Media.SolidColorBrush(bgMediaColor);
			menu.Foreground = new System.Windows.Media.SolidColorBrush(fgMediaColor);

			item.Click += openFile_Click;
			item.Header = Language.GetGenericText("ResultsContextMenu.OpenFileCurrentLine");
			item.IsEnabled = false;
			menu.Items.Add(item);

			menu.Items.Add(new System.Windows.Controls.Separator());

			item = new System.Windows.Controls.MenuItem();
			item.Click += copyItem_Click;
			item.Header = Language.GetGenericText("ResultsContextMenu.Copy");
			menu.Items.Add(item);

			item = new System.Windows.Controls.MenuItem
			{
				Header = Language.GetGenericText("ResultsContextMenu.SelectAll")
			};
			item.Click += selectAllItem_Click;
			menu.Items.Add(item);

			txtHits.ContextMenu = menu;
			txtHits.ContextMenuOpening += menu_ContextMenuOpening;
			*/

			// open current line in editor
			TxtHitsContextMenuStrip.Items.Clear();
			var stripItem = new ToolStripMenuItem(Language.GetGenericText("ResultsContextMenu.OpenFileCurrentLine"));
			stripItem.Image = Properties.Resources.OpenText;
			stripItem.Click += openFile_Click;
			TxtHitsContextMenuStrip.Items.Add(stripItem);

			TxtHitsContextMenuStrip.Items.Add(new ToolStripSeparator());

			// copy
			stripItem = new ToolStripMenuItem(Language.GetGenericText("ResultsContextMenu.Copy"));
			stripItem.Image = Properties.Resources.page_copy;
			stripItem.Click += copyItem_Click;
			TxtHitsContextMenuStrip.Items.Add(stripItem);

			// select all
			stripItem = new ToolStripMenuItem(Language.GetGenericText("ResultsContextMenu.SelectAll"));
			stripItem.Image = Properties.Resources.Select;
			stripItem.Click += selectAllItem_Click;
			TxtHitsContextMenuStrip.Items.Add(stripItem);
		}

		/// <summary>
		/// Add a file hit to the listview (Thread safe).
		/// </summary>
		/// <param name="file">File to add</param>
		/// <param name="index">Position in GrepCollection</param>
		/// <history>
		/// [Curtis_Beard]		10/17/2005	Created
		/// [Curtis_Beard]		12/02/2005	CHG: Add the count column
		/// [Curtis_Beard]		07/07/2006	CHG: Make thread safe
		/// [Curtis_Beard]		09/14/2006	CHG: Update to use date's ToString method
		/// [Curtis_Beard]		02/17/2012	CHG: update listview sorting
		/// [Curtis_Beard]		03/06/2012	ADD: listview image for file type
		/// [Curtis_Beard]		10/27/2014	CHG: 88, add file extension column
		/// [Curtis_Beard]		11/10/2014	FIX: 59, check for duplicate entries
		/// </history>
		private void AddHitToList(FileInfo file, int index)
		{
			lstFileNames.InvokeIfRequired(() =>
			{
				// don't add if it already exists
				foreach (ListViewItem item in lstFileNames.Items)
				{
					MatchResult hit = __Grep.RetrieveMatchResult(int.Parse(item.SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
					if (hit.File.FullName.Equals(file.FullName, StringComparison.InvariantCultureIgnoreCase))
					{
						return;
					}
				}

				// Create the list item
				var listItem = new ListViewItem(file.Name)
				{
					Name = index.ToString(),
					ImageIndex = ListViewImageManager.GetImageIndex(file, ListViewImageList)
				};
				listItem.SubItems.Add(file.DirectoryName);
				listItem.SubItems.Add(file.Extension);
				listItem.SubItems.Add(file.LastWriteTime.ToString());

				// add explorer style of file size for display but store file size in bytes for comparison
				ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(listItem, API.StrFormatByteSize(file.Length))
				{
					Tag = file.Length
				};
				listItem.SubItems.Add(subItem);

				listItem.SubItems.Add("0");

				// must be last
				listItem.SubItems.Add(index.ToString());

				// Add list item to listview
				lstFileNames.Items.Add(listItem);

				// clear it out
				listItem = null;
			});
		}

		/// <summary>
		/// View all messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/11/2014	Initial
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void AllMessageMenuItem_Click(object sender, System.EventArgs e)
		{
			DisplaySearchMessages(null);
		}

		/// <summary>
		/// View all results.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	ADD: AvalonEdit view menu options
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void AllResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			foreach (ListViewItem item in lstFileNames.Items)
			{
				item.Selected = false;
			}

			ProcessAllMatchesForDisplay();
		}

		/// <summary>
		/// Determines if any log items are present.
		/// </summary>
		/// <returns>true if log items are present, false otherwise</returns>
		/// <history>
		/// [Curtis_Beard]	   ??/??/????	Initial
		/// </history>
		private bool AnyLogItems()
		{
			return (LogItems != null ? LogItems.Count > 0 : false);
		}

		/// <summary>
		/// Open Browser for Folder dialog
		/// </summary>
		/// <param name="shift">True if shift was applied, False otherwise</param>
		/// <history>
		/// [Curtis_Beard]		10/13/2005	ADD: Initial
		/// [Curtis_Beard]		10/02/2006	FIX: Clear ComboBox when only Browse... is present
		/// [Curtis_Beard]		11/22/2006	CHG: Remove use of browse in combobox
		/// [Justin_Dearing]	05/06/2007	CHG: Dialog defaults to the folder selected in the combobox.
		/// [Curtis_Beard]		05/23/2007	CHG: Remove cancel highlight
		/// [Curtis_Beard]		05/06/2014	CHG: 76, use vista based folder selection (downgrades to normal FolderBrowserDialog)
		/// [Curtis_Beard]		01/10/2019	CHG: 125, allow Shift to append to path
		/// </history>
		private void BrowseForFolder(bool shift)
		{
			VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
			if (!API.IsWindowsVistaOrLater)
			{
				dlg.Description = Language.GetGenericText("OpenFolderDescription");
				dlg.ShowNewFolderButton = false;
			}

			// set initial directory if valid
			if (System.IO.Directory.Exists(cboFilePath.Text))
			{
				dlg.SelectedPath = cboFilePath.Text;
			}

			// display dialog and setup path if selected
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				var newPath = dlg.SelectedPath;
				if (shift)
				{
					var currentPath = cboFilePath.Text.Trim();
					if (!string.IsNullOrWhiteSpace(currentPath))
					{
						newPath = string.Format("{0}|{1}", currentPath, newPath);
					}
				}
				AddComboSelection(cboFilePath, newPath);
			}
		}

		/// <summary>
		/// Cancel Button Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// </history>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if (__Grep != null)
				__Grep.Abort();
		}

		/// <summary>
		/// Search Button Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]       ??/??/????  Initial
		/// [Curtis_Beard]	    01/11/2005	.Net Conversion
		/// [Curtis_Beard]	    10/30/2012	ADD: 28, search within results
		/// [Curtis_Beard]	    10/27/2015	FIX: 78, adjust search in results
		/// </history>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (!VerifyInterface())
				return;

			StartSearch(chkSearchInResults.Checked);
		}

		/// <summary>
		/// Calculate and set the total number of hits for the current search. (Thread Safe)
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		01/27/2007	Created
		/// [Curtis_Beard]		07/02/2007	ADD: update file/error total counts
		/// [Curtis_Beard]		01/08/2019	CHG: 119, add line hit count
		/// </history>
		private void CalculateTotalCount()
		{
			lstFileNames.InvokeIfRequired(() =>
			{
				//update total hit count
				int total = 0;
				int lineTotal = 0;

				foreach (ListViewItem item in lstFileNames.Items)
				{
					total += Convertors.GetHitCountFromCountDisplay(item.SubItems[Constants.COLUMN_INDEX_COUNT].Text);
					lineTotal += Convertors.GetLineCountFromCountDisplay(item.SubItems[Constants.COLUMN_INDEX_COUNT].Text);
				}

				SetStatusBarTotalCount(total, lineTotal);
				SetStatusBarFileCount(lstFileNames.Items.Count);
				SetStatusBarFilterCount(GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion));
				SetStatusBarErrorCount(GetLogItemsCountByType(LogItem.LogItemTypes.Error));
			});
		}

		/// <summary>
		/// Resize drop down list if necessary
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]    11/21/2005	Created
		/// [Curtis_Beard]    03/05/2020	CHG: set a smaller max width
		/// </history>
		private void cboFileName_DropDown(object sender, EventArgs e)
		{
			cboFileName.DropDownWidth = Convertors.CalculateDropDownWidth(cboFileName, 300);
		}

		/// <summary>
		/// Handles the DragDrop event for the path combo box.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <remarks>
		/// Handles file drops only by putting the directory into the combo box.
		/// </remarks>
		/// <history>
		/// [Curtis_Beard]	   09/18/2013	ADD: 58, add drag/drop support for path
		/// </history>
		private void cboFilePath_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files != null && files.Length > 0)
			{
				string path1 = files[0];
				if (Directory.Exists(path1))
				{
					AddComboSelection(cboFilePath, path1);
				}
				else if (File.Exists(path1))
				{
					FileInfo file = new FileInfo(path1);
					AddComboSelection(cboFilePath, file.DirectoryName);
				}
			}
		}

		/// <summary>
		/// Handles the DragEnter event for the path combo box.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <remarks>
		/// Handles file drops only by putting the directory into the combo box.
		/// </remarks>
		/// <history>
		/// [Curtis_Beard]	   09/18/2013	ADD: 58, add drag/drop support for path
		/// </history>
		private void cboFilePath_DragEnter(object sender, DragEventArgs e)
		{
			if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
		}

		/// <summary>
		/// Resize drop down list if necessary
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]    11/21/2005	Created
		/// </history>
		private void cboFilePath_DropDown(object sender, EventArgs e)
		{
			cboFilePath.DropDownWidth = Convertors.CalculateDropDownWidth(cboFilePath);
		}

		/// <summary>
		/// Resize drop down list if necessary
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]    11/21/2005	Created
		/// </history>
		private void cboSearchForText_DropDown(object sender, EventArgs e)
		{
			cboSearchForText.DropDownWidth = Convertors.CalculateDropDownWidth(cboSearchForText);
		}

		/// <summary>
		/// Displays update check window to user.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/06/2014	Initial
		/// [Curtis_Beard]		05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void CheckForUpdateMenuItem_Click(object sender, EventArgs e)
		{
			var dlg = new frmCheckForUpdateTemp();
			dlg.ShowDialog(this);
		}

		/// <summary>
		/// Checks whether to show all results after a search (thread safe).
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		04/08/2015  CHG: 54, add check to show all the results after a search
		/// </history>
		private void CheckShowAllResults()
		{
			chkAllResultsAfterSearch.InvokeIfRequired(() =>
			{
				if (chkAllResultsAfterSearch.Checked)
				{
					ProcessAllMatchesForDisplay();
				}
			});
		}

		/// <summary>
		/// Negation check event
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]	   01/28/2005	Created
		/// [Curtis_Beard]	   06/13/2005	CHG: Gray out file names only when checked
		/// </history>
		private void chkNegation_CheckedChanged(object sender, EventArgs e)
		{
			chkFileNamesOnly.Checked = chkNegation.Checked;
			chkFileNamesOnly.Enabled = !chkNegation.Checked;
		}

		/// <summary>
		/// Clears the file list (Thread safe).
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		07/10/2006	Created
		/// </history>
		private void ClearItems()
		{
			lstFileNames.InvokeIfRequired(() =>
			{
				lstFileNames.Items.Clear();
			});
		}

		/// <summary>
		/// Clear MRU Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   11/22/2006	CHG: Remove use of browse in combobox
		/// </history>
		private void ClearMRUAllMenuItem_Click(object sender, System.EventArgs e)
		{
			cboFilePath.Items.Clear();
			fileFilterHelper.ClearUserValues(cboFileName);
			cboSearchForText.Items.Clear();

			GeneralSettings.SearchStarts = string.Empty;
			GeneralSettings.SearchFilters = string.Empty;
			GeneralSettings.SearchTexts = string.Empty;
			GeneralSettings.Save();
		}

		/// <summary>
		/// Clear Search Paths MRU event.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/07/2014	ADD: clear each MRU list individually
		/// </history>
		private void ClearMRUPathsMenuItem_Click(object sender, EventArgs e)
		{
			cboFilePath.Items.Clear();

			GeneralSettings.SearchStarts = string.Empty;
			GeneralSettings.Save();
		}

		/// <summary>
		/// Clear Search Text MRU event.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/07/2014	ADD: clear each MRU list individually
		/// </history>
		private void ClearMRUTextsMenuItem_Click(object sender, EventArgs e)
		{
			cboSearchForText.Items.Clear();

			GeneralSettings.SearchTexts = string.Empty;
			GeneralSettings.Save();
		}

		/// <summary>
		/// Clear File Types MRU event.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/07/2014	ADD: clear each MRU list individually
		/// </history>
		private void ClearMRUTypesMenuItem_Click(object sender, EventArgs e)
		{
			fileFilterHelper.ClearUserValues(cboFileName);

			GeneralSettings.SearchFilters = string.Empty;
			GeneralSettings.Save();
		}

		/// <summary>
		/// Handles setting clipboard data with currently selected text from results preview area.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		04/08/2015	Initial
		/// </history>
		private void copyItem_Click(object sender, EventArgs e)
		{
			txtHits.Copy();
		}

		/// <summary>
		/// Sends all selected items from the file list to the clipboard
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]        01/31/2012  ADD: 2078252, add right click options for copying name,located in,located in + name
		/// </history>
		private void CopyLocatedInAndNameMenuItem_Click(object sender, System.EventArgs e)
		{
			if (lstFileNames.SelectedItems.Count <= 0)
				return;

			System.Text.StringBuilder data = new System.Text.StringBuilder();
			try
			{
				foreach (ListViewItem lvi in lstFileNames.SelectedItems)
				{
					data.AppendFormat("{0}{1}{2}", lvi.SubItems[1].Text, Path.DirectorySeparatorChar.ToString(), lvi.Text);
					data.Append(Environment.NewLine);
				}
				Clipboard.SetDataObject(data.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Sends all selected items from the file list to the clipboard
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]        01/31/2012  ADD: 2078252, add right click options for copying name,located in,located in + name
		/// </history>
		private void CopyLocatedInMenuItem_Click(object sender, System.EventArgs e)
		{
			if (lstFileNames.SelectedItems.Count <= 0)
				return;

			System.Text.StringBuilder data = new System.Text.StringBuilder();
			try
			{
				foreach (ListViewItem lvi in lstFileNames.SelectedItems)
				{
					data.Append(lvi.SubItems[1].Text);
					data.Append(Environment.NewLine);
				}
				Clipboard.SetDataObject(data.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Sends all selected items from the file list to the clipboard
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Ed_Jakbuowski]       05/20/2009  Created
		/// [Curtis_Beard]        01/31/2012  FIX: 3482207, show all columns when copying data
		/// [Curtis_Beard]        08/25/2018  FIX: 108, use culture ListSeparator instead of comma
		/// </history>
		private void CopyMenuItem_Click(object sender, System.EventArgs e)
		{
			if (lstFileNames.SelectedItems.Count <= 0)
				return;

			System.Text.StringBuilder data = new System.Text.StringBuilder();
			try
			{
				foreach (ListViewItem lvi in lstFileNames.SelectedItems)
				{
					data.Append(lvi.Text);

					// skip first and last columns (filename, sort order)
					for (int i = 0; i < lvi.SubItems.Count; i++)
					{
						if (i != 0 && i != lvi.SubItems.Count - 1)
						{
							var subLvi = lvi.SubItems[i];
							data.AppendFormat("{0} ", System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
							data.Append(subLvi.Text);
						}
					}
					data.Append(Environment.NewLine);
				}
				Clipboard.SetDataObject(data.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Sends all selected items from the file list to the clipboard
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]        01/31/2012  ADD: 2078252, add right click options for copying name,located in,located in + name
		/// </history>
		private void CopyNameMenuItem_Click(object sender, System.EventArgs e)
		{
			if (lstFileNames.SelectedItems.Count <= 0)
				return;

			System.Text.StringBuilder data = new System.Text.StringBuilder();
			try
			{
				foreach (ListViewItem lvi in lstFileNames.SelectedItems)
				{
					data.Append(lvi.Text);
					data.Append(Environment.NewLine);
				}
				Clipboard.SetDataObject(data.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Show tooltip for a ToolStripStatusLabel.
		/// </summary>
		/// <param name="sender">Current ToolStripStatusLabel</param>
		/// <param name="e">System parameter</param>
		/// <history>
		/// [Curtis_Beard]	   08/16/2016	FIX: 88, manually show tooltip to avoid being shown over control
		/// </history>
		private void CountPanel_MouseEnter(object sender, EventArgs e)
		{
			var label = (ToolStripStatusLabel)sender;
			label.ToolTipText = "";
			var newPoint = PointToClient(Cursor.Position);
			toolTip1.Show(Language.GetControlToolTipText(label), this, newPoint.X, newPoint.Y);
		}

		/// <summary>
		/// Hide tooltip for a ToolStripStatusLabel.
		/// </summary>
		/// <param name="sender">Current ToolStripStatusLabel</param>
		/// <param name="e">System parameter</param>
		/// <history>
		/// [Curtis_Beard]	   08/16/2016	FIX: 88, manually show tooltip to avoid being shown over control
		/// </history>
		private void CountPanel_MouseLeave(object sender, EventArgs e)
		{
			toolTip1.Hide(this);
		}

		/// <summary>
		/// Creates the file filter system entries and <see cref="ComboBoxExHelper"/>.
		/// </summary>
		private void CreateFileFilterHelper()
		{
			List<ComboBoxExEntry> filterSystemEntries = new List<ComboBoxExEntry>
			{
				// Separator
				new ComboBoxExEntry(ComboBoxEx.Separator, "", true),

				// All
				new ComboBoxExEntry(Language.GetGenericText("All", "All"), "*.*", true),

				// Documents
				new ComboBoxExEntry(Language.GetGenericText("Documents", "Documents"), "*.doc,*.docx,*.xls,*.xlsx,*.pdf,*.txt", true),

				// Code
				new ComboBoxExEntry(Language.GetGenericText("Code", "Code"), "*.cs,*.vb,*.c,*.cpp,*.h,*.java,*.py,*.cls,*.jsp,*.asm,*.bas,*.asp,*.htm,*.html,*.sql", true),

				// Media
				new ComboBoxExEntry(Language.GetGenericText("Media", "Media"), Plugins.MediaTag.MediaTagsPlugin.GetExtensionsFilter(), true)
			};

			// initialize the class
			fileFilterHelper = new ComboBoxExHelper(filterSystemEntries);
		}

		/// <summary>
		/// Context Menu Item for deleting items from the list.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// /// <history>
		/// [Ed_Jakbuowski]     05/20/2009  Created
		/// [Curtis_Beard]      09/17/2013  FIX: 40, update counts after a removal from the list
		/// [Curtis_Beard]      12/02/2014  ADD: begin/end update calls when changing file listview
		/// </history>
		private void DeleteMenuItem_Click(object sender, System.EventArgs e)
		{
			if (lstFileNames.SelectedItems.Count <= 0)
				return;

			try
			{
				lstFileNames.BeginUpdate();
				while (lstFileNames.SelectedItems.Count > 0)
				{
					lstFileNames.SelectedItems[0].Remove();
				}
				lstFileNames.EndUpdate();

				// update counts
				CalculateTotalCount();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Used to display search messages (like exclusions, errors) to the user.
		/// </summary>
		/// <param name="displayType">The type of message to display</param>
		/// <history>
		/// [Curtis_Beard]	   09/27/2012	ADD: 1741735, better error handling display
		/// [Curtis_Beard]	   12/06/2012	CHG: 1741735, rework to use common LogItems
		/// [Curtis_Beard]	   11/11/2014	CHG: use new log items viewer form
		/// [Curtis_Beard]	   03/03/2015	CHG: 93, used saved window position if enabled, add bounds check
		/// [Curtis_Beard]	   08/15/2016	FIX: 92, use a copy of the log items so we don't cause a write/read conflict
		/// </history>
		private void DisplaySearchMessages(LogItem.LogItemTypes? displayType)
		{
			this.InvokeIfRequired(() =>
			{
				if ((!displayType.HasValue && AnyLogItems()) || (displayType.HasValue && GetLogItemsCountByType(displayType.Value) > 0))
				{
					using (var frm = new frmLogDisplay())
					{
						frm.LogItems = LogItems.Clone();

						frm.DefaultFilterType = displayType;
						frm.StartPosition = FormStartPosition.Manual;

						Rectangle defaultBounds = new Rectangle(Left + 20, Bottom - frm.Height - stbStatus.Height - 20, Width - 40, frm.Height);

						int width = GeneralSettings.LogDisplaySavePosition && GeneralSettings.LogDisplayWidth != -1 ? GeneralSettings.LogDisplayWidth : defaultBounds.Width;
						int height = GeneralSettings.LogDisplaySavePosition && GeneralSettings.LogDisplayHeight != -1 ? GeneralSettings.LogDisplayHeight : defaultBounds.Height;
						int left = GeneralSettings.LogDisplaySavePosition && GeneralSettings.LogDisplayLeft != -1 ? GeneralSettings.LogDisplayLeft : defaultBounds.X;
						int top = GeneralSettings.LogDisplaySavePosition && GeneralSettings.LogDisplayTop != -1 ? GeneralSettings.LogDisplayTop : defaultBounds.Y;

						frm.Bounds = new Rectangle(left, top, width, height);

						// form can't find a screen to fit on, so reset to default on primary screen
						if (!Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(frm.Bounds)))
						{
							frm.Bounds = defaultBounds;
						}

						frm.ShowDialog(this);
					}
				}
			});
		}

		/// <summary>
		/// Handle donate menu item selection.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      09/18/2013  CHG: 113, add donation link
		/// </history>
		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(ProductInformation.DonationUrl);
		}

		/// <summary>
		/// Enables edit menu items when necessary.
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]      11/03/2005	Created
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void EditMenu_DropDownOpening(object sender, EventArgs e)
		{
			SelectAllMenuItem.Enabled = OpenSelectedMenuItem.Enabled = lstFileNames.Items.Count > 0;
		}

		/// <summary>
		/// Option to view entire file contents.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	ADD: AvalonEdit view menu options
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void EntireFileMenuItem_Click(object sender, System.EventArgs e)
		{
			EntireFileMenuItem.Checked = !EntireFileMenuItem.Checked;
			ResultsViewEntireFileButton.Checked = EntireFileMenuItem.Checked;

			lstFileNames_SelectedIndexChanged(null, null);
		}

		/// <summary>
		/// View error messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/11/2014	Initial
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ErrorMessageMenuItem_Click(object sender, System.EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Error);
		}

		/// <summary>
		/// View exclusion messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/11/2014	Initial
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ExclusionMessageMenuItem_Click(object sender, System.EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Exclusion);
		}

		/// <summary>
		/// Closes all the open AstroGrep windows (including this one).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	   09/12/2019	CHG: 138, add exit all menu item
		/// </history>
		private void ExitAllMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// find all AstroGrep instances
				var ourProcesses = Process.GetProcesses().Where(p => p.ProcessName.StartsWith(AstroGrep.Common.ProductInformation.ApplicationName, StringComparison.OrdinalIgnoreCase));
				int thisProcessId = Process.GetCurrentProcess().Id;

				foreach (Process process in ourProcesses)
				{
					// don't kill the current process
					if (process.Id != thisProcessId)
					{
						process.Kill();
					}
				}
			}
			catch (Exception ex)
			{
				LogClient.Instance.Logger.Error(ex, "Unable to close all the open AstroGrep processes");
			}

			// Close the current window
			Close();
		}

		/// <summary>
		/// Closes the program.
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ExitMenuItem_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Handles copying the selected files to the clipboard.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      09/18/2013  ADD: 65, file operations context menu
		/// </history>
		private void FileCopyMenuItem_Click(object sender, EventArgs e)
		{
			System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();

			for (int i = 0; i < lstFileNames.SelectedItems.Count; i++)
			{
				// retrieve hit object
				var hit = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));

				// retrieve the filename
				files.Add(hit.File.FullName);
			}

			if (files.Count > 0)
			{
				Clipboard.SetFileDropList(files);
			}
		}

		/// <summary>
		/// Handles deleting the selected files by using the recycle bin.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      09/18/2013  ADD: 65, file operations context menu
		/// [Curtis_Beard]      12/02/2014  ADD: begin/end update calls when changing file listview
		/// </history>
		private void FileDeleteMenuItem_Click(object sender, EventArgs e)
		{
			Dictionary<string, string> files = new Dictionary<string, string>();

			for (int i = 0; i < lstFileNames.SelectedItems.Count; i++)
			{
				// retrieve hit object
				var hit = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
				var index = lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text;

				files.Add(index, hit.File.FullName);
			}

			lstFileNames.BeginUpdate();
			foreach (var file in files)
			{
				// recycle bin delete
				API.FileDeletion.Delete(file.Value);

				// remove from list
				ListViewItem[] items = lstFileNames.Items.Find(file.Key, false);
				if (items != null && items.Length == 1)
				{
					items[0].Remove();
				}
			}
			lstFileNames.EndUpdate();

			// clear results area since all selected files are removed
			if (files.Count > 0)
			{
				txtHits.Clear();
				CalculateTotalCount();
			}
		}

		/// <summary>
		/// Enables file menu items when necessary.
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]      11/03/2005	Created
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void FileMenu_DropDownOpening(object sender, EventArgs e)
		{
			SaveResultsMenuItem.Enabled = PrintResultsMenuItem.Enabled = lstFileNames.Items.Count > 0;
		}

		/// <summary>
		/// Closed Event - Save settings and exit
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   10/11/2006	CHG: Use common function to remove Browse..., call SaveSettings
		/// [Curtis_Beard]	   11/22/2006	CHG: Remove use of browse in combobox
		/// [Curtis_Beard]	   10/16/2012	CHG: Save search settings on exit
		/// [Curtis_Beard]	   05/15/2015	CHG: add exiting log entry
		/// [Curtis_Beard]	   05/28/2015	CHG: update exit log entry to stopping
		/// [Curtis_Beard]	  06/02/2015	CHG: add portable to exit log message if enabled
		/// </history>
		private void frmMain_Closed(object sender, EventArgs e)
		{
			SaveSettings();

			if (GeneralSettings.SaveSearchOptionsOnExit)
			{
				SaveSearchSettings();
			}

			if (textElementHost != null)
			{
				textElementHost.Dispose();
			}

			try
			{
				registryMonitor.Stop();
				registryMonitor.Dispose();
			}
			catch (Exception ex)
			{
				LogClient.Instance.Logger.Warn(ex, $"Issue stopping registry monitor for theme detection: {ex.Message}");
			}

			LogClient.Instance.Logger.Info("### STOPPING {0}, version {1}{2} ###",
			   ProductInformation.ApplicationName,
			   ProductInformation.ApplicationVersion.ToString(3),
			   ProductInformation.IsPortable ? " (Portable)" : string.Empty);

			Application.Exit();
		}

		/// <summary>
		/// Form Load Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]		??/??/????	Initial
		/// [Curtis_Beard]		01/11/2005	.Net Conversion
		/// [Curtis_Beard]		04/12/2005	ADD: Command line additions
		/// [Son_Le]			08/08/2005	CHG: save listview header widths
		/// [Curtis_Beard]		10/15/2005	CHG: validate command line parameter as valid dir
		/// [Curtis_Beard]		07/12/2006	CHG: allow drives for a valid command line parameter
		/// [Curtis_Beard]		07/25/2006	CHG: Moved cmd line processing to ProcessCommandLine routine
		/// [Curtis_Beard]		10/10/2006	CHG: Remove call to load search settings, perform check only.
		/// [Curtis_Beard]		10/11/2007	ADD: convert language value if necessary
		/// [Ed_Jakubowski]		10/29/2009	CHG: Fix for Startup Path when using Mono 2.4
		/// [Curtis_Beard]		01/30/2012	CHG: 1955653, set focus to search text on startup
		/// [Curtis_Beard]	    02/24/2012	CHG: 3489693, save state of search options
		/// [Curtis_Beard]		09/26/2012	CHG: 3572487, remove init of command line options (now in program.cs)
		/// [LinkNet]			04/30/2017	ADD: Load Windows DPI settings
		/// [Curtis_Beard]		05/19/2017	CHG: 120, add option to use accent color
		/// [Curtis_Beard]		08/20/2019  CHG: 142, dynamically display context lines
		/// [Curtis_Beard]		08/20/2019  CHG: application color for splitters and use system color for default instead of black
		/// [Curtis_Beard]		08/27/2022  Add theme support
		/// </history>
		private void frmMain_Load(object sender, System.EventArgs e)
		{
			// Load Windows DPI settings and set window and column positions and fonts to defaults if setting has changed
			LoadDPISettings();

			// set defaults
			ResultsContextLinesBeforeCombo.Items.Clear();
			ResultsContextLinesAfterCombo.Items.Clear();
			for (int i = 0; i <= Constants.MAX_CONTEXT_LINES; i++)
			{
				ResultsContextLinesBeforeCombo.Items.Add(i.ToString());
				ResultsContextLinesAfterCombo.Items.Add(i.ToString());
			}

			// Load language
			//Language.GenerateXml(this, Application.StartupPath + "\\" + this.Name + ".xml");
			Language.ProcessForm(this, toolTip1);

			// Load the general settings
			Legacy.ConvertGeneralSettings();

			LoadSettings();

			// Load the search settings
			Legacy.ConvertSearchSettings();
			_ = SearchSettings.ContextLinesBefore;
			LoadSearchSettings();

			// Make sure in Mono to set the command-line path
			if (CommandLineArgs.AnyArguments && CommandLineArgs.IsValidStartPath)
			{
				cboFilePath.Text = CommandLineArgs.StartPath;
			}

			// Delete registry entry (if exist)
			Legacy.DeleteRegistry();

			// Load plug-ins
			PluginManager.Load();

			// set view state of controls
			LoadLinkLabelStates();

			// Handle any command line arguments
			ProcessCommandLine();

			// set focus to search text combo-box
			cboSearchForText.Select();

			// Enlarge font without changing font family
			lblSearchHeading.Font = new Font(lblSearchHeading.Font.FontFamily, 12F);
			lblSearchOptions.Font = new Font(lblSearchOptions.Font.FontFamily, 12F);

			// Fonts
			MainMenu.Font = Font;
			stbStatus.Font = Font;

			// reload the theme based on current setting
			if (Enum.TryParse(GeneralSettings.ThemeType.ToString(), out ThemeProvider.ThemeType themeType))
			{
				ReloadTheme(themeType);
			}
		}

		/// <summary>
		/// First time shown event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	  09/16/2019	FIX: set file name to no selection and focus to search text
		/// </history>
		private void frmMain_Shown(object sender, EventArgs e)
		{
			cboFileName.SelectionLength = 0;

			cboSearchForText.Select();
		}

		/// <summary>
		/// Generates a list of <see cref="ICSharpCode.AvalonEdit.Document.TextAnchor"/> for the given <see cref="MatchResult"/>.
		/// </summary>
		/// <param name="match">The current <see cref="MatchResult"/></param>
		/// <param name="showingFullFile">Determines if the entire file is displayed or just the match lines</param>
		/// <param name="removeWhiteSpace">Determines if the leading white space is removed or not</param>
		/// <param name="beforeContextLines">The number of before context lines</param>
		/// <param name="afterContextLines">The number of after context lines</param>
		/// <returns>A list of <see cref="ICSharpCode.AvalonEdit.Document.TextAnchor"/></returns>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019  ADD: 135, support prev/next navigation of matches
		/// </history>
		private List<ICSharpCode.AvalonEdit.Document.TextAnchor> GetAnchorsForMatch(MatchResult match, bool showingFullFile, bool removeWhiteSpace, int beforeContextLines, int afterContextLines)
		{
			List<ICSharpCode.AvalonEdit.Document.TextAnchor> anchors = new List<ICSharpCode.AvalonEdit.Document.TextAnchor>();

			if (match != null)
			{
				var matches = match.GetDisplayMatches(beforeContextLines, afterContextLines);

				if (matches == null || matches.Count == 0)
					return anchors;

				for (int i = 0; i < txtHits.Document.Lines.Count; i++)
				{
					var line = txtHits.Document.Lines[i];
					string lineText = txtHits.Document.GetText(txtHits.Document.Lines[i].Offset, txtHits.Document.Lines[i].Length);
					int lineStartOffset = line.Offset;
					int lineNumber = line.LineNumber; // 1 based

					// lines in grep are 0 based array
					MatchResultLine matchLine = null;
					if (showingFullFile)
					{
						matchLine = (from m in matches where m.LineNumber == lineNumber select m).FirstOrDefault();
					}
					else
					{
						matchLine = lineNumber - 1 < matches.Count ? matches[lineNumber - 1] : null;
					}

					string contents = matchLine != null ? matchLine.Line : string.Empty;
					bool isHit = matchLine != null ? matchLine.HasMatch : false;

					if (isHit && !string.IsNullOrEmpty(contents))
					{
						int trimOffset = 0;

						if (removeWhiteSpace)
						{
							if (matchLine.HasMatch)
							{
								trimOffset = Utils.GetValidLeadingSpaces(contents, matchLine.Matches[0].StartPosition);
							}
							else
							{
								trimOffset = contents.Length - contents.TrimStart().Length;
							}
						}

						for (int j = 0; j < matchLine.Matches.Count; j++)
						{
							int startPosition = matchLine.Matches[j].StartPosition;
							int startOffset = lineStartOffset + (startPosition - trimOffset);

							// create anchor to support prev/next navigation
							anchors.Add(txtHits.Document.CreateAnchor(startOffset));
						}
					}
				}
			}

			return anchors;
		}

		/// <summary>
		/// Get TextEditorOpener for give position.
		/// </summary>
		/// <param name="position">Current TextViewPosition from results preview area</param>
		/// <returns>TextEditorOpener for current position</returns>
		/// <history>
		/// [Curtis_Beard]		06/29/2015	CHG: reconfigure to use common method
		/// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines support
		/// </history>
		private TextEditorOpener GetEditorAtLocation(ICSharpCode.AvalonEdit.TextViewPosition? position)
		{
			var opener = new TextEditorOpener();

			if (position.HasValue)
			{
				try
				{
					string path = string.Empty;
					int lineNumber = position.Value.Line;
					int columnNumber = 1;

					if (txtHits.LineNumbers != null && (lstFileNames.SelectedItems.Count == 0 || !EntireFileMenuItem.Checked))
					{
						// either all results or file's matches
						var lineNumberHolder = txtHits.LineNumbers[position.Value.Line - 1];

						path = lineNumberHolder.FileFullName;

						if (!string.IsNullOrEmpty(path))
						{
							string line = string.Empty;
							lineNumber = lineNumberHolder.Number > -1 ? lineNumberHolder.Number : 1;
							columnNumber = lineNumberHolder.ColumnNumber;

							var fileMatch = (from m in __Grep.MatchResults where m.File.FullName.Equals(path) select m).FirstOrDefault();
							foreach (var matchLine in fileMatch.Matches)
							{
								if (matchLine.LineNumber == lineNumber)
								{
									line = matchLine.OriginalLine;
									break;
								}
							}

							opener = new TextEditorOpener(path, lineNumber, columnNumber, line, __Grep.SearchSpec.SearchText);
						}
					}
					else if (lstFileNames.SelectedItems.Count > 0 && __Grep != null)
					{
						// full file
						MatchResult result = __Grep.RetrieveMatchResult(Convert.ToInt32(lstFileNames.SelectedItems[0].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
						string line = string.Empty;
						path = result.File.FullName;
						MatchResultLine matchLine = (from m in result.Matches where m.LineNumber == position.Value.Line select m).FirstOrDefault();
						if (matchLine != null && matchLine.LineNumber > -1)
						{
							line = matchLine.OriginalLine;
							lineNumber = matchLine.LineNumber;
							columnNumber = matchLine.ColumnNumber;
						}

						opener = new TextEditorOpener(path, lineNumber, columnNumber, line, __Grep.SearchSpec.SearchText);
					}
				}
				catch { }
			}

			return opener;
		}

		/// <summary>
		/// Gets a list of FilterItems that are the given FilterType.
		/// </summary>
		/// <param name="ft">Desired FilterType</param>
		/// <returns>List of FilterItems</returns>
		/// <history>
		/// [Curtis_Beard]		04/08/2014	CHG: 74, add missing search options
		/// </history>
		private List<FilterItem> GetFilterItemsByFilterType(FilterType ft)
		{
			return __FilterItems.FindAll(f => f.FilterType == ft);
		}

		/// <summary>
		/// Gets the number of items in the LogItems list for a given type.
		/// </summary>
		/// <param name="type">LogItemType to determine count</param>
		/// <returns>0 if LogItems is null, count for type otherwise</returns>
		/// <history>
		/// [Curtis_Beard]	   12/06/2012	CHG: 1741735, rework to use common LogItems
		/// </history>
		private int GetLogItemsCountByType(LogItem.LogItemTypes type)
		{
			return LogItems.CountByType(type);
		}

		/// <summary>
		/// Sets the grep options
		/// </summary>
		/// <param name="searchWithInResults">is true when desired to search within current results list</param>
		/// <history>
		/// [Curtis_Beard]		10/17/2005	Created
		/// [Curtis_Beard]		07/28/2006  ADD: extension exclusion list
		/// [Andrew_Radford]    13/08/2009  CHG: Now retruns ISearchSpec rather than altering global state
		/// [Curtis_Beard]	   01/31/2012	CHG: 3424154/1816655, allow multiple starting directories
		/// [Curtis_Beard]	   08/01/2012	FIX: 3553252, use | character for path delimitation character
		/// [Curtis_Beard]	   10/30/2012	ADD: 28, search within results
		/// [Curtis_Beard]	   02/04/2014	ADD: 66, option to detect file encoding
		/// [Curtis_Beard]	   12/01/2014	CHG: moved struct declaration to SearchInterfaces.cs under Core.
		/// [Curtis_Beard]      02/09/2015	CHG: 92, support for specific file encodings
		/// [Curtis_Beard]	   05/26/2015	FIX: 69, add performance setting for file detection
		/// [Curtis_Beard]	   09/29/2016	CHG: 24/115, use one interface for search in prep for saving to file
		/// [Curtis_Beard]	   01/31/2019	CHG: 139, expand environment variables within search path(s)
		/// </history>
		private ISearchSpec GetSearchSpecFromUI(bool searchWithInResults)
		{
			string path = Environment.ExpandEnvironmentVariables(cboFilePath.Text.Trim());

			List<string> filePaths = new List<string>();
			if (searchWithInResults)
			{
				// get currently listed file paths from ListView either by selection or all
				if (lstFileNames.SelectedItems != null && lstFileNames.SelectedItems.Count > 0)
				{
					for (int i = 0; i < lstFileNames.SelectedItems.Count; i++)
					{
						filePaths.Add(Path.Combine(lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_DIRECTORY].Text, lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_FILE].Text));
					}
				}
				else
				{
					for (int i = 0; i < lstFileNames.Items.Count; i++)
					{
						filePaths.Add(Path.Combine(lstFileNames.Items[i].SubItems[Constants.COLUMN_INDEX_DIRECTORY].Text, lstFileNames.Items[i].SubItems[Constants.COLUMN_INDEX_FILE].Text));
					}
				}
			}

			var spec = new SearchInterfaces.SearchSpec
			{
				UseCaseSensitivity = chkCaseSensitive.Checked,
				// default context lines to max as the UI toggles what is shown
				ContextLines = Constants.MAX_CONTEXT_LINES,
				UseNegation = chkNegation.Checked,
				ReturnOnlyFileNames = chkFileNamesOnly.Checked,
				SearchInSubfolders = chkRecurse.Checked,
				UseRegularExpressions = chkRegularExpressions.Checked,
				UseWholeWordMatching = chkWholeWordOnly.Checked,
				SearchText = cboSearchForText.Text,
				FileEncodings = FileEncoding.ConvertStringToFileEncodings(GeneralSettings.FileEncodings),
				EncodingDetectionOptions = new EncodingOptions()
				{
					DetectFileEncoding = GeneralSettings.DetectFileEncoding,
					PerformanceSetting = (EncodingOptions.Performance)GeneralSettings.EncodingPerformance,
					UseEncodingCache = GeneralSettings.UseEncodingCache,
					ForcedEncoding = GeneralSettings.ForcedEncoding
				},
				FilterItems = __FilterItems.FindAll(f => f.Enabled),
				LongLineCharCount = GeneralSettings.LongLineCharCount,
				BeforeAfterCharCount = GeneralSettings.BeforeAfterCharCount
			};

			// handle paths used in file filter entry, update path and fileName if fileName has a path in it
			string fileName = fileFilterHelper.GetSelectedFilter(cboFileName);
			int slashPos = fileName.LastIndexOf(Path.DirectorySeparatorChar.ToString());
			if (slashPos > -1)
			{
				fileName = fileName.Substring(slashPos + 1);
			}
			spec.FileFilter = fileName;

			// setup file paths if defined, otherwise setup directories
			if (filePaths != null && filePaths.Count > 0)
			{
				spec.StartFilePaths = filePaths.ToArray();
				spec.StartDirectories = null;
			}
			else
			{
				string[] paths = path.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

				// fileName has a slash, so append the directory and get the file filter
				slashPos = spec.FileFilter.LastIndexOf(Path.DirectorySeparatorChar.ToString());
				if (slashPos > -1)
				{
					// append to each starting directory
					for (int i = 0; i < paths.Length; i++)
					{
						paths[i] += spec.FileFilter.Substring(0, slashPos);
					}
				}
				spec.StartDirectories = paths;
				spec.StartFilePaths = null;
			}

			return spec;
		}

		/// <summary>
		/// Handles drawing bottom border line.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   05/06/2015	Created
		/// [Curtis_Beard]	   05/19/2017	CHG: 120, add option to use accent color
		/// </history>
		private void lblSearchHeading_Paint(object sender, PaintEventArgs e)
		{
			var rect = lblSearchHeading.ClientRectangle;
			rect.Height -= 1;

			e.Graphics.DrawLine(new Pen(ThemeProvider.Theme.Colors.ApplicationAccentColor) { Width = 2 }, rect.X, rect.Height, rect.Width, rect.Height);
		}

		/// <summary>
		/// Handles drawing bottom border line.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   05/06/2015	Created
		/// [Curtis_Beard]	   05/19/2017	CHG: 120, add option to use accent color
		/// </history>
		private void lblSearchOptions_Paint(object sender, PaintEventArgs e)
		{
			var rect = lblSearchOptions.ClientRectangle;
			rect.Height -= 1;

			e.Graphics.DrawLine(new Pen(ThemeProvider.Theme.Colors.ApplicationAccentColor) { Width = 2 }, rect.X, rect.Height, rect.Width, rect.Height);
		}

		/// <summary>
		/// Option to toggle line numbers being displayed.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	ADD: AvalonEdit view menu options
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void LineNumbersMenuItem_Click(object sender, System.EventArgs e)
		{
			LineNumbersMenuItem.Checked = !LineNumbersMenuItem.Checked;
			ResultsViewLineNumbersButton.Checked = LineNumbersMenuItem.Checked;

			txtHits.ShowLineNumbers = LineNumbersMenuItem.Checked;
		}

		/// <summary>
		/// Show the Search Exclusions dialog
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
		/// </history>
		private void lnkExclusions_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			var dlg = new frmExclusions(__FilterItems);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				__FilterItems = dlg.FilterItems;

				LoadLinkLabelStates();
			}
		}

		/// <summary>
		/// Show the Search Plugins dialog
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]	   09/10/2019	ADD: plugins own form
		/// </history>
		private void lnkPlugins_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var dlg = new frmPlugins();
			dlg.ShowDialog(this);

			LoadLinkLabelStates();
		}

		/// <summary>
		/// Loads the given System.Windows.Forms.ComboBox with the values.
		/// </summary>
		/// <param name="combo">System.Windows.Forms.ComboBoxy</param>
		/// <param name="values">string of the values to load</param>
		/// <history>
		/// [Curtis_Beard]	   10/11/2006	Created
		/// [Curtis_Beard]	   11/22/2006	CHG: Remove use of browse in combobox
		/// </history>
		private void LoadComboBoxEntry(System.Windows.Forms.ComboBox combo, string values)
		{
			if (!values.Equals(string.Empty))
			{
				string[] items = Convertors.GetComboBoxEntriesFromString(values);

				if (items.Length > 0)
				{
					int start = items.Length;
					if (start > GeneralSettings.MaximumMRUPaths)
					{
						start = GeneralSettings.MaximumMRUPaths;
					}

					combo.BeginUpdate();
					for (int i = start - 1; i > -1; i--)
					{
						AddComboSelection(combo, items[i]);
					}
					combo.EndUpdate();
				}
			}
		}

		/// <summary>
		/// Load Windows DPI settings and set window and column positions and fonts to defaults if setting has changed.
		/// </summary>
		/// <history>
		/// [LinkNet]        04/30/2016  ADD: Created
		/// </history>
		private void LoadDPISettings()
		{
			using (var graphics = CreateGraphics())
			{
				// get Windows DPI percent scale setting
				int dpi_percent = API.GetCurrentDPISettingPerCent(graphics);

				// set window and column positions and fonts to defaults if DPI percent scale setting has changed
				if (dpi_percent != GeneralSettings.WindowsDPIPerCentSetting)
				{
					// set default window and column positions
					GeneralSettingsReset.WindowSetDefaultPositions();
					GeneralSettingsReset.WindowFileSetDefaultPositions();
					GeneralSettingsReset.LogDisplaySetDefaultPositions();
					GeneralSettingsReset.ExclusionsDisplaySetDefaultPositions();

					// if first start for version with this variable
					if (GeneralSettings.SetDefaultFonts)
					{
						// set default fonts
						GeneralSettings.FilePanelFont = GeneralSettings.FilePanelFontDefault;
						GeneralSettings.ResultsFont = GeneralSettings.ResultsFontDefault;
					}
				}

				// disable after first start
				GeneralSettings.SetDefaultFonts = false;

				// store setting in global variables
				GeneralSettings.WindowsDPIPerCentSetting = dpi_percent;
			}
		}

		/// <summary>
		/// Set the view states of the controls.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      05/16/2007  ADD: created
		/// [LinkNet]           04/29/2017  CHG: removed redundant commented out code
		/// </history>
		private void LoadLinkLabelStates()
		{
			Regex regex = new Regex(@" \(\d*\)$", RegexOptions.Compiled);

			// Plugins
			int pluginCount = PluginManager.Items.Where(p => p.Enabled).Count();
			if (regex.IsMatch(lnkPlugins.Text))
			{
				if (pluginCount > 0)
					lnkPlugins.Text = regex.Replace(lnkPlugins.Text, $" ({pluginCount})");
				else
					lnkPlugins.Text = regex.Replace(lnkPlugins.Text, string.Empty);
			}
			else if (pluginCount > 0)
			{
				lnkPlugins.Text += $" ({pluginCount})";
			}
			if (pluginCount > 0)
			{
				StringBuilder tipBuilder = new StringBuilder();
				tipBuilder.AppendFormat("{0}:", Language.GetGenericText("PluginsColumnEnabled", "Enabled"));
				tipBuilder.AppendLine();
				tipBuilder.AppendLine();
				tipBuilder.Append(string.Join(Environment.NewLine, PluginManager.Items.Where(p => p.Enabled).OrderBy(p => p.Index).Select(p => p.Plugin.Name)));
				toolTip1.SetToolTip(lnkPlugins, tipBuilder.ToString());
			}
			else
			{
				toolTip1.SetToolTip(lnkPlugins, string.Empty);
			}

			// Exclusions/Filters
			int filterCount = __FilterItems.Where(f => f.Enabled).Count();
			if (regex.IsMatch(lnkExclusions.Text))
			{
				if (filterCount > 0)
					lnkExclusions.Text = regex.Replace(lnkExclusions.Text, $" ({filterCount})");
				else
					lnkExclusions.Text = regex.Replace(lnkExclusions.Text, string.Empty);
			}
			else if (filterCount > 0)
			{
				lnkExclusions.Text += $" ({filterCount})";
			}
			if (filterCount > 0)
			{
				StringBuilder tipBuilder = new StringBuilder();
				tipBuilder.AppendFormat("{0}:", Language.GetControlText(lnkExclusions));
				tipBuilder.AppendLine();
				tipBuilder.AppendLine();
				foreach (var filterItem in __FilterItems.Where(f => f.Enabled))
				{
					tipBuilder.Append(Language.GetGenericText($"Exclusions.{filterItem.FilterType.Category}", filterItem.FilterType.Category.ToString()));
					tipBuilder.Append("/");
					tipBuilder.Append(Language.GetGenericText($"Exclusions.{filterItem.FilterType.SubCategory}", filterItem.FilterType.SubCategory.ToString()));

					string valueText = filterItem.Value;
					string optionText = Language.GetGenericText($"Exclusions.{filterItem.ValueOption}");
					string additionalInfo = string.Empty;
					if (filterItem.ValueIgnoreCase)
					{
						additionalInfo = Language.GetGenericText("Exclusions.IgnoreCase");
					}
					else if (filterItem.FilterType.Category == FilterType.Categories.File && filterItem.FilterType.SubCategory == FilterType.SubCategories.Size && !string.IsNullOrEmpty(filterItem.ValueSizeOption))
					{
						valueText = string.Format("{0} {1}", AstroGrep.Core.Convertors.ConvertFileSizeForDisplay(filterItem.Value, filterItem.ValueSizeOption), filterItem.ValueSizeOption);
					}

					tipBuilder.Append(" ");
					tipBuilder.Append(valueText);
					tipBuilder.Append(" ");
					tipBuilder.AppendFormat("{0}{1}", optionText, additionalInfo);
					tipBuilder.AppendLine();
				}
				toolTip1.SetToolTip(lnkExclusions, tipBuilder.ToString());
			}
			else
			{
				toolTip1.SetToolTip(lnkExclusions, string.Empty);
			}
		}

		/// <summary>
		/// Set the Common Search Settings on the form
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   01/28/2005	Created
		/// [Curtis_Beard]	   10/10/2006	CHG: Use search settings implementation.
		/// [Curtis_Beard]	   01/31/2012	ADD: 1561584, ability to skip hidden/system files/directories
		/// [Curtis_Beard]	   02/07/2012	FIX: 3485448, save modified start/end date, min/max file sizes
		/// [Curtis_Beard]      02/09/2012  ADD: 3424156, size drop down selection
		/// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
		/// [Curtis_Beard]	   02/04/2014	FIX: use NumericUpDown's Value property instead of text
		/// [Curtis_Beard]	   04/08/2015	CHG: 54, show all results option
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void LoadSearchSettings()
		{
			chkRegularExpressions.Checked = Core.SearchSettings.UseRegularExpressions;
			chkCaseSensitive.Checked = Core.SearchSettings.UseCaseSensitivity;
			chkWholeWordOnly.Checked = Core.SearchSettings.UseWholeWordMatching;
			ResultsViewLineNumbersButton.Checked = LineNumbersMenuItem.Checked = txtHits.ShowLineNumbers = Core.SearchSettings.IncludeLineNumbers;
			chkRecurse.Checked = Core.SearchSettings.UseRecursion;
			chkFileNamesOnly.Checked = Core.SearchSettings.ReturnOnlyFileNames;
			ResultsContextLinesBeforeCombo.SelectedItem = Core.SearchSettings.ContextLinesBefore.ToString();
			ResultsContextLinesAfterCombo.SelectedItem = Core.SearchSettings.ContextLinesAfter.ToString();
			chkNegation.Checked = Core.SearchSettings.UseNegation;
			chkAllResultsAfterSearch.Checked = Core.SearchSettings.ShowAllResultsAfterSearch;
			__FilterItems = FilterItem.ConvertStringToFilterItems(Core.SearchSettings.FilterItems);
		}

		/// <summary>
		/// Load the general settings values.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/11/2006	Created
		/// [Curtis_Beard]      11/22/2006	CHG: Remove use of browse in combobox
		/// [Curtis_Beard]      02/24/2012	CHG: 3488321, ability to change results font
		/// [Curtis_Beard]      10/10/2012	ADD: 3479503, ability to change file list font
		/// [Curtis_Beard]      04/08/2015	CHG: 20/81, load word wrap and entire file options
		/// [LinkNet]           04/27/2017	CHG: Moved text editor load code to LoadTextEditorSettings
		/// [Curtis_Beard]      09/09/2019  ADD: Show editor characters
		/// </history>
		private void LoadSettings()
		{
			//  Only load up to the desired number of paths.
			if (GeneralSettings.MaximumMRUPaths < 0 || GeneralSettings.MaximumMRUPaths > Constants.MAX_STORED_PATHS)
			{
				GeneralSettings.MaximumMRUPaths = Constants.MAX_STORED_PATHS;
			}

			LoadComboBoxEntry(cboFilePath, GeneralSettings.SearchStarts);
			fileFilterHelper.LoadUserValuesFromString(cboFileName, GeneralSettings.SearchFilters, true);
			LoadComboBoxEntry(cboSearchForText, GeneralSettings.SearchTexts);

			// Path
			if (cboFilePath.Items.Count > 0 && cboFilePath.Items.Count != 1)
			{
				cboFilePath.SelectedIndex = 0;

				SetWindowText();
			}

			// Filter
			if (cboFileName.Items.Count > 0)
			{
				if (GeneralSettings.SearchFiltersIndex > -1 && GeneralSettings.SearchFiltersIndex < cboFileName.Items.Count)
					cboFileName.SelectedIndex = GeneralSettings.SearchFiltersIndex;
				else
					cboFileName.SelectedIndex = 0;
			}

			// Search
			if (cboSearchForText.Items.Count > 0)
			{
				cboSearchForText.SelectedIndex = 0;
			}

			// Viewing options
			ResultsViewRemovingLeadingButton.Checked = RemoveWhiteSpaceMenuItem.Checked = GeneralSettings.RemoveLeadingWhiteSpace;
			ResultsViewEntireFileButton.Checked = EntireFileMenuItem.Checked = GeneralSettings.ShowEntireFile;
			ResultsViewShowCharactersButton.Checked = ShowCharactersMenuItem.Checked = GeneralSettings.ShowEditorCharacters;

			// File list columns
			lstFileNames.Font = Convertors.ConvertStringToFont(GeneralSettings.FilePanelFont);
			SetColumnsText();

			// Load the window settings
			LoadWindowSettings();

			// Load the text editor settings
			LoadTextEditorSettings();

			// Load the text editor
			TextEditors.Load();
		}

		/// <summary>
		/// Load the texteditor settings.
		/// </summary>
		/// <history>
		/// [LinkNet]        04/27/2016  ADD: Created
		/// [LinkNet]        04/27/2017  CHG: add "txtHits.PreviewMouseDoubleClick" event (removed from frmMain designer)
		/// [LinkNet]        04/27/2017  CHG: add texteditor scrollbar visibility code (removed from frmMain designer)
		/// [LinkNet]        04/27/2016  CHG: Duplicate code transferred from LoadSettings and OptionsMenuItem_Click
		/// [LinkNet]        04/27/2016  ADD: Adjust texteditor font size for DPI font scaling
		/// [Curtis_Beard]   09/09/2019  ADD: Show editor characters
		/// [Curtis_Beard]   08/27/2022  Remove existing event handlers before adding
		/// </history>
		private void LoadTextEditorSettings()
		{
			var foreground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsForeColor);
			var background = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsBackColor);
			var contextForeground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsContextForeColor);
			var font = Convertors.ConvertStringToFont(GeneralSettings.ResultsFont);

			txtHits.Foreground = foreground;
			txtHits.Background = background;
			txtHits.LineNumbersForeground = contextForeground;
			txtHits.LineNumbersMatchForeground = foreground;
			txtHits.FontFamily = new System.Windows.Media.FontFamily(font.FontFamily.Name);
			txtHits.FontSize = font.SizeInPoints * 96 / 72;
			txtHits.WordWrap = ResultsViewWordWrapButton.Checked = WordWrapMenuItem.Checked = GeneralSettings.ResultsWordWrap;

			// Sets text editor scroll bar visibility to windows system default
			txtHits.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
			txtHits.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;

			/*
			 * Handles finding the current line under the mouse pointer during a double click to open the text editor at that location
			*/
			// remove any existing handlers
			txtHits.PreviewMouseDoubleClick -= txtHits_PreviewMouseDoubleClick;
			txtHits.PreviewKeyDown -= txtHits_PreviewKeyDown;

			// add handlers
			txtHits.PreviewMouseDoubleClick += txtHits_PreviewMouseDoubleClick;
			txtHits.PreviewKeyDown += txtHits_PreviewKeyDown;

			// editor characters
			ToggleShowCharacters(GeneralSettings.ShowEditorCharacters);
		}

		/// <summary>
		/// Load the window settings.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		06/28/2007	Created
		/// [Curtis_Beard]		03/02/2015	FIX: 63, fix issue when window doesn't fit on a screen (like when a screen is removed)
		/// [Curtis_Beard]		05/14/2015	CHG: adjust display of MenuStrip in Windows XP
		/// [LinkNet]			04/24/2017	CHG: set initial form start position to center of screen corrected for DPI setting
		/// [LinkNet]			04/27/2017	CHG: increase minimum search panel width to avoid cutoff text with higher DPI settings
		/// [LinkNet]			04/29/2017	CHG: set initial file panel column widths corrected for DPI setting
		/// [LinkNet]			04/29/2017	CHG: set initial file panel height corrected for DPI setting
		/// [Curtis_Beard]	    08/27/2022	Safety check for file name list height, better checking for values
		/// </history>
		private void LoadWindowSettings()
		{
			Rectangle defaultBounds = Bounds;

			// set the form start position method
			StartPosition = FormStartPosition.Manual;

			// set the form top position
			if (GeneralSettings.WindowTop > 0)
				Top = GeneralSettings.WindowTop;
			else
			{
				Top = (Screen.PrimaryScreen.Bounds.Height - Constants.WINDOW_HEIGHT * GeneralSettings.WindowsDPIPerCentSetting / 100) / 2;
				if (Top < 0) Top = 0;
			}

			// set the form left position
			if (GeneralSettings.WindowLeft > 0)
				Left = GeneralSettings.WindowLeft;
			else
			{
				Left = (Screen.PrimaryScreen.Bounds.Width - Constants.WINDOW_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100) / 2;
				if (Left < 0) Left = 0;
			}

			// set the form width
			if (GeneralSettings.WindowWidth > 0)
				Width = GeneralSettings.WindowWidth;
			else
			{
				Width = Constants.WINDOW_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100;
				if (Width > Screen.PrimaryScreen.Bounds.Width)
					Width = Constants.WINDOW_WIDTH;
			}

			// set the form height
			if (GeneralSettings.WindowHeight > 0)
				Height = GeneralSettings.WindowHeight;
			else
			{
				Height = Constants.WINDOW_HEIGHT * GeneralSettings.WindowsDPIPerCentSetting / 100;
				if (Height > Screen.PrimaryScreen.Bounds.Height)
					Height = Constants.WINDOW_HEIGHT;
			}

			// form can't find a screen to fit on, so reset to center screen on primary screen
			if (!Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(Bounds)))
			{
				Bounds = defaultBounds;
			}

			// Maximize state check
			if (Enum.TryParse(GeneralSettings.WindowState.ToString(), out FormWindowState stateValue) && stateValue == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Maximized;
			}

			// set the splitter position
			if (GeneralSettings.WindowSearchPanelWidth > 0)
				pnlSearch.Width = GeneralSettings.WindowSearchPanelWidth;

			// set the file panel height
			if (GeneralSettings.WindowFilePanelHeight > 0)
				lstFileNames.Height = GeneralSettings.WindowFilePanelHeight;
			else
				lstFileNames.Height = GeneralSettings.DEFAULT_FILE_PANEL_HEIGHT * GeneralSettings.WindowsDPIPerCentSetting / 100;

			// adjust splitter position if necessary
			if (lstFileNames.Height < splitUpDown.MinSize)
			{
				lstFileNames.Height = splitUpDown.MinSize;
			}
			else if (lstFileNames.Height >= pnlRightSide.Height - splitUpDown.MinExtra)
			{
				lstFileNames.Height = pnlRightSide.Height - splitUpDown.MinExtra;
			}

			// increase minimum default splitter position to avoid cutoff text with increasing DPI values
			splitLeftRight.MinSize = Constants.DEFAULT_SEARCH_PANEL_WIDTH * GeneralSettings.WindowsDPIPerCentSetting / 100;

			// adjust splitter position if necessary
			if (pnlSearch.Width < splitLeftRight.MinSize)
			{
				pnlSearch.Width = splitLeftRight.MinSize;
			}

			// set file column widths from user settings
			if (GeneralSettings.WindowFileColumnNameWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE].Width = GeneralSettings.WindowFileColumnNameWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE].Width = Constants.COLUMN_WIDTH_FILE * GeneralSettings.WindowsDPIPerCentSetting / 100;

			if (GeneralSettings.WindowFileColumnLocationWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_DIRECTORY].Width = GeneralSettings.WindowFileColumnLocationWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_DIRECTORY].Width = Constants.COLUMN_WIDTH_DIRECTORY * GeneralSettings.WindowsDPIPerCentSetting / 100;

			if (GeneralSettings.WindowFileColumnFileExtWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE_EXT].Width = GeneralSettings.WindowFileColumnFileExtWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE_EXT].Width = Constants.COLUMN_WIDTH_FILE_EXT * GeneralSettings.WindowsDPIPerCentSetting / 100;

			if (GeneralSettings.WindowFileColumnDateWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_DATE].Width = GeneralSettings.WindowFileColumnDateWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_DATE].Width = Constants.COLUMN_WIDTH_DATE * GeneralSettings.WindowsDPIPerCentSetting / 100;

			if (GeneralSettings.WindowFileColumnSizeWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_SIZE].Width = GeneralSettings.WindowFileColumnSizeWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_SIZE].Width = Constants.COLUMN_WIDTH_SIZE * GeneralSettings.WindowsDPIPerCentSetting / 100;

			if (GeneralSettings.WindowFileColumnCountWidth > 0)
				lstFileNames.Columns[Constants.COLUMN_INDEX_COUNT].Width = GeneralSettings.WindowFileColumnCountWidth;
			else
				lstFileNames.Columns[Constants.COLUMN_INDEX_COUNT].Width = Constants.COLUMN_WIDTH_COUNT * GeneralSettings.WindowsDPIPerCentSetting / 100;
		}

		/// <summary>
		/// Open folder container log file(s).
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/15/2015	Initial
		/// </history>
		private void LogFileMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(ApplicationPaths.LogFile);
		}

		/// <summary>
		/// Handles formatting of log search option.
		/// </summary>
		/// <param name="optionBuilder">Current StringBuilder</param>
		/// <param name="option">if option is enabled</param>
		/// <param name="displayText">text to display for option</param>
		/// <history>
		/// [Curtis_Beard]		05/15/2015	Initial
		/// </history>
		private void LogSearchOptionHelper(StringBuilder optionBuilder, bool option, string displayText)
		{
			if (option)
			{
				if (optionBuilder.Length > 0)
					optionBuilder.Append(", ");

				optionBuilder.Append(displayText);
			}
		}

		/// <summary>
		/// Logs a start search message to log file.
		/// </summary>
		/// <param name="searchSpec">Current search specification</param>
		/// <history>
		/// [Curtis_Beard]		05/15/2015	Initial
		/// [Curtis_Beard]	   05/26/2015	FIX: 69, add performance setting, cache for file encoding detection
		/// </history>
		private void LogStartSearchMessage(ISearchSpec searchSpec)
		{
			StringBuilder searchTextOptions = new StringBuilder();
			LogSearchOptionHelper(searchTextOptions, searchSpec.UseRegularExpressions, "regex");
			LogSearchOptionHelper(searchTextOptions, searchSpec.UseCaseSensitivity, "case sensitive");
			LogSearchOptionHelper(searchTextOptions, searchSpec.UseWholeWordMatching, "whole word");
			LogSearchOptionHelper(searchTextOptions, searchSpec.UseNegation, "negation");
			LogSearchOptionHelper(searchTextOptions, searchSpec.ReturnOnlyFileNames, "only file names");
			//LogSearchOptionHelper(searchTextOptions, searchSpec.ContextLines > 0, string.Format("{0} context lines", searchSpec.ContextLines));

			if (searchTextOptions.Length > 0)
			{
				searchTextOptions.Insert(0, "[");
				searchTextOptions.Append("]");
			}

			StringBuilder fileEncoding = new StringBuilder();
			if (searchSpec.EncodingDetectionOptions.DetectFileEncoding)
			{
				fileEncoding.Append("[");
				fileEncoding.Append("detect encoding");
				fileEncoding.AppendFormat(", performance set at {0}", Enum.GetName(typeof(EncodingOptions.Performance), GeneralSettings.EncodingPerformance).ToLower());

				if (searchSpec.EncodingDetectionOptions.UseEncodingCache)
				{
					fileEncoding.Append(", cache enabled");
				}

				fileEncoding.Append("]");
			}

			LogClient.Instance.Logger.Info("Search started in '{0}'{1} against {2}{3} for {4}{5}",
			   searchSpec.StartFilePaths != null && searchSpec.StartFilePaths.Length > 0 ? string.Join(", ", searchSpec.StartFilePaths) : string.Join(", ", searchSpec.StartDirectories),
			   searchSpec.SearchInSubfolders ? "[include sub folders]" : "",
			   searchSpec.FileFilter,
			   fileEncoding.ToString(),
			   searchSpec.SearchText,
			   searchTextOptions.ToString());
		}

		/// <summary>
		/// Logs a stop search message to log file.
		/// </summary>
		/// <param name="stopType">Stopping type for current log entry</param>
		/// <returns>Number of elapsed seconds the search took</returns>
		/// <history>
		/// [Curtis_Beard]		05/26/2015  CHG: add stop search messsage to log with time
		/// [Curtis_Beard]		01/08/2019  CHG: 129, add total files searched
		/// [Curtis_Beard]	    03/05/2020	CHG: return search stats
		/// </history>
		private Tuple<double, long> LogStopSearchMessage(string stopType)
		{
			long endingTime = Stopwatch.GetTimestamp();
			long elapsedTime = endingTime - StartingTime;
			double elapsedSeconds = elapsedTime * (1.0 / Stopwatch.Frequency);
			string stopTypeMessage = !string.IsNullOrEmpty(stopType) ? string.Format(" ({0})", stopType) : string.Empty;
			long totalFiles = __Grep != null ? __Grep.TotalFilesSearched : 0;

			LogClient.Instance.Logger.Info("Search stopped{0}, taking {1} seconds, searching {2} file(s)", stopTypeMessage, elapsedSeconds, totalFiles);

			return new Tuple<double, long>(elapsedSeconds, totalFiles);
		}

		/// <summary>
		/// Allow sorting of list view columns
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]	   02/06/2005	Created
		/// [Curtis_Beard]	   07/07/2006	CHG: add support for count column sorting
		/// [Curtis_Beard]	   10/06/2006	FIX: clear sort indicator propertly
		/// [Curtis_Beard]		02/17/2012	CHG: update listview sorting
		/// </history>
		private void lstFileNames_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// set to wait cursor
			lstFileNames.Cursor = Cursors.WaitCursor;

			// Determine whether the column is the same as the last column clicked.
			if (e.Column != __SortColumn)
			{
				// Set the sort column to the new column.
				__SortColumn = e.Column;

				// Set the sort order to ascending by default.
				if (e.Column == Constants.COLUMN_INDEX_COUNT ||
				   e.Column == Constants.COLUMN_INDEX_SIZE ||
				   e.Column == Constants.COLUMN_INDEX_DATE)
				{
					lstFileNames.Sorting = SortOrder.Descending;
				}
				else
				{
					lstFileNames.Sorting = SortOrder.Ascending;
				}
			}
			else
			{
				// Determine what the last sort order was and change it.
				if (lstFileNames.Sorting == SortOrder.Ascending)
					lstFileNames.Sorting = SortOrder.Descending;
				else
					lstFileNames.Sorting = SortOrder.Ascending;
			}

			// Set the ListViewItemSorter property to a new ListViewItemComparer object.
			ListViewItemComparer comparer = new ListViewItemComparer(e.Column, lstFileNames.Sorting);
			lstFileNames.ListViewItemSorter = comparer;

			// Call the sort method to manually sort.
			lstFileNames.Sort();

			// Display sort image and highlight sort column
			Windows.API.ListViewExtensions.SetSortIcon(lstFileNames, e.Column, lstFileNames.Sorting);

			// Apply theming since sorting removes it.
			Windows.API.ListViewExtensions.SetTheme(lstFileNames);

			// restore to default cursor
			lstFileNames.Cursor = Cursors.Default;
		}

		/// <summary>
		/// Handles setting the form's AcceptButton to null so that enter key can be processed by control.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// /// <history>
		/// [Curtis_Beard]		03/20/2014	CHG: 73, enter key opens editor of file list
		/// </history>
		private void lstFileNames_Enter(object sender, EventArgs e)
		{
			AcceptButton = null;
		}

		/// <summary>
		/// Handles setting up the drag event for a selected file.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		07/25/2006	ADD: 1512028, Drag support
		/// [Andrew_Radford]		26/09/2009	FIX: 2864409, Drag and drop to editor open only 1 file
		/// </history>
		private void lstFileNames_ItemDrag(object sender, ItemDragEventArgs e)
		{
			var lst = sender as ListView;
			var paths = new List<string>();

			foreach (ListViewItem item in lst.SelectedItems)
			{
				var path = item.SubItems[Constants.COLUMN_INDEX_DIRECTORY].Text +
				 Path.DirectorySeparatorChar +
				 item.SubItems[Constants.COLUMN_INDEX_FILE].Text;

				if (File.Exists(path))
				{
					paths.Add(path);
				}
			}

			var dataObject = new DataObject(DataFormats.FileDrop, paths.ToArray());
			lstFileNames.DoDragDrop(dataObject, DragDropEffects.Copy);
		}

		/// <summary>
		/// Need certain keyboard events on the lstFileNames.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Ed_Jakbuowski]     05/20/2009  Created
		/// [Curtis_Beard]	   09/28/2012	CHG: use common select method
		/// [Curtis_Beard]	   03/21/2014	CHG: 73, edit selected files on enter key
		/// </history>
		private void lstFileNames_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//ctrl+c  Copy to clipboard
			if (e.KeyCode == Keys.C && e.Control)
			{
				CopyMenuItem_Click(sender, EventArgs.Empty);
			}

			//ctrl+a  Select All
			if (e.KeyCode == Keys.A && e.Control)
			{
				SelectAllListItems();
			}

			//enter Edit selected file
			if (e.KeyCode == Keys.Enter)
			{
				OpenSelectedMenuItem_Click(null, null);
			}

			// ** I think the delete key is a bad idea, because its too easy to modify the results by mistake.
			//delete Delete selected from list
			//if (e.KeyCode == Keys.Delete)
			//{
			//   DeleteMenuItem_Click(sender, EventArgs.Empty);
			//}
		}

		/// <summary>
		/// Handles setting the form's AcceptButton to btnSearch so that enter key defaults to btnSearch for rest of form.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		03/20/2014	CHG: 73, enter key opens editor of file list
		/// </history>
		private void lstFileNames_Leave(object sender, EventArgs e)
		{
			AcceptButton = btnSearch;
		}

		/// <summary>
		/// File Name List Double Click Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   12/07/2005	CHG: Use column constant
		/// [Curtis_Beard]	   07/03/2006	FIX: 1516777, stop right click to open text editor,
		///                                 changed from DoubleClick event to MouseDown
		/// [Curtis_Beard]	   07/26/2006	ADD: 1512026, column position
		/// [Curtis_Beard]	   02/24/2012	CHG: 3488322, use first hit line/column position instead of line 1, column 1
		/// [Curtis_Beard]	   05/27/2015	FIX: 73, open text editor even when no first match (usually during file only search)
		/// </history>
		private void lstFileNames_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.Clicks == 2)
			{
				// Make sure there is something to click on
				if (lstFileNames.SelectedItems.Count == 0)
					return;

				var match = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[0].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
				TextEditors.Open(TextEditorOpener.FromMatch(match, __Grep.SearchSpec.SearchText));
			}
		}

		/// <summary>
		/// File Name List Select Index Change Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   12/07/2005	CHG: Use column constant
		/// [Curtis_Beard]	   02/24/2012	CHG: 3488322, use hand cursor for results view to signal click
		/// [Curtis_Beard]	   09/28/2012	CHG: only attempt file show when 1 item is selected (prevents flickering and loading on all deselect)
		/// [Curtis_Beard]	   02/24/2015	CHG: remove isSearching check so that you can view selected file during a search
		/// [Curtis_Beard]	   10/27/2015	FIX: 78, clear content area when nothing selected
		/// [Curtis_Beard]	   09/04/2019	CHG: 105, Show the context of a file only if left click
		/// </history>
		private void lstFileNames_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// don't let a right click change the selection
			if (Control.MouseButtons == System.Windows.Forms.MouseButtons.Right)
				return;

			if (lstFileNames.SelectedItems.Count == 1)
			{
				// retrieve hit object
				MatchResult match = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[0].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));

				SetStatusBarEncoding(match.DetectedEncoding != null ? match.DetectedEncoding.EncodingName : string.Empty);

				if (EntireFileMenuItem.Checked)
				{
					ProcessFileForDisplay(match);
				}
				else
				{
					ProcessMatchForDisplay(match);
				}
			}

			if (lstFileNames.SelectedItems.Count == 0)
			{
				SetStatusBarEncoding(string.Empty);
				txtHits.Clear();
			}
		}

		/// <summary>
		/// Handles determining enabling the open file menuitem.
		/// </summary>
		/// <param name="sender">CustomTextEditor</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		04/08/2015	Initial
		/// [Curtis_Beard]		06/29/2015	CHG: reconfigure to use common method
		/// </history>
		private void menu_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
		{
			// enable open at menu item
			var opener = GetEditorAtLocation(txtHits.GetPositionFromRightClickPoint());
			((sender as TextEditorEx).ContextMenu.Items[0] as System.Windows.Controls.MenuItem).IsEnabled = opener.HasValue();
		}

		/// <summary>
		/// Create a new search window.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      05/06/2014  Created
		/// [Curtis_Beard]      05/14/2015  CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void NewWindowMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(ApplicationPaths.ExecutingAssembly);
		}

		/// <summary>
		/// Handles finding the current line under the mouse pointer to open the texteditor at that location.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		04/08/2015	Initial
		/// [Curtis_Beard]		06/29/2015	CHG: reconfigure to use common method
		/// </history>
		private void openFile_Click(object sender, EventArgs e)
		{
			var opener = GetEditorAtLocation(txtHits.GetPositionFromRightClickPoint());
			if (opener.HasValue())
			{
				TextEditors.Open(opener);
			}
		}

		/// <summary>
		/// Context Menu item for opening selected file's Directory
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// /// <history>
		/// [Ed_Jakbuowski]     05/20/2009  Created
		/// [Curtis_Beard]      02/12/2014  CHG: 77, select file in explorer window
		/// </history>
		private void OpenFolderMenuItem_Click(object sender, System.EventArgs e)
		{
			try
			{
				foreach (ListViewItem lvi in lstFileNames.SelectedItems)
				{
					string filename = Path.Combine(lvi.SubItems[Constants.COLUMN_INDEX_DIRECTORY].Text, lvi.SubItems[Constants.COLUMN_INDEX_FILE].Text);
					if (File.Exists(filename))
					{
						// explorer argument format: "/select, " + filename;
						string fileSelect = string.Format("/select, \"{0}\"", filename);
						System.Diagnostics.Process.Start("Explorer.exe", fileSelect);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Context Menu item for opening selected files
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// /// <history>
		/// [Ed_Jakbuowski]     05/20/2009  Created
		/// </history>
		private void OpenMenuItem_Click(object sender, System.EventArgs e)
		{
			OpenSelectedMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Open Selected Files Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   12/07/2005	CHG: Use column constant
		/// [Curtis_Beard]	   07/26/2006	ADD: 1512026, column position
		/// [Curtis_Beard]	   03/16/2015	CHG: check for null HitObject
		/// [Curtis_Beard]	   05/27/2015	FIX: 73, open text editor even when no first match (usually during file only search)
		/// </history>
		private void OpenSelectedMenuItem_Click(object sender, System.EventArgs e)
		{
			for (int i = 0; i < lstFileNames.SelectedItems.Count; i++)
			{
				var match = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
				TextEditors.Open(TextEditorOpener.FromMatch(match, __Grep.SearchSpec.SearchText));
			}
		}

		/// <summary>
		/// Context Menu item for opening selected file with associated application.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      02/12/2014  ADD: 67, open selected file(s) with associated application
		/// </history>
		private void OpenWithAssociatedApp_Click(object sender, System.EventArgs e)
		{
			try
			{
				string path;
				MatchResult hit;

				for (int i = 0; i < lstFileNames.SelectedItems.Count; i++)
				{
					// retrieve hit object
					hit = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[i].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));

					// retrieve the filename
					path = hit.File.FullName;

					TextEditors.OpenWithDefault(path);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Menu Options Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]		??/??/????  Initial
		/// [Curtis_Beard]		01/11/2005	.Net Conversion
		/// [Curtis_Beard]		11/10/2006	ADD: Update combo boxes and language changes
		/// [Curtis_Beard]		11/22/2006	CHG: Remove use of browse in combobox
		/// [Curtis_Beard]		05/22/2007	FIX: 1723814, rehighlight the selected result
		/// [Curtis_Beard]	   02/24/2012	CHG: 3488321, ability to change results font
		/// [Curtis_Beard]	   09/26/2012	CHG: Update status bar text
		/// [Curtis_Beard]	   10/10/2012	ADD: 3479503, ability to change file list font
		/// [Curtis_Beard]		06/15/2015	CHG: 57, support external language files
		/// [Curtis_Beard]	   05/19/2017	CHG: 120, add option to use accent color
		/// [Curtis_Beard]	   08/20/2019  CHG: app color for splitters and use system color for default instead of black
		/// </history>
		private void OptionsMenuItem_Click(object sender, System.EventArgs e)
		{
			var optionsForm = new frmOptions();

			if (optionsForm.ShowDialog(this) == DialogResult.OK)
			{
				//update combo-box lengths
				while (cboFilePath.Items.Count > GeneralSettings.MaximumMRUPaths)
					cboFilePath.Items.RemoveAt(cboFilePath.Items.Count - 1);
				while (cboSearchForText.Items.Count > GeneralSettings.MaximumMRUPaths)
					cboSearchForText.Items.RemoveAt(cboSearchForText.Items.Count - 1);

				// load new language if necessary
				if (optionsForm.IsLanguageChange)
				{
					Language.ProcessForm(this, toolTip1);

					SetColumnsText();
					SetWindowText();

					// clear statusbar text
					SetStatusBarMessage(string.Empty);
					CalculateTotalCount();

					// reload system file filters
					CreateFileFilterHelper();

					LoadLinkLabelStates();
				}

				// this will also reload any language changes if needed
				fileFilterHelper.AdjustUserValueMax(cboFileName, GeneralSettings.MaximumMRUPaths);

				lstFileNames.Font = Convertors.ConvertStringToFont(GeneralSettings.FilePanelFont);

				// reload the text editor settings
				LoadTextEditorSettings();

				if (optionsForm.IsThemeChange)
				{
					if (Enum.TryParse(GeneralSettings.ThemeType.ToString(), out ThemeProvider.ThemeType themeType))
					{
						ThemeProvider.ChangeTheme(themeType);

						ReloadTheme(themeType);
					}
				}

				// update current display
				if (lstFileNames.SelectedItems.Count > 0)
				{
					lstFileNames_SelectedIndexChanged(null, null);
				}
				else if (txtHits.Text.Length > 0)
				{
					ProcessAllMatchesForDisplay();
				}
			}
		}

		/// <summary>
		/// Output results using given export delegate.
		/// </summary>
		/// <param name="filename">Current filename</param>
		/// <param name="outputter">File delegate</param>
		/// <history>
		/// [Andrew_Radford]   20/09/2009	Initial
		/// [Curtis_Beard]     12/03/2014	CHG: pass grep indexes instead of ListView
		/// [Curtis_Beard]     04/08/2015	CHG: update export delegate with settings class
		/// [Curtis_Beard]     05/19/2016	FIX: 89, add logging
		/// </history>
		private void OutputResults(string filename, MatchResultsExport.FileDelegate outputter)
		{
			SetStatusBarMessage(string.Format(Language.GetGenericText("SaveSaving"), filename));

			Invoke(new Action(() =>
			{
				try
				{
					// get all grep indexes
					IEnumerable<ListViewItem> lv = lstFileNames.Items.Cast<ListViewItem>();
					var indexes = (from i in lv select int.Parse(i.SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text)).ToList();

					MatchResultsExportSettings settings = new MatchResultsExportSettings()
					{
						Path = filename,
						Grep = __Grep,
						GrepIndexes = indexes,
						ShowLineNumbers = LineNumbersMenuItem.Checked,
						RemoveLeadingWhiteSpace = RemoveWhiteSpaceMenuItem.Checked,
						ContextLinesBefore = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem),
						ContextLinesAfter = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem)
					};
					outputter(settings);
					SetStatusBarMessage(Language.GetGenericText("SaveSaved"));
				}
				catch (Exception ex)
				{
					LogClient.Instance.Logger.Error("Save Results Error: {0}", LogClient.GetAllExceptions(ex));

					MessageBox.Show(
						string.Format(Language.GetGenericText("SaveError"), ex),
						ProductInformation.ApplicationName,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}));
		}

		/// <summary>
		/// Allows selection of the search path.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/22/2006	Created
		/// </history>
		private void picBrowse_Click(object sender, System.EventArgs e)
		{
			BrowseForFolder(ModifierKeys.HasFlag(Keys.Shift));
		}

		/// <summary>
		/// Resize the comboboxes when the main search panel is resized.
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <remarks>
		///   This is a workaround for a bug in the .NET 2003 combobox control
		///   when it is set to ComboBoxStyle.DropDown that will select the text
		///   in the control when it is resized.
		///   By temporarily changing the style to ComboBoxStyle.Simple and manually
		///   setting the width, we can avoid this annoying feature.
		/// </remarks>
		/// <history>
		/// [Son_Le]            08/08/2005  FIX:1180742, remove highlight of combobox
		/// [Curtis_Beard]      11/03/2006  CHG: don't resize just change style
		/// [Curtis_Beard]      02/21/2014  FIX: 50, set back text after change so user entered text isn't cleared
		/// [Curtis_Beard]      09/16/2019  FIX: reset selection length instead of changing/setting text/type
		/// </history>
		private void pnlSearch_SizeChanged(object sender, EventArgs e)
		{
			// reset selection length
			cboFilePath.SelectionLength = 0;
			cboFileName.SelectionLength = 0;
			cboSearchForText.SelectionLength = 0;
		}

		/// <summary>
		/// Show Print Dialog
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   09/10/2005	CHG: pass in listView and grep hashtable
		/// [Curtis_Beard]	   10/14/2005	CHG: use No Results for message box title
		/// [Curtis_Beard]	   12/07/2005	CHG: Pass in font name and size to print dialog
		/// [Curtis_Beard]	   10/11/2006	CHG: Pass in font and icon
		/// </history>
		private void PrintResultsMenuItem_Click(object sender, EventArgs e)
		{
			if (lstFileNames.Items.Count > 0)
			{
				// get all grep indexes
				IEnumerable<ListViewItem> lv = lstFileNames.Items.Cast<ListViewItem>();
				var indexes = (from i in lv where i.Selected select int.Parse(i.SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text)).ToList();

				MatchResultsExportSettings settings = new MatchResultsExportSettings()
				{
					Grep = __Grep,
					GrepIndexes = indexes,
					ShowLineNumbers = LineNumbersMenuItem.Checked,
					RemoveLeadingWhiteSpace = RemoveWhiteSpaceMenuItem.Checked,
					ContextLinesBefore = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem),
					ContextLinesAfter = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem)
				};

				using (var printForm = new frmPrint(settings, Convertors.ConvertStringToFont(GeneralSettings.ResultsFont), Icon))
				{
					printForm.ShowDialog(this);
				}
			}
			else
				MessageBox.Show(Language.GetGenericText("PrintNoResults"), ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Handles generating the text for all matches.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]     04/08/2015  CHG: 54, add check to show all the results after a search
		/// [LinkNet]          08/01/2017  FIX: Resolved problem searching for spaces with "remove leading white space" option selected
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void ProcessAllMatchesForDisplay()
		{
			txtHits.Dispatcher.Invoke(new Action(() =>
			{
				int beforeContextLines = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem);
				int afterContextLines = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem);

				txtHits.Clear();
				txtHits.SyntaxHighlighting = null;
				txtHits.ScrollToHome();

				if (__Grep == null || __Grep.MatchResults.Count == 0)
					return;

				for (int i = txtHits.TextArea.TextView.LineTransformers.Count - 1; i >= 0; i--)
				{
					if (txtHits.TextArea.TextView.LineTransformers[i] is ResultHighlighter)
						txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
					else if (txtHits.TextArea.TextView.LineTransformers[i] is AllResultHighlighter)
						txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
				}
				var foreground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightForeColor);
				var background = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightBackColor);
				var nonForeground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsContextForeColor);
				txtHits.TextArea.TextView.LineTransformers.Add(new AllResultHighlighter(__Grep.MatchResults, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines)
				{
					MatchForeground = foreground,
					MatchBackground = background,
					NonMatchForeground = nonForeground
				});

				StringBuilder builder = new StringBuilder();
				var lineNumbers = new List<LineNumber>();

				int maxResults = __Grep.MatchResults.Count;
				for (int i = 0; i < maxResults; i++)
				{
					var match = __Grep.MatchResults[i];
					var path = match.File.FullName;
					builder.AppendLine(match.File.FullName);
					builder.AppendLine();
					lineNumbers.Add(new LineNumber() { FileFullName = path });
					lineNumbers.Add(new LineNumber());

					var matches = match.GetDisplayMatches(beforeContextLines, afterContextLines);
					int max = matches.Count;
					for (int j = 0; j < max; j++)
					{
						string line = matches[j].Line;

						if (RemoveWhiteSpaceMenuItem.Checked)
						{
							if (matches[j].HasMatch)
							{
								line = line.Substring(Utils.GetValidLeadingSpaces(line, matches[j].Matches[0].StartPosition));
							}
							else
							{
								line = line.TrimStart();
							}
						}
						builder.AppendLine(line);
						lineNumbers.Add(new LineNumber()
						{
							Number = matches[j].LineNumber,
							HasMatch = matches[j].HasMatch,
							FileFullName = matches[j].LineNumber > -1 ? path : string.Empty,
							ColumnNumber = matches[j].ColumnNumber
						});
					}

					if (i + 1 < maxResults)
					{
						builder.AppendLine();
						builder.AppendLine();
						lineNumbers.Add(new LineNumber());
						lineNumbers.Add(new LineNumber());
					}
				}

				// the last result will have a hanging newline, so remove it.
				if (builder.Length > 0)
				{
					builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
				}

				txtHits.LineNumbers = lineNumbers;
				txtHits.Text = builder.ToString();

				ProcessDocumentForAnchors(false, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines);
			}));
		}

		/// <summary>
		/// Processes any command line arguments
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		07/25/2006	ADD: 1492221, command line parameters
		/// [Curtis_Beard]		05/18/2007	CHG: adapt to new processing
		/// [Curtis_Beard]		09/26/2012	CHG: 3572487, remove args parameter and use class property
		/// [Curtis_Beard]		04/08/2014	CHG: 74, add missing search options
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void ProcessCommandLine()
		{
			if (CommandLineArgs.AnyArguments)
			{
				if (CommandLineArgs.IsValidStartPath)
					AddComboSelection(cboFilePath, CommandLineArgs.StartPath);

				if (CommandLineArgs.IsValidFileTypes)
					fileFilterHelper.AddUserValue(cboFileName, CommandLineArgs.FileTypes, Core.GeneralSettings.MaximumMRUPaths);

				if (CommandLineArgs.IsValidSearchText)
					AddComboSelection(cboSearchForText, CommandLineArgs.SearchText);

				// turn on option if specified (options default to last saved otherwise)
				if (CommandLineArgs.UseRegularExpressions)
					chkRegularExpressions.Checked = true;
				if (CommandLineArgs.IsCaseSensitive)
					chkCaseSensitive.Checked = true;
				if (CommandLineArgs.IsWholeWord)
					chkWholeWordOnly.Checked = true;
				if (CommandLineArgs.UseRecursion)
					chkRecurse.Checked = true;
				if (CommandLineArgs.IsFileNamesOnly)
					chkFileNamesOnly.Checked = true;
				if (CommandLineArgs.IsNegation)
					chkNegation.Checked = true;
				if (CommandLineArgs.UseLineNumbers)
					LineNumbersMenuItem.Checked = true;
				if (CommandLineArgs.ContextLines > -1)
				{
					ResultsContextLinesBeforeCombo.SelectedItem = CommandLineArgs.ContextLines.ToString();
					ResultsContextLinesAfterCombo.SelectedItem = CommandLineArgs.ContextLines.ToString();
				}
				if (CommandLineArgs.ContextLinesBefore > -1)
					ResultsContextLinesBeforeCombo.SelectedItem = CommandLineArgs.ContextLinesBefore.ToString();
				if (CommandLineArgs.ContextLinesAfter > -1)
					ResultsContextLinesAfterCombo.SelectedItem = CommandLineArgs.ContextLinesAfter.ToString();

				if (CommandLineArgs.SkipHiddenFile)
				{
					// check if exists, enable if necessary, or create them
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Hidden));

					if (fiFile == null || fiFile.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Hidden),
						   string.Empty, FilterType.ValueOptions.None, false, true));
					}
					else if (!fiFile.First().Enabled)
					{
						int index = __FilterItems.IndexOf(fiFile.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
						}
					}
				}

				if (CommandLineArgs.SkipHiddenDirectory)
				{
					// check if exists, enable if necessary, or create them
					var fiDir = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.Hidden));

					if (fiDir == null || fiDir.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.Hidden),
						   string.Empty, FilterType.ValueOptions.None, false, true));
					}
					else if (!fiDir.First().Enabled)
					{
						int index = __FilterItems.IndexOf(fiDir.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
						}
					}
				}
				if (CommandLineArgs.SkipSystemFile)
				{
					// check if exists, enable if necessary, or create them
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.System));

					if (fiFile == null || fiFile.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.System),
						   string.Empty, FilterType.ValueOptions.None, false, true));
					}
					else if (!fiFile.First().Enabled)
					{
						int index = __FilterItems.IndexOf(fiFile.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
						}
					}
				}

				if (CommandLineArgs.SkipSystemDirectory)
				{
					// check if exists, enable if necessary, or create them
					var fiDir = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.System));

					if (fiDir == null || fiDir.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.System),
						   string.Empty, FilterType.ValueOptions.None, false, true));
					}
					else if (!fiDir.First().Enabled)
					{
						int index = __FilterItems.IndexOf(fiDir.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
						}
					}
				}

				if (CommandLineArgs.DateModifiedFile != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.DateModified));

					bool foundExact = false;
					if (fiFile != null && fiFile.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiFile)
						{
							if (item.ValueOption == CommandLineArgs.DateModifiedFile.ValueOption && item.Value.Equals(CommandLineArgs.DateModifiedFile.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiFile == null || fiFile.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.DateModified),
						   CommandLineArgs.DateModifiedFile.Value.ToString(), CommandLineArgs.DateModifiedFile.ValueOption, false, true));
					}
				}

				if (CommandLineArgs.DateModifiedDirectory != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiDir = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.DateModified));

					bool foundExact = false;
					if (fiDir != null && fiDir.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiDir)
						{
							if (item.ValueOption == CommandLineArgs.DateModifiedDirectory.ValueOption && item.Value.Equals(CommandLineArgs.DateModifiedDirectory.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiDir == null || fiDir.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.DateModified),
						   CommandLineArgs.DateModifiedDirectory.Value.ToString(), CommandLineArgs.DateModifiedDirectory.ValueOption, false, true));
					}
				}

				if (CommandLineArgs.DateCreatedFile != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.DateCreated));

					bool foundExact = false;
					if (fiFile != null && fiFile.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiFile)
						{
							if (item.ValueOption == CommandLineArgs.DateCreatedFile.ValueOption && item.Value.Equals(CommandLineArgs.DateCreatedFile.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiFile == null || fiFile.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.DateCreated),
						   CommandLineArgs.DateCreatedFile.Value.ToString(), CommandLineArgs.DateCreatedFile.ValueOption, false, true));
					}
				}

				if (CommandLineArgs.DateCreatedDirectory != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiDir = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.DateCreated));

					bool foundExact = false;
					if (fiDir != null && fiDir.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiDir)
						{
							if (item.ValueOption == CommandLineArgs.DateCreatedDirectory.ValueOption && item.Value.Equals(CommandLineArgs.DateCreatedDirectory.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiDir == null || fiDir.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.Directory, FilterType.SubCategories.DateCreated),
						   CommandLineArgs.DateCreatedDirectory.Value.ToString(), CommandLineArgs.DateCreatedDirectory.ValueOption, false, true));
					}
				}

				if (CommandLineArgs.MinFileSize != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Size));

					bool foundExact = false;
					if (fiFile != null && fiFile.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiFile)
						{
							if (item.ValueOption == CommandLineArgs.MinFileSize.ValueOption && item.Value.Equals(CommandLineArgs.MinFileSize.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiFile == null || fiFile.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Size),
						   CommandLineArgs.MinFileSize.Value.ToString(), CommandLineArgs.MinFileSize.ValueOption, false, "byte", true));
					}
				}

				if (CommandLineArgs.MaxFileSize != null)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Size));

					bool foundExact = false;
					if (fiFile != null && fiFile.Count > 0)
					{
						// more than one entry, so disable any that aren't less
						foreach (var item in fiFile)
						{
							if (item.ValueOption == CommandLineArgs.MaxFileSize.ValueOption && item.Value.Equals(CommandLineArgs.MaxFileSize.Value.ToString()))
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = true;
									foundExact = true;
								}
							}
							else
							{
								int index = __FilterItems.IndexOf(item);
								if (index > -1)
								{
									__FilterItems[index].Enabled = false;
								}
							}
						}
					}

					if (fiFile == null || fiFile.Count == 0 || !foundExact)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.Size),
						   CommandLineArgs.MaxFileSize.Value.ToString(), CommandLineArgs.MaxFileSize.ValueOption, false, "byte", true));
					}
				}

				if (CommandLineArgs.MinFileCount > 0)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.MinimumHitCount));

					if (fiFile == null || fiFile.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.MinimumHitCount),
						   CommandLineArgs.MinFileCount.ToString(), FilterType.ValueOptions.None, false, true));
					}
					else
					{
						int index = __FilterItems.IndexOf(fiFile.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
							__FilterItems[index].Value = CommandLineArgs.MinFileCount.ToString();
						}
					}
				}

				if (CommandLineArgs.ReadOnlyFile)
				{
					// check if exists, enable if necessary/set value, or create it
					var fiFile = GetFilterItemsByFilterType(new FilterType(FilterType.Categories.File, FilterType.SubCategories.ReadOnly));

					if (fiFile == null || fiFile.Count == 0)
					{
						__FilterItems.Add(new FilterItem(new FilterType(FilterType.Categories.File, FilterType.SubCategories.ReadOnly),
						   string.Empty, FilterType.ValueOptions.None, false, true));
					}
					else
					{
						int index = __FilterItems.IndexOf(fiFile.First());
						if (index > -1)
						{
							__FilterItems[index].Enabled = true;
						}
					}
				}

				// keep last to ensure all options are set before a search begins
				if (CommandLineArgs.StartSearch)
				{
					btnSearch_Click(null, null);
					Show();
					Refresh();
				}
			}
		}

		/// <summary>
		/// Process the current document and generate the anchor matches.
		/// </summary>
		/// <param name="showingFullFile">Determines if the entire file is displayed or just the match lines</param>
		/// <param name="removeWhiteSpace">Determines if the leading white space is removed or not</param>
		/// <param name="beforeContextLines">The number of before context lines</param>
		/// <param name="afterContextLines">The number of after context lines</param>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019  ADD: 135, support prev/next navigation of matches
		/// </history>
		private void ProcessDocumentForAnchors(bool showingFullFile, bool removeWhiteSpace, int beforeContextLines, int afterContextLines)
		{
			matchAnchors.Clear();

			MatchResult match = null;
			if (lstFileNames.SelectedItems.Count == 1)
			{
				match = __Grep.RetrieveMatchResult(int.Parse(lstFileNames.SelectedItems[0].SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text));
			}

			if (match == null)
			{
				// assuming all results are display, loop through all of them to find the anchors
				if (__Grep != null && __Grep.MatchResults != null && __Grep.MatchResults.Count > 0)
				{
					for (int i = 0; i < txtHits.Document.Lines.Count; i++)
					{
						string text = txtHits.Document.GetText(txtHits.Document.Lines[i]);
						int lineStartOffset = txtHits.Document.Lines[i].Offset;

						if (__Grep != null && __Grep.MatchResults.Count > 0)
						{
							foreach (MatchResult result in __Grep.MatchResults)
							{
								MatchResultLine matchLine = null;

								if (!result.File.FullName.Equals(text, StringComparison.OrdinalIgnoreCase))
								{
									foreach (var matchResultLine in result.GetDisplayMatches(beforeContextLines, afterContextLines))
									{
										string lineText = matchResultLine.Line;

										if (removeWhiteSpace)
										{
											if (matchResultLine.HasMatch)
											{
												lineText = lineText.Substring(Utils.GetValidLeadingSpaces(lineText, matchResultLine.Matches[0].StartPosition));
											}
											else
											{
												lineText = lineText.TrimStart();
											}
										}

										if (lineText.Equals(text))
										{
											matchLine = matchResultLine;
											break;
										}
									}
								}

								if (matchLine != null && matchLine.HasMatch)
								{
									int trimOffset = 0;

									if (removeWhiteSpace)
									{
										trimOffset = Utils.GetValidLeadingSpaces(matchLine.Line, matchLine.Matches[0].StartPosition);
									}

									for (int j = 0; j < matchLine.Matches.Count; j++)
									{
										int startPosition = matchLine.Matches[j].StartPosition;

										matchAnchors.Add(txtHits.Document.CreateAnchor(lineStartOffset + (startPosition - trimOffset)));
									}
								}
							}
						}
					}
				}
			}
			else
			{
				matchAnchors.AddRange(GetAnchorsForMatch(match, showingFullFile, removeWhiteSpace, beforeContextLines, afterContextLines));
			}
		}

		/// <summary>
		/// Displays the entire selected file and uses a syntax highlighter when available for that file extension.
		/// </summary>
		/// <param name="match">Currently selected MatchResult</param>
		/// <history>
		/// [Curtis_Beard]     04/08/2015  CHG: 20/21, display entire file and use syntax highlighter
		/// [Curtis_Beard]     05/18/2015  FIX: 71, use language text for message
		/// [LinkNet]          07/17/2017  FIX: Remove leading whitespace when displaying entire file
		/// [LinkNet]          08/01/2017  FIX: Resolved problem searching for spaces with "remove leading white space" option selected
		/// [Curtis_Beard]	  08/20/2019  CHG: 142, dynamically display context lines
		/// [Curtis_Beard]	  09/09/2019  CHG: plugin based matches are not supported
		/// </history>
		private void ProcessFileForDisplay(MatchResult match)
		{
			int beforeContextLines = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem);
			int afterContextLines = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem);

			var matches = match?.GetDisplayMatches(beforeContextLines, afterContextLines);

			if (matches == null || matches.Count == 0)
			{
				txtHits.Clear();
				txtHits.LineNumbers = null;
				return;
			}

			// A plugin based match is a specific format so don't attempt to display the entire file
			if (match.FromPlugin)
			{
				ProcessMatchForDisplay(match);
				return;
			}

			if ((match.File.Length > 1024000 || FilterItem.IsBinaryFile(match.File)) &&
			   MessageBox.Show(this, Language.GetGenericText("ResultsPreviewLargeBinaryFile"), ProductInformation.ApplicationName, MessageBoxButtons.YesNo,
			   MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
			{
				ProcessMatchForDisplay(match); // display just the results then and not the whole file.
				return;
			}

			txtHits.Clear();
			txtHits.LineNumbers = null;
			txtHits.ScrollToHome();

			var def = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(match.File.Extension);
			txtHits.SyntaxHighlighting = def;

			for (int i = txtHits.TextArea.TextView.LineTransformers.Count - 1; i >= 0; i--)
			{
				if (txtHits.TextArea.TextView.LineTransformers[i] is ResultHighlighter)
					txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
				else if (txtHits.TextArea.TextView.LineTransformers[i] is AllResultHighlighter)
					txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
			}
			var foreground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightForeColor);
			var background = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightBackColor);
			var nonForeground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsContextForeColor);
			txtHits.TextArea.TextView.LineTransformers.Add(new ResultHighlighter(match, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines, true)
			{
				MatchForeground = foreground,
				MatchBackground = background,
				NonMatchForeground = nonForeground
			});

			// convert MatchResultLine to LineNumber
			List<LineNumber> lineNumbers = new List<LineNumber>();
			foreach (MatchResultLine matchLine in matches)
			{
				lineNumbers.Add(new LineNumber()
				{
					ColumnNumber = matchLine.ColumnNumber,
					Number = matchLine.LineNumber,
					HasMatch = matchLine.HasMatch,
					FileFullName = match.File.FullName
				});
			}

			txtHits.LineNumbers = lineNumbers;
			txtHits.Encoding = match.DetectedEncoding;
			txtHits.Load(match.File.FullName);

			/*
			 * Remove leading whitespace when displaying entire file.
			 */
			if (RemoveWhiteSpaceMenuItem.Checked)
			{
				ICSharpCode.AvalonEdit.Document.DocumentLine textEditorLine;
				string textLine = string.Empty;

				using (txtHits.Document.RunUpdate())
				{
					for (int i = 1; i <= txtHits.Document.LineCount; i++)
					{
						textEditorLine = txtHits.Document.GetLineByNumber(i);
						textLine = txtHits.Document.GetText(textEditorLine.Offset, textEditorLine.Length);
						MatchResultLine matchLine = null;
						matchLine = (from m in matches where m.LineNumber == i select m).FirstOrDefault();

						if (matchLine != null && matchLine.HasMatch)
						{
							textLine = textLine.Substring(Utils.GetValidLeadingSpaces(textLine, matchLine.Matches[0].StartPosition));
						}
						else
						{
							textLine = textLine.TrimStart();
						}

						txtHits.Document.Replace(textEditorLine.Offset, textEditorLine.Length, textLine);
					}
				}
			}

			ProcessDocumentForAnchors(EntireFileMenuItem.Checked, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines);
		}

		/// <summary>
		/// Highlight the searched text in the results
		/// </summary>
		/// <param name="match">MatchResult containing results</param>
		/// <history>
		/// [Curtis_Beard]     01/27/2005  ADD: Created
		/// [Curtis_Beard]     04/12/2005  FIX: 1180741, Don't capitalize hit line
		/// [Curtis_Beard]     11/18/2005  ADD: custom highlight colors
		/// [Curtis_Beard]     12/06/2005  CHG: call WholeWordOnly from Grep class
		/// [Curtis_Beard]     04/21/2006  CHG: highlight regular expression searches
		/// [Curtis_Beard]     09/28/2006  FIX: use grep object for settings instead of gui items
		/// [Ed_Jakubowski]    05/20/2009  CHG: Skip highlight if hitCount = 0
		/// [Curtis_Beard]     01/24/2012  CHG: allow back color use again since using .Net v2+
		/// [Curtis_Beard]     10/27/2014  CHG: 85, remove leading white space, add newline for display, fix windows sounds for empty text
		/// [Curtis_Beard]     11/26/2014  FIX: don't highlight found text that is part of the spacer text
		/// [Curtis_Beard]     02/24/2015  CHG: remove hit line restriction until proper fix for long loading
		/// [Curtis_Beard]     03/04/2015  CHG: move standard code to function to cleanup this function.
		/// [Curtis_Beard]     03/05/2015  FIX: 64/35, clear text field before anything to not have left over content.
		/// [Curtis_Beard]     04/08/2015  CHG: 61, change from RichTextBox to AvalonEdit
		/// [Curtis_Beard]     07/06/2015  FIX: 78, display empty preview area when result is null or no matches
		/// [LinkNet]          08/01/2017  FIX: Resolved problem searching for spaces with "remove leading white space" option selected
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void ProcessMatchForDisplay(MatchResult match)
		{
			int beforeContextLines = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem);
			int afterContextLines = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem);

			var matches = match?.GetDisplayMatches(beforeContextLines, afterContextLines);

			if (matches == null || matches.Count == 0)
			{
				txtHits.Clear();
				txtHits.SyntaxHighlighting = null;
				txtHits.LineNumbers = null;
				return;
			}

			txtHits.Clear();
			txtHits.SyntaxHighlighting = null;
			txtHits.ScrollToHome();

			for (int i = txtHits.TextArea.TextView.LineTransformers.Count - 1; i >= 0; i--)
			{
				if (txtHits.TextArea.TextView.LineTransformers[i] is ResultHighlighter)
					txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
				else if (txtHits.TextArea.TextView.LineTransformers[i] is AllResultHighlighter)
					txtHits.TextArea.TextView.LineTransformers.RemoveAt(i);
			}
			var foreground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightForeColor);
			var background = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.HighlightBackColor);
			var nonForeground = Convertors.ConvertStringToSolidColorBrush(GeneralSettings.ResultsContextForeColor);
			txtHits.TextArea.TextView.LineTransformers.Add(new ResultHighlighter(match, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines)
			{
				MatchForeground = foreground,
				MatchBackground = background,
				NonMatchForeground = nonForeground
			});

			StringBuilder builder = new StringBuilder();
			var lineNumbers = new List<LineNumber>();
			var path = match.File.FullName;
			int max = matches.Count;
			for (int i = 0; i < max; i++)
			{
				string line = matches[i].Line;

				if (RemoveWhiteSpaceMenuItem.Checked)
				{
					if (matches[i].HasMatch)
					{
						line = line.Substring(Utils.GetValidLeadingSpaces(line, matches[i].Matches[0].StartPosition));
					}
					else
					{
						line = line.TrimStart();
					}
				}
				builder.AppendLine(line);
				lineNumbers.Add(new LineNumber()
				{
					Number = matches[i].LineNumber,
					HasMatch = matches[i].HasMatch,
					FileFullName = matches[i].LineNumber > -1 || match.FromPlugin ? path : string.Empty,
					ColumnNumber = matches[i].ColumnNumber
				});
			}

			// the last result will have a hanging newline, so remove it.
			if (builder.Length > 0)
			{
				builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
			}

			txtHits.LineNumbers = lineNumbers;
			txtHits.Text = builder.ToString();

			ProcessDocumentForAnchors(EntireFileMenuItem.Checked, RemoveWhiteSpaceMenuItem.Checked, beforeContextLines, afterContextLines);
		}

		/// <summary>
		/// Handles the Grep object's DirectoryFiltered event
		/// </summary>
		/// <param name="dir">DirectoryInfo object containg currently being searched directory</param>
		/// <param name="filterItem">FilterItem causing the filtering</param>
		/// <param name="filterValue">Value that caused the filtering</param>
		/// <history>
		/// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
		/// [Curtis_Beard]	   11/11/2014	CHG: more descriptive exclusion information
		/// </history>
		private void ReceiveDirectoryFiltered(System.IO.DirectoryInfo dir, FilterItem filterItem, string filterValue)
		{
			LogItems.Add(new LogItem(LogItem.LogItemTypes.Exclusion, dir.FullName, string.Format("{0}~~{1}", filterItem.ToString(), filterValue)));
			SetStatusBarFilterCount(GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion));
		}

		/// <summary>
		/// Handles the Grep object's FileEncodingDetected event
		/// </summary>
		/// <param name="file">FileInfo object containg currently being search file</param>
		/// <param name="encoding">File's detected System.Text.Encoding</param>
		/// <param name="usedEncoder">The used encoder's name</param>
		/// <history>
		/// [Curtis_Beard]		12/01/2014	Created
		/// [Curtis_Beard]		08/15/2017	FIX: 100, performance changes
		/// </history>
		private void ReceiveFileEncodingDetected(FileInfo file, System.Text.Encoding encoding, string usedEncoder)
		{
			LogItems.UpdateItemDetails(file, string.Format("{0} [{1}]", encoding.EncodingName, usedEncoder));
		}

		/// <summary>
		/// Handles the Grep object's FileFiltered event
		/// </summary>
		/// <param name="file">FileInfo object containg currently being search file</param>
		/// <param name="filterItem">FilterItem causing the filtering</param>
		/// <param name="filterValue">Value that caused the filtering</param>
		/// <history>
		/// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
		/// [Curtis_Beard]	   11/11/2014	CHG: more descriptive exclusion information
		/// </history>
		private void ReceiveFileFiltered(System.IO.FileInfo file, FilterItem filterItem, string filterValue)
		{
			LogItems.Add(new LogItem(LogItem.LogItemTypes.Exclusion, file.FullName, string.Format("{0}~~{1}", filterItem.ToString(), filterValue)));
			SetStatusBarFilterCount(GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion));
		}

		/// <summary>
		/// A file has been detected to contain the searching text
		/// </summary>
		/// <param name="file">File detected to contain searching text</param>
		/// <param name="index">Position in GrepCollection</param>
		/// <history>
		/// [Curtis_Beard]		10/17/2005	Created
		/// </history>
		private void ReceiveFileHit(System.IO.FileInfo file, int index)
		{
			AddHitToList(file, index);
			CalculateTotalCount();
		}

		/// <summary>
		/// A line has been detected to contain the searching text
		/// </summary>
		/// <param name="match">The MatchResult that contains the line</param>
		/// <param name="index">The position in the HitObject's line collection</param>
		/// <history>
		/// [Curtis_Beard]		11/04/2005   Created
		/// </history>
		private void ReceiveLineHit(MatchResult match, int index)
		{
			UpdateHitCount(match);
			//CalculateTotalCount();
		}

		/// <summary>
		/// Receives the search cancel event when the grep has been cancelled.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		07/12/2006	Created
		/// [Curtis_Beard]		08/07/2007  ADD: 1741735, display any search errors
		/// [Curtis_Beard]	    12/17/2014	ADD: support for Win7+ taskbar progress
		/// [Curtis_Beard]	    02/24/2015	CHG: remove isSearching check so that you can view selected file during a search
		/// [Curtis_Beard]		04/08/2015  CHG: 54, add check to show all the results after a search
		/// [Curtis_Beard]		05/26/2015  CHG: add stop search messsage to log with time
		/// [Curtis_Beard]	    03/05/2020	CHG:log search stats
		/// </history>
		private void ReceiveSearchCancel()
		{
			var stats = LogStopSearchMessage("cancel");
			string statsMsg = string.Format("{0} seconds, {1} files", stats.Item1, stats.Item2);

			RestoreTaskBarProgress();

			const string languageLookupText = "SearchCancelled";
			LogItems.Add(new LogItem(LogItem.LogItemTypes.Status, languageLookupText, string.Format("||{0}", statsMsg)));

			SetStatusBarMessage(string.Format("{0} [{1}]", Language.GetGenericText(languageLookupText), statsMsg));
			RemoveStatusBarProgress();
			SetSearchState(true);
			CalculateTotalCount();

			CheckShowAllResults();

			ShowExclusionsErrorMessageBox();
		}

		/// <summary>
		/// Receives the search complete event when the grep has completed.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		07/12/2006	Created
		/// [Curtis_Beard]		06/27/2007  CHG: removed message parameter
		/// [Curtis_Beard]		08/07/2007  ADD: 1741735, display any search errors
		/// [Ed_Jakubowski]		05/20/2009  ADD: Display the Count
		/// [Curtis_Beard]		01/30/2012  CHG: use language class for count text
		/// [Curtis_Beard]		04/08/2014	CHG: 74, command line exit, save options
		/// [Curtis_Beard]	    12/17/2014	ADD: support for Win7+ taskbar progress
		/// [Curtis_Beard]	    02/24/2015	CHG: only attempt file show when 1 item is selected (prevents flickering and loading on all deselect)
		/// [Curtis_Beard]	    02/24/2015	CHG: remove isSearching check so that you can view selected file during a search
		/// [Curtis_Beard]		04/08/2015  CHG: 54, add check to show all the results after a search
		/// [Curtis_Beard]		05/26/2015  CHG: add stop search messsage to log with time
		/// [Curtis_Beard]	    03/05/2020	CHG:log search stats
		/// </history>
		private void ReceiveSearchComplete()
		{
			var stats = LogStopSearchMessage("finished");
			string statsMsg = string.Format("{0} seconds, {1} files", stats.Item1, stats.Item2);

			RestoreTaskBarProgress();

			const string languageLookupText = "SearchFinished";
			LogItems.Add(new LogItem(LogItem.LogItemTypes.Status, languageLookupText, string.Format("||{0}", statsMsg)));

			SetStatusBarMessage(string.Format("{0} [{1}]", Language.GetGenericText(languageLookupText), statsMsg));
			RemoveStatusBarProgress();
			CalculateTotalCount();
			SetSearchState(true);

			CheckShowAllResults();

			ShowExclusionsErrorMessageBox();

			if (CommandLineArgs.AnyArguments)
			{
				if (!string.IsNullOrEmpty(CommandLineArgs.OutputPath))
				{
					MatchResultsExport.FileDelegate del = MatchResultsExport.SaveResultsAsText;
					switch (CommandLineArgs.OutputType)
					{
						case "xml":
							del = MatchResultsExport.SaveResultsAsXML;
							break;

						case "json":
							del = MatchResultsExport.SaveResultsAsJSON;
							break;

						case "html":
							del = MatchResultsExport.SaveResultsAsHTML;
							break;

						default:
						case "txt":
							del = MatchResultsExport.SaveResultsAsText;
							break;
					}

					OutputResults(CommandLineArgs.OutputPath, del);
				}

				if (CommandLineArgs.ExitAfterSearch)
				{
					Invoke(new Action(() =>
					{
						Close();
					}));
				}
			}
		}

		/// <summary>
		/// Receives the search error event when a file search causes an uncatchable error
		/// </summary>
		/// <param name="file">FileInfo object of error file</param>
		/// <param name="ex">Exception</param>
		/// <history>
		/// [Curtis_Beard]		03/14/2006	Created
		/// [Curtis_Beard]		05/28/2007  CHG: use Exception and display error
		/// [Curtis_Beard]		08/07/2007  ADD: 1741735, better search error handling
		/// [Curtis_Beard]		02/07/2012  CHG: 1741735, report full error message
		/// [Curtis_Beard]	   04/08/2015	CHG: add logging
		/// </history>
		private void ReceiveSearchError(System.IO.FileInfo file, Exception ex)
		{
			string languageLookupText = "SearchGenericError";

			if (file == null)
			{
				LogItems.Add(new LogItem(LogItem.LogItemTypes.Error, languageLookupText, string.Format("{0}||{1}", string.Empty, ex.Message)));
				LogClient.Instance.Logger.Error("Search error from grep: {0}", LogClient.GetAllExceptions(ex));
			}
			else
			{
				languageLookupText = "SearchFileError";
				LogItems.Add(new LogItem(LogItem.LogItemTypes.Error, languageLookupText, string.Format("{0}||{1}", file.FullName, ex.Message)));
				LogClient.Instance.Logger.Error("Search error from grep for file {0}: {1}", file.FullName, LogClient.GetAllExceptions(ex));
			}

			SetStatusBarErrorCount(GetLogItemsCountByType(LogItem.LogItemTypes.Error));
		}

		/// <summary>
		/// Handles the Grep object's SearchingFile event
		/// </summary>
		/// <param name="file">FileInfo object containg currently being search file</param>
		/// <history>
		/// [Curtis_Beard]		10/17/2005	Created
		/// [Curtis_Beard]		12/02/2005	CHG: handle SearchingFile event instead of StatusMessage
		/// [Curtis_Beard]		04/21/2006	CHG: truncate the file name if necessary
		/// [Curtis_Beard]	    08/15/2017	FIX: 100, performance changes (remove name truncation and just show current file name).
		/// [Curtis_Beard]	    03/05/2020	CHG: don't update status bar with current file for performance
		/// </history>
		private void ReceiveSearchingFile(FileInfo file)
		{
			const string languageLookupText = "SearchSearching";
			LogItems.Add(new LogItem(LogItem.LogItemTypes.Status, languageLookupText, file.FullName));

			//SetStatusBarMessage(string.Format(Language.GetGenericText(languageLookupText), file.Name));
		}

		/// <summary>
		/// Handles the Grep object's SearchingFileByPlugin event
		/// </summary>
		/// <param name="file">Current FileInfo</param>
		/// <param name="pluginName">Name of plugin currently searching file</param>
		/// <history>
		/// [Curtis_Beard]		10/16/2012	Created
		/// [Curtis_Beard]		08/21/2017	CHG: remove displaying plugin in status bar, adjust updating search file status message
		/// </history>
		private void ReceiveSearchingFileByPlugin(FileInfo file, string pluginName)
		{
			LogItems.UpdateItemDetails(file, pluginName);
		}

		/// <summary>
		/// Log the registry monitor errors.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RegistryMonitor_Error(object sender, ErrorEventArgs e)
		{
			var ex = e.GetException();
			if (ex != null)
			{
				LogClient.Instance.Logger.Warn(ex, $"Failure during registry monitoring for theme change: {ex.Message}");
			}
			else
			{
				LogClient.Instance.Logger.Warn($"Failure during registry monitoring for theme change: unknown error.");
			}
		}

		/// <summary>
		/// Handle the system theme registry key value change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RegistryMonitor_RegChanged(object sender, EventArgs e)
		{
			// change the actual theme provider
			ThemeProvider.ChangeThemeBySystem();

			// reload this form's theming
			ReloadTheme(ThemeProvider.ThemeType.System);
		}

		/// <summary>
		/// Reload this form based on the selected theme.
		/// </summary>
		/// <param name="themeType"></param>
		private void ReloadTheme(ThemeProvider.ThemeType themeType)
		{
			this.InvokeIfRequired(() =>
			{
				LoadTheme(this);

				toolTip1.Reset();

				// update text editor main colors based on theme change
				GeneralSettings.ResultsBackColor = Convertors.ConvertColorToString(ThemeProvider.Theme.Colors.Window);
				GeneralSettings.ResultsForeColor = Convertors.ConvertColorToString(ThemeProvider.Theme.Colors.ForeColor);

				// reload text editor with new theme colors above
				LoadTextEditorSettings();

				ResultsToolStrip.Renderer = ThemeProvider.Theme.ToolStripRenderer;
				MainMenuStrip.Renderer = ThemeProvider.Theme.MenuRenderer;
				TxtHitsContextMenuStrip.Renderer = ThemeProvider.Theme.MenuRenderer;
				fileLstMnu.Renderer = ThemeProvider.Theme.MenuRenderer;
				stbStatus.Reset();

				lblSearchHeading.ForeColor = ThemeProvider.Theme.Colors.ApplicationAccentColor;
				lblSearchOptions.ForeColor = ThemeProvider.Theme.Colors.ApplicationAccentColor;

				splitUpDown.BackColor = GeneralSettings.UseAstroGrepAccentColor ? ThemeProvider.Theme.Colors.ApplicationAccentColor : ThemeProvider.Theme.Colors.Control;
				splitLeftRight.BackColor = GeneralSettings.UseAstroGrepAccentColor ? ThemeProvider.Theme.Colors.ApplicationAccentColor : ThemeProvider.Theme.Colors.Control;

				SetStatusBarFilterCount(GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion));
				SetStatusBarErrorCount(GetLogItemsCountByType(LogItem.LogItemTypes.Error));

				// start up registry monitor for system theme detection change
				if (themeType == ThemeProvider.ThemeType.System)
				{
					if (!registryMonitor.IsMonitoring)
					{
						registryMonitor.Start();
					}
				}
				else
				{
					registryMonitor.Stop();
				}
			});
		}

		/// <summary>
		/// Remove the status bar search progress (Thread Safe)
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   05/18/2020	Created
		/// </history>
		private void RemoveStatusBarProgress()
		{
			stbStatus.InvokeIfRequired(() =>
			{
				if (stbStatus.Items.ContainsKey("SearchProgress"))
				{
					stbStatus.Items.RemoveByKey("SearchProgress");
				}
			});
		}

		/// <summary>
		/// Option to remove leading white space from each line.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	ADD: AvalonEdit view menu options
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void RemoveWhiteSpaceMenuItem_Click(object sender, System.EventArgs e)
		{
			RemoveWhiteSpaceMenuItem.Checked = !RemoveWhiteSpaceMenuItem.Checked;
			ResultsViewRemovingLeadingButton.Checked = RemoveWhiteSpaceMenuItem.Checked;

			if (lstFileNames.SelectedItems.Count > 0)
			{
				lstFileNames_SelectedIndexChanged(null, null);
			}
			else if (txtHits.Text.Length > 0)
			{
				ProcessAllMatchesForDisplay();
			}
		}

		/// <summary>
		/// Restore the Win7+ TaskBar progress state to normal.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   12/17/2014	ADD: support for Win7+ taskbar progress
		/// [Curtis_Beard]		07/06/2015  FIX: 78, add try/catch to resolve crash
		/// </history>
		private void RestoreTaskBarProgress()
		{
			this.InvokeIfRequired(() =>
			{
				try
				{
					API.TaskbarProgress.SetState(Handle, API.TaskbarProgress.TaskbarStates.NoProgress);
				}
				catch { }
			});
		}

		/// <summary>
		/// Update displayed results text with changed context line
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void ResultsContextLinesCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			lstFileNames_SelectedIndexChanged(sender, e);
		}

		/// <summary>
		/// Go to the next match within the results display
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019  ADD: 135, support prev/next navigation of matches
		/// </history>
		private void ResultsNavNextButton_Click(object sender, EventArgs e)
		{
			int nextOffset = -1;

			foreach (var anchor in matchAnchors)
			{
				if (txtHits.CaretOffset < anchor.Offset)
				{
					nextOffset = anchor.Offset;
					break;
				}
			}

			// wrap around
			if (nextOffset == -1 && matchAnchors.Count > 0)
			{
				nextOffset = matchAnchors[0].Offset;
			}

			if (nextOffset >= 0 && nextOffset <= txtHits.Document.TextLength)
			{
				txtHits.Select(nextOffset, 0);
				//txtHits.ScrollToHorizontalOffset(nextOffset + matchLength); // scroll into view
				txtHits.TextArea.Caret.BringCaretToView();
				txtHits.TextArea.Caret.Show();
			}
		}

		/// <summary>
		/// Go to the previous match within the results display
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019  ADD: 135, support prev/next navigation of matches
		/// </history>
		private void ResultsNavPrevButton_Click(object sender, EventArgs e)
		{
			int prevOffset = -1;

			for (int i = matchAnchors.Count - 1; i >= 0; i--)
			{
				if (txtHits.CaretOffset > matchAnchors[i].Offset)
				{
					prevOffset = matchAnchors[i].Offset;
					break;
				}
			}

			// wrap around
			if (prevOffset == -1 && matchAnchors.Count > 0)
			{
				prevOffset = matchAnchors[matchAnchors.Count - 1].Offset;
			}

			if (prevOffset >= 0 && prevOffset <= txtHits.Document.TextLength)
			{
				txtHits.Select(prevOffset, 0);
				//txtHits.ScrollToHorizontalOffset(nextOffset + matchLength); // scroll into view
				txtHits.TextArea.Caret.BringCaretToView();
				txtHits.TextArea.Caret.Show();
			}
		}

		/// <summary>
		/// Handle showing results tool strip item's tool tip.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResultsToolStripItem_MouseEnter(object sender, EventArgs e)
		{
			var item = sender as ToolStripItem;
			if (item is ToolStripButton || item is ToolStripComboBox)
			{
				var newPoint = PointToClient(Cursor.Position);
				toolTip1.Show(Language.GetControlToolTipText(item), this, newPoint.X, newPoint.Y);
			}
		}

		/// <summary>
		/// Handle hiding results tool strip item's tool tip.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResultsToolStripItem_MouseLeave(object sender, EventArgs e)
		{
			toolTip1.Hide(this);
		}

		/// <summary>
		/// Handle results toolbar view all results
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsViewAllResultsButton_Click(object sender, EventArgs e)
		{
			AllResultsMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar enable/disable viewing entire file
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsViewEntireFileButton_Click(object sender, EventArgs e)
		{
			EntireFileMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar enable/disable line numbers
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsViewLineNumbersButton_Click(object sender, EventArgs e)
		{
			LineNumbersMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar enable/disable removing leading white space
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsViewRemovingLeadingButton_Click(object sender, EventArgs e)
		{
			RemoveWhiteSpaceMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Option to show all characters in results viewer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]      09/09/2019  ADD: Show editor characters
		/// </history>
		private void ResultsViewShowCharactersButton_Click(object sender, EventArgs e)
		{
			ShowCharactersMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar enable/disable word wrap
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsViewWordWrapButton_Click(object sender, EventArgs e)
		{
			WordWrapMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar zoom in
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsZoomInButton_Click(object sender, EventArgs e)
		{
			ZoomInMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar zoom out
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsZoomOutButton_Click(object sender, EventArgs e)
		{
			ZoomOutMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Handle results toolbar zoom reset
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]      08/20/2019  Initial
		/// </history>
		private void ResultsZoomResetButton_Click(object sender, EventArgs e)
		{
			ZoomRestoreMenuItem_Click(sender, e);
		}

		/// <summary>
		/// Save the results to a file
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Curtis_Beard]	   01/11/2005	Initial
		/// [Curtis_Beard]	   10/14/2005	CHG: use No Results for message box title
		/// [Curtis_Beard]	   12/07/2005	CHG: Use column constant
		/// [Curtis_Beard]	   09/06/2006	CHG: Update to support html and xml output
		/// [Andrew_Radford]    20/09/2009	CHG: Use export class
		/// [Curtis_Beard]	   01/31/2012	CHG: show status bar message only once for text/html/xml
		/// </history>
		private void SaveResultsMenuItem_Click(object sender, System.EventArgs e)
		{
			// only show dialog if information to save
			if (lstFileNames.Items.Count <= 0)
			{
				MessageBox.Show(Language.GetGenericText("SaveNoResults"), ProductInformation.ApplicationName, MessageBoxButtons.OK,
								MessageBoxIcon.Information);
				return;
			}

			var dlg = new SaveFileDialog
			{
				CheckPathExists = true,
				AddExtension = true,
				Title = Language.GetGenericText("SaveDialogTitle"),
				Filter = "Text (*.txt)|*.txt|HTML (*.html)|*.html|XML (*.xml)|*.xml|JSON (*.json)|*.json"
			};

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				switch (dlg.FilterIndex)
				{
					case 1:
						// Save to text
						OutputResults(dlg.FileName, MatchResultsExport.SaveResultsAsText);
						break;

					case 2:
						// Save to html
						OutputResults(dlg.FileName, MatchResultsExport.SaveResultsAsHTML);
						break;

					case 3:
						// Save to xml
						OutputResults(dlg.FileName, MatchResultsExport.SaveResultsAsXML);
						break;

					case 4:
						// Save to json
						OutputResults(dlg.FileName, MatchResultsExport.SaveResultsAsJSON);
						break;
				}
			}
		}

		/// <summary>
		/// Save Search Settings Event
		/// </summary>
		/// <param name="sender">system parm</param>
		/// <param name="e">system parm</param>
		/// <history>
		/// [Curtis_Beard]	   01/28/2005	Created
		/// </history>
		private void SaveSearchOptionsMenuItem_Click(object sender, System.EventArgs e)
		{
			if (VerifyInterface())
			{
				SaveSearchSettings();
			}
		}

		/// <summary>
		/// Save the search options.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   10/11/2006	Created
		/// [Curtis_Beard]	   01/31/2012	ADD: 1561584, ability to skip hidden/system files/directories
		/// [Curtis_Beard]	   02/07/2012	FIX: 3485448, save modified start/end date, min/max file sizes
		/// [Curtis_Beard]      02/09/2012  ADD: 3424156, size drop down selection
		/// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
		/// [Curtis_Beard]	   02/04/2014	FIX: use NumericUpDown's Value property instead of text
		/// [Curtis_Beard]	   04/08/2015	CHG: 54, save show all results option
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private void SaveSearchSettings()
		{
			Core.SearchSettings.UseRegularExpressions = chkRegularExpressions.Checked;
			Core.SearchSettings.UseCaseSensitivity = chkCaseSensitive.Checked;
			Core.SearchSettings.UseWholeWordMatching = chkWholeWordOnly.Checked;
			Core.SearchSettings.IncludeLineNumbers = LineNumbersMenuItem.Checked;
			Core.SearchSettings.UseRecursion = chkRecurse.Checked;
			Core.SearchSettings.ReturnOnlyFileNames = chkFileNamesOnly.Checked;
			Core.SearchSettings.ContextLinesBefore = Convert.ToInt32(ResultsContextLinesBeforeCombo.SelectedItem);
			Core.SearchSettings.ContextLinesAfter = Convert.ToInt32(ResultsContextLinesAfterCombo.SelectedItem);
			Core.SearchSettings.UseNegation = chkNegation.Checked;
			Core.SearchSettings.ShowAllResultsAfterSearch = chkAllResultsAfterSearch.Checked;
			Core.SearchSettings.FilterItems = FilterItem.ConvertFilterItemsToString(__FilterItems);

			Core.SearchSettings.Save();
		}

		/// <summary>
		/// Save the general settings values.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   10/11/2006	Created
		/// [Curtis_Beard]	   01/31/2012	ADD: save size column width
		/// [Curtis_Beard]		10/27/2014	CHG: 88, add file extension column
		/// [Curtis_Beard]	   04/08/2015	CHG: 20/81, save word wrap and entire file options
		/// [Curtis_Beard]      09/09/2019  ADD: Show editor characters
		/// </history>
		private void SaveSettings()
		{
			SaveWindowSettings();

			//save column widths
			GeneralSettings.WindowFileColumnNameWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_FILE].Width;
			GeneralSettings.WindowFileColumnLocationWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_DIRECTORY].Width;
			GeneralSettings.WindowFileColumnDateWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_DATE].Width;
			GeneralSettings.WindowFileColumnCountWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_COUNT].Width;
			GeneralSettings.WindowFileColumnSizeWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_SIZE].Width;
			GeneralSettings.WindowFileColumnFileExtWidth = lstFileNames.Columns[Constants.COLUMN_INDEX_FILE_EXT].Width;

			//save divider panel positions
			GeneralSettings.WindowSearchPanelWidth = pnlSearch.Width;
			GeneralSettings.WindowFilePanelHeight = lstFileNames.Height;

			//save search comboboxes
			GeneralSettings.SearchStarts = Convertors.GetComboBoxEntriesAsString(cboFilePath);
			GeneralSettings.SearchFilters = fileFilterHelper.GetUserValuesAsString(cboFileName);
			GeneralSettings.SearchFiltersIndex = cboFileName.SelectedIndex;
			GeneralSettings.SearchTexts = Convertors.GetComboBoxEntriesAsString(cboSearchForText);

			//save view options
			GeneralSettings.ResultsWordWrap = WordWrapMenuItem.Checked;
			GeneralSettings.RemoveLeadingWhiteSpace = RemoveWhiteSpaceMenuItem.Checked;
			GeneralSettings.ShowEntireFile = EntireFileMenuItem.Checked;
			GeneralSettings.ShowEditorCharacters = ShowCharactersMenuItem.Checked;

			GeneralSettings.Save();
		}

		/// <summary>
		/// Save the window settings in the config.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   06/28/2007	Created
		/// </history>
		private void SaveWindowSettings()
		{
			if (WindowState == FormWindowState.Normal)
			{
				GeneralSettings.WindowLeft = Left;
				GeneralSettings.WindowTop = Top;
				GeneralSettings.WindowWidth = Width;
				GeneralSettings.WindowHeight = Height;
				GeneralSettings.WindowState = (int)WindowState;
			}
			else
			{
				// just save the state, so that previous normal dimensions are valid
				GeneralSettings.WindowState = (int)WindowState;
			}
		}

		/// <summary>
		/// Displays the error messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   09/27/2012	ADD: 1741735, better error handling display
		/// </history>
		private void sbErrorCountPanel_DoubleClick(object sender, EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Error);
		}

		/// <summary>
		/// Displays the exclusions messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   09/27/2012	ADD: 1741735, better error handling display
		/// </history>
		private void sbFilterCountPanel_DoubleClick(object sender, EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Exclusion);
		}

		/// <summary>
		/// Displays the status messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   09/27/2012	ADD: 1741735, better error handling display
		/// </history>
		private void sbStatusPanel_DoubleClick(object sender, EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Status);
		}

		/// <summary>
		/// Handles selecting all text in results preview area.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		04/08/2015	Initial
		/// </history>
		private void selectAllItem_Click(object sender, EventArgs e)
		{
			txtHits.Focus();
			txtHits.SelectAll();
		}

		/// <summary>
		/// Select all the list items.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]        09/28/2012  Initial: don't load each selected items file
		/// </history>
		private void SelectAllListItems()
		{
			foreach (ListViewItem lvi in lstFileNames.Items)
			{
				lvi.Selected = true;
			}
		}

		/// <summary>
		/// Menu Select All Event
		/// </summary>
		/// <param name="sender">System parm</param>
		/// <param name="e">System parm</param>
		/// <history>
		/// [Theodore_Ward]     ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   09/28/2012	CHG: use common select method
		/// </history>
		private void SelectAllMenuItem_Click(object sender, System.EventArgs e)
		{
			SelectAllListItems();
		}

		/// <summary>
		/// Select a folder for the search path.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/22/2006	Created
		/// </history>
		private void SelectPathMenuItem_Click(object sender, System.EventArgs e)
		{
			BrowseForFolder(false);
		}

		/// <summary>
		/// Sets the file list's columns' text to the correct language.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]		07/25/2006	Created
		/// [Curtis_Beard]	   01/31/2012	ADD: size column width/language
		/// [Curtis_Beard]		10/27/2014	CHG: 88, add file extension column
		/// </history>
		private void SetColumnsText()
		{
			if (lstFileNames.Columns.Count == 0)
			{
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnFile"), GeneralSettings.WindowFileColumnNameWidth, HorizontalAlignment.Left);
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnLocation"), GeneralSettings.WindowFileColumnLocationWidth, HorizontalAlignment.Left);
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnFileExt"), GeneralSettings.WindowFileColumnFileExtWidth, HorizontalAlignment.Left);
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnDate"), GeneralSettings.WindowFileColumnDateWidth, HorizontalAlignment.Left);
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnSize"), GeneralSettings.WindowFileColumnSizeWidth, HorizontalAlignment.Left);
				lstFileNames.Columns.Add(Language.GetGenericText("ResultsColumnCount"), GeneralSettings.WindowFileColumnCountWidth, HorizontalAlignment.Left);
			}
			else
			{
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE].Text = Language.GetGenericText("ResultsColumnFile");
				lstFileNames.Columns[Constants.COLUMN_INDEX_DIRECTORY].Text = Language.GetGenericText("ResultsColumnLocation");
				lstFileNames.Columns[Constants.COLUMN_INDEX_FILE_EXT].Text = Language.GetGenericText("ResultsColumnFileExt");
				lstFileNames.Columns[Constants.COLUMN_INDEX_SIZE].Text = Language.GetGenericText("ResultsColumnSize");
				lstFileNames.Columns[Constants.COLUMN_INDEX_DATE].Text = Language.GetGenericText("ResultsColumnDate");
				lstFileNames.Columns[Constants.COLUMN_INDEX_COUNT].Text = Language.GetGenericText("ResultsColumnCount");
			}

			AddContextMenuForResults();
		}

		/// <summary>
		/// Enable/Disable menu items (Thread safe)
		/// </summary>
		/// <param name="enable">True - enable menu items, False - disable</param>
		/// <history>
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   07/10/2006	CHG: Disable combo boxes during search
		/// [Curtis_Beard]	   07/12/2006	CHG: make thread safe
		/// [Curtis_Beard]	   07/25/2006	ADD: enable/disable context lines label
		/// [Curtis_Beard]	   10/30/2012	ADD: 28, search within results
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// [Curtis_Beard]	    10/27/2015	FIX: 78, adjust search in results
		/// </history>
		private void SetSearchState(bool enable)
		{
			this.InvokeIfRequired(() =>
			{
				FileMenu.Enabled = enable;
				EditMenu.Enabled = enable;
				ViewMenu.Enabled = enable;
				ToolsMenu.Enabled = enable;
				HelpMenu.Enabled = enable;

				btnSearch.Enabled = enable;
				btnCancel.Enabled = !enable;
				picBrowse.Enabled = enable;
				PanelOptionsContainer.Enabled = enable;

				cboFileName.Enabled = enable;
				cboFilePath.Enabled = enable;
				cboSearchForText.Enabled = enable;

				if (enable)
					btnSearch.Focus();
				else
					btnCancel.Focus();
			});
		}

		/// <summary>
		/// Set the status bar text for the encoding name. (Thread Safe)
		/// </summary>
		/// <param name="encodingName">Encoding display name</param>
		/// <history>
		/// [Curtis_Beard]	   03/03/2015	Created
		/// </history>
		private void SetStatusBarEncoding(string encodingName)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbEncodingPanel.Text = encodingName;

				// setup borders depending on value
				if (string.IsNullOrEmpty(encodingName))
				{
					sbEncodingPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
				}
				else
				{
					sbEncodingPanel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
				}
			});
		}

		/// <summary>
		/// Set the status bar text for the error count. (Thread Safe)
		/// </summary>
		/// <param name="count">Total number of errors</param>
		/// <history>
		/// [Curtis_Beard]		07/02/2007	Created
		/// [Curtis_Beard]		10/22/2012	CHG: use red background color to alert user
		/// [Curtis_Beard]		04/07/2016	CHG: use control for system color when no errors
		/// </history>
		private void SetStatusBarErrorCount(int count)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbErrorCountPanel.Text = string.Format(Language.GetGenericText("ResultsStatusErrorCount"), count);
				sbErrorCountPanel.BackColor = count > 0 ? Color.Red : Color.Transparent;
				sbErrorCountPanel.ForeColor = count > 0 ? Color.White : ThemeProvider.Theme.Colors.ForeColor;
			});
		}

		/// <summary>
		/// Set the status bar text for the file count. (Thread Safe)
		/// </summary>
		/// <param name="count">Total number of files</param>
		/// <history>
		/// [Curtis_Beard]	   07/02/2007	Created
		/// [Curtis_Beard]	   01/08/2019	CHG: 129, show total searched file count
		/// </history>
		private void SetStatusBarFileCount(int count)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbFileCountPanel.Text = string.Format(Language.GetGenericText("ResultsStatusFileCount"), count, __Grep != null ? __Grep.TotalFilesSearched : 0);
			});
		}

		/// <summary>
		/// Set the status bar text for the filter count. (Thread Safe)
		/// </summary>
		/// <param name="count">Total number of filtered items</param>
		/// <history>
		/// [Curtis_Beard]		09/26/2012	Created
		/// [Curtis_Beard]		10/22/2012	CHG: use yellow background color to alert user
		/// [Curtis_Beard]		04/07/2016	CHG: use control for system color when no errors
		/// </history>
		private void SetStatusBarFilterCount(int count)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbFilterCountPanel.Text = string.Format(Language.GetGenericText("ResultsStatusFilterCount"), count);
				sbFilterCountPanel.BackColor = count > 0 ? Color.Yellow : Color.Transparent;
				sbFilterCountPanel.ForeColor = count > 0 ? Color.Black : ThemeProvider.Theme.Colors.ForeColor;
			});
		}

		/// <summary>
		/// Set the status bar message text. (Thread Safe)
		/// </summary>
		/// <param name="message">Text to display</param>
		/// <history>
		/// [Curtis_Beard]		01/27/2007	Created
		/// </history>
		private void SetStatusBarMessage(string message)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbStatusPanel.Text = message;
			});
		}

		/// <summary>
		/// Set the status bar text for the total count. (Thread Safe)
		/// </summary>
		/// <param name="count">Total number of hits</param>
		/// <param name="lineCount">Total number of lines with hits</param>
		/// <history>
		/// [Curtis_Beard]	   01/27/2007	Created
		/// </history>
		private void SetStatusBarTotalCount(int count, int lineCount)
		{
			stbStatus.InvokeIfRequired(() =>
			{
				sbTotalCountPanel.Text = string.Format(Language.GetGenericText("ResultsStatusTotalCount"), count, lineCount);
			});
		}

		/// <summary>
		/// Sets the form's text to include the first entry of the search path's
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   09/18/2013	CHG: 64/53, add search path to window title
		/// [Curtis_Beard]		06/15/2015	CHG: 57, support external language files
		/// [Curtis_Beard]		07/09/2015	FIX: prevent repeating folder text
		/// </history>
		private void SetWindowText()
		{
			if (cboFilePath.Items.Count > 0)
			{
				Text = string.Format("{0} - {1}", cboFilePath.Items[0].ToString(), ProductInformation.ApplicationName);
			}
		}

		/// <summary>
		/// Option to show all characters in results viewer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]      09/09/2019  ADD: Show editor characters
		/// </history>
		private void ShowCharactersMenuItem_Click(object sender, EventArgs e)
		{
			ShowCharactersMenuItem.Checked = !ShowCharactersMenuItem.Checked;
			ResultsViewShowCharactersButton.Checked = ShowCharactersMenuItem.Checked;

			ToggleShowCharacters(ShowCharactersMenuItem.Checked);
		}

		/// <summary>
		/// Show a message box to the user if they have exclusions or errors and how to view them.  Can be disabled
		/// via the setting in the Options dialog.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]        10/05/2012  Initial: 1741735, show dialog to user about filter/error
		/// </history>
		private void ShowExclusionsErrorMessageBox()
		{
			this.InvokeIfRequired(() =>
			{
				// only show if not disabled by user and either filter count or error count is greater than 0
				if (GeneralSettings.ShowExclusionErrorMessage &&
				   (GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion) > 0 ||
				   GetLogItemsCountByType(LogItem.LogItemTypes.Error) > 0))
				{
					MessageBox.Show(this, Language.GetGenericText("ExclusionErrorMessageText"),
					ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			});
		}

		/// <summary>
		/// Start the searching
		/// </summary>
		/// <param name="searchWithInResults">true for searching within current results, false starts a new search</param>
		/// <history>
		/// [Curtis_Beard]		10/17/2005	Created
		/// [Curtis_Beard]		07/03/2006	FIX: 1516775, Remove trim on the search expression
		/// [Curtis_Beard]		07/12/2006	CHG: moved thread actions to grep class
		/// [Curtis_Beard]		11/22/2006	CHG: Remove use of browse in combobox
		/// [Curtis_Beard]		08/07/2007  ADD: 1741735, better search error handling
		/// [Curtis_Beard]		08/21/2007  FIX: 1778467, make sure file pattern is correct if a '\' is present
		/// [Curtis_Beard]		01/31/2012	CHG: 3424154/1816655, allow multiple starting directories
		/// [Curtis_Beard]		02/07/2012  CHG: 1741735, report full error message
		/// [Curtis_Beard]		02/24/2012	CHG: 3488322, use hand cursor for results view to signal click
		/// [Curtis_Beard]		10/30/2012	ADD: 28, search within results
		/// [Curtis_Beard]		12/01/2014	ADD: support for encoding detection event
		/// [Curtis_Beard]		12/17/2014	ADD: support for Win7+ taskbar progress
		/// [Curtis_Beard]		02/24/2015	CHG: remove isSearching check so that you can view selected file during a search
		/// [Curtis_Beard]		05/26/2015  CHG: add stop search messsage to log with time
		/// [Curtis_Beard]		04/12/2016  FIX: 78, use selected items for search in results if available
		/// [Curtis_Beard]	    09/29/2016	CHG: 24/115, use one interface for search in prep for saving to file
		/// [Curtis_Beard]	    03/05/2020	CHG: reset grep object on start, log search stats
		/// </history>
		private void StartSearch(bool searchWithInResults)
		{
			try
			{
				// reset
				if (__Grep != null)
				{
					__Grep.FileHit -= ReceiveFileHit;
					__Grep.LineHit -= ReceiveLineHit;
					__Grep.SearchCancel -= ReceiveSearchCancel;
					__Grep.SearchComplete -= ReceiveSearchComplete;
					__Grep.SearchError -= ReceiveSearchError;
					__Grep.SearchingFile -= ReceiveSearchingFile;
					__Grep.FileFiltered -= ReceiveFileFiltered;
					__Grep.DirectoryFiltered -= ReceiveDirectoryFiltered;
					__Grep.SearchingFileByPlugin -= ReceiveSearchingFileByPlugin;
					__Grep.FileEncodingDetected -= ReceiveFileEncodingDetected;

					__Grep = null;
				}

				// generate search specification from user interface
				var searchSpec = GetSearchSpecFromUI(searchWithInResults);

				// update combo selections
				AddComboSelection(cboSearchForText, cboSearchForText.Text);
				fileFilterHelper.AddUserValue(cboFileName, fileFilterHelper.GetSelectedEntry(cboFileName), GeneralSettings.MaximumMRUPaths);
				AddComboSelection(cboFilePath, cboFilePath.Text.Trim());

				SetWindowText();

				// disable gui
				SetSearchState(false);

				// reset display
				LogItems.Clear();
				SetStatusBarMessage(string.Format(Language.GetGenericText("SearchSearching"), "..."));
				SetStatusBarEncoding(string.Empty);
				SetStatusBarTotalCount(0, 0);
				SetStatusBarFileCount(0);
				SetStatusBarFilterCount(0);
				SetStatusBarErrorCount(0);

				// add a progress bar
				ToolStripProgressBar bar = new ToolStripProgressBar("SearchProgress") { Style = ProgressBarStyle.Marquee };
				stbStatus.Items.Insert(0, bar);

				ClearItems();
				txtHits.LineNumbers = null;
				txtHits.Clear();

				// create new grep instance
				__Grep = new Grep(searchSpec)
				{
					// add plugins
					Plugins = Core.PluginManager.Items
				};

				// attach events
				__Grep.FileHit += ReceiveFileHit;
				__Grep.LineHit += ReceiveLineHit;
				__Grep.SearchCancel += ReceiveSearchCancel;
				__Grep.SearchComplete += ReceiveSearchComplete;
				__Grep.SearchError += ReceiveSearchError;
				__Grep.SearchingFile += ReceiveSearchingFile;
				__Grep.FileFiltered += ReceiveFileFiltered;
				__Grep.DirectoryFiltered += ReceiveDirectoryFiltered;
				__Grep.SearchingFileByPlugin += ReceiveSearchingFileByPlugin;
				__Grep.FileEncodingDetected += ReceiveFileEncodingDetected;

				API.TaskbarProgress.SetState(Handle, API.TaskbarProgress.TaskbarStates.Indeterminate);
				LogItems.Add(new LogItem(LogItem.LogItemTypes.Status, "SearchStarted"));
				LogStartSearchMessage(searchSpec);
				StartingTime = Stopwatch.GetTimestamp();

				__Grep.BeginExecute();
			}
			catch (Exception ex)
			{
				var stats = LogStopSearchMessage("error");
				string statsMsg = string.Format("{0} seconds, {1} files", stats.Item1, stats.Item2);

				RestoreTaskBarProgress();
				LogItems.Add(new LogItem(LogItem.LogItemTypes.Error, "SearchGenericError", string.Format("{0}||{1},{2}", string.Empty, ex.Message, statsMsg)));
				LogClient.Instance.Logger.Error("Search error: {0}", LogClient.GetAllExceptions(ex));

				string message = string.Format(Language.GetGenericText("SearchGenericError"), ex.Message);
				MessageBox.Show(this, message, ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				SetStatusBarMessage(string.Format("{0} [{1}]", message, statsMsg));
				RemoveStatusBarProgress();
				SetSearchState(true);
				CalculateTotalCount();
			}
		}

		/// <summary>
		/// View status messages.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/11/2014	Initial
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void StatusMessageMenuItem_Click(object sender, System.EventArgs e)
		{
			DisplaySearchMessages(LogItem.LogItemTypes.Status);
		}

		/// <summary>
		/// Toggles the showing of all characters in the results viewer.
		/// </summary>
		/// <param name="showCharacters">True to show all characters, False to hide the control based characters</param>
		/// <history>
		/// [Curtis_Beard]      09/09/2019  ADD: Show editor characters
		/// </history>
		private void ToggleShowCharacters(bool showCharacters)
		{
			txtHits.Options.ShowTabs = showCharacters;
			txtHits.Options.ShowSpaces = showCharacters;
			txtHits.Options.ShowEndOfLine = showCharacters;
		}

		/// <summary>
		/// Handle showing a <see cref="ContextMenuStrip"/> for the results editor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]      08/26/2022  Created
		/// </history>
		private void TxtHits_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var point = e.GetPosition((System.Windows.IInputElement)e.Source);
			TxtHitsContextMenuStrip.Show(textElementHost, new Point((int)point.X, (int)point.Y));
		}

		/// <summary>
		/// Handles finding moving the focus to the next/previous control when tab is pressed.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		09/03/2019	CHG: 141, add support for tab/shift-tab in results view
		/// </history>
		private void txtHits_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.KeyboardDevice.IsKeyDown(System.Windows.Input.Key.Tab))
			{
				if (e.KeyboardDevice.IsKeyDown(System.Windows.Input.Key.LeftShift) || e.KeyboardDevice.IsKeyDown(System.Windows.Input.Key.RightShift))
				{
					lstFileNames.Focus();
				}
				else
				{
					FindForm().SelectNextControl(this, true, true, true, true);
				}
				e.Handled = true;
			}
		}

		/// <summary>
		/// Handles finding the current line under the mouse pointer during a double click to open the texteditor at that location.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		06/29/2015	FIX: 77, add back support for double click to open editor
		/// </history>
		private void txtHits_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
			{
				ICSharpCode.AvalonEdit.TextViewPosition? position = null;
				var mousePos = e.GetPosition(txtHits);
				if (mousePos != null)
				{
					position = txtHits.GetPositionFromPoint(mousePos);
				}

				var opener = GetEditorAtLocation(position);
				if (opener.HasValue())
				{
					e.Handled = true;
					TextEditors.Open(opener);
				}
			}
		}

		/// <summary>
		/// Disable the first <see cref="ContextMenuStrip"/> item for the results editor
		/// if no text editor is available for this line.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]      08/26/2022  Created
		/// </history>
		private void TxtHitsContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// enable open at menu item
			var opener = GetEditorAtLocation(txtHits.GetPositionFromRightClickPoint());
			(TxtHitsContextMenuStrip.Items[0] as ToolStripMenuItem).Enabled = opener.HasValue();
		}

		/// <summary>
		/// Updates the count column (Thread safe)
		/// </summary>
		/// <param name="match">MatchResult that contains updated information</param>
		/// <history>
		/// [Curtis_Beard]		11/21/2005  Created
		/// [Curtis_Beard]		01/08/2019	CHG: 119, add line hit count
		/// </history>
		private void UpdateHitCount(MatchResult match)
		{
			lstFileNames.InvokeIfRequired(() =>
			{
				// find correct item to update
				foreach (ListViewItem item in lstFileNames.Items)
				{
					if (int.Parse(item.SubItems[Constants.COLUMN_INDEX_GREP_INDEX].Text) == match.Index)
					{
						item.SubItems[Constants.COLUMN_INDEX_COUNT].Text = string.Format("{0} / {1}", match.HitCount, match.LineHitCount);
						break;
					}
				}
			});
		}

		/// <summary>
		/// Verify user selected options
		/// </summary>
		/// <returns>True - Verified, False - Otherwise</returns>
		/// <history>
		/// [Theodore_Ward]   ??/??/????  Initial
		/// [Curtis_Beard]	   01/11/2005	.Net Conversion
		/// [Curtis_Beard]	   10/14/2005	CHG: Use max context lines constant in message
		/// [Ed_Jakubowski]	   05/20/2009  CHG: Allow filename only searching
		/// [Curtis_Beard]	   01/31/2012	CHG: 3424154/1816655, allow multiple starting directories
		/// [Curtis_Beard]	   08/01/2012	FIX: 3553252, use | character for path delimitation character
		/// [Curtis_Beard]	   09/27/2012	FIX: 1881938, validate regular expression
		/// [Curtis_Beard]	   02/12/2014	ADD: check for empty minimum file count, use tryparse instead of try/catch for context lines
		/// [Curtis_Beard]	   10/27/2015	FIX: 78, adjust search in results
		/// [Curtis_Beard]	   01/31/2019	CHG: 139, expand environment variables within search path(s)
		/// [Curtis_Beard]	   08/20/2019  CHG: 142, dynamically display context lines
		/// </history>
		private bool VerifyInterface()
		{
			try
			{
				int _lines = -1;
				if (!int.TryParse(ResultsContextLinesBeforeCombo.SelectedItem.ToString(), out _lines) || _lines < 0 || _lines > Constants.MAX_CONTEXT_LINES)
				{
					MessageBox.Show(string.Format(Language.GetGenericText("VerifyErrorContextLines"), 0, Constants.MAX_CONTEXT_LINES.ToString()),
					   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				_lines = -1;
				if (!int.TryParse(ResultsContextLinesAfterCombo.SelectedItem.ToString(), out _lines) || _lines < 0 || _lines > Constants.MAX_CONTEXT_LINES)
				{
					MessageBox.Show(string.Format(Language.GetGenericText("VerifyErrorContextLines"), 0, Constants.MAX_CONTEXT_LINES.ToString()),
					   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				if (fileFilterHelper.GetSelectedFilter(cboFileName).Trim().Equals(string.Empty))
				{
					MessageBox.Show(Language.GetGenericText("VerifyErrorFileType"),
					   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				if (cboFilePath.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Language.GetGenericText("VerifyErrorNoStartPath"),
					   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				string[] paths = cboFilePath.Text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string path in paths)
				{
					if (!System.IO.Directory.Exists(Environment.ExpandEnvironmentVariables(path)))
					{
						MessageBox.Show(string.Format(Language.GetGenericText("VerifyErrorInvalidStartPath"), path),
						   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}

				if (chkSearchInResults.Checked && lstFileNames.Items.Count == 0)
				{
					MessageBox.Show(Language.GetGenericText("VerifyErrorNoResultsToSearchIn"),
					   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				//if (cboSearchForText.Text.Trim().Equals(string.Empty))
				//{
				//   MessageBox.Show(Language.GetGenericText("VerifyErrorNoSearchText"),
				//      Constants.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				//   return false;
				//}

				if (chkRegularExpressions.Checked && !cboSearchForText.Text.Trim().Equals(string.Empty))
				{
					// test reg ex
					try
					{
						var reg = new Regex(cboSearchForText.Text, RegexOptions.IgnoreCase);
					}
					catch (Exception ex)
					{
						MessageBox.Show(string.Format(Language.GetGenericText("VerifyErrorInvalidRegEx"), ex.Message),
						   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
			}
			catch
			{
				MessageBox.Show(Language.GetGenericText("VerifyErrorGeneric"),
				   ProductInformation.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Shows help file (.chm) to user.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/06/2014	Initial
		/// [Curtis_Beard]		05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// [Curtis_Beard]		06/02/2015	CHG: use Common code to get url
		/// </history>
		private void ViewHelpMenuItem_Click(object sender, EventArgs e)
		{
			//Future?: support currently selected language help file (AstroGrep-Help-en-us.chm, AstroGrep-Help-da-dk.chm, etc.)
			//Help.ShowHelp(this, Path.Combine(Constants.ProductLocation, "AstroGrep-Help.chm"));

			System.Diagnostics.Process.Start(ProductInformation.HelpUrl);
		}

		/// <summary>
		/// Enables view menu items when necessary.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   11/11/2014	Initial
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ViewMenu_DropDownOpening(object sender, EventArgs e)
		{
			StatusMessageMenuItem.Enabled = GetLogItemsCountByType(LogItem.LogItemTypes.Status) > 0;
			ExclusionMessageMenuItem.Enabled = GetLogItemsCountByType(LogItem.LogItemTypes.Exclusion) > 0;
			ErrorMessageMenuItem.Enabled = GetLogItemsCountByType(LogItem.LogItemTypes.Error) > 0;
			AllMessageMenuItem.Enabled = AnyLogItems();
		}

		/// <summary>
		/// Shows regular expression help to user.
		/// </summary>
		/// /// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]		05/06/2014	Initial
		/// [Curtis_Beard]		05/14/2015	CHG: use https version of url, use ToolStripMenuItem instead of MenuItem
		/// [Curtis_Beard]		06/02/2015	CHG: use Common code to get url
		/// </history>
		private void ViewRegExHelpMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(ProductInformation.RegExHelpUrl);
		}

		/// <summary>
		/// Option to toggle word wrapping.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	ADD: AvalonEdit view menu options
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void WordWrapMenuItem_Click(object sender, System.EventArgs e)
		{
			WordWrapMenuItem.Checked = !WordWrapMenuItem.Checked;
			ResultsViewWordWrapButton.Checked = WordWrapMenuItem.Checked;

			txtHits.WordWrap = WordWrapMenuItem.Checked;
		}

		/// <summary>
		/// Zoom in view by 1.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   05/11/2015	CHG: zoom for TextEditor control
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ZoomInMenuItem_Click(object sender, EventArgs e)
		{
			txtHits.ZoomIn();
		}

		/// <summary>
		/// Zoom out view by 1.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   05/11/2015	CHG: zoom for TextEditor control
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ZoomOutMenuItem_Click(object sender, EventArgs e)
		{
			txtHits.ZoomOut();
		}

		/// <summary>
		/// Reset zoom level to initial value.
		/// </summary>
		/// <param name="sender">system parameter</param>
		/// <param name="e">system parameter</param>
		/// <history>
		/// [Curtis_Beard]	   05/11/2015	CHG: zoom for TextEditor control
		/// [Curtis_Beard]	   05/14/2015	CHG: use ToolStripMenuItem instead of MenuItem
		/// </history>
		private void ZoomRestoreMenuItem_Click(object sender, EventArgs e)
		{
			txtHits.ZoomReset();
		}
	}
}