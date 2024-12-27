using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E4 RID: 484
	internal class Datatype_positiveInteger : Datatype_nonNegativeInteger
	{
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00064254 File Offset: 0x00063254
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_positiveInteger.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x0006425B File Offset: 0x0006325B
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.PositiveInteger;
			}
		}

		// Token: 0x04000DAC RID: 3500
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(1m, decimal.MaxValue);
	}
}
