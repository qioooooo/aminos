using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062E RID: 1582
	internal class SystemIPAddressInformation : IPAddressInformation
	{
		// Token: 0x060030BC RID: 12476 RVA: 0x000D1F8E File Offset: 0x000D0F8E
		internal SystemIPAddressInformation(IPAddress address)
		{
			this.address = address;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				this.dnsEligible = (address.m_Address & 65193L) <= 0L;
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000D1FC7 File Offset: 0x000D0FC7
		internal SystemIPAddressInformation(IpAdapterUnicastAddress adapterAddress, IPAddress address)
		{
			this.address = address;
			this.transient = (adapterAddress.flags & AdapterAddressFlags.Transient) > (AdapterAddressFlags)0;
			this.dnsEligible = (adapterAddress.flags & AdapterAddressFlags.DnsEligible) > (AdapterAddressFlags)0;
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000D2001 File Offset: 0x000D1001
		internal SystemIPAddressInformation(IpAdapterAddress adapterAddress, IPAddress address)
		{
			this.address = address;
			this.transient = (adapterAddress.flags & AdapterAddressFlags.Transient) > (AdapterAddressFlags)0;
			this.dnsEligible = (adapterAddress.flags & AdapterAddressFlags.DnsEligible) > (AdapterAddressFlags)0;
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x000D203B File Offset: 0x000D103B
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000D2043 File Offset: 0x000D1043
		public override bool IsTransient
		{
			get
			{
				return this.transient;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x000D204B File Offset: 0x000D104B
		public override bool IsDnsEligible
		{
			get
			{
				return this.dnsEligible;
			}
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000D2054 File Offset: 0x000D1054
		internal static IPAddressCollection ToAddressCollection(IntPtr ptr, IPVersion versionSupported)
		{
			IPAddressCollection ipaddressCollection = new IPAddressCollection();
			if (ptr == IntPtr.Zero)
			{
				return ipaddressCollection;
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
			ipaddressCollection.InternalAdd(ipendPoint.Address);
			while (ipAdapterAddress.next != IntPtr.Zero)
			{
				ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ipAdapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				if ((addressFamily == AddressFamily.InterNetwork && (versionSupported & IPVersion.IPv4) > IPVersion.None) || (addressFamily == AddressFamily.InterNetworkV6 && (versionSupported & IPVersion.IPv6) > IPVersion.None))
				{
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
					ipaddressCollection.InternalAdd(ipendPoint.Address);
				}
			}
			return ipaddressCollection;
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000D21E4 File Offset: 0x000D11E4
		internal static IPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr, IPVersion versionSupported)
		{
			IPAddressInformationCollection ipaddressInformationCollection = new IPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return ipaddressInformationCollection;
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
			ipaddressInformationCollection.InternalAdd(new SystemIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
			while (ipAdapterAddress.next != IntPtr.Zero)
			{
				ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ipAdapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				if ((addressFamily == AddressFamily.InterNetwork && (versionSupported & IPVersion.IPv4) > IPVersion.None) || (addressFamily == AddressFamily.InterNetworkV6 && (versionSupported & IPVersion.IPv6) > IPVersion.None))
				{
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
					ipaddressInformationCollection.InternalAdd(new SystemIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
				}
			}
			return ipaddressInformationCollection;
		}

		// Token: 0x04002E41 RID: 11841
		private IPAddress address;

		// Token: 0x04002E42 RID: 11842
		internal bool transient;

		// Token: 0x04002E43 RID: 11843
		internal bool dnsEligible = true;
	}
}
