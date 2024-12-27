using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D4 RID: 468
	internal class Datatype_ID : Datatype_NCName
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00063969 File Offset: 0x00062969
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Id;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x0006396D File Offset: 0x0006296D
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ID;
			}
		}
	}
}
