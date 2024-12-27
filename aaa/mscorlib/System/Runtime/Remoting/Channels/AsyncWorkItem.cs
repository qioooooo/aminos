using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B5 RID: 1717
	internal class AsyncWorkItem : IMessageSink
	{
		// Token: 0x06003E75 RID: 15989 RVA: 0x000D71B2 File Offset: 0x000D61B2
		internal AsyncWorkItem(IMessageSink replySink, Context oldCtx)
			: this(null, replySink, oldCtx, null)
		{
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x000D71BE File Offset: 0x000D61BE
		internal AsyncWorkItem(IMessage reqMsg, IMessageSink replySink, Context oldCtx, ServerIdentity srvID)
		{
			this._reqMsg = reqMsg;
			this._replySink = replySink;
			this._oldCtx = oldCtx;
			this._callCtx = CallContext.GetLogicalCallContext();
			this._srvID = srvID;
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x000D71F0 File Offset: 0x000D61F0
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessageSink messageSink = (IMessageSink)args[0];
			IMessage message = (IMessage)args[1];
			return messageSink.SyncProcessMessage(message);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x000D7218 File Offset: 0x000D6218
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage message = null;
			if (this._replySink != null)
			{
				Thread.CurrentContext.NotifyDynamicSinks(msg, false, false, true, true);
				object[] array = new object[] { this._replySink, msg };
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(AsyncWorkItem.SyncProcessMessageCallback);
				message = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(this._oldCtx, internalCrossContextDelegate, array);
			}
			return message;
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x000D727B File Offset: 0x000D627B
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003E7A RID: 15994 RVA: 0x000D728C File Offset: 0x000D628C
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x000D7294 File Offset: 0x000D6294
		internal static object FinishAsyncWorkCallback(object[] args)
		{
			AsyncWorkItem asyncWorkItem = (AsyncWorkItem)args[0];
			Context serverContext = asyncWorkItem._srvID.ServerContext;
			LogicalCallContext logicalCallContext = CallContext.SetLogicalCallContext(asyncWorkItem._callCtx);
			serverContext.NotifyDynamicSinks(asyncWorkItem._reqMsg, false, true, true, true);
			serverContext.GetServerContextChain().AsyncProcessMessage(asyncWorkItem._reqMsg, asyncWorkItem);
			CallContext.SetLogicalCallContext(logicalCallContext);
			return null;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x000D72F0 File Offset: 0x000D62F0
		internal virtual void FinishAsyncWork(object stateIgnored)
		{
			InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(AsyncWorkItem.FinishAsyncWorkCallback);
			object[] array = new object[] { this };
			Thread.CurrentThread.InternalCrossContextCallback(this._srvID.ServerContext, internalCrossContextDelegate, array);
		}

		// Token: 0x04001F9D RID: 8093
		private IMessageSink _replySink;

		// Token: 0x04001F9E RID: 8094
		private ServerIdentity _srvID;

		// Token: 0x04001F9F RID: 8095
		private Context _oldCtx;

		// Token: 0x04001FA0 RID: 8096
		private LogicalCallContext _callCtx;

		// Token: 0x04001FA1 RID: 8097
		private IMessage _reqMsg;
	}
}
