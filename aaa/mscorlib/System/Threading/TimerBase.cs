using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200016C RID: 364
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	internal sealed class TimerBase : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x060013BB RID: 5051 RVA: 0x00035C50 File Offset: 0x00034C50
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
							this.DeleteTimerNative(null);
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

		// Token: 0x060013BC RID: 5052 RVA: 0x00035CB0 File Offset: 0x00034CB0
		internal void AddTimer(TimerCallback callback, object state, uint dueTime, uint period, ref StackCrawlMark stackMark)
		{
			if (callback != null)
			{
				_TimerCallback timerCallback = new _TimerCallback(callback, state, ref stackMark);
				state = timerCallback;
				this.AddTimerNative(state, dueTime, period, ref stackMark);
				this.timerDeleted = 0;
				return;
			}
			throw new ArgumentNullException("TimerCallback");
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00035CEC File Offset: 0x00034CEC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool ChangeTimer(uint dueTime, uint period)
		{
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag2 = true;
						try
						{
							if (this.timerDeleted != 0)
							{
								throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
							}
							flag = this.ChangeTimerNative(dueTime, period);
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

		// Token: 0x060013BE RID: 5054 RVA: 0x00035D68 File Offset: 0x00034D68
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool Dispose(WaitHandle notifyObject)
		{
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag2 = true;
						try
						{
							flag = this.DeleteTimerNative(notifyObject.SafeWaitHandle);
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag2);
				GC.SuppressFinalize(this);
			}
			return flag;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00035DD8 File Offset: 0x00034DD8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Dispose()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				do
				{
					if (Interlocked.CompareExchange(ref this.m_lock, 1, 0) == 0)
					{
						flag = true;
						try
						{
							this.DeleteTimerNative(null);
						}
						finally
						{
							this.m_lock = 0;
						}
					}
					Thread.SpinWait(1);
				}
				while (!flag);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060013C0 RID: 5056
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTimerNative(object state, uint dueTime, uint period, ref StackCrawlMark stackMark);

		// Token: 0x060013C1 RID: 5057
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool ChangeTimerNative(uint dueTime, uint period);

		// Token: 0x060013C2 RID: 5058
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool DeleteTimerNative(SafeHandle notifyObject);

		// Token: 0x0400068D RID: 1677
		private IntPtr timerHandle;

		// Token: 0x0400068E RID: 1678
		private IntPtr delegateInfo;

		// Token: 0x0400068F RID: 1679
		private int timerDeleted;

		// Token: 0x04000690 RID: 1680
		private int m_lock;
	}
}
