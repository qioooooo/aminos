using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063E RID: 1598
	public abstract class UdpStatistics
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003184 RID: 12676
		public abstract long DatagramsReceived { get; }

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06003185 RID: 12677
		public abstract long DatagramsSent { get; }

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06003186 RID: 12678
		public abstract long IncomingDatagramsDiscarded { get; }

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003187 RID: 12679
		public abstract long IncomingDatagramsWithErrors { get; }

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003188 RID: 12680
		public abstract int UdpListeners { get; }
	}
}
