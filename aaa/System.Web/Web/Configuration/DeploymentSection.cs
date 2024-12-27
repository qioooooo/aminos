using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001D4 RID: 468
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DeploymentSection : ConfigurationSection
	{
		// Token: 0x06001A45 RID: 6725 RVA: 0x0007B3AB File Offset: 0x0007A3AB
		static DeploymentSection()
		{
			DeploymentSection._properties.Add(DeploymentSection._propRetail);
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0007B3EE File Offset: 0x0007A3EE
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return DeploymentSection._properties;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001A48 RID: 6728 RVA: 0x0007B3F5 File Offset: 0x0007A3F5
		// (set) Token: 0x06001A49 RID: 6729 RVA: 0x0007B407 File Offset: 0x0007A407
		[ConfigurationProperty("retail", DefaultValue = false)]
		public bool Retail
		{
			get
			{
				return (bool)base[DeploymentSection._propRetail];
			}
			set
			{
				base[DeploymentSection._propRetail] = value;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x0007B41A File Offset: 0x0007A41A
		internal static bool RetailInternal
		{
			get
			{
				if (!DeploymentSection.s_hasCachedData)
				{
					DeploymentSection.s_retail = RuntimeConfig.GetAppConfig().Deployment.Retail;
					DeploymentSection.s_hasCachedData = true;
				}
				return DeploymentSection.s_retail;
			}
		}

		// Token: 0x040017CF RID: 6095
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017D0 RID: 6096
		private static readonly ConfigurationProperty _propRetail = new ConfigurationProperty("retail", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040017D1 RID: 6097
		private static bool s_hasCachedData;

		// Token: 0x040017D2 RID: 6098
		private static bool s_retail;
	}
}
