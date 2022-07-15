using System;

namespace AstroGrep.Plugins.FileHandlers.COMTypeDef
{
	/// <summary>
	/// </summary>
	public enum CHUNK_BREAKTYPE
	{
		/// <summary></summary>
		CHUNK_NO_BREAK,

		/// <summary></summary>
		CHUNK_EOW,

		/// <summary></summary>
		CHUNK_EOS,

		/// <summary></summary>
		CHUNK_EOP,

		/// <summary></summary>
		CHUNK_EOC
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum CHUNKSTATE
	{
		/// <summary></summary>
		CHUNK_FILTER_OWNED_VALUE = 4,
		/// <summary></summary>
		CHUNK_TEXT = 1,
		/// <summary></summary>
		CHUNK_VALUE = 2
	}

	/// <summary>
	/// 
	/// </summary>
	public enum CLIPFORMAT : ushort
	{
		/// <summary></summary>
		CF_TEXT = 1,
		/// <summary></summary>
		CF_BITMAP = 2,
		/// <summary></summary>
		CF_METAFILEPICT = 3,
		/// <summary></summary>
		CF_SYLK = 4,
		/// <summary></summary>
		CF_DIF = 5,
		/// <summary></summary>
		CF_TIFF = 6,
		/// <summary></summary>
		CF_OEMTEXT = 7,
		/// <summary></summary>
		CF_DIB = 8,
		/// <summary></summary>
		CF_PALETTE = 9,
		/// <summary></summary>
		CF_PENDATA = 10,
		/// <summary></summary>
		CF_RIFF = 11,
		/// <summary></summary>
		CF_WAVE = 12,
		/// <summary></summary>
		CF_UNICODETEXT = 13,
		/// <summary></summary>
		CF_ENHMETAFILE = 14,
		/// <summary></summary>
		CF_HDROP = 15,
		/// <summary></summary>
		CF_LOCALE = 16,
		/// <summary></summary>
		CF_MAX = 17,
		/// <summary></summary>
		CF_OWNERDISPLAY = 0x80,
		/// <summary></summary>
		CF_DSPTEXT = 0x81,
		/// <summary></summary>
		CF_DSPBITMAP = 0x82,
		/// <summary></summary>
		CF_DSPMETAFILEPICT = 0x83,
		/// <summary></summary>
		CF_DSPENHMETAFILE = 0x8E,
	}

	/// <summary>
	/// 
	/// </summary>
	public enum DocSumInfoProperty : uint
	{
		/// <summary></summary>
		PIDDSI_NOTECOUNT = 0x00000008,
		/// <summary></summary>
		PIDDSI_HIDDENCOUNT = 0x00000009,
		/// <summary></summary>
		PIDDSI_MMCLIPCOUNT = 0x0000000A,
		/// <summary></summary>
		PIDDSI_SCALE = 0x0000000B,
		/// <summary></summary>
		PIDDSI_HEADINGPAIR = 0x0000000C,
		/// <summary></summary>
		PIDDSI_DOCPARTS = 0x0000000D,
		/// <summary></summary>
		PIDDSI_MANAGER = 0x0000000E,
		/// <summary></summary>
		PIDDSI_COMPANY = 0x0000000F,
		/// <summary></summary>
		PIDDSI_LINKSDIRTY = 0x00000010
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum grfMode : uint
	{
		/// <summary></summary>
		STGM_DIRECT = 0x00000000,
		/// <summary></summary>
		STGM_TRANSACTED = 0x00010000,
		/// <summary></summary>
		STGM_SIMPLE = 0x08000000,

		/// <summary></summary>
		STGM_READ = 0x00000000,
		/// <summary></summary>
		STGM_WRITE = 0x00000001,
		/// <summary></summary>
		STGM_READWRITE = 0x00000002,

		/// <summary></summary>
		STGM_SHARE_DENY_NONE = 0x00000040,
		/// <summary></summary>
		STGM_SHARE_DENY_READ = 0x00000030,
		/// <summary></summary>
		STGM_SHARE_DENY_WRITE = 0x00000020,
		/// <summary></summary>
		STGM_SHARE_EXCLUSIVE = 0x00000010,

		/// <summary></summary>
		STGM_PRIORITY = 0x00040000,
		/// <summary></summary>
		STGM_DELETEONRELEASE = 0x04000000,
		/// <summary></summary>
		STGM_NOSCRATCH = 0x00100000,

		/// <summary></summary>
		STGM_CREATE = 0x00001000,
		/// <summary></summary>
		STGM_CONVERT = 0x00020000,
		/// <summary></summary>
		STGM_FAILIFTHERE = 0x00000000,

		/// <summary></summary>
		STGM_NOSNAPSHOT = 0x00200000,
		/// <summary></summary>
		STGM_DIRECT_SWMR = 0x00400000
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum IFILTER_FLAGS
	{
		/// <summary></summary>
		IFILTER_FLAGS_NONE,
		/// <summary></summary>
		IFILTER_FLAGS_OLE_PROPERTIES
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum IFILTER_INIT
	{
		/// <summary></summary>
		IFILTER_INIT_APPLY_CRAWL_ATTRIBUTES = 0x100,
		/// <summary></summary>
		IFILTER_INIT_APPLY_INDEX_ATTRIBUTES = 0x10,
		/// <summary></summary>
		IFILTER_INIT_APPLY_OTHER_ATTRIBUTES = 0x20,
		/// <summary></summary>
		IFILTER_INIT_CANON_HYPHENS = 4,
		/// <summary></summary>
		IFILTER_INIT_CANON_PARAGRAPHS = 1,
		/// <summary></summary>
		IFILTER_INIT_CANON_SPACES = 8,
		/// <summary></summary>
		IFILTER_INIT_FILTER_OWNED_VALUE_OK = 0x200,
		/// <summary></summary>
		IFILTER_INIT_HARD_LINE_BREAKS = 2,
		/// <summary></summary>
		IFILTER_INIT_INDEXING_ONLY = 0x40,
		/// <summary></summary>
		IFILTER_INIT_SEARCH_LINKS = 0x80
	}

	/// <summary>
	/// 
	/// </summary>
	public enum IFilterReturnCodes : uint
	{
		/// <summary>
		/// Success
		/// </summary>
		S_OK = 0,

		/// <summary>
		/// The function was denied access to the filter file.
		/// </summary>
		E_ACCESSDENIED = 0x80070005,

		/// <summary>
		/// The function encountered an invalid handle, probably due to a low-memory situation.
		/// </summary>
		E_HANDLE = 0x80070006,

		/// <summary>
		/// The function received an invalid parameter.
		/// </summary>
		E_INVALIDARG = 0x80070057,

		/// <summary>
		/// Out of memory
		/// </summary>
		E_OUTOFMEMORY = 0x8007000E,

		/// <summary>
		/// Not implemented
		/// </summary>
		E_NOTIMPL = 0x80004001,

		/// <summary>
		/// Unknown error
		/// </summary>
		E_FAIL = 0x80000008,

		/// <summary>
		/// File not filtered due to password protection
		/// </summary>
		FILTER_E_PASSWORD = 0x8004170B,

		/// <summary>
		/// The document format is not recognised by the filter
		/// </summary>
		FILTER_E_UNKNOWNFORMAT = 0x8004170C,

		/// <summary>
		/// No text in current chunk
		/// </summary>
		FILTER_E_NO_TEXT = 0x80041705,

		/// <summary>
		/// No values in current chunk
		/// </summary>
		FILTER_E_NO_VALUES = 0x80041706,

		/// <summary>
		/// No more chunks of text available in object
		/// </summary>
		FILTER_E_END_OF_CHUNKS = 0x80041700,

		/// <summary>
		/// No more text available in chunk
		/// </summary>
		FILTER_E_NO_MORE_TEXT = 0x80041701,

		/// <summary>
		/// No more property values available in chunk
		/// </summary>
		FILTER_E_NO_MORE_VALUES = 0x80041702,

		/// <summary>
		/// Unable to access object
		/// </summary>
		FILTER_E_ACCESS = 0x80041703,

		/// <summary>
		/// Moniker doesn't cover entire region
		/// </summary>
		FILTER_W_MONIKER_CLIPPED = 0x00041704,

		/// <summary>
		/// Unable to bind IFilter for embedded object
		/// </summary>
		FILTER_E_EMBEDDING_UNAVAILABLE = 0x80041707,

		/// <summary>
		/// Unable to bind IFilter for linked object
		/// </summary>
		FILTER_E_LINK_UNAVAILABLE = 0x80041708,

		/// <summary>
		/// This is the last text in the current chunk
		/// </summary>
		FILTER_S_LAST_TEXT = 0x00041709,

		/// <summary>
		/// This is the last value in the current chunk
		/// </summary>
		FILTER_S_LAST_VALUES = 0x0004170A,

		/// <summary>
		/// The data area passed to a system call is too small
		/// </summary>
		ERROR_INSUFFICIENT_BUFFER = 0x8007007A
	}

	/// <summary>
	/// 
	/// </summary>
	public enum PID_STG
	{
		/// <summary></summary>
		ACCESSTIME = 0x10,
		/// <summary></summary>
		ALLOCSIZE = 0x12,
		/// <summary></summary>
		ATTRIBUTES = 13,
		/// <summary></summary>
		CHANGETIME = 0x11,
		/// <summary></summary>
		CLASSID = 3,
		/// <summary></summary>
		CONTENTS = 0x13,
		/// <summary></summary>
		CREATETIME = 15,
		/// <summary></summary>
		DIRECTORY = 2,
		/// <summary></summary>
		FILEINDEX = 8,
		/// <summary></summary>
		LASTCHANGEUSN = 9,
		/// <summary></summary>
		NAME = 10,
		/// <summary></summary>
		PARENT_WORKID = 6,
		/// <summary></summary>
		PATH = 11,
		/// <summary></summary>
		SECONDARYSTORE = 7,
		/// <summary></summary>
		SHORTNAME = 20,
		/// <summary></summary>
		SIZE = 12,
		/// <summary></summary>
		STORAGETYPE = 4,
		/// <summary></summary>
		VOLUME_ID = 5,
		/// <summary></summary>
		WRITETIME = 14
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum PROPSETFLAG : uint
	{
		/// <summary></summary>
		PROPSETFLAG_DEFAULT = 0,
		/// <summary></summary>
		PROPSETFLAG_NONSIMPLE = 1,
		/// <summary></summary>
		PROPSETFLAG_ANSI = 2,
		/// <summary></summary>
		PROPSETFLAG_UNBUFFERED = 4,
		/// <summary></summary>
		PROPSETFLAG_CASE_SENSITIVE = 8
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum STGM_FLAGS
	{
		/// <summary></summary>
		ACCESS = 3,
		/// <summary></summary>
		CREATE = 0x1000,
		/// <summary></summary>
		MODE = 0x1000,
		/// <summary></summary>
		READ = 0,
		/// <summary></summary>
		READWRITE = 2,
		/// <summary></summary>
		SHARE_DENY_NONE = 0x40,
		/// <summary></summary>
		SHARE_DENY_READ = 0x30,
		/// <summary></summary>
		SHARE_DENY_WRITE = 0x20,
		/// <summary></summary>
		SHARE_EXCLUSIVE = 0x10,
		/// <summary></summary>
		SHARING = 0x70,
		/// <summary></summary>
		WRITE = 1
	}

	/// <summary>
	/// for PSGUID_STORAGE (defined in stgprop.h)
	/// </summary>
	public enum STGPROP : uint
	{
		/// <summary></summary>
		PID_STG_DIRECTORY = 0x00000002,
		/// <summary></summary>
		PID_STG_CLASSID = 0x00000003,
		/// <summary></summary>
		PID_STG_STORAGETYPE = 0x00000004,
		/// <summary></summary>
		PID_STG_VOLUME_ID = 0x00000005,
		/// <summary></summary>
		PID_STG_PARENT_WORKID = 0x00000006,
		/// <summary></summary>
		PID_STG_SECONDARYSTORE = 0x00000007,
		/// <summary></summary>
		PID_STG_FILEINDEX = 0x00000008,
		/// <summary></summary>
		PID_STG_LASTCHANGEUSN = 0x00000009,
		/// <summary></summary>
		PID_STG_NAME = 0x0000000a,
		/// <summary></summary>
		PID_STG_PATH = 0x0000000b,
		/// <summary></summary>
		PID_STG_SIZE = 0x0000000c,
		/// <summary></summary>
		PID_STG_ATTRIBUTES = 0x0000000d,
		/// <summary></summary>
		PID_STG_WRITETIME = 0x0000000e,
		/// <summary></summary>
		PID_STG_CREATETIME = 0x0000000f,
		/// <summary></summary>
		PID_STG_ACCESSTIME = 0x00000010,
		/// <summary></summary>
		PID_STG_CHANGETIME = 0x00000011,
		/// <summary></summary>
		PID_STG_ALLOCSIZE = 0x00000012,
		/// <summary></summary>
		PID_STG_CONTENTS = 0x00000013,
		/// <summary></summary>
		PID_STG_SHORTNAME = 0x00000014,
		/// <summary></summary>
		PID_STG_FRN = 0x00000015,
		/// <summary></summary>
		PID_STG_SCOPE = 0x00000016
	}

	/// <summary>
	/// 
	/// </summary>
	public enum SumInfoProperty : uint
	{
		/// <summary></summary>
		PIDSI_TITLE = 0x00000002,
		/// <summary></summary>
		PIDSI_SUBJECT = 0x00000003,
		/// <summary></summary>
		PIDSI_AUTHOR = 0x00000004,
		/// <summary></summary>
		PIDSI_KEYWORDS = 0x00000005,
		/// <summary></summary>
		PIDSI_COMMENTS = 0x00000006,
		/// <summary></summary>
		PIDSI_TEMPLATE = 0x00000007,
		/// <summary></summary>
		PIDSI_LASTAUTHOR = 0x00000008,
		/// <summary></summary>
		PIDSI_REVNUMBER = 0x00000009,
		/// <summary></summary>
		PIDSI_EDITTIME = 0x0000000A,
		/// <summary></summary>
		PIDSI_LASTPRINTED = 0x0000000B,
		/// <summary></summary>
		PIDSI_CREATE_DTM = 0x0000000C,
		/// <summary></summary>
		PIDSI_LASTSAVE_DTM = 0x0000000D,
		/// <summary></summary>
		PIDSI_PAGECOUNT = 0x0000000E,
		/// <summary></summary>
		PIDSI_WORDCOUNT = 0x0000000F,
		/// <summary></summary>
		PIDSI_CHARCOUNT = 0x00000010,
		/// <summary></summary>
		PIDSI_THUMBNAIL = 0x00000011,
		/// <summary></summary>
		PIDSI_APPNAME = 0x00000012,
		/// <summary></summary>
		PIDSI_SECURITY = 0x00000013
	}

	/// <summary>
	/// 
	/// </summary>
	public enum tagSTGTY
	{
		/// <summary></summary>
		STGTY_STORAGE = 1,
		/// <summary></summary>
		STGTY_STREAM = 2,
		/// <summary></summary>
		STGTY_LOCKBYTES = 3,
		/// <summary></summary>
		STGTY_PROPERTY = 4
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ulKind : uint
	{
		/// <summary></summary>
		PRSPEC_LPWSTR = 0,
		/// <summary></summary>
		PRSPEC_PROPID = 1
	}

	/// <summary>
	/// 
	/// </summary>
	public enum VARTYPE : short
	{
		/// <summary></summary>
		VT_BSTR = 8,
		/// <summary></summary>
		VT_FILETIME = 0x40,
		/// <summary></summary>
		VT_LPSTR = 30
	}

	/* Storage instantiation modes */
}