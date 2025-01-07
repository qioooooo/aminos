using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class ListViewGroupCollectionEditor : CollectionEditor
	{
		public ListViewGroupCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override object CreateInstance(Type itemType)
		{
			ListViewGroup listViewGroup = (ListViewGroup)base.CreateInstance(itemType);
			listViewGroup.Name = this.CreateListViewGroupName((ListViewGroupCollection)this.editValue);
			return listViewGroup;
		}

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

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			this.editValue = value;
			object obj = base.EditValue(context, provider, value);
			this.editValue = null;
			return obj;
		}

		private object editValue;
	}
}
