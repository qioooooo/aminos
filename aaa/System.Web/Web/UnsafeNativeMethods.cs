using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000DA RID: 218
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal sealed class UnsafeNativeMethods
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0002B64E File Offset: 0x0002A64E
		private UnsafeNativeMethods()
		{
		}

		// Token: 0x060009E8 RID: 2536
		[DllImport("advapi32.dll")]
		internal static extern int SetThreadToken(IntPtr threadref, IntPtr token);

		// Token: 0x060009E9 RID: 2537
		[DllImport("advapi32.dll")]
		internal static extern int RevertToSelf();

		// Token: 0x060009EA RID: 2538
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern int OpenThreadToken(IntPtr thread, int access, bool openAsSelf, ref IntPtr hToken);

		// Token: 0x060009EB RID: 2539
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetFileSecurity(string filename, int requestedInformation, byte[] securityDescriptor, int length, ref int lengthNeeded);

		// Token: 0x060009EC RID: 2540
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int LogonUser(string username, string domain, string password, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		// Token: 0x060009ED RID: 2541
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int ConvertStringSidToSid(string stringSid, out IntPtr pSid);

		// Token: 0x060009EE RID: 2542
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int LookupAccountSid(string systemName, IntPtr pSid, StringBuilder szName, ref int nameSize, StringBuilder szDomain, ref int domainSize, ref int eUse);

		// Token: 0x060009EF RID: 2543
		[DllImport("aspnet_state.exe")]
		internal static extern void STWNDCloseConnection(IntPtr tracker);

		// Token: 0x060009F0 RID: 2544
		[DllImport("aspnet_state.exe")]
		internal static extern void STWNDDeleteStateItem(IntPtr stateItem);

		// Token: 0x060009F1 RID: 2545
		[DllImport("aspnet_state.exe")]
		internal static extern void STWNDEndOfRequest(IntPtr tracker);

		// Token: 0x060009F2 RID: 2546
		[DllImport("aspnet_state.exe", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern void STWNDGetLocalAddress(IntPtr tracker, StringBuilder buf);

		// Token: 0x060009F3 RID: 2547
		[DllImport("aspnet_state.exe")]
		internal static extern int STWNDGetLocalPort(IntPtr tracker);

		// Token: 0x060009F4 RID: 2548
		[DllImport("aspnet_state.exe", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern void STWNDGetRemoteAddress(IntPtr tracker, StringBuilder buf);

		// Token: 0x060009F5 RID: 2549
		[DllImport("aspnet_state.exe")]
		internal static extern int STWNDGetRemotePort(IntPtr tracker);

		// Token: 0x060009F6 RID: 2550
		[DllImport("aspnet_state.exe")]
		internal static extern bool STWNDIsClientConnected(IntPtr tracker);

		// Token: 0x060009F7 RID: 2551
		[DllImport("aspnet_state.exe", CharSet = CharSet.Unicode)]
		internal static extern void STWNDSendResponse(IntPtr tracker, StringBuilder status, int statusLength, StringBuilder headers, int headersLength, IntPtr unmanagedState);

		// Token: 0x060009F8 RID: 2552
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int lstrlenW(IntPtr ptr);

		// Token: 0x060009F9 RID: 2553
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern int lstrlenA(IntPtr ptr);

		// Token: 0x060009FA RID: 2554
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool MoveFileEx(string oldFilename, string newFilename, uint flags);

		// Token: 0x060009FB RID: 2555
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060009FC RID: 2556
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool FindClose(IntPtr hndFindFile);

		// Token: 0x060009FD RID: 2557
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr FindFirstFile(string pFileName, out UnsafeNativeMethods.WIN32_FIND_DATA pFindFileData);

		// Token: 0x060009FE RID: 2558
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool FindNextFile(IntPtr hndFindFile, out UnsafeNativeMethods.WIN32_FIND_DATA pFindFileData);

		// Token: 0x060009FF RID: 2559
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool GetFileAttributesEx(string name, int fileInfoLevel, out UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA data);

		// Token: 0x06000A00 RID: 2560
		[DllImport("kernel32.dll")]
		internal static extern int GetProcessAffinityMask(IntPtr handle, out IntPtr processAffinityMask, out IntPtr systemAffinityMask);

		// Token: 0x06000A01 RID: 2561
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetComputerName(StringBuilder nameBuffer, ref int bufferSize);

		// Token: 0x06000A02 RID: 2562
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetModuleFileName(IntPtr module, StringBuilder filename, int size);

		// Token: 0x06000A03 RID: 2563
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr GetModuleHandle(string moduleName);

		// Token: 0x06000A04 RID: 2564
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern void GetSystemInfo(out UnsafeNativeMethods.SYSTEM_INFO si);

		// Token: 0x06000A05 RID: 2565
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string libFilename);

		// Token: 0x06000A06 RID: 2566
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x06000A07 RID: 2567
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

		// Token: 0x06000A08 RID: 2568
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

		// Token: 0x06000A09 RID: 2569
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

		// Token: 0x06000A0A RID: 2570
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LockResource(IntPtr hResData);

		// Token: 0x06000A0B RID: 2571
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr LocalFree(IntPtr pMem);

		// Token: 0x06000A0C RID: 2572
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GlobalMemoryStatusEx(ref UnsafeNativeMethods.MEMORYSTATUSEX memoryStatusEx);

		// Token: 0x06000A0D RID: 2573
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetCurrentThread();

		// Token: 0x06000A0E RID: 2574
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern void AppDomainRestart(string appId);

		// Token: 0x06000A0F RID: 2575
		[DllImport("webengine.dll")]
		internal static extern int AspCompatProcessRequest(AspCompatCallback callback, [MarshalAs(UnmanagedType.Interface)] object context, bool sharedActivity, int activityHash);

		// Token: 0x06000A10 RID: 2576
		[DllImport("webengine.dll")]
		internal static extern int AspCompatOnPageStart([MarshalAs(UnmanagedType.Interface)] object obj);

		// Token: 0x06000A11 RID: 2577
		[DllImport("webengine.dll")]
		internal static extern int AspCompatOnPageEnd();

		// Token: 0x06000A12 RID: 2578
		[DllImport("webengine.dll")]
		internal static extern int AspCompatIsApartmentComponent([MarshalAs(UnmanagedType.Interface)] object obj);

		// Token: 0x06000A13 RID: 2579
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int AttachDebugger(string clsId, string sessId, IntPtr userToken);

		// Token: 0x06000A14 RID: 2580
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int ChangeAccessToKeyContainer(string containerName, string accountName, string csp, int options);

		// Token: 0x06000A15 RID: 2581
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int CookieAuthParseTicket(byte[] pData, int iDataLen, StringBuilder szName, int iNameLen, StringBuilder szData, int iUserDataLen, StringBuilder szPath, int iPathLen, byte[] pBytes, long[] pDates);

		// Token: 0x06000A16 RID: 2582
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int CookieAuthConstructTicket(byte[] pData, int iDataLen, string szName, string szData, string szPath, byte[] pBytes, long[] pDates);

		// Token: 0x06000A17 RID: 2583
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr CreateUserToken(string name, string password, int fImpersonationToken, StringBuilder strError, int iErrorSize);

		// Token: 0x06000A18 RID: 2584
		[DllImport("webengine.dll")]
		internal static extern void GetDirMonConfiguration(out int FCNMode);

		// Token: 0x06000A19 RID: 2585
		[DllImport("webengine.dll")]
		internal static extern void DirMonClose(HandleRef dirMon);

		// Token: 0x06000A1A RID: 2586
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int DirMonOpen(string dir, string appId, bool watchSubtree, uint notifyFilter, NativeFileChangeNotification callback, out IntPtr pCompletion);

		// Token: 0x06000A1B RID: 2587
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int GrowFileNotificationBuffer(string appId, bool fWatchSubtree);

		// Token: 0x06000A1C RID: 2588
		[DllImport("webengine.dll")]
		internal static extern void EcbFreeExecUrlEntityInfo(IntPtr pEntity);

		// Token: 0x06000A1D RID: 2589
		[DllImport("webengine.dll")]
		internal static extern int EcbGetBasics(IntPtr pECB, byte[] buffer, int size, int[] contentInfo);

		// Token: 0x06000A1E RID: 2590
		[DllImport("webengine.dll")]
		internal static extern int EcbGetBasicsContentInfo(IntPtr pECB, int[] contentInfo);

		// Token: 0x06000A1F RID: 2591
		[DllImport("webengine.dll")]
		internal static extern int EcbGetTraceFlags(IntPtr pECB, int[] contentInfo);

		// Token: 0x06000A20 RID: 2592
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int EcbEmitSimpleTrace(IntPtr pECB, int type, string eventData);

		// Token: 0x06000A21 RID: 2593
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int EcbEmitWebEventTrace(IntPtr pECB, int webEventType, int fieldCount, string[] fieldNames, int[] fieldTypes, string[] fieldData);

		// Token: 0x06000A22 RID: 2594
		[DllImport("webengine.dll")]
		internal static extern int EcbGetClientCertificate(IntPtr pECB, byte[] buffer, int size, int[] pInts, long[] pDates);

		// Token: 0x06000A23 RID: 2595
		[DllImport("webengine.dll")]
		internal static extern int EcbGetExecUrlEntityInfo(int entityLength, byte[] entity, out IntPtr ppEntity);

		// Token: 0x06000A24 RID: 2596
		[DllImport("webengine.dll")]
		internal static extern int EcbGetTraceContextId(IntPtr pECB, out Guid traceContextId);

		// Token: 0x06000A25 RID: 2597
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int EcbGetServerVariable(IntPtr pECB, string name, byte[] buffer, int size);

		// Token: 0x06000A26 RID: 2598
		[DllImport("webengine.dll")]
		internal static extern int EcbGetServerVariableByIndex(IntPtr pECB, int nameIndex, byte[] buffer, int size);

		// Token: 0x06000A27 RID: 2599
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int EcbGetQueryString(IntPtr pECB, int encode, StringBuilder buffer, int size);

		// Token: 0x06000A28 RID: 2600
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int EcbGetUnicodeServerVariable(IntPtr pECB, string name, IntPtr buffer, int size);

		// Token: 0x06000A29 RID: 2601
		[DllImport("webengine.dll")]
		internal static extern int EcbGetUnicodeServerVariableByIndex(IntPtr pECB, int nameIndex, IntPtr buffer, int size);

		// Token: 0x06000A2A RID: 2602
		[DllImport("webengine.dll")]
		internal static extern int EcbGetUnicodeServerVariables(IntPtr pECB, IntPtr buffer, int bufferSizeInChars, int[] serverVarLengths, int serverVarCount, int startIndex, ref int requiredSize);

		// Token: 0x06000A2B RID: 2603
		[DllImport("webengine.dll")]
		internal static extern int EcbGetVersion(IntPtr pECB);

		// Token: 0x06000A2C RID: 2604
		[DllImport("webengine.dll")]
		internal static extern int EcbGetQueryStringRawBytes(IntPtr pECB, byte[] buffer, int size);

		// Token: 0x06000A2D RID: 2605
		[DllImport("webengine.dll")]
		internal static extern int EcbGetPreloadedPostedContent(IntPtr pECB, byte[] bytes, int offset, int bufferSize);

		// Token: 0x06000A2E RID: 2606
		[DllImport("webengine.dll")]
		internal static extern int EcbGetAdditionalPostedContent(IntPtr pECB, byte[] bytes, int offset, int bufferSize);

		// Token: 0x06000A2F RID: 2607
		[DllImport("webengine.dll")]
		internal static extern int EcbFlushCore(IntPtr pECB, byte[] status, byte[] header, int keepConnected, int totalBodySize, int numBodyFragments, IntPtr[] bodyFragments, int[] bodyFragmentLengths, int doneWithSession, int finalStatus, int kernelCache, int async, ISAPIAsyncCompletionCallback asyncCompletionCallback);

		// Token: 0x06000A30 RID: 2608
		[DllImport("webengine.dll")]
		internal static extern int EcbIsClientConnected(IntPtr pECB);

		// Token: 0x06000A31 RID: 2609
		[DllImport("webengine.dll")]
		internal static extern int EcbCloseConnection(IntPtr pECB);

		// Token: 0x06000A32 RID: 2610
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int EcbMapUrlToPath(IntPtr pECB, string url, byte[] buffer, int size);

		// Token: 0x06000A33 RID: 2611
		[DllImport("webengine.dll")]
		internal static extern IntPtr EcbGetImpersonationToken(IntPtr pECB, IntPtr processHandle);

		// Token: 0x06000A34 RID: 2612
		[DllImport("webengine.dll")]
		internal static extern IntPtr EcbGetVirtualPathToken(IntPtr pECB, IntPtr processHandle);

		// Token: 0x06000A35 RID: 2613
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int EcbAppendLogParameter(IntPtr pECB, string logParam);

		// Token: 0x06000A36 RID: 2614
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int EcbExecuteUrlUnicode(IntPtr pECB, string url, string method, string childHeaders, bool sendHeaders, bool addUserIndo, IntPtr token, string name, string authType, IntPtr pEntity, ISAPIAsyncCompletionCallback asyncCompletionCallback);

		// Token: 0x06000A37 RID: 2615
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void InvalidateKernelCache(string key);

		// Token: 0x06000A38 RID: 2616
		[DllImport("webengine.dll")]
		internal static extern void FreeFileSecurityDescriptor(IntPtr securityDesciptor);

		// Token: 0x06000A39 RID: 2617
		[DllImport("webengine.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr GetFileHandleForTransmitFile(string strFile);

		// Token: 0x06000A3A RID: 2618
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr GetFileSecurityDescriptor(string strFile);

		// Token: 0x06000A3B RID: 2619
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetGroupsForUser(IntPtr token, StringBuilder allGroups, int allGrpSize, StringBuilder error, int errorSize);

		// Token: 0x06000A3C RID: 2620
		[DllImport("webengine.dll")]
		internal static extern int GetHMACSHA1Hash(byte[] data1, int dataOffset1, int dataSize1, byte[] data2, int dataSize2, byte[] innerKey, int innerKeySize, byte[] outerKey, int outerKeySize, byte[] hash, int hashSize);

		// Token: 0x06000A3D RID: 2621
		[DllImport("webengine.dll")]
		internal static extern int GetPrivateBytesIIS6(out long privatePageCount, bool nocache);

		// Token: 0x06000A3E RID: 2622
		[DllImport("webengine.dll")]
		internal static extern int GetProcessMemoryInformation(uint pid, out uint privatePageCount, out uint peakPagefileUsage, bool nocache);

		// Token: 0x06000A3F RID: 2623
		[DllImport("webengine.dll")]
		internal static extern int GetSHA1Hash(byte[] data, int dataSize, byte[] hash, int hashSize);

		// Token: 0x06000A40 RID: 2624
		[DllImport("webengine.dll")]
		internal static extern int GetW3WPMemoryLimitInKB();

		// Token: 0x06000A41 RID: 2625
		[DllImport("webengine.dll")]
		internal static extern void SetGCLastCalledTime(out bool pfCall, int lFrequencyInSeconds);

		// Token: 0x06000A42 RID: 2626
		[DllImport("webengine.dll")]
		internal static extern void SetClrThreadPoolLimits(int maxWorkerThreads, int maxIoThreads);

		// Token: 0x06000A43 RID: 2627
		[DllImport("webengine.dll")]
		internal static extern void SetMinRequestsExecutingToDetectDeadlock(int minRequestsExecutingToDetectDeadlock);

		// Token: 0x06000A44 RID: 2628
		[DllImport("webengine.dll")]
		internal static extern void InitializeLibrary();

		// Token: 0x06000A45 RID: 2629
		[DllImport("webengine.dll")]
		internal static extern void PerfCounterInitialize();

		// Token: 0x06000A46 RID: 2630
		[DllImport("webengine.dll")]
		internal static extern void InitializeHealthMonitor(int deadlockIntervalSeconds, int requestQueueLimit);

		// Token: 0x06000A47 RID: 2631
		[DllImport("webengine.dll")]
		internal static extern int IsAccessToFileAllowed(IntPtr securityDesciptor, IntPtr iThreadToken, int iAccess);

		// Token: 0x06000A48 RID: 2632
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int IsUserInRole(IntPtr token, string rolename, StringBuilder error, int errorSize);

		// Token: 0x06000A49 RID: 2633
		[DllImport("webengine.dll")]
		internal static extern void UpdateLastActivityTimeForHealthMonitor();

		// Token: 0x06000A4A RID: 2634
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int GetCredentialFromRegistry(string strRegKey, StringBuilder buffer, int size);

		// Token: 0x06000A4B RID: 2635
		[DllImport("webengine.dll", BestFitMapping = false)]
		internal static extern int EcbGetChannelBindingToken(IntPtr pECB, out IntPtr token, out int tokenSize);

		// Token: 0x06000A4C RID: 2636
		[DllImport("webengine.dll")]
		internal static extern int EcbCallISAPI(IntPtr pECB, UnsafeNativeMethods.CallISAPIFunc iFunction, byte[] bufferIn, int sizeIn, byte[] bufferOut, int sizeOut);

		// Token: 0x06000A4D RID: 2637
		[DllImport("webengine.dll")]
		internal static extern int PassportVersion();

		// Token: 0x06000A4E RID: 2638
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportCreateHttpRaw(string szRequestLine, string szHeaders, int fSecure, StringBuilder szBufOut, int dwRetBufSize, ref IntPtr passportManager);

		// Token: 0x06000A4F RID: 2639
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportTicket(IntPtr pManager, string szAttr, out object pReturn);

		// Token: 0x06000A50 RID: 2640
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetCurrentConfig(IntPtr pManager, string szAttr, out object pReturn);

		// Token: 0x06000A51 RID: 2641
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportLogoutURL(IntPtr pManager, string szReturnURL, string szCOBrandArgs, int iLangID, string strDomain, int iUseSecureAuth, StringBuilder szAuthVal, int iAuthValSize);

		// Token: 0x06000A52 RID: 2642
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetOption(IntPtr pManager, string szOption, out object vOut);

		// Token: 0x06000A53 RID: 2643
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportSetOption(IntPtr pManager, string szOption, object vOut);

		// Token: 0x06000A54 RID: 2644
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetLoginChallenge(IntPtr pManager, string szRetURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth, object vExtraParams, StringBuilder szOut, int iOutSize);

		// Token: 0x06000A55 RID: 2645
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportHexPUID(IntPtr pManager, StringBuilder szOut, int iOutSize);

		// Token: 0x06000A56 RID: 2646
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportCreate(string szQueryStrT, string szQueryStrP, string szAuthCookie, string szProfCookie, string szProfCCookie, StringBuilder szAuthCookieRet, StringBuilder szProfCookieRet, int iRetBufSize, ref IntPtr passportManager);

		// Token: 0x06000A57 RID: 2647
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportAuthURL(IntPtr iPassport, string szReturnURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth, StringBuilder szAuthVal, int iAuthValSize);

		// Token: 0x06000A58 RID: 2648
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportAuthURL2(IntPtr iPassport, string szReturnURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth, StringBuilder szAuthVal, int iAuthValSize);

		// Token: 0x06000A59 RID: 2649
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetError(IntPtr iPassport);

		// Token: 0x06000A5A RID: 2650
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportDomainFromMemberName(IntPtr iPassport, string szDomain, StringBuilder szMember, int iMemberSize);

		// Token: 0x06000A5B RID: 2651
		[DllImport("webengine.dll")]
		internal static extern int PassportGetFromNetworkServer(IntPtr iPassport);

		// Token: 0x06000A5C RID: 2652
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetDomainAttribute(IntPtr iPassport, string szAttributeName, int iLCID, string szDomain, StringBuilder szValue, int iValueSize);

		// Token: 0x06000A5D RID: 2653
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportHasProfile(IntPtr iPassport, string szProfile);

		// Token: 0x06000A5E RID: 2654
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportHasFlag(IntPtr iPassport, int iFlagMask);

		// Token: 0x06000A5F RID: 2655
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportHasConsent(IntPtr iPassport, int iFullConsent, int iNeedBirthdate);

		// Token: 0x06000A60 RID: 2656
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetHasSavedPassword(IntPtr iPassport);

		// Token: 0x06000A61 RID: 2657
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportHasTicket(IntPtr iPassport);

		// Token: 0x06000A62 RID: 2658
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportIsAuthenticated(IntPtr iPassport, int iTimeWindow, int fForceLogin, int iUseSecureAuth);

		// Token: 0x06000A63 RID: 2659
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportLogoTag(IntPtr iPassport, string szRetURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, int fSecure, string strNameSpace, int iKPP, int iUseSecureAuth, StringBuilder szValue, int iValueSize);

		// Token: 0x06000A64 RID: 2660
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportLogoTag2(IntPtr iPassport, string szRetURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, int fSecure, string strNameSpace, int iKPP, int iUseSecureAuth, StringBuilder szValue, int iValueSize);

		// Token: 0x06000A65 RID: 2661
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetProfile(IntPtr iPassport, string szProfile, out object rOut);

		// Token: 0x06000A66 RID: 2662
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetTicketAge(IntPtr iPassport);

		// Token: 0x06000A67 RID: 2663
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportGetTimeSinceSignIn(IntPtr iPassport);

		// Token: 0x06000A68 RID: 2664
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void PassportDestroy(IntPtr iPassport);

		// Token: 0x06000A69 RID: 2665
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportCrypt(int iFunctionID, string szSrc, StringBuilder szDest, int iDestLength);

		// Token: 0x06000A6A RID: 2666
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int PassportCryptPut(int iFunctionID, string szSrc);

		// Token: 0x06000A6B RID: 2667
		[DllImport("webengine.dll")]
		internal static extern int PassportCryptIsValid();

		// Token: 0x06000A6C RID: 2668
		[DllImport("webengine.dll")]
		internal static extern int PostThreadPoolWorkItem(WorkItemCallback callback);

		// Token: 0x06000A6D RID: 2669
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr InstrumentedMutexCreate(string name);

		// Token: 0x06000A6E RID: 2670
		[DllImport("webengine.dll")]
		internal static extern void InstrumentedMutexDelete(HandleRef mutex);

		// Token: 0x06000A6F RID: 2671
		[DllImport("webengine.dll")]
		internal static extern int InstrumentedMutexGetLock(HandleRef mutex, int timeout);

		// Token: 0x06000A70 RID: 2672
		[DllImport("webengine.dll")]
		internal static extern int InstrumentedMutexReleaseLock(HandleRef mutex);

		// Token: 0x06000A71 RID: 2673
		[DllImport("webengine.dll")]
		internal static extern void InstrumentedMutexSetState(HandleRef mutex, int state);

		// Token: 0x06000A72 RID: 2674
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostMapPath(string appId, string virtualPath, StringBuilder buffer, int size);

		// Token: 0x06000A73 RID: 2675
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostGetAppPath(string aboPath, StringBuilder buffer, int size);

		// Token: 0x06000A74 RID: 2676
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostGetUncUser(string appId, StringBuilder usernameBuffer, int usernameSize, StringBuilder passwordBuffer, int passwordSize);

		// Token: 0x06000A75 RID: 2677
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostGetSiteName(string appId, StringBuilder buffer, int size);

		// Token: 0x06000A76 RID: 2678
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostGetSiteId(string site, StringBuilder buffer, int size);

		// Token: 0x06000A77 RID: 2679
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Unicode)]
		internal static extern int IsapiAppHostGetNextVirtualSubdir(string aboPath, bool inApp, ref int index, StringBuilder sb, int size);

		// Token: 0x06000A78 RID: 2680
		[DllImport("webengine.dll", BestFitMapping = false)]
		internal static extern IntPtr BufferPoolGetPool(int bufferSize, int maxFreeListCount);

		// Token: 0x06000A79 RID: 2681
		[DllImport("webengine.dll", BestFitMapping = false)]
		internal static extern IntPtr BufferPoolGetBuffer(IntPtr pool);

		// Token: 0x06000A7A RID: 2682
		[DllImport("webengine.dll", BestFitMapping = false)]
		internal static extern void BufferPoolReleaseBuffer(IntPtr buffer);

		// Token: 0x06000A7B RID: 2683
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetTraceContextId(IntPtr pMsg, out Guid traceContextId);

		// Token: 0x06000A7C RID: 2684
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetHistoryTable(int iRows, int[] dwPIDArr, int[] dwReqExecuted, int[] dwReqPending, int[] dwReqExecuting, int[] dwReasonForDeath, int[] dwPeakMemoryUsed, long[] tmCreateTime, long[] tmDeathTime);

		// Token: 0x06000A7D RID: 2685
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetCurrentProcessInfo(ref int dwReqExecuted, ref int dwReqExecuting, ref int dwPeakMemoryUsed, ref long tmCreateTime, ref int pid);

		// Token: 0x06000A7E RID: 2686
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetMemoryLimitInMB();

		// Token: 0x06000A7F RID: 2687
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetBasics(IntPtr pMsg, byte[] buffer, int size, int[] contentInfo);

		// Token: 0x06000A80 RID: 2688
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetClientCertificate(IntPtr pMsg, byte[] buffer, int size, int[] pInts, long[] pDates);

		// Token: 0x06000A81 RID: 2689
		[DllImport("aspnet_wp.exe")]
		internal static extern long PMGetStartTimeStamp(IntPtr pMsg);

		// Token: 0x06000A82 RID: 2690
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetAllServerVariables(IntPtr pMsg, byte[] buffer, int size);

		// Token: 0x06000A83 RID: 2691
		[DllImport("aspnet_wp.exe", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int PMGetQueryString(IntPtr pMsg, int encode, StringBuilder buffer, int size);

		// Token: 0x06000A84 RID: 2692
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetQueryStringRawBytes(IntPtr pMsg, byte[] buffer, int size);

		// Token: 0x06000A85 RID: 2693
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetPreloadedPostedContent(IntPtr pMsg, byte[] bytes, int offset, int bufferSize);

		// Token: 0x06000A86 RID: 2694
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMGetAdditionalPostedContent(IntPtr pMsg, byte[] bytes, int offset, int bufferSize);

		// Token: 0x06000A87 RID: 2695
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMEmptyResponse(IntPtr pMsg);

		// Token: 0x06000A88 RID: 2696
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMIsClientConnected(IntPtr pMsg);

		// Token: 0x06000A89 RID: 2697
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMCloseConnection(IntPtr pMsg);

		// Token: 0x06000A8A RID: 2698
		[DllImport("aspnet_wp.exe", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int PMMapUrlToPath(IntPtr pMsg, string url, byte[] buffer, int size);

		// Token: 0x06000A8B RID: 2699
		[DllImport("aspnet_wp.exe")]
		internal static extern IntPtr PMGetImpersonationToken(IntPtr pMsg);

		// Token: 0x06000A8C RID: 2700
		[DllImport("aspnet_wp.exe")]
		internal static extern IntPtr PMGetVirtualPathToken(IntPtr pMsg);

		// Token: 0x06000A8D RID: 2701
		[DllImport("aspnet_wp.exe", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int PMAppendLogParameter(IntPtr pMsg, string logParam);

		// Token: 0x06000A8E RID: 2702
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMFlushCore(IntPtr pMsg, byte[] status, byte[] header, int keepConnected, int totalBodySize, int bodyFragmentsOffset, int numBodyFragments, IntPtr[] bodyFragments, int[] bodyFragmentLengths, int doneWithSession, int finalStatus);

		// Token: 0x06000A8F RID: 2703
		[DllImport("aspnet_wp.exe")]
		internal static extern int PMCallISAPI(IntPtr pECB, UnsafeNativeMethods.CallISAPIFunc iFunction, byte[] bufferIn, int sizeIn, byte[] bufferOut, int sizeOut);

		// Token: 0x06000A90 RID: 2704
		[DllImport("webengine.dll")]
		internal static extern IntPtr PerfOpenGlobalCounters();

		// Token: 0x06000A91 RID: 2705
		[DllImport("webengine.dll")]
		internal static extern IntPtr PerfOpenStateCounters();

		// Token: 0x06000A92 RID: 2706
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern PerfInstanceDataHandle PerfOpenAppCounters(string AppName);

		// Token: 0x06000A93 RID: 2707
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("webengine.dll")]
		internal static extern void PerfCloseAppCounters(IntPtr pCounters);

		// Token: 0x06000A94 RID: 2708
		[DllImport("webengine.dll")]
		internal static extern void PerfIncrementCounter(IntPtr pCounters, int number);

		// Token: 0x06000A95 RID: 2709
		[DllImport("webengine.dll")]
		internal static extern void PerfDecrementCounter(IntPtr pCounters, int number);

		// Token: 0x06000A96 RID: 2710
		[DllImport("webengine.dll")]
		internal static extern void PerfIncrementCounterEx(IntPtr pCounters, int number, int increment);

		// Token: 0x06000A97 RID: 2711
		[DllImport("webengine.dll")]
		internal static extern void PerfSetCounter(IntPtr pCounters, int number, int increment);

		// Token: 0x06000A98 RID: 2712
		[DllImport("webengine.dll")]
		internal static extern int PerfGetCounter(IntPtr pCounters, int number);

		// Token: 0x06000A99 RID: 2713
		[DllImport("webengine.dll")]
		internal static extern void GetEtwValues(out int level, out int flags);

		// Token: 0x06000A9A RID: 2714
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void TraceRaiseEventMgdHandler(int eventType, IntPtr pRequestContext, string data1, string data2, string data3, string data4);

		// Token: 0x06000A9B RID: 2715
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void TraceRaiseEventWithEcb(int eventType, IntPtr ecb, string data1, string data2, string data3, string data4);

		// Token: 0x06000A9C RID: 2716
		[DllImport("aspnet_wp.exe", CharSet = CharSet.Unicode)]
		internal static extern void PMTraceRaiseEvent(int eventType, IntPtr pMsg, string data1, string data2, string data3, string data4);

		// Token: 0x06000A9D RID: 2717
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int SessionNDConnectToService(string server);

		// Token: 0x06000A9E RID: 2718
		[DllImport("webengine.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern int SessionNDMakeRequest(HandleRef socket, string server, int port, int networkTimeout, UnsafeNativeMethods.StateProtocolVerb verb, string uri, UnsafeNativeMethods.StateProtocolExclusive exclusive, int extraFlags, int timeout, int lockCookie, byte[] body, int cb, bool checkVersion, out UnsafeNativeMethods.SessionNDMakeRequestResults results);

		// Token: 0x06000A9F RID: 2719
		[DllImport("webengine.dll")]
		internal static extern void SessionNDFreeBody(HandleRef body);

		// Token: 0x06000AA0 RID: 2720
		[DllImport("webengine.dll")]
		internal static extern void SessionNDCloseConnection(HandleRef socket);

		// Token: 0x06000AA1 RID: 2721
		[DllImport("webengine.dll")]
		internal static extern int TransactManagedCallback(TransactedExecCallback callback, int mode);

		// Token: 0x06000AA2 RID: 2722
		[DllImport("webengine.dll", SetLastError = true)]
		internal static extern bool IsValidResource(IntPtr hModule, IntPtr ip, int size);

		// Token: 0x06000AA3 RID: 2723
		[DllImport("mscorwks.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetCachePath(int dwCacheFlags, StringBuilder pwzCachePath, ref int pcchPath);

		// Token: 0x06000AA4 RID: 2724
		[DllImport("mscorwks.dll", CharSet = CharSet.Unicode)]
		internal static extern int DeleteShadowCache(string pwzCachePath, string pwzAppName);

		// Token: 0x06000AA5 RID: 2725
		[DllImport("webengine.dll")]
		internal static extern int InitializeWmiManager();

		// Token: 0x06000AA6 RID: 2726
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int DoesKeyContainerExist(string containerName, string provider, int useMachineContainer);

		// Token: 0x06000AA7 RID: 2727
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int RaiseWmiEvent(ref UnsafeNativeMethods.WmiData pWmiData, bool IsInAspCompatMode);

		// Token: 0x06000AA8 RID: 2728
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern int RaiseEventlogEvent(int eventType, string[] dataFields, int size);

		// Token: 0x06000AA9 RID: 2729
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void LogWebeventProviderFailure(string appUrl, string providerName, string exception);

		// Token: 0x06000AAA RID: 2730
		[DllImport("webengine.dll")]
		internal static extern IntPtr GetEcb(IntPtr pHttpCompletion);

		// Token: 0x06000AAB RID: 2731
		[DllImport("webengine.dll")]
		internal static extern void SetDoneWithSessionCalled(IntPtr pHttpCompletion);

		// Token: 0x06000AAC RID: 2732
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void ReportUnhandledException(string eventInfo);

		// Token: 0x06000AAD RID: 2733
		[DllImport("webengine.dll", CharSet = CharSet.Unicode)]
		internal static extern void RaiseFileMonitoringEventlogEvent(string eventInfo, string path, string appVirtualPath, int hr);

		// Token: 0x06000AAE RID: 2734
		[DllImport("ole32.dll", CharSet = CharSet.Unicode)]
		internal static extern int CoCreateInstanceEx(ref Guid clsid, IntPtr pUnkOuter, int dwClsContext, [In] [Out] COSERVERINFO srv, int num, [In] [Out] MULTI_QI[] amqi);

		// Token: 0x06000AAF RID: 2735
		[DllImport("ole32.dll", CharSet = CharSet.Unicode)]
		internal static extern int CoCreateInstanceEx(ref Guid clsid, IntPtr pUnkOuter, int dwClsContext, [In] [Out] COSERVERINFO_X64 srv, int num, [In] [Out] MULTI_QI_X64[] amqi);

		// Token: 0x06000AB0 RID: 2736
		[DllImport("ole32.dll", CharSet = CharSet.Unicode)]
		internal static extern int CoSetProxyBlanket(IntPtr pProxy, RpcAuthent authent, RpcAuthor author, string serverprinc, RpcLevel level, RpcImpers impers, IntPtr ciptr, int dwCapabilities);

		// Token: 0x04001268 RID: 4712
		public const int TOKEN_ALL_ACCESS = 983551;

		// Token: 0x04001269 RID: 4713
		public const int TOKEN_EXECUTE = 131072;

		// Token: 0x0400126A RID: 4714
		public const int TOKEN_READ = 131080;

		// Token: 0x0400126B RID: 4715
		public const int TOKEN_IMPERSONATE = 4;

		// Token: 0x0400126C RID: 4716
		public const int ERROR_NO_TOKEN = 1008;

		// Token: 0x0400126D RID: 4717
		public const int OWNER_SECURITY_INFORMATION = 1;

		// Token: 0x0400126E RID: 4718
		public const int GROUP_SECURITY_INFORMATION = 2;

		// Token: 0x0400126F RID: 4719
		public const int DACL_SECURITY_INFORMATION = 4;

		// Token: 0x04001270 RID: 4720
		public const int SACL_SECURITY_INFORMATION = 8;

		// Token: 0x04001271 RID: 4721
		internal const int FILE_ATTRIBUTE_READONLY = 1;

		// Token: 0x04001272 RID: 4722
		internal const int FILE_ATTRIBUTE_HIDDEN = 2;

		// Token: 0x04001273 RID: 4723
		internal const int FILE_ATTRIBUTE_SYSTEM = 4;

		// Token: 0x04001274 RID: 4724
		internal const int FILE_ATTRIBUTE_DIRECTORY = 16;

		// Token: 0x04001275 RID: 4725
		internal const int FILE_ATTRIBUTE_ARCHIVE = 32;

		// Token: 0x04001276 RID: 4726
		internal const int FILE_ATTRIBUTE_DEVICE = 64;

		// Token: 0x04001277 RID: 4727
		internal const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x04001278 RID: 4728
		internal const int FILE_ATTRIBUTE_TEMPORARY = 256;

		// Token: 0x04001279 RID: 4729
		internal const int FILE_ATTRIBUTE_SPARSE_FILE = 512;

		// Token: 0x0400127A RID: 4730
		internal const int FILE_ATTRIBUTE_REPARSE_POINT = 1024;

		// Token: 0x0400127B RID: 4731
		internal const int FILE_ATTRIBUTE_COMPRESSED = 2048;

		// Token: 0x0400127C RID: 4732
		internal const int FILE_ATTRIBUTE_OFFLINE = 4096;

		// Token: 0x0400127D RID: 4733
		internal const int FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 8192;

		// Token: 0x0400127E RID: 4734
		internal const int FILE_ATTRIBUTE_ENCRYPTED = 16384;

		// Token: 0x0400127F RID: 4735
		internal const int DELETE = 65536;

		// Token: 0x04001280 RID: 4736
		internal const int READ_CONTROL = 131072;

		// Token: 0x04001281 RID: 4737
		internal const int WRITE_DAC = 262144;

		// Token: 0x04001282 RID: 4738
		internal const int WRITE_OWNER = 524288;

		// Token: 0x04001283 RID: 4739
		internal const int SYNCHRONIZE = 1048576;

		// Token: 0x04001284 RID: 4740
		internal const int STANDARD_RIGHTS_REQUIRED = 983040;

		// Token: 0x04001285 RID: 4741
		internal const int STANDARD_RIGHTS_READ = 131072;

		// Token: 0x04001286 RID: 4742
		internal const int STANDARD_RIGHTS_WRITE = 131072;

		// Token: 0x04001287 RID: 4743
		internal const int STANDARD_RIGHTS_EXECUTE = 131072;

		// Token: 0x04001288 RID: 4744
		internal const int GENERIC_READ = -2147483648;

		// Token: 0x04001289 RID: 4745
		internal const int STANDARD_RIGHTS_ALL = 2031616;

		// Token: 0x0400128A RID: 4746
		internal const int SPECIFIC_RIGHTS_ALL = 65535;

		// Token: 0x0400128B RID: 4747
		internal const int FILE_SHARE_READ = 1;

		// Token: 0x0400128C RID: 4748
		internal const int FILE_SHARE_WRITE = 2;

		// Token: 0x0400128D RID: 4749
		internal const int FILE_SHARE_DELETE = 4;

		// Token: 0x0400128E RID: 4750
		internal const int OPEN_EXISTING = 3;

		// Token: 0x0400128F RID: 4751
		internal const int OPEN_ALWAYS = 4;

		// Token: 0x04001290 RID: 4752
		internal const int FILE_FLAG_WRITE_THROUGH = -2147483648;

		// Token: 0x04001291 RID: 4753
		internal const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x04001292 RID: 4754
		internal const int FILE_FLAG_NO_BUFFERING = 536870912;

		// Token: 0x04001293 RID: 4755
		internal const int FILE_FLAG_RANDOM_ACCESS = 268435456;

		// Token: 0x04001294 RID: 4756
		internal const int FILE_FLAG_SEQUENTIAL_SCAN = 134217728;

		// Token: 0x04001295 RID: 4757
		internal const int FILE_FLAG_DELETE_ON_CLOSE = 67108864;

		// Token: 0x04001296 RID: 4758
		internal const int FILE_FLAG_BACKUP_SEMANTICS = 33554432;

		// Token: 0x04001297 RID: 4759
		internal const int FILE_FLAG_POSIX_SEMANTICS = 16777216;

		// Token: 0x04001298 RID: 4760
		internal const int GetFileExInfoStandard = 0;

		// Token: 0x04001299 RID: 4761
		internal const uint FILE_NOTIFY_CHANGE_FILE_NAME = 1U;

		// Token: 0x0400129A RID: 4762
		internal const uint FILE_NOTIFY_CHANGE_DIR_NAME = 2U;

		// Token: 0x0400129B RID: 4763
		internal const uint FILE_NOTIFY_CHANGE_ATTRIBUTES = 4U;

		// Token: 0x0400129C RID: 4764
		internal const uint FILE_NOTIFY_CHANGE_SIZE = 8U;

		// Token: 0x0400129D RID: 4765
		internal const uint FILE_NOTIFY_CHANGE_LAST_WRITE = 16U;

		// Token: 0x0400129E RID: 4766
		internal const uint FILE_NOTIFY_CHANGE_LAST_ACCESS = 32U;

		// Token: 0x0400129F RID: 4767
		internal const uint FILE_NOTIFY_CHANGE_CREATION = 64U;

		// Token: 0x040012A0 RID: 4768
		internal const uint FILE_NOTIFY_CHANGE_SECURITY = 256U;

		// Token: 0x040012A1 RID: 4769
		internal const uint RDCW_FILTER_FILE_AND_DIR_CHANGES = 347U;

		// Token: 0x040012A2 RID: 4770
		internal const uint RDCW_FILTER_FILE_CHANGES = 345U;

		// Token: 0x040012A3 RID: 4771
		internal const uint RDCW_FILTER_DIR_RENAMES = 2U;

		// Token: 0x040012A4 RID: 4772
		public const int RESTRICT_BIN = 1;

		// Token: 0x040012A5 RID: 4773
		internal const int StateProtocolFlagUninitialized = 1;

		// Token: 0x040012A6 RID: 4774
		internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		// Token: 0x020000DB RID: 219
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WIN32_FIND_DATA
		{
			// Token: 0x040012A7 RID: 4775
			internal uint dwFileAttributes;

			// Token: 0x040012A8 RID: 4776
			internal uint ftCreationTime_dwLowDateTime;

			// Token: 0x040012A9 RID: 4777
			internal uint ftCreationTime_dwHighDateTime;

			// Token: 0x040012AA RID: 4778
			internal uint ftLastAccessTime_dwLowDateTime;

			// Token: 0x040012AB RID: 4779
			internal uint ftLastAccessTime_dwHighDateTime;

			// Token: 0x040012AC RID: 4780
			internal uint ftLastWriteTime_dwLowDateTime;

			// Token: 0x040012AD RID: 4781
			internal uint ftLastWriteTime_dwHighDateTime;

			// Token: 0x040012AE RID: 4782
			internal uint nFileSizeHigh;

			// Token: 0x040012AF RID: 4783
			internal uint nFileSizeLow;

			// Token: 0x040012B0 RID: 4784
			internal uint dwReserved0;

			// Token: 0x040012B1 RID: 4785
			internal uint dwReserved1;

			// Token: 0x040012B2 RID: 4786
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string cFileName;

			// Token: 0x040012B3 RID: 4787
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			internal string cAlternateFileName;
		}

		// Token: 0x020000DC RID: 220
		internal struct WIN32_FILE_ATTRIBUTE_DATA
		{
			// Token: 0x040012B4 RID: 4788
			internal int fileAttributes;

			// Token: 0x040012B5 RID: 4789
			internal uint ftCreationTimeLow;

			// Token: 0x040012B6 RID: 4790
			internal uint ftCreationTimeHigh;

			// Token: 0x040012B7 RID: 4791
			internal uint ftLastAccessTimeLow;

			// Token: 0x040012B8 RID: 4792
			internal uint ftLastAccessTimeHigh;

			// Token: 0x040012B9 RID: 4793
			internal uint ftLastWriteTimeLow;

			// Token: 0x040012BA RID: 4794
			internal uint ftLastWriteTimeHigh;

			// Token: 0x040012BB RID: 4795
			internal uint fileSizeHigh;

			// Token: 0x040012BC RID: 4796
			internal uint fileSizeLow;
		}

		// Token: 0x020000DD RID: 221
		internal struct WIN32_BY_HANDLE_FILE_INFORMATION
		{
			// Token: 0x040012BD RID: 4797
			internal int fileAttributes;

			// Token: 0x040012BE RID: 4798
			internal uint ftCreationTimeLow;

			// Token: 0x040012BF RID: 4799
			internal uint ftCreationTimeHigh;

			// Token: 0x040012C0 RID: 4800
			internal uint ftLastAccessTimeLow;

			// Token: 0x040012C1 RID: 4801
			internal uint ftLastAccessTimeHigh;

			// Token: 0x040012C2 RID: 4802
			internal uint ftLastWriteTimeLow;

			// Token: 0x040012C3 RID: 4803
			internal uint ftLastWriteTimeHigh;

			// Token: 0x040012C4 RID: 4804
			internal uint volumeSerialNumber;

			// Token: 0x040012C5 RID: 4805
			internal uint fileSizeHigh;

			// Token: 0x040012C6 RID: 4806
			internal uint fileSizeLow;

			// Token: 0x040012C7 RID: 4807
			internal uint numberOfLinks;

			// Token: 0x040012C8 RID: 4808
			internal uint fileIndexHigh;

			// Token: 0x040012C9 RID: 4809
			internal uint fileIndexLow;
		}

		// Token: 0x020000DE RID: 222
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SYSTEM_INFO
		{
			// Token: 0x040012CA RID: 4810
			public ushort wProcessorArchitecture;

			// Token: 0x040012CB RID: 4811
			public ushort wReserved;

			// Token: 0x040012CC RID: 4812
			public uint dwPageSize;

			// Token: 0x040012CD RID: 4813
			public IntPtr lpMinimumApplicationAddress;

			// Token: 0x040012CE RID: 4814
			public IntPtr lpMaximumApplicationAddress;

			// Token: 0x040012CF RID: 4815
			public IntPtr dwActiveProcessorMask;

			// Token: 0x040012D0 RID: 4816
			public uint dwNumberOfProcessors;

			// Token: 0x040012D1 RID: 4817
			public uint dwProcessorType;

			// Token: 0x040012D2 RID: 4818
			public uint dwAllocationGranularity;

			// Token: 0x040012D3 RID: 4819
			public ushort wProcessorLevel;

			// Token: 0x040012D4 RID: 4820
			public ushort wProcessorRevision;
		}

		// Token: 0x020000DF RID: 223
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct MEMORYSTATUSEX
		{
			// Token: 0x06000AB2 RID: 2738 RVA: 0x0002B663 File Offset: 0x0002A663
			internal void Init()
			{
				this.dwLength = Marshal.SizeOf(typeof(UnsafeNativeMethods.MEMORYSTATUSEX));
			}

			// Token: 0x040012D5 RID: 4821
			internal int dwLength;

			// Token: 0x040012D6 RID: 4822
			internal int dwMemoryLoad;

			// Token: 0x040012D7 RID: 4823
			internal long ullTotalPhys;

			// Token: 0x040012D8 RID: 4824
			internal long ullAvailPhys;

			// Token: 0x040012D9 RID: 4825
			internal long ullTotalPageFile;

			// Token: 0x040012DA RID: 4826
			internal long ullAvailPageFile;

			// Token: 0x040012DB RID: 4827
			internal long ullTotalVirtual;

			// Token: 0x040012DC RID: 4828
			internal long ullAvailVirtual;

			// Token: 0x040012DD RID: 4829
			internal long ullAvailExtendedVirtual;
		}

		// Token: 0x020000E0 RID: 224
		internal enum CallISAPIFunc
		{
			// Token: 0x040012DF RID: 4831
			GetSiteServerComment = 1,
			// Token: 0x040012E0 RID: 4832
			RestrictIISFolders,
			// Token: 0x040012E1 RID: 4833
			CreateTempDir,
			// Token: 0x040012E2 RID: 4834
			GetAutogenKeys,
			// Token: 0x040012E3 RID: 4835
			GenerateToken
		}

		// Token: 0x020000E1 RID: 225
		internal struct SessionNDMakeRequestResults
		{
			// Token: 0x040012E4 RID: 4836
			internal IntPtr socket;

			// Token: 0x040012E5 RID: 4837
			internal int httpStatus;

			// Token: 0x040012E6 RID: 4838
			internal int timeout;

			// Token: 0x040012E7 RID: 4839
			internal int contentLength;

			// Token: 0x040012E8 RID: 4840
			internal IntPtr content;

			// Token: 0x040012E9 RID: 4841
			internal int lockCookie;

			// Token: 0x040012EA RID: 4842
			internal long lockDate;

			// Token: 0x040012EB RID: 4843
			internal int lockAge;

			// Token: 0x040012EC RID: 4844
			internal int stateServerMajVer;

			// Token: 0x040012ED RID: 4845
			internal int actionFlags;

			// Token: 0x040012EE RID: 4846
			internal int lastPhase;
		}

		// Token: 0x020000E2 RID: 226
		internal enum SessionNDMakeRequestPhase
		{
			// Token: 0x040012F0 RID: 4848
			Initialization,
			// Token: 0x040012F1 RID: 4849
			Connecting,
			// Token: 0x040012F2 RID: 4850
			SendingRequest,
			// Token: 0x040012F3 RID: 4851
			ReadingResponse
		}

		// Token: 0x020000E3 RID: 227
		internal enum StateProtocolVerb
		{
			// Token: 0x040012F5 RID: 4853
			GET = 1,
			// Token: 0x040012F6 RID: 4854
			PUT,
			// Token: 0x040012F7 RID: 4855
			DELETE,
			// Token: 0x040012F8 RID: 4856
			HEAD
		}

		// Token: 0x020000E4 RID: 228
		internal enum StateProtocolExclusive
		{
			// Token: 0x040012FA RID: 4858
			NONE,
			// Token: 0x040012FB RID: 4859
			ACQUIRE,
			// Token: 0x040012FC RID: 4860
			RELEASE
		}

		// Token: 0x020000E5 RID: 229
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WmiData
		{
			// Token: 0x040012FD RID: 4861
			internal int eventType;

			// Token: 0x040012FE RID: 4862
			internal int eventCode;

			// Token: 0x040012FF RID: 4863
			internal int eventDetailCode;

			// Token: 0x04001300 RID: 4864
			internal string eventTime;

			// Token: 0x04001301 RID: 4865
			internal string eventMessage;

			// Token: 0x04001302 RID: 4866
			internal string eventId;

			// Token: 0x04001303 RID: 4867
			internal string sequenceNumber;

			// Token: 0x04001304 RID: 4868
			internal string occurrence;

			// Token: 0x04001305 RID: 4869
			internal int processId;

			// Token: 0x04001306 RID: 4870
			internal string processName;

			// Token: 0x04001307 RID: 4871
			internal string accountName;

			// Token: 0x04001308 RID: 4872
			internal string machineName;

			// Token: 0x04001309 RID: 4873
			internal string appDomain;

			// Token: 0x0400130A RID: 4874
			internal string trustLevel;

			// Token: 0x0400130B RID: 4875
			internal string appVirtualPath;

			// Token: 0x0400130C RID: 4876
			internal string appPath;

			// Token: 0x0400130D RID: 4877
			internal string details;

			// Token: 0x0400130E RID: 4878
			internal string requestUrl;

			// Token: 0x0400130F RID: 4879
			internal string requestPath;

			// Token: 0x04001310 RID: 4880
			internal string userHostAddress;

			// Token: 0x04001311 RID: 4881
			internal string userName;

			// Token: 0x04001312 RID: 4882
			internal bool userAuthenticated;

			// Token: 0x04001313 RID: 4883
			internal string userAuthenticationType;

			// Token: 0x04001314 RID: 4884
			internal string requestThreadAccountName;

			// Token: 0x04001315 RID: 4885
			internal string processStartTime;

			// Token: 0x04001316 RID: 4886
			internal int threadCount;

			// Token: 0x04001317 RID: 4887
			internal string workingSet;

			// Token: 0x04001318 RID: 4888
			internal string peakWorkingSet;

			// Token: 0x04001319 RID: 4889
			internal string managedHeapSize;

			// Token: 0x0400131A RID: 4890
			internal int appdomainCount;

			// Token: 0x0400131B RID: 4891
			internal int requestsExecuting;

			// Token: 0x0400131C RID: 4892
			internal int requestsQueued;

			// Token: 0x0400131D RID: 4893
			internal int requestsRejected;

			// Token: 0x0400131E RID: 4894
			internal int threadId;

			// Token: 0x0400131F RID: 4895
			internal string threadAccountName;

			// Token: 0x04001320 RID: 4896
			internal string stackTrace;

			// Token: 0x04001321 RID: 4897
			internal bool isImpersonating;

			// Token: 0x04001322 RID: 4898
			internal string exceptionType;

			// Token: 0x04001323 RID: 4899
			internal string exceptionMessage;

			// Token: 0x04001324 RID: 4900
			internal string nameToAuthenticate;

			// Token: 0x04001325 RID: 4901
			internal string remoteAddress;

			// Token: 0x04001326 RID: 4902
			internal string remotePort;

			// Token: 0x04001327 RID: 4903
			internal string userAgent;

			// Token: 0x04001328 RID: 4904
			internal string persistedState;

			// Token: 0x04001329 RID: 4905
			internal string referer;

			// Token: 0x0400132A RID: 4906
			internal string path;
		}
	}
}
