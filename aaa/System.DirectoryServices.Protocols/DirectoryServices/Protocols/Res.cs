using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000004 RID: 4
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (Res.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Res.s_InternalSyncObject, obj, null);
				}
				return Res.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal Res()
		{
			this.resources = new ResourceManager("System.DirectoryServices.Protocols", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (Res.InternalSyncObject)
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			string @string = res.resources.GetString(name, Res.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string DsmlNonHttpUri = "DsmlNonHttpUri";

		// Token: 0x04000003 RID: 3
		internal const string NoNegativeTime = "NoNegativeTime";

		// Token: 0x04000004 RID: 4
		internal const string NoNegativeSizeLimit = "NoNegativeSizeLimit";

		// Token: 0x04000005 RID: 5
		internal const string InvalidDocument = "InvalidDocument";

		// Token: 0x04000006 RID: 6
		internal const string MissingSessionId = "MissingSessionId";

		// Token: 0x04000007 RID: 7
		internal const string MissingResponse = "MissingResponse";

		// Token: 0x04000008 RID: 8
		internal const string ErrorResponse = "ErrorResponse";

		// Token: 0x04000009 RID: 9
		internal const string BadControl = "BadControl";

		// Token: 0x0400000A RID: 10
		internal const string NullDirectoryAttribute = "NullDirectoryAttribute";

		// Token: 0x0400000B RID: 11
		internal const string NullDirectoryAttributeCollection = "NullDirectoryAttributeCollection";

		// Token: 0x0400000C RID: 12
		internal const string WhiteSpaceServerName = "WhiteSpaceServerName";

		// Token: 0x0400000D RID: 13
		internal const string DirectoryAttributeConversion = "DirectoryAttributeConversion";

		// Token: 0x0400000E RID: 14
		internal const string WrongNumValuesCompare = "WrongNumValuesCompare";

		// Token: 0x0400000F RID: 15
		internal const string WrongAssertionCompare = "WrongAssertionCompare";

		// Token: 0x04000010 RID: 16
		internal const string DefaultOperationsError = "DefaultOperationsError";

		// Token: 0x04000011 RID: 17
		internal const string BadSearchLDAPFilter = "BadSearchLDAPFilter";

		// Token: 0x04000012 RID: 18
		internal const string ReadOnlyProperty = "ReadOnlyProperty";

		// Token: 0x04000013 RID: 19
		internal const string MissingOperationResponseResultCode = "MissingOperationResponseResultCode";

		// Token: 0x04000014 RID: 20
		internal const string MissingSearchResultEntryDN = "MissingSearchResultEntryDN";

		// Token: 0x04000015 RID: 21
		internal const string MissingSearchResultEntryAttributeName = "MissingSearchResultEntryAttributeName";

		// Token: 0x04000016 RID: 22
		internal const string BadOperationResponseResultCode = "BadOperationResponseResultCode";

		// Token: 0x04000017 RID: 23
		internal const string MissingErrorResponseType = "MissingErrorResponseType";

		// Token: 0x04000018 RID: 24
		internal const string ErrorResponseInvalidValue = "ErrorResponseInvalidValue";

		// Token: 0x04000019 RID: 25
		internal const string NotSupportOnDsmlErrRes = "NotSupportOnDsmlErrRes";

		// Token: 0x0400001A RID: 26
		internal const string BadBase64Value = "BadBase64Value";

		// Token: 0x0400001B RID: 27
		internal const string WrongAuthType = "WrongAuthType";

		// Token: 0x0400001C RID: 28
		internal const string SessionInUse = "SessionInUse";

		// Token: 0x0400001D RID: 29
		internal const string ReadOnlyDocument = "ReadOnlyDocument";

		// Token: 0x0400001E RID: 30
		internal const string NotWellFormedResponse = "NotWellFormedResponse";

		// Token: 0x0400001F RID: 31
		internal const string NoCurrentSession = "NoCurrentSession";

		// Token: 0x04000020 RID: 32
		internal const string UnknownResponseElement = "UnknownResponseElement";

		// Token: 0x04000021 RID: 33
		internal const string InvalidClientCertificates = "InvalidClientCertificates";

		// Token: 0x04000022 RID: 34
		internal const string InvalidAuthCredential = "InvalidAuthCredential";

		// Token: 0x04000023 RID: 35
		internal const string InvalidLdapSearchRequestFilter = "InvalidLdapSearchRequestFilter";

		// Token: 0x04000024 RID: 36
		internal const string PartialResultsNotSupported = "PartialResultsNotSupported";

		// Token: 0x04000025 RID: 37
		internal const string BerConverterNotMatch = "BerConverterNotMatch";

		// Token: 0x04000026 RID: 38
		internal const string BerConverterUndefineChar = "BerConverterUndefineChar";

		// Token: 0x04000027 RID: 39
		internal const string BerConversionError = "BerConversionError";

		// Token: 0x04000028 RID: 40
		internal const string TLSStopFailure = "TLSStopFailure";

		// Token: 0x04000029 RID: 41
		internal const string NoPartialResults = "NoPartialResults";

		// Token: 0x0400002A RID: 42
		internal const string DefaultLdapError = "DefaultLdapError";

		// Token: 0x0400002B RID: 43
		internal const string LDAP_PARTIAL_RESULTS = "LDAP_PARTIAL_RESULTS";

		// Token: 0x0400002C RID: 44
		internal const string LDAP_IS_LEAF = "LDAP_IS_LEAF";

		// Token: 0x0400002D RID: 45
		internal const string LDAP_SORT_CONTROL_MISSING = "LDAP_SORT_CONTROL_MISSING";

		// Token: 0x0400002E RID: 46
		internal const string LDAP_OFFSET_RANGE_ERROR = "LDAP_OFFSET_RANGE_ERROR";

		// Token: 0x0400002F RID: 47
		internal const string LDAP_RESULTS_TOO_LARGE = "LDAP_RESULTS_TOO_LARGE";

		// Token: 0x04000030 RID: 48
		internal const string LDAP_SERVER_DOWN = "LDAP_SERVER_DOWN";

		// Token: 0x04000031 RID: 49
		internal const string LDAP_LOCAL_ERROR = "LDAP_LOCAL_ERROR";

		// Token: 0x04000032 RID: 50
		internal const string LDAP_ENCODING_ERROR = "LDAP_ENCODING_ERROR";

		// Token: 0x04000033 RID: 51
		internal const string LDAP_DECODING_ERROR = "LDAP_DECODING_ERROR";

		// Token: 0x04000034 RID: 52
		internal const string LDAP_TIMEOUT = "LDAP_TIMEOUT";

		// Token: 0x04000035 RID: 53
		internal const string LDAP_AUTH_UNKNOWN = "LDAP_AUTH_UNKNOWN";

		// Token: 0x04000036 RID: 54
		internal const string LDAP_FILTER_ERROR = "LDAP_FILTER_ERROR";

		// Token: 0x04000037 RID: 55
		internal const string LDAP_USER_CANCELLED = "LDAP_USER_CANCELLED";

		// Token: 0x04000038 RID: 56
		internal const string LDAP_PARAM_ERROR = "LDAP_PARAM_ERROR";

		// Token: 0x04000039 RID: 57
		internal const string LDAP_NO_MEMORY = "LDAP_NO_MEMORY";

		// Token: 0x0400003A RID: 58
		internal const string LDAP_CONNECT_ERROR = "LDAP_CONNECT_ERROR";

		// Token: 0x0400003B RID: 59
		internal const string LDAP_NOT_SUPPORTED = "LDAP_NOT_SUPPORTED";

		// Token: 0x0400003C RID: 60
		internal const string LDAP_NO_RESULTS_RETURNED = "LDAP_NO_RESULTS_RETURNED";

		// Token: 0x0400003D RID: 61
		internal const string LDAP_CONTROL_NOT_FOUND = "LDAP_CONTROL_NOT_FOUND";

		// Token: 0x0400003E RID: 62
		internal const string LDAP_MORE_RESULTS_TO_RETURN = "LDAP_MORE_RESULTS_TO_RETURN";

		// Token: 0x0400003F RID: 63
		internal const string LDAP_CLIENT_LOOP = "LDAP_CLIENT_LOOP";

		// Token: 0x04000040 RID: 64
		internal const string LDAP_REFERRAL_LIMIT_EXCEEDED = "LDAP_REFERRAL_LIMIT_EXCEEDED";

		// Token: 0x04000041 RID: 65
		internal const string LDAP_INVALID_CREDENTIALS = "LDAP_INVALID_CREDENTIALS";

		// Token: 0x04000042 RID: 66
		internal const string LDAP_SUCCESS = "LDAP_SUCCESS";

		// Token: 0x04000043 RID: 67
		internal const string NoSessionIDReturned = "NoSessionIDReturned";

		// Token: 0x04000044 RID: 68
		internal const string LDAP_OPERATIONS_ERROR = "LDAP_OPERATIONS_ERROR";

		// Token: 0x04000045 RID: 69
		internal const string LDAP_PROTOCOL_ERROR = "LDAP_PROTOCOL_ERROR";

		// Token: 0x04000046 RID: 70
		internal const string LDAP_TIMELIMIT_EXCEEDED = "LDAP_TIMELIMIT_EXCEEDED";

		// Token: 0x04000047 RID: 71
		internal const string LDAP_SIZELIMIT_EXCEEDED = "LDAP_SIZELIMIT_EXCEEDED";

		// Token: 0x04000048 RID: 72
		internal const string LDAP_COMPARE_FALSE = "LDAP_COMPARE_FALSE";

		// Token: 0x04000049 RID: 73
		internal const string LDAP_COMPARE_TRUE = "LDAP_COMPARE_TRUE";

		// Token: 0x0400004A RID: 74
		internal const string LDAP_AUTH_METHOD_NOT_SUPPORTED = "LDAP_AUTH_METHOD_NOT_SUPPORTED";

		// Token: 0x0400004B RID: 75
		internal const string LDAP_STRONG_AUTH_REQUIRED = "LDAP_STRONG_AUTH_REQUIRED";

		// Token: 0x0400004C RID: 76
		internal const string LDAP_REFERRAL = "LDAP_REFERRAL";

		// Token: 0x0400004D RID: 77
		internal const string LDAP_ADMIN_LIMIT_EXCEEDED = "LDAP_ADMIN_LIMIT_EXCEEDED";

		// Token: 0x0400004E RID: 78
		internal const string LDAP_UNAVAILABLE_CRIT_EXTENSION = "LDAP_UNAVAILABLE_CRIT_EXTENSION";

		// Token: 0x0400004F RID: 79
		internal const string LDAP_CONFIDENTIALITY_REQUIRED = "LDAP_CONFIDENTIALITY_REQUIRED";

		// Token: 0x04000050 RID: 80
		internal const string LDAP_SASL_BIND_IN_PROGRESS = "LDAP_SASL_BIND_IN_PROGRESS";

		// Token: 0x04000051 RID: 81
		internal const string LDAP_NO_SUCH_ATTRIBUTE = "LDAP_NO_SUCH_ATTRIBUTE";

		// Token: 0x04000052 RID: 82
		internal const string LDAP_UNDEFINED_TYPE = "LDAP_UNDEFINED_TYPE";

		// Token: 0x04000053 RID: 83
		internal const string LDAP_INAPPROPRIATE_MATCHING = "LDAP_INAPPROPRIATE_MATCHING";

		// Token: 0x04000054 RID: 84
		internal const string LDAP_CONSTRAINT_VIOLATION = "LDAP_CONSTRAINT_VIOLATION";

		// Token: 0x04000055 RID: 85
		internal const string LDAP_ATTRIBUTE_OR_VALUE_EXISTS = "LDAP_ATTRIBUTE_OR_VALUE_EXISTS";

		// Token: 0x04000056 RID: 86
		internal const string LDAP_INVALID_SYNTAX = "LDAP_INVALID_SYNTAX";

		// Token: 0x04000057 RID: 87
		internal const string LDAP_NO_SUCH_OBJECT = "LDAP_NO_SUCH_OBJECT";

		// Token: 0x04000058 RID: 88
		internal const string LDAP_ALIAS_PROBLEM = "LDAP_ALIAS_PROBLEM";

		// Token: 0x04000059 RID: 89
		internal const string LDAP_INVALID_DN_SYNTAX = "LDAP_INVALID_DN_SYNTAX";

		// Token: 0x0400005A RID: 90
		internal const string LDAP_ALIAS_DEREF_PROBLEM = "LDAP_ALIAS_DEREF_PROBLEM";

		// Token: 0x0400005B RID: 91
		internal const string LDAP_INAPPROPRIATE_AUTH = "LDAP_INAPPROPRIATE_AUTH";

		// Token: 0x0400005C RID: 92
		internal const string LDAP_INSUFFICIENT_RIGHTS = "LDAP_INSUFFICIENT_RIGHTS";

		// Token: 0x0400005D RID: 93
		internal const string LDAP_BUSY = "LDAP_BUSY";

		// Token: 0x0400005E RID: 94
		internal const string LDAP_UNAVAILABLE = "LDAP_UNAVAILABLE";

		// Token: 0x0400005F RID: 95
		internal const string LDAP_UNWILLING_TO_PERFORM = "LDAP_UNWILLING_TO_PERFORM";

		// Token: 0x04000060 RID: 96
		internal const string LDAP_LOOP_DETECT = "LDAP_LOOP_DETECT";

		// Token: 0x04000061 RID: 97
		internal const string LDAP_NAMING_VIOLATION = "LDAP_NAMING_VIOLATION";

		// Token: 0x04000062 RID: 98
		internal const string LDAP_OBJECT_CLASS_VIOLATION = "LDAP_OBJECT_CLASS_VIOLATION";

		// Token: 0x04000063 RID: 99
		internal const string LDAP_NOT_ALLOWED_ON_NONLEAF = "LDAP_NOT_ALLOWED_ON_NONLEAF";

		// Token: 0x04000064 RID: 100
		internal const string LDAP_NOT_ALLOWED_ON_RDN = "LDAP_NOT_ALLOWED_ON_RDN";

		// Token: 0x04000065 RID: 101
		internal const string LDAP_ALREADY_EXISTS = "LDAP_ALREADY_EXISTS";

		// Token: 0x04000066 RID: 102
		internal const string LDAP_NO_OBJECT_CLASS_MODS = "LDAP_NO_OBJECT_CLASS_MODS";

		// Token: 0x04000067 RID: 103
		internal const string LDAP_AFFECTS_MULTIPLE_DSAS = "LDAP_AFFECTS_MULTIPLE_DSAS";

		// Token: 0x04000068 RID: 104
		internal const string LDAP_VIRTUAL_LIST_VIEW_ERROR = "LDAP_VIRTUAL_LIST_VIEW_ERROR";

		// Token: 0x04000069 RID: 105
		internal const string LDAP_OTHER = "LDAP_OTHER";

		// Token: 0x0400006A RID: 106
		internal const string LDAP_SEND_TIMEOUT = "LDAP_SEND_TIMEOUT";

		// Token: 0x0400006B RID: 107
		internal const string InvalidAsyncResult = "InvalidAsyncResult";

		// Token: 0x0400006C RID: 108
		internal const string ValidDirectoryAttributeType = "ValidDirectoryAttributeType";

		// Token: 0x0400006D RID: 109
		internal const string ValidFilterType = "ValidFilterType";

		// Token: 0x0400006E RID: 110
		internal const string ValidValuesType = "ValidValuesType";

		// Token: 0x0400006F RID: 111
		internal const string ValidValueType = "ValidValueType";

		// Token: 0x04000070 RID: 112
		internal const string SupportedPlatforms = "SupportedPlatforms";

		// Token: 0x04000071 RID: 113
		internal const string TLSNotSupported = "TLSNotSupported";

		// Token: 0x04000072 RID: 114
		internal const string InvalidValueType = "InvalidValueType";

		// Token: 0x04000073 RID: 115
		internal const string ValidValue = "ValidValue";

		// Token: 0x04000074 RID: 116
		internal const string ContainNullControl = "ContainNullControl";

		// Token: 0x04000075 RID: 117
		internal const string InvalidFilterType = "InvalidFilterType";

		// Token: 0x04000076 RID: 118
		internal const string NotReturnedAsyncResult = "NotReturnedAsyncResult";

		// Token: 0x04000077 RID: 119
		internal const string DsmlAuthRequestNotSupported = "DsmlAuthRequestNotSupported";

		// Token: 0x04000078 RID: 120
		internal const string CallBackIsNull = "CallBackIsNull";

		// Token: 0x04000079 RID: 121
		internal const string NullValueArray = "NullValueArray";

		// Token: 0x0400007A RID: 122
		internal const string NonCLSException = "NonCLSException";

		// Token: 0x0400007B RID: 123
		internal const string ConcurrentBindNotSupport = "ConcurrentBindNotSupport";

		// Token: 0x0400007C RID: 124
		internal const string TimespanExceedMax = "TimespanExceedMax";

		// Token: 0x0400007D RID: 125
		internal const string InvliadRequestType = "InvliadRequestType";

		// Token: 0x0400007E RID: 126
		private static Res loader;

		// Token: 0x0400007F RID: 127
		private ResourceManager resources;

		// Token: 0x04000080 RID: 128
		private static object s_InternalSyncObject;
	}
}
