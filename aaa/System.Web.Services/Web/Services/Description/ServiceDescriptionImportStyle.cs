using System;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000104 RID: 260
	public enum ServiceDescriptionImportStyle
	{
		// Token: 0x04000495 RID: 1173
		[XmlEnum("client")]
		Client,
		// Token: 0x04000496 RID: 1174
		[XmlEnum("server")]
		Server,
		// Token: 0x04000497 RID: 1175
		[XmlEnum("serverInterface")]
		ServerInterface
	}
}
