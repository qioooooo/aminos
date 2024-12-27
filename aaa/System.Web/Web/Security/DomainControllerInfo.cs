using System;
using System.Runtime.InteropServices;

namespace System.Web.Security
{
	// Token: 0x0200031B RID: 795
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DomainControllerInfo
	{
		// Token: 0x04001E2A RID: 7722
		public string DomainControllerName;

		// Token: 0x04001E2B RID: 7723
		public string DomainControllerAddress;

		// Token: 0x04001E2C RID: 7724
		public int DomainControllerAddressType;

		// Token: 0x04001E2D RID: 7725
		public Guid DomainGuid;

		// Token: 0x04001E2E RID: 7726
		public string DomainName;

		// Token: 0x04001E2F RID: 7727
		public string DnsForestName;

		// Token: 0x04001E30 RID: 7728
		public int Flags;

		// Token: 0x04001E31 RID: 7729
		public string DcSiteName;

		// Token: 0x04001E32 RID: 7730
		public string ClientSiteName;
	}
}
