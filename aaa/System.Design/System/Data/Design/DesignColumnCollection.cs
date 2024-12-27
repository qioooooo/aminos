using System;

namespace System.Data.Design
{
	// Token: 0x0200008F RID: 143
	internal class DesignColumnCollection : DataSourceCollectionBase
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0000B862 File Offset: 0x0000A862
		protected override Type ItemType
		{
			get
			{
				return typeof(DesignColumn);
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0000B870 File Offset: 0x0000A870
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0000B8F4 File Offset: 0x0000A8F4
		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0000B8FC File Offset: 0x0000A8FC
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

		// Token: 0x060005FB RID: 1531 RVA: 0x0000B998 File Offset: 0x0000A998
		public void Remove(DesignColumn column)
		{
			base.List.Remove(column);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000B9A6 File Offset: 0x0000A9A6
		public int IndexOf(DesignColumn column)
		{
			return base.List.IndexOf(column);
		}

		// Token: 0x17000072 RID: 114
		public DesignColumn this[string columnName]
		{
			get
			{
				return (DesignColumn)this.FindObject(columnName);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0000B9C4 File Offset: 0x0000A9C4
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

		// Token: 0x060005FF RID: 1535 RVA: 0x0000BA40 File Offset: 0x0000AA40
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

		// Token: 0x06000600 RID: 1536 RVA: 0x0000BAFC File Offset: 0x0000AAFC
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

		// Token: 0x17000073 RID: 115
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

		// Token: 0x04000B19 RID: 2841
		private DesignTable table;

		// Token: 0x04000B1A RID: 2842
		private DesignTable designTable;
	}
}
