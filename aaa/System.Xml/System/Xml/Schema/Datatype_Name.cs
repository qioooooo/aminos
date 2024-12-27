using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D2 RID: 466
	internal class Datatype_Name : Datatype_token
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x0006390F File Offset: 0x0006290F
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Name;
			}
		}
	}
}
