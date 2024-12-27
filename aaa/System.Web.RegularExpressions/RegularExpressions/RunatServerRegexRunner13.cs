using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000026 RID: 38
	internal class RunatServerRegexRunner13 : RegexRunner
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00008404 File Offset: 0x00007404
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
			if (5 <= runtextend - num && char.ToLower(runtext[num], CultureInfo.InvariantCulture) == 'r' && char.ToLower(runtext[num + 1], CultureInfo.InvariantCulture) == 'u' && char.ToLower(runtext[num + 2], CultureInfo.InvariantCulture) == 'n' && char.ToLower(runtext[num + 3], CultureInfo.InvariantCulture) == 'a' && char.ToLower(runtext[num + 4], CultureInfo.InvariantCulture) == 't')
			{
				num += 5;
				int num5;
				num4 = (num5 = runtextend - num) + 1;
				while (--num4 > 0)
				{
					if (!RegexRunner.CharInClass(char.ToLower(runtext[num++], CultureInfo.InvariantCulture), "\u0001\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
					goto IL_0197;
				}
				goto IL_0197;
			}
			for (;;)
			{
				IL_028E:
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
					goto IL_02E3;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_0300;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				}
			}
			IL_02E3:
			num = array[num2++];
			goto IL_0285;
			IL_0300:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
			}
			IL_0197:
			if (6 > runtextend - num || char.ToLower(runtext[num], CultureInfo.InvariantCulture) != 's' || char.ToLower(runtext[num + 1], CultureInfo.InvariantCulture) != 'e' || char.ToLower(runtext[num + 2], CultureInfo.InvariantCulture) != 'r' || char.ToLower(runtext[num + 3], CultureInfo.InvariantCulture) != 'v' || char.ToLower(runtext[num + 4], CultureInfo.InvariantCulture) != 'e' || char.ToLower(runtext[num + 5], CultureInfo.InvariantCulture) != 'r')
			{
				goto IL_028E;
			}
			num += 6;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_0285:
			this.runtextpos = num;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00008780 File Offset: 0x00007780
		public override bool FindFirstChar()
		{
			string runtext = this.runtext;
			int runtextend = this.runtextend;
			int num2;
			for (int i = this.runtextpos + 4; i < runtextend; i = num2 + i)
			{
				int num;
				if ((num = (int)char.ToLower(runtext[i], CultureInfo.InvariantCulture)) != 116)
				{
					if ((num -= 97) <= 20)
					{
						switch (num)
						{
						default:
							num2 = 1;
							break;
						case 1:
							num2 = 5;
							break;
						case 2:
							num2 = 5;
							break;
						case 3:
							num2 = 5;
							break;
						case 4:
							num2 = 5;
							break;
						case 5:
							num2 = 5;
							break;
						case 6:
							num2 = 5;
							break;
						case 7:
							num2 = 5;
							break;
						case 8:
							num2 = 5;
							break;
						case 9:
							num2 = 5;
							break;
						case 10:
							num2 = 5;
							break;
						case 11:
							num2 = 5;
							break;
						case 12:
							num2 = 5;
							break;
						case 13:
							num2 = 2;
							break;
						case 14:
							num2 = 5;
							break;
						case 15:
							num2 = 5;
							break;
						case 16:
							num2 = 5;
							break;
						case 17:
							num2 = 4;
							break;
						case 18:
							num2 = 5;
							break;
						case 19:
							num2 = 0;
							break;
						case 20:
							num2 = 3;
							break;
						}
					}
					else
					{
						num2 = 5;
					}
				}
				else
				{
					num = i;
					if (char.ToLower(runtext[--num], CultureInfo.InvariantCulture) != 'a')
					{
						num2 = 1;
					}
					else if (char.ToLower(runtext[--num], CultureInfo.InvariantCulture) != 'n')
					{
						num2 = 1;
					}
					else if (char.ToLower(runtext[--num], CultureInfo.InvariantCulture) != 'u')
					{
						num2 = 1;
					}
					else
					{
						if (char.ToLower(runtext[--num], CultureInfo.InvariantCulture) == 'r')
						{
							this.runtextpos = num;
							return true;
						}
						num2 = 1;
					}
				}
			}
			this.runtextpos = this.runtextend;
			return false;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00008980 File Offset: 0x00007980
		public override void InitTrackCount()
		{
			this.runtrackcount = 4;
		}
	}
}
