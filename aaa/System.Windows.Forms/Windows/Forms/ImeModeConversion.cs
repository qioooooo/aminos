using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	// Token: 0x020002B0 RID: 688
	public struct ImeModeConversion
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000588D8 File Offset: 0x000578D8
		internal static ImeMode[] ChineseTable
		{
			get
			{
				return ImeModeConversion.chineseTable;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000588DF File Offset: 0x000578DF
		internal static ImeMode[] JapaneseTable
		{
			get
			{
				return ImeModeConversion.japaneseTable;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060025C5 RID: 9669 RVA: 0x000588E6 File Offset: 0x000578E6
		internal static ImeMode[] KoreanTable
		{
			get
			{
				return ImeModeConversion.koreanTable;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000588ED File Offset: 0x000578ED
		internal static ImeMode[] UnsupportedTable
		{
			get
			{
				return ImeModeConversion.unsupportedTable;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060025C7 RID: 9671 RVA: 0x000588F4 File Offset: 0x000578F4
		internal static ImeMode[] InputLanguageTable
		{
			get
			{
				InputLanguage currentInputLanguage = InputLanguage.CurrentInputLanguage;
				int num = (int)((long)currentInputLanguage.Handle & 65535L);
				int num2 = num;
				if (num2 <= 2052)
				{
					if (num2 != 1028)
					{
						switch (num2)
						{
						case 1041:
							return ImeModeConversion.japaneseTable;
						case 1042:
							goto IL_007A;
						default:
							if (num2 != 2052)
							{
								goto IL_0086;
							}
							break;
						}
					}
				}
				else if (num2 <= 3076)
				{
					if (num2 == 2066)
					{
						goto IL_007A;
					}
					if (num2 != 3076)
					{
						goto IL_0086;
					}
				}
				else if (num2 != 4100 && num2 != 5124)
				{
					goto IL_0086;
				}
				return ImeModeConversion.chineseTable;
				IL_007A:
				return ImeModeConversion.koreanTable;
				IL_0086:
				return ImeModeConversion.unsupportedTable;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060025C8 RID: 9672 RVA: 0x0005898C File Offset: 0x0005798C
		public static Dictionary<ImeMode, ImeModeConversion> ImeModeConversionBits
		{
			get
			{
				if (ImeModeConversion.imeModeConversionBits == null)
				{
					ImeModeConversion.imeModeConversionBits = new Dictionary<ImeMode, ImeModeConversion>(7);
					ImeModeConversion imeModeConversion;
					imeModeConversion.setBits = 9;
					imeModeConversion.clearBits = 2;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Hiragana, imeModeConversion);
					imeModeConversion.setBits = 11;
					imeModeConversion.clearBits = 0;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Katakana, imeModeConversion);
					imeModeConversion.setBits = 3;
					imeModeConversion.clearBits = 8;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.KatakanaHalf, imeModeConversion);
					imeModeConversion.setBits = 8;
					imeModeConversion.clearBits = 3;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.AlphaFull, imeModeConversion);
					imeModeConversion.setBits = 0;
					imeModeConversion.clearBits = 11;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Alpha, imeModeConversion);
					imeModeConversion.setBits = 9;
					imeModeConversion.clearBits = 0;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.HangulFull, imeModeConversion);
					imeModeConversion.setBits = 1;
					imeModeConversion.clearBits = 8;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Hangul, imeModeConversion);
					imeModeConversion.setBits = 1;
					imeModeConversion.clearBits = 10;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.OnHalf, imeModeConversion);
				}
				return ImeModeConversion.imeModeConversionBits;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x00058A9B File Offset: 0x00057A9B
		public static bool IsCurrentConversionTableSupported
		{
			get
			{
				return ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable;
			}
		}

		// Token: 0x040015F5 RID: 5621
		internal const int ImeDisabled = 1;

		// Token: 0x040015F6 RID: 5622
		internal const int ImeDirectInput = 2;

		// Token: 0x040015F7 RID: 5623
		internal const int ImeClosed = 3;

		// Token: 0x040015F8 RID: 5624
		internal const int ImeNativeInput = 4;

		// Token: 0x040015F9 RID: 5625
		internal const int ImeNativeFullHiragana = 4;

		// Token: 0x040015FA RID: 5626
		internal const int ImeNativeHalfHiragana = 5;

		// Token: 0x040015FB RID: 5627
		internal const int ImeNativeFullKatakana = 6;

		// Token: 0x040015FC RID: 5628
		internal const int ImeNativeHalfKatakana = 7;

		// Token: 0x040015FD RID: 5629
		internal const int ImeAlphaFull = 8;

		// Token: 0x040015FE RID: 5630
		internal const int ImeAlphaHalf = 9;

		// Token: 0x040015FF RID: 5631
		private static Dictionary<ImeMode, ImeModeConversion> imeModeConversionBits;

		// Token: 0x04001600 RID: 5632
		internal int setBits;

		// Token: 0x04001601 RID: 5633
		internal int clearBits;

		// Token: 0x04001602 RID: 5634
		private static ImeMode[] japaneseTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Off,
			ImeMode.Off,
			ImeMode.Hiragana,
			ImeMode.Hiragana,
			ImeMode.Katakana,
			ImeMode.KatakanaHalf,
			ImeMode.AlphaFull,
			ImeMode.Alpha
		};

		// Token: 0x04001603 RID: 5635
		private static ImeMode[] koreanTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Alpha,
			ImeMode.Alpha,
			ImeMode.HangulFull,
			ImeMode.Hangul,
			ImeMode.HangulFull,
			ImeMode.Hangul,
			ImeMode.AlphaFull,
			ImeMode.Alpha
		};

		// Token: 0x04001604 RID: 5636
		private static ImeMode[] chineseTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Off,
			ImeMode.Close,
			ImeMode.On,
			ImeMode.OnHalf,
			ImeMode.On,
			ImeMode.OnHalf,
			ImeMode.Off,
			ImeMode.Off
		};

		// Token: 0x04001605 RID: 5637
		private static ImeMode[] unsupportedTable = new ImeMode[0];
	}
}
