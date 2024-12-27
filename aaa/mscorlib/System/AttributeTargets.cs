using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000074 RID: 116
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum AttributeTargets
	{
		// Token: 0x040001FF RID: 511
		Assembly = 1,
		// Token: 0x04000200 RID: 512
		Module = 2,
		// Token: 0x04000201 RID: 513
		Class = 4,
		// Token: 0x04000202 RID: 514
		Struct = 8,
		// Token: 0x04000203 RID: 515
		Enum = 16,
		// Token: 0x04000204 RID: 516
		Constructor = 32,
		// Token: 0x04000205 RID: 517
		Method = 64,
		// Token: 0x04000206 RID: 518
		Property = 128,
		// Token: 0x04000207 RID: 519
		Field = 256,
		// Token: 0x04000208 RID: 520
		Event = 512,
		// Token: 0x04000209 RID: 521
		Interface = 1024,
		// Token: 0x0400020A RID: 522
		Parameter = 2048,
		// Token: 0x0400020B RID: 523
		Delegate = 4096,
		// Token: 0x0400020C RID: 524
		ReturnValue = 8192,
		// Token: 0x0400020D RID: 525
		GenericParameter = 16384,
		// Token: 0x0400020E RID: 526
		All = 32767
	}
}
