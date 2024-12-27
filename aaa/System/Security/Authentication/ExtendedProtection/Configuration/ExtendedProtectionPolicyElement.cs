using System;
using System.Collections.Generic;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200034F RID: 847
	public sealed class ExtendedProtectionPolicyElement : ConfigurationElement
	{
		// Token: 0x06001A86 RID: 6790 RVA: 0x0005CA30 File Offset: 0x0005BA30
		public ExtendedProtectionPolicyElement()
		{
			this.properties.Add(this.policyEnforcement);
			this.properties.Add(this.protectionScenario);
			this.properties.Add(this.customServiceNames);
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x0005CAE3 File Offset: 0x0005BAE3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x0005CAEB File Offset: 0x0005BAEB
		// (set) Token: 0x06001A89 RID: 6793 RVA: 0x0005CAFE File Offset: 0x0005BAFE
		[ConfigurationProperty("policyEnforcement")]
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return (PolicyEnforcement)base[this.policyEnforcement];
			}
			set
			{
				base[this.policyEnforcement] = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x0005CB12 File Offset: 0x0005BB12
		// (set) Token: 0x06001A8B RID: 6795 RVA: 0x0005CB25 File Offset: 0x0005BB25
		[ConfigurationProperty("protectionScenario", DefaultValue = ProtectionScenario.TransportSelected)]
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return (ProtectionScenario)base[this.protectionScenario];
			}
			set
			{
				base[this.protectionScenario] = value;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0005CB39 File Offset: 0x0005BB39
		[ConfigurationProperty("customServiceNames")]
		public ServiceNameElementCollection CustomServiceNames
		{
			get
			{
				return (ServiceNameElementCollection)base[this.customServiceNames];
			}
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0005CB4C File Offset: 0x0005BB4C
		public ExtendedProtectionPolicy BuildPolicy()
		{
			if (this.PolicyEnforcement == PolicyEnforcement.Never)
			{
				return new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			}
			ServiceNameCollection serviceNameCollection = null;
			ServiceNameElementCollection serviceNameElementCollection = this.CustomServiceNames;
			if (serviceNameElementCollection != null && serviceNameElementCollection.Count > 0)
			{
				List<string> list = new List<string>(serviceNameElementCollection.Count);
				foreach (object obj in serviceNameElementCollection)
				{
					ServiceNameElement serviceNameElement = (ServiceNameElement)obj;
					list.Add(serviceNameElement.Name);
				}
				serviceNameCollection = new ServiceNameCollection(list);
			}
			return new ExtendedProtectionPolicy(this.PolicyEnforcement, this.ProtectionScenario, serviceNameCollection);
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0005CBF8 File Offset: 0x0005BBF8
		private static PolicyEnforcement DefaultPolicyEnforcement
		{
			get
			{
				return PolicyEnforcement.Never;
			}
		}

		// Token: 0x04001B4E RID: 6990
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001B4F RID: 6991
		private readonly ConfigurationProperty policyEnforcement = new ConfigurationProperty("policyEnforcement", typeof(PolicyEnforcement), ExtendedProtectionPolicyElement.DefaultPolicyEnforcement, ConfigurationPropertyOptions.None);

		// Token: 0x04001B50 RID: 6992
		private readonly ConfigurationProperty protectionScenario = new ConfigurationProperty("protectionScenario", typeof(ProtectionScenario), ProtectionScenario.TransportSelected, ConfigurationPropertyOptions.None);

		// Token: 0x04001B51 RID: 6993
		private readonly ConfigurationProperty customServiceNames = new ConfigurationProperty("customServiceNames", typeof(ServiceNameElementCollection), null, ConfigurationPropertyOptions.None);
	}
}
