using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Devices
{
	[DebuggerTypeProxy(typeof(ComputerInfo.ComputerInfoDebugView))]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class ComputerInfo
	{
		public ComputerInfo()
		{
			this.m_OSManagementObject = null;
			this.m_InternalMemoryStatus = null;
		}

		[CLSCompliant(false)]
		public ulong TotalPhysicalMemory
		{
			get
			{
				return this.MemoryStatus.TotalPhysicalMemory;
			}
		}

		[CLSCompliant(false)]
		public ulong AvailablePhysicalMemory
		{
			get
			{
				return this.MemoryStatus.AvailablePhysicalMemory;
			}
		}

		[CLSCompliant(false)]
		public ulong TotalVirtualMemory
		{
			get
			{
				return this.MemoryStatus.TotalVirtualMemory;
			}
		}

		[CLSCompliant(false)]
		public ulong AvailableVirtualMemory
		{
			get
			{
				return this.MemoryStatus.AvailableVirtualMemory;
			}
		}

		public CultureInfo InstalledUICulture
		{
			get
			{
				return CultureInfo.InstalledUICulture;
			}
		}

		public string OSFullName
		{
			get
			{
				string text3;
				try
				{
					string text = "Name";
					char c = '|';
					string text2 = Conversions.ToString(this.OSManagementBaseObject.Properties[text].Value);
					if (text2.Contains(Conversions.ToString(c)))
					{
						text3 = text2.Substring(0, text2.IndexOf(c));
					}
					else
					{
						text3 = text2;
					}
				}
				catch (COMException ex)
				{
					text3 = this.OSPlatform;
				}
				return text3;
			}
		}

		public string OSPlatform
		{
			get
			{
				return Environment.OSVersion.Platform.ToString();
			}
		}

		public string OSVersion
		{
			get
			{
				return Environment.OSVersion.Version.ToString();
			}
		}

		private ComputerInfo.InternalMemoryStatus MemoryStatus
		{
			get
			{
				if (this.m_InternalMemoryStatus == null)
				{
					this.m_InternalMemoryStatus = new ComputerInfo.InternalMemoryStatus();
				}
				return this.m_InternalMemoryStatus;
			}
		}

		private ManagementBaseObject OSManagementBaseObject
		{
			get
			{
				string text = "Win32_OperatingSystem";
				if (this.m_OSManagementObject == null)
				{
					SelectQuery selectQuery = new SelectQuery(text);
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(selectQuery);
					ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
					if (managementObjectCollection.Count <= 0)
					{
						throw ExceptionUtils.GetInvalidOperationException("DiagnosticInfo_FullOSName", new string[0]);
					}
					ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator();
					enumerator.MoveNext();
					this.m_OSManagementObject = enumerator.Current;
				}
				return this.m_OSManagementObject;
			}
		}

		private ManagementBaseObject m_OSManagementObject;

		private ComputerInfo.InternalMemoryStatus m_InternalMemoryStatus;

		internal sealed class ComputerInfoDebugView
		{
			public ComputerInfoDebugView(ComputerInfo RealClass)
			{
				this.m_InstanceBeingWatched = RealClass;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ulong TotalPhysicalMemory
			{
				get
				{
					return this.m_InstanceBeingWatched.TotalPhysicalMemory;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ulong AvailablePhysicalMemory
			{
				get
				{
					return this.m_InstanceBeingWatched.AvailablePhysicalMemory;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ulong TotalVirtualMemory
			{
				get
				{
					return this.m_InstanceBeingWatched.TotalVirtualMemory;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ulong AvailableVirtualMemory
			{
				get
				{
					return this.m_InstanceBeingWatched.AvailableVirtualMemory;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public CultureInfo InstalledUICulture
			{
				get
				{
					return this.m_InstanceBeingWatched.InstalledUICulture;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public string OSPlatform
			{
				get
				{
					return this.m_InstanceBeingWatched.OSPlatform;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public string OSVersion
			{
				get
				{
					return this.m_InstanceBeingWatched.OSVersion;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private ComputerInfo m_InstanceBeingWatched;
		}

		private class InternalMemoryStatus
		{
			public InternalMemoryStatus()
			{
				this.m_IsOldOS = Environment.OSVersion.Version.Major < 5;
			}

			internal ulong TotalPhysicalMemory
			{
				get
				{
					this.Refresh();
					if (this.m_IsOldOS)
					{
						return (ulong)this.m_MemoryStatus.dwTotalPhys;
					}
					return this.m_MemoryStatusEx.ullTotalPhys;
				}
			}

			internal ulong AvailablePhysicalMemory
			{
				get
				{
					this.Refresh();
					if (this.m_IsOldOS)
					{
						return (ulong)this.m_MemoryStatus.dwAvailPhys;
					}
					return this.m_MemoryStatusEx.ullAvailPhys;
				}
			}

			internal ulong TotalVirtualMemory
			{
				get
				{
					this.Refresh();
					if (this.m_IsOldOS)
					{
						return (ulong)this.m_MemoryStatus.dwTotalVirtual;
					}
					return this.m_MemoryStatusEx.ullTotalVirtual;
				}
			}

			internal ulong AvailableVirtualMemory
			{
				get
				{
					this.Refresh();
					if (this.m_IsOldOS)
					{
						return (ulong)this.m_MemoryStatus.dwAvailVirtual;
					}
					return this.m_MemoryStatusEx.ullAvailVirtual;
				}
			}

			private void Refresh()
			{
				if (this.m_IsOldOS)
				{
					this.m_MemoryStatus = default(NativeMethods.MEMORYSTATUS);
					NativeMethods.GlobalMemoryStatus(ref this.m_MemoryStatus);
				}
				else
				{
					this.m_MemoryStatusEx = default(NativeMethods.MEMORYSTATUSEX);
					this.m_MemoryStatusEx.Init();
					if (!NativeMethods.GlobalMemoryStatusEx(ref this.m_MemoryStatusEx))
					{
						throw ExceptionUtils.GetWin32Exception("DiagnosticInfo_Memory", new string[0]);
					}
				}
			}

			private bool m_IsOldOS;

			private NativeMethods.MEMORYSTATUS m_MemoryStatus;

			private NativeMethods.MEMORYSTATUSEX m_MemoryStatusEx;
		}
	}
}
