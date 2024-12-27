using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200003A RID: 58
	public class ExtendedRequest : DirectoryRequest
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00005A1E File Offset: 0x00004A1E
		public ExtendedRequest()
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005A26 File Offset: 0x00004A26
		public ExtendedRequest(string requestName)
		{
			this.requestName = requestName;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005A35 File Offset: 0x00004A35
		public ExtendedRequest(string requestName, byte[] requestValue)
			: this(requestName)
		{
			this.requestValue = requestValue;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005A45 File Offset: 0x00004A45
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00005A4D File Offset: 0x00004A4D
		public string RequestName
		{
			get
			{
				return this.requestName;
			}
			set
			{
				this.requestName = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005A58 File Offset: 0x00004A58
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00005AA1 File Offset: 0x00004AA1
		public byte[] RequestValue
		{
			get
			{
				if (this.requestValue == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.requestValue.Length];
				for (int i = 0; i < this.requestValue.Length; i++)
				{
					array[i] = this.requestValue[i];
				}
				return array;
			}
			set
			{
				this.requestValue = value;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005AAC File Offset: 0x00004AAC
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "extendedRequest", false, null);
			XmlElement xmlElement2 = doc.CreateElement("requestName", "urn:oasis:names:tc:DSML:2:0:core");
			xmlElement2.InnerText = this.requestName;
			xmlElement.AppendChild(xmlElement2);
			if (this.requestValue != null)
			{
				XmlElement xmlElement3 = doc.CreateElement("requestValue", "urn:oasis:names:tc:DSML:2:0:core");
				xmlElement3.InnerText = Convert.ToBase64String(this.requestValue);
				XmlAttribute xmlAttribute = doc.CreateAttribute("xsi:type", "http://www.w3.org/2001/XMLSchema-instance");
				xmlAttribute.InnerText = "xsd:base64Binary";
				xmlElement3.Attributes.Append(xmlAttribute);
				xmlElement.AppendChild(xmlElement3);
			}
			return xmlElement;
		}

		// Token: 0x0400010F RID: 271
		private string requestName;

		// Token: 0x04000110 RID: 272
		private byte[] requestValue;
	}
}
