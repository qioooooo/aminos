using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000041 RID: 65
	internal class EvalExpressionRegexRunner22 : RegexRunner
	{
		// Token: 0x06000094 RID: 148 RVA: 0x0000E080 File Offset: 0x0000D080
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
			if (num <= runtextbeg || runtext[num - 1] == '\n')
			{
				num4 = (num5 = runtextend - num) + 1;
				while (--num4 > 0)
				{
					if (!RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\0\0\u0001d"))
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
					goto IL_010F;
				}
				goto IL_010F;
			}
			for (;;)
			{
				IL_03E9:
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
					goto IL_044A;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_0467;
				case 3:
					goto IL_04B7;
				case 4:
					goto IL_0507;
				case 5:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 6:
					goto IL_0576;
				}
			}
			IL_044A:
			num = array[num2++];
			goto IL_03E0;
			IL_0467:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_010F;
			}
			goto IL_010F;
			IL_04B7:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
				goto IL_0211;
			}
			goto IL_0211;
			IL_0507:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_02D0;
			}
			goto IL_02D0;
			IL_0576:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 6;
				goto IL_03A7;
			}
			goto IL_03A7;
			IL_010F:
			if (4 > runtextend - num || char.ToLower(runtext[num], CultureInfo.InvariantCulture) != 'e' || char.ToLower(runtext[num + 1], CultureInfo.InvariantCulture) != 'v' || char.ToLower(runtext[num + 2], CultureInfo.InvariantCulture) != 'a' || char.ToLower(runtext[num + 3], CultureInfo.InvariantCulture) != 'l')
			{
				goto IL_03E9;
			}
			num += 4;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\0\0\u0001d"))
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
			IL_0211:
			if (num >= runtextend || char.ToLower(runtext[num++], CultureInfo.InvariantCulture) != '(')
			{
				goto IL_03E9;
			}
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\0\u0001\0\0"))
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
			IL_02D0:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 5;
			if (num >= runtextend || char.ToLower(runtext[num++], CultureInfo.InvariantCulture) != ')')
			{
				goto IL_03E9;
			}
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\0\0\u0001d"))
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
			IL_03A7:
			if (num < runtextend)
			{
				goto IL_03E9;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 5;
			IL_03E0:
			this.runtextpos = num;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000E654 File Offset: 0x0000D654
		public override bool FindFirstChar()
		{
			int num = this.runtextpos;
			string runtext = this.runtext;
			int num2 = this.runtextend - num;
			if (num2 > 0)
			{
				do
				{
					num2--;
					if (RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\0\u0002\u0001efd"))
					{
						goto IL_0063;
					}
				}
				while (num2 > 0);
				bool flag = false;
				goto IL_006C;
				IL_0063:
				num--;
				flag = true;
				IL_006C:
				this.runtextpos = num;
				return flag;
			}
			return false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000E6D8 File Offset: 0x0000D6D8
		public override void InitTrackCount()
		{
			this.runtrackcount = 9;
		}
	}
}
