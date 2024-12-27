using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B8 RID: 440
	internal class Datatype_duration : Datatype_anySimpleType
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x000631B9 File Offset: 0x000621B9
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x000631C1 File Offset: 0x000621C1
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.durationFacetsChecker;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x000631C8 File Offset: 0x000621C8
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Duration;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600167D RID: 5757 RVA: 0x000631CC File Offset: 0x000621CC
		public override Type ValueType
		{
			get
			{
				return Datatype_duration.atomicValueType;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x000631D3 File Offset: 0x000621D3
		internal override Type ListValueType
		{
			get
			{
				return Datatype_duration.listValueType;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x000631DA File Offset: 0x000621DA
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x000631DD File Offset: 0x000621DD
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive;
			}
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000631E4 File Offset: 0x000621E4
		internal override int Compare(object value1, object value2)
		{
			return ((TimeSpan)value1).CompareTo(value2);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00063200 File Offset: 0x00062200
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			if (s == null || s.Length == 0)
			{
				return new XmlSchemaException("Sch_EmptyAttributeValue", string.Empty);
			}
			Exception ex = DatatypeImplementation.durationFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				TimeSpan timeSpan;
				ex = XmlConvert.TryToTimeSpan(s, out timeSpan);
				if (ex == null)
				{
					ex = DatatypeImplementation.durationFacetsChecker.CheckValueFacets(timeSpan, this);
					if (ex == null)
					{
						typedValue = timeSpan;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D82 RID: 3458
		private static readonly Type atomicValueType = typeof(TimeSpan);

		// Token: 0x04000D83 RID: 3459
		private static readonly Type listValueType = typeof(TimeSpan[]);
	}
}
