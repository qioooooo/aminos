using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DF RID: 479
	internal class Datatype_nonNegativeInteger : Datatype_integer
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00063F0E File Offset: 0x00062F0E
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_nonNegativeInteger.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00063F15 File Offset: 0x00062F15
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NonNegativeInteger;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00063F19 File Offset: 0x00062F19
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000D9F RID: 3487
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, decimal.MaxValue);
	}
}
