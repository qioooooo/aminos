using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E9 RID: 1513
	internal class SystemGatewayIPAddressInformation : GatewayIPAddressInformation
	{
		// Token: 0x06002FB3 RID: 12211 RVA: 0x000CF0A8 File Offset: 0x000CE0A8
		internal SystemGatewayIPAddressInformation(IPAddress address)
		{
			this.address = address;
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000CF0B7 File Offset: 0x000CE0B7
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x04002CB9 RID: 11449
		private IPAddress address;
	}
}
