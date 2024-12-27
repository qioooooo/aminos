using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000549 RID: 1353
	internal struct SecurityPackageInfo
	{
		// Token: 0x04002822 RID: 10274
		internal int Capabilities;

		// Token: 0x04002823 RID: 10275
		internal short Version;

		// Token: 0x04002824 RID: 10276
		internal short RPCID;

		// Token: 0x04002825 RID: 10277
		internal int MaxToken;

		// Token: 0x04002826 RID: 10278
		internal IntPtr Name;

		// Token: 0x04002827 RID: 10279
		internal IntPtr Comment;

		// Token: 0x04002828 RID: 10280
		internal static readonly int Size = Marshal.SizeOf(typeof(SecurityPackageInfo));

		// Token: 0x04002829 RID: 10281
		internal static readonly int NameOffest = (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name");
	}
}
