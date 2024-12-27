using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001D RID: 29
	internal class GTRegexRunner10 : RegexRunner
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00007778 File Offset: 0x00006778
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
			if (num < runtextend && runtext[num++] != '%' && num < runtextend && runtext[num++] == '>')
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

		// Token: 0x06000041 RID: 65 RVA: 0x00007900 File Offset: 0x00006900
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
					if (RegexRunner.CharInClass(runtext[num++], "\0\u0003\0\0%&"))
					{
						goto IL_0059;
					}
				}
				while (num2 > 0);
				bool flag = false;
				goto IL_0062;
				IL_0059:
				num--;
				flag = true;
				IL_0062:
				this.runtextpos = num;
				return flag;
			}
			return false;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000797C File Offset: 0x0000697C
		public override void InitTrackCount()
		{
			this.runtrackcount = 3;
		}
	}
}
