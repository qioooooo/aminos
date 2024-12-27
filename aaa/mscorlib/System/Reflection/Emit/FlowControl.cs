using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082C RID: 2092
	[ComVisible(true)]
	[Serializable]
	public enum FlowControl
	{
		// Token: 0x04002770 RID: 10096
		Branch,
		// Token: 0x04002771 RID: 10097
		Break,
		// Token: 0x04002772 RID: 10098
		Call,
		// Token: 0x04002773 RID: 10099
		Cond_Branch,
		// Token: 0x04002774 RID: 10100
		Meta,
		// Token: 0x04002775 RID: 10101
		Next,
		// Token: 0x04002776 RID: 10102
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		Phi,
		// Token: 0x04002777 RID: 10103
		Return,
		// Token: 0x04002778 RID: 10104
		Throw
	}
}
