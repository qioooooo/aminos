using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Security.Permissions;

namespace System.EnterpriseServices
{
	// Token: 0x0200002D RID: 45
	internal class RemoteServicedComponentProxy : RealProxy
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004118 File Offset: 0x00003118
		private Type ProxiedType
		{
			get
			{
				if (this._pt == null)
				{
					this._pt = base.GetProxiedType();
				}
				return this._pt;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004134 File Offset: 0x00003134
		private RemoteServicedComponentProxy()
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000413C File Offset: 0x0000313C
		private void AssertValid()
		{
			if (this._server == null)
			{
				throw new ObjectDisposedException("ServicedComponent");
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004154 File Offset: 0x00003154
		private bool IsDisposeRequest(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				MethodBase methodBase = methodCallMessage.MethodBase;
				if (methodBase == RemoteServicedComponentProxy._getServicedComponentDispose || methodBase == RemoteServicedComponentProxy._getIDisposableDispose)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004188 File Offset: 0x00003188
		internal RemoteServicedComponentProxy(Type serverType, IntPtr pUnk, bool fAttachServer)
			: base(serverType)
		{
			this._fUseIntfDispatch = ServicedComponentInfo.IsTypeEventSource(serverType) || ServicedComponentInfo.AreMethodsSecure(serverType);
			if (pUnk != IntPtr.Zero)
			{
				this._pUnk = pUnk;
				this._server = EnterpriseServicesHelper.WrapIUnknownWithComObject(pUnk);
				if (fAttachServer)
				{
					base.AttachServer((MarshalByRefObject)this._server);
					this._fAttachedServer = true;
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000041F0 File Offset: 0x000031F0
		internal void Dispose(bool disposing)
		{
			object server = this._server;
			this._server = null;
			if (server != null)
			{
				this._pUnk = IntPtr.Zero;
				if (disposing)
				{
					Marshal.ReleaseComObject(server);
				}
				if (this._fAttachedServer)
				{
					base.DetachServer();
					this._fAttachedServer = false;
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000423C File Offset: 0x0000323C
		internal RemotingIntermediary RemotingIntermediary
		{
			get
			{
				if (this._intermediary == null)
				{
					lock (this)
					{
						if (this._intermediary == null)
						{
							this._intermediary = new RemotingIntermediary(this);
						}
					}
				}
				return this._intermediary;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004294 File Offset: 0x00003294
		public override IntPtr GetCOMIUnknown(bool fIsMarshalled)
		{
			if (this._server != null)
			{
				return Marshal.GetIUnknownForObject(this._server);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000042AF File Offset: 0x000032AF
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override void SetCOMIUnknown(IntPtr pUnk)
		{
			if (this._server == null)
			{
				this._pUnk = pUnk;
				this._server = EnterpriseServicesHelper.WrapIUnknownWithComObject(pUnk);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000042CC File Offset: 0x000032CC
		public override ObjRef CreateObjRef(Type requestedType)
		{
			return new ServicedComponentMarshaler((MarshalByRefObject)this.GetTransparentProxy(), requestedType);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000042E0 File Offset: 0x000032E0
		public override IMessage Invoke(IMessage reqMsg)
		{
			this.AssertValid();
			IMessage message = null;
			if (reqMsg is IConstructionCallMessage)
			{
				if (((IConstructionCallMessage)reqMsg).ArgCount > 0)
				{
					throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_ConstructorArguments"));
				}
				MarshalByRefObject marshalByRefObject = (MarshalByRefObject)this.GetTransparentProxy();
				return EnterpriseServicesHelper.CreateConstructionReturnMessage((IConstructionCallMessage)reqMsg, marshalByRefObject);
			}
			else
			{
				MethodBase methodBase = ((IMethodMessage)reqMsg).MethodBase;
				MemberInfo memberInfo = methodBase;
				if (methodBase == RemoteServicedComponentProxy._getTypeMethod)
				{
					IMethodCallMessage methodCallMessage = (IMethodCallMessage)reqMsg;
					return new ReturnMessage(this.ProxiedType, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
				}
				if (methodBase == RemoteServicedComponentProxy._getHashCodeMethod)
				{
					int hashCode = this.GetHashCode();
					IMethodCallMessage methodCallMessage2 = (IMethodCallMessage)reqMsg;
					return new ReturnMessage(hashCode, null, 0, methodCallMessage2.LogicalCallContext, methodCallMessage2);
				}
				MemberInfo memberInfo2 = ReflectionCache.ConvertToClassMI(this.ProxiedType, memberInfo);
				try
				{
					int num;
					if (this._fUseIntfDispatch || ((num = ServicedComponentInfo.MICachedLookup(memberInfo2)) & 4) != 0 || (num & 8) != 0)
					{
						MemberInfo memberInfo3 = ReflectionCache.ConvertToInterfaceMI(memberInfo);
						if (memberInfo3 == null)
						{
							throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_SecurityMapping"));
						}
						MethodCallMessageWrapperEx methodCallMessageWrapperEx = new MethodCallMessageWrapperEx((IMethodCallMessage)reqMsg, (MethodBase)memberInfo3);
						message = RemotingServices.ExecuteMessage((MarshalByRefObject)this._server, methodCallMessageWrapperEx);
					}
					else
					{
						bool flag = (num & 2) != 0;
						string text = ComponentServices.ConvertToString(reqMsg);
						IRemoteDispatch remoteDispatch = (IRemoteDispatch)this._server;
						string text2;
						if (flag)
						{
							text2 = remoteDispatch.RemoteDispatchAutoDone(text);
						}
						else
						{
							text2 = remoteDispatch.RemoteDispatchNotAutoDone(text);
						}
						message = ComponentServices.ConvertToReturnMessage(text2, reqMsg);
					}
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != Util.CONTEXT_E_ABORTED && ex.ErrorCode != Util.CONTEXT_E_ABORTING)
					{
						throw;
					}
					if (!this.IsDisposeRequest(reqMsg))
					{
						throw;
					}
					IMethodCallMessage methodCallMessage3 = reqMsg as IMethodCallMessage;
					message = new ReturnMessage(null, null, 0, methodCallMessage3.LogicalCallContext, methodCallMessage3);
				}
				if (this.IsDisposeRequest(reqMsg))
				{
					this.Dispose(true);
				}
				return message;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000044BC File Offset: 0x000034BC
		~RemoteServicedComponentProxy()
		{
			this.Dispose(false);
		}

		// Token: 0x04000059 RID: 89
		private IntPtr _pUnk;

		// Token: 0x0400005A RID: 90
		private object _server;

		// Token: 0x0400005B RID: 91
		private bool _fUseIntfDispatch;

		// Token: 0x0400005C RID: 92
		private bool _fAttachedServer;

		// Token: 0x0400005D RID: 93
		private volatile RemotingIntermediary _intermediary;

		// Token: 0x0400005E RID: 94
		private static MethodInfo _getTypeMethod = typeof(object).GetMethod("GetType");

		// Token: 0x0400005F RID: 95
		private static MethodInfo _getHashCodeMethod = typeof(object).GetMethod("GetHashCode");

		// Token: 0x04000060 RID: 96
		private static MethodBase _getIDisposableDispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);

		// Token: 0x04000061 RID: 97
		private static MethodBase _getServicedComponentDispose = typeof(ServicedComponent).GetMethod("Dispose", new Type[0]);

		// Token: 0x04000062 RID: 98
		private Type _pt;
	}
}
