using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200006B RID: 107
	internal class TransactionTable
	{
		// Token: 0x060002FF RID: 767 RVA: 0x00031F24 File Offset: 0x00031324
		internal TransactionTable()
		{
			this.timer = new Timer(new TimerCallback(this.ThreadTimer), null, -1, this.timerInterval);
			this.timerEnabled = false;
			this.timerInterval = 512;
			this.ticks = 0L;
			this.headBucketSet = new BucketSet(this, long.MaxValue);
			this.rwLock = new CheapUnfairReaderWriterLock();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00031F90 File Offset: 0x00031390
		internal long TimeoutTicks(TimeSpan timeout)
		{
			if (timeout != TimeSpan.Zero)
			{
				return (timeout.Ticks / 10000L >> 9) + this.ticks;
			}
			return long.MaxValue;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00031FD0 File Offset: 0x000313D0
		internal TimeSpan RecalcTimeout(InternalTransaction tx)
		{
			return TimeSpan.FromMilliseconds((double)((tx.AbsoluteTimeout - this.ticks) * (long)this.timerInterval));
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00031FF8 File Offset: 0x000313F8
		private long CurrentTime
		{
			get
			{
				if (this.timerEnabled)
				{
					return this.lastTimerTime;
				}
				return DateTime.UtcNow.Ticks;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00032024 File Offset: 0x00031424
		internal int Add(InternalTransaction txNew)
		{
			Thread.BeginCriticalRegion();
			int num = 0;
			try
			{
				num = this.rwLock.AcquireReaderLock();
				try
				{
					if (txNew.AbsoluteTimeout != 9223372036854775807L && !this.timerEnabled)
					{
						if (!this.timer.Change(this.timerInterval, this.timerInterval))
						{
							throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedTimerFailure"), null);
						}
						this.lastTimerTime = DateTime.UtcNow.Ticks;
						this.timerEnabled = true;
					}
					txNew.CreationTime = this.CurrentTime;
					this.AddIter(txNew);
				}
				finally
				{
					this.rwLock.ReleaseReaderLock();
				}
			}
			finally
			{
				Thread.EndCriticalRegion();
			}
			return num;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0003210C File Offset: 0x0003150C
		private void AddIter(InternalTransaction txNew)
		{
			BucketSet bucketSet = this.headBucketSet;
			while (bucketSet.AbsoluteTimeout != txNew.AbsoluteTimeout)
			{
				BucketSet bucketSet2 = null;
				do
				{
					WeakReference weakReference = (WeakReference)bucketSet.nextSetWeak;
					BucketSet bucketSet3 = null;
					if (weakReference != null)
					{
						bucketSet3 = (BucketSet)weakReference.Target;
					}
					if (bucketSet3 == null)
					{
						BucketSet bucketSet4 = new BucketSet(this, txNew.AbsoluteTimeout);
						WeakReference weakReference2 = new WeakReference(bucketSet4);
						WeakReference weakReference3 = (WeakReference)Interlocked.CompareExchange(ref bucketSet.nextSetWeak, weakReference2, weakReference);
						if (weakReference3 == weakReference)
						{
							bucketSet4.prevSet = bucketSet;
						}
					}
					else
					{
						bucketSet2 = bucketSet;
						bucketSet = bucketSet3;
					}
				}
				while (bucketSet.AbsoluteTimeout > txNew.AbsoluteTimeout);
				if (bucketSet.AbsoluteTimeout != txNew.AbsoluteTimeout)
				{
					BucketSet bucketSet5 = new BucketSet(this, txNew.AbsoluteTimeout);
					WeakReference weakReference4 = new WeakReference(bucketSet5);
					bucketSet5.nextSetWeak = bucketSet2.nextSetWeak;
					WeakReference weakReference5 = (WeakReference)Interlocked.CompareExchange(ref bucketSet2.nextSetWeak, weakReference4, bucketSet5.nextSetWeak);
					if (weakReference5 == bucketSet5.nextSetWeak)
					{
						if (weakReference5 != null)
						{
							BucketSet bucketSet6 = (BucketSet)weakReference5.Target;
							if (bucketSet6 != null)
							{
								bucketSet6.prevSet = bucketSet5;
							}
						}
						bucketSet5.prevSet = bucketSet;
					}
					bucketSet = bucketSet2;
				}
			}
			bucketSet.Add(txNew);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0003222C File Offset: 0x0003162C
		internal void Remove(InternalTransaction tx)
		{
			tx.tableBucket.Remove(tx);
			tx.tableBucket = null;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0003224C File Offset: 0x0003164C
		private void ThreadTimer(object state)
		{
			if (!this.timerEnabled)
			{
				return;
			}
			this.ticks += 1L;
			this.lastTimerTime = DateTime.UtcNow.Ticks;
			BucketSet bucketSet = null;
			BucketSet bucketSet2 = this.headBucketSet;
			WeakReference weakReference = (WeakReference)bucketSet2.nextSetWeak;
			BucketSet bucketSet3 = null;
			if (weakReference != null)
			{
				bucketSet3 = (BucketSet)weakReference.Target;
			}
			if (bucketSet3 == null)
			{
				this.rwLock.AcquireWriterLock();
				try
				{
					if (!this.timer.Change(-1, -1))
					{
						throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedTimerFailure"), null);
					}
					this.timerEnabled = false;
					return;
				}
				finally
				{
					this.rwLock.ReleaseWriterLock();
				}
			}
			for (;;)
			{
				weakReference = (WeakReference)bucketSet2.nextSetWeak;
				if (weakReference == null)
				{
					return;
				}
				bucketSet3 = (BucketSet)weakReference.Target;
				if (bucketSet3 == null)
				{
					break;
				}
				bucketSet = bucketSet2;
				bucketSet2 = bucketSet3;
				if (bucketSet2.AbsoluteTimeout <= this.ticks)
				{
					Thread.BeginCriticalRegion();
					try
					{
						WeakReference weakReference2 = (WeakReference)Interlocked.CompareExchange(ref bucketSet.nextSetWeak, null, weakReference);
						if (weakReference2 == weakReference)
						{
							BucketSet bucketSet4;
							do
							{
								if (weakReference2 != null)
								{
									bucketSet4 = (BucketSet)weakReference2.Target;
								}
								else
								{
									bucketSet4 = null;
								}
								if (bucketSet4 != null)
								{
									bucketSet4.TimeoutTransactions();
									weakReference2 = (WeakReference)bucketSet4.nextSetWeak;
								}
							}
							while (bucketSet4 != null);
							return;
						}
					}
					finally
					{
						Thread.EndCriticalRegion();
					}
					bucketSet2 = bucketSet;
				}
			}
		}

		// Token: 0x0400012E RID: 302
		private const int timerInternalExponent = 9;

		// Token: 0x0400012F RID: 303
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x04000130 RID: 304
		private Timer timer;

		// Token: 0x04000131 RID: 305
		private bool timerEnabled;

		// Token: 0x04000132 RID: 306
		private int timerInterval;

		// Token: 0x04000133 RID: 307
		private long ticks;

		// Token: 0x04000134 RID: 308
		private long lastTimerTime;

		// Token: 0x04000135 RID: 309
		private BucketSet headBucketSet;

		// Token: 0x04000136 RID: 310
		private CheapUnfairReaderWriterLock rwLock;
	}
}
