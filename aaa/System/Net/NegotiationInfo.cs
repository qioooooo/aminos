using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000547 RID: 1351
	internal struct NegotiationInfo
	{
		// Token: 0x04002819 RID: 10265
		internal IntPtr PackageInfo;

		// Token: 0x0400281A RID: 10266
		internal uint NegotiationState;

		// Token: 0x0400281B RID: 10267
		internal static readonly int Size = Marshal.SizeOf(typeof(NegotiationInfo));

		// Token: 0x0400281C RID: 10268
		internal static readonly int NegotiationStateOffest = (int)Marshal.OffsetOf(typeof(NegotiationInfo), "NegotiationState");
	}
}
