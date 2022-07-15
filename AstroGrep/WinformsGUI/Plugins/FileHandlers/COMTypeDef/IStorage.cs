using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity, Guid("0000000B-0000-0000-C000-000000000046")]
	public interface IStorage
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="grfMode"></param>
		/// <param name="reserved1"></param>
		/// <param name="reserved2"></param>
		/// <returns></returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		IStream CreateStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="reserved1"></param>
		/// <param name="grfMode"></param>
		/// <param name="reserved2"></param>
		/// <returns></returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		IStream OpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr reserved1, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved2);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="grfMode"></param>
		/// <param name="reserved1"></param>
		/// <param name="reserved2"></param>
		/// <returns></returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		IStorage CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.U4)] int grfMode, [In, MarshalAs(UnmanagedType.U4)] int reserved1, [In, MarshalAs(UnmanagedType.U4)] int reserved2);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="pstgPriority"></param>
		/// <param name="grfMode"></param>
		/// <param name="snbExclude"></param>
		/// <param name="reserved"></param>
		/// <returns></returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		IStorage OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, IntPtr pstgPriority, [In, MarshalAs(UnmanagedType.U4)] int grfMode, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.U4)] int reserved);

		/// <summary>
		///
		/// </summary>
		/// <param name="ciidExclude"></param>
		/// <param name="pIIDExclude"></param>
		/// <param name="snbExclude"></param>
		/// <param name="stgDest"></param>
		void CopyTo(int ciidExclude, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] pIIDExclude, IntPtr snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="stgDest"></param>
		/// <param name="pwcsNewName"></param>
		/// <param name="grfFlags"></param>
		void MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage stgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In, MarshalAs(UnmanagedType.U4)] int grfFlags);

		/// <summary>
		///
		/// </summary>
		/// <param name="grfCommitFlags"></param>
		void Commit(int grfCommitFlags);

		/// <summary>
		///
		/// </summary>
		void Revert();

		/// <summary>
		///
		/// </summary>
		/// <param name="reserved1"></param>
		/// <param name="reserved2"></param>
		/// <param name="reserved3"></param>
		/// <param name="ppVal"></param>
		void EnumElements([In, MarshalAs(UnmanagedType.U4)] int reserved1, IntPtr reserved2, [In, MarshalAs(UnmanagedType.U4)] int reserved3, [MarshalAs(UnmanagedType.Interface)] out IEnumSTATSTG ppVal);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		void DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsOldName"></param>
		/// <param name="pwcsNewName"></param>
		void RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);

		/// <summary>
		///
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="pctime"></param>
		/// <param name="patime"></param>
		/// <param name="pmtime"></param>
		void SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] FILETIME pctime, [In] FILETIME patime, [In] FILETIME pmtime);

		/// <summary>
		///
		/// </summary>
		/// <param name="clsid"></param>
		void SetClass([In] ref Guid clsid);

		/// <summary>
		///
		/// </summary>
		/// <param name="grfStateBits"></param>
		/// <param name="grfMask"></param>
		void SetStateBits(int grfStateBits, int grfMask);

		/// <summary>
		///
		/// </summary>
		/// <param name="pStatStg"></param>
		/// <param name="grfStatFlag"></param>
		void Stat([Out] STATSTG pStatStg, int grfStatFlag);
	}
}