using System;

namespace System.Xml.Schema
{
	// Token: 0x020001E5 RID: 485
	internal class Datatype_doubleXdr : Datatype_double
	{
		// Token: 0x06001758 RID: 5976 RVA: 0x00064284 File Offset: 0x00063284
		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			double num;
			try
			{
				num = XmlConvert.ToDouble(s);
			}
			catch (Exception ex)
			{
				throw new XmlSchemaException(Res.GetString("Sch_InvalidValue", new object[] { s }), ex);
			}
			if (double.IsInfinity(num) || double.IsNaN(num))
			{
				throw new XmlSchemaException("Sch_InvalidValue", s);
			}
			return num;
		}
	}
}
