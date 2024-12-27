using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000245 RID: 581
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SecurityPolicySection : ConfigurationSection
	{
		// Token: 0x06001ED3 RID: 7891 RVA: 0x00089BBC File Offset: 0x00088BBC
		static SecurityPolicySection()
		{
			SecurityPolicySection._properties.Add(SecurityPolicySection._propTrustLevels);
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x00089BF6 File Offset: 0x00088BF6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SecurityPolicySection._properties;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x00089BFD File Offset: 0x00088BFD
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public TrustLevelCollection TrustLevels
		{
			get
			{
				return (TrustLevelCollection)base[SecurityPolicySection._propTrustLevels];
			}
		}

		// Token: 0x04001A16 RID: 6678
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A17 RID: 6679
		private static readonly ConfigurationProperty _propTrustLevels = new ConfigurationProperty(null, typeof(TrustLevelCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
