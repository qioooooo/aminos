using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Diagnostics.Design
{
	// Token: 0x020000F0 RID: 240
	internal class StringValueConverter : TypeConverter
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x00025FD4 File Offset: 0x00024FD4
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00025FF0 File Offset: 0x00024FF0
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
