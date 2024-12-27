using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053E RID: 1342
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.PARAMFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04001A59 RID: 6745
		PARAMFLAG_NONE = 0,
		// Token: 0x04001A5A RID: 6746
		PARAMFLAG_FIN = 1,
		// Token: 0x04001A5B RID: 6747
		PARAMFLAG_FOUT = 2,
		// Token: 0x04001A5C RID: 6748
		PARAMFLAG_FLCID = 4,
		// Token: 0x04001A5D RID: 6749
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04001A5E RID: 6750
		PARAMFLAG_FOPT = 16,
		// Token: 0x04001A5F RID: 6751
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04001A60 RID: 6752
		PARAMFLAG_FHASCUSTDATA = 64
	}
}
