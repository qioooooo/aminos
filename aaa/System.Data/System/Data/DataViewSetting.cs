using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000AE RID: 174
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class DataViewSetting
	{
		// Token: 0x06000BF6 RID: 3062 RVA: 0x001F940C File Offset: 0x001F880C
		internal DataViewSetting()
		{
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x001F9440 File Offset: 0x001F8840
		internal DataViewSetting(string sort, string rowFilter, DataViewRowState rowStateFilter)
		{
			this.sort = sort;
			this.rowFilter = rowFilter;
			this.rowStateFilter = rowStateFilter;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x001F9488 File Offset: 0x001F8888
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x001F949C File Offset: 0x001F889C
		public bool ApplyDefaultSort
		{
			get
			{
				return this.applyDefaultSort;
			}
			set
			{
				if (this.applyDefaultSort != value)
				{
					this.applyDefaultSort = value;
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x001F94BC File Offset: 0x001F88BC
		[Browsable(false)]
		public DataViewManager DataViewManager
		{
			get
			{
				return this.dataViewManager;
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x001F94D0 File Offset: 0x001F88D0
		internal void SetDataViewManager(DataViewManager dataViewManager)
		{
			if (this.dataViewManager != dataViewManager)
			{
				DataViewManager dataViewManager2 = this.dataViewManager;
				this.dataViewManager = dataViewManager;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x001F94F4 File Offset: 0x001F88F4
		[Browsable(false)]
		public DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x001F9508 File Offset: 0x001F8908
		internal void SetDataTable(DataTable table)
		{
			if (this.table != table)
			{
				DataTable dataTable = this.table;
				this.table = table;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x001F952C File Offset: 0x001F892C
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x001F9540 File Offset: 0x001F8940
		public string RowFilter
		{
			get
			{
				return this.rowFilter;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.rowFilter != value)
				{
					this.rowFilter = value;
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x001F956C File Offset: 0x001F896C
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x001F9580 File Offset: 0x001F8980
		public DataViewRowState RowStateFilter
		{
			get
			{
				return this.rowStateFilter;
			}
			set
			{
				if (this.rowStateFilter != value)
				{
					this.rowStateFilter = value;
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x001F95A0 File Offset: 0x001F89A0
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x001F95B4 File Offset: 0x001F89B4
		public string Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.sort != value)
				{
					this.sort = value;
				}
			}
		}

		// Token: 0x04000861 RID: 2145
		private DataViewManager dataViewManager;

		// Token: 0x04000862 RID: 2146
		private DataTable table;

		// Token: 0x04000863 RID: 2147
		private string sort = "";

		// Token: 0x04000864 RID: 2148
		private string rowFilter = "";

		// Token: 0x04000865 RID: 2149
		private DataViewRowState rowStateFilter = DataViewRowState.CurrentRows;

		// Token: 0x04000866 RID: 2150
		private bool applyDefaultSort;
	}
}
