using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200023A RID: 570
	public class XmlSchemaAttributeGroupRef : XmlSchemaAnnotated
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x00081349 File Offset: 0x00080349
		// (set) Token: 0x06001B3F RID: 6975 RVA: 0x00081351 File Offset: 0x00080351
		[XmlAttribute("ref")]
		public XmlQualifiedName RefName
		{
			get
			{
				return this.refName;
			}
			set
			{
				this.refName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x040010FF RID: 4351
		private XmlQualifiedName refName = XmlQualifiedName.Empty;
	}
}
