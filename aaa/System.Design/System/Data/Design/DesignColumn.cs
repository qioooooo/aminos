using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.Design
{
	// Token: 0x0200008E RID: 142
	internal class DesignColumn : DataSourceComponent, IDataSourceNamedObject, INamedObject, ICloneable
	{
		// Token: 0x060005D1 RID: 1489 RVA: 0x0000B3D8 File Offset: 0x0000A3D8
		public DesignColumn()
		{
			this.dataColumn = new DataColumn();
			this.designTable = null;
			this.namingPropNames.Add("typedName");
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000B40E File Offset: 0x0000A40E
		public DesignColumn(DataColumn dataColumn)
		{
			if (dataColumn == null)
			{
				throw new InternalException("DesignColumn object needs a valid DataColumn", 20009);
			}
			this.dataColumn = dataColumn;
			this.namingPropNames.Add("typedName");
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0000B44C File Offset: 0x0000A44C
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x0000B45C File Offset: 0x0000A45C
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(false)]
		public bool AutoIncrement
		{
			get
			{
				return this.dataColumn.AutoIncrement;
			}
			set
			{
				if (this.dataColumn.AutoIncrement != value)
				{
					Type dataType = this.DataType;
					this.dataColumn.AutoIncrement = value;
					Type dataType2 = this.DataType;
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0000B493 File Offset: 0x0000A493
		public DataColumn DataColumn
		{
			get
			{
				return this.dataColumn;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0000B49B File Offset: 0x0000A49B
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0000B4A8 File Offset: 0x0000A4A8
		[DefaultValue(typeof(string))]
		[RefreshProperties(RefreshProperties.All)]
		public Type DataType
		{
			get
			{
				return this.dataColumn.DataType;
			}
			set
			{
				if (this.dataColumn.DataType != value)
				{
					bool autoIncrement = this.AutoIncrement;
					this.dataColumn.DataType = value;
					this.OnDataTypeChanged();
					bool autoIncrement2 = this.AutoIncrement;
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0000B4E5 File Offset: 0x0000A4E5
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0000B4ED File Offset: 0x0000A4ED
		internal DesignTable DesignTable
		{
			get
			{
				return this.designTable;
			}
			set
			{
				this.designTable = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0000B4F6 File Offset: 0x0000A4F6
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0000B503 File Offset: 0x0000A503
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		public string Expression
		{
			get
			{
				return this.dataColumn.Expression;
			}
			set
			{
				bool readOnly = this.dataColumn.ReadOnly;
				this.dataColumn.Expression = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0000B51D File Offset: 0x0000A51D
		protected override object ExternalPropertyHost
		{
			get
			{
				return this.dataColumn;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0000B525 File Offset: 0x0000A525
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x0000B532 File Offset: 0x0000A532
		[DefaultValue(-1)]
		public int MaxLength
		{
			get
			{
				return this.dataColumn.MaxLength;
			}
			set
			{
				if (this.MaxLength >= 0 && value > this.MaxLength)
				{
					this.dataColumn.MaxLength = -1;
				}
				this.dataColumn.MaxLength = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0000B55E File Offset: 0x0000A55E
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0000B56C File Offset: 0x0000A56C
		[MergableProperty(false)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.dataColumn.ColumnName;
			}
			set
			{
				string columnName = this.dataColumn.ColumnName;
				if (!StringUtil.EqualValue(value, columnName))
				{
					if (this.CollectionParent != null)
					{
						this.CollectionParent.ValidateUniqueName(this, value);
					}
					this.dataColumn.ColumnName = value;
					if (columnName.Length > 0 && value.Length > 0)
					{
						DesignTable designTable = this.DesignTable;
						if (designTable != null)
						{
							designTable.UpdateColumnMappingDataSetColumnName(columnName, value);
						}
					}
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0000B5D3 File Offset: 0x0000A5D3
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0000B60C File Offset: 0x0000A60C
		[DefaultValue("_throw")]
		public string NullValue
		{
			get
			{
				if (this.dataColumn.ExtendedProperties.Contains("nullValue"))
				{
					return this.dataColumn.ExtendedProperties["nullValue"] as string;
				}
				return "_throw";
			}
			set
			{
				if (value != this.NullValue)
				{
					this.dataColumn.ExtendedProperties["nullValue"] = value;
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0000B632 File Offset: 0x0000A632
		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Column";
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0000B63C File Offset: 0x0000A63C
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0000B6AB File Offset: 0x0000A6AB
		[DefaultValue("")]
		public string Source
		{
			get
			{
				if (this.DesignTable != null && this.DesignTable.Mappings != null)
				{
					int num = this.DesignTable.Mappings.IndexOfDataSetColumn(this.DataColumn.ColumnName);
					DataColumnMapping dataColumnMapping = null;
					if (num >= 0)
					{
						dataColumnMapping = this.DesignTable.Mappings.GetByDataSetColumn(this.DataColumn.ColumnName);
					}
					if (dataColumnMapping != null)
					{
						return dataColumnMapping.SourceColumn;
					}
				}
				return string.Empty;
			}
			set
			{
				if (this.DesignTable != null)
				{
					this.DesignTable.UpdateColumnMappingSourceColumnName(this.DataColumn.ColumnName, value);
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000B6CC File Offset: 0x0000A6CC
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0000B6D9 File Offset: 0x0000A6D9
		[DefaultValue(false)]
		public bool Unique
		{
			get
			{
				return this.dataColumn.Unique;
			}
			set
			{
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000B6DC File Offset: 0x0000A6DC
		public object Clone()
		{
			DataColumn dataColumn = DataDesignUtil.CloneColumn(this.dataColumn);
			return new DesignColumn(dataColumn);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000B700 File Offset: 0x0000A700
		internal bool IsKeyColumn()
		{
			if (this.DesignTable == null)
			{
				return false;
			}
			ArrayList relatedDataConstraints = this.DesignTable.GetRelatedDataConstraints(new DesignColumn[] { this }, true);
			return relatedDataConstraints != null && relatedDataConstraints.Count > 0;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0000B73E File Offset: 0x0000A73E
		private void OnDataTypeChanged()
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000B740 File Offset: 0x0000A740
		public override string ToString()
		{
			return this.PublicTypeName + " " + this.Name;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0000B758 File Offset: 0x0000A758
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0000B774 File Offset: 0x0000A774
		internal string UserColumnName
		{
			get
			{
				return this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_USER_COLUMNNAME] as string;
			}
			set
			{
				this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_USER_COLUMNNAME] = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0000B78C File Offset: 0x0000A78C
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0000B7A8 File Offset: 0x0000A7A8
		internal string GeneratorColumnPropNameInTable
		{
			get
			{
				return this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINTABLE] as string;
			}
			set
			{
				this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINTABLE] = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0000B7C0 File Offset: 0x0000A7C0
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0000B7DC File Offset: 0x0000A7DC
		internal string GeneratorColumnVarNameInTable
		{
			get
			{
				return this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNVARNAMEINTABLE] as string;
			}
			set
			{
				this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNVARNAMEINTABLE] = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0000B7F4 File Offset: 0x0000A7F4
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0000B810 File Offset: 0x0000A810
		internal string GeneratorColumnPropNameInRow
		{
			get
			{
				return this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINROW] as string;
			}
			set
			{
				this.dataColumn.ExtendedProperties[DesignColumn.EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINROW] = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000B828 File Offset: 0x0000A828
		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0000B830 File Offset: 0x0000A830
		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorColumnPropNameInRow;
			}
		}

		// Token: 0x04000B0F RID: 2831
		private const string NullValuePropertyName = "nullValue";

		// Token: 0x04000B10 RID: 2832
		private const string NullValueThrow = "_throw";

		// Token: 0x04000B11 RID: 2833
		private const string ROPNAME_EXPRESSION = "Expression";

		// Token: 0x04000B12 RID: 2834
		private DataColumn dataColumn;

		// Token: 0x04000B13 RID: 2835
		private DesignTable designTable;

		// Token: 0x04000B14 RID: 2836
		private StringCollection namingPropNames = new StringCollection();

		// Token: 0x04000B15 RID: 2837
		internal static string EXTPROPNAME_USER_COLUMNNAME = "Generator_UserColumnName";

		// Token: 0x04000B16 RID: 2838
		internal static string EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINTABLE = "Generator_ColumnPropNameInTable";

		// Token: 0x04000B17 RID: 2839
		internal static string EXTPROPNAME_GENERATOR_COLUMNVARNAMEINTABLE = "Generator_ColumnVarNameInTable";

		// Token: 0x04000B18 RID: 2840
		internal static string EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINROW = "Generator_ColumnPropNameInRow";
	}
}
