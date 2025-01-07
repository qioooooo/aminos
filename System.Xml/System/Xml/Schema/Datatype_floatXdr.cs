using System;

namespace System.Xml.Schema
{
	internal class Datatype_floatXdr : Datatype_float
	{
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
