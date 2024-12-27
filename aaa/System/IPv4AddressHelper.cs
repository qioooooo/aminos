using System;

namespace System
{
	// Token: 0x02000363 RID: 867
	internal class IPv4AddressHelper
	{
		// Token: 0x06001B9D RID: 7069 RVA: 0x00067DF8 File Offset: 0x00066DF8
		private IPv4AddressHelper()
		{
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00067E00 File Offset: 0x00066E00
		internal unsafe static string ParseCanonicalName(string str, int start, int end, ref bool isLoopback)
		{
			byte* ptr = stackalloc byte[1 * 4];
			isLoopback = IPv4AddressHelper.Parse(str, ptr, start, end);
			return string.Concat(new object[]
			{
				*ptr,
				".",
				ptr[1],
				".",
				ptr[2],
				".",
				ptr[3]
			});
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00067E74 File Offset: 0x00066E74
		internal unsafe static int ParseHostNumber(string str, int start, int end)
		{
			byte* ptr = stackalloc byte[1 * 4];
			IPv4AddressHelper.Parse(str, ptr, start, end);
			return ((int)(*ptr) << 24) + ((int)ptr[1] << 16) + ((int)ptr[2] << 8) + (int)ptr[3];
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00067EB0 File Offset: 0x00066EB0
		internal unsafe static bool IsValid(char* name, int start, ref int end, bool allowIPv6, bool notImplicitFile)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			while (start < end)
			{
				char c = name[start];
				if (allowIPv6)
				{
					if (c == ']' || c == '/')
					{
						break;
					}
					if (c == '%')
					{
						break;
					}
				}
				else if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					break;
				}
				if (c <= '9' && c >= '0')
				{
					flag = true;
					num2 = num2 * 10 + (int)(name[start] - '0');
					if (num2 > 255)
					{
						return false;
					}
				}
				else
				{
					if (c != '.')
					{
						return false;
					}
					if (!flag)
					{
						return false;
					}
					num++;
					flag = false;
					num2 = 0;
				}
				start++;
			}
			bool flag2 = num == 3 && flag;
			if (flag2)
			{
				end = start;
			}
			return flag2;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00067F58 File Offset: 0x00066F58
		private unsafe static bool Parse(string name, byte* numbers, int start, int end)
		{
			for (int i = 0; i < 4; i++)
			{
				byte b = 0;
				char c;
				while (start < end && (c = name[start]) != '.' && c != ':')
				{
					b = b * 10 + (byte)(c - '0');
					start++;
				}
				numbers[i] = b;
				start++;
			}
			return *numbers == 127;
		}

		// Token: 0x04001C2F RID: 7215
		private const int NumberOfLabels = 4;
	}
}
