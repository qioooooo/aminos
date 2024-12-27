using System;

namespace System.Data
{
	// Token: 0x020000C2 RID: 194
	public interface ITableMapping
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000CB5 RID: 3253
		IColumnMappingCollection ColumnMappings { get; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000CB6 RID: 3254
		// (set) Token: 0x06000CB7 RID: 3255
		string DataSetTable { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000CB8 RID: 3256
		// (set) Token: 0x06000CB9 RID: 3257
		string SourceTable { get; set; }
	}
}
