using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	internal class FastRSCPObjRef : ObjRef
	{
		// Token: 0x06000121 RID: 289 RVA: 0x000053CE File Offset: 0x000043CE
		internal FastRSCPObjRef(IntPtr pUnk, Type serverType, string uri)
		{
			this._pUnk = pUnk;
			this._serverType = serverType;
			this.URI = uri;
			this.TypeInfo = new SCMTypeName(serverType);
			this.ChannelInfo = new SCMChannelInfo();
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005404 File Offset: 0x00004404
		public override object GetRealObject(StreamingContext context)
		{
			RealProxy realProxy = new RemoteServicedComponentProxy(this._serverType, this._pUnk, false);
			this._rp = realProxy;
			return (MarshalByRefObject)realProxy.GetTransparentProxy();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005438 File Offset: 0x00004438
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			ComponentServices.InitializeRemotingChannels();
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			object data = CallContext.GetData("__ClientIsClr");
			bool flag = data != null && (bool)data;
			if (!flag)
			{
				base.GetObjectData(info, context);
				info.SetType(typeof(ServicedComponentMarshaler));
				info.AddValue("servertype", this._rp.GetProxiedType());
				byte[] dcombuffer = ComponentServices.GetDCOMBuffer((MarshalByRefObject)this._rp.GetTransparentProxy());
				if (dcombuffer != null)
				{
					info.AddValue("dcomInfo", dcombuffer);
				}
				return;
			}
			RemoteServicedComponentProxy remoteServicedComponentProxy = this._rp as RemoteServicedComponentProxy;
			if (remoteServicedComponentProxy != null)
			{
				ObjRef objRef = RemotingServices.Marshal((MarshalByRefObject)remoteServicedComponentProxy.RemotingIntermediary.GetTransparentProxy(), null, null);
				objRef.GetObjectData(info, context);
				return;
			}
			base.GetObjectData(info, context);
		}

		// Token: 0x04000084 RID: 132
		private IntPtr _pUnk;

		// Token: 0x04000085 RID: 133
		private Type _serverType;

		// Token: 0x04000086 RID: 134
		private RealProxy _rp;
	}
}
