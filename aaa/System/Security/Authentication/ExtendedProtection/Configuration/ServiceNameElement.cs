using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x02000351 RID: 849
	public sealed class ServiceNameElement : ConfigurationElement
	{
		// Token: 0x06001A9C RID: 6812 RVA: 0x0005CCBC File Offset: 0x0005BCBC
		public ServiceNameElement()
		{
			this.properties.Add(this.name);
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x0005CCFC File Offset: 0x0005BCFC
		// (set) Token: 0x06001A9E RID: 6814 RVA: 0x0005CD0F File Offset: 0x0005BD0F
		[ConfigurationProperty("name")]
		public string Name
		{
			get
			{
				return (string)base[this.name];
			}
			set
			{
				base[this.name] = value;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001A9F RID: 6815 RVA: 0x0005CD1E File Offset: 0x0005BD1E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0005CD26 File Offset: 0x0005BD26
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x04001B52 RID: 6994
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001B53 RID: 6995
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
	}
}
