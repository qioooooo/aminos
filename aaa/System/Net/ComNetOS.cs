using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x020003F0 RID: 1008
	internal static class ComNetOS
	{
		// Token: 0x0600208E RID: 8334 RVA: 0x00080734 File Offset: 0x0007F734
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		static ComNetOS()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32Windows)
			{
				ComNetOS.IsWin9x = true;
				return;
			}
			try
			{
				ComNetOS.IsAspNetServer = Thread.GetDomain().GetData(".appDomain") != null;
			}
			catch
			{
			}
			ComNetOS.IsWinNt = true;
			ComNetOS.IsWin2K = true;
			if (osversion.Version.Major == 5 && osversion.Version.Minor == 0)
			{
				ComNetOS.IsWinHttp51 = osversion.Version.MajorRevision >= 3;
				return;
			}
			ComNetOS.IsPostWin2K = true;
			if ((osversion.Version.Major == 5 && osversion.Version.Minor == 1 && osversion.Version.MajorRevision >= 2) || osversion.Version.Major >= 6)
			{
				ComNetOS.IsXpSp2 = true;
			}
			if (osversion.Version.Major == 5 && osversion.Version.Minor == 1)
			{
				ComNetOS.IsWinHttp51 = osversion.Version.MajorRevision >= 1;
				return;
			}
			ComNetOS.IsWinHttp51 = true;
			ComNetOS.IsWin2k3 = true;
			if ((osversion.Version.Major == 5 && osversion.Version.Minor == 2 && osversion.Version.MajorRevision >= 1) || osversion.Version.Major >= 6)
			{
				ComNetOS.IsWin2k3Sp1 = true;
			}
			if (osversion.Version.Major >= 6)
			{
				ComNetOS.IsVista = true;
			}
			if (osversion.Version.Major >= 7 || (osversion.Version.Major == 6 && osversion.Version.Minor >= 1))
			{
				ComNetOS.IsWin7 = true;
			}
			ComNetOS.InstallationType = ComNetOS.GetWindowsInstallType();
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000808D4 File Offset: 0x0007F8D4
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion")]
		private static WindowsInstallationType GetWindowsInstallType()
		{
			WindowsInstallationType windowsInstallationType;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
				{
					string text = registryKey.GetValue("InstallationType") as string;
					if (string.IsNullOrEmpty(text))
					{
						windowsInstallationType = WindowsInstallationType.Unknown;
					}
					else if (string.Compare(text, "Client", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Client;
					}
					else if (string.Compare(text, "Server", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Server;
					}
					else if (string.Compare(text, "Server Core", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.ServerCore;
					}
					else if (string.Compare(text, "Embedded", StringComparison.OrdinalIgnoreCase) == 0)
					{
						windowsInstallationType = WindowsInstallationType.Embedded;
					}
					else
					{
						windowsInstallationType = WindowsInstallationType.Unknown;
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
				windowsInstallationType = WindowsInstallationType.Unknown;
			}
			catch (SecurityException)
			{
				windowsInstallationType = WindowsInstallationType.Unknown;
			}
			return windowsInstallationType;
		}

		// Token: 0x04001FC0 RID: 8128
		private const string OSInstallTypeRegKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04001FC1 RID: 8129
		private const string OSInstallTypeRegKeyPath = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04001FC2 RID: 8130
		private const string OSInstallTypeRegName = "InstallationType";

		// Token: 0x04001FC3 RID: 8131
		private const string InstallTypeStringClient = "Client";

		// Token: 0x04001FC4 RID: 8132
		private const string InstallTypeStringServer = "Server";

		// Token: 0x04001FC5 RID: 8133
		private const string InstallTypeStringServerCore = "Server Core";

		// Token: 0x04001FC6 RID: 8134
		private const string InstallTypeStringEmbedded = "Embedded";

		// Token: 0x04001FC7 RID: 8135
		internal static readonly bool IsWin9x;

		// Token: 0x04001FC8 RID: 8136
		internal static readonly bool IsWinNt;

		// Token: 0x04001FC9 RID: 8137
		internal static readonly bool IsWin2K;

		// Token: 0x04001FCA RID: 8138
		internal static readonly bool IsPostWin2K;

		// Token: 0x04001FCB RID: 8139
		internal static readonly bool IsAspNetServer;

		// Token: 0x04001FCC RID: 8140
		internal static readonly bool IsWinHttp51;

		// Token: 0x04001FCD RID: 8141
		internal static readonly bool IsWin2k3;

		// Token: 0x04001FCE RID: 8142
		internal static readonly bool IsXpSp2;

		// Token: 0x04001FCF RID: 8143
		internal static readonly bool IsWin2k3Sp1;

		// Token: 0x04001FD0 RID: 8144
		internal static readonly bool IsVista;

		// Token: 0x04001FD1 RID: 8145
		internal static readonly bool IsWin7;

		// Token: 0x04001FD2 RID: 8146
		internal static readonly WindowsInstallationType InstallationType;
	}
}
