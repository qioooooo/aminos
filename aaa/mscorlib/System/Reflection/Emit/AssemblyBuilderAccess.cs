using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x020007EE RID: 2030
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum AssemblyBuilderAccess
	{
		// Token: 0x04002536 RID: 9526
		Run = 1,
		// Token: 0x04002537 RID: 9527
		Save = 2,
		// Token: 0x04002538 RID: 9528
		RunAndSave = 3,
		// Token: 0x04002539 RID: 9529
		ReflectionOnly = 6
	}
}
