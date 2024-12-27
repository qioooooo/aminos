using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027F RID: 639
	public enum XmlSchemaUse
	{
		// Token: 0x040011E6 RID: 4582
		[XmlIgnore]
		None,
		// Token: 0x040011E7 RID: 4583
		[XmlEnum("optional")]
		Optional,
		// Token: 0x040011E8 RID: 4584
		[XmlEnum("prohibited")]
		Prohibited,
		// Token: 0x040011E9 RID: 4585
		[XmlEnum("required")]
		Required
	}
}
