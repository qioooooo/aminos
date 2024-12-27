using System;
using System.Collections;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000024 RID: 36
	internal class TransactionStatePromoted : TransactionStatePromotedBase
	{
		// Token: 0x06000129 RID: 297 RVA: 0x0002BD20 File Offset: 0x0002B120
		internal override void EnterState(InternalTransaction tx)
		{
			if (tx.outcomeSource.isoLevel == IsolationLevel.Snapshot)
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("CannotPromoteSnapshot"), null);
			}
			base.CommonEnterState(tx);
			global::System.Transactions.Oletx.OletxCommittableTransaction oletxCommittableTransaction = null;
			try
			{
				TimeSpan timeSpan;
				if (tx.AbsoluteTimeout == 9223372036854775807L)
				{
					timeSpan = TimeSpan.Zero;
				}
				else
				{
					timeSpan = TransactionManager.TransactionTable.RecalcTimeout(tx);
					if (timeSpan <= TimeSpan.Zero)
					{
						return;
					}
				}
				TransactionOptions transactionOptions = default(TransactionOptions);
				transactionOptions.IsolationLevel = tx.outcomeSource.isoLevel;
				transactionOptions.Timeout = timeSpan;
				oletxCommittableTransaction = TransactionManager.DistributedTransactionManager.CreateTransaction(transactionOptions);
				oletxCommittableTransaction.savedLtmPromotedTransaction = tx.outcomeSource;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
				{
					global::System.Transactions.Diagnostics.TransactionPromotedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId, oletxCommittableTransaction.TransactionTraceId);
				}
			}
			catch (TransactionException ex)
			{
				tx.innerException = ex;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
				return;
			}
			finally
			{
				if (oletxCommittableTransaction == null)
				{
					tx.State.ChangeStateAbortedDuringPromotion(tx);
				}
			}
			tx.PromotedTransaction = oletxCommittableTransaction;
			Hashtable promotedTransactionTable = TransactionManager.PromotedTransactionTable;
			lock (promotedTransactionTable)
			{
				tx.finalizedObject = new FinalizedObject(tx, oletxCommittableTransaction.Identifier);
				WeakReference weakReference = new WeakReference(tx.outcomeSource, false);
				promotedTransactionTable[oletxCommittableTransaction.Identifier] = weakReference;
			}
			TransactionManager.FireDistributedTransactionStarted(tx.outcomeSource);
			this.PromoteEnlistmentsAndOutcome(tx);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0002BEDC File Offset: 0x0002B2DC
		protected bool PromotePhaseVolatiles(InternalTransaction tx, ref VolatileEnlistmentSet volatiles, bool phase0)
		{
			if (volatiles.volatileEnlistmentCount + volatiles.dependentClones > 0)
			{
				if (phase0)
				{
					volatiles.VolatileDemux = new Phase0VolatileDemultiplexer(tx);
				}
				else
				{
					volatiles.VolatileDemux = new Phase1VolatileDemultiplexer(tx);
				}
				volatiles.VolatileDemux.oletxEnlistment = tx.PromotedTransaction.EnlistVolatile(volatiles.VolatileDemux, phase0 ? EnlistmentOptions.EnlistDuringPrepareRequired : EnlistmentOptions.None);
			}
			return true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0002BF3C File Offset: 0x0002B33C
		internal virtual bool PromoteDurable(InternalTransaction tx)
		{
			if (tx.durableEnlistment != null)
			{
				InternalEnlistment durableEnlistment = tx.durableEnlistment;
				IPromotedEnlistment promotedEnlistment = tx.PromotedTransaction.EnlistDurable(durableEnlistment.ResourceManagerIdentifier, (DurableInternalEnlistment)durableEnlistment, durableEnlistment.SinglePhaseNotification != null, EnlistmentOptions.None);
				tx.durableEnlistment.State.ChangeStatePromoted(tx.durableEnlistment, promotedEnlistment);
			}
			return true;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0002BF98 File Offset: 0x0002B398
		internal virtual void PromoteEnlistmentsAndOutcome(InternalTransaction tx)
		{
			bool flag = false;
			tx.PromotedTransaction.RealTransaction.InternalTransaction = tx;
			try
			{
				flag = this.PromotePhaseVolatiles(tx, ref tx.phase0Volatiles, true);
			}
			catch (TransactionException ex)
			{
				tx.innerException = ex;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
				return;
			}
			finally
			{
				if (!flag)
				{
					tx.PromotedTransaction.Rollback();
					tx.State.ChangeStateAbortedDuringPromotion(tx);
				}
			}
			flag = false;
			try
			{
				flag = this.PromotePhaseVolatiles(tx, ref tx.phase1Volatiles, false);
			}
			catch (TransactionException ex2)
			{
				tx.innerException = ex2;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex2);
				}
				return;
			}
			finally
			{
				if (!flag)
				{
					tx.PromotedTransaction.Rollback();
					tx.State.ChangeStateAbortedDuringPromotion(tx);
				}
			}
			flag = false;
			try
			{
				flag = this.PromoteDurable(tx);
			}
			catch (TransactionException ex3)
			{
				tx.innerException = ex3;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex3);
				}
			}
			finally
			{
				if (!flag)
				{
					tx.PromotedTransaction.Rollback();
					tx.State.ChangeStateAbortedDuringPromotion(tx);
				}
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0002C13C File Offset: 0x0002B53C
		internal override void DisposeRoot(InternalTransaction tx)
		{
			tx.State.Rollback(tx, null);
		}
	}
}
