using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063C RID: 1596
	public abstract class TcpStatistics
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003165 RID: 12645
		public abstract long ConnectionsAccepted { get; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003166 RID: 12646
		public abstract long ConnectionsInitiated { get; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003167 RID: 12647
		public abstract long CumulativeConnections { get; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003168 RID: 12648
		public abstract long CurrentConnections { get; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003169 RID: 12649
		public abstract long ErrorsReceived { get; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600316A RID: 12650
		public abstract long FailedConnectionAttempts { get; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x0600316B RID: 12651
		public abstract long MaximumConnections { get; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x0600316C RID: 12652
		public abstract long MaximumTransmissionTimeout { get; }

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x0600316D RID: 12653
		public abstract long MinimumTransmissionTimeout { get; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x0600316E RID: 12654
		public abstract long ResetConnections { get; }

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x0600316F RID: 12655
		public abstract long SegmentsReceived { get; }

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003170 RID: 12656
		public abstract long SegmentsResent { get; }

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003171 RID: 12657
		public abstract long SegmentsSent { get; }

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06003172 RID: 12658
		public abstract long ResetsSent { get; }
	}
}
