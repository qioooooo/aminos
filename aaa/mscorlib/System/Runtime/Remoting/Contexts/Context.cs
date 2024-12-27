using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006AF RID: 1711
	[ComVisible(true)]
	public class Context
	{
		// Token: 0x06003E23 RID: 15907 RVA: 0x000D603C File Offset: 0x000D503C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public Context()
			: this(0)
		{
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x000D6048 File Offset: 0x000D5048
		private Context(int flags)
		{
			this._ctxFlags = flags;
			if ((this._ctxFlags & 1) != 0)
			{
				this._ctxID = 0;
			}
			else
			{
				this._ctxID = Interlocked.Increment(ref Context._ctxIDCounter);
			}
			DomainSpecificRemotingData remotingData = Thread.GetDomain().RemotingData;
			if (remotingData != null)
			{
				IContextProperty[] appDomainContextProperties = remotingData.AppDomainContextProperties;
				if (appDomainContextProperties != null)
				{
					for (int i = 0; i < appDomainContextProperties.Length; i++)
					{
						this.SetProperty(appDomainContextProperties[i]);
					}
				}
			}
			if ((this._ctxFlags & 1) != 0)
			{
				this.Freeze();
			}
			this.SetupInternalContext((this._ctxFlags & 1) == 1);
		}

		// Token: 0x06003E25 RID: 15909
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetupInternalContext(bool bDefault);

		// Token: 0x06003E26 RID: 15910 RVA: 0x000D60D8 File Offset: 0x000D50D8
		~Context()
		{
			if (this._internalContext != IntPtr.Zero && (this._ctxFlags & 1) == 0)
			{
				this.CleanupInternalContext();
			}
		}

		// Token: 0x06003E27 RID: 15911
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CleanupInternalContext();

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x000D6120 File Offset: 0x000D5120
		public virtual int ContextID
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this._ctxID;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x000D6128 File Offset: 0x000D5128
		internal virtual IntPtr InternalContextID
		{
			get
			{
				return this._internalContext;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x000D6130 File Offset: 0x000D5130
		internal virtual AppDomain AppDomain
		{
			get
			{
				return this._appDomain;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x000D6138 File Offset: 0x000D5138
		internal bool IsDefaultContext
		{
			get
			{
				return this._ctxID == 0;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x000D6143 File Offset: 0x000D5143
		public static Context DefaultContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Thread.GetDomain().GetDefaultContext();
			}
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x000D614F File Offset: 0x000D514F
		internal static Context CreateDefaultContext()
		{
			return new Context(1);
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x000D6158 File Offset: 0x000D5158
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IContextProperty GetProperty(string name)
		{
			if (this._ctxProps == null || name == null)
			{
				return null;
			}
			IContextProperty contextProperty = null;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				if (this._ctxProps[i].Name.Equals(name))
				{
					contextProperty = this._ctxProps[i];
					break;
				}
			}
			return contextProperty;
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x000D61A8 File Offset: 0x000D51A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual void SetProperty(IContextProperty prop)
		{
			if (prop == null || prop.Name == null)
			{
				throw new ArgumentNullException((prop == null) ? "prop" : "property name");
			}
			if ((this._ctxFlags & 2) != 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AddContextFrozen"));
			}
			lock (this)
			{
				Context.CheckPropertyNameClash(prop.Name, this._ctxProps, this._numCtxProps);
				if (this._ctxProps == null || this._numCtxProps == this._ctxProps.Length)
				{
					this._ctxProps = Context.GrowPropertiesArray(this._ctxProps);
				}
				this._ctxProps[this._numCtxProps++] = prop;
			}
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x000D626C File Offset: 0x000D526C
		internal virtual void InternalFreeze()
		{
			this._ctxFlags |= 2;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				this._ctxProps[i].Freeze(this);
			}
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x000D62A8 File Offset: 0x000D52A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual void Freeze()
		{
			lock (this)
			{
				if ((this._ctxFlags & 2) != 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ContextAlreadyFrozen"));
				}
				this.InternalFreeze();
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x000D62F8 File Offset: 0x000D52F8
		internal virtual void SetThreadPoolAware()
		{
			this._ctxFlags |= 4;
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x000D6308 File Offset: 0x000D5308
		internal virtual bool IsThreadPoolAware
		{
			get
			{
				return (this._ctxFlags & 4) == 4;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x000D6318 File Offset: 0x000D5318
		public virtual IContextProperty[] ContextProperties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				if (this._ctxProps == null)
				{
					return null;
				}
				IContextProperty[] array2;
				lock (this)
				{
					IContextProperty[] array = new IContextProperty[this._numCtxProps];
					Array.Copy(this._ctxProps, array, this._numCtxProps);
					array2 = array;
				}
				return array2;
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x000D6374 File Offset: 0x000D5374
		internal static void CheckPropertyNameClash(string name, IContextProperty[] props, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (props[i].Name.Equals(name))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DuplicatePropertyName"));
				}
			}
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x000D63B0 File Offset: 0x000D53B0
		internal static IContextProperty[] GrowPropertiesArray(IContextProperty[] props)
		{
			int num = ((props != null) ? props.Length : 0) + 8;
			IContextProperty[] array = new IContextProperty[num];
			if (props != null)
			{
				Array.Copy(props, array, props.Length);
			}
			return array;
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x000D63E0 File Offset: 0x000D53E0
		internal virtual IMessageSink GetServerContextChain()
		{
			if (this._serverContextChain == null)
			{
				IMessageSink messageSink = ServerContextTerminatorSink.MessageSink;
				int numCtxProps = this._numCtxProps;
				while (numCtxProps-- > 0)
				{
					object obj = this._ctxProps[numCtxProps];
					IContributeServerContextSink contributeServerContextSink = obj as IContributeServerContextSink;
					if (contributeServerContextSink != null)
					{
						messageSink = contributeServerContextSink.GetServerContextSink(messageSink);
						if (messageSink == null)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
						}
					}
				}
				lock (this)
				{
					if (this._serverContextChain == null)
					{
						this._serverContextChain = messageSink;
					}
				}
			}
			return this._serverContextChain;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x000D6474 File Offset: 0x000D5474
		internal virtual IMessageSink GetClientContextChain()
		{
			if (this._clientContextChain == null)
			{
				IMessageSink messageSink = ClientContextTerminatorSink.MessageSink;
				for (int i = 0; i < this._numCtxProps; i++)
				{
					object obj = this._ctxProps[i];
					IContributeClientContextSink contributeClientContextSink = obj as IContributeClientContextSink;
					if (contributeClientContextSink != null)
					{
						messageSink = contributeClientContextSink.GetClientContextSink(messageSink);
						if (messageSink == null)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
						}
					}
				}
				lock (this)
				{
					if (this._clientContextChain == null)
					{
						this._clientContextChain = messageSink;
					}
				}
			}
			return this._clientContextChain;
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x000D6508 File Offset: 0x000D5508
		internal virtual IMessageSink CreateServerObjectChain(MarshalByRefObject serverObj)
		{
			IMessageSink messageSink = new ServerObjectTerminatorSink(serverObj);
			int numCtxProps = this._numCtxProps;
			while (numCtxProps-- > 0)
			{
				object obj = this._ctxProps[numCtxProps];
				IContributeObjectSink contributeObjectSink = obj as IContributeObjectSink;
				if (contributeObjectSink != null)
				{
					messageSink = contributeObjectSink.GetObjectSink(serverObj, messageSink);
					if (messageSink == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
					}
				}
			}
			return messageSink;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x000D6560 File Offset: 0x000D5560
		internal virtual IMessageSink CreateEnvoyChain(MarshalByRefObject objectOrProxy)
		{
			IMessageSink messageSink = EnvoyTerminatorSink.MessageSink;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				object obj = this._ctxProps[i];
				IContributeEnvoySink contributeEnvoySink = obj as IContributeEnvoySink;
				if (contributeEnvoySink != null)
				{
					messageSink = contributeEnvoySink.GetEnvoySink(objectOrProxy, messageSink);
					if (messageSink == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
					}
				}
			}
			return messageSink;
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x000D65BC File Offset: 0x000D55BC
		internal IMessage NotifyActivatorProperties(IMessage msg, bool bServerSide)
		{
			IMessage message = null;
			try
			{
				int numCtxProps = this._numCtxProps;
				while (numCtxProps-- != 0)
				{
					object obj = this._ctxProps[numCtxProps];
					IContextPropertyActivator contextPropertyActivator = obj as IContextPropertyActivator;
					if (contextPropertyActivator != null)
					{
						IConstructionCallMessage constructionCallMessage = msg as IConstructionCallMessage;
						if (constructionCallMessage != null)
						{
							if (!bServerSide)
							{
								contextPropertyActivator.CollectFromClientContext(constructionCallMessage);
							}
							else
							{
								contextPropertyActivator.DeliverClientContextToServerContext(constructionCallMessage);
							}
						}
						else if (bServerSide)
						{
							contextPropertyActivator.CollectFromServerContext((IConstructionReturnMessage)msg);
						}
						else
						{
							contextPropertyActivator.DeliverServerContextToClientContext((IConstructionReturnMessage)msg);
						}
					}
				}
			}
			catch (Exception ex)
			{
				IMethodCallMessage methodCallMessage;
				if (msg is IConstructionCallMessage)
				{
					methodCallMessage = (IMethodCallMessage)msg;
				}
				else
				{
					methodCallMessage = new ErrorMessage();
				}
				message = new ReturnMessage(ex, methodCallMessage);
				if (msg != null)
				{
					((ReturnMessage)message).SetLogicalCallContext((LogicalCallContext)msg.Properties[Message.CallContextKey]);
				}
			}
			return message;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x000D6694 File Offset: 0x000D5694
		public override string ToString()
		{
			return "ContextID: " + this._ctxID;
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x000D66AC File Offset: 0x000D56AC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public void DoCallBack(CrossContextDelegate deleg)
		{
			if (deleg == null)
			{
				throw new ArgumentNullException("deleg");
			}
			if ((this._ctxFlags & 2) == 0)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_ContextNotFrozenForCallBack"));
			}
			Context currentContext = Thread.CurrentContext;
			if (currentContext == this)
			{
				deleg();
				return;
			}
			currentContext.DoCallBackGeneric(this.InternalContextID, deleg);
			GC.KeepAlive(this);
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x000D6708 File Offset: 0x000D5708
		internal static void DoCallBackFromEE(IntPtr targetCtxID, IntPtr privateData, int targetDomainID)
		{
			if (targetDomainID == 0)
			{
				CallBackHelper callBackHelper = new CallBackHelper(privateData, true, targetDomainID);
				CrossContextDelegate crossContextDelegate = new CrossContextDelegate(callBackHelper.Func);
				Thread.CurrentContext.DoCallBackGeneric(targetCtxID, crossContextDelegate);
				return;
			}
			TransitionCall transitionCall = new TransitionCall(targetCtxID, privateData, targetDomainID);
			Message.PropagateCallContextFromThreadToMessage(transitionCall);
			IMessage message = Thread.CurrentContext.GetClientContextChain().SyncProcessMessage(transitionCall);
			Message.PropagateCallContextFromMessageToThread(message);
			IMethodReturnMessage methodReturnMessage = message as IMethodReturnMessage;
			if (methodReturnMessage != null && methodReturnMessage.Exception != null)
			{
				throw methodReturnMessage.Exception;
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x000D6780 File Offset: 0x000D5780
		internal void DoCallBackGeneric(IntPtr targetCtxID, CrossContextDelegate deleg)
		{
			TransitionCall transitionCall = new TransitionCall(targetCtxID, deleg);
			Message.PropagateCallContextFromThreadToMessage(transitionCall);
			IMessage message = this.GetClientContextChain().SyncProcessMessage(transitionCall);
			if (message != null)
			{
				Message.PropagateCallContextFromMessageToThread(message);
			}
			IMethodReturnMessage methodReturnMessage = message as IMethodReturnMessage;
			if (methodReturnMessage != null && methodReturnMessage.Exception != null)
			{
				throw methodReturnMessage.Exception;
			}
		}

		// Token: 0x06003E40 RID: 15936
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ExecuteCallBackInEE(IntPtr privateData);

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x000D67CC File Offset: 0x000D57CC
		private LocalDataStore MyLocalStore
		{
			get
			{
				if (this._localDataStore == null)
				{
					lock (Context._localDataStoreMgr)
					{
						if (this._localDataStore == null)
						{
							this._localDataStore = Context._localDataStoreMgr.CreateLocalDataStore();
						}
					}
				}
				return this._localDataStore;
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x000D6824 File Offset: 0x000D5824
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return Context._localDataStoreMgr.AllocateDataSlot();
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x000D6830 File Offset: 0x000D5830
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Context._localDataStoreMgr.AllocateNamedDataSlot(name);
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x000D683D File Offset: 0x000D583D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Context._localDataStoreMgr.GetNamedDataSlot(name);
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x000D684A File Offset: 0x000D584A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void FreeNamedDataSlot(string name)
		{
			Context._localDataStoreMgr.FreeNamedDataSlot(name);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x000D6857 File Offset: 0x000D5857
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			Thread.CurrentContext.MyLocalStore.SetData(slot, data);
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x000D686A File Offset: 0x000D586A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static object GetData(LocalDataStoreSlot slot)
		{
			return Thread.CurrentContext.MyLocalStore.GetData(slot);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x000D687C File Offset: 0x000D587C
		private int ReserveSlot()
		{
			if (this._ctxStatics == null)
			{
				this._ctxStatics = new object[8];
				this._ctxStatics[0] = null;
				this._ctxStaticsFreeIndex = 1;
				this._ctxStaticsCurrentBucket = 0;
			}
			if (this._ctxStaticsFreeIndex == 8)
			{
				object[] array = new object[8];
				object[] array2 = this._ctxStatics;
				while (array2[0] != null)
				{
					array2 = (object[])array2[0];
				}
				array2[0] = array;
				this._ctxStaticsFreeIndex = 1;
				this._ctxStaticsCurrentBucket++;
			}
			return this._ctxStaticsFreeIndex++ | (this._ctxStaticsCurrentBucket << 16);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x000D6910 File Offset: 0x000D5910
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool RegisterDynamicProperty(IDynamicProperty prop, ContextBoundObject obj, Context ctx)
		{
			if (prop == null || prop.Name == null || !(prop is IContributeDynamicSink))
			{
				throw new ArgumentNullException("prop");
			}
			if (obj != null && ctx != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NonNullObjAndCtx"));
			}
			bool flag;
			if (obj != null)
			{
				flag = IdentityHolder.AddDynamicProperty(obj, prop);
			}
			else
			{
				flag = Context.AddDynamicProperty(ctx, prop);
			}
			return flag;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x000D696C File Offset: 0x000D596C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool UnregisterDynamicProperty(string name, ContextBoundObject obj, Context ctx)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (obj != null && ctx != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NonNullObjAndCtx"));
			}
			bool flag;
			if (obj != null)
			{
				flag = IdentityHolder.RemoveDynamicProperty(obj, name);
			}
			else
			{
				flag = Context.RemoveDynamicProperty(ctx, name);
			}
			return flag;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x000D69B5 File Offset: 0x000D59B5
		internal static bool AddDynamicProperty(Context ctx, IDynamicProperty prop)
		{
			if (ctx != null)
			{
				return ctx.AddPerContextDynamicProperty(prop);
			}
			return Context.AddGlobalDynamicProperty(prop);
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x000D69C8 File Offset: 0x000D59C8
		private bool AddPerContextDynamicProperty(IDynamicProperty prop)
		{
			if (this._dphCtx == null)
			{
				DynamicPropertyHolder dynamicPropertyHolder = new DynamicPropertyHolder();
				lock (this)
				{
					if (this._dphCtx == null)
					{
						this._dphCtx = dynamicPropertyHolder;
					}
				}
			}
			return this._dphCtx.AddDynamicProperty(prop);
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x000D6A20 File Offset: 0x000D5A20
		private static bool AddGlobalDynamicProperty(IDynamicProperty prop)
		{
			return Context._dphGlobal.AddDynamicProperty(prop);
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x000D6A2D File Offset: 0x000D5A2D
		internal static bool RemoveDynamicProperty(Context ctx, string name)
		{
			if (ctx != null)
			{
				return ctx.RemovePerContextDynamicProperty(name);
			}
			return Context.RemoveGlobalDynamicProperty(name);
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x000D6A40 File Offset: 0x000D5A40
		private bool RemovePerContextDynamicProperty(string name)
		{
			if (this._dphCtx == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Contexts_NoProperty"), new object[] { name }));
			}
			return this._dphCtx.RemoveDynamicProperty(name);
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x000D6A87 File Offset: 0x000D5A87
		private static bool RemoveGlobalDynamicProperty(string name)
		{
			return Context._dphGlobal.RemoveDynamicProperty(name);
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x000D6A94 File Offset: 0x000D5A94
		internal virtual IDynamicProperty[] PerContextDynamicProperties
		{
			get
			{
				if (this._dphCtx == null)
				{
					return null;
				}
				return this._dphCtx.DynamicProperties;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x000D6AAB File Offset: 0x000D5AAB
		internal static ArrayWithSize GlobalDynamicSinks
		{
			get
			{
				return Context._dphGlobal.DynamicSinks;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x000D6AB7 File Offset: 0x000D5AB7
		internal virtual ArrayWithSize DynamicSinks
		{
			get
			{
				if (this._dphCtx == null)
				{
					return null;
				}
				return this._dphCtx.DynamicSinks;
			}
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x000D6AD0 File Offset: 0x000D5AD0
		internal virtual bool NotifyDynamicSinks(IMessage msg, bool bCliSide, bool bStart, bool bAsync, bool bNotifyGlobals)
		{
			bool flag = false;
			if (bNotifyGlobals && Context._dphGlobal.DynamicProperties != null)
			{
				ArrayWithSize globalDynamicSinks = Context.GlobalDynamicSinks;
				if (globalDynamicSinks != null)
				{
					DynamicPropertyHolder.NotifyDynamicSinks(msg, globalDynamicSinks, bCliSide, bStart, bAsync);
					flag = true;
				}
			}
			ArrayWithSize dynamicSinks = this.DynamicSinks;
			if (dynamicSinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(msg, dynamicSinks, bCliSide, bStart, bAsync);
				flag = true;
			}
			return flag;
		}

		// Token: 0x04001F7D RID: 8061
		internal const int CTX_DEFAULT_CONTEXT = 1;

		// Token: 0x04001F7E RID: 8062
		internal const int CTX_FROZEN = 2;

		// Token: 0x04001F7F RID: 8063
		internal const int CTX_THREADPOOL_AWARE = 4;

		// Token: 0x04001F80 RID: 8064
		private const int GROW_BY = 8;

		// Token: 0x04001F81 RID: 8065
		private const int STATICS_BUCKET_SIZE = 8;

		// Token: 0x04001F82 RID: 8066
		private IContextProperty[] _ctxProps;

		// Token: 0x04001F83 RID: 8067
		private DynamicPropertyHolder _dphCtx;

		// Token: 0x04001F84 RID: 8068
		private LocalDataStore _localDataStore;

		// Token: 0x04001F85 RID: 8069
		private IMessageSink _serverContextChain;

		// Token: 0x04001F86 RID: 8070
		private IMessageSink _clientContextChain;

		// Token: 0x04001F87 RID: 8071
		private AppDomain _appDomain;

		// Token: 0x04001F88 RID: 8072
		private object[] _ctxStatics;

		// Token: 0x04001F89 RID: 8073
		private IntPtr _internalContext;

		// Token: 0x04001F8A RID: 8074
		private int _ctxID;

		// Token: 0x04001F8B RID: 8075
		private int _ctxFlags;

		// Token: 0x04001F8C RID: 8076
		private int _numCtxProps;

		// Token: 0x04001F8D RID: 8077
		private int _ctxStaticsCurrentBucket;

		// Token: 0x04001F8E RID: 8078
		private int _ctxStaticsFreeIndex;

		// Token: 0x04001F8F RID: 8079
		private static DynamicPropertyHolder _dphGlobal = new DynamicPropertyHolder();

		// Token: 0x04001F90 RID: 8080
		private static LocalDataStoreMgr _localDataStoreMgr = new LocalDataStoreMgr();

		// Token: 0x04001F91 RID: 8081
		private static int _ctxIDCounter = 0;
	}
}
