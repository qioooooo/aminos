using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020004F5 RID: 1269
	internal static class NetworkingPerfCounters
	{
		// Token: 0x060027A7 RID: 10151 RVA: 0x000A32E8 File Offset: 0x000A22E8
		internal static void Initialize()
		{
			if (!NetworkingPerfCounters.initialized)
			{
				lock (NetworkingPerfCounters.syncObject)
				{
					if (!NetworkingPerfCounters.initialized)
					{
						if (ComNetOS.IsWin2K)
						{
							PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PermissionState.Unrestricted);
							performanceCounterPermission.Assert();
							try
							{
								string instanceName = NetworkingPerfCounters.GetInstanceName();
								NetworkingPerfCounters.ConnectionsEstablished = new PerformanceCounter();
								NetworkingPerfCounters.ConnectionsEstablished.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.ConnectionsEstablished.CounterName = "Connections Established";
								NetworkingPerfCounters.ConnectionsEstablished.InstanceName = instanceName;
								NetworkingPerfCounters.ConnectionsEstablished.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.ConnectionsEstablished.ReadOnly = false;
								NetworkingPerfCounters.ConnectionsEstablished.RawValue = 0L;
								NetworkingPerfCounters.BytesReceived = new PerformanceCounter();
								NetworkingPerfCounters.BytesReceived.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.BytesReceived.CounterName = "Bytes Received";
								NetworkingPerfCounters.BytesReceived.InstanceName = instanceName;
								NetworkingPerfCounters.BytesReceived.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.BytesReceived.ReadOnly = false;
								NetworkingPerfCounters.BytesReceived.RawValue = 0L;
								NetworkingPerfCounters.BytesSent = new PerformanceCounter();
								NetworkingPerfCounters.BytesSent.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.BytesSent.CounterName = "Bytes Sent";
								NetworkingPerfCounters.BytesSent.InstanceName = instanceName;
								NetworkingPerfCounters.BytesSent.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.BytesSent.ReadOnly = false;
								NetworkingPerfCounters.BytesSent.RawValue = 0L;
								NetworkingPerfCounters.DatagramsReceived = new PerformanceCounter();
								NetworkingPerfCounters.DatagramsReceived.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.DatagramsReceived.CounterName = "Datagrams Received";
								NetworkingPerfCounters.DatagramsReceived.InstanceName = instanceName;
								NetworkingPerfCounters.DatagramsReceived.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.DatagramsReceived.ReadOnly = false;
								NetworkingPerfCounters.DatagramsReceived.RawValue = 0L;
								NetworkingPerfCounters.DatagramsSent = new PerformanceCounter();
								NetworkingPerfCounters.DatagramsSent.CategoryName = ".NET CLR Networking";
								NetworkingPerfCounters.DatagramsSent.CounterName = "Datagrams Sent";
								NetworkingPerfCounters.DatagramsSent.InstanceName = instanceName;
								NetworkingPerfCounters.DatagramsSent.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
								NetworkingPerfCounters.DatagramsSent.ReadOnly = false;
								NetworkingPerfCounters.DatagramsSent.RawValue = 0L;
								NetworkingPerfCounters.globalConnectionsEstablished = new PerformanceCounter(".NET CLR Networking", "Connections Established", "_Global_", false);
								NetworkingPerfCounters.globalBytesReceived = new PerformanceCounter(".NET CLR Networking", "Bytes Received", "_Global_", false);
								NetworkingPerfCounters.globalBytesSent = new PerformanceCounter(".NET CLR Networking", "Bytes Sent", "_Global_", false);
								NetworkingPerfCounters.globalDatagramsReceived = new PerformanceCounter(".NET CLR Networking", "Datagrams Received", "_Global_", false);
								NetworkingPerfCounters.globalDatagramsSent = new PerformanceCounter(".NET CLR Networking", "Datagrams Sent", "_Global_", false);
								AppDomain.CurrentDomain.DomainUnload += NetworkingPerfCounters.ExitOrUnloadEventHandler;
								AppDomain.CurrentDomain.ProcessExit += NetworkingPerfCounters.ExitOrUnloadEventHandler;
								AppDomain.CurrentDomain.UnhandledException += NetworkingPerfCounters.ExceptionEventHandler;
							}
							catch (Win32Exception)
							{
							}
							catch (InvalidOperationException)
							{
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
						NetworkingPerfCounters.initialized = true;
					}
				}
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000A3624 File Offset: 0x000A2624
		private static void ExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				NetworkingPerfCounters.Cleanup();
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000A3633 File Offset: 0x000A2633
		private static void ExitOrUnloadEventHandler(object sender, EventArgs e)
		{
			NetworkingPerfCounters.Cleanup();
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000A363C File Offset: 0x000A263C
		private static void Cleanup()
		{
			PerformanceCounter performanceCounter = NetworkingPerfCounters.ConnectionsEstablished;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.BytesReceived;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.BytesSent;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.DatagramsReceived;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
			performanceCounter = NetworkingPerfCounters.DatagramsSent;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000A3694 File Offset: 0x000A2694
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private static string GetAssemblyName()
		{
			string text = null;
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				AssemblyName name = entryAssembly.GetName();
				if (name != null)
				{
					text = name.Name;
				}
			}
			return text;
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000A36C0 File Offset: 0x000A26C0
		[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
		private static string GetInstanceName()
		{
			string text = NetworkingPerfCounters.GetAssemblyName();
			if (text == null || text.Length == 0)
			{
				text = AppDomain.CurrentDomain.FriendlyName;
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			int i = 0;
			while (i < stringBuilder.Length)
			{
				char c = stringBuilder[i];
				if (c <= ')')
				{
					if (c == '#')
					{
						goto IL_0076;
					}
					switch (c)
					{
					case '(':
						stringBuilder[i] = '[';
						break;
					case ')':
						stringBuilder[i] = ']';
						break;
					}
				}
				else if (c == '/' || c == '\\')
				{
					goto IL_0076;
				}
				IL_007F:
				i++;
				continue;
				IL_0076:
				stringBuilder[i] = '_';
				goto IL_007F;
			}
			return string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", new object[]
			{
				stringBuilder.ToString(),
				Process.GetCurrentProcess().Id
			});
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000A3791 File Offset: 0x000A2791
		internal static void IncrementConnectionsEstablished()
		{
			if (NetworkingPerfCounters.ConnectionsEstablished != null)
			{
				NetworkingPerfCounters.ConnectionsEstablished.Increment();
			}
			if (NetworkingPerfCounters.globalConnectionsEstablished != null)
			{
				NetworkingPerfCounters.globalConnectionsEstablished.Increment();
			}
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000A37B7 File Offset: 0x000A27B7
		internal static void AddBytesReceived(int increment)
		{
			if (NetworkingPerfCounters.BytesReceived != null)
			{
				NetworkingPerfCounters.BytesReceived.IncrementBy((long)increment);
			}
			if (NetworkingPerfCounters.globalBytesReceived != null)
			{
				NetworkingPerfCounters.globalBytesReceived.IncrementBy((long)increment);
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000A37E1 File Offset: 0x000A27E1
		internal static void AddBytesSent(int increment)
		{
			if (NetworkingPerfCounters.BytesSent != null)
			{
				NetworkingPerfCounters.BytesSent.IncrementBy((long)increment);
			}
			if (NetworkingPerfCounters.globalBytesSent != null)
			{
				NetworkingPerfCounters.globalBytesSent.IncrementBy((long)increment);
			}
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000A380B File Offset: 0x000A280B
		internal static void IncrementDatagramsReceived()
		{
			if (NetworkingPerfCounters.DatagramsReceived != null)
			{
				NetworkingPerfCounters.DatagramsReceived.Increment();
			}
			if (NetworkingPerfCounters.globalDatagramsReceived != null)
			{
				NetworkingPerfCounters.globalDatagramsReceived.Increment();
			}
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000A3831 File Offset: 0x000A2831
		internal static void IncrementDatagramsSent()
		{
			if (NetworkingPerfCounters.DatagramsSent != null)
			{
				NetworkingPerfCounters.DatagramsSent.Increment();
			}
			if (NetworkingPerfCounters.globalDatagramsSent != null)
			{
				NetworkingPerfCounters.globalDatagramsSent.Increment();
			}
		}

		// Token: 0x040026C3 RID: 9923
		private const string CategoryName = ".NET CLR Networking";

		// Token: 0x040026C4 RID: 9924
		private const string ConnectionsEstablishedName = "Connections Established";

		// Token: 0x040026C5 RID: 9925
		private const string BytesReceivedName = "Bytes Received";

		// Token: 0x040026C6 RID: 9926
		private const string BytesSentName = "Bytes Sent";

		// Token: 0x040026C7 RID: 9927
		private const string DatagramsReceivedName = "Datagrams Received";

		// Token: 0x040026C8 RID: 9928
		private const string DatagramsSentName = "Datagrams Sent";

		// Token: 0x040026C9 RID: 9929
		private const string GlobalInstanceName = "_Global_";

		// Token: 0x040026CA RID: 9930
		private static PerformanceCounter ConnectionsEstablished;

		// Token: 0x040026CB RID: 9931
		private static PerformanceCounter BytesReceived;

		// Token: 0x040026CC RID: 9932
		private static PerformanceCounter BytesSent;

		// Token: 0x040026CD RID: 9933
		private static PerformanceCounter DatagramsReceived;

		// Token: 0x040026CE RID: 9934
		private static PerformanceCounter DatagramsSent;

		// Token: 0x040026CF RID: 9935
		private static PerformanceCounter globalConnectionsEstablished;

		// Token: 0x040026D0 RID: 9936
		private static PerformanceCounter globalBytesReceived;

		// Token: 0x040026D1 RID: 9937
		private static PerformanceCounter globalBytesSent;

		// Token: 0x040026D2 RID: 9938
		private static PerformanceCounter globalDatagramsReceived;

		// Token: 0x040026D3 RID: 9939
		private static PerformanceCounter globalDatagramsSent;

		// Token: 0x040026D4 RID: 9940
		private static object syncObject = new object();

		// Token: 0x040026D5 RID: 9941
		private static bool initialized = false;
	}
}
