using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x0200049D RID: 1181
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum PolicyStatementAttribute
	{
		// Token: 0x04001800 RID: 6144
		Nothing = 0,
		// Token: 0x04001801 RID: 6145
		Exclusive = 1,
		// Token: 0x04001802 RID: 6146
		LevelFinal = 2,
		// Token: 0x04001803 RID: 6147
		All = 3
	}
}
