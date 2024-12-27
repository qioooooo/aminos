using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000232 RID: 562
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProtocolsSection : ConfigurationSection
	{
		// Token: 0x06001E1F RID: 7711 RVA: 0x00087390 File Offset: 0x00086390
		static ProtocolsSection()
		{
			ProtocolsSection._properties.Add(ProtocolsSection._propProtocols);
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x000873CA File Offset: 0x000863CA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProtocolsSection._properties;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x000873D1 File Offset: 0x000863D1
		[ConfigurationProperty("protocols", IsRequired = true, IsDefaultCollection = true)]
		public ProtocolCollection Protocols
		{
			get
			{
				return (ProtocolCollection)base[ProtocolsSection._propProtocols];
			}
		}

		// Token: 0x040019AC RID: 6572
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040019AD RID: 6573
		private static readonly ConfigurationProperty _propProtocols = new ConfigurationProperty(null, typeof(ProtocolCollection), null, ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired);
	}
}
