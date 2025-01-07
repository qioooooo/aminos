using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class ListViewItemCollectionEditor : CollectionEditor
	{
		public ListViewItemCollectionEditor(Type type)
			: base(type)
		{
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
	}
}
