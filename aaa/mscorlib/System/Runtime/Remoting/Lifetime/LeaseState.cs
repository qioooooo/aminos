using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006F7 RID: 1783
	[ComVisible(true)]
	[Serializable]
	public enum LeaseState
	{
		// Token: 0x04002012 RID: 8210
		Null,
		// Token: 0x04002013 RID: 8211
		Initial,
		// Token: 0x04002014 RID: 8212
		Active,
		// Token: 0x04002015 RID: 8213
		Renewing,
		// Token: 0x04002016 RID: 8214
		Expired
	}
}
