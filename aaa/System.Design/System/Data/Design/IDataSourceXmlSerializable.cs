using System;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x0200009B RID: 155
	internal interface IDataSourceXmlSerializable
	{
		// Token: 0x060006C3 RID: 1731
		void ReadXml(XmlElement xmlElement, DataSourceXmlSerializer serializer);

		// Token: 0x060006C4 RID: 1732
		void WriteXml(XmlWriter writer, DataSourceXmlSerializer serializer);
	}
}
