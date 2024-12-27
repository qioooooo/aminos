using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000E RID: 14
	internal class AspExprRegexRunner5 : RegexRunner
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00005800 File Offset: 0x00004800
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
			int num4;
			if (num == this.runtextstart && 2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%')
			{
				num += 2;
				if ((num4 = runtextend - num) > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num;
					array[--num2] = 2;
					goto IL_00EE;
				}
				goto IL_00EE;
			}
			int num5;
			for (;;)
			{
				IL_02B4:
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
					goto IL_031D;
				case 1:
					num3++;
					break;
				case 2:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_15;
					}
					break;
				case 3:
					num3 += 2;
					break;
				case 4:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
					{
						goto Block_17;
					}
					break;
				case 5:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 6:
					if ((num4 = array2[num3++] - 1) >= 0)
					{
						goto Block_19;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num4;
					break;
				case 7:
					goto IL_0136;
				case 8:
					num4 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num4;
					break;
				}
			}
			IL_031D:
			num = array[num2++];
			goto IL_02AB;
			Block_15:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 2;
				goto IL_00EE;
			}
			goto IL_00EE;
			Block_17:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 4;
				goto IL_0181;
			}
			goto IL_0181;
			Block_19:
			num = array2[num3++];
			array[--num2] = num4;
			array[--num2] = 8;
			goto IL_0245;
			IL_00EE:
			if (num < runtextend && runtext[num++] == '=')
			{
				array2[--num3] = -1;
				array2[--num3] = 0;
				array[--num2] = 3;
				goto IL_01B1;
			}
			goto IL_02B4;
			IL_0136:
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 4;
			}
			IL_0181:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 5;
			IL_01B1:
			num4 = array2[num3++];
			int num6 = (num5 = array2[num3++]);
			array[--num2] = num5;
			if ((num6 != num || num4 < 0) && num4 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 6;
				if (num2 <= 40 || num3 <= 30)
				{
					array[--num2] = 7;
					goto IL_02B4;
				}
				goto IL_0136;
			}
			else
			{
				array[--num2] = num4;
				array[--num2] = 8;
			}
			IL_0245:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_02B4;
			}
			num += 2;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 5;
			IL_02AB:
			this.runtextpos = num;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00005CD8 File Offset: 0x00004CD8
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00005D04 File Offset: 0x00004D04
		public override void InitTrackCount()
		{
			this.runtrackcount = 10;
		}
	}
}
