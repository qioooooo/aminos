using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000066 RID: 102
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class Pkcs9DocumentDescription : Pkcs9AttributeObject
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00006995 File Offset: 0x00005995
		public Pkcs9DocumentDescription()
			: base("1.3.6.1.4.1.311.88.2.2")
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000069A2 File Offset: 0x000059A2
		public Pkcs9DocumentDescription(string documentDescription)
			: base("1.3.6.1.4.1.311.88.2.2", Pkcs9DocumentDescription.Encode(documentDescription))
		{
			this.m_documentDescription = documentDescription;
			this.m_decoded = true;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000069C3 File Offset: 0x000059C3
		public Pkcs9DocumentDescription(byte[] encodedDocumentDescription)
			: base("1.3.6.1.4.1.311.88.2.2", encodedDocumentDescription)
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000069D1 File Offset: 0x000059D1
		public string DocumentDescription
		{
			get
			{
				if (!this.m_decoded && base.RawData != null)
				{
					this.Decode();
				}
				return this.m_documentDescription;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000069EF File Offset: 0x000059EF
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000069FF File Offset: 0x000059FF
		private void Decode()
		{
			this.m_documentDescription = PkcsUtils.DecodeOctetString(base.RawData);
			this.m_decoded = true;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006A19 File Offset: 0x00005A19
		private static byte[] Encode(string documentDescription)
		{
			if (string.IsNullOrEmpty(documentDescription))
			{
				throw new ArgumentNullException("documentDescription");
			}
			return PkcsUtils.EncodeOctetString(documentDescription);
		}

		// Token: 0x04000463 RID: 1123
		private string m_documentDescription;

		// Token: 0x04000464 RID: 1124
		private bool m_decoded;
	}
}
