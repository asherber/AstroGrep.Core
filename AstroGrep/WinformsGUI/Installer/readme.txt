Changelog for AstroGrep v4.4.8
===================================================================
Bugs
-094: AstroGrep cant seem to search for japanese characters properly (tied with #115)
-111: Open File from search results fails for some menu selections (more logging)
-114: Disabled exclusions become enabled over time
-115: Improve small file and non-BOM encoding detection
-116: IFilterPlugin file full path
-117: IFilterPlugin txt file in zip archive
-119: v4.4.7 RegEx not working. .NET 4.8 installed. (New Word Plugin for .docx can do RegEx now)
-120: [CRITICAL] Astrogrep executes AutoOpen macros regardless of security settings
-122: Typo in size units (French, StrFormatByteSize issue with buffer length)
-123: Exclusions apply to search path (don't apply exclusions to initial search paths)

Featured Requests:
-055: Create plugin for Microsoft Excel files
-105: Show the context of a file only if left click
-117: Print Results... Emphasize search text
-130: Filetypes grouping
-131: GUI's Tab Stops
-135: AstroGrep missing feature: no navigation between search results (part of new results viewer toolbar)
-138: Add Close All menu item (File->Exit All menu item)
-141: Custom key combination to toggle between files and results (properly handle tab/shift-tab in results view)
-142: Make context lines switch dynamically instead of requiring a new search
-143: Show context before the matched line (x lines before /​ y lines after)
-144: Add ability to exclude more than one extension at once

Patches:
-008: Translation polish for German language file
-010: Post-build event fix (support spaces in path names)
-011: Precompilation of regex before search (moved all common regex logic to one place and build it outside any loops)

Support Requests:
N/A

Other:
- Now requires .Net framework v4.5 (which removes Windows XP support)
- Microsoft Word Plugin will now only be used for older .doc files, where Microsoft Word Open XML plugin will be used for .docx files going forward
- Created a new PDF plugin to better support text based pdf files (does not handle ocr of pdf images)
- Created a new Media Tags plugin to allow searching within media file tags
- Moved search plugins from options screen to main search options area (above Exclusions)
- Added a results viewer toolbar to give direct access to viewing options for the results viewer (line numbers, context lines, show all characters, etc.)
- Cleaned up regular expression creation
- Switched some code to use newer .Net 4.5 features
- Rework unhandled exception handler
- Correct spelling error in en-us language