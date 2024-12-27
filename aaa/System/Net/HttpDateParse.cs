using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020004E2 RID: 1250
	internal static class HttpDateParse
	{
		// Token: 0x060026E2 RID: 9954 RVA: 0x000A03A4 File Offset: 0x0009F3A4
		private static char MAKE_UPPER(char c)
		{
			return char.ToUpper(c, CultureInfo.InvariantCulture);
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000A03B4 File Offset: 0x0009F3B4
		private static int MapDayMonthToDword(char[] lpszDay, int index)
		{
			switch (HttpDateParse.MAKE_UPPER(lpszDay[index]))
			{
			case 'A':
			{
				char c = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c == 'P')
				{
					return 4;
				}
				if (c != 'U')
				{
					return -999;
				}
				return 8;
			}
			case 'D':
				return 12;
			case 'F':
			{
				char c2 = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c2 == 'E')
				{
					return 2;
				}
				if (c2 == 'R')
				{
					return 5;
				}
				return -999;
			}
			case 'G':
				return -1000;
			case 'J':
			{
				char c3 = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c3 != 'A')
				{
					if (c3 == 'U')
					{
						switch (HttpDateParse.MAKE_UPPER(lpszDay[index + 2]))
						{
						case 'L':
							return 7;
						case 'N':
							return 6;
						}
					}
					return -999;
				}
				return 1;
			}
			case 'M':
			{
				char c4 = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c4 != 'A')
				{
					if (c4 == 'O')
					{
						return 1;
					}
				}
				else
				{
					char c5 = HttpDateParse.MAKE_UPPER(lpszDay[index + 2]);
					if (c5 == 'R')
					{
						return 3;
					}
					if (c5 == 'Y')
					{
						return 5;
					}
				}
				return -999;
			}
			case 'N':
				return 11;
			case 'O':
				return 10;
			case 'S':
			{
				char c6 = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c6 == 'A')
				{
					return 6;
				}
				if (c6 == 'E')
				{
					return 9;
				}
				if (c6 != 'U')
				{
					return -999;
				}
				return 0;
			}
			case 'T':
			{
				char c7 = HttpDateParse.MAKE_UPPER(lpszDay[index + 1]);
				if (c7 == 'H')
				{
					return 4;
				}
				if (c7 == 'U')
				{
					return 2;
				}
				return -999;
			}
			case 'U':
				return -1000;
			case 'W':
				return 3;
			}
			return -999;
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000A0564 File Offset: 0x0009F564
		public static bool ParseHttpDate(string DateString, out DateTime dtOut)
		{
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			bool flag = false;
			int[] array = new int[8];
			bool flag2 = true;
			char[] array2 = DateString.ToCharArray();
			dtOut = default(DateTime);
			while (num < DateString.Length && num2 < 8)
			{
				if (array2[num] >= '0' && array2[num] <= '9')
				{
					array[num2] = 0;
					do
					{
						array[num2] *= 10;
						array[num2] += (int)(array2[num] - '0');
						num++;
					}
					while (num < DateString.Length && array2[num] >= '0' && array2[num] <= '9');
					num2++;
				}
				else if ((array2[num] >= 'A' && array2[num] <= 'Z') || (array2[num] >= 'a' && array2[num] <= 'z'))
				{
					array[num2] = HttpDateParse.MapDayMonthToDword(array2, num);
					num3 = num2;
					if (array[num2] == -999 && (!flag || num2 != 6))
					{
						flag2 = false;
						return flag2;
					}
					if (num2 == 1)
					{
						flag = true;
					}
					do
					{
						num++;
					}
					while (num < DateString.Length && ((array2[num] >= 'A' && array2[num] <= 'Z') || (array2[num] >= 'a' && array2[num] <= 'z')));
					num2++;
				}
				else
				{
					num++;
				}
			}
			int num4 = 0;
			int num5;
			int num6;
			int num7;
			int num8;
			int num9;
			int num10;
			if (flag)
			{
				num5 = array[2];
				num6 = array[1];
				num7 = array[3];
				num8 = array[4];
				num9 = array[5];
				if (num3 != 6)
				{
					num10 = array[6];
				}
				else
				{
					num10 = array[7];
				}
			}
			else
			{
				num5 = array[1];
				num6 = array[2];
				num10 = array[3];
				num7 = array[4];
				num8 = array[5];
				num9 = array[6];
			}
			if (num10 < 100)
			{
				num10 += ((num10 < 80) ? 2000 : 1900);
			}
			if (num2 < 4 || num5 > 31 || num7 > 23 || num8 > 59 || num9 > 59)
			{
				return false;
			}
			dtOut = new DateTime(num10, num6, num5, num7, num8, num9, num4);
			if (num3 == 6)
			{
				dtOut = dtOut.ToUniversalTime();
			}
			if (num2 > 7 && array[7] != -1000)
			{
				double num11 = (double)array[7];
				dtOut.AddHours(num11);
			}
			dtOut = dtOut.ToLocalTime();
			return flag2;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000A078C File Offset: 0x0009F78C
		public static bool ParseCookieDate(string dateString, out DateTime dtOut)
		{
			dtOut = DateTime.MinValue;
			char[] array = dateString.ToCharArray();
			if (array.Length < 18)
			{
				return false;
			}
			int num = 0;
			char c;
			if (!char.IsDigit(c = array[num++]))
			{
				return false;
			}
			int num2 = (int)(c - '0');
			if (!char.IsDigit(c = array[num++]))
			{
				num--;
			}
			else
			{
				num2 = num2 * 10 + (int)(c - '0');
			}
			if (num2 > 31)
			{
				return false;
			}
			num++;
			int num3 = HttpDateParse.MapDayMonthToDword(array, num);
			if (num3 == -999)
			{
				return false;
			}
			num += 4;
			int num4 = 0;
			int i = 0;
			while (i < 4)
			{
				if (!char.IsDigit(c = array[i + num]))
				{
					if (i != 2)
					{
						return false;
					}
					break;
				}
				else
				{
					num4 = num4 * 10 + (int)(c - '0');
					i++;
				}
			}
			if (i == 2)
			{
				num4 += ((num4 < 80) ? 2000 : 1900);
			}
			i += num;
			if (array[i++] != ' ')
			{
				return false;
			}
			if (!char.IsDigit(c = array[i++]))
			{
				return false;
			}
			int num5 = (int)(c - '0');
			if (!char.IsDigit(c = array[i++]))
			{
				i--;
			}
			else
			{
				num5 = num5 * 10 + (int)(c - '0');
			}
			if (num5 > 24 || array[i++] != ':')
			{
				return false;
			}
			if (!char.IsDigit(c = array[i++]))
			{
				return false;
			}
			int num6 = (int)(c - '0');
			if (!char.IsDigit(c = array[i++]))
			{
				i--;
			}
			else
			{
				num6 = num6 * 10 + (int)(c - '0');
			}
			if (num6 > 60 || array[i++] != ':')
			{
				return false;
			}
			if (array.Length - i < 5)
			{
				return false;
			}
			if (!char.IsDigit(c = array[i++]))
			{
				return false;
			}
			int num7 = (int)(c - '0');
			if (!char.IsDigit(c = array[i++]))
			{
				i--;
			}
			else
			{
				num7 = num7 * 10 + (int)(c - '0');
			}
			if (num7 > 60 || array[i++] != ' ')
			{
				return false;
			}
			if (array.Length - i < 3 || array[i++] != 'G' || array[i++] != 'M' || array[i++] != 'T')
			{
				return false;
			}
			dtOut = new DateTime(num4, num3, num2, num5, num6, num7, 0).ToLocalTime();
			return true;
		}

		// Token: 0x04002662 RID: 9826
		private const int BASE_DEC = 10;

		// Token: 0x04002663 RID: 9827
		private const int DATE_INDEX_DAY_OF_WEEK = 0;

		// Token: 0x04002664 RID: 9828
		private const int DATE_1123_INDEX_DAY = 1;

		// Token: 0x04002665 RID: 9829
		private const int DATE_1123_INDEX_MONTH = 2;

		// Token: 0x04002666 RID: 9830
		private const int DATE_1123_INDEX_YEAR = 3;

		// Token: 0x04002667 RID: 9831
		private const int DATE_1123_INDEX_HRS = 4;

		// Token: 0x04002668 RID: 9832
		private const int DATE_1123_INDEX_MINS = 5;

		// Token: 0x04002669 RID: 9833
		private const int DATE_1123_INDEX_SECS = 6;

		// Token: 0x0400266A RID: 9834
		private const int DATE_ANSI_INDEX_MONTH = 1;

		// Token: 0x0400266B RID: 9835
		private const int DATE_ANSI_INDEX_DAY = 2;

		// Token: 0x0400266C RID: 9836
		private const int DATE_ANSI_INDEX_HRS = 3;

		// Token: 0x0400266D RID: 9837
		private const int DATE_ANSI_INDEX_MINS = 4;

		// Token: 0x0400266E RID: 9838
		private const int DATE_ANSI_INDEX_SECS = 5;

		// Token: 0x0400266F RID: 9839
		private const int DATE_ANSI_INDEX_YEAR = 6;

		// Token: 0x04002670 RID: 9840
		private const int DATE_INDEX_TZ = 7;

		// Token: 0x04002671 RID: 9841
		private const int DATE_INDEX_LAST = 7;

		// Token: 0x04002672 RID: 9842
		private const int MAX_FIELD_DATE_ENTRIES = 8;

		// Token: 0x04002673 RID: 9843
		private const int DATE_TOKEN_JANUARY = 1;

		// Token: 0x04002674 RID: 9844
		private const int DATE_TOKEN_FEBRUARY = 2;

		// Token: 0x04002675 RID: 9845
		private const int DATE_TOKEN_MARCH = 3;

		// Token: 0x04002676 RID: 9846
		private const int DATE_TOKEN_APRIL = 4;

		// Token: 0x04002677 RID: 9847
		private const int DATE_TOKEN_MAY = 5;

		// Token: 0x04002678 RID: 9848
		private const int DATE_TOKEN_JUNE = 6;

		// Token: 0x04002679 RID: 9849
		private const int DATE_TOKEN_JULY = 7;

		// Token: 0x0400267A RID: 9850
		private const int DATE_TOKEN_AUGUST = 8;

		// Token: 0x0400267B RID: 9851
		private const int DATE_TOKEN_SEPTEMBER = 9;

		// Token: 0x0400267C RID: 9852
		private const int DATE_TOKEN_OCTOBER = 10;

		// Token: 0x0400267D RID: 9853
		private const int DATE_TOKEN_NOVEMBER = 11;

		// Token: 0x0400267E RID: 9854
		private const int DATE_TOKEN_DECEMBER = 12;

		// Token: 0x0400267F RID: 9855
		private const int DATE_TOKEN_LAST_MONTH = 13;

		// Token: 0x04002680 RID: 9856
		private const int DATE_TOKEN_SUNDAY = 0;

		// Token: 0x04002681 RID: 9857
		private const int DATE_TOKEN_MONDAY = 1;

		// Token: 0x04002682 RID: 9858
		private const int DATE_TOKEN_TUESDAY = 2;

		// Token: 0x04002683 RID: 9859
		private const int DATE_TOKEN_WEDNESDAY = 3;

		// Token: 0x04002684 RID: 9860
		private const int DATE_TOKEN_THURSDAY = 4;

		// Token: 0x04002685 RID: 9861
		private const int DATE_TOKEN_FRIDAY = 5;

		// Token: 0x04002686 RID: 9862
		private const int DATE_TOKEN_SATURDAY = 6;

		// Token: 0x04002687 RID: 9863
		private const int DATE_TOKEN_LAST_DAY = 7;

		// Token: 0x04002688 RID: 9864
		private const int DATE_TOKEN_GMT = -1000;

		// Token: 0x04002689 RID: 9865
		private const int DATE_TOKEN_LAST = -1000;

		// Token: 0x0400268A RID: 9866
		private const int DATE_TOKEN_ERROR = -999;
	}
}
