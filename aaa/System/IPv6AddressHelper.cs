using System;
using System.Diagnostics;
using System.Net;

namespace System
{
	// Token: 0x02000364 RID: 868
	internal class IPv6AddressHelper
	{
		// Token: 0x06001BA2 RID: 7074 RVA: 0x00067FAC File Offset: 0x00066FAC
		private IPv6AddressHelper()
		{
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00067FB4 File Offset: 0x00066FB4
		internal unsafe static string ParseCanonicalName(string str, int start, ref bool isLoopback, ref string scopeId)
		{
			ushort* ptr = stackalloc ushort[2 * 9];
			*(long*)ptr = 0L;
			*(long*)(ptr + 4) = 0L;
			isLoopback = IPv6AddressHelper.Parse(str, ptr, start, ref scopeId);
			return IPv6AddressHelper.CreateCanonicalName(ptr);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00067FE4 File Offset: 0x00066FE4
		private unsafe static string CreateCanonicalName(ushort* numbers)
		{
			return string.Concat(new object[]
			{
				'[',
				string.Format("{0:X4}", *numbers),
				':',
				string.Format("{0:X4}", numbers[1]),
				':',
				string.Format("{0:X4}", numbers[2]),
				':',
				string.Format("{0:X4}", numbers[3]),
				':',
				string.Format("{0:X4}", numbers[4]),
				':',
				string.Format("{0:X4}", numbers[5]),
				':',
				string.Format("{0:X4}", numbers[6]),
				':',
				string.Format("{0:X4}", numbers[7]),
				']'
			});
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0006811C File Offset: 0x0006711C
		internal unsafe static bool IsValid(char* name, int start, ref int end)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = true;
			int num3 = 1;
			if (name[start] == ':' && (start + 1 >= end || name[start + 1] != ':') && ServicePointManager.UseStrictIPv6AddressParsing)
			{
				return false;
			}
			int i;
			for (i = start; i < end; i++)
			{
				if (flag3 ? (name[i] >= '0' && name[i] <= '9') : Uri.IsHexDigit(name[i]))
				{
					num2++;
					flag4 = false;
				}
				else
				{
					if (num2 > 4)
					{
						return false;
					}
					if (num2 != 0)
					{
						num++;
						num3 = i - num2;
					}
					char c = name[i];
					if (c <= '/')
					{
						if (c == '%')
						{
							while (++i != end)
							{
								if (name[i] == ']')
								{
									goto IL_00F8;
								}
								if (name[i] == '/')
								{
									goto IL_0126;
								}
							}
							return false;
						}
						switch (c)
						{
						case '.':
							if (flag2)
							{
								return false;
							}
							i = end;
							if (!IPv4AddressHelper.IsValid(name, num3, ref i, true, false))
							{
								return false;
							}
							num++;
							flag2 = true;
							i--;
							goto IL_0160;
						case '/':
							break;
						default:
							return false;
						}
						IL_0126:
						if (num == 0 || flag3)
						{
							return false;
						}
						flag3 = true;
						flag4 = true;
						goto IL_0160;
					}
					else if (c != ':')
					{
						if (c != ']')
						{
							return false;
						}
					}
					else
					{
						if (i <= 0 || name[i - 1] != ':')
						{
							flag4 = true;
							goto IL_0160;
						}
						if (flag)
						{
							return false;
						}
						flag = true;
						flag4 = false;
						goto IL_0160;
					}
					IL_00F8:
					start = i;
					i = end;
					goto IL_0162;
					IL_0160:
					num2 = 0;
				}
				IL_0162:;
			}
			if (flag3 && (num2 < 1 || num2 > 2))
			{
				return false;
			}
			int num4 = 8 + (flag3 ? 1 : 0);
			if (flag4 || num2 > 4 || !(flag ? (num < num4) : (num == num4)))
			{
				return false;
			}
			if (i == end + 1)
			{
				end = start + 1;
				return true;
			}
			return false;
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000682E0 File Offset: 0x000672E0
		internal unsafe static bool Parse(string address, ushort* numbers, int start, ref string scopeId)
		{
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			bool flag = true;
			int num4 = 0;
			if (address[start] == '[')
			{
				start++;
			}
			int num5 = start;
			while (num5 < address.Length && address[num5] != ']')
			{
				char c = address[num5];
				if (c != '%')
				{
					if (c != '/')
					{
						if (c != ':')
						{
							num = num * 16 + Uri.FromHex(address[num5++]);
						}
						else
						{
							numbers[num2++] = (ushort)num;
							num = 0;
							num5++;
							if (address[num5] == ':')
							{
								num3 = num2;
								num5++;
							}
							else if (num3 < 0 && num2 < 6)
							{
								continue;
							}
							int num6 = num5;
							while (address[num6] != ']' && address[num6] != ':' && address[num6] != '%' && address[num6] != '/')
							{
								if (num6 >= num5 + 4)
								{
									break;
								}
								if (address[num6] == '.')
								{
									while (address[num6] != ']' && address[num6] != '/' && address[num6] != '%')
									{
										num6++;
									}
									num = IPv4AddressHelper.ParseHostNumber(address, num5, num6);
									numbers[num2++] = (ushort)(num >> 16);
									numbers[num2++] = (ushort)num;
									num5 = num6;
									num = 0;
									flag = false;
									break;
								}
								num6++;
							}
						}
					}
					else
					{
						if (flag)
						{
							numbers[num2++] = (ushort)num;
							flag = false;
						}
						num5++;
						while (address[num5] != ']')
						{
							num4 = num4 * 10 + (int)(address[num5] - '0');
							num5++;
						}
					}
				}
				else
				{
					if (flag)
					{
						numbers[num2++] = (ushort)num;
						flag = false;
					}
					start = num5;
					num5++;
					while (address[num5] != ']' && address[num5] != '/')
					{
						num5++;
					}
					scopeId = address.Substring(start, num5 - start);
					while (address[num5] != ']')
					{
						num5++;
					}
				}
			}
			if (flag)
			{
				numbers[num2++] = (ushort)num;
			}
			if (num3 > 0)
			{
				int num7 = 7;
				int num8 = num2 - 1;
				for (int i = num2 - num3; i > 0; i--)
				{
					numbers[num7--] = numbers[num8];
					numbers[num8--] = 0;
				}
			}
			return *numbers == 0 && numbers[1] == 0 && numbers[2] == 0 && numbers[3] == 0 && numbers[4] == 0 && ((numbers[5] == 0 && numbers[6] == 0 && numbers[7] == 1) || (numbers[6] == 32512 && numbers[7] == 1 && (numbers[5] == 0 || numbers[5] == ushort.MaxValue)));
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000685B4 File Offset: 0x000675B4
		[Conditional("DEBUG")]
		private static void ValidateIndex(int index)
		{
			bool useStrictIPv6AddressParsing = ServicePointManager.UseStrictIPv6AddressParsing;
		}

		// Token: 0x04001C30 RID: 7216
		private const int NumberOfLabels = 8;

		// Token: 0x04001C31 RID: 7217
		private const string CanonicalNumberFormat = "{0:X4}";
	}
}
