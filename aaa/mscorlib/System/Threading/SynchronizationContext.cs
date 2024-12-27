using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000130 RID: 304
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
	public class SynchronizationContext
	{
		// Token: 0x06001181 RID: 4481 RVA: 0x00031D4C File Offset: 0x00030D4C
		protected void SetWaitNotificationRequired()
		{
			RuntimeHelpers.PrepareDelegate(new WaitDelegate(this.Wait));
			this._props |= SynchronizationContextProperties.RequireWaitNotification;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00031D6E File Offset: 0x00030D6E
		public bool IsWaitNotificationRequired()
		{
			return (this._props & SynchronizationContextProperties.RequireWaitNotification) != SynchronizationContextProperties.None;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00031D7E File Offset: 0x00030D7E
		public virtual void Send(SendOrPostCallback d, object state)
		{
			d(state);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00031D87 File Offset: 0x00030D87
		public virtual void Post(SendOrPostCallback d, object state)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(d.Invoke), state);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00031D9C File Offset: 0x00030D9C
		public virtual void OperationStarted()
		{
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00031D9E File Offset: 0x00030D9E
		public virtual void OperationCompleted()
		{
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00031DA0 File Offset: 0x00030DA0
		[CLSCompliant(false)]
		[PrePrepareMethod]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
		public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			if (waitHandles == null)
			{
				throw new ArgumentNullException("waitHandles");
			}
			return SynchronizationContext.WaitHelper(waitHandles, waitAll, millisecondsTimeout);
		}

		// Token: 0x06001188 RID: 4488
		[CLSCompliant(false)]
		[PrePrepareMethod]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout);

		// Token: 0x06001189 RID: 4489 RVA: 0x00031DB8 File Offset: 0x00030DB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
		public static void SetSynchronizationContext(SynchronizationContext syncContext)
		{
			SynchronizationContext.SetSynchronizationContext(syncContext, Thread.CurrentThread.ExecutionContext.SynchronizationContext);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00031DD0 File Offset: 0x00030DD0
		internal static SynchronizationContextSwitcher SetSynchronizationContext(SynchronizationContext syncContext, SynchronizationContext prevSyncContext)
		{
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			SynchronizationContextSwitcher synchronizationContextSwitcher = default(SynchronizationContextSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				synchronizationContextSwitcher._ec = executionContext;
				synchronizationContextSwitcher.savedSC = prevSyncContext;
				synchronizationContextSwitcher.currSC = syncContext;
				executionContext.SynchronizationContext = syncContext;
			}
			catch
			{
				synchronizationContextSwitcher.UndoNoThrow();
				throw;
			}
			return synchronizationContextSwitcher;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00031E34 File Offset: 0x00030E34
		public static SynchronizationContext Current
		{
			get
			{
				ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
				if (executionContextNoCreate != null)
				{
					return executionContextNoCreate.SynchronizationContext;
				}
				return null;
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00031E57 File Offset: 0x00030E57
		public virtual SynchronizationContext CreateCopy()
		{
			return new SynchronizationContext();
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00031E5E File Offset: 0x00030E5E
		private static int InvokeWaitMethodHelper(SynchronizationContext syncContext, IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			return syncContext.Wait(waitHandles, waitAll, millisecondsTimeout);
		}

		// Token: 0x040005D3 RID: 1491
		private SynchronizationContextProperties _props;
	}
}
