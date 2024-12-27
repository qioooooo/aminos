using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020001E7 RID: 487
	public interface IConfigurationSectionHandler
	{
		// Token: 0x0600100B RID: 4107
		object Create(object parent, object configContext, XmlNode section);
	}
}
