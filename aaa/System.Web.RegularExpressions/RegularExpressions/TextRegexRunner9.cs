using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001A RID: 26
	internal class TextRegexRunner9 : RegexRunner
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00007470 File Offset: 0x00006470
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
			if (num == this.runtextstart && 1 <= runtextend - num)
			{
				num++;
				num4 = 1;
				while (runtext[num - num4--] != '<')
				{
					if (num4 <= 0)
					{
						int num5;
						num4 = (num5 = runtextend - num) + 1;
						while (--num4 > 0)
						{
							if (runtext[num++] == '<')
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
							goto IL_0121;
						}
						goto IL_0121;
					}
				}
			}
			for (;;)
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
					goto IL_01AF;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_01CC;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				}
			}
			IL_01AF:
			num = array[num2++];
			goto IL_0151;
			IL_01CC:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
			}
			IL_0121:
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_0151:
			this.runtextpos = num;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000076B8 File Offset: 0x000066B8
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000076E4 File Offset: 0x000066E4
		public override void InitTrackCount()
		{
			this.runtrackcount = 4;
		}
	}
}
