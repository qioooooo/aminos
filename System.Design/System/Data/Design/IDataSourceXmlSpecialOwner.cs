using System;
using System.Xml;

namespace System.Data.Design
{
	internal interface IDataSourceXmlSpecialOwner
	{
		void ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer);

		void WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer);
	}
}
