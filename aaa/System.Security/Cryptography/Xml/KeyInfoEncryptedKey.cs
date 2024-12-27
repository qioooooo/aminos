using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A1 RID: 161
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class KeyInfoEncryptedKey : KeyInfoClause
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00010133 File Offset: 0x0000F133
		public KeyInfoEncryptedKey()
		{
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0001013B File Offset: 0x0000F13B
		public KeyInfoEncryptedKey(EncryptedKey encryptedKey)
		{
			this.m_encryptedKey = encryptedKey;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0001014A File Offset: 0x0000F14A
		// (set) Token: 0x06000313 RID: 787 RVA: 0x00010152 File Offset: 0x0000F152
		public EncryptedKey EncryptedKey
		{
			get
			{
				return this.m_encryptedKey;
			}
			set
			{
				this.m_encryptedKey = value;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0001015B File Offset: 0x0000F15B
		public override XmlElement GetXml()
		{
			if (this.m_encryptedKey == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "KeyInfoEncryptedKey");
			}
			return this.m_encryptedKey.GetXml();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00010185 File Offset: 0x0000F185
		internal override XmlElement GetXml(XmlDocument xmlDocument)
		{
			if (this.m_encryptedKey == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "KeyInfoEncryptedKey");
			}
			return this.m_encryptedKey.GetXml(xmlDocument);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000101B0 File Offset: 0x0000F1B0
		public override void LoadXml(XmlElement value)
		{
			this.m_encryptedKey = new EncryptedKey();
			this.m_encryptedKey.LoadXml(value);
		}

		// Token: 0x040004FB RID: 1275
		private EncryptedKey m_encryptedKey;
	}
}
