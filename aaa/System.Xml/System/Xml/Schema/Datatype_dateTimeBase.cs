using System;

namespace System.Xml.Schema
{
	// Token: 0x020001BB RID: 443
	internal class Datatype_dateTimeBase : Datatype_anySimpleType
	{
		// Token: 0x0600168B RID: 5771 RVA: 0x00063390 File Offset: 0x00062390
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlDateTimeConverter.Create(schemaType);
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x00063398 File Offset: 0x00062398
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.dateTimeFacetsChecker;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x0006339F File Offset: 0x0006239F
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.DateTime;
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000633A3 File Offset: 0x000623A3
		internal Datatype_dateTimeBase()
		{
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x000633AB File Offset: 0x000623AB
		internal Datatype_dateTimeBase(XsdDateTimeFlags dateTimeFlags)
		{
			this.dateTimeFlags = dateTimeFlags;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x000633BA File Offset: 0x000623BA
		public override Type ValueType
		{
			get
			{
				return Datatype_dateTimeBase.atomicValueType;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x000633C1 File Offset: 0x000623C1
		internal override Type ListValueType
		{
			get
			{
				return Datatype_dateTimeBase.listValueType;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x000633C8 File Offset: 0x000623C8
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x000633CB File Offset: 0x000623CB
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace | RestrictionFlags.MaxInclusive | RestrictionFlags.MaxExclusive | RestrictionFlags.MinInclusive | RestrictionFlags.MinExclusive;
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000633D4 File Offset: 0x000623D4
		internal override int Compare(object value1, object value2)
		{
			DateTime dateTime = (DateTime)value1;
			DateTime dateTime2 = (DateTime)value2;
			if (dateTime.Kind == DateTimeKind.Unspecified || dateTime2.Kind == DateTimeKind.Unspecified)
			{
				return dateTime.CompareTo(dateTime2);
			}
			return dateTime.ToUniversalTime().CompareTo(dateTime2.ToUniversalTime());
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00063420 File Offset: 0x00062420
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.dateTimeFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				XsdDateTime xsdDateTime;
				if (!XsdDateTime.TryParse(s, this.dateTimeFlags, out xsdDateTime))
				{
					ex = new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, "XsdDateTime" }));
				}
				else
				{
					DateTime dateTime = DateTime.MinValue;
					try
					{
						dateTime = xsdDateTime;
					}
					catch (ArgumentException ex2)
					{
						return ex2;
					}
					ex = DatatypeImplementation.dateTimeFacetsChecker.CheckValueFacets(dateTime, this);
					if (ex == null)
					{
						typedValue = dateTime;
						return null;
					}
				}
			}
			return ex;
		}

		// Token: 0x04000D84 RID: 3460
		private static readonly Type atomicValueType = typeof(DateTime);

		// Token: 0x04000D85 RID: 3461
		private static readonly Type listValueType = typeof(DateTime[]);

		// Token: 0x04000D86 RID: 3462
		private XsdDateTimeFlags dateTimeFlags;
	}
}
