using System;

namespace System.Xml.Schema
{
	// Token: 0x020001AE RID: 430
	internal class Datatype_anySimpleType : DatatypeImplementation
	{
		// Token: 0x06001617 RID: 5655 RVA: 0x000625E3 File Offset: 0x000615E3
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlUntypedConverter.Untyped;
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x000625EA File Offset: 0x000615EA
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.miscFacetsChecker;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001619 RID: 5657 RVA: 0x000625F1 File Offset: 0x000615F1
		public override Type ValueType
		{
			get
			{
				return Datatype_anySimpleType.atomicValueType;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x000625F8 File Offset: 0x000615F8
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyAtomicType;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600161B RID: 5659 RVA: 0x000625FC File Offset: 0x000615FC
		internal override Type ListValueType
		{
			get
			{
				return Datatype_anySimpleType.listValueType;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x00062603 File Offset: 0x00061603
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.None;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x00062607 File Offset: 0x00061607
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return (RestrictionFlags)0;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x0006260A File Offset: 0x0006160A
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0006260D File Offset: 0x0006160D
		internal override int Compare(object value1, object value2)
		{
			return string.Compare(value1.ToString(), value2.ToString(), StringComparison.Ordinal);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00062621 File Offset: 0x00061621
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = XmlComplianceUtil.NonCDataNormalize(s);
			return null;
		}

		// Token: 0x04000D72 RID: 3442
		private static readonly Type atomicValueType = typeof(string);

		// Token: 0x04000D73 RID: 3443
		private static readonly Type listValueType = typeof(string[]);
	}
}
