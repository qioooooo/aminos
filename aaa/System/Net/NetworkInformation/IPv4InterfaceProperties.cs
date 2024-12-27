using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005EB RID: 1515
	public abstract class IPv4InterfaceProperties
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002FC1 RID: 12225
		public abstract bool UsesWins { get; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002FC2 RID: 12226
		public abstract bool IsDhcpEnabled { get; }

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002FC3 RID: 12227
		public abstract bool IsAutomaticPrivateAddressingActive { get; }

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002FC4 RID: 12228
		public abstract bool IsAutomaticPrivateAddressingEnabled { get; }

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002FC5 RID: 12229
		public abstract int Index { get; }

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002FC6 RID: 12230
		public abstract bool IsForwardingEnabled { get; }

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002FC7 RID: 12231
		public abstract int Mtu { get; }
	}
}
