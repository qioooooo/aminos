using System;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200078A RID: 1930
	[Serializable]
	internal class EnvoyTerminatorSink : InternalSink, IMessageSink
	{
		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004535 RID: 17717 RVA: 0x000EC4C0 File Offset: 0x000EB4C0
		internal static IMessageSink MessageSink
		{
			get
			{
				if (EnvoyTerminatorSink.messageSink == null)
				{
					EnvoyTerminatorSink envoyTerminatorSink = new EnvoyTerminatorSink();
					lock (EnvoyTerminatorSink.staticSyncObject)
					{
						if (EnvoyTerminatorSink.messageSink == null)
						{
							EnvoyTerminatorSink.messageSink = envoyTerminatorSink;
						}
					}
				}
				return EnvoyTerminatorSink.messageSink;
			}
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x000EC514 File Offset: 0x000EB514
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			return Thread.CurrentContext.GetClientContextChain().SyncProcessMessage(reqMsg);
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x000EC540 File Offset: 0x000EB540
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
				messageCtrl = Thread.CurrentContext.GetClientContextChain().AsyncProcessMessage(reqMsg, replySink);
			}
			return messageCtrl;
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004538 RID: 17720 RVA: 0x000EC579 File Offset: 0x000EB579
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04002245 RID: 8773
		private static EnvoyTerminatorSink messageSink;

		// Token: 0x04002246 RID: 8774
		private static object staticSyncObject = new object();
	}
}
