using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.ServiceProcess
{
	// Token: 0x0200000A RID: 10
	internal static class NativeMethods
	{
		// Token: 0x0600000D RID: 13
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr databaseHandle, string serviceName, int access);

		// Token: 0x0600000E RID: 14
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr RegisterServiceCtrlHandler(string serviceName, Delegate callback);

		// Token: 0x0600000F RID: 15
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr RegisterServiceCtrlHandlerEx(string serviceName, Delegate callback, IntPtr userData);

		// Token: 0x06000010 RID: 16
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public unsafe static extern bool SetServiceStatus(IntPtr serviceStatusHandle, NativeMethods.SERVICE_STATUS* status);

		// Token: 0x06000011 RID: 17
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool StartServiceCtrlDispatcher(IntPtr entry);

		// Token: 0x06000012 RID: 18
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateService(IntPtr databaseHandle, string serviceName, string displayName, int access, int serviceType, int startType, int errorControl, string binaryPath, string loadOrderGroup, IntPtr pTagId, string dependencies, string servicesStartName, string password);

		// Token: 0x06000013 RID: 19
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool DeleteService(IntPtr serviceHandle);

		// Token: 0x06000014 RID: 20
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaOpenPolicy(NativeMethods.LSA_UNICODE_STRING systemName, IntPtr pointerObjectAttributes, int desiredAccess, out IntPtr pointerPolicyHandle);

		// Token: 0x06000015 RID: 21
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaAddAccountRights(IntPtr policyHandle, byte[] accountSid, NativeMethods.LSA_UNICODE_STRING userRights, int countOfRights);

		// Token: 0x06000016 RID: 22
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaRemoveAccountRights(IntPtr policyHandle, byte[] accountSid, bool allRights, NativeMethods.LSA_UNICODE_STRING userRights, int countOfRights);

		// Token: 0x06000017 RID: 23
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaEnumerateAccountRights(IntPtr policyHandle, byte[] accountSid, out IntPtr pLsaUnicodeStringUserRights, out int RightsCount);

		// Token: 0x06000018 RID: 24
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool LookupAccountName(string systemName, string accountName, byte[] sid, int[] sidLen, char[] refDomainName, int[] domNameLen, [In] [Out] int[] sidNameUse);

		// Token: 0x06000019 RID: 25
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool GetComputerName(StringBuilder lpBuffer, ref int nSize);

		// Token: 0x0600001A RID: 26
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ChangeServiceConfig2(IntPtr serviceHandle, uint infoLevel, ref NativeMethods.SERVICE_DESCRIPTION serviceDesc);

		// Token: 0x0600001B RID: 27
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[Obsolete("Use LoadLibraryHelper.SafeLoadLibraryEx instead")]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LoadLibraryEx([MarshalAs(UnmanagedType.LPTStr)] [In] string lpFileName, IntPtr hFile, [In] NativeMethods.LoadLibraryFlags dwFlags);

		// Token: 0x0600001C RID: 28
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		internal static extern IntPtr GetProcAddress([In] IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] [In] string lpProcName);

		// Token: 0x0600001D RID: 29
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetModuleHandleEx([In] NativeMethods.GetModuleHandleFlags dwFlags, [MarshalAs(UnmanagedType.LPTStr)] [In] [Optional] string lpModuleName, out IntPtr hModule);

		// Token: 0x0600001E RID: 30
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool FreeLibrary([In] IntPtr hModule);

		// Token: 0x040000DC RID: 220
		public const int MAX_COMPUTERNAME_LENGTH = 31;

		// Token: 0x040000DD RID: 221
		public const int WM_POWERBROADCAST = 536;

		// Token: 0x040000DE RID: 222
		public const int NO_ERROR = 0;

		// Token: 0x040000DF RID: 223
		public const int BROADCAST_QUERY_DENY = 1112363332;

		// Token: 0x040000E0 RID: 224
		public const int PBT_APMBATTERYLOW = 9;

		// Token: 0x040000E1 RID: 225
		public const int PBT_APMOEMEVENT = 11;

		// Token: 0x040000E2 RID: 226
		public const int PBT_APMPOWERSTATUSCHANGE = 10;

		// Token: 0x040000E3 RID: 227
		public const int PBT_APMQUERYSUSPEND = 0;

		// Token: 0x040000E4 RID: 228
		public const int PBT_APMQUERYSUSPENDFAILED = 2;

		// Token: 0x040000E5 RID: 229
		public const int PBT_APMRESUMEAUTOMATIC = 18;

		// Token: 0x040000E6 RID: 230
		public const int PBT_APMRESUMECRITICAL = 6;

		// Token: 0x040000E7 RID: 231
		public const int PBT_APMRESUMESUSPEND = 7;

		// Token: 0x040000E8 RID: 232
		public const int PBT_APMSUSPEND = 4;

		// Token: 0x040000E9 RID: 233
		public const int ERROR_MORE_DATA = 234;

		// Token: 0x040000EA RID: 234
		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x040000EB RID: 235
		public const int MB_OK = 0;

		// Token: 0x040000EC RID: 236
		public const int MB_OKCANCEL = 1;

		// Token: 0x040000ED RID: 237
		public const int MB_ABORTRETRYIGNORE = 2;

		// Token: 0x040000EE RID: 238
		public const int MB_YESNOCANCEL = 3;

		// Token: 0x040000EF RID: 239
		public const int MB_YESNO = 4;

		// Token: 0x040000F0 RID: 240
		public const int MB_RETRYCANCEL = 5;

		// Token: 0x040000F1 RID: 241
		public const int MB_ICONHAND = 16;

		// Token: 0x040000F2 RID: 242
		public const int MB_ICONQUESTION = 32;

		// Token: 0x040000F3 RID: 243
		public const int MB_ICONEXCLAMATION = 48;

		// Token: 0x040000F4 RID: 244
		public const int MB_ICONASTERISK = 64;

		// Token: 0x040000F5 RID: 245
		public const int MB_USERICON = 128;

		// Token: 0x040000F6 RID: 246
		public const int MB_ICONWARNING = 48;

		// Token: 0x040000F7 RID: 247
		public const int MB_ICONERROR = 16;

		// Token: 0x040000F8 RID: 248
		public const int MB_ICONINFORMATION = 64;

		// Token: 0x040000F9 RID: 249
		public const int MB_DEFBUTTON1 = 0;

		// Token: 0x040000FA RID: 250
		public const int MB_DEFBUTTON2 = 256;

		// Token: 0x040000FB RID: 251
		public const int MB_DEFBUTTON3 = 512;

		// Token: 0x040000FC RID: 252
		public const int MB_DEFBUTTON4 = 768;

		// Token: 0x040000FD RID: 253
		public const int MB_APPLMODAL = 0;

		// Token: 0x040000FE RID: 254
		public const int MB_SYSTEMMODAL = 4096;

		// Token: 0x040000FF RID: 255
		public const int MB_TASKMODAL = 8192;

		// Token: 0x04000100 RID: 256
		public const int MB_HELP = 16384;

		// Token: 0x04000101 RID: 257
		public const int MB_NOFOCUS = 32768;

		// Token: 0x04000102 RID: 258
		public const int MB_SETFOREGROUND = 65536;

		// Token: 0x04000103 RID: 259
		public const int MB_DEFAULT_DESKTOP_ONLY = 131072;

		// Token: 0x04000104 RID: 260
		public const int MB_TOPMOST = 262144;

		// Token: 0x04000105 RID: 261
		public const int MB_RIGHT = 524288;

		// Token: 0x04000106 RID: 262
		public const int MB_RTLREADING = 1048576;

		// Token: 0x04000107 RID: 263
		public const int MB_SERVICE_NOTIFICATION = 2097152;

		// Token: 0x04000108 RID: 264
		public const int MB_SERVICE_NOTIFICATION_NT3X = 262144;

		// Token: 0x04000109 RID: 265
		public const int MB_TYPEMASK = 15;

		// Token: 0x0400010A RID: 266
		public const int MB_ICONMASK = 240;

		// Token: 0x0400010B RID: 267
		public const int MB_DEFMASK = 3840;

		// Token: 0x0400010C RID: 268
		public const int MB_MODEMASK = 12288;

		// Token: 0x0400010D RID: 269
		public const int MB_MISCMASK = 49152;

		// Token: 0x0400010E RID: 270
		public const int STANDARD_RIGHTS_DELETE = 65536;

		// Token: 0x0400010F RID: 271
		public const int STANDARD_RIGHTS_REQUIRED = 983040;

		// Token: 0x04000110 RID: 272
		public const int SERVICE_NO_CHANGE = -1;

		// Token: 0x04000111 RID: 273
		public const int ACCESS_TYPE_CHANGE_CONFIG = 2;

		// Token: 0x04000112 RID: 274
		public const int ACCESS_TYPE_ENUMERATE_DEPENDENTS = 8;

		// Token: 0x04000113 RID: 275
		public const int ACCESS_TYPE_INTERROGATE = 128;

		// Token: 0x04000114 RID: 276
		public const int ACCESS_TYPE_PAUSE_CONTINUE = 64;

		// Token: 0x04000115 RID: 277
		public const int ACCESS_TYPE_QUERY_CONFIG = 1;

		// Token: 0x04000116 RID: 278
		public const int ACCESS_TYPE_QUERY_STATUS = 4;

		// Token: 0x04000117 RID: 279
		public const int ACCESS_TYPE_START = 16;

		// Token: 0x04000118 RID: 280
		public const int ACCESS_TYPE_STOP = 32;

		// Token: 0x04000119 RID: 281
		public const int ACCESS_TYPE_USER_DEFINED_CONTROL = 256;

		// Token: 0x0400011A RID: 282
		public const int ACCESS_TYPE_ALL = 983551;

		// Token: 0x0400011B RID: 283
		public const int ACCEPT_NETBINDCHANGE = 16;

		// Token: 0x0400011C RID: 284
		public const int ACCEPT_PAUSE_CONTINUE = 2;

		// Token: 0x0400011D RID: 285
		public const int ACCEPT_PARAMCHANGE = 8;

		// Token: 0x0400011E RID: 286
		public const int ACCEPT_POWEREVENT = 64;

		// Token: 0x0400011F RID: 287
		public const int ACCEPT_SHUTDOWN = 4;

		// Token: 0x04000120 RID: 288
		public const int ACCEPT_STOP = 1;

		// Token: 0x04000121 RID: 289
		public const int ACCEPT_SESSIONCHANGE = 128;

		// Token: 0x04000122 RID: 290
		public const int CONTROL_CONTINUE = 3;

		// Token: 0x04000123 RID: 291
		public const int CONTROL_INTERROGATE = 4;

		// Token: 0x04000124 RID: 292
		public const int CONTROL_NETBINDADD = 7;

		// Token: 0x04000125 RID: 293
		public const int CONTROL_NETBINDDISABLE = 10;

		// Token: 0x04000126 RID: 294
		public const int CONTROL_NETBINDENABLE = 9;

		// Token: 0x04000127 RID: 295
		public const int CONTROL_NETBINDREMOVE = 8;

		// Token: 0x04000128 RID: 296
		public const int CONTROL_PARAMCHANGE = 6;

		// Token: 0x04000129 RID: 297
		public const int CONTROL_PAUSE = 2;

		// Token: 0x0400012A RID: 298
		public const int CONTROL_POWEREVENT = 13;

		// Token: 0x0400012B RID: 299
		public const int CONTROL_SHUTDOWN = 5;

		// Token: 0x0400012C RID: 300
		public const int CONTROL_STOP = 1;

		// Token: 0x0400012D RID: 301
		public const int CONTROL_DEVICEEVENT = 11;

		// Token: 0x0400012E RID: 302
		public const int CONTROL_SESSIONCHANGE = 14;

		// Token: 0x0400012F RID: 303
		public const int SERVICE_CONFIG_DESCRIPTION = 1;

		// Token: 0x04000130 RID: 304
		public const int SERVICE_CONFIG_FAILURE_ACTIONS = 2;

		// Token: 0x04000131 RID: 305
		public const int ERROR_CONTROL_CRITICAL = 3;

		// Token: 0x04000132 RID: 306
		public const int ERROR_CONTROL_IGNORE = 0;

		// Token: 0x04000133 RID: 307
		public const int ERROR_CONTROL_NORMAL = 1;

		// Token: 0x04000134 RID: 308
		public const int ERROR_CONTROL_SEVERE = 2;

		// Token: 0x04000135 RID: 309
		public const int SC_MANAGER_CONNECT = 1;

		// Token: 0x04000136 RID: 310
		public const int SC_MANAGER_CREATE_SERVICE = 2;

		// Token: 0x04000137 RID: 311
		public const int SC_MANAGER_ENUMERATE_SERVICE = 4;

		// Token: 0x04000138 RID: 312
		public const int SC_MANAGER_LOCK = 8;

		// Token: 0x04000139 RID: 313
		public const int SC_MANAGER_MODIFY_BOOT_CONFIG = 32;

		// Token: 0x0400013A RID: 314
		public const int SC_MANAGER_QUERY_LOCK_STATUS = 16;

		// Token: 0x0400013B RID: 315
		public const int SC_MANAGER_ALL = 983103;

		// Token: 0x0400013C RID: 316
		public const int SC_ENUM_PROCESS_INFO = 0;

		// Token: 0x0400013D RID: 317
		public const int SERVICE_QUERY_CONFIG = 1;

		// Token: 0x0400013E RID: 318
		public const int SERVICE_CHANGE_CONFIG = 2;

		// Token: 0x0400013F RID: 319
		public const int SERVICE_QUERY_STATUS = 4;

		// Token: 0x04000140 RID: 320
		public const int SERVICE_ENUMERATE_DEPENDENTS = 8;

		// Token: 0x04000141 RID: 321
		public const int SERVICE_START = 16;

		// Token: 0x04000142 RID: 322
		public const int SERVICE_STOP = 32;

		// Token: 0x04000143 RID: 323
		public const int SERVICE_PAUSE_CONTINUE = 64;

		// Token: 0x04000144 RID: 324
		public const int SERVICE_INTERROGATE = 128;

		// Token: 0x04000145 RID: 325
		public const int SERVICE_USER_DEFINED_CONTROL = 256;

		// Token: 0x04000146 RID: 326
		public const int SERVICE_ALL_ACCESS = 983551;

		// Token: 0x04000147 RID: 327
		public const int SERVICE_TYPE_ADAPTER = 4;

		// Token: 0x04000148 RID: 328
		public const int SERVICE_TYPE_FILE_SYSTEM_DRIVER = 2;

		// Token: 0x04000149 RID: 329
		public const int SERVICE_TYPE_INTERACTIVE_PROCESS = 256;

		// Token: 0x0400014A RID: 330
		public const int SERVICE_TYPE_KERNEL_DRIVER = 1;

		// Token: 0x0400014B RID: 331
		public const int SERVICE_TYPE_RECOGNIZER_DRIVER = 8;

		// Token: 0x0400014C RID: 332
		public const int SERVICE_TYPE_WIN32_OWN_PROCESS = 16;

		// Token: 0x0400014D RID: 333
		public const int SERVICE_TYPE_WIN32_SHARE_PROCESS = 32;

		// Token: 0x0400014E RID: 334
		public const int SERVICE_TYPE_WIN32 = 48;

		// Token: 0x0400014F RID: 335
		public const int SERVICE_TYPE_DRIVER = 11;

		// Token: 0x04000150 RID: 336
		public const int SERVICE_TYPE_ALL = 319;

		// Token: 0x04000151 RID: 337
		public const int START_TYPE_AUTO = 2;

		// Token: 0x04000152 RID: 338
		public const int START_TYPE_BOOT = 0;

		// Token: 0x04000153 RID: 339
		public const int START_TYPE_DEMAND = 3;

		// Token: 0x04000154 RID: 340
		public const int START_TYPE_DISABLED = 4;

		// Token: 0x04000155 RID: 341
		public const int START_TYPE_SYSTEM = 1;

		// Token: 0x04000156 RID: 342
		public const int SERVICE_ACTIVE = 1;

		// Token: 0x04000157 RID: 343
		public const int SERVICE_INACTIVE = 2;

		// Token: 0x04000158 RID: 344
		public const int SERVICE_STATE_ALL = 3;

		// Token: 0x04000159 RID: 345
		public const int STATE_CONTINUE_PENDING = 5;

		// Token: 0x0400015A RID: 346
		public const int STATE_PAUSED = 7;

		// Token: 0x0400015B RID: 347
		public const int STATE_PAUSE_PENDING = 6;

		// Token: 0x0400015C RID: 348
		public const int STATE_RUNNING = 4;

		// Token: 0x0400015D RID: 349
		public const int STATE_START_PENDING = 2;

		// Token: 0x0400015E RID: 350
		public const int STATE_STOPPED = 1;

		// Token: 0x0400015F RID: 351
		public const int STATE_STOP_PENDING = 3;

		// Token: 0x04000160 RID: 352
		public const int STATUS_ACTIVE = 1;

		// Token: 0x04000161 RID: 353
		public const int STATUS_INACTIVE = 2;

		// Token: 0x04000162 RID: 354
		public const int STATUS_ALL = 3;

		// Token: 0x04000163 RID: 355
		public const int POLICY_VIEW_LOCAL_INFORMATION = 1;

		// Token: 0x04000164 RID: 356
		public const int POLICY_VIEW_AUDIT_INFORMATION = 2;

		// Token: 0x04000165 RID: 357
		public const int POLICY_GET_PRIVATE_INFORMATION = 4;

		// Token: 0x04000166 RID: 358
		public const int POLICY_TRUST_ADMIN = 8;

		// Token: 0x04000167 RID: 359
		public const int POLICY_CREATE_ACCOUNT = 16;

		// Token: 0x04000168 RID: 360
		public const int POLICY_CREATE_SECRET = 32;

		// Token: 0x04000169 RID: 361
		public const int POLICY_CREATE_PRIVILEGE = 64;

		// Token: 0x0400016A RID: 362
		public const int POLICY_SET_DEFAULT_QUOTA_LIMITS = 128;

		// Token: 0x0400016B RID: 363
		public const int POLICY_SET_AUDIT_REQUIREMENTS = 256;

		// Token: 0x0400016C RID: 364
		public const int POLICY_AUDIT_LOG_ADMIN = 512;

		// Token: 0x0400016D RID: 365
		public const int POLICY_SERVER_ADMIN = 1024;

		// Token: 0x0400016E RID: 366
		public const int POLICY_LOOKUP_NAMES = 2048;

		// Token: 0x0400016F RID: 367
		public const int POLICY_ALL_ACCESS = 985087;

		// Token: 0x04000170 RID: 368
		public const int STATUS_OBJECT_NAME_NOT_FOUND = -1073741772;

		// Token: 0x04000171 RID: 369
		public const int WTS_CONSOLE_CONNECT = 1;

		// Token: 0x04000172 RID: 370
		public const int WTS_CONSOLE_DISCONNECT = 2;

		// Token: 0x04000173 RID: 371
		public const int WTS_REMOTE_CONNECT = 3;

		// Token: 0x04000174 RID: 372
		public const int WTS_REMOTE_DISCONNECT = 4;

		// Token: 0x04000175 RID: 373
		public const int WTS_SESSION_LOGON = 5;

		// Token: 0x04000176 RID: 374
		public const int WTS_SESSION_LOGOFF = 6;

		// Token: 0x04000177 RID: 375
		public const int WTS_SESSION_LOCK = 7;

		// Token: 0x04000178 RID: 376
		public const int WTS_SESSION_UNLOCK = 8;

		// Token: 0x04000179 RID: 377
		public const int WTS_SESSION_REMOTE_CONTROL = 9;

		// Token: 0x0400017A RID: 378
		public static readonly string DATABASE_ACTIVE = "ServicesActive";

		// Token: 0x0400017B RID: 379
		public static readonly string DATABASE_FAILED = "ServicesFailed";

		// Token: 0x0200000B RID: 11
		internal static class LoadLibraryHelper
		{
			// Token: 0x06000020 RID: 32 RVA: 0x000022B4 File Offset: 0x000012B4
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private static bool IsKnowledgeBase2533623OrGreater()
			{
				bool flag = false;
				IntPtr zero = IntPtr.Zero;
				if (NativeMethods.GetModuleHandleEx(NativeMethods.GetModuleHandleFlags.None, "kernel32.dll", out zero) && zero != IntPtr.Zero)
				{
					try
					{
						flag = NativeMethods.GetProcAddress(zero, "AddDllDirectory") != IntPtr.Zero;
					}
					finally
					{
						NativeMethods.FreeLibrary(zero);
					}
				}
				return flag;
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00002318 File Offset: 0x00001318
			[SecurityCritical]
			internal static IntPtr SecureLoadLibraryEx(string lpFileName, IntPtr hFile, NativeMethods.LoadLibraryFlags dwFlags)
			{
				if (!NativeMethods.LoadLibraryHelper.IsKnowledgeBase2533623OrGreater() && (dwFlags & NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_APPLICATION_DIR & NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS & NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR & NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_SYSTEM32 & NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_USER_DIRS) != NativeMethods.LoadLibraryFlags.None)
				{
					dwFlags &= ~(NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_APPLICATION_DIR | NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR | NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_SYSTEM32 | NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_USER_DIRS);
				}
				return NativeMethods.LoadLibraryEx(lpFileName, hFile, dwFlags);
			}
		}

		// Token: 0x0200000C RID: 12
		[ComVisible(false)]
		public enum StructFormat
		{
			// Token: 0x0400017D RID: 381
			Ansi = 1,
			// Token: 0x0400017E RID: 382
			Unicode,
			// Token: 0x0400017F RID: 383
			Auto
		}

		// Token: 0x0200000D RID: 13
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class ENUM_SERVICE_STATUS
		{
			// Token: 0x04000180 RID: 384
			public string serviceName;

			// Token: 0x04000181 RID: 385
			public string displayName;

			// Token: 0x04000182 RID: 386
			public int serviceType;

			// Token: 0x04000183 RID: 387
			public int currentState;

			// Token: 0x04000184 RID: 388
			public int controlsAccepted;

			// Token: 0x04000185 RID: 389
			public int win32ExitCode;

			// Token: 0x04000186 RID: 390
			public int serviceSpecificExitCode;

			// Token: 0x04000187 RID: 391
			public int checkPoint;

			// Token: 0x04000188 RID: 392
			public int waitHint;
		}

		// Token: 0x0200000E RID: 14
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class ENUM_SERVICE_STATUS_PROCESS
		{
			// Token: 0x04000189 RID: 393
			public string serviceName;

			// Token: 0x0400018A RID: 394
			public string displayName;

			// Token: 0x0400018B RID: 395
			public int serviceType;

			// Token: 0x0400018C RID: 396
			public int currentState;

			// Token: 0x0400018D RID: 397
			public int controlsAccepted;

			// Token: 0x0400018E RID: 398
			public int win32ExitCode;

			// Token: 0x0400018F RID: 399
			public int serviceSpecificExitCode;

			// Token: 0x04000190 RID: 400
			public int checkPoint;

			// Token: 0x04000191 RID: 401
			public int waitHint;

			// Token: 0x04000192 RID: 402
			public int processID;

			// Token: 0x04000193 RID: 403
			public int serviceFlags;
		}

		// Token: 0x0200000F RID: 15
		[Flags]
		public enum LoadLibraryFlags : uint
		{
			// Token: 0x04000195 RID: 405
			None = 0U,
			// Token: 0x04000196 RID: 406
			DONT_RESOLVE_DLL_REFERENCES = 1U,
			// Token: 0x04000197 RID: 407
			LOAD_IGNORE_CODE_AUTHZ_LEVEL = 16U,
			// Token: 0x04000198 RID: 408
			LOAD_LIBRARY_AS_DATAFILE = 2U,
			// Token: 0x04000199 RID: 409
			LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 64U,
			// Token: 0x0400019A RID: 410
			LOAD_LIBRARY_AS_IMAGE_RESOURCE = 32U,
			// Token: 0x0400019B RID: 411
			LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 512U,
			// Token: 0x0400019C RID: 412
			LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096U,
			// Token: 0x0400019D RID: 413
			LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 256U,
			// Token: 0x0400019E RID: 414
			LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048U,
			// Token: 0x0400019F RID: 415
			LOAD_LIBRARY_SEARCH_USER_DIRS = 1024U,
			// Token: 0x040001A0 RID: 416
			LOAD_WITH_ALTERED_SEARCH_PATH = 8U
		}

		// Token: 0x02000010 RID: 16
		[Flags]
		public enum GetModuleHandleFlags : uint
		{
			// Token: 0x040001A2 RID: 418
			None = 0U,
			// Token: 0x040001A3 RID: 419
			GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS = 4U,
			// Token: 0x040001A4 RID: 420
			GET_MODULE_HANDLE_EX_FLAG_PIN = 1U,
			// Token: 0x040001A5 RID: 421
			GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT = 2U
		}

		// Token: 0x02000011 RID: 17
		public struct SERVICE_STATUS
		{
			// Token: 0x040001A6 RID: 422
			public int serviceType;

			// Token: 0x040001A7 RID: 423
			public int currentState;

			// Token: 0x040001A8 RID: 424
			public int controlsAccepted;

			// Token: 0x040001A9 RID: 425
			public int win32ExitCode;

			// Token: 0x040001AA RID: 426
			public int serviceSpecificExitCode;

			// Token: 0x040001AB RID: 427
			public int checkPoint;

			// Token: 0x040001AC RID: 428
			public int waitHint;
		}

		// Token: 0x02000012 RID: 18
		[StructLayout(LayoutKind.Sequential)]
		public class QUERY_SERVICE_CONFIG
		{
			// Token: 0x040001AD RID: 429
			public int dwServiceType;

			// Token: 0x040001AE RID: 430
			public int dwStartType;

			// Token: 0x040001AF RID: 431
			public int dwErrorControl;

			// Token: 0x040001B0 RID: 432
			public unsafe char* lpBinaryPathName;

			// Token: 0x040001B1 RID: 433
			public unsafe char* lpLoadOrderGroup;

			// Token: 0x040001B2 RID: 434
			public int dwTagId;

			// Token: 0x040001B3 RID: 435
			public unsafe char* lpDependencies;

			// Token: 0x040001B4 RID: 436
			public unsafe char* lpServiceStartName;

			// Token: 0x040001B5 RID: 437
			public unsafe char* lpDisplayName;
		}

		// Token: 0x02000013 RID: 19
		[StructLayout(LayoutKind.Sequential)]
		public class SERVICE_TABLE_ENTRY
		{
			// Token: 0x040001B6 RID: 438
			public IntPtr name;

			// Token: 0x040001B7 RID: 439
			public Delegate callback;
		}

		// Token: 0x02000014 RID: 20
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class LSA_UNICODE_STRING
		{
			// Token: 0x040001B8 RID: 440
			public short length;

			// Token: 0x040001B9 RID: 441
			public short maximumLength;

			// Token: 0x040001BA RID: 442
			public string buffer;
		}

		// Token: 0x02000015 RID: 21
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class LSA_UNICODE_STRING_withPointer
		{
			// Token: 0x040001BB RID: 443
			public short length;

			// Token: 0x040001BC RID: 444
			public short maximumLength;

			// Token: 0x040001BD RID: 445
			public IntPtr pwstr = (IntPtr)0;
		}

		// Token: 0x02000016 RID: 22
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class LSA_OBJECT_ATTRIBUTES
		{
			// Token: 0x040001BE RID: 446
			public int length;

			// Token: 0x040001BF RID: 447
			public IntPtr rootDirectory = (IntPtr)0;

			// Token: 0x040001C0 RID: 448
			public IntPtr pointerLsaString = (IntPtr)0;

			// Token: 0x040001C1 RID: 449
			public int attributes;

			// Token: 0x040001C2 RID: 450
			public IntPtr pointerSecurityDescriptor = (IntPtr)0;

			// Token: 0x040001C3 RID: 451
			public IntPtr pointerSecurityQualityOfService = (IntPtr)0;
		}

		// Token: 0x02000017 RID: 23
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SERVICE_DESCRIPTION
		{
			// Token: 0x040001C4 RID: 452
			public IntPtr description;
		}

		// Token: 0x02000018 RID: 24
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SERVICE_FAILURE_ACTIONS
		{
			// Token: 0x040001C5 RID: 453
			public uint dwResetPeriod;

			// Token: 0x040001C6 RID: 454
			public IntPtr rebootMsg;

			// Token: 0x040001C7 RID: 455
			public IntPtr command;

			// Token: 0x040001C8 RID: 456
			public uint numActions;

			// Token: 0x040001C9 RID: 457
			public unsafe NativeMethods.SC_ACTION* actions;
		}

		// Token: 0x02000019 RID: 25
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SC_ACTION
		{
			// Token: 0x040001CA RID: 458
			public int type;

			// Token: 0x040001CB RID: 459
			public uint delay;
		}

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class WTSSESSION_NOTIFICATION
		{
			// Token: 0x040001CC RID: 460
			public int size;

			// Token: 0x040001CD RID: 461
			public int sessionId;
		}

		// Token: 0x0200001B RID: 27
		// (Invoke) Token: 0x0600002B RID: 43
		public delegate void ServiceMainCallback(int argCount, IntPtr argPointer);

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x0600002F RID: 47
		public delegate void ServiceControlCallback(int control);

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x06000033 RID: 51
		public delegate int ServiceControlCallbackEx(int control, int eventType, IntPtr eventData, IntPtr eventContext);
	}
}
