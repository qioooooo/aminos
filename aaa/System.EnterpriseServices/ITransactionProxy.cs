using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x02000014 RID: 20
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("02558374-DF2E-4dae-BD6B-1D5C994F9BDC")]
	[ComImport]
	internal interface ITransactionProxy
	{
		// Token: 0x06000040 RID: 64
		void Commit(Guid guid);

		// Token: 0x06000041 RID: 65
		void Abort();

		// Token: 0x06000042 RID: 66
		[return: MarshalAs(UnmanagedType.Interface)]
		IDtcTransaction Promote();

		// Token: 0x06000043 RID: 67
		void CreateVoter([MarshalAs(UnmanagedType.Interface)] ITransactionVoterNotifyAsync2 voterNotification, [MarshalAs(UnmanagedType.Interface)] out ITransactionVoterBallotAsync2 voterBallot);

		// Token: 0x06000044 RID: 68
		DtcIsolationLevel GetIsolationLevel();

		// Token: 0x06000045 RID: 69
		Guid GetIdentifier();

		// Token: 0x06000046 RID: 70
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsReusable();
	}
}
