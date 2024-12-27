using System;
using System.Globalization;

namespace System.Data.Common
{
	// Token: 0x02000144 RID: 324
	internal sealed class DbSchemaRow
	{
		// Token: 0x06001500 RID: 5376 RVA: 0x00228D9C File Offset: 0x0022819C
		internal static DbSchemaRow[] GetSortedSchemaRows(DataTable dataTable, bool returnProviderSpecificTypes)
		{
			DataColumn dataColumn = dataTable.Columns["SchemaMapping Unsorted Index"];
			if (dataColumn == null)
			{
				dataColumn = new DataColumn("SchemaMapping Unsorted Index", typeof(int));
				dataTable.Columns.Add(dataColumn);
			}
			int count = dataTable.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				dataTable.Rows[i][dataColumn] = i;
			}
			DbSchemaTable dbSchemaTable = new DbSchemaTable(dataTable, returnProviderSpecificTypes);
			DataRow[] array = dataTable.Select(null, "ColumnOrdinal ASC", DataViewRowState.CurrentRows);
			DbSchemaRow[] array2 = new DbSchemaRow[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = new DbSchemaRow(dbSchemaTable, array[j]);
			}
			return array2;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00228E50 File Offset: 0x00228250
		internal DbSchemaRow(DbSchemaTable schemaTable, DataRow dataRow)
		{
			this.schemaTable = schemaTable;
			this.dataRow = dataRow;
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00228E74 File Offset: 0x00228274
		internal DataRow DataRow
		{
			get
			{
				return this.dataRow;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x00228E88 File Offset: 0x00228288
		internal string ColumnName
		{
			get
			{
				object obj = this.dataRow[this.schemaTable.ColumnName, DataRowVersion.Default];
				if (!Convert.IsDBNull(obj))
				{
					return Convert.ToString(obj, CultureInfo.InvariantCulture);
				}
				return "";
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00228ECC File Offset: 0x002282CC
		internal int Size
		{
			get
			{
				object obj = this.dataRow[this.schemaTable.Size, DataRowVersion.Default];
				if (!Convert.IsDBNull(obj))
				{
					return Convert.ToInt32(obj, CultureInfo.InvariantCulture);
				}
				return 0;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x00228F0C File Offset: 0x0022830C
		internal string BaseColumnName
		{
			get
			{
				if (this.schemaTable.BaseColumnName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseColumnName, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj, CultureInfo.InvariantCulture);
					}
				}
				return "";
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x00228F5C File Offset: 0x0022835C
		internal string BaseServerName
		{
			get
			{
				if (this.schemaTable.BaseServerName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseServerName, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj, CultureInfo.InvariantCulture);
					}
				}
				return "";
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x00228FAC File Offset: 0x002283AC
		internal string BaseCatalogName
		{
			get
			{
				if (this.schemaTable.BaseCatalogName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseCatalogName, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj, CultureInfo.InvariantCulture);
					}
				}
				return "";
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00228FFC File Offset: 0x002283FC
		internal string BaseSchemaName
		{
			get
			{
				if (this.schemaTable.BaseSchemaName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseSchemaName, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj, CultureInfo.InvariantCulture);
					}
				}
				return "";
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0022904C File Offset: 0x0022844C
		internal string BaseTableName
		{
			get
			{
				if (this.schemaTable.BaseTableName != null)
				{
					object obj = this.dataRow[this.schemaTable.BaseTableName, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToString(obj, CultureInfo.InvariantCulture);
					}
				}
				return "";
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0022909C File Offset: 0x0022849C
		internal bool IsAutoIncrement
		{
			get
			{
				if (this.schemaTable.IsAutoIncrement != null)
				{
					object obj = this.dataRow[this.schemaTable.IsAutoIncrement, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x002290E8 File Offset: 0x002284E8
		internal bool IsUnique
		{
			get
			{
				if (this.schemaTable.IsUnique != null)
				{
					object obj = this.dataRow[this.schemaTable.IsUnique, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x00229134 File Offset: 0x00228534
		internal bool IsRowVersion
		{
			get
			{
				if (this.schemaTable.IsRowVersion != null)
				{
					object obj = this.dataRow[this.schemaTable.IsRowVersion, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x00229180 File Offset: 0x00228580
		internal bool IsKey
		{
			get
			{
				if (this.schemaTable.IsKey != null)
				{
					object obj = this.dataRow[this.schemaTable.IsKey, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x002291CC File Offset: 0x002285CC
		internal bool IsExpression
		{
			get
			{
				if (this.schemaTable.IsExpression != null)
				{
					object obj = this.dataRow[this.schemaTable.IsExpression, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x00229218 File Offset: 0x00228618
		internal bool IsHidden
		{
			get
			{
				if (this.schemaTable.IsHidden != null)
				{
					object obj = this.dataRow[this.schemaTable.IsHidden, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x00229264 File Offset: 0x00228664
		internal bool IsLong
		{
			get
			{
				if (this.schemaTable.IsLong != null)
				{
					object obj = this.dataRow[this.schemaTable.IsLong, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x002292B0 File Offset: 0x002286B0
		internal bool IsReadOnly
		{
			get
			{
				if (this.schemaTable.IsReadOnly != null)
				{
					object obj = this.dataRow[this.schemaTable.IsReadOnly, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return false;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x002292FC File Offset: 0x002286FC
		internal Type DataType
		{
			get
			{
				if (this.schemaTable.DataType != null)
				{
					object obj = this.dataRow[this.schemaTable.DataType, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return (Type)obj;
					}
				}
				return null;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x00229344 File Offset: 0x00228744
		internal bool AllowDBNull
		{
			get
			{
				if (this.schemaTable.AllowDBNull != null)
				{
					object obj = this.dataRow[this.schemaTable.AllowDBNull, DataRowVersion.Default];
					if (!Convert.IsDBNull(obj))
					{
						return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
					}
				}
				return true;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x00229390 File Offset: 0x00228790
		internal int UnsortedIndex
		{
			get
			{
				return (int)this.dataRow[this.schemaTable.UnsortedIndex, DataRowVersion.Default];
			}
		}

		// Token: 0x04000C59 RID: 3161
		internal const string SchemaMappingUnsortedIndex = "SchemaMapping Unsorted Index";

		// Token: 0x04000C5A RID: 3162
		private DbSchemaTable schemaTable;

		// Token: 0x04000C5B RID: 3163
		private DataRow dataRow;
	}
}
