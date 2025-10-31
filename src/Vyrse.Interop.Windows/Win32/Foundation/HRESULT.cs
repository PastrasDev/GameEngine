using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="HRESULT.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.HRESULT']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct HRESULT : IEquatable<HRESULT>
{
	public readonly int Value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public HRESULT(int value) => Value = value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool(HRESULT value) => value.Value >= 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int(HRESULT value) => value.Value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator HRESULT(int value) => new(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(HRESULT left, HRESULT right) => left.Value == right.Value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(HRESULT left, HRESULT right) => !(left == right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(HRESULT other) => Value == other.Value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj) => obj is HRESULT other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => Value.GetHashCode();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", Value);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint(HRESULT value) => (uint)value.Value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator HRESULT(uint value) => new((int)value);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public bool Succeeded => Value >= 0;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public bool Failed => Value < 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public HRESULT ThrowOnFailure(string? message = null, IntPtr errorInfo = default)
	{
		if (Succeeded) return this;
		if (message == null) throw Marshal.GetExceptionForHR(Value, errorInfo)!;
		var ex = Marshal.GetExceptionForHR(Value, errorInfo)!;
		ex.Data["Context"] = message;
		ExceptionDispatchInfo.Capture(ex).Throw();
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider) => ((uint)Value).ToString(format, formatProvider);

	public static readonly HRESULT CO_E_NOTINITIALIZED = (HRESULT)(-2147221008);

	public static readonly HRESULT E_NOTIMPL = (HRESULT)(-2147467263);

	public static readonly HRESULT E_OUTOFMEMORY = (HRESULT)(-2147024882);

	public static readonly HRESULT E_INVALIDARG = (HRESULT)(-2147024809);

	public static readonly HRESULT E_FAIL = (HRESULT)(-2147467259);

	public static readonly HRESULT SEC_E_OK = (HRESULT)(0);

	public static readonly HRESULT E_UNEXPECTED = (HRESULT)(-2147418113);

	public static readonly HRESULT E_NOINTERFACE = (HRESULT)(-2147467262);

	public static readonly HRESULT E_POINTER = (HRESULT)(-2147467261);

	public static readonly HRESULT E_HANDLE = (HRESULT)(-2147024890);

	public static readonly HRESULT E_ABORT = (HRESULT)(-2147467260);

	public static readonly HRESULT E_ACCESSDENIED = (HRESULT)(-2147024891);

	public static readonly HRESULT E_BOUNDS = (HRESULT)(-2147483637);

	public static readonly HRESULT E_CHANGED_STATE = (HRESULT)(-2147483636);

	public static readonly HRESULT E_ILLEGAL_STATE_CHANGE = (HRESULT)(-2147483635);

	public static readonly HRESULT E_ILLEGAL_METHOD_CALL = (HRESULT)(-2147483634);

	public static readonly HRESULT RO_E_METADATA_NAME_NOT_FOUND = (HRESULT)(-2147483633);

	public static readonly HRESULT RO_E_METADATA_NAME_IS_NAMESPACE = (HRESULT)(-2147483632);

	public static readonly HRESULT RO_E_METADATA_INVALID_TYPE_FORMAT = (HRESULT)(-2147483631);

	public static readonly HRESULT RO_E_INVALID_METADATA_FILE = (HRESULT)(-2147483630);

	public static readonly HRESULT RO_E_CLOSED = (HRESULT)(-2147483629);

	public static readonly HRESULT RO_E_EXCLUSIVE_WRITE = (HRESULT)(-2147483628);

	public static readonly HRESULT RO_E_CHANGE_NOTIFICATION_IN_PROGRESS = (HRESULT)(-2147483627);

	public static readonly HRESULT RO_E_ERROR_STRING_NOT_FOUND = (HRESULT)(-2147483626);

	public static readonly HRESULT E_STRING_NOT_NULL_TERMINATED = (HRESULT)(-2147483625);

	public static readonly HRESULT E_ILLEGAL_DELEGATE_ASSIGNMENT = (HRESULT)(-2147483624);

	public static readonly HRESULT E_ASYNC_OPERATION_NOT_STARTED = (HRESULT)(-2147483623);

	public static readonly HRESULT E_APPLICATION_EXITING = (HRESULT)(-2147483622);

	public static readonly HRESULT E_APPLICATION_VIEW_EXITING = (HRESULT)(-2147483621);

	public static readonly HRESULT RO_E_MUST_BE_AGILE = (HRESULT)(-2147483620);

	public static readonly HRESULT RO_E_UNSUPPORTED_FROM_MTA = (HRESULT)(-2147483619);

	public static readonly HRESULT RO_E_COMMITTED = (HRESULT)(-2147483618);

	public static readonly HRESULT RO_E_BLOCKED_CROSS_ASTA_CALL = (HRESULT)(-2147483617);

	public static readonly HRESULT RO_E_CANNOT_ACTIVATE_FULL_TRUST_SERVER = (HRESULT)(-2147483616);

	public static readonly HRESULT RO_E_CANNOT_ACTIVATE_UNIVERSAL_APPLICATION_SERVER = (HRESULT)(-2147483615);

	public static readonly HRESULT CO_E_INIT_TLS = (HRESULT)(-2147467258);

	public static readonly HRESULT CO_E_INIT_SHARED_ALLOCATOR = (HRESULT)(-2147467257);

	public static readonly HRESULT CO_E_INIT_MEMORY_ALLOCATOR = (HRESULT)(-2147467256);

	public static readonly HRESULT CO_E_INIT_CLASS_CACHE = (HRESULT)(-2147467255);

	public static readonly HRESULT CO_E_INIT_RPC_CHANNEL = (HRESULT)(-2147467254);

	public static readonly HRESULT CO_E_INIT_TLS_SET_CHANNEL_CONTROL = (HRESULT)(-2147467253);

	public static readonly HRESULT CO_E_INIT_TLS_CHANNEL_CONTROL = (HRESULT)(-2147467252);

	public static readonly HRESULT CO_E_INIT_UNACCEPTED_USER_ALLOCATOR = (HRESULT)(-2147467251);

	public static readonly HRESULT CO_E_INIT_SCM_MUTEX_EXISTS = (HRESULT)(-2147467250);

	public static readonly HRESULT CO_E_INIT_SCM_FILE_MAPPING_EXISTS = (HRESULT)(-2147467249);

	public static readonly HRESULT CO_E_INIT_SCM_MAP_VIEW_OF_FILE = (HRESULT)(-2147467248);

	public static readonly HRESULT CO_E_INIT_SCM_EXEC_FAILURE = (HRESULT)(-2147467247);

	public static readonly HRESULT CO_E_INIT_ONLY_SINGLE_THREADED = (HRESULT)(-2147467246);

	public static readonly HRESULT CO_E_CANT_REMOTE = (HRESULT)(-2147467245);

	public static readonly HRESULT CO_E_BAD_SERVER_NAME = (HRESULT)(-2147467244);

	public static readonly HRESULT CO_E_WRONG_SERVER_IDENTITY = (HRESULT)(-2147467243);

	public static readonly HRESULT CO_E_OLE1DDE_DISABLED = (HRESULT)(-2147467242);

	public static readonly HRESULT CO_E_RUNAS_SYNTAX = (HRESULT)(-2147467241);

	public static readonly HRESULT CO_E_CREATEPROCESS_FAILURE = (HRESULT)(-2147467240);

	public static readonly HRESULT CO_E_RUNAS_CREATEPROCESS_FAILURE = (HRESULT)(-2147467239);

	public static readonly HRESULT CO_E_RUNAS_LOGON_FAILURE = (HRESULT)(-2147467238);

	public static readonly HRESULT CO_E_LAUNCH_PERMSSION_DENIED = (HRESULT)(-2147467237);

	public static readonly HRESULT CO_E_START_SERVICE_FAILURE = (HRESULT)(-2147467236);

	public static readonly HRESULT CO_E_REMOTE_COMMUNICATION_FAILURE = (HRESULT)(-2147467235);

	public static readonly HRESULT CO_E_SERVER_START_TIMEOUT = (HRESULT)(-2147467234);

	public static readonly HRESULT CO_E_CLSREG_INCONSISTENT = (HRESULT)(-2147467233);

	public static readonly HRESULT CO_E_IIDREG_INCONSISTENT = (HRESULT)(-2147467232);

	public static readonly HRESULT CO_E_NOT_SUPPORTED = (HRESULT)(-2147467231);

	public static readonly HRESULT CO_E_RELOAD_DLL = (HRESULT)(-2147467230);

	public static readonly HRESULT CO_E_MSI_ERROR = (HRESULT)(-2147467229);

	public static readonly HRESULT CO_E_ATTEMPT_TO_CREATE_OUTSIDE_CLIENT_CONTEXT = (HRESULT)(-2147467228);

	public static readonly HRESULT CO_E_SERVER_PAUSED = (HRESULT)(-2147467227);

	public static readonly HRESULT CO_E_SERVER_NOT_PAUSED = (HRESULT)(-2147467226);

	public static readonly HRESULT CO_E_CLASS_DISABLED = (HRESULT)(-2147467225);

	public static readonly HRESULT CO_E_CLRNOTAVAILABLE = (HRESULT)(-2147467224);

	public static readonly HRESULT CO_E_ASYNC_WORK_REJECTED = (HRESULT)(-2147467223);

	public static readonly HRESULT CO_E_SERVER_INIT_TIMEOUT = (HRESULT)(-2147467222);

	public static readonly HRESULT CO_E_NO_SECCTX_IN_ACTIVATE = (HRESULT)(-2147467221);

	public static readonly HRESULT CO_E_TRACKER_CONFIG = (HRESULT)(-2147467216);

	public static readonly HRESULT CO_E_THREADPOOL_CONFIG = (HRESULT)(-2147467215);

	public static readonly HRESULT CO_E_SXS_CONFIG = (HRESULT)(-2147467214);

	public static readonly HRESULT CO_E_MALFORMED_SPN = (HRESULT)(-2147467213);

	public static readonly HRESULT CO_E_UNREVOKED_REGISTRATION_ON_APARTMENT_SHUTDOWN = (HRESULT)(-2147467212);

	public static readonly HRESULT CO_E_PREMATURE_STUB_RUNDOWN = (HRESULT)(-2147467211);

	public static readonly HRESULT S_OK = (HRESULT)(0);

	public static readonly HRESULT S_FALSE = (HRESULT)(1);

	public static readonly HRESULT OLE_E_FIRST = (HRESULT)(-2147221504);

	public static readonly HRESULT OLE_E_LAST = (HRESULT)(-2147221249);

	public static readonly HRESULT OLE_S_FIRST = (HRESULT)(262144);

	public static readonly HRESULT OLE_S_LAST = (HRESULT)(262399);

	public static readonly HRESULT OLE_E_OLEVERB = (HRESULT)(-2147221504);

	public static readonly HRESULT OLE_E_ADVF = (HRESULT)(-2147221503);

	public static readonly HRESULT OLE_E_ENUM_NOMORE = (HRESULT)(-2147221502);

	public static readonly HRESULT OLE_E_ADVISENOTSUPPORTED = (HRESULT)(-2147221501);

	public static readonly HRESULT OLE_E_NOCONNECTION = (HRESULT)(-2147221500);

	public static readonly HRESULT OLE_E_NOTRUNNING = (HRESULT)(-2147221499);

	public static readonly HRESULT OLE_E_NOCACHE = (HRESULT)(-2147221498);

	public static readonly HRESULT OLE_E_BLANK = (HRESULT)(-2147221497);

	public static readonly HRESULT OLE_E_CLASSDIFF = (HRESULT)(-2147221496);

	public static readonly HRESULT OLE_E_CANT_GETMONIKER = (HRESULT)(-2147221495);

	public static readonly HRESULT OLE_E_CANT_BINDTOSOURCE = (HRESULT)(-2147221494);

	public static readonly HRESULT OLE_E_STATIC = (HRESULT)(-2147221493);

	public static readonly HRESULT OLE_E_PROMPTSAVECANCELLED = (HRESULT)(-2147221492);

	public static readonly HRESULT OLE_E_INVALIDRECT = (HRESULT)(-2147221491);

	public static readonly HRESULT OLE_E_WRONGCOMPOBJ = (HRESULT)(-2147221490);

	public static readonly HRESULT OLE_E_INVALIDHWND = (HRESULT)(-2147221489);

	public static readonly HRESULT OLE_E_NOT_INPLACEACTIVE = (HRESULT)(-2147221488);

	public static readonly HRESULT OLE_E_CANTCONVERT = (HRESULT)(-2147221487);

	public static readonly HRESULT OLE_E_NOSTORAGE = (HRESULT)(-2147221486);

	public static readonly HRESULT DV_E_FORMATETC = (HRESULT)(-2147221404);

	public static readonly HRESULT DV_E_DVTARGETDEVICE = (HRESULT)(-2147221403);

	public static readonly HRESULT DV_E_STGMEDIUM = (HRESULT)(-2147221402);

	public static readonly HRESULT DV_E_STATDATA = (HRESULT)(-2147221401);

	public static readonly HRESULT DV_E_LINDEX = (HRESULT)(-2147221400);

	public static readonly HRESULT DV_E_TYMED = (HRESULT)(-2147221399);

	public static readonly HRESULT DV_E_CLIPFORMAT = (HRESULT)(-2147221398);

	public static readonly HRESULT DV_E_DVASPECT = (HRESULT)(-2147221397);

	public static readonly HRESULT DV_E_DVTARGETDEVICE_SIZE = (HRESULT)(-2147221396);

	public static readonly HRESULT DV_E_NOIVIEWOBJECT = (HRESULT)(-2147221395);

	public static readonly HRESULT DRAGDROP_E_NOTREGISTERED = (HRESULT)(-2147221248);

	public static readonly HRESULT DRAGDROP_E_ALREADYREGISTERED = (HRESULT)(-2147221247);

	public static readonly HRESULT DRAGDROP_E_INVALIDHWND = (HRESULT)(-2147221246);

	public static readonly HRESULT DRAGDROP_E_CONCURRENT_DRAG_ATTEMPTED = (HRESULT)(-2147221245);

	public static readonly HRESULT CLASS_E_NOAGGREGATION = (HRESULT)(-2147221232);

	public static readonly HRESULT CLASS_E_CLASSNOTAVAILABLE = (HRESULT)(-2147221231);

	public static readonly HRESULT CLASS_E_NOTLICENSED = (HRESULT)(-2147221230);

	public static readonly HRESULT VIEW_E_DRAW = (HRESULT)(-2147221184);

	public static readonly HRESULT REGDB_E_READREGDB = (HRESULT)(-2147221168);

	public static readonly HRESULT REGDB_E_WRITEREGDB = (HRESULT)(-2147221167);

	public static readonly HRESULT REGDB_E_KEYMISSING = (HRESULT)(-2147221166);

	public static readonly HRESULT REGDB_E_INVALIDVALUE = (HRESULT)(-2147221165);

	public static readonly HRESULT REGDB_E_CLASSNOTREG = (HRESULT)(-2147221164);

	public static readonly HRESULT REGDB_E_IIDNOTREG = (HRESULT)(-2147221163);

	public static readonly HRESULT REGDB_E_BADTHREADINGMODEL = (HRESULT)(-2147221162);

	public static readonly HRESULT REGDB_E_PACKAGEPOLICYVIOLATION = (HRESULT)(-2147221161);

	public static readonly HRESULT CAT_E_CATIDNOEXIST = (HRESULT)(-2147221152);

	public static readonly HRESULT CAT_E_NODESCRIPTION = (HRESULT)(-2147221151);

	public static readonly HRESULT CS_E_PACKAGE_NOTFOUND = (HRESULT)(-2147221148);

	public static readonly HRESULT CS_E_NOT_DELETABLE = (HRESULT)(-2147221147);

	public static readonly HRESULT CS_E_CLASS_NOTFOUND = (HRESULT)(-2147221146);

	public static readonly HRESULT CS_E_INVALID_VERSION = (HRESULT)(-2147221145);

	public static readonly HRESULT CS_E_NO_CLASSSTORE = (HRESULT)(-2147221144);

	public static readonly HRESULT CS_E_OBJECT_NOTFOUND = (HRESULT)(-2147221143);

	public static readonly HRESULT CS_E_OBJECT_ALREADY_EXISTS = (HRESULT)(-2147221142);

	public static readonly HRESULT CS_E_INVALID_PATH = (HRESULT)(-2147221141);

	public static readonly HRESULT CS_E_NETWORK_ERROR = (HRESULT)(-2147221140);

	public static readonly HRESULT CS_E_ADMIN_LIMIT_EXCEEDED = (HRESULT)(-2147221139);

	public static readonly HRESULT CS_E_SCHEMA_MISMATCH = (HRESULT)(-2147221138);

	public static readonly HRESULT CS_E_INTERNAL_ERROR = (HRESULT)(-2147221137);

	public static readonly HRESULT CACHE_E_NOCACHE_UPDATED = (HRESULT)(-2147221136);

	public static readonly HRESULT OLEOBJ_E_NOVERBS = (HRESULT)(-2147221120);

	public static readonly HRESULT OLEOBJ_E_INVALIDVERB = (HRESULT)(-2147221119);

	public static readonly HRESULT INPLACE_E_NOTUNDOABLE = (HRESULT)(-2147221088);

	public static readonly HRESULT INPLACE_E_NOTOOLSPACE = (HRESULT)(-2147221087);

	public static readonly HRESULT CONVERT10_E_OLESTREAM_GET = (HRESULT)(-2147221056);

	public static readonly HRESULT CONVERT10_E_OLESTREAM_PUT = (HRESULT)(-2147221055);

	public static readonly HRESULT CONVERT10_E_OLESTREAM_FMT = (HRESULT)(-2147221054);

	public static readonly HRESULT CONVERT10_E_OLESTREAM_BITMAP_TO_DIB = (HRESULT)(-2147221053);

	public static readonly HRESULT CONVERT10_E_STG_FMT = (HRESULT)(-2147221052);

	public static readonly HRESULT CONVERT10_E_STG_NO_STD_STREAM = (HRESULT)(-2147221051);

	public static readonly HRESULT CONVERT10_E_STG_DIB_TO_BITMAP = (HRESULT)(-2147221050);

	public static readonly HRESULT CONVERT10_E_OLELINK_DISABLED = (HRESULT)(-2147221049);

	public static readonly HRESULT CLIPBRD_E_CANT_OPEN = (HRESULT)(-2147221040);

	public static readonly HRESULT CLIPBRD_E_CANT_EMPTY = (HRESULT)(-2147221039);

	public static readonly HRESULT CLIPBRD_E_CANT_SET = (HRESULT)(-2147221038);

	public static readonly HRESULT CLIPBRD_E_BAD_DATA = (HRESULT)(-2147221037);

	public static readonly HRESULT CLIPBRD_E_CANT_CLOSE = (HRESULT)(-2147221036);

	public static readonly HRESULT MK_E_CONNECTMANUALLY = (HRESULT)(-2147221024);

	public static readonly HRESULT MK_E_EXCEEDEDDEADLINE = (HRESULT)(-2147221023);

	public static readonly HRESULT MK_E_NEEDGENERIC = (HRESULT)(-2147221022);

	public static readonly HRESULT MK_E_UNAVAILABLE = (HRESULT)(-2147221021);

	public static readonly HRESULT MK_E_SYNTAX = (HRESULT)(-2147221020);

	public static readonly HRESULT MK_E_NOOBJECT = (HRESULT)(-2147221019);

	public static readonly HRESULT MK_E_INVALIDEXTENSION = (HRESULT)(-2147221018);

	public static readonly HRESULT MK_E_INTERMEDIATEINTERFACENOTSUPPORTED = (HRESULT)(-2147221017);

	public static readonly HRESULT MK_E_NOTBINDABLE = (HRESULT)(-2147221016);

	public static readonly HRESULT MK_E_NOTBOUND = (HRESULT)(-2147221015);

	public static readonly HRESULT MK_E_CANTOPENFILE = (HRESULT)(-2147221014);

	public static readonly HRESULT MK_E_MUSTBOTHERUSER = (HRESULT)(-2147221013);

	public static readonly HRESULT MK_E_NOINVERSE = (HRESULT)(-2147221012);

	public static readonly HRESULT MK_E_NOSTORAGE = (HRESULT)(-2147221011);

	public static readonly HRESULT MK_E_NOPREFIX = (HRESULT)(-2147221010);

	public static readonly HRESULT MK_E_ENUMERATION_FAILED = (HRESULT)(-2147221009);

	public static readonly HRESULT CO_E_ALREADYINITIALIZED = (HRESULT)(-2147221007);

	public static readonly HRESULT CO_E_CANTDETERMINECLASS = (HRESULT)(-2147221006);

	public static readonly HRESULT CO_E_CLASSSTRING = (HRESULT)(-2147221005);

	public static readonly HRESULT CO_E_IIDSTRING = (HRESULT)(-2147221004);

	public static readonly HRESULT CO_E_APPNOTFOUND = (HRESULT)(-2147221003);

	public static readonly HRESULT CO_E_APPSINGLEUSE = (HRESULT)(-2147221002);

	public static readonly HRESULT CO_E_ERRORINAPP = (HRESULT)(-2147221001);

	public static readonly HRESULT CO_E_DLLNOTFOUND = (HRESULT)(-2147221000);

	public static readonly HRESULT CO_E_ERRORINDLL = (HRESULT)(-2147220999);

	public static readonly HRESULT CO_E_WRONGOSFORAPP = (HRESULT)(-2147220998);

	public static readonly HRESULT CO_E_OBJNOTREG = (HRESULT)(-2147220997);

	public static readonly HRESULT CO_E_OBJISREG = (HRESULT)(-2147220996);

	public static readonly HRESULT CO_E_OBJNOTCONNECTED = (HRESULT)(-2147220995);

	public static readonly HRESULT CO_E_APPDIDNTREG = (HRESULT)(-2147220994);

	public static readonly HRESULT CO_E_RELEASED = (HRESULT)(-2147220993);

	public static readonly HRESULT EVENT_S_SOME_SUBSCRIBERS_FAILED = (HRESULT)(262656);

	public static readonly HRESULT EVENT_E_ALL_SUBSCRIBERS_FAILED = (HRESULT)(-2147220991);

	public static readonly HRESULT EVENT_S_NOSUBSCRIBERS = (HRESULT)(262658);

	public static readonly HRESULT EVENT_E_QUERYSYNTAX = (HRESULT)(-2147220989);

	public static readonly HRESULT EVENT_E_QUERYFIELD = (HRESULT)(-2147220988);

	public static readonly HRESULT EVENT_E_INTERNALEXCEPTION = (HRESULT)(-2147220987);

	public static readonly HRESULT EVENT_E_INTERNALERROR = (HRESULT)(-2147220986);

	public static readonly HRESULT EVENT_E_INVALID_PER_USER_SID = (HRESULT)(-2147220985);

	public static readonly HRESULT EVENT_E_USER_EXCEPTION = (HRESULT)(-2147220984);

	public static readonly HRESULT EVENT_E_TOO_MANY_METHODS = (HRESULT)(-2147220983);

	public static readonly HRESULT EVENT_E_MISSING_EVENTCLASS = (HRESULT)(-2147220982);

	public static readonly HRESULT EVENT_E_NOT_ALL_REMOVED = (HRESULT)(-2147220981);

	public static readonly HRESULT EVENT_E_COMPLUS_NOT_INSTALLED = (HRESULT)(-2147220980);

	public static readonly HRESULT EVENT_E_CANT_MODIFY_OR_DELETE_UNCONFIGURED_OBJECT = (HRESULT)(-2147220979);

	public static readonly HRESULT EVENT_E_CANT_MODIFY_OR_DELETE_CONFIGURED_OBJECT = (HRESULT)(-2147220978);

	public static readonly HRESULT EVENT_E_INVALID_EVENT_CLASS_PARTITION = (HRESULT)(-2147220977);

	public static readonly HRESULT EVENT_E_PER_USER_SID_NOT_LOGGED_ON = (HRESULT)(-2147220976);

	public static readonly HRESULT TPC_E_INVALID_PROPERTY = (HRESULT)(-2147220927);

	public static readonly HRESULT TPC_E_NO_DEFAULT_TABLET = (HRESULT)(-2147220974);

	public static readonly HRESULT TPC_E_UNKNOWN_PROPERTY = (HRESULT)(-2147220965);

	public static readonly HRESULT TPC_E_INVALID_INPUT_RECT = (HRESULT)(-2147220967);

	public static readonly HRESULT TPC_E_INVALID_STROKE = (HRESULT)(-2147220958);

	public static readonly HRESULT TPC_E_INITIALIZE_FAIL = (HRESULT)(-2147220957);

	public static readonly HRESULT TPC_E_NOT_RELEVANT = (HRESULT)(-2147220942);

	public static readonly HRESULT TPC_E_INVALID_PACKET_DESCRIPTION = (HRESULT)(-2147220941);

	public static readonly HRESULT TPC_E_RECOGNIZER_NOT_REGISTERED = (HRESULT)(-2147220939);

	public static readonly HRESULT TPC_E_INVALID_RIGHTS = (HRESULT)(-2147220938);

	public static readonly HRESULT TPC_E_OUT_OF_ORDER_CALL = (HRESULT)(-2147220937);

	public static readonly HRESULT TPC_E_QUEUE_FULL = (HRESULT)(-2147220936);

	public static readonly HRESULT TPC_E_INVALID_CONFIGURATION = (HRESULT)(-2147220935);

	public static readonly HRESULT TPC_E_INVALID_DATA_FROM_RECOGNIZER = (HRESULT)(-2147220934);

	public static readonly HRESULT TPC_S_TRUNCATED = (HRESULT)(262738);

	public static readonly HRESULT TPC_S_INTERRUPTED = (HRESULT)(262739);

	public static readonly HRESULT TPC_S_NO_DATA_TO_PROCESS = (HRESULT)(262740);

	public static readonly HRESULT XACT_E_ALREADYOTHERSINGLEPHASE = (HRESULT)(-2147168256);

	public static readonly HRESULT XACT_E_CANTRETAIN = (HRESULT)(-2147168255);

	public static readonly HRESULT XACT_E_COMMITFAILED = (HRESULT)(-2147168254);

	public static readonly HRESULT XACT_E_COMMITPREVENTED = (HRESULT)(-2147168253);

	public static readonly HRESULT XACT_E_HEURISTICABORT = (HRESULT)(-2147168252);

	public static readonly HRESULT XACT_E_HEURISTICCOMMIT = (HRESULT)(-2147168251);

	public static readonly HRESULT XACT_E_HEURISTICDAMAGE = (HRESULT)(-2147168250);

	public static readonly HRESULT XACT_E_HEURISTICDANGER = (HRESULT)(-2147168249);

	public static readonly HRESULT XACT_E_ISOLATIONLEVEL = (HRESULT)(-2147168248);

	public static readonly HRESULT XACT_E_NOASYNC = (HRESULT)(-2147168247);

	public static readonly HRESULT XACT_E_NOENLIST = (HRESULT)(-2147168246);

	public static readonly HRESULT XACT_E_NOISORETAIN = (HRESULT)(-2147168245);

	public static readonly HRESULT XACT_E_NORESOURCE = (HRESULT)(-2147168244);

	public static readonly HRESULT XACT_E_NOTCURRENT = (HRESULT)(-2147168243);

	public static readonly HRESULT XACT_E_NOTRANSACTION = (HRESULT)(-2147168242);

	public static readonly HRESULT XACT_E_NOTSUPPORTED = (HRESULT)(-2147168241);

	public static readonly HRESULT XACT_E_UNKNOWNRMGRID = (HRESULT)(-2147168240);

	public static readonly HRESULT XACT_E_WRONGSTATE = (HRESULT)(-2147168239);

	public static readonly HRESULT XACT_E_WRONGUOW = (HRESULT)(-2147168238);

	public static readonly HRESULT XACT_E_XTIONEXISTS = (HRESULT)(-2147168237);

	public static readonly HRESULT XACT_E_NOIMPORTOBJECT = (HRESULT)(-2147168236);

	public static readonly HRESULT XACT_E_INVALIDCOOKIE = (HRESULT)(-2147168235);

	public static readonly HRESULT XACT_E_INDOUBT = (HRESULT)(-2147168234);

	public static readonly HRESULT XACT_E_NOTIMEOUT = (HRESULT)(-2147168233);

	public static readonly HRESULT XACT_E_ALREADYINPROGRESS = (HRESULT)(-2147168232);

	public static readonly HRESULT XACT_E_ABORTED = (HRESULT)(-2147168231);

	public static readonly HRESULT XACT_E_LOGFULL = (HRESULT)(-2147168230);

	public static readonly HRESULT XACT_E_TMNOTAVAILABLE = (HRESULT)(-2147168229);

	public static readonly HRESULT XACT_E_CONNECTION_DOWN = (HRESULT)(-2147168228);

	public static readonly HRESULT XACT_E_CONNECTION_DENIED = (HRESULT)(-2147168227);

	public static readonly HRESULT XACT_E_REENLISTTIMEOUT = (HRESULT)(-2147168226);

	public static readonly HRESULT XACT_E_TIP_CONNECT_FAILED = (HRESULT)(-2147168225);

	public static readonly HRESULT XACT_E_TIP_PROTOCOL_ERROR = (HRESULT)(-2147168224);

	public static readonly HRESULT XACT_E_TIP_PULL_FAILED = (HRESULT)(-2147168223);

	public static readonly HRESULT XACT_E_DEST_TMNOTAVAILABLE = (HRESULT)(-2147168222);

	public static readonly HRESULT XACT_E_TIP_DISABLED = (HRESULT)(-2147168221);

	public static readonly HRESULT XACT_E_NETWORK_TX_DISABLED = (HRESULT)(-2147168220);

	public static readonly HRESULT XACT_E_PARTNER_NETWORK_TX_DISABLED = (HRESULT)(-2147168219);

	public static readonly HRESULT XACT_E_XA_TX_DISABLED = (HRESULT)(-2147168218);

	public static readonly HRESULT XACT_E_UNABLE_TO_READ_DTC_CONFIG = (HRESULT)(-2147168217);

	public static readonly HRESULT XACT_E_UNABLE_TO_LOAD_DTC_PROXY = (HRESULT)(-2147168216);

	public static readonly HRESULT XACT_E_ABORTING = (HRESULT)(-2147168215);

	public static readonly HRESULT XACT_E_PUSH_COMM_FAILURE = (HRESULT)(-2147168214);

	public static readonly HRESULT XACT_E_PULL_COMM_FAILURE = (HRESULT)(-2147168213);

	public static readonly HRESULT XACT_E_LU_TX_DISABLED = (HRESULT)(-2147168212);

	public static readonly HRESULT XACT_E_CLERKNOTFOUND = (HRESULT)(-2147168128);

	public static readonly HRESULT XACT_E_CLERKEXISTS = (HRESULT)(-2147168127);

	public static readonly HRESULT XACT_E_RECOVERYINPROGRESS = (HRESULT)(-2147168126);

	public static readonly HRESULT XACT_E_TRANSACTIONCLOSED = (HRESULT)(-2147168125);

	public static readonly HRESULT XACT_E_INVALIDLSN = (HRESULT)(-2147168124);

	public static readonly HRESULT XACT_E_REPLAYREQUEST = (HRESULT)(-2147168123);

	public static readonly HRESULT XACT_S_ASYNC = (HRESULT)(315392);

	public static readonly HRESULT XACT_S_DEFECT = (HRESULT)(315393);

	public static readonly HRESULT XACT_S_READONLY = (HRESULT)(315394);

	public static readonly HRESULT XACT_S_SOMENORETAIN = (HRESULT)(315395);

	public static readonly HRESULT XACT_S_OKINFORM = (HRESULT)(315396);

	public static readonly HRESULT XACT_S_MADECHANGESCONTENT = (HRESULT)(315397);

	public static readonly HRESULT XACT_S_MADECHANGESINFORM = (HRESULT)(315398);

	public static readonly HRESULT XACT_S_ALLNORETAIN = (HRESULT)(315399);

	public static readonly HRESULT XACT_S_ABORTING = (HRESULT)(315400);

	public static readonly HRESULT XACT_S_SINGLEPHASE = (HRESULT)(315401);

	public static readonly HRESULT XACT_S_LOCALLY_OK = (HRESULT)(315402);

	public static readonly HRESULT XACT_S_LASTRESOURCEMANAGER = (HRESULT)(315408);

	public static readonly HRESULT CONTEXT_E_ABORTED = (HRESULT)(-2147164158);

	public static readonly HRESULT CONTEXT_E_ABORTING = (HRESULT)(-2147164157);

	public static readonly HRESULT CONTEXT_E_NOCONTEXT = (HRESULT)(-2147164156);

	public static readonly HRESULT CONTEXT_E_WOULD_DEADLOCK = (HRESULT)(-2147164155);

	public static readonly HRESULT CONTEXT_E_SYNCH_TIMEOUT = (HRESULT)(-2147164154);

	public static readonly HRESULT CONTEXT_E_OLDREF = (HRESULT)(-2147164153);

	public static readonly HRESULT CONTEXT_E_ROLENOTFOUND = (HRESULT)(-2147164148);

	public static readonly HRESULT CONTEXT_E_TMNOTAVAILABLE = (HRESULT)(-2147164145);

	public static readonly HRESULT CO_E_ACTIVATIONFAILED = (HRESULT)(-2147164127);

	public static readonly HRESULT CO_E_ACTIVATIONFAILED_EVENTLOGGED = (HRESULT)(-2147164126);

	public static readonly HRESULT CO_E_ACTIVATIONFAILED_CATALOGERROR = (HRESULT)(-2147164125);

	public static readonly HRESULT CO_E_ACTIVATIONFAILED_TIMEOUT = (HRESULT)(-2147164124);

	public static readonly HRESULT CO_E_INITIALIZATIONFAILED = (HRESULT)(-2147164123);

	public static readonly HRESULT CONTEXT_E_NOJIT = (HRESULT)(-2147164122);

	public static readonly HRESULT CONTEXT_E_NOTRANSACTION = (HRESULT)(-2147164121);

	public static readonly HRESULT CO_E_THREADINGMODEL_CHANGED = (HRESULT)(-2147164120);

	public static readonly HRESULT CO_E_NOIISINTRINSICS = (HRESULT)(-2147164119);

	public static readonly HRESULT CO_E_NOCOOKIES = (HRESULT)(-2147164118);

	public static readonly HRESULT CO_E_DBERROR = (HRESULT)(-2147164117);

	public static readonly HRESULT CO_E_NOTPOOLED = (HRESULT)(-2147164116);

	public static readonly HRESULT CO_E_NOTCONSTRUCTED = (HRESULT)(-2147164115);

	public static readonly HRESULT CO_E_NOSYNCHRONIZATION = (HRESULT)(-2147164114);

	public static readonly HRESULT CO_E_ISOLEVELMISMATCH = (HRESULT)(-2147164113);

	public static readonly HRESULT CO_E_CALL_OUT_OF_TX_SCOPE_NOT_ALLOWED = (HRESULT)(-2147164112);

	public static readonly HRESULT CO_E_EXIT_TRANSACTION_SCOPE_NOT_CALLED = (HRESULT)(-2147164111);

	public static readonly HRESULT OLE_S_USEREG = (HRESULT)(262144);

	public static readonly HRESULT OLE_S_STATIC = (HRESULT)(262145);

	public static readonly HRESULT OLE_S_MAC_CLIPFORMAT = (HRESULT)(262146);

	public static readonly HRESULT DRAGDROP_S_DROP = (HRESULT)(262400);

	public static readonly HRESULT DRAGDROP_S_CANCEL = (HRESULT)(262401);

	public static readonly HRESULT DRAGDROP_S_USEDEFAULTCURSORS = (HRESULT)(262402);

	public static readonly HRESULT DATA_S_SAMEFORMATETC = (HRESULT)(262448);

	public static readonly HRESULT VIEW_S_ALREADY_FROZEN = (HRESULT)(262464);

	public static readonly HRESULT CACHE_S_FORMATETC_NOTSUPPORTED = (HRESULT)(262512);

	public static readonly HRESULT CACHE_S_SAMECACHE = (HRESULT)(262513);

	public static readonly HRESULT CACHE_S_SOMECACHES_NOTUPDATED = (HRESULT)(262514);

	public static readonly HRESULT OLEOBJ_S_INVALIDVERB = (HRESULT)(262528);

	public static readonly HRESULT OLEOBJ_S_CANNOT_DOVERB_NOW = (HRESULT)(262529);

	public static readonly HRESULT OLEOBJ_S_INVALIDHWND = (HRESULT)(262530);

	public static readonly HRESULT INPLACE_S_TRUNCATED = (HRESULT)(262560);

	public static readonly HRESULT CONVERT10_S_NO_PRESENTATION = (HRESULT)(262592);

	public static readonly HRESULT MK_S_REDUCED_TO_SELF = (HRESULT)(262626);

	public static readonly HRESULT MK_S_ME = (HRESULT)(262628);

	public static readonly HRESULT MK_S_HIM = (HRESULT)(262629);

	public static readonly HRESULT MK_S_US = (HRESULT)(262630);

	public static readonly HRESULT MK_S_MONIKERALREADYREGISTERED = (HRESULT)(262631);

	public static readonly HRESULT SCHED_S_TASK_READY = (HRESULT)(267008);

	public static readonly HRESULT SCHED_S_TASK_RUNNING = (HRESULT)(267009);

	public static readonly HRESULT SCHED_S_TASK_DISABLED = (HRESULT)(267010);

	public static readonly HRESULT SCHED_S_TASK_HAS_NOT_RUN = (HRESULT)(267011);

	public static readonly HRESULT SCHED_S_TASK_NO_MORE_RUNS = (HRESULT)(267012);

	public static readonly HRESULT SCHED_S_TASK_NOT_SCHEDULED = (HRESULT)(267013);

	public static readonly HRESULT SCHED_S_TASK_TERMINATED = (HRESULT)(267014);

	public static readonly HRESULT SCHED_S_TASK_NO_VALID_TRIGGERS = (HRESULT)(267015);

	public static readonly HRESULT SCHED_S_EVENT_TRIGGER = (HRESULT)(267016);

	public static readonly HRESULT SCHED_E_TRIGGER_NOT_FOUND = (HRESULT)(-2147216631);

	public static readonly HRESULT SCHED_E_TASK_NOT_READY = (HRESULT)(-2147216630);

	public static readonly HRESULT SCHED_E_TASK_NOT_RUNNING = (HRESULT)(-2147216629);

	public static readonly HRESULT SCHED_E_SERVICE_NOT_INSTALLED = (HRESULT)(-2147216628);

	public static readonly HRESULT SCHED_E_CANNOT_OPEN_TASK = (HRESULT)(-2147216627);

	public static readonly HRESULT SCHED_E_INVALID_TASK = (HRESULT)(-2147216626);

	public static readonly HRESULT SCHED_E_ACCOUNT_INFORMATION_NOT_SET = (HRESULT)(-2147216625);

	public static readonly HRESULT SCHED_E_ACCOUNT_NAME_NOT_FOUND = (HRESULT)(-2147216624);

	public static readonly HRESULT SCHED_E_ACCOUNT_DBASE_CORRUPT = (HRESULT)(-2147216623);

	public static readonly HRESULT SCHED_E_NO_SECURITY_SERVICES = (HRESULT)(-2147216622);

	public static readonly HRESULT SCHED_E_UNKNOWN_OBJECT_VERSION = (HRESULT)(-2147216621);

	public static readonly HRESULT SCHED_E_UNSUPPORTED_ACCOUNT_OPTION = (HRESULT)(-2147216620);

	public static readonly HRESULT SCHED_E_SERVICE_NOT_RUNNING = (HRESULT)(-2147216619);

	public static readonly HRESULT SCHED_E_UNEXPECTEDNODE = (HRESULT)(-2147216618);

	public static readonly HRESULT SCHED_E_NAMESPACE = (HRESULT)(-2147216617);

	public static readonly HRESULT SCHED_E_INVALIDVALUE = (HRESULT)(-2147216616);

	public static readonly HRESULT SCHED_E_MISSINGNODE = (HRESULT)(-2147216615);

	public static readonly HRESULT SCHED_E_MALFORMEDXML = (HRESULT)(-2147216614);

	public static readonly HRESULT SCHED_S_SOME_TRIGGERS_FAILED = (HRESULT)(267035);

	public static readonly HRESULT SCHED_S_BATCH_LOGON_PROBLEM = (HRESULT)(267036);

	public static readonly HRESULT SCHED_E_TOO_MANY_NODES = (HRESULT)(-2147216611);

	public static readonly HRESULT SCHED_E_PAST_END_BOUNDARY = (HRESULT)(-2147216610);

	public static readonly HRESULT SCHED_E_ALREADY_RUNNING = (HRESULT)(-2147216609);

	public static readonly HRESULT SCHED_E_USER_NOT_LOGGED_ON = (HRESULT)(-2147216608);

	public static readonly HRESULT SCHED_E_INVALID_TASK_HASH = (HRESULT)(-2147216607);

	public static readonly HRESULT SCHED_E_SERVICE_NOT_AVAILABLE = (HRESULT)(-2147216606);

	public static readonly HRESULT SCHED_E_SERVICE_TOO_BUSY = (HRESULT)(-2147216605);

	public static readonly HRESULT SCHED_E_TASK_ATTEMPTED = (HRESULT)(-2147216604);

	public static readonly HRESULT SCHED_S_TASK_QUEUED = (HRESULT)(267045);

	public static readonly HRESULT SCHED_E_TASK_DISABLED = (HRESULT)(-2147216602);

	public static readonly HRESULT SCHED_E_TASK_NOT_V1_COMPAT = (HRESULT)(-2147216601);

	public static readonly HRESULT SCHED_E_START_ON_DEMAND = (HRESULT)(-2147216600);

	public static readonly HRESULT SCHED_E_TASK_NOT_UBPM_COMPAT = (HRESULT)(-2147216599);

	public static readonly HRESULT SCHED_E_DEPRECATED_FEATURE_USED = (HRESULT)(-2147216592);

	public static readonly HRESULT CO_E_CLASS_CREATE_FAILED = (HRESULT)(-2146959359);

	public static readonly HRESULT CO_E_SCM_ERROR = (HRESULT)(-2146959358);

	public static readonly HRESULT CO_E_SCM_RPC_FAILURE = (HRESULT)(-2146959357);

	public static readonly HRESULT CO_E_BAD_PATH = (HRESULT)(-2146959356);

	public static readonly HRESULT CO_E_SERVER_EXEC_FAILURE = (HRESULT)(-2146959355);

	public static readonly HRESULT CO_E_OBJSRV_RPC_FAILURE = (HRESULT)(-2146959354);

	public static readonly HRESULT MK_E_NO_NORMALIZED = (HRESULT)(-2146959353);

	public static readonly HRESULT CO_E_SERVER_STOPPING = (HRESULT)(-2146959352);

	public static readonly HRESULT MEM_E_INVALID_ROOT = (HRESULT)(-2146959351);

	public static readonly HRESULT MEM_E_INVALID_LINK = (HRESULT)(-2146959344);

	public static readonly HRESULT MEM_E_INVALID_SIZE = (HRESULT)(-2146959343);

	public static readonly HRESULT CO_S_NOTALLINTERFACES = (HRESULT)(524306);

	public static readonly HRESULT CO_S_MACHINENAMENOTFOUND = (HRESULT)(524307);

	public static readonly HRESULT CO_E_MISSING_DISPLAYNAME = (HRESULT)(-2146959339);

	public static readonly HRESULT CO_E_RUNAS_VALUE_MUST_BE_AAA = (HRESULT)(-2146959338);

	public static readonly HRESULT CO_E_ELEVATION_DISABLED = (HRESULT)(-2146959337);

	public static readonly HRESULT APPX_E_PACKAGING_INTERNAL = (HRESULT)(-2146958848);

	public static readonly HRESULT APPX_E_INTERLEAVING_NOT_ALLOWED = (HRESULT)(-2146958847);

	public static readonly HRESULT APPX_E_RELATIONSHIPS_NOT_ALLOWED = (HRESULT)(-2146958846);

	public static readonly HRESULT APPX_E_MISSING_REQUIRED_FILE = (HRESULT)(-2146958845);

	public static readonly HRESULT APPX_E_INVALID_MANIFEST = (HRESULT)(-2146958844);

	public static readonly HRESULT APPX_E_INVALID_BLOCKMAP = (HRESULT)(-2146958843);

	public static readonly HRESULT APPX_E_CORRUPT_CONTENT = (HRESULT)(-2146958842);

	public static readonly HRESULT APPX_E_BLOCK_HASH_INVALID = (HRESULT)(-2146958841);

	public static readonly HRESULT APPX_E_REQUESTED_RANGE_TOO_LARGE = (HRESULT)(-2146958840);

	public static readonly HRESULT APPX_E_INVALID_SIP_CLIENT_DATA = (HRESULT)(-2146958839);

	public static readonly HRESULT APPX_E_INVALID_KEY_INFO = (HRESULT)(-2146958838);

	public static readonly HRESULT APPX_E_INVALID_CONTENTGROUPMAP = (HRESULT)(-2146958837);

	public static readonly HRESULT APPX_E_INVALID_APPINSTALLER = (HRESULT)(-2146958836);

	public static readonly HRESULT APPX_E_DELTA_BASELINE_VERSION_MISMATCH = (HRESULT)(-2146958835);

	public static readonly HRESULT APPX_E_DELTA_PACKAGE_MISSING_FILE = (HRESULT)(-2146958834);

	public static readonly HRESULT APPX_E_INVALID_DELTA_PACKAGE = (HRESULT)(-2146958833);

	public static readonly HRESULT APPX_E_DELTA_APPENDED_PACKAGE_NOT_ALLOWED = (HRESULT)(-2146958832);

	public static readonly HRESULT APPX_E_INVALID_PACKAGING_LAYOUT = (HRESULT)(-2146958831);

	public static readonly HRESULT APPX_E_INVALID_PACKAGESIGNCONFIG = (HRESULT)(-2146958830);

	public static readonly HRESULT APPX_E_RESOURCESPRI_NOT_ALLOWED = (HRESULT)(-2146958829);

	public static readonly HRESULT APPX_E_FILE_COMPRESSION_MISMATCH = (HRESULT)(-2146958828);

	public static readonly HRESULT APPX_E_INVALID_PAYLOAD_PACKAGE_EXTENSION = (HRESULT)(-2146958827);

	public static readonly HRESULT APPX_E_INVALID_ENCRYPTION_EXCLUSION_FILE_LIST = (HRESULT)(-2146958826);

	public static readonly HRESULT APPX_E_INVALID_PACKAGE_FOLDER_ACLS = (HRESULT)(-2146958825);

	public static readonly HRESULT APPX_E_INVALID_PUBLISHER_BRIDGING = (HRESULT)(-2146958824);

	public static readonly HRESULT APPX_E_DIGEST_MISMATCH = (HRESULT)(-2146958823);

	public static readonly HRESULT BT_E_SPURIOUS_ACTIVATION = (HRESULT)(-2146958592);

	public static readonly HRESULT DISP_E_UNKNOWNINTERFACE = (HRESULT)(-2147352575);

	public static readonly HRESULT DISP_E_MEMBERNOTFOUND = (HRESULT)(-2147352573);

	public static readonly HRESULT DISP_E_PARAMNOTFOUND = (HRESULT)(-2147352572);

	public static readonly HRESULT DISP_E_TYPEMISMATCH = (HRESULT)(-2147352571);

	public static readonly HRESULT DISP_E_UNKNOWNNAME = (HRESULT)(-2147352570);

	public static readonly HRESULT DISP_E_NONAMEDARGS = (HRESULT)(-2147352569);

	public static readonly HRESULT DISP_E_BADVARTYPE = (HRESULT)(-2147352568);

	public static readonly HRESULT DISP_E_EXCEPTION = (HRESULT)(-2147352567);

	public static readonly HRESULT DISP_E_OVERFLOW = (HRESULT)(-2147352566);

	public static readonly HRESULT DISP_E_BADINDEX = (HRESULT)(-2147352565);

	public static readonly HRESULT DISP_E_UNKNOWNLCID = (HRESULT)(-2147352564);

	public static readonly HRESULT DISP_E_ARRAYISLOCKED = (HRESULT)(-2147352563);

	public static readonly HRESULT DISP_E_BADPARAMCOUNT = (HRESULT)(-2147352562);

	public static readonly HRESULT DISP_E_PARAMNOTOPTIONAL = (HRESULT)(-2147352561);

	public static readonly HRESULT DISP_E_BADCALLEE = (HRESULT)(-2147352560);

	public static readonly HRESULT DISP_E_NOTACOLLECTION = (HRESULT)(-2147352559);

	public static readonly HRESULT DISP_E_DIVBYZERO = (HRESULT)(-2147352558);

	public static readonly HRESULT DISP_E_BUFFERTOOSMALL = (HRESULT)(-2147352557);

	public static readonly HRESULT TYPE_E_BUFFERTOOSMALL = (HRESULT)(-2147319786);

	public static readonly HRESULT TYPE_E_FIELDNOTFOUND = (HRESULT)(-2147319785);

	public static readonly HRESULT TYPE_E_INVDATAREAD = (HRESULT)(-2147319784);

	public static readonly HRESULT TYPE_E_UNSUPFORMAT = (HRESULT)(-2147319783);

	public static readonly HRESULT TYPE_E_REGISTRYACCESS = (HRESULT)(-2147319780);

	public static readonly HRESULT TYPE_E_LIBNOTREGISTERED = (HRESULT)(-2147319779);

	public static readonly HRESULT TYPE_E_UNDEFINEDTYPE = (HRESULT)(-2147319769);

	public static readonly HRESULT TYPE_E_QUALIFIEDNAMEDISALLOWED = (HRESULT)(-2147319768);

	public static readonly HRESULT TYPE_E_INVALIDSTATE = (HRESULT)(-2147319767);

	public static readonly HRESULT TYPE_E_WRONGTYPEKIND = (HRESULT)(-2147319766);

	public static readonly HRESULT TYPE_E_ELEMENTNOTFOUND = (HRESULT)(-2147319765);

	public static readonly HRESULT TYPE_E_AMBIGUOUSNAME = (HRESULT)(-2147319764);

	public static readonly HRESULT TYPE_E_NAMECONFLICT = (HRESULT)(-2147319763);

	public static readonly HRESULT TYPE_E_UNKNOWNLCID = (HRESULT)(-2147319762);

	public static readonly HRESULT TYPE_E_DLLFUNCTIONNOTFOUND = (HRESULT)(-2147319761);

	public static readonly HRESULT TYPE_E_BADMODULEKIND = (HRESULT)(-2147317571);

	public static readonly HRESULT TYPE_E_SIZETOOBIG = (HRESULT)(-2147317563);

	public static readonly HRESULT TYPE_E_DUPLICATEID = (HRESULT)(-2147317562);

	public static readonly HRESULT TYPE_E_INVALIDID = (HRESULT)(-2147317553);

	public static readonly HRESULT TYPE_E_TYPEMISMATCH = (HRESULT)(-2147316576);

	public static readonly HRESULT TYPE_E_OUTOFBOUNDS = (HRESULT)(-2147316575);

	public static readonly HRESULT TYPE_E_IOERROR = (HRESULT)(-2147316574);

	public static readonly HRESULT TYPE_E_CANTCREATETMPFILE = (HRESULT)(-2147316573);

	public static readonly HRESULT TYPE_E_CANTLOADLIBRARY = (HRESULT)(-2147312566);

	public static readonly HRESULT TYPE_E_INCONSISTENTPROPFUNCS = (HRESULT)(-2147312509);

	public static readonly HRESULT TYPE_E_CIRCULARTYPE = (HRESULT)(-2147312508);

	public static readonly HRESULT STG_E_INVALIDFUNCTION = (HRESULT)(-2147287039);

	public static readonly HRESULT STG_E_FILENOTFOUND = (HRESULT)(-2147287038);

	public static readonly HRESULT STG_E_PATHNOTFOUND = (HRESULT)(-2147287037);

	public static readonly HRESULT STG_E_TOOMANYOPENFILES = (HRESULT)(-2147287036);

	public static readonly HRESULT STG_E_ACCESSDENIED = (HRESULT)(-2147287035);

	public static readonly HRESULT STG_E_INVALIDHANDLE = (HRESULT)(-2147287034);

	public static readonly HRESULT STG_E_INSUFFICIENTMEMORY = (HRESULT)(-2147287032);

	public static readonly HRESULT STG_E_INVALIDPOINTER = (HRESULT)(-2147287031);

	public static readonly HRESULT STG_E_NOMOREFILES = (HRESULT)(-2147287022);

	public static readonly HRESULT STG_E_DISKISWRITEPROTECTED = (HRESULT)(-2147287021);

	public static readonly HRESULT STG_E_SEEKERROR = (HRESULT)(-2147287015);

	public static readonly HRESULT STG_E_WRITEFAULT = (HRESULT)(-2147287011);

	public static readonly HRESULT STG_E_READFAULT = (HRESULT)(-2147287010);

	public static readonly HRESULT STG_E_SHAREVIOLATION = (HRESULT)(-2147287008);

	public static readonly HRESULT STG_E_LOCKVIOLATION = (HRESULT)(-2147287007);

	public static readonly HRESULT STG_E_FILEALREADYEXISTS = (HRESULT)(-2147286960);

	public static readonly HRESULT STG_E_INVALIDPARAMETER = (HRESULT)(-2147286953);

	public static readonly HRESULT STG_E_MEDIUMFULL = (HRESULT)(-2147286928);

	public static readonly HRESULT STG_E_PROPSETMISMATCHED = (HRESULT)(-2147286800);

	public static readonly HRESULT STG_E_ABNORMALAPIEXIT = (HRESULT)(-2147286790);

	public static readonly HRESULT STG_E_INVALIDHEADER = (HRESULT)(-2147286789);

	public static readonly HRESULT STG_E_INVALIDNAME = (HRESULT)(-2147286788);

	public static readonly HRESULT STG_E_UNKNOWN = (HRESULT)(-2147286787);

	public static readonly HRESULT STG_E_UNIMPLEMENTEDFUNCTION = (HRESULT)(-2147286786);

	public static readonly HRESULT STG_E_INVALIDFLAG = (HRESULT)(-2147286785);

	public static readonly HRESULT STG_E_INUSE = (HRESULT)(-2147286784);

	public static readonly HRESULT STG_E_NOTCURRENT = (HRESULT)(-2147286783);

	public static readonly HRESULT STG_E_REVERTED = (HRESULT)(-2147286782);

	public static readonly HRESULT STG_E_CANTSAVE = (HRESULT)(-2147286781);

	public static readonly HRESULT STG_E_OLDFORMAT = (HRESULT)(-2147286780);

	public static readonly HRESULT STG_E_OLDDLL = (HRESULT)(-2147286779);

	public static readonly HRESULT STG_E_SHAREREQUIRED = (HRESULT)(-2147286778);

	public static readonly HRESULT STG_E_NOTFILEBASEDSTORAGE = (HRESULT)(-2147286777);

	public static readonly HRESULT STG_E_EXTANTMARSHALLINGS = (HRESULT)(-2147286776);

	public static readonly HRESULT STG_E_DOCFILECORRUPT = (HRESULT)(-2147286775);

	public static readonly HRESULT STG_E_BADBASEADDRESS = (HRESULT)(-2147286768);

	public static readonly HRESULT STG_E_DOCFILETOOLARGE = (HRESULT)(-2147286767);

	public static readonly HRESULT STG_E_NOTSIMPLEFORMAT = (HRESULT)(-2147286766);

	public static readonly HRESULT STG_E_INCOMPLETE = (HRESULT)(-2147286527);

	public static readonly HRESULT STG_E_TERMINATED = (HRESULT)(-2147286526);

	public static readonly HRESULT STG_S_CONVERTED = (HRESULT)(197120);

	public static readonly HRESULT STG_S_BLOCK = (HRESULT)(197121);

	public static readonly HRESULT STG_S_RETRYNOW = (HRESULT)(197122);

	public static readonly HRESULT STG_S_MONITORING = (HRESULT)(197123);

	public static readonly HRESULT STG_S_MULTIPLEOPENS = (HRESULT)(197124);

	public static readonly HRESULT STG_S_CONSOLIDATIONFAILED = (HRESULT)(197125);

	public static readonly HRESULT STG_S_CANNOTCONSOLIDATE = (HRESULT)(197126);

	public static readonly HRESULT STG_S_POWER_CYCLE_REQUIRED = (HRESULT)(197127);

	public static readonly HRESULT STG_E_FIRMWARE_SLOT_INVALID = (HRESULT)(-2147286520);

	public static readonly HRESULT STG_E_FIRMWARE_IMAGE_INVALID = (HRESULT)(-2147286519);

	public static readonly HRESULT STG_E_DEVICE_UNRESPONSIVE = (HRESULT)(-2147286518);

	public static readonly HRESULT STG_E_STATUS_COPY_PROTECTION_FAILURE = (HRESULT)(-2147286267);

	public static readonly HRESULT STG_E_CSS_AUTHENTICATION_FAILURE = (HRESULT)(-2147286266);

	public static readonly HRESULT STG_E_CSS_KEY_NOT_PRESENT = (HRESULT)(-2147286265);

	public static readonly HRESULT STG_E_CSS_KEY_NOT_ESTABLISHED = (HRESULT)(-2147286264);

	public static readonly HRESULT STG_E_CSS_SCRAMBLED_SECTOR = (HRESULT)(-2147286263);

	public static readonly HRESULT STG_E_CSS_REGION_MISMATCH = (HRESULT)(-2147286262);

	public static readonly HRESULT STG_E_RESETS_EXHAUSTED = (HRESULT)(-2147286261);

	public static readonly HRESULT RPC_E_CALL_REJECTED = (HRESULT)(-2147418111);

	public static readonly HRESULT RPC_E_CALL_CANCELED = (HRESULT)(-2147418110);

	public static readonly HRESULT RPC_E_CANTPOST_INSENDCALL = (HRESULT)(-2147418109);

	public static readonly HRESULT RPC_E_CANTCALLOUT_INASYNCCALL = (HRESULT)(-2147418108);

	public static readonly HRESULT RPC_E_CANTCALLOUT_INEXTERNALCALL = (HRESULT)(-2147418107);

	public static readonly HRESULT RPC_E_CONNECTION_TERMINATED = (HRESULT)(-2147418106);

	public static readonly HRESULT RPC_E_SERVER_DIED = (HRESULT)(-2147418105);

	public static readonly HRESULT RPC_E_CLIENT_DIED = (HRESULT)(-2147418104);

	public static readonly HRESULT RPC_E_INVALID_DATAPACKET = (HRESULT)(-2147418103);

	public static readonly HRESULT RPC_E_CANTTRANSMIT_CALL = (HRESULT)(-2147418102);

	public static readonly HRESULT RPC_E_CLIENT_CANTMARSHAL_DATA = (HRESULT)(-2147418101);

	public static readonly HRESULT RPC_E_CLIENT_CANTUNMARSHAL_DATA = (HRESULT)(-2147418100);

	public static readonly HRESULT RPC_E_SERVER_CANTMARSHAL_DATA = (HRESULT)(-2147418099);

	public static readonly HRESULT RPC_E_SERVER_CANTUNMARSHAL_DATA = (HRESULT)(-2147418098);

	public static readonly HRESULT RPC_E_INVALID_DATA = (HRESULT)(-2147418097);

	public static readonly HRESULT RPC_E_INVALID_PARAMETER = (HRESULT)(-2147418096);

	public static readonly HRESULT RPC_E_CANTCALLOUT_AGAIN = (HRESULT)(-2147418095);

	public static readonly HRESULT RPC_E_SERVER_DIED_DNE = (HRESULT)(-2147418094);

	public static readonly HRESULT RPC_E_SYS_CALL_FAILED = (HRESULT)(-2147417856);

	public static readonly HRESULT RPC_E_OUT_OF_RESOURCES = (HRESULT)(-2147417855);

	public static readonly HRESULT RPC_E_ATTEMPTED_MULTITHREAD = (HRESULT)(-2147417854);

	public static readonly HRESULT RPC_E_NOT_REGISTERED = (HRESULT)(-2147417853);

	public static readonly HRESULT RPC_E_FAULT = (HRESULT)(-2147417852);

	public static readonly HRESULT RPC_E_SERVERFAULT = (HRESULT)(-2147417851);

	public static readonly HRESULT RPC_E_CHANGED_MODE = (HRESULT)(-2147417850);

	public static readonly HRESULT RPC_E_INVALIDMETHOD = (HRESULT)(-2147417849);

	public static readonly HRESULT RPC_E_DISCONNECTED = (HRESULT)(-2147417848);

	public static readonly HRESULT RPC_E_RETRY = (HRESULT)(-2147417847);

	public static readonly HRESULT RPC_E_SERVERCALL_RETRYLATER = (HRESULT)(-2147417846);

	public static readonly HRESULT RPC_E_SERVERCALL_REJECTED = (HRESULT)(-2147417845);

	public static readonly HRESULT RPC_E_INVALID_CALLDATA = (HRESULT)(-2147417844);

	public static readonly HRESULT RPC_E_CANTCALLOUT_ININPUTSYNCCALL = (HRESULT)(-2147417843);

	public static readonly HRESULT RPC_E_WRONG_THREAD = (HRESULT)(-2147417842);

	public static readonly HRESULT RPC_E_THREAD_NOT_INIT = (HRESULT)(-2147417841);

	public static readonly HRESULT RPC_E_VERSION_MISMATCH = (HRESULT)(-2147417840);

	public static readonly HRESULT RPC_E_INVALID_HEADER = (HRESULT)(-2147417839);

	public static readonly HRESULT RPC_E_INVALID_EXTENSION = (HRESULT)(-2147417838);

	public static readonly HRESULT RPC_E_INVALID_IPID = (HRESULT)(-2147417837);

	public static readonly HRESULT RPC_E_INVALID_OBJECT = (HRESULT)(-2147417836);

	public static readonly HRESULT RPC_S_CALLPENDING = (HRESULT)(-2147417835);

	public static readonly HRESULT RPC_S_WAITONTIMER = (HRESULT)(-2147417834);

	public static readonly HRESULT RPC_E_CALL_COMPLETE = (HRESULT)(-2147417833);

	public static readonly HRESULT RPC_E_UNSECURE_CALL = (HRESULT)(-2147417832);

	public static readonly HRESULT RPC_E_TOO_LATE = (HRESULT)(-2147417831);

	public static readonly HRESULT RPC_E_NO_GOOD_SECURITY_PACKAGES = (HRESULT)(-2147417830);

	public static readonly HRESULT RPC_E_ACCESS_DENIED = (HRESULT)(-2147417829);

	public static readonly HRESULT RPC_E_REMOTE_DISABLED = (HRESULT)(-2147417828);

	public static readonly HRESULT RPC_E_INVALID_OBJREF = (HRESULT)(-2147417827);

	public static readonly HRESULT RPC_E_NO_CONTEXT = (HRESULT)(-2147417826);

	public static readonly HRESULT RPC_E_TIMEOUT = (HRESULT)(-2147417825);

	public static readonly HRESULT RPC_E_NO_SYNC = (HRESULT)(-2147417824);

	public static readonly HRESULT RPC_E_FULLSIC_REQUIRED = (HRESULT)(-2147417823);

	public static readonly HRESULT RPC_E_INVALID_STD_NAME = (HRESULT)(-2147417822);

	public static readonly HRESULT CO_E_FAILEDTOIMPERSONATE = (HRESULT)(-2147417821);

	public static readonly HRESULT CO_E_FAILEDTOGETSECCTX = (HRESULT)(-2147417820);

	public static readonly HRESULT CO_E_FAILEDTOOPENTHREADTOKEN = (HRESULT)(-2147417819);

	public static readonly HRESULT CO_E_FAILEDTOGETTOKENINFO = (HRESULT)(-2147417818);

	public static readonly HRESULT CO_E_TRUSTEEDOESNTMATCHCLIENT = (HRESULT)(-2147417817);

	public static readonly HRESULT CO_E_FAILEDTOQUERYCLIENTBLANKET = (HRESULT)(-2147417816);

	public static readonly HRESULT CO_E_FAILEDTOSETDACL = (HRESULT)(-2147417815);

	public static readonly HRESULT CO_E_ACCESSCHECKFAILED = (HRESULT)(-2147417814);

	public static readonly HRESULT CO_E_NETACCESSAPIFAILED = (HRESULT)(-2147417813);

	public static readonly HRESULT CO_E_WRONGTRUSTEENAMESYNTAX = (HRESULT)(-2147417812);

	public static readonly HRESULT CO_E_INVALIDSID = (HRESULT)(-2147417811);

	public static readonly HRESULT CO_E_CONVERSIONFAILED = (HRESULT)(-2147417810);

	public static readonly HRESULT CO_E_NOMATCHINGSIDFOUND = (HRESULT)(-2147417809);

	public static readonly HRESULT CO_E_LOOKUPACCSIDFAILED = (HRESULT)(-2147417808);

	public static readonly HRESULT CO_E_NOMATCHINGNAMEFOUND = (HRESULT)(-2147417807);

	public static readonly HRESULT CO_E_LOOKUPACCNAMEFAILED = (HRESULT)(-2147417806);

	public static readonly HRESULT CO_E_SETSERLHNDLFAILED = (HRESULT)(-2147417805);

	public static readonly HRESULT CO_E_FAILEDTOGETWINDIR = (HRESULT)(-2147417804);

	public static readonly HRESULT CO_E_PATHTOOLONG = (HRESULT)(-2147417803);

	public static readonly HRESULT CO_E_FAILEDTOGENUUID = (HRESULT)(-2147417802);

	public static readonly HRESULT CO_E_FAILEDTOCREATEFILE = (HRESULT)(-2147417801);

	public static readonly HRESULT CO_E_FAILEDTOCLOSEHANDLE = (HRESULT)(-2147417800);

	public static readonly HRESULT CO_E_EXCEEDSYSACLLIMIT = (HRESULT)(-2147417799);

	public static readonly HRESULT CO_E_ACESINWRONGORDER = (HRESULT)(-2147417798);

	public static readonly HRESULT CO_E_INCOMPATIBLESTREAMVERSION = (HRESULT)(-2147417797);

	public static readonly HRESULT CO_E_FAILEDTOOPENPROCESSTOKEN = (HRESULT)(-2147417796);

	public static readonly HRESULT CO_E_DECODEFAILED = (HRESULT)(-2147417795);

	public static readonly HRESULT CO_E_ACNOTINITIALIZED = (HRESULT)(-2147417793);

	public static readonly HRESULT CO_E_CANCEL_DISABLED = (HRESULT)(-2147417792);

	public static readonly HRESULT RPC_E_UNEXPECTED = (HRESULT)(-2147352577);

	public static readonly HRESULT ERROR_AUDITING_DISABLED = (HRESULT)(-1073151999);

	public static readonly HRESULT ERROR_ALL_SIDS_FILTERED = (HRESULT)(-1073151998);

	public static readonly HRESULT ERROR_BIZRULES_NOT_ENABLED = (HRESULT)(-1073151997);

	public static readonly HRESULT NTE_BAD_UID = (HRESULT)(-2146893823);

	public static readonly HRESULT NTE_BAD_HASH = (HRESULT)(-2146893822);

	public static readonly HRESULT NTE_BAD_KEY = (HRESULT)(-2146893821);

	public static readonly HRESULT NTE_BAD_LEN = (HRESULT)(-2146893820);

	public static readonly HRESULT NTE_BAD_DATA = (HRESULT)(-2146893819);

	public static readonly HRESULT NTE_BAD_SIGNATURE = (HRESULT)(-2146893818);

	public static readonly HRESULT NTE_BAD_VER = (HRESULT)(-2146893817);

	public static readonly HRESULT NTE_BAD_ALGID = (HRESULT)(-2146893816);

	public static readonly HRESULT NTE_BAD_FLAGS = (HRESULT)(-2146893815);

	public static readonly HRESULT NTE_BAD_TYPE = (HRESULT)(-2146893814);

	public static readonly HRESULT NTE_BAD_KEY_STATE = (HRESULT)(-2146893813);

	public static readonly HRESULT NTE_BAD_HASH_STATE = (HRESULT)(-2146893812);

	public static readonly HRESULT NTE_NO_KEY = (HRESULT)(-2146893811);

	public static readonly HRESULT NTE_NO_MEMORY = (HRESULT)(-2146893810);

	public static readonly HRESULT NTE_EXISTS = (HRESULT)(-2146893809);

	public static readonly HRESULT NTE_PERM = (HRESULT)(-2146893808);

	public static readonly HRESULT NTE_NOT_FOUND = (HRESULT)(-2146893807);

	public static readonly HRESULT NTE_DOUBLE_ENCRYPT = (HRESULT)(-2146893806);

	public static readonly HRESULT NTE_BAD_PROVIDER = (HRESULT)(-2146893805);

	public static readonly HRESULT NTE_BAD_PROV_TYPE = (HRESULT)(-2146893804);

	public static readonly HRESULT NTE_BAD_PUBLIC_KEY = (HRESULT)(-2146893803);

	public static readonly HRESULT NTE_BAD_KEYSET = (HRESULT)(-2146893802);

	public static readonly HRESULT NTE_PROV_TYPE_NOT_DEF = (HRESULT)(-2146893801);

	public static readonly HRESULT NTE_PROV_TYPE_ENTRY_BAD = (HRESULT)(-2146893800);

	public static readonly HRESULT NTE_KEYSET_NOT_DEF = (HRESULT)(-2146893799);

	public static readonly HRESULT NTE_KEYSET_ENTRY_BAD = (HRESULT)(-2146893798);

	public static readonly HRESULT NTE_PROV_TYPE_NO_MATCH = (HRESULT)(-2146893797);

	public static readonly HRESULT NTE_SIGNATURE_FILE_BAD = (HRESULT)(-2146893796);

	public static readonly HRESULT NTE_PROVIDER_DLL_FAIL = (HRESULT)(-2146893795);

	public static readonly HRESULT NTE_PROV_DLL_NOT_FOUND = (HRESULT)(-2146893794);

	public static readonly HRESULT NTE_BAD_KEYSET_PARAM = (HRESULT)(-2146893793);

	public static readonly HRESULT NTE_FAIL = (HRESULT)(-2146893792);

	public static readonly HRESULT NTE_SYS_ERR = (HRESULT)(-2146893791);

	public static readonly HRESULT NTE_SILENT_CONTEXT = (HRESULT)(-2146893790);

	public static readonly HRESULT NTE_TOKEN_KEYSET_STORAGE_FULL = (HRESULT)(-2146893789);

	public static readonly HRESULT NTE_TEMPORARY_PROFILE = (HRESULT)(-2146893788);

	public static readonly HRESULT NTE_FIXEDPARAMETER = (HRESULT)(-2146893787);

	public static readonly HRESULT NTE_INVALID_HANDLE = (HRESULT)(-2146893786);

	public static readonly HRESULT NTE_INVALID_PARAMETER = (HRESULT)(-2146893785);

	public static readonly HRESULT NTE_BUFFER_TOO_SMALL = (HRESULT)(-2146893784);

	public static readonly HRESULT NTE_NOT_SUPPORTED = (HRESULT)(-2146893783);

	public static readonly HRESULT NTE_NO_MORE_ITEMS = (HRESULT)(-2146893782);

	public static readonly HRESULT NTE_BUFFERS_OVERLAP = (HRESULT)(-2146893781);

	public static readonly HRESULT NTE_DECRYPTION_FAILURE = (HRESULT)(-2146893780);

	public static readonly HRESULT NTE_INTERNAL_ERROR = (HRESULT)(-2146893779);

	public static readonly HRESULT NTE_UI_REQUIRED = (HRESULT)(-2146893778);

	public static readonly HRESULT NTE_HMAC_NOT_SUPPORTED = (HRESULT)(-2146893777);

	public static readonly HRESULT NTE_DEVICE_NOT_READY = (HRESULT)(-2146893776);

	public static readonly HRESULT NTE_AUTHENTICATION_IGNORED = (HRESULT)(-2146893775);

	public static readonly HRESULT NTE_VALIDATION_FAILED = (HRESULT)(-2146893774);

	public static readonly HRESULT NTE_INCORRECT_PASSWORD = (HRESULT)(-2146893773);

	public static readonly HRESULT NTE_ENCRYPTION_FAILURE = (HRESULT)(-2146893772);

	public static readonly HRESULT NTE_DEVICE_NOT_FOUND = (HRESULT)(-2146893771);

	public static readonly HRESULT NTE_USER_CANCELLED = (HRESULT)(-2146893770);

	public static readonly HRESULT NTE_PASSWORD_CHANGE_REQUIRED = (HRESULT)(-2146893769);

	public static readonly HRESULT NTE_NOT_ACTIVE_CONSOLE = (HRESULT)(-2146893768);

	public static readonly HRESULT SEC_E_INSUFFICIENT_MEMORY = (HRESULT)(-2146893056);

	public static readonly HRESULT SEC_E_INVALID_HANDLE = (HRESULT)(-2146893055);

	public static readonly HRESULT SEC_E_UNSUPPORTED_FUNCTION = (HRESULT)(-2146893054);

	public static readonly HRESULT SEC_E_TARGET_UNKNOWN = (HRESULT)(-2146893053);

	public static readonly HRESULT SEC_E_INTERNAL_ERROR = (HRESULT)(-2146893052);

	public static readonly HRESULT SEC_E_SECPKG_NOT_FOUND = (HRESULT)(-2146893051);

	public static readonly HRESULT SEC_E_NOT_OWNER = (HRESULT)(-2146893050);

	public static readonly HRESULT SEC_E_CANNOT_INSTALL = (HRESULT)(-2146893049);

	public static readonly HRESULT SEC_E_INVALID_TOKEN = (HRESULT)(-2146893048);

	public static readonly HRESULT SEC_E_CANNOT_PACK = (HRESULT)(-2146893047);

	public static readonly HRESULT SEC_E_QOP_NOT_SUPPORTED = (HRESULT)(-2146893046);

	public static readonly HRESULT SEC_E_NO_IMPERSONATION = (HRESULT)(-2146893045);

	public static readonly HRESULT SEC_E_LOGON_DENIED = (HRESULT)(-2146893044);

	public static readonly HRESULT SEC_E_UNKNOWN_CREDENTIALS = (HRESULT)(-2146893043);

	public static readonly HRESULT SEC_E_NO_CREDENTIALS = (HRESULT)(-2146893042);

	public static readonly HRESULT SEC_E_MESSAGE_ALTERED = (HRESULT)(-2146893041);

	public static readonly HRESULT SEC_E_OUT_OF_SEQUENCE = (HRESULT)(-2146893040);

	public static readonly HRESULT SEC_E_NO_AUTHENTICATING_AUTHORITY = (HRESULT)(-2146893039);

	public static readonly HRESULT SEC_I_CONTINUE_NEEDED = (HRESULT)(590610);

	public static readonly HRESULT SEC_I_COMPLETE_NEEDED = (HRESULT)(590611);

	public static readonly HRESULT SEC_I_COMPLETE_AND_CONTINUE = (HRESULT)(590612);

	public static readonly HRESULT SEC_I_LOCAL_LOGON = (HRESULT)(590613);

	public static readonly HRESULT SEC_I_GENERIC_EXTENSION_RECEIVED = (HRESULT)(590614);

	public static readonly HRESULT SEC_E_BAD_PKGID = (HRESULT)(-2146893034);

	public static readonly HRESULT SEC_E_CONTEXT_EXPIRED = (HRESULT)(-2146893033);

	public static readonly HRESULT SEC_I_CONTEXT_EXPIRED = (HRESULT)(590615);

	public static readonly HRESULT SEC_E_INCOMPLETE_MESSAGE = (HRESULT)(-2146893032);

	public static readonly HRESULT SEC_E_INCOMPLETE_CREDENTIALS = (HRESULT)(-2146893024);

	public static readonly HRESULT SEC_E_BUFFER_TOO_SMALL = (HRESULT)(-2146893023);

	public static readonly HRESULT SEC_I_INCOMPLETE_CREDENTIALS = (HRESULT)(590624);

	public static readonly HRESULT SEC_I_RENEGOTIATE = (HRESULT)(590625);

	public static readonly HRESULT SEC_E_WRONG_PRINCIPAL = (HRESULT)(-2146893022);

	public static readonly HRESULT SEC_I_NO_LSA_CONTEXT = (HRESULT)(590627);

	public static readonly HRESULT SEC_E_TIME_SKEW = (HRESULT)(-2146893020);

	public static readonly HRESULT SEC_E_UNTRUSTED_ROOT = (HRESULT)(-2146893019);

	public static readonly HRESULT SEC_E_ILLEGAL_MESSAGE = (HRESULT)(-2146893018);

	public static readonly HRESULT SEC_E_CERT_UNKNOWN = (HRESULT)(-2146893017);

	public static readonly HRESULT SEC_E_CERT_EXPIRED = (HRESULT)(-2146893016);

	public static readonly HRESULT SEC_E_ENCRYPT_FAILURE = (HRESULT)(-2146893015);

	public static readonly HRESULT SEC_E_DECRYPT_FAILURE = (HRESULT)(-2146893008);

	public static readonly HRESULT SEC_E_ALGORITHM_MISMATCH = (HRESULT)(-2146893007);

	public static readonly HRESULT SEC_E_SECURITY_QOS_FAILED = (HRESULT)(-2146893006);

	public static readonly HRESULT SEC_E_UNFINISHED_CONTEXT_DELETED = (HRESULT)(-2146893005);

	public static readonly HRESULT SEC_E_NO_TGT_REPLY = (HRESULT)(-2146893004);

	public static readonly HRESULT SEC_E_NO_IP_ADDRESSES = (HRESULT)(-2146893003);

	public static readonly HRESULT SEC_E_WRONG_CREDENTIAL_HANDLE = (HRESULT)(-2146893002);

	public static readonly HRESULT SEC_E_CRYPTO_SYSTEM_INVALID = (HRESULT)(-2146893001);

	public static readonly HRESULT SEC_E_MAX_REFERRALS_EXCEEDED = (HRESULT)(-2146893000);

	public static readonly HRESULT SEC_E_MUST_BE_KDC = (HRESULT)(-2146892999);

	public static readonly HRESULT SEC_E_STRONG_CRYPTO_NOT_SUPPORTED = (HRESULT)(-2146892998);

	public static readonly HRESULT SEC_E_TOO_MANY_PRINCIPALS = (HRESULT)(-2146892997);

	public static readonly HRESULT SEC_E_NO_PA_DATA = (HRESULT)(-2146892996);

	public static readonly HRESULT SEC_E_PKINIT_NAME_MISMATCH = (HRESULT)(-2146892995);

	public static readonly HRESULT SEC_E_SMARTCARD_LOGON_REQUIRED = (HRESULT)(-2146892994);

	public static readonly HRESULT SEC_E_SHUTDOWN_IN_PROGRESS = (HRESULT)(-2146892993);

	public static readonly HRESULT SEC_E_KDC_INVALID_REQUEST = (HRESULT)(-2146892992);

	public static readonly HRESULT SEC_E_KDC_UNABLE_TO_REFER = (HRESULT)(-2146892991);

	public static readonly HRESULT SEC_E_KDC_UNKNOWN_ETYPE = (HRESULT)(-2146892990);

	public static readonly HRESULT SEC_E_UNSUPPORTED_PREAUTH = (HRESULT)(-2146892989);

	public static readonly HRESULT SEC_E_DELEGATION_REQUIRED = (HRESULT)(-2146892987);

	public static readonly HRESULT SEC_E_BAD_BINDINGS = (HRESULT)(-2146892986);

	public static readonly HRESULT SEC_E_MULTIPLE_ACCOUNTS = (HRESULT)(-2146892985);

	public static readonly HRESULT SEC_E_NO_KERB_KEY = (HRESULT)(-2146892984);

	public static readonly HRESULT SEC_E_CERT_WRONG_USAGE = (HRESULT)(-2146892983);

	public static readonly HRESULT SEC_E_DOWNGRADE_DETECTED = (HRESULT)(-2146892976);

	public static readonly HRESULT SEC_E_SMARTCARD_CERT_REVOKED = (HRESULT)(-2146892975);

	public static readonly HRESULT SEC_E_ISSUING_CA_UNTRUSTED = (HRESULT)(-2146892974);

	public static readonly HRESULT SEC_E_REVOCATION_OFFLINE_C = (HRESULT)(-2146892973);

	public static readonly HRESULT SEC_E_PKINIT_CLIENT_FAILURE = (HRESULT)(-2146892972);

	public static readonly HRESULT SEC_E_SMARTCARD_CERT_EXPIRED = (HRESULT)(-2146892971);

	public static readonly HRESULT SEC_E_NO_S4U_PROT_SUPPORT = (HRESULT)(-2146892970);

	public static readonly HRESULT SEC_E_CROSSREALM_DELEGATION_FAILURE = (HRESULT)(-2146892969);

	public static readonly HRESULT SEC_E_REVOCATION_OFFLINE_KDC = (HRESULT)(-2146892968);

	public static readonly HRESULT SEC_E_ISSUING_CA_UNTRUSTED_KDC = (HRESULT)(-2146892967);

	public static readonly HRESULT SEC_E_KDC_CERT_EXPIRED = (HRESULT)(-2146892966);

	public static readonly HRESULT SEC_E_KDC_CERT_REVOKED = (HRESULT)(-2146892965);

	public static readonly HRESULT SEC_I_SIGNATURE_NEEDED = (HRESULT)(590684);

	public static readonly HRESULT SEC_E_INVALID_PARAMETER = (HRESULT)(-2146892963);

	public static readonly HRESULT SEC_E_DELEGATION_POLICY = (HRESULT)(-2146892962);

	public static readonly HRESULT SEC_E_POLICY_NLTM_ONLY = (HRESULT)(-2146892961);

	public static readonly HRESULT SEC_I_NO_RENEGOTIATION = (HRESULT)(590688);

	public static readonly HRESULT SEC_E_NO_CONTEXT = (HRESULT)(-2146892959);

	public static readonly HRESULT SEC_E_PKU2U_CERT_FAILURE = (HRESULT)(-2146892958);

	public static readonly HRESULT SEC_E_MUTUAL_AUTH_FAILED = (HRESULT)(-2146892957);

	public static readonly HRESULT SEC_I_MESSAGE_FRAGMENT = (HRESULT)(590692);

	public static readonly HRESULT SEC_E_ONLY_HTTPS_ALLOWED = (HRESULT)(-2146892955);

	public static readonly HRESULT SEC_I_CONTINUE_NEEDED_MESSAGE_OK = (HRESULT)(590694);

	public static readonly HRESULT SEC_E_APPLICATION_PROTOCOL_MISMATCH = (HRESULT)(-2146892953);

	public static readonly HRESULT SEC_I_ASYNC_CALL_PENDING = (HRESULT)(590696);

	public static readonly HRESULT SEC_E_INVALID_UPN_NAME = (HRESULT)(-2146892951);

	public static readonly HRESULT SEC_E_EXT_BUFFER_TOO_SMALL = (HRESULT)(-2146892950);

	public static readonly HRESULT SEC_E_INSUFFICIENT_BUFFERS = (HRESULT)(-2146892949);

	public static readonly HRESULT CRYPT_E_MSG_ERROR = (HRESULT)(-2146889727);

	public static readonly HRESULT CRYPT_E_UNKNOWN_ALGO = (HRESULT)(-2146889726);

	public static readonly HRESULT CRYPT_E_OID_FORMAT = (HRESULT)(-2146889725);

	public static readonly HRESULT CRYPT_E_INVALID_MSG_TYPE = (HRESULT)(-2146889724);

	public static readonly HRESULT CRYPT_E_UNEXPECTED_ENCODING = (HRESULT)(-2146889723);

	public static readonly HRESULT CRYPT_E_AUTH_ATTR_MISSING = (HRESULT)(-2146889722);

	public static readonly HRESULT CRYPT_E_HASH_VALUE = (HRESULT)(-2146889721);

	public static readonly HRESULT CRYPT_E_INVALID_INDEX = (HRESULT)(-2146889720);

	public static readonly HRESULT CRYPT_E_ALREADY_DECRYPTED = (HRESULT)(-2146889719);

	public static readonly HRESULT CRYPT_E_NOT_DECRYPTED = (HRESULT)(-2146889718);

	public static readonly HRESULT CRYPT_E_RECIPIENT_NOT_FOUND = (HRESULT)(-2146889717);

	public static readonly HRESULT CRYPT_E_CONTROL_TYPE = (HRESULT)(-2146889716);

	public static readonly HRESULT CRYPT_E_ISSUER_SERIALNUMBER = (HRESULT)(-2146889715);

	public static readonly HRESULT CRYPT_E_SIGNER_NOT_FOUND = (HRESULT)(-2146889714);

	public static readonly HRESULT CRYPT_E_ATTRIBUTES_MISSING = (HRESULT)(-2146889713);

	public static readonly HRESULT CRYPT_E_STREAM_MSG_NOT_READY = (HRESULT)(-2146889712);

	public static readonly HRESULT CRYPT_E_STREAM_INSUFFICIENT_DATA = (HRESULT)(-2146889711);

	public static readonly HRESULT CRYPT_I_NEW_PROTECTION_REQUIRED = (HRESULT)(593938);

	public static readonly HRESULT CRYPT_E_BAD_LEN = (HRESULT)(-2146885631);

	public static readonly HRESULT CRYPT_E_BAD_ENCODE = (HRESULT)(-2146885630);

	public static readonly HRESULT CRYPT_E_FILE_ERROR = (HRESULT)(-2146885629);

	public static readonly HRESULT CRYPT_E_NOT_FOUND = (HRESULT)(-2146885628);

	public static readonly HRESULT CRYPT_E_EXISTS = (HRESULT)(-2146885627);

	public static readonly HRESULT CRYPT_E_NO_PROVIDER = (HRESULT)(-2146885626);

	public static readonly HRESULT CRYPT_E_SELF_SIGNED = (HRESULT)(-2146885625);

	public static readonly HRESULT CRYPT_E_DELETED_PREV = (HRESULT)(-2146885624);

	public static readonly HRESULT CRYPT_E_NO_MATCH = (HRESULT)(-2146885623);

	public static readonly HRESULT CRYPT_E_UNEXPECTED_MSG_TYPE = (HRESULT)(-2146885622);

	public static readonly HRESULT CRYPT_E_NO_KEY_PROPERTY = (HRESULT)(-2146885621);

	public static readonly HRESULT CRYPT_E_NO_DECRYPT_CERT = (HRESULT)(-2146885620);

	public static readonly HRESULT CRYPT_E_BAD_MSG = (HRESULT)(-2146885619);

	public static readonly HRESULT CRYPT_E_NO_SIGNER = (HRESULT)(-2146885618);

	public static readonly HRESULT CRYPT_E_PENDING_CLOSE = (HRESULT)(-2146885617);

	public static readonly HRESULT CRYPT_E_REVOKED = (HRESULT)(-2146885616);

	public static readonly HRESULT CRYPT_E_NO_REVOCATION_DLL = (HRESULT)(-2146885615);

	public static readonly HRESULT CRYPT_E_NO_REVOCATION_CHECK = (HRESULT)(-2146885614);

	public static readonly HRESULT CRYPT_E_REVOCATION_OFFLINE = (HRESULT)(-2146885613);

	public static readonly HRESULT CRYPT_E_NOT_IN_REVOCATION_DATABASE = (HRESULT)(-2146885612);

	public static readonly HRESULT CRYPT_E_INVALID_NUMERIC_STRING = (HRESULT)(-2146885600);

	public static readonly HRESULT CRYPT_E_INVALID_PRINTABLE_STRING = (HRESULT)(-2146885599);

	public static readonly HRESULT CRYPT_E_INVALID_IA5_STRING = (HRESULT)(-2146885598);

	public static readonly HRESULT CRYPT_E_INVALID_X500_STRING = (HRESULT)(-2146885597);

	public static readonly HRESULT CRYPT_E_NOT_CHAR_STRING = (HRESULT)(-2146885596);

	public static readonly HRESULT CRYPT_E_FILERESIZED = (HRESULT)(-2146885595);

	public static readonly HRESULT CRYPT_E_SECURITY_SETTINGS = (HRESULT)(-2146885594);

	public static readonly HRESULT CRYPT_E_NO_VERIFY_USAGE_DLL = (HRESULT)(-2146885593);

	public static readonly HRESULT CRYPT_E_NO_VERIFY_USAGE_CHECK = (HRESULT)(-2146885592);

	public static readonly HRESULT CRYPT_E_VERIFY_USAGE_OFFLINE = (HRESULT)(-2146885591);

	public static readonly HRESULT CRYPT_E_NOT_IN_CTL = (HRESULT)(-2146885590);

	public static readonly HRESULT CRYPT_E_NO_TRUSTED_SIGNER = (HRESULT)(-2146885589);

	public static readonly HRESULT CRYPT_E_MISSING_PUBKEY_PARA = (HRESULT)(-2146885588);

	public static readonly HRESULT CRYPT_E_OBJECT_LOCATOR_OBJECT_NOT_FOUND = (HRESULT)(-2146885587);

	public static readonly HRESULT CRYPT_E_OSS_ERROR = (HRESULT)(-2146881536);

	public static readonly HRESULT OSS_MORE_BUF = (HRESULT)(-2146881535);

	public static readonly HRESULT OSS_NEGATIVE_UINTEGER = (HRESULT)(-2146881534);

	public static readonly HRESULT OSS_PDU_RANGE = (HRESULT)(-2146881533);

	public static readonly HRESULT OSS_MORE_INPUT = (HRESULT)(-2146881532);

	public static readonly HRESULT OSS_DATA_ERROR = (HRESULT)(-2146881531);

	public static readonly HRESULT OSS_BAD_ARG = (HRESULT)(-2146881530);

	public static readonly HRESULT OSS_BAD_VERSION = (HRESULT)(-2146881529);

	public static readonly HRESULT OSS_OUT_MEMORY = (HRESULT)(-2146881528);

	public static readonly HRESULT OSS_PDU_MISMATCH = (HRESULT)(-2146881527);

	public static readonly HRESULT OSS_LIMITED = (HRESULT)(-2146881526);

	public static readonly HRESULT OSS_BAD_PTR = (HRESULT)(-2146881525);

	public static readonly HRESULT OSS_BAD_TIME = (HRESULT)(-2146881524);

	public static readonly HRESULT OSS_INDEFINITE_NOT_SUPPORTED = (HRESULT)(-2146881523);

	public static readonly HRESULT OSS_MEM_ERROR = (HRESULT)(-2146881522);

	public static readonly HRESULT OSS_BAD_TABLE = (HRESULT)(-2146881521);

	public static readonly HRESULT OSS_TOO_LONG = (HRESULT)(-2146881520);

	public static readonly HRESULT OSS_CONSTRAINT_VIOLATED = (HRESULT)(-2146881519);

	public static readonly HRESULT OSS_FATAL_ERROR = (HRESULT)(-2146881518);

	public static readonly HRESULT OSS_ACCESS_SERIALIZATION_ERROR = (HRESULT)(-2146881517);

	public static readonly HRESULT OSS_NULL_TBL = (HRESULT)(-2146881516);

	public static readonly HRESULT OSS_NULL_FCN = (HRESULT)(-2146881515);

	public static readonly HRESULT OSS_BAD_ENCRULES = (HRESULT)(-2146881514);

	public static readonly HRESULT OSS_UNAVAIL_ENCRULES = (HRESULT)(-2146881513);

	public static readonly HRESULT OSS_CANT_OPEN_TRACE_WINDOW = (HRESULT)(-2146881512);

	public static readonly HRESULT OSS_UNIMPLEMENTED = (HRESULT)(-2146881511);

	public static readonly HRESULT OSS_OID_DLL_NOT_LINKED = (HRESULT)(-2146881510);

	public static readonly HRESULT OSS_CANT_OPEN_TRACE_FILE = (HRESULT)(-2146881509);

	public static readonly HRESULT OSS_TRACE_FILE_ALREADY_OPEN = (HRESULT)(-2146881508);

	public static readonly HRESULT OSS_TABLE_MISMATCH = (HRESULT)(-2146881507);

	public static readonly HRESULT OSS_TYPE_NOT_SUPPORTED = (HRESULT)(-2146881506);

	public static readonly HRESULT OSS_REAL_DLL_NOT_LINKED = (HRESULT)(-2146881505);

	public static readonly HRESULT OSS_REAL_CODE_NOT_LINKED = (HRESULT)(-2146881504);

	public static readonly HRESULT OSS_OUT_OF_RANGE = (HRESULT)(-2146881503);

	public static readonly HRESULT OSS_COPIER_DLL_NOT_LINKED = (HRESULT)(-2146881502);

	public static readonly HRESULT OSS_CONSTRAINT_DLL_NOT_LINKED = (HRESULT)(-2146881501);

	public static readonly HRESULT OSS_COMPARATOR_DLL_NOT_LINKED = (HRESULT)(-2146881500);

	public static readonly HRESULT OSS_COMPARATOR_CODE_NOT_LINKED = (HRESULT)(-2146881499);

	public static readonly HRESULT OSS_MEM_MGR_DLL_NOT_LINKED = (HRESULT)(-2146881498);

	public static readonly HRESULT OSS_PDV_DLL_NOT_LINKED = (HRESULT)(-2146881497);

	public static readonly HRESULT OSS_PDV_CODE_NOT_LINKED = (HRESULT)(-2146881496);

	public static readonly HRESULT OSS_API_DLL_NOT_LINKED = (HRESULT)(-2146881495);

	public static readonly HRESULT OSS_BERDER_DLL_NOT_LINKED = (HRESULT)(-2146881494);

	public static readonly HRESULT OSS_PER_DLL_NOT_LINKED = (HRESULT)(-2146881493);

	public static readonly HRESULT OSS_OPEN_TYPE_ERROR = (HRESULT)(-2146881492);

	public static readonly HRESULT OSS_MUTEX_NOT_CREATED = (HRESULT)(-2146881491);

	public static readonly HRESULT OSS_CANT_CLOSE_TRACE_FILE = (HRESULT)(-2146881490);

	public static readonly HRESULT CRYPT_E_ASN1_ERROR = (HRESULT)(-2146881280);

	public static readonly HRESULT CRYPT_E_ASN1_INTERNAL = (HRESULT)(-2146881279);

	public static readonly HRESULT CRYPT_E_ASN1_EOD = (HRESULT)(-2146881278);

	public static readonly HRESULT CRYPT_E_ASN1_CORRUPT = (HRESULT)(-2146881277);

	public static readonly HRESULT CRYPT_E_ASN1_LARGE = (HRESULT)(-2146881276);

	public static readonly HRESULT CRYPT_E_ASN1_CONSTRAINT = (HRESULT)(-2146881275);

	public static readonly HRESULT CRYPT_E_ASN1_MEMORY = (HRESULT)(-2146881274);

	public static readonly HRESULT CRYPT_E_ASN1_OVERFLOW = (HRESULT)(-2146881273);

	public static readonly HRESULT CRYPT_E_ASN1_BADPDU = (HRESULT)(-2146881272);

	public static readonly HRESULT CRYPT_E_ASN1_BADARGS = (HRESULT)(-2146881271);

	public static readonly HRESULT CRYPT_E_ASN1_BADREAL = (HRESULT)(-2146881270);

	public static readonly HRESULT CRYPT_E_ASN1_BADTAG = (HRESULT)(-2146881269);

	public static readonly HRESULT CRYPT_E_ASN1_CHOICE = (HRESULT)(-2146881268);

	public static readonly HRESULT CRYPT_E_ASN1_RULE = (HRESULT)(-2146881267);

	public static readonly HRESULT CRYPT_E_ASN1_UTF8 = (HRESULT)(-2146881266);

	public static readonly HRESULT CRYPT_E_ASN1_PDU_TYPE = (HRESULT)(-2146881229);

	public static readonly HRESULT CRYPT_E_ASN1_NYI = (HRESULT)(-2146881228);

	public static readonly HRESULT CRYPT_E_ASN1_EXTENDED = (HRESULT)(-2146881023);

	public static readonly HRESULT CRYPT_E_ASN1_NOEOD = (HRESULT)(-2146881022);

	public static readonly HRESULT CERTSRV_E_BAD_REQUESTSUBJECT = (HRESULT)(-2146877439);

	public static readonly HRESULT CERTSRV_E_NO_REQUEST = (HRESULT)(-2146877438);

	public static readonly HRESULT CERTSRV_E_BAD_REQUESTSTATUS = (HRESULT)(-2146877437);

	public static readonly HRESULT CERTSRV_E_PROPERTY_EMPTY = (HRESULT)(-2146877436);

	public static readonly HRESULT CERTSRV_E_INVALID_CA_CERTIFICATE = (HRESULT)(-2146877435);

	public static readonly HRESULT CERTSRV_E_SERVER_SUSPENDED = (HRESULT)(-2146877434);

	public static readonly HRESULT CERTSRV_E_ENCODING_LENGTH = (HRESULT)(-2146877433);

	public static readonly HRESULT CERTSRV_E_ROLECONFLICT = (HRESULT)(-2146877432);

	public static readonly HRESULT CERTSRV_E_RESTRICTEDOFFICER = (HRESULT)(-2146877431);

	public static readonly HRESULT CERTSRV_E_KEY_ARCHIVAL_NOT_CONFIGURED = (HRESULT)(-2146877430);

	public static readonly HRESULT CERTSRV_E_NO_VALID_KRA = (HRESULT)(-2146877429);

	public static readonly HRESULT CERTSRV_E_BAD_REQUEST_KEY_ARCHIVAL = (HRESULT)(-2146877428);

	public static readonly HRESULT CERTSRV_E_NO_CAADMIN_DEFINED = (HRESULT)(-2146877427);

	public static readonly HRESULT CERTSRV_E_BAD_RENEWAL_CERT_ATTRIBUTE = (HRESULT)(-2146877426);

	public static readonly HRESULT CERTSRV_E_NO_DB_SESSIONS = (HRESULT)(-2146877425);

	public static readonly HRESULT CERTSRV_E_ALIGNMENT_FAULT = (HRESULT)(-2146877424);

	public static readonly HRESULT CERTSRV_E_ENROLL_DENIED = (HRESULT)(-2146877423);

	public static readonly HRESULT CERTSRV_E_TEMPLATE_DENIED = (HRESULT)(-2146877422);

	public static readonly HRESULT CERTSRV_E_DOWNLEVEL_DC_SSL_OR_UPGRADE = (HRESULT)(-2146877421);

	public static readonly HRESULT CERTSRV_E_ADMIN_DENIED_REQUEST = (HRESULT)(-2146877420);

	public static readonly HRESULT CERTSRV_E_NO_POLICY_SERVER = (HRESULT)(-2146877419);

	public static readonly HRESULT CERTSRV_E_WEAK_SIGNATURE_OR_KEY = (HRESULT)(-2146877418);

	public static readonly HRESULT CERTSRV_E_KEY_ATTESTATION_NOT_SUPPORTED = (HRESULT)(-2146877417);

	public static readonly HRESULT CERTSRV_E_ENCRYPTION_CERT_REQUIRED = (HRESULT)(-2146877416);

	public static readonly HRESULT CERTSRV_E_UNSUPPORTED_CERT_TYPE = (HRESULT)(-2146875392);

	public static readonly HRESULT CERTSRV_E_NO_CERT_TYPE = (HRESULT)(-2146875391);

	public static readonly HRESULT CERTSRV_E_TEMPLATE_CONFLICT = (HRESULT)(-2146875390);

	public static readonly HRESULT CERTSRV_E_SUBJECT_ALT_NAME_REQUIRED = (HRESULT)(-2146875389);

	public static readonly HRESULT CERTSRV_E_ARCHIVED_KEY_REQUIRED = (HRESULT)(-2146875388);

	public static readonly HRESULT CERTSRV_E_SMIME_REQUIRED = (HRESULT)(-2146875387);

	public static readonly HRESULT CERTSRV_E_BAD_RENEWAL_SUBJECT = (HRESULT)(-2146875386);

	public static readonly HRESULT CERTSRV_E_BAD_TEMPLATE_VERSION = (HRESULT)(-2146875385);

	public static readonly HRESULT CERTSRV_E_TEMPLATE_POLICY_REQUIRED = (HRESULT)(-2146875384);

	public static readonly HRESULT CERTSRV_E_SIGNATURE_POLICY_REQUIRED = (HRESULT)(-2146875383);

	public static readonly HRESULT CERTSRV_E_SIGNATURE_COUNT = (HRESULT)(-2146875382);

	public static readonly HRESULT CERTSRV_E_SIGNATURE_REJECTED = (HRESULT)(-2146875381);

	public static readonly HRESULT CERTSRV_E_ISSUANCE_POLICY_REQUIRED = (HRESULT)(-2146875380);

	public static readonly HRESULT CERTSRV_E_SUBJECT_UPN_REQUIRED = (HRESULT)(-2146875379);

	public static readonly HRESULT CERTSRV_E_SUBJECT_DIRECTORY_GUID_REQUIRED = (HRESULT)(-2146875378);

	public static readonly HRESULT CERTSRV_E_SUBJECT_DNS_REQUIRED = (HRESULT)(-2146875377);

	public static readonly HRESULT CERTSRV_E_ARCHIVED_KEY_UNEXPECTED = (HRESULT)(-2146875376);

	public static readonly HRESULT CERTSRV_E_KEY_LENGTH = (HRESULT)(-2146875375);

	public static readonly HRESULT CERTSRV_E_SUBJECT_EMAIL_REQUIRED = (HRESULT)(-2146875374);

	public static readonly HRESULT CERTSRV_E_UNKNOWN_CERT_TYPE = (HRESULT)(-2146875373);

	public static readonly HRESULT CERTSRV_E_CERT_TYPE_OVERLAP = (HRESULT)(-2146875372);

	public static readonly HRESULT CERTSRV_E_TOO_MANY_SIGNATURES = (HRESULT)(-2146875371);

	public static readonly HRESULT CERTSRV_E_RENEWAL_BAD_PUBLIC_KEY = (HRESULT)(-2146875370);

	public static readonly HRESULT CERTSRV_E_INVALID_EK = (HRESULT)(-2146875369);

	public static readonly HRESULT CERTSRV_E_INVALID_IDBINDING = (HRESULT)(-2146875368);

	public static readonly HRESULT CERTSRV_E_INVALID_ATTESTATION = (HRESULT)(-2146875367);

	public static readonly HRESULT CERTSRV_E_KEY_ATTESTATION = (HRESULT)(-2146875366);

	public static readonly HRESULT CERTSRV_E_CORRUPT_KEY_ATTESTATION = (HRESULT)(-2146875365);

	public static readonly HRESULT CERTSRV_E_EXPIRED_CHALLENGE = (HRESULT)(-2146875364);

	public static readonly HRESULT CERTSRV_E_INVALID_RESPONSE = (HRESULT)(-2146875363);

	public static readonly HRESULT CERTSRV_E_INVALID_REQUESTID = (HRESULT)(-2146875362);

	public static readonly HRESULT CERTSRV_E_REQUEST_PRECERTIFICATE_MISMATCH = (HRESULT)(-2146875361);

	public static readonly HRESULT CERTSRV_E_PENDING_CLIENT_RESPONSE = (HRESULT)(-2146875360);

	public static readonly HRESULT CERTSRV_E_SEC_EXT_DIRECTORY_SID_REQUIRED = (HRESULT)(-2146875359);

	public static readonly HRESULT XENROLL_E_KEY_NOT_EXPORTABLE = (HRESULT)(-2146873344);

	public static readonly HRESULT XENROLL_E_CANNOT_ADD_ROOT_CERT = (HRESULT)(-2146873343);

	public static readonly HRESULT XENROLL_E_RESPONSE_KA_HASH_NOT_FOUND = (HRESULT)(-2146873342);

	public static readonly HRESULT XENROLL_E_RESPONSE_UNEXPECTED_KA_HASH = (HRESULT)(-2146873341);

	public static readonly HRESULT XENROLL_E_RESPONSE_KA_HASH_MISMATCH = (HRESULT)(-2146873340);

	public static readonly HRESULT XENROLL_E_KEYSPEC_SMIME_MISMATCH = (HRESULT)(-2146873339);

	public static readonly HRESULT TRUST_E_SYSTEM_ERROR = (HRESULT)(-2146869247);

	public static readonly HRESULT TRUST_E_NO_SIGNER_CERT = (HRESULT)(-2146869246);

	public static readonly HRESULT TRUST_E_COUNTER_SIGNER = (HRESULT)(-2146869245);

	public static readonly HRESULT TRUST_E_CERT_SIGNATURE = (HRESULT)(-2146869244);

	public static readonly HRESULT TRUST_E_TIME_STAMP = (HRESULT)(-2146869243);

	public static readonly HRESULT TRUST_E_BAD_DIGEST = (HRESULT)(-2146869232);

	public static readonly HRESULT TRUST_E_MALFORMED_SIGNATURE = (HRESULT)(-2146869231);

	public static readonly HRESULT TRUST_E_BASIC_CONSTRAINTS = (HRESULT)(-2146869223);

	public static readonly HRESULT TRUST_E_FINANCIAL_CRITERIA = (HRESULT)(-2146869218);

	public static readonly HRESULT MSSIPOTF_E_OUTOFMEMRANGE = (HRESULT)(-2146865151);

	public static readonly HRESULT MSSIPOTF_E_CANTGETOBJECT = (HRESULT)(-2146865150);

	public static readonly HRESULT MSSIPOTF_E_NOHEADTABLE = (HRESULT)(-2146865149);

	public static readonly HRESULT MSSIPOTF_E_BAD_MAGICNUMBER = (HRESULT)(-2146865148);

	public static readonly HRESULT MSSIPOTF_E_BAD_OFFSET_TABLE = (HRESULT)(-2146865147);

	public static readonly HRESULT MSSIPOTF_E_TABLE_TAGORDER = (HRESULT)(-2146865146);

	public static readonly HRESULT MSSIPOTF_E_TABLE_LONGWORD = (HRESULT)(-2146865145);

	public static readonly HRESULT MSSIPOTF_E_BAD_FIRST_TABLE_PLACEMENT = (HRESULT)(-2146865144);

	public static readonly HRESULT MSSIPOTF_E_TABLES_OVERLAP = (HRESULT)(-2146865143);

	public static readonly HRESULT MSSIPOTF_E_TABLE_PADBYTES = (HRESULT)(-2146865142);

	public static readonly HRESULT MSSIPOTF_E_FILETOOSMALL = (HRESULT)(-2146865141);

	public static readonly HRESULT MSSIPOTF_E_TABLE_CHECKSUM = (HRESULT)(-2146865140);

	public static readonly HRESULT MSSIPOTF_E_FILE_CHECKSUM = (HRESULT)(-2146865139);

	public static readonly HRESULT MSSIPOTF_E_FAILED_POLICY = (HRESULT)(-2146865136);

	public static readonly HRESULT MSSIPOTF_E_FAILED_HINTS_CHECK = (HRESULT)(-2146865135);

	public static readonly HRESULT MSSIPOTF_E_NOT_OPENTYPE = (HRESULT)(-2146865134);

	public static readonly HRESULT MSSIPOTF_E_FILE = (HRESULT)(-2146865133);

	public static readonly HRESULT MSSIPOTF_E_CRYPT = (HRESULT)(-2146865132);

	public static readonly HRESULT MSSIPOTF_E_BADVERSION = (HRESULT)(-2146865131);

	public static readonly HRESULT MSSIPOTF_E_DSIG_STRUCTURE = (HRESULT)(-2146865130);

	public static readonly HRESULT MSSIPOTF_E_PCONST_CHECK = (HRESULT)(-2146865129);

	public static readonly HRESULT MSSIPOTF_E_STRUCTURE = (HRESULT)(-2146865128);

	public static readonly HRESULT ERROR_CRED_REQUIRES_CONFIRMATION = (HRESULT)(-2146865127);

	public static readonly HRESULT TRUST_E_PROVIDER_UNKNOWN = (HRESULT)(-2146762751);

	public static readonly HRESULT TRUST_E_ACTION_UNKNOWN = (HRESULT)(-2146762750);

	public static readonly HRESULT TRUST_E_SUBJECT_FORM_UNKNOWN = (HRESULT)(-2146762749);

	public static readonly HRESULT TRUST_E_SUBJECT_NOT_TRUSTED = (HRESULT)(-2146762748);

	public static readonly HRESULT DIGSIG_E_ENCODE = (HRESULT)(-2146762747);

	public static readonly HRESULT DIGSIG_E_DECODE = (HRESULT)(-2146762746);

	public static readonly HRESULT DIGSIG_E_EXTENSIBILITY = (HRESULT)(-2146762745);

	public static readonly HRESULT DIGSIG_E_CRYPTO = (HRESULT)(-2146762744);

	public static readonly HRESULT PERSIST_E_SIZEDEFINITE = (HRESULT)(-2146762743);

	public static readonly HRESULT PERSIST_E_SIZEINDEFINITE = (HRESULT)(-2146762742);

	public static readonly HRESULT PERSIST_E_NOTSELFSIZING = (HRESULT)(-2146762741);

	public static readonly HRESULT TRUST_E_NOSIGNATURE = (HRESULT)(-2146762496);

	public static readonly HRESULT CERT_E_EXPIRED = (HRESULT)(-2146762495);

	public static readonly HRESULT CERT_E_VALIDITYPERIODNESTING = (HRESULT)(-2146762494);

	public static readonly HRESULT CERT_E_ROLE = (HRESULT)(-2146762493);

	public static readonly HRESULT CERT_E_PATHLENCONST = (HRESULT)(-2146762492);

	public static readonly HRESULT CERT_E_CRITICAL = (HRESULT)(-2146762491);

	public static readonly HRESULT CERT_E_PURPOSE = (HRESULT)(-2146762490);

	public static readonly HRESULT CERT_E_ISSUERCHAINING = (HRESULT)(-2146762489);

	public static readonly HRESULT CERT_E_MALFORMED = (HRESULT)(-2146762488);

	public static readonly HRESULT CERT_E_UNTRUSTEDROOT = (HRESULT)(-2146762487);

	public static readonly HRESULT CERT_E_CHAINING = (HRESULT)(-2146762486);

	public static readonly HRESULT TRUST_E_FAIL = (HRESULT)(-2146762485);

	public static readonly HRESULT CERT_E_REVOKED = (HRESULT)(-2146762484);

	public static readonly HRESULT CERT_E_UNTRUSTEDTESTROOT = (HRESULT)(-2146762483);

	public static readonly HRESULT CERT_E_REVOCATION_FAILURE = (HRESULT)(-2146762482);

	public static readonly HRESULT CERT_E_CN_NO_MATCH = (HRESULT)(-2146762481);

	public static readonly HRESULT CERT_E_WRONG_USAGE = (HRESULT)(-2146762480);

	public static readonly HRESULT TRUST_E_EXPLICIT_DISTRUST = (HRESULT)(-2146762479);

	public static readonly HRESULT CERT_E_UNTRUSTEDCA = (HRESULT)(-2146762478);

	public static readonly HRESULT CERT_E_INVALID_POLICY = (HRESULT)(-2146762477);

	public static readonly HRESULT CERT_E_INVALID_NAME = (HRESULT)(-2146762476);

	public static readonly HRESULT SPAPI_E_EXPECTED_SECTION_NAME = (HRESULT)(-2146500608);

	public static readonly HRESULT SPAPI_E_BAD_SECTION_NAME_LINE = (HRESULT)(-2146500607);

	public static readonly HRESULT SPAPI_E_SECTION_NAME_TOO_LONG = (HRESULT)(-2146500606);

	public static readonly HRESULT SPAPI_E_GENERAL_SYNTAX = (HRESULT)(-2146500605);

	public static readonly HRESULT SPAPI_E_WRONG_INF_STYLE = (HRESULT)(-2146500352);

	public static readonly HRESULT SPAPI_E_SECTION_NOT_FOUND = (HRESULT)(-2146500351);

	public static readonly HRESULT SPAPI_E_LINE_NOT_FOUND = (HRESULT)(-2146500350);

	public static readonly HRESULT SPAPI_E_NO_BACKUP = (HRESULT)(-2146500349);

	public static readonly HRESULT SPAPI_E_NO_ASSOCIATED_CLASS = (HRESULT)(-2146500096);

	public static readonly HRESULT SPAPI_E_CLASS_MISMATCH = (HRESULT)(-2146500095);

	public static readonly HRESULT SPAPI_E_DUPLICATE_FOUND = (HRESULT)(-2146500094);

	public static readonly HRESULT SPAPI_E_NO_DRIVER_SELECTED = (HRESULT)(-2146500093);

	public static readonly HRESULT SPAPI_E_KEY_DOES_NOT_EXIST = (HRESULT)(-2146500092);

	public static readonly HRESULT SPAPI_E_INVALID_DEVINST_NAME = (HRESULT)(-2146500091);

	public static readonly HRESULT SPAPI_E_INVALID_CLASS = (HRESULT)(-2146500090);

	public static readonly HRESULT SPAPI_E_DEVINST_ALREADY_EXISTS = (HRESULT)(-2146500089);

	public static readonly HRESULT SPAPI_E_DEVINFO_NOT_REGISTERED = (HRESULT)(-2146500088);

	public static readonly HRESULT SPAPI_E_INVALID_REG_PROPERTY = (HRESULT)(-2146500087);

	public static readonly HRESULT SPAPI_E_NO_INF = (HRESULT)(-2146500086);

	public static readonly HRESULT SPAPI_E_NO_SUCH_DEVINST = (HRESULT)(-2146500085);

	public static readonly HRESULT SPAPI_E_CANT_LOAD_CLASS_ICON = (HRESULT)(-2146500084);

	public static readonly HRESULT SPAPI_E_INVALID_CLASS_INSTALLER = (HRESULT)(-2146500083);

	public static readonly HRESULT SPAPI_E_DI_DO_DEFAULT = (HRESULT)(-2146500082);

	public static readonly HRESULT SPAPI_E_DI_NOFILECOPY = (HRESULT)(-2146500081);

	public static readonly HRESULT SPAPI_E_INVALID_HWPROFILE = (HRESULT)(-2146500080);

	public static readonly HRESULT SPAPI_E_NO_DEVICE_SELECTED = (HRESULT)(-2146500079);

	public static readonly HRESULT SPAPI_E_DEVINFO_LIST_LOCKED = (HRESULT)(-2146500078);

	public static readonly HRESULT SPAPI_E_DEVINFO_DATA_LOCKED = (HRESULT)(-2146500077);

	public static readonly HRESULT SPAPI_E_DI_BAD_PATH = (HRESULT)(-2146500076);

	public static readonly HRESULT SPAPI_E_NO_CLASSINSTALL_PARAMS = (HRESULT)(-2146500075);

	public static readonly HRESULT SPAPI_E_FILEQUEUE_LOCKED = (HRESULT)(-2146500074);

	public static readonly HRESULT SPAPI_E_BAD_SERVICE_INSTALLSECT = (HRESULT)(-2146500073);

	public static readonly HRESULT SPAPI_E_NO_CLASS_DRIVER_LIST = (HRESULT)(-2146500072);

	public static readonly HRESULT SPAPI_E_NO_ASSOCIATED_SERVICE = (HRESULT)(-2146500071);

	public static readonly HRESULT SPAPI_E_NO_DEFAULT_DEVICE_INTERFACE = (HRESULT)(-2146500070);

	public static readonly HRESULT SPAPI_E_DEVICE_INTERFACE_ACTIVE = (HRESULT)(-2146500069);

	public static readonly HRESULT SPAPI_E_DEVICE_INTERFACE_REMOVED = (HRESULT)(-2146500068);

	public static readonly HRESULT SPAPI_E_BAD_INTERFACE_INSTALLSECT = (HRESULT)(-2146500067);

	public static readonly HRESULT SPAPI_E_NO_SUCH_INTERFACE_CLASS = (HRESULT)(-2146500066);

	public static readonly HRESULT SPAPI_E_INVALID_REFERENCE_STRING = (HRESULT)(-2146500065);

	public static readonly HRESULT SPAPI_E_INVALID_MACHINENAME = (HRESULT)(-2146500064);

	public static readonly HRESULT SPAPI_E_REMOTE_COMM_FAILURE = (HRESULT)(-2146500063);

	public static readonly HRESULT SPAPI_E_MACHINE_UNAVAILABLE = (HRESULT)(-2146500062);

	public static readonly HRESULT SPAPI_E_NO_CONFIGMGR_SERVICES = (HRESULT)(-2146500061);

	public static readonly HRESULT SPAPI_E_INVALID_PROPPAGE_PROVIDER = (HRESULT)(-2146500060);

	public static readonly HRESULT SPAPI_E_NO_SUCH_DEVICE_INTERFACE = (HRESULT)(-2146500059);

	public static readonly HRESULT SPAPI_E_DI_POSTPROCESSING_REQUIRED = (HRESULT)(-2146500058);

	public static readonly HRESULT SPAPI_E_INVALID_COINSTALLER = (HRESULT)(-2146500057);

	public static readonly HRESULT SPAPI_E_NO_COMPAT_DRIVERS = (HRESULT)(-2146500056);

	public static readonly HRESULT SPAPI_E_NO_DEVICE_ICON = (HRESULT)(-2146500055);

	public static readonly HRESULT SPAPI_E_INVALID_INF_LOGCONFIG = (HRESULT)(-2146500054);

	public static readonly HRESULT SPAPI_E_DI_DONT_INSTALL = (HRESULT)(-2146500053);

	public static readonly HRESULT SPAPI_E_INVALID_FILTER_DRIVER = (HRESULT)(-2146500052);

	public static readonly HRESULT SPAPI_E_NON_WINDOWS_NT_DRIVER = (HRESULT)(-2146500051);

	public static readonly HRESULT SPAPI_E_NON_WINDOWS_DRIVER = (HRESULT)(-2146500050);

	public static readonly HRESULT SPAPI_E_NO_CATALOG_FOR_OEM_INF = (HRESULT)(-2146500049);

	public static readonly HRESULT SPAPI_E_DEVINSTALL_QUEUE_NONNATIVE = (HRESULT)(-2146500048);

	public static readonly HRESULT SPAPI_E_NOT_DISABLEABLE = (HRESULT)(-2146500047);

	public static readonly HRESULT SPAPI_E_CANT_REMOVE_DEVINST = (HRESULT)(-2146500046);

	public static readonly HRESULT SPAPI_E_INVALID_TARGET = (HRESULT)(-2146500045);

	public static readonly HRESULT SPAPI_E_DRIVER_NONNATIVE = (HRESULT)(-2146500044);

	public static readonly HRESULT SPAPI_E_IN_WOW64 = (HRESULT)(-2146500043);

	public static readonly HRESULT SPAPI_E_SET_SYSTEM_RESTORE_POINT = (HRESULT)(-2146500042);

	public static readonly HRESULT SPAPI_E_INCORRECTLY_COPIED_INF = (HRESULT)(-2146500041);

	public static readonly HRESULT SPAPI_E_SCE_DISABLED = (HRESULT)(-2146500040);

	public static readonly HRESULT SPAPI_E_UNKNOWN_EXCEPTION = (HRESULT)(-2146500039);

	public static readonly HRESULT SPAPI_E_PNP_REGISTRY_ERROR = (HRESULT)(-2146500038);

	public static readonly HRESULT SPAPI_E_REMOTE_REQUEST_UNSUPPORTED = (HRESULT)(-2146500037);

	public static readonly HRESULT SPAPI_E_NOT_AN_INSTALLED_OEM_INF = (HRESULT)(-2146500036);

	public static readonly HRESULT SPAPI_E_INF_IN_USE_BY_DEVICES = (HRESULT)(-2146500035);

	public static readonly HRESULT SPAPI_E_DI_FUNCTION_OBSOLETE = (HRESULT)(-2146500034);

	public static readonly HRESULT SPAPI_E_NO_AUTHENTICODE_CATALOG = (HRESULT)(-2146500033);

	public static readonly HRESULT SPAPI_E_AUTHENTICODE_DISALLOWED = (HRESULT)(-2146500032);

	public static readonly HRESULT SPAPI_E_AUTHENTICODE_TRUSTED_PUBLISHER = (HRESULT)(-2146500031);

	public static readonly HRESULT SPAPI_E_AUTHENTICODE_TRUST_NOT_ESTABLISHED = (HRESULT)(-2146500030);

	public static readonly HRESULT SPAPI_E_AUTHENTICODE_PUBLISHER_NOT_TRUSTED = (HRESULT)(-2146500029);

	public static readonly HRESULT SPAPI_E_SIGNATURE_OSATTRIBUTE_MISMATCH = (HRESULT)(-2146500028);

	public static readonly HRESULT SPAPI_E_ONLY_VALIDATE_VIA_AUTHENTICODE = (HRESULT)(-2146500027);

	public static readonly HRESULT SPAPI_E_DEVICE_INSTALLER_NOT_READY = (HRESULT)(-2146500026);

	public static readonly HRESULT SPAPI_E_DRIVER_STORE_ADD_FAILED = (HRESULT)(-2146500025);

	public static readonly HRESULT SPAPI_E_DEVICE_INSTALL_BLOCKED = (HRESULT)(-2146500024);

	public static readonly HRESULT SPAPI_E_DRIVER_INSTALL_BLOCKED = (HRESULT)(-2146500023);

	public static readonly HRESULT SPAPI_E_WRONG_INF_TYPE = (HRESULT)(-2146500022);

	public static readonly HRESULT SPAPI_E_FILE_HASH_NOT_IN_CATALOG = (HRESULT)(-2146500021);

	public static readonly HRESULT SPAPI_E_DRIVER_STORE_DELETE_FAILED = (HRESULT)(-2146500020);

	public static readonly HRESULT SPAPI_E_UNRECOVERABLE_STACK_OVERFLOW = (HRESULT)(-2146499840);

	public static readonly HRESULT SPAPI_E_ERROR_NOT_INSTALLED = (HRESULT)(-2146496512);

	public static readonly HRESULT SCARD_F_INTERNAL_ERROR = (HRESULT)(-2146435071);

	public static readonly HRESULT SCARD_E_CANCELLED = (HRESULT)(-2146435070);

	public static readonly HRESULT SCARD_E_INVALID_HANDLE = (HRESULT)(-2146435069);

	public static readonly HRESULT SCARD_E_INVALID_PARAMETER = (HRESULT)(-2146435068);

	public static readonly HRESULT SCARD_E_INVALID_TARGET = (HRESULT)(-2146435067);

	public static readonly HRESULT SCARD_E_NO_MEMORY = (HRESULT)(-2146435066);

	public static readonly HRESULT SCARD_F_WAITED_TOO_LONG = (HRESULT)(-2146435065);

	public static readonly HRESULT SCARD_E_INSUFFICIENT_BUFFER = (HRESULT)(-2146435064);

	public static readonly HRESULT SCARD_E_UNKNOWN_READER = (HRESULT)(-2146435063);

	public static readonly HRESULT SCARD_E_TIMEOUT = (HRESULT)(-2146435062);

	public static readonly HRESULT SCARD_E_SHARING_VIOLATION = (HRESULT)(-2146435061);

	public static readonly HRESULT SCARD_E_NO_SMARTCARD = (HRESULT)(-2146435060);

	public static readonly HRESULT SCARD_E_UNKNOWN_CARD = (HRESULT)(-2146435059);

	public static readonly HRESULT SCARD_E_CANT_DISPOSE = (HRESULT)(-2146435058);

	public static readonly HRESULT SCARD_E_PROTO_MISMATCH = (HRESULT)(-2146435057);

	public static readonly HRESULT SCARD_E_NOT_READY = (HRESULT)(-2146435056);

	public static readonly HRESULT SCARD_E_INVALID_VALUE = (HRESULT)(-2146435055);

	public static readonly HRESULT SCARD_E_SYSTEM_CANCELLED = (HRESULT)(-2146435054);

	public static readonly HRESULT SCARD_F_COMM_ERROR = (HRESULT)(-2146435053);

	public static readonly HRESULT SCARD_F_UNKNOWN_ERROR = (HRESULT)(-2146435052);

	public static readonly HRESULT SCARD_E_INVALID_ATR = (HRESULT)(-2146435051);

	public static readonly HRESULT SCARD_E_NOT_TRANSACTED = (HRESULT)(-2146435050);

	public static readonly HRESULT SCARD_E_READER_UNAVAILABLE = (HRESULT)(-2146435049);

	public static readonly HRESULT SCARD_P_SHUTDOWN = (HRESULT)(-2146435048);

	public static readonly HRESULT SCARD_E_PCI_TOO_SMALL = (HRESULT)(-2146435047);

	public static readonly HRESULT SCARD_E_READER_UNSUPPORTED = (HRESULT)(-2146435046);

	public static readonly HRESULT SCARD_E_DUPLICATE_READER = (HRESULT)(-2146435045);

	public static readonly HRESULT SCARD_E_CARD_UNSUPPORTED = (HRESULT)(-2146435044);

	public static readonly HRESULT SCARD_E_NO_SERVICE = (HRESULT)(-2146435043);

	public static readonly HRESULT SCARD_E_SERVICE_STOPPED = (HRESULT)(-2146435042);

	public static readonly HRESULT SCARD_E_UNEXPECTED = (HRESULT)(-2146435041);

	public static readonly HRESULT SCARD_E_ICC_INSTALLATION = (HRESULT)(-2146435040);

	public static readonly HRESULT SCARD_E_ICC_CREATEORDER = (HRESULT)(-2146435039);

	public static readonly HRESULT SCARD_E_UNSUPPORTED_FEATURE = (HRESULT)(-2146435038);

	public static readonly HRESULT SCARD_E_DIR_NOT_FOUND = (HRESULT)(-2146435037);

	public static readonly HRESULT SCARD_E_FILE_NOT_FOUND = (HRESULT)(-2146435036);

	public static readonly HRESULT SCARD_E_NO_DIR = (HRESULT)(-2146435035);

	public static readonly HRESULT SCARD_E_NO_FILE = (HRESULT)(-2146435034);

	public static readonly HRESULT SCARD_E_NO_ACCESS = (HRESULT)(-2146435033);

	public static readonly HRESULT SCARD_E_WRITE_TOO_MANY = (HRESULT)(-2146435032);

	public static readonly HRESULT SCARD_E_BAD_SEEK = (HRESULT)(-2146435031);

	public static readonly HRESULT SCARD_E_INVALID_CHV = (HRESULT)(-2146435030);

	public static readonly HRESULT SCARD_E_UNKNOWN_RES_MNG = (HRESULT)(-2146435029);

	public static readonly HRESULT SCARD_E_NO_SUCH_CERTIFICATE = (HRESULT)(-2146435028);

	public static readonly HRESULT SCARD_E_CERTIFICATE_UNAVAILABLE = (HRESULT)(-2146435027);

	public static readonly HRESULT SCARD_E_NO_READERS_AVAILABLE = (HRESULT)(-2146435026);

	public static readonly HRESULT SCARD_E_COMM_DATA_LOST = (HRESULT)(-2146435025);

	public static readonly HRESULT SCARD_E_NO_KEY_CONTAINER = (HRESULT)(-2146435024);

	public static readonly HRESULT SCARD_E_SERVER_TOO_BUSY = (HRESULT)(-2146435023);

	public static readonly HRESULT SCARD_E_PIN_CACHE_EXPIRED = (HRESULT)(-2146435022);

	public static readonly HRESULT SCARD_E_NO_PIN_CACHE = (HRESULT)(-2146435021);

	public static readonly HRESULT SCARD_E_READ_ONLY_CARD = (HRESULT)(-2146435020);

	public static readonly HRESULT SCARD_W_UNSUPPORTED_CARD = (HRESULT)(-2146434971);

	public static readonly HRESULT SCARD_W_UNRESPONSIVE_CARD = (HRESULT)(-2146434970);

	public static readonly HRESULT SCARD_W_UNPOWERED_CARD = (HRESULT)(-2146434969);

	public static readonly HRESULT SCARD_W_RESET_CARD = (HRESULT)(-2146434968);

	public static readonly HRESULT SCARD_W_REMOVED_CARD = (HRESULT)(-2146434967);

	public static readonly HRESULT SCARD_W_SECURITY_VIOLATION = (HRESULT)(-2146434966);

	public static readonly HRESULT SCARD_W_WRONG_CHV = (HRESULT)(-2146434965);

	public static readonly HRESULT SCARD_W_CHV_BLOCKED = (HRESULT)(-2146434964);

	public static readonly HRESULT SCARD_W_EOF = (HRESULT)(-2146434963);

	public static readonly HRESULT SCARD_W_CANCELLED_BY_USER = (HRESULT)(-2146434962);

	public static readonly HRESULT SCARD_W_CARD_NOT_AUTHENTICATED = (HRESULT)(-2146434961);

	public static readonly HRESULT SCARD_W_CACHE_ITEM_NOT_FOUND = (HRESULT)(-2146434960);

	public static readonly HRESULT SCARD_W_CACHE_ITEM_STALE = (HRESULT)(-2146434959);

	public static readonly HRESULT SCARD_W_CACHE_ITEM_TOO_BIG = (HRESULT)(-2146434958);

	public static readonly HRESULT COMADMIN_E_OBJECTERRORS = (HRESULT)(-2146368511);

	public static readonly HRESULT COMADMIN_E_OBJECTINVALID = (HRESULT)(-2146368510);

	public static readonly HRESULT COMADMIN_E_KEYMISSING = (HRESULT)(-2146368509);

	public static readonly HRESULT COMADMIN_E_ALREADYINSTALLED = (HRESULT)(-2146368508);

	public static readonly HRESULT COMADMIN_E_APP_FILE_WRITEFAIL = (HRESULT)(-2146368505);

	public static readonly HRESULT COMADMIN_E_APP_FILE_READFAIL = (HRESULT)(-2146368504);

	public static readonly HRESULT COMADMIN_E_APP_FILE_VERSION = (HRESULT)(-2146368503);

	public static readonly HRESULT COMADMIN_E_BADPATH = (HRESULT)(-2146368502);

	public static readonly HRESULT COMADMIN_E_APPLICATIONEXISTS = (HRESULT)(-2146368501);

	public static readonly HRESULT COMADMIN_E_ROLEEXISTS = (HRESULT)(-2146368500);

	public static readonly HRESULT COMADMIN_E_CANTCOPYFILE = (HRESULT)(-2146368499);

	public static readonly HRESULT COMADMIN_E_NOUSER = (HRESULT)(-2146368497);

	public static readonly HRESULT COMADMIN_E_INVALIDUSERIDS = (HRESULT)(-2146368496);

	public static readonly HRESULT COMADMIN_E_NOREGISTRYCLSID = (HRESULT)(-2146368495);

	public static readonly HRESULT COMADMIN_E_BADREGISTRYPROGID = (HRESULT)(-2146368494);

	public static readonly HRESULT COMADMIN_E_AUTHENTICATIONLEVEL = (HRESULT)(-2146368493);

	public static readonly HRESULT COMADMIN_E_USERPASSWDNOTVALID = (HRESULT)(-2146368492);

	public static readonly HRESULT COMADMIN_E_CLSIDORIIDMISMATCH = (HRESULT)(-2146368488);

	public static readonly HRESULT COMADMIN_E_REMOTEINTERFACE = (HRESULT)(-2146368487);

	public static readonly HRESULT COMADMIN_E_DLLREGISTERSERVER = (HRESULT)(-2146368486);

	public static readonly HRESULT COMADMIN_E_NOSERVERSHARE = (HRESULT)(-2146368485);

	public static readonly HRESULT COMADMIN_E_DLLLOADFAILED = (HRESULT)(-2146368483);

	public static readonly HRESULT COMADMIN_E_BADREGISTRYLIBID = (HRESULT)(-2146368482);

	public static readonly HRESULT COMADMIN_E_APPDIRNOTFOUND = (HRESULT)(-2146368481);

	public static readonly HRESULT COMADMIN_E_REGISTRARFAILED = (HRESULT)(-2146368477);

	public static readonly HRESULT COMADMIN_E_COMPFILE_DOESNOTEXIST = (HRESULT)(-2146368476);

	public static readonly HRESULT COMADMIN_E_COMPFILE_LOADDLLFAIL = (HRESULT)(-2146368475);

	public static readonly HRESULT COMADMIN_E_COMPFILE_GETCLASSOBJ = (HRESULT)(-2146368474);

	public static readonly HRESULT COMADMIN_E_COMPFILE_CLASSNOTAVAIL = (HRESULT)(-2146368473);

	public static readonly HRESULT COMADMIN_E_COMPFILE_BADTLB = (HRESULT)(-2146368472);

	public static readonly HRESULT COMADMIN_E_COMPFILE_NOTINSTALLABLE = (HRESULT)(-2146368471);

	public static readonly HRESULT COMADMIN_E_NOTCHANGEABLE = (HRESULT)(-2146368470);

	public static readonly HRESULT COMADMIN_E_NOTDELETEABLE = (HRESULT)(-2146368469);

	public static readonly HRESULT COMADMIN_E_SESSION = (HRESULT)(-2146368468);

	public static readonly HRESULT COMADMIN_E_COMP_MOVE_LOCKED = (HRESULT)(-2146368467);

	public static readonly HRESULT COMADMIN_E_COMP_MOVE_BAD_DEST = (HRESULT)(-2146368466);

	public static readonly HRESULT COMADMIN_E_REGISTERTLB = (HRESULT)(-2146368464);

	public static readonly HRESULT COMADMIN_E_SYSTEMAPP = (HRESULT)(-2146368461);

	public static readonly HRESULT COMADMIN_E_COMPFILE_NOREGISTRAR = (HRESULT)(-2146368460);

	public static readonly HRESULT COMADMIN_E_COREQCOMPINSTALLED = (HRESULT)(-2146368459);

	public static readonly HRESULT COMADMIN_E_SERVICENOTINSTALLED = (HRESULT)(-2146368458);

	public static readonly HRESULT COMADMIN_E_PROPERTYSAVEFAILED = (HRESULT)(-2146368457);

	public static readonly HRESULT COMADMIN_E_OBJECTEXISTS = (HRESULT)(-2146368456);

	public static readonly HRESULT COMADMIN_E_COMPONENTEXISTS = (HRESULT)(-2146368455);

	public static readonly HRESULT COMADMIN_E_REGFILE_CORRUPT = (HRESULT)(-2146368453);

	public static readonly HRESULT COMADMIN_E_PROPERTY_OVERFLOW = (HRESULT)(-2146368452);

	public static readonly HRESULT COMADMIN_E_NOTINREGISTRY = (HRESULT)(-2146368450);

	public static readonly HRESULT COMADMIN_E_OBJECTNOTPOOLABLE = (HRESULT)(-2146368449);

	public static readonly HRESULT COMADMIN_E_APPLID_MATCHES_CLSID = (HRESULT)(-2146368442);

	public static readonly HRESULT COMADMIN_E_ROLE_DOES_NOT_EXIST = (HRESULT)(-2146368441);

	public static readonly HRESULT COMADMIN_E_START_APP_NEEDS_COMPONENTS = (HRESULT)(-2146368440);

	public static readonly HRESULT COMADMIN_E_REQUIRES_DIFFERENT_PLATFORM = (HRESULT)(-2146368439);

	public static readonly HRESULT COMADMIN_E_CAN_NOT_EXPORT_APP_PROXY = (HRESULT)(-2146368438);

	public static readonly HRESULT COMADMIN_E_CAN_NOT_START_APP = (HRESULT)(-2146368437);

	public static readonly HRESULT COMADMIN_E_CAN_NOT_EXPORT_SYS_APP = (HRESULT)(-2146368436);

	public static readonly HRESULT COMADMIN_E_CANT_SUBSCRIBE_TO_COMPONENT = (HRESULT)(-2146368435);

	public static readonly HRESULT COMADMIN_E_EVENTCLASS_CANT_BE_SUBSCRIBER = (HRESULT)(-2146368434);

	public static readonly HRESULT COMADMIN_E_LIB_APP_PROXY_INCOMPATIBLE = (HRESULT)(-2146368433);

	public static readonly HRESULT COMADMIN_E_BASE_PARTITION_ONLY = (HRESULT)(-2146368432);

	public static readonly HRESULT COMADMIN_E_START_APP_DISABLED = (HRESULT)(-2146368431);

	public static readonly HRESULT COMADMIN_E_CAT_DUPLICATE_PARTITION_NAME = (HRESULT)(-2146368425);

	public static readonly HRESULT COMADMIN_E_CAT_INVALID_PARTITION_NAME = (HRESULT)(-2146368424);

	public static readonly HRESULT COMADMIN_E_CAT_PARTITION_IN_USE = (HRESULT)(-2146368423);

	public static readonly HRESULT COMADMIN_E_FILE_PARTITION_DUPLICATE_FILES = (HRESULT)(-2146368422);

	public static readonly HRESULT COMADMIN_E_CAT_IMPORTED_COMPONENTS_NOT_ALLOWED = (HRESULT)(-2146368421);

	public static readonly HRESULT COMADMIN_E_AMBIGUOUS_APPLICATION_NAME = (HRESULT)(-2146368420);

	public static readonly HRESULT COMADMIN_E_AMBIGUOUS_PARTITION_NAME = (HRESULT)(-2146368419);

	public static readonly HRESULT COMADMIN_E_REGDB_NOTINITIALIZED = (HRESULT)(-2146368398);

	public static readonly HRESULT COMADMIN_E_REGDB_NOTOPEN = (HRESULT)(-2146368397);

	public static readonly HRESULT COMADMIN_E_REGDB_SYSTEMERR = (HRESULT)(-2146368396);

	public static readonly HRESULT COMADMIN_E_REGDB_ALREADYRUNNING = (HRESULT)(-2146368395);

	public static readonly HRESULT COMADMIN_E_MIG_VERSIONNOTSUPPORTED = (HRESULT)(-2146368384);

	public static readonly HRESULT COMADMIN_E_MIG_SCHEMANOTFOUND = (HRESULT)(-2146368383);

	public static readonly HRESULT COMADMIN_E_CAT_BITNESSMISMATCH = (HRESULT)(-2146368382);

	public static readonly HRESULT COMADMIN_E_CAT_UNACCEPTABLEBITNESS = (HRESULT)(-2146368381);

	public static readonly HRESULT COMADMIN_E_CAT_WRONGAPPBITNESS = (HRESULT)(-2146368380);

	public static readonly HRESULT COMADMIN_E_CAT_PAUSE_RESUME_NOT_SUPPORTED = (HRESULT)(-2146368379);

	public static readonly HRESULT COMADMIN_E_CAT_SERVERFAULT = (HRESULT)(-2146368378);

	public static readonly HRESULT COMQC_E_APPLICATION_NOT_QUEUED = (HRESULT)(-2146368000);

	public static readonly HRESULT COMQC_E_NO_QUEUEABLE_INTERFACES = (HRESULT)(-2146367999);

	public static readonly HRESULT COMQC_E_QUEUING_SERVICE_NOT_AVAILABLE = (HRESULT)(-2146367998);

	public static readonly HRESULT COMQC_E_NO_IPERSISTSTREAM = (HRESULT)(-2146367997);

	public static readonly HRESULT COMQC_E_BAD_MESSAGE = (HRESULT)(-2146367996);

	public static readonly HRESULT COMQC_E_UNAUTHENTICATED = (HRESULT)(-2146367995);

	public static readonly HRESULT COMQC_E_UNTRUSTED_ENQUEUER = (HRESULT)(-2146367994);

	public static readonly HRESULT MSDTC_E_DUPLICATE_RESOURCE = (HRESULT)(-2146367743);

	public static readonly HRESULT COMADMIN_E_OBJECT_PARENT_MISSING = (HRESULT)(-2146367480);

	public static readonly HRESULT COMADMIN_E_OBJECT_DOES_NOT_EXIST = (HRESULT)(-2146367479);

	public static readonly HRESULT COMADMIN_E_APP_NOT_RUNNING = (HRESULT)(-2146367478);

	public static readonly HRESULT COMADMIN_E_INVALID_PARTITION = (HRESULT)(-2146367477);

	public static readonly HRESULT COMADMIN_E_SVCAPP_NOT_POOLABLE_OR_RECYCLABLE = (HRESULT)(-2146367475);

	public static readonly HRESULT COMADMIN_E_USER_IN_SET = (HRESULT)(-2146367474);

	public static readonly HRESULT COMADMIN_E_CANTRECYCLELIBRARYAPPS = (HRESULT)(-2146367473);

	public static readonly HRESULT COMADMIN_E_CANTRECYCLESERVICEAPPS = (HRESULT)(-2146367471);

	public static readonly HRESULT COMADMIN_E_PROCESSALREADYRECYCLED = (HRESULT)(-2146367470);

	public static readonly HRESULT COMADMIN_E_PAUSEDPROCESSMAYNOTBERECYCLED = (HRESULT)(-2146367469);

	public static readonly HRESULT COMADMIN_E_CANTMAKEINPROCSERVICE = (HRESULT)(-2146367468);

	public static readonly HRESULT COMADMIN_E_PROGIDINUSEBYCLSID = (HRESULT)(-2146367467);

	public static readonly HRESULT COMADMIN_E_DEFAULT_PARTITION_NOT_IN_SET = (HRESULT)(-2146367466);

	public static readonly HRESULT COMADMIN_E_RECYCLEDPROCESSMAYNOTBEPAUSED = (HRESULT)(-2146367465);

	public static readonly HRESULT COMADMIN_E_PARTITION_ACCESSDENIED = (HRESULT)(-2146367464);

	public static readonly HRESULT COMADMIN_E_PARTITION_MSI_ONLY = (HRESULT)(-2146367463);

	public static readonly HRESULT COMADMIN_E_LEGACYCOMPS_NOT_ALLOWED_IN_1_0_FORMAT = (HRESULT)(-2146367462);

	public static readonly HRESULT COMADMIN_E_LEGACYCOMPS_NOT_ALLOWED_IN_NONBASE_PARTITIONS = (HRESULT)(-2146367461);

	public static readonly HRESULT COMADMIN_E_COMP_MOVE_SOURCE = (HRESULT)(-2146367460);

	public static readonly HRESULT COMADMIN_E_COMP_MOVE_DEST = (HRESULT)(-2146367459);

	public static readonly HRESULT COMADMIN_E_COMP_MOVE_PRIVATE = (HRESULT)(-2146367458);

	public static readonly HRESULT COMADMIN_E_BASEPARTITION_REQUIRED_IN_SET = (HRESULT)(-2146367457);

	public static readonly HRESULT COMADMIN_E_CANNOT_ALIAS_EVENTCLASS = (HRESULT)(-2146367456);

	public static readonly HRESULT COMADMIN_E_PRIVATE_ACCESSDENIED = (HRESULT)(-2146367455);

	public static readonly HRESULT COMADMIN_E_SAFERINVALID = (HRESULT)(-2146367454);

	public static readonly HRESULT COMADMIN_E_REGISTRY_ACCESSDENIED = (HRESULT)(-2146367453);

	public static readonly HRESULT COMADMIN_E_PARTITIONS_DISABLED = (HRESULT)(-2146367452);

	public static readonly HRESULT MENROLL_S_ENROLLMENT_SUSPENDED = (HRESULT)(1572881);

	public static readonly HRESULT WER_S_REPORT_DEBUG = (HRESULT)(1769472);

	public static readonly HRESULT WER_S_REPORT_UPLOADED = (HRESULT)(1769473);

	public static readonly HRESULT WER_S_REPORT_QUEUED = (HRESULT)(1769474);

	public static readonly HRESULT WER_S_DISABLED = (HRESULT)(1769475);

	public static readonly HRESULT WER_S_SUSPENDED_UPLOAD = (HRESULT)(1769476);

	public static readonly HRESULT WER_S_DISABLED_QUEUE = (HRESULT)(1769477);

	public static readonly HRESULT WER_S_DISABLED_ARCHIVE = (HRESULT)(1769478);

	public static readonly HRESULT WER_S_REPORT_ASYNC = (HRESULT)(1769479);

	public static readonly HRESULT WER_S_IGNORE_ASSERT_INSTANCE = (HRESULT)(1769480);

	public static readonly HRESULT WER_S_IGNORE_ALL_ASSERTS = (HRESULT)(1769481);

	public static readonly HRESULT WER_S_ASSERT_CONTINUE = (HRESULT)(1769482);

	public static readonly HRESULT WER_S_THROTTLED = (HRESULT)(1769483);

	public static readonly HRESULT WER_S_REPORT_UPLOADED_CAB = (HRESULT)(1769484);

	public static readonly HRESULT WER_E_CRASH_FAILURE = (HRESULT)(-2145681408);

	public static readonly HRESULT WER_E_CANCELED = (HRESULT)(-2145681407);

	public static readonly HRESULT WER_E_NETWORK_FAILURE = (HRESULT)(-2145681406);

	public static readonly HRESULT WER_E_NOT_INITIALIZED = (HRESULT)(-2145681405);

	public static readonly HRESULT WER_E_ALREADY_REPORTING = (HRESULT)(-2145681404);

	public static readonly HRESULT WER_E_DUMP_THROTTLED = (HRESULT)(-2145681403);

	public static readonly HRESULT WER_E_INSUFFICIENT_CONSENT = (HRESULT)(-2145681402);

	public static readonly HRESULT WER_E_TOO_HEAVY = (HRESULT)(-2145681401);

	public static readonly HRESULT ERROR_FLT_IO_COMPLETE = (HRESULT)(2031617);

	public static readonly HRESULT ERROR_FLT_NO_HANDLER_DEFINED = (HRESULT)(-2145452031);

	public static readonly HRESULT ERROR_FLT_CONTEXT_ALREADY_DEFINED = (HRESULT)(-2145452030);

	public static readonly HRESULT ERROR_FLT_INVALID_ASYNCHRONOUS_REQUEST = (HRESULT)(-2145452029);

	public static readonly HRESULT ERROR_FLT_DISALLOW_FAST_IO = (HRESULT)(-2145452028);

	public static readonly HRESULT ERROR_FLT_INVALID_NAME_REQUEST = (HRESULT)(-2145452027);

	public static readonly HRESULT ERROR_FLT_NOT_SAFE_TO_POST_OPERATION = (HRESULT)(-2145452026);

	public static readonly HRESULT ERROR_FLT_NOT_INITIALIZED = (HRESULT)(-2145452025);

	public static readonly HRESULT ERROR_FLT_FILTER_NOT_READY = (HRESULT)(-2145452024);

	public static readonly HRESULT ERROR_FLT_POST_OPERATION_CLEANUP = (HRESULT)(-2145452023);

	public static readonly HRESULT ERROR_FLT_INTERNAL_ERROR = (HRESULT)(-2145452022);

	public static readonly HRESULT ERROR_FLT_DELETING_OBJECT = (HRESULT)(-2145452021);

	public static readonly HRESULT ERROR_FLT_MUST_BE_NONPAGED_POOL = (HRESULT)(-2145452020);

	public static readonly HRESULT ERROR_FLT_DUPLICATE_ENTRY = (HRESULT)(-2145452019);

	public static readonly HRESULT ERROR_FLT_CBDQ_DISABLED = (HRESULT)(-2145452018);

	public static readonly HRESULT ERROR_FLT_DO_NOT_ATTACH = (HRESULT)(-2145452017);

	public static readonly HRESULT ERROR_FLT_DO_NOT_DETACH = (HRESULT)(-2145452016);

	public static readonly HRESULT ERROR_FLT_INSTANCE_ALTITUDE_COLLISION = (HRESULT)(-2145452015);

	public static readonly HRESULT ERROR_FLT_INSTANCE_NAME_COLLISION = (HRESULT)(-2145452014);

	public static readonly HRESULT ERROR_FLT_FILTER_NOT_FOUND = (HRESULT)(-2145452013);

	public static readonly HRESULT ERROR_FLT_VOLUME_NOT_FOUND = (HRESULT)(-2145452012);

	public static readonly HRESULT ERROR_FLT_INSTANCE_NOT_FOUND = (HRESULT)(-2145452011);

	public static readonly HRESULT ERROR_FLT_CONTEXT_ALLOCATION_NOT_FOUND = (HRESULT)(-2145452010);

	public static readonly HRESULT ERROR_FLT_INVALID_CONTEXT_REGISTRATION = (HRESULT)(-2145452009);

	public static readonly HRESULT ERROR_FLT_NAME_CACHE_MISS = (HRESULT)(-2145452008);

	public static readonly HRESULT ERROR_FLT_NO_DEVICE_OBJECT = (HRESULT)(-2145452007);

	public static readonly HRESULT ERROR_FLT_VOLUME_ALREADY_MOUNTED = (HRESULT)(-2145452006);

	public static readonly HRESULT ERROR_FLT_ALREADY_ENLISTED = (HRESULT)(-2145452005);

	public static readonly HRESULT ERROR_FLT_CONTEXT_ALREADY_LINKED = (HRESULT)(-2145452004);

	public static readonly HRESULT ERROR_FLT_NO_WAITER_FOR_REPLY = (HRESULT)(-2145452000);

	public static readonly HRESULT ERROR_FLT_REGISTRATION_BUSY = (HRESULT)(-2145451997);

	public static readonly HRESULT ERROR_FLT_WCOS_NOT_SUPPORTED = (HRESULT)(-2145451996);

	public static readonly HRESULT ERROR_HUNG_DISPLAY_DRIVER_THREAD = (HRESULT)(-2144993279);

	public static readonly HRESULT DWM_E_COMPOSITIONDISABLED = (HRESULT)(-2144980991);

	public static readonly HRESULT DWM_E_REMOTING_NOT_SUPPORTED = (HRESULT)(-2144980990);

	public static readonly HRESULT DWM_E_NO_REDIRECTION_SURFACE_AVAILABLE = (HRESULT)(-2144980989);

	public static readonly HRESULT DWM_E_NOT_QUEUING_PRESENTS = (HRESULT)(-2144980988);

	public static readonly HRESULT DWM_E_ADAPTER_NOT_FOUND = (HRESULT)(-2144980987);

	public static readonly HRESULT DWM_S_GDI_REDIRECTION_SURFACE = (HRESULT)(2502661);

	public static readonly HRESULT DWM_E_TEXTURE_TOO_LARGE = (HRESULT)(-2144980985);

	public static readonly HRESULT DWM_S_GDI_REDIRECTION_SURFACE_BLT_VIA_GDI = (HRESULT)(2502664);

	public static readonly HRESULT ERROR_MONITOR_NO_DESCRIPTOR = (HRESULT)(2494465);

	public static readonly HRESULT ERROR_MONITOR_UNKNOWN_DESCRIPTOR_FORMAT = (HRESULT)(2494466);

	public static readonly HRESULT ERROR_MONITOR_INVALID_DESCRIPTOR_CHECKSUM = (HRESULT)(-1071247357);

	public static readonly HRESULT ERROR_MONITOR_INVALID_STANDARD_TIMING_BLOCK = (HRESULT)(-1071247356);

	public static readonly HRESULT ERROR_MONITOR_WMI_DATABLOCK_REGISTRATION_FAILED = (HRESULT)(-1071247355);

	public static readonly HRESULT ERROR_MONITOR_INVALID_SERIAL_NUMBER_MONDSC_BLOCK = (HRESULT)(-1071247354);

	public static readonly HRESULT ERROR_MONITOR_INVALID_USER_FRIENDLY_MONDSC_BLOCK = (HRESULT)(-1071247353);

	public static readonly HRESULT ERROR_MONITOR_NO_MORE_DESCRIPTOR_DATA = (HRESULT)(-1071247352);

	public static readonly HRESULT ERROR_MONITOR_INVALID_DETAILED_TIMING_BLOCK = (HRESULT)(-1071247351);

	public static readonly HRESULT ERROR_MONITOR_INVALID_MANUFACTURE_DATE = (HRESULT)(-1071247350);

	public static readonly HRESULT ERROR_GRAPHICS_NOT_EXCLUSIVE_MODE_OWNER = (HRESULT)(-1071243264);

	public static readonly HRESULT ERROR_GRAPHICS_INSUFFICIENT_DMA_BUFFER = (HRESULT)(-1071243263);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_DISPLAY_ADAPTER = (HRESULT)(-1071243262);

	public static readonly HRESULT ERROR_GRAPHICS_ADAPTER_WAS_RESET = (HRESULT)(-1071243261);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_DRIVER_MODEL = (HRESULT)(-1071243260);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_MODE_CHANGED = (HRESULT)(-1071243259);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_OCCLUDED = (HRESULT)(-1071243258);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_DENIED = (HRESULT)(-1071243257);

	public static readonly HRESULT ERROR_GRAPHICS_CANNOTCOLORCONVERT = (HRESULT)(-1071243256);

	public static readonly HRESULT ERROR_GRAPHICS_DRIVER_MISMATCH = (HRESULT)(-1071243255);

	public static readonly HRESULT ERROR_GRAPHICS_PARTIAL_DATA_POPULATED = (HRESULT)(1076240394);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_REDIRECTION_DISABLED = (HRESULT)(-1071243253);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_UNOCCLUDED = (HRESULT)(-1071243252);

	public static readonly HRESULT ERROR_GRAPHICS_WINDOWDC_NOT_AVAILABLE = (HRESULT)(-1071243251);

	public static readonly HRESULT ERROR_GRAPHICS_WINDOWLESS_PRESENT_DISABLED = (HRESULT)(-1071243250);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_INVALID_WINDOW = (HRESULT)(-1071243249);

	public static readonly HRESULT ERROR_GRAPHICS_PRESENT_BUFFER_NOT_BOUND = (HRESULT)(-1071243248);

	public static readonly HRESULT ERROR_GRAPHICS_VAIL_STATE_CHANGED = (HRESULT)(-1071243247);

	public static readonly HRESULT ERROR_GRAPHICS_INDIRECT_DISPLAY_ABANDON_SWAPCHAIN = (HRESULT)(-1071243246);

	public static readonly HRESULT ERROR_GRAPHICS_INDIRECT_DISPLAY_DEVICE_STOPPED = (HRESULT)(-1071243245);

	public static readonly HRESULT ERROR_GRAPHICS_VAIL_FAILED_TO_SEND_CREATE_SUPERWETINK_MESSAGE = (HRESULT)(-1071243244);

	public static readonly HRESULT ERROR_GRAPHICS_VAIL_FAILED_TO_SEND_DESTROY_SUPERWETINK_MESSAGE = (HRESULT)(-1071243243);

	public static readonly HRESULT ERROR_GRAPHICS_VAIL_FAILED_TO_SEND_COMPOSITION_WINDOW_DPI_MESSAGE = (HRESULT)(-1071243242);

	public static readonly HRESULT ERROR_GRAPHICS_LINK_CONFIGURATION_IN_PROGRESS = (HRESULT)(-1071243241);

	public static readonly HRESULT ERROR_GRAPHICS_MPO_ALLOCATION_UNPINNED = (HRESULT)(-1071243240);

	public static readonly HRESULT ERROR_GRAPHICS_NO_VIDEO_MEMORY = (HRESULT)(-1071243008);

	public static readonly HRESULT ERROR_GRAPHICS_CANT_LOCK_MEMORY = (HRESULT)(-1071243007);

	public static readonly HRESULT ERROR_GRAPHICS_ALLOCATION_BUSY = (HRESULT)(-1071243006);

	public static readonly HRESULT ERROR_GRAPHICS_TOO_MANY_REFERENCES = (HRESULT)(-1071243005);

	public static readonly HRESULT ERROR_GRAPHICS_TRY_AGAIN_LATER = (HRESULT)(-1071243004);

	public static readonly HRESULT ERROR_GRAPHICS_TRY_AGAIN_NOW = (HRESULT)(-1071243003);

	public static readonly HRESULT ERROR_GRAPHICS_ALLOCATION_INVALID = (HRESULT)(-1071243002);

	public static readonly HRESULT ERROR_GRAPHICS_UNSWIZZLING_APERTURE_UNAVAILABLE = (HRESULT)(-1071243001);

	public static readonly HRESULT ERROR_GRAPHICS_UNSWIZZLING_APERTURE_UNSUPPORTED = (HRESULT)(-1071243000);

	public static readonly HRESULT ERROR_GRAPHICS_CANT_EVICT_PINNED_ALLOCATION = (HRESULT)(-1071242999);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_ALLOCATION_USAGE = (HRESULT)(-1071242992);

	public static readonly HRESULT ERROR_GRAPHICS_CANT_RENDER_LOCKED_ALLOCATION = (HRESULT)(-1071242991);

	public static readonly HRESULT ERROR_GRAPHICS_ALLOCATION_CLOSED = (HRESULT)(-1071242990);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_ALLOCATION_INSTANCE = (HRESULT)(-1071242989);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_ALLOCATION_HANDLE = (HRESULT)(-1071242988);

	public static readonly HRESULT ERROR_GRAPHICS_WRONG_ALLOCATION_DEVICE = (HRESULT)(-1071242987);

	public static readonly HRESULT ERROR_GRAPHICS_ALLOCATION_CONTENT_LOST = (HRESULT)(-1071242986);

	public static readonly HRESULT ERROR_GRAPHICS_GPU_EXCEPTION_ON_DEVICE = (HRESULT)(-1071242752);

	public static readonly HRESULT ERROR_GRAPHICS_SKIP_ALLOCATION_PREPARATION = (HRESULT)(1076240897);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_TOPOLOGY = (HRESULT)(-1071242496);

	public static readonly HRESULT ERROR_GRAPHICS_VIDPN_TOPOLOGY_NOT_SUPPORTED = (HRESULT)(-1071242495);

	public static readonly HRESULT ERROR_GRAPHICS_VIDPN_TOPOLOGY_CURRENTLY_NOT_SUPPORTED = (HRESULT)(-1071242494);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN = (HRESULT)(-1071242493);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE = (HRESULT)(-1071242492);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET = (HRESULT)(-1071242491);

	public static readonly HRESULT ERROR_GRAPHICS_VIDPN_MODALITY_NOT_SUPPORTED = (HRESULT)(-1071242490);

	public static readonly HRESULT ERROR_GRAPHICS_MODE_NOT_PINNED = (HRESULT)(2499335);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_SOURCEMODESET = (HRESULT)(-1071242488);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_TARGETMODESET = (HRESULT)(-1071242487);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_FREQUENCY = (HRESULT)(-1071242486);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_ACTIVE_REGION = (HRESULT)(-1071242485);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_TOTAL_REGION = (HRESULT)(-1071242484);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE_MODE = (HRESULT)(-1071242480);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET_MODE = (HRESULT)(-1071242479);

	public static readonly HRESULT ERROR_GRAPHICS_PINNED_MODE_MUST_REMAIN_IN_SET = (HRESULT)(-1071242478);

	public static readonly HRESULT ERROR_GRAPHICS_PATH_ALREADY_IN_TOPOLOGY = (HRESULT)(-1071242477);

	public static readonly HRESULT ERROR_GRAPHICS_MODE_ALREADY_IN_MODESET = (HRESULT)(-1071242476);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEOPRESENTSOURCESET = (HRESULT)(-1071242475);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDEOPRESENTTARGETSET = (HRESULT)(-1071242474);

	public static readonly HRESULT ERROR_GRAPHICS_SOURCE_ALREADY_IN_SET = (HRESULT)(-1071242473);

	public static readonly HRESULT ERROR_GRAPHICS_TARGET_ALREADY_IN_SET = (HRESULT)(-1071242472);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_PRESENT_PATH = (HRESULT)(-1071242471);

	public static readonly HRESULT ERROR_GRAPHICS_NO_RECOMMENDED_VIDPN_TOPOLOGY = (HRESULT)(-1071242470);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGESET = (HRESULT)(-1071242469);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE = (HRESULT)(-1071242468);

	public static readonly HRESULT ERROR_GRAPHICS_FREQUENCYRANGE_NOT_IN_SET = (HRESULT)(-1071242467);

	public static readonly HRESULT ERROR_GRAPHICS_NO_PREFERRED_MODE = (HRESULT)(2499358);

	public static readonly HRESULT ERROR_GRAPHICS_FREQUENCYRANGE_ALREADY_IN_SET = (HRESULT)(-1071242465);

	public static readonly HRESULT ERROR_GRAPHICS_STALE_MODESET = (HRESULT)(-1071242464);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_SOURCEMODESET = (HRESULT)(-1071242463);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_SOURCE_MODE = (HRESULT)(-1071242462);

	public static readonly HRESULT ERROR_GRAPHICS_NO_RECOMMENDED_FUNCTIONAL_VIDPN = (HRESULT)(-1071242461);

	public static readonly HRESULT ERROR_GRAPHICS_MODE_ID_MUST_BE_UNIQUE = (HRESULT)(-1071242460);

	public static readonly HRESULT ERROR_GRAPHICS_EMPTY_ADAPTER_MONITOR_MODE_SUPPORT_INTERSECTION = (HRESULT)(-1071242459);

	public static readonly HRESULT ERROR_GRAPHICS_VIDEO_PRESENT_TARGETS_LESS_THAN_SOURCES = (HRESULT)(-1071242458);

	public static readonly HRESULT ERROR_GRAPHICS_PATH_NOT_IN_TOPOLOGY = (HRESULT)(-1071242457);

	public static readonly HRESULT ERROR_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_SOURCE = (HRESULT)(-1071242456);

	public static readonly HRESULT ERROR_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_TARGET = (HRESULT)(-1071242455);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITORDESCRIPTORSET = (HRESULT)(-1071242454);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITORDESCRIPTOR = (HRESULT)(-1071242453);

	public static readonly HRESULT ERROR_GRAPHICS_MONITORDESCRIPTOR_NOT_IN_SET = (HRESULT)(-1071242452);

	public static readonly HRESULT ERROR_GRAPHICS_MONITORDESCRIPTOR_ALREADY_IN_SET = (HRESULT)(-1071242451);

	public static readonly HRESULT ERROR_GRAPHICS_MONITORDESCRIPTOR_ID_MUST_BE_UNIQUE = (HRESULT)(-1071242450);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_TARGET_SUBSET_TYPE = (HRESULT)(-1071242449);

	public static readonly HRESULT ERROR_GRAPHICS_RESOURCES_NOT_RELATED = (HRESULT)(-1071242448);

	public static readonly HRESULT ERROR_GRAPHICS_SOURCE_ID_MUST_BE_UNIQUE = (HRESULT)(-1071242447);

	public static readonly HRESULT ERROR_GRAPHICS_TARGET_ID_MUST_BE_UNIQUE = (HRESULT)(-1071242446);

	public static readonly HRESULT ERROR_GRAPHICS_NO_AVAILABLE_VIDPN_TARGET = (HRESULT)(-1071242445);

	public static readonly HRESULT ERROR_GRAPHICS_MONITOR_COULD_NOT_BE_ASSOCIATED_WITH_ADAPTER = (HRESULT)(-1071242444);

	public static readonly HRESULT ERROR_GRAPHICS_NO_VIDPNMGR = (HRESULT)(-1071242443);

	public static readonly HRESULT ERROR_GRAPHICS_NO_ACTIVE_VIDPN = (HRESULT)(-1071242442);

	public static readonly HRESULT ERROR_GRAPHICS_STALE_VIDPN_TOPOLOGY = (HRESULT)(-1071242441);

	public static readonly HRESULT ERROR_GRAPHICS_MONITOR_NOT_CONNECTED = (HRESULT)(-1071242440);

	public static readonly HRESULT ERROR_GRAPHICS_SOURCE_NOT_IN_TOPOLOGY = (HRESULT)(-1071242439);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PRIMARYSURFACE_SIZE = (HRESULT)(-1071242438);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VISIBLEREGION_SIZE = (HRESULT)(-1071242437);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_STRIDE = (HRESULT)(-1071242436);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PIXELFORMAT = (HRESULT)(-1071242435);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_COLORBASIS = (HRESULT)(-1071242434);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PIXELVALUEACCESSMODE = (HRESULT)(-1071242433);

	public static readonly HRESULT ERROR_GRAPHICS_TARGET_NOT_IN_TOPOLOGY = (HRESULT)(-1071242432);

	public static readonly HRESULT ERROR_GRAPHICS_NO_DISPLAY_MODE_MANAGEMENT_SUPPORT = (HRESULT)(-1071242431);

	public static readonly HRESULT ERROR_GRAPHICS_VIDPN_SOURCE_IN_USE = (HRESULT)(-1071242430);

	public static readonly HRESULT ERROR_GRAPHICS_CANT_ACCESS_ACTIVE_VIDPN = (HRESULT)(-1071242429);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PATH_IMPORTANCE_ORDINAL = (HRESULT)(-1071242428);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PATH_CONTENT_GEOMETRY_TRANSFORMATION = (HRESULT)(-1071242427);

	public static readonly HRESULT ERROR_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_SUPPORTED = (HRESULT)(-1071242426);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_GAMMA_RAMP = (HRESULT)(-1071242425);

	public static readonly HRESULT ERROR_GRAPHICS_GAMMA_RAMP_NOT_SUPPORTED = (HRESULT)(-1071242424);

	public static readonly HRESULT ERROR_GRAPHICS_MULTISAMPLING_NOT_SUPPORTED = (HRESULT)(-1071242423);

	public static readonly HRESULT ERROR_GRAPHICS_MODE_NOT_IN_MODESET = (HRESULT)(-1071242422);

	public static readonly HRESULT ERROR_GRAPHICS_DATASET_IS_EMPTY = (HRESULT)(2499403);

	public static readonly HRESULT ERROR_GRAPHICS_NO_MORE_ELEMENTS_IN_DATASET = (HRESULT)(2499404);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_VIDPN_TOPOLOGY_RECOMMENDATION_REASON = (HRESULT)(-1071242419);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PATH_CONTENT_TYPE = (HRESULT)(-1071242418);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_COPYPROTECTION_TYPE = (HRESULT)(-1071242417);

	public static readonly HRESULT ERROR_GRAPHICS_UNASSIGNED_MODESET_ALREADY_EXISTS = (HRESULT)(-1071242416);

	public static readonly HRESULT ERROR_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_PINNED = (HRESULT)(2499409);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_SCANLINE_ORDERING = (HRESULT)(-1071242414);

	public static readonly HRESULT ERROR_GRAPHICS_TOPOLOGY_CHANGES_NOT_ALLOWED = (HRESULT)(-1071242413);

	public static readonly HRESULT ERROR_GRAPHICS_NO_AVAILABLE_IMPORTANCE_ORDINALS = (HRESULT)(-1071242412);

	public static readonly HRESULT ERROR_GRAPHICS_INCOMPATIBLE_PRIVATE_FORMAT = (HRESULT)(-1071242411);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MODE_PRUNING_ALGORITHM = (HRESULT)(-1071242410);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_CAPABILITY_ORIGIN = (HRESULT)(-1071242409);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE_CONSTRAINT = (HRESULT)(-1071242408);

	public static readonly HRESULT ERROR_GRAPHICS_MAX_NUM_PATHS_REACHED = (HRESULT)(-1071242407);

	public static readonly HRESULT ERROR_GRAPHICS_CANCEL_VIDPN_TOPOLOGY_AUGMENTATION = (HRESULT)(-1071242406);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_CLIENT_TYPE = (HRESULT)(-1071242405);

	public static readonly HRESULT ERROR_GRAPHICS_CLIENTVIDPN_NOT_SET = (HRESULT)(-1071242404);

	public static readonly HRESULT ERROR_GRAPHICS_SPECIFIED_CHILD_ALREADY_CONNECTED = (HRESULT)(-1071242240);

	public static readonly HRESULT ERROR_GRAPHICS_CHILD_DESCRIPTOR_NOT_SUPPORTED = (HRESULT)(-1071242239);

	public static readonly HRESULT ERROR_GRAPHICS_UNKNOWN_CHILD_STATUS = (HRESULT)(1076241455);

	public static readonly HRESULT ERROR_GRAPHICS_NOT_A_LINKED_ADAPTER = (HRESULT)(-1071242192);

	public static readonly HRESULT ERROR_GRAPHICS_LEADLINK_NOT_ENUMERATED = (HRESULT)(-1071242191);

	public static readonly HRESULT ERROR_GRAPHICS_CHAINLINKS_NOT_ENUMERATED = (HRESULT)(-1071242190);

	public static readonly HRESULT ERROR_GRAPHICS_ADAPTER_CHAIN_NOT_READY = (HRESULT)(-1071242189);

	public static readonly HRESULT ERROR_GRAPHICS_CHAINLINKS_NOT_STARTED = (HRESULT)(-1071242188);

	public static readonly HRESULT ERROR_GRAPHICS_CHAINLINKS_NOT_POWERED_ON = (HRESULT)(-1071242187);

	public static readonly HRESULT ERROR_GRAPHICS_INCONSISTENT_DEVICE_LINK_STATE = (HRESULT)(-1071242186);

	public static readonly HRESULT ERROR_GRAPHICS_LEADLINK_START_DEFERRED = (HRESULT)(1076241463);

	public static readonly HRESULT ERROR_GRAPHICS_NOT_POST_DEVICE_DRIVER = (HRESULT)(-1071242184);

	public static readonly HRESULT ERROR_GRAPHICS_POLLING_TOO_FREQUENTLY = (HRESULT)(1076241465);

	public static readonly HRESULT ERROR_GRAPHICS_START_DEFERRED = (HRESULT)(1076241466);

	public static readonly HRESULT ERROR_GRAPHICS_ADAPTER_ACCESS_NOT_EXCLUDED = (HRESULT)(-1071242181);

	public static readonly HRESULT ERROR_GRAPHICS_DEPENDABLE_CHILD_STATUS = (HRESULT)(1076241468);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_NOT_SUPPORTED = (HRESULT)(-1071241984);

	public static readonly HRESULT ERROR_GRAPHICS_COPP_NOT_SUPPORTED = (HRESULT)(-1071241983);

	public static readonly HRESULT ERROR_GRAPHICS_UAB_NOT_SUPPORTED = (HRESULT)(-1071241982);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INVALID_ENCRYPTED_PARAMETERS = (HRESULT)(-1071241981);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_NO_VIDEO_OUTPUTS_EXIST = (HRESULT)(-1071241979);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INTERNAL_ERROR = (HRESULT)(-1071241973);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INVALID_HANDLE = (HRESULT)(-1071241972);

	public static readonly HRESULT ERROR_GRAPHICS_PVP_INVALID_CERTIFICATE_LENGTH = (HRESULT)(-1071241970);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_SPANNING_MODE_ENABLED = (HRESULT)(-1071241969);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_THEATER_MODE_ENABLED = (HRESULT)(-1071241968);

	public static readonly HRESULT ERROR_GRAPHICS_PVP_HFS_FAILED = (HRESULT)(-1071241967);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INVALID_SRM = (HRESULT)(-1071241966);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_HDCP = (HRESULT)(-1071241965);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_ACP = (HRESULT)(-1071241964);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_CGMSA = (HRESULT)(-1071241963);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_HDCP_SRM_NEVER_SET = (HRESULT)(-1071241962);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_RESOLUTION_TOO_HIGH = (HRESULT)(-1071241961);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_ALL_HDCP_HARDWARE_ALREADY_IN_USE = (HRESULT)(-1071241960);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_VIDEO_OUTPUT_NO_LONGER_EXISTS = (HRESULT)(-1071241958);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_SESSION_TYPE_CHANGE_IN_PROGRESS = (HRESULT)(-1071241957);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_VIDEO_OUTPUT_DOES_NOT_HAVE_COPP_SEMANTICS = (HRESULT)(-1071241956);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INVALID_INFORMATION_REQUEST = (HRESULT)(-1071241955);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_DRIVER_INTERNAL_ERROR = (HRESULT)(-1071241954);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_VIDEO_OUTPUT_DOES_NOT_HAVE_OPM_SEMANTICS = (HRESULT)(-1071241953);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_SIGNALING_NOT_SUPPORTED = (HRESULT)(-1071241952);

	public static readonly HRESULT ERROR_GRAPHICS_OPM_INVALID_CONFIGURATION_REQUEST = (HRESULT)(-1071241951);

	public static readonly HRESULT ERROR_GRAPHICS_I2C_NOT_SUPPORTED = (HRESULT)(-1071241856);

	public static readonly HRESULT ERROR_GRAPHICS_I2C_DEVICE_DOES_NOT_EXIST = (HRESULT)(-1071241855);

	public static readonly HRESULT ERROR_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA = (HRESULT)(-1071241854);

	public static readonly HRESULT ERROR_GRAPHICS_I2C_ERROR_RECEIVING_DATA = (HRESULT)(-1071241853);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = (HRESULT)(-1071241852);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_INVALID_DATA = (HRESULT)(-1071241851);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE = (HRESULT)(-1071241850);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_INVALID_CAPABILITIES_STRING = (HRESULT)(-1071241849);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_INTERNAL_ERROR = (HRESULT)(-1071241848);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND = (HRESULT)(-1071241847);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH = (HRESULT)(-1071241846);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM = (HRESULT)(-1071241845);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE = (HRESULT)(-1071241844);

	public static readonly HRESULT ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS = (HRESULT)(-1071241843);

	public static readonly HRESULT ERROR_GRAPHICS_DDCCI_CURRENT_CURRENT_VALUE_GREATER_THAN_MAXIMUM_VALUE = (HRESULT)(-1071241768);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_INVALID_VCP_VERSION = (HRESULT)(-1071241767);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_MONITOR_VIOLATES_MCCS_SPECIFICATION = (HRESULT)(-1071241766);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_MCCS_VERSION_MISMATCH = (HRESULT)(-1071241765);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_UNSUPPORTED_MCCS_VERSION = (HRESULT)(-1071241764);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_INVALID_TECHNOLOGY_TYPE_RETURNED = (HRESULT)(-1071241762);

	public static readonly HRESULT ERROR_GRAPHICS_MCA_UNSUPPORTED_COLOR_TEMPERATURE = (HRESULT)(-1071241761);

	public static readonly HRESULT ERROR_GRAPHICS_ONLY_CONSOLE_SESSION_SUPPORTED = (HRESULT)(-1071241760);

	public static readonly HRESULT ERROR_GRAPHICS_NO_DISPLAY_DEVICE_CORRESPONDS_TO_NAME = (HRESULT)(-1071241759);

	public static readonly HRESULT ERROR_GRAPHICS_DISPLAY_DEVICE_NOT_ATTACHED_TO_DESKTOP = (HRESULT)(-1071241758);

	public static readonly HRESULT ERROR_GRAPHICS_MIRRORING_DEVICES_NOT_SUPPORTED = (HRESULT)(-1071241757);

	public static readonly HRESULT ERROR_GRAPHICS_INVALID_POINTER = (HRESULT)(-1071241756);

	public static readonly HRESULT ERROR_GRAPHICS_NO_MONITORS_CORRESPOND_TO_DISPLAY_DEVICE = (HRESULT)(-1071241755);

	public static readonly HRESULT ERROR_GRAPHICS_PARAMETER_ARRAY_TOO_SMALL = (HRESULT)(-1071241754);

	public static readonly HRESULT ERROR_GRAPHICS_INTERNAL_ERROR = (HRESULT)(-1071241753);

	public static readonly HRESULT ERROR_GRAPHICS_SESSION_TYPE_CHANGE_IN_PROGRESS = (HRESULT)(-1071249944);

	public static readonly HRESULT NAP_E_INVALID_PACKET = (HRESULT)(-2144927743);

	public static readonly HRESULT NAP_E_MISSING_SOH = (HRESULT)(-2144927742);

	public static readonly HRESULT NAP_E_CONFLICTING_ID = (HRESULT)(-2144927741);

	public static readonly HRESULT NAP_E_NO_CACHED_SOH = (HRESULT)(-2144927740);

	public static readonly HRESULT NAP_E_STILL_BOUND = (HRESULT)(-2144927739);

	public static readonly HRESULT NAP_E_NOT_REGISTERED = (HRESULT)(-2144927738);

	public static readonly HRESULT NAP_E_NOT_INITIALIZED = (HRESULT)(-2144927737);

	public static readonly HRESULT NAP_E_MISMATCHED_ID = (HRESULT)(-2144927736);

	public static readonly HRESULT NAP_E_NOT_PENDING = (HRESULT)(-2144927735);

	public static readonly HRESULT NAP_E_ID_NOT_FOUND = (HRESULT)(-2144927734);

	public static readonly HRESULT NAP_E_MAXSIZE_TOO_SMALL = (HRESULT)(-2144927733);

	public static readonly HRESULT NAP_E_SERVICE_NOT_RUNNING = (HRESULT)(-2144927732);

	public static readonly HRESULT NAP_S_CERT_ALREADY_PRESENT = (HRESULT)(2555917);

	public static readonly HRESULT NAP_E_ENTITY_DISABLED = (HRESULT)(-2144927730);

	public static readonly HRESULT NAP_E_NETSH_GROUPPOLICY_ERROR = (HRESULT)(-2144927729);

	public static readonly HRESULT NAP_E_TOO_MANY_CALLS = (HRESULT)(-2144927728);

	public static readonly HRESULT NAP_E_SHV_CONFIG_EXISTED = (HRESULT)(-2144927727);

	public static readonly HRESULT NAP_E_SHV_CONFIG_NOT_FOUND = (HRESULT)(-2144927726);

	public static readonly HRESULT NAP_E_SHV_TIMEOUT = (HRESULT)(-2144927725);

	public static readonly HRESULT TPM_E_ERROR_MASK = (HRESULT)(-2144862208);

	public static readonly HRESULT TPM_E_AUTHFAIL = (HRESULT)(-2144862207);

	public static readonly HRESULT TPM_E_BADINDEX = (HRESULT)(-2144862206);

	public static readonly HRESULT TPM_E_BAD_PARAMETER = (HRESULT)(-2144862205);

	public static readonly HRESULT TPM_E_AUDITFAILURE = (HRESULT)(-2144862204);

	public static readonly HRESULT TPM_E_CLEAR_DISABLED = (HRESULT)(-2144862203);

	public static readonly HRESULT TPM_E_DEACTIVATED = (HRESULT)(-2144862202);

	public static readonly HRESULT TPM_E_DISABLED = (HRESULT)(-2144862201);

	public static readonly HRESULT TPM_E_DISABLED_CMD = (HRESULT)(-2144862200);

	public static readonly HRESULT TPM_E_FAIL = (HRESULT)(-2144862199);

	public static readonly HRESULT TPM_E_BAD_ORDINAL = (HRESULT)(-2144862198);

	public static readonly HRESULT TPM_E_INSTALL_DISABLED = (HRESULT)(-2144862197);

	public static readonly HRESULT TPM_E_INVALID_KEYHANDLE = (HRESULT)(-2144862196);

	public static readonly HRESULT TPM_E_KEYNOTFOUND = (HRESULT)(-2144862195);

	public static readonly HRESULT TPM_E_INAPPROPRIATE_ENC = (HRESULT)(-2144862194);

	public static readonly HRESULT TPM_E_MIGRATEFAIL = (HRESULT)(-2144862193);

	public static readonly HRESULT TPM_E_INVALID_PCR_INFO = (HRESULT)(-2144862192);

	public static readonly HRESULT TPM_E_NOSPACE = (HRESULT)(-2144862191);

	public static readonly HRESULT TPM_E_NOSRK = (HRESULT)(-2144862190);

	public static readonly HRESULT TPM_E_NOTSEALED_BLOB = (HRESULT)(-2144862189);

	public static readonly HRESULT TPM_E_OWNER_SET = (HRESULT)(-2144862188);

	public static readonly HRESULT TPM_E_RESOURCES = (HRESULT)(-2144862187);

	public static readonly HRESULT TPM_E_SHORTRANDOM = (HRESULT)(-2144862186);

	public static readonly HRESULT TPM_E_SIZE = (HRESULT)(-2144862185);

	public static readonly HRESULT TPM_E_WRONGPCRVAL = (HRESULT)(-2144862184);

	public static readonly HRESULT TPM_E_BAD_PARAM_SIZE = (HRESULT)(-2144862183);

	public static readonly HRESULT TPM_E_SHA_THREAD = (HRESULT)(-2144862182);

	public static readonly HRESULT TPM_E_SHA_ERROR = (HRESULT)(-2144862181);

	public static readonly HRESULT TPM_E_FAILEDSELFTEST = (HRESULT)(-2144862180);

	public static readonly HRESULT TPM_E_AUTH2FAIL = (HRESULT)(-2144862179);

	public static readonly HRESULT TPM_E_BADTAG = (HRESULT)(-2144862178);

	public static readonly HRESULT TPM_E_IOERROR = (HRESULT)(-2144862177);

	public static readonly HRESULT TPM_E_ENCRYPT_ERROR = (HRESULT)(-2144862176);

	public static readonly HRESULT TPM_E_DECRYPT_ERROR = (HRESULT)(-2144862175);

	public static readonly HRESULT TPM_E_INVALID_AUTHHANDLE = (HRESULT)(-2144862174);

	public static readonly HRESULT TPM_E_NO_ENDORSEMENT = (HRESULT)(-2144862173);

	public static readonly HRESULT TPM_E_INVALID_KEYUSAGE = (HRESULT)(-2144862172);

	public static readonly HRESULT TPM_E_WRONG_ENTITYTYPE = (HRESULT)(-2144862171);

	public static readonly HRESULT TPM_E_INVALID_POSTINIT = (HRESULT)(-2144862170);

	public static readonly HRESULT TPM_E_INAPPROPRIATE_SIG = (HRESULT)(-2144862169);

	public static readonly HRESULT TPM_E_BAD_KEY_PROPERTY = (HRESULT)(-2144862168);

	public static readonly HRESULT TPM_E_BAD_MIGRATION = (HRESULT)(-2144862167);

	public static readonly HRESULT TPM_E_BAD_SCHEME = (HRESULT)(-2144862166);

	public static readonly HRESULT TPM_E_BAD_DATASIZE = (HRESULT)(-2144862165);

	public static readonly HRESULT TPM_E_BAD_MODE = (HRESULT)(-2144862164);

	public static readonly HRESULT TPM_E_BAD_PRESENCE = (HRESULT)(-2144862163);

	public static readonly HRESULT TPM_E_BAD_VERSION = (HRESULT)(-2144862162);

	public static readonly HRESULT TPM_E_NO_WRAP_TRANSPORT = (HRESULT)(-2144862161);

	public static readonly HRESULT TPM_E_AUDITFAIL_UNSUCCESSFUL = (HRESULT)(-2144862160);

	public static readonly HRESULT TPM_E_AUDITFAIL_SUCCESSFUL = (HRESULT)(-2144862159);

	public static readonly HRESULT TPM_E_NOTRESETABLE = (HRESULT)(-2144862158);

	public static readonly HRESULT TPM_E_NOTLOCAL = (HRESULT)(-2144862157);

	public static readonly HRESULT TPM_E_BAD_TYPE = (HRESULT)(-2144862156);

	public static readonly HRESULT TPM_E_INVALID_RESOURCE = (HRESULT)(-2144862155);

	public static readonly HRESULT TPM_E_NOTFIPS = (HRESULT)(-2144862154);

	public static readonly HRESULT TPM_E_INVALID_FAMILY = (HRESULT)(-2144862153);

	public static readonly HRESULT TPM_E_NO_NV_PERMISSION = (HRESULT)(-2144862152);

	public static readonly HRESULT TPM_E_REQUIRES_SIGN = (HRESULT)(-2144862151);

	public static readonly HRESULT TPM_E_KEY_NOTSUPPORTED = (HRESULT)(-2144862150);

	public static readonly HRESULT TPM_E_AUTH_CONFLICT = (HRESULT)(-2144862149);

	public static readonly HRESULT TPM_E_AREA_LOCKED = (HRESULT)(-2144862148);

	public static readonly HRESULT TPM_E_BAD_LOCALITY = (HRESULT)(-2144862147);

	public static readonly HRESULT TPM_E_READ_ONLY = (HRESULT)(-2144862146);

	public static readonly HRESULT TPM_E_PER_NOWRITE = (HRESULT)(-2144862145);

	public static readonly HRESULT TPM_E_FAMILYCOUNT = (HRESULT)(-2144862144);

	public static readonly HRESULT TPM_E_WRITE_LOCKED = (HRESULT)(-2144862143);

	public static readonly HRESULT TPM_E_BAD_ATTRIBUTES = (HRESULT)(-2144862142);

	public static readonly HRESULT TPM_E_INVALID_STRUCTURE = (HRESULT)(-2144862141);

	public static readonly HRESULT TPM_E_KEY_OWNER_CONTROL = (HRESULT)(-2144862140);

	public static readonly HRESULT TPM_E_BAD_COUNTER = (HRESULT)(-2144862139);

	public static readonly HRESULT TPM_E_NOT_FULLWRITE = (HRESULT)(-2144862138);

	public static readonly HRESULT TPM_E_CONTEXT_GAP = (HRESULT)(-2144862137);

	public static readonly HRESULT TPM_E_MAXNVWRITES = (HRESULT)(-2144862136);

	public static readonly HRESULT TPM_E_NOOPERATOR = (HRESULT)(-2144862135);

	public static readonly HRESULT TPM_E_RESOURCEMISSING = (HRESULT)(-2144862134);

	public static readonly HRESULT TPM_E_DELEGATE_LOCK = (HRESULT)(-2144862133);

	public static readonly HRESULT TPM_E_DELEGATE_FAMILY = (HRESULT)(-2144862132);

	public static readonly HRESULT TPM_E_DELEGATE_ADMIN = (HRESULT)(-2144862131);

	public static readonly HRESULT TPM_E_TRANSPORT_NOTEXCLUSIVE = (HRESULT)(-2144862130);

	public static readonly HRESULT TPM_E_OWNER_CONTROL = (HRESULT)(-2144862129);

	public static readonly HRESULT TPM_E_DAA_RESOURCES = (HRESULT)(-2144862128);

	public static readonly HRESULT TPM_E_DAA_INPUT_DATA0 = (HRESULT)(-2144862127);

	public static readonly HRESULT TPM_E_DAA_INPUT_DATA1 = (HRESULT)(-2144862126);

	public static readonly HRESULT TPM_E_DAA_ISSUER_SETTINGS = (HRESULT)(-2144862125);

	public static readonly HRESULT TPM_E_DAA_TPM_SETTINGS = (HRESULT)(-2144862124);

	public static readonly HRESULT TPM_E_DAA_STAGE = (HRESULT)(-2144862123);

	public static readonly HRESULT TPM_E_DAA_ISSUER_VALIDITY = (HRESULT)(-2144862122);

	public static readonly HRESULT TPM_E_DAA_WRONG_W = (HRESULT)(-2144862121);

	public static readonly HRESULT TPM_E_BAD_HANDLE = (HRESULT)(-2144862120);

	public static readonly HRESULT TPM_E_BAD_DELEGATE = (HRESULT)(-2144862119);

	public static readonly HRESULT TPM_E_BADCONTEXT = (HRESULT)(-2144862118);

	public static readonly HRESULT TPM_E_TOOMANYCONTEXTS = (HRESULT)(-2144862117);

	public static readonly HRESULT TPM_E_MA_TICKET_SIGNATURE = (HRESULT)(-2144862116);

	public static readonly HRESULT TPM_E_MA_DESTINATION = (HRESULT)(-2144862115);

	public static readonly HRESULT TPM_E_MA_SOURCE = (HRESULT)(-2144862114);

	public static readonly HRESULT TPM_E_MA_AUTHORITY = (HRESULT)(-2144862113);

	public static readonly HRESULT TPM_E_PERMANENTEK = (HRESULT)(-2144862111);

	public static readonly HRESULT TPM_E_BAD_SIGNATURE = (HRESULT)(-2144862110);

	public static readonly HRESULT TPM_E_NOCONTEXTSPACE = (HRESULT)(-2144862109);

	public static readonly HRESULT TPM_20_E_ASYMMETRIC = (HRESULT)(-2144862079);

	public static readonly HRESULT TPM_20_E_ATTRIBUTES = (HRESULT)(-2144862078);

	public static readonly HRESULT TPM_20_E_HASH = (HRESULT)(-2144862077);

	public static readonly HRESULT TPM_20_E_VALUE = (HRESULT)(-2144862076);

	public static readonly HRESULT TPM_20_E_HIERARCHY = (HRESULT)(-2144862075);

	public static readonly HRESULT TPM_20_E_KEY_SIZE = (HRESULT)(-2144862073);

	public static readonly HRESULT TPM_20_E_MGF = (HRESULT)(-2144862072);

	public static readonly HRESULT TPM_20_E_MODE = (HRESULT)(-2144862071);

	public static readonly HRESULT TPM_20_E_TYPE = (HRESULT)(-2144862070);

	public static readonly HRESULT TPM_20_E_HANDLE = (HRESULT)(-2144862069);

	public static readonly HRESULT TPM_20_E_KDF = (HRESULT)(-2144862068);

	public static readonly HRESULT TPM_20_E_RANGE = (HRESULT)(-2144862067);

	public static readonly HRESULT TPM_20_E_AUTH_FAIL = (HRESULT)(-2144862066);

	public static readonly HRESULT TPM_20_E_NONCE = (HRESULT)(-2144862065);

	public static readonly HRESULT TPM_20_E_PP = (HRESULT)(-2144862064);

	public static readonly HRESULT TPM_20_E_SCHEME = (HRESULT)(-2144862062);

	public static readonly HRESULT TPM_20_E_SIZE = (HRESULT)(-2144862059);

	public static readonly HRESULT TPM_20_E_SYMMETRIC = (HRESULT)(-2144862058);

	public static readonly HRESULT TPM_20_E_TAG = (HRESULT)(-2144862057);

	public static readonly HRESULT TPM_20_E_SELECTOR = (HRESULT)(-2144862056);

	public static readonly HRESULT TPM_20_E_INSUFFICIENT = (HRESULT)(-2144862054);

	public static readonly HRESULT TPM_20_E_SIGNATURE = (HRESULT)(-2144862053);

	public static readonly HRESULT TPM_20_E_KEY = (HRESULT)(-2144862052);

	public static readonly HRESULT TPM_20_E_POLICY_FAIL = (HRESULT)(-2144862051);

	public static readonly HRESULT TPM_20_E_INTEGRITY = (HRESULT)(-2144862049);

	public static readonly HRESULT TPM_20_E_TICKET = (HRESULT)(-2144862048);

	public static readonly HRESULT TPM_20_E_RESERVED_BITS = (HRESULT)(-2144862047);

	public static readonly HRESULT TPM_20_E_BAD_AUTH = (HRESULT)(-2144862046);

	public static readonly HRESULT TPM_20_E_EXPIRED = (HRESULT)(-2144862045);

	public static readonly HRESULT TPM_20_E_POLICY_CC = (HRESULT)(-2144862044);

	public static readonly HRESULT TPM_20_E_BINDING = (HRESULT)(-2144862043);

	public static readonly HRESULT TPM_20_E_CURVE = (HRESULT)(-2144862042);

	public static readonly HRESULT TPM_20_E_ECC_POINT = (HRESULT)(-2144862041);

	public static readonly HRESULT TPM_20_E_INITIALIZE = (HRESULT)(-2144861952);

	public static readonly HRESULT TPM_20_E_FAILURE = (HRESULT)(-2144861951);

	public static readonly HRESULT TPM_20_E_SEQUENCE = (HRESULT)(-2144861949);

	public static readonly HRESULT TPM_20_E_PRIVATE = (HRESULT)(-2144861941);

	public static readonly HRESULT TPM_20_E_HMAC = (HRESULT)(-2144861927);

	public static readonly HRESULT TPM_20_E_DISABLED = (HRESULT)(-2144861920);

	public static readonly HRESULT TPM_20_E_EXCLUSIVE = (HRESULT)(-2144861919);

	public static readonly HRESULT TPM_20_E_ECC_CURVE = (HRESULT)(-2144861917);

	public static readonly HRESULT TPM_20_E_AUTH_TYPE = (HRESULT)(-2144861916);

	public static readonly HRESULT TPM_20_E_AUTH_MISSING = (HRESULT)(-2144861915);

	public static readonly HRESULT TPM_20_E_POLICY = (HRESULT)(-2144861914);

	public static readonly HRESULT TPM_20_E_PCR = (HRESULT)(-2144861913);

	public static readonly HRESULT TPM_20_E_PCR_CHANGED = (HRESULT)(-2144861912);

	public static readonly HRESULT TPM_20_E_UPGRADE = (HRESULT)(-2144861907);

	public static readonly HRESULT TPM_20_E_TOO_MANY_CONTEXTS = (HRESULT)(-2144861906);

	public static readonly HRESULT TPM_20_E_AUTH_UNAVAILABLE = (HRESULT)(-2144861905);

	public static readonly HRESULT TPM_20_E_REBOOT = (HRESULT)(-2144861904);

	public static readonly HRESULT TPM_20_E_UNBALANCED = (HRESULT)(-2144861903);

	public static readonly HRESULT TPM_20_E_COMMAND_SIZE = (HRESULT)(-2144861886);

	public static readonly HRESULT TPM_20_E_COMMAND_CODE = (HRESULT)(-2144861885);

	public static readonly HRESULT TPM_20_E_AUTHSIZE = (HRESULT)(-2144861884);

	public static readonly HRESULT TPM_20_E_AUTH_CONTEXT = (HRESULT)(-2144861883);

	public static readonly HRESULT TPM_20_E_NV_RANGE = (HRESULT)(-2144861882);

	public static readonly HRESULT TPM_20_E_NV_SIZE = (HRESULT)(-2144861881);

	public static readonly HRESULT TPM_20_E_NV_LOCKED = (HRESULT)(-2144861880);

	public static readonly HRESULT TPM_20_E_NV_AUTHORIZATION = (HRESULT)(-2144861879);

	public static readonly HRESULT TPM_20_E_NV_UNINITIALIZED = (HRESULT)(-2144861878);

	public static readonly HRESULT TPM_20_E_NV_SPACE = (HRESULT)(-2144861877);

	public static readonly HRESULT TPM_20_E_NV_DEFINED = (HRESULT)(-2144861876);

	public static readonly HRESULT TPM_20_E_BAD_CONTEXT = (HRESULT)(-2144861872);

	public static readonly HRESULT TPM_20_E_CPHASH = (HRESULT)(-2144861871);

	public static readonly HRESULT TPM_20_E_PARENT = (HRESULT)(-2144861870);

	public static readonly HRESULT TPM_20_E_NEEDS_TEST = (HRESULT)(-2144861869);

	public static readonly HRESULT TPM_20_E_NO_RESULT = (HRESULT)(-2144861868);

	public static readonly HRESULT TPM_20_E_SENSITIVE = (HRESULT)(-2144861867);

	public static readonly HRESULT TPM_E_COMMAND_BLOCKED = (HRESULT)(-2144861184);

	public static readonly HRESULT TPM_E_INVALID_HANDLE = (HRESULT)(-2144861183);

	public static readonly HRESULT TPM_E_DUPLICATE_VHANDLE = (HRESULT)(-2144861182);

	public static readonly HRESULT TPM_E_EMBEDDED_COMMAND_BLOCKED = (HRESULT)(-2144861181);

	public static readonly HRESULT TPM_E_EMBEDDED_COMMAND_UNSUPPORTED = (HRESULT)(-2144861180);

	public static readonly HRESULT TPM_E_RETRY = (HRESULT)(-2144860160);

	public static readonly HRESULT TPM_E_NEEDS_SELFTEST = (HRESULT)(-2144860159);

	public static readonly HRESULT TPM_E_DOING_SELFTEST = (HRESULT)(-2144860158);

	public static readonly HRESULT TPM_E_DEFEND_LOCK_RUNNING = (HRESULT)(-2144860157);

	public static readonly HRESULT TPM_20_E_CONTEXT_GAP = (HRESULT)(-2144859903);

	public static readonly HRESULT TPM_20_E_OBJECT_MEMORY = (HRESULT)(-2144859902);

	public static readonly HRESULT TPM_20_E_SESSION_MEMORY = (HRESULT)(-2144859901);

	public static readonly HRESULT TPM_20_E_MEMORY = (HRESULT)(-2144859900);

	public static readonly HRESULT TPM_20_E_SESSION_HANDLES = (HRESULT)(-2144859899);

	public static readonly HRESULT TPM_20_E_OBJECT_HANDLES = (HRESULT)(-2144859898);

	public static readonly HRESULT TPM_20_E_LOCALITY = (HRESULT)(-2144859897);

	public static readonly HRESULT TPM_20_E_YIELDED = (HRESULT)(-2144859896);

	public static readonly HRESULT TPM_20_E_CANCELED = (HRESULT)(-2144859895);

	public static readonly HRESULT TPM_20_E_TESTING = (HRESULT)(-2144859894);

	public static readonly HRESULT TPM_20_E_NV_RATE = (HRESULT)(-2144859872);

	public static readonly HRESULT TPM_20_E_LOCKOUT = (HRESULT)(-2144859871);

	public static readonly HRESULT TPM_20_E_RETRY = (HRESULT)(-2144859870);

	public static readonly HRESULT TPM_20_E_NV_UNAVAILABLE = (HRESULT)(-2144859869);

	public static readonly HRESULT TBS_E_INTERNAL_ERROR = (HRESULT)(-2144845823);

	public static readonly HRESULT TBS_E_BAD_PARAMETER = (HRESULT)(-2144845822);

	public static readonly HRESULT TBS_E_INVALID_OUTPUT_POINTER = (HRESULT)(-2144845821);

	public static readonly HRESULT TBS_E_INVALID_CONTEXT = (HRESULT)(-2144845820);

	public static readonly HRESULT TBS_E_INSUFFICIENT_BUFFER = (HRESULT)(-2144845819);

	public static readonly HRESULT TBS_E_IOERROR = (HRESULT)(-2144845818);

	public static readonly HRESULT TBS_E_INVALID_CONTEXT_PARAM = (HRESULT)(-2144845817);

	public static readonly HRESULT TBS_E_SERVICE_NOT_RUNNING = (HRESULT)(-2144845816);

	public static readonly HRESULT TBS_E_TOO_MANY_TBS_CONTEXTS = (HRESULT)(-2144845815);

	public static readonly HRESULT TBS_E_TOO_MANY_RESOURCES = (HRESULT)(-2144845814);

	public static readonly HRESULT TBS_E_SERVICE_START_PENDING = (HRESULT)(-2144845813);

	public static readonly HRESULT TBS_E_PPI_NOT_SUPPORTED = (HRESULT)(-2144845812);

	public static readonly HRESULT TBS_E_COMMAND_CANCELED = (HRESULT)(-2144845811);

	public static readonly HRESULT TBS_E_BUFFER_TOO_LARGE = (HRESULT)(-2144845810);

	public static readonly HRESULT TBS_E_TPM_NOT_FOUND = (HRESULT)(-2144845809);

	public static readonly HRESULT TBS_E_SERVICE_DISABLED = (HRESULT)(-2144845808);

	public static readonly HRESULT TBS_E_NO_EVENT_LOG = (HRESULT)(-2144845807);

	public static readonly HRESULT TBS_E_ACCESS_DENIED = (HRESULT)(-2144845806);

	public static readonly HRESULT TBS_E_PROVISIONING_NOT_ALLOWED = (HRESULT)(-2144845805);

	public static readonly HRESULT TBS_E_PPI_FUNCTION_UNSUPPORTED = (HRESULT)(-2144845804);

	public static readonly HRESULT TBS_E_OWNERAUTH_NOT_FOUND = (HRESULT)(-2144845803);

	public static readonly HRESULT TBS_E_PROVISIONING_INCOMPLETE = (HRESULT)(-2144845802);

	public static readonly HRESULT TPMAPI_E_INVALID_STATE = (HRESULT)(-2144796416);

	public static readonly HRESULT TPMAPI_E_NOT_ENOUGH_DATA = (HRESULT)(-2144796415);

	public static readonly HRESULT TPMAPI_E_TOO_MUCH_DATA = (HRESULT)(-2144796414);

	public static readonly HRESULT TPMAPI_E_INVALID_OUTPUT_POINTER = (HRESULT)(-2144796413);

	public static readonly HRESULT TPMAPI_E_INVALID_PARAMETER = (HRESULT)(-2144796412);

	public static readonly HRESULT TPMAPI_E_OUT_OF_MEMORY = (HRESULT)(-2144796411);

	public static readonly HRESULT TPMAPI_E_BUFFER_TOO_SMALL = (HRESULT)(-2144796410);

	public static readonly HRESULT TPMAPI_E_INTERNAL_ERROR = (HRESULT)(-2144796409);

	public static readonly HRESULT TPMAPI_E_ACCESS_DENIED = (HRESULT)(-2144796408);

	public static readonly HRESULT TPMAPI_E_AUTHORIZATION_FAILED = (HRESULT)(-2144796407);

	public static readonly HRESULT TPMAPI_E_INVALID_CONTEXT_HANDLE = (HRESULT)(-2144796406);

	public static readonly HRESULT TPMAPI_E_TBS_COMMUNICATION_ERROR = (HRESULT)(-2144796405);

	public static readonly HRESULT TPMAPI_E_TPM_COMMAND_ERROR = (HRESULT)(-2144796404);

	public static readonly HRESULT TPMAPI_E_MESSAGE_TOO_LARGE = (HRESULT)(-2144796403);

	public static readonly HRESULT TPMAPI_E_INVALID_ENCODING = (HRESULT)(-2144796402);

	public static readonly HRESULT TPMAPI_E_INVALID_KEY_SIZE = (HRESULT)(-2144796401);

	public static readonly HRESULT TPMAPI_E_ENCRYPTION_FAILED = (HRESULT)(-2144796400);

	public static readonly HRESULT TPMAPI_E_INVALID_KEY_PARAMS = (HRESULT)(-2144796399);

	public static readonly HRESULT TPMAPI_E_INVALID_MIGRATION_AUTHORIZATION_BLOB = (HRESULT)(-2144796398);

	public static readonly HRESULT TPMAPI_E_INVALID_PCR_INDEX = (HRESULT)(-2144796397);

	public static readonly HRESULT TPMAPI_E_INVALID_DELEGATE_BLOB = (HRESULT)(-2144796396);

	public static readonly HRESULT TPMAPI_E_INVALID_CONTEXT_PARAMS = (HRESULT)(-2144796395);

	public static readonly HRESULT TPMAPI_E_INVALID_KEY_BLOB = (HRESULT)(-2144796394);

	public static readonly HRESULT TPMAPI_E_INVALID_PCR_DATA = (HRESULT)(-2144796393);

	public static readonly HRESULT TPMAPI_E_INVALID_OWNER_AUTH = (HRESULT)(-2144796392);

	public static readonly HRESULT TPMAPI_E_FIPS_RNG_CHECK_FAILED = (HRESULT)(-2144796391);

	public static readonly HRESULT TPMAPI_E_EMPTY_TCG_LOG = (HRESULT)(-2144796390);

	public static readonly HRESULT TPMAPI_E_INVALID_TCG_LOG_ENTRY = (HRESULT)(-2144796389);

	public static readonly HRESULT TPMAPI_E_TCG_SEPARATOR_ABSENT = (HRESULT)(-2144796388);

	public static readonly HRESULT TPMAPI_E_TCG_INVALID_DIGEST_ENTRY = (HRESULT)(-2144796387);

	public static readonly HRESULT TPMAPI_E_POLICY_DENIES_OPERATION = (HRESULT)(-2144796386);

	public static readonly HRESULT TPMAPI_E_NV_BITS_NOT_DEFINED = (HRESULT)(-2144796385);

	public static readonly HRESULT TPMAPI_E_NV_BITS_NOT_READY = (HRESULT)(-2144796384);

	public static readonly HRESULT TPMAPI_E_SEALING_KEY_NOT_AVAILABLE = (HRESULT)(-2144796383);

	public static readonly HRESULT TPMAPI_E_NO_AUTHORIZATION_CHAIN_FOUND = (HRESULT)(-2144796382);

	public static readonly HRESULT TPMAPI_E_SVN_COUNTER_NOT_AVAILABLE = (HRESULT)(-2144796381);

	public static readonly HRESULT TPMAPI_E_OWNER_AUTH_NOT_NULL = (HRESULT)(-2144796380);

	public static readonly HRESULT TPMAPI_E_ENDORSEMENT_AUTH_NOT_NULL = (HRESULT)(-2144796379);

	public static readonly HRESULT TPMAPI_E_AUTHORIZATION_REVOKED = (HRESULT)(-2144796378);

	public static readonly HRESULT TPMAPI_E_MALFORMED_AUTHORIZATION_KEY = (HRESULT)(-2144796377);

	public static readonly HRESULT TPMAPI_E_AUTHORIZING_KEY_NOT_SUPPORTED = (HRESULT)(-2144796376);

	public static readonly HRESULT TPMAPI_E_INVALID_AUTHORIZATION_SIGNATURE = (HRESULT)(-2144796375);

	public static readonly HRESULT TPMAPI_E_MALFORMED_AUTHORIZATION_POLICY = (HRESULT)(-2144796374);

	public static readonly HRESULT TPMAPI_E_MALFORMED_AUTHORIZATION_OTHER = (HRESULT)(-2144796373);

	public static readonly HRESULT TPMAPI_E_SEALING_KEY_CHANGED = (HRESULT)(-2144796372);

	public static readonly HRESULT TPMAPI_E_INVALID_TPM_VERSION = (HRESULT)(-2144796371);

	public static readonly HRESULT TPMAPI_E_INVALID_POLICYAUTH_BLOB_TYPE = (HRESULT)(-2144796370);

	public static readonly HRESULT TBSIMP_E_BUFFER_TOO_SMALL = (HRESULT)(-2144796160);

	public static readonly HRESULT TBSIMP_E_CLEANUP_FAILED = (HRESULT)(-2144796159);

	public static readonly HRESULT TBSIMP_E_INVALID_CONTEXT_HANDLE = (HRESULT)(-2144796158);

	public static readonly HRESULT TBSIMP_E_INVALID_CONTEXT_PARAM = (HRESULT)(-2144796157);

	public static readonly HRESULT TBSIMP_E_TPM_ERROR = (HRESULT)(-2144796156);

	public static readonly HRESULT TBSIMP_E_HASH_BAD_KEY = (HRESULT)(-2144796155);

	public static readonly HRESULT TBSIMP_E_DUPLICATE_VHANDLE = (HRESULT)(-2144796154);

	public static readonly HRESULT TBSIMP_E_INVALID_OUTPUT_POINTER = (HRESULT)(-2144796153);

	public static readonly HRESULT TBSIMP_E_INVALID_PARAMETER = (HRESULT)(-2144796152);

	public static readonly HRESULT TBSIMP_E_RPC_INIT_FAILED = (HRESULT)(-2144796151);

	public static readonly HRESULT TBSIMP_E_SCHEDULER_NOT_RUNNING = (HRESULT)(-2144796150);

	public static readonly HRESULT TBSIMP_E_COMMAND_CANCELED = (HRESULT)(-2144796149);

	public static readonly HRESULT TBSIMP_E_OUT_OF_MEMORY = (HRESULT)(-2144796148);

	public static readonly HRESULT TBSIMP_E_LIST_NO_MORE_ITEMS = (HRESULT)(-2144796147);

	public static readonly HRESULT TBSIMP_E_LIST_NOT_FOUND = (HRESULT)(-2144796146);

	public static readonly HRESULT TBSIMP_E_NOT_ENOUGH_SPACE = (HRESULT)(-2144796145);

	public static readonly HRESULT TBSIMP_E_NOT_ENOUGH_TPM_CONTEXTS = (HRESULT)(-2144796144);

	public static readonly HRESULT TBSIMP_E_COMMAND_FAILED = (HRESULT)(-2144796143);

	public static readonly HRESULT TBSIMP_E_UNKNOWN_ORDINAL = (HRESULT)(-2144796142);

	public static readonly HRESULT TBSIMP_E_RESOURCE_EXPIRED = (HRESULT)(-2144796141);

	public static readonly HRESULT TBSIMP_E_INVALID_RESOURCE = (HRESULT)(-2144796140);

	public static readonly HRESULT TBSIMP_E_NOTHING_TO_UNLOAD = (HRESULT)(-2144796139);

	public static readonly HRESULT TBSIMP_E_HASH_TABLE_FULL = (HRESULT)(-2144796138);

	public static readonly HRESULT TBSIMP_E_TOO_MANY_TBS_CONTEXTS = (HRESULT)(-2144796137);

	public static readonly HRESULT TBSIMP_E_TOO_MANY_RESOURCES = (HRESULT)(-2144796136);

	public static readonly HRESULT TBSIMP_E_PPI_NOT_SUPPORTED = (HRESULT)(-2144796135);

	public static readonly HRESULT TBSIMP_E_TPM_INCOMPATIBLE = (HRESULT)(-2144796134);

	public static readonly HRESULT TBSIMP_E_NO_EVENT_LOG = (HRESULT)(-2144796133);

	public static readonly HRESULT TPM_E_PPI_ACPI_FAILURE = (HRESULT)(-2144795904);

	public static readonly HRESULT TPM_E_PPI_USER_ABORT = (HRESULT)(-2144795903);

	public static readonly HRESULT TPM_E_PPI_BIOS_FAILURE = (HRESULT)(-2144795902);

	public static readonly HRESULT TPM_E_PPI_NOT_SUPPORTED = (HRESULT)(-2144795901);

	public static readonly HRESULT TPM_E_PPI_BLOCKED_IN_BIOS = (HRESULT)(-2144795900);

	public static readonly HRESULT TPM_E_PCP_ERROR_MASK = (HRESULT)(-2144795648);

	public static readonly HRESULT TPM_E_PCP_DEVICE_NOT_READY = (HRESULT)(-2144795647);

	public static readonly HRESULT TPM_E_PCP_INVALID_HANDLE = (HRESULT)(-2144795646);

	public static readonly HRESULT TPM_E_PCP_INVALID_PARAMETER = (HRESULT)(-2144795645);

	public static readonly HRESULT TPM_E_PCP_FLAG_NOT_SUPPORTED = (HRESULT)(-2144795644);

	public static readonly HRESULT TPM_E_PCP_NOT_SUPPORTED = (HRESULT)(-2144795643);

	public static readonly HRESULT TPM_E_PCP_BUFFER_TOO_SMALL = (HRESULT)(-2144795642);

	public static readonly HRESULT TPM_E_PCP_INTERNAL_ERROR = (HRESULT)(-2144795641);

	public static readonly HRESULT TPM_E_PCP_AUTHENTICATION_FAILED = (HRESULT)(-2144795640);

	public static readonly HRESULT TPM_E_PCP_AUTHENTICATION_IGNORED = (HRESULT)(-2144795639);

	public static readonly HRESULT TPM_E_PCP_POLICY_NOT_FOUND = (HRESULT)(-2144795638);

	public static readonly HRESULT TPM_E_PCP_PROFILE_NOT_FOUND = (HRESULT)(-2144795637);

	public static readonly HRESULT TPM_E_PCP_VALIDATION_FAILED = (HRESULT)(-2144795636);

	public static readonly HRESULT TPM_E_PCP_WRONG_PARENT = (HRESULT)(-2144795634);

	public static readonly HRESULT TPM_E_KEY_NOT_LOADED = (HRESULT)(-2144795633);

	public static readonly HRESULT TPM_E_NO_KEY_CERTIFICATION = (HRESULT)(-2144795632);

	public static readonly HRESULT TPM_E_KEY_NOT_FINALIZED = (HRESULT)(-2144795631);

	public static readonly HRESULT TPM_E_ATTESTATION_CHALLENGE_NOT_SET = (HRESULT)(-2144795630);

	public static readonly HRESULT TPM_E_NOT_PCR_BOUND = (HRESULT)(-2144795629);

	public static readonly HRESULT TPM_E_KEY_ALREADY_FINALIZED = (HRESULT)(-2144795628);

	public static readonly HRESULT TPM_E_KEY_USAGE_POLICY_NOT_SUPPORTED = (HRESULT)(-2144795627);

	public static readonly HRESULT TPM_E_KEY_USAGE_POLICY_INVALID = (HRESULT)(-2144795626);

	public static readonly HRESULT TPM_E_SOFT_KEY_ERROR = (HRESULT)(-2144795625);

	public static readonly HRESULT TPM_E_KEY_NOT_AUTHENTICATED = (HRESULT)(-2144795624);

	public static readonly HRESULT TPM_E_PCP_KEY_NOT_AIK = (HRESULT)(-2144795623);

	public static readonly HRESULT TPM_E_KEY_NOT_SIGNING_KEY = (HRESULT)(-2144795622);

	public static readonly HRESULT TPM_E_LOCKED_OUT = (HRESULT)(-2144795621);

	public static readonly HRESULT TPM_E_CLAIM_TYPE_NOT_SUPPORTED = (HRESULT)(-2144795620);

	public static readonly HRESULT TPM_E_VERSION_NOT_SUPPORTED = (HRESULT)(-2144795619);

	public static readonly HRESULT TPM_E_BUFFER_LENGTH_MISMATCH = (HRESULT)(-2144795618);

	public static readonly HRESULT TPM_E_PCP_IFX_RSA_KEY_CREATION_BLOCKED = (HRESULT)(-2144795617);

	public static readonly HRESULT TPM_E_PCP_TICKET_MISSING = (HRESULT)(-2144795616);

	public static readonly HRESULT TPM_E_PCP_RAW_POLICY_NOT_SUPPORTED = (HRESULT)(-2144795615);

	public static readonly HRESULT TPM_E_PCP_KEY_HANDLE_INVALIDATED = (HRESULT)(-2144795614);

	public static readonly HRESULT TPM_E_PCP_UNSUPPORTED_PSS_SALT = (HRESULT)(1076429859);

	public static readonly HRESULT TPM_E_PCP_PLATFORM_CLAIM_MAY_BE_OUTDATED = (HRESULT)(1076429860);

	public static readonly HRESULT TPM_E_PCP_PLATFORM_CLAIM_OUTDATED = (HRESULT)(1076429861);

	public static readonly HRESULT TPM_E_PCP_PLATFORM_CLAIM_REBOOT = (HRESULT)(1076429862);

	public static readonly HRESULT TPM_E_ZERO_EXHAUST_ENABLED = (HRESULT)(-2144795392);

	public static readonly HRESULT TPM_E_PROVISIONING_INCOMPLETE = (HRESULT)(-2144795136);

	public static readonly HRESULT TPM_E_INVALID_OWNER_AUTH = (HRESULT)(-2144795135);

	public static readonly HRESULT TPM_E_TOO_MUCH_DATA = (HRESULT)(-2144795134);

	public static readonly HRESULT TPM_E_TPM_GENERATED_EPS = (HRESULT)(-2144795133);

	public static readonly HRESULT PLA_E_DCS_NOT_FOUND = (HRESULT)(-2144337918);

	public static readonly HRESULT PLA_E_DCS_IN_USE = (HRESULT)(-2144337750);

	public static readonly HRESULT PLA_E_TOO_MANY_FOLDERS = (HRESULT)(-2144337851);

	public static readonly HRESULT PLA_E_NO_MIN_DISK = (HRESULT)(-2144337808);

	public static readonly HRESULT PLA_E_DCS_ALREADY_EXISTS = (HRESULT)(-2144337737);

	public static readonly HRESULT PLA_S_PROPERTY_IGNORED = (HRESULT)(3145984);

	public static readonly HRESULT PLA_E_PROPERTY_CONFLICT = (HRESULT)(-2144337663);

	public static readonly HRESULT PLA_E_DCS_SINGLETON_REQUIRED = (HRESULT)(-2144337662);

	public static readonly HRESULT PLA_E_CREDENTIALS_REQUIRED = (HRESULT)(-2144337661);

	public static readonly HRESULT PLA_E_DCS_NOT_RUNNING = (HRESULT)(-2144337660);

	public static readonly HRESULT PLA_E_CONFLICT_INCL_EXCL_API = (HRESULT)(-2144337659);

	public static readonly HRESULT PLA_E_NETWORK_EXE_NOT_VALID = (HRESULT)(-2144337658);

	public static readonly HRESULT PLA_E_EXE_ALREADY_CONFIGURED = (HRESULT)(-2144337657);

	public static readonly HRESULT PLA_E_EXE_PATH_NOT_VALID = (HRESULT)(-2144337656);

	public static readonly HRESULT PLA_E_DC_ALREADY_EXISTS = (HRESULT)(-2144337655);

	public static readonly HRESULT PLA_E_DCS_START_WAIT_TIMEOUT = (HRESULT)(-2144337654);

	public static readonly HRESULT PLA_E_DC_START_WAIT_TIMEOUT = (HRESULT)(-2144337653);

	public static readonly HRESULT PLA_E_REPORT_WAIT_TIMEOUT = (HRESULT)(-2144337652);

	public static readonly HRESULT PLA_E_NO_DUPLICATES = (HRESULT)(-2144337651);

	public static readonly HRESULT PLA_E_EXE_FULL_PATH_REQUIRED = (HRESULT)(-2144337650);

	public static readonly HRESULT PLA_E_INVALID_SESSION_NAME = (HRESULT)(-2144337649);

	public static readonly HRESULT PLA_E_PLA_CHANNEL_NOT_ENABLED = (HRESULT)(-2144337648);

	public static readonly HRESULT PLA_E_TASKSCHED_CHANNEL_NOT_ENABLED = (HRESULT)(-2144337647);

	public static readonly HRESULT PLA_E_RULES_MANAGER_FAILED = (HRESULT)(-2144337646);

	public static readonly HRESULT PLA_E_CABAPI_FAILURE = (HRESULT)(-2144337645);

	public static readonly HRESULT FVE_E_LOCKED_VOLUME = (HRESULT)(-2144272384);

	public static readonly HRESULT FVE_E_NOT_ENCRYPTED = (HRESULT)(-2144272383);

	public static readonly HRESULT FVE_E_NO_TPM_BIOS = (HRESULT)(-2144272382);

	public static readonly HRESULT FVE_E_NO_MBR_METRIC = (HRESULT)(-2144272381);

	public static readonly HRESULT FVE_E_NO_BOOTSECTOR_METRIC = (HRESULT)(-2144272380);

	public static readonly HRESULT FVE_E_NO_BOOTMGR_METRIC = (HRESULT)(-2144272379);

	public static readonly HRESULT FVE_E_WRONG_BOOTMGR = (HRESULT)(-2144272378);

	public static readonly HRESULT FVE_E_SECURE_KEY_REQUIRED = (HRESULT)(-2144272377);

	public static readonly HRESULT FVE_E_NOT_ACTIVATED = (HRESULT)(-2144272376);

	public static readonly HRESULT FVE_E_ACTION_NOT_ALLOWED = (HRESULT)(-2144272375);

	public static readonly HRESULT FVE_E_AD_SCHEMA_NOT_INSTALLED = (HRESULT)(-2144272374);

	public static readonly HRESULT FVE_E_AD_INVALID_DATATYPE = (HRESULT)(-2144272373);

	public static readonly HRESULT FVE_E_AD_INVALID_DATASIZE = (HRESULT)(-2144272372);

	public static readonly HRESULT FVE_E_AD_NO_VALUES = (HRESULT)(-2144272371);

	public static readonly HRESULT FVE_E_AD_ATTR_NOT_SET = (HRESULT)(-2144272370);

	public static readonly HRESULT FVE_E_AD_GUID_NOT_FOUND = (HRESULT)(-2144272369);

	public static readonly HRESULT FVE_E_BAD_INFORMATION = (HRESULT)(-2144272368);

	public static readonly HRESULT FVE_E_TOO_SMALL = (HRESULT)(-2144272367);

	public static readonly HRESULT FVE_E_SYSTEM_VOLUME = (HRESULT)(-2144272366);

	public static readonly HRESULT FVE_E_FAILED_WRONG_FS = (HRESULT)(-2144272365);

	public static readonly HRESULT FVE_E_BAD_PARTITION_SIZE = (HRESULT)(-2144272364);

	public static readonly HRESULT FVE_E_NOT_SUPPORTED = (HRESULT)(-2144272363);

	public static readonly HRESULT FVE_E_BAD_DATA = (HRESULT)(-2144272362);

	public static readonly HRESULT FVE_E_VOLUME_NOT_BOUND = (HRESULT)(-2144272361);

	public static readonly HRESULT FVE_E_TPM_NOT_OWNED = (HRESULT)(-2144272360);

	public static readonly HRESULT FVE_E_NOT_DATA_VOLUME = (HRESULT)(-2144272359);

	public static readonly HRESULT FVE_E_AD_INSUFFICIENT_BUFFER = (HRESULT)(-2144272358);

	public static readonly HRESULT FVE_E_CONV_READ = (HRESULT)(-2144272357);

	public static readonly HRESULT FVE_E_CONV_WRITE = (HRESULT)(-2144272356);

	public static readonly HRESULT FVE_E_KEY_REQUIRED = (HRESULT)(-2144272355);

	public static readonly HRESULT FVE_E_CLUSTERING_NOT_SUPPORTED = (HRESULT)(-2144272354);

	public static readonly HRESULT FVE_E_VOLUME_BOUND_ALREADY = (HRESULT)(-2144272353);

	public static readonly HRESULT FVE_E_OS_NOT_PROTECTED = (HRESULT)(-2144272352);

	public static readonly HRESULT FVE_E_PROTECTION_DISABLED = (HRESULT)(-2144272351);

	public static readonly HRESULT FVE_E_RECOVERY_KEY_REQUIRED = (HRESULT)(-2144272350);

	public static readonly HRESULT FVE_E_FOREIGN_VOLUME = (HRESULT)(-2144272349);

	public static readonly HRESULT FVE_E_OVERLAPPED_UPDATE = (HRESULT)(-2144272348);

	public static readonly HRESULT FVE_E_TPM_SRK_AUTH_NOT_ZERO = (HRESULT)(-2144272347);

	public static readonly HRESULT FVE_E_FAILED_SECTOR_SIZE = (HRESULT)(-2144272346);

	public static readonly HRESULT FVE_E_FAILED_AUTHENTICATION = (HRESULT)(-2144272345);

	public static readonly HRESULT FVE_E_NOT_OS_VOLUME = (HRESULT)(-2144272344);

	public static readonly HRESULT FVE_E_AUTOUNLOCK_ENABLED = (HRESULT)(-2144272343);

	public static readonly HRESULT FVE_E_WRONG_BOOTSECTOR = (HRESULT)(-2144272342);

	public static readonly HRESULT FVE_E_WRONG_SYSTEM_FS = (HRESULT)(-2144272341);

	public static readonly HRESULT FVE_E_POLICY_PASSWORD_REQUIRED = (HRESULT)(-2144272340);

	public static readonly HRESULT FVE_E_CANNOT_SET_FVEK_ENCRYPTED = (HRESULT)(-2144272339);

	public static readonly HRESULT FVE_E_CANNOT_ENCRYPT_NO_KEY = (HRESULT)(-2144272338);

	public static readonly HRESULT FVE_E_BOOTABLE_CDDVD = (HRESULT)(-2144272336);

	public static readonly HRESULT FVE_E_PROTECTOR_EXISTS = (HRESULT)(-2144272335);

	public static readonly HRESULT FVE_E_RELATIVE_PATH = (HRESULT)(-2144272334);

	public static readonly HRESULT FVE_E_PROTECTOR_NOT_FOUND = (HRESULT)(-2144272333);

	public static readonly HRESULT FVE_E_INVALID_KEY_FORMAT = (HRESULT)(-2144272332);

	public static readonly HRESULT FVE_E_INVALID_PASSWORD_FORMAT = (HRESULT)(-2144272331);

	public static readonly HRESULT FVE_E_FIPS_RNG_CHECK_FAILED = (HRESULT)(-2144272330);

	public static readonly HRESULT FVE_E_FIPS_PREVENTS_RECOVERY_PASSWORD = (HRESULT)(-2144272329);

	public static readonly HRESULT FVE_E_FIPS_PREVENTS_EXTERNAL_KEY_EXPORT = (HRESULT)(-2144272328);

	public static readonly HRESULT FVE_E_NOT_DECRYPTED = (HRESULT)(-2144272327);

	public static readonly HRESULT FVE_E_INVALID_PROTECTOR_TYPE = (HRESULT)(-2144272326);

	public static readonly HRESULT FVE_E_NO_PROTECTORS_TO_TEST = (HRESULT)(-2144272325);

	public static readonly HRESULT FVE_E_KEYFILE_NOT_FOUND = (HRESULT)(-2144272324);

	public static readonly HRESULT FVE_E_KEYFILE_INVALID = (HRESULT)(-2144272323);

	public static readonly HRESULT FVE_E_KEYFILE_NO_VMK = (HRESULT)(-2144272322);

	public static readonly HRESULT FVE_E_TPM_DISABLED = (HRESULT)(-2144272321);

	public static readonly HRESULT FVE_E_NOT_ALLOWED_IN_SAFE_MODE = (HRESULT)(-2144272320);

	public static readonly HRESULT FVE_E_TPM_INVALID_PCR = (HRESULT)(-2144272319);

	public static readonly HRESULT FVE_E_TPM_NO_VMK = (HRESULT)(-2144272318);

	public static readonly HRESULT FVE_E_PIN_INVALID = (HRESULT)(-2144272317);

	public static readonly HRESULT FVE_E_AUTH_INVALID_APPLICATION = (HRESULT)(-2144272316);

	public static readonly HRESULT FVE_E_AUTH_INVALID_CONFIG = (HRESULT)(-2144272315);

	public static readonly HRESULT FVE_E_FIPS_DISABLE_PROTECTION_NOT_ALLOWED = (HRESULT)(-2144272314);

	public static readonly HRESULT FVE_E_FS_NOT_EXTENDED = (HRESULT)(-2144272313);

	public static readonly HRESULT FVE_E_FIRMWARE_TYPE_NOT_SUPPORTED = (HRESULT)(-2144272312);

	public static readonly HRESULT FVE_E_NO_LICENSE = (HRESULT)(-2144272311);

	public static readonly HRESULT FVE_E_NOT_ON_STACK = (HRESULT)(-2144272310);

	public static readonly HRESULT FVE_E_FS_MOUNTED = (HRESULT)(-2144272309);

	public static readonly HRESULT FVE_E_TOKEN_NOT_IMPERSONATED = (HRESULT)(-2144272308);

	public static readonly HRESULT FVE_E_DRY_RUN_FAILED = (HRESULT)(-2144272307);

	public static readonly HRESULT FVE_E_REBOOT_REQUIRED = (HRESULT)(-2144272306);

	public static readonly HRESULT FVE_E_DEBUGGER_ENABLED = (HRESULT)(-2144272305);

	public static readonly HRESULT FVE_E_RAW_ACCESS = (HRESULT)(-2144272304);

	public static readonly HRESULT FVE_E_RAW_BLOCKED = (HRESULT)(-2144272303);

	public static readonly HRESULT FVE_E_BCD_APPLICATIONS_PATH_INCORRECT = (HRESULT)(-2144272302);

	public static readonly HRESULT FVE_E_NOT_ALLOWED_IN_VERSION = (HRESULT)(-2144272301);

	public static readonly HRESULT FVE_E_NO_AUTOUNLOCK_MASTER_KEY = (HRESULT)(-2144272300);

	public static readonly HRESULT FVE_E_MOR_FAILED = (HRESULT)(-2144272299);

	public static readonly HRESULT FVE_E_HIDDEN_VOLUME = (HRESULT)(-2144272298);

	public static readonly HRESULT FVE_E_TRANSIENT_STATE = (HRESULT)(-2144272297);

	public static readonly HRESULT FVE_E_PUBKEY_NOT_ALLOWED = (HRESULT)(-2144272296);

	public static readonly HRESULT FVE_E_VOLUME_HANDLE_OPEN = (HRESULT)(-2144272295);

	public static readonly HRESULT FVE_E_NO_FEATURE_LICENSE = (HRESULT)(-2144272294);

	public static readonly HRESULT FVE_E_INVALID_STARTUP_OPTIONS = (HRESULT)(-2144272293);

	public static readonly HRESULT FVE_E_POLICY_RECOVERY_PASSWORD_NOT_ALLOWED = (HRESULT)(-2144272292);

	public static readonly HRESULT FVE_E_POLICY_RECOVERY_PASSWORD_REQUIRED = (HRESULT)(-2144272291);

	public static readonly HRESULT FVE_E_POLICY_RECOVERY_KEY_NOT_ALLOWED = (HRESULT)(-2144272290);

	public static readonly HRESULT FVE_E_POLICY_RECOVERY_KEY_REQUIRED = (HRESULT)(-2144272289);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_PIN_NOT_ALLOWED = (HRESULT)(-2144272288);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_PIN_REQUIRED = (HRESULT)(-2144272287);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_KEY_NOT_ALLOWED = (HRESULT)(-2144272286);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_KEY_REQUIRED = (HRESULT)(-2144272285);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_PIN_KEY_NOT_ALLOWED = (HRESULT)(-2144272284);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_PIN_KEY_REQUIRED = (HRESULT)(-2144272283);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_TPM_NOT_ALLOWED = (HRESULT)(-2144272282);

	public static readonly HRESULT FVE_E_POLICY_STARTUP_TPM_REQUIRED = (HRESULT)(-2144272281);

	public static readonly HRESULT FVE_E_POLICY_INVALID_PIN_LENGTH = (HRESULT)(-2144272280);

	public static readonly HRESULT FVE_E_KEY_PROTECTOR_NOT_SUPPORTED = (HRESULT)(-2144272279);

	public static readonly HRESULT FVE_E_POLICY_PASSPHRASE_NOT_ALLOWED = (HRESULT)(-2144272278);

	public static readonly HRESULT FVE_E_POLICY_PASSPHRASE_REQUIRED = (HRESULT)(-2144272277);

	public static readonly HRESULT FVE_E_FIPS_PREVENTS_PASSPHRASE = (HRESULT)(-2144272276);

	public static readonly HRESULT FVE_E_OS_VOLUME_PASSPHRASE_NOT_ALLOWED = (HRESULT)(-2144272275);

	public static readonly HRESULT FVE_E_INVALID_BITLOCKER_OID = (HRESULT)(-2144272274);

	public static readonly HRESULT FVE_E_VOLUME_TOO_SMALL = (HRESULT)(-2144272273);

	public static readonly HRESULT FVE_E_DV_NOT_SUPPORTED_ON_FS = (HRESULT)(-2144272272);

	public static readonly HRESULT FVE_E_DV_NOT_ALLOWED_BY_GP = (HRESULT)(-2144272271);

	public static readonly HRESULT FVE_E_POLICY_USER_CERTIFICATE_NOT_ALLOWED = (HRESULT)(-2144272270);

	public static readonly HRESULT FVE_E_POLICY_USER_CERTIFICATE_REQUIRED = (HRESULT)(-2144272269);

	public static readonly HRESULT FVE_E_POLICY_USER_CERT_MUST_BE_HW = (HRESULT)(-2144272268);

	public static readonly HRESULT FVE_E_POLICY_USER_CONFIGURE_FDV_AUTOUNLOCK_NOT_ALLOWED = (HRESULT)(-2144272267);

	public static readonly HRESULT FVE_E_POLICY_USER_CONFIGURE_RDV_AUTOUNLOCK_NOT_ALLOWED = (HRESULT)(-2144272266);

	public static readonly HRESULT FVE_E_POLICY_USER_CONFIGURE_RDV_NOT_ALLOWED = (HRESULT)(-2144272265);

	public static readonly HRESULT FVE_E_POLICY_USER_ENABLE_RDV_NOT_ALLOWED = (HRESULT)(-2144272264);

	public static readonly HRESULT FVE_E_POLICY_USER_DISABLE_RDV_NOT_ALLOWED = (HRESULT)(-2144272263);

	public static readonly HRESULT FVE_E_POLICY_INVALID_PASSPHRASE_LENGTH = (HRESULT)(-2144272256);

	public static readonly HRESULT FVE_E_POLICY_PASSPHRASE_TOO_SIMPLE = (HRESULT)(-2144272255);

	public static readonly HRESULT FVE_E_RECOVERY_PARTITION = (HRESULT)(-2144272254);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_FDV_RK_OFF_AUK_ON = (HRESULT)(-2144272253);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_RDV_RK_OFF_AUK_ON = (HRESULT)(-2144272252);

	public static readonly HRESULT FVE_E_NON_BITLOCKER_OID = (HRESULT)(-2144272251);

	public static readonly HRESULT FVE_E_POLICY_PROHIBITS_SELFSIGNED = (HRESULT)(-2144272250);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_RO_AND_STARTUP_KEY_REQUIRED = (HRESULT)(-2144272249);

	public static readonly HRESULT FVE_E_CONV_RECOVERY_FAILED = (HRESULT)(-2144272248);

	public static readonly HRESULT FVE_E_VIRTUALIZED_SPACE_TOO_BIG = (HRESULT)(-2144272247);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_OSV_RP_OFF_ADB_ON = (HRESULT)(-2144272240);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_FDV_RP_OFF_ADB_ON = (HRESULT)(-2144272239);

	public static readonly HRESULT FVE_E_POLICY_CONFLICT_RDV_RP_OFF_ADB_ON = (HRESULT)(-2144272238);

	public static readonly HRESULT FVE_E_NON_BITLOCKER_KU = (HRESULT)(-2144272237);

	public static readonly HRESULT FVE_E_PRIVATEKEY_AUTH_FAILED = (HRESULT)(-2144272236);

	public static readonly HRESULT FVE_E_REMOVAL_OF_DRA_FAILED = (HRESULT)(-2144272235);

	public static readonly HRESULT FVE_E_OPERATION_NOT_SUPPORTED_ON_VISTA_VOLUME = (HRESULT)(-2144272234);

	public static readonly HRESULT FVE_E_CANT_LOCK_AUTOUNLOCK_ENABLED_VOLUME = (HRESULT)(-2144272233);

	public static readonly HRESULT FVE_E_FIPS_HASH_KDF_NOT_ALLOWED = (HRESULT)(-2144272232);

	public static readonly HRESULT FVE_E_ENH_PIN_INVALID = (HRESULT)(-2144272231);

	public static readonly HRESULT FVE_E_INVALID_PIN_CHARS = (HRESULT)(-2144272230);

	public static readonly HRESULT FVE_E_INVALID_DATUM_TYPE = (HRESULT)(-2144272229);

	public static readonly HRESULT FVE_E_EFI_ONLY = (HRESULT)(-2144272228);

	public static readonly HRESULT FVE_E_MULTIPLE_NKP_CERTS = (HRESULT)(-2144272227);

	public static readonly HRESULT FVE_E_REMOVAL_OF_NKP_FAILED = (HRESULT)(-2144272226);

	public static readonly HRESULT FVE_E_INVALID_NKP_CERT = (HRESULT)(-2144272225);

	public static readonly HRESULT FVE_E_NO_EXISTING_PIN = (HRESULT)(-2144272224);

	public static readonly HRESULT FVE_E_PROTECTOR_CHANGE_PIN_MISMATCH = (HRESULT)(-2144272223);

	public static readonly HRESULT FVE_E_PIN_PROTECTOR_CHANGE_BY_STD_USER_DISALLOWED = (HRESULT)(-2144272222);

	public static readonly HRESULT FVE_E_PROTECTOR_CHANGE_MAX_PIN_CHANGE_ATTEMPTS_REACHED = (HRESULT)(-2144272221);

	public static readonly HRESULT FVE_E_POLICY_PASSPHRASE_REQUIRES_ASCII = (HRESULT)(-2144272220);

	public static readonly HRESULT FVE_E_FULL_ENCRYPTION_NOT_ALLOWED_ON_TP_STORAGE = (HRESULT)(-2144272219);

	public static readonly HRESULT FVE_E_WIPE_NOT_ALLOWED_ON_TP_STORAGE = (HRESULT)(-2144272218);

	public static readonly HRESULT FVE_E_KEY_LENGTH_NOT_SUPPORTED_BY_EDRIVE = (HRESULT)(-2144272217);

	public static readonly HRESULT FVE_E_NO_EXISTING_PASSPHRASE = (HRESULT)(-2144272216);

	public static readonly HRESULT FVE_E_PROTECTOR_CHANGE_PASSPHRASE_MISMATCH = (HRESULT)(-2144272215);

	public static readonly HRESULT FVE_E_PASSPHRASE_TOO_LONG = (HRESULT)(-2144272214);

	public static readonly HRESULT FVE_E_NO_PASSPHRASE_WITH_TPM = (HRESULT)(-2144272213);

	public static readonly HRESULT FVE_E_NO_TPM_WITH_PASSPHRASE = (HRESULT)(-2144272212);

	public static readonly HRESULT FVE_E_NOT_ALLOWED_ON_CSV_STACK = (HRESULT)(-2144272211);

	public static readonly HRESULT FVE_E_NOT_ALLOWED_ON_CLUSTER = (HRESULT)(-2144272210);

	public static readonly HRESULT FVE_E_EDRIVE_NO_FAILOVER_TO_SW = (HRESULT)(-2144272209);

	public static readonly HRESULT FVE_E_EDRIVE_BAND_IN_USE = (HRESULT)(-2144272208);

	public static readonly HRESULT FVE_E_EDRIVE_DISALLOWED_BY_GP = (HRESULT)(-2144272207);

	public static readonly HRESULT FVE_E_EDRIVE_INCOMPATIBLE_VOLUME = (HRESULT)(-2144272206);

	public static readonly HRESULT FVE_E_NOT_ALLOWED_TO_UPGRADE_WHILE_CONVERTING = (HRESULT)(-2144272205);

	public static readonly HRESULT FVE_E_EDRIVE_DV_NOT_SUPPORTED = (HRESULT)(-2144272204);

	public static readonly HRESULT FVE_E_NO_PREBOOT_KEYBOARD_DETECTED = (HRESULT)(-2144272203);

	public static readonly HRESULT FVE_E_NO_PREBOOT_KEYBOARD_OR_WINRE_DETECTED = (HRESULT)(-2144272202);

	public static readonly HRESULT FVE_E_POLICY_REQUIRES_STARTUP_PIN_ON_TOUCH_DEVICE = (HRESULT)(-2144272201);

	public static readonly HRESULT FVE_E_POLICY_REQUIRES_RECOVERY_PASSWORD_ON_TOUCH_DEVICE = (HRESULT)(-2144272200);

	public static readonly HRESULT FVE_E_WIPE_CANCEL_NOT_APPLICABLE = (HRESULT)(-2144272199);

	public static readonly HRESULT FVE_E_SECUREBOOT_DISABLED = (HRESULT)(-2144272198);

	public static readonly HRESULT FVE_E_SECUREBOOT_CONFIGURATION_INVALID = (HRESULT)(-2144272197);

	public static readonly HRESULT FVE_E_EDRIVE_DRY_RUN_FAILED = (HRESULT)(-2144272196);

	public static readonly HRESULT FVE_E_SHADOW_COPY_PRESENT = (HRESULT)(-2144272195);

	public static readonly HRESULT FVE_E_POLICY_INVALID_ENHANCED_BCD_SETTINGS = (HRESULT)(-2144272194);

	public static readonly HRESULT FVE_E_EDRIVE_INCOMPATIBLE_FIRMWARE = (HRESULT)(-2144272193);

	public static readonly HRESULT FVE_E_PROTECTOR_CHANGE_MAX_PASSPHRASE_CHANGE_ATTEMPTS_REACHED = (HRESULT)(-2144272192);

	public static readonly HRESULT FVE_E_PASSPHRASE_PROTECTOR_CHANGE_BY_STD_USER_DISALLOWED = (HRESULT)(-2144272191);

	public static readonly HRESULT FVE_E_LIVEID_ACCOUNT_SUSPENDED = (HRESULT)(-2144272190);

	public static readonly HRESULT FVE_E_LIVEID_ACCOUNT_BLOCKED = (HRESULT)(-2144272189);

	public static readonly HRESULT FVE_E_NOT_PROVISIONED_ON_ALL_VOLUMES = (HRESULT)(-2144272188);

	public static readonly HRESULT FVE_E_DE_FIXED_DATA_NOT_SUPPORTED = (HRESULT)(-2144272187);

	public static readonly HRESULT FVE_E_DE_HARDWARE_NOT_COMPLIANT = (HRESULT)(-2144272186);

	public static readonly HRESULT FVE_E_DE_WINRE_NOT_CONFIGURED = (HRESULT)(-2144272185);

	public static readonly HRESULT FVE_E_DE_PROTECTION_SUSPENDED = (HRESULT)(-2144272184);

	public static readonly HRESULT FVE_E_DE_OS_VOLUME_NOT_PROTECTED = (HRESULT)(-2144272183);

	public static readonly HRESULT FVE_E_DE_DEVICE_LOCKEDOUT = (HRESULT)(-2144272182);

	public static readonly HRESULT FVE_E_DE_PROTECTION_NOT_YET_ENABLED = (HRESULT)(-2144272181);

	public static readonly HRESULT FVE_E_INVALID_PIN_CHARS_DETAILED = (HRESULT)(-2144272180);

	public static readonly HRESULT FVE_E_DEVICE_LOCKOUT_COUNTER_UNAVAILABLE = (HRESULT)(-2144272179);

	public static readonly HRESULT FVE_E_DEVICELOCKOUT_COUNTER_MISMATCH = (HRESULT)(-2144272178);

	public static readonly HRESULT FVE_E_BUFFER_TOO_LARGE = (HRESULT)(-2144272177);

	public static readonly HRESULT FVE_E_NO_SUCH_CAPABILITY_ON_TARGET = (HRESULT)(-2144272176);

	public static readonly HRESULT FVE_E_DE_PREVENTED_FOR_OS = (HRESULT)(-2144272175);

	public static readonly HRESULT FVE_E_DE_VOLUME_OPTED_OUT = (HRESULT)(-2144272174);

	public static readonly HRESULT FVE_E_DE_VOLUME_NOT_SUPPORTED = (HRESULT)(-2144272173);

	public static readonly HRESULT FVE_E_EOW_NOT_SUPPORTED_IN_VERSION = (HRESULT)(-2144272172);

	public static readonly HRESULT FVE_E_ADBACKUP_NOT_ENABLED = (HRESULT)(-2144272171);

	public static readonly HRESULT FVE_E_VOLUME_EXTEND_PREVENTS_EOW_DECRYPT = (HRESULT)(-2144272170);

	public static readonly HRESULT FVE_E_NOT_DE_VOLUME = (HRESULT)(-2144272169);

	public static readonly HRESULT FVE_E_PROTECTION_CANNOT_BE_DISABLED = (HRESULT)(-2144272168);

	public static readonly HRESULT FVE_E_OSV_KSR_NOT_ALLOWED = (HRESULT)(-2144272167);

	public static readonly HRESULT FVE_E_AD_BACKUP_REQUIRED_POLICY_NOT_SET_OS_DRIVE = (HRESULT)(-2144272166);

	public static readonly HRESULT FVE_E_AD_BACKUP_REQUIRED_POLICY_NOT_SET_FIXED_DRIVE = (HRESULT)(-2144272165);

	public static readonly HRESULT FVE_E_AD_BACKUP_REQUIRED_POLICY_NOT_SET_REMOVABLE_DRIVE = (HRESULT)(-2144272164);

	public static readonly HRESULT FVE_E_KEY_ROTATION_NOT_SUPPORTED = (HRESULT)(-2144272163);

	public static readonly HRESULT FVE_E_EXECUTE_REQUEST_SENT_TOO_SOON = (HRESULT)(-2144272162);

	public static readonly HRESULT FVE_E_KEY_ROTATION_NOT_ENABLED = (HRESULT)(-2144272161);

	public static readonly HRESULT FVE_E_DEVICE_NOT_JOINED = (HRESULT)(-2144272160);

	public static readonly HRESULT FVE_E_AAD_ENDPOINT_BUSY = (HRESULT)(-2144272159);

	public static readonly HRESULT FVE_E_INVALID_NBP_CERT = (HRESULT)(-2144272158);

	public static readonly HRESULT FVE_E_EDRIVE_BAND_ENUMERATION_FAILED = (HRESULT)(-2144272157);

	public static readonly HRESULT FVE_E_POLICY_ON_RDV_EXCLUSION_LIST = (HRESULT)(-2144272156);

	public static readonly HRESULT FVE_E_PREDICTED_TPM_PROTECTOR_NOT_SUPPORTED = (HRESULT)(-2144272155);

	public static readonly HRESULT FVE_E_SETUP_TPM_CALLBACK_NOT_SUPPORTED = (HRESULT)(-2144272154);

	public static readonly HRESULT FVE_E_TPM_CONTEXT_SETUP_NOT_SUPPORTED = (HRESULT)(-2144272153);

	public static readonly HRESULT FVE_E_UPDATE_INVALID_CONFIG = (HRESULT)(-2144272152);

	public static readonly HRESULT FVE_E_AAD_SERVER_FAIL_RETRY_AFTER = (HRESULT)(-2144272151);

	public static readonly HRESULT FVE_E_AAD_SERVER_FAIL_BACKOFF = (HRESULT)(-2144272150);

	public static readonly HRESULT FVE_E_DATASET_FULL = (HRESULT)(-2144272149);

	public static readonly HRESULT FVE_E_METADATA_FULL = (HRESULT)(-2144272148);

	public static readonly HRESULT FWP_E_CALLOUT_NOT_FOUND = (HRESULT)(-2144206847);

	public static readonly HRESULT FWP_E_CONDITION_NOT_FOUND = (HRESULT)(-2144206846);

	public static readonly HRESULT FWP_E_FILTER_NOT_FOUND = (HRESULT)(-2144206845);

	public static readonly HRESULT FWP_E_LAYER_NOT_FOUND = (HRESULT)(-2144206844);

	public static readonly HRESULT FWP_E_PROVIDER_NOT_FOUND = (HRESULT)(-2144206843);

	public static readonly HRESULT FWP_E_PROVIDER_CONTEXT_NOT_FOUND = (HRESULT)(-2144206842);

	public static readonly HRESULT FWP_E_SUBLAYER_NOT_FOUND = (HRESULT)(-2144206841);

	public static readonly HRESULT FWP_E_NOT_FOUND = (HRESULT)(-2144206840);

	public static readonly HRESULT FWP_E_ALREADY_EXISTS = (HRESULT)(-2144206839);

	public static readonly HRESULT FWP_E_IN_USE = (HRESULT)(-2144206838);

	public static readonly HRESULT FWP_E_DYNAMIC_SESSION_IN_PROGRESS = (HRESULT)(-2144206837);

	public static readonly HRESULT FWP_E_WRONG_SESSION = (HRESULT)(-2144206836);

	public static readonly HRESULT FWP_E_NO_TXN_IN_PROGRESS = (HRESULT)(-2144206835);

	public static readonly HRESULT FWP_E_TXN_IN_PROGRESS = (HRESULT)(-2144206834);

	public static readonly HRESULT FWP_E_TXN_ABORTED = (HRESULT)(-2144206833);

	public static readonly HRESULT FWP_E_SESSION_ABORTED = (HRESULT)(-2144206832);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_TXN = (HRESULT)(-2144206831);

	public static readonly HRESULT FWP_E_TIMEOUT = (HRESULT)(-2144206830);

	public static readonly HRESULT FWP_E_NET_EVENTS_DISABLED = (HRESULT)(-2144206829);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_LAYER = (HRESULT)(-2144206828);

	public static readonly HRESULT FWP_E_KM_CLIENTS_ONLY = (HRESULT)(-2144206827);

	public static readonly HRESULT FWP_E_LIFETIME_MISMATCH = (HRESULT)(-2144206826);

	public static readonly HRESULT FWP_E_BUILTIN_OBJECT = (HRESULT)(-2144206825);

	public static readonly HRESULT FWP_E_TOO_MANY_CALLOUTS = (HRESULT)(-2144206824);

	public static readonly HRESULT FWP_E_NOTIFICATION_DROPPED = (HRESULT)(-2144206823);

	public static readonly HRESULT FWP_E_TRAFFIC_MISMATCH = (HRESULT)(-2144206822);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_SA_STATE = (HRESULT)(-2144206821);

	public static readonly HRESULT FWP_E_NULL_POINTER = (HRESULT)(-2144206820);

	public static readonly HRESULT FWP_E_INVALID_ENUMERATOR = (HRESULT)(-2144206819);

	public static readonly HRESULT FWP_E_INVALID_FLAGS = (HRESULT)(-2144206818);

	public static readonly HRESULT FWP_E_INVALID_NET_MASK = (HRESULT)(-2144206817);

	public static readonly HRESULT FWP_E_INVALID_RANGE = (HRESULT)(-2144206816);

	public static readonly HRESULT FWP_E_INVALID_INTERVAL = (HRESULT)(-2144206815);

	public static readonly HRESULT FWP_E_ZERO_LENGTH_ARRAY = (HRESULT)(-2144206814);

	public static readonly HRESULT FWP_E_NULL_DISPLAY_NAME = (HRESULT)(-2144206813);

	public static readonly HRESULT FWP_E_INVALID_ACTION_TYPE = (HRESULT)(-2144206812);

	public static readonly HRESULT FWP_E_INVALID_WEIGHT = (HRESULT)(-2144206811);

	public static readonly HRESULT FWP_E_MATCH_TYPE_MISMATCH = (HRESULT)(-2144206810);

	public static readonly HRESULT FWP_E_TYPE_MISMATCH = (HRESULT)(-2144206809);

	public static readonly HRESULT FWP_E_OUT_OF_BOUNDS = (HRESULT)(-2144206808);

	public static readonly HRESULT FWP_E_RESERVED = (HRESULT)(-2144206807);

	public static readonly HRESULT FWP_E_DUPLICATE_CONDITION = (HRESULT)(-2144206806);

	public static readonly HRESULT FWP_E_DUPLICATE_KEYMOD = (HRESULT)(-2144206805);

	public static readonly HRESULT FWP_E_ACTION_INCOMPATIBLE_WITH_LAYER = (HRESULT)(-2144206804);

	public static readonly HRESULT FWP_E_ACTION_INCOMPATIBLE_WITH_SUBLAYER = (HRESULT)(-2144206803);

	public static readonly HRESULT FWP_E_CONTEXT_INCOMPATIBLE_WITH_LAYER = (HRESULT)(-2144206802);

	public static readonly HRESULT FWP_E_CONTEXT_INCOMPATIBLE_WITH_CALLOUT = (HRESULT)(-2144206801);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_AUTH_METHOD = (HRESULT)(-2144206800);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_DH_GROUP = (HRESULT)(-2144206799);

	public static readonly HRESULT FWP_E_EM_NOT_SUPPORTED = (HRESULT)(-2144206798);

	public static readonly HRESULT FWP_E_NEVER_MATCH = (HRESULT)(-2144206797);

	public static readonly HRESULT FWP_E_PROVIDER_CONTEXT_MISMATCH = (HRESULT)(-2144206796);

	public static readonly HRESULT FWP_E_INVALID_PARAMETER = (HRESULT)(-2144206795);

	public static readonly HRESULT FWP_E_TOO_MANY_SUBLAYERS = (HRESULT)(-2144206794);

	public static readonly HRESULT FWP_E_CALLOUT_NOTIFICATION_FAILED = (HRESULT)(-2144206793);

	public static readonly HRESULT FWP_E_INVALID_AUTH_TRANSFORM = (HRESULT)(-2144206792);

	public static readonly HRESULT FWP_E_INVALID_CIPHER_TRANSFORM = (HRESULT)(-2144206791);

	public static readonly HRESULT FWP_E_INCOMPATIBLE_CIPHER_TRANSFORM = (HRESULT)(-2144206790);

	public static readonly HRESULT FWP_E_INVALID_TRANSFORM_COMBINATION = (HRESULT)(-2144206789);

	public static readonly HRESULT FWP_E_DUPLICATE_AUTH_METHOD = (HRESULT)(-2144206788);

	public static readonly HRESULT FWP_E_INVALID_TUNNEL_ENDPOINT = (HRESULT)(-2144206787);

	public static readonly HRESULT FWP_E_L2_DRIVER_NOT_READY = (HRESULT)(-2144206786);

	public static readonly HRESULT FWP_E_KEY_DICTATOR_ALREADY_REGISTERED = (HRESULT)(-2144206785);

	public static readonly HRESULT FWP_E_KEY_DICTATION_INVALID_KEYING_MATERIAL = (HRESULT)(-2144206784);

	public static readonly HRESULT FWP_E_CONNECTIONS_DISABLED = (HRESULT)(-2144206783);

	public static readonly HRESULT FWP_E_INVALID_DNS_NAME = (HRESULT)(-2144206782);

	public static readonly HRESULT FWP_E_STILL_ON = (HRESULT)(-2144206781);

	public static readonly HRESULT FWP_E_IKEEXT_NOT_RUNNING = (HRESULT)(-2144206780);

	public static readonly HRESULT FWP_E_DROP_NOICMP = (HRESULT)(-2144206588);

	public static readonly HRESULT WS_S_ASYNC = (HRESULT)(3997696);

	public static readonly HRESULT WS_S_END = (HRESULT)(3997697);

	public static readonly HRESULT WS_E_INVALID_FORMAT = (HRESULT)(-2143485952);

	public static readonly HRESULT WS_E_OBJECT_FAULTED = (HRESULT)(-2143485951);

	public static readonly HRESULT WS_E_NUMERIC_OVERFLOW = (HRESULT)(-2143485950);

	public static readonly HRESULT WS_E_INVALID_OPERATION = (HRESULT)(-2143485949);

	public static readonly HRESULT WS_E_OPERATION_ABORTED = (HRESULT)(-2143485948);

	public static readonly HRESULT WS_E_ENDPOINT_ACCESS_DENIED = (HRESULT)(-2143485947);

	public static readonly HRESULT WS_E_OPERATION_TIMED_OUT = (HRESULT)(-2143485946);

	public static readonly HRESULT WS_E_OPERATION_ABANDONED = (HRESULT)(-2143485945);

	public static readonly HRESULT WS_E_QUOTA_EXCEEDED = (HRESULT)(-2143485944);

	public static readonly HRESULT WS_E_NO_TRANSLATION_AVAILABLE = (HRESULT)(-2143485943);

	public static readonly HRESULT WS_E_SECURITY_VERIFICATION_FAILURE = (HRESULT)(-2143485942);

	public static readonly HRESULT WS_E_ADDRESS_IN_USE = (HRESULT)(-2143485941);

	public static readonly HRESULT WS_E_ADDRESS_NOT_AVAILABLE = (HRESULT)(-2143485940);

	public static readonly HRESULT WS_E_ENDPOINT_NOT_FOUND = (HRESULT)(-2143485939);

	public static readonly HRESULT WS_E_ENDPOINT_NOT_AVAILABLE = (HRESULT)(-2143485938);

	public static readonly HRESULT WS_E_ENDPOINT_FAILURE = (HRESULT)(-2143485937);

	public static readonly HRESULT WS_E_ENDPOINT_UNREACHABLE = (HRESULT)(-2143485936);

	public static readonly HRESULT WS_E_ENDPOINT_ACTION_NOT_SUPPORTED = (HRESULT)(-2143485935);

	public static readonly HRESULT WS_E_ENDPOINT_TOO_BUSY = (HRESULT)(-2143485934);

	public static readonly HRESULT WS_E_ENDPOINT_FAULT_RECEIVED = (HRESULT)(-2143485933);

	public static readonly HRESULT WS_E_ENDPOINT_DISCONNECTED = (HRESULT)(-2143485932);

	public static readonly HRESULT WS_E_PROXY_FAILURE = (HRESULT)(-2143485931);

	public static readonly HRESULT WS_E_PROXY_ACCESS_DENIED = (HRESULT)(-2143485930);

	public static readonly HRESULT WS_E_NOT_SUPPORTED = (HRESULT)(-2143485929);

	public static readonly HRESULT WS_E_PROXY_REQUIRES_BASIC_AUTH = (HRESULT)(-2143485928);

	public static readonly HRESULT WS_E_PROXY_REQUIRES_DIGEST_AUTH = (HRESULT)(-2143485927);

	public static readonly HRESULT WS_E_PROXY_REQUIRES_NTLM_AUTH = (HRESULT)(-2143485926);

	public static readonly HRESULT WS_E_PROXY_REQUIRES_NEGOTIATE_AUTH = (HRESULT)(-2143485925);

	public static readonly HRESULT WS_E_SERVER_REQUIRES_BASIC_AUTH = (HRESULT)(-2143485924);

	public static readonly HRESULT WS_E_SERVER_REQUIRES_DIGEST_AUTH = (HRESULT)(-2143485923);

	public static readonly HRESULT WS_E_SERVER_REQUIRES_NTLM_AUTH = (HRESULT)(-2143485922);

	public static readonly HRESULT WS_E_SERVER_REQUIRES_NEGOTIATE_AUTH = (HRESULT)(-2143485921);

	public static readonly HRESULT WS_E_INVALID_ENDPOINT_URL = (HRESULT)(-2143485920);

	public static readonly HRESULT WS_E_OTHER = (HRESULT)(-2143485919);

	public static readonly HRESULT WS_E_SECURITY_TOKEN_EXPIRED = (HRESULT)(-2143485918);

	public static readonly HRESULT WS_E_SECURITY_SYSTEM_FAILURE = (HRESULT)(-2143485917);

	public static readonly HRESULT HCS_E_TERMINATED_DURING_START = (HRESULT)(-2143878912);

	public static readonly HRESULT HCS_E_IMAGE_MISMATCH = (HRESULT)(-2143878911);

	public static readonly HRESULT HCS_E_HYPERV_NOT_INSTALLED = (HRESULT)(-2143878910);

	public static readonly HRESULT HCS_E_INVALID_STATE = (HRESULT)(-2143878907);

	public static readonly HRESULT HCS_E_UNEXPECTED_EXIT = (HRESULT)(-2143878906);

	public static readonly HRESULT HCS_E_TERMINATED = (HRESULT)(-2143878905);

	public static readonly HRESULT HCS_E_CONNECT_FAILED = (HRESULT)(-2143878904);

	public static readonly HRESULT HCS_E_CONNECTION_TIMEOUT = (HRESULT)(-2143878903);

	public static readonly HRESULT HCS_E_CONNECTION_CLOSED = (HRESULT)(-2143878902);

	public static readonly HRESULT HCS_E_UNKNOWN_MESSAGE = (HRESULT)(-2143878901);

	public static readonly HRESULT HCS_E_UNSUPPORTED_PROTOCOL_VERSION = (HRESULT)(-2143878900);

	public static readonly HRESULT HCS_E_INVALID_JSON = (HRESULT)(-2143878899);

	public static readonly HRESULT HCS_E_SYSTEM_NOT_FOUND = (HRESULT)(-2143878898);

	public static readonly HRESULT HCS_E_SYSTEM_ALREADY_EXISTS = (HRESULT)(-2143878897);

	public static readonly HRESULT HCS_E_SYSTEM_ALREADY_STOPPED = (HRESULT)(-2143878896);

	public static readonly HRESULT HCS_E_PROTOCOL_ERROR = (HRESULT)(-2143878895);

	public static readonly HRESULT HCS_E_INVALID_LAYER = (HRESULT)(-2143878894);

	public static readonly HRESULT HCS_E_WINDOWS_INSIDER_REQUIRED = (HRESULT)(-2143878893);

	public static readonly HRESULT HCS_E_SERVICE_NOT_AVAILABLE = (HRESULT)(-2143878892);

	public static readonly HRESULT HCS_E_OPERATION_NOT_STARTED = (HRESULT)(-2143878891);

	public static readonly HRESULT HCS_E_OPERATION_ALREADY_STARTED = (HRESULT)(-2143878890);

	public static readonly HRESULT HCS_E_OPERATION_PENDING = (HRESULT)(-2143878889);

	public static readonly HRESULT HCS_E_OPERATION_TIMEOUT = (HRESULT)(-2143878888);

	public static readonly HRESULT HCS_E_OPERATION_SYSTEM_CALLBACK_ALREADY_SET = (HRESULT)(-2143878887);

	public static readonly HRESULT HCS_E_OPERATION_RESULT_ALLOCATION_FAILED = (HRESULT)(-2143878886);

	public static readonly HRESULT HCS_E_ACCESS_DENIED = (HRESULT)(-2143878885);

	public static readonly HRESULT HCS_E_GUEST_CRITICAL_ERROR = (HRESULT)(-2143878884);

	public static readonly HRESULT HCS_E_PROCESS_INFO_NOT_AVAILABLE = (HRESULT)(-2143878883);

	public static readonly HRESULT HCS_E_SERVICE_DISCONNECT = (HRESULT)(-2143878882);

	public static readonly HRESULT HCS_E_PROCESS_ALREADY_STOPPED = (HRESULT)(-2143878881);

	public static readonly HRESULT HCS_E_SYSTEM_NOT_CONFIGURED_FOR_OPERATION = (HRESULT)(-2143878880);

	public static readonly HRESULT HCS_E_OPERATION_ALREADY_CANCELLED = (HRESULT)(-2143878879);

	public static readonly HRESULT WHV_E_UNKNOWN_CAPABILITY = (HRESULT)(-2143878400);

	public static readonly HRESULT WHV_E_INSUFFICIENT_BUFFER = (HRESULT)(-2143878399);

	public static readonly HRESULT WHV_E_UNKNOWN_PROPERTY = (HRESULT)(-2143878398);

	public static readonly HRESULT WHV_E_UNSUPPORTED_HYPERVISOR_CONFIG = (HRESULT)(-2143878397);

	public static readonly HRESULT WHV_E_INVALID_PARTITION_CONFIG = (HRESULT)(-2143878396);

	public static readonly HRESULT WHV_E_GPA_RANGE_NOT_FOUND = (HRESULT)(-2143878395);

	public static readonly HRESULT WHV_E_VP_ALREADY_EXISTS = (HRESULT)(-2143878394);

	public static readonly HRESULT WHV_E_VP_DOES_NOT_EXIST = (HRESULT)(-2143878393);

	public static readonly HRESULT WHV_E_INVALID_VP_STATE = (HRESULT)(-2143878392);

	public static readonly HRESULT WHV_E_INVALID_VP_REGISTER_NAME = (HRESULT)(-2143878391);

	public static readonly HRESULT WHV_E_UNSUPPORTED_PROCESSOR_CONFIG = (HRESULT)(-2143878384);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_PARTITION_STATE_NOT_FOUND = (HRESULT)(-1070136064);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_GUEST_MEMORY_NOT_FOUND = (HRESULT)(-1070136063);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_NO_VP_FOUND_IN_PARTITION_STATE = (HRESULT)(-1070136062);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_NESTED_VIRTUALIZATION_NOT_SUPPORTED = (HRESULT)(-1070136061);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_WINDOWS_KERNEL_IMAGE_NOT_FOUND = (HRESULT)(-1070136060);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_VA_NOT_MAPPED = (HRESULT)(-1070136059);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_INVALID_VP_STATE = (HRESULT)(-1070136058);

	public static readonly HRESULT VM_SAVED_STATE_DUMP_E_VP_VTL_NOT_ENABLED = (HRESULT)(-1070136055);

	public static readonly HRESULT ERROR_DM_OPERATION_LIMIT_EXCEEDED = (HRESULT)(-1070135808);

	public static readonly HRESULT HCN_E_NETWORK_NOT_FOUND = (HRESULT)(-2143617023);

	public static readonly HRESULT HCN_E_ENDPOINT_NOT_FOUND = (HRESULT)(-2143617022);

	public static readonly HRESULT HCN_E_LAYER_NOT_FOUND = (HRESULT)(-2143617021);

	public static readonly HRESULT HCN_E_SWITCH_NOT_FOUND = (HRESULT)(-2143617020);

	public static readonly HRESULT HCN_E_SUBNET_NOT_FOUND = (HRESULT)(-2143617019);

	public static readonly HRESULT HCN_E_ADAPTER_NOT_FOUND = (HRESULT)(-2143617018);

	public static readonly HRESULT HCN_E_PORT_NOT_FOUND = (HRESULT)(-2143617017);

	public static readonly HRESULT HCN_E_POLICY_NOT_FOUND = (HRESULT)(-2143617016);

	public static readonly HRESULT HCN_E_VFP_PORTSETTING_NOT_FOUND = (HRESULT)(-2143617015);

	public static readonly HRESULT HCN_E_INVALID_NETWORK = (HRESULT)(-2143617014);

	public static readonly HRESULT HCN_E_INVALID_NETWORK_TYPE = (HRESULT)(-2143617013);

	public static readonly HRESULT HCN_E_INVALID_ENDPOINT = (HRESULT)(-2143617012);

	public static readonly HRESULT HCN_E_INVALID_POLICY = (HRESULT)(-2143617011);

	public static readonly HRESULT HCN_E_INVALID_POLICY_TYPE = (HRESULT)(-2143617010);

	public static readonly HRESULT HCN_E_INVALID_REMOTE_ENDPOINT_OPERATION = (HRESULT)(-2143617009);

	public static readonly HRESULT HCN_E_NETWORK_ALREADY_EXISTS = (HRESULT)(-2143617008);

	public static readonly HRESULT HCN_E_LAYER_ALREADY_EXISTS = (HRESULT)(-2143617007);

	public static readonly HRESULT HCN_E_POLICY_ALREADY_EXISTS = (HRESULT)(-2143617006);

	public static readonly HRESULT HCN_E_PORT_ALREADY_EXISTS = (HRESULT)(-2143617005);

	public static readonly HRESULT HCN_E_ENDPOINT_ALREADY_ATTACHED = (HRESULT)(-2143617004);

	public static readonly HRESULT HCN_E_REQUEST_UNSUPPORTED = (HRESULT)(-2143617003);

	public static readonly HRESULT HCN_E_MAPPING_NOT_SUPPORTED = (HRESULT)(-2143617002);

	public static readonly HRESULT HCN_E_DEGRADED_OPERATION = (HRESULT)(-2143617001);

	public static readonly HRESULT HCN_E_SHARED_SWITCH_MODIFICATION = (HRESULT)(-2143617000);

	public static readonly HRESULT HCN_E_GUID_CONVERSION_FAILURE = (HRESULT)(-2143616999);

	public static readonly HRESULT HCN_E_REGKEY_FAILURE = (HRESULT)(-2143616998);

	public static readonly HRESULT HCN_E_INVALID_JSON = (HRESULT)(-2143616997);

	public static readonly HRESULT HCN_E_INVALID_JSON_REFERENCE = (HRESULT)(-2143616996);

	public static readonly HRESULT HCN_E_ENDPOINT_SHARING_DISABLED = (HRESULT)(-2143616995);

	public static readonly HRESULT HCN_E_INVALID_IP = (HRESULT)(-2143616994);

	public static readonly HRESULT HCN_E_SWITCH_EXTENSION_NOT_FOUND = (HRESULT)(-2143616993);

	public static readonly HRESULT HCN_E_MANAGER_STOPPED = (HRESULT)(-2143616992);

	public static readonly HRESULT GCN_E_MODULE_NOT_FOUND = (HRESULT)(-2143616991);

	public static readonly HRESULT GCN_E_NO_REQUEST_HANDLERS = (HRESULT)(-2143616990);

	public static readonly HRESULT GCN_E_REQUEST_UNSUPPORTED = (HRESULT)(-2143616989);

	public static readonly HRESULT GCN_E_RUNTIMEKEYS_FAILED = (HRESULT)(-2143616988);

	public static readonly HRESULT GCN_E_NETADAPTER_TIMEOUT = (HRESULT)(-2143616987);

	public static readonly HRESULT GCN_E_NETADAPTER_NOT_FOUND = (HRESULT)(-2143616986);

	public static readonly HRESULT GCN_E_NETCOMPARTMENT_NOT_FOUND = (HRESULT)(-2143616985);

	public static readonly HRESULT GCN_E_NETINTERFACE_NOT_FOUND = (HRESULT)(-2143616984);

	public static readonly HRESULT GCN_E_DEFAULTNAMESPACE_EXISTS = (HRESULT)(-2143616983);

	public static readonly HRESULT HCN_E_ICS_DISABLED = (HRESULT)(-2143616982);

	public static readonly HRESULT HCN_E_ENDPOINT_NAMESPACE_ALREADY_EXISTS = (HRESULT)(-2143616981);

	public static readonly HRESULT HCN_E_ENTITY_HAS_REFERENCES = (HRESULT)(-2143616980);

	public static readonly HRESULT HCN_E_INVALID_INTERNAL_PORT = (HRESULT)(-2143616979);

	public static readonly HRESULT HCN_E_NAMESPACE_ATTACH_FAILED = (HRESULT)(-2143616978);

	public static readonly HRESULT HCN_E_ADDR_INVALID_OR_RESERVED = (HRESULT)(-2143616977);

	public static readonly HRESULT HCN_E_INVALID_PREFIX = (HRESULT)(-2143616976);

	public static readonly HRESULT HCN_E_OBJECT_USED_AFTER_UNLOAD = (HRESULT)(-2143616975);

	public static readonly HRESULT HCN_E_INVALID_SUBNET = (HRESULT)(-2143616974);

	public static readonly HRESULT HCN_E_INVALID_IP_SUBNET = (HRESULT)(-2143616973);

	public static readonly HRESULT HCN_E_ENDPOINT_NOT_ATTACHED = (HRESULT)(-2143616972);

	public static readonly HRESULT HCN_E_ENDPOINT_NOT_LOCAL = (HRESULT)(-2143616971);

	public static readonly HRESULT HCN_INTERFACEPARAMETERS_ALREADY_APPLIED = (HRESULT)(-2143616970);

	public static readonly HRESULT HCN_E_VFP_NOT_ALLOWED = (HRESULT)(-2143616969);

	public static readonly HRESULT WPN_E_CHANNEL_CLOSED = (HRESULT)(-2143420160);

	public static readonly HRESULT WPN_E_CHANNEL_REQUEST_NOT_COMPLETE = (HRESULT)(-2143420159);

	public static readonly HRESULT WPN_E_INVALID_APP = (HRESULT)(-2143420158);

	public static readonly HRESULT WPN_E_OUTSTANDING_CHANNEL_REQUEST = (HRESULT)(-2143420157);

	public static readonly HRESULT WPN_E_DUPLICATE_CHANNEL = (HRESULT)(-2143420156);

	public static readonly HRESULT WPN_E_PLATFORM_UNAVAILABLE = (HRESULT)(-2143420155);

	public static readonly HRESULT WPN_E_NOTIFICATION_POSTED = (HRESULT)(-2143420154);

	public static readonly HRESULT WPN_E_NOTIFICATION_HIDDEN = (HRESULT)(-2143420153);

	public static readonly HRESULT WPN_E_NOTIFICATION_NOT_POSTED = (HRESULT)(-2143420152);

	public static readonly HRESULT WPN_E_CLOUD_DISABLED = (HRESULT)(-2143420151);

	public static readonly HRESULT WPN_E_CLOUD_INCAPABLE = (HRESULT)(-2143420144);

	public static readonly HRESULT WPN_E_CLOUD_AUTH_UNAVAILABLE = (HRESULT)(-2143420134);

	public static readonly HRESULT WPN_E_CLOUD_SERVICE_UNAVAILABLE = (HRESULT)(-2143420133);

	public static readonly HRESULT WPN_E_FAILED_LOCK_SCREEN_UPDATE_INTIALIZATION = (HRESULT)(-2143420132);

	public static readonly HRESULT WPN_E_NOTIFICATION_DISABLED = (HRESULT)(-2143420143);

	public static readonly HRESULT WPN_E_NOTIFICATION_INCAPABLE = (HRESULT)(-2143420142);

	public static readonly HRESULT WPN_E_INTERNET_INCAPABLE = (HRESULT)(-2143420141);

	public static readonly HRESULT WPN_E_NOTIFICATION_TYPE_DISABLED = (HRESULT)(-2143420140);

	public static readonly HRESULT WPN_E_NOTIFICATION_SIZE = (HRESULT)(-2143420139);

	public static readonly HRESULT WPN_E_TAG_SIZE = (HRESULT)(-2143420138);

	public static readonly HRESULT WPN_E_ACCESS_DENIED = (HRESULT)(-2143420137);

	public static readonly HRESULT WPN_E_DUPLICATE_REGISTRATION = (HRESULT)(-2143420136);

	public static readonly HRESULT WPN_E_PUSH_NOTIFICATION_INCAPABLE = (HRESULT)(-2143420135);

	public static readonly HRESULT WPN_E_DEV_ID_SIZE = (HRESULT)(-2143420128);

	public static readonly HRESULT WPN_E_TAG_ALPHANUMERIC = (HRESULT)(-2143420118);

	public static readonly HRESULT WPN_E_INVALID_HTTP_STATUS_CODE = (HRESULT)(-2143420117);

	public static readonly HRESULT WPN_E_OUT_OF_SESSION = (HRESULT)(-2143419904);

	public static readonly HRESULT WPN_E_POWER_SAVE = (HRESULT)(-2143419903);

	public static readonly HRESULT WPN_E_IMAGE_NOT_FOUND_IN_CACHE = (HRESULT)(-2143419902);

	public static readonly HRESULT WPN_E_ALL_URL_NOT_COMPLETED = (HRESULT)(-2143419901);

	public static readonly HRESULT WPN_E_INVALID_CLOUD_IMAGE = (HRESULT)(-2143419900);

	public static readonly HRESULT WPN_E_NOTIFICATION_ID_MATCHED = (HRESULT)(-2143419899);

	public static readonly HRESULT WPN_E_CALLBACK_ALREADY_REGISTERED = (HRESULT)(-2143419898);

	public static readonly HRESULT WPN_E_TOAST_NOTIFICATION_DROPPED = (HRESULT)(-2143419897);

	public static readonly HRESULT WPN_E_STORAGE_LOCKED = (HRESULT)(-2143419896);

	public static readonly HRESULT WPN_E_GROUP_SIZE = (HRESULT)(-2143419895);

	public static readonly HRESULT WPN_E_GROUP_ALPHANUMERIC = (HRESULT)(-2143419894);

	public static readonly HRESULT WPN_E_CLOUD_DISABLED_FOR_APP = (HRESULT)(-2143419893);

	public static readonly HRESULT E_MBN_CONTEXT_NOT_ACTIVATED = (HRESULT)(-2141945343);

	public static readonly HRESULT E_MBN_BAD_SIM = (HRESULT)(-2141945342);

	public static readonly HRESULT E_MBN_DATA_CLASS_NOT_AVAILABLE = (HRESULT)(-2141945341);

	public static readonly HRESULT E_MBN_INVALID_ACCESS_STRING = (HRESULT)(-2141945340);

	public static readonly HRESULT E_MBN_MAX_ACTIVATED_CONTEXTS = (HRESULT)(-2141945339);

	public static readonly HRESULT E_MBN_PACKET_SVC_DETACHED = (HRESULT)(-2141945338);

	public static readonly HRESULT E_MBN_PROVIDER_NOT_VISIBLE = (HRESULT)(-2141945337);


	public static readonly HRESULT E_MBN_RADIO_POWER_OFF = (HRESULT)(-2141945336);


	public static readonly HRESULT E_MBN_SERVICE_NOT_ACTIVATED = (HRESULT)(-2141945335);

	public static readonly HRESULT E_MBN_SIM_NOT_INSERTED = (HRESULT)(-2141945334);

	public static readonly HRESULT E_MBN_VOICE_CALL_IN_PROGRESS = (HRESULT)(-2141945333);

	public static readonly HRESULT E_MBN_INVALID_CACHE = (HRESULT)(-2141945332);

	public static readonly HRESULT E_MBN_NOT_REGISTERED = (HRESULT)(-2141945331);

	public static readonly HRESULT E_MBN_PROVIDERS_NOT_FOUND = (HRESULT)(-2141945330);

	public static readonly HRESULT E_MBN_PIN_NOT_SUPPORTED = (HRESULT)(-2141945329);

	public static readonly HRESULT E_MBN_PIN_REQUIRED = (HRESULT)(-2141945328);

	public static readonly HRESULT E_MBN_PIN_DISABLED = (HRESULT)(-2141945327);

	public static readonly HRESULT E_MBN_FAILURE = (HRESULT)(-2141945326);

	public static readonly HRESULT E_MBN_INVALID_PROFILE = (HRESULT)(-2141945320);

	public static readonly HRESULT E_MBN_DEFAULT_PROFILE_EXIST = (HRESULT)(-2141945319);

	public static readonly HRESULT E_MBN_SMS_ENCODING_NOT_SUPPORTED = (HRESULT)(-2141945312);

	public static readonly HRESULT E_MBN_SMS_FILTER_NOT_SUPPORTED = (HRESULT)(-2141945311);

	public static readonly HRESULT E_MBN_SMS_INVALID_MEMORY_INDEX = (HRESULT)(-2141945310);

	public static readonly HRESULT E_MBN_SMS_LANG_NOT_SUPPORTED = (HRESULT)(-2141945309);

	public static readonly HRESULT E_MBN_SMS_MEMORY_FAILURE = (HRESULT)(-2141945308);

	public static readonly HRESULT E_MBN_SMS_NETWORK_TIMEOUT = (HRESULT)(-2141945307);

	public static readonly HRESULT E_MBN_SMS_UNKNOWN_SMSC_ADDRESS = (HRESULT)(-2141945306);

	public static readonly HRESULT E_MBN_SMS_FORMAT_NOT_SUPPORTED = (HRESULT)(-2141945305);

	public static readonly HRESULT E_MBN_SMS_OPERATION_NOT_ALLOWED = (HRESULT)(-2141945304);

	public static readonly HRESULT E_MBN_SMS_MEMORY_FULL = (HRESULT)(-2141945303);

	public static readonly HRESULT PEER_E_IPV6_NOT_INSTALLED = (HRESULT)(-2140995583);

	public static readonly HRESULT PEER_E_NOT_INITIALIZED = (HRESULT)(-2140995582);

	public static readonly HRESULT PEER_E_CANNOT_START_SERVICE = (HRESULT)(-2140995581);

	public static readonly HRESULT PEER_E_NOT_LICENSED = (HRESULT)(-2140995580);

	public static readonly HRESULT PEER_E_INVALID_GRAPH = (HRESULT)(-2140995568);

	public static readonly HRESULT PEER_E_DBNAME_CHANGED = (HRESULT)(-2140995567);

	public static readonly HRESULT PEER_E_DUPLICATE_GRAPH = (HRESULT)(-2140995566);

	public static readonly HRESULT PEER_E_GRAPH_NOT_READY = (HRESULT)(-2140995565);

	public static readonly HRESULT PEER_E_GRAPH_SHUTTING_DOWN = (HRESULT)(-2140995564);

	public static readonly HRESULT PEER_E_GRAPH_IN_USE = (HRESULT)(-2140995563);

	public static readonly HRESULT PEER_E_INVALID_DATABASE = (HRESULT)(-2140995562);

	public static readonly HRESULT PEER_E_TOO_MANY_ATTRIBUTES = (HRESULT)(-2140995561);

	public static readonly HRESULT PEER_E_CONNECTION_NOT_FOUND = (HRESULT)(-2140995325);

	public static readonly HRESULT PEER_E_CONNECT_SELF = (HRESULT)(-2140995322);

	public static readonly HRESULT PEER_E_ALREADY_LISTENING = (HRESULT)(-2140995321);

	public static readonly HRESULT PEER_E_NODE_NOT_FOUND = (HRESULT)(-2140995320);

	public static readonly HRESULT PEER_E_CONNECTION_FAILED = (HRESULT)(-2140995319);

	public static readonly HRESULT PEER_E_CONNECTION_NOT_AUTHENTICATED = (HRESULT)(-2140995318);

	public static readonly HRESULT PEER_E_CONNECTION_REFUSED = (HRESULT)(-2140995317);

	public static readonly HRESULT PEER_E_CLASSIFIER_TOO_LONG = (HRESULT)(-2140995071);

	public static readonly HRESULT PEER_E_TOO_MANY_IDENTITIES = (HRESULT)(-2140995070);

	public static readonly HRESULT PEER_E_NO_KEY_ACCESS = (HRESULT)(-2140995069);

	public static readonly HRESULT PEER_E_GROUPS_EXIST = (HRESULT)(-2140995068);

	public static readonly HRESULT PEER_E_RECORD_NOT_FOUND = (HRESULT)(-2140994815);

	public static readonly HRESULT PEER_E_DATABASE_ACCESSDENIED = (HRESULT)(-2140994814);

	public static readonly HRESULT PEER_E_DBINITIALIZATION_FAILED = (HRESULT)(-2140994813);

	public static readonly HRESULT PEER_E_MAX_RECORD_SIZE_EXCEEDED = (HRESULT)(-2140994812);

	public static readonly HRESULT PEER_E_DATABASE_ALREADY_PRESENT = (HRESULT)(-2140994811);

	public static readonly HRESULT PEER_E_DATABASE_NOT_PRESENT = (HRESULT)(-2140994810);

	public static readonly HRESULT PEER_E_IDENTITY_NOT_FOUND = (HRESULT)(-2140994559);

	public static readonly HRESULT PEER_E_EVENT_HANDLE_NOT_FOUND = (HRESULT)(-2140994303);

	public static readonly HRESULT PEER_E_INVALID_SEARCH = (HRESULT)(-2140994047);

	public static readonly HRESULT PEER_E_INVALID_ATTRIBUTES = (HRESULT)(-2140994046);

	public static readonly HRESULT PEER_E_INVITATION_NOT_TRUSTED = (HRESULT)(-2140993791);

	public static readonly HRESULT PEER_E_CHAIN_TOO_LONG = (HRESULT)(-2140993789);

	public static readonly HRESULT PEER_E_INVALID_TIME_PERIOD = (HRESULT)(-2140993787);

	public static readonly HRESULT PEER_E_CIRCULAR_CHAIN_DETECTED = (HRESULT)(-2140993786);

	public static readonly HRESULT PEER_E_CERT_STORE_CORRUPTED = (HRESULT)(-2140993535);

	public static readonly HRESULT PEER_E_NO_CLOUD = (HRESULT)(-2140991487);

	public static readonly HRESULT PEER_E_CLOUD_NAME_AMBIGUOUS = (HRESULT)(-2140991483);

	public static readonly HRESULT PEER_E_INVALID_RECORD = (HRESULT)(-2140987376);

	public static readonly HRESULT PEER_E_NOT_AUTHORIZED = (HRESULT)(-2140987360);

	public static readonly HRESULT PEER_E_PASSWORD_DOES_NOT_MEET_POLICY = (HRESULT)(-2140987359);

	public static readonly HRESULT PEER_E_DEFERRED_VALIDATION = (HRESULT)(-2140987344);

	public static readonly HRESULT PEER_E_INVALID_GROUP_PROPERTIES = (HRESULT)(-2140987328);

	public static readonly HRESULT PEER_E_INVALID_PEER_NAME = (HRESULT)(-2140987312);

	public static readonly HRESULT PEER_E_INVALID_CLASSIFIER = (HRESULT)(-2140987296);

	public static readonly HRESULT PEER_E_INVALID_FRIENDLY_NAME = (HRESULT)(-2140987280);

	public static readonly HRESULT PEER_E_INVALID_ROLE_PROPERTY = (HRESULT)(-2140987279);

	public static readonly HRESULT PEER_E_INVALID_CLASSIFIER_PROPERTY = (HRESULT)(-2140987278);

	public static readonly HRESULT PEER_E_INVALID_RECORD_EXPIRATION = (HRESULT)(-2140987264);

	public static readonly HRESULT PEER_E_INVALID_CREDENTIAL_INFO = (HRESULT)(-2140987263);

	public static readonly HRESULT PEER_E_INVALID_CREDENTIAL = (HRESULT)(-2140987262);

	public static readonly HRESULT PEER_E_INVALID_RECORD_SIZE = (HRESULT)(-2140987261);

	public static readonly HRESULT PEER_E_UNSUPPORTED_VERSION = (HRESULT)(-2140987248);

	public static readonly HRESULT PEER_E_GROUP_NOT_READY = (HRESULT)(-2140987247);

	public static readonly HRESULT PEER_E_GROUP_IN_USE = (HRESULT)(-2140987246);

	public static readonly HRESULT PEER_E_INVALID_GROUP = (HRESULT)(-2140987245);

	public static readonly HRESULT PEER_E_NO_MEMBERS_FOUND = (HRESULT)(-2140987244);

	public static readonly HRESULT PEER_E_NO_MEMBER_CONNECTIONS = (HRESULT)(-2140987243);

	public static readonly HRESULT PEER_E_UNABLE_TO_LISTEN = (HRESULT)(-2140987242);

	public static readonly HRESULT PEER_E_IDENTITY_DELETED = (HRESULT)(-2140987232);

	public static readonly HRESULT PEER_E_SERVICE_NOT_AVAILABLE = (HRESULT)(-2140987231);

	public static readonly HRESULT PEER_E_CONTACT_NOT_FOUND = (HRESULT)(-2140971007);

	public static readonly HRESULT PEER_S_GRAPH_DATA_CREATED = (HRESULT)(6488065);

	public static readonly HRESULT PEER_S_NO_EVENT_DATA = (HRESULT)(6488066);

	public static readonly HRESULT PEER_S_ALREADY_CONNECTED = (HRESULT)(6496256);

	public static readonly HRESULT PEER_S_SUBSCRIPTION_EXISTS = (HRESULT)(6512640);

	public static readonly HRESULT PEER_S_NO_CONNECTIVITY = (HRESULT)(6488069);

	public static readonly HRESULT PEER_S_ALREADY_A_MEMBER = (HRESULT)(6488070);

	public static readonly HRESULT PEER_E_CANNOT_CONVERT_PEER_NAME = (HRESULT)(-2140979199);

	public static readonly HRESULT PEER_E_INVALID_PEER_HOST_NAME = (HRESULT)(-2140979198);

	public static readonly HRESULT PEER_E_NO_MORE = (HRESULT)(-2140979197);

	public static readonly HRESULT PEER_E_PNRP_DUPLICATE_PEER_NAME = (HRESULT)(-2140979195);

	public static readonly HRESULT PEER_E_INVITE_CANCELLED = (HRESULT)(-2140966912);

	public static readonly HRESULT PEER_E_INVITE_RESPONSE_NOT_AVAILABLE = (HRESULT)(-2140966911);

	public static readonly HRESULT PEER_E_NOT_SIGNED_IN = (HRESULT)(-2140966909);

	public static readonly HRESULT PEER_E_PRIVACY_DECLINED = (HRESULT)(-2140966908);

	public static readonly HRESULT PEER_E_TIMEOUT = (HRESULT)(-2140966907);

	public static readonly HRESULT PEER_E_INVALID_ADDRESS = (HRESULT)(-2140966905);

	public static readonly HRESULT PEER_E_FW_EXCEPTION_DISABLED = (HRESULT)(-2140966904);

	public static readonly HRESULT PEER_E_FW_BLOCKED_BY_POLICY = (HRESULT)(-2140966903);

	public static readonly HRESULT PEER_E_FW_BLOCKED_BY_SHIELDS_UP = (HRESULT)(-2140966902);

	public static readonly HRESULT PEER_E_FW_DECLINED = (HRESULT)(-2140966901);

	public static readonly HRESULT UI_E_CREATE_FAILED = (HRESULT)(-2144731135);

	public static readonly HRESULT UI_E_SHUTDOWN_CALLED = (HRESULT)(-2144731134);

	public static readonly HRESULT UI_E_ILLEGAL_REENTRANCY = (HRESULT)(-2144731133);

	public static readonly HRESULT UI_E_OBJECT_SEALED = (HRESULT)(-2144731132);

	public static readonly HRESULT UI_E_VALUE_NOT_SET = (HRESULT)(-2144731131);

	public static readonly HRESULT UI_E_VALUE_NOT_DETERMINED = (HRESULT)(-2144731130);

	public static readonly HRESULT UI_E_INVALID_OUTPUT = (HRESULT)(-2144731129);

	public static readonly HRESULT UI_E_BOOLEAN_EXPECTED = (HRESULT)(-2144731128);

	public static readonly HRESULT UI_E_DIFFERENT_OWNER = (HRESULT)(-2144731127);

	public static readonly HRESULT UI_E_AMBIGUOUS_MATCH = (HRESULT)(-2144731126);

	public static readonly HRESULT UI_E_FP_OVERFLOW = (HRESULT)(-2144731125);

	public static readonly HRESULT UI_E_WRONG_THREAD = (HRESULT)(-2144731124);

	public static readonly HRESULT UI_E_STORYBOARD_ACTIVE = (HRESULT)(-2144730879);

	public static readonly HRESULT UI_E_STORYBOARD_NOT_PLAYING = (HRESULT)(-2144730878);

	public static readonly HRESULT UI_E_START_KEYFRAME_AFTER_END = (HRESULT)(-2144730877);

	public static readonly HRESULT UI_E_END_KEYFRAME_NOT_DETERMINED = (HRESULT)(-2144730876);

	public static readonly HRESULT UI_E_LOOPS_OVERLAP = (HRESULT)(-2144730875);

	public static readonly HRESULT UI_E_TRANSITION_ALREADY_USED = (HRESULT)(-2144730874);

	public static readonly HRESULT UI_E_TRANSITION_NOT_IN_STORYBOARD = (HRESULT)(-2144730873);

	public static readonly HRESULT UI_E_TRANSITION_ECLIPSED = (HRESULT)(-2144730872);

	public static readonly HRESULT UI_E_TIME_BEFORE_LAST_UPDATE = (HRESULT)(-2144730871);

	public static readonly HRESULT UI_E_TIMER_CLIENT_ALREADY_CONNECTED = (HRESULT)(-2144730870);

	public static readonly HRESULT UI_E_INVALID_DIMENSION = (HRESULT)(-2144730869);

	public static readonly HRESULT UI_E_PRIMITIVE_OUT_OF_BOUNDS = (HRESULT)(-2144730868);

	public static readonly HRESULT UI_E_WINDOW_CLOSED = (HRESULT)(-2144730623);

	public static readonly HRESULT E_BLUETOOTH_ATT_INVALID_HANDLE = (HRESULT)(-2140864511);

	public static readonly HRESULT E_BLUETOOTH_ATT_READ_NOT_PERMITTED = (HRESULT)(-2140864510);

	public static readonly HRESULT E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED = (HRESULT)(-2140864509);

	public static readonly HRESULT E_BLUETOOTH_ATT_INVALID_PDU = (HRESULT)(-2140864508);

	public static readonly HRESULT E_BLUETOOTH_ATT_INSUFFICIENT_AUTHENTICATION = (HRESULT)(-2140864507);

	public static readonly HRESULT E_BLUETOOTH_ATT_REQUEST_NOT_SUPPORTED = (HRESULT)(-2140864506);

	public static readonly HRESULT E_BLUETOOTH_ATT_INVALID_OFFSET = (HRESULT)(-2140864505);

	public static readonly HRESULT E_BLUETOOTH_ATT_INSUFFICIENT_AUTHORIZATION = (HRESULT)(-2140864504);

	public static readonly HRESULT E_BLUETOOTH_ATT_PREPARE_QUEUE_FULL = (HRESULT)(-2140864503);

	public static readonly HRESULT E_BLUETOOTH_ATT_ATTRIBUTE_NOT_FOUND = (HRESULT)(-2140864502);

	public static readonly HRESULT E_BLUETOOTH_ATT_ATTRIBUTE_NOT_LONG = (HRESULT)(-2140864501);

	public static readonly HRESULT E_BLUETOOTH_ATT_INSUFFICIENT_ENCRYPTION_KEY_SIZE = (HRESULT)(-2140864500);

	public static readonly HRESULT E_BLUETOOTH_ATT_INVALID_ATTRIBUTE_VALUE_LENGTH = (HRESULT)(-2140864499);

	public static readonly HRESULT E_BLUETOOTH_ATT_UNLIKELY = (HRESULT)(-2140864498);

	public static readonly HRESULT E_BLUETOOTH_ATT_INSUFFICIENT_ENCRYPTION = (HRESULT)(-2140864497);

	public static readonly HRESULT E_BLUETOOTH_ATT_UNSUPPORTED_GROUP_TYPE = (HRESULT)(-2140864496);

	public static readonly HRESULT E_BLUETOOTH_ATT_INSUFFICIENT_RESOURCES = (HRESULT)(-2140864495);

	public static readonly HRESULT E_BLUETOOTH_ATT_UNKNOWN_ERROR = (HRESULT)(-2140860416);

	public static readonly HRESULT E_AUDIO_ENGINE_NODE_NOT_FOUND = (HRESULT)(-2140798975);

	public static readonly HRESULT E_HDAUDIO_EMPTY_CONNECTION_LIST = (HRESULT)(-2140798974);

	public static readonly HRESULT E_HDAUDIO_CONNECTION_LIST_NOT_SUPPORTED = (HRESULT)(-2140798973);

	public static readonly HRESULT E_HDAUDIO_NO_LOGICAL_DEVICES_CREATED = (HRESULT)(-2140798972);

	public static readonly HRESULT E_HDAUDIO_NULL_LINKED_LIST_ENTRY = (HRESULT)(-2140798971);

	public static readonly HRESULT STATEREPOSITORY_E_CONCURRENCY_LOCKING_FAILURE = (HRESULT)(-2140733439);

	public static readonly HRESULT STATEREPOSITORY_E_STATEMENT_INPROGRESS = (HRESULT)(-2140733438);

	public static readonly HRESULT STATEREPOSITORY_E_CONFIGURATION_INVALID = (HRESULT)(-2140733437);

	public static readonly HRESULT STATEREPOSITORY_E_UNKNOWN_SCHEMA_VERSION = (HRESULT)(-2140733436);

	public static readonly HRESULT STATEREPOSITORY_ERROR_DICTIONARY_CORRUPTED = (HRESULT)(-2140733435);

	public static readonly HRESULT STATEREPOSITORY_E_BLOCKED = (HRESULT)(-2140733434);

	public static readonly HRESULT STATEREPOSITORY_E_BUSY_RETRY = (HRESULT)(-2140733433);

	public static readonly HRESULT STATEREPOSITORY_E_BUSY_RECOVERY_RETRY = (HRESULT)(-2140733432);

	public static readonly HRESULT STATEREPOSITORY_E_LOCKED_RETRY = (HRESULT)(-2140733431);

	public static readonly HRESULT STATEREPOSITORY_E_LOCKED_SHAREDCACHE_RETRY = (HRESULT)(-2140733430);

	public static readonly HRESULT STATEREPOSITORY_E_TRANSACTION_REQUIRED = (HRESULT)(-2140733429);

	public static readonly HRESULT STATEREPOSITORY_E_BUSY_TIMEOUT_EXCEEDED = (HRESULT)(-2140733428);

	public static readonly HRESULT STATEREPOSITORY_E_BUSY_RECOVERY_TIMEOUT_EXCEEDED = (HRESULT)(-2140733427);

	public static readonly HRESULT STATEREPOSITORY_E_LOCKED_TIMEOUT_EXCEEDED = (HRESULT)(-2140733426);

	public static readonly HRESULT STATEREPOSITORY_E_LOCKED_SHAREDCACHE_TIMEOUT_EXCEEDED = (HRESULT)(-2140733425);

	public static readonly HRESULT STATEREPOSITORY_E_SERVICE_STOP_IN_PROGRESS = (HRESULT)(-2140733424);

	public static readonly HRESULT STATEREPOSTORY_E_NESTED_TRANSACTION_NOT_SUPPORTED = (HRESULT)(-2140733423);

	public static readonly HRESULT STATEREPOSITORY_ERROR_CACHE_CORRUPTED = (HRESULT)(-2140733422);

	public static readonly HRESULT STATEREPOSITORY_TRANSACTION_CALLER_ID_CHANGED = (HRESULT)(6750227);

	public static readonly HRESULT STATEREPOSITORY_TRANSACTION_IN_PROGRESS = (HRESULT)(-2140733420);

	public static readonly HRESULT STATEREPOSITORY_E_CACHE_NOT_INIITALIZED = (HRESULT)(-2140733419);

	public static readonly HRESULT STATEREPOSITORY_E_DEPENDENCY_NOT_RESOLVED = (HRESULT)(-2140733418);

	public static readonly HRESULT ERROR_SPACES_POOL_WAS_DELETED = (HRESULT)(15138817);

	public static readonly HRESULT ERROR_SPACES_FAULT_DOMAIN_TYPE_INVALID = (HRESULT)(-2132344831);

	public static readonly HRESULT ERROR_SPACES_INTERNAL_ERROR = (HRESULT)(-2132344830);

	public static readonly HRESULT ERROR_SPACES_RESILIENCY_TYPE_INVALID = (HRESULT)(-2132344829);

	public static readonly HRESULT ERROR_SPACES_DRIVE_SECTOR_SIZE_INVALID = (HRESULT)(-2132344828);

	public static readonly HRESULT ERROR_SPACES_DRIVE_REDUNDANCY_INVALID = (HRESULT)(-2132344826);

	public static readonly HRESULT ERROR_SPACES_NUMBER_OF_DATA_COPIES_INVALID = (HRESULT)(-2132344825);

	public static readonly HRESULT ERROR_SPACES_PARITY_LAYOUT_INVALID = (HRESULT)(-2132344824);

	public static readonly HRESULT ERROR_SPACES_INTERLEAVE_LENGTH_INVALID = (HRESULT)(-2132344823);

	public static readonly HRESULT ERROR_SPACES_NUMBER_OF_COLUMNS_INVALID = (HRESULT)(-2132344822);

	public static readonly HRESULT ERROR_SPACES_NOT_ENOUGH_DRIVES = (HRESULT)(-2132344821);

	public static readonly HRESULT ERROR_SPACES_EXTENDED_ERROR = (HRESULT)(-2132344820);

	public static readonly HRESULT ERROR_SPACES_PROVISIONING_TYPE_INVALID = (HRESULT)(-2132344819);

	public static readonly HRESULT ERROR_SPACES_ALLOCATION_SIZE_INVALID = (HRESULT)(-2132344818);

	public static readonly HRESULT ERROR_SPACES_ENCLOSURE_AWARE_INVALID = (HRESULT)(-2132344817);

	public static readonly HRESULT ERROR_SPACES_WRITE_CACHE_SIZE_INVALID = (HRESULT)(-2132344816);

	public static readonly HRESULT ERROR_SPACES_NUMBER_OF_GROUPS_INVALID = (HRESULT)(-2132344815);

	public static readonly HRESULT ERROR_SPACES_DRIVE_OPERATIONAL_STATE_INVALID = (HRESULT)(-2132344814);

	public static readonly HRESULT ERROR_SPACES_ENTRY_INCOMPLETE = (HRESULT)(-2132344813);

	public static readonly HRESULT ERROR_SPACES_ENTRY_INVALID = (HRESULT)(-2132344812);

	public static readonly HRESULT ERROR_SPACES_UPDATE_COLUMN_STATE = (HRESULT)(-2132344811);

	public static readonly HRESULT ERROR_SPACES_MAP_REQUIRED = (HRESULT)(-2132344810);

	public static readonly HRESULT ERROR_SPACES_UNSUPPORTED_VERSION = (HRESULT)(-2132344809);

	public static readonly HRESULT ERROR_SPACES_CORRUPT_METADATA = (HRESULT)(-2132344808);

	public static readonly HRESULT ERROR_SPACES_DRT_FULL = (HRESULT)(-2132344807);

	public static readonly HRESULT ERROR_SPACES_INCONSISTENCY = (HRESULT)(-2132344806);

	public static readonly HRESULT ERROR_SPACES_LOG_NOT_READY = (HRESULT)(-2132344805);

	public static readonly HRESULT ERROR_SPACES_NO_REDUNDANCY = (HRESULT)(-2132344804);

	public static readonly HRESULT ERROR_SPACES_DRIVE_NOT_READY = (HRESULT)(-2132344803);

	public static readonly HRESULT ERROR_SPACES_DRIVE_SPLIT = (HRESULT)(-2132344802);

	public static readonly HRESULT ERROR_SPACES_DRIVE_LOST_DATA = (HRESULT)(-2132344801);

	public static readonly HRESULT ERROR_SPACES_MARK_DIRTY = (HRESULT)(-2132344800);

	public static readonly HRESULT ERROR_SPACES_FLUSH_METADATA = (HRESULT)(-2132344795);

	public static readonly HRESULT ERROR_SPACES_CACHE_FULL = (HRESULT)(-2132344794);

	public static readonly HRESULT ERROR_SPACES_REPAIR_IN_PROGRESS = (HRESULT)(-2132344793);

	public static readonly HRESULT ERROR_VOLSNAP_BOOTFILE_NOT_VALID = (HRESULT)(-2138963967);

	public static readonly HRESULT ERROR_VOLSNAP_ACTIVATION_TIMEOUT = (HRESULT)(-2138963966);

	public static readonly HRESULT ERROR_VOLSNAP_NO_BYPASSIO_WITH_SNAPSHOT = (HRESULT)(-2138963965);

	public static readonly HRESULT ERROR_TIERING_NOT_SUPPORTED_ON_VOLUME = (HRESULT)(-2138898431);

	public static readonly HRESULT ERROR_TIERING_VOLUME_DISMOUNT_IN_PROGRESS = (HRESULT)(-2138898430);

	public static readonly HRESULT ERROR_TIERING_STORAGE_TIER_NOT_FOUND = (HRESULT)(-2138898429);

	public static readonly HRESULT ERROR_TIERING_INVALID_FILE_ID = (HRESULT)(-2138898428);

	public static readonly HRESULT ERROR_TIERING_WRONG_CLUSTER_NODE = (HRESULT)(-2138898427);

	public static readonly HRESULT ERROR_TIERING_ALREADY_PROCESSING = (HRESULT)(-2138898426);

	public static readonly HRESULT ERROR_TIERING_CANNOT_PIN_OBJECT = (HRESULT)(-2138898425);

	public static readonly HRESULT ERROR_TIERING_FILE_IS_NOT_PINNED = (HRESULT)(-2138898424);

	public static readonly HRESULT ERROR_NOT_A_TIERED_VOLUME = (HRESULT)(-2138898423);

	public static readonly HRESULT ERROR_ATTRIBUTE_NOT_PRESENT = (HRESULT)(-2138898422);

	public static readonly HRESULT ERROR_SECCORE_INVALID_COMMAND = (HRESULT)(-1058537472);

	public static readonly HRESULT ERROR_NO_APPLICABLE_APP_LICENSES_FOUND = (HRESULT)(-1058406399);

	public static readonly HRESULT ERROR_CLIP_LICENSE_NOT_FOUND = (HRESULT)(-1058406398);

	public static readonly HRESULT ERROR_CLIP_DEVICE_LICENSE_MISSING = (HRESULT)(-1058406397);

	public static readonly HRESULT ERROR_CLIP_LICENSE_INVALID_SIGNATURE = (HRESULT)(-1058406396);

	public static readonly HRESULT ERROR_CLIP_KEYHOLDER_LICENSE_MISSING_OR_INVALID = (HRESULT)(-1058406395);

	public static readonly HRESULT ERROR_CLIP_LICENSE_EXPIRED = (HRESULT)(-1058406394);

	public static readonly HRESULT ERROR_CLIP_LICENSE_SIGNED_BY_UNKNOWN_SOURCE = (HRESULT)(-1058406393);

	public static readonly HRESULT ERROR_CLIP_LICENSE_NOT_SIGNED = (HRESULT)(-1058406392);

	public static readonly HRESULT ERROR_CLIP_LICENSE_HARDWARE_ID_OUT_OF_TOLERANCE = (HRESULT)(-1058406391);

	public static readonly HRESULT ERROR_CLIP_LICENSE_DEVICE_ID_MISMATCH = (HRESULT)(-1058406390);

	public static readonly HRESULT DXGI_STATUS_OCCLUDED = (HRESULT)(142213121);

	public static readonly HRESULT DXGI_STATUS_CLIPPED = (HRESULT)(142213122);

	public static readonly HRESULT DXGI_STATUS_NO_REDIRECTION = (HRESULT)(142213124);

	public static readonly HRESULT DXGI_STATUS_NO_DESKTOP_ACCESS = (HRESULT)(142213125);

	public static readonly HRESULT DXGI_STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE = (HRESULT)(142213126);

	public static readonly HRESULT DXGI_STATUS_MODE_CHANGED = (HRESULT)(142213127);

	public static readonly HRESULT DXGI_STATUS_MODE_CHANGE_IN_PROGRESS = (HRESULT)(142213128);

	public static readonly HRESULT DXCORE_ERROR_EVENT_NOT_UNREGISTERED = (HRESULT)(-2004877311);

	public static readonly HRESULT PRESENTATION_ERROR_LOST = (HRESULT)(-2004811775);

	public static readonly HRESULT DXGI_STATUS_UNOCCLUDED = (HRESULT)(142213129);

	public static readonly HRESULT DXGI_STATUS_DDA_WAS_STILL_DRAWING = (HRESULT)(142213130);

	public static readonly HRESULT DXGI_STATUS_PRESENT_REQUIRED = (HRESULT)(142213167);

	public static readonly HRESULT DXGI_DDI_ERR_WASSTILLDRAWING = (HRESULT)(-2005204991);

	public static readonly HRESULT DXGI_DDI_ERR_UNSUPPORTED = (HRESULT)(-2005204990);

	public static readonly HRESULT DXGI_DDI_ERR_NONEXCLUSIVE = (HRESULT)(-2005204989);

	public static readonly HRESULT D3D10_ERROR_TOO_MANY_UNIQUE_STATE_OBJECTS = (HRESULT)(-2005336063);

	public static readonly HRESULT D3D10_ERROR_FILE_NOT_FOUND = (HRESULT)(-2005336062);

	public static readonly HRESULT D3D11_ERROR_TOO_MANY_UNIQUE_STATE_OBJECTS = (HRESULT)(-2005139455);

	public static readonly HRESULT D3D11_ERROR_FILE_NOT_FOUND = (HRESULT)(-2005139454);

	public static readonly HRESULT D3D11_ERROR_TOO_MANY_UNIQUE_VIEW_OBJECTS = (HRESULT)(-2005139453);

	public static readonly HRESULT D3D11_ERROR_DEFERRED_CONTEXT_MAP_WITHOUT_INITIAL_DISCARD = (HRESULT)(-2005139452);

	public static readonly HRESULT D3D12_ERROR_ADAPTER_NOT_FOUND = (HRESULT)(-2005008383);

	public static readonly HRESULT D3D12_ERROR_DRIVER_VERSION_MISMATCH = (HRESULT)(-2005008382);

	public static readonly HRESULT D3D12_ERROR_INVALID_REDIST = (HRESULT)(-2005008381);

	public static readonly HRESULT D2DERR_WRONG_STATE = (HRESULT)(-2003238911);

	public static readonly HRESULT D2DERR_NOT_INITIALIZED = (HRESULT)(-2003238910);

	public static readonly HRESULT D2DERR_UNSUPPORTED_OPERATION = (HRESULT)(-2003238909);

	public static readonly HRESULT D2DERR_SCANNER_FAILED = (HRESULT)(-2003238908);

	public static readonly HRESULT D2DERR_SCREEN_ACCESS_DENIED = (HRESULT)(-2003238907);

	public static readonly HRESULT D2DERR_DISPLAY_STATE_INVALID = (HRESULT)(-2003238906);

	public static readonly HRESULT D2DERR_ZERO_VECTOR = (HRESULT)(-2003238905);

	public static readonly HRESULT D2DERR_INTERNAL_ERROR = (HRESULT)(-2003238904);

	public static readonly HRESULT D2DERR_DISPLAY_FORMAT_NOT_SUPPORTED = (HRESULT)(-2003238903);

	public static readonly HRESULT D2DERR_INVALID_CALL = (HRESULT)(-2003238902);

	public static readonly HRESULT D2DERR_NO_HARDWARE_DEVICE = (HRESULT)(-2003238901);

	public static readonly HRESULT D2DERR_RECREATE_TARGET = (HRESULT)(-2003238900);

	public static readonly HRESULT D2DERR_TOO_MANY_SHADER_ELEMENTS = (HRESULT)(-2003238899);

	public static readonly HRESULT D2DERR_SHADER_COMPILE_FAILED = (HRESULT)(-2003238898);

	public static readonly HRESULT D2DERR_MAX_TEXTURE_SIZE_EXCEEDED = (HRESULT)(-2003238897);

	public static readonly HRESULT D2DERR_UNSUPPORTED_VERSION = (HRESULT)(-2003238896);

	public static readonly HRESULT D2DERR_BAD_NUMBER = (HRESULT)(-2003238895);

	public static readonly HRESULT D2DERR_WRONG_FACTORY = (HRESULT)(-2003238894);

	public static readonly HRESULT D2DERR_LAYER_ALREADY_IN_USE = (HRESULT)(-2003238893);

	public static readonly HRESULT D2DERR_POP_CALL_DID_NOT_MATCH_PUSH = (HRESULT)(-2003238892);

	public static readonly HRESULT D2DERR_WRONG_RESOURCE_DOMAIN = (HRESULT)(-2003238891);

	public static readonly HRESULT D2DERR_PUSH_POP_UNBALANCED = (HRESULT)(-2003238890);

	public static readonly HRESULT D2DERR_RENDER_TARGET_HAS_LAYER_OR_CLIPRECT = (HRESULT)(-2003238889);

	public static readonly HRESULT D2DERR_INCOMPATIBLE_BRUSH_TYPES = (HRESULT)(-2003238888);

	public static readonly HRESULT D2DERR_WIN32_ERROR = (HRESULT)(-2003238887);

	public static readonly HRESULT D2DERR_TARGET_NOT_GDI_COMPATIBLE = (HRESULT)(-2003238886);

	public static readonly HRESULT D2DERR_TEXT_EFFECT_IS_WRONG_TYPE = (HRESULT)(-2003238885);

	public static readonly HRESULT D2DERR_TEXT_RENDERER_NOT_RELEASED = (HRESULT)(-2003238884);

	public static readonly HRESULT D2DERR_EXCEEDS_MAX_BITMAP_SIZE = (HRESULT)(-2003238883);

	public static readonly HRESULT D2DERR_INVALID_GRAPH_CONFIGURATION = (HRESULT)(-2003238882);

	public static readonly HRESULT D2DERR_INVALID_INTERNAL_GRAPH_CONFIGURATION = (HRESULT)(-2003238881);

	public static readonly HRESULT D2DERR_CYCLIC_GRAPH = (HRESULT)(-2003238880);

	public static readonly HRESULT D2DERR_BITMAP_CANNOT_DRAW = (HRESULT)(-2003238879);

	public static readonly HRESULT D2DERR_OUTSTANDING_BITMAP_REFERENCES = (HRESULT)(-2003238878);

	public static readonly HRESULT D2DERR_ORIGINAL_TARGET_NOT_BOUND = (HRESULT)(-2003238877);

	public static readonly HRESULT D2DERR_INVALID_TARGET = (HRESULT)(-2003238876);

	public static readonly HRESULT D2DERR_BITMAP_BOUND_AS_TARGET = (HRESULT)(-2003238875);

	public static readonly HRESULT D2DERR_INSUFFICIENT_DEVICE_CAPABILITIES = (HRESULT)(-2003238874);

	public static readonly HRESULT D2DERR_INTERMEDIATE_TOO_LARGE = (HRESULT)(-2003238873);

	public static readonly HRESULT D2DERR_EFFECT_IS_NOT_REGISTERED = (HRESULT)(-2003238872);

	public static readonly HRESULT D2DERR_INVALID_PROPERTY = (HRESULT)(-2003238871);

	public static readonly HRESULT D2DERR_NO_SUBPROPERTIES = (HRESULT)(-2003238870);

	public static readonly HRESULT D2DERR_PRINT_JOB_CLOSED = (HRESULT)(-2003238869);

	public static readonly HRESULT D2DERR_PRINT_FORMAT_NOT_SUPPORTED = (HRESULT)(-2003238868);

	public static readonly HRESULT D2DERR_TOO_MANY_TRANSFORM_INPUTS = (HRESULT)(-2003238867);

	public static readonly HRESULT D2DERR_INVALID_GLYPH_IMAGE = (HRESULT)(-2003238866);

	public static readonly HRESULT DWRITE_E_FILEFORMAT = (HRESULT)(-2003283968);

	public static readonly HRESULT DWRITE_E_UNEXPECTED = (HRESULT)(-2003283967);

	public static readonly HRESULT DWRITE_E_NOFONT = (HRESULT)(-2003283966);

	public static readonly HRESULT DWRITE_E_FILENOTFOUND = (HRESULT)(-2003283965);

	public static readonly HRESULT DWRITE_E_FILEACCESS = (HRESULT)(-2003283964);

	public static readonly HRESULT DWRITE_E_FONTCOLLECTIONOBSOLETE = (HRESULT)(-2003283963);

	public static readonly HRESULT DWRITE_E_ALREADYREGISTERED = (HRESULT)(-2003283962);

	public static readonly HRESULT DWRITE_E_CACHEFORMAT = (HRESULT)(-2003283961);

	public static readonly HRESULT DWRITE_E_CACHEVERSION = (HRESULT)(-2003283960);

	public static readonly HRESULT DWRITE_E_UNSUPPORTEDOPERATION = (HRESULT)(-2003283959);

	public static readonly HRESULT DWRITE_E_TEXTRENDERERINCOMPATIBLE = (HRESULT)(-2003283958);

	public static readonly HRESULT DWRITE_E_FLOWDIRECTIONCONFLICTS = (HRESULT)(-2003283957);

	public static readonly HRESULT DWRITE_E_NOCOLOR = (HRESULT)(-2003283956);

	public static readonly HRESULT WINCODEC_ERR_WRONGSTATE = (HRESULT)(-2003292412);

	public static readonly HRESULT WINCODEC_ERR_VALUEOUTOFRANGE = (HRESULT)(-2003292411);

	public static readonly HRESULT WINCODEC_ERR_UNKNOWNIMAGEFORMAT = (HRESULT)(-2003292409);

	public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDVERSION = (HRESULT)(-2003292405);

	public static readonly HRESULT WINCODEC_ERR_NOTINITIALIZED = (HRESULT)(-2003292404);

	public static readonly HRESULT WINCODEC_ERR_ALREADYLOCKED = (HRESULT)(-2003292403);

	public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTFOUND = (HRESULT)(-2003292352);

	public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTSUPPORTED = (HRESULT)(-2003292351);

	public static readonly HRESULT WINCODEC_ERR_PROPERTYSIZE = (HRESULT)(-2003292350);

	public static readonly HRESULT WINCODEC_ERR_CODECPRESENT = (HRESULT)(-2003292349);

	public static readonly HRESULT WINCODEC_ERR_CODECNOTHUMBNAIL = (HRESULT)(-2003292348);

	public static readonly HRESULT WINCODEC_ERR_PALETTEUNAVAILABLE = (HRESULT)(-2003292347);

	public static readonly HRESULT WINCODEC_ERR_CODECTOOMANYSCANLINES = (HRESULT)(-2003292346);

	public static readonly HRESULT WINCODEC_ERR_INTERNALERROR = (HRESULT)(-2003292344);

	public static readonly HRESULT WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS = (HRESULT)(-2003292343);

	public static readonly HRESULT WINCODEC_ERR_COMPONENTNOTFOUND = (HRESULT)(-2003292336);

	public static readonly HRESULT WINCODEC_ERR_IMAGESIZEOUTOFRANGE = (HRESULT)(-2003292335);

	public static readonly HRESULT WINCODEC_ERR_TOOMUCHMETADATA = (HRESULT)(-2003292334);

	public static readonly HRESULT WINCODEC_ERR_BADIMAGE = (HRESULT)(-2003292320);

	public static readonly HRESULT WINCODEC_ERR_BADHEADER = (HRESULT)(-2003292319);

	public static readonly HRESULT WINCODEC_ERR_FRAMEMISSING = (HRESULT)(-2003292318);

	public static readonly HRESULT WINCODEC_ERR_BADMETADATAHEADER = (HRESULT)(-2003292317);

	public static readonly HRESULT WINCODEC_ERR_BADSTREAMDATA = (HRESULT)(-2003292304);

	public static readonly HRESULT WINCODEC_ERR_STREAMWRITE = (HRESULT)(-2003292303);

	public static readonly HRESULT WINCODEC_ERR_STREAMREAD = (HRESULT)(-2003292302);

	public static readonly HRESULT WINCODEC_ERR_STREAMNOTAVAILABLE = (HRESULT)(-2003292301);

	public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT = (HRESULT)(-2003292288);

	public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDOPERATION = (HRESULT)(-2003292287);

	public static readonly HRESULT WINCODEC_ERR_INVALIDREGISTRATION = (HRESULT)(-2003292278);

	public static readonly HRESULT WINCODEC_ERR_COMPONENTINITIALIZEFAILURE = (HRESULT)(-2003292277);

	public static readonly HRESULT WINCODEC_ERR_INSUFFICIENTBUFFER = (HRESULT)(-2003292276);

	public static readonly HRESULT WINCODEC_ERR_DUPLICATEMETADATAPRESENT = (HRESULT)(-2003292275);

	public static readonly HRESULT WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE = (HRESULT)(-2003292274);

	public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDSIZE = (HRESULT)(-2003292273);

	public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYREQUEST = (HRESULT)(-2003292272);

	public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDMETADATATYPE = (HRESULT)(-2003292271);

	public static readonly HRESULT WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT = (HRESULT)(-2003292270);

	public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYCHARACTER = (HRESULT)(-2003292269);

	public static readonly HRESULT WINCODEC_ERR_WIN32ERROR = (HRESULT)(-2003292268);

	public static readonly HRESULT WINCODEC_ERR_INVALIDPROGRESSIVELEVEL = (HRESULT)(-2003292267);

	public static readonly HRESULT WINCODEC_ERR_INVALIDJPEGSCANINDEX = (HRESULT)(-2003292266);

	public static readonly HRESULT MILERR_OBJECTBUSY = (HRESULT)(-2003304447);

	public static readonly HRESULT MILERR_INSUFFICIENTBUFFER = (HRESULT)(-2003304446);

	public static readonly HRESULT MILERR_WIN32ERROR = (HRESULT)(-2003304445);

	public static readonly HRESULT MILERR_SCANNER_FAILED = (HRESULT)(-2003304444);

	public static readonly HRESULT MILERR_SCREENACCESSDENIED = (HRESULT)(-2003304443);

	public static readonly HRESULT MILERR_DISPLAYSTATEINVALID = (HRESULT)(-2003304442);

	public static readonly HRESULT MILERR_NONINVERTIBLEMATRIX = (HRESULT)(-2003304441);

	public static readonly HRESULT MILERR_ZEROVECTOR = (HRESULT)(-2003304440);

	public static readonly HRESULT MILERR_TERMINATED = (HRESULT)(-2003304439);

	public static readonly HRESULT MILERR_BADNUMBER = (HRESULT)(-2003304438);

	public static readonly HRESULT MILERR_INTERNALERROR = (HRESULT)(-2003304320);

	public static readonly HRESULT MILERR_DISPLAYFORMATNOTSUPPORTED = (HRESULT)(-2003304316);

	public static readonly HRESULT MILERR_INVALIDCALL = (HRESULT)(-2003304315);

	public static readonly HRESULT MILERR_ALREADYLOCKED = (HRESULT)(-2003304314);

	public static readonly HRESULT MILERR_NOTLOCKED = (HRESULT)(-2003304313);

	public static readonly HRESULT MILERR_DEVICECANNOTRENDERTEXT = (HRESULT)(-2003304312);

	public static readonly HRESULT MILERR_GLYPHBITMAPMISSED = (HRESULT)(-2003304311);

	public static readonly HRESULT MILERR_MALFORMEDGLYPHCACHE = (HRESULT)(-2003304310);

	public static readonly HRESULT MILERR_GENERIC_IGNORE = (HRESULT)(-2003304309);

	public static readonly HRESULT MILERR_MALFORMED_GUIDELINE_DATA = (HRESULT)(-2003304308);

	public static readonly HRESULT MILERR_NO_HARDWARE_DEVICE = (HRESULT)(-2003304307);

	public static readonly HRESULT MILERR_NEED_RECREATE_AND_PRESENT = (HRESULT)(-2003304306);

	public static readonly HRESULT MILERR_ALREADY_INITIALIZED = (HRESULT)(-2003304305);

	public static readonly HRESULT MILERR_MISMATCHED_SIZE = (HRESULT)(-2003304304);

	public static readonly HRESULT MILERR_NO_REDIRECTION_SURFACE_AVAILABLE = (HRESULT)(-2003304303);

	public static readonly HRESULT MILERR_REMOTING_NOT_SUPPORTED = (HRESULT)(-2003304302);

	public static readonly HRESULT MILERR_QUEUED_PRESENT_NOT_SUPPORTED = (HRESULT)(-2003304301);

	public static readonly HRESULT MILERR_NOT_QUEUING_PRESENTS = (HRESULT)(-2003304300);

	public static readonly HRESULT MILERR_NO_REDIRECTION_SURFACE_RETRY_LATER = (HRESULT)(-2003304299);

	public static readonly HRESULT MILERR_TOOMANYSHADERELEMNTS = (HRESULT)(-2003304298);

	public static readonly HRESULT MILERR_MROW_READLOCK_FAILED = (HRESULT)(-2003304297);

	public static readonly HRESULT MILERR_MROW_UPDATE_FAILED = (HRESULT)(-2003304296);

	public static readonly HRESULT MILERR_SHADER_COMPILE_FAILED = (HRESULT)(-2003304295);

	public static readonly HRESULT MILERR_MAX_TEXTURE_SIZE_EXCEEDED = (HRESULT)(-2003304294);

	public static readonly HRESULT MILERR_QPC_TIME_WENT_BACKWARD = (HRESULT)(-2003304293);

	public static readonly HRESULT MILERR_DXGI_ENUMERATION_OUT_OF_SYNC = (HRESULT)(-2003304291);

	public static readonly HRESULT MILERR_ADAPTER_NOT_FOUND = (HRESULT)(-2003304290);

	public static readonly HRESULT MILERR_COLORSPACE_NOT_SUPPORTED = (HRESULT)(-2003304289);

	public static readonly HRESULT MILERR_PREFILTER_NOT_SUPPORTED = (HRESULT)(-2003304288);

	public static readonly HRESULT MILERR_DISPLAYID_ACCESS_DENIED = (HRESULT)(-2003304287);

	public static readonly HRESULT UCEERR_INVALIDPACKETHEADER = (HRESULT)(-2003303424);

	public static readonly HRESULT UCEERR_UNKNOWNPACKET = (HRESULT)(-2003303423);

	public static readonly HRESULT UCEERR_ILLEGALPACKET = (HRESULT)(-2003303422);

	public static readonly HRESULT UCEERR_MALFORMEDPACKET = (HRESULT)(-2003303421);

	public static readonly HRESULT UCEERR_ILLEGALHANDLE = (HRESULT)(-2003303420);

	public static readonly HRESULT UCEERR_HANDLELOOKUPFAILED = (HRESULT)(-2003303419);

	public static readonly HRESULT UCEERR_RENDERTHREADFAILURE = (HRESULT)(-2003303418);

	public static readonly HRESULT UCEERR_CTXSTACKFRSTTARGETNULL = (HRESULT)(-2003303417);

	public static readonly HRESULT UCEERR_CONNECTIONIDLOOKUPFAILED = (HRESULT)(-2003303416);

	public static readonly HRESULT UCEERR_BLOCKSFULL = (HRESULT)(-2003303415);

	public static readonly HRESULT UCEERR_MEMORYFAILURE = (HRESULT)(-2003303414);

	public static readonly HRESULT UCEERR_PACKETRECORDOUTOFRANGE = (HRESULT)(-2003303413);

	public static readonly HRESULT UCEERR_ILLEGALRECORDTYPE = (HRESULT)(-2003303412);

	public static readonly HRESULT UCEERR_OUTOFHANDLES = (HRESULT)(-2003303411);

	public static readonly HRESULT UCEERR_UNCHANGABLE_UPDATE_ATTEMPTED = (HRESULT)(-2003303410);

	public static readonly HRESULT UCEERR_NO_MULTIPLE_WORKER_THREADS = (HRESULT)(-2003303409);

	public static readonly HRESULT UCEERR_REMOTINGNOTSUPPORTED = (HRESULT)(-2003303408);

	public static readonly HRESULT UCEERR_MISSINGENDCOMMAND = (HRESULT)(-2003303407);

	public static readonly HRESULT UCEERR_MISSINGBEGINCOMMAND = (HRESULT)(-2003303406);

	public static readonly HRESULT UCEERR_CHANNELSYNCTIMEDOUT = (HRESULT)(-2003303405);

	public static readonly HRESULT UCEERR_CHANNELSYNCABANDONED = (HRESULT)(-2003303404);

	public static readonly HRESULT UCEERR_UNSUPPORTEDTRANSPORTVERSION = (HRESULT)(-2003303403);

	public static readonly HRESULT UCEERR_TRANSPORTUNAVAILABLE = (HRESULT)(-2003303402);

	public static readonly HRESULT UCEERR_FEEDBACK_UNSUPPORTED = (HRESULT)(-2003303401);

	public static readonly HRESULT UCEERR_COMMANDTRANSPORTDENIED = (HRESULT)(-2003303400);

	public static readonly HRESULT UCEERR_GRAPHICSSTREAMUNAVAILABLE = (HRESULT)(-2003303399);

	public static readonly HRESULT UCEERR_GRAPHICSSTREAMALREADYOPEN = (HRESULT)(-2003303392);

	public static readonly HRESULT UCEERR_TRANSPORTDISCONNECTED = (HRESULT)(-2003303391);

	public static readonly HRESULT UCEERR_TRANSPORTOVERLOADED = (HRESULT)(-2003303390);

	public static readonly HRESULT UCEERR_PARTITION_ZOMBIED = (HRESULT)(-2003303389);

	public static readonly HRESULT MILAVERR_NOCLOCK = (HRESULT)(-2003303168);

	public static readonly HRESULT MILAVERR_NOMEDIATYPE = (HRESULT)(-2003303167);

	public static readonly HRESULT MILAVERR_NOVIDEOMIXER = (HRESULT)(-2003303166);

	public static readonly HRESULT MILAVERR_NOVIDEOPRESENTER = (HRESULT)(-2003303165);

	public static readonly HRESULT MILAVERR_NOREADYFRAMES = (HRESULT)(-2003303164);

	public static readonly HRESULT MILAVERR_MODULENOTLOADED = (HRESULT)(-2003303163);

	public static readonly HRESULT MILAVERR_WMPFACTORYNOTREGISTERED = (HRESULT)(-2003303162);

	public static readonly HRESULT MILAVERR_INVALIDWMPVERSION = (HRESULT)(-2003303161);

	public static readonly HRESULT MILAVERR_INSUFFICIENTVIDEORESOURCES = (HRESULT)(-2003303160);

	public static readonly HRESULT MILAVERR_VIDEOACCELERATIONNOTAVAILABLE = (HRESULT)(-2003303159);

	public static readonly HRESULT MILAVERR_REQUESTEDTEXTURETOOBIG = (HRESULT)(-2003303158);

	public static readonly HRESULT MILAVERR_SEEKFAILED = (HRESULT)(-2003303157);

	public static readonly HRESULT MILAVERR_UNEXPECTEDWMPFAILURE = (HRESULT)(-2003303156);

	public static readonly HRESULT MILAVERR_MEDIAPLAYERCLOSED = (HRESULT)(-2003303155);

	public static readonly HRESULT MILAVERR_UNKNOWNHARDWAREERROR = (HRESULT)(-2003303154);

	public static readonly HRESULT MILEFFECTSERR_UNKNOWNPROPERTY = (HRESULT)(-2003302898);

	public static readonly HRESULT MILEFFECTSERR_EFFECTNOTPARTOFGROUP = (HRESULT)(-2003302897);

	public static readonly HRESULT MILEFFECTSERR_NOINPUTSOURCEATTACHED = (HRESULT)(-2003302896);

	public static readonly HRESULT MILEFFECTSERR_CONNECTORNOTCONNECTED = (HRESULT)(-2003302895);

	public static readonly HRESULT MILEFFECTSERR_CONNECTORNOTASSOCIATEDWITHEFFECT = (HRESULT)(-2003302894);

	public static readonly HRESULT MILEFFECTSERR_RESERVED = (HRESULT)(-2003302893);

	public static readonly HRESULT MILEFFECTSERR_CYCLEDETECTED = (HRESULT)(-2003302892);

	public static readonly HRESULT MILEFFECTSERR_EFFECTINMORETHANONEGRAPH = (HRESULT)(-2003302891);

	public static readonly HRESULT MILEFFECTSERR_EFFECTALREADYINAGRAPH = (HRESULT)(-2003302890);

	public static readonly HRESULT MILEFFECTSERR_EFFECTHASNOCHILDREN = (HRESULT)(-2003302889);

	public static readonly HRESULT MILEFFECTSERR_ALREADYATTACHEDTOLISTENER = (HRESULT)(-2003302888);

	public static readonly HRESULT MILEFFECTSERR_NOTAFFINETRANSFORM = (HRESULT)(-2003302887);

	public static readonly HRESULT MILEFFECTSERR_EMPTYBOUNDS = (HRESULT)(-2003302886);

	public static readonly HRESULT MILEFFECTSERR_OUTPUTSIZETOOLARGE = (HRESULT)(-2003302885);

	public static readonly HRESULT DWMERR_STATE_TRANSITION_FAILED = (HRESULT)(-2003302656);

	public static readonly HRESULT DWMERR_THEME_FAILED = (HRESULT)(-2003302655);

	public static readonly HRESULT DWMERR_CATASTROPHIC_FAILURE = (HRESULT)(-2003302654);

	public static readonly HRESULT DCOMPOSITION_ERROR_WINDOW_ALREADY_COMPOSED = (HRESULT)(-2003302400);

	public static readonly HRESULT DCOMPOSITION_ERROR_SURFACE_BEING_RENDERED = (HRESULT)(-2003302399);

	public static readonly HRESULT DCOMPOSITION_ERROR_SURFACE_NOT_BEING_RENDERED = (HRESULT)(-2003302398);

	public static readonly HRESULT ONL_E_INVALID_AUTHENTICATION_TARGET = (HRESULT)(-2138701823);

	public static readonly HRESULT ONL_E_ACCESS_DENIED_BY_TOU = (HRESULT)(-2138701822);

	public static readonly HRESULT ONL_E_INVALID_APPLICATION = (HRESULT)(-2138701821);

	public static readonly HRESULT ONL_E_PASSWORD_UPDATE_REQUIRED = (HRESULT)(-2138701820);

	public static readonly HRESULT ONL_E_ACCOUNT_UPDATE_REQUIRED = (HRESULT)(-2138701819);

	public static readonly HRESULT ONL_E_FORCESIGNIN = (HRESULT)(-2138701818);

	public static readonly HRESULT ONL_E_ACCOUNT_LOCKED = (HRESULT)(-2138701817);

	public static readonly HRESULT ONL_E_PARENTAL_CONSENT_REQUIRED = (HRESULT)(-2138701816);

	public static readonly HRESULT ONL_E_EMAIL_VERIFICATION_REQUIRED = (HRESULT)(-2138701815);

	public static readonly HRESULT ONL_E_ACCOUNT_SUSPENDED_COMPROIMISE = (HRESULT)(-2138701814);

	public static readonly HRESULT ONL_E_ACCOUNT_SUSPENDED_ABUSE = (HRESULT)(-2138701813);

	public static readonly HRESULT ONL_E_ACTION_REQUIRED = (HRESULT)(-2138701812);

	public static readonly HRESULT ONL_CONNECTION_COUNT_LIMIT = (HRESULT)(-2138701811);

	public static readonly HRESULT ONL_E_CONNECTED_ACCOUNT_CAN_NOT_SIGNOUT = (HRESULT)(-2138701810);

	public static readonly HRESULT ONL_E_USER_AUTHENTICATION_REQUIRED = (HRESULT)(-2138701809);

	public static readonly HRESULT ONL_E_REQUEST_THROTTLED = (HRESULT)(-2138701808);

	public static readonly HRESULT FA_E_MAX_PERSISTED_ITEMS_REACHED = (HRESULT)(-2144927200);

	public static readonly HRESULT FA_E_HOMEGROUP_NOT_AVAILABLE = (HRESULT)(-2144927198);

	public static readonly HRESULT E_MONITOR_RESOLUTION_TOO_LOW = (HRESULT)(-2144927152);

	public static readonly HRESULT E_ELEVATED_ACTIVATION_NOT_SUPPORTED = (HRESULT)(-2144927151);

	public static readonly HRESULT E_UAC_DISABLED = (HRESULT)(-2144927150);

	public static readonly HRESULT E_FULL_ADMIN_NOT_SUPPORTED = (HRESULT)(-2144927149);

	public static readonly HRESULT E_APPLICATION_NOT_REGISTERED = (HRESULT)(-2144927148);

	public static readonly HRESULT E_MULTIPLE_EXTENSIONS_FOR_APPLICATION = (HRESULT)(-2144927147);

	public static readonly HRESULT E_MULTIPLE_PACKAGES_FOR_FAMILY = (HRESULT)(-2144927146);

	public static readonly HRESULT E_APPLICATION_MANAGER_NOT_RUNNING = (HRESULT)(-2144927145);

	public static readonly HRESULT S_STORE_LAUNCHED_FOR_REMEDIATION = (HRESULT)(2556504);

	public static readonly HRESULT S_APPLICATION_ACTIVATION_ERROR_HANDLED_BY_DIALOG = (HRESULT)(2556505);

	public static readonly HRESULT E_APPLICATION_ACTIVATION_TIMED_OUT = (HRESULT)(-2144927142);

	public static readonly HRESULT E_APPLICATION_ACTIVATION_EXEC_FAILURE = (HRESULT)(-2144927141);

	public static readonly HRESULT E_APPLICATION_TEMPORARY_LICENSE_ERROR = (HRESULT)(-2144927140);

	public static readonly HRESULT E_APPLICATION_TRIAL_LICENSE_EXPIRED = (HRESULT)(-2144927139);

	public static readonly HRESULT E_SKYDRIVE_ROOT_TARGET_FILE_SYSTEM_NOT_SUPPORTED = (HRESULT)(-2144927136);

	public static readonly HRESULT E_SKYDRIVE_ROOT_TARGET_OVERLAP = (HRESULT)(-2144927135);

	public static readonly HRESULT E_SKYDRIVE_ROOT_TARGET_CANNOT_INDEX = (HRESULT)(-2144927134);

	public static readonly HRESULT E_SKYDRIVE_FILE_NOT_UPLOADED = (HRESULT)(-2144927133);

	public static readonly HRESULT E_SKYDRIVE_UPDATE_AVAILABILITY_FAIL = (HRESULT)(-2144927132);

	public static readonly HRESULT E_SKYDRIVE_ROOT_TARGET_VOLUME_ROOT_NOT_SUPPORTED = (HRESULT)(-2144927131);

	public static readonly HRESULT E_SYNCENGINE_FILE_SIZE_OVER_LIMIT = (HRESULT)(-2013089791);

	public static readonly HRESULT E_SYNCENGINE_FILE_SIZE_EXCEEDS_REMAINING_QUOTA = (HRESULT)(-2013089790);

	public static readonly HRESULT E_SYNCENGINE_UNSUPPORTED_FILE_NAME = (HRESULT)(-2013089789);

	public static readonly HRESULT E_SYNCENGINE_FOLDER_ITEM_COUNT_LIMIT_EXCEEDED = (HRESULT)(-2013089788);

	public static readonly HRESULT E_SYNCENGINE_FILE_SYNC_PARTNER_ERROR = (HRESULT)(-2013089787);

	public static readonly HRESULT E_SYNCENGINE_SYNC_PAUSED_BY_SERVICE = (HRESULT)(-2013089786);

	public static readonly HRESULT E_SYNCENGINE_FILE_IDENTIFIER_UNKNOWN = (HRESULT)(-2013085694);

	public static readonly HRESULT E_SYNCENGINE_SERVICE_AUTHENTICATION_FAILED = (HRESULT)(-2013085693);

	public static readonly HRESULT E_SYNCENGINE_UNKNOWN_SERVICE_ERROR = (HRESULT)(-2013085692);

	public static readonly HRESULT E_SYNCENGINE_SERVICE_RETURNED_UNEXPECTED_SIZE = (HRESULT)(-2013085691);

	public static readonly HRESULT E_SYNCENGINE_REQUEST_BLOCKED_BY_SERVICE = (HRESULT)(-2013085690);

	public static readonly HRESULT E_SYNCENGINE_REQUEST_BLOCKED_DUE_TO_CLIENT_ERROR = (HRESULT)(-2013085689);

	public static readonly HRESULT E_SYNCENGINE_FOLDER_INACCESSIBLE = (HRESULT)(-2013081599);

	public static readonly HRESULT E_SYNCENGINE_UNSUPPORTED_FOLDER_NAME = (HRESULT)(-2013081598);

	public static readonly HRESULT E_SYNCENGINE_UNSUPPORTED_MARKET = (HRESULT)(-2013081597);

	public static readonly HRESULT E_SYNCENGINE_PATH_LENGTH_LIMIT_EXCEEDED = (HRESULT)(-2013081596);

	public static readonly HRESULT E_SYNCENGINE_REMOTE_PATH_LENGTH_LIMIT_EXCEEDED = (HRESULT)(-2013081595);

	public static readonly HRESULT E_SYNCENGINE_CLIENT_UPDATE_NEEDED = (HRESULT)(-2013081594);

	public static readonly HRESULT E_SYNCENGINE_PROXY_AUTHENTICATION_REQUIRED = (HRESULT)(-2013081593);

	public static readonly HRESULT E_SYNCENGINE_STORAGE_SERVICE_PROVISIONING_FAILED = (HRESULT)(-2013081592);

	public static readonly HRESULT E_SYNCENGINE_UNSUPPORTED_REPARSE_POINT = (HRESULT)(-2013081591);

	public static readonly HRESULT E_SYNCENGINE_STORAGE_SERVICE_BLOCKED = (HRESULT)(-2013081590);

	public static readonly HRESULT E_SYNCENGINE_FOLDER_IN_REDIRECTION = (HRESULT)(-2013081589);

	public static readonly HRESULT EAS_E_POLICY_NOT_MANAGED_BY_OS = (HRESULT)(-2141913087);

	public static readonly HRESULT EAS_E_POLICY_COMPLIANT_WITH_ACTIONS = (HRESULT)(-2141913086);

	public static readonly HRESULT EAS_E_REQUESTED_POLICY_NOT_ENFORCEABLE = (HRESULT)(-2141913085);

	public static readonly HRESULT EAS_E_CURRENT_USER_HAS_BLANK_PASSWORD = (HRESULT)(-2141913084);

	public static readonly HRESULT EAS_E_REQUESTED_POLICY_PASSWORD_EXPIRATION_INCOMPATIBLE = (HRESULT)(-2141913083);

	public static readonly HRESULT EAS_E_USER_CANNOT_CHANGE_PASSWORD = (HRESULT)(-2141913082);

	public static readonly HRESULT EAS_E_ADMINS_HAVE_BLANK_PASSWORD = (HRESULT)(-2141913081);

	public static readonly HRESULT EAS_E_ADMINS_CANNOT_CHANGE_PASSWORD = (HRESULT)(-2141913080);

	public static readonly HRESULT EAS_E_LOCAL_CONTROLLED_USERS_CANNOT_CHANGE_PASSWORD = (HRESULT)(-2141913079);

	public static readonly HRESULT EAS_E_PASSWORD_POLICY_NOT_ENFORCEABLE_FOR_CONNECTED_ADMINS = (HRESULT)(-2141913078);

	public static readonly HRESULT EAS_E_CONNECTED_ADMINS_NEED_TO_CHANGE_PASSWORD = (HRESULT)(-2141913077);

	public static readonly HRESULT EAS_E_PASSWORD_POLICY_NOT_ENFORCEABLE_FOR_CURRENT_CONNECTED_USER = (HRESULT)(-2141913076);

	public static readonly HRESULT EAS_E_CURRENT_CONNECTED_USER_NEED_TO_CHANGE_PASSWORD = (HRESULT)(-2141913075);

	public static readonly HRESULT WEB_E_UNSUPPORTED_FORMAT = (HRESULT)(-2089484287);

	public static readonly HRESULT WEB_E_INVALID_XML = (HRESULT)(-2089484286);

	public static readonly HRESULT WEB_E_MISSING_REQUIRED_ELEMENT = (HRESULT)(-2089484285);

	public static readonly HRESULT WEB_E_MISSING_REQUIRED_ATTRIBUTE = (HRESULT)(-2089484284);

	public static readonly HRESULT WEB_E_UNEXPECTED_CONTENT = (HRESULT)(-2089484283);

	public static readonly HRESULT WEB_E_RESOURCE_TOO_LARGE = (HRESULT)(-2089484282);

	public static readonly HRESULT WEB_E_INVALID_JSON_STRING = (HRESULT)(-2089484281);

	public static readonly HRESULT WEB_E_INVALID_JSON_NUMBER = (HRESULT)(-2089484280);

	public static readonly HRESULT WEB_E_JSON_VALUE_NOT_FOUND = (HRESULT)(-2089484279);

	public static readonly HRESULT HTTP_E_STATUS_UNEXPECTED = (HRESULT)(-2145845247);

	public static readonly HRESULT HTTP_E_STATUS_UNEXPECTED_REDIRECTION = (HRESULT)(-2145845245);

	public static readonly HRESULT HTTP_E_STATUS_UNEXPECTED_CLIENT_ERROR = (HRESULT)(-2145845244);

	public static readonly HRESULT HTTP_E_STATUS_UNEXPECTED_SERVER_ERROR = (HRESULT)(-2145845243);

	public static readonly HRESULT HTTP_E_STATUS_AMBIGUOUS = (HRESULT)(-2145844948);

	public static readonly HRESULT HTTP_E_STATUS_MOVED = (HRESULT)(-2145844947);

	public static readonly HRESULT HTTP_E_STATUS_REDIRECT = (HRESULT)(-2145844946);

	public static readonly HRESULT HTTP_E_STATUS_REDIRECT_METHOD = (HRESULT)(-2145844945);

	public static readonly HRESULT HTTP_E_STATUS_NOT_MODIFIED = (HRESULT)(-2145844944);

	public static readonly HRESULT HTTP_E_STATUS_USE_PROXY = (HRESULT)(-2145844943);

	public static readonly HRESULT HTTP_E_STATUS_REDIRECT_KEEP_VERB = (HRESULT)(-2145844941);

	public static readonly HRESULT HTTP_E_STATUS_BAD_REQUEST = (HRESULT)(-2145844848);

	public static readonly HRESULT HTTP_E_STATUS_DENIED = (HRESULT)(-2145844847);

	public static readonly HRESULT HTTP_E_STATUS_PAYMENT_REQ = (HRESULT)(-2145844846);

	public static readonly HRESULT HTTP_E_STATUS_FORBIDDEN = (HRESULT)(-2145844845);

	public static readonly HRESULT HTTP_E_STATUS_NOT_FOUND = (HRESULT)(-2145844844);

	public static readonly HRESULT HTTP_E_STATUS_BAD_METHOD = (HRESULT)(-2145844843);

	public static readonly HRESULT HTTP_E_STATUS_NONE_ACCEPTABLE = (HRESULT)(-2145844842);

	public static readonly HRESULT HTTP_E_STATUS_PROXY_AUTH_REQ = (HRESULT)(-2145844841);

	public static readonly HRESULT HTTP_E_STATUS_REQUEST_TIMEOUT = (HRESULT)(-2145844840);

	public static readonly HRESULT HTTP_E_STATUS_CONFLICT = (HRESULT)(-2145844839);

	public static readonly HRESULT HTTP_E_STATUS_GONE = (HRESULT)(-2145844838);

	public static readonly HRESULT HTTP_E_STATUS_LENGTH_REQUIRED = (HRESULT)(-2145844837);

	public static readonly HRESULT HTTP_E_STATUS_PRECOND_FAILED = (HRESULT)(-2145844836);

	public static readonly HRESULT HTTP_E_STATUS_REQUEST_TOO_LARGE = (HRESULT)(-2145844835);

	public static readonly HRESULT HTTP_E_STATUS_URI_TOO_LONG = (HRESULT)(-2145844834);

	public static readonly HRESULT HTTP_E_STATUS_UNSUPPORTED_MEDIA = (HRESULT)(-2145844833);

	public static readonly HRESULT HTTP_E_STATUS_RANGE_NOT_SATISFIABLE = (HRESULT)(-2145844832);

	public static readonly HRESULT HTTP_E_STATUS_EXPECTATION_FAILED = (HRESULT)(-2145844831);

	public static readonly HRESULT HTTP_E_STATUS_SERVER_ERROR = (HRESULT)(-2145844748);

	public static readonly HRESULT HTTP_E_STATUS_NOT_SUPPORTED = (HRESULT)(-2145844747);

	public static readonly HRESULT HTTP_E_STATUS_BAD_GATEWAY = (HRESULT)(-2145844746);

	public static readonly HRESULT HTTP_E_STATUS_SERVICE_UNAVAIL = (HRESULT)(-2145844745);

	public static readonly HRESULT HTTP_E_STATUS_GATEWAY_TIMEOUT = (HRESULT)(-2145844744);

	public static readonly HRESULT HTTP_E_STATUS_VERSION_NOT_SUP = (HRESULT)(-2145844743);

	public static readonly HRESULT E_INVALID_PROTOCOL_OPERATION = (HRESULT)(-2089418751);

	public static readonly HRESULT E_INVALID_PROTOCOL_FORMAT = (HRESULT)(-2089418750);

	public static readonly HRESULT E_PROTOCOL_EXTENSIONS_NOT_SUPPORTED = (HRESULT)(-2089418749);

	public static readonly HRESULT E_SUBPROTOCOL_NOT_SUPPORTED = (HRESULT)(-2089418748);

	public static readonly HRESULT E_PROTOCOL_VERSION_NOT_SUPPORTED = (HRESULT)(-2089418747);

	public static readonly HRESULT INPUT_E_OUT_OF_ORDER = (HRESULT)(-2143289344);

	public static readonly HRESULT INPUT_E_REENTRANCY = (HRESULT)(-2143289343);

	public static readonly HRESULT INPUT_E_MULTIMODAL = (HRESULT)(-2143289342);

	public static readonly HRESULT INPUT_E_PACKET = (HRESULT)(-2143289341);

	public static readonly HRESULT INPUT_E_FRAME = (HRESULT)(-2143289340);

	public static readonly HRESULT INPUT_E_HISTORY = (HRESULT)(-2143289339);

	public static readonly HRESULT INPUT_E_DEVICE_INFO = (HRESULT)(-2143289338);

	public static readonly HRESULT INPUT_E_TRANSFORM = (HRESULT)(-2143289337);

	public static readonly HRESULT INPUT_E_DEVICE_PROPERTY = (HRESULT)(-2143289336);

	public static readonly HRESULT ERROR_DBG_CREATE_PROCESS_FAILURE_LOCKDOWN = (HRESULT)(-2135949311);

	public static readonly HRESULT ERROR_DBG_ATTACH_PROCESS_FAILURE_LOCKDOWN = (HRESULT)(-2135949310);

	public static readonly HRESULT ERROR_DBG_CONNECT_SERVER_FAILURE_LOCKDOWN = (HRESULT)(-2135949309);

	public static readonly HRESULT ERROR_DBG_START_SERVER_FAILURE_LOCKDOWN = (HRESULT)(-2135949308);

	public static readonly HRESULT HSP_E_ERROR_MASK = (HRESULT)(-2128084992);

	public static readonly HRESULT HSP_E_INTERNAL_ERROR = (HRESULT)(-2128080897);

	public static readonly HRESULT HSP_BS_ERROR_MASK = (HRESULT)(-2128080896);

	public static readonly HRESULT HSP_BS_INTERNAL_ERROR = (HRESULT)(-2128080641);

	public static readonly HRESULT HSP_DRV_ERROR_MASK = (HRESULT)(-2128019456);

	public static readonly HRESULT HSP_DRV_INTERNAL_ERROR = (HRESULT)(-2128019201);

	public static readonly HRESULT HSP_BASE_ERROR_MASK = (HRESULT)(-2128019200);

	public static readonly HRESULT HSP_BASE_INTERNAL_ERROR = (HRESULT)(-2128018945);

	public static readonly HRESULT HSP_KSP_ERROR_MASK = (HRESULT)(-2128018944);

	public static readonly HRESULT HSP_KSP_DEVICE_NOT_READY = (HRESULT)(-2128018943);

	public static readonly HRESULT HSP_KSP_INVALID_PROVIDER_HANDLE = (HRESULT)(-2128018942);

	public static readonly HRESULT HSP_KSP_INVALID_KEY_HANDLE = (HRESULT)(-2128018941);

	public static readonly HRESULT HSP_KSP_INVALID_PARAMETER = (HRESULT)(-2128018940);

	public static readonly HRESULT HSP_KSP_BUFFER_TOO_SMALL = (HRESULT)(-2128018939);

	public static readonly HRESULT HSP_KSP_NOT_SUPPORTED = (HRESULT)(-2128018938);

	public static readonly HRESULT HSP_KSP_INVALID_DATA = (HRESULT)(-2128018937);

	public static readonly HRESULT HSP_KSP_INVALID_FLAGS = (HRESULT)(-2128018936);

	public static readonly HRESULT HSP_KSP_ALGORITHM_NOT_SUPPORTED = (HRESULT)(-2128018935);

	public static readonly HRESULT HSP_KSP_KEY_ALREADY_FINALIZED = (HRESULT)(-2128018934);

	public static readonly HRESULT HSP_KSP_KEY_NOT_FINALIZED = (HRESULT)(-2128018933);

	public static readonly HRESULT HSP_KSP_INVALID_KEY_TYPE = (HRESULT)(-2128018932);

	public static readonly HRESULT HSP_KSP_NO_MEMORY = (HRESULT)(-2128018928);

	public static readonly HRESULT HSP_KSP_PARAMETER_NOT_SET = (HRESULT)(-2128018927);

	public static readonly HRESULT HSP_KSP_KEY_EXISTS = (HRESULT)(-2128018923);

	public static readonly HRESULT HSP_KSP_KEY_MISSING = (HRESULT)(-2128018922);

	public static readonly HRESULT HSP_KSP_KEY_LOAD_FAIL = (HRESULT)(-2128018921);

	public static readonly HRESULT HSP_KSP_NO_MORE_ITEMS = (HRESULT)(-2128018920);

	public static readonly HRESULT HSP_KSP_INTERNAL_ERROR = (HRESULT)(-2128018689);

	public static readonly HRESULT ERROR_IO_PREEMPTED = (HRESULT)(-1996423167);

	public static readonly HRESULT JSCRIPT_E_CANTEXECUTE = (HRESULT)(-1996357631);

	public static readonly HRESULT WEP_E_NOT_PROVISIONED_ON_ALL_VOLUMES = (HRESULT)(-2013200383);

	public static readonly HRESULT WEP_E_FIXED_DATA_NOT_SUPPORTED = (HRESULT)(-2013200382);

	public static readonly HRESULT WEP_E_HARDWARE_NOT_COMPLIANT = (HRESULT)(-2013200381);

	public static readonly HRESULT WEP_E_LOCK_NOT_CONFIGURED = (HRESULT)(-2013200380);

	public static readonly HRESULT WEP_E_PROTECTION_SUSPENDED = (HRESULT)(-2013200379);

	public static readonly HRESULT WEP_E_NO_LICENSE = (HRESULT)(-2013200378);

	public static readonly HRESULT WEP_E_OS_NOT_PROTECTED = (HRESULT)(-2013200377);

	public static readonly HRESULT WEP_E_UNEXPECTED_FAIL = (HRESULT)(-2013200376);

	public static readonly HRESULT WEP_E_BUFFER_TOO_LARGE = (HRESULT)(-2013200375);

	public static readonly HRESULT ERROR_SVHDX_ERROR_STORED = (HRESULT)(-1067712512);

	public static readonly HRESULT ERROR_SVHDX_ERROR_NOT_AVAILABLE = (HRESULT)(-1067647232);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_AVAILABLE = (HRESULT)(-1067647231);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_CAPACITY_DATA_CHANGED = (HRESULT)(-1067647230);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_RESERVATIONS_PREEMPTED = (HRESULT)(-1067647229);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_RESERVATIONS_RELEASED = (HRESULT)(-1067647228);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_REGISTRATIONS_PREEMPTED = (HRESULT)(-1067647227);

	public static readonly HRESULT ERROR_SVHDX_UNIT_ATTENTION_OPERATING_DEFINITION_CHANGED = (HRESULT)(-1067647226);

	public static readonly HRESULT ERROR_SVHDX_RESERVATION_CONFLICT = (HRESULT)(-1067647225);

	public static readonly HRESULT ERROR_SVHDX_WRONG_FILE_TYPE = (HRESULT)(-1067647224);

	public static readonly HRESULT ERROR_SVHDX_VERSION_MISMATCH = (HRESULT)(-1067647223);

	public static readonly HRESULT ERROR_VHD_SHARED = (HRESULT)(-1067647222);

	public static readonly HRESULT ERROR_SVHDX_NO_INITIATOR = (HRESULT)(-1067647221);

	public static readonly HRESULT ERROR_VHDSET_BACKING_STORAGE_NOT_FOUND = (HRESULT)(-1067647220);

	public static readonly HRESULT ERROR_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP = (HRESULT)(-1067646976);

	public static readonly HRESULT ERROR_SMB_BAD_CLUSTER_DIALECT = (HRESULT)(-1067646975);

	public static readonly HRESULT ERROR_SMB_NO_SIGNING_ALGORITHM_OVERLAP = (HRESULT)(-1067646974);

	public static readonly HRESULT WININET_E_OUT_OF_HANDLES = (HRESULT)(-2147012895);

	public static readonly HRESULT WININET_E_TIMEOUT = (HRESULT)(-2147012894);

	public static readonly HRESULT WININET_E_EXTENDED_ERROR = (HRESULT)(-2147012893);

	public static readonly HRESULT WININET_E_INTERNAL_ERROR = (HRESULT)(-2147012892);

	public static readonly HRESULT WININET_E_INVALID_URL = (HRESULT)(-2147012891);

	public static readonly HRESULT WININET_E_UNRECOGNIZED_SCHEME = (HRESULT)(-2147012890);

	public static readonly HRESULT WININET_E_NAME_NOT_RESOLVED = (HRESULT)(-2147012889);

	public static readonly HRESULT WININET_E_PROTOCOL_NOT_FOUND = (HRESULT)(-2147012888);

	public static readonly HRESULT WININET_E_INVALID_OPTION = (HRESULT)(-2147012887);

	public static readonly HRESULT WININET_E_BAD_OPTION_LENGTH = (HRESULT)(-2147012886);

	public static readonly HRESULT WININET_E_OPTION_NOT_SETTABLE = (HRESULT)(-2147012885);

	public static readonly HRESULT WININET_E_SHUTDOWN = (HRESULT)(-2147012884);

	public static readonly HRESULT WININET_E_INCORRECT_USER_NAME = (HRESULT)(-2147012883);

	public static readonly HRESULT WININET_E_INCORRECT_PASSWORD = (HRESULT)(-2147012882);

	public static readonly HRESULT WININET_E_LOGIN_FAILURE = (HRESULT)(-2147012881);

	public static readonly HRESULT WININET_E_INVALID_OPERATION = (HRESULT)(-2147012880);

	public static readonly HRESULT WININET_E_OPERATION_CANCELLED = (HRESULT)(-2147012879);

	public static readonly HRESULT WININET_E_INCORRECT_HANDLE_TYPE = (HRESULT)(-2147012878);

	public static readonly HRESULT WININET_E_INCORRECT_HANDLE_STATE = (HRESULT)(-2147012877);

	public static readonly HRESULT WININET_E_NOT_PROXY_REQUEST = (HRESULT)(-2147012876);

	public static readonly HRESULT WININET_E_REGISTRY_VALUE_NOT_FOUND = (HRESULT)(-2147012875);

	public static readonly HRESULT WININET_E_BAD_REGISTRY_PARAMETER = (HRESULT)(-2147012874);

	public static readonly HRESULT WININET_E_NO_DIRECT_ACCESS = (HRESULT)(-2147012873);

	public static readonly HRESULT WININET_E_NO_CONTEXT = (HRESULT)(-2147012872);

	public static readonly HRESULT WININET_E_NO_CALLBACK = (HRESULT)(-2147012871);

	public static readonly HRESULT WININET_E_REQUEST_PENDING = (HRESULT)(-2147012870);

	public static readonly HRESULT WININET_E_INCORRECT_FORMAT = (HRESULT)(-2147012869);

	public static readonly HRESULT WININET_E_ITEM_NOT_FOUND = (HRESULT)(-2147012868);

	public static readonly HRESULT WININET_E_CANNOT_CONNECT = (HRESULT)(-2147012867);

	public static readonly HRESULT WININET_E_CONNECTION_ABORTED = (HRESULT)(-2147012866);

	public static readonly HRESULT WININET_E_CONNECTION_RESET = (HRESULT)(-2147012865);

	public static readonly HRESULT WININET_E_FORCE_RETRY = (HRESULT)(-2147012864);

	public static readonly HRESULT WININET_E_INVALID_PROXY_REQUEST = (HRESULT)(-2147012863);

	public static readonly HRESULT WININET_E_NEED_UI = (HRESULT)(-2147012862);

	public static readonly HRESULT WININET_E_HANDLE_EXISTS = (HRESULT)(-2147012860);

	public static readonly HRESULT WININET_E_SEC_CERT_DATE_INVALID = (HRESULT)(-2147012859);

	public static readonly HRESULT WININET_E_SEC_CERT_CN_INVALID = (HRESULT)(-2147012858);

	public static readonly HRESULT WININET_E_HTTP_TO_HTTPS_ON_REDIR = (HRESULT)(-2147012857);

	public static readonly HRESULT WININET_E_HTTPS_TO_HTTP_ON_REDIR = (HRESULT)(-2147012856);

	public static readonly HRESULT WININET_E_MIXED_SECURITY = (HRESULT)(-2147012855);

	public static readonly HRESULT WININET_E_CHG_POST_IS_NON_SECURE = (HRESULT)(-2147012854);

	public static readonly HRESULT WININET_E_POST_IS_NON_SECURE = (HRESULT)(-2147012853);

	public static readonly HRESULT WININET_E_CLIENT_AUTH_CERT_NEEDED = (HRESULT)(-2147012852);

	public static readonly HRESULT WININET_E_INVALID_CA = (HRESULT)(-2147012851);

	public static readonly HRESULT WININET_E_CLIENT_AUTH_NOT_SETUP = (HRESULT)(-2147012850);

	public static readonly HRESULT WININET_E_ASYNC_THREAD_FAILED = (HRESULT)(-2147012849);

	public static readonly HRESULT WININET_E_REDIRECT_SCHEME_CHANGE = (HRESULT)(-2147012848);

	public static readonly HRESULT WININET_E_DIALOG_PENDING = (HRESULT)(-2147012847);

	public static readonly HRESULT WININET_E_RETRY_DIALOG = (HRESULT)(-2147012846);

	public static readonly HRESULT WININET_E_NO_NEW_CONTAINERS = (HRESULT)(-2147012845);

	public static readonly HRESULT WININET_E_HTTPS_HTTP_SUBMIT_REDIR = (HRESULT)(-2147012844);

	public static readonly HRESULT WININET_E_SEC_CERT_ERRORS = (HRESULT)(-2147012841);

	public static readonly HRESULT WININET_E_SEC_CERT_REV_FAILED = (HRESULT)(-2147012839);

	public static readonly HRESULT WININET_E_HEADER_NOT_FOUND = (HRESULT)(-2147012746);

	public static readonly HRESULT WININET_E_DOWNLEVEL_SERVER = (HRESULT)(-2147012745);

	public static readonly HRESULT WININET_E_INVALID_SERVER_RESPONSE = (HRESULT)(-2147012744);

	public static readonly HRESULT WININET_E_INVALID_HEADER = (HRESULT)(-2147012743);

	public static readonly HRESULT WININET_E_INVALID_QUERY_REQUEST = (HRESULT)(-2147012742);

	public static readonly HRESULT WININET_E_HEADER_ALREADY_EXISTS = (HRESULT)(-2147012741);

	public static readonly HRESULT WININET_E_REDIRECT_FAILED = (HRESULT)(-2147012740);

	public static readonly HRESULT WININET_E_SECURITY_CHANNEL_ERROR = (HRESULT)(-2147012739);

	public static readonly HRESULT WININET_E_UNABLE_TO_CACHE_FILE = (HRESULT)(-2147012738);

	public static readonly HRESULT WININET_E_TCPIP_NOT_INSTALLED = (HRESULT)(-2147012737);

	public static readonly HRESULT WININET_E_DISCONNECTED = (HRESULT)(-2147012733);

	public static readonly HRESULT WININET_E_SERVER_UNREACHABLE = (HRESULT)(-2147012732);

	public static readonly HRESULT WININET_E_PROXY_SERVER_UNREACHABLE = (HRESULT)(-2147012731);

	public static readonly HRESULT WININET_E_BAD_AUTO_PROXY_SCRIPT = (HRESULT)(-2147012730);

	public static readonly HRESULT WININET_E_UNABLE_TO_DOWNLOAD_SCRIPT = (HRESULT)(-2147012729);

	public static readonly HRESULT WININET_E_SEC_INVALID_CERT = (HRESULT)(-2147012727);

	public static readonly HRESULT WININET_E_SEC_CERT_REVOKED = (HRESULT)(-2147012726);

	public static readonly HRESULT WININET_E_FAILED_DUETOSECURITYCHECK = (HRESULT)(-2147012725);

	public static readonly HRESULT WININET_E_NOT_INITIALIZED = (HRESULT)(-2147012724);

	public static readonly HRESULT WININET_E_LOGIN_FAILURE_DISPLAY_ENTITY_BODY = (HRESULT)(-2147012722);

	public static readonly HRESULT WININET_E_DECODING_FAILED = (HRESULT)(-2147012721);

	public static readonly HRESULT WININET_E_NOT_REDIRECTED = (HRESULT)(-2147012736);

	public static readonly HRESULT WININET_E_COOKIE_NEEDS_CONFIRMATION = (HRESULT)(-2147012735);

	public static readonly HRESULT WININET_E_COOKIE_DECLINED = (HRESULT)(-2147012734);

	public static readonly HRESULT WININET_E_REDIRECT_NEEDS_CONFIRMATION = (HRESULT)(-2147012728);

	#region SQLite Errors

	public static readonly HRESULT SQLITE_E_ERROR = (HRESULT)(-2018574335);
	public static readonly HRESULT SQLITE_E_INTERNAL = (HRESULT)(-2018574334);
	public static readonly HRESULT SQLITE_E_PERM = (HRESULT)(-2018574333);
	public static readonly HRESULT SQLITE_E_ABORT = (HRESULT)(-2018574332);
	public static readonly HRESULT SQLITE_E_BUSY = (HRESULT)(-2018574331);
	public static readonly HRESULT SQLITE_E_LOCKED = (HRESULT)(-2018574330);
	public static readonly HRESULT SQLITE_E_NOMEM = (HRESULT)(-2018574329);
	public static readonly HRESULT SQLITE_E_READONLY = (HRESULT)(-2018574328);
	public static readonly HRESULT SQLITE_E_INTERRUPT = (HRESULT)(-2018574327);
	public static readonly HRESULT SQLITE_E_IOERR = (HRESULT)(-2018574326);
	public static readonly HRESULT SQLITE_E_CORRUPT = (HRESULT)(-2018574325);
	public static readonly HRESULT SQLITE_E_NOTFOUND = (HRESULT)(-2018574324);
	public static readonly HRESULT SQLITE_E_FULL = (HRESULT)(-2018574323);
	public static readonly HRESULT SQLITE_E_CANTOPEN = (HRESULT)(-2018574322);
	public static readonly HRESULT SQLITE_E_PROTOCOL = (HRESULT)(-2018574321);
	public static readonly HRESULT SQLITE_E_EMPTY = (HRESULT)(-2018574320);
	public static readonly HRESULT SQLITE_E_SCHEMA = (HRESULT)(-2018574319);
	public static readonly HRESULT SQLITE_E_TOOBIG = (HRESULT)(-2018574318);
	public static readonly HRESULT SQLITE_E_CONSTRAINT = (HRESULT)(-2018574317);
	public static readonly HRESULT SQLITE_E_MISMATCH = (HRESULT)(-2018574316);
	public static readonly HRESULT SQLITE_E_MISUSE = (HRESULT)(-2018574315);
	public static readonly HRESULT SQLITE_E_NOLFS = (HRESULT)(-2018574314);
	public static readonly HRESULT SQLITE_E_AUTH = (HRESULT)(-2018574313);
	public static readonly HRESULT SQLITE_E_FORMAT = (HRESULT)(-2018574312);
	public static readonly HRESULT SQLITE_E_RANGE = (HRESULT)(-2018574311);
	public static readonly HRESULT SQLITE_E_NOTADB = (HRESULT)(-2018574310);
	public static readonly HRESULT SQLITE_E_NOTICE = (HRESULT)(-2018574309);
	public static readonly HRESULT SQLITE_E_WARNING = (HRESULT)(-2018574308);
	public static readonly HRESULT SQLITE_E_ROW = (HRESULT)(-2018574236);
	public static readonly HRESULT SQLITE_E_DONE = (HRESULT)(-2018574235);
	public static readonly HRESULT SQLITE_E_IOERR_READ = (HRESULT)(-2018574070);
	public static readonly HRESULT SQLITE_E_IOERR_SHORT_READ = (HRESULT)(-2018573814);
	public static readonly HRESULT SQLITE_E_IOERR_WRITE = (HRESULT)(-2018573558);
	public static readonly HRESULT SQLITE_E_IOERR_FSYNC = (HRESULT)(-2018573302);
	public static readonly HRESULT SQLITE_E_IOERR_DIR_FSYNC = (HRESULT)(-2018573046);
	public static readonly HRESULT SQLITE_E_IOERR_TRUNCATE = (HRESULT)(-2018572790);
	public static readonly HRESULT SQLITE_E_IOERR_FSTAT = (HRESULT)(-2018572534);
	public static readonly HRESULT SQLITE_E_IOERR_UNLOCK = (HRESULT)(-2018572278);
	public static readonly HRESULT SQLITE_E_IOERR_RDLOCK = (HRESULT)(-2018572022);
	public static readonly HRESULT SQLITE_E_IOERR_DELETE = (HRESULT)(-2018571766);
	public static readonly HRESULT SQLITE_E_IOERR_BLOCKED = (HRESULT)(-2018571510);
	public static readonly HRESULT SQLITE_E_IOERR_NOMEM = (HRESULT)(-2018571254);
	public static readonly HRESULT SQLITE_E_IOERR_ACCESS = (HRESULT)(-2018570998);
	public static readonly HRESULT SQLITE_E_IOERR_CHECKRESERVEDLOCK = (HRESULT)(-2018570742);
	public static readonly HRESULT SQLITE_E_IOERR_LOCK = (HRESULT)(-2018570486);
	public static readonly HRESULT SQLITE_E_IOERR_CLOSE = (HRESULT)(-2018570230);
	public static readonly HRESULT SQLITE_E_IOERR_DIR_CLOSE = (HRESULT)(-2018569974);
	public static readonly HRESULT SQLITE_E_IOERR_SHMOPEN = (HRESULT)(-2018569718);
	public static readonly HRESULT SQLITE_E_IOERR_SHMSIZE = (HRESULT)(-2018569462);
	public static readonly HRESULT SQLITE_E_IOERR_SHMLOCK = (HRESULT)(-2018569206);
	public static readonly HRESULT SQLITE_E_IOERR_SHMMAP = (HRESULT)(-2018568950);
	public static readonly HRESULT SQLITE_E_IOERR_SEEK = (HRESULT)(-2018568694);
	public static readonly HRESULT SQLITE_E_IOERR_DELETE_NOENT = (HRESULT)(-2018568438);
	public static readonly HRESULT SQLITE_E_IOERR_MMAP = (HRESULT)(-2018568182);
	public static readonly HRESULT SQLITE_E_IOERR_GETTEMPPATH = (HRESULT)(-2018567926);
	public static readonly HRESULT SQLITE_E_IOERR_CONVPATH = (HRESULT)(-2018567670);
	public static readonly HRESULT SQLITE_E_IOERR_VNODE = (HRESULT)(-2018567678);
	public static readonly HRESULT SQLITE_E_IOERR_AUTH = (HRESULT)(-2018567677);
	public static readonly HRESULT SQLITE_E_LOCKED_SHAREDCACHE = (HRESULT)(-2018574074);
	public static readonly HRESULT SQLITE_E_BUSY_RECOVERY = (HRESULT)(-2018574075);
	public static readonly HRESULT SQLITE_E_BUSY_SNAPSHOT = (HRESULT)(-2018573819);
	public static readonly HRESULT SQLITE_E_CANTOPEN_NOTEMPDIR = (HRESULT)(-2018574066);
	public static readonly HRESULT SQLITE_E_CANTOPEN_ISDIR = (HRESULT)(-2018573810);
	public static readonly HRESULT SQLITE_E_CANTOPEN_FULLPATH = (HRESULT)(-2018573554);
	public static readonly HRESULT SQLITE_E_CANTOPEN_CONVPATH = (HRESULT)(-2018573298);
	public static readonly HRESULT SQLITE_E_CORRUPT_VTAB = (HRESULT)(-2018574069);
	public static readonly HRESULT SQLITE_E_READONLY_RECOVERY = (HRESULT)(-2018574072);
	public static readonly HRESULT SQLITE_E_READONLY_CANTLOCK = (HRESULT)(-2018573816);
	public static readonly HRESULT SQLITE_E_READONLY_ROLLBACK = (HRESULT)(-2018573560);
	public static readonly HRESULT SQLITE_E_READONLY_DBMOVED = (HRESULT)(-2018573304);
	public static readonly HRESULT SQLITE_E_ABORT_ROLLBACK = (HRESULT)(-2018573820);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_CHECK = (HRESULT)(-2018574061);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_COMMITHOOK = (HRESULT)(-2018573805);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_FOREIGNKEY = (HRESULT)(-2018573549);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_FUNCTION = (HRESULT)(-2018573293);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_NOTNULL = (HRESULT)(-2018573037);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_PRIMARYKEY = (HRESULT)(-2018572781);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_TRIGGER = (HRESULT)(-2018572525);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_UNIQUE = (HRESULT)(-2018572269);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_VTAB = (HRESULT)(-2018572013);
	public static readonly HRESULT SQLITE_E_CONSTRAINT_ROWID = (HRESULT)(-2018571757);
	public static readonly HRESULT SQLITE_E_NOTICE_RECOVER_WAL = (HRESULT)(-2018574053);
	public static readonly HRESULT SQLITE_E_NOTICE_RECOVER_ROLLBACK = (HRESULT)(-2018573797);
	public static readonly HRESULT SQLITE_E_WARNING_AUTOINDEX = (HRESULT)(-2018574052);

	#endregion

	#region Universal Telemetry Client (UTC) Errors

	public static readonly HRESULT UTC_E_TOGGLE_TRACE_STARTED = (HRESULT)(-2017128447);
	public static readonly HRESULT UTC_E_ALTERNATIVE_TRACE_CANNOT_PREEMPT = (HRESULT)(-2017128446);
	public static readonly HRESULT UTC_E_AOT_NOT_RUNNING = (HRESULT)(-2017128445);
	public static readonly HRESULT UTC_E_SCRIPT_TYPE_INVALID = (HRESULT)(-2017128444);
	public static readonly HRESULT UTC_E_SCENARIODEF_NOT_FOUND = (HRESULT)(-2017128443);
	public static readonly HRESULT UTC_E_TRACEPROFILE_NOT_FOUND = (HRESULT)(-2017128442);
	public static readonly HRESULT UTC_E_FORWARDER_ALREADY_ENABLED = (HRESULT)(-2017128441);
	public static readonly HRESULT UTC_E_FORWARDER_ALREADY_DISABLED = (HRESULT)(-2017128440);
	public static readonly HRESULT UTC_E_EVENTLOG_ENTRY_MALFORMED = (HRESULT)(-2017128439);
	public static readonly HRESULT UTC_E_DIAGRULES_SCHEMAVERSION_MISMATCH = (HRESULT)(-2017128438);
	public static readonly HRESULT UTC_E_SCRIPT_TERMINATED = (HRESULT)(-2017128437);
	public static readonly HRESULT UTC_E_INVALID_CUSTOM_FILTER = (HRESULT)(-2017128436);
	public static readonly HRESULT UTC_E_TRACE_NOT_RUNNING = (HRESULT)(-2017128435);
	public static readonly HRESULT UTC_E_REESCALATED_TOO_QUICKLY = (HRESULT)(-2017128434);
	public static readonly HRESULT UTC_E_ESCALATION_ALREADY_RUNNING = (HRESULT)(-2017128433);
	public static readonly HRESULT UTC_E_PERFTRACK_ALREADY_TRACING = (HRESULT)(-2017128432);
	public static readonly HRESULT UTC_E_REACHED_MAX_ESCALATIONS = (HRESULT)(-2017128431);
	public static readonly HRESULT UTC_E_FORWARDER_PRODUCER_MISMATCH = (HRESULT)(-2017128430);
	public static readonly HRESULT UTC_E_INTENTIONAL_SCRIPT_FAILURE = (HRESULT)(-2017128429);
	public static readonly HRESULT UTC_E_SQM_INIT_FAILED = (HRESULT)(-2017128428);
	public static readonly HRESULT UTC_E_NO_WER_LOGGER_SUPPORTED = (HRESULT)(-2017128427);
	public static readonly HRESULT UTC_E_TRACERS_DONT_EXIST = (HRESULT)(-2017128426);
	public static readonly HRESULT UTC_E_WINRT_INIT_FAILED = (HRESULT)(-2017128425);
	public static readonly HRESULT UTC_E_SCENARIODEF_SCHEMAVERSION_MISMATCH = (HRESULT)(-2017128424);
	public static readonly HRESULT UTC_E_INVALID_FILTER = (HRESULT)(-2017128423);
	public static readonly HRESULT UTC_E_EXE_TERMINATED = (HRESULT)(-2017128422);
	public static readonly HRESULT UTC_E_ESCALATION_NOT_AUTHORIZED = (HRESULT)(-2017128421);
	public static readonly HRESULT UTC_E_SETUP_NOT_AUTHORIZED = (HRESULT)(-2017128420);
	public static readonly HRESULT UTC_E_CHILD_PROCESS_FAILED = (HRESULT)(-2017128419);
	public static readonly HRESULT UTC_E_COMMAND_LINE_NOT_AUTHORIZED = (HRESULT)(-2017128418);
	public static readonly HRESULT UTC_E_CANNOT_LOAD_SCENARIO_EDITOR_XML = (HRESULT)(-2017128417);
	public static readonly HRESULT UTC_E_ESCALATION_TIMED_OUT = (HRESULT)(-2017128416);
	public static readonly HRESULT UTC_E_SETUP_TIMED_OUT = (HRESULT)(-2017128415);
	public static readonly HRESULT UTC_E_TRIGGER_MISMATCH = (HRESULT)(-2017128414);
	public static readonly HRESULT UTC_E_TRIGGER_NOT_FOUND = (HRESULT)(-2017128413);
	public static readonly HRESULT UTC_E_SIF_NOT_SUPPORTED = (HRESULT)(-2017128412);
	public static readonly HRESULT UTC_E_DELAY_TERMINATED = (HRESULT)(-2017128411);
	public static readonly HRESULT UTC_E_DEVICE_TICKET_ERROR = (HRESULT)(-2017128410);
	public static readonly HRESULT UTC_E_TRACE_BUFFER_LIMIT_EXCEEDED = (HRESULT)(-2017128409);
	public static readonly HRESULT UTC_E_API_RESULT_UNAVAILABLE = (HRESULT)(-2017128408);
	public static readonly HRESULT UTC_E_RPC_TIMEOUT = (HRESULT)(-2017128407);
	public static readonly HRESULT UTC_E_RPC_WAIT_FAILED = (HRESULT)(-2017128406);
	public static readonly HRESULT UTC_E_API_BUSY = (HRESULT)(-2017128405);
	public static readonly HRESULT UTC_E_TRACE_MIN_DURATION_REQUIREMENT_NOT_MET = (HRESULT)(-2017128404);
	public static readonly HRESULT UTC_E_EXCLUSIVITY_NOT_AVAILABLE = (HRESULT)(-2017128403);
	public static readonly HRESULT UTC_E_GETFILE_FILE_PATH_NOT_APPROVED = (HRESULT)(-2017128402);
	public static readonly HRESULT UTC_E_ESCALATION_DIRECTORY_ALREADY_EXISTS = (HRESULT)(-2017128401);
	public static readonly HRESULT UTC_E_TIME_TRIGGER_ON_START_INVALID = (HRESULT)(-2017128400);
	public static readonly HRESULT UTC_E_TIME_TRIGGER_ONLY_VALID_ON_SINGLE_TRANSITION = (HRESULT)(-2017128399);
	public static readonly HRESULT UTC_E_TIME_TRIGGER_INVALID_TIME_RANGE = (HRESULT)(-2017128398);
	public static readonly HRESULT UTC_E_MULTIPLE_TIME_TRIGGER_ON_SINGLE_STATE = (HRESULT)(-2017128397);
	public static readonly HRESULT UTC_E_BINARY_MISSING = (HRESULT)(-2017128396);
	public static readonly HRESULT UTC_E_FAILED_TO_RESOLVE_CONTAINER_ID = (HRESULT)(-2017128394);
	public static readonly HRESULT UTC_E_UNABLE_TO_RESOLVE_SESSION = (HRESULT)(-2017128393);
	public static readonly HRESULT UTC_E_THROTTLED = (HRESULT)(-2017128392);
	public static readonly HRESULT UTC_E_UNAPPROVED_SCRIPT = (HRESULT)(-2017128391);
	public static readonly HRESULT UTC_E_SCRIPT_MISSING = (HRESULT)(-2017128390);
	public static readonly HRESULT UTC_E_SCENARIO_THROTTLED = (HRESULT)(-2017128389);
	public static readonly HRESULT UTC_E_API_NOT_SUPPORTED = (HRESULT)(-2017128388);
	public static readonly HRESULT UTC_E_GETFILE_EXTERNAL_PATH_NOT_APPROVED = (HRESULT)(-2017128387);
	public static readonly HRESULT UTC_E_TRY_GET_SCENARIO_TIMEOUT_EXCEEDED = (HRESULT)(-2017128386);
	public static readonly HRESULT UTC_E_CERT_REV_FAILED = (HRESULT)(-2017128385);
	public static readonly HRESULT UTC_E_FAILED_TO_START_NDISCAP = (HRESULT)(-2017128384);
	public static readonly HRESULT UTC_E_KERNELDUMP_LIMIT_REACHED = (HRESULT)(-2017128383);
	public static readonly HRESULT UTC_E_MISSING_AGGREGATE_EVENT_TAG = (HRESULT)(-2017128382);
	public static readonly HRESULT UTC_E_INVALID_AGGREGATION_STRUCT = (HRESULT)(-2017128381);
	public static readonly HRESULT UTC_E_ACTION_NOT_SUPPORTED_IN_DESTINATION = (HRESULT)(-2017128380);
	public static readonly HRESULT UTC_E_FILTER_MISSING_ATTRIBUTE = (HRESULT)(-2017128379);
	public static readonly HRESULT UTC_E_FILTER_INVALID_TYPE = (HRESULT)(-2017128378);
	public static readonly HRESULT UTC_E_FILTER_VARIABLE_NOT_FOUND = (HRESULT)(-2017128377);
	public static readonly HRESULT UTC_E_FILTER_FUNCTION_RESTRICTED = (HRESULT)(-2017128376);
	public static readonly HRESULT UTC_E_FILTER_VERSION_MISMATCH = (HRESULT)(-2017128375);
	public static readonly HRESULT UTC_E_FILTER_INVALID_FUNCTION = (HRESULT)(-2017128368);
	public static readonly HRESULT UTC_E_FILTER_INVALID_FUNCTION_PARAMS = (HRESULT)(-2017128367);
	public static readonly HRESULT UTC_E_FILTER_INVALID_COMMAND = (HRESULT)(-2017128366);
	public static readonly HRESULT UTC_E_FILTER_ILLEGAL_EVAL = (HRESULT)(-2017128365);
	public static readonly HRESULT UTC_E_TTTRACER_RETURNED_ERROR = (HRESULT)(-2017128364);
	public static readonly HRESULT UTC_E_AGENT_DIAGNOSTICS_TOO_LARGE = (HRESULT)(-2017128363);
	public static readonly HRESULT UTC_E_FAILED_TO_RECEIVE_AGENT_DIAGNOSTICS = (HRESULT)(-2017128362);
	public static readonly HRESULT UTC_E_SCENARIO_HAS_NO_ACTIONS = (HRESULT)(-2017128361);
	public static readonly HRESULT UTC_E_TTTRACER_STORAGE_FULL = (HRESULT)(-2017128360);
	public static readonly HRESULT UTC_E_INSUFFICIENT_SPACE_TO_START_TRACE = (HRESULT)(-2017128359);
	public static readonly HRESULT UTC_E_ESCALATION_CANCELLED_AT_SHUTDOWN = (HRESULT)(-2017128358);
	public static readonly HRESULT UTC_E_GETFILEINFOACTION_FILE_NOT_APPROVED = (HRESULT)(-2017128357);
	public static readonly HRESULT UTC_E_SETREGKEYACTION_TYPE_NOT_APPROVED = (HRESULT)(-2017128356);
	public static readonly HRESULT UTC_E_TRACE_THROTTLED = (HRESULT)(-2017128355);

	#endregion

	public static readonly HRESULT WINML_ERR_INVALID_DEVICE = (HRESULT)(-2003828735);

	public static readonly HRESULT WINML_ERR_INVALID_BINDING = (HRESULT)(-2003828734);

	public static readonly HRESULT WINML_ERR_VALUE_NOTFOUND = (HRESULT)(-2003828733);

	public static readonly HRESULT WINML_ERR_SIZE_MISMATCH = (HRESULT)(-2003828732);

	public static readonly HRESULT ERROR_QUIC_HANDSHAKE_FAILURE = (HRESULT)(-2143223808);

	public static readonly HRESULT ERROR_QUIC_VER_NEG_FAILURE = (HRESULT)(-2143223807);

	public static readonly HRESULT ERROR_QUIC_USER_CANCELED = (HRESULT)(-2143223806);

	public static readonly HRESULT ERROR_QUIC_INTERNAL_ERROR = (HRESULT)(-2143223805);

	public static readonly HRESULT ERROR_QUIC_PROTOCOL_VIOLATION = (HRESULT)(-2143223804);

	public static readonly HRESULT ERROR_QUIC_CONNECTION_IDLE = (HRESULT)(-2143223803);

	public static readonly HRESULT ERROR_QUIC_CONNECTION_TIMEOUT = (HRESULT)(-2143223802);

	public static readonly HRESULT ERROR_QUIC_ALPN_NEG_FAILURE = (HRESULT)(-2143223801);

	public static readonly HRESULT IORING_E_REQUIRED_FLAG_NOT_SUPPORTED = (HRESULT)(-2142896127);

	public static readonly HRESULT IORING_E_SUBMISSION_QUEUE_FULL = (HRESULT)(-2142896126);

	public static readonly HRESULT IORING_E_VERSION_NOT_SUPPORTED = (HRESULT)(-2142896125);

	public static readonly HRESULT IORING_E_SUBMISSION_QUEUE_TOO_BIG = (HRESULT)(-2142896124);

	public static readonly HRESULT IORING_E_COMPLETION_QUEUE_TOO_BIG = (HRESULT)(-2142896123);

	public static readonly HRESULT IORING_E_SUBMIT_IN_PROGRESS = (HRESULT)(-2142896122);

	public static readonly HRESULT IORING_E_CORRUPT = (HRESULT)(-2142896121);

	public static readonly HRESULT IORING_E_COMPLETION_QUEUE_TOO_FULL = (HRESULT)(-2142896120);

	public static readonly HRESULT NOT_AN_ERROR1 = (HRESULT)(529920);

	public static readonly HRESULT QUERY_E_FAILED = (HRESULT)(-2147215872);

	public static readonly HRESULT QUERY_E_INVALIDQUERY = (HRESULT)(-2147215871);

	public static readonly HRESULT QUERY_E_INVALIDRESTRICTION = (HRESULT)(-2147215870);

	public static readonly HRESULT QUERY_E_INVALIDSORT = (HRESULT)(-2147215869);

	public static readonly HRESULT QUERY_E_INVALIDCATEGORIZE = (HRESULT)(-2147215868);

	public static readonly HRESULT QUERY_E_ALLNOISE = (HRESULT)(-2147215867);

	public static readonly HRESULT QUERY_E_TOOCOMPLEX = (HRESULT)(-2147215866);

	public static readonly HRESULT QUERY_E_TIMEDOUT = (HRESULT)(-2147215865);

	public static readonly HRESULT QUERY_E_DUPLICATE_OUTPUT_COLUMN = (HRESULT)(-2147215864);

	public static readonly HRESULT QUERY_E_INVALID_OUTPUT_COLUMN = (HRESULT)(-2147215863);

	public static readonly HRESULT QUERY_E_INVALID_DIRECTORY = (HRESULT)(-2147215862);

	public static readonly HRESULT QUERY_E_DIR_ON_REMOVABLE_DRIVE = (HRESULT)(-2147215861);

	public static readonly HRESULT QUERY_S_NO_QUERY = (HRESULT)(-2147215860);

	public static readonly HRESULT QPLIST_E_CANT_OPEN_FILE = (HRESULT)(-2147215791);

	public static readonly HRESULT QPLIST_E_READ_ERROR = (HRESULT)(-2147215790);

	public static readonly HRESULT QPLIST_E_EXPECTING_NAME = (HRESULT)(-2147215789);

	public static readonly HRESULT QPLIST_E_EXPECTING_TYPE = (HRESULT)(-2147215788);

	public static readonly HRESULT QPLIST_E_UNRECOGNIZED_TYPE = (HRESULT)(-2147215787);

	public static readonly HRESULT QPLIST_E_EXPECTING_INTEGER = (HRESULT)(-2147215786);

	public static readonly HRESULT QPLIST_E_EXPECTING_CLOSE_PAREN = (HRESULT)(-2147215785);

	public static readonly HRESULT QPLIST_E_EXPECTING_GUID = (HRESULT)(-2147215784);

	public static readonly HRESULT QPLIST_E_BAD_GUID = (HRESULT)(-2147215783);

	public static readonly HRESULT QPLIST_E_EXPECTING_PROP_SPEC = (HRESULT)(-2147215782);

	public static readonly HRESULT QPLIST_E_CANT_SET_PROPERTY = (HRESULT)(-2147215781);

	public static readonly HRESULT QPLIST_E_DUPLICATE = (HRESULT)(-2147215780);

	public static readonly HRESULT QPLIST_E_VECTORBYREF_USED_ALONE = (HRESULT)(-2147215779);

	public static readonly HRESULT QPLIST_E_BYREF_USED_WITHOUT_PTRTYPE = (HRESULT)(-2147215778);

	public static readonly HRESULT QPARSE_E_UNEXPECTED_NOT = (HRESULT)(-2147215776);

	public static readonly HRESULT QPARSE_E_EXPECTING_INTEGER = (HRESULT)(-2147215775);

	public static readonly HRESULT QPARSE_E_EXPECTING_REAL = (HRESULT)(-2147215774);

	public static readonly HRESULT QPARSE_E_EXPECTING_DATE = (HRESULT)(-2147215773);

	public static readonly HRESULT QPARSE_E_EXPECTING_CURRENCY = (HRESULT)(-2147215772);

	public static readonly HRESULT QPARSE_E_EXPECTING_GUID = (HRESULT)(-2147215771);

	public static readonly HRESULT QPARSE_E_EXPECTING_BRACE = (HRESULT)(-2147215770);

	public static readonly HRESULT QPARSE_E_EXPECTING_PAREN = (HRESULT)(-2147215769);

	public static readonly HRESULT QPARSE_E_EXPECTING_PROPERTY = (HRESULT)(-2147215768);

	public static readonly HRESULT QPARSE_E_NOT_YET_IMPLEMENTED = (HRESULT)(-2147215767);

	public static readonly HRESULT QPARSE_E_EXPECTING_PHRASE = (HRESULT)(-2147215766);

	public static readonly HRESULT QPARSE_E_UNSUPPORTED_PROPERTY_TYPE = (HRESULT)(-2147215765);

	public static readonly HRESULT QPARSE_E_EXPECTING_REGEX = (HRESULT)(-2147215764);

	public static readonly HRESULT QPARSE_E_EXPECTING_REGEX_PROPERTY = (HRESULT)(-2147215763);

	public static readonly HRESULT QPARSE_E_INVALID_LITERAL = (HRESULT)(-2147215762);

	public static readonly HRESULT QPARSE_E_NO_SUCH_PROPERTY = (HRESULT)(-2147215761);

	public static readonly HRESULT QPARSE_E_EXPECTING_EOS = (HRESULT)(-2147215760);

	public static readonly HRESULT QPARSE_E_EXPECTING_COMMA = (HRESULT)(-2147215759);

	public static readonly HRESULT QPARSE_E_UNEXPECTED_EOS = (HRESULT)(-2147215758);

	public static readonly HRESULT QPARSE_E_WEIGHT_OUT_OF_RANGE = (HRESULT)(-2147215757);

	public static readonly HRESULT QPARSE_E_NO_SUCH_SORT_PROPERTY = (HRESULT)(-2147215756);

	public static readonly HRESULT QPARSE_E_INVALID_SORT_ORDER = (HRESULT)(-2147215755);

	public static readonly HRESULT QUTIL_E_CANT_CONVERT_VROOT = (HRESULT)(-2147215754);

	public static readonly HRESULT QPARSE_E_INVALID_GROUPING = (HRESULT)(-2147215753);

	public static readonly HRESULT QUTIL_E_INVALID_CODEPAGE = (HRESULT)(-1073473928);

	public static readonly HRESULT QPLIST_S_DUPLICATE = (HRESULT)(267897);

	public static readonly HRESULT QPARSE_E_INVALID_QUERY = (HRESULT)(-2147215750);

	public static readonly HRESULT QPARSE_E_INVALID_RANKMETHOD = (HRESULT)(-2147215749);

	#region Daemon Errors

	public static readonly HRESULT FDAEMON_W_WORDLISTFULL = (HRESULT)(267904);
	public static readonly HRESULT FDAEMON_E_LOWRESOURCE = (HRESULT)(-2147215743);
	public static readonly HRESULT FDAEMON_E_FATALERROR = (HRESULT)(-2147215742);
	public static readonly HRESULT FDAEMON_E_PARTITIONDELETED = (HRESULT)(-2147215741);
	public static readonly HRESULT FDAEMON_E_CHANGEUPDATEFAILED = (HRESULT)(-2147215740);
	public static readonly HRESULT FDAEMON_W_EMPTYWORDLIST = (HRESULT)(267909);
	public static readonly HRESULT FDAEMON_E_WORDLISTCOMMITFAILED = (HRESULT)(-2147215738);
	public static readonly HRESULT FDAEMON_E_NOWORDLIST = (HRESULT)(-2147215737);
	public static readonly HRESULT FDAEMON_E_TOOMANYFILTEREDBLOCKS = (HRESULT)(-2147215736);

	#endregion

	#region Search API Errors

	public static readonly HRESULT SEARCH_S_NOMOREHITS = (HRESULT)(267936);
	public static readonly HRESULT SEARCH_E_NOMONIKER = (HRESULT)(-2147215711);
	public static readonly HRESULT SEARCH_E_NOREGION = (HRESULT)(-2147215710);

	#endregion

	#region Content Filter Errors

	public static readonly HRESULT FILTER_E_TOO_BIG = (HRESULT)(-2147215568);
	public static readonly HRESULT FILTER_S_PARTIAL_CONTENTSCAN_IMMEDIATE = (HRESULT)(268081);
	public static readonly HRESULT FILTER_S_FULL_CONTENTSCAN_IMMEDIATE = (HRESULT)(268082);
	public static readonly HRESULT FILTER_S_CONTENTSCAN_DELAYED = (HRESULT)(268083);
	public static readonly HRESULT FILTER_E_CONTENTINDEXCORRUPT = (HRESULT)(-1073473740);
	public static readonly HRESULT FILTER_S_DISK_FULL = (HRESULT)(268085);
	public static readonly HRESULT FILTER_E_ALREADY_OPEN = (HRESULT)(-2147215562);
	public static readonly HRESULT FILTER_E_UNREACHABLE = (HRESULT)(-2147215561);
	public static readonly HRESULT FILTER_E_IN_USE = (HRESULT)(-2147215560);
	public static readonly HRESULT FILTER_E_NOT_OPEN = (HRESULT)(-2147215559);
	public static readonly HRESULT FILTER_S_NO_PROPSETS = (HRESULT)(268090);
	public static readonly HRESULT FILTER_E_NO_SUCH_PROPERTY = (HRESULT)(-2147215557);
	public static readonly HRESULT FILTER_S_NO_SECURITY_DESCRIPTOR = (HRESULT)(268092);
	public static readonly HRESULT FILTER_E_OFFLINE = (HRESULT)(-2147215555);
	public static readonly HRESULT FILTER_E_PARTIALLY_FILTERED = (HRESULT)(-2147215554);

	#endregion

	#region Language Service Errors

	public static readonly HRESULT LANGUAGE_S_LARGE_WORD = (HRESULT)(268161);
	public static readonly HRESULT LANGUAGE_E_DATABASE_NOT_FOUND = (HRESULT)(-2147215484);

	#endregion

	#region Word Breaker Errors

	public static readonly HRESULT WBREAK_E_END_OF_TEXT = (HRESULT)(-2147215488);
	public static readonly HRESULT WBREAK_E_QUERY_ONLY = (HRESULT)(-2147215486);
	public static readonly HRESULT WBREAK_E_BUFFER_TOO_SMALL = (HRESULT)(-2147215485);
	public static readonly HRESULT WBREAK_E_INIT_FAILED = (HRESULT)(-2147215483);

	#endregion

	#region Property Sink Errors

	public static readonly HRESULT PSINK_E_QUERY_ONLY = (HRESULT)(-2147215472);
	public static readonly HRESULT PSINK_E_INDEX_ONLY = (HRESULT)(-2147215471);
	public static readonly HRESULT PSINK_E_LARGE_ATTACHMENT = (HRESULT)(-2147215470);
	public static readonly HRESULT PSINK_S_LARGE_WORD = (HRESULT)(268179);

	#endregion

	#region Content Indexing Errors

	public static readonly HRESULT CI_CORRUPT_DATABASE = (HRESULT)(-1073473536);
	public static readonly HRESULT CI_CORRUPT_CATALOG = (HRESULT)(-1073473535);
	public static readonly HRESULT CI_INVALID_PARTITION = (HRESULT)(-1073473534);
	public static readonly HRESULT CI_INVALID_PRIORITY = (HRESULT)(-1073473533);
	public static readonly HRESULT CI_NO_STARTING_KEY = (HRESULT)(-1073473532);
	public static readonly HRESULT CI_OUT_OF_INDEX_IDS = (HRESULT)(-1073473531);
	public static readonly HRESULT CI_NO_CATALOG = (HRESULT)(-1073473530);
	public static readonly HRESULT CI_CORRUPT_FILTER_BUFFER = (HRESULT)(-1073473529);
	public static readonly HRESULT CI_INVALID_INDEX = (HRESULT)(-1073473528);
	public static readonly HRESULT CI_PROPSTORE_INCONSISTENCY = (HRESULT)(-1073473527);
	public static readonly HRESULT CI_E_ALREADY_INITIALIZED = (HRESULT)(-2147215350);
	public static readonly HRESULT CI_E_NOT_INITIALIZED = (HRESULT)(-2147215349);
	public static readonly HRESULT CI_E_BUFFERTOOSMALL = (HRESULT)(-2147215348);
	public static readonly HRESULT CI_E_PROPERTY_NOT_CACHED = (HRESULT)(-2147215347);
	public static readonly HRESULT CI_S_WORKID_DELETED = (HRESULT)(268302);
	public static readonly HRESULT CI_E_INVALID_STATE = (HRESULT)(-2147215345);
	public static readonly HRESULT CI_E_FILTERING_DISABLED = (HRESULT)(-2147215344);
	public static readonly HRESULT CI_E_DISK_FULL = (HRESULT)(-2147215343);
	public static readonly HRESULT CI_E_SHUTDOWN = (HRESULT)(-2147215342);
	public static readonly HRESULT CI_E_WORKID_NOTVALID = (HRESULT)(-2147215341);
	public static readonly HRESULT CI_S_END_OF_ENUMERATION = (HRESULT)(268308);
	public static readonly HRESULT CI_E_NOT_FOUND = (HRESULT)(-2147215339);
	public static readonly HRESULT CI_E_USE_DEFAULT_PID = (HRESULT)(-2147215338);
	public static readonly HRESULT CI_E_DUPLICATE_NOTIFICATION = (HRESULT)(-2147215337);
	public static readonly HRESULT CI_E_UPDATES_DISABLED = (HRESULT)(-2147215336);
	public static readonly HRESULT CI_E_INVALID_FLAGS_COMBINATION = (HRESULT)(-2147215335);
	public static readonly HRESULT CI_E_OUTOFSEQ_INCREMENT_DATA = (HRESULT)(-2147215334);
	public static readonly HRESULT CI_E_SHARING_VIOLATION = (HRESULT)(-2147215333);
	public static readonly HRESULT CI_E_LOGON_FAILURE = (HRESULT)(-2147215332);
	public static readonly HRESULT CI_E_NO_CATALOG = (HRESULT)(-2147215331);
	public static readonly HRESULT CI_E_STRANGE_PAGEORSECTOR_SIZE = (HRESULT)(-2147215330);
	public static readonly HRESULT CI_E_TIMEOUT = (HRESULT)(-2147215329);
	public static readonly HRESULT CI_E_NOT_RUNNING = (HRESULT)(-2147215328);
	public static readonly HRESULT CI_INCORRECT_VERSION = (HRESULT)(-1073473503);
	public static readonly HRESULT CI_E_ENUMERATION_STARTED = (HRESULT)(-1073473502);
	public static readonly HRESULT CI_E_PROPERTY_TOOLARGE = (HRESULT)(-1073473501);
	public static readonly HRESULT CI_E_CLIENT_FILTER_ABORT = (HRESULT)(-1073473500);
	public static readonly HRESULT CI_S_NO_DOCSTORE = (HRESULT)(268325);
	public static readonly HRESULT CI_S_CAT_STOPPED = (HRESULT)(268326);
	public static readonly HRESULT CI_E_CARDINALITY_MISMATCH = (HRESULT)(-2147215321);
	public static readonly HRESULT CI_E_CONFIG_DISK_FULL = (HRESULT)(-2147215320);

	public static readonly HRESULT CI_E_DISTRIBUTED_GROUPBY_UNSUPPORTED = (HRESULT)(-2147215319);

	#endregion

	#region StrSafe Errors

	public static readonly HRESULT STRSAFE_E_INSUFFICIENT_BUFFER = (HRESULT)(-2147024774);
	public static readonly HRESULT STRSAFE_E_INVALID_PARAMETER = (HRESULT)(-2147024809);
	public static readonly HRESULT STRSAFE_E_END_OF_FILE = (HRESULT)(-2147024858);

	#endregion

	#region DXGI Errors

	public static readonly HRESULT DXGI_ERROR_INVALID_CALL = (HRESULT)(-2005270527);
	public static readonly HRESULT DXGI_ERROR_NOT_FOUND = (HRESULT)(-2005270526);
	public static readonly HRESULT DXGI_ERROR_MORE_DATA = (HRESULT)(-2005270525);
	public static readonly HRESULT DXGI_ERROR_UNSUPPORTED = (HRESULT)(-2005270524);
	public static readonly HRESULT DXGI_ERROR_DEVICE_REMOVED = (HRESULT)(-2005270523);
	public static readonly HRESULT DXGI_ERROR_DEVICE_HUNG = (HRESULT)(-2005270522);
	public static readonly HRESULT DXGI_ERROR_DEVICE_RESET = (HRESULT)(-2005270521);
	public static readonly HRESULT DXGI_ERROR_WAS_STILL_DRAWING = (HRESULT)(-2005270518);
	public static readonly HRESULT DXGI_ERROR_FRAME_STATISTICS_DISJOINT = (HRESULT)(-2005270517);
	public static readonly HRESULT DXGI_ERROR_GRAPHICS_VIDPN_SOURCE_IN_USE = (HRESULT)(-2005270516);
	public static readonly HRESULT DXGI_ERROR_DRIVER_INTERNAL_ERROR = (HRESULT)(-2005270496);
	public static readonly HRESULT DXGI_ERROR_NONEXCLUSIVE = (HRESULT)(-2005270495);
	public static readonly HRESULT DXGI_ERROR_NOT_CURRENTLY_AVAILABLE = (HRESULT)(-2005270494);
	public static readonly HRESULT DXGI_ERROR_REMOTE_CLIENT_DISCONNECTED = (HRESULT)(-2005270493);
	public static readonly HRESULT DXGI_ERROR_REMOTE_OUTOFMEMORY = (HRESULT)(-2005270492);
	public static readonly HRESULT DXGI_ERROR_ACCESS_LOST = (HRESULT)(-2005270490);
	public static readonly HRESULT DXGI_ERROR_WAIT_TIMEOUT = (HRESULT)(-2005270489);
	public static readonly HRESULT DXGI_ERROR_SESSION_DISCONNECTED = (HRESULT)(-2005270488);
	public static readonly HRESULT DXGI_ERROR_RESTRICT_TO_OUTPUT_STALE = (HRESULT)(-2005270487);
	public static readonly HRESULT DXGI_ERROR_CANNOT_PROTECT_CONTENT = (HRESULT)(-2005270486);
	public static readonly HRESULT DXGI_ERROR_ACCESS_DENIED = (HRESULT)(-2005270485);
	public static readonly HRESULT DXGI_ERROR_NAME_ALREADY_EXISTS = (HRESULT)(-2005270484);
	public static readonly HRESULT DXGI_ERROR_SDK_COMPONENT_MISSING = (HRESULT)(-2005270483);
	public static readonly HRESULT DXGI_ERROR_NOT_CURRENT = (HRESULT)(-2005270482);
	public static readonly HRESULT DXGI_ERROR_HW_PROTECTION_OUTOFMEMORY = (HRESULT)(-2005270480);
	public static readonly HRESULT DXGI_ERROR_DYNAMIC_CODE_POLICY_VIOLATION = (HRESULT)(-2005270479);
	public static readonly HRESULT DXGI_ERROR_NON_COMPOSITED_UI = (HRESULT)(-2005270478);
	public static readonly HRESULT DXGI_ERROR_MODE_CHANGE_IN_PROGRESS = (HRESULT)(-2005270491);
	public static readonly HRESULT DXGI_ERROR_CACHE_CORRUPT = (HRESULT)(-2005270477);
	public static readonly HRESULT DXGI_ERROR_CACHE_FULL = (HRESULT)(-2005270476);
	public static readonly HRESULT DXGI_ERROR_CACHE_HASH_COLLISION = (HRESULT)(-2005270475);
	public static readonly HRESULT DXGI_ERROR_ALREADY_EXISTS = (HRESULT)(-2005270474);
	public static readonly HRESULT DXGI_ERROR_MPO_UNPINNED = (HRESULT)(-2005270428);

	#endregion

	#region GameInput Errors

	public static readonly HRESULT GAMEINPUT_E_DEVICE_DISCONNECTED = (HRESULT)0x838A0001;
	public static readonly HRESULT GAMEINPUT_E_DEVICE_NOT_FOUND = (HRESULT)0x838A0002;
	public static readonly HRESULT GAMEINPUT_E_READING_NOT_FOUND = (HRESULT)0x838A0003;
	public static readonly HRESULT GAMEINPUT_E_REFERENCE_READING_TOO_OLD = (HRESULT)0x838A0004;
	public static readonly HRESULT GAMEINPUT_E_FEEDBACK_NOT_SUPPORTED = (HRESULT)0x838A0007;
	public static readonly HRESULT GAMEINPUT_E_OBJECT_NO_LONGER_EXISTS = (HRESULT)0x838A0008;
	public static readonly HRESULT GAMEINPUT_E_CALLBACK_NOT_FOUND = (HRESULT)0x838A0009;
	public static readonly HRESULT GAMEINPUT_E_HAPTIC_INFO_NOT_FOUND = (HRESULT)0x838A000A;
	public static readonly HRESULT GAMEINPUT_E_AGGREGATE_OPERATION_NOT_SUPPORTED = (HRESULT)0x838A000B;
	public static readonly HRESULT GAMEINPUT_E_INPUT_KIND_NOT_PRESENT = (HRESULT)0x838A000C;

	#endregion
}