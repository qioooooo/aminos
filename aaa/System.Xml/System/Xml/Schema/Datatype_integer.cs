using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D8 RID: 472
	internal class Datatype_integer : Datatype_decimal
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x00063B1C File Offset: 0x00062B1C
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Integer;
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00063B20 File Offset: 0x00062B20
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			Exception ex = this.FacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				decimal num;
				ex = XmlConvert.TryToInteger(s, out num);
				if (ex == null)
				{
					ex = this.FacetsChecker.CheckValueFacets(num, this);
					if (ex == null)
					{
						typedValue = num;
						return null;
					}
				}
			}
			return ex;
		}
	}
}
