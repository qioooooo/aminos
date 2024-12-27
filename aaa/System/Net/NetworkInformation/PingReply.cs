using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000628 RID: 1576
	public class PingReply
	{
		// Token: 0x06003075 RID: 12405 RVA: 0x000D178F File Offset: 0x000D078F
		internal PingReply()
		{
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000D1797 File Offset: 0x000D0797
		internal PingReply(IPStatus ipStatus)
		{
			this.ipStatus = ipStatus;
			this.buffer = new byte[0];
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000D17B4 File Offset: 0x000D07B4
		internal PingReply(byte[] data, int dataLength, IPAddress address, int time)
		{
			this.address = address;
			this.rtt = (long)time;
			this.ipStatus = this.GetIPStatus((IcmpV4Type)data[20], (IcmpV4Code)data[21]);
			if (this.ipStatus == IPStatus.Success)
			{
				this.buffer = new byte[dataLength - 28];
				Array.Copy(data, 28, this.buffer, 0, dataLength - 28);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000D1824 File Offset: 0x000D0824
		internal PingReply(IcmpEchoReply reply)
		{
			this.address = new IPAddress((long)((ulong)reply.address));
			this.ipStatus = (IPStatus)reply.status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.roundTripTime);
				this.buffer = new byte[(int)reply.dataSize];
				Marshal.Copy(reply.data, this.buffer, 0, (int)reply.dataSize);
				this.options = new PingOptions(reply.options);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000D18B8 File Offset: 0x000D08B8
		internal PingReply(Icmp6EchoReply reply, IntPtr dataPtr, int sendSize)
		{
			this.address = new IPAddress(reply.Address.Address, (long)((ulong)reply.Address.ScopeID));
			this.ipStatus = (IPStatus)reply.Status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.RoundTripTime);
				this.buffer = new byte[sendSize];
				Marshal.Copy(IntPtrHelper.Add(dataPtr, 36), this.buffer, 0, sendSize);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000D1940 File Offset: 0x000D0940
		private IPStatus GetIPStatus(IcmpV4Type type, IcmpV4Code code)
		{
			switch (type)
			{
			case IcmpV4Type.ICMP4_ECHO_REPLY:
				return IPStatus.Success;
			case (IcmpV4Type)1:
			case (IcmpV4Type)2:
				break;
			case IcmpV4Type.ICMP4_DST_UNREACH:
				switch (code)
				{
				case IcmpV4Code.ICMP4_UNREACH_NET:
					return IPStatus.DestinationNetworkUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_HOST:
					return IPStatus.DestinationHostUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PROTOCOL:
					return IPStatus.DestinationProtocolUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PORT:
					return IPStatus.DestinationPortUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_FRAG_NEEDED:
					return IPStatus.PacketTooBig;
				default:
					return IPStatus.DestinationUnreachable;
				}
				break;
			case IcmpV4Type.ICMP4_SOURCE_QUENCH:
				return IPStatus.SourceQuench;
			default:
				switch (type)
				{
				case IcmpV4Type.ICMP4_TIME_EXCEEDED:
					return IPStatus.TtlExpired;
				case IcmpV4Type.ICMP4_PARAM_PROB:
					return IPStatus.ParameterProblem;
				}
				break;
			}
			return IPStatus.Unknown;
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x000D19D3 File Offset: 0x000D09D3
		public IPStatus Status
		{
			get
			{
				return this.ipStatus;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000D19DB File Offset: 0x000D09DB
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x000D19E3 File Offset: 0x000D09E3
		public long RoundtripTime
		{
			get
			{
				return this.rtt;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x000D19EB File Offset: 0x000D09EB
		public PingOptions Options
		{
			get
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				return this.options;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x000D1A0A File Offset: 0x000D0A0A
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x04002E1E RID: 11806
		private IPAddress address;

		// Token: 0x04002E1F RID: 11807
		private PingOptions options;

		// Token: 0x04002E20 RID: 11808
		private IPStatus ipStatus;

		// Token: 0x04002E21 RID: 11809
		private long rtt;

		// Token: 0x04002E22 RID: 11810
		private byte[] buffer;
	}
}
