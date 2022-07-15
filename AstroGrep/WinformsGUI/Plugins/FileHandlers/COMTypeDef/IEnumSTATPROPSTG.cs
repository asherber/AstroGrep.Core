using System.Runtime.InteropServices;
using System.Security;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("00000139-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATPROPSTG
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ppenum"></param>
		void Clone(out IEnumSTATPROPSTG ppenum);

		/// <summary>
		///
		/// </summary>
		/// <param name="celt"></param>
		/// <param name="rgelt"></param>
		/// <param name="pceltFetched"></param>
		/// <returns></returns>
		int Next(uint celt, STATPROPSTG rgelt, out uint pceltFetched);

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