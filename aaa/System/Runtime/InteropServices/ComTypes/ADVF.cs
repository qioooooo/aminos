using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200026B RID: 619
	[Flags]
	public enum ADVF
	{
		// Token: 0x0400122C RID: 4652
		ADVF_NODATA = 1,
		// Token: 0x0400122D RID: 4653
		ADVF_PRIMEFIRST = 2,
		// Token: 0x0400122E RID: 4654
		ADVF_ONLYONCE = 4,
		// Token: 0x0400122F RID: 4655
		ADVF_DATAONSTOP = 64,
		// Token: 0x04001230 RID: 4656
		ADVFCACHE_NOHANDLER = 8,
		// Token: 0x04001231 RID: 4657
		ADVFCACHE_FORCEBUILTIN = 16,
		// Token: 0x04001232 RID: 4658
		ADVFCACHE_ONSAVE = 32
	}
}
