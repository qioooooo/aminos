using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x0200013D RID: 317
	[Serializable]
	public sealed class ExecutionContext : ISerializable
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x0003289F File Offset: 0x0003189F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext()
		{
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x000328A7 File Offset: 0x000318A7
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x000328C2 File Offset: 0x000318C2
		internal LogicalCallContext LogicalCallContext
		{
			get
			{
				if (this._logicalCallContext == null)
				{
					this._logicalCallContext = new LogicalCallContext();
				}
				return this._logicalCallContext;
			}
			set
			{
				this._logicalCallContext = value;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x000328CB File Offset: 0x000318CB
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x000328E6 File Offset: 0x000318E6
		internal IllogicalCallContext IllogicalCallContext
		{
			get
			{
				if (this._illogicalCallContext == null)
				{
					this._illogicalCallContext = new IllogicalCallContext();
				}
				return this._illogicalCallContext;
			}
			set
			{
				this._illogicalCallContext = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x000328EF File Offset: 0x000318EF
		internal Thread Thread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._thread = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x000328F8 File Offset: 0x000318F8
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x00032900 File Offset: 0x00031900
		internal SynchronizationContext SynchronizationContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._syncContext;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._syncContext = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00032909 File Offset: 0x00031909
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00032911 File Offset: 0x00031911
		internal HostExecutionContext HostExecutionContext
		{
			get
			{
				return this._hostExecutionContext;
			}
			set
			{
				this._hostExecutionContext = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x0003291A File Offset: 0x0003191A
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00032922 File Offset: 0x00031922
		internal SecurityContext SecurityContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._securityContext;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._securityContext = value;
				if (value != null)
				{
					this._securityContext.ExecutionContext = this;
				}
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003293C File Offset: 0x0003193C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void Run(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (executionContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullContext"));
			}
			if (!executionContext.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotNewCaptureContext"));
			}
			executionContext.isNewCapture = false;
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if ((executionContextNoCreate == null || executionContextNoCreate.IsDefaultFTContext()) && SecurityContext.CurrentlyInDefaultFTSecurityContext(executionContextNoCreate) && executionContext.IsDefaultFTContext())
			{
				callback(state);
				return;
			}
			ExecutionContext.RunInternal(executionContext, callback, state);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000329B4 File Offset: 0x000319B4
		internal static void RunInternal(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (ExecutionContext.cleanupCode == null)
			{
				ExecutionContext.tryCode = new RuntimeHelpers.TryCode(ExecutionContext.runTryCode);
				ExecutionContext.cleanupCode = new RuntimeHelpers.CleanupCode(ExecutionContext.runFinallyCode);
			}
			ExecutionContext.ExecutionContextRunData executionContextRunData = new ExecutionContext.ExecutionContextRunData(executionContext, callback, state);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(ExecutionContext.tryCode, ExecutionContext.cleanupCode, executionContextRunData);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00032A04 File Offset: 0x00031A04
		internal static void runTryCode(object userData)
		{
			ExecutionContext.ExecutionContextRunData executionContextRunData = (ExecutionContext.ExecutionContextRunData)userData;
			executionContextRunData.ecsw = ExecutionContext.SetExecutionContext(executionContextRunData.ec);
			executionContextRunData.callBack(executionContextRunData.state);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00032A3C File Offset: 0x00031A3C
		[PrePrepareMethod]
		internal static void runFinallyCode(object userData, bool exceptionThrown)
		{
			ExecutionContext.ExecutionContextRunData executionContextRunData = (ExecutionContext.ExecutionContextRunData)userData;
			executionContextRunData.ecsw.Undo();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00032A5C File Offset: 0x00031A5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static ExecutionContextSwitcher SetExecutionContext(ExecutionContext executionContext)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
			executionContextSwitcher.thread = Thread.CurrentThread;
			executionContextSwitcher.prevEC = Thread.CurrentThread.GetExecutionContextNoCreate();
			executionContextSwitcher.currEC = executionContext;
			Thread.CurrentThread.SetExecutionContext(executionContext);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (executionContext != null)
				{
					SecurityContext securityContext = executionContext.SecurityContext;
					if (securityContext != null)
					{
						SecurityContext securityContext2 = ((executionContextSwitcher.prevEC != null) ? executionContextSwitcher.prevEC.SecurityContext : null);
						executionContextSwitcher.scsw = SecurityContext.SetSecurityContext(securityContext, securityContext2, ref stackCrawlMark);
					}
					else if (!SecurityContext.CurrentlyInDefaultFTSecurityContext(executionContextSwitcher.prevEC))
					{
						SecurityContext securityContext3 = ((executionContextSwitcher.prevEC != null) ? executionContextSwitcher.prevEC.SecurityContext : null);
						executionContextSwitcher.scsw = SecurityContext.SetSecurityContext(SecurityContext.FullTrustSecurityContext, securityContext3, ref stackCrawlMark);
					}
					SynchronizationContext synchronizationContext = executionContext.SynchronizationContext;
					if (synchronizationContext != null)
					{
						SynchronizationContext synchronizationContext2 = ((executionContextSwitcher.prevEC != null) ? executionContextSwitcher.prevEC.SynchronizationContext : null);
						executionContextSwitcher.sysw = SynchronizationContext.SetSynchronizationContext(synchronizationContext, synchronizationContext2);
					}
					HostExecutionContext hostExecutionContext = executionContext.HostExecutionContext;
					if (hostExecutionContext != null)
					{
						executionContextSwitcher.hecsw = HostExecutionContextManager.SetHostExecutionContextInternal(hostExecutionContext);
					}
				}
			}
			catch
			{
				executionContextSwitcher.UndoNoThrow();
				throw;
			}
			return executionContextSwitcher;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00032B90 File Offset: 0x00031B90
		public ExecutionContext CreateCopy()
		{
			if (!this.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotCopyUsedContext"));
			}
			ExecutionContext executionContext = new ExecutionContext();
			executionContext.isNewCapture = true;
			executionContext._syncContext = ((this._syncContext == null) ? null : this._syncContext.CreateCopy());
			executionContext._hostExecutionContext = ((this._hostExecutionContext == null) ? null : this._hostExecutionContext.CreateCopy());
			if (this._securityContext != null)
			{
				executionContext._securityContext = this._securityContext.CreateCopy();
				executionContext._securityContext.ExecutionContext = executionContext;
			}
			if (this._logicalCallContext != null)
			{
				LogicalCallContext logicalCallContext = this.LogicalCallContext;
				executionContext.LogicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
			}
			if (this._illogicalCallContext != null)
			{
				IllogicalCallContext illogicalCallContext = this.IllogicalCallContext;
				executionContext.IllogicalCallContext = (IllogicalCallContext)illogicalCallContext.Clone();
			}
			return executionContext;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00032C60 File Offset: 0x00031C60
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static AsyncFlowControl SuppressFlow()
		{
			if (ExecutionContext.IsFlowSuppressed())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotSupressFlowMultipleTimes"));
			}
			AsyncFlowControl asyncFlowControl = default(AsyncFlowControl);
			asyncFlowControl.Setup();
			return asyncFlowControl;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00032C94 File Offset: 0x00031C94
		public static void RestoreFlow()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (executionContextNoCreate == null || !executionContextNoCreate.isFlowSuppressed)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotRestoreUnsupressedFlow"));
			}
			executionContextNoCreate.isFlowSuppressed = false;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00032CD0 File Offset: 0x00031CD0
		public static bool IsFlowSuppressed()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			return executionContextNoCreate != null && executionContextNoCreate.isFlowSuppressed;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00032CF4 File Offset: 0x00031CF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ExecutionContext Capture()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ExecutionContext.Capture(ref stackCrawlMark);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00032D0C File Offset: 0x00031D0C
		internal static ExecutionContext Capture(ref StackCrawlMark stackMark)
		{
			if (ExecutionContext.IsFlowSuppressed())
			{
				return null;
			}
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			ExecutionContext executionContext = new ExecutionContext();
			executionContext.isNewCapture = true;
			executionContext.SecurityContext = SecurityContext.Capture(executionContextNoCreate, ref stackMark);
			if (executionContext.SecurityContext != null)
			{
				executionContext.SecurityContext.ExecutionContext = executionContext;
			}
			executionContext._hostExecutionContext = HostExecutionContextManager.CaptureHostExecutionContext();
			if (executionContextNoCreate != null)
			{
				executionContext._syncContext = ((executionContextNoCreate._syncContext == null) ? null : executionContextNoCreate._syncContext.CreateCopy());
				if (executionContextNoCreate._logicalCallContext != null)
				{
					LogicalCallContext logicalCallContext = executionContextNoCreate.LogicalCallContext;
					executionContext.LogicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
				}
			}
			return executionContext;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00032DA6 File Offset: 0x00031DA6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this._logicalCallContext != null)
			{
				info.AddValue("LogicalCallContext", this._logicalCallContext, typeof(LogicalCallContext));
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00032DDC File Offset: 0x00031DDC
		private ExecutionContext(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("LogicalCallContext"))
				{
					this._logicalCallContext = (LogicalCallContext)enumerator.Value;
				}
			}
			this.Thread = Thread.CurrentThread;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00032E2E File Offset: 0x00031E2E
		internal static void ClearSyncContext(ExecutionContext ec)
		{
			if (ec != null)
			{
				ec.SynchronizationContext = null;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00032E3C File Offset: 0x00031E3C
		internal bool IsDefaultFTContext()
		{
			return this._hostExecutionContext == null && this._syncContext == null && (this._securityContext == null || this._securityContext.IsDefaultFTSecurityContext()) && (this._logicalCallContext == null || !this._logicalCallContext.HasInfo) && (this._illogicalCallContext == null || !this._illogicalCallContext.HasUserData);
		}

		// Token: 0x040005F8 RID: 1528
		private HostExecutionContext _hostExecutionContext;

		// Token: 0x040005F9 RID: 1529
		private SynchronizationContext _syncContext;

		// Token: 0x040005FA RID: 1530
		private SecurityContext _securityContext;

		// Token: 0x040005FB RID: 1531
		private LogicalCallContext _logicalCallContext;

		// Token: 0x040005FC RID: 1532
		private IllogicalCallContext _illogicalCallContext;

		// Token: 0x040005FD RID: 1533
		private Thread _thread;

		// Token: 0x040005FE RID: 1534
		internal bool isNewCapture;

		// Token: 0x040005FF RID: 1535
		internal bool isFlowSuppressed;

		// Token: 0x04000600 RID: 1536
		internal static RuntimeHelpers.TryCode tryCode;

		// Token: 0x04000601 RID: 1537
		internal static RuntimeHelpers.CleanupCode cleanupCode;

		// Token: 0x0200013E RID: 318
		internal class ExecutionContextRunData
		{
			// Token: 0x06001206 RID: 4614 RVA: 0x00032EA3 File Offset: 0x00031EA3
			internal ExecutionContextRunData(ExecutionContext executionContext, ContextCallback cb, object state)
			{
				this.ec = executionContext;
				this.callBack = cb;
				this.state = state;
				this.ecsw = default(ExecutionContextSwitcher);
			}

			// Token: 0x04000602 RID: 1538
			internal ExecutionContext ec;

			// Token: 0x04000603 RID: 1539
			internal ContextCallback callBack;

			// Token: 0x04000604 RID: 1540
			internal object state;

			// Token: 0x04000605 RID: 1541
			internal ExecutionContextSwitcher ecsw;
		}
	}
}
