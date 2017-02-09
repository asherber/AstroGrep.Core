using libAstroGrep.EncodingDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libAstroGrep
{
    /// <summary>
    /// Implement ISearchSpec interface.
    /// </summary>
    /// <history>
    /// [Curtis_Beard]		12/01/2014	Moved from frmMain.cs
    /// [Curtis_Beard]      02/09/2015	CHG: 92, support for specific file encodings
    /// [Curtis_Beard]      04/07/2015	CHG: remove line numbers
    /// [Curtis_Beard]	   05/26/2015	FIX: 69, add performance setting for file detection
    /// </history>
    public class SearchSpec : ISearchSpec
    {
        /// <summary>starting directories</summary>
        public List<string> StartDirectories { get; set; }

        /// <summary>starting full file paths which if defined will ignore StartDirectories</summary>
        public List<string> StartFilePaths { get; set; }

        /// <summary>search in sub folders</summary>
        public bool SearchInSubfolders { get; set; }

        /// <summary>search text will be used as a regular expression</summary>
        public bool UseRegularExpressions { get; set; }

        /// <summary>enable case sensitive searching</summary>
        public bool UseCaseSensitivity { get; set; }

        /// <summary>enable whole word matching</summary>
        public bool UseWholeWordMatching { get; set; }

        /// <summary>enable detecting files that don't match the search text</summary>
        public bool UseNegation { get; set; }

        /// <summary>number of context lines (0 is default)</summary>
        public int ContextLines { get; set; }

        /// <summary>the text to find</summary>
        public string SearchText { get; set; }

        /// <summary>enable only processing the file up until one match is found</summary>
        public bool ReturnOnlyFileNames { get; set; }

        /// <summary>list of FileEncodings objects to force encoding of certain files selected by user</summary>
        public List<FileEncoding> FileEncodings { get; set; }

        /// <summary>Current encoding options set by user</summary>
        public EncodingOptions EncodingDetectionOptions { get; set; }

        public SearchSpec()
        {
            StartDirectories = new List<string>();
            StartFilePaths = new List<string>();
            FileEncodings = new List<FileEncoding>();
            EncodingDetectionOptions = new EncodingDetection.EncodingOptions();
        }
    }
}
