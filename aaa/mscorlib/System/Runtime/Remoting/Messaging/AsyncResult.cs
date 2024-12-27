using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000690 RID: 1680
	[ComVisible(true)]
	public class AsyncResult : IAsyncResult, IMessageSink
	{
		// Token: 0x06003D22 RID: 15650 RVA: 0x000D1FF1 File Offset: 0x000D0FF1
		internal AsyncResult(Message m)
		{
			m.GetAsyncBeginInfo(out this._acbd, out this._asyncState);
			this._asyncDelegate = (Delegate)m.GetThisPtr();
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x000D201C File Offset: 0x000D101C
		public virtual bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003D24 RID: 15652 RVA: 0x000D2024 File Offset: 0x000D1024
		public virtual object AsyncDelegate
		{
			get
			{
				return this._asyncDelegate;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x000D202C File Offset: 0x000D102C
		public virtual object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06003D26 RID: 15654 RVA: 0x000D2034 File Offset: 0x000D1034
		public virtual bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06003D27 RID: 15655 RVA: 0x000D2037 File Offset: 0x000D1037
		// (set) Token: 0x06003D28 RID: 15656 RVA: 0x000D203F File Offset: 0x000D103F
		public bool EndInvokeCalled
		{
			get
			{
				return this._endInvokeCalled;
			}
			set
			{
				this._endInvokeCalled = value;
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x000D2048 File Offset: 0x000D1048
		private void FaultInWaitHandle()
		{
			lock (this)
			{
				if (this._AsyncWaitHandle == null)
				{
					this._AsyncWaitHandle = new ManualResetEvent(this._isCompleted);
				}
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x000D2090 File Offset: 0x000D1090
		public virtual WaitHandle AsyncWaitHandle
		{
			get
			{
				this.FaultInWaitHandle();
				return this._AsyncWaitHandle;
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x000D209E File Offset: 0x000D109E
		public virtual void SetMessageCtrl(IMessageCtrl mc)
		{
			this._mc = mc;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x000D20A8 File Offset: 0x000D10A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			if (msg == null)
			{
				this._replyMsg = new ReturnMessage(new RemotingException(Environment.GetResourceString("Remoting_NullMessage")), new ErrorMessage());
			}
			else if (!(msg is IMethodReturnMessage))
			{
				this._replyMsg = new ReturnMessage(new RemotingException(Environment.GetResourceString("Remoting_Message_BadType")), new ErrorMessage());
			}
			else
			{
				this._replyMsg = msg;
			}
			lock (this)
			{
				this._isCompleted = true;
				if (this._AsyncWaitHandle != null)
				{
					this._AsyncWaitHandle.Set();
				}
			}
			if (this._acbd != null)
			{
				this._acbd(this);
			}
			return null;
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x000D215C File Offset: 0x000D115C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000D216D File Offset: 0x000D116D
		public IMessageSink NextSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return null;
			}
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x000D2170 File Offset: 0x000D1170
		public virtual IMessage GetReplyMessage()
		{
			return this._replyMsg;
		}

		// Token: 0x04001F1F RID: 7967
		private IMessageCtrl _mc;

		// Token: 0x04001F20 RID: 7968
		private AsyncCallback _acbd;

		// Token: 0x04001F21 RID: 7969
		private IMessage _replyMsg;

		// Token: 0x04001F22 RID: 7970
		private bool _isCompleted;

		// Token: 0x04001F23 RID: 7971
		private bool _endInvokeCalled;

		// Token: 0x04001F24 RID: 7972
		private ManualResetEvent _AsyncWaitHandle;

		// Token: 0x04001F25 RID: 7973
		private Delegate _asyncDelegate;

		// Token: 0x04001F26 RID: 7974
		private object _asyncState;
	}
}
