using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200078B RID: 1931
	internal class ClientContextTerminatorSink : InternalSink, IMessageSink
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x000EC590 File Offset: 0x000EB590
		internal static IMessageSink MessageSink
		{
			get
			{
				if (ClientContextTerminatorSink.messageSink == null)
				{
					ClientContextTerminatorSink clientContextTerminatorSink = new ClientContextTerminatorSink();
					lock (ClientContextTerminatorSink.staticSyncObject)
					{
						if (ClientContextTerminatorSink.messageSink == null)
						{
							ClientContextTerminatorSink.messageSink = clientContextTerminatorSink;
						}
					}
				}
				return ClientContextTerminatorSink.messageSink;
			}
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x000EC5E4 File Offset: 0x000EB5E4
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessage message = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			return messageSink.SyncProcessMessage(message);
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x000EC60C File Offset: 0x000EB60C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			Context currentContext = Thread.CurrentContext;
			bool flag = currentContext.NotifyDynamicSinks(reqMsg, true, true, false, true);
			IMessage message2;
			if (reqMsg is IConstructionCallMessage)
			{
				message = currentContext.NotifyActivatorProperties(reqMsg, false);
				if (message != null)
				{
					return message;
				}
				message2 = ((IConstructionCallMessage)reqMsg).Activator.Activate((IConstructionCallMessage)reqMsg);
				message = currentContext.NotifyActivatorProperties(message2, false);
				if (message != null)
				{
					return message;
				}
			}
			else
			{
				ChannelServices.NotifyProfiler(reqMsg, RemotingProfilerEvent.ClientSend);
				object[] array = new object[2];
				object[] array2 = array;
				IMessageSink channelSink = this.GetChannelSink(reqMsg);
				array2[0] = reqMsg;
				array2[1] = channelSink;
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(ClientContextTerminatorSink.SyncProcessMessageCallback);
				if (channelSink != CrossContextChannel.MessageSink)
				{
					message2 = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(Context.DefaultContext, internalCrossContextDelegate, array2);
				}
				else
				{
					message2 = (IMessage)internalCrossContextDelegate(array2);
				}
				ChannelServices.NotifyProfiler(message2, RemotingProfilerEvent.ClientReceive);
			}
			if (flag)
			{
				currentContext.NotifyDynamicSinks(reqMsg, true, false, false, true);
			}
			return message2;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x000EC6F4 File Offset: 0x000EB6F4
		internal static object AsyncProcessMessageCallback(object[] args)
		{
			IMessage message = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			IMessageSink messageSink2 = (IMessageSink)args[2];
			return messageSink2.AsyncProcessMessage(message, messageSink);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x000EC724 File Offset: 0x000EB724
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			IMessageCtrl messageCtrl = null;
			if (message == null)
			{
				message = InternalSink.DisallowAsyncActivation(reqMsg);
			}
			if (message != null)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			else
			{
				if (RemotingServices.CORProfilerTrackRemotingAsync())
				{
					Guid guid;
					RemotingServices.CORProfilerRemotingClientSendingMessage(out guid, true);
					if (RemotingServices.CORProfilerTrackRemotingCookie())
					{
						reqMsg.Properties["CORProfilerCookie"] = guid;
					}
					if (replySink != null)
					{
						IMessageSink messageSink = new ClientAsyncReplyTerminatorSink(replySink);
						replySink = messageSink;
					}
				}
				Context currentContext = Thread.CurrentContext;
				currentContext.NotifyDynamicSinks(reqMsg, true, true, true, true);
				if (replySink != null)
				{
					replySink = new AsyncReplySink(replySink, currentContext);
				}
				object[] array = new object[3];
				object[] array2 = array;
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(ClientContextTerminatorSink.AsyncProcessMessageCallback);
				IMessageSink channelSink = this.GetChannelSink(reqMsg);
				array2[0] = reqMsg;
				array2[1] = replySink;
				array2[2] = channelSink;
				if (channelSink != CrossContextChannel.MessageSink)
				{
					messageCtrl = (IMessageCtrl)Thread.CurrentThread.InternalCrossContextCallback(Context.DefaultContext, internalCrossContextDelegate, array2);
				}
				else
				{
					messageCtrl = (IMessageCtrl)internalCrossContextDelegate(array2);
				}
			}
			return messageCtrl;
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x000EC81B File Offset: 0x000EB81B
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x000EC820 File Offset: 0x000EB820
		private IMessageSink GetChannelSink(IMessage reqMsg)
		{
			Identity identity = InternalSink.GetIdentity(reqMsg);
			return identity.ChannelSink;
		}

		// Token: 0x04002247 RID: 8775
		private static ClientContextTerminatorSink messageSink;

		// Token: 0x04002248 RID: 8776
		private static object staticSyncObject = new object();
	}
}
