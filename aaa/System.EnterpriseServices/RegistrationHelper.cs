using System;
using System.Diagnostics;
using System.EnterpriseServices.Thunk;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x0200008B RID: 139
	[Guid("89a86e7b-c229-4008-9baa-2f5c8411d7e0")]
	public sealed class RegistrationHelper : MarshalByRefObject, IRegistrationHelper, IThunkInstallation
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000A5A0 File Offset: 0x000095A0
		void IThunkInstallation.DefaultInstall(string asm)
		{
			string text = null;
			string text2 = null;
			this.InstallAssembly(asm, ref text, ref text2, InstallationFlags.FindOrCreateTargetApplication | InstallationFlags.ReconfigureExistingApplication);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000A5BE File Offset: 0x000095BE
		public void InstallAssembly(string assembly, ref string application, ref string tlb, InstallationFlags installFlags)
		{
			this.InstallAssembly(assembly, ref application, null, ref tlb, installFlags);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000A5CC File Offset: 0x000095CC
		public void InstallAssembly(string assembly, ref string application, string partition, ref string tlb, InstallationFlags installFlags)
		{
			RegistrationConfig registrationConfig = new RegistrationConfig();
			registrationConfig.AssemblyFile = assembly;
			registrationConfig.Application = application;
			registrationConfig.Partition = partition;
			registrationConfig.TypeLibrary = tlb;
			registrationConfig.InstallationFlags = installFlags;
			this.InstallAssemblyFromConfig(ref registrationConfig);
			application = registrationConfig.Application;
			tlb = registrationConfig.TypeLibrary;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000A620 File Offset: 0x00009620
		public void InstallAssemblyFromConfig([MarshalAs(UnmanagedType.IUnknown)] ref RegistrationConfig regConfig)
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			Platform.Assert(Platform.W2K, "RegistrationHelper.InstallAssemblyFromConfig");
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
			{
				RegistrationThreadWrapper registrationThreadWrapper = new RegistrationThreadWrapper(this, regConfig);
				Thread thread = new Thread(new ThreadStart(registrationThreadWrapper.InstallThread));
				thread.Start();
				thread.Join();
				registrationThreadWrapper.PropInstallResult();
				return;
			}
			if (!Platform.Supports(PlatformFeature.SWC))
			{
				if (Platform.IsLessThan(Platform.W2K) || !this.TryTransactedInstall(regConfig))
				{
					RegistrationDriver registrationDriver = new RegistrationDriver();
					registrationDriver.InstallAssembly(regConfig, null);
					return;
				}
			}
			else
			{
				TransactionOptions transactionOptions = default(TransactionOptions);
				transactionOptions.Timeout = TimeSpan.FromSeconds(0.0);
				transactionOptions.IsolationLevel = IsolationLevel.Serializable;
				CatalogSync catalogSync = new CatalogSync();
				using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, EnterpriseServicesInteropOption.Full))
				{
					RegistrationDriver registrationDriver2 = new RegistrationDriver();
					registrationDriver2.InstallAssembly(regConfig, catalogSync);
					transactionScope.Complete();
				}
				catalogSync.Wait();
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000A72C File Offset: 0x0000972C
		public void UninstallAssembly(string assembly, string application)
		{
			this.UninstallAssembly(assembly, application, null);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000A738 File Offset: 0x00009738
		public void UninstallAssembly(string assembly, string application, string partition)
		{
			RegistrationConfig registrationConfig = new RegistrationConfig();
			registrationConfig.AssemblyFile = assembly;
			registrationConfig.Application = application;
			registrationConfig.Partition = partition;
			this.UninstallAssemblyFromConfig(ref registrationConfig);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000A768 File Offset: 0x00009768
		public void UninstallAssemblyFromConfig([MarshalAs(UnmanagedType.IUnknown)] ref RegistrationConfig regConfig)
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			Platform.Assert(Platform.W2K, "RegistrationHelper.UninstallAssemblyFromConfig");
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
			{
				RegistrationThreadWrapper registrationThreadWrapper = new RegistrationThreadWrapper(this, regConfig);
				Thread thread = new Thread(new ThreadStart(registrationThreadWrapper.UninstallThread));
				thread.Start();
				thread.Join();
				registrationThreadWrapper.PropUninstallResult();
				return;
			}
			if (!Platform.Supports(PlatformFeature.SWC))
			{
				if (Platform.IsLessThan(Platform.W2K) || !this.TryTransactedUninstall(regConfig))
				{
					RegistrationDriver registrationDriver = new RegistrationDriver();
					registrationDriver.UninstallAssembly(regConfig, null);
					return;
				}
			}
			else
			{
				TransactionOptions transactionOptions = default(TransactionOptions);
				transactionOptions.Timeout = TimeSpan.FromMinutes(0.0);
				transactionOptions.IsolationLevel = IsolationLevel.Serializable;
				CatalogSync catalogSync = new CatalogSync();
				using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, EnterpriseServicesInteropOption.Full))
				{
					RegistrationDriver registrationDriver2 = new RegistrationDriver();
					registrationDriver2.UninstallAssembly(regConfig, catalogSync);
					transactionScope.Complete();
				}
				catalogSync.Wait();
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000A874 File Offset: 0x00009874
		private bool TryTransactedInstall(RegistrationConfig regConfig)
		{
			RegistrationHelperTx registrationHelperTx = null;
			try
			{
				registrationHelperTx = new RegistrationHelperTx();
				if (!registrationHelperTx.IsInTransaction())
				{
					registrationHelperTx = null;
				}
			}
			catch (Exception ex)
			{
				try
				{
					EventLog eventLog = new EventLog();
					eventLog.Source = "System.EnterpriseServices";
					string text = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxInst"), new object[] { ex });
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
					string text2 = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxInst"), new object[] { Resource.FormatString("Err_NonClsException", "RegistrationHelper.TryTransactedInstall") });
					eventLog2.WriteEntry(text2, EventLogEntryType.Error);
				}
				catch
				{
				}
			}
			if (registrationHelperTx == null)
			{
				return false;
			}
			CatalogSync catalogSync = new CatalogSync();
			registrationHelperTx.InstallAssemblyFromConfig(ref regConfig, catalogSync);
			catalogSync.Wait();
			return true;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000A988 File Offset: 0x00009988
		private bool TryTransactedUninstall(RegistrationConfig regConfig)
		{
			RegistrationHelperTx registrationHelperTx = null;
			try
			{
				registrationHelperTx = new RegistrationHelperTx();
				if (!registrationHelperTx.IsInTransaction())
				{
					registrationHelperTx = null;
				}
			}
			catch (Exception ex)
			{
				try
				{
					EventLog eventLog = new EventLog();
					eventLog.Source = "System.EnterpriseServices";
					string text = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxUninst"), new object[] { ex });
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
					string text2 = string.Format(CultureInfo.CurrentCulture, Resource.FormatString("Reg_ErrTxInst"), new object[] { Resource.FormatString("Err_NonClsException", "RegistrationHelper.TryTransactedUninstall") });
					eventLog2.WriteEntry(text2, EventLogEntryType.Error);
				}
				catch
				{
				}
			}
			if (registrationHelperTx == null)
			{
				return false;
			}
			CatalogSync catalogSync = new CatalogSync();
			registrationHelperTx.UninstallAssemblyFromConfig(ref regConfig, catalogSync);
			catalogSync.Wait();
			return true;
		}
	}
}
