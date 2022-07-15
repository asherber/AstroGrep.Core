using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using libAstroGrep;
using libAstroGrep.Plugin;

namespace AstroGrep.Plugins.FileHandlers
{
	/// <summary>
	/// Used to search any file that has an iFilter module loaded in the system.
	/// </summary>
	/// <remarks>
	///   AstroGrep File Searching Utility. Written by Theodore L. Ward
	///   Copyright (C) 2002 AstroComma Incorporated.
	///
	///   This program is free software; you can redistribute it and/or
	///   modify it under the terms of the GNU General public License
	///   as published by the Free Software Foundation; either version 2
	///   of the License, or (at your option) any later version.
	///
	///   This program is distributed in the hope that it will be useful,
	///   but WITHOUT ANY WARRANTY; without even the implied warranty of
	///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	///   GNU General public License for more details.
	///
	///   You should have received a copy of the GNU General public License
	///   along with this program; if (not, write to the Free Software
	///   Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
	///
	///   The author may be contacted at:
	///   ted@astrocomma.com or curtismbeard@gmail.com
	/// </remarks>
	/// <history>
	/// [Curtis_Beard]      10/17/2012  Created
	/// [Curtis_Beard]      03/31/2015	CHG: rework Grep/Matches
	/// </history>
	public class FileHandlersPlugin : IAstroGrepPlugin
	{
		private bool __IsAvailable;

		/// <summary>
		/// Initializes a new instance of the IFilterPlugin class.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		public FileHandlersPlugin()
		{
			__IsAvailable = true;
		}

		/// <summary>
		/// Handles destruction of the object.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		~FileHandlersPlugin()
		{
			this.Dispose();
		}

		/// <summary>
		/// Gets the author of the plugin.
		/// </summary>
		public string Author
		{
			get { return "The AstroGrep Team"; }
		}

		/// <summary>
		/// Gets the description of the plugin.
		/// </summary>
		public string Description
		{
			get { return "Searches documents using the system file handler (IFilter) for the given file extension.  Currently doesn't support Context lines or Line Numbers.  Can be slower."; }
		}

		/// <summary>
		/// Gets the valid extensions for this grep type.
		/// </summary>
		/// <remarks>Comma separated list of strings.</remarks>
		public string Extensions
		{
			get { return "File Handlers"; }
		}

		/// <summary>
		/// Checks to see if the plugin is available on this system.
		/// </summary>
		public bool IsAvailable
		{
			get { return __IsAvailable; }
		}

		/// <summary>
		/// Gets the name of the plugin.
		/// </summary>
		public string Name
		{
			get { return "File Handlers"; }
		}

		/// <summary>
		/// Gets the version of the plugin.
		/// </summary>
		public string Version
		{
			get { return "1.2.0"; }
		}

		/// <summary>
		/// Handles disposing of the object.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		public void Dispose()
		{
			__IsAvailable = false;
		}

		/// <summary>
		/// Searches the given file for the given search text.
		/// </summary>
		/// <param name="file">FileInfo object</param>
		/// <param name="searchSpec">ISearchSpec interface value</param>
		/// <param name="ex">Exception holder if error occurs</param>
		/// <returns>Hitobject containing grep results, null if on error</returns>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// [Curtis_Beard]      03/31/2015	CHG: rework Grep/Matches
		/// [Curtis_Beard]      01/16/2019	FIX: 103, CHG: 122, trim long lines support
		/// </history>
		public MatchResult Grep(FileInfo file, ISearchSpec searchSpec, ref Exception ex)
		{
			// initialize Exception object to null
			ex = null;
			MatchResult match = null;

			if (IsFileSupported(file))
			{
				try
				{
					Regex reg = libAstroGrep.Grep.BuildSearchRegEx(searchSpec);
					byte[] allBytes = File.ReadAllBytes(file.FullName);
					using (FilterReader reader = new FilterReader(allBytes))
					{
						reader.Init();

						string fileContent = reader.ReadToEnd();

						if (!string.IsNullOrEmpty(fileContent))
						{
							string[] lines = fileContent.Split(new char[] { '\n', '\r' });
							for (int i = 0; i < lines.Length; i++)
							{
								string line = lines[i];

								int posInStr = -1;
								MatchCollection regCol = null;

								if (searchSpec.UseRegularExpressions)
								{
									regCol = reg.Matches(line);

									if (regCol.Count > 0)
									{
										posInStr = 1;
									}
								}
								else
								{
									// If we are looking for whole worlds only, perform the check.
									if (searchSpec.UseWholeWordMatching)
									{
										// if match is found, also check against our internal line hit count method to be sure they are in sync
										Match mtc = reg.Match(line);
										if (mtc != null && mtc.Success && libAstroGrep.Grep.RetrieveLineMatches(line, searchSpec).Count > 0)
										{
											posInStr = mtc.Index;
										}
									}
									else
									{
										posInStr = line.IndexOf(searchSpec.SearchText, searchSpec.UseCaseSensitivity ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
									}
								}

								if (posInStr > -1)
								{
									if (match == null)
									{
										match = new MatchResult(file);

										// found hit in file so just return
										if (searchSpec.ReturnOnlyFileNames)
										{
											break;
										}
									}

									var matchLineFound = new MatchResultLine() { Line = line, LineNumber = -1, HasMatch = true, LongLineCharCount = searchSpec.LongLineCharCount, BeforeAfterCharCount = searchSpec.BeforeAfterCharCount };

									if (searchSpec.UseRegularExpressions)
									{
										posInStr = regCol[0].Index;
										match.SetHitCount(regCol.Count);

										foreach (Match regExMatch in regCol)
										{
											matchLineFound.Matches.Add(new MatchResultLineMatch(regExMatch.Index, regExMatch.Length));
										}
									}
									else
									{
										var lineMatches = libAstroGrep.Grep.RetrieveLineMatches(line, searchSpec);
										match.SetHitCount(lineMatches.Count);
										matchLineFound.Matches = lineMatches;
									}
									matchLineFound.ColumnNumber = 1;
									match.Matches.Add(matchLineFound);
								}
							}
						}
					}
				}
				catch (Exception funcEx)
				{
					ex = funcEx;
				}
			}

			return match;
		}

		/// <summary>
		/// Determines if given file is supported by current plugin.
		/// </summary>
		/// <param name="file">Current FileInfo object</param>
		/// <returns>True if supported, False if not supported</returns>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// [Curtis_Beard]      07/12/2019  FIX: 116, use full path instead of name
		/// </history>
		public bool IsFileSupported(FileInfo file)
		{
			return FilterLoader.LoadIFilter(file.Extension) != null;
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		public bool Load()
		{
			return Load(false);
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <param name="visible">true makes underlying application visible, false is make it hidden</param>
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		public bool Load(bool visible)
		{
			return true;
		}

		/// <summary>
		/// Unloads Microsoft Word.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      10/17/2012  Created
		/// </history>
		public void Unload()
		{
		}
	}
}