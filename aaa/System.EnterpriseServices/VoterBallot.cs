using System;
using System.Runtime.InteropServices;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x02000047 RID: 71
	internal class VoterBallot : ITransactionVoterBallotAsync2, IEnlistmentNotification
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00005D10 File Offset: 0x00004D10
		internal VoterBallot(ITransactionVoterNotifyAsync2 notification, Transaction transaction)
		{
			this.transaction = transaction;
			this.notification = notification;
			this.enlistment = transaction.EnlistVolatile(this, EnlistmentOptions.None);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005D34 File Offset: 0x00004D34
		public void Prepare(PreparingEnlistment enlistment)
		{
			this.preparingEnlistment = enlistment;
			this.notification.VoteRequest();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005D48 File Offset: 0x00004D48
		public void Rollback(Enlistment enlistment)
		{
			enlistment.Done();
			this.notification.Aborted(0, false, 0, 0);
			Marshal.ReleaseComObject(this.notification);
			this.notification = null;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005D72 File Offset: 0x00004D72
		public void Commit(Enlistment enlistment)
		{
			enlistment.Done();
			this.notification.Committed(false, 0, 0);
			Marshal.ReleaseComObject(this.notification);
			this.notification = null;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005D9B File Offset: 0x00004D9B
		public void InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
			this.notification.InDoubt();
			Marshal.ReleaseComObject(this.notification);
			this.notification = null;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005DC1 File Offset: 0x00004DC1
		public void VoteRequestDone(int hr, int reason)
		{
			if (this.preparingEnlistment == null)
			{
				Marshal.ThrowExceptionForHR(Util.E_FAIL);
			}
			if (hr == 0)
			{
				this.preparingEnlistment.Prepared();
				return;
			}
			this.preparingEnlistment.ForceRollback();
		}

		// Token: 0x04000091 RID: 145
		private const int S_OK = 0;

		// Token: 0x04000092 RID: 146
		private ITransactionVoterNotifyAsync2 notification;

		// Token: 0x04000093 RID: 147
		private Transaction transaction;

		// Token: 0x04000094 RID: 148
		private Enlistment enlistment;

		// Token: 0x04000095 RID: 149
		private PreparingEnlistment preparingEnlistment;
	}
}
