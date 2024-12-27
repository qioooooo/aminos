using System;
using System.Collections;
using System.Net.Sockets;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000631 RID: 1585
	internal class SystemIPInterfaceProperties : IPInterfaceProperties
	{
		// Token: 0x060030E2 RID: 12514 RVA: 0x000D2A07 File Offset: 0x000D1A07
		private SystemIPInterfaceProperties()
		{
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000D2A10 File Offset: 0x000D1A10
		internal SystemIPInterfaceProperties(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.dnsEnabled = fixedInfo.EnableDns;
			this.index = ipAdapterAddresses.index;
			this.name = ipAdapterAddresses.AdapterName;
			this.ipv6Index = ipAdapterAddresses.ipv6Index;
			if (this.index > 0U)
			{
				this.versionSupported |= IPVersion.IPv4;
			}
			if (this.ipv6Index > 0U)
			{
				this.versionSupported |= IPVersion.IPv6;
			}
			this.mtu = ipAdapterAddresses.mtu;
			this.adapterFlags = ipAdapterAddresses.flags;
			this.dnsSuffix = ipAdapterAddresses.dnsSuffix;
			this.dynamicDnsEnabled = (ipAdapterAddresses.flags & AdapterFlags.DnsEnabled) > (AdapterFlags)0;
			this.multicastAddresses = SystemMulticastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstMulticastAddress);
			this.dnsAddresses = SystemIPAddressInformation.ToAddressCollection(ipAdapterAddresses.FirstDnsServerAddress, this.versionSupported);
			this.anycastAddresses = SystemIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstAnycastAddress, this.versionSupported);
			this.unicastAddresses = SystemUnicastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstUnicastAddress);
			if (this.ipv6Index > 0U)
			{
				this.ipv6Properties = new SystemIPv6InterfaceProperties(this.ipv6Index, this.mtu);
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000D2B34 File Offset: 0x000D1B34
		internal SystemIPInterfaceProperties(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.dnsEnabled = fixedInfo.EnableDns;
			this.name = ipAdapterInfo.adapterName;
			this.index = ipAdapterInfo.index;
			this.multicastAddresses = new MulticastIPAddressInformationCollection();
			this.anycastAddresses = new IPAddressInformationCollection();
			if (this.index > 0U)
			{
				this.versionSupported |= IPVersion.IPv4;
			}
			if (ComNetOS.IsWin2K)
			{
				this.ReadRegDnsSuffix();
			}
			this.unicastAddresses = new UnicastIPAddressInformationCollection();
			ArrayList arrayList = ipAdapterInfo.ipAddressList.ToIPExtendedAddressArrayList();
			foreach (object obj in arrayList)
			{
				IPExtendedAddress ipextendedAddress = (IPExtendedAddress)obj;
				this.unicastAddresses.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterInfo, ipextendedAddress));
			}
			try
			{
				this.ipv4Properties = new SystemIPv4InterfaceProperties(fixedInfo, ipAdapterInfo);
				if (this.dnsAddresses == null || this.dnsAddresses.Count == 0)
				{
					this.dnsAddresses = this.ipv4Properties.DnsAddresses;
				}
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode != 87L)
				{
					throw;
				}
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x000D2C68 File Offset: 0x000D1C68
		public override bool IsDnsEnabled
		{
			get
			{
				return this.dnsEnabled;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000D2C70 File Offset: 0x000D1C70
		public override bool IsDynamicDnsEnabled
		{
			get
			{
				return this.dynamicDnsEnabled;
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000D2C78 File Offset: 0x000D1C78
		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (this.index == 0U)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv4Properties;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000D2C93 File Offset: 0x000D1C93
		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			if (this.ipv6Index == 0U)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv6Properties;
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000D2CAE File Offset: 0x000D1CAE
		public override string DnsSuffix
		{
			get
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				return this.dnsSuffix;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000D2CCD File Offset: 0x000D1CCD
		public override IPAddressInformationCollection AnycastAddresses
		{
			get
			{
				return this.anycastAddresses;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x000D2CD5 File Offset: 0x000D1CD5
		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				return this.unicastAddresses;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000D2CDD File Offset: 0x000D1CDD
		public override MulticastIPAddressInformationCollection MulticastAddresses
		{
			get
			{
				return this.multicastAddresses;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x000D2CE5 File Offset: 0x000D1CE5
		public override IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x000D2CED File Offset: 0x000D1CED
		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetGatewayAddresses();
				}
				return new GatewayIPAddressInformationCollection();
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x000D2D08 File Offset: 0x000D1D08
		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetDhcpServerAddresses();
				}
				return new IPAddressCollection();
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000D2D23 File Offset: 0x000D1D23
		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetWinsServersAddresses();
				}
				return new IPAddressCollection();
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000D2D40 File Offset: 0x000D1D40
		internal bool Update(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			try
			{
				ArrayList arrayList = ipAdapterInfo.ipAddressList.ToIPExtendedAddressArrayList();
				foreach (object obj in arrayList)
				{
					IPExtendedAddress ipextendedAddress = (IPExtendedAddress)obj;
					foreach (UnicastIPAddressInformation unicastIPAddressInformation in this.unicastAddresses)
					{
						SystemUnicastIPAddressInformation systemUnicastIPAddressInformation = (SystemUnicastIPAddressInformation)unicastIPAddressInformation;
						if (ipextendedAddress.address.Equals(systemUnicastIPAddressInformation.Address))
						{
							systemUnicastIPAddressInformation.ipv4Mask = ipextendedAddress.mask;
						}
					}
				}
				this.ipv4Properties = new SystemIPv4InterfaceProperties(fixedInfo, ipAdapterInfo);
				if (this.dnsAddresses == null || this.dnsAddresses.Count == 0)
				{
					this.dnsAddresses = this.ipv4Properties.DnsAddresses;
				}
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode == 87L || (long)ex.ErrorCode == 13L || (long)ex.ErrorCode == 232L || (long)ex.ErrorCode == 1L || (long)ex.ErrorCode == 2L)
				{
					return false;
				}
				throw;
			}
			return true;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000D2E8C File Offset: 0x000D1E8C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces")]
		private void ReadRegDnsSuffix()
		{
			RegistryKey registryKey = null;
			try
			{
				string text = "SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + this.name;
				registryKey = Registry.LocalMachine.OpenSubKey(text);
				if (registryKey != null)
				{
					this.dnsSuffix = (string)registryKey.GetValue("DhcpDomain");
					if (this.dnsSuffix == null)
					{
						this.dnsSuffix = (string)registryKey.GetValue("Domain");
						if (this.dnsSuffix == null)
						{
							this.dnsSuffix = string.Empty;
						}
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
		}

		// Token: 0x04002E4B RID: 11851
		private uint mtu;

		// Token: 0x04002E4C RID: 11852
		internal uint index;

		// Token: 0x04002E4D RID: 11853
		internal uint ipv6Index;

		// Token: 0x04002E4E RID: 11854
		internal IPVersion versionSupported;

		// Token: 0x04002E4F RID: 11855
		private bool dnsEnabled;

		// Token: 0x04002E50 RID: 11856
		private bool dynamicDnsEnabled;

		// Token: 0x04002E51 RID: 11857
		private IPAddressCollection dnsAddresses;

		// Token: 0x04002E52 RID: 11858
		private UnicastIPAddressInformationCollection unicastAddresses;

		// Token: 0x04002E53 RID: 11859
		private MulticastIPAddressInformationCollection multicastAddresses;

		// Token: 0x04002E54 RID: 11860
		private IPAddressInformationCollection anycastAddresses;

		// Token: 0x04002E55 RID: 11861
		private AdapterFlags adapterFlags;

		// Token: 0x04002E56 RID: 11862
		private string dnsSuffix;

		// Token: 0x04002E57 RID: 11863
		private string name;

		// Token: 0x04002E58 RID: 11864
		private SystemIPv4InterfaceProperties ipv4Properties;

		// Token: 0x04002E59 RID: 11865
		private SystemIPv6InterfaceProperties ipv6Properties;
	}
}
