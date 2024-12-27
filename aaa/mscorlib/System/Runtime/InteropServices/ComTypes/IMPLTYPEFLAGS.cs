using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056C RID: 1388
	[Flags]
	[Serializable]
	public enum IMPLTYPEFLAGS
	{
		// Token: 0x04001AF7 RID: 6903
		IMPLTYPEFLAG_FDEFAULT = 1,
		// Token: 0x04001AF8 RID: 6904
		IMPLTYPEFLAG_FSOURCE = 2,
		// Token: 0x04001AF9 RID: 6905
		IMPLTYPEFLAG_FRESTRICTED = 4,
		// Token: 0x04001AFA RID: 6906
		IMPLTYPEFLAG_FDEFAULTVTABLE = 8
	}
}
