using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B5 RID: 437
	internal class Datatype_float : Datatype_anySimpleType
	{
		// Token: 0x06001659 RID: 5721 RVA: 0x00062F56 File Offset: 0x00061F56
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlNumeric2Converter.Create(schemaType);
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x00062F5E File Offset: 0x00061F5E
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.numeric2FacetsChecker;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x00062F65 File Offset: 0x00061F65
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Float;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x00062F69 File Offset: 0x00061F69
		public override Type ValueType
		{
			get
			{
				return Datatype_float.atomicValueType;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x00062F70 File Offset: 0x00061F70
		internal override Type ListValueType
		{
			get
			{
				return Datatype_float.listValueType;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600165E RID: 5726 RVA: 0x00062F77 File Offset: 0x00061F77
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x00062F7A File Offset: 0x00061F7A
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive;
			}
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00062F84 File Offset: 0x00061F84
		internal override int Compare(object value1, object value2)
		{
			return ((float)value1).CompareTo(value2);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00062FA0 File Offset: 0x00061FA0
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.numeric2FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				float num;
				ex = XmlConvert.TryToSingle(s, out num);
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

		// Token: 0x04000D7B RID: 3451
		private static readonly Type atomicValueType = typeof(float);

		// Token: 0x04000D7C RID: 3452
		private static readonly Type listValueType = typeof(float[]);
	}
}
