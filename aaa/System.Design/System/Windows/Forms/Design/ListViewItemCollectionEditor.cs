using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000267 RID: 615
	internal class ListViewItemCollectionEditor : CollectionEditor
	{
		// Token: 0x06001740 RID: 5952 RVA: 0x00077F0A File Offset: 0x00076F0A
		public ListViewItemCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00077F14 File Offset: 0x00076F14
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
	}
}
