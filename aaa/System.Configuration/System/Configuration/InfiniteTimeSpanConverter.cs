using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x0200006E RID: 110
	public sealed class InfiniteTimeSpanConverter : ConfigurationConverterBase
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x00013EE5 File Offset: 0x00012EE5
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(TimeSpan));
			if ((TimeSpan)value == TimeSpan.MaxValue)
			{
				return "Infinite";
			}
			return InfiniteTimeSpanConverter.s_TimeSpanConverter.ConvertToInvariantString(value);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00013F1B File Offset: 0x00012F1B
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			if ((string)data == "Infinite")
			{
				return TimeSpan.MaxValue;
			}
			return InfiniteTimeSpanConverter.s_TimeSpanConverter.ConvertFromInvariantString((string)data);
		}

		// Token: 0x0400031A RID: 794
		private static readonly TypeConverter s_TimeSpanConverter = TypeDescriptor.GetConverter(typeof(TimeSpan));
	}
}
