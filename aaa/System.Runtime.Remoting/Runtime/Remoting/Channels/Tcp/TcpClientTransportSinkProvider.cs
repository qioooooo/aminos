using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x0200003F RID: 63
	internal class TcpClientTransportSinkProvider : IClientChannelSinkProvider
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000A35B File Offset: 0x0000935B
		internal TcpClientTransportSinkProvider(IDictionary properties)
		{
			this._prop = properties;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A36C File Offset: 0x0000936C
		public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
		{
			TcpClientTransportSink tcpClientTransportSink = new TcpClientTransportSink(url, (TcpClientChannel)channel);
			if (this._prop != null)
			{
				foreach (object obj in this._prop.Keys)
				{
					tcpClientTransportSink[obj] = this._prop[obj];
				}
			}
			return tcpClientTransportSink;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000A3E8 File Offset: 0x000093E8
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000A3EB File Offset: 0x000093EB
		public IClientChannelSinkProvider Next
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0400016A RID: 362
		private IDictionary _prop;
	}
}
