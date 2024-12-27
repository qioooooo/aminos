using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ServiceProcess.Telemetry;
using System.Threading;

namespace System.ServiceProcess
{
	// Token: 0x02000022 RID: 34
	[InstallerType(typeof(ServiceProcessInstaller))]
	public class ServiceBase : Component
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000023EC File Offset: 0x000013EC
		public ServiceBase()
		{
			this.acceptedCommands = 1;
			this.AutoLog = true;
			this.ServiceName = "";
			this.SendServiceStartTelemetry();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002420 File Offset: 0x00001420
		[ComVisible(false)]
		public unsafe void RequestAdditionalTime(int milliseconds)
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				if (this.status.currentState != 5 && this.status.currentState != 2 && this.status.currentState != 3 && this.status.currentState != 6)
				{
					throw new InvalidOperationException(Res.GetString("NotInPendingState"));
				}
				this.status.waitHint = milliseconds;
				this.status.checkPoint = this.status.checkPoint + 1;
				NativeMethods.SetServiceStatus(this.statusHandle, ptr);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000024AC File Offset: 0x000014AC
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000024B4 File Offset: 0x000014B4
		[ServiceProcessDescription("SBAutoLog")]
		[DefaultValue(true)]
		public bool AutoLog
		{
			get
			{
				return this.autoLog;
			}
			set
			{
				this.autoLog = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000024BD File Offset: 0x000014BD
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000024CA File Offset: 0x000014CA
		[ComVisible(false)]
		public int ExitCode
		{
			get
			{
				return this.status.win32ExitCode;
			}
			set
			{
				this.status.win32ExitCode = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000024D8 File Offset: 0x000014D8
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000024E9 File Offset: 0x000014E9
		[DefaultValue(false)]
		public bool CanHandlePowerEvent
		{
			get
			{
				return (this.acceptedCommands & 64) != 0;
			}
			set
			{
				if (this.commandPropsFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeProperties"));
				}
				if (value)
				{
					this.acceptedCommands |= 64;
					return;
				}
				this.acceptedCommands &= -65;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002525 File Offset: 0x00001525
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000253C File Offset: 0x0000153C
		[ComVisible(false)]
		[DefaultValue(false)]
		public bool CanHandleSessionChangeEvent
		{
			get
			{
				return (this.acceptedCommands & 128) != 0;
			}
			set
			{
				if (this.commandPropsFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeProperties"));
				}
				if (value)
				{
					this.acceptedCommands |= 128;
					return;
				}
				this.acceptedCommands &= -129;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002589 File Offset: 0x00001589
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002599 File Offset: 0x00001599
		[DefaultValue(false)]
		public bool CanPauseAndContinue
		{
			get
			{
				return (this.acceptedCommands & 2) != 0;
			}
			set
			{
				if (this.commandPropsFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeProperties"));
				}
				if (value)
				{
					this.acceptedCommands |= 2;
					return;
				}
				this.acceptedCommands &= -3;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000025D4 File Offset: 0x000015D4
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000025E4 File Offset: 0x000015E4
		[DefaultValue(false)]
		public bool CanShutdown
		{
			get
			{
				return (this.acceptedCommands & 4) != 0;
			}
			set
			{
				if (this.commandPropsFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeProperties"));
				}
				if (value)
				{
					this.acceptedCommands |= 4;
					return;
				}
				this.acceptedCommands &= -5;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000261F File Offset: 0x0000161F
		// (set) Token: 0x0600004E RID: 78 RVA: 0x0000262F File Offset: 0x0000162F
		[DefaultValue(true)]
		public bool CanStop
		{
			get
			{
				return (this.acceptedCommands & 1) != 0;
			}
			set
			{
				if (this.commandPropsFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeProperties"));
				}
				if (value)
				{
					this.acceptedCommands |= 1;
					return;
				}
				this.acceptedCommands &= -2;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000266A File Offset: 0x0000166A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual EventLog EventLog
		{
			get
			{
				if (this.eventLog == null)
				{
					this.eventLog = new EventLog();
					this.eventLog.Source = this.ServiceName;
					this.eventLog.Log = "Application";
				}
				return this.eventLog;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000026A6 File Offset: 0x000016A6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected IntPtr ServiceHandle
		{
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return this.statusHandle;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000026B9 File Offset: 0x000016B9
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000026C4 File Offset: 0x000016C4
		[ServiceProcessDescription("SBServiceName")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string ServiceName
		{
			get
			{
				return this.serviceName;
			}
			set
			{
				if (this.nameFrozen)
				{
					throw new InvalidOperationException(Res.GetString("CannotChangeName"));
				}
				if (value != "" && !ServiceController.ValidServiceName(value))
				{
					throw new ArgumentException(Res.GetString("ServiceName", new object[]
					{
						value,
						80.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.serviceName = value;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002734 File Offset: 0x00001734
		protected override void Dispose(bool disposing)
		{
			if (this.handleName != (IntPtr)0)
			{
				Marshal.FreeHGlobal(this.handleName);
				this.handleName = (IntPtr)0;
			}
			this.nameFrozen = false;
			this.commandPropsFrozen = false;
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002787 File Offset: 0x00001787
		protected virtual void OnContinue()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002789 File Offset: 0x00001789
		protected virtual void OnPause()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000278B File Offset: 0x0000178B
		protected virtual bool OnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			return true;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000278E File Offset: 0x0000178E
		protected virtual void OnSessionChange(SessionChangeDescription changeDescription)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002790 File Offset: 0x00001790
		protected virtual void OnShutdown()
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002792 File Offset: 0x00001792
		protected virtual void OnStart(string[] args)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002794 File Offset: 0x00001794
		protected virtual void OnStop()
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002798 File Offset: 0x00001798
		private unsafe void DeferredContinue()
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				try
				{
					this.OnContinue();
					this.WriteEventLogEntry(Res.GetString("ContinueSuccessful"));
					this.status.currentState = 4;
				}
				catch (Exception ex)
				{
					this.status.currentState = 7;
					this.WriteEventLogEntry(Res.GetString("ContinueFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
					throw;
				}
				catch
				{
					this.status.currentState = 7;
					this.WriteEventLogEntry(Res.GetString("ContinueFailed", new object[] { string.Empty }), EventLogEntryType.Error);
					throw;
				}
				finally
				{
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002870 File Offset: 0x00001870
		private void DeferredCustomCommand(int command)
		{
			try
			{
				this.OnCustomCommand(command);
				this.WriteEventLogEntry(Res.GetString("CommandSuccessful"));
			}
			catch (Exception ex)
			{
				this.WriteEventLogEntry(Res.GetString("CommandFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
				throw;
			}
			catch
			{
				this.WriteEventLogEntry(Res.GetString("CommandFailed", new object[] { string.Empty }), EventLogEntryType.Error);
				throw;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000028FC File Offset: 0x000018FC
		private unsafe void DeferredPause()
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				try
				{
					this.OnPause();
					this.WriteEventLogEntry(Res.GetString("PauseSuccessful"));
					this.status.currentState = 7;
				}
				catch (Exception ex)
				{
					this.status.currentState = 4;
					this.WriteEventLogEntry(Res.GetString("PauseFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
					throw;
				}
				catch
				{
					this.status.currentState = 4;
					this.WriteEventLogEntry(Res.GetString("PauseFailed", new object[] { string.Empty }), EventLogEntryType.Error);
					throw;
				}
				finally
				{
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000029D4 File Offset: 0x000019D4
		private void DeferredPowerEvent(int eventType, IntPtr eventData)
		{
			try
			{
				this.OnPowerEvent((PowerBroadcastStatus)eventType);
				this.WriteEventLogEntry(Res.GetString("PowerEventOK"));
			}
			catch (Exception ex)
			{
				this.WriteEventLogEntry(Res.GetString("PowerEventFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
				throw;
			}
			catch
			{
				this.WriteEventLogEntry(Res.GetString("PowerEventFailed", new object[] { string.Empty }), EventLogEntryType.Error);
				throw;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002A64 File Offset: 0x00001A64
		private void DeferredSessionChange(int eventType, int sessionId)
		{
			try
			{
				this.OnSessionChange(new SessionChangeDescription((SessionChangeReason)eventType, sessionId));
			}
			catch (Exception ex)
			{
				this.WriteEventLogEntry(Res.GetString("SessionChangeFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
				throw;
			}
			catch
			{
				this.WriteEventLogEntry(Res.GetString("SessionChangeFailed", new object[] { string.Empty }), EventLogEntryType.Error);
				throw;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002AE8 File Offset: 0x00001AE8
		private unsafe void DeferredStop()
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				int currentState = this.status.currentState;
				this.status.checkPoint = 0;
				this.status.waitHint = 0;
				this.status.currentState = 3;
				NativeMethods.SetServiceStatus(this.statusHandle, ptr);
				try
				{
					this.OnStop();
					this.WriteEventLogEntry(Res.GetString("StopSuccessful"));
					this.status.currentState = 1;
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
					if (this.isServiceHosted)
					{
						try
						{
							AppDomain.Unload(AppDomain.CurrentDomain);
						}
						catch (CannotUnloadAppDomainException ex)
						{
							this.WriteEventLogEntry(Res.GetString("FailedToUnloadAppDomain", new object[]
							{
								AppDomain.CurrentDomain.FriendlyName,
								ex.Message
							}), EventLogEntryType.Error);
						}
					}
				}
				catch (Exception ex2)
				{
					this.status.currentState = currentState;
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
					this.WriteEventLogEntry(Res.GetString("StopFailed", new object[] { ex2.ToString() }), EventLogEntryType.Error);
					throw;
				}
				catch
				{
					this.status.currentState = currentState;
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
					this.WriteEventLogEntry(Res.GetString("StopFailed", new object[] { string.Empty }), EventLogEntryType.Error);
					throw;
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002C68 File Offset: 0x00001C68
		private unsafe void DeferredShutdown()
		{
			try
			{
				this.OnShutdown();
				this.WriteEventLogEntry(Res.GetString("ShutdownOK"));
				if (this.status.currentState != 7)
				{
					if (this.status.currentState != 4)
					{
						goto IL_00BA;
					}
				}
				try
				{
					fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
					{
						this.status.checkPoint = 0;
						this.status.waitHint = 0;
						this.status.currentState = 1;
						NativeMethods.SetServiceStatus(this.statusHandle, ptr);
						if (this.isServiceHosted)
						{
							try
							{
								AppDomain.Unload(AppDomain.CurrentDomain);
							}
							catch (CannotUnloadAppDomainException ex)
							{
								this.WriteEventLogEntry(Res.GetString("FailedToUnloadAppDomain", new object[]
								{
									AppDomain.CurrentDomain.FriendlyName,
									ex.Message
								}), EventLogEntryType.Error);
							}
						}
					}
				}
				finally
				{
					NativeMethods.SERVICE_STATUS* ptr = null;
				}
				IL_00BA:;
			}
			catch (Exception ex2)
			{
				this.WriteEventLogEntry(Res.GetString("ShutdownFailed", new object[] { ex2.ToString() }), EventLogEntryType.Error);
				throw;
			}
			catch
			{
				this.WriteEventLogEntry(Res.GetString("ShutdownFailed", new object[] { string.Empty }), EventLogEntryType.Error);
				throw;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002DB4 File Offset: 0x00001DB4
		protected virtual void OnCustomCommand(int command)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002DB8 File Offset: 0x00001DB8
		public static void Run(ServiceBase[] services)
		{
			if (services == null || services.Length == 0)
			{
				throw new ArgumentException(Res.GetString("NoServices"));
			}
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				string @string = Res.GetString("CantRunOnWin9x");
				string string2 = Res.GetString("CantRunOnWin9xTitle");
				ServiceBase.LateBoundMessageBoxShow(@string, string2);
				return;
			}
			IntPtr intPtr = Marshal.AllocHGlobal((IntPtr)((services.Length + 1) * Marshal.SizeOf(typeof(NativeMethods.SERVICE_TABLE_ENTRY))));
			NativeMethods.SERVICE_TABLE_ENTRY[] array = new NativeMethods.SERVICE_TABLE_ENTRY[services.Length];
			bool flag = services.Length > 1;
			IntPtr intPtr2 = (IntPtr)0;
			for (int i = 0; i < services.Length; i++)
			{
				services[i].Initialize(flag);
				array[i] = services[i].GetEntry();
				intPtr2 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(NativeMethods.SERVICE_TABLE_ENTRY)) * i));
				Marshal.StructureToPtr(array[i], intPtr2, true);
			}
			NativeMethods.SERVICE_TABLE_ENTRY service_TABLE_ENTRY = new NativeMethods.SERVICE_TABLE_ENTRY();
			service_TABLE_ENTRY.callback = null;
			service_TABLE_ENTRY.name = (IntPtr)0;
			intPtr2 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(NativeMethods.SERVICE_TABLE_ENTRY)) * services.Length));
			Marshal.StructureToPtr(service_TABLE_ENTRY, intPtr2, true);
			bool flag2 = NativeMethods.StartServiceCtrlDispatcher(intPtr);
			string text = "";
			if (!flag2)
			{
				text = new Win32Exception().Message;
				string string3 = Res.GetString("CantStartFromCommandLine");
				if (Environment.UserInteractive)
				{
					string string4 = Res.GetString("CantStartFromCommandLineTitle");
					ServiceBase.LateBoundMessageBoxShow(string3, string4);
				}
				else
				{
					Console.WriteLine(string3);
				}
			}
			foreach (ServiceBase serviceBase in services)
			{
				serviceBase.Dispose();
				if (!flag2 && serviceBase.EventLog.Source.Length != 0)
				{
					serviceBase.WriteEventLogEntry(Res.GetString("StartFailed", new object[] { text }), EventLogEntryType.Error);
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F90 File Offset: 0x00001F90
		public static void Run(ServiceBase service)
		{
			if (service == null)
			{
				throw new ArgumentException(Res.GetString("NoServices"));
			}
			ServiceBase.Run(new ServiceBase[] { service });
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002FC1 File Offset: 0x00001FC1
		public void Stop()
		{
			this.DeferredStop();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002FCC File Offset: 0x00001FCC
		private void Initialize(bool multipleServices)
		{
			if (!this.initialized)
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (!multipleServices)
				{
					this.status.serviceType = 16;
				}
				else
				{
					this.status.serviceType = 32;
				}
				this.status.currentState = 2;
				this.status.controlsAccepted = 0;
				this.status.win32ExitCode = 0;
				this.status.serviceSpecificExitCode = 0;
				this.status.checkPoint = 0;
				this.status.waitHint = 0;
				this.mainCallback = new NativeMethods.ServiceMainCallback(this.ServiceMainCallback);
				this.commandCallback = new NativeMethods.ServiceControlCallback(this.ServiceCommandCallback);
				this.commandCallbackEx = new NativeMethods.ServiceControlCallbackEx(this.ServiceCommandCallbackEx);
				this.handleName = Marshal.StringToHGlobalUni(this.ServiceName);
				this.initialized = true;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000030B4 File Offset: 0x000020B4
		private NativeMethods.SERVICE_TABLE_ENTRY GetEntry()
		{
			NativeMethods.SERVICE_TABLE_ENTRY service_TABLE_ENTRY = new NativeMethods.SERVICE_TABLE_ENTRY();
			this.nameFrozen = true;
			service_TABLE_ENTRY.callback = this.mainCallback;
			service_TABLE_ENTRY.name = this.handleName;
			return service_TABLE_ENTRY;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000030E8 File Offset: 0x000020E8
		private static void LateBoundMessageBoxShow(string message, string title)
		{
			int num = 0;
			if (ServiceBase.IsRTLResources)
			{
				num |= 1572864;
			}
			Type type = Type.GetType("System.Windows.Forms.MessageBox, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type2 = Type.GetType("System.Windows.Forms.MessageBoxButtons, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type3 = Type.GetType("System.Windows.Forms.MessageBoxIcon, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type4 = Type.GetType("System.Windows.Forms.MessageBoxDefaultButton, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type5 = Type.GetType("System.Windows.Forms.MessageBoxOptions, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			type.InvokeMember("Show", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[]
			{
				message,
				title,
				Enum.ToObject(type2, 0),
				Enum.ToObject(type3, 0),
				Enum.ToObject(type4, 0),
				Enum.ToObject(type5, num)
			}, CultureInfo.InvariantCulture);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003199 File Offset: 0x00002199
		private static bool IsRTLResources
		{
			get
			{
				return Res.GetString("RTL") != "RTL_False";
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000031B0 File Offset: 0x000021B0
		private int ServiceCommandCallbackEx(int command, int eventType, IntPtr eventData, IntPtr eventContext)
		{
			int num = 0;
			switch (command)
			{
			case 13:
			{
				ServiceBase.DeferredHandlerDelegateAdvanced deferredHandlerDelegateAdvanced = new ServiceBase.DeferredHandlerDelegateAdvanced(this.DeferredPowerEvent);
				deferredHandlerDelegateAdvanced.BeginInvoke(eventType, eventData, null, null);
				break;
			}
			case 14:
			{
				ServiceBase.DeferredHandlerDelegateAdvancedSession deferredHandlerDelegateAdvancedSession = new ServiceBase.DeferredHandlerDelegateAdvancedSession(this.DeferredSessionChange);
				NativeMethods.WTSSESSION_NOTIFICATION wtssession_NOTIFICATION = new NativeMethods.WTSSESSION_NOTIFICATION();
				Marshal.PtrToStructure(eventData, wtssession_NOTIFICATION);
				deferredHandlerDelegateAdvancedSession.BeginInvoke(eventType, wtssession_NOTIFICATION.sessionId, null, null);
				break;
			}
			default:
				this.ServiceCommandCallback(command);
				break;
			}
			return num;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003224 File Offset: 0x00002224
		private unsafe void ServiceCommandCallback(int command)
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				if (command == 4)
				{
					NativeMethods.SetServiceStatus(this.statusHandle, ptr);
				}
				else if (this.status.currentState != 5 && this.status.currentState != 2 && this.status.currentState != 3 && this.status.currentState != 6)
				{
					switch (command)
					{
					case 1:
					{
						int currentState = this.status.currentState;
						if (this.status.currentState == 7 || this.status.currentState == 4)
						{
							this.status.currentState = 3;
							NativeMethods.SetServiceStatus(this.statusHandle, ptr);
							this.status.currentState = currentState;
							ServiceBase.DeferredHandlerDelegate deferredHandlerDelegate = new ServiceBase.DeferredHandlerDelegate(this.DeferredStop);
							deferredHandlerDelegate.BeginInvoke(null, null);
							goto IL_01AE;
						}
						goto IL_01AE;
					}
					case 2:
						if (this.status.currentState == 4)
						{
							this.status.currentState = 6;
							NativeMethods.SetServiceStatus(this.statusHandle, ptr);
							ServiceBase.DeferredHandlerDelegate deferredHandlerDelegate2 = new ServiceBase.DeferredHandlerDelegate(this.DeferredPause);
							deferredHandlerDelegate2.BeginInvoke(null, null);
							goto IL_01AE;
						}
						goto IL_01AE;
					case 3:
						if (this.status.currentState == 7)
						{
							this.status.currentState = 5;
							NativeMethods.SetServiceStatus(this.statusHandle, ptr);
							ServiceBase.DeferredHandlerDelegate deferredHandlerDelegate3 = new ServiceBase.DeferredHandlerDelegate(this.DeferredContinue);
							deferredHandlerDelegate3.BeginInvoke(null, null);
							goto IL_01AE;
						}
						goto IL_01AE;
					case 5:
					{
						ServiceBase.DeferredHandlerDelegate deferredHandlerDelegate4 = new ServiceBase.DeferredHandlerDelegate(this.DeferredShutdown);
						deferredHandlerDelegate4.BeginInvoke(null, null);
						goto IL_01AE;
					}
					}
					ServiceBase.DeferredHandlerDelegateCommand deferredHandlerDelegateCommand = new ServiceBase.DeferredHandlerDelegateCommand(this.DeferredCustomCommand);
					deferredHandlerDelegateCommand.BeginInvoke(command, null, null);
				}
				IL_01AE:;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000033E4 File Offset: 0x000023E4
		private void ServiceQueuedMainCallback(object state)
		{
			string[] array = (string[])state;
			try
			{
				this.OnStart(array);
				this.WriteEventLogEntry(Res.GetString("StartSuccessful"));
				this.status.checkPoint = 0;
				this.status.waitHint = 0;
				this.status.currentState = 4;
			}
			catch (Exception ex)
			{
				this.WriteEventLogEntry(Res.GetString("StartFailed", new object[] { ex.ToString() }), EventLogEntryType.Error);
				this.status.currentState = 1;
			}
			catch
			{
				this.WriteEventLogEntry(Res.GetString("StartFailed", new object[] { string.Empty }), EventLogEntryType.Error);
				this.status.currentState = 1;
			}
			this.startCompletedSignal.Set();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000034C0 File Offset: 0x000024C0
		[ComVisible(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public unsafe void ServiceMainCallback(int argCount, IntPtr argPointer)
		{
			fixed (NativeMethods.SERVICE_STATUS* ptr = &this.status)
			{
				string[] array = null;
				if (argCount > 0)
				{
					char** ptr2 = (char**)argPointer.ToPointer();
					array = new string[argCount - 1];
					for (int i = 0; i < array.Length; i++)
					{
						ptr2 += sizeof(char*) / sizeof(char*);
						array[i] = Marshal.PtrToStringUni((IntPtr)(*(IntPtr*)ptr2));
					}
				}
				if (!this.initialized)
				{
					this.isServiceHosted = true;
					this.Initialize(true);
				}
				if (Environment.OSVersion.Version.Major >= 5)
				{
					this.statusHandle = NativeMethods.RegisterServiceCtrlHandlerEx(this.ServiceName, this.commandCallbackEx, (IntPtr)0);
				}
				else
				{
					this.statusHandle = NativeMethods.RegisterServiceCtrlHandler(this.ServiceName, this.commandCallback);
				}
				this.nameFrozen = true;
				if (this.statusHandle == (IntPtr)0)
				{
					string message = new Win32Exception().Message;
					this.WriteEventLogEntry(Res.GetString("StartFailed", new object[] { message }), EventLogEntryType.Error);
				}
				this.status.controlsAccepted = this.acceptedCommands;
				this.commandPropsFrozen = true;
				if ((this.status.controlsAccepted & 1) != 0)
				{
					this.status.controlsAccepted = this.status.controlsAccepted | 4;
				}
				if (Environment.OSVersion.Version.Major < 5)
				{
					this.status.controlsAccepted = this.status.controlsAccepted & -65;
				}
				this.status.currentState = 2;
				if (NativeMethods.SetServiceStatus(this.statusHandle, ptr))
				{
					this.startCompletedSignal = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.ServiceQueuedMainCallback), array);
					this.startCompletedSignal.WaitOne();
					if (!NativeMethods.SetServiceStatus(this.statusHandle, ptr))
					{
						this.WriteEventLogEntry(Res.GetString("StartFailed", new object[] { new Win32Exception().Message }), EventLogEntryType.Error);
						this.status.currentState = 1;
						NativeMethods.SetServiceStatus(this.statusHandle, ptr);
					}
					ptr = null;
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000036C4 File Offset: 0x000026C4
		private void WriteEventLogEntry(string message)
		{
			try
			{
				if (this.AutoLog)
				{
					this.EventLog.WriteEntry(message);
				}
			}
			catch (StackOverflowException)
			{
				throw;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003728 File Offset: 0x00002728
		private void WriteEventLogEntry(string message, EventLogEntryType errorType)
		{
			try
			{
				if (this.AutoLog)
				{
					this.EventLog.WriteEntry(message, errorType);
				}
			}
			catch (StackOverflowException)
			{
				throw;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000378C File Offset: 0x0000278C
		private void SendServiceStartTelemetry()
		{
			ServiceProcessTraceLogger.TraceServiceProcessStart();
		}

		// Token: 0x040001DD RID: 477
		public const int MaxNameLength = 80;

		// Token: 0x040001DE RID: 478
		private NativeMethods.SERVICE_STATUS status = default(NativeMethods.SERVICE_STATUS);

		// Token: 0x040001DF RID: 479
		private IntPtr statusHandle;

		// Token: 0x040001E0 RID: 480
		private NativeMethods.ServiceControlCallback commandCallback;

		// Token: 0x040001E1 RID: 481
		private NativeMethods.ServiceControlCallbackEx commandCallbackEx;

		// Token: 0x040001E2 RID: 482
		private NativeMethods.ServiceMainCallback mainCallback;

		// Token: 0x040001E3 RID: 483
		private IntPtr handleName;

		// Token: 0x040001E4 RID: 484
		private ManualResetEvent startCompletedSignal;

		// Token: 0x040001E5 RID: 485
		private int acceptedCommands;

		// Token: 0x040001E6 RID: 486
		private bool autoLog;

		// Token: 0x040001E7 RID: 487
		private string serviceName;

		// Token: 0x040001E8 RID: 488
		private EventLog eventLog;

		// Token: 0x040001E9 RID: 489
		private bool nameFrozen;

		// Token: 0x040001EA RID: 490
		private bool commandPropsFrozen;

		// Token: 0x040001EB RID: 491
		private bool disposed;

		// Token: 0x040001EC RID: 492
		private bool initialized;

		// Token: 0x040001ED RID: 493
		private bool isServiceHosted;

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x06000072 RID: 114
		private delegate void DeferredHandlerDelegate();

		// Token: 0x02000024 RID: 36
		// (Invoke) Token: 0x06000076 RID: 118
		private delegate void DeferredHandlerDelegateCommand(int command);

		// Token: 0x02000025 RID: 37
		// (Invoke) Token: 0x0600007A RID: 122
		private delegate void DeferredHandlerDelegateAdvanced(int eventType, IntPtr eventData);

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x0600007E RID: 126
		private delegate void DeferredHandlerDelegateAdvancedSession(int eventType, int sessionId);
	}
}
