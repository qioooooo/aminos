using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000774 RID: 1908
	[MonitoringDescription("ProcessDesc")]
	[DefaultProperty("StartInfo")]
	[DefaultEvent("Exited")]
	[Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true, Synchronization = true, ExternalProcessMgmt = true, SelfAffectingProcessMgmt = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class Process : Component
	{
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06003AAB RID: 15019 RVA: 0x000F96AF File Offset: 0x000F86AF
		// (remove) Token: 0x06003AAC RID: 15020 RVA: 0x000F96C8 File Offset: 0x000F86C8
		[MonitoringDescription("ProcessAssociated")]
		[Browsable(true)]
		public event DataReceivedEventHandler OutputDataReceived;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06003AAD RID: 15021 RVA: 0x000F96E1 File Offset: 0x000F86E1
		// (remove) Token: 0x06003AAE RID: 15022 RVA: 0x000F96FA File Offset: 0x000F86FA
		[MonitoringDescription("ProcessAssociated")]
		[Browsable(true)]
		public event DataReceivedEventHandler ErrorDataReceived;

		// Token: 0x06003AAF RID: 15023 RVA: 0x000F9713 File Offset: 0x000F8713
		public Process()
		{
			this.machineName = ".";
			this.outputStreamReadMode = Process.StreamReadMode.undefined;
			this.errorStreamReadMode = Process.StreamReadMode.undefined;
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x000F9734 File Offset: 0x000F8734
		private Process(string machineName, bool isRemoteMachine, int processId, ProcessInfo processInfo)
		{
			this.processInfo = processInfo;
			this.machineName = machineName;
			this.isRemoteMachine = isRemoteMachine;
			this.processId = processId;
			this.haveProcessId = true;
			this.outputStreamReadMode = Process.StreamReadMode.undefined;
			this.errorStreamReadMode = Process.StreamReadMode.undefined;
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x000F976E File Offset: 0x000F876E
		[MonitoringDescription("ProcessAssociated")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private bool Associated
		{
			get
			{
				return this.haveProcessId || this.haveProcessHandle;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x000F9780 File Offset: 0x000F8780
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessBasePriority")]
		public int BasePriority
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				return this.processInfo.basePriority;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x000F9794 File Offset: 0x000F8794
		[MonitoringDescription("ProcessExitCode")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ExitCode
		{
			get
			{
				this.EnsureState(Process.State.Exited);
				return this.exitCode;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003AB4 RID: 15028 RVA: 0x000F97A4 File Offset: 0x000F87A4
		[Browsable(false)]
		[MonitoringDescription("ProcessTerminated")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HasExited
		{
			get
			{
				if (!this.exited)
				{
					this.EnsureState(Process.State.Associated);
					SafeProcessHandle safeProcessHandle = null;
					try
					{
						safeProcessHandle = this.GetProcessHandle(1049600, false);
						int num;
						if (safeProcessHandle.IsInvalid)
						{
							this.exited = true;
						}
						else if (NativeMethods.GetExitCodeProcess(safeProcessHandle, out num) && num != 259)
						{
							this.exited = true;
							this.exitCode = num;
						}
						else
						{
							if (!this.signaled)
							{
								ProcessWaitHandle processWaitHandle = null;
								try
								{
									processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
									this.signaled = processWaitHandle.WaitOne(0, false);
								}
								finally
								{
									if (processWaitHandle != null)
									{
										processWaitHandle.Close();
									}
								}
							}
							if (this.signaled)
							{
								if (!NativeMethods.GetExitCodeProcess(safeProcessHandle, out num))
								{
									throw new Win32Exception();
								}
								this.exited = true;
								this.exitCode = num;
							}
						}
					}
					finally
					{
						this.ReleaseProcessHandle(safeProcessHandle);
					}
					if (this.exited)
					{
						this.RaiseOnExited();
					}
				}
				return this.exited;
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x000F9894 File Offset: 0x000F8894
		private ProcessThreadTimes GetProcessTimes()
		{
			ProcessThreadTimes processThreadTimes = new ProcessThreadTimes();
			SafeProcessHandle safeProcessHandle = null;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1024, false);
				if (safeProcessHandle.IsInvalid)
				{
					throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[] { this.processId.ToString(CultureInfo.CurrentCulture) }));
				}
				if (!NativeMethods.GetProcessTimes(safeProcessHandle, out processThreadTimes.create, out processThreadTimes.exit, out processThreadTimes.kernel, out processThreadTimes.user))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
			return processThreadTimes;
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x000F992C File Offset: 0x000F892C
		[MonitoringDescription("ProcessExitTime")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime ExitTime
		{
			get
			{
				if (!this.haveExitTime)
				{
					this.EnsureState((Process.State)20);
					this.exitTime = this.GetProcessTimes().ExitTime;
					this.haveExitTime = true;
				}
				return this.exitTime;
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000F995C File Offset: 0x000F895C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessHandle")]
		public IntPtr Handle
		{
			get
			{
				this.EnsureState(Process.State.Associated);
				return this.OpenProcessHandle().DangerousGetHandle();
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x000F9971 File Offset: 0x000F8971
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessHandleCount")]
		public int HandleCount
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				return this.processInfo.handleCount;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x000F9985 File Offset: 0x000F8985
		[MonitoringDescription("ProcessId")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Id
		{
			get
			{
				this.EnsureState(Process.State.HaveId);
				return this.processId;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x000F9994 File Offset: 0x000F8994
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("ProcessMachineName")]
		public string MachineName
		{
			get
			{
				this.EnsureState(Process.State.Associated);
				return this.machineName;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000F99A4 File Offset: 0x000F89A4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessMainWindowHandle")]
		public IntPtr MainWindowHandle
		{
			get
			{
				if (!this.haveMainWindow)
				{
					this.EnsureState((Process.State)10);
					this.mainWindowHandle = ProcessManager.GetMainWindowHandle(this.processInfo);
					this.haveMainWindow = true;
				}
				return this.mainWindowHandle;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x000F99D4 File Offset: 0x000F89D4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessMainWindowTitle")]
		public string MainWindowTitle
		{
			get
			{
				if (this.mainWindowTitle == null)
				{
					IntPtr intPtr = this.MainWindowHandle;
					if (intPtr == (IntPtr)0)
					{
						this.mainWindowTitle = string.Empty;
					}
					else
					{
						int num = NativeMethods.GetWindowTextLength(new HandleRef(this, intPtr)) * 2;
						StringBuilder stringBuilder = new StringBuilder(num);
						NativeMethods.GetWindowText(new HandleRef(this, intPtr), stringBuilder, stringBuilder.Capacity);
						this.mainWindowTitle = stringBuilder.ToString();
					}
				}
				return this.mainWindowTitle;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003ABD RID: 15037 RVA: 0x000F9A48 File Offset: 0x000F8A48
		[Browsable(false)]
		[MonitoringDescription("ProcessMainModule")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProcessModule MainModule
		{
			get
			{
				if (this.OperatingSystem.Platform == PlatformID.Win32NT)
				{
					this.EnsureState((Process.State)3);
					ModuleInfo firstModuleInfo = NtProcessManager.GetFirstModuleInfo(this.processId);
					return new ProcessModule(firstModuleInfo);
				}
				ProcessModuleCollection processModuleCollection = this.Modules;
				this.EnsureState(Process.State.HaveProcessInfo);
				foreach (object obj in processModuleCollection)
				{
					ProcessModule processModule = (ProcessModule)obj;
					if (processModule.moduleInfo.Id == this.processInfo.mainModuleId)
					{
						return processModule;
					}
				}
				return null;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x000F9AF4 File Offset: 0x000F8AF4
		// (set) Token: 0x06003ABF RID: 15039 RVA: 0x000F9B02 File Offset: 0x000F8B02
		[MonitoringDescription("ProcessMaxWorkingSet")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr MaxWorkingSet
		{
			get
			{
				this.EnsureWorkingSetLimits();
				return this.maxWorkingSet;
			}
			set
			{
				this.SetWorkingSetLimits(null, value);
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x000F9B11 File Offset: 0x000F8B11
		// (set) Token: 0x06003AC1 RID: 15041 RVA: 0x000F9B1F File Offset: 0x000F8B1F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessMinWorkingSet")]
		public IntPtr MinWorkingSet
		{
			get
			{
				this.EnsureWorkingSetLimits();
				return this.minWorkingSet;
			}
			set
			{
				this.SetWorkingSetLimits(value, null);
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x000F9B30 File Offset: 0x000F8B30
		[MonitoringDescription("ProcessModules")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProcessModuleCollection Modules
		{
			get
			{
				if (this.modules == null)
				{
					this.EnsureState((Process.State)3);
					ModuleInfo[] moduleInfos = ProcessManager.GetModuleInfos(this.processId);
					ProcessModule[] array = new ProcessModule[moduleInfos.Length];
					for (int i = 0; i < moduleInfos.Length; i++)
					{
						array[i] = new ProcessModule(moduleInfos[i]);
					}
					ProcessModuleCollection processModuleCollection = new ProcessModuleCollection(array);
					this.modules = processModuleCollection;
				}
				return this.modules;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x000F9B8E File Offset: 0x000F8B8E
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.NonpagedSystemMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessNonpagedSystemMemorySize")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int NonpagedSystemMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.poolNonpagedBytes;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003AC4 RID: 15044 RVA: 0x000F9BA4 File Offset: 0x000F8BA4
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessNonpagedSystemMemorySize")]
		public long NonpagedSystemMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.poolNonpagedBytes;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x000F9BB9 File Offset: 0x000F8BB9
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PagedMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("ProcessPagedMemorySize")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagedMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.pageFileBytes;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x000F9BCF File Offset: 0x000F8BCF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPagedMemorySize")]
		public long PagedMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.pageFileBytes;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x000F9BE4 File Offset: 0x000F8BE4
		[MonitoringDescription("ProcessPagedSystemMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PagedSystemMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagedSystemMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.poolPagedBytes;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000F9BFA File Offset: 0x000F8BFA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPagedSystemMemorySize")]
		public long PagedSystemMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.poolPagedBytes;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x000F9C0F File Offset: 0x000F8C0F
		[MonitoringDescription("ProcessPeakPagedMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakPagedMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakPagedMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.pageFileBytesPeak;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000F9C25 File Offset: 0x000F8C25
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPeakPagedMemorySize")]
		public long PeakPagedMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.pageFileBytesPeak;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000F9C3A File Offset: 0x000F8C3A
		[MonitoringDescription("ProcessPeakWorkingSet")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakWorkingSet64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakWorkingSet
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.workingSetPeak;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x000F9C50 File Offset: 0x000F8C50
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPeakWorkingSet")]
		public long PeakWorkingSet64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.workingSetPeak;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x000F9C65 File Offset: 0x000F8C65
		[MonitoringDescription("ProcessPeakVirtualMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PeakVirtualMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakVirtualMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.virtualBytesPeak;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x000F9C7B File Offset: 0x000F8C7B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessPeakVirtualMemorySize")]
		public long PeakVirtualMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.virtualBytesPeak;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x000F9C90 File Offset: 0x000F8C90
		private OperatingSystem OperatingSystem
		{
			get
			{
				if (this.operatingSystem == null)
				{
					this.operatingSystem = Environment.OSVersion;
				}
				return this.operatingSystem;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x000F9CAC File Offset: 0x000F8CAC
		// (set) Token: 0x06003AD1 RID: 15057 RVA: 0x000F9D1C File Offset: 0x000F8D1C
		[MonitoringDescription("ProcessPriorityBoostEnabled")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool PriorityBoostEnabled
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				if (!this.havePriorityBoostEnabled)
				{
					SafeProcessHandle safeProcessHandle = null;
					try
					{
						safeProcessHandle = this.GetProcessHandle(1024);
						bool flag = false;
						if (!NativeMethods.GetProcessPriorityBoost(safeProcessHandle, out flag))
						{
							throw new Win32Exception();
						}
						this.priorityBoostEnabled = !flag;
						this.havePriorityBoostEnabled = true;
					}
					finally
					{
						this.ReleaseProcessHandle(safeProcessHandle);
					}
				}
				return this.priorityBoostEnabled;
			}
			set
			{
				this.EnsureState(Process.State.IsNt);
				SafeProcessHandle safeProcessHandle = null;
				try
				{
					safeProcessHandle = this.GetProcessHandle(512);
					if (!NativeMethods.SetProcessPriorityBoost(safeProcessHandle, !value))
					{
						throw new Win32Exception();
					}
					this.priorityBoostEnabled = value;
					this.havePriorityBoostEnabled = true;
				}
				finally
				{
					this.ReleaseProcessHandle(safeProcessHandle);
				}
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x000F9D78 File Offset: 0x000F8D78
		// (set) Token: 0x06003AD3 RID: 15059 RVA: 0x000F9DDC File Offset: 0x000F8DDC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPriorityClass")]
		public ProcessPriorityClass PriorityClass
		{
			get
			{
				if (!this.havePriorityClass)
				{
					SafeProcessHandle safeProcessHandle = null;
					try
					{
						safeProcessHandle = this.GetProcessHandle(1024);
						int num = NativeMethods.GetPriorityClass(safeProcessHandle);
						if (num == 0)
						{
							throw new Win32Exception();
						}
						this.priorityClass = (ProcessPriorityClass)num;
						this.havePriorityClass = true;
					}
					finally
					{
						this.ReleaseProcessHandle(safeProcessHandle);
					}
				}
				return this.priorityClass;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessPriorityClass), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessPriorityClass));
				}
				if ((value & (ProcessPriorityClass)49152) != (ProcessPriorityClass)0 && (this.OperatingSystem.Platform != PlatformID.Win32NT || this.OperatingSystem.Version.Major < 5))
				{
					throw new PlatformNotSupportedException(SR.GetString("PriorityClassNotSupported"), null);
				}
				SafeProcessHandle safeProcessHandle = null;
				try
				{
					safeProcessHandle = this.GetProcessHandle(512);
					if (!NativeMethods.SetPriorityClass(safeProcessHandle, (int)value))
					{
						throw new Win32Exception();
					}
					this.priorityClass = value;
					this.havePriorityClass = true;
				}
				finally
				{
					this.ReleaseProcessHandle(safeProcessHandle);
				}
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x000F9E98 File Offset: 0x000F8E98
		[MonitoringDescription("ProcessPrivateMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.PrivateMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PrivateMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.privateBytes;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000F9EAE File Offset: 0x000F8EAE
		[ComVisible(false)]
		[MonitoringDescription("ProcessPrivateMemorySize")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long PrivateMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.privateBytes;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x000F9EC3 File Offset: 0x000F8EC3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessPrivilegedProcessorTime")]
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().PrivilegedProcessorTime;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x000F9ED8 File Offset: 0x000F8ED8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessProcessName")]
		public string ProcessName
		{
			get
			{
				this.EnsureState(Process.State.HaveProcessInfo);
				string processName = this.processInfo.processName;
				if (processName.Length == 15 && ProcessManager.IsNt && ProcessManager.IsOSOlderThanXP && !this.isRemoteMachine)
				{
					try
					{
						string moduleName = this.MainModule.ModuleName;
						if (moduleName != null)
						{
							this.processInfo.processName = Path.ChangeExtension(Path.GetFileName(moduleName), null);
						}
					}
					catch (Exception)
					{
					}
				}
				return this.processInfo.processName;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x000F9F60 File Offset: 0x000F8F60
		// (set) Token: 0x06003AD9 RID: 15065 RVA: 0x000F9FC4 File Offset: 0x000F8FC4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessProcessorAffinity")]
		public IntPtr ProcessorAffinity
		{
			get
			{
				if (!this.haveProcessorAffinity)
				{
					SafeProcessHandle safeProcessHandle = null;
					try
					{
						safeProcessHandle = this.GetProcessHandle(1024);
						IntPtr intPtr;
						IntPtr intPtr2;
						if (!NativeMethods.GetProcessAffinityMask(safeProcessHandle, out intPtr, out intPtr2))
						{
							throw new Win32Exception();
						}
						this.processorAffinity = intPtr;
					}
					finally
					{
						this.ReleaseProcessHandle(safeProcessHandle);
					}
					this.haveProcessorAffinity = true;
				}
				return this.processorAffinity;
			}
			set
			{
				SafeProcessHandle safeProcessHandle = null;
				try
				{
					safeProcessHandle = this.GetProcessHandle(512);
					if (!NativeMethods.SetProcessAffinityMask(safeProcessHandle, value))
					{
						throw new Win32Exception();
					}
					this.processorAffinity = value;
					this.haveProcessorAffinity = true;
				}
				finally
				{
					this.ReleaseProcessHandle(safeProcessHandle);
				}
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x000FA018 File Offset: 0x000F9018
		[MonitoringDescription("ProcessResponding")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Responding
		{
			get
			{
				if (!this.haveResponding)
				{
					IntPtr intPtr = this.MainWindowHandle;
					if (intPtr == (IntPtr)0)
					{
						this.responding = true;
					}
					else
					{
						IntPtr intPtr2;
						this.responding = NativeMethods.SendMessageTimeout(new HandleRef(this, intPtr), 0, IntPtr.Zero, IntPtr.Zero, 2, 5000, out intPtr2) != (IntPtr)0;
					}
				}
				return this.responding;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000FA081 File Offset: 0x000F9081
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessSessionId")]
		public int SessionId
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.sessionId;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000FA096 File Offset: 0x000F9096
		// (set) Token: 0x06003ADD RID: 15069 RVA: 0x000FA0B2 File Offset: 0x000F90B2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[MonitoringDescription("ProcessStartInfo")]
		public ProcessStartInfo StartInfo
		{
			get
			{
				if (this.startInfo == null)
				{
					this.startInfo = new ProcessStartInfo(this);
				}
				return this.startInfo;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.startInfo = value;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000FA0C9 File Offset: 0x000F90C9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStartTime")]
		public DateTime StartTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().StartTime;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000FA0E0 File Offset: 0x000F90E0
		// (set) Token: 0x06003AE0 RID: 15072 RVA: 0x000FA13A File Offset: 0x000F913A
		[Browsable(false)]
		[DefaultValue(null)]
		[MonitoringDescription("ProcessSynchronizingObject")]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x000FA144 File Offset: 0x000F9144
		[MonitoringDescription("ProcessThreads")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProcessThreadCollection Threads
		{
			get
			{
				if (this.threads == null)
				{
					this.EnsureState(Process.State.HaveProcessInfo);
					int count = this.processInfo.threadInfoList.Count;
					ProcessThread[] array = new ProcessThread[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new ProcessThread(this.isRemoteMachine, (ThreadInfo)this.processInfo.threadInfoList[i]);
					}
					ProcessThreadCollection processThreadCollection = new ProcessThreadCollection(array);
					this.threads = processThreadCollection;
				}
				return this.threads;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x000FA1BC File Offset: 0x000F91BC
		[MonitoringDescription("ProcessTotalProcessorTime")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan TotalProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().TotalProcessorTime;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x000FA1D0 File Offset: 0x000F91D0
		[MonitoringDescription("ProcessUserProcessorTime")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan UserProcessorTime
		{
			get
			{
				this.EnsureState(Process.State.IsNt);
				return this.GetProcessTimes().UserProcessorTime;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x000FA1E4 File Offset: 0x000F91E4
		[MonitoringDescription("ProcessVirtualMemorySize")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.VirtualMemorySize64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int VirtualMemorySize
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.virtualBytes;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x000FA1FA File Offset: 0x000F91FA
		[MonitoringDescription("ProcessVirtualMemorySize")]
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long VirtualMemorySize64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.virtualBytes;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x000FA20F File Offset: 0x000F920F
		// (set) Token: 0x06003AE7 RID: 15079 RVA: 0x000FA217 File Offset: 0x000F9217
		[Browsable(false)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessEnableRaisingEvents")]
		public bool EnableRaisingEvents
		{
			get
			{
				return this.watchForExit;
			}
			set
			{
				if (value != this.watchForExit)
				{
					if (this.Associated)
					{
						if (value)
						{
							this.OpenProcessHandle();
							this.EnsureWatchingForExit();
						}
						else
						{
							this.StopWatchingForExit();
						}
					}
					this.watchForExit = value;
				}
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x000FA249 File Offset: 0x000F9249
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStandardInput")]
		[Browsable(false)]
		public StreamWriter StandardInput
		{
			get
			{
				if (this.standardInput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardIn"));
				}
				return this.standardInput;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x000FA26C File Offset: 0x000F926C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStandardOutput")]
		public StreamReader StandardOutput
		{
			get
			{
				if (this.standardOutput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardOut"));
				}
				if (this.outputStreamReadMode == Process.StreamReadMode.undefined)
				{
					this.outputStreamReadMode = Process.StreamReadMode.syncMode;
				}
				else if (this.outputStreamReadMode != Process.StreamReadMode.syncMode)
				{
					throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
				}
				return this.standardOutput;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000FA2C4 File Offset: 0x000F92C4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("ProcessStandardError")]
		[Browsable(false)]
		public StreamReader StandardError
		{
			get
			{
				if (this.standardError == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardError"));
				}
				if (this.errorStreamReadMode == Process.StreamReadMode.undefined)
				{
					this.errorStreamReadMode = Process.StreamReadMode.syncMode;
				}
				else if (this.errorStreamReadMode != Process.StreamReadMode.syncMode)
				{
					throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
				}
				return this.standardError;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x000FA319 File Offset: 0x000F9319
		[MonitoringDescription("ProcessWorkingSet")]
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.Process.WorkingSet64 instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int WorkingSet
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return (int)this.processInfo.workingSet;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000FA32F File Offset: 0x000F932F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		[MonitoringDescription("ProcessWorkingSet")]
		public long WorkingSet64
		{
			get
			{
				this.EnsureState(Process.State.HaveNtProcessInfo);
				return this.processInfo.workingSet;
			}
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06003AED RID: 15085 RVA: 0x000FA344 File Offset: 0x000F9344
		// (remove) Token: 0x06003AEE RID: 15086 RVA: 0x000FA35D File Offset: 0x000F935D
		[Category("Behavior")]
		[MonitoringDescription("ProcessExited")]
		public event EventHandler Exited
		{
			add
			{
				this.onExited = (EventHandler)Delegate.Combine(this.onExited, value);
			}
			remove
			{
				this.onExited = (EventHandler)Delegate.Remove(this.onExited, value);
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000FA378 File Offset: 0x000F9378
		public bool CloseMainWindow()
		{
			IntPtr intPtr = this.MainWindowHandle;
			if (intPtr == (IntPtr)0)
			{
				return false;
			}
			int windowLong = NativeMethods.GetWindowLong(new HandleRef(this, intPtr), -16);
			if ((windowLong & 134217728) != 0)
			{
				return false;
			}
			NativeMethods.PostMessage(new HandleRef(this, intPtr), 16, IntPtr.Zero, IntPtr.Zero);
			return true;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000FA3D0 File Offset: 0x000F93D0
		private void ReleaseProcessHandle(SafeProcessHandle handle)
		{
			if (handle == null)
			{
				return;
			}
			if (this.haveProcessHandle && handle == this.m_processHandle)
			{
				return;
			}
			handle.Close();
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000FA3EE File Offset: 0x000F93EE
		private void CompletionCallback(object context, bool wasSignaled)
		{
			this.StopWatchingForExit();
			this.RaiseOnExited();
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000FA3FC File Offset: 0x000F93FC
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.Close();
				}
				this.disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000FA420 File Offset: 0x000F9420
		public void Close()
		{
			if (this.Associated)
			{
				if (this.haveProcessHandle)
				{
					this.StopWatchingForExit();
					this.m_processHandle.Close();
					this.m_processHandle = null;
					this.haveProcessHandle = false;
				}
				this.haveProcessId = false;
				this.isRemoteMachine = false;
				this.machineName = ".";
				this.raisedOnExited = false;
				this.standardOutput = null;
				this.standardInput = null;
				this.standardError = null;
				this.Refresh();
			}
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x000FA498 File Offset: 0x000F9498
		private void EnsureState(Process.State state)
		{
			if ((state & Process.State.IsWin2k) != (Process.State)0 && (this.OperatingSystem.Platform != PlatformID.Win32NT || this.OperatingSystem.Version.Major < 5))
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2kRequired"));
			}
			if ((state & Process.State.IsNt) != (Process.State)0 && this.OperatingSystem.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
			if ((state & Process.State.Associated) != (Process.State)0 && !this.Associated)
			{
				throw new InvalidOperationException(SR.GetString("NoAssociatedProcess"));
			}
			if ((state & Process.State.HaveId) != (Process.State)0 && !this.haveProcessId)
			{
				if (!this.haveProcessHandle)
				{
					this.EnsureState(Process.State.Associated);
					throw new InvalidOperationException(SR.GetString("ProcessIdRequired"));
				}
				this.SetProcessId(ProcessManager.GetProcessIdFromHandle(this.m_processHandle));
			}
			if ((state & Process.State.IsLocal) != (Process.State)0 && this.isRemoteMachine)
			{
				throw new NotSupportedException(SR.GetString("NotSupportedRemote"));
			}
			if ((state & Process.State.HaveProcessInfo) != (Process.State)0 && this.processInfo == null)
			{
				if ((state & Process.State.HaveId) == (Process.State)0)
				{
					this.EnsureState(Process.State.HaveId);
				}
				ProcessInfo[] processInfos = ProcessManager.GetProcessInfos(this.machineName);
				for (int i = 0; i < processInfos.Length; i++)
				{
					if (processInfos[i].processId == this.processId)
					{
						this.processInfo = processInfos[i];
						break;
					}
				}
				if (this.processInfo == null)
				{
					throw new InvalidOperationException(SR.GetString("NoProcessInfo"));
				}
			}
			if ((state & Process.State.Exited) != (Process.State)0)
			{
				if (!this.HasExited)
				{
					throw new InvalidOperationException(SR.GetString("WaitTillExit"));
				}
				if (!this.haveProcessHandle)
				{
					throw new InvalidOperationException(SR.GetString("NoProcessHandle"));
				}
			}
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x000FA618 File Offset: 0x000F9618
		private void EnsureWatchingForExit()
		{
			if (!this.watchingForExit)
			{
				lock (this)
				{
					if (!this.watchingForExit)
					{
						this.watchingForExit = true;
						try
						{
							this.waitHandle = new ProcessWaitHandle(this.m_processHandle);
							this.registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(this.waitHandle, new WaitOrTimerCallback(this.CompletionCallback), null, -1, true);
						}
						catch
						{
							this.watchingForExit = false;
							throw;
						}
					}
				}
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x000FA6A8 File Offset: 0x000F96A8
		private void EnsureWorkingSetLimits()
		{
			this.EnsureState(Process.State.IsNt);
			if (!this.haveWorkingSetLimits)
			{
				SafeProcessHandle safeProcessHandle = null;
				try
				{
					safeProcessHandle = this.GetProcessHandle(1024);
					IntPtr intPtr;
					IntPtr intPtr2;
					if (!NativeMethods.GetProcessWorkingSetSize(safeProcessHandle, out intPtr, out intPtr2))
					{
						throw new Win32Exception();
					}
					this.minWorkingSet = intPtr;
					this.maxWorkingSet = intPtr2;
					this.haveWorkingSetLimits = true;
				}
				finally
				{
					this.ReleaseProcessHandle(safeProcessHandle);
				}
			}
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x000FA714 File Offset: 0x000F9714
		public static void EnterDebugMode()
		{
			if (ProcessManager.IsNt)
			{
				Process.SetPrivilege("SeDebugPrivilege", 2);
			}
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x000FA728 File Offset: 0x000F9728
		private static void SetPrivilege(string privilegeName, int attrib)
		{
			IntPtr intPtr = (IntPtr)0;
			NativeMethods.LUID luid = default(NativeMethods.LUID);
			IntPtr currentProcess = NativeMethods.GetCurrentProcess();
			if (!NativeMethods.OpenProcessToken(new HandleRef(null, currentProcess), 32, out intPtr))
			{
				throw new Win32Exception();
			}
			try
			{
				if (!NativeMethods.LookupPrivilegeValue(null, privilegeName, out luid))
				{
					throw new Win32Exception();
				}
				NativeMethods.TokenPrivileges tokenPrivileges = new NativeMethods.TokenPrivileges();
				tokenPrivileges.Luid = luid;
				tokenPrivileges.Attributes = attrib;
				NativeMethods.AdjustTokenPrivileges(new HandleRef(null, intPtr), false, tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
				if (Marshal.GetLastWin32Error() != 0)
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				SafeNativeMethods.CloseHandle(new HandleRef(null, intPtr));
			}
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x000FA7D0 File Offset: 0x000F97D0
		public static void LeaveDebugMode()
		{
			if (ProcessManager.IsNt)
			{
				Process.SetPrivilege("SeDebugPrivilege", 0);
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x000FA7E4 File Offset: 0x000F97E4
		public static Process GetProcessById(int processId, string machineName)
		{
			if (!ProcessManager.IsProcessRunning(processId, machineName))
			{
				throw new ArgumentException(SR.GetString("MissingProccess", new object[] { processId.ToString(CultureInfo.CurrentCulture) }));
			}
			return new Process(machineName, ProcessManager.IsRemoteMachine(machineName), processId, null);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000FA82F File Offset: 0x000F982F
		public static Process GetProcessById(int processId)
		{
			return Process.GetProcessById(processId, ".");
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x000FA83C File Offset: 0x000F983C
		public static Process[] GetProcessesByName(string processName)
		{
			return Process.GetProcessesByName(processName, ".");
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x000FA84C File Offset: 0x000F984C
		public static Process[] GetProcessesByName(string processName, string machineName)
		{
			if (processName == null)
			{
				processName = string.Empty;
			}
			Process[] processes = Process.GetProcesses(machineName);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < processes.Length; i++)
			{
				if (string.Equals(processName, processes[i].ProcessName, StringComparison.OrdinalIgnoreCase))
				{
					arrayList.Add(processes[i]);
				}
			}
			Process[] array = new Process[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x000FA8AE File Offset: 0x000F98AE
		public static Process[] GetProcesses()
		{
			return Process.GetProcesses(".");
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x000FA8BC File Offset: 0x000F98BC
		public static Process[] GetProcesses(string machineName)
		{
			bool flag = ProcessManager.IsRemoteMachine(machineName);
			ProcessInfo[] processInfos = ProcessManager.GetProcessInfos(machineName);
			Process[] array = new Process[processInfos.Length];
			for (int i = 0; i < processInfos.Length; i++)
			{
				ProcessInfo processInfo = processInfos[i];
				array[i] = new Process(machineName, flag, processInfo.processId, processInfo);
			}
			return array;
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x000FA907 File Offset: 0x000F9907
		public static Process GetCurrentProcess()
		{
			return new Process(".", false, NativeMethods.GetCurrentProcessId(), null);
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x000FA91C File Offset: 0x000F991C
		protected void OnExited()
		{
			EventHandler eventHandler = this.onExited;
			if (eventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(eventHandler, new object[]
					{
						this,
						EventArgs.Empty
					});
					return;
				}
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000FA978 File Offset: 0x000F9978
		private SafeProcessHandle GetProcessHandle(int access, bool throwIfExited)
		{
			if (this.haveProcessHandle)
			{
				if (throwIfExited)
				{
					ProcessWaitHandle processWaitHandle = null;
					try
					{
						processWaitHandle = new ProcessWaitHandle(this.m_processHandle);
						if (processWaitHandle.WaitOne(0, false))
						{
							if (this.haveProcessId)
							{
								throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[] { this.processId.ToString(CultureInfo.CurrentCulture) }));
							}
							throw new InvalidOperationException(SR.GetString("ProcessHasExitedNoId"));
						}
					}
					finally
					{
						if (processWaitHandle != null)
						{
							processWaitHandle.Close();
						}
					}
				}
				return this.m_processHandle;
			}
			this.EnsureState((Process.State)3);
			SafeProcessHandle safeProcessHandle = SafeProcessHandle.InvalidHandle;
			safeProcessHandle = ProcessManager.OpenProcess(this.processId, access, throwIfExited);
			if (throwIfExited && (access & 1024) != 0 && NativeMethods.GetExitCodeProcess(safeProcessHandle, out this.exitCode) && this.exitCode != 259)
			{
				throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[] { this.processId.ToString(CultureInfo.CurrentCulture) }));
			}
			return safeProcessHandle;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000FAA7C File Offset: 0x000F9A7C
		private SafeProcessHandle GetProcessHandle(int access)
		{
			return this.GetProcessHandle(access, true);
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000FAA86 File Offset: 0x000F9A86
		private SafeProcessHandle OpenProcessHandle()
		{
			if (!this.haveProcessHandle)
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				this.SetProcessHandle(this.GetProcessHandle(2035711));
			}
			return this.m_processHandle;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000FAAC0 File Offset: 0x000F9AC0
		private void RaiseOnExited()
		{
			if (!this.raisedOnExited)
			{
				lock (this)
				{
					if (!this.raisedOnExited)
					{
						this.raisedOnExited = true;
						this.OnExited();
					}
				}
			}
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000FAB0C File Offset: 0x000F9B0C
		public void Refresh()
		{
			this.processInfo = null;
			this.threads = null;
			this.modules = null;
			this.mainWindowTitle = null;
			this.exited = false;
			this.signaled = false;
			this.haveMainWindow = false;
			this.haveWorkingSetLimits = false;
			this.haveProcessorAffinity = false;
			this.havePriorityClass = false;
			this.haveExitTime = false;
			this.haveResponding = false;
			this.havePriorityBoostEnabled = false;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000FAB74 File Offset: 0x000F9B74
		private void SetProcessHandle(SafeProcessHandle processHandle)
		{
			this.m_processHandle = processHandle;
			this.haveProcessHandle = true;
			if (this.watchForExit)
			{
				this.EnsureWatchingForExit();
			}
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000FAB92 File Offset: 0x000F9B92
		private void SetProcessId(int processId)
		{
			this.processId = processId;
			this.haveProcessId = true;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000FABA4 File Offset: 0x000F9BA4
		private void SetWorkingSetLimits(object newMin, object newMax)
		{
			this.EnsureState(Process.State.IsNt);
			SafeProcessHandle safeProcessHandle = null;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1280);
				IntPtr intPtr;
				IntPtr intPtr2;
				if (!NativeMethods.GetProcessWorkingSetSize(safeProcessHandle, out intPtr, out intPtr2))
				{
					throw new Win32Exception();
				}
				if (newMin != null)
				{
					intPtr = (IntPtr)newMin;
				}
				if (newMax != null)
				{
					intPtr2 = (IntPtr)newMax;
				}
				if ((long)intPtr > (long)intPtr2)
				{
					if (newMin != null)
					{
						throw new ArgumentException(SR.GetString("BadMinWorkset"));
					}
					throw new ArgumentException(SR.GetString("BadMaxWorkset"));
				}
				else
				{
					if (!NativeMethods.SetProcessWorkingSetSize(safeProcessHandle, intPtr, intPtr2))
					{
						throw new Win32Exception();
					}
					if (!NativeMethods.GetProcessWorkingSetSize(safeProcessHandle, out intPtr, out intPtr2))
					{
						throw new Win32Exception();
					}
					this.minWorkingSet = intPtr;
					this.maxWorkingSet = intPtr2;
					this.haveWorkingSetLimits = true;
				}
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000FAC70 File Offset: 0x000F9C70
		public bool Start()
		{
			this.Close();
			ProcessStartInfo processStartInfo = this.StartInfo;
			if (processStartInfo.FileName.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("FileNameMissing"));
			}
			if (processStartInfo.UseShellExecute)
			{
				return this.StartWithShellExecuteEx(processStartInfo);
			}
			return this.StartWithCreateProcess(processStartInfo);
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000FACC0 File Offset: 0x000F9CC0
		private static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize)
		{
			bool flag = NativeMethods.CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize);
			if (!flag || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000FACF4 File Offset: 0x000F9CF4
		private void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
		{
			NativeMethods.SECURITY_ATTRIBUTES security_ATTRIBUTES = new NativeMethods.SECURITY_ATTRIBUTES();
			security_ATTRIBUTES.bInheritHandle = true;
			SafeFileHandle safeFileHandle = null;
			try
			{
				if (parentInputs)
				{
					Process.CreatePipeWithSecurityAttributes(out childHandle, out safeFileHandle, security_ATTRIBUTES, 0);
				}
				else
				{
					Process.CreatePipeWithSecurityAttributes(out safeFileHandle, out childHandle, security_ATTRIBUTES, 0);
				}
				if (!NativeMethods.DuplicateHandle(new HandleRef(this, NativeMethods.GetCurrentProcess()), safeFileHandle, new HandleRef(this, NativeMethods.GetCurrentProcess()), out parentHandle, 0, false, 2))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				if (safeFileHandle != null && !safeFileHandle.IsInvalid)
				{
					safeFileHandle.Close();
				}
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000FAD78 File Offset: 0x000F9D78
		private static StringBuilder BuildCommandLine(string executableFileName, string arguments)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = executableFileName.Trim();
			bool flag = text.StartsWith("\"", StringComparison.Ordinal) && text.EndsWith("\"", StringComparison.Ordinal);
			if (!flag)
			{
				stringBuilder.Append("\"");
			}
			stringBuilder.Append(text);
			if (!flag)
			{
				stringBuilder.Append("\"");
			}
			if (!string.IsNullOrEmpty(arguments))
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(arguments);
			}
			return stringBuilder;
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000FADF4 File Offset: 0x000F9DF4
		private bool StartWithCreateProcess(ProcessStartInfo startInfo)
		{
			if (startInfo.StandardOutputEncoding != null && !startInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException(SR.GetString("StandardOutputEncodingNotAllowed"));
			}
			if (startInfo.StandardErrorEncoding != null && !startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException(SR.GetString("StandardErrorEncodingNotAllowed"));
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			StringBuilder stringBuilder = Process.BuildCommandLine(startInfo.FileName, startInfo.Arguments);
			NativeMethods.STARTUPINFO startupinfo = new NativeMethods.STARTUPINFO();
			SafeNativeMethods.PROCESS_INFORMATION process_INFORMATION = new SafeNativeMethods.PROCESS_INFORMATION();
			SafeProcessHandle safeProcessHandle = new SafeProcessHandle();
			SafeThreadHandle safeThreadHandle = new SafeThreadHandle();
			int num = 0;
			SafeFileHandle safeFileHandle = null;
			SafeFileHandle safeFileHandle2 = null;
			SafeFileHandle safeFileHandle3 = null;
			GCHandle gchandle = default(GCHandle);
			try
			{
				if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
				{
					if (startInfo.RedirectStandardInput)
					{
						this.CreatePipe(out safeFileHandle, out startupinfo.hStdInput, true);
					}
					else
					{
						startupinfo.hStdInput = new SafeFileHandle(NativeMethods.GetStdHandle(-10), false);
					}
					if (startInfo.RedirectStandardOutput)
					{
						this.CreatePipe(out safeFileHandle2, out startupinfo.hStdOutput, false);
					}
					else
					{
						startupinfo.hStdOutput = new SafeFileHandle(NativeMethods.GetStdHandle(-11), false);
					}
					if (startInfo.RedirectStandardError)
					{
						this.CreatePipe(out safeFileHandle3, out startupinfo.hStdError, false);
					}
					else
					{
						startupinfo.hStdError = new SafeFileHandle(NativeMethods.GetStdHandle(-12), false);
					}
					startupinfo.dwFlags = 256;
				}
				int num2 = 0;
				if (startInfo.CreateNoWindow)
				{
					num2 |= 134217728;
				}
				IntPtr intPtr = (IntPtr)0;
				if (startInfo.environmentVariables != null)
				{
					bool flag = false;
					if (ProcessManager.IsNt)
					{
						num2 |= 1024;
						flag = true;
					}
					byte[] array = EnvironmentBlock.ToByteArray(startInfo.environmentVariables, flag);
					gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
					intPtr = gchandle.AddrOfPinnedObject();
				}
				string text = startInfo.WorkingDirectory;
				if (text == string.Empty)
				{
					text = Environment.CurrentDirectory;
				}
				bool flag2;
				if (startInfo.UserName.Length != 0)
				{
					NativeMethods.LogonFlags logonFlags = (NativeMethods.LogonFlags)0;
					if (startInfo.LoadUserProfile)
					{
						logonFlags = NativeMethods.LogonFlags.LOGON_WITH_PROFILE;
					}
					IntPtr intPtr2 = IntPtr.Zero;
					try
					{
						if (startInfo.Password == null)
						{
							intPtr2 = Marshal.StringToCoTaskMemUni(string.Empty);
						}
						else
						{
							intPtr2 = Marshal.SecureStringToCoTaskMemUnicode(startInfo.Password);
						}
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
						}
						finally
						{
							flag2 = NativeMethods.CreateProcessWithLogonW(startInfo.UserName, startInfo.Domain, intPtr2, logonFlags, null, stringBuilder, num2, intPtr, text, startupinfo, process_INFORMATION);
							if (!flag2)
							{
								num = Marshal.GetLastWin32Error();
							}
							if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
							}
							if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
							{
								safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
							}
						}
						if (flag2)
						{
							goto IL_0395;
						}
						if (num == 193)
						{
							throw new Win32Exception(num, SR.GetString("InvalidApplication"));
						}
						throw new Win32Exception(num);
					}
					finally
					{
						if (intPtr2 != IntPtr.Zero)
						{
							Marshal.ZeroFreeCoTaskMemUnicode(intPtr2);
						}
					}
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					flag2 = NativeMethods.CreateProcess(null, stringBuilder, null, null, true, num2, intPtr, text, startupinfo, process_INFORMATION);
					if (!flag2)
					{
						num = Marshal.GetLastWin32Error();
					}
					if (process_INFORMATION.hProcess != (IntPtr)0 && process_INFORMATION.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
					{
						safeProcessHandle.InitialSetHandle(process_INFORMATION.hProcess);
					}
					if (process_INFORMATION.hThread != (IntPtr)0 && process_INFORMATION.hThread != NativeMethods.INVALID_HANDLE_VALUE)
					{
						safeThreadHandle.InitialSetHandle(process_INFORMATION.hThread);
					}
				}
				if (!flag2)
				{
					if (num == 193)
					{
						throw new Win32Exception(num, SR.GetString("InvalidApplication"));
					}
					throw new Win32Exception(num);
				}
				IL_0395:;
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				startupinfo.Dispose();
			}
			if (startInfo.RedirectStandardInput)
			{
				this.standardInput = new StreamWriter(new FileStream(safeFileHandle, FileAccess.Write, 4096, false), Encoding.GetEncoding(NativeMethods.GetConsoleCP()), 4096);
				this.standardInput.AutoFlush = true;
			}
			if (startInfo.RedirectStandardOutput)
			{
				Encoding encoding = ((startInfo.StandardOutputEncoding != null) ? startInfo.StandardOutputEncoding : Encoding.GetEncoding(NativeMethods.GetConsoleOutputCP()));
				this.standardOutput = new StreamReader(new FileStream(safeFileHandle2, FileAccess.Read, 4096, false), encoding, true, 4096);
			}
			if (startInfo.RedirectStandardError)
			{
				Encoding encoding2 = ((startInfo.StandardErrorEncoding != null) ? startInfo.StandardErrorEncoding : Encoding.GetEncoding(NativeMethods.GetConsoleOutputCP()));
				this.standardError = new StreamReader(new FileStream(safeFileHandle3, FileAccess.Read, 4096, false), encoding2, true, 4096);
			}
			bool flag3 = false;
			if (!safeProcessHandle.IsInvalid)
			{
				this.SetProcessHandle(safeProcessHandle);
				this.SetProcessId(process_INFORMATION.dwProcessId);
				safeThreadHandle.Close();
				flag3 = true;
			}
			return flag3;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x000FB304 File Offset: 0x000FA304
		private bool StartWithShellExecuteEx(ProcessStartInfo startInfo)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!string.IsNullOrEmpty(startInfo.UserName) || startInfo.Password != null)
			{
				throw new InvalidOperationException(SR.GetString("CantStartAsUser"));
			}
			if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException(SR.GetString("CantRedirectStreams"));
			}
			if (startInfo.StandardErrorEncoding != null)
			{
				throw new InvalidOperationException(SR.GetString("StandardErrorEncodingNotAllowed"));
			}
			if (startInfo.StandardOutputEncoding != null)
			{
				throw new InvalidOperationException(SR.GetString("StandardOutputEncodingNotAllowed"));
			}
			if (startInfo.environmentVariables != null)
			{
				throw new InvalidOperationException(SR.GetString("CantUseEnvVars"));
			}
			NativeMethods.ShellExecuteInfo shellExecuteInfo = new NativeMethods.ShellExecuteInfo();
			shellExecuteInfo.fMask = 64;
			if (startInfo.ErrorDialog)
			{
				shellExecuteInfo.hwnd = startInfo.ErrorDialogParentHandle;
			}
			else
			{
				shellExecuteInfo.fMask |= 1024;
			}
			switch (startInfo.WindowStyle)
			{
			case ProcessWindowStyle.Hidden:
				shellExecuteInfo.nShow = 0;
				break;
			case ProcessWindowStyle.Minimized:
				shellExecuteInfo.nShow = 2;
				break;
			case ProcessWindowStyle.Maximized:
				shellExecuteInfo.nShow = 3;
				break;
			default:
				shellExecuteInfo.nShow = 1;
				break;
			}
			try
			{
				if (startInfo.FileName.Length != 0)
				{
					shellExecuteInfo.lpFile = Marshal.StringToHGlobalAuto(startInfo.FileName);
				}
				if (startInfo.Verb.Length != 0)
				{
					shellExecuteInfo.lpVerb = Marshal.StringToHGlobalAuto(startInfo.Verb);
				}
				if (startInfo.Arguments.Length != 0)
				{
					shellExecuteInfo.lpParameters = Marshal.StringToHGlobalAuto(startInfo.Arguments);
				}
				if (startInfo.WorkingDirectory.Length != 0)
				{
					shellExecuteInfo.lpDirectory = Marshal.StringToHGlobalAuto(startInfo.WorkingDirectory);
				}
				shellExecuteInfo.fMask |= 256;
				ShellExecuteHelper shellExecuteHelper = new ShellExecuteHelper(shellExecuteInfo);
				if (!shellExecuteHelper.ShellExecuteOnSTAThread())
				{
					int num = shellExecuteHelper.ErrorCode;
					if (num == 0)
					{
						long num2 = (long)shellExecuteInfo.hInstApp;
						if (num2 <= 8L)
						{
							if (num2 < 2L)
							{
								goto IL_0276;
							}
							switch ((int)(num2 - 2L))
							{
							case 0:
								num = 2;
								goto IL_0282;
							case 1:
								num = 3;
								goto IL_0282;
							case 2:
							case 4:
							case 5:
								goto IL_0276;
							case 3:
								num = 5;
								goto IL_0282;
							case 6:
								num = 8;
								goto IL_0282;
							}
						}
						if (num2 <= 32L && num2 >= 26L)
						{
							switch ((int)(num2 - 26L))
							{
							case 0:
								num = 32;
								goto IL_0282;
							case 2:
							case 3:
							case 4:
								num = 1156;
								goto IL_0282;
							case 5:
								num = 1155;
								goto IL_0282;
							case 6:
								num = 1157;
								goto IL_0282;
							}
						}
						IL_0276:
						num = (int)shellExecuteInfo.hInstApp;
					}
					IL_0282:
					throw new Win32Exception(num);
				}
			}
			finally
			{
				if (shellExecuteInfo.lpFile != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpFile);
				}
				if (shellExecuteInfo.lpVerb != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpVerb);
				}
				if (shellExecuteInfo.lpParameters != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpParameters);
				}
				if (shellExecuteInfo.lpDirectory != (IntPtr)0)
				{
					Marshal.FreeHGlobal(shellExecuteInfo.lpDirectory);
				}
			}
			if (shellExecuteInfo.hProcess != (IntPtr)0)
			{
				SafeProcessHandle safeProcessHandle = new SafeProcessHandle(shellExecuteInfo.hProcess);
				this.SetProcessHandle(safeProcessHandle);
				return true;
			}
			return false;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x000FB65C File Offset: 0x000FA65C
		public static Process Start(string fileName, string userName, SecureString password, string domain)
		{
			return Process.Start(new ProcessStartInfo(fileName)
			{
				UserName = userName,
				Password = password,
				Domain = domain,
				UseShellExecute = false
			});
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000FB694 File Offset: 0x000FA694
		public static Process Start(string fileName, string arguments, string userName, SecureString password, string domain)
		{
			return Process.Start(new ProcessStartInfo(fileName, arguments)
			{
				UserName = userName,
				Password = password,
				Domain = domain,
				UseShellExecute = false
			});
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000FB6CC File Offset: 0x000FA6CC
		public static Process Start(string fileName)
		{
			return Process.Start(new ProcessStartInfo(fileName));
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x000FB6D9 File Offset: 0x000FA6D9
		public static Process Start(string fileName, string arguments)
		{
			return Process.Start(new ProcessStartInfo(fileName, arguments));
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x000FB6E8 File Offset: 0x000FA6E8
		public static Process Start(ProcessStartInfo startInfo)
		{
			Process process = new Process();
			if (startInfo == null)
			{
				throw new ArgumentNullException("startInfo");
			}
			process.StartInfo = startInfo;
			if (process.Start())
			{
				return process;
			}
			return null;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x000FB71C File Offset: 0x000FA71C
		public void Kill()
		{
			SafeProcessHandle safeProcessHandle = null;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1);
				if (!NativeMethods.TerminateProcess(safeProcessHandle, -1))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x000FB75C File Offset: 0x000FA75C
		private void StopWatchingForExit()
		{
			if (this.watchingForExit)
			{
				lock (this)
				{
					if (this.watchingForExit)
					{
						this.watchingForExit = false;
						this.registeredWaitHandle.Unregister(null);
						this.waitHandle.Close();
						this.waitHandle = null;
						this.registeredWaitHandle = null;
					}
				}
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x000FB7C8 File Offset: 0x000FA7C8
		public override string ToString()
		{
			if (!this.Associated)
			{
				return base.ToString();
			}
			string text = string.Empty;
			try
			{
				text = this.ProcessName;
			}
			catch (PlatformNotSupportedException)
			{
			}
			if (text.Length != 0)
			{
				return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
				{
					base.ToString(),
					text
				});
			}
			return base.ToString();
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x000FB83C File Offset: 0x000FA83C
		public bool WaitForExit(int milliseconds)
		{
			SafeProcessHandle safeProcessHandle = null;
			ProcessWaitHandle processWaitHandle = null;
			bool flag;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1048576, false);
				if (safeProcessHandle.IsInvalid)
				{
					flag = true;
				}
				else
				{
					processWaitHandle = new ProcessWaitHandle(safeProcessHandle);
					if (processWaitHandle.WaitOne(milliseconds, false))
					{
						flag = true;
						this.signaled = true;
					}
					else
					{
						flag = false;
						this.signaled = false;
					}
				}
			}
			finally
			{
				if (processWaitHandle != null)
				{
					processWaitHandle.Close();
				}
				if (this.output != null && milliseconds == 2147483647)
				{
					this.output.WaitUtilEOF();
				}
				if (this.error != null && milliseconds == 2147483647)
				{
					this.error.WaitUtilEOF();
				}
				this.ReleaseProcessHandle(safeProcessHandle);
			}
			if (flag && this.watchForExit)
			{
				this.RaiseOnExited();
			}
			return flag;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x000FB8F8 File Offset: 0x000FA8F8
		public void WaitForExit()
		{
			this.WaitForExit(int.MaxValue);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x000FB908 File Offset: 0x000FA908
		public bool WaitForInputIdle(int milliseconds)
		{
			SafeProcessHandle safeProcessHandle = null;
			bool flag;
			try
			{
				safeProcessHandle = this.GetProcessHandle(1049600);
				int num = NativeMethods.WaitForInputIdle(safeProcessHandle, milliseconds);
				int num2 = num;
				switch (num2)
				{
				case -1:
					break;
				case 0:
					flag = true;
					goto IL_004A;
				default:
					if (num2 == 258)
					{
						flag = false;
						goto IL_004A;
					}
					break;
				}
				throw new InvalidOperationException(SR.GetString("InputIdleUnkownError"));
				IL_004A:;
			}
			finally
			{
				this.ReleaseProcessHandle(safeProcessHandle);
			}
			return flag;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x000FB97C File Offset: 0x000FA97C
		public bool WaitForInputIdle()
		{
			return this.WaitForInputIdle(int.MaxValue);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x000FB98C File Offset: 0x000FA98C
		[ComVisible(false)]
		public void BeginOutputReadLine()
		{
			if (this.outputStreamReadMode == Process.StreamReadMode.undefined)
			{
				this.outputStreamReadMode = Process.StreamReadMode.asyncMode;
			}
			else if (this.outputStreamReadMode != Process.StreamReadMode.asyncMode)
			{
				throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
			}
			if (this.pendingOutputRead)
			{
				throw new InvalidOperationException(SR.GetString("PendingAsyncOperation"));
			}
			this.pendingOutputRead = true;
			if (this.output == null)
			{
				if (this.standardOutput == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardOut"));
				}
				Stream baseStream = this.standardOutput.BaseStream;
				this.output = new AsyncStreamReader(this, baseStream, new UserCallBack(this.OutputReadNotifyUser), this.standardOutput.CurrentEncoding);
			}
			this.output.BeginReadLine();
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x000FBA40 File Offset: 0x000FAA40
		[ComVisible(false)]
		public void BeginErrorReadLine()
		{
			if (this.errorStreamReadMode == Process.StreamReadMode.undefined)
			{
				this.errorStreamReadMode = Process.StreamReadMode.asyncMode;
			}
			else if (this.errorStreamReadMode != Process.StreamReadMode.asyncMode)
			{
				throw new InvalidOperationException(SR.GetString("CantMixSyncAsyncOperation"));
			}
			if (this.pendingErrorRead)
			{
				throw new InvalidOperationException(SR.GetString("PendingAsyncOperation"));
			}
			this.pendingErrorRead = true;
			if (this.error == null)
			{
				if (this.standardError == null)
				{
					throw new InvalidOperationException(SR.GetString("CantGetStandardError"));
				}
				Stream baseStream = this.standardError.BaseStream;
				this.error = new AsyncStreamReader(this, baseStream, new UserCallBack(this.ErrorReadNotifyUser), this.standardError.CurrentEncoding);
			}
			this.error.BeginReadLine();
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x000FBAF1 File Offset: 0x000FAAF1
		[ComVisible(false)]
		public void CancelOutputRead()
		{
			if (this.output != null)
			{
				this.output.CancelOperation();
				this.pendingOutputRead = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("NoAsyncOperation"));
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x000FBB1F File Offset: 0x000FAB1F
		[ComVisible(false)]
		public void CancelErrorRead()
		{
			if (this.error != null)
			{
				this.error.CancelOperation();
				this.pendingErrorRead = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("NoAsyncOperation"));
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000FBB50 File Offset: 0x000FAB50
		internal void OutputReadNotifyUser(string data)
		{
			DataReceivedEventHandler outputDataReceived = this.OutputDataReceived;
			if (outputDataReceived != null)
			{
				DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs(data);
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.Invoke(outputDataReceived, new object[] { this, dataReceivedEventArgs });
					return;
				}
				outputDataReceived(this, dataReceivedEventArgs);
			}
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000FBBAC File Offset: 0x000FABAC
		internal void ErrorReadNotifyUser(string data)
		{
			DataReceivedEventHandler errorDataReceived = this.ErrorDataReceived;
			if (errorDataReceived != null)
			{
				DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs(data);
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.Invoke(errorDataReceived, new object[] { this, dataReceivedEventArgs });
					return;
				}
				errorDataReceived(this, dataReceivedEventArgs);
			}
		}

		// Token: 0x0400336F RID: 13167
		private bool haveProcessId;

		// Token: 0x04003370 RID: 13168
		private int processId;

		// Token: 0x04003371 RID: 13169
		private bool haveProcessHandle;

		// Token: 0x04003372 RID: 13170
		private SafeProcessHandle m_processHandle;

		// Token: 0x04003373 RID: 13171
		private bool isRemoteMachine;

		// Token: 0x04003374 RID: 13172
		private string machineName;

		// Token: 0x04003375 RID: 13173
		private ProcessInfo processInfo;

		// Token: 0x04003376 RID: 13174
		private ProcessThreadCollection threads;

		// Token: 0x04003377 RID: 13175
		private ProcessModuleCollection modules;

		// Token: 0x04003378 RID: 13176
		private bool haveMainWindow;

		// Token: 0x04003379 RID: 13177
		private IntPtr mainWindowHandle;

		// Token: 0x0400337A RID: 13178
		private string mainWindowTitle;

		// Token: 0x0400337B RID: 13179
		private bool haveWorkingSetLimits;

		// Token: 0x0400337C RID: 13180
		private IntPtr minWorkingSet;

		// Token: 0x0400337D RID: 13181
		private IntPtr maxWorkingSet;

		// Token: 0x0400337E RID: 13182
		private bool haveProcessorAffinity;

		// Token: 0x0400337F RID: 13183
		private IntPtr processorAffinity;

		// Token: 0x04003380 RID: 13184
		private bool havePriorityClass;

		// Token: 0x04003381 RID: 13185
		private ProcessPriorityClass priorityClass;

		// Token: 0x04003382 RID: 13186
		private ProcessStartInfo startInfo;

		// Token: 0x04003383 RID: 13187
		private bool watchForExit;

		// Token: 0x04003384 RID: 13188
		private bool watchingForExit;

		// Token: 0x04003385 RID: 13189
		private EventHandler onExited;

		// Token: 0x04003386 RID: 13190
		private bool exited;

		// Token: 0x04003387 RID: 13191
		private int exitCode;

		// Token: 0x04003388 RID: 13192
		private bool signaled;

		// Token: 0x04003389 RID: 13193
		private DateTime exitTime;

		// Token: 0x0400338A RID: 13194
		private bool haveExitTime;

		// Token: 0x0400338B RID: 13195
		private bool responding;

		// Token: 0x0400338C RID: 13196
		private bool haveResponding;

		// Token: 0x0400338D RID: 13197
		private bool priorityBoostEnabled;

		// Token: 0x0400338E RID: 13198
		private bool havePriorityBoostEnabled;

		// Token: 0x0400338F RID: 13199
		private bool raisedOnExited;

		// Token: 0x04003390 RID: 13200
		private RegisteredWaitHandle registeredWaitHandle;

		// Token: 0x04003391 RID: 13201
		private WaitHandle waitHandle;

		// Token: 0x04003392 RID: 13202
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x04003393 RID: 13203
		private StreamReader standardOutput;

		// Token: 0x04003394 RID: 13204
		private StreamWriter standardInput;

		// Token: 0x04003395 RID: 13205
		private StreamReader standardError;

		// Token: 0x04003396 RID: 13206
		private OperatingSystem operatingSystem;

		// Token: 0x04003397 RID: 13207
		private bool disposed;

		// Token: 0x04003398 RID: 13208
		private Process.StreamReadMode outputStreamReadMode;

		// Token: 0x04003399 RID: 13209
		private Process.StreamReadMode errorStreamReadMode;

		// Token: 0x0400339C RID: 13212
		internal AsyncStreamReader output;

		// Token: 0x0400339D RID: 13213
		internal AsyncStreamReader error;

		// Token: 0x0400339E RID: 13214
		internal bool pendingOutputRead;

		// Token: 0x0400339F RID: 13215
		internal bool pendingErrorRead;

		// Token: 0x040033A0 RID: 13216
		private static SafeFileHandle InvalidPipeHandle = new SafeFileHandle(IntPtr.Zero, false);

		// Token: 0x040033A1 RID: 13217
		internal static TraceSwitch processTracing = null;

		// Token: 0x02000775 RID: 1909
		private enum StreamReadMode
		{
			// Token: 0x040033A3 RID: 13219
			undefined,
			// Token: 0x040033A4 RID: 13220
			syncMode,
			// Token: 0x040033A5 RID: 13221
			asyncMode
		}

		// Token: 0x02000776 RID: 1910
		private enum State
		{
			// Token: 0x040033A7 RID: 13223
			HaveId = 1,
			// Token: 0x040033A8 RID: 13224
			IsLocal,
			// Token: 0x040033A9 RID: 13225
			IsNt = 4,
			// Token: 0x040033AA RID: 13226
			HaveProcessInfo = 8,
			// Token: 0x040033AB RID: 13227
			Exited = 16,
			// Token: 0x040033AC RID: 13228
			Associated = 32,
			// Token: 0x040033AD RID: 13229
			IsWin2k = 64,
			// Token: 0x040033AE RID: 13230
			HaveNtProcessInfo = 12
		}
	}
}
