using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A3 RID: 163
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfoX509Data : KeyInfoClause
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00010249 File Offset: 0x0000F249
		public KeyInfoX509Data()
		{
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00010254 File Offset: 0x0000F254
		public KeyInfoX509Data(byte[] rgbCert)
		{
			X509Certificate2 x509Certificate = new X509Certificate2(rgbCert);
			this.AddCertificate(x509Certificate);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00010275 File Offset: 0x0000F275
		public KeyInfoX509Data(X509Certificate cert)
		{
			this.AddCertificate(cert);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00010284 File Offset: 0x0000F284
		public KeyInfoX509Data(X509Certificate cert, X509IncludeOption includeOption)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			X509Certificate2 x509Certificate = new X509Certificate2(cert);
			switch (includeOption)
			{
			case X509IncludeOption.ExcludeRoot:
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.Build(x509Certificate);
				if (x509Chain.ChainStatus.Length > 0 && (x509Chain.ChainStatus[0].Status & X509ChainStatusFlags.PartialChain) == X509ChainStatusFlags.PartialChain)
				{
					throw new CryptographicException(-2146762486);
				}
				X509ChainElementCollection x509ChainElementCollection = x509Chain.ChainElements;
				for (int i = 0; i < (X509Utils.IsSelfSigned(x509Chain) ? 1 : (x509ChainElementCollection.Count - 1)); i++)
				{
					this.AddCertificate(x509ChainElementCollection[i].Certificate);
				}
				return;
			}
			case X509IncludeOption.EndCertOnly:
				this.AddCertificate(x509Certificate);
				return;
			case X509IncludeOption.WholeChain:
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.Build(x509Certificate);
				if (x509Chain.ChainStatus.Length > 0 && (x509Chain.ChainStatus[0].Status & X509ChainStatusFlags.PartialChain) == X509ChainStatusFlags.PartialChain)
				{
					throw new CryptographicException(-2146762486);
				}
				X509ChainElementCollection x509ChainElementCollection = x509Chain.ChainElements;
				foreach (X509ChainElement x509ChainElement in x509ChainElementCollection)
				{
					this.AddCertificate(x509ChainElement.Certificate);
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000320 RID: 800 RVA: 0x000103BD File Offset: 0x0000F3BD
		public ArrayList Certificates
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000103C8 File Offset: 0x0000F3C8
		public void AddCertificate(X509Certificate certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (this.m_certificates == null)
			{
				this.m_certificates = new ArrayList();
			}
			X509Certificate2 x509Certificate = new X509Certificate2(certificate);
			this.m_certificates.Add(x509Certificate);
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0001040A File Offset: 0x0000F40A
		public ArrayList SubjectKeyIds
		{
			get
			{
				return this.m_subjectKeyIds;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010412 File Offset: 0x0000F412
		public void AddSubjectKeyId(byte[] subjectKeyId)
		{
			if (this.m_subjectKeyIds == null)
			{
				this.m_subjectKeyIds = new ArrayList();
			}
			this.m_subjectKeyIds.Add(subjectKeyId);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00010434 File Offset: 0x0000F434
		[ComVisible(false)]
		public void AddSubjectKeyId(string subjectKeyId)
		{
			if (this.m_subjectKeyIds == null)
			{
				this.m_subjectKeyIds = new ArrayList();
			}
			this.m_subjectKeyIds.Add(X509Utils.DecodeHexString(subjectKeyId));
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0001045B File Offset: 0x0000F45B
		public ArrayList SubjectNames
		{
			get
			{
				return this.m_subjectNames;
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00010463 File Offset: 0x0000F463
		public void AddSubjectName(string subjectName)
		{
			if (this.m_subjectNames == null)
			{
				this.m_subjectNames = new ArrayList();
			}
			this.m_subjectNames.Add(subjectName);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00010485 File Offset: 0x0000F485
		public ArrayList IssuerSerials
		{
			get
			{
				return this.m_issuerSerials;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00010490 File Offset: 0x0000F490
		public void AddIssuerSerial(string issuerName, string serialNumber)
		{
			BigInt bigInt = new BigInt();
			bigInt.FromHexadecimal(serialNumber);
			if (this.m_issuerSerials == null)
			{
				this.m_issuerSerials = new ArrayList();
			}
			this.m_issuerSerials.Add(new X509IssuerSerial(issuerName, bigInt.ToDecimal()));
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000104DA File Offset: 0x0000F4DA
		internal void InternalAddIssuerSerial(string issuerName, string serialNumber)
		{
			if (this.m_issuerSerials == null)
			{
				this.m_issuerSerials = new ArrayList();
			}
			this.m_issuerSerials.Add(new X509IssuerSerial(issuerName, serialNumber));
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00010507 File Offset: 0x0000F507
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0001050F File Offset: 0x0000F50F
		public byte[] CRL
		{
			get
			{
				return this.m_CRL;
			}
			set
			{
				this.m_CRL = value;
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00010518 File Offset: 0x0000F518
		private void Clear()
		{
			this.m_CRL = null;
			if (this.m_subjectKeyIds != null)
			{
				this.m_subjectKeyIds.Clear();
			}
			if (this.m_subjectNames != null)
			{
				this.m_subjectNames.Clear();
			}
			if (this.m_issuerSerials != null)
			{
				this.m_issuerSerials.Clear();
			}
			if (this.m_certificates != null)
			{
				this.m_certificates.Clear();
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00010578 File Offset: 0x0000F578
		public override XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001059C File Offset: 0x0000F59C
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			XmlElement xmlElement = xmlDocument.CreateElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
			if (this.m_issuerSerials != null)
			{
				foreach (object obj in this.m_issuerSerials)
				{
					X509IssuerSerial x509IssuerSerial = (X509IssuerSerial)obj;
					XmlElement xmlElement2 = xmlDocument.CreateElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
					XmlElement xmlElement3 = xmlDocument.CreateElement("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#");
					xmlElement3.AppendChild(xmlDocument.CreateTextNode(x509IssuerSerial.IssuerName));
					xmlElement2.AppendChild(xmlElement3);
					XmlElement xmlElement4 = xmlDocument.CreateElement("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#");
					xmlElement4.AppendChild(xmlDocument.CreateTextNode(x509IssuerSerial.SerialNumber));
					xmlElement2.AppendChild(xmlElement4);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (this.m_subjectKeyIds != null)
			{
				foreach (object obj2 in this.m_subjectKeyIds)
				{
					byte[] array = (byte[])obj2;
					XmlElement xmlElement5 = xmlDocument.CreateElement("X509SKI", "http://www.w3.org/2000/09/xmldsig#");
					xmlElement5.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(array)));
					xmlElement.AppendChild(xmlElement5);
				}
			}
			if (this.m_subjectNames != null)
			{
				foreach (object obj3 in this.m_subjectNames)
				{
					string text = (string)obj3;
					XmlElement xmlElement6 = xmlDocument.CreateElement("X509SubjectName", "http://www.w3.org/2000/09/xmldsig#");
					xmlElement6.AppendChild(xmlDocument.CreateTextNode(text));
					xmlElement.AppendChild(xmlElement6);
				}
			}
			if (this.m_certificates != null)
			{
				foreach (object obj4 in this.m_certificates)
				{
					X509Certificate x509Certificate = (X509Certificate)obj4;
					XmlElement xmlElement7 = xmlDocument.CreateElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#");
					xmlElement7.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(x509Certificate.GetRawCertData())));
					xmlElement.AppendChild(xmlElement7);
				}
			}
			if (this.m_CRL != null)
			{
				XmlElement xmlElement8 = xmlDocument.CreateElement("X509CRL", "http://www.w3.org/2000/09/xmldsig#");
				xmlElement8.AppendChild(xmlDocument.CreateTextNode(Convert.ToBase64String(this.m_CRL)));
				xmlElement.AppendChild(xmlElement8);
			}
			return xmlElement;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00010850 File Offset: 0x0000F850
		public override void LoadXml(XmlElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(element.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlNodeList xmlNodeList = element.SelectNodes("ds:X509IssuerSerial", xmlNamespaceManager);
			XmlNodeList xmlNodeList2 = element.SelectNodes("ds:X509SKI", xmlNamespaceManager);
			XmlNodeList xmlNodeList3 = element.SelectNodes("ds:X509SubjectName", xmlNamespaceManager);
			XmlNodeList xmlNodeList4 = element.SelectNodes("ds:X509Certificate", xmlNamespaceManager);
			XmlNodeList xmlNodeList5 = element.SelectNodes("ds:X509CRL", xmlNamespaceManager);
			if (xmlNodeList5.Count == 0 && xmlNodeList.Count == 0 && xmlNodeList2.Count == 0 && xmlNodeList3.Count == 0 && xmlNodeList4.Count == 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "X509Data");
			}
			this.Clear();
			if (xmlNodeList5.Count != 0)
			{
				this.m_CRL = Convert.FromBase64String(Utils.DiscardWhiteSpaces(xmlNodeList5.Item(0).InnerText));
			}
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("ds:X509IssuerName", xmlNamespaceManager);
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("ds:X509SerialNumber", xmlNamespaceManager);
				if (xmlNode2 == null || xmlNode3 == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "IssuerSerial");
				}
				this.InternalAddIssuerSerial(xmlNode2.InnerText.Trim(), xmlNode3.InnerText.Trim());
			}
			foreach (object obj2 in xmlNodeList2)
			{
				XmlNode xmlNode4 = (XmlNode)obj2;
				this.AddSubjectKeyId(Convert.FromBase64String(Utils.DiscardWhiteSpaces(xmlNode4.InnerText)));
			}
			foreach (object obj3 in xmlNodeList3)
			{
				XmlNode xmlNode5 = (XmlNode)obj3;
				this.AddSubjectName(xmlNode5.InnerText.Trim());
			}
			foreach (object obj4 in xmlNodeList4)
			{
				XmlNode xmlNode6 = (XmlNode)obj4;
				this.AddCertificate(new X509Certificate2(Convert.FromBase64String(Utils.DiscardWhiteSpaces(xmlNode6.InnerText))));
			}
		}

		// Token: 0x040004FE RID: 1278
		private ArrayList m_certificates;

		// Token: 0x040004FF RID: 1279
		private ArrayList m_issuerSerials;

		// Token: 0x04000500 RID: 1280
		private ArrayList m_subjectKeyIds;

		// Token: 0x04000501 RID: 1281
		private ArrayList m_subjectNames;

		// Token: 0x04000502 RID: 1282
		private byte[] m_CRL;
	}
}
