using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BF RID: 1727
	internal class DispatchChannelSinkProvider : IServerChannelSinkProvider
	{
		// Token: 0x06003EB3 RID: 16051 RVA: 0x000D7BCA File Offset: 0x000D6BCA
		internal DispatchChannelSinkProvider()
		{
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x000D7BD2 File Offset: 0x000D6BD2
		public void GetChannelData(IChannelDataStore channelData)
		{
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x000D7BD4 File Offset: 0x000D6BD4
		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			return new DispatchChannelSink();
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x000D7BDB File Offset: 0x000D6BDB
		// (set) Token: 0x06003EB7 RID: 16055 RVA: 0x000D7BDE File Offset: 0x000D6BDE
		public IServerChannelSinkProvider Next
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
	}
}
