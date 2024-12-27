using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042F RID: 1071
	internal class HttpListenerRequestContext : TransportContext
	{
		// Token: 0x0600216A RID: 8554 RVA: 0x0008424A File Offset: 0x0008324A
		internal HttpListenerRequestContext(HttpListenerRequest request)
		{
			this.request = request;
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0008425C File Offset: 0x0008325C
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				throw new NotSupportedException(SR.GetString("net_listener_invalid_cbt_type", new object[] { kind.ToString() }));
			}
			return this.request.GetChannelBinding();
		}

		// Token: 0x04002187 RID: 8583
		private HttpListenerRequest request;
	}
}
