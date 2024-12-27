using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Data.Design
{
	// Token: 0x02000098 RID: 152
	internal class DesignRelation : DataSourceComponent, IDataSourceNamedObject, INamedObject
	{
		// Token: 0x0600069D RID: 1693 RVA: 0x0000D021 File Offset: 0x0000C021
		public DesignRelation(DataRelation dataRelation)
		{
			this.DataRelation = dataRelation;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000D030 File Offset: 0x0000C030
		public DesignRelation(ForeignKeyConstraint foreignKeyConstraint)
		{
			this.DataRelation = null;
			this.dataForeignKeyConstraint = foreignKeyConstraint;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0000D046 File Offset: 0x0000C046
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0000D078 File Offset: 0x0000C078
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0000D0CF File Offset: 0x0000C0CF
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0000D0D7 File Offset: 0x0000C0D7
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0000D0EF File Offset: 0x0000C0EF
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0000D118 File Offset: 0x0000C118
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0000D121 File Offset: 0x0000C121
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0000D150 File Offset: 0x0000C150
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0000D1A8 File Offset: 0x0000C1A8
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0000D1B0 File Offset: 0x0000C1B0
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0000D1B9 File Offset: 0x0000C1B9
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0000D1EC File Offset: 0x0000C1EC
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0000D243 File Offset: 0x0000C243
		[Browsable(false)]
		public string PublicTypeName
		{
			get
			{
				return "Relation";
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0000D24A File Offset: 0x0000C24A
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x0000D266 File Offset: 0x0000C266
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0000D27E File Offset: 0x0000C27E
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0000D29A File Offset: 0x0000C29A
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0000D2B2 File Offset: 0x0000C2B2
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x0000D2CE File Offset: 0x0000C2CE
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0000D2E6 File Offset: 0x0000C2E6
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x0000D302 File Offset: 0x0000C302
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0000D31A File Offset: 0x0000C31A
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0000D336 File Offset: 0x0000C336
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0000D34E File Offset: 0x0000C34E
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0000D36A File Offset: 0x0000C36A
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0000D384 File Offset: 0x0000C384
		internal override StringCollection NamingPropertyNames
		{
			get
			{
				StringCollection stringCollection = new StringCollection();
				stringCollection.AddRange(new string[] { "typedParent", "typedChildren" });
				return stringCollection;
			}
		}

		// Token: 0x04000B4B RID: 2891
		internal const string NAMEROOT = "Relation";

		// Token: 0x04000B4C RID: 2892
		private const string EXTPROPNAME_USER_RELATIONNAME = "Generator_UserRelationName";

		// Token: 0x04000B4D RID: 2893
		private const string EXTPROPNAME_USER_PARENTTABLE = "Generator_UserParentTable";

		// Token: 0x04000B4E RID: 2894
		private const string EXTPROPNAME_USER_CHILDTABLE = "Generator_UserChildTable";

		// Token: 0x04000B4F RID: 2895
		private const string EXTPROPNAME_GENERATOR_RELATIONVARNAME = "Generator_RelationVarName";

		// Token: 0x04000B50 RID: 2896
		private const string EXTPROPNAME_GENERATOR_PARENTPROPNAME = "Generator_ParentPropName";

		// Token: 0x04000B51 RID: 2897
		private const string EXTPROPNAME_GENERATOR_CHILDPROPNAME = "Generator_ChildPropName";

		// Token: 0x04000B52 RID: 2898
		private DesignDataSource owner;

		// Token: 0x04000B53 RID: 2899
		private DataRelation dataRelation;

		// Token: 0x04000B54 RID: 2900
		private ForeignKeyConstraint dataForeignKeyConstraint;

		// Token: 0x02000099 RID: 153
		[Flags]
		public enum CompareOption
		{
			// Token: 0x04000B56 RID: 2902
			Columns = 0,
			// Token: 0x04000B57 RID: 2903
			Tables = 1,
			// Token: 0x04000B58 RID: 2904
			ForeignKeyConstraints = 2
		}
	}
}
