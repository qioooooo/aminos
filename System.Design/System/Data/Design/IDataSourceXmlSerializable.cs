using System;
using System.Xml;

namespace System.Data.Design
{
	internal interface IDataSourceXmlSerializable
	{
		void ReadXml(XmlElement xmlElement, DataSourceXmlSerializer serializer);

		void WriteXml(XmlWriter writer, DataSourceXmlSerializer serializer);
	}
}
