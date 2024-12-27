using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005C5 RID: 1477
	public static class RuntimeHelpers
	{
		// Token: 0x06003768 RID: 14184
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

		// Token: 0x06003769 RID: 14185
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectValue(object obj);

		// Token: 0x0600376A RID: 14186
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _RunClassConstructor(IntPtr type);

		// Token: 0x0600376B RID: 14187 RVA: 0x000BB68A File Offset: 0x000BA68A
		public static void RunClassConstructor(RuntimeTypeHandle type)
		{
			RuntimeHelpers._RunClassConstructor(type.Value);
		}

		// Token: 0x0600376C RID: 14188
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _RunModuleConstructor(IntPtr module);

		// Token: 0x0600376D RID: 14189 RVA: 0x000BB698 File Offset: 0x000BA698
		public static void RunModuleConstructor(ModuleHandle module)
		{
			RuntimeHelpers._RunModuleConstructor(new IntPtr(module.Value));
		}

		// Token: 0x0600376E RID: 14190
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _PrepareMethod(IntPtr method, RuntimeTypeHandle[] instantiation);

		// Token: 0x0600376F RID: 14191
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _CompileMethod(IntPtr method);

		// Token: 0x06003770 RID: 14192 RVA: 0x000BB6AB File Offset: 0x000BA6AB
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareMethod(RuntimeMethodHandle method)
		{
			RuntimeHelpers._PrepareMethod(method.Value, null);
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000BB6BA File Offset: 0x000BA6BA
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers._PrepareMethod(method.Value, instantiation);
		}

		// Token: 0x06003772 RID: 14194
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PrepareDelegate(Delegate d);

		// Token: 0x06003773 RID: 14195 RVA: 0x000BB6C9 File Offset: 0x000BA6C9
		public static int GetHashCode(object o)
		{
			return object.InternalGetHashCode(o);
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000BB6D1 File Offset: 0x000BA6D1
		public new static bool Equals(object o1, object o2)
		{
			return object.InternalEquals(o1, o2);
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003775 RID: 14197 RVA: 0x000BB6DA File Offset: 0x000BA6DA
		public static int OffsetToStringData
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06003776 RID: 14198
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ProbeForSufficientStack();

		// Token: 0x06003777 RID: 14199 RVA: 0x000BB6DE File Offset: 0x000BA6DE
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareConstrainedRegions()
		{
			RuntimeHelpers.ProbeForSufficientStack();
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x000BB6E5 File Offset: 0x000BA6E5
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void PrepareConstrainedRegionsNoOP()
		{
		}

		// Token: 0x06003779 RID: 14201
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCodeWithGuaranteedCleanup(RuntimeHelpers.TryCode code, RuntimeHelpers.CleanupCode backoutCode, object userData);

		// Token: 0x0600377A RID: 14202 RVA: 0x000BB6E7 File Offset: 0x000BA6E7
		[PrePrepareMethod]
		internal static void ExecuteBackoutCodeHelper(object backoutCode, object userData, bool exceptionThrown)
		{
			((RuntimeHelpers.CleanupCode)backoutCode)(userData, exceptionThrown);
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x000BB6F8 File Offset: 0x000BA6F8
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void ExecuteCodeWithLock(object lockObject, RuntimeHelpers.TryCode code, object userState)
		{
			RuntimeHelpers.ExecuteWithLockHelper executeWithLockHelper = new RuntimeHelpers.ExecuteWithLockHelper(lockObject, code, userState);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(RuntimeHelpers.s_EnterMonitor, RuntimeHelpers.s_ExitMonitor, executeWithLockHelper);
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x000BB720 File Offset: 0x000BA720
		private static void EnterMonitorAndTryCode(object helper)
		{
			RuntimeHelpers.ExecuteWithLockHelper executeWithLockHelper = (RuntimeHelpers.ExecuteWithLockHelper)helper;
			Monitor.ReliableEnter(executeWithLockHelper.m_lockObject, ref executeWithLockHelper.m_tookLock);
			executeWithLockHelper.m_userCode(executeWithLockHelper.m_userState);
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000BB758 File Offset: 0x000BA758
		[PrePrepareMethod]
		private static void ExitMonitorOnBackout(object helper, bool exceptionThrown)
		{
			RuntimeHelpers.ExecuteWithLockHelper executeWithLockHelper = (RuntimeHelpers.ExecuteWithLockHelper)helper;
			if (executeWithLockHelper.m_tookLock)
			{
				Monitor.Exit(executeWithLockHelper.m_lockObject);
			}
		}

		// Token: 0x04001C90 RID: 7312
		private static RuntimeHelpers.TryCode s_EnterMonitor = new RuntimeHelpers.TryCode(RuntimeHelpers.EnterMonitorAndTryCode);

		// Token: 0x04001C91 RID: 7313
		private static RuntimeHelpers.CleanupCode s_ExitMonitor = new RuntimeHelpers.CleanupCode(RuntimeHelpers.ExitMonitorOnBackout);

		// Token: 0x020005C6 RID: 1478
		// (Invoke) Token: 0x06003780 RID: 14208
		public delegate void TryCode(object userData);

		// Token: 0x020005C7 RID: 1479
		// (Invoke) Token: 0x06003784 RID: 14212
		public delegate void CleanupCode(object userData, bool exceptionThrown);

		// Token: 0x020005C8 RID: 1480
		private class ExecuteWithLockHelper
		{
			// Token: 0x06003787 RID: 14215 RVA: 0x000BB7A3 File Offset: 0x000BA7A3
			internal ExecuteWithLockHelper(object lockObject, RuntimeHelpers.TryCode userCode, object userState)
			{
				this.m_lockObject = lockObject;
				this.m_userCode = userCode;
				this.m_userState = userState;
			}

			// Token: 0x04001C92 RID: 7314
			internal object m_lockObject;

			// Token: 0x04001C93 RID: 7315
			internal bool m_tookLock;

			// Token: 0x04001C94 RID: 7316
			internal RuntimeHelpers.TryCode m_userCode;

			// Token: 0x04001C95 RID: 7317
			internal object m_userState;
		}
	}
}
