using System;

namespace System.Data
{
	// Token: 0x020000B9 RID: 185
	public interface IDataAdapter
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000C69 RID: 3177
		// (set) Token: 0x06000C6A RID: 3178
		MissingMappingAction MissingMappingAction { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000C6B RID: 3179
		// (set) Token: 0x06000C6C RID: 3180
		MissingSchemaAction MissingSchemaAction { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000C6D RID: 3181
		ITableMappingCollection TableMappings { get; }

		// Token: 0x06000C6E RID: 3182
		DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType);

		// Token: 0x06000C6F RID: 3183
		int Fill(DataSet dataSet);

		// Token: 0x06000C70 RID: 3184
		IDataParameter[] GetFillParameters();

		// Token: 0x06000C71 RID: 3185
		int Update(DataSet dataSet);
	}
}
