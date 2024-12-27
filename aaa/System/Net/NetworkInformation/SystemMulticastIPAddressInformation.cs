using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000634 RID: 1588
	internal class SystemMulticastIPAddressInformation : MulticastIPAddressInformation
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x000D3236 File Offset: 0x000D2236
		private SystemMulticastIPAddressInformation()
		{
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000D323E File Offset: 0x000D223E
		internal SystemMulticastIPAddressInformation(IpAdapterAddress adapterAddress, IPAddress ipAddress)
		{
			this.innerInfo = new SystemIPAddressInformation(adapterAddress, ipAddress);
			this.adapterAddress = adapterAddress;
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x000D325A File Offset: 0x000D225A
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x000D3267 File Offset: 0x000D2267
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x000D3274 File Offset: 0x000D2274
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x000D3281 File Offset: 0x000D2281
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return PrefixOrigin.Other;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x000D329B File Offset: 0x000D229B
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return SuffixOrigin.Other;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x000D32B5 File Offset: 0x000D22B5
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return DuplicateAddressDetectionState.Invalid;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x000D32CF File Offset: 0x000D22CF
		public override long AddressValidLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return 0L;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x000D32EA File Offset: 0x000D22EA
		public override long AddressPreferredLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return 0L;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x000D3305 File Offset: 0x000D2305
		public override long DhcpLeaseLifetime
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000D330C File Offset: 0x000D230C
		internal static MulticastIPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr)
		{
			MulticastIPAddressInformationCollection multicastIPAddressInformationCollection = new MulticastIPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return multicastIPAddressInformationCollection;
			}
			IpAdapterAddress ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterAddress));
			AddressFamily addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
			SocketAddress socketAddress = new SocketAddress(addressFamily, ipAdapterAddress.address.addressLength);
			Marshal.Copy(ipAdapterAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterAddress.address.addressLength);
			IPEndPoint ipendPoint;
			if (addressFamily == AddressFamily.InterNetwork)
			{
				ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
			}
			else
			{
				ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
			}
			multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
			while (ipAdapterAddress.next != IntPtr.Zero)
			{
				ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ipAdapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				socketAddress = new SocketAddress(addressFamily, ipAdapterAddress.address.addressLength);
				Marshal.Copy(ipAdapterAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterAddress.address.addressLength);
				if (addressFamily == AddressFamily.InterNetwork)
				{
					ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
				}
				else
				{
					ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
				}
				multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
			}
			return multicastIPAddressInformationCollection;
		}

		// Token: 0x04002E5C RID: 11868
		private IpAdapterAddress adapterAddress;

		// Token: 0x04002E5D RID: 11869
		private SystemIPAddressInformation innerInfo;
	}
}
