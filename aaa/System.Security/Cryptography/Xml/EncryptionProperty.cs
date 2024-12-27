using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BC RID: 188
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EncryptionProperty
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x00016AAA File Offset: 0x00015AAA
		public EncryptionProperty()
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00016AB4 File Offset: 0x00015AB4
		public EncryptionProperty(XmlElement elementProperty)
		{
			if (elementProperty == null)
			{
				throw new ArgumentNullException("elementProperty");
			}
			if (elementProperty.LocalName != "EncryptionProperty" || elementProperty.NamespaceURI != "http://www.w3.org/2001/04/xmlenc#")
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidEncryptionProperty"));
			}
			this.m_elemProp = elementProperty;
			this.m_cachedXml = null;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00016B17 File Offset: 0x00015B17
		public string Id
		{
			get
			{
				return this.m_id;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00016B1F File Offset: 0x00015B1F
		public string Target
		{
			get
			{
				return this.m_target;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00016B27 File Offset: 0x00015B27
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00016B30 File Offset: 0x00015B30
		public XmlElement PropertyElement
		{
			get
			{
				return this.m_elemProp;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.LocalName != "EncryptionProperty" || value.NamespaceURI != "http://www.w3.org/2001/04/xmlenc#")
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidEncryptionProperty"));
				}
				this.m_elemProp = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00016B8D File Offset: 0x00015B8D
		private bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00016B9C File Offset: 0x00015B9C
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

		// Token: 0x06000469 RID: 1129 RVA: 0x00016BCC File Offset: 0x00015BCC
		internal XmlElement GetXml(XmlDocument document)
		{
			return document.ImportNode(this.m_elemProp, true) as XmlElement;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00016BE0 File Offset: 0x00015BE0
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.LocalName != "EncryptionProperty" || value.NamespaceURI != "http://www.w3.org/2001/04/xmlenc#")
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidEncryptionProperty"));
			}
			this.m_cachedXml = value;
			this.m_id = Utils.GetAttribute(value, "Id", "http://www.w3.org/2001/04/xmlenc#");
			this.m_target = Utils.GetAttribute(value, "Target", "http://www.w3.org/2001/04/xmlenc#");
			this.m_elemProp = value;
		}

		// Token: 0x0400059D RID: 1437
		private string m_target;

		// Token: 0x0400059E RID: 1438
		private string m_id;

		// Token: 0x0400059F RID: 1439
		private XmlElement m_elemProp;

		// Token: 0x040005A0 RID: 1440
		private XmlElement m_cachedXml;
	}
}
