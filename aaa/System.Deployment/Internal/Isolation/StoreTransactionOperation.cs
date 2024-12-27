using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200013F RID: 319
	internal struct StoreTransactionOperation
	{
		// Token: 0x04000595 RID: 1429
		[MarshalAs(UnmanagedType.U4)]
		public StoreTransactionOperationType Operation;

		// Token: 0x04000596 RID: 1430
		public StoreTransactionData Data;
	}
}
