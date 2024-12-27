using System;
using System.Threading;

namespace System.Transactions.Oletx
{
	// Token: 0x0200008D RID: 141
	internal abstract class OletxBaseEnlistment
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00034BF4 File Offset: 0x00033FF4
		public OletxBaseEnlistment(OletxResourceManager oletxResourceManager, OletxTransaction oletxTransaction)
		{
			Guid empty = Guid.Empty;
			this.enlistmentGuid = Guid.NewGuid();
			this.oletxResourceManager = oletxResourceManager;
			this.oletxTransaction = oletxTransaction;
			if (oletxTransaction != null)
			{
				this.enlistmentId = oletxTransaction.realOletxTransaction.enlistmentCount++;
				this.transactionGuidString = oletxTransaction.realOletxTransaction.TxGuid.ToString();
			}
			else
			{
				this.transactionGuidString = Guid.Empty.ToString();
			}
			this.traceIdentifier = EnlistmentTraceIdentifier.Empty;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00034C8C File Offset: 0x0003408C
		protected EnlistmentTraceIdentifier InternalTraceIdentifier
		{
			get
			{
				if (EnlistmentTraceIdentifier.Empty == this.traceIdentifier)
				{
					lock (this)
					{
						if (EnlistmentTraceIdentifier.Empty == this.traceIdentifier)
						{
							Guid guid = Guid.Empty;
							if (this.oletxResourceManager != null)
							{
								guid = this.oletxResourceManager.resourceManagerIdentifier;
							}
							EnlistmentTraceIdentifier enlistmentTraceIdentifier;
							if (this.oletxTransaction != null)
							{
								enlistmentTraceIdentifier = new EnlistmentTraceIdentifier(guid, this.oletxTransaction.TransactionTraceId, this.enlistmentId);
							}
							else
							{
								TransactionTraceIdentifier transactionTraceIdentifier = new TransactionTraceIdentifier(this.transactionGuidString, 0);
								enlistmentTraceIdentifier = new EnlistmentTraceIdentifier(guid, transactionTraceIdentifier, this.enlistmentId);
							}
							Thread.MemoryBarrier();
							this.traceIdentifier = enlistmentTraceIdentifier;
						}
					}
				}
				return this.traceIdentifier;
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00034D5C File Offset: 0x0003415C
		protected void AddToEnlistmentTable()
		{
			lock (this.oletxResourceManager.enlistmentHashtable.SyncRoot)
			{
				this.oletxResourceManager.enlistmentHashtable.Add(this.enlistmentGuid, this);
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00034DC4 File Offset: 0x000341C4
		protected void RemoveFromEnlistmentTable()
		{
			lock (this.oletxResourceManager.enlistmentHashtable.SyncRoot)
			{
				this.oletxResourceManager.enlistmentHashtable.Remove(this.enlistmentGuid);
			}
		}

		// Token: 0x040001D9 RID: 473
		protected Guid enlistmentGuid;

		// Token: 0x040001DA RID: 474
		protected OletxResourceManager oletxResourceManager;

		// Token: 0x040001DB RID: 475
		protected OletxTransaction oletxTransaction;

		// Token: 0x040001DC RID: 476
		protected string transactionGuidString;

		// Token: 0x040001DD RID: 477
		protected int enlistmentId;

		// Token: 0x040001DE RID: 478
		internal EnlistmentTraceIdentifier traceIdentifier;

		// Token: 0x040001DF RID: 479
		protected InternalEnlistment internalEnlistment;
	}
}
