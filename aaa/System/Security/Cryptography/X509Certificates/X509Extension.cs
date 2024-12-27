using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033A RID: 826
	public class X509Extension : AsnEncodedData
	{
		// Token: 0x06001A07 RID: 6663 RVA: 0x0005A8B0 File Offset: 0x000598B0
		internal X509Extension(string oid)
			: base(new Oid(oid, OidGroup.ExtensionOrAttribute, false))
		{
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0005A8C0 File Offset: 0x000598C0
		internal X509Extension(IntPtr pExtension)
		{
			CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(pExtension, typeof(CAPIBase.CERT_EXTENSION));
			this.m_critical = cert_EXTENSION.fCritical;
			string pszObjId = cert_EXTENSION.pszObjId;
			this.m_oid = new Oid(pszObjId, OidGroup.ExtensionOrAttribute, false);
			byte[] array = new byte[cert_EXTENSION.Value.cbData];
			if (cert_EXTENSION.Value.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cert_EXTENSION.Value.pbData, array, 0, array.Length);
			}
			this.m_rawData = array;
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0005A954 File Offset: 0x00059954
		protected X509Extension()
		{
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0005A95C File Offset: 0x0005995C
		public X509Extension(string oid, byte[] rawData, bool critical)
			: this(new Oid(oid, OidGroup.ExtensionOrAttribute, true), rawData, critical)
		{
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0005A96E File Offset: 0x0005996E
		public X509Extension(AsnEncodedData encodedExtension, bool critical)
			: this(encodedExtension.Oid, encodedExtension.RawData, critical)
		{
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0005A984 File Offset: 0x00059984
		public X509Extension(Oid oid, byte[] rawData, bool critical)
			: base(oid, rawData)
		{
			if (base.Oid == null || base.Oid.Value == null)
			{
				throw new ArgumentNullException("oid");
			}
			if (base.Oid.Value.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Arg_EmptyOrNullString"), "oid.Value");
			}
			this.m_critical = critical;
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x0005A9E7 File Offset: 0x000599E7
		// (set) Token: 0x06001A0E RID: 6670 RVA: 0x0005A9EF File Offset: 0x000599EF
		public bool Critical
		{
			get
			{
				return this.m_critical;
			}
			set
			{
				this.m_critical = value;
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0005A9F8 File Offset: 0x000599F8
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(SR.GetString("Cryptography_X509_ExtensionMismatch"));
			}
			base.CopyFrom(asnEncodedData);
			this.m_critical = x509Extension.Critical;
		}

		// Token: 0x04001B07 RID: 6919
		private bool m_critical;
	}
}
