using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000018 RID: 24
	internal class TransactionStateActive : EnlistableStates
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x0002A750 File Offset: 0x00029B50
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0002A764 File Offset: 0x00029B64
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			tx.asyncCommit = asyncCommit;
			tx.asyncCallback = asyncCallback;
			tx.asyncState = asyncState;
			TransactionState._TransactionStatePhase0.EnterState(tx);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0002A794 File Offset: 0x00029B94
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			TransactionState._TransactionStateAborted.EnterState(tx);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0002A7BC File Offset: 0x00029BBC
		internal override Enlistment EnlistVolatile(InternalTransaction tx, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Enlistment enlistment = new Enlistment(tx, enlistmentNotification, null, atomicTransaction, enlistmentOptions);
			if ((enlistmentOptions & EnlistmentOptions.EnlistDuringPrepareRequired) != EnlistmentOptions.None)
			{
				base.AddVolatileEnlistment(ref tx.phase0Volatiles, enlistment);
			}
			else
			{
				base.AddVolatileEnlistment(ref tx.phase1Volatiles, enlistment);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.EnlistmentTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.InternalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentType.Volatile, enlistmentOptions);
			}
			return enlistment;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0002A81C File Offset: 0x00029C1C
		internal override Enlistment EnlistVolatile(InternalTransaction tx, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions, Transaction atomicTransaction)
		{
			Enlistment enlistment = new Enlistment(tx, enlistmentNotification, enlistmentNotification, atomicTransaction, enlistmentOptions);
			if ((enlistmentOptions & EnlistmentOptions.EnlistDuringPrepareRequired) != EnlistmentOptions.None)
			{
				base.AddVolatileEnlistment(ref tx.phase0Volatiles, enlistment);
			}
			else
			{
				base.AddVolatileEnlistment(ref tx.phase1Volatiles, enlistment);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.EnlistmentTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.InternalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentType.Volatile, enlistmentOptions);
			}
			return enlistment;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0002A87C File Offset: 0x00029C7C
		internal override bool EnlistPromotableSinglePhase(InternalTransaction tx, IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Transaction atomicTransaction)
		{
			if (tx.durableEnlistment != null)
			{
				return false;
			}
			TransactionState._TransactionStatePSPEOperation.PSPEInitialize(tx, promotableSinglePhaseNotification);
			Enlistment enlistment = new Enlistment(tx, promotableSinglePhaseNotification, atomicTransaction);
			tx.durableEnlistment = enlistment.InternalEnlistment;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.EnlistmentTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.durableEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentType.PromotableSinglePhase, EnlistmentOptions.None);
			}
			tx.promoter = promotableSinglePhaseNotification;
			tx.promoteState = TransactionState._TransactionStateDelegated;
			DurableEnlistmentState._DurableEnlistmentActive.EnterState(tx.durableEnlistment);
			return true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0002A8FC File Offset: 0x00029CFC
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0002A90C File Offset: 0x00029D0C
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0002A91C File Offset: 0x00029D1C
		internal override void DisposeRoot(InternalTransaction tx)
		{
			tx.State.Rollback(tx, null);
		}
	}
}
