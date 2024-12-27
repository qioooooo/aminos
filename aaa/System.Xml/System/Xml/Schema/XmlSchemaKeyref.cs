using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000263 RID: 611
	public class XmlSchemaKeyref : XmlSchemaIdentityConstraint
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x00083281 File Offset: 0x00082281
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x00083289 File Offset: 0x00082289
		[XmlAttribute("refer")]
		public XmlQualifiedName Refer
		{
			get
			{
				return this.refer;
			}
			set
			{
				this.refer = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x04001191 RID: 4497
		private XmlQualifiedName refer = XmlQualifiedName.Empty;
	}
}
