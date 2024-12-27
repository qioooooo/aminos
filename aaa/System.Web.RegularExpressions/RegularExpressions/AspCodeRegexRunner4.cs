using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000B RID: 11
	internal class AspCodeRegexRunner4 : RegexRunner
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00005300 File Offset: 0x00004300
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
			if (num == this.runtextstart && 2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%')
			{
				num += 2;
				array2[--num3] = this.runtrack.Length - num2;
				array2[--num3] = this.Crawlpos();
				array[--num2] = 2;
				array[--num2] = num;
				array[--num2] = 3;
				if (num < runtextend && runtext[num++] == '@')
				{
					int num4 = array2[num3++];
					num2 = this.runtrack.Length - array2[num3++];
					int num5 = num4;
					if (num4 != this.Crawlpos())
					{
						do
						{
							this.Uncapture();
						}
						while ((num5 = num5) != this.Crawlpos());
					}
				}
			}
			int num6;
			do
			{
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
					goto IL_02E1;
				case 1:
					num3++;
					continue;
				case 2:
					num3 += 2;
					continue;
				case 3:
					num = array[num2++];
					num6 = array2[num3++];
					num2 = this.runtrack.Length - array2[num3++];
					array[--num2] = num6;
					array[--num2] = 4;
					array2[--num3] = num;
					array[--num2] = 1;
					if ((num6 = runtextend - num) > 0)
					{
						array[--num2] = num6 - 1;
						array[--num2] = num;
						array[--num2] = 5;
					}
					break;
				case 4:
				{
					int num7;
					if ((num7 = array[num2++]) != this.Crawlpos())
					{
						do
						{
							this.Uncapture();
						}
						while ((num7 = num7) != this.Crawlpos());
					}
					continue;
				}
				case 5:
				{
					num = array[num2++];
					int num8 = array[num2++];
					if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
					{
						continue;
					}
					if (num8 > 0)
					{
						array[--num2] = num8 - 1;
						array[--num2] = num;
						array[--num2] = 5;
					}
					break;
				}
				case 6:
					array2[--num3] = array[num2++];
					this.Uncapture();
					continue;
				}
				num6 = array2[num3++];
				this.Capture(1, num6, num);
				array[--num2] = num6;
				array[--num2] = 6;
			}
			while (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>');
			num += 2;
			num6 = array2[num3++];
			this.Capture(0, num6, num);
			array[--num2] = num6;
			array[num2 - 1] = 6;
			IL_0277:
			this.runtextpos = num;
			return;
			IL_02E1:
			num = array[num2++];
			goto IL_0277;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000056DC File Offset: 0x000046DC
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00005708 File Offset: 0x00004708
		public override void InitTrackCount()
		{
			this.runtrackcount = 10;
		}
	}
}
