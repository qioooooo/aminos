using System;
using System.Globalization;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000339 RID: 825
	public sealed class X509ChainPolicy
	{
		// Token: 0x060019F8 RID: 6648 RVA: 0x0005A717 File Offset: 0x00059717
		public X509ChainPolicy()
		{
			this.Reset();
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0005A725 File Offset: 0x00059725
		public OidCollection ApplicationPolicy
		{
			get
			{
				return this.m_applicationPolicy;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0005A72D File Offset: 0x0005972D
		public OidCollection CertificatePolicy
		{
			get
			{
				return this.m_certificatePolicy;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x0005A735 File Offset: 0x00059735
		// (set) Token: 0x060019FC RID: 6652 RVA: 0x0005A740 File Offset: 0x00059740
		public X509RevocationMode RevocationMode
		{
			get
			{
				return this.m_revocationMode;
			}
			set
			{
				if (value < X509RevocationMode.NoCheck || value > X509RevocationMode.Offline)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_revocationMode = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0005A786 File Offset: 0x00059786
		// (set) Token: 0x060019FE RID: 6654 RVA: 0x0005A790 File Offset: 0x00059790
		public X509RevocationFlag RevocationFlag
		{
			get
			{
				return this.m_revocationFlag;
			}
			set
			{
				if (value < X509RevocationFlag.EndCertificateOnly || value > X509RevocationFlag.ExcludeRoot)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_revocationFlag = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0005A7D6 File Offset: 0x000597D6
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x0005A7E0 File Offset: 0x000597E0
		public X509VerificationFlags VerificationFlags
		{
			get
			{
				return this.m_verificationFlags;
			}
			set
			{
				if (value < X509VerificationFlags.NoFlag || value > X509VerificationFlags.AllFlags)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_verificationFlags = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0005A82A File Offset: 0x0005982A
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x0005A832 File Offset: 0x00059832
		public DateTime VerificationTime
		{
			get
			{
				return this.m_verificationTime;
			}
			set
			{
				this.m_verificationTime = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0005A83B File Offset: 0x0005983B
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0005A843 File Offset: 0x00059843
		public TimeSpan UrlRetrievalTimeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_timeout = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0005A84C File Offset: 0x0005984C
		public X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_extraStore;
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005A854 File Offset: 0x00059854
		public void Reset()
		{
			this.m_applicationPolicy = new OidCollection();
			this.m_certificatePolicy = new OidCollection();
			this.m_revocationMode = X509RevocationMode.Online;
			this.m_revocationFlag = X509RevocationFlag.ExcludeRoot;
			this.m_verificationFlags = X509VerificationFlags.NoFlag;
			this.m_verificationTime = DateTime.Now;
			this.m_timeout = new TimeSpan(0, 0, 0);
			this.m_extraStore = new X509Certificate2Collection();
		}

		// Token: 0x04001AFF RID: 6911
		private OidCollection m_applicationPolicy;

		// Token: 0x04001B00 RID: 6912
		private OidCollection m_certificatePolicy;

		// Token: 0x04001B01 RID: 6913
		private X509RevocationMode m_revocationMode;

		// Token: 0x04001B02 RID: 6914
		private X509RevocationFlag m_revocationFlag;

		// Token: 0x04001B03 RID: 6915
		private DateTime m_verificationTime;

		// Token: 0x04001B04 RID: 6916
		private TimeSpan m_timeout;

		// Token: 0x04001B05 RID: 6917
		private X509Certificate2Collection m_extraStore;

		// Token: 0x04001B06 RID: 6918
		private X509VerificationFlags m_verificationFlags;
	}
}
