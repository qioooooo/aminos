using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057D RID: 1405
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04001B59 RID: 7001
		CC_CDECL = 1,
		// Token: 0x04001B5A RID: 7002
		CC_MSCPASCAL,
		// Token: 0x04001B5B RID: 7003
		CC_PASCAL = 2,
		// Token: 0x04001B5C RID: 7004
		CC_MACPASCAL,
		// Token: 0x04001B5D RID: 7005
		CC_STDCALL,
		// Token: 0x04001B5E RID: 7006
		CC_RESERVED,
		// Token: 0x04001B5F RID: 7007
		CC_SYSCALL,
		// Token: 0x04001B60 RID: 7008
		CC_MPWCDECL,
		// Token: 0x04001B61 RID: 7009
		CC_MPWPASCAL,
		// Token: 0x04001B62 RID: 7010
		CC_MAX
	}
}
