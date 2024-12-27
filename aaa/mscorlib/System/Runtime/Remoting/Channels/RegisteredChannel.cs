using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200069A RID: 1690
	internal class RegisteredChannel
	{
		// Token: 0x06003D8E RID: 15758 RVA: 0x000D3858 File Offset: 0x000D2858
		internal RegisteredChannel(IChannel chnl)
		{
			this.channel = chnl;
			this.flags = 0;
			if (chnl is IChannelSender)
			{
				this.flags |= 1;
			}
			if (chnl is IChannelReceiver)
			{
				this.flags |= 2;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x000D38A7 File Offset: 0x000D28A7
		internal virtual IChannel Channel
		{
			get
			{
				return this.channel;
			}
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x000D38AF File Offset: 0x000D28AF
		internal virtual bool IsSender()
		{
			return (this.flags & 1) != 0;
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x000D38BF File Offset: 0x000D28BF
		internal virtual bool IsReceiver()
		{
			return (this.flags & 2) != 0;
		}

		// Token: 0x04001F3F RID: 7999
		private const byte SENDER = 1;

		// Token: 0x04001F40 RID: 8000
		private const byte RECEIVER = 2;

		// Token: 0x04001F41 RID: 8001
		private IChannel channel;

		// Token: 0x04001F42 RID: 8002
		private byte flags;
	}
}
