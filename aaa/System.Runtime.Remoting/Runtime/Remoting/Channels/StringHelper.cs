using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200001E RID: 30
	internal static class StringHelper
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0000553E File Offset: 0x0000453E
		internal static bool StartsWithDoubleUnderscore(string str)
		{
			return str.Length >= 2 && str[0] == '_' && str[1] == '_';
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005564 File Offset: 0x00004564
		internal static bool StartsWithAsciiIgnoreCasePrefixLower(string str, string asciiPrefix)
		{
			int length = asciiPrefix.Length;
			if (str.Length < length)
			{
				return false;
			}
			for (int i = 0; i < length; i++)
			{
				if (StringHelper.ToLowerAscii(str[i]) != asciiPrefix[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000055A7 File Offset: 0x000045A7
		private static char ToLowerAscii(char ch)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch + ' ';
			}
			return ch;
		}
	}
}
