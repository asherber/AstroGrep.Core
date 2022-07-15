using System;
using System.Runtime.InteropServices;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("0000010A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistStorage
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="pClassID"></param>
		void GetClassID(out Guid pClassID);

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int IsDirty();

		/// <summary>
		///
		/// </summary>
		/// <param name="pstg"></param>
		void InitNew(IStorage pstg);

		/// <summary>
		///
		/// </summary>
		/// <param name="pstg"></param>
		/// <returns></returns>
		[PreserveSig]
		int Load(IStorage pstg);

		/// <summary>
		///
		/// </summary>
		/// <param name="pStgSave"></param>
		/// <param name="fSameAsLoad"></param>
		void Save(IStorage pStgSave, bool fSameAsLoad);

		/// <summary>
		///
		/// </summary>
		/// <param name="pStgNew"></param>
		void SaveCompleted(IStorage pStgNew);

		/// <summary>
		///
		/// </summary>
		void HandsOffStorage();
	}
}