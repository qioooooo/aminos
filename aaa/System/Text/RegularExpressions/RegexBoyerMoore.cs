﻿using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200000F RID: 15
	internal sealed class RegexBoyerMoore
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00003294 File Offset: 0x00002294
		internal RegexBoyerMoore(string pattern, bool caseInsensitive, bool rightToLeft, CultureInfo culture)
		{
			if (caseInsensitive)
			{
				StringBuilder stringBuilder = new StringBuilder(pattern.Length);
				for (int i = 0; i < pattern.Length; i++)
				{
					stringBuilder.Append(char.ToLower(pattern[i], culture));
				}
				pattern = stringBuilder.ToString();
			}
			this._pattern = pattern;
			this._rightToLeft = rightToLeft;
			this._caseInsensitive = caseInsensitive;
			this._culture = culture;
			int num;
			int num2;
			int num3;
			if (!rightToLeft)
			{
				num = -1;
				num2 = pattern.Length - 1;
				num3 = 1;
			}
			else
			{
				num = pattern.Length;
				num2 = 0;
				num3 = -1;
			}
			this._positive = new int[pattern.Length];
			int num4 = num2;
			char c = pattern[num4];
			this._positive[num4] = num3;
			num4 -= num3;
			while (num4 != num)
			{
				if (pattern[num4] != c)
				{
					num4 -= num3;
				}
				else
				{
					int num5 = num2;
					int num6 = num4;
					while (num6 != num && pattern[num5] == pattern[num6])
					{
						num6 -= num3;
						num5 -= num3;
					}
					if (this._positive[num5] == 0)
					{
						this._positive[num5] = num5 - num6;
					}
					num4 -= num3;
				}
			}
			for (int num5 = num2 - num3; num5 != num; num5 -= num3)
			{
				if (this._positive[num5] == 0)
				{
					this._positive[num5] = num3;
				}
			}
			this._negativeASCII = new int[128];
			for (int j = 0; j < 128; j++)
			{
				this._negativeASCII[j] = num2 - num;
			}
			this._lowASCII = 127;
			this._highASCII = 0;
			for (num4 = num2; num4 != num; num4 -= num3)
			{
				c = pattern[num4];
				if (c < '\u0080')
				{
					if (this._lowASCII > (int)c)
					{
						this._lowASCII = (int)c;
					}
					if (this._highASCII < (int)c)
					{
						this._highASCII = (int)c;
					}
					if (this._negativeASCII[(int)c] == num2 - num)
					{
						this._negativeASCII[(int)c] = num2 - num4;
					}
				}
				else
				{
					int num7 = (int)(c >> 8);
					int num8 = (int)(c & 'ÿ');
					if (this._negativeUnicode == null)
					{
						this._negativeUnicode = new int[256][];
					}
					if (this._negativeUnicode[num7] == null)
					{
						int[] array = new int[256];
						for (int k = 0; k < 256; k++)
						{
							array[k] = num2 - num;
						}
						if (num7 == 0)
						{
							Array.Copy(this._negativeASCII, array, 128);
							this._negativeASCII = array;
						}
						this._negativeUnicode[num7] = array;
					}
					if (this._negativeUnicode[num7][num8] == num2 - num)
					{
						this._negativeUnicode[num7][num8] = num2 - num4;
					}
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003524 File Offset: 0x00002524
		private bool MatchPattern(string text, int index)
		{
			if (!this._caseInsensitive)
			{
				return 0 == string.CompareOrdinal(this._pattern, 0, text, index, this._pattern.Length);
			}
			if (text.Length - index < this._pattern.Length)
			{
				return false;
			}
			TextInfo textInfo = this._culture.TextInfo;
			for (int i = 0; i < this._pattern.Length; i++)
			{
				if (textInfo.ToLower(text[index + i]) != this._pattern[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000035B0 File Offset: 0x000025B0
		internal bool IsMatch(string text, int index, int beglimit, int endlimit)
		{
			if (!this._rightToLeft)
			{
				return index >= beglimit && endlimit - index >= this._pattern.Length && this.MatchPattern(text, index);
			}
			return index <= endlimit && index - beglimit >= this._pattern.Length && this.MatchPattern(text, index - this._pattern.Length);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003610 File Offset: 0x00002610
		internal int Scan(string text, int index, int beglimit, int endlimit)
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			if (!this._rightToLeft)
			{
				num = this._pattern.Length;
				num2 = this._pattern.Length - 1;
				num3 = 0;
				num4 = index + num - 1;
				num5 = 1;
			}
			else
			{
				num = -this._pattern.Length;
				num2 = 0;
				num3 = -num - 1;
				num4 = index + num;
				num5 = -1;
			}
			char c = this._pattern[num2];
			IL_005F:
			while (num4 < endlimit && num4 >= beglimit)
			{
				char c2 = text[num4];
				if (this._caseInsensitive)
				{
					c2 = char.ToLower(c2, this._culture);
				}
				if (c2 != c)
				{
					int num6;
					int[] array;
					if (c2 < '\u0080')
					{
						num6 = this._negativeASCII[(int)c2];
					}
					else if (this._negativeUnicode != null && (array = this._negativeUnicode[(int)(c2 >> 8)]) != null)
					{
						num6 = array[(int)(c2 & 'ÿ')];
					}
					else
					{
						num6 = num;
					}
					num4 += num6;
				}
				else
				{
					int num7 = num4;
					int num8 = num2;
					while (num8 != num3)
					{
						num8 -= num5;
						num7 -= num5;
						c2 = text[num7];
						if (this._caseInsensitive)
						{
							c2 = char.ToLower(c2, this._culture);
						}
						if (c2 != this._pattern[num8])
						{
							int num6 = this._positive[num8];
							if ((c2 & 'ﾀ') == '\0')
							{
								num7 = num8 - num2 + this._negativeASCII[(int)c2];
							}
							else
							{
								int[] array;
								if (this._negativeUnicode == null || (array = this._negativeUnicode[(int)(c2 >> 8)]) == null)
								{
									num4 += num6;
									goto IL_005F;
								}
								num7 = num8 - num2 + array[(int)(c2 & 'ÿ')];
							}
							if (this._rightToLeft ? (num7 < num6) : (num7 > num6))
							{
								num6 = num7;
							}
							num4 += num6;
							goto IL_005F;
						}
					}
					if (!this._rightToLeft)
					{
						return num7;
					}
					return num7 + 1;
				}
			}
			return -1;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000037C0 File Offset: 0x000027C0
		public override string ToString()
		{
			return this._pattern;
		}

		// Token: 0x04000642 RID: 1602
		internal const int infinite = 2147483647;

		// Token: 0x04000643 RID: 1603
		internal int[] _positive;

		// Token: 0x04000644 RID: 1604
		internal int[] _negativeASCII;

		// Token: 0x04000645 RID: 1605
		internal int[][] _negativeUnicode;

		// Token: 0x04000646 RID: 1606
		internal string _pattern;

		// Token: 0x04000647 RID: 1607
		internal int _lowASCII;

		// Token: 0x04000648 RID: 1608
		internal int _highASCII;

		// Token: 0x04000649 RID: 1609
		internal bool _rightToLeft;

		// Token: 0x0400064A RID: 1610
		internal bool _caseInsensitive;

		// Token: 0x0400064B RID: 1611
		internal CultureInfo _culture;
	}
}
