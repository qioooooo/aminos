using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000534 RID: 1332
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.DESCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum DESCKIND
	{
		// Token: 0x04001A09 RID: 6665
		DESCKIND_NONE,
		// Token: 0x04001A0A RID: 6666
		DESCKIND_FUNCDESC,
		// Token: 0x04001A0B RID: 6667
		DESCKIND_VARDESC,
		// Token: 0x04001A0C RID: 6668
		DESCKIND_TYPECOMP,
		// Token: 0x04001A0D RID: 6669
		DESCKIND_IMPLICITAPPOBJ,
		// Token: 0x04001A0E RID: 6670
		DESCKIND_MAX
	}
}
