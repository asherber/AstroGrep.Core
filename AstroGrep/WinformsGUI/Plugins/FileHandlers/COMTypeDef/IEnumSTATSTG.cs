using System.Runtime.InteropServices;
using System.Security;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("0000000D-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumSTATSTG
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="celt"></param>
		/// <param name="rgVar"></param>
		/// <param name="pceltFetched"></param>
		/// <returns></returns>
		[PreserveSig]
		int Next([In, MarshalAs(UnmanagedType.U4)] int celt, [Out, MarshalAs(UnmanagedType.LPArray)] STATSTG[] rgVar, [MarshalAs(UnmanagedType.U4)] out int pceltFetched);

		/// <summary>
		///
		/// </summary>
		/// <param name="celt"></param>
		/// <returns></returns>
		[PreserveSig]
		int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int Reset();

		/// <summary>
		///
		/// </summary>
		/// <param name="newEnum"></param>
		/// <returns></returns>
		int Clone([MarshalAs(UnmanagedType.Interface)] out IEnumSTATSTG newEnum);
	}
}