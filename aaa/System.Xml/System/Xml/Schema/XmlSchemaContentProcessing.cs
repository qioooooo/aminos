using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000246 RID: 582
	public enum XmlSchemaContentProcessing
	{
		// Token: 0x04001135 RID: 4405
		[XmlIgnore]
		None,
		// Token: 0x04001136 RID: 4406
		[XmlEnum("skip")]
		Skip,
		// Token: 0x04001137 RID: 4407
		[XmlEnum("lax")]
		Lax,
		// Token: 0x04001138 RID: 4408
		[XmlEnum("strict")]
		Strict
	}
}
