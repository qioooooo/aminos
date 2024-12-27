using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000428 RID: 1064
	internal static class RegistryConfiguration
	{
		// Token: 0x0600214B RID: 8523 RVA: 0x000835DC File Offset: 0x000825DC
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int GlobalConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00083604 File Offset: 0x00082604
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string GlobalConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0008362C File Offset: 0x0008262C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int AppConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00083658 File Offset: 0x00082658
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string AppConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00083684 File Offset: 0x00082684
		private static object ReadConfig(string path, string valueName, RegistryValueKind kind)
		{
			object obj = null;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path))
				{
					if (registryKey == null)
					{
						return obj;
					}
					try
					{
						object value = registryKey.GetValue(valueName, null);
						if (value != null && registryKey.GetValueKind(valueName) == kind)
						{
							obj = value;
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (IOException)
					{
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			return obj;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00083720 File Offset: 0x00082720
		private static string GetNetFrameworkVersionedPath()
		{
			return string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\Microsoft\\.NETFramework\\v{0}", new object[] { Environment.Version.ToString(3) });
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00083754 File Offset: 0x00082754
		private static string GetAppConfigPath(string valueName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
			{
				RegistryConfiguration.GetNetFrameworkVersionedPath(),
				valueName
			});
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00083784 File Offset: 0x00082784
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		private static string GetAppConfigValueName()
		{
			string text = "Unknown";
			Process currentProcess = Process.GetCurrentProcess();
			try
			{
				ProcessModule mainModule = currentProcess.MainModule;
				text = mainModule.FileName;
			}
			catch (NotSupportedException)
			{
			}
			catch (Win32Exception)
			{
			}
			catch (InvalidOperationException)
			{
			}
			try
			{
				text = Path.GetFullPath(text);
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (PathTooLongException)
			{
			}
			return text;
		}

		// Token: 0x04002172 RID: 8562
		private const string netFrameworkPath = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04002173 RID: 8563
		private const string netFrameworkVersionedPath = "SOFTWARE\\Microsoft\\.NETFramework\\v{0}";

		// Token: 0x04002174 RID: 8564
		private const string netFrameworkFullPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";
	}
}
