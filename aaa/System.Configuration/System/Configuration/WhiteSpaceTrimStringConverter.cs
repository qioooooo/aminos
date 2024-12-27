using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x020000B2 RID: 178
	public sealed class WhiteSpaceTrimStringConverter : ConfigurationConverterBase
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x0001DE6D File Offset: 0x0001CE6D
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			base.ValidateType(value, typeof(string));
			if (value == null)
			{
				return string.Empty;
			}
			return ((string)value).Trim();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001DE94 File Offset: 0x0001CE94
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			return ((string)data).Trim();
		}
	}
}
