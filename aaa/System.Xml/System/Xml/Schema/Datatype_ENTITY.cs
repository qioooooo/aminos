using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D6 RID: 470
	internal class Datatype_ENTITY : Datatype_NCName
	{
		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x00063987 File Offset: 0x00062987
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Entity;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0006398B File Offset: 0x0006298B
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ENTITY;
			}
		}
	}
}
