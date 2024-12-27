using System;
using System.Configuration;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x02000664 RID: 1636
	public sealed class SmtpNetworkElement : ConfigurationElement
	{
		// Token: 0x0600329C RID: 12956 RVA: 0x000D6E30 File Offset: 0x000D5E30
		public SmtpNetworkElement()
		{
			this.properties.Add(this.defaultCredentials);
			this.properties.Add(this.host);
			this.properties.Add(this.clientDomain);
			this.properties.Add(this.password);
			this.properties.Add(this.port);
			this.properties.Add(this.userName);
			this.properties.Add(this.targetName);
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x000D6FA0 File Offset: 0x000D5FA0
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation propertyInformation = base.ElementInformation.Properties["port"];
			if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere && (int)propertyInformation.Value != (int)propertyInformation.DefaultValue)
			{
				try
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
				}
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000D7034 File Offset: 0x000D6034
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x000D703C File Offset: 0x000D603C
		// (set) Token: 0x060032A0 RID: 12960 RVA: 0x000D704F File Offset: 0x000D604F
		[ConfigurationProperty("defaultCredentials", DefaultValue = false)]
		public bool DefaultCredentials
		{
			get
			{
				return (bool)base[this.defaultCredentials];
			}
			set
			{
				base[this.defaultCredentials] = value;
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000D7063 File Offset: 0x000D6063
		// (set) Token: 0x060032A2 RID: 12962 RVA: 0x000D7076 File Offset: 0x000D6076
		[ConfigurationProperty("host")]
		public string Host
		{
			get
			{
				return (string)base[this.host];
			}
			set
			{
				base[this.host] = value;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000D7085 File Offset: 0x000D6085
		// (set) Token: 0x060032A4 RID: 12964 RVA: 0x000D7098 File Offset: 0x000D6098
		[ConfigurationProperty("clientDomain")]
		public string ClientDomain
		{
			get
			{
				return (string)base[this.clientDomain];
			}
			set
			{
				base[this.clientDomain] = value;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000D70A7 File Offset: 0x000D60A7
		// (set) Token: 0x060032A6 RID: 12966 RVA: 0x000D70BA File Offset: 0x000D60BA
		[ConfigurationProperty("targetName")]
		public string TargetName
		{
			get
			{
				return (string)base[this.targetName];
			}
			set
			{
				base[this.targetName] = value;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000D70C9 File Offset: 0x000D60C9
		// (set) Token: 0x060032A8 RID: 12968 RVA: 0x000D70DC File Offset: 0x000D60DC
		[ConfigurationProperty("password")]
		public string Password
		{
			get
			{
				return (string)base[this.password];
			}
			set
			{
				base[this.password] = value;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000D70EB File Offset: 0x000D60EB
		// (set) Token: 0x060032AA RID: 12970 RVA: 0x000D70FE File Offset: 0x000D60FE
		[ConfigurationProperty("port", DefaultValue = 25)]
		public int Port
		{
			get
			{
				return (int)base[this.port];
			}
			set
			{
				base[this.port] = value;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000D7112 File Offset: 0x000D6112
		// (set) Token: 0x060032AC RID: 12972 RVA: 0x000D7125 File Offset: 0x000D6125
		[ConfigurationProperty("userName")]
		public string UserName
		{
			get
			{
				return (string)base[this.userName];
			}
			set
			{
				base[this.userName] = value;
			}
		}

		// Token: 0x04002F54 RID: 12116
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F55 RID: 12117
		private readonly ConfigurationProperty defaultCredentials = new ConfigurationProperty("defaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F56 RID: 12118
		private readonly ConfigurationProperty host = new ConfigurationProperty("host", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F57 RID: 12119
		private readonly ConfigurationProperty clientDomain = new ConfigurationProperty("clientDomain", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F58 RID: 12120
		private readonly ConfigurationProperty password = new ConfigurationProperty("password", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F59 RID: 12121
		private readonly ConfigurationProperty port = new ConfigurationProperty("port", typeof(int), 25, null, new IntegerValidator(1, 65535), ConfigurationPropertyOptions.None);

		// Token: 0x04002F5A RID: 12122
		private readonly ConfigurationProperty userName = new ConfigurationProperty("userName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F5B RID: 12123
		private readonly ConfigurationProperty targetName = new ConfigurationProperty("targetName", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
