using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000A0 RID: 160
	internal sealed class DataTablePropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x001F4478 File Offset: 0x001F3878
		public DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x001F448C File Offset: 0x001F388C
		internal DataTablePropertyDescriptor(DataTable dataTable)
			: base(dataTable.TableName, null)
		{
			this.table = dataTable;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x001F44B0 File Offset: 0x001F38B0
		public override Type ComponentType
		{
			get
			{
				return typeof(DataRowView);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x001F44C8 File Offset: 0x001F38C8
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x001F44D8 File Offset: 0x001F38D8
		public override Type PropertyType
		{
			get
			{
				return typeof(IBindingList);
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x001F44F0 File Offset: 0x001F38F0
		public override bool Equals(object other)
		{
			if (other is DataTablePropertyDescriptor)
			{
				DataTablePropertyDescriptor dataTablePropertyDescriptor = (DataTablePropertyDescriptor)other;
				return dataTablePropertyDescriptor.Table == this.Table;
			}
			return false;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x001F451C File Offset: 0x001F391C
		public override int GetHashCode()
		{
			return this.Table.GetHashCode();
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x001F4534 File Offset: 0x001F3934
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x001F4544 File Offset: 0x001F3944
		public override object GetValue(object component)
		{
			DataViewManagerListItemTypeDescriptor dataViewManagerListItemTypeDescriptor = (DataViewManagerListItemTypeDescriptor)component;
			return dataViewManagerListItemTypeDescriptor.GetDataView(this.table);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x001F4564 File Offset: 0x001F3964
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x001F4574 File Offset: 0x001F3974
		public override void SetValue(object component, object value)
		{
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x001F4584 File Offset: 0x001F3984
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x04000817 RID: 2071
		private DataTable table;
	}
}
