using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class TabPageCollectionEditor : CollectionEditor
	{
		public TabPageCollectionEditor()
			: base(typeof(TabControl.TabPageCollection))
		{
		}

		protected override object SetItems(object editValue, object[] value)
		{
			TabControl tabControl = base.Context.Instance as TabControl;
			if (tabControl != null)
			{
				tabControl.SuspendLayout();
			}
			foreach (object obj in value)
			{
				TabPage tabPage = obj as TabPage;
				if (tabPage != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(tabPage)["UseVisualStyleBackColor"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
					{
						propertyDescriptor.SetValue(tabPage, true);
					}
				}
			}
			object obj2 = base.SetItems(editValue, value);
			if (tabControl != null)
			{
				tabControl.ResumeLayout();
			}
			return obj2;
		}
	}
}
