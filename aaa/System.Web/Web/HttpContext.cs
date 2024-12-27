using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000064 RID: 100
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpContext : IServiceProvider
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x00011A54 File Offset: 0x00010A54
		public HttpContext(HttpRequest request, HttpResponse response)
		{
			this.Init(request, response);
			request.Context = this;
			response.Context = this;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00011A7D File Offset: 0x00010A7D
		public HttpContext(HttpWorkerRequest wr)
		{
			this._wr = wr;
			this.Init(new HttpRequest(wr, this), new HttpResponse(wr, this));
			this._response.InitResponseWriter();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00011AB8 File Offset: 0x00010AB8
		internal HttpContext(HttpWorkerRequest wr, bool initResponseWriter)
		{
			this._wr = wr;
			this.Init(new HttpRequest(wr, this), new HttpResponse(wr, this));
			if (initResponseWriter)
			{
				this._response.InitResponseWriter();
			}
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_EXECUTING);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00011B08 File Offset: 0x00010B08
		private void Init(HttpRequest request, HttpResponse response)
		{
			this._request = request;
			this._response = response;
			this._utcTimestamp = DateTime.UtcNow;
			if (this._wr is IIS7WorkerRequest)
			{
				this._isIntegratedPipeline = true;
			}
			if (!(this._wr is StateHttpWorkerRequest))
			{
				this.CookielessHelper.RemoveCookielessValuesFromPath();
			}
			Profiler profile = HttpRuntime.Profile;
			if (profile != null && profile.IsEnabled)
			{
				this._topTraceContext = new TraceContext(this);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00011B77 File Offset: 0x00010B77
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00011B83 File Offset: 0x00010B83
		public static HttpContext Current
		{
			get
			{
				return ContextBase.Current as HttpContext;
			}
			set
			{
				ContextBase.Current = value;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00011B8B File Offset: 0x00010B8B
		internal void Root()
		{
			this._root = GCHandle.Alloc(this);
			this._ctxPtr = GCHandle.ToIntPtr(this._root);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00011BAA File Offset: 0x00010BAA
		internal void Unroot()
		{
			if (this._root.IsAllocated)
			{
				this._root.Free();
				this._ctxPtr = IntPtr.Zero;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00011BCF File Offset: 0x00010BCF
		internal void FinishPipelineRequest()
		{
			if (!this._finishPipelineRequestCalled)
			{
				this._finishPipelineRequestCalled = true;
				HttpRuntime.FinishPipelineRequest(this);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00011BE6 File Offset: 0x00010BE6
		internal IntPtr ContextPtr
		{
			get
			{
				return this._ctxPtr;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011BF0 File Offset: 0x00010BF0
		internal void ValidatePath()
		{
			string physicalPathInternal = this._request.PhysicalPathInternal;
			CachedPathData configurationPathData = this.GetConfigurationPathData();
			if (StringUtil.EqualsIgnoreCase(configurationPathData.PhysicalPath, physicalPathInternal))
			{
				return;
			}
			FileUtil.CheckSuspiciousPhysicalPath(physicalPathInternal);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011C28 File Offset: 0x00010C28
		object IServiceProvider.GetService(Type service)
		{
			object obj;
			if (service == typeof(HttpWorkerRequest))
			{
				InternalSecurityPermissions.UnmanagedCode.Demand();
				obj = this._wr;
			}
			else if (service == typeof(HttpRequest))
			{
				obj = this.Request;
			}
			else if (service == typeof(HttpResponse))
			{
				obj = this.Response;
			}
			else if (service == typeof(HttpApplication))
			{
				obj = this.ApplicationInstance;
			}
			else if (service == typeof(HttpApplicationState))
			{
				obj = this.Application;
			}
			else if (service == typeof(HttpSessionState))
			{
				obj = this.Session;
			}
			else if (service == typeof(HttpServerUtility))
			{
				obj = this.Server;
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00011CDF File Offset: 0x00010CDF
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00011CE7 File Offset: 0x00010CE7
		internal IHttpAsyncHandler AsyncAppHandler
		{
			get
			{
				return this._asyncAppHandler;
			}
			set
			{
				this._asyncAppHandler = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00011CF0 File Offset: 0x00010CF0
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00011CF8 File Offset: 0x00010CF8
		public HttpApplication ApplicationInstance
		{
			get
			{
				return this._appInstance;
			}
			set
			{
				if (this._isIntegratedPipeline && this._appInstance != null && value != null)
				{
					throw new InvalidOperationException(SR.GetString("Application_instance_cannot_be_changed"));
				}
				this._appInstance = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00011D24 File Offset: 0x00010D24
		public HttpApplicationState Application
		{
			get
			{
				return HttpApplicationFactory.ApplicationState;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00011D2B File Offset: 0x00010D2B
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x00011D34 File Offset: 0x00010D34
		public IHttpHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
				this.RequiresSessionState = false;
				this.ReadOnlySessionState = false;
				this.InAspCompatMode = false;
				if (this._handler != null)
				{
					if (this._handler is IRequiresSessionState)
					{
						this.RequiresSessionState = true;
					}
					if (this._handler is IReadOnlySessionState)
					{
						this.ReadOnlySessionState = true;
					}
					Page page = this._handler as Page;
					if (page != null && page.IsInAspCompatMode)
					{
						this.InAspCompatMode = true;
					}
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00011DAB File Offset: 0x00010DAB
		public IHttpHandler PreviousHandler
		{
			get
			{
				if (this._handlerStack == null || this._handlerStack.Count == 0)
				{
					return null;
				}
				return (IHttpHandler)this._handlerStack.Peek();
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00011DD4 File Offset: 0x00010DD4
		public IHttpHandler CurrentHandler
		{
			get
			{
				if (this._currentHandler == null)
				{
					this._currentHandler = this._handler;
				}
				return this._currentHandler;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00011DF0 File Offset: 0x00010DF0
		internal void RestoreCurrentHandler()
		{
			this._currentHandler = (IHttpHandler)this._handlerStack.Pop();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00011E08 File Offset: 0x00010E08
		internal void SetCurrentHandler(IHttpHandler newtHandler)
		{
			if (this._handlerStack == null)
			{
				this._handlerStack = new Stack();
			}
			this._handlerStack.Push(this.CurrentHandler);
			this._currentHandler = newtHandler;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00011E38 File Offset: 0x00010E38
		public void RemapHandler(IHttpHandler handler)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				if (this._notificationContext.CurrentNotification >= RequestNotification.MapRequestHandler)
				{
					throw new InvalidOperationException(SR.GetString("Invoke_before_pipeline_event", new object[] { "HttpContext.RemapHandler", "HttpApplication.MapRequestHandler" }));
				}
				string text = null;
				string text2 = null;
				if (handler != null)
				{
					Type type = handler.GetType();
					text = type.AssemblyQualifiedName;
					text2 = type.FullName;
				}
				iis7WorkerRequest.SetRemapHandler(text, text2);
			}
			this._remapHandler = handler;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00011EBA File Offset: 0x00010EBA
		internal IHttpHandler RemapHandlerInstance
		{
			get
			{
				return this._remapHandler;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00011EC2 File Offset: 0x00010EC2
		public HttpRequest Request
		{
			get
			{
				if (this.HideRequestResponse)
				{
					throw new HttpException(SR.GetString("Request_not_available"));
				}
				return this._request;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00011EE2 File Offset: 0x00010EE2
		public HttpResponse Response
		{
			get
			{
				if (this.HideRequestResponse)
				{
					throw new HttpException(SR.GetString("Response_not_available"));
				}
				return this._response;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00011F04 File Offset: 0x00010F04
		internal IHttpHandler TopHandler
		{
			get
			{
				if (this._handlerStack == null)
				{
					return this._handler;
				}
				object[] array = this._handlerStack.ToArray();
				if (array == null || array.Length == 0)
				{
					return this._handler;
				}
				return (IHttpHandler)array[array.Length - 1];
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00011F47 File Offset: 0x00010F47
		public TraceContext Trace
		{
			get
			{
				if (this._topTraceContext == null)
				{
					this._topTraceContext = new TraceContext(this);
				}
				return this._topTraceContext;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00011F63 File Offset: 0x00010F63
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x00011F7A File Offset: 0x00010F7A
		internal bool TraceIsEnabled
		{
			get
			{
				return this._topTraceContext != null && this._topTraceContext.IsEnabled;
			}
			set
			{
				if (value)
				{
					this._topTraceContext = new TraceContext(this);
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00011F8B File Offset: 0x00010F8B
		public IDictionary Items
		{
			get
			{
				if (this._items == null)
				{
					this._items = new Hashtable();
				}
				return this._items;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00011FA8 File Offset: 0x00010FA8
		public HttpSessionState Session
		{
			get
			{
				if (this._sessionStateModule != null)
				{
					lock (this)
					{
						if (this._sessionStateModule != null)
						{
							this._sessionStateModule.InitStateStoreItem(true);
							this._sessionStateModule = null;
						}
					}
				}
				return (HttpSessionState)this.Items["AspSession"];
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00012010 File Offset: 0x00011010
		internal void AddDelayedHttpSessionState(SessionStateModule module)
		{
			if (this._sessionStateModule != null)
			{
				throw new HttpException(SR.GetString("Cant_have_multiple_session_module"));
			}
			this._sessionStateModule = module;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00012031 File Offset: 0x00011031
		internal void RemoveDelayedHttpSessionState()
		{
			this._sessionStateModule = null;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001203A File Offset: 0x0001103A
		public HttpServerUtility Server
		{
			get
			{
				if (this._server == null)
				{
					this._server = new HttpServerUtility(this);
				}
				return this._server;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00012058 File Offset: 0x00011058
		internal void ReportRuntimeErrorIfExists(ref RequestNotificationStatus status)
		{
			Exception error = this.Error;
			if (error == null || this._runtimeErrorReported)
			{
				return;
			}
			if (this._notificationContext != null && this.CurrentModuleIndex == -1)
			{
				try
				{
					IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
					if (this.Request.QueryString["aspxerrorpath"] != null && iis7WorkerRequest != null && string.IsNullOrEmpty(iis7WorkerRequest.GetManagedHandlerType()) && iis7WorkerRequest.GetCurrentModuleName() == "AspNetInitializationExceptionModule")
					{
						status = RequestNotificationStatus.Continue;
						return;
					}
				}
				catch
				{
				}
			}
			this._runtimeErrorReported = true;
			if (HttpRuntime.AppOfflineMessage != null)
			{
				try
				{
					this.Response.StatusCode = 404;
					this.Response.TrySkipIisCustomErrors = true;
					this.Response.OutputStream.Write(HttpRuntime.AppOfflineMessage, 0, HttpRuntime.AppOfflineMessage.Length);
					goto IL_0110;
				}
				catch
				{
					goto IL_0110;
				}
			}
			using (new HttpContextWrapper(this))
			{
				using (new ApplicationImpersonationContext())
				{
					try
					{
						try
						{
							this.Response.ReportRuntimeError(error, true, false);
						}
						catch (Exception ex)
						{
							this.Response.ReportRuntimeError(ex, false, false);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			IL_0110:
			status = RequestNotificationStatus.FinishRequest;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x000121C4 File Offset: 0x000111C4
		public Exception Error
		{
			get
			{
				if (this._tempError != null)
				{
					return this._tempError;
				}
				if (this._errors == null || this._errors.Count == 0 || this._errorCleared)
				{
					return null;
				}
				return (Exception)this._errors[0];
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00012210 File Offset: 0x00011210
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x00012218 File Offset: 0x00011218
		internal Exception TempError
		{
			get
			{
				return this._tempError;
			}
			set
			{
				this._tempError = value;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00012224 File Offset: 0x00011224
		public Exception[] AllErrors
		{
			get
			{
				int num = ((this._errors != null) ? this._errors.Count : 0);
				if (num == 0)
				{
					return null;
				}
				Exception[] array = new Exception[num];
				this._errors.CopyTo(0, array, 0, num);
				return array;
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00012264 File Offset: 0x00011264
		public void AddError(Exception errorInfo)
		{
			if (this._errors == null)
			{
				this._errors = new ArrayList();
			}
			this._errors.Add(errorInfo);
			if (this._isIntegratedPipeline && this._notificationContext != null)
			{
				this._notificationContext.Error = errorInfo;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000122A2 File Offset: 0x000112A2
		public void ClearError()
		{
			if (this._tempError != null)
			{
				this._tempError = null;
			}
			else
			{
				this._errorCleared = true;
			}
			if (this._isIntegratedPipeline && this._notificationContext != null)
			{
				this._notificationContext.Error = null;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000122D8 File Offset: 0x000112D8
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000122E0 File Offset: 0x000112E0
		public IPrincipal User
		{
			get
			{
				return this._user;
			}
			[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
			set
			{
				this.SetPrincipalNoDemand(value);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000122EC File Offset: 0x000112EC
		internal void SetPrincipalNoDemand(IPrincipal principal, bool needToSetNativePrincipal)
		{
			this._user = principal;
			if (needToSetNativePrincipal && this._isIntegratedPipeline && this._notificationContext.CurrentNotification == RequestNotification.AuthenticateRequest)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
					if (principal != null)
					{
						GCHandle gchandle = GCHandle.Alloc(principal);
						try
						{
							intPtr = GCHandle.ToIntPtr(gchandle);
							iis7WorkerRequest.SetPrincipal(principal, intPtr);
							goto IL_007B;
						}
						catch
						{
							intPtr = IntPtr.Zero;
							if (gchandle.IsAllocated)
							{
								gchandle.Free();
							}
							throw;
						}
					}
					iis7WorkerRequest.SetPrincipal(null, IntPtr.Zero);
					IL_007B:;
				}
				finally
				{
					if (this._pManagedPrincipal != IntPtr.Zero)
					{
						GCHandle gchandle2 = GCHandle.FromIntPtr(this._pManagedPrincipal);
						if (gchandle2.IsAllocated)
						{
							gchandle2.Free();
						}
					}
					this._pManagedPrincipal = intPtr;
				}
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000123C8 File Offset: 0x000113C8
		internal void SetPrincipalNoDemand(IPrincipal principal)
		{
			this.SetPrincipalNoDemand(principal, true);
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000123D4 File Offset: 0x000113D4
		public ProfileBase Profile
		{
			get
			{
				if (this._Profile == null && this._ProfileDelayLoad)
				{
					this._Profile = ProfileBase.Create(this.Request.IsAuthenticated ? this.User.Identity.Name : this.Request.AnonymousID, this.Request.IsAuthenticated);
				}
				return this._Profile;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00012437 File Offset: 0x00011437
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0001243F File Offset: 0x0001143F
		public bool SkipAuthorization
		{
			get
			{
				return this._skipAuthorization;
			}
			[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
			set
			{
				this.SetSkipAuthorizationNoDemand(value, false);
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00012449 File Offset: 0x00011449
		internal void SetSkipAuthorizationNoDemand(bool value, bool managedOnly)
		{
			if (HttpRuntime.UseIntegratedPipeline && !managedOnly && value != this._skipAuthorization)
			{
				this._request.SetSkipAuthorization(value);
			}
			this._skipAuthorization = value;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00012474 File Offset: 0x00011474
		public bool IsDebuggingEnabled
		{
			get
			{
				bool flag;
				try
				{
					flag = CompilationUtil.IsDebuggingEnabled(this);
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000124A0 File Offset: 0x000114A0
		public bool IsCustomErrorEnabled
		{
			get
			{
				return CustomErrorsSection.GetSettings(this).CustomErrorsEnabled(this._request);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000124B3 File Offset: 0x000114B3
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x000124BB File Offset: 0x000114BB
		internal TemplateControl TemplateControl
		{
			get
			{
				return this._templateControl;
			}
			set
			{
				this._templateControl = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000124C4 File Offset: 0x000114C4
		public DateTime Timestamp
		{
			get
			{
				return this._utcTimestamp.ToLocalTime();
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000124D1 File Offset: 0x000114D1
		internal DateTime UtcTimestamp
		{
			get
			{
				return this._utcTimestamp;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x000124D9 File Offset: 0x000114D9
		internal HttpWorkerRequest WorkerRequest
		{
			get
			{
				return this._wr;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000124E1 File Offset: 0x000114E1
		public Cache Cache
		{
			get
			{
				return HttpRuntime.Cache;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000124E8 File Offset: 0x000114E8
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00012510 File Offset: 0x00011510
		internal VirtualPath ConfigurationPath
		{
			get
			{
				if (this._configurationPath == null)
				{
					this._configurationPath = this._request.FilePathObject;
				}
				return this._configurationPath;
			}
			set
			{
				this._configurationPath = value;
				if (this._configurationPathData != null)
				{
					if (!this._configurationPathData.CompletedFirstRequest)
					{
						CachedPathData.RemoveBadPathData(this._configurationPathData);
					}
					this._configurationPathData = null;
				}
				if (this._filePathData != null)
				{
					if (!this._filePathData.CompletedFirstRequest)
					{
						CachedPathData.RemoveBadPathData(this._filePathData);
					}
					this._filePathData = null;
				}
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00012572 File Offset: 0x00011572
		internal CachedPathData GetFilePathData()
		{
			if (this._filePathData == null)
			{
				this._filePathData = CachedPathData.GetVirtualPathData(this._request.FilePathObject, false);
			}
			return this._filePathData;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00012599 File Offset: 0x00011599
		internal CachedPathData GetConfigurationPathData()
		{
			if (this._configurationPath == null)
			{
				return this.GetFilePathData();
			}
			if (this._configurationPathData == null)
			{
				this._configurationPathData = CachedPathData.GetVirtualPathData(this._configurationPath, true);
			}
			return this._configurationPathData;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000125D0 File Offset: 0x000115D0
		internal CachedPathData GetPathData(VirtualPath path)
		{
			if (path != null)
			{
				if (path.Equals(this._request.FilePathObject))
				{
					return this.GetFilePathData();
				}
				if (this._configurationPath != null && path.Equals(this._configurationPath))
				{
					return this.GetConfigurationPathData();
				}
			}
			return CachedPathData.GetVirtualPathData(path, false);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001262A File Offset: 0x0001162A
		internal void FirstNotificationInit()
		{
			if (!this._firstNotificationInitCalled)
			{
				this._firstNotificationInitCalled = true;
				this.ValidatePath();
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00012641 File Offset: 0x00011641
		internal void FinishRequestForCachedPathData(int statusCode)
		{
			if (this._filePathData != null && !this._filePathData.CompletedFirstRequest)
			{
				if (400 <= statusCode && statusCode < 500)
				{
					CachedPathData.RemoveBadPathData(this._filePathData);
					return;
				}
				CachedPathData.MarkCompleted(this._filePathData);
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001267F File Offset: 0x0001167F
		[Obsolete("The recommended alternative is System.Web.Configuration.WebConfigurationManager.GetWebApplicationSection in System.Web.dll. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static object GetAppConfig(string name)
		{
			return WebConfigurationManager.GetWebApplicationSection(name);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00012687 File Offset: 0x00011687
		[Obsolete("The recommended alternative is System.Web.HttpContext.GetSection in System.Web.dll. http://go.microsoft.com/fwlink/?linkid=14202")]
		public object GetConfig(string name)
		{
			return this.GetSection(name);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00012690 File Offset: 0x00011690
		public object GetSection(string sectionName)
		{
			if (HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return this.GetConfigurationPathData().ConfigRecord.GetSection(sectionName);
			}
			return ConfigurationManager.GetSection(sectionName);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000126B1 File Offset: 0x000116B1
		internal RuntimeConfig GetRuntimeConfig()
		{
			return this.GetConfigurationPathData().RuntimeConfig;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000126BE File Offset: 0x000116BE
		internal RuntimeConfig GetRuntimeConfig(VirtualPath path)
		{
			return this.GetPathData(path).RuntimeConfig;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000126CC File Offset: 0x000116CC
		public void RewritePath(string path)
		{
			this.RewritePath(path, true);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000126D8 File Offset: 0x000116D8
		public void RewritePath(string path, bool rebaseClientPath)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string text = null;
			int num = path.IndexOf('?');
			if (num >= 0)
			{
				text = ((num < path.Length - 1) ? path.Substring(num + 1) : string.Empty);
				path = path.Substring(0, num);
			}
			VirtualPath virtualPath = VirtualPath.Create(path);
			virtualPath = this.Request.FilePathObject.Combine(virtualPath);
			virtualPath.FailIfNotWithinAppRoot();
			this.ConfigurationPath = null;
			this.Request.InternalRewritePath(virtualPath, text, rebaseClientPath);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001275C File Offset: 0x0001175C
		public void RewritePath(string filePath, string pathInfo, string queryString)
		{
			this.RewritePath(VirtualPath.CreateAllowNull(filePath), VirtualPath.CreateAllowNull(pathInfo), queryString, false);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00012772 File Offset: 0x00011772
		public void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
		{
			this.RewritePath(VirtualPath.CreateAllowNull(filePath), VirtualPath.CreateAllowNull(pathInfo), queryString, setClientFilePath);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001278C File Offset: 0x0001178C
		internal void RewritePath(VirtualPath filePath, VirtualPath pathInfo, string queryString, bool setClientFilePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			filePath = this.Request.FilePathObject.Combine(filePath);
			filePath.FailIfNotWithinAppRoot();
			this.ConfigurationPath = null;
			this.Request.InternalRewritePath(filePath, pathInfo, queryString, setClientFilePath);
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x000127DD File Offset: 0x000117DD
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x000127E5 File Offset: 0x000117E5
		internal CultureInfo DynamicCulture
		{
			get
			{
				return this._dynamicCulture;
			}
			set
			{
				this._dynamicCulture = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000127EE File Offset: 0x000117EE
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x000127F6 File Offset: 0x000117F6
		internal CultureInfo DynamicUICulture
		{
			get
			{
				return this._dynamicUICulture;
			}
			set
			{
				this._dynamicUICulture = value;
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000127FF File Offset: 0x000117FF
		public static object GetGlobalResourceObject(string classKey, string resourceKey)
		{
			return HttpContext.GetGlobalResourceObject(classKey, resourceKey, null);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00012809 File Offset: 0x00011809
		public static object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
		{
			return ResourceExpressionBuilder.GetGlobalResourceObject(classKey, resourceKey, null, null, culture);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00012815 File Offset: 0x00011815
		public static object GetLocalResourceObject(string virtualPath, string resourceKey)
		{
			return HttpContext.GetLocalResourceObject(virtualPath, resourceKey, null);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00012820 File Offset: 0x00011820
		public static object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
		{
			IResourceProvider localResourceProvider = ResourceExpressionBuilder.GetLocalResourceProvider(VirtualPath.Create(virtualPath));
			return ResourceExpressionBuilder.GetResourceObject(localResourceProvider, resourceKey, culture);
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00012841 File Offset: 0x00011841
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00012849 File Offset: 0x00011849
		internal int ServerExecuteDepth
		{
			get
			{
				return this._serverExecuteDepth;
			}
			set
			{
				this._serverExecuteDepth = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00012852 File Offset: 0x00011852
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0001285A File Offset: 0x0001185A
		internal bool PreventPostback
		{
			get
			{
				return this._preventPostback;
			}
			set
			{
				this._preventPostback = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00012863 File Offset: 0x00011863
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0001286B File Offset: 0x0001186B
		internal Thread CurrentThread
		{
			get
			{
				return this._thread;
			}
			set
			{
				this._thread = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00012874 File Offset: 0x00011874
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00012882 File Offset: 0x00011882
		internal TimeSpan Timeout
		{
			get
			{
				this.EnsureTimeout();
				return this._timeout;
			}
			set
			{
				this._timeout = value;
				this._timeoutSet = true;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00012894 File Offset: 0x00011894
		internal void EnsureTimeout()
		{
			if (!this._timeoutSet)
			{
				HttpRuntimeSection httpRuntime = RuntimeConfig.GetConfig(this).HttpRuntime;
				int num = (int)httpRuntime.ExecutionTimeout.TotalSeconds;
				this._timeout = new TimeSpan(0, 0, num);
				this._timeoutSet = true;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000128DA File Offset: 0x000118DA
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x000128E2 File Offset: 0x000118E2
		internal DoubleLink TimeoutLink
		{
			get
			{
				return this._timeoutLink;
			}
			set
			{
				this._timeoutLink = value;
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x000128EB File Offset: 0x000118EB
		internal void BeginCancellablePeriod()
		{
			if (this._timeoutStartTime == DateTime.MinValue)
			{
				this.SetStartTime();
			}
			this._timeoutState = 1;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001290C File Offset: 0x0001190C
		internal void SetStartTime()
		{
			this._timeoutStartTime = DateTime.UtcNow;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00012919 File Offset: 0x00011919
		internal void EndCancellablePeriod()
		{
			Interlocked.CompareExchange(ref this._timeoutState, 0, 1);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00012929 File Offset: 0x00011929
		internal void WaitForExceptionIfCancelled()
		{
			while (this._timeoutState == -1)
			{
				Thread.Sleep(100);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001293D File Offset: 0x0001193D
		internal bool IsInCancellablePeriod
		{
			get
			{
				return this._timeoutState == 1;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00012948 File Offset: 0x00011948
		internal Thread MustTimeout(DateTime utcNow)
		{
			if (this._timeoutState == 1 && TimeSpan.Compare(utcNow.Subtract(this._timeoutStartTime), this.Timeout) >= 0)
			{
				try
				{
					if (CompilationUtil.IsDebuggingEnabled(this) || Debugger.IsAttached)
					{
						return null;
					}
				}
				catch
				{
					return null;
				}
				if (Interlocked.CompareExchange(ref this._timeoutState, -1, 1) == 1)
				{
					if (this._wr.IsInReadEntitySync)
					{
						this.AbortConnection();
					}
					return this._thread;
				}
			}
			IL_0068:
			return null;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000129D0 File Offset: 0x000119D0
		internal long TimeLeft
		{
			get
			{
				long num = long.MaxValue;
				if (this.IsInCancellablePeriod && this.Timeout.TotalMilliseconds >= 0.0)
				{
					try
					{
						if (!CompilationUtil.IsDebuggingEnabled(this) && !Debugger.IsAttached)
						{
							num = (long)(this._timeoutStartTime.Add(this.Timeout) - DateTime.UtcNow).TotalMilliseconds;
						}
					}
					catch
					{
					}
				}
				return num;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00012A54 File Offset: 0x00011A54
		internal void InvokeCancellableCallback(WaitCallback callback, object state)
		{
			if (this.IsInCancellablePeriod)
			{
				callback(state);
				return;
			}
			try
			{
				this.BeginCancellablePeriod();
				try
				{
					callback(state);
				}
				finally
				{
					this.EndCancellablePeriod();
				}
				this.WaitForExceptionIfCancelled();
			}
			catch (ThreadAbortException ex)
			{
				if (ex.ExceptionState != null && ex.ExceptionState is HttpApplication.CancelModuleException && ((HttpApplication.CancelModuleException)ex.ExceptionState).Timeout)
				{
					Thread.ResetAbort();
					PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_TIMED_OUT);
					throw new HttpException(SR.GetString("Request_timed_out"), null, 3001);
				}
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00012AF8 File Offset: 0x00011AF8
		internal void PushTraceContext()
		{
			if (this._traceContextStack == null)
			{
				this._traceContextStack = new Stack();
			}
			this._traceContextStack.Push(this._topTraceContext);
			if (this._topTraceContext != null)
			{
				TraceContext traceContext = new TraceContext(this);
				this._topTraceContext.CopySettingsTo(traceContext);
				this._topTraceContext = traceContext;
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00012B4B File Offset: 0x00011B4B
		internal void PopTraceContext()
		{
			this._topTraceContext = (TraceContext)this._traceContextStack.Pop();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00012B63 File Offset: 0x00011B63
		internal bool RequestRequiresAuthorization()
		{
			return this.User.Identity.IsAuthenticated && (FileAuthorizationModule.RequestRequiresAuthorization(this) || UrlAuthorizationModule.RequestRequiresAuthorization(this));
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00012B89 File Offset: 0x00011B89
		internal int CallISAPI(UnsafeNativeMethods.CallISAPIFunc iFunction, byte[] bufIn, byte[] bufOut)
		{
			if (this._wr == null || !(this._wr is ISAPIWorkerRequest))
			{
				throw new HttpException(SR.GetString("Cannot_call_ISAPI_functions"));
			}
			return ((ISAPIWorkerRequest)this._wr).CallISAPI(iFunction, bufIn, bufOut);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00012BC3 File Offset: 0x00011BC3
		internal void SendEmptyResponse()
		{
			if (this._wr != null && this._wr is ISAPIWorkerRequest)
			{
				((ISAPIWorkerRequest)this._wr).SendEmptyResponse();
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00012BEA File Offset: 0x00011BEA
		internal CookielessHelperClass CookielessHelper
		{
			get
			{
				if (this._CookielessHelper == null)
				{
					this._CookielessHelper = new CookielessHelperClass(this);
				}
				return this._CookielessHelper;
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00012C06 File Offset: 0x00011C06
		internal void ResetSqlDependencyCookie()
		{
			if (this._sqlDependencyCookie != null)
			{
				CallContext.LogicalSetData("MS.SqlDependencyCookie", this._sqlDependencyCookie);
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00012C20 File Offset: 0x00011C20
		internal void RemoveSqlDependencyCookie()
		{
			if (this._sqlDependencyCookie != null)
			{
				CallContext.LogicalSetData("MS.SqlDependencyCookie", null);
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00012C35 File Offset: 0x00011C35
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00012C3D File Offset: 0x00011C3D
		internal string SqlDependencyCookie
		{
			get
			{
				return this._sqlDependencyCookie;
			}
			set
			{
				this._sqlDependencyCookie = value;
				CallContext.LogicalSetData("MS.SqlDependencyCookie", value);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00012C51 File Offset: 0x00011C51
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00012C59 File Offset: 0x00011C59
		internal NotificationContext NotificationContext
		{
			get
			{
				return this._notificationContext;
			}
			set
			{
				this._notificationContext = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00012C62 File Offset: 0x00011C62
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00012C86 File Offset: 0x00011C86
		public RequestNotification CurrentNotification
		{
			get
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				return this._notificationContext.CurrentNotification;
			}
			internal set
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this._notificationContext.CurrentNotification = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00012CAB File Offset: 0x00011CAB
		internal bool IsChangeInServerVars
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 1) == 1;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00012CBD File Offset: 0x00011CBD
		internal bool IsChangeInRequestHeaders
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 2) == 2;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00012CCF File Offset: 0x00011CCF
		internal bool IsChangeInResponseHeaders
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 4) == 4;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00012CE1 File Offset: 0x00011CE1
		internal bool IsChangeInResponseStatus
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 128) == 128;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00012CFB File Offset: 0x00011CFB
		internal bool IsChangeInUserPrincipal
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 8) == 8;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00012D0D File Offset: 0x00011D0D
		internal bool IsSendResponseHeaders
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 16) == 16;
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00012D24 File Offset: 0x00011D24
		internal void SetImpersonationEnabled()
		{
			IdentitySection identity = RuntimeConfig.GetConfig(this).Identity;
			this._impersonationEnabled = identity != null && identity.Impersonate;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00012D50 File Offset: 0x00011D50
		internal bool UsesImpersonation
		{
			get
			{
				return (HttpRuntime.IsOnUNCShareInternal && HostingEnvironment.ApplicationIdentityToken != IntPtr.Zero) || (this._impersonationEnabled && ((this._notificationContext.CurrentNotification == RequestNotification.AuthenticateRequest && this._notificationContext.IsPostNotification) || this._notificationContext.CurrentNotification > RequestNotification.AuthenticateRequest) && this._notificationContext.CurrentNotification != RequestNotification.SendResponse);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00012DC1 File Offset: 0x00011DC1
		internal bool AreResponseHeadersSent
		{
			get
			{
				return (this._notificationContext.CurrentNotificationFlags & 32) == 32;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00012DD8 File Offset: 0x00011DD8
		internal bool NeedToInitializeApp()
		{
			bool flag = !this._isAppInitialized;
			if (flag)
			{
				this._isAppInitialized = true;
			}
			return flag;
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00012DFA File Offset: 0x00011DFA
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00012E07 File Offset: 0x00011E07
		internal int CurrentNotificationFlags
		{
			get
			{
				return this._notificationContext.CurrentNotificationFlags;
			}
			set
			{
				this._notificationContext.CurrentNotificationFlags = value;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00012E15 File Offset: 0x00011E15
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00012E22 File Offset: 0x00011E22
		internal int CurrentModuleIndex
		{
			get
			{
				return this._notificationContext.CurrentModuleIndex;
			}
			set
			{
				this._notificationContext.CurrentModuleIndex = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00012E30 File Offset: 0x00011E30
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00012E3D File Offset: 0x00011E3D
		internal int CurrentModuleEventIndex
		{
			get
			{
				return this._notificationContext.CurrentModuleEventIndex;
			}
			set
			{
				this._notificationContext.CurrentModuleEventIndex = value;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00012E4C File Offset: 0x00011E4C
		internal void DisableNotifications(RequestNotification notifications, RequestNotification postNotifications)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				iis7WorkerRequest.DisableNotifications(notifications, postNotifications);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00012E70 File Offset: 0x00011E70
		internal void DisposePrincipal()
		{
			if (this._pManagedPrincipal == IntPtr.Zero && this._user != null && this._user != WindowsAuthenticationModule.AnonymousPrincipal)
			{
				WindowsIdentity windowsIdentity = this._user.Identity as WindowsIdentity;
				if (windowsIdentity != null)
				{
					this._user = null;
					windowsIdentity.Dispose();
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00012EC5 File Offset: 0x00011EC5
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00012EE9 File Offset: 0x00011EE9
		public bool IsPostNotification
		{
			get
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				return this._notificationContext.IsPostNotification;
			}
			internal set
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this._notificationContext.IsPostNotification = value;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00012F0E File Offset: 0x00011F0E
		internal IntPtr ClientIdentityToken
		{
			get
			{
				if (this._wr != null)
				{
					return this._wr.GetUserToken();
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00012F2C File Offset: 0x00011F2C
		internal bool IsClientImpersonationConfigured
		{
			get
			{
				bool flag;
				try
				{
					IdentitySection identity = RuntimeConfig.GetConfig(this).Identity;
					flag = identity != null && identity.Impersonate && identity.ImpersonateToken == IntPtr.Zero;
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00012F7C File Offset: 0x00011F7C
		internal IntPtr ImpersonationToken
		{
			get
			{
				IntPtr intPtr = HostingEnvironment.ApplicationIdentityToken;
				IdentitySection identity = RuntimeConfig.GetConfig(this).Identity;
				if (identity != null)
				{
					if (identity.Impersonate)
					{
						intPtr = ((identity.ImpersonateToken != IntPtr.Zero) ? identity.ImpersonateToken : this.ClientIdentityToken);
					}
					else if (!HttpRuntime.IsOnUNCShareInternal)
					{
						intPtr = IntPtr.Zero;
					}
				}
				return intPtr;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00012FD7 File Offset: 0x00011FD7
		internal AspNetSynchronizationContext SyncContext
		{
			get
			{
				if (this._syncContext == null)
				{
					this._syncContext = new AspNetSynchronizationContext(this.ApplicationInstance);
				}
				return this._syncContext;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00012FF8 File Offset: 0x00011FF8
		internal AspNetSynchronizationContext InstallNewAspNetSynchronizationContext()
		{
			AspNetSynchronizationContext syncContext = this._syncContext;
			if (syncContext != null && syncContext == AsyncOperationManager.SynchronizationContext)
			{
				this._syncContext = new AspNetSynchronizationContext(this.ApplicationInstance);
				AsyncOperationManager.SynchronizationContext = this._syncContext;
				return syncContext;
			}
			return null;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00013036 File Offset: 0x00012036
		internal void RestoreSavedAspNetSynchronizationContext(AspNetSynchronizationContext syncContext)
		{
			AsyncOperationManager.SynchronizationContext = syncContext;
			this._syncContext = syncContext;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00013048 File Offset: 0x00012048
		internal string UserLanguageFromContext()
		{
			if (this.Request != null && this.Request.UserLanguages != null)
			{
				string text = this.Request.UserLanguages[0];
				if (text != null)
				{
					int num = text.IndexOf(';');
					if (num != -1)
					{
						return text.Substring(0, num);
					}
					return text;
				}
			}
			return null;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00013094 File Offset: 0x00012094
		internal void ClearReferences()
		{
			this._appInstance = null;
			this._handler = null;
			this._handlerStack = null;
			this._currentHandler = null;
			if (this._isIntegratedPipeline)
			{
				this._items = null;
				this._syncContext = null;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000130C8 File Offset: 0x000120C8
		internal CultureInfo CultureFromConfig(string configString, bool requireSpecific)
		{
			if (StringUtil.EqualsIgnoreCase(configString, HttpApplication.AutoCulture))
			{
				string text = this.UserLanguageFromContext();
				if (text != null)
				{
					try
					{
						if (requireSpecific)
						{
							return HttpServerUtility.CreateReadOnlySpecificCultureInfo(text);
						}
						return HttpServerUtility.CreateReadOnlyCultureInfo(text);
					}
					catch
					{
						return null;
					}
				}
				return null;
			}
			if (StringUtil.StringStartsWithIgnoreCase(configString, "auto:"))
			{
				string text2 = this.UserLanguageFromContext();
				if (text2 != null)
				{
					try
					{
						if (requireSpecific)
						{
							return HttpServerUtility.CreateReadOnlySpecificCultureInfo(text2);
						}
						return HttpServerUtility.CreateReadOnlyCultureInfo(text2);
					}
					catch
					{
						if (requireSpecific)
						{
							return HttpServerUtility.CreateReadOnlySpecificCultureInfo(HttpApplication.GetFallbackCulture(configString));
						}
						return HttpServerUtility.CreateReadOnlyCultureInfo(HttpApplication.GetFallbackCulture(configString));
					}
				}
				if (requireSpecific)
				{
					return HttpServerUtility.CreateReadOnlySpecificCultureInfo(configString.Substring(5));
				}
				return HttpServerUtility.CreateReadOnlyCultureInfo(configString.Substring(5));
			}
			else
			{
				if (requireSpecific)
				{
					return HttpServerUtility.CreateReadOnlySpecificCultureInfo(configString);
				}
				return HttpServerUtility.CreateReadOnlyCultureInfo(configString);
			}
			CultureInfo cultureInfo;
			return cultureInfo;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000131A8 File Offset: 0x000121A8
		private void AbortConnection()
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				iis7WorkerRequest.AbortConnection();
				return;
			}
			this._wr.CloseConnection();
		}

		// Token: 0x04000FDD RID: 4061
		private const int FLAG_NONE = 0;

		// Token: 0x04000FDE RID: 4062
		private const int FLAG_CHANGE_IN_SERVER_VARIABLES = 1;

		// Token: 0x04000FDF RID: 4063
		private const int FLAG_CHANGE_IN_REQUEST_HEADERS = 2;

		// Token: 0x04000FE0 RID: 4064
		private const int FLAG_CHANGE_IN_RESPONSE_HEADERS = 4;

		// Token: 0x04000FE1 RID: 4065
		private const int FLAG_CHANGE_IN_USER_OBJECT = 8;

		// Token: 0x04000FE2 RID: 4066
		private const int FLAG_SEND_RESPONSE_HEADERS = 16;

		// Token: 0x04000FE3 RID: 4067
		private const int FLAG_RESPONSE_HEADERS_SENT = 32;

		// Token: 0x04000FE4 RID: 4068
		internal const int FLAG_ETW_PROVIDER_ENABLED = 64;

		// Token: 0x04000FE5 RID: 4069
		private const int FLAG_CHANGE_IN_RESPONSE_STATUS = 128;

		// Token: 0x04000FE6 RID: 4070
		internal static readonly Assembly SystemWebAssembly = typeof(HttpContext).Assembly;

		// Token: 0x04000FE7 RID: 4071
		private IHttpAsyncHandler _asyncAppHandler;

		// Token: 0x04000FE8 RID: 4072
		private HttpApplication _appInstance;

		// Token: 0x04000FE9 RID: 4073
		private IHttpHandler _handler;

		// Token: 0x04000FEA RID: 4074
		private HttpRequest _request;

		// Token: 0x04000FEB RID: 4075
		private HttpResponse _response;

		// Token: 0x04000FEC RID: 4076
		private HttpServerUtility _server;

		// Token: 0x04000FED RID: 4077
		private Stack _traceContextStack;

		// Token: 0x04000FEE RID: 4078
		private TraceContext _topTraceContext;

		// Token: 0x04000FEF RID: 4079
		private Hashtable _items;

		// Token: 0x04000FF0 RID: 4080
		private ArrayList _errors;

		// Token: 0x04000FF1 RID: 4081
		private Exception _tempError;

		// Token: 0x04000FF2 RID: 4082
		private bool _errorCleared;

		// Token: 0x04000FF3 RID: 4083
		private IPrincipal _user;

		// Token: 0x04000FF4 RID: 4084
		private IntPtr _pManagedPrincipal;

		// Token: 0x04000FF5 RID: 4085
		internal ProfileBase _Profile;

		// Token: 0x04000FF6 RID: 4086
		private DateTime _utcTimestamp;

		// Token: 0x04000FF7 RID: 4087
		private HttpWorkerRequest _wr;

		// Token: 0x04000FF8 RID: 4088
		private GCHandle _root;

		// Token: 0x04000FF9 RID: 4089
		private IntPtr _ctxPtr;

		// Token: 0x04000FFA RID: 4090
		private VirtualPath _configurationPath;

		// Token: 0x04000FFB RID: 4091
		internal bool _skipAuthorization;

		// Token: 0x04000FFC RID: 4092
		private CultureInfo _dynamicCulture;

		// Token: 0x04000FFD RID: 4093
		private CultureInfo _dynamicUICulture;

		// Token: 0x04000FFE RID: 4094
		private int _serverExecuteDepth;

		// Token: 0x04000FFF RID: 4095
		private Stack _handlerStack;

		// Token: 0x04001000 RID: 4096
		private bool _preventPostback;

		// Token: 0x04001001 RID: 4097
		private bool _runtimeErrorReported;

		// Token: 0x04001002 RID: 4098
		private bool _firstNotificationInitCalled;

		// Token: 0x04001003 RID: 4099
		private DateTime _timeoutStartTime = DateTime.MinValue;

		// Token: 0x04001004 RID: 4100
		private bool _timeoutSet;

		// Token: 0x04001005 RID: 4101
		private TimeSpan _timeout;

		// Token: 0x04001006 RID: 4102
		private int _timeoutState;

		// Token: 0x04001007 RID: 4103
		private DoubleLink _timeoutLink;

		// Token: 0x04001008 RID: 4104
		private Thread _thread;

		// Token: 0x04001009 RID: 4105
		private CachedPathData _configurationPathData;

		// Token: 0x0400100A RID: 4106
		private CachedPathData _filePathData;

		// Token: 0x0400100B RID: 4107
		private string _sqlDependencyCookie;

		// Token: 0x0400100C RID: 4108
		private SessionStateModule _sessionStateModule;

		// Token: 0x0400100D RID: 4109
		private TemplateControl _templateControl;

		// Token: 0x0400100E RID: 4110
		private NotificationContext _notificationContext;

		// Token: 0x0400100F RID: 4111
		private bool _isAppInitialized;

		// Token: 0x04001010 RID: 4112
		private bool _isIntegratedPipeline;

		// Token: 0x04001011 RID: 4113
		private bool _finishPipelineRequestCalled;

		// Token: 0x04001012 RID: 4114
		private bool _impersonationEnabled;

		// Token: 0x04001013 RID: 4115
		internal bool HideRequestResponse;

		// Token: 0x04001014 RID: 4116
		internal volatile bool InIndicateCompletion;

		// Token: 0x04001015 RID: 4117
		internal volatile HttpApplication.ThreadContext IndicateCompletionContext;

		// Token: 0x04001016 RID: 4118
		private AspNetSynchronizationContext _syncContext;

		// Token: 0x04001017 RID: 4119
		internal bool RequiresSessionState;

		// Token: 0x04001018 RID: 4120
		internal bool ReadOnlySessionState;

		// Token: 0x04001019 RID: 4121
		internal bool InAspCompatMode;

		// Token: 0x0400101A RID: 4122
		private IHttpHandler _remapHandler;

		// Token: 0x0400101B RID: 4123
		private IHttpHandler _currentHandler;

		// Token: 0x0400101C RID: 4124
		internal bool _ProfileDelayLoad;

		// Token: 0x0400101D RID: 4125
		private CookielessHelperClass _CookielessHelper;
	}
}
