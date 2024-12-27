using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000129 RID: 297
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal class UnsafeNativeMethods
	{
		// Token: 0x06000783 RID: 1923
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern int FormatMessageW(int dwFlags, int lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, int arguments);

		// Token: 0x06000784 RID: 1924
		[DllImport("kernel32.dll")]
		public static extern int LocalFree(IntPtr mem);

		// Token: 0x06000785 RID: 1925
		[DllImport("activeds.dll", CharSet = CharSet.Unicode)]
		public static extern int ADsEncodeBinaryData(byte[] data, int length, ref IntPtr result);

		// Token: 0x06000786 RID: 1926
		[DllImport("activeds.dll")]
		public static extern bool FreeADsMem(IntPtr pVoid);

		// Token: 0x06000787 RID: 1927
		[DllImport("netapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "DsGetSiteNameW")]
		public static extern int DsGetSiteName(string dcName, ref IntPtr ptr);

		// Token: 0x06000788 RID: 1928
		[DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int DsEnumerateDomainTrustsW(string serverName, int flags, out IntPtr domains, out int count);

		// Token: 0x06000789 RID: 1929
		[DllImport("Netapi32.dll")]
		public static extern int NetApiBufferFree(IntPtr buffer);

		// Token: 0x0600078A RID: 1930
		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int LogonUserW(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		// Token: 0x0600078B RID: 1931
		[DllImport("Advapi32.dll", SetLastError = true)]
		public static extern int ImpersonateLoggedOnUser(IntPtr hToken);

		// Token: 0x0600078C RID: 1932
		[DllImport("Advapi32.dll", SetLastError = true)]
		public static extern int RevertToSelf();

		// Token: 0x0600078D RID: 1933
		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int ConvertSidToStringSidW(IntPtr pSid, ref IntPtr stringSid);

		// Token: 0x0600078E RID: 1934
		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int ConvertStringSidToSidW(IntPtr stringSid, ref IntPtr pSid);

		// Token: 0x0600078F RID: 1935
		[DllImport("Advapi32.dll")]
		public static extern int LsaSetForestTrustInformation(PolicySafeHandle handle, LSA_UNICODE_STRING target, IntPtr forestTrustInfo, int checkOnly, out IntPtr collisionInfo);

		// Token: 0x06000790 RID: 1936
		[DllImport("Advapi32.dll")]
		public static extern int LsaOpenPolicy(LSA_UNICODE_STRING target, LSA_OBJECT_ATTRIBUTES objectAttributes, int access, out IntPtr handle);

		// Token: 0x06000791 RID: 1937
		[DllImport("Advapi32.dll")]
		public static extern int LsaClose(IntPtr handle);

		// Token: 0x06000792 RID: 1938
		[DllImport("Advapi32.dll")]
		public static extern int LsaQueryForestTrustInformation(PolicySafeHandle handle, LSA_UNICODE_STRING target, ref IntPtr ForestTrustInfo);

		// Token: 0x06000793 RID: 1939
		[DllImport("Advapi32.dll")]
		public static extern int LsaQueryTrustedDomainInfoByName(PolicySafeHandle handle, LSA_UNICODE_STRING trustedDomain, TRUSTED_INFORMATION_CLASS infoClass, ref IntPtr buffer);

		// Token: 0x06000794 RID: 1940
		[DllImport("Advapi32.dll")]
		public static extern int LsaNtStatusToWinError(int status);

		// Token: 0x06000795 RID: 1941
		[DllImport("Advapi32.dll")]
		public static extern int LsaFreeMemory(IntPtr ptr);

		// Token: 0x06000796 RID: 1942
		[DllImport("Advapi32.dll")]
		public static extern int LsaSetTrustedDomainInfoByName(PolicySafeHandle handle, LSA_UNICODE_STRING trustedDomain, TRUSTED_INFORMATION_CLASS infoClass, IntPtr buffer);

		// Token: 0x06000797 RID: 1943
		[DllImport("Advapi32.dll")]
		public static extern int LsaOpenTrustedDomainByName(PolicySafeHandle policyHandle, LSA_UNICODE_STRING trustedDomain, int access, ref IntPtr trustedDomainHandle);

		// Token: 0x06000798 RID: 1944
		[DllImport("Advapi32.dll")]
		public static extern int LsaDeleteTrustedDomain(PolicySafeHandle handle, IntPtr pSid);

		// Token: 0x06000799 RID: 1945
		[DllImport("netapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int I_NetLogonControl2(string serverName, int FunctionCode, int QueryLevel, IntPtr data, out IntPtr buffer);

		// Token: 0x0600079A RID: 1946
		[DllImport("Kernel32.dll")]
		public static extern void GetSystemTimeAsFileTime(IntPtr fileTime);

		// Token: 0x0600079B RID: 1947
		[DllImport("Advapi32.dll")]
		public static extern int LsaQueryInformationPolicy(PolicySafeHandle handle, int infoClass, out IntPtr buffer);

		// Token: 0x0600079C RID: 1948
		[DllImport("Advapi32.dll")]
		public static extern int LsaCreateTrustedDomainEx(PolicySafeHandle handle, TRUSTED_DOMAIN_INFORMATION_EX domainEx, TRUSTED_DOMAIN_AUTH_INFORMATION authInfo, int classInfo, out IntPtr domainHandle);

		// Token: 0x0600079D RID: 1949
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenThread(uint desiredAccess, bool inheirted, int threadID);

		// Token: 0x0600079E RID: 1950
		[DllImport("Kernel32.dll")]
		public static extern int GetCurrentThreadId();

		// Token: 0x0600079F RID: 1951
		[DllImport("Advapi32.dll", SetLastError = true)]
		public static extern int ImpersonateAnonymousToken(IntPtr token);

		// Token: 0x060007A0 RID: 1952
		[DllImport("Kernel32.dll")]
		public static extern int CloseHandle(IntPtr handle);

		// Token: 0x060007A1 RID: 1953
		[DllImport("ntdll.dll")]
		public static extern int RtlInitUnicodeString(LSA_UNICODE_STRING result, IntPtr s);

		// Token: 0x060007A2 RID: 1954
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadLibraryW", SetLastError = true)]
		public static extern IntPtr LoadLibrary(string name);

		// Token: 0x060007A3 RID: 1955
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern uint FreeLibrary(IntPtr libName);

		// Token: 0x060007A4 RID: 1956
		[DllImport("kernel32.dll", BestFitMapping = false, SetLastError = true)]
		public static extern IntPtr GetProcAddress(LoadLibrarySafeHandle hModule, string entryPoint);

		// Token: 0x040006E3 RID: 1763
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x040006E4 RID: 1764
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040006E5 RID: 1765
		public const int FORMAT_MESSAGE_FROM_STRING = 1024;

		// Token: 0x040006E6 RID: 1766
		public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;

		// Token: 0x040006E7 RID: 1767
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040006E8 RID: 1768
		public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x040006E9 RID: 1769
		public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;

		// Token: 0x0200012A RID: 298
		// (Invoke) Token: 0x060007A7 RID: 1959
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaConsistencyCheck([In] IntPtr handle, int taskID, int flags);

		// Token: 0x0200012B RID: 299
		// (Invoke) Token: 0x060007AB RID: 1963
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaGetInfo2W(IntPtr handle, int type, [MarshalAs(UnmanagedType.LPWStr)] string objectPath, IntPtr sourceGUID, string attributeName, string value, int flag, int context, ref IntPtr info);

		// Token: 0x0200012C RID: 300
		// (Invoke) Token: 0x060007AF RID: 1967
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaGetInfoW(IntPtr handle, int type, [MarshalAs(UnmanagedType.LPWStr)] string objectPath, IntPtr sourceGUID, ref IntPtr info);

		// Token: 0x0200012D RID: 301
		// (Invoke) Token: 0x060007B3 RID: 1971
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaFreeInfo(int type, IntPtr value);

		// Token: 0x0200012E RID: 302
		// (Invoke) Token: 0x060007B7 RID: 1975
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaSyncW(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string partition, IntPtr uuid, int option);

		// Token: 0x0200012F RID: 303
		// (Invoke) Token: 0x060007BB RID: 1979
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsReplicaSyncAllW(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string partition, int flags, SyncReplicaFromAllServersCallback callback, IntPtr data, ref IntPtr error);

		// Token: 0x02000130 RID: 304
		// (Invoke) Token: 0x060007BF RID: 1983
		[SuppressUnmanagedCodeSecurity]
		public delegate int DsListDomainsInSiteW(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string site, ref IntPtr info);

		// Token: 0x02000131 RID: 305
		// (Invoke) Token: 0x060007C3 RID: 1987
		[SuppressUnmanagedCodeSecurity]
		public delegate void DsFreeNameResultW(IntPtr result);
	}
}
