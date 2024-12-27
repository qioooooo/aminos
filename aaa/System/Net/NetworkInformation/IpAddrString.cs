using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F4 RID: 1524
	internal struct IpAddrString
	{
		// Token: 0x06002FCF RID: 12239 RVA: 0x000CF1AC File Offset: 0x000CE1AC
		internal IPAddressCollection ToIPAddressCollection()
		{
			IpAddrString ipAddrString = this;
			IPAddressCollection ipaddressCollection = new IPAddressCollection();
			if (ipAddrString.IpAddress.Length != 0)
			{
				ipaddressCollection.InternalAdd(IPAddress.Parse(ipAddrString.IpAddress));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					ipaddressCollection.InternalAdd(IPAddress.Parse(ipAddrString.IpAddress));
				}
			}
			return ipaddressCollection;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000CF238 File Offset: 0x000CE238
		internal ArrayList ToIPExtendedAddressArrayList()
		{
			IpAddrString ipAddrString = this;
			ArrayList arrayList = new ArrayList();
			if (ipAddrString.IpAddress.Length != 0)
			{
				arrayList.Add(new IPExtendedAddress(IPAddress.Parse(ipAddrString.IpAddress), IPAddress.Parse(ipAddrString.IpMask)));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					arrayList.Add(new IPExtendedAddress(IPAddress.Parse(ipAddrString.IpAddress), IPAddress.Parse(ipAddrString.IpMask)));
				}
			}
			return arrayList;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000CF2F4 File Offset: 0x000CE2F4
		internal GatewayIPAddressInformationCollection ToIPGatewayAddressCollection()
		{
			IpAddrString ipAddrString = this;
			GatewayIPAddressInformationCollection gatewayIPAddressInformationCollection = new GatewayIPAddressInformationCollection();
			if (ipAddrString.IpAddress.Length != 0)
			{
				gatewayIPAddressInformationCollection.InternalAdd(new SystemGatewayIPAddressInformation(IPAddress.Parse(ipAddrString.IpAddress)));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					gatewayIPAddressInformationCollection.InternalAdd(new SystemGatewayIPAddressInformation(IPAddress.Parse(ipAddrString.IpAddress)));
				}
			}
			return gatewayIPAddressInformationCollection;
		}

		// Token: 0x04002CE7 RID: 11495
		internal IntPtr Next;

		// Token: 0x04002CE8 RID: 11496
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpAddress;

		// Token: 0x04002CE9 RID: 11497
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpMask;

		// Token: 0x04002CEA RID: 11498
		internal uint Context;
	}
}
