using System;

namespace System.Data.Common
{
	// Token: 0x02000145 RID: 325
	internal sealed class DbSchemaTable
	{
		// Token: 0x06001515 RID: 5397 RVA: 0x002293C0 File Offset: 0x002287C0
		internal DbSchemaTable(DataTable dataTable, bool returnProviderSpecificTypes)
		{
			this.dataTable = dataTable;
			this.columns = dataTable.Columns;
			this._returnProviderSpecificTypes = returnProviderSpecificTypes;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x00229400 File Offset: 0x00228800
		internal DataColumn ColumnName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.ColumnName);
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x00229414 File Offset: 0x00228814
		internal DataColumn Size
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.ColumnSize);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x00229428 File Offset: 0x00228828
		internal DataColumn BaseServerName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.BaseServerName);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0022943C File Offset: 0x0022883C
		internal DataColumn BaseColumnName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.BaseColumnName);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x00229450 File Offset: 0x00228850
		internal DataColumn BaseTableName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.BaseTableName);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x00229464 File Offset: 0x00228864
		internal DataColumn BaseCatalogName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.BaseCatalogName);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x00229478 File Offset: 0x00228878
		internal DataColumn BaseSchemaName
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.BaseSchemaName);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0022948C File Offset: 0x0022888C
		internal DataColumn IsAutoIncrement
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsAutoIncrement);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x002294A0 File Offset: 0x002288A0
		internal DataColumn IsUnique
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsUnique);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x002294B8 File Offset: 0x002288B8
		internal DataColumn IsKey
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsKey);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x002294D0 File Offset: 0x002288D0
		internal DataColumn IsRowVersion
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsRowVersion);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x002294E8 File Offset: 0x002288E8
		internal DataColumn AllowDBNull
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.AllowDBNull);
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x00229500 File Offset: 0x00228900
		internal DataColumn IsExpression
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsExpression);
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x00229518 File Offset: 0x00228918
		internal DataColumn IsHidden
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsHidden);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x00229530 File Offset: 0x00228930
		internal DataColumn IsLong
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsLong);
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00229548 File Offset: 0x00228948
		internal DataColumn IsReadOnly
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.IsReadOnly);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00229560 File Offset: 0x00228960
		internal DataColumn UnsortedIndex
		{
			get
			{
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.SchemaMappingUnsortedIndex);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x00229578 File Offset: 0x00228978
		internal DataColumn DataType
		{
			get
			{
				if (this._returnProviderSpecificTypes)
				{
					return this.CachedDataColumn(DbSchemaTable.ColumnEnum.ProviderSpecificDataType, DbSchemaTable.ColumnEnum.DataType);
				}
				return this.CachedDataColumn(DbSchemaTable.ColumnEnum.DataType);
			}
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x002295A0 File Offset: 0x002289A0
		private DataColumn CachedDataColumn(DbSchemaTable.ColumnEnum column)
		{
			return this.CachedDataColumn(column, column);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x002295B8 File Offset: 0x002289B8
		private DataColumn CachedDataColumn(DbSchemaTable.ColumnEnum column, DbSchemaTable.ColumnEnum column2)
		{
			DataColumn dataColumn = this.columnCache[(int)column];
			if (dataColumn == null)
			{
				int num = this.columns.IndexOf(DbSchemaTable.DBCOLUMN_NAME[(int)column]);
				if (-1 == num && column != column2)
				{
					num = this.columns.IndexOf(DbSchemaTable.DBCOLUMN_NAME[(int)column2]);
				}
				if (-1 != num)
				{
					dataColumn = this.columns[num];
					this.columnCache[(int)column] = dataColumn;
				}
			}
			return dataColumn;
		}

		// Token: 0x04000C5C RID: 3164
		private static readonly string[] DBCOLUMN_NAME = new string[]
		{
			SchemaTableColumn.ColumnName,
			SchemaTableColumn.ColumnOrdinal,
			SchemaTableColumn.ColumnSize,
			SchemaTableOptionalColumn.BaseServerName,
			SchemaTableOptionalColumn.BaseCatalogName,
			SchemaTableColumn.BaseColumnName,
			SchemaTableColumn.BaseSchemaName,
			SchemaTableColumn.BaseTableName,
			SchemaTableOptionalColumn.IsAutoIncrement,
			SchemaTableColumn.IsUnique,
			SchemaTableColumn.IsKey,
			SchemaTableOptionalColumn.IsRowVersion,
			SchemaTableColumn.DataType,
			SchemaTableOptionalColumn.ProviderSpecificDataType,
			SchemaTableColumn.AllowDBNull,
			SchemaTableColumn.ProviderType,
			SchemaTableColumn.IsExpression,
			SchemaTableOptionalColumn.IsHidden,
			SchemaTableColumn.IsLong,
			SchemaTableOptionalColumn.IsReadOnly,
			"SchemaMapping Unsorted Index"
		};

		// Token: 0x04000C5D RID: 3165
		internal DataTable dataTable;

		// Token: 0x04000C5E RID: 3166
		private DataColumnCollection columns;

		// Token: 0x04000C5F RID: 3167
		private DataColumn[] columnCache = new DataColumn[DbSchemaTable.DBCOLUMN_NAME.Length];

		// Token: 0x04000C60 RID: 3168
		private bool _returnProviderSpecificTypes;

		// Token: 0x02000146 RID: 326
		private enum ColumnEnum
		{
			// Token: 0x04000C62 RID: 3170
			ColumnName,
			// Token: 0x04000C63 RID: 3171
			ColumnOrdinal,
			// Token: 0x04000C64 RID: 3172
			ColumnSize,
			// Token: 0x04000C65 RID: 3173
			BaseServerName,
			// Token: 0x04000C66 RID: 3174
			BaseCatalogName,
			// Token: 0x04000C67 RID: 3175
			BaseColumnName,
			// Token: 0x04000C68 RID: 3176
			BaseSchemaName,
			// Token: 0x04000C69 RID: 3177
			BaseTableName,
			// Token: 0x04000C6A RID: 3178
			IsAutoIncrement,
			// Token: 0x04000C6B RID: 3179
			IsUnique,
			// Token: 0x04000C6C RID: 3180
			IsKey,
			// Token: 0x04000C6D RID: 3181
			IsRowVersion,
			// Token: 0x04000C6E RID: 3182
			DataType,
			// Token: 0x04000C6F RID: 3183
			ProviderSpecificDataType,
			// Token: 0x04000C70 RID: 3184
			AllowDBNull,
			// Token: 0x04000C71 RID: 3185
			ProviderType,
			// Token: 0x04000C72 RID: 3186
			IsExpression,
			// Token: 0x04000C73 RID: 3187
			IsHidden,
			// Token: 0x04000C74 RID: 3188
			IsLong,
			// Token: 0x04000C75 RID: 3189
			IsReadOnly,
			// Token: 0x04000C76 RID: 3190
			SchemaMappingUnsortedIndex
		}
	}
}
