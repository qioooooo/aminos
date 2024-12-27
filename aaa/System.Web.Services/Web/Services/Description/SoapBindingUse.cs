using System;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200011D RID: 285
	public enum SoapBindingUse
	{
		// Token: 0x040005BE RID: 1470
		[XmlIgnore]
		Default,
		// Token: 0x040005BF RID: 1471
		[XmlEnum("encoded")]
		Encoded,
		// Token: 0x040005C0 RID: 1472
		[XmlEnum("literal")]
		Literal
	}
}
