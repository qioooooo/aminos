using System;

namespace System.Net
{
	// Token: 0x02000408 RID: 1032
	internal struct SecurityBufferStruct
	{
		// Token: 0x0400208A RID: 8330
		public int count;

		// Token: 0x0400208B RID: 8331
		public BufferType type;

		// Token: 0x0400208C RID: 8332
		public IntPtr token;

		// Token: 0x0400208D RID: 8333
		public static readonly int Size = sizeof(SecurityBufferStruct);
	}
}
