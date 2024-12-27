using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E0 RID: 480
	internal class Datatype_unsignedLong : Datatype_nonNegativeInteger
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00063F40 File Offset: 0x00062F40
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_unsignedLong.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00063F47 File Offset: 0x00062F47
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedLong;
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00063F4C File Offset: 0x00062F4C
		internal override int Compare(object value1, object value2)
		{
			return ((ulong)value1).CompareTo(value2);
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00063F68 File Offset: 0x00062F68
		public override Type ValueType
		{
			get
			{
				return Datatype_unsignedLong.atomicValueType;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00063F6F File Offset: 0x00062F6F
		internal override Type ListValueType
		{
			get
			{
				return Datatype_unsignedLong.listValueType;
			}
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00063F78 File Offset: 0x00062F78
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_unsignedLong.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				ulong num;
				ex = XmlConvert.TryToUInt64(s, out num);
				if (ex == null)
				{
					ex = Datatype_unsignedLong.numeric10FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000DA0 RID: 3488
		private static readonly Type atomicValueType = typeof(ulong);

		// Token: 0x04000DA1 RID: 3489
		private static readonly Type listValueType = typeof(ulong[]);

		// Token: 0x04000DA2 RID: 3490
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(0m, 18446744073709551615m);
	}
}
