using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000680 RID: 1664
	[ComVisible(true)]
	[Serializable]
	public enum SecurityZone
	{
		// Token: 0x04001F06 RID: 7942
		MyComputer,
		// Token: 0x04001F07 RID: 7943
		Intranet,
		// Token: 0x04001F08 RID: 7944
		Trusted,
		// Token: 0x04001F09 RID: 7945
		Internet,
		// Token: 0x04001F0A RID: 7946
		Untrusted,
		// Token: 0x04001F0B RID: 7947
		NoZone = -1
	}
}
