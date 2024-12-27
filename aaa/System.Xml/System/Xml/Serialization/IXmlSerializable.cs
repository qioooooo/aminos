using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002BC RID: 700
	public interface IXmlSerializable
	{
		// Token: 0x0600216B RID: 8555
		XmlSchema GetSchema();

		// Token: 0x0600216C RID: 8556
		void ReadXml(XmlReader reader);

		// Token: 0x0600216D RID: 8557
		void WriteXml(XmlWriter writer);
	}
}
