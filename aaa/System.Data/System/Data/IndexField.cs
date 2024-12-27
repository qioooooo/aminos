using System;

namespace System.Data
{
	// Token: 0x020000DB RID: 219
	internal struct IndexField
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x001FE4E8 File Offset: 0x001FD8E8
		internal IndexField(DataColumn column, bool isDescending)
		{
			this.Column = column;
			this.IsDescending = isDescending;
		}

		// Token: 0x040008FC RID: 2300
		public readonly DataColumn Column;

		// Token: 0x040008FD RID: 2301
		public readonly bool IsDescending;
	}
}
