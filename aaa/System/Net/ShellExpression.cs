using System;

namespace System.Net
{
	// Token: 0x0200053A RID: 1338
	internal struct ShellExpression
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000A9DB3 File Offset: 0x000A8DB3
		internal ShellExpression(string pattern)
		{
			this.pattern = null;
			this.match = null;
			this.Parse(pattern);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000A9DCC File Offset: 0x000A8DCC
		internal bool IsMatch(string target)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			for (;;)
			{
				if (!flag)
				{
					if (num2 > target.Length)
					{
						return flag2;
					}
					switch (this.pattern[num])
					{
					case ShellExpression.ShExpTokens.End:
						if (num2 == target.Length)
						{
							goto Block_10;
						}
						flag = true;
						break;
					case ShellExpression.ShExpTokens.Start:
						if (num2 != 0)
						{
							return flag2;
						}
						this.match[num++] = 0;
						break;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
						if (num2 == target.Length || target[num2] == '.')
						{
							this.match[num++] = num2;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedAsterisk:
						if (num2 == target.Length || target[num2] == '.')
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedDot:
						if (num2 == target.Length)
						{
							this.match[num++] = num2;
						}
						else if (target[num2] == '.')
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					case ShellExpression.ShExpTokens.Question:
						if (num2 == target.Length)
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.Asterisk:
						num2 = (this.match[num++] = target.Length);
						break;
					default:
						if (num2 < target.Length && this.pattern[num] == (ShellExpression.ShExpTokens)char.ToLowerInvariant(target[num2]))
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					}
				}
				else
				{
					switch (this.pattern[--num])
					{
					case ShellExpression.ShExpTokens.End:
					case ShellExpression.ShExpTokens.Start:
						return flag2;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
					case ShellExpression.ShExpTokens.Asterisk:
						if (this.match[num] != this.match[num - 1])
						{
							num2 = --this.match[num++];
							flag = false;
						}
						break;
					}
				}
			}
			Block_10:
			flag2 = true;
			return flag2;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000A9FF4 File Offset: 0x000A8FF4
		private void Parse(string patString)
		{
			this.pattern = new ShellExpression.ShExpTokens[patString.Length + 2];
			this.match = null;
			int num = 0;
			this.pattern[num++] = ShellExpression.ShExpTokens.Start;
			for (int i = 0; i < patString.Length; i++)
			{
				char c = patString[i];
				if (c != '*')
				{
					if (c != '?')
					{
						if (c != '^')
						{
							this.pattern[num++] = (ShellExpression.ShExpTokens)char.ToLowerInvariant(patString[i]);
						}
						else
						{
							if (i >= patString.Length - 1)
							{
								this.pattern = null;
								if (Logging.On)
								{
									Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[] { patString }));
								}
								throw new FormatException(SR.GetString("net_format_shexp", new object[] { patString }));
							}
							i++;
							char c2 = patString[i];
							if (c2 != '*')
							{
								if (c2 != '.')
								{
									if (c2 != '?')
									{
										this.pattern = null;
										if (Logging.On)
										{
											Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[] { patString }));
										}
										throw new FormatException(SR.GetString("net_format_shexp", new object[] { patString }));
									}
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedQuestion;
								}
								else
								{
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedDot;
								}
							}
							else
							{
								this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedAsterisk;
							}
						}
					}
					else
					{
						this.pattern[num++] = ShellExpression.ShExpTokens.Question;
					}
				}
				else
				{
					this.pattern[num++] = ShellExpression.ShExpTokens.Asterisk;
				}
			}
			this.pattern[num++] = ShellExpression.ShExpTokens.End;
			this.match = new int[num];
		}

		// Token: 0x040027BF RID: 10175
		private ShellExpression.ShExpTokens[] pattern;

		// Token: 0x040027C0 RID: 10176
		private int[] match;

		// Token: 0x0200053B RID: 1339
		private enum ShExpTokens
		{
			// Token: 0x040027C2 RID: 10178
			Asterisk = -1,
			// Token: 0x040027C3 RID: 10179
			Question = -2,
			// Token: 0x040027C4 RID: 10180
			AugmentedDot = -3,
			// Token: 0x040027C5 RID: 10181
			AugmentedAsterisk = -4,
			// Token: 0x040027C6 RID: 10182
			AugmentedQuestion = -5,
			// Token: 0x040027C7 RID: 10183
			Start = -6,
			// Token: 0x040027C8 RID: 10184
			End = -7
		}
	}
}
