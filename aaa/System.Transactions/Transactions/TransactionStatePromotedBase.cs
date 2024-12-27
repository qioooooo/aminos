using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000022 RID: 34
	internal abstract class TransactionStatePromotedBase : TransactionState
	{
		// Token: 0x0600010C RID: 268 RVA: 0x0002B674 File Offset: 0x0002AA74
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Active;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0002B684 File Offset: 0x0002AA84
		internal override Enlistment EnlistVolatile(InternalTransaction tx, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Monitor.Exit(tx);
			Enlistment enlistment2;
			try
			{
				Enlistment enlistment = new Enlistment(enlistmentNotification, tx, atomicTransaction);
				EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment.InternalEnlistment);
				enlistment.InternalEnlistment.PromotedEnlistment = tx.PromotedTransaction.EnlistVolatile(enlistment.InternalEnlistment, enlistmentOptions);
				enlistment2 = enlistment;
			}
			finally
			{
				Monitor.Enter(tx);
			}
			return enlistment2;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0002B6F8 File Offset: 0x0002AAF8
		internal override Enlistment EnlistVolatile(InternalTransaction tx, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Monitor.Exit(tx);
			Enlistment enlistment2;
			try
			{
				Enlistment enlistment = new Enlistment(enlistmentNotification, tx, atomicTransaction);
				EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment.InternalEnlistment);
				enlistment.InternalEnlistment.PromotedEnlistment = tx.PromotedTransaction.EnlistVolatile(enlistment.InternalEnlistment, enlistmentOptions);
				enlistment2 = enlistment;
			}
			finally
			{
				Monitor.Enter(tx);
			}
			return enlistment2;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0002B76C File Offset: 0x0002AB6C
		internal override Enlistment EnlistDurable(InternalTransaction tx, Guid resourceManagerIdentifier, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Monitor.Exit(tx);
			Enlistment enlistment2;
			try
			{
				Enlistment enlistment = new Enlistment(resourceManagerIdentifier, tx, enlistmentNotification, null, atomicTransaction);
				EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment.InternalEnlistment);
				enlistment.InternalEnlistment.PromotedEnlistment = tx.PromotedTransaction.EnlistDurable(resourceManagerIdentifier, (DurableInternalEnlistment)enlistment.InternalEnlistment, false, enlistmentOptions);
				enlistment2 = enlistment;
			}
			finally
			{
				Monitor.Enter(tx);
			}
			return enlistment2;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0002B7E8 File Offset: 0x0002ABE8
		internal override Enlistment EnlistDurable(InternalTransaction tx, Guid resourceManagerIdentifier, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Monitor.Exit(tx);
			Enlistment enlistment2;
			try
			{
				Enlistment enlistment = new Enlistment(resourceManagerIdentifier, tx, enlistmentNotification, enlistmentNotification, atomicTransaction);
				EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment.InternalEnlistment);
				enlistment.InternalEnlistment.PromotedEnlistment = tx.PromotedTransaction.EnlistDurable(resourceManagerIdentifier, (DurableInternalEnlistment)enlistment.InternalEnlistment, true, enlistmentOptions);
				enlistment2 = enlistment;
			}
			finally
			{
				Monitor.Enter(tx);
			}
			return enlistment2;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0002B864 File Offset: 0x0002AC64
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			Monitor.Exit(tx);
			try
			{
				tx.PromotedTransaction.Rollback();
			}
			finally
			{
				Monitor.Enter(tx);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0002B8B8 File Offset: 0x0002ACB8
		internal override Guid get_Identifier(InternalTransaction tx)
		{
			return tx.PromotedTransaction.Identifier;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0002B8D0 File Offset: 0x0002ACD0
		internal override void AddOutcomeRegistrant(InternalTransaction tx, TransactionCompletedEventHandler transactionCompletedDelegate)
		{
			tx.transactionCompletedDelegate = (TransactionCompletedEventHandler)Delegate.Combine(tx.transactionCompletedDelegate, transactionCompletedDelegate);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0002B8F4 File Offset: 0x0002ACF4
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			tx.asyncCommit = asyncCommit;
			tx.asyncCallback = asyncCallback;
			tx.asyncState = asyncState;
			TransactionState._TransactionStatePromotedCommitting.EnterState(tx);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0002B924 File Offset: 0x0002AD24
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedP0Wave.EnterState(tx);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0002B93C File Offset: 0x0002AD3C
		internal override bool EnlistPromotableSinglePhase(InternalTransaction tx, IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Transaction atomicTransaction)
		{
			return false;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0002B94C File Offset: 0x0002AD4C
		internal override void CompleteBlockingClone(InternalTransaction tx)
		{
			if (tx.phase0Volatiles.dependentClones > 0)
			{
				tx.phase0Volatiles.dependentClones = tx.phase0Volatiles.dependentClones - 1;
				if (tx.phase0Volatiles.preparedVolatileEnlistments == tx.phase0VolatileWaveCount + tx.phase0Volatiles.dependentClones)
				{
					tx.State.Phase0VolatilePrepareDone(tx);
					return;
				}
			}
			else
			{
				tx.phase0WaveDependentCloneCount--;
				if (tx.phase0WaveDependentCloneCount == 0)
				{
					global::System.Transactions.Oletx.OletxDependentTransaction phase0WaveDependentClone = tx.phase0WaveDependentClone;
					tx.phase0WaveDependentClone = null;
					Monitor.Exit(tx);
					try
					{
						try
						{
							phase0WaveDependentClone.Complete();
						}
						finally
						{
							phase0WaveDependentClone.Dispose();
						}
					}
					finally
					{
						Monitor.Enter(tx);
					}
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0002BA1C File Offset: 0x0002AE1C
		internal override void CompleteAbortingClone(InternalTransaction tx)
		{
			if (tx.phase1Volatiles.VolatileDemux != null)
			{
				tx.phase1Volatiles.dependentClones = tx.phase1Volatiles.dependentClones - 1;
				return;
			}
			tx.abortingDependentCloneCount--;
			if (tx.abortingDependentCloneCount == 0)
			{
				global::System.Transactions.Oletx.OletxDependentTransaction abortingDependentClone = tx.abortingDependentClone;
				tx.abortingDependentClone = null;
				Monitor.Exit(tx);
				try
				{
					try
					{
						abortingDependentClone.Complete();
					}
					finally
					{
						abortingDependentClone.Dispose();
					}
				}
				finally
				{
					Monitor.Enter(tx);
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0002BAC0 File Offset: 0x0002AEC0
		internal override void CreateBlockingClone(InternalTransaction tx)
		{
			if (tx.phase0WaveDependentClone == null)
			{
				tx.phase0WaveDependentClone = tx.PromotedTransaction.DependentClone(true);
			}
			tx.phase0WaveDependentCloneCount++;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0002BAF8 File Offset: 0x0002AEF8
		internal override void CreateAbortingClone(InternalTransaction tx)
		{
			if (tx.phase1Volatiles.VolatileDemux != null)
			{
				tx.phase1Volatiles.dependentClones = tx.phase1Volatiles.dependentClones + 1;
				return;
			}
			if (tx.abortingDependentClone == null)
			{
				tx.abortingDependentClone = tx.PromotedTransaction.DependentClone(false);
			}
			tx.abortingDependentCloneCount++;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0002BB50 File Offset: 0x0002AF50
		internal override bool ContinuePhase0Prepares()
		{
			return true;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0002BB60 File Offset: 0x0002AF60
		internal override void GetObjectData(InternalTransaction tx, SerializationInfo serializationInfo, StreamingContext context)
		{
			ISerializable promotedTransaction = tx.PromotedTransaction;
			if (promotedTransaction == null)
			{
				throw new NotSupportedException();
			}
			serializationInfo.FullTypeName = tx.PromotedTransaction.GetType().FullName;
			promotedTransaction.GetObjectData(serializationInfo, context);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0002BB9C File Offset: 0x0002AF9C
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedAborted.EnterState(tx);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0002BBB4 File Offset: 0x0002AFB4
		internal override void ChangeStatePromotedCommitted(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedCommitted.EnterState(tx);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0002BBCC File Offset: 0x0002AFCC
		internal override void InDoubtFromDtc(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedIndoubt.EnterState(tx);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0002BBE4 File Offset: 0x0002AFE4
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedIndoubt.EnterState(tx);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0002BBFC File Offset: 0x0002AFFC
		internal override void ChangeStateAbortedDuringPromotion(InternalTransaction tx)
		{
			TransactionState._TransactionStateAborted.EnterState(tx);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0002BC14 File Offset: 0x0002B014
		internal override void Timeout(InternalTransaction tx)
		{
			try
			{
				if (tx.innerException == null)
				{
					tx.innerException = new TimeoutException(SR.GetString("TraceTransactionTimeout"));
				}
				tx.PromotedTransaction.Rollback();
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
				{
					global::System.Transactions.Diagnostics.TransactionTimeoutTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
				}
			}
			catch (TransactionException ex)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0002BCA0 File Offset: 0x0002B0A0
		internal override void Promote(InternalTransaction tx)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0002BCB0 File Offset: 0x0002B0B0
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0002BCC0 File Offset: 0x0002B0C0
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
		}
	}
}
