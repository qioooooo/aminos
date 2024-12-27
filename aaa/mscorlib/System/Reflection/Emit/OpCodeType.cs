using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000829 RID: 2089
	[ComVisible(true)]
	[Serializable]
	public enum OpCodeType
	{
		// Token: 0x04002738 RID: 10040
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		Annotation,
		// Token: 0x04002739 RID: 10041
		Macro,
		// Token: 0x0400273A RID: 10042
		Nternal,
		// Token: 0x0400273B RID: 10043
		Objmodel,
		// Token: 0x0400273C RID: 10044
		Prefix,
		// Token: 0x0400273D RID: 10045
		Primitive
	}
}
