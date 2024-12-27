using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200020B RID: 523
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class LowerCaseStringConverter : TypeConverter
	{
		// Token: 0x06001C35 RID: 7221 RVA: 0x000810A9 File Offset: 0x000800A9
		public override bool CanConvertTo(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000810B8 File Offset: 0x000800B8
		public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000810C7 File Offset: 0x000800C7
		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
		{
			if (value == null)
			{
				return string.Empty;
			}
			return ((string)value).ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000810E2 File Offset: 0x000800E2
		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			return ((string)data).ToLower(CultureInfo.InvariantCulture);
		}
	}
}
