using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054E RID: 1358
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.LIBFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04001AB3 RID: 6835
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04001AB4 RID: 6836
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04001AB5 RID: 6837
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04001AB6 RID: 6838
		LIBFLAG_FHASDISKIMAGE = 8
	}
}
