using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C9 RID: 457
	internal class Datatype_base64Binary : Datatype_anySimpleType
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x00063648 File Offset: 0x00062648
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00063650 File Offset: 0x00062650
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.binaryFacetsChecker;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00063657 File Offset: 0x00062657
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Base64Binary;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x0006365B File Offset: 0x0006265B
		public override Type ValueType
		{
			get
			{
				return Datatype_base64Binary.atomicValueType;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00063662 File Offset: 0x00062662
		internal override Type ListValueType
		{
			get
			{
				return Datatype_base64Binary.listValueType;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060016BA RID: 5818 RVA: 0x00063669 File Offset: 0x00062669
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x0006366C File Offset: 0x0006266C
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00063670 File Offset: 0x00062670
		internal override int Compare(object value1, object value2)
		{
			return base.Compare((byte[])value1, (byte[])value2);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00063684 File Offset: 0x00062684
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.binaryFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				byte[] array = null;
				try
				{
					array = Convert.FromBase64String(s);
				}
				catch (ArgumentException ex2)
				{
					return ex2;
				}
				catch (FormatException ex3)
				{
					return ex3;
				}
				ex = DatatypeImplementation.binaryFacetsChecker.CheckValueFacets(array, this);
				if (ex == null)
				{
					typedValue = array;
					return null;
				}
			}
			return ex;
		}

		// Token: 0x04000D89 RID: 3465
		private static readonly Type atomicValueType = typeof(byte[]);

		// Token: 0x04000D8A RID: 3466
		private static readonly Type listValueType = typeof(byte[][]);
	}
}
