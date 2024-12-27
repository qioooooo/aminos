using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BC RID: 1724
	internal class ADAsyncWorkItem
	{
		// Token: 0x06003EA5 RID: 16037 RVA: 0x000D79C4 File Offset: 0x000D69C4
		internal ADAsyncWorkItem(IMessage reqMsg, IMessageSink nextSink, IMessageSink replySink)
		{
			this._reqMsg = reqMsg;
			this._nextSink = nextSink;
			this._replySink = replySink;
			this._callCtx = CallContext.GetLogicalCallContext();
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x000D79EC File Offset: 0x000D69EC
		internal virtual void FinishAsyncWork(object stateIgnored)
		{
			LogicalCallContext logicalCallContext = CallContext.SetLogicalCallContext(this._callCtx);
			IMessage message = this._nextSink.SyncProcessMessage(this._reqMsg);
			if (this._replySink != null)
			{
				this._replySink.SyncProcessMessage(message);
			}
			CallContext.SetLogicalCallContext(logicalCallContext);
		}

		// Token: 0x04001FB0 RID: 8112
		private IMessageSink _replySink;

		// Token: 0x04001FB1 RID: 8113
		private IMessageSink _nextSink;

		// Token: 0x04001FB2 RID: 8114
		private LogicalCallContext _callCtx;

		// Token: 0x04001FB3 RID: 8115
		private IMessage _reqMsg;
	}
}
