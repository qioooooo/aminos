using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000727 RID: 1831
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public abstract class RealProxy
	{
		// Token: 0x060041EF RID: 16879 RVA: 0x000E1162 File Offset: 0x000E0162
		protected RealProxy(Type classToProxy)
			: this(classToProxy, (IntPtr)0, null)
		{
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x000E1174 File Offset: 0x000E0174
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected RealProxy(Type classToProxy, IntPtr stub, object stubData)
		{
			if (!classToProxy.IsMarshalByRef && !classToProxy.IsInterface)
			{
				throw new ArgumentException(Environment.GetResourceString("Remoting_Proxy_ProxyTypeIsNotMBR"));
			}
			if ((IntPtr)0 == stub)
			{
				stub = RealProxy._defaultStub;
				stubData = RealProxy._defaultStubData;
			}
			this._tp = null;
			if (stubData == null)
			{
				throw new ArgumentNullException("stubdata");
			}
			this._tp = RemotingServices.CreateTransparentProxy(this, classToProxy, stub, stubData);
			RemotingProxy remotingProxy = this as RemotingProxy;
			if (remotingProxy != null)
			{
				this._flags |= RealProxyFlags.RemotingProxy;
			}
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x000E11FF File Offset: 0x000E01FF
		internal bool IsRemotingProxy()
		{
			return (this._flags & RealProxyFlags.RemotingProxy) == RealProxyFlags.RemotingProxy;
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060041F2 RID: 16882 RVA: 0x000E120C File Offset: 0x000E020C
		// (set) Token: 0x060041F3 RID: 16883 RVA: 0x000E1219 File Offset: 0x000E0219
		internal bool Initialized
		{
			get
			{
				return (this._flags & RealProxyFlags.Initialized) == RealProxyFlags.Initialized;
			}
			set
			{
				if (value)
				{
					this._flags |= RealProxyFlags.Initialized;
					return;
				}
				this._flags &= ~RealProxyFlags.Initialized;
			}
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x000E123C File Offset: 0x000E023C
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IConstructionReturnMessage InitializeServerObject(IConstructionCallMessage ctorMsg)
		{
			IConstructionReturnMessage constructionReturnMessage = null;
			if (this._serverObject == null)
			{
				Type proxiedType = this.GetProxiedType();
				if (ctorMsg != null && ctorMsg.ActivationType != proxiedType)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Proxy_BadTypeForActivation"), new object[] { proxiedType.FullName, ctorMsg.ActivationType }));
				}
				this._serverObject = RemotingServices.AllocateUninitializedObject(proxiedType);
				this.SetContextForDefaultStub();
				MarshalByRefObject marshalByRefObject = (MarshalByRefObject)this.GetTransparentProxy();
				IMethodReturnMessage methodReturnMessage = null;
				Exception ex = null;
				if (ctorMsg != null)
				{
					methodReturnMessage = RemotingServices.ExecuteMessage(marshalByRefObject, ctorMsg);
					ex = methodReturnMessage.Exception;
				}
				else
				{
					try
					{
						RemotingServices.CallDefaultCtor(marshalByRefObject);
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
				}
				if (ex == null)
				{
					object[] array = ((methodReturnMessage == null) ? null : methodReturnMessage.OutArgs);
					int num = ((array == null) ? 0 : array.Length);
					LogicalCallContext logicalCallContext = ((methodReturnMessage == null) ? null : methodReturnMessage.LogicalCallContext);
					constructionReturnMessage = new ConstructorReturnMessage(marshalByRefObject, array, num, logicalCallContext, ctorMsg);
					this.SetupIdentity();
					if (this.IsRemotingProxy())
					{
						((RemotingProxy)this).Initialized = true;
					}
				}
				else
				{
					constructionReturnMessage = new ConstructorReturnMessage(ex, ctorMsg);
				}
			}
			return constructionReturnMessage;
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x000E135C File Offset: 0x000E035C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected MarshalByRefObject GetUnwrappedServer()
		{
			return this.UnwrappedServerObject;
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x000E1364 File Offset: 0x000E0364
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected MarshalByRefObject DetachServer()
		{
			object transparentProxy = this.GetTransparentProxy();
			if (transparentProxy != null)
			{
				RemotingServices.ResetInterfaceCache(transparentProxy);
			}
			MarshalByRefObject serverObject = this._serverObject;
			this._serverObject = null;
			serverObject.__ResetServerIdentity();
			return serverObject;
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x000E1398 File Offset: 0x000E0398
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected void AttachServer(MarshalByRefObject s)
		{
			object transparentProxy = this.GetTransparentProxy();
			if (transparentProxy != null)
			{
				RemotingServices.ResetInterfaceCache(transparentProxy);
			}
			this.AttachServerHelper(s);
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x000E13BC File Offset: 0x000E03BC
		private void SetupIdentity()
		{
			if (this._identity == null)
			{
				this._identity = IdentityHolder.FindOrCreateServerIdentity(this._serverObject, null, 0);
				((Identity)this._identity).RaceSetTransparentProxy(this.GetTransparentProxy());
			}
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x000E13F0 File Offset: 0x000E03F0
		private void SetContextForDefaultStub()
		{
			if (this.GetStub() == RealProxy._defaultStub)
			{
				object stubData = RealProxy.GetStubData(this);
				if (stubData is IntPtr && ((IntPtr)stubData).Equals(RealProxy._defaultStubValue))
				{
					RealProxy.SetStubData(this, Thread.CurrentContext.InternalContextID);
				}
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x000E1454 File Offset: 0x000E0454
		internal bool DoContextsMatch()
		{
			bool flag = false;
			if (this.GetStub() == RealProxy._defaultStub)
			{
				object stubData = RealProxy.GetStubData(this);
				if (stubData is IntPtr && ((IntPtr)stubData).Equals(Thread.CurrentContext.InternalContextID))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x000E14AC File Offset: 0x000E04AC
		internal void AttachServerHelper(MarshalByRefObject s)
		{
			if (s == null || this._serverObject != null)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentNull_Generic"), "s");
			}
			this._serverObject = s;
			this.SetupIdentity();
		}

		// Token: 0x060041FC RID: 16892
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr GetStub();

		// Token: 0x060041FD RID: 16893
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStubData(RealProxy rp, object stubData);

		// Token: 0x060041FE RID: 16894 RVA: 0x000E14DB File Offset: 0x000E04DB
		internal void SetSrvInfo(GCHandle srvIdentity, int domainID)
		{
			this._srvIdentity = srvIdentity;
			this._domainID = domainID;
		}

		// Token: 0x060041FF RID: 16895
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetStubData(RealProxy rp);

		// Token: 0x06004200 RID: 16896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetDefaultStub();

		// Token: 0x06004201 RID: 16897
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Type GetProxiedType();

		// Token: 0x06004202 RID: 16898
		public abstract IMessage Invoke(IMessage msg);

		// Token: 0x06004203 RID: 16899 RVA: 0x000E14EB File Offset: 0x000E04EB
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			if (this._identity == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_NoIdentityEntry"));
			}
			return new ObjRef((MarshalByRefObject)this.GetTransparentProxy(), requestedType);
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x000E1518 File Offset: 0x000E0518
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			object transparentProxy = this.GetTransparentProxy();
			RemotingServices.GetObjectData(transparentProxy, info, context);
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x000E1534 File Offset: 0x000E0534
		private static void HandleReturnMessage(IMessage reqMsg, IMessage retMsg)
		{
			IMethodReturnMessage methodReturnMessage = retMsg as IMethodReturnMessage;
			if (retMsg == null || methodReturnMessage == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
			}
			Exception exception = methodReturnMessage.Exception;
			if (exception != null)
			{
				throw exception.PrepForRemoting();
			}
			if (!(retMsg is StackBasedReturnMessage))
			{
				if (reqMsg is Message)
				{
					RealProxy.PropagateOutParameters(reqMsg, methodReturnMessage.Args, methodReturnMessage.ReturnValue);
					return;
				}
				if (reqMsg is ConstructorCallMessage)
				{
					RealProxy.PropagateOutParameters(reqMsg, methodReturnMessage.Args, null);
				}
			}
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x000E15A8 File Offset: 0x000E05A8
		internal static void PropagateOutParameters(IMessage msg, object[] outArgs, object returnValue)
		{
			Message message = msg as Message;
			if (message == null)
			{
				ConstructorCallMessage constructorCallMessage = msg as ConstructorCallMessage;
				if (constructorCallMessage != null)
				{
					message = constructorCallMessage.GetMessage();
				}
			}
			if (message == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Remoting_Proxy_ExpectedOriginalMessage"));
			}
			MethodBase methodBase = message.GetMethodBase();
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
			if (outArgs != null && outArgs.Length > 0)
			{
				object[] args = message.Args;
				ParameterInfo[] parameters = reflectionCachedData.Parameters;
				foreach (int num in reflectionCachedData.MarshalRequestArgMap)
				{
					ParameterInfo parameterInfo = parameters[num];
					if (parameterInfo.IsIn && parameterInfo.ParameterType.IsByRef && !parameterInfo.IsOut)
					{
						outArgs[num] = args[num];
					}
				}
				if (reflectionCachedData.NonRefOutArgMap.Length > 0)
				{
					foreach (int num2 in reflectionCachedData.NonRefOutArgMap)
					{
						Array array = args[num2] as Array;
						if (array != null)
						{
							Array.Copy((Array)outArgs[num2], array, array.Length);
						}
					}
				}
				int[] outRefArgMap = reflectionCachedData.OutRefArgMap;
				if (outRefArgMap.Length > 0)
				{
					foreach (int num3 in outRefArgMap)
					{
						RealProxy.ValidateReturnArg(outArgs[num3], parameters[num3].ParameterType);
					}
				}
			}
			int callType = message.GetCallType();
			if ((callType & 15) != 1)
			{
				Type returnType = reflectionCachedData.ReturnType;
				if (returnType != null)
				{
					RealProxy.ValidateReturnArg(returnValue, returnType);
				}
			}
			message.PropagateOutParameters(outArgs, returnValue);
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x000E1724 File Offset: 0x000E0724
		private static void ValidateReturnArg(object arg, Type paramType)
		{
			if (paramType.IsByRef)
			{
				paramType = paramType.GetElementType();
			}
			if (paramType.IsValueType)
			{
				if (arg == null)
				{
					if (!paramType.IsGenericType || paramType.GetGenericTypeDefinition() != typeof(Nullable<>))
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_ReturnValueTypeCannotBeNull"));
					}
				}
				else if (!paramType.IsInstanceOfType(arg))
				{
					throw new InvalidCastException(Environment.GetResourceString("Remoting_Proxy_BadReturnType"));
				}
			}
			else if (arg != null && !paramType.IsInstanceOfType(arg))
			{
				throw new InvalidCastException(Environment.GetResourceString("Remoting_Proxy_BadReturnType"));
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x000E17AC File Offset: 0x000E07AC
		internal static IMessage EndInvokeHelper(Message reqMsg, bool bProxyCase)
		{
			AsyncResult asyncResult = reqMsg.GetAsyncResult() as AsyncResult;
			IMessage message = null;
			if (asyncResult == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadAsyncResult"));
			}
			if (asyncResult.AsyncDelegate != reqMsg.GetThisPtr())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MismatchedAsyncResult"));
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne(int.MaxValue, Thread.CurrentContext.IsThreadPoolAware);
			}
			lock (asyncResult)
			{
				if (asyncResult.EndInvokeCalled)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EndInvokeCalledMultiple"));
				}
				asyncResult.EndInvokeCalled = true;
				IMethodReturnMessage methodReturnMessage = (IMethodReturnMessage)asyncResult.GetReplyMessage();
				if (!bProxyCase)
				{
					Exception exception = methodReturnMessage.Exception;
					if (exception != null)
					{
						throw exception.PrepForRemoting();
					}
					reqMsg.PropagateOutParameters(methodReturnMessage.Args, methodReturnMessage.ReturnValue);
				}
				else
				{
					message = methodReturnMessage;
				}
				CallContext.GetLogicalCallContext().Merge(methodReturnMessage.LogicalCallContext);
			}
			return message;
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x000E18A4 File Offset: 0x000E08A4
		public virtual IntPtr GetCOMIUnknown(bool fIsMarshalled)
		{
			return MarshalByRefObject.GetComIUnknown((MarshalByRefObject)this.GetTransparentProxy());
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x000E18B6 File Offset: 0x000E08B6
		public virtual void SetCOMIUnknown(IntPtr i)
		{
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x000E18B8 File Offset: 0x000E08B8
		public virtual IntPtr SupportsInterface(ref Guid iid)
		{
			return IntPtr.Zero;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x000E18BF File Offset: 0x000E08BF
		public virtual object GetTransparentProxy()
		{
			return this._tp;
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x000E18C7 File Offset: 0x000E08C7
		internal MarshalByRefObject UnwrappedServerObject
		{
			get
			{
				return this._serverObject;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x000E18CF File Offset: 0x000E08CF
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x000E18DC File Offset: 0x000E08DC
		internal virtual Identity IdentityObject
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (Identity)this._identity;
			}
			set
			{
				this._identity = value;
			}
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x000E18E8 File Offset: 0x000E08E8
		private void PrivateInvoke(ref MessageData msgData, int type)
		{
			IMessage message = null;
			IMessage message2 = null;
			int num = -1;
			RemotingProxy remotingProxy = null;
			if (1 == type)
			{
				Message message3 = new Message();
				message3.InitFields(msgData);
				message = message3;
				num = message3.GetCallType();
			}
			else if (2 == type)
			{
				num = 0;
				remotingProxy = this as RemotingProxy;
				bool flag = false;
				ConstructorCallMessage constructorCallMessage;
				if (!this.IsRemotingProxy())
				{
					constructorCallMessage = new ConstructorCallMessage(null, null, null, this.GetProxiedType());
				}
				else
				{
					constructorCallMessage = remotingProxy.ConstructorMessage;
					Identity identityObject = remotingProxy.IdentityObject;
					if (identityObject != null)
					{
						flag = identityObject.IsWellKnown();
					}
				}
				if (constructorCallMessage == null || flag)
				{
					constructorCallMessage = new ConstructorCallMessage(null, null, null, this.GetProxiedType());
					constructorCallMessage.SetFrame(msgData);
					message = constructorCallMessage;
					if (flag)
					{
						remotingProxy.ConstructorMessage = null;
						if (constructorCallMessage.ArgCount != 0)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Activation_WellKnownCTOR"));
						}
					}
					message2 = new ConstructorReturnMessage((MarshalByRefObject)this.GetTransparentProxy(), null, 0, null, constructorCallMessage);
				}
				else
				{
					constructorCallMessage.SetFrame(msgData);
					message = constructorCallMessage;
				}
			}
			ChannelServices.IncrementRemoteCalls();
			if (!this.IsRemotingProxy() && (num & 2) == 2)
			{
				Message message4 = message as Message;
				message2 = RealProxy.EndInvokeHelper(message4, true);
			}
			if (message2 == null)
			{
				Thread currentThread = Thread.CurrentThread;
				LogicalCallContext logicalCallContext = currentThread.GetLogicalCallContext();
				this.SetCallContextInMessage(message, num, logicalCallContext);
				logicalCallContext.PropagateOutgoingHeadersToMessage(message);
				message2 = this.Invoke(message);
				this.ReturnCallContextToThread(currentThread, message2, num, logicalCallContext);
				CallContext.GetLogicalCallContext().PropagateIncomingHeadersToCallContext(message2);
			}
			if (!this.IsRemotingProxy() && (num & 1) == 1)
			{
				Message message5 = message as Message;
				AsyncResult asyncResult = new AsyncResult(message5);
				asyncResult.SyncProcessMessage(message2);
				message2 = new ReturnMessage(asyncResult, null, 0, null, message5);
			}
			RealProxy.HandleReturnMessage(message, message2);
			if (2 == type)
			{
				IConstructionReturnMessage constructionReturnMessage = message2 as IConstructionReturnMessage;
				if (constructionReturnMessage == null)
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_Proxy_BadReturnTypeForActivation"));
				}
				ConstructorReturnMessage constructorReturnMessage = constructionReturnMessage as ConstructorReturnMessage;
				MarshalByRefObject marshalByRefObject;
				if (constructorReturnMessage != null)
				{
					marshalByRefObject = (MarshalByRefObject)constructorReturnMessage.GetObject();
					if (marshalByRefObject == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Activation_NullReturnValue"));
					}
				}
				else
				{
					marshalByRefObject = (MarshalByRefObject)RemotingServices.InternalUnmarshal((ObjRef)constructionReturnMessage.ReturnValue, this.GetTransparentProxy(), true);
					if (marshalByRefObject == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Activation_NullFromInternalUnmarshal"));
					}
				}
				if (marshalByRefObject != (MarshalByRefObject)this.GetTransparentProxy())
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_Activation_InconsistentState"));
				}
				if (this.IsRemotingProxy())
				{
					remotingProxy.ConstructorMessage = null;
				}
			}
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x000E1B4C File Offset: 0x000E0B4C
		private void SetCallContextInMessage(IMessage reqMsg, int msgFlags, LogicalCallContext cctx)
		{
			Message message = reqMsg as Message;
			if (msgFlags != 0)
			{
				return;
			}
			if (message != null)
			{
				message.SetLogicalCallContext(cctx);
				return;
			}
			((ConstructorCallMessage)reqMsg).SetLogicalCallContext(cctx);
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x000E1B80 File Offset: 0x000E0B80
		private void ReturnCallContextToThread(Thread currentThread, IMessage retMsg, int msgFlags, LogicalCallContext currCtx)
		{
			if (msgFlags == 0)
			{
				if (retMsg == null)
				{
					return;
				}
				IMethodReturnMessage methodReturnMessage = retMsg as IMethodReturnMessage;
				if (methodReturnMessage == null)
				{
					return;
				}
				LogicalCallContext logicalCallContext = methodReturnMessage.LogicalCallContext;
				if (logicalCallContext == null)
				{
					currentThread.SetLogicalCallContext(currCtx);
					return;
				}
				if (!(methodReturnMessage is StackBasedReturnMessage))
				{
					LogicalCallContext logicalCallContext2 = currentThread.SetLogicalCallContext(logicalCallContext);
					if (logicalCallContext2 != logicalCallContext)
					{
						IPrincipal principal = logicalCallContext2.Principal;
						if (principal != null)
						{
							logicalCallContext.Principal = principal;
						}
					}
				}
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x000E1BD8 File Offset: 0x000E0BD8
		internal virtual void Wrap()
		{
			ServerIdentity serverIdentity = this._identity as ServerIdentity;
			if (serverIdentity != null && this is RemotingProxy)
			{
				RealProxy.SetStubData(this, serverIdentity.ServerContext.InternalContextID);
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x000E1C12 File Offset: 0x000E0C12
		protected RealProxy()
		{
		}

		// Token: 0x040020E5 RID: 8421
		private object _tp;

		// Token: 0x040020E6 RID: 8422
		private object _identity;

		// Token: 0x040020E7 RID: 8423
		private MarshalByRefObject _serverObject;

		// Token: 0x040020E8 RID: 8424
		private RealProxyFlags _flags;

		// Token: 0x040020E9 RID: 8425
		internal GCHandle _srvIdentity;

		// Token: 0x040020EA RID: 8426
		internal int _optFlags;

		// Token: 0x040020EB RID: 8427
		internal int _domainID;

		// Token: 0x040020EC RID: 8428
		private static IntPtr _defaultStub = RealProxy.GetDefaultStub();

		// Token: 0x040020ED RID: 8429
		private static IntPtr _defaultStubValue = new IntPtr(-1);

		// Token: 0x040020EE RID: 8430
		private static object _defaultStubData = RealProxy._defaultStubValue;
	}
}
