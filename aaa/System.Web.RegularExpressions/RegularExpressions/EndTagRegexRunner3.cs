using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000008 RID: 8
	internal class EndTagRegexRunner3 : RegexRunner
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00004E14 File Offset: 0x00003E14
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
			int num5;
			if (num == this.runtextstart && 2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '/')
			{
				num += 2;
				array2[--num3] = num;
				array[--num2] = 1;
				if (1 <= runtextend - num)
				{
					num++;
					num4 = 1;
					while (RegexRunner.CharInClass(runtext[num - num4--], "\0\u0004\t./:;\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
					{
						if (num4 <= 0)
						{
							num4 = (num5 = runtextend - num) + 1;
							while (--num4 > 0)
							{
								if (!RegexRunner.CharInClass(runtext[num++], "\0\u0004\t./:;\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
								goto IL_0188;
							}
							goto IL_0188;
						}
					}
				}
			}
			for (;;)
			{
				IL_0284:
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
					goto IL_02DD;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_02FA;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				case 4:
					goto IL_0369;
				}
			}
			IL_02DD:
			num = array[num2++];
			goto IL_027B;
			IL_02FA:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_0188;
			}
			goto IL_0188;
			IL_0369:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_022C;
			}
			goto IL_022C;
			IL_0188:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
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
			IL_022C:
			if (num >= runtextend || runtext[num++] != '>')
			{
				goto IL_0284;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_027B:
			this.runtextpos = num;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000051DC File Offset: 0x000041DC
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00005208 File Offset: 0x00004208
		public override void InitTrackCount()
		{
			this.runtrackcount = 7;
		}
	}
}
