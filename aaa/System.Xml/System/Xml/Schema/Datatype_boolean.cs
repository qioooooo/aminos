using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B4 RID: 436
	internal class Datatype_boolean : Datatype_anySimpleType
	{
		// Token: 0x0600164E RID: 5710 RVA: 0x00062EAD File Offset: 0x00061EAD
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlBooleanConverter.Create(schemaType);
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x00062EB5 File Offset: 0x00061EB5
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.miscFacetsChecker;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x00062EBC File Offset: 0x00061EBC
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Boolean;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x00062EC0 File Offset: 0x00061EC0
		public override Type ValueType
		{
			get
			{
				return Datatype_boolean.atomicValueType;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x00062EC7 File Offset: 0x00061EC7
		internal override Type ListValueType
		{
			get
			{
				return Datatype_boolean.listValueType;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x00062ECE File Offset: 0x00061ECE
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x00062ED1 File Offset: 0x00061ED1
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00062ED8 File Offset: 0x00061ED8
		internal override int Compare(object value1, object value2)
		{
			return ((bool)value1).CompareTo(value2);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00062EF4 File Offset: 0x00061EF4
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.miscFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				bool flag;
				ex = XmlConvert.TryToBoolean(s, out flag);
				if (ex == null)
				{
					typedValue = flag;
					return null;
				}
			}
			return ex;
		}

		// Token: 0x04000D79 RID: 3449
		private static readonly Type atomicValueType = typeof(bool);

		// Token: 0x04000D7A RID: 3450
		private static readonly Type listValueType = typeof(bool[]);
	}
}
