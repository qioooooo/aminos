using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices
{
	// Token: 0x02000016 RID: 22
	[SuppressUnmanagedCodeSecurity]
	[Guid("3A6AD9E2-23B9-11cf-AD60-00AA00A74CCD")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITransactionOutcomeEvents
	{
		// Token: 0x06000048 RID: 72
		void Committed([MarshalAs(UnmanagedType.Bool)] bool retaining, int newUow, int hr);

		// Token: 0x06000049 RID: 73
		void Aborted(int reason, [MarshalAs(UnmanagedType.Bool)] bool retaining, int newUow, int hr);

		// Token: 0x0600004A RID: 74
		void HeuristicDecision(int decision, int reason, int hr);

		// Token: 0x0600004B RID: 75
		void InDoubt();
	}
}
