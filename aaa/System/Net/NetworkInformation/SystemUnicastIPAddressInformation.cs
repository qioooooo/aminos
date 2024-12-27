using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000635 RID: 1589
	internal class SystemUnicastIPAddressInformation : UnicastIPAddressInformation
	{
		// Token: 0x06003129 RID: 12585 RVA: 0x000D3490 File Offset: 0x000D2490
		private SystemUnicastIPAddressInformation()
		{
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000D3498 File Offset: 0x000D2498
		internal SystemUnicastIPAddressInformation(IpAdapterInfo ipAdapterInfo, IPExtendedAddress address)
		{
			this.innerInfo = new SystemIPAddressInformation(address.address);
			DateTime dateTime = new DateTime(1970, 1, 1);
			dateTime = dateTime.AddSeconds(ipAdapterInfo.leaseExpires);
			this.dhcpLeaseLifetime = (long)(dateTime - DateTime.UtcNow).TotalSeconds;
			this.ipv4Mask = address.mask;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000D3503 File Offset: 0x000D2503
		internal SystemUnicastIPAddressInformation(IpAdapterUnicastAddress adapterAddress, IPAddress ipAddress)
		{
			this.innerInfo = new SystemIPAddressInformation(adapterAddress, ipAddress);
			this.adapterAddress = adapterAddress;
			this.dhcpLeaseLifetime = (long)((ulong)adapterAddress.leaseLifetime);
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x000D352D File Offset: 0x000D252D
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x000D353A File Offset: 0x000D253A
		public override IPAddress IPv4Mask
		{
			get
			{
				if (this.Address.AddressFamily != AddressFamily.InterNetwork)
				{
					return new IPAddress(0);
				}
				return this.ipv4Mask;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000D3557 File Offset: 0x000D2557
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x000D3564 File Offset: 0x000D2564
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x000D3571 File Offset: 0x000D2571
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.prefixOrigin;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x000D3595 File Offset: 0x000D2595
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.suffixOrigin;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x000D35B9 File Offset: 0x000D25B9
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.dadState;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x000D35DD File Offset: 0x000D25DD
		public override long AddressValidLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (long)((ulong)this.adapterAddress.validLifetime);
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003134 RID: 12596 RVA: 0x000D3602 File Offset: 0x000D2602
		public override long AddressPreferredLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (long)((ulong)this.adapterAddress.preferredLifetime);
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x000D3627 File Offset: 0x000D2627
		public override long DhcpLeaseLifetime
		{
			get
			{
				return this.dhcpLeaseLifetime;
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000D3630 File Offset: 0x000D2630
		internal static UnicastIPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr)
		{
			UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = new UnicastIPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return unicastIPAddressInformationCollection;
			}
			IpAdapterUnicastAddress ipAdapterUnicastAddress = (IpAdapterUnicastAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterUnicastAddress));
			AddressFamily addressFamily = ((ipAdapterUnicastAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
			SocketAddress socketAddress = new SocketAddress(addressFamily, ipAdapterUnicastAddress.address.addressLength);
			Marshal.Copy(ipAdapterUnicastAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterUnicastAddress.address.addressLength);
			IPEndPoint ipendPoint;
			if (addressFamily == AddressFamily.InterNetwork)
			{
				ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
			}
			else
			{
				ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
			}
			unicastIPAddressInformationCollection.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterUnicastAddress, ipendPoint.Address));
			while (ipAdapterUnicastAddress.next != IntPtr.Zero)
			{
				ipAdapterUnicastAddress = (IpAdapterUnicastAddress)Marshal.PtrToStructure(ipAdapterUnicastAddress.next, typeof(IpAdapterUnicastAddress));
				addressFamily = ((ipAdapterUnicastAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				socketAddress = new SocketAddress(addressFamily, ipAdapterUnicastAddress.address.addressLength);
				Marshal.Copy(ipAdapterUnicastAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterUnicastAddress.address.addressLength);
				if (addressFamily == AddressFamily.InterNetwork)
				{
					ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
				}
				else
				{
					ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
				}
				unicastIPAddressInformationCollection.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterUnicastAddress, ipendPoint.Address));
			}
			return unicastIPAddressInformationCollection;
		}

		// Token: 0x04002E5E RID: 11870
		private IpAdapterUnicastAddress adapterAddress;

		// Token: 0x04002E5F RID: 11871
		private long dhcpLeaseLifetime;

		// Token: 0x04002E60 RID: 11872
		private SystemIPAddressInformation innerInfo;

		// Token: 0x04002E61 RID: 11873
		internal IPAddress ipv4Mask;
	}
}
