using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000266 RID: 614
	internal class ListViewGroupCollectionEditor : CollectionEditor
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x00077DE8 File Offset: 0x00076DE8
		public ListViewGroupCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00077DF4 File Offset: 0x00076DF4
		protected override object CreateInstance(Type itemType)
		{
			ListViewGroup listViewGroup = (ListViewGroup)base.CreateInstance(itemType);
			listViewGroup.Name = this.CreateListViewGroupName((ListViewGroupCollection)this.editValue);
			return listViewGroup;
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00077E28 File Offset: 0x00076E28
		private string CreateListViewGroupName(ListViewGroupCollection lvgCollection)
		{
			string text = "ListViewGroup";
			INameCreationService nameCreationService = base.GetService(typeof(INameCreationService)) as INameCreationService;
			IContainer container = base.GetService(typeof(IContainer)) as IContainer;
			if (nameCreationService != null && container != null)
			{
				text = nameCreationService.CreateName(container, typeof(ListViewGroup));
			}
			while (char.IsDigit(text[text.Length - 1]))
			{
				text = text.Substring(0, text.Length - 1);
			}
			int num = 1;
			string text2 = text + num.ToString(CultureInfo.CurrentCulture);
			while (lvgCollection[text2] != null)
			{
				num++;
				text2 = text + num.ToString(CultureInfo.CurrentCulture);
			}
			return text2;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00077EE4 File Offset: 0x00076EE4
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			this.editValue = value;
			object obj = base.EditValue(context, provider, value);
			this.editValue = null;
			return obj;
		}

		// Token: 0x04001322 RID: 4898
		private object editValue;
	}
}
