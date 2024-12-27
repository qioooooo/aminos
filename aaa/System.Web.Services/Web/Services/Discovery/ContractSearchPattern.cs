using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x02000098 RID: 152
	public sealed class ContractSearchPattern : DiscoverySearchPattern
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00013C08 File Offset: 0x00012C08
		public override string Pattern
		{
			get
			{
				return "*.asmx";
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00013C0F File Offset: 0x00012C0F
		public override DiscoveryReference GetDiscoveryReference(string filename)
		{
			return new ContractReference(filename + "?wsdl", filename);
		}
	}
}
