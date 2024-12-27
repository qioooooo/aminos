using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data
{
	// Token: 0x0200006C RID: 108
	internal sealed class DataColumnPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x001D9AE0 File Offset: 0x001D8EE0
		internal DataColumnPropertyDescriptor(DataColumn dataColumn)
			: base(dataColumn.ColumnName, null)
		{
			this.column = dataColumn;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x001D9B04 File Offset: 0x001D8F04
		public override AttributeCollection Attributes
		{
			get
			{
				if (typeof(IList).IsAssignableFrom(this.PropertyType))
				{
					Attribute[] array = new Attribute[base.Attributes.Count + 1];
					base.Attributes.CopyTo(array, 0);
					array[array.Length - 1] = new ListBindableAttribute(false);
					return new AttributeCollection(array);
				}
				return base.Attributes;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x001D9B64 File Offset: 0x001D8F64
		internal DataColumn Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x001D9B78 File Offset: 0x001D8F78
		public override Type ComponentType
		{
			get
			{
				return typeof(DataRowView);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x001D9B90 File Offset: 0x001D8F90
		public override bool IsReadOnly
		{
			get
			{
				return this.column.ReadOnly;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x001D9BA8 File Offset: 0x001D8FA8
		public override Type PropertyType
		{
			get
			{
				return this.column.DataType;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x001D9BC0 File Offset: 0x001D8FC0
		public override bool Equals(object other)
		{
			if (other is DataColumnPropertyDescriptor)
			{
				DataColumnPropertyDescriptor dataColumnPropertyDescriptor = (DataColumnPropertyDescriptor)other;
				return dataColumnPropertyDescriptor.Column == this.Column;
			}
			return false;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x001D9BEC File Offset: 0x001D8FEC
		public override int GetHashCode()
		{
			return this.Column.GetHashCode();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x001D9C04 File Offset: 0x001D9004
		public override bool CanResetValue(object component)
		{
			DataRowView dataRowView = (DataRowView)component;
			if (!this.column.IsSqlType)
			{
				return dataRowView.GetColumnValue(this.column) != DBNull.Value;
			}
			return !DataStorage.IsObjectNull(dataRowView.GetColumnValue(this.column));
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x001D9C50 File Offset: 0x001D9050
		public override object GetValue(object component)
		{
			DataRowView dataRowView = (DataRowView)component;
			return dataRowView.GetColumnValue(this.column);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x001D9C70 File Offset: 0x001D9070
		public override void ResetValue(object component)
		{
			DataRowView dataRowView = (DataRowView)component;
			dataRowView.SetColumnValue(this.column, DBNull.Value);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x001D9C98 File Offset: 0x001D9098
		public override void SetValue(object component, object value)
		{
			DataRowView dataRowView = (DataRowView)component;
			dataRowView.SetColumnValue(this.column, value);
			this.OnValueChanged(component, EventArgs.Empty);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x001D9CC8 File Offset: 0x001D90C8
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x001D9CD8 File Offset: 0x001D90D8
		public override bool IsBrowsable
		{
			get
			{
				return this.column.ColumnMapping != MappingType.Hidden && base.IsBrowsable;
			}
		}

		// Token: 0x04000708 RID: 1800
		private DataColumn column;
	}
}
