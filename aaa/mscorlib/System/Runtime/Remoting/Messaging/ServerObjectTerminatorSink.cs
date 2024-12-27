using System;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200078F RID: 1935
	[Serializable]
	internal class ServerObjectTerminatorSink : InternalSink, IMessageSink
	{
		// Token: 0x06004554 RID: 17748 RVA: 0x000ECAEB File Offset: 0x000EBAEB
		internal ServerObjectTerminatorSink(MarshalByRefObject srvObj)
		{
			this._stackBuilderSink = new StackBuilderSink(srvObj);
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x000ECB00 File Offset: 0x000EBB00
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			ServerIdentity serverIdentity = InternalSink.GetServerIdentity(reqMsg);
			ArrayWithSize serverSideDynamicSinks = serverIdentity.ServerSideDynamicSinks;
			if (serverSideDynamicSinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(reqMsg, serverSideDynamicSinks, false, true, false);
			}
			IMessageSink messageSink = this._stackBuilderSink.ServerObject as IMessageSink;
			IMessage message2;
			if (messageSink != null)
			{
				message2 = messageSink.SyncProcessMessage(reqMsg);
			}
			else
			{
				message2 = this._stackBuilderSink.SyncProcessMessage(reqMsg);
			}
			if (serverSideDynamicSinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(message2, serverSideDynamicSinks, false, false, false);
			}
			return message2;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x000ECB70 File Offset: 0x000EBB70
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			IMessageCtrl messageCtrl = null;
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			else
			{
				IMessageSink messageSink = this._stackBuilderSink.ServerObject as IMessageSink;
				if (messageSink != null)
				{
					messageCtrl = messageSink.AsyncProcessMessage(reqMsg, replySink);
				}
				else
				{
					messageCtrl = this._stackBuilderSink.AsyncProcessMessage(reqMsg, replySink);
				}
			}
			return messageCtrl;
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x000ECBC4 File Offset: 0x000EBBC4
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400224F RID: 8783
		internal StackBuilderSink _stackBuilderSink;
	}
}
