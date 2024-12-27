using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BF RID: 191
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CipherReference : EncryptedReference
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x00017082 File Offset: 0x00016082
		public CipherReference()
		{
			base.ReferenceType = "CipherReference";
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00017095 File Offset: 0x00016095
		public CipherReference(string uri)
			: base(uri)
		{
			base.ReferenceType = "CipherReference";
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000170A9 File Offset: 0x000160A9
		public CipherReference(string uri, TransformChain transformChain)
			: base(uri, transformChain)
		{
			base.ReferenceType = "CipherReference";
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000170BE File Offset: 0x000160BE
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x000170D0 File Offset: 0x000160D0
		internal byte[] CipherValue
		{
			get
			{
				if (!base.CacheValid)
				{
					return null;
				}
				return this.m_cipherValue;
			}
			set
			{
				this.m_cipherValue = value;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000170DC File Offset: 0x000160DC
		public override XmlElement GetXml()
		{
			if (base.CacheValid)
			{
				return this.m_cachedXml;
			}
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001710C File Offset: 0x0001610C
		internal new XmlElement GetXml(XmlDocument document)
		{
			if (base.ReferenceType == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_ReferenceTypeRequired"));
			}
			XmlElement xmlElement = document.CreateElement(base.ReferenceType, "http://www.w3.org/2001/04/xmlenc#");
			if (!string.IsNullOrEmpty(base.Uri))
			{
				xmlElement.SetAttribute("URI", base.Uri);
			}
			if (base.TransformChain.Count > 0)
			{
				xmlElement.AppendChild(base.TransformChain.GetXml(document, "http://www.w3.org/2001/04/xmlenc#"));
			}
			return xmlElement;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00017188 File Offset: 0x00016188
		public override void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			base.ReferenceType = value.LocalName;
			string attribute = Utils.GetAttribute(value, "URI", "http://www.w3.org/2001/04/xmlenc#");
			if (!Utils.GetSkipSignatureAttributeEnforcement() && attribute == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UriRequired"));
			}
			base.Uri = attribute;
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			XmlNode xmlNode = value.SelectSingleNode("enc:Transforms", xmlNamespaceManager);
			if (xmlNode != null)
			{
				base.TransformChain.LoadXml(xmlNode as XmlElement);
			}
			this.m_cachedXml = value;
		}

		// Token: 0x040005A6 RID: 1446
		private byte[] m_cipherValue;
	}
}
