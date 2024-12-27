using System;
using System.Globalization;
using System.Threading;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000013 RID: 19
	internal class InternalTransaction : IDisposable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00028FD0 File Offset: 0x000283D0
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00028FE4 File Offset: 0x000283E4
		internal TransactionState State
		{
			get
			{
				return this.transactionState;
			}
			set
			{
				this.transactionState = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00028FF8 File Offset: 0x000283F8
		internal int TransactionHash
		{
			get
			{
				return this.transactionHash;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0002900C File Offset: 0x0002840C
		internal long AbsoluteTimeout
		{
			get
			{
				return this.absoluteTimeout;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00029020 File Offset: 0x00028420
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00029034 File Offset: 0x00028434
		internal long CreationTime
		{
			get
			{
				return this.creationTime;
			}
			set
			{
				this.creationTime = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00029048 File Offset: 0x00028448
		// (set) Token: 0x06000058 RID: 88 RVA: 0x0002905C File Offset: 0x0002845C
		internal global::System.Transactions.Oletx.OletxTransaction PromotedTransaction
		{
			get
			{
				return this.promotedTransaction;
			}
			set
			{
				this.promotedTransaction = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00029070 File Offset: 0x00028470
		internal static object ClassSyncObject
		{
			get
			{
				if (InternalTransaction.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref InternalTransaction.classSyncObject, obj, null);
				}
				return InternalTransaction.classSyncObject;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0002909C File Offset: 0x0002849C
		internal static string InstanceIdentifier
		{
			get
			{
				if (InternalTransaction.instanceIdentifier == null)
				{
					lock (InternalTransaction.ClassSyncObject)
					{
						if (InternalTransaction.instanceIdentifier == null)
						{
							string text = Guid.NewGuid().ToString() + ":";
							Thread.MemoryBarrier();
							InternalTransaction.instanceIdentifier = text;
						}
					}
				}
				return InternalTransaction.instanceIdentifier;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00029118 File Offset: 0x00028518
		internal TransactionTraceIdentifier TransactionTraceId
		{
			get
			{
				if (this.traceIdentifier == TransactionTraceIdentifier.Empty)
				{
					lock (this)
					{
						if (this.traceIdentifier == TransactionTraceIdentifier.Empty)
						{
							TransactionTraceIdentifier transactionTraceIdentifier = new TransactionTraceIdentifier(InternalTransaction.InstanceIdentifier + Convert.ToString(this.transactionHash, CultureInfo.InvariantCulture), 0);
							Thread.MemoryBarrier();
							this.traceIdentifier = transactionTraceIdentifier;
						}
					}
				}
				return this.traceIdentifier;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000291AC File Offset: 0x000285AC
		internal InternalTransaction(TimeSpan timeout, CommittableTransaction committableTransaction)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			this.absoluteTimeout = TransactionManager.TransactionTable.TimeoutTicks(timeout);
			TransactionState._TransactionStateActive.EnterState(this);
			this.promoteState = TransactionState._TransactionStatePromoted;
			this.committableTransaction = committableTransaction;
			this.outcomeSource = committableTransaction;
			this.transactionHash = TransactionManager.TransactionTable.Add(this);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00029214 File Offset: 0x00028614
		internal InternalTransaction(Transaction outcomeSource, global::System.Transactions.Oletx.OletxTransaction distributedTx)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			this.promotedTransaction = distributedTx;
			this.absoluteTimeout = long.MaxValue;
			this.outcomeSource = outcomeSource;
			this.transactionHash = TransactionManager.TransactionTable.Add(this);
			TransactionState._TransactionStateNonCommittablePromoted.EnterState(this);
			this.promoteState = TransactionState._TransactionStateNonCommittablePromoted;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00029278 File Offset: 0x00028678
		internal InternalTransaction(Transaction outcomeSource, ITransactionPromoter promoter)
		{
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			this.absoluteTimeout = long.MaxValue;
			this.outcomeSource = outcomeSource;
			this.transactionHash = TransactionManager.TransactionTable.Add(this);
			this.promoter = promoter;
			TransactionState._TransactionStateSubordinateActive.EnterState(this);
			this.promoteState = TransactionState._TransactionStateDelegatedSubordinate;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000292DC File Offset: 0x000286DC
		internal static void DistributedTransactionOutcome(InternalTransaction tx, TransactionStatus status)
		{
			FinalizedObject finalizedObject = null;
			lock (tx)
			{
				if (tx.innerException == null)
				{
					tx.innerException = tx.PromotedTransaction.InnerException;
				}
				switch (status)
				{
				case TransactionStatus.Committed:
					tx.State.ChangeStatePromotedCommitted(tx);
					break;
				case TransactionStatus.Aborted:
					tx.State.ChangeStatePromotedAborted(tx);
					break;
				case TransactionStatus.InDoubt:
					tx.State.InDoubtFromDtc(tx);
					break;
				default:
					TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), "", null);
					break;
				}
				finalizedObject = tx.finalizedObject;
			}
			if (finalizedObject != null)
			{
				finalizedObject.Dispose();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0002939C File Offset: 0x0002879C
		internal void SignalAsyncCompletion()
		{
			if (this.asyncResultEvent != null)
			{
				this.asyncResultEvent.Set();
			}
			if (this.asyncCallback != null)
			{
				Monitor.Exit(this);
				try
				{
					this.asyncCallback(this.committableTransaction);
				}
				finally
				{
					Monitor.Enter(this);
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00029404 File Offset: 0x00028804
		internal void FireCompletion()
		{
			TransactionCompletedEventHandler transactionCompletedEventHandler = this.transactionCompletedDelegate;
			if (transactionCompletedEventHandler != null)
			{
				TransactionEventArgs transactionEventArgs = new TransactionEventArgs();
				transactionEventArgs.transaction = this.outcomeSource.InternalClone();
				transactionCompletedEventHandler(transactionEventArgs.transaction, transactionEventArgs);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00029440 File Offset: 0x00028840
		public void Dispose()
		{
			if (this.promotedTransaction != null)
			{
				this.promotedTransaction.Dispose();
			}
		}

		// Token: 0x040000A2 RID: 162
		internal const int volatileArrayIncrement = 8;

		// Token: 0x040000A3 RID: 163
		protected TransactionState transactionState;

		// Token: 0x040000A4 RID: 164
		internal TransactionState promoteState;

		// Token: 0x040000A5 RID: 165
		internal FinalizedObject finalizedObject;

		// Token: 0x040000A6 RID: 166
		internal int transactionHash;

		// Token: 0x040000A7 RID: 167
		internal static int nextHash;

		// Token: 0x040000A8 RID: 168
		private long absoluteTimeout;

		// Token: 0x040000A9 RID: 169
		private long creationTime;

		// Token: 0x040000AA RID: 170
		internal InternalEnlistment durableEnlistment;

		// Token: 0x040000AB RID: 171
		internal VolatileEnlistmentSet phase0Volatiles;

		// Token: 0x040000AC RID: 172
		internal VolatileEnlistmentSet phase1Volatiles;

		// Token: 0x040000AD RID: 173
		internal int phase0VolatileWaveCount;

		// Token: 0x040000AE RID: 174
		internal global::System.Transactions.Oletx.OletxDependentTransaction phase0WaveDependentClone;

		// Token: 0x040000AF RID: 175
		internal int phase0WaveDependentCloneCount;

		// Token: 0x040000B0 RID: 176
		internal global::System.Transactions.Oletx.OletxDependentTransaction abortingDependentClone;

		// Token: 0x040000B1 RID: 177
		internal int abortingDependentCloneCount;

		// Token: 0x040000B2 RID: 178
		internal Bucket tableBucket;

		// Token: 0x040000B3 RID: 179
		internal int bucketIndex;

		// Token: 0x040000B4 RID: 180
		internal TransactionCompletedEventHandler transactionCompletedDelegate;

		// Token: 0x040000B5 RID: 181
		private global::System.Transactions.Oletx.OletxTransaction promotedTransaction;

		// Token: 0x040000B6 RID: 182
		internal Exception innerException;

		// Token: 0x040000B7 RID: 183
		internal int cloneCount;

		// Token: 0x040000B8 RID: 184
		internal int enlistmentCount;

		// Token: 0x040000B9 RID: 185
		internal ManualResetEvent asyncResultEvent;

		// Token: 0x040000BA RID: 186
		internal bool asyncCommit;

		// Token: 0x040000BB RID: 187
		internal AsyncCallback asyncCallback;

		// Token: 0x040000BC RID: 188
		internal object asyncState;

		// Token: 0x040000BD RID: 189
		internal bool needPulse;

		// Token: 0x040000BE RID: 190
		internal TransactionInformation transactionInformation;

		// Token: 0x040000BF RID: 191
		internal CommittableTransaction committableTransaction;

		// Token: 0x040000C0 RID: 192
		internal Transaction outcomeSource;

		// Token: 0x040000C1 RID: 193
		private static object classSyncObject;

		// Token: 0x040000C2 RID: 194
		private static string instanceIdentifier;

		// Token: 0x040000C3 RID: 195
		private TransactionTraceIdentifier traceIdentifier;

		// Token: 0x040000C4 RID: 196
		internal ITransactionPromoter promoter;
	}
}
