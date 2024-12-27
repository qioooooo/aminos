using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000242 RID: 578
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBBINDING
	{
		// Token: 0x0600206A RID: 8298 RVA: 0x00262AC0 File Offset: 0x00261EC0
		internal tagDBBINDING()
		{
		}

		// Token: 0x040014A6 RID: 5286
		internal IntPtr iOrdinal;

		// Token: 0x040014A7 RID: 5287
		internal IntPtr obValue;

		// Token: 0x040014A8 RID: 5288
		internal IntPtr obLength;

		// Token: 0x040014A9 RID: 5289
		internal IntPtr obStatus;

		// Token: 0x040014AA RID: 5290
		internal IntPtr pTypeInfo;

		// Token: 0x040014AB RID: 5291
		internal IntPtr pObject;

		// Token: 0x040014AC RID: 5292
		internal IntPtr pBindExt;

		// Token: 0x040014AD RID: 5293
		internal int dwPart;

		// Token: 0x040014AE RID: 5294
		internal int dwMemOwner;

		// Token: 0x040014AF RID: 5295
		internal int eParamIO;

		// Token: 0x040014B0 RID: 5296
		internal IntPtr cbMaxLen;

		// Token: 0x040014B1 RID: 5297
		internal int dwFlags;

		// Token: 0x040014B2 RID: 5298
		internal short wType;

		// Token: 0x040014B3 RID: 5299
		internal byte bPrecision;

		// Token: 0x040014B4 RID: 5300
		internal byte bScale;
	}
}
