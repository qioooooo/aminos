using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.ServiceProcess
{
	// Token: 0x0200002E RID: 46
	public class ServiceInstaller : ComponentInstaller
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x000050F8 File Offset: 0x000040F8
		public ServiceInstaller()
		{
			this.eventLogInstaller = new EventLogInstaller();
			this.eventLogInstaller.Log = "Application";
			this.eventLogInstaller.Source = "";
			this.eventLogInstaller.UninstallAction = UninstallAction.Remove;
			base.Installers.Add(this.eventLogInstaller);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005188 File Offset: 0x00004188
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00005190 File Offset: 0x00004190
		[DefaultValue("")]
		[ServiceProcessDescription("ServiceInstallerDisplayName")]
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.displayName = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000051A3 File Offset: 0x000041A3
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000051AB File Offset: 0x000041AB
		[ComVisible(false)]
		[ServiceProcessDescription("ServiceInstallerDescription")]
		[DefaultValue("")]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.description = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000051BE File Offset: 0x000041BE
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000051C6 File Offset: 0x000041C6
		[ServiceProcessDescription("ServiceInstallerServicesDependedOn")]
		public string[] ServicesDependedOn
		{
			get
			{
				return this.servicesDependedOn;
			}
			set
			{
				if (value == null)
				{
					value = new string[0];
				}
				this.servicesDependedOn = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000051DA File Offset: 0x000041DA
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000051E4 File Offset: 0x000041E4
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ServiceProcessDescription("ServiceInstallerServiceName")]
		public string ServiceName
		{
			get
			{
				return this.serviceName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!ServiceController.ValidServiceName(value))
				{
					throw new ArgumentException(Res.GetString("ServiceName", new object[]
					{
						value,
						80.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.serviceName = value;
				this.eventLogInstaller.Source = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005244 File Offset: 0x00004244
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000524C File Offset: 0x0000424C
		[DefaultValue(ServiceStartMode.Manual)]
		[ServiceProcessDescription("ServiceInstallerStartType")]
		public ServiceStartMode StartType
		{
			get
			{
				return this.startType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ServiceStartMode), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ServiceStartMode));
				}
				this.startType = value;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005284 File Offset: 0x00004284
		internal static void CheckEnvironment()
		{
			if (ServiceInstaller.environmentChecked)
			{
				if (ServiceInstaller.isWin9x)
				{
					throw new PlatformNotSupportedException(Res.GetString("CantControlOnWin9x"));
				}
				return;
			}
			else
			{
				ServiceInstaller.isWin9x = Environment.OSVersion.Platform != PlatformID.Win32NT;
				ServiceInstaller.environmentChecked = true;
				if (ServiceInstaller.isWin9x)
				{
					throw new PlatformNotSupportedException(Res.GetString("CantInstallOnWin9x"));
				}
				return;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000052E4 File Offset: 0x000042E4
		public override void CopyFromComponent(IComponent component)
		{
			if (!(component is ServiceBase))
			{
				throw new ArgumentException(Res.GetString("NotAService"));
			}
			ServiceBase serviceBase = (ServiceBase)component;
			this.ServiceName = serviceBase.ServiceName;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000531C File Offset: 0x0000431C
		public override void Install(IDictionary stateSaver)
		{
			base.Context.LogMessage(Res.GetString("InstallingService", new object[] { this.ServiceName }));
			try
			{
				ServiceInstaller.CheckEnvironment();
				string text = null;
				string text2 = null;
				ServiceProcessInstaller serviceProcessInstaller = null;
				if (base.Parent is ServiceProcessInstaller)
				{
					serviceProcessInstaller = (ServiceProcessInstaller)base.Parent;
				}
				else
				{
					for (int i = 0; i < base.Parent.Installers.Count; i++)
					{
						if (base.Parent.Installers[i] is ServiceProcessInstaller)
						{
							serviceProcessInstaller = (ServiceProcessInstaller)base.Parent.Installers[i];
							break;
						}
					}
				}
				if (serviceProcessInstaller == null)
				{
					throw new InvalidOperationException(Res.GetString("NoInstaller"));
				}
				switch (serviceProcessInstaller.Account)
				{
				case ServiceAccount.LocalService:
					text = "NT AUTHORITY\\LocalService";
					break;
				case ServiceAccount.NetworkService:
					text = "NT AUTHORITY\\NetworkService";
					break;
				case ServiceAccount.User:
					text = serviceProcessInstaller.Username;
					text2 = serviceProcessInstaller.Password;
					break;
				}
				string text3 = base.Context.Parameters["assemblypath"];
				if (text3 == null || text3.Length == 0)
				{
					throw new InvalidOperationException(Res.GetString("FileName"));
				}
				text3 = "\"" + text3 + "\"";
				if (!ServiceInstaller.ValidateServiceName(this.ServiceName))
				{
					throw new InvalidOperationException(Res.GetString("ServiceName", new object[]
					{
						this.ServiceName,
						80.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.DisplayName.Length > 255)
				{
					throw new ArgumentException(Res.GetString("DisplayNameTooLong", new object[] { this.DisplayName }));
				}
				string text4 = null;
				if (this.ServicesDependedOn.Length > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int j = 0; j < this.ServicesDependedOn.Length; j++)
					{
						string text5 = this.ServicesDependedOn[j];
						try
						{
							ServiceController serviceController = new ServiceController(text5, ".");
							text5 = serviceController.ServiceName;
						}
						catch
						{
						}
						stringBuilder.Append(text5);
						stringBuilder.Append('\0');
					}
					stringBuilder.Append('\0');
					text4 = stringBuilder.ToString();
				}
				IntPtr intPtr = SafeNativeMethods.OpenSCManager(null, null, 983103);
				IntPtr intPtr2 = IntPtr.Zero;
				if (intPtr == IntPtr.Zero)
				{
					throw new InvalidOperationException(Res.GetString("OpenSC", new object[] { "." }), new Win32Exception());
				}
				int num = 16;
				int num2 = 0;
				for (int k = 0; k < base.Parent.Installers.Count; k++)
				{
					if (base.Parent.Installers[k] is ServiceInstaller)
					{
						num2++;
						if (num2 > 1)
						{
							break;
						}
					}
				}
				if (num2 > 1)
				{
					num = 32;
				}
				try
				{
					intPtr2 = NativeMethods.CreateService(intPtr, this.ServiceName, this.DisplayName, 983551, num, (int)this.StartType, 1, text3, null, IntPtr.Zero, text4, text, text2);
					if (intPtr2 == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
					if (this.Description.Length != 0)
					{
						NativeMethods.SERVICE_DESCRIPTION service_DESCRIPTION = default(NativeMethods.SERVICE_DESCRIPTION);
						service_DESCRIPTION.description = Marshal.StringToHGlobalUni(this.Description);
						bool flag = NativeMethods.ChangeServiceConfig2(intPtr2, 1U, ref service_DESCRIPTION);
						Marshal.FreeHGlobal(service_DESCRIPTION.description);
						if (!flag)
						{
							throw new Win32Exception();
						}
					}
					stateSaver["installed"] = true;
				}
				finally
				{
					if (intPtr2 != IntPtr.Zero)
					{
						SafeNativeMethods.CloseServiceHandle(intPtr2);
					}
					SafeNativeMethods.CloseServiceHandle(intPtr);
				}
				base.Context.LogMessage(Res.GetString("InstallOK", new object[] { this.ServiceName }));
			}
			finally
			{
				base.Install(stateSaver);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000572C File Offset: 0x0000472C
		public override bool IsEquivalentInstaller(ComponentInstaller otherInstaller)
		{
			ServiceInstaller serviceInstaller = otherInstaller as ServiceInstaller;
			return serviceInstaller != null && serviceInstaller.ServiceName == this.ServiceName;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005758 File Offset: 0x00004758
		private void RemoveService()
		{
			base.Context.LogMessage(Res.GetString("ServiceRemoving", new object[] { this.ServiceName }));
			IntPtr intPtr = SafeNativeMethods.OpenSCManager(null, null, 983103);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				intPtr2 = NativeMethods.OpenService(intPtr, this.ServiceName, 65536);
				if (intPtr2 == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				NativeMethods.DeleteService(intPtr2);
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					SafeNativeMethods.CloseServiceHandle(intPtr2);
				}
				SafeNativeMethods.CloseServiceHandle(intPtr);
			}
			base.Context.LogMessage(Res.GetString("ServiceRemoved", new object[] { this.ServiceName }));
			try
			{
				using (ServiceController serviceController = new ServiceController(this.ServiceName))
				{
					if (serviceController.Status != ServiceControllerStatus.Stopped)
					{
						base.Context.LogMessage(Res.GetString("TryToStop", new object[] { this.ServiceName }));
						serviceController.Stop();
						int num = 10;
						serviceController.Refresh();
						while (serviceController.Status != ServiceControllerStatus.Stopped && num > 0)
						{
							Thread.Sleep(1000);
							serviceController.Refresh();
							num--;
						}
					}
				}
			}
			catch
			{
			}
			Thread.Sleep(5000);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000058DC File Offset: 0x000048DC
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
			object obj = savedState["installed"];
			if (obj == null || !(bool)obj)
			{
				return;
			}
			this.RemoveService();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000590E File Offset: 0x0000490E
		private bool ShouldSerializeServicesDependedOn()
		{
			return this.servicesDependedOn != null && this.servicesDependedOn.Length > 0;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005926 File Offset: 0x00004926
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			this.RemoveService();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005938 File Offset: 0x00004938
		private static bool ValidateServiceName(string name)
		{
			if (name == null || name.Length == 0 || name.Length > 80)
			{
				return false;
			}
			char[] array = name.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < ' ' || array[i] == '/' || array[i] == '\\')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000216 RID: 534
		private const string NetworkServiceName = "NT AUTHORITY\\NetworkService";

		// Token: 0x04000217 RID: 535
		private const string LocalServiceName = "NT AUTHORITY\\LocalService";

		// Token: 0x04000218 RID: 536
		private EventLogInstaller eventLogInstaller;

		// Token: 0x04000219 RID: 537
		private string serviceName = "";

		// Token: 0x0400021A RID: 538
		private string displayName = "";

		// Token: 0x0400021B RID: 539
		private string description = "";

		// Token: 0x0400021C RID: 540
		private string[] servicesDependedOn = new string[0];

		// Token: 0x0400021D RID: 541
		private ServiceStartMode startType = ServiceStartMode.Manual;

		// Token: 0x0400021E RID: 542
		private static bool environmentChecked;

		// Token: 0x0400021F RID: 543
		private static bool isWin9x;
	}
}
