using System;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000726 RID: 1830
	internal struct MessageData
	{
		// Token: 0x040020DF RID: 8415
		internal IntPtr pFrame;

		// Token: 0x040020E0 RID: 8416
		internal IntPtr pMethodDesc;

		// Token: 0x040020E1 RID: 8417
		internal IntPtr pDelegateMD;

		// Token: 0x040020E2 RID: 8418
		internal IntPtr pSig;

		// Token: 0x040020E3 RID: 8419
		internal IntPtr thGoverningType;

		// Token: 0x040020E4 RID: 8420
		internal int iFlags;
	}
}
