using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000161 RID: 353
	// (Invoke) Token: 0x0600137E RID: 4990
	[CLSCompliant(false)]
	[ComVisible(true)]
	public unsafe delegate void IOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP);
}
