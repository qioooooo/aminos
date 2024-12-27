using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Configuration.Install
{
	// Token: 0x02000007 RID: 7
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (Res.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Res.s_InternalSyncObject, obj, null);
				}
				return Res.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal Res()
		{
			this.resources = new ResourceManager("System.Configuration.Install", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (Res.InternalSyncObject)
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			string @string = res.resources.GetString(name, Res.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000031 RID: 49
		internal const string InstallAbort = "InstallAbort";

		// Token: 0x04000032 RID: 50
		internal const string InstallException = "InstallException";

		// Token: 0x04000033 RID: 51
		internal const string InstallLogContent = "InstallLogContent";

		// Token: 0x04000034 RID: 52
		internal const string InstallFileLocation = "InstallFileLocation";

		// Token: 0x04000035 RID: 53
		internal const string InstallLogParameters = "InstallLogParameters";

		// Token: 0x04000036 RID: 54
		internal const string InstallLogNone = "InstallLogNone";

		// Token: 0x04000037 RID: 55
		internal const string InstallNoPublicInstallers = "InstallNoPublicInstallers";

		// Token: 0x04000038 RID: 56
		internal const string InstallFileNotFound = "InstallFileNotFound";

		// Token: 0x04000039 RID: 57
		internal const string InstallNoInstallerTypes = "InstallNoInstallerTypes";

		// Token: 0x0400003A RID: 58
		internal const string InstallCannotCreateInstance = "InstallCannotCreateInstance";

		// Token: 0x0400003B RID: 59
		internal const string InstallBadParent = "InstallBadParent";

		// Token: 0x0400003C RID: 60
		internal const string InstallRecursiveParent = "InstallRecursiveParent";

		// Token: 0x0400003D RID: 61
		internal const string InstallNullParameter = "InstallNullParameter";

		// Token: 0x0400003E RID: 62
		internal const string InstallDictionaryMissingValues = "InstallDictionaryMissingValues";

		// Token: 0x0400003F RID: 63
		internal const string InstallDictionaryCorrupted = "InstallDictionaryCorrupted";

		// Token: 0x04000040 RID: 64
		internal const string InstallCommitException = "InstallCommitException";

		// Token: 0x04000041 RID: 65
		internal const string InstallRollbackException = "InstallRollbackException";

		// Token: 0x04000042 RID: 66
		internal const string InstallUninstallException = "InstallUninstallException";

		// Token: 0x04000043 RID: 67
		internal const string InstallEventException = "InstallEventException";

		// Token: 0x04000044 RID: 68
		internal const string InstallInstallerNotFound = "InstallInstallerNotFound";

		// Token: 0x04000045 RID: 69
		internal const string InstallSeverityError = "InstallSeverityError";

		// Token: 0x04000046 RID: 70
		internal const string InstallSeverityWarning = "InstallSeverityWarning";

		// Token: 0x04000047 RID: 71
		internal const string InstallLogInner = "InstallLogInner";

		// Token: 0x04000048 RID: 72
		internal const string InstallLogError = "InstallLogError";

		// Token: 0x04000049 RID: 73
		internal const string InstallLogCommitException = "InstallLogCommitException";

		// Token: 0x0400004A RID: 74
		internal const string InstallLogRollbackException = "InstallLogRollbackException";

		// Token: 0x0400004B RID: 75
		internal const string InstallLogUninstallException = "InstallLogUninstallException";

		// Token: 0x0400004C RID: 76
		internal const string InstallRollback = "InstallRollback";

		// Token: 0x0400004D RID: 77
		internal const string InstallAssemblyHelp = "InstallAssemblyHelp";

		// Token: 0x0400004E RID: 78
		internal const string InstallActivityRollingBack = "InstallActivityRollingBack";

		// Token: 0x0400004F RID: 79
		internal const string InstallActivityUninstalling = "InstallActivityUninstalling";

		// Token: 0x04000050 RID: 80
		internal const string InstallActivityCommitting = "InstallActivityCommitting";

		// Token: 0x04000051 RID: 81
		internal const string InstallActivityInstalling = "InstallActivityInstalling";

		// Token: 0x04000052 RID: 82
		internal const string InstallInfoTransacted = "InstallInfoTransacted";

		// Token: 0x04000053 RID: 83
		internal const string InstallInfoBeginInstall = "InstallInfoBeginInstall";

		// Token: 0x04000054 RID: 84
		internal const string InstallInfoException = "InstallInfoException";

		// Token: 0x04000055 RID: 85
		internal const string InstallInfoBeginRollback = "InstallInfoBeginRollback";

		// Token: 0x04000056 RID: 86
		internal const string InstallInfoRollbackDone = "InstallInfoRollbackDone";

		// Token: 0x04000057 RID: 87
		internal const string InstallInfoBeginCommit = "InstallInfoBeginCommit";

		// Token: 0x04000058 RID: 88
		internal const string InstallInfoCommitDone = "InstallInfoCommitDone";

		// Token: 0x04000059 RID: 89
		internal const string InstallInfoTransactedDone = "InstallInfoTransactedDone";

		// Token: 0x0400005A RID: 90
		internal const string InstallInfoBeginUninstall = "InstallInfoBeginUninstall";

		// Token: 0x0400005B RID: 91
		internal const string InstallInfoUninstallDone = "InstallInfoUninstallDone";

		// Token: 0x0400005C RID: 92
		internal const string InstallSavedStateFileCorruptedWarning = "InstallSavedStateFileCorruptedWarning";

		// Token: 0x0400005D RID: 93
		internal const string IncompleteEventLog = "IncompleteEventLog";

		// Token: 0x0400005E RID: 94
		internal const string IncompletePerformanceCounter = "IncompletePerformanceCounter";

		// Token: 0x0400005F RID: 95
		internal const string PerfInvalidCategoryName = "PerfInvalidCategoryName";

		// Token: 0x04000060 RID: 96
		internal const string NotCustomPerformanceCategory = "NotCustomPerformanceCategory";

		// Token: 0x04000061 RID: 97
		internal const string RemovingInstallState = "RemovingInstallState";

		// Token: 0x04000062 RID: 98
		internal const string InstallUnableDeleteFile = "InstallUnableDeleteFile";

		// Token: 0x04000063 RID: 99
		internal const string InstallInitializeException = "InstallInitializeException";

		// Token: 0x04000064 RID: 100
		internal const string InstallFileDoesntExist = "InstallFileDoesntExist";

		// Token: 0x04000065 RID: 101
		internal const string InstallFileDoesntExistCommandLine = "InstallFileDoesntExistCommandLine";

		// Token: 0x04000066 RID: 102
		internal const string WinNTRequired = "WinNTRequired";

		// Token: 0x04000067 RID: 103
		internal const string WrappedExceptionSource = "WrappedExceptionSource";

		// Token: 0x04000068 RID: 104
		internal const string InvalidProperty = "InvalidProperty";

		// Token: 0x04000069 RID: 105
		internal const string InstallRollbackNtRun = "InstallRollbackNtRun";

		// Token: 0x0400006A RID: 106
		internal const string InstallCommitNtRun = "InstallCommitNtRun";

		// Token: 0x0400006B RID: 107
		internal const string InstallUninstallNtRun = "InstallUninstallNtRun";

		// Token: 0x0400006C RID: 108
		internal const string InstallInstallNtRun = "InstallInstallNtRun";

		// Token: 0x0400006D RID: 109
		internal const string InstallHelpMessageStart = "InstallHelpMessageStart";

		// Token: 0x0400006E RID: 110
		internal const string InstallHelpMessageEnd = "InstallHelpMessageEnd";

		// Token: 0x0400006F RID: 111
		internal const string CantAddSelf = "CantAddSelf";

		// Token: 0x04000070 RID: 112
		internal const string Desc_Installer_HelpText = "Desc_Installer_HelpText";

		// Token: 0x04000071 RID: 113
		internal const string Desc_Installer_Parent = "Desc_Installer_Parent";

		// Token: 0x04000072 RID: 114
		internal const string Desc_AssemblyInstaller_Assembly = "Desc_AssemblyInstaller_Assembly";

		// Token: 0x04000073 RID: 115
		internal const string Desc_AssemblyInstaller_CommandLine = "Desc_AssemblyInstaller_CommandLine";

		// Token: 0x04000074 RID: 116
		internal const string Desc_AssemblyInstaller_Path = "Desc_AssemblyInstaller_Path";

		// Token: 0x04000075 RID: 117
		internal const string Desc_AssemblyInstaller_UseNewContext = "Desc_AssemblyInstaller_UseNewContext";

		// Token: 0x04000076 RID: 118
		internal const string NotAnEventLog = "NotAnEventLog";

		// Token: 0x04000077 RID: 119
		internal const string CreatingEventLog = "CreatingEventLog";

		// Token: 0x04000078 RID: 120
		internal const string RestoringEventLog = "RestoringEventLog";

		// Token: 0x04000079 RID: 121
		internal const string RemovingEventLog = "RemovingEventLog";

		// Token: 0x0400007A RID: 122
		internal const string DeletingEventLog = "DeletingEventLog";

		// Token: 0x0400007B RID: 123
		internal const string LocalSourceNotRegisteredWarning = "LocalSourceNotRegisteredWarning";

		// Token: 0x0400007C RID: 124
		internal const string Desc_CategoryResourceFile = "Desc_CategoryResourceFile";

		// Token: 0x0400007D RID: 125
		internal const string Desc_CategoryCount = "Desc_CategoryCount";

		// Token: 0x0400007E RID: 126
		internal const string Desc_Log = "Desc_Log";

		// Token: 0x0400007F RID: 127
		internal const string Desc_MessageResourceFile = "Desc_MessageResourceFile";

		// Token: 0x04000080 RID: 128
		internal const string Desc_ParameterResourceFile = "Desc_ParameterResourceFile";

		// Token: 0x04000081 RID: 129
		internal const string Desc_Source = "Desc_Source";

		// Token: 0x04000082 RID: 130
		internal const string Desc_UninstallAction = "Desc_UninstallAction";

		// Token: 0x04000083 RID: 131
		internal const string NotAPerformanceCounter = "NotAPerformanceCounter";

		// Token: 0x04000084 RID: 132
		internal const string NewCategory = "NewCategory";

		// Token: 0x04000085 RID: 133
		internal const string RestoringPerformanceCounter = "RestoringPerformanceCounter";

		// Token: 0x04000086 RID: 134
		internal const string CreatingPerformanceCounter = "CreatingPerformanceCounter";

		// Token: 0x04000087 RID: 135
		internal const string RemovingPerformanceCounter = "RemovingPerformanceCounter";

		// Token: 0x04000088 RID: 136
		internal const string PCCategoryName = "PCCategoryName";

		// Token: 0x04000089 RID: 137
		internal const string PCCounterName = "PCCounterName";

		// Token: 0x0400008A RID: 138
		internal const string PCInstanceName = "PCInstanceName";

		// Token: 0x0400008B RID: 139
		internal const string PCMachineName = "PCMachineName";

		// Token: 0x0400008C RID: 140
		internal const string PCI_CategoryHelp = "PCI_CategoryHelp";

		// Token: 0x0400008D RID: 141
		internal const string PCI_Counters = "PCI_Counters";

		// Token: 0x0400008E RID: 142
		internal const string PCI_IsMultiInstance = "PCI_IsMultiInstance";

		// Token: 0x0400008F RID: 143
		internal const string PCI_UninstallAction = "PCI_UninstallAction";

		// Token: 0x04000090 RID: 144
		private static Res loader;

		// Token: 0x04000091 RID: 145
		private ResourceManager resources;

		// Token: 0x04000092 RID: 146
		private static object s_InternalSyncObject;
	}
}
