using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002F RID: 47
	internal class ExpressionBuilderRegexRunner16 : RegexRunner
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000A7E4 File Offset: 0x000097E4
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
				IL_049B:
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
					goto IL_0510;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_052D;
				case 3:
					goto IL_057D;
				case 4:
					goto IL_05CD;
				case 5:
					num3 += 2;
					break;
				case 6:
					goto IL_0629;
				case 7:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 8:
					if ((num4 = array2[num3++] - 1) >= 0)
					{
						goto Block_33;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 9:
					goto IL_025F;
				case 10:
					num4 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 11:
					goto IL_0726;
				}
			}
			IL_0510:
			num = array[num2++];
			goto IL_0492;
			IL_052D:
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
			IL_057D:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
				goto IL_01A3;
			}
			goto IL_01A3;
			IL_05CD:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_0236;
			}
			goto IL_0236;
			IL_0629:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 6;
				goto IL_02EB;
			}
			goto IL_02EB;
			Block_33:
			num = array2[num3++];
			array[--num2] = num4;
			array[--num2] = 10;
			goto IL_03AF;
			IL_0726:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 11;
				goto IL_0459;
			}
			goto IL_0459;
			IL_00F9:
			if (2 > runtextend - num || runtext[num] != '<' || runtext[num + 1] != '%')
			{
				goto IL_049B;
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
				array[--num2] = 3;
			}
			IL_01A3:
			if (num >= runtextend || runtext[num++] != '$')
			{
				goto IL_049B;
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
				array[--num2] = 4;
			}
			IL_0236:
			array2[--num3] = -1;
			array2[--num3] = 0;
			array[--num2] = 5;
			goto IL_031B;
			IL_025F:
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
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
			IL_02EB:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 7;
			IL_031B:
			num4 = array2[num3++];
			int num6 = (num5 = array2[num3++]);
			array[--num2] = num5;
			if ((num6 != num || num4 < 0) && num4 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 8;
				if (num2 <= 52 || num3 <= 39)
				{
					array[--num2] = 9;
					goto IL_049B;
				}
				goto IL_025F;
			}
			else
			{
				array[--num2] = num4;
				array[--num2] = 10;
			}
			IL_03AF:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_049B;
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
				array[--num2] = 11;
			}
			IL_0459:
			if (num < runtextend)
			{
				goto IL_049B;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 7;
			IL_0492:
			this.runtextpos = num;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000AF68 File Offset: 0x00009F68
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000AF94 File Offset: 0x00009F94
		public override void InitTrackCount()
		{
			this.runtrackcount = 13;
		}
	}
}
