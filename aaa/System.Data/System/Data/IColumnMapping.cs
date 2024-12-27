using System;

namespace System.Data
{
	// Token: 0x020000B7 RID: 183
	public interface IColumnMapping
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000C5E RID: 3166
		// (set) Token: 0x06000C5F RID: 3167
		string DataSetColumn { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000C60 RID: 3168
		// (set) Token: 0x06000C61 RID: 3169
		string SourceColumn { get; set; }
	}
}
