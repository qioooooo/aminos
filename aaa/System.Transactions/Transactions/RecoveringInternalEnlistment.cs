using System;

namespace System.Transactions
{
	// Token: 0x0200003C RID: 60
	internal class RecoveringInternalEnlistment : DurableInternalEnlistment
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0002D9B8 File Offset: 0x0002CDB8
		internal RecoveringInternalEnlistment(Enlistment enlistment, IEnlistmentNotification twoPhaseNotifications, object syncRoot)
			: base(enlistment, twoPhaseNotifications)
		{
			this.syncRoot = syncRoot;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0002D9D4 File Offset: 0x0002CDD4
		internal override object SyncRoot
		{
			get
			{
				return this.syncRoot;
			}
		}

		// Token: 0x040000EF RID: 239
		private object syncRoot;
	}
}
