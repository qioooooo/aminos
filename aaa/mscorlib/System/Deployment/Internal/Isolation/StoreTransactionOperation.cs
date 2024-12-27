using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000219 RID: 537
	internal struct StoreTransactionOperation
	{
		// Token: 0x040008AD RID: 2221
		[MarshalAs(UnmanagedType.U4)]
		public StoreTransactionOperationType Operation;

		// Token: 0x040008AE RID: 2222
		public StoreTransactionData Data;
	}
}
