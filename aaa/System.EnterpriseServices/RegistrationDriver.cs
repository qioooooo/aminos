using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.EnterpriseServices
{
	// Token: 0x02000086 RID: 134
	internal class RegistrationDriver
	{
		// Token: 0x06000306 RID: 774 RVA: 0x00008339 File Offset: 0x00007339
		internal static void SaveChanges(ICatalogCollection coll)
		{
			coll.SaveChanges();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00008344 File Offset: 0x00007344
		internal static void Populate(ICatalogCollection coll)
		{
			try
			{
				coll.Populate();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != Util.COMADMIN_E_OBJECTERRORS)
				{
					throw;
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000837C File Offset: 0x0000737C
		private static RegistrationErrorInfo[] BuildErrorInfoChain(ICatalogCollection coll)
		{
			RegistrationErrorInfo[] array2;
			try
			{
				RegistrationDriver.Populate(coll);
				int num = coll.Count();
				RegistrationErrorInfo[] array = null;
				if (num > 0)
				{
					array = new RegistrationErrorInfo[num];
					for (int i = 0; i < num; i++)
					{
						string text = null;
						string text2 = null;
						ICatalogObject catalogObject = (ICatalogObject)coll.Item(i);
						string text3 = (string)catalogObject.GetValue("Name");
						int num2 = (int)catalogObject.GetValue("ErrorCode");
						if (!Platform.IsLessThan(Platform.W2K))
						{
							text = (string)catalogObject.GetValue("MajorRef");
							text2 = (string)catalogObject.GetValue("MinorRef");
						}
						array[i] = new RegistrationErrorInfo(text, text2, text3, num2);
					}
				}
				array2 = array;
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_ErrCollectionErr"), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.BuildErrorInfoChain"));
			}
			return array2;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00008484 File Offset: 0x00007484
		private static void RegisterTypeLibrary(string tlb)
		{
			IntPtr intPtr = (IntPtr)0;
			tlb = Path.GetFullPath(tlb);
			int num = Util.LoadTypeLibEx(tlb, 1, out intPtr);
			if (num < 0 || intPtr == (IntPtr)0)
			{
				Exception exceptionForHR = Marshal.GetExceptionForHR(num);
				throw new RegistrationException(Resource.FormatString("Reg_TypeLibRegErr", tlb), exceptionForHR);
			}
			num = Util.RegisterTypeLib(intPtr, tlb, Path.GetDirectoryName(tlb));
			if (num < 0 || intPtr == (IntPtr)0)
			{
				Exception exceptionForHR2 = Marshal.GetExceptionForHR(num);
				throw new RegistrationException(Resource.FormatString("Reg_TypeLibRegErr", tlb), exceptionForHR2);
			}
			Marshal.Release(intPtr);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00008518 File Offset: 0x00007518
		private RegistrationException WrapCOMException(ICatalogCollection coll, COMException e, string msg)
		{
			RegistrationErrorInfo[] array = null;
			if (e.ErrorCode == Util.COMADMIN_E_OBJECTERRORS)
			{
				ICatalogCollection catalogCollection;
				if (coll == null)
				{
					catalogCollection = (ICatalogCollection)this._cat.GetCollection("ErrorInfo");
				}
				else
				{
					catalogCollection = (ICatalogCollection)coll.GetCollection("ErrorInfo", "");
				}
				if (catalogCollection != null)
				{
					array = RegistrationDriver.BuildErrorInfoChain(catalogCollection);
				}
			}
			return new RegistrationException(msg, array);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00008578 File Offset: 0x00007578
		internal void ReportWarning(string msg)
		{
			if ((this._installFlags & InstallationFlags.ReportWarningsToConsole) != InstallationFlags.Default)
			{
				Console.WriteLine(msg);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000858C File Offset: 0x0000758C
		public void CheckForAppSecurityAttribute(Assembly asm)
		{
			object[] customAttributes = asm.GetCustomAttributes(typeof(ApplicationAccessControlAttribute), true);
			if (customAttributes.Length > 0)
			{
				return;
			}
			this.ReportWarning(Resource.FormatString("Reg_NoApplicationSecurity"));
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000085C4 File Offset: 0x000075C4
		public void CheckAssemblySCValidity(Assembly asm)
		{
			Type[] types = asm.GetTypes();
			bool flag = true;
			ArrayList arrayList = null;
			RegistrationServices registrationServices = new RegistrationServices();
			foreach (Type type in types)
			{
				if (type.IsClass && type.IsSubclassOf(typeof(ServicedComponent)))
				{
					if (!registrationServices.TypeRequiresRegistration(type) && !type.IsAbstract)
					{
						flag = false;
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						RegistrationErrorInfo registrationErrorInfo = new RegistrationErrorInfo(null, null, type.ToString(), -2147467259);
						arrayList.Add(registrationErrorInfo);
					}
					ClassInterfaceType classInterfaceType = ServicedComponentInfo.GetClassInterfaceType(type);
					MethodInfo[] methods = type.GetMethods();
					foreach (MethodInfo methodInfo in methods)
					{
						if (ReflectionCache.ConvertToInterfaceMI(methodInfo) == null)
						{
							if (ServicedComponentInfo.HasSpecialMethodAttributes(methodInfo))
							{
								this.ReportWarning(Resource.FormatString("Reg_NoClassInterfaceSecure", type.FullName, methodInfo.Name));
							}
							if (classInterfaceType == ClassInterfaceType.AutoDispatch && ServicedComponentInfo.IsMethodAutoDone(methodInfo))
							{
								this.ReportWarning(Resource.FormatString("Reg_NoClassInterface", type.FullName, methodInfo.Name));
							}
						}
					}
				}
			}
			if (!flag)
			{
				RegistrationErrorInfo[] array3 = (RegistrationErrorInfo[])arrayList.ToArray(typeof(RegistrationErrorInfo));
				throw new RegistrationException(Resource.FormatString("Reg_InvalidServicedComponents"), array3);
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000871F File Offset: 0x0000771F
		internal bool AssemblyHasStrongName(Assembly asm)
		{
			return asm.GetName().GetPublicKeyToken().Length > 0;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00008734 File Offset: 0x00007734
		internal Assembly NewLoadAssembly(string assembly)
		{
			Assembly assembly2;
			if (!File.Exists(assembly))
			{
				assembly2 = Assembly.Load(assembly);
				this.CheckAssemblySCValidity(assembly2);
			}
			else
			{
				assembly2 = this.LoadAssembly(assembly);
				this.CheckAssemblySCValidity(assembly2);
				if (!this.AssemblyHasStrongName(assembly2))
				{
					throw new RegistrationException(Resource.FormatString("Reg_NoStrongName", assembly));
				}
			}
			return assembly2;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00008784 File Offset: 0x00007784
		internal Assembly LoadAssembly(string assembly)
		{
			assembly = Path.GetFullPath(assembly).ToLower(CultureInfo.InvariantCulture);
			bool flag = false;
			string text = null;
			string directoryName = Path.GetDirectoryName(assembly);
			text = Environment.CurrentDirectory;
			if (text != directoryName)
			{
				Environment.CurrentDirectory = directoryName;
				flag = true;
			}
			Assembly assembly2 = null;
			try
			{
				assembly2 = Assembly.LoadFrom(assembly);
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_AssemblyLoadErr", assembly), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.LoadAssembly"));
			}
			if (flag)
			{
				Environment.CurrentDirectory = text;
			}
			if (assembly2 == null)
			{
				throw new RegistrationException(Resource.FormatString("Reg_AssemblyLoadErr", assembly));
			}
			if (assembly2.GetName().Name == "System.EnterpriseServices")
			{
				throw new RegistrationException(Resource.FormatString("RegSvcs_NoBootstrap"));
			}
			return assembly2;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000885C File Offset: 0x0000785C
		internal static object GenerateTypeLibrary(Assembly asm, string tlb, Report report)
		{
			object obj2;
			try
			{
				TypeLibConverter typeLibConverter = new TypeLibConverter();
				RegistrationExporterNotifySink registrationExporterNotifySink = new RegistrationExporterNotifySink(tlb, report);
				object obj = typeLibConverter.ConvertAssemblyToTypeLib(asm, tlb, TypeLibExporterFlags.OnlyReferenceRegistered, registrationExporterNotifySink);
				ICreateTypeLib createTypeLib = (ICreateTypeLib)obj;
				createTypeLib.SaveAllChanges();
				RegistrationDriver.RegisterTypeLibrary(tlb);
				obj2 = obj;
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_TypeLibGenErr", tlb, asm), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.GenerateTypeLibrary"));
			}
			return obj2;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000088E4 File Offset: 0x000078E4
		private void PostProcessApplication(ICatalogObject app, ApplicationSpec spec)
		{
			try
			{
				if (this.AfterSaveChanges(spec.Assembly, app, this._appColl, "Application", this._cache))
				{
					RegistrationDriver.SaveChanges(this._appColl);
				}
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_ConfigUnkErr"), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PostProcessApplication"));
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00008964 File Offset: 0x00007964
		private ICatalogObject CreateApplication(ApplicationSpec spec, bool checkExistence)
		{
			if (checkExistence)
			{
				ICatalogObject catalogObject = this.FindApplication(this._appColl, spec);
				if (catalogObject != null)
				{
					throw new RegistrationException(Resource.FormatString("Reg_AppExistsErr", spec));
				}
			}
			ICatalogObject catalogObject2 = (ICatalogObject)this._appColl.Add();
			this.CheckForAppSecurityAttribute(spec.Assembly);
			this.ApplicationDefaults(catalogObject2, this._appColl);
			catalogObject2.SetValue("Name", spec.Name);
			if (spec.ID != null)
			{
				catalogObject2.SetValue("ID", spec.ID);
			}
			if (spec.AppRootDir != null)
			{
				Platform.Assert(Platform.Whistler, "ApplicationRootDirectory");
				catalogObject2.SetValue("ApplicationDirectory", spec.AppRootDir);
			}
			RegistrationDriver.SaveChanges(this._appColl);
			this.ConfigureObject(spec.Assembly, catalogObject2, this._appColl, "Application", this._cache);
			spec.Name = (string)catalogObject2.GetValue("Name");
			RegistrationDriver.SaveChanges(this._appColl);
			return catalogObject2;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008A60 File Offset: 0x00007A60
		private ICatalogObject FindOrCreateApplication(ApplicationSpec spec, bool configure)
		{
			ICatalogObject catalogObject = this.FindApplication(this._appColl, spec);
			if (catalogObject == null)
			{
				catalogObject = this.CreateApplication(spec, false);
			}
			else if (configure)
			{
				this.CheckForAppSecurityAttribute(spec.Assembly);
				this.ApplicationDefaults(catalogObject, this._appColl);
				catalogObject.SetValue("Name", spec.Name);
				if (!Platform.IsLessThan(Platform.Whistler))
				{
					catalogObject.SetValue("ApplicationDirectory", (spec.AppRootDir == null) ? "" : spec.AppRootDir);
				}
				this.ConfigureObject(spec.Assembly, catalogObject, this._appColl, "Application", this._cache);
				spec.Name = (string)catalogObject.GetValue("Name");
				RegistrationDriver.SaveChanges(this._appColl);
			}
			return catalogObject;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008B29 File Offset: 0x00007B29
		private void InstallTypeLibrary(ApplicationSpec spec)
		{
			if (Platform.IsLessThan(Platform.W2K))
			{
				this.InstallTypeLibrary_MTS(spec);
				return;
			}
			this.InstallTypeLibrary_W2K(spec);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008B48 File Offset: 0x00007B48
		private void InstallTypeLibrary_W2K(ApplicationSpec spec)
		{
			try
			{
				object[] array = new object[] { spec.TypeLib };
				Type[] array2 = spec.NormalTypes;
				if (array2 != null)
				{
					if (array2 == null || array2.Length == 0)
					{
						throw new RegistrationException(Resource.FormatString("Reg_NoConfigTypesErr"));
					}
					object[] array3 = new object[array2.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						array3[i] = "{" + Marshal.GenerateGuidForType(array2[i]).ToString() + "}";
					}
					this._cat.InstallMultipleComponents(spec.DefinitiveName, ref array, ref array3);
				}
				array2 = spec.EventTypes;
				if (array2 != null)
				{
					if (array2 == null || array2.Length == 0)
					{
						throw new RegistrationException(Resource.FormatString("Reg_NoConfigTypesErr"));
					}
					object[] array4 = new object[array2.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						array4[j] = "{" + Marshal.GenerateGuidForType(array2[j]).ToString() + "}";
					}
					this._cat.InstallMultipleEventClasses(spec.DefinitiveName, ref array, ref array4);
				}
			}
			catch (COMException ex)
			{
				throw this.WrapCOMException(null, ex, Resource.FormatString("Reg_TypeLibInstallErr", spec.TypeLib, spec.Name));
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00008C9C File Offset: 0x00007C9C
		private void InstallTypeLibrary_MTS(ApplicationSpec spec)
		{
			ICatalogCollection catalogCollection = null;
			try
			{
				ICatalogObject catalogObject = this.FindApplication(this._appColl, spec);
				catalogCollection = (ICatalogCollection)this._appColl.GetCollection(CollectionName.Components, catalogObject.Key());
				RegistrationDriver.Populate(catalogCollection);
				IComponentUtil componentUtil = (IComponentUtil)catalogCollection.GetUtilInterface();
				foreach (Type type in spec.NormalTypes)
				{
					Guid guid = Marshal.GenerateGuidForType(type);
					bool flag = false;
					for (int j = 0; j < catalogCollection.Count(); j++)
					{
						ICatalogObject catalogObject2 = (ICatalogObject)catalogCollection.Item(j);
						Guid guid2 = new Guid((string)catalogObject2.Key());
						if (guid2 == guid)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						componentUtil.ImportComponent("{" + guid + "}");
					}
				}
			}
			catch (COMException ex)
			{
				throw this.WrapCOMException(catalogCollection, ex, Resource.FormatString("Reg_TypeLibInstallErr", spec.TypeLib, spec.Name));
			}
			catch (Exception ex2)
			{
				throw new RegistrationException(Resource.FormatString("Reg_TypeLibInstallErr", spec.TypeLib, spec.Name), ex2);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.InstallTypeLibrary_MTS"));
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00008E00 File Offset: 0x00007E00
		private ICatalogObject FindApplication(ICatalogCollection apps, ApplicationSpec spec)
		{
			for (int i = 0; i < apps.Count(); i++)
			{
				ICatalogObject catalogObject = (ICatalogObject)apps.Item(i);
				if (spec.Matches(catalogObject))
				{
					return catalogObject;
				}
			}
			return null;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008E38 File Offset: 0x00007E38
		private void ApplicationDefaults(ICatalogObject obj, ICatalogCollection coll)
		{
			if (Platform.IsLessThan(Platform.W2K))
			{
				obj.SetValue("Activation", "Inproc");
				obj.SetValue("SecurityEnabled", "N");
				obj.SetValue("Authentication", AuthenticationOption.Packet);
			}
			else
			{
				obj.SetValue("Activation", ActivationOption.Library);
				obj.SetValue("AccessChecksLevel", AccessChecksLevelOption.Application);
				obj.SetValue("ApplicationAccessChecksEnabled", true);
				obj.SetValue("Authentication", AuthenticationOption.Packet);
				obj.SetValue("CRMEnabled", false);
				obj.SetValue("EventsEnabled", true);
				obj.SetValue("ImpersonationLevel", ImpersonationLevelOption.Impersonate);
				obj.SetValue("QueuingEnabled", false);
				obj.SetValue("QueueListenerEnabled", false);
			}
			if (!Platform.IsLessThan(Platform.Whistler))
			{
				obj.SetValue("SoapActivated", false);
				obj.SetValue("QCListenerMaxThreads", 0);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008F50 File Offset: 0x00007F50
		internal bool ConfigureObject(ICustomAttributeProvider t, ICatalogObject obj, ICatalogCollection coll, string prefix, Hashtable cache)
		{
			bool flag = false;
			object[] customAttributes = t.GetCustomAttributes(true);
			cache[prefix] = obj;
			cache[prefix + "Type"] = t;
			cache[prefix + "Collection"] = coll;
			cache["CurrentTarget"] = prefix;
			foreach (object obj2 in customAttributes)
			{
				if (obj2 is IConfigurationAttribute)
				{
					try
					{
						IConfigurationAttribute configurationAttribute = (IConfigurationAttribute)obj2;
						if (configurationAttribute.IsValidTarget(prefix) && configurationAttribute.Apply(cache))
						{
							flag = true;
						}
					}
					catch (Exception ex)
					{
						throw new RegistrationException(Resource.FormatString("Reg_ComponentAttrErr", obj.Name(), obj2), ex);
					}
					catch
					{
						throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.ConfigureObject"));
					}
				}
			}
			return flag;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00009038 File Offset: 0x00008038
		internal bool AfterSaveChanges(ICustomAttributeProvider t, ICatalogObject obj, ICatalogCollection coll, string prefix, Hashtable cache)
		{
			bool flag = false;
			object[] customAttributes = t.GetCustomAttributes(true);
			cache[prefix] = obj;
			cache[prefix + "Type"] = t;
			cache[prefix + "Collection"] = coll;
			cache["CurrentTarget"] = prefix;
			foreach (object obj2 in customAttributes)
			{
				if (obj2 is IConfigurationAttribute)
				{
					IConfigurationAttribute configurationAttribute = (IConfigurationAttribute)obj2;
					if (configurationAttribute.IsValidTarget(prefix) && configurationAttribute.AfterSaveChanges(cache))
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000090D4 File Offset: 0x000080D4
		internal void ConfigureCollection(ICatalogCollection coll, IConfigCallback cb)
		{
			bool flag = false;
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			foreach (object obj in cb)
			{
				object obj2 = cb.FindObject(coll, obj);
				cb.ConfigureDefaults(obj2, obj);
			}
			RegistrationDriver.SaveChanges(coll);
			flag = false;
			foreach (object obj3 in cb)
			{
				object obj4 = cb.FindObject(coll, obj3);
				if (cb.Configure(obj4, obj3))
				{
					flag = true;
				}
			}
			RegistrationDriver.SaveChanges(coll);
			flag = false;
			foreach (object obj5 in cb)
			{
				object obj6 = cb.FindObject(coll, obj5);
				if (cb.AfterSaveChanges(obj6, obj5))
				{
					flag = true;
				}
			}
			if (flag)
			{
				RegistrationDriver.SaveChanges(coll);
			}
			cb.ConfigureSubCollections(coll);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000921C File Offset: 0x0000821C
		private void ConfigureComponents(ApplicationSpec spec)
		{
			ICatalogCollection catalogCollection = null;
			try
			{
				ICatalogObject catalogObject = this.FindApplication(this._appColl, spec);
				if (catalogObject == null)
				{
					throw new RegistrationException(Resource.FormatString("Reg_AppNotFoundErr", spec));
				}
				this._cache["Application"] = catalogObject;
				this._cache["ApplicationType"] = spec.Assembly;
				this._cache["ApplicationCollection"] = this._appColl;
				catalogCollection = (ICatalogCollection)this._appColl.GetCollection(CollectionName.Components, catalogObject.Key());
				this.ConfigureCollection(catalogCollection, new ComponentConfigCallback(catalogCollection, spec, this._cache, this, this._installFlags));
			}
			catch (RegistrationException)
			{
				throw;
			}
			catch (COMException ex)
			{
				throw this.WrapCOMException(catalogCollection, ex, Resource.FormatString("Reg_ConfigErr"));
			}
			catch (Exception ex2)
			{
				throw new RegistrationException(Resource.FormatString("Reg_ConfigUnkErr"), ex2);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.ConfigureComponents"));
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00009338 File Offset: 0x00008338
		internal bool IsAssemblyRegistered(ApplicationSpec spec)
		{
			bool flag = false;
			if (spec == null || spec.ConfigurableTypes == null)
			{
				return false;
			}
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("CLSID");
			if (registryKey == null)
			{
				throw new RegistrationException(Resource.FormatString("Reg_RegistryErr"));
			}
			foreach (Type type in spec.ConfigurableTypes)
			{
				string text = "{" + Marshal.GenerateGuidForType(type).ToString() + "}";
				RegistryKey registryKey2 = null;
				RegistryKey registryKey3 = null;
				try
				{
					registryKey2 = registryKey.OpenSubKey(text);
					if (registryKey2 != null)
					{
						registryKey3 = registryKey2.OpenSubKey("InprocServer32");
						if (registryKey3 != null && registryKey3.GetValue("Assembly") != null && registryKey3.GetValue("Class") != null)
						{
							flag = true;
							break;
						}
					}
				}
				catch
				{
				}
				finally
				{
					if (registryKey3 != null)
					{
						registryKey3.Close();
					}
					if (registryKey2 != null)
					{
						registryKey2.Close();
					}
				}
			}
			registryKey.Close();
			return flag;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000944C File Offset: 0x0000844C
		internal void UnregisterAssembly(Assembly asm, ApplicationSpec spec)
		{
			bool flag = true;
			if (asm == null)
			{
				return;
			}
			if (spec == null || spec.ConfigurableTypes == null)
			{
				return;
			}
			if (!Platform.IsLessThan(Platform.Whistler) && this._cat != null)
			{
				foreach (Type type in spec.ConfigurableTypes)
				{
					string text = "{" + Marshal.GenerateGuidForType(type).ToString() + "}";
					try
					{
						int num = 0;
						Type type2 = this._cat.GetType();
						try
						{
							Type type3 = type2;
							string text2 = "GetComponentVersions";
							BindingFlags bindingFlags = BindingFlags.InvokeMethod;
							Binder binder = null;
							object cat = this._cat;
							object[] array = new object[5];
							array[0] = text;
							num = (int)this.InvokeMemberHelper(type3, text2, bindingFlags, binder, cat, array);
						}
						catch (COMException ex)
						{
							if (Util.DISP_E_UNKNOWNNAME != ex.ErrorCode)
							{
								throw;
							}
							num = (int)this.InvokeMemberHelper(type2, "GetComponentVersionCount", BindingFlags.InvokeMethod, null, this._cat, new object[] { text });
						}
						if (num > 0)
						{
							flag = false;
							break;
						}
					}
					catch (COMException ex2)
					{
						if (Util.REGDB_E_CLASSNOTREG != ex2.ErrorCode)
						{
							throw;
						}
					}
				}
			}
			if (flag)
			{
				this.ClassicUnregistration(asm);
				try
				{
					this.UnregisterTypeLib(asm);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000095B0 File Offset: 0x000085B0
		internal void ClassicRegistration(Assembly asm)
		{
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Demand();
			registryPermission.Assert();
			try
			{
				RegistrationServices registrationServices = new RegistrationServices();
				registrationServices.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_AssemblyRegErr", asm), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.ClassicRegistration"));
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00009628 File Offset: 0x00008628
		internal void ClassicUnregistration(Assembly asm)
		{
			try
			{
				new RegistrationServices().UnregisterAssembly(asm);
			}
			catch (Exception ex)
			{
				throw new RegistrationException(Resource.FormatString("Reg_AssemblyUnregErr", asm), ex);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.ClassicUnregistration"));
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00009688 File Offset: 0x00008688
		internal void UnregisterTypeLib(Assembly asm)
		{
			IntPtr zero = IntPtr.Zero;
			object obj = null;
			ITypeLib typeLib = null;
			try
			{
				Guid typeLibGuidForAssembly = Marshal.GetTypeLibGuidForAssembly(asm);
				Version version = asm.GetName().Version;
				if (version.Major == 0 && version.Minor == 0)
				{
					version = new Version(1, 0);
				}
				if (Util.LoadRegTypeLib(typeLibGuidForAssembly, (short)version.Major, (short)version.Minor, 0, out obj) == 0)
				{
					typeLib = (ITypeLib)obj;
					typeLib.GetLibAttr(out zero);
					global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR)Marshal.PtrToStructure(zero, typeof(global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR));
					Util.UnRegisterTypeLib(typelibattr.guid, typelibattr.wMajorVerNum, typelibattr.wMinorVerNum, typelibattr.lcid, typelibattr.syskind);
				}
			}
			finally
			{
				if (typeLib != null && zero != IntPtr.Zero)
				{
					typeLib.ReleaseTLibAttr(zero);
				}
				if (typeLib != null)
				{
					Marshal.ReleaseComObject(typeLib);
				}
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009770 File Offset: 0x00008770
		private object InvokeMemberHelper(Type type, string name, BindingFlags invokeAttr, Binder binder, object target, object[] args)
		{
			object obj;
			try
			{
				obj = type.InvokeMember(name, invokeAttr, binder, target, args, CultureInfo.InvariantCulture);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return obj;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000097AC File Offset: 0x000087AC
		private void PrepDriver(ref ApplicationSpec spec)
		{
			if (Platform.IsLessThan(Platform.W2K))
			{
				try
				{
					this._cat = null;
					this._mts = (IMtsCatalog)new xMtsCatalog();
					this._appColl = (ICatalogCollection)this._mts.GetCollection(CollectionName.Applications);
					RegistrationDriver.Populate(this._appColl);
					goto IL_030F;
				}
				catch (Exception ex)
				{
					throw new RegistrationException(Resource.FormatString("Reg_CatalogErr"), ex);
				}
				catch
				{
					throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PrepDriver"));
				}
			}
			if (Platform.IsLessThan(Platform.Whistler))
			{
				try
				{
					this._mts = null;
					this._cat = (ICatalog)new xCatalog();
					this._appColl = (ICatalogCollection)this._cat.GetCollection(CollectionName.Applications);
					RegistrationDriver.Populate(this._appColl);
					goto IL_030F;
				}
				catch (Exception ex2)
				{
					throw new RegistrationException(Resource.FormatString("Reg_CatalogErr"), ex2);
				}
				catch
				{
					throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PrepDriver"));
				}
			}
			try
			{
				this._cat = (ICatalog)new xCatalog();
			}
			catch (Exception ex3)
			{
				throw new RegistrationException(Resource.FormatString("Reg_CatalogErr"), ex3);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PrepDriver"));
			}
			if ((spec.Partition == null || spec.Partition.Length == 0) && spec.ID != null)
			{
				try
				{
					Type type = this._cat.GetType();
					try
					{
						spec.Partition = (string)this.InvokeMemberHelper(type, "GetAppPartitionId", BindingFlags.InvokeMethod, null, this._cat, new object[] { spec.ID });
					}
					catch (COMException ex4)
					{
						if (Util.DISP_E_UNKNOWNNAME == ex4.ErrorCode)
						{
							spec.Partition = (string)this.InvokeMemberHelper(type, "GetPartitionID", BindingFlags.InvokeMethod, null, this._cat, new object[] { spec.ID });
						}
					}
				}
				catch
				{
				}
			}
			if (spec.Partition != null && spec.Partition.Length != 0)
			{
				try
				{
					Type type2 = this._cat.GetType();
					try
					{
						this.InvokeMemberHelper(type2, "SetApplicationPartition", BindingFlags.InvokeMethod, null, this._cat, new object[] { spec.Partition });
					}
					catch (COMException ex5)
					{
						if (Util.DISP_E_UNKNOWNNAME != ex5.ErrorCode)
						{
							throw;
						}
						this.InvokeMemberHelper(type2, "CurrentPartition", BindingFlags.SetProperty, null, this._cat, new object[] { spec.Partition });
					}
				}
				catch (Exception ex6)
				{
					throw new RegistrationException(Resource.FormatString("Reg_PartitionErr", spec.Partition), ex6);
				}
				catch
				{
					throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PrepDriver"));
				}
			}
			try
			{
				this._mts = null;
				this._appColl = (ICatalogCollection)this._cat.GetCollection(CollectionName.Applications);
				RegistrationDriver.Populate(this._appColl);
			}
			catch (Exception ex7)
			{
				throw new RegistrationException(Resource.FormatString("Reg_CatalogErr"), ex7);
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.PrepDriver"));
			}
			IL_030F:
			this._cache = new Hashtable();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00009B74 File Offset: 0x00008B74
		private void CleanupDriver()
		{
			this._cat = null;
			this._cache = null;
			this._appColl = null;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00009B8C File Offset: 0x00008B8C
		private void PrepArguments(RegistrationConfig regConfig)
		{
			if (regConfig.AssemblyFile == null || regConfig.AssemblyFile.Length == 0)
			{
				throw new RegistrationException(Resource.FormatString("Reg_ArgumentAssembly"));
			}
			if ((regConfig.InstallationFlags & InstallationFlags.ExpectExistingTypeLib) != InstallationFlags.Default && (regConfig.TypeLibrary == null || regConfig.TypeLibrary.Length == 0))
			{
				throw new RegistrationException(Resource.FormatString("Reg_ExpectExisting"));
			}
			if ((regConfig.InstallationFlags & InstallationFlags.CreateTargetApplication) != InstallationFlags.Default && (regConfig.InstallationFlags & InstallationFlags.FindOrCreateTargetApplication) != InstallationFlags.Default)
			{
				throw new RegistrationException(Resource.FormatString("Reg_CreateFlagErr"));
			}
			if ((regConfig.InstallationFlags & InstallationFlags.Register) == InstallationFlags.Default && (regConfig.InstallationFlags & InstallationFlags.Install) == InstallationFlags.Default && (regConfig.InstallationFlags & InstallationFlags.Configure) == InstallationFlags.Default)
			{
				regConfig.InstallationFlags |= InstallationFlags.Register | InstallationFlags.Install | InstallationFlags.Configure;
			}
			this._installFlags = regConfig.InstallationFlags;
			if (Platform.IsLessThan(Platform.W2K))
			{
				this._installFlags |= InstallationFlags.ConfigureComponentsOnly;
			}
			if (regConfig.Partition != null && regConfig.Partition.Length != 0)
			{
				string text = "Base Application Partition";
				string text2 = "{41E90F3E-56C1-4633-81C3-6E8BAC8BDD70}";
				if (string.Compare(regConfig.Partition, text2, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(regConfig.Partition, text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					regConfig.Partition = null;
				}
				if (regConfig.Partition != null && Platform.IsLessThan(Platform.Whistler))
				{
					throw new RegistrationException(Resource.FormatString("Reg_PartitionsNotSupported"));
				}
			}
			if (regConfig.ApplicationRootDirectory != null && !Directory.Exists(regConfig.ApplicationRootDirectory))
			{
				throw new RegistrationException(Resource.FormatString("Reg_BadAppRootDir"));
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00009D04 File Offset: 0x00008D04
		private bool ValidateBitness(ApplicationSpec spec, out string message)
		{
			bool flag = true;
			message = string.Empty;
			if (!Wow64Helper.IsWow64Supported())
			{
				return flag;
			}
			bool flag2 = Wow64Helper.IsWow64Process();
			ICatalogObject catalogObject = this.FindApplication(this._appColl, spec);
			if (catalogObject == null)
			{
				return flag;
			}
			ICatalogCollection catalogCollection = (ICatalogCollection)this._appColl.GetCollection(CollectionName.Components, catalogObject.Key());
			RegistrationDriver.Populate(catalogCollection);
			int num = catalogCollection.Count();
			if (num <= 0)
			{
				return flag;
			}
			Guid[] array = new Guid[spec.ConfigurableTypes.Length];
			for (int i = 0; i < spec.ConfigurableTypes.Length; i++)
			{
				array[i] = Marshal.GenerateGuidForType(spec.ConfigurableTypes[i]);
			}
			for (int j = 0; j < num; j++)
			{
				ICatalogObject catalogObject2 = (ICatalogObject)catalogCollection.Item(j);
				string text = (string)catalogObject2.Key();
				Guid guid = new Guid(text);
				if (this.FindIndexOf(array, guid) != -1)
				{
					int num2 = (int)catalogObject2.GetValue("Bitness");
					if (flag2 && num2 == 2)
					{
						message = Resource.FormatString("Reg_Already64bit");
						flag = false;
						break;
					}
					if (!flag2 && num2 == 1)
					{
						message = Resource.FormatString("Reg_Already32bit");
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00009E3C File Offset: 0x00008E3C
		public void InstallAssembly(RegistrationConfig regConfig, object obSync)
		{
			Assembly assembly = null;
			ApplicationSpec applicationSpec = null;
			CatalogSync catalogSync = null;
			bool flag = false;
			bool flag2 = false;
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			try
			{
				securityPermission.Demand();
				securityPermission.Assert();
				ICatalogObject catalogObject = null;
				this.PrepArguments(regConfig);
				assembly = this.NewLoadAssembly(regConfig.AssemblyFile);
				applicationSpec = new ApplicationSpec(assembly, regConfig);
				if (applicationSpec.ConfigurableTypes == null)
				{
					regConfig.Application = null;
					regConfig.TypeLibrary = null;
				}
				else
				{
					if (obSync != null)
					{
						if (!(obSync is CatalogSync))
						{
							throw new ArgumentException(Resource.FormatString("Err_obSync"));
						}
						catalogSync = (CatalogSync)obSync;
					}
					this.PrepDriver(ref applicationSpec);
					string empty = string.Empty;
					if (!this.ValidateBitness(applicationSpec, out empty))
					{
						throw new RegistrationException(empty);
					}
					if ((regConfig.InstallationFlags & InstallationFlags.Register) != InstallationFlags.Default)
					{
						flag = !this.IsAssemblyRegistered(applicationSpec);
						this.ClassicRegistration(applicationSpec.Assembly);
						if ((regConfig.InstallationFlags & InstallationFlags.ExpectExistingTypeLib) != InstallationFlags.Default)
						{
							RegistrationDriver.RegisterTypeLibrary(applicationSpec.TypeLib);
						}
						else
						{
							flag2 = true;
							RegistrationDriver.GenerateTypeLibrary(applicationSpec.Assembly, applicationSpec.TypeLib, new Report(this.ReportWarning));
						}
					}
					if ((regConfig.InstallationFlags & InstallationFlags.Install) != InstallationFlags.Default && applicationSpec.ConfigurableTypes != null)
					{
						if ((regConfig.InstallationFlags & InstallationFlags.CreateTargetApplication) != InstallationFlags.Default)
						{
							catalogObject = this.CreateApplication(applicationSpec, true);
						}
						else if ((regConfig.InstallationFlags & InstallationFlags.FindOrCreateTargetApplication) != InstallationFlags.Default)
						{
							catalogObject = this.FindOrCreateApplication(applicationSpec, (regConfig.InstallationFlags & InstallationFlags.ReconfigureExistingApplication) != InstallationFlags.Default);
						}
						this.InstallTypeLibrary(applicationSpec);
						if (catalogSync != null)
						{
							catalogSync.Set();
						}
					}
					if ((regConfig.InstallationFlags & InstallationFlags.Configure) != InstallationFlags.Default && applicationSpec.ConfigurableTypes != null)
					{
						this.ConfigureComponents(applicationSpec);
						if (catalogSync != null)
						{
							catalogSync.Set();
						}
					}
					if (catalogObject != null)
					{
						this.PostProcessApplication(catalogObject, applicationSpec);
					}
					this.CleanupDriver();
				}
			}
			catch (Exception ex)
			{
				if (ex is SecurityException || ex is UnauthorizedAccessException || (ex.InnerException != null && (ex.InnerException is SecurityException || ex.InnerException is UnauthorizedAccessException)))
				{
					ex = new RegistrationException(Resource.FormatString("Reg_Unauthorized"), ex);
				}
				if (flag && assembly != null)
				{
					try
					{
						this.ClassicUnregistration(assembly);
					}
					catch
					{
					}
				}
				if (flag2 && assembly != null)
				{
					try
					{
						this.UnregisterTypeLib(assembly);
					}
					catch
					{
					}
				}
				throw ex;
			}
			catch
			{
				throw new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationDriver.InstallAssembly"));
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000A0D0 File Offset: 0x000090D0
		private int FindIndexOf(string[] arr, string key)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == key)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000A0FC File Offset: 0x000090FC
		private int FindIndexOf(Guid[] arr, Guid key)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == key)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000A130 File Offset: 0x00009130
		public void UninstallAssembly(RegistrationConfig regConfig, object obSync)
		{
			CatalogSync catalogSync = null;
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			if (obSync != null)
			{
				if (!(obSync is CatalogSync))
				{
					throw new ArgumentException(Resource.FormatString("Err_obSync"));
				}
				catalogSync = (CatalogSync)obSync;
			}
			Assembly assembly = this.NewLoadAssembly(regConfig.AssemblyFile);
			ApplicationSpec applicationSpec = new ApplicationSpec(assembly, regConfig);
			if (applicationSpec.ConfigurableTypes == null)
			{
				return;
			}
			this.PrepDriver(ref applicationSpec);
			if (applicationSpec.ConfigurableTypes != null)
			{
				ICatalogObject catalogObject = this.FindApplication(this._appColl, applicationSpec);
				if (catalogObject == null)
				{
					throw new RegistrationException(Resource.FormatString("Reg_AppNotFoundErr", applicationSpec));
				}
				ICatalogCollection catalogCollection = (ICatalogCollection)this._appColl.GetCollection(CollectionName.Components, catalogObject.Key());
				string[] array = new string[applicationSpec.ConfigurableTypes.Length];
				int num = 0;
				foreach (Type type in applicationSpec.ConfigurableTypes)
				{
					array[num] = Marshal.GenerateGuidForType(type).ToString();
					num++;
				}
				RegistrationDriver.Populate(catalogCollection);
				bool flag = true;
				int j = 0;
				while (j < catalogCollection.Count())
				{
					ICatalogObject catalogObject2 = (ICatalogObject)catalogCollection.Item(j);
					string text = (string)catalogObject2.Key();
					text = new Guid(text).ToString();
					if (this.FindIndexOf(array, text) != -1)
					{
						catalogCollection.Remove(j);
						if (catalogSync != null)
						{
							catalogSync.Set();
						}
					}
					else
					{
						j++;
						flag = false;
					}
				}
				RegistrationDriver.SaveChanges(catalogCollection);
				if (flag)
				{
					int k = 0;
					while (k < this._appColl.Count())
					{
						ICatalogObject catalogObject3 = (ICatalogObject)this._appColl.Item(k);
						if (catalogObject3.Key().Equals(catalogObject.Key()))
						{
							this._appColl.Remove(k);
							if (catalogSync != null)
							{
								catalogSync.Set();
								break;
							}
							break;
						}
						else
						{
							k++;
						}
					}
					RegistrationDriver.SaveChanges(this._appColl);
				}
			}
			this.UnregisterAssembly(assembly, applicationSpec);
			this.CleanupDriver();
		}

		// Token: 0x04000139 RID: 313
		private ICatalog _cat;

		// Token: 0x0400013A RID: 314
		private IMtsCatalog _mts;

		// Token: 0x0400013B RID: 315
		private ICatalogCollection _appColl;

		// Token: 0x0400013C RID: 316
		private Hashtable _cache;

		// Token: 0x0400013D RID: 317
		private InstallationFlags _installFlags;
	}
}
