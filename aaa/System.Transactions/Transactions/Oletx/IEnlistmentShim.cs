using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000082 RID: 130
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5EC35E09-B285-422c-83F5-1372384A42CC")]
	[ComImport]
	internal interface IEnlistmentShim
	{
		// Token: 0x06000353 RID: 851
		void PrepareRequestDone(OletxPrepareVoteType voteType);

		// Token: 0x06000354 RID: 852
		void CommitRequestDone();

		// Token: 0x06000355 RID: 853
		void AbortRequestDone();
	}
}
