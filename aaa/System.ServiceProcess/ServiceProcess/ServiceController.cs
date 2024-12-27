using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.ServiceProcess.Design;
using System.Text;
using System.Threading;

namespace System.ServiceProcess
{
	// Token: 0x02000027 RID: 39
	[ServiceProcessDescription("ServiceControllerDesc")]
	[Designer("System.ServiceProcess.Design.ServiceControllerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ServiceController : Component
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003794 File Offset: 0x00002794
		private static object InternalSyncObject
		{
			get
			{
				if (ServiceController.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ServiceController.s_InternalSyncObject, obj, null);
				}
				return ServiceController.s_InternalSyncObject;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000037C0 File Offset: 0x000027C0
		public ServiceController()
		{
			this.type = 319;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000037FF File Offset: 0x000027FF
		public ServiceController(string name)
			: this(name, ".")
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003810 File Offset: 0x00002810
		public ServiceController(string name, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(Res.GetString("BadMachineName", new object[] { machineName }));
			}
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("InvalidParameter", new object[] { "name", name }));
			}
			this.machineName = machineName;
			this.eitherName = name;
			this.type = 319;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000038BC File Offset: 0x000028BC
		internal ServiceController(string machineName, NativeMethods.ENUM_SERVICE_STATUS status)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(Res.GetString("BadMachineName", new object[] { machineName }));
			}
			this.machineName = machineName;
			this.name = status.serviceName;
			this.displayName = status.displayName;
			this.commandsAccepted = status.controlsAccepted;
			this.status = (ServiceControllerStatus)status.currentState;
			this.type = status.serviceType;
			this.statusGenerated = true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000396C File Offset: 0x0000296C
		internal ServiceController(string machineName, NativeMethods.ENUM_SERVICE_STATUS_PROCESS status)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(Res.GetString("BadMachineName", new object[] { machineName }));
			}
			this.machineName = machineName;
			this.name = status.serviceName;
			this.displayName = status.displayName;
			this.commandsAccepted = status.controlsAccepted;
			this.status = (ServiceControllerStatus)status.currentState;
			this.type = status.serviceType;
			this.statusGenerated = true;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003A19 File Offset: 0x00002A19
		[ServiceProcessDescription("SPCanPauseAndContinue")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanPauseAndContinue
		{
			get
			{
				this.GenerateStatus();
				return (this.commandsAccepted & 2) != 0;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003A2F File Offset: 0x00002A2F
		[ServiceProcessDescription("SPCanShutdown")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanShutdown
		{
			get
			{
				this.GenerateStatus();
				return (this.commandsAccepted & 4) != 0;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003A45 File Offset: 0x00002A45
		[ServiceProcessDescription("SPCanStop")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanStop
		{
			get
			{
				this.GenerateStatus();
				return (this.commandsAccepted & 1) != 0;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003A5B File Offset: 0x00002A5B
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003A92 File Offset: 0x00002A92
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ServiceProcessDescription("SPDisplayName")]
		public string DisplayName
		{
			get
			{
				if (this.displayName.Length == 0 && (this.eitherName.Length > 0 || this.name.Length > 0))
				{
					this.GenerateNames();
				}
				return this.displayName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (string.Compare(value, this.displayName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.displayName = value;
					return;
				}
				this.Close();
				this.displayName = value;
				this.name = "";
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003AD4 File Offset: 0x00002AD4
		[ServiceProcessDescription("SPDependentServices")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ServiceController[] DependentServices
		{
			get
			{
				if (!this.browseGranted)
				{
					ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Browse, this.machineName, this.ServiceName);
					serviceControllerPermission.Demand();
					this.browseGranted = true;
				}
				if (this.dependentServices == null)
				{
					IntPtr serviceHandle = this.GetServiceHandle(8);
					try
					{
						int num = 0;
						int num2 = 0;
						bool flag = UnsafeNativeMethods.EnumDependentServices(serviceHandle, 3, (IntPtr)0, 0, ref num, ref num2);
						if (flag)
						{
							this.dependentServices = new ServiceController[0];
							return this.dependentServices;
						}
						if (Marshal.GetLastWin32Error() != 234)
						{
							throw ServiceController.CreateSafeWin32Exception();
						}
						IntPtr intPtr = Marshal.AllocHGlobal((IntPtr)num);
						try
						{
							if (!UnsafeNativeMethods.EnumDependentServices(serviceHandle, 3, intPtr, num, ref num, ref num2))
							{
								throw ServiceController.CreateSafeWin32Exception();
							}
							this.dependentServices = new ServiceController[num2];
							for (int i = 0; i < num2; i++)
							{
								NativeMethods.ENUM_SERVICE_STATUS enum_SERVICE_STATUS = new NativeMethods.ENUM_SERVICE_STATUS();
								IntPtr intPtr2 = (IntPtr)((long)intPtr + (long)(i * Marshal.SizeOf(typeof(NativeMethods.ENUM_SERVICE_STATUS))));
								Marshal.PtrToStructure(intPtr2, enum_SERVICE_STATUS);
								this.dependentServices[i] = new ServiceController(this.MachineName, enum_SERVICE_STATUS);
							}
						}
						finally
						{
							Marshal.FreeHGlobal(intPtr);
						}
					}
					finally
					{
						SafeNativeMethods.CloseServiceHandle(serviceHandle);
					}
				}
				return this.dependentServices;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003C28 File Offset: 0x00002C28
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003C30 File Offset: 0x00002C30
		[ServiceProcessDescription("SPMachineName")]
		[RecommendedAsConfigurable(true)]
		[Browsable(false)]
		[DefaultValue(".")]
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(Res.GetString("BadMachineName", new object[] { value }));
				}
				if (string.Compare(this.machineName, value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.machineName = value;
					return;
				}
				this.Close();
				this.machineName = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003C85 File Offset: 0x00002C85
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003CBC File Offset: 0x00002CBC
		[DefaultValue("")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter(typeof(ServiceNameConverter))]
		[ReadOnly(true)]
		[ServiceProcessDescription("SPServiceName")]
		public string ServiceName
		{
			get
			{
				if (this.name.Length == 0 && (this.eitherName.Length > 0 || this.displayName.Length > 0))
				{
					this.GenerateNames();
				}
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (string.Compare(value, this.name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.name = value;
					return;
				}
				if (!ServiceController.ValidServiceName(value))
				{
					throw new ArgumentException(Res.GetString("ServiceName", new object[]
					{
						value,
						80.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.Close();
				this.name = value;
				this.displayName = "";
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003D3C File Offset: 0x00002D3C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ServiceProcessDescription("SPServicesDependedOn")]
		public unsafe ServiceController[] ServicesDependedOn
		{
			get
			{
				if (!this.browseGranted)
				{
					ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Browse, this.machineName, this.ServiceName);
					serviceControllerPermission.Demand();
					this.browseGranted = true;
				}
				if (this.servicesDependedOn != null)
				{
					return this.servicesDependedOn;
				}
				IntPtr serviceHandle = this.GetServiceHandle(1);
				ServiceController[] array;
				try
				{
					int num = 0;
					bool flag = UnsafeNativeMethods.QueryServiceConfig(serviceHandle, (IntPtr)0, 0, out num);
					if (flag)
					{
						this.servicesDependedOn = new ServiceController[0];
						array = this.servicesDependedOn;
					}
					else
					{
						if (Marshal.GetLastWin32Error() != 122)
						{
							throw ServiceController.CreateSafeWin32Exception();
						}
						IntPtr intPtr = Marshal.AllocHGlobal((IntPtr)num);
						try
						{
							if (!UnsafeNativeMethods.QueryServiceConfig(serviceHandle, intPtr, num, out num))
							{
								throw ServiceController.CreateSafeWin32Exception();
							}
							NativeMethods.QUERY_SERVICE_CONFIG query_SERVICE_CONFIG = new NativeMethods.QUERY_SERVICE_CONFIG();
							Marshal.PtrToStructure(intPtr, query_SERVICE_CONFIG);
							char* ptr = query_SERVICE_CONFIG.lpDependencies;
							Hashtable hashtable = new Hashtable();
							if (ptr != null)
							{
								StringBuilder stringBuilder = new StringBuilder();
								while (*ptr != '\0')
								{
									stringBuilder.Append(*ptr);
									ptr++;
									if (*ptr == '\0')
									{
										string text = stringBuilder.ToString();
										stringBuilder = new StringBuilder();
										ptr++;
										if (text.StartsWith("+", StringComparison.Ordinal))
										{
											NativeMethods.ENUM_SERVICE_STATUS_PROCESS[] servicesInGroup = ServiceController.GetServicesInGroup(this.machineName, text.Substring(1));
											foreach (NativeMethods.ENUM_SERVICE_STATUS_PROCESS enum_SERVICE_STATUS_PROCESS in servicesInGroup)
											{
												if (!hashtable.Contains(enum_SERVICE_STATUS_PROCESS.serviceName))
												{
													hashtable.Add(enum_SERVICE_STATUS_PROCESS.serviceName, new ServiceController(this.MachineName, enum_SERVICE_STATUS_PROCESS));
												}
											}
										}
										else if (!hashtable.Contains(text))
										{
											hashtable.Add(text, new ServiceController(text, this.MachineName));
										}
									}
								}
							}
							this.servicesDependedOn = new ServiceController[hashtable.Count];
							hashtable.Values.CopyTo(this.servicesDependedOn, 0);
							array = this.servicesDependedOn;
						}
						finally
						{
							Marshal.FreeHGlobal(intPtr);
						}
					}
				}
				finally
				{
					SafeNativeMethods.CloseServiceHandle(serviceHandle);
				}
				return array;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003F5C File Offset: 0x00002F5C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public SafeHandle ServiceHandle
		{
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return new SafeServiceHandle(this.GetServiceHandle(983551), true);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003F7A File Offset: 0x00002F7A
		[ServiceProcessDescription("SPStatus")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ServiceControllerStatus Status
		{
			get
			{
				this.GenerateStatus();
				return this.status;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003F88 File Offset: 0x00002F88
		[ServiceProcessDescription("SPServiceType")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ServiceType ServiceType
		{
			get
			{
				this.GenerateStatus();
				return (ServiceType)this.type;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003F98 File Offset: 0x00002F98
		private static void CheckEnvironment()
		{
			if (ServiceController.environment == ServiceController.UnknownEnvironment)
			{
				lock (ServiceController.InternalSyncObject)
				{
					if (ServiceController.environment == ServiceController.UnknownEnvironment)
					{
						if (Environment.OSVersion.Platform == PlatformID.Win32NT)
						{
							ServiceController.environment = ServiceController.NtEnvironment;
						}
						else
						{
							ServiceController.environment = ServiceController.NonNtEnvironment;
						}
					}
				}
			}
			if (ServiceController.environment == ServiceController.NonNtEnvironment)
			{
				throw new PlatformNotSupportedException(Res.GetString("CantControlOnWin9x"));
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004024 File Offset: 0x00003024
		public void Close()
		{
			if (this.serviceManagerHandle != (IntPtr)0)
			{
				SafeNativeMethods.CloseServiceHandle(this.serviceManagerHandle);
			}
			this.serviceManagerHandle = (IntPtr)0;
			this.statusGenerated = false;
			this.type = 319;
			this.browseGranted = false;
			this.controlGranted = false;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000407C File Offset: 0x0000307C
		private static Win32Exception CreateSafeWin32Exception()
		{
			Win32Exception ex = null;
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				ex = new Win32Exception();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return ex;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000040B8 File Offset: 0x000030B8
		protected override void Dispose(bool disposing)
		{
			this.Close();
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000040D0 File Offset: 0x000030D0
		private unsafe void GenerateStatus()
		{
			if (!this.statusGenerated)
			{
				if (!this.browseGranted)
				{
					ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Browse, this.machineName, this.ServiceName);
					serviceControllerPermission.Demand();
					this.browseGranted = true;
				}
				IntPtr serviceHandle = this.GetServiceHandle(4);
				try
				{
					NativeMethods.SERVICE_STATUS service_STATUS = default(NativeMethods.SERVICE_STATUS);
					if (!UnsafeNativeMethods.QueryServiceStatus(serviceHandle, &service_STATUS))
					{
						throw ServiceController.CreateSafeWin32Exception();
					}
					this.commandsAccepted = service_STATUS.controlsAccepted;
					this.status = (ServiceControllerStatus)service_STATUS.currentState;
					this.type = service_STATUS.serviceType;
					this.statusGenerated = true;
				}
				finally
				{
					SafeNativeMethods.CloseServiceHandle(serviceHandle);
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000417C File Offset: 0x0000317C
		private void GenerateNames()
		{
			if (this.machineName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("NoMachineName"));
			}
			this.GetDataBaseHandleWithConnectAccess();
			if (this.name.Length == 0)
			{
				string text = this.eitherName;
				if (text.Length == 0)
				{
					text = this.displayName;
				}
				if (text.Length == 0)
				{
					throw new InvalidOperationException(Res.GetString("NoGivenName"));
				}
				int num = 256;
				StringBuilder stringBuilder = new StringBuilder(num);
				bool flag = SafeNativeMethods.GetServiceKeyName(this.serviceManagerHandle, text, stringBuilder, ref num);
				if (flag)
				{
					this.name = stringBuilder.ToString();
					this.displayName = text;
					this.eitherName = "";
				}
				else
				{
					flag = SafeNativeMethods.GetServiceDisplayName(this.serviceManagerHandle, text, stringBuilder, ref num);
					if (!flag && num >= 256)
					{
						stringBuilder = new StringBuilder(++num);
						flag = SafeNativeMethods.GetServiceDisplayName(this.serviceManagerHandle, text, stringBuilder, ref num);
					}
					if (!flag)
					{
						Exception ex = ServiceController.CreateSafeWin32Exception();
						throw new InvalidOperationException(Res.GetString("NoService", new object[] { text, this.machineName }), ex);
					}
					this.name = text;
					this.displayName = stringBuilder.ToString();
					this.eitherName = "";
				}
			}
			if (this.displayName.Length == 0)
			{
				int num2 = 256;
				StringBuilder stringBuilder2 = new StringBuilder(num2);
				bool flag2 = SafeNativeMethods.GetServiceDisplayName(this.serviceManagerHandle, this.name, stringBuilder2, ref num2);
				if (!flag2 && num2 >= 256)
				{
					stringBuilder2 = new StringBuilder(++num2);
					flag2 = SafeNativeMethods.GetServiceDisplayName(this.serviceManagerHandle, this.name, stringBuilder2, ref num2);
				}
				if (!flag2)
				{
					Exception ex2 = ServiceController.CreateSafeWin32Exception();
					throw new InvalidOperationException(Res.GetString("NoDisplayName", new object[] { this.name, this.machineName }), ex2);
				}
				this.displayName = stringBuilder2.ToString();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000436C File Offset: 0x0000336C
		private static IntPtr GetDataBaseHandleWithAccess(string machineName, int serviceControlManaqerAccess)
		{
			ServiceController.CheckEnvironment();
			IntPtr intPtr = IntPtr.Zero;
			if (machineName.Equals(".") || machineName.Length == 0)
			{
				intPtr = SafeNativeMethods.OpenSCManager(null, null, serviceControlManaqerAccess);
			}
			else
			{
				intPtr = SafeNativeMethods.OpenSCManager(machineName, null, serviceControlManaqerAccess);
			}
			if (intPtr == (IntPtr)0)
			{
				Exception ex = ServiceController.CreateSafeWin32Exception();
				throw new InvalidOperationException(Res.GetString("OpenSC", new object[] { machineName }), ex);
			}
			return intPtr;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000043DF File Offset: 0x000033DF
		private void GetDataBaseHandleWithConnectAccess()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.serviceManagerHandle == (IntPtr)0)
			{
				this.serviceManagerHandle = ServiceController.GetDataBaseHandleWithAccess(this.MachineName, 1);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000441F File Offset: 0x0000341F
		private static IntPtr GetDataBaseHandleWithEnumerateAccess(string machineName)
		{
			return ServiceController.GetDataBaseHandleWithAccess(machineName, 4);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004428 File Offset: 0x00003428
		public static ServiceController[] GetDevices()
		{
			return ServiceController.GetDevices(".");
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004434 File Offset: 0x00003434
		public static ServiceController[] GetDevices(string machineName)
		{
			return ServiceController.GetServicesOfType(machineName, 11);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004440 File Offset: 0x00003440
		private IntPtr GetServiceHandle(int desiredAccess)
		{
			this.GetDataBaseHandleWithConnectAccess();
			IntPtr intPtr = UnsafeNativeMethods.OpenService(this.serviceManagerHandle, this.ServiceName, desiredAccess);
			if (intPtr == (IntPtr)0)
			{
				Exception ex = ServiceController.CreateSafeWin32Exception();
				throw new InvalidOperationException(Res.GetString("OpenService", new object[] { this.ServiceName, this.MachineName }), ex);
			}
			return intPtr;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000044A6 File Offset: 0x000034A6
		public static ServiceController[] GetServices()
		{
			return ServiceController.GetServices(".");
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000044B2 File Offset: 0x000034B2
		public static ServiceController[] GetServices(string machineName)
		{
			return ServiceController.GetServicesOfType(machineName, 48);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000044BC File Offset: 0x000034BC
		private static NativeMethods.ENUM_SERVICE_STATUS_PROCESS[] GetServicesInGroup(string machineName, string group)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			int num = 0;
			NativeMethods.ENUM_SERVICE_STATUS_PROCESS[] array;
			try
			{
				intPtr = ServiceController.GetDataBaseHandleWithEnumerateAccess(machineName);
				int num2;
				int num3;
				UnsafeNativeMethods.EnumServicesStatusEx(intPtr, 0, 48, 3, (IntPtr)0, 0, out num2, out num3, ref num, group);
				intPtr2 = Marshal.AllocHGlobal((IntPtr)num2);
				UnsafeNativeMethods.EnumServicesStatusEx(intPtr, 0, 48, 3, intPtr2, num2, out num2, out num3, ref num, group);
				int num4 = num3;
				array = new NativeMethods.ENUM_SERVICE_STATUS_PROCESS[num4];
				for (int i = 0; i < num4; i++)
				{
					IntPtr intPtr3 = (IntPtr)((long)intPtr2 + (long)(i * Marshal.SizeOf(typeof(NativeMethods.ENUM_SERVICE_STATUS_PROCESS))));
					NativeMethods.ENUM_SERVICE_STATUS_PROCESS enum_SERVICE_STATUS_PROCESS = new NativeMethods.ENUM_SERVICE_STATUS_PROCESS();
					Marshal.PtrToStructure(intPtr3, enum_SERVICE_STATUS_PROCESS);
					array[i] = enum_SERVICE_STATUS_PROCESS;
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr2);
				if (intPtr != (IntPtr)0)
				{
					SafeNativeMethods.CloseServiceHandle(intPtr);
				}
			}
			return array;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000045A0 File Offset: 0x000035A0
		private static ServiceController[] GetServicesOfType(string machineName, int serviceType)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(Res.GetString("BadMachineName", new object[] { machineName }));
			}
			ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Browse, machineName, "*");
			serviceControllerPermission.Demand();
			ServiceController.CheckEnvironment();
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			int num = 0;
			ServiceController[] array;
			try
			{
				intPtr = ServiceController.GetDataBaseHandleWithEnumerateAccess(machineName);
				int num2;
				int num3;
				UnsafeNativeMethods.EnumServicesStatus(intPtr, serviceType, 3, (IntPtr)0, 0, out num2, out num3, ref num);
				intPtr2 = Marshal.AllocHGlobal((IntPtr)num2);
				UnsafeNativeMethods.EnumServicesStatus(intPtr, serviceType, 3, intPtr2, num2, out num2, out num3, ref num);
				int num4 = num3;
				array = new ServiceController[num4];
				for (int i = 0; i < num4; i++)
				{
					IntPtr intPtr3 = (IntPtr)((long)intPtr2 + (long)(i * Marshal.SizeOf(typeof(NativeMethods.ENUM_SERVICE_STATUS))));
					NativeMethods.ENUM_SERVICE_STATUS enum_SERVICE_STATUS = new NativeMethods.ENUM_SERVICE_STATUS();
					Marshal.PtrToStructure(intPtr3, enum_SERVICE_STATUS);
					array[i] = new ServiceController(machineName, enum_SERVICE_STATUS);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr2);
				if (intPtr != (IntPtr)0)
				{
					SafeNativeMethods.CloseServiceHandle(intPtr);
				}
			}
			return array;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000046C4 File Offset: 0x000036C4
		public unsafe void Pause()
		{
			if (!this.controlGranted)
			{
				ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, this.machineName, this.ServiceName);
				serviceControllerPermission.Demand();
				this.controlGranted = true;
			}
			IntPtr serviceHandle = this.GetServiceHandle(64);
			try
			{
				NativeMethods.SERVICE_STATUS service_STATUS = default(NativeMethods.SERVICE_STATUS);
				if (!UnsafeNativeMethods.ControlService(serviceHandle, 2, &service_STATUS))
				{
					Exception ex = ServiceController.CreateSafeWin32Exception();
					throw new InvalidOperationException(Res.GetString("PauseService", new object[] { this.ServiceName, this.MachineName }), ex);
				}
			}
			finally
			{
				SafeNativeMethods.CloseServiceHandle(serviceHandle);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000476C File Offset: 0x0000376C
		public unsafe void Continue()
		{
			if (!this.controlGranted)
			{
				ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, this.machineName, this.ServiceName);
				serviceControllerPermission.Demand();
				this.controlGranted = true;
			}
			IntPtr serviceHandle = this.GetServiceHandle(64);
			try
			{
				NativeMethods.SERVICE_STATUS service_STATUS = default(NativeMethods.SERVICE_STATUS);
				if (!UnsafeNativeMethods.ControlService(serviceHandle, 3, &service_STATUS))
				{
					Exception ex = ServiceController.CreateSafeWin32Exception();
					throw new InvalidOperationException(Res.GetString("ResumeService", new object[] { this.ServiceName, this.MachineName }), ex);
				}
			}
			finally
			{
				SafeNativeMethods.CloseServiceHandle(serviceHandle);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004814 File Offset: 0x00003814
		public unsafe void ExecuteCommand(int command)
		{
			if (!this.controlGranted)
			{
				ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, this.machineName, this.ServiceName);
				serviceControllerPermission.Demand();
				this.controlGranted = true;
			}
			IntPtr serviceHandle = this.GetServiceHandle(256);
			try
			{
				NativeMethods.SERVICE_STATUS service_STATUS = default(NativeMethods.SERVICE_STATUS);
				if (!UnsafeNativeMethods.ControlService(serviceHandle, command, &service_STATUS))
				{
					Exception ex = ServiceController.CreateSafeWin32Exception();
					throw new InvalidOperationException(Res.GetString("ControlService", new object[] { this.ServiceName, this.MachineName }), ex);
				}
			}
			finally
			{
				SafeNativeMethods.CloseServiceHandle(serviceHandle);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000048BC File Offset: 0x000038BC
		public void Refresh()
		{
			this.statusGenerated = false;
			this.dependentServices = null;
			this.servicesDependedOn = null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000048D3 File Offset: 0x000038D3
		public void Start()
		{
			this.Start(new string[0]);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000048E4 File Offset: 0x000038E4
		public void Start(string[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (!this.controlGranted)
			{
				ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, this.machineName, this.ServiceName);
				serviceControllerPermission.Demand();
				this.controlGranted = true;
			}
			IntPtr serviceHandle = this.GetServiceHandle(16);
			try
			{
				IntPtr[] array = new IntPtr[args.Length];
				int i = 0;
				try
				{
					for (i = 0; i < args.Length; i++)
					{
						if (args[i] == null)
						{
							throw new ArgumentNullException(Res.GetString("ArgsCantBeNull"), "args");
						}
						array[i] = Marshal.StringToHGlobalUni(args[i]);
					}
				}
				catch
				{
					for (int j = 0; j < i; j++)
					{
						Marshal.FreeHGlobal(array[i]);
					}
					throw;
				}
				GCHandle gchandle = default(GCHandle);
				try
				{
					gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
					if (!UnsafeNativeMethods.StartService(serviceHandle, args.Length, gchandle.AddrOfPinnedObject()))
					{
						Exception ex = ServiceController.CreateSafeWin32Exception();
						throw new InvalidOperationException(Res.GetString("CannotStart", new object[] { this.ServiceName, this.MachineName }), ex);
					}
				}
				finally
				{
					for (i = 0; i < args.Length; i++)
					{
						Marshal.FreeHGlobal(array[i]);
					}
					if (gchandle.IsAllocated)
					{
						gchandle.Free();
					}
				}
			}
			finally
			{
				SafeNativeMethods.CloseServiceHandle(serviceHandle);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004A60 File Offset: 0x00003A60
		public unsafe void Stop()
		{
			if (!this.controlGranted)
			{
				ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, this.machineName, this.ServiceName);
				serviceControllerPermission.Demand();
				this.controlGranted = true;
			}
			IntPtr serviceHandle = this.GetServiceHandle(32);
			try
			{
				for (int i = 0; i < this.DependentServices.Length; i++)
				{
					ServiceController serviceController = this.DependentServices[i];
					serviceController.Refresh();
					if (serviceController.Status != ServiceControllerStatus.Stopped)
					{
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
					}
				}
				NativeMethods.SERVICE_STATUS service_STATUS = default(NativeMethods.SERVICE_STATUS);
				if (!UnsafeNativeMethods.ControlService(serviceHandle, 1, &service_STATUS))
				{
					Exception ex = ServiceController.CreateSafeWin32Exception();
					throw new InvalidOperationException(Res.GetString("StopService", new object[] { this.ServiceName, this.MachineName }), ex);
				}
			}
			finally
			{
				SafeNativeMethods.CloseServiceHandle(serviceHandle);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004B48 File Offset: 0x00003B48
		internal static bool ValidServiceName(string serviceName)
		{
			if (serviceName == null)
			{
				return false;
			}
			if (serviceName.Length > 80 || serviceName.Length == 0)
			{
				return false;
			}
			foreach (char c in serviceName.ToCharArray())
			{
				if (c == '\\' || c == '/')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004B98 File Offset: 0x00003B98
		public void WaitForStatus(ServiceControllerStatus desiredStatus)
		{
			this.WaitForStatus(desiredStatus, TimeSpan.MaxValue);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004BA8 File Offset: 0x00003BA8
		public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
		{
			if (!Enum.IsDefined(typeof(ServiceControllerStatus), desiredStatus))
			{
				throw new InvalidEnumArgumentException("desiredStatus", (int)desiredStatus, typeof(ServiceControllerStatus));
			}
			DateTime utcNow = DateTime.UtcNow;
			this.Refresh();
			while (this.Status != desiredStatus)
			{
				if (DateTime.UtcNow - utcNow > timeout)
				{
					throw new TimeoutException(Res.GetString("Timeout"));
				}
				Thread.Sleep(250);
				this.Refresh();
			}
		}

		// Token: 0x040001EE RID: 494
		private const int DISPLAYNAMEBUFFERSIZE = 256;

		// Token: 0x040001EF RID: 495
		private string machineName = ".";

		// Token: 0x040001F0 RID: 496
		private string name = "";

		// Token: 0x040001F1 RID: 497
		private string displayName = "";

		// Token: 0x040001F2 RID: 498
		private string eitherName = "";

		// Token: 0x040001F3 RID: 499
		private int commandsAccepted;

		// Token: 0x040001F4 RID: 500
		private ServiceControllerStatus status;

		// Token: 0x040001F5 RID: 501
		private IntPtr serviceManagerHandle;

		// Token: 0x040001F6 RID: 502
		private bool statusGenerated;

		// Token: 0x040001F7 RID: 503
		private bool controlGranted;

		// Token: 0x040001F8 RID: 504
		private bool browseGranted;

		// Token: 0x040001F9 RID: 505
		private ServiceController[] dependentServices;

		// Token: 0x040001FA RID: 506
		private ServiceController[] servicesDependedOn;

		// Token: 0x040001FB RID: 507
		private int type;

		// Token: 0x040001FC RID: 508
		private bool disposed;

		// Token: 0x040001FD RID: 509
		private static readonly int UnknownEnvironment = 0;

		// Token: 0x040001FE RID: 510
		private static readonly int NtEnvironment = 1;

		// Token: 0x040001FF RID: 511
		private static readonly int NonNtEnvironment = 2;

		// Token: 0x04000200 RID: 512
		private static int environment = ServiceController.UnknownEnvironment;

		// Token: 0x04000201 RID: 513
		private static object s_InternalSyncObject;
	}
}
