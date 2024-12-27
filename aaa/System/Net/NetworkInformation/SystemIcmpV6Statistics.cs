using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062D RID: 1581
	internal class SystemIcmpV6Statistics : IcmpV6Statistics
	{
		// Token: 0x0600309B RID: 12443 RVA: 0x000D1C2C File Offset: 0x000D0C2C
		internal SystemIcmpV6Statistics()
		{
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			uint icmpStatisticsEx = UnsafeNetInfoNativeMethods.GetIcmpStatisticsEx(out this.stats, AddressFamily.InterNetworkV6);
			if (icmpStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)icmpStatisticsEx);
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x000D1C6E File Offset: 0x000D0C6E
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwMsgs);
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x000D1C81 File Offset: 0x000D0C81
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwMsgs);
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x000D1C94 File Offset: 0x000D0C94
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwErrors);
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x000D1CA7 File Offset: 0x000D0CA7
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwErrors);
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060030A0 RID: 12448 RVA: 0x000D1CBA File Offset: 0x000D0CBA
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x000D1CD1 File Offset: 0x000D0CD1
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x000D1CE8 File Offset: 0x000D0CE8
		public override long PacketTooBigMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x000D1CFF File Offset: 0x000D0CFF
		public override long PacketTooBigMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x000D1D16 File Offset: 0x000D0D16
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000D1D2D File Offset: 0x000D0D2D
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x000D1D44 File Offset: 0x000D0D44
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x000D1D5B File Offset: 0x000D0D5B
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000D1D72 File Offset: 0x000D0D72
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000D1D8D File Offset: 0x000D0D8D
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x000D1DA8 File Offset: 0x000D0DA8
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x000D1DC3 File Offset: 0x000D0DC3
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060030AC RID: 12460 RVA: 0x000D1DDE File Offset: 0x000D0DDE
		public override long MembershipQueriesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x000D1DF9 File Offset: 0x000D0DF9
		public override long MembershipQueriesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x000D1E14 File Offset: 0x000D0E14
		public override long MembershipReportsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x000D1E2F File Offset: 0x000D0E2F
		public override long MembershipReportsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x000D1E4A File Offset: 0x000D0E4A
		public override long MembershipReductionsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x000D1E65 File Offset: 0x000D0E65
		public override long MembershipReductionsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000D1E80 File Offset: 0x000D0E80
		public override long RouterAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000D1E9B File Offset: 0x000D0E9B
		public override long RouterAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000D1EB6 File Offset: 0x000D0EB6
		public override long RouterSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000D1ED1 File Offset: 0x000D0ED1
		public override long RouterSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000D1EEC File Offset: 0x000D0EEC
		public override long NeighborAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x000D1F07 File Offset: 0x000D0F07
		public override long NeighborAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000D1F22 File Offset: 0x000D0F22
		public override long NeighborSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000D1F3D File Offset: 0x000D0F3D
		public override long NeighborSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000D1F58 File Offset: 0x000D0F58
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x000D1F73 File Offset: 0x000D0F73
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x04002E40 RID: 11840
		private MibIcmpInfoEx stats;
	}
}
