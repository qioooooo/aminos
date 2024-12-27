using System;
using System.Configuration;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020001C2 RID: 450
	internal static class DiagnosticsConfiguration
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0002CB48 File Offset: 0x0002BB48
		internal static SwitchElementsCollection SwitchSettings
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.Switches;
				}
				return null;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0002CB6C File Offset: 0x0002BB6C
		internal static bool AssertUIEnabled
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection == null || systemDiagnosticsSection.Assert == null || systemDiagnosticsSection.Assert.AssertUIEnabled;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0002CB9C File Offset: 0x0002BB9C
		internal static string ConfigFilePath
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.ElementInformation.Source;
				}
				return string.Empty;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0002CBC8 File Offset: 0x0002BBC8
		internal static string LogFileName
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Assert != null)
				{
					return systemDiagnosticsSection.Assert.LogFileName;
				}
				return string.Empty;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0002CBFC File Offset: 0x0002BBFC
		internal static bool AutoFlush
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection != null && systemDiagnosticsSection.Trace != null && systemDiagnosticsSection.Trace.AutoFlush;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0002CC2C File Offset: 0x0002BC2C
		internal static bool UseGlobalLock
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection == null || systemDiagnosticsSection.Trace == null || systemDiagnosticsSection.Trace.UseGlobalLock;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0002CC5C File Offset: 0x0002BC5C
		internal static int IndentSize
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Trace != null)
				{
					return systemDiagnosticsSection.Trace.IndentSize;
				}
				return 4;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002CC8C File Offset: 0x0002BC8C
		internal static int PerfomanceCountersFileMappingSize
		{
			get
			{
				int num = 0;
				while (!DiagnosticsConfiguration.CanInitialize() && num <= 5)
				{
					if (num == 5)
					{
						return 524288;
					}
					Thread.Sleep(200);
					num++;
				}
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.PerfCounters != null)
				{
					int num2 = systemDiagnosticsSection.PerfCounters.FileMappingSize;
					if (num2 < 32768)
					{
						num2 = 32768;
					}
					if (num2 > 33554432)
					{
						num2 = 33554432;
					}
					return num2;
				}
				return 524288;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x0002CD08 File Offset: 0x0002BD08
		internal static ListenerElementsCollection SharedListeners
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.SharedListeners;
				}
				return null;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0002CD2C File Offset: 0x0002BD2C
		internal static SourceElementsCollection Sources
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Sources != null)
				{
					return systemDiagnosticsSection.Sources;
				}
				return null;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0002CD57 File Offset: 0x0002BD57
		internal static SystemDiagnosticsSection SystemDiagnosticsSection
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				return DiagnosticsConfiguration.configSection;
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0002CD64 File Offset: 0x0002BD64
		private static SystemDiagnosticsSection GetConfigSection()
		{
			return (SystemDiagnosticsSection)PrivilegedConfigurationManager.GetSection("system.diagnostics");
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002CD82 File Offset: 0x0002BD82
		internal static bool IsInitializing()
		{
			return DiagnosticsConfiguration.initState == InitState.Initializing;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0002CD8C File Offset: 0x0002BD8C
		internal static bool IsInitialized()
		{
			return DiagnosticsConfiguration.initState == InitState.Initialized;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0002CD96 File Offset: 0x0002BD96
		internal static bool CanInitialize()
		{
			return DiagnosticsConfiguration.initState != InitState.Initializing && !ConfigurationManagerInternalFactory.Instance.SetConfigurationSystemInProgress;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0002CDB0 File Offset: 0x0002BDB0
		internal static void Initialize()
		{
			lock (TraceInternal.critSec)
			{
				if (DiagnosticsConfiguration.initState == InitState.NotInitialized && !ConfigurationManagerInternalFactory.Instance.SetConfigurationSystemInProgress)
				{
					DiagnosticsConfiguration.initState = InitState.Initializing;
					try
					{
						DiagnosticsConfiguration.configSection = DiagnosticsConfiguration.GetConfigSection();
					}
					finally
					{
						DiagnosticsConfiguration.initState = InitState.Initialized;
					}
				}
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0002CE1C File Offset: 0x0002BE1C
		internal static void Refresh()
		{
			ConfigurationManager.RefreshSection("system.diagnostics");
			SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
			if (systemDiagnosticsSection != null)
			{
				if (systemDiagnosticsSection.Switches != null)
				{
					foreach (object obj in systemDiagnosticsSection.Switches)
					{
						SwitchElement switchElement = (SwitchElement)obj;
						switchElement.ResetProperties();
					}
				}
				if (systemDiagnosticsSection.SharedListeners != null)
				{
					foreach (object obj2 in systemDiagnosticsSection.SharedListeners)
					{
						ListenerElement listenerElement = (ListenerElement)obj2;
						listenerElement.ResetProperties();
					}
				}
				if (systemDiagnosticsSection.Sources != null)
				{
					foreach (object obj3 in systemDiagnosticsSection.Sources)
					{
						SourceElement sourceElement = (SourceElement)obj3;
						sourceElement.ResetProperties();
					}
				}
			}
			DiagnosticsConfiguration.configSection = null;
			DiagnosticsConfiguration.initState = InitState.NotInitialized;
			DiagnosticsConfiguration.Initialize();
		}

		// Token: 0x04000EE1 RID: 3809
		private static SystemDiagnosticsSection configSection;

		// Token: 0x04000EE2 RID: 3810
		private static InitState initState;
	}
}
