using System;

namespace System.Transactions
{
	// Token: 0x02000042 RID: 66
	internal struct EnlistmentTraceIdentifier
	{
		// Token: 0x060001EE RID: 494 RVA: 0x0002E2D0 File Offset: 0x0002D6D0
		public EnlistmentTraceIdentifier(Guid resourceManagerIdentifier, TransactionTraceIdentifier transactionTraceId, int enlistmentIdentifier)
		{
			this.resourceManagerIdentifier = resourceManagerIdentifier;
			this.transactionTraceIdentifier = transactionTraceId;
			this.enlistmentIdentifier = enlistmentIdentifier;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0002E2F4 File Offset: 0x0002D6F4
		public Guid ResourceManagerIdentifier
		{
			get
			{
				return this.resourceManagerIdentifier;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0002E308 File Offset: 0x0002D708
		public TransactionTraceIdentifier TransactionTraceId
		{
			get
			{
				return this.transactionTraceIdentifier;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0002E31C File Offset: 0x0002D71C
		public int EnlistmentIdentifier
		{
			get
			{
				return this.enlistmentIdentifier;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0002E330 File Offset: 0x0002D730
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0002E350 File Offset: 0x0002D750
		public override bool Equals(object objectToCompare)
		{
			if (!(objectToCompare is EnlistmentTraceIdentifier))
			{
				return false;
			}
			EnlistmentTraceIdentifier enlistmentTraceIdentifier = (EnlistmentTraceIdentifier)objectToCompare;
			return !(enlistmentTraceIdentifier.ResourceManagerIdentifier != this.ResourceManagerIdentifier) && !(enlistmentTraceIdentifier.TransactionTraceId != this.TransactionTraceId) && enlistmentTraceIdentifier.EnlistmentIdentifier == this.EnlistmentIdentifier;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0002E3A8 File Offset: 0x0002D7A8
		public static bool operator ==(EnlistmentTraceIdentifier id1, EnlistmentTraceIdentifier id2)
		{
			return id1.Equals(id2);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0002E3C8 File Offset: 0x0002D7C8
		public static bool operator !=(EnlistmentTraceIdentifier id1, EnlistmentTraceIdentifier id2)
		{
			return !id1.Equals(id2);
		}

		// Token: 0x040000F2 RID: 242
		public static readonly EnlistmentTraceIdentifier Empty = default(EnlistmentTraceIdentifier);

		// Token: 0x040000F3 RID: 243
		private Guid resourceManagerIdentifier;

		// Token: 0x040000F4 RID: 244
		private TransactionTraceIdentifier transactionTraceIdentifier;

		// Token: 0x040000F5 RID: 245
		private int enlistmentIdentifier;
	}
}
