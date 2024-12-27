using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Interop
{
	// Token: 0x0200005A RID: 90
	[ComVisible(false)]
	internal class NativeMethods
	{
		// Token: 0x0200005B RID: 91
		public enum AuthenticationModes
		{
			// Token: 0x0400027D RID: 637
			SecureAuthentication = 1,
			// Token: 0x0400027E RID: 638
			UseEncryption,
			// Token: 0x0400027F RID: 639
			UseSSL = 2,
			// Token: 0x04000280 RID: 640
			ReadonlyServer = 4,
			// Token: 0x04000281 RID: 641
			NoAuthentication = 16,
			// Token: 0x04000282 RID: 642
			FastBind = 32,
			// Token: 0x04000283 RID: 643
			UseSigning = 64,
			// Token: 0x04000284 RID: 644
			UseSealing = 128,
			// Token: 0x04000285 RID: 645
			UseDelegation = 256,
			// Token: 0x04000286 RID: 646
			UseServerBinding = 512
		}
	}
}
