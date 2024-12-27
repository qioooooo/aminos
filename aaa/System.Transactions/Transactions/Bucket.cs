using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200006D RID: 109
	internal class Bucket
	{
		// Token: 0x0600030B RID: 779 RVA: 0x0003245C File Offset: 0x0003185C
		internal Bucket(BucketSet owningSet)
		{
			this.timedOut = false;
			this.index = -1;
			this.size = 1024;
			this.transactions = new InternalTransaction[this.size];
			this.owningSet = owningSet;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000324A0 File Offset: 0x000318A0
		internal bool Add(InternalTransaction tx)
		{
			int num = Interlocked.Increment(ref this.index);
			if (num < this.size)
			{
				tx.tableBucket = this;
				tx.bucketIndex = num;
				Thread.MemoryBarrier();
				this.transactions[num] = tx;
				if (this.timedOut)
				{
					lock (tx)
					{
						tx.State.Timeout(tx);
						return true;
					}
					goto IL_0056;
				}
				return true;
			}
			IL_0056:
			Bucket bucket = new Bucket(this.owningSet);
			bucket.nextBucketWeak = new WeakReference(this);
			Bucket bucket2 = Interlocked.CompareExchange<Bucket>(ref this.owningSet.headBucket, bucket, this);
			if (bucket2 == this)
			{
				this.previous = bucket;
			}
			return false;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00032558 File Offset: 0x00031958
		internal void Remove(InternalTransaction tx)
		{
			this.transactions[tx.bucketIndex] = null;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00032574 File Offset: 0x00031974
		internal void TimeoutTransactions()
		{
			int num = this.index;
			this.timedOut = true;
			Thread.MemoryBarrier();
			int num2 = 0;
			while (num2 <= num && num2 < this.size)
			{
				InternalTransaction internalTransaction = this.transactions[num2];
				if (internalTransaction != null)
				{
					lock (internalTransaction)
					{
						internalTransaction.State.Timeout(internalTransaction);
					}
				}
				num2++;
			}
		}

		// Token: 0x0400013C RID: 316
		private bool timedOut;

		// Token: 0x0400013D RID: 317
		private int index;

		// Token: 0x0400013E RID: 318
		private int size;

		// Token: 0x0400013F RID: 319
		private InternalTransaction[] transactions;

		// Token: 0x04000140 RID: 320
		internal WeakReference nextBucketWeak;

		// Token: 0x04000141 RID: 321
		private Bucket previous;

		// Token: 0x04000142 RID: 322
		private BucketSet owningSet;
	}
}
