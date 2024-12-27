using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000790 RID: 1936
	internal class ClientAsyncReplyTerminatorSink : IMessageSink
	{
		// Token: 0x06004558 RID: 17752 RVA: 0x000ECBC7 File Offset: 0x000EBBC7
		internal ClientAsyncReplyTerminatorSink(IMessageSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x000ECBD8 File Offset: 0x000EBBD8
		public virtual IMessage SyncProcessMessage(IMessage replyMsg)
		{
			Guid guid = Guid.Empty;
			if (RemotingServices.CORProfilerTrackRemotingCookie())
			{
				object obj = replyMsg.Properties["CORProfilerCookie"];
				if (obj != null)
				{
					guid = (Guid)obj;
				}
			}
			RemotingServices.CORProfilerRemotingClientReceivingReply(guid, true);
			return this._nextSink.SyncProcessMessage(replyMsg);
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x000ECC20 File Offset: 0x000EBC20
		public virtual IMessageCtrl AsyncProcessMessage(IMessage replyMsg, IMessageSink replySink)
		{
			return null;
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x000ECC23 File Offset: 0x000EBC23
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x04002250 RID: 8784
		internal IMessageSink _nextSink;
	}
}
