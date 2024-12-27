using System;

namespace System.Xml.Schema
{
	// Token: 0x020001DE RID: 478
	internal class Datatype_byte : Datatype_short
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x00063E4C File Offset: 0x00062E4C
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_byte.numeric10FacetsChecker;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00063E53 File Offset: 0x00062E53
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Byte;
			}
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00063E58 File Offset: 0x00062E58
		internal override int Compare(object value1, object value2)
		{
			return ((sbyte)value1).CompareTo(value2);
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x00063E74 File Offset: 0x00062E74
		public override Type ValueType
		{
			get
			{
				return Datatype_byte.atomicValueType;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600172B RID: 5931 RVA: 0x00063E7B File Offset: 0x00062E7B
		internal override Type ListValueType
		{
			get
			{
				return Datatype_byte.listValueType;
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00063E84 File Offset: 0x00062E84
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_byte.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				sbyte b;
				ex = XmlConvert.TryToSByte(s, out b);
				if (ex == null)
				{
					ex = Datatype_byte.numeric10FacetsChecker.CheckValueFacets((short)b, this);
					if (ex == null)
					{
						typedValue = b;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D9C RID: 3484
		private static readonly Type atomicValueType = typeof(sbyte);

		// Token: 0x04000D9D RID: 3485
		private static readonly Type listValueType = typeof(sbyte[]);

		// Token: 0x04000D9E RID: 3486
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(-128m, 127m);
	}
}
