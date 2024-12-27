using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000144 RID: 324
	public class HostExecutionContextManager
	{
		// Token: 0x0600122C RID: 4652
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HostSecurityManagerPresent();

		// Token: 0x0600122D RID: 4653
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ReleaseHostSecurityContext(IntPtr context);

		// Token: 0x0600122E RID: 4654
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int CloneHostSecurityContext(SafeHandle context, SafeHandle clonedContext);

		// Token: 0x0600122F RID: 4655
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int CaptureHostSecurityContext(SafeHandle capturedContext);

		// Token: 0x06001230 RID: 4656
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SetHostSecurityContext(SafeHandle context, bool fReturnPrevious, SafeHandle prevContext);

		// Token: 0x06001231 RID: 4657 RVA: 0x00032FF0 File Offset: 0x00031FF0
		internal static bool CheckIfHosted()
		{
			if (!HostExecutionContextManager._fIsHostedChecked)
			{
				HostExecutionContextManager._fIsHosted = HostExecutionContextManager.HostSecurityManagerPresent();
				HostExecutionContextManager._fIsHostedChecked = true;
			}
			return HostExecutionContextManager._fIsHosted;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00033010 File Offset: 0x00032010
		public virtual HostExecutionContext Capture()
		{
			HostExecutionContext hostExecutionContext = null;
			if (HostExecutionContextManager.CheckIfHosted())
			{
				IUnknownSafeHandle unknownSafeHandle = new IUnknownSafeHandle();
				hostExecutionContext = new HostExecutionContext(unknownSafeHandle);
				HostExecutionContextManager.CaptureHostSecurityContext(unknownSafeHandle);
			}
			return hostExecutionContext;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003303C File Offset: 0x0003203C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual object SetHostExecutionContext(HostExecutionContext hostExecutionContext)
		{
			if (hostExecutionContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			HostExecutionContextSwitcher hostExecutionContextSwitcher = new HostExecutionContextSwitcher();
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			hostExecutionContextSwitcher.executionContext = executionContext;
			hostExecutionContextSwitcher.currentHostContext = hostExecutionContext;
			hostExecutionContextSwitcher.previousHostContext = null;
			if (HostExecutionContextManager.CheckIfHosted() && hostExecutionContext.State is IUnknownSafeHandle)
			{
				IUnknownSafeHandle unknownSafeHandle = new IUnknownSafeHandle();
				hostExecutionContextSwitcher.previousHostContext = new HostExecutionContext(unknownSafeHandle);
				IUnknownSafeHandle unknownSafeHandle2 = (IUnknownSafeHandle)hostExecutionContext.State;
				HostExecutionContextManager.SetHostSecurityContext(unknownSafeHandle2, true, unknownSafeHandle);
			}
			executionContext.HostExecutionContext = hostExecutionContext;
			return hostExecutionContextSwitcher;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000330C8 File Offset: 0x000320C8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual void Revert(object previousState)
		{
			HostExecutionContextSwitcher hostExecutionContextSwitcher = previousState as HostExecutionContextSwitcher;
			if (hostExecutionContextSwitcher == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotOverrideSetWithoutRevert"));
			}
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			if (executionContext != hostExecutionContextSwitcher.executionContext)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseSwitcherOtherThread"));
			}
			hostExecutionContextSwitcher.executionContext = null;
			HostExecutionContext hostExecutionContext = executionContext.HostExecutionContext;
			if (hostExecutionContext != hostExecutionContextSwitcher.currentHostContext)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseSwitcherOtherThread"));
			}
			HostExecutionContext previousHostContext = hostExecutionContextSwitcher.previousHostContext;
			if (HostExecutionContextManager.CheckIfHosted() && previousHostContext != null && previousHostContext.State is IUnknownSafeHandle)
			{
				IUnknownSafeHandle unknownSafeHandle = (IUnknownSafeHandle)previousHostContext.State;
				HostExecutionContextManager.SetHostSecurityContext(unknownSafeHandle, false, null);
			}
			executionContext.HostExecutionContext = previousHostContext;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00033178 File Offset: 0x00032178
		internal static HostExecutionContext CaptureHostExecutionContext()
		{
			HostExecutionContext hostExecutionContext = null;
			HostExecutionContextManager currentHostExecutionContextManager = HostExecutionContextManager.GetCurrentHostExecutionContextManager();
			if (currentHostExecutionContextManager != null)
			{
				hostExecutionContext = currentHostExecutionContextManager.Capture();
			}
			return hostExecutionContext;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00033198 File Offset: 0x00032198
		internal static object SetHostExecutionContextInternal(HostExecutionContext hostContext)
		{
			HostExecutionContextManager currentHostExecutionContextManager = HostExecutionContextManager.GetCurrentHostExecutionContextManager();
			object obj = null;
			if (currentHostExecutionContextManager != null)
			{
				obj = currentHostExecutionContextManager.SetHostExecutionContext(hostContext);
			}
			return obj;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000331B9 File Offset: 0x000321B9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static HostExecutionContextManager GetCurrentHostExecutionContextManager()
		{
			if (AppDomainManager.CurrentAppDomainManager != null)
			{
				return AppDomainManager.CurrentAppDomainManager.HostExecutionContextManager;
			}
			return null;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000331CE File Offset: 0x000321CE
		internal static HostExecutionContextManager GetInternalHostExecutionContextManager()
		{
			if (HostExecutionContextManager._hostExecutionContextManager == null)
			{
				HostExecutionContextManager._hostExecutionContextManager = new HostExecutionContextManager();
			}
			return HostExecutionContextManager._hostExecutionContextManager;
		}

		// Token: 0x0400060A RID: 1546
		private static bool _fIsHostedChecked;

		// Token: 0x0400060B RID: 1547
		private static bool _fIsHosted;

		// Token: 0x0400060C RID: 1548
		private static HostExecutionContextManager _hostExecutionContextManager;
	}
}
