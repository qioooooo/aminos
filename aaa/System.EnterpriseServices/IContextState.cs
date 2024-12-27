using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000003 RID: 3
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3C05E54B-A42A-11D2-AFC4-00C04F8EE1C4")]
	[ComImport]
	internal interface IContextState
	{
		// Token: 0x06000001 RID: 1
		void SetDeactivateOnReturn([MarshalAs(UnmanagedType.Bool)] [In] bool bDeactivate);

		// Token: 0x06000002 RID: 2
		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetDeactivateOnReturn();

		// Token: 0x06000003 RID: 3
		void SetMyTransactionVote([MarshalAs(UnmanagedType.I4)] [In] TransactionVote txVote);

		// Token: 0x06000004 RID: 4
		[return: MarshalAs(UnmanagedType.I4)]
		TransactionVote GetMyTransactionVote();
	}
}
