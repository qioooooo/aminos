using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CF RID: 463
	internal class Datatype_tokenV1Compat : Datatype_normalizedStringV1Compat
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x000638E8 File Offset: 0x000628E8
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Token;
			}
		}
	}
}
