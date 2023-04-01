using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using libAstroGrep;
using libAstroGrep.Plugin;
using System.Diagnostics;

namespace AstroGrep.Plugins.PDF
{
	/// <summary>
	/// Used to search a PDF file for a specified string.
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
	/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
	/// </history>
	public class PDFPlugin : IDisposable, IAstroGrepPlugin
	{
		private string pdfToTxtAppPath = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="PDFPlugin"/> class.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		public PDFPlugin()
		{
			// extract pdf to text application
			if (string.IsNullOrEmpty(pdfToTxtAppPath))
			{
				ExtractPDFToTxtApp();
			}

			IsAvailable = !string.IsNullOrEmpty(pdfToTxtAppPath);
		}

		/// <summary>
		/// Handles destruction of the object.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		~PDFPlugin()
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
			get { return "Searches PDF documents using the pdf to text utility from Xpdf.  Currently doesn't support Context lines or Line Numbers."; }
		}

		/// <summary>
		/// Gets the valid extensions for this grep type.
		/// </summary>
		/// <remarks>Comma separated list of strings.</remarks>
		public string Extensions
		{
			get { return ".pdf"; }
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
			get { return "PDF"; }
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
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		public void Dispose()
		{
			IsAvailable = false;

			// cleanup temp files and directory
			string path = GetPDFFolder();
			try
			{
				Directory.Delete(path, true);
			}
			catch { }
		}

		/// <summary>
		/// Searches the given file for the given search text.
		/// </summary>
		/// <param name="file">FileInfo object</param>
		/// <param name="searchSpec">ISearchSpec interface value</param>
		/// <param name="ex">Exception holder if error occurs</param>
		/// <returns>Hitobject containing grep results, null if on error</returns>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
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
					string[] lines = ExtractText(file);

					if (lines.Length > 0)
					{
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
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
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
		/// <returns>returns true if (successfully loaded or false otherwise</returns>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
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
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		public bool Load(bool visible)
		{
			// extract pdf to text application
			if (string.IsNullOrEmpty(pdfToTxtAppPath))
			{
				ExtractPDFToTxtApp();
			}

			return !string.IsNullOrEmpty(pdfToTxtAppPath);
		}

		/// <summary>
		/// Unloads the plugin.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		public void Unload()
		{
		}

		/// <summary>
		/// Extracts the pdf to text application to the pdf temp folder.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		private void ExtractPDFToTxtApp()
		{
			pdfToTxtAppPath = Path.Combine(GetPDFFolder(), "pdftotext.exe");
			File.WriteAllBytes(pdfToTxtAppPath, AstroGrep.Properties.Resources.pdftotext);
		}

		/// <summary>
		/// Extracts the text from the given pdf file.
		/// </summary>
		/// <param name="file">The <see cref="FileInfo"/> of the pdf file to extract the text from</param>
		/// <returns>The lines of text from the pdf if able to extract</returns>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		private string[] ExtractText(FileInfo file)
		{
			string tempFolder = GetPDFFolder();
			string tempFileName = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(file.Name) + ".txt");

			using (Process process = new Process())
			{
				// use command prompt
				process.StartInfo.FileName = pdfToTxtAppPath;
				process.StartInfo.Arguments = string.Format("-layout \"{0}\" \"{1}\"", file.FullName, tempFileName);
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.WorkingDirectory = tempFolder;
				process.StartInfo.CreateNoWindow = true;
				// start cmd prompt, execute command
				process.Start();
				process.WaitForExit();

				if (process.ExitCode == 0)
				{
					if (File.Exists(tempFileName))
					{
						return File.ReadAllLines(tempFileName);
					}
					else
						throw new Exception(string.Format("pdftotext did not generate an output file when converting '{0}'", file.FullName));
				}
				else
				{
					string errorMessage = string.Empty;
					switch (process.ExitCode)
					{
						case 1:
							errorMessage = "Error opening PDF file";
							break;

						case 2:
							errorMessage = "Error opening an output file";
							break;

						case 3:
							errorMessage = "Error related to PDF permissions";
							break;

						default:
							errorMessage = "Unknown error";
							break;
					}

					throw new Exception(string.Format("pdftotext returned '{0}' converting '{1}'", errorMessage, file.FullName));
				}
			}
		}

		/// <summary>
		/// Retrieves the pdf temp folder path (and creates it if not found).
		/// </summary>
		/// <returns>The full directory path to the pdf temp folder</returns>
		/// <history>
		/// [Curtis_Beard]      09/09/2019	ADD: PDF plugin
		/// </history>
		private string GetPDFFolder()
		{
			string tempFolder = Path.Combine(Path.GetTempPath(), "AstroGrep-PDF");
			if (!Directory.Exists(tempFolder))
				Directory.CreateDirectory(tempFolder);

			return tempFolder;
		}
	}
}