using System;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003A7 RID: 935
	public sealed class IdnMapping
	{
		// Token: 0x06002608 RID: 9736 RVA: 0x0006D536 File Offset: 0x0006C536
		public IdnMapping()
		{
			this.m_bAllowUnassigned = false;
			this.m_bUseStd3AsciiRules = false;
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x0006D54C File Offset: 0x0006C54C
		// (set) Token: 0x0600260A RID: 9738 RVA: 0x0006D554 File Offset: 0x0006C554
		public bool AllowUnassigned
		{
			get
			{
				return this.m_bAllowUnassigned;
			}
			set
			{
				this.m_bAllowUnassigned = value;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x0006D55D File Offset: 0x0006C55D
		// (set) Token: 0x0600260C RID: 9740 RVA: 0x0006D565 File Offset: 0x0006C565
		public bool UseStd3AsciiRules
		{
			get
			{
				return this.m_bUseStd3AsciiRules;
			}
			set
			{
				this.m_bUseStd3AsciiRules = value;
			}
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0006D56E File Offset: 0x0006C56E
		public string GetAscii(string unicode)
		{
			return this.GetAscii(unicode, 0);
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0006D578 File Offset: 0x0006C578
		public string GetAscii(string unicode, int index)
		{
			if (unicode == null)
			{
				throw new ArgumentNullException("unicode");
			}
			return this.GetAscii(unicode, index, unicode.Length - index);
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0006D598 File Offset: 0x0006C598
		public string GetAscii(string unicode, int index, int count)
		{
			if (unicode == null)
			{
				throw new ArgumentNullException("unicode");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (index > unicode.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index > unicode.Length - count)
			{
				throw new ArgumentOutOfRangeException("unicode", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			unicode = unicode.Substring(index, count);
			if (this.ValidateStd3AndAscii(unicode, this.UseStd3AsciiRules, true))
			{
				return unicode;
			}
			if (unicode[unicode.Length - 1] <= '\u001f')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequence", new object[] { unicode.Length - 1 }), "unicode");
			}
			bool flag = unicode.Length > 0 && this.IsDot(unicode[unicode.Length - 1]);
			unicode = unicode.Normalize(this.m_bAllowUnassigned ? ((NormalizationForm)13) : ((NormalizationForm)269));
			if (!flag && unicode.Length > 0 && this.IsDot(unicode[unicode.Length - 1]))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
			}
			if (this.UseStd3AsciiRules)
			{
				this.ValidateStd3AndAscii(unicode, true, false);
			}
			return this.punycode_encode(unicode);
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0006D6FE File Offset: 0x0006C6FE
		public string GetUnicode(string ascii)
		{
			return this.GetUnicode(ascii, 0);
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0006D708 File Offset: 0x0006C708
		public string GetUnicode(string ascii, int index)
		{
			if (ascii == null)
			{
				throw new ArgumentNullException("unicode");
			}
			return this.GetUnicode(ascii, index, ascii.Length - index);
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0006D728 File Offset: 0x0006C728
		public string GetUnicode(string ascii, int index, int count)
		{
			if (ascii == null)
			{
				throw new ArgumentNullException("ascii");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (index > ascii.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index > ascii.Length - count)
			{
				throw new ArgumentOutOfRangeException("ascii", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			ascii = ascii.Substring(index, count);
			string text = this.punycode_decode(ascii);
			if (!ascii.Equals(this.GetAscii(text), StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnIllegalName"), "ascii");
			}
			return text;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0006D7E4 File Offset: 0x0006C7E4
		public override bool Equals(object obj)
		{
			IdnMapping idnMapping = obj as IdnMapping;
			return idnMapping != null && this.m_bAllowUnassigned == idnMapping.m_bAllowUnassigned && this.m_bUseStd3AsciiRules == idnMapping.m_bUseStd3AsciiRules;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0006D81B File Offset: 0x0006C81B
		public override int GetHashCode()
		{
			return (this.m_bAllowUnassigned ? 100 : 200) + (this.m_bUseStd3AsciiRules ? 1000 : 2000);
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0006D843 File Offset: 0x0006C843
		private bool IsSupplementary(int cTest)
		{
			return cTest >= 65536;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0006D850 File Offset: 0x0006C850
		private bool IsDot(char c)
		{
			return c == '.' || c == '。' || c == '．' || c == '｡';
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0006D874 File Offset: 0x0006C874
		private bool ValidateStd3AndAscii(string unicode, bool bUseStd3, bool bCheckAscii)
		{
			int num = -1;
			if (unicode.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
			}
			for (int i = 0; i < unicode.Length; i++)
			{
				if (unicode[i] <= '\u001f')
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequence", new object[] { i }), "unicode");
				}
				if (bCheckAscii && unicode[i] >= '\u007f')
				{
					return false;
				}
				if (this.IsDot(unicode[i]))
				{
					if (i == num + 1)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
					}
					if (i - num > 64)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "Unicode");
					}
					if (bUseStd3 && i > 0)
					{
						this.ValidateStd3(unicode[i - 1], true);
					}
					num = i;
				}
				else if (bUseStd3)
				{
					this.ValidateStd3(unicode[i], i == num + 1);
				}
			}
			if (num == -1 && unicode.Length > 63)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
			}
			if (unicode.Length > 255 - (this.IsDot(unicode[unicode.Length - 1]) ? 0 : 1))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadNameSize", new object[] { 255 - (this.IsDot(unicode[unicode.Length - 1]) ? 0 : 1) }), "unicode");
			}
			if (bUseStd3 && !this.IsDot(unicode[unicode.Length - 1]))
			{
				this.ValidateStd3(unicode[unicode.Length - 1], true);
			}
			return true;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0006DA30 File Offset: 0x0006CA30
		private void ValidateStd3(char c, bool bNextToDot)
		{
			if (c <= ',' || c == '/' || (c >= ':' && c <= '@') || (c >= '[' && c <= '`') || (c >= '{' && c <= '\u007f') || (c == '-' && bNextToDot))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadStd3", new object[] { c }), "Unicode");
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0006DA93 File Offset: 0x0006CA93
		private static bool HasUpperCaseFlag(char punychar)
		{
			return punychar >= 'A' && punychar <= 'Z';
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0006DAA4 File Offset: 0x0006CAA4
		private static bool basic(uint cp)
		{
			return cp < 128U;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0006DAB0 File Offset: 0x0006CAB0
		private static int decode_digit(char cp)
		{
			if (cp >= '0' && cp <= '9')
			{
				return (int)(cp - '0' + '\u001a');
			}
			if (cp >= 'a' && cp <= 'z')
			{
				return (int)(cp - 'a');
			}
			if (cp >= 'A' && cp <= 'Z')
			{
				return (int)(cp - 'A');
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0006DB01 File Offset: 0x0006CB01
		private static char encode_digit(int d)
		{
			if (d > 25)
			{
				return (char)(d - 26 + 48);
			}
			return (char)(d + 97);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0006DB16 File Offset: 0x0006CB16
		private static char encode_basic(char bcp)
		{
			if (IdnMapping.HasUpperCaseFlag(bcp))
			{
				bcp += ' ';
			}
			return bcp;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0006DB28 File Offset: 0x0006CB28
		private static int adapt(int delta, int numpoints, bool firsttime)
		{
			delta = (firsttime ? (delta / 700) : (delta / 2));
			delta += delta / numpoints;
			uint num = 0U;
			while (delta > 455)
			{
				delta /= 35;
				num += 36U;
			}
			return (int)((ulong)num + (ulong)((long)(36 * delta / (delta + 38))));
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0006DB74 File Offset: 0x0006CB74
		private string punycode_encode(string unicode)
		{
			StringBuilder stringBuilder = new StringBuilder(unicode.Length);
			int i = 0;
			int num = 0;
			int num2 = 0;
			if (unicode.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
			}
			while (i < unicode.Length)
			{
				i = unicode.IndexOfAny(IdnMapping.M_Dots, num);
				if (i < 0)
				{
					i = unicode.Length;
				}
				if (i == num)
				{
					if (i != unicode.Length)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
					}
					break;
				}
				else
				{
					stringBuilder.Append("xn--");
					bool flag = false;
					BidiCategory bidiCategory = CharUnicodeInfo.GetBidiCategory(unicode, num);
					if (bidiCategory == BidiCategory.RightToLeft || bidiCategory == BidiCategory.RightToLeftArabic)
					{
						flag = true;
						int num3 = i - 1;
						if (char.IsLowSurrogate(unicode, num3))
						{
							num3--;
						}
						bidiCategory = CharUnicodeInfo.GetBidiCategory(unicode, num3);
						if (bidiCategory != BidiCategory.RightToLeft && bidiCategory != BidiCategory.RightToLeftArabic)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadBidi"), "unicode");
						}
					}
					int j = 0;
					for (int k = num; k < i; k++)
					{
						BidiCategory bidiCategory2 = CharUnicodeInfo.GetBidiCategory(unicode, k);
						if (flag && bidiCategory2 == BidiCategory.LeftToRight)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadBidi"), "unicode");
						}
						if (!flag && (bidiCategory2 == BidiCategory.RightToLeft || bidiCategory2 == BidiCategory.RightToLeftArabic))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadBidi"), "unicode");
						}
						if (IdnMapping.basic((uint)unicode[k]))
						{
							stringBuilder.Append(IdnMapping.encode_basic(unicode[k]));
							j++;
						}
						else if (char.IsSurrogatePair(unicode, k))
						{
							k++;
						}
					}
					int num4 = j;
					if (num4 == i - num)
					{
						stringBuilder.Remove(num2, "xn--".Length);
					}
					else
					{
						if (unicode.Length - num >= "xn--".Length && unicode.Substring(num, "xn--".Length).Equals("xn--", StringComparison.OrdinalIgnoreCase))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "unicode");
						}
						int num5 = 0;
						if (num4 > 0)
						{
							stringBuilder.Append('-');
						}
						int num6 = 128;
						int num7 = 0;
						int num8 = 72;
						while (j < i - num)
						{
							int num9 = 134217727;
							int num10;
							for (int l = num; l < i; l += (this.IsSupplementary(num10) ? 2 : 1))
							{
								num10 = char.ConvertToUtf32(unicode, l);
								if (num10 >= num6 && num10 < num9)
								{
									num9 = num10;
								}
							}
							num7 += (num9 - num6) * (j - num5 + 1);
							num6 = num9;
							for (int l = num; l < i; l += (this.IsSupplementary(num10) ? 2 : 1))
							{
								num10 = char.ConvertToUtf32(unicode, l);
								if (num10 < num6)
								{
									num7++;
								}
								if (num10 == num6)
								{
									int num11 = num7;
									int num12 = 36;
									for (;;)
									{
										int num13 = ((num12 <= num8) ? 1 : ((num12 >= num8 + 26) ? 26 : (num12 - num8)));
										if (num11 < num13)
										{
											break;
										}
										stringBuilder.Append(IdnMapping.encode_digit(num13 + (num11 - num13) % (36 - num13)));
										num11 = (num11 - num13) / (36 - num13);
										num12 += 36;
									}
									stringBuilder.Append(IdnMapping.encode_digit(num11));
									num8 = IdnMapping.adapt(num7, j - num5 + 1, j == num4);
									num7 = 0;
									j++;
									if (this.IsSupplementary(num9))
									{
										j++;
										num5++;
									}
								}
							}
							num7++;
							num6++;
						}
					}
					if (stringBuilder.Length - num2 > 63)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "unicode");
					}
					if (i != unicode.Length)
					{
						stringBuilder.Append('.');
					}
					num = i + 1;
					num2 = stringBuilder.Length;
				}
			}
			if (stringBuilder.Length > 255 - (this.IsDot(unicode[unicode.Length - 1]) ? 0 : 1))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadNameSize", new object[] { 255 - (this.IsDot(unicode[unicode.Length - 1]) ? 0 : 1) }), "unicode");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0006DF8C File Offset: 0x0006CF8C
		private string punycode_decode(string ascii)
		{
			if (ascii.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "ascii");
			}
			if (ascii.Length > 255 - (this.IsDot(ascii[ascii.Length - 1]) ? 0 : 1))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadNameSize", new object[] { 255 - (this.IsDot(ascii[ascii.Length - 1]) ? 0 : 1) }), "ascii");
			}
			StringBuilder stringBuilder = new StringBuilder(ascii.Length);
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < ascii.Length)
			{
				i = ascii.IndexOf('.', num);
				if (i < 0 || i > ascii.Length)
				{
					i = ascii.Length;
				}
				if (i == num)
				{
					if (i != ascii.Length)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "ascii");
					}
					break;
				}
				else
				{
					if (i - num > 63)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "ascii");
					}
					if (ascii.Length < "xn--".Length + num || !ascii.Substring(num, "xn--".Length).Equals("xn--", StringComparison.OrdinalIgnoreCase))
					{
						stringBuilder.Append(ascii.Substring(num, i - num));
					}
					else
					{
						num += "xn--".Length;
						int num3 = ascii.LastIndexOf('-', i - 1);
						if (num3 == i - 1)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
						}
						int num4;
						if (num3 <= num)
						{
							num4 = 0;
						}
						else
						{
							num4 = num3 - num;
							for (int j = num; j < num + num4; j++)
							{
								if (ascii[j] > '\u007f')
								{
									throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
								}
								stringBuilder.Append((ascii[j] >= 'A' && ascii[j] <= 'Z') ? (ascii[j] - 'A' + 'a') : ascii[j]);
							}
						}
						int k = num + ((num4 > 0) ? (num4 + 1) : 0);
						int num5 = 128;
						int num6 = 72;
						int num7 = 0;
						int num8 = 0;
						IL_0415:
						while (k < i)
						{
							int num9 = num7;
							int num10 = 1;
							int num11 = 36;
							while (k < i)
							{
								int num12 = IdnMapping.decode_digit(ascii[k++]);
								if (num12 > (134217727 - num7) / num10)
								{
									throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
								}
								num7 += num12 * num10;
								int num13 = ((num11 <= num6) ? 1 : ((num11 >= num6 + 26) ? 26 : (num11 - num6)));
								if (num12 >= num13)
								{
									if (num10 > 134217727 / (36 - num13))
									{
										throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
									}
									num10 *= 36 - num13;
									num11 += 36;
								}
								else
								{
									num6 = IdnMapping.adapt(num7 - num9, stringBuilder.Length - num2 - num8 + 1, num9 == 0);
									if (num7 / (stringBuilder.Length - num2 - num8 + 1) > 134217727 - num5)
									{
										throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
									}
									num5 += num7 / (stringBuilder.Length - num2 - num8 + 1);
									num7 %= stringBuilder.Length - num2 - num8 + 1;
									if (num5 < 0 || num5 > 1114111 || (num5 >= 55296 && num5 <= 57343))
									{
										throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
									}
									string text = char.ConvertFromUtf32(num5);
									int num14;
									if (num8 > 0)
									{
										int l = num7;
										num14 = num2;
										while (l > 0)
										{
											if (num14 >= stringBuilder.Length)
											{
												throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
											}
											if (char.IsSurrogate(stringBuilder[num14]))
											{
												num14++;
											}
											l--;
											num14++;
										}
									}
									else
									{
										num14 = num2 + num7;
									}
									stringBuilder.Insert(num14, text);
									if (this.IsSupplementary(num5))
									{
										num8++;
									}
									num7++;
									goto IL_0415;
								}
							}
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadPunycode"), "ascii");
						}
						bool flag = false;
						BidiCategory bidiCategory = CharUnicodeInfo.GetBidiCategory(stringBuilder.ToString(), num2);
						if (bidiCategory == BidiCategory.RightToLeft || bidiCategory == BidiCategory.RightToLeftArabic)
						{
							flag = true;
						}
						for (int m = num2; m < stringBuilder.Length; m++)
						{
							if (!char.IsLowSurrogate(stringBuilder.ToString(), m))
							{
								bidiCategory = CharUnicodeInfo.GetBidiCategory(stringBuilder.ToString(), m);
								if ((flag && bidiCategory == BidiCategory.LeftToRight) || (!flag && (bidiCategory == BidiCategory.RightToLeft || bidiCategory == BidiCategory.RightToLeftArabic)))
								{
									throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadBidi"), "ascii");
								}
							}
						}
						if (flag && bidiCategory != BidiCategory.RightToLeft && bidiCategory != BidiCategory.RightToLeftArabic)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadBidi"), "ascii");
						}
					}
					if (i - num > 63)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadLabelSize"), "ascii");
					}
					if (i != ascii.Length)
					{
						stringBuilder.Append('.');
					}
					num = i + 1;
					num2 = stringBuilder.Length;
				}
			}
			if (stringBuilder.Length > 255 - (this.IsDot(stringBuilder[stringBuilder.Length - 1]) ? 0 : 1))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IdnBadNameSize", new object[] { 255 - (this.IsDot(stringBuilder[stringBuilder.Length - 1]) ? 0 : 1) }), "ascii");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001135 RID: 4405
		private const int M_labelLimit = 63;

		// Token: 0x04001136 RID: 4406
		private const int M_defaultNameLimit = 255;

		// Token: 0x04001137 RID: 4407
		private const string M_strAcePrefix = "xn--";

		// Token: 0x04001138 RID: 4408
		private const int punycodeBase = 36;

		// Token: 0x04001139 RID: 4409
		private const int tmin = 1;

		// Token: 0x0400113A RID: 4410
		private const int tmax = 26;

		// Token: 0x0400113B RID: 4411
		private const int skew = 38;

		// Token: 0x0400113C RID: 4412
		private const int damp = 700;

		// Token: 0x0400113D RID: 4413
		private const int initial_bias = 72;

		// Token: 0x0400113E RID: 4414
		private const int initial_n = 128;

		// Token: 0x0400113F RID: 4415
		private const char delimiter = '-';

		// Token: 0x04001140 RID: 4416
		private const int maxint = 134217727;

		// Token: 0x04001141 RID: 4417
		private static char[] M_Dots = new char[] { '.', '。', '．', '｡' };

		// Token: 0x04001142 RID: 4418
		private bool m_bAllowUnassigned;

		// Token: 0x04001143 RID: 4419
		private bool m_bUseStd3AsciiRules;
	}
}
