using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Runtime.Remoting
{
	// Token: 0x02000728 RID: 1832
	internal class RedirectionProxy : MarshalByRefObject, IMessageSink
	{
		// Token: 0x06004216 RID: 16918 RVA: 0x000E1C40 File Offset: 0x000E0C40
		internal RedirectionProxy(MarshalByRefObject proxy, Type serverType)
		{
			this._proxy = proxy;
			this._realProxy = RemotingServices.GetRealProxy(this._proxy);
			this._serverType = serverType;
			this._objectMode = WellKnownObjectMode.Singleton;
		}

		// Token: 0x17000BA7 RID: 2983
		// (set) Token: 0x06004217 RID: 16919 RVA: 0x000E1C6E File Offset: 0x000E0C6E
		public WellKnownObjectMode ObjectMode
		{
			set
			{
				this._objectMode = value;
			}
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x000E1C78 File Offset: 0x000E0C78
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage message = null;
			try
			{
				msg.Properties["__Uri"] = this._realProxy.IdentityObject.URI;
				if (this._objectMode == WellKnownObjectMode.Singleton)
				{
					message = this._realProxy.Invoke(msg);
				}
				else
				{
					MarshalByRefObject marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._serverType, true);
					RealProxy realProxy = RemotingServices.GetRealProxy(marshalByRefObject);
					message = realProxy.Invoke(msg);
				}
			}
			catch (Exception ex)
			{
				message = new ReturnMessage(ex, msg as IMethodCallMessage);
			}
			return message;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x000E1D04 File Offset: 0x000E0D04
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMessage message = this.SyncProcessMessage(msg);
			if (replySink != null)
			{
				replySink.SyncProcessMessage(message);
			}
			return null;
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x000E1D27 File Offset: 0x000E0D27
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040020EF RID: 8431
		private MarshalByRefObject _proxy;

		// Token: 0x040020F0 RID: 8432
		private RealProxy _realProxy;

		// Token: 0x040020F1 RID: 8433
		private Type _serverType;

		// Token: 0x040020F2 RID: 8434
		private WellKnownObjectMode _objectMode;
	}
}
