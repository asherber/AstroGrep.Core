using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[ComImport, Guid("0000013A-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyStorage
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="cpspec"></param>
		/// <param name="rgpspec"></param>
		/// <param name="rgpropvar"></param>
		/// <returns></returns>
		int ReadMultiple(uint cpspec, PROPSPEC[] rgpspec, PROPVARIANT[] rgpropvar);

		/// <summary>
		///
		/// </summary>
		/// <param name="cpspec"></param>
		/// <param name="rgpspec"></param>
		/// <param name="rgpropvar"></param>
		/// <param name="propidNameFirst"></param>
		void WriteMultiple(uint cpspec, PROPSPEC[] rgpspec, PROPVARIANT[] rgpropvar, uint propidNameFirst);

		/// <summary>
		///
		/// </summary>
		/// <param name="cpspec"></param>
		/// <param name="rgpspec"></param>
		void DeleteMultiple(uint cpspec, PROPSPEC[] rgpspec);

		/// <summary>
		///
		/// </summary>
		/// <param name="cpropid"></param>
		/// <param name="rgpropid"></param>
		/// <param name="rglpwstrName"></param>
		void ReadPropertyNames(uint cpropid, uint[] rgpropid, string[] rglpwstrName);

		/// <summary>
		///
		/// </summary>
		/// <param name="cpropid"></param>
		/// <param name="rgpropid"></param>
		/// <param name="rglpwstrName"></param>
		void WritePropertyNames(uint cpropid, uint[] rgpropid, string[] rglpwstrName);

		/// <summary>
		///
		/// </summary>
		/// <param name="cpropid"></param>
		/// <param name="rgpropid"></param>
		void DeletePropertyNames(uint cpropid, uint[] rgpropid);

		/// <summary>
		///
		/// </summary>
		/// <param name="clsid"></param>
		void SetClass(ref Guid clsid);

		/// <summary>
		///
		/// </summary>
		/// <param name="grfCommitFlags"></param>
		void Commit(uint grfCommitFlags);

		/// <summary>
		///
		/// </summary>
		void Revert();

		/// <summary>
		///
		/// </summary>
		/// <param name="ppenum"></param>
		void Enum(out IEnumSTATPROPSTG ppenum);

		/// <summary>
		///
		/// </summary>
		/// <param name="pstatpsstg"></param>
		void Stat(out STATPROPSETSTG pstatpsstg);

		/// <summary>
		///
		/// </summary>
		/// <param name="pctime"></param>
		/// <param name="patime"></param>
		/// <param name="pmtime"></param>
		void SetTimes(ref FILETIME pctime, ref FILETIME patime, ref FILETIME pmtime);
	}
}