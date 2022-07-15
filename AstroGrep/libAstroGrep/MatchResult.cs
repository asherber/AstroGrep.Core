using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace libAstroGrep
{
	/// <summary>
	/// Contains all the information for all the matches within a file.
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
	/// [Curtis_Beard]      05/18/2020	CHG: detect when match result lines are added/removed to calculate total line hit count instead of when requested
	/// </history>
	public class MatchResult
	{
		private readonly System.Collections.ObjectModel.ObservableCollection<MatchResultLine> matches = new System.Collections.ObjectModel.ObservableCollection<MatchResultLine>();
		private Encoding detectedEncoding = null;
		private FileInfo file = null;
		private bool fromPlugin = false;

		private int totalLineHitCount = 0;

		/// <summary>
		/// Initializes this MatchResult with the current FileInfo.
		/// </summary>
		/// <param name="file">Current FileInfo</param>
		/// <history>
		/// [Curtis_Beard]	   03/31/2015	Created
		/// </history>
		public MatchResult(FileInfo file)
		{
			File = file;
			HitCount = 0;
			matches.CollectionChanged += Matches_CollectionChanged;
		}

		/// <summary>
		/// Gets/Sets the detected file encoding.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   12/01/2014	Created
		/// </history>
		public Encoding DetectedEncoding
		{
			get { return detectedEncoding; }
			set { detectedEncoding = value; }
		}

		/// <summary>
		/// Gets/Sets the current FileInfo for this MatchResult.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   03/31/2015	Created
		/// </history>
		public FileInfo File
		{
			get { return file; }
			set { file = value; }
		}

		/// <summary>
		/// Gets/Sets whether this MatchResult is from a plugin.
		/// </summary>
		public bool FromPlugin
		{
			get { return fromPlugin; }
			set { fromPlugin = value; }
		}

		/// <summary>
		/// Gets the total hit count in the object
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   11/21/2005	Created
		/// </history>
		public int HitCount { get; private set; }

		/// <summary>
		/// Gets/Sets the Index of the hit in the collection
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/09/2005	Created
		/// </history>
		public int Index { get; set; }

		/// <summary>
		/// Gets the total number of lines that contain a hit.
		/// </summary>
		public int LineHitCount
		{
			get
			{
				return totalLineHitCount;
			}
		}

		/// <summary>
		/// Gets/Sets all the MatchResultLines for this MatchResult.
		/// </summary>
		public System.Collections.ObjectModel.ObservableCollection<MatchResultLine> Matches
		{
			get { return matches; }
		}

		/// <summary>
		/// Gets all the MatchResultLines for this MatchResult for display based on the number of context lines desired.
		/// </summary>
		/// <param name="beforeContextLines">The number of context lines before a match</param>
		/// <param name="afterContextLines">The number of context lines after a match</param>
		/// <returns>A list of MatchResultLines for display purposes (context line based)</returns>
		/// <history>
		/// [Curtis_Beard]	   08/20/2019	ADD: 142, dynamically display context lines
		/// </history>
		public List<MatchResultLine> GetDisplayMatches(int beforeContextLines, int afterContextLines)
		{
			List<MatchResultLine> displayMatches = new List<MatchResultLine>();

			if (beforeContextLines < 0)
			{
				beforeContextLines = 0;
			}

			if (afterContextLines < 0)
			{
				afterContextLines = 0;
			}

			for (int i = 0; i < matches.Count; i++)
			{
				if (matches[i].HasMatch)
				{
					// before context lines
					bool addSeparator = false;
					for (int j = beforeContextLines; j > 0; j--)
					{
						if (i - j >= 0)
						{
							if (!displayMatches.Contains(matches[i - j]))
							{
								// blank line between matches/context lines that aren't together
								if (displayMatches.Count > 0 && addSeparator == false)
								{
									int firstCL = i - j;
									if (!displayMatches.Select(d => d.LineNumber).Contains(matches[firstCL].LineNumber) && matches[firstCL].LineNumber - 1 != displayMatches[displayMatches.Count - 1].LineNumber)
									{
										displayMatches.Add(new MatchResultLine() { LineNumber = -1 });
										addSeparator = true;
									}
								}

								displayMatches.Add(matches[i - j]);
							}
						}
					}

					// actual match line
					if (!displayMatches.Contains(matches[i]))
					{
						displayMatches.Add(matches[i]);
					}

					// after context lines
					for (int j = 1; j <= afterContextLines; j++)
					{
						if (i + j < matches.Count)
						{
							if (!displayMatches.Contains(matches[i + j]))
							{
								displayMatches.Add(matches[i + j]);
							}
						}
					}
				}
			}

			return displayMatches;
		}

		/// <summary>
		/// Retrieves the first MatchResultLine from the MatchResult list.
		/// </summary>
		/// <returns>First MatchResultLine that contains a match, otherwise null</returns>
		/// <history>
		/// [Curtis_Beard]	   03/31/2015	Created
		/// </history>
		public MatchResultLine GetFirstMatch()
		{
			return matches.Where(m => m.HasMatch).FirstOrDefault();
		}

		/// <summary>
		/// Updates the total hit count
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   11/21/2005	Created
		/// </history>
		public void SetHitCount()
		{
			SetHitCount(1);
		}

		/// <summary>
		/// Updates the total hit count
		/// </summary>
		/// <param name="count">Value to add count to total</param>
		/// <history>
		/// [Curtis_Beard]	   11/21/2005	Created
		/// </history>
		public void SetHitCount(int count)
		{
			HitCount += count;
		}

		/// <summary>
		/// Handle updating the total line hit count based on <see cref="MatchResultLine"/> added or removed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	   05/18/2020	Created
		/// </history>
		private void Matches_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				// add
				totalLineHitCount += e.NewItems.Cast<MatchResultLine>().Where(m => m.HasMatch).Count();
			}
			else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
			{
				// subtract
				totalLineHitCount -= e.OldItems.Cast<MatchResultLine>().Where(m => m.HasMatch).Count();
			}
		}
	}
}