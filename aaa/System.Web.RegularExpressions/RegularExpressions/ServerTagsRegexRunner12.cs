using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000023 RID: 35
	internal class ServerTagsRegexRunner12 : RegexRunner
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00007CB4 File Offset: 0x00006CB4
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
			if (2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%')
			{
				num += 2;
				array2[--num3] = this.runtrack.Length - num2;
				array2[--num3] = this.Crawlpos();
				array[--num2] = 2;
				array[--num2] = num;
				array[--num2] = 3;
				if (num < runtextend && RegexRunner.CharInClass(runtext[num++], "\0\u0002\0#%"))
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
					goto IL_03E7;
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
					array2[--num3] = -1;
					array[--num2] = 1;
					goto IL_02C3;
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
					num = array[num2++];
					num6 = array[num2++];
					if (num6 > 0)
					{
						array[--num2] = num6 - 1;
						array[--num2] = num - 1;
						array[--num2] = 5;
					}
					break;
				case 6:
					array2[--num3] = array[num2++];
					this.Uncapture();
					continue;
				case 7:
				{
					num = array[num2++];
					array2[--num3] = num;
					array[--num2] = 8;
					if (num2 <= 56 || num3 <= 42)
					{
						array[--num2] = 9;
						continue;
					}
					array2[--num3] = num;
					array[--num2] = 1;
					array2[--num3] = num;
					array[--num2] = 1;
					int num8;
					num6 = (num8 = runtextend - num) + 1;
					while (--num6 > 0)
					{
						if (runtext[num++] == '%')
						{
							num--;
							break;
						}
					}
					if (num8 > num6)
					{
						array[--num2] = num8 - num6 - 1;
						array[--num2] = num - 1;
						array[--num2] = 5;
					}
					break;
				}
				case 8:
					array2[num3] = array[num2++];
					continue;
				}
				num6 = array2[num3++];
				this.Capture(2, num6, num);
				array[--num2] = num6;
				array[--num2] = 6;
				if (num >= runtextend || runtext[num++] != '%')
				{
					continue;
				}
				num6 = array2[num3++];
				this.Capture(1, num6, num);
				array[--num2] = num6;
				array[--num2] = 6;
				IL_02C3:
				int num9 = (num6 = array2[num3++]);
				if (num6 != -1)
				{
					array[--num2] = num6;
				}
				else
				{
					array[--num2] = num;
				}
				if (num9 != num)
				{
					array[--num2] = num;
					array[--num2] = 7;
				}
				else
				{
					array2[--num3] = num6;
					array[--num2] = 8;
				}
			}
			while (num >= runtextend || runtext[num++] != '>');
			num6 = array2[num3++];
			this.Capture(0, num6, num);
			array[--num2] = num6;
			array[num2 - 1] = 6;
			IL_0375:
			this.runtextpos = num;
			return;
			IL_03E7:
			num = array[num2++];
			goto IL_0375;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000081D0 File Offset: 0x000071D0
		public override bool FindFirstChar()
		{
			string runtext = this.runtext;
			int runtextend = this.runtextend;
			int num2;
			for (int i = this.runtextpos + 1; i < runtextend; i = num2 + i)
			{
				int num;
				if ((num = (int)runtext[i]) != 37)
				{
					if ((num -= 37) <= 23)
					{
						switch (num)
						{
						default:
							num2 = 0;
							break;
						case 1:
							num2 = 2;
							break;
						case 2:
							num2 = 2;
							break;
						case 3:
							num2 = 2;
							break;
						case 4:
							num2 = 2;
							break;
						case 5:
							num2 = 2;
							break;
						case 6:
							num2 = 2;
							break;
						case 7:
							num2 = 2;
							break;
						case 8:
							num2 = 2;
							break;
						case 9:
							num2 = 2;
							break;
						case 10:
							num2 = 2;
							break;
						case 11:
							num2 = 2;
							break;
						case 12:
							num2 = 2;
							break;
						case 13:
							num2 = 2;
							break;
						case 14:
							num2 = 2;
							break;
						case 15:
							num2 = 2;
							break;
						case 16:
							num2 = 2;
							break;
						case 17:
							num2 = 2;
							break;
						case 18:
							num2 = 2;
							break;
						case 19:
							num2 = 2;
							break;
						case 20:
							num2 = 2;
							break;
						case 21:
							num2 = 2;
							break;
						case 22:
							num2 = 2;
							break;
						case 23:
							num2 = 1;
							break;
						}
					}
					else
					{
						num2 = 2;
					}
				}
				else
				{
					num = i;
					if (runtext[--num] == '<')
					{
						this.runtextpos = num;
						return true;
					}
					num2 = 1;
				}
			}
			this.runtextpos = this.runtextend;
			return false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00008370 File Offset: 0x00007370
		public override void InitTrackCount()
		{
			this.runtrackcount = 14;
		}
	}
}
