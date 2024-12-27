using System;

namespace System.Configuration
{
	// Token: 0x0200007E RID: 126
	public sealed class NameValueConfigurationElement : ConfigurationElement
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x00018A08 File Offset: 0x00017A08
		static NameValueConfigurationElement()
		{
			NameValueConfigurationElement._properties.Add(NameValueConfigurationElement._propName);
			NameValueConfigurationElement._properties.Add(NameValueConfigurationElement._propValue);
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00018A7B File Offset: 0x00017A7B
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return NameValueConfigurationElement._properties;
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00018A82 File Offset: 0x00017A82
		internal NameValueConfigurationElement()
		{
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00018A8A File Offset: 0x00017A8A
		public NameValueConfigurationElement(string name, string value)
		{
			base[NameValueConfigurationElement._propName] = name;
			base[NameValueConfigurationElement._propValue] = value;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00018AAA File Offset: 0x00017AAA
		[ConfigurationProperty("name", IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[NameValueConfigurationElement._propName];
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00018ABC File Offset: 0x00017ABC
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00018ACE File Offset: 0x00017ACE
		[ConfigurationProperty("value", DefaultValue = "")]
		public string Value
		{
			get
			{
				return (string)base[NameValueConfigurationElement._propValue];
			}
			set
			{
				base[NameValueConfigurationElement._propValue] = value;
			}
		}

		// Token: 0x0400034D RID: 845
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400034E RID: 846
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsKey);

		// Token: 0x0400034F RID: 847
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}
