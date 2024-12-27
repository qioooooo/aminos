using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Management
{
	// Token: 0x02000040 RID: 64
	internal static class CompatSwitches
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000C32C File Offset: 0x0000B32C
		public static bool AllowIManagementObjectQI
		{
			get
			{
				if (CompatSwitches.s_allowManagementObjectQI == 0)
				{
					lock (CompatSwitches.s_syncLock)
					{
						if (CompatSwitches.s_allowManagementObjectQI == 0)
						{
							CompatSwitches.s_allowManagementObjectQI = (CompatSwitches.GetSwitchValueFromRegistry() ? 1 : (-1));
						}
					}
				}
				return CompatSwitches.s_allowManagementObjectQI == 1;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C388 File Offset: 0x0000B388
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool GetSwitchValueFromRegistry()
		{
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\v2.0.50727");
				if (registryKey == null)
				{
					return false;
				}
				return (int)registryKey.GetValue("WMIDisableCOMSecurity", -1) == 1;
			}
			catch (Exception ex)
			{
				if (ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is AccessViolationException)
				{
					throw;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return false;
		}

		// Token: 0x04000181 RID: 385
		private const string DotNetVersion = "v2.0.50727";

		// Token: 0x04000182 RID: 386
		private const string RegKeyLocation = "SOFTWARE\\Microsoft\\.NETFramework\\v2.0.50727";

		// Token: 0x04000183 RID: 387
		private const string c_WMIDisableCOMSecurity = "WMIDisableCOMSecurity";

		// Token: 0x04000184 RID: 388
		private static readonly object s_syncLock = new object();

		// Token: 0x04000185 RID: 389
		private static int s_allowManagementObjectQI;
	}
}
