using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000729 RID: 1833
	internal class ComRedirectionProxy : MarshalByRefObject, IMessageSink
	{
		// Token: 0x0600421B RID: 16923 RVA: 0x000E1D2A File Offset: 0x000E0D2A
		internal ComRedirectionProxy(MarshalByRefObject comObject, Type serverType)
		{
			this._comObject = comObject;
			this._serverType = serverType;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x000E1D40 File Offset: 0x000E0D40
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMethodReturnMessage methodReturnMessage = RemotingServices.ExecuteMessage(this._comObject, methodCallMessage);
			if (methodReturnMessage != null)
			{
				COMException ex = methodReturnMessage.Exception as COMException;
				if (ex != null && (ex._HResult == -2147023174 || ex._HResult == -2147023169))
				{
					this._comObject = (MarshalByRefObject)Activator.CreateInstance(this._serverType, true);
					methodReturnMessage = RemotingServices.ExecuteMessage(this._comObject, methodCallMessage);
				}
			}
			return methodReturnMessage;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x000E1DB4 File Offset: 0x000E0DB4
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMessage message = this.SyncProcessMessage(msg);
			if (replySink != null)
			{
				replySink.SyncProcessMessage(message);
			}
			return null;
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x000E1DD7 File Offset: 0x000E0DD7
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040020F3 RID: 8435
		private MarshalByRefObject _comObject;

		// Token: 0x040020F4 RID: 8436
		private Type _serverType;
	}
}
