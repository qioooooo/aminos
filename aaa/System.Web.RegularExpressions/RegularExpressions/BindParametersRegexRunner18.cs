using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000035 RID: 53
	internal class BindParametersRegexRunner18 : RegexRunner
	{
		// Token: 0x06000078 RID: 120 RVA: 0x0000B7DC File Offset: 0x0000A7DC
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
			int num4 = (num5 = runtextend - num) + 1;
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
			}
			for (;;)
			{
				array2[--num3] = num;
				array[--num2] = 1;
				array[--num2] = num;
				array[--num2] = 3;
				array2[--num3] = num;
				array[--num2] = 1;
				if (num < runtextend && runtext[num++] == '"')
				{
					array2[--num3] = num;
					array[--num2] = 1;
					array2[--num3] = num;
					array[--num2] = 1;
					array[--num2] = num;
					array[--num2] = 4;
					array2[--num3] = num;
					array[--num2] = 1;
					if (1 <= runtextend - num)
					{
						num++;
						num4 = 1;
						while (RegexRunner.CharInClass(runtext[num - num4--], "\0\u0002\t./\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
						{
							if (num4 <= 0)
							{
								num4 = (num5 = runtextend - num) + 1;
								while (--num4 > 0)
								{
									if (!RegexRunner.CharInClass(runtext[num++], "\0\u0002\t./\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
									goto IL_0268;
								}
								goto IL_0268;
							}
						}
					}
				}
				for (;;)
				{
					IL_0E4C:
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
						goto IL_0EE9;
					case 1:
						num3++;
						continue;
					case 2:
						goto IL_0F06;
					case 3:
						num = array[num2++];
						array2[--num3] = num;
						array[--num2] = 1;
						if (num >= runtextend || runtext[num++] != '\'')
						{
							continue;
						}
						array2[--num3] = num;
						array[--num2] = 1;
						array2[--num3] = num;
						array[--num2] = 1;
						array[--num2] = num;
						array[--num2] = 8;
						array2[--num3] = num;
						array[--num2] = 1;
						if (1 <= runtextend - num)
						{
							num++;
							num4 = 1;
							while (RegexRunner.CharInClass(runtext[num - num4--], "\0\u0002\t./\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
							{
								if (num4 <= 0)
								{
									num4 = (num5 = runtextend - num) + 1;
									while (--num4 > 0)
									{
										if (!RegexRunner.CharInClass(runtext[num++], "\0\u0002\t./\0\u0002\u0004\u0005\u0003\u0001\t\u0013\0"))
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
										goto IL_05D8;
									}
									goto IL_05D8;
								}
							}
							continue;
						}
						continue;
					case 4:
						num = array[num2++];
						array2[--num3] = num;
						array[--num2] = 1;
						if (num < runtextend && runtext[num++] == '[' && 1 <= runtextend - num)
						{
							num++;
							num4 = 1;
							while (RegexRunner.CharInClass(runtext[num - num4--], "\0\u0001\0\0"))
							{
								if (num4 <= 0)
								{
									num4 = (num5 = runtextend - num) + 1;
									while (--num4 > 0)
									{
										if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
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
										goto IL_0389;
									}
									goto IL_0389;
								}
							}
							continue;
						}
						continue;
					case 5:
						goto IL_0F78;
					case 6:
						array2[--num3] = array[num2++];
						this.Uncapture();
						continue;
					case 7:
						num = array[num2++];
						num4 = array[num2++];
						if (num4 > 0)
						{
							array[--num2] = num4 - 1;
							array[--num2] = num - 1;
							array[--num2] = 7;
						}
						break;
					case 8:
						num = array[num2++];
						array2[--num3] = num;
						array[--num2] = 1;
						if (num < runtextend && runtext[num++] == '[' && 1 <= runtextend - num)
						{
							num++;
							num4 = 1;
							while (RegexRunner.CharInClass(runtext[num - num4--], "\0\u0001\0\0"))
							{
								if (num4 <= 0)
								{
									num4 = (num5 = runtextend - num) + 1;
									while (--num4 > 0)
									{
										if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
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
										goto IL_06F9;
									}
									goto IL_06F9;
								}
							}
							continue;
						}
						continue;
					case 9:
						num = array[num2++];
						num4 = array[num2++];
						if (num4 > 0)
						{
							array[--num2] = num4 - 1;
							array[--num2] = num - 1;
							array[--num2] = 9;
							goto IL_05D8;
						}
						goto IL_05D8;
					case 10:
						num = array[num2++];
						num4 = array[num2++];
						if (num4 > 0)
						{
							array[--num2] = num4 - 1;
							array[--num2] = num - 1;
							array[--num2] = 10;
							goto IL_06F9;
						}
						goto IL_06F9;
					case 11:
						goto IL_10E8;
					case 12:
						num3 += 2;
						continue;
					case 13:
						goto IL_1144;
					case 14:
						num = array[num2++];
						array2[--num3] = num;
						array[--num2] = 1;
						if (num >= runtextend || runtext[num++] != '\'')
						{
							continue;
						}
						array2[--num3] = num;
						array[--num2] = 1;
						num4 = (num5 = runtextend - num) + 1;
						while (--num4 > 0)
						{
							if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
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
							goto IL_0BA9;
						}
						goto IL_0BA9;
					case 15:
						goto IL_11A5;
					case 16:
						num = array[num2++];
						num4 = array[num2++];
						if (num4 > 0)
						{
							array[--num2] = num4 - 1;
							array[--num2] = num - 1;
							array[--num2] = 16;
							goto IL_0BA9;
						}
						goto IL_0BA9;
					case 17:
						goto IL_1245;
					case 18:
						if ((num4 = array2[num3++] - 1) >= 0)
						{
							goto Block_86;
						}
						array2[num3] = array[num2++];
						array2[--num3] = num4;
						continue;
					case 19:
						goto IL_08C4;
					case 20:
						num4 = array[num2++];
						array2[--num3] = array[num2++];
						array2[--num3] = num4;
						continue;
					case 21:
						goto IL_1323;
					}
					IL_0389:
					if (num < runtextend && runtext[num++] == ']')
					{
						goto Block_21;
					}
					continue;
					IL_06F9:
					if (num >= runtextend || runtext[num++] != ']')
					{
						continue;
					}
					num4 = array2[num3++];
					this.Capture(9, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					IL_0748:
					num4 = array2[num3++];
					this.Capture(7, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					num4 = array2[num3++];
					this.Capture(14, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					if (num < runtextend && runtext[num++] == '\'')
					{
						goto Block_43;
					}
					continue;
					IL_05D8:
					num4 = array2[num3++];
					this.Capture(8, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					goto IL_0748;
					IL_0BA9:
					num4 = array2[num3++];
					this.Capture(15, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					if (num < runtextend && runtext[num++] == '\'')
					{
						goto Block_65;
					}
				}
				IL_0F06:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 2;
					continue;
				}
				continue;
				Block_21:
				num4 = array2[num3++];
				this.Capture(5, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				goto IL_03D8;
				IL_0F78:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 5;
					goto IL_0268;
				}
				goto IL_0268;
				IL_1245:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 17;
					goto IL_0CCC;
				}
				goto IL_0CCC;
				Block_65:
				num4 = array2[num3++];
				this.Capture(13, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				goto IL_0C28;
				IL_10E8:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 11;
					goto IL_089B;
				}
				goto IL_089B;
				Block_43:
				num4 = array2[num3++];
				this.Capture(6, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				goto IL_07F7;
				IL_1144:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 13;
					goto IL_096F;
				}
				goto IL_096F;
				IL_11A5:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 15;
					goto IL_0A62;
				}
				goto IL_0A62;
				IL_1323:
				num = array[num2++];
				num4 = array[num2++];
				if (num4 > 0)
				{
					array[--num2] = num4 - 1;
					array[--num2] = num - 1;
					array[--num2] = 21;
					goto IL_0E0A;
				}
				goto IL_0E0A;
				Block_86:
				num = array2[num3++];
				array[--num2] = num4;
				array[--num2] = 20;
				goto IL_0D96;
				IL_03D8:
				num4 = array2[num3++];
				this.Capture(3, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				num4 = array2[num3++];
				this.Capture(14, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				if (num < runtextend && runtext[num++] == '"')
				{
					num4 = array2[num3++];
					this.Capture(2, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					goto IL_07F7;
				}
				goto IL_0E4C;
				IL_0268:
				num4 = array2[num3++];
				this.Capture(4, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				goto IL_03D8;
				IL_08C4:
				array2[--num3] = num;
				array[--num2] = 1;
				if (num >= runtextend || runtext[num++] != ',')
				{
					goto IL_0E4C;
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
					array[--num2] = 13;
					goto IL_096F;
				}
				goto IL_096F;
				IL_0CFC:
				num4 = array2[num3++];
				int num6 = (num5 = array2[num3++]);
				array[--num2] = num5;
				if ((num6 == num && num4 >= 0) || num4 >= 1)
				{
					array[--num2] = num4;
					array[--num2] = 20;
					goto IL_0D96;
				}
				array2[--num3] = num;
				array2[--num3] = num4 + 1;
				array[--num2] = 18;
				if (num2 <= 236 || num3 <= 177)
				{
					array[--num2] = 19;
					goto IL_0E4C;
				}
				goto IL_08C4;
				IL_0CCC:
				num4 = array2[num3++];
				this.Capture(10, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				goto IL_0CFC;
				IL_0C28:
				num4 = array2[num3++];
				this.Capture(11, num4, num);
				array[--num2] = num4;
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
					array[--num2] = 17;
					goto IL_0CCC;
				}
				goto IL_0CCC;
				IL_089B:
				array2[--num3] = -1;
				array2[--num3] = 0;
				array[--num2] = 12;
				goto IL_0CFC;
				IL_07F7:
				num4 = array2[num3++];
				this.Capture(1, num4, num);
				array[--num2] = num4;
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
					array[--num2] = 11;
					goto IL_089B;
				}
				goto IL_089B;
				IL_096F:
				array2[--num3] = num;
				array[--num2] = 1;
				array[--num2] = num;
				array[--num2] = 14;
				array2[--num3] = num;
				array[--num2] = 1;
				if (num >= runtextend || runtext[num++] != '"')
				{
					goto IL_0E4C;
				}
				array2[--num3] = num;
				array[--num2] = 1;
				num4 = (num5 = runtextend - num) + 1;
				while (--num4 > 0)
				{
					if (!RegexRunner.CharInClass(runtext[num++], "\0\u0001\0\0"))
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
				IL_0A62:
				num4 = array2[num3++];
				this.Capture(15, num4, num);
				array[--num2] = num4;
				array[--num2] = 6;
				if (num < runtextend && runtext[num++] == '"')
				{
					num4 = array2[num3++];
					this.Capture(12, num4, num);
					array[--num2] = num4;
					array[--num2] = 6;
					goto IL_0C28;
				}
				goto IL_0E4C;
				IL_0E0A:
				if (num >= runtextend)
				{
					break;
				}
				goto IL_0E4C;
				IL_0D96:
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
					array[--num2] = 21;
					goto IL_0E0A;
				}
				goto IL_0E0A;
			}
			num4 = array2[num3++];
			this.Capture(0, num4, num);
			array[--num2] = num4;
			array[num2 - 1] = 6;
			IL_0E43:
			this.runtextpos = num;
			return;
			IL_0EE9:
			num = array[num2++];
			goto IL_0E43;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000CB5C File Offset: 0x0000BB5C
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
					if (RegexRunner.CharInClass(runtext[num++], "\0\u0004\u0001\"#'(d"))
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

		// Token: 0x0600007A RID: 122 RVA: 0x0000CBD8 File Offset: 0x0000BBD8
		public override void InitTrackCount()
		{
			this.runtrackcount = 59;
		}
	}
}
