using System;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B7 RID: 183
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlLicenseTransform : Transform
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x00015E34 File Offset: 0x00014E34
		public XmlLicenseTransform()
		{
			base.Algorithm = "urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform";
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00015E88 File Offset: 0x00014E88
		public override Type[] InputTypes
		{
			get
			{
				return this.inputTypes;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00015E90 File Offset: 0x00014E90
		public override Type[] OutputTypes
		{
			get
			{
				return this.outputTypes;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00015E98 File Offset: 0x00014E98
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00015EA0 File Offset: 0x00014EA0
		public IRelDecryptor Decryptor
		{
			get
			{
				return this.relDecryptor;
			}
			set
			{
				this.relDecryptor = value;
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00015EAC File Offset: 0x00014EAC
		private void DecryptEncryptedGrants(XmlNodeList encryptedGrantList, IRelDecryptor decryptor)
		{
			int i = 0;
			int count = encryptedGrantList.Count;
			while (i < count)
			{
				XmlElement xmlElement = encryptedGrantList[i].SelectSingleNode("//r:encryptedGrant/enc:EncryptionMethod", this.namespaceManager) as XmlElement;
				XmlElement xmlElement2 = encryptedGrantList[i].SelectSingleNode("//r:encryptedGrant/dsig:KeyInfo", this.namespaceManager) as XmlElement;
				XmlElement xmlElement3 = encryptedGrantList[i].SelectSingleNode("//r:encryptedGrant/enc:CipherData", this.namespaceManager) as XmlElement;
				if (xmlElement != null && xmlElement2 != null && xmlElement3 != null)
				{
					EncryptionMethod encryptionMethod = new EncryptionMethod();
					KeyInfo keyInfo = new KeyInfo();
					CipherData cipherData = new CipherData();
					encryptionMethod.LoadXml(xmlElement);
					keyInfo.LoadXml(xmlElement2);
					cipherData.LoadXml(xmlElement3);
					MemoryStream memoryStream = null;
					Stream stream = null;
					StreamReader streamReader = null;
					try
					{
						memoryStream = new MemoryStream(cipherData.CipherValue);
						stream = this.relDecryptor.Decrypt(encryptionMethod, keyInfo, memoryStream);
						if (stream == null || stream.Length == 0L)
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_XrmlUnableToDecryptGrant"));
						}
						streamReader = new StreamReader(stream);
						string text = streamReader.ReadToEnd();
						encryptedGrantList[i].ParentNode.InnerXml = text;
					}
					finally
					{
						if (memoryStream != null)
						{
							memoryStream.Close();
						}
						if (stream != null)
						{
							stream.Close();
						}
						if (streamReader != null)
						{
							streamReader.Close();
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00016030 File Offset: 0x00015030
		protected override XmlNodeList GetInnerXml()
		{
			return null;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00016033 File Offset: 0x00015033
		public override object GetOutput()
		{
			return this.license;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001603B File Offset: 0x0001503B
		public override object GetOutput(Type type)
		{
			if (type != typeof(XmlDocument) || !type.IsSubclassOf(typeof(XmlDocument)))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
			}
			return this.GetOutput();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00016077 File Offset: 0x00015077
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (!Utils.GetAllowAdditionalSignatureNodes() && nodeList != null && nodeList.Count > 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001609C File Offset: 0x0001509C
		public override void LoadInput(object obj)
		{
			if (base.Context == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_XrmlMissingContext"));
			}
			this.license = new XmlDocument();
			this.license.PreserveWhitespace = true;
			this.namespaceManager = new XmlNamespaceManager(this.license.NameTable);
			this.namespaceManager.AddNamespace("dsig", "http://www.w3.org/2000/09/xmldsig#");
			this.namespaceManager.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			this.namespaceManager.AddNamespace("r", "urn:mpeg:mpeg21:2003:01-REL-R-NS");
			XmlElement xmlElement = base.Context.SelectSingleNode("ancestor-or-self::r:issuer[1]", this.namespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_XrmlMissingIssuer"));
			}
			XmlNode xmlNode = xmlElement.SelectSingleNode("descendant-or-self::dsig:Signature[1]", this.namespaceManager) as XmlElement;
			if (xmlNode != null)
			{
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			XmlElement xmlElement2 = xmlElement.SelectSingleNode("ancestor-or-self::r:license[1]", this.namespaceManager) as XmlElement;
			if (xmlElement2 == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_XrmlMissingLicence"));
			}
			XmlNodeList xmlNodeList = xmlElement2.SelectNodes("descendant-or-self::r:license[1]/r:issuer", this.namespaceManager);
			int i = 0;
			int count = xmlNodeList.Count;
			while (i < count)
			{
				if (xmlNodeList[i] != xmlElement && xmlNodeList[i].LocalName == "issuer" && xmlNodeList[i].NamespaceURI == "urn:mpeg:mpeg21:2003:01-REL-R-NS")
				{
					xmlNodeList[i].ParentNode.RemoveChild(xmlNodeList[i]);
				}
				i++;
			}
			XmlNodeList xmlNodeList2 = xmlElement2.SelectNodes("/r:license/r:grant/r:encryptedGrant", this.namespaceManager);
			if (xmlNodeList2.Count > 0)
			{
				if (this.relDecryptor == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_XrmlMissingIRelDecryptor"));
				}
				this.DecryptEncryptedGrants(xmlNodeList2, this.relDecryptor);
			}
			this.license.InnerXml = xmlElement2.OuterXml;
		}

		// Token: 0x04000588 RID: 1416
		private const string ElementIssuer = "issuer";

		// Token: 0x04000589 RID: 1417
		private const string NamespaceUriCore = "urn:mpeg:mpeg21:2003:01-REL-R-NS";

		// Token: 0x0400058A RID: 1418
		private Type[] inputTypes = new Type[] { typeof(XmlDocument) };

		// Token: 0x0400058B RID: 1419
		private Type[] outputTypes = new Type[] { typeof(XmlDocument) };

		// Token: 0x0400058C RID: 1420
		private XmlNamespaceManager namespaceManager;

		// Token: 0x0400058D RID: 1421
		private XmlDocument license;

		// Token: 0x0400058E RID: 1422
		private IRelDecryptor relDecryptor;
	}
}
