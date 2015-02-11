using System;

namespace AstroGrep
{
   /// <summary>
   /// Constant values for use in this application.
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
   /// [Curtis_Beard]		07/20/2006	Created
   /// [Curtis_Beard]		11/02/2006	ADD: Constants for plugin separators
   /// [Curtis_Beard]	   01/31/2012	CHG: 1947760, update default exclude list to exclude images (bmp,gif,jpg,jpeg,png)
   /// [Curtis_Beard]		02/24/2012	Created: 3488321, ability to change results font
   /// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
   /// [Curtis_Beard]		10/27/2014	CHG: 88, add file extension column
   /// [Mark_Guerra]       11/06/2014  CHG: 91, exclude compiled java classes and compiled windows html help files (class,chm)
   /// </history>
   public class Constants
   {
      // Maximum value constants
      /// <summary>Maximum number of mru paths allowed</summary>
      public const int MAX_STORED_PATHS = 25;
      /// <summary>Maximum number of context lines allowed</summary>
      public const int MAX_CONTEXT_LINES = 10;

      /// <summary>Separator for search entries</summary>
      public static string SEARCH_ENTRIES_SEPARATOR = "|;;|";
      /// <summary>Separator for colors</summary>
      public static string COLOR_SEPARATOR = "-";
      /// <summary>Separator for fonts</summary>
      public static string FONT_SEPARATOR = "||";
      /// <summary>Separator for text editor</summary>
      public static string TEXT_EDITOR_SEPARATOR = "|;;|";
      /// <summary>Separator for text editor file types</summary>
      public static string TEXT_EDITOR_TYPE_SEPARATOR = "|";
      /// <summary>Separator for plugins</summary>
      public static string PLUGIN_SEPARATOR = "|;;|";
      /// <summary>Separator for plugin arguments</summary>
      public static string PLUGIN_ARGS_SEPARATOR = "|@@|";

      // ListView column index constants
      /// <summary>File Index</summary>
      public const int COLUMN_INDEX_FILE = 0;
      /// <summary>Directory Index</summary>
      public const int COLUMN_INDEX_DIRECTORY = 1;
      /// <summary>File Extension Index</summary>
      public const int COLUMN_INDEX_FILE_EXT = 2;
      /// <summary>Date Index</summary>
      public const int COLUMN_INDEX_DATE = 3;
      /// <summary>Size Index</summary>
      public const int COLUMN_INDEX_SIZE = 4;
      /// <summary>Count Index</summary>
      public const int COLUMN_INDEX_COUNT = 5;
      /// <summary>Grep Index Index</summary>
      public const int COLUMN_INDEX_GREP_INDEX  = 6;   //Must be last

      /// <summary>Identifier for all file types</summary>
      public const string ALL_FILE_TYPES = "*";

      /// <summary>Default language</summary>
      public static string DEFAULT_LANGUAGE = "en-us";

      /// <summary>Default extension exclusion list</summary>
      private static string DEFAULT_EXTENSION_EXCLUDE_LIST = ".exe;.dll;.pdb;.msi;.sys;.ppt;.gif;.jpg;.jpeg;.png;.bmp;.class;.chm";

      /// <summary>Product name</summary>
      public const string ProductName = "AstroGrep";

      /// <summary>Product Location</summary>
      public static string ProductLocation
      {
         get
         {
            System.IO.FileInfo file = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            return file.Directory.FullName;
         }
      }

      /// <summary>Product FullName to executing file.</summary>
      public static string ProductFullName
      {
         get
         {
            System.IO.FileInfo file = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            return file.FullName;
         }
      }

      /// <summary>
      /// Gets the current product version.
      /// </summary>
      public static Version ProductVersion
      {
         get
         {
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return _assembly.GetName().Version;
         }
      }

      /// <summary>
      /// Gets the default exclusions
      /// </summary>
      /// <history>
      /// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
      /// [Curtis_Beard]	   11/06/2014	CHG: update to FilterItem
      /// </history>
      public static string DefaultFilterItems
      {
         get
         {
            return libAstroGrep.FilterItem.ConvertFilterItemsToString(GetDefaultFilterItemsList());
         }
      }

      /// <summary>
      /// Converts the default extensions list to the a FilterItem list.
      /// </summary>
      /// <returns>List of FilterItems</returns>
      /// <history>
      /// [Curtis_Beard]	   03/07/2012	ADD: 3131609, exclusions
      /// [Curtis_Beard]	   09/18/2013	ADD: 56, add suggested default directory names to exclude for common apps
      /// [Curtis_Beard]	   11/06/2014	CHG: update to FilterItem
      /// </history>
      private static System.Collections.Generic.List<libAstroGrep.FilterItem> GetDefaultFilterItemsList()
      {
         var list = new System.Collections.Generic.List<libAstroGrep.FilterItem>();

         // default extensions
         var exts = DEFAULT_EXTENSION_EXCLUDE_LIST.Split(';');
         foreach (var ext in exts)
         {
            var item = new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.File, libAstroGrep.FilterType.SubCategories.Extension), ext, libAstroGrep.FilterType.ValueOptions.None, false, true);
            list.Add(item);
         }

         // default directory names to ignore (Git, Mercurial, SVN, CVS, Eclipse Metadata, Eclipse Settings)
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".git", libAstroGrep.FilterType.ValueOptions.Equals, false, true));
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".hg", libAstroGrep.FilterType.ValueOptions.Equals, false, true));
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".svn", libAstroGrep.FilterType.ValueOptions.Equals, false, true));
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".cvs", libAstroGrep.FilterType.ValueOptions.Equals, false, true));
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".metadata", libAstroGrep.FilterType.ValueOptions.Equals, false, true));
         list.Add(new libAstroGrep.FilterItem(new libAstroGrep.FilterType(libAstroGrep.FilterType.Categories.Directory, libAstroGrep.FilterType.SubCategories.Name), ".settings", libAstroGrep.FilterType.ValueOptions.Equals, false, true));

         return list;
      }
   }
}
