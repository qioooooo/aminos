using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027E RID: 638
	internal class XmlSchemaSubstitutionGroupV1Compat : XmlSchemaSubstitutionGroup
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x00085F71 File Offset: 0x00084F71
		[XmlIgnore]
		internal XmlSchemaChoice Choice
		{
			get
			{
				return this.choice;
			}
		}

		// Token: 0x040011E4 RID: 4580
		private XmlSchemaChoice choice = new XmlSchemaChoice();
	}
}
