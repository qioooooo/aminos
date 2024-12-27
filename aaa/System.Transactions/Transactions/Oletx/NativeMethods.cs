using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000077 RID: 119
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x0600034D RID: 845
		[DllImport("System.Transactions.Dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int GetNotificationFactory(SafeHandle notificationEventHandle, [MarshalAs(UnmanagedType.Interface)] out IDtcProxyShimFactory ppProxyShimFactory);

		// Token: 0x04000161 RID: 353
		internal static int S_OK = 0;

		// Token: 0x04000162 RID: 354
		internal static int E_FAIL = -2147467259;

		// Token: 0x04000163 RID: 355
		internal static int XACT_S_READONLY = 315394;

		// Token: 0x04000164 RID: 356
		internal static int XACT_S_SINGLEPHASE = 315401;

		// Token: 0x04000165 RID: 357
		internal static int XACT_E_ABORTED = -2147168231;

		// Token: 0x04000166 RID: 358
		internal static int XACT_E_NOTRANSACTION = -2147168242;

		// Token: 0x04000167 RID: 359
		internal static int XACT_E_CONNECTION_DOWN = -2147168228;

		// Token: 0x04000168 RID: 360
		internal static int XACT_E_REENLISTTIMEOUT = -2147168226;

		// Token: 0x04000169 RID: 361
		internal static int XACT_E_RECOVERYALREADYDONE = -2147167996;

		// Token: 0x0400016A RID: 362
		internal static int XACT_E_TMNOTAVAILABLE = -2147168229;

		// Token: 0x0400016B RID: 363
		internal static int XACT_E_INDOUBT = -2147168234;

		// Token: 0x0400016C RID: 364
		internal static int XACT_E_ALREADYINPROGRESS = -2147168232;

		// Token: 0x0400016D RID: 365
		internal static int XACT_E_TOOMANY_ENLISTMENTS = -2147167999;

		// Token: 0x0400016E RID: 366
		internal static int XACT_E_PROTOCOL = -2147167995;

		// Token: 0x0400016F RID: 367
		internal static int XACT_E_FIRST = -2147168256;

		// Token: 0x04000170 RID: 368
		internal static int XACT_E_LAST = -2147168215;

		// Token: 0x04000171 RID: 369
		internal static int XACT_E_NOTSUPPORTED = -2147168241;

		// Token: 0x04000172 RID: 370
		internal static int XACT_E_NETWORK_TX_DISABLED = -2147168220;
	}
}
