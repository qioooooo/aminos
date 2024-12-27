using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000136 RID: 310
	[Guid("581AD3D9-2BAA-3770-B92B-38607E1B463A")]
	[ComVisible(true)]
	public enum VSAITEMTYPE2
	{
		// Token: 0x04000788 RID: 1928
		None,
		// Token: 0x04000789 RID: 1929
		HOSTOBJECT = 16,
		// Token: 0x0400078A RID: 1930
		HOSTSCOPE,
		// Token: 0x0400078B RID: 1931
		HOSTSCOPEANDOBJECT,
		// Token: 0x0400078C RID: 1932
		SCRIPTSCOPE,
		// Token: 0x0400078D RID: 1933
		SCRIPTBLOCK,
		// Token: 0x0400078E RID: 1934
		STATEMENT,
		// Token: 0x0400078F RID: 1935
		EXPRESSION
	}
}
