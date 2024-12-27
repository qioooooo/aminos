using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000834 RID: 2100
	[ComVisible(true)]
	[Serializable]
	public enum PackingSize
	{
		// Token: 0x040027CE RID: 10190
		Unspecified,
		// Token: 0x040027CF RID: 10191
		Size1,
		// Token: 0x040027D0 RID: 10192
		Size2,
		// Token: 0x040027D1 RID: 10193
		Size4 = 4,
		// Token: 0x040027D2 RID: 10194
		Size8 = 8,
		// Token: 0x040027D3 RID: 10195
		Size16 = 16,
		// Token: 0x040027D4 RID: 10196
		Size32 = 32,
		// Token: 0x040027D5 RID: 10197
		Size64 = 64,
		// Token: 0x040027D6 RID: 10198
		Size128 = 128
	}
}
