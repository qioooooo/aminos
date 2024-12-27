using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B5 RID: 181
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DomainControllerInfo
	{
		// Token: 0x04000494 RID: 1172
		public string DomainControllerName;

		// Token: 0x04000495 RID: 1173
		public string DomainControllerAddress;

		// Token: 0x04000496 RID: 1174
		public int DomainControllerAddressType;

		// Token: 0x04000497 RID: 1175
		public Guid DomainGuid;

		// Token: 0x04000498 RID: 1176
		public string DomainName;

		// Token: 0x04000499 RID: 1177
		public string DnsForestName;

		// Token: 0x0400049A RID: 1178
		public int Flags;

		// Token: 0x0400049B RID: 1179
		public string DcSiteName;

		// Token: 0x0400049C RID: 1180
		public string ClientSiteName;
	}
}
