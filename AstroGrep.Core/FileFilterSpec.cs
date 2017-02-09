using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroGrep.Core
{
    /// <summary>
    /// Implement IFileFilterSpec interface.
    /// </summary>
    /// <history>
    /// [Curtis_Beard]		12/01/2014	Moved from frmMain.cs, adjusted to match new interface
    /// </history>
    public class FileFilterSpec : IFileFilterSpec
    {
        /// <summary>List of file filters used when retrieving files from a directory (multiples separated by , or ; )</summary>
        public string FileFilter { get; set; }

        /// <summary>List of FilterItems that can be used to filter out files/directories based on certain features of said file/directory.</summary>
        public List<FilterItem> FilterItems { get; set; }

        public FileFilterSpec()
        {
            FilterItems = new List<FilterItem>();
        }
    }
}
