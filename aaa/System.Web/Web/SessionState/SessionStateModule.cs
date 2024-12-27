using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Threading;
using System.Web.Configuration;
using System.Web.Util;
using Microsoft.Win32;

namespace System.Web.SessionState
{
	// Token: 0x02000375 RID: 885
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SessionStateModule : IHttpModule
	{
		// Token: 0x06002ADD RID: 10973 RVA: 0x000BD665 File Offset: 0x000BC665
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public SessionStateModule()
		{
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000BD678 File Offset: 0x000BC678
		private static bool CheckTrustLevel(SessionStateSection config)
		{
			switch (config.Mode)
			{
			case SessionStateMode.StateServer:
			case SessionStateMode.SQLServer:
				return HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium);
			}
			return true;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000BD6B0 File Offset: 0x000BC6B0
		private SessionStateStoreProviderBase InitCustomStore(SessionStateSection config)
		{
			string customProvider = config.CustomProvider;
			if (string.IsNullOrEmpty(customProvider))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_session_custom_provider", new object[] { customProvider }), config.ElementInformation.Properties["customProvider"].Source, config.ElementInformation.Properties["customProvider"].LineNumber);
			}
			ProviderSettings providerSettings = config.Providers[customProvider];
			if (providerSettings == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Missing_session_custom_provider", new object[] { customProvider }), config.ElementInformation.Properties["customProvider"].Source, config.ElementInformation.Properties["customProvider"].LineNumber);
			}
			return (SessionStateStoreProviderBase)ProvidersHelper.InstantiateProvider(providerSettings, typeof(SessionStateStoreProviderBase));
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000BD794 File Offset: 0x000BC794
		private IPartitionResolver InitPartitionResolver(SessionStateSection config)
		{
			string partitionResolverType = config.PartitionResolverType;
			if (string.IsNullOrEmpty(partitionResolverType))
			{
				return null;
			}
			if (config.Mode != SessionStateMode.StateServer && config.Mode != SessionStateMode.SQLServer)
			{
				throw new ConfigurationErrorsException(SR.GetString("Cant_use_partition_resolve"), config.ElementInformation.Properties["partitionResolverType"].Source, config.ElementInformation.Properties["partitionResolverType"].LineNumber);
			}
			Type type = ConfigUtil.GetType(partitionResolverType, "partitionResolverType", config);
			ConfigUtil.CheckAssignableType(typeof(IPartitionResolver), type, config, "partitionResolverType");
			IPartitionResolver partitionResolver = (IPartitionResolver)HttpRuntime.CreatePublicInstance(type);
			partitionResolver.Initialize();
			return partitionResolver;
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000BD840 File Offset: 0x000BC840
		private ISessionIDManager InitSessionIDManager(SessionStateSection config)
		{
			string sessionIDManagerType = config.SessionIDManagerType;
			ISessionIDManager sessionIDManager;
			if (string.IsNullOrEmpty(sessionIDManagerType))
			{
				sessionIDManager = new SessionIDManager();
				this._usingAspnetSessionIdManager = true;
			}
			else
			{
				Type type = ConfigUtil.GetType(sessionIDManagerType, "sessionIDManagerType", config);
				ConfigUtil.CheckAssignableType(typeof(ISessionIDManager), type, config, "sessionIDManagerType");
				sessionIDManager = (ISessionIDManager)HttpRuntime.CreatePublicInstance(type);
			}
			sessionIDManager.Initialize();
			return sessionIDManager;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000BD8A4 File Offset: 0x000BC8A4
		private void InitModuleFromConfig(HttpApplication app, SessionStateSection config)
		{
			if (config.Mode == SessionStateMode.Off)
			{
				return;
			}
			app.AddOnAcquireRequestStateAsync(new BeginEventHandler(this.BeginAcquireState), new EndEventHandler(this.EndAcquireState));
			app.ReleaseRequestState += this.OnReleaseState;
			app.EndRequest += this.OnEndRequest;
			this._partitionResolver = this.InitPartitionResolver(config);
			switch (config.Mode)
			{
			case SessionStateMode.InProc:
				if (HttpRuntime.UseIntegratedPipeline)
				{
					SessionStateModule.s_canSkipEndRequestCall = true;
				}
				this._store = new InProcSessionStateStore();
				this._store.Initialize(null, null);
				break;
			case SessionStateMode.StateServer:
				if (HttpRuntime.UseIntegratedPipeline)
				{
					SessionStateModule.s_canSkipEndRequestCall = true;
				}
				this._store = new OutOfProcSessionStateStore();
				((OutOfProcSessionStateStore)this._store).Initialize(null, null, this._partitionResolver);
				break;
			case SessionStateMode.SQLServer:
				this._store = new SqlSessionStateStore();
				((SqlSessionStateStore)this._store).Initialize(null, null, this._partitionResolver);
				break;
			case SessionStateMode.Custom:
				this._store = this.InitCustomStore(config);
				break;
			}
			this._idManager = this.InitSessionIDManager(config);
			if ((config.Mode == SessionStateMode.InProc || config.Mode == SessionStateMode.StateServer) && this._usingAspnetSessionIdManager)
			{
				this._ignoreImpersonation = true;
			}
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000BD9E8 File Offset: 0x000BC9E8
		private void InitSessionIDSalt()
		{
			if (!SessionStateModule.IsRequestQueueEnabled || SessionStateModule.s_sessionIDSalt != null)
			{
				return;
			}
			try
			{
				RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
				byte[] array = new byte[8];
				rngcryptoServiceProvider.GetBytes(array);
				SessionStateModule.s_sessionIDSalt = Convert.ToBase64String(array);
			}
			catch (CryptographicException)
			{
				SessionStateModule.s_sessionIDSalt = string.Empty;
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000BDA44 File Offset: 0x000BCA44
		public void Init(HttpApplication app)
		{
			bool flag = false;
			SessionStateSection sessionState = RuntimeConfig.GetAppConfig().SessionState;
			if (!this.s_oneTimeInit)
			{
				SessionStateModule.s_lock.AcquireWriterLock();
				try
				{
					if (!this.s_oneTimeInit)
					{
						this.InitModuleFromConfig(app, sessionState);
						flag = true;
						if (!SessionStateModule.CheckTrustLevel(sessionState))
						{
							SessionStateModule.s_trustLevelInsufficient = true;
						}
						SessionStateModule.s_timeout = (int)sessionState.Timeout.TotalMinutes;
						SessionStateModule.s_useHostingIdentity = sessionState.UseHostingIdentity;
						if (sessionState.Mode == SessionStateMode.InProc && this._usingAspnetSessionIdManager)
						{
							SessionStateModule.s_allowInProcOptimization = true;
						}
						if (sessionState.Mode != SessionStateMode.Custom && sessionState.Mode != SessionStateMode.Off && !sessionState.RegenerateExpiredSessionId)
						{
							SessionStateModule.s_allowDelayedStateStoreItemCreation = true;
						}
						SessionStateModule.s_configExecutionTimeout = RuntimeConfig.GetConfig().HttpRuntime.ExecutionTimeout;
						SessionStateModule.s_configRegenerateExpiredSessionId = sessionState.RegenerateExpiredSessionId;
						SessionStateModule.s_configCookieless = sessionState.Cookieless;
						SessionStateModule.s_configMode = sessionState.Mode;
						this.s_oneTimeInit = true;
						this.InitSessionIDSalt();
					}
				}
				finally
				{
					SessionStateModule.s_lock.ReleaseWriterLock();
				}
			}
			if (!flag)
			{
				this.InitModuleFromConfig(app, sessionState);
			}
			if (SessionStateModule.s_trustLevelInsufficient)
			{
				throw new HttpException(SR.GetString("Session_state_need_higher_trust"));
			}
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000BDB6C File Offset: 0x000BCB6C
		public void Dispose()
		{
			if (this._timer != null)
			{
				((IDisposable)this._timer).Dispose();
			}
			if (this._store != null)
			{
				this._store.Dispose();
			}
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000BDB94 File Offset: 0x000BCB94
		private void ResetPerRequestFields()
		{
			this._rqSessionState = null;
			this._rqId = null;
			this._rqSessionItems = null;
			this._rqStaticObjects = null;
			this._rqIsNewSession = false;
			this._rqSessionStateNotFound = true;
			this._rqReadonly = false;
			this._rqItem = null;
			this._rqContext = null;
			this._rqAr = null;
			this._rqLockId = null;
			this._rqInCallback = 0;
			this._rqLastPollCompleted = DateTime.MinValue;
			this._rqExecutionTimeout = TimeSpan.Zero;
			this._rqAddedCookie = false;
			this._rqIdNew = false;
			this._rqActionFlags = SessionStateActions.None;
			this._rqIctx = null;
			this._rqChangeImpersonationRefCount = 0;
			this._rqTimerThreadImpersonationIctx = null;
			this._rqSupportSessionIdReissue = false;
			this._rqUninitializedItem = false;
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002AE7 RID: 10983 RVA: 0x000BDC43 File Offset: 0x000BCC43
		// (remove) Token: 0x06002AE8 RID: 10984 RVA: 0x000BDC5C File Offset: 0x000BCC5C
		public event EventHandler Start
		{
			add
			{
				this._sessionStartEventHandler = (EventHandler)Delegate.Combine(this._sessionStartEventHandler, value);
			}
			remove
			{
				this._sessionStartEventHandler = (EventHandler)Delegate.Remove(this._sessionStartEventHandler, value);
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000BDC78 File Offset: 0x000BCC78
		private void RaiseOnStart(EventArgs e)
		{
			if (this._sessionStartEventHandler == null)
			{
				return;
			}
			if (HttpRuntime.ApartmentThreading || this._rqContext.InAspCompatMode)
			{
				AspCompatApplicationStep.RaiseAspCompatEvent(this._rqContext, this._rqContext.ApplicationInstance, null, this._sessionStartEventHandler, this, e);
				return;
			}
			if (HttpContext.Current == null)
			{
				HttpContextWrapper.SwitchContext(this._rqContext);
			}
			this._sessionStartEventHandler(this, e);
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000BDCE2 File Offset: 0x000BCCE2
		private void OnStart(EventArgs e)
		{
			this.RaiseOnStart(e);
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002AEB RID: 10987 RVA: 0x000BDCEC File Offset: 0x000BCCEC
		// (remove) Token: 0x06002AEC RID: 10988 RVA: 0x000BDD6C File Offset: 0x000BCD6C
		public event EventHandler End
		{
			add
			{
				lock (this._onEndTarget)
				{
					if (this._store != null && this._onEndTarget.SessionEndEventHandlerCount == 0)
					{
						this._supportSessionExpiry = this._store.SetItemExpireCallback(new SessionStateItemExpireCallback(this._onEndTarget.RaiseSessionOnEnd));
					}
					this._onEndTarget.SessionEndEventHandlerCount++;
				}
			}
			remove
			{
				lock (this._onEndTarget)
				{
					this._onEndTarget.SessionEndEventHandlerCount--;
					if (this._store != null && this._onEndTarget.SessionEndEventHandlerCount == 0)
					{
						this._store.SetItemExpireCallback(null);
						this._supportSessionExpiry = false;
					}
				}
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000BDDDC File Offset: 0x000BCDDC
		private IAsyncResult BeginAcquireState(object source, EventArgs e, AsyncCallback cb, object extraData)
		{
			bool flag = true;
			bool flag2 = false;
			this._acquireCalled = true;
			this._releaseCalled = false;
			this.ResetPerRequestFields();
			this._rqContext = ((HttpApplication)source).Context;
			this._rqAr = new HttpAsyncResult(cb, extraData);
			this.ChangeImpersonation(this._rqContext, false);
			IAsyncResult asyncResult;
			try
			{
				if (EtwTrace.IsTraceEnabled(4, 8))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSION_DATA_BEGIN, this._rqContext.WorkerRequest);
				}
				this._store.InitializeRequest(this._rqContext);
				bool requiresSessionState = this._rqContext.RequiresSessionState;
				if (this._idManager.InitializeRequest(this._rqContext, false, out this._rqSupportSessionIdReissue))
				{
					this._rqAr.Complete(true, null, null);
					if (EtwTrace.IsTraceEnabled(4, 8))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSION_DATA_END, this._rqContext.WorkerRequest);
					}
					asyncResult = this._rqAr;
				}
				else
				{
					if (SessionStateModule.s_allowInProcOptimization && !SessionStateModule.s_sessionEverSet && (!requiresSessionState || !((SessionIDManager)this._idManager).UseCookieless(this._rqContext)))
					{
						flag2 = true;
					}
					else
					{
						this._rqId = this._idManager.GetSessionID(this._rqContext);
					}
					if (!requiresSessionState)
					{
						if (this._rqId != null)
						{
							this._store.ResetItemTimeout(this._rqContext, this._rqId);
						}
						this._rqAr.Complete(true, null, null);
						if (EtwTrace.IsTraceEnabled(4, 8))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSION_DATA_END, this._rqContext.WorkerRequest);
						}
						asyncResult = this._rqAr;
					}
					else
					{
						this._rqExecutionTimeout = this._rqContext.Timeout;
						if (this._rqExecutionTimeout == SessionStateModule.DEFAULT_DBG_EXECUTION_TIMEOUT)
						{
							this._rqExecutionTimeout = SessionStateModule.s_configExecutionTimeout;
						}
						this._rqReadonly = this._rqContext.ReadOnlySessionState;
						if (this._rqId != null)
						{
							flag = this.GetSessionStateItem();
						}
						else if (!flag2)
						{
							bool flag3 = this.CreateSessionId();
							this._rqIdNew = true;
							if (flag3)
							{
								if (SessionStateModule.s_configRegenerateExpiredSessionId)
								{
									this.CreateUninitializedSessionState();
								}
								this._rqAr.Complete(true, null, null);
								if (EtwTrace.IsTraceEnabled(4, 8))
								{
									EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSION_DATA_END, this._rqContext.WorkerRequest);
								}
								return this._rqAr;
							}
						}
						if (flag)
						{
							this.CompleteAcquireState();
							this._rqAr.Complete(true, null, null);
						}
						asyncResult = this._rqAr;
					}
				}
			}
			finally
			{
				this.RestoreImpersonation();
			}
			return asyncResult;
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000BE03C File Offset: 0x000BD03C
		internal bool CreateSessionId()
		{
			this._rqId = this._idManager.CreateSessionID(this._rqContext);
			bool flag;
			this._idManager.SaveSessionID(this._rqContext, this._rqId, out flag, out this._rqAddedCookie);
			return flag;
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000BE080 File Offset: 0x000BD080
		private void CompleteAcquireState()
		{
			bool flag = false;
			try
			{
				if (this._rqItem != null)
				{
					this._rqSessionStateNotFound = false;
					if ((this._rqActionFlags & SessionStateActions.InitializeItem) != SessionStateActions.None)
					{
						this._rqIsNewSession = true;
					}
					else
					{
						this._rqIsNewSession = false;
					}
				}
				else
				{
					this._rqIsNewSession = true;
					this._rqSessionStateNotFound = true;
					if (SessionStateModule.s_allowDelayedStateStoreItemCreation)
					{
						flag = true;
					}
					if (!this._rqIdNew && SessionStateModule.s_configRegenerateExpiredSessionId && this._rqSupportSessionIdReissue)
					{
						bool flag2 = this.CreateSessionId();
						if (flag2)
						{
							this.CreateUninitializedSessionState();
							return;
						}
					}
				}
				if (flag)
				{
					SessionStateUtility.AddDelayedHttpSessionStateToContext(this._rqContext, this);
					this._rqSessionState = SessionStateModule.s_delayedSessionState;
				}
				else
				{
					this.InitStateStoreItem(true);
				}
				if (this._rqIsNewSession)
				{
					this.OnStart(EventArgs.Empty);
				}
			}
			finally
			{
				if (EtwTrace.IsTraceEnabled(4, 8))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_SESSION_DATA_END, this._rqContext.WorkerRequest);
				}
			}
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x000BE160 File Offset: 0x000BD160
		private void CreateUninitializedSessionState()
		{
			this._store.CreateUninitializedItem(this._rqContext, this._rqId, SessionStateModule.s_timeout);
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000BE180 File Offset: 0x000BD180
		internal void InitStateStoreItem(bool addToContext)
		{
			this.ChangeImpersonation(this._rqContext, false);
			try
			{
				if (this._rqItem == null)
				{
					this._rqItem = this._store.CreateNewStoreData(this._rqContext, SessionStateModule.s_timeout);
				}
				this._rqSessionItems = this._rqItem.Items;
				if (this._rqSessionItems == null)
				{
					throw new HttpException(SR.GetString("Null_value_for_SessionStateItemCollection"));
				}
				this._rqStaticObjects = this._rqItem.StaticObjects;
				this._rqSessionItems.Dirty = false;
				this._rqSessionState = new HttpSessionStateContainer(this, this._rqId, this._rqSessionItems, this._rqStaticObjects, this._rqItem.Timeout, this._rqIsNewSession, SessionStateModule.s_configCookieless, SessionStateModule.s_configMode, this._rqReadonly);
				if (addToContext)
				{
					SessionStateUtility.AddHttpSessionStateToContext(this._rqContext, this._rqSessionState);
				}
			}
			finally
			{
				this.RestoreImpersonation();
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000BE270 File Offset: 0x000BD270
		internal string DelayedGetSessionId()
		{
			this.ChangeImpersonation(this._rqContext, false);
			try
			{
				this._rqId = this._idManager.GetSessionID(this._rqContext);
				if (this._rqId == null)
				{
					this.CreateSessionId();
				}
			}
			finally
			{
				this.RestoreImpersonation();
			}
			return this._rqId;
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000BE2D0 File Offset: 0x000BD2D0
		private bool GetSessionStateItem()
		{
			bool flag = true;
			bool flag2;
			TimeSpan timeSpan;
			if (this._rqReadonly)
			{
				this._rqItem = this._store.GetItem(this._rqContext, this._rqId, out flag2, out timeSpan, out this._rqLockId, out this._rqActionFlags);
			}
			else
			{
				this._rqItem = this._store.GetItemExclusive(this._rqContext, this._rqId, out flag2, out timeSpan, out this._rqLockId, out this._rqActionFlags);
				if (this._rqItem == null && !flag2 && this._rqId != null && (SessionStateModule.s_configCookieless != HttpCookieMode.UseUri || !SessionStateModule.s_configRegenerateExpiredSessionId))
				{
					this._rqUninitializedItem = true;
					this.CreateUninitializedSessionState();
					this._rqItem = this._store.GetItemExclusive(this._rqContext, this._rqId, out flag2, out timeSpan, out this._rqLockId, out this._rqActionFlags);
				}
			}
			if (this._rqItem == null && flag2)
			{
				if (timeSpan >= this._rqExecutionTimeout)
				{
					this._store.ReleaseItemExclusive(this._rqContext, this._rqId, this._rqLockId);
				}
				flag = false;
				this.PollLockedSession();
			}
			return flag;
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000BE3E0 File Offset: 0x000BD3E0
		private void PollLockedSession()
		{
			this.EnsureRequestTimeout();
			if (this._timerCallback == null)
			{
				this._timerCallback = new TimerCallback(this.PollLockedSessionCallback);
			}
			if (this._timer == null)
			{
				this._timerId++;
				this.QueueRef();
				if (!SessionStateModule.s_PollIntervalRegLookedUp)
				{
					SessionStateModule.LookUpRegForPollInterval();
				}
				this._timer = new Timer(this._timerCallback, this._timerId, SessionStateModule.LOCKED_ITEM_POLLING_INTERVAL, SessionStateModule.LOCKED_ITEM_POLLING_INTERVAL);
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000BE461 File Offset: 0x000BD461
		private string SaltedSessionId
		{
			get
			{
				return this._rqId + SessionStateModule.s_sessionIDSalt;
			}
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000BE473 File Offset: 0x000BD473
		private void EnsureRequestTimeout()
		{
			if (SessionStateModule.IsRequestQueueEnabled && this._rqContext.TimeLeft <= 0L)
			{
				throw new HttpException(SR.GetString("Request_timed_out"));
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000BE49B File Offset: 0x000BD49B
		private static bool IsRequestQueueEnabled
		{
			get
			{
				return AppSettings.RequestQueueLimitPerSession != int.MaxValue;
			}
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000BE4AC File Offset: 0x000BD4AC
		private void QueueRef()
		{
			if (!SessionStateModule.IsRequestQueueEnabled || this._rqId == null)
			{
				return;
			}
			QueuedRequestCounter queuedRequestCounter = this.GetQueuedRequestCounter();
			if (queuedRequestCounter.Count >= AppSettings.RequestQueueLimitPerSession)
			{
				throw new HttpException(SR.GetString("Failed_to_process_request"));
			}
			queuedRequestCounter.Increment();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000BE4F4 File Offset: 0x000BD4F4
		private void DequeRef()
		{
			if (!SessionStateModule.IsRequestQueueEnabled || this._rqId == null)
			{
				return;
			}
			QueuedRequestCounter queuedRequestCounter = this.GetQueuedRequestCounter();
			if (queuedRequestCounter.Decrement() == 0)
			{
				SessionStateModule.s_lock.AcquireWriterLock();
				try
				{
					if (queuedRequestCounter.Count == 0)
					{
						SessionStateModule.s_queuedRequestsNumPerSession.Remove(this.SaltedSessionId);
					}
				}
				finally
				{
					SessionStateModule.s_lock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000BE560 File Offset: 0x000BD560
		private QueuedRequestCounter GetQueuedRequestCounter()
		{
			QueuedRequestCounter queuedRequestCounter = null;
			SessionStateModule.s_lock.AcquireReaderLock();
			try
			{
				if (SessionStateModule.s_queuedRequestsNumPerSession.TryGetValue(this.SaltedSessionId, out queuedRequestCounter))
				{
					return queuedRequestCounter;
				}
			}
			finally
			{
				SessionStateModule.s_lock.ReleaseReaderLock();
			}
			SessionStateModule.s_lock.AcquireWriterLock();
			try
			{
				if (SessionStateModule.s_queuedRequestsNumPerSession.TryGetValue(this.SaltedSessionId, out queuedRequestCounter))
				{
					return queuedRequestCounter;
				}
				queuedRequestCounter = new QueuedRequestCounter();
				SessionStateModule.s_queuedRequestsNumPerSession.Add(this.SaltedSessionId, queuedRequestCounter);
			}
			finally
			{
				SessionStateModule.s_lock.ReleaseWriterLock();
			}
			return queuedRequestCounter;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000BE604 File Offset: 0x000BD604
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static void LookUpRegForPollInterval()
		{
			lock (SessionStateModule.s_PollIntervalRegLock)
			{
				if (!SessionStateModule.s_PollIntervalRegLookedUp)
				{
					try
					{
						object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\ASP.NET", "SessionStateLockedItemPollInterval", 0);
						if (value != null && (value is int || value is uint) && (int)value > 0)
						{
							SessionStateModule.LOCKED_ITEM_POLLING_INTERVAL = (long)((int)value);
						}
						SessionStateModule.s_PollIntervalRegLookedUp = true;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000BE694 File Offset: 0x000BD694
		private void ResetPollTimer()
		{
			this._timerId++;
			if (this._timer != null)
			{
				((IDisposable)this._timer).Dispose();
				this._timer = null;
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000BE6C4 File Offset: 0x000BD6C4
		private void ChangeImpersonation(HttpContext context, bool timerThread)
		{
			this._rqChangeImpersonationRefCount++;
			if (this._ignoreImpersonation)
			{
				return;
			}
			if (SessionStateModule.s_configMode == SessionStateMode.SQLServer && ((SqlSessionStateStore)this._store).KnowForSureNotUsingIntegratedSecurity && this._usingAspnetSessionIdManager)
			{
				return;
			}
			if (SessionStateModule.s_useHostingIdentity)
			{
				if (this._rqIctx == null)
				{
					this._rqIctx = new ApplicationImpersonationContext();
					return;
				}
			}
			else if (timerThread)
			{
				this._rqTimerThreadImpersonationIctx = new ClientImpersonationContext(context, false);
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000BE738 File Offset: 0x000BD738
		private void RestoreImpersonation()
		{
			this._rqChangeImpersonationRefCount--;
			if (this._rqChangeImpersonationRefCount == 0)
			{
				if (this._rqIctx != null)
				{
					this._rqIctx.Undo();
					this._rqIctx = null;
				}
				if (this._rqTimerThreadImpersonationIctx != null)
				{
					this._rqTimerThreadImpersonationIctx.Undo();
					this._rqTimerThreadImpersonationIctx = null;
				}
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000BE790 File Offset: 0x000BD790
		private void PollLockedSessionCallback(object state)
		{
			bool flag = false;
			Exception ex = null;
			if (Interlocked.CompareExchange(ref this._rqInCallback, 1, 0) != 0)
			{
				return;
			}
			try
			{
				int num = (int)state;
				if (num == this._timerId && DateTime.UtcNow - this._rqLastPollCompleted >= SessionStateModule.LOCKED_ITEM_POLLING_DELTA)
				{
					this.ChangeImpersonation(this._rqContext, true);
					try
					{
						flag = this.GetSessionStateItem();
						this._rqLastPollCompleted = DateTime.UtcNow;
						if (flag)
						{
							this.ResetPollTimer();
							this.CompleteAcquireState();
						}
					}
					finally
					{
						this.RestoreImpersonation();
					}
				}
			}
			catch (Exception ex2)
			{
				this.ResetPollTimer();
				ex = ex2;
			}
			finally
			{
				Interlocked.Exchange(ref this._rqInCallback, 0);
			}
			if (flag || ex != null)
			{
				this.DequeRef();
				this._rqAr.Complete(false, null, ex);
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000BE878 File Offset: 0x000BD878
		private void EndAcquireState(IAsyncResult ar)
		{
			((HttpAsyncResult)ar).End();
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000BE886 File Offset: 0x000BD886
		private string ReleaseStateGetSessionID()
		{
			if (this._rqId == null)
			{
				this.DelayedGetSessionId();
			}
			return this._rqId;
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000BE8A0 File Offset: 0x000BD8A0
		private void OnReleaseState(object source, EventArgs eventArgs)
		{
			bool flag = false;
			this._releaseCalled = true;
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			this.ChangeImpersonation(context, false);
			try
			{
				if (this._rqSessionState != null)
				{
					bool flag2 = this._rqSessionState == SessionStateModule.s_delayedSessionState;
					SessionStateUtility.RemoveHttpSessionStateFromContext(this._rqContext, flag2);
					if (!this._rqSessionStateNotFound || this._sessionStartEventHandler != null || (!flag2 && this._rqSessionItems.Dirty) || (!flag2 && this._rqStaticObjects != null && !this._rqStaticObjects.NeverAccessed))
					{
						if (this._rqSessionState.IsAbandoned)
						{
							if (this._rqSessionStateNotFound)
							{
								if (this._supportSessionExpiry)
								{
									if (flag2)
									{
										this.InitStateStoreItem(false);
									}
									this._onEndTarget.RaiseSessionOnEnd(this.ReleaseStateGetSessionID(), this._rqItem);
								}
							}
							else
							{
								this._store.RemoveItem(this._rqContext, this.ReleaseStateGetSessionID(), this._rqLockId, this._rqItem);
							}
						}
						else if (!this._rqReadonly)
						{
							if (context.Error == null && (this._rqSessionStateNotFound || this._rqSessionItems.Dirty || (this._rqStaticObjects != null && !this._rqStaticObjects.NeverAccessed) || this._rqItem.Timeout != this._rqSessionState.Timeout))
							{
								if (flag2)
								{
									this.InitStateStoreItem(false);
								}
								if (this._rqItem.Timeout != this._rqSessionState.Timeout)
								{
									this._rqItem.Timeout = this._rqSessionState.Timeout;
								}
								SessionStateModule.s_sessionEverSet = true;
								flag = true;
								this._store.SetAndReleaseItemExclusive(this._rqContext, this.ReleaseStateGetSessionID(), this._rqItem, this._rqLockId, this._rqSessionStateNotFound);
							}
							else if (this._rqUninitializedItem)
							{
								this._store.RemoveItem(this._rqContext, this.ReleaseStateGetSessionID(), this._rqLockId, this._rqItem);
							}
							else if (!this._rqSessionStateNotFound)
							{
								this._store.ReleaseItemExclusive(this._rqContext, this.ReleaseStateGetSessionID(), this._rqLockId);
							}
						}
					}
				}
				if (this._rqAddedCookie && !flag && context.Response.IsBuffered())
				{
					this._idManager.RemoveSessionID(this._rqContext);
				}
			}
			finally
			{
				this.RestoreImpersonation();
			}
			bool requiresSessionState = context.RequiresSessionState;
			if (HttpRuntime.UseIntegratedPipeline && context.NotificationContext.CurrentNotification == RequestNotification.ReleaseRequestState && (SessionStateModule.s_canSkipEndRequestCall || !requiresSessionState))
			{
				context.DisableNotifications(RequestNotification.EndRequest, (RequestNotification)0);
				this._acquireCalled = false;
				this._releaseCalled = false;
				this.ResetPerRequestFields();
			}
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000BEB50 File Offset: 0x000BDB50
		private void OnEndRequest(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!context.RequiresSessionState)
			{
				return;
			}
			this.ChangeImpersonation(context, false);
			try
			{
				if (!this._releaseCalled)
				{
					if (this._acquireCalled)
					{
						this.OnReleaseState(source, eventArgs);
					}
					else
					{
						if (this._rqContext == null)
						{
							this._rqContext = context;
						}
						this._store.InitializeRequest(this._rqContext);
						bool flag;
						this._idManager.InitializeRequest(this._rqContext, true, out flag);
						string sessionID = this._idManager.GetSessionID(context);
						if (sessionID != null)
						{
							this._store.ResetItemTimeout(context, sessionID);
						}
					}
				}
				this._store.EndRequest(this._rqContext);
			}
			finally
			{
				this._acquireCalled = false;
				this._releaseCalled = false;
				this.RestoreImpersonation();
				this.ResetPerRequestFields();
			}
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000BEC28 File Offset: 0x000BDC28
		internal static void ReadConnectionString(SessionStateSection config, ref string cntString, string propName)
		{
			ConfigsHelper.GetRegistryStringAttribute(ref cntString, config, propName);
			HandlerBase.CheckAndReadConnectionString(ref cntString, true);
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000BEC3A File Offset: 0x000BDC3A
		internal bool SessionIDManagerUseCookieless
		{
			get
			{
				if (!this._usingAspnetSessionIdManager)
				{
					return SessionStateModule.s_configCookieless == HttpCookieMode.UseUri;
				}
				return ((SessionIDManager)this._idManager).UseCookieless(this._rqContext);
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000BEC64 File Offset: 0x000BDC64
		internal void EnsureReleaseState(HttpApplication app)
		{
			if (HttpRuntime.UseIntegratedPipeline && this._acquireCalled && !this._releaseCalled)
			{
				try
				{
					this.OnReleaseState(app, null);
				}
				catch
				{
				}
			}
		}

		// Token: 0x04001F81 RID: 8065
		internal const string SQL_CONNECTION_STRING_DEFAULT = "data source=localhost;Integrated Security=SSPI";

		// Token: 0x04001F82 RID: 8066
		internal const string STATE_CONNECTION_STRING_DEFAULT = "tcpip=loopback:42424";

		// Token: 0x04001F83 RID: 8067
		internal const int TIMEOUT_DEFAULT = 20;

		// Token: 0x04001F84 RID: 8068
		internal const SessionStateMode MODE_DEFAULT = SessionStateMode.InProc;

		// Token: 0x04001F85 RID: 8069
		internal const int MAX_CACHE_BASED_TIMEOUT_MINUTES = 525600;

		// Token: 0x04001F86 RID: 8070
		private static long LOCKED_ITEM_POLLING_INTERVAL = 500L;

		// Token: 0x04001F87 RID: 8071
		private static readonly TimeSpan LOCKED_ITEM_POLLING_DELTA = new TimeSpan(2500000L);

		// Token: 0x04001F88 RID: 8072
		private static readonly TimeSpan DEFAULT_DBG_EXECUTION_TIMEOUT = new TimeSpan(0, 0, 30000000);

		// Token: 0x04001F89 RID: 8073
		private bool s_oneTimeInit;

		// Token: 0x04001F8A RID: 8074
		private static int s_timeout;

		// Token: 0x04001F8B RID: 8075
		private static Dictionary<string, QueuedRequestCounter> s_queuedRequestsNumPerSession = new Dictionary<string, QueuedRequestCounter>();

		// Token: 0x04001F8C RID: 8076
		private static string s_sessionIDSalt;

		// Token: 0x04001F8D RID: 8077
		private static ReadWriteSpinLock s_lock;

		// Token: 0x04001F8E RID: 8078
		private static bool s_trustLevelInsufficient;

		// Token: 0x04001F8F RID: 8079
		private static TimeSpan s_configExecutionTimeout;

		// Token: 0x04001F90 RID: 8080
		private static bool s_configRegenerateExpiredSessionId;

		// Token: 0x04001F91 RID: 8081
		private static bool s_useHostingIdentity;

		// Token: 0x04001F92 RID: 8082
		internal static HttpCookieMode s_configCookieless;

		// Token: 0x04001F93 RID: 8083
		internal static SessionStateMode s_configMode;

		// Token: 0x04001F94 RID: 8084
		private static bool s_canSkipEndRequestCall;

		// Token: 0x04001F95 RID: 8085
		private static bool s_PollIntervalRegLookedUp = false;

		// Token: 0x04001F96 RID: 8086
		private static object s_PollIntervalRegLock = new object();

		// Token: 0x04001F97 RID: 8087
		private static bool s_allowInProcOptimization;

		// Token: 0x04001F98 RID: 8088
		private static bool s_sessionEverSet;

		// Token: 0x04001F99 RID: 8089
		private static bool s_allowDelayedStateStoreItemCreation;

		// Token: 0x04001F9A RID: 8090
		private static HttpSessionStateContainer s_delayedSessionState = new HttpSessionStateContainer();

		// Token: 0x04001F9B RID: 8091
		private EventHandler _sessionStartEventHandler;

		// Token: 0x04001F9C RID: 8092
		private Timer _timer;

		// Token: 0x04001F9D RID: 8093
		private TimerCallback _timerCallback;

		// Token: 0x04001F9E RID: 8094
		private volatile int _timerId;

		// Token: 0x04001F9F RID: 8095
		private ISessionIDManager _idManager;

		// Token: 0x04001FA0 RID: 8096
		private bool _usingAspnetSessionIdManager;

		// Token: 0x04001FA1 RID: 8097
		private SessionStateStoreProviderBase _store;

		// Token: 0x04001FA2 RID: 8098
		private bool _supportSessionExpiry;

		// Token: 0x04001FA3 RID: 8099
		private IPartitionResolver _partitionResolver;

		// Token: 0x04001FA4 RID: 8100
		private bool _ignoreImpersonation;

		// Token: 0x04001FA5 RID: 8101
		private readonly SessionOnEndTarget _onEndTarget = new SessionOnEndTarget();

		// Token: 0x04001FA6 RID: 8102
		private bool _acquireCalled;

		// Token: 0x04001FA7 RID: 8103
		private bool _releaseCalled;

		// Token: 0x04001FA8 RID: 8104
		private HttpSessionStateContainer _rqSessionState;

		// Token: 0x04001FA9 RID: 8105
		private string _rqId;

		// Token: 0x04001FAA RID: 8106
		private bool _rqIdNew;

		// Token: 0x04001FAB RID: 8107
		private ISessionStateItemCollection _rqSessionItems;

		// Token: 0x04001FAC RID: 8108
		private HttpStaticObjectsCollection _rqStaticObjects;

		// Token: 0x04001FAD RID: 8109
		private bool _rqIsNewSession;

		// Token: 0x04001FAE RID: 8110
		private bool _rqSessionStateNotFound;

		// Token: 0x04001FAF RID: 8111
		private bool _rqReadonly;

		// Token: 0x04001FB0 RID: 8112
		private HttpContext _rqContext;

		// Token: 0x04001FB1 RID: 8113
		private HttpAsyncResult _rqAr;

		// Token: 0x04001FB2 RID: 8114
		private SessionStateStoreData _rqItem;

		// Token: 0x04001FB3 RID: 8115
		private object _rqLockId;

		// Token: 0x04001FB4 RID: 8116
		private int _rqInCallback;

		// Token: 0x04001FB5 RID: 8117
		private DateTime _rqLastPollCompleted;

		// Token: 0x04001FB6 RID: 8118
		private TimeSpan _rqExecutionTimeout;

		// Token: 0x04001FB7 RID: 8119
		private bool _rqAddedCookie;

		// Token: 0x04001FB8 RID: 8120
		private SessionStateActions _rqActionFlags;

		// Token: 0x04001FB9 RID: 8121
		private ImpersonationContext _rqIctx;

		// Token: 0x04001FBA RID: 8122
		internal int _rqChangeImpersonationRefCount;

		// Token: 0x04001FBB RID: 8123
		private ImpersonationContext _rqTimerThreadImpersonationIctx;

		// Token: 0x04001FBC RID: 8124
		private bool _rqSupportSessionIdReissue;

		// Token: 0x04001FBD RID: 8125
		private bool _rqUninitializedItem;
	}
}
