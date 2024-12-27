using System;

namespace System.Xml.Schema
{
	// Token: 0x020001B3 RID: 435
	internal class Datatype_string : Datatype_anySimpleType
	{
		// Token: 0x06001646 RID: 5702 RVA: 0x00062E4C File Offset: 0x00061E4C
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlStringConverter.Create(schemaType);
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x00062E54 File Offset: 0x00061E54
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Preserve;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x00062E57 File Offset: 0x00061E57
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.stringFacetsChecker;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x00062E5E File Offset: 0x00061E5E
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.String;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x00062E62 File Offset: 0x00061E62
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.CDATA;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x00062E65 File Offset: 0x00061E65
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00062E6C File Offset: 0x00061E6C
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.stringFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				ex = DatatypeImplementation.stringFacetsChecker.CheckValueFacets(s, this);
				if (ex == null)
				{
					typedValue = s;
					return null;
				}
			}
			return ex;
		}
	}
}
