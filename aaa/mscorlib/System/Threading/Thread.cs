using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Threading
{
	// Token: 0x02000155 RID: 341
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_Thread))]
	public sealed class Thread : CriticalFinalizerObject, _Thread
	{
		// Token: 0x060012BC RID: 4796 RVA: 0x000344C3 File Offset: 0x000334C3
		public Thread(ThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000344E1 File Offset: 0x000334E1
		public Thread(ThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00034518 File Offset: 0x00033518
		public Thread(ParameterizedThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00034536 File Offset: 0x00033536
		public Thread(ParameterizedThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x060012C0 RID: 4800
		[ComVisible(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern int GetHashCode();

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x0003456D File Offset: 0x0003356D
		public int ManagedThreadId
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_ManagedThreadId;
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00034578 File Offset: 0x00033578
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Start()
		{
			this.StartupSetApartmentStateInternal();
			if (this.m_Delegate != null)
			{
				ThreadHelper threadHelper = (ThreadHelper)this.m_Delegate.Target;
				ExecutionContext executionContext = ExecutionContext.Capture();
				ExecutionContext.ClearSyncContext(executionContext);
				threadHelper.SetExecutionContextHelper(executionContext);
			}
			IPrincipal principal = CallContext.Principal;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this.StartInternal(principal, ref stackCrawlMark);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x000345C9 File Offset: 0x000335C9
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public void Start(object parameter)
		{
			if (this.m_Delegate is ThreadStart)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadWrongThreadStart"));
			}
			this.m_ThreadStartArg = parameter;
			this.Start();
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000345F5 File Offset: 0x000335F5
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext GetExecutionContextNoCreate()
		{
			return this.m_ExecutionContext;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x000345FD File Offset: 0x000335FD
		public ExecutionContext ExecutionContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (this.m_ExecutionContext == null && this == Thread.CurrentThread)
				{
					this.m_ExecutionContext = new ExecutionContext();
					this.m_ExecutionContext.Thread = this;
				}
				return this.m_ExecutionContext;
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0003462C File Offset: 0x0003362C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetExecutionContext(ExecutionContext value)
		{
			this.m_ExecutionContext = value;
			if (value != null)
			{
				this.m_ExecutionContext.Thread = this;
			}
		}

		// Token: 0x060012C7 RID: 4807
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StartInternal(IPrincipal principal, ref StackCrawlMark stackMark);

		// Token: 0x060012C8 RID: 4808 RVA: 0x00034644 File Offset: 0x00033644
		[Obsolete("Thread.SetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public void SetCompressedStack(CompressedStack stack)
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadAPIsNotSupported"));
		}

		// Token: 0x060012C9 RID: 4809
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr SetAppDomainStack(SafeCompressedStackHandle csHandle);

		// Token: 0x060012CA RID: 4810
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RestoreAppDomainStack(IntPtr appDomainStack);

		// Token: 0x060012CB RID: 4811 RVA: 0x00034655 File Offset: 0x00033655
		[Obsolete("Thread.GetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public CompressedStack GetCompressedStack()
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ThreadAPIsNotSupported"));
		}

		// Token: 0x060012CC RID: 4812
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr InternalGetCurrentThread();

		// Token: 0x060012CD RID: 4813 RVA: 0x00034666 File Offset: 0x00033666
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Abort(object stateInfo)
		{
			this.AbortReason = stateInfo;
			this.AbortInternal();
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00034675 File Offset: 0x00033675
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Abort()
		{
			this.AbortInternal();
		}

		// Token: 0x060012CF RID: 4815
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AbortInternal();

		// Token: 0x060012D0 RID: 4816 RVA: 0x00034680 File Offset: 0x00033680
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public static void ResetAbort()
		{
			Thread currentThread = Thread.CurrentThread;
			if ((currentThread.ThreadState & ThreadState.AbortRequested) == ThreadState.Running)
			{
				throw new ThreadStateException(Environment.GetResourceString("ThreadState_NoAbortRequested"));
			}
			currentThread.ResetAbortNative();
			currentThread.ClearAbortReason();
		}

		// Token: 0x060012D1 RID: 4817
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResetAbortNative();

		// Token: 0x060012D2 RID: 4818 RVA: 0x000346BD File Offset: 0x000336BD
		[Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Suspend()
		{
			this.SuspendInternal();
		}

		// Token: 0x060012D3 RID: 4819
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SuspendInternal();

		// Token: 0x060012D4 RID: 4820 RVA: 0x000346C5 File Offset: 0x000336C5
		[Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Resume()
		{
			this.ResumeInternal();
		}

		// Token: 0x060012D5 RID: 4821
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResumeInternal();

		// Token: 0x060012D6 RID: 4822 RVA: 0x000346CD File Offset: 0x000336CD
		[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
		public void Interrupt()
		{
			this.InterruptInternal();
		}

		// Token: 0x060012D7 RID: 4823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InterruptInternal();

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x000346D5 File Offset: 0x000336D5
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x000346DD File Offset: 0x000336DD
		public ThreadPriority Priority
		{
			get
			{
				return (ThreadPriority)this.GetPriorityNative();
			}
			[HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
			set
			{
				this.SetPriorityNative((int)value);
			}
		}

		// Token: 0x060012DA RID: 4826
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPriorityNative();

		// Token: 0x060012DB RID: 4827
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPriorityNative(int priority);

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x000346E6 File Offset: 0x000336E6
		public bool IsAlive
		{
			get
			{
				return this.IsAliveNative();
			}
		}

		// Token: 0x060012DD RID: 4829
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsAliveNative();

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000346EE File Offset: 0x000336EE
		public bool IsThreadPoolThread
		{
			get
			{
				return this.IsThreadpoolThreadNative();
			}
		}

		// Token: 0x060012DF RID: 4831
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsThreadpoolThreadNative();

		// Token: 0x060012E0 RID: 4832
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void JoinInternal();

		// Token: 0x060012E1 RID: 4833 RVA: 0x000346F6 File Offset: 0x000336F6
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public void Join()
		{
			this.JoinInternal();
		}

		// Token: 0x060012E2 RID: 4834
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool JoinInternal(int millisecondsTimeout);

		// Token: 0x060012E3 RID: 4835 RVA: 0x000346FE File Offset: 0x000336FE
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public bool Join(int millisecondsTimeout)
		{
			return this.JoinInternal(millisecondsTimeout);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00034708 File Offset: 0x00033708
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public bool Join(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return this.Join((int)num);
		}

		// Token: 0x060012E5 RID: 4837
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SleepInternal(int millisecondsTimeout);

		// Token: 0x060012E6 RID: 4838 RVA: 0x00034749 File Offset: 0x00033749
		public static void Sleep(int millisecondsTimeout)
		{
			Thread.SleepInternal(millisecondsTimeout);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00034754 File Offset: 0x00033754
		public static void Sleep(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			Thread.Sleep((int)num);
		}

		// Token: 0x060012E8 RID: 4840
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SpinWaitInternal(int iterations);

		// Token: 0x060012E9 RID: 4841 RVA: 0x00034794 File Offset: 0x00033794
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		public static void SpinWait(int iterations)
		{
			Thread.SpinWaitInternal(iterations);
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x0003479C File Offset: 0x0003379C
		public static Thread CurrentThread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				Thread thread = Thread.GetFastCurrentThreadNative();
				if (thread == null)
				{
					thread = Thread.GetCurrentThreadNative();
				}
				return thread;
			}
		}

		// Token: 0x060012EB RID: 4843
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Thread GetCurrentThreadNative();

		// Token: 0x060012EC RID: 4844
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Thread GetFastCurrentThreadNative();

		// Token: 0x060012ED RID: 4845 RVA: 0x000347BC File Offset: 0x000337BC
		private void SetStartHelper(Delegate start, int maxStackSize)
		{
			ThreadHelper threadHelper = new ThreadHelper(start);
			if (start is ThreadStart)
			{
				this.SetStart(new ThreadStart(threadHelper.ThreadStart), maxStackSize);
				return;
			}
			this.SetStart(new ParameterizedThreadStart(threadHelper.ThreadStart), maxStackSize);
		}

		// Token: 0x060012EE RID: 4846
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStart(Delegate start, int maxStackSize);

		// Token: 0x060012EF RID: 4847 RVA: 0x00034800 File Offset: 0x00033800
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		~Thread()
		{
			this.InternalFinalize();
		}

		// Token: 0x060012F0 RID: 4848
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalFinalize();

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0003482C File Offset: 0x0003382C
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x00034834 File Offset: 0x00033834
		public bool IsBackground
		{
			get
			{
				return this.IsBackgroundNative();
			}
			[HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
			set
			{
				this.SetBackgroundNative(value);
			}
		}

		// Token: 0x060012F3 RID: 4851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsBackgroundNative();

		// Token: 0x060012F4 RID: 4852
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBackgroundNative(bool isBackground);

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0003483D File Offset: 0x0003383D
		public ThreadState ThreadState
		{
			get
			{
				return (ThreadState)this.GetThreadStateNative();
			}
		}

		// Token: 0x060012F6 RID: 4854
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetThreadStateNative();

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00034845 File Offset: 0x00033845
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x0003484D File Offset: 0x0003384D
		[Obsolete("The ApartmentState property has been deprecated.  Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.", false)]
		public ApartmentState ApartmentState
		{
			get
			{
				return (ApartmentState)this.GetApartmentStateNative();
			}
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
			set
			{
				this.SetApartmentStateNative((int)value, true);
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00034858 File Offset: 0x00033858
		public ApartmentState GetApartmentState()
		{
			return (ApartmentState)this.GetApartmentStateNative();
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00034860 File Offset: 0x00033860
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
		public bool TrySetApartmentState(ApartmentState state)
		{
			return this.SetApartmentStateHelper(state, false);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0003486C File Offset: 0x0003386C
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SelfAffectingThreading = true)]
		public void SetApartmentState(ApartmentState state)
		{
			if (!this.SetApartmentStateHelper(state, true))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ApartmentStateSwitchFailed"));
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00034898 File Offset: 0x00033898
		private bool SetApartmentStateHelper(ApartmentState state, bool fireMDAOnMismatch)
		{
			ApartmentState apartmentState = (ApartmentState)this.SetApartmentStateNative((int)state, fireMDAOnMismatch);
			return (state == ApartmentState.Unknown && apartmentState == ApartmentState.MTA) || apartmentState == state;
		}

		// Token: 0x060012FD RID: 4861
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetApartmentStateNative();

		// Token: 0x060012FE RID: 4862
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int SetApartmentStateNative(int state, bool fireMDAOnMismatch);

		// Token: 0x060012FF RID: 4863
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int StartupSetApartmentStateInternal();

		// Token: 0x06001300 RID: 4864 RVA: 0x000348BF File Offset: 0x000338BF
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return Thread.LocalDataStoreManager.AllocateDataSlot();
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000348CB File Offset: 0x000338CB
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.AllocateNamedDataSlot(name);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000348D8 File Offset: 0x000338D8
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.GetNamedDataSlot(name);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000348E5 File Offset: 0x000338E5
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static void FreeNamedDataSlot(string name)
		{
			Thread.LocalDataStoreManager.FreeNamedDataSlot(name);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000348F4 File Offset: 0x000338F4
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static object GetData(LocalDataStoreSlot slot)
		{
			Thread.LocalDataStoreManager.ValidateSlot(slot);
			LocalDataStore domainLocalStore = Thread.GetDomainLocalStore();
			if (domainLocalStore == null)
			{
				return null;
			}
			return domainLocalStore.GetData(slot);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00034920 File Offset: 0x00033920
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			LocalDataStore localDataStore = Thread.GetDomainLocalStore();
			if (localDataStore == null)
			{
				localDataStore = Thread.LocalDataStoreManager.CreateLocalDataStore();
				Thread.SetDomainLocalStore(localDataStore);
			}
			localDataStore.SetData(slot, data);
		}

		// Token: 0x06001306 RID: 4870
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LocalDataStore GetDomainLocalStore();

		// Token: 0x06001307 RID: 4871
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDomainLocalStore(LocalDataStore dls);

		// Token: 0x06001308 RID: 4872 RVA: 0x0003494F File Offset: 0x0003394F
		private static void RemoveDomainLocalStore(LocalDataStore dls)
		{
			if (dls != null)
			{
				Thread.LocalDataStoreManager.DeleteLocalDataStore(dls);
			}
		}

		// Token: 0x06001309 RID: 4873
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeGetSafeCulture(Thread t, int appDomainId, bool isUI, ref CultureInfo safeCulture);

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00034960 File Offset: 0x00033960
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x00034998 File Offset: 0x00033998
		public CultureInfo CurrentUICulture
		{
			get
			{
				if (this.m_CurrentUICulture == null)
				{
					return CultureInfo.UserDefaultUICulture;
				}
				CultureInfo cultureInfo = null;
				if (!Thread.nativeGetSafeCulture(this, Thread.GetDomainID(), true, ref cultureInfo) || cultureInfo == null)
				{
					return CultureInfo.UserDefaultUICulture;
				}
				return cultureInfo;
			}
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				CultureInfo.VerifyCultureName(value, true);
				if (!Thread.nativeSetThreadUILocale(value.LCID))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidResourceCultureName", new object[] { value.Name }));
				}
				value.StartCrossDomainTracking();
				this.m_CurrentUICulture = value;
			}
		}

		// Token: 0x0600130C RID: 4876
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeSetThreadUILocale(int LCID);

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x000349F8 File Offset: 0x000339F8
		// (set) Token: 0x0600130E RID: 4878 RVA: 0x00034A2F File Offset: 0x00033A2F
		public CultureInfo CurrentCulture
		{
			get
			{
				if (this.m_CurrentCulture == null)
				{
					return CultureInfo.UserDefaultCulture;
				}
				CultureInfo cultureInfo = null;
				if (!Thread.nativeGetSafeCulture(this, Thread.GetDomainID(), false, ref cultureInfo) || cultureInfo == null)
				{
					return CultureInfo.UserDefaultCulture;
				}
				return cultureInfo;
			}
			[SecurityPermission(SecurityAction.Demand, ControlThread = true)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				CultureInfo.CheckNeutral(value);
				CultureInfo.nativeSetThreadLocale(value.LCID);
				value.StartCrossDomainTracking();
				this.m_CurrentCulture = value;
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00034A60 File Offset: 0x00033A60
		private int ReserveSlot()
		{
			if (this.m_ThreadStaticsBuckets == null)
			{
				object[][] array = new object[1][];
				Thread.SetIsThreadStaticsArray(array);
				array[0] = new object[32];
				Thread.SetIsThreadStaticsArray(array[0]);
				int[] array2 = new int[array.Length * 32 / 32];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = -1;
				}
				array2[0] &= -2;
				array2[0] &= -3;
				this.m_ThreadStaticsBits = array2;
				this.m_ThreadStaticsBuckets = array;
				return 1;
			}
			int num = this.FindSlot();
			if (num == 0)
			{
				int num2 = this.m_ThreadStaticsBuckets.Length;
				int num3 = this.m_ThreadStaticsBits.Length;
				int num4 = this.m_ThreadStaticsBuckets.Length + 1;
				object[][] array3 = new object[num4][];
				Thread.SetIsThreadStaticsArray(array3);
				int num5 = num4 * 32 / 32;
				int[] array4 = new int[num5];
				Array.Copy(this.m_ThreadStaticsBuckets, array3, this.m_ThreadStaticsBuckets.Length);
				for (int j = num2; j < num4; j++)
				{
					array3[j] = new object[32];
					Thread.SetIsThreadStaticsArray(array3[j]);
				}
				Array.Copy(this.m_ThreadStaticsBits, array4, this.m_ThreadStaticsBits.Length);
				for (int k = num3; k < num5; k++)
				{
					array4[k] = -1;
				}
				array4[num3] &= -2;
				this.m_ThreadStaticsBits = array4;
				this.m_ThreadStaticsBuckets = array3;
				return num2 * 32;
			}
			return num;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00034BD8 File Offset: 0x00033BD8
		private int FindSlot()
		{
			int num = 0;
			bool flag = false;
			if (this.m_ThreadStaticsBits.Length != 0 && this.m_ThreadStaticsBits.Length != this.m_ThreadStaticsBuckets.Length * 32 / 32)
			{
				return 0;
			}
			int i;
			for (i = 0; i < this.m_ThreadStaticsBits.Length; i++)
			{
				int num2 = this.m_ThreadStaticsBits[i];
				if (num2 != 0)
				{
					if ((num2 & 65535) != 0)
					{
						num2 &= 65535;
					}
					else
					{
						num2 = (num2 >> 16) & 65535;
						num += 16;
					}
					if ((num2 & 255) != 0)
					{
						num2 &= 255;
					}
					else
					{
						num += 8;
						num2 = (num2 >> 8) & 255;
					}
					int j;
					for (j = 0; j < 8; j++)
					{
						if ((num2 & (1 << j)) != 0)
						{
							flag = true;
							break;
						}
					}
					num += j;
					this.m_ThreadStaticsBits[i] &= ~(1 << num);
					break;
				}
			}
			if (flag)
			{
				num += 32 * i;
			}
			return num;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x00034CCB File Offset: 0x00033CCB
		public static Context CurrentContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Thread.CurrentThread.GetCurrentContextInternal();
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00034CD7 File Offset: 0x00033CD7
		internal Context GetCurrentContextInternal()
		{
			if (this.m_Context == null)
			{
				this.m_Context = Context.DefaultContext;
			}
			return this.m_Context;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00034CF2 File Offset: 0x00033CF2
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		internal LogicalCallContext GetLogicalCallContext()
		{
			return this.ExecutionContext.LogicalCallContext;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00034D00 File Offset: 0x00033D00
		[HostProtection(SecurityAction.LinkDemand, SharedState = true, ExternalThreading = true)]
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			LogicalCallContext logicalCallContext = this.ExecutionContext.LogicalCallContext;
			this.ExecutionContext.LogicalCallContext = callCtx;
			return logicalCallContext;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00034D26 File Offset: 0x00033D26
		internal IllogicalCallContext GetIllogicalCallContext()
		{
			return this.ExecutionContext.IllogicalCallContext;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x00034D34 File Offset: 0x00033D34
		// (set) Token: 0x06001317 RID: 4887 RVA: 0x00034D84 File Offset: 0x00033D84
		public static IPrincipal CurrentPrincipal
		{
			get
			{
				IPrincipal principal2;
				lock (Thread.CurrentThread)
				{
					IPrincipal principal = CallContext.Principal;
					if (principal == null)
					{
						principal = Thread.GetDomain().GetThreadPrincipal();
						CallContext.Principal = principal;
					}
					principal2 = principal;
				}
				return principal2;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
			set
			{
				CallContext.Principal = value;
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00034D8C File Offset: 0x00033D8C
		private void SetPrincipalInternal(IPrincipal principal)
		{
			this.GetLogicalCallContext().SecurityData.Principal = principal;
		}

		// Token: 0x06001319 RID: 4889
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Context GetContextInternal(IntPtr id);

		// Token: 0x0600131A RID: 4890
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object InternalCrossContextCallback(Context ctx, IntPtr ctxID, int appDomainID, InternalCrossContextDelegate ftnToCall, object[] args);

		// Token: 0x0600131B RID: 4891 RVA: 0x00034D9F File Offset: 0x00033D9F
		internal object InternalCrossContextCallback(Context ctx, InternalCrossContextDelegate ftnToCall, object[] args)
		{
			return this.InternalCrossContextCallback(ctx, ctx.InternalContextID, 0, ftnToCall, args);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00034DB1 File Offset: 0x00033DB1
		private static object CompleteCrossContextCallback(InternalCrossContextDelegate ftnToCall, object[] args)
		{
			return ftnToCall(args);
		}

		// Token: 0x0600131D RID: 4893
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain GetDomainInternal();

		// Token: 0x0600131E RID: 4894
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain GetFastDomainInternal();

		// Token: 0x0600131F RID: 4895 RVA: 0x00034DBC File Offset: 0x00033DBC
		public static AppDomain GetDomain()
		{
			if (Thread.CurrentThread.m_Context == null)
			{
				AppDomain appDomain = Thread.GetFastDomainInternal();
				if (appDomain == null)
				{
					appDomain = Thread.GetDomainInternal();
				}
				return appDomain;
			}
			return Thread.CurrentThread.m_Context.AppDomain;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00034DF5 File Offset: 0x00033DF5
		public static int GetDomainID()
		{
			return Thread.GetDomain().GetId();
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00034E01 File Offset: 0x00033E01
		// (set) Token: 0x06001322 RID: 4898 RVA: 0x00034E0C File Offset: 0x00033E0C
		public string Name
		{
			get
			{
				return this.m_Name;
			}
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			set
			{
				lock (this)
				{
					if (this.m_Name != null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_WriteOnce"));
					}
					this.m_Name = value;
					Thread.InformThreadNameChangeEx(this, this.m_Name);
				}
			}
		}

		// Token: 0x06001323 RID: 4899
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InformThreadNameChangeEx(Thread t, string name);

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x00034E68 File Offset: 0x00033E68
		// (set) Token: 0x06001325 RID: 4901 RVA: 0x00034EA4 File Offset: 0x00033EA4
		internal object AbortReason
		{
			get
			{
				object obj = null;
				try
				{
					obj = this.GetAbortReason();
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ExceptionStateCrossAppDomain"), ex);
				}
				return obj;
			}
			set
			{
				this.SetAbortReason(value);
			}
		}

		// Token: 0x06001326 RID: 4902
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginCriticalRegion();

		// Token: 0x06001327 RID: 4903
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndCriticalRegion();

		// Token: 0x06001328 RID: 4904
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginThreadAffinity();

		// Token: 0x06001329 RID: 4905
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, ControlThread = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndThreadAffinity();

		// Token: 0x0600132A RID: 4906 RVA: 0x00034EB0 File Offset: 0x00033EB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static byte VolatileRead(ref byte address)
		{
			byte b = address;
			Thread.MemoryBarrier();
			return b;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00034EC8 File Offset: 0x00033EC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static short VolatileRead(ref short address)
		{
			short num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00034EE0 File Offset: 0x00033EE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int VolatileRead(ref int address)
		{
			int num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00034EF8 File Offset: 0x00033EF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static long VolatileRead(ref long address)
		{
			long num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00034F10 File Offset: 0x00033F10
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static sbyte VolatileRead(ref sbyte address)
		{
			sbyte b = address;
			Thread.MemoryBarrier();
			return b;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00034F28 File Offset: 0x00033F28
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ushort VolatileRead(ref ushort address)
		{
			ushort num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00034F40 File Offset: 0x00033F40
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint VolatileRead(ref uint address)
		{
			uint num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00034F58 File Offset: 0x00033F58
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IntPtr VolatileRead(ref IntPtr address)
		{
			IntPtr intPtr = address;
			Thread.MemoryBarrier();
			return intPtr;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00034F74 File Offset: 0x00033F74
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr VolatileRead(ref UIntPtr address)
		{
			UIntPtr uintPtr = address;
			Thread.MemoryBarrier();
			return uintPtr;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00034F90 File Offset: 0x00033F90
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong VolatileRead(ref ulong address)
		{
			ulong num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00034FA8 File Offset: 0x00033FA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static float VolatileRead(ref float address)
		{
			float num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00034FC0 File Offset: 0x00033FC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static double VolatileRead(ref double address)
		{
			double num = address;
			Thread.MemoryBarrier();
			return num;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00034FD8 File Offset: 0x00033FD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static object VolatileRead(ref object address)
		{
			object obj = address;
			Thread.MemoryBarrier();
			return obj;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00034FEE File Offset: 0x00033FEE
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref byte address, byte value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00034FF8 File Offset: 0x00033FF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref short address, short value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00035002 File Offset: 0x00034002
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref int address, int value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0003500C File Offset: 0x0003400C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref long address, long value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00035016 File Offset: 0x00034016
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref sbyte address, sbyte value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00035020 File Offset: 0x00034020
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref ushort address, ushort value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0003502A File Offset: 0x0003402A
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref uint address, uint value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00035034 File Offset: 0x00034034
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref IntPtr address, IntPtr value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00035042 File Offset: 0x00034042
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref UIntPtr address, UIntPtr value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00035050 File Offset: 0x00034050
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref ulong address, ulong value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0003505A File Offset: 0x0003405A
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref float address, float value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00035064 File Offset: 0x00034064
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref double address, double value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0003506E File Offset: 0x0003406E
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void VolatileWrite(ref object address, object value)
		{
			Thread.MemoryBarrier();
			address = value;
		}

		// Token: 0x06001344 RID: 4932
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemoryBarrier();

		// Token: 0x06001345 RID: 4933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIsThreadStaticsArray(object o);

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x00035078 File Offset: 0x00034078
		private static LocalDataStoreMgr LocalDataStoreManager
		{
			get
			{
				if (Thread.s_LocalDataStoreMgr == null)
				{
					lock (Thread.s_SyncObject)
					{
						if (Thread.s_LocalDataStoreMgr == null)
						{
							Thread.s_LocalDataStoreMgr = new LocalDataStoreMgr();
						}
					}
				}
				return Thread.s_LocalDataStoreMgr;
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000350C8 File Offset: 0x000340C8
		void _Thread.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000350CF File Offset: 0x000340CF
		void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x000350D6 File Offset: 0x000340D6
		void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000350DD File Offset: 0x000340DD
		void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600134B RID: 4939
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetAbortReason(object o);

		// Token: 0x0600134C RID: 4940
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetAbortReason();

		// Token: 0x0600134D RID: 4941
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ClearAbortReason();

		// Token: 0x0400064A RID: 1610
		private const int STATICS_BUCKET_SIZE = 32;

		// Token: 0x0400064B RID: 1611
		private Context m_Context;

		// Token: 0x0400064C RID: 1612
		private ExecutionContext m_ExecutionContext;

		// Token: 0x0400064D RID: 1613
		private string m_Name;

		// Token: 0x0400064E RID: 1614
		private Delegate m_Delegate;

		// Token: 0x0400064F RID: 1615
		private object[][] m_ThreadStaticsBuckets;

		// Token: 0x04000650 RID: 1616
		private int[] m_ThreadStaticsBits;

		// Token: 0x04000651 RID: 1617
		private CultureInfo m_CurrentCulture;

		// Token: 0x04000652 RID: 1618
		private CultureInfo m_CurrentUICulture;

		// Token: 0x04000653 RID: 1619
		private object m_ThreadStartArg;

		// Token: 0x04000654 RID: 1620
		private IntPtr DONT_USE_InternalThread;

		// Token: 0x04000655 RID: 1621
		private int m_Priority;

		// Token: 0x04000656 RID: 1622
		private int m_ManagedThreadId;

		// Token: 0x04000657 RID: 1623
		private static LocalDataStoreMgr s_LocalDataStoreMgr = null;

		// Token: 0x04000658 RID: 1624
		private static object s_SyncObject = new object();
	}
}
