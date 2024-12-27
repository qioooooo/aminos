using System;

namespace System.Data.Design
{
	// Token: 0x0200009E RID: 158
	internal class DesignTableCollection : DataSourceCollectionBase
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x0000F64D File Offset: 0x0000E64D
		public DesignTableCollection(DesignDataSource dataSource)
			: base(dataSource)
		{
			this.dataSource = dataSource;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000F65D File Offset: 0x0000E65D
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0000F674 File Offset: 0x0000E674
		protected override Type ItemType
		{
			get
			{
				return typeof(DesignTable);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0000F680 File Offset: 0x0000E680
		protected override INameService NameService
		{
			get
			{
				return DataSetNameService.DefaultInstance;
			}
		}

		// Token: 0x170000F7 RID: 247
		internal DesignTable this[string name]
		{
			get
			{
				return (DesignTable)this.FindObject(name);
			}
		}

		// Token: 0x170000F8 RID: 248
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

		// Token: 0x0600075C RID: 1884 RVA: 0x0000F6F8 File Offset: 0x0000E6F8
		public void Add(DesignTable designTable)
		{
			base.List.Add(designTable);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0000F707 File Offset: 0x0000E707
		public bool Contains(DesignTable table)
		{
			return base.List.Contains(table);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0000F715 File Offset: 0x0000E715
		public int IndexOf(DesignTable table)
		{
			return base.List.IndexOf(table);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0000F723 File Offset: 0x0000E723
		public void Remove(DesignTable table)
		{
			base.List.Remove(table);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0000F734 File Offset: 0x0000E734
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

		// Token: 0x06000761 RID: 1889 RVA: 0x0000F7FC File Offset: 0x0000E7FC
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

		// Token: 0x04000B8B RID: 2955
		private DesignDataSource dataSource;
	}
}
