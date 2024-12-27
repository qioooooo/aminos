using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000787 RID: 1927
	internal class SynchronizedClientContextSink : InternalSink, IMessageSink
	{
		// Token: 0x0600452B RID: 17707 RVA: 0x000EC272 File Offset: 0x000EB272
		internal SynchronizedClientContextSink(SynchronizationAttribute prop, IMessageSink nextSink)
		{
			this._property = prop;
			this._nextSink = nextSink;
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x000EC288 File Offset: 0x000EB288
		~SynchronizedClientContextSink()
		{
			this._property.Dispose();
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x000EC2BC File Offset: 0x000EB2BC
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message;
			if (this._property.IsReEntrant)
			{
				this._property.HandleThreadExit();
				message = this._nextSink.SyncProcessMessage(reqMsg);
				this._property.HandleThreadReEntry();
			}
			else
			{
				LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
				string text = logicalCallContext.RemotingData.LogicalCallID;
				bool flag = false;
				if (text == null)
				{
					text = Identity.GetNewLogicalCallID();
					logicalCallContext.RemotingData.LogicalCallID = text;
					flag = true;
				}
				bool flag2 = false;
				if (this._property.SyncCallOutLCID == null)
				{
					this._property.SyncCallOutLCID = text;
					flag2 = true;
				}
				message = this._nextSink.SyncProcessMessage(reqMsg);
				if (flag2)
				{
					this._property.SyncCallOutLCID = null;
					if (flag)
					{
						LogicalCallContext logicalCallContext2 = (LogicalCallContext)message.Properties[Message.CallContextKey];
						logicalCallContext2.RemotingData.LogicalCallID = null;
					}
				}
			}
			return message;
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x000EC3A0 File Offset: 0x000EB3A0
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			if (!this._property.IsReEntrant)
			{
				LogicalCallContext logicalCallContext = (LogicalCallContext)reqMsg.Properties[Message.CallContextKey];
				string newLogicalCallID = Identity.GetNewLogicalCallID();
				logicalCallContext.RemotingData.LogicalCallID = newLogicalCallID;
				this._property.AsyncCallOutLCIDList.Add(newLogicalCallID);
			}
			SynchronizedClientContextSink.AsyncReplySink asyncReplySink = new SynchronizedClientContextSink.AsyncReplySink(replySink, this._property);
			return this._nextSink.AsyncProcessMessage(reqMsg, asyncReplySink);
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600452F RID: 17711 RVA: 0x000EC412 File Offset: 0x000EB412
		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x0400223C RID: 8764
		internal IMessageSink _nextSink;

		// Token: 0x0400223D RID: 8765
		internal SynchronizationAttribute _property;

		// Token: 0x02000788 RID: 1928
		internal class AsyncReplySink : IMessageSink
		{
			// Token: 0x06004530 RID: 17712 RVA: 0x000EC41A File Offset: 0x000EB41A
			internal AsyncReplySink(IMessageSink nextSink, SynchronizationAttribute prop)
			{
				this._nextSink = nextSink;
				this._property = prop;
			}

			// Token: 0x06004531 RID: 17713 RVA: 0x000EC430 File Offset: 0x000EB430
			public virtual IMessage SyncProcessMessage(IMessage reqMsg)
			{
				WorkItem workItem = new WorkItem(reqMsg, this._nextSink, null);
				this._property.HandleWorkRequest(workItem);
				if (!this._property.IsReEntrant)
				{
					this._property.AsyncCallOutLCIDList.Remove(((LogicalCallContext)reqMsg.Properties[Message.CallContextKey]).RemotingData.LogicalCallID);
				}
				return workItem.ReplyMessage;
			}

			// Token: 0x06004532 RID: 17714 RVA: 0x000EC499 File Offset: 0x000EB499
			public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000C37 RID: 3127
			// (get) Token: 0x06004533 RID: 17715 RVA: 0x000EC4A0 File Offset: 0x000EB4A0
			public IMessageSink NextSink
			{
				get
				{
					return this._nextSink;
				}
			}

			// Token: 0x0400223E RID: 8766
			internal IMessageSink _nextSink;

			// Token: 0x0400223F RID: 8767
			internal SynchronizationAttribute _property;
		}
	}
}
