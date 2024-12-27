using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000020 RID: 32
	internal class LTRegexRunner11 : RegexRunner
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00007A10 File Offset: 0x00006A10
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
			if (num < runtextend && runtext[num++] == '<' && num < runtextend && runtext[num++] != '%')
			{
				int num4 = array2[num3++];
				this.Capture(0, num4, num);
				array[--num2] = num4;
				array[num2 - 1] = 2;
			}
			else
			{
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
						goto IL_0140;
					case 1:
						num3++;
						break;
					case 2:
						array2[--num3] = array[num2++];
						this.Uncapture();
						break;
					}
				}
				IL_0140:
				num = array[num2++];
			}
			this.runtextpos = num;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00007B98 File Offset: 0x00006B98
		public override bool FindFirstChar()
		{
			string runtext = this.runtext;
			int runtextend = this.runtextend;
			int num2;
			for (int i = this.runtextpos + 0; i < runtextend; i = num2 + i)
			{
				int num;
				if ((num = (int)runtext[i]) == 60)
				{
					num = i;
					this.runtextpos = num;
					return true;
				}
				if ((num -= 60) == 0)
				{
					switch (num)
					{
					default:
						num2 = 0;
						break;
					}
				}
				else
				{
					num2 = 1;
				}
			}
			this.runtextpos = this.runtextend;
			return false;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00007C20 File Offset: 0x00006C20
		public override void InitTrackCount()
		{
			this.runtrackcount = 3;
		}
	}
}
