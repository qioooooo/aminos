using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000751 RID: 1873
	internal class RemotingProxy : RealProxy, IRemotingTypeInfo
	{
		// Token: 0x060042FE RID: 17150 RVA: 0x000E590B File Offset: 0x000E490B
		public RemotingProxy(Type serverType)
			: base(serverType)
		{
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x000E5914 File Offset: 0x000E4914
		private RemotingProxy()
		{
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004300 RID: 17152 RVA: 0x000E591C File Offset: 0x000E491C
		// (set) Token: 0x06004301 RID: 17153 RVA: 0x000E5924 File Offset: 0x000E4924
		internal int CtorThread
		{
			get
			{
				return this._ctorThread;
			}
			set
			{
				this._ctorThread = value;
			}
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x000E5930 File Offset: 0x000E4930
		internal static IMessage CallProcessMessage(IMessageSink ms, IMessage reqMsg, ArrayWithSize proxySinks, Thread currentThread, Context currentContext, bool bSkippingContextChain)
		{
			if (proxySinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(reqMsg, proxySinks, true, true, false);
			}
			bool flag = false;
			if (bSkippingContextChain)
			{
				flag = currentContext.NotifyDynamicSinks(reqMsg, true, true, false, true);
				ChannelServices.NotifyProfiler(reqMsg, RemotingProfilerEvent.ClientSend);
			}
			if (ms == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_NoChannelSink"));
			}
			IMessage message = ms.SyncProcessMessage(reqMsg);
			if (bSkippingContextChain)
			{
				ChannelServices.NotifyProfiler(message, RemotingProfilerEvent.ClientReceive);
				if (flag)
				{
					currentContext.NotifyDynamicSinks(message, true, false, false, true);
				}
			}
			IMethodReturnMessage methodReturnMessage = message as IMethodReturnMessage;
			if (message == null || methodReturnMessage == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
			}
			if (proxySinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(message, proxySinks, true, false, false);
			}
			return message;
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x000E59C8 File Offset: 0x000E49C8
		public override IMessage Invoke(IMessage reqMsg)
		{
			IConstructionCallMessage constructionCallMessage = reqMsg as IConstructionCallMessage;
			if (constructionCallMessage != null)
			{
				return this.InternalActivate(constructionCallMessage);
			}
			if (!base.Initialized)
			{
				if (this.CtorThread != Thread.CurrentThread.GetHashCode())
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_InvalidCall"));
				}
				Identity identityObject = this.IdentityObject;
				RemotingServices.Wrap((ContextBoundObject)base.UnwrappedServerObject);
			}
			int num = 0;
			Message message = reqMsg as Message;
			if (message != null)
			{
				num = message.GetCallType();
			}
			return this.InternalInvoke((IMethodCallMessage)reqMsg, false, num);
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x000E5A4C File Offset: 0x000E4A4C
		internal virtual IMessage InternalInvoke(IMethodCallMessage reqMcmMsg, bool useDispatchMessage, int callType)
		{
			Message message = reqMcmMsg as Message;
			if (message == null && callType != 0)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_InvalidCallType"));
			}
			IMessage message2 = null;
			Thread currentThread = Thread.CurrentThread;
			LogicalCallContext logicalCallContext = currentThread.GetLogicalCallContext();
			Identity identityObject = this.IdentityObject;
			ServerIdentity serverIdentity = identityObject as ServerIdentity;
			if (serverIdentity != null && identityObject.IsFullyDisconnected())
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_ServerObjectNotFound"), new object[] { reqMcmMsg.Uri }));
			}
			MethodBase methodBase = reqMcmMsg.MethodBase;
			if (RemotingProxy._getTypeMethod == methodBase)
			{
				Type proxiedType = base.GetProxiedType();
				return new ReturnMessage(proxiedType, null, 0, logicalCallContext, reqMcmMsg);
			}
			if (RemotingProxy._getHashCodeMethod == methodBase)
			{
				int hashCode = identityObject.GetHashCode();
				return new ReturnMessage(hashCode, null, 0, logicalCallContext, reqMcmMsg);
			}
			if (identityObject.ChannelSink == null)
			{
				IMessageSink messageSink = null;
				IMessageSink messageSink2 = null;
				if (!identityObject.ObjectRef.IsObjRefLite())
				{
					RemotingServices.CreateEnvoyAndChannelSinks(null, identityObject.ObjectRef, out messageSink, out messageSink2);
				}
				else
				{
					RemotingServices.CreateEnvoyAndChannelSinks(identityObject.ObjURI, null, out messageSink, out messageSink2);
				}
				RemotingServices.SetEnvoyAndChannelSinks(identityObject, messageSink, messageSink2);
				if (identityObject.ChannelSink == null)
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_NoChannelSink"));
				}
			}
			IInternalMessage internalMessage = (IInternalMessage)reqMcmMsg;
			internalMessage.IdentityObject = identityObject;
			if (serverIdentity != null)
			{
				internalMessage.ServerIdentityObject = serverIdentity;
			}
			else
			{
				internalMessage.SetURI(identityObject.URI);
			}
			switch (callType)
			{
			case 0:
			{
				bool flag = false;
				Context currentContextInternal = currentThread.GetCurrentContextInternal();
				IMessageSink messageSink3 = identityObject.EnvoyChain;
				if (currentContextInternal.IsDefaultContext && messageSink3 is EnvoyTerminatorSink)
				{
					flag = true;
					messageSink3 = identityObject.ChannelSink;
				}
				message2 = RemotingProxy.CallProcessMessage(messageSink3, reqMcmMsg, identityObject.ProxySideDynamicSinks, currentThread, currentContextInternal, flag);
				break;
			}
			case 1:
			case 9:
			{
				logicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
				internalMessage.SetCallContext(logicalCallContext);
				AsyncResult asyncResult = new AsyncResult(message);
				this.InternalInvokeAsync(asyncResult, message, useDispatchMessage, callType);
				message2 = new ReturnMessage(asyncResult, null, 0, null, message);
				break;
			}
			case 2:
				message2 = RealProxy.EndInvokeHelper(message, true);
				break;
			case 8:
				logicalCallContext = (LogicalCallContext)logicalCallContext.Clone();
				internalMessage.SetCallContext(logicalCallContext);
				this.InternalInvokeAsync(null, message, useDispatchMessage, callType);
				message2 = new ReturnMessage(null, null, 0, null, reqMcmMsg);
				break;
			case 10:
				message2 = new ReturnMessage(null, null, 0, null, reqMcmMsg);
				break;
			}
			return message2;
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x000E5CAC File Offset: 0x000E4CAC
		internal void InternalInvokeAsync(IMessageSink ar, Message reqMsg, bool useDispatchMessage, int callType)
		{
			Identity identityObject = this.IdentityObject;
			ServerIdentity serverIdentity = identityObject as ServerIdentity;
			MethodCall methodCall = new MethodCall(reqMsg);
			IInternalMessage internalMessage = methodCall;
			internalMessage.IdentityObject = identityObject;
			if (serverIdentity != null)
			{
				internalMessage.ServerIdentityObject = serverIdentity;
			}
			if (useDispatchMessage)
			{
				ChannelServices.AsyncDispatchMessage(methodCall, ((callType & 8) != 0) ? null : ar);
			}
			else
			{
				if (identityObject.EnvoyChain == null)
				{
					throw new ExecutionEngineException(Environment.GetResourceString("Remoting_Proxy_InvalidState"));
				}
				identityObject.EnvoyChain.AsyncProcessMessage(methodCall, ((callType & 8) != 0) ? null : ar);
			}
			if ((callType & 1) != 0 && (callType & 8) != 0)
			{
				ar.SyncProcessMessage(null);
			}
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x000E5D3C File Offset: 0x000E4D3C
		private IConstructionReturnMessage InternalActivate(IConstructionCallMessage ctorMsg)
		{
			this.CtorThread = Thread.CurrentThread.GetHashCode();
			IConstructionReturnMessage constructionReturnMessage = ActivationServices.Activate(this, ctorMsg);
			base.Initialized = true;
			return constructionReturnMessage;
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x000E5D6C File Offset: 0x000E4D6C
		private static void Invoke(object NotUsed, ref MessageData msgData)
		{
			Message message = new Message();
			message.InitFields(msgData);
			object thisPtr = message.GetThisPtr();
			Delegate @delegate;
			if ((@delegate = thisPtr as Delegate) == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
			RemotingProxy remotingProxy = (RemotingProxy)RemotingServices.GetRealProxy(@delegate.Target);
			if (remotingProxy != null)
			{
				remotingProxy.InternalInvoke(message, true, message.GetCallType());
				return;
			}
			int callType = message.GetCallType();
			int num = callType;
			switch (num)
			{
			case 1:
				break;
			case 2:
				RealProxy.EndInvokeHelper(message, false);
				return;
			default:
				switch (num)
				{
				case 9:
					break;
				case 10:
					return;
				default:
					return;
				}
				break;
			}
			message.Properties[Message.CallContextKey] = CallContext.GetLogicalCallContext().Clone();
			AsyncResult asyncResult = new AsyncResult(message);
			AgileAsyncWorkerItem agileAsyncWorkerItem = new AgileAsyncWorkerItem(message, ((callType & 8) != 0) ? null : asyncResult, @delegate.Target);
			ThreadPool.QueueUserWorkItem(new WaitCallback(AgileAsyncWorkerItem.ThreadPoolCallBack), agileAsyncWorkerItem);
			if ((callType & 8) != 0)
			{
				asyncResult.SyncProcessMessage(null);
			}
			message.PropagateOutParameters(null, asyncResult);
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004308 RID: 17160 RVA: 0x000E5E73 File Offset: 0x000E4E73
		// (set) Token: 0x06004309 RID: 17161 RVA: 0x000E5E7B File Offset: 0x000E4E7B
		internal ConstructorCallMessage ConstructorMessage
		{
			get
			{
				return this._ccm;
			}
			set
			{
				this._ccm = value;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x000E5E84 File Offset: 0x000E4E84
		// (set) Token: 0x0600430B RID: 17163 RVA: 0x000E5E91 File Offset: 0x000E4E91
		public string TypeName
		{
			get
			{
				return base.GetProxiedType().FullName;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x000E5E98 File Offset: 0x000E4E98
		public override IntPtr GetCOMIUnknown(bool fIsBeingMarshalled)
		{
			IntPtr intPtr = IntPtr.Zero;
			object transparentProxy = this.GetTransparentProxy();
			bool flag = RemotingServices.IsObjectOutOfProcess(transparentProxy);
			if (flag)
			{
				if (fIsBeingMarshalled)
				{
					intPtr = MarshalByRefObject.GetComIUnknown((MarshalByRefObject)transparentProxy);
				}
				else
				{
					intPtr = MarshalByRefObject.GetComIUnknown((MarshalByRefObject)transparentProxy);
				}
			}
			else
			{
				bool flag2 = RemotingServices.IsObjectOutOfAppDomain(transparentProxy);
				if (flag2)
				{
					intPtr = ((MarshalByRefObject)transparentProxy).GetComIUnknown(fIsBeingMarshalled);
				}
				else
				{
					intPtr = MarshalByRefObject.GetComIUnknown((MarshalByRefObject)transparentProxy);
				}
			}
			return intPtr;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x000E5F01 File Offset: 0x000E4F01
		public override void SetCOMIUnknown(IntPtr i)
		{
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x000E5F04 File Offset: 0x000E4F04
		public bool CanCastTo(Type castType, object o)
		{
			bool flag = false;
			if (castType == RemotingProxy.s_typeofObject || castType == RemotingProxy.s_typeofMarshalByRefObject)
			{
				return true;
			}
			ObjRef objectRef = this.IdentityObject.ObjectRef;
			if (objectRef != null)
			{
				object transparentProxy = this.GetTransparentProxy();
				IRemotingTypeInfo typeInfo = objectRef.TypeInfo;
				if (typeInfo != null)
				{
					flag = typeInfo.CanCastTo(castType, transparentProxy);
					if (!flag && typeInfo.GetType() == typeof(TypeInfo) && objectRef.IsWellKnown())
					{
						flag = this.CanCastToWK(castType);
					}
				}
				else if (objectRef.IsObjRefLite())
				{
					flag = MarshalByRefObject.CanCastToXmlTypeHelper(castType, (MarshalByRefObject)o);
				}
			}
			else
			{
				flag = this.CanCastToWK(castType);
			}
			return flag;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x000E5F98 File Offset: 0x000E4F98
		private bool CanCastToWK(Type castType)
		{
			bool flag = false;
			if (castType.IsClass)
			{
				flag = base.GetProxiedType().IsAssignableFrom(castType);
			}
			else if (!(this.IdentityObject is ServerIdentity))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x04002187 RID: 8583
		private static MethodInfo _getTypeMethod = typeof(object).GetMethod("GetType");

		// Token: 0x04002188 RID: 8584
		private static MethodInfo _getHashCodeMethod = typeof(object).GetMethod("GetHashCode");

		// Token: 0x04002189 RID: 8585
		private static Type s_typeofObject = typeof(object);

		// Token: 0x0400218A RID: 8586
		private static Type s_typeofMarshalByRefObject = typeof(MarshalByRefObject);

		// Token: 0x0400218B RID: 8587
		private ConstructorCallMessage _ccm;

		// Token: 0x0400218C RID: 8588
		private int _ctorThread;
	}
}
