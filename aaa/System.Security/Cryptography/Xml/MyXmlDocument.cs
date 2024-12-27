using System;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000082 RID: 130
	internal class MyXmlDocument : XmlDocument
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000C122 File Offset: 0x0000B122
		protected override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return this.CreateAttribute(prefix, localName, namespaceURI);
		}
	}
}
