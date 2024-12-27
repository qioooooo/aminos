using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A4 RID: 676
	internal class TabPageCollectionEditor : CollectionEditor
	{
		// Token: 0x06001974 RID: 6516 RVA: 0x000895E8 File Offset: 0x000885E8
		public TabPageCollectionEditor()
			: base(typeof(TabControl.TabPageCollection))
		{
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000895FC File Offset: 0x000885FC
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
