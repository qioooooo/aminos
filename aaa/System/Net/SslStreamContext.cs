using System;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042E RID: 1070
	internal class SslStreamContext : TransportContext
	{
		// Token: 0x06002168 RID: 8552 RVA: 0x0008422D File Offset: 0x0008322D
		internal SslStreamContext(SslStream sslStream)
		{
			this.sslStream = sslStream;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0008423C File Offset: 0x0008323C
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.sslStream.GetChannelBinding(kind);
		}

		// Token: 0x04002186 RID: 8582
		private SslStream sslStream;
	}
}
