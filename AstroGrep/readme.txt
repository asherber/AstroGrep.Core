AstroGrep
-------------------------------------------------------------------------------
AstroGrep is a Microsoft Windows GUI File Searching (grep) utility. Its 
features include regular expressions, versatile printing options, stores most 
recent used paths and has a "context" feature which is very nice for looking at 
source code.


Build Instructions
-------------------------------------------------------------------------------
Visual Studio 2019 (Community is used/supported)
- .Net Framework 4.5

Visual Studio Extensions:
- CodeMaid (Used)
- EditorConfig Language Service (Used)
- Fix Mixed Tabs (Optional)
- Match Margin (Optional)
- Power Commands for Visual Studio (Optional)
- VSColorOutput (Optional)
- Copy as HTML (Optional)
- Visual Studio Spell Checker (VS2017 and Later) (Optional)

Nuget Packages Used:
- AvalonEdit (Results preview display)
- CommandLineParser (Future use for command line parser)
- DocumentFormat.OpenXml (Microsoft Word plugin)
- ExcelDataReader (Microsoft Excel plugin)
- ExcelNumberFormat (Microsoft Excel plugin)
- NLog (Logging)
- SharpZipLib (Part of AvalonEdit)
- TagLibSharp (Media Tag plugin)

1. Grab the latest (head) revision from SVN under the /trunk/AstroGrep folder
2. Open the AstroGrep.sln solution file in VS
3. Do a full rebuild (which should pull in the Nuget packages)
	