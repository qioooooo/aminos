using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D9 RID: 473
	internal class Datatype_nonPositiveInteger : Datatype_integer
	{
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00063B74 File Offset: 0x00062B74
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_nonPositiveInteger.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x00063B7B File Offset: 0x00062B7B
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NonPositiveInteger;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00063B7F File Offset: 0x00062B7F
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000D91 RID: 3473
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(decimal.MinValue, 0m);
	}
}
