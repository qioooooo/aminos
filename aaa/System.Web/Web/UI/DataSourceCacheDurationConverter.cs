using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003D9 RID: 985
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataSourceCacheDurationConverter : Int32Converter
	{
		// Token: 0x06002FF9 RID: 12281 RVA: 0x000D4AA2 File Offset: 0x000D3AA2
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000D4ABC File Offset: 0x000D3ABC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return null;
			}
			string text = value as string;
			if (text != null)
			{
				string text2 = text.Trim();
				if (text2.Length == 0)
				{
					return 0;
				}
				if (string.Equals(text2, "infinite", StringComparison.OrdinalIgnoreCase))
				{
					return 0;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000D4B0C File Offset: 0x000D3B0C
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000D4B25 File Offset: 0x000D3B25
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value != null && destinationType == typeof(string) && (int)value == 0)
			{
				return "Infinite";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000D4B54 File Offset: 0x000D3B54
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this._values == null)
			{
				object[] array = new object[] { 0 };
				this._values = new TypeConverter.StandardValuesCollection(array);
			}
			return this._values;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000D4B8D File Offset: 0x000D3B8D
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000D4B90 File Offset: 0x000D3B90
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x040021FB RID: 8699
		private TypeConverter.StandardValuesCollection _values;
	}
}
