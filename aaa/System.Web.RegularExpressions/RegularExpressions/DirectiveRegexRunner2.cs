using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000005 RID: 5
	internal class DirectiveRegexRunner2 : RegexRunner
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00003944 File Offset: 0x00002944
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
			if (num == this.runtextstart && 2 <= runtextend - num && runtext[num] == '<' && runtext[num + 1] == '%')
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
					goto IL_012F;
				}
				goto IL_012F;
			}
			for (;;)
			{
				IL_0CCC:
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
					goto IL_0D79;
				case 1:
					num3++;
					continue;
				case 2:
					goto IL_0D96;
				case 3:
					goto IL_0DE6;
				case 4:
					goto IL_0E36;
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
					goto IL_0F03;
				case 11:
					goto IL_0F53;
				case 12:
					goto IL_0FA3;
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
						goto IL_08F8;
					}
					goto IL_08F8;
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
						goto IL_077B;
					}
					goto IL_077B;
				case 16:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 16;
						goto IL_0818;
					}
					goto IL_0818;
				case 17:
					goto IL_10F4;
				case 18:
					num = array[num2++];
					num4 = array[num2++];
					if (num4 > 0)
					{
						array[--num2] = num4 - 1;
						array[--num2] = num - 1;
						array[--num2] = 18;
						goto IL_08F8;
					}
					goto IL_08F8;
				case 19:
					goto IL_1155;
				case 20:
					goto IL_11A5;
				case 21:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_80;
					}
					continue;
				case 22:
					goto IL_1260;
				case 23:
					goto IL_016B;
				case 24:
					array2[--num3] = array[num2++];
					continue;
				case 25:
					num = array[num2++];
					num5 = array[num2++];
					if (RegexRunner.CharInClass(runtext[num++], "\0\0\u0001d"))
					{
						goto Block_82;
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
				IL_077B:
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
				IL_0818:
				num4 = array2[num3++];
				this.Capture(5, num4, num);
				array[--num2] = num4;
				array[--num2] = 8;
				if (num < runtextend && runtext[num++] == '\'')
				{
					break;
				}
				continue;
				IL_08F8:
				array2[--num3] = num;
				array[--num2] = 1;
				if (num < runtextend && runtext[num++] == '=')
				{
					goto Block_54;
				}
			}
			goto IL_0B57;
			Block_54:
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
			IL_09D3:
			array2[--num3] = num;
			array[--num2] = 1;
			num4 = (num5 = runtextend - num) + 1;
			while (--num4 > 0)
			{
				if (!RegexRunner.CharInClass(runtext[num++], "\u0001\u0004\u0001%&>?d"))
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
			IL_0A5F:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			goto IL_0B57;
			IL_0B27:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			goto IL_0B57;
			IL_0D79:
			num = array[num2++];
			goto IL_0CC3;
			IL_0D96:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
				goto IL_012F;
			}
			goto IL_012F;
			IL_0DE6:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 3;
				goto IL_01F7;
			}
			goto IL_01F7;
			IL_0E36:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 4;
				goto IL_02AA;
			}
			goto IL_02AA;
			IL_0F03:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 10;
				goto IL_0448;
			}
			goto IL_0448;
			IL_0F53:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 11;
				goto IL_0523;
			}
			goto IL_0523;
			IL_0FA3:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 12;
				goto IL_05C0;
			}
			goto IL_05C0;
			IL_10F4:
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
				goto IL_0B27;
			}
			goto IL_0B27;
			IL_1155:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 19;
				goto IL_09D3;
			}
			goto IL_09D3;
			IL_11A5:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 20;
				goto IL_0A5F;
			}
			goto IL_0A5F;
			Block_80:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 21;
				goto IL_0B27;
			}
			goto IL_0B27;
			IL_1260:
			num = array[num2++];
			int num7 = array2[num3++];
			array[--num2] = 24;
			goto IL_0C2A;
			Block_82:
			if (num5 > 0)
			{
				array[--num2] = num5 - 1;
				array[--num2] = num;
				array[--num2] = 25;
				goto IL_0C5D;
			}
			goto IL_0C5D;
			IL_012F:
			if (num < runtextend && runtext[num++] == '@')
			{
				array2[--num3] = -1;
				array[--num2] = 1;
				goto IL_0BB7;
			}
			goto IL_0CCC;
			IL_016B:
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
			IL_01F7:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || !RegexRunner.CharInClass(runtext[num++], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
			{
				goto IL_0CCC;
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
			IL_02AA:
			array2[--num3] = this.runtrack.Length - num2;
			array2[--num3] = this.Crawlpos();
			array[--num2] = 5;
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || !RegexRunner.CharInClass(runtext[num++], "\u0001\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
			{
				goto IL_0CCC;
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
			IL_0448:
			array2[--num3] = num;
			array[--num2] = 1;
			if (num >= runtextend || runtext[num++] != '=')
			{
				goto IL_0CCC;
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
			IL_0523:
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0CCC;
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
			IL_05C0:
			num4 = array2[num3++];
			this.Capture(5, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			if (num >= runtextend || runtext[num++] != '"')
			{
				goto IL_0CCC;
			}
			IL_0B57:
			num4 = array2[num3++];
			this.Capture(2, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 8;
			IL_0BB7:
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
					goto IL_0CCC;
				}
				goto IL_016B;
			}
			else
			{
				array[--num2] = 24;
			}
			IL_0C2A:
			if ((num4 = runtextend - num) > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num;
				array[--num2] = 25;
			}
			IL_0C5D:
			if (2 > runtextend - num || runtext[num] != '%' || runtext[num + 1] != '>')
			{
				goto IL_0CCC;
			}
			num += 2;
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 8;
			IL_0CC3:
			this.runtextpos = num;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00004C5C File Offset: 0x00003C5C
		public override bool FindFirstChar()
		{
			if (this.runtextpos > this.runtextstart)
			{
				this.runtextpos = this.runtextend;
				return false;
			}
			return true;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00004C88 File Offset: 0x00003C88
		public override void InitTrackCount()
		{
			this.runtrackcount = 51;
		}
	}
}
