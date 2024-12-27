using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000054 RID: 84
	internal class IpcClientTransportSinkProvider : IClientChannelSinkProvider
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000D3B7 File Offset: 0x0000C3B7
		internal IpcClientTransportSinkProvider(IDictionary properties)
		{
			this._prop = properties;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000D3C8 File Offset: 0x0000C3C8
		public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
		{
			IpcClientTransportSink ipcClientTransportSink = new IpcClientTransportSink(url, (IpcClientChannel)channel);
			if (this._prop != null)
			{
				foreach (object obj in this._prop.Keys)
				{
					ipcClientTransportSink[obj] = this._prop[obj];
				}
			}
			return ipcClientTransportSink;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000D444 File Offset: 0x0000C444
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000D447 File Offset: 0x0000C447
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

		// Token: 0x040001EA RID: 490
		private IDictionary _prop;
	}
}
