using System;

namespace Microsoft.JScript
{
	// Token: 0x020000F4 RID: 244
	internal enum OpPrec
	{
		// Token: 0x04000685 RID: 1669
		precNone,
		// Token: 0x04000686 RID: 1670
		precSeqEval,
		// Token: 0x04000687 RID: 1671
		precAssignment,
		// Token: 0x04000688 RID: 1672
		precConditional,
		// Token: 0x04000689 RID: 1673
		precLogicalOr,
		// Token: 0x0400068A RID: 1674
		precLogicalAnd,
		// Token: 0x0400068B RID: 1675
		precBitwiseOr,
		// Token: 0x0400068C RID: 1676
		precBitwiseXor,
		// Token: 0x0400068D RID: 1677
		precBitwiseAnd,
		// Token: 0x0400068E RID: 1678
		precEquality,
		// Token: 0x0400068F RID: 1679
		precRelational,
		// Token: 0x04000690 RID: 1680
		precShift,
		// Token: 0x04000691 RID: 1681
		precAdditive,
		// Token: 0x04000692 RID: 1682
		precMultiplicative
	}
}
