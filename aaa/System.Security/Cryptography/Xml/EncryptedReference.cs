using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BE RID: 190
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public abstract class EncryptedReference
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x00016E87 File Offset: 0x00015E87
		protected EncryptedReference()
			: this(string.Empty, new TransformChain())
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00016E99 File Offset: 0x00015E99
		protected EncryptedReference(string uri)
			: this(uri, new TransformChain())
		{
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00016EA7 File Offset: 0x00015EA7
		protected EncryptedReference(string uri, TransformChain transformChain)
		{
			this.TransformChain = transformChain;
			this.Uri = uri;
			this.m_cachedXml = null;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00016EC4 File Offset: 0x00015EC4
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x00016ECC File Offset: 0x00015ECC
		public string Uri
		{
			get
			{
				return this.m_uri;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(SecurityResources.GetResourceString("Cryptography_Xml_UriRequired"));
				}
				this.m_uri = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00016EEF File Offset: 0x00015EEF
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x00016F0A File Offset: 0x00015F0A
		public TransformChain TransformChain
		{
			get
			{
				if (this.m_transformChain == null)
				{
					this.m_transformChain = new TransformChain();
				}
				return this.m_transformChain;
			}
			set
			{
				this.m_transformChain = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00016F1A File Offset: 0x00015F1A
		public void AddTransform(Transform transform)
		{
			this.TransformChain.Add(transform);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00016F28 File Offset: 0x00015F28
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00016F30 File Offset: 0x00015F30
		protected string ReferenceType
		{
			get
			{
				return this.m_referenceType;
			}
			set
			{
				this.m_referenceType = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00016F40 File Offset: 0x00015F40
		protected internal bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00016F50 File Offset: 0x00015F50
		public virtual XmlElement GetXml()
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

		// Token: 0x06000491 RID: 1169 RVA: 0x00016F80 File Offset: 0x00015F80
		internal XmlElement GetXml(XmlDocument document)
		{
			if (this.ReferenceType == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_ReferenceTypeRequired"));
			}
			XmlElement xmlElement = document.CreateElement(this.ReferenceType, "http://www.w3.org/2001/04/xmlenc#");
			if (!string.IsNullOrEmpty(this.m_uri))
			{
				xmlElement.SetAttribute("URI", this.m_uri);
			}
			if (this.TransformChain.Count > 0)
			{
				xmlElement.AppendChild(this.TransformChain.GetXml(document, "http://www.w3.org/2000/09/xmldsig#"));
			}
			return xmlElement;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00016FFC File Offset: 0x00015FFC
		public virtual void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.ReferenceType = value.LocalName;
			this.Uri = Utils.GetAttribute(value, "URI", "http://www.w3.org/2001/04/xmlenc#");
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlNode xmlNode = value.SelectSingleNode("ds:Transforms", xmlNamespaceManager);
			if (xmlNode != null)
			{
				this.TransformChain.LoadXml(xmlNode as XmlElement);
			}
			this.m_cachedXml = value;
		}

		// Token: 0x040005A2 RID: 1442
		private string m_uri;

		// Token: 0x040005A3 RID: 1443
		private string m_referenceType;

		// Token: 0x040005A4 RID: 1444
		private TransformChain m_transformChain;

		// Token: 0x040005A5 RID: 1445
		internal XmlElement m_cachedXml;
	}
}
