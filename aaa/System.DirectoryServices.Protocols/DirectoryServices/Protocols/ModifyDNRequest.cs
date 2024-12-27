using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000039 RID: 57
	public class ModifyDNRequest : DirectoryRequest
	{
		// Token: 0x0600011C RID: 284 RVA: 0x000058FA File Offset: 0x000048FA
		public ModifyDNRequest()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005909 File Offset: 0x00004909
		public ModifyDNRequest(string distinguishedName, string newParentDistinguishedName, string newName)
		{
			this.dn = distinguishedName;
			this.newSuperior = newParentDistinguishedName;
			this.newRDN = newName;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000592D File Offset: 0x0000492D
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00005935 File Offset: 0x00004935
		public string DistinguishedName
		{
			get
			{
				return this.dn;
			}
			set
			{
				this.dn = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000593E File Offset: 0x0000493E
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005946 File Offset: 0x00004946
		public string NewParentDistinguishedName
		{
			get
			{
				return this.newSuperior;
			}
			set
			{
				this.newSuperior = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000594F File Offset: 0x0000494F
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00005957 File Offset: 0x00004957
		public string NewName
		{
			get
			{
				return this.newRDN;
			}
			set
			{
				this.newRDN = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005960 File Offset: 0x00004960
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00005968 File Offset: 0x00004968
		public bool DeleteOldRdn
		{
			get
			{
				return this.deleteOldRDN;
			}
			set
			{
				this.deleteOldRDN = value;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005974 File Offset: 0x00004974
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "modDNRequest", true, this.dn);
			XmlAttribute xmlAttribute = doc.CreateAttribute("newrdn", null);
			xmlAttribute.InnerText = this.newRDN;
			xmlElement.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("deleteoldrdn", null);
			xmlAttribute2.InnerText = (this.deleteOldRDN ? "true" : "false");
			xmlElement.Attributes.Append(xmlAttribute2);
			if (this.newSuperior != null)
			{
				XmlAttribute xmlAttribute3 = doc.CreateAttribute("newSuperior", null);
				xmlAttribute3.InnerText = this.newSuperior;
				xmlElement.Attributes.Append(xmlAttribute3);
			}
			return xmlElement;
		}

		// Token: 0x0400010B RID: 267
		private string dn;

		// Token: 0x0400010C RID: 268
		private string newSuperior;

		// Token: 0x0400010D RID: 269
		private string newRDN;

		// Token: 0x0400010E RID: 270
		private bool deleteOldRDN = true;
	}
}
