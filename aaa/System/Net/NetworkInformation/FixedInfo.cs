using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000630 RID: 1584
	internal struct FixedInfo
	{
		// Token: 0x060030D9 RID: 12505 RVA: 0x000D2989 File Offset: 0x000D1989
		internal FixedInfo(FIXED_INFO info)
		{
			this.info = info;
			this.dnsAddresses = info.DnsServerList.ToIPAddressCollection();
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x000D29A4 File Offset: 0x000D19A4
		internal IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x000D29AC File Offset: 0x000D19AC
		internal string HostName
		{
			get
			{
				return this.info.hostName;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060030DC RID: 12508 RVA: 0x000D29B9 File Offset: 0x000D19B9
		internal string DomainName
		{
			get
			{
				return this.info.domainName;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x000D29C6 File Offset: 0x000D19C6
		internal NetBiosNodeType NodeType
		{
			get
			{
				return this.info.nodeType;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x000D29D3 File Offset: 0x000D19D3
		internal string ScopeId
		{
			get
			{
				return this.info.scopeId;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000D29E0 File Offset: 0x000D19E0
		internal bool EnableRouting
		{
			get
			{
				return this.info.enableRouting;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x000D29ED File Offset: 0x000D19ED
		internal bool EnableProxy
		{
			get
			{
				return this.info.enableProxy;
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000D29FA File Offset: 0x000D19FA
		internal bool EnableDns
		{
			get
			{
				return this.info.enableDns;
			}
		}

		// Token: 0x04002E49 RID: 11849
		internal FIXED_INFO info;

		// Token: 0x04002E4A RID: 11850
		internal IPAddressCollection dnsAddresses;
	}
}
