using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000571 RID: 1393
	[Flags]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04001B23 RID: 6947
		PARAMFLAG_NONE = 0,
		// Token: 0x04001B24 RID: 6948
		PARAMFLAG_FIN = 1,
		// Token: 0x04001B25 RID: 6949
		PARAMFLAG_FOUT = 2,
		// Token: 0x04001B26 RID: 6950
		PARAMFLAG_FLCID = 4,
		// Token: 0x04001B27 RID: 6951
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04001B28 RID: 6952
		PARAMFLAG_FOPT = 16,
		// Token: 0x04001B29 RID: 6953
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04001B2A RID: 6954
		PARAMFLAG_FHASCUSTDATA = 64
	}
}
