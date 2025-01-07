using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Data.Design
{
	internal class DesignRelation : DataSourceComponent, IDataSourceNamedObject, INamedObject
	{
		public DesignRelation(DataRelation dataRelation)
		{
			this.DataRelation = dataRelation;
		}

		public DesignRelation(ForeignKeyConstraint foreignKeyConstraint)
		{
			this.DataRelation = null;
			this.dataForeignKeyConstraint = foreignKeyConstraint;
		}

		internal DataColumn[] ChildDataColumns
		{
			get
			{
				if (this.dataRelation != null)
				{
					return this.dataRelation.ChildColumns;
				}
				if (this.dataForeignKeyConstraint != null)
				{
					return this.dataForeignKeyConstraint.Columns;
				}
				return new DataColumn[0];
			}
		}

		internal DesignTable ChildDesignTable
		{
			get
			{
				DataTable dataTable = null;
				if (this.dataRelation != null)
				{
					dataTable = this.dataRelation.ChildTable;
				}
				else if (this.dataForeignKeyConstraint != null)
				{
					dataTable = this.dataForeignKeyConstraint.Table;
				}
				if (dataTable != null && this.Owner != null)
				{
					return this.Owner.DesignTables[dataTable];
				}
				return null;
			}
		}

		internal DataRelation DataRelation
		{
			get
			{
				return this.dataRelation;
			}
			set
			{
				this.dataRelation = value;
				if (this.dataRelation != null)
				{
					this.dataForeignKeyConstraint = null;
				}
			}
		}

		internal ForeignKeyConstraint ForeignKeyConstraint
		{
			get
			{
				if (this.dataRelation != null && this.dataRelation.ChildKeyConstraint != null)
				{
					return this.dataRelation.ChildKeyConstraint;
				}
				return this.dataForeignKeyConstraint;
			}
			set
			{
				this.dataForeignKeyConstraint = value;
			}
		}

		[DefaultValue("")]
		[MergableProperty(false)]
		public string Name
		{
			get
			{
				if (this.dataRelation != null)
				{
					return this.dataRelation.RelationName;
				}
				if (this.dataForeignKeyConstraint != null)
				{
					return this.dataForeignKeyConstraint.ConstraintName;
				}
				return string.Empty;
			}
			set
			{
				if (!StringUtil.EqualValue(this.Name, value))
				{
					if (this.CollectionParent != null)
					{
						this.CollectionParent.ValidateUniqueName(this, value);
					}
					if (this.dataRelation != null)
					{
						this.dataRelation.RelationName = value;
					}
					if (this.dataForeignKeyConstraint != null)
					{
						this.dataForeignKeyConstraint.ConstraintName = value;
					}
				}
			}
		}

		internal DesignDataSource Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		internal DataColumn[] ParentDataColumns
		{
			get
			{
				if (this.dataRelation != null)
				{
					return this.dataRelation.ParentColumns;
				}
				if (this.dataForeignKeyConstraint != null)
				{
					return this.dataForeignKeyConstraint.RelatedColumns;
				}
				return new DataColumn[0];
			}
		}

		internal DesignTable ParentDesignTable
		{
			get
			{
				DataTable dataTable = null;
				if (this.dataRelation != null)
				{
					dataTable = this.dataRelation.ParentTable;
				}
				else if (this.dataForeignKeyConstraint != null)
				{
					dataTable = this.dataForeignKeyConstraint.RelatedTable;
				}
				if (dataTable != null && this.Owner != null)
				{
					return this.Owner.DesignTables[dataTable];
				}
				return null;
			}
		}

		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Relation";
			}
		}

		internal string UserRelationName
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_UserRelationName"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_UserRelationName"] = value;
			}
		}

		internal string UserParentTable
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_UserParentTable"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_UserParentTable"] = value;
			}
		}

		internal string UserChildTable
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_UserChildTable"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_UserChildTable"] = value;
			}
		}

		internal string GeneratorRelationVarName
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_RelationVarName"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_RelationVarName"] = value;
			}
		}

		internal string GeneratorChildPropName
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_ChildPropName"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_ChildPropName"] = value;
			}
		}

		internal string GeneratorParentPropName
		{
			get
			{
				return this.dataRelation.ExtendedProperties["Generator_ParentPropName"] as string;
			}
			set
			{
				this.dataRelation.ExtendedProperties["Generator_ParentPropName"] = value;
			}
		}

		internal override StringCollection NamingPropertyNames
		{
			get
			{
				StringCollection stringCollection = new StringCollection();
				stringCollection.AddRange(new string[] { "typedParent", "typedChildren" });
				return stringCollection;
			}
		}

		internal const string NAMEROOT = "Relation";

		private const string EXTPROPNAME_USER_RELATIONNAME = "Generator_UserRelationName";

		private const string EXTPROPNAME_USER_PARENTTABLE = "Generator_UserParentTable";

		private const string EXTPROPNAME_USER_CHILDTABLE = "Generator_UserChildTable";

		private const string EXTPROPNAME_GENERATOR_RELATIONVARNAME = "Generator_RelationVarName";

		private const string EXTPROPNAME_GENERATOR_PARENTPROPNAME = "Generator_ParentPropName";

		private const string EXTPROPNAME_GENERATOR_CHILDPROPNAME = "Generator_ChildPropName";

		private DesignDataSource owner;

		private DataRelation dataRelation;

		private ForeignKeyConstraint dataForeignKeyConstraint;

		[Flags]
		public enum CompareOption
		{
			Columns = 0,
			Tables = 1,
			ForeignKeyConstraints = 2
		}
	}
}
