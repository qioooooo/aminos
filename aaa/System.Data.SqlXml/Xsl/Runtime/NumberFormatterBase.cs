using System;
using System.Text;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007A RID: 122
	internal class NumberFormatterBase
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x00025548 File Offset: 0x00024548
		public static void ConvertToAlphabetic(StringBuilder sb, double val, char firstChar, int totalChars)
		{
			char[] array = new char[7];
			int num = 7;
			int i;
			int num2;
			for (i = (int)val; i > totalChars; i = num2)
			{
				num2 = --i / totalChars;
				array[--num] = (char)((int)firstChar + (i - num2 * totalChars));
			}
			array[--num] = (char)((int)firstChar + (i - 1));
			sb.Append(array, num, 7 - num);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002559C File Offset: 0x0002459C
		public static void ConvertToRoman(StringBuilder sb, double val, bool upperCase)
		{
			int i = (int)val;
			string text = (upperCase ? "IIVIXXLXCCDCM" : "iivixxlxccdcm");
			int num = NumberFormatterBase.RomanDigitValue.Length;
			while (num-- != 0)
			{
				while (i >= NumberFormatterBase.RomanDigitValue[num])
				{
					i -= NumberFormatterBase.RomanDigitValue[num];
					sb.Append(text, num, 1 + (num & 1));
				}
			}
		}

		// Token: 0x04000489 RID: 1161
		protected const int MaxAlphabeticValue = 2147483647;

		// Token: 0x0400048A RID: 1162
		private const int MaxAlphabeticLength = 7;

		// Token: 0x0400048B RID: 1163
		protected const int MaxRomanValue = 32767;

		// Token: 0x0400048C RID: 1164
		private const string RomanDigitsUC = "IIVIXXLXCCDCM";

		// Token: 0x0400048D RID: 1165
		private const string RomanDigitsLC = "iivixxlxccdcm";

		// Token: 0x0400048E RID: 1166
		private const string hiraganaAiueo = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";

		// Token: 0x0400048F RID: 1167
		private const string hiraganaIroha = "いろはにほへとちりぬるをわかよたれそつねならむうゐのおくやまけふこえてあさきゆめみしゑひもせす";

		// Token: 0x04000490 RID: 1168
		private const string katakanaAiueo = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン";

		// Token: 0x04000491 RID: 1169
		private const string katakanaIroha = "イロハニホヘトチリヌルヲワカヨタレソツネナラムウヰノオクヤマケフコエテアサキユメミシヱヒモセスン";

		// Token: 0x04000492 RID: 1170
		private const string katakanaAiueoHw = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝ";

		// Token: 0x04000493 RID: 1171
		private const string katakanaIrohaHw = "ｲﾛﾊﾆﾎﾍﾄﾁﾘﾇﾙｦﾜｶﾖﾀﾚｿﾂﾈﾅﾗﾑｳヰﾉｵｸﾔﾏｹﾌｺｴﾃｱｻｷﾕﾒﾐｼヱﾋﾓｾｽﾝ";

		// Token: 0x04000494 RID: 1172
		private const string cjkIdeographic = "〇一二三四五六七八九";

		// Token: 0x04000495 RID: 1173
		private static readonly int[] RomanDigitValue = new int[]
		{
			1, 4, 5, 9, 10, 40, 50, 90, 100, 400,
			500, 900, 1000
		};
	}
}
