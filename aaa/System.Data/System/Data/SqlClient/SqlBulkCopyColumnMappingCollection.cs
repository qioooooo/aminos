using System;
using System.Collections;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x020002B7 RID: 695
	public sealed class SqlBulkCopyColumnMappingCollection : CollectionBase
	{
		// Token: 0x0600234C RID: 9036 RVA: 0x002719E8 File Offset: 0x00270DE8
		internal SqlBulkCopyColumnMappingCollection()
		{
		}

		// Token: 0x1700053E RID: 1342
		public SqlBulkCopyColumnMapping this[int index]
		{
			get
			{
				return (SqlBulkCopyColumnMapping)base.List[index];
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x00271A1C File Offset: 0x00270E1C
		// (set) Token: 0x0600234F RID: 9039 RVA: 0x00271A30 File Offset: 0x00270E30
		internal bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x00271A44 File Offset: 0x00270E44
		public SqlBulkCopyColumnMapping Add(SqlBulkCopyColumnMapping bulkCopyColumnMapping)
		{
			this.AssertWriteAccess();
			if ((ADP.IsEmpty(bulkCopyColumnMapping.SourceColumn) && bulkCopyColumnMapping.SourceOrdinal == -1) || (ADP.IsEmpty(bulkCopyColumnMapping.DestinationColumn) && bulkCopyColumnMapping.DestinationOrdinal == -1))
			{
				throw SQL.BulkLoadNonMatchingColumnMapping();
			}
			base.InnerList.Add(bulkCopyColumnMapping);
			return bulkCopyColumnMapping;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x00271A98 File Offset: 0x00270E98
		public SqlBulkCopyColumnMapping Add(string sourceColumn, string destinationColumn)
		{
			this.AssertWriteAccess();
			SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumn, destinationColumn);
			return this.Add(sqlBulkCopyColumnMapping);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x00271ABC File Offset: 0x00270EBC
		public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, string destinationColumn)
		{
			this.AssertWriteAccess();
			SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumn);
			return this.Add(sqlBulkCopyColumnMapping);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x00271AE0 File Offset: 0x00270EE0
		public SqlBulkCopyColumnMapping Add(string sourceColumn, int destinationColumnIndex)
		{
			this.AssertWriteAccess();
			SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumn, destinationColumnIndex);
			return this.Add(sqlBulkCopyColumnMapping);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x00271B04 File Offset: 0x00270F04
		public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, int destinationColumnIndex)
		{
			this.AssertWriteAccess();
			SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumnIndex);
			return this.Add(sqlBulkCopyColumnMapping);
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x00271B28 File Offset: 0x00270F28
		private void AssertWriteAccess()
		{
			if (this.ReadOnly)
			{
				throw SQL.BulkLoadMappingInaccessible();
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x00271B44 File Offset: 0x00270F44
		public new void Clear()
		{
			this.AssertWriteAccess();
			base.Clear();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x00271B60 File Offset: 0x00270F60
		public bool Contains(SqlBulkCopyColumnMapping value)
		{
			return -1 != base.InnerList.IndexOf(value);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x00271B80 File Offset: 0x00270F80
		public void CopyTo(SqlBulkCopyColumnMapping[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x00271B9C File Offset: 0x00270F9C
		internal void CreateDefaultMapping(int columnCount)
		{
			for (int i = 0; i < columnCount; i++)
			{
				base.InnerList.Add(new SqlBulkCopyColumnMapping(i, i));
			}
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x00271BC8 File Offset: 0x00270FC8
		public int IndexOf(SqlBulkCopyColumnMapping value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x00271BE4 File Offset: 0x00270FE4
		public void Insert(int index, SqlBulkCopyColumnMapping value)
		{
			this.AssertWriteAccess();
			base.InnerList.Insert(index, value);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x00271C04 File Offset: 0x00271004
		public void Remove(SqlBulkCopyColumnMapping value)
		{
			this.AssertWriteAccess();
			base.InnerList.Remove(value);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x00271C24 File Offset: 0x00271024
		public new void RemoveAt(int index)
		{
			this.AssertWriteAccess();
			base.RemoveAt(index);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x00271C40 File Offset: 0x00271040
		internal void ValidateCollection()
		{
			foreach (object obj in this)
			{
				SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = (SqlBulkCopyColumnMapping)obj;
				SqlBulkCopyColumnMappingCollection.MappingSchema mappingSchema;
				if (sqlBulkCopyColumnMapping.SourceOrdinal != -1)
				{
					if (sqlBulkCopyColumnMapping.DestinationOrdinal != -1)
					{
						mappingSchema = SqlBulkCopyColumnMappingCollection.MappingSchema.OrdinalsOrdinals;
					}
					else
					{
						mappingSchema = SqlBulkCopyColumnMappingCollection.MappingSchema.OrdinalsNames;
					}
				}
				else if (sqlBulkCopyColumnMapping.DestinationOrdinal != -1)
				{
					mappingSchema = SqlBulkCopyColumnMappingCollection.MappingSchema.NemesOrdinals;
				}
				else
				{
					mappingSchema = SqlBulkCopyColumnMappingCollection.MappingSchema.NamesNames;
				}
				if (this._mappingSchema == SqlBulkCopyColumnMappingCollection.MappingSchema.Undefined)
				{
					this._mappingSchema = mappingSchema;
				}
				else if (this._mappingSchema != mappingSchema)
				{
					throw SQL.BulkLoadMappingsNamesOrOrdinalsOnly();
				}
			}
		}

		// Token: 0x040016EA RID: 5866
		private bool _readOnly;

		// Token: 0x040016EB RID: 5867
		private SqlBulkCopyColumnMappingCollection.MappingSchema _mappingSchema;

		// Token: 0x020002B8 RID: 696
		private enum MappingSchema
		{
			// Token: 0x040016ED RID: 5869
			Undefined,
			// Token: 0x040016EE RID: 5870
			NamesNames,
			// Token: 0x040016EF RID: 5871
			NemesOrdinals,
			// Token: 0x040016F0 RID: 5872
			OrdinalsNames,
			// Token: 0x040016F1 RID: 5873
			OrdinalsOrdinals
		}
	}
}
