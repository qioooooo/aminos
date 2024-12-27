using System;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003CE RID: 974
	internal class HebrewNumber
	{
		// Token: 0x060028A6 RID: 10406 RVA: 0x0007E2D5 File Offset: 0x0007D2D5
		private HebrewNumber()
		{
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0007E2E0 File Offset: 0x0007D2E0
		internal static string ToString(int Number)
		{
			char c = '\0';
			StringBuilder stringBuilder = new StringBuilder();
			if (Number > 5000)
			{
				Number -= 5000;
			}
			int num = Number / 100;
			if (num > 0)
			{
				Number -= num * 100;
				for (int i = 0; i < num / 4; i++)
				{
					stringBuilder.Append('ת');
				}
				int num2 = num % 4;
				if (num2 > 0)
				{
					stringBuilder.Append((char)(1510 + num2));
				}
			}
			int num3 = Number / 10;
			Number %= 10;
			switch (num3)
			{
			case 0:
				c = '\0';
				break;
			case 1:
				c = 'י';
				break;
			case 2:
				c = 'כ';
				break;
			case 3:
				c = 'ל';
				break;
			case 4:
				c = 'מ';
				break;
			case 5:
				c = 'נ';
				break;
			case 6:
				c = 'ס';
				break;
			case 7:
				c = 'ע';
				break;
			case 8:
				c = 'פ';
				break;
			case 9:
				c = 'צ';
				break;
			}
			char c2 = (char)((Number > 0) ? (1488 + Number - 1) : 0);
			if (c2 == 'ה' && c == 'י')
			{
				c2 = 'ו';
				c = 'ט';
			}
			if (c2 == 'ו' && c == 'י')
			{
				c2 = 'ז';
				c = 'ט';
			}
			if (c != '\0')
			{
				stringBuilder.Append(c);
			}
			if (c2 != '\0')
			{
				stringBuilder.Append(c2);
			}
			if (stringBuilder.Length > 1)
			{
				stringBuilder.Insert(stringBuilder.Length - 1, '"');
			}
			else
			{
				stringBuilder.Append('\'');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0007E470 File Offset: 0x0007D470
		internal static HebrewNumberParsingState ParseByChar(char ch, ref HebrewNumberParsingContext context)
		{
			HebrewNumber.HebrewToken hebrewToken;
			if (ch == '\'')
			{
				hebrewToken = HebrewNumber.HebrewToken.SingleQuote;
			}
			else if (ch == '"')
			{
				hebrewToken = HebrewNumber.HebrewToken.DoubleQuote;
			}
			else
			{
				int num = (int)(ch - 'א');
				if (num < 0 || num >= HebrewNumber.HebrewValues.Length)
				{
					return HebrewNumberParsingState.NotHebrewDigit;
				}
				hebrewToken = HebrewNumber.HebrewValues[num].token;
				if (hebrewToken == HebrewNumber.HebrewToken.Invalid)
				{
					return HebrewNumberParsingState.NotHebrewDigit;
				}
				context.result += HebrewNumber.HebrewValues[num].value;
			}
			context.state = HebrewNumber.m_numberPasingState[(int)context.state][(int)hebrewToken];
			if (context.state == HebrewNumber.HS._err)
			{
				return HebrewNumberParsingState.InvalidHebrewNumber;
			}
			if (context.state == HebrewNumber.HS.END)
			{
				return HebrewNumberParsingState.FoundEndOfHebrewNumber;
			}
			return HebrewNumberParsingState.ContinueParsing;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0007E502 File Offset: 0x0007D502
		internal static bool IsDigit(char ch)
		{
			if (ch >= 'א' && ch <= HebrewNumber.maxHebrewNumberCh)
			{
				return HebrewNumber.HebrewValues[(int)(ch - 'א')].value >= 0;
			}
			return ch == '\'' || ch == '"';
		}

		// Token: 0x0400137C RID: 4988
		private const int minHebrewNumberCh = 1488;

		// Token: 0x0400137D RID: 4989
		private static HebrewNumber.HebrewValue[] HebrewValues = new HebrewNumber.HebrewValue[]
		{
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 2),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 3),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 4),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 5),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit6_7, 6),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit6_7, 7),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit1, 8),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit9, 9),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 10),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Invalid, -1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 20),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 30),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Invalid, -1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 40),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Invalid, -1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 50),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 60),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 70),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Invalid, -1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 80),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Invalid, -1),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit10, 90),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit100, 100),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit200_300, 200),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit200_300, 300),
			new HebrewNumber.HebrewValue(HebrewNumber.HebrewToken.Digit400, 400)
		};

		// Token: 0x0400137E RID: 4990
		private static char maxHebrewNumberCh = (char)(1488 + HebrewNumber.HebrewValues.Length - 1);

		// Token: 0x0400137F RID: 4991
		private static HebrewNumber.HS[][] m_numberPasingState = new HebrewNumber.HS[][]
		{
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS.S400,
				HebrewNumber.HS.X00,
				HebrewNumber.HS.X00,
				HebrewNumber.HS.X0,
				HebrewNumber.HS.X,
				HebrewNumber.HS.X,
				HebrewNumber.HS.X,
				HebrewNumber.HS.S9,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS.S400_400,
				HebrewNumber.HS.S400_X00,
				HebrewNumber.HS.S400_X00,
				HebrewNumber.HS.S400_X0,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_S9,
				HebrewNumber.HS.END,
				HebrewNumber.HS.S400_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S400_400_100,
				HebrewNumber.HS.S400_X0,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_S9,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S400_400_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S400_X00_X0,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_S9,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X0_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X0_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.X0_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S400_X0,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_S9,
				HebrewNumber.HS.END,
				HebrewNumber.HS.X00_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S400_X00_X0,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_S9,
				HebrewNumber.HS._err,
				HebrewNumber.HS.X00_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.S9_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.S9_DQ
			},
			new HebrewNumber.HS[]
			{
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS.END,
				HebrewNumber.HS.END,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err,
				HebrewNumber.HS._err
			}
		};

		// Token: 0x020003CF RID: 975
		private enum HebrewToken
		{
			// Token: 0x04001381 RID: 4993
			Invalid = -1,
			// Token: 0x04001382 RID: 4994
			Digit400,
			// Token: 0x04001383 RID: 4995
			Digit200_300,
			// Token: 0x04001384 RID: 4996
			Digit100,
			// Token: 0x04001385 RID: 4997
			Digit10,
			// Token: 0x04001386 RID: 4998
			Digit1,
			// Token: 0x04001387 RID: 4999
			Digit6_7,
			// Token: 0x04001388 RID: 5000
			Digit7,
			// Token: 0x04001389 RID: 5001
			Digit9,
			// Token: 0x0400138A RID: 5002
			SingleQuote,
			// Token: 0x0400138B RID: 5003
			DoubleQuote
		}

		// Token: 0x020003D0 RID: 976
		private class HebrewValue
		{
			// Token: 0x060028AB RID: 10411 RVA: 0x0007EB1E File Offset: 0x0007DB1E
			internal HebrewValue(HebrewNumber.HebrewToken token, int value)
			{
				this.token = token;
				this.value = value;
			}

			// Token: 0x0400138C RID: 5004
			internal HebrewNumber.HebrewToken token;

			// Token: 0x0400138D RID: 5005
			internal int value;
		}

		// Token: 0x020003D1 RID: 977
		internal enum HS
		{
			// Token: 0x0400138F RID: 5007
			_err = -1,
			// Token: 0x04001390 RID: 5008
			Start,
			// Token: 0x04001391 RID: 5009
			S400,
			// Token: 0x04001392 RID: 5010
			S400_400,
			// Token: 0x04001393 RID: 5011
			S400_X00,
			// Token: 0x04001394 RID: 5012
			S400_X0,
			// Token: 0x04001395 RID: 5013
			X00_DQ,
			// Token: 0x04001396 RID: 5014
			S400_X00_X0,
			// Token: 0x04001397 RID: 5015
			X0_DQ,
			// Token: 0x04001398 RID: 5016
			X,
			// Token: 0x04001399 RID: 5017
			X0,
			// Token: 0x0400139A RID: 5018
			X00,
			// Token: 0x0400139B RID: 5019
			S400_DQ,
			// Token: 0x0400139C RID: 5020
			S400_400_DQ,
			// Token: 0x0400139D RID: 5021
			S400_400_100,
			// Token: 0x0400139E RID: 5022
			S9,
			// Token: 0x0400139F RID: 5023
			X00_S9,
			// Token: 0x040013A0 RID: 5024
			S9_DQ,
			// Token: 0x040013A1 RID: 5025
			END = 100
		}
	}
}
