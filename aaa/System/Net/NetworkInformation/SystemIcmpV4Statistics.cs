using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062B RID: 1579
	internal class SystemIcmpV4Statistics : IcmpV4Statistics
	{
		// Token: 0x06003080 RID: 12416 RVA: 0x000D1A14 File Offset: 0x000D0A14
		internal SystemIcmpV4Statistics()
		{
			uint icmpStatistics = UnsafeNetInfoNativeMethods.GetIcmpStatistics(out this.stats);
			if (icmpStatistics != 0U)
			{
				throw new NetworkInformationException((int)icmpStatistics);
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000D1A3D File Offset: 0x000D0A3D
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.messages);
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000D1A50 File Offset: 0x000D0A50
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.messages);
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000D1A63 File Offset: 0x000D0A63
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.errors);
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000D1A76 File Offset: 0x000D0A76
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.errors);
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x000D1A89 File Offset: 0x000D0A89
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.destinationUnreachables);
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000D1A9C File Offset: 0x000D0A9C
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.destinationUnreachables);
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x000D1AAF File Offset: 0x000D0AAF
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timeExceeds);
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000D1AC2 File Offset: 0x000D0AC2
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timeExceeds);
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000D1AD5 File Offset: 0x000D0AD5
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.parameterProblems);
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000D1AE8 File Offset: 0x000D0AE8
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.parameterProblems);
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x000D1AFB File Offset: 0x000D0AFB
		public override long SourceQuenchesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.sourceQuenches);
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000D1B0E File Offset: 0x000D0B0E
		public override long SourceQuenchesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.sourceQuenches);
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000D1B21 File Offset: 0x000D0B21
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.redirects);
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000D1B34 File Offset: 0x000D0B34
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.redirects);
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000D1B47 File Offset: 0x000D0B47
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoRequests);
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000D1B5A File Offset: 0x000D0B5A
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoRequests);
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000D1B6D File Offset: 0x000D0B6D
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoReplies);
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000D1B80 File Offset: 0x000D0B80
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoReplies);
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000D1B93 File Offset: 0x000D0B93
		public override long TimestampRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampRequests);
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x000D1BA6 File Offset: 0x000D0BA6
		public override long TimestampRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampRequests);
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x000D1BB9 File Offset: 0x000D0BB9
		public override long TimestampRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampReplies);
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003096 RID: 12438 RVA: 0x000D1BCC File Offset: 0x000D0BCC
		public override long TimestampRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampReplies);
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x000D1BDF File Offset: 0x000D0BDF
		public override long AddressMaskRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskRequests);
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x000D1BF2 File Offset: 0x000D0BF2
		public override long AddressMaskRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskRequests);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000D1C05 File Offset: 0x000D0C05
		public override long AddressMaskRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskReplies);
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x000D1C18 File Offset: 0x000D0C18
		public override long AddressMaskRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskReplies);
			}
		}

		// Token: 0x04002E30 RID: 11824
		private MibIcmpInfo stats;
	}
}
