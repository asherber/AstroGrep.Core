using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroGrep.Core
{
   /// <summary>
   /// Contains the information for a match within a given line.
   /// </summary>
   /// <remarks>
   /// AstroGrep File Searching Utility. Written by Theodore L. Ward
   /// Copyright (C) 2002 AstroComma Incorporated.
   /// 
   /// This program is free software; you can redistribute it and/or
   /// modify it under the terms of the GNU General Public License
   /// as published by the Free Software Foundation; either version 2
   /// of the License, or (at your option) any later version.
   /// 
   /// This program is distributed in the hope that it will be useful,
   /// but WITHOUT ANY WARRANTY; without even the implied warranty of
   /// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   /// GNU General Public License for more details.
   /// 
   /// You should have received a copy of the GNU General Public License
   /// along with this program; if not, write to the Free Software
   /// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
   /// 
   /// The author may be contacted at:
   /// ted@astrocomma.com or curtismbeard@gmail.com
   /// </remarks>
   /// <history>
   /// [Curtis_Beard]      03/31/2015	ADD: rework Grep/Matches
   /// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines
   /// </history>
   public class MatchResultLine
   {
      private string originalLine = string.Empty;
      private string shortenLine = string.Empty;

      /// <summary>Current line (could be shortened)</summary>
      public string Line
      {
         get { return ShortenLine(); }
         set { originalLine = value; } 
      }

      /// <summary>The original current line</summary>
      public string OriginalLine
      {
         get { return originalLine; }
      }

      /// <summary>Current line number</summary>
      public int LineNumber { get; set; }

      /// <summary>Current column number</summary>
      public int ColumnNumber { get; set; }

      /// <summary>Determines if this line has a match within it</summary>
      public bool HasMatch { get; set; }

      /// <summary>List of line matches</summary>
      public List<MatchResultLineMatch> Matches { get; set; }

      /// <summary>The number of characters to include before and after a match (0 is disabled)</summary>
      public int BeforeAfterCharCount { get; set; }

      /// <summary>The number of characters to indicate a long line</summary>
      public int LongLineCharCount { get; set; }

      /// <summary>
      /// Initializes this class.
      /// </summary>
      /// <history>
      /// [Curtis_Beard]      03/31/2015	ADD: rework Grep/Matches
      /// </history>
      public MatchResultLine()
      {
         Line = string.Empty;
         LineNumber = 1;
         ColumnNumber = 1;
         HasMatch = false;
         Matches = new List<MatchResultLineMatch>();
         LongLineCharCount = 0;
         BeforeAfterCharCount = 0;
      }

      /// <summary>
      /// Retrieves the original line as a shortened down line if <see cref="BeforeAfterCharCount"/> > 0.
      /// </summary>
      /// <returns>The shortened down line or the original line if disabled/not matching</returns>
      /// <remarks>Updates each match's StartPosition if the line changes</remarks>
      /// <history>
      /// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines
      /// </history>
      private string ShortenLine()
      {
         const string More_Text = "...";

         // only trim it once
         if (!string.IsNullOrEmpty(originalLine) && string.IsNullOrEmpty(shortenLine))
         {
            StringBuilder shortenLineBuilder = new StringBuilder();

            if (HasMatch && (BeforeAfterCharCount > 0 && BeforeAfterCharCount < originalLine.Length) && (LongLineCharCount > 0 && originalLine.Length >= LongLineCharCount) && Matches != null && Matches.Count > 0)
            {
               int start = GetStartPosition(Matches[0].StartPosition, BeforeAfterCharCount);
               int end = GetEndPosition(Matches[0].StartPosition, Matches[0].Length, BeforeAfterCharCount, originalLine.Length);
               int currentMatch = 0;
               List<int> includedMatches = new List<int>();

               for (int i = 1; i < Matches.Count; i++)
               {
                  var match = Matches[i];
                  var beforePos = GetStartPosition(match.StartPosition, BeforeAfterCharCount);
                  var afterPos = GetEndPosition(match.StartPosition, match.Length, BeforeAfterCharCount, originalLine.Length);

                  if (beforePos >= start && beforePos <= end)
                  {
                     end = afterPos;
                     includedMatches.Add(i);
                  }
                  else
                  {
                     // valid section for previous match
                     int newStartI = Matches[currentMatch].StartPosition - start + shortenLineBuilder.Length;
                     if (includedMatches.Count > 0)
                     {
                        foreach (var im in includedMatches)
                        {
                           Matches[im].StartPosition = Matches[im].StartPosition - start + shortenLineBuilder.Length;
                        }
                     }
                     if (start != 0)
                     {
                        shortenLineBuilder.Append(More_Text);

                        newStartI += More_Text.Length;

                        if (includedMatches.Count > 0)
                        {
                           foreach (var im in includedMatches)
                           {
                              Matches[im].StartPosition += More_Text.Length;
                           }
                        }
                     }
                     shortenLineBuilder.Append(originalLine.Substring(start, end - start + 1));
                     Matches[currentMatch].StartPosition = newStartI;

                     // start next
                     start = beforePos;
                     end = afterPos;

                     currentMatch++;
                     if (includedMatches.Count > 0)
                     {
                        currentMatch += includedMatches.Count;
                     }
                     includedMatches.Clear();
                  }
               }

               // process any that were not added
               int newStart = Matches[currentMatch].StartPosition - start + shortenLineBuilder.Length;
               if (includedMatches.Count > 0)
               {
                  foreach (var im in includedMatches)
                  {
                     Matches[im].StartPosition = Matches[im].StartPosition - start + shortenLineBuilder.Length;
                  }
               }
               if (start != 0)
               {
                  shortenLineBuilder.Append(More_Text);
                  newStart += More_Text.Length;

                  if (includedMatches.Count > 0)
                  {
                     foreach (var im in includedMatches)
                     {
                        Matches[im].StartPosition += More_Text.Length;
                     }
                  }
               }
               shortenLineBuilder.Append(originalLine.Substring(start, end - start + 1));
               Matches[currentMatch].StartPosition = newStart;

               if (end < originalLine.Length - 1)
               {
                  shortenLineBuilder.Append(More_Text);
               }
            }
            else
            {
               shortenLineBuilder.Append(originalLine);
            }

            shortenLine = shortenLineBuilder.ToString();
         }

         return shortenLine;
      }

      /// <summary>
      /// Retrieve the start position based on the current start position and the <paramref name="beforeAfterCount"/>.
      /// </summary>
      /// <param name="position">The current start position</param>
      /// <param name="beforeAfterCount">The before/after character count</param>
      /// <returns>The new calculated start position</returns>
      /// <history>
      /// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines
      /// </history>
      private int GetStartPosition(int position, int beforeAfterCount)
      {
         int start = position - beforeAfterCount;

         if (start < 0)
         {
            start = 0;
         }

         return start;
      }

      /// <summary>
      /// Retrieve the end position based on the current start position and the <paramref name="beforeAfterCount"/>.
      /// </summary>
      /// <param name="position">The current start position</param>
      /// <param name="length">The length of the match</param>
      /// <param name="beforeAfterCount">The before/after character count</param>
      /// <param name="maxLength">The maximum length of the line</param>
      /// <returns>The calculated end position</returns>
      /// <history>
      /// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines
      /// </history>
      private int GetEndPosition(int position, int length, int beforeAfterCount, int maxLength)
      {
         int end = position + length + beforeAfterCount - 1;

         if (end > maxLength)
         {
            end = maxLength - 1;
         }

         return end;
      }
   }
}
