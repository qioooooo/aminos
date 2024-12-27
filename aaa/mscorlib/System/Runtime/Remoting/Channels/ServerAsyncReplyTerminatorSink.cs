using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200069D RID: 1693
	internal class ServerAsyncReplyTerminatorSink : IMessageSink
	{
		// Token: 0x06003D9D RID: 15773 RVA: 0x000D39E8 File Offset: 0x000D29E8
		internal ServerAsyncReplyTerminatorSink(IMessageSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x000D39F8 File Offset: 0x000D29F8
		public virtual IMessage SyncProcessMessage(IMessage replyMsg)
		{
			Guid guid;
			RemotingServices.CORProfilerRemotingServerSendingReply(out guid, true);
			if (RemotingServices.CORProfilerTrackRemotingCookie())
			{
				replyMsg.Properties["CORProfilerCookie"] = guid;
			}
			return this._nextSink.SyncProcessMessage(replyMsg);
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x000D3A36 File Offset: 0x000D2A36
		public virtual IMessageCtrl AsyncProcessMessage(IMessage replyMsg, IMessageSink replySink)
		{
			return null;
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x000D3A39 File Offset: 0x000D2A39
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x04001F48 RID: 8008
		internal IMessageSink _nextSink;
	}
}
