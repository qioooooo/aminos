using System;
using System.ComponentModel;
using System.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001F4 RID: 500
	internal class DataMemberFieldConverter : TypeConverter
	{
		// Token: 0x06001338 RID: 4920 RVA: 0x000624F3 File Offset: 0x000614F3
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0006250C File Offset: 0x0006150C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null && value.Equals(SR.GetString("None")))
			{
				return string.Empty;
			}
			return value;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0006252A File Offset: 0x0006152A
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && (value == null || value.Equals(string.Empty)))
			{
				return SR.GetString("None_lc");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
