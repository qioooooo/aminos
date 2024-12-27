using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000038 RID: 56
	internal class FormatStringRegexRunner19 : RegexRunner
	{
		// Token: 0x0600007F RID: 127 RVA: 0x0000CED4 File Offset: 0x0000BED4
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
			if (num <= runtextbeg || runtext[num - 1] == '\n')
			{
				array2[--num3] = num;
				array[--num2] = 1;
				array2[--num3] = -1;
				array[--num2] = 1;
				goto IL_02AF;
			}
			int num4;
			for (;;)
			{
				IL_039B:
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
					goto IL_040C;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_0429;
				case 3:
					num3 += 2;
					break;
				case 4:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 5:
					if ((num4 = array2[num3++] - 1) >= 0)
					{
						goto Block_15;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 6:
					goto IL_016D;
				case 7:
					num4 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 8:
					goto IL_0532;
				case 9:
					goto IL_00C6;
				case 10:
					array2[--num3] = array[num2++];
					break;
				}
			}
			IL_040C:
			num = array[num2++];
			goto IL_0392;
			IL_0429:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_0144;
			}
			goto IL_0144;
			Block_15:
			num = array2[num3++];
			array[--num2] = num4;
			array[--num2] = 7;
			goto IL_027F;
			IL_0532:
			num = array[num2++];
			int num5 = array2[num3++];
			array[--num2] = 10;
			goto IL_031C;
			IL_00C6:
			array2[--num3] = num;
			array[--num2] = 1;
			int num6;
			num4 = (num6 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (runtext[num++] == '"')
				{
					num--;
					break;
				}
			}
			if (num6 > num4)
			{
				array[--num2] = num6 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
			}
			IL_0144:
			array2[--num3] = -1;
			array2[--num3] = 0;
			array[--num2] = 3;
			goto IL_01EB;
			IL_016D:
			array2[--num3] = num;
			array[--num2] = 1;
			if (2 > runtextend - num || runtext[num] != '"' || runtext[num + 1] != '"')
			{
				goto IL_039B;
			}
			num += 2;
			num4 = array2[num3++];
			this.Capture(3, num4, num);
			array[--num2] = num4;
			array[--num2] = 4;
			IL_01EB:
			num4 = array2[num3++];
			int num7 = (num6 = array2[num3++]);
			array[--num2] = num6;
			if ((num7 != num || num4 < 0) && num4 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 5;
				if (num2 <= 60 || num3 <= 45)
				{
					array[--num2] = 6;
					goto IL_039B;
				}
				goto IL_016D;
			}
			else
			{
				array[--num2] = num4;
				array[--num2] = 7;
			}
			IL_027F:
			num4 = array2[num3++];
			this.Capture(2, num4, num);
			array[--num2] = num4;
			array[--num2] = 4;
			IL_02AF:
			int num8 = (num4 = array2[num3++]);
			array[--num2] = num4;
			if (num8 != num)
			{
				array[--num2] = num;
				array2[--num3] = num;
				array[--num2] = 8;
				if (num2 <= 60 || num3 <= 45)
				{
					array[--num2] = 9;
					goto IL_039B;
				}
				goto IL_00C6;
			}
			else
			{
				array[--num2] = 10;
			}
			IL_031C:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 4;
			if (num < runtextend && runtext[num] != '\n')
			{
				goto IL_039B;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 4;
			IL_0392:
			this.runtextpos = num;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000D454 File Offset: 0x0000C454
		public override bool FindFirstChar()
		{
			return true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000D464 File Offset: 0x0000C464
		public override void InitTrackCount()
		{
			this.runtrackcount = 15;
		}
	}
}
