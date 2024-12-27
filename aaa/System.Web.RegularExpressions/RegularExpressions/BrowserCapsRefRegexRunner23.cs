using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000044 RID: 68
	internal class BrowserCapsRefRegexRunner23 : RegexRunner
	{
		// Token: 0x0600009B RID: 155 RVA: 0x0000E7D0 File Offset: 0x0000D7D0
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
			if (2 <= runtextend - num && runtext[num] == '$' && runtext[num + 1] == '{')
			{
				num += 2;
				array2[--num3] = num;
				array[--num2] = 1;
				if (1 <= runtextend - num)
				{
					num++;
					num4 = 1;
					while (RegexRunner.CharInClass(runtext[num - num4--], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
					{
						if (num4 <= 0)
						{
							int num5;
							num4 = (num5 = runtextend - num) + 1;
							while (--num4 > 0)
							{
								if (!RegexRunner.CharInClass(runtext[num++], "\0\0\t\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
								goto IL_017B;
							}
							goto IL_017B;
						}
					}
				}
			}
			for (;;)
			{
				IL_0203:
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
					goto IL_0258;
				case 1:
					num3++;
					break;
				case 2:
					goto IL_0275;
				case 3:
					array2[--num3] = array[num2++];
					this.Uncapture();
					break;
				}
			}
			IL_0258:
			num = array[num2++];
			goto IL_01FA;
			IL_0275:
			num = array[num2++];
			num4 = array[num2++];
			if (num4 > 0)
			{
				array[--num2] = num4 - 1;
				array[--num2] = num - 1;
				array[--num2] = 2;
			}
			IL_017B:
			num4 = array2[num3++];
			this.Capture(1, num4, num);
			array[--num2] = num4;
			array[--num2] = 3;
			if (num >= runtextend || runtext[num++] != '}')
			{
				goto IL_0203;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 3;
			IL_01FA:
			this.runtextpos = num;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000EAC0 File Offset: 0x0000DAC0
		public override bool FindFirstChar()
		{
			string runtext = this.runtext;
			int runtextend = this.runtextend;
			int num2;
			for (int i = this.runtextpos + 1; i < runtextend; i = num2 + i)
			{
				int num;
				if ((num = (int)runtext[i]) != 123)
				{
					if ((num -= 36) <= 87)
					{
						switch (num)
						{
						default:
							num2 = 1;
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
							num2 = 2;
							break;
						case 24:
							num2 = 2;
							break;
						case 25:
							num2 = 2;
							break;
						case 26:
							num2 = 2;
							break;
						case 27:
							num2 = 2;
							break;
						case 28:
							num2 = 2;
							break;
						case 29:
							num2 = 2;
							break;
						case 30:
							num2 = 2;
							break;
						case 31:
							num2 = 2;
							break;
						case 32:
							num2 = 2;
							break;
						case 33:
							num2 = 2;
							break;
						case 34:
							num2 = 2;
							break;
						case 35:
							num2 = 2;
							break;
						case 36:
							num2 = 2;
							break;
						case 37:
							num2 = 2;
							break;
						case 38:
							num2 = 2;
							break;
						case 39:
							num2 = 2;
							break;
						case 40:
							num2 = 2;
							break;
						case 41:
							num2 = 2;
							break;
						case 42:
							num2 = 2;
							break;
						case 43:
							num2 = 2;
							break;
						case 44:
							num2 = 2;
							break;
						case 45:
							num2 = 2;
							break;
						case 46:
							num2 = 2;
							break;
						case 47:
							num2 = 2;
							break;
						case 48:
							num2 = 2;
							break;
						case 49:
							num2 = 2;
							break;
						case 50:
							num2 = 2;
							break;
						case 51:
							num2 = 2;
							break;
						case 52:
							num2 = 2;
							break;
						case 53:
							num2 = 2;
							break;
						case 54:
							num2 = 2;
							break;
						case 55:
							num2 = 2;
							break;
						case 56:
							num2 = 2;
							break;
						case 57:
							num2 = 2;
							break;
						case 58:
							num2 = 2;
							break;
						case 59:
							num2 = 2;
							break;
						case 60:
							num2 = 2;
							break;
						case 61:
							num2 = 2;
							break;
						case 62:
							num2 = 2;
							break;
						case 63:
							num2 = 2;
							break;
						case 64:
							num2 = 2;
							break;
						case 65:
							num2 = 2;
							break;
						case 66:
							num2 = 2;
							break;
						case 67:
							num2 = 2;
							break;
						case 68:
							num2 = 2;
							break;
						case 69:
							num2 = 2;
							break;
						case 70:
							num2 = 2;
							break;
						case 71:
							num2 = 2;
							break;
						case 72:
							num2 = 2;
							break;
						case 73:
							num2 = 2;
							break;
						case 74:
							num2 = 2;
							break;
						case 75:
							num2 = 2;
							break;
						case 76:
							num2 = 2;
							break;
						case 77:
							num2 = 2;
							break;
						case 78:
							num2 = 2;
							break;
						case 79:
							num2 = 2;
							break;
						case 80:
							num2 = 2;
							break;
						case 81:
							num2 = 2;
							break;
						case 82:
							num2 = 2;
							break;
						case 83:
							num2 = 2;
							break;
						case 84:
							num2 = 2;
							break;
						case 85:
							num2 = 2;
							break;
						case 86:
							num2 = 2;
							break;
						case 87:
							num2 = 0;
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
					if (runtext[--num] == '$')
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

		// Token: 0x0600009D RID: 157 RVA: 0x0000EF20 File Offset: 0x0000DF20
		public override void InitTrackCount()
		{
			this.runtrackcount = 6;
		}
	}
}
