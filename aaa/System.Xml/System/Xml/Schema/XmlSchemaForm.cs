using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200025C RID: 604
	public enum XmlSchemaForm
	{
		// Token: 0x0400117F RID: 4479
		[XmlIgnore]
		None,
		// Token: 0x04001180 RID: 4480
		[XmlEnum("qualified")]
		Qualified,
		// Token: 0x04001181 RID: 4481
		[XmlEnum("unqualified")]
		Unqualified
	}
}
