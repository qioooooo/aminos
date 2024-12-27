using System;
using System.Collections;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

namespace System.Web
{
	// Token: 0x02000089 RID: 137
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpRuntime
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0001BEFC File Offset: 0x0001AEFC
		static HttpRuntime()
		{
			HttpRuntime.AddAppDomainTraceMessage("*HttpRuntime::cctor");
			HttpRuntime.StaticInit();
			HttpRuntime._theRuntime = new HttpRuntime();
			HttpRuntime._theRuntime.Init();
			HttpRuntime.AddAppDomainTraceMessage("HttpRuntime::cctor*");
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001BF78 File Offset: 0x0001AF78
		[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public HttpRuntime()
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001BF87 File Offset: 0x0001AF87
		internal static void ForceStaticInit()
		{
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001BF8C File Offset: 0x0001AF8C
		private static void StaticInit()
		{
			if (HttpRuntime.s_initialized)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
			if (UnsafeNativeMethods.GetModuleHandle("webengine.dll") != IntPtr.Zero)
			{
				flag = true;
			}
			if (!flag)
			{
				string text = runtimeDirectory + Path.DirectorySeparatorChar + "webengine.dll";
				if (UnsafeNativeMethods.LoadLibrary(text) != IntPtr.Zero)
				{
					flag = true;
					flag2 = true;
				}
			}
			if (flag)
			{
				UnsafeNativeMethods.InitializeLibrary();
				if (flag2)
				{
					UnsafeNativeMethods.PerfCounterInitialize();
				}
			}
			HttpRuntime.s_installDirectory = runtimeDirectory;
			HttpRuntime.s_isEngineLoaded = flag;
			HttpRuntime.s_initialized = true;
			HttpRuntime.AddAppDomainTraceMessage("Initialize");
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001C020 File Offset: 0x0001B020
		private void Init()
		{
			try
			{
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					throw new PlatformNotSupportedException(SR.GetString("RequiresNT"));
				}
				this._profiler = new Profiler();
				this._timeoutManager = new RequestTimeoutManager();
				this._wpUserId = HttpRuntime.GetCurrentUserName();
				this._requestNotificationCompletionCallback = new AsyncCallback(this.OnRequestNotificationCompletion);
				this._handlerCompletionCallback = new AsyncCallback(this.OnHandlerCompletion);
				this._asyncEndOfSendCallback = new HttpWorkerRequest.EndOfSendNotification(this.EndOfSendCallback);
				this._appDomainUnloadallback = new WaitCallback(this.ReleaseResourcesAndUnloadAppDomain);
				if (HttpRuntime.GetAppDomainString(".appDomain") != null)
				{
					this._appDomainAppId = HttpRuntime.GetAppDomainString(".appId");
					this._appDomainAppPath = HttpRuntime.GetAppDomainString(".appPath");
					this._appDomainAppVPath = VirtualPath.CreateNonRelativeTrailingSlash(HttpRuntime.GetAppDomainString(".appVPath"));
					this._appDomainId = HttpRuntime.GetAppDomainString(".domainId");
					this._isOnUNCShare = StringUtil.StringStartsWith(this._appDomainAppPath, "\\\\");
					PerfCounters.Open(this._appDomainAppId);
				}
				this._fcm = new FileChangesMonitor();
			}
			catch (Exception ex)
			{
				HttpRuntime.InitializationException = ex;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001C158 File Offset: 0x0001B158
		private void SetUpDataDirectory()
		{
			string text = Path.Combine(this._appDomainAppPath, "App_Data");
			AppDomain.CurrentDomain.SetData("DataDirectory", text, new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text));
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001C190 File Offset: 0x0001B190
		private void DisposeAppDomainShutdownTimer()
		{
			Timer appDomainShutdownTimer = this._appDomainShutdownTimer;
			if (appDomainShutdownTimer != null && Interlocked.CompareExchange<Timer>(ref this._appDomainShutdownTimer, null, appDomainShutdownTimer) == appDomainShutdownTimer)
			{
				appDomainShutdownTimer.Dispose();
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001C1C0 File Offset: 0x0001B1C0
		private void AppDomainShutdownTimerCallback(object state)
		{
			try
			{
				this.DisposeAppDomainShutdownTimer();
				HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.InitializationError, "Initialization Error");
			}
			catch
			{
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001C1F8 File Offset: 0x0001B1F8
		private void StartAppDomainShutdownTimer()
		{
			if (this._appDomainShutdownTimer == null && !this._shutdownInProgress)
			{
				lock (this)
				{
					if (this._appDomainShutdownTimer == null && !this._shutdownInProgress)
					{
						this._appDomainShutdownTimer = new Timer(new TimerCallback(this.AppDomainShutdownTimerCallback), null, 10000, 0);
					}
				}
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001C264 File Offset: 0x0001B264
		private void HostingInit(HostingEnvironmentFlags hostingFlags)
		{
			using (new ApplicationImpersonationContext())
			{
				try
				{
					this._firstRequestStartTime = DateTime.UtcNow;
					this.SetUpDataDirectory();
					this.EnsureAccessToApplicationDirectory();
					this.StartMonitoringDirectoryRenamesAndBinDirectory();
					CacheSection cacheSection;
					TrustSection trustSection;
					SecurityPolicySection securityPolicySection;
					CompilationSection compilationSection;
					HostingEnvironmentSection hostingEnvironmentSection;
					Exception ex;
					this.GetInitConfigSections(out cacheSection, out trustSection, out securityPolicySection, out compilationSection, out hostingEnvironmentSection, out ex);
					HttpRuntime.CacheInternal.ReadCacheInternalConfig(cacheSection);
					this.SetUpCodegenDirectory(compilationSection);
					try
					{
						this.SetTrustLevel(trustSection, securityPolicySection);
					}
					catch
					{
						if (ex != null)
						{
							throw ex;
						}
						throw;
					}
					this.InitFusion(hostingEnvironmentSection);
					HttpConfigurationSystem.CompleteInit();
					if (ex != null)
					{
						throw ex;
					}
					this.SetThreadPoolLimits();
					HttpRuntime.SetAutogenKeys();
					BuildManager.InitializeBuildManager();
					this.InitApartmentThreading();
					this.InitDebuggingSupport();
					this._processRequestInApplicationTrust = trustSection.ProcessRequestInApplicationTrust;
				}
				catch (Exception ex2)
				{
					this._hostingInitFailed = true;
					HttpRuntime.InitializationException = ex2;
					if ((hostingFlags & HostingEnvironmentFlags.ThrowHostingInitErrors) != HostingEnvironmentFlags.Default)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001C35C File Offset: 0x0001B35C
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001C368 File Offset: 0x0001B368
		internal static Exception InitializationException
		{
			get
			{
				return HttpRuntime._theRuntime._initializationError;
			}
			set
			{
				HttpRuntime._theRuntime._initializationError = value;
				if (!HttpRuntime.HostingInitFailed)
				{
					HttpRuntime._theRuntime.StartAppDomainShutdownTimer();
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001C386 File Offset: 0x0001B386
		internal static bool HostingInitFailed
		{
			get
			{
				return HttpRuntime._theRuntime._hostingInitFailed;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001C392 File Offset: 0x0001B392
		internal static void InitializeHostingFeatures(HostingEnvironmentFlags hostingFlags)
		{
			HttpRuntime._theRuntime.HostingInit(hostingFlags);
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001C39F File Offset: 0x0001B39F
		internal static bool EnableHeaderChecking
		{
			get
			{
				return HttpRuntime._theRuntime._enableHeaderChecking;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001C3AB File Offset: 0x0001B3AB
		internal static bool ProcessRequestInApplicationTrust
		{
			get
			{
				return HttpRuntime._theRuntime._processRequestInApplicationTrust;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001C3B7 File Offset: 0x0001B3B7
		internal static byte[] AppOfflineMessage
		{
			get
			{
				return HttpRuntime._theRuntime._appOfflineMessage;
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001C3C4 File Offset: 0x0001B3C4
		private void FirstRequestInit(HttpContext context)
		{
			Exception ex = null;
			if (HttpRuntime.InitializationException == null && this._appDomainId != null)
			{
				try
				{
					using (new ApplicationImpersonationContext())
					{
						CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
						CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
						try
						{
							HttpRuntime.InitHttpConfiguration();
							HttpRuntime.CheckApplicationEnabled();
							this.CheckAccessToTempDirectory();
							this.InitializeHealthMonitoring();
							this.InitRequestQueue();
							this.InitTrace(context);
							HealthMonitoringManager.StartHealthMonitoringHeartbeat();
							HttpRuntime.RestrictIISFolders(context);
							this.PreloadAssembliesFromBin();
							this.InitHeaderEncoding();
							if (context.WorkerRequest is ISAPIWorkerRequestOutOfProc)
							{
								ProcessModelSection processModel = RuntimeConfig.GetMachineConfig().ProcessModel;
							}
						}
						finally
						{
							Thread.CurrentThread.CurrentUICulture = currentUICulture;
							Thread.CurrentThread.CurrentCulture = currentCulture;
						}
					}
				}
				catch (ConfigurationException ex2)
				{
					ex = ex2;
				}
				catch (Exception ex3)
				{
					ex = new HttpException(SR.GetString("XSP_init_error", new object[] { ex3.Message }), ex3);
				}
			}
			if (HttpRuntime.InitializationException != null)
			{
				throw new HttpException(HttpRuntime.InitializationException.Message, HttpRuntime.InitializationException);
			}
			if (ex != null)
			{
				HttpRuntime.InitializationException = ex;
				throw ex;
			}
			HttpRuntime.AddAppDomainTraceMessage("FirstRequestInit");
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001C518 File Offset: 0x0001B518
		private void EnsureFirstRequestInit(HttpContext context)
		{
			if (this._beforeFirstRequest)
			{
				lock (this)
				{
					if (this._beforeFirstRequest)
					{
						this._firstRequestStartTime = DateTime.UtcNow;
						this.FirstRequestInit(context);
						this._beforeFirstRequest = false;
					}
				}
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001C570 File Offset: 0x0001B570
		private void EnsureAccessToApplicationDirectory()
		{
			if (FileUtil.DirectoryAccessible(this._appDomainAppPath))
			{
				return;
			}
			if (this._appDomainAppPath.IndexOf('?') >= 0)
			{
				throw new HttpException(SR.GetString("Access_denied_to_unicode_app_dir", new object[] { this._appDomainAppPath }));
			}
			throw new HttpException(SR.GetString("Access_denied_to_app_dir", new object[] { this._appDomainAppPath }));
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001C5DC File Offset: 0x0001B5DC
		private void StartMonitoringDirectoryRenamesAndBinDirectory()
		{
			this._fcm.StartMonitoringDirectoryRenamesAndBinDirectory(HttpRuntime.AppDomainAppPathInternal, new FileChangeEventHandler(this.OnCriticalDirectoryChange));
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001C5FA File Offset: 0x0001B5FA
		internal static void StartListeningToLocalResourcesDirectory(VirtualPath virtualDir)
		{
			HttpRuntime._theRuntime._fcm.StartListeningToLocalResourcesDirectory(virtualDir);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001C60C File Offset: 0x0001B60C
		private void GetInitConfigSections(out CacheSection cacheSection, out TrustSection trustSection, out SecurityPolicySection securityPolicySection, out CompilationSection compilationSection, out HostingEnvironmentSection hostingEnvironmentSection, out Exception initException)
		{
			cacheSection = null;
			trustSection = null;
			securityPolicySection = null;
			compilationSection = null;
			hostingEnvironmentSection = null;
			initException = null;
			RuntimeConfig appLKGConfig = RuntimeConfig.GetAppLKGConfig();
			RuntimeConfig runtimeConfig = null;
			try
			{
				runtimeConfig = RuntimeConfig.GetAppConfig();
			}
			catch (Exception ex)
			{
				initException = ex;
			}
			if (runtimeConfig != null)
			{
				try
				{
					cacheSection = runtimeConfig.Cache;
				}
				catch (Exception ex2)
				{
					if (initException == null)
					{
						initException = ex2;
					}
				}
			}
			if (cacheSection == null)
			{
				cacheSection = appLKGConfig.Cache;
			}
			if (runtimeConfig != null)
			{
				try
				{
					trustSection = runtimeConfig.Trust;
				}
				catch (Exception ex3)
				{
					if (initException == null)
					{
						initException = ex3;
					}
				}
			}
			if (trustSection == null)
			{
				trustSection = appLKGConfig.Trust;
			}
			if (runtimeConfig != null)
			{
				try
				{
					securityPolicySection = runtimeConfig.SecurityPolicy;
				}
				catch (Exception ex4)
				{
					if (initException == null)
					{
						initException = ex4;
					}
				}
			}
			if (securityPolicySection == null)
			{
				securityPolicySection = appLKGConfig.SecurityPolicy;
			}
			if (runtimeConfig != null)
			{
				try
				{
					compilationSection = runtimeConfig.Compilation;
				}
				catch (Exception ex5)
				{
					if (initException == null)
					{
						initException = ex5;
					}
				}
			}
			if (compilationSection == null)
			{
				compilationSection = appLKGConfig.Compilation;
			}
			if (runtimeConfig != null)
			{
				try
				{
					hostingEnvironmentSection = runtimeConfig.HostingEnvironment;
				}
				catch (Exception ex6)
				{
					if (initException == null)
					{
						initException = ex6;
					}
				}
			}
			if (hostingEnvironmentSection == null)
			{
				hostingEnvironmentSection = appLKGConfig.HostingEnvironment;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001C758 File Offset: 0x0001B758
		private void SetUpCodegenDirectory(CompilationSection compilationSection)
		{
			AppDomain domain = Thread.GetDomain();
			string text = AppManagerAppDomainFactory.ConstructSimpleAppName(HttpRuntime.AppDomainAppVirtualPath);
			string text2 = null;
			string text3 = null;
			string text4 = null;
			int num = 0;
			if (compilationSection != null && !string.IsNullOrEmpty(compilationSection.TempDirectory))
			{
				text2 = compilationSection.TempDirectory;
				compilationSection.GetTempDirectoryErrorInfo(out text3, out text4, out num);
			}
			if (text2 != null)
			{
				text2 = text2.Trim();
				if (!Path.IsPathRooted(text2))
				{
					text2 = null;
				}
				else
				{
					try
					{
						text2 = new DirectoryInfo(text2).FullName;
					}
					catch
					{
						text2 = null;
					}
				}
				if (text2 == null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_temp_directory", new object[] { text3 }), text4, num);
				}
				try
				{
					Directory.CreateDirectory(text2);
					goto IL_00D0;
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Invalid_temp_directory", new object[] { text3 }), ex, text4, num);
				}
			}
			text2 = Path.Combine(HttpRuntime.s_installDirectory, "Temporary ASP.NET Files");
			IL_00D0:
			if (!Util.HasWriteAccessToDirectory(text2))
			{
				if (!Environment.UserInteractive)
				{
					throw new HttpException(SR.GetString("No_codegen_access", new object[]
					{
						Util.GetCurrentAccountName(),
						text2
					}));
				}
				text2 = Path.GetTempPath();
				text2 = Path.Combine(text2, "Temporary ASP.NET Files");
			}
			this._tempDir = text2;
			string text5 = Path.Combine(text2, text);
			domain.SetDynamicBase(text5);
			this._codegenDir = Thread.GetDomain().DynamicDirectory;
			Directory.CreateDirectory(this._codegenDir);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001C8CC File Offset: 0x0001B8CC
		private void InitFusion(HostingEnvironmentSection hostingEnvironmentSection)
		{
			AppDomain domain = Thread.GetDomain();
			string text = this._appDomainAppPath;
			if (text.IndexOf(HttpRuntime.DoubleDirectorySeparatorString, 1, StringComparison.Ordinal) >= 1)
			{
				text = text[0] + text.Substring(1).Replace(HttpRuntime.DoubleDirectorySeparatorString, HttpRuntime.DirectorySeparatorString);
			}
			domain.AppendPrivatePath(text + "bin");
			if (hostingEnvironmentSection != null && !hostingEnvironmentSection.ShadowCopyBinAssemblies)
			{
				domain.ClearShadowCopyPath();
			}
			else
			{
				domain.SetShadowCopyPath(text + "bin");
			}
			string fullName = Directory.GetParent(this._codegenDir).FullName;
			domain.SetCachePath(fullName);
			this._fusionInited = true;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001C974 File Offset: 0x0001B974
		private void InitRequestQueue()
		{
			RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
			HttpRuntimeSection httpRuntime = appConfig.HttpRuntime;
			ProcessModelSection processModel = appConfig.ProcessModel;
			if (processModel.AutoConfig)
			{
				this._requestQueue = new RequestQueue(88 * processModel.CpuCount, 76 * processModel.CpuCount, httpRuntime.AppRequestQueueLimit, processModel.ClientConnectedCheck);
				return;
			}
			int num = ((processModel.MaxWorkerThreadsTimesCpuCount < processModel.MaxIoThreadsTimesCpuCount) ? processModel.MaxWorkerThreadsTimesCpuCount : processModel.MaxIoThreadsTimesCpuCount);
			if (httpRuntime.MinFreeThreads >= num)
			{
				if (httpRuntime.ElementInformation.Properties["minFreeThreads"].LineNumber != 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Min_free_threads_must_be_under_thread_pool_limits", new object[] { num.ToString(CultureInfo.InvariantCulture) }), httpRuntime.ElementInformation.Properties["minFreeThreads"].Source, httpRuntime.ElementInformation.Properties["minFreeThreads"].LineNumber);
				}
				if (processModel.ElementInformation.Properties["maxWorkerThreads"].LineNumber != 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Thread_pool_limit_must_be_greater_than_minFreeThreads", new object[] { httpRuntime.MinFreeThreads.ToString(CultureInfo.InvariantCulture) }), processModel.ElementInformation.Properties["maxWorkerThreads"].Source, processModel.ElementInformation.Properties["maxWorkerThreads"].LineNumber);
				}
				throw new ConfigurationErrorsException(SR.GetString("Thread_pool_limit_must_be_greater_than_minFreeThreads", new object[] { httpRuntime.MinFreeThreads.ToString(CultureInfo.InvariantCulture) }), processModel.ElementInformation.Properties["maxIoThreads"].Source, processModel.ElementInformation.Properties["maxIoThreads"].LineNumber);
			}
			else
			{
				if (httpRuntime.MinLocalRequestFreeThreads <= httpRuntime.MinFreeThreads)
				{
					this._requestQueue = new RequestQueue(httpRuntime.MinFreeThreads, httpRuntime.MinLocalRequestFreeThreads, httpRuntime.AppRequestQueueLimit, processModel.ClientConnectedCheck);
					return;
				}
				if (httpRuntime.ElementInformation.Properties["minLocalRequestFreeThreads"].LineNumber == 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Local_free_threads_cannot_exceed_free_threads"), processModel.ElementInformation.Properties["minFreeThreads"].Source, processModel.ElementInformation.Properties["minFreeThreads"].LineNumber);
				}
				throw new ConfigurationErrorsException(SR.GetString("Local_free_threads_cannot_exceed_free_threads"), httpRuntime.ElementInformation.Properties["minLocalRequestFreeThreads"].Source, httpRuntime.ElementInformation.Properties["minLocalRequestFreeThreads"].LineNumber);
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001CC30 File Offset: 0x0001BC30
		private void InitApartmentThreading()
		{
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetAppConfig().HttpRuntime;
			if (httpRuntime != null)
			{
				this._apartmentThreading = httpRuntime.ApartmentThreading;
				return;
			}
			this._apartmentThreading = false;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001CC60 File Offset: 0x0001BC60
		private void InitTrace(HttpContext context)
		{
			TraceSection trace = RuntimeConfig.GetAppConfig().Trace;
			HttpRuntime.Profile.RequestsToProfile = trace.RequestLimit;
			HttpRuntime.Profile.PageOutput = trace.PageOutput;
			HttpRuntime.Profile.OutputMode = TraceMode.SortByTime;
			if (trace.TraceMode == TraceDisplayMode.SortByCategory)
			{
				HttpRuntime.Profile.OutputMode = TraceMode.SortByCategory;
			}
			HttpRuntime.Profile.LocalOnly = trace.LocalOnly;
			HttpRuntime.Profile.IsEnabled = trace.Enabled;
			HttpRuntime.Profile.MostRecent = trace.MostRecent;
			HttpRuntime.Profile.Reset();
			context.TraceIsEnabled = trace.Enabled;
			TraceContext.SetWriteToDiagnosticsTrace(trace.WriteToDiagnosticsTrace);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001CD08 File Offset: 0x0001BD08
		private void InitDebuggingSupport()
		{
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			this._debuggingEnabled = compilation.Debug;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001CD2C File Offset: 0x0001BD2C
		private void PreloadAssembliesFromBin()
		{
			bool flag = false;
			if (!this._isOnUNCShare)
			{
				IdentitySection identity = RuntimeConfig.GetAppConfig().Identity;
				if (identity.Impersonate && identity.ImpersonateToken == IntPtr.Zero)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			string binDirectoryInternal = HttpRuntime.BinDirectoryInternal;
			DirectoryInfo directoryInfo = new DirectoryInfo(binDirectoryInternal);
			if (!directoryInfo.Exists)
			{
				return;
			}
			this.PreloadAssembliesFromBinRecursive(directoryInfo);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001CD8C File Offset: 0x0001BD8C
		private void PreloadAssembliesFromBinRecursive(DirectoryInfo dirInfo)
		{
			FileInfo[] files = dirInfo.GetFiles("*.dll");
			foreach (FileInfo fileInfo in files)
			{
				try
				{
					Assembly.Load(Util.GetAssemblyNameFromFileName(fileInfo.Name));
				}
				catch (FileNotFoundException)
				{
					try
					{
						Assembly.LoadFrom(fileInfo.FullName);
					}
					catch
					{
					}
				}
				catch
				{
				}
			}
			DirectoryInfo[] directories = dirInfo.GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				this.PreloadAssembliesFromBinRecursive(directoryInfo);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001CE40 File Offset: 0x0001BE40
		private void SetAutoConfigLimits(ProcessModelSection pmConfig)
		{
			int num;
			int num2;
			ThreadPool.GetMaxThreads(out num, out num2);
			if (pmConfig.DefaultMaxWorkerThreadsForAutoConfig != num || pmConfig.DefaultMaxIoThreadsForAutoConfig != num2)
			{
				UnsafeNativeMethods.SetClrThreadPoolLimits(pmConfig.DefaultMaxWorkerThreadsForAutoConfig, pmConfig.DefaultMaxIoThreadsForAutoConfig);
			}
			ServicePointManager.DefaultConnectionLimit = 12 * pmConfig.CpuCount;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001CE88 File Offset: 0x0001BE88
		private void SetThreadPoolLimits()
		{
			try
			{
				ProcessModelSection processModel = RuntimeConfig.GetMachineConfig().ProcessModel;
				if (processModel.AutoConfig)
				{
					this.SetAutoConfigLimits(processModel);
				}
				else if (processModel.MaxWorkerThreadsTimesCpuCount > 0 && processModel.MaxIoThreadsTimesCpuCount > 0)
				{
					int num;
					int num2;
					ThreadPool.GetMaxThreads(out num, out num2);
					if (processModel.MaxWorkerThreadsTimesCpuCount != num || processModel.MaxIoThreadsTimesCpuCount != num2)
					{
						UnsafeNativeMethods.SetClrThreadPoolLimits(processModel.MaxWorkerThreadsTimesCpuCount, processModel.MaxIoThreadsTimesCpuCount);
					}
				}
				if (processModel.MinWorkerThreadsTimesCpuCount > 0 || processModel.MinIoThreadsTimesCpuCount > 0)
				{
					int num3;
					int num4;
					ThreadPool.GetMinThreads(out num3, out num4);
					int num5 = ((processModel.MinWorkerThreadsTimesCpuCount > 0) ? processModel.MinWorkerThreadsTimesCpuCount : num3);
					int num6 = ((processModel.MinIoThreadsTimesCpuCount > 0) ? processModel.MinIoThreadsTimesCpuCount : num4);
					if (num5 > 0 && num6 > 0 && (num5 != num3 || num6 != num4))
					{
						ThreadPool.SetMinThreads(num5, num6);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001CF68 File Offset: 0x0001BF68
		internal static void CheckApplicationEnabled()
		{
			string text = Path.Combine(HttpRuntime._theRuntime._appDomainAppPath, "App_Offline.htm");
			bool flag = false;
			HttpRuntime._theRuntime._fcm.StartMonitoringFile(text, new FileChangeEventHandler(HttpRuntime._theRuntime.OnAppOfflineFileChange));
			try
			{
				if (File.Exists(text))
				{
					using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						if (fileStream.Length <= 1048576L)
						{
							int num = (int)fileStream.Length;
							if (num > 0)
							{
								byte[] array = new byte[num];
								if (fileStream.Read(array, 0, num) == num)
								{
									HttpRuntime._theRuntime._appOfflineMessage = array;
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
					}
				}
			}
			catch
			{
			}
			if (flag)
			{
				throw new HttpException(404, string.Empty);
			}
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetAppConfig().HttpRuntime;
			if (!httpRuntime.Enable)
			{
				throw new HttpException(404, string.Empty);
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001D068 File Offset: 0x0001C068
		private void CheckAccessToTempDirectory()
		{
			if (HostingEnvironment.HasHostingIdentity)
			{
				using (new ApplicationImpersonationContext())
				{
					if (!Util.HasWriteAccessToDirectory(this._tempDir))
					{
						throw new HttpException(SR.GetString("No_codegen_access", new object[]
						{
							Util.GetCurrentAccountName(),
							this._tempDir
						}));
					}
				}
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001D0D4 File Offset: 0x0001C0D4
		private void InitializeHealthMonitoring()
		{
			ProcessModelSection processModel = RuntimeConfig.GetMachineConfig().ProcessModel;
			int num = (int)processModel.ResponseDeadlockInterval.TotalSeconds;
			int requestQueueLimit = processModel.RequestQueueLimit;
			UnsafeNativeMethods.InitializeHealthMonitor(num, requestQueueLimit);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001D10C File Offset: 0x0001C10C
		private static void InitHttpConfiguration()
		{
			if (!HttpRuntime._theRuntime._configInited)
			{
				HttpRuntime._theRuntime._configInited = true;
				HttpConfigurationSystem.EnsureInit(null, true, true);
				GlobalizationSection globalization = RuntimeConfig.GetAppLKGConfig().Globalization;
				if (globalization != null)
				{
					if (!string.IsNullOrEmpty(globalization.Culture) && !StringUtil.StringStartsWithIgnoreCase(globalization.Culture, "auto"))
					{
						Thread.CurrentThread.CurrentCulture = HttpServerUtility.CreateReadOnlyCultureInfo(globalization.Culture);
					}
					if (!string.IsNullOrEmpty(globalization.UICulture) && !StringUtil.StringStartsWithIgnoreCase(globalization.UICulture, "auto"))
					{
						Thread.CurrentThread.CurrentUICulture = HttpServerUtility.CreateReadOnlyCultureInfo(globalization.UICulture);
					}
				}
				RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
				ProcessModelSection processModel = appConfig.ProcessModel;
				HostingEnvironmentSection hostingEnvironment = appConfig.HostingEnvironment;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001D1C8 File Offset: 0x0001C1C8
		private void InitHeaderEncoding()
		{
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetAppConfig().HttpRuntime;
			this._enableHeaderChecking = httpRuntime.EnableHeaderChecking;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001D1EC File Offset: 0x0001C1EC
		private static void SetAutogenKeys()
		{
			byte[] array = new byte[HttpRuntime.s_autogenKeys.Length];
			byte[] array2 = new byte[HttpRuntime.s_autogenKeys.Length];
			bool flag = false;
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array);
			if (!flag)
			{
				flag = UnsafeNativeMethods.EcbCallISAPI(IntPtr.Zero, UnsafeNativeMethods.CallISAPIFunc.GetAutogenKeys, array, array.Length, array2, array2.Length) == 1;
			}
			if (flag)
			{
				Buffer.BlockCopy(array2, 0, HttpRuntime.s_autogenKeys, 0, HttpRuntime.s_autogenKeys.Length);
				return;
			}
			Buffer.BlockCopy(array, 0, HttpRuntime.s_autogenKeys, 0, HttpRuntime.s_autogenKeys.Length);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001D268 File Offset: 0x0001C268
		internal static void IncrementActivePipelineCount()
		{
			Interlocked.Increment(ref HttpRuntime._theRuntime._activeRequestCount);
			HostingEnvironment.IncrementBusyCount();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001D27F File Offset: 0x0001C27F
		internal static void DecrementActivePipelineCount()
		{
			HostingEnvironment.DecrementBusyCount();
			Interlocked.Decrement(ref HttpRuntime._theRuntime._activeRequestCount);
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001D296 File Offset: 0x0001C296
		public static bool UsingIntegratedPipeline
		{
			get
			{
				return HttpRuntime.UseIntegratedPipeline;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001D29D File Offset: 0x0001C29D
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0001D2A4 File Offset: 0x0001C2A4
		internal static bool UseIntegratedPipeline
		{
			get
			{
				return HttpRuntime._useIntegratedPipeline;
			}
			set
			{
				HttpRuntime._useIntegratedPipeline = value;
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001D2AC File Offset: 0x0001C2AC
		internal static RequestNotificationStatus ProcessRequestNotification(IIS7WorkerRequest wr, HttpContext context)
		{
			return HttpRuntime._theRuntime.ProcessRequestNotificationPrivate(wr, context);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001D2BC File Offset: 0x0001C2BC
		private RequestNotificationStatus ProcessRequestNotificationPrivate(IIS7WorkerRequest wr, HttpContext context)
		{
			RequestNotificationStatus requestNotificationStatus = RequestNotificationStatus.Pending;
			try
			{
				context.CurrentModuleIndex = UnsafeIISMethods.MgdGetCurrentModuleIndex(wr.RequestContext);
				context.IsPostNotification = UnsafeIISMethods.MgdIsCurrentNotificationPost(wr.RequestContext);
				context.CurrentNotification = (RequestNotification)UnsafeIISMethods.MgdGetCurrentNotification(wr.RequestContext);
				IHttpHandler httpHandler = null;
				if (context.NeedToInitializeApp())
				{
					try
					{
						this.EnsureFirstRequestInit(context);
					}
					catch
					{
						if (!context.Request.IsDebuggingRequest)
						{
							throw;
						}
					}
					context.Response.InitResponseWriter();
					httpHandler = HttpApplicationFactory.GetApplicationInstance(context);
					if (httpHandler == null)
					{
						throw new HttpException(SR.GetString("Unable_create_app_object"));
					}
					if (EtwTrace.IsTraceEnabled(5, 1))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_START_HANDLER, context.WorkerRequest, httpHandler.GetType().FullName, "Start");
					}
					HttpApplication httpApplication = httpHandler as HttpApplication;
					if (httpApplication != null)
					{
						httpApplication.AssignContext(context);
					}
				}
				wr.SynchronizeVariables(context);
				if (context.ApplicationInstance != null)
				{
					IAsyncResult asyncResult = context.ApplicationInstance.BeginProcessRequestNotification(context, this._requestNotificationCompletionCallback);
					if (asyncResult.CompletedSynchronously)
					{
						requestNotificationStatus = RequestNotificationStatus.Continue;
					}
				}
				else if (httpHandler != null)
				{
					httpHandler.ProcessRequest(context);
					requestNotificationStatus = RequestNotificationStatus.FinishRequest;
				}
				else
				{
					requestNotificationStatus = RequestNotificationStatus.Continue;
				}
			}
			catch (Exception ex)
			{
				requestNotificationStatus = RequestNotificationStatus.FinishRequest;
				context.Response.InitResponseWriter();
				context.AddError(ex);
			}
			if (requestNotificationStatus != RequestNotificationStatus.Pending)
			{
				this.FinishRequestNotification(wr, context, ref requestNotificationStatus);
			}
			return requestNotificationStatus;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001D404 File Offset: 0x0001C404
		private void FinishRequestNotification(IIS7WorkerRequest wr, HttpContext context, ref RequestNotificationStatus status)
		{
			HttpApplication applicationInstance = context.ApplicationInstance;
			if (context.NotificationContext.RequestCompleted)
			{
				status = RequestNotificationStatus.FinishRequest;
			}
			context.ReportRuntimeErrorIfExists(ref status);
			if (status == RequestNotificationStatus.FinishRequest && (context.CurrentNotification == RequestNotification.LogRequest || context.CurrentNotification == RequestNotification.EndRequest))
			{
				status = RequestNotificationStatus.Continue;
			}
			IntPtr requestContext = wr.RequestContext;
			bool flag = UnsafeIISMethods.MgdIsLastNotification(requestContext, status);
			try
			{
				context.Response.UpdateNativeResponse(flag);
			}
			catch (Exception ex)
			{
				wr.UnlockCachedResponseBytes();
				context.AddError(ex);
				context.ReportRuntimeErrorIfExists(ref status);
				context.Response.UpdateNativeResponse(flag);
			}
			if (flag)
			{
				context.FinishPipelineRequest();
			}
			if (status != RequestNotificationStatus.Pending)
			{
				PipelineRuntime.DisposeHandler(context, requestContext, status);
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001D4BC File Offset: 0x0001C4BC
		internal static void FinishPipelineRequest(HttpContext context)
		{
			HttpRuntime._theRuntime._firstRequestCompleted = true;
			context.Request.Dispose();
			context.Response.Dispose();
			HttpApplication applicationInstance = context.ApplicationInstance;
			if (applicationInstance != null)
			{
				HttpApplication.ThreadContext indicateCompletionContext = context.IndicateCompletionContext;
				if (indicateCompletionContext != null && !indicateCompletionContext.HasLeaveBeenCalled)
				{
					lock (indicateCompletionContext)
					{
						if (!indicateCompletionContext.HasLeaveBeenCalled)
						{
							indicateCompletionContext.Leave();
							context.IndicateCompletionContext = null;
							context.InIndicateCompletion = false;
						}
					}
				}
				applicationInstance.ReleaseAppInstance();
			}
			HttpRuntime.SetExecutionTimePerformanceCounter(context);
			HttpRuntime.UpdatePerfCounters(context.Response.StatusCode);
			if (EtwTrace.IsTraceEnabled(5, 1))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_END_HANDLER, context.WorkerRequest);
			}
			if (HttpRuntime.HostingInitFailed)
			{
				HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.HostingEnvironment, "HostingInit error");
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001D590 File Offset: 0x0001C590
		private void ProcessRequestInternal(HttpWorkerRequest wr)
		{
			HttpContext httpContext;
			try
			{
				httpContext = new HttpContext(wr, false);
			}
			catch
			{
				wr.SendStatus(400, "Bad Request");
				wr.SendKnownResponseHeader(12, "text/html; charset=utf-8");
				byte[] bytes = Encoding.ASCII.GetBytes("<html><body>Bad Request</body></html>");
				wr.SendResponseFromMemory(bytes, bytes.Length);
				wr.FlushResponse(true);
				wr.EndOfRequest();
				return;
			}
			wr.SetEndOfSendNotification(this._asyncEndOfSendCallback, httpContext);
			Interlocked.Increment(ref this._activeRequestCount);
			HostingEnvironment.IncrementBusyCount();
			try
			{
				try
				{
					this.EnsureFirstRequestInit(httpContext);
				}
				catch
				{
					if (!httpContext.Request.IsDebuggingRequest)
					{
						throw;
					}
				}
				httpContext.Response.InitResponseWriter();
				IHttpHandler applicationInstance = HttpApplicationFactory.GetApplicationInstance(httpContext);
				if (applicationInstance == null)
				{
					throw new HttpException(SR.GetString("Unable_create_app_object"));
				}
				if (EtwTrace.IsTraceEnabled(5, 1))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_START_HANDLER, httpContext.WorkerRequest, applicationInstance.GetType().FullName, "Start");
				}
				if (applicationInstance is IHttpAsyncHandler)
				{
					IHttpAsyncHandler httpAsyncHandler = (IHttpAsyncHandler)applicationInstance;
					httpContext.AsyncAppHandler = httpAsyncHandler;
					httpAsyncHandler.BeginProcessRequest(httpContext, this._handlerCompletionCallback, httpContext);
				}
				else
				{
					applicationInstance.ProcessRequest(httpContext);
					this.FinishRequest(httpContext.WorkerRequest, httpContext, null);
				}
			}
			catch (Exception ex)
			{
				httpContext.Response.InitResponseWriter();
				this.FinishRequest(wr, httpContext, ex);
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001D6F4 File Offset: 0x0001C6F4
		private void RejectRequestInternal(HttpWorkerRequest wr, bool silent)
		{
			HttpContext httpContext = new HttpContext(wr, false);
			wr.SetEndOfSendNotification(this._asyncEndOfSendCallback, httpContext);
			Interlocked.Increment(ref this._activeRequestCount);
			HostingEnvironment.IncrementBusyCount();
			if (silent)
			{
				httpContext.Response.InitResponseWriter();
				this.FinishRequest(wr, httpContext, null);
				return;
			}
			PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.REQUESTS_REJECTED);
			PerfCounters.IncrementCounter(AppPerfCounter.APP_REQUESTS_REJECTED);
			try
			{
				throw new HttpException(503, SR.GetString("Server_too_busy"));
			}
			catch (Exception ex)
			{
				httpContext.Response.InitResponseWriter();
				this.FinishRequest(wr, httpContext, ex);
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001D788 File Offset: 0x0001C788
		private void FinishRequest(HttpWorkerRequest wr, HttpContext context, Exception e)
		{
			HttpResponse response = context.Response;
			if (EtwTrace.IsTraceEnabled(5, 1))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_END_HANDLER, context.WorkerRequest);
			}
			HttpRuntime.SetExecutionTimePerformanceCounter(context);
			if (e == null)
			{
				using (new ClientImpersonationContext(context, false))
				{
					try
					{
						response.FinalFlushAtTheEndOfRequestProcessing();
					}
					catch (Exception ex)
					{
						e = ex;
					}
				}
			}
			if (e != null)
			{
				if (this._appOfflineMessage != null)
				{
					try
					{
						response.StatusCode = 404;
						response.OutputStream.Write(this._appOfflineMessage, 0, this._appOfflineMessage.Length);
						response.FinalFlushAtTheEndOfRequestProcessing();
						goto IL_00D6;
					}
					catch
					{
						goto IL_00D6;
					}
				}
				using (new HttpContextWrapper(context))
				{
					using (new ApplicationImpersonationContext())
					{
						try
						{
							try
							{
								response.ReportRuntimeError(e, true, false);
							}
							catch (Exception ex2)
							{
								response.ReportRuntimeError(ex2, false, false);
							}
							response.FinalFlushAtTheEndOfRequestProcessing();
						}
						catch
						{
						}
					}
				}
			}
			IL_00D6:
			this._firstRequestCompleted = true;
			if (this._hostingInitFailed)
			{
				HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.HostingEnvironment, "HostingInit error");
			}
			int statusCode = response.StatusCode;
			HttpRuntime.UpdatePerfCounters(statusCode);
			context.FinishRequestForCachedPathData(statusCode);
			wr.EndOfRequest();
			HostingEnvironment.DecrementBusyCount();
			Interlocked.Decrement(ref this._activeRequestCount);
			if (this._requestQueue != null)
			{
				this._requestQueue.ScheduleMoreWorkIfNeeded();
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001D91C File Offset: 0x0001C91C
		private bool InitiateShutdownOnce()
		{
			if (this._shutdownInProgress)
			{
				return false;
			}
			lock (this)
			{
				if (this._shutdownInProgress)
				{
					return false;
				}
				this._shutdownInProgress = true;
			}
			return true;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001D96C File Offset: 0x0001C96C
		private void ReleaseResourcesAndUnloadAppDomain(object state)
		{
			try
			{
				PerfCounters.IncrementGlobalCounter(GlobalPerfCounter.APPLICATION_RESTARTS);
			}
			catch
			{
			}
			try
			{
				this.Dispose();
			}
			catch
			{
			}
			Thread.Sleep(250);
			HttpRuntime.AddAppDomainTraceMessage("before Unload");
			try
			{
				for (;;)
				{
					IL_002A:
					AppDomain.Unload(Thread.GetDomain());
				}
			}
			catch (CannotUnloadAppDomainException)
			{
				goto IL_002A;
			}
			catch (Exception ex)
			{
				HttpRuntime.AddAppDomainTraceMessage("Unload Exception: " + ex);
				throw;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001D9F8 File Offset: 0x0001C9F8
		private static void SetExecutionTimePerformanceCounter(HttpContext context)
		{
			long num = DateTime.UtcNow.Subtract(context.WorkerRequest.GetStartTime()).Ticks / 10000L;
			if (num > 2147483647L)
			{
				num = 2147483647L;
			}
			PerfCounters.SetGlobalCounter(GlobalPerfCounter.REQUEST_EXECUTION_TIME, (int)num);
			PerfCounters.SetCounter(AppPerfCounter.APP_REQUEST_EXEC_TIME, (int)num);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001DA50 File Offset: 0x0001CA50
		private static void UpdatePerfCounters(int statusCode)
		{
			if (400 > statusCode)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_SUCCEDED);
				return;
			}
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_FAILED);
			if (statusCode == 401)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_AUTHORIZED);
				return;
			}
			if (statusCode != 404 && statusCode != 414)
			{
				return;
			}
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_FOUND);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001DAA0 File Offset: 0x0001CAA0
		private void WaitForRequestsToFinish(int waitTimeoutMs)
		{
			DateTime dateTime = DateTime.UtcNow.AddMilliseconds((double)waitTimeoutMs);
			for (;;)
			{
				if (this._activeRequestCount == 0)
				{
					if (this._requestQueue == null)
					{
						return;
					}
					if (this._requestQueue.IsEmpty)
					{
						break;
					}
				}
				Thread.Sleep(250);
				if (!Debugger.IsAttached && DateTime.UtcNow > dateTime)
				{
					return;
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001DAFC File Offset: 0x0001CAFC
		private void Dispose()
		{
			int num = 90;
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetAppLKGConfig().HttpRuntime;
			if (httpRuntime != null)
			{
				num = (int)httpRuntime.ShutdownTimeout.TotalSeconds;
			}
			this.WaitForRequestsToFinish(num * 1000);
			if (this._requestQueue != null)
			{
				this._requestQueue.Drain();
			}
			this.WaitForRequestsToFinish(num * 1000 / 6);
			ISAPIWorkerRequestInProcForIIS6.WaitForPendingAsyncIo();
			if (HttpRuntime.UseIntegratedPipeline)
			{
				PipelineRuntime.WaitForRequestsToDrain();
			}
			this.DisposeAppDomainShutdownTimer();
			this._timeoutManager.Stop();
			this.WaitForRequestsToFinish(num * 1000 / 6);
			ISAPIWorkerRequestInProcForIIS6.WaitForPendingAsyncIo();
			SqlCacheDependencyManager.Dispose(num * 1000 / 2);
			if (this._cacheInternal != null)
			{
				this._cacheInternal.Dispose();
			}
			HttpApplicationFactory.EndApplication();
			this._fcm.Stop();
			HealthMonitoringManager.Shutdown();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001DBC8 File Offset: 0x0001CBC8
		private void OnRequestNotificationCompletion(IAsyncResult ar)
		{
			try
			{
				this.OnRequestNotificationCompletionHelper(ar);
			}
			catch (Exception ex)
			{
				ApplicationManager.RecordFatalException(ex);
				throw;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001DBF8 File Offset: 0x0001CBF8
		private void OnRequestNotificationCompletionHelper(IAsyncResult ar)
		{
			if (ar.CompletedSynchronously)
			{
				return;
			}
			RequestNotificationStatus requestNotificationStatus = RequestNotificationStatus.Continue;
			HttpContext httpContext = (HttpContext)ar.AsyncState;
			IIS7WorkerRequest iis7WorkerRequest = httpContext.WorkerRequest as IIS7WorkerRequest;
			try
			{
				httpContext.ApplicationInstance.EndProcessRequestNotification(ar);
			}
			catch (Exception ex)
			{
				requestNotificationStatus = RequestNotificationStatus.FinishRequest;
				httpContext.AddError(ex);
			}
			IntPtr requestContext = iis7WorkerRequest.RequestContext;
			this.FinishRequestNotification(iis7WorkerRequest, httpContext, ref requestNotificationStatus);
			httpContext.NotificationContext = null;
			int num = UnsafeIISMethods.MgdPostCompletion(requestContext, requestNotificationStatus);
			Misc.ThrowIfFailedHr(num);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001DC80 File Offset: 0x0001CC80
		private void OnHandlerCompletion(IAsyncResult ar)
		{
			HttpContext httpContext = (HttpContext)ar.AsyncState;
			try
			{
				httpContext.AsyncAppHandler.EndProcessRequest(ar);
			}
			catch (Exception ex)
			{
				httpContext.AddError(ex);
			}
			finally
			{
				httpContext.AsyncAppHandler = null;
			}
			this.FinishRequest(httpContext.WorkerRequest, httpContext, httpContext.Error);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001DCEC File Offset: 0x0001CCEC
		private void EndOfSendCallback(HttpWorkerRequest wr, object arg)
		{
			HttpContext httpContext = (HttpContext)arg;
			httpContext.Request.Dispose();
			httpContext.Response.Dispose();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001DD18 File Offset: 0x0001CD18
		private void OnCriticalDirectoryChange(object sender, FileChangeEvent e)
		{
			ApplicationShutdownReason applicationShutdownReason = ApplicationShutdownReason.None;
			string name = new DirectoryInfo(e.FileName).Name;
			string text = name + " dir change or directory rename";
			if (StringUtil.EqualsIgnoreCase(name, "App_Code"))
			{
				applicationShutdownReason = ApplicationShutdownReason.CodeDirChangeOrDirectoryRename;
			}
			else if (StringUtil.EqualsIgnoreCase(name, "App_GlobalResources"))
			{
				applicationShutdownReason = ApplicationShutdownReason.ResourcesDirChangeOrDirectoryRename;
			}
			else if (StringUtil.EqualsIgnoreCase(name, "App_Browsers"))
			{
				applicationShutdownReason = ApplicationShutdownReason.BrowsersDirChangeOrDirectoryRename;
			}
			else if (StringUtil.EqualsIgnoreCase(name, "bin"))
			{
				applicationShutdownReason = ApplicationShutdownReason.BinDirChangeOrDirectoryRename;
			}
			if (e.Action == FileAction.Added)
			{
				HttpRuntime.SetUserForcedShutdown();
			}
			HttpRuntime.ShutdownAppDomain(applicationShutdownReason, text);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001DDA0 File Offset: 0x0001CDA0
		private void CoalesceNotifications()
		{
			int num = 0;
			int num2 = 0;
			try
			{
				HttpRuntimeSection httpRuntime = RuntimeConfig.GetAppLKGConfig().HttpRuntime;
				if (httpRuntime != null)
				{
					num = httpRuntime.WaitChangeNotification;
					num2 = httpRuntime.MaxWaitChangeNotification;
				}
			}
			catch
			{
			}
			if (num == 0 || num2 == 0)
			{
				return;
			}
			DateTime dateTime = DateTime.UtcNow.AddSeconds((double)num2);
			try
			{
				while (DateTime.UtcNow < dateTime && !(DateTime.UtcNow > this.LastShutdownAttemptTime.AddSeconds((double)num)))
				{
					Thread.Sleep(250);
				}
			}
			catch
			{
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060006BA RID: 1722 RVA: 0x0001DE44 File Offset: 0x0001CE44
		// (remove) Token: 0x060006BB RID: 1723 RVA: 0x0001DE5B File Offset: 0x0001CE5B
		internal static event BuildManagerHostUnloadEventHandler AppDomainShutdown;

		// Token: 0x060006BC RID: 1724 RVA: 0x0001DE72 File Offset: 0x0001CE72
		internal static void OnAppDomainShutdown(BuildManagerHostUnloadEventArgs e)
		{
			if (HttpRuntime.AppDomainShutdown != null)
			{
				HttpRuntime.AppDomainShutdown(HttpRuntime._theRuntime, e);
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001DE8B File Offset: 0x0001CE8B
		internal static void SetUserForcedShutdown()
		{
			HttpRuntime._theRuntime._userForcedShutdown = true;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001DE98 File Offset: 0x0001CE98
		internal static bool ShutdownAppDomain(ApplicationShutdownReason reason, string message)
		{
			return HttpRuntime.ShutdownAppDomainWithStackTrace(reason, message, null);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001DEA2 File Offset: 0x0001CEA2
		internal static bool ShutdownAppDomainWithStackTrace(ApplicationShutdownReason reason, string message, string stackTrace)
		{
			HttpRuntime.SetShutdownReason(reason, message);
			return HttpRuntime.ShutdownAppDomain(stackTrace);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001DEB4 File Offset: 0x0001CEB4
		private static bool ShutdownAppDomain(string stackTrace)
		{
			if (!HttpRuntime._theRuntime._firstRequestCompleted && !HttpRuntime._theRuntime._userForcedShutdown)
			{
				try
				{
					RuntimeConfig appLKGConfig = RuntimeConfig.GetAppLKGConfig();
					if (appLKGConfig != null)
					{
						HttpRuntimeSection httpRuntime = appLKGConfig.HttpRuntime;
						if (httpRuntime != null)
						{
							int num = (int)httpRuntime.DelayNotificationTimeout.TotalSeconds;
							if (DateTime.UtcNow < HttpRuntime._theRuntime._firstRequestStartTime.AddSeconds((double)num))
							{
								return false;
							}
						}
					}
				}
				catch
				{
				}
			}
			try
			{
				HttpRuntime._theRuntime.RaiseShutdownWebEventOnce();
			}
			catch
			{
			}
			HttpRuntime._theRuntime.LastShutdownAttemptTime = DateTime.UtcNow;
			HttpRuntime._theRuntime.CoalesceNotifications();
			if (!HostingEnvironment.ShutdownInitiated)
			{
				HostingEnvironment.InitiateShutdown();
				return true;
			}
			if (HostingEnvironment.ShutdownInProgress)
			{
				return false;
			}
			if (!HttpRuntime._theRuntime.InitiateShutdownOnce())
			{
				return false;
			}
			if (string.IsNullOrEmpty(stackTrace))
			{
				new EnvironmentPermission(PermissionState.Unrestricted).Assert();
				try
				{
					HttpRuntime._theRuntime._shutDownStack = Environment.StackTrace;
					goto IL_00E9;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			HttpRuntime._theRuntime._shutDownStack = stackTrace;
			IL_00E9:
			HttpRuntime.OnAppDomainShutdown(new BuildManagerHostUnloadEventArgs(HttpRuntime._theRuntime._shutdownReason));
			ThreadPool.QueueUserWorkItem(HttpRuntime._theRuntime._appDomainUnloadallback);
			return true;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001DFFC File Offset: 0x0001CFFC
		internal static void RecoverFromUnexceptedAppDomainUnload()
		{
			if (HttpRuntime._theRuntime._shutdownInProgress)
			{
				return;
			}
			HttpRuntime._theRuntime._shutdownInProgress = true;
			try
			{
				ISAPIRuntime.RemoveThisAppDomainFromUnmanagedTable();
				PipelineRuntime.RemoveThisAppDomainFromUnmanagedTable();
				HttpRuntime.AddAppDomainTraceMessage("AppDomainRestart");
			}
			finally
			{
				HttpRuntime._theRuntime.Dispose();
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001E054 File Offset: 0x0001D054
		internal static void OnConfigChange()
		{
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.ConfigurationChange, "CONFIG change");
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001E062 File Offset: 0x0001D062
		internal static void SetShutdownReason(ApplicationShutdownReason reason, string message)
		{
			if (HttpRuntime._theRuntime._shutdownReason == ApplicationShutdownReason.None)
			{
				HttpRuntime._theRuntime._shutdownReason = reason;
			}
			HttpRuntime.SetShutdownMessage(message);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001E081 File Offset: 0x0001D081
		internal static void SetShutdownMessage(string message)
		{
			if (message != null)
			{
				if (HttpRuntime._theRuntime._shutDownMessage == null)
				{
					HttpRuntime._theRuntime._shutDownMessage = message;
					return;
				}
				HttpRuntime theRuntime = HttpRuntime._theRuntime;
				theRuntime._shutDownMessage = theRuntime._shutDownMessage + "\r\n" + message;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001E0B9 File Offset: 0x0001D0B9
		internal static ApplicationShutdownReason ShutdownReason
		{
			get
			{
				return HttpRuntime._theRuntime._shutdownReason;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001E0C8 File Offset: 0x0001D0C8
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public static void ProcessRequest(HttpWorkerRequest wr)
		{
			if (wr == null)
			{
				throw new ArgumentNullException("wr");
			}
			if (HttpRuntime.UseIntegratedPipeline)
			{
				throw new PlatformNotSupportedException(SR.GetString("Method_Not_Supported_By_Iis_Integrated_Mode", new object[] { "HttpRuntime.ProcessRequest" }));
			}
			HttpRuntime.ProcessRequestNoDemand(wr);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001E110 File Offset: 0x0001D110
		internal static void ProcessRequestNoDemand(HttpWorkerRequest wr)
		{
			RequestQueue requestQueue = HttpRuntime._theRuntime._requestQueue;
			if (requestQueue != null)
			{
				wr = requestQueue.GetRequestToExecute(wr);
			}
			if (wr != null)
			{
				HttpRuntime.CalculateWaitTimeAndUpdatePerfCounter(wr);
				wr.ResetStartTime();
				HttpRuntime.ProcessRequestNow(wr);
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001E14C File Offset: 0x0001D14C
		private static void CalculateWaitTimeAndUpdatePerfCounter(HttpWorkerRequest wr)
		{
			DateTime startTime = wr.GetStartTime();
			long num = DateTime.UtcNow.Subtract(startTime).Ticks / 10000L;
			if (num > 2147483647L)
			{
				num = 2147483647L;
			}
			PerfCounters.SetGlobalCounter(GlobalPerfCounter.REQUEST_WAIT_TIME, (int)num);
			PerfCounters.SetCounter(AppPerfCounter.APP_REQUEST_WAIT_TIME, (int)num);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001E19F File Offset: 0x0001D19F
		internal static void ProcessRequestNow(HttpWorkerRequest wr)
		{
			HttpRuntime._theRuntime.ProcessRequestInternal(wr);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001E1AC File Offset: 0x0001D1AC
		internal static void RejectRequestNow(HttpWorkerRequest wr, bool silent)
		{
			HttpRuntime._theRuntime.RejectRequestInternal(wr, silent);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001E1BA File Offset: 0x0001D1BA
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static void Close()
		{
			if (HttpRuntime._theRuntime.InitiateShutdownOnce())
			{
				HttpRuntime.SetShutdownReason(ApplicationShutdownReason.HttpRuntimeClose, "HttpRuntime.Close is called");
				if (HostingEnvironment.IsHosted)
				{
					HostingEnvironment.InitiateShutdown();
					return;
				}
				HttpRuntime._theRuntime.Dispose();
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001E1EB File Offset: 0x0001D1EB
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static void UnloadAppDomain()
		{
			HttpRuntime._theRuntime._userForcedShutdown = true;
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.UnloadAppDomainCalled, "User code called UnloadAppDomain");
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001E204 File Offset: 0x0001D204
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x0001E23C File Offset: 0x0001D23C
		private DateTime LastShutdownAttemptTime
		{
			get
			{
				DateTime lastShutdownAttemptTime;
				lock (this)
				{
					lastShutdownAttemptTime = this._lastShutdownAttemptTime;
				}
				return lastShutdownAttemptTime;
			}
			set
			{
				lock (this)
				{
					this._lastShutdownAttemptTime = value;
				}
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001E274 File Offset: 0x0001D274
		internal static Profiler Profile
		{
			get
			{
				return HttpRuntime._theRuntime._profiler;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001E280 File Offset: 0x0001D280
		internal static bool IsTrustLevelInitialized
		{
			get
			{
				return !HostingEnvironment.IsHosted || HttpRuntime.TrustLevel != null;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001E296 File Offset: 0x0001D296
		internal static NamedPermissionSet NamedPermissionSet
		{
			get
			{
				return HttpRuntime._theRuntime._namedPermissionSet;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001E2A4 File Offset: 0x0001D2A4
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Unrestricted)]
		public static NamedPermissionSet GetNamedPermissionSet()
		{
			NamedPermissionSet namedPermissionSet = HttpRuntime._theRuntime._namedPermissionSet;
			if (namedPermissionSet == null)
			{
				return null;
			}
			return new NamedPermissionSet(namedPermissionSet);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001E2C7 File Offset: 0x0001D2C7
		internal static bool IsFullTrust
		{
			get
			{
				return HttpRuntime._theRuntime._namedPermissionSet == null;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001E2D8 File Offset: 0x0001D2D8
		internal static void CheckVirtualFilePermission(string virtualPath)
		{
			string text = HostingEnvironment.MapPath(virtualPath);
			HttpRuntime.CheckFilePermission(text);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001E2F2 File Offset: 0x0001D2F2
		internal static void CheckFilePermission(string path)
		{
			HttpRuntime.CheckFilePermission(path, false);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001E2FC File Offset: 0x0001D2FC
		internal static void CheckFilePermission(string path, bool writePermissions)
		{
			if (!HttpRuntime.HasFilePermission(path, writePermissions))
			{
				throw new HttpException(SR.GetString("Access_denied_to_path", new object[] { HttpRuntime.GetSafePath(path) }));
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001E333 File Offset: 0x0001D333
		internal static bool HasFilePermission(string path)
		{
			return HttpRuntime.HasFilePermission(path, false);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001E33C File Offset: 0x0001D33C
		internal static bool HasFilePermission(string path, bool writePermissions)
		{
			if (HttpRuntime.TrustLevel == null && HttpRuntime.InitializationException != null)
			{
				return true;
			}
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			bool flag = false;
			IPermission permission = HttpRuntime.NamedPermissionSet.GetPermission(typeof(FileIOPermission));
			if (permission != null)
			{
				IPermission permission2 = null;
				try
				{
					if (!writePermissions)
					{
						permission2 = new FileIOPermission(FileIOPermissionAccess.Read, path);
					}
					else
					{
						permission2 = new FileIOPermission(FileIOPermissionAccess.AllAccess, path);
					}
				}
				catch
				{
					return false;
				}
				return permission2.IsSubsetOf(permission);
			}
			return flag;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001E3B8 File Offset: 0x0001D3B8
		internal static bool HasWebPermission(Uri uri)
		{
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			bool flag = false;
			IPermission permission = HttpRuntime.NamedPermissionSet.GetPermission(typeof(WebPermission));
			if (permission != null)
			{
				IPermission permission2 = null;
				try
				{
					permission2 = new WebPermission(NetworkAccess.Connect, uri.ToString());
				}
				catch
				{
					return false;
				}
				return permission2.IsSubsetOf(permission);
			}
			return flag;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001E41C File Offset: 0x0001D41C
		internal static bool HasDbPermission(DbProviderFactory factory)
		{
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			bool flag = false;
			CodeAccessPermission codeAccessPermission = factory.CreatePermission(PermissionState.Unrestricted);
			if (codeAccessPermission != null)
			{
				IPermission permission = HttpRuntime.NamedPermissionSet.GetPermission(codeAccessPermission.GetType());
				if (permission != null)
				{
					flag = codeAccessPermission.IsSubsetOf(permission);
				}
			}
			return flag;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001E45C File Offset: 0x0001D45C
		internal static bool HasPathDiscoveryPermission(string path)
		{
			if (HttpRuntime.TrustLevel == null && HttpRuntime.InitializationException != null)
			{
				return true;
			}
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			bool flag = false;
			IPermission permission = HttpRuntime.NamedPermissionSet.GetPermission(typeof(FileIOPermission));
			if (permission != null)
			{
				IPermission permission2 = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path);
				flag = permission2.IsSubsetOf(permission);
			}
			return flag;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001E4AD File Offset: 0x0001D4AD
		internal static bool HasAppPathDiscoveryPermission()
		{
			return HttpRuntime.HasPathDiscoveryPermission(HttpRuntime.AppDomainAppPathInternal);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001E4BC File Offset: 0x0001D4BC
		internal static string GetSafePath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			try
			{
				if (HttpRuntime.HasPathDiscoveryPermission(path))
				{
					return path;
				}
			}
			catch
			{
			}
			return Path.GetFileName(path);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001E4FC File Offset: 0x0001D4FC
		internal static bool HasUnmanagedPermission()
		{
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			SecurityPermission securityPermission = (SecurityPermission)HttpRuntime.NamedPermissionSet.GetPermission(typeof(SecurityPermission));
			return securityPermission != null && (securityPermission.Flags & SecurityPermissionFlag.UnmanagedCode) != SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001E540 File Offset: 0x0001D540
		internal static bool HasAspNetHostingPermission(AspNetHostingPermissionLevel level)
		{
			if (HttpRuntime.NamedPermissionSet == null)
			{
				return true;
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)HttpRuntime.NamedPermissionSet.GetPermission(typeof(AspNetHostingPermission));
			return aspNetHostingPermission != null && aspNetHostingPermission.Level >= level;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001E581 File Offset: 0x0001D581
		internal static void CheckAspNetHostingPermission(AspNetHostingPermissionLevel level, string errorMessageId)
		{
			if (!HttpRuntime.HasAspNetHostingPermission(level))
			{
				throw new HttpException(SR.GetString(errorMessageId));
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001E598 File Offset: 0x0001D598
		internal static void FailIfNoAPTCABit(Type t, ElementInformation elemInfo, string propertyName)
		{
			if (HttpRuntime.IsTypeAllowedInConfig(t))
			{
				return;
			}
			if (elemInfo != null)
			{
				PropertyInformation propertyInformation = elemInfo.Properties[propertyName];
				throw new ConfigurationErrorsException(SR.GetString("Type_from_untrusted_assembly", new object[] { t.FullName }), propertyInformation.Source, propertyInformation.LineNumber);
			}
			throw new ConfigurationErrorsException(SR.GetString("Type_from_untrusted_assembly", new object[] { t.FullName }));
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001E60C File Offset: 0x0001D60C
		internal static void FailIfNoAPTCABit(Type t, XmlNode node)
		{
			if (!HttpRuntime.IsTypeAllowedInConfig(t))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_from_untrusted_assembly", new object[] { t.FullName }), node);
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001E644 File Offset: 0x0001D644
		private static bool HasAPTCABit(Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(typeof(AllowPartiallyTrustedCallersAttribute), false);
			return customAttributes != null && customAttributes.Length > 0;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001E670 File Offset: 0x0001D670
		internal static bool IsTypeAllowedInConfig(Type t)
		{
			if (HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Unrestricted))
			{
				return true;
			}
			Assembly assembly = t.Assembly;
			return !assembly.GlobalAssemblyCache || HttpRuntime.HasAPTCABit(assembly);
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001E6A7 File Offset: 0x0001D6A7
		internal static FileChangesMonitor FileChangesMonitor
		{
			get
			{
				return HttpRuntime._theRuntime._fcm;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001E6B3 File Offset: 0x0001D6B3
		internal static RequestTimeoutManager RequestTimeoutManager
		{
			get
			{
				return HttpRuntime._theRuntime._timeoutManager;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001E6C0 File Offset: 0x0001D6C0
		public static Cache Cache
		{
			get
			{
				if (HttpRuntime.AspInstallDirectoryInternal == null)
				{
					throw new HttpException(SR.GetString("Aspnet_not_installed", new object[] { VersionInfo.SystemWebVersion }));
				}
				Cache cache = HttpRuntime._theRuntime._cachePublic;
				if (cache == null)
				{
					CacheInternal cacheInternal = HttpRuntime.CacheInternal;
					CacheSection cache2 = RuntimeConfig.GetAppConfig().Cache;
					cacheInternal.ReadCacheInternalConfig(cache2);
					HttpRuntime._theRuntime._cachePublic = cacheInternal.CachePublic;
					cache = HttpRuntime._theRuntime._cachePublic;
				}
				return cache;
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001E738 File Offset: 0x0001D738
		private void CreateCache()
		{
			lock (this)
			{
				if (this._cacheInternal == null)
				{
					this._cacheInternal = CacheInternal.Create();
				}
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001E77C File Offset: 0x0001D77C
		internal static CacheInternal CacheInternal
		{
			get
			{
				CacheInternal cacheInternal = HttpRuntime._theRuntime._cacheInternal;
				if (cacheInternal == null)
				{
					HttpRuntime._theRuntime.CreateCache();
					cacheInternal = HttpRuntime._theRuntime._cacheInternal;
				}
				return cacheInternal;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001E7B0 File Offset: 0x0001D7B0
		public static string AspInstallDirectory
		{
			get
			{
				string aspInstallDirectoryInternal = HttpRuntime.AspInstallDirectoryInternal;
				if (aspInstallDirectoryInternal == null)
				{
					throw new HttpException(SR.GetString("Aspnet_not_installed", new object[] { VersionInfo.SystemWebVersion }));
				}
				InternalSecurityPermissions.PathDiscovery(aspInstallDirectoryInternal).Demand();
				return aspInstallDirectoryInternal;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001E7F2 File Offset: 0x0001D7F2
		internal static string AspInstallDirectoryInternal
		{
			get
			{
				return HttpRuntime.s_installDirectory;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001E7FC File Offset: 0x0001D7FC
		public static string AspClientScriptVirtualPath
		{
			get
			{
				if (HttpRuntime._theRuntime._clientScriptVirtualPath == null)
				{
					string systemWebVersion = VersionInfo.SystemWebVersion;
					string text = "/aspnet_client/system_web/" + systemWebVersion.Substring(0, systemWebVersion.LastIndexOf('.')).Replace('.', '_');
					HttpRuntime._theRuntime._clientScriptVirtualPath = text;
				}
				return HttpRuntime._theRuntime._clientScriptVirtualPath;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0001E854 File Offset: 0x0001D854
		public static string AspClientScriptPhysicalPath
		{
			get
			{
				string aspClientScriptPhysicalPathInternal = HttpRuntime.AspClientScriptPhysicalPathInternal;
				if (aspClientScriptPhysicalPathInternal == null)
				{
					throw new HttpException(SR.GetString("Aspnet_not_installed", new object[] { VersionInfo.SystemWebVersion }));
				}
				InternalSecurityPermissions.PathDiscovery(aspClientScriptPhysicalPathInternal).Demand();
				return aspClientScriptPhysicalPathInternal;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001E898 File Offset: 0x0001D898
		internal static string AspClientScriptPhysicalPathInternal
		{
			get
			{
				if (HttpRuntime._theRuntime._clientScriptPhysicalPath == null)
				{
					string text = Path.Combine(HttpRuntime.AspInstallDirectoryInternal, "asp.netclientfiles");
					HttpRuntime._theRuntime._clientScriptPhysicalPath = text;
				}
				return HttpRuntime._theRuntime._clientScriptPhysicalPath;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001E8D8 File Offset: 0x0001D8D8
		public static string ClrInstallDirectory
		{
			get
			{
				string clrInstallDirectoryInternal = HttpRuntime.ClrInstallDirectoryInternal;
				InternalSecurityPermissions.PathDiscovery(clrInstallDirectoryInternal).Demand();
				return clrInstallDirectoryInternal;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001E8F7 File Offset: 0x0001D8F7
		internal static string ClrInstallDirectoryInternal
		{
			get
			{
				return HttpConfigurationSystem.MsCorLibDirectory;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001E900 File Offset: 0x0001D900
		public static string MachineConfigurationDirectory
		{
			get
			{
				string machineConfigurationDirectoryInternal = HttpRuntime.MachineConfigurationDirectoryInternal;
				InternalSecurityPermissions.PathDiscovery(machineConfigurationDirectoryInternal).Demand();
				return machineConfigurationDirectoryInternal;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0001E91F File Offset: 0x0001D91F
		internal static string MachineConfigurationDirectoryInternal
		{
			get
			{
				return HttpConfigurationSystem.MachineConfigurationDirectory;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001E926 File Offset: 0x0001D926
		internal static bool IsEngineLoaded
		{
			get
			{
				return HttpRuntime.s_isEngineLoaded;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001E930 File Offset: 0x0001D930
		public static string CodegenDir
		{
			get
			{
				string codegenDirInternal = HttpRuntime.CodegenDirInternal;
				InternalSecurityPermissions.PathDiscovery(codegenDirInternal).Demand();
				return codegenDirInternal;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001E94F File Offset: 0x0001D94F
		internal static string CodegenDirInternal
		{
			get
			{
				return HttpRuntime._theRuntime._codegenDir;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001E95B File Offset: 0x0001D95B
		internal static string TempDirInternal
		{
			get
			{
				return HttpRuntime._theRuntime._tempDir;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001E967 File Offset: 0x0001D967
		public static string AppDomainAppId
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
			get
			{
				return HttpRuntime.AppDomainAppIdInternal;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001E96E File Offset: 0x0001D96E
		internal static string AppDomainAppIdInternal
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppId;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001E97A File Offset: 0x0001D97A
		internal static bool IsAspNetAppDomain
		{
			get
			{
				return HttpRuntime.AppDomainAppIdInternal != null;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001E987 File Offset: 0x0001D987
		public static string AppDomainAppPath
		{
			get
			{
				InternalSecurityPermissions.AppPathDiscovery.Demand();
				return HttpRuntime.AppDomainAppPathInternal;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001E998 File Offset: 0x0001D998
		internal static string AppDomainAppPathInternal
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppPath;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001E9A4 File Offset: 0x0001D9A4
		public static string AppDomainAppVirtualPath
		{
			get
			{
				return VirtualPath.GetVirtualPathStringNoTrailingSlash(HttpRuntime._theRuntime._appDomainAppVPath);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001E9B5 File Offset: 0x0001D9B5
		internal static string AppDomainAppVirtualPathString
		{
			get
			{
				return VirtualPath.GetVirtualPathString(HttpRuntime._theRuntime._appDomainAppVPath);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001E9C6 File Offset: 0x0001D9C6
		internal static VirtualPath AppDomainAppVirtualPathObject
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppVPath;
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001E9D2 File Offset: 0x0001D9D2
		internal static bool IsPathWithinAppRoot(string path)
		{
			return HttpRuntime.AppDomainIdInternal == null || UrlPath.IsEqualOrSubpath(HttpRuntime.AppDomainAppVirtualPathString, path);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001E9E8 File Offset: 0x0001D9E8
		public static string AppDomainId
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.High)]
			get
			{
				return HttpRuntime.AppDomainIdInternal;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001E9EF File Offset: 0x0001D9EF
		internal static string AppDomainIdInternal
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainId;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001E9FC File Offset: 0x0001D9FC
		public static string BinDirectory
		{
			get
			{
				string binDirectoryInternal = HttpRuntime.BinDirectoryInternal;
				InternalSecurityPermissions.PathDiscovery(binDirectoryInternal).Demand();
				return binDirectoryInternal;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001EA1B File Offset: 0x0001DA1B
		internal static string BinDirectoryInternal
		{
			get
			{
				return Path.Combine(HttpRuntime._theRuntime._appDomainAppPath, "bin") + Path.DirectorySeparatorChar;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001EA40 File Offset: 0x0001DA40
		internal static VirtualPath CodeDirectoryVirtualPath
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppVPath.SimpleCombineWithDir("App_Code");
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001EA56 File Offset: 0x0001DA56
		internal static VirtualPath ResourcesDirectoryVirtualPath
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppVPath.SimpleCombineWithDir("App_GlobalResources");
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001EA6C File Offset: 0x0001DA6C
		internal static VirtualPath WebRefDirectoryVirtualPath
		{
			get
			{
				return HttpRuntime._theRuntime._appDomainAppVPath.SimpleCombineWithDir("App_WebReferences");
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001EA82 File Offset: 0x0001DA82
		public static bool IsOnUNCShare
		{
			[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
			get
			{
				return HttpRuntime.IsOnUNCShareInternal;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001EA89 File Offset: 0x0001DA89
		internal static bool IsOnUNCShareInternal
		{
			get
			{
				return HttpRuntime._theRuntime._isOnUNCShare;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001EA98 File Offset: 0x0001DA98
		private static string GetAppDomainString(string key)
		{
			object data = Thread.GetDomain().GetData(key);
			return data as string;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001EAB8 File Offset: 0x0001DAB8
		internal static void AddAppDomainTraceMessage(string message)
		{
			AppDomain domain = Thread.GetDomain();
			string text = domain.GetData("ASP.NET Domain Trace") as string;
			domain.SetData("ASP.NET Domain Trace", (text != null) ? (text + " ... " + message) : message);
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001EAF9 File Offset: 0x0001DAF9
		internal static bool DebuggingEnabled
		{
			get
			{
				return HttpRuntime._theRuntime._debuggingEnabled;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001EB05 File Offset: 0x0001DB05
		internal static bool ConfigInited
		{
			get
			{
				return HttpRuntime._theRuntime._configInited;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001EB11 File Offset: 0x0001DB11
		internal static bool FusionInited
		{
			get
			{
				return HttpRuntime._theRuntime._fusionInited;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001EB1D File Offset: 0x0001DB1D
		internal static bool ApartmentThreading
		{
			get
			{
				return HttpRuntime._theRuntime._apartmentThreading;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001EB29 File Offset: 0x0001DB29
		internal static bool ShutdownInProgress
		{
			get
			{
				return HttpRuntime._theRuntime._shutdownInProgress;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001EB35 File Offset: 0x0001DB35
		internal static string TrustLevel
		{
			get
			{
				return HttpRuntime._theRuntime._trustLevel;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001EB41 File Offset: 0x0001DB41
		internal static string WpUserId
		{
			get
			{
				return HttpRuntime._theRuntime._wpUserId;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001EB50 File Offset: 0x0001DB50
		private void SetTrustLevel(TrustSection trustSection, SecurityPolicySection securityPolicySection)
		{
			if (trustSection == null || string.IsNullOrEmpty(trustSection.Level))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_section_not_present", new object[] { "trust" }));
			}
			string level = trustSection.Level;
			if (trustSection.Level == "Full")
			{
				this._trustLevel = level;
				return;
			}
			if (securityPolicySection == null || securityPolicySection.TrustLevels[trustSection.Level] == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Unable_to_get_policy_file", new object[] { trustSection.Level }), string.Empty, 0);
			}
			string policyFileExpanded = securityPolicySection.TrustLevels[trustSection.Level].PolicyFileExpanded;
			if (policyFileExpanded == null || !FileUtil.FileExists(policyFileExpanded))
			{
				throw new HttpException(SR.GetString("Unable_to_get_policy_file", new object[] { trustSection.Level }));
			}
			bool flag = false;
			PolicyLevel policyLevel = HttpRuntime.CreatePolicyLevel(policyFileExpanded, HttpRuntime.AppDomainAppPathInternal, HttpRuntime.CodegenDirInternal, trustSection.OriginUrl, out flag);
			if (flag)
			{
				CodeGroup rootCodeGroup = policyLevel.RootCodeGroup;
				bool flag2 = false;
				foreach (object obj in rootCodeGroup.Children)
				{
					CodeGroup codeGroup = (CodeGroup)obj;
					if (codeGroup.MembershipCondition is GacMembershipCondition)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && rootCodeGroup is FirstMatchCodeGroup)
				{
					FirstMatchCodeGroup firstMatchCodeGroup = (FirstMatchCodeGroup)rootCodeGroup;
					if (firstMatchCodeGroup.MembershipCondition is AllMembershipCondition && firstMatchCodeGroup.PermissionSetName == "Nothing")
					{
						PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
						CodeGroup codeGroup2 = new UnionCodeGroup(new GacMembershipCondition(), new PolicyStatement(permissionSet));
						CodeGroup codeGroup3 = new FirstMatchCodeGroup(rootCodeGroup.MembershipCondition, rootCodeGroup.PolicyStatement);
						foreach (object obj2 in rootCodeGroup.Children)
						{
							CodeGroup codeGroup4 = (CodeGroup)obj2;
							if (codeGroup4 is UnionCodeGroup && codeGroup4.MembershipCondition is UrlMembershipCondition && codeGroup4.PolicyStatement.PermissionSet.IsUnrestricted() && codeGroup2 != null)
							{
								codeGroup3.AddChild(codeGroup2);
								codeGroup2 = null;
							}
							codeGroup3.AddChild(codeGroup4);
						}
						policyLevel.RootCodeGroup = codeGroup3;
					}
				}
			}
			AppDomain.CurrentDomain.SetAppDomainPolicy(policyLevel);
			this._namedPermissionSet = policyLevel.GetNamedPermissionSet("ASP.Net");
			this._trustLevel = level;
			this._fcm.StartMonitoringFile(policyFileExpanded, new FileChangeEventHandler(this.OnSecurityPolicyFileChange));
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001EE08 File Offset: 0x0001DE08
		private static PolicyLevel CreatePolicyLevel(string configFile, string appDir, string binDir, string strOriginUrl, out bool foundGacToken)
		{
			FileStream fileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
			StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			appDir = FileUtil.RemoveTrailingDirectoryBackSlash(appDir);
			binDir = FileUtil.RemoveTrailingDirectoryBackSlash(binDir);
			text = text.Replace("$AppDir$", appDir);
			text = text.Replace("$AppDirUrl$", HttpRuntime.MakeFileUrl(appDir));
			text = text.Replace("$CodeGen$", HttpRuntime.MakeFileUrl(binDir));
			if (strOriginUrl == null)
			{
				strOriginUrl = string.Empty;
			}
			text = text.Replace("$OriginHost$", strOriginUrl);
			int num = text.IndexOf("$Gac$", StringComparison.Ordinal);
			if (num != -1)
			{
				string text2 = HttpRuntime.GetGacLocation();
				if (text2 != null)
				{
					text2 = HttpRuntime.MakeFileUrl(text2);
				}
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				text = text.Replace("$Gac$", text2);
				foundGacToken = true;
			}
			else
			{
				foundGacToken = false;
			}
			return SecurityManager.LoadPolicyLevelFromString(text, PolicyLevelType.AppDomain);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001EEDE File Offset: 0x0001DEDE
		private void OnSecurityPolicyFileChange(object sender, FileChangeEvent e)
		{
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.ChangeInSecurityPolicyFile, "Change in code-access security policy file");
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001EEEC File Offset: 0x0001DEEC
		private void OnAppOfflineFileChange(object sender, FileChangeEvent e)
		{
			HttpRuntime.SetUserForcedShutdown();
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.ConfigurationChange, "Change in App_Offline.htm");
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001EF00 File Offset: 0x0001DF00
		private static string MakeFileUrl(string path)
		{
			Uri uri = new Uri(path);
			return uri.ToString();
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001EF1C File Offset: 0x0001DF1C
		private static string GetGacLocation()
		{
			StringBuilder stringBuilder = new StringBuilder(262);
			int num = 260;
			if (UnsafeNativeMethods.GetCachePath(2, stringBuilder, ref num) >= 0)
			{
				return stringBuilder.ToString();
			}
			throw new HttpException(SR.GetString("GetGacLocaltion_failed"));
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001EF5C File Offset: 0x0001DF5C
		internal static void RestrictIISFolders(HttpContext context)
		{
			HttpWorkerRequest workerRequest = context.WorkerRequest;
			if (workerRequest == null || !(workerRequest is ISAPIWorkerRequest))
			{
				return;
			}
			if (!(workerRequest is ISAPIWorkerRequestInProcForIIS6))
			{
				byte[] array = new byte[1];
				byte[] bytes = BitConverter.GetBytes(1);
				int num = context.CallISAPI(UnsafeNativeMethods.CallISAPIFunc.RestrictIISFolders, bytes, array);
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001EFA0 File Offset: 0x0001DFA0
		internal static object CreateNonPublicInstance(Type type)
		{
			return HttpRuntime.CreateNonPublicInstance(type, null);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001EFA9 File Offset: 0x0001DFA9
		internal static object CreateNonPublicInstance(Type type, object[] args)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001EFB9 File Offset: 0x0001DFB9
		internal static object CreatePublicInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001EFC4 File Offset: 0x0001DFC4
		internal static object FastCreatePublicInstance(Type type)
		{
			if (!type.Assembly.GlobalAssemblyCache)
			{
				return HttpRuntime.CreatePublicInstance(type);
			}
			if (!HttpRuntime.s_initializedFactory)
			{
				lock (HttpRuntime.s_factoryLock)
				{
					if (!HttpRuntime.s_initializedFactory)
					{
						HttpRuntime.s_factoryGenerator = new FactoryGenerator();
						HttpRuntime.s_factoryCache = Hashtable.Synchronized(new Hashtable());
						HttpRuntime.s_initializedFactory = true;
					}
				}
			}
			IWebObjectFactory webObjectFactory = (IWebObjectFactory)HttpRuntime.s_factoryCache[type];
			if (webObjectFactory == null)
			{
				webObjectFactory = HttpRuntime.s_factoryGenerator.CreateFactory(type);
				HttpRuntime.s_factoryCache[type] = webObjectFactory;
			}
			return webObjectFactory.CreateInstance();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001F06C File Offset: 0x0001E06C
		internal static object CreatePublicInstance(Type type, object[] args)
		{
			if (args == null)
			{
				return Activator.CreateInstance(type);
			}
			return Activator.CreateInstance(type, args);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001F080 File Offset: 0x0001E080
		private static string GetCurrentUserName()
		{
			string text;
			try
			{
				text = WindowsIdentity.GetCurrent().Name;
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001F0B0 File Offset: 0x0001E0B0
		private void RaiseShutdownWebEventOnce()
		{
			if (!this._shutdownWebEventRaised)
			{
				lock (this)
				{
					if (!this._shutdownWebEventRaised)
					{
						WebBaseEvent.RaiseSystemEvent(this, 1002, WebApplicationLifetimeEvent.DetailCodeFromShutdownReason(HttpRuntime.ShutdownReason));
						this._shutdownWebEventRaised = true;
					}
				}
			}
		}

		// Token: 0x040010F2 RID: 4338
		private const string codegenDirName = "Temporary ASP.NET Files";

		// Token: 0x040010F3 RID: 4339
		internal const string BinDirectoryName = "bin";

		// Token: 0x040010F4 RID: 4340
		internal const string CodeDirectoryName = "App_Code";

		// Token: 0x040010F5 RID: 4341
		internal const string WebRefDirectoryName = "App_WebReferences";

		// Token: 0x040010F6 RID: 4342
		internal const string ResourcesDirectoryName = "App_GlobalResources";

		// Token: 0x040010F7 RID: 4343
		internal const string LocalResourcesDirectoryName = "App_LocalResources";

		// Token: 0x040010F8 RID: 4344
		internal const string DataDirectoryName = "App_Data";

		// Token: 0x040010F9 RID: 4345
		internal const string ThemesDirectoryName = "App_Themes";

		// Token: 0x040010FA RID: 4346
		internal const string GlobalThemesDirectoryName = "Themes";

		// Token: 0x040010FB RID: 4347
		internal const string BrowsersDirectoryName = "App_Browsers";

		// Token: 0x040010FC RID: 4348
		private const string AppOfflineFileName = "App_Offline.htm";

		// Token: 0x040010FD RID: 4349
		private const long MaxAppOfflineFileLength = 1048576L;

		// Token: 0x040010FE RID: 4350
		private const string AspNetClientFilesSubDirectory = "asp.netclientfiles";

		// Token: 0x040010FF RID: 4351
		private const string AspNetClientFilesParentVirtualPath = "/aspnet_client/system_web/";

		// Token: 0x04001100 RID: 4352
		private static HttpRuntime _theRuntime;

		// Token: 0x04001101 RID: 4353
		internal static byte[] s_autogenKeys = new byte[88];

		// Token: 0x04001102 RID: 4354
		private static string DirectorySeparatorString = new string(Path.DirectorySeparatorChar, 1);

		// Token: 0x04001103 RID: 4355
		private static string DoubleDirectorySeparatorString = new string(Path.DirectorySeparatorChar, 2);

		// Token: 0x04001104 RID: 4356
		private static bool s_initialized = false;

		// Token: 0x04001105 RID: 4357
		private static string s_installDirectory;

		// Token: 0x04001106 RID: 4358
		private static bool s_isEngineLoaded = false;

		// Token: 0x04001107 RID: 4359
		private NamedPermissionSet _namedPermissionSet;

		// Token: 0x04001108 RID: 4360
		private FileChangesMonitor _fcm;

		// Token: 0x04001109 RID: 4361
		private CacheInternal _cacheInternal;

		// Token: 0x0400110A RID: 4362
		private Cache _cachePublic;

		// Token: 0x0400110B RID: 4363
		private bool _isOnUNCShare;

		// Token: 0x0400110C RID: 4364
		private Profiler _profiler;

		// Token: 0x0400110D RID: 4365
		private RequestTimeoutManager _timeoutManager;

		// Token: 0x0400110E RID: 4366
		private RequestQueue _requestQueue;

		// Token: 0x0400110F RID: 4367
		private bool _apartmentThreading;

		// Token: 0x04001110 RID: 4368
		private bool _processRequestInApplicationTrust;

		// Token: 0x04001111 RID: 4369
		private bool _beforeFirstRequest = true;

		// Token: 0x04001112 RID: 4370
		private DateTime _firstRequestStartTime;

		// Token: 0x04001113 RID: 4371
		private bool _firstRequestCompleted;

		// Token: 0x04001114 RID: 4372
		private bool _userForcedShutdown;

		// Token: 0x04001115 RID: 4373
		private bool _configInited;

		// Token: 0x04001116 RID: 4374
		private bool _fusionInited;

		// Token: 0x04001117 RID: 4375
		private int _activeRequestCount;

		// Token: 0x04001118 RID: 4376
		private DateTime _lastShutdownAttemptTime;

		// Token: 0x04001119 RID: 4377
		private bool _shutdownInProgress;

		// Token: 0x0400111A RID: 4378
		private string _shutDownStack;

		// Token: 0x0400111B RID: 4379
		private string _shutDownMessage;

		// Token: 0x0400111C RID: 4380
		private ApplicationShutdownReason _shutdownReason;

		// Token: 0x0400111D RID: 4381
		private string _trustLevel;

		// Token: 0x0400111E RID: 4382
		private string _wpUserId;

		// Token: 0x0400111F RID: 4383
		private bool _shutdownWebEventRaised;

		// Token: 0x04001120 RID: 4384
		private bool _enableHeaderChecking;

		// Token: 0x04001121 RID: 4385
		private AsyncCallback _requestNotificationCompletionCallback;

		// Token: 0x04001122 RID: 4386
		private AsyncCallback _handlerCompletionCallback;

		// Token: 0x04001123 RID: 4387
		private HttpWorkerRequest.EndOfSendNotification _asyncEndOfSendCallback;

		// Token: 0x04001124 RID: 4388
		private WaitCallback _appDomainUnloadallback;

		// Token: 0x04001125 RID: 4389
		private Exception _initializationError;

		// Token: 0x04001126 RID: 4390
		private bool _hostingInitFailed;

		// Token: 0x04001127 RID: 4391
		private Timer _appDomainShutdownTimer;

		// Token: 0x04001128 RID: 4392
		private string _tempDir;

		// Token: 0x04001129 RID: 4393
		private string _codegenDir;

		// Token: 0x0400112A RID: 4394
		private string _appDomainAppId;

		// Token: 0x0400112B RID: 4395
		private string _appDomainAppPath;

		// Token: 0x0400112C RID: 4396
		private VirtualPath _appDomainAppVPath;

		// Token: 0x0400112D RID: 4397
		private string _appDomainId;

		// Token: 0x0400112E RID: 4398
		private bool _debuggingEnabled;

		// Token: 0x0400112F RID: 4399
		private byte[] _appOfflineMessage;

		// Token: 0x04001130 RID: 4400
		private string _clientScriptVirtualPath;

		// Token: 0x04001131 RID: 4401
		private string _clientScriptPhysicalPath;

		// Token: 0x04001132 RID: 4402
		private static bool _useIntegratedPipeline;

		// Token: 0x04001134 RID: 4404
		private static FactoryGenerator s_factoryGenerator;

		// Token: 0x04001135 RID: 4405
		private static Hashtable s_factoryCache;

		// Token: 0x04001136 RID: 4406
		private static bool s_initializedFactory;

		// Token: 0x04001137 RID: 4407
		private static object s_factoryLock = new object();
	}
}
