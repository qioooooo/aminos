using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B6 RID: 438
	internal class Datatype_double : Datatype_anySimpleType
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00063012 File Offset: 0x00062012
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlNumeric2Converter.Create(schemaType);
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x0006301A File Offset: 0x0006201A
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.numeric2FacetsChecker;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x00063021 File Offset: 0x00062021
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Double;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x00063025 File Offset: 0x00062025
		public override Type ValueType
		{
			get
			{
				return Datatype_double.atomicValueType;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x0006302C File Offset: 0x0006202C
		internal override Type ListValueType
		{
			get
			{
				return Datatype_double.listValueType;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x00063033 File Offset: 0x00062033
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00063036 File Offset: 0x00062036
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive;
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00063040 File Offset: 0x00062040
		internal override int Compare(object value1, object value2)
		{
			return ((double)value1).CompareTo(value2);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0006305C File Offset: 0x0006205C
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.numeric2FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				double num;
				ex = XmlConvert.TryToDouble(s, out num);
				if (ex == null)
				{
					ex = DatatypeImplementation.numeric2FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D7D RID: 3453
		private static readonly Type atomicValueType = typeof(double);

		// Token: 0x04000D7E RID: 3454
		private static readonly Type listValueType = typeof(double[]);
	}
}
