using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B7 RID: 439
	internal class Datatype_decimal : Datatype_anySimpleType
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x000630CE File Offset: 0x000620CE
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlNumeric10Converter.Create(schemaType);
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x000630D6 File Offset: 0x000620D6
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return Datatype_decimal.numeric10FacetsChecker;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x000630DD File Offset: 0x000620DD
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Decimal;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x000630E1 File Offset: 0x000620E1
		public override Type ValueType
		{
			get
			{
				return Datatype_decimal.atomicValueType;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000630E8 File Offset: 0x000620E8
		internal override Type ListValueType
		{
			get
			{
				return Datatype_decimal.listValueType;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x000630EF File Offset: 0x000620EF
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000630F2 File Offset: 0x000620F2
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive | RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits;
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000630FC File Offset: 0x000620FC
		internal override int Compare(object value1, object value2)
		{
			return ((decimal)value1).CompareTo(value2);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00063118 File Offset: 0x00062118
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = Datatype_decimal.numeric10FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				decimal num;
				ex = XmlConvert.TryToDecimal(s, out num);
				if (ex == null)
				{
					ex = Datatype_decimal.numeric10FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D7F RID: 3455
		private static readonly Type atomicValueType = typeof(decimal);

		// Token: 0x04000D80 RID: 3456
		private static readonly Type listValueType = typeof(decimal[]);

		// Token: 0x04000D81 RID: 3457
		private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(decimal.MinValue, decimal.MaxValue);
	}
}
