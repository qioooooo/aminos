using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000114 RID: 276
	internal struct StoreTransactionOperation
	{
		// Token: 0x04000E21 RID: 3617
		[MarshalAs(UnmanagedType.U4)]
		public StoreTransactionOperationType Operation;

		// Token: 0x04000E22 RID: 3618
		public StoreTransactionData Data;
	}
}
