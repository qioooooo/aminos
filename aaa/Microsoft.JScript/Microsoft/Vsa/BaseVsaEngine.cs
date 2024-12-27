using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;
using Microsoft.Win32;

namespace Microsoft.Vsa
{
	// Token: 0x02000024 RID: 36
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[ComVisible(true)]
	[Guid("F8932A50-9127-48B6-B115-2BFDC627CEE3")]
	public abstract class BaseVsaEngine : IVsaEngine
	{
		// Token: 0x06000133 RID: 307 RVA: 0x0000709C File Offset: 0x0000609C
		internal BaseVsaEngine(string language, string version, bool supportDebug)
		{
			this.applicationPath = "";
			this.compiledRootNamespace = null;
			this.genDebugInfo = false;
			this.haveCompiledState = false;
			this.failedCompilation = false;
			this.isClosed = false;
			this.isEngineCompiled = false;
			this.isEngineDirty = false;
			this.isEngineInitialized = false;
			this.isEngineRunning = false;
			this.vsaItems = null;
			this.engineSite = null;
			this.errorLocale = CultureInfo.CurrentUICulture.LCID;
			this.engineName = "";
			this.rootNamespace = "";
			this.engineMoniker = "";
			this.scriptLanguage = language;
			this.assemblyVersion = version;
			this.isDebugInfoSupported = supportDebug;
			this.executionEvidence = null;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007154 File Offset: 0x00006154
		protected VsaException Error(VsaError vsaErrorNumber)
		{
			return new VsaException(vsaErrorNumber);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000715C File Offset: 0x0000615C
		internal void TryObtainLock()
		{
			if (!Monitor.TryEnter(this))
			{
				throw new VsaException(VsaError.EngineBusy);
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007171 File Offset: 0x00006171
		internal void ReleaseLock()
		{
			Monitor.Exit(this);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007179 File Offset: 0x00006179
		private bool IsCondition(BaseVsaEngine.Pre flag, BaseVsaEngine.Pre test)
		{
			return (flag & test) != BaseVsaEngine.Pre.None;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007184 File Offset: 0x00006184
		protected void Preconditions(BaseVsaEngine.Pre flags)
		{
			if (this.isClosed)
			{
				throw this.Error(VsaError.EngineClosed);
			}
			if (flags == BaseVsaEngine.Pre.EngineNotClosed)
			{
				return;
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.SupportForDebug) && !this.isDebugInfoSupported)
			{
				throw this.Error(VsaError.DebugInfoNotSupported);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineCompiled) && !this.haveCompiledState)
			{
				throw this.Error(VsaError.EngineNotCompiled);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineRunning) && !this.isEngineRunning)
			{
				throw this.Error(VsaError.EngineNotRunning);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineNotRunning) && this.isEngineRunning)
			{
				throw this.Error(VsaError.EngineRunning);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.RootMonikerSet) && this.engineMoniker == "")
			{
				throw this.Error(VsaError.RootMonikerNotSet);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.RootMonikerNotSet) && this.engineMoniker != "")
			{
				throw this.Error(VsaError.RootMonikerAlreadySet);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.RootNamespaceSet) && this.rootNamespace == "")
			{
				throw this.Error(VsaError.RootNamespaceNotSet);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.SiteSet) && this.engineSite == null)
			{
				throw this.Error(VsaError.SiteNotSet);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.SiteNotSet) && this.engineSite != null)
			{
				throw this.Error(VsaError.SiteAlreadySet);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineInitialised) && !this.isEngineInitialized)
			{
				throw this.Error(VsaError.EngineNotInitialized);
			}
			if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineNotInitialised) && this.isEngineInitialized)
			{
				throw this.Error(VsaError.EngineInitialized);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00007329 File Offset: 0x00006329
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00007337 File Offset: 0x00006337
		public _AppDomain AppDomain
		{
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				throw new NotSupportedException();
			}
			set
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				throw new VsaException(VsaError.AppDomainCannotBeSet);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000734A File Offset: 0x0000634A
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000735D File Offset: 0x0000635D
		public Evidence Evidence
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.executionEvidence;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			set
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
				this.executionEvidence = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00007371 File Offset: 0x00006371
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000737F File Offset: 0x0000637F
		public string ApplicationBase
		{
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				throw new NotSupportedException();
			}
			set
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				throw new VsaException(VsaError.ApplicationBaseCannotBeSet);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00007392 File Offset: 0x00006392
		public Assembly Assembly
		{
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineRunning);
				return this.loadedAssembly;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000073A2 File Offset: 0x000063A2
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000073B8 File Offset: 0x000063B8
		public bool GenerateDebugInfo
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.genDebugInfo;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.SupportForDebug | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
					if (this.genDebugInfo != value)
					{
						this.genDebugInfo = value;
						this.isEngineDirty = true;
						this.isEngineCompiled = false;
					}
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00007410 File Offset: 0x00006410
		public bool IsCompiled
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.isEngineCompiled;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007423 File Offset: 0x00006423
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00007438 File Offset: 0x00006438
		public bool IsDirty
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.isEngineDirty;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
					this.isEngineDirty = value;
					if (this.isEngineDirty)
					{
						this.isEngineCompiled = false;
					}
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00007484 File Offset: 0x00006484
		public bool IsRunning
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.isEngineRunning;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007497 File Offset: 0x00006497
		public IVsaItems Items
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.vsaItems;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000074AA File Offset: 0x000064AA
		public string Language
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.scriptLanguage;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000074BD File Offset: 0x000064BD
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000074D0 File Offset: 0x000064D0
		public int LCID
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.errorLocale;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
					try
					{
						new CultureInfo(value);
					}
					catch (ArgumentException)
					{
						throw this.Error(VsaError.LCIDNotSupported);
					}
					this.errorLocale = value;
					this.isEngineDirty = true;
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007538 File Offset: 0x00006538
		// (set) Token: 0x0600014B RID: 331 RVA: 0x0000754C File Offset: 0x0000654C
		public string Name
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.engineName;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
					if (!(this.engineName == value))
					{
						lock (BaseVsaEngine.nameTable)
						{
							if (BaseVsaEngine.nameTable[value] != null)
							{
								throw this.Error(VsaError.EngineNameInUse);
							}
							BaseVsaEngine.nameTable[value] = new object();
							if (this.engineName != null && this.engineName.Length > 0)
							{
								BaseVsaEngine.nameTable[this.engineName] = null;
							}
						}
						this.engineName = value;
						this.isEngineDirty = true;
						this.isEngineCompiled = false;
					}
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000761C File Offset: 0x0000661C
		// (set) Token: 0x0600014D RID: 333 RVA: 0x0000762C File Offset: 0x0000662C
		public string RootMoniker
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				return this.engineMoniker;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerNotSet);
					this.ValidateRootMoniker(value);
					this.engineMoniker = value;
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007670 File Offset: 0x00006670
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00007684 File Offset: 0x00006684
		public string RootNamespace
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.rootNamespace;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
					if (!this.IsValidNamespaceName(value))
					{
						throw this.Error(VsaError.RootNamespaceInvalid);
					}
					this.rootNamespace = value;
					this.isEngineDirty = true;
					this.isEngineCompiled = false;
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000076E8 File Offset: 0x000066E8
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000076F8 File Offset: 0x000066F8
		public IVsaSite Site
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet);
				return this.engineSite;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.TryObtainLock();
				try
				{
					this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.SiteNotSet);
					if (value == null)
					{
						throw this.Error(VsaError.SiteInvalid);
					}
					this.engineSite = value;
				}
				finally
				{
					this.ReleaseLock();
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007748 File Offset: 0x00006748
		public string Version
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				return this.assemblyVersion;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000775C File Offset: 0x0000675C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void Close()
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
				if (this.isEngineRunning)
				{
					this.Reset();
				}
				lock (BaseVsaEngine.nameTable)
				{
					if (this.engineName != null && this.engineName.Length > 0)
					{
						BaseVsaEngine.nameTable[this.engineName] = null;
					}
				}
				this.DoClose();
				this.isClosed = true;
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000077F4 File Offset: 0x000067F4
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public virtual bool Compile()
		{
			this.TryObtainLock();
			bool flag2;
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.RootNamespaceSet | BaseVsaEngine.Pre.EngineInitialised);
				bool flag = false;
				int num = 0;
				int count = this.vsaItems.Count;
				while (!flag && num < count)
				{
					IVsaItem vsaItem = this.vsaItems[num];
					flag = this.vsaItems[num].ItemType == VsaItemType.Code;
					num++;
				}
				if (!flag)
				{
					throw this.Error(VsaError.EngineClosed);
				}
				try
				{
					this.ResetCompiledState();
					this.isEngineCompiled = this.DoCompile();
				}
				catch (VsaException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.InternalCompilerError, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.InternalCompilerError);
				}
				if (this.isEngineCompiled)
				{
					this.haveCompiledState = true;
					this.failedCompilation = false;
					this.compiledRootNamespace = this.rootNamespace;
				}
				flag2 = this.isEngineCompiled;
			}
			finally
			{
				this.ReleaseLock();
			}
			return flag2;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007900 File Offset: 0x00006900
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual object GetOption(string name)
		{
			this.TryObtainLock();
			object obj;
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
				object customOption = this.GetCustomOption(name);
				obj = customOption;
			}
			finally
			{
				this.ReleaseLock();
			}
			return obj;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007944 File Offset: 0x00006944
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void InitNew()
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.SiteSet | BaseVsaEngine.Pre.EngineNotInitialised);
				this.isEngineInitialized = true;
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007984 File Offset: 0x00006984
		protected virtual Assembly LoadCompiledState()
		{
			byte[] array;
			byte[] array2;
			this.DoSaveCompiledState(out array, out array2);
			return Assembly.Load(array, array2, this.executionEvidence);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000079A8 File Offset: 0x000069A8
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void LoadSourceState(IVsaPersistSite site)
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.SiteSet | BaseVsaEngine.Pre.EngineNotInitialised);
				this.isEngineInitialized = true;
				try
				{
					this.DoLoadSourceState(site);
				}
				catch
				{
					this.isEngineInitialized = false;
					throw;
				}
				this.isEngineDirty = false;
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007A0C File Offset: 0x00006A0C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void Reset()
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineRunning);
				try
				{
					this.startupInstance.Shutdown();
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.EngineCannotReset, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.EngineCannotReset);
				}
				this.isEngineRunning = false;
				this.loadedAssembly = null;
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007A94 File Offset: 0x00006A94
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void RevokeCache()
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.RootMonikerSet);
				try
				{
					global::System.AppDomain.CurrentDomain.SetData(this.engineMoniker, null);
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.RevokeFailed, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.RevokeFailed);
				}
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007B14 File Offset: 0x00006B14
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void Run()
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.RootNamespaceSet | BaseVsaEngine.Pre.SiteSet);
				AppDomain currentDomain = global::System.AppDomain.CurrentDomain;
				if (this.haveCompiledState)
				{
					if (this.rootNamespace != this.compiledRootNamespace)
					{
						throw new VsaException(VsaError.RootNamespaceInvalid);
					}
					this.loadedAssembly = this.LoadCompiledState();
					currentDomain.SetData(this.engineMoniker, this.loadedAssembly);
				}
				else
				{
					if (this.failedCompilation)
					{
						throw new VsaException(VsaError.EngineNotCompiled);
					}
					this.startupClass = null;
					this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as Assembly;
					if (this.loadedAssembly == null)
					{
						string text = this.engineMoniker + "/" + currentDomain.GetHashCode().ToString(CultureInfo.InvariantCulture);
						Mutex mutex = new Mutex(false, text);
						if (mutex.WaitOne())
						{
							try
							{
								this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as Assembly;
								if (this.loadedAssembly == null)
								{
									byte[] array;
									byte[] array2;
									this.engineSite.GetCompiledState(out array, out array2);
									if (array == null)
									{
										throw new VsaException(VsaError.GetCompiledStateFailed);
									}
									this.loadedAssembly = Assembly.Load(array, array2, this.executionEvidence);
									currentDomain.SetData(this.engineMoniker, this.loadedAssembly);
								}
							}
							finally
							{
								mutex.ReleaseMutex();
								mutex.Close();
							}
						}
					}
				}
				try
				{
					if (this.startupClass == null)
					{
						this.startupClass = this.loadedAssembly.GetType(this.rootNamespace + "._Startup", true);
					}
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.BadAssembly, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.BadAssembly);
				}
				try
				{
					this.startupInstance = (BaseVsaStartup)Activator.CreateInstance(this.startupClass);
					this.isEngineRunning = true;
					this.startupInstance.SetSite(this.engineSite);
					this.startupInstance.Startup();
				}
				catch (Exception ex2)
				{
					throw new VsaException(VsaError.UnknownError, ex2.ToString(), ex2);
				}
				catch
				{
					throw new VsaException(VsaError.UnknownError);
				}
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007DAC File Offset: 0x00006DAC
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void SetOption(string name, object value)
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
				this.SetCustomOption(name, value);
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007DEC File Offset: 0x00006DEC
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void SaveCompiledState(out byte[] pe, out byte[] debugInfo)
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineCompiled | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
				this.DoSaveCompiledState(out pe, out debugInfo);
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007E2C File Offset: 0x00006E2C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void SaveSourceState(IVsaPersistSite site)
		{
			this.TryObtainLock();
			try
			{
				this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
				if (site == null)
				{
					throw this.Error(VsaError.SiteInvalid);
				}
				try
				{
					this.DoSaveSourceState(site);
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.SaveElementFailed, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.SaveElementFailed);
				}
			}
			finally
			{
				this.ReleaseLock();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007EB4 File Offset: 0x00006EB4
		protected virtual void ValidateRootMoniker(string rootMoniker)
		{
			if (rootMoniker == null)
			{
				throw new VsaException(VsaError.RootMonikerInvalid);
			}
			Uri uri = null;
			try
			{
				uri = new Uri(rootMoniker);
			}
			catch (UriFormatException)
			{
				throw new VsaException(VsaError.RootMonikerInvalid);
			}
			string scheme = uri.Scheme;
			if (scheme.Length == 0)
			{
				throw new VsaException(VsaError.RootMonikerProtocolInvalid);
			}
			string[] array = new string[]
			{
				"file", "ftp", "gopher", "http", "https", "javascript", "mailto", "microsoft", "news", "res",
				"smtp", "socks", "vbscript", "xlang", "xml", "xpath", "xsd", "xsl"
			};
			try
			{
				RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\PROTOCOLS\\Handler");
				registryPermission.Assert();
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\PROTOCOLS\\Handler");
				array = registryKey.GetSubKeyNames();
			}
			catch
			{
			}
			foreach (string text in array)
			{
				if (string.Compare(text, scheme, StringComparison.OrdinalIgnoreCase) == 0)
				{
					throw new VsaException(VsaError.RootMonikerProtocolInvalid);
				}
			}
		}

		// Token: 0x06000160 RID: 352
		protected abstract void DoClose();

		// Token: 0x06000161 RID: 353
		protected abstract bool DoCompile();

		// Token: 0x06000162 RID: 354
		protected abstract void DoLoadSourceState(IVsaPersistSite site);

		// Token: 0x06000163 RID: 355
		protected abstract void DoSaveCompiledState(out byte[] pe, out byte[] debugInfo);

		// Token: 0x06000164 RID: 356
		protected abstract void DoSaveSourceState(IVsaPersistSite site);

		// Token: 0x06000165 RID: 357
		protected abstract object GetCustomOption(string name);

		// Token: 0x06000166 RID: 358
		protected abstract bool IsValidNamespaceName(string name);

		// Token: 0x06000167 RID: 359
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public abstract bool IsValidIdentifier(string ident);

		// Token: 0x06000168 RID: 360
		protected abstract void ResetCompiledState();

		// Token: 0x06000169 RID: 361
		protected abstract void SetCustomOption(string name, object value);

		// Token: 0x04000052 RID: 82
		protected string applicationPath;

		// Token: 0x04000053 RID: 83
		protected Assembly loadedAssembly;

		// Token: 0x04000054 RID: 84
		protected string compiledRootNamespace;

		// Token: 0x04000055 RID: 85
		protected IVsaSite engineSite;

		// Token: 0x04000056 RID: 86
		protected bool genDebugInfo;

		// Token: 0x04000057 RID: 87
		protected bool haveCompiledState;

		// Token: 0x04000058 RID: 88
		protected bool failedCompilation;

		// Token: 0x04000059 RID: 89
		protected bool isClosed;

		// Token: 0x0400005A RID: 90
		protected bool isEngineCompiled;

		// Token: 0x0400005B RID: 91
		protected bool isDebugInfoSupported;

		// Token: 0x0400005C RID: 92
		protected bool isEngineDirty;

		// Token: 0x0400005D RID: 93
		protected bool isEngineInitialized;

		// Token: 0x0400005E RID: 94
		protected bool isEngineRunning;

		// Token: 0x0400005F RID: 95
		protected IVsaItems vsaItems;

		// Token: 0x04000060 RID: 96
		protected string scriptLanguage;

		// Token: 0x04000061 RID: 97
		protected int errorLocale;

		// Token: 0x04000062 RID: 98
		protected static Hashtable nameTable = new Hashtable(10);

		// Token: 0x04000063 RID: 99
		protected string engineName;

		// Token: 0x04000064 RID: 100
		protected string engineMoniker;

		// Token: 0x04000065 RID: 101
		protected string rootNamespace;

		// Token: 0x04000066 RID: 102
		protected Type startupClass;

		// Token: 0x04000067 RID: 103
		protected BaseVsaStartup startupInstance;

		// Token: 0x04000068 RID: 104
		protected string assemblyVersion;

		// Token: 0x04000069 RID: 105
		protected Evidence executionEvidence;

		// Token: 0x02000025 RID: 37
		[Flags]
		protected enum Pre
		{
			// Token: 0x0400006B RID: 107
			None = 0,
			// Token: 0x0400006C RID: 108
			EngineNotClosed = 1,
			// Token: 0x0400006D RID: 109
			SupportForDebug = 2,
			// Token: 0x0400006E RID: 110
			EngineCompiled = 4,
			// Token: 0x0400006F RID: 111
			EngineRunning = 8,
			// Token: 0x04000070 RID: 112
			EngineNotRunning = 16,
			// Token: 0x04000071 RID: 113
			RootMonikerSet = 32,
			// Token: 0x04000072 RID: 114
			RootMonikerNotSet = 64,
			// Token: 0x04000073 RID: 115
			RootNamespaceSet = 128,
			// Token: 0x04000074 RID: 116
			SiteSet = 256,
			// Token: 0x04000075 RID: 117
			SiteNotSet = 512,
			// Token: 0x04000076 RID: 118
			EngineInitialised = 1024,
			// Token: 0x04000077 RID: 119
			EngineNotInitialised = 2048
		}
	}
}
