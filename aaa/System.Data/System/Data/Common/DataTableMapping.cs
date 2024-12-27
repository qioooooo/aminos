using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Data.Common
{
	// Token: 0x02000120 RID: 288
	[TypeConverter(typeof(DataTableMapping.DataTableMappingConverter))]
	public sealed class DataTableMapping : MarshalByRefObject, ITableMapping, ICloneable
	{
		// Token: 0x0600127F RID: 4735 RVA: 0x0021EEA8 File Offset: 0x0021E2A8
		public DataTableMapping()
		{
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0021EEBC File Offset: 0x0021E2BC
		public DataTableMapping(string sourceTable, string dataSetTable)
		{
			this.SourceTable = sourceTable;
			this.DataSetTable = dataSetTable;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0021EEE0 File Offset: 0x0021E2E0
		public DataTableMapping(string sourceTable, string dataSetTable, DataColumnMapping[] columnMappings)
		{
			this.SourceTable = sourceTable;
			this.DataSetTable = dataSetTable;
			if (columnMappings != null && 0 < columnMappings.Length)
			{
				this.ColumnMappings.AddRange(columnMappings);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0021EF18 File Offset: 0x0021E318
		IColumnMappingCollection ITableMapping.ColumnMappings
		{
			get
			{
				return this.ColumnMappings;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x0021EF2C File Offset: 0x0021E32C
		[ResDescription("DataTableMapping_ColumnMappings")]
		[ResCategory("DataCategory_Mapping")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DataColumnMappingCollection ColumnMappings
		{
			get
			{
				DataColumnMappingCollection dataColumnMappingCollection = this._columnMappings;
				if (dataColumnMappingCollection == null)
				{
					dataColumnMappingCollection = new DataColumnMappingCollection();
					this._columnMappings = dataColumnMappingCollection;
				}
				return dataColumnMappingCollection;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0021EF54 File Offset: 0x0021E354
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0021EF74 File Offset: 0x0021E374
		[ResDescription("DataTableMapping_DataSetTable")]
		[ResCategory("DataCategory_Mapping")]
		[DefaultValue("")]
		public string DataSetTable
		{
			get
			{
				string dataSetTableName = this._dataSetTableName;
				if (dataSetTableName == null)
				{
					return ADP.StrEmpty;
				}
				return dataSetTableName;
			}
			set
			{
				this._dataSetTableName = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0021EF88 File Offset: 0x0021E388
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0021EF9C File Offset: 0x0021E39C
		internal DataTableMappingCollection Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0021EFB0 File Offset: 0x0021E3B0
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0021EFD0 File Offset: 0x0021E3D0
		[ResDescription("DataTableMapping_SourceTable")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Mapping")]
		public string SourceTable
		{
			get
			{
				string sourceTableName = this._sourceTableName;
				if (sourceTableName == null)
				{
					return ADP.StrEmpty;
				}
				return sourceTableName;
			}
			set
			{
				if (this.Parent != null && ADP.SrcCompare(this._sourceTableName, value) != 0)
				{
					this.Parent.ValidateSourceTable(-1, value);
				}
				this._sourceTableName = value;
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0021F008 File Offset: 0x0021E408
		object ICloneable.Clone()
		{
			DataTableMapping dataTableMapping = new DataTableMapping();
			dataTableMapping._dataSetTableName = this._dataSetTableName;
			dataTableMapping._sourceTableName = this._sourceTableName;
			if (this._columnMappings != null && 0 < this.ColumnMappings.Count)
			{
				DataColumnMappingCollection columnMappings = dataTableMapping.ColumnMappings;
				foreach (object obj in this.ColumnMappings)
				{
					ICloneable cloneable = (ICloneable)obj;
					columnMappings.Add(cloneable.Clone());
				}
			}
			return dataTableMapping;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0021F0B4 File Offset: 0x0021E4B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataColumn GetDataColumn(string sourceColumn, Type dataType, DataTable dataTable, MissingMappingAction mappingAction, MissingSchemaAction schemaAction)
		{
			return DataColumnMappingCollection.GetDataColumn(this._columnMappings, sourceColumn, dataType, dataTable, mappingAction, schemaAction);
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0021F0D4 File Offset: 0x0021E4D4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataColumnMapping GetColumnMappingBySchemaAction(string sourceColumn, MissingMappingAction mappingAction)
		{
			return DataColumnMappingCollection.GetColumnMappingBySchemaAction(this._columnMappings, sourceColumn, mappingAction);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0021F0F0 File Offset: 0x0021E4F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataTable GetDataTableBySchemaAction(DataSet dataSet, MissingSchemaAction schemaAction)
		{
			if (dataSet == null)
			{
				throw ADP.ArgumentNull("dataSet");
			}
			string dataSetTable = this.DataSetTable;
			if (ADP.IsEmpty(dataSetTable))
			{
				return null;
			}
			DataTableCollection tables = dataSet.Tables;
			int num = tables.IndexOf(dataSetTable);
			if (0 <= num && num < tables.Count)
			{
				return tables[num];
			}
			switch (schemaAction)
			{
			case MissingSchemaAction.Add:
			case MissingSchemaAction.AddWithKey:
				return new DataTable(dataSetTable);
			case MissingSchemaAction.Ignore:
				return null;
			case MissingSchemaAction.Error:
				throw ADP.MissingTableSchema(dataSetTable, this.SourceTable);
			default:
				throw ADP.InvalidMissingSchemaAction(schemaAction);
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0021F178 File Offset: 0x0021E578
		public override string ToString()
		{
			return this.SourceTable;
		}

		// Token: 0x04000BAD RID: 2989
		private DataTableMappingCollection parent;

		// Token: 0x04000BAE RID: 2990
		private DataColumnMappingCollection _columnMappings;

		// Token: 0x04000BAF RID: 2991
		private string _dataSetTableName;

		// Token: 0x04000BB0 RID: 2992
		private string _sourceTableName;

		// Token: 0x02000121 RID: 289
		internal sealed class DataTableMappingConverter : ExpandableObjectConverter
		{
			// Token: 0x06001290 RID: 4752 RVA: 0x0021F1A0 File Offset: 0x0021E5A0
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001291 RID: 4753 RVA: 0x0021F1C4 File Offset: 0x0021E5C4
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType && value is DataTableMapping)
				{
					DataTableMapping dataTableMapping = (DataTableMapping)value;
					DataColumnMapping[] array = new DataColumnMapping[dataTableMapping.ColumnMappings.Count];
					dataTableMapping.ColumnMappings.CopyTo(array, 0);
					object[] array2 = new object[] { dataTableMapping.SourceTable, dataTableMapping.DataSetTable, array };
					Type[] array3 = new Type[]
					{
						typeof(string),
						typeof(string),
						typeof(DataColumnMapping[])
					};
					ConstructorInfo constructor = typeof(DataTableMapping).GetConstructor(array3);
					return new InstanceDescriptor(constructor, array2);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
