using System;
using System.Collections;

namespace System.Transactions.Oletx
{
	// Token: 0x02000095 RID: 149
	internal abstract class OletxVolatileEnlistmentContainer
	{
		// Token: 0x060003F7 RID: 1015
		internal abstract void DecrementOutstandingNotifications(bool voteYes);

		// Token: 0x060003F8 RID: 1016
		internal abstract void AddDependentClone();

		// Token: 0x060003F9 RID: 1017
		internal abstract void DependentCloneCompleted();

		// Token: 0x060003FA RID: 1018
		internal abstract void RollbackFromTransaction();

		// Token: 0x060003FB RID: 1019
		internal abstract void OutcomeFromTransaction(TransactionStatus outcome);

		// Token: 0x060003FC RID: 1020
		internal abstract void Committed();

		// Token: 0x060003FD RID: 1021
		internal abstract void Aborted();

		// Token: 0x060003FE RID: 1022
		internal abstract void InDoubt();

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00039198 File Offset: 0x00038598
		internal Guid TransactionIdentifier
		{
			get
			{
				return this.realOletxTransaction.Identifier;
			}
		}

		// Token: 0x0400022B RID: 555
		protected RealOletxTransaction realOletxTransaction;

		// Token: 0x0400022C RID: 556
		protected ArrayList enlistmentList;

		// Token: 0x0400022D RID: 557
		protected int phase;

		// Token: 0x0400022E RID: 558
		protected int outstandingNotifications;

		// Token: 0x0400022F RID: 559
		protected bool collectedVoteYes;

		// Token: 0x04000230 RID: 560
		protected int incompleteDependentClones;

		// Token: 0x04000231 RID: 561
		protected bool alreadyVoted;
	}
}
