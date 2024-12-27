using System;
using System.Globalization;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x02000424 RID: 1060
	[Serializable]
	public class IPEndPoint : EndPoint
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x00082511 File Offset: 0x00081511
		public override AddressFamily AddressFamily
		{
			get
			{
				return this.m_Address.AddressFamily;
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0008251E File Offset: 0x0008151E
		public IPEndPoint(long address, int port)
		{
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = new IPAddress(address);
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0008254C File Offset: 0x0008154C
		public IPEndPoint(IPAddress address, int port)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.m_Port = port;
			this.m_Address = address;
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x00082583 File Offset: 0x00081583
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x0008258B File Offset: 0x0008158B
		public IPAddress Address
		{
			get
			{
				return this.m_Address;
			}
			set
			{
				this.m_Address = value;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x00082594 File Offset: 0x00081594
		// (set) Token: 0x0600210F RID: 8463 RVA: 0x0008259C File Offset: 0x0008159C
		public int Port
		{
			get
			{
				return this.m_Port;
			}
			set
			{
				if (!ValidationHelper.ValidateTcpPort(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_Port = value;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000825B8 File Offset: 0x000815B8
		public override string ToString()
		{
			return this.Address.ToString() + ":" + this.Port.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000825F0 File Offset: 0x000815F0
		public override SocketAddress Serialize()
		{
			if (this.m_Address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				SocketAddress socketAddress = new SocketAddress(this.AddressFamily, 28);
				int port = this.Port;
				socketAddress[2] = (byte)(port >> 8);
				socketAddress[3] = (byte)port;
				socketAddress[4] = 0;
				socketAddress[5] = 0;
				socketAddress[6] = 0;
				socketAddress[7] = 0;
				long scopeId = this.Address.ScopeId;
				socketAddress[24] = (byte)scopeId;
				socketAddress[25] = (byte)(scopeId >> 8);
				socketAddress[26] = (byte)(scopeId >> 16);
				socketAddress[27] = (byte)(scopeId >> 24);
				byte[] addressBytes = this.Address.GetAddressBytes();
				for (int i = 0; i < addressBytes.Length; i++)
				{
					socketAddress[8 + i] = addressBytes[i];
				}
				return socketAddress;
			}
			SocketAddress socketAddress2 = new SocketAddress(this.m_Address.AddressFamily, 16);
			socketAddress2[2] = (byte)(this.Port >> 8);
			socketAddress2[3] = (byte)this.Port;
			socketAddress2[4] = (byte)this.Address.m_Address;
			socketAddress2[5] = (byte)(this.Address.m_Address >> 8);
			socketAddress2[6] = (byte)(this.Address.m_Address >> 16);
			socketAddress2[7] = (byte)(this.Address.m_Address >> 24);
			return socketAddress2;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00082750 File Offset: 0x00081750
		public override EndPoint Create(SocketAddress socketAddress)
		{
			if (socketAddress.Family != this.AddressFamily)
			{
				throw new ArgumentException(SR.GetString("net_InvalidAddressFamily", new object[]
				{
					socketAddress.Family.ToString(),
					base.GetType().FullName,
					this.AddressFamily.ToString()
				}), "socketAddress");
			}
			if (socketAddress.Size < 8)
			{
				throw new ArgumentException(SR.GetString("net_InvalidSocketAddressSize", new object[]
				{
					socketAddress.GetType().FullName,
					base.GetType().FullName
				}), "socketAddress");
			}
			if (this.AddressFamily == AddressFamily.InterNetworkV6)
			{
				byte[] array = new byte[16];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = socketAddress[i + 8];
				}
				int num = (((int)socketAddress[2] << 8) & 65280) | (int)socketAddress[3];
				long num2 = (long)(((int)socketAddress[27] << 24) + ((int)socketAddress[26] << 16) + ((int)socketAddress[25] << 8) + (int)socketAddress[24]);
				return new IPEndPoint(new IPAddress(array, num2), num);
			}
			int num3 = (((int)socketAddress[2] << 8) & 65280) | (int)socketAddress[3];
			long num4 = (long)((int)(socketAddress[4] & byte.MaxValue) | (((int)socketAddress[5] << 8) & 65280) | (((int)socketAddress[6] << 16) & 16711680) | ((int)socketAddress[7] << 24)) & (long)((ulong)(-1));
			return new IPEndPoint(num4, num3);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000828EF File Offset: 0x000818EF
		public override bool Equals(object comparand)
		{
			return comparand is IPEndPoint && ((IPEndPoint)comparand).m_Address.Equals(this.m_Address) && ((IPEndPoint)comparand).m_Port == this.m_Port;
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00082928 File Offset: 0x00081928
		public override int GetHashCode()
		{
			return this.m_Address.GetHashCode() ^ this.m_Port;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0008293C File Offset: 0x0008193C
		internal IPEndPoint Snapshot()
		{
			return new IPEndPoint(this.Address.Snapshot(), this.Port);
		}

		// Token: 0x04002156 RID: 8534
		public const int MinPort = 0;

		// Token: 0x04002157 RID: 8535
		public const int MaxPort = 65535;

		// Token: 0x04002158 RID: 8536
		internal const int AnyPort = 0;

		// Token: 0x04002159 RID: 8537
		private IPAddress m_Address;

		// Token: 0x0400215A RID: 8538
		private int m_Port;

		// Token: 0x0400215B RID: 8539
		internal static IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);

		// Token: 0x0400215C RID: 8540
		internal static IPEndPoint IPv6Any = new IPEndPoint(IPAddress.IPv6Any, 0);
	}
}
