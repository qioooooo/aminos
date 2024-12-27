using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x0200031F RID: 799
	public sealed class Oid
	{
		// Token: 0x06001917 RID: 6423 RVA: 0x000557F4 File Offset: 0x000547F4
		public Oid()
		{
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x000557FC File Offset: 0x000547FC
		public Oid(string oid)
			: this(oid, OidGroup.AllGroups, true)
		{
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x00055808 File Offset: 0x00054808
		internal Oid(string oid, OidGroup group, bool lookupFriendlyName)
		{
			if (oid != null && string.Equals(oid, "1.2.840.113549.1.7.1", StringComparison.Ordinal))
			{
				this.Value = oid;
			}
			else if (lookupFriendlyName)
			{
				string text = X509Utils.FindOidInfo(2U, oid, group);
				if (text == null)
				{
					text = oid;
				}
				this.Value = text;
			}
			else
			{
				this.Value = oid;
			}
			this.m_group = group;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0005585D File Offset: 0x0005485D
		public Oid(string value, string friendlyName)
		{
			this.m_value = value;
			this.m_friendlyName = friendlyName;
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x00055873 File Offset: 0x00054873
		public Oid(Oid oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			this.m_value = oid.m_value;
			this.m_friendlyName = oid.m_friendlyName;
			this.m_group = oid.m_group;
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x000558AD File Offset: 0x000548AD
		// (set) Token: 0x0600191D RID: 6429 RVA: 0x000558B5 File Offset: 0x000548B5
		public string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x000558BE File Offset: 0x000548BE
		// (set) Token: 0x0600191F RID: 6431 RVA: 0x000558F0 File Offset: 0x000548F0
		public string FriendlyName
		{
			get
			{
				if (this.m_friendlyName == null && this.m_value != null)
				{
					this.m_friendlyName = X509Utils.FindOidInfo(1U, this.m_value, this.m_group);
				}
				return this.m_friendlyName;
			}
			set
			{
				this.m_friendlyName = value;
				if (this.m_friendlyName != null)
				{
					string text = X509Utils.FindOidInfo(2U, this.m_friendlyName, this.m_group);
					if (text != null)
					{
						this.m_value = text;
					}
				}
			}
		}

		// Token: 0x04001A78 RID: 6776
		private const string RsaDataOid = "1.2.840.113549.1.7.1";

		// Token: 0x04001A79 RID: 6777
		private string m_value;

		// Token: 0x04001A7A RID: 6778
		private string m_friendlyName;

		// Token: 0x04001A7B RID: 6779
		private OidGroup m_group;
	}
}
