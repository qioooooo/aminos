using System;
using System.Collections;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000030 RID: 48
	internal abstract class TransactionStateDelegatedBase : TransactionStatePromoted
	{
		// Token: 0x0600018D RID: 397 RVA: 0x0002CEC8 File Offset: 0x0002C2C8
		internal override void EnterState(InternalTransaction tx)
		{
			if (tx.outcomeSource.isoLevel == IsolationLevel.Snapshot)
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("CannotPromoteSnapshot"), null);
			}
			base.CommonEnterState(tx);
			global::System.Transactions.Oletx.OletxTransaction oletxTransaction = null;
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose && tx.durableEnlistment != null)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.durableEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.Promote);
				}
				oletxTransaction = TransactionState._TransactionStatePSPEOperation.PSPEPromote(tx);
			}
			catch (TransactionPromotionException ex)
			{
				tx.innerException = ex;
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
			}
			finally
			{
				if (oletxTransaction == null)
				{
					tx.State.ChangeStateAbortedDuringPromotion(tx);
				}
			}
			if (oletxTransaction == null)
			{
				return;
			}
			tx.PromotedTransaction = oletxTransaction;
			Hashtable promotedTransactionTable = TransactionManager.PromotedTransactionTable;
			lock (promotedTransactionTable)
			{
				tx.finalizedObject = new FinalizedObject(tx, tx.PromotedTransaction.Identifier);
				WeakReference weakReference = new WeakReference(tx.outcomeSource, false);
				promotedTransactionTable[tx.PromotedTransaction.Identifier] = weakReference;
			}
			TransactionManager.FireDistributedTransactionStarted(tx.outcomeSource);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.TransactionPromotedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId, oletxTransaction.TransactionTraceId);
			}
			this.PromoteEnlistmentsAndOutcome(tx);
		}
	}
}
