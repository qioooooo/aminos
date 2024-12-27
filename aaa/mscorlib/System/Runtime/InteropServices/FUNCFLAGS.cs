﻿using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054A RID: 1354
	[Flags]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum FUNCFLAGS : short
	{
		// Token: 0x04001A93 RID: 6803
		FUNCFLAG_FRESTRICTED = 1,
		// Token: 0x04001A94 RID: 6804
		FUNCFLAG_FSOURCE = 2,
		// Token: 0x04001A95 RID: 6805
		FUNCFLAG_FBINDABLE = 4,
		// Token: 0x04001A96 RID: 6806
		FUNCFLAG_FREQUESTEDIT = 8,
		// Token: 0x04001A97 RID: 6807
		FUNCFLAG_FDISPLAYBIND = 16,
		// Token: 0x04001A98 RID: 6808
		FUNCFLAG_FDEFAULTBIND = 32,
		// Token: 0x04001A99 RID: 6809
		FUNCFLAG_FHIDDEN = 64,
		// Token: 0x04001A9A RID: 6810
		FUNCFLAG_FUSESGETLASTERROR = 128,
		// Token: 0x04001A9B RID: 6811
		FUNCFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04001A9C RID: 6812
		FUNCFLAG_FUIDEFAULT = 512,
		// Token: 0x04001A9D RID: 6813
		FUNCFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04001A9E RID: 6814
		FUNCFLAG_FREPLACEABLE = 2048,
		// Token: 0x04001A9F RID: 6815
		FUNCFLAG_FIMMEDIATEBIND = 4096
	}
}
