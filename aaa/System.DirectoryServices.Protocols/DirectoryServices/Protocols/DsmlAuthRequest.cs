using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003C RID: 60
	public class DsmlAuthRequest : DirectoryRequest
	{
		// Token: 0x06000144 RID: 324 RVA: 0x000061F8 File Offset: 0x000051F8
		public DsmlAuthRequest()
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000620B File Offset: 0x0000520B
		public DsmlAuthRequest(string principal)
		{
			this.directoryPrincipal = principal;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00006225 File Offset: 0x00005225
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000622D File Offset: 0x0000522D
		public string Principal
		{
			get
			{
				return this.directoryPrincipal;
			}
			set
			{
				this.directoryPrincipal = value;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006238 File Offset: 0x00005238
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "authRequest", false, null);
			XmlAttribute xmlAttribute = doc.CreateAttribute("principal", null);
			xmlAttribute.InnerText = this.Principal;
			xmlElement.Attributes.Append(xmlAttribute);
			return xmlElement;
		}

		// Token: 0x04000119 RID: 281
		private string directoryPrincipal = "";
	}
}
