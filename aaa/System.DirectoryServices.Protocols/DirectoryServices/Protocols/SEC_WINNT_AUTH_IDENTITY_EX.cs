using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000090 RID: 144
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class SEC_WINNT_AUTH_IDENTITY_EX
	{
		// Token: 0x040002A7 RID: 679
		public int version;

		// Token: 0x040002A8 RID: 680
		public int length;

		// Token: 0x040002A9 RID: 681
		public string user;

		// Token: 0x040002AA RID: 682
		public int userLength;

		// Token: 0x040002AB RID: 683
		public string domain;

		// Token: 0x040002AC RID: 684
		public int domainLength;

		// Token: 0x040002AD RID: 685
		public string password;

		// Token: 0x040002AE RID: 686
		public int passwordLength;

		// Token: 0x040002AF RID: 687
		public int flags;

		// Token: 0x040002B0 RID: 688
		public string packageList;

		// Token: 0x040002B1 RID: 689
		public int packageListLength;
	}
}
