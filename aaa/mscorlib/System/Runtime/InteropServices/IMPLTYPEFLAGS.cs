using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000539 RID: 1337
	[Flags]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum IMPLTYPEFLAGS
	{
		// Token: 0x04001A2D RID: 6701
		IMPLTYPEFLAG_FDEFAULT = 1,
		// Token: 0x04001A2E RID: 6702
		IMPLTYPEFLAG_FSOURCE = 2,
		// Token: 0x04001A2F RID: 6703
		IMPLTYPEFLAG_FRESTRICTED = 4,
		// Token: 0x04001A30 RID: 6704
		IMPLTYPEFLAG_FDEFAULTVTABLE = 8
	}
}
