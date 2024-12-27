using System;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x0200008A RID: 138
	internal interface IDataSourceXmlSpecialOwner
	{
		// Token: 0x06000581 RID: 1409
		void ReadSpecialItem(string propertyName, XmlNode xmlNode, DataSourceXmlSerializer serializer);

		// Token: 0x06000582 RID: 1410
		void WriteSpecialItem(string propertyName, XmlWriter writer, DataSourceXmlSerializer serializer);
	}
}
