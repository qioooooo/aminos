using System;
using System.Configuration;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x02000231 RID: 561
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProtocolsConfigurationHandler : IConfigurationSectionHandler
	{
		// Token: 0x06001E1E RID: 7710 RVA: 0x00087388 File Offset: 0x00086388
		public object Create(object parent, object configContextObj, XmlNode section)
		{
			return new ProtocolsConfiguration(section);
		}
	}
}
