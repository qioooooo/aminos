using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CE RID: 462
	internal class Datatype_token : Datatype_normalizedString
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x000638D9 File Offset: 0x000628D9
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Token;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x000638DD File Offset: 0x000628DD
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}
	}
}
