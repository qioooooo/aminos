using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000080 RID: 128
	[Guid("A5FAB903-21CB-49eb-93AE-EF72CD45169E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IVoterBallotShim
	{
		// Token: 0x06000350 RID: 848
		void Vote([MarshalAs(UnmanagedType.Bool)] bool voteYes);
	}
}
