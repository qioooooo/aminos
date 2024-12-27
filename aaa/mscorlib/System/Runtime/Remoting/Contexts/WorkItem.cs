using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000786 RID: 1926
	internal class WorkItem
	{
		// Token: 0x0600451E RID: 17694 RVA: 0x000EC122 File Offset: 0x000EB122
		internal WorkItem(IMessage reqMsg, IMessageSink nextSink, IMessageSink replySink)
		{
			this._reqMsg = reqMsg;
			this._replyMsg = null;
			this._nextSink = nextSink;
			this._replySink = replySink;
			this._ctx = Thread.CurrentContext;
			this._callCtx = CallContext.GetLogicalCallContext();
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x000EC15C File Offset: 0x000EB15C
		internal virtual void SetWaiting()
		{
			this._flags |= 1;
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x000EC16C File Offset: 0x000EB16C
		internal virtual bool IsWaiting()
		{
			return (this._flags & 1) == 1;
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x000EC179 File Offset: 0x000EB179
		internal virtual void SetSignaled()
		{
			this._flags |= 2;
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x000EC189 File Offset: 0x000EB189
		internal virtual bool IsSignaled()
		{
			return (this._flags & 2) == 2;
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x000EC196 File Offset: 0x000EB196
		internal virtual void SetAsync()
		{
			this._flags |= 4;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x000EC1A6 File Offset: 0x000EB1A6
		internal virtual bool IsAsync()
		{
			return (this._flags & 4) == 4;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x000EC1B3 File Offset: 0x000EB1B3
		internal virtual void SetDummy()
		{
			this._flags |= 8;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x000EC1C3 File Offset: 0x000EB1C3
		internal virtual bool IsDummy()
		{
			return (this._flags & 8) == 8;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x000EC1D0 File Offset: 0x000EB1D0
		internal static object ExecuteCallback(object[] args)
		{
			WorkItem workItem = (WorkItem)args[0];
			if (workItem.IsAsync())
			{
				workItem._nextSink.AsyncProcessMessage(workItem._reqMsg, workItem._replySink);
			}
			else if (workItem._nextSink != null)
			{
				workItem._replyMsg = workItem._nextSink.SyncProcessMessage(workItem._reqMsg);
			}
			return null;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x000EC228 File Offset: 0x000EB228
		internal virtual void Execute()
		{
			Thread.CurrentThread.InternalCrossContextCallback(this._ctx, WorkItem._xctxDel, new object[] { this });
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004529 RID: 17705 RVA: 0x000EC257 File Offset: 0x000EB257
		internal virtual IMessage ReplyMessage
		{
			get
			{
				return this._replyMsg;
			}
		}

		// Token: 0x04002230 RID: 8752
		private const int FLG_WAITING = 1;

		// Token: 0x04002231 RID: 8753
		private const int FLG_SIGNALED = 2;

		// Token: 0x04002232 RID: 8754
		private const int FLG_ASYNC = 4;

		// Token: 0x04002233 RID: 8755
		private const int FLG_DUMMY = 8;

		// Token: 0x04002234 RID: 8756
		internal int _flags;

		// Token: 0x04002235 RID: 8757
		internal IMessage _reqMsg;

		// Token: 0x04002236 RID: 8758
		internal IMessageSink _nextSink;

		// Token: 0x04002237 RID: 8759
		internal IMessageSink _replySink;

		// Token: 0x04002238 RID: 8760
		internal IMessage _replyMsg;

		// Token: 0x04002239 RID: 8761
		internal Context _ctx;

		// Token: 0x0400223A RID: 8762
		internal LogicalCallContext _callCtx;

		// Token: 0x0400223B RID: 8763
		internal static InternalCrossContextDelegate _xctxDel = new InternalCrossContextDelegate(WorkItem.ExecuteCallback);
	}
}
