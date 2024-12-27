using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x0200039F RID: 927
	internal class CredentialHostKey
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x0006E305 File Offset: 0x0006D305
		internal CredentialHostKey(string host, int port, string authenticationType)
		{
			this.Host = host;
			this.Port = port;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0006E322 File Offset: 0x0006D322
		internal bool Match(string host, int port, string authenticationType)
		{
			return host != null && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, host, StringComparison.OrdinalIgnoreCase) == 0 && port == this.Port;
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0006E35C File Offset: 0x0006D35C
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.Host.ToUpperInvariant().GetHashCode() + this.Port.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0006E3B4 File Offset: 0x0006D3B4
		public override bool Equals(object comparand)
		{
			CredentialHostKey credentialHostKey = comparand as CredentialHostKey;
			return comparand != null && (string.Compare(this.AuthenticationType, credentialHostKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, credentialHostKey.Host, StringComparison.OrdinalIgnoreCase) == 0) && this.Port == credentialHostKey.Port;
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x0006E408 File Offset: 0x0006D408
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.Host.Length.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				this.Host,
				":",
				this.Port.ToString(NumberFormatInfo.InvariantInfo),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04001D4C RID: 7500
		internal string Host;

		// Token: 0x04001D4D RID: 7501
		internal string AuthenticationType;

		// Token: 0x04001D4E RID: 7502
		internal int Port;

		// Token: 0x04001D4F RID: 7503
		private int m_HashCode;

		// Token: 0x04001D50 RID: 7504
		private bool m_ComputedHashCode;
	}
}
