using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

// Help information for this assembly
[assembly: CommandLine.Text.AssemblyUsage("Search for files and within file contents.")]

namespace AstroGrep.Windows
{
	/// <summary>
	///
	/// </summary>
	public class CLOptions
	{
		/// <summary>
		/// Allowed export types.
		/// </summary>
		public enum ExportTypes
		{
			/// <summary>Text</summary>
			TXT,

			/// <summary>XML</summary>
			XML,

			/// <summary>JSON</summary>
			JSON,

			/// <summary>HTML</summary>
			HTML
		}

		/// <summary></summary>
		[Option("cl", HelpText = "Number of context lines", Default = -1)]
		public int ContextLines { get; set; }

		/// <summary></summary>
		[Option("cla", HelpText = "Number of context lines after", Default = -1)]
		public int ContextLinesAfter { get; set; }

		/// <summary></summary>
		[Option("clb", HelpText = "Number of context lines before", Default = -1)]
		public int ContextLinesBefore { get; set; }

		/// <summary></summary>
		[Option("exit", HelpText = "Exit application after search", Default = false)]
		public bool ExitAfterSearch { get; set; }

		/// <summary></summary>
		[Option("filetypes", HelpText = "File Types")]
		public string FileTypes { get; set; }

		/// <summary></summary>
		[Option("c", HelpText = "Case sensitive", Default = false)]
		public bool IsCaseSensitive { get; set; }

		/// <summary></summary>
		[Option("f", HelpText = "File names only", Default = false)]
		public bool IsFileNamesOnly { get; set; }

		/// <summary></summary>
		[Option("n", HelpText = "Negation", Default = false)]
		public bool IsNegation { get; set; }

		/// <summary></summary>
		[Option("w", HelpText = "Whole word", Default = false)]
		public bool IsWholeWord { get; set; }

		/// <summary></summary>
		[Option("minfc", HelpText = "Minimum file count", Default = 0)]
		public int MinFileCount { get; set; }

		/// <summary></summary>
		[Option("opath", HelpText = "Save results to path (start search implied)")]
		public string OutputPath { get; set; }

		/// <summary></summary>
		[Option("otype", HelpText = "Save results type (JSON,HTML,XML,TXT [default])", Default = ExportTypes.TXT)]
		public ExportTypes OutputType { get; set; }

		/// <summary>
		///
		/// </summary>
		[Value(0, Min = 1, HelpText = "The full path to the directory to search (or supply more than one).")]
		public IEnumerable<string> Paths { get; set; }

		/// <summary></summary>
		[Option("stext", HelpText = "The text to search for within each file", Default = "")]
		public string SearchText { get; set; }

		/// <summary></summary>
		[Option("shd", HelpText = "Skip hidden directories", Default = false)]
		public bool SkipHiddenDirectory { get; set; }

		/// <summary></summary>
		[Option("shf", HelpText = "Skip hidden files", Default = false)]
		public bool SkipHiddenFile { get; set; }

		/// <summary></summary>
		[Option("sh", HelpText = "Skip hidden files and folders", Default = false)]
		public bool SkipHiddenFileAndDirectory { get; set; }

		/// <summary></summary>
		[Option("srf", HelpText = "Skip read-only files", Default = false)]
		public bool SkipReadOnlyFile { get; set; }

		/// <summary></summary>
		[Option("ssd", HelpText = "Skip system directories", Default = false)]
		public bool SkipSystemDirectory { get; set; }

		/// <summary></summary>
		[Option("ssf", HelpText = "Skip system files", Default = false)]
		public bool SkipSystemFile { get; set; }

		/// <summary></summary>
		[Option("ss", HelpText = "Skip system files and folders", Default = false)]
		public bool SkipSystemFileAndDirectory { get; set; }

		/// <summary></summary>
		[Option("s", HelpText = "Start searching immediately", Default = false)]
		public bool StartSearch { get; set; }

		/// <summary></summary>
		[Option("l", HelpText = "Line numbers", Default = false)]
		public bool UseLineNumbers { get; set; }

		/// <summary></summary>
		[Option("r", HelpText = "Search within sub folders", Default = false)]
		public bool UseRecursion { get; set; }

		/// <summary></summary>
		[Option("e", HelpText = "Use regular expressions", Default = false)]
		public bool UseRegularExpressions { get; set; }
	}
}