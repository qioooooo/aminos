using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200031F RID: 799
	[ComVisible(true)]
	[Flags]
	public enum ExceptionHandlingClauseOptions
	{
		// Token: 0x04000D3A RID: 3386
		Clause = 0,
		// Token: 0x04000D3B RID: 3387
		Filter = 1,
		// Token: 0x04000D3C RID: 3388
		Finally = 2,
		// Token: 0x04000D3D RID: 3389
		Fault = 4
	}
}
