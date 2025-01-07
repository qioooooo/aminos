using System;

namespace System.Data.Design
{
	internal class DesignRelationCollection : DataSourceCollectionBase
	{
		public DesignRelationCollection(DesignDataSource dataSource)
			: base(dataSource)
		{
			this.dataSource = dataSource;
		}

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

		protected override Type ItemType
		{
			get
			{
				return typeof(DesignRelation);
			}
		}

		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

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

		internal DesignRelation this[string name]
		{
			get
			{
				return (DesignRelation)this.FindObject(name);
			}
		}

		public void Remove(DesignRelation rel)
		{
			base.List.Remove(rel);
		}

		public int Add(DesignRelation rel)
		{
			return base.List.Add(rel);
		}

		public bool Contains(DesignRelation rel)
		{
			return base.List.Contains(rel);
		}

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

		private DesignDataSource dataSource;
	}
}
