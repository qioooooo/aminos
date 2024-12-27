using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A1 RID: 161
	public class DiscoveryDocumentLinksPattern : DiscoverySearchPattern
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00014F0A File Offset: 0x00013F0A
		public override string Pattern
		{
			get
			{
				return "*.disco";
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00014F11 File Offset: 0x00013F11
		public override DiscoveryReference GetDiscoveryReference(string filename)
		{
			return new DiscoveryDocumentReference(filename);
		}
	}
}
