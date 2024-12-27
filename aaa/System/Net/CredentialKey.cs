using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020003A0 RID: 928
	internal class CredentialKey
	{
		// Token: 0x06001CEF RID: 7407 RVA: 0x0006E487 File Offset: 0x0006D487
		internal CredentialKey(Uri uriPrefix, string authenticationType)
		{
			this.UriPrefix = uriPrefix;
			this.UriPrefixLength = this.UriPrefix.ToString().Length;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x0006E4BA File Offset: 0x0006D4BA
		internal bool Match(Uri uri, string authenticationType)
		{
			return !(uri == null) && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.IsPrefix(uri, this.UriPrefix);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x0006E4E8 File Offset: 0x0006D4E8
		internal bool IsPrefix(Uri uri, Uri prefixUri)
		{
			if (prefixUri.Scheme != uri.Scheme || prefixUri.Host != uri.Host || prefixUri.Port != uri.Port)
			{
				return false;
			}
			int num = prefixUri.AbsolutePath.LastIndexOf('/');
			return num <= uri.AbsolutePath.LastIndexOf('/') && string.Compare(uri.AbsolutePath, 0, prefixUri.AbsolutePath, 0, num, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x0006E563 File Offset: 0x0006D563
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.UriPrefixLength + this.UriPrefix.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x0006E5A4 File Offset: 0x0006D5A4
		public override bool Equals(object comparand)
		{
			CredentialKey credentialKey = comparand as CredentialKey;
			return comparand != null && string.Compare(this.AuthenticationType, credentialKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.UriPrefix.Equals(credentialKey.UriPrefix);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0006E5E8 File Offset: 0x0006D5E8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.UriPrefixLength.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				ValidationHelper.ToString(this.UriPrefix),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04001D51 RID: 7505
		internal Uri UriPrefix;

		// Token: 0x04001D52 RID: 7506
		internal int UriPrefixLength = -1;

		// Token: 0x04001D53 RID: 7507
		internal string AuthenticationType;

		// Token: 0x04001D54 RID: 7508
		private int m_HashCode;

		// Token: 0x04001D55 RID: 7509
		private bool m_ComputedHashCode;
	}
}
