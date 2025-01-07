using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class ColumnHeaderCollectionEditor : CollectionEditor
	{
		public ColumnHeaderCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.ColumnHeaderCollectionEditor";
			}
		}

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
