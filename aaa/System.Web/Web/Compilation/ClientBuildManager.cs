using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Threading;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200015D RID: 349
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public sealed class ClientBuildManager : MarshalByRefObject, IDisposable
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000FC7 RID: 4039 RVA: 0x0004650A File Offset: 0x0004550A
		// (remove) Token: 0x06000FC8 RID: 4040 RVA: 0x00046523 File Offset: 0x00045523
		public event BuildManagerHostUnloadEventHandler AppDomainUnloaded;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000FC9 RID: 4041 RVA: 0x0004653C File Offset: 0x0004553C
		// (remove) Token: 0x06000FCA RID: 4042 RVA: 0x00046555 File Offset: 0x00045555
		public event EventHandler AppDomainStarted;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000FCB RID: 4043 RVA: 0x0004656E File Offset: 0x0004556E
		// (remove) Token: 0x06000FCC RID: 4044 RVA: 0x00046587 File Offset: 0x00045587
		public event BuildManagerHostUnloadEventHandler AppDomainShutdown;

		// Token: 0x06000FCD RID: 4045 RVA: 0x000465A0 File Offset: 0x000455A0
		public ClientBuildManager(string appVirtualDir, string appPhysicalSourceDir)
			: this(appVirtualDir, appPhysicalSourceDir, null, null)
		{
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000465AC File Offset: 0x000455AC
		public ClientBuildManager(string appVirtualDir, string appPhysicalSourceDir, string appPhysicalTargetDir)
			: this(appVirtualDir, appPhysicalSourceDir, appPhysicalTargetDir, null)
		{
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x000465B8 File Offset: 0x000455B8
		public ClientBuildManager(string appVirtualDir, string appPhysicalSourceDir, string appPhysicalTargetDir, ClientBuildManagerParameter parameter)
		{
			if (parameter == null)
			{
				parameter = new ClientBuildManagerParameter();
			}
			if (!string.IsNullOrEmpty(appPhysicalTargetDir))
			{
				parameter.PrecompilationFlags |= PrecompilationFlags.Clean;
			}
			this._hostingParameters = new HostingEnvironmentParameters();
			this._hostingParameters.HostingFlags = HostingEnvironmentFlags.DontCallAppInitialize | HostingEnvironmentFlags.ClientBuildManager;
			this._hostingParameters.ClientBuildManagerParameter = parameter;
			this._hostingParameters.PrecompilationTargetPhysicalDirectory = appPhysicalTargetDir;
			if (appVirtualDir[0] != '/')
			{
				appVirtualDir = "/" + appVirtualDir;
			}
			this.Initialize(VirtualPath.CreateNonRelative(appVirtualDir), appPhysicalSourceDir);
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0004664E File Offset: 0x0004564E
		public string CodeGenDir
		{
			get
			{
				if (this._codeGenDir == null)
				{
					this.EnsureHostCreated();
					this._codeGenDir = this._host.CodeGenDir;
				}
				return this._codeGenDir;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00046675 File Offset: 0x00045675
		public bool IsHostCreated
		{
			get
			{
				return this._host != null;
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00046684 File Offset: 0x00045684
		public IRegisteredObject CreateObject(Type type, bool failIfExists)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.EnsureHostCreated();
			this._host.RegisterAssembly(type.Assembly.FullName, type.Assembly.Location);
			ApplicationManager applicationManager = ApplicationManager.GetApplicationManager();
			return applicationManager.CreateObjectInternal(this._appId, type, this._host.ApplicationHost, failIfExists);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000466E5 File Offset: 0x000456E5
		public string[] GetAppDomainShutdownDirectories()
		{
			return FileChangesMonitor.s_dirsToMonitor;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x000466EC File Offset: 0x000456EC
		public void CompileApplicationDependencies()
		{
			this.EnsureHostCreated();
			this._host.CompileApplicationDependencies();
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000466FF File Offset: 0x000456FF
		public IDictionary GetBrowserDefinitions()
		{
			this.EnsureHostCreated();
			return this._host.GetBrowserDefinitions();
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00046712 File Offset: 0x00045712
		public string GetGeneratedSourceFile(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			return this._host.GetGeneratedSourceFile(VirtualPath.CreateTrailingSlash(virtualPath));
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00046739 File Offset: 0x00045739
		public string GetGeneratedFileVirtualPath(string filePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			this.EnsureHostCreated();
			return this._host.GetGeneratedFileVirtualPath(filePath);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0004675B File Offset: 0x0004575B
		public string[] GetVirtualCodeDirectories()
		{
			this.EnsureHostCreated();
			return this._host.GetVirtualCodeDirectories();
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0004676E File Offset: 0x0004576E
		public string[] GetTopLevelAssemblyReferences(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			return this._host.GetTopLevelAssemblyReferences(VirtualPath.Create(virtualPath));
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00046795 File Offset: 0x00045795
		public void GetCodeDirectoryInformation(string virtualCodeDir, out Type codeDomProviderType, out CompilerParameters compilerParameters, out string generatedFilesDir)
		{
			if (virtualCodeDir == null)
			{
				throw new ArgumentNullException("virtualCodeDir");
			}
			this.EnsureHostCreated();
			this._host.GetCodeDirectoryInformation(VirtualPath.CreateTrailingSlash(virtualCodeDir), out codeDomProviderType, out compilerParameters, out generatedFilesDir);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x000467C0 File Offset: 0x000457C0
		public void GetCompilerParameters(string virtualPath, out Type codeDomProviderType, out CompilerParameters compilerParameters)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			this._host.GetCompilerParams(VirtualPath.Create(virtualPath), out codeDomProviderType, out compilerParameters);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x000467E9 File Offset: 0x000457E9
		public CodeCompileUnit GenerateCodeCompileUnit(string virtualPath, out Type codeDomProviderType, out CompilerParameters compilerParameters, out IDictionary linePragmasTable)
		{
			return this.GenerateCodeCompileUnit(virtualPath, null, out codeDomProviderType, out compilerParameters, out linePragmasTable);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x000467F7 File Offset: 0x000457F7
		public CodeCompileUnit GenerateCodeCompileUnit(string virtualPath, string virtualFileString, out Type codeDomProviderType, out CompilerParameters compilerParameters, out IDictionary linePragmasTable)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			return this._host.GenerateCodeCompileUnit(VirtualPath.Create(virtualPath), virtualFileString, out codeDomProviderType, out compilerParameters, out linePragmasTable);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00046824 File Offset: 0x00045824
		public string GenerateCode(string virtualPath, string virtualFileString, out IDictionary linePragmasTable)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			return this._host.GenerateCode(VirtualPath.Create(virtualPath), virtualFileString, out linePragmasTable);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00046850 File Offset: 0x00045850
		public Type GetCompiledType(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			this.EnsureHostCreated();
			string[] compiledTypeAndAssemblyName = this._host.GetCompiledTypeAndAssemblyName(VirtualPath.Create(virtualPath), null);
			if (compiledTypeAndAssemblyName == null)
			{
				return null;
			}
			Assembly assembly = Assembly.LoadFrom(compiledTypeAndAssemblyName[1]);
			return assembly.GetType(compiledTypeAndAssemblyName[0]);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0004689D File Offset: 0x0004589D
		public void CompileFile(string virtualPath)
		{
			this.CompileFile(virtualPath, null);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x000468A8 File Offset: 0x000458A8
		public void CompileFile(string virtualPath, ClientBuildManagerCallback callback)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			try
			{
				this.EnsureHostCreated();
				this._host.GetCompiledTypeAndAssemblyName(VirtualPath.Create(virtualPath), callback);
			}
			finally
			{
				if (callback != null)
				{
					RemotingServices.Disconnect(callback);
				}
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x000468FC File Offset: 0x000458FC
		public bool IsCodeAssembly(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			this.EnsureHostCreated();
			return this._host.IsCodeAssembly(assemblyName);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0004692C File Offset: 0x0004592C
		public bool Unload()
		{
			BuildManagerHost host = this._host;
			if (host != null)
			{
				this._host = null;
				return host.UnloadAppDomain();
			}
			return false;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00046952 File Offset: 0x00045952
		public void PrecompileApplication()
		{
			this.PrecompileApplication(null);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0004695B File Offset: 0x0004595B
		public void PrecompileApplication(ClientBuildManagerCallback callback)
		{
			this.PrecompileApplication(callback, false);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00046968 File Offset: 0x00045968
		public void PrecompileApplication(ClientBuildManagerCallback callback, bool forceCleanBuild)
		{
			PrecompilationFlags precompilationFlags = this._hostingParameters.ClientBuildManagerParameter.PrecompilationFlags;
			if (forceCleanBuild)
			{
				this._waitForCallBack = this._host != null;
				this.Unload();
				this._hostingParameters.ClientBuildManagerParameter.PrecompilationFlags = precompilationFlags | PrecompilationFlags.Clean;
				this.WaitForCallBack();
			}
			try
			{
				this.EnsureHostCreated();
				this._host.PrecompileApp(callback);
			}
			finally
			{
				if (forceCleanBuild)
				{
					this._hostingParameters.ClientBuildManagerParameter.PrecompilationFlags = precompilationFlags;
				}
				if (callback != null)
				{
					RemotingServices.Disconnect(callback);
				}
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00046A00 File Offset: 0x00045A00
		private void WaitForCallBack()
		{
			int num = 0;
			while (this._waitForCallBack && num <= 50)
			{
				Thread.Sleep(200);
				num++;
			}
			if (this._waitForCallBack)
			{
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00046A34 File Offset: 0x00045A34
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00046A38 File Offset: 0x00045A38
		internal void Initialize(VirtualPath virtualPath, string physicalPath)
		{
			this._virtualPath = virtualPath;
			this._physicalPath = FileUtil.FixUpPhysicalDirectory(physicalPath);
			this._onAppDomainUnloadedCallback = new WaitCallback(this.OnAppDomainUnloadedCallback);
			this._onAppDomainShutdown = new WaitCallback(this.OnAppDomainShutdownCallback);
			this._installPath = RuntimeEnvironment.GetRuntimeDirectory();
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00046A88 File Offset: 0x00045A88
		private void EnsureHostCreated()
		{
			if (this._host == null)
			{
				lock (this._lock)
				{
					if (this._host == null)
					{
						this.CreateHost();
					}
				}
			}
			if (this._hostCreationException != null)
			{
				throw new HttpException(this._hostCreationException.Message, this._hostCreationException);
			}
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00046AF0 File Offset: 0x00045AF0
		private void CreateHost()
		{
			this._hostCreationPending = true;
			BuildManagerHost buildManagerHost = null;
			try
			{
				ApplicationManager applicationManager = ApplicationManager.GetApplicationManager();
				string text;
				buildManagerHost = (BuildManagerHost)applicationManager.CreateObjectWithDefaultAppHostAndAppId(this._physicalPath, this._virtualPath, typeof(BuildManagerHost), false, this._hostingParameters, out text);
				buildManagerHost.AddPendingCall();
				buildManagerHost.Configure(this);
				this._host = buildManagerHost;
				this._appId = text;
				this._hostCreationException = this._host.InitializationException;
			}
			catch (Exception ex)
			{
				this._hostCreationException = ex;
				this._host = buildManagerHost;
			}
			finally
			{
				this._hostCreationPending = false;
				if (buildManagerHost != null)
				{
					if (this.AppDomainStarted != null)
					{
						this.AppDomainStarted(this, EventArgs.Empty);
					}
					buildManagerHost.RemovePendingCall();
				}
			}
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00046BC0 File Offset: 0x00045BC0
		internal void OnAppDomainUnloaded(ApplicationShutdownReason reason)
		{
			this._host = null;
			this._hostCreationException = null;
			this._reason = reason;
			this._waitForCallBack = false;
			ThreadPool.QueueUserWorkItem(this._onAppDomainUnloadedCallback);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00046BEA File Offset: 0x00045BEA
		private void OnAppDomainUnloadedCallback(object unused)
		{
			if (this.AppDomainUnloaded != null)
			{
				this.AppDomainUnloaded(this, new BuildManagerHostUnloadEventArgs(this._reason));
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00046C0B File Offset: 0x00045C0B
		private void OnAppDomainShutdownCallback(object o)
		{
			if (this.AppDomainShutdown != null)
			{
				this.AppDomainShutdown(this, new BuildManagerHostUnloadEventArgs((ApplicationShutdownReason)o));
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00046C2C File Offset: 0x00045C2C
		internal void OnAppDomainShutdown(ApplicationShutdownReason reason)
		{
			ThreadPool.QueueUserWorkItem(this._onAppDomainShutdown, reason);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00046C40 File Offset: 0x00045C40
		void IDisposable.Dispose()
		{
			this.Unload();
		}

		// Token: 0x04001614 RID: 5652
		private VirtualPath _virtualPath;

		// Token: 0x04001615 RID: 5653
		private string _physicalPath;

		// Token: 0x04001616 RID: 5654
		private string _installPath;

		// Token: 0x04001617 RID: 5655
		private string _appId;

		// Token: 0x04001618 RID: 5656
		private string _codeGenDir;

		// Token: 0x04001619 RID: 5657
		private HostingEnvironmentParameters _hostingParameters;

		// Token: 0x0400161A RID: 5658
		private WaitCallback _onAppDomainUnloadedCallback;

		// Token: 0x0400161B RID: 5659
		private WaitCallback _onAppDomainShutdown;

		// Token: 0x0400161C RID: 5660
		private ApplicationShutdownReason _reason;

		// Token: 0x0400161D RID: 5661
		private BuildManagerHost _host;

		// Token: 0x0400161E RID: 5662
		private Exception _hostCreationException;

		// Token: 0x0400161F RID: 5663
		private bool _hostCreationPending;

		// Token: 0x04001623 RID: 5667
		private object _lock = new object();

		// Token: 0x04001624 RID: 5668
		private bool _waitForCallBack;
	}
}
