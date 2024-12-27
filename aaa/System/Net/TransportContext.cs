using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042C RID: 1068
	public abstract class TransportContext
	{
		// Token: 0x06002164 RID: 8548
		public abstract ChannelBinding GetChannelBinding(ChannelBindingKind kind);
	}
}
