using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.Design
{
	internal class DesignColumn : DataSourceComponent, IDataSourceNamedObject, INamedObject, ICloneable
	{
		public DesignColumn()
		{
			this.dataColumn = new DataColumn();
			this.designTable = null;
			this.namingPropNames.Add("typedName");
		}

		public DesignColumn(DataColumn dataColumn)
		{
			if (dataColumn == null)
			{
				throw new InternalException("DesignColumn object needs a valid DataColumn", 20009);
			}
			this.dataColumn = dataColumn;
			this.namingPropNames.Add("typedName");
		}

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

		public DataColumn DataColumn
		{
			get
			{
				return this.dataColumn;
			}
		}

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

		protected override object ExternalPropertyHost
		{
			get
			{
				return this.dataColumn;
			}
		}

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

		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Column";
			}
		}

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

		public object Clone()
		{
			DataColumn dataColumn = DataDesignUtil.CloneColumn(this.dataColumn);
			return new DesignColumn(dataColumn);
		}

		internal bool IsKeyColumn()
		{
			if (this.DesignTable == null)
			{
				return false;
			}
			ArrayList relatedDataConstraints = this.DesignTable.GetRelatedDataConstraints(new DesignColumn[] { this }, true);
			return relatedDataConstraints != null && relatedDataConstraints.Count > 0;
		}

		private void OnDataTypeChanged()
		{
		}

		public override string ToString()
		{
			return this.PublicTypeName + " " + this.Name;
		}

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

		internal override StringCollection NamingPropertyNames
		{
			get
			{
				return this.namingPropNames;
			}
		}

		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorColumnPropNameInRow;
			}
		}

		private const string NullValuePropertyName = "nullValue";

		private const string NullValueThrow = "_throw";

		private const string ROPNAME_EXPRESSION = "Expression";

		private DataColumn dataColumn;

		private DesignTable designTable;

		private StringCollection namingPropNames = new StringCollection();

		internal static string EXTPROPNAME_USER_COLUMNNAME = "Generator_UserColumnName";

		internal static string EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINTABLE = "Generator_ColumnPropNameInTable";

		internal static string EXTPROPNAME_GENERATOR_COLUMNVARNAMEINTABLE = "Generator_ColumnVarNameInTable";

		internal static string EXTPROPNAME_GENERATOR_COLUMNPROPNAMEINROW = "Generator_ColumnPropNameInRow";
	}
}
