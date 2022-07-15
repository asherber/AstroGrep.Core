using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using libAstroGrep;
using libAstroGrep.Plugin;

namespace AstroGrep.Plugins.MicrosoftWord
{
	/// <summary>
	/// Used to search a Microsoft Word OpenXML based files.
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
	/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
	public class MicrosoftWordOpenXMLPlugin : IDisposable, IAstroGrepPlugin
	{
		private bool isInTableRow;

		/// <summary>
		/// Initializes a new instance of the <see cref="MicrosoftWordOpenXMLPlugin"/> class.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
		public MicrosoftWordOpenXMLPlugin()
		{
			IsAvailable = true;
		}

		/// <summary>
		/// Handles destruction of the object.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
		~MicrosoftWordOpenXMLPlugin()
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
			get { return "Searches Microsoft Word based documents using the OpenXML format.  Currently doesn't support Context lines or Line Numbers."; }
		}

		/// <summary>
		/// Gets the valid extensions for this grep type.
		/// </summary>
		/// <remarks>Comma separated list of strings.</remarks>
		public string Extensions
		{
			get { return ".docx,.docm"; }
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
			get { return "Microsoft Word Open XML"; }
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
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
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
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
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
					string[] lines = ExtractText(file);

					if (lines.Length > 0)
					{
						for (int i = 0; i < lines.Length; i++)
						{
							string line = lines[i];
							line = line.TrimEnd('\r');

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
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
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
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
		public bool Load()
		{
			return Load(false);
		}

		/// <summary>
		/// Loads the plugin and prepares it for a grep.
		/// </summary>
		/// <param name="visible">true makes underlying application visible, false is make it hidden</param>
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
		public bool Load(bool visible)
		{
			return true;
		}

		/// <summary>
		/// Unloads the plugin.
		/// </summary>
		/// <history>[Curtis_Beard] 09/09/2019 ADD: OpenXML plugin</history>
		public void Unload()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		private string[] ExtractText(FileInfo file)
		{
			using (var input = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				if (file.Extension.StartsWith(".doc", StringComparison.OrdinalIgnoreCase))
				{
					return ExtractWordText(input, file);
				}
			}

			return new string[0] { };
		}

		private void ExtractText(OpenXmlElement elem, IEnumerable<Style> docStyles, WordListManager wlm, StringBuilder sb)
		{
			if (elem is Paragraph)
			{
				var para = elem as Paragraph;

				string indent = GetIndent(para, docStyles);
				string fmtNum = wlm.GetFormattedNumber(para);

				if (isInTableRow)
					sb.Append(indent).Append(fmtNum).Append(elem.InnerText).Append('\t');
				else
					sb.Append(indent).Append(fmtNum).AppendLine(elem.InnerText);
			}
			else if (elem is TableRow)
			{
				isInTableRow = true;

				sb.Append('\t');

				foreach (var child in elem)
				{
					ExtractText(child, docStyles, wlm, sb);
				}

				sb.AppendLine();

				isInTableRow = false;
			}
			else
			{
				foreach (var child in elem)
				{
					ExtractText(child, docStyles, wlm, sb);
				}
			}
		}

		private string[] ExtractWordText(Stream stream, FileInfo file)
		{
			try
			{
				StringBuilder sb = new StringBuilder();

				// Open a given Word document as readonly
				using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
				{
					var body = doc.MainDocumentPart.Document.Body;
					var docStyles = doc.MainDocumentPart.StyleDefinitionsPart.Styles
						.Where(r => r is Style).Select(r => r as Style);

					WordListManager wlm = WordListManager.Empty;
					if (doc.MainDocumentPart.NumberingDefinitionsPart != null && doc.MainDocumentPart.NumberingDefinitionsPart.Numbering != null)
					{
						wlm = new WordListManager(doc.MainDocumentPart.NumberingDefinitionsPart.Numbering);
					}

					ExtractText(body, docStyles, wlm, sb);
				}

				return sb.ToString().Split('\n');
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Failed to extract text from inside Word file '{0}', error: {1}", file.FullName, ex));
			}
		}

		private string GetIndent(Paragraph para, IEnumerable<Style> docStyles)
		{
			string indent = string.Empty;
			if (para != null && para.ParagraphProperties != null && para.ParagraphProperties.Indentation != null)
			{
				var indentation = para.ParagraphProperties.Indentation;
				if (indentation.Left != null && indentation.Left.HasValue)
				{
					indent = WordListManager.TwipsToSpaces(indentation.Left);
				}
				else if (indentation.Start != null && indentation.Start.HasValue)
				{
					indent = WordListManager.TwipsToSpaces(indentation.Start);
				}
			}
			if (para != null && para.ParagraphProperties != null && para.ParagraphProperties.ParagraphStyleId != null &&
				para.ParagraphProperties.ParagraphStyleId.Val != null &&
				para.ParagraphProperties.ParagraphStyleId.Val.HasValue)
			{
				var style = docStyles.Where(r => r.StyleId == para.ParagraphProperties.ParagraphStyleId.Val.Value)
					.Select(r => r).FirstOrDefault();

				if (style != null)
				{
					var pp = style.Where(r => r is StyleParagraphProperties)
						.Select(r => r as StyleParagraphProperties).FirstOrDefault();

					if (pp != null && pp.Indentation != null)
					{
						if (pp.Indentation.Left != null && pp.Indentation.Left.HasValue)
						{
							indent = WordListManager.TwipsToSpaces(pp.Indentation.Left);
						}
						else if (pp.Indentation.Start != null && pp.Indentation.Start.HasValue)
						{
							indent = WordListManager.TwipsToSpaces(pp.Indentation.Start);
						}
					}
				}
			}

			return indent;
		}
	}
}