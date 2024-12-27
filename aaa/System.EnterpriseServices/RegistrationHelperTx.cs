using System;
using System.Diagnostics;
using System.EnterpriseServices.Admin;
using System.EnterpriseServices.Thunk;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.EnterpriseServices
{
	// Token: 0x0200008D RID: 141
	[Transaction(TransactionOption.RequiresNew)]
	[Guid("c89ac250-e18a-4fc7-abd5-b8897b6a78a5")]
	public sealed class RegistrationHelperTx : ServicedComponent
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000AB84 File Offset: 0x00009B84
		static RegistrationHelperTx()
		{
			if (Wow64Helper.IsWow64Process())
			{
				RegistrationHelperTx._appid = RegistrationHelperTx._appidWow64;
				RegistrationHelperTx._appname = RegistrationHelperTx._appnameWow64;
				RegistrationHelperTx._isRunningInWow64 = true;
				return;
			}
			RegistrationHelperTx._appid = RegistrationHelperTx._appidNoWow64;
			RegistrationHelperTx._appname = RegistrationHelperTx._appnameNoWow64;
			RegistrationHelperTx._isRunningInWow64 = false;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000AC00 File Offset: 0x00009C00
		private static ICatalogObject FindApplication(ICatalogCollection coll, Guid appid, ref int idx)
		{
			int num = coll.Count();
			for (int i = 0; i < num; i++)
			{
				ICatalogObject catalogObject = (ICatalogObject)coll.Item(i);
				Guid guid = new Guid((string)catalogObject.GetValue("ID"));
				if (guid == appid)
				{
					idx = i;
					return catalogObject;
				}
			}
			return null;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000AC54 File Offset: 0x00009C54
		private static ICatalogObject FindComponent(ICatalogCollection coll, Guid clsid, ref int idx)
		{
			RegistrationDriver.Populate(coll);
			int num = coll.Count();
			for (int i = 0; i < num; i++)
			{
				ICatalogObject catalogObject = (ICatalogObject)coll.Item(i);
				Guid guid = new Guid((string)catalogObject.GetValue("CLSID"));
				if (guid == clsid)
				{
					idx = i;
					return catalogObject;
				}
			}
			return null;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000ACAD File Offset: 0x00009CAD
		private static void ConfigureComponent(ICatalogCollection coll, ICatalogObject obj)
		{
			obj.SetValue("Transaction", TransactionOption.RequiresNew);
			obj.SetValue("ComponentTransactionTimeoutEnabled", true);
			obj.SetValue("ComponentTransactionTimeout", 0);
			coll.SaveChanges();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000ACEC File Offset: 0x00009CEC
		[ComRegisterFunction]
		internal static void InstallUtilityApplication(Type t)
		{
			if (!Platform.Supports(PlatformFeature.SWC))
			{
				try
				{
					if (!Platform.IsLessThan(Platform.W2K))
					{
						ICatalog catalog = null;
						ICatalogCollection catalogCollection = null;
						ICatalogObject catalogObject = null;
						int num = 0;
						catalog = (ICatalog)new xCatalog();
						if (!Platform.IsLessThan(Platform.Whistler))
						{
							ICatalog2 catalog2 = catalog as ICatalog2;
							if (catalog2 != null)
							{
								catalog2.CurrentPartition(catalog2.GlobalPartitionID());
							}
						}
						catalogCollection = (ICatalogCollection)catalog.GetCollection("Applications");
						RegistrationDriver.Populate(catalogCollection);
						catalogObject = RegistrationHelperTx.FindApplication(catalogCollection, RegistrationHelperTx._appid, ref num);
						if (catalogObject == null)
						{
							catalogObject = (ICatalogObject)catalogCollection.Add();
							catalogObject.SetValue("Name", RegistrationHelperTx._appname);
							catalogObject.SetValue("Activation", ActivationOption.Library);
							catalogObject.SetValue("ID", "{" + RegistrationHelperTx._appid.ToString() + "}");
							if (!Platform.IsLessThan(Platform.Whistler))
							{
								try
								{
									catalogObject.SetValue("Replicable", 0);
								}
								catch
								{
								}
							}
							catalogCollection.SaveChanges();
						}
						else
						{
							catalogObject.SetValue("Changeable", true);
							catalogObject.SetValue("Deleteable", true);
							catalogCollection.SaveChanges();
							catalogObject.SetValue("Name", RegistrationHelperTx._appname);
							if (!Platform.IsLessThan(Platform.Whistler))
							{
								try
								{
									catalogObject.SetValue("Replicable", 0);
								}
								catch
								{
								}
							}
							catalogCollection.SaveChanges();
						}
						Guid guid = Marshal.GenerateGuidForType(typeof(RegistrationHelperTx));
						ICatalogCollection catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection("Components", catalogObject.Key());
						ICatalogObject catalogObject2 = RegistrationHelperTx.FindComponent(catalogCollection2, guid, ref num);
						if (catalogObject2 == null)
						{
							if (RegistrationHelperTx._isRunningInWow64)
							{
								ICatalog2 catalog3 = catalog as ICatalog2;
								string text = "{" + guid + "}";
								int num2 = 1;
								object obj = text;
								object obj2 = num2;
								catalog3.ImportComponents("{" + RegistrationHelperTx._appid + "}", ref obj, ref obj2);
							}
							else
							{
								catalog.ImportComponent("{" + RegistrationHelperTx._appid + "}", "{" + guid + "}");
							}
							catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection("Components", catalogObject.Key());
							catalogObject2 = RegistrationHelperTx.FindComponent(catalogCollection2, guid, ref num);
						}
						RegistrationHelperTx.ConfigureComponent(catalogCollection2, catalogObject2);
						catalogObject.SetValue("Changeable", false);
						catalogObject.SetValue("Deleteable", false);
						catalogCollection.SaveChanges();
						Proxy.RegisterProxyStub();
						RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
						registryPermission.Demand();
						registryPermission.Assert();
						RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\MICROSOFT\\OLE\\NONREDIST");
						registryKey.SetValue("System.EnterpriseServices.Thunk.dll", "");
						registryKey.Close();
					}
				}
				catch (Exception ex)
				{
					try
					{
						EventLog eventLog = new EventLog();
						eventLog.Source = "System.EnterpriseServices";
						string text2 = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrInstSysEnt"), new object[] { ex });
						eventLog.WriteEntry(text2, EventLogEntryType.Error);
					}
					catch
					{
					}
				}
				catch
				{
					try
					{
						EventLog eventLog2 = new EventLog();
						eventLog2.Source = "System.EnterpriseServices";
						string text3 = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxInst"), new object[] { Resource.FormatString("Err_NonClsException", "RegistrationHelperTx.InstallUtilityApplication") });
						eventLog2.WriteEntry(text3, EventLogEntryType.Error);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000B108 File Offset: 0x0000A108
		[ComUnregisterFunction]
		internal static void UninstallUtilityApplication(Type t)
		{
			if (!Platform.Supports(PlatformFeature.SWC))
			{
				try
				{
					if (!Platform.IsLessThan(Platform.W2K))
					{
						int num = 0;
						ICatalog catalog = (ICatalog)new xCatalog();
						if (!Platform.IsLessThan(Platform.Whistler))
						{
							ICatalog2 catalog2 = catalog as ICatalog2;
							if (catalog2 != null)
							{
								catalog2.CurrentPartition(catalog2.GlobalPartitionID());
							}
						}
						ICatalogCollection catalogCollection = (ICatalogCollection)catalog.GetCollection("Applications");
						RegistrationDriver.Populate(catalogCollection);
						ICatalogObject catalogObject = RegistrationHelperTx.FindApplication(catalogCollection, RegistrationHelperTx._appid, ref num);
						if (catalogObject != null)
						{
							catalogObject.SetValue("Changeable", true);
							catalogObject.SetValue("Deleteable", true);
							catalogCollection.SaveChanges();
							int num2 = 0;
							Guid guid = Marshal.GenerateGuidForType(typeof(RegistrationHelperTx));
							ICatalogCollection catalogCollection2 = (ICatalogCollection)catalogCollection.GetCollection("Components", catalogObject.Key());
							ICatalogObject catalogObject2 = RegistrationHelperTx.FindComponent(catalogCollection2, guid, ref num2);
							int num3 = catalogCollection2.Count();
							if (catalogObject2 != null)
							{
								catalogCollection2.Remove(num2);
								catalogCollection2.SaveChanges();
							}
							if (catalogObject2 != null && num3 == 1)
							{
								catalogCollection.Remove(num);
								catalogCollection.SaveChanges();
							}
							else
							{
								catalogObject.SetValue("Changeable", false);
								catalogObject.SetValue("Deleteable", false);
								catalogCollection.SaveChanges();
							}
						}
					}
				}
				catch (Exception ex)
				{
					try
					{
						EventLog eventLog = new EventLog();
						eventLog.Source = "System.EnterpriseServices";
						string text = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrUninstSysEnt"), new object[] { ex });
						eventLog.WriteEntry(text, EventLogEntryType.Error);
					}
					catch
					{
					}
				}
				catch
				{
					try
					{
						EventLog eventLog2 = new EventLog();
						eventLog2.Source = "System.EnterpriseServices";
						string text2 = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxInst"), new object[] { Resource.FormatString("Err_NonClsException", "RegistrationHelperTx.UninstallUtilityApplication") });
						eventLog2.WriteEntry(text2, EventLogEntryType.Error);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000B368 File Offset: 0x0000A368
		public void InstallAssembly(string assembly, ref string application, ref string tlb, InstallationFlags installFlags, object sync)
		{
			this.InstallAssembly(assembly, ref application, null, ref tlb, installFlags, sync);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000B378 File Offset: 0x0000A378
		public void InstallAssembly(string assembly, ref string application, string partition, ref string tlb, InstallationFlags installFlags, object sync)
		{
			RegistrationConfig registrationConfig = new RegistrationConfig();
			registrationConfig.AssemblyFile = assembly;
			registrationConfig.Application = application;
			registrationConfig.Partition = partition;
			registrationConfig.TypeLibrary = tlb;
			registrationConfig.InstallationFlags = installFlags;
			this.InstallAssemblyFromConfig(ref registrationConfig, sync);
			application = registrationConfig.AssemblyFile;
			tlb = registrationConfig.TypeLibrary;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000B3D0 File Offset: 0x0000A3D0
		public void InstallAssemblyFromConfig([MarshalAs(UnmanagedType.IUnknown)] ref RegistrationConfig regConfig, object sync)
		{
			bool flag = false;
			try
			{
				RegistrationDriver registrationDriver = new RegistrationDriver();
				registrationDriver.InstallAssembly(regConfig, sync);
				ContextUtil.SetComplete();
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					ContextUtil.SetAbort();
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000B410 File Offset: 0x0000A410
		public void UninstallAssembly(string assembly, string application, object sync)
		{
			this.UninstallAssembly(assembly, application, null, sync);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000B41C File Offset: 0x0000A41C
		public void UninstallAssembly(string assembly, string application, string partition, object sync)
		{
			RegistrationConfig registrationConfig = new RegistrationConfig();
			registrationConfig.AssemblyFile = assembly;
			registrationConfig.Application = application;
			registrationConfig.Partition = partition;
			this.UninstallAssemblyFromConfig(ref registrationConfig, sync);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000B450 File Offset: 0x0000A450
		public void UninstallAssemblyFromConfig([MarshalAs(UnmanagedType.IUnknown)] ref RegistrationConfig regConfig, object sync)
		{
			bool flag = false;
			try
			{
				RegistrationDriver registrationDriver = new RegistrationDriver();
				registrationDriver.UninstallAssembly(regConfig, sync);
				ContextUtil.SetComplete();
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					ContextUtil.SetAbort();
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000B490 File Offset: 0x0000A490
		public bool IsInTransaction()
		{
			return ContextUtil.IsInTransaction;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000B497 File Offset: 0x0000A497
		protected internal override void Activate()
		{
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000B499 File Offset: 0x0000A499
		protected internal override void Deactivate()
		{
		}

		// Token: 0x0400014B RID: 331
		private static Guid _appid;

		// Token: 0x0400014C RID: 332
		private static string _appname;

		// Token: 0x0400014D RID: 333
		private static bool _isRunningInWow64;

		// Token: 0x0400014E RID: 334
		private static Guid _appidNoWow64 = new Guid("1e246775-2281-484f-8ad4-044c15b86eb7");

		// Token: 0x0400014F RID: 335
		private static string _appnameNoWow64 = ".NET Utilities";

		// Token: 0x04000150 RID: 336
		private static Guid _appidWow64 = new Guid("57926702-ab7c-402b-abce-e262da1dd7c9");

		// Token: 0x04000151 RID: 337
		private static string _appnameWow64 = ".NET Utilities (32 bit)";
	}
}
