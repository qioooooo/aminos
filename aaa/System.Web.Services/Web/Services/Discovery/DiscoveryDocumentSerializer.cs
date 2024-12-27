using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A0 RID: 160
	internal class DiscoveryDocumentSerializer : XmlSerializer
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x00014EC7 File Offset: 0x00013EC7
		protected override XmlSerializationReader CreateReader()
		{
			return new DiscoveryDocumentSerializationReader();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00014ECE File Offset: 0x00013ECE
		protected override XmlSerializationWriter CreateWriter()
		{
			return new DiscoveryDocumentSerializationWriter();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00014ED5 File Offset: 0x00013ED5
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("discovery", "http://schemas.xmlsoap.org/disco/");
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00014EE7 File Offset: 0x00013EE7
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((DiscoveryDocumentSerializationWriter)writer).Write10_discovery(objectToSerialize);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00014EF5 File Offset: 0x00013EF5
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((DiscoveryDocumentSerializationReader)reader).Read10_discovery();
		}
	}
}
