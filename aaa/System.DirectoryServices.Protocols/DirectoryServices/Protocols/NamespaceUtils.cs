using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200006A RID: 106
	internal class NamespaceUtils
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0000A3FA File Offset: 0x000093FA
		private NamespaceUtils()
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A404 File Offset: 0x00009404
		static NamespaceUtils()
		{
			NamespaceUtils.xmlNamespace.AddNamespace("se", "http://schemas.xmlsoap.org/soap/envelope/");
			NamespaceUtils.xmlNamespace.AddNamespace("dsml", "urn:oasis:names:tc:DSML:2:0:core");
			NamespaceUtils.xmlNamespace.AddNamespace("ad", "urn:schema-microsoft-com:activedirectory:dsmlv2");
			NamespaceUtils.xmlNamespace.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
			NamespaceUtils.xmlNamespace.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000A484 File Offset: 0x00009484
		public static XmlNamespaceManager GetDsmlNamespaceManager()
		{
			return NamespaceUtils.xmlNamespace;
		}

		// Token: 0x04000219 RID: 537
		private static XmlNamespaceManager xmlNamespace = new XmlNamespaceManager(new NameTable());
	}
}
