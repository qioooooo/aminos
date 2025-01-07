using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ListViewSubItemCollectionEditor : CollectionEditor
	{
		public ListViewSubItemCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override object CreateInstance(Type type)
		{
			object obj = base.CreateInstance(type);
			if (obj is ListViewItem.ListViewSubItem)
			{
				((ListViewItem.ListViewSubItem)obj).Name = SR.GetString("ListViewSubItemBaseName") + ListViewSubItemCollectionEditor.count++;
			}
			return obj;
		}

		protected override string GetDisplayText(object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(base.CollectionType);
			string text;
			if (defaultProperty != null && defaultProperty.PropertyType == typeof(string))
			{
				text = (string)defaultProperty.GetValue(value);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			text = TypeDescriptor.GetConverter(value).ConvertToString(value);
			if (text == null || text.Length == 0)
			{
				text = value.GetType().Name;
			}
			return text;
		}

		protected override object[] GetItems(object editValue)
		{
			ListViewItem.ListViewSubItemCollection listViewSubItemCollection = (ListViewItem.ListViewSubItemCollection)editValue;
			object[] array = new object[listViewSubItemCollection.Count];
			((ICollection)listViewSubItemCollection).CopyTo(array, 0);
			if (array.Length > 0)
			{
				this.firstSubItem = listViewSubItemCollection[0];
				object[] array2 = new object[array.Length - 1];
				Array.Copy(array, 1, array2, 0, array2.Length);
				array = array2;
			}
			return array;
		}

		protected override object SetItems(object editValue, object[] value)
		{
			IList list = editValue as IList;
			list.Clear();
			list.Add(this.firstSubItem);
			for (int i = 0; i < value.Length; i++)
			{
				list.Add(value[i]);
			}
			return editValue;
		}

		private static int count;

		private ListViewItem.ListViewSubItem firstSubItem;
	}
}
