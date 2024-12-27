using System;

namespace System.Net.Configuration
{
	// Token: 0x02000665 RID: 1637
	internal sealed class SmtpNetworkElementInternal
	{
		// Token: 0x060032AD RID: 12973 RVA: 0x000D7134 File Offset: 0x000D6134
		internal SmtpNetworkElementInternal(SmtpNetworkElement element)
		{
			this.host = element.Host;
			this.port = element.Port;
			this.clientDomain = element.ClientDomain;
			this.targetname = element.TargetName;
			if (element.DefaultCredentials)
			{
				this.credential = (NetworkCredential)CredentialCache.DefaultCredentials;
				return;
			}
			if (element.UserName != null && element.UserName.Length > 0)
			{
				this.credential = new NetworkCredential(element.UserName, element.Password);
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x000D71BD File Offset: 0x000D61BD
		internal NetworkCredential Credential
		{
			get
			{
				return this.credential;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000D71C5 File Offset: 0x000D61C5
		internal string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x000D71CD File Offset: 0x000D61CD
		internal string ClientDomain
		{
			get
			{
				return this.clientDomain;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000D71D5 File Offset: 0x000D61D5
		internal int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x000D71DD File Offset: 0x000D61DD
		internal string TargetName
		{
			get
			{
				return this.targetname;
			}
		}

		// Token: 0x04002F5C RID: 12124
		private string targetname;

		// Token: 0x04002F5D RID: 12125
		private string host;

		// Token: 0x04002F5E RID: 12126
		private string clientDomain;

		// Token: 0x04002F5F RID: 12127
		private int port;

		// Token: 0x04002F60 RID: 12128
		private NetworkCredential credential;
	}
}
