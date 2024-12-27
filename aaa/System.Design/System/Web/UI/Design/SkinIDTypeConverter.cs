using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.UI.Design
{
	// Token: 0x0200038F RID: 911
	public class SkinIDTypeConverter : TypeConverter
	{
		// Token: 0x06002181 RID: 8577 RVA: 0x000B9B6F File Offset: 0x000B8B6F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000B9B88 File Offset: 0x000B8B88
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return value;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000B9B9D File Offset: 0x000B8B9D
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof(string) || base.CanConvertTo(context, destType);
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000B9BB6 File Offset: 0x000B8BB6
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is string)
			{
				return value;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000B9BD0 File Offset: 0x000B8BD0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context == null)
			{
				return new TypeConverter.StandardValuesCollection(new ArrayList());
			}
			Control control = context.Instance as Control;
			ArrayList arrayList = new ArrayList();
			if (control != null && control.Site != null)
			{
				IThemeResolutionService themeResolutionService = (IThemeResolutionService)control.Site.GetService(typeof(IThemeResolutionService));
				ThemeProvider stylesheetThemeProvider = themeResolutionService.GetStylesheetThemeProvider();
				ThemeProvider themeProvider = themeResolutionService.GetThemeProvider();
				if (stylesheetThemeProvider != null)
				{
					arrayList.AddRange(stylesheetThemeProvider.GetSkinsForControl(control.GetType()));
					arrayList.Remove(string.Empty);
				}
				if (themeProvider != null)
				{
					ICollection skinsForControl = themeProvider.GetSkinsForControl(control.GetType());
					foreach (object obj in skinsForControl)
					{
						string text = (string)obj;
						if (!arrayList.Contains(text))
						{
							arrayList.Add(text);
						}
					}
					arrayList.Remove(string.Empty);
				}
				arrayList.Sort();
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000B9CE0 File Offset: 0x000B8CE0
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			ThemeProvider themeProvider = null;
			if (context != null)
			{
				Control control = context.Instance as Control;
				if (control != null && control.Site != null)
				{
					IThemeResolutionService themeResolutionService = (IThemeResolutionService)control.Site.GetService(typeof(IThemeResolutionService));
					if (themeResolutionService != null)
					{
						themeProvider = themeResolutionService.GetThemeProvider();
						if (themeProvider == null)
						{
							themeProvider = themeResolutionService.GetStylesheetThemeProvider();
						}
					}
				}
			}
			return themeProvider != null;
		}
	}
}
