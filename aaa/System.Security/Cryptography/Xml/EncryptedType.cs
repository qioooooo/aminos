using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BA RID: 186
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public abstract class EncryptedType
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000167EE File Offset: 0x000157EE
		internal bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x000167FC File Offset: 0x000157FC
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00016804 File Offset: 0x00015804
		public virtual string Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00016814 File Offset: 0x00015814
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0001681C File Offset: 0x0001581C
		public virtual string Type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001682C File Offset: 0x0001582C
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00016834 File Offset: 0x00015834
		public virtual string MimeType
		{
			get
			{
				return this.m_mimeType;
			}
			set
			{
				this.m_mimeType = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00016844 File Offset: 0x00015844
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0001684C File Offset: 0x0001584C
		public virtual string Encoding
		{
			get
			{
				return this.m_encoding;
			}
			set
			{
				this.m_encoding = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001685C File Offset: 0x0001585C
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00016877 File Offset: 0x00015877
		public KeyInfo KeyInfo
		{
			get
			{
				if (this.m_keyInfo == null)
				{
					this.m_keyInfo = new KeyInfo();
				}
				return this.m_keyInfo;
			}
			set
			{
				this.m_keyInfo = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00016880 File Offset: 0x00015880
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00016888 File Offset: 0x00015888
		public virtual EncryptionMethod EncryptionMethod
		{
			get
			{
				return this.m_encryptionMethod;
			}
			set
			{
				this.m_encryptionMethod = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00016898 File Offset: 0x00015898
		public virtual EncryptionPropertyCollection EncryptionProperties
		{
			get
			{
				if (this.m_props == null)
				{
					this.m_props = new EncryptionPropertyCollection();
				}
				return this.m_props;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000168B3 File Offset: 0x000158B3
		public void AddProperty(EncryptionProperty ep)
		{
			this.EncryptionProperties.Add(ep);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000168C2 File Offset: 0x000158C2
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x000168DD File Offset: 0x000158DD
		public virtual CipherData CipherData
		{
			get
			{
				if (this.m_cipherData == null)
				{
					this.m_cipherData = new CipherData();
				}
				return this.m_cipherData;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_cipherData = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x06000454 RID: 1108
		public abstract void LoadXml(XmlElement value);

		// Token: 0x06000455 RID: 1109
		public abstract XmlElement GetXml();

		// Token: 0x04000591 RID: 1425
		private string m_id;

		// Token: 0x04000592 RID: 1426
		private string m_type;

		// Token: 0x04000593 RID: 1427
		private string m_mimeType;

		// Token: 0x04000594 RID: 1428
		private string m_encoding;

		// Token: 0x04000595 RID: 1429
		private EncryptionMethod m_encryptionMethod;

		// Token: 0x04000596 RID: 1430
		private CipherData m_cipherData;

		// Token: 0x04000597 RID: 1431
		private EncryptionPropertyCollection m_props;

		// Token: 0x04000598 RID: 1432
		private KeyInfo m_keyInfo;

		// Token: 0x04000599 RID: 1433
		internal XmlElement m_cachedXml;
	}
}
