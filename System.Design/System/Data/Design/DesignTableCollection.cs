using System;

namespace System.Data.Design
{
	internal class DesignTableCollection : DataSourceCollectionBase
	{
		public DesignTableCollection(DesignDataSource dataSource)
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
				return typeof(DesignTable);
			}
		}

		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

		internal DesignTable this[string name]
		{
			get
			{
				return (DesignTable)this.FindObject(name);
			}
		}

		internal DesignTable this[DataTable dataTable]
		{
			get
			{
				foreach (object obj in this)
				{
					DesignTable designTable = (DesignTable)obj;
					if (designTable.DataTable == dataTable)
					{
						return designTable;
					}
				}
				return null;
			}
		}

		public void Add(DesignTable designTable)
		{
			base.List.Add(designTable);
		}

		public bool Contains(DesignTable table)
		{
			return base.List.Contains(table);
		}

		public int IndexOf(DesignTable table)
		{
			return base.List.IndexOf(table);
		}

		public void Remove(DesignTable table)
		{
			base.List.Remove(table);
		}

		protected override void OnInsert(int index, object value)
		{
			base.OnInsert(index, value);
			DesignTable designTable = (DesignTable)value;
			if (designTable.Name == null || designTable.Name.Length == 0)
			{
				designTable.Name = this.CreateUniqueName(designTable);
			}
			this.NameService.ValidateUniqueName(this, designTable.Name);
			if (this.dataSource != null && designTable.Owner == this.dataSource)
			{
				return;
			}
			if (this.dataSource != null && designTable.Owner != null)
			{
				throw new InternalException("This table belongs to another DataSource already", 20002);
			}
			DataSet dataSet = this.DataSet;
			if (dataSet != null && !dataSet.Tables.Contains(designTable.DataTable.TableName))
			{
				dataSet.Tables.Add(designTable.DataTable);
			}
			designTable.Owner = this.dataSource;
		}

		protected override void OnRemove(int index, object value)
		{
			base.OnRemove(index, value);
			DesignTable designTable = (DesignTable)value;
			DataSet dataSet = this.DataSet;
			if (dataSet != null && designTable.DataTable != null && dataSet.Tables.Contains(designTable.DataTable.TableName))
			{
				dataSet.Tables.Remove(designTable.DataTable);
			}
			designTable.Owner = null;
		}

		private DesignDataSource dataSource;
	}
}
