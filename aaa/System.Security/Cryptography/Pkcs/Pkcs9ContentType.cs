using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000067 RID: 103
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class Pkcs9ContentType : Pkcs9AttributeObject
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00006A34 File Offset: 0x00005A34
		internal Pkcs9ContentType(byte[] encodedContentType)
			: base("1.2.840.113549.1.9.3", encodedContentType)
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006A42 File Offset: 0x00005A42
		public Pkcs9ContentType()
			: base("1.2.840.113549.1.9.3")
		{
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006A4F File Offset: 0x00005A4F
		public Oid ContentType
		{
			get
			{
				if (!this.m_decoded && base.RawData != null)
				{
					this.Decode();
				}
				return this.m_contentType;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006A6D File Offset: 0x00005A6D
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006A80 File Offset: 0x00005A80
		private void Decode()
		{
			if (base.RawData.Length < 2 || (int)base.RawData[1] != base.RawData.Length - 2)
			{
				throw new CryptographicException(-2146885630);
			}
			if (base.RawData[0] != 6)
			{
				throw new CryptographicException(-2146881269);
			}
			this.m_contentType = new Oid(PkcsUtils.DecodeObjectIdentifier(base.RawData, 2));
			this.m_decoded = true;
		}

		// Token: 0x04000465 RID: 1125
		private Oid m_contentType;

		// Token: 0x04000466 RID: 1126
		private bool m_decoded;
	}
}
