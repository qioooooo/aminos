using System;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200011C RID: 284
	public enum SoapBindingStyle
	{
		// Token: 0x040005BA RID: 1466
		[XmlIgnore]
		Default,
		// Token: 0x040005BB RID: 1467
		[XmlEnum("document")]
		Document,
		// Token: 0x040005BC RID: 1468
		[XmlEnum("rpc")]
		Rpc
	}
}
