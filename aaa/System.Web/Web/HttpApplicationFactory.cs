using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200004D RID: 77
	internal class HttpApplicationFactory
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0000BBE8 File Offset: 0x0000ABE8
		internal HttpApplicationFactory()
		{
			this._sessionOnEndEventHandlerAspCompatHelper = new EventHandler(this.SessionOnEndEventHandlerAspCompatHelper);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000BC18 File Offset: 0x0000AC18
		private void Init()
		{
			if (HttpApplicationFactory._customApplication != null)
			{
				return;
			}
			try
			{
				try
				{
					this._appFilename = HttpApplicationFactory.GetApplicationFile();
					this.CompileApplication();
				}
				finally
				{
					this.SetupChangesMonitor();
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BC68 File Offset: 0x0000AC68
		internal static void SetupFileChangeNotifications()
		{
			if (HttpRuntime.CodegenDirInternal != null)
			{
				HttpApplicationFactory._theApplicationFactory.EnsureInited();
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000BC7C File Offset: 0x0000AC7C
		private void EnsureInited()
		{
			if (!this._inited)
			{
				lock (this)
				{
					if (!this._inited)
					{
						this.Init();
						this._inited = true;
					}
				}
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000BCC8 File Offset: 0x0000ACC8
		internal static void EnsureAppStartCalledForIntegratedMode(HttpContext context, HttpApplication app)
		{
			if (!HttpApplicationFactory._theApplicationFactory._appOnStartCalled)
			{
				Exception ex = null;
				lock (HttpApplicationFactory._theApplicationFactory)
				{
					if (!HttpApplicationFactory._theApplicationFactory._appOnStartCalled)
					{
						using (new HttpContextWrapper(context))
						{
							WebBaseEvent.RaiseSystemEvent(HttpApplicationFactory._theApplicationFactory, 1001);
							if (HttpApplicationFactory._theApplicationFactory._onStartMethod != null)
							{
								app.ProcessSpecialRequest(context, HttpApplicationFactory._theApplicationFactory._onStartMethod, HttpApplicationFactory._theApplicationFactory._onStartParamCount, HttpApplicationFactory._theApplicationFactory, EventArgs.Empty, null);
							}
						}
					}
					HttpApplicationFactory._theApplicationFactory._appOnStartCalled = true;
					ex = context.Error;
				}
				if (ex != null)
				{
					throw new HttpException(ex.Message, ex);
				}
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000BD9C File Offset: 0x0000AD9C
		private void EnsureAppStartCalled(HttpContext context)
		{
			if (!this._appOnStartCalled)
			{
				lock (this)
				{
					if (!this._appOnStartCalled)
					{
						using (new HttpContextWrapper(context))
						{
							WebBaseEvent.RaiseSystemEvent(this, 1001);
							this.FireApplicationOnStart(context);
						}
						this._appOnStartCalled = true;
					}
				}
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000BE14 File Offset: 0x0000AE14
		internal static string GetApplicationFile()
		{
			return Path.Combine(HttpRuntime.AppDomainAppPathInternal, "global.asax");
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000BE28 File Offset: 0x0000AE28
		private void CompileApplication()
		{
			this._theApplicationType = BuildManager.GetGlobalAsaxType();
			BuildResultCompiledGlobalAsaxType globalAsaxBuildResult = BuildManager.GetGlobalAsaxBuildResult();
			if (globalAsaxBuildResult != null)
			{
				if (globalAsaxBuildResult.HasAppOrSessionObjects)
				{
					this.GetAppStateByParsingGlobalAsax();
				}
				this._fileDependencies = globalAsaxBuildResult.VirtualPathDependencies;
			}
			if (this._state == null)
			{
				this._state = new HttpApplicationState();
			}
			this.ReflectOnApplicationType();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000BE7C File Offset: 0x0000AE7C
		private void GetAppStateByParsingGlobalAsax()
		{
			using (new ApplicationImpersonationContext())
			{
				if (FileUtil.FileExists(this._appFilename))
				{
					ApplicationFileParser applicationFileParser = new ApplicationFileParser();
					AssemblySet referencedAssemblies = Util.GetReferencedAssemblies(this._theApplicationType.Assembly);
					referencedAssemblies.Add(typeof(string).Assembly);
					VirtualPath virtualPath = HttpRuntime.AppDomainAppVirtualPathObject.SimpleCombine("global.asax");
					applicationFileParser.Parse(referencedAssemblies, virtualPath);
					this._state = new HttpApplicationState(applicationFileParser.ApplicationObjects, applicationFileParser.SessionObjects);
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000BF14 File Offset: 0x0000AF14
		private bool ReflectOnMethodInfoIfItLooksLikeEventHandler(MethodInfo m)
		{
			if (m.ReturnType != typeof(void))
			{
				return false;
			}
			ParameterInfo[] parameters = m.GetParameters();
			switch (parameters.Length)
			{
			case 0:
				goto IL_007A;
			case 2:
				if (parameters[0].ParameterType != typeof(object))
				{
					return false;
				}
				if (parameters[1].ParameterType != typeof(EventArgs) && !parameters[1].ParameterType.IsSubclassOf(typeof(EventArgs)))
				{
					return false;
				}
				goto IL_007A;
			}
			return false;
			IL_007A:
			string name = m.Name;
			int num = name.IndexOf('_');
			if (num <= 0 || num > name.Length - 1)
			{
				return false;
			}
			if (StringUtil.EqualsIgnoreCase(name, "Application_OnStart") || StringUtil.EqualsIgnoreCase(name, "Application_Start"))
			{
				this._onStartMethod = m;
				this._onStartParamCount = parameters.Length;
			}
			else if (StringUtil.EqualsIgnoreCase(name, "Application_OnEnd") || StringUtil.EqualsIgnoreCase(name, "Application_End"))
			{
				this._onEndMethod = m;
				this._onEndParamCount = parameters.Length;
			}
			else if (StringUtil.EqualsIgnoreCase(name, "Session_OnEnd") || StringUtil.EqualsIgnoreCase(name, "Session_End"))
			{
				this._sessionOnEndMethod = m;
				this._sessionOnEndParamCount = parameters.Length;
			}
			return true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000C040 File Offset: 0x0000B040
		private void ReflectOnApplicationType()
		{
			ArrayList arrayList = new ArrayList();
			MethodInfo[] array = this._theApplicationType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in array)
			{
				if (this.ReflectOnMethodInfoIfItLooksLikeEventHandler(methodInfo))
				{
					arrayList.Add(methodInfo);
				}
			}
			Type baseType = this._theApplicationType.BaseType;
			if (baseType != null && baseType != typeof(HttpApplication))
			{
				array = baseType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo2 in array)
				{
					if (methodInfo2.IsPrivate && this.ReflectOnMethodInfoIfItLooksLikeEventHandler(methodInfo2))
					{
						arrayList.Add(methodInfo2);
					}
				}
			}
			this._eventHandlerMethods = new MethodInfo[arrayList.Count];
			for (int k = 0; k < this._eventHandlerMethods.Length; k++)
			{
				this._eventHandlerMethods[k] = (MethodInfo)arrayList[k];
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C12C File Offset: 0x0000B12C
		private void SetupChangesMonitor()
		{
			FileChangeEventHandler fileChangeEventHandler = new FileChangeEventHandler(this.OnAppFileChange);
			HttpRuntime.FileChangesMonitor.StartMonitoringFile(this._appFilename, fileChangeEventHandler);
			if (this._fileDependencies != null)
			{
				foreach (object obj in this._fileDependencies)
				{
					string text = (string)obj;
					HttpRuntime.FileChangesMonitor.StartMonitoringFile(HostingEnvironment.MapPathInternal(text), fileChangeEventHandler);
				}
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000C1B8 File Offset: 0x0000B1B8
		private void OnAppFileChange(object sender, FileChangeEvent e)
		{
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.ChangeInGlobalAsax, "Change in GLOBAL.ASAX");
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000C1C8 File Offset: 0x0000B1C8
		private HttpApplication GetNormalApplicationInstance(HttpContext context)
		{
			HttpApplication httpApplication = null;
			lock (this._freeList)
			{
				if (this._numFreeAppInstances > 0)
				{
					httpApplication = (HttpApplication)this._freeList.Pop();
					this._numFreeAppInstances--;
					if (this._numFreeAppInstances < this._minFreeAppInstances)
					{
						this._minFreeAppInstances = this._numFreeAppInstances;
					}
				}
			}
			if (httpApplication == null)
			{
				httpApplication = (HttpApplication)HttpRuntime.CreateNonPublicInstance(this._theApplicationType);
				using (new ApplicationImpersonationContext())
				{
					httpApplication.InitInternal(context, this._state, this._eventHandlerMethods);
				}
			}
			return httpApplication;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000C288 File Offset: 0x0000B288
		private void RecycleNormalApplicationInstance(HttpApplication app)
		{
			if (this._numFreeAppInstances < 100)
			{
				lock (this._freeList)
				{
					this._freeList.Push(app);
					this._numFreeAppInstances++;
					return;
				}
			}
			app.DisposeInternal();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000C2E8 File Offset: 0x0000B2E8
		private void TrimApplicationInstanceFreeList()
		{
			int minFreeAppInstances = this._minFreeAppInstances;
			this._minFreeAppInstances = this._numFreeAppInstances;
			if (minFreeAppInstances <= 1)
			{
				return;
			}
			HttpApplication httpApplication = null;
			lock (this._freeList)
			{
				if (this._numFreeAppInstances > 1)
				{
					httpApplication = (HttpApplication)this._freeList.Pop();
					this._numFreeAppInstances--;
					this._minFreeAppInstances = this._numFreeAppInstances;
				}
			}
			if (httpApplication != null)
			{
				httpApplication.DisposeInternal();
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000C374 File Offset: 0x0000B374
		internal static HttpApplication GetPipelineApplicationInstance(IntPtr appContext, HttpContext context)
		{
			HttpApplicationFactory._theApplicationFactory.EnsureInited();
			return HttpApplicationFactory._theApplicationFactory.GetSpecialApplicationInstance(appContext, context);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000C38C File Offset: 0x0000B38C
		internal static void RecyclePipelineApplicationInstance(HttpApplication app)
		{
			HttpApplicationFactory._theApplicationFactory.RecycleSpecialApplicationInstance(app);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000C39C File Offset: 0x0000B39C
		private HttpApplication GetSpecialApplicationInstance(IntPtr appContext, HttpContext context)
		{
			HttpApplication httpApplication = null;
			lock (this._specialFreeList)
			{
				if (this._numFreeSpecialAppInstances > 0)
				{
					httpApplication = (HttpApplication)this._specialFreeList.Pop();
					this._numFreeSpecialAppInstances--;
				}
			}
			if (httpApplication == null)
			{
				using (new HttpContextWrapper(context))
				{
					httpApplication = (HttpApplication)HttpRuntime.CreateNonPublicInstance(this._theApplicationType);
					using (new ApplicationImpersonationContext())
					{
						httpApplication.InitSpecial(this._state, this._eventHandlerMethods, appContext, context);
					}
				}
			}
			return httpApplication;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000C460 File Offset: 0x0000B460
		private HttpApplication GetSpecialApplicationInstance()
		{
			return this.GetSpecialApplicationInstance(IntPtr.Zero, null);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000C470 File Offset: 0x0000B470
		private void RecycleSpecialApplicationInstance(HttpApplication app)
		{
			if (this._numFreeSpecialAppInstances < 20)
			{
				lock (this._specialFreeList)
				{
					this._specialFreeList.Push(app);
					this._numFreeSpecialAppInstances++;
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C4C8 File Offset: 0x0000B4C8
		private void FireApplicationOnStart(HttpContext context)
		{
			if (this._onStartMethod != null)
			{
				HttpApplication specialApplicationInstance = this.GetSpecialApplicationInstance();
				specialApplicationInstance.ProcessSpecialRequest(context, this._onStartMethod, this._onStartParamCount, this, EventArgs.Empty, null);
				this.RecycleSpecialApplicationInstance(specialApplicationInstance);
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000C508 File Offset: 0x0000B508
		private void FireApplicationOnEnd()
		{
			if (this._onEndMethod != null)
			{
				HttpApplication specialApplicationInstance = this.GetSpecialApplicationInstance();
				specialApplicationInstance.ProcessSpecialRequest(null, this._onEndMethod, this._onEndParamCount, this, EventArgs.Empty, null);
				this.RecycleSpecialApplicationInstance(specialApplicationInstance);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000C548 File Offset: 0x0000B548
		private void SessionOnEndEventHandlerAspCompatHelper(object eventSource, EventArgs eventArgs)
		{
			HttpApplicationFactory.AspCompatSessionOnEndHelper aspCompatSessionOnEndHelper = (HttpApplicationFactory.AspCompatSessionOnEndHelper)eventSource;
			aspCompatSessionOnEndHelper.Application.ProcessSpecialRequest(null, this._sessionOnEndMethod, this._sessionOnEndParamCount, aspCompatSessionOnEndHelper.Source, aspCompatSessionOnEndHelper.Args, aspCompatSessionOnEndHelper.Session);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000C588 File Offset: 0x0000B588
		private void FireSessionOnEnd(HttpSessionState session, object eventSource, EventArgs eventArgs)
		{
			if (this._sessionOnEndMethod != null)
			{
				HttpApplication specialApplicationInstance = this.GetSpecialApplicationInstance();
				if (AspCompatApplicationStep.AnyStaObjectsInSessionState(session) || HttpRuntime.ApartmentThreading)
				{
					HttpApplicationFactory.AspCompatSessionOnEndHelper aspCompatSessionOnEndHelper = new HttpApplicationFactory.AspCompatSessionOnEndHelper(specialApplicationInstance, session, eventSource, eventArgs);
					AspCompatApplicationStep.RaiseAspCompatEvent(null, specialApplicationInstance, session.SessionID, this._sessionOnEndEventHandlerAspCompatHelper, aspCompatSessionOnEndHelper, EventArgs.Empty);
				}
				else
				{
					specialApplicationInstance.ProcessSpecialRequest(null, this._sessionOnEndMethod, this._sessionOnEndParamCount, eventSource, eventArgs, session);
				}
				this.RecycleSpecialApplicationInstance(specialApplicationInstance);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000C5F8 File Offset: 0x0000B5F8
		private void FireApplicationOnError(Exception error)
		{
			HttpApplication specialApplicationInstance = this.GetSpecialApplicationInstance();
			specialApplicationInstance.RaiseErrorWithoutContext(error);
			this.RecycleSpecialApplicationInstance(specialApplicationInstance);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000C61C File Offset: 0x0000B61C
		private void Dispose()
		{
			ArrayList arrayList = new ArrayList();
			lock (this._freeList)
			{
				while (this._numFreeAppInstances > 0)
				{
					arrayList.Add(this._freeList.Pop());
					this._numFreeAppInstances--;
				}
			}
			int count = arrayList.Count;
			for (int i = 0; i < count; i++)
			{
				((HttpApplication)arrayList[i]).DisposeInternal();
			}
			if (this._appOnStartCalled && !this._appOnEndCalled)
			{
				lock (this)
				{
					if (!this._appOnEndCalled)
					{
						this.FireApplicationOnEnd();
						this._appOnEndCalled = true;
					}
				}
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000C6E8 File Offset: 0x0000B6E8
		internal static void SetCustomApplication(IHttpHandler customApplication)
		{
			if (HttpRuntime.AppDomainAppIdInternal == null)
			{
				HttpApplicationFactory._customApplication = customApplication;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000C6F8 File Offset: 0x0000B6F8
		internal static IHttpHandler GetApplicationInstance(HttpContext context)
		{
			if (HttpApplicationFactory._customApplication != null)
			{
				return HttpApplicationFactory._customApplication;
			}
			if (context.Request.IsDebuggingRequest)
			{
				return new HttpDebugHandler();
			}
			HttpApplicationFactory._theApplicationFactory.EnsureInited();
			HttpApplicationFactory._theApplicationFactory.EnsureAppStartCalled(context);
			return HttpApplicationFactory._theApplicationFactory.GetNormalApplicationInstance(context);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000C745 File Offset: 0x0000B745
		internal static void RecycleApplicationInstance(HttpApplication app)
		{
			HttpApplicationFactory._theApplicationFactory.RecycleNormalApplicationInstance(app);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000C752 File Offset: 0x0000B752
		internal static void TrimApplicationInstances()
		{
			if (HttpApplicationFactory._theApplicationFactory != null)
			{
				HttpApplicationFactory._theApplicationFactory.TrimApplicationInstanceFreeList();
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000C765 File Offset: 0x0000B765
		internal static void EndApplication()
		{
			HttpApplicationFactory._theApplicationFactory.Dispose();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000C771 File Offset: 0x0000B771
		internal static void EndSession(HttpSessionState session, object eventSource, EventArgs eventArgs)
		{
			HttpApplicationFactory._theApplicationFactory.FireSessionOnEnd(session, eventSource, eventArgs);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000C780 File Offset: 0x0000B780
		internal static void RaiseError(Exception error)
		{
			HttpApplicationFactory._theApplicationFactory.EnsureInited();
			HttpApplicationFactory._theApplicationFactory.FireApplicationOnError(error);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000C798 File Offset: 0x0000B798
		internal static HttpApplicationState ApplicationState
		{
			get
			{
				HttpApplicationState httpApplicationState = HttpApplicationFactory._theApplicationFactory._state;
				if (httpApplicationState == null)
				{
					httpApplicationState = new HttpApplicationState();
				}
				return httpApplicationState;
			}
		}

		// Token: 0x04000E49 RID: 3657
		internal const string applicationFileName = "global.asax";

		// Token: 0x04000E4A RID: 3658
		private const int _maxFreeAppInstances = 100;

		// Token: 0x04000E4B RID: 3659
		private const int _maxFreeSpecialAppInstances = 20;

		// Token: 0x04000E4C RID: 3660
		private static HttpApplicationFactory _theApplicationFactory = new HttpApplicationFactory();

		// Token: 0x04000E4D RID: 3661
		private bool _inited;

		// Token: 0x04000E4E RID: 3662
		private string _appFilename;

		// Token: 0x04000E4F RID: 3663
		private ICollection _fileDependencies;

		// Token: 0x04000E50 RID: 3664
		private bool _appOnStartCalled;

		// Token: 0x04000E51 RID: 3665
		private bool _appOnEndCalled;

		// Token: 0x04000E52 RID: 3666
		private HttpApplicationState _state;

		// Token: 0x04000E53 RID: 3667
		private Type _theApplicationType;

		// Token: 0x04000E54 RID: 3668
		private Stack _freeList = new Stack();

		// Token: 0x04000E55 RID: 3669
		private int _numFreeAppInstances;

		// Token: 0x04000E56 RID: 3670
		private int _minFreeAppInstances;

		// Token: 0x04000E57 RID: 3671
		private Stack _specialFreeList = new Stack();

		// Token: 0x04000E58 RID: 3672
		private int _numFreeSpecialAppInstances;

		// Token: 0x04000E59 RID: 3673
		private MethodInfo _onStartMethod;

		// Token: 0x04000E5A RID: 3674
		private int _onStartParamCount;

		// Token: 0x04000E5B RID: 3675
		private MethodInfo _onEndMethod;

		// Token: 0x04000E5C RID: 3676
		private int _onEndParamCount;

		// Token: 0x04000E5D RID: 3677
		private MethodInfo _sessionOnEndMethod;

		// Token: 0x04000E5E RID: 3678
		private int _sessionOnEndParamCount;

		// Token: 0x04000E5F RID: 3679
		private EventHandler _sessionOnEndEventHandlerAspCompatHelper;

		// Token: 0x04000E60 RID: 3680
		private MethodInfo[] _eventHandlerMethods;

		// Token: 0x04000E61 RID: 3681
		private static IHttpHandler _customApplication;

		// Token: 0x0200004E RID: 78
		private class AspCompatSessionOnEndHelper
		{
			// Token: 0x0600026C RID: 620 RVA: 0x0000C7C6 File Offset: 0x0000B7C6
			internal AspCompatSessionOnEndHelper(HttpApplication app, HttpSessionState session, object eventSource, EventArgs eventArgs)
			{
				this._app = app;
				this._session = session;
				this._eventSource = eventSource;
				this._eventArgs = eventArgs;
			}

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x0600026D RID: 621 RVA: 0x0000C7EB File Offset: 0x0000B7EB
			internal HttpApplication Application
			{
				get
				{
					return this._app;
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x0600026E RID: 622 RVA: 0x0000C7F3 File Offset: 0x0000B7F3
			internal HttpSessionState Session
			{
				get
				{
					return this._session;
				}
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x0600026F RID: 623 RVA: 0x0000C7FB File Offset: 0x0000B7FB
			internal object Source
			{
				get
				{
					return this._eventSource;
				}
			}

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000270 RID: 624 RVA: 0x0000C803 File Offset: 0x0000B803
			internal EventArgs Args
			{
				get
				{
					return this._eventArgs;
				}
			}

			// Token: 0x04000E62 RID: 3682
			private HttpApplication _app;

			// Token: 0x04000E63 RID: 3683
			private HttpSessionState _session;

			// Token: 0x04000E64 RID: 3684
			private object _eventSource;

			// Token: 0x04000E65 RID: 3685
			private EventArgs _eventArgs;
		}
	}
}
