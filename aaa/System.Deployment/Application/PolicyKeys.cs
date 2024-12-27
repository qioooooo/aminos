using System;
using System.Globalization;
using System.IO;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x020000E9 RID: 233
	internal static class PolicyKeys
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0001E8EC File Offset: 0x0001D8EC
		public static bool RequireSignedManifests()
		{
			return PolicyKeys.CheckDeploymentBoolString("RequireSignedManifests", true, false);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001E8FF File Offset: 0x0001D8FF
		public static bool RequireHashInManifests()
		{
			return PolicyKeys.CheckDeploymentBoolString("RequireHashInManifests", true, false);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001E912 File Offset: 0x0001D912
		public static bool SkipDeploymentProvider()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SkipDeploymentProvider", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SkipDeploymentProvider"));
				return true;
			}
			return false;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001E934 File Offset: 0x0001D934
		public static bool SkipApplicationDependencyHashCheck()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SkipApplicationDependencyHashCheck", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SkipApplicationDependencyHashCheck"));
				return true;
			}
			return false;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001E956 File Offset: 0x0001D956
		public static bool SkipSignatureValidation()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SkipSignatureValidation", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SkipAllSigValidation"));
				return true;
			}
			return false;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001E978 File Offset: 0x0001D978
		public static bool SkipSchemaValidation()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SkipSchemaValidation", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SkipSchemaValidation"));
				return true;
			}
			return false;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001E99A File Offset: 0x0001D99A
		public static bool SkipSemanticValidation()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SkipSemanticValidation", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SkipAllSemanticValidation"));
				return true;
			}
			return false;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001E9BC File Offset: 0x0001D9BC
		public static bool SuppressLimitOnNumberOfActivations()
		{
			if (PolicyKeys.CheckDeploymentBoolString("SuppressLimitOnNumberOfActivations", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("SuppressLimitOnNumberOfActivations"));
				return true;
			}
			return false;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001E9DE File Offset: 0x0001D9DE
		public static bool DisableGenericExceptionHandler()
		{
			if (PolicyKeys.CheckDeploymentBoolString("DisableGenericExceptionHandler", true, false))
			{
				Logger.AddWarningInformation(Resources.GetString("DisableGenericExceptionHandler"));
				return true;
			}
			return false;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001EA00 File Offset: 0x0001DA00
		public static PolicyKeys.HostType ClrHostType()
		{
			int num = 0;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework\\DeploymentFramework", false))
			{
				if (registryKey != null)
				{
					object value = registryKey.GetValue("ClickOnceHost");
					num = ((value != null) ? ((int)value) : 0);
				}
			}
			PolicyKeys.HostType hostType = (PolicyKeys.HostType)num;
			if (hostType == PolicyKeys.HostType.AppLaunch)
			{
				Logger.AddWarningInformation(Resources.GetString("ForceAppLaunch"));
			}
			else if (hostType == PolicyKeys.HostType.Cor)
			{
				Logger.AddWarningInformation(Resources.GetString("ForceCor"));
			}
			return (PolicyKeys.HostType)num;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001EA84 File Offset: 0x0001DA84
		private static bool CheckDeploymentBoolString(string keyName, bool compare, bool defaultIfNotSet)
		{
			bool flag = false;
			bool flag2 = false;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework\\DeploymentFramework", false))
			{
				if (registryKey != null)
				{
					string text = registryKey.GetValue(keyName) as string;
					if (text != null)
					{
						flag2 = true;
						CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
						if (compareInfo.Compare(text, "true", CompareOptions.IgnoreCase) == 0)
						{
							flag = true;
						}
						else if (compareInfo.Compare(text, "false", CompareOptions.IgnoreCase) == 0)
						{
							flag = false;
						}
					}
				}
			}
			if (!flag2)
			{
				return defaultIfNotSet;
			}
			return flag == compare;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001EB14 File Offset: 0x0001DB14
		public static bool SkipSKUDetection()
		{
			bool flag = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Fusion", false))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("NoClientChecks");
						if (value != null && Convert.ToUInt32(value) > 0U)
						{
							Logger.AddWarningInformation(Resources.GetString("SkippedSKUDetection"));
							flag = true;
						}
					}
				}
			}
			catch (OverflowException)
			{
				flag = false;
			}
			catch (InvalidCastException)
			{
				flag = false;
			}
			catch (IOException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x020000EA RID: 234
		public enum HostType
		{
			// Token: 0x040004B9 RID: 1209
			Default,
			// Token: 0x040004BA RID: 1210
			AppLaunch,
			// Token: 0x040004BB RID: 1211
			Cor
		}
	}
}
