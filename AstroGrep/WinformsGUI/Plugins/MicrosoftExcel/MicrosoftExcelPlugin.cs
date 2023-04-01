using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using ExcelDataReader;
using ExcelNumberFormat;
using libAstroGrep;
using libAstroGrep.Plugin;

namespace AstroGrep.Plugins.MicrosoftExcel
{
	/// <summary>
	/// Used to search a Microsoft Excel based files.
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
	/// <history>
	/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
	/// </history>
	public class MicrosoftExcelPlugin : IDisposable, IAstroGrepPlugin
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MicrosoftExcelPlugin"/> class.
		/// </summary>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		public MicrosoftExcelPlugin()
		{
			IsAvailable = true;
		}

		/// <summary>
		/// Handles destruction of the object.
		/// </summary>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		~MicrosoftExcelPlugin()
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
			get { return "Searches Microsoft Excel based documents.  Currently doesn't support Context lines or Line Numbers."; }
		}

		/// <summary>
		/// Gets the valid extensions for this grep type.
		/// </summary>
		/// <remarks>
		/// Comma separated list of strings.
		/// </remarks>
		public string Extensions
		{
			get { return ".xls,.xlsx,.xlsm"; }
		}

		/// <summary>
		/// Checks to see if the plugin is available on this system.
		/// </summary>
		public bool IsAvailable { get; private set; }

		/// <summary>
		/// Determines whether the plug-in skipped the file and it should be read by another plug-in or the default search.
		/// </summary>
		public bool IsFileSkipped => false;

		/// <summary>
		/// Gets the name of the plugin.
		/// </summary>
		public string Name
		{
			get { return "Microsoft Excel"; }
		}

		/// <summary>
		/// Gets the version of the plugin.
		/// </summary>
		public string Version
		{
			get { return "1.0.0"; }
		}

		/// <summary>
		/// Handles disposing of the object.
		/// </summary>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		public void Dispose()
		{
			IsAvailable = false;
		}

		/// <summary>
		/// Searches the given file for the given search text.
		/// </summary>
		/// <param name="file">
		/// FileInfo object
		/// </param>
		/// <param name="searchSpec">
		/// ISearchSpec interface value
		/// </param>
		/// <param name="ex">
		/// Exception holder if error occurs
		/// </param>
		/// <returns>
		/// Hitobject containing grep results, null if on error
		/// </returns>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
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

					// pull text from pdf file and parse it
					List<Tuple<string, string>> lines = ExtractText(file);

					if (lines.Count > 0)
					{
						for (int i = 0; i < lines.Count; i++)
						{
							string line = lines[i].Item2.TrimEnd('\r');
							string marginText = string.Format("Sheet {0}: ", lines[i].Item1);

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
									// if match is found, also check against our internal line hit
									// count method to be sure they are in sync
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

								var matchLineFound = new MatchResultLine() { Line = marginText + line, LineNumber = -1, HasMatch = true, LongLineCharCount = searchSpec.LongLineCharCount, BeforeAfterCharCount = searchSpec.BeforeAfterCharCount };

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
									var lineMatches = libAstroGrep.Grep.RetrieveLineMatches(line, searchSpec);
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
		/// <param name="file">
		/// Current FileInfo object
		/// </param>
		/// <returns>
		/// True if supported, False if not supported
		/// </returns>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
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
		/// <returns>
		/// returns true if (successfully loaded or false otherwise
		/// </returns>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		public bool Load()
		{
			return Load(false);
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <param name="visible">
		/// true makes underlying application visible, false is make it hidden
		/// </param>
		/// <returns>
		/// returns true if (successfully loaded or false otherwise
		/// </returns>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		public bool Load(bool visible)
		{
			return true;
		}

		/// <summary>
		/// Unloads the plugin.
		/// </summary>
		/// <history>
		/// [Curtis_Beard] 09/09/2019 ADD: OpenXML plugin
		/// </history>
		public void Unload()
		{
		}

		private List<Tuple<string, string>> ExtractExcelText(Stream stream, FileInfo file)
		{
			try
			{
				var sheets = ExtractExcelTextFromSheets(stream);
				List<Tuple<string, string>> lines = new List<Tuple<string, string>>();
				foreach (var kvPair in sheets)
				{
					//lines.Add(kvPair.Value);
					string[] sheetLines = kvPair.Value.Split('\n');
					foreach (string line in sheetLines)
					{
						lines.Add(new Tuple<string, string>(kvPair.Key, line.TrimEnd('\r')));
					}
				}

				return lines;
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Failed to extract text from inside Excel file '{0}', error: {1}", file.FullName, ex));
			}
		}

		private List<KeyValuePair<string, string>> ExtractExcelTextFromSheets(Stream stream)
		{
			List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();

			// Auto-detect format, supports:
			// - Binary Excel files (2.0-2003 format; *.xls)
			// - OpenXml Excel files (2007 format; *.xlsx)
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				do
				{
					StringBuilder sb = new StringBuilder();
					while (reader.Read())
					{
						for (int col = 0; col < reader.FieldCount; col++)
						{
							sb.Append(GetFormattedValue(reader, col, System.Threading.Thread.CurrentThread.CurrentCulture)).Append('\t');
						}

						sb.Append(Environment.NewLine);
					}

					results.Add(new KeyValuePair<string, string>(reader.Name, sb.ToString()));
				} while (reader.NextResult());
			}

			return results;
		}

		/// <summary>
		/// </summary>
		/// <param name="file">
		/// </param>
		/// <returns>
		/// </returns>
		private List<Tuple<string, string>> ExtractText(FileInfo file)
		{
			using (var input = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				if (file.Extension.StartsWith(".xls", StringComparison.OrdinalIgnoreCase))
				{
					return ExtractExcelText(input, file);
				}
			}

			return new List<Tuple<string, string>>();
		}

		private string GetFormattedValue(IExcelDataReader reader, int columnIndex, System.Globalization.CultureInfo culture)
		{
			var value = reader.GetValue(columnIndex);
			var formatString = reader.GetNumberFormatString(columnIndex);
			if (formatString != null)
			{
				var format = new NumberFormat(formatString);
				return format.Format(value, culture);
			}
			return Convert.ToString(value, culture);
		}
	}
}