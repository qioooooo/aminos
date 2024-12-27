using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CC RID: 460
	internal class Datatype_normalizedString : Datatype_string
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x000638B8 File Offset: 0x000628B8
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NormalizedString;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x000638BC File Offset: 0x000628BC
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Replace;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x000638BF File Offset: 0x000628BF
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}
	}
}
