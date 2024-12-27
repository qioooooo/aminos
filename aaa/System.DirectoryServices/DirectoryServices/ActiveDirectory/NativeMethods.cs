using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000C1 RID: 193
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal sealed class NativeMethods
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00022644 File Offset: 0x00021644
		private NativeMethods()
		{
		}

		// Token: 0x060005F5 RID: 1525
		[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcNameW")]
		internal static extern int DsGetDcName([In] string computerName, [In] string domainName, [In] IntPtr domainGuid, [In] string siteName, [In] int flags, out IntPtr domainControllerInfo);

		// Token: 0x060005F6 RID: 1526
		[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcOpenW")]
		internal static extern int DsGetDcOpen([In] string dnsName, [In] int optionFlags, [In] string siteName, [In] IntPtr domainGuid, [In] string dnsForestName, [In] int dcFlags, out IntPtr retGetDcContext);

		// Token: 0x060005F7 RID: 1527
		[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcNextW")]
		internal static extern int DsGetDcNext([In] IntPtr getDcContextHandle, [In] [Out] ref IntPtr sockAddressCount, out IntPtr sockAdresses, out IntPtr dnsHostName);

		// Token: 0x060005F8 RID: 1528
		[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcCloseW")]
		internal static extern void DsGetDcClose([In] IntPtr getDcContextHandle);

		// Token: 0x060005F9 RID: 1529
		[DllImport("Netapi32.dll")]
		internal static extern int NetApiBufferFree([In] IntPtr buffer);

		// Token: 0x060005FA RID: 1530
		[DllImport("Kernel32.dll")]
		internal static extern int GetLastError();

		// Token: 0x060005FB RID: 1531
		[DllImport("Dnsapi.dll", CharSet = CharSet.Unicode, EntryPoint = "DnsQuery_W")]
		internal static extern int DnsQuery([In] string recordName, [In] short recordType, [In] int options, [In] IntPtr servers, out IntPtr dnsResultList, [Out] IntPtr reserved);

		// Token: 0x060005FC RID: 1532
		[DllImport("Dnsapi.dll", CharSet = CharSet.Unicode)]
		internal static extern void DnsRecordListFree([In] IntPtr dnsResultList, [In] bool dnsFreeType);

		// Token: 0x060005FD RID: 1533
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetVersionExW", SetLastError = true)]
		internal static extern bool GetVersionEx([In] [Out] OSVersionInfoEx ver);

		// Token: 0x060005FE RID: 1534
		[DllImport("Secur32.dll")]
		internal static extern int LsaConnectUntrusted(out LsaLogonProcessSafeHandle lsaHandle);

		// Token: 0x060005FF RID: 1535
		[DllImport("Secur32.dll")]
		internal static extern int LsaCallAuthenticationPackage([In] LsaLogonProcessSafeHandle lsaHandle, [In] int authenticationPackage, [In] NegotiateCallerNameRequest protocolSubmitBuffer, [In] int submitBufferLength, out IntPtr protocolReturnBuffer, out int returnBufferLength, out int protocolStatus);

		// Token: 0x06000600 RID: 1536
		[DllImport("Secur32.dll")]
		internal static extern uint LsaFreeReturnBuffer([In] IntPtr buffer);

		// Token: 0x06000601 RID: 1537
		[DllImport("Secur32.dll")]
		internal static extern int LsaDeregisterLogonProcess([In] IntPtr lsaHandle);

		// Token: 0x06000602 RID: 1538
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "CompareStringW", SetLastError = true)]
		internal static extern int CompareString([In] uint locale, [In] uint dwCmpFlags, [In] IntPtr lpString1, [In] int cchCount1, [In] IntPtr lpString2, [In] int cchCount2);

		// Token: 0x040004E5 RID: 1253
		internal const int VER_PLATFORM_WIN32_NT = 2;

		// Token: 0x040004E6 RID: 1254
		internal const int ERROR_INVALID_DOMAIN_NAME_FORMAT = 1212;

		// Token: 0x040004E7 RID: 1255
		internal const int ERROR_NO_SUCH_DOMAIN = 1355;

		// Token: 0x040004E8 RID: 1256
		internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x040004E9 RID: 1257
		internal const int ERROR_INVALID_FLAGS = 1004;

		// Token: 0x040004EA RID: 1258
		internal const int DS_NAME_NO_ERROR = 0;

		// Token: 0x040004EB RID: 1259
		internal const int ERROR_NO_MORE_ITEMS = 259;

		// Token: 0x040004EC RID: 1260
		internal const int ERROR_FILE_MARK_DETECTED = 1101;

		// Token: 0x040004ED RID: 1261
		internal const int DNS_ERROR_RCODE_NAME_ERROR = 9003;

		// Token: 0x040004EE RID: 1262
		internal const int ERROR_NO_SUCH_LOGON_SESSION = 1312;

		// Token: 0x040004EF RID: 1263
		internal const int DS_NAME_FLAG_SYNTACTICAL_ONLY = 1;

		// Token: 0x040004F0 RID: 1264
		internal const int DS_FQDN_1779_NAME = 1;

		// Token: 0x040004F1 RID: 1265
		internal const int DS_CANONICAL_NAME = 7;

		// Token: 0x040004F2 RID: 1266
		internal const int DS_NAME_ERROR_NO_SYNTACTICAL_MAPPING = 6;

		// Token: 0x040004F3 RID: 1267
		internal const int STATUS_QUOTA_EXCEEDED = -1073741756;

		// Token: 0x040004F4 RID: 1268
		internal const int DsDomainControllerInfoLevel2 = 2;

		// Token: 0x040004F5 RID: 1269
		internal const int DsDomainControllerInfoLevel3 = 3;

		// Token: 0x040004F6 RID: 1270
		internal const int DsNameNoError = 0;

		// Token: 0x040004F7 RID: 1271
		internal const int DnsSrvData = 33;

		// Token: 0x040004F8 RID: 1272
		internal const int DnsQueryBypassCache = 8;

		// Token: 0x040004F9 RID: 1273
		internal const int NegGetCallerName = 1;

		// Token: 0x020000C2 RID: 194
		// (Invoke) Token: 0x06000604 RID: 1540
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsMakePasswordCredentials([MarshalAs(UnmanagedType.LPWStr)] string user, [MarshalAs(UnmanagedType.LPWStr)] string domain, [MarshalAs(UnmanagedType.LPWStr)] string password, out IntPtr authIdentity);

		// Token: 0x020000C3 RID: 195
		// (Invoke) Token: 0x06000608 RID: 1544
		[SuppressUnmanagedCodeSecurity]
		internal delegate void DsFreePasswordCredentials([In] IntPtr authIdentity);

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x0600060C RID: 1548
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsBindWithCred([MarshalAs(UnmanagedType.LPWStr)] string domainController, [MarshalAs(UnmanagedType.LPWStr)] string dnsDomainName, [In] IntPtr authIdentity, out IntPtr handle);

		// Token: 0x020000C5 RID: 197
		// (Invoke) Token: 0x06000610 RID: 1552
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsUnBind([In] ref IntPtr handle);

		// Token: 0x020000C6 RID: 198
		// (Invoke) Token: 0x06000614 RID: 1556
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsGetDomainControllerInfo([In] IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string domainName, [In] int infoLevel, out int dcCount, out IntPtr dcInfo);

		// Token: 0x020000C7 RID: 199
		// (Invoke) Token: 0x06000618 RID: 1560
		[SuppressUnmanagedCodeSecurity]
		internal delegate void DsFreeDomainControllerInfo([In] int infoLevel, [In] int dcInfoListCount, [In] IntPtr dcInfoList);

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x0600061C RID: 1564
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsListSites([In] IntPtr dsHandle, out IntPtr sites);

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x06000620 RID: 1568
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsListRoles([In] IntPtr dsHandle, out IntPtr roles);

		// Token: 0x020000CA RID: 202
		// (Invoke) Token: 0x06000624 RID: 1572
		[SuppressUnmanagedCodeSecurity]
		internal delegate int DsCrackNames([In] IntPtr hDS, [In] int flags, [In] int formatOffered, [In] int formatDesired, [In] int nameCount, [In] IntPtr names, out IntPtr results);
	}
}
