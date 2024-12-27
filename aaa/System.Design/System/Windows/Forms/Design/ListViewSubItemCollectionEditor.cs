using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000268 RID: 616
	internal class ListViewSubItemCollectionEditor : CollectionEditor
	{
		// Token: 0x06001742 RID: 5954 RVA: 0x00077F8B File Offset: 0x00076F8B
		public ListViewSubItemCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x00077F94 File Offset: 0x00076F94
		protected override object CreateInstance(Type type)
		{
			object obj = base.CreateInstance(type);
			if (obj is ListViewItem.ListViewSubItem)
			{
				((ListViewItem.ListViewSubItem)obj).Name = SR.GetString("ListViewSubItemBaseName") + ListViewSubItemCollectionEditor.count++;
			}
			return obj;
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00077FE0 File Offset: 0x00076FE0
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

		// Token: 0x06001745 RID: 5957 RVA: 0x00078058 File Offset: 0x00077058
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

		// Token: 0x06001746 RID: 5958 RVA: 0x000780B0 File Offset: 0x000770B0
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

		// Token: 0x04001323 RID: 4899
		private static int count;

		// Token: 0x04001324 RID: 4900
		private ListViewItem.ListViewSubItem firstSubItem;
	}
}
