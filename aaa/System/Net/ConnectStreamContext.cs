using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042D RID: 1069
	internal class ConnectStreamContext : TransportContext
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x00084210 File Offset: 0x00083210
		internal ConnectStreamContext(ConnectStream connectStream)
		{
			this.connectStream = connectStream;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0008421F File Offset: 0x0008321F
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.connectStream.GetChannelBinding(kind);
		}

		// Token: 0x04002185 RID: 8581
		private ConnectStream connectStream;
	}
}
