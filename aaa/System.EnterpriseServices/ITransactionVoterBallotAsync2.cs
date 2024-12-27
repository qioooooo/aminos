using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices
{
	// Token: 0x02000015 RID: 21
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5433376C-414D-11d3-B206-00C04FC2F3EF")]
	[ComImport]
	internal interface ITransactionVoterBallotAsync2
	{
		// Token: 0x06000047 RID: 71
		void VoteRequestDone(int hr, int reason);
	}
}
