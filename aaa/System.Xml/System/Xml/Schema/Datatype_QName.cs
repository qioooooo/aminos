using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CB RID: 459
	internal class Datatype_QName : Datatype_anySimpleType
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x000637D6 File Offset: 0x000627D6
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x000637DE File Offset: 0x000627DE
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.qnameFacetsChecker;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x000637E5 File Offset: 0x000627E5
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.QName;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x000637E9 File Offset: 0x000627E9
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.QName;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x000637ED File Offset: 0x000627ED
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x000637F1 File Offset: 0x000627F1
		public override Type ValueType
		{
			get
			{
				return Datatype_QName.atomicValueType;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000637F8 File Offset: 0x000627F8
		internal override Type ListValueType
		{
			get
			{
				return Datatype_QName.listValueType;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x000637FF File Offset: 0x000627FF
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00063804 File Offset: 0x00062804
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			if (s == null || s.Length == 0)
			{
				return new XmlSchemaException("Sch_EmptyAttributeValue", string.Empty);
			}
			Exception ex = DatatypeImplementation.qnameFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				XmlQualifiedName xmlQualifiedName = null;
				try
				{
					string text;
					xmlQualifiedName = XmlQualifiedName.Parse(s, nsmgr, out text);
				}
				catch (ArgumentException ex2)
				{
					return ex2;
				}
				catch (XmlException ex3)
				{
					return ex3;
				}
				ex = DatatypeImplementation.qnameFacetsChecker.CheckValueFacets(xmlQualifiedName, this);
				if (ex == null)
				{
					typedValue = xmlQualifiedName;
					return null;
				}
			}
			return ex;
		}

		// Token: 0x04000D8D RID: 3469
		private static readonly Type atomicValueType = typeof(XmlQualifiedName);

		// Token: 0x04000D8E RID: 3470
		private static readonly Type listValueType = typeof(XmlQualifiedName[]);
	}
}
