using System;

namespace System.Management
{
	// Token: 0x02000006 RID: 6
	public enum CimType
	{
		// Token: 0x04000050 RID: 80
		None,
		// Token: 0x04000051 RID: 81
		SInt8 = 16,
		// Token: 0x04000052 RID: 82
		UInt8,
		// Token: 0x04000053 RID: 83
		SInt16 = 2,
		// Token: 0x04000054 RID: 84
		UInt16 = 18,
		// Token: 0x04000055 RID: 85
		SInt32 = 3,
		// Token: 0x04000056 RID: 86
		UInt32 = 19,
		// Token: 0x04000057 RID: 87
		SInt64,
		// Token: 0x04000058 RID: 88
		UInt64,
		// Token: 0x04000059 RID: 89
		Real32 = 4,
		// Token: 0x0400005A RID: 90
		Real64,
		// Token: 0x0400005B RID: 91
		Boolean = 11,
		// Token: 0x0400005C RID: 92
		String = 8,
		// Token: 0x0400005D RID: 93
		DateTime = 101,
		// Token: 0x0400005E RID: 94
		Reference,
		// Token: 0x0400005F RID: 95
		Char16,
		// Token: 0x04000060 RID: 96
		Object = 13
	}
}
