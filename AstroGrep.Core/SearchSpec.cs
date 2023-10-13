using AstroGrep.Core.EncodingDetection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AstroGrep.Core
{
    /// <summary>
    /// Implement ISearchSpec interface.
    /// </summary>
    /// <history>
    /// [Curtis_Beard]		12/01/2014	Moved from frmMain.cs
    /// [Curtis_Beard]      02/09/2015	CHG: 92, support for specific file encodings
    /// [Curtis_Beard]      04/07/2015	CHG: remove line numbers
    /// [Curtis_Beard]	   05/26/2015	FIX: 69, add performance setting for file detection
    /// [Curtis_Beard]	   09/29/2016	CHG: 24/115, use one interface for search in prep for saving to file
    /// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines support
    /// </history>
    /// From WinformsGUI\Core\SearchInterfaces.cs
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

        /// <summary>List of file filters used when retrieving files from a directory (multiples separated by , or ; )</summary>
        public string FileFilter { get; set; }

        /// <summary>The number of characters to include before and after a match (0 is disabled)</summary>
        public int BeforeAfterCharCount { get; set; }

        /// <summary>The number of characters to indicate a long line</summary>
        public int LongLineCharCount { get; set; }

        /// <summary>List of FilterItems that can be used to filter out files/directories based on certain features of said file/directory.</summary>
        public List<FilterItem> FilterItems { get; set; }

        /// <summary>
        /// Comment about current instance of the class.
        /// </summary>
        public string Comment { get; set; }


        /// <summary>
        /// Gets the full location to the config file.
        /// </summary>
        static public string Location
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                    "AstroGrep.SearchSpec.config");
            }
        }

        /// <summary>
        /// Saves current class to default location.
        /// </summary>
        public void Save()
        {
            Save(Location);

            //var spec = (SearchInterfaces.SearchSpec)GetSearchSpecFromUI(false);
            //spec.Save();
        }

        /// <summary>
        /// Saves current class to given location.
        /// </summary>
        /// <param name="path">Full path to file</param>
        public void Save(string path)
        {
            using (var writer = new System.IO.StreamWriter(path))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        /// <summary>
        /// Loads the default file to a class instance.
        /// </summary>
        /// <returns>Instance of SearchSpec</returns>
        static public SearchSpec Load()
        {
            return Load(Location);
        }

        /// <summary>
        /// Loads give file to a class instance.
        /// </summary>
        /// <param name="path">Full path to file</param>
        /// <returns>Instance of SearchSpec</returns>
        static public SearchSpec Load(string path)
        {
            using (var reader = new System.IO.StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(SearchSpec));
                return (SearchSpec)serializer.Deserialize(reader);
            }
        }
    }
}
