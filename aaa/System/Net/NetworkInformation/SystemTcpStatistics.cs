using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063D RID: 1597
	internal class SystemTcpStatistics : TcpStatistics
	{
		// Token: 0x06003174 RID: 12660 RVA: 0x000D4344 File Offset: 0x000D3344
		private SystemTcpStatistics()
		{
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000D434C File Offset: 0x000D334C
		internal SystemTcpStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetTcpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetTcpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000D439F File Offset: 0x000D339F
		public override long MinimumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.minimumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x000D43AD File Offset: 0x000D33AD
		public override long MaximumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.maximumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000D43BB File Offset: 0x000D33BB
		public override long MaximumConnections
		{
			get
			{
				return (long)((ulong)this.stats.maximumConnections);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000D43C9 File Offset: 0x000D33C9
		public override long ConnectionsInitiated
		{
			get
			{
				return (long)((ulong)this.stats.activeOpens);
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x000D43D7 File Offset: 0x000D33D7
		public override long ConnectionsAccepted
		{
			get
			{
				return (long)((ulong)this.stats.passiveOpens);
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x000D43E5 File Offset: 0x000D33E5
		public override long FailedConnectionAttempts
		{
			get
			{
				return (long)((ulong)this.stats.failedConnectionAttempts);
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x000D43F3 File Offset: 0x000D33F3
		public override long ResetConnections
		{
			get
			{
				return (long)((ulong)this.stats.resetConnections);
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x000D4401 File Offset: 0x000D3401
		public override long CurrentConnections
		{
			get
			{
				return (long)((ulong)this.stats.currentConnections);
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x000D440F File Offset: 0x000D340F
		public override long SegmentsReceived
		{
			get
			{
				return (long)((ulong)this.stats.segmentsReceived);
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x000D441D File Offset: 0x000D341D
		public override long SegmentsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSent);
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000D442B File Offset: 0x000D342B
		public override long SegmentsResent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsResent);
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000D4439 File Offset: 0x000D3439
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.errorsReceived);
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000D4447 File Offset: 0x000D3447
		public override long ResetsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSentWithReset);
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000D4455 File Offset: 0x000D3455
		public override long CumulativeConnections
		{
			get
			{
				return (long)((ulong)this.stats.cumulativeConnections);
			}
		}

		// Token: 0x04002E82 RID: 11906
		private MibTcpStats stats;
	}
}
