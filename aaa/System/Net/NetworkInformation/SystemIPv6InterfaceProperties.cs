using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000637 RID: 1591
	internal class SystemIPv6InterfaceProperties : IPv6InterfaceProperties
	{
		// Token: 0x06003144 RID: 12612 RVA: 0x000D3A34 File Offset: 0x000D2A34
		internal SystemIPv6InterfaceProperties(uint index, uint mtu)
		{
			this.index = index;
			this.mtu = mtu;
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x000D3A4A File Offset: 0x000D2A4A
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000D3A52 File Offset: 0x000D2A52
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x04002E6D RID: 11885
		private uint index;

		// Token: 0x04002E6E RID: 11886
		private uint mtu;
	}
}
