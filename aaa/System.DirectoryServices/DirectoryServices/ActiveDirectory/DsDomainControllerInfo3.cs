using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B7 RID: 183
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DsDomainControllerInfo3
	{
		// Token: 0x040004AB RID: 1195
		public string netBiosName;

		// Token: 0x040004AC RID: 1196
		public string dnsHostName;

		// Token: 0x040004AD RID: 1197
		public string siteName;

		// Token: 0x040004AE RID: 1198
		public string siteObjectName;

		// Token: 0x040004AF RID: 1199
		public string computerObjectName;

		// Token: 0x040004B0 RID: 1200
		public string serverObjectName;

		// Token: 0x040004B1 RID: 1201
		public string ntdsaObjectName;

		// Token: 0x040004B2 RID: 1202
		public bool isPdc;

		// Token: 0x040004B3 RID: 1203
		public bool dsEnabled;

		// Token: 0x040004B4 RID: 1204
		public bool isGC;

		// Token: 0x040004B5 RID: 1205
		public bool isRodc;

		// Token: 0x040004B6 RID: 1206
		public Guid siteObjectGuid;

		// Token: 0x040004B7 RID: 1207
		public Guid computerObjectGuid;

		// Token: 0x040004B8 RID: 1208
		public Guid serverObjectGuid;

		// Token: 0x040004B9 RID: 1209
		public Guid ntdsDsaObjectGuid;
	}
}
