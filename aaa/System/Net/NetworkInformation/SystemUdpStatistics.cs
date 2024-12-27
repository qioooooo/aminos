using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063F RID: 1599
	internal class SystemUdpStatistics : UdpStatistics
	{
		// Token: 0x0600318A RID: 12682 RVA: 0x000D446B File Offset: 0x000D346B
		private SystemUdpStatistics()
		{
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000D4474 File Offset: 0x000D3474
		internal SystemUdpStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetUdpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetUdpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600318C RID: 12684 RVA: 0x000D44C7 File Offset: 0x000D34C7
		public override long DatagramsReceived
		{
			get
			{
				return (long)((ulong)this.stats.datagramsReceived);
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x000D44D5 File Offset: 0x000D34D5
		public override long IncomingDatagramsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsDiscarded);
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x000D44E3 File Offset: 0x000D34E3
		public override long IncomingDatagramsWithErrors
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsWithErrors);
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x000D44F1 File Offset: 0x000D34F1
		public override long DatagramsSent
		{
			get
			{
				return (long)((ulong)this.stats.datagramsSent);
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000D44FF File Offset: 0x000D34FF
		public override int UdpListeners
		{
			get
			{
				return (int)this.stats.udpListeners;
			}
		}

		// Token: 0x04002E83 RID: 11907
		private MibUdpStats stats;
	}
}
