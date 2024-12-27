using System;

namespace System.Transactions
{
	// Token: 0x02000006 RID: 6
	public class TransactionEventArgs : EventArgs
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00027638 File Offset: 0x00026A38
		public Transaction Transaction
		{
			get
			{
				return this.transaction;
			}
		}

		// Token: 0x04000079 RID: 121
		internal Transaction transaction;
	}
}
