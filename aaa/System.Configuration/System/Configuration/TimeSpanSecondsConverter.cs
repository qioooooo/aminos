using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000A5 RID: 165
	public class TimeSpanSecondsConverter : ConfigurationConverterBase
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x0001D0D0 File Offset: 0x0001C0D0
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(TimeSpan));
			return ((long)((TimeSpan)value).TotalSeconds).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001D10C File Offset: 0x0001C10C
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			long num = 0L;
			try
			{
				num = long.Parse((string)data, CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new ArgumentException(SR.GetString("Converter_timespan_not_in_second"));
			}
			return TimeSpan.FromSeconds((double)num);
		}
	}
}
