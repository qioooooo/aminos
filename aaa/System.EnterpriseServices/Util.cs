using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.EnterpriseServices
{
	// Token: 0x02000096 RID: 150
	internal class Util
	{
		// Token: 0x0600038D RID: 909
		[DllImport("oleaut32.dll")]
		internal static extern int LoadTypeLibEx([MarshalAs(UnmanagedType.LPWStr)] [In] string str, int regKind, out IntPtr pptlib);

		// Token: 0x0600038E RID: 910
		[DllImport("user32.dll")]
		internal static extern int MessageBox(int hWnd, string lpText, string lpCaption, int type);

		// Token: 0x0600038F RID: 911
		[DllImport("kernel32.dll")]
		internal static extern void OutputDebugString(string msg);

		// Token: 0x06000390 RID: 912
		[DllImport("ole32.dll", PreserveSig = false)]
		internal static extern void CoGetCallContext([MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out ISecurityCallContext iface);

		// Token: 0x06000391 RID: 913
		[DllImport("oleaut32.dll")]
		internal static extern int RegisterTypeLib(IntPtr pptlib, [MarshalAs(UnmanagedType.LPWStr)] [In] string str, [MarshalAs(UnmanagedType.LPWStr)] [In] string help);

		// Token: 0x06000392 RID: 914
		[DllImport("oleaut32.dll", PreserveSig = false)]
		internal static extern void UnRegisterTypeLib([MarshalAs(UnmanagedType.LPStruct)] [In] Guid libID, short wVerMajor, short wVerMinor, int lcid, global::System.Runtime.InteropServices.ComTypes.SYSKIND syskind);

		// Token: 0x06000393 RID: 915
		[DllImport("oleaut32.dll")]
		internal static extern int LoadRegTypeLib([MarshalAs(UnmanagedType.LPStruct)] [In] Guid lidID, short wVerMajor, short wVerMinor, int lcid, [MarshalAs(UnmanagedType.Interface)] out object pptlib);

		// Token: 0x06000394 RID: 916
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool QueryPerformanceCounter(out long count);

		// Token: 0x06000395 RID: 917
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool QueryPerformanceFrequency(out long count);

		// Token: 0x06000396 RID: 918
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, int arguments);

		// Token: 0x06000397 RID: 919
		[DllImport("mtxex.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int GetObjectContext([MarshalAs(UnmanagedType.Interface)] out IObjectContext pCtx);

		// Token: 0x06000398 RID: 920
		[DllImport("KERNEL32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVersionEx([In] [Out] Util.OSVERSIONINFOEX ver);

		// Token: 0x06000399 RID: 921 RVA: 0x0000BB60 File Offset: 0x0000AB60
		internal static string GetErrorString(int hr)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			int num = Util.FormatMessage(Util.FORMAT_MESSAGE_IGNORE_INSERTS | Util.FORMAT_MESSAGE_FROM_SYSTEM | Util.FORMAT_MESSAGE_ARGUMENT_ARRAY, (IntPtr)0, hr, 0, stringBuilder, stringBuilder.Capacity + 1, 0);
			if (num != 0)
			{
				int i;
				for (i = stringBuilder.Length; i > 0; i--)
				{
					char c = stringBuilder[i - 1];
					if (c > ' ' && c != '.')
					{
						break;
					}
				}
				return stringBuilder.ToString(0, i);
			}
			return null;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000BBD3 File Offset: 0x0000ABD3
		internal static bool ExtendedLifetime
		{
			get
			{
				return (Proxy.GetManagedExts() & 1) != 0;
			}
		}

		// Token: 0x0400016F RID: 367
		internal static readonly int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000170 RID: 368
		internal static readonly int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000171 RID: 369
		internal static readonly int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x04000172 RID: 370
		internal static readonly int CLSCTX_SERVER = 21;

		// Token: 0x04000173 RID: 371
		internal static readonly Guid GUID_NULL = new Guid("00000000-0000-0000-0000-000000000000");

		// Token: 0x04000174 RID: 372
		internal static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x04000175 RID: 373
		internal static readonly Guid IID_IObjectContext = new Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25");

		// Token: 0x04000176 RID: 374
		internal static readonly Guid IID_ISecurityCallContext = new Guid("CAFC823E-B441-11D1-B82B-0000F8757E2A");

		// Token: 0x04000177 RID: 375
		internal static readonly int E_FAIL = -2147467259;

		// Token: 0x04000178 RID: 376
		internal static readonly int E_UNEXPECTED = -2147418113;

		// Token: 0x04000179 RID: 377
		internal static readonly int E_ACCESSDENIED = -2147024891;

		// Token: 0x0400017A RID: 378
		internal static readonly int E_NOINTERFACE = -2147467262;

		// Token: 0x0400017B RID: 379
		internal static readonly int REGDB_E_CLASSNOTREG = -2147221164;

		// Token: 0x0400017C RID: 380
		internal static readonly int COMADMIN_E_OBJECTERRORS = -2146368511;

		// Token: 0x0400017D RID: 381
		internal static readonly int CONTEXT_E_NOCONTEXT = -2147164156;

		// Token: 0x0400017E RID: 382
		internal static readonly int DISP_E_UNKNOWNNAME = -2147352570;

		// Token: 0x0400017F RID: 383
		internal static readonly int CONTEXT_E_ABORTED = -2147164158;

		// Token: 0x04000180 RID: 384
		internal static readonly int CONTEXT_E_ABORTING = -2147164157;

		// Token: 0x04000181 RID: 385
		internal static readonly int XACT_E_INDOUBT = -2147168234;

		// Token: 0x04000182 RID: 386
		internal static readonly int CONTEXT_E_TMNOTAVAILABLE = -2147164145;

		// Token: 0x04000183 RID: 387
		internal static readonly int SECURITY_NULL_SID_AUTHORITY = 0;

		// Token: 0x04000184 RID: 388
		internal static readonly int SECURITY_WORLD_SID_AUTHORITY = 1;

		// Token: 0x04000185 RID: 389
		internal static readonly int SECURITY_LOCAL_SID_AUTHORITY = 2;

		// Token: 0x04000186 RID: 390
		internal static readonly int SECURITY_CREATOR_SID_AUTHORITY = 3;

		// Token: 0x04000187 RID: 391
		internal static readonly int SECURITY_NT_SID_AUTHORITY = 5;

		// Token: 0x04000188 RID: 392
		internal static readonly int ERROR_SUCCESS = 0;

		// Token: 0x04000189 RID: 393
		internal static readonly int ERROR_NO_TOKEN = 1008;

		// Token: 0x0400018A RID: 394
		internal static readonly int MB_ABORTRETRYIGNORE = 2;

		// Token: 0x0400018B RID: 395
		internal static readonly int MB_ICONEXCLAMATION = 48;

		// Token: 0x02000097 RID: 151
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class OSVERSIONINFOEX
		{
			// Token: 0x0600039D RID: 925 RVA: 0x0000BD0D File Offset: 0x0000AD0D
			public OSVERSIONINFOEX()
			{
				this.OSVersionInfoSize = Marshal.SizeOf(this);
			}

			// Token: 0x0400018C RID: 396
			internal int OSVersionInfoSize;

			// Token: 0x0400018D RID: 397
			internal int MajorVersion;

			// Token: 0x0400018E RID: 398
			internal int MinorVersion;

			// Token: 0x0400018F RID: 399
			internal int BuildNumber;

			// Token: 0x04000190 RID: 400
			internal int PlatformId;

			// Token: 0x04000191 RID: 401
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;

			// Token: 0x04000192 RID: 402
			internal short ServicePackMajor;

			// Token: 0x04000193 RID: 403
			internal short ServicePackMinor;

			// Token: 0x04000194 RID: 404
			internal short SuiteMask;

			// Token: 0x04000195 RID: 405
			internal byte ProductType;

			// Token: 0x04000196 RID: 406
			internal byte Reserved;
		}
	}
}
