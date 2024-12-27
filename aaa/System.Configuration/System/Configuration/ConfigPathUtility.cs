using System;

namespace System.Configuration
{
	// Token: 0x0200000A RID: 10
	internal static class ConfigPathUtility
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000022A0 File Offset: 0x000012A0
		internal static bool IsValid(string configPath)
		{
			if (string.IsNullOrEmpty(configPath))
			{
				return false;
			}
			int num = -1;
			for (int i = 0; i <= configPath.Length; i++)
			{
				char c;
				if (i < configPath.Length)
				{
					c = configPath[i];
				}
				else
				{
					c = '/';
				}
				if (c == '\\')
				{
					return false;
				}
				if (c == '/')
				{
					if (i == num + 1)
					{
						return false;
					}
					if (i == num + 2 && configPath[num + 1] == '.')
					{
						return false;
					}
					if (i == num + 3 && configPath[num + 1] == '.' && configPath[num + 2] == '.')
					{
						return false;
					}
					num = i;
				}
			}
			return true;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000232E File Offset: 0x0000132E
		internal static string Combine(string parentConfigPath, string childConfigPath)
		{
			if (string.IsNullOrEmpty(parentConfigPath))
			{
				return childConfigPath;
			}
			if (string.IsNullOrEmpty(childConfigPath))
			{
				return parentConfigPath;
			}
			return parentConfigPath + "/" + childConfigPath;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002350 File Offset: 0x00001350
		internal static string[] GetParts(string configPath)
		{
			return configPath.Split(new char[] { '/' });
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002374 File Offset: 0x00001374
		internal static string GetName(string configPath)
		{
			if (string.IsNullOrEmpty(configPath))
			{
				return configPath;
			}
			int num = configPath.LastIndexOf('/');
			if (num == -1)
			{
				return configPath;
			}
			return configPath.Substring(num + 1);
		}

		// Token: 0x04000140 RID: 320
		private const char SeparatorChar = '/';
	}
}
