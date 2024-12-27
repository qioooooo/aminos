using System;
using System.Collections;
using System.Diagnostics;
using System.EnterpriseServices.Thunk;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Security.Permissions;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x0200002B RID: 43
	internal class ServicedComponentProxy : RealProxy, IProxyInvoke, IManagedPoolAction
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00002814 File Offset: 0x00001814
		static ServicedComponentProxy()
		{
			try
			{
				BooleanSwitch booleanSwitch = new BooleanSwitch("DisableAsyncFinalization");
				ServicedComponentProxy._asyncFinalizeEnabled = !booleanSwitch.Enabled;
			}
			catch
			{
				ServicedComponentProxy._asyncFinalizeEnabled = true;
			}
			if (ServicedComponentProxy._asyncFinalizeEnabled)
			{
				ServicedComponentProxy._ctxQueue = new Queue();
				ServicedComponentProxy._gitQueue = new Queue();
				ServicedComponentProxy._Wakeup = new AutoResetEvent(false);
				ServicedComponentProxy._exitCleanupThread = new ManualResetEvent(false);
				ServicedComponentProxy._cleanupThread = new Thread(new ThreadStart(ServicedComponentProxy.QueueCleaner));
				ServicedComponentProxy._cleanupThread.IsBackground = true;
				ServicedComponentProxy._cleanupThread.Start();
				AppDomain.CurrentDomain.DomainUnload += ServicedComponentProxy.ShutdownDomain;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002A48 File Offset: 0x00001A48
		private static void ShutdownDomain(object sender, EventArgs e)
		{
			ServicedComponentProxy._exitCleanupThread.Set();
			ServicedComponentProxy._Wakeup.Set();
			while (!ServicedComponentProxy.CleanupQueues(true))
			{
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002A68 File Offset: 0x00001A68
		private ServicedComponentProxy()
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002A70 File Offset: 0x00001A70
		internal ServicedComponentProxy(Type serverType, bool fIsJitActivated, bool fIsPooled, bool fAreMethodsSecure, bool fCreateRealServer)
			: base(serverType, ServicedComponentProxy._stub, -1)
		{
			this._gitCookie = 0;
			this._fIsObjectPooled = fIsPooled;
			this._fIsJitActivated = fIsJitActivated;
			this._fDeliverADC = this._fIsObjectPooled || this._fIsJitActivated;
			this._fIsActive = !this._fDeliverADC;
			this._tabled = false;
			this._fUseIntfDispatch = fAreMethodsSecure;
			this._context = ServicedComponentProxy.NegativeOne;
			this._token = ServicedComponentProxy.NegativeOne;
			this._tracker = null;
			this._callback = new Callback();
			this._pPoolUnk = IntPtr.Zero;
			if (Util.ExtendedLifetime)
			{
				this._scstub = new ServicedComponentStub(this);
			}
			if (fCreateRealServer)
			{
				try
				{
					this.ConstructServer();
				}
				catch
				{
					this.ReleaseContext();
					if (!Util.ExtendedLifetime)
					{
						this.ReleaseGitCookie();
					}
					this._fIsServerActivated = false;
					GC.SuppressFinalize(this);
					throw;
				}
				this.SendCreationEvents();
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002B64 File Offset: 0x00001B64
		private static void QueueCleaner()
		{
			while (!ServicedComponentProxy._exitCleanupThread.WaitOne(0, false))
			{
				ServicedComponentProxy.CleanupQueues(true);
				if (ServicedComponentProxy._gitQueue.Count == 0 && ServicedComponentProxy._ctxQueue.Count == 0)
				{
					ServicedComponentProxy._Wakeup.WaitOne(2500, false);
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002BB4 File Offset: 0x00001BB4
		internal static bool CleanupQueues(bool bGit)
		{
			bool flag = true;
			bool flag2 = true;
			if (!ServicedComponentProxy._asyncFinalizeEnabled)
			{
				return true;
			}
			if (bGit)
			{
				if (ServicedComponentProxy._gitQueue.Count > 0)
				{
					bool flag3 = false;
					int num = 0;
					lock (ServicedComponentProxy._gitQueue)
					{
						if (ServicedComponentProxy._gitQueue.Count > 0)
						{
							num = (int)ServicedComponentProxy._gitQueue.Dequeue();
							flag3 = true;
							flag = ServicedComponentProxy._gitQueue.Count <= 0;
						}
					}
					if (flag3)
					{
						Proxy.RevokeObject(num);
					}
				}
			}
			else if (ServicedComponentProxy._gitQueue.Count > 0)
			{
				lock (ServicedComponentProxy._gitQueue)
				{
					if (ServicedComponentProxy._gitQueue.Count > 0 && ServicedComponentProxy._QueuedItemsCount < 25)
					{
						try
						{
							ThreadPool.QueueUserWorkItem(new WaitCallback(ServicedComponentProxy.RevokeAsync), ServicedComponentProxy._gitQueue.Count);
							Interlocked.Increment(ref ServicedComponentProxy._QueuedItemsCount);
						}
						catch
						{
						}
					}
				}
			}
			object obj = null;
			if (ServicedComponentProxy._ctxQueue.Count > 0)
			{
				lock (ServicedComponentProxy._ctxQueue)
				{
					if (ServicedComponentProxy._ctxQueue.Count > 0)
					{
						obj = ServicedComponentProxy._ctxQueue.Dequeue();
						flag2 = ServicedComponentProxy._ctxQueue.Count <= 0;
					}
				}
				if (obj != null)
				{
					if (!Util.ExtendedLifetime)
					{
						Marshal.Release((IntPtr)obj);
					}
					else
					{
						ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)obj;
						try
						{
							servicedComponentProxy.SendDestructionEvents(false);
						}
						catch
						{
						}
						try
						{
							servicedComponentProxy.ReleaseContext();
						}
						catch
						{
						}
					}
				}
			}
			return flag2 && flag;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002D80 File Offset: 0x00001D80
		internal static void RevokeAsync(object o)
		{
			int num = (int)o;
			try
			{
				int num2 = 0;
				while (num2 < num && !ServicedComponentProxy.CleanupQueues(true))
				{
					num2++;
				}
			}
			catch
			{
			}
			Interlocked.Decrement(ref ServicedComponentProxy._QueuedItemsCount);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002DC8 File Offset: 0x00001DC8
		private void AssertValid()
		{
			if (this._context == ServicedComponentProxy.NegativeOne || this._context == IntPtr.Zero)
			{
				throw new ObjectDisposedException("ServicedComponent");
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00002DF9 File Offset: 0x00001DF9
		internal IntPtr HomeToken
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00002E01 File Offset: 0x00001E01
		internal bool IsProxyDeactivated
		{
			get
			{
				return !this._fIsActive;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002E0C File Offset: 0x00001E0C
		internal bool IsJitActivated
		{
			get
			{
				return this._fIsJitActivated;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002E14 File Offset: 0x00001E14
		internal bool IsObjectPooled
		{
			get
			{
				return this._fIsObjectPooled;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00002E1C File Offset: 0x00001E1C
		internal bool AreMethodsSecure
		{
			get
			{
				return this._fUseIntfDispatch;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002E24 File Offset: 0x00001E24
		private void DispatchActivate()
		{
			if (this._fDeliverADC)
			{
				this._fIsServerActivated = true;
				ServicedComponent servicedComponent = (ServicedComponent)this.GetTransparentProxy();
				try
				{
					servicedComponent.Activate();
				}
				catch (Exception ex)
				{
					this.SendDestructionEvents(false);
					this.ReleasePoolUnk();
					this.ReleaseContext();
					this.ReleaseGitCookie();
					this._fIsServerActivated = false;
					try
					{
						EventLog eventLog = new EventLog();
						eventLog.Source = "System.EnterpriseServices";
						string text = Resource.FormatString("Err_ActivationFailed", ex.ToString());
						eventLog.WriteEntry(text, EventLogEntryType.Error);
					}
					catch
					{
					}
					throw new COMException(Resource.FormatString("ServicedComponentException_ActivationFailed"), -2147164123);
				}
				catch
				{
					try
					{
						EventLog eventLog2 = new EventLog();
						eventLog2.Source = "System.EnterpriseServices";
						string text2 = Resource.FormatString("Err_ActivationFailed", Resource.FormatString("Err_NonClsException", "ServicedComponentProxy.DispatchActivate"));
						eventLog2.WriteEntry(text2, EventLogEntryType.Error);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002F34 File Offset: 0x00001F34
		private void DispatchDeactivate()
		{
			if (this._fDeliverADC)
			{
				ServicedComponent servicedComponent = (ServicedComponent)this.GetTransparentProxy();
				this._fIsServerActivated = false;
				try
				{
					if (!this._fFinalized)
					{
						servicedComponent.Deactivate();
					}
				}
				catch
				{
				}
				if (this.IsObjectPooled)
				{
					bool flag = false;
					try
					{
						if (!this._fFinalized)
						{
							flag = servicedComponent.CanBePooled();
						}
						this._proxyTearoff.SetCanBePooled(flag);
					}
					catch
					{
						this._proxyTearoff.SetCanBePooled(false);
					}
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002FC4 File Offset: 0x00001FC4
		internal void DispatchConstruct(string str)
		{
			ServicedComponent servicedComponent = (ServicedComponent)this.GetTransparentProxy();
			servicedComponent.Construct(str);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002FE4 File Offset: 0x00001FE4
		internal void ConnectForPooling(ServicedComponentProxy oldscp, ServicedComponent server, ProxyTearoff proxyTearoff, bool fForJit)
		{
			if (oldscp != null)
			{
				this._fReturnedByFinalizer = oldscp._fFinalized;
				if (fForJit)
				{
					this._pPoolUnk = oldscp._pPoolUnk;
					oldscp._pPoolUnk = IntPtr.Zero;
				}
			}
			if (server != null)
			{
				base.AttachServer(server);
			}
			this._proxyTearoff = proxyTearoff;
			this._proxyTearoff.Init(this);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003038 File Offset: 0x00002038
		internal ServicedComponent DisconnectForPooling(ref ProxyTearoff proxyTearoff)
		{
			if (this._fIsServerActivated)
			{
				this.DispatchDeactivate();
			}
			proxyTearoff = this._proxyTearoff;
			this._proxyTearoff = null;
			if (base.GetUnwrappedServer() != null)
			{
				return (ServicedComponent)base.DetachServer();
			}
			return null;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000306C File Offset: 0x0000206C
		internal void DeactivateProxy(bool disposing)
		{
			if (this._fIsActive)
			{
				object transparentProxy = this.GetTransparentProxy();
				if (base.GetUnwrappedServer() != null)
				{
					this.DispatchDeactivate();
					ServicedComponent servicedComponent = (ServicedComponent)transparentProxy;
					servicedComponent._callFinalize(disposing);
					base.DetachServer();
				}
				RealProxy.SetStubData(this, ServicedComponentProxy.NegativeOne);
				this._fIsActive = false;
				if (!this.IsJitActivated)
				{
					this.ReleaseGitCookie();
				}
				this.ReleasePoolUnk();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000030D8 File Offset: 0x000020D8
		internal void ActivateObject()
		{
			IntPtr currentContextToken = Proxy.GetCurrentContextToken();
			if (this.IsObjectPooled && this.IsJitActivated && this.HomeToken != currentContextToken)
			{
				object obj = IdentityTable.FindObject(currentContextToken);
				if (obj != null)
				{
					ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)RemotingServices.GetRealProxy(obj);
					ProxyTearoff proxyTearoff = null;
					ServicedComponent servicedComponent = this.DisconnectForPooling(ref proxyTearoff);
					proxyTearoff.SetCanBePooled(false);
					servicedComponentProxy.ConnectForPooling(this, servicedComponent, proxyTearoff, true);
					EnterpriseServicesHelper.SwitchWrappers(this, servicedComponentProxy);
					servicedComponentProxy.ActivateProxy();
					return;
				}
			}
			this.ActivateProxy();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003154 File Offset: 0x00002154
		internal void SendCreationEvents()
		{
			if (Util.ExtendedLifetime && this._context != IntPtr.Zero && this._context != ServicedComponentProxy.NegativeOne)
			{
				IntPtr intPtr = this.SupportsInterface(ref ServicedComponentProxy._s_IID_IManagedObjectInfo);
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						Proxy.SendCreationEvents(this._context, intPtr, this.IsJitActivated);
					}
					finally
					{
						Marshal.Release(intPtr);
					}
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000031D4 File Offset: 0x000021D4
		private void ReleasePoolUnk()
		{
			if (this._pPoolUnk != IntPtr.Zero)
			{
				IntPtr pPoolUnk = this._pPoolUnk;
				this._pPoolUnk = IntPtr.Zero;
				Proxy.PoolUnmark(pPoolUnk);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000320C File Offset: 0x0000220C
		internal void SendDestructionEvents(bool disposing)
		{
			if (Util.ExtendedLifetime && this._context != IntPtr.Zero && this._context != ServicedComponentProxy.NegativeOne)
			{
				IntPtr intPtr = this.SupportsInterface(ref ServicedComponentProxy._s_IID_IManagedObjectInfo);
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						Proxy.SendDestructionEvents(this._context, intPtr, disposing);
					}
					finally
					{
						Marshal.Release(intPtr);
					}
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003284 File Offset: 0x00002284
		private void SendDestructionEventsAsync()
		{
			if (AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				this.SendDestructionEvents(false);
				return;
			}
			lock (ServicedComponentProxy._ctxQueue)
			{
				ServicedComponentProxy._ctxQueue.Enqueue(this);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000032D8 File Offset: 0x000022D8
		internal void ConstructServer()
		{
			this.SetupContext(true);
			IConstructionReturnMessage constructionReturnMessage = base.InitializeServerObject(null);
			if (constructionReturnMessage != null && constructionReturnMessage.Exception != null)
			{
				ServicedComponent servicedComponent = (ServicedComponent)this.GetTransparentProxy();
				servicedComponent._callFinalize(true);
				base.DetachServer();
				throw constructionReturnMessage.Exception;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003320 File Offset: 0x00002320
		internal void SuppressFinalizeServer()
		{
			GC.SuppressFinalize(base.GetUnwrappedServer());
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000332D File Offset: 0x0000232D
		internal void ActivateProxy()
		{
			if (!this._fIsActive)
			{
				this._fIsActive = true;
				this.SetupContext(false);
				this.DispatchActivate();
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000334B File Offset: 0x0000234B
		internal void FilterConstructors()
		{
			if (this._fIsJitActivated)
			{
				throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_BadConfiguration"));
			}
			this._filterConstructors = true;
			RealProxy.SetStubData(this, ServicedComponentProxy.NegativeOne);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000337C File Offset: 0x0000237C
		public IntPtr GetOuterIUnknown()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			try
			{
				intPtr = base.GetCOMIUnknown(false);
				Guid iid_IUnknown = Util.IID_IUnknown;
				int num = Marshal.QueryInterface(intPtr, ref iid_IUnknown, out zero);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
			}
			return zero;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000033E0 File Offset: 0x000023E0
		public override IntPtr GetCOMIUnknown(bool fIsBeingMarshalled)
		{
			if (this._token == IntPtr.Zero || this._token == ServicedComponentProxy.NegativeOne || this._token == Proxy.GetCurrentContextToken())
			{
				if (fIsBeingMarshalled)
				{
					IntPtr intPtr = IntPtr.Zero;
					IntPtr intPtr2 = IntPtr.Zero;
					try
					{
						intPtr = base.GetCOMIUnknown(false);
						intPtr2 = Proxy.GetStandardMarshal(intPtr);
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.Release(intPtr);
						}
					}
					return intPtr2;
				}
				return base.GetCOMIUnknown(false);
			}
			else
			{
				if (Util.ExtendedLifetime)
				{
					IntPtr comiunknown = base.GetCOMIUnknown(false);
					IntPtr intPtr3 = IntPtr.Zero;
					try
					{
						byte[] array = this._callback.SwitchMarshal(this._context, comiunknown);
						intPtr3 = Proxy.UnmarshalObject(array);
					}
					finally
					{
						if (comiunknown != IntPtr.Zero)
						{
							Marshal.Release(comiunknown);
						}
					}
					return intPtr3;
				}
				if (this._gitCookie == 0)
				{
					return base.GetCOMIUnknown(false);
				}
				return Proxy.GetObject(this._gitCookie);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000034E4 File Offset: 0x000024E4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override void SetCOMIUnknown(IntPtr i)
		{
			bool flag = false;
			if (this._gitCookie != 0)
			{
				return;
			}
			if (Util.ExtendedLifetime)
			{
				return;
			}
			try
			{
				if (i == IntPtr.Zero)
				{
					flag = true;
					i = Marshal.GetIUnknownForObject(this.GetTransparentProxy());
				}
				this._gitCookie = Proxy.StoreObject(i);
				if (this._proxyTearoff != null)
				{
					Marshal.ChangeWrapperHandleStrength(this._proxyTearoff, true);
				}
				Marshal.ChangeWrapperHandleStrength(this.GetTransparentProxy(), true);
			}
			finally
			{
				if (flag && i != IntPtr.Zero)
				{
					Marshal.Release(i);
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003578 File Offset: 0x00002578
		internal ProxyTearoff GetProxyTearoff()
		{
			if (this._proxyTearoff == null)
			{
				if (Util.ExtendedLifetime)
				{
					this._proxyTearoff = new WeakProxyTearoff();
				}
				else
				{
					this._proxyTearoff = new ClassicProxyTearoff();
				}
				this._proxyTearoff.Init(this);
			}
			return this._proxyTearoff;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000035B4 File Offset: 0x000025B4
		public override IntPtr SupportsInterface(ref Guid iid)
		{
			if (ServicedComponentProxy._s_IID_IObjectControl.Equals(iid))
			{
				return Marshal.GetComInterfaceForObject(this.GetProxyTearoff(), typeof(IObjectControl));
			}
			if (ServicedComponentProxy._s_IID_IObjectConstruct.Equals(iid))
			{
				return Marshal.GetComInterfaceForObject(this.GetProxyTearoff(), typeof(IObjectConstruct));
			}
			if (ServicedComponentProxy._s_IID_IManagedPoolAction.Equals(iid))
			{
				return Marshal.GetComInterfaceForObject(this, typeof(IManagedPoolAction));
			}
			if (Util.ExtendedLifetime && ServicedComponentProxy._s_IID_IManagedObjectInfo.Equals(iid))
			{
				return Marshal.GetComInterfaceForObject(this._scstub, typeof(IManagedObjectInfo));
			}
			return IntPtr.Zero;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003668 File Offset: 0x00002668
		public override ObjRef CreateObjRef(Type requestedType)
		{
			return new ServicedComponentMarshaler((MarshalByRefObject)this.GetTransparentProxy(), requestedType);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000367C File Offset: 0x0000267C
		public override IMessage Invoke(IMessage request)
		{
			IMessage message;
			if (this._token == Proxy.GetCurrentContextToken())
			{
				message = this.LocalInvoke(request);
			}
			else
			{
				message = this.CrossCtxInvoke(request);
			}
			return message;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000036B0 File Offset: 0x000026B0
		public IMessage LocalInvoke(IMessage reqMsg)
		{
			IMessage message = null;
			if (reqMsg is IConstructionCallMessage)
			{
				this.ActivateProxy();
				if (this._filterConstructors)
				{
					this._filterConstructors = false;
					RealProxy.SetStubData(this, this._token);
				}
				if (((IConstructionCallMessage)reqMsg).ArgCount > 0)
				{
					throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_ConstructorArguments"));
				}
				MarshalByRefObject marshalByRefObject = (MarshalByRefObject)this.GetTransparentProxy();
				message = EnterpriseServicesHelper.CreateConstructionReturnMessage((IConstructionCallMessage)reqMsg, marshalByRefObject);
			}
			else if (reqMsg is IMethodCallMessage)
			{
				message = this.HandleSpecialMethods(reqMsg);
				if (message != null)
				{
					return message;
				}
				if (base.GetUnwrappedServer() == null || (IntPtr)RealProxy.GetStubData(this) == ServicedComponentProxy.NegativeOne)
				{
					throw new ObjectDisposedException("ServicedComponent");
				}
				bool flag = this.SendMethodCall(reqMsg);
				try
				{
					message = RemotingServices.ExecuteMessage((MarshalByRefObject)this.GetTransparentProxy(), (IMethodCallMessage)reqMsg);
					if (flag)
					{
						this.SendMethodReturn(reqMsg, ((IMethodReturnMessage)message).Exception);
					}
				}
				catch (Exception ex)
				{
					if (flag)
					{
						this.SendMethodReturn(reqMsg, ex);
					}
					throw;
				}
				catch
				{
					if (flag)
					{
						this.SendMethodReturn(reqMsg, null);
					}
					throw;
				}
			}
			return message;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000037DC File Offset: 0x000027DC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private IMessage CrossCtxInvoke(IMessage reqMsg)
		{
			this.AssertValid();
			IMessage message = this.HandleDispose(reqMsg);
			if (message != null)
			{
				return message;
			}
			message = this.HandleSetCOMIUnknown(reqMsg);
			if (message != null)
			{
				return message;
			}
			message = this.HandleSpecialMethods(reqMsg);
			if (message != null)
			{
				return message;
			}
			object transparentProxy = this.GetTransparentProxy();
			MethodBase methodBase = ((IMethodMessage)reqMsg).MethodBase;
			MemberInfo memberInfo = methodBase;
			MemberInfo memberInfo2 = memberInfo;
			MemberInfo memberInfo3 = ReflectionCache.ConvertToClassMI(base.GetProxiedType(), memberInfo);
			bool flag = false;
			int num = ServicedComponentInfo.MICachedLookup(memberInfo3);
			MemberInfo memberInfo4;
			if (reqMsg is IConstructionCallMessage)
			{
				ComMemberType comMemberType = ComMemberType.Method;
				memberInfo2 = Marshal.GetMethodInfoForComSlot(typeof(IManagedObject), 3, ref comMemberType);
			}
			else if ((memberInfo4 = this.AliasCall(methodBase as MethodInfo)) != null)
			{
				memberInfo2 = memberInfo4;
			}
			else if (this._fUseIntfDispatch || (num & 4) != 0)
			{
				memberInfo2 = ReflectionCache.ConvertToInterfaceMI(memberInfo);
				if (memberInfo2 == null)
				{
					throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_SecurityMapping"));
				}
			}
			else
			{
				flag = (num & 2) != 0;
			}
			return this._callback.DoCallback(transparentProxy, reqMsg, this._context, flag, memberInfo2, this._gitCookie != 0);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000038E0 File Offset: 0x000028E0
		private MemberInfo AliasCall(MethodInfo mi)
		{
			if (mi == null)
			{
				return null;
			}
			MethodInfo baseDefinition = mi.GetBaseDefinition();
			if (baseDefinition == ServicedComponentProxy._internalDeactivateMethod)
			{
				return ServicedComponentProxy._getIDisposableDispose;
			}
			if (baseDefinition == ServicedComponentProxy._initializeLifetimeServiceMethod || baseDefinition == ServicedComponentProxy._getLifetimeServiceMethod || baseDefinition == ServicedComponentProxy._getComIUnknownMethod || baseDefinition == ServicedComponentProxy._setCOMIUnknownMethod)
			{
				ComMemberType comMemberType = ComMemberType.Method;
				return Marshal.GetMethodInfoForComSlot(typeof(IManagedObject), 3, ref comMemberType);
			}
			return null;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003940 File Offset: 0x00002940
		private IMessage HandleDispose(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				MethodBase methodBase = methodCallMessage.MethodBase;
				if (methodBase == ServicedComponentProxy._getServicedComponentDispose || methodBase == ServicedComponentProxy._getIDisposableDispose)
				{
					ServicedComponent.DisposeObject((ServicedComponent)this.GetTransparentProxy());
					IMethodCallMessage methodCallMessage2 = (IMethodCallMessage)msg;
					return new ReturnMessage(null, null, 0, methodCallMessage2.LogicalCallContext, methodCallMessage2);
				}
			}
			return null;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003998 File Offset: 0x00002998
		private IMessage HandleSetCOMIUnknown(IMessage reqMsg)
		{
			MethodBase methodBase = ((IMethodMessage)reqMsg).MethodBase;
			if (methodBase == ServicedComponentProxy._setCOMIUnknownMethod)
			{
				IMethodCallMessage methodCallMessage = (IMethodCallMessage)reqMsg;
				IntPtr intPtr = (IntPtr)methodCallMessage.InArgs[0];
				if (intPtr != IntPtr.Zero)
				{
					this.SetCOMIUnknown(intPtr);
					return new ReturnMessage(null, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
				}
			}
			return null;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000039F4 File Offset: 0x000029F4
		private IMessage HandleSpecialMethods(IMessage reqMsg)
		{
			MethodBase methodBase = ((IMethodMessage)reqMsg).MethodBase;
			if (methodBase == ServicedComponentProxy._getTypeMethod)
			{
				IMethodCallMessage methodCallMessage = (IMethodCallMessage)reqMsg;
				return new ReturnMessage(base.GetProxiedType(), null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
			}
			if (methodBase == ServicedComponentProxy._getHashCodeMethod)
			{
				int hashCode = this.GetHashCode();
				IMethodCallMessage methodCallMessage2 = (IMethodCallMessage)reqMsg;
				return new ReturnMessage(hashCode, null, 0, methodCallMessage2.LogicalCallContext, methodCallMessage2);
			}
			return null;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003A5D File Offset: 0x00002A5D
		private bool IsRealCall(MethodBase mb)
		{
			return mb != ServicedComponentProxy._internalDeactivateMethod && mb != ServicedComponentProxy._initializeLifetimeServiceMethod && mb != ServicedComponentProxy._getLifetimeServiceMethod && mb != ServicedComponentProxy._getComIUnknownMethod && mb != ServicedComponentProxy._setCOMIUnknownMethod && mb != ServicedComponentProxy._getTypeMethod && mb != ServicedComponentProxy._getHashCodeMethod;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003A9C File Offset: 0x00002A9C
		private bool SendMethodCall(IMessage req)
		{
			bool flag = false;
			if (this._tracker != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					IMethodCallMessage methodCallMessage = req as IMethodCallMessage;
					if (!this.IsRealCall(methodCallMessage.MethodBase))
					{
						return false;
					}
					if (Util.ExtendedLifetime)
					{
						intPtr = this.SupportsInterface(ref ServicedComponentProxy._s_IID_IManagedObjectInfo);
					}
					else
					{
						intPtr = this.GetOuterIUnknown();
					}
					MethodBase methodBase = ReflectionCache.ConvertToInterfaceMI(methodCallMessage.MethodBase) as MethodBase;
					if (methodBase != null)
					{
						this._tracker.SendMethodCall(intPtr, methodBase);
						flag = true;
					}
				}
				catch
				{
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.Release(intPtr);
					}
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003B50 File Offset: 0x00002B50
		private void SendMethodReturn(IMessage req, Exception except)
		{
			if (this._tracker != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					IMethodCallMessage methodCallMessage = req as IMethodCallMessage;
					if (this.IsRealCall(methodCallMessage.MethodBase))
					{
						if (Util.ExtendedLifetime)
						{
							intPtr = this.SupportsInterface(ref ServicedComponentProxy._s_IID_IManagedObjectInfo);
						}
						else
						{
							intPtr = this.GetOuterIUnknown();
						}
						MethodBase methodBase = ReflectionCache.ConvertToInterfaceMI(methodCallMessage.MethodBase) as MethodBase;
						if (methodBase != null)
						{
							this._tracker.SendMethodReturn(intPtr, methodBase, except);
						}
					}
				}
				catch
				{
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.Release(intPtr);
					}
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003BF8 File Offset: 0x00002BF8
		private void ReleaseGitCookie()
		{
			int num = Interlocked.Exchange(ref this._gitCookie, 0);
			if (num != 0)
			{
				Proxy.RevokeObject(num);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003C1C File Offset: 0x00002C1C
		private void SetupContext(bool construction)
		{
			IntPtr currentContextToken = Proxy.GetCurrentContextToken();
			IdentityManager.NoticeApartment();
			if (this._token != currentContextToken)
			{
				if (this._token != ServicedComponentProxy.NegativeOne)
				{
					this.ReleaseContext();
				}
				this._token = currentContextToken;
				this._context = Proxy.GetCurrentContext();
				this._tracker = Proxy.FindTracker(this._context);
			}
			if (!this._filterConstructors)
			{
				RealProxy.SetStubData(this, this._token);
			}
			if (this.IsJitActivated && !this._tabled && !construction)
			{
				IdentityTable.AddObject(this._token, this.GetTransparentProxy());
				this._tabled = true;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003CC4 File Offset: 0x00002CC4
		private void ReleaseContext()
		{
			if (this._token != ServicedComponentProxy.NegativeOne)
			{
				object transparentProxy = this.GetTransparentProxy();
				if (this.IsJitActivated && this._tabled)
				{
					IdentityTable.RemoveObject(this._token, transparentProxy);
					this._tabled = false;
				}
				if (this._tracker != null)
				{
					this._tracker.Release();
				}
				Marshal.Release(this._context);
				this._context = ServicedComponentProxy.NegativeOne;
				this._token = ServicedComponentProxy.NegativeOne;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003D42 File Offset: 0x00002D42
		internal void SetInPool(bool fInPool, IntPtr pPooledObject)
		{
			if (!fInPool)
			{
				Proxy.PoolMark(pPooledObject);
				this._pPoolUnk = pPooledObject;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003D54 File Offset: 0x00002D54
		void IManagedPoolAction.LastRelease()
		{
			if (this.IsObjectPooled && base.GetUnwrappedServer() != null)
			{
				this.ReleaseContext();
				IntPtr currentContextToken = Proxy.GetCurrentContextToken();
				IntPtr currentContext = Proxy.GetCurrentContext();
				try
				{
					RealProxy.SetStubData(this, currentContextToken);
					((ServicedComponent)this.GetTransparentProxy())._callFinalize(!this._fFinalized && !this._fReturnedByFinalizer);
					GC.SuppressFinalize(this);
				}
				finally
				{
					Marshal.Release(currentContext);
					RealProxy.SetStubData(this, this._token);
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003DE4 File Offset: 0x00002DE4
		private void FinalizeHere()
		{
			IntPtr currentContextToken = Proxy.GetCurrentContextToken();
			IntPtr currentContext = Proxy.GetCurrentContext();
			try
			{
				RealProxy.SetStubData(this, currentContextToken);
				((ServicedComponent)this.GetTransparentProxy())._callFinalize(false);
			}
			finally
			{
				Marshal.Release(currentContext);
				RealProxy.SetStubData(this, ServicedComponentProxy.NegativeOne);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003E44 File Offset: 0x00002E44
		private void ReleaseContextAsync()
		{
			if (this._token != ServicedComponentProxy.NegativeOne)
			{
				if (AppDomain.CurrentDomain.IsFinalizingForUnload())
				{
					this.ReleaseContext();
					return;
				}
				object transparentProxy = this.GetTransparentProxy();
				if (this.IsJitActivated && this._tabled)
				{
					IdentityTable.RemoveObject(this._token, transparentProxy);
					this._tabled = false;
				}
				lock (ServicedComponentProxy._ctxQueue)
				{
					ServicedComponentProxy._ctxQueue.Enqueue(this._context);
				}
				this._context = ServicedComponentProxy.NegativeOne;
				this._token = ServicedComponentProxy.NegativeOne;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003EF0 File Offset: 0x00002EF0
		private void ReleaseGitCookieAsync()
		{
			if (this._gitCookie != 0)
			{
				if (AppDomain.CurrentDomain.IsFinalizingForUnload())
				{
					this.ReleaseGitCookie();
					return;
				}
				int gitCookie = this._gitCookie;
				this._gitCookie = 0;
				lock (ServicedComponentProxy._gitQueue)
				{
					ServicedComponentProxy._gitQueue.Enqueue(gitCookie);
				}
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003F5C File Offset: 0x00002F5C
		internal void Dispose(bool disposing)
		{
			if (Util.ExtendedLifetime && (disposing || !ServicedComponentProxy._asyncFinalizeEnabled))
			{
				this.SendDestructionEvents(disposing);
			}
			if (this._fIsActive)
			{
				ServicedComponent servicedComponent = (ServicedComponent)this.GetTransparentProxy();
				try
				{
					servicedComponent._internalDeactivate(disposing);
				}
				catch (ObjectDisposedException)
				{
				}
			}
			if (!disposing && this.IsObjectPooled && base.GetUnwrappedServer() != null)
			{
				this.FinalizeHere();
			}
			this.ReleasePoolUnk();
			if (Util.ExtendedLifetime && !disposing && ServicedComponentProxy._asyncFinalizeEnabled)
			{
				this.SendDestructionEventsAsync();
			}
			this.ReleaseGitCookie();
			if (disposing || !ServicedComponentProxy._asyncFinalizeEnabled || AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				this.ReleaseContext();
			}
			else if (!Util.ExtendedLifetime)
			{
				this.ReleaseContextAsync();
			}
			this._fIsActive = false;
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004028 File Offset: 0x00003028
		private void RefreshStub()
		{
			if (this._proxyTearoff != null)
			{
				this._proxyTearoff.Init(this);
			}
			if (this._scstub != null)
			{
				this._scstub.Refresh(this);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004054 File Offset: 0x00003054
		~ServicedComponentProxy()
		{
			this._fFinalized = true;
			try
			{
				if (this._gitCookie != 0)
				{
					GC.ReRegisterForFinalize(this);
					if (ServicedComponentProxy._asyncFinalizeEnabled)
					{
						this.ReleaseGitCookieAsync();
					}
					else
					{
						this.ReleaseGitCookie();
					}
					if (this._proxyTearoff != null)
					{
						Marshal.ChangeWrapperHandleStrength(this._proxyTearoff, false);
					}
					Marshal.ChangeWrapperHandleStrength(this.GetTransparentProxy(), false);
				}
				else
				{
					if (Util.ExtendedLifetime)
					{
						this.RefreshStub();
					}
					this.Dispose(!(this._pPoolUnk == IntPtr.Zero));
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000030 RID: 48
		private static readonly IntPtr NegativeOne = new IntPtr(-1);

		// Token: 0x04000031 RID: 49
		private static IntPtr _stub = Proxy.GetContextCheck();

		// Token: 0x04000032 RID: 50
		private static MethodInfo _getTypeMethod = typeof(object).GetMethod("GetType");

		// Token: 0x04000033 RID: 51
		private static MethodInfo _getHashCodeMethod = typeof(object).GetMethod("GetHashCode");

		// Token: 0x04000034 RID: 52
		private static MethodBase _getIDisposableDispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);

		// Token: 0x04000035 RID: 53
		private static MethodBase _getServicedComponentDispose = typeof(ServicedComponent).GetMethod("Dispose", new Type[0]);

		// Token: 0x04000036 RID: 54
		private static MethodInfo _internalDeactivateMethod = typeof(ServicedComponent).GetMethod("_internalDeactivate", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000037 RID: 55
		private static MethodInfo _initializeLifetimeServiceMethod = typeof(MarshalByRefObject).GetMethod("InitializeLifetimeService", new Type[0]);

		// Token: 0x04000038 RID: 56
		private static MethodInfo _getLifetimeServiceMethod = typeof(MarshalByRefObject).GetMethod("GetLifetimeService", new Type[0]);

		// Token: 0x04000039 RID: 57
		private static MethodInfo _getComIUnknownMethod = typeof(MarshalByRefObject).GetMethod("GetComIUnknown", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(bool) }, null);

		// Token: 0x0400003A RID: 58
		private static MethodInfo _setCOMIUnknownMethod = typeof(ServicedComponent).GetMethod("DoSetCOMIUnknown", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400003B RID: 59
		private int _gitCookie;

		// Token: 0x0400003C RID: 60
		private IntPtr _token;

		// Token: 0x0400003D RID: 61
		private IntPtr _context;

		// Token: 0x0400003E RID: 62
		private IntPtr _pPoolUnk;

		// Token: 0x0400003F RID: 63
		private Tracker _tracker;

		// Token: 0x04000040 RID: 64
		private bool _fIsObjectPooled;

		// Token: 0x04000041 RID: 65
		private bool _fIsJitActivated;

		// Token: 0x04000042 RID: 66
		private bool _fDeliverADC;

		// Token: 0x04000043 RID: 67
		private bool _fUseIntfDispatch;

		// Token: 0x04000044 RID: 68
		private bool _fIsServerActivated;

		// Token: 0x04000045 RID: 69
		private bool _fIsActive;

		// Token: 0x04000046 RID: 70
		private bool _tabled;

		// Token: 0x04000047 RID: 71
		private bool _fFinalized;

		// Token: 0x04000048 RID: 72
		private bool _fReturnedByFinalizer;

		// Token: 0x04000049 RID: 73
		private bool _filterConstructors;

		// Token: 0x0400004A RID: 74
		private ProxyTearoff _proxyTearoff;

		// Token: 0x0400004B RID: 75
		private ServicedComponentStub _scstub;

		// Token: 0x0400004C RID: 76
		private Callback _callback;

		// Token: 0x0400004D RID: 77
		private static bool _asyncFinalizeEnabled = true;

		// Token: 0x0400004E RID: 78
		private static Queue _ctxQueue;

		// Token: 0x0400004F RID: 79
		private static Queue _gitQueue;

		// Token: 0x04000050 RID: 80
		private static Thread _cleanupThread;

		// Token: 0x04000051 RID: 81
		private static AutoResetEvent _Wakeup;

		// Token: 0x04000052 RID: 82
		private static ManualResetEvent _exitCleanupThread;

		// Token: 0x04000053 RID: 83
		private static int _QueuedItemsCount;

		// Token: 0x04000054 RID: 84
		private static Guid _s_IID_IObjectControl = Marshal.GenerateGuidForType(typeof(IObjectControl));

		// Token: 0x04000055 RID: 85
		private static Guid _s_IID_IObjectConstruct = Marshal.GenerateGuidForType(typeof(IObjectConstruct));

		// Token: 0x04000056 RID: 86
		private static Guid _s_IID_IManagedObjectInfo = Marshal.GenerateGuidForType(typeof(IManagedObjectInfo));

		// Token: 0x04000057 RID: 87
		private static Guid _s_IID_IManagedPoolAction = Marshal.GenerateGuidForType(typeof(IManagedPoolAction));
	}
}
