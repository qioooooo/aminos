using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace Microsoft.Vsa
{
	// Token: 0x02000003 RID: 3
	[Guid("37e0fbbb-865f-339d-acf9-fd66f9a2867e")]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public sealed class VsaLoader : IVsaEngine
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000020D0 File Offset: 0x000010D0
		public VsaLoader()
		{
			this.m_IsRunning = false;
			this.m_clientDebug = false;
			this.m_RootNamespace = "";
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000020F1 File Offset: 0x000010F1
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000020F9 File Offset: 0x000010F9
		public IVsaSite Site
		{
			get
			{
				return this.m_RTSite;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_RTSite = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002110 File Offset: 0x00001110
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002118 File Offset: 0x00001118
		public string Name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002121 File Offset: 0x00001121
		public bool Compile()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002128 File Offset: 0x00001128
		private bool IsRootNamespaceSet()
		{
			return this.m_RootNamespace != null && this.m_RootNamespace != string.Empty;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002144 File Offset: 0x00001144
		public void Run()
		{
			if (this.m_IsRunning)
			{
				throw new VsaException(VsaError.EngineRunning);
			}
			if (this.m_RootMoniker == null)
			{
				throw new VsaException(VsaError.RootMonikerInvalid);
			}
			if (!this.IsRootNamespaceSet())
			{
				throw new VsaException(VsaError.RootNamespaceNotSet);
			}
			Mutex mutex = null;
			try
			{
				if (this.m_Domain == null)
				{
					this.m_Domain = Thread.GetDomain();
					this.m_Assembly = null;
				}
				this.GetDataFromCache();
				if (this.m_Assembly == null)
				{
					string text = this.m_RootMoniker + "/" + Convert.ToString(this.m_Domain.GetHashCode(), CultureInfo.InvariantCulture);
					mutex = new Mutex(false, text);
					if (mutex.WaitOne())
					{
						this.GetDataFromCache();
						if (this.m_Assembly == null)
						{
							byte[] array;
							byte[] array2;
							this.m_RTSite.GetCompiledState(out array, out array2);
							this.m_Assembly = this.m_Domain.Load(array, array2, this.m_Evidence);
							this.PutDataToCache();
						}
					}
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
			this.m_IsRunning = true;
			this.m_StartupClassInstance = null;
			Type type;
			object startupClassInstance = this.GetStartupClassInstance(out type);
			object[] array3 = new object[] { this.m_RTSite };
			type.InvokeMember("SetSite", BindingFlags.InvokeMethod, null, startupClassInstance, array3, CultureInfo.CurrentCulture);
			type.InvokeMember("Startup", BindingFlags.InvokeMethod, null, startupClassInstance, null, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000022AC File Offset: 0x000012AC
		private object GetStartupClassInstance(out Type type)
		{
			type = null;
			if (this.m_Assembly == null)
			{
				return null;
			}
			if (this.m_StartupClassInstance != null)
			{
				type = this.m_StartupClassInstance.GetType();
				return this.m_StartupClassInstance;
			}
			if (!this.IsRootNamespaceSet())
			{
				throw new VsaException(VsaError.RootNamespaceNotSet);
			}
			string text = this.RootNamespace + "." + VsaLoader.startupClassName;
			type = this.m_Assembly.GetType(text, true);
			this.m_StartupClassInstance = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
			if (this.m_StartupClassInstance == null)
			{
				throw new VsaException(VsaError.BadAssembly);
			}
			return this.m_StartupClassInstance;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002348 File Offset: 0x00001348
		private void RunShutdown()
		{
			if (this.m_Assembly != null && this.m_IsRunning)
			{
				Type type;
				object startupClassInstance = this.GetStartupClassInstance(out type);
				type.InvokeMember("Shutdown", BindingFlags.InvokeMethod, null, startupClassInstance, null, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002387 File Offset: 0x00001387
		public void Reset()
		{
			if (this.m_IsRunning)
			{
				this.RunShutdown();
			}
			this.m_IsRunning = false;
			this.m_Assembly = null;
			this.m_StartupClassInstance = null;
			this.m_Evidence = null;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000023B3 File Offset: 0x000013B3
		public void Close()
		{
			this.m_RTSite = null;
			this.Reset();
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000023C2 File Offset: 0x000013C2
		public bool IsRunning
		{
			get
			{
				return this.m_IsRunning;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000023CA File Offset: 0x000013CA
		public bool IsCompiled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000023CD File Offset: 0x000013CD
		public IVsaItems Items
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000023D4 File Offset: 0x000013D4
		public void SaveSourceState(IVsaPersistSite Site)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023DB File Offset: 0x000013DB
		public void LoadSourceState(IVsaPersistSite Site)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000023E2 File Offset: 0x000013E2
		public void SaveCompiledState(out byte[] PE, out byte[] PDB)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000023E9 File Offset: 0x000013E9
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000023F1 File Offset: 0x000013F1
		public string RootMoniker
		{
			get
			{
				return this.m_RootMoniker;
			}
			set
			{
				this.m_RootMoniker = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000023FA File Offset: 0x000013FA
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002402 File Offset: 0x00001402
		public string RootNamespace
		{
			get
			{
				return this.m_RootNamespace;
			}
			set
			{
				if (this.m_IsRunning)
				{
					throw new VsaException(VsaError.EngineRunning);
				}
				if (value == null || value == string.Empty)
				{
					throw new VsaException(VsaError.RootNamespaceInvalid);
				}
				this.m_RootNamespace = value;
				this.m_StartupClassInstance = null;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002440 File Offset: 0x00001440
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002447 File Offset: 0x00001447
		public bool GenerateDebugInfo
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002450 File Offset: 0x00001450
		public void RevokeCache()
		{
			if (this.m_Domain == null)
			{
				this.m_Domain = Thread.GetDomain();
			}
			if (this.m_RootMoniker == null)
			{
				throw new VsaException(VsaError.RootMonikerInvalid);
			}
			Mutex mutex = null;
			try
			{
				string text = this.m_RootMoniker + "/" + Convert.ToString(this.m_Domain.GetHashCode(), CultureInfo.InvariantCulture);
				mutex = new Mutex(false, text);
				if (mutex.WaitOne())
				{
					this.m_Domain.SetData(this.m_RootMoniker, null);
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000024F0 File Offset: 0x000014F0
		public object GetOption(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.IsRunning)
			{
				throw new VsaException(VsaError.EngineRunning);
			}
			if (name != null && name == "ClientDebug")
			{
				return this.m_clientDebug;
			}
			throw new VsaException(VsaError.OptionNotSupported);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002548 File Offset: 0x00001548
		public void SetOption(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.IsRunning)
			{
				throw new VsaException(VsaError.EngineRunning);
			}
			try
			{
				if (name == null || !(name == "ClientDebug"))
				{
					throw new VsaException(VsaError.OptionNotSupported);
				}
				if (value == null)
				{
					throw new VsaException(VsaError.OptionInvalid);
				}
				this.m_clientDebug = (bool)value;
			}
			catch (InvalidCastException)
			{
				throw new VsaException(VsaError.OptionInvalid);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000025D0 File Offset: 0x000015D0
		public void InitNew()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000025D2 File Offset: 0x000015D2
		public bool IsValidIdentifier(string identifier)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000025D9 File Offset: 0x000015D9
		public bool IsDirty
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000025E0 File Offset: 0x000015E0
		public string Language
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000025E7 File Offset: 0x000015E7
		public string Version
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000025EE File Offset: 0x000015EE
		public Assembly Assembly
		{
			get
			{
				return this.m_Assembly;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000025F6 File Offset: 0x000015F6
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000025FD File Offset: 0x000015FD
		public int LCID
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002604 File Offset: 0x00001604
		private void PutDataToCache()
		{
			this.m_Domain.SetData(this.m_RootMoniker, this.m_Assembly);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000261D File Offset: 0x0000161D
		private void GetDataFromCache()
		{
			this.m_Assembly = (Assembly)this.m_Domain.GetData(this.RootMoniker);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000263B File Offset: 0x0000163B
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002643 File Offset: 0x00001643
		public Evidence Evidence
		{
			get
			{
				return this.m_Evidence;
			}
			set
			{
				this.m_Evidence = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private const string clientDebugPropertyName = "ClientDebug";

		// Token: 0x04000002 RID: 2
		private IVsaSite m_RTSite;

		// Token: 0x04000003 RID: 3
		private string m_Name;

		// Token: 0x04000004 RID: 4
		private string m_RootNamespace;

		// Token: 0x04000005 RID: 5
		private bool m_IsRunning;

		// Token: 0x04000006 RID: 6
		private Assembly m_Assembly;

		// Token: 0x04000007 RID: 7
		private _AppDomain m_Domain;

		// Token: 0x04000008 RID: 8
		private string m_RootMoniker;

		// Token: 0x04000009 RID: 9
		private object m_StartupClassInstance;

		// Token: 0x0400000A RID: 10
		private bool m_clientDebug;

		// Token: 0x0400000B RID: 11
		private Evidence m_Evidence;

		// Token: 0x0400000C RID: 12
		private static readonly string startupClassName = "_Startup";
	}
}
