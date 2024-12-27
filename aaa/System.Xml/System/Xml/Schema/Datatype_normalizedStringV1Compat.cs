using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CD RID: 461
	internal class Datatype_normalizedStringV1Compat : Datatype_string
	{
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x000638CA File Offset: 0x000628CA
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NormalizedString;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x000638CE File Offset: 0x000628CE
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}
	}
}
