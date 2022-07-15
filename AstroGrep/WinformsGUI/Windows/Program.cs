using System;
using System.Diagnostics;
using System.Windows.Forms;

using AstroGrep.Common;
using AstroGrep.Common.Logging;

namespace AstroGrep.Windows
{
	/// <summary>
	/// Startup for application
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
	/// [Curtis_Beard]	  01/27/2005	.Net Conversion/Support for xp themes
	/// [Curtis_Beard]	  10/14/2005	CHG: Made a class instead of module
	/// [Curtis_Beard]	  05/18/2020	CHG: use standard unhandled exception handling instead of try/catch
	/// </history>
	public static class Program
	{
		/// <summary>
		/// Handles any uncaught thread based exceptions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	  05/18/2020	CHG: use standard unhandled exception handling instead of try/catch
		/// </history>
		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			ProcessUnhandledException(e.Exception, false);
		}

		/// <summary>
		/// Starts the application
		/// </summary>
		/// <remarks>
		/// Enables visual styles for the controls if available.
		/// </remarks>
		/// <history>
		/// [Curtis_Beard]	  10/14/2005	Created
		/// [Curtis_Beard]	  07/21/2006	CHG: Use a try/catch block for any erroneous errors
		/// [Curtis_Beard]	  10/11/2006	CHG: Remove setting reference to frmMain in Common class
		/// [Curtis_Beard]    09/26/2012	CHG: 3572487, detect command line arg for displaying help and show dialog with options
		/// [Curtis_Beard]	  04/08/2015	CHG: add logging
		/// [Curtis_Beard]	  06/02/2015	CHG: add portable to starting log message if enabled
		/// [Curtis_Beard]	  09/11/2015	CHG: add operating system to log file
		/// [Curtis_Beard]	  05/18/2020	CHG: use standard unhandled exception handling instead of try/catch
		/// </history>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Unhandled exception handlers
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				Application.ThreadException += Application_ThreadException;
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			}

			// Parse command line, must be done before any use of user settings and form creation in case of help
			CommandLineProcessing.CommandLineArguments args = CommandLineProcessing.Process(Environment.GetCommandLineArgs());

			// needs to go after command line processing since StoreDataLocal determines log file location
			LogClient.Instance.Logger.Info("### STARTING {0}, version {1}{2} ###",
			   ProductInformation.ApplicationName,
			   ProductInformation.ApplicationVersion.ToString(3),
			   ProductInformation.IsPortable ? " (Portable)" : string.Empty);

			LogClient.Instance.Logger.Info("Operating System: {0}", Environment.OSVersion.VersionString);

			Legacy.ConvertLanguageValue();
			Language.Load(AstroGrep.Core.GeneralSettings.Language);

			if (args.AnyArguments && args.DisplayHelp)
			{
				LogClient.Instance.Logger.Info("Displaying command line help window.");

				// display command line options
				Application.Run(new Windows.Forms.frmCommandLine());
			}
			else
			{
				LogClient.Instance.Logger.Info("Displaying main search window.");

				// display main form
				Windows.Forms.frmMain mainForm = new AstroGrep.Windows.Forms.frmMain
				{
					CommandLineArgs = args
				};

				Application.AddMessageFilter(new ListViewMouseWheelMoveMessageFilter());
				Application.Run(mainForm);
			}
		}

		/// <summary>
		/// Processes an unhandled exception.
		/// </summary>
		/// <param name="ex"><see cref="Exception"/> to process</param>
		/// <param name="isTerminating">True if terminating application, False otherwise</param>
		/// <history>
		/// [Curtis_Beard]	  05/18/2020	CHG: use standard unhandled exception handling instead of try/catch
		/// </history>
		private static void ProcessUnhandledException(Exception ex, bool isTerminating)
		{
			if (isTerminating)
			{
				LogClient.Instance.Logger.Fatal(ex, "UNHANDLED EXCEPTION - Program Terminating, full details: {0}", LogClient.GetAllExceptions(ex));
			}
			else
			{
				LogClient.Instance.Logger.Error("UNHANDLED EXCEPTION, full details: {0}", LogClient.GetAllExceptions(ex));
			}

			// display the unhandled exception dialog
			using (Forms.UnhandledExceptionDialog d = new Forms.UnhandledExceptionDialog(ex, isTerminating))
			{
				if (d.ShowDialog() == DialogResult.Abort || isTerminating)
				{
					Application.Exit();
				}
			}
		}

		/// <summary>
		/// Handles any uncaught application domain based exceptions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <history>
		/// [Curtis_Beard]	  05/18/2020	CHG: use standard unhandled exception handling instead of try/catch
		/// </history>
		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ProcessUnhandledException((Exception)e.ExceptionObject, e.IsTerminating);
		}
	}
}