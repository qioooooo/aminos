using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security
{
	// Token: 0x02000678 RID: 1656
	public sealed class SecurityContext
	{
		// Token: 0x06003C3A RID: 15418 RVA: 0x000CEE18 File Offset: 0x000CDE18
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal SecurityContext()
		{
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x000CEE20 File Offset: 0x000CDE20
		internal static SecurityContext FullTrustSecurityContext
		{
			get
			{
				if (SecurityContext._fullTrustSC == null)
				{
					SecurityContext._fullTrustSC = SecurityContext.CreateFullTrustSecurityContext();
				}
				return SecurityContext._fullTrustSC;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (set) Token: 0x06003C3C RID: 15420 RVA: 0x000CEE38 File Offset: 0x000CDE38
		internal ExecutionContext ExecutionContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._executionContext = value;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000CEE41 File Offset: 0x000CDE41
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x000CEE49 File Offset: 0x000CDE49
		internal WindowsIdentity WindowsIdentity
		{
			get
			{
				return this._windowsIdentity;
			}
			set
			{
				this._windowsIdentity = value;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000CEE52 File Offset: 0x000CDE52
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x000CEE5A File Offset: 0x000CDE5A
		internal CompressedStack CompressedStack
		{
			get
			{
				return this._compressedStack;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._compressedStack = value;
			}
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x000CEE63 File Offset: 0x000CDE63
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlow()
		{
			return SecurityContext.SuppressFlow(SecurityContextDisableFlow.All);
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x000CEE6F File Offset: 0x000CDE6F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlowWindowsIdentity()
		{
			return SecurityContext.SuppressFlow(SecurityContextDisableFlow.WI);
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x000CEE78 File Offset: 0x000CDE78
		internal static AsyncFlowControl SuppressFlow(SecurityContextDisableFlow flags)
		{
			if (SecurityContext.IsFlowSuppressed(flags))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotSupressFlowMultipleTimes"));
			}
			if (Thread.CurrentThread.ExecutionContext.SecurityContext == null)
			{
				Thread.CurrentThread.ExecutionContext.SecurityContext = new SecurityContext();
			}
			AsyncFlowControl asyncFlowControl = default(AsyncFlowControl);
			asyncFlowControl.Setup(flags);
			return asyncFlowControl;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x000CEED4 File Offset: 0x000CDED4
		public static void RestoreFlow()
		{
			SecurityContext currentSecurityContextNoCreate = SecurityContext.GetCurrentSecurityContextNoCreate();
			if (currentSecurityContextNoCreate == null || currentSecurityContextNoCreate._disableFlow == SecurityContextDisableFlow.Nothing)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotRestoreUnsupressedFlow"));
			}
			currentSecurityContextNoCreate._disableFlow = SecurityContextDisableFlow.Nothing;
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x000CEF09 File Offset: 0x000CDF09
		public static bool IsFlowSuppressed()
		{
			return SecurityContext.IsFlowSuppressed(SecurityContextDisableFlow.All);
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000CEF15 File Offset: 0x000CDF15
		public static bool IsWindowsIdentityFlowSuppressed()
		{
			return SecurityContext._LegacyImpersonationPolicy || SecurityContext.IsFlowSuppressed(SecurityContextDisableFlow.WI);
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x000CEF28 File Offset: 0x000CDF28
		internal static bool IsFlowSuppressed(SecurityContextDisableFlow flags)
		{
			SecurityContext currentSecurityContextNoCreate = SecurityContext.GetCurrentSecurityContextNoCreate();
			return currentSecurityContextNoCreate != null && (currentSecurityContextNoCreate._disableFlow & flags) == flags;
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000CEF4C File Offset: 0x000CDF4C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
		{
			if (securityContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));
			}
			if (!securityContext.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			securityContext.isNewCapture = false;
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (SecurityContext.CurrentlyInDefaultFTSecurityContext(executionContextNoCreate) && securityContext.IsDefaultFTSecurityContext())
			{
				callback(state);
				return;
			}
			SecurityContext.RunInternal(securityContext, callback, state);
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x000CEFB8 File Offset: 0x000CDFB8
		internal static void RunInternal(SecurityContext securityContext, ContextCallback callBack, object state)
		{
			if (SecurityContext.cleanupCode == null)
			{
				SecurityContext.tryCode = new RuntimeHelpers.TryCode(SecurityContext.runTryCode);
				SecurityContext.cleanupCode = new RuntimeHelpers.CleanupCode(SecurityContext.runFinallyCode);
			}
			SecurityContext.SecurityContextRunData securityContextRunData = new SecurityContext.SecurityContextRunData(securityContext, callBack, state);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(SecurityContext.tryCode, SecurityContext.cleanupCode, securityContextRunData);
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000CF008 File Offset: 0x000CE008
		internal static void runTryCode(object userData)
		{
			SecurityContext.SecurityContextRunData securityContextRunData = (SecurityContext.SecurityContextRunData)userData;
			securityContextRunData.scsw = SecurityContext.SetSecurityContext(securityContextRunData.sc, Thread.CurrentThread.ExecutionContext.SecurityContext);
			securityContextRunData.callBack(securityContextRunData.state);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x000CF050 File Offset: 0x000CE050
		[PrePrepareMethod]
		internal static void runFinallyCode(object userData, bool exceptionThrown)
		{
			SecurityContext.SecurityContextRunData securityContextRunData = (SecurityContext.SecurityContextRunData)userData;
			securityContextRunData.scsw.Undo();
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000CF070 File Offset: 0x000CE070
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static SecurityContextSwitcher SetSecurityContext(SecurityContext sc, SecurityContext prevSecurityContext)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return SecurityContext.SetSecurityContext(sc, prevSecurityContext, ref stackCrawlMark);
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x000CF088 File Offset: 0x000CE088
		internal static SecurityContextSwitcher SetSecurityContext(SecurityContext sc, SecurityContext prevSecurityContext, ref StackCrawlMark stackMark)
		{
			SecurityContextDisableFlow disableFlow = sc._disableFlow;
			sc._disableFlow = SecurityContextDisableFlow.Nothing;
			SecurityContextSwitcher securityContextSwitcher = default(SecurityContextSwitcher);
			securityContextSwitcher.currSC = sc;
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			securityContextSwitcher.currEC = executionContext;
			securityContextSwitcher.prevSC = prevSecurityContext;
			executionContext.SecurityContext = sc;
			if (sc != null)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					securityContextSwitcher.wic = null;
					if (!SecurityContext._LegacyImpersonationPolicy)
					{
						if (sc.WindowsIdentity != null)
						{
							securityContextSwitcher.wic = sc.WindowsIdentity.Impersonate(ref stackMark);
						}
						else if ((disableFlow & SecurityContextDisableFlow.WI) == SecurityContextDisableFlow.Nothing && prevSecurityContext != null && prevSecurityContext.WindowsIdentity != null)
						{
							securityContextSwitcher.wic = WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackMark);
						}
					}
					securityContextSwitcher.cssw = CompressedStack.SetCompressedStack(sc.CompressedStack, (prevSecurityContext != null) ? prevSecurityContext.CompressedStack : null);
				}
				catch
				{
					securityContextSwitcher.UndoNoThrow();
					throw;
				}
			}
			return securityContextSwitcher;
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x000CF168 File Offset: 0x000CE168
		public SecurityContext CreateCopy()
		{
			if (!this.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			securityContext._disableFlow = this._disableFlow;
			securityContext._windowsIdentity = this.WindowsIdentity;
			if (this._compressedStack != null)
			{
				securityContext._compressedStack = this._compressedStack.CreateCopy();
			}
			return securityContext;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x000CF1CC File Offset: 0x000CE1CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static SecurityContext Capture()
		{
			if (SecurityContext.IsFlowSuppressed() || !SecurityManager._IsSecurityOn())
			{
				return null;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityContext securityContext = SecurityContext.Capture(Thread.CurrentThread.GetExecutionContextNoCreate(), ref stackCrawlMark);
			if (securityContext == null)
			{
				securityContext = SecurityContext.CreateFullTrustSecurityContext();
			}
			return securityContext;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x000CF208 File Offset: 0x000CE208
		internal static SecurityContext Capture(ExecutionContext currThreadEC, ref StackCrawlMark stackMark)
		{
			if (SecurityContext.IsFlowSuppressed() || !SecurityManager._IsSecurityOn())
			{
				return null;
			}
			if (SecurityContext.CurrentlyInDefaultFTSecurityContext(currThreadEC))
			{
				return null;
			}
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			if (!SecurityContext.IsWindowsIdentityFlowSuppressed())
			{
				securityContext._windowsIdentity = SecurityContext.GetCurrentWI(currThreadEC);
			}
			else
			{
				securityContext._disableFlow = SecurityContextDisableFlow.WI;
			}
			securityContext.CompressedStack = CompressedStack.GetCompressedStack(ref stackMark);
			return securityContext;
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x000CF268 File Offset: 0x000CE268
		internal static SecurityContext CreateFullTrustSecurityContext()
		{
			SecurityContext securityContext = new SecurityContext();
			securityContext.isNewCapture = true;
			if (SecurityContext.IsWindowsIdentityFlowSuppressed())
			{
				securityContext._disableFlow = SecurityContextDisableFlow.WI;
			}
			securityContext.CompressedStack = new CompressedStack(null);
			return securityContext;
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x000CF2A0 File Offset: 0x000CE2A0
		internal static SecurityContext GetCurrentSecurityContextNoCreate()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (executionContextNoCreate != null)
			{
				return executionContextNoCreate.SecurityContext;
			}
			return null;
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x000CF2C4 File Offset: 0x000CE2C4
		internal static WindowsIdentity GetCurrentWI(ExecutionContext threadEC)
		{
			if (SecurityContext._alwaysFlowImpersonationPolicy)
			{
				return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, true);
			}
			SecurityContext securityContext = ((threadEC == null) ? null : threadEC.SecurityContext);
			if (securityContext != null)
			{
				return securityContext.WindowsIdentity;
			}
			return null;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000CF2FC File Offset: 0x000CE2FC
		internal bool IsDefaultFTSecurityContext()
		{
			return this.WindowsIdentity == null && (this.CompressedStack == null || this.CompressedStack.CompressedStackHandle == null);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x000CF320 File Offset: 0x000CE320
		internal static bool CurrentlyInDefaultFTSecurityContext(ExecutionContext threadEC)
		{
			return SecurityContext.IsDefaultThreadSecurityInfo() && SecurityContext.GetCurrentWI(threadEC) == null;
		}

		// Token: 0x06003C56 RID: 15446
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern WindowsImpersonationFlowMode GetImpersonationFlowMode();

		// Token: 0x06003C57 RID: 15447
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDefaultThreadSecurityInfo();

		// Token: 0x04001ECB RID: 7883
		private static bool _LegacyImpersonationPolicy = SecurityContext.GetImpersonationFlowMode() == WindowsImpersonationFlowMode.IMP_NOFLOW;

		// Token: 0x04001ECC RID: 7884
		private static bool _alwaysFlowImpersonationPolicy = SecurityContext.GetImpersonationFlowMode() == WindowsImpersonationFlowMode.IMP_ALWAYSFLOW;

		// Token: 0x04001ECD RID: 7885
		private ExecutionContext _executionContext;

		// Token: 0x04001ECE RID: 7886
		private WindowsIdentity _windowsIdentity;

		// Token: 0x04001ECF RID: 7887
		private CompressedStack _compressedStack;

		// Token: 0x04001ED0 RID: 7888
		private static SecurityContext _fullTrustSC;

		// Token: 0x04001ED1 RID: 7889
		internal bool isNewCapture;

		// Token: 0x04001ED2 RID: 7890
		internal SecurityContextDisableFlow _disableFlow;

		// Token: 0x04001ED3 RID: 7891
		internal static RuntimeHelpers.TryCode tryCode;

		// Token: 0x04001ED4 RID: 7892
		internal static RuntimeHelpers.CleanupCode cleanupCode;

		// Token: 0x02000679 RID: 1657
		internal class SecurityContextRunData
		{
			// Token: 0x06003C59 RID: 15449 RVA: 0x000CF350 File Offset: 0x000CE350
			internal SecurityContextRunData(SecurityContext securityContext, ContextCallback cb, object state)
			{
				this.sc = securityContext;
				this.callBack = cb;
				this.state = state;
				this.scsw = default(SecurityContextSwitcher);
			}

			// Token: 0x04001ED5 RID: 7893
			internal SecurityContext sc;

			// Token: 0x04001ED6 RID: 7894
			internal ContextCallback callBack;

			// Token: 0x04001ED7 RID: 7895
			internal object state;

			// Token: 0x04001ED8 RID: 7896
			internal SecurityContextSwitcher scsw;
		}
	}
}
