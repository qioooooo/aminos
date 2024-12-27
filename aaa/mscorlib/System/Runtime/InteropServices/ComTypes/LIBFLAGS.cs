using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000582 RID: 1410
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04001B85 RID: 7045
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04001B86 RID: 7046
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04001B87 RID: 7047
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04001B88 RID: 7048
		LIBFLAG_FHASDISKIMAGE = 8
	}
}
