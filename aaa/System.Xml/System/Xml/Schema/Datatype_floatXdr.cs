using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E6 RID: 486
	internal class Datatype_floatXdr : Datatype_float
	{
		// Token: 0x0600175A RID: 5978 RVA: 0x000642F4 File Offset: 0x000632F4
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			float num;
			try
			{
				num = XmlConvert.ToSingle(s);
			}
			catch (Exception ex)
			{
				throw new XmlSchemaException(Res.GetString("Sch_InvalidValue", new object[] { s }), ex);
			}
			if (float.IsInfinity(num) || float.IsNaN(num))
			{
				throw new XmlSchemaException("Sch_InvalidValue", s);
			}
			return num;
		}
	}
}
