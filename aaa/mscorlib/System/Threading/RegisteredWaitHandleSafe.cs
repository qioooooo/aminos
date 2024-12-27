using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Threading
{
	// Token: 0x0200015B RID: 347
	internal sealed class RegisteredWaitHandleSafe : CriticalFinalizerObject
	{
		// Token: 0x0600135C RID: 4956 RVA: 0x00035308 File Offset: 0x00034308
		internal RegisteredWaitHandleSafe()
		{
			this.registeredWaitHandle = RegisteredWaitHandleSafe.InvalidHandle;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0003531B File Offset: 0x0003431B
		internal IntPtr GetHandle()
		{
			return this.registeredWaitHandle;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00035323 File Offset: 0x00034323
		internal void SetHandle(IntPtr handle)
		{
			this.registeredWaitHandle = handle;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0003532C File Offset: 0x0003432C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void SetWaitObject(WaitHandle waitObject)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.m_internalWaitObject = waitObject;
				if (waitObject != null)
				{
					this.m_internalWaitObject.SafeWaitHandle.DangerousAddRef(ref this.bReleaseNeeded);
				}
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00035374 File Offset: 0x00034374
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool Unregister(WaitHandle waitObject)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				bool flag2 = false;
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag2 = true;
						try
						{
							if (this.ValidHandle())
							{
								flag = RegisteredWaitHandleSafe.UnregisterWaitNative(this.GetHandle(), (waitObject == null) ? null : waitObject.SafeWaitHandle);
								if (flag)
								{
									if (this.bReleaseNeeded)
									{
										this.m_internalWaitObject.SafeWaitHandle.DangerousRelease();
										this.bReleaseNeeded = false;
									}
									this.SetHandle(RegisteredWaitHandleSafe.InvalidHandle);
									this.m_internalWaitObject = null;
									GC.SuppressFinalize(this);
								}
							}
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag2);
			}
			return flag;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0003542C File Offset: 0x0003442C
		private bool ValidHandle()
		{
			return this.registeredWaitHandle != RegisteredWaitHandleSafe.InvalidHandle && this.registeredWaitHandle != IntPtr.Zero;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00035454 File Offset: 0x00034454
		protected override void Finalize()
		{
			try
			{
				bool flag = false;
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							if (this.ValidHandle())
							{
								RegisteredWaitHandleSafe.WaitHandleCleanupNative(this.registeredWaitHandle);
								if (this.bReleaseNeeded)
								{
									this.m_internalWaitObject.SafeWaitHandle.DangerousRelease();
									this.bReleaseNeeded = false;
								}
								this.SetHandle(RegisteredWaitHandleSafe.InvalidHandle);
								this.m_internalWaitObject = null;
							}
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag);
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001363 RID: 4963
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WaitHandleCleanupNative(IntPtr handle);

		// Token: 0x06001364 RID: 4964
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UnregisterWaitNative(IntPtr handle, SafeHandle waitObject);

		// Token: 0x04000667 RID: 1639
		private static readonly IntPtr InvalidHandle = Win32Native.INVALID_HANDLE_VALUE;

		// Token: 0x04000668 RID: 1640
		private IntPtr registeredWaitHandle;

		// Token: 0x04000669 RID: 1641
		private WaitHandle m_internalWaitObject;

		// Token: 0x0400066A RID: 1642
		private bool bReleaseNeeded;

		// Token: 0x0400066B RID: 1643
		private int m_lock;
	}
}
