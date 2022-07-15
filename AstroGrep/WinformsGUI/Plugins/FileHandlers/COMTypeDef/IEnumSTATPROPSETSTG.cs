using System.Runtime.InteropServices;
using System.Security;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("0000013B-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATPROPSETSTG
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ppenum"></param>
		void Clone(out IEnumSTATPROPSETSTG ppenum);

		/// <summary>
		///
		/// </summary>
		/// <param name="celt"></param>
		/// <param name="rgelt"></param>
		/// <param name="pceltFetched"></param>
		/// <returns></returns>
		int Next(uint celt, STATPROPSETSTG rgelt, out uint pceltFetched);

		/// <summary>
		///
		/// </summary>
		void Reset();

		/// <summary>
		///
		/// </summary>
		/// <param name="celt"></param>
		void Skip(uint celt);
	}
}