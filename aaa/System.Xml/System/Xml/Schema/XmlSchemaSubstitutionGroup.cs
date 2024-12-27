using System;
using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027D RID: 637
	internal class XmlSchemaSubstitutionGroup : XmlSchemaObject
	{
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00085F3A File Offset: 0x00084F3A
		[XmlIgnore]
		internal ArrayList Members
		{
			get
			{
				return this.membersList;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001D5E RID: 7518 RVA: 0x00085F42 File Offset: 0x00084F42
		// (set) Token: 0x06001D5F RID: 7519 RVA: 0x00085F4A File Offset: 0x00084F4A
		[XmlIgnore]
		internal XmlQualifiedName Examplar
		{
			get
			{
				return this.examplar;
			}
			set
			{
				this.examplar = value;
			}
		}

		// Token: 0x040011E2 RID: 4578
		private ArrayList membersList = new ArrayList();

		// Token: 0x040011E3 RID: 4579
		private XmlQualifiedName examplar = XmlQualifiedName.Empty;
	}
}
