using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BB RID: 187
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class EncryptionMethod
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x00016903 File Offset: 0x00015903
		public EncryptionMethod()
		{
			this.m_cachedXml = null;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00016912 File Offset: 0x00015912
		public EncryptionMethod(string algorithm)
		{
			this.m_algorithm = algorithm;
			this.m_cachedXml = null;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00016928 File Offset: 0x00015928
		private bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00016936 File Offset: 0x00015936
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0001693E File Offset: 0x0001593E
		public int KeySize
		{
			get
			{
				return this.m_keySize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidKeySize"));
				}
				this.m_keySize = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00016962 File Offset: 0x00015962
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x0001696A File Offset: 0x0001596A
		public string KeyAlgorithm
		{
			get
			{
				return this.m_algorithm;
			}
			set
			{
				this.m_algorithm = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001697C File Offset: 0x0001597C
		public XmlElement GetXml()
		{
			if (this.CacheValid)
			{
				return this.m_cachedXml;
			}
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000169AC File Offset: 0x000159AC
		internal XmlElement GetXml(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#");
			if (!string.IsNullOrEmpty(this.m_algorithm))
			{
				xmlElement.SetAttribute("Algorithm", this.m_algorithm);
			}
			if (this.m_keySize > 0)
			{
				XmlElement xmlElement2 = document.CreateElement("KeySize", "http://www.w3.org/2001/04/xmlenc#");
				xmlElement2.AppendChild(document.CreateTextNode(this.m_keySize.ToString(null, null)));
				xmlElement.AppendChild(xmlElement2);
			}
			return xmlElement;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00016A28 File Offset: 0x00015A28
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			this.m_algorithm = Utils.GetAttribute(value, "Algorithm", "http://www.w3.org/2001/04/xmlenc#");
			XmlNode xmlNode = value.SelectSingleNode("enc:KeySize", xmlNamespaceManager);
			if (xmlNode != null)
			{
				this.KeySize = Convert.ToInt32(Utils.DiscardWhiteSpaces(xmlNode.InnerText), null);
			}
			this.m_cachedXml = value;
		}

		// Token: 0x0400059A RID: 1434
		private XmlElement m_cachedXml;

		// Token: 0x0400059B RID: 1435
		private int m_keySize;

		// Token: 0x0400059C RID: 1436
		private string m_algorithm;
	}
}
