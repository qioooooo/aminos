using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000002 RID: 2
	internal class TagRegexRunner1 : RegexRunner
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
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
			if (num == this.runtextstart && num < runtextend && runtext[num++] == '<')
			{
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
								goto IL_0171;
							}
							goto IL_0171;
						}
					}
				}
			}
			for (;;)
			{
				IL_0EE8:
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
					goto IL_0FA9;
				case 1:
					num3++;
					continue;
				case 2:
					goto IL_0FC6;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					continue;
				case 4:
					goto IL_1035;
				case 5:
					goto IL_1085;
				case 6:
					num = array[num2++];
					array[--num2] = num;
					array[--num2] = 10;
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
					break;
				case 7:
					goto IL_10E6;
				case 8:
					goto IL_1136;
				case 9:
					goto IL_1186;
				case 10:
					num = array[num2++];
					array[--num2] = num;
					array[--num2] = 14;
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
						goto IL_0832;
					}
					goto IL_0832;
				case 11:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 11;
					}
					break;
				case 12:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 12;
						goto IL_06B5;
					}
					goto IL_06B5;
				case 13:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 13;
						goto IL_0752;
					}
					goto IL_0752;
				case 14:
					num = array[num2++];
					array[--num2] = num;
					array[--num2] = 18;
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
						goto IL_0A50;
					}
					goto IL_0A50;
				case 15:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 15;
						goto IL_0832;
					}
					goto IL_0832;
				case 16:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 16;
						goto IL_08C5;
					}
					goto IL_08C5;
				case 17:
					num = array[num2++];
					num5 = array[num2++];
					if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
					{
						continue;
					}
					if (num5 > 0)
					{
						array[--num2] = num5 - 1;
						array[--num2] = num;
						array[--num2] = 17;
						goto IL_0959;
					}
					goto IL_0959;
				case 18:
					goto IL_13F3;
				case 19:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 19;
						goto IL_0A50;
					}
					goto IL_0A50;
				case 20:
					goto IL_1454;
				case 21:
					goto IL_14A4;
				case 22:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_107;
					}
					continue;
				case 23:
					goto IL_155F;
				case 24:
					goto IL_01BE;
				case 25:
					array2[--num3] = array[num2++];
					continue;
				case 26:
					goto IL_15A0;
				case 27:
					num3 += 2;
					continue;
				case 28:
					if ((num4 = array2[num3++] - 1) >= 0)
					{
						goto Block_110;
					}
					array2[num3] = array[num2++];
					array2[--num3] = num4;
					continue;
				case 29:
					goto IL_0D8F;
				case 30:
					num4 = array[num2++];
					array2[--num3] = array[num2++];
					array2[--num3] = num4;
					continue;
				}
				if (num >= runtextend || runtext[num++] != '=')
				{
					continue;
				}
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
					array[--num2] = 12;
				}
				IL_06B5:
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
					array[--num2] = 13;
				}
				IL_0752:
				num4 = array2[num3++];
				this.Capture(5, num4, num);
				array[--num2] = num4;
				array[--num2] = 3;
				if (num < runtextend && runtext[num++] == '\'')
				{
					break;
				}
				continue;
				IL_0832:
				if (num >= runtextend || runtext[num++] != '=')
				{
					continue;
				}
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
					array[--num2] = 16;
				}
				IL_08C5:
				array2[--num3] = num;
				array[--num2] = 1;
				if (3 > runtextend - num || runtext[num] != '<' || runtext[num + 1] != '%' || runtext[num + 2] != '#')
				{
					continue;
				}
				num += 3;
				if ((num4 = runtextend - num) > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num;
					array[--num2] = 17;
				}
				IL_0959:
				if (2 <= runtextend - num && runtext[num] == '%' && runtext[num + 1] == '>')
				{
					goto Block_66;
				}
				continue;
				IL_0A50:
				if (num < runtextend && runtext[num++] == '=')
				{
					goto Block_71;
				}
			}
			goto IL_0C1F;
			Block_66:
			num += 2;
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			goto IL_0C1F;
			Block_71:
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
				array[--num2] = 20;
			}
			IL_0AE3:
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\u0001\u0004\u0001/0=?d"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 21;
			}
			IL_0B6F:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			goto IL_0C1F;
			IL_0BEF:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			goto IL_0C1F;
			IL_0FA9:
			num = array[num2++];
			goto IL_0EDF;
			IL_0FC6:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_0171;
			}
			goto IL_0171;
			IL_1035:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_028B;
			}
			goto IL_028B;
			IL_1085:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 5;
				goto IL_033E;
			}
			goto IL_033E;
			IL_10E6:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 7;
				goto IL_0412;
			}
			goto IL_0412;
			IL_1136:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 8;
				goto IL_04A5;
			}
			goto IL_04A5;
			IL_1186:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 9;
				goto IL_0542;
			}
			goto IL_0542;
			IL_13F3:
			num = array[num2++];
			array2[--num3] = num;
			array[--num2] = 1;
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 22;
				goto IL_0BEF;
			}
			goto IL_0BEF;
			IL_1454:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 20;
				goto IL_0AE3;
			}
			goto IL_0AE3;
			IL_14A4:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 21;
				goto IL_0B6F;
			}
			goto IL_0B6F;
			Block_107:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 22;
				goto IL_0BEF;
			}
			goto IL_0BEF;
			IL_155F:
			num = array[num2++];
			int num6 = array2[num3++];
			array[--num2] = 25;
			goto IL_0CF2;
			IL_15A0:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 26;
				goto IL_0D66;
			}
			goto IL_0D66;
			Block_110:
			num = array2[num3++];
			array[--num2] = num4;
			array[--num2] = 30;
			goto IL_0E90;
			IL_0171:
			num4 = array2[num3++];
			this.Capture(3, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			array2[--num3] = -1;
			array[--num2] = 1;
			goto IL_0C7F;
			IL_01BE:
			array2[--num3] = num;
			array[--num2] = 1;
			if (1 <= runtextend - num)
			{
				num++;
				num4 = 1;
				while (RegexRunner.CharInClass(runtext[num - num4--], "\0\0\u0001d"))
				{
					if (num4 <= 0)
					{
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
							goto IL_028B;
						}
						goto IL_028B;
					}
				}
				goto IL_0EE8;
			}
			goto IL_0EE8;
			IL_028B:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || !RegexRunner.CharInClass(runtext[num++], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
			{
				goto IL_0EE8;
			}
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\0\u0004\t-.:;\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
				{
					num--;
					break;
				}
			}
			if (num5 > num4)
			{
				array[--num2] = num5 - num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 5;
			}
			IL_033E:
			num4 = array2[num3++];
			this.Capture(4, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			array2[--num3] = num;
			array[--num2] = 1;
			array[--num2] = num;
			array[--num2] = 6;
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
				array[--num2] = 7;
			}
			IL_0412:
			if (num >= runtextend || runtext[num++] != '=')
			{
				goto IL_0EE8;
			}
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
				array[--num2] = 8;
			}
			IL_04A5:
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0EE8;
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
				array[--num2] = 9;
			}
			IL_0542:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0EE8;
			}
			IL_0C1F:
			num4 = array2[num3++];
			this.Capture(2, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			IL_0C7F:
			int num7 = (num4 = array2[num3++]);
			array[--num2] = num4;
			if (num7 != num)
			{
				array[--num2] = num;
				array2[--num3] = num;
				array[--num2] = 23;
				if (num2 <= 212 || num3 <= 159)
				{
					array[--num2] = 24;
					goto IL_0EE8;
				}
				goto IL_01BE;
			}
			else
			{
				array[--num2] = 25;
			}
			IL_0CF2:
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
				array[--num2] = 26;
			}
			IL_0D66:
			array2[--num3] = -1;
			array2[--num3] = 0;
			array[--num2] = 27;
			goto IL_0DF6;
			IL_0D8F:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || runtext[num++] != '/')
			{
				goto IL_0EE8;
			}
			num4 = array2[num3++];
			this.Capture(6, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			IL_0DF6:
			num4 = array2[num3++];
			int num8 = (num5 = array2[num3++]);
			array[--num2] = num5;
			if ((num8 != num || num4 < 0) && num4 < 1)
			{
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 28;
				if (num2 <= 212 || num3 <= 159)
				{
					array[--num2] = 29;
					goto IL_0EE8;
				}
				goto IL_0D8F;
			}
			else
			{
				array[--num2] = num4;
				array[--num2] = 30;
			}
			IL_0E90:
			if (num >= runtextend || runtext[num++] != '>')
			{
				goto IL_0EE8;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_0EDF:
			this.runtextpos = num;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00003768 File Offset: 0x00002768
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00003794 File Offset: 0x00002794
		public override void InitTrackCount()
		{
			this.runtrackcount = 53;
		}
	}
}
