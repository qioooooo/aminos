using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001BE RID: 446
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ClientTargetSection : ConfigurationSection
	{
		// Token: 0x06001995 RID: 6549 RVA: 0x0007929D File Offset: 0x0007829D
		static ClientTargetSection()
		{
			ClientTargetSection._properties.Add(ClientTargetSection._propClientTargets);
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x000792CF File Offset: 0x000782CF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientTargetSection._properties;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x000792D6 File Offset: 0x000782D6
		[ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
		public ClientTargetCollection ClientTargets
		{
			get
			{
				return (ClientTargetCollection)base[ClientTargetSection._propClientTargets];
			}
		}

		// Token: 0x0400174D RID: 5965
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400174E RID: 5966
		private static readonly ConfigurationProperty _propClientTargets = new ConfigurationProperty(null, typeof(ClientTargetCollection), null, ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired);
	}
}
