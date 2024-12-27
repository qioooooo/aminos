using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000AD RID: 173
	internal class SchemaNaming
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x00024DBC File Offset: 0x00023DBC
		public static SchemaNaming GetSchemaNaming(Assembly assembly)
		{
			InstrumentedAttribute attribute = InstrumentedAttribute.GetAttribute(assembly);
			if (attribute == null)
			{
				return null;
			}
			return new SchemaNaming(attribute.NamespaceName, attribute.SecurityDescriptor, assembly);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00024DE7 File Offset: 0x00023DE7
		private SchemaNaming(string namespaceName, string securityDescriptor, Assembly assembly)
		{
			this.assembly = assembly;
			this.assemblyInfo = new SchemaNaming.AssemblySpecificNaming(namespaceName, securityDescriptor, assembly);
			if (!SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				this.assemblyInfo.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyMinorVersion(assembly);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00024E22 File Offset: 0x00023E22
		public string NamespaceName
		{
			get
			{
				return this.assemblyInfo.NamespaceName;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00024E2F File Offset: 0x00023E2F
		public string SecurityDescriptor
		{
			get
			{
				return this.assemblyInfo.SecurityDescriptor;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00024E3C File Offset: 0x00023E3C
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00024E49 File Offset: 0x00023E49
		public string DecoupledProviderInstanceName
		{
			get
			{
				return this.assemblyInfo.DecoupledProviderInstanceName;
			}
			set
			{
				this.assemblyInfo.DecoupledProviderInstanceName = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00024E57 File Offset: 0x00023E57
		private string AssemblyUniqueIdentifier
		{
			get
			{
				return this.assemblyInfo.AssemblyUniqueIdentifier;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00024E64 File Offset: 0x00023E64
		private string AssemblyName
		{
			get
			{
				return this.assemblyInfo.AssemblyName;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00024E71 File Offset: 0x00023E71
		private string AssemblyPath
		{
			get
			{
				return this.assemblyInfo.AssemblyPath;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00024E7E File Offset: 0x00023E7E
		private string Win32ProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__Win32Provider");
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00024E95 File Offset: 0x00023E95
		private string DecoupledProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "MSFT_DecoupledProvider");
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00024EAC File Offset: 0x00023EAC
		private string InstrumentationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_Instrumentation");
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00024EC3 File Offset: 0x00023EC3
		private string EventProviderRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__EventProviderRegistration");
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00024EDA File Offset: 0x00023EDA
		private string EventProviderRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.EventProviderRegistrationClassPath, "provider", "\\\\\\\\.\\\\" + this.ProviderPath.Replace("\\", "\\\\").Replace("\"", "\\\""));
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00024F1A File Offset: 0x00023F1A
		private string InstanceProviderRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "__InstanceProviderRegistration");
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00024F31 File Offset: 0x00023F31
		private string InstanceProviderRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.InstanceProviderRegistrationClassPath, "provider", "\\\\\\\\.\\\\" + this.ProviderPath.Replace("\\", "\\\\").Replace("\"", "\\\""));
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00024F71 File Offset: 0x00023F71
		private string ProviderClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_ManagedAssemblyProvider");
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00024F88 File Offset: 0x00023F88
		private string ProviderPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.ProviderClassPath, "Name", this.assemblyInfo.DecoupledProviderInstanceName);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00024FA5 File Offset: 0x00023FA5
		private string RegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath(this.assemblyInfo.NamespaceName, "WMINET_InstrumentedAssembly");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00024FBC File Offset: 0x00023FBC
		private string RegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.RegistrationClassPath, "Name", this.assemblyInfo.DecoupledProviderInstanceName);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00024FD9 File Offset: 0x00023FD9
		private string GlobalRegistrationNamespace
		{
			get
			{
				return "root\\MicrosoftWmiNet";
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00024FE0 File Offset: 0x00023FE0
		private string GlobalInstrumentationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_Instrumentation");
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00024FF1 File Offset: 0x00023FF1
		private string GlobalRegistrationClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_InstrumentedNamespaces");
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00025002 File Offset: 0x00024002
		private string GlobalRegistrationPath
		{
			get
			{
				return SchemaNaming.AppendProperty(this.GlobalRegistrationClassPath, "NamespaceName", this.assemblyInfo.NamespaceName.Replace("\\", "\\\\"));
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0002502E File Offset: 0x0002402E
		private string GlobalNamingClassPath
		{
			get
			{
				return SchemaNaming.MakeClassPath("root\\MicrosoftWmiNet", "WMINET_Naming");
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0002503F File Offset: 0x0002403F
		private string DataDirectory
		{
			get
			{
				return Path.Combine(WMICapabilities.FrameworkDirectory, this.NamespaceName);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00025051 File Offset: 0x00024051
		private string MofPath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".mof");
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0002506E File Offset: 0x0002406E
		private string CodePath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".cs");
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0002508B File Offset: 0x0002408B
		private string PrecompiledAssemblyPath
		{
			get
			{
				return Path.Combine(this.DataDirectory, this.DecoupledProviderInstanceName + ".dll");
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000250A8 File Offset: 0x000240A8
		private static string MakeClassPath(string namespaceName, string className)
		{
			return namespaceName + ":" + className;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000250B8 File Offset: 0x000240B8
		private static string AppendProperty(string classPath, string propertyName, string propertyValue)
		{
			return string.Concat(new object[] { classPath, '.', propertyName, "=\"", propertyValue, '"' });
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000250FC File Offset: 0x000240FC
		public bool IsAssemblyRegistered()
		{
			if (SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				ManagementObject managementObject = new ManagementObject(this.RegistrationPath);
				return 0 == string.Compare(this.AssemblyUniqueIdentifier, managementObject["RegisteredBuild"].ToString(), StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00025144 File Offset: 0x00024144
		private bool IsSchemaToBeCompared()
		{
			bool flag = false;
			if (SchemaNaming.DoesInstanceExist(this.RegistrationPath))
			{
				ManagementObject managementObject = new ManagementObject(this.RegistrationPath);
				flag = 0 != string.Compare(this.AssemblyUniqueIdentifier, managementObject["RegisteredBuild"].ToString(), StringComparison.OrdinalIgnoreCase);
			}
			return flag;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00025190 File Offset: 0x00024190
		private ManagementObject RegistrationInstance
		{
			get
			{
				if (this.registrationInstance == null)
				{
					this.registrationInstance = new ManagementObject(this.RegistrationPath);
				}
				return this.registrationInstance;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x000251B4 File Offset: 0x000241B4
		public string Code
		{
			get
			{
				string text;
				using (StreamReader streamReader = new StreamReader(this.CodePath))
				{
					text = streamReader.ReadToEnd();
				}
				return text;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x000251F4 File Offset: 0x000241F4
		public string Mof
		{
			get
			{
				string text;
				using (StreamReader streamReader = new StreamReader(this.MofPath))
				{
					text = streamReader.ReadToEnd();
				}
				return text;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00025234 File Offset: 0x00024234
		public Assembly PrecompiledAssembly
		{
			get
			{
				if (File.Exists(this.PrecompiledAssemblyPath))
				{
					return Assembly.LoadFrom(this.PrecompiledAssemblyPath);
				}
				return null;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00025250 File Offset: 0x00024250
		private bool IsClassAlreadyPresentInRepository(ManagementObject obj)
		{
			bool flag = false;
			string text = SchemaNaming.MakeClassPath(this.NamespaceName, (string)obj.SystemProperties["__CLASS"].Value);
			if (SchemaNaming.DoesClassExist(text))
			{
				ManagementObject managementObject = new ManagementClass(text);
				flag = managementObject.CompareTo(obj, ComparisonSettings.IgnoreObjectSource | ComparisonSettings.IgnoreCase);
			}
			return flag;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000252A0 File Offset: 0x000242A0
		private string GenerateMof(string[] mofs)
		{
			return string.Concat(new string[]
			{
				"//**************************************************************************\r\n",
				string.Format("//* {0}\r\n", this.DecoupledProviderInstanceName),
				string.Format("//* {0}\r\n", this.AssemblyUniqueIdentifier),
				"//**************************************************************************\r\n",
				"#pragma autorecover\r\n",
				SchemaNaming.EnsureNamespaceInMof(this.GlobalRegistrationNamespace),
				SchemaNaming.EnsureNamespaceInMof(this.NamespaceName),
				SchemaNaming.PragmaNamespace(this.GlobalRegistrationNamespace),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalInstrumentationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalRegistrationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.GlobalNamingClassPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.GlobalRegistrationPath)),
				SchemaNaming.PragmaNamespace(this.NamespaceName),
				SchemaNaming.GetMofFormat(new ManagementClass(this.InstrumentationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.RegistrationClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.DecoupledProviderClassPath)),
				SchemaNaming.GetMofFormat(new ManagementClass(this.ProviderClassPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.ProviderPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.EventProviderRegistrationPath)),
				SchemaNaming.GetMofFormat(new ManagementObject(this.InstanceProviderRegistrationPath)),
				string.Concat(mofs),
				SchemaNaming.GetMofFormat(new ManagementObject(this.RegistrationPath))
			});
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0002542C File Offset: 0x0002442C
		public void RegisterNonAssemblySpecificSchema(InstallContext installContext)
		{
			SecurityHelper.UnmanagedCode.Demand();
			WmiNetUtilsHelper.VerifyClientKey_f();
			SchemaNaming.InstallLogWrapper installLogWrapper = new SchemaNaming.InstallLogWrapper(installContext);
			SchemaNaming.EnsureNamespace(installLogWrapper, this.GlobalRegistrationNamespace);
			SchemaNaming.EnsureClassExists(installLogWrapper, this.GlobalInstrumentationClassPath, new SchemaNaming.ClassMaker(this.MakeGlobalInstrumentationClass));
			SchemaNaming.EnsureClassExists(installLogWrapper, this.GlobalRegistrationClassPath, new SchemaNaming.ClassMaker(this.MakeNamespaceRegistrationClass));
			SchemaNaming.EnsureClassExists(installLogWrapper, this.GlobalNamingClassPath, new SchemaNaming.ClassMaker(this.MakeNamingClass));
			SchemaNaming.EnsureNamespace(installLogWrapper, this.NamespaceName);
			SchemaNaming.EnsureClassExists(installLogWrapper, this.InstrumentationClassPath, new SchemaNaming.ClassMaker(this.MakeInstrumentationClass));
			SchemaNaming.EnsureClassExists(installLogWrapper, this.RegistrationClassPath, new SchemaNaming.ClassMaker(this.MakeRegistrationClass));
			try
			{
				ManagementClass managementClass = new ManagementClass(this.DecoupledProviderClassPath);
				if (managementClass["HostingModel"].ToString() != "Decoupled:Com")
				{
					managementClass.Delete();
				}
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
			}
			SchemaNaming.EnsureClassExists(installLogWrapper, this.DecoupledProviderClassPath, new SchemaNaming.ClassMaker(this.MakeDecoupledProviderClass));
			SchemaNaming.EnsureClassExists(installLogWrapper, this.ProviderClassPath, new SchemaNaming.ClassMaker(this.MakeProviderClass));
			if (!SchemaNaming.DoesInstanceExist(this.GlobalRegistrationPath))
			{
				this.RegisterNamespaceAsInstrumented();
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0002557C File Offset: 0x0002457C
		public void RegisterAssemblySpecificSchema()
		{
			SecurityHelper.UnmanagedCode.Demand();
			Type[] instrumentedTypes = InstrumentedAttribute.GetInstrumentedTypes(this.assembly);
			StringCollection stringCollection = new StringCollection();
			StringCollection stringCollection2 = new StringCollection();
			StringCollection stringCollection3 = new StringCollection();
			string[] array = new string[instrumentedTypes.Length];
			CodeWriter codeWriter = new CodeWriter();
			ReferencesCollection referencesCollection = new ReferencesCollection();
			codeWriter.AddChild(referencesCollection.UsingCode);
			referencesCollection.Add(typeof(object));
			referencesCollection.Add(typeof(ManagementClass));
			referencesCollection.Add(typeof(Marshal));
			referencesCollection.Add(typeof(SuppressUnmanagedCodeSecurityAttribute));
			referencesCollection.Add(typeof(FieldInfo));
			referencesCollection.Add(typeof(Hashtable));
			codeWriter.Line();
			CodeWriter codeWriter2 = codeWriter.AddChild("public class WMINET_Converter");
			codeWriter2.Line("public static Hashtable mapTypeToConverter = new Hashtable();");
			CodeWriter codeWriter3 = codeWriter2.AddChild("static WMINET_Converter()");
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < instrumentedTypes.Length; i++)
			{
				hashtable[instrumentedTypes[i]] = "ConvertClass_" + i;
			}
			bool flag = this.IsSchemaToBeCompared();
			bool flag2 = false;
			if (!flag)
			{
				flag2 = !this.IsAssemblyRegistered();
			}
			for (int j = 0; j < instrumentedTypes.Length; j++)
			{
				SchemaMapping schemaMapping = new SchemaMapping(instrumentedTypes[j], this, hashtable);
				codeWriter3.Line(string.Format("mapTypeToConverter[typeof({0})] = typeof({1});", schemaMapping.ClassType.FullName.Replace('+', '.'), schemaMapping.CodeClassName));
				if (flag && !this.IsClassAlreadyPresentInRepository(schemaMapping.NewClass))
				{
					flag2 = true;
				}
				SchemaNaming.ReplaceClassIfNecessary(schemaMapping.ClassPath, schemaMapping.NewClass);
				array[j] = SchemaNaming.GetMofFormat(schemaMapping.NewClass);
				codeWriter.AddChild(schemaMapping.Code);
				switch (schemaMapping.InstrumentationType)
				{
				case InstrumentationType.Instance:
					stringCollection2.Add(schemaMapping.ClassName);
					break;
				case InstrumentationType.Event:
					stringCollection.Add(schemaMapping.ClassName);
					break;
				case InstrumentationType.Abstract:
					stringCollection3.Add(schemaMapping.ClassName);
					break;
				}
			}
			this.RegisterAssemblySpecificDecoupledProviderInstance();
			this.RegisterProviderAsEventProvider(stringCollection);
			this.RegisterProviderAsInstanceProvider();
			this.RegisterAssemblyAsInstrumented();
			Directory.CreateDirectory(this.DataDirectory);
			using (StreamWriter streamWriter = new StreamWriter(this.CodePath, false, Encoding.Unicode))
			{
				streamWriter.WriteLine(codeWriter);
				streamWriter.WriteLine("class IWOA\r\n{\r\nprotected const string DllName = \"wminet_utils.dll\";\r\nprotected const string EntryPointName = \"UFunc\";\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyHandle\")] public static extern int GetPropertyHandle_f27(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszPropertyName, [Out] out Int32 pType, [Out] out Int32 plHandle);\r\n//[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadPropertyValue\")] public static extern int ReadPropertyValue_f29(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lBufferSize, [Out] out Int32 plNumBytes, [Out] out Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadDWORD\")] public static extern int ReadDWORD_f30(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt32 pdw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadQWORD\")] public static extern int ReadQWORD_f32(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt64 pqw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyInfoByHandle\")] public static extern int GetPropertyInfoByHandle_f34(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out][MarshalAs(UnmanagedType.BStr)]  out string   pstrName, [Out] out Int32 pType);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Lock\")] public static extern int Lock_f35(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Unlock\")] public static extern int Unlock_f36(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Put\")] public static extern int Put_f5(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszName, [In] Int32 lFlags, [In] ref object pVal, [In] Int32 Type);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In][MarshalAs(UnmanagedType.LPWStr)] string str);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref SByte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Int16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref UInt16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\", CharSet=CharSet.Unicode)] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Char c);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteSingle\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Single dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDouble\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Double pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Clone\")] public static extern int Clone_f(int vFunc, IntPtr pWbemClassObject, [Out] out IntPtr ppCopy);\r\n}\r\ninterface IWmiConverter\r\n{\r\n    void ToWMI(object obj);\r\n    ManagementObject GetInstance();\r\n}\r\nclass SafeAssign\r\n{\r\n    public static UInt16 boolTrue = 0xffff;\r\n    public static UInt16 boolFalse = 0;\r\n    static Hashtable validTypes = new Hashtable();\r\n    static SafeAssign()\r\n    {\r\n        validTypes.Add(typeof(SByte), null);\r\n        validTypes.Add(typeof(Byte), null);\r\n        validTypes.Add(typeof(Int16), null);\r\n        validTypes.Add(typeof(UInt16), null);\r\n        validTypes.Add(typeof(Int32), null);\r\n        validTypes.Add(typeof(UInt32), null);\r\n        validTypes.Add(typeof(Int64), null);\r\n        validTypes.Add(typeof(UInt64), null);\r\n        validTypes.Add(typeof(Single), null);\r\n        validTypes.Add(typeof(Double), null);\r\n        validTypes.Add(typeof(Boolean), null);\r\n        validTypes.Add(typeof(String), null);\r\n        validTypes.Add(typeof(Char), null);\r\n        validTypes.Add(typeof(DateTime), null);\r\n        validTypes.Add(typeof(TimeSpan), null);\r\n        validTypes.Add(typeof(ManagementObject), null);\r\n        nullClass.SystemProperties [\"__CLASS\"].Value = \"nullInstance\";\r\n    }\r\n    public static object GetInstance(object o)\r\n    {\r\n        if(o is ManagementObject)\r\n            return o;\r\n        return null;\r\n    }\r\n    static ManagementClass nullClass = new ManagementClass(new ManagementPath(@\"" + this.NamespaceName + "\"));\r\n    \r\n    public static ManagementObject GetManagementObject(object o)\r\n    {\r\n        if(o != null && o is ManagementObject)\r\n            return o as ManagementObject;\r\n        // Must return empty instance\r\n        return nullClass.CreateInstance();\r\n    }\r\n    public static object GetValue(object o)\r\n    {\r\n        Type t = o.GetType();\r\n        if(t.IsArray)\r\n            t = t.GetElementType();\r\n        if(validTypes.Contains(t))\r\n            return o;\r\n        return null;\r\n    }\r\n    public static string WMITimeToString(DateTime dt)\r\n    {\r\n        TimeSpan ts = dt.Subtract(dt.ToUniversalTime());\r\n        int diffUTC = (ts.Minutes + ts.Hours * 60);\r\n        if(diffUTC >= 0)\r\n            return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000+{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, diffUTC);\r\n        return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000-{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, -diffUTC);\r\n    }\r\n    public static string WMITimeToString(TimeSpan ts)\r\n    {\r\n        return String.Format(\"{0:D8}{1:D2}{2:D2}{3:D2}.{4:D3}000:000\", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(DateTime[] dates)\r\n    {\r\n        string[] strings = new string[dates.Length];\r\n        for(int i=0;i<dates.Length;i++)\r\n            strings[i] = WMITimeToString(dates[i]);\r\n        return strings;\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(TimeSpan[] timeSpans)\r\n    {\r\n        string[] strings = new string[timeSpans.Length];\r\n        for(int i=0;i<timeSpans.Length;i++)\r\n            strings[i] = WMITimeToString(timeSpans[i]);\r\n        return strings;\r\n    }\r\n}\r\n");
			}
			using (StreamWriter streamWriter2 = new StreamWriter(this.MofPath, false, Encoding.Unicode))
			{
				streamWriter2.WriteLine(this.GenerateMof(array));
			}
			if (flag2)
			{
				SchemaNaming.RegisterSchemaUsingMofcomp(this.MofPath);
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0002586C File Offset: 0x0002486C
		private void RegisterNamespaceAsInstrumented()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["NamespaceName"] = this.NamespaceName;
			managementObject.Put();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000258A4 File Offset: 0x000248A4
		private void RegisterAssemblyAsInstrumented()
		{
			ManagementClass managementClass = new ManagementClass(this.RegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["Name"] = this.DecoupledProviderInstanceName;
			managementObject["RegisteredBuild"] = this.AssemblyUniqueIdentifier;
			managementObject["FullName"] = this.AssemblyName;
			managementObject["PathToAssembly"] = this.AssemblyPath;
			managementObject["Code"] = "";
			managementObject["Mof"] = "";
			managementObject.Put();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00025930 File Offset: 0x00024930
		private void RegisterAssemblySpecificDecoupledProviderInstance()
		{
			ManagementClass managementClass = new ManagementClass(this.ProviderClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["Name"] = this.DecoupledProviderInstanceName;
			managementObject["HostingModel"] = "Decoupled:Com";
			if (this.SecurityDescriptor != null)
			{
				managementObject["SecurityDescriptor"] = this.SecurityDescriptor;
			}
			managementObject.Put();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00025994 File Offset: 0x00024994
		private string RegisterProviderAsEventProvider(StringCollection events)
		{
			ManagementClass managementClass = new ManagementClass(this.EventProviderRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["provider"] = "\\\\.\\" + this.ProviderPath;
			string[] array = new string[events.Count];
			int num = 0;
			foreach (string text in events)
			{
				array[num++] = "select * from " + text;
			}
			managementObject["EventQueryList"] = array;
			return managementObject.Put().Path;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00025A4C File Offset: 0x00024A4C
		private string RegisterProviderAsInstanceProvider()
		{
			ManagementClass managementClass = new ManagementClass(this.InstanceProviderRegistrationClassPath);
			ManagementObject managementObject = managementClass.CreateInstance();
			managementObject["provider"] = "\\\\.\\" + this.ProviderPath;
			managementObject["SupportsGet"] = true;
			managementObject["SupportsEnumeration"] = true;
			return managementObject.Put().Path;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00025AB4 File Offset: 0x00024AB4
		private ManagementClass MakeNamingClass()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalInstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_Naming");
			managementClass2.Qualifiers.Add("abstract", true);
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("InstrumentedAssembliesClassName", "WMINET_InstrumentedAssembly", CimType.String);
			return managementClass2;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00025B08 File Offset: 0x00024B08
		private ManagementClass MakeInstrumentationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.NamespaceName, "", null);
			managementClass.SystemProperties["__CLASS"].Value = "WMINET_Instrumentation";
			managementClass.Qualifiers.Add("abstract", true);
			return managementClass;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00025B58 File Offset: 0x00024B58
		private ManagementClass MakeGlobalInstrumentationClass()
		{
			ManagementClass managementClass = new ManagementClass("root\\MicrosoftWmiNet", "", null);
			managementClass.SystemProperties["__CLASS"].Value = "WMINET_Instrumentation";
			managementClass.Qualifiers.Add("abstract", true);
			return managementClass;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00025BA8 File Offset: 0x00024BA8
		private ManagementClass MakeRegistrationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.InstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_InstrumentedAssembly");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("Name", CimType.String, false);
			PropertyData propertyData = properties["Name"];
			propertyData.Qualifiers.Add("key", true);
			properties.Add("RegisteredBuild", CimType.String, false);
			properties.Add("FullName", CimType.String, false);
			properties.Add("PathToAssembly", CimType.String, false);
			properties.Add("Code", CimType.String, false);
			properties.Add("Mof", CimType.String, false);
			return managementClass2;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00025C48 File Offset: 0x00024C48
		private ManagementClass MakeNamespaceRegistrationClass()
		{
			ManagementClass managementClass = new ManagementClass(this.GlobalInstrumentationClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_InstrumentedNamespaces");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("NamespaceName", CimType.String, false);
			PropertyData propertyData = properties["NamespaceName"];
			propertyData.Qualifiers.Add("key", true);
			return managementClass2;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00025CA4 File Offset: 0x00024CA4
		private ManagementClass MakeDecoupledProviderClass()
		{
			ManagementClass managementClass = new ManagementClass(this.Win32ProviderClassPath);
			ManagementClass managementClass2 = managementClass.Derive("MSFT_DecoupledProvider");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("HostingModel", "Decoupled:Com", CimType.String);
			properties.Add("SecurityDescriptor", CimType.String, false);
			properties.Add("Version", 1, CimType.UInt32);
			properties["CLSID"].Value = "{54D8502C-527D-43f7-A506-A9DA075E229C}";
			return managementClass2;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00025D18 File Offset: 0x00024D18
		private ManagementClass MakeProviderClass()
		{
			ManagementClass managementClass = new ManagementClass(this.DecoupledProviderClassPath);
			ManagementClass managementClass2 = managementClass.Derive("WMINET_ManagedAssemblyProvider");
			PropertyDataCollection properties = managementClass2.Properties;
			properties.Add("Assembly", CimType.String, false);
			return managementClass2;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00025D54 File Offset: 0x00024D54
		private static void RegisterSchemaUsingMofcomp(string mofPath)
		{
			Process process = Process.Start(new ProcessStartInfo
			{
				Arguments = mofPath,
				FileName = WMICapabilities.InstallationDirectory + "\\mofcomp.exe",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			});
			process.WaitForExit();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00025DA8 File Offset: 0x00024DA8
		private static void EnsureNamespace(string baseNamespace, string childNamespaceName)
		{
			if (!SchemaNaming.DoesInstanceExist(baseNamespace + ":__NAMESPACE.Name=\"" + childNamespaceName + "\""))
			{
				ManagementClass managementClass = new ManagementClass(baseNamespace + ":__NAMESPACE");
				ManagementObject managementObject = managementClass.CreateInstance();
				managementObject["Name"] = childNamespaceName;
				managementObject.Put();
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00025DF8 File Offset: 0x00024DF8
		private static void EnsureNamespace(SchemaNaming.InstallLogWrapper context, string namespaceName)
		{
			context.LogMessage(RC.GetString("NAMESPACE_ENSURE") + " " + namespaceName);
			string text = null;
			foreach (string text2 in namespaceName.Split(new char[] { '\\' }))
			{
				if (text == null)
				{
					text = text2;
				}
				else
				{
					SchemaNaming.EnsureNamespace(text, text2);
					text = text + "\\" + text2;
				}
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00025E68 File Offset: 0x00024E68
		private static void EnsureClassExists(SchemaNaming.InstallLogWrapper context, string classPath, SchemaNaming.ClassMaker classMakerFunction)
		{
			try
			{
				context.LogMessage(RC.GetString("CLASS_ENSURE") + " " + classPath);
				ManagementClass managementClass = new ManagementClass(classPath);
				managementClass.Get();
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
				context.LogMessage(RC.GetString("CLASS_ENSURECREATE") + " " + classPath);
				ManagementClass managementClass2 = classMakerFunction();
				managementClass2.Put();
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00025EEC File Offset: 0x00024EEC
		private static bool DoesInstanceExist(string objectPath)
		{
			bool flag = false;
			try
			{
				ManagementObject managementObject = new ManagementObject(objectPath);
				managementObject.Get();
				flag = true;
			}
			catch (ManagementException ex)
			{
				if (ManagementStatus.InvalidNamespace != ex.ErrorCode && ManagementStatus.InvalidClass != ex.ErrorCode && ManagementStatus.NotFound != ex.ErrorCode)
				{
					throw ex;
				}
			}
			return flag;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00025F4C File Offset: 0x00024F4C
		private static bool DoesClassExist(string objectPath)
		{
			bool flag = false;
			try
			{
				ManagementObject managementObject = new ManagementClass(objectPath);
				managementObject.Get();
				flag = true;
			}
			catch (ManagementException ex)
			{
				if (ManagementStatus.InvalidNamespace != ex.ErrorCode && ManagementStatus.InvalidClass != ex.ErrorCode && ManagementStatus.NotFound != ex.ErrorCode)
				{
					throw ex;
				}
			}
			return flag;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00025FAC File Offset: 0x00024FAC
		private static ManagementClass SafeGetClass(string classPath)
		{
			ManagementClass managementClass = null;
			try
			{
				ManagementClass managementClass2 = new ManagementClass(classPath);
				managementClass2.Get();
				managementClass = managementClass2;
			}
			catch (ManagementException ex)
			{
				if (ex.ErrorCode != ManagementStatus.NotFound)
				{
					throw ex;
				}
			}
			return managementClass;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00025FF0 File Offset: 0x00024FF0
		private static void ReplaceClassIfNecessary(string classPath, ManagementClass newClass)
		{
			try
			{
				ManagementClass managementClass = SchemaNaming.SafeGetClass(classPath);
				if (managementClass == null)
				{
					newClass.Put();
				}
				else if (newClass.GetText(TextFormat.Mof) != managementClass.GetText(TextFormat.Mof))
				{
					managementClass.Delete();
					newClass.Put();
				}
			}
			catch (ManagementException ex)
			{
				string text = RC.GetString("CLASS_NOTREPLACED_EXCEPT") + "\r\n{0}\r\n{1}";
				throw new ArgumentException(string.Format(text, classPath, newClass.GetText(TextFormat.Mof)), ex);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00026070 File Offset: 0x00025070
		private static string GetMofFormat(ManagementObject obj)
		{
			return obj.GetText(TextFormat.Mof).Replace("\n", "\r\n") + "\r\n";
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00026092 File Offset: 0x00025092
		private static string PragmaNamespace(string namespaceName)
		{
			return string.Format("#pragma namespace(\"\\\\\\\\.\\\\{0}\")\r\n\r\n", namespaceName.Replace("\\", "\\\\"));
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000260AE File Offset: 0x000250AE
		private static string EnsureNamespaceInMof(string baseNamespace, string childNamespaceName)
		{
			return string.Format("{0}instance of __Namespace\r\n{{\r\n  Name = \"{1}\";\r\n}};\r\n\r\n", SchemaNaming.PragmaNamespace(baseNamespace), childNamespaceName);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000260C4 File Offset: 0x000250C4
		private static string EnsureNamespaceInMof(string namespaceName)
		{
			string text = "";
			string text2 = null;
			foreach (string text3 in namespaceName.Split(new char[] { '\\' }))
			{
				if (text2 == null)
				{
					text2 = text3;
				}
				else
				{
					text += SchemaNaming.EnsureNamespaceInMof(text2, text3);
					text2 = text2 + "\\" + text3;
				}
			}
			return text;
		}

		// Token: 0x040002AB RID: 683
		private const string Win32ProviderClassName = "__Win32Provider";

		// Token: 0x040002AC RID: 684
		private const string EventProviderRegistrationClassName = "__EventProviderRegistration";

		// Token: 0x040002AD RID: 685
		private const string InstanceProviderRegistrationClassName = "__InstanceProviderRegistration";

		// Token: 0x040002AE RID: 686
		private const string DecoupledProviderClassName = "MSFT_DecoupledProvider";

		// Token: 0x040002AF RID: 687
		private const string ProviderClassName = "WMINET_ManagedAssemblyProvider";

		// Token: 0x040002B0 RID: 688
		private const string InstrumentationClassName = "WMINET_Instrumentation";

		// Token: 0x040002B1 RID: 689
		private const string InstrumentedAssembliesClassName = "WMINET_InstrumentedAssembly";

		// Token: 0x040002B2 RID: 690
		private const string DecoupledProviderCLSID = "{54D8502C-527D-43f7-A506-A9DA075E229C}";

		// Token: 0x040002B3 RID: 691
		private const string GlobalWmiNetNamespace = "root\\MicrosoftWmiNet";

		// Token: 0x040002B4 RID: 692
		private const string InstrumentedNamespacesClassName = "WMINET_InstrumentedNamespaces";

		// Token: 0x040002B5 RID: 693
		private const string NamingClassName = "WMINET_Naming";

		// Token: 0x040002B6 RID: 694
		private const string iwoaDef = "class IWOA\r\n{\r\nprotected const string DllName = \"wminet_utils.dll\";\r\nprotected const string EntryPointName = \"UFunc\";\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyHandle\")] public static extern int GetPropertyHandle_f27(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszPropertyName, [Out] out Int32 pType, [Out] out Int32 plHandle);\r\n//[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadPropertyValue\")] public static extern int ReadPropertyValue_f29(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lBufferSize, [Out] out Int32 plNumBytes, [Out] out Byte aData);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadDWORD\")] public static extern int ReadDWORD_f30(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt32 pdw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"ReadQWORD\")] public static extern int ReadQWORD_f32(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out] out UInt64 pqw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] UInt64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"GetPropertyInfoByHandle\")] public static extern int GetPropertyInfoByHandle_f34(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [Out][MarshalAs(UnmanagedType.BStr)]  out string   pstrName, [Out] out Int32 pType);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Lock\")] public static extern int Lock_f35(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Unlock\")] public static extern int Unlock_f36(int vFunc, IntPtr pWbemClassObject, [In] Int32 lFlags);\r\n\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Put\")] public static extern int Put_f5(int vFunc, IntPtr pWbemClassObject, [In][MarshalAs(UnmanagedType.LPWStr)]  string   wszName, [In] Int32 lFlags, [In] ref object pVal, [In] Int32 Type);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In][MarshalAs(UnmanagedType.LPWStr)] string str);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Byte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref SByte n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Int16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\")] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref UInt16 n);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WritePropertyValue\", CharSet=CharSet.Unicode)] public static extern int WritePropertyValue_f28(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 lNumBytes, [In] ref Char c);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDWORD\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int32 dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteSingle\")] public static extern int WriteDWORD_f31(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Single dw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteQWORD\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Int64 pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"WriteDouble\")] public static extern int WriteQWORD_f33(int vFunc, IntPtr pWbemClassObject, [In] Int32 lHandle, [In] Double pw);\r\n[SuppressUnmanagedCodeSecurity, DllImport(DllName, EntryPoint=\"Clone\")] public static extern int Clone_f(int vFunc, IntPtr pWbemClassObject, [Out] out IntPtr ppCopy);\r\n}\r\ninterface IWmiConverter\r\n{\r\n    void ToWMI(object obj);\r\n    ManagementObject GetInstance();\r\n}\r\nclass SafeAssign\r\n{\r\n    public static UInt16 boolTrue = 0xffff;\r\n    public static UInt16 boolFalse = 0;\r\n    static Hashtable validTypes = new Hashtable();\r\n    static SafeAssign()\r\n    {\r\n        validTypes.Add(typeof(SByte), null);\r\n        validTypes.Add(typeof(Byte), null);\r\n        validTypes.Add(typeof(Int16), null);\r\n        validTypes.Add(typeof(UInt16), null);\r\n        validTypes.Add(typeof(Int32), null);\r\n        validTypes.Add(typeof(UInt32), null);\r\n        validTypes.Add(typeof(Int64), null);\r\n        validTypes.Add(typeof(UInt64), null);\r\n        validTypes.Add(typeof(Single), null);\r\n        validTypes.Add(typeof(Double), null);\r\n        validTypes.Add(typeof(Boolean), null);\r\n        validTypes.Add(typeof(String), null);\r\n        validTypes.Add(typeof(Char), null);\r\n        validTypes.Add(typeof(DateTime), null);\r\n        validTypes.Add(typeof(TimeSpan), null);\r\n        validTypes.Add(typeof(ManagementObject), null);\r\n        nullClass.SystemProperties [\"__CLASS\"].Value = \"nullInstance\";\r\n    }\r\n    public static object GetInstance(object o)\r\n    {\r\n        if(o is ManagementObject)\r\n            return o;\r\n        return null;\r\n    }\r\n    static ManagementClass nullClass = new ManagementClass(";

		// Token: 0x040002B7 RID: 695
		private const string iwoaDefEnd = ");\r\n    \r\n    public static ManagementObject GetManagementObject(object o)\r\n    {\r\n        if(o != null && o is ManagementObject)\r\n            return o as ManagementObject;\r\n        // Must return empty instance\r\n        return nullClass.CreateInstance();\r\n    }\r\n    public static object GetValue(object o)\r\n    {\r\n        Type t = o.GetType();\r\n        if(t.IsArray)\r\n            t = t.GetElementType();\r\n        if(validTypes.Contains(t))\r\n            return o;\r\n        return null;\r\n    }\r\n    public static string WMITimeToString(DateTime dt)\r\n    {\r\n        TimeSpan ts = dt.Subtract(dt.ToUniversalTime());\r\n        int diffUTC = (ts.Minutes + ts.Hours * 60);\r\n        if(diffUTC >= 0)\r\n            return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000+{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, diffUTC);\r\n        return String.Format(\"{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.{6:D3}000-{7:D3}\", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, -diffUTC);\r\n    }\r\n    public static string WMITimeToString(TimeSpan ts)\r\n    {\r\n        return String.Format(\"{0:D8}{1:D2}{2:D2}{3:D2}.{4:D3}000:000\", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(DateTime[] dates)\r\n    {\r\n        string[] strings = new string[dates.Length];\r\n        for(int i=0;i<dates.Length;i++)\r\n            strings[i] = WMITimeToString(dates[i]);\r\n        return strings;\r\n    }\r\n    public static string[] WMITimeArrayToStringArray(TimeSpan[] timeSpans)\r\n    {\r\n        string[] strings = new string[timeSpans.Length];\r\n        for(int i=0;i<timeSpans.Length;i++)\r\n            strings[i] = WMITimeToString(timeSpans[i]);\r\n        return strings;\r\n    }\r\n}\r\n";

		// Token: 0x040002B8 RID: 696
		private Assembly assembly;

		// Token: 0x040002B9 RID: 697
		private SchemaNaming.AssemblySpecificNaming assemblyInfo;

		// Token: 0x040002BA RID: 698
		private ManagementObject registrationInstance;

		// Token: 0x020000AE RID: 174
		private class InstallLogWrapper
		{
			// Token: 0x0600053D RID: 1341 RVA: 0x0002612A File Offset: 0x0002512A
			public InstallLogWrapper(InstallContext context)
			{
				this.context = context;
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x00026139 File Offset: 0x00025139
			public void LogMessage(string str)
			{
				if (this.context != null)
				{
					this.context.LogMessage(str);
				}
			}

			// Token: 0x040002BB RID: 699
			private InstallContext context;
		}

		// Token: 0x020000AF RID: 175
		private class AssemblySpecificNaming
		{
			// Token: 0x0600053F RID: 1343 RVA: 0x00026150 File Offset: 0x00025150
			public AssemblySpecificNaming(string namespaceName, string securityDescriptor, Assembly assembly)
			{
				this.namespaceName = namespaceName;
				this.securityDescriptor = securityDescriptor;
				this.decoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
				this.assemblyUniqueIdentifier = AssemblyNameUtility.UniqueToAssemblyBuild(assembly);
				this.assemblyName = assembly.FullName;
				this.assemblyPath = assembly.Location;
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000540 RID: 1344 RVA: 0x000261A1 File Offset: 0x000251A1
			public string NamespaceName
			{
				get
				{
					return this.namespaceName;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x06000541 RID: 1345 RVA: 0x000261A9 File Offset: 0x000251A9
			public string SecurityDescriptor
			{
				get
				{
					return this.securityDescriptor;
				}
			}

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x06000542 RID: 1346 RVA: 0x000261B1 File Offset: 0x000251B1
			// (set) Token: 0x06000543 RID: 1347 RVA: 0x000261B9 File Offset: 0x000251B9
			public string DecoupledProviderInstanceName
			{
				get
				{
					return this.decoupledProviderInstanceName;
				}
				set
				{
					this.decoupledProviderInstanceName = value;
				}
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x06000544 RID: 1348 RVA: 0x000261C2 File Offset: 0x000251C2
			public string AssemblyUniqueIdentifier
			{
				get
				{
					return this.assemblyUniqueIdentifier;
				}
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x06000545 RID: 1349 RVA: 0x000261CA File Offset: 0x000251CA
			public string AssemblyName
			{
				get
				{
					return this.assemblyName;
				}
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x06000546 RID: 1350 RVA: 0x000261D2 File Offset: 0x000251D2
			public string AssemblyPath
			{
				get
				{
					return this.assemblyPath;
				}
			}

			// Token: 0x040002BC RID: 700
			private string namespaceName;

			// Token: 0x040002BD RID: 701
			private string securityDescriptor;

			// Token: 0x040002BE RID: 702
			private string decoupledProviderInstanceName;

			// Token: 0x040002BF RID: 703
			private string assemblyUniqueIdentifier;

			// Token: 0x040002C0 RID: 704
			private string assemblyName;

			// Token: 0x040002C1 RID: 705
			private string assemblyPath;
		}

		// Token: 0x020000B0 RID: 176
		// (Invoke) Token: 0x06000548 RID: 1352
		private delegate ManagementClass ClassMaker();
	}
}
