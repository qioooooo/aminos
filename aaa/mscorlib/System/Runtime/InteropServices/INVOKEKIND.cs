using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000548 RID: 1352
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.INVOKEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum INVOKEKIND
	{
		// Token: 0x04001A83 RID: 6787
		INVOKE_FUNC = 1,
		// Token: 0x04001A84 RID: 6788
		INVOKE_PROPERTYGET,
		// Token: 0x04001A85 RID: 6789
		INVOKE_PROPERTYPUT = 4,
		// Token: 0x04001A86 RID: 6790
		INVOKE_PROPERTYPUTREF = 8
	}
}
