using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005F RID: 95
	[StructLayout(LayoutKind.Sequential)]
	internal class SECURITY_ATTRIBUTES
	{
		// Token: 0x04000217 RID: 535
		internal int nLength;

		// Token: 0x04000218 RID: 536
		internal IntPtr lpSecurityDescriptor = IntPtr.Zero;

		// Token: 0x04000219 RID: 537
		internal int bInheritHandle;
	}
}
