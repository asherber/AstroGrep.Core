using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000109-0000-0000-C000-000000000046")]
	public interface IPersistStream
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="pClassID"></param>
		void GetClassID(out Guid pClassID);

		/// <summary>
		///
		/// </summary>
		/// <param name="pcbSize"></param>
		void GetSizeMax(out long pcbSize);

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int IsDirty();

		/// <summary>
		///
		/// </summary>
		/// <param name="pStm"></param>
		void Load([In] IStream pStm);

		/// <summary>
		///
		/// </summary>
		/// <param name="pStm"></param>
		/// <param name="fClearDirty"></param>
		void Save([In] IStream pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
	};
}