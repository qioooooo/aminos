using System;

namespace System.Transactions
{
	// Token: 0x0200003D RID: 61
	internal class PromotableInternalEnlistment : InternalEnlistment
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x0002D9E8 File Offset: 0x0002CDE8
		internal PromotableInternalEnlistment(Enlistment enlistment, InternalTransaction transaction, IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Transaction atomicTransaction)
			: base(enlistment, transaction, atomicTransaction)
		{
			this.promotableNotificationInterface = promotableSinglePhaseNotification;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0002DA08 File Offset: 0x0002CE08
		internal override IPromotableSinglePhaseNotification PromotableSinglePhaseNotification
		{
			get
			{
				return this.promotableNotificationInterface;
			}
		}

		// Token: 0x040000F0 RID: 240
		private IPromotableSinglePhaseNotification promotableNotificationInterface;
	}
}
