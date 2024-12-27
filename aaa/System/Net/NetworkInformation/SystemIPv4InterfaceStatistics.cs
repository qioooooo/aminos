using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000632 RID: 1586
	internal class SystemIPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		// Token: 0x060030F3 RID: 12531 RVA: 0x000D2F20 File Offset: 0x000D1F20
		private SystemIPv4InterfaceStatistics()
		{
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000D2F34 File Offset: 0x000D1F34
		internal SystemIPv4InterfaceStatistics(long index)
		{
			this.GetIfEntry(index);
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x000D2F4F File Offset: 0x000D1F4F
		public override long OutputQueueLength
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutQLen);
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000D2F5D File Offset: 0x000D1F5D
		public override long BytesSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutOctets);
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000D2F6B File Offset: 0x000D1F6B
		public override long BytesReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInOctets);
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000D2F79 File Offset: 0x000D1F79
		public override long UnicastPacketsSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutUcastPkts);
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000D2F87 File Offset: 0x000D1F87
		public override long UnicastPacketsReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInUcastPkts);
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x000D2F95 File Offset: 0x000D1F95
		public override long NonUnicastPacketsSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutNUcastPkts);
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x000D2FA3 File Offset: 0x000D1FA3
		public override long NonUnicastPacketsReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInNUcastPkts);
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x000D2FB1 File Offset: 0x000D1FB1
		public override long IncomingPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInDiscards);
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x000D2FBF File Offset: 0x000D1FBF
		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutDiscards);
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x000D2FCD File Offset: 0x000D1FCD
		public override long IncomingPacketsWithErrors
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInErrors);
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x000D2FDB File Offset: 0x000D1FDB
		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutErrors);
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000D2FE9 File Offset: 0x000D1FE9
		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInUnknownProtos);
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000D2FF7 File Offset: 0x000D1FF7
		internal long Mtu
		{
			get
			{
				return (long)((ulong)this.ifRow.dwMtu);
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000D3008 File Offset: 0x000D2008
		internal OperationalStatus OperationalStatus
		{
			get
			{
				switch (this.ifRow.operStatus)
				{
				case OldOperationalStatus.NonOperational:
					return OperationalStatus.Down;
				case OldOperationalStatus.Unreachable:
					return OperationalStatus.Down;
				case OldOperationalStatus.Disconnected:
					return OperationalStatus.Dormant;
				case OldOperationalStatus.Connecting:
					return OperationalStatus.Dormant;
				case OldOperationalStatus.Connected:
					return OperationalStatus.Up;
				case OldOperationalStatus.Operational:
					return OperationalStatus.Up;
				default:
					return OperationalStatus.Unknown;
				}
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x000D304E File Offset: 0x000D204E
		internal long Speed
		{
			get
			{
				return (long)((ulong)this.ifRow.dwSpeed);
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x000D305C File Offset: 0x000D205C
		private void GetIfEntry(long index)
		{
			if (index == 0L)
			{
				return;
			}
			this.ifRow.dwIndex = (uint)index;
			uint ifEntry = UnsafeNetInfoNativeMethods.GetIfEntry(ref this.ifRow);
			if (ifEntry != 0U)
			{
				throw new NetworkInformationException((int)ifEntry);
			}
		}

		// Token: 0x04002E5A RID: 11866
		private MibIfRow ifRow = default(MibIfRow);
	}
}
