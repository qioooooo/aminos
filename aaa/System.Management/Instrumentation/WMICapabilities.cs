using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B1 RID: 177
	internal sealed class WMICapabilities
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000261DC File Offset: 0x000251DC
		public static bool MultiIndicateSupported
		{
			get
			{
				if (-1 == WMICapabilities.multiIndicateSupported)
				{
					WMICapabilities.multiIndicateSupported = (WMICapabilities.MultiIndicatePossible() ? 1 : 0);
					if (WMICapabilities.wmiNetKey != null)
					{
						object value = WMICapabilities.wmiNetKey.GetValue("MultiIndicateSupported", WMICapabilities.multiIndicateSupported);
						if (value.GetType() == typeof(int) && (int)value == 1)
						{
							WMICapabilities.multiIndicateSupported = 1;
						}
					}
				}
				return WMICapabilities.multiIndicateSupported == 1;
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0002624C File Offset: 0x0002524C
		public static void AddAutorecoverMof(string path)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM\\CIMOM", true);
			if (registryKey != null)
			{
				object value = registryKey.GetValue("Autorecover MOFs");
				string[] array = value as string[];
				if (array == null)
				{
					if (value != null)
					{
						return;
					}
					array = new string[0];
				}
				registryKey.SetValue("Autorecover MOFs timestamp", DateTime.Now.ToFileTime().ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))));
				foreach (string text in array)
				{
					if (string.Compare(text, path, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return;
					}
				}
				string[] array3 = new string[array.Length + 1];
				array.CopyTo(array3, 0);
				array3[array3.Length - 1] = path;
				registryKey.SetValue("Autorecover MOFs", array3);
				registryKey.SetValue("Autorecover MOFs timestamp", DateTime.Now.ToFileTime().ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))));
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0002635B File Offset: 0x0002535B
		public static string InstallationDirectory
		{
			get
			{
				if (WMICapabilities.installationDirectory == null && WMICapabilities.wmiKey != null)
				{
					WMICapabilities.installationDirectory = WMICapabilities.wmiKey.GetValue("Installation Directory").ToString();
				}
				return WMICapabilities.installationDirectory;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00026389 File Offset: 0x00025389
		public static string FrameworkDirectory
		{
			get
			{
				return Path.Combine(WMICapabilities.InstallationDirectory, "Framework");
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0002639C File Offset: 0x0002539C
		public static bool IsUserAdmin()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				return true;
			}
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			return windowsPrincipal.Identity.IsAuthenticated && windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000263DD File Offset: 0x000253DD
		private static bool IsNovaFile(FileVersionInfo info)
		{
			return info.FileMajorPart == 1 && info.FileMinorPart == 50 && info.FileBuildPart == 1085;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00026404 File Offset: 0x00025404
		private static bool MultiIndicatePossible()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 1))
			{
				return true;
			}
			string text = Path.Combine(Environment.SystemDirectory, "wbem\\fastprox.dll");
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(text);
			return WMICapabilities.IsNovaFile(versionInfo) && versionInfo.FilePrivatePart >= 56;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00026464 File Offset: 0x00025464
		public static bool IsWindowsXPOrHigher()
		{
			OperatingSystem osversion = Environment.OSVersion;
			return osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 1);
		}

		// Token: 0x040002C2 RID: 706
		private const string WMIKeyPath = "Software\\Microsoft\\WBEM";

		// Token: 0x040002C3 RID: 707
		private const string WMINetKeyPath = "Software\\Microsoft\\WBEM\\.NET";

		// Token: 0x040002C4 RID: 708
		private const string WMICIMOMKeyPath = "Software\\Microsoft\\WBEM\\CIMOM";

		// Token: 0x040002C5 RID: 709
		private const string MultiIndicateSupportedValueNameVal = "MultiIndicateSupported";

		// Token: 0x040002C6 RID: 710
		private const string AutoRecoverMofsVal = "Autorecover MOFs";

		// Token: 0x040002C7 RID: 711
		private const string AutoRecoverMofsTimestampVal = "Autorecover MOFs timestamp";

		// Token: 0x040002C8 RID: 712
		private const string InstallationDirectoryVal = "Installation Directory";

		// Token: 0x040002C9 RID: 713
		private const string FrameworkSubDirectory = "Framework";

		// Token: 0x040002CA RID: 714
		private static RegistryKey wmiNetKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM\\.NET", false);

		// Token: 0x040002CB RID: 715
		private static RegistryKey wmiKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM", false);

		// Token: 0x040002CC RID: 716
		private static int multiIndicateSupported = -1;

		// Token: 0x040002CD RID: 717
		private static string installationDirectory = null;
	}
}
