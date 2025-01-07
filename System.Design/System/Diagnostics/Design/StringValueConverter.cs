using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	internal class StringValueConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text == string.Empty)
				{
					text = null;
				}
				return text;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
