using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000547 RID: 1351
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum FUNCKIND
	{
		// Token: 0x04001A7D RID: 6781
		FUNC_VIRTUAL,
		// Token: 0x04001A7E RID: 6782
		FUNC_PUREVIRTUAL,
		// Token: 0x04001A7F RID: 6783
		FUNC_NONVIRTUAL,
		// Token: 0x04001A80 RID: 6784
		FUNC_STATIC,
		// Token: 0x04001A81 RID: 6785
		FUNC_DISPATCH
	}
}
