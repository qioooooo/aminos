using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x02000056 RID: 86
	internal abstract class VolatileDemultiplexer : IEnlistmentNotificationInternal
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0002FADC File Offset: 0x0002EEDC
		public VolatileDemultiplexer(InternalTransaction transaction)
		{
			this.transaction = transaction;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0002FAF8 File Offset: 0x0002EEF8
		internal void BroadcastCommitted(ref VolatileEnlistmentSet volatiles)
		{
			for (int i = 0; i < volatiles.volatileEnlistmentCount; i++)
			{
				volatiles.volatileEnlistments[i].twoPhaseState.InternalCommitted(volatiles.volatileEnlistments[i]);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0002FB30 File Offset: 0x0002EF30
		internal void BroadcastRollback(ref VolatileEnlistmentSet volatiles)
		{
			for (int i = 0; i < volatiles.volatileEnlistmentCount; i++)
			{
				volatiles.volatileEnlistments[i].twoPhaseState.InternalAborted(volatiles.volatileEnlistments[i]);
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0002FB68 File Offset: 0x0002EF68
		internal void BroadcastInDoubt(ref VolatileEnlistmentSet volatiles)
		{
			for (int i = 0; i < volatiles.volatileEnlistmentCount; i++)
			{
				volatiles.volatileEnlistments[i].twoPhaseState.InternalIndoubt(volatiles.volatileEnlistments[i]);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0002FBA0 File Offset: 0x0002EFA0
		internal static object ClassSyncObject
		{
			get
			{
				if (VolatileDemultiplexer.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref VolatileDemultiplexer.classSyncObject, obj, null);
				}
				return VolatileDemultiplexer.classSyncObject;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0002FBCC File Offset: 0x0002EFCC
		private static WaitCallback PrepareCallback
		{
			get
			{
				if (VolatileDemultiplexer.prepareCallback == null)
				{
					lock (VolatileDemultiplexer.ClassSyncObject)
					{
						if (VolatileDemultiplexer.prepareCallback == null)
						{
							WaitCallback waitCallback = new WaitCallback(VolatileDemultiplexer.PoolablePrepare);
							Thread.MemoryBarrier();
							VolatileDemultiplexer.prepareCallback = waitCallback;
						}
					}
				}
				return VolatileDemultiplexer.prepareCallback;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0002FC38 File Offset: 0x0002F038
		protected static void PoolablePrepare(object state)
		{
			VolatileDemultiplexer volatileDemultiplexer = (VolatileDemultiplexer)state;
			if (Monitor.TryEnter(volatileDemultiplexer.transaction, 250))
			{
				try
				{
					volatileDemultiplexer.InternalPrepare();
					return;
				}
				finally
				{
					Monitor.Exit(volatileDemultiplexer.transaction);
				}
			}
			if (!ThreadPool.QueueUserWorkItem(VolatileDemultiplexer.PrepareCallback, volatileDemultiplexer))
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedFailureOfThreadPool"), null);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0002FCB8 File Offset: 0x0002F0B8
		private static WaitCallback CommitCallback
		{
			get
			{
				if (VolatileDemultiplexer.commitCallback == null)
				{
					lock (VolatileDemultiplexer.ClassSyncObject)
					{
						if (VolatileDemultiplexer.commitCallback == null)
						{
							WaitCallback waitCallback = new WaitCallback(VolatileDemultiplexer.PoolableCommit);
							Thread.MemoryBarrier();
							VolatileDemultiplexer.commitCallback = waitCallback;
						}
					}
				}
				return VolatileDemultiplexer.commitCallback;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0002FD24 File Offset: 0x0002F124
		protected static void PoolableCommit(object state)
		{
			VolatileDemultiplexer volatileDemultiplexer = (VolatileDemultiplexer)state;
			if (Monitor.TryEnter(volatileDemultiplexer.transaction, 250))
			{
				try
				{
					volatileDemultiplexer.InternalCommit();
					return;
				}
				finally
				{
					Monitor.Exit(volatileDemultiplexer.transaction);
				}
			}
			if (!ThreadPool.QueueUserWorkItem(VolatileDemultiplexer.CommitCallback, volatileDemultiplexer))
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedFailureOfThreadPool"), null);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0002FDA4 File Offset: 0x0002F1A4
		private static WaitCallback RollbackCallback
		{
			get
			{
				if (VolatileDemultiplexer.rollbackCallback == null)
				{
					lock (VolatileDemultiplexer.ClassSyncObject)
					{
						if (VolatileDemultiplexer.rollbackCallback == null)
						{
							WaitCallback waitCallback = new WaitCallback(VolatileDemultiplexer.PoolableRollback);
							Thread.MemoryBarrier();
							VolatileDemultiplexer.rollbackCallback = waitCallback;
						}
					}
				}
				return VolatileDemultiplexer.rollbackCallback;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0002FE10 File Offset: 0x0002F210
		protected static void PoolableRollback(object state)
		{
			VolatileDemultiplexer volatileDemultiplexer = (VolatileDemultiplexer)state;
			if (Monitor.TryEnter(volatileDemultiplexer.transaction, 250))
			{
				try
				{
					volatileDemultiplexer.InternalRollback();
					return;
				}
				finally
				{
					Monitor.Exit(volatileDemultiplexer.transaction);
				}
			}
			if (!ThreadPool.QueueUserWorkItem(VolatileDemultiplexer.RollbackCallback, volatileDemultiplexer))
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedFailureOfThreadPool"), null);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0002FE90 File Offset: 0x0002F290
		private static WaitCallback InDoubtCallback
		{
			get
			{
				if (VolatileDemultiplexer.inDoubtCallback == null)
				{
					lock (VolatileDemultiplexer.ClassSyncObject)
					{
						if (VolatileDemultiplexer.inDoubtCallback == null)
						{
							WaitCallback waitCallback = new WaitCallback(VolatileDemultiplexer.PoolableInDoubt);
							Thread.MemoryBarrier();
							VolatileDemultiplexer.inDoubtCallback = waitCallback;
						}
					}
				}
				return VolatileDemultiplexer.inDoubtCallback;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0002FEFC File Offset: 0x0002F2FC
		protected static void PoolableInDoubt(object state)
		{
			VolatileDemultiplexer volatileDemultiplexer = (VolatileDemultiplexer)state;
			if (Monitor.TryEnter(volatileDemultiplexer.transaction, 250))
			{
				try
				{
					volatileDemultiplexer.InternalInDoubt();
					return;
				}
				finally
				{
					Monitor.Exit(volatileDemultiplexer.transaction);
				}
			}
			if (!ThreadPool.QueueUserWorkItem(VolatileDemultiplexer.InDoubtCallback, volatileDemultiplexer))
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedFailureOfThreadPool"), null);
			}
		}

		// Token: 0x06000282 RID: 642
		protected abstract void InternalPrepare();

		// Token: 0x06000283 RID: 643
		protected abstract void InternalCommit();

		// Token: 0x06000284 RID: 644
		protected abstract void InternalRollback();

		// Token: 0x06000285 RID: 645
		protected abstract void InternalInDoubt();

		// Token: 0x06000286 RID: 646
		public abstract void Prepare(IPromotedEnlistment en);

		// Token: 0x06000287 RID: 647
		public abstract void Commit(IPromotedEnlistment en);

		// Token: 0x06000288 RID: 648
		public abstract void Rollback(IPromotedEnlistment en);

		// Token: 0x06000289 RID: 649
		public abstract void InDoubt(IPromotedEnlistment en);

		// Token: 0x04000109 RID: 265
		protected InternalTransaction transaction;

		// Token: 0x0400010A RID: 266
		internal IPromotedEnlistment oletxEnlistment;

		// Token: 0x0400010B RID: 267
		internal IPromotedEnlistment preparingEnlistment;

		// Token: 0x0400010C RID: 268
		private static object classSyncObject;

		// Token: 0x0400010D RID: 269
		private static WaitCallback prepareCallback;

		// Token: 0x0400010E RID: 270
		private static WaitCallback commitCallback;

		// Token: 0x0400010F RID: 271
		private static WaitCallback rollbackCallback;

		// Token: 0x04000110 RID: 272
		private static WaitCallback inDoubtCallback;
	}
}
