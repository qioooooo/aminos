using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D3 RID: 467
	internal class Datatype_NCName : Datatype_Name
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060016EA RID: 5866 RVA: 0x0006391B File Offset: 0x0006291B
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NCName;
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00063920 File Offset: 0x00062920
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = DatatypeImplementation.stringFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				ex = DatatypeImplementation.stringFacetsChecker.CheckValueFacets(s, this);
				if (ex == null)
				{
					nameTable.Add(s);
					typedValue = s;
					return null;
				}
			}
			return ex;
		}
	}
}
