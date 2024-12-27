using System;
using System.Collections;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.TCEAdapterGen;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051E RID: 1310
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("F1C3BF79-C3E4-11d3-88E7-00902754C43A")]
	[ComVisible(true)]
	public sealed class TypeLibConverter : ITypeLibConverter
	{
		// Token: 0x060032CF RID: 13007 RVA: 0x000ACF94 File Offset: 0x000ABF94
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, int flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, bool unsafeInterfaces)
		{
			return this.ConvertTypeLibToAssembly(typeLib, asmFileName, unsafeInterfaces ? TypeLibImporterFlags.UnsafeInterfaces : TypeLibImporterFlags.None, notifySink, publicKey, keyPair, null, null);
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000ACFBC File Offset: 0x000ABFBC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, TypeLibImporterFlags flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, string asmNamespace, Version asmVersion)
		{
			ArrayList arrayList = null;
			if (typeLib == null)
			{
				throw new ArgumentNullException("typeLib");
			}
			if (asmFileName == null)
			{
				throw new ArgumentNullException("asmFileName");
			}
			if (notifySink == null)
			{
				throw new ArgumentNullException("notifySink");
			}
			if (string.Empty.Equals(asmFileName))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileName"), "asmFileName");
			}
			if (asmFileName.Length > 260)
			{
				throw new ArgumentException(Environment.GetResourceString("IO.PathTooLong"), asmFileName);
			}
			if ((flags & TypeLibImporterFlags.PrimaryInteropAssembly) != TypeLibImporterFlags.None && publicKey == null && keyPair == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_PIAMustBeStrongNamed"));
			}
			AssemblyNameFlags assemblyNameFlags = AssemblyNameFlags.None;
			AssemblyName assemblyNameFromTypelib = TypeLibConverter.GetAssemblyNameFromTypelib(typeLib, asmFileName, publicKey, keyPair, asmVersion, assemblyNameFlags);
			AssemblyBuilder assemblyBuilder = TypeLibConverter.CreateAssemblyForTypeLib(typeLib, asmFileName, assemblyNameFromTypelib, (flags & TypeLibImporterFlags.PrimaryInteropAssembly) != TypeLibImporterFlags.None, (flags & TypeLibImporterFlags.ReflectionOnlyLoading) != TypeLibImporterFlags.None);
			string fileName = Path.GetFileName(asmFileName);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(fileName, fileName);
			if (asmNamespace == null)
			{
				asmNamespace = assemblyNameFromTypelib.Name;
			}
			TypeLibConverter.TypeResolveHandler typeResolveHandler = new TypeLibConverter.TypeResolveHandler(moduleBuilder, notifySink);
			AppDomain domain = Thread.GetDomain();
			ResolveEventHandler resolveEventHandler = new ResolveEventHandler(typeResolveHandler.ResolveEvent);
			ResolveEventHandler resolveEventHandler2 = new ResolveEventHandler(typeResolveHandler.ResolveAsmEvent);
			ResolveEventHandler resolveEventHandler3 = new ResolveEventHandler(typeResolveHandler.ResolveROAsmEvent);
			domain.TypeResolve += resolveEventHandler;
			domain.AssemblyResolve += resolveEventHandler2;
			domain.ReflectionOnlyAssemblyResolve += resolveEventHandler3;
			TypeLibConverter.nConvertTypeLibToMetadata(typeLib, assemblyBuilder.InternalAssembly, moduleBuilder.InternalModule, asmNamespace, flags, typeResolveHandler, out arrayList);
			TypeLibConverter.UpdateComTypesInAssembly(assemblyBuilder, moduleBuilder);
			if (arrayList.Count > 0)
			{
				new TCEAdapterGenerator().Process(moduleBuilder, arrayList);
			}
			domain.TypeResolve -= resolveEventHandler;
			domain.AssemblyResolve -= resolveEventHandler2;
			domain.ReflectionOnlyAssemblyResolve -= resolveEventHandler3;
			return assemblyBuilder;
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000AD151 File Offset: 0x000AC151
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public object ConvertAssemblyToTypeLib(Assembly assembly, string strTypeLibName, TypeLibExporterFlags flags, ITypeLibExporterNotifySink notifySink)
		{
			return TypeLibConverter.nConvertAssemblyToTypeLib((assembly == null) ? null : assembly.InternalAssembly, strTypeLibName, flags, notifySink);
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000AD168 File Offset: 0x000AC168
		public bool GetPrimaryInteropAssembly(Guid g, int major, int minor, int lcid, out string asmName, out string asmCodeBase)
		{
			string text = "{" + g.ToString().ToUpper(CultureInfo.InvariantCulture) + "}";
			string text2 = major.ToString("x", CultureInfo.InvariantCulture) + "." + minor.ToString("x", CultureInfo.InvariantCulture);
			asmName = null;
			asmCodeBase = null;
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("TypeLib", false))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(text))
					{
						if (registryKey2 != null)
						{
							using (RegistryKey registryKey3 = registryKey2.OpenSubKey(text2, false))
							{
								if (registryKey3 != null)
								{
									asmName = (string)registryKey3.GetValue("PrimaryInteropAssemblyName");
									asmCodeBase = (string)registryKey3.GetValue("PrimaryInteropAssemblyCodeBase");
								}
							}
						}
					}
				}
			}
			return asmName != null;
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000AD280 File Offset: 0x000AC280
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static AssemblyBuilder CreateAssemblyForTypeLib(object typeLib, string asmFileName, AssemblyName asmName, bool bPrimaryInteropAssembly, bool bReflectionOnly)
		{
			AppDomain domain = Thread.GetDomain();
			string text = null;
			if (asmFileName != null)
			{
				text = Path.GetDirectoryName(asmFileName);
				if (string.Empty.Equals(text))
				{
					text = null;
				}
			}
			AssemblyBuilderAccess assemblyBuilderAccess;
			if (bReflectionOnly)
			{
				assemblyBuilderAccess = AssemblyBuilderAccess.ReflectionOnly;
			}
			else
			{
				assemblyBuilderAccess = AssemblyBuilderAccess.RunAndSave;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			AssemblyBuilder assemblyBuilder = domain.InternalDefineDynamicAssembly(asmName, assemblyBuilderAccess, text, null, null, null, null, ref stackCrawlMark, null);
			TypeLibConverter.SetGuidAttributeOnAssembly(assemblyBuilder, typeLib);
			TypeLibConverter.SetImportedFromTypeLibAttrOnAssembly(assemblyBuilder, typeLib);
			TypeLibConverter.SetVersionInformation(assemblyBuilder, typeLib, asmName);
			if (bPrimaryInteropAssembly)
			{
				TypeLibConverter.SetPIAAttributeOnAssembly(assemblyBuilder, typeLib);
			}
			return assemblyBuilder;
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000AD2F4 File Offset: 0x000AC2F4
		internal static AssemblyName GetAssemblyNameFromTypelib(object typeLib, string asmFileName, byte[] publicKey, StrongNameKeyPair keyPair, Version asmVersion, AssemblyNameFlags asmNameFlags)
		{
			string text = null;
			string text2 = null;
			int num = 0;
			string text3 = null;
			ITypeLib typeLib2 = (ITypeLib)typeLib;
			typeLib2.GetDocumentation(-1, out text, out text2, out num, out text3);
			if (asmFileName == null)
			{
				asmFileName = text;
			}
			else
			{
				string fileName = Path.GetFileName(asmFileName);
				string extension = Path.GetExtension(asmFileName);
				if (!".dll".Equals(extension, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileExtension"));
				}
				asmFileName = fileName.Substring(0, fileName.Length - ".dll".Length);
			}
			if (asmVersion == null)
			{
				int num2;
				int num3;
				Marshal.GetTypeLibVersion(typeLib2, out num2, out num3);
				asmVersion = new Version(num2, num3, 0, 0);
			}
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Init(asmFileName, publicKey, null, asmVersion, null, AssemblyHashAlgorithm.None, AssemblyVersionCompatibility.SameMachine, null, asmNameFlags, keyPair);
			return assemblyName;
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000AD3B8 File Offset: 0x000AC3B8
		private static void UpdateComTypesInAssembly(AssemblyBuilder asmBldr, ModuleBuilder modBldr)
		{
			AssemblyBuilderData assemblyData = asmBldr.m_assemblyData;
			Type[] types = modBldr.GetTypes();
			int num = types.Length;
			for (int i = 0; i < num; i++)
			{
				assemblyData.AddPublicComType(types[i]);
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000AD3EC File Offset: 0x000AC3EC
		private static void SetGuidAttributeOnAssembly(AssemblyBuilder asmBldr, object typeLib)
		{
			Type[] array = new Type[] { typeof(string) };
			ConstructorInfo constructor = typeof(GuidAttribute).GetConstructor(array);
			object[] array2 = new object[] { Marshal.GetTypeLibGuid((ITypeLib)typeLib).ToString() };
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, array2);
			asmBldr.SetCustomAttribute(customAttributeBuilder);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000AD45C File Offset: 0x000AC45C
		private static void SetImportedFromTypeLibAttrOnAssembly(AssemblyBuilder asmBldr, object typeLib)
		{
			Type[] array = new Type[] { typeof(string) };
			ConstructorInfo constructor = typeof(ImportedFromTypeLibAttribute).GetConstructor(array);
			string typeLibName = Marshal.GetTypeLibName((ITypeLib)typeLib);
			object[] array2 = new object[] { typeLibName };
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, array2);
			asmBldr.SetCustomAttribute(customAttributeBuilder);
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000AD4C0 File Offset: 0x000AC4C0
		private static void SetTypeLibVersionAttribute(AssemblyBuilder asmBldr, object typeLib)
		{
			Type[] array = new Type[]
			{
				typeof(int),
				typeof(int)
			};
			ConstructorInfo constructor = typeof(TypeLibVersionAttribute).GetConstructor(array);
			int num;
			int num2;
			Marshal.GetTypeLibVersion((ITypeLib)typeLib, out num, out num2);
			object[] array2 = new object[] { num, num2 };
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, array2);
			asmBldr.SetCustomAttribute(customAttributeBuilder);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000AD548 File Offset: 0x000AC548
		private static void SetVersionInformation(AssemblyBuilder asmBldr, object typeLib, AssemblyName asmName)
		{
			string text = null;
			string text2 = null;
			int num = 0;
			string text3 = null;
			ITypeLib typeLib2 = (ITypeLib)typeLib;
			typeLib2.GetDocumentation(-1, out text, out text2, out num, out text3);
			string text4 = string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("TypeLibConverter_ImportedTypeLibProductName"), new object[] { text });
			asmBldr.DefineVersionInfoResource(text4, asmName.Version.ToString(), null, null, null);
			TypeLibConverter.SetTypeLibVersionAttribute(asmBldr, typeLib);
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x000AD5B8 File Offset: 0x000AC5B8
		private static void SetPIAAttributeOnAssembly(AssemblyBuilder asmBldr, object typeLib)
		{
			IntPtr @null = Win32Native.NULL;
			ITypeLib typeLib2 = (ITypeLib)typeLib;
			int num = 0;
			int num2 = 0;
			Type[] array = new Type[]
			{
				typeof(int),
				typeof(int)
			};
			ConstructorInfo constructor = typeof(PrimaryInteropAssemblyAttribute).GetConstructor(array);
			try
			{
				typeLib2.GetLibAttr(out @null);
				TYPELIBATTR typelibattr = (TYPELIBATTR)Marshal.PtrToStructure(@null, typeof(TYPELIBATTR));
				num = (int)typelibattr.wMajorVerNum;
				num2 = (int)typelibattr.wMinorVerNum;
			}
			finally
			{
				if (@null != Win32Native.NULL)
				{
					typeLib2.ReleaseTLibAttr(@null);
				}
			}
			object[] array2 = new object[] { num, num2 };
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, array2);
			asmBldr.SetCustomAttribute(customAttributeBuilder);
		}

		// Token: 0x060032DB RID: 13019
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nConvertTypeLibToMetadata(object typeLib, Assembly asmBldr, Module modBldr, string nameSpace, TypeLibImporterFlags flags, ITypeLibImporterNotifySink notifySink, out ArrayList eventItfInfoList);

		// Token: 0x060032DC RID: 13020
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object nConvertAssemblyToTypeLib(Assembly assembly, string strTypeLibName, TypeLibExporterFlags flags, ITypeLibExporterNotifySink notifySink);

		// Token: 0x040019EF RID: 6639
		private const string s_strTypeLibAssemblyTitlePrefix = "TypeLib ";

		// Token: 0x040019F0 RID: 6640
		private const string s_strTypeLibAssemblyDescPrefix = "Assembly generated from typelib ";

		// Token: 0x040019F1 RID: 6641
		private const int MAX_NAMESPACE_LENGTH = 1024;

		// Token: 0x0200051F RID: 1311
		private class TypeResolveHandler : ITypeLibImporterNotifySink
		{
			// Token: 0x060032DE RID: 13022 RVA: 0x000AD6A4 File Offset: 0x000AC6A4
			public TypeResolveHandler(Module mod, ITypeLibImporterNotifySink userSink)
			{
				this.m_Module = mod;
				this.m_UserSink = userSink;
			}

			// Token: 0x060032DF RID: 13023 RVA: 0x000AD6C5 File Offset: 0x000AC6C5
			public void ReportEvent(ImporterEventKind eventKind, int eventCode, string eventMsg)
			{
				this.m_UserSink.ReportEvent(eventKind, eventCode, eventMsg);
			}

			// Token: 0x060032E0 RID: 13024 RVA: 0x000AD6D8 File Offset: 0x000AC6D8
			public Assembly ResolveRef(object typeLib)
			{
				Assembly assembly = this.m_UserSink.ResolveRef(typeLib);
				this.m_AsmList.Add(assembly);
				return assembly;
			}

			// Token: 0x060032E1 RID: 13025 RVA: 0x000AD700 File Offset: 0x000AC700
			public Assembly ResolveEvent(object sender, ResolveEventArgs args)
			{
				try
				{
					this.m_Module.InternalLoadInMemoryTypeByName(args.Name);
					return this.m_Module.Assembly;
				}
				catch (TypeLoadException ex)
				{
					if (ex.ResourceId != -2146233054)
					{
						throw;
					}
				}
				foreach (object obj in this.m_AsmList)
				{
					Assembly assembly = (Assembly)obj;
					try
					{
						assembly.GetType(args.Name, true, false);
						return assembly;
					}
					catch (TypeLoadException ex2)
					{
						if (ex2._HResult != -2146233054)
						{
							throw;
						}
					}
				}
				return null;
			}

			// Token: 0x060032E2 RID: 13026 RVA: 0x000AD7D0 File Offset: 0x000AC7D0
			public Assembly ResolveAsmEvent(object sender, ResolveEventArgs args)
			{
				foreach (object obj in this.m_AsmList)
				{
					Assembly assembly = (Assembly)obj;
					if (string.Compare(assembly.FullName, args.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return assembly;
					}
				}
				return null;
			}

			// Token: 0x060032E3 RID: 13027 RVA: 0x000AD844 File Offset: 0x000AC844
			public Assembly ResolveROAsmEvent(object sender, ResolveEventArgs args)
			{
				foreach (object obj in this.m_AsmList)
				{
					Assembly assembly = (Assembly)obj;
					if (string.Compare(assembly.FullName, args.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return assembly;
					}
				}
				string text = AppDomain.CurrentDomain.ApplyPolicy(args.Name);
				return Assembly.ReflectionOnlyLoad(text);
			}

			// Token: 0x040019F2 RID: 6642
			private Module m_Module;

			// Token: 0x040019F3 RID: 6643
			private ITypeLibImporterNotifySink m_UserSink;

			// Token: 0x040019F4 RID: 6644
			private ArrayList m_AsmList = new ArrayList();
		}
	}
}
