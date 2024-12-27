using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001AB RID: 427
	internal class ColumnHeaderCollectionEditor : CollectionEditor
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x0004AB31 File Offset: 0x00049B31
		public ColumnHeaderCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0004AB3A File Offset: 0x00049B3A
		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.ColumnHeaderCollectionEditor";
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0004AB44 File Offset: 0x00049B44
		protected override object SetItems(object editValue, object[] value)
		{
			if (editValue != null)
			{
				Array items = this.GetItems(editValue);
				int length = items.Length;
				int num = value.Length;
				ListView.ColumnHeaderCollection columnHeaderCollection = editValue as ListView.ColumnHeaderCollection;
				if (editValue != null)
				{
					columnHeaderCollection.Clear();
					ColumnHeader[] array = new ColumnHeader[value.Length];
					Array.Copy(value, 0, array, 0, value.Length);
					columnHeaderCollection.AddRange(array);
				}
			}
			return editValue;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0004AB94 File Offset: 0x00049B94
		internal override void OnItemRemoving(object item)
		{
			ListView listView = base.Context.Instance as ListView;
			if (listView == null)
			{
				return;
			}
			ColumnHeader columnHeader = item as ColumnHeader;
			if (columnHeader != null)
			{
				IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = null;
				if (componentChangeService != null)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(base.Context.Instance)["Columns"];
					componentChangeService.OnComponentChanging(base.Context.Instance, propertyDescriptor);
				}
				listView.Columns.Remove(columnHeader);
				if (componentChangeService != null && propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanged(base.Context.Instance, propertyDescriptor, null, null);
				}
			}
		}
	}
}
