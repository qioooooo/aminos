using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Security.Util;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x02000056 RID: 86
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_AppDomain))]
	public sealed class AppDomain : MarshalByRefObject, _AppDomain, IEvidenceFactory
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600048B RID: 1163 RVA: 0x00010E9C File Offset: 0x0000FE9C
		// (remove) Token: 0x0600048C RID: 1164 RVA: 0x00010EB5 File Offset: 0x0000FEB5
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event AssemblyLoadEventHandler AssemblyLoad;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600048D RID: 1165 RVA: 0x00010ECE File Offset: 0x0000FECE
		// (remove) Token: 0x0600048E RID: 1166 RVA: 0x00010EE7 File Offset: 0x0000FEE7
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler TypeResolve;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600048F RID: 1167 RVA: 0x00010F00 File Offset: 0x0000FF00
		// (remove) Token: 0x06000490 RID: 1168 RVA: 0x00010F19 File Offset: 0x0000FF19
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler ResourceResolve;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000491 RID: 1169 RVA: 0x00010F32 File Offset: 0x0000FF32
		// (remove) Token: 0x06000492 RID: 1170 RVA: 0x00010F4B File Offset: 0x0000FF4B
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler AssemblyResolve;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000493 RID: 1171 RVA: 0x00010F64 File Offset: 0x0000FF64
		// (remove) Token: 0x06000494 RID: 1172 RVA: 0x00010F7D File Offset: 0x0000FF7D
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler ReflectionOnlyAssemblyResolve;

		// Token: 0x06000495 RID: 1173 RVA: 0x00010F96 File Offset: 0x0000FF96
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06000496 RID: 1174
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string nGetDomainManagerAsm();

		// Token: 0x06000497 RID: 1175
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string nGetDomainManagerType();

		// Token: 0x06000498 RID: 1176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nSetHostSecurityManagerFlags(HostSecurityManagerOptions flags);

		// Token: 0x06000499 RID: 1177
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nSetSecurityHomogeneousFlag();

		// Token: 0x0600049A RID: 1178 RVA: 0x00010F9E File Offset: 0x0000FF9E
		private void SetDefaultDomainManager(string fullName, string[] manifestPaths, string[] activationData)
		{
			if (fullName != null)
			{
				this.FusionStore.ActivationArguments = new ActivationArguments(fullName, manifestPaths, activationData);
			}
			this.SetDomainManager(null, null, IntPtr.Zero, false);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00010FC4 File Offset: 0x0000FFC4
		private void SetDomainManager(Evidence providedSecurityInfo, Evidence creatorsSecurityInfo, IntPtr parentSecurityDescriptor, bool publishAppDomain)
		{
			string text = AppDomain.nGetDomainManagerAsm();
			string text2 = AppDomain.nGetDomainManagerType();
			if (text != null && text2 != null)
			{
				this._domainManager = this.CreateDomainManager(text, text2);
			}
			AppDomainSetup fusionStore = this.FusionStore;
			if (this._domainManager != null)
			{
				this._domainManager.InitializeNewDomain(fusionStore);
				AppDomainManagerInitializationOptions initializationFlags = this._domainManager.InitializationFlags;
				if ((initializationFlags & AppDomainManagerInitializationOptions.RegisterWithHost) == AppDomainManagerInitializationOptions.RegisterWithHost)
				{
					this._domainManager.nRegisterWithHost();
				}
			}
			if (fusionStore.ActivationArguments != null)
			{
				ActivationContext activationContext = null;
				ApplicationIdentity applicationIdentity = null;
				CmsUtils.CreateActivationContext(fusionStore.ActivationArguments.ApplicationFullName, fusionStore.ActivationArguments.ApplicationManifestPaths, fusionStore.ActivationArguments.UseFusionActivationContext, out applicationIdentity, out activationContext);
				string[] activationData = fusionStore.ActivationArguments.ActivationData;
				providedSecurityInfo = CmsUtils.MergeApplicationEvidence(providedSecurityInfo, applicationIdentity, activationContext, activationData, fusionStore.ApplicationTrust);
				this.SetupApplicationHelper(providedSecurityInfo, creatorsSecurityInfo, applicationIdentity, activationContext, activationData);
			}
			else
			{
				ApplicationTrust applicationTrust = fusionStore.ApplicationTrust;
				if (applicationTrust != null)
				{
					this.SetupDomainSecurityForApplication(applicationTrust.ApplicationIdentity, applicationTrust);
				}
			}
			Evidence evidence = ((providedSecurityInfo != null) ? providedSecurityInfo : creatorsSecurityInfo);
			if (this._domainManager != null)
			{
				HostSecurityManager hostSecurityManager = this._domainManager.HostSecurityManager;
				if (hostSecurityManager != null)
				{
					AppDomain.nSetHostSecurityManagerFlags(hostSecurityManager.Flags);
					if ((hostSecurityManager.Flags & HostSecurityManagerOptions.HostAppDomainEvidence) == HostSecurityManagerOptions.HostAppDomainEvidence)
					{
						evidence = hostSecurityManager.ProvideAppDomainEvidence(evidence);
					}
				}
			}
			this._SecurityIdentity = evidence;
			this.nSetupDomainSecurity(evidence, parentSecurityDescriptor, publishAppDomain);
			if (this._domainManager != null)
			{
				this.RunDomainManagerPostInitialization(this._domainManager);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001111C File Offset: 0x0001011C
		private AppDomainManager CreateDomainManager(string domainManagerAssemblyName, string domainManagerTypeName)
		{
			AppDomainManager appDomainManager = null;
			try
			{
				appDomainManager = this.CreateInstanceAndUnwrap(domainManagerAssemblyName, domainManagerTypeName) as AppDomainManager;
			}
			catch (FileNotFoundException)
			{
			}
			catch (TypeLoadException)
			{
			}
			finally
			{
				if (appDomainManager == null)
				{
					throw new TypeLoadException(Environment.GetResourceString("Argument_NoDomainManager"));
				}
			}
			return appDomainManager;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00011180 File Offset: 0x00010180
		private void RunDomainManagerPostInitialization(AppDomainManager domainManager)
		{
			HostExecutionContextManager hostExecutionContextManager = domainManager.HostExecutionContextManager;
			HostSecurityManager hostSecurityManager = domainManager.HostSecurityManager;
			if (hostSecurityManager != null && (hostSecurityManager.Flags & HostSecurityManagerOptions.HostPolicyLevel) == HostSecurityManagerOptions.HostPolicyLevel)
			{
				PolicyLevel domainPolicy = hostSecurityManager.DomainPolicy;
				if (domainPolicy != null)
				{
					this.SetAppDomainPolicy(domainPolicy);
				}
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000111BC File Offset: 0x000101BC
		private void SetupApplicationHelper(Evidence providedSecurityInfo, Evidence creatorsSecurityInfo, ApplicationIdentity appIdentity, ActivationContext activationContext, string[] activationData)
		{
			HostSecurityManager hostSecurityManager = AppDomain.CurrentDomain.HostSecurityManager;
			ApplicationTrust applicationTrust = hostSecurityManager.DetermineApplicationTrust(providedSecurityInfo, creatorsSecurityInfo, new TrustManagerContext());
			if (applicationTrust == null || !applicationTrust.IsApplicationTrustedToRun)
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoExecutionPermission"), -2146233320, null);
			}
			if (activationContext != null)
			{
				this.SetupDomainForApplication(activationContext, activationData);
			}
			this.SetupDomainSecurityForApplication(appIdentity, applicationTrust);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001121C File Offset: 0x0001021C
		private void SetupDomainForApplication(ActivationContext activationContext, string[] activationData)
		{
			if (this.IsDefaultAppDomain())
			{
				AppDomainSetup fusionStore = this.FusionStore;
				fusionStore.ActivationArguments = new ActivationArguments(activationContext, activationData);
				string entryPointFullPath = CmsUtils.GetEntryPointFullPath(activationContext);
				if (!string.IsNullOrEmpty(entryPointFullPath))
				{
					fusionStore.SetupDefaultApplicationBase(entryPointFullPath);
				}
				else
				{
					fusionStore.ApplicationBase = activationContext.ApplicationDirectory;
				}
				this.SetupFusionStore(fusionStore);
			}
			activationContext.PrepareForExecution();
			activationContext.SetApplicationState(ActivationContext.ApplicationState.Starting);
			activationContext.SetApplicationState(ActivationContext.ApplicationState.Running);
			IPermission permission = null;
			string dataDirectory = activationContext.DataDirectory;
			if (dataDirectory != null && dataDirectory.Length > 0)
			{
				permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, dataDirectory);
			}
			this.SetData("DataDirectory", dataDirectory, permission);
			this._activationContext = activationContext;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000112B7 File Offset: 0x000102B7
		private void SetupDomainSecurityForApplication(ApplicationIdentity appIdentity, ApplicationTrust appTrust)
		{
			this._applicationIdentity = appIdentity;
			this._applicationTrust = appTrust;
			AppDomain.nSetSecurityHomogeneousFlag();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000112CC File Offset: 0x000102CC
		private int ActivateApplication()
		{
			ObjectHandle objectHandle = Activator.CreateInstance(AppDomain.CurrentDomain.ActivationContext);
			return (int)objectHandle.Unwrap();
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x000112F4 File Offset: 0x000102F4
		public AppDomainManager DomainManager
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
			get
			{
				return this._domainManager;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x000112FC File Offset: 0x000102FC
		internal HostSecurityManager HostSecurityManager
		{
			get
			{
				HostSecurityManager hostSecurityManager = null;
				AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
				if (domainManager != null)
				{
					hostSecurityManager = domainManager.HostSecurityManager;
				}
				if (hostSecurityManager == null)
				{
					hostSecurityManager = new HostSecurityManager();
				}
				return hostSecurityManager;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001132A File Offset: 0x0001032A
		private Assembly ResolveAssemblyForIntrospection(object sender, ResolveEventArgs args)
		{
			return Assembly.ReflectionOnlyLoad(this.ApplyPolicy(args.Name));
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001133D File Offset: 0x0001033D
		private void EnableResolveAssembliesForIntrospection()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.ReflectionOnlyAssemblyResolve = (ResolveEventHandler)Delegate.Combine(currentDomain.ReflectionOnlyAssemblyResolve, new ResolveEventHandler(this.ResolveAssemblyForIntrospection));
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00011368 File Offset: 0x00010368
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, null, null, null, null, null, ref stackCrawlMark, null);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00011388 File Offset: 0x00010388
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, null, null, null, null, null, ref stackCrawlMark, assemblyAttributes);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000113AC File Offset: 0x000103AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, null, null, null, null, ref stackCrawlMark, null);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000113CC File Offset: 0x000103CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, null, evidence, null, null, null, ref stackCrawlMark, null);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000113EC File Offset: 0x000103EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, null, null, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, null);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00011410 File Offset: 0x00010410
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, evidence, null, null, null, ref stackCrawlMark, null);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00011430 File Offset: 0x00010430
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, null, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, null);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00011454 File Offset: 0x00010454
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, null, evidence, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, null);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00011478 File Offset: 0x00010478
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, null);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001149C File Offset: 0x0001049C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, null);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000114C4 File Offset: 0x000104C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalDefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, ref stackCrawlMark, assemblyAttributes);
		}

		// Token: 0x060004B1 RID: 1201
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string nApplyPolicy(AssemblyName an);

		// Token: 0x060004B2 RID: 1202 RVA: 0x000114EC File Offset: 0x000104EC
		[ComVisible(false)]
		public string ApplyPolicy(string assemblyName)
		{
			AssemblyName assemblyName2 = new AssemblyName(assemblyName);
			byte[] array = assemblyName2.GetPublicKeyToken();
			if (array == null)
			{
				array = assemblyName2.GetPublicKey();
			}
			if (array == null || array.Length == 0)
			{
				return assemblyName;
			}
			return this.nApplyPolicy(assemblyName2);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00011522 File Offset: 0x00010522
		public ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00011542 File Offset: 0x00010542
		internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName)
		{
			PermissionSet.s_fullTrust.Assert();
			return this.CreateInstance(assemblyName, typeName);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00011556 File Offset: 0x00010556
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00011568 File Offset: 0x00010568
		internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName)
		{
			PermissionSet.s_fullTrust.Assert();
			return this.CreateInstanceFrom(assemblyName, typeName);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001157C File Offset: 0x0001057C
		public ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			return Activator.CreateComInstanceFrom(assemblyName, typeName);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001158E File Offset: 0x0001058E
		public ObjectHandle CreateComInstanceFrom(string assemblyFile, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			return Activator.CreateComInstanceFrom(assemblyFile, typeName, hashValue, hashAlgorithm);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000115A3 File Offset: 0x000105A3
		public ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName, activationAttributes);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000115C4 File Offset: 0x000105C4
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName, activationAttributes);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000115D8 File Offset: 0x000105D8
		public ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00011610 File Offset: 0x00010610
		internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			PermissionSet.s_fullTrust.Assert();
			return this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001163C File Offset: 0x0001063C
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00011668 File Offset: 0x00010668
		internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			PermissionSet.s_fullTrust.Assert();
			return this.CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00011694 File Offset: 0x00010694
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(AssemblyName assemblyRef)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyRef, null, ref stackCrawlMark, false);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000116B0 File Offset: 0x000106B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(string assemblyString)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyString, null, ref stackCrawlMark, false);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000116CC File Offset: 0x000106CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(byte[] rawAssembly)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, null, null, ref stackCrawlMark, false);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000116E8 File Offset: 0x000106E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, rawSymbolStore, null, ref stackCrawlMark, false);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00011704 File Offset: 0x00010704
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, rawSymbolStore, securityEvidence, ref stackCrawlMark, false);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00011720 File Offset: 0x00010720
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyRef, assemblySecurity, ref stackCrawlMark, false);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001173C File Offset: 0x0001073C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(string assemblyString, Evidence assemblySecurity)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyString, assemblySecurity, ref stackCrawlMark, false);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00011755 File Offset: 0x00010755
		public int ExecuteAssembly(string assemblyFile)
		{
			return this.ExecuteAssembly(assemblyFile, null, null);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00011760 File Offset: 0x00010760
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity)
		{
			return this.ExecuteAssembly(assemblyFile, assemblySecurity, null);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001176C File Offset: 0x0001076C
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyFile, assemblySecurity);
			if (args == null)
			{
				args = new string[0];
			}
			return this.nExecuteAssembly(assembly, args);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00011794 File Offset: 0x00010794
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyFile, assemblySecurity, hashValue, hashAlgorithm);
			if (args == null)
			{
				args = new string[0];
			}
			return this.nExecuteAssembly(assembly, args);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000117C0 File Offset: 0x000107C0
		public int ExecuteAssemblyByName(string assemblyName)
		{
			return this.ExecuteAssemblyByName(assemblyName, null, null);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000117CB File Offset: 0x000107CB
		public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity)
		{
			return this.ExecuteAssemblyByName(assemblyName, assemblySecurity, null);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000117D8 File Offset: 0x000107D8
		public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly assembly = Assembly.Load(assemblyName, assemblySecurity);
			if (args == null)
			{
				args = new string[0];
			}
			return this.nExecuteAssembly(assembly, args);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00011800 File Offset: 0x00010800
		public int ExecuteAssemblyByName(AssemblyName assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly assembly = Assembly.Load(assemblyName, assemblySecurity);
			if (args == null)
			{
				args = new string[0];
			}
			return this.nExecuteAssembly(assembly, args);
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00011828 File Offset: 0x00010828
		public static AppDomain CurrentDomain
		{
			get
			{
				return Thread.GetDomain();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00011830 File Offset: 0x00010830
		public Evidence Evidence
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			get
			{
				if (this._SecurityIdentity == null)
				{
					if (this.IsDefaultAppDomain())
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							return entryAssembly.Evidence;
						}
						return new Evidence();
					}
					else if (this.nIsDefaultAppDomainForSecurity())
					{
						return AppDomain.GetDefaultDomain().Evidence;
					}
				}
				Evidence internalEvidence = this.InternalEvidence;
				if (internalEvidence != null)
				{
					return internalEvidence.Copy();
				}
				return internalEvidence;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00011888 File Offset: 0x00010888
		internal Evidence InternalEvidence
		{
			get
			{
				return this._SecurityIdentity;
			}
		}

		// Token: 0x060004D1 RID: 1233
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern uint GetAppDomainId();

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00011890 File Offset: 0x00010890
		public string FriendlyName
		{
			get
			{
				return this.nGetFriendlyName();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00011898 File Offset: 0x00010898
		public string BaseDirectory
		{
			get
			{
				return this.FusionStore.ApplicationBase;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x000118A5 File Offset: 0x000108A5
		public string RelativeSearchPath
		{
			get
			{
				return this.FusionStore.PrivateBinPath;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000118B4 File Offset: 0x000108B4
		public bool ShadowCopyFiles
		{
			get
			{
				string shadowCopyFiles = this.FusionStore.ShadowCopyFiles;
				return shadowCopyFiles != null && string.Compare(shadowCopyFiles, "true", StringComparison.OrdinalIgnoreCase) == 0;
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000118E4 File Offset: 0x000108E4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.nGetFriendlyName();
			if (text != null)
			{
				stringBuilder.Append(Environment.GetResourceString("Loader_Name") + text);
				stringBuilder.Append(Environment.NewLine);
			}
			if (this._Policies == null || this._Policies.Length == 0)
			{
				stringBuilder.Append(Environment.GetResourceString("Loader_NoContextPolicies") + Environment.NewLine);
			}
			else
			{
				stringBuilder.Append(Environment.GetResourceString("Loader_ContextPolicies") + Environment.NewLine);
				for (int i = 0; i < this._Policies.Length; i++)
				{
					stringBuilder.Append(this._Policies[i]);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x000119A2 File Offset: 0x000109A2
		public Assembly[] GetAssemblies()
		{
			return this.nGetAssemblies(false);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000119AB File Offset: 0x000109AB
		public Assembly[] ReflectionOnlyGetAssemblies()
		{
			return this.nGetAssemblies(true);
		}

		// Token: 0x060004D9 RID: 1241
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Assembly[] nGetAssemblies(bool forIntrospection);

		// Token: 0x060004DA RID: 1242
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsUnloadingForcedFinalize();

		// Token: 0x060004DB RID: 1243
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsFinalizingForUnload();

		// Token: 0x060004DC RID: 1244
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void PublishAnonymouslyHostedDynamicMethodsAssembly(Assembly assembly);

		// Token: 0x060004DD RID: 1245 RVA: 0x000119B4 File Offset: 0x000109B4
		[Obsolete("AppDomain.AppendPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void AppendPrivatePath(string path)
		{
			if (path == null || path.Length == 0)
			{
				return;
			}
			string text = this.FusionStore.Value[5];
			StringBuilder stringBuilder = new StringBuilder();
			if (text != null && text.Length > 0)
			{
				stringBuilder.Append(text);
				if (text[text.Length - 1] != Path.PathSeparator && path[0] != Path.PathSeparator)
				{
					stringBuilder.Append(Path.PathSeparator);
				}
			}
			stringBuilder.Append(path);
			string text2 = stringBuilder.ToString();
			this.InternalSetPrivateBinPath(text2);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00011A3A File Offset: 0x00010A3A
		[Obsolete("AppDomain.ClearPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearPrivatePath()
		{
			this.InternalSetPrivateBinPath(string.Empty);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00011A47 File Offset: 0x00010A47
		[Obsolete("AppDomain.ClearShadowCopyPath has been deprecated. Please investigate the use of AppDomainSetup.ShadowCopyDirectories instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearShadowCopyPath()
		{
			this.InternalSetShadowCopyPath(string.Empty);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00011A54 File Offset: 0x00010A54
		[Obsolete("AppDomain.SetCachePath has been deprecated. Please investigate the use of AppDomainSetup.CachePath instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetCachePath(string path)
		{
			this.InternalSetCachePath(path);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00011A5D File Offset: 0x00010A5D
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetData(string name, object data)
		{
			this.SetDataHelper(name, data, null);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00011A68 File Offset: 0x00010A68
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetData(string name, object data, IPermission permission)
		{
			this.SetDataHelper(name, data, permission);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00011A74 File Offset: 0x00010A74
		private void SetDataHelper(string name, object data, IPermission permission)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Equals("IgnoreSystemPolicy"))
			{
				lock (this)
				{
					if (!this._HasSetPolicy)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SetData"));
					}
				}
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			int num = AppDomainSetup.Locate(name);
			if (num == -1)
			{
				this.LocalStore[name] = new object[] { data, permission };
				return;
			}
			if (permission != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SetData"));
			}
			switch (num)
			{
			case 2:
				this.FusionStore.DynamicBase = (string)data;
				return;
			case 3:
				this.FusionStore.DeveloperPath = (string)data;
				return;
			case 7:
				this.FusionStore.ShadowCopyDirectories = (string)data;
				return;
			case 11:
				if (data != null)
				{
					this.FusionStore.DisallowPublisherPolicy = true;
					return;
				}
				this.FusionStore.DisallowPublisherPolicy = false;
				return;
			case 12:
				if (data != null)
				{
					this.FusionStore.DisallowCodeDownload = true;
					return;
				}
				this.FusionStore.DisallowCodeDownload = false;
				return;
			case 13:
				if (data != null)
				{
					this.FusionStore.DisallowBindingRedirects = true;
					return;
				}
				this.FusionStore.DisallowBindingRedirects = false;
				return;
			case 14:
				if (data != null)
				{
					this.FusionStore.DisallowApplicationBaseProbing = true;
					return;
				}
				this.FusionStore.DisallowApplicationBaseProbing = false;
				return;
			case 15:
				this.FusionStore.SetConfigurationBytes((byte[])data);
				return;
			}
			this.FusionStore.Value[num] = (string)data;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00011C34 File Offset: 0x00010C34
		public object GetData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			int num = AppDomainSetup.Locate(name);
			if (num == -1)
			{
				if (name.Equals(AppDomainSetup.LoaderOptimizationKey))
				{
					return this.FusionStore.LoaderOptimization;
				}
				object[] array = (object[])this.LocalStore[name];
				if (array == null)
				{
					return null;
				}
				if (array[1] != null)
				{
					IPermission permission = (IPermission)array[1];
					permission.Demand();
				}
				return array[0];
			}
			else
			{
				switch (num)
				{
				case 0:
					return this.FusionStore.ApplicationBase;
				case 1:
					return this.FusionStore.ConfigurationFile;
				case 2:
					return this.FusionStore.DynamicBase;
				case 3:
					return this.FusionStore.DeveloperPath;
				case 4:
					return this.FusionStore.ApplicationName;
				case 5:
					return this.FusionStore.PrivateBinPath;
				case 6:
					return this.FusionStore.PrivateBinPathProbe;
				case 7:
					return this.FusionStore.ShadowCopyDirectories;
				case 8:
					return this.FusionStore.ShadowCopyFiles;
				case 9:
					return this.FusionStore.CachePath;
				case 10:
					return this.FusionStore.LicenseFile;
				case 11:
					return this.FusionStore.DisallowPublisherPolicy;
				case 12:
					return this.FusionStore.DisallowCodeDownload;
				case 13:
					return this.FusionStore.DisallowBindingRedirects;
				case 14:
					return this.FusionStore.DisallowApplicationBaseProbing;
				case 15:
					return this.FusionStore.GetConfigurationBytes();
				default:
					return null;
				}
			}
		}

		// Token: 0x060004E5 RID: 1253
		[Obsolete("AppDomain.GetCurrentThreadId has been deprecated because it does not provide a stable Id when managed threads are running on fibers (aka lightweight threads). To get a stable identifier for a managed thread, use the ManagedThreadId property on Thread.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[DllImport("kernel32.dll")]
		public static extern int GetCurrentThreadId();

		// Token: 0x060004E6 RID: 1254 RVA: 0x00011DCC File Offset: 0x00010DCC
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
		public static void Unload(AppDomain domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			try
			{
				int idForUnload = AppDomain.GetIdForUnload(domain);
				if (idForUnload == 0)
				{
					throw new CannotUnloadAppDomainException();
				}
				AppDomain.nUnload(idForUnload);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00011E14 File Offset: 0x00010E14
		[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
		public void SetAppDomainPolicy(PolicyLevel domainPolicy)
		{
			if (domainPolicy == null)
			{
				throw new ArgumentNullException("domainPolicy");
			}
			lock (this)
			{
				if (this._HasSetPolicy)
				{
					throw new PolicyException(Environment.GetResourceString("Policy_PolicyAlreadySet"));
				}
				this._HasSetPolicy = true;
				this.nChangeSecurityPolicy();
			}
			SecurityManager.PolicyManager.AddLevel(domainPolicy);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00011E80 File Offset: 0x00010E80
		public ActivationContext ActivationContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
			get
			{
				return this._activationContext;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00011E88 File Offset: 0x00010E88
		public ApplicationIdentity ApplicationIdentity
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
			get
			{
				return this._applicationIdentity;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00011E90 File Offset: 0x00010E90
		public ApplicationTrust ApplicationTrust
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
			get
			{
				return this._applicationTrust;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00011E98 File Offset: 0x00010E98
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public void SetThreadPrincipal(IPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentNullException("principal");
			}
			lock (this)
			{
				if (this._DefaultPrincipal != null)
				{
					throw new PolicyException(Environment.GetResourceString("Policy_PrincipalTwice"));
				}
				this._DefaultPrincipal = principal;
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00011EF4 File Offset: 0x00010EF4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public void SetPrincipalPolicy(PrincipalPolicy policy)
		{
			this._PrincipalPolicy = policy;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00011EFD File Offset: 0x00010EFD
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00011F00 File Offset: 0x00010F00
		public void DoCallBack(CrossAppDomainDelegate callBackDelegate)
		{
			if (callBackDelegate == null)
			{
				throw new ArgumentNullException("callBackDelegate");
			}
			callBackDelegate();
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00011F18 File Offset: 0x00010F18
		public string DynamicDirectory
		{
			get
			{
				string dynamicDir = this.GetDynamicDir();
				if (dynamicDir != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, dynamicDir).Demand();
				}
				return dynamicDir;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00011F3C File Offset: 0x00010F3C
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo)
		{
			return AppDomain.CreateDomain(friendlyName, securityInfo, null);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00011F48 File Offset: 0x00010F48
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = appBasePath;
			appDomainSetup.PrivateBinPath = appRelativeSearchPath;
			if (shadowCopyFiles)
			{
				appDomainSetup.ShadowCopyFiles = "true";
			}
			return AppDomain.CreateDomain(friendlyName, securityInfo, appDomainSetup);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00011F80 File Offset: 0x00010F80
		public static AppDomain CreateDomain(string friendlyName)
		{
			return AppDomain.CreateDomain(friendlyName, null, null);
		}

		// Token: 0x060004F3 RID: 1267
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetDynamicDir();

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011F8A File Offset: 0x00010F8A
		private static byte[] MarshalObject(object o)
		{
			CodeAccessPermission.AssertAllPossible();
			return AppDomain.Serialize(o);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00011F98 File Offset: 0x00010F98
		private static byte[] MarshalObjects(object o1, object o2, out byte[] blob2)
		{
			CodeAccessPermission.AssertAllPossible();
			byte[] array = AppDomain.Serialize(o1);
			blob2 = AppDomain.Serialize(o2);
			return array;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00011FBA File Offset: 0x00010FBA
		private static object UnmarshalObject(byte[] blob)
		{
			CodeAccessPermission.AssertAllPossible();
			return AppDomain.Deserialize(blob);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011FC8 File Offset: 0x00010FC8
		private static object UnmarshalObjects(byte[] blob1, byte[] blob2, out object o2)
		{
			CodeAccessPermission.AssertAllPossible();
			object obj = AppDomain.Deserialize(blob1);
			o2 = AppDomain.Deserialize(blob2);
			return obj;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00011FEC File Offset: 0x00010FEC
		private static byte[] Serialize(object o)
		{
			if (o == null)
			{
				return null;
			}
			if (o is ISecurityEncodable)
			{
				SecurityElement securityElement = ((ISecurityEncodable)o).ToXml();
				MemoryStream memoryStream = new MemoryStream(4096);
				memoryStream.WriteByte(0);
				StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
				securityElement.ToWriter(streamWriter);
				streamWriter.Flush();
				return memoryStream.ToArray();
			}
			MemoryStream memoryStream2 = new MemoryStream();
			memoryStream2.WriteByte(1);
			CrossAppDomainSerializer.SerializeObject(o, memoryStream2);
			return memoryStream2.ToArray();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00012060 File Offset: 0x00011060
		private static object Deserialize(byte[] blob)
		{
			if (blob == null)
			{
				return null;
			}
			if (blob[0] != 0)
			{
				object obj = null;
				using (MemoryStream memoryStream = new MemoryStream(blob, 1, blob.Length - 1))
				{
					obj = CrossAppDomainSerializer.DeserializeObject(memoryStream);
				}
				return obj;
			}
			Parser parser = new Parser(blob, Tokenizer.ByteTokenEncoding.UTF8Tokens, 1);
			SecurityElement topElement = parser.GetTopElement();
			if (topElement.Tag.Equals("IPermission") || topElement.Tag.Equals("Permission"))
			{
				IPermission permission = XMLUtil.CreatePermission(topElement, PermissionState.None, false);
				if (permission == null)
				{
					return null;
				}
				permission.FromXml(topElement);
				return permission;
			}
			else
			{
				if (topElement.Tag.Equals("PermissionSet"))
				{
					PermissionSet permissionSet = new PermissionSet();
					permissionSet.FromXml(topElement, false, false);
					return permissionSet;
				}
				if (topElement.Tag.Equals("PermissionToken"))
				{
					PermissionToken permissionToken = new PermissionToken();
					permissionToken.FromXml(topElement);
					return permissionToken;
				}
				return null;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00012148 File Offset: 0x00011148
		private AppDomain()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Constructor"));
		}

		// Token: 0x060004FB RID: 1275
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Assembly nCreateDynamicAssembly(AssemblyName name, Evidence identity, ref StackCrawlMark stackMark, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, AssemblyBuilderAccess access, DynamicAssemblyFlags flags);

		// Token: 0x060004FC RID: 1276 RVA: 0x00012160 File Offset: 0x00011160
		internal AssemblyBuilder InternalDefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, ref StackCrawlMark stackMark, IEnumerable<CustomAttributeBuilder> unsafeAssemblyAttributes)
		{
			AssemblyBuilder assemblyBuilder2;
			lock (typeof(AppDomain.AssemblyBuilderLock))
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				if (access != AssemblyBuilderAccess.Run && access != AssemblyBuilderAccess.Save && access != AssemblyBuilderAccess.RunAndSave && access != AssemblyBuilderAccess.ReflectionOnly)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)access }), "access");
				}
				if (name.KeyPair != null)
				{
					name.SetPublicKey(name.KeyPair.PublicKey);
				}
				if (evidence != null)
				{
					new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
				}
				List<CustomAttributeBuilder> list = null;
				DynamicAssemblyFlags dynamicAssemblyFlags = DynamicAssemblyFlags.None;
				if (unsafeAssemblyAttributes != null)
				{
					list = new List<CustomAttributeBuilder>(unsafeAssemblyAttributes);
					foreach (CustomAttributeBuilder customAttributeBuilder in list)
					{
						if (customAttributeBuilder.m_con.DeclaringType == typeof(SecurityTransparentAttribute))
						{
							dynamicAssemblyFlags |= DynamicAssemblyFlags.Transparent;
						}
					}
				}
				AssemblyBuilder assemblyBuilder = new AssemblyBuilder((AssemblyBuilder)this.nCreateDynamicAssembly(name, evidence, ref stackMark, requiredPermissions, optionalPermissions, refusedPermissions, access, dynamicAssemblyFlags));
				assemblyBuilder.m_assemblyData = new AssemblyBuilderData(assemblyBuilder, name.Name, access, dir);
				assemblyBuilder.m_assemblyData.AddPermissionRequests(requiredPermissions, optionalPermissions, refusedPermissions);
				if (list != null)
				{
					foreach (CustomAttributeBuilder customAttributeBuilder2 in list)
					{
						assemblyBuilder.SetCustomAttribute(customAttributeBuilder2);
					}
				}
				assemblyBuilder.m_assemblyData.GetInMemoryAssemblyModule();
				assemblyBuilder2 = assemblyBuilder;
			}
			return assemblyBuilder2;
		}

		// Token: 0x060004FD RID: 1277
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nExecuteAssembly(Assembly assembly, string[] args);

		// Token: 0x060004FE RID: 1278 RVA: 0x00012328 File Offset: 0x00011328
		internal int nExecuteAssembly(Assembly assembly, string[] args)
		{
			return this._nExecuteAssembly(assembly.InternalAssembly, args);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00012338 File Offset: 0x00011338
		internal void CreateRemotingData()
		{
			lock (this)
			{
				if (this._RemotingData == null)
				{
					this._RemotingData = new DomainSpecificRemotingData();
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001237C File Offset: 0x0001137C
		internal DomainSpecificRemotingData RemotingData
		{
			get
			{
				if (this._RemotingData == null)
				{
					this.CreateRemotingData();
				}
				return this._RemotingData;
			}
		}

		// Token: 0x06000501 RID: 1281
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string nGetFriendlyName();

		// Token: 0x06000502 RID: 1282
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool nIsDefaultAppDomainForSecurity();

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000503 RID: 1283 RVA: 0x00012394 File Offset: 0x00011394
		// (remove) Token: 0x06000504 RID: 1284 RVA: 0x000123E4 File Offset: 0x000113E4
		public event EventHandler ProcessExit
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			add
			{
				if (value != null)
				{
					RuntimeHelpers.PrepareDelegate(value);
					lock (this)
					{
						this._processExit = (EventHandler)Delegate.Combine(this._processExit, value);
					}
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			remove
			{
				lock (this)
				{
					this._processExit = (EventHandler)Delegate.Remove(this._processExit, value);
				}
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000505 RID: 1285 RVA: 0x0001242C File Offset: 0x0001142C
		// (remove) Token: 0x06000506 RID: 1286 RVA: 0x0001247C File Offset: 0x0001147C
		public event EventHandler DomainUnload
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			add
			{
				if (value != null)
				{
					RuntimeHelpers.PrepareDelegate(value);
					lock (this)
					{
						this._domainUnload = (EventHandler)Delegate.Combine(this._domainUnload, value);
					}
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			remove
			{
				lock (this)
				{
					this._domainUnload = (EventHandler)Delegate.Remove(this._domainUnload, value);
				}
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000507 RID: 1287 RVA: 0x000124C4 File Offset: 0x000114C4
		// (remove) Token: 0x06000508 RID: 1288 RVA: 0x00012514 File Offset: 0x00011514
		public event UnhandledExceptionEventHandler UnhandledException
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			add
			{
				if (value != null)
				{
					RuntimeHelpers.PrepareDelegate(value);
					lock (this)
					{
						this._unhandledException = (UnhandledExceptionEventHandler)Delegate.Combine(this._unhandledException, value);
					}
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			remove
			{
				lock (this)
				{
					this._unhandledException = (UnhandledExceptionEventHandler)Delegate.Remove(this._unhandledException, value);
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001255C File Offset: 0x0001155C
		private void OnAssemblyLoadEvent(Assembly LoadedAssembly)
		{
			AssemblyLoadEventHandler assemblyLoad = this.AssemblyLoad;
			if (assemblyLoad != null)
			{
				AssemblyLoadEventArgs assemblyLoadEventArgs = new AssemblyLoadEventArgs(LoadedAssembly);
				assemblyLoad(this, assemblyLoadEventArgs);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00012584 File Offset: 0x00011584
		private Assembly OnResourceResolveEvent(string resourceName)
		{
			ResolveEventHandler resourceResolve = this.ResourceResolve;
			if (resourceResolve == null)
			{
				return null;
			}
			Delegate[] invocationList = resourceResolve.GetInvocationList();
			int num = invocationList.Length;
			for (int i = 0; i < num; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(resourceName));
				if (assembly != null)
				{
					return assembly.InternalAssembly;
				}
			}
			return null;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000125D8 File Offset: 0x000115D8
		private Assembly OnTypeResolveEvent(string typeName)
		{
			ResolveEventHandler typeResolve = this.TypeResolve;
			if (typeResolve == null)
			{
				return null;
			}
			Delegate[] invocationList = typeResolve.GetInvocationList();
			int num = invocationList.Length;
			for (int i = 0; i < num; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(typeName));
				if (assembly != null)
				{
					return assembly.InternalAssembly;
				}
			}
			return null;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001262C File Offset: 0x0001162C
		private Assembly OnAssemblyResolveEvent(string assemblyFullName)
		{
			ResolveEventHandler assemblyResolve = this.AssemblyResolve;
			if (assemblyResolve == null)
			{
				return null;
			}
			Delegate[] invocationList = assemblyResolve.GetInvocationList();
			int num = invocationList.Length;
			for (int i = 0; i < num; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(assemblyFullName));
				if (assembly != null)
				{
					return assembly.InternalAssembly;
				}
			}
			return null;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00012680 File Offset: 0x00011680
		private Assembly OnReflectionOnlyAssemblyResolveEvent(string assemblyFullName)
		{
			ResolveEventHandler reflectionOnlyAssemblyResolve = this.ReflectionOnlyAssemblyResolve;
			if (reflectionOnlyAssemblyResolve != null)
			{
				Delegate[] invocationList = reflectionOnlyAssemblyResolve.GetInvocationList();
				int num = invocationList.Length;
				for (int i = 0; i < num; i++)
				{
					Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(assemblyFullName));
					if (assembly != null)
					{
						return assembly.InternalAssembly;
					}
				}
			}
			return null;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x000126D1 File Offset: 0x000116D1
		internal AppDomainSetup FusionStore
		{
			get
			{
				return this._FusionStore;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x000126D9 File Offset: 0x000116D9
		private Hashtable LocalStore
		{
			get
			{
				if (this._LocalStore != null)
				{
					return this._LocalStore;
				}
				this._LocalStore = Hashtable.Synchronized(new Hashtable());
				return this._LocalStore;
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00012700 File Offset: 0x00011700
		private void TurnOnBindingRedirects()
		{
			this._FusionStore.DisallowBindingRedirects = false;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001270E File Offset: 0x0001170E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int GetIdForUnload(AppDomain domain)
		{
			if (RemotingServices.IsTransparentProxy(domain))
			{
				return RemotingServices.GetServerDomainIdForProxy(domain);
			}
			return domain.Id;
		}

		// Token: 0x06000512 RID: 1298
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDomainIdValid(int id);

		// Token: 0x06000513 RID: 1299
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AppDomain GetDefaultDomain();

		// Token: 0x06000514 RID: 1300 RVA: 0x00012728 File Offset: 0x00011728
		internal IPrincipal GetThreadPrincipal()
		{
			IPrincipal principal2;
			lock (this)
			{
				IPrincipal principal;
				if (this._DefaultPrincipal == null)
				{
					switch (this._PrincipalPolicy)
					{
					case PrincipalPolicy.UnauthenticatedPrincipal:
						principal = new GenericPrincipal(new GenericIdentity("", ""), new string[] { "" });
						break;
					case PrincipalPolicy.NoPrincipal:
						principal = null;
						break;
					case PrincipalPolicy.WindowsPrincipal:
						principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
						break;
					default:
						principal = null;
						break;
					}
				}
				else
				{
					principal = this._DefaultPrincipal;
				}
				principal2 = principal;
			}
			return principal2;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000127C4 File Offset: 0x000117C4
		internal void CreateDefaultContext()
		{
			lock (this)
			{
				if (this._DefaultContext == null)
				{
					this._DefaultContext = Context.CreateDefaultContext();
				}
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00012808 File Offset: 0x00011808
		internal Context GetDefaultContext()
		{
			if (this._DefaultContext == null)
			{
				this.CreateDefaultContext();
			}
			return this._DefaultContext;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00012820 File Offset: 0x00011820
		[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info)
		{
			AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
			if (domainManager != null)
			{
				return domainManager.CreateDomain(friendlyName, securityInfo, info);
			}
			if (friendlyName == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_String"));
			}
			if (securityInfo != null)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			return AppDomain.nCreateDomain(friendlyName, info, securityInfo, (securityInfo == null) ? AppDomain.CurrentDomain.InternalEvidence : null, AppDomain.CurrentDomain.GetSecurityDescriptor());
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00012889 File Offset: 0x00011889
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info, PermissionSet grantSet, params StrongName[] fullTrustAssemblies)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (info.ApplicationBase == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AppDomainSandboxAPINeedsExplicitAppBase"));
			}
			info.ApplicationTrust = new ApplicationTrust(grantSet, fullTrustAssemblies);
			return AppDomain.CreateDomain(friendlyName, securityInfo, info);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000128C8 File Offset: 0x000118C8
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles, AppDomainInitializer adInit, string[] adInitArgs)
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = appBasePath;
			appDomainSetup.PrivateBinPath = appRelativeSearchPath;
			appDomainSetup.AppDomainInitializer = adInit;
			appDomainSetup.AppDomainInitializerArguments = adInitArgs;
			if (shadowCopyFiles)
			{
				appDomainSetup.ShadowCopyFiles = "true";
			}
			return AppDomain.CreateDomain(friendlyName, securityInfo, appDomainSetup);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00012910 File Offset: 0x00011910
		private void SetupFusionStore(AppDomainSetup info)
		{
			if (info.Value[0] == null || info.Value[1] == null)
			{
				AppDomain defaultDomain = AppDomain.GetDefaultDomain();
				if (this == defaultDomain)
				{
					info.SetupDefaultApplicationBase(RuntimeEnvironment.GetModuleFileName());
				}
				else
				{
					if (info.Value[1] == null)
					{
						info.ConfigurationFile = defaultDomain.FusionStore.Value[1];
					}
					if (info.Value[0] == null)
					{
						info.ApplicationBase = defaultDomain.FusionStore.Value[0];
					}
					if (info.Value[4] == null)
					{
						info.ApplicationName = defaultDomain.FusionStore.Value[4];
					}
				}
			}
			if (info.Value[5] == null)
			{
				info.PrivateBinPath = Environment.nativeGetEnvironmentVariable(AppDomainSetup.PrivateBinPathEnvironmentVariable);
			}
			if (info.DeveloperPath == null)
			{
				info.DeveloperPath = RuntimeEnvironment.GetDeveloperPath();
			}
			IntPtr fusionContext = this.GetFusionContext();
			info.SetupFusionContext(fusionContext);
			if (info.LoaderOptimization != LoaderOptimization.NotSpecified)
			{
				this.UpdateLoaderOptimization((int)info.LoaderOptimization);
			}
			this._FusionStore = info;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000129F8 File Offset: 0x000119F8
		private static void RunInitializer(AppDomainSetup setup)
		{
			if (setup.AppDomainInitializer != null)
			{
				string[] array = null;
				if (setup.AppDomainInitializerArguments != null)
				{
					array = (string[])setup.AppDomainInitializerArguments.Clone();
				}
				setup.AppDomainInitializer(array);
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00012A34 File Offset: 0x00011A34
		private static object RemotelySetupRemoteDomain(AppDomain appDomainProxy, string friendlyName, AppDomainSetup setup, Evidence providedSecurityInfo, Evidence creatorsSecurityInfo, IntPtr parentSecurityDescriptor)
		{
			IntPtr intPtr;
			int num;
			RemotingServices.GetServerContextAndDomainIdForProxy(appDomainProxy, out intPtr, out num);
			if (intPtr == IntPtr.Zero)
			{
				throw new AppDomainUnloadedException();
			}
			AppDomain.EvidenceCollection evidenceCollection = null;
			if (providedSecurityInfo != null || creatorsSecurityInfo != null)
			{
				evidenceCollection = new AppDomain.EvidenceCollection();
				evidenceCollection.ProvidedSecurityInfo = providedSecurityInfo;
				evidenceCollection.CreatorsSecurityInfo = creatorsSecurityInfo;
			}
			bool flag = false;
			char[] array = null;
			char[] array2 = null;
			byte[] array3 = null;
			AppDomainInitializerInfo appDomainInitializerInfo = null;
			if (providedSecurityInfo != null)
			{
				array = PolicyManager.MakeEvidenceArray(providedSecurityInfo, true);
				if (array == null)
				{
					flag = true;
				}
			}
			if (creatorsSecurityInfo != null && !flag)
			{
				array2 = PolicyManager.MakeEvidenceArray(creatorsSecurityInfo, true);
				if (array2 == null)
				{
					flag = true;
				}
			}
			if (evidenceCollection != null && flag)
			{
				array2 = (array = null);
				array3 = CrossAppDomainSerializer.SerializeObject(evidenceCollection).GetBuffer();
			}
			if (setup != null && setup.AppDomainInitializer != null)
			{
				appDomainInitializerInfo = new AppDomainInitializerInfo(setup.AppDomainInitializer);
			}
			return AppDomain.InternalRemotelySetupRemoteDomain(intPtr, num, friendlyName, setup, parentSecurityDescriptor, array, array2, array3, appDomainInitializerInfo);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00012AF8 File Offset: 0x00011AF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static object InternalRemotelySetupRemoteDomainHelper(object[] args)
		{
			string text = (string)args[0];
			AppDomainSetup appDomainSetup = (AppDomainSetup)args[1];
			IntPtr intPtr = (IntPtr)args[2];
			char[] array = (char[])args[3];
			char[] array2 = (char[])args[4];
			byte[] array3 = (byte[])args[5];
			AppDomainInitializerInfo appDomainInitializerInfo = (AppDomainInitializerInfo)args[6];
			AppDomain appDomain = Thread.CurrentContext.AppDomain;
			AppDomainSetup appDomainSetup2 = new AppDomainSetup(appDomainSetup, false);
			appDomain.SetupFusionStore(appDomainSetup2);
			Evidence evidence = null;
			Evidence evidence2 = null;
			if (array3 == null)
			{
				if (array != null)
				{
					evidence = new Evidence(array);
				}
				if (array2 != null)
				{
					evidence2 = new Evidence(array2);
				}
			}
			else
			{
				AppDomain.EvidenceCollection evidenceCollection = (AppDomain.EvidenceCollection)CrossAppDomainSerializer.DeserializeObject(new MemoryStream(array3));
				evidence = evidenceCollection.ProvidedSecurityInfo;
				evidence2 = evidenceCollection.CreatorsSecurityInfo;
			}
			appDomain.nSetupFriendlyName(text);
			if (appDomainSetup != null && appDomainSetup.SandboxInterop)
			{
				appDomain.nSetDisableInterfaceCache();
			}
			appDomain.SetDomainManager(evidence, evidence2, intPtr, true);
			if (appDomainInitializerInfo != null)
			{
				appDomainSetup2.AppDomainInitializer = appDomainInitializerInfo.Unwrap();
			}
			AppDomain.RunInitializer(appDomainSetup2);
			ObjectHandle objectHandle = null;
			AppDomainSetup fusionStore = appDomain.FusionStore;
			if (fusionStore.ActivationArguments != null && fusionStore.ActivationArguments.ActivateInstance)
			{
				objectHandle = Activator.CreateInstance(appDomain.ActivationContext);
			}
			return RemotingServices.MarshalInternal(objectHandle, null, null);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00012C28 File Offset: 0x00011C28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static object InternalRemotelySetupRemoteDomain(IntPtr contextId, int domainId, string friendlyName, AppDomainSetup setup, IntPtr parentSecurityDescriptor, char[] serProvidedEvidence, char[] serCreatorEvidence, byte[] serializedEvidence, AppDomainInitializerInfo initializerInfo)
		{
			InternalCrossContextDelegate internalCrossContextDelegate = new InternalCrossContextDelegate(AppDomain.InternalRemotelySetupRemoteDomainHelper);
			object[] array = new object[] { friendlyName, setup, parentSecurityDescriptor, serProvidedEvidence, serCreatorEvidence, serializedEvidence, initializerInfo };
			return Thread.CurrentThread.InternalCrossContextCallback(null, contextId, domainId, internalCrossContextDelegate, array);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00012C80 File Offset: 0x00011C80
		private void SetupDomain(bool allowRedirects, string path, string configFile)
		{
			lock (this)
			{
				if (this._FusionStore == null)
				{
					AppDomainSetup appDomainSetup = new AppDomainSetup();
					if (path != null)
					{
						appDomainSetup.Value[0] = path;
					}
					if (configFile != null)
					{
						appDomainSetup.Value[1] = configFile;
					}
					if (!allowRedirects)
					{
						appDomainSetup.DisallowBindingRedirects = true;
					}
					this.SetupFusionStore(appDomainSetup);
				}
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00012CE8 File Offset: 0x00011CE8
		private void SetupLoaderOptimization(LoaderOptimization policy)
		{
			if (policy != LoaderOptimization.NotSpecified)
			{
				this.FusionStore.LoaderOptimization = policy;
				this.UpdateLoaderOptimization((int)this.FusionStore.LoaderOptimization);
			}
		}

		// Token: 0x06000521 RID: 1313
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetFusionContext();

		// Token: 0x06000522 RID: 1314
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetSecurityDescriptor();

		// Token: 0x06000523 RID: 1315
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AppDomain nCreateDomain(string friendlyName, AppDomainSetup setup, Evidence providedSecurityInfo, Evidence creatorsSecurityInfo, IntPtr parentSecurityDescriptor);

		// Token: 0x06000524 RID: 1316
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ObjRef nCreateInstance(string friendlyName, AppDomainSetup setup, Evidence providedSecurityInfo, Evidence creatorsSecurityInfo, IntPtr parentSecurityDescriptor);

		// Token: 0x06000525 RID: 1317
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void nSetupDomainSecurity(Evidence appDomainEvidence, IntPtr creatorsSecurityDescriptor, bool publishAppDomain);

		// Token: 0x06000526 RID: 1318
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void nSetupFriendlyName(string friendlyName);

		// Token: 0x06000527 RID: 1319
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void nSetDisableInterfaceCache();

		// Token: 0x06000528 RID: 1320
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void UpdateLoaderOptimization(int optimization);

		// Token: 0x06000529 RID: 1321 RVA: 0x00012D0A File Offset: 0x00011D0A
		[Obsolete("AppDomain.SetShadowCopyPath has been deprecated. Please investigate the use of AppDomainSetup.ShadowCopyDirectories instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyPath(string path)
		{
			this.InternalSetShadowCopyPath(path);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00012D13 File Offset: 0x00011D13
		[Obsolete("AppDomain.SetShadowCopyFiles has been deprecated. Please investigate the use of AppDomainSetup.ShadowCopyFiles instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyFiles()
		{
			this.InternalSetShadowCopyFiles();
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00012D1B File Offset: 0x00011D1B
		[Obsolete("AppDomain.SetDynamicBase has been deprecated. Please investigate the use of AppDomainSetup.DynamicBase instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetDynamicBase(string path)
		{
			this.InternalSetDynamicBase(path);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00012D24 File Offset: 0x00011D24
		public AppDomainSetup SetupInformation
		{
			get
			{
				return new AppDomainSetup(this.FusionStore, true);
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00012D34 File Offset: 0x00011D34
		internal void InternalSetShadowCopyPath(string path)
		{
			IntPtr fusionContext = this.GetFusionContext();
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ShadowCopyDirectoriesKey, path);
			this.FusionStore.ShadowCopyDirectories = path;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00012D60 File Offset: 0x00011D60
		internal void InternalSetShadowCopyFiles()
		{
			IntPtr fusionContext = this.GetFusionContext();
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ShadowCopyFilesKey, "true");
			this.FusionStore.ShadowCopyFiles = "true";
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00012D94 File Offset: 0x00011D94
		internal void InternalSetCachePath(string path)
		{
			IntPtr fusionContext = this.GetFusionContext();
			this.FusionStore.CachePath = path;
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.CachePathKey, this.FusionStore.Value[9]);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00012DD0 File Offset: 0x00011DD0
		internal void InternalSetPrivateBinPath(string path)
		{
			IntPtr fusionContext = this.GetFusionContext();
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.PrivateBinPathKey, path);
			this.FusionStore.PrivateBinPath = path;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00012DFC File Offset: 0x00011DFC
		internal void InternalSetDynamicBase(string path)
		{
			IntPtr fusionContext = this.GetFusionContext();
			this.FusionStore.DynamicBase = path;
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DynamicBaseKey, this.FusionStore.Value[2]);
		}

		// Token: 0x06000532 RID: 1330
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string IsStringInterned(string str);

		// Token: 0x06000533 RID: 1331
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetOrInternString(string str);

		// Token: 0x06000534 RID: 1332
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void nGetGrantSet(out PermissionSet granted, out PermissionSet denied);

		// Token: 0x06000535 RID: 1333
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void nChangeSecurityPolicy();

		// Token: 0x06000536 RID: 1334
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nUnload(int domainInternal);

		// Token: 0x06000537 RID: 1335 RVA: 0x00012E34 File Offset: 0x00011E34
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00012E58 File Offset: 0x00011E58
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00012E7C File Offset: 0x00011E7C
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00012EAC File Offset: 0x00011EAC
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00012ED0 File Offset: 0x00011ED0
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00012EF4 File Offset: 0x00011EF4
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00012F22 File Offset: 0x00011F22
		public int Id
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.GetId();
			}
		}

		// Token: 0x0600053E RID: 1342
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetId();

		// Token: 0x0600053F RID: 1343 RVA: 0x00012F2A File Offset: 0x00011F2A
		public bool IsDefaultAppDomain()
		{
			return this == AppDomain.GetDefaultDomain();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00012F38 File Offset: 0x00011F38
		private static AppDomainSetup InternalCreateDomainSetup(string imageLocation)
		{
			int num = imageLocation.LastIndexOf('\\');
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = imageLocation.Substring(0, num + 1);
			StringBuilder stringBuilder = new StringBuilder(imageLocation.Substring(num + 1));
			stringBuilder.Append(AppDomainSetup.ConfigurationExtension);
			appDomainSetup.ConfigurationFile = stringBuilder.ToString();
			return appDomainSetup;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00012F8C File Offset: 0x00011F8C
		private static AppDomain InternalCreateDomain(string imageLocation)
		{
			AppDomainSetup appDomainSetup = AppDomain.InternalCreateDomainSetup(imageLocation);
			return AppDomain.CreateDomain("Validator", null, appDomainSetup);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00012FAC File Offset: 0x00011FAC
		private void InternalSetDomainContext(string imageLocation)
		{
			this.SetupFusionStore(AppDomain.InternalCreateDomainSetup(imageLocation));
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00012FBA File Offset: 0x00011FBA
		void _AppDomain.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00012FC1 File Offset: 0x00011FC1
		void _AppDomain.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012FC8 File Offset: 0x00011FC8
		void _AppDomain.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012FCF File Offset: 0x00011FCF
		void _AppDomain.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000195 RID: 405
		private AppDomainManager _domainManager;

		// Token: 0x04000196 RID: 406
		private Hashtable _LocalStore;

		// Token: 0x04000197 RID: 407
		private AppDomainSetup _FusionStore;

		// Token: 0x04000198 RID: 408
		private Evidence _SecurityIdentity;

		// Token: 0x04000199 RID: 409
		private object[] _Policies;

		// Token: 0x0400019F RID: 415
		private Context _DefaultContext;

		// Token: 0x040001A0 RID: 416
		private ActivationContext _activationContext;

		// Token: 0x040001A1 RID: 417
		private ApplicationIdentity _applicationIdentity;

		// Token: 0x040001A2 RID: 418
		private ApplicationTrust _applicationTrust;

		// Token: 0x040001A3 RID: 419
		private IPrincipal _DefaultPrincipal;

		// Token: 0x040001A4 RID: 420
		private DomainSpecificRemotingData _RemotingData;

		// Token: 0x040001A5 RID: 421
		private EventHandler _processExit;

		// Token: 0x040001A6 RID: 422
		private EventHandler _domainUnload;

		// Token: 0x040001A7 RID: 423
		private UnhandledExceptionEventHandler _unhandledException;

		// Token: 0x040001A8 RID: 424
		private IntPtr _dummyField;

		// Token: 0x040001A9 RID: 425
		private PrincipalPolicy _PrincipalPolicy;

		// Token: 0x040001AA RID: 426
		private bool _HasSetPolicy;

		// Token: 0x02000057 RID: 87
		internal class AssemblyBuilderLock
		{
		}

		// Token: 0x02000058 RID: 88
		[Serializable]
		private class EvidenceCollection
		{
			// Token: 0x040001AB RID: 427
			public Evidence ProvidedSecurityInfo;

			// Token: 0x040001AC RID: 428
			public Evidence CreatorsSecurityInfo;
		}
	}
}
