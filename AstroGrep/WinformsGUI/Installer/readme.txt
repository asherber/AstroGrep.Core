Changelog for AstroGrep v4.4.7
===================================================================
Bugs
-85: Possible issue with word plugin and leaving winword.exe process open.
-98: Error "the string was not recognized as a valid DateTime"
-100: Performance issues
-101: Searching Multiple MS Word Documents
-102: Context Lines Display Discrepancy
-103: Astrogrep 4.4.6 hangs clicking on found file
-104: commandline spath not accepting multiple searchPath
-108: Used ListSeparator on right mouse "Copy all"
-109: Command Line issues - Check logic and docs
-113: Feature 108 is not working (Add additional text editor parameter for search text)

Featured Requests:
-101: Stopped painting status bar as often
-110: Exclude directories that do not match pattern (added not equals option for path based options)
-119: Added line hit count to count column values (format: total / line in current Count column)
-122: Add option to only show x chars before/​after matched text
-125: Support to add many search target directories from UI (Hold down shift and click the folder icon to append a path)
-129: Added total files search to File search bar text (format: found / searched)
-136: Scanning all files as UTF-8 encoded (Tools -> Options -> File Encoding, Choose a Force Encoding from the list.  Selecting the blank (first) will disable)

Patches:
-5: Caching translation strings for most common texts in IDE
-7: Remove white space not working when displaying entire file

Support Requests:
-8: Silent Install (supported by /S command line to the installer.  Note: this defaults to English)
-10: Files without extensions implicitly excluded (bug fix)

Other:
- Fixed issue with Spanish language where it wouldn't load
- Use extension method to check for InvokeRequired instead of defining delegates