using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B4 RID: 1716
	internal class CrossContextChannel : InternalSink, IMessageSink
	{
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x000D6D4E File Offset: 0x000D5D4E
		// (set) Token: 0x06003E6A RID: 15978 RVA: 0x000D6D64 File Offset: 0x000D5D64
		private static CrossContextChannel messageSink
		{
			get
			{
				return Thread.GetDomain().RemotingData.ChannelServicesData.xctxmessageSink;
			}
			set
			{
				Thread.GetDomain().RemotingData.ChannelServicesData.xctxmessageSink = value;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x000D6D7C File Offset: 0x000D5D7C
		internal static IMessageSink MessageSink
		{
			get
			{
				if (CrossContextChannel.messageSink == null)
				{
					CrossContextChannel crossContextChannel = new CrossContextChannel();
					lock (CrossContextChannel.staticSyncObject)
					{
						if (CrossContextChannel.messageSink == null)
						{
							CrossContextChannel.messageSink = crossContextChannel;
						}
					}
				}
				return CrossContextChannel.messageSink;
			}
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x000D6DD0 File Offset: 0x000D5DD0
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessage message = args[0] as IMessage;
			Context context = args[1] as Context;
			if (RemotingServices.CORProfilerTrackRemoting())
			{
				Guid guid = Guid.Empty;
				if (RemotingServices.CORProfilerTrackRemotingCookie())
				{
					object obj = message.Properties["CORProfilerCookie"];
					if (obj != null)
					{
						guid = (Guid)obj;
					}
				}
				RemotingServices.CORProfilerRemotingServerReceivingMessage(guid, false);
			}
			context.NotifyDynamicSinks(message, false, true, false, true);
			IMessage message2 = context.GetServerContextChain().SyncProcessMessage(message);
			context.NotifyDynamicSinks(message2, false, false, false, true);
			if (RemotingServices.CORProfilerTrackRemoting())
			{
				Guid guid2;
				RemotingServices.CORProfilerRemotingServerSendingReply(out guid2, false);
				if (RemotingServices.CORProfilerTrackRemotingCookie())
				{
					message2.Properties["CORProfilerCookie"] = guid2;
				}
			}
			return message2;
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x000D6E80 File Offset: 0x000D5E80
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			object[] array = new object[2];
			object[] array2 = array;
			IMessage message = null;
			try
			{
				IMessage message2 = InternalSink.ValidateMessage(reqMsg);
				if (message2 != null)
				{
					return message2;
				}
				ServerIdentity serverIdentity = InternalSink.GetServerIdentity(reqMsg);
				array2[0] = reqMsg;
				array2[1] = serverIdentity.ServerContext;
				message = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(serverIdentity.ServerContext, CrossContextChannel.s_xctxDel, array2);
			}
			catch (Exception ex)
			{
				message = new ReturnMessage(ex, (IMethodCallMessage)reqMsg);
				if (reqMsg != null)
				{
					((ReturnMessage)message).SetLogicalCallContext((LogicalCallContext)reqMsg.Properties[Message.CallContextKey]);
				}
			}
			return message;
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x000D6F28 File Offset: 0x000D5F28
		internal static object AsyncProcessMessageCallback(object[] args)
		{
			AsyncWorkItem asyncWorkItem = null;
			IMessage message = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			Context context = (Context)args[2];
			Context context2 = (Context)args[3];
			if (messageSink != null)
			{
				asyncWorkItem = new AsyncWorkItem(messageSink, context);
			}
			context2.NotifyDynamicSinks(message, false, true, true, true);
			return context2.GetServerContextChain().AsyncProcessMessage(message, asyncWorkItem);
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x000D6F8C File Offset: 0x000D5F8C
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			object[] array = new object[4];
			object[] array2 = array;
			IMessageCtrl messageCtrl = null;
			if (message != null)
			{
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			else
			{
				ServerIdentity serverIdentity = InternalSink.GetServerIdentity(reqMsg);
				if (RemotingServices.CORProfilerTrackRemotingAsync())
				{
					Guid guid = Guid.Empty;
					if (RemotingServices.CORProfilerTrackRemotingCookie())
					{
						object obj = reqMsg.Properties["CORProfilerCookie"];
						if (obj != null)
						{
							guid = (Guid)obj;
						}
					}
					RemotingServices.CORProfilerRemotingServerReceivingMessage(guid, true);
					if (replySink != null)
					{
						IMessageSink messageSink = new ServerAsyncReplyTerminatorSink(replySink);
						replySink = messageSink;
					}
				}
				Context serverContext = serverIdentity.ServerContext;
				if (serverContext.IsThreadPoolAware)
				{
					array2[0] = reqMsg;
					array2[1] = replySink;
					array2[2] = Thread.CurrentContext;
					array2[3] = serverContext;
					InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(CrossContextChannel.AsyncProcessMessageCallback);
					messageCtrl = (IMessageCtrl)Thread.CurrentThread.InternalCrossContextCallback(serverContext, internalCrossContextDelegate, array2);
				}
				else
				{
					AsyncWorkItem asyncWorkItem = new AsyncWorkItem(reqMsg, replySink, Thread.CurrentContext, serverIdentity);
					WaitCallback waitCallback = new WaitCallback(asyncWorkItem.FinishAsyncWork);
					ThreadPool.QueueUserWorkItem(waitCallback);
				}
			}
			return messageCtrl;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x000D708C File Offset: 0x000D608C
		internal static object DoAsyncDispatchCallback(object[] args)
		{
			AsyncWorkItem asyncWorkItem = null;
			IMessage message = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			Context context = (Context)args[2];
			Context context2 = (Context)args[3];
			if (messageSink != null)
			{
				asyncWorkItem = new AsyncWorkItem(messageSink, context);
			}
			return context2.GetServerContextChain().AsyncProcessMessage(message, asyncWorkItem);
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x000D70E0 File Offset: 0x000D60E0
		internal static IMessageCtrl DoAsyncDispatch(IMessage reqMsg, IMessageSink replySink)
		{
			object[] array = new object[4];
			object[] array2 = array;
			ServerIdentity serverIdentity = InternalSink.GetServerIdentity(reqMsg);
			if (RemotingServices.CORProfilerTrackRemotingAsync())
			{
				Guid guid = Guid.Empty;
				if (RemotingServices.CORProfilerTrackRemotingCookie())
				{
					object obj = reqMsg.Properties["CORProfilerCookie"];
					if (obj != null)
					{
						guid = (Guid)obj;
					}
				}
				RemotingServices.CORProfilerRemotingServerReceivingMessage(guid, true);
				if (replySink != null)
				{
					IMessageSink messageSink = new ServerAsyncReplyTerminatorSink(replySink);
					replySink = messageSink;
				}
			}
			Context serverContext = serverIdentity.ServerContext;
			array2[0] = reqMsg;
			array2[1] = replySink;
			array2[2] = Thread.CurrentContext;
			array2[3] = serverContext;
			InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(CrossContextChannel.DoAsyncDispatchCallback);
			return (IMessageCtrl)Thread.CurrentThread.InternalCrossContextCallback(serverContext, internalCrossContextDelegate, array2);
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003E72 RID: 15986 RVA: 0x000D718A File Offset: 0x000D618A
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04001F98 RID: 8088
		private const string _channelName = "XCTX";

		// Token: 0x04001F99 RID: 8089
		private const int _channelCapability = 0;

		// Token: 0x04001F9A RID: 8090
		private const string _channelURI = "XCTX_URI";

		// Token: 0x04001F9B RID: 8091
		private static object staticSyncObject = new object();

		// Token: 0x04001F9C RID: 8092
		private static InternalCrossContextDelegate s_xctxDel = new InternalCrossContextDelegate(CrossContextChannel.SyncProcessMessageCallback);
	}
}
