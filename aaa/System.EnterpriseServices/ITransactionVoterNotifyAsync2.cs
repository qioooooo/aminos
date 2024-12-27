using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices
{
	// Token: 0x02000017 RID: 23
	[Guid("5433376B-414D-11d3-B206-00C04FC2F3EF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface ITransactionVoterNotifyAsync2
	{
		// Token: 0x0600004C RID: 76
		void Committed([MarshalAs(UnmanagedType.Bool)] bool retaining, int newUow, int hr);

		// Token: 0x0600004D RID: 77
		void Aborted(int reason, [MarshalAs(UnmanagedType.Bool)] bool retaining, int newUow, int hr);

		// Token: 0x0600004E RID: 78
		void HeuristicDecision(int decision, int reason, int hr);

		// Token: 0x0600004F RID: 79
		void InDoubt();

		// Token: 0x06000050 RID: 80
		void VoteRequest();
	}
}
