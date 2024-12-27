using System;
using System.Collections;

namespace System.Transactions
{
	// Token: 0x02000014 RID: 20
	internal sealed class FinalizedObject : IDisposable
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00029460 File Offset: 0x00028860
		internal FinalizedObject(InternalTransaction internalTransaction, Guid identifier)
		{
			this.internalTransaction = internalTransaction;
			this.identifier = identifier;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00029484 File Offset: 0x00028884
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
			Hashtable promotedTransactionTable = TransactionManager.PromotedTransactionTable;
			lock (promotedTransactionTable)
			{
				WeakReference weakReference = (WeakReference)promotedTransactionTable[this.identifier];
				if (weakReference != null && weakReference.Target != null)
				{
					weakReference.Target = null;
				}
				promotedTransactionTable.Remove(this.identifier);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00029508 File Offset: 0x00028908
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0002951C File Offset: 0x0002891C
		~FinalizedObject()
		{
			this.Dispose(false);
		}

		// Token: 0x040000C5 RID: 197
		private Guid identifier;

		// Token: 0x040000C6 RID: 198
		private InternalTransaction internalTransaction;
	}
}
