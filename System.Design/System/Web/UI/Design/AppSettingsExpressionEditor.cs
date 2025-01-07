using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	public class AppSettingsExpressionEditor : ExpressionEditor
	{
		private KeyValueConfigurationCollection GetAppSettings(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
				if (webApplication != null)
				{
					Configuration configuration = webApplication.OpenWebConfiguration(true);
					if (configuration != null)
					{
						AppSettingsSection appSettings = configuration.AppSettings;
						if (appSettings != null)
						{
							return appSettings.Settings;
						}
					}
				}
			}
			return null;
		}

		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet(expression, this, serviceProvider);
		}

		public override object EvaluateExpression(string expression, object parseTimeData, Type propertyType, IServiceProvider serviceProvider)
		{
			KeyValueConfigurationCollection appSettings = this.GetAppSettings(serviceProvider);
			if (appSettings != null)
			{
				KeyValueConfigurationElement keyValueConfigurationElement = appSettings[expression];
				if (keyValueConfigurationElement != null)
				{
					return keyValueConfigurationElement.Value;
				}
			}
			return null;
		}

		private class AppSettingsExpressionEditorSheet : ExpressionEditorSheet
		{
			public AppSettingsExpressionEditorSheet(string expression, AppSettingsExpressionEditor owner, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this._owner = owner;
				this._appSetting = expression;
			}

			[SRDescription("AppSettingExpressionEditor_AppSetting")]
			[DefaultValue("")]
			[TypeConverter(typeof(AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet.AppSettingsTypeConverter))]
			public string AppSetting
			{
				get
				{
					return this._appSetting;
				}
				set
				{
					this._appSetting = value;
				}
			}

			public override bool IsValid
			{
				get
				{
					return !string.IsNullOrEmpty(this.AppSetting);
				}
			}

			public override string GetExpression()
			{
				return this._appSetting;
			}

			private AppSettingsExpressionEditor _owner;

			private string _appSetting;

			private class AppSettingsTypeConverter : TypeConverter
			{
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					if (!(value is string))
					{
						return base.ConvertFrom(context, culture, value);
					}
					if (string.Equals((string)value, AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet.AppSettingsTypeConverter.NoAppSetting, StringComparison.OrdinalIgnoreCase))
					{
						return string.Empty;
					}
					return value;
				}

				public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
				{
					return destType == typeof(string) || base.CanConvertTo(context, destType);
				}

				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (!(value is string))
					{
						return base.ConvertTo(context, culture, value, destinationType);
					}
					if (((string)value).Length == 0)
					{
						return AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet.AppSettingsTypeConverter.NoAppSetting;
					}
					return value;
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return false;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					if (context != null)
					{
						AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet appSettingsExpressionEditorSheet = (AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet)context.Instance;
						AppSettingsExpressionEditor owner = appSettingsExpressionEditorSheet._owner;
						KeyValueConfigurationCollection appSettings = owner.GetAppSettings(appSettingsExpressionEditorSheet.ServiceProvider);
						if (appSettings != null)
						{
							return appSettings.Count > 0;
						}
					}
					return base.GetStandardValuesSupported(context);
				}

				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					if (context != null)
					{
						AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet appSettingsExpressionEditorSheet = (AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet)context.Instance;
						AppSettingsExpressionEditor owner = appSettingsExpressionEditorSheet._owner;
						KeyValueConfigurationCollection appSettings = owner.GetAppSettings(appSettingsExpressionEditorSheet.ServiceProvider);
						if (appSettings != null)
						{
							ArrayList arrayList = new ArrayList(appSettings.AllKeys);
							arrayList.Sort();
							arrayList.Add(string.Empty);
							return new TypeConverter.StandardValuesCollection(arrayList);
						}
					}
					return base.GetStandardValues(context);
				}

				private static readonly string NoAppSetting = "(None)";
			}
		}
	}
}
