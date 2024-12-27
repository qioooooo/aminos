using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x020007EA RID: 2026
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_AssemblyBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class AssemblyBuilder : Assembly, _AssemblyBuilder
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004816 RID: 18454 RVA: 0x000FAAB8 File Offset: 0x000F9AB8
		private PermissionSet GrantedPermissionSet
		{
			get
			{
				AssemblyBuilder assemblyBuilder = (AssemblyBuilder)this.InternalAssembly;
				if (assemblyBuilder.m_grantedPermissionSet == null)
				{
					PermissionSet permissionSet = null;
					this.InternalAssembly.nGetGrantSet(out assemblyBuilder.m_grantedPermissionSet, out permissionSet);
					if (assemblyBuilder.m_grantedPermissionSet == null)
					{
						assemblyBuilder.m_grantedPermissionSet = new PermissionSet(PermissionState.Unrestricted);
					}
				}
				return assemblyBuilder.m_grantedPermissionSet;
			}
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x000FAB08 File Offset: 0x000F9B08
		internal void DemandGrantedPermission()
		{
			this.GrantedPermissionSet.Demand();
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004818 RID: 18456 RVA: 0x000FAB15 File Offset: 0x000F9B15
		private bool IsInternal
		{
			get
			{
				return this.m_internalAssemblyBuilder == null;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004819 RID: 18457 RVA: 0x000FAB20 File Offset: 0x000F9B20
		internal override Assembly InternalAssembly
		{
			get
			{
				if (this.IsInternal)
				{
					return this;
				}
				return this.m_internalAssemblyBuilder;
			}
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x000FAB34 File Offset: 0x000F9B34
		internal override Module[] nGetModules(bool loadIfNotFound, bool getResourceModules)
		{
			Module[] array = this.InternalAssembly._nGetModules(loadIfNotFound, getResourceModules);
			if (!this.IsInternal)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ModuleBuilder.GetModuleBuilder(array[i]);
				}
			}
			return array;
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x000FAB74 File Offset: 0x000F9B74
		internal override Module GetModuleInternal(string name)
		{
			Module module = this.InternalAssembly._GetModule(name);
			if (module == null)
			{
				return null;
			}
			if (!this.IsInternal)
			{
				return ModuleBuilder.GetModuleBuilder(module);
			}
			return module;
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x000FABA3 File Offset: 0x000F9BA3
		internal AssemblyBuilder(AssemblyBuilder internalAssemblyBuilder)
		{
			this.m_internalAssemblyBuilder = internalAssemblyBuilder;
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x000FABB4 File Offset: 0x000F9BB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, false, ref stackCrawlMark);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x000FABDC File Offset: 0x000F9BDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, emitSymbolInfo, ref stackCrawlMark);
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x000FAC04 File Offset: 0x000F9C04
		private ModuleBuilder DefineDynamicModuleInternal(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
				}
			}
			return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x000FAC5C File Offset: 0x000F9C5C
		private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
			}
			base.m_assemblyData.CheckNameConflict(name);
			ModuleBuilder moduleBuilder = (ModuleBuilder)Assembly.nDefineDynamicModule(this, emitSymbolInfo, name, ref stackMark);
			moduleBuilder = new ModuleBuilder(this, moduleBuilder);
			ISymbolWriter symbolWriter = null;
			if (emitSymbolInfo)
			{
				Assembly assembly = this.LoadISymWrapper();
				Type type = assembly.GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
				if (type == null)
				{
					throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[] { "SymWriter" }));
				}
				new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
				try
				{
					new PermissionSet(PermissionState.Unrestricted).Assert();
					symbolWriter = (ISymbolWriter)Activator.CreateInstance(type);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			moduleBuilder.Init(name, null, symbolWriter);
			base.m_assemblyData.AddModule(moduleBuilder);
			return moduleBuilder;
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x000FAD7C File Offset: 0x000F9D7C
		private Assembly LoadISymWrapper()
		{
			if (base.m_assemblyData.m_ISymWrapperAssembly != null)
			{
				return base.m_assemblyData.m_ISymWrapperAssembly;
			}
			Assembly assembly = Assembly.Load("ISymWrapper, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			base.m_assemblyData.m_ISymWrapperAssembly = assembly;
			return assembly;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x000FADBC File Offset: 0x000F9DBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, string fileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, fileName, false, ref stackCrawlMark);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x000FADE4 File Offset: 0x000F9DE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, fileName, emitSymbolInfo, ref stackCrawlMark);
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x000FAE0C File Offset: 0x000F9E0C
		private ModuleBuilder DefineDynamicModuleInternal(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
				}
			}
			return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x000FAE68 File Offset: 0x000F9E68
		internal void CheckContext(params Type[][] typess)
		{
			if (typess == null)
			{
				return;
			}
			foreach (Type[] array in typess)
			{
				if (array != null)
				{
					this.CheckContext(array);
				}
			}
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x000FAE98 File Offset: 0x000F9E98
		internal void CheckContext(params Type[] types)
		{
			if (types == null)
			{
				return;
			}
			foreach (Type type in types)
			{
				if (type == null || type.Module.Assembly == typeof(object).Module.Assembly)
				{
					break;
				}
				if (type.Module.Assembly.ReflectionOnly && !this.ReflectionOnly)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext1"), new object[] { type.AssemblyQualifiedName }));
				}
				if (!type.Module.Assembly.ReflectionOnly && this.ReflectionOnly)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext2"), new object[] { type.AssemblyQualifiedName }));
				}
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x000FAF7C File Offset: 0x000F9F7C
		private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_BadPersistableModuleInTransientAssembly"));
			}
			if (base.m_assemblyData.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			ModuleBuilder moduleBuilder = (ModuleBuilder)Assembly.nDefineDynamicModule(this, emitSymbolInfo, fileName, ref stackMark);
			moduleBuilder = new ModuleBuilder(this, moduleBuilder);
			ISymbolWriter symbolWriter = null;
			if (emitSymbolInfo)
			{
				Assembly assembly = this.LoadISymWrapper();
				Type type = assembly.GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
				if (type == null)
				{
					throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[] { "SymWriter" }));
				}
				new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
				try
				{
					new PermissionSet(PermissionState.Unrestricted).Assert();
					symbolWriter = (ISymbolWriter)Activator.CreateInstance(type);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			moduleBuilder.Init(name, fileName, symbolWriter);
			base.m_assemblyData.AddModule(moduleBuilder);
			return moduleBuilder;
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x000FB134 File Offset: 0x000FA134
		public IResourceWriter DefineResource(string name, string description, string fileName)
		{
			return this.DefineResource(name, description, fileName, ResourceAttributes.Public);
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x000FB140 File Offset: 0x000FA140
		public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineResourceNoLock(name, description, fileName, attribute);
				}
			}
			return this.DefineResourceNoLock(name, description, fileName, attribute);
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x000FB1A8 File Offset: 0x000FA1A8
		private IResourceWriter DefineResourceNoLock(string name, string description, string fileName, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckResNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			string text;
			ResourceWriter resourceWriter;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, fileName);
				resourceWriter = new ResourceWriter(text);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, fileName);
				resourceWriter = new ResourceWriter(text);
			}
			text = Path.GetFullPath(text);
			fileName = Path.GetFileName(text);
			base.m_assemblyData.AddResWriter(new ResWriterData(resourceWriter, null, name, fileName, text, attribute));
			return resourceWriter;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x000FB2A4 File Offset: 0x000FA2A4
		public void AddResourceFile(string name, string fileName)
		{
			this.AddResourceFile(name, fileName, ResourceAttributes.Public);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x000FB2B0 File Offset: 0x000FA2B0
		public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.AddResourceFileNoLock(name, fileName, attribute);
					return;
				}
			}
			this.AddResourceFileNoLock(name, fileName, attribute);
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x000FB310 File Offset: 0x000FA310
		private void AddResourceFileNoLock(string name, string fileName, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), fileName);
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckResNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			string text;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, fileName);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, fileName);
			}
			text = Path.GetFullPath(text);
			fileName = Path.GetFileName(text);
			if (!File.Exists(text))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { fileName }), fileName);
			}
			base.m_assemblyData.AddResWriter(new ResWriterData(null, null, name, fileName, text, attribute));
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x000FB427 File Offset: 0x000FA427
		public override string[] GetManifestResourceNames()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x000FB438 File Offset: 0x000FA438
		public override FileStream GetFile(string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x000FB449 File Offset: 0x000FA449
		public override FileStream[] GetFiles(bool getResourceModules)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x000FB45A File Offset: 0x000FA45A
		public override Stream GetManifestResourceStream(Type type, string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x000FB46B File Offset: 0x000FA46B
		public override Stream GetManifestResourceStream(string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x000FB47C File Offset: 0x000FA47C
		public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x000FB48D File Offset: 0x000FA48D
		public override string Location
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004835 RID: 18485 RVA: 0x000FB49E File Offset: 0x000FA49E
		public override string ImageRuntimeVersion
		{
			get
			{
				return RuntimeEnvironment.GetSystemVersion();
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004836 RID: 18486 RVA: 0x000FB4A5 File Offset: 0x000FA4A5
		public override string CodeBase
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004837 RID: 18487 RVA: 0x000FB4B6 File Offset: 0x000FA4B6
		public override MethodInfo EntryPoint
		{
			get
			{
				if (this.IsInternal)
				{
					this.DemandGrantedPermission();
				}
				return base.m_assemblyData.m_entryPointMethod;
			}
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x000FB4D1 File Offset: 0x000FA4D1
		public override Type[] GetExportedTypes()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x000FB4E4 File Offset: 0x000FA4E4
		public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
					return;
				}
			}
			this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x000FB54C File Offset: 0x000FA54C
		private void DefineVersionInfoResourceNoLock(string product, string productVersion, string company, string copyright, string trademark)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
			base.m_assemblyData.m_nativeVersion.m_strCopyright = copyright;
			base.m_assemblyData.m_nativeVersion.m_strTrademark = trademark;
			base.m_assemblyData.m_nativeVersion.m_strCompany = company;
			base.m_assemblyData.m_nativeVersion.m_strProduct = product;
			base.m_assemblyData.m_nativeVersion.m_strProductVersion = productVersion;
			base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
			base.m_assemblyData.m_OverrideUnmanagedVersionInfo = true;
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x000FB610 File Offset: 0x000FA610
		public void DefineVersionInfoResource()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineVersionInfoResourceNoLock();
					return;
				}
			}
			this.DefineVersionInfoResourceNoLock();
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x000FB66C File Offset: 0x000FA66C
		private void DefineVersionInfoResourceNoLock()
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
			base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x000FB6CC File Offset: 0x000FA6CC
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineUnmanagedResourceNoLock(resource);
					return;
				}
			}
			this.DefineUnmanagedResourceNoLock(resource);
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x000FB738 File Offset: 0x000FA738
		private void DefineUnmanagedResourceNoLock(byte[] resource)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_resourceBytes = new byte[resource.Length];
			Array.Copy(resource, base.m_assemblyData.m_resourceBytes, resource.Length);
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x000FB7A4 File Offset: 0x000FA7A4
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineUnmanagedResourceNoLock(resourceFileName);
					return;
				}
			}
			this.DefineUnmanagedResourceNoLock(resourceFileName);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x000FB810 File Offset: 0x000FA810
		private void DefineUnmanagedResourceNoLock(string resourceFileName)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			string text;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, resourceFileName);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, resourceFileName);
			}
			text = Path.GetFullPath(resourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, text).Demand();
			if (!File.Exists(text))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { resourceFileName }), resourceFileName);
			}
			base.m_assemblyData.m_strResourceFileName = text;
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x000FB8D0 File Offset: 0x000FA8D0
		public ModuleBuilder GetDynamicModule(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.GetDynamicModuleNoLock(name);
				}
			}
			return this.GetDynamicModuleNoLock(name);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x000FB930 File Offset: 0x000FA930
		private ModuleBuilder GetDynamicModuleNoLock(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			int count = base.m_assemblyData.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)base.m_assemblyData.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName.Equals(name))
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x000FB9B2 File Offset: 0x000FA9B2
		public void SetEntryPoint(MethodInfo entryMethod)
		{
			this.SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x000FB9BC File Offset: 0x000FA9BC
		public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetEntryPointNoLock(entryMethod, fileKind);
					return;
				}
			}
			this.SetEntryPointNoLock(entryMethod, fileKind);
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x000FBA1C File Offset: 0x000FAA1C
		private void SetEntryPointNoLock(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			if (entryMethod == null)
			{
				throw new ArgumentNullException("entryMethod");
			}
			Module internalModule = entryMethod.Module.InternalModule;
			if (!(internalModule is ModuleBuilder) || !this.InternalAssembly.Equals(internalModule.Assembly.InternalAssembly))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EntryMethodNotDefinedInAssembly"));
			}
			base.m_assemblyData.m_entryPointModule = (ModuleBuilder)ModuleBuilder.GetModuleBuilder(internalModule);
			base.m_assemblyData.m_entryPointMethod = entryMethod;
			base.m_assemblyData.m_peFileKind = fileKind;
			base.m_assemblyData.m_entryPointModule.SetEntryPoint(entryMethod);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x000FBAB4 File Offset: 0x000FAAB4
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetCustomAttributeNoLock(con, binaryAttribute);
					return;
				}
			}
			this.SetCustomAttributeNoLock(con, binaryAttribute);
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x000FBB30 File Offset: 0x000FAB30
		private void SetCustomAttributeNoLock(ConstructorInfo con, byte[] binaryAttribute)
		{
			ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
			TypeBuilder.InternalCreateCustomAttribute(536870913, inMemoryAssemblyModule.GetConstructorToken(con).Token, binaryAttribute, inMemoryAssemblyModule, false, typeof(DebuggableAttribute) == con.DeclaringType);
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				return;
			}
			base.m_assemblyData.AddCustomAttribute(con, binaryAttribute);
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x000FBB94 File Offset: 0x000FAB94
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetCustomAttributeNoLock(customBuilder);
					return;
				}
			}
			this.SetCustomAttributeNoLock(customBuilder);
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x000FBC00 File Offset: 0x000FAC00
		private void SetCustomAttributeNoLock(CustomAttributeBuilder customBuilder)
		{
			ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
			customBuilder.CreateCustomAttribute(inMemoryAssemblyModule, 536870913);
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				return;
			}
			base.m_assemblyData.AddCustomAttribute(customBuilder);
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x000FBC40 File Offset: 0x000FAC40
		public void Save(string assemblyFileName)
		{
			this.Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x000FBC50 File Offset: 0x000FAC50
		public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x000FBCB0 File Offset: 0x000FACB0
		private void SaveNoLock(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			int[] array = null;
			int[] array2 = null;
			string text = null;
			try
			{
				if (base.m_assemblyData.m_iCABuilder != 0)
				{
					array = new int[base.m_assemblyData.m_iCABuilder];
				}
				if (base.m_assemblyData.m_iCAs != 0)
				{
					array2 = new int[base.m_assemblyData.m_iCAs];
				}
				if (base.m_assemblyData.m_isSaved)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_AssemblyHasBeenSaved"), new object[] { base.nGetSimpleName() }));
				}
				if ((base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) != AssemblyBuilderAccess.Save)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CantSaveTransientAssembly"));
				}
				if (assemblyFileName == null)
				{
					throw new ArgumentNullException("assemblyFileName");
				}
				if (assemblyFileName.Length == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "assemblyFileName");
				}
				if (!string.Equals(assemblyFileName, Path.GetFileName(assemblyFileName)))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "assemblyFileName");
				}
				ModuleBuilder moduleBuilder = base.m_assemblyData.FindModuleWithFileName(assemblyFileName);
				if (moduleBuilder != null)
				{
					base.m_assemblyData.SetOnDiskAssemblyModule(moduleBuilder);
				}
				if (moduleBuilder == null)
				{
					base.m_assemblyData.CheckFileNameConflict(assemblyFileName);
				}
				if (base.m_assemblyData.m_strDir == null)
				{
					base.m_assemblyData.m_strDir = Environment.CurrentDirectory;
				}
				else if (!Directory.Exists(base.m_assemblyData.m_strDir))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Argument_InvalidDirectory"), new object[] { base.m_assemblyData.m_strDir }));
				}
				assemblyFileName = Path.Combine(base.m_assemblyData.m_strDir, assemblyFileName);
				assemblyFileName = Path.GetFullPath(assemblyFileName);
				new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, assemblyFileName).Demand();
				if (moduleBuilder != null)
				{
					for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
					{
						array[i] = base.m_assemblyData.m_CABuilders[i].PrepareCreateCustomAttributeToDisk(moduleBuilder);
					}
					for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
					{
						array2[i] = moduleBuilder.InternalGetConstructorToken(base.m_assemblyData.m_CACons[i], true).Token;
					}
					moduleBuilder.PreSave(assemblyFileName, portableExecutableKind, imageFileMachine);
				}
				base.nPrepareForSavingManifestToDisk(moduleBuilder);
				ModuleBuilder onDiskAssemblyModule = base.m_assemblyData.GetOnDiskAssemblyModule();
				if (base.m_assemblyData.m_strResourceFileName != null)
				{
					onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(base.m_assemblyData.m_strResourceFileName);
				}
				else if (base.m_assemblyData.m_resourceBytes != null)
				{
					onDiskAssemblyModule.DefineUnmanagedResourceInternalNoLock(base.m_assemblyData.m_resourceBytes);
				}
				else if (base.m_assemblyData.m_hasUnmanagedVersionInfo)
				{
					base.m_assemblyData.FillUnmanagedVersionInfo();
					string text2 = base.m_assemblyData.m_nativeVersion.m_strFileVersion;
					if (text2 == null)
					{
						text2 = base.GetVersion().ToString();
					}
					text = Assembly.nDefineVersionInfoResource(assemblyFileName, base.m_assemblyData.m_nativeVersion.m_strTitle, null, base.m_assemblyData.m_nativeVersion.m_strDescription, base.m_assemblyData.m_nativeVersion.m_strCopyright, base.m_assemblyData.m_nativeVersion.m_strTrademark, base.m_assemblyData.m_nativeVersion.m_strCompany, base.m_assemblyData.m_nativeVersion.m_strProduct, base.m_assemblyData.m_nativeVersion.m_strProductVersion, text2, base.m_assemblyData.m_nativeVersion.m_lcid, base.m_assemblyData.m_peFileKind == PEFileKinds.Dll);
					onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(text);
				}
				if (moduleBuilder == null)
				{
					for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
					{
						array[i] = base.m_assemblyData.m_CABuilders[i].PrepareCreateCustomAttributeToDisk(onDiskAssemblyModule);
					}
					for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
					{
						array2[i] = onDiskAssemblyModule.InternalGetConstructorToken(base.m_assemblyData.m_CACons[i], true).Token;
					}
				}
				int num = base.m_assemblyData.m_moduleBuilderList.Count;
				for (int i = 0; i < num; i++)
				{
					ModuleBuilder moduleBuilder2 = (ModuleBuilder)base.m_assemblyData.m_moduleBuilderList[i];
					if (!moduleBuilder2.IsTransient() && moduleBuilder2 != moduleBuilder)
					{
						string text3 = moduleBuilder2.m_moduleData.m_strFileName;
						if (base.m_assemblyData.m_strDir != null)
						{
							text3 = Path.Combine(base.m_assemblyData.m_strDir, text3);
							text3 = Path.GetFullPath(text3);
						}
						new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, text3).Demand();
						moduleBuilder2.m_moduleData.m_tkFile = base.nSaveToFileList(moduleBuilder2.m_moduleData.m_strFileName);
						moduleBuilder2.PreSave(text3, portableExecutableKind, imageFileMachine);
						moduleBuilder2.Save(text3, false, portableExecutableKind, imageFileMachine);
						base.nSetHashValue(moduleBuilder2.m_moduleData.m_tkFile, text3);
					}
				}
				for (int i = 0; i < base.m_assemblyData.m_iPublicComTypeCount; i++)
				{
					Type type = base.m_assemblyData.m_publicComTypeList[i];
					if (type is RuntimeType)
					{
						ModuleBuilder moduleBuilder3 = base.m_assemblyData.FindModuleWithName(type.Module.m_moduleData.m_strModuleName);
						if (moduleBuilder3 != moduleBuilder)
						{
							this.DefineNestedComType(type, moduleBuilder3.m_moduleData.m_tkFile, type.MetadataTokenInternal);
						}
					}
					else
					{
						TypeBuilder typeBuilder = (TypeBuilder)type;
						ModuleBuilder moduleBuilder3 = (ModuleBuilder)type.Module;
						if (moduleBuilder3 != moduleBuilder)
						{
							this.DefineNestedComType(type, moduleBuilder3.m_moduleData.m_tkFile, typeBuilder.MetadataTokenInternal);
						}
					}
				}
				for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
				{
					base.m_assemblyData.m_CABuilders[i].CreateCustomAttribute(onDiskAssemblyModule, 536870913, array[i], true);
				}
				for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
				{
					TypeBuilder.InternalCreateCustomAttribute(536870913, array2[i], base.m_assemblyData.m_CABytes[i], onDiskAssemblyModule, true);
				}
				if (base.m_assemblyData.m_RequiredPset != null || base.m_assemblyData.m_OptionalPset != null || base.m_assemblyData.m_RefusedPset != null)
				{
					byte[] array3 = null;
					byte[] array4 = null;
					byte[] array5 = null;
					if (base.m_assemblyData.m_RequiredPset != null)
					{
						array3 = base.m_assemblyData.m_RequiredPset.EncodeXml();
					}
					if (base.m_assemblyData.m_OptionalPset != null)
					{
						array4 = base.m_assemblyData.m_OptionalPset.EncodeXml();
					}
					if (base.m_assemblyData.m_RefusedPset != null)
					{
						array5 = base.m_assemblyData.m_RefusedPset.EncodeXml();
					}
					base.nSavePermissionRequests(array3, array4, array5);
				}
				num = base.m_assemblyData.m_resWriterList.Count;
				for (int i = 0; i < num; i++)
				{
					ResWriterData resWriterData = null;
					try
					{
						resWriterData = (ResWriterData)base.m_assemblyData.m_resWriterList[i];
						if (resWriterData.m_resWriter != null)
						{
							new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, resWriterData.m_strFullFileName).Demand();
						}
					}
					finally
					{
						if (resWriterData != null && resWriterData.m_resWriter != null)
						{
							resWriterData.m_resWriter.Close();
						}
					}
					base.nAddStandAloneResource(resWriterData.m_strName, resWriterData.m_strFileName, resWriterData.m_strFullFileName, (int)resWriterData.m_attribute);
				}
				if (moduleBuilder == null)
				{
					if (onDiskAssemblyModule.m_moduleData.m_strResourceFileName != null)
					{
						onDiskAssemblyModule.InternalDefineNativeResourceFile(onDiskAssemblyModule.m_moduleData.m_strResourceFileName, (int)portableExecutableKind, (int)imageFileMachine);
					}
					else if (onDiskAssemblyModule.m_moduleData.m_resourceBytes != null)
					{
						onDiskAssemblyModule.InternalDefineNativeResourceBytes(onDiskAssemblyModule.m_moduleData.m_resourceBytes, (int)portableExecutableKind, (int)imageFileMachine);
					}
					if (base.m_assemblyData.m_entryPointModule != null)
					{
						base.nSaveManifestToDisk(assemblyFileName, base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile, (int)base.m_assemblyData.m_peFileKind, (int)portableExecutableKind, (int)imageFileMachine);
					}
					else
					{
						base.nSaveManifestToDisk(assemblyFileName, 0, (int)base.m_assemblyData.m_peFileKind, (int)portableExecutableKind, (int)imageFileMachine);
					}
				}
				else
				{
					if (base.m_assemblyData.m_entryPointModule != null && base.m_assemblyData.m_entryPointModule != moduleBuilder)
					{
						moduleBuilder.m_EntryPoint = new MethodToken(base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile);
					}
					moduleBuilder.Save(assemblyFileName, true, portableExecutableKind, imageFileMachine);
				}
				base.m_assemblyData.m_isSaved = true;
			}
			finally
			{
				if (text != null)
				{
					File.Delete(text);
				}
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x000FC4CC File Offset: 0x000FB4CC
		internal bool IsPersistable()
		{
			return (base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) == AssemblyBuilderAccess.Save;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x000FC4E4 File Offset: 0x000FB4E4
		private int DefineNestedComType(Type type, int tkResolutionScope, int tkTypeDef)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				return base.nSaveExportedType(type.FullName, tkResolutionScope, tkTypeDef, type.Attributes);
			}
			tkResolutionScope = this.DefineNestedComType(declaringType, tkResolutionScope, tkTypeDef);
			return base.nSaveExportedType(type.FullName, tkResolutionScope, tkTypeDef, type.Attributes);
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x000FC52F File Offset: 0x000FB52F
		private AssemblyBuilder()
		{
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x000FC537 File Offset: 0x000FB537
		void _AssemblyBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x000FC53E File Offset: 0x000FB53E
		void _AssemblyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x000FC545 File Offset: 0x000FB545
		void _AssemblyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x000FC54C File Offset: 0x000FB54C
		void _AssemblyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002504 RID: 9476
		private AssemblyBuilder m_internalAssemblyBuilder;

		// Token: 0x04002505 RID: 9477
		private PermissionSet m_grantedPermissionSet;
	}
}
