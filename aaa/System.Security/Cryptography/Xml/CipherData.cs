using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C5 RID: 197
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CipherData
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x00017DAC File Offset: 0x00016DAC
		public CipherData()
		{
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00017DB4 File Offset: 0x00016DB4
		public CipherData(byte[] cipherValue)
		{
			this.CipherValue = cipherValue;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00017DC3 File Offset: 0x00016DC3
		public CipherData(CipherReference cipherReference)
		{
			this.CipherReference = cipherReference;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00017DD2 File Offset: 0x00016DD2
		private bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00017DE0 File Offset: 0x00016DE0
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00017DE8 File Offset: 0x00016DE8
		public CipherReference CipherReference
		{
			get
			{
				return this.m_cipherReference;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.CipherValue != null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CipherValueElementRequired"));
				}
				this.m_cipherReference = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00017E1E File Offset: 0x00016E1E
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00017E26 File Offset: 0x00016E26
		public byte[] CipherValue
		{
			get
			{
				return this.m_cipherValue;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.CipherReference != null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CipherValueElementRequired"));
				}
				this.m_cipherValue = (byte[])value.Clone();
				this.m_cachedXml = null;
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00017E68 File Offset: 0x00016E68
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

		// Token: 0x060004CD RID: 1229 RVA: 0x00017E98 File Offset: 0x00016E98
		internal XmlElement GetXml(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("CipherData", "http://www.w3.org/2001/04/xmlenc#");
			if (this.CipherValue != null)
			{
				XmlElement xmlElement2 = document.CreateElement("CipherValue", "http://www.w3.org/2001/04/xmlenc#");
				xmlElement2.AppendChild(document.CreateTextNode(Convert.ToBase64String(this.CipherValue)));
				xmlElement.AppendChild(xmlElement2);
			}
			else
			{
				if (this.CipherReference == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CipherValueElementRequired"));
				}
				xmlElement.AppendChild(this.CipherReference.GetXml(document));
			}
			return xmlElement;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00017F20 File Offset: 0x00016F20
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			XmlNode xmlNode = value.SelectSingleNode("enc:CipherValue", xmlNamespaceManager);
			XmlNode xmlNode2 = value.SelectSingleNode("enc:CipherReference", xmlNamespaceManager);
			if (xmlNode != null)
			{
				if (xmlNode2 != null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CipherValueElementRequired"));
				}
				this.m_cipherValue = Convert.FromBase64String(Utils.DiscardWhiteSpaces(xmlNode.InnerText));
			}
			else
			{
				if (xmlNode2 == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CipherValueElementRequired"));
				}
				this.m_cipherReference = new CipherReference();
				this.m_cipherReference.LoadXml((XmlElement)xmlNode2);
			}
			this.m_cachedXml = value;
		}

		// Token: 0x040005AB RID: 1451
		private XmlElement m_cachedXml;

		// Token: 0x040005AC RID: 1452
		private CipherReference m_cipherReference;

		// Token: 0x040005AD RID: 1453
		private byte[] m_cipherValue;
	}
}
