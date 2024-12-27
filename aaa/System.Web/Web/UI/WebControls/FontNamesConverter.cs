using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200057C RID: 1404
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FontNamesConverter : TypeConverter
	{
		// Token: 0x060044E4 RID: 17636 RVA: 0x0011B07A File Offset: 0x0011A07A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0011B08C File Offset: 0x0011A08C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				throw base.GetConvertFromException(value);
			}
			if (((string)value).Length == 0)
			{
				return new string[0];
			}
			string[] array = ((string)value).Split(new char[] { culture.TextInfo.ListSeparator[0] });
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
			}
			return array;
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x0011B0FF File Offset: 0x0011A0FF
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				throw base.GetConvertToException(value, destinationType);
			}
			if (value == null)
			{
				return string.Empty;
			}
			return string.Join(culture.TextInfo.ListSeparator, (string[])value);
		}
	}
}
