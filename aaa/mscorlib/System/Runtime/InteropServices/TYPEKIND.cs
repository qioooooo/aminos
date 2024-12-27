using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000537 RID: 1335
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum TYPEKIND
	{
		// Token: 0x04001A13 RID: 6675
		TKIND_ENUM,
		// Token: 0x04001A14 RID: 6676
		TKIND_RECORD,
		// Token: 0x04001A15 RID: 6677
		TKIND_MODULE,
		// Token: 0x04001A16 RID: 6678
		TKIND_INTERFACE,
		// Token: 0x04001A17 RID: 6679
		TKIND_DISPATCH,
		// Token: 0x04001A18 RID: 6680
		TKIND_COCLASS,
		// Token: 0x04001A19 RID: 6681
		TKIND_ALIAS,
		// Token: 0x04001A1A RID: 6682
		TKIND_UNION,
		// Token: 0x04001A1B RID: 6683
		TKIND_MAX
	}
}
