using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000633 RID: 1587
	internal class SystemIPGlobalStatistics : IPGlobalStatistics
	{
		// Token: 0x06003105 RID: 12549 RVA: 0x000D3092 File Offset: 0x000D2092
		private SystemIPGlobalStatistics()
		{
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000D30A8 File Offset: 0x000D20A8
		internal SystemIPGlobalStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetIpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetIpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000D3107 File Offset: 0x000D2107
		public override bool ForwardingEnabled
		{
			get
			{
				return this.stats.forwardingEnabled;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000D3114 File Offset: 0x000D2114
		public override int DefaultTtl
		{
			get
			{
				return (int)this.stats.defaultTtl;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000D3121 File Offset: 0x000D2121
		public override long ReceivedPackets
		{
			get
			{
				return (long)((ulong)this.stats.packetsReceived);
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x000D312F File Offset: 0x000D212F
		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithHeaderErrors);
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000D313D File Offset: 0x000D213D
		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithAddressErrors);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x000D314B File Offset: 0x000D214B
		public override long ReceivedPacketsForwarded
		{
			get
			{
				return (long)((ulong)this.stats.packetsForwarded);
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x000D3159 File Offset: 0x000D2159
		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithUnknownProtocols);
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x000D3167 File Offset: 0x000D2167
		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDiscarded);
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x000D3175 File Offset: 0x000D2175
		public override long ReceivedPacketsDelivered
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDelivered);
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x000D3183 File Offset: 0x000D2183
		public override long OutputPacketRequests
		{
			get
			{
				return (long)((ulong)this.stats.packetOutputRequests);
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003111 RID: 12561 RVA: 0x000D3191 File Offset: 0x000D2191
		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketRoutingDiscards);
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003112 RID: 12562 RVA: 0x000D319F File Offset: 0x000D219F
		public override long OutputPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsDiscarded);
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x000D31AD File Offset: 0x000D21AD
		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsWithNoRoute);
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000D31BB File Offset: 0x000D21BB
		public override long PacketReassemblyTimeout
		{
			get
			{
				return (long)((ulong)this.stats.packetReassemblyTimeout);
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x000D31C9 File Offset: 0x000D21C9
		public override long PacketReassembliesRequired
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyRequired);
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x000D31D7 File Offset: 0x000D21D7
		public override long PacketsReassembled
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassembled);
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003117 RID: 12567 RVA: 0x000D31E5 File Offset: 0x000D21E5
		public override long PacketReassemblyFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyFailed);
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x000D31F3 File Offset: 0x000D21F3
		public override long PacketsFragmented
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmented);
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003119 RID: 12569 RVA: 0x000D3201 File Offset: 0x000D2201
		public override long PacketFragmentFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmentFailed);
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600311A RID: 12570 RVA: 0x000D320F File Offset: 0x000D220F
		public override int NumberOfInterfaces
		{
			get
			{
				return (int)this.stats.interfaces;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x000D321C File Offset: 0x000D221C
		public override int NumberOfIPAddresses
		{
			get
			{
				return (int)this.stats.ipAddresses;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x000D3229 File Offset: 0x000D2229
		public override int NumberOfRoutes
		{
			get
			{
				return (int)this.stats.routes;
			}
		}

		// Token: 0x04002E5B RID: 11867
		private MibIpStats stats = default(MibIpStats);
	}
}
