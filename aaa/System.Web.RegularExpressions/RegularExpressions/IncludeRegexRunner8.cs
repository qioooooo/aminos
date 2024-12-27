using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000017 RID: 23
	internal class IncludeRegexRunner8 : RegexRunner
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00006850 File Offset: 0x00005850
		public override void Go()
		{
			string runtext = this.runtext;
			int runtextstart = this.runtextstart;
			int runtextbeg = this.runtextbeg;
			int runtextend = this.runtextend;
			int num = this.runtextpos;
			int[] array = this.runtrack;
			int num2 = this.runtrackpos;
			int[] array2 = this.runstack;
			int num3 = this.runstackpos;
			array[--num2] = num;
			array[--num2] = 0;
			array2[--num3] = num;
			array[--num2] = 1;
			int num5;
			int num4;
			if (num == this.runtextstart && 4 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '!' && runtext[num + 2] == '-' && runtext[num + 3] == '-')
			{
				num += 4;
				num4 = (num5 = runtextend - num) + 1;
				while (--num4 > 0)
				{
					if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						num--;
						break;
					}
				}
				if (num5 > num4)
				{
					array[--num2] = num5 - num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 2;
					goto IL_0155;
				}
				goto IL_0155;
			}
			for (;;)
			{
				IL_0730:
				this.runtrackpos = num2;
				this.runstackpos = num3;
				this.EnsureStorage();
				num2 = this.runtrackpos;
				num3 = this.runstackpos;
				array = this.runtrack;
				array2 = this.runstack;
				switch (array[num2++])
				{
				default:
					goto IL_07A5;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_07C2;
				case 3:
					goto IL_0812;
				case 4:
					goto IL_0862;
				case 5:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 6:
					goto IL_08D1;
				case 7:
					goto IL_0921;
				case 8:
					goto IL_0971;
				case 9:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\u0001\u0004\0\"#'("))
					{
						goto Block_59;
					}
					break;
				case 10:
					goto IL_0A2C;
				case 11:
					goto IL_0A7C;
				}
			}
			IL_07A5:
			num = array[num2++];
			goto IL_0727;
			IL_07C2:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_0155;
			}
			goto IL_0155;
			IL_0812:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
				goto IL_02C3;
			}
			goto IL_02C3;
			IL_0862:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_0390;
			}
			goto IL_0390;
			IL_08D1:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 6;
				goto IL_0434;
			}
			goto IL_0434;
			IL_0921:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 7;
				goto IL_04C7;
			}
			goto IL_04C7;
			IL_0971:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 8;
				goto IL_0543;
			}
			goto IL_0543;
			Block_59:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 9;
				goto IL_058E;
			}
			goto IL_058E;
			IL_0A2C:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 10;
				goto IL_063A;
			}
			goto IL_063A;
			IL_0A7C:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 11;
				goto IL_06AE;
			}
			goto IL_06AE;
			IL_0155:
			if (num >= runtextend || runtext[num++] != '#' || 7 > runtextend - num || char.ToLower(runtext[num], CultureInfo.CurrentCulture) != 'i' || char.ToLower(runtext[num + 1], CultureInfo.CurrentCulture) != 'n' || char.ToLower(runtext[num + 2], CultureInfo.CurrentCulture) != 'c' || char.ToLower(runtext[num + 3], CultureInfo.CurrentCulture) != 'l' || char.ToLower(runtext[num + 4], CultureInfo.CurrentCulture) != 'u' || char.ToLower(runtext[num + 5], CultureInfo.CurrentCulture) != 'd' || char.ToLower(runtext[num + 6], CultureInfo.CurrentCulture) != 'e')
			{
				goto IL_0730;
			}
			num += 7;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
			}
			IL_02C3:
			array2[--num3] = num;
			array[--num2] = 1;
			if (1 <= runtextend - num)
			{
				num++;
				num4 = 1;
				while (RegexRunner.CharInClass(runtext[num - num4--], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
				{
					if (num4 <= 0)
					{
						num4 = (num5 = runtextend - num) + 1;
						while (--num4 > 0)
						{
							if (!RegexRunner.CharInClass(runtext[num++], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
							{
								num--;
								break;
							}
						}
						if (num5 > num4)
						{
							array[--num2] = num5 - num4 - 1;
							array[--num2] = num - 1;
							array[--num2] = 4;
							goto IL_0390;
						}
						goto IL_0390;
					}
				}
				goto IL_0730;
			}
			goto IL_0730;
			IL_0390:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 5;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 6;
			}
			IL_0434:
			if (num >= runtextend || runtext[num++] != '=')
			{
				goto IL_0730;
			}
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 7;
			}
			IL_04C7:
			int num6;
			if ((num6 = runtextend - num) >= 1)
			{
				num6 = 1;
			}
			num4 = (num5 = num6) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\u0004\0\"#'("))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 8;
			}
			IL_0543:
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 9;
			}
			IL_058E:
			num4 = array2[num3++];
			this.Capture(2, num4, num);
			array[--num2] = num4;
			array[--num2] = 5;
			int num7;
			if ((num7 = runtextend - num) >= 1)
			{
				num7 = 1;
			}
			num4 = (num5 = num7) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\u0004\0\"#'("))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 10;
			}
			IL_063A:
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 11;
			}
			IL_06AE:
			if (3 > runtextend - num || runtext[num] != '-' || runtext[num + 1] != '-' || runtext[num + 2] != '>')
			{
				goto IL_0730;
			}
			num += 3;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 5;
			IL_0727:
			this.runtextpos = num;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00007328 File Offset: 0x00006328
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00007354 File Offset: 0x00006354
		public override void InitTrackCount()
		{
			this.runtrackcount = 16;
		}
	}
}
