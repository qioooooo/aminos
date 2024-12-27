using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D5 RID: 469
	internal class Datatype_IDREF : Datatype_NCName
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00063978 File Offset: 0x00062978
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Idref;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0006397C File Offset: 0x0006297C
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.IDREF;
			}
		}
	}
}
