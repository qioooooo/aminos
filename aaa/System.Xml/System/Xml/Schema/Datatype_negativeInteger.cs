using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DA RID: 474
	internal class Datatype_negativeInteger : Datatype_nonPositiveInteger
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x00063BAA File Offset: 0x00062BAA
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_negativeInteger.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00063BB1 File Offset: 0x00062BB1
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NegativeInteger;
			}
		}

		// Token: 0x04000D92 RID: 3474
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(decimal.MinValue, -1m);
	}
}
