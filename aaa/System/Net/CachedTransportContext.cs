using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000430 RID: 1072
	internal class CachedTransportContext : TransportContext
	{
		// Token: 0x0600216C RID: 8556 RVA: 0x0008429F File Offset: 0x0008329F
		internal CachedTransportContext(ChannelBinding binding)
		{
			this.binding = binding;
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000842AE File Offset: 0x000832AE
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				return null;
			}
			return this.binding;
		}

		// Token: 0x04002188 RID: 8584
		private ChannelBinding binding;
	}
}
