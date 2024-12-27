using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000AC RID: 172
	internal sealed class DataViewManagerListItemTypeDescriptor : ICustomTypeDescriptor
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x001F928C File Offset: 0x001F868C
		internal DataViewManagerListItemTypeDescriptor(DataViewManager dataViewManager)
		{
			this.dataViewManager = dataViewManager;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x001F92A8 File Offset: 0x001F86A8
		internal void Reset()
		{
			this.propsCollection = null;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x001F92BC File Offset: 0x001F86BC
		internal DataView GetDataView(DataTable table)
		{
			DataView dataView = new DataView(table);
			dataView.SetDataViewManager(this.dataViewManager);
			return dataView;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x001F92E0 File Offset: 0x001F86E0
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x001F92F4 File Offset: 0x001F86F4
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x001F9304 File Offset: 0x001F8704
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x001F9314 File Offset: 0x001F8714
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x001F9324 File Offset: 0x001F8724
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x001F9334 File Offset: 0x001F8734
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x001F9344 File Offset: 0x001F8744
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x001F9354 File Offset: 0x001F8754
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x001F9368 File Offset: 0x001F8768
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x001F937C File Offset: 0x001F877C
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x001F9390 File Offset: 0x001F8790
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			if (this.propsCollection == null)
			{
				PropertyDescriptor[] array = null;
				DataSet dataSet = this.dataViewManager.DataSet;
				if (dataSet != null)
				{
					int count = dataSet.Tables.Count;
					array = new PropertyDescriptor[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new DataTablePropertyDescriptor(dataSet.Tables[i]);
					}
				}
				this.propsCollection = new PropertyDescriptorCollection(array);
			}
			return this.propsCollection;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x001F93FC File Offset: 0x001F87FC
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x04000856 RID: 2134
		private DataViewManager dataViewManager;

		// Token: 0x04000857 RID: 2135
		private PropertyDescriptorCollection propsCollection;
	}
}
