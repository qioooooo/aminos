using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020005A8 RID: 1448
	public class OpacityConverter : TypeConverter
	{
		// Token: 0x06004B13 RID: 19219 RVA: 0x0011017F File Offset: 0x0010F17F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x00110198 File Offset: 0x0010F198
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = ((string)value).Replace('%', ' ').Trim();
			double num = double.Parse(text, CultureInfo.CurrentCulture);
			int num2 = ((string)value).IndexOf("%");
			if (num2 > 0 && num >= 0.0 && num <= 1.0)
			{
				text = (num / 100.0).ToString(CultureInfo.CurrentCulture);
			}
			double num3 = 1.0;
			try
			{
				num3 = (double)TypeDescriptor.GetConverter(typeof(double)).ConvertFrom(context, culture, text);
				if (num3 > 1.0)
				{
					num3 /= 100.0;
				}
			}
			catch (FormatException ex)
			{
				throw new FormatException(SR.GetString("InvalidBoundArgument", new object[] { "Opacity", text, "0%", "100%" }), ex);
			}
			if (num3 < 0.0 || num3 > 1.0)
			{
				throw new FormatException(SR.GetString("InvalidBoundArgument", new object[] { "Opacity", text, "0%", "100%" }));
			}
			return num3;
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x00110308 File Offset: 0x0010F308
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				double num = (double)value;
				return ((int)(num * 100.0)).ToString(CultureInfo.CurrentCulture) + "%";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
