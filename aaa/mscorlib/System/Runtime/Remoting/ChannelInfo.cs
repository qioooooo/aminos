using System;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071F RID: 1823
	[Serializable]
	internal sealed class ChannelInfo : IChannelInfo
	{
		// Token: 0x060041B9 RID: 16825 RVA: 0x000E058C File Offset: 0x000DF58C
		internal ChannelInfo()
		{
			this.ChannelData = ChannelServices.CurrentChannelData;
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x000E059F File Offset: 0x000DF59F
		// (set) Token: 0x060041BB RID: 16827 RVA: 0x000E05A7 File Offset: 0x000DF5A7
		public object[] ChannelData
		{
			get
			{
				return this.channelData;
			}
			set
			{
				this.channelData = value;
			}
		}

		// Token: 0x040020C9 RID: 8393
		private object[] channelData;
	}
}
