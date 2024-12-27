using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054B RID: 1355
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum VARFLAGS : short
	{
		// Token: 0x04001AA1 RID: 6817
		VARFLAG_FREADONLY = 1,
		// Token: 0x04001AA2 RID: 6818
		VARFLAG_FSOURCE = 2,
		// Token: 0x04001AA3 RID: 6819
		VARFLAG_FBINDABLE = 4,
		// Token: 0x04001AA4 RID: 6820
		VARFLAG_FREQUESTEDIT = 8,
		// Token: 0x04001AA5 RID: 6821
		VARFLAG_FDISPLAYBIND = 16,
		// Token: 0x04001AA6 RID: 6822
		VARFLAG_FDEFAULTBIND = 32,
		// Token: 0x04001AA7 RID: 6823
		VARFLAG_FHIDDEN = 64,
		// Token: 0x04001AA8 RID: 6824
		VARFLAG_FRESTRICTED = 128,
		// Token: 0x04001AA9 RID: 6825
		VARFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04001AAA RID: 6826
		VARFLAG_FUIDEFAULT = 512,
		// Token: 0x04001AAB RID: 6827
		VARFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04001AAC RID: 6828
		VARFLAG_FREPLACEABLE = 2048,
		// Token: 0x04001AAD RID: 6829
		VARFLAG_FIMMEDIATEBIND = 4096
	}
}
