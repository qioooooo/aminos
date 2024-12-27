using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Compilation;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001E5 RID: 485
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class GlobalizationSection : ConfigurationSection
	{
		// Token: 0x06001ACF RID: 6863 RVA: 0x0007C22C File Offset: 0x0007B22C
		static GlobalizationSection()
		{
			GlobalizationSection._properties.Add(GlobalizationSection._propRequestEncoding);
			GlobalizationSection._properties.Add(GlobalizationSection._propResponseEncoding);
			GlobalizationSection._properties.Add(GlobalizationSection._propFileEncoding);
			GlobalizationSection._properties.Add(GlobalizationSection._propCulture);
			GlobalizationSection._properties.Add(GlobalizationSection._propUICulture);
			GlobalizationSection._properties.Add(GlobalizationSection._propEnableClientBasedCulture);
			GlobalizationSection._properties.Add(GlobalizationSection._propResponseHeaderEncoding);
			GlobalizationSection._properties.Add(GlobalizationSection._propResourceProviderFactoryType);
			GlobalizationSection._properties.Add(GlobalizationSection._propEnableBestFitResponseEncoding);
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x0007C3FA File Offset: 0x0007B3FA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return GlobalizationSection._properties;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x0007C401 File Offset: 0x0007B401
		// (set) Token: 0x06001AD3 RID: 6867 RVA: 0x0007C41C File Offset: 0x0007B41C
		[ConfigurationProperty("requestEncoding", DefaultValue = "utf-8")]
		public Encoding RequestEncoding
		{
			get
			{
				if (this.requestEncodingCache == null)
				{
					this.requestEncodingCache = Encoding.UTF8;
				}
				return this.requestEncodingCache;
			}
			set
			{
				if (value != null)
				{
					base[GlobalizationSection._propRequestEncoding] = value.WebName;
					this.requestEncodingCache = value;
					return;
				}
				base[GlobalizationSection._propRequestEncoding] = value;
				this.requestEncodingCache = Encoding.UTF8;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x0007C451 File Offset: 0x0007B451
		// (set) Token: 0x06001AD5 RID: 6869 RVA: 0x0007C46C File Offset: 0x0007B46C
		[ConfigurationProperty("responseEncoding", DefaultValue = "utf-8")]
		public Encoding ResponseEncoding
		{
			get
			{
				if (this.responseEncodingCache == null)
				{
					this.responseEncodingCache = Encoding.UTF8;
				}
				return this.responseEncodingCache;
			}
			set
			{
				if (value != null)
				{
					base[GlobalizationSection._propResponseEncoding] = value.WebName;
					this.responseEncodingCache = value;
					return;
				}
				base[GlobalizationSection._propResponseEncoding] = value;
				this.responseEncodingCache = Encoding.UTF8;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0007C4A1 File Offset: 0x0007B4A1
		// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x0007C4BC File Offset: 0x0007B4BC
		[ConfigurationProperty("responseHeaderEncoding", DefaultValue = "utf-8")]
		public Encoding ResponseHeaderEncoding
		{
			get
			{
				if (this.responseHeaderEncodingCache == null)
				{
					this.responseHeaderEncodingCache = Encoding.UTF8;
				}
				return this.responseHeaderEncodingCache;
			}
			set
			{
				if (value != null)
				{
					base[GlobalizationSection._propResponseHeaderEncoding] = value.WebName;
					this.responseHeaderEncodingCache = value;
					return;
				}
				base[GlobalizationSection._propResponseHeaderEncoding] = value;
				this.responseHeaderEncodingCache = Encoding.UTF8;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x0007C4F1 File Offset: 0x0007B4F1
		// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x0007C50C File Offset: 0x0007B50C
		[ConfigurationProperty("fileEncoding")]
		public Encoding FileEncoding
		{
			get
			{
				if (this.fileEncodingCache == null)
				{
					this.fileEncodingCache = Encoding.Default;
				}
				return this.fileEncodingCache;
			}
			set
			{
				if (value != null)
				{
					base[GlobalizationSection._propFileEncoding] = value.WebName;
					this.fileEncodingCache = value;
					return;
				}
				base[GlobalizationSection._propFileEncoding] = value;
				this.fileEncodingCache = Encoding.Default;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0007C541 File Offset: 0x0007B541
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x0007C567 File Offset: 0x0007B567
		[ConfigurationProperty("culture", DefaultValue = "")]
		public string Culture
		{
			get
			{
				if (this.cultureCache == null)
				{
					this.cultureCache = (string)base[GlobalizationSection._propCulture];
				}
				return this.cultureCache;
			}
			set
			{
				base[GlobalizationSection._propCulture] = value;
				this.cultureCache = value;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x0007C57C File Offset: 0x0007B57C
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x0007C5A2 File Offset: 0x0007B5A2
		[ConfigurationProperty("uiCulture", DefaultValue = "")]
		public string UICulture
		{
			get
			{
				if (this.uiCultureCache == null)
				{
					this.uiCultureCache = (string)base[GlobalizationSection._propUICulture];
				}
				return this.uiCultureCache;
			}
			set
			{
				base[GlobalizationSection._propUICulture] = value;
				this.uiCultureCache = value;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0007C5B7 File Offset: 0x0007B5B7
		// (set) Token: 0x06001ADF RID: 6879 RVA: 0x0007C5C9 File Offset: 0x0007B5C9
		[ConfigurationProperty("enableClientBasedCulture", DefaultValue = false)]
		public bool EnableClientBasedCulture
		{
			get
			{
				return (bool)base[GlobalizationSection._propEnableClientBasedCulture];
			}
			set
			{
				base[GlobalizationSection._propEnableClientBasedCulture] = value;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0007C5DC File Offset: 0x0007B5DC
		// (set) Token: 0x06001AE1 RID: 6881 RVA: 0x0007C5EE File Offset: 0x0007B5EE
		[ConfigurationProperty("resourceProviderFactoryType", DefaultValue = "")]
		public string ResourceProviderFactoryType
		{
			get
			{
				return (string)base[GlobalizationSection._propResourceProviderFactoryType];
			}
			set
			{
				base[GlobalizationSection._propResourceProviderFactoryType] = value;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0007C5FC File Offset: 0x0007B5FC
		// (set) Token: 0x06001AE3 RID: 6883 RVA: 0x0007C60E File Offset: 0x0007B60E
		[ConfigurationProperty("enableBestFitResponseEncoding", DefaultValue = false)]
		public bool EnableBestFitResponseEncoding
		{
			get
			{
				return (bool)base[GlobalizationSection._propEnableBestFitResponseEncoding];
			}
			set
			{
				base[GlobalizationSection._propEnableBestFitResponseEncoding] = value;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0007C624 File Offset: 0x0007B624
		internal Type ResourceProviderFactoryTypeInternal
		{
			get
			{
				if (this._resourceProviderFactoryType == null && !string.IsNullOrEmpty(this.ResourceProviderFactoryType))
				{
					lock (this)
					{
						if (this._resourceProviderFactoryType == null)
						{
							Type type = ConfigUtil.GetType(this.ResourceProviderFactoryType, "resourceProviderFactoryType", this);
							ConfigUtil.CheckBaseType(typeof(ResourceProviderFactory), type, "resourceProviderFactoryType", this);
							this._resourceProviderFactoryType = type;
						}
					}
				}
				return this._resourceProviderFactoryType;
			}
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0007C6A4 File Offset: 0x0007B6A4
		private void CheckCulture(string configCulture)
		{
			if (StringUtil.EqualsIgnoreCase(configCulture, HttpApplication.AutoCulture))
			{
				return;
			}
			if (StringUtil.StringStartsWithIgnoreCase(configCulture, HttpApplication.AutoCulture))
			{
				new CultureInfo(configCulture.Substring(5));
				return;
			}
			new CultureInfo(configCulture);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0007C6D6 File Offset: 0x0007B6D6
		protected override void PreSerialize(XmlWriter writer)
		{
			this.PostDeserialize();
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0007C6E0 File Offset: 0x0007B6E0
		protected override void PostDeserialize()
		{
			ConfigurationPropertyCollection properties = this.Properties;
			ConfigurationProperty configurationProperty = null;
			int num = int.MaxValue;
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propResponseEncoding]))
				{
					this.responseEncodingCache = Encoding.GetEncoding((string)base[GlobalizationSection._propResponseEncoding]);
				}
			}
			catch
			{
				configurationProperty = GlobalizationSection._propResponseEncoding;
				num = base.ElementInformation.Properties[configurationProperty.Name].LineNumber;
			}
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propResponseHeaderEncoding]))
				{
					this.responseHeaderEncodingCache = Encoding.GetEncoding((string)base[GlobalizationSection._propResponseHeaderEncoding]);
				}
			}
			catch
			{
				if (num > base.ElementInformation.Properties[GlobalizationSection._propResponseHeaderEncoding.Name].LineNumber)
				{
					configurationProperty = GlobalizationSection._propResponseHeaderEncoding;
					num = base.ElementInformation.Properties[configurationProperty.Name].LineNumber;
				}
			}
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propRequestEncoding]))
				{
					this.requestEncodingCache = Encoding.GetEncoding((string)base[GlobalizationSection._propRequestEncoding]);
				}
			}
			catch
			{
				if (num > base.ElementInformation.Properties[GlobalizationSection._propRequestEncoding.Name].LineNumber)
				{
					configurationProperty = GlobalizationSection._propRequestEncoding;
					num = base.ElementInformation.Properties[configurationProperty.Name].LineNumber;
				}
			}
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propFileEncoding]))
				{
					this.fileEncodingCache = Encoding.GetEncoding((string)base[GlobalizationSection._propFileEncoding]);
				}
			}
			catch
			{
				if (num > base.ElementInformation.Properties[GlobalizationSection._propFileEncoding.Name].LineNumber)
				{
					configurationProperty = GlobalizationSection._propFileEncoding;
					num = base.ElementInformation.Properties[configurationProperty.Name].LineNumber;
				}
			}
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propCulture]))
				{
					this.CheckCulture((string)base[GlobalizationSection._propCulture]);
				}
			}
			catch
			{
				if (num > base.ElementInformation.Properties[GlobalizationSection._propCulture.Name].LineNumber)
				{
					configurationProperty = GlobalizationSection._propCulture;
					num = base.ElementInformation.Properties[GlobalizationSection._propCulture.Name].LineNumber;
				}
			}
			try
			{
				if (!string.IsNullOrEmpty((string)base[GlobalizationSection._propUICulture]))
				{
					this.CheckCulture((string)base[GlobalizationSection._propUICulture]);
				}
			}
			catch
			{
				if (num > base.ElementInformation.Properties[GlobalizationSection._propUICulture.Name].LineNumber)
				{
					configurationProperty = GlobalizationSection._propUICulture;
					num = base.ElementInformation.Properties[GlobalizationSection._propUICulture.Name].LineNumber;
				}
			}
			if (configurationProperty != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_value_for_globalization_attr", new object[] { configurationProperty.Name }), base.ElementInformation.Properties[configurationProperty.Name].Source, base.ElementInformation.Properties[configurationProperty.Name].LineNumber);
			}
		}

		// Token: 0x04001800 RID: 6144
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001801 RID: 6145
		private static readonly ConfigurationProperty _propRequestEncoding = new ConfigurationProperty("requestEncoding", typeof(string), Encoding.UTF8.WebName, ConfigurationPropertyOptions.None);

		// Token: 0x04001802 RID: 6146
		private static readonly ConfigurationProperty _propResponseEncoding = new ConfigurationProperty("responseEncoding", typeof(string), Encoding.UTF8.WebName, ConfigurationPropertyOptions.None);

		// Token: 0x04001803 RID: 6147
		private static readonly ConfigurationProperty _propFileEncoding = new ConfigurationProperty("fileEncoding", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001804 RID: 6148
		private static readonly ConfigurationProperty _propCulture = new ConfigurationProperty("culture", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001805 RID: 6149
		private static readonly ConfigurationProperty _propUICulture = new ConfigurationProperty("uiCulture", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001806 RID: 6150
		private static readonly ConfigurationProperty _propEnableClientBasedCulture = new ConfigurationProperty("enableClientBasedCulture", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001807 RID: 6151
		private static readonly ConfigurationProperty _propResponseHeaderEncoding = new ConfigurationProperty("responseHeaderEncoding", typeof(string), Encoding.UTF8.WebName, ConfigurationPropertyOptions.None);

		// Token: 0x04001808 RID: 6152
		private static readonly ConfigurationProperty _propResourceProviderFactoryType = new ConfigurationProperty("resourceProviderFactoryType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001809 RID: 6153
		private static readonly ConfigurationProperty _propEnableBestFitResponseEncoding = new ConfigurationProperty("enableBestFitResponseEncoding", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400180A RID: 6154
		private Encoding responseEncodingCache;

		// Token: 0x0400180B RID: 6155
		private Encoding responseHeaderEncodingCache;

		// Token: 0x0400180C RID: 6156
		private Encoding requestEncodingCache;

		// Token: 0x0400180D RID: 6157
		private Encoding fileEncodingCache;

		// Token: 0x0400180E RID: 6158
		private string cultureCache;

		// Token: 0x0400180F RID: 6159
		private string uiCultureCache;

		// Token: 0x04001810 RID: 6160
		private Type _resourceProviderFactoryType;
	}
}
