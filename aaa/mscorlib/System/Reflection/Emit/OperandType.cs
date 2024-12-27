using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082B RID: 2091
	[ComVisible(true)]
	[Serializable]
	public enum OperandType
	{
		// Token: 0x0400275D RID: 10077
		InlineBrTarget,
		// Token: 0x0400275E RID: 10078
		InlineField,
		// Token: 0x0400275F RID: 10079
		InlineI,
		// Token: 0x04002760 RID: 10080
		InlineI8,
		// Token: 0x04002761 RID: 10081
		InlineMethod,
		// Token: 0x04002762 RID: 10082
		InlineNone,
		// Token: 0x04002763 RID: 10083
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		InlinePhi,
		// Token: 0x04002764 RID: 10084
		InlineR,
		// Token: 0x04002765 RID: 10085
		InlineSig = 9,
		// Token: 0x04002766 RID: 10086
		InlineString,
		// Token: 0x04002767 RID: 10087
		InlineSwitch,
		// Token: 0x04002768 RID: 10088
		InlineTok,
		// Token: 0x04002769 RID: 10089
		InlineType,
		// Token: 0x0400276A RID: 10090
		InlineVar,
		// Token: 0x0400276B RID: 10091
		ShortInlineBrTarget,
		// Token: 0x0400276C RID: 10092
		ShortInlineI,
		// Token: 0x0400276D RID: 10093
		ShortInlineR,
		// Token: 0x0400276E RID: 10094
		ShortInlineVar
	}
}
