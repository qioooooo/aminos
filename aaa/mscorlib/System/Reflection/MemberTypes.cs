using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000316 RID: 790
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MemberTypes
	{
		// Token: 0x04000CE9 RID: 3305
		Constructor = 1,
		// Token: 0x04000CEA RID: 3306
		Event = 2,
		// Token: 0x04000CEB RID: 3307
		Field = 4,
		// Token: 0x04000CEC RID: 3308
		Method = 8,
		// Token: 0x04000CED RID: 3309
		Property = 16,
		// Token: 0x04000CEE RID: 3310
		TypeInfo = 32,
		// Token: 0x04000CEF RID: 3311
		Custom = 64,
		// Token: 0x04000CF0 RID: 3312
		NestedType = 128,
		// Token: 0x04000CF1 RID: 3313
		All = 191
	}
}
