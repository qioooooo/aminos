using System;

namespace System.Configuration
{
	// Token: 0x02000074 RID: 116
	public class KeyValueConfigurationElement : ConfigurationElement
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x000141FC File Offset: 0x000131FC
		static KeyValueConfigurationElement()
		{
			KeyValueConfigurationElement._properties.Add(KeyValueConfigurationElement._propKey);
			KeyValueConfigurationElement._properties.Add(KeyValueConfigurationElement._propValue);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x0001426F File Offset: 0x0001326F
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return KeyValueConfigurationElement._properties;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00014276 File Offset: 0x00013276
		internal KeyValueConfigurationElement()
		{
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001427E File Offset: 0x0001327E
		public KeyValueConfigurationElement(string key, string value)
		{
			this._needsInit = true;
			this._initKey = key;
			this._initValue = value;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001429B File Offset: 0x0001329B
		protected internal override void Init()
		{
			base.Init();
			if (this._needsInit)
			{
				this._needsInit = false;
				base[KeyValueConfigurationElement._propKey] = this._initKey;
				this.Value = this._initValue;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x000142CF File Offset: 0x000132CF
		[ConfigurationProperty("key", Options = ConfigurationPropertyOptions.IsKey, DefaultValue = "")]
		public string Key
		{
			get
			{
				return (string)base[KeyValueConfigurationElement._propKey];
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x000142E1 File Offset: 0x000132E1
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x000142F3 File Offset: 0x000132F3
		[ConfigurationProperty("value", DefaultValue = "")]
		public string Value
		{
			get
			{
				return (string)base[KeyValueConfigurationElement._propValue];
			}
			set
			{
				base[KeyValueConfigurationElement._propValue] = value;
			}
		}

		// Token: 0x04000328 RID: 808
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000329 RID: 809
		private static readonly ConfigurationProperty _propKey = new ConfigurationProperty("key", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x0400032A RID: 810
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400032B RID: 811
		private bool _needsInit;

		// Token: 0x0400032C RID: 812
		private string _initKey;

		// Token: 0x0400032D RID: 813
		private string _initValue;
	}
}
