using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Web.UI.Design
{
	public class SkinIDTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return value;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof(string) || base.CanConvertTo(context, destType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is string)
			{
				return value;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

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
