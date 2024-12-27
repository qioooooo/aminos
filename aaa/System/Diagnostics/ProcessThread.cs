using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x0200078A RID: 1930
	[Designer("System.Diagnostics.Design.ProcessThreadDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[HostProtection(SecurityAction.LinkDemand, SelfAffectingProcessMgmt = true, SelfAffectingThreading = true)]
	public class ProcessThread : Component
	{
		// Token: 0x06003B99 RID: 15257 RVA: 0x000FDDAA File Offset: 0x000FCDAA
		internal ProcessThread(bool isRemoteMachine, ThreadInfo threadInfo)
		{
			this.isRemoteMachine = isRemoteMachine;
			this.threadInfo = threadInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003B9A RID: 15258 RVA: 0x000FDDC6 File Offset: 0x000FCDC6
		[MonitoringDescription("ThreadBasePriority")]
		public int BasePriority
		{
			get
			{
				return this.threadInfo.basePriority;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003B9B RID: 15259 RVA: 0x000FDDD3 File Offset: 0x000FCDD3
		[MonitoringDescription("ThreadCurrentPriority")]
		public int CurrentPriority
		{
			get
			{
				return this.threadInfo.currentPriority;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x000FDDE0 File Offset: 0x000FCDE0
		[MonitoringDescription("ThreadId")]
		public int Id
		{
			get
			{
				return this.threadInfo.threadId;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (set) Token: 0x06003B9D RID: 15261 RVA: 0x000FDDF0 File Offset: 0x000FCDF0
		[Browsable(false)]
		public int IdealProcessor
		{
			set
			{
				SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (NativeMethods.SetThreadIdealProcessor(safeThreadHandle, value) < 0)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x000FDE34 File Offset: 0x000FCE34
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x000FDE98 File Offset: 0x000FCE98
		[MonitoringDescription("ThreadPriorityBoostEnabled")]
		public bool PriorityBoostEnabled
		{
			get
			{
				if (!this.havePriorityBoostEnabled)
				{
					SafeThreadHandle safeThreadHandle = null;
					try
					{
						safeThreadHandle = this.OpenThreadHandle(64);
						bool flag = false;
						if (!NativeMethods.GetThreadPriorityBoost(safeThreadHandle, out flag))
						{
							throw new Win32Exception();
						}
						this.priorityBoostEnabled = !flag;
						this.havePriorityBoostEnabled = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(safeThreadHandle);
					}
				}
				return this.priorityBoostEnabled;
			}
			set
			{
				SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (!NativeMethods.SetThreadPriorityBoost(safeThreadHandle, !value))
					{
						throw new Win32Exception();
					}
					this.priorityBoostEnabled = value;
					this.havePriorityBoostEnabled = true;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x000FDEEC File Offset: 0x000FCEEC
		// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x000FDF50 File Offset: 0x000FCF50
		[MonitoringDescription("ThreadPriorityLevel")]
		public ThreadPriorityLevel PriorityLevel
		{
			get
			{
				if (!this.havePriorityLevel)
				{
					SafeThreadHandle safeThreadHandle = null;
					try
					{
						safeThreadHandle = this.OpenThreadHandle(64);
						int threadPriority = NativeMethods.GetThreadPriority(safeThreadHandle);
						if (threadPriority == 2147483647)
						{
							throw new Win32Exception();
						}
						this.priorityLevel = (ThreadPriorityLevel)threadPriority;
						this.havePriorityLevel = true;
					}
					finally
					{
						ProcessThread.CloseThreadHandle(safeThreadHandle);
					}
				}
				return this.priorityLevel;
			}
			set
			{
				SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(32);
					if (!NativeMethods.SetThreadPriority(safeThreadHandle, (int)value))
					{
						throw new Win32Exception();
					}
					this.priorityLevel = value;
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x000FDF98 File Offset: 0x000FCF98
		[MonitoringDescription("ThreadPrivilegedProcessorTime")]
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().PrivilegedProcessorTime;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x000FDFAC File Offset: 0x000FCFAC
		[MonitoringDescription("ThreadStartAddress")]
		public IntPtr StartAddress
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.startAddress;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x000FDFC0 File Offset: 0x000FCFC0
		[MonitoringDescription("ThreadStartTime")]
		public DateTime StartTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().StartTime;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x000FDFD4 File Offset: 0x000FCFD4
		[MonitoringDescription("ThreadThreadState")]
		public ThreadState ThreadState
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.threadInfo.threadState;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x000FDFE8 File Offset: 0x000FCFE8
		[MonitoringDescription("ThreadTotalProcessorTime")]
		public TimeSpan TotalProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().TotalProcessorTime;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x000FDFFC File Offset: 0x000FCFFC
		[MonitoringDescription("ThreadUserProcessorTime")]
		public TimeSpan UserProcessorTime
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				return this.GetThreadTimes().UserProcessorTime;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x000FE010 File Offset: 0x000FD010
		[MonitoringDescription("ThreadWaitReason")]
		public ThreadWaitReason WaitReason
		{
			get
			{
				this.EnsureState(ProcessThread.State.IsNt);
				if (this.threadInfo.threadState != ThreadState.Wait)
				{
					throw new InvalidOperationException(SR.GetString("WaitReasonUnavailable"));
				}
				return this.threadInfo.threadWaitReason;
			}
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000FE042 File Offset: 0x000FD042
		private static void CloseThreadHandle(SafeThreadHandle handle)
		{
			if (handle != null)
			{
				handle.Close();
			}
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000FE050 File Offset: 0x000FD050
		private void EnsureState(ProcessThread.State state)
		{
			if ((state & ProcessThread.State.IsLocal) != (ProcessThread.State)0 && this.isRemoteMachine)
			{
				throw new NotSupportedException(SR.GetString("NotSupportedRemoteThread"));
			}
			if ((state & ProcessThread.State.IsNt) != (ProcessThread.State)0 && Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000FE09C File Offset: 0x000FD09C
		private SafeThreadHandle OpenThreadHandle(int access)
		{
			this.EnsureState(ProcessThread.State.IsLocal);
			return ProcessManager.OpenThread(this.threadInfo.threadId, access);
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x000FE0B6 File Offset: 0x000FD0B6
		public void ResetIdealProcessor()
		{
			this.IdealProcessor = 32;
		}

		// Token: 0x17000E0A RID: 3594
		// (set) Token: 0x06003BAD RID: 15277 RVA: 0x000FE0C0 File Offset: 0x000FD0C0
		[Browsable(false)]
		public IntPtr ProcessorAffinity
		{
			set
			{
				SafeThreadHandle safeThreadHandle = null;
				try
				{
					safeThreadHandle = this.OpenThreadHandle(96);
					if (NativeMethods.SetThreadAffinityMask(safeThreadHandle, new HandleRef(this, value)) == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					ProcessThread.CloseThreadHandle(safeThreadHandle);
				}
			}
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x000FE110 File Offset: 0x000FD110
		private ProcessThreadTimes GetThreadTimes()
		{
			ProcessThreadTimes processThreadTimes = new ProcessThreadTimes();
			SafeThreadHandle safeThreadHandle = null;
			try
			{
				safeThreadHandle = this.OpenThreadHandle(64);
				if (!NativeMethods.GetThreadTimes(safeThreadHandle, out processThreadTimes.create, out processThreadTimes.exit, out processThreadTimes.kernel, out processThreadTimes.user))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				ProcessThread.CloseThreadHandle(safeThreadHandle);
			}
			return processThreadTimes;
		}

		// Token: 0x0400343D RID: 13373
		private ThreadInfo threadInfo;

		// Token: 0x0400343E RID: 13374
		private bool isRemoteMachine;

		// Token: 0x0400343F RID: 13375
		private bool priorityBoostEnabled;

		// Token: 0x04003440 RID: 13376
		private bool havePriorityBoostEnabled;

		// Token: 0x04003441 RID: 13377
		private ThreadPriorityLevel priorityLevel;

		// Token: 0x04003442 RID: 13378
		private bool havePriorityLevel;

		// Token: 0x0200078B RID: 1931
		private enum State
		{
			// Token: 0x04003444 RID: 13380
			IsLocal = 2,
			// Token: 0x04003445 RID: 13381
			IsNt = 4
		}
	}
}
