using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x0200006D RID: 109
	public sealed class InfiniteIntConverter : ConfigurationConverterBase
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x00013E68 File Offset: 0x00012E68
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(int));
			if ((int)value == 2147483647)
			{
				return "Infinite";
			}
			return ((int)value).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00013EAC File Offset: 0x00012EAC
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			if ((string)data == "Infinite")
			{
				return int.MaxValue;
			}
			return Convert.ToInt32((string)data, 10);
		}
	}
}
