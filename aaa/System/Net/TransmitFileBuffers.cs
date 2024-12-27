using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004FE RID: 1278
	[StructLayout(LayoutKind.Sequential)]
	internal class TransmitFileBuffers
	{
		// Token: 0x04002716 RID: 10006
		internal IntPtr preBuffer;

		// Token: 0x04002717 RID: 10007
		internal int preBufferLength;

		// Token: 0x04002718 RID: 10008
		internal IntPtr postBuffer;

		// Token: 0x04002719 RID: 10009
		internal int postBufferLength;
	}
}
