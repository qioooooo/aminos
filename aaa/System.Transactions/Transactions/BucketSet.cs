using System;

namespace System.Transactions
{
	// Token: 0x0200006C RID: 108
	internal class BucketSet
	{
		// Token: 0x06000307 RID: 775 RVA: 0x000323C4 File Offset: 0x000317C4
		internal BucketSet(TransactionTable table, long absoluteTimeout)
		{
			this.headBucket = new Bucket(this);
			this.table = table;
			this.absoluteTimeout = absoluteTimeout;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000308 RID: 776 RVA: 0x000323F4 File Offset: 0x000317F4
		internal long AbsoluteTimeout
		{
			get
			{
				return this.absoluteTimeout;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00032408 File Offset: 0x00031808
		internal void Add(InternalTransaction newTx)
		{
			while (!this.headBucket.Add(newTx))
			{
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00032424 File Offset: 0x00031824
		internal void TimeoutTransactions()
		{
			Bucket bucket = this.headBucket;
			do
			{
				bucket.TimeoutTransactions();
				WeakReference nextBucketWeak = bucket.nextBucketWeak;
				if (nextBucketWeak != null)
				{
					bucket = (Bucket)nextBucketWeak.Target;
				}
				else
				{
					bucket = null;
				}
			}
			while (bucket != null);
		}

		// Token: 0x04000137 RID: 311
		internal object nextSetWeak;

		// Token: 0x04000138 RID: 312
		internal BucketSet prevSet;

		// Token: 0x04000139 RID: 313
		private TransactionTable table;

		// Token: 0x0400013A RID: 314
		private long absoluteTimeout;

		// Token: 0x0400013B RID: 315
		internal Bucket headBucket;
	}
}
