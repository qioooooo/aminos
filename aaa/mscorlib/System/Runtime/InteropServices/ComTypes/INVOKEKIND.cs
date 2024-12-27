using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057C RID: 1404
	[Flags]
	[Serializable]
	public enum INVOKEKIND
	{
		// Token: 0x04001B54 RID: 6996
		INVOKE_FUNC = 1,
		// Token: 0x04001B55 RID: 6997
		INVOKE_PROPERTYGET = 2,
		// Token: 0x04001B56 RID: 6998
		INVOKE_PROPERTYPUT = 4,
		// Token: 0x04001B57 RID: 6999
		INVOKE_PROPERTYPUTREF = 8
	}
}
