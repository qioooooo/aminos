using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Vsa;

namespace Microsoft.JScript.Vsa
{
	// Token: 0x0200013B RID: 315
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("B71E484D-93ED-4b56-BFB9-CEED5134822B")]
	[ComVisible(true)]
	public sealed class VsaEngine : BaseVsaEngine, IEngine2, IRedirectOutput
	{
		// Token: 0x06000E11 RID: 3601 RVA: 0x0005E6E8 File Offset: 0x0005D6E8
		private static string GetVersionString()
		{
			return string.Concat(new object[]
			{
				8,
				".",
				0.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
				".",
				0.ToString(CultureInfo.InvariantCulture),
				".",
				50727.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')
			});
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0005E76A File Offset: 0x0005D76A
		public VsaEngine()
			: this(true)
		{
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0005E774 File Offset: 0x0005D774
		public VsaEngine(bool fast)
			: base("JScript", VsaEngine.engineVersion, true)
		{
			this.alwaysGenerateIL = false;
			this.autoRef = false;
			this.doCRS = false;
			this.doFast = fast;
			this.genDebugInfo = false;
			this.genStartupClass = true;
			this.doPrint = false;
			this.doWarnAsError = false;
			this.nWarningLevel = 4;
			this.isCLSCompliant = false;
			this.versionSafe = false;
			this.PEFileName = null;
			this.PEFileKind = PEFileKinds.Dll;
			this.PEKindFlags = PortableExecutableKinds.ILOnly;
			this.PEMachineArchitecture = ImageFileMachine.I386;
			this.ReferenceLoaderAPI = LoaderAPI.LoadFrom;
			this.errorCultureInfo = null;
			this.libpath = null;
			this.libpathList = null;
			this.globalScope = null;
			this.vsaItems = new VsaItems(this);
			this.packages = null;
			this.scopes = null;
			this.classCounter = 0;
			this.implicitAssemblies = null;
			this.implicitAssemblyCache = null;
			this.cachedTypeLookups = null;
			this.isEngineRunning = false;
			this.isEngineCompiled = false;
			this.isCompilerSet = false;
			this.isClosed = false;
			this.runningThread = null;
			this.compilerGlobals = null;
			this.globals = null;
			this.runtimeDirectory = null;
			Globals.contextEngine = this;
			this.runtimeAssembly = null;
			this.typenameTable = null;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0005E8A4 File Offset: 0x0005D8A4
		private VsaEngine(Assembly runtimeAssembly)
			: this(true)
		{
			this.runtimeAssembly = runtimeAssembly;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0005E8B4 File Offset: 0x0005D8B4
		internal void EnsureReflectionOnlyModulesLoaded()
		{
			if (VsaEngine.reflectionOnlyVsaModule == null)
			{
				VsaEngine.reflectionOnlyVsaModule = Assembly.ReflectionOnlyLoadFrom(typeof(IVsaEngine).Assembly.Location).GetModule("Microsoft.Vsa.dll");
				VsaEngine.reflectionOnlyJScriptModule = Assembly.ReflectionOnlyLoadFrom(typeof(VsaEngine).Assembly.Location).GetModule("Microsoft.JScript.dll");
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0005E918 File Offset: 0x0005D918
		internal Module VsaModule
		{
			get
			{
				if (this.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom)
				{
					return typeof(IVsaEngine).Module;
				}
				this.EnsureReflectionOnlyModulesLoaded();
				return VsaEngine.reflectionOnlyVsaModule;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0005E93E File Offset: 0x0005D93E
		internal Module JScriptModule
		{
			get
			{
				if (this.ReferenceLoaderAPI != LoaderAPI.ReflectionOnlyLoadFrom)
				{
					return typeof(VsaEngine).Module;
				}
				this.EnsureReflectionOnlyModulesLoaded();
				return VsaEngine.reflectionOnlyJScriptModule;
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0005E964 File Offset: 0x0005D964
		private void AddChildAndValue(XmlDocument doc, XmlElement parent, string name, string value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			this.CreateAttribute(doc, xmlElement, "Value", value);
			parent.AppendChild(xmlElement);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0005E990 File Offset: 0x0005D990
		internal void AddPackage(PackageScope pscope)
		{
			if (this.packages == null)
			{
				this.packages = new ArrayList(8);
			}
			foreach (object obj in this.packages)
			{
				PackageScope packageScope = (PackageScope)obj;
				if (packageScope.name.Equals(pscope.name))
				{
					packageScope.owner.MergeWith(pscope.owner);
					return;
				}
			}
			this.packages.Add(pscope);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0005EA08 File Offset: 0x0005DA08
		internal void CheckForErrors()
		{
			if (!this.isClosed && !this.isEngineCompiled)
			{
				this.SetUpCompilerEnvironment();
				this.Globals.ScopeStack.Push(this.GetGlobalScope().GetObject());
				try
				{
					foreach (object obj in this.vsaItems)
					{
						if (obj is VsaReference)
						{
							((VsaReference)obj).Compile();
						}
					}
					if (this.vsaItems.Count > 0)
					{
						this.SetEnclosingContext(new WrappedNamespace("", this));
					}
					foreach (object obj2 in this.vsaItems)
					{
						if (!(obj2 is VsaReference))
						{
							((VsaItem)obj2).CheckForErrors();
						}
					}
					if (this.globalScope != null)
					{
						this.globalScope.CheckForErrors();
					}
				}
				finally
				{
					this.Globals.ScopeStack.Pop();
				}
			}
			this.globalScope = null;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0005EB50 File Offset: 0x0005DB50
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public IVsaEngine Clone(AppDomain domain)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0005EB58 File Offset: 0x0005DB58
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public bool CompileEmpty()
		{
			base.TryObtainLock();
			bool flag;
			try
			{
				flag = this.DoCompile();
			}
			finally
			{
				base.ReleaseLock();
			}
			return flag;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0005EB8C File Offset: 0x0005DB8C
		private void CreateAttribute(XmlDocument doc, XmlElement elem, string name, string value)
		{
			XmlAttribute xmlAttribute = doc.CreateAttribute(name);
			elem.SetAttributeNode(xmlAttribute);
			elem.SetAttribute(name, value);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0005EBB2 File Offset: 0x0005DBB2
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void ConnectEvents()
		{
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x0005EBB4 File Offset: 0x0005DBB4
		internal CompilerGlobals CompilerGlobals
		{
			get
			{
				if (this.compilerGlobals == null)
				{
					this.compilerGlobals = new CompilerGlobals(this, base.Name, this.PEFileName, this.PEFileKind, this.doSaveAfterCompile || this.genStartupClass, !this.doSaveAfterCompile || this.genStartupClass, this.genDebugInfo, this.isCLSCompliant, this.versionInfo, this.globals);
				}
				return this.compilerGlobals;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0005EC28 File Offset: 0x0005DC28
		private TypeReferences TypeRefs
		{
			get
			{
				TypeReferences typeReferences;
				if (LoaderAPI.ReflectionOnlyLoadFrom == this.ReferenceLoaderAPI)
				{
					typeReferences = VsaEngine._reflectionOnlyTypeRefs;
					if (typeReferences == null)
					{
						typeReferences = (VsaEngine._reflectionOnlyTypeRefs = new TypeReferences(this.JScriptModule));
					}
				}
				else
				{
					typeReferences = Runtime.TypeRefs;
				}
				return typeReferences;
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0005EC64 File Offset: 0x0005DC64
		public static GlobalScope CreateEngineAndGetGlobalScope(bool fast, string[] assemblyNames)
		{
			VsaEngine vsaEngine = new VsaEngine(fast);
			vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", new DefaultVsaSite());
			vsaEngine.doPrint = true;
			vsaEngine.SetEnclosingContext(new WrappedNamespace("", vsaEngine));
			foreach (string text in assemblyNames)
			{
				VsaReference vsaReference = (VsaReference)vsaEngine.vsaItems.CreateItem(text, VsaItemType.Reference, VsaItemFlag.None);
				vsaReference.AssemblyName = text;
			}
			VsaEngine.exeEngine = vsaEngine;
			GlobalScope globalScope = (GlobalScope)vsaEngine.GetGlobalScope().GetObject();
			globalScope.globalObject = vsaEngine.Globals.globalObject;
			return globalScope;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0005ED02 File Offset: 0x0005DD02
		public static GlobalScope CreateEngineAndGetGlobalScopeWithType(bool fast, string[] assemblyNames, RuntimeTypeHandle callingTypeHandle)
		{
			return VsaEngine.CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace(fast, assemblyNames, callingTypeHandle, null);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0005ED10 File Offset: 0x0005DD10
		public static GlobalScope CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace(bool fast, string[] assemblyNames, RuntimeTypeHandle callingTypeHandle, string rootNamespace)
		{
			VsaEngine vsaEngine = new VsaEngine(fast);
			vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", new DefaultVsaSite());
			vsaEngine.doPrint = true;
			vsaEngine.SetEnclosingContext(new WrappedNamespace("", vsaEngine));
			if (rootNamespace != null)
			{
				vsaEngine.SetEnclosingContext(new WrappedNamespace(rootNamespace, vsaEngine));
			}
			foreach (string text in assemblyNames)
			{
				VsaReference vsaReference = (VsaReference)vsaEngine.vsaItems.CreateItem(text, VsaItemType.Reference, VsaItemFlag.None);
				vsaReference.AssemblyName = text;
			}
			Type typeFromHandle = Type.GetTypeFromHandle(callingTypeHandle);
			Assembly assembly = typeFromHandle.Assembly;
			CallContext.SetData("JScript:" + assembly.FullName, vsaEngine);
			GlobalScope globalScope = (GlobalScope)vsaEngine.GetGlobalScope().GetObject();
			globalScope.globalObject = vsaEngine.Globals.globalObject;
			return globalScope;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0005EDE0 File Offset: 0x0005DDE0
		public static VsaEngine CreateEngine()
		{
			if (VsaEngine.exeEngine == null)
			{
				VsaEngine vsaEngine = new VsaEngine(true);
				vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", new DefaultVsaSite());
				VsaEngine.exeEngine = vsaEngine;
			}
			return VsaEngine.exeEngine;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0005EE1C File Offset: 0x0005DE1C
		internal static VsaEngine CreateEngineForDebugger()
		{
			VsaEngine vsaEngine = new VsaEngine(true);
			vsaEngine.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", new DefaultVsaSite());
			GlobalScope globalScope = (GlobalScope)vsaEngine.GetGlobalScope().GetObject();
			globalScope.globalObject = vsaEngine.Globals.globalObject;
			return vsaEngine;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0005EE64 File Offset: 0x0005DE64
		public static VsaEngine CreateEngineWithType(RuntimeTypeHandle callingTypeHandle)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(callingTypeHandle);
			Assembly assembly = typeFromHandle.Assembly;
			object data = CallContext.GetData("JScript:" + assembly.FullName);
			if (data != null)
			{
				VsaEngine vsaEngine = data as VsaEngine;
				if (vsaEngine != null)
				{
					return vsaEngine;
				}
			}
			VsaEngine vsaEngine2 = new VsaEngine(assembly);
			vsaEngine2.InitVsaEngine("JScript.Vsa.VsaEngine://Microsoft.JScript.VsaEngine.Vsa", new DefaultVsaSite());
			GlobalScope globalScope = (GlobalScope)vsaEngine2.GetGlobalScope().GetObject();
			globalScope.globalObject = vsaEngine2.Globals.globalObject;
			int num = 0;
			Type type = null;
			do
			{
				string text = "JScript " + num.ToString(CultureInfo.InvariantCulture);
				type = assembly.GetType(text, false);
				if (type != null)
				{
					vsaEngine2.SetEnclosingContext(new WrappedNamespace("", vsaEngine2));
					ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(GlobalScope) });
					MethodInfo method = type.GetMethod("Global Code");
					try
					{
						object obj = constructor.Invoke(new object[] { globalScope });
						method.Invoke(obj, new object[0]);
					}
					catch (SecurityException)
					{
						break;
					}
				}
				num++;
			}
			while (type != null);
			if (data == null)
			{
				CallContext.SetData("JScript:" + assembly.FullName, vsaEngine2);
			}
			return vsaEngine2;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0005EFBC File Offset: 0x0005DFBC
		private void AddReferences()
		{
			if (VsaEngine.assemblyReferencesTable == null)
			{
				Hashtable hashtable = new Hashtable();
				VsaEngine.assemblyReferencesTable = Hashtable.Synchronized(hashtable);
			}
			string[] array = VsaEngine.assemblyReferencesTable[this.runtimeAssembly.FullName] as string[];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					VsaReference vsaReference = (VsaReference)this.vsaItems.CreateItem(array[i], VsaItemType.Reference, VsaItemFlag.None);
					vsaReference.AssemblyName = array[i];
				}
				return;
			}
			object[] customAttributes = CustomAttribute.GetCustomAttributes(this.runtimeAssembly, typeof(ReferenceAttribute), false);
			string[] array2 = new string[customAttributes.Length];
			for (int j = 0; j < customAttributes.Length; j++)
			{
				string reference = ((ReferenceAttribute)customAttributes[j]).reference;
				VsaReference vsaReference2 = (VsaReference)this.vsaItems.CreateItem(reference, VsaItemType.Reference, VsaItemFlag.None);
				vsaReference2.AssemblyName = reference;
				array2[j] = reference;
			}
			VsaEngine.assemblyReferencesTable[this.runtimeAssembly.FullName] = array2;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0005F0B4 File Offset: 0x0005E0B4
		private void EmitReferences()
		{
			SimpleHashtable simpleHashtable = new SimpleHashtable((uint)(this.vsaItems.Count + ((this.implicitAssemblies == null) ? 0 : this.implicitAssemblies.Count)));
			foreach (object obj in this.vsaItems)
			{
				if (obj is VsaReference)
				{
					string fullName = ((VsaReference)obj).Assembly.GetName().FullName;
					if (simpleHashtable[fullName] == null)
					{
						CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(CompilerGlobals.referenceAttributeConstructor, new object[] { fullName });
						this.CompilerGlobals.assemblyBuilder.SetCustomAttribute(customAttributeBuilder);
						simpleHashtable[fullName] = obj;
					}
				}
			}
			if (this.implicitAssemblies != null)
			{
				foreach (object obj2 in this.implicitAssemblies)
				{
					Assembly assembly = obj2 as Assembly;
					if (assembly != null)
					{
						string fullName2 = assembly.GetName().FullName;
						if (simpleHashtable[fullName2] == null)
						{
							CustomAttributeBuilder customAttributeBuilder2 = new CustomAttributeBuilder(CompilerGlobals.referenceAttributeConstructor, new object[] { fullName2 });
							this.CompilerGlobals.assemblyBuilder.SetCustomAttribute(customAttributeBuilder2);
							simpleHashtable[fullName2] = obj2;
						}
					}
				}
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0005F238 File Offset: 0x0005E238
		private void CreateMain()
		{
			TypeBuilder typeBuilder = this.CompilerGlobals.module.DefineType("JScript Main", TypeAttributes.Public);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, Typeob.Void, new Type[] { Typeob.ArrayOfString });
			methodBuilder.SetCustomAttribute(Typeob.STAThreadAttribute.GetConstructor(Type.EmptyTypes), new byte[0]);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			this.CreateEntryPointIL(ilgenerator, null);
			typeBuilder.CreateType();
			this.CompilerGlobals.assemblyBuilder.SetEntryPoint(methodBuilder, this.PEFileKind);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0005F2C8 File Offset: 0x0005E2C8
		private void CreateStartupClass()
		{
			TypeBuilder typeBuilder = this.CompilerGlobals.module.DefineType(this.rootNamespace + "._Startup", TypeAttributes.Public, Typeob.BaseVsaStartup);
			FieldInfo field = Typeob.BaseVsaStartup.GetField("site", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Startup", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, Typeob.Void, Type.EmptyTypes);
			this.CreateEntryPointIL(methodBuilder.GetILGenerator(), field, typeBuilder);
			MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("Shutdown", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, Typeob.Void, Type.EmptyTypes);
			this.CreateShutdownIL(methodBuilder2.GetILGenerator());
			typeBuilder.CreateType();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0005F35F File Offset: 0x0005E35F
		private void CreateEntryPointIL(ILGenerator il, FieldInfo site)
		{
			this.CreateEntryPointIL(il, site, null);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0005F36C File Offset: 0x0005E36C
		private void CreateEntryPointIL(ILGenerator il, FieldInfo site, TypeBuilder startupClass)
		{
			LocalBuilder localBuilder = il.DeclareLocal(Typeob.GlobalScope);
			if (this.doFast)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			SimpleHashtable simpleHashtable = new SimpleHashtable((uint)this.vsaItems.Count);
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.vsaItems)
			{
				if (obj is VsaReference)
				{
					string fullName = ((VsaReference)obj).Assembly.GetName().FullName;
					if (simpleHashtable[fullName] == null)
					{
						arrayList.Add(fullName);
						simpleHashtable[fullName] = obj;
					}
				}
			}
			if (this.implicitAssemblies != null)
			{
				foreach (object obj2 in this.implicitAssemblies)
				{
					Assembly assembly = obj2 as Assembly;
					if (assembly != null)
					{
						string fullName2 = assembly.GetName().FullName;
						if (simpleHashtable[fullName2] == null)
						{
							arrayList.Add(fullName2);
							simpleHashtable[fullName2] = obj2;
						}
					}
				}
			}
			ConstantWrapper.TranslateToILInt(il, arrayList.Count);
			il.Emit(OpCodes.Newarr, Typeob.String);
			int num = 0;
			foreach (object obj3 in arrayList)
			{
				string text = (string)obj3;
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, num++);
				il.Emit(OpCodes.Ldstr, text);
				il.Emit(OpCodes.Stelem_Ref);
			}
			if (startupClass != null)
			{
				il.Emit(OpCodes.Ldtoken, startupClass);
				if (this.rootNamespace != null)
				{
					il.Emit(OpCodes.Ldstr, this.rootNamespace);
				}
				else
				{
					il.Emit(OpCodes.Ldnull);
				}
				MethodInfo method = Typeob.VsaEngine.GetMethod("CreateEngineAndGetGlobalScopeWithTypeAndRootNamespace");
				il.Emit(OpCodes.Call, method);
			}
			else
			{
				MethodInfo method2 = Typeob.VsaEngine.GetMethod("CreateEngineAndGetGlobalScope");
				il.Emit(OpCodes.Call, method2);
			}
			il.Emit(OpCodes.Stloc, localBuilder);
			if (site != null)
			{
				this.CreateHostCallbackIL(il, site);
			}
			bool flag = this.genDebugInfo;
			bool flag2 = false;
			foreach (object obj4 in this.vsaItems)
			{
				Type compiledType = ((VsaItem)obj4).GetCompiledType();
				if (compiledType != null)
				{
					ConstructorInfo constructor = compiledType.GetConstructor(new Type[] { Typeob.GlobalScope });
					MethodInfo method3 = compiledType.GetMethod("Global Code");
					if (flag)
					{
						this.CompilerGlobals.module.SetUserEntryPoint(method3);
						flag = false;
					}
					il.Emit(OpCodes.Ldloc, localBuilder);
					il.Emit(OpCodes.Newobj, constructor);
					if (!flag2 && obj4 is VsaStaticCode)
					{
						LocalBuilder localBuilder2 = il.DeclareLocal(compiledType);
						il.Emit(OpCodes.Stloc, localBuilder2);
						il.Emit(OpCodes.Ldloc, localBuilder);
						il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
						il.Emit(OpCodes.Ldloc, localBuilder2);
						il.Emit(OpCodes.Call, CompilerGlobals.pushScriptObjectMethod);
						il.Emit(OpCodes.Ldloc, localBuilder2);
						flag2 = true;
					}
					il.Emit(OpCodes.Call, method3);
					il.Emit(OpCodes.Pop);
				}
			}
			if (flag2)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
				il.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
				il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
				il.Emit(OpCodes.Pop);
			}
			il.Emit(OpCodes.Ret);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0005F7A0 File Offset: 0x0005E7A0
		private void CreateHostCallbackIL(ILGenerator il, FieldInfo site)
		{
			MethodInfo method = site.FieldType.GetMethod("GetGlobalInstance");
			site.FieldType.GetMethod("GetEventSourceInstance");
			foreach (object obj in this.vsaItems)
			{
				if (obj is VsaHostObject)
				{
					VsaHostObject vsaHostObject = (VsaHostObject)obj;
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldfld, site);
					il.Emit(OpCodes.Ldstr, vsaHostObject.Name);
					il.Emit(OpCodes.Callvirt, method);
					Type fieldType = vsaHostObject.Field.FieldType;
					il.Emit(OpCodes.Ldtoken, fieldType);
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
					ConstantWrapper.TranslateToILInt(il, 0);
					il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
					if (fieldType.IsValueType)
					{
						Convert.EmitUnbox(il, fieldType, Type.GetTypeCode(fieldType));
					}
					else
					{
						il.Emit(OpCodes.Castclass, fieldType);
					}
					il.Emit(OpCodes.Stsfld, vsaHostObject.Field);
				}
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0005F8D8 File Offset: 0x0005E8D8
		private void CreateShutdownIL(ILGenerator il)
		{
			foreach (object obj in this.vsaItems)
			{
				if (obj is VsaHostObject)
				{
					il.Emit(OpCodes.Ldnull);
					il.Emit(OpCodes.Stsfld, ((VsaHostObject)obj).Field);
				}
			}
			il.Emit(OpCodes.Ret);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0005F95C File Offset: 0x0005E95C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void DisconnectEvents()
		{
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0005F960 File Offset: 0x0005E960
		protected override void DoClose()
		{
			((VsaItems)this.vsaItems).Close();
			if (this.globalScope != null)
			{
				this.globalScope.Close();
			}
			this.vsaItems = null;
			this.engineSite = null;
			this.globalScope = null;
			this.runningThread = null;
			this.compilerGlobals = null;
			this.globals = null;
			ScriptStream.Out = Console.Out;
			ScriptStream.Error = Console.Error;
			this.rawPE = null;
			this.rawPDB = null;
			this.isClosed = true;
			if (this.tempDirectory != null && Directory.Exists(this.tempDirectory))
			{
				Directory.Delete(this.tempDirectory);
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0005FA04 File Offset: 0x0005EA04
		protected override bool DoCompile()
		{
			if (!this.isClosed && !this.isEngineCompiled)
			{
				this.SetUpCompilerEnvironment();
				if (this.PEFileName == null)
				{
					this.PEFileName = this.GenerateRandomPEFileName();
				}
				this.SaveSourceForDebugging();
				this.numberOfErrors = 0;
				this.isEngineCompiled = true;
				this.Globals.ScopeStack.Push(this.GetGlobalScope().GetObject());
				try
				{
					foreach (object obj in this.vsaItems)
					{
						if (obj is VsaReference)
						{
							((VsaReference)obj).Compile();
						}
					}
					if (this.vsaItems.Count > 0)
					{
						this.SetEnclosingContext(new WrappedNamespace("", this));
					}
					foreach (object obj2 in this.vsaItems)
					{
						if (obj2 is VsaHostObject)
						{
							((VsaHostObject)obj2).Compile();
						}
					}
					foreach (object obj3 in this.vsaItems)
					{
						if (obj3 is VsaStaticCode)
						{
							((VsaStaticCode)obj3).Parse();
						}
					}
					foreach (object obj4 in this.vsaItems)
					{
						if (obj4 is VsaStaticCode)
						{
							((VsaStaticCode)obj4).ProcessAssemblyAttributeLists();
						}
					}
					foreach (object obj5 in this.vsaItems)
					{
						if (obj5 is VsaStaticCode)
						{
							((VsaStaticCode)obj5).PartiallyEvaluate();
						}
					}
					foreach (object obj6 in this.vsaItems)
					{
						if (obj6 is VsaStaticCode)
						{
							((VsaStaticCode)obj6).TranslateToIL();
						}
					}
					foreach (object obj7 in this.vsaItems)
					{
						if (obj7 is VsaStaticCode)
						{
							((VsaStaticCode)obj7).GetCompiledType();
						}
					}
					if (this.globalScope != null)
					{
						this.globalScope.Compile();
					}
				}
				catch (JScriptException ex)
				{
					this.OnCompilerError(ex);
				}
				catch (FileLoadException ex2)
				{
					this.OnCompilerError(new JScriptException(JSError.ImplicitlyReferencedAssemblyNotFound)
					{
						value = ex2.FileName
					});
					this.isEngineCompiled = false;
				}
				catch (EndOfFile)
				{
				}
				catch
				{
					this.isEngineCompiled = false;
					throw;
				}
				finally
				{
					this.Globals.ScopeStack.Pop();
				}
				if (this.isEngineCompiled)
				{
					this.isEngineCompiled = this.numberOfErrors == 0 || this.alwaysGenerateIL;
				}
			}
			if (this.win32resource != null)
			{
				this.CompilerGlobals.assemblyBuilder.DefineUnmanagedResource(this.win32resource);
			}
			else if (this.compilerGlobals != null)
			{
				this.compilerGlobals.assemblyBuilder.DefineVersionInfoResource();
			}
			if (this.managedResources != null)
			{
				foreach (object obj8 in this.managedResources)
				{
					ResInfo resInfo = (ResInfo)obj8;
					if (resInfo.isLinked)
					{
						this.CompilerGlobals.assemblyBuilder.AddResourceFile(resInfo.name, Path.GetFileName(resInfo.filename), resInfo.isPublic ? ResourceAttributes.Public : ResourceAttributes.Private);
					}
					else
					{
						try
						{
							using (ResourceReader resourceReader = new ResourceReader(resInfo.filename))
							{
								IResourceWriter resourceWriter = this.CompilerGlobals.module.DefineResource(resInfo.name, resInfo.filename, resInfo.isPublic ? ResourceAttributes.Public : ResourceAttributes.Private);
								foreach (object obj9 in resourceReader)
								{
									DictionaryEntry dictionaryEntry = (DictionaryEntry)obj9;
									resourceWriter.AddResource((string)dictionaryEntry.Key, dictionaryEntry.Value);
								}
							}
						}
						catch (ArgumentException)
						{
							this.OnCompilerError(new JScriptException(JSError.InvalidResource)
							{
								value = resInfo.filename
							});
							this.isEngineCompiled = false;
							return false;
						}
					}
				}
			}
			if (this.isEngineCompiled)
			{
				this.EmitReferences();
			}
			if (this.isEngineCompiled)
			{
				if (this.doSaveAfterCompile)
				{
					if (this.PEFileKind != PEFileKinds.Dll)
					{
						this.CreateMain();
					}
					try
					{
						this.compilerGlobals.assemblyBuilder.Save(Path.GetFileName(this.PEFileName), this.PEKindFlags, this.PEMachineArchitecture);
						goto IL_0539;
					}
					catch (Exception ex3)
					{
						throw new VsaException(VsaError.SaveCompiledStateFailed, ex3.Message, ex3);
					}
					catch
					{
						throw new VsaException(VsaError.SaveCompiledStateFailed);
					}
				}
				if (this.genStartupClass)
				{
					this.CreateStartupClass();
				}
			}
			IL_0539:
			return this.isEngineCompiled;
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00060108 File Offset: 0x0005F108
		internal CultureInfo ErrorCultureInfo
		{
			get
			{
				if (this.errorCultureInfo == null || this.errorCultureInfo.LCID != this.errorLocale)
				{
					this.errorCultureInfo = new CultureInfo(this.errorLocale);
				}
				return this.errorCultureInfo;
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0006013C File Offset: 0x0005F13C
		private string GenerateRandomPEFileName()
		{
			if (this.randomNumberGenerator == null)
			{
				this.randomNumberGenerator = new RNGCryptoServiceProvider();
			}
			byte[] array = new byte[6];
			this.randomNumberGenerator.GetBytes(array);
			string text = Convert.ToBase64String(array);
			text = text.Replace('/', '-');
			text = text.Replace('+', '_');
			if (this.tempDirectory == null)
			{
				this.tempDirectory = Path.GetTempPath() + text;
			}
			string text2 = text + ((this.PEFileKind == PEFileKinds.Dll) ? ".dll" : ".exe");
			return this.tempDirectory + Path.DirectorySeparatorChar + text2;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000601D8 File Offset: 0x0005F1D8
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public Assembly GetAssembly()
		{
			base.TryObtainLock();
			Assembly assembly;
			try
			{
				if (this.PEFileName != null)
				{
					assembly = Assembly.LoadFrom(this.PEFileName);
				}
				else
				{
					assembly = this.compilerGlobals.assemblyBuilder;
				}
			}
			finally
			{
				base.ReleaseLock();
			}
			return assembly;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00060228 File Offset: 0x0005F228
		internal ClassScope GetClass(string className)
		{
			if (this.packages != null)
			{
				int i = 0;
				int count = this.packages.Count;
				while (i < count)
				{
					PackageScope packageScope = (PackageScope)this.packages[i];
					object memberValue = packageScope.GetMemberValue(className, 1);
					if (!(memberValue is Missing))
					{
						return (ClassScope)memberValue;
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00060284 File Offset: 0x0005F284
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public IVsaItem GetItem(string itemName)
		{
			return this.vsaItems[itemName];
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00060292 File Offset: 0x0005F292
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public IVsaItem GetItemAtIndex(int index)
		{
			return this.vsaItems[index];
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000602A0 File Offset: 0x0005F2A0
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public int GetItemCount()
		{
			return this.vsaItems.Count;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000602B0 File Offset: 0x0005F2B0
		public IVsaScriptScope GetGlobalScope()
		{
			if (this.globalScope == null)
			{
				this.globalScope = new VsaScriptScope(this, "Global", null);
				GlobalScope globalScope = (GlobalScope)this.globalScope.GetObject();
				globalScope.globalObject = this.Globals.globalObject;
				globalScope.fast = this.doFast;
				globalScope.isKnownAtCompileTime = this.doFast;
			}
			return this.globalScope;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00060318 File Offset: 0x0005F318
		public GlobalScope GetMainScope()
		{
			ScriptObject scriptObject = this.ScriptObjectStackTop();
			while (scriptObject != null && !(scriptObject is GlobalScope))
			{
				scriptObject = scriptObject.GetParent();
			}
			return (GlobalScope)scriptObject;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00060348 File Offset: 0x0005F348
		public Module GetModule()
		{
			if (this.PEFileName != null)
			{
				Assembly assembly = this.GetAssembly();
				Module[] modules = assembly.GetModules();
				return modules[0];
			}
			return this.CompilerGlobals.module;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0006037A File Offset: 0x0005F37A
		public ArrayConstructor GetOriginalArrayConstructor()
		{
			return this.Globals.globalObject.originalArray;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0006038C File Offset: 0x0005F38C
		public ObjectConstructor GetOriginalObjectConstructor()
		{
			return this.Globals.globalObject.originalObject;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0006039E File Offset: 0x0005F39E
		public RegExpConstructor GetOriginalRegExpConstructor()
		{
			return this.Globals.globalObject.originalRegExp;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000603B0 File Offset: 0x0005F3B0
		protected override object GetCustomOption(string name)
		{
			if (string.Compare(name, "CLSCompliant", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.isCLSCompliant;
			}
			if (string.Compare(name, "fast", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.doFast;
			}
			if (string.Compare(name, "output", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.PEFileName;
			}
			if (string.Compare(name, "PEFileKind", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.PEFileKind;
			}
			if (string.Compare(name, "PortableExecutableKind", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.PEKindFlags;
			}
			if (string.Compare(name, "ImageFileMachine", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.PEMachineArchitecture;
			}
			if (string.Compare(name, "ReferenceLoaderAPI", StringComparison.OrdinalIgnoreCase) == 0)
			{
				switch (this.ReferenceLoaderAPI)
				{
				case LoaderAPI.LoadFrom:
					return "LoadFrom";
				case LoaderAPI.LoadFile:
					return "LoadFile";
				case LoaderAPI.ReflectionOnlyLoadFrom:
					return "ReflectionOnlyLoadFrom";
				default:
					throw new VsaException(VsaError.OptionNotSupported);
				}
			}
			else
			{
				if (string.Compare(name, "print", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.doPrint;
				}
				if (string.Compare(name, "UseContextRelativeStatics", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.doCRS;
				}
				if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return null;
				}
				if (string.Compare(name, "define", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return null;
				}
				if (string.Compare(name, "defines", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.Defines;
				}
				if (string.Compare(name, "ee", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return VsaEngine.executeForJSEE;
				}
				if (string.Compare(name, "version", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.versionInfo;
				}
				if (string.Compare(name, "VersionSafe", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.versionSafe;
				}
				if (string.Compare(name, "warnaserror", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.doWarnAsError;
				}
				if (string.Compare(name, "WarningLevel", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.nWarningLevel;
				}
				if (string.Compare(name, "win32resource", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.win32resource;
				}
				if (string.Compare(name, "managedResources", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.managedResources;
				}
				if (string.Compare(name, "alwaysGenerateIL", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.alwaysGenerateIL;
				}
				if (string.Compare(name, "DebugDirectory", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.debugDirectory;
				}
				if (string.Compare(name, "AutoRef", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return this.autoRef;
				}
				throw new VsaException(VsaError.OptionNotSupported);
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x000605FC File Offset: 0x0005F5FC
		internal int GetStaticCodeBlockCount()
		{
			return ((VsaItems)this.vsaItems).staticCodeBlockCount;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00060610 File Offset: 0x0005F610
		internal Type GetType(string typeName)
		{
			if (this.cachedTypeLookups == null)
			{
				this.cachedTypeLookups = new SimpleHashtable(1000U);
			}
			object obj = this.cachedTypeLookups[typeName];
			if (obj != null)
			{
				return obj as Type;
			}
			int i = 0;
			int count = this.Scopes.Count;
			while (i < count)
			{
				GlobalScope globalScope = (GlobalScope)this.scopes[i];
				Type type = Globals.TypeRefs.ToReferenceContext(globalScope.GetType()).Assembly.GetType(typeName, false);
				if (type != null)
				{
					this.cachedTypeLookups[typeName] = type;
					return type;
				}
				i++;
			}
			if (this.runtimeAssembly != null)
			{
				this.AddReferences();
				this.runtimeAssembly = null;
			}
			int j = 0;
			int count2 = this.vsaItems.Count;
			while (j < count2)
			{
				object obj2 = this.vsaItems[j];
				if (obj2 is VsaReference)
				{
					Type type2 = ((VsaReference)obj2).GetType(typeName);
					if (type2 != null)
					{
						this.cachedTypeLookups[typeName] = type2;
						return type2;
					}
				}
				j++;
			}
			if (this.implicitAssemblies == null)
			{
				this.cachedTypeLookups[typeName] = false;
				return null;
			}
			int k = 0;
			int count3 = this.implicitAssemblies.Count;
			while (k < count3)
			{
				Assembly assembly = (Assembly)this.implicitAssemblies[k];
				Type type3 = assembly.GetType(typeName, false);
				if (type3 != null)
				{
					if (type3.IsPublic && !CustomAttribute.IsDefined(type3, typeof(RequiredAttributeAttribute), true))
					{
						this.cachedTypeLookups[typeName] = type3;
						return type3;
					}
				}
				k++;
			}
			this.cachedTypeLookups[typeName] = false;
			return null;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x000607BE File Offset: 0x0005F7BE
		internal Globals Globals
		{
			get
			{
				if (this.globals == null)
				{
					this.globals = new Globals(this.doFast, this);
				}
				return this.globals;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x000607E0 File Offset: 0x0005F7E0
		internal bool HasErrors
		{
			get
			{
				return this.numberOfErrors != 0;
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0006081C File Offset: 0x0005F81C
		private JSScanner GetScannerInstance(string name)
		{
			char[] array = new char[]
			{
				'\t', '\n', '\v', '\f', '\r', ' ', '\u00a0', '\u2000', '\u2001', '\u2002',
				'\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200a', '\u200b', '\u3000',
				'\ufeff'
			};
			if (name == null || name.IndexOfAny(array) > -1)
			{
				return null;
			}
			VsaItem vsaItem = new VsaStaticCode(this, "itemName", VsaItemFlag.None);
			Context context = new Context(new DocumentContext(vsaItem), name);
			context.errorReported = -1;
			JSScanner jsscanner = new JSScanner();
			jsscanner.SetSource(context);
			return jsscanner;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0006087A File Offset: 0x0005F87A
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void InitVsaEngine(string rootMoniker, IVsaSite site)
		{
			this.genStartupClass = false;
			this.engineMoniker = rootMoniker;
			this.engineSite = site;
			this.isEngineInitialized = true;
			this.rootNamespace = "JScript.DefaultNamespace";
			this.isEngineDirty = true;
			this.isEngineCompiled = false;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000608B1 File Offset: 0x0005F8B1
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void Interrupt()
		{
			if (this.runningThread != null)
			{
				this.runningThread.Abort();
				this.runningThread = null;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000608D0 File Offset: 0x0005F8D0
		protected override bool IsValidNamespaceName(string name)
		{
			JSScanner scannerInstance = this.GetScannerInstance(name);
			if (scannerInstance == null)
			{
				return false;
			}
			while (scannerInstance.PeekToken() == JSToken.Identifier)
			{
				scannerInstance.GetNextToken();
				if (scannerInstance.PeekToken() == JSToken.EndOfFile)
				{
					return true;
				}
				if (scannerInstance.PeekToken() != JSToken.AccessField)
				{
					return false;
				}
				scannerInstance.GetNextToken();
			}
			return false;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0006091C File Offset: 0x0005F91C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override bool IsValidIdentifier(string ident)
		{
			JSScanner scannerInstance = this.GetScannerInstance(ident);
			if (scannerInstance == null)
			{
				return false;
			}
			if (scannerInstance.PeekToken() != JSToken.Identifier)
			{
				return false;
			}
			scannerInstance.GetNextToken();
			return scannerInstance.PeekToken() == JSToken.EndOfFile;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x00060953 File Offset: 0x0005F953
		public LenientGlobalObject LenientGlobalObject
		{
			get
			{
				return (LenientGlobalObject)this.Globals.globalObject;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00060968 File Offset: 0x0005F968
		protected override Assembly LoadCompiledState()
		{
			if (!this.genDebugInfo)
			{
				Evidence compilationEvidence = this.CompilerGlobals.compilationEvidence;
				Evidence executionEvidence = this.executionEvidence;
				if ((compilationEvidence == null && executionEvidence == null) || (compilationEvidence != null && compilationEvidence.Equals(executionEvidence)))
				{
					return this.compilerGlobals.assemblyBuilder;
				}
			}
			byte[] array;
			byte[] array2;
			this.DoSaveCompiledState(out array, out array2);
			return Assembly.Load(array, array2, this.executionEvidence);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000609C8 File Offset: 0x0005F9C8
		protected override void DoLoadSourceState(IVsaPersistSite site)
		{
			string text = site.LoadElement(null);
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(text);
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (this.LoadProjectVersion(documentElement) == VsaEngine.CurrentProjectVersion)
				{
					this.LoadVsaEngineState(documentElement);
					this.isEngineDirty = false;
				}
			}
			catch (Exception ex)
			{
				throw new VsaException(VsaError.UnknownError, ex.ToString(), ex);
			}
			catch
			{
				throw new VsaException(VsaError.UnknownError);
			}
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00060A50 File Offset: 0x0005FA50
		private Version LoadProjectVersion(XmlElement root)
		{
			return new Version(root["ProjectVersion"].GetAttribute("Version"));
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00060A6C File Offset: 0x0005FA6C
		private void LoadVsaEngineState(XmlElement parent)
		{
			XmlElement xmlElement = parent["VsaEngine"];
			this.applicationPath = xmlElement.GetAttribute("ApplicationBase");
			this.genDebugInfo = bool.Parse(xmlElement.GetAttribute("GenerateDebugInfo"));
			this.scriptLanguage = xmlElement.GetAttribute("Language");
			base.LCID = int.Parse(xmlElement.GetAttribute("LCID"), CultureInfo.InvariantCulture);
			base.Name = xmlElement.GetAttribute("Name");
			this.rootNamespace = xmlElement.GetAttribute("RootNamespace");
			this.assemblyVersion = xmlElement.GetAttribute("Version");
			this.LoadCustomOptions(xmlElement);
			this.LoadVsaItems(xmlElement);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00060B1C File Offset: 0x0005FB1C
		private void LoadCustomOptions(XmlElement parent)
		{
			XmlElement xmlElement = parent["Options"];
			this.doFast = bool.Parse(xmlElement.GetAttribute("fast"));
			this.doPrint = bool.Parse(xmlElement.GetAttribute("print"));
			this.doCRS = bool.Parse(xmlElement.GetAttribute("UseContextRelativeStatics"));
			this.versionSafe = bool.Parse(xmlElement.GetAttribute("VersionSafe"));
			this.libpath = xmlElement.GetAttribute("libpath");
			this.doWarnAsError = bool.Parse(xmlElement.GetAttribute("warnaserror"));
			this.nWarningLevel = int.Parse(xmlElement.GetAttribute("WarningLevel"), CultureInfo.InvariantCulture);
			if (xmlElement.HasAttribute("win32resource"))
			{
				this.win32resource = xmlElement.GetAttribute("win32resource");
			}
			this.LoadUserDefines(xmlElement);
			this.LoadManagedResources(xmlElement);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00060BFC File Offset: 0x0005FBFC
		private void LoadUserDefines(XmlElement parent)
		{
			XmlElement xmlElement = parent["Defines"];
			XmlNodeList childNodes = xmlElement.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlElement xmlElement2 = (XmlElement)obj;
				this.Defines[xmlElement2.Name] = xmlElement2.GetAttribute("Value");
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00060C7C File Offset: 0x0005FC7C
		private void LoadManagedResources(XmlElement parent)
		{
			XmlElement xmlElement = parent["ManagedResources"];
			XmlNodeList childNodes = xmlElement.ChildNodes;
			if (childNodes.Count > 0)
			{
				this.managedResources = new ArrayList(childNodes.Count);
				foreach (object obj in childNodes)
				{
					XmlElement xmlElement2 = (XmlElement)obj;
					string attribute = xmlElement2.GetAttribute("Name");
					string attribute2 = xmlElement2.GetAttribute("FileName");
					bool flag = bool.Parse(xmlElement2.GetAttribute("Public"));
					bool flag2 = bool.Parse(xmlElement2.GetAttribute("Linked"));
					((ArrayList)this.managedResources).Add(new ResInfo(attribute2, attribute, flag, flag2));
				}
			}
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00060D5C File Offset: 0x0005FD5C
		private void LoadVsaItems(XmlElement parent)
		{
			XmlNodeList childNodes = parent["VsaItems"].ChildNodes;
			string text = VsaItemType.Reference.ToString();
			string text2 = VsaItemType.AppGlobal.ToString();
			string text3 = VsaItemType.Code.ToString();
			foreach (object obj in childNodes)
			{
				XmlElement xmlElement = (XmlElement)obj;
				string attribute = xmlElement.GetAttribute("Name");
				string attribute2 = xmlElement.GetAttribute("ItemType");
				IVsaItem vsaItem;
				if (string.Compare(attribute2, text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					vsaItem = this.vsaItems.CreateItem(attribute, VsaItemType.Reference, VsaItemFlag.None);
					((IVsaReferenceItem)vsaItem).AssemblyName = xmlElement.GetAttribute("AssemblyName");
				}
				else if (string.Compare(attribute2, text2, StringComparison.OrdinalIgnoreCase) == 0)
				{
					vsaItem = this.vsaItems.CreateItem(attribute, VsaItemType.AppGlobal, VsaItemFlag.None);
					((IVsaGlobalItem)vsaItem).ExposeMembers = bool.Parse(xmlElement.GetAttribute("ExposeMembers"));
					((IVsaGlobalItem)vsaItem).TypeString = xmlElement.GetAttribute("TypeString");
				}
				else
				{
					if (string.Compare(attribute2, text3, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new VsaException(VsaError.LoadElementFailed);
					}
					vsaItem = this.vsaItems.CreateItem(attribute, VsaItemType.Code, VsaItemFlag.None);
					XmlCDataSection xmlCDataSection = (XmlCDataSection)xmlElement.FirstChild;
					string text4 = xmlCDataSection.Value.Replace(" >", ">");
					((IVsaCodeItem)vsaItem).SourceText = text4;
				}
				XmlNodeList childNodes2 = xmlElement["Options"].ChildNodes;
				foreach (object obj2 in childNodes2)
				{
					XmlElement xmlElement2 = (XmlElement)obj2;
					vsaItem.SetOption(xmlElement2.Name, xmlElement2.GetAttribute("Value"));
				}
				((VsaItem)vsaItem).IsDirty = false;
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00060F80 File Offset: 0x0005FF80
		internal bool OnCompilerError(JScriptException se)
		{
			if (se.Severity == 0 || (this.doWarnAsError && se.Severity <= this.nWarningLevel))
			{
				this.numberOfErrors++;
			}
			bool flag = this.engineSite.OnCompilerError(se);
			if (!flag)
			{
				this.isEngineCompiled = false;
			}
			return flag;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00060FD1 File Offset: 0x0005FFD1
		public ScriptObject PopScriptObject()
		{
			return (ScriptObject)this.Globals.ScopeStack.Pop();
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00060FE8 File Offset: 0x0005FFE8
		public void PushScriptObject(ScriptObject obj)
		{
			this.Globals.ScopeStack.Push(obj);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00060FFB File Offset: 0x0005FFFB
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void RegisterEventSource(string name)
		{
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00060FFD File Offset: 0x0005FFFD
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override void Reset()
		{
			if (this.genStartupClass)
			{
				base.Reset();
				return;
			}
			this.ResetCompiledState();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00061014 File Offset: 0x00060014
		protected override void ResetCompiledState()
		{
			if (this.globalScope != null)
			{
				this.globalScope.Reset();
				this.globalScope = null;
			}
			this.classCounter = 0;
			this.haveCompiledState = false;
			this.failedCompilation = true;
			this.compiledRootNamespace = null;
			this.startupClass = null;
			this.compilerGlobals = null;
			this.globals = null;
			foreach (object obj in this.vsaItems)
			{
				((VsaItem)obj).Reset();
			}
			this.implicitAssemblies = null;
			this.implicitAssemblyCache = null;
			this.cachedTypeLookups = null;
			this.isEngineCompiled = false;
			this.isEngineRunning = false;
			this.isCompilerSet = false;
			this.packages = null;
			if (!this.doSaveAfterCompile)
			{
				this.PEFileName = null;
			}
			this.rawPE = null;
			this.rawPDB = null;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00061108 File Offset: 0x00060108
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void Restart()
		{
			base.TryObtainLock();
			try
			{
				((VsaItems)this.vsaItems).Close();
				if (this.globalScope != null)
				{
					this.globalScope.Close();
				}
				this.globalScope = null;
				this.vsaItems = new VsaItems(this);
				this.isEngineRunning = false;
				this.isEngineCompiled = false;
				this.isCompilerSet = false;
				this.isClosed = false;
				this.runningThread = null;
				this.globals = null;
			}
			finally
			{
				base.ReleaseLock();
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00061194 File Offset: 0x00060194
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void RunEmpty()
		{
			base.TryObtainLock();
			try
			{
				base.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.SiteSet);
				this.isEngineRunning = true;
				this.runningThread = Thread.CurrentThread;
				if (this.globalScope != null)
				{
					this.globalScope.Run();
				}
				foreach (object obj in this.vsaItems)
				{
					((VsaItem)obj).Run();
				}
			}
			finally
			{
				this.runningThread = null;
				base.ReleaseLock();
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00061240 File Offset: 0x00060240
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public void Run(AppDomain domain)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00061248 File Offset: 0x00060248
		protected override void DoSaveCompiledState(out byte[] pe, out byte[] pdb)
		{
			pe = null;
			pdb = null;
			if (this.rawPE == null)
			{
				try
				{
					if (!Directory.Exists(this.tempDirectory))
					{
						Directory.CreateDirectory(this.tempDirectory);
					}
					this.compilerGlobals.assemblyBuilder.Save(Path.GetFileName(this.PEFileName), this.PEKindFlags, this.PEMachineArchitecture);
					string text = Path.ChangeExtension(this.PEFileName, ".pdb");
					try
					{
						FileStream fileStream = new FileStream(this.PEFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
						try
						{
							this.rawPE = new byte[(int)fileStream.Length];
							fileStream.Read(this.rawPE, 0, this.rawPE.Length);
						}
						finally
						{
							fileStream.Close();
						}
						if (File.Exists(text))
						{
							fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
							try
							{
								this.rawPDB = new byte[(int)fileStream.Length];
								fileStream.Read(this.rawPDB, 0, this.rawPDB.Length);
							}
							finally
							{
								fileStream.Close();
							}
						}
					}
					finally
					{
						File.Delete(this.PEFileName);
						if (File.Exists(text))
						{
							File.Delete(text);
						}
					}
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.SaveCompiledStateFailed, ex.ToString(), ex);
				}
				catch
				{
					throw new VsaException(VsaError.SaveCompiledStateFailed);
				}
			}
			pe = this.rawPE;
			pdb = this.rawPDB;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000613C8 File Offset: 0x000603C8
		protected override void DoSaveSourceState(IVsaPersistSite site)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<project></project>");
			XmlElement documentElement = xmlDocument.DocumentElement;
			this.SaveProjectVersion(xmlDocument, documentElement);
			this.SaveVsaEngineState(xmlDocument, documentElement);
			site.SaveElement(null, xmlDocument.OuterXml);
			this.SaveSourceForDebugging();
			this.isEngineDirty = false;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00061418 File Offset: 0x00060418
		private void SaveSourceForDebugging()
		{
			if (!base.GenerateDebugInfo || this.debugDirectory == null || !this.isEngineDirty)
			{
				return;
			}
			foreach (object obj in this.vsaItems)
			{
				VsaItem vsaItem = (VsaItem)obj;
				if (vsaItem is VsaStaticCode)
				{
					string text = this.debugDirectory + vsaItem.Name + ".js";
					try
					{
						using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write))
						{
							using (StreamWriter streamWriter = new StreamWriter(fileStream))
							{
								streamWriter.Write(((VsaStaticCode)vsaItem).SourceText);
							}
							vsaItem.SetOption("codebase", text);
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00061518 File Offset: 0x00060518
		private void SaveProjectVersion(XmlDocument project, XmlElement root)
		{
			XmlElement xmlElement = project.CreateElement("ProjectVersion");
			this.CreateAttribute(project, xmlElement, "Version", VsaEngine.CurrentProjectVersion.ToString());
			root.AppendChild(xmlElement);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00061550 File Offset: 0x00060550
		private void SaveVsaEngineState(XmlDocument project, XmlElement parent)
		{
			XmlElement xmlElement = project.CreateElement("VsaEngine");
			this.CreateAttribute(project, xmlElement, "ApplicationBase", this.applicationPath);
			this.CreateAttribute(project, xmlElement, "GenerateDebugInfo", this.genDebugInfo.ToString());
			this.CreateAttribute(project, xmlElement, "Language", this.scriptLanguage);
			this.CreateAttribute(project, xmlElement, "LCID", this.errorLocale.ToString(CultureInfo.InvariantCulture));
			this.CreateAttribute(project, xmlElement, "Name", this.engineName);
			this.CreateAttribute(project, xmlElement, "RootNamespace", this.rootNamespace);
			this.CreateAttribute(project, xmlElement, "Version", this.assemblyVersion);
			this.SaveCustomOptions(project, xmlElement);
			this.SaveVsaItems(project, xmlElement);
			parent.AppendChild(xmlElement);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00061618 File Offset: 0x00060618
		private void SaveCustomOptions(XmlDocument project, XmlElement parent)
		{
			XmlElement xmlElement = project.CreateElement("Options");
			this.CreateAttribute(project, xmlElement, "fast", this.doFast.ToString());
			this.CreateAttribute(project, xmlElement, "print", this.doPrint.ToString());
			this.CreateAttribute(project, xmlElement, "UseContextRelativeStatics", this.doCRS.ToString());
			this.CreateAttribute(project, xmlElement, "VersionSafe", this.versionSafe.ToString());
			this.CreateAttribute(project, xmlElement, "libpath", this.libpath);
			this.CreateAttribute(project, xmlElement, "warnaserror", this.doWarnAsError.ToString());
			this.CreateAttribute(project, xmlElement, "WarningLevel", this.nWarningLevel.ToString(CultureInfo.InvariantCulture));
			if (this.win32resource != null)
			{
				this.CreateAttribute(project, xmlElement, "win32resource", this.win32resource);
			}
			this.SaveUserDefines(project, xmlElement);
			this.SaveManagedResources(project, xmlElement);
			parent.AppendChild(xmlElement);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0006170C File Offset: 0x0006070C
		private void SaveUserDefines(XmlDocument project, XmlElement parent)
		{
			XmlElement xmlElement = project.CreateElement("Defines");
			if (this.Defines != null)
			{
				foreach (object obj in this.Defines.Keys)
				{
					string text = (string)obj;
					this.AddChildAndValue(project, xmlElement, text, (string)this.Defines[text]);
				}
			}
			parent.AppendChild(xmlElement);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0006179C File Offset: 0x0006079C
		private void SaveManagedResources(XmlDocument project, XmlElement parent)
		{
			XmlElement xmlElement = project.CreateElement("ManagedResources");
			if (this.managedResources != null)
			{
				foreach (object obj in this.managedResources)
				{
					ResInfo resInfo = (ResInfo)obj;
					XmlElement xmlElement2 = project.CreateElement(resInfo.name);
					this.CreateAttribute(project, xmlElement2, "File", resInfo.filename);
					this.CreateAttribute(project, xmlElement2, "Public", resInfo.isPublic.ToString());
					this.CreateAttribute(project, xmlElement2, "Linked", resInfo.isLinked.ToString());
					xmlElement.AppendChild(xmlElement2);
				}
			}
			parent.AppendChild(xmlElement);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00061868 File Offset: 0x00060868
		private void SaveVsaItems(XmlDocument project, XmlElement parent)
		{
			XmlElement xmlElement = project.CreateElement("VsaItems");
			foreach (object obj in this.vsaItems)
			{
				IVsaItem vsaItem = (IVsaItem)obj;
				XmlElement xmlElement2 = project.CreateElement("IVsaItem");
				this.CreateAttribute(project, xmlElement2, "Name", vsaItem.Name);
				this.CreateAttribute(project, xmlElement2, "ItemType", vsaItem.ItemType.ToString(CultureInfo.InvariantCulture));
				XmlElement xmlElement3 = project.CreateElement("Options");
				if (vsaItem is VsaHostObject)
				{
					this.CreateAttribute(project, xmlElement2, "TypeString", ((VsaHostObject)vsaItem).TypeString);
					this.CreateAttribute(project, xmlElement2, "ExposeMembers", ((VsaHostObject)vsaItem).ExposeMembers.ToString(CultureInfo.InvariantCulture));
				}
				else if (vsaItem is VsaReference)
				{
					this.CreateAttribute(project, xmlElement2, "AssemblyName", ((VsaReference)vsaItem).AssemblyName);
				}
				else
				{
					if (!(vsaItem is VsaStaticCode))
					{
						throw new VsaException(VsaError.SaveElementFailed);
					}
					string text = ((VsaStaticCode)vsaItem).SourceText.Replace(">", " >");
					XmlCDataSection xmlCDataSection = project.CreateCDataSection(text);
					xmlElement2.AppendChild(xmlCDataSection);
					string text2 = (string)vsaItem.GetOption("codebase");
					if (text2 != null)
					{
						this.AddChildAndValue(project, xmlElement3, "codebase", text2);
					}
				}
				((VsaItem)vsaItem).IsDirty = false;
				xmlElement2.AppendChild(xmlElement3);
				xmlElement.AppendChild(xmlElement2);
			}
			parent.AppendChild(xmlElement);
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00061A28 File Offset: 0x00060A28
		internal ArrayList Scopes
		{
			get
			{
				if (this.scopes == null)
				{
					this.scopes = new ArrayList(8);
				}
				return this.scopes;
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00061A44 File Offset: 0x00060A44
		public ScriptObject ScriptObjectStackTop()
		{
			return this.Globals.ScopeStack.Peek();
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00061A58 File Offset: 0x00060A58
		internal void SetEnclosingContext(ScriptObject ob)
		{
			ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
			while (scriptObject.GetParent() != null)
			{
				scriptObject = scriptObject.GetParent();
			}
			scriptObject.SetParent(ob);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00061A90 File Offset: 0x00060A90
		public void SetOutputStream(IMessageReceiver output)
		{
			COMCharStream comcharStream = new COMCharStream(output);
			StreamWriter streamWriter = new StreamWriter(comcharStream, Encoding.Default);
			streamWriter.AutoFlush = true;
			ScriptStream.Out = streamWriter;
			ScriptStream.Error = streamWriter;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00061AC4 File Offset: 0x00060AC4
		protected override void SetCustomOption(string name, object value)
		{
			try
			{
				if (string.Compare(name, "CLSCompliant", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.isCLSCompliant = (bool)value;
				}
				else if (string.Compare(name, "fast", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.doFast = (bool)value;
				}
				else if (string.Compare(name, "output", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (value is string)
					{
						this.PEFileName = (string)value;
						this.doSaveAfterCompile = true;
					}
				}
				else if (string.Compare(name, "PEFileKind", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.PEFileKind = (PEFileKinds)value;
				}
				else if (string.Compare(name, "PortableExecutableKind", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.PEKindFlags = (PortableExecutableKinds)value;
				}
				else if (string.Compare(name, "ImageFileMachine", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.PEMachineArchitecture = (ImageFileMachine)value;
				}
				else if (string.Compare(name, "ReferenceLoaderAPI", StringComparison.OrdinalIgnoreCase) == 0)
				{
					string text = (string)value;
					if (string.Compare(text, "LoadFrom", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.ReferenceLoaderAPI = LoaderAPI.LoadFrom;
					}
					else if (string.Compare(text, "LoadFile", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.ReferenceLoaderAPI = LoaderAPI.LoadFile;
					}
					else
					{
						if (string.Compare(text, "ReflectionOnlyLoadFrom", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new VsaException(VsaError.OptionInvalid);
						}
						this.ReferenceLoaderAPI = LoaderAPI.ReflectionOnlyLoadFrom;
					}
				}
				else if (string.Compare(name, "print", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.doPrint = (bool)value;
				}
				else if (string.Compare(name, "UseContextRelativeStatics", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.doCRS = (bool)value;
				}
				else if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (string.Compare(name, "define", StringComparison.OrdinalIgnoreCase) != 0)
					{
						if (string.Compare(name, "defines", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.Defines = (Hashtable)value;
						}
						else if (string.Compare(name, "ee", StringComparison.OrdinalIgnoreCase) == 0)
						{
							VsaEngine.executeForJSEE = (bool)value;
						}
						else if (string.Compare(name, "version", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.versionInfo = (Version)value;
						}
						else if (string.Compare(name, "VersionSafe", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.versionSafe = (bool)value;
						}
						else if (string.Compare(name, "libpath", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.libpath = (string)value;
						}
						else if (string.Compare(name, "warnaserror", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.doWarnAsError = (bool)value;
						}
						else if (string.Compare(name, "WarningLevel", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.nWarningLevel = (int)value;
						}
						else if (string.Compare(name, "win32resource", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.win32resource = (string)value;
						}
						else if (string.Compare(name, "managedResources", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.managedResources = (ICollection)value;
						}
						else if (string.Compare(name, "alwaysGenerateIL", StringComparison.OrdinalIgnoreCase) == 0)
						{
							this.alwaysGenerateIL = (bool)value;
						}
						else if (string.Compare(name, "DebugDirectory", StringComparison.OrdinalIgnoreCase) == 0)
						{
							if (value == null)
							{
								this.debugDirectory = null;
							}
							else
							{
								string text2 = value as string;
								if (text2 == null)
								{
									throw new VsaException(VsaError.OptionInvalid);
								}
								try
								{
									text2 = Path.GetFullPath(text2 + Path.DirectorySeparatorChar);
									if (!Directory.Exists(text2))
									{
										Directory.CreateDirectory(text2);
									}
								}
								catch (Exception ex)
								{
									throw new VsaException(VsaError.OptionInvalid, "", ex);
								}
								catch
								{
									throw new VsaException(VsaError.OptionInvalid);
								}
								this.debugDirectory = text2;
							}
						}
						else
						{
							if (string.Compare(name, "AutoRef", StringComparison.OrdinalIgnoreCase) != 0)
							{
								throw new VsaException(VsaError.OptionNotSupported);
							}
							this.autoRef = (bool)value;
						}
					}
				}
			}
			catch (VsaException)
			{
				throw;
			}
			catch
			{
				throw new VsaException(VsaError.OptionInvalid);
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00061EC0 File Offset: 0x00060EC0
		internal void SetUpCompilerEnvironment()
		{
			if (!this.isCompilerSet)
			{
				Globals.TypeRefs = this.TypeRefs;
				this.globals = this.Globals;
				this.isCompilerSet = true;
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00061EE8 File Offset: 0x00060EE8
		internal void TryToAddImplicitAssemblyReference(string name)
		{
			if (!this.autoRef)
			{
				return;
			}
			string text;
			if (this.implicitAssemblyCache == null)
			{
				this.implicitAssemblyCache = new SimpleHashtable(50U);
				this.implicitAssemblyCache[Path.GetFileNameWithoutExtension(this.PEFileName).ToLowerInvariant()] = true;
				foreach (object obj in this.vsaItems)
				{
					VsaReference vsaReference = obj as VsaReference;
					if (vsaReference != null && vsaReference.AssemblyName != null)
					{
						text = Path.GetFileName(vsaReference.AssemblyName).ToLowerInvariant();
						if (text.EndsWith(".dll", StringComparison.Ordinal))
						{
							text = text.Substring(0, text.Length - 4);
						}
						this.implicitAssemblyCache[text] = true;
					}
				}
				this.implicitAssemblyCache = this.implicitAssemblyCache;
			}
			text = name.ToLowerInvariant();
			if (this.implicitAssemblyCache[text] != null)
			{
				return;
			}
			this.implicitAssemblyCache[text] = true;
			try
			{
				VsaReference vsaReference2 = new VsaReference(this, name + ".dll");
				if (vsaReference2.Compile(false))
				{
					ArrayList arrayList = this.implicitAssemblies;
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						this.implicitAssemblies = arrayList;
					}
					arrayList.Add(vsaReference2.Assembly);
				}
			}
			catch (VsaException)
			{
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0006205C File Offset: 0x0006105C
		internal string RuntimeDirectory
		{
			get
			{
				if (this.runtimeDirectory == null)
				{
					string fullyQualifiedName = typeof(object).Module.FullyQualifiedName;
					this.runtimeDirectory = Path.GetDirectoryName(fullyQualifiedName);
				}
				return this.runtimeDirectory;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00062098 File Offset: 0x00061098
		internal string[] LibpathList
		{
			get
			{
				if (this.libpathList == null)
				{
					if (this.libpath == null)
					{
						this.libpathList = new string[] { typeof(object).Module.Assembly.Location };
					}
					else
					{
						this.libpathList = this.libpath.Split(new char[] { Path.PathSeparator });
					}
				}
				return this.libpathList;
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00062108 File Offset: 0x00061108
		internal string FindAssembly(string name)
		{
			string text = name;
			if (Path.GetFileName(name) == name)
			{
				if (File.Exists(name))
				{
					text = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + name;
				}
				else
				{
					string text2 = this.RuntimeDirectory + Path.DirectorySeparatorChar + name;
					if (File.Exists(text2))
					{
						text = text2;
					}
					else
					{
						string[] array = this.LibpathList;
						foreach (string text3 in array)
						{
							if (text3.Length > 0)
							{
								text2 = text3 + Path.DirectorySeparatorChar + name;
								if (File.Exists(text2))
								{
									text = text2;
									break;
								}
							}
						}
					}
				}
			}
			if (!File.Exists(text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000621C0 File Offset: 0x000611C0
		protected override void ValidateRootMoniker(string rootMoniker)
		{
			if (this.genStartupClass)
			{
				base.ValidateRootMoniker(rootMoniker);
				return;
			}
			if (rootMoniker == null || rootMoniker.Length == 0)
			{
				throw new VsaException(VsaError.RootMonikerInvalid);
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x000621E8 File Offset: 0x000611E8
		internal static bool CheckIdentifierForCLSCompliance(string name)
		{
			if (name[0] == '_')
			{
				return false;
			}
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == '$')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00062224 File Offset: 0x00061224
		internal void CheckTypeNameForCLSCompliance(string name, string fullname, Context context)
		{
			if (!this.isCLSCompliant)
			{
				return;
			}
			if (name[0] == '_')
			{
				context.HandleError(JSError.NonCLSCompliantType);
				return;
			}
			if (!VsaEngine.CheckIdentifierForCLSCompliance(fullname))
			{
				context.HandleError(JSError.NonCLSCompliantType);
				return;
			}
			if (this.typenameTable == null)
			{
				this.typenameTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			if (this.typenameTable[fullname] == null)
			{
				this.typenameTable[fullname] = fullname;
				return;
			}
			context.HandleError(JSError.NonCLSCompliantType);
		}

		// Token: 0x04000794 RID: 1940
		internal bool alwaysGenerateIL;

		// Token: 0x04000795 RID: 1941
		private bool autoRef;

		// Token: 0x04000796 RID: 1942
		private Hashtable Defines;

		// Token: 0x04000797 RID: 1943
		internal bool doCRS;

		// Token: 0x04000798 RID: 1944
		internal bool doFast;

		// Token: 0x04000799 RID: 1945
		internal bool doPrint;

		// Token: 0x0400079A RID: 1946
		internal bool doSaveAfterCompile;

		// Token: 0x0400079B RID: 1947
		private bool doWarnAsError;

		// Token: 0x0400079C RID: 1948
		private int nWarningLevel;

		// Token: 0x0400079D RID: 1949
		internal bool genStartupClass;

		// Token: 0x0400079E RID: 1950
		internal bool isCLSCompliant;

		// Token: 0x0400079F RID: 1951
		internal bool versionSafe;

		// Token: 0x040007A0 RID: 1952
		private string PEFileName;

		// Token: 0x040007A1 RID: 1953
		internal PEFileKinds PEFileKind;

		// Token: 0x040007A2 RID: 1954
		internal PortableExecutableKinds PEKindFlags;

		// Token: 0x040007A3 RID: 1955
		internal ImageFileMachine PEMachineArchitecture;

		// Token: 0x040007A4 RID: 1956
		internal LoaderAPI ReferenceLoaderAPI;

		// Token: 0x040007A5 RID: 1957
		private Version versionInfo;

		// Token: 0x040007A6 RID: 1958
		private CultureInfo errorCultureInfo;

		// Token: 0x040007A7 RID: 1959
		internal static bool executeForJSEE = false;

		// Token: 0x040007A8 RID: 1960
		private string libpath;

		// Token: 0x040007A9 RID: 1961
		private string[] libpathList;

		// Token: 0x040007AA RID: 1962
		private bool isCompilerSet;

		// Token: 0x040007AB RID: 1963
		internal VsaScriptScope globalScope;

		// Token: 0x040007AC RID: 1964
		private ArrayList packages;

		// Token: 0x040007AD RID: 1965
		private ArrayList scopes;

		// Token: 0x040007AE RID: 1966
		private ArrayList implicitAssemblies;

		// Token: 0x040007AF RID: 1967
		private SimpleHashtable implicitAssemblyCache;

		// Token: 0x040007B0 RID: 1968
		private string win32resource;

		// Token: 0x040007B1 RID: 1969
		private ICollection managedResources;

		// Token: 0x040007B2 RID: 1970
		private string debugDirectory;

		// Token: 0x040007B3 RID: 1971
		private string tempDirectory;

		// Token: 0x040007B4 RID: 1972
		private RNGCryptoServiceProvider randomNumberGenerator;

		// Token: 0x040007B5 RID: 1973
		private byte[] rawPE;

		// Token: 0x040007B6 RID: 1974
		private byte[] rawPDB;

		// Token: 0x040007B7 RID: 1975
		internal int classCounter;

		// Token: 0x040007B8 RID: 1976
		private SimpleHashtable cachedTypeLookups;

		// Token: 0x040007B9 RID: 1977
		internal Thread runningThread;

		// Token: 0x040007BA RID: 1978
		private CompilerGlobals compilerGlobals;

		// Token: 0x040007BB RID: 1979
		private Globals globals;

		// Token: 0x040007BC RID: 1980
		private int numberOfErrors;

		// Token: 0x040007BD RID: 1981
		private string runtimeDirectory;

		// Token: 0x040007BE RID: 1982
		private static readonly Version CurrentProjectVersion = new Version("1.0");

		// Token: 0x040007BF RID: 1983
		private Hashtable typenameTable;

		// Token: 0x040007C0 RID: 1984
		private static string engineVersion = VsaEngine.GetVersionString();

		// Token: 0x040007C1 RID: 1985
		private Assembly runtimeAssembly;

		// Token: 0x040007C2 RID: 1986
		private static Hashtable assemblyReferencesTable = null;

		// Token: 0x040007C3 RID: 1987
		private static Module reflectionOnlyVsaModule = null;

		// Token: 0x040007C4 RID: 1988
		private static Module reflectionOnlyJScriptModule = null;

		// Token: 0x040007C5 RID: 1989
		private static TypeReferences _reflectionOnlyTypeRefs;

		// Token: 0x040007C6 RID: 1990
		private static volatile VsaEngine exeEngine;
	}
}
