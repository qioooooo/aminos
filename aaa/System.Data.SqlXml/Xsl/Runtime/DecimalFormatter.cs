using System;
using System.Globalization;
using System.Text;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000074 RID: 116
	internal class DecimalFormatter
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x00024CC8 File Offset: 0x00023CC8
		public DecimalFormatter(string formatPicture, DecimalFormat decimalFormat)
		{
			if (formatPicture.Length == 0)
			{
				throw XsltException.Create("Xslt_InvalidFormat", new string[0]);
			}
			this.zeroDigit = decimalFormat.zeroDigit;
			this.posFormatInfo = (NumberFormatInfo)decimalFormat.info.Clone();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			char c = this.posFormatInfo.NumberDecimalSeparator[0];
			char c2 = this.posFormatInfo.NumberGroupSeparator[0];
			char c3 = this.posFormatInfo.PercentSymbol[0];
			char c4 = this.posFormatInfo.PerMilleSymbol[0];
			int num = 0;
			int num2 = -1;
			int num3 = -1;
			int num4;
			for (int i = 0; i < formatPicture.Length; i++)
			{
				char c5 = formatPicture[i];
				if (c5 == decimalFormat.digit)
				{
					if (flag3 && flag)
					{
						throw XsltException.Create("Xslt_InvalidFormat1", new string[] { formatPicture });
					}
					num3 = stringBuilder.Length;
					flag6 = (flag4 = true);
					stringBuilder.Append('#');
				}
				else if (c5 == decimalFormat.zeroDigit)
				{
					if (flag4 && !flag)
					{
						throw XsltException.Create("Xslt_InvalidFormat2", new string[] { formatPicture });
					}
					num3 = stringBuilder.Length;
					flag6 = (flag3 = true);
					stringBuilder.Append('0');
				}
				else if (c5 == decimalFormat.patternSeparator)
				{
					if (!flag6)
					{
						throw XsltException.Create("Xslt_InvalidFormat8", new string[0]);
					}
					if (flag2)
					{
						throw XsltException.Create("Xslt_InvalidFormat3", new string[] { formatPicture });
					}
					flag2 = true;
					if (num2 < 0)
					{
						num2 = num3 + 1;
					}
					num4 = DecimalFormatter.RemoveTrailingComma(stringBuilder, num, num2);
					if (num4 > 9)
					{
						num4 = 0;
					}
					this.posFormatInfo.NumberGroupSizes = new int[] { num4 };
					if (!flag5)
					{
						this.posFormatInfo.NumberDecimalDigits = 0;
					}
					this.posFormat = stringBuilder.ToString();
					stringBuilder.Length = 0;
					num2 = -1;
					num3 = -1;
					num = 0;
					flag3 = (flag4 = (flag6 = false));
					flag5 = false;
					flag = true;
					this.negFormatInfo = (NumberFormatInfo)decimalFormat.info.Clone();
					this.negFormatInfo.NegativeSign = string.Empty;
				}
				else if (c5 == c)
				{
					if (flag5)
					{
						throw XsltException.Create("Xslt_InvalidFormat5", new string[] { formatPicture });
					}
					num2 = stringBuilder.Length;
					flag5 = true;
					flag3 = (flag4 = (flag = false));
					stringBuilder.Append('.');
				}
				else if (c5 == c2)
				{
					num = stringBuilder.Length;
					num3 = num;
					stringBuilder.Append(',');
				}
				else if (c5 == c3)
				{
					stringBuilder.Append('%');
				}
				else if (c5 == c4)
				{
					stringBuilder.Append('‰');
				}
				else if (c5 == '\'')
				{
					int num5 = formatPicture.IndexOf('\'', i + 1);
					if (num5 < 0)
					{
						num5 = formatPicture.Length - 1;
					}
					stringBuilder.Append(formatPicture, i, num5 - i + 1);
					i = num5;
				}
				else
				{
					if ((('0' <= c5 && c5 <= '9') || c5 == '\a') && decimalFormat.zeroDigit != '0')
					{
						stringBuilder.Append('\a');
					}
					if ("0#.,%‰Ee\\'\";".IndexOf(c5) >= 0)
					{
						stringBuilder.Append('\\');
					}
					stringBuilder.Append(c5);
				}
			}
			if (!flag6)
			{
				throw XsltException.Create("Xslt_InvalidFormat8", new string[0]);
			}
			NumberFormatInfo numberFormatInfo = (flag2 ? this.negFormatInfo : this.posFormatInfo);
			if (num2 < 0)
			{
				num2 = num3 + 1;
			}
			num4 = DecimalFormatter.RemoveTrailingComma(stringBuilder, num, num2);
			if (num4 > 9)
			{
				num4 = 0;
			}
			numberFormatInfo.NumberGroupSizes = new int[] { num4 };
			if (!flag5)
			{
				numberFormatInfo.NumberDecimalDigits = 0;
			}
			if (flag2)
			{
				this.negFormat = stringBuilder.ToString();
				return;
			}
			this.posFormat = stringBuilder.ToString();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000250A5 File Offset: 0x000240A5
		private static int RemoveTrailingComma(StringBuilder builder, int commaIndex, int decimalIndex)
		{
			if (commaIndex > 0 && commaIndex == decimalIndex - 1)
			{
				builder.Remove(decimalIndex - 1, 1);
			}
			else if (decimalIndex > commaIndex)
			{
				return decimalIndex - commaIndex - 1;
			}
			return 0;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000250CC File Offset: 0x000240CC
		public string Format(double value)
		{
			NumberFormatInfo numberFormatInfo;
			string text;
			if (value < 0.0 && this.negFormatInfo != null)
			{
				numberFormatInfo = this.negFormatInfo;
				text = this.negFormat;
			}
			else
			{
				numberFormatInfo = this.posFormatInfo;
				text = this.posFormat;
			}
			string text2 = value.ToString(text, numberFormatInfo);
			if (this.zeroDigit != '0')
			{
				StringBuilder stringBuilder = new StringBuilder(text2.Length);
				int num = (int)(this.zeroDigit - '0');
				for (int i = 0; i < text2.Length; i++)
				{
					char c = text2[i];
					if (c - '0' <= '\t')
					{
						c += (char)num;
					}
					else if (c == '\a')
					{
						c = text2[++i];
					}
					stringBuilder.Append(c);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002518E File Offset: 0x0002418E
		public static string Format(double value, string formatPicture, DecimalFormat decimalFormat)
		{
			return new DecimalFormatter(formatPicture, decimalFormat).Format(value);
		}

		// Token: 0x04000456 RID: 1110
		private const string ClrSpecialChars = "0#.,%‰Ee\\'\";";

		// Token: 0x04000457 RID: 1111
		private const char EscChar = '\a';

		// Token: 0x04000458 RID: 1112
		private NumberFormatInfo posFormatInfo;

		// Token: 0x04000459 RID: 1113
		private NumberFormatInfo negFormatInfo;

		// Token: 0x0400045A RID: 1114
		private string posFormat;

		// Token: 0x0400045B RID: 1115
		private string negFormat;

		// Token: 0x0400045C RID: 1116
		private char zeroDigit;
	}
}
