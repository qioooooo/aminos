using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C8 RID: 456
	internal class Datatype_hexBinary : Datatype_anySimpleType
	{
		// Token: 0x060016AA RID: 5802 RVA: 0x00063577 File Offset: 0x00062577
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060016AB RID: 5803 RVA: 0x0006357F File Offset: 0x0006257F
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.binaryFacetsChecker;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00063586 File Offset: 0x00062586
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.HexBinary;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x0006358A File Offset: 0x0006258A
		public override Type ValueType
		{
			get
			{
				return Datatype_hexBinary.atomicValueType;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x00063591 File Offset: 0x00062591
		internal override Type ListValueType
		{
			get
			{
				return Datatype_hexBinary.listValueType;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x00063598 File Offset: 0x00062598
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x0006359B File Offset: 0x0006259B
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0006359F File Offset: 0x0006259F
		internal override int Compare(object value1, object value2)
		{
			return base.Compare((byte[])value1, (byte[])value2);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x000635B4 File Offset: 0x000625B4
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.binaryFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				byte[] array = null;
				try
				{
					array = XmlConvert.FromBinHexString(s, false);
				}
				catch (ArgumentException ex2)
				{
					return ex2;
				}
				catch (XmlException ex3)
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

		// Token: 0x04000D87 RID: 3463
		private static readonly Type atomicValueType = typeof(byte[]);

		// Token: 0x04000D88 RID: 3464
		private static readonly Type listValueType = typeof(byte[][]);
	}
}
