using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace System.Net
{
	// Token: 0x02000423 RID: 1059
	[Serializable]
	public class IPAddress
	{
		// Token: 0x060020EA RID: 8426 RVA: 0x0008183D File Offset: 0x0008083D
		public IPAddress(long newAddress)
		{
			if (newAddress < 0L || newAddress > (long)((ulong)(-1)))
			{
				throw new ArgumentOutOfRangeException("newAddress");
			}
			this.m_Address = newAddress;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00081874 File Offset: 0x00080874
		public IPAddress(byte[] address, long scopeid)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Length != 16)
			{
				throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "address");
			}
			this.m_Family = AddressFamily.InterNetworkV6;
			for (int i = 0; i < 8; i++)
			{
				this.m_Numbers[i] = (ushort)((int)address[i * 2] * 256 + (int)address[i * 2 + 1]);
			}
			if (scopeid < 0L || scopeid > (long)((ulong)(-1)))
			{
				throw new ArgumentOutOfRangeException("scopeid");
			}
			this.m_ScopeId = scopeid;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00081910 File Offset: 0x00080910
		private IPAddress(ushort[] address, uint scopeid)
		{
			this.m_Family = AddressFamily.InterNetworkV6;
			this.m_Numbers = address;
			this.m_ScopeId = (long)((ulong)scopeid);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00081944 File Offset: 0x00080944
		public IPAddress(byte[] address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address.Length != 4 && address.Length != 16)
			{
				throw new ArgumentException(SR.GetString("dns_bad_ip_address"), "address");
			}
			if (address.Length == 4)
			{
				this.m_Family = AddressFamily.InterNetwork;
				this.m_Address = (long)(((int)address[3] << 24) | ((int)address[2] << 16) | ((int)address[1] << 8) | (int)address[0]) & (long)((ulong)(-1));
				return;
			}
			this.m_Family = AddressFamily.InterNetworkV6;
			for (int i = 0; i < 8; i++)
			{
				this.m_Numbers[i] = (ushort)((int)address[i * 2] * 256 + (int)address[i * 2 + 1]);
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000819F9 File Offset: 0x000809F9
		internal IPAddress(int newAddress)
		{
			this.m_Address = (long)newAddress & (long)((ulong)(-1));
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00081A1F File Offset: 0x00080A1F
		public static bool TryParse(string ipString, out IPAddress address)
		{
			address = IPAddress.InternalParse(ipString, true);
			return address != null;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00081A32 File Offset: 0x00080A32
		public static IPAddress Parse(string ipString)
		{
			return IPAddress.InternalParse(ipString, false);
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00081A3C File Offset: 0x00080A3C
		private unsafe static IPAddress InternalParse(string ipString, bool tryParse)
		{
			if (ipString == null)
			{
				throw new ArgumentNullException("ipString");
			}
			if (ipString.IndexOf(':') != -1)
			{
				SocketException ex;
				if (Socket.OSSupportsIPv6)
				{
					byte[] array = new byte[16];
					SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
					if (UnsafeNclNativeMethods.OSSOCK.WSAStringToAddress(ipString, AddressFamily.InterNetworkV6, IntPtr.Zero, socketAddress.m_Buffer, ref socketAddress.m_Size) == SocketError.Success)
					{
						for (int i = 0; i < 16; i++)
						{
							array[i] = socketAddress[i + 8];
						}
						long num = (long)(((int)socketAddress[27] << 24) + ((int)socketAddress[26] << 16) + ((int)socketAddress[25] << 8) + (int)socketAddress[24]);
						return new IPAddress(array, num);
					}
					if (tryParse)
					{
						return null;
					}
					ex = new SocketException();
				}
				else
				{
					int num2 = 0;
					if (ipString[0] != '[')
					{
						ipString += ']';
					}
					else
					{
						num2 = 1;
					}
					int length = ipString.Length;
					fixed (char* ptr = ipString)
					{
						if (IPv6AddressHelper.IsValid(ptr, num2, ref length) || length != ipString.Length)
						{
							ushort[] array2 = new ushort[8];
							string text = null;
							fixed (ushort* ptr2 = array2)
							{
								IPv6AddressHelper.Parse(ipString, ptr2, 0, ref text);
							}
							IPAddress ipaddress;
							if (text == null || text.Length == 0)
							{
								ipaddress = new IPAddress(array2, 0U);
							}
							else
							{
								text = text.Substring(1);
								uint num3;
								if (!uint.TryParse(text, NumberStyles.None, null, out num3))
								{
									goto IL_018E;
								}
								ipaddress = new IPAddress(array2, num3);
							}
							return ipaddress;
						}
						IL_018E:;
					}
					if (tryParse)
					{
						return null;
					}
					ex = new SocketException(SocketError.InvalidArgument);
				}
				throw new FormatException(SR.GetString("dns_bad_ip_address"), ex);
			}
			int num4 = -1;
			if (ipString.Length > 0 && ipString[0] >= '0' && ipString[0] <= '9' && ((ipString[ipString.Length - 1] >= '0' && ipString[ipString.Length - 1] <= '9') || (ipString[ipString.Length - 1] >= 'a' && ipString[ipString.Length - 1] <= 'f') || (ipString[ipString.Length - 1] >= 'A' && ipString[ipString.Length - 1] <= 'F')))
			{
				Socket.InitializeSockets();
				num4 = UnsafeNclNativeMethods.OSSOCK.inet_addr(ipString);
			}
			if (num4 != -1 || string.Compare(ipString, "255.255.255.255", StringComparison.Ordinal) == 0 || string.Compare(ipString, "0xff.0xff.0xff.0xff", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(ipString, "0377.0377.0377.0377", StringComparison.Ordinal) == 0)
			{
				return new IPAddress(num4);
			}
			if (tryParse)
			{
				return null;
			}
			throw new FormatException(SR.GetString("dns_bad_ip_address"));
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00081CEE File Offset: 0x00080CEE
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x00081D0B File Offset: 0x00080D0B
		[Obsolete("This property has been deprecated. It is address family dependent. Please use IPAddress.Equals method to perform comparisons. http://go.microsoft.com/fwlink/?linkid=14202")]
		public long Address
		{
			get
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				return this.m_Address;
			}
			set
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				if (this.m_Address != value)
				{
					this.m_ToString = null;
					this.m_Address = value;
				}
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00081D3C File Offset: 0x00080D3C
		public byte[] GetAddressBytes()
		{
			byte[] array;
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				array = new byte[16];
				int num = 0;
				for (int i = 0; i < 8; i++)
				{
					array[num++] = (byte)((this.m_Numbers[i] >> 8) & 255);
					array[num++] = (byte)(this.m_Numbers[i] & 255);
				}
			}
			else
			{
				array = new byte[]
				{
					(byte)this.m_Address,
					(byte)(this.m_Address >> 8),
					(byte)(this.m_Address >> 16),
					(byte)(this.m_Address >> 24)
				};
			}
			return array;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060020F5 RID: 8437 RVA: 0x00081DD1 File Offset: 0x00080DD1
		public AddressFamily AddressFamily
		{
			get
			{
				return this.m_Family;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00081DD9 File Offset: 0x00080DD9
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x00081DF8 File Offset: 0x00080DF8
		public long ScopeId
		{
			get
			{
				if (this.m_Family == AddressFamily.InterNetwork)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				return this.m_ScopeId;
			}
			set
			{
				if (this.m_Family == AddressFamily.InterNetwork)
				{
					throw new SocketException(SocketError.OperationNotSupported);
				}
				if (value < 0L || value > (long)((ulong)(-1)))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.m_ScopeId != value)
				{
					this.m_Address = value;
					this.m_ScopeId = value;
				}
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00081E48 File Offset: 0x00080E48
		public unsafe override string ToString()
		{
			if (this.m_ToString == null)
			{
				if (this.m_Family == AddressFamily.InterNetworkV6)
				{
					int num = 256;
					StringBuilder stringBuilder = new StringBuilder(num);
					if (Socket.OSSupportsIPv6)
					{
						SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetworkV6, 28);
						int num2 = 8;
						for (int i = 0; i < 8; i++)
						{
							socketAddress[num2++] = (byte)(this.m_Numbers[i] >> 8);
							socketAddress[num2++] = (byte)this.m_Numbers[i];
						}
						if (this.m_ScopeId > 0L)
						{
							socketAddress[24] = (byte)this.m_ScopeId;
							socketAddress[25] = (byte)(this.m_ScopeId >> 8);
							socketAddress[26] = (byte)(this.m_ScopeId >> 16);
							socketAddress[27] = (byte)(this.m_ScopeId >> 24);
						}
						SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAAddressToString(socketAddress.m_Buffer, socketAddress.m_Size, IntPtr.Zero, stringBuilder, ref num);
						if (socketError != SocketError.Success)
						{
							throw new SocketException();
						}
					}
					else
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[0] })).Append(':');
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[1] })).Append(':');
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[2] })).Append(':');
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[3] })).Append(':');
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[4] })).Append(':');
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0:x4}", new object[] { this.m_Numbers[5] })).Append(':');
						stringBuilder.Append((this.m_Numbers[6] >> 8) & 255).Append('.');
						stringBuilder.Append((int)(this.m_Numbers[6] & 255)).Append('.');
						stringBuilder.Append((this.m_Numbers[7] >> 8) & 255).Append('.');
						stringBuilder.Append((int)(this.m_Numbers[7] & 255));
						if (this.m_ScopeId != 0L)
						{
							stringBuilder.Append('%').Append((uint)this.m_ScopeId);
						}
					}
					this.m_ToString = stringBuilder.ToString();
				}
				else
				{
					int num3 = 15;
					char* ptr = stackalloc char[2 * 15];
					int num4 = (int)((this.m_Address >> 24) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)((this.m_Address >> 16) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)((this.m_Address >> 8) & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					ptr[(IntPtr)(--num3) * 2] = '.';
					num4 = (int)(this.m_Address & 255L);
					do
					{
						ptr[(IntPtr)(--num3) * 2] = (char)(48 + num4 % 10);
						num4 /= 10;
					}
					while (num4 > 0);
					this.m_ToString = new string(ptr, num3, 15 - num3);
				}
			}
			return this.m_ToString;
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00082259 File Offset: 0x00081259
		public static long HostToNetworkOrder(long host)
		{
			return (((long)IPAddress.HostToNetworkOrder((int)host) & (long)((ulong)(-1))) << 32) | ((long)IPAddress.HostToNetworkOrder((int)(host >> 32)) & (long)((ulong)(-1)));
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00082278 File Offset: 0x00081278
		public static int HostToNetworkOrder(int host)
		{
			return (((int)IPAddress.HostToNetworkOrder((short)host) & 65535) << 16) | ((int)IPAddress.HostToNetworkOrder((short)(host >> 16)) & 65535);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0008229B File Offset: 0x0008129B
		public static short HostToNetworkOrder(short host)
		{
			return (short)(((int)(host & 255) << 8) | ((host >> 8) & 255));
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000822B1 File Offset: 0x000812B1
		public static long NetworkToHostOrder(long network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000822B9 File Offset: 0x000812B9
		public static int NetworkToHostOrder(int network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000822C1 File Offset: 0x000812C1
		public static short NetworkToHostOrder(short network)
		{
			return IPAddress.HostToNetworkOrder(network);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000822C9 File Offset: 0x000812C9
		public static bool IsLoopback(IPAddress address)
		{
			if (address.m_Family == AddressFamily.InterNetworkV6)
			{
				return address.Equals(IPAddress.IPv6Loopback);
			}
			return (address.m_Address & 127L) == (IPAddress.Loopback.m_Address & 127L);
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x000822FB File Offset: 0x000812FB
		internal bool IsBroadcast
		{
			get
			{
				return this.m_Family != AddressFamily.InterNetworkV6 && this.m_Address == IPAddress.Broadcast.m_Address;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x0008231B File Offset: 0x0008131B
		public bool IsIPv6Multicast
		{
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65280) == 65280;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x0008233E File Offset: 0x0008133E
		public bool IsIPv6LinkLocal
		{
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65472) == 65152;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x00082361 File Offset: 0x00081361
		public bool IsIPv6SiteLocal
		{
			get
			{
				return this.m_Family == AddressFamily.InterNetworkV6 && (this.m_Numbers[0] & 65472) == 65216;
			}
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00082384 File Offset: 0x00081384
		internal bool Equals(object comparand, bool compareScopeId)
		{
			if (!(comparand is IPAddress))
			{
				return false;
			}
			if (this.m_Family != ((IPAddress)comparand).m_Family)
			{
				return false;
			}
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				for (int i = 0; i < 8; i++)
				{
					if (((IPAddress)comparand).m_Numbers[i] != this.m_Numbers[i])
					{
						return false;
					}
				}
				return ((IPAddress)comparand).m_ScopeId == this.m_ScopeId || !compareScopeId;
			}
			return ((IPAddress)comparand).m_Address == this.m_Address;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0008240E File Offset: 0x0008140E
		public override bool Equals(object comparand)
		{
			return this.Equals(comparand, true);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00082418 File Offset: 0x00081418
		public override int GetHashCode()
		{
			if (this.m_Family == AddressFamily.InterNetworkV6)
			{
				if (this.m_HashCode == 0)
				{
					this.m_HashCode = Uri.CalculateCaseInsensitiveHashCode(this.ToString());
				}
				return this.m_HashCode;
			}
			return (int)this.m_Address;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x0008244C File Offset: 0x0008144C
		internal IPAddress Snapshot()
		{
			AddressFamily family = this.m_Family;
			if (family == AddressFamily.InterNetwork)
			{
				return new IPAddress(this.m_Address);
			}
			if (family != AddressFamily.InterNetworkV6)
			{
				throw new InternalException();
			}
			return new IPAddress(this.m_Numbers, (uint)this.m_ScopeId);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00082490 File Offset: 0x00081490
		// Note: this type is marked as 'beforefieldinit'.
		static IPAddress()
		{
			byte[] array = new byte[16];
			IPAddress.IPv6Any = new IPAddress(array, 0L);
			IPAddress.IPv6Loopback = new IPAddress(new byte[]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 1
			}, 0L);
			byte[] array2 = new byte[16];
			IPAddress.IPv6None = new IPAddress(array2, 0L);
		}

		// Token: 0x04002142 RID: 8514
		internal const long LoopbackMask = 127L;

		// Token: 0x04002143 RID: 8515
		internal const string InaddrNoneString = "255.255.255.255";

		// Token: 0x04002144 RID: 8516
		internal const string InaddrNoneStringHex = "0xff.0xff.0xff.0xff";

		// Token: 0x04002145 RID: 8517
		internal const string InaddrNoneStringOct = "0377.0377.0377.0377";

		// Token: 0x04002146 RID: 8518
		internal const int IPv4AddressBytes = 4;

		// Token: 0x04002147 RID: 8519
		internal const int IPv6AddressBytes = 16;

		// Token: 0x04002148 RID: 8520
		internal const int NumberOfLabels = 8;

		// Token: 0x04002149 RID: 8521
		public static readonly IPAddress Any = new IPAddress(0);

		// Token: 0x0400214A RID: 8522
		public static readonly IPAddress Loopback = new IPAddress(16777343);

		// Token: 0x0400214B RID: 8523
		public static readonly IPAddress Broadcast = new IPAddress((long)((ulong)(-1)));

		// Token: 0x0400214C RID: 8524
		public static readonly IPAddress None = IPAddress.Broadcast;

		// Token: 0x0400214D RID: 8525
		internal long m_Address;

		// Token: 0x0400214E RID: 8526
		[NonSerialized]
		internal string m_ToString;

		// Token: 0x0400214F RID: 8527
		public static readonly IPAddress IPv6Any;

		// Token: 0x04002150 RID: 8528
		public static readonly IPAddress IPv6Loopback;

		// Token: 0x04002151 RID: 8529
		public static readonly IPAddress IPv6None;

		// Token: 0x04002152 RID: 8530
		private AddressFamily m_Family = AddressFamily.InterNetwork;

		// Token: 0x04002153 RID: 8531
		private ushort[] m_Numbers = new ushort[8];

		// Token: 0x04002154 RID: 8532
		private long m_ScopeId;

		// Token: 0x04002155 RID: 8533
		private int m_HashCode;
	}
}
