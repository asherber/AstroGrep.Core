using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using libAstroGrep;

namespace AstroGrep.Output
{
   /// <summary>
   /// All export settings.
   /// </summary>
   public class MatchResultsExportSettings
   {
      /// <summary>File path</summary>
      public string Path { get; set; }

      /// <summary>Grep object containing settings and all results</summary>
      public Grep Grep { get; set; }

      /// <summary>The indexes of the grep's MatchResults to export</summary>
      public List<int> GrepIndexes { get; set; }

      /// <summary>Determines whether to show line numbers</summary>
      public bool ShowLineNumbers { get; set; }

      /// <summary>Determines whether to trim leading white space</summary>
      public bool RemoveLeadingWhiteSpace { get; set; }

      /// <summary>The number of context lines before a matched line</summary>
      public int ContextLinesBefore { get; set; }

      /// <summary>The number of context lines after a matched line</summary>
      public int ContextLinesAfter { get; set; }
   }
}
