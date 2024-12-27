using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Data.Common
{
	// Token: 0x02000119 RID: 281
	[TypeConverter(typeof(DataColumnMapping.DataColumnMappingConverter))]
	public sealed class DataColumnMapping : MarshalByRefObject, IColumnMapping, ICloneable
	{
		// Token: 0x060011EF RID: 4591 RVA: 0x0021DBC0 File Offset: 0x0021CFC0
		public DataColumnMapping()
		{
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0021DBD4 File Offset: 0x0021CFD4
		public DataColumnMapping(string sourceColumn, string dataSetColumn)
		{
			this.SourceColumn = sourceColumn;
			this.DataSetColumn = dataSetColumn;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0021DBF8 File Offset: 0x0021CFF8
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x0021DC18 File Offset: 0x0021D018
		[ResDescription("DataColumnMapping_DataSetColumn")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Mapping")]
		public string DataSetColumn
		{
			get
			{
				string dataSetColumnName = this._dataSetColumnName;
				if (dataSetColumnName == null)
				{
					return ADP.StrEmpty;
				}
				return dataSetColumnName;
			}
			set
			{
				this._dataSetColumnName = value;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0021DC2C File Offset: 0x0021D02C
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x0021DC40 File Offset: 0x0021D040
		internal DataColumnMappingCollection Parent
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

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x0021DC54 File Offset: 0x0021D054
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x0021DC74 File Offset: 0x0021D074
		[ResDescription("DataColumnMapping_SourceColumn")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Mapping")]
		public string SourceColumn
		{
			get
			{
				string sourceColumnName = this._sourceColumnName;
				if (sourceColumnName == null)
				{
					return ADP.StrEmpty;
				}
				return sourceColumnName;
			}
			set
			{
				if (this.Parent != null && ADP.SrcCompare(this._sourceColumnName, value) != 0)
				{
					this.Parent.ValidateSourceColumn(-1, value);
				}
				this._sourceColumnName = value;
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0021DCAC File Offset: 0x0021D0AC
		object ICloneable.Clone()
		{
			return new DataColumnMapping
			{
				_sourceColumnName = this._sourceColumnName,
				_dataSetColumnName = this._dataSetColumnName
			};
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0021DCD8 File Offset: 0x0021D0D8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataColumn GetDataColumnBySchemaAction(DataTable dataTable, Type dataType, MissingSchemaAction schemaAction)
		{
			return DataColumnMapping.GetDataColumnBySchemaAction(this.SourceColumn, this.DataSetColumn, dataTable, dataType, schemaAction);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0021DCFC File Offset: 0x0021D0FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static DataColumn GetDataColumnBySchemaAction(string sourceColumn, string dataSetColumn, DataTable dataTable, Type dataType, MissingSchemaAction schemaAction)
		{
			if (dataTable == null)
			{
				throw ADP.ArgumentNull("dataTable");
			}
			if (ADP.IsEmpty(dataSetColumn))
			{
				return null;
			}
			DataColumnCollection columns = dataTable.Columns;
			int num = columns.IndexOf(dataSetColumn);
			if (0 <= num && num < columns.Count)
			{
				DataColumn dataColumn = columns[num];
				if (!ADP.IsEmpty(dataColumn.Expression))
				{
					throw ADP.ColumnSchemaExpression(sourceColumn, dataSetColumn);
				}
				if (dataType == null || dataType.IsArray == dataColumn.DataType.IsArray)
				{
					return dataColumn;
				}
				throw ADP.ColumnSchemaMismatch(sourceColumn, dataType, dataColumn);
			}
			else
			{
				switch (schemaAction)
				{
				case MissingSchemaAction.Add:
				case MissingSchemaAction.AddWithKey:
					return new DataColumn(dataSetColumn, dataType);
				case MissingSchemaAction.Ignore:
					return null;
				case MissingSchemaAction.Error:
					throw ADP.ColumnSchemaMissing(dataSetColumn, dataTable.TableName, sourceColumn);
				default:
					throw ADP.InvalidMissingSchemaAction(schemaAction);
				}
			}
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0021DDB8 File Offset: 0x0021D1B8
		public override string ToString()
		{
			return this.SourceColumn;
		}

		// Token: 0x04000B79 RID: 2937
		private DataColumnMappingCollection parent;

		// Token: 0x04000B7A RID: 2938
		private string _dataSetColumnName;

		// Token: 0x04000B7B RID: 2939
		private string _sourceColumnName;

		// Token: 0x0200011A RID: 282
		internal sealed class DataColumnMappingConverter : ExpandableObjectConverter
		{
			// Token: 0x060011FC RID: 4604 RVA: 0x0021DDE0 File Offset: 0x0021D1E0
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x060011FD RID: 4605 RVA: 0x0021DE04 File Offset: 0x0021D204
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType && value is DataColumnMapping)
				{
					DataColumnMapping dataColumnMapping = (DataColumnMapping)value;
					object[] array = new object[] { dataColumnMapping.SourceColumn, dataColumnMapping.DataSetColumn };
					Type[] array2 = new Type[]
					{
						typeof(string),
						typeof(string)
					};
					ConstructorInfo constructor = typeof(DataColumnMapping).GetConstructor(array2);
					return new InstanceDescriptor(constructor, array);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
