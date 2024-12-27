using System;

namespace System.Data.Design
{
	// Token: 0x0200009A RID: 154
	internal class DesignRelationCollection : DataSourceCollectionBase
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0000D3B6 File Offset: 0x0000C3B6
		public DesignRelationCollection(DesignDataSource dataSource)
			: base(dataSource)
		{
			this.dataSource = dataSource;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0000D3C6 File Offset: 0x0000C3C6
		private DataSet DataSet
		{
			get
			{
				if (this.dataSource != null)
				{
					return this.dataSource.DataSet;
				}
				return null;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0000D3DD File Offset: 0x0000C3DD
		protected override Type ItemType
		{
			get
			{
				return typeof(DesignRelation);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0000D3E9 File Offset: 0x0000C3E9
		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

		// Token: 0x170000C1 RID: 193
		internal DesignRelation this[ForeignKeyConstraint constraint]
		{
			get
			{
				if (constraint == null)
				{
					return null;
				}
				foreach (object obj in this)
				{
					DesignRelation designRelation = (DesignRelation)obj;
					if (designRelation.ForeignKeyConstraint == constraint)
					{
						return designRelation;
					}
				}
				return null;
			}
		}

		// Token: 0x170000C2 RID: 194
		internal DesignRelation this[string name]
		{
			get
			{
				return (DesignRelation)this.FindObject(name);
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000D462 File Offset: 0x0000C462
		public void Remove(DesignRelation rel)
		{
			base.List.Remove(rel);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0000D470 File Offset: 0x0000C470
		public int Add(DesignRelation rel)
		{
			return base.List.Add(rel);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0000D47E File Offset: 0x0000C47E
		public bool Contains(DesignRelation rel)
		{
			return base.List.Contains(rel);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0000D48C File Offset: 0x0000C48C
		protected override void OnInsert(int index, object value)
		{
			base.ValidateType(value);
			DesignRelation designRelation = (DesignRelation)value;
			if (this.dataSource != null && designRelation.Owner == this.dataSource)
			{
				return;
			}
			if (this.dataSource != null && designRelation.Owner != null)
			{
				throw new InternalException("This relation belongs to another DataSource already", 20010);
			}
			if (designRelation.Name == null || designRelation.Name.Length == 0)
			{
				designRelation.Name = this.CreateUniqueName(designRelation);
			}
			this.ValidateName(designRelation);
			DataSet dataSet = this.DataSet;
			if (dataSet != null)
			{
				if (designRelation.ForeignKeyConstraint != null)
				{
					ForeignKeyConstraint foreignKeyConstraint = designRelation.ForeignKeyConstraint;
					if (foreignKeyConstraint.Columns.Length > 0)
					{
						DataTable table = foreignKeyConstraint.Columns[0].Table;
						if (table != null && !table.Constraints.Contains(foreignKeyConstraint.ConstraintName))
						{
							table.Constraints.Add(foreignKeyConstraint);
						}
					}
				}
				if (designRelation.DataRelation != null && !dataSet.Relations.Contains(designRelation.DataRelation.RelationName))
				{
					dataSet.Relations.Add(designRelation.DataRelation);
				}
			}
			base.OnInsert(index, value);
			designRelation.Owner = this.dataSource;
		}

		// Token: 0x04000B59 RID: 2905
		private DesignDataSource dataSource;
	}
}
