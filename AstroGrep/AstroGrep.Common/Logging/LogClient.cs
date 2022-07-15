using System;
using System.IO;
using System.Text;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace AstroGrep.Common.Logging
{
	/// <summary>
	/// AstroGrep Log Client implementation.
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
	/// [Curtis_Beard]	   04/08/2015	Add: logging
	/// </history>
	public class LogClient : ILogClient
	{
		private static LogClient instance;

		/// <summary>
		/// Setup the LogClient object with the target and rules.
		/// </summary>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	CHG: add logging
		/// </history>
		private LogClient()
		{
			LoggingConfiguration loggingConfiguration = new LoggingConfiguration();

			FileTarget fileTarget = new FileTarget();
			loggingConfiguration.AddTarget("file", fileTarget);

			LogFile = ApplicationPaths.LogFile;
			fileTarget.FileName = LogFile;
			fileTarget.ArchiveFileName = ApplicationPaths.LogArchiveFile;
			//fileTarget.Layout = "${longdate}|${level}|${callsite}|${trim-whitespace:inner=${message}${onexception:inner=|EXCEPTION ${exception-sql:format=short:tail= }${exception:format=toString}}}";
			fileTarget.Layout = "${longdate}|${level}|${callsite}|${message}";
			fileTarget.ArchiveAboveSize = 5242880L;
			fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
			fileTarget.MaxArchiveFiles = 3;

			LoggingRule item = new LoggingRule("*", LogLevel.Debug, fileTarget);
			loggingConfiguration.LoggingRules.Add(item);
			LogManager.Configuration = loggingConfiguration;

			Logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Retrieves the current instance of the LogClient.
		/// </summary>
		public static LogClient Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new LogClient();
				}
				return instance;
			}
		}

		/// <summary>
		/// The full path to the log file.
		/// </summary>
		public string LogFile { get; set; }

		/// <summary>
		/// The NLog.Logger object.
		/// </summary>
		public Logger Logger { get; set; }

		/// <summary>
		/// Gets all the exceptions for this System.Exception.
		/// </summary>
		/// <param name="ex">Current exception</param>
		/// <returns>string containing all exception information</returns>
		/// <history>
		/// [Curtis_Beard]	   04/08/2015	CHG: add logging
		/// </history>
		public static string GetAllExceptions(Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Exception:");
			stringBuilder.AppendLine(ex.ToString());
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine("Stack trace:");
			stringBuilder.AppendLine(ex.StackTrace);
			int num = 0;
			checked
			{
				while (ex.InnerException != null)
				{
					num++;
					stringBuilder.AppendLine("Inner Exception " + num.ToString() + ":");
					stringBuilder.AppendLine(ex.InnerException.ToString());
					ex = ex.InnerException;
				}
				return stringBuilder.ToString();
			}
		}
	}
}