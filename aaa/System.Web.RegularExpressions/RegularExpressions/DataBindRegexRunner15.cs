using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002C RID: 44
	internal class DataBindRegexRunner15 : RegexRunner
	{
		// Token: 0x06000063 RID: 99 RVA: 0x0000A050 File Offset: 0x00009050
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
			if (num == this.runtextstart)
			{
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
					goto IL_00F9;
				}
				goto IL_00F9;
			}
			for (;;)
			{
				IL_03A5:
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
					goto IL_0416;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_0433;
				case 3:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_23;
					}
					break;
				case 4:
					num3 += 2;
					break;
				case 5:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
					{
						goto Block_25;
					}
					break;
				case 6:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 7:
					if ((num4 = array2[num3++] - 1) >= 0)
					{
						goto Block_27;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 8:
					goto IL_01AA;
				case 9:
					num4 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 10:
					goto IL_0612;
				}
			}
			IL_0416:
			num = array[num2++];
			goto IL_039C;
			IL_0433:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_00F9;
			}
			goto IL_00F9;
			Block_23:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 3;
				goto IL_0162;
			}
			goto IL_0162;
			Block_25:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 5;
				goto IL_01F5;
			}
			goto IL_01F5;
			Block_27:
			num = array2[num3++];
			array[--num2] = num4;
			array[--num2] = 9;
			goto IL_02B9;
			IL_0612:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 10;
				goto IL_0363;
			}
			goto IL_0363;
			IL_00F9:
			if (2 > runtextend - num || runtext[num] != '<' || runtext[num + 1] != '%')
			{
				goto IL_03A5;
			}
			num += 2;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 3;
			}
			IL_0162:
			if (num < runtextend && runtext[num++] == '#')
			{
				array2[--num3] = -1;
				array2[--num3] = 0;
				array[--num2] = 4;
				goto IL_0225;
			}
			goto IL_03A5;
			IL_01AA:
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 5;
			}
			IL_01F5:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 6;
			IL_0225:
			num4 = array2[num3++];
			int num6 = (num5 = array2[num3++]);
			array[--num2] = num5;
			if ((num6 != num || num4 < 0) && num4 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 7;
				if (num2 <= 48 || num3 <= 36)
				{
					array[--num2] = 8;
					goto IL_03A5;
				}
				goto IL_01AA;
			}
			else
			{
				array[--num2] = num4;
				array[--num2] = 9;
			}
			IL_02B9:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_03A5;
			}
			num += 2;
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
				array[--num2] = 10;
			}
			IL_0363:
			if (num < runtextend)
			{
				goto IL_03A5;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 6;
			IL_039C:
			this.runtextpos = num;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000A6C0 File Offset: 0x000096C0
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000A6EC File Offset: 0x000096EC
		public override void InitTrackCount()
		{
			this.runtrackcount = 12;
		}
	}
}
