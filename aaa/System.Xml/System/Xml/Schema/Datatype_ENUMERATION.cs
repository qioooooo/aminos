using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E8 RID: 488
	internal class Datatype_ENUMERATION : Datatype_NMTOKEN
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00064426 File Offset: 0x00063426
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ENUMERATION;
			}
		}
	}
}
