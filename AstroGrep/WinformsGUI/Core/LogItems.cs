using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AstroGrep.Core
{
   /// <summary>
   /// Container/Helper methods around log items collection.
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
   /// [Curtis_Beard]	   08/15/2017	FIX: 100, performance changes
   /// </history>
   public class LogItems
   {
      private readonly List<LogItem> logItems = null;
      private readonly Dictionary<LogItem.LogItemTypes, int> typesDict = null;

      /// <summary>
      /// Create an instance of this class.
      /// </summary>
      public LogItems()
      {
         logItems = new List<LogItem>();

         typesDict = new Dictionary<LogItem.LogItemTypes, int>();
         typesDict.Add(LogItem.LogItemTypes.Error, 0);
         typesDict.Add(LogItem.LogItemTypes.Exclusion, 0);
         typesDict.Add(LogItem.LogItemTypes.Status, 0);
      }

      /// <summary>
      /// Internal initialize, mainly for cloning.
      /// </summary>
      /// <param name="items">List of LogItems</param>
      /// <param name="types">Dictionary of types</param>
      private LogItems(List<LogItem> items, Dictionary<LogItem.LogItemTypes, int> types)
      {
         logItems = items;
         typesDict = types;
      }

      /// <summary>
      /// Clone the current class.
      /// </summary>
      /// <returns>LogItems copy of current class</returns>
      public LogItems Clone()
      {
         return new LogItems(logItems.ToList(), typesDict);
      }

      /// <summary>
      /// Add a log item.
      /// </summary>
      /// <param name="item">Item to add</param>
      public void Add(LogItem item)
      {
         logItems.Add(item);

         switch (item.ItemType)
         {
            case LogItem.LogItemTypes.Error:
               typesDict[LogItem.LogItemTypes.Error]++;
               break;

            case LogItem.LogItemTypes.Exclusion:
               typesDict[LogItem.LogItemTypes.Exclusion]++;
               break;

            case LogItem.LogItemTypes.Status:
               typesDict[LogItem.LogItemTypes.Status]++;
               break;
         }
      }

      /// <summary>
      /// Clear the log items.
      /// </summary>
      public void Clear()
      {
         logItems.Clear();

         typesDict[LogItem.LogItemTypes.Error] = 0;
         typesDict[LogItem.LogItemTypes.Exclusion] = 0;
         typesDict[LogItem.LogItemTypes.Status] = 0;
      }

      /// <summary>
      /// Get the total count of log items.
      /// </summary>
      public int Count
      {
         get 
         { 
            return logItems.Count;
         }
      }

      /// <summary>
      /// Get the specific item type count.
      /// </summary>
      /// <param name="itemType">Type to count</param>
      /// <returns>total item type</returns>
      public int CountByType(LogItem.LogItemTypes itemType)
      {
         return typesDict[itemType];
      }

      /// <summary>
      /// Gets all the LogItems by a given LogItemType.
      /// </summary>
      /// <param name="itemType">Type to retrieve</param>
      /// <returns>List of LogItems</returns>
      public List<LogItem> GetItemsByType(LogItem.LogItemTypes itemType)
      {
         return logItems.FindAll(l => l.ItemType == itemType);
      }

      /// <summary>
      /// Update a log item's details to include the additional details.
      /// </summary>
      /// <param name="file">Current file</param>
      /// <param name="additionalDetails">Additional details</param>
      public void UpdateItemDetails(FileInfo file, string additionalDetails)
      {
         const string languageLookupText = "SearchSearching";

         for (int i = logItems.Count - 1; i >= 0; --i)
         {
            var item = logItems[i];

            if (item.Value == languageLookupText &&
               item.Details == file.FullName)
            {
               item.Details = string.Format("{0}||{1}", file.FullName, additionalDetails);

               return;
            }
         }
      }
   }
}
