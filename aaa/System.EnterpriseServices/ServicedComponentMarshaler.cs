using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	internal class ServicedComponentMarshaler : ObjRef
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00005129 File Offset: 0x00004129
		private ServicedComponentMarshaler()
		{
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005134 File Offset: 0x00004134
		protected ServicedComponentMarshaler(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			byte[] array = null;
			Type type = null;
			bool flag = false;
			ComponentServices.InitializeRemotingChannels();
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("servertype"))
				{
					type = (Type)enumerator.Value;
				}
				else if (enumerator.Name.Equals("dcomInfo"))
				{
					array = (byte[])enumerator.Value;
				}
				else if (enumerator.Name.Equals("fIsMarshalled"))
				{
					object value = enumerator.Value;
					int num;
					if (value.GetType() == typeof(string))
					{
						num = ((IConvertible)value).ToInt32(null);
					}
					else
					{
						num = (int)value;
					}
					if (num == 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this._marshalled = true;
			}
			this._um = new SCUnMarshaler(type, array);
			this._rt = type;
			if (base.IsFromThisProcess() && !ServicedComponentInfo.IsTypeEventSource(type))
			{
				this._rp = RemotingServices.GetRealProxy(base.GetRealObject(context));
			}
			else
			{
				if (ServicedComponentInfo.IsTypeEventSource(type))
				{
					this.TypeInfo = new SCMTypeName(type);
				}
				object realObject = base.GetRealObject(context);
				this._rp = RemotingServices.GetRealProxy(realObject);
			}
			this._um.Dispose();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005272 File Offset: 0x00004272
		internal ServicedComponentMarshaler(MarshalByRefObject o, Type requestedType)
			: base(o, requestedType)
		{
			this._rp = RemotingServices.GetRealProxy(o);
			this._rt = requestedType;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000528F File Offset: 0x0000428F
		private bool IsMarshaledObject
		{
			get
			{
				return this._marshalled;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005298 File Offset: 0x00004298
		public override object GetRealObject(StreamingContext context)
		{
			if (!this.IsMarshaledObject)
			{
				return this;
			}
			if (base.IsFromThisProcess() && !ServicedComponentInfo.IsTypeEventSource(this._rt))
			{
				object realObject = base.GetRealObject(context);
				((ServicedComponent)realObject).DoSetCOMIUnknown(IntPtr.Zero);
				return realObject;
			}
			if (this._rp == null)
			{
				this._rp = this._um.GetRealProxy();
			}
			return this._rp.GetTransparentProxy();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005304 File Offset: 0x00004304
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

		// Token: 0x04000080 RID: 128
		private RealProxy _rp;

		// Token: 0x04000081 RID: 129
		private SCUnMarshaler _um;

		// Token: 0x04000082 RID: 130
		private Type _rt;

		// Token: 0x04000083 RID: 131
		private bool _marshalled;
	}
}
