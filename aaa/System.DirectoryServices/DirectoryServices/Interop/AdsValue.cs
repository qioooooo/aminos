using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000055 RID: 85
	[StructLayout(LayoutKind.Explicit)]
	internal struct AdsValue
	{
		// Token: 0x04000268 RID: 616
		[FieldOffset(0)]
		public int dwType;

		// Token: 0x04000269 RID: 617
		[FieldOffset(4)]
		internal int pad;

		// Token: 0x0400026A RID: 618
		[FieldOffset(8)]
		public Ads_Pointer pointer;

		// Token: 0x0400026B RID: 619
		[FieldOffset(8)]
		public Ads_OctetString octetString;

		// Token: 0x0400026C RID: 620
		[FieldOffset(8)]
		public Ads_Generic generic;
	}
}
