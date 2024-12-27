using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000527 RID: 1319
	internal class SafeNativeOverlapped : SafeHandle
	{
		// Token: 0x0600286D RID: 10349 RVA: 0x000A7687 File Offset: 0x000A6687
		internal SafeNativeOverlapped()
			: this(IntPtr.Zero)
		{
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000A7694 File Offset: 0x000A6694
		internal unsafe SafeNativeOverlapped(NativeOverlapped* handle)
			: this((IntPtr)((void*)handle))
		{
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000A76A2 File Offset: 0x000A66A2
		internal SafeNativeOverlapped(IntPtr handle)
			: base(IntPtr.Zero, true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x000A76B7 File Offset: 0x000A66B7
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000A76CC File Offset: 0x000A66CC
		protected unsafe override bool ReleaseHandle()
		{
			IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
			if (intPtr != IntPtr.Zero && !NclUtilities.HasShutdownStarted)
			{
				Overlapped.Free((NativeOverlapped*)(void*)intPtr);
			}
			return true;
		}

		// Token: 0x04002784 RID: 10116
		internal static readonly SafeNativeOverlapped Zero = new SafeNativeOverlapped();
	}
}
