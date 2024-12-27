using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000632 RID: 1586
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039A8 RID: 14760 RVA: 0x000C26CC File Offset: 0x000C16CC
		public PublisherIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this.m_x509cert = null;
			this.m_certFile = null;
			this.m_signedFile = null;
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060039A9 RID: 14761 RVA: 0x000C26EA File Offset: 0x000C16EA
		// (set) Token: 0x060039AA RID: 14762 RVA: 0x000C26F2 File Offset: 0x000C16F2
		public string X509Certificate
		{
			get
			{
				return this.m_x509cert;
			}
			set
			{
				this.m_x509cert = value;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x000C26FB File Offset: 0x000C16FB
		// (set) Token: 0x060039AC RID: 14764 RVA: 0x000C2703 File Offset: 0x000C1703
		public string CertFile
		{
			get
			{
				return this.m_certFile;
			}
			set
			{
				this.m_certFile = value;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x000C270C File Offset: 0x000C170C
		// (set) Token: 0x060039AE RID: 14766 RVA: 0x000C2714 File Offset: 0x000C1714
		public string SignedFile
		{
			get
			{
				return this.m_signedFile;
			}
			set
			{
				this.m_signedFile = value;
			}
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x000C2720 File Offset: 0x000C1720
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new PublisherIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_x509cert != null)
			{
				return new PublisherIdentityPermission(new X509Certificate(Hex.DecodeHexString(this.m_x509cert)));
			}
			if (this.m_certFile != null)
			{
				return new PublisherIdentityPermission(global::System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(this.m_certFile));
			}
			if (this.m_signedFile != null)
			{
				return new PublisherIdentityPermission(global::System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(this.m_signedFile));
			}
			return new PublisherIdentityPermission(PermissionState.None);
		}

		// Token: 0x04001DCB RID: 7627
		private string m_x509cert;

		// Token: 0x04001DCC RID: 7628
		private string m_certFile;

		// Token: 0x04001DCD RID: 7629
		private string m_signedFile;
	}
}
