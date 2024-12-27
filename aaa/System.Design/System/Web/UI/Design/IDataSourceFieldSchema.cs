using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200034D RID: 845
	public interface IDataSourceFieldSchema
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001FC5 RID: 8133
		Type DataType { get; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001FC6 RID: 8134
		bool Identity { get; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001FC7 RID: 8135
		bool IsReadOnly { get; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001FC8 RID: 8136
		bool IsUnique { get; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001FC9 RID: 8137
		int Length { get; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001FCA RID: 8138
		string Name { get; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001FCB RID: 8139
		bool Nullable { get; }

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001FCC RID: 8140
		int Precision { get; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001FCD RID: 8141
		bool PrimaryKey { get; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001FCE RID: 8142
		int Scale { get; }
	}
}
