using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D1 RID: 465
	internal class Datatype_NMTOKEN : Datatype_token
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x00063900 File Offset: 0x00062900
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NmToken;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x00063904 File Offset: 0x00062904
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.NMTOKEN;
			}
		}
	}
}
