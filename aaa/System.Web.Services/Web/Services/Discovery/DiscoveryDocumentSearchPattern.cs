using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A3 RID: 163
	public sealed class DiscoveryDocumentSearchPattern : DiscoverySearchPattern
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000155A0 File Offset: 0x000145A0
		public override string Pattern
		{
			get
			{
				return "*.vsdisco";
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000155A7 File Offset: 0x000145A7
		public override DiscoveryReference GetDiscoveryReference(string filename)
		{
			return new DiscoveryDocumentReference(filename);
		}
	}
}
