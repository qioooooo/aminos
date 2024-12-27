using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x0200045C RID: 1116
	[ComVisible(true)]
	public static class Registry
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x00096DEC File Offset: 0x00095DEC
		private static RegistryKey GetBaseKeyFromKeyName(string keyName, out string subKeyName)
		{
			if (keyName == null)
			{
				throw new ArgumentNullException("keyName");
			}
			int num = keyName.IndexOf('\\');
			string text;
			if (num != -1)
			{
				text = keyName.Substring(0, num).ToUpper(CultureInfo.InvariantCulture);
			}
			else
			{
				text = keyName.ToUpper(CultureInfo.InvariantCulture);
			}
			string text2;
			if ((text2 = text) != null)
			{
				if (<PrivateImplementationDetails>{E09AAADC-8726-45E8-8303-680933A9C2AA}.$$method0x6002ca6-1 == null)
				{
					<PrivateImplementationDetails>{E09AAADC-8726-45E8-8303-680933A9C2AA}.$$method0x6002ca6-1 = new Dictionary<string, int>(7)
					{
						{ "HKEY_CURRENT_USER", 0 },
						{ "HKEY_LOCAL_MACHINE", 1 },
						{ "HKEY_CLASSES_ROOT", 2 },
						{ "HKEY_USERS", 3 },
						{ "HKEY_PERFORMANCE_DATA", 4 },
						{ "HKEY_CURRENT_CONFIG", 5 },
						{ "HKEY_DYN_DATA", 6 }
					};
				}
				int num2;
				if (<PrivateImplementationDetails>{E09AAADC-8726-45E8-8303-680933A9C2AA}.$$method0x6002ca6-1.TryGetValue(text2, out num2))
				{
					RegistryKey registryKey;
					switch (num2)
					{
					case 0:
						registryKey = Registry.CurrentUser;
						break;
					case 1:
						registryKey = Registry.LocalMachine;
						break;
					case 2:
						registryKey = Registry.ClassesRoot;
						break;
					case 3:
						registryKey = Registry.Users;
						break;
					case 4:
						registryKey = Registry.PerformanceData;
						break;
					case 5:
						registryKey = Registry.CurrentConfig;
						break;
					case 6:
						registryKey = Registry.DynData;
						break;
					default:
						goto IL_011E;
					}
					if (num == -1 || num == keyName.Length)
					{
						subKeyName = string.Empty;
					}
					else
					{
						subKeyName = keyName.Substring(num + 1, keyName.Length - num - 1);
					}
					return registryKey;
				}
			}
			IL_011E:
			throw new ArgumentException(Environment.GetResourceString("Arg_RegInvalidKeyName", new object[] { "keyName" }));
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x00096F68 File Offset: 0x00095F68
		public static object GetValue(string keyName, string valueName, object defaultValue)
		{
			string text;
			RegistryKey baseKeyFromKeyName = Registry.GetBaseKeyFromKeyName(keyName, out text);
			RegistryKey registryKey = baseKeyFromKeyName.OpenSubKey(text);
			if (registryKey == null)
			{
				return null;
			}
			object value;
			try
			{
				value = registryKey.GetValue(valueName, defaultValue);
			}
			finally
			{
				registryKey.Close();
			}
			return value;
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x00096FB0 File Offset: 0x00095FB0
		public static void SetValue(string keyName, string valueName, object value)
		{
			Registry.SetValue(keyName, valueName, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x00096FBC File Offset: 0x00095FBC
		public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
		{
			string text;
			RegistryKey baseKeyFromKeyName = Registry.GetBaseKeyFromKeyName(keyName, out text);
			RegistryKey registryKey = baseKeyFromKeyName.CreateSubKey(text);
			try
			{
				registryKey.SetValue(valueName, value, valueKind);
			}
			finally
			{
				registryKey.Close();
			}
		}

		// Token: 0x04001714 RID: 5908
		public static readonly RegistryKey CurrentUser = RegistryKey.GetBaseKey(RegistryKey.HKEY_CURRENT_USER);

		// Token: 0x04001715 RID: 5909
		public static readonly RegistryKey LocalMachine = RegistryKey.GetBaseKey(RegistryKey.HKEY_LOCAL_MACHINE);

		// Token: 0x04001716 RID: 5910
		public static readonly RegistryKey ClassesRoot = RegistryKey.GetBaseKey(RegistryKey.HKEY_CLASSES_ROOT);

		// Token: 0x04001717 RID: 5911
		public static readonly RegistryKey Users = RegistryKey.GetBaseKey(RegistryKey.HKEY_USERS);

		// Token: 0x04001718 RID: 5912
		public static readonly RegistryKey PerformanceData = RegistryKey.GetBaseKey(RegistryKey.HKEY_PERFORMANCE_DATA);

		// Token: 0x04001719 RID: 5913
		public static readonly RegistryKey CurrentConfig = RegistryKey.GetBaseKey(RegistryKey.HKEY_CURRENT_CONFIG);

		// Token: 0x0400171A RID: 5914
		public static readonly RegistryKey DynData = RegistryKey.GetBaseKey(RegistryKey.HKEY_DYN_DATA);
	}
}
