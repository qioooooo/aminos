using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007F RID: 127
	[EditorBrowsable(EditorBrowsableState.Never)]
	public enum SetIteratorResult
	{
		// Token: 0x0400049E RID: 1182
		NoMoreNodes,
		// Token: 0x0400049F RID: 1183
		InitRightIterator,
		// Token: 0x040004A0 RID: 1184
		NeedLeftNode,
		// Token: 0x040004A1 RID: 1185
		NeedRightNode,
		// Token: 0x040004A2 RID: 1186
		HaveCurrentNode
	}
}
