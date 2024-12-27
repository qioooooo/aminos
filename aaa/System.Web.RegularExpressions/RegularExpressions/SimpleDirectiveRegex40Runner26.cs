using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200004D RID: 77
	internal class SimpleDirectiveRegex40Runner26 : RegexRunner
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00011D5C File Offset: 0x00010D5C
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
			if (2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%')
			{
				num += 2;
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
					array[--num2] = 2;
					goto IL_0122;
				}
				goto IL_0122;
			}
			for (;;)
			{
				IL_0CBF:
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
					goto IL_0D6C;
				case 1:
					num3++;
					continue;
				case 2:
					goto IL_0D89;
				case 3:
					goto IL_0DD9;
				case 4:
					goto IL_0E29;
				case 5:
					num3 += 2;
					continue;
				case 6:
					array2[--num3] = array[num2++];
					continue;
				case 7:
				{
					int num6;
					if ((num6 = array[num2++]) != this.Crawlpos())
					{
						do
						{
							this.Uncapture();
						}
						while ((num6 = num6) != this.Crawlpos());
					}
					continue;
				}
				case 8:
					array2[--num3] = array[num2++];
					this.Uncapture();
					continue;
				case 9:
					num = array[num2++];
					array[--num2] = num;
					array[--num2] = 13;
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
						array[--num2] = 14;
					}
					break;
				case 10:
					goto IL_0EF6;
				case 11:
					goto IL_0F46;
				case 12:
					goto IL_0F96;
				case 13:
					num = array[num2++];
					array[--num2] = num;
					array[--num2] = 17;
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
						array[--num2] = 18;
						goto IL_08EB;
					}
					goto IL_08EB;
				case 14:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 14;
					}
					break;
				case 15:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 15;
						goto IL_076E;
					}
					goto IL_076E;
				case 16:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 16;
						goto IL_080B;
					}
					goto IL_080B;
				case 17:
					goto IL_10E7;
				case 18:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 18;
						goto IL_08EB;
					}
					goto IL_08EB;
				case 19:
					goto IL_1148;
				case 20:
					goto IL_1198;
				case 21:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_79;
					}
					continue;
				case 22:
					goto IL_1253;
				case 23:
					goto IL_015E;
				case 24:
					array2[--num3] = array[num2++];
					continue;
				case 25:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_81;
					}
					continue;
				}
				array2[--num3] = num;
				array[--num2] = 1;
				if (num >= runtextend || runtext[num++] != '=')
				{
					continue;
				}
				num4 = array2[num3++];
				this.Capture(4, num4, num);
				array[--num2] = num4;
				array[--num2] = 8;
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
					array[--num2] = 15;
				}
				IL_076E:
				if (num >= runtextend || runtext[num++] != '\'')
				{
					continue;
				}
				array2[--num3] = num;
				array[--num2] = 1;
				num4 = (num5 = runtextend - num) + 1;
				while (--num4 > 0)
				{
					if (runtext[num++] == '\'')
					{
						num--;
						break;
					}
				}
				if (num5 > num4)
				{
					array[--num2] = num5 - num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 16;
				}
				IL_080B:
				num4 = array2[num3++];
				this.Capture(5, num4, num);
				array[--num2] = num4;
				array[--num2] = 8;
				if (num < runtextend && runtext[num++] == '\'')
				{
					break;
				}
				continue;
				IL_08EB:
				array2[--num3] = num;
				array[--num2] = 1;
				if (num < runtextend && runtext[num++] == '=')
				{
					goto Block_53;
				}
			}
			goto IL_0B4A;
			Block_53:
			num4 = array2[num3++];
			this.Capture(4, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
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
				array[--num2] = 19;
			}
			IL_09C6:
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\u0001\b\u0001\"#%&'(>?d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 20;
			}
			IL_0A52:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			goto IL_0B4A;
			IL_0B1A:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			goto IL_0B4A;
			IL_0D6C:
			num = array[num2++];
			goto IL_0CB6;
			IL_0D89:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_0122;
			}
			goto IL_0122;
			IL_0DD9:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
				goto IL_01EA;
			}
			goto IL_01EA;
			IL_0E29:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_029D;
			}
			goto IL_029D;
			IL_0EF6:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 10;
				goto IL_043B;
			}
			goto IL_043B;
			IL_0F46:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 11;
				goto IL_0516;
			}
			goto IL_0516;
			IL_0F96:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 12;
				goto IL_05B3;
			}
			goto IL_05B3;
			IL_10E7:
			num = array[num2++];
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = array2[num3++];
			this.Capture(4, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 21;
				goto IL_0B1A;
			}
			goto IL_0B1A;
			IL_1148:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 19;
				goto IL_09C6;
			}
			goto IL_09C6;
			IL_1198:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 20;
				goto IL_0A52;
			}
			goto IL_0A52;
			Block_79:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 21;
				goto IL_0B1A;
			}
			goto IL_0B1A;
			IL_1253:
			num = array[num2++];
			int num7 = array2[num3++];
			array[--num2] = 24;
			goto IL_0C1D;
			Block_81:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 25;
				goto IL_0C50;
			}
			goto IL_0C50;
			IL_0122:
			if (num < runtextend && runtext[num++] == '@')
			{
				array2[--num3] = -1;
				array[--num2] = 1;
				goto IL_0BAA;
			}
			goto IL_0CBF;
			IL_015E:
			array2[--num3] = num;
			array[--num2] = 1;
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
				array[--num2] = 3;
			}
			IL_01EA:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || !RegexRunner.CharInClass(runtext[num++], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
			{
				goto IL_0CBF;
			}
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\u0002\t:;\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
			IL_029D:
			array2[--num3] = this.runtrack.Length - num2;
			array2[--num3] = this.Crawlpos();
			array[--num2] = 5;
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || !RegexRunner.CharInClass(runtext[num++], "\u0001\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
			{
				goto IL_0CBF;
			}
			num = (array[--num2] = array2[num3++]);
			array[num2 - 1] = 6;
			num4 = array2[num3++];
			num2 = this.runtrack.Length - array2[num3++];
			array[--num2] = num4;
			array[--num2] = 7;
			num4 = array2[num3++];
			this.Capture(3, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			array2[--num3] = num;
			array[--num2] = 1;
			array[--num2] = num;
			array[--num2] = 9;
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
				array[--num2] = 10;
			}
			IL_043B:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || runtext[num++] != '=')
			{
				goto IL_0CBF;
			}
			num4 = array2[num3++];
			this.Capture(4, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
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
				array[--num2] = 11;
			}
			IL_0516:
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0CBF;
			}
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (runtext[num++] == '"')
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 12;
			}
			IL_05B3:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0CBF;
			}
			IL_0B4A:
			num4 = array2[num3++];
			this.Capture(2, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			IL_0BAA:
			int num8 = (num4 = array2[num3++]);
			array[--num2] = num4;
			if (num8 != num)
			{
				array[--num2] = num;
				array2[--num3] = num;
				array[--num2] = 22;
				if (num2 <= 204 || num3 <= 153)
				{
					array[--num2] = 23;
					goto IL_0CBF;
				}
				goto IL_015E;
			}
			else
			{
				array[--num2] = 24;
			}
			IL_0C1D:
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 25;
			}
			IL_0C50:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_0CBF;
			}
			num += 2;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 8;
			IL_0CB6:
			this.runtextpos = num;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00013068 File Offset: 0x00012068
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

		// Token: 0x060000B2 RID: 178 RVA: 0x00013208 File Offset: 0x00012208
		public override void InitTrackCount()
		{
			this.runtrackcount = 51;
		}
	}
}
