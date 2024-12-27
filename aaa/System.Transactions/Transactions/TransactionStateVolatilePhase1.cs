using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200001B RID: 27
	internal class TransactionStateVolatilePhase1 : ActiveStates
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0002AE48 File Offset: 0x0002A248
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			tx.committableTransaction.complete = true;
			if (tx.phase1Volatiles.dependentClones != 0)
			{
				TransactionState._TransactionStateAborted.EnterState(tx);
				return;
			}
			if (tx.phase1Volatiles.volatileEnlistmentCount == 1 && tx.durableEnlistment == null && tx.phase1Volatiles.volatileEnlistments[0].SinglePhaseNotification != null)
			{
				TransactionState._TransactionStateVolatileSPC.EnterState(tx);
				return;
			}
			if (tx.phase1Volatiles.volatileEnlistmentCount > 0)
			{
				for (int i = 0; i < tx.phase1Volatiles.volatileEnlistmentCount; i++)
				{
					tx.phase1Volatiles.volatileEnlistments[i].twoPhaseState.ChangeStatePreparing(tx.phase1Volatiles.volatileEnlistments[i]);
					if (!tx.State.ContinuePhase1Prepares())
					{
						return;
					}
				}
				return;
			}
			TransactionState._TransactionStateSPC.EnterState(tx);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0002AF1C File Offset: 0x0002A31C
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			this.ChangeStateTransactionAborted(tx, e);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0002AF34 File Offset: 0x0002A334
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			TransactionState._TransactionStateAborted.EnterState(tx);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0002AF5C File Offset: 0x0002A35C
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
			TransactionState._TransactionStateSPC.EnterState(tx);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0002AF74 File Offset: 0x0002A374
		internal override bool ContinuePhase1Prepares()
		{
			return true;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0002AF84 File Offset: 0x0002A384
		internal override void Timeout(InternalTransaction tx)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionTimeoutTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
			TimeoutException ex = new TimeoutException(SR.GetString("TraceTransactionTimeout"));
			this.Rollback(tx, ex);
		}
	}
}
