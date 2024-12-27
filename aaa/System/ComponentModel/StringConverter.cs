using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200013D RID: 317
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class StringConverter : TypeConverter
	{
		// Token: 0x06000A56 RID: 2646 RVA: 0x00023EC5 File Offset: 0x00022EC5
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00023EDE File Offset: 0x00022EDE
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return (string)value;
			}
			if (value == null)
			{
				return "";
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
