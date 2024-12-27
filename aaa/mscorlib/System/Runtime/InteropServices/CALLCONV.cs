using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000549 RID: 1353
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CALLCONV instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04001A88 RID: 6792
		CC_CDECL = 1,
		// Token: 0x04001A89 RID: 6793
		CC_MSCPASCAL,
		// Token: 0x04001A8A RID: 6794
		CC_PASCAL = 2,
		// Token: 0x04001A8B RID: 6795
		CC_MACPASCAL,
		// Token: 0x04001A8C RID: 6796
		CC_STDCALL,
		// Token: 0x04001A8D RID: 6797
		CC_RESERVED,
		// Token: 0x04001A8E RID: 6798
		CC_SYSCALL,
		// Token: 0x04001A8F RID: 6799
		CC_MPWCDECL,
		// Token: 0x04001A90 RID: 6800
		CC_MPWPASCAL,
		// Token: 0x04001A91 RID: 6801
		CC_MAX
	}
}
