using System;

namespace System.Transactions
{
	// Token: 0x02000037 RID: 55
	internal interface IPromotedEnlistment
	{
		// Token: 0x060001A7 RID: 423
		void EnlistmentDone();

		// Token: 0x060001A8 RID: 424
		void Prepared();

		// Token: 0x060001A9 RID: 425
		void ForceRollback();

		// Token: 0x060001AA RID: 426
		void ForceRollback(Exception e);

		// Token: 0x060001AB RID: 427
		void Committed();

		// Token: 0x060001AC RID: 428
		void Aborted();

		// Token: 0x060001AD RID: 429
		void Aborted(Exception e);

		// Token: 0x060001AE RID: 430
		void InDoubt();

		// Token: 0x060001AF RID: 431
		void InDoubt(Exception e);

		// Token: 0x060001B0 RID: 432
		byte[] GetRecoveryInformation();

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001B1 RID: 433
		// (set) Token: 0x060001B2 RID: 434
		InternalEnlistment InternalEnlistment { get; set; }
	}
}
