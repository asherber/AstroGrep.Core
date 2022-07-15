using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using libAstroGrep;
using libAstroGrep.Plugin;
using TagLib;

namespace AstroGrep.Plugins.MediaTag
{
	/// <summary>
	/// Used to search media files for a specified string using TagLib to read common properties.
	/// </summary>
	/// <remarks>
	/// AstroGrep File Searching Utility. Written by Theodore L. Ward Copyright (C) 2002 AstroComma Incorporated.
	///
	/// This program is free software; you can redistribute it and/or modify it under the terms of
	/// the GNU General public License as published by the Free Software Foundation; either version 2
	/// of the License, or (at your option) any later version.
	///
	/// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
	/// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
	/// the GNU General public License for more details.
	///
	/// You should have received a copy of the GNU General public License along with this program; if
	/// (not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
	/// 02111-1307, USA.
	///
	/// The author may be contacted at: ted@astrocomma.com or curtismbeard@gmail.com
	/// </remarks>
	/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tags plugin</history>
	public class MediaTagsPlugin : IDisposable, IAstroGrepPlugin
	{
		private string supportedExtensions = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaTagsPlugin"/> class.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public MediaTagsPlugin()
		{
			IsAvailable = true;
		}

		/// <summary>
		/// Handles destruction of the object.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		~MediaTagsPlugin()
		{
			Dispose();
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
			get { return "Searches media tags: Title, Album, Comment, Lyrics, Conductor, Year, Composers, AlbumArtists, Genres, Performers.  Currently doesn't support Context lines or Line Numbers."; }
		}

		/// <summary>
		/// Gets the valid extensions for this grep type.
		/// </summary>
		/// <remarks>Comma separated list of strings.</remarks>
		public string Extensions
		{
			get
			{
				if (supportedExtensions == null)
				{
					List<string> exts = new List<string>();

					// find all supported mime type extensions
					foreach (var typ in TagLib.FileTypes.AvailableTypes)
					{
						var attrs = typ.Value.GetCustomAttributes(typeof(TagLib.SupportedMimeType), false);
						foreach (var attr in attrs)
						{
							if (attr is SupportedMimeType mime && !string.IsNullOrEmpty(mime.Extension))
							{
								string ext = string.Format(".{0}", mime.Extension);
								if (!exts.Contains(ext))
								{
									exts.Add(ext);
								}
							}
						}
					}

					supportedExtensions = string.Join(",", exts);
				}

				return supportedExtensions;
			}
		}

		/// <summary>
		/// Checks to see if the plugin is available on this system.
		/// </summary>
		public bool IsAvailable { get; private set; }

		/// <summary>
		/// Gets the name of the plugin.
		/// </summary>
		public string Name
		{
			get { return "Media Tags"; }
		}

		/// <summary>
		/// Gets the version of the plugin.
		/// </summary>
		public string Version
		{
			get { return "1.0.0"; }
		}

		/// <summary>
		/// Gets the valid extentions as a proper file filter (*.ext,*.ext2,etc.)
		/// </summary>
		/// <returns>string containing valid file filter</returns>
		public static string GetExtensionsFilter()
		{
			List<string> exts = new List<string>();

			// find all supported mime type extensions
			foreach (var typ in TagLib.FileTypes.AvailableTypes)
			{
				var attrs = typ.Value.GetCustomAttributes(typeof(TagLib.SupportedMimeType), false);
				foreach (var attr in attrs)
				{
					if (attr is TagLib.SupportedMimeType mime && !string.IsNullOrEmpty(mime.Extension))
					{
						string ext = string.Format("*.{0}", mime.Extension);
						if (!exts.Contains(ext))
						{
							exts.Add(ext);
						}
					}
				}
			}

			return string.Join(",", exts);
		}

		/// <summary>
		/// Handles disposing of the object.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public void Dispose()
		{
			IsAvailable = false;
		}

		/// <summary>
		/// Searches the given file for the given search text.
		/// </summary>
		/// <param name="file">FileInfo object</param>
		/// <param name="searchSpec">ISearchSpec interface value</param>
		/// <param name="ex">Exception holder if error occurs</param>
		/// <returns>Hitobject containing grep results, null if on error</returns>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public MatchResult Grep(FileInfo file, ISearchSpec searchSpec, ref Exception ex)
		{
			// initialize Exception object to null
			ex = null;
			MatchResult match = null;

			if (IsFileSupported(file))
			{
				try
				{
					TagLib.File tagFile = TagLib.File.Create(file.FullName);

					if (tagFile != null)
					{
						Regex reg = libAstroGrep.Grep.BuildSearchRegEx(searchSpec);

						// search different tags
						GrepTag("Title", tagFile.Tag.Title, file, searchSpec, reg, ref match);
						GrepTag("Album", tagFile.Tag.Album, file, searchSpec, reg, ref match);
						GrepTag("Comment", tagFile.Tag.Comment, file, searchSpec, reg, ref match);
						GrepTag("Lyrics", tagFile.Tag.Lyrics, file, searchSpec, reg, ref match);
						GrepTag("Conductor", tagFile.Tag.Conductor, file, searchSpec, reg, ref match);
						GrepTag("Year", tagFile.Tag.Year.ToString(), file, searchSpec, reg, ref match);
						GrepTag("Composers", tagFile.Tag.JoinedComposers, file, searchSpec, reg, ref match);
						GrepTag("AlbumArtists", tagFile.Tag.JoinedAlbumArtists, file, searchSpec, reg, ref match);
						GrepTag("Genres", tagFile.Tag.JoinedGenres, file, searchSpec, reg, ref match);
						GrepTag("Performers", tagFile.Tag.JoinedPerformers, file, searchSpec, reg, ref match);
					}
					else
					{
						throw new Exception("Unable to create tagging reference from the file");
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
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public bool IsFileSupported(FileInfo file)
		{
			string ext = file.Extension;

			string[] supportedExtensions = Extensions.Split(new char[1] { ',' });
			foreach (string supportedExtension in supportedExtensions)
			{
				if (ext.Equals(supportedExtension, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public bool Load()
		{
			return Load(false);
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <param name="visible">true makes underlying application visible, false is make it hidden</param>
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public bool Load(bool visible)
		{
			return true;
		}

		/// <summary>
		/// Unloads the plugin.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: Media Tag plugin</history>
		public void Unload()
		{
		}

		/// <summary>
		/// Searches the given tag value for the search text.
		/// </summary>
		/// <param name="tagTitle">Title of tag</param>
		/// <param name="tagValue">Tag value</param>
		/// <param name="file">FileInfo</param>
		/// <param name="searchSpec">ISearchSpec interface value</param>
		/// <param name="reg">The current regular expression if needed</param>
		/// <param name="match">MatchResult used to attach hits when found</param>
		/// <history>[Curtis_Beard] 08/22/2007 Created</history>
		private void GrepTag(string tagTitle, string tagValue, FileInfo file, ISearchSpec searchSpec, Regex reg, ref MatchResult match)
		{
			// safety check
			if (string.IsNullOrEmpty(tagValue) || string.IsNullOrEmpty(searchSpec.SearchText)) return;

			// file name only check, to not check if a hit already found in this file
			if (searchSpec.ReturnOnlyFileNames && match != null) return;

			int posInStr = -1;
			MatchCollection regCol = null;
			if (searchSpec.UseRegularExpressions)
			{
				regCol = reg.Matches(tagValue);

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
					// if match is found, also check against our internal line hit count method to be
					// sure they are in sync
					Match mtc = reg.Match(tagValue);
					if (mtc != null && mtc.Success && libAstroGrep.Grep.RetrieveLineMatches(tagValue, searchSpec).Count > 0)
					{
						posInStr = mtc.Index;
					}
				}
				else
				{
					posInStr = tagValue.IndexOf(searchSpec.SearchText, searchSpec.UseCaseSensitivity ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
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
						return;
					}
				}

				// set tag title as text before found, update initial found indexes with new lengths
				string marginText = string.Format("{0}: ", tagTitle);
				var matchLineFound = new MatchResultLine() { Line = marginText + tagValue, LineNumber = -1, HasMatch = true, LongLineCharCount = searchSpec.LongLineCharCount, BeforeAfterCharCount = searchSpec.BeforeAfterCharCount };

				if (searchSpec.UseRegularExpressions)
				{
					match.SetHitCount(regCol.Count);

					foreach (Match regExMatch in regCol)
					{
						matchLineFound.Matches.Add(new MatchResultLineMatch(regExMatch.Index + marginText.Length, regExMatch.Length));
					}
				}
				else
				{
					var lineMatches = libAstroGrep.Grep.RetrieveLineMatches(tagValue, searchSpec);
					match.SetHitCount(lineMatches.Count);
					matchLineFound.Matches = lineMatches;
					foreach (var lineMatch in matchLineFound.Matches)
					{
						lineMatch.StartPosition += marginText.Length;
						lineMatch.OriginalStartPosition += marginText.Length;
					}
				}
				matchLineFound.ColumnNumber = 1;
				match.Matches.Add(matchLineFound);
			}
		}
	}
}