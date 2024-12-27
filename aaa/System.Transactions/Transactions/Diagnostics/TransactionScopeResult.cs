using System;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A5 RID: 165
	internal enum TransactionScopeResult
	{
		// Token: 0x04000287 RID: 647
		CreatedTransaction,
		// Token: 0x04000288 RID: 648
		UsingExistingCurrent,
		// Token: 0x04000289 RID: 649
		TransactionPassed,
		// Token: 0x0400028A RID: 650
		DependentTransactionPassed,
		// Token: 0x0400028B RID: 651
		NoTransaction
	}
}
