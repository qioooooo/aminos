using System;

namespace System.Transactions
{
	// Token: 0x02000074 RID: 116
	internal struct TransactionTraceIdentifier
	{
		// Token: 0x0600033D RID: 829 RVA: 0x00033BB0 File Offset: 0x00032FB0
		public TransactionTraceIdentifier(string transactionIdentifier, int cloneIdentifier)
		{
			this.transactionIdentifier = transactionIdentifier;
			this.cloneIdentifier = cloneIdentifier;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00033BCC File Offset: 0x00032FCC
		public string TransactionIdentifier
		{
			get
			{
				return this.transactionIdentifier;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00033BE0 File Offset: 0x00032FE0
		public int CloneIdentifier
		{
			get
			{
				return this.cloneIdentifier;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00033BF4 File Offset: 0x00032FF4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00033C14 File Offset: 0x00033014
		public override bool Equals(object objectToCompare)
		{
			if (!(objectToCompare is TransactionTraceIdentifier))
			{
				return false;
			}
			TransactionTraceIdentifier transactionTraceIdentifier = (TransactionTraceIdentifier)objectToCompare;
			return !(transactionTraceIdentifier.TransactionIdentifier != this.TransactionIdentifier) && transactionTraceIdentifier.CloneIdentifier == this.CloneIdentifier;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00033C58 File Offset: 0x00033058
		public static bool operator ==(TransactionTraceIdentifier id1, TransactionTraceIdentifier id2)
		{
			return id1.Equals(id2);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00033C78 File Offset: 0x00033078
		public static bool operator !=(TransactionTraceIdentifier id1, TransactionTraceIdentifier id2)
		{
			return !id1.Equals(id2);
		}

		// Token: 0x0400015E RID: 350
		public static readonly TransactionTraceIdentifier Empty = default(TransactionTraceIdentifier);

		// Token: 0x0400015F RID: 351
		private string transactionIdentifier;

		// Token: 0x04000160 RID: 352
		private int cloneIdentifier;
	}
}
