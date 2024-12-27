using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BF RID: 191
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class NegotiateCallerNameRequest
	{
		// Token: 0x040004E1 RID: 1249
		public int messageType;

		// Token: 0x040004E2 RID: 1250
		public LUID logonId;
	}
}
