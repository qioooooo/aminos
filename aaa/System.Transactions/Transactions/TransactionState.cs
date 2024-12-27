using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000015 RID: 21
	internal abstract class TransactionState
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00029558 File Offset: 0x00028958
		internal static TransactionStateActive _TransactionStateActive
		{
			get
			{
				if (TransactionState._transactionStateActive == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateActive == null)
						{
							TransactionStateActive transactionStateActive = new TransactionStateActive();
							Thread.MemoryBarrier();
							TransactionState._transactionStateActive = transactionStateActive;
						}
					}
				}
				return TransactionState._transactionStateActive;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000295BC File Offset: 0x000289BC
		internal static TransactionStateSubordinateActive _TransactionStateSubordinateActive
		{
			get
			{
				if (TransactionState._transactionStateSubordinateActive == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateSubordinateActive == null)
						{
							TransactionStateSubordinateActive transactionStateSubordinateActive = new TransactionStateSubordinateActive();
							Thread.MemoryBarrier();
							TransactionState._transactionStateSubordinateActive = transactionStateSubordinateActive;
						}
					}
				}
				return TransactionState._transactionStateSubordinateActive;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00029620 File Offset: 0x00028A20
		internal static TransactionStatePSPEOperation _TransactionStatePSPEOperation
		{
			get
			{
				if (TransactionState._transactionStatePSPEOperation == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePSPEOperation == null)
						{
							TransactionStatePSPEOperation transactionStatePSPEOperation = new TransactionStatePSPEOperation();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePSPEOperation = transactionStatePSPEOperation;
						}
					}
				}
				return TransactionState._transactionStatePSPEOperation;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00029684 File Offset: 0x00028A84
		protected static TransactionStatePhase0 _TransactionStatePhase0
		{
			get
			{
				if (TransactionState._transactionStatePhase0 == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePhase0 == null)
						{
							TransactionStatePhase0 transactionStatePhase = new TransactionStatePhase0();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePhase0 = transactionStatePhase;
						}
					}
				}
				return TransactionState._transactionStatePhase0;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000296E8 File Offset: 0x00028AE8
		protected static TransactionStateVolatilePhase1 _TransactionStateVolatilePhase1
		{
			get
			{
				if (TransactionState._transactionStateVolatilePhase1 == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateVolatilePhase1 == null)
						{
							TransactionStateVolatilePhase1 transactionStateVolatilePhase = new TransactionStateVolatilePhase1();
							Thread.MemoryBarrier();
							TransactionState._transactionStateVolatilePhase1 = transactionStateVolatilePhase;
						}
					}
				}
				return TransactionState._transactionStateVolatilePhase1;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0002974C File Offset: 0x00028B4C
		protected static TransactionStateVolatileSPC _TransactionStateVolatileSPC
		{
			get
			{
				if (TransactionState._transactionStateVolatileSPC == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateVolatileSPC == null)
						{
							TransactionStateVolatileSPC transactionStateVolatileSPC = new TransactionStateVolatileSPC();
							Thread.MemoryBarrier();
							TransactionState._transactionStateVolatileSPC = transactionStateVolatileSPC;
						}
					}
				}
				return TransactionState._transactionStateVolatileSPC;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000297B0 File Offset: 0x00028BB0
		protected static TransactionStateSPC _TransactionStateSPC
		{
			get
			{
				if (TransactionState._transactionStateSPC == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateSPC == null)
						{
							TransactionStateSPC transactionStateSPC = new TransactionStateSPC();
							Thread.MemoryBarrier();
							TransactionState._transactionStateSPC = transactionStateSPC;
						}
					}
				}
				return TransactionState._transactionStateSPC;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00029814 File Offset: 0x00028C14
		protected static TransactionStateAborted _TransactionStateAborted
		{
			get
			{
				if (TransactionState._transactionStateAborted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateAborted == null)
						{
							TransactionStateAborted transactionStateAborted = new TransactionStateAborted();
							Thread.MemoryBarrier();
							TransactionState._transactionStateAborted = transactionStateAborted;
						}
					}
				}
				return TransactionState._transactionStateAborted;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00029878 File Offset: 0x00028C78
		protected static TransactionStateCommitted _TransactionStateCommitted
		{
			get
			{
				if (TransactionState._transactionStateCommitted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateCommitted == null)
						{
							TransactionStateCommitted transactionStateCommitted = new TransactionStateCommitted();
							Thread.MemoryBarrier();
							TransactionState._transactionStateCommitted = transactionStateCommitted;
						}
					}
				}
				return TransactionState._transactionStateCommitted;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000298DC File Offset: 0x00028CDC
		protected static TransactionStateInDoubt _TransactionStateInDoubt
		{
			get
			{
				if (TransactionState._transactionStateInDoubt == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateInDoubt == null)
						{
							TransactionStateInDoubt transactionStateInDoubt = new TransactionStateInDoubt();
							Thread.MemoryBarrier();
							TransactionState._transactionStateInDoubt = transactionStateInDoubt;
						}
					}
				}
				return TransactionState._transactionStateInDoubt;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00029940 File Offset: 0x00028D40
		internal static TransactionStatePromoted _TransactionStatePromoted
		{
			get
			{
				if (TransactionState._transactionStatePromoted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromoted == null)
						{
							TransactionStatePromoted transactionStatePromoted = new TransactionStatePromoted();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromoted = transactionStatePromoted;
						}
					}
				}
				return TransactionState._transactionStatePromoted;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000299A4 File Offset: 0x00028DA4
		internal static TransactionStateNonCommittablePromoted _TransactionStateNonCommittablePromoted
		{
			get
			{
				if (TransactionState._transactionStateNonCommittablePromoted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateNonCommittablePromoted == null)
						{
							TransactionStateNonCommittablePromoted transactionStateNonCommittablePromoted = new TransactionStateNonCommittablePromoted();
							Thread.MemoryBarrier();
							TransactionState._transactionStateNonCommittablePromoted = transactionStateNonCommittablePromoted;
						}
					}
				}
				return TransactionState._transactionStateNonCommittablePromoted;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00029A08 File Offset: 0x00028E08
		protected static TransactionStatePromotedP0Wave _TransactionStatePromotedP0Wave
		{
			get
			{
				if (TransactionState._transactionStatePromotedP0Wave == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedP0Wave == null)
						{
							TransactionStatePromotedP0Wave transactionStatePromotedP0Wave = new TransactionStatePromotedP0Wave();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedP0Wave = transactionStatePromotedP0Wave;
						}
					}
				}
				return TransactionState._transactionStatePromotedP0Wave;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00029A6C File Offset: 0x00028E6C
		protected static TransactionStatePromotedCommitting _TransactionStatePromotedCommitting
		{
			get
			{
				if (TransactionState._transactionStatePromotedCommitting == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedCommitting == null)
						{
							TransactionStatePromotedCommitting transactionStatePromotedCommitting = new TransactionStatePromotedCommitting();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedCommitting = transactionStatePromotedCommitting;
						}
					}
				}
				return TransactionState._transactionStatePromotedCommitting;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00029AD0 File Offset: 0x00028ED0
		protected static TransactionStatePromotedPhase0 _TransactionStatePromotedPhase0
		{
			get
			{
				if (TransactionState._transactionStatePromotedPhase0 == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedPhase0 == null)
						{
							TransactionStatePromotedPhase0 transactionStatePromotedPhase = new TransactionStatePromotedPhase0();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedPhase0 = transactionStatePromotedPhase;
						}
					}
				}
				return TransactionState._transactionStatePromotedPhase0;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00029B34 File Offset: 0x00028F34
		protected static TransactionStatePromotedPhase1 _TransactionStatePromotedPhase1
		{
			get
			{
				if (TransactionState._transactionStatePromotedPhase1 == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedPhase1 == null)
						{
							TransactionStatePromotedPhase1 transactionStatePromotedPhase = new TransactionStatePromotedPhase1();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedPhase1 = transactionStatePromotedPhase;
						}
					}
				}
				return TransactionState._transactionStatePromotedPhase1;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00029B98 File Offset: 0x00028F98
		protected static TransactionStatePromotedP0Aborting _TransactionStatePromotedP0Aborting
		{
			get
			{
				if (TransactionState._transactionStatePromotedP0Aborting == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedP0Aborting == null)
						{
							TransactionStatePromotedP0Aborting transactionStatePromotedP0Aborting = new TransactionStatePromotedP0Aborting();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedP0Aborting = transactionStatePromotedP0Aborting;
						}
					}
				}
				return TransactionState._transactionStatePromotedP0Aborting;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00029BFC File Offset: 0x00028FFC
		protected static TransactionStatePromotedP1Aborting _TransactionStatePromotedP1Aborting
		{
			get
			{
				if (TransactionState._transactionStatePromotedP1Aborting == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedP1Aborting == null)
						{
							TransactionStatePromotedP1Aborting transactionStatePromotedP1Aborting = new TransactionStatePromotedP1Aborting();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedP1Aborting = transactionStatePromotedP1Aborting;
						}
					}
				}
				return TransactionState._transactionStatePromotedP1Aborting;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00029C60 File Offset: 0x00029060
		protected static TransactionStatePromotedAborted _TransactionStatePromotedAborted
		{
			get
			{
				if (TransactionState._transactionStatePromotedAborted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedAborted == null)
						{
							TransactionStatePromotedAborted transactionStatePromotedAborted = new TransactionStatePromotedAborted();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedAborted = transactionStatePromotedAborted;
						}
					}
				}
				return TransactionState._transactionStatePromotedAborted;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00029CC4 File Offset: 0x000290C4
		protected static TransactionStatePromotedCommitted _TransactionStatePromotedCommitted
		{
			get
			{
				if (TransactionState._transactionStatePromotedCommitted == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedCommitted == null)
						{
							TransactionStatePromotedCommitted transactionStatePromotedCommitted = new TransactionStatePromotedCommitted();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedCommitted = transactionStatePromotedCommitted;
						}
					}
				}
				return TransactionState._transactionStatePromotedCommitted;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00029D28 File Offset: 0x00029128
		protected static TransactionStatePromotedIndoubt _TransactionStatePromotedIndoubt
		{
			get
			{
				if (TransactionState._transactionStatePromotedIndoubt == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStatePromotedIndoubt == null)
						{
							TransactionStatePromotedIndoubt transactionStatePromotedIndoubt = new TransactionStatePromotedIndoubt();
							Thread.MemoryBarrier();
							TransactionState._transactionStatePromotedIndoubt = transactionStatePromotedIndoubt;
						}
					}
				}
				return TransactionState._transactionStatePromotedIndoubt;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00029D8C File Offset: 0x0002918C
		protected static TransactionStateDelegated _TransactionStateDelegated
		{
			get
			{
				if (TransactionState._transactionStateDelegated == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateDelegated == null)
						{
							TransactionStateDelegated transactionStateDelegated = new TransactionStateDelegated();
							Thread.MemoryBarrier();
							TransactionState._transactionStateDelegated = transactionStateDelegated;
						}
					}
				}
				return TransactionState._transactionStateDelegated;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00029DF0 File Offset: 0x000291F0
		internal static TransactionStateDelegatedSubordinate _TransactionStateDelegatedSubordinate
		{
			get
			{
				if (TransactionState._transactionStateDelegatedSubordinate == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateDelegatedSubordinate == null)
						{
							TransactionStateDelegatedSubordinate transactionStateDelegatedSubordinate = new TransactionStateDelegatedSubordinate();
							Thread.MemoryBarrier();
							TransactionState._transactionStateDelegatedSubordinate = transactionStateDelegatedSubordinate;
						}
					}
				}
				return TransactionState._transactionStateDelegatedSubordinate;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00029E54 File Offset: 0x00029254
		protected static TransactionStateDelegatedP0Wave _TransactionStateDelegatedP0Wave
		{
			get
			{
				if (TransactionState._transactionStateDelegatedP0Wave == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateDelegatedP0Wave == null)
						{
							TransactionStateDelegatedP0Wave transactionStateDelegatedP0Wave = new TransactionStateDelegatedP0Wave();
							Thread.MemoryBarrier();
							TransactionState._transactionStateDelegatedP0Wave = transactionStateDelegatedP0Wave;
						}
					}
				}
				return TransactionState._transactionStateDelegatedP0Wave;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00029EB8 File Offset: 0x000292B8
		protected static TransactionStateDelegatedCommitting _TransactionStateDelegatedCommitting
		{
			get
			{
				if (TransactionState._transactionStateDelegatedCommitting == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateDelegatedCommitting == null)
						{
							TransactionStateDelegatedCommitting transactionStateDelegatedCommitting = new TransactionStateDelegatedCommitting();
							Thread.MemoryBarrier();
							TransactionState._transactionStateDelegatedCommitting = transactionStateDelegatedCommitting;
						}
					}
				}
				return TransactionState._transactionStateDelegatedCommitting;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00029F1C File Offset: 0x0002931C
		protected static TransactionStateDelegatedAborting _TransactionStateDelegatedAborting
		{
			get
			{
				if (TransactionState._transactionStateDelegatedAborting == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionState._transactionStateDelegatedAborting == null)
						{
							TransactionStateDelegatedAborting transactionStateDelegatedAborting = new TransactionStateDelegatedAborting();
							Thread.MemoryBarrier();
							TransactionState._transactionStateDelegatedAborting = transactionStateDelegatedAborting;
						}
					}
				}
				return TransactionState._transactionStateDelegatedAborting;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00029F80 File Offset: 0x00029380
		internal static object ClassSyncObject
		{
			get
			{
				if (TransactionState.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref TransactionState.classSyncObject, obj, null);
				}
				return TransactionState.classSyncObject;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00029FAC File Offset: 0x000293AC
		internal void CommonEnterState(InternalTransaction tx)
		{
			tx.State = this;
		}

		// Token: 0x06000083 RID: 131
		internal abstract void EnterState(InternalTransaction tx);

		// Token: 0x06000084 RID: 132 RVA: 0x00029FC0 File Offset: 0x000293C0
		internal virtual void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00029FE4 File Offset: 0x000293E4
		internal virtual void EndCommit(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0002A008 File Offset: 0x00029408
		internal virtual void Rollback(InternalTransaction tx, Exception e)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0002A02C File Offset: 0x0002942C
		internal virtual Enlistment EnlistDurable(InternalTransaction tx, Guid resourceManagerIdentifier, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0002A050 File Offset: 0x00029450
		internal virtual Enlistment EnlistDurable(InternalTransaction tx, Guid resourceManagerIdentifier, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0002A074 File Offset: 0x00029474
		internal virtual Enlistment EnlistVolatile(InternalTransaction tx, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0002A098 File Offset: 0x00029498
		internal virtual Enlistment EnlistVolatile(InternalTransaction tx, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0002A0BC File Offset: 0x000294BC
		internal virtual void CheckForFinishedTransaction(InternalTransaction tx)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0002A0CC File Offset: 0x000294CC
		internal virtual Guid get_Identifier(InternalTransaction tx)
		{
			return Guid.Empty;
		}

		// Token: 0x0600008D RID: 141
		internal abstract TransactionStatus get_Status(InternalTransaction tx);

		// Token: 0x0600008E RID: 142 RVA: 0x0002A0E0 File Offset: 0x000294E0
		internal virtual void AddOutcomeRegistrant(InternalTransaction tx, TransactionCompletedEventHandler transactionCompletedDelegate)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0002A104 File Offset: 0x00029504
		internal virtual void GetObjectData(InternalTransaction tx, SerializationInfo serializationInfo, StreamingContext context)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0002A128 File Offset: 0x00029528
		internal virtual bool EnlistPromotableSinglePhase(InternalTransaction tx, IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Transaction atomicTransaction)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0002A14C File Offset: 0x0002954C
		internal virtual void CompleteBlockingClone(InternalTransaction tx)
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0002A15C File Offset: 0x0002955C
		internal virtual void CompleteAbortingClone(InternalTransaction tx)
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0002A16C File Offset: 0x0002956C
		internal virtual void CreateBlockingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0002A190 File Offset: 0x00029590
		internal virtual void CreateAbortingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0002A1B4 File Offset: 0x000295B4
		internal virtual void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0002A1E4 File Offset: 0x000295E4
		internal virtual void ChangeStateTransactionCommitted(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0002A214 File Offset: 0x00029614
		internal virtual void InDoubtFromEnlistment(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0002A244 File Offset: 0x00029644
		internal virtual void ChangeStatePromotedAborted(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0002A274 File Offset: 0x00029674
		internal virtual void ChangeStatePromotedCommitted(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0002A2A4 File Offset: 0x000296A4
		internal virtual void InDoubtFromDtc(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0002A2D4 File Offset: 0x000296D4
		internal virtual void ChangeStatePromotedPhase0(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0002A304 File Offset: 0x00029704
		internal virtual void ChangeStatePromotedPhase1(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0002A334 File Offset: 0x00029734
		internal virtual void ChangeStateAbortedDuringPromotion(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0002A364 File Offset: 0x00029764
		internal virtual void Timeout(InternalTransaction tx)
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0002A374 File Offset: 0x00029774
		internal virtual void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0002A398 File Offset: 0x00029798
		internal virtual void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0002A3BC File Offset: 0x000297BC
		internal virtual void RestartCommitIfNeeded(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
			{
				global::System.Transactions.Diagnostics.InvalidOperationExceptionTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0002A3EC File Offset: 0x000297EC
		internal virtual bool ContinuePhase0Prepares()
		{
			return false;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0002A3FC File Offset: 0x000297FC
		internal virtual bool ContinuePhase1Prepares()
		{
			return false;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0002A40C File Offset: 0x0002980C
		internal virtual void Promote(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0002A430 File Offset: 0x00029830
		internal virtual void DisposeRoot(InternalTransaction tx)
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0002A440 File Offset: 0x00029840
		internal virtual bool IsCompleted(InternalTransaction tx)
		{
			tx.needPulse = true;
			return false;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0002A458 File Offset: 0x00029858
		protected void AddVolatileEnlistment(ref VolatileEnlistmentSet enlistments, Enlistment enlistment)
		{
			if (enlistments.volatileEnlistmentCount == enlistments.volatileEnlistmentSize)
			{
				InternalEnlistment[] array = new InternalEnlistment[enlistments.volatileEnlistmentSize + 8];
				if (enlistments.volatileEnlistmentSize > 0)
				{
					Array.Copy(enlistments.volatileEnlistments, array, enlistments.volatileEnlistmentSize);
				}
				enlistments.volatileEnlistmentSize += 8;
				enlistments.volatileEnlistments = array;
			}
			enlistments.volatileEnlistments[enlistments.volatileEnlistmentCount] = enlistment.InternalEnlistment;
			enlistments.volatileEnlistmentCount++;
			VolatileEnlistmentState._VolatileEnlistmentActive.EnterState(enlistments.volatileEnlistments[enlistments.volatileEnlistmentCount - 1]);
		}

		// Token: 0x040000C7 RID: 199
		private static TransactionStateActive _transactionStateActive;

		// Token: 0x040000C8 RID: 200
		private static TransactionStateSubordinateActive _transactionStateSubordinateActive;

		// Token: 0x040000C9 RID: 201
		private static TransactionStatePhase0 _transactionStatePhase0;

		// Token: 0x040000CA RID: 202
		private static TransactionStateVolatilePhase1 _transactionStateVolatilePhase1;

		// Token: 0x040000CB RID: 203
		private static TransactionStateVolatileSPC _transactionStateVolatileSPC;

		// Token: 0x040000CC RID: 204
		private static TransactionStateSPC _transactionStateSPC;

		// Token: 0x040000CD RID: 205
		private static TransactionStateAborted _transactionStateAborted;

		// Token: 0x040000CE RID: 206
		private static TransactionStateCommitted _transactionStateCommitted;

		// Token: 0x040000CF RID: 207
		private static TransactionStateInDoubt _transactionStateInDoubt;

		// Token: 0x040000D0 RID: 208
		private static TransactionStatePromoted _transactionStatePromoted;

		// Token: 0x040000D1 RID: 209
		private static TransactionStateNonCommittablePromoted _transactionStateNonCommittablePromoted;

		// Token: 0x040000D2 RID: 210
		private static TransactionStatePromotedP0Wave _transactionStatePromotedP0Wave;

		// Token: 0x040000D3 RID: 211
		private static TransactionStatePromotedCommitting _transactionStatePromotedCommitting;

		// Token: 0x040000D4 RID: 212
		private static TransactionStatePromotedPhase0 _transactionStatePromotedPhase0;

		// Token: 0x040000D5 RID: 213
		private static TransactionStatePromotedPhase1 _transactionStatePromotedPhase1;

		// Token: 0x040000D6 RID: 214
		private static TransactionStatePromotedP0Aborting _transactionStatePromotedP0Aborting;

		// Token: 0x040000D7 RID: 215
		private static TransactionStatePromotedP1Aborting _transactionStatePromotedP1Aborting;

		// Token: 0x040000D8 RID: 216
		private static TransactionStatePromotedAborted _transactionStatePromotedAborted;

		// Token: 0x040000D9 RID: 217
		private static TransactionStatePromotedCommitted _transactionStatePromotedCommitted;

		// Token: 0x040000DA RID: 218
		private static TransactionStatePromotedIndoubt _transactionStatePromotedIndoubt;

		// Token: 0x040000DB RID: 219
		private static TransactionStateDelegated _transactionStateDelegated;

		// Token: 0x040000DC RID: 220
		private static TransactionStateDelegatedSubordinate _transactionStateDelegatedSubordinate;

		// Token: 0x040000DD RID: 221
		private static TransactionStateDelegatedP0Wave _transactionStateDelegatedP0Wave;

		// Token: 0x040000DE RID: 222
		private static TransactionStateDelegatedCommitting _transactionStateDelegatedCommitting;

		// Token: 0x040000DF RID: 223
		private static TransactionStateDelegatedAborting _transactionStateDelegatedAborting;

		// Token: 0x040000E0 RID: 224
		private static TransactionStatePSPEOperation _transactionStatePSPEOperation;

		// Token: 0x040000E1 RID: 225
		private static object classSyncObject;
	}
}
