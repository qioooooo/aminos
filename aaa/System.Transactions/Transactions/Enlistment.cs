using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200003F RID: 63
	public class Enlistment
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0002DABC File Offset: 0x0002CEBC
		internal Enlistment(InternalEnlistment internalEnlistment)
		{
			this.internalEnlistment = internalEnlistment;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0002DAD8 File Offset: 0x0002CED8
		internal Enlistment(Guid resourceManagerIdentifier, InternalTransaction transaction, IEnlistmentNotification twoPhaseNotifications, ISinglePhaseNotification singlePhaseNotifications, Transaction atomicTransaction)
		{
			this.internalEnlistment = new DurableInternalEnlistment(this, resourceManagerIdentifier, transaction, twoPhaseNotifications, singlePhaseNotifications, atomicTransaction);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0002DB00 File Offset: 0x0002CF00
		internal Enlistment(InternalTransaction transaction, IEnlistmentNotification twoPhaseNotifications, ISinglePhaseNotification singlePhaseNotifications, Transaction atomicTransaction, EnlistmentOptions enlistmentOptions)
		{
			if ((enlistmentOptions & EnlistmentOptions.EnlistDuringPrepareRequired) != EnlistmentOptions.None)
			{
				this.internalEnlistment = new InternalEnlistment(this, transaction, twoPhaseNotifications, singlePhaseNotifications, atomicTransaction);
				return;
			}
			this.internalEnlistment = new Phase1VolatileEnlistment(this, transaction, twoPhaseNotifications, singlePhaseNotifications, atomicTransaction);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0002DB3C File Offset: 0x0002CF3C
		internal Enlistment(InternalTransaction transaction, IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Transaction atomicTransaction)
		{
			this.internalEnlistment = new PromotableInternalEnlistment(this, transaction, promotableSinglePhaseNotification, atomicTransaction);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0002DB60 File Offset: 0x0002CF60
		internal Enlistment(IEnlistmentNotification twoPhaseNotifications, InternalTransaction transaction, Transaction atomicTransaction)
		{
			this.internalEnlistment = new InternalEnlistment(this, twoPhaseNotifications, transaction, atomicTransaction);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0002DB84 File Offset: 0x0002CF84
		internal Enlistment(IEnlistmentNotification twoPhaseNotifications, object syncRoot)
		{
			this.internalEnlistment = new RecoveringInternalEnlistment(this, twoPhaseNotifications, syncRoot);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0002DBA8 File Offset: 0x0002CFA8
		public void Done()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Enlistment.Done");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.Done);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.EnlistmentDone(this.internalEnlistment);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "Enlistment.Done");
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0002DC54 File Offset: 0x0002D054
		internal InternalEnlistment InternalEnlistment
		{
			get
			{
				return this.internalEnlistment;
			}
		}

		// Token: 0x040000F1 RID: 241
		internal InternalEnlistment internalEnlistment;
	}
}
