using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200069B RID: 1691
	internal class RegisteredChannelList
	{
		// Token: 0x06003D92 RID: 15762 RVA: 0x000D38CF File Offset: 0x000D28CF
		internal RegisteredChannelList()
		{
			this._channels = new RegisteredChannel[0];
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x000D38E3 File Offset: 0x000D28E3
		internal RegisteredChannelList(RegisteredChannel[] channels)
		{
			this._channels = channels;
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x000D38F2 File Offset: 0x000D28F2
		internal RegisteredChannel[] RegisteredChannels
		{
			get
			{
				return this._channels;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003D95 RID: 15765 RVA: 0x000D38FA File Offset: 0x000D28FA
		internal int Count
		{
			get
			{
				if (this._channels == null)
				{
					return 0;
				}
				return this._channels.Length;
			}
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x000D390E File Offset: 0x000D290E
		internal IChannel GetChannel(int index)
		{
			return this._channels[index].Channel;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x000D391D File Offset: 0x000D291D
		internal bool IsSender(int index)
		{
			return this._channels[index].IsSender();
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x000D392C File Offset: 0x000D292C
		internal bool IsReceiver(int index)
		{
			return this._channels[index].IsReceiver();
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x000D393C File Offset: 0x000D293C
		internal int ReceiverCount
		{
			get
			{
				if (this._channels == null)
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this._channels.Length; i++)
				{
					if (this.IsReceiver(i))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x000D3978 File Offset: 0x000D2978
		internal int FindChannelIndex(IChannel channel)
		{
			for (int i = 0; i < this._channels.Length; i++)
			{
				if (channel == this.GetChannel(i))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x000D39A8 File Offset: 0x000D29A8
		internal int FindChannelIndex(string name)
		{
			for (int i = 0; i < this._channels.Length; i++)
			{
				if (string.Compare(name, this.GetChannel(i).ChannelName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04001F43 RID: 8003
		private RegisteredChannel[] _channels;
	}
}
