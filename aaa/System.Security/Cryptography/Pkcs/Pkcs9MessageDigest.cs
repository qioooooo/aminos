using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000068 RID: 104
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class Pkcs9MessageDigest : Pkcs9AttributeObject
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00006AEB File Offset: 0x00005AEB
		internal Pkcs9MessageDigest(byte[] encodedMessageDigest)
			: base("1.2.840.113549.1.9.4", encodedMessageDigest)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006AF9 File Offset: 0x00005AF9
		public Pkcs9MessageDigest()
			: base("1.2.840.113549.1.9.4")
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006B06 File Offset: 0x00005B06
		public byte[] MessageDigest
		{
			get
			{
				if (!this.m_decoded && base.RawData != null)
				{
					this.Decode();
				}
				return this.m_messageDigest;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006B24 File Offset: 0x00005B24
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006B34 File Offset: 0x00005B34
		private void Decode()
		{
			this.m_messageDigest = PkcsUtils.DecodeOctetBytes(base.RawData);
			this.m_decoded = true;
		}

		// Token: 0x04000467 RID: 1127
		private byte[] m_messageDigest;

		// Token: 0x04000468 RID: 1128
		private bool m_decoded;
	}
}
