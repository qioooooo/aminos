using System;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003A4 RID: 932
	internal class ContextAwareResult : LazyAsyncResult
	{
		// Token: 0x06001D31 RID: 7473 RVA: 0x0006FAB5 File Offset: 0x0006EAB5
		internal ContextAwareResult(object myObject, object myState, AsyncCallback myCallBack)
			: this(false, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0006FAC2 File Offset: 0x0006EAC2
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, object myObject, object myState, AsyncCallback myCallBack)
			: this(captureIdentity, forceCaptureContext, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0006FAD2 File Offset: 0x0006EAD2
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, bool threadSafeContextCopy, object myObject, object myState, AsyncCallback myCallBack)
			: base(myObject, myState, myCallBack)
		{
			if (forceCaptureContext)
			{
				this._Flags = ContextAwareResult.StateFlags.CaptureContext;
			}
			if (captureIdentity)
			{
				this._Flags |= ContextAwareResult.StateFlags.CaptureIdentity;
			}
			if (threadSafeContextCopy)
			{
				this._Flags |= ContextAwareResult.StateFlags.ThreadSafeContextCopy;
			}
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0006FB0C File Offset: 0x0006EB0C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void SafeCaptureIdenity()
		{
			this._Wi = WindowsIdentity.GetCurrent();
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x0006FB1C File Offset: 0x0006EB1C
		internal ExecutionContext ContextCopy
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				ExecutionContext executionContext = this._Context;
				if (executionContext != null)
				{
					return executionContext.CreateCopy();
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					lock (this._Lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				executionContext = this._Context;
				if (executionContext != null)
				{
					return executionContext.CreateCopy();
				}
				return null;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x0006FBB4 File Offset: 0x0006EBB4
		internal WindowsIdentity Identity
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				if (this._Wi != null)
				{
					return this._Wi;
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					lock (this._Lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				return this._Wi;
			}
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0006FC38 File Offset: 0x0006EC38
		internal object StartPostingAsyncOp()
		{
			return this.StartPostingAsyncOp(true);
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x0006FC41 File Offset: 0x0006EC41
		internal object StartPostingAsyncOp(bool lockCapture)
		{
			this._Lock = (lockCapture ? new object() : null);
			this._Flags |= ContextAwareResult.StateFlags.PostBlockStarted;
			return this._Lock;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x0006FC68 File Offset: 0x0006EC68
		internal bool FinishPostingAsyncOp()
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			ExecutionContext executionContext = null;
			return this.CaptureOrComplete(ref executionContext, false);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0006FCA0 File Offset: 0x0006ECA0
		internal bool FinishPostingAsyncOp(ref CallbackClosure closure)
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			CallbackClosure callbackClosure = closure;
			ExecutionContext executionContext;
			if (callbackClosure == null)
			{
				executionContext = null;
			}
			else if (!callbackClosure.IsCompatible(base.AsyncCallback))
			{
				closure = null;
				executionContext = null;
			}
			else
			{
				base.AsyncCallback = callbackClosure.AsyncCallback;
				executionContext = callbackClosure.Context;
			}
			bool flag = this.CaptureOrComplete(ref executionContext, true);
			if (closure == null && base.AsyncCallback != null && executionContext != null)
			{
				closure = new CallbackClosure(executionContext, base.AsyncCallback);
			}
			return flag;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0006FD24 File Offset: 0x0006ED24
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this._Wi != null)
			{
				this._Wi.Dispose();
				this._Wi = null;
			}
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0006FD48 File Offset: 0x0006ED48
		private bool CaptureOrComplete(ref ExecutionContext cachedContext, bool returnContext)
		{
			bool flag = base.AsyncCallback != null || (this._Flags & ContextAwareResult.StateFlags.CaptureContext) != ContextAwareResult.StateFlags.None;
			if ((this._Flags & ContextAwareResult.StateFlags.CaptureIdentity) != ContextAwareResult.StateFlags.None && !base.InternalPeekCompleted && (!flag || SecurityContext.IsWindowsIdentityFlowSuppressed()))
			{
				this.SafeCaptureIdenity();
			}
			if (flag && !base.InternalPeekCompleted)
			{
				if (cachedContext == null)
				{
					cachedContext = ExecutionContext.Capture();
				}
				if (cachedContext != null)
				{
					if (!returnContext)
					{
						this._Context = cachedContext;
						cachedContext = null;
					}
					else
					{
						this._Context = cachedContext.CreateCopy();
					}
				}
			}
			else
			{
				cachedContext = null;
			}
			if (base.CompletedSynchronously)
			{
				base.Complete(IntPtr.Zero);
				return true;
			}
			return false;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0006FDE8 File Offset: 0x0006EDE8
		protected override void Complete(IntPtr userToken)
		{
			if ((this._Flags & ContextAwareResult.StateFlags.PostBlockStarted) == ContextAwareResult.StateFlags.None)
			{
				base.Complete(userToken);
				return;
			}
			if (base.CompletedSynchronously)
			{
				return;
			}
			ExecutionContext context = this._Context;
			if (userToken != IntPtr.Zero || context == null)
			{
				base.Complete(userToken);
				return;
			}
			ExecutionContext.Run(((this._Flags & ContextAwareResult.StateFlags.ThreadSafeContextCopy) != ContextAwareResult.StateFlags.None) ? context.CreateCopy() : context, new ContextCallback(this.CompleteCallback), null);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x0006FE56 File Offset: 0x0006EE56
		private void CompleteCallback(object state)
		{
			base.Complete(IntPtr.Zero);
		}

		// Token: 0x04001D67 RID: 7527
		private volatile ExecutionContext _Context;

		// Token: 0x04001D68 RID: 7528
		private object _Lock;

		// Token: 0x04001D69 RID: 7529
		private ContextAwareResult.StateFlags _Flags;

		// Token: 0x04001D6A RID: 7530
		private WindowsIdentity _Wi;

		// Token: 0x020003A5 RID: 933
		[Flags]
		private enum StateFlags
		{
			// Token: 0x04001D6C RID: 7532
			None = 0,
			// Token: 0x04001D6D RID: 7533
			CaptureIdentity = 1,
			// Token: 0x04001D6E RID: 7534
			CaptureContext = 2,
			// Token: 0x04001D6F RID: 7535
			ThreadSafeContextCopy = 4,
			// Token: 0x04001D70 RID: 7536
			PostBlockStarted = 8,
			// Token: 0x04001D71 RID: 7537
			PostBlockFinished = 16
		}
	}
}
