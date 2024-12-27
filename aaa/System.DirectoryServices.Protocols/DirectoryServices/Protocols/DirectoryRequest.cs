using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000034 RID: 52
	public abstract class DirectoryRequest : DirectoryOperation
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x000053B9 File Offset: 0x000043B9
		internal DirectoryRequest()
		{
			Utility.CheckOSVersion();
			this.directoryControlCollection = new DirectoryControlCollection();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000053D1 File Offset: 0x000043D1
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000053D9 File Offset: 0x000043D9
		public string RequestId
		{
			get
			{
				return this.directoryRequestID;
			}
			set
			{
				this.directoryRequestID = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000053E2 File Offset: 0x000043E2
		public DirectoryControlCollection Controls
		{
			get
			{
				return this.directoryControlCollection;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000053EA File Offset: 0x000043EA
		internal XmlElement ToXmlNodeHelper(XmlDocument doc)
		{
			return this.ToXmlNode(doc);
		}

		// Token: 0x060000FD RID: 253
		protected abstract XmlElement ToXmlNode(XmlDocument doc);

		// Token: 0x060000FE RID: 254 RVA: 0x000053F4 File Offset: 0x000043F4
		internal XmlElement CreateRequestElement(XmlDocument doc, string requestName, bool includeDistinguishedName, string distinguishedName)
		{
			XmlElement xmlElement = doc.CreateElement(requestName, "urn:oasis:names:tc:DSML:2:0:core");
			if (includeDistinguishedName)
			{
				XmlAttribute xmlAttribute = doc.CreateAttribute("dn", null);
				xmlAttribute.InnerText = distinguishedName;
				xmlElement.Attributes.Append(xmlAttribute);
			}
			if (this.directoryRequestID != null)
			{
				XmlAttribute xmlAttribute2 = doc.CreateAttribute("requestID", null);
				xmlAttribute2.InnerText = this.directoryRequestID;
				xmlElement.Attributes.Append(xmlAttribute2);
			}
			if (this.directoryControlCollection != null)
			{
				foreach (object obj in this.directoryControlCollection)
				{
					DirectoryControl directoryControl = (DirectoryControl)obj;
					XmlElement xmlElement2 = directoryControl.ToXmlNode(doc);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			return xmlElement;
		}

		// Token: 0x04000103 RID: 259
		internal DirectoryControlCollection directoryControlCollection;
	}
}
