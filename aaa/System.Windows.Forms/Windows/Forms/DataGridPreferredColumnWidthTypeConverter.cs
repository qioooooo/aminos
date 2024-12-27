using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002D6 RID: 726
	public class DataGridPreferredColumnWidthTypeConverter : TypeConverter
	{
		// Token: 0x06002A28 RID: 10792 RVA: 0x0006F81F File Offset: 0x0006E81F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(int);
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0006F840 File Offset: 0x0006E840
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value.GetType() != typeof(int))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			int num = (int)value;
			if (num == -1)
			{
				return "AutoColumnResize (-1)";
			}
			return num.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0006F8A4 File Offset: 0x0006E8A4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				string text = value.ToString();
				if (text.Equals("AutoColumnResize (-1)"))
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.CurrentCulture);
			}
			else
			{
				if (value.GetType() == typeof(int))
				{
					return (int)value;
				}
				throw base.GetConvertFromException(value);
			}
		}
	}
}
