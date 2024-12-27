using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000539 RID: 1337
	internal sealed class Semaphore : WaitHandle
	{
		// Token: 0x060028E0 RID: 10464 RVA: 0x000A9D54 File Offset: 0x000A8D54
		internal Semaphore(int initialCount, int maxCount)
		{
			lock (this)
			{
				this.Handle = UnsafeNclNativeMethods.CreateSemaphore(IntPtr.Zero, initialCount, maxCount, IntPtr.Zero);
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000A9DA0 File Offset: 0x000A8DA0
		internal bool ReleaseSemaphore()
		{
			return UnsafeNclNativeMethods.ReleaseSemaphore(this.Handle, 1, IntPtr.Zero);
		}
	}
}
