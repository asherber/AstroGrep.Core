using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace AstroGrep.Windows.Forms
{
   public partial class frmMain
   {
      #region Windows Form Designer generated code
	  private System.ComponentModel.IContainer components;
      private System.Windows.Forms.ListView lstFileNames;
      private AstroGrep.Windows.Controls.TextEditorEx txtHits;
      private System.Windows.Forms.Integration.ElementHost textElementHost;
      private System.Windows.Forms.Panel pnlSearch;
      private System.Windows.Forms.ComboBox cboSearchForText;
      private AstroGrep.Windows.Controls.ComboBoxEx cboFileName;
      private System.Windows.Forms.ComboBox cboFilePath;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnSearch;
      private System.Windows.Forms.Panel pnlSearchOptions;
      private System.Windows.Forms.Panel pnlMainSearch;
      private System.Windows.Forms.Label lblSearchText;
      private System.Windows.Forms.Label lblFileTypes;
      private System.Windows.Forms.Label lblSearchPath;
      private System.Windows.Forms.Label lblSearchHeading;
      private System.Windows.Forms.Splitter splitUpDown;
      private System.Windows.Forms.Splitter splitLeftRight;
      private System.Windows.Forms.Panel pnlRightSide;
      private System.Windows.Forms.StatusStrip stbStatus;
      private System.Windows.Forms.ToolStripStatusLabel sbStatusPanel;
      private System.Windows.Forms.ToolStripStatusLabel sbEncodingPanel;
      private System.Windows.Forms.ToolStripStatusLabel sbTotalCountPanel;
      private System.Windows.Forms.ToolStripStatusLabel sbFileCountPanel;
      private System.Windows.Forms.ToolStripStatusLabel sbFilterCountPanel;
      private System.Windows.Forms.ToolStripStatusLabel sbErrorCountPanel;
      private System.Windows.Forms.Label lblSearchOptions;
      private System.Windows.Forms.LinkLabel lnkExclusions;
      private System.Windows.Forms.ToolTip toolTip1;
      private System.Windows.Forms.Panel PanelOptionsContainer;
      private System.Windows.Forms.CheckBox chkNegation;
      private System.Windows.Forms.CheckBox chkCaseSensitive;
      private System.Windows.Forms.CheckBox chkRecurse;
      private System.Windows.Forms.CheckBox chkFileNamesOnly;
      private System.Windows.Forms.CheckBox chkRegularExpressions;
      private System.Windows.Forms.CheckBox chkSearchInResults;
      private System.Windows.Forms.CheckBox chkWholeWordOnly;
      private System.Windows.Forms.MenuItem menuItem4;
      private System.Windows.Forms.MenuItem CopyMenuItem;
      private System.Windows.Forms.MenuItem OpenMenuItem;
      private System.Windows.Forms.MenuItem DeleteMenuItem;
      private System.Windows.Forms.MenuItem OpenFolderMenuItem;
      private System.Windows.Forms.MenuItem menuItem2;
      private System.Windows.Forms.MenuItem CopyNameMenuItem;
      private System.Windows.Forms.MenuItem CopyLocatedInMenuItem;
      private System.Windows.Forms.MenuItem CopyLocatedInAndNameMenuItem;
      private System.Windows.Forms.MenuItem OpenWithAssociatedApp;
      private System.Windows.Forms.MenuItem FileOperationsMenuItem;
      private System.Windows.Forms.MenuItem FileCopyMenuItem;
      private System.Windows.Forms.MenuItem FileDeleteMenuItem;
      private System.Windows.Forms.MenuItem menuItem6;
      private System.Windows.Forms.ImageList ListViewImageList;
      private AstroGrep.Windows.Controls.PictureButton picBrowse;      
      private System.Windows.Forms.ContextMenu fileLstMnu;      
      private System.Windows.Forms.CheckBox chkAllResultsAfterSearch;      

      private MenuStrip MainMenu;

      private ToolStripMenuItem FileMenu;
      private ToolStripMenuItem NewWindowMenuItem;
      private ToolStripSeparator FileSep1Separator;
      private ToolStripMenuItem SelectPathMenuItem;
      private ToolStripSeparator FileSep2Separator;
      private ToolStripMenuItem SaveResultsMenuItem;
      private ToolStripMenuItem PrintResultsMenuItem;
      private ToolStripSeparator FileSep3Separator;
      private ToolStripMenuItem ExitMenuItem;

      private ToolStripMenuItem EditMenu;
      private ToolStripMenuItem SelectAllMenuItem;
      private ToolStripMenuItem OpenSelectedMenuItem;

      private ToolStripMenuItem ViewMenu;
      private ToolStripMenuItem StatusMessageMenuItem;
      private ToolStripMenuItem ExclusionMessageMenuItem;
      private ToolStripMenuItem ErrorMessageMenuItem;
      private ToolStripMenuItem AllMessageMenuItem;
      private ToolStripSeparator ViewSep1Separator;
      private ToolStripMenuItem ZoomMenuItem;
      private ToolStripMenuItem ZoomInMenuItem;
      private ToolStripMenuItem ZoomOutMenuItem;
      private ToolStripMenuItem ZoomRestoreMenuItem;
      private ToolStripMenuItem LineNumbersMenuItem;
      private ToolStripMenuItem WordWrapMenuItem;
      private ToolStripMenuItem RemoveWhiteSpaceMenuItem;
      private ToolStripMenuItem EntireFileMenuItem;
      private ToolStripSeparator ViewSep2Separator;
      private ToolStripMenuItem AllResultsMenuItem;

      private ToolStripMenuItem ToolsMenu;
      private ToolStripMenuItem ClearMRUMenuItem;
      private ToolStripMenuItem ClearMRUPathsMenuItem;
      private ToolStripMenuItem ClearMRUTypesMenuItem;
      private ToolStripMenuItem ClearMRUTextsMenuItem;
      private ToolStripSeparator ClearMRUSep1Separator;
      private ToolStripMenuItem ClearMRUAllMenuItem;
      private ToolStripSeparator ToolsSep1Separator;
      private ToolStripMenuItem SaveSearchOptionsMenuItem;
      private ToolStripMenuItem OptionsMenuItem;

      private ToolStripMenuItem HelpMenu;
      private ToolStripMenuItem ViewHelpMenuItem;
      private ToolStripMenuItem ViewRegExHelpMenuItem;
      private ToolStripMenuItem LogFileMenuItem;
      private ToolStripSeparator HelpSep1Separator;
      private ToolStripMenuItem CheckForUpdateMenuItem;
      private ToolStripSeparator HelpSep2Separator;
      private ToolStripMenuItem AboutMenuItem;

      private void InitializeComponent()
      {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.pnlSearchOptions = new System.Windows.Forms.Panel();
			this.PanelOptionsContainer = new System.Windows.Forms.Panel();
			this.lnkPlugins = new System.Windows.Forms.LinkLabel();
			this.lnkExclusions = new System.Windows.Forms.LinkLabel();
			this.chkAllResultsAfterSearch = new System.Windows.Forms.CheckBox();
			this.chkWholeWordOnly = new System.Windows.Forms.CheckBox();
			this.chkRegularExpressions = new System.Windows.Forms.CheckBox();
			this.chkSearchInResults = new System.Windows.Forms.CheckBox();
			this.chkNegation = new System.Windows.Forms.CheckBox();
			this.chkFileNamesOnly = new System.Windows.Forms.CheckBox();
			this.chkRecurse = new System.Windows.Forms.CheckBox();
			this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
			this.lblSearchOptions = new System.Windows.Forms.Label();
			this.pnlMainSearch = new System.Windows.Forms.Panel();
			this.picBrowse = new AstroGrep.Windows.Controls.PictureButton();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cboFilePath = new System.Windows.Forms.ComboBox();
			this.cboFileName = new AstroGrep.Windows.Controls.ComboBoxEx();
			this.cboSearchForText = new System.Windows.Forms.ComboBox();
			this.lblSearchText = new System.Windows.Forms.Label();
			this.lblFileTypes = new System.Windows.Forms.Label();
			this.lblSearchPath = new System.Windows.Forms.Label();
			this.lblSearchHeading = new System.Windows.Forms.Label();
			this.pnlRightSide = new System.Windows.Forms.Panel();
			this.textElementHost = new System.Windows.Forms.Integration.ElementHost();
			this.txtHits = new AstroGrep.Windows.Controls.TextEditorEx();
			this.ResultsToolStrip = new System.Windows.Forms.ToolStrip();
			this.ResultsZoomInButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsZoomOutButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsZoomResetButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ResultsViewLineNumbersButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsViewWordWrapButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsViewRemovingLeadingButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsViewEntireFileButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsViewShowCharactersButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsViewAllResultsButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.ResultsContextLinesLabel = new System.Windows.Forms.ToolStripLabel();
			this.ResultsContextLinesBeforeCombo = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.ResultsContextLinesAfterCombo = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.ResultsNavNextButton = new System.Windows.Forms.ToolStripButton();
			this.ResultsNavPrevButton = new System.Windows.Forms.ToolStripButton();
			this.splitUpDown = new System.Windows.Forms.Splitter();
			this.lstFileNames = new System.Windows.Forms.ListView();
			this.fileLstMnu = new System.Windows.Forms.ContextMenu();
			this.CopyMenuItem = new System.Windows.Forms.MenuItem();
			this.CopyNameMenuItem = new System.Windows.Forms.MenuItem();
			this.CopyLocatedInMenuItem = new System.Windows.Forms.MenuItem();
			this.CopyLocatedInAndNameMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.FileOperationsMenuItem = new System.Windows.Forms.MenuItem();
			this.FileCopyMenuItem = new System.Windows.Forms.MenuItem();
			this.FileDeleteMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.OpenMenuItem = new System.Windows.Forms.MenuItem();
			this.OpenFolderMenuItem = new System.Windows.Forms.MenuItem();
			this.OpenWithAssociatedApp = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.DeleteMenuItem = new System.Windows.Forms.MenuItem();
			this.ListViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.splitLeftRight = new System.Windows.Forms.Splitter();
			this.stbStatus = new System.Windows.Forms.StatusStrip();
			this.sbStatusPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbEncodingPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbTotalCountPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbFileCountPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbFilterCountPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbErrorCountPanel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NewWindowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FileSep1Separator = new System.Windows.Forms.ToolStripSeparator();
			this.SelectPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FileSep2Separator = new System.Windows.Forms.ToolStripSeparator();
			this.SaveResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PrintResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FileSep3Separator = new System.Windows.Forms.ToolStripSeparator();
			this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.StatusMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExclusionMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ErrorMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AllMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewSep1Separator = new System.Windows.Forms.ToolStripSeparator();
			this.ZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ZoomInMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ZoomOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ZoomRestoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LineNumbersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.WordWrapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveWhiteSpaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EntireFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ShowCharactersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewSep2Separator = new System.Windows.Forms.ToolStripSeparator();
			this.AllResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearMRUMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearMRUPathsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearMRUTypesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearMRUTextsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearMRUSep1Separator = new System.Windows.Forms.ToolStripSeparator();
			this.ClearMRUAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolsSep1Separator = new System.Windows.Forms.ToolStripSeparator();
			this.SaveSearchOptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ViewRegExHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LogFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpSep1Separator = new System.Windows.Forms.ToolStripSeparator();
			this.CheckForUpdateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpSep2Separator = new System.Windows.Forms.ToolStripSeparator();
			this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlSearch.SuspendLayout();
			this.pnlSearchOptions.SuspendLayout();
			this.PanelOptionsContainer.SuspendLayout();
			this.pnlMainSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBrowse)).BeginInit();
			this.pnlRightSide.SuspendLayout();
			this.ResultsToolStrip.SuspendLayout();
			this.stbStatus.SuspendLayout();
			this.MainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlSearch
			// 
			this.pnlSearch.AutoScroll = true;
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.pnlSearchOptions);
			this.pnlSearch.Controls.Add(this.pnlMainSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlSearch.Location = new System.Drawing.Point(0, 24);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(290, 513);
			this.pnlSearch.TabIndex = 0;
			this.pnlSearch.SizeChanged += new System.EventHandler(this.pnlSearch_SizeChanged);
			// 
			// pnlSearchOptions
			// 
			this.pnlSearchOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSearchOptions.Controls.Add(this.PanelOptionsContainer);
			this.pnlSearchOptions.Controls.Add(this.lblSearchOptions);
			this.pnlSearchOptions.Location = new System.Drawing.Point(0, 210);
			this.pnlSearchOptions.Name = "pnlSearchOptions";
			this.pnlSearchOptions.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this.pnlSearchOptions.Size = new System.Drawing.Size(290, 303);
			this.pnlSearchOptions.TabIndex = 1;
			// 
			// PanelOptionsContainer
			// 
			this.PanelOptionsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PanelOptionsContainer.Controls.Add(this.lnkPlugins);
			this.PanelOptionsContainer.Controls.Add(this.lnkExclusions);
			this.PanelOptionsContainer.Controls.Add(this.chkAllResultsAfterSearch);
			this.PanelOptionsContainer.Controls.Add(this.chkWholeWordOnly);
			this.PanelOptionsContainer.Controls.Add(this.chkRegularExpressions);
			this.PanelOptionsContainer.Controls.Add(this.chkSearchInResults);
			this.PanelOptionsContainer.Controls.Add(this.chkNegation);
			this.PanelOptionsContainer.Controls.Add(this.chkFileNamesOnly);
			this.PanelOptionsContainer.Controls.Add(this.chkRecurse);
			this.PanelOptionsContainer.Controls.Add(this.chkCaseSensitive);
			this.PanelOptionsContainer.Location = new System.Drawing.Point(0, 40);
			this.PanelOptionsContainer.Name = "PanelOptionsContainer";
			this.PanelOptionsContainer.Size = new System.Drawing.Size(290, 251);
			this.PanelOptionsContainer.TabIndex = 1;
			// 
			// lnkPlugins
			// 
			this.lnkPlugins.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.lnkPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkPlugins.BackColor = System.Drawing.SystemColors.Window;
			this.lnkPlugins.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkPlugins.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
			this.lnkPlugins.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.lnkPlugins.Location = new System.Drawing.Point(21, 200);
			this.lnkPlugins.Name = "lnkPlugins";
			this.lnkPlugins.Size = new System.Drawing.Size(269, 20);
			this.lnkPlugins.TabIndex = 14;
			this.lnkPlugins.TabStop = true;
			this.lnkPlugins.Text = "Plugins...";
			this.lnkPlugins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lnkPlugins.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPlugins_LinkClicked);
			// 
			// lnkExclusions
			// 
			this.lnkExclusions.ActiveLinkColor = System.Drawing.SystemColors.HotTrack;
			this.lnkExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkExclusions.BackColor = System.Drawing.SystemColors.Window;
			this.lnkExclusions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkExclusions.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
			this.lnkExclusions.LinkColor = System.Drawing.SystemColors.HotTrack;
			this.lnkExclusions.Location = new System.Drawing.Point(21, 224);
			this.lnkExclusions.Name = "lnkExclusions";
			this.lnkExclusions.Size = new System.Drawing.Size(269, 20);
			this.lnkExclusions.TabIndex = 15;
			this.lnkExclusions.TabStop = true;
			this.lnkExclusions.Text = "Exclusions...";
			this.lnkExclusions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lnkExclusions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExclusions_LinkClicked);
			// 
			// chkAllResultsAfterSearch
			// 
			this.chkAllResultsAfterSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAllResultsAfterSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkAllResultsAfterSearch.Location = new System.Drawing.Point(23, 176);
			this.chkAllResultsAfterSearch.Name = "chkAllResultsAfterSearch";
			this.chkAllResultsAfterSearch.Size = new System.Drawing.Size(267, 20);
			this.chkAllResultsAfterSearch.TabIndex = 13;
			this.chkAllResultsAfterSearch.Text = "&Show all results after search";
			this.toolTip1.SetToolTip(this.chkAllResultsAfterSearch, "Shows all the results together in the preview after a search.");
			// 
			// chkWholeWordOnly
			// 
			this.chkWholeWordOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkWholeWordOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkWholeWordOnly.Location = new System.Drawing.Point(23, 80);
			this.chkWholeWordOnly.Name = "chkWholeWordOnly";
			this.chkWholeWordOnly.Size = new System.Drawing.Size(267, 20);
			this.chkWholeWordOnly.TabIndex = 9;
			this.chkWholeWordOnly.Text = "&Whole Word";
			this.toolTip1.SetToolTip(this.chkWholeWordOnly, "Only match entire words (not parts of words)");
			// 
			// chkRegularExpressions
			// 
			this.chkRegularExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkRegularExpressions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkRegularExpressions.Location = new System.Drawing.Point(23, 32);
			this.chkRegularExpressions.Name = "chkRegularExpressions";
			this.chkRegularExpressions.Size = new System.Drawing.Size(267, 20);
			this.chkRegularExpressions.TabIndex = 7;
			this.chkRegularExpressions.Text = "Regular &Expressions";
			this.toolTip1.SetToolTip(this.chkRegularExpressions, "Use \"regular expression\" matching");
			// 
			// chkSearchInResults
			// 
			this.chkSearchInResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSearchInResults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkSearchInResults.Location = new System.Drawing.Point(23, 8);
			this.chkSearchInResults.Name = "chkSearchInResults";
			this.chkSearchInResults.Size = new System.Drawing.Size(267, 20);
			this.chkSearchInResults.TabIndex = 6;
			this.chkSearchInResults.Text = "Search in results";
			this.toolTip1.SetToolTip(this.chkSearchInResults, "Search in results");
			// 
			// chkNegation
			// 
			this.chkNegation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkNegation.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkNegation.Location = new System.Drawing.Point(23, 152);
			this.chkNegation.Name = "chkNegation";
			this.chkNegation.Size = new System.Drawing.Size(267, 20);
			this.chkNegation.TabIndex = 12;
			this.chkNegation.Text = "&Negation";
			this.toolTip1.SetToolTip(this.chkNegation, "Find the files without the Search Text in them");
			this.chkNegation.CheckedChanged += new System.EventHandler(this.chkNegation_CheckedChanged);
			// 
			// chkFileNamesOnly
			// 
			this.chkFileNamesOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkFileNamesOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkFileNamesOnly.Location = new System.Drawing.Point(23, 128);
			this.chkFileNamesOnly.Name = "chkFileNamesOnly";
			this.chkFileNamesOnly.Size = new System.Drawing.Size(267, 20);
			this.chkFileNamesOnly.TabIndex = 11;
			this.chkFileNamesOnly.Text = "Show File Names &Only";
			this.toolTip1.SetToolTip(this.chkFileNamesOnly, "Show names but not contents of files that have matches (may be faster on large fi" +
        "les)");
			// 
			// chkRecurse
			// 
			this.chkRecurse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkRecurse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkRecurse.Location = new System.Drawing.Point(23, 104);
			this.chkRecurse.Name = "chkRecurse";
			this.chkRecurse.Size = new System.Drawing.Size(267, 20);
			this.chkRecurse.TabIndex = 10;
			this.chkRecurse.Text = "&Recurse";
			this.toolTip1.SetToolTip(this.chkRecurse, "Search in subdirectories");
			// 
			// chkCaseSensitive
			// 
			this.chkCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkCaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkCaseSensitive.Location = new System.Drawing.Point(23, 56);
			this.chkCaseSensitive.Name = "chkCaseSensitive";
			this.chkCaseSensitive.Size = new System.Drawing.Size(267, 20);
			this.chkCaseSensitive.TabIndex = 8;
			this.chkCaseSensitive.Text = "&Case Sensitive";
			this.toolTip1.SetToolTip(this.chkCaseSensitive, "Match upper and lower case letters exactly");
			// 
			// lblSearchOptions
			// 
			this.lblSearchOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchOptions.BackColor = System.Drawing.SystemColors.Window;
			this.lblSearchOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(127)))), ((int)(((byte)(6)))));
			this.lblSearchOptions.Location = new System.Drawing.Point(16, 15);
			this.lblSearchOptions.Name = "lblSearchOptions";
			this.lblSearchOptions.Padding = new System.Windows.Forms.Padding(3, 0, 0, 4);
			this.lblSearchOptions.Size = new System.Drawing.Size(258, 23);
			this.lblSearchOptions.TabIndex = 5;
			this.lblSearchOptions.Text = "Search Options";
			this.lblSearchOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblSearchOptions.Paint += new System.Windows.Forms.PaintEventHandler(this.lblSearchOptions_Paint);
			// 
			// pnlMainSearch
			// 
			this.pnlMainSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlMainSearch.Controls.Add(this.picBrowse);
			this.pnlMainSearch.Controls.Add(this.btnSearch);
			this.pnlMainSearch.Controls.Add(this.btnCancel);
			this.pnlMainSearch.Controls.Add(this.cboFilePath);
			this.pnlMainSearch.Controls.Add(this.cboFileName);
			this.pnlMainSearch.Controls.Add(this.cboSearchForText);
			this.pnlMainSearch.Controls.Add(this.lblSearchText);
			this.pnlMainSearch.Controls.Add(this.lblFileTypes);
			this.pnlMainSearch.Controls.Add(this.lblSearchPath);
			this.pnlMainSearch.Controls.Add(this.lblSearchHeading);
			this.pnlMainSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlMainSearch.Name = "pnlMainSearch";
			this.pnlMainSearch.Size = new System.Drawing.Size(290, 210);
			this.pnlMainSearch.TabIndex = 0;
			// 
			// picBrowse
			// 
			this.picBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBrowse.Image = ((System.Drawing.Image)(resources.GetObject("picBrowse.Image")));
			this.picBrowse.Location = new System.Drawing.Point(251, 56);
			this.picBrowse.Name = "picBrowse";
			this.picBrowse.Size = new System.Drawing.Size(16, 16);
			this.picBrowse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picBrowse.TabIndex = 6;
			this.picBrowse.TabStop = false;
			this.picBrowse.Click += new System.EventHandler(this.picBrowse_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
			this.btnSearch.Location = new System.Drawing.Point(23, 182);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(85, 25);
			this.btnSearch.TabIndex = 4;
			this.btnSearch.Text = "&Search";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Enabled = false;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(182, 182);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(85, 25);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cboFilePath
			// 
			this.cboFilePath.AllowDrop = true;
			this.cboFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboFilePath.Location = new System.Drawing.Point(24, 55);
			this.cboFilePath.Name = "cboFilePath";
			this.cboFilePath.Size = new System.Drawing.Size(216, 23);
			this.cboFilePath.TabIndex = 1;
			this.cboFilePath.DropDown += new System.EventHandler(this.cboFilePath_DropDown);
			this.cboFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilePath_DragDrop);
			this.cboFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilePath_DragEnter);
			// 
			// cboFileName
			// 
			this.cboFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboFileName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cboFileName.Location = new System.Drawing.Point(24, 103);
			this.cboFileName.Name = "cboFileName";
			this.cboFileName.Size = new System.Drawing.Size(242, 22);
			this.cboFileName.TabIndex = 2;
			this.cboFileName.DropDown += new System.EventHandler(this.cboFileName_DropDown);
			// 
			// cboSearchForText
			// 
			this.cboSearchForText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboSearchForText.Location = new System.Drawing.Point(24, 149);
			this.cboSearchForText.Name = "cboSearchForText";
			this.cboSearchForText.Size = new System.Drawing.Size(242, 23);
			this.cboSearchForText.TabIndex = 3;
			this.cboSearchForText.DropDown += new System.EventHandler(this.cboSearchForText_DropDown);
			// 
			// lblSearchText
			// 
			this.lblSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchText.AutoSize = true;
			this.lblSearchText.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblSearchText.Location = new System.Drawing.Point(24, 133);
			this.lblSearchText.Name = "lblSearchText";
			this.lblSearchText.Size = new System.Drawing.Size(72, 15);
			this.lblSearchText.TabIndex = 3;
			this.lblSearchText.Text = "Search Text";
			// 
			// lblFileTypes
			// 
			this.lblFileTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFileTypes.AutoSize = true;
			this.lblFileTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblFileTypes.Location = new System.Drawing.Point(24, 87);
			this.lblFileTypes.Name = "lblFileTypes";
			this.lblFileTypes.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
			this.lblFileTypes.Size = new System.Drawing.Size(70, 15);
			this.lblFileTypes.TabIndex = 2;
			this.lblFileTypes.Text = "File Types";
			// 
			// lblSearchPath
			// 
			this.lblSearchPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchPath.AutoSize = true;
			this.lblSearchPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblSearchPath.Location = new System.Drawing.Point(24, 39);
			this.lblSearchPath.Name = "lblSearchPath";
			this.lblSearchPath.Size = new System.Drawing.Size(74, 15);
			this.lblSearchPath.TabIndex = 1;
			this.lblSearchPath.Text = "Search Path";
			// 
			// lblSearchHeading
			// 
			this.lblSearchHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchHeading.BackColor = System.Drawing.SystemColors.Window;
			this.lblSearchHeading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(127)))), ((int)(((byte)(6)))));
			this.lblSearchHeading.Location = new System.Drawing.Point(16, 9);
			this.lblSearchHeading.Name = "lblSearchHeading";
			this.lblSearchHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this.lblSearchHeading.Size = new System.Drawing.Size(258, 23);
			this.lblSearchHeading.TabIndex = 0;
			this.lblSearchHeading.Text = "AstroGrep Search";
			this.lblSearchHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblSearchHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.lblSearchHeading_Paint);
			// 
			// pnlRightSide
			// 
			this.pnlRightSide.BackColor = System.Drawing.SystemColors.Window;
			this.pnlRightSide.Controls.Add(this.textElementHost);
			this.pnlRightSide.Controls.Add(this.ResultsToolStrip);
			this.pnlRightSide.Controls.Add(this.splitUpDown);
			this.pnlRightSide.Controls.Add(this.lstFileNames);
			this.pnlRightSide.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRightSide.Location = new System.Drawing.Point(290, 24);
			this.pnlRightSide.Name = "pnlRightSide";
			this.pnlRightSide.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.pnlRightSide.Size = new System.Drawing.Size(494, 513);
			this.pnlRightSide.TabIndex = 1;
			// 
			// textElementHost
			// 
			this.textElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textElementHost.Location = new System.Drawing.Point(2, 225);
			this.textElementHost.Name = "textElementHost";
			this.textElementHost.Size = new System.Drawing.Size(492, 288);
			this.textElementHost.TabIndex = 16;
			this.textElementHost.Child = this.txtHits;
			// 
			// ResultsToolStrip
			// 
			this.ResultsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ResultsToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.ResultsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResultsZoomInButton,
            this.ResultsZoomOutButton,
            this.ResultsZoomResetButton,
            this.toolStripSeparator1,
            this.ResultsViewLineNumbersButton,
            this.ResultsViewWordWrapButton,
            this.ResultsViewRemovingLeadingButton,
            this.ResultsViewEntireFileButton,
            this.ResultsViewShowCharactersButton,
            this.ResultsViewAllResultsButton,
            this.toolStripSeparator2,
            this.ResultsContextLinesLabel,
            this.ResultsContextLinesBeforeCombo,
            this.toolStripLabel2,
            this.ResultsContextLinesAfterCombo,
            this.toolStripSeparator3,
            this.ResultsNavNextButton,
            this.ResultsNavPrevButton});
			this.ResultsToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.ResultsToolStrip.Location = new System.Drawing.Point(2, 194);
			this.ResultsToolStrip.Name = "ResultsToolStrip";
			this.ResultsToolStrip.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ResultsToolStrip.Size = new System.Drawing.Size(492, 31);
			this.ResultsToolStrip.TabIndex = 3;
			this.ResultsToolStrip.Text = "toolStrip1";
			// 
			// ResultsZoomInButton
			// 
			this.ResultsZoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsZoomInButton.Image = global::AstroGrep.Properties.Resources.zoom_in;
			this.ResultsZoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsZoomInButton.Name = "ResultsZoomInButton";
			this.ResultsZoomInButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsZoomInButton.Text = "+";
			this.ResultsZoomInButton.ToolTipText = "Zoom In";
			this.ResultsZoomInButton.Click += new System.EventHandler(this.ResultsZoomInButton_Click);
			// 
			// ResultsZoomOutButton
			// 
			this.ResultsZoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsZoomOutButton.Image = global::AstroGrep.Properties.Resources.zoom_out;
			this.ResultsZoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsZoomOutButton.Name = "ResultsZoomOutButton";
			this.ResultsZoomOutButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsZoomOutButton.Text = "-";
			this.ResultsZoomOutButton.ToolTipText = "Zoom Out";
			this.ResultsZoomOutButton.Click += new System.EventHandler(this.ResultsZoomOutButton_Click);
			// 
			// ResultsZoomResetButton
			// 
			this.ResultsZoomResetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsZoomResetButton.Image = global::AstroGrep.Properties.Resources.zoom;
			this.ResultsZoomResetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsZoomResetButton.Name = "ResultsZoomResetButton";
			this.ResultsZoomResetButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsZoomResetButton.Text = "/";
			this.ResultsZoomResetButton.ToolTipText = "Reset Zoom";
			this.ResultsZoomResetButton.Click += new System.EventHandler(this.ResultsZoomResetButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
			// 
			// ResultsViewLineNumbersButton
			// 
			this.ResultsViewLineNumbersButton.CheckOnClick = true;
			this.ResultsViewLineNumbersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewLineNumbersButton.Image = global::AstroGrep.Properties.Resources.line_numbers;
			this.ResultsViewLineNumbersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewLineNumbersButton.Name = "ResultsViewLineNumbersButton";
			this.ResultsViewLineNumbersButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewLineNumbersButton.Text = "#";
			this.ResultsViewLineNumbersButton.ToolTipText = "Line Numbers";
			this.ResultsViewLineNumbersButton.Click += new System.EventHandler(this.ResultsViewLineNumbersButton_Click);
			// 
			// ResultsViewWordWrapButton
			// 
			this.ResultsViewWordWrapButton.CheckOnClick = true;
			this.ResultsViewWordWrapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewWordWrapButton.Image = global::AstroGrep.Properties.Resources.word_wrap;
			this.ResultsViewWordWrapButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewWordWrapButton.Name = "ResultsViewWordWrapButton";
			this.ResultsViewWordWrapButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewWordWrapButton.Text = "WW";
			this.ResultsViewWordWrapButton.ToolTipText = "Word Wrap";
			this.ResultsViewWordWrapButton.Click += new System.EventHandler(this.ResultsViewWordWrapButton_Click);
			// 
			// ResultsViewRemovingLeadingButton
			// 
			this.ResultsViewRemovingLeadingButton.CheckOnClick = true;
			this.ResultsViewRemovingLeadingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewRemovingLeadingButton.Image = global::AstroGrep.Properties.Resources.text_indent_remove;
			this.ResultsViewRemovingLeadingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewRemovingLeadingButton.Name = "ResultsViewRemovingLeadingButton";
			this.ResultsViewRemovingLeadingButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewRemovingLeadingButton.Text = "RL";
			this.ResultsViewRemovingLeadingButton.ToolTipText = "Remove Leading White Space";
			this.ResultsViewRemovingLeadingButton.Click += new System.EventHandler(this.ResultsViewRemovingLeadingButton_Click);
			// 
			// ResultsViewEntireFileButton
			// 
			this.ResultsViewEntireFileButton.CheckOnClick = true;
			this.ResultsViewEntireFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewEntireFileButton.Image = global::AstroGrep.Properties.Resources.text_resize;
			this.ResultsViewEntireFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewEntireFileButton.Name = "ResultsViewEntireFileButton";
			this.ResultsViewEntireFileButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewEntireFileButton.Text = "EF";
			this.ResultsViewEntireFileButton.ToolTipText = "Entire File";
			this.ResultsViewEntireFileButton.Click += new System.EventHandler(this.ResultsViewEntireFileButton_Click);
			// 
			// ResultsViewShowCharactersButton
			// 
			this.ResultsViewShowCharactersButton.CheckOnClick = true;
			this.ResultsViewShowCharactersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewShowCharactersButton.Image = global::AstroGrep.Properties.Resources.pilcrow;
			this.ResultsViewShowCharactersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewShowCharactersButton.Name = "ResultsViewShowCharactersButton";
			this.ResultsViewShowCharactersButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewShowCharactersButton.ToolTipText = "Show all characters";
			this.ResultsViewShowCharactersButton.Click += new System.EventHandler(this.ResultsViewShowCharactersButton_Click);
			// 
			// ResultsViewAllResultsButton
			// 
			this.ResultsViewAllResultsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsViewAllResultsButton.Image = global::AstroGrep.Properties.Resources.preview_results;
			this.ResultsViewAllResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsViewAllResultsButton.Name = "ResultsViewAllResultsButton";
			this.ResultsViewAllResultsButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsViewAllResultsButton.Text = "AR";
			this.ResultsViewAllResultsButton.ToolTipText = "All Results";
			this.ResultsViewAllResultsButton.Click += new System.EventHandler(this.ResultsViewAllResultsButton_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
			// 
			// ResultsContextLinesLabel
			// 
			this.ResultsContextLinesLabel.Name = "ResultsContextLinesLabel";
			this.ResultsContextLinesLabel.Size = new System.Drawing.Size(79, 28);
			this.ResultsContextLinesLabel.Text = "Context Lines";
			// 
			// ResultsContextLinesBeforeCombo
			// 
			this.ResultsContextLinesBeforeCombo.AutoSize = false;
			this.ResultsContextLinesBeforeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ResultsContextLinesBeforeCombo.DropDownWidth = 50;
			this.ResultsContextLinesBeforeCombo.Name = "ResultsContextLinesBeforeCombo";
			this.ResultsContextLinesBeforeCombo.Size = new System.Drawing.Size(50, 23);
			this.ResultsContextLinesBeforeCombo.ToolTipText = "Before Context Lines";
			this.ResultsContextLinesBeforeCombo.SelectedIndexChanged += new System.EventHandler(this.ResultsContextLinesCombo_SelectedIndexChanged);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(18, 28);
			this.toolStripLabel2.Text = " / ";
			// 
			// ResultsContextLinesAfterCombo
			// 
			this.ResultsContextLinesAfterCombo.AutoSize = false;
			this.ResultsContextLinesAfterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ResultsContextLinesAfterCombo.Name = "ResultsContextLinesAfterCombo";
			this.ResultsContextLinesAfterCombo.Size = new System.Drawing.Size(50, 23);
			this.ResultsContextLinesAfterCombo.ToolTipText = "After Context Lines";
			this.ResultsContextLinesAfterCombo.SelectedIndexChanged += new System.EventHandler(this.ResultsContextLinesCombo_SelectedIndexChanged);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
			// 
			// ResultsNavNextButton
			// 
			this.ResultsNavNextButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ResultsNavNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsNavNextButton.Image = ((System.Drawing.Image)(resources.GetObject("ResultsNavNextButton.Image")));
			this.ResultsNavNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsNavNextButton.Name = "ResultsNavNextButton";
			this.ResultsNavNextButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsNavNextButton.Text = ">";
			this.ResultsNavNextButton.ToolTipText = "Next Result";
			this.ResultsNavNextButton.Click += new System.EventHandler(this.ResultsNavNextButton_Click);
			// 
			// ResultsNavPrevButton
			// 
			this.ResultsNavPrevButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.ResultsNavPrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResultsNavPrevButton.Image = ((System.Drawing.Image)(resources.GetObject("ResultsNavPrevButton.Image")));
			this.ResultsNavPrevButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResultsNavPrevButton.Name = "ResultsNavPrevButton";
			this.ResultsNavPrevButton.Size = new System.Drawing.Size(28, 28);
			this.ResultsNavPrevButton.Text = "<";
			this.ResultsNavPrevButton.ToolTipText = "Previous Result";
			this.ResultsNavPrevButton.Click += new System.EventHandler(this.ResultsNavPrevButton_Click);
			// 
			// splitUpDown
			// 
			this.splitUpDown.BackColor = System.Drawing.SystemColors.Control;
			this.splitUpDown.Cursor = System.Windows.Forms.Cursors.SizeNS;
			this.splitUpDown.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitUpDown.Location = new System.Drawing.Point(2, 192);
			this.splitUpDown.Name = "splitUpDown";
			this.splitUpDown.Size = new System.Drawing.Size(492, 2);
			this.splitUpDown.TabIndex = 2;
			this.splitUpDown.TabStop = false;
			// 
			// lstFileNames
			// 
			this.lstFileNames.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstFileNames.ContextMenu = this.fileLstMnu;
			this.lstFileNames.Dock = System.Windows.Forms.DockStyle.Top;
			this.lstFileNames.FullRowSelect = true;
			this.lstFileNames.HideSelection = false;
			this.lstFileNames.Location = new System.Drawing.Point(2, 0);
			this.lstFileNames.Name = "lstFileNames";
			this.lstFileNames.Size = new System.Drawing.Size(492, 192);
			this.lstFileNames.SmallImageList = this.ListViewImageList;
			this.lstFileNames.TabIndex = 15;
			this.lstFileNames.UseCompatibleStateImageBehavior = false;
			this.lstFileNames.View = System.Windows.Forms.View.Details;
			this.lstFileNames.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstFileNames_ColumnClick);
			this.lstFileNames.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lstFileNames_ItemDrag);
			this.lstFileNames.SelectedIndexChanged += new System.EventHandler(this.lstFileNames_SelectedIndexChanged);
			this.lstFileNames.Enter += new System.EventHandler(this.lstFileNames_Enter);
			this.lstFileNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFileNames_KeyDown);
			this.lstFileNames.Leave += new System.EventHandler(this.lstFileNames_Leave);
			this.lstFileNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstFileNames_MouseDown);
			// 
			// fileLstMnu
			// 
			this.fileLstMnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.CopyMenuItem,
            this.CopyNameMenuItem,
            this.CopyLocatedInMenuItem,
            this.CopyLocatedInAndNameMenuItem,
            this.menuItem4,
            this.FileOperationsMenuItem,
            this.menuItem6,
            this.OpenMenuItem,
            this.OpenFolderMenuItem,
            this.OpenWithAssociatedApp,
            this.menuItem2,
            this.DeleteMenuItem});
			// 
			// CopyMenuItem
			// 
			this.CopyMenuItem.Index = 0;
			this.CopyMenuItem.Text = "Copy All";
			this.CopyMenuItem.Click += new System.EventHandler(this.CopyMenuItem_Click);
			// 
			// CopyNameMenuItem
			// 
			this.CopyNameMenuItem.Index = 1;
			this.CopyNameMenuItem.Text = "Copy Name";
			this.CopyNameMenuItem.Click += new System.EventHandler(this.CopyNameMenuItem_Click);
			// 
			// CopyLocatedInMenuItem
			// 
			this.CopyLocatedInMenuItem.Index = 2;
			this.CopyLocatedInMenuItem.Text = "Copy Located In";
			this.CopyLocatedInMenuItem.Click += new System.EventHandler(this.CopyLocatedInMenuItem_Click);
			// 
			// CopyLocatedInAndNameMenuItem
			// 
			this.CopyLocatedInAndNameMenuItem.Index = 3;
			this.CopyLocatedInAndNameMenuItem.Text = "Copy Located In + Name";
			this.CopyLocatedInAndNameMenuItem.Click += new System.EventHandler(this.CopyLocatedInAndNameMenuItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// FileOperationsMenuItem
			// 
			this.FileOperationsMenuItem.Index = 5;
			this.FileOperationsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileCopyMenuItem,
            this.FileDeleteMenuItem});
			this.FileOperationsMenuItem.Text = "File Operations";
			// 
			// FileCopyMenuItem
			// 
			this.FileCopyMenuItem.Index = 0;
			this.FileCopyMenuItem.Text = "Copy";
			this.FileCopyMenuItem.Click += new System.EventHandler(this.FileCopyMenuItem_Click);
			// 
			// FileDeleteMenuItem
			// 
			this.FileDeleteMenuItem.Index = 1;
			this.FileDeleteMenuItem.Text = "Delete";
			this.FileDeleteMenuItem.Click += new System.EventHandler(this.FileDeleteMenuItem_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 6;
			this.menuItem6.Text = "-";
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Index = 7;
			this.OpenMenuItem.Text = "Open File";
			this.OpenMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
			// 
			// OpenFolderMenuItem
			// 
			this.OpenFolderMenuItem.Index = 8;
			this.OpenFolderMenuItem.Text = "Open Directory";
			this.OpenFolderMenuItem.Click += new System.EventHandler(this.OpenFolderMenuItem_Click);
			// 
			// OpenWithAssociatedApp
			// 
			this.OpenWithAssociatedApp.Index = 9;
			this.OpenWithAssociatedApp.Text = "Open With Associated App";
			this.OpenWithAssociatedApp.Click += new System.EventHandler(this.OpenWithAssociatedApp_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 10;
			this.menuItem2.Text = "-";
			// 
			// DeleteMenuItem
			// 
			this.DeleteMenuItem.Index = 11;
			this.DeleteMenuItem.Text = "Remove from list";
			this.DeleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
			// 
			// ListViewImageList
			// 
			this.ListViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ListViewImageList.ImageStream")));
			this.ListViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.ListViewImageList.Images.SetKeyName(0, "");
			this.ListViewImageList.Images.SetKeyName(1, "");
			// 
			// splitLeftRight
			// 
			this.splitLeftRight.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this.splitLeftRight.Location = new System.Drawing.Point(290, 24);
			this.splitLeftRight.MinExtra = 100;
			this.splitLeftRight.MinSize = 290;
			this.splitLeftRight.Name = "splitLeftRight";
			this.splitLeftRight.Size = new System.Drawing.Size(2, 513);
			this.splitLeftRight.TabIndex = 2;
			this.splitLeftRight.TabStop = false;
			// 
			// stbStatus
			// 
			this.stbStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbStatusPanel,
            this.sbEncodingPanel,
            this.sbTotalCountPanel,
            this.sbFileCountPanel,
            this.sbFilterCountPanel,
            this.sbErrorCountPanel});
			this.stbStatus.Location = new System.Drawing.Point(0, 537);
			this.stbStatus.Name = "stbStatus";
			this.stbStatus.ShowItemToolTips = true;
			this.stbStatus.Size = new System.Drawing.Size(784, 24);
			this.stbStatus.TabIndex = 1;
			// 
			// sbStatusPanel
			// 
			this.sbStatusPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbStatusPanel.DoubleClickEnabled = true;
			this.sbStatusPanel.Name = "sbStatusPanel";
			this.sbStatusPanel.Size = new System.Drawing.Size(554, 19);
			this.sbStatusPanel.Spring = true;
			this.sbStatusPanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.sbStatusPanel.DoubleClick += new System.EventHandler(this.sbStatusPanel_DoubleClick);
			// 
			// sbEncodingPanel
			// 
			this.sbEncodingPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.sbEncodingPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbEncodingPanel.Name = "sbEncodingPanel";
			this.sbEncodingPanel.Size = new System.Drawing.Size(4, 19);
			// 
			// sbTotalCountPanel
			// 
			this.sbTotalCountPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.sbTotalCountPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbTotalCountPanel.Name = "sbTotalCountPanel";
			this.sbTotalCountPanel.Size = new System.Drawing.Size(48, 19);
			this.sbTotalCountPanel.Text = "Total: 0";
			// 
			// sbFileCountPanel
			// 
			this.sbFileCountPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.sbFileCountPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbFileCountPanel.Name = "sbFileCountPanel";
			this.sbFileCountPanel.Size = new System.Drawing.Size(41, 19);
			this.sbFileCountPanel.Text = "File: 0";
			// 
			// sbFilterCountPanel
			// 
			this.sbFilterCountPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.sbFilterCountPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbFilterCountPanel.DoubleClickEnabled = true;
			this.sbFilterCountPanel.Name = "sbFilterCountPanel";
			this.sbFilterCountPanel.Size = new System.Drawing.Size(78, 19);
			this.sbFilterCountPanel.Text = "Exclusions: 0";
			this.sbFilterCountPanel.ToolTipText = "Double click to display exclusions.";
			this.sbFilterCountPanel.DoubleClick += new System.EventHandler(this.sbFilterCountPanel_DoubleClick);
			this.sbFilterCountPanel.MouseEnter += new System.EventHandler(this.CountPanel_MouseEnter);
			this.sbFilterCountPanel.MouseLeave += new System.EventHandler(this.CountPanel_MouseLeave);
			// 
			// sbErrorCountPanel
			// 
			this.sbErrorCountPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sbErrorCountPanel.DoubleClickEnabled = true;
			this.sbErrorCountPanel.Name = "sbErrorCountPanel";
			this.sbErrorCountPanel.Size = new System.Drawing.Size(44, 19);
			this.sbErrorCountPanel.Text = "Error: 0";
			this.sbErrorCountPanel.ToolTipText = "Double click to display errors.";
			this.sbErrorCountPanel.DoubleClick += new System.EventHandler(this.sbErrorCountPanel_DoubleClick);
			this.sbErrorCountPanel.MouseEnter += new System.EventHandler(this.CountPanel_MouseEnter);
			this.sbErrorCountPanel.MouseLeave += new System.EventHandler(this.CountPanel_MouseLeave);
			// 
			// MainMenu
			// 
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.ViewMenu,
            this.ToolsMenu,
            this.HelpMenu});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(784, 24);
			this.MainMenu.TabIndex = 0;
			this.MainMenu.Text = "MainMenu";
			// 
			// FileMenu
			// 
			this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewWindowMenuItem,
            this.FileSep1Separator,
            this.SelectPathMenuItem,
            this.FileSep2Separator,
            this.SaveResultsMenuItem,
            this.PrintResultsMenuItem,
            this.FileSep3Separator,
            this.ExitMenuItem,
            this.ExitAllMenuItem});
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(37, 20);
			this.FileMenu.Text = "File";
			this.FileMenu.DropDownOpening += new System.EventHandler(this.FileMenu_DropDownOpening);
			// 
			// NewWindowMenuItem
			// 
			this.NewWindowMenuItem.Name = "NewWindowMenuItem";
			this.NewWindowMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.NewWindowMenuItem.Size = new System.Drawing.Size(222, 22);
			this.NewWindowMenuItem.Text = "&New Window";
			this.NewWindowMenuItem.Click += new System.EventHandler(this.NewWindowMenuItem_Click);
			// 
			// FileSep1Separator
			// 
			this.FileSep1Separator.Name = "FileSep1Separator";
			this.FileSep1Separator.Size = new System.Drawing.Size(219, 6);
			// 
			// SelectPathMenuItem
			// 
			this.SelectPathMenuItem.Name = "SelectPathMenuItem";
			this.SelectPathMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.SelectPathMenuItem.Size = new System.Drawing.Size(222, 22);
			this.SelectPathMenuItem.Text = "Select Sea&rch Path...";
			this.SelectPathMenuItem.Click += new System.EventHandler(this.SelectPathMenuItem_Click);
			// 
			// FileSep2Separator
			// 
			this.FileSep2Separator.Name = "FileSep2Separator";
			this.FileSep2Separator.Size = new System.Drawing.Size(219, 6);
			// 
			// SaveResultsMenuItem
			// 
			this.SaveResultsMenuItem.Name = "SaveResultsMenuItem";
			this.SaveResultsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.SaveResultsMenuItem.Size = new System.Drawing.Size(222, 22);
			this.SaveResultsMenuItem.Text = "&Save Results";
			this.SaveResultsMenuItem.Click += new System.EventHandler(this.SaveResultsMenuItem_Click);
			// 
			// PrintResultsMenuItem
			// 
			this.PrintResultsMenuItem.Name = "PrintResultsMenuItem";
			this.PrintResultsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.PrintResultsMenuItem.Size = new System.Drawing.Size(222, 22);
			this.PrintResultsMenuItem.Text = "&Print Results";
			this.PrintResultsMenuItem.Click += new System.EventHandler(this.PrintResultsMenuItem_Click);
			// 
			// FileSep3Separator
			// 
			this.FileSep3Separator.Name = "FileSep3Separator";
			this.FileSep3Separator.Size = new System.Drawing.Size(219, 6);
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Name = "ExitMenuItem";
			this.ExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.ExitMenuItem.Size = new System.Drawing.Size(222, 22);
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// ExitAllMenuItem
			// 
			this.ExitAllMenuItem.Name = "ExitAllMenuItem";
			this.ExitAllMenuItem.Size = new System.Drawing.Size(222, 22);
			this.ExitAllMenuItem.Text = "Exit All";
			this.ExitAllMenuItem.Click += new System.EventHandler(this.ExitAllMenuItem_Click);
			// 
			// EditMenu
			// 
			this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllMenuItem,
            this.OpenSelectedMenuItem});
			this.EditMenu.Name = "EditMenu";
			this.EditMenu.Size = new System.Drawing.Size(39, 20);
			this.EditMenu.Text = "Edit";
			this.EditMenu.DropDownOpening += new System.EventHandler(this.EditMenu_DropDownOpening);
			// 
			// SelectAllMenuItem
			// 
			this.SelectAllMenuItem.Name = "SelectAllMenuItem";
			this.SelectAllMenuItem.Size = new System.Drawing.Size(176, 22);
			this.SelectAllMenuItem.Text = "&Select All Files";
			this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
			// 
			// OpenSelectedMenuItem
			// 
			this.OpenSelectedMenuItem.Name = "OpenSelectedMenuItem";
			this.OpenSelectedMenuItem.Size = new System.Drawing.Size(176, 22);
			this.OpenSelectedMenuItem.Text = "&Open Selected Files";
			this.OpenSelectedMenuItem.Click += new System.EventHandler(this.OpenSelectedMenuItem_Click);
			// 
			// ViewMenu
			// 
			this.ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMessageMenuItem,
            this.ExclusionMessageMenuItem,
            this.ErrorMessageMenuItem,
            this.AllMessageMenuItem,
            this.ViewSep1Separator,
            this.ZoomMenuItem,
            this.LineNumbersMenuItem,
            this.WordWrapMenuItem,
            this.RemoveWhiteSpaceMenuItem,
            this.EntireFileMenuItem,
            this.ShowCharactersMenuItem,
            this.ViewSep2Separator,
            this.AllResultsMenuItem});
			this.ViewMenu.Name = "ViewMenu";
			this.ViewMenu.Size = new System.Drawing.Size(44, 20);
			this.ViewMenu.Text = "View";
			this.ViewMenu.DropDownOpening += new System.EventHandler(this.ViewMenu_DropDownOpening);
			// 
			// StatusMessageMenuItem
			// 
			this.StatusMessageMenuItem.Name = "StatusMessageMenuItem";
			this.StatusMessageMenuItem.Size = new System.Drawing.Size(230, 22);
			this.StatusMessageMenuItem.Text = "&Status Messages";
			this.StatusMessageMenuItem.Click += new System.EventHandler(this.StatusMessageMenuItem_Click);
			// 
			// ExclusionMessageMenuItem
			// 
			this.ExclusionMessageMenuItem.Name = "ExclusionMessageMenuItem";
			this.ExclusionMessageMenuItem.Size = new System.Drawing.Size(230, 22);
			this.ExclusionMessageMenuItem.Text = "&Exclusion Messages";
			this.ExclusionMessageMenuItem.Click += new System.EventHandler(this.ExclusionMessageMenuItem_Click);
			// 
			// ErrorMessageMenuItem
			// 
			this.ErrorMessageMenuItem.Name = "ErrorMessageMenuItem";
			this.ErrorMessageMenuItem.Size = new System.Drawing.Size(230, 22);
			this.ErrorMessageMenuItem.Text = "E&rror Messages";
			this.ErrorMessageMenuItem.Click += new System.EventHandler(this.ErrorMessageMenuItem_Click);
			// 
			// AllMessageMenuItem
			// 
			this.AllMessageMenuItem.Name = "AllMessageMenuItem";
			this.AllMessageMenuItem.Size = new System.Drawing.Size(230, 22);
			this.AllMessageMenuItem.Text = "&All Messages";
			this.AllMessageMenuItem.Click += new System.EventHandler(this.AllMessageMenuItem_Click);
			// 
			// ViewSep1Separator
			// 
			this.ViewSep1Separator.Name = "ViewSep1Separator";
			this.ViewSep1Separator.Size = new System.Drawing.Size(227, 6);
			// 
			// ZoomMenuItem
			// 
			this.ZoomMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomInMenuItem,
            this.ZoomOutMenuItem,
            this.ZoomRestoreMenuItem});
			this.ZoomMenuItem.Name = "ZoomMenuItem";
			this.ZoomMenuItem.Size = new System.Drawing.Size(230, 22);
			this.ZoomMenuItem.Text = "Zoom";
			// 
			// ZoomInMenuItem
			// 
			this.ZoomInMenuItem.Image = global::AstroGrep.Properties.Resources.zoom_in_16;
			this.ZoomInMenuItem.Name = "ZoomInMenuItem";
			this.ZoomInMenuItem.ShortcutKeyDisplayString = "Ctrl+Num +";
			this.ZoomInMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Add)));
			this.ZoomInMenuItem.Size = new System.Drawing.Size(342, 22);
			this.ZoomInMenuItem.Text = "Zoom In (Ctrl+Mouse Wheel Up)";
			this.ZoomInMenuItem.Click += new System.EventHandler(this.ZoomInMenuItem_Click);
			// 
			// ZoomOutMenuItem
			// 
			this.ZoomOutMenuItem.Image = global::AstroGrep.Properties.Resources.zoom_out_16;
			this.ZoomOutMenuItem.Name = "ZoomOutMenuItem";
			this.ZoomOutMenuItem.ShortcutKeyDisplayString = "Ctrl+Num -";
			this.ZoomOutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Subtract)));
			this.ZoomOutMenuItem.Size = new System.Drawing.Size(342, 22);
			this.ZoomOutMenuItem.Text = "Zoom Out (Ctrl+Mouse Wheel Down)";
			this.ZoomOutMenuItem.Click += new System.EventHandler(this.ZoomOutMenuItem_Click);
			// 
			// ZoomRestoreMenuItem
			// 
			this.ZoomRestoreMenuItem.Image = global::AstroGrep.Properties.Resources.zoom_16;
			this.ZoomRestoreMenuItem.Name = "ZoomRestoreMenuItem";
			this.ZoomRestoreMenuItem.ShortcutKeyDisplayString = "Ctrl+Num /";
			this.ZoomRestoreMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Divide)));
			this.ZoomRestoreMenuItem.Size = new System.Drawing.Size(342, 22);
			this.ZoomRestoreMenuItem.Text = "Restore Default Zoom";
			this.ZoomRestoreMenuItem.Click += new System.EventHandler(this.ZoomRestoreMenuItem_Click);
			// 
			// LineNumbersMenuItem
			// 
			this.LineNumbersMenuItem.Image = global::AstroGrep.Properties.Resources.line_numbers_16;
			this.LineNumbersMenuItem.Name = "LineNumbersMenuItem";
			this.LineNumbersMenuItem.Size = new System.Drawing.Size(230, 22);
			this.LineNumbersMenuItem.Text = "Line Numbers";
			this.LineNumbersMenuItem.Click += new System.EventHandler(this.LineNumbersMenuItem_Click);
			// 
			// WordWrapMenuItem
			// 
			this.WordWrapMenuItem.Image = global::AstroGrep.Properties.Resources.word_wrap_16;
			this.WordWrapMenuItem.Name = "WordWrapMenuItem";
			this.WordWrapMenuItem.Size = new System.Drawing.Size(230, 22);
			this.WordWrapMenuItem.Text = "Word Wrap";
			this.WordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItem_Click);
			// 
			// RemoveWhiteSpaceMenuItem
			// 
			this.RemoveWhiteSpaceMenuItem.Image = global::AstroGrep.Properties.Resources.text_indent_remove_16;
			this.RemoveWhiteSpaceMenuItem.Name = "RemoveWhiteSpaceMenuItem";
			this.RemoveWhiteSpaceMenuItem.Size = new System.Drawing.Size(230, 22);
			this.RemoveWhiteSpaceMenuItem.Text = "Remove Leading White Space";
			this.RemoveWhiteSpaceMenuItem.Click += new System.EventHandler(this.RemoveWhiteSpaceMenuItem_Click);
			// 
			// EntireFileMenuItem
			// 
			this.EntireFileMenuItem.Image = global::AstroGrep.Properties.Resources.text_resize_16;
			this.EntireFileMenuItem.Name = "EntireFileMenuItem";
			this.EntireFileMenuItem.Size = new System.Drawing.Size(230, 22);
			this.EntireFileMenuItem.Text = "Entire File";
			this.EntireFileMenuItem.Click += new System.EventHandler(this.EntireFileMenuItem_Click);
			// 
			// ShowCharactersMenuItem
			// 
			this.ShowCharactersMenuItem.Image = global::AstroGrep.Properties.Resources.pilcrow_16;
			this.ShowCharactersMenuItem.Name = "ShowCharactersMenuItem";
			this.ShowCharactersMenuItem.Size = new System.Drawing.Size(230, 22);
			this.ShowCharactersMenuItem.Text = "Show All Characters";
			this.ShowCharactersMenuItem.Click += new System.EventHandler(this.ShowCharactersMenuItem_Click);
			// 
			// ViewSep2Separator
			// 
			this.ViewSep2Separator.Name = "ViewSep2Separator";
			this.ViewSep2Separator.Size = new System.Drawing.Size(227, 6);
			// 
			// AllResultsMenuItem
			// 
			this.AllResultsMenuItem.Image = global::AstroGrep.Properties.Resources.preview_results_16;
			this.AllResultsMenuItem.Name = "AllResultsMenuItem";
			this.AllResultsMenuItem.Size = new System.Drawing.Size(230, 22);
			this.AllResultsMenuItem.Text = "All Results";
			this.AllResultsMenuItem.Click += new System.EventHandler(this.AllResultsMenuItem_Click);
			// 
			// ToolsMenu
			// 
			this.ToolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearMRUMenuItem,
            this.ToolsSep1Separator,
            this.SaveSearchOptionsMenuItem,
            this.OptionsMenuItem});
			this.ToolsMenu.Name = "ToolsMenu";
			this.ToolsMenu.Size = new System.Drawing.Size(46, 20);
			this.ToolsMenu.Text = "Tools";
			// 
			// ClearMRUMenuItem
			// 
			this.ClearMRUMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearMRUPathsMenuItem,
            this.ClearMRUTypesMenuItem,
            this.ClearMRUTextsMenuItem,
            this.ClearMRUSep1Separator,
            this.ClearMRUAllMenuItem});
			this.ClearMRUMenuItem.Name = "ClearMRUMenuItem";
			this.ClearMRUMenuItem.Size = new System.Drawing.Size(234, 22);
			this.ClearMRUMenuItem.Text = "&Clear Most Recently Used Lists";
			// 
			// ClearMRUPathsMenuItem
			// 
			this.ClearMRUPathsMenuItem.Name = "ClearMRUPathsMenuItem";
			this.ClearMRUPathsMenuItem.Size = new System.Drawing.Size(141, 22);
			this.ClearMRUPathsMenuItem.Text = "&Search Paths";
			this.ClearMRUPathsMenuItem.Click += new System.EventHandler(this.ClearMRUPathsMenuItem_Click);
			// 
			// ClearMRUTypesMenuItem
			// 
			this.ClearMRUTypesMenuItem.Name = "ClearMRUTypesMenuItem";
			this.ClearMRUTypesMenuItem.Size = new System.Drawing.Size(141, 22);
			this.ClearMRUTypesMenuItem.Text = "&File Types";
			this.ClearMRUTypesMenuItem.Click += new System.EventHandler(this.ClearMRUTypesMenuItem_Click);
			// 
			// ClearMRUTextsMenuItem
			// 
			this.ClearMRUTextsMenuItem.Name = "ClearMRUTextsMenuItem";
			this.ClearMRUTextsMenuItem.Size = new System.Drawing.Size(141, 22);
			this.ClearMRUTextsMenuItem.Text = "Search &Text";
			this.ClearMRUTextsMenuItem.Click += new System.EventHandler(this.ClearMRUTextsMenuItem_Click);
			// 
			// ClearMRUSep1Separator
			// 
			this.ClearMRUSep1Separator.Name = "ClearMRUSep1Separator";
			this.ClearMRUSep1Separator.Size = new System.Drawing.Size(138, 6);
			// 
			// ClearMRUAllMenuItem
			// 
			this.ClearMRUAllMenuItem.Name = "ClearMRUAllMenuItem";
			this.ClearMRUAllMenuItem.Size = new System.Drawing.Size(141, 22);
			this.ClearMRUAllMenuItem.Text = "&All";
			this.ClearMRUAllMenuItem.Click += new System.EventHandler(this.ClearMRUAllMenuItem_Click);
			// 
			// ToolsSep1Separator
			// 
			this.ToolsSep1Separator.Name = "ToolsSep1Separator";
			this.ToolsSep1Separator.Size = new System.Drawing.Size(231, 6);
			// 
			// SaveSearchOptionsMenuItem
			// 
			this.SaveSearchOptionsMenuItem.Name = "SaveSearchOptionsMenuItem";
			this.SaveSearchOptionsMenuItem.Size = new System.Drawing.Size(234, 22);
			this.SaveSearchOptionsMenuItem.Text = "&Save Search Options";
			this.SaveSearchOptionsMenuItem.Click += new System.EventHandler(this.SaveSearchOptionsMenuItem_Click);
			// 
			// OptionsMenuItem
			// 
			this.OptionsMenuItem.Image = global::AstroGrep.Properties.Resources.cog_16;
			this.OptionsMenuItem.Name = "OptionsMenuItem";
			this.OptionsMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.OptionsMenuItem.Size = new System.Drawing.Size(234, 22);
			this.OptionsMenuItem.Text = "&Options...";
			this.OptionsMenuItem.Click += new System.EventHandler(this.OptionsMenuItem_Click);
			// 
			// HelpMenu
			// 
			this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewHelpMenuItem,
            this.ViewRegExHelpMenuItem,
            this.LogFileMenuItem,
            this.HelpSep1Separator,
            this.CheckForUpdateMenuItem,
            this.HelpSep2Separator,
            this.donateToolStripMenuItem,
            this.AboutMenuItem});
			this.HelpMenu.Name = "HelpMenu";
			this.HelpMenu.Size = new System.Drawing.Size(44, 20);
			this.HelpMenu.Text = "&Help";
			// 
			// ViewHelpMenuItem
			// 
			this.ViewHelpMenuItem.Name = "ViewHelpMenuItem";
			this.ViewHelpMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.ViewHelpMenuItem.Size = new System.Drawing.Size(180, 22);
			this.ViewHelpMenuItem.Text = "&View Help";
			this.ViewHelpMenuItem.Click += new System.EventHandler(this.ViewHelpMenuItem_Click);
			// 
			// ViewRegExHelpMenuItem
			// 
			this.ViewRegExHelpMenuItem.Name = "ViewRegExHelpMenuItem";
			this.ViewRegExHelpMenuItem.Size = new System.Drawing.Size(180, 22);
			this.ViewRegExHelpMenuItem.Text = "&Regular Expressions";
			this.ViewRegExHelpMenuItem.Click += new System.EventHandler(this.ViewRegExHelpMenuItem_Click);
			// 
			// LogFileMenuItem
			// 
			this.LogFileMenuItem.Name = "LogFileMenuItem";
			this.LogFileMenuItem.Size = new System.Drawing.Size(180, 22);
			this.LogFileMenuItem.Text = "&Log File";
			this.LogFileMenuItem.Click += new System.EventHandler(this.LogFileMenuItem_Click);
			// 
			// HelpSep1Separator
			// 
			this.HelpSep1Separator.Name = "HelpSep1Separator";
			this.HelpSep1Separator.Size = new System.Drawing.Size(177, 6);
			// 
			// CheckForUpdateMenuItem
			// 
			this.CheckForUpdateMenuItem.Name = "CheckForUpdateMenuItem";
			this.CheckForUpdateMenuItem.Size = new System.Drawing.Size(180, 22);
			this.CheckForUpdateMenuItem.Text = "&Check for Updates...";
			this.CheckForUpdateMenuItem.Click += new System.EventHandler(this.CheckForUpdateMenuItem_Click);
			// 
			// HelpSep2Separator
			// 
			this.HelpSep2Separator.Name = "HelpSep2Separator";
			this.HelpSep2Separator.Size = new System.Drawing.Size(177, 6);
			// 
			// donateToolStripMenuItem
			// 
			this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
			this.donateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.donateToolStripMenuItem.Text = "&Donate";
			this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Name = "AboutMenuItem";
			this.AboutMenuItem.Size = new System.Drawing.Size(180, 22);
			this.AboutMenuItem.Text = "&About AstroGrep";
			this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.splitLeftRight);
			this.Controls.Add(this.pnlRightSide);
			this.Controls.Add(this.pnlSearch);
			this.Controls.Add(this.stbStatus);
			this.Controls.Add(this.MainMenu);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.MainMenu;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AstroGrep";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_Closed);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Shown += new System.EventHandler(this.frmMain_Shown);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearchOptions.ResumeLayout(false);
			this.PanelOptionsContainer.ResumeLayout(false);
			this.pnlMainSearch.ResumeLayout(false);
			this.pnlMainSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBrowse)).EndInit();
			this.pnlRightSide.ResumeLayout(false);
			this.pnlRightSide.PerformLayout();
			this.ResultsToolStrip.ResumeLayout(false);
			this.ResultsToolStrip.PerformLayout();
			this.stbStatus.ResumeLayout(false);
			this.stbStatus.PerformLayout();
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

      }
      
      #endregion

      /// <summary>
      /// Dispose form.
      /// </summary>
      /// <param name="disposing">system parameter</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
            {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }

      private ToolStripMenuItem donateToolStripMenuItem;
      private ToolStrip ResultsToolStrip;
      private ToolStripButton ResultsZoomInButton;
      private ToolStripButton ResultsZoomOutButton;
      private ToolStripButton ResultsZoomResetButton;
      private ToolStripSeparator toolStripSeparator1;
      private ToolStripButton ResultsViewLineNumbersButton;
      private ToolStripButton ResultsViewWordWrapButton;
      private ToolStripButton ResultsViewRemovingLeadingButton;
      private ToolStripButton ResultsViewEntireFileButton;
      private ToolStripButton ResultsViewAllResultsButton;
      private ToolStripSeparator toolStripSeparator2;
      private ToolStripComboBox ResultsContextLinesBeforeCombo;
      private ToolStripSeparator toolStripSeparator3;
      private ToolStripButton ResultsNavPrevButton;
      private ToolStripLabel ResultsContextLinesLabel;
      private ToolStripButton ResultsNavNextButton;
      private ToolStripLabel toolStripLabel2;
      private ToolStripComboBox ResultsContextLinesAfterCombo;
      private ToolStripButton ResultsViewShowCharactersButton;
      private ToolStripMenuItem ShowCharactersMenuItem;
      private LinkLabel lnkPlugins;
      private ToolStripMenuItem ExitAllMenuItem;
   }
}
