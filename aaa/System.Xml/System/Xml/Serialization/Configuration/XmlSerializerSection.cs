using System;
using System.Configuration;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x02000355 RID: 853
	public sealed class XmlSerializerSection : ConfigurationSection
	{
		// Token: 0x06002940 RID: 10560 RVA: 0x000D3944 File Offset: 0x000D2944
		public XmlSerializerSection()
		{
			this.properties.Add(this.checkDeserializeAdvances);
			this.properties.Add(this.tempFilesLocation);
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06002941 RID: 10561 RVA: 0x000D39C7 File Offset: 0x000D29C7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06002942 RID: 10562 RVA: 0x000D39CF File Offset: 0x000D29CF
		// (set) Token: 0x06002943 RID: 10563 RVA: 0x000D39E2 File Offset: 0x000D29E2
		[ConfigurationProperty("checkDeserializeAdvances", DefaultValue = false)]
		public bool CheckDeserializeAdvances
		{
			get
			{
				return (bool)base[this.checkDeserializeAdvances];
			}
			set
			{
				base[this.checkDeserializeAdvances] = value;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x000D39F6 File Offset: 0x000D29F6
		// (set) Token: 0x06002945 RID: 10565 RVA: 0x000D3A09 File Offset: 0x000D2A09
		[ConfigurationProperty("tempFilesLocation", DefaultValue = null)]
		public string TempFilesLocation
		{
			get
			{
				return (string)base[this.tempFilesLocation];
			}
			set
			{
				base[this.tempFilesLocation] = value;
			}
		}

		// Token: 0x040016E6 RID: 5862
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040016E7 RID: 5863
		private readonly ConfigurationProperty checkDeserializeAdvances = new ConfigurationProperty("checkDeserializeAdvances", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040016E8 RID: 5864
		private readonly ConfigurationProperty tempFilesLocation = new ConfigurationProperty("tempFilesLocation", typeof(string), null, null, new RootedPathValidator(), ConfigurationPropertyOptions.None);
	}
}
