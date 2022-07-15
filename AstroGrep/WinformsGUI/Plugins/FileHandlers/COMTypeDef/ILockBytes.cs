using System;
using System.Runtime.InteropServices;
using System.Security;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("0000000A-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ILockBytes
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ulOffset"></param>
		/// <param name="pv"></param>
		/// <param name="cb"></param>
		/// <param name="pcbRead"></param>
		void ReadAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, [Out] IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbRead);

		/// <summary>
		///
		/// </summary>
		/// <param name="ulOffset"></param>
		/// <param name="pv"></param>
		/// <param name="cb"></param>
		/// <param name="pcbWritten"></param>
		void WriteAt([In, MarshalAs(UnmanagedType.U8)] long ulOffset, IntPtr pv, [In, MarshalAs(UnmanagedType.U4)] int cb, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcbWritten);

		/// <summary>
		///
		/// </summary>
		void Flush();

		/// <summary>
		///
		/// </summary>
		/// <param name="cb"></param>
		void SetSize([In, MarshalAs(UnmanagedType.U8)] long cb);

		/// <summary>
		///
		/// </summary>
		/// <param name="libOffset"></param>
		/// <param name="cb"></param>
		/// <param name="dwLockType"></param>
		void LockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);

		/// <summary>
		///
		/// </summary>
		/// <param name="libOffset"></param>
		/// <param name="cb"></param>
		/// <param name="dwLockType"></param>
		void UnlockRegion([In, MarshalAs(UnmanagedType.U8)] long libOffset, [In, MarshalAs(UnmanagedType.U8)] long cb, [In, MarshalAs(UnmanagedType.U4)] int dwLockType);

		/// <summary>
		///
		/// </summary>
		/// <param name="pstatstg"></param>
		/// <param name="grfStatFlag"></param>
		void Stat([Out] STATSTG pstatstg, [In, MarshalAs(UnmanagedType.U4)] int grfStatFlag);
	}
}