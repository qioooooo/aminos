using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Design;
using System.Globalization;

namespace System.Web.UI.Design
{
	// Token: 0x0200031C RID: 796
	public class AppSettingsExpressionEditor : ExpressionEditor
	{
		// Token: 0x06001E0D RID: 7693 RVA: 0x000AB804 File Offset: 0x000AA804
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

		// Token: 0x06001E0E RID: 7694 RVA: 0x000AB84A File Offset: 0x000AA84A
		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new AppSettingsExpressionEditor.AppSettingsExpressionEditorSheet(expression, this, serviceProvider);
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000AB854 File Offset: 0x000AA854
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

		// Token: 0x0200031D RID: 797
		private class AppSettingsExpressionEditorSheet : ExpressionEditorSheet
		{
			// Token: 0x06001E11 RID: 7697 RVA: 0x000AB888 File Offset: 0x000AA888
			public AppSettingsExpressionEditorSheet(string expression, AppSettingsExpressionEditor owner, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this._owner = owner;
				this._appSetting = expression;
			}

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x06001E12 RID: 7698 RVA: 0x000AB89F File Offset: 0x000AA89F
			// (set) Token: 0x06001E13 RID: 7699 RVA: 0x000AB8A7 File Offset: 0x000AA8A7
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

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x06001E14 RID: 7700 RVA: 0x000AB8B0 File Offset: 0x000AA8B0
			public override bool IsValid
			{
				get
				{
					return !string.IsNullOrEmpty(this.AppSetting);
				}
			}

			// Token: 0x06001E15 RID: 7701 RVA: 0x000AB8C0 File Offset: 0x000AA8C0
			public override string GetExpression()
			{
				return this._appSetting;
			}

			// Token: 0x04001727 RID: 5927
			private AppSettingsExpressionEditor _owner;

			// Token: 0x04001728 RID: 5928
			private string _appSetting;

			// Token: 0x0200031E RID: 798
			private class AppSettingsTypeConverter : TypeConverter
			{
				// Token: 0x06001E16 RID: 7702 RVA: 0x000AB8C8 File Offset: 0x000AA8C8
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

				// Token: 0x06001E17 RID: 7703 RVA: 0x000AB8E1 File Offset: 0x000AA8E1
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

				// Token: 0x06001E18 RID: 7704 RVA: 0x000AB90F File Offset: 0x000AA90F
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
				{
					return destType == typeof(string) || base.CanConvertTo(context, destType);
				}

				// Token: 0x06001E19 RID: 7705 RVA: 0x000AB928 File Offset: 0x000AA928
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

				// Token: 0x06001E1A RID: 7706 RVA: 0x000AB952 File Offset: 0x000AA952
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return false;
				}

				// Token: 0x06001E1B RID: 7707 RVA: 0x000AB958 File Offset: 0x000AA958
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

				// Token: 0x06001E1C RID: 7708 RVA: 0x000AB99C File Offset: 0x000AA99C
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

				// Token: 0x04001729 RID: 5929
				private static readonly string NoAppSetting = "(None)";
			}
		}
	}
}
