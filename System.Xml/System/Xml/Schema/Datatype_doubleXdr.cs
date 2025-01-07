using System;

namespace System.Xml.Schema
{
	internal class Datatype_doubleXdr : Datatype_double
	{
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
