using System;

namespace System.Xml.Schema
{
	// Token: 0x02000282 RID: 642
	[Flags]
	public enum XmlSchemaValidationFlags
	{
		// Token: 0x040011EC RID: 4588
		None = 0,
		// Token: 0x040011ED RID: 4589
		ProcessInlineSchema = 1,
		// Token: 0x040011EE RID: 4590
		ProcessSchemaLocation = 2,
		// Token: 0x040011EF RID: 4591
		ReportValidationWarnings = 4,
		// Token: 0x040011F0 RID: 4592
		ProcessIdentityConstraints = 8,
		// Token: 0x040011F1 RID: 4593
		AllowXmlAttributes = 16
	}
}
