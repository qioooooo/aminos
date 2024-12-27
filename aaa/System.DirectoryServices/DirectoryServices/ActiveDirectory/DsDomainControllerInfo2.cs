using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B6 RID: 182
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DsDomainControllerInfo2
	{
		// Token: 0x0400049D RID: 1181
		public string netBiosName;

		// Token: 0x0400049E RID: 1182
		public string dnsHostName;

		// Token: 0x0400049F RID: 1183
		public string siteName;

		// Token: 0x040004A0 RID: 1184
		public string siteObjectName;

		// Token: 0x040004A1 RID: 1185
		public string computerObjectName;

		// Token: 0x040004A2 RID: 1186
		public string serverObjectName;

		// Token: 0x040004A3 RID: 1187
		public string ntdsaObjectName;

		// Token: 0x040004A4 RID: 1188
		public bool isPdc;

		// Token: 0x040004A5 RID: 1189
		public bool dsEnabled;

		// Token: 0x040004A6 RID: 1190
		public bool isGC;

		// Token: 0x040004A7 RID: 1191
		public Guid siteObjectGuid;

		// Token: 0x040004A8 RID: 1192
		public Guid computerObjectGuid;

		// Token: 0x040004A9 RID: 1193
		public Guid serverObjectGuid;

		// Token: 0x040004AA RID: 1194
		public Guid ntdsDsaObjectGuid;
	}
}
