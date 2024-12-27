using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000A3 RID: 163
	public class TimeSpanMinutesConverter : ConfigurationConverterBase
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x0001CFFC File Offset: 0x0001BFFC
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(TimeSpan));
			return ((long)((TimeSpan)value).TotalMinutes).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001D038 File Offset: 0x0001C038
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			long num = long.Parse((string)data, CultureInfo.InvariantCulture);
			return TimeSpan.FromMinutes((double)num);
		}
	}
}
