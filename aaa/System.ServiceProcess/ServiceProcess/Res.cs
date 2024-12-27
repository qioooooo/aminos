using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.ServiceProcess
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
			this.resources = new ResourceManager("System.ServiceProcess", base.GetType().Assembly);
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
		internal const string RTL = "RTL";

		// Token: 0x04000032 RID: 50
		internal const string FileName = "FileName";

		// Token: 0x04000033 RID: 51
		internal const string ServiceStartedIncorrectly = "ServiceStartedIncorrectly";

		// Token: 0x04000034 RID: 52
		internal const string CallbackHandler = "CallbackHandler";

		// Token: 0x04000035 RID: 53
		internal const string OpenService = "OpenService";

		// Token: 0x04000036 RID: 54
		internal const string StartService = "StartService";

		// Token: 0x04000037 RID: 55
		internal const string StopService = "StopService";

		// Token: 0x04000038 RID: 56
		internal const string PauseService = "PauseService";

		// Token: 0x04000039 RID: 57
		internal const string ResumeService = "ResumeService";

		// Token: 0x0400003A RID: 58
		internal const string ControlService = "ControlService";

		// Token: 0x0400003B RID: 59
		internal const string ServiceName = "ServiceName";

		// Token: 0x0400003C RID: 60
		internal const string ServiceStartType = "ServiceStartType";

		// Token: 0x0400003D RID: 61
		internal const string ServiceDependency = "ServiceDependency";

		// Token: 0x0400003E RID: 62
		internal const string InstallService = "InstallService";

		// Token: 0x0400003F RID: 63
		internal const string InstallError = "InstallError";

		// Token: 0x04000040 RID: 64
		internal const string UserName = "UserName";

		// Token: 0x04000041 RID: 65
		internal const string UserPassword = "UserPassword";

		// Token: 0x04000042 RID: 66
		internal const string ButtonOK = "ButtonOK";

		// Token: 0x04000043 RID: 67
		internal const string ServiceUsage = "ServiceUsage";

		// Token: 0x04000044 RID: 68
		internal const string ServiceNameTooLongForNt4 = "ServiceNameTooLongForNt4";

		// Token: 0x04000045 RID: 69
		internal const string DisplayNameTooLong = "DisplayNameTooLong";

		// Token: 0x04000046 RID: 70
		internal const string NoService = "NoService";

		// Token: 0x04000047 RID: 71
		internal const string NoDisplayName = "NoDisplayName";

		// Token: 0x04000048 RID: 72
		internal const string OpenSC = "OpenSC";

		// Token: 0x04000049 RID: 73
		internal const string Timeout = "Timeout";

		// Token: 0x0400004A RID: 74
		internal const string CannotChangeProperties = "CannotChangeProperties";

		// Token: 0x0400004B RID: 75
		internal const string CannotChangeName = "CannotChangeName";

		// Token: 0x0400004C RID: 76
		internal const string NoServices = "NoServices";

		// Token: 0x0400004D RID: 77
		internal const string NoMachineName = "NoMachineName";

		// Token: 0x0400004E RID: 78
		internal const string BadMachineName = "BadMachineName";

		// Token: 0x0400004F RID: 79
		internal const string NoGivenName = "NoGivenName";

		// Token: 0x04000050 RID: 80
		internal const string CannotStart = "CannotStart";

		// Token: 0x04000051 RID: 81
		internal const string NotAService = "NotAService";

		// Token: 0x04000052 RID: 82
		internal const string NoInstaller = "NoInstaller";

		// Token: 0x04000053 RID: 83
		internal const string UserCanceledInstall = "UserCanceledInstall";

		// Token: 0x04000054 RID: 84
		internal const string UnattendedCannotPrompt = "UnattendedCannotPrompt";

		// Token: 0x04000055 RID: 85
		internal const string InvalidParameter = "InvalidParameter";

		// Token: 0x04000056 RID: 86
		internal const string FailedToUnloadAppDomain = "FailedToUnloadAppDomain";

		// Token: 0x04000057 RID: 87
		internal const string NotInPendingState = "NotInPendingState";

		// Token: 0x04000058 RID: 88
		internal const string ArgsCantBeNull = "ArgsCantBeNull";

		// Token: 0x04000059 RID: 89
		internal const string StartSuccessful = "StartSuccessful";

		// Token: 0x0400005A RID: 90
		internal const string StopSuccessful = "StopSuccessful";

		// Token: 0x0400005B RID: 91
		internal const string PauseSuccessful = "PauseSuccessful";

		// Token: 0x0400005C RID: 92
		internal const string ContinueSuccessful = "ContinueSuccessful";

		// Token: 0x0400005D RID: 93
		internal const string InstallSuccessful = "InstallSuccessful";

		// Token: 0x0400005E RID: 94
		internal const string UninstallSuccessful = "UninstallSuccessful";

		// Token: 0x0400005F RID: 95
		internal const string CommandSuccessful = "CommandSuccessful";

		// Token: 0x04000060 RID: 96
		internal const string StartFailed = "StartFailed";

		// Token: 0x04000061 RID: 97
		internal const string StopFailed = "StopFailed";

		// Token: 0x04000062 RID: 98
		internal const string PauseFailed = "PauseFailed";

		// Token: 0x04000063 RID: 99
		internal const string ContinueFailed = "ContinueFailed";

		// Token: 0x04000064 RID: 100
		internal const string SessionChangeFailed = "SessionChangeFailed";

		// Token: 0x04000065 RID: 101
		internal const string InstallFailed = "InstallFailed";

		// Token: 0x04000066 RID: 102
		internal const string UninstallFailed = "UninstallFailed";

		// Token: 0x04000067 RID: 103
		internal const string CommandFailed = "CommandFailed";

		// Token: 0x04000068 RID: 104
		internal const string ErrorNumber = "ErrorNumber";

		// Token: 0x04000069 RID: 105
		internal const string ShutdownOK = "ShutdownOK";

		// Token: 0x0400006A RID: 106
		internal const string ShutdownFailed = "ShutdownFailed";

		// Token: 0x0400006B RID: 107
		internal const string PowerEventOK = "PowerEventOK";

		// Token: 0x0400006C RID: 108
		internal const string PowerEventFailed = "PowerEventFailed";

		// Token: 0x0400006D RID: 109
		internal const string InstallOK = "InstallOK";

		// Token: 0x0400006E RID: 110
		internal const string TryToStop = "TryToStop";

		// Token: 0x0400006F RID: 111
		internal const string ServiceRemoving = "ServiceRemoving";

		// Token: 0x04000070 RID: 112
		internal const string ServiceRemoved = "ServiceRemoved";

		// Token: 0x04000071 RID: 113
		internal const string HelpText = "HelpText";

		// Token: 0x04000072 RID: 114
		internal const string CantStartFromCommandLine = "CantStartFromCommandLine";

		// Token: 0x04000073 RID: 115
		internal const string CantStartFromCommandLineTitle = "CantStartFromCommandLineTitle";

		// Token: 0x04000074 RID: 116
		internal const string CantRunOnWin9x = "CantRunOnWin9x";

		// Token: 0x04000075 RID: 117
		internal const string CantRunOnWin9xTitle = "CantRunOnWin9xTitle";

		// Token: 0x04000076 RID: 118
		internal const string CantControlOnWin9x = "CantControlOnWin9x";

		// Token: 0x04000077 RID: 119
		internal const string CantInstallOnWin9x = "CantInstallOnWin9x";

		// Token: 0x04000078 RID: 120
		internal const string InstallingService = "InstallingService";

		// Token: 0x04000079 RID: 121
		internal const string StartingService = "StartingService";

		// Token: 0x0400007A RID: 122
		internal const string SBAutoLog = "SBAutoLog";

		// Token: 0x0400007B RID: 123
		internal const string SBServiceName = "SBServiceName";

		// Token: 0x0400007C RID: 124
		internal const string SBServiceDescription = "SBServiceDescription";

		// Token: 0x0400007D RID: 125
		internal const string ServiceControllerDesc = "ServiceControllerDesc";

		// Token: 0x0400007E RID: 126
		internal const string SPCanPauseAndContinue = "SPCanPauseAndContinue";

		// Token: 0x0400007F RID: 127
		internal const string SPCanShutdown = "SPCanShutdown";

		// Token: 0x04000080 RID: 128
		internal const string SPCanStop = "SPCanStop";

		// Token: 0x04000081 RID: 129
		internal const string SPDisplayName = "SPDisplayName";

		// Token: 0x04000082 RID: 130
		internal const string SPDependentServices = "SPDependentServices";

		// Token: 0x04000083 RID: 131
		internal const string SPMachineName = "SPMachineName";

		// Token: 0x04000084 RID: 132
		internal const string SPServiceName = "SPServiceName";

		// Token: 0x04000085 RID: 133
		internal const string SPServicesDependedOn = "SPServicesDependedOn";

		// Token: 0x04000086 RID: 134
		internal const string SPStatus = "SPStatus";

		// Token: 0x04000087 RID: 135
		internal const string SPServiceType = "SPServiceType";

		// Token: 0x04000088 RID: 136
		internal const string ServiceProcessInstallerAccount = "ServiceProcessInstallerAccount";

		// Token: 0x04000089 RID: 137
		internal const string ServiceInstallerDescription = "ServiceInstallerDescription";

		// Token: 0x0400008A RID: 138
		internal const string ServiceInstallerServicesDependedOn = "ServiceInstallerServicesDependedOn";

		// Token: 0x0400008B RID: 139
		internal const string ServiceInstallerServiceName = "ServiceInstallerServiceName";

		// Token: 0x0400008C RID: 140
		internal const string ServiceInstallerStartType = "ServiceInstallerStartType";

		// Token: 0x0400008D RID: 141
		internal const string ServiceInstallerDisplayName = "ServiceInstallerDisplayName";

		// Token: 0x0400008E RID: 142
		internal const string Label_SetServiceLogin = "Label_SetServiceLogin";

		// Token: 0x0400008F RID: 143
		internal const string Label_MissmatchedPasswords = "Label_MissmatchedPasswords";

		// Token: 0x04000090 RID: 144
		private static Res loader;

		// Token: 0x04000091 RID: 145
		private ResourceManager resources;

		// Token: 0x04000092 RID: 146
		private static object s_InternalSyncObject;
	}
}
