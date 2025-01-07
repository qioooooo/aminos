using System;

namespace System.Data.Design
{
	internal class DesignColumnCollection : DataSourceCollectionBase
	{
		protected override Type ItemType
		{
			get
			{
				return typeof(DesignColumn);
			}
		}

		public DesignColumnCollection(DesignTable designTable)
			: base(designTable)
		{
			this.designTable = designTable;
			if (designTable != null && designTable.DataTable != null)
			{
				foreach (object obj in designTable.DataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj;
					this.Add(new DesignColumn(dataColumn));
				}
			}
			this.table = designTable;
		}

		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

		public void Add(DesignColumn designColumn)
		{
			if (designColumn.DesignTable != null && designColumn.DesignTable != this.designTable)
			{
				throw new InternalException("Cannot insert a DesignColumn object in two collections.");
			}
			designColumn.DesignTable = this.designTable;
			base.List.Add(designColumn);
			if (designColumn.DataColumn != null && this.designTable != null && this.designTable.DataTable != null && !this.designTable.DataTable.Columns.Contains(designColumn.Name))
			{
				this.designTable.DataTable.Columns.Add(designColumn.DataColumn);
			}
		}

		public void Remove(DesignColumn column)
		{
			base.List.Remove(column);
		}

		public int IndexOf(DesignColumn column)
		{
			return base.List.IndexOf(column);
		}

		public DesignColumn this[string columnName]
		{
			get
			{
				return (DesignColumn)this.FindObject(columnName);
			}
		}

		protected override void OnInsert(int index, object value)
		{
			base.OnInsert(index, value);
			base.ValidateType(value);
			DesignColumn designColumn = (DesignColumn)value;
			if (designColumn.DataColumn != null && this.table != null && !this.table.DataTable.Columns.Contains(designColumn.DataColumn.ColumnName))
			{
				this.table.DataTable.Columns.Add(designColumn.DataColumn);
			}
			designColumn.DesignTable = this.designTable;
		}

		protected override void OnSet(int index, object oldValue, object newValue)
		{
			base.OnSet(index, oldValue, newValue);
			base.ValidateType(newValue);
			base.ValidateType(oldValue);
			DesignColumn designColumn = (DesignColumn)oldValue;
			DesignColumn designColumn2 = (DesignColumn)newValue;
			if (this.table != null && oldValue != newValue)
			{
				if (designColumn.DataColumn != null)
				{
					this.table.DataTable.Columns.Remove(designColumn.DataColumn);
					designColumn.DesignTable = null;
				}
				if (designColumn2.DataColumn != null && !this.table.DataTable.Columns.Contains(designColumn2.DataColumn.ColumnName))
				{
					this.table.DataTable.Columns.Add(designColumn2.DataColumn);
					designColumn2.DesignTable = this.designTable;
				}
			}
		}

		protected override void OnRemove(int index, object value)
		{
			base.OnRemove(index, value);
			base.ValidateType(value);
			DesignColumn designColumn = (DesignColumn)value;
			if (this.table != null && designColumn.DataColumn != null)
			{
				this.table.DataTable.Columns.Remove(designColumn.DataColumn);
			}
			designColumn.DesignTable = null;
		}

		public DesignColumn this[int index]
		{
			get
			{
				int num = 0;
				foreach (object obj in base.InnerList)
				{
					DesignColumn designColumn = (DesignColumn)obj;
					if (index == num)
					{
						return designColumn;
					}
					num++;
				}
				throw new InternalException("Index out of range in getting DesignColumn", 20011);
			}
		}

		private DesignTable table;

		private DesignTable designTable;
	}
}
