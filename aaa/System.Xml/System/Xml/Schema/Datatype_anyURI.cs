using System;

namespace System.Xml.Schema
{
	// Token: 0x020001CA RID: 458
	internal class Datatype_anyURI : Datatype_anySimpleType
	{
		// Token: 0x060016C0 RID: 5824 RVA: 0x00063718 File Offset: 0x00062718
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x00063720 File Offset: 0x00062720
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.stringFacetsChecker;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00063727 File Offset: 0x00062727
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyUri;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x0006372B File Offset: 0x0006272B
		public override Type ValueType
		{
			get
			{
				return Datatype_anyURI.atomicValueType;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00063732 File Offset: 0x00062732
		internal override bool HasValueFacets
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00063735 File Offset: 0x00062735
		internal override Type ListValueType
		{
			get
			{
				return Datatype_anyURI.listValueType;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x0006373C File Offset: 0x0006273C
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0006373F File Offset: 0x0006273F
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00063743 File Offset: 0x00062743
		internal override int Compare(object value1, object value2)
		{
			if (!((Uri)value1).Equals((Uri)value2))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x0006375C File Offset: 0x0006275C
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.stringFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				Uri uri;
				ex = XmlConvert.TryToUri(s, out uri);
				if (ex == null)
				{
					string originalString = uri.OriginalString;
					ex = ((StringFacetsChecker)DatatypeImplementation.stringFacetsChecker).CheckValueFacets(originalString, this, false);
					if (ex == null)
					{
						typedValue = uri;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D8B RID: 3467
		private static readonly Type atomicValueType = typeof(Uri);

		// Token: 0x04000D8C RID: 3468
		private static readonly Type listValueType = typeof(Uri[]);
	}
}
