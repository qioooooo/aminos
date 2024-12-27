using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000636 RID: 1590
	internal class SystemIPv4InterfaceProperties : IPv4InterfaceProperties
	{
		// Token: 0x06003137 RID: 12599 RVA: 0x000D37B4 File Offset: 0x000D27B4
		internal SystemIPv4InterfaceProperties(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.index = ipAdapterInfo.index;
			this.routingEnabled = fixedInfo.EnableRouting;
			this.dhcpEnabled = ipAdapterInfo.dhcpEnabled;
			this.haveWins = ipAdapterInfo.haveWins;
			this.gatewayAddresses = ipAdapterInfo.gatewayList.ToIPGatewayAddressCollection();
			this.dhcpAddresses = ipAdapterInfo.dhcpServer.ToIPAddressCollection();
			IPAddressCollection ipaddressCollection = ipAdapterInfo.primaryWinsServer.ToIPAddressCollection();
			IPAddressCollection ipaddressCollection2 = ipAdapterInfo.secondaryWinsServer.ToIPAddressCollection();
			this.winsServerAddresses = new IPAddressCollection();
			foreach (IPAddress ipaddress in ipaddressCollection)
			{
				this.winsServerAddresses.InternalAdd(ipaddress);
			}
			foreach (IPAddress ipaddress2 in ipaddressCollection2)
			{
				this.winsServerAddresses.InternalAdd(ipaddress2);
			}
			SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
			this.mtu = (uint)systemIPv4InterfaceStatistics.Mtu;
			if (ComNetOS.IsWin2K)
			{
				this.GetPerAdapterInfo(ipAdapterInfo.index);
				return;
			}
			this.dnsAddresses = fixedInfo.DnsAddresses;
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003138 RID: 12600 RVA: 0x000D3908 File Offset: 0x000D2908
		internal IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x000D3910 File Offset: 0x000D2910
		public override bool UsesWins
		{
			get
			{
				return this.haveWins;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x000D3918 File Offset: 0x000D2918
		public override bool IsDhcpEnabled
		{
			get
			{
				return this.dhcpEnabled;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x000D3920 File Offset: 0x000D2920
		public override bool IsForwardingEnabled
		{
			get
			{
				return this.routingEnabled;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x000D3928 File Offset: 0x000D2928
		public override bool IsAutomaticPrivateAddressingEnabled
		{
			get
			{
				return this.autoConfigEnabled;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x000D3930 File Offset: 0x000D2930
		public override bool IsAutomaticPrivateAddressingActive
		{
			get
			{
				return this.autoConfigActive;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x000D3938 File Offset: 0x000D2938
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x000D3940 File Offset: 0x000D2940
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000D3948 File Offset: 0x000D2948
		internal GatewayIPAddressInformationCollection GetGatewayAddresses()
		{
			return this.gatewayAddresses;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000D3950 File Offset: 0x000D2950
		internal IPAddressCollection GetDhcpServerAddresses()
		{
			return this.dhcpAddresses;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000D3958 File Offset: 0x000D2958
		internal IPAddressCollection GetWinsServersAddresses()
		{
			return this.winsServerAddresses;
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000D3960 File Offset: 0x000D2960
		private void GetPerAdapterInfo(uint index)
		{
			if (index != 0U)
			{
				uint num = 0U;
				SafeLocalFree safeLocalFree = null;
				uint num2 = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, SafeLocalFree.Zero, ref num);
				while (num2 == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, safeLocalFree, ref num);
						if (num2 == 0U)
						{
							IpPerAdapterInfo ipPerAdapterInfo = (IpPerAdapterInfo)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpPerAdapterInfo));
							this.autoConfigEnabled = ipPerAdapterInfo.autoconfigEnabled;
							this.autoConfigActive = ipPerAdapterInfo.autoconfigActive;
							this.dnsAddresses = ipPerAdapterInfo.dnsServerList.ToIPAddressCollection();
						}
					}
					finally
					{
						if (this.dnsAddresses == null)
						{
							this.dnsAddresses = new IPAddressCollection();
						}
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (this.dnsAddresses == null)
				{
					this.dnsAddresses = new IPAddressCollection();
				}
				if (num2 != 0U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
		}

		// Token: 0x04002E62 RID: 11874
		private bool haveWins;

		// Token: 0x04002E63 RID: 11875
		private bool dhcpEnabled;

		// Token: 0x04002E64 RID: 11876
		private bool routingEnabled;

		// Token: 0x04002E65 RID: 11877
		private bool autoConfigEnabled;

		// Token: 0x04002E66 RID: 11878
		private bool autoConfigActive;

		// Token: 0x04002E67 RID: 11879
		private uint index;

		// Token: 0x04002E68 RID: 11880
		private uint mtu;

		// Token: 0x04002E69 RID: 11881
		private GatewayIPAddressInformationCollection gatewayAddresses;

		// Token: 0x04002E6A RID: 11882
		private IPAddressCollection dhcpAddresses;

		// Token: 0x04002E6B RID: 11883
		private IPAddressCollection winsServerAddresses;

		// Token: 0x04002E6C RID: 11884
		internal IPAddressCollection dnsAddresses;
	}
}
