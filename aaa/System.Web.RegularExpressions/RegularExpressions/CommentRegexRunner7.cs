using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000014 RID: 20
	internal class CommentRegexRunner7 : RegexRunner
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00006348 File Offset: 0x00005348
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
			if (num == this.runtextstart && 4 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%' && runtext[num + 2] == '-' && runtext[num + 3] == '-')
			{
				num += 4;
				array2[--num3] = -1;
				array[--num2] = 1;
				goto IL_0213;
			}
			int num4;
			do
			{
				IL_02F8:
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
					goto IL_0355;
				case 1:
					num3++;
					continue;
				case 2:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 2;
					}
					break;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					continue;
				case 4:
				{
					num = array[num2++];
					array2[--num3] = num;
					array[--num2] = 5;
					if (num2 <= 40 || num3 <= 30)
					{
						array[--num2] = 6;
						continue;
					}
					array2[--num3] = num;
					array[--num2] = 1;
					array2[--num3] = num;
					array[--num2] = 1;
					int num5;
					num4 = (num5 = runtextend - num) + 1;
					while (--num4 > 0)
					{
						if (runtext[num++] == '-')
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
					}
					break;
				}
				case 5:
					array2[num3] = array[num2++];
					continue;
				}
				num4 = array2[num3++];
				this.Capture(2, num4, num);
				array[--num2] = num4;
				array[--num2] = 3;
			}
			while (num >= runtextend || runtext[num++] != '-');
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			goto IL_0213;
			IL_0355:
			num = array[num2++];
			goto IL_02EF;
			IL_0213:
			int num6 = (num4 = array2[num3++]);
			if (num4 != -1)
			{
				array[--num2] = num4;
			}
			else
			{
				array[--num2] = num;
			}
			if (num6 != num)
			{
				array[--num2] = num;
				array[--num2] = 4;
			}
			else
			{
				array2[--num3] = num4;
				array[--num2] = 5;
			}
			if (3 > runtextend - num || runtext[num] != '-' || runtext[num + 1] != '%' || runtext[num + 2] != '>')
			{
				goto IL_02F8;
			}
			num += 3;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_02EF:
			this.runtextpos = num;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00006790 File Offset: 0x00005790
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000067BC File Offset: 0x000057BC
		public override void InitTrackCount()
		{
			this.runtrackcount = 10;
		}
	}
}
