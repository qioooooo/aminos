using System;

namespace System.Configuration
{
	// Token: 0x02000009 RID: 9
	internal static class ConfigPathUtility
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002300 File Offset: 0x00001300
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

		// Token: 0x06000011 RID: 17 RVA: 0x00002390 File Offset: 0x00001390
		internal static string GetParent(string configPath)
		{
			if (string.IsNullOrEmpty(configPath))
			{
				return null;
			}
			int num = configPath.LastIndexOf('/');
			string text;
			if (num == -1)
			{
				text = null;
			}
			else
			{
				text = configPath.Substring(0, num);
			}
			return text;
		}

		// Token: 0x04000C7B RID: 3195
		private const char SeparatorChar = '/';
	}
}
