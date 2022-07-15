using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public class PROPSPEC
	{
		/// <summary></summary>
		public ulKind propType;

		/// <summary></summary>
		public PROPSPECunion union;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct BLOB
	{
		/// <summary></summary>
		public uint cbSize;

		/// <summary></summary>
		public IntPtr pBlobData;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct BSTRBLOB
	{
		/// <summary></summary>
		public uint cbSize;

		/// <summary></summary>
		public IntPtr pData;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CArray
	{
		/// <summary></summary>
		public uint cElems;

		/// <summary></summary>
		public IntPtr pElems;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CY
	{
		/// <summary></summary>
		public uint Lo;

		/// <summary></summary>
		public int Hi;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct FILTERREGION
	{
		private uint idChunk;
		private uint cwcStart;
		private uint cwcExtent;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct FULLPROPSPEC
	{
		/// <summary></summary>
		public Guid guid;

		/// <summary></summary>
		public PROPSPEC property;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct PROPSPECunion
	{
		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr lpwstr;

		/// <summary></summary>
		[FieldOffset(0)]
		public uint propId;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PROPVARIANT
	{
		/// <summary></summary>
		public VARTYPE vt;

		/// <summary></summary>
		public ushort wReserved1;

		/// <summary></summary>
		public ushort wReserved2;

		/// <summary></summary>
		public ushort wReserved3;

		/// <summary></summary>
		public PropVariantUnion union;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct PropVariantUnion
	{
		// Fields
		/// <summary></summary>
		[FieldOffset(0)]
		public BLOB blob;

		/// <summary></summary>
		[FieldOffset(0)]
		public short boolVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public BSTRBLOB bstrblobVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr bstrVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public byte bVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public CArray cArray;

		/// <summary></summary>
		[FieldOffset(0)]
		public sbyte cVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public CY cyVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public double date;

		/// <summary></summary>
		[FieldOffset(0)]
		public double dblVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public FILETIME filetime;

		/// <summary></summary>
		[FieldOffset(0)]
		public float fltVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public long hVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public int intVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public short iVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public int lVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr parray;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pboolVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pbstrVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pbVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pclipdata;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pcVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pcyVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pdate;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pdblVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pdecVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pdispVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pfltVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pintVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr piVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr plVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pparray;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr ppdispVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr ppunkVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pscode;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pStorage;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pStream;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pszVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr puintVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr puiVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pulVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr punkVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr puuid;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pvarVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pVersionedStream;

		/// <summary></summary>
		[FieldOffset(0)]
		public IntPtr pwszVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public int scode;

		/// <summary></summary>
		[FieldOffset(0)]
		public ulong uhVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public uint uintVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public ushort uiVal;

		/// <summary></summary>
		[FieldOffset(0)]
		public uint ulVal;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct STAT_CHUNK
	{
		/// <summary></summary>
		public uint idChunk;

		/// <summary></summary>
		public CHUNK_BREAKTYPE breakType;

		/// <summary></summary>
		public CHUNKSTATE flags;

		/// <summary></summary>
		public uint locale;

		/// <summary></summary>
		public FULLPROPSPEC attribute;

		/// <summary></summary>
		public uint idChunkSource;

		/// <summary></summary>
		public uint cwcStartSource;

		/// <summary></summary>
		public uint cwcLenSource;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct STATPROPSETSTG
	{
		private Guid fmtid;
		private Guid clsid;
		private uint grfFlags;
		private FILETIME mtime;
		private FILETIME ctime;
		private FILETIME atime;
		private uint dwOSVersion;
	}

	/// <summary>
	///
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct STATPROPSTG
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		private string lpwstrName;

		private uint propid;
		private VARTYPE vt;
	}
}