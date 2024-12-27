using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000011 RID: 17
	internal class DatabindExprRegexRunner6 : RegexRunner
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00005DFC File Offset: 0x00004DFC
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
			if (num == this.runtextstart && 3 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%' && runtext[num + 2] == '#')
			{
				num += 3;
				array2[--num3] = -1;
				array2[--num3] = 0;
				array[--num2] = 2;
				goto IL_0172;
			}
			int num4;
			int num5;
			for (;;)
			{
				IL_0275:
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
					goto IL_02DA;
				case 1:
					num3++;
					break;
				case 2:
					num3 += 2;
					break;
				case 3:
					num = array[num2++];
					num4 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
					{
						goto Block_13;
					}
					break;
				case 4:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 5:
					if ((num5 = array2[num3++] - 1) >= 0)
					{
						goto Block_15;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num5;
					break;
				case 6:
					goto IL_00F7;
				case 7:
					num5 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num5;
					break;
				}
			}
			IL_02DA:
			num = array[num2++];
			goto IL_026C;
			Block_13:
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 3;
				goto IL_0142;
			}
			goto IL_0142;
			Block_15:
			num = array2[num3++];
			array[--num2] = num5;
			array[--num2] = 7;
			goto IL_0206;
			IL_00F7:
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num5 = runtextend - num) > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 3;
			}
			IL_0142:
			num5 = array2[num3++];
			this.Capture(1, num5, num);
			array[--num2] = num5;
			array[--num2] = 4;
			IL_0172:
			num5 = array2[num3++];
			int num6 = (num4 = array2[num3++]);
			array[--num2] = num4;
			if ((num6 != num || num5 < 0) && num5 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num5 + 1;
				array[--num2] = 5;
				if (num2 <= 36 || num3 <= 27)
				{
					array[--num2] = 6;
					goto IL_0275;
				}
				goto IL_00F7;
			}
			else
			{
				array[--num2] = num5;
				array[--num2] = 7;
			}
			IL_0206:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_0275;
			}
			num += 2;
			num5 = array2[num3++];
			this.Capture(0, num5, num);
			array[--num2] = num5;
			array[num2 - 1] = 4;
			IL_026C:
			this.runtextpos = num;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00006224 File Offset: 0x00005224
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00006250 File Offset: 0x00005250
		public override void InitTrackCount()
		{
			this.runtrackcount = 9;
		}
	}
}
