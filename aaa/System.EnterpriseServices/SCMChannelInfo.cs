using System;
using System.Runtime.Remoting;

namespace System.EnterpriseServices
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	internal class SCMChannelInfo : IChannelInfo
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005502 File Offset: 0x00004502
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000550A File Offset: 0x0000450A
		public virtual object[] ChannelData
		{
			get
			{
				return new object[0];
			}
			set
			{
			}
		}
	}
}
