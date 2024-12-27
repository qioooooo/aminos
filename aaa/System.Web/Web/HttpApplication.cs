using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web.Configuration;
using System.Web.Configuration.Common;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200003A RID: 58
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpApplication : IHttpAsyncHandler, IHttpHandler, IComponent, IDisposable
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007A2A File Offset: 0x00006A2A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpContext Context
		{
			get
			{
				if (this._context == null)
				{
					return this._initContext;
				}
				return this._context;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007A41 File Offset: 0x00006A41
		private bool IsContainerInitalizationAllowed
		{
			get
			{
				return HttpRuntime.UseIntegratedPipeline && HttpApplication._initSpecialCompleted && !this._initInternalCompleted;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007A5C File Offset: 0x00006A5C
		private void ThrowIfEventBindingDisallowed()
		{
			if (HttpRuntime.UseIntegratedPipeline && HttpApplication._initSpecialCompleted && this._initInternalCompleted)
			{
				throw new InvalidOperationException(SR.GetString("Event_Binding_Disallowed"));
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007A84 File Offset: 0x00006A84
		private PipelineModuleStepContainer[] ModuleContainers
		{
			get
			{
				if (this._moduleContainers == null)
				{
					this._moduleContainers = new PipelineModuleStepContainer[HttpApplication._moduleIndexMap.Count];
					for (int i = 0; i < this._moduleContainers.Length; i++)
					{
						this._moduleContainers[i] = new PipelineModuleStepContainer();
					}
				}
				return this._moduleContainers;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000147 RID: 327 RVA: 0x00007AD4 File Offset: 0x00006AD4
		// (remove) Token: 0x06000148 RID: 328 RVA: 0x00007AE7 File Offset: 0x00006AE7
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(HttpApplication.EventDisposed, value);
			}
			remove
			{
				this.Events.RemoveHandler(HttpApplication.EventDisposed, value);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00007AFA File Offset: 0x00006AFA
		protected EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007B15 File Offset: 0x00006B15
		private HttpApplication.AsyncAppEventHandlersTable AsyncEvents
		{
			get
			{
				if (this._asyncEvents == null)
				{
					this._asyncEvents = new HttpApplication.AsyncAppEventHandlersTable();
				}
				return this._asyncEvents;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00007B30 File Offset: 0x00006B30
		internal Exception LastError
		{
			get
			{
				if (this._context == null)
				{
					return this._lastError;
				}
				return this._context.Error;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007B4C File Offset: 0x00006B4C
		internal byte[] EntityBuffer
		{
			get
			{
				if (this._entityBuffer == null)
				{
					this._entityBuffer = new byte[8192];
				}
				return this._entityBuffer;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007B6C File Offset: 0x00006B6C
		internal void ClearError()
		{
			this._lastError = null;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007B78 File Offset: 0x00006B78
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpRequest Request
		{
			get
			{
				HttpRequest httpRequest = null;
				if (this._context != null && !this._hideRequestResponse)
				{
					httpRequest = this._context.Request;
				}
				if (httpRequest == null)
				{
					throw new HttpException(SR.GetString("Request_not_available"));
				}
				return httpRequest;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007BB8 File Offset: 0x00006BB8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpResponse Response
		{
			get
			{
				HttpResponse httpResponse = null;
				if (this._context != null && !this._hideRequestResponse)
				{
					httpResponse = this._context.Response;
				}
				if (httpResponse == null)
				{
					throw new HttpException(SR.GetString("Response_not_available"));
				}
				return httpResponse;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007BF8 File Offset: 0x00006BF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpSessionState Session
		{
			get
			{
				HttpSessionState httpSessionState = null;
				if (this._session != null)
				{
					httpSessionState = this._session;
				}
				else if (this._context != null)
				{
					httpSessionState = this._context.Session;
				}
				if (httpSessionState == null)
				{
					throw new HttpException(SR.GetString("Session_not_available"));
				}
				return httpSessionState;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007C40 File Offset: 0x00006C40
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpApplicationState Application
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007C48 File Offset: 0x00006C48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpServerUtility Server
		{
			get
			{
				if (this._context != null)
				{
					return this._context.Server;
				}
				return new HttpServerUtility(this);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007C64 File Offset: 0x00006C64
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPrincipal User
		{
			get
			{
				if (this._context == null)
				{
					throw new HttpException(SR.GetString("User_not_available"));
				}
				return this._context.User;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00007C89 File Offset: 0x00006C89
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpModuleCollection Modules
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
			get
			{
				if (this._moduleCollection == null)
				{
					this._moduleCollection = new HttpModuleCollection();
				}
				return this._moduleCollection;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00007CA4 File Offset: 0x00006CA4
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00007CBF File Offset: 0x00006CBF
		internal EventArgs AppEvent
		{
			get
			{
				if (this._appEvent == null)
				{
					this._appEvent = EventArgs.Empty;
				}
				return this._appEvent;
			}
			set
			{
				this._appEvent = null;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007CC8 File Offset: 0x00006CC8
		internal void EnsureReleaseState()
		{
			if (this._moduleCollection != null)
			{
				for (int i = 0; i < this._moduleCollection.Count; i++)
				{
					IHttpModule httpModule = this._moduleCollection.Get(i);
					if (httpModule is SessionStateModule)
					{
						((SessionStateModule)httpModule).EnsureReleaseState(this);
						return;
					}
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007D15 File Offset: 0x00006D15
		public void CompleteRequest()
		{
			this._stepManager.CompleteRequest();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00007D22 File Offset: 0x00006D22
		internal bool IsRequestCompleted
		{
			get
			{
				return this._stepManager != null && this._stepManager.IsCompleted;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007D3C File Offset: 0x00006D3C
		internal void AcquireNotifcationContextLock(ref bool locked)
		{
			try
			{
			}
			finally
			{
				Monitor.Enter(this._stepManager);
				locked = true;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007D6C File Offset: 0x00006D6C
		internal void ReleaseNotifcationContextLock()
		{
			Monitor.Exit(this._stepManager);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007D7C File Offset: 0x00006D7C
		private void RaiseOnError()
		{
			EventHandler eventHandler = (EventHandler)this.Events[HttpApplication.EventErrorRecorded];
			if (eventHandler != null)
			{
				try
				{
					eventHandler(this, this.AppEvent);
				}
				catch (Exception ex)
				{
					if (this._context != null)
					{
						this._context.AddError(ex);
					}
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007DD8 File Offset: 0x00006DD8
		internal void RaiseOnPreSendRequestHeaders()
		{
			EventHandler eventHandler = (EventHandler)this.Events[HttpApplication.EventPreSendRequestHeaders];
			if (eventHandler != null)
			{
				try
				{
					eventHandler(this, this.AppEvent);
				}
				catch (Exception ex)
				{
					this.RecordError(ex);
				}
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007E28 File Offset: 0x00006E28
		internal void RaiseOnPreSendRequestContent()
		{
			EventHandler eventHandler = (EventHandler)this.Events[HttpApplication.EventPreSendRequestContent];
			if (eventHandler != null)
			{
				try
				{
					eventHandler(this, this.AppEvent);
				}
				catch (Exception ex)
				{
					this.RecordError(ex);
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007E78 File Offset: 0x00006E78
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00007EA7 File Offset: 0x00006EA7
		internal HttpAsyncResult AsyncResult
		{
			get
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					return this._ar;
				}
				if (this._context.NotificationContext == null)
				{
					return null;
				}
				return this._context.NotificationContext.AsyncResult;
			}
			set
			{
				if (HttpRuntime.UseIntegratedPipeline)
				{
					this._context.NotificationContext.AsyncResult = value;
					return;
				}
				this._ar = value;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007EC9 File Offset: 0x00006EC9
		internal void AddSyncEventHookup(object key, Delegate handler, RequestNotification notification)
		{
			this.AddSyncEventHookup(key, handler, notification, false);
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007ED5 File Offset: 0x00006ED5
		private PipelineModuleStepContainer CurrentModuleContainer
		{
			get
			{
				return this.ModuleContainers[this._context.CurrentModuleIndex];
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007EEC File Offset: 0x00006EEC
		private PipelineModuleStepContainer GetModuleContainer(string moduleName)
		{
			object obj = HttpApplication._moduleIndexMap[moduleName];
			if (obj == null)
			{
				return null;
			}
			int num = (int)obj;
			return this.ModuleContainers[num];
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007F1C File Offset: 0x00006F1C
		private void AddSyncEventHookup(object key, Delegate handler, RequestNotification notification, bool isPostNotification)
		{
			this.ThrowIfEventBindingDisallowed();
			this.Events.AddHandler(key, handler);
			if (this.IsContainerInitalizationAllowed)
			{
				PipelineModuleStepContainer moduleContainer = this.GetModuleContainer(this.CurrentModuleCollectionKey);
				if (moduleContainer != null)
				{
					HttpApplication.SyncEventExecutionStep syncEventExecutionStep = new HttpApplication.SyncEventExecutionStep(this, (EventHandler)handler);
					moduleContainer.AddEvent(notification, isPostNotification, syncEventExecutionStep);
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007F6B File Offset: 0x00006F6B
		internal void RemoveSyncEventHookup(object key, Delegate handler, RequestNotification notification)
		{
			this.RemoveSyncEventHookup(key, handler, notification, false);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007F78 File Offset: 0x00006F78
		internal void RemoveSyncEventHookup(object key, Delegate handler, RequestNotification notification, bool isPostNotification)
		{
			this.ThrowIfEventBindingDisallowed();
			this.Events.RemoveHandler(key, handler);
			if (this.IsContainerInitalizationAllowed)
			{
				PipelineModuleStepContainer moduleContainer = this.GetModuleContainer(this.CurrentModuleCollectionKey);
				if (moduleContainer != null)
				{
					moduleContainer.RemoveEvent(notification, isPostNotification, handler);
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007FBC File Offset: 0x00006FBC
		private void AddSendResponseEventHookup(object key, Delegate handler)
		{
			this.ThrowIfEventBindingDisallowed();
			this.Events.AddHandler(key, handler);
			if (this.IsContainerInitalizationAllowed)
			{
				PipelineModuleStepContainer moduleContainer = this.GetModuleContainer(this.CurrentModuleCollectionKey);
				if (moduleContainer != null)
				{
					bool flag = key == HttpApplication.EventPreSendRequestHeaders;
					HttpApplication.SendResponseExecutionStep sendResponseExecutionStep = new HttpApplication.SendResponseExecutionStep(this, (EventHandler)handler, flag);
					moduleContainer.AddEvent(RequestNotification.SendResponse, false, sendResponseExecutionStep);
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008018 File Offset: 0x00007018
		private void RemoveSendResponseEventHookup(object key, Delegate handler)
		{
			this.ThrowIfEventBindingDisallowed();
			this.Events.RemoveHandler(key, handler);
			if (this.IsContainerInitalizationAllowed)
			{
				PipelineModuleStepContainer moduleContainer = this.GetModuleContainer(this.CurrentModuleCollectionKey);
				if (moduleContainer != null)
				{
					moduleContainer.RemoveEvent(RequestNotification.SendResponse, false, handler);
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000169 RID: 361 RVA: 0x0000805D File Offset: 0x0000705D
		// (remove) Token: 0x0600016A RID: 362 RVA: 0x0000806C File Offset: 0x0000706C
		public event EventHandler BeginRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventBeginRequest, value, RequestNotification.BeginRequest);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventBeginRequest, value, RequestNotification.BeginRequest);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600016B RID: 363 RVA: 0x0000807B File Offset: 0x0000707B
		// (remove) Token: 0x0600016C RID: 364 RVA: 0x0000808A File Offset: 0x0000708A
		public event EventHandler AuthenticateRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventAuthenticateRequest, value, RequestNotification.AuthenticateRequest);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventAuthenticateRequest, value, RequestNotification.AuthenticateRequest);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600016D RID: 365 RVA: 0x00008099 File Offset: 0x00007099
		// (remove) Token: 0x0600016E RID: 366 RVA: 0x000080A8 File Offset: 0x000070A8
		internal event EventHandler DefaultAuthentication
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventDefaultAuthentication, value, RequestNotification.AuthenticateRequest);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventDefaultAuthentication, value, RequestNotification.AuthenticateRequest);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600016F RID: 367 RVA: 0x000080B7 File Offset: 0x000070B7
		// (remove) Token: 0x06000170 RID: 368 RVA: 0x000080C7 File Offset: 0x000070C7
		public event EventHandler PostAuthenticateRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostAuthenticateRequest, value, RequestNotification.AuthenticateRequest, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostAuthenticateRequest, value, RequestNotification.AuthenticateRequest, true);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000171 RID: 369 RVA: 0x000080D7 File Offset: 0x000070D7
		// (remove) Token: 0x06000172 RID: 370 RVA: 0x000080E6 File Offset: 0x000070E6
		public event EventHandler AuthorizeRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventAuthorizeRequest, value, RequestNotification.AuthorizeRequest);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventAuthorizeRequest, value, RequestNotification.AuthorizeRequest);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000173 RID: 371 RVA: 0x000080F5 File Offset: 0x000070F5
		// (remove) Token: 0x06000174 RID: 372 RVA: 0x00008105 File Offset: 0x00007105
		public event EventHandler PostAuthorizeRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostAuthorizeRequest, value, RequestNotification.AuthorizeRequest, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostAuthorizeRequest, value, RequestNotification.AuthorizeRequest, true);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000175 RID: 373 RVA: 0x00008115 File Offset: 0x00007115
		// (remove) Token: 0x06000176 RID: 374 RVA: 0x00008124 File Offset: 0x00007124
		public event EventHandler ResolveRequestCache
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventResolveRequestCache, value, RequestNotification.ResolveRequestCache);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventResolveRequestCache, value, RequestNotification.ResolveRequestCache);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000177 RID: 375 RVA: 0x00008133 File Offset: 0x00007133
		// (remove) Token: 0x06000178 RID: 376 RVA: 0x00008143 File Offset: 0x00007143
		public event EventHandler PostResolveRequestCache
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostResolveRequestCache, value, RequestNotification.ResolveRequestCache, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostResolveRequestCache, value, RequestNotification.ResolveRequestCache, true);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000179 RID: 377 RVA: 0x00008153 File Offset: 0x00007153
		// (remove) Token: 0x0600017A RID: 378 RVA: 0x0000817A File Offset: 0x0000717A
		public event EventHandler MapRequestHandler
		{
			add
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.AddSyncEventHookup(HttpApplication.EventMapRequestHandler, value, RequestNotification.MapRequestHandler);
			}
			remove
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.RemoveSyncEventHookup(HttpApplication.EventMapRequestHandler, value, RequestNotification.MapRequestHandler);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600017B RID: 379 RVA: 0x000081A1 File Offset: 0x000071A1
		// (remove) Token: 0x0600017C RID: 380 RVA: 0x000081B2 File Offset: 0x000071B2
		public event EventHandler PostMapRequestHandler
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostMapRequestHandler, value, RequestNotification.MapRequestHandler, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostMapRequestHandler, value, RequestNotification.MapRequestHandler);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600017D RID: 381 RVA: 0x000081C2 File Offset: 0x000071C2
		// (remove) Token: 0x0600017E RID: 382 RVA: 0x000081D2 File Offset: 0x000071D2
		public event EventHandler AcquireRequestState
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventAcquireRequestState, value, RequestNotification.AcquireRequestState);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventAcquireRequestState, value, RequestNotification.AcquireRequestState);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600017F RID: 383 RVA: 0x000081E2 File Offset: 0x000071E2
		// (remove) Token: 0x06000180 RID: 384 RVA: 0x000081F3 File Offset: 0x000071F3
		public event EventHandler PostAcquireRequestState
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostAcquireRequestState, value, RequestNotification.AcquireRequestState, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostAcquireRequestState, value, RequestNotification.AcquireRequestState, true);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000181 RID: 385 RVA: 0x00008204 File Offset: 0x00007204
		// (remove) Token: 0x06000182 RID: 386 RVA: 0x00008214 File Offset: 0x00007214
		public event EventHandler PreRequestHandlerExecute
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPreRequestHandlerExecute, value, RequestNotification.PreExecuteRequestHandler);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPreRequestHandlerExecute, value, RequestNotification.PreExecuteRequestHandler);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000183 RID: 387 RVA: 0x00008224 File Offset: 0x00007224
		// (remove) Token: 0x06000184 RID: 388 RVA: 0x00008238 File Offset: 0x00007238
		public event EventHandler PostRequestHandlerExecute
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostRequestHandlerExecute, value, RequestNotification.ExecuteRequestHandler, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostRequestHandlerExecute, value, RequestNotification.ExecuteRequestHandler, true);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000185 RID: 389 RVA: 0x0000824C File Offset: 0x0000724C
		// (remove) Token: 0x06000186 RID: 390 RVA: 0x0000825F File Offset: 0x0000725F
		public event EventHandler ReleaseRequestState
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventReleaseRequestState, value, RequestNotification.ReleaseRequestState);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventReleaseRequestState, value, RequestNotification.ReleaseRequestState);
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000187 RID: 391 RVA: 0x00008272 File Offset: 0x00007272
		// (remove) Token: 0x06000188 RID: 392 RVA: 0x00008286 File Offset: 0x00007286
		public event EventHandler PostReleaseRequestState
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostReleaseRequestState, value, RequestNotification.ReleaseRequestState, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostReleaseRequestState, value, RequestNotification.ReleaseRequestState, true);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000189 RID: 393 RVA: 0x0000829A File Offset: 0x0000729A
		// (remove) Token: 0x0600018A RID: 394 RVA: 0x000082AD File Offset: 0x000072AD
		public event EventHandler UpdateRequestCache
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventUpdateRequestCache, value, RequestNotification.UpdateRequestCache);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventUpdateRequestCache, value, RequestNotification.UpdateRequestCache);
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600018B RID: 395 RVA: 0x000082C0 File Offset: 0x000072C0
		// (remove) Token: 0x0600018C RID: 396 RVA: 0x000082D4 File Offset: 0x000072D4
		public event EventHandler PostUpdateRequestCache
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventPostUpdateRequestCache, value, RequestNotification.UpdateRequestCache, true);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventPostUpdateRequestCache, value, RequestNotification.UpdateRequestCache, true);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600018D RID: 397 RVA: 0x000082E8 File Offset: 0x000072E8
		// (remove) Token: 0x0600018E RID: 398 RVA: 0x00008312 File Offset: 0x00007312
		public event EventHandler LogRequest
		{
			add
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.AddSyncEventHookup(HttpApplication.EventLogRequest, value, RequestNotification.LogRequest);
			}
			remove
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.RemoveSyncEventHookup(HttpApplication.EventLogRequest, value, RequestNotification.LogRequest);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600018F RID: 399 RVA: 0x0000833C File Offset: 0x0000733C
		// (remove) Token: 0x06000190 RID: 400 RVA: 0x00008367 File Offset: 0x00007367
		public event EventHandler PostLogRequest
		{
			add
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.AddSyncEventHookup(HttpApplication.EventPostLogRequest, value, RequestNotification.LogRequest, true);
			}
			remove
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				this.RemoveSyncEventHookup(HttpApplication.EventPostLogRequest, value, RequestNotification.LogRequest, true);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000191 RID: 401 RVA: 0x00008392 File Offset: 0x00007392
		// (remove) Token: 0x06000192 RID: 402 RVA: 0x000083A5 File Offset: 0x000073A5
		public event EventHandler EndRequest
		{
			add
			{
				this.AddSyncEventHookup(HttpApplication.EventEndRequest, value, RequestNotification.EndRequest);
			}
			remove
			{
				this.RemoveSyncEventHookup(HttpApplication.EventEndRequest, value, RequestNotification.EndRequest);
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000193 RID: 403 RVA: 0x000083B8 File Offset: 0x000073B8
		// (remove) Token: 0x06000194 RID: 404 RVA: 0x000083CB File Offset: 0x000073CB
		public event EventHandler Error
		{
			add
			{
				this.Events.AddHandler(HttpApplication.EventErrorRecorded, value);
			}
			remove
			{
				this.Events.RemoveHandler(HttpApplication.EventErrorRecorded, value);
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000195 RID: 405 RVA: 0x000083DE File Offset: 0x000073DE
		// (remove) Token: 0x06000196 RID: 406 RVA: 0x000083EC File Offset: 0x000073EC
		public event EventHandler PreSendRequestHeaders
		{
			add
			{
				this.AddSendResponseEventHookup(HttpApplication.EventPreSendRequestHeaders, value);
			}
			remove
			{
				this.RemoveSendResponseEventHookup(HttpApplication.EventPreSendRequestHeaders, value);
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000197 RID: 407 RVA: 0x000083FA File Offset: 0x000073FA
		// (remove) Token: 0x06000198 RID: 408 RVA: 0x00008408 File Offset: 0x00007408
		public event EventHandler PreSendRequestContent
		{
			add
			{
				this.AddSendResponseEventHookup(HttpApplication.EventPreSendRequestContent, value);
			}
			remove
			{
				this.RemoveSendResponseEventHookup(HttpApplication.EventPreSendRequestContent, value);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008416 File Offset: 0x00007416
		public void AddOnBeginRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnBeginRequestAsync(bh, eh, null);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008421 File Offset: 0x00007421
		public void AddOnBeginRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventBeginRequest, beginHandler, endHandler, state, RequestNotification.BeginRequest, false, this);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008439 File Offset: 0x00007439
		public void AddOnAuthenticateRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnAuthenticateRequestAsync(bh, eh, null);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008444 File Offset: 0x00007444
		public void AddOnAuthenticateRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventAuthenticateRequest, beginHandler, endHandler, state, RequestNotification.AuthenticateRequest, false, this);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000845C File Offset: 0x0000745C
		public void AddOnPostAuthenticateRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostAuthenticateRequestAsync(bh, eh, null);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008467 File Offset: 0x00007467
		public void AddOnPostAuthenticateRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostAuthenticateRequest, beginHandler, endHandler, state, RequestNotification.AuthenticateRequest, true, this);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000847F File Offset: 0x0000747F
		public void AddOnAuthorizeRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnAuthorizeRequestAsync(bh, eh, null);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000848A File Offset: 0x0000748A
		public void AddOnAuthorizeRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventAuthorizeRequest, beginHandler, endHandler, state, RequestNotification.AuthorizeRequest, false, this);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000084A2 File Offset: 0x000074A2
		public void AddOnPostAuthorizeRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostAuthorizeRequestAsync(bh, eh, null);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000084AD File Offset: 0x000074AD
		public void AddOnPostAuthorizeRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostAuthorizeRequest, beginHandler, endHandler, state, RequestNotification.AuthorizeRequest, true, this);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000084C5 File Offset: 0x000074C5
		public void AddOnResolveRequestCacheAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnResolveRequestCacheAsync(bh, eh, null);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000084D0 File Offset: 0x000074D0
		public void AddOnResolveRequestCacheAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventResolveRequestCache, beginHandler, endHandler, state, RequestNotification.ResolveRequestCache, false, this);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000084E8 File Offset: 0x000074E8
		public void AddOnPostResolveRequestCacheAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostResolveRequestCacheAsync(bh, eh, null);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000084F3 File Offset: 0x000074F3
		public void AddOnPostResolveRequestCacheAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostResolveRequestCache, beginHandler, endHandler, state, RequestNotification.ResolveRequestCache, true, this);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000850B File Offset: 0x0000750B
		public void AddOnMapRequestHandlerAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AddOnMapRequestHandlerAsync(bh, eh, null);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000852D File Offset: 0x0000752D
		public void AddOnMapRequestHandlerAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AsyncEvents.AddHandler(HttpApplication.EventMapRequestHandler, beginHandler, endHandler, state, RequestNotification.MapRequestHandler, false, this);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000855D File Offset: 0x0000755D
		public void AddOnPostMapRequestHandlerAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostMapRequestHandlerAsync(bh, eh, null);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008568 File Offset: 0x00007568
		public void AddOnPostMapRequestHandlerAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostMapRequestHandler, beginHandler, endHandler, state, RequestNotification.MapRequestHandler, true, this);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008581 File Offset: 0x00007581
		public void AddOnAcquireRequestStateAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnAcquireRequestStateAsync(bh, eh, null);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000858C File Offset: 0x0000758C
		public void AddOnAcquireRequestStateAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventAcquireRequestState, beginHandler, endHandler, state, RequestNotification.AcquireRequestState, false, this);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000085A5 File Offset: 0x000075A5
		public void AddOnPostAcquireRequestStateAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostAcquireRequestStateAsync(bh, eh, null);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000085B0 File Offset: 0x000075B0
		public void AddOnPostAcquireRequestStateAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostAcquireRequestState, beginHandler, endHandler, state, RequestNotification.AcquireRequestState, true, this);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000085C9 File Offset: 0x000075C9
		public void AddOnPreRequestHandlerExecuteAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPreRequestHandlerExecuteAsync(bh, eh, null);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000085D4 File Offset: 0x000075D4
		public void AddOnPreRequestHandlerExecuteAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPreRequestHandlerExecute, beginHandler, endHandler, state, RequestNotification.PreExecuteRequestHandler, false, this);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000085ED File Offset: 0x000075ED
		public void AddOnPostRequestHandlerExecuteAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostRequestHandlerExecuteAsync(bh, eh, null);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000085F8 File Offset: 0x000075F8
		public void AddOnPostRequestHandlerExecuteAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostRequestHandlerExecute, beginHandler, endHandler, state, RequestNotification.ExecuteRequestHandler, true, this);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008614 File Offset: 0x00007614
		public void AddOnReleaseRequestStateAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnReleaseRequestStateAsync(bh, eh, null);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000861F File Offset: 0x0000761F
		public void AddOnReleaseRequestStateAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventReleaseRequestState, beginHandler, endHandler, state, RequestNotification.ReleaseRequestState, false, this);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000863B File Offset: 0x0000763B
		public void AddOnPostReleaseRequestStateAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostReleaseRequestStateAsync(bh, eh, null);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008646 File Offset: 0x00007646
		public void AddOnPostReleaseRequestStateAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostReleaseRequestState, beginHandler, endHandler, state, RequestNotification.ReleaseRequestState, true, this);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008662 File Offset: 0x00007662
		public void AddOnUpdateRequestCacheAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnUpdateRequestCacheAsync(bh, eh, null);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000866D File Offset: 0x0000766D
		public void AddOnUpdateRequestCacheAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventUpdateRequestCache, beginHandler, endHandler, state, RequestNotification.UpdateRequestCache, false, this);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008689 File Offset: 0x00007689
		public void AddOnPostUpdateRequestCacheAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnPostUpdateRequestCacheAsync(bh, eh, null);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008694 File Offset: 0x00007694
		public void AddOnPostUpdateRequestCacheAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventPostUpdateRequestCache, beginHandler, endHandler, state, RequestNotification.UpdateRequestCache, true, this);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000086B0 File Offset: 0x000076B0
		public void AddOnLogRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AddOnLogRequestAsync(bh, eh, null);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000086D2 File Offset: 0x000076D2
		public void AddOnLogRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AsyncEvents.AddHandler(HttpApplication.EventLogRequest, beginHandler, endHandler, state, RequestNotification.LogRequest, false, this);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008705 File Offset: 0x00007705
		public void AddOnPostLogRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AddOnPostLogRequestAsync(bh, eh, null);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008727 File Offset: 0x00007727
		public void AddOnPostLogRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			if (!HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
			}
			this.AsyncEvents.AddHandler(HttpApplication.EventPostLogRequest, beginHandler, endHandler, state, RequestNotification.LogRequest, true, this);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000875A File Offset: 0x0000775A
		public void AddOnEndRequestAsync(BeginEventHandler bh, EndEventHandler eh)
		{
			this.AddOnEndRequestAsync(bh, eh, null);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008765 File Offset: 0x00007765
		public void AddOnEndRequestAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			this.AsyncEvents.AddHandler(HttpApplication.EventEndRequest, beginHandler, endHandler, state, RequestNotification.EndRequest, false, this);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008781 File Offset: 0x00007781
		public virtual void Init()
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008784 File Offset: 0x00007784
		public virtual void Dispose()
		{
			this._site = null;
			if (this._events != null)
			{
				try
				{
					EventHandler eventHandler = (EventHandler)this._events[HttpApplication.EventDisposed];
					if (eventHandler != null)
					{
						eventHandler(this, EventArgs.Empty);
					}
				}
				finally
				{
					this._events.Dispose();
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000087E4 File Offset: 0x000077E4
		private HttpHandlerAction GetHandlerMapping(HttpContext context, string requestType, VirtualPath path, bool useAppConfig)
		{
			CachedPathData cachedPathData = null;
			HandlerMappingMemo handlerMappingMemo = null;
			if (!useAppConfig)
			{
				cachedPathData = context.GetPathData(path);
				handlerMappingMemo = cachedPathData.CachedHandler;
				if (handlerMappingMemo != null && !handlerMappingMemo.IsMatch(requestType, path))
				{
					handlerMappingMemo = null;
				}
			}
			HttpHandlerAction httpHandlerAction;
			if (handlerMappingMemo == null)
			{
				HttpHandlersSection httpHandlersSection = (useAppConfig ? RuntimeConfig.GetAppConfig().HttpHandlers : RuntimeConfig.GetConfig(context).HttpHandlers);
				httpHandlerAction = httpHandlersSection.FindMapping(requestType, path);
				if (!useAppConfig)
				{
					handlerMappingMemo = new HandlerMappingMemo(httpHandlerAction, requestType, path);
					cachedPathData.CachedHandler = handlerMappingMemo;
				}
			}
			else
			{
				httpHandlerAction = handlerMappingMemo.Mapping;
			}
			return httpHandlerAction;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008860 File Offset: 0x00007860
		internal IHttpHandler MapIntegratedHttpHandler(HttpContext context, string requestType, VirtualPath path, string pathTranslated, bool useAppConfig, bool convertNativeStaticFileModule)
		{
			IHttpHandler httpHandler = null;
			using (new ApplicationImpersonationContext())
			{
				string text = path.VirtualPathString;
				if (useAppConfig)
				{
					int num = text.LastIndexOf('/');
					num++;
					if (num != 0 && num < text.Length)
					{
						text = UrlPath.SimpleCombine(HttpRuntime.AppDomainAppVirtualPathString, text.Substring(num));
					}
					else
					{
						text = HttpRuntime.AppDomainAppVirtualPathString;
					}
				}
				IIS7WorkerRequest iis7WorkerRequest = context.WorkerRequest as IIS7WorkerRequest;
				string text2 = iis7WorkerRequest.MapHandlerAndGetHandlerTypeString(requestType, text, convertNativeStaticFileModule, false);
				if (text2 == null)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_FOUND);
					PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_FAILED);
					throw new HttpException(SR.GetString("Http_handler_not_found_for_request_type", new object[] { requestType }));
				}
				if (string.IsNullOrEmpty(text2))
				{
					return httpHandler;
				}
				IHttpHandlerFactory factory = this.GetFactory(text2);
				try
				{
					httpHandler = factory.GetHandler(context, requestType, path.VirtualPathString, pathTranslated);
				}
				catch (FileNotFoundException ex)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(404, null, ex);
					}
					throw new HttpException(404, null);
				}
				catch (DirectoryNotFoundException ex2)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(404, null, ex2);
					}
					throw new HttpException(404, null);
				}
				catch (PathTooLongException ex3)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(414, null, ex3);
					}
					throw new HttpException(414, null);
				}
				if (this._handlerRecycleList == null)
				{
					this._handlerRecycleList = new ArrayList();
				}
				this._handlerRecycleList.Add(new HandlerWithFactory(httpHandler, factory));
			}
			return httpHandler;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008A38 File Offset: 0x00007A38
		internal IHttpHandler MapHttpHandler(HttpContext context, string requestType, VirtualPath path, string pathTranslated, bool useAppConfig)
		{
			IHttpHandler httpHandler = ((context.ServerExecuteDepth == 0) ? context.RemapHandlerInstance : null);
			using (new ApplicationImpersonationContext())
			{
				if (httpHandler != null)
				{
					return httpHandler;
				}
				HttpHandlerAction handlerMapping = this.GetHandlerMapping(context, requestType, path, useAppConfig);
				if (handlerMapping == null)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_FOUND);
					PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_FAILED);
					throw new HttpException(SR.GetString("Http_handler_not_found_for_request_type", new object[] { requestType }));
				}
				IHttpHandlerFactory factory = this.GetFactory(handlerMapping);
				try
				{
					IHttpHandlerFactory2 httpHandlerFactory = factory as IHttpHandlerFactory2;
					if (httpHandlerFactory != null)
					{
						httpHandler = httpHandlerFactory.GetHandler(context, requestType, path, pathTranslated);
					}
					else
					{
						httpHandler = factory.GetHandler(context, requestType, path.VirtualPathString, pathTranslated);
					}
				}
				catch (FileNotFoundException ex)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(404, null, ex);
					}
					throw new HttpException(404, null);
				}
				catch (DirectoryNotFoundException ex2)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(404, null, ex2);
					}
					throw new HttpException(404, null);
				}
				catch (PathTooLongException ex3)
				{
					if (HttpRuntime.HasPathDiscoveryPermission(pathTranslated))
					{
						throw new HttpException(414, null, ex3);
					}
					throw new HttpException(414, null);
				}
				if (this._handlerRecycleList == null)
				{
					this._handlerRecycleList = new ArrayList();
				}
				this._handlerRecycleList.Add(new HandlerWithFactory(httpHandler, factory));
			}
			return httpHandler;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008BE4 File Offset: 0x00007BE4
		public virtual string GetVaryByCustomString(HttpContext context, string custom)
		{
			if (StringUtil.EqualsIgnoreCase(custom, "browser"))
			{
				return context.Request.Browser.Type;
			}
			return null;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00008C05 File Offset: 0x00007C05
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00008C0D File Offset: 0x00007C0D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ISite Site
		{
			get
			{
				return this._site;
			}
			set
			{
				this._site = value;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008C18 File Offset: 0x00007C18
		IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		{
			this._context = context;
			this._context.ApplicationInstance = this;
			this._stepManager.InitRequest();
			this._context.Root();
			HttpAsyncResult httpAsyncResult = new HttpAsyncResult(cb, extraData);
			this.AsyncResult = httpAsyncResult;
			if (this._context.TraceIsEnabled)
			{
				HttpRuntime.Profile.StartRequest(this._context);
			}
			this.ResumeSteps(null);
			return httpAsyncResult;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008C84 File Offset: 0x00007C84
		void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
		{
			HttpAsyncResult httpAsyncResult = (HttpAsyncResult)result;
			if (httpAsyncResult.Error != null)
			{
				throw httpAsyncResult.Error;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008CA7 File Offset: 0x00007CA7
		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			throw new HttpException(SR.GetString("Sync_not_supported"));
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008CB8 File Offset: 0x00007CB8
		bool IHttpHandler.IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008CBC File Offset: 0x00007CBC
		internal void ProcessSpecialRequest(HttpContext context, MethodInfo method, int paramCount, object eventSource, EventArgs eventArgs, HttpSessionState session)
		{
			this._context = context;
			if (HttpRuntime.UseIntegratedPipeline && this._context != null)
			{
				this._context.HideRequestResponse = true;
			}
			this._hideRequestResponse = true;
			this._session = session;
			this._lastError = null;
			using (new HttpContextWrapper(context))
			{
				using (new ApplicationImpersonationContext())
				{
					try
					{
						this.SetAppLevelCulture();
						if (paramCount == 0)
						{
							method.Invoke(this, new object[0]);
						}
						else
						{
							method.Invoke(this, new object[] { eventSource, eventArgs });
						}
					}
					catch (Exception ex)
					{
						Exception ex2;
						if (ex is TargetInvocationException)
						{
							ex2 = ex.InnerException;
						}
						else
						{
							ex2 = ex;
						}
						this.RecordError(ex2);
						if (context == null)
						{
							try
							{
								WebBaseEvent.RaiseRuntimeError(ex2, this);
							}
							catch
							{
							}
						}
					}
					finally
					{
						if (this._state != null)
						{
							this._state.EnsureUnLock();
						}
						this.RestoreAppLevelCulture();
						if (HttpRuntime.UseIntegratedPipeline && this._context != null)
						{
							this._context.HideRequestResponse = false;
						}
						this._hideRequestResponse = false;
						this._context = null;
						this._session = null;
						this._lastError = null;
						this._appEvent = null;
					}
				}
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008E28 File Offset: 0x00007E28
		internal void RaiseErrorWithoutContext(Exception error)
		{
			try
			{
				try
				{
					this.SetAppLevelCulture();
					this._lastError = error;
					this.RaiseOnError();
				}
				finally
				{
					if (this._state != null)
					{
						this._state.EnsureUnLock();
					}
					this.RestoreAppLevelCulture();
					this._lastError = null;
					this._appEvent = null;
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008E94 File Offset: 0x00007E94
		internal void InitInternal(HttpContext context, HttpApplicationState state, MethodInfo[] handlers)
		{
			this._state = state;
			PerfCounters.IncrementCounter(AppPerfCounter.PIPELINES);
			try
			{
				try
				{
					this._initContext = context;
					this._initContext.ApplicationInstance = this;
					context.ConfigurationPath = context.Request.ApplicationPathObject;
					using (new HttpContextWrapper(context))
					{
						if (HttpRuntime.UseIntegratedPipeline)
						{
							try
							{
								context.HideRequestResponse = true;
								this._hideRequestResponse = true;
								this.InitIntegratedModules();
								goto IL_006B;
							}
							finally
							{
								context.HideRequestResponse = false;
								this._hideRequestResponse = false;
							}
						}
						this.InitModules();
						IL_006B:
						if (handlers != null)
						{
							this.HookupEventHandlersForApplicationAndModules(handlers);
						}
						this._context = context;
						if (HttpRuntime.UseIntegratedPipeline && this._context != null)
						{
							this._context.HideRequestResponse = true;
						}
						this._hideRequestResponse = true;
						try
						{
							this.Init();
						}
						catch (Exception ex)
						{
							this.RecordError(ex);
						}
					}
					if (HttpRuntime.UseIntegratedPipeline && this._context != null)
					{
						this._context.HideRequestResponse = false;
					}
					this._hideRequestResponse = false;
					this._context = null;
					this._resumeStepsWaitCallback = new WaitCallback(this.ResumeStepsWaitCallback);
					if (HttpRuntime.UseIntegratedPipeline)
					{
						this._stepManager = new HttpApplication.PipelineStepManager(this);
					}
					else
					{
						this._stepManager = new HttpApplication.ApplicationStepManager(this);
					}
					this._stepManager.BuildSteps(this._resumeStepsWaitCallback);
				}
				finally
				{
					this._initInternalCompleted = true;
					context.ConfigurationPath = null;
					this._initContext.ApplicationInstance = null;
					this._initContext = null;
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009070 File Offset: 0x00008070
		private void CreateEventExecutionSteps(object eventIndex, ArrayList steps)
		{
			HttpApplication.AsyncAppEventHandler asyncAppEventHandler = this.AsyncEvents[eventIndex];
			if (asyncAppEventHandler != null)
			{
				asyncAppEventHandler.CreateExecutionSteps(this, steps);
			}
			EventHandler eventHandler = (EventHandler)this.Events[eventIndex];
			if (eventHandler != null)
			{
				Delegate[] invocationList = eventHandler.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					steps.Add(new HttpApplication.SyncEventExecutionStep(this, (EventHandler)invocationList[i]));
				}
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000090D4 File Offset: 0x000080D4
		internal void InitSpecial(HttpApplicationState state, MethodInfo[] handlers, IntPtr appContext, HttpContext context)
		{
			this._state = state;
			try
			{
				if (context != null)
				{
					this._initContext = context;
					this._initContext.ApplicationInstance = this;
				}
				if (appContext != IntPtr.Zero)
				{
					using (new ApplicationImpersonationContext())
					{
						HttpRuntime.CheckApplicationEnabled();
					}
					this.InitAppLevelCulture();
					this.RegisterEventSubscriptionsWithIIS(appContext, context, handlers);
				}
				else
				{
					this.InitAppLevelCulture();
					if (handlers != null)
					{
						this.HookupEventHandlersForApplicationAndModules(handlers);
					}
				}
				if (appContext != IntPtr.Zero && (this._appPostNotifications != (RequestNotification)0 || this._appRequestNotifications != (RequestNotification)0))
				{
					this.RegisterIntegratedEvent(appContext, "global.asax", this._appRequestNotifications, this._appPostNotifications, base.GetType().FullName, "managedHandler", false);
				}
			}
			finally
			{
				HttpApplication._initSpecialCompleted = true;
				if (this._initContext != null)
				{
					this._initContext.ApplicationInstance = null;
					this._initContext = null;
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000091CC File Offset: 0x000081CC
		internal void DisposeInternal()
		{
			PerfCounters.DecrementCounter(AppPerfCounter.PIPELINES);
			try
			{
				this.Dispose();
			}
			catch (Exception ex)
			{
				this.RecordError(ex);
			}
			if (this._moduleCollection != null)
			{
				int count = this._moduleCollection.Count;
				for (int i = 0; i < count; i++)
				{
					try
					{
						if (HttpRuntime.UseIntegratedPipeline)
						{
							this._currentModuleCollectionKey = this._moduleCollection.GetKey(i);
						}
						this._moduleCollection[i].Dispose();
					}
					catch
					{
					}
				}
				this._moduleCollection = null;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009264 File Offset: 0x00008264
		private void BuildEventMaskDictionary(Dictionary<string, RequestNotification> eventMask)
		{
			eventMask["BeginRequest"] = RequestNotification.BeginRequest;
			eventMask["AuthenticateRequest"] = RequestNotification.AuthenticateRequest;
			eventMask["PostAuthenticateRequest"] = RequestNotification.AuthenticateRequest;
			eventMask["AuthorizeRequest"] = RequestNotification.AuthorizeRequest;
			eventMask["PostAuthorizeRequest"] = RequestNotification.AuthorizeRequest;
			eventMask["ResolveRequestCache"] = RequestNotification.ResolveRequestCache;
			eventMask["PostResolveRequestCache"] = RequestNotification.ResolveRequestCache;
			eventMask["MapRequestHandler"] = RequestNotification.MapRequestHandler;
			eventMask["PostMapRequestHandler"] = RequestNotification.MapRequestHandler;
			eventMask["AcquireRequestState"] = RequestNotification.AcquireRequestState;
			eventMask["PostAcquireRequestState"] = RequestNotification.AcquireRequestState;
			eventMask["PreRequestHandlerExecute"] = RequestNotification.PreExecuteRequestHandler;
			eventMask["PostRequestHandlerExecute"] = RequestNotification.ExecuteRequestHandler;
			eventMask["ReleaseRequestState"] = RequestNotification.ReleaseRequestState;
			eventMask["PostReleaseRequestState"] = RequestNotification.ReleaseRequestState;
			eventMask["UpdateRequestCache"] = RequestNotification.UpdateRequestCache;
			eventMask["PostUpdateRequestCache"] = RequestNotification.UpdateRequestCache;
			eventMask["LogRequest"] = RequestNotification.LogRequest;
			eventMask["PostLogRequest"] = RequestNotification.LogRequest;
			eventMask["EndRequest"] = RequestNotification.EndRequest;
			eventMask["PreSendRequestHeaders"] = RequestNotification.SendResponse;
			eventMask["PreSendRequestContent"] = RequestNotification.SendResponse;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000093A8 File Offset: 0x000083A8
		private void HookupEventHandlersForApplicationAndModules(MethodInfo[] handlers)
		{
			this._currentModuleCollectionKey = "global.asax";
			if (this._pipelineEventMasks == null)
			{
				Dictionary<string, RequestNotification> dictionary = new Dictionary<string, RequestNotification>();
				this.BuildEventMaskDictionary(dictionary);
				if (this._pipelineEventMasks == null)
				{
					this._pipelineEventMasks = dictionary;
				}
			}
			foreach (MethodInfo methodInfo in handlers)
			{
				string name = methodInfo.Name;
				int num = name.IndexOf('_');
				string text = name.Substring(0, num);
				object obj = null;
				if (StringUtil.EqualsIgnoreCase(text, "Application"))
				{
					obj = this;
				}
				else if (this._moduleCollection != null)
				{
					obj = this._moduleCollection[text];
				}
				if (obj != null)
				{
					Type type = obj.GetType();
					EventDescriptorCollection events = TypeDescriptor.GetEvents(type);
					string text2 = name.Substring(num + 1);
					EventDescriptor eventDescriptor = events.Find(text2, true);
					if (eventDescriptor == null && StringUtil.EqualsIgnoreCase(text2.Substring(0, 2), "on"))
					{
						text2 = text2.Substring(2);
						eventDescriptor = events.Find(text2, true);
					}
					MethodInfo methodInfo2 = null;
					if (eventDescriptor != null)
					{
						EventInfo @event = type.GetEvent(eventDescriptor.Name);
						if (@event != null)
						{
							methodInfo2 = @event.GetAddMethod();
						}
					}
					if (methodInfo2 != null)
					{
						ParameterInfo[] parameters = methodInfo2.GetParameters();
						if (parameters.Length == 1)
						{
							Delegate @delegate = null;
							ParameterInfo[] parameters2 = methodInfo.GetParameters();
							if (parameters2.Length == 0)
							{
								if (parameters[0].ParameterType != typeof(EventHandler))
								{
									goto IL_01F4;
								}
								ArglessEventHandlerProxy arglessEventHandlerProxy = new ArglessEventHandlerProxy(this, methodInfo);
								@delegate = arglessEventHandlerProxy.Handler;
							}
							else
							{
								try
								{
									@delegate = Delegate.CreateDelegate(parameters[0].ParameterType, this, name);
								}
								catch
								{
									goto IL_01F4;
								}
							}
							try
							{
								methodInfo2.Invoke(obj, new object[] { @delegate });
							}
							catch
							{
								if (HttpRuntime.UseIntegratedPipeline)
								{
									throw;
								}
							}
							if (text2 != null && this._pipelineEventMasks.ContainsKey(text2))
							{
								if (!StringUtil.StringStartsWith(text2, "Post"))
								{
									this._appRequestNotifications |= this._pipelineEventMasks[text2];
								}
								else
								{
									this._appPostNotifications |= this._pipelineEventMasks[text2];
								}
							}
						}
					}
				}
				IL_01F4:;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000095D4 File Offset: 0x000085D4
		private void RegisterIntegratedEvent(IntPtr appContext, string moduleName, RequestNotification requestNotifications, RequestNotification postRequestNotifications, string moduleType, string modulePrecondition, bool useHighPriority)
		{
			int num;
			if (HttpApplication._moduleIndexMap.ContainsKey(moduleName))
			{
				num = (int)HttpApplication._moduleIndexMap[moduleName];
			}
			else
			{
				num = HttpApplication._moduleIndexMap.Count;
				HttpApplication._moduleIndexMap[moduleName] = num;
			}
			int num2 = UnsafeIISMethods.MgdRegisterEventSubscription(appContext, moduleName, requestNotifications, postRequestNotifications, moduleType, modulePrecondition, new IntPtr(num), useHighPriority);
			if (num2 < 0)
			{
				throw new HttpException(SR.GetString("Failed_Pipeline_Subscription", new object[] { moduleName }));
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009654 File Offset: 0x00008654
		private void SetAppLevelCulture()
		{
			CultureInfo cultureInfo = null;
			if ((this._appLevelAutoCulture || this._appLevelAutoUICulture) && this._context != null && !this._context.HideRequestResponse)
			{
				string text = this._context.UserLanguageFromContext();
				if (text != null)
				{
					try
					{
						cultureInfo = HttpServerUtility.CreateReadOnlySpecificCultureInfo(text);
					}
					catch
					{
					}
				}
			}
			CultureInfo cultureInfo2 = this._appLevelCulture;
			CultureInfo cultureInfo3 = this._appLevelUICulture;
			if (cultureInfo != null)
			{
				if (this._appLevelAutoCulture)
				{
					cultureInfo2 = cultureInfo;
				}
				if (this._appLevelAutoUICulture)
				{
					cultureInfo3 = cultureInfo;
				}
			}
			this._savedAppLevelCulture = Thread.CurrentThread.CurrentCulture;
			this._savedAppLevelUICulture = Thread.CurrentThread.CurrentUICulture;
			if (cultureInfo2 != null && cultureInfo2 != Thread.CurrentThread.CurrentCulture)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo2;
			}
			if (cultureInfo3 != null && cultureInfo3 != Thread.CurrentThread.CurrentUICulture)
			{
				Thread.CurrentThread.CurrentUICulture = cultureInfo3;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009734 File Offset: 0x00008734
		private void RestoreAppLevelCulture()
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
			if (this._savedAppLevelCulture != null)
			{
				if (currentCulture != this._savedAppLevelCulture)
				{
					Thread.CurrentThread.CurrentCulture = this._savedAppLevelCulture;
				}
				this._savedAppLevelCulture = null;
			}
			if (this._savedAppLevelUICulture != null)
			{
				if (currentUICulture != this._savedAppLevelUICulture)
				{
					Thread.CurrentThread.CurrentUICulture = this._savedAppLevelUICulture;
				}
				this._savedAppLevelUICulture = null;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000097A8 File Offset: 0x000087A8
		private HttpApplication.ThreadContext OnThreadEnterPrivate(bool setImpersonationContext)
		{
			HttpApplication.ThreadContext threadContext = new HttpApplication.ThreadContext(this._context);
			threadContext.Enter(setImpersonationContext);
			if (!this._timeoutManagerInitialized)
			{
				this._context.EnsureTimeout();
				HttpRuntime.RequestTimeoutManager.Add(this._context);
				this._timeoutManagerInitialized = true;
			}
			return threadContext;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000097F3 File Offset: 0x000087F3
		internal HttpApplication.ThreadContext OnThreadEnter()
		{
			return this.OnThreadEnterPrivate(true);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000097FC File Offset: 0x000087FC
		internal HttpApplication.ThreadContext OnThreadEnter(bool setImpersonationContext)
		{
			return this.OnThreadEnterPrivate(setImpersonationContext);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009808 File Offset: 0x00008808
		internal Exception ExecuteStep(HttpApplication.IExecutionStep step, ref bool completedSynchronously)
		{
			Exception ex = null;
			try
			{
				try
				{
					if (step.IsCancellable)
					{
						this._context.BeginCancellablePeriod();
						try
						{
							step.Execute();
						}
						finally
						{
							this._context.EndCancellablePeriod();
						}
						this._context.WaitForExceptionIfCancelled();
					}
					else
					{
						step.Execute();
					}
					if (!step.CompletedSynchronously)
					{
						completedSynchronously = false;
						return null;
					}
				}
				catch (Exception ex2)
				{
					ex = ex2;
					if (ImpersonationContext.CurrentThreadTokenExists)
					{
						ex2.Data["ASPIMPERSONATING"] = string.Empty;
					}
					if (ex2 is ThreadAbortException && (Thread.CurrentThread.ThreadState & global::System.Threading.ThreadState.AbortRequested) == global::System.Threading.ThreadState.Running)
					{
						ex = null;
						this._stepManager.CompleteRequest();
					}
				}
				catch
				{
				}
			}
			catch (ThreadAbortException ex3)
			{
				if (ex3.ExceptionState != null && ex3.ExceptionState is HttpApplication.CancelModuleException)
				{
					HttpApplication.CancelModuleException ex4 = (HttpApplication.CancelModuleException)ex3.ExceptionState;
					if (ex4.Timeout)
					{
						ex = new HttpException(SR.GetString("Request_timed_out"), null, 3001);
						PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_TIMED_OUT);
					}
					else
					{
						ex = null;
						this._stepManager.CompleteRequest();
					}
					Thread.ResetAbort();
				}
			}
			completedSynchronously = true;
			return ex;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000994C File Offset: 0x0000894C
		private void ResumeStepsFromThreadPoolThread(Exception error)
		{
			if (Thread.CurrentThread.IsThreadPoolThread)
			{
				this.ResumeSteps(error);
				return;
			}
			ThreadPool.QueueUserWorkItem(this._resumeStepsWaitCallback, error);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000996F File Offset: 0x0000896F
		private void ResumeStepsWaitCallback(object error)
		{
			this.ResumeSteps(error as Exception);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000997D File Offset: 0x0000897D
		private void ResumeSteps(Exception error)
		{
			this._stepManager.ResumeSteps(error);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000998C File Offset: 0x0000898C
		private void RecordError(Exception error)
		{
			bool flag = true;
			if (this._context != null)
			{
				if (this._context.Error != null)
				{
					flag = false;
				}
				this._context.AddError(error);
			}
			else
			{
				if (this._lastError != null)
				{
					flag = false;
				}
				this._lastError = error;
			}
			if (flag)
			{
				this.RaiseOnError();
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000099DC File Offset: 0x000089DC
		private void InitModulesCommon()
		{
			int count = this._moduleCollection.Count;
			for (int i = 0; i < count; i++)
			{
				this._currentModuleCollectionKey = this._moduleCollection.GetKey(i);
				this._moduleCollection[i].Init(this);
			}
			this._currentModuleCollectionKey = null;
			this.InitAppLevelCulture();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009A32 File Offset: 0x00008A32
		private void InitIntegratedModules()
		{
			this._moduleCollection = this.BuildIntegratedModuleCollection(HttpApplication._moduleConfigInfo);
			this.InitModulesCommon();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009A4C File Offset: 0x00008A4C
		private void InitModules()
		{
			HttpModulesSection httpModules = RuntimeConfig.GetAppConfig().HttpModules;
			this._moduleCollection = httpModules.CreateModules();
			this.InitModulesCommon();
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00009A76 File Offset: 0x00008A76
		internal string CurrentModuleCollectionKey
		{
			get
			{
				if (this._currentModuleCollectionKey != null)
				{
					return this._currentModuleCollectionKey;
				}
				return "UnknownModule";
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009A8C File Offset: 0x00008A8C
		private void RegisterEventSubscriptionsWithIIS(IntPtr appContext, HttpContext context, MethodInfo[] handlers)
		{
			this.RegisterIntegratedEvent(appContext, "AspNetFilterModule", RequestNotification.UpdateRequestCache | RequestNotification.LogRequest, (RequestNotification)0, string.Empty, string.Empty, true);
			this._moduleCollection = this.GetModuleCollection(appContext);
			if (handlers != null)
			{
				this.HookupEventHandlersForApplicationAndModules(handlers);
			}
			HttpApplicationFactory.EnsureAppStartCalledForIntegratedMode(context, this);
			this._currentModuleCollectionKey = "global.asax";
			try
			{
				this._hideRequestResponse = true;
				context.HideRequestResponse = true;
				this._context = context;
				this.Init();
			}
			catch (Exception ex)
			{
				this.RecordError(ex);
				Exception error = context.Error;
				if (error != null)
				{
					throw error;
				}
			}
			finally
			{
				this._context = null;
				context.HideRequestResponse = false;
				this._hideRequestResponse = false;
			}
			RequestNotification requestNotification;
			RequestNotification requestNotification2;
			this.ProcessEventSubscriptions(out requestNotification, out requestNotification2);
			this._appRequestNotifications |= requestNotification;
			this._appPostNotifications |= requestNotification2;
			for (int i = 0; i < this._moduleCollection.Count; i++)
			{
				this._currentModuleCollectionKey = this._moduleCollection.GetKey(i);
				IHttpModule httpModule = this._moduleCollection.Get(i);
				ModuleConfigurationInfo moduleConfigurationInfo = HttpApplication._moduleConfigInfo[i];
				httpModule.Init(this);
				this.ProcessEventSubscriptions(out requestNotification, out requestNotification2);
				if (requestNotification != (RequestNotification)0 || requestNotification2 != (RequestNotification)0)
				{
					this.RegisterIntegratedEvent(appContext, moduleConfigurationInfo.Name, requestNotification, requestNotification2, moduleConfigurationInfo.Type, moduleConfigurationInfo.Precondition, false);
				}
			}
			this.RegisterIntegratedEvent(appContext, "ManagedPipelineHandler", RequestNotification.MapRequestHandler | RequestNotification.ExecuteRequestHandler, (RequestNotification)0, string.Empty, string.Empty, false);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009C0C File Offset: 0x00008C0C
		private void ProcessEventSubscriptions(out RequestNotification requestNotifications, out RequestNotification postRequestNotifications)
		{
			requestNotifications = (RequestNotification)0;
			postRequestNotifications = (RequestNotification)0;
			if (this.HasEventSubscription(HttpApplication.EventBeginRequest))
			{
				requestNotifications |= RequestNotification.BeginRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventAuthenticateRequest))
			{
				requestNotifications |= RequestNotification.AuthenticateRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostAuthenticateRequest))
			{
				postRequestNotifications |= RequestNotification.AuthenticateRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventAuthorizeRequest))
			{
				requestNotifications |= RequestNotification.AuthorizeRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostAuthorizeRequest))
			{
				postRequestNotifications |= RequestNotification.AuthorizeRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventResolveRequestCache))
			{
				requestNotifications |= RequestNotification.ResolveRequestCache;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostResolveRequestCache))
			{
				postRequestNotifications |= RequestNotification.ResolveRequestCache;
			}
			if (this.HasEventSubscription(HttpApplication.EventMapRequestHandler))
			{
				requestNotifications |= RequestNotification.MapRequestHandler;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostMapRequestHandler))
			{
				postRequestNotifications |= RequestNotification.MapRequestHandler;
			}
			if (this.HasEventSubscription(HttpApplication.EventAcquireRequestState))
			{
				requestNotifications |= RequestNotification.AcquireRequestState;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostAcquireRequestState))
			{
				postRequestNotifications |= RequestNotification.AcquireRequestState;
			}
			if (this.HasEventSubscription(HttpApplication.EventPreRequestHandlerExecute))
			{
				requestNotifications |= RequestNotification.PreExecuteRequestHandler;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostRequestHandlerExecute))
			{
				postRequestNotifications |= RequestNotification.ExecuteRequestHandler;
			}
			if (this.HasEventSubscription(HttpApplication.EventReleaseRequestState))
			{
				requestNotifications |= RequestNotification.ReleaseRequestState;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostReleaseRequestState))
			{
				postRequestNotifications |= RequestNotification.ReleaseRequestState;
			}
			if (this.HasEventSubscription(HttpApplication.EventUpdateRequestCache))
			{
				requestNotifications |= RequestNotification.UpdateRequestCache;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostUpdateRequestCache))
			{
				postRequestNotifications |= RequestNotification.UpdateRequestCache;
			}
			if (this.HasEventSubscription(HttpApplication.EventLogRequest))
			{
				requestNotifications |= RequestNotification.LogRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventPostLogRequest))
			{
				postRequestNotifications |= RequestNotification.LogRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventEndRequest))
			{
				requestNotifications |= RequestNotification.EndRequest;
			}
			if (this.HasEventSubscription(HttpApplication.EventPreSendRequestHeaders))
			{
				requestNotifications |= RequestNotification.SendResponse;
			}
			if (this.HasEventSubscription(HttpApplication.EventPreSendRequestContent))
			{
				requestNotifications |= RequestNotification.SendResponse;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009DF0 File Offset: 0x00008DF0
		private bool HasEventSubscription(object eventIndex)
		{
			bool flag = false;
			HttpApplication.AsyncAppEventHandler asyncAppEventHandler = this.AsyncEvents[eventIndex];
			if (asyncAppEventHandler != null && asyncAppEventHandler.Count > 0)
			{
				asyncAppEventHandler.Reset();
				flag = true;
			}
			EventHandler eventHandler = (EventHandler)this.Events[eventIndex];
			if (eventHandler != null)
			{
				Delegate[] invocationList = eventHandler.GetInvocationList();
				if (invocationList.Length > 0)
				{
					flag = true;
				}
				foreach (Delegate @delegate in invocationList)
				{
					this.Events.RemoveHandler(eventIndex, @delegate);
				}
			}
			return flag;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009E70 File Offset: 0x00008E70
		private void InitAppLevelCulture()
		{
			GlobalizationSection globalization = RuntimeConfig.GetAppConfig().Globalization;
			string culture = globalization.Culture;
			string uiculture = globalization.UICulture;
			if (!string.IsNullOrEmpty(culture))
			{
				if (StringUtil.StringStartsWithIgnoreCase(culture, HttpApplication.AutoCulture))
				{
					this._appLevelAutoCulture = true;
					string fallbackCulture = HttpApplication.GetFallbackCulture(culture);
					if (fallbackCulture != null)
					{
						this._appLevelCulture = HttpServerUtility.CreateReadOnlyCultureInfo(culture.Substring(5));
					}
				}
				else
				{
					this._appLevelAutoCulture = false;
					this._appLevelCulture = HttpServerUtility.CreateReadOnlyCultureInfo(globalization.Culture);
				}
			}
			if (!string.IsNullOrEmpty(uiculture))
			{
				if (StringUtil.StringStartsWithIgnoreCase(uiculture, HttpApplication.AutoCulture))
				{
					this._appLevelAutoUICulture = true;
					string fallbackCulture2 = HttpApplication.GetFallbackCulture(uiculture);
					if (fallbackCulture2 != null)
					{
						this._appLevelUICulture = HttpServerUtility.CreateReadOnlyCultureInfo(uiculture.Substring(5));
						return;
					}
				}
				else
				{
					this._appLevelAutoUICulture = false;
					this._appLevelUICulture = HttpServerUtility.CreateReadOnlyCultureInfo(globalization.UICulture);
				}
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009F3B File Offset: 0x00008F3B
		internal static string GetFallbackCulture(string culture)
		{
			if (culture.Length > 5 && culture.IndexOf(':') == 4)
			{
				return culture.Substring(5);
			}
			return null;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009F5C File Offset: 0x00008F5C
		private IHttpHandlerFactory GetFactory(HttpHandlerAction mapping)
		{
			HandlerFactoryCache handlerFactoryCache = (HandlerFactoryCache)this._handlerFactories[mapping.Type];
			if (handlerFactoryCache == null)
			{
				handlerFactoryCache = new HandlerFactoryCache(mapping);
				this._handlerFactories[mapping.Type] = handlerFactoryCache;
			}
			return handlerFactoryCache.Factory;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009FA4 File Offset: 0x00008FA4
		private IHttpHandlerFactory GetFactory(string type)
		{
			HandlerFactoryCache handlerFactoryCache = (HandlerFactoryCache)this._handlerFactories[type];
			if (handlerFactoryCache == null)
			{
				handlerFactoryCache = new HandlerFactoryCache(type);
				this._handlerFactories[type] = handlerFactoryCache;
			}
			return handlerFactoryCache.Factory;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009FE0 File Offset: 0x00008FE0
		private void RecycleHandlers()
		{
			if (this._handlerRecycleList != null)
			{
				int count = this._handlerRecycleList.Count;
				for (int i = 0; i < count; i++)
				{
					((HandlerWithFactory)this._handlerRecycleList[i]).Recycle();
				}
				this._handlerRecycleList = null;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A02C File Offset: 0x0000902C
		internal void AssignContext(HttpContext context)
		{
			if (this._context == null)
			{
				this._stepManager.InitRequest();
				this._context = context;
				this._context.ApplicationInstance = this;
				if (this._context.TraceIsEnabled)
				{
					HttpRuntime.Profile.StartRequest(this._context);
				}
				this._context.SetImpersonationEnabled();
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A088 File Offset: 0x00009088
		internal IAsyncResult BeginProcessRequestNotification(HttpContext context, AsyncCallback cb)
		{
			if (this._context == null)
			{
				this.AssignContext(context);
			}
			context.CurrentModuleEventIndex = -1;
			HttpAsyncResult httpAsyncResult = new HttpAsyncResult(cb, context);
			context.NotificationContext.AsyncResult = httpAsyncResult;
			this.ResumeSteps(null);
			return httpAsyncResult;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000A0C8 File Offset: 0x000090C8
		internal RequestNotificationStatus EndProcessRequestNotification(IAsyncResult result)
		{
			HttpAsyncResult httpAsyncResult = (HttpAsyncResult)result;
			if (httpAsyncResult.Error != null)
			{
				throw httpAsyncResult.Error;
			}
			return httpAsyncResult.Status;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A0F4 File Offset: 0x000090F4
		internal void ReleaseAppInstance()
		{
			if (this._context != null)
			{
				if (this._context.TraceIsEnabled)
				{
					HttpRuntime.Profile.EndRequest(this._context);
				}
				this._context.ClearReferences();
				if (this._timeoutManagerInitialized)
				{
					HttpRuntime.RequestTimeoutManager.Remove(this._context);
					this._timeoutManagerInitialized = false;
				}
			}
			this.RecycleHandlers();
			if (this.AsyncResult != null)
			{
				this.AsyncResult = null;
			}
			this._context = null;
			this.AppEvent = null;
			HttpApplicationFactory.RecycleApplicationInstance(this);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A17C File Offset: 0x0000917C
		private void AddEventMapping(string moduleName, RequestNotification requestNotification, bool isPostNotification, HttpApplication.IExecutionStep step)
		{
			this.ThrowIfEventBindingDisallowed();
			if (!this.IsContainerInitalizationAllowed)
			{
				return;
			}
			PipelineModuleStepContainer moduleContainer = this.GetModuleContainer(moduleName);
			if (moduleContainer != null)
			{
				moduleContainer.AddEvent(requestNotification, isPostNotification, step);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000A1AD File Offset: 0x000091AD
		internal static List<ModuleConfigurationInfo> IntegratedModuleList
		{
			get
			{
				return HttpApplication._moduleConfigInfo;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A1B4 File Offset: 0x000091B4
		private HttpModuleCollection GetModuleCollection(IntPtr appContext)
		{
			if (HttpApplication._moduleConfigInfo != null)
			{
				return this.BuildIntegratedModuleCollection(HttpApplication._moduleConfigInfo);
			}
			List<ModuleConfigurationInfo> list = null;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			int num = 0;
			IntPtr intPtr3 = IntPtr.Zero;
			int num2 = 0;
			IntPtr intPtr4 = IntPtr.Zero;
			int num3 = 0;
			try
			{
				int num4 = 0;
				int num5 = UnsafeIISMethods.MgdGetModuleCollection(appContext, out intPtr, out num4);
				if (num5 < 0)
				{
					throw new HttpException(SR.GetString("Cant_Read_Native_Modules", new object[] { num5.ToString("X8", CultureInfo.InvariantCulture) }));
				}
				list = new List<ModuleConfigurationInfo>(num4);
				uint num6 = 0U;
				while ((ulong)num6 < (ulong)((long)num4))
				{
					num5 = UnsafeIISMethods.MgdGetNextModule(intPtr, ref num6, out intPtr2, out num, out intPtr3, out num2, out intPtr4, out num3);
					if (num5 < 0)
					{
						throw new HttpException(SR.GetString("Cant_Read_Native_Modules", new object[] { num5.ToString("X8", CultureInfo.InvariantCulture) }));
					}
					string text = ((num > 0) ? StringUtil.StringFromWCharPtr(intPtr2, num) : null);
					string text2 = ((num2 > 0) ? StringUtil.StringFromWCharPtr(intPtr3, num2) : null);
					string text3 = ((num3 > 0) ? StringUtil.StringFromWCharPtr(intPtr4, num3) : string.Empty);
					Marshal.FreeBSTR(intPtr2);
					intPtr2 = IntPtr.Zero;
					num = 0;
					Marshal.FreeBSTR(intPtr3);
					intPtr3 = IntPtr.Zero;
					num2 = 0;
					Marshal.FreeBSTR(intPtr4);
					intPtr4 = IntPtr.Zero;
					num3 = 0;
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						list.Add(new ModuleConfigurationInfo(text, text2, text3));
					}
					num6 += 1U;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
					intPtr = IntPtr.Zero;
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeBSTR(intPtr2);
					intPtr2 = IntPtr.Zero;
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.FreeBSTR(intPtr3);
					intPtr3 = IntPtr.Zero;
				}
				if (intPtr4 != IntPtr.Zero)
				{
					Marshal.FreeBSTR(intPtr4);
					intPtr4 = IntPtr.Zero;
				}
			}
			HttpApplication._moduleConfigInfo = list;
			return this.BuildIntegratedModuleCollection(HttpApplication._moduleConfigInfo);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A3D0 File Offset: 0x000093D0
		private HttpModuleCollection BuildIntegratedModuleCollection(List<ModuleConfigurationInfo> moduleList)
		{
			HttpModuleCollection httpModuleCollection = new HttpModuleCollection();
			foreach (ModuleConfigurationInfo moduleConfigurationInfo in moduleList)
			{
				ModulesEntry modulesEntry = new ModulesEntry(moduleConfigurationInfo.Name, moduleConfigurationInfo.Type, "type", null);
				httpModuleCollection.AddModule(modulesEntry.ModuleName, modulesEntry.Create());
			}
			return httpModuleCollection;
		}

		// Token: 0x04000DDC RID: 3548
		internal const string MANAGED_PRECONDITION = "managedHandler";

		// Token: 0x04000DDD RID: 3549
		internal const string IMPLICIT_FILTER_MODULE = "AspNetFilterModule";

		// Token: 0x04000DDE RID: 3550
		internal const string IMPLICIT_HANDLER = "ManagedPipelineHandler";

		// Token: 0x04000DDF RID: 3551
		private HttpApplicationState _state;

		// Token: 0x04000DE0 RID: 3552
		private HttpContext _initContext;

		// Token: 0x04000DE1 RID: 3553
		private HttpAsyncResult _ar;

		// Token: 0x04000DE2 RID: 3554
		private HttpModuleCollection _moduleCollection;

		// Token: 0x04000DE3 RID: 3555
		private static readonly object EventDisposed = new object();

		// Token: 0x04000DE4 RID: 3556
		private static readonly object EventErrorRecorded = new object();

		// Token: 0x04000DE5 RID: 3557
		private static readonly object EventPreSendRequestHeaders = new object();

		// Token: 0x04000DE6 RID: 3558
		private static readonly object EventPreSendRequestContent = new object();

		// Token: 0x04000DE7 RID: 3559
		private static readonly object EventBeginRequest = new object();

		// Token: 0x04000DE8 RID: 3560
		private static readonly object EventAuthenticateRequest = new object();

		// Token: 0x04000DE9 RID: 3561
		private static readonly object EventDefaultAuthentication = new object();

		// Token: 0x04000DEA RID: 3562
		private static readonly object EventPostAuthenticateRequest = new object();

		// Token: 0x04000DEB RID: 3563
		private static readonly object EventAuthorizeRequest = new object();

		// Token: 0x04000DEC RID: 3564
		private static readonly object EventPostAuthorizeRequest = new object();

		// Token: 0x04000DED RID: 3565
		private static readonly object EventResolveRequestCache = new object();

		// Token: 0x04000DEE RID: 3566
		private static readonly object EventPostResolveRequestCache = new object();

		// Token: 0x04000DEF RID: 3567
		private static readonly object EventMapRequestHandler = new object();

		// Token: 0x04000DF0 RID: 3568
		private static readonly object EventPostMapRequestHandler = new object();

		// Token: 0x04000DF1 RID: 3569
		private static readonly object EventAcquireRequestState = new object();

		// Token: 0x04000DF2 RID: 3570
		private static readonly object EventPostAcquireRequestState = new object();

		// Token: 0x04000DF3 RID: 3571
		private static readonly object EventPreRequestHandlerExecute = new object();

		// Token: 0x04000DF4 RID: 3572
		private static readonly object EventPostRequestHandlerExecute = new object();

		// Token: 0x04000DF5 RID: 3573
		private static readonly object EventReleaseRequestState = new object();

		// Token: 0x04000DF6 RID: 3574
		private static readonly object EventPostReleaseRequestState = new object();

		// Token: 0x04000DF7 RID: 3575
		private static readonly object EventUpdateRequestCache = new object();

		// Token: 0x04000DF8 RID: 3576
		private static readonly object EventPostUpdateRequestCache = new object();

		// Token: 0x04000DF9 RID: 3577
		private static readonly object EventLogRequest = new object();

		// Token: 0x04000DFA RID: 3578
		private static readonly object EventPostLogRequest = new object();

		// Token: 0x04000DFB RID: 3579
		private static readonly object EventEndRequest = new object();

		// Token: 0x04000DFC RID: 3580
		internal static readonly string AutoCulture = "auto";

		// Token: 0x04000DFD RID: 3581
		private EventHandlerList _events;

		// Token: 0x04000DFE RID: 3582
		private HttpApplication.AsyncAppEventHandlersTable _asyncEvents;

		// Token: 0x04000DFF RID: 3583
		private HttpApplication.StepManager _stepManager;

		// Token: 0x04000E00 RID: 3584
		private WaitCallback _resumeStepsWaitCallback;

		// Token: 0x04000E01 RID: 3585
		private EventArgs _appEvent;

		// Token: 0x04000E02 RID: 3586
		private Hashtable _handlerFactories = new Hashtable();

		// Token: 0x04000E03 RID: 3587
		private ArrayList _handlerRecycleList;

		// Token: 0x04000E04 RID: 3588
		private bool _hideRequestResponse;

		// Token: 0x04000E05 RID: 3589
		private HttpContext _context;

		// Token: 0x04000E06 RID: 3590
		private Exception _lastError;

		// Token: 0x04000E07 RID: 3591
		private bool _timeoutManagerInitialized;

		// Token: 0x04000E08 RID: 3592
		private HttpSessionState _session;

		// Token: 0x04000E09 RID: 3593
		private CultureInfo _appLevelCulture;

		// Token: 0x04000E0A RID: 3594
		private CultureInfo _appLevelUICulture;

		// Token: 0x04000E0B RID: 3595
		private CultureInfo _savedAppLevelCulture;

		// Token: 0x04000E0C RID: 3596
		private CultureInfo _savedAppLevelUICulture;

		// Token: 0x04000E0D RID: 3597
		private bool _appLevelAutoCulture;

		// Token: 0x04000E0E RID: 3598
		private bool _appLevelAutoUICulture;

		// Token: 0x04000E0F RID: 3599
		private Dictionary<string, RequestNotification> _pipelineEventMasks;

		// Token: 0x04000E10 RID: 3600
		private ISite _site;

		// Token: 0x04000E11 RID: 3601
		private static Hashtable _moduleIndexMap = new Hashtable();

		// Token: 0x04000E12 RID: 3602
		private static bool _initSpecialCompleted;

		// Token: 0x04000E13 RID: 3603
		private bool _initInternalCompleted;

		// Token: 0x04000E14 RID: 3604
		private RequestNotification _appRequestNotifications;

		// Token: 0x04000E15 RID: 3605
		private RequestNotification _appPostNotifications;

		// Token: 0x04000E16 RID: 3606
		private string _currentModuleCollectionKey = "global.asax";

		// Token: 0x04000E17 RID: 3607
		private static List<ModuleConfigurationInfo> _moduleConfigInfo;

		// Token: 0x04000E18 RID: 3608
		private PipelineModuleStepContainer[] _moduleContainers;

		// Token: 0x04000E19 RID: 3609
		private byte[] _entityBuffer;

		// Token: 0x0200003B RID: 59
		internal class ThreadContext
		{
			// Token: 0x060001F6 RID: 502 RVA: 0x0000A581 File Offset: 0x00009581
			internal ThreadContext(HttpContext context)
			{
				this._context = context;
			}

			// Token: 0x060001F7 RID: 503 RVA: 0x0000A590 File Offset: 0x00009590
			internal void Enter(bool setImpersonationContext)
			{
				this._savedContext = HttpContextWrapper.SwitchContext(this._context);
				if (setImpersonationContext)
				{
					this.SetImpersonationContext();
				}
				this._savedSynchronizationContext = AsyncOperationManager.SynchronizationContext;
				AsyncOperationManager.SynchronizationContext = this._context.SyncContext;
				Guid requestTraceIdentifier = this._context.WorkerRequest.RequestTraceIdentifier;
				if (!(requestTraceIdentifier == Guid.Empty))
				{
					CallContext.LogicalSetData("E2ETrace.ActivityID", requestTraceIdentifier);
				}
				this._context.ResetSqlDependencyCookie();
				this._savedPrincipal = Thread.CurrentPrincipal;
				Thread.CurrentPrincipal = this._context.User;
				this.SetRequestLevelCulture(this._context);
				if (this._context.CurrentThread == null)
				{
					this._setThread = true;
					this._context.CurrentThread = Thread.CurrentThread;
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A656 File Offset: 0x00009656
			internal bool HasLeaveBeenCalled
			{
				get
				{
					return this._hasLeaveBeenCalled;
				}
			}

			// Token: 0x060001F9 RID: 505 RVA: 0x0000A660 File Offset: 0x00009660
			internal void Leave()
			{
				this._hasLeaveBeenCalled = true;
				if (this._setThread)
				{
					this._context.CurrentThread = null;
				}
				HttpApplicationFactory.ApplicationState.EnsureUnLock();
				this.UndoImpersonationContext();
				this.RestoreRequestLevelCulture();
				AsyncOperationManager.SynchronizationContext = this._savedSynchronizationContext;
				Thread.CurrentPrincipal = this._savedPrincipal;
				this._context.RemoveSqlDependencyCookie();
				HttpContextWrapper.SwitchContext(this._savedContext);
				this._savedContext = null;
			}

			// Token: 0x060001FA RID: 506 RVA: 0x0000A6D2 File Offset: 0x000096D2
			internal void Synchronize(HttpContext context)
			{
				context.DynamicCulture = Thread.CurrentThread.CurrentCulture;
				context.DynamicUICulture = Thread.CurrentThread.CurrentUICulture;
			}

			// Token: 0x060001FB RID: 507 RVA: 0x0000A6F4 File Offset: 0x000096F4
			internal void SetImpersonationContext()
			{
				if (this._impersonationContext == null)
				{
					this._impersonationContext = new ClientImpersonationContext(this._context);
				}
			}

			// Token: 0x060001FC RID: 508 RVA: 0x0000A70F File Offset: 0x0000970F
			internal void UndoImpersonationContext()
			{
				if (this._impersonationContext != null)
				{
					this._impersonationContext.Undo();
					this._impersonationContext = null;
				}
			}

			// Token: 0x060001FD RID: 509 RVA: 0x0000A72C File Offset: 0x0000972C
			private void SetRequestLevelCulture(HttpContext context)
			{
				CultureInfo cultureInfo = null;
				CultureInfo cultureInfo2 = null;
				GlobalizationSection globalization = RuntimeConfig.GetConfig(context).Globalization;
				if (!string.IsNullOrEmpty(globalization.Culture))
				{
					cultureInfo = context.CultureFromConfig(globalization.Culture, true);
				}
				if (!string.IsNullOrEmpty(globalization.UICulture))
				{
					cultureInfo2 = context.CultureFromConfig(globalization.UICulture, false);
				}
				if (context.DynamicCulture != null)
				{
					cultureInfo = context.DynamicCulture;
				}
				if (context.DynamicUICulture != null)
				{
					cultureInfo2 = context.DynamicUICulture;
				}
				Page page = context.CurrentHandler as Page;
				if (page != null)
				{
					if (page.DynamicCulture != null)
					{
						cultureInfo = page.DynamicCulture;
					}
					if (page.DynamicUICulture != null)
					{
						cultureInfo2 = page.DynamicUICulture;
					}
				}
				this._savedCulture = Thread.CurrentThread.CurrentCulture;
				this._savedUICulture = Thread.CurrentThread.CurrentUICulture;
				if (cultureInfo != null && cultureInfo != Thread.CurrentThread.CurrentCulture)
				{
					Thread.CurrentThread.CurrentCulture = cultureInfo;
					this._setThreadCulture = cultureInfo;
				}
				if (cultureInfo2 != null && cultureInfo2 != Thread.CurrentThread.CurrentUICulture)
				{
					Thread.CurrentThread.CurrentUICulture = cultureInfo2;
					this._setThreadUICulture = cultureInfo2;
				}
			}

			// Token: 0x060001FE RID: 510 RVA: 0x0000A830 File Offset: 0x00009830
			private void RestoreRequestLevelCulture()
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
				if (this._savedCulture != null)
				{
					if (currentCulture != this._savedCulture)
					{
						Thread.CurrentThread.CurrentCulture = this._savedCulture;
						if (this._context != null)
						{
							this._context.DynamicCulture = currentCulture;
						}
					}
					this._savedCulture = null;
				}
				if (this._savedUICulture != null)
				{
					if (currentUICulture != this._savedUICulture)
					{
						Thread.CurrentThread.CurrentUICulture = this._savedUICulture;
						if (this._context != null)
						{
							this._context.DynamicUICulture = currentUICulture;
						}
					}
					this._savedUICulture = null;
				}
			}

			// Token: 0x04000E1A RID: 3610
			private HttpContext _context;

			// Token: 0x04000E1B RID: 3611
			private IPrincipal _savedPrincipal;

			// Token: 0x04000E1C RID: 3612
			private HttpContext _savedContext;

			// Token: 0x04000E1D RID: 3613
			private ImpersonationContext _impersonationContext;

			// Token: 0x04000E1E RID: 3614
			private SynchronizationContext _savedSynchronizationContext;

			// Token: 0x04000E1F RID: 3615
			private CultureInfo _savedCulture;

			// Token: 0x04000E20 RID: 3616
			private CultureInfo _setThreadCulture;

			// Token: 0x04000E21 RID: 3617
			private CultureInfo _savedUICulture;

			// Token: 0x04000E22 RID: 3618
			private CultureInfo _setThreadUICulture;

			// Token: 0x04000E23 RID: 3619
			private bool _hasLeaveBeenCalled;

			// Token: 0x04000E24 RID: 3620
			private bool _setThread;
		}

		// Token: 0x0200003C RID: 60
		internal class CancelModuleException
		{
			// Token: 0x060001FF RID: 511 RVA: 0x0000A8CB File Offset: 0x000098CB
			internal CancelModuleException(bool timeout)
			{
				this._timeout = timeout;
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A8DA File Offset: 0x000098DA
			internal bool Timeout
			{
				get
				{
					return this._timeout;
				}
			}

			// Token: 0x04000E25 RID: 3621
			private bool _timeout;
		}

		// Token: 0x0200003D RID: 61
		internal class AsyncAppEventHandler
		{
			// Token: 0x06000201 RID: 513 RVA: 0x0000A8E2 File Offset: 0x000098E2
			internal AsyncAppEventHandler()
			{
				this._count = 0;
				this._beginHandlers = new ArrayList();
				this._endHandlers = new ArrayList();
				this._stateObjects = new ArrayList();
			}

			// Token: 0x06000202 RID: 514 RVA: 0x0000A912 File Offset: 0x00009912
			internal void Reset()
			{
				this._count = 0;
				this._beginHandlers.Clear();
				this._endHandlers.Clear();
				this._stateObjects.Clear();
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x06000203 RID: 515 RVA: 0x0000A93C File Offset: 0x0000993C
			internal int Count
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x06000204 RID: 516 RVA: 0x0000A944 File Offset: 0x00009944
			internal void Add(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
			{
				this._beginHandlers.Add(beginHandler);
				this._endHandlers.Add(endHandler);
				this._stateObjects.Add(state);
				this._count++;
			}

			// Token: 0x06000205 RID: 517 RVA: 0x0000A97C File Offset: 0x0000997C
			internal void CreateExecutionSteps(HttpApplication app, ArrayList steps)
			{
				for (int i = 0; i < this._count; i++)
				{
					steps.Add(new HttpApplication.AsyncEventExecutionStep(app, (BeginEventHandler)this._beginHandlers[i], (EndEventHandler)this._endHandlers[i], this._stateObjects[i]));
				}
			}

			// Token: 0x04000E26 RID: 3622
			private int _count;

			// Token: 0x04000E27 RID: 3623
			private ArrayList _beginHandlers;

			// Token: 0x04000E28 RID: 3624
			private ArrayList _endHandlers;

			// Token: 0x04000E29 RID: 3625
			private ArrayList _stateObjects;
		}

		// Token: 0x0200003E RID: 62
		internal class AsyncAppEventHandlersTable
		{
			// Token: 0x06000206 RID: 518 RVA: 0x0000A9D8 File Offset: 0x000099D8
			internal void AddHandler(object eventId, BeginEventHandler beginHandler, EndEventHandler endHandler, object state, RequestNotification requestNotification, bool isPost, HttpApplication app)
			{
				if (this._table == null)
				{
					this._table = new Hashtable();
				}
				HttpApplication.AsyncAppEventHandler asyncAppEventHandler = (HttpApplication.AsyncAppEventHandler)this._table[eventId];
				if (asyncAppEventHandler == null)
				{
					asyncAppEventHandler = new HttpApplication.AsyncAppEventHandler();
					this._table[eventId] = asyncAppEventHandler;
				}
				asyncAppEventHandler.Add(beginHandler, endHandler, state);
				if (HttpRuntime.UseIntegratedPipeline)
				{
					HttpApplication.AsyncEventExecutionStep asyncEventExecutionStep = new HttpApplication.AsyncEventExecutionStep(app, beginHandler, endHandler, state);
					app.AddEventMapping(app.CurrentModuleCollectionKey, requestNotification, isPost, asyncEventExecutionStep);
				}
			}

			// Token: 0x17000094 RID: 148
			internal HttpApplication.AsyncAppEventHandler this[object eventId]
			{
				get
				{
					if (this._table == null)
					{
						return null;
					}
					return (HttpApplication.AsyncAppEventHandler)this._table[eventId];
				}
			}

			// Token: 0x04000E2A RID: 3626
			private Hashtable _table;
		}

		// Token: 0x0200003F RID: 63
		internal interface IExecutionStep
		{
			// Token: 0x06000209 RID: 521
			void Execute();

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x0600020A RID: 522
			bool CompletedSynchronously { get; }

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x0600020B RID: 523
			bool IsCancellable { get; }
		}

		// Token: 0x02000040 RID: 64
		internal class NoopExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x0600020C RID: 524 RVA: 0x0000AA75 File Offset: 0x00009A75
			internal NoopExecutionStep()
			{
			}

			// Token: 0x0600020D RID: 525 RVA: 0x0000AA7D File Offset: 0x00009A7D
			void HttpApplication.IExecutionStep.Execute()
			{
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x0600020E RID: 526 RVA: 0x0000AA7F File Offset: 0x00009A7F
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x0600020F RID: 527 RVA: 0x0000AA82 File Offset: 0x00009A82
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}
		}

		// Token: 0x02000041 RID: 65
		internal class SyncEventExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000210 RID: 528 RVA: 0x0000AA85 File Offset: 0x00009A85
			internal SyncEventExecutionStep(HttpApplication app, EventHandler handler)
			{
				this._application = app;
				this._handler = handler;
			}

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x06000211 RID: 529 RVA: 0x0000AA9B File Offset: 0x00009A9B
			internal EventHandler Handler
			{
				get
				{
					return this._handler;
				}
			}

			// Token: 0x06000212 RID: 530 RVA: 0x0000AAA4 File Offset: 0x00009AA4
			void HttpApplication.IExecutionStep.Execute()
			{
				string text = null;
				if (this._handler != null)
				{
					if (EtwTrace.IsTraceEnabled(5, 2))
					{
						text = this._handler.Method.ReflectedType.ToString();
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_ENTER, this._application.Context.WorkerRequest, text);
					}
					this._handler(this._application, this._application.AppEvent);
					if (EtwTrace.IsTraceEnabled(5, 2))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_LEAVE, this._application.Context.WorkerRequest, text);
					}
				}
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x06000213 RID: 531 RVA: 0x0000AB2F File Offset: 0x00009B2F
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x06000214 RID: 532 RVA: 0x0000AB32 File Offset: 0x00009B32
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04000E2B RID: 3627
			private HttpApplication _application;

			// Token: 0x04000E2C RID: 3628
			private EventHandler _handler;
		}

		// Token: 0x02000042 RID: 66
		internal class AsyncEventExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000215 RID: 533 RVA: 0x0000AB35 File Offset: 0x00009B35
			internal AsyncEventExecutionStep(HttpApplication app, BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
				: this(app, beginHandler, endHandler, state, HttpRuntime.UseIntegratedPipeline)
			{
			}

			// Token: 0x06000216 RID: 534 RVA: 0x0000AB47 File Offset: 0x00009B47
			internal AsyncEventExecutionStep(HttpApplication app, BeginEventHandler beginHandler, EndEventHandler endHandler, object state, bool useIntegratedPipeline)
			{
				this._application = app;
				this._beginHandler = beginHandler;
				this._endHandler = endHandler;
				this._state = state;
				this._completionCallback = new AsyncCallback(this.OnAsyncEventCompletion);
			}

			// Token: 0x06000217 RID: 535 RVA: 0x0000AB80 File Offset: 0x00009B80
			private void OnAsyncEventCompletion(IAsyncResult ar)
			{
				if (ar.CompletedSynchronously)
				{
					return;
				}
				HttpContext context = this._application.Context;
				Exception ex = null;
				try
				{
					this._endHandler(ar);
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				if (EtwTrace.IsTraceEnabled(5, 2))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_LEAVE, context.WorkerRequest, this._targetTypeStr);
				}
				context.SetStartTime();
				this.ResumeStepsWithAssert(ex);
			}

			// Token: 0x06000218 RID: 536 RVA: 0x0000ABF0 File Offset: 0x00009BF0
			[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
			private void ResumeStepsWithAssert(Exception error)
			{
				this._application.ResumeStepsFromThreadPoolThread(error);
			}

			// Token: 0x06000219 RID: 537 RVA: 0x0000AC00 File Offset: 0x00009C00
			void HttpApplication.IExecutionStep.Execute()
			{
				this._sync = false;
				if (EtwTrace.IsTraceEnabled(5, 2))
				{
					this._targetTypeStr = this._beginHandler.Method.ReflectedType.ToString();
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_ENTER, this._application.Context.WorkerRequest, this._targetTypeStr);
				}
				IAsyncResult asyncResult = this._beginHandler(this._application, this._application.AppEvent, this._completionCallback, this._state);
				if (asyncResult.CompletedSynchronously)
				{
					this._sync = true;
					this._endHandler(asyncResult);
					if (EtwTrace.IsTraceEnabled(5, 2))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_LEAVE, this._application.Context.WorkerRequest, this._targetTypeStr);
					}
				}
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x0600021A RID: 538 RVA: 0x0000ACBF File Offset: 0x00009CBF
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return this._sync;
				}
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x0600021B RID: 539 RVA: 0x0000ACC7 File Offset: 0x00009CC7
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x04000E2D RID: 3629
			private HttpApplication _application;

			// Token: 0x04000E2E RID: 3630
			private BeginEventHandler _beginHandler;

			// Token: 0x04000E2F RID: 3631
			private EndEventHandler _endHandler;

			// Token: 0x04000E30 RID: 3632
			private object _state;

			// Token: 0x04000E31 RID: 3633
			private AsyncCallback _completionCallback;

			// Token: 0x04000E32 RID: 3634
			private bool _sync;

			// Token: 0x04000E33 RID: 3635
			private string _targetTypeStr;
		}

		// Token: 0x02000043 RID: 67
		internal class ValidatePathExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x0600021C RID: 540 RVA: 0x0000ACCA File Offset: 0x00009CCA
			internal ValidatePathExecutionStep(HttpApplication app)
			{
				this._application = app;
			}

			// Token: 0x0600021D RID: 541 RVA: 0x0000ACD9 File Offset: 0x00009CD9
			void HttpApplication.IExecutionStep.Execute()
			{
				this._application.Context.ValidatePath();
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x0600021E RID: 542 RVA: 0x0000ACEB File Offset: 0x00009CEB
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x0600021F RID: 543 RVA: 0x0000ACEE File Offset: 0x00009CEE
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x04000E34 RID: 3636
			private HttpApplication _application;
		}

		// Token: 0x02000044 RID: 68
		internal class MaterializeHandlerExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000220 RID: 544 RVA: 0x0000ACF1 File Offset: 0x00009CF1
			internal MaterializeHandlerExecutionStep(HttpApplication app)
			{
				this._application = app;
			}

			// Token: 0x06000221 RID: 545 RVA: 0x0000AD00 File Offset: 0x00009D00
			void HttpApplication.IExecutionStep.Execute()
			{
				HttpContext context = this._application.Context;
				HttpRequest request = context.Request;
				IHttpHandler httpHandler = null;
				string text = null;
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_MAPHANDLER_ENTER, context.WorkerRequest);
				}
				IIS7WorkerRequest iis7WorkerRequest = context.WorkerRequest as IIS7WorkerRequest;
				if (context.RemapHandlerInstance != null)
				{
					context.Handler = context.RemapHandlerInstance;
				}
				else if (request.RewrittenUrl != null)
				{
					bool flag;
					text = iis7WorkerRequest.ReMapHandlerAndGetHandlerTypeString(context, request.Path, out flag);
					if (!flag)
					{
						throw new HttpException(404, SR.GetString("Http_handler_not_found_for_request_type", new object[] { request.RequestType }));
					}
				}
				else
				{
					text = iis7WorkerRequest.GetManagedHandlerType();
				}
				if (!string.IsNullOrEmpty(text))
				{
					IHttpHandlerFactory factory = this._application.GetFactory(text);
					string physicalPathInternal = request.PhysicalPathInternal;
					try
					{
						httpHandler = factory.GetHandler(context, request.RequestType, request.FilePath, physicalPathInternal);
					}
					catch (FileNotFoundException ex)
					{
						if (HttpRuntime.HasPathDiscoveryPermission(physicalPathInternal))
						{
							throw new HttpException(404, null, ex);
						}
						throw new HttpException(404, null);
					}
					catch (DirectoryNotFoundException ex2)
					{
						if (HttpRuntime.HasPathDiscoveryPermission(physicalPathInternal))
						{
							throw new HttpException(404, null, ex2);
						}
						throw new HttpException(404, null);
					}
					catch (PathTooLongException ex3)
					{
						if (HttpRuntime.HasPathDiscoveryPermission(physicalPathInternal))
						{
							throw new HttpException(414, null, ex3);
						}
						throw new HttpException(414, null);
					}
					context.Handler = httpHandler;
					if (this._application._handlerRecycleList == null)
					{
						this._application._handlerRecycleList = new ArrayList();
					}
					this._application._handlerRecycleList.Add(new HandlerWithFactory(httpHandler, factory));
				}
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_MAPHANDLER_LEAVE, context.WorkerRequest);
				}
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x06000222 RID: 546 RVA: 0x0000AED0 File Offset: 0x00009ED0
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x06000223 RID: 547 RVA: 0x0000AED3 File Offset: 0x00009ED3
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x04000E35 RID: 3637
			private HttpApplication _application;
		}

		// Token: 0x02000045 RID: 69
		internal class MapHandlerExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000224 RID: 548 RVA: 0x0000AED6 File Offset: 0x00009ED6
			internal MapHandlerExecutionStep(HttpApplication app)
			{
				this._application = app;
			}

			// Token: 0x06000225 RID: 549 RVA: 0x0000AEE8 File Offset: 0x00009EE8
			void HttpApplication.IExecutionStep.Execute()
			{
				HttpContext context = this._application.Context;
				HttpRequest request = context.Request;
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_MAPHANDLER_ENTER, context.WorkerRequest);
				}
				context.Handler = this._application.MapHttpHandler(context, request.RequestType, request.FilePathObject, request.PhysicalPathInternal, false);
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_MAPHANDLER_LEAVE, context.WorkerRequest);
				}
			}

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x06000226 RID: 550 RVA: 0x0000AF59 File Offset: 0x00009F59
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000227 RID: 551 RVA: 0x0000AF5C File Offset: 0x00009F5C
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x04000E36 RID: 3638
			private HttpApplication _application;
		}

		// Token: 0x02000046 RID: 70
		internal class CallHandlerExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000228 RID: 552 RVA: 0x0000AF5F File Offset: 0x00009F5F
			internal CallHandlerExecutionStep(HttpApplication app)
			{
				this._application = app;
				this._completionCallback = new AsyncCallback(this.OnAsyncHandlerCompletion);
			}

			// Token: 0x06000229 RID: 553 RVA: 0x0000AF80 File Offset: 0x00009F80
			private void OnAsyncHandlerCompletion(IAsyncResult ar)
			{
				if (ar.CompletedSynchronously)
				{
					return;
				}
				HttpContext context = this._application.Context;
				Exception ex = null;
				try
				{
					try
					{
						this._handler.EndProcessRequest(ar);
					}
					finally
					{
						context.Response.GenerateResponseHeadersForHandler();
					}
				}
				catch (Exception ex2)
				{
					if (ex2 is ThreadAbortException || (ex2.InnerException != null && ex2.InnerException is ThreadAbortException))
					{
						this._application.CompleteRequest();
					}
					else
					{
						ex = ex2;
					}
				}
				if (EtwTrace.IsTraceEnabled(4, 4))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_HTTPHANDLER_LEAVE, context.WorkerRequest);
				}
				this._handler = null;
				context.SetStartTime();
				this.ResumeStepsWithAssert(ex);
			}

			// Token: 0x0600022A RID: 554 RVA: 0x0000B038 File Offset: 0x0000A038
			[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
			private void ResumeStepsWithAssert(Exception error)
			{
				this._application.ResumeStepsFromThreadPoolThread(error);
			}

			// Token: 0x0600022B RID: 555 RVA: 0x0000B048 File Offset: 0x0000A048
			void HttpApplication.IExecutionStep.Execute()
			{
				HttpContext context = this._application.Context;
				IHttpHandler handler = context.Handler;
				if (EtwTrace.IsTraceEnabled(4, 4))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_HTTPHANDLER_ENTER, context.WorkerRequest);
				}
				if (handler != null && HttpRuntime.UseIntegratedPipeline)
				{
					IIS7WorkerRequest iis7WorkerRequest = context.WorkerRequest as IIS7WorkerRequest;
					if (iis7WorkerRequest != null && iis7WorkerRequest.IsHandlerExecutionDenied())
					{
						this._sync = true;
						HttpException ex = new HttpException(403, SR.GetString("Handler_access_denied"));
						ex.SetFormatter(new PageForbiddenErrorFormatter(context.Request.Path, SR.GetString("Handler_access_denied")));
						throw ex;
					}
				}
				if (handler == null)
				{
					this._sync = true;
					return;
				}
				if (handler is IHttpAsyncHandler)
				{
					IHttpAsyncHandler httpAsyncHandler = (IHttpAsyncHandler)handler;
					this._sync = false;
					this._handler = httpAsyncHandler;
					IAsyncResult asyncResult = httpAsyncHandler.BeginProcessRequest(context, this._completionCallback, null);
					if (asyncResult.CompletedSynchronously)
					{
						this._sync = true;
						this._handler = null;
						try
						{
							httpAsyncHandler.EndProcessRequest(asyncResult);
						}
						finally
						{
							context.Response.GenerateResponseHeadersForHandler();
						}
						if (EtwTrace.IsTraceEnabled(4, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_HTTPHANDLER_LEAVE, context.WorkerRequest);
							return;
						}
					}
				}
				else
				{
					this._sync = true;
					context.SyncContext.SetSyncCaller();
					try
					{
						handler.ProcessRequest(context);
					}
					finally
					{
						context.SyncContext.ResetSyncCaller();
						if (EtwTrace.IsTraceEnabled(4, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_HTTPHANDLER_LEAVE, context.WorkerRequest);
						}
						context.Response.GenerateResponseHeadersForHandler();
					}
				}
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x0600022C RID: 556 RVA: 0x0000B1C4 File Offset: 0x0000A1C4
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return this._sync;
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x0600022D RID: 557 RVA: 0x0000B1CC File Offset: 0x0000A1CC
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return !(this._application.Context.Handler is IHttpAsyncHandler);
				}
			}

			// Token: 0x04000E37 RID: 3639
			private HttpApplication _application;

			// Token: 0x04000E38 RID: 3640
			private AsyncCallback _completionCallback;

			// Token: 0x04000E39 RID: 3641
			private IHttpAsyncHandler _handler;

			// Token: 0x04000E3A RID: 3642
			private bool _sync;
		}

		// Token: 0x02000047 RID: 71
		internal class CallFilterExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x0600022E RID: 558 RVA: 0x0000B1E8 File Offset: 0x0000A1E8
			internal CallFilterExecutionStep(HttpApplication app)
			{
				this._application = app;
			}

			// Token: 0x0600022F RID: 559 RVA: 0x0000B1F8 File Offset: 0x0000A1F8
			void HttpApplication.IExecutionStep.Execute()
			{
				try
				{
					this._application.Context.Response.FilterOutput();
				}
				finally
				{
					if (HttpRuntime.UseIntegratedPipeline && this._application.Context.CurrentNotification == RequestNotification.UpdateRequestCache)
					{
						this._application.Context.DisableNotifications(RequestNotification.LogRequest, (RequestNotification)0);
					}
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B264 File Offset: 0x0000A264
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000231 RID: 561 RVA: 0x0000B267 File Offset: 0x0000A267
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04000E3B RID: 3643
			private HttpApplication _application;
		}

		// Token: 0x02000048 RID: 72
		internal class SendResponseExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000232 RID: 562 RVA: 0x0000B26A File Offset: 0x0000A26A
			internal SendResponseExecutionStep(HttpApplication app, EventHandler handler, bool isHeaders)
			{
				this._application = app;
				this._handler = handler;
				this._isHeaders = isHeaders;
			}

			// Token: 0x06000233 RID: 563 RVA: 0x0000B288 File Offset: 0x0000A288
			void HttpApplication.IExecutionStep.Execute()
			{
				if ((this._application.Context.IsSendResponseHeaders && this._isHeaders) || !this._isHeaders)
				{
					string text = null;
					if (this._handler != null)
					{
						if (EtwTrace.IsTraceEnabled(5, 2))
						{
							text = this._handler.Method.ReflectedType.ToString();
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_ENTER, this._application.Context.WorkerRequest, text);
						}
						this._handler(this._application, this._application.AppEvent);
						if (EtwTrace.IsTraceEnabled(5, 2))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PIPELINE_LEAVE, this._application.Context.WorkerRequest, text);
						}
					}
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x06000234 RID: 564 RVA: 0x0000B335 File Offset: 0x0000A335
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x06000235 RID: 565 RVA: 0x0000B338 File Offset: 0x0000A338
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04000E3C RID: 3644
			private HttpApplication _application;

			// Token: 0x04000E3D RID: 3645
			private EventHandler _handler;

			// Token: 0x04000E3E RID: 3646
			private bool _isHeaders;
		}

		// Token: 0x02000049 RID: 73
		internal class UrlMappingsExecutionStep : HttpApplication.IExecutionStep
		{
			// Token: 0x06000236 RID: 566 RVA: 0x0000B33B File Offset: 0x0000A33B
			internal UrlMappingsExecutionStep(HttpApplication app)
			{
				this._application = app;
			}

			// Token: 0x06000237 RID: 567 RVA: 0x0000B34C File Offset: 0x0000A34C
			void HttpApplication.IExecutionStep.Execute()
			{
				HttpContext context = this._application.Context;
				HttpRequest request = context.Request;
				UrlMappingsSection urlMappings = RuntimeConfig.GetAppConfig().UrlMappings;
				string text = urlMappings.HttpResolveMapping(request.RawUrl);
				if (text == null)
				{
					text = urlMappings.HttpResolveMapping(request.Path);
				}
				if (!string.IsNullOrEmpty(text))
				{
					context.RewritePath(text, false);
				}
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B3A4 File Offset: 0x0000A3A4
			bool HttpApplication.IExecutionStep.CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000239 RID: 569 RVA: 0x0000B3A7 File Offset: 0x0000A3A7
			bool HttpApplication.IExecutionStep.IsCancellable
			{
				get
				{
					return false;
				}
			}

			// Token: 0x04000E3F RID: 3647
			private HttpApplication _application;
		}

		// Token: 0x0200004A RID: 74
		internal abstract class StepManager
		{
			// Token: 0x0600023A RID: 570 RVA: 0x0000B3AA File Offset: 0x0000A3AA
			internal StepManager(HttpApplication application)
			{
				this._application = application;
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B3B9 File Offset: 0x0000A3B9
			internal bool IsCompleted
			{
				get
				{
					return this._requestCompleted;
				}
			}

			// Token: 0x0600023C RID: 572
			internal abstract void BuildSteps(WaitCallback stepCallback);

			// Token: 0x0600023D RID: 573 RVA: 0x0000B3C4 File Offset: 0x0000A3C4
			internal void CompleteRequest()
			{
				this._requestCompleted = true;
				if (HttpRuntime.UseIntegratedPipeline)
				{
					HttpContext context = this._application.Context;
					if (context != null && context.NotificationContext != null)
					{
						context.NotificationContext.RequestCompleted = true;
					}
				}
			}

			// Token: 0x0600023E RID: 574
			internal abstract void InitRequest();

			// Token: 0x0600023F RID: 575
			internal abstract void ResumeSteps(Exception error);

			// Token: 0x04000E40 RID: 3648
			protected HttpApplication _application;

			// Token: 0x04000E41 RID: 3649
			protected bool _requestCompleted;
		}

		// Token: 0x0200004B RID: 75
		internal class ApplicationStepManager : HttpApplication.StepManager
		{
			// Token: 0x06000240 RID: 576 RVA: 0x0000B402 File Offset: 0x0000A402
			internal ApplicationStepManager(HttpApplication app)
				: base(app)
			{
			}

			// Token: 0x06000241 RID: 577 RVA: 0x0000B40C File Offset: 0x0000A40C
			internal override void BuildSteps(WaitCallback stepCallback)
			{
				ArrayList arrayList = new ArrayList();
				HttpApplication application = this._application;
				UrlMappingsSection urlMappings = RuntimeConfig.GetConfig().UrlMappings;
				bool flag = urlMappings.IsEnabled && urlMappings.UrlMappings.Count > 0;
				arrayList.Add(new HttpApplication.ValidatePathExecutionStep(application));
				if (flag)
				{
					arrayList.Add(new HttpApplication.UrlMappingsExecutionStep(application));
				}
				application.CreateEventExecutionSteps(HttpApplication.EventBeginRequest, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventAuthenticateRequest, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventDefaultAuthentication, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostAuthenticateRequest, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventAuthorizeRequest, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostAuthorizeRequest, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventResolveRequestCache, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostResolveRequestCache, arrayList);
				arrayList.Add(new HttpApplication.MapHandlerExecutionStep(application));
				application.CreateEventExecutionSteps(HttpApplication.EventPostMapRequestHandler, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventAcquireRequestState, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostAcquireRequestState, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPreRequestHandlerExecute, arrayList);
				arrayList.Add(new HttpApplication.CallHandlerExecutionStep(application));
				application.CreateEventExecutionSteps(HttpApplication.EventPostRequestHandlerExecute, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventReleaseRequestState, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostReleaseRequestState, arrayList);
				arrayList.Add(new HttpApplication.CallFilterExecutionStep(application));
				application.CreateEventExecutionSteps(HttpApplication.EventUpdateRequestCache, arrayList);
				application.CreateEventExecutionSteps(HttpApplication.EventPostUpdateRequestCache, arrayList);
				this._endRequestStepIndex = arrayList.Count;
				application.CreateEventExecutionSteps(HttpApplication.EventEndRequest, arrayList);
				arrayList.Add(new HttpApplication.NoopExecutionStep());
				this._execSteps = new HttpApplication.IExecutionStep[arrayList.Count];
				arrayList.CopyTo(this._execSteps);
				this._resumeStepsWaitCallback = stepCallback;
			}

			// Token: 0x06000242 RID: 578 RVA: 0x0000B5A5 File Offset: 0x0000A5A5
			internal override void InitRequest()
			{
				this._currentStepIndex = -1;
				this._numStepCalls = 0;
				this._numSyncStepCalls = 0;
				this._requestCompleted = false;
			}

			// Token: 0x06000243 RID: 579 RVA: 0x0000B5C4 File Offset: 0x0000A5C4
			[DebuggerStepperBoundary]
			internal override void ResumeSteps(Exception error)
			{
				bool flag = false;
				bool flag2 = true;
				HttpApplication application = this._application;
				HttpContext context = application.Context;
				HttpApplication.ThreadContext threadContext = null;
				AspNetSynchronizationContext syncContext = context.SyncContext;
				lock (this._application)
				{
					try
					{
						threadContext = application.OnThreadEnter();
						goto IL_0040;
					}
					catch (Exception ex)
					{
						if (error == null)
						{
							error = ex;
							goto IL_003E;
						}
						goto IL_003E;
						IL_003E:
						goto IL_0040;
					}
					try
					{
						try
						{
							for (;;)
							{
								IL_0040:
								if (syncContext.Error != null)
								{
									error = syncContext.Error;
									syncContext.ClearError();
								}
								if (error != null)
								{
									application.RecordError(error);
									error = null;
								}
								if (syncContext.PendingOperationsCount > 0)
								{
									break;
								}
								if (this._currentStepIndex < this._endRequestStepIndex && (context.Error != null || this._requestCompleted))
								{
									context.Response.FilterOutput();
									this._currentStepIndex = this._endRequestStepIndex;
								}
								else
								{
									this._currentStepIndex++;
								}
								if (this._currentStepIndex >= this._execSteps.Length)
								{
									goto Block_14;
								}
								this._numStepCalls++;
								context.SyncContext.Enable();
								error = application.ExecuteStep(this._execSteps[this._currentStepIndex], ref flag2);
								if (!flag2)
								{
									goto IL_0121;
								}
								this._numSyncStepCalls++;
							}
							syncContext.SetLastCompletionWorkItem(this._resumeStepsWaitCallback);
							goto IL_0121;
							Block_14:
							flag = true;
							IL_0121:;
						}
						finally
						{
							if (threadContext != null)
							{
								try
								{
									threadContext.Leave();
								}
								catch
								{
								}
							}
						}
					}
					catch
					{
						throw;
					}
				}
				if (flag)
				{
					context.Unroot();
					application.AsyncResult.Complete(this._numStepCalls == this._numSyncStepCalls, null, null);
					application.ReleaseAppInstance();
				}
			}

			// Token: 0x04000E42 RID: 3650
			private HttpApplication.IExecutionStep[] _execSteps;

			// Token: 0x04000E43 RID: 3651
			private WaitCallback _resumeStepsWaitCallback;

			// Token: 0x04000E44 RID: 3652
			private int _currentStepIndex;

			// Token: 0x04000E45 RID: 3653
			private int _numStepCalls;

			// Token: 0x04000E46 RID: 3654
			private int _numSyncStepCalls;

			// Token: 0x04000E47 RID: 3655
			private int _endRequestStepIndex;
		}

		// Token: 0x0200004C RID: 76
		internal class PipelineStepManager : HttpApplication.StepManager
		{
			// Token: 0x06000244 RID: 580 RVA: 0x0000B7BC File Offset: 0x0000A7BC
			internal PipelineStepManager(HttpApplication app)
				: base(app)
			{
			}

			// Token: 0x06000245 RID: 581 RVA: 0x0000B7C8 File Offset: 0x0000A7C8
			internal override void BuildSteps(WaitCallback stepCallback)
			{
				HttpApplication application = this._application;
				HttpApplication.IExecutionStep executionStep = new HttpApplication.MaterializeHandlerExecutionStep(application);
				application.AddEventMapping("ManagedPipelineHandler", RequestNotification.MapRequestHandler, false, executionStep);
				HttpApplication.IExecutionStep executionStep2 = new HttpApplication.CallHandlerExecutionStep(application);
				application.AddEventMapping("ManagedPipelineHandler", RequestNotification.ExecuteRequestHandler, false, executionStep2);
				HttpApplication.IExecutionStep executionStep3 = new HttpApplication.CallFilterExecutionStep(application);
				application.AddEventMapping("AspNetFilterModule", RequestNotification.UpdateRequestCache, false, executionStep3);
				application.AddEventMapping("AspNetFilterModule", RequestNotification.LogRequest, false, executionStep3);
				this._resumeStepsWaitCallback = stepCallback;
			}

			// Token: 0x06000246 RID: 582 RVA: 0x0000B83D File Offset: 0x0000A83D
			internal override void InitRequest()
			{
				this._requestCompleted = false;
			}

			// Token: 0x06000247 RID: 583 RVA: 0x0000B848 File Offset: 0x0000A848
			[DebuggerStepperBoundary]
			internal override void ResumeSteps(Exception error)
			{
				HttpContext context = this._application.Context;
				IIS7WorkerRequest iis7WorkerRequest = context.WorkerRequest as IIS7WorkerRequest;
				AspNetSynchronizationContext syncContext = context.SyncContext;
				RequestNotificationStatus requestNotificationStatus = RequestNotificationStatus.Continue;
				HttpApplication.ThreadContext threadContext = null;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				int num = this._application.CurrentModuleContainer.GetEventCount(context.CurrentNotification, context.IsPostNotification) - 1;
				bool isReEntry = context.NotificationContext.IsReEntry;
				if (!isReEntry)
				{
					Monitor.Enter(this._application);
				}
				try
				{
					bool flag4 = false;
					try
					{
						if (!isReEntry)
						{
							if (context.InIndicateCompletion)
							{
								threadContext = context.IndicateCompletionContext;
								if (context.UsesImpersonation)
								{
									threadContext.SetImpersonationContext();
								}
							}
							else
							{
								threadContext = this._application.OnThreadEnter(context.UsesImpersonation);
							}
						}
						context.FirstNotificationInit();
						for (;;)
						{
							if (syncContext.Error != null)
							{
								error = syncContext.Error;
								syncContext.ClearError();
							}
							if (error != null)
							{
								this._application.RecordError(error);
								error = null;
							}
							if (syncContext.PendingOperationsCount > 0)
							{
								break;
							}
							bool flag5 = (context.NotificationContext.Error != null || context.NotificationContext.RequestCompleted) && context.CurrentNotification != RequestNotification.LogRequest && context.CurrentNotification != RequestNotification.EndRequest;
							if (flag5 || context.CurrentModuleEventIndex == num)
							{
								requestNotificationStatus = (flag5 ? RequestNotificationStatus.FinishRequest : RequestNotificationStatus.Continue);
								if (context.NotificationContext.PendingAsyncCompletion)
								{
									goto Block_16;
								}
								if (flag5 || UnsafeIISMethods.MgdGetNextNotification(iis7WorkerRequest.RequestContext, RequestNotificationStatus.Continue) != 1)
								{
									goto IL_0194;
								}
								context.CurrentModuleIndex = UnsafeIISMethods.MgdGetCurrentModuleIndex(iis7WorkerRequest.RequestContext);
								context.IsPostNotification = UnsafeIISMethods.MgdIsCurrentNotificationPost(iis7WorkerRequest.RequestContext);
								context.CurrentNotification = (RequestNotification)UnsafeIISMethods.MgdGetCurrentNotification(iis7WorkerRequest.RequestContext);
								context.CurrentModuleEventIndex = -1;
								num = this._application.CurrentModuleContainer.GetEventCount(context.CurrentNotification, context.IsPostNotification) - 1;
							}
							context.CurrentModuleEventIndex++;
							HttpApplication.IExecutionStep nextEvent = this._application.CurrentModuleContainer.GetNextEvent(context.CurrentNotification, context.IsPostNotification, context.CurrentModuleEventIndex);
							context.SyncContext.Enable();
							flag3 = false;
							error = this._application.ExecuteStep(nextEvent, ref flag3);
							if (!flag3)
							{
								goto Block_18;
							}
							context.Response.SyncStatusIntegrated();
						}
						context.NotificationContext.PendingAsyncCompletion = true;
						syncContext.SetLastCompletionWorkItem(this._resumeStepsWaitCallback);
						goto IL_0279;
						Block_16:
						context.Response.SyncStatusIntegrated();
						context.NotificationContext.PendingAsyncCompletion = false;
						flag = false;
						flag2 = true;
						goto IL_0279;
						IL_0194:
						flag = true;
						flag2 = true;
						goto IL_0279;
						Block_18:
						this._application.AcquireNotifcationContextLock(ref flag4);
						context.NotificationContext.PendingAsyncCompletion = true;
						IL_0279:;
					}
					finally
					{
						if (flag4)
						{
							this._application.ReleaseNotifcationContextLock();
						}
						if (threadContext != null)
						{
							if (context.InIndicateCompletion)
							{
								if (flag)
								{
									threadContext.Synchronize(context);
									threadContext.UndoImpersonationContext();
									goto IL_0318;
								}
								if (threadContext.HasLeaveBeenCalled)
								{
									goto IL_0318;
								}
								lock (threadContext)
								{
									if (!threadContext.HasLeaveBeenCalled)
									{
										threadContext.Leave();
										context.IndicateCompletionContext = null;
										context.InIndicateCompletion = false;
									}
									goto IL_0318;
								}
							}
							if (flag)
							{
								threadContext.Synchronize(context);
								context.IndicateCompletionContext = threadContext;
								threadContext.UndoImpersonationContext();
							}
							else
							{
								threadContext.Leave();
							}
						}
						IL_0318:;
					}
					if (flag2)
					{
						this._application.AsyncResult.Complete(flag, null, null, requestNotificationStatus);
					}
				}
				finally
				{
					if (!isReEntry)
					{
						Monitor.Exit(this._application);
					}
				}
			}

			// Token: 0x04000E48 RID: 3656
			private WaitCallback _resumeStepsWaitCallback;
		}
	}
}
