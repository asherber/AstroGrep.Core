using libAstroGrep;
using System.IO;

namespace AstroGrep.Core
{
	/// <summary>
	/// Defines a full search session.
	/// </summary>
	public class SearchSession
	{
		// settings
		// exclusions?
		// plug-ins?

		// path
		// file type
		// search text

		// results

		/// <summary>
		/// Constructor
		/// </summary>
		public SearchSession()
		{

		}


		/// <summary>
		/// Construct a new session based on the current <see cref="Grep"/>.
		/// </summary>
		/// <param name="grep"></param>
		public SearchSession(Grep grep)
		{
			
		}

		/// <summary>
		/// Save the  current session to the file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool Save(string filePath)
		{
			if (File.Exists(filePath))
			{
				try
				{
					File.Delete(filePath);
				}
				catch { }
			}

			// create file

			return false;
		}

		/// <summary>
		/// Load a session from a file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static SearchSession Load(string filePath)
		{
			if (File.Exists(filePath))
			{
				// load file into object
			}

			return new SearchSession();
		}
	}
}
