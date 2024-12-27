using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200078C RID: 1932
	internal class AsyncReplySink : IMessageSink
	{
		// Token: 0x06004544 RID: 17732 RVA: 0x000EC84E File Offset: 0x000EB84E
		internal AsyncReplySink(IMessageSink replySink, Context cliCtx)
		{
			this._replySink = replySink;
			this._cliCtx = cliCtx;
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x000EC864 File Offset: 0x000EB864
		internal static object SyncProcessMessageCallback(object[] args)
		{
			IMessage message = (IMessage)args[0];
			IMessageSink messageSink = (IMessageSink)args[1];
			Thread.CurrentContext.NotifyDynamicSinks(message, true, false, true, true);
			return messageSink.SyncProcessMessage(message);
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x000EC89C File Offset: 0x000EB89C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = null;
			if (this._replySink != null)
			{
				object[] array = new object[] { reqMsg, this._replySink };
				InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(AsyncReplySink.SyncProcessMessageCallback);
				message = (IMessage)Thread.CurrentThread.InternalCrossContextCallback(this._cliCtx, internalCrossContextDelegate, array);
			}
			return message;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x000EC8EF File Offset: 0x000EB8EF
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x000EC8F6 File Offset: 0x000EB8F6
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x04002249 RID: 8777
		private IMessageSink _replySink;

		// Token: 0x0400224A RID: 8778
		private Context _cliCtx;
	}
}
