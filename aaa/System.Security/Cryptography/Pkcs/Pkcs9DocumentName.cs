using System;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000065 RID: 101
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class Pkcs9DocumentName : Pkcs9AttributeObject
	{
		// Token: 0x0600011B RID: 283 RVA: 0x000068F6 File Offset: 0x000058F6
		public Pkcs9DocumentName()
			: base("1.3.6.1.4.1.311.88.2.1")
		{
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006903 File Offset: 0x00005903
		public Pkcs9DocumentName(string documentName)
			: base("1.3.6.1.4.1.311.88.2.1", Pkcs9DocumentName.Encode(documentName))
		{
			this.m_documentName = documentName;
			this.m_decoded = true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006924 File Offset: 0x00005924
		public Pkcs9DocumentName(byte[] encodedDocumentName)
			: base("1.3.6.1.4.1.311.88.2.1", encodedDocumentName)
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006932 File Offset: 0x00005932
		public string DocumentName
		{
			get
			{
				if (!this.m_decoded && base.RawData != null)
				{
					this.Decode();
				}
				return this.m_documentName;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006950 File Offset: 0x00005950
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006960 File Offset: 0x00005960
		private void Decode()
		{
			this.m_documentName = PkcsUtils.DecodeOctetString(base.RawData);
			this.m_decoded = true;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000697A File Offset: 0x0000597A
		private static byte[] Encode(string documentName)
		{
			if (string.IsNullOrEmpty(documentName))
			{
				throw new ArgumentNullException("documentName");
			}
			return PkcsUtils.EncodeOctetString(documentName);
		}

		// Token: 0x04000461 RID: 1121
		private string m_documentName;

		// Token: 0x04000462 RID: 1122
		private bool m_decoded;
	}
}
