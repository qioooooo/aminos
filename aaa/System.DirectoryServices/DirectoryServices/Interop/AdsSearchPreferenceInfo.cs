using System;

namespace System.DirectoryServices.Interop
{
	// Token: 0x0200004D RID: 77
	internal struct AdsSearchPreferenceInfo
	{
		// Token: 0x04000223 RID: 547
		public int dwSearchPref;

		// Token: 0x04000224 RID: 548
		internal int pad;

		// Token: 0x04000225 RID: 549
		public AdsValue vValue;

		// Token: 0x04000226 RID: 550
		public int dwStatus;

		// Token: 0x04000227 RID: 551
		internal int pad2;
	}
}
