using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200023F RID: 575
	public abstract class XmlSchemaContentModel : XmlSchemaAnnotated
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001B72 RID: 7026
		// (set) Token: 0x06001B73 RID: 7027
		[XmlIgnore]
		public abstract XmlSchemaContent Content { get; set; }
	}
}
