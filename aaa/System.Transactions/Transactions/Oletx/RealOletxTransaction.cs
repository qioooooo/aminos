using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x02000091 RID: 145
	internal class RealOletxTransaction
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00037370 File Offset: 0x00036770
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00037384 File Offset: 0x00036784
		internal InternalTransaction InternalTransaction
		{
			get
			{
				return this.internalTransaction;
			}
			set
			{
				this.internalTransaction = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00037398 File Offset: 0x00036798
		internal OletxTransactionManager OletxTransactionManagerInstance
		{
			get
			{
				return this.oletxTransactionManager;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000373AC File Offset: 0x000367AC
		internal Guid Identifier
		{
			get
			{
				if (this.txGuid.Equals(Guid.Empty))
				{
					throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("CannotGetTransactionIdentifier"), null);
				}
				return this.txGuid;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000373EC File Offset: 0x000367EC
		internal IsolationLevel TransactionIsolationLevel
		{
			get
			{
				return this.isolationLevel;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00037400 File Offset: 0x00036800
		internal TransactionStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00037414 File Offset: 0x00036814
		internal Guid TxGuid
		{
			get
			{
				return this.txGuid;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00037428 File Offset: 0x00036828
		internal void IncrementUndecidedEnlistments()
		{
			Interlocked.Increment(ref this.undecidedEnlistmentCount);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00037444 File Offset: 0x00036844
		internal void DecrementUndecidedEnlistments()
		{
			Interlocked.Decrement(ref this.undecidedEnlistmentCount);
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00037460 File Offset: 0x00036860
		internal int UndecidedEnlistments
		{
			get
			{
				return this.undecidedEnlistmentCount;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00037474 File Offset: 0x00036874
		internal bool Doomed
		{
			get
			{
				return this.doomed;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00037488 File Offset: 0x00036888
		internal ITransactionShim TransactionShim
		{
			get
			{
				ITransactionShim transactionShim = this.transactionShim;
				if (transactionShim == null)
				{
					throw TransactionInDoubtException.Create(SR.GetString("TraceSourceOletx"), null);
				}
				return transactionShim;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000374B4 File Offset: 0x000368B4
		internal RealOletxTransaction(OletxTransactionManager transactionManager, ITransactionShim transactionShim, OutcomeEnlistment outcomeEnlistment, Guid identifier, OletxTransactionIsolationLevel oletxIsoLevel, bool isRoot)
		{
			bool flag = false;
			try
			{
				this.oletxTransactionManager = transactionManager;
				this.transactionShim = transactionShim;
				this.outcomeEnlistment = outcomeEnlistment;
				this.txGuid = identifier;
				this.isolationLevel = OletxTransactionManager.ConvertIsolationLevelFromProxyValue(oletxIsoLevel);
				this.status = TransactionStatus.Active;
				this.undisposedOletxTransactionCount = 0;
				this.phase0EnlistVolatilementContainerList = null;
				this.phase1EnlistVolatilementContainer = null;
				this.tooLateForEnlistments = false;
				this.internalTransaction = null;
				this.creationTime = DateTime.UtcNow;
				this.lastStateChangeTime = this.creationTime;
				this.internalClone = new OletxTransaction(this);
				if (this.outcomeEnlistment != null)
				{
					this.outcomeEnlistment.SetRealTransaction(this);
				}
				else
				{
					this.status = TransactionStatus.InDoubt;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.HaveListeners)
				{
					global::System.Transactions.Diagnostics.DiagnosticTrace.TraceTransfer(this.txGuid);
				}
				flag = true;
			}
			finally
			{
				if (!flag && this.outcomeEnlistment != null)
				{
					this.outcomeEnlistment.UnregisterOutcomeCallback();
					this.outcomeEnlistment = null;
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000375B8 File Offset: 0x000369B8
		// (set) Token: 0x060003CD RID: 973 RVA: 0x000375CC File Offset: 0x000369CC
		internal bool TooLateForEnlistments
		{
			get
			{
				return this.tooLateForEnlistments;
			}
			set
			{
				this.tooLateForEnlistments = value;
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000375E0 File Offset: 0x000369E0
		internal OletxVolatileEnlistmentContainer AddDependentClone(bool delayCommit)
		{
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			IVoterBallotShim voterBallotShim = null;
			bool flag = false;
			bool flag2 = false;
			OletxVolatileEnlistmentContainer oletxVolatileEnlistmentContainer = null;
			OletxPhase0VolatileEnlistmentContainer oletxPhase0VolatileEnlistmentContainer = null;
			OletxPhase1VolatileEnlistmentContainer oletxPhase1VolatileEnlistmentContainer = null;
			bool flag3 = false;
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				lock (this)
				{
					if (delayCommit)
					{
						if (this.phase0EnlistVolatilementContainerList == null)
						{
							this.phase0EnlistVolatilementContainerList = new ArrayList(1);
						}
						if (this.phase0EnlistVolatilementContainerList.Count == 0)
						{
							oletxPhase0VolatileEnlistmentContainer = new OletxPhase0VolatileEnlistmentContainer(this);
							flag2 = true;
						}
						else
						{
							oletxPhase0VolatileEnlistmentContainer = this.phase0EnlistVolatilementContainerList[this.phase0EnlistVolatilementContainerList.Count - 1] as OletxPhase0VolatileEnlistmentContainer;
							if (!oletxPhase0VolatileEnlistmentContainer.NewEnlistmentsAllowed)
							{
								oletxPhase0VolatileEnlistmentContainer = new OletxPhase0VolatileEnlistmentContainer(this);
								flag2 = true;
							}
							else
							{
								flag2 = false;
							}
						}
						if (flag2)
						{
							intPtr = HandleTable.AllocHandle(oletxPhase0VolatileEnlistmentContainer);
						}
					}
					else if (this.phase1EnlistVolatilementContainer == null)
					{
						oletxPhase1VolatileEnlistmentContainer = new OletxPhase1VolatileEnlistmentContainer(this);
						flag = true;
						oletxPhase1VolatileEnlistmentContainer.voterHandle = HandleTable.AllocHandle(oletxPhase1VolatileEnlistmentContainer);
					}
					else
					{
						flag = false;
						oletxPhase1VolatileEnlistmentContainer = this.phase1EnlistVolatilementContainer;
					}
					try
					{
						if (flag2)
						{
							lock (oletxPhase0VolatileEnlistmentContainer)
							{
								this.transactionShim.Phase0Enlist(intPtr, out phase0EnlistmentShim);
								oletxPhase0VolatileEnlistmentContainer.Phase0EnlistmentShim = phase0EnlistmentShim;
							}
						}
						if (flag)
						{
							this.OletxTransactionManagerInstance.dtcTransactionManagerLock.AcquireReaderLock(-1);
							try
							{
								this.transactionShim.CreateVoter(oletxPhase1VolatileEnlistmentContainer.voterHandle, out voterBallotShim);
								flag3 = true;
							}
							finally
							{
								this.OletxTransactionManagerInstance.dtcTransactionManagerLock.ReleaseReaderLock();
							}
							oletxPhase1VolatileEnlistmentContainer.VoterBallotShim = voterBallotShim;
						}
						if (delayCommit)
						{
							if (flag2)
							{
								this.phase0EnlistVolatilementContainerList.Add(oletxPhase0VolatileEnlistmentContainer);
							}
							oletxPhase0VolatileEnlistmentContainer.AddDependentClone();
							oletxVolatileEnlistmentContainer = oletxPhase0VolatileEnlistmentContainer;
						}
						else
						{
							if (flag)
							{
								this.phase1EnlistVolatilementContainer = oletxPhase1VolatileEnlistmentContainer;
							}
							oletxPhase1VolatileEnlistmentContainer.AddDependentClone();
							oletxVolatileEnlistmentContainer = oletxPhase1VolatileEnlistmentContainer;
						}
					}
					catch (COMException ex)
					{
						OletxTransactionManager.ProxyException(ex);
						throw;
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero && oletxPhase0VolatileEnlistmentContainer.Phase0EnlistmentShim == null)
				{
					HandleTable.FreeHandle(intPtr);
				}
				if (!flag3 && oletxPhase1VolatileEnlistmentContainer != null && oletxPhase1VolatileEnlistmentContainer.voterHandle != IntPtr.Zero && flag)
				{
					HandleTable.FreeHandle(oletxPhase1VolatileEnlistmentContainer.voterHandle);
				}
			}
			return oletxVolatileEnlistmentContainer;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00037834 File Offset: 0x00036C34
		internal IPromotedEnlistment CommonEnlistVolatile(IEnlistmentNotificationInternal enlistmentNotification, EnlistmentOptions enlistmentOptions, OletxTransaction oletxTransaction)
		{
			OletxVolatileEnlistment oletxVolatileEnlistment = null;
			bool flag = false;
			bool flag2 = false;
			OletxPhase0VolatileEnlistmentContainer oletxPhase0VolatileEnlistmentContainer = null;
			OletxPhase1VolatileEnlistmentContainer oletxPhase1VolatileEnlistmentContainer = null;
			IntPtr intPtr = IntPtr.Zero;
			IVoterBallotShim voterBallotShim = null;
			IPhase0EnlistmentShim phase0EnlistmentShim = null;
			bool flag3 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				lock (this)
				{
					oletxVolatileEnlistment = new OletxVolatileEnlistment(enlistmentNotification, enlistmentOptions, oletxTransaction);
					if ((enlistmentOptions & EnlistmentOptions.EnlistDuringPrepareRequired) != EnlistmentOptions.None)
					{
						if (this.phase0EnlistVolatilementContainerList == null)
						{
							this.phase0EnlistVolatilementContainerList = new ArrayList(1);
						}
						if (this.phase0EnlistVolatilementContainerList.Count == 0)
						{
							oletxPhase0VolatileEnlistmentContainer = new OletxPhase0VolatileEnlistmentContainer(this);
							flag2 = true;
						}
						else
						{
							oletxPhase0VolatileEnlistmentContainer = this.phase0EnlistVolatilementContainerList[this.phase0EnlistVolatilementContainerList.Count - 1] as OletxPhase0VolatileEnlistmentContainer;
							if (!oletxPhase0VolatileEnlistmentContainer.NewEnlistmentsAllowed)
							{
								oletxPhase0VolatileEnlistmentContainer = new OletxPhase0VolatileEnlistmentContainer(this);
								flag2 = true;
							}
							else
							{
								flag2 = false;
							}
						}
						if (flag2)
						{
							intPtr = HandleTable.AllocHandle(oletxPhase0VolatileEnlistmentContainer);
						}
					}
					else if (this.phase1EnlistVolatilementContainer == null)
					{
						flag = true;
						oletxPhase1VolatileEnlistmentContainer = new OletxPhase1VolatileEnlistmentContainer(this);
						oletxPhase1VolatileEnlistmentContainer.voterHandle = HandleTable.AllocHandle(oletxPhase1VolatileEnlistmentContainer);
					}
					else
					{
						flag = false;
						oletxPhase1VolatileEnlistmentContainer = this.phase1EnlistVolatilementContainer;
					}
					try
					{
						if (flag2)
						{
							lock (oletxPhase0VolatileEnlistmentContainer)
							{
								this.transactionShim.Phase0Enlist(intPtr, out phase0EnlistmentShim);
								oletxPhase0VolatileEnlistmentContainer.Phase0EnlistmentShim = phase0EnlistmentShim;
							}
						}
						if (flag)
						{
							this.transactionShim.CreateVoter(oletxPhase1VolatileEnlistmentContainer.voterHandle, out voterBallotShim);
							flag3 = true;
							oletxPhase1VolatileEnlistmentContainer.VoterBallotShim = voterBallotShim;
						}
						if ((enlistmentOptions & EnlistmentOptions.EnlistDuringPrepareRequired) != EnlistmentOptions.None)
						{
							oletxPhase0VolatileEnlistmentContainer.AddEnlistment(oletxVolatileEnlistment);
							if (flag2)
							{
								this.phase0EnlistVolatilementContainerList.Add(oletxPhase0VolatileEnlistmentContainer);
							}
						}
						else
						{
							oletxPhase1VolatileEnlistmentContainer.AddEnlistment(oletxVolatileEnlistment);
							if (flag)
							{
								this.phase1EnlistVolatilementContainer = oletxPhase1VolatileEnlistmentContainer;
							}
						}
					}
					catch (COMException ex)
					{
						OletxTransactionManager.ProxyException(ex);
						throw;
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero && oletxPhase0VolatileEnlistmentContainer.Phase0EnlistmentShim == null)
				{
					HandleTable.FreeHandle(intPtr);
				}
				if (!flag3 && oletxPhase1VolatileEnlistmentContainer != null && oletxPhase1VolatileEnlistmentContainer.voterHandle != IntPtr.Zero && flag)
				{
					HandleTable.FreeHandle(oletxPhase1VolatileEnlistmentContainer.voterHandle);
				}
			}
			return oletxVolatileEnlistment;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00037A58 File Offset: 0x00036E58
		internal IPromotedEnlistment EnlistVolatile(ISinglePhaseNotificationInternal enlistmentNotification, EnlistmentOptions enlistmentOptions, OletxTransaction oletxTransaction)
		{
			return this.CommonEnlistVolatile(enlistmentNotification, enlistmentOptions, oletxTransaction);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00037A70 File Offset: 0x00036E70
		internal IPromotedEnlistment EnlistVolatile(IEnlistmentNotificationInternal enlistmentNotification, EnlistmentOptions enlistmentOptions, OletxTransaction oletxTransaction)
		{
			return this.CommonEnlistVolatile(enlistmentNotification, enlistmentOptions, oletxTransaction);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00037A88 File Offset: 0x00036E88
		internal void Commit()
		{
			try
			{
				this.transactionShim.Commit();
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_ABORTED == ex.ErrorCode || NativeMethods.XACT_E_INDOUBT == ex.ErrorCode)
				{
					Interlocked.CompareExchange<Exception>(ref this.innerException, ex, null);
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
					}
				}
				else
				{
					if (NativeMethods.XACT_E_ALREADYINPROGRESS == ex.ErrorCode)
					{
						throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionAlreadyOver"), ex);
					}
					OletxTransactionManager.ProxyException(ex);
					throw;
				}
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00037B38 File Offset: 0x00036F38
		internal void Rollback()
		{
			Guid empty = Guid.Empty;
			lock (this)
			{
				if (TransactionStatus.Aborted != this.status && this.status != TransactionStatus.Active)
				{
					throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionAlreadyOver"), null);
				}
				if (TransactionStatus.Aborted == this.status)
				{
					return;
				}
				if (0 < this.undecidedEnlistmentCount)
				{
					this.doomed = true;
				}
				else if (this.tooLateForEnlistments)
				{
					throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionAlreadyOver"), null);
				}
				if (this.phase0EnlistVolatilementContainerList != null)
				{
					foreach (object obj in this.phase0EnlistVolatilementContainerList)
					{
						OletxPhase0VolatileEnlistmentContainer oletxPhase0VolatileEnlistmentContainer = (OletxPhase0VolatileEnlistmentContainer)obj;
						oletxPhase0VolatileEnlistmentContainer.RollbackFromTransaction();
					}
				}
				if (this.phase1EnlistVolatilementContainer != null)
				{
					this.phase1EnlistVolatilementContainer.RollbackFromTransaction();
				}
			}
			try
			{
				this.transactionShim.Abort();
			}
			catch (COMException ex)
			{
				if (NativeMethods.XACT_E_ALREADYINPROGRESS != ex.ErrorCode)
				{
					OletxTransactionManager.ProxyException(ex);
					throw;
				}
				if (!this.doomed)
				{
					throw TransactionException.Create(SR.GetString("TraceSourceOletx"), SR.GetString("TransactionAlreadyOver"), ex);
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), ex);
				}
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00037CD4 File Offset: 0x000370D4
		internal void OletxTransactionCreated()
		{
			Interlocked.Increment(ref this.undisposedOletxTransactionCount);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00037CF0 File Offset: 0x000370F0
		internal void OletxTransactionDisposed()
		{
			Interlocked.Decrement(ref this.undisposedOletxTransactionCount);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00037D0C File Offset: 0x0003710C
		internal void FireOutcome(TransactionStatus statusArg)
		{
			lock (this)
			{
				if (statusArg == TransactionStatus.Committed)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
					{
						global::System.Transactions.Diagnostics.TransactionCommittedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), this.TransactionTraceId);
					}
					this.status = TransactionStatus.Committed;
				}
				else if (statusArg == TransactionStatus.Aborted)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						global::System.Transactions.Diagnostics.TransactionAbortedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), this.TransactionTraceId);
					}
					this.status = TransactionStatus.Aborted;
				}
				else
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						global::System.Transactions.Diagnostics.TransactionInDoubtTraceRecord.Trace(SR.GetString("TraceSourceOletx"), this.TransactionTraceId);
					}
					this.status = TransactionStatus.InDoubt;
				}
			}
			if (this.InternalTransaction != null)
			{
				InternalTransaction.DistributedTransactionOutcome(this.InternalTransaction, this.status);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00037DD4 File Offset: 0x000371D4
		internal TransactionTraceIdentifier TransactionTraceId
		{
			get
			{
				if (TransactionTraceIdentifier.Empty == this.traceIdentifier)
				{
					lock (this)
					{
						if (TransactionTraceIdentifier.Empty == this.traceIdentifier && Guid.Empty != this.txGuid)
						{
							TransactionTraceIdentifier transactionTraceIdentifier = new TransactionTraceIdentifier(this.txGuid.ToString(), 0);
							Thread.MemoryBarrier();
							this.traceIdentifier = transactionTraceIdentifier;
						}
					}
				}
				return this.traceIdentifier;
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00037E70 File Offset: 0x00037270
		internal void TMDown()
		{
			lock (this)
			{
				if (this.phase0EnlistVolatilementContainerList != null)
				{
					foreach (object obj in this.phase0EnlistVolatilementContainerList)
					{
						OletxPhase0VolatileEnlistmentContainer oletxPhase0VolatileEnlistmentContainer = (OletxPhase0VolatileEnlistmentContainer)obj;
						oletxPhase0VolatileEnlistmentContainer.TMDown();
					}
				}
			}
			this.outcomeEnlistment.TMDown();
		}

		// Token: 0x04000203 RID: 515
		private OletxTransactionManager oletxTransactionManager;

		// Token: 0x04000204 RID: 516
		private ITransactionShim transactionShim;

		// Token: 0x04000205 RID: 517
		private Guid txGuid;

		// Token: 0x04000206 RID: 518
		private IsolationLevel isolationLevel;

		// Token: 0x04000207 RID: 519
		internal Exception innerException;

		// Token: 0x04000208 RID: 520
		private TransactionStatus status;

		// Token: 0x04000209 RID: 521
		private int undisposedOletxTransactionCount;

		// Token: 0x0400020A RID: 522
		internal ArrayList phase0EnlistVolatilementContainerList;

		// Token: 0x0400020B RID: 523
		internal OletxPhase1VolatileEnlistmentContainer phase1EnlistVolatilementContainer;

		// Token: 0x0400020C RID: 524
		private OutcomeEnlistment outcomeEnlistment;

		// Token: 0x0400020D RID: 525
		private int undecidedEnlistmentCount;

		// Token: 0x0400020E RID: 526
		private bool doomed;

		// Token: 0x0400020F RID: 527
		internal int enlistmentCount;

		// Token: 0x04000210 RID: 528
		private DateTime creationTime;

		// Token: 0x04000211 RID: 529
		private DateTime lastStateChangeTime;

		// Token: 0x04000212 RID: 530
		private TransactionTraceIdentifier traceIdentifier = TransactionTraceIdentifier.Empty;

		// Token: 0x04000213 RID: 531
		internal OletxCommittableTransaction committableTransaction;

		// Token: 0x04000214 RID: 532
		internal OletxTransaction internalClone;

		// Token: 0x04000215 RID: 533
		private bool tooLateForEnlistments;

		// Token: 0x04000216 RID: 534
		private InternalTransaction internalTransaction;
	}
}
