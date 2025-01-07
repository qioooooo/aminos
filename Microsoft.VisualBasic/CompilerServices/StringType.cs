using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class StringType
	{
		private StringType()
		{
		}

		public static string FromBoolean(bool Value)
		{
			if (Value)
			{
				return bool.TrueString;
			}
			return bool.FalseString;
		}

		public static string FromByte(byte Value)
		{
			return Value.ToString(null, null);
		}

		public static string FromChar(char Value)
		{
			return Value.ToString();
		}

		public static string FromShort(short Value)
		{
			return Value.ToString(null, null);
		}

		public static string FromInteger(int Value)
		{
			return Value.ToString(null, null);
		}

		public static string FromLong(long Value)
		{
			return Value.ToString(null, null);
		}

		public static string FromSingle(float Value)
		{
			return StringType.FromSingle(Value, null);
		}

		public static string FromDouble(double Value)
		{
			return StringType.FromDouble(Value, null);
		}

		public static string FromSingle(float Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString(null, NumberFormat);
		}

		public static string FromDouble(double Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		public static string FromDate(DateTime Value)
		{
			long ticks = Value.TimeOfDay.Ticks;
			if (ticks == Value.Ticks || (Value.Year == 1899 && Value.Month == 12 && Value.Day == 30))
			{
				return Value.ToString("T", null);
			}
			if (ticks == 0L)
			{
				return Value.ToString("d", null);
			}
			return Value.ToString("G", null);
		}

		public static string FromDecimal(decimal Value)
		{
			return StringType.FromDecimal(Value, null);
		}

		public static string FromDecimal(decimal Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		public static string FromObject(object Value)
		{
			if (Value == null)
			{
				return null;
			}
			string text = Value as string;
			if (text != null)
			{
				return text;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return StringType.FromBoolean(convertible.ToBoolean(null));
				case TypeCode.Char:
					return StringType.FromChar(convertible.ToChar(null));
				case TypeCode.Byte:
					return StringType.FromByte(convertible.ToByte(null));
				case TypeCode.Int16:
					return StringType.FromShort(convertible.ToInt16(null));
				case TypeCode.Int32:
					return StringType.FromInteger(convertible.ToInt32(null));
				case TypeCode.Int64:
					return StringType.FromLong(convertible.ToInt64(null));
				case TypeCode.Single:
					return StringType.FromSingle(convertible.ToSingle(null));
				case TypeCode.Double:
					return StringType.FromDouble(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return StringType.FromDecimal(convertible.ToDecimal(null));
				case TypeCode.DateTime:
					return StringType.FromDate(convertible.ToDateTime(null));
				case TypeCode.String:
					return convertible.ToString(null);
				}
			}
			else
			{
				char[] array = Value as char[];
				if (array != null && array.Rank == 1)
				{
					return new string(CharArrayType.FromObject(Value));
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"String"
			}));
		}

		public static int StrCmp(string sLeft, string sRight, bool TextCompare)
		{
			if (sLeft == sRight)
			{
				return 0;
			}
			if (sLeft == null)
			{
				if (sRight.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else if (sRight == null)
			{
				if (sLeft.Length == 0)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (TextCompare)
				{
					return Utils.GetCultureInfo().CompareInfo.Compare(sLeft, sRight, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
				}
				return string.CompareOrdinal(sLeft, sRight);
			}
		}

		public static bool StrLike(string Source, string Pattern, CompareMethod CompareOption)
		{
			if (CompareOption == CompareMethod.Binary)
			{
				return StringType.StrLikeBinary(Source, Pattern);
			}
			return StringType.StrLikeText(Source, Pattern);
		}

		public static bool StrLikeBinary(string Source, string Pattern)
		{
			bool flag = false;
			int num;
			if (Pattern == null)
			{
				num = 0;
			}
			else
			{
				num = Pattern.Length;
			}
			int num2;
			if (Source == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = Source.Length;
			}
			int num3;
			char c;
			if (num3 < num2)
			{
				c = Source[num3];
			}
			checked
			{
				int i;
				bool flag2;
				while (i < num)
				{
					char c2 = Pattern[i];
					bool flag3;
					bool flag4;
					bool flag5;
					if (c2 == '*' && !flag2)
					{
						int num4 = StringType.AsteriskSkip(Pattern.Substring(i + 1), Source.Substring(num3), num2 - num3, CompareMethod.Binary, Strings.m_InvariantCompareInfo);
						if (num4 < 0)
						{
							return false;
						}
						if (num4 > 0)
						{
							num3 += num4;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
					}
					else if (c2 == '?' && !flag2)
					{
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '#' && !flag2)
					{
						if (!char.IsDigit(c))
						{
							break;
						}
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '-' && flag2 && flag3 && !flag && !flag4 && (i + 1 >= num || Pattern[i + 1] != ']'))
					{
						flag4 = true;
					}
					else if (c2 == '!' && flag2 && !flag5)
					{
						flag5 = true;
						bool flag6 = true;
					}
					else if (c2 == '[' && !flag2)
					{
						flag2 = true;
						char c3 = '\0';
						flag3 = false;
					}
					else if (c2 == ']' && flag2)
					{
						flag2 = false;
						bool flag6;
						if (flag3)
						{
							if (!flag6)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						else if (flag4)
						{
							if (!flag6)
							{
								break;
							}
						}
						else if (flag5)
						{
							if ('!' != c)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						flag6 = false;
						flag3 = false;
						flag5 = false;
						flag4 = false;
					}
					else
					{
						flag3 = true;
						flag = false;
						if (flag2)
						{
							if (flag4)
							{
								flag4 = false;
								flag = true;
								char c4 = c2;
								char c3;
								if (c3 > c4)
								{
									throw ExceptionUtils.VbMakeException(93);
								}
								bool flag6;
								if (!flag5 || !flag6)
								{
									if (flag5 || flag6)
									{
										goto IL_0255;
									}
								}
								flag6 = c > c3 && c <= c4;
								if (flag5)
								{
									flag6 = !flag6;
								}
							}
							else
							{
								char c3 = c2;
								bool flag6 = StringType.StrLikeCompareBinary(flag5, flag6, c2, c);
							}
						}
						else
						{
							if (c2 != c && !flag5)
							{
								break;
							}
							flag5 = false;
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
							else if (num3 > num2)
							{
								return false;
							}
						}
					}
					IL_0255:
					i++;
				}
				if (!flag2)
				{
					return i == num && num3 == num2;
				}
				if (num2 == 0)
				{
					return false;
				}
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
			}
		}

		public static bool StrLikeText(string Source, string Pattern)
		{
			bool flag = false;
			int num;
			if (Pattern == null)
			{
				num = 0;
			}
			else
			{
				num = Pattern.Length;
			}
			int num2;
			if (Source == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = Source.Length;
			}
			int num3;
			char c;
			if (num3 < num2)
			{
				c = Source[num3];
			}
			CompareInfo compareInfo = Utils.GetCultureInfo().CompareInfo;
			CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
			checked
			{
				int i;
				bool flag2;
				while (i < num)
				{
					char c2 = Pattern[i];
					bool flag3;
					bool flag4;
					bool flag5;
					if (c2 == '*' && !flag2)
					{
						int num4 = StringType.AsteriskSkip(Pattern.Substring(i + 1), Source.Substring(num3), num2 - num3, CompareMethod.Text, compareInfo);
						if (num4 < 0)
						{
							return false;
						}
						if (num4 > 0)
						{
							num3 += num4;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
					}
					else if (c2 == '?' && !flag2)
					{
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '#' && !flag2)
					{
						if (!char.IsDigit(c))
						{
							break;
						}
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '-' && flag2 && flag3 && !flag && !flag4 && (i + 1 >= num || Pattern[i + 1] != ']'))
					{
						flag4 = true;
					}
					else if (c2 == '!' && flag2 && !flag5)
					{
						flag5 = true;
						bool flag6 = true;
					}
					else if (c2 == '[' && !flag2)
					{
						flag2 = true;
						char c3 = '\0';
						flag3 = false;
					}
					else if (c2 == ']' && flag2)
					{
						flag2 = false;
						bool flag6;
						if (flag3)
						{
							if (!flag6)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						else if (flag4)
						{
							if (!flag6)
							{
								break;
							}
						}
						else if (flag5)
						{
							if (compareInfo.Compare("!", Conversions.ToString(c)) != 0)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						flag6 = false;
						flag3 = false;
						flag5 = false;
						flag4 = false;
					}
					else
					{
						flag3 = true;
						flag = false;
						if (flag2)
						{
							if (flag4)
							{
								flag4 = false;
								flag = true;
								char c4 = c2;
								char c3;
								if (c3 > c4)
								{
									throw ExceptionUtils.VbMakeException(93);
								}
								bool flag6;
								if (!flag5 || !flag6)
								{
									if (flag5 || flag6)
									{
										goto IL_039F;
									}
								}
								if (compareOptions == CompareOptions.Ordinal)
								{
									flag6 = c > c3 && c <= c4;
								}
								else
								{
									flag6 = compareInfo.Compare(Conversions.ToString(c3), Conversions.ToString(c), compareOptions) < 0 && compareInfo.Compare(Conversions.ToString(c4), Conversions.ToString(c), compareOptions) >= 0;
								}
								if (flag5)
								{
									flag6 = !flag6;
								}
							}
							else
							{
								char c3 = c2;
								bool flag6 = StringType.StrLikeCompare(compareInfo, flag5, flag6, c2, c, compareOptions);
							}
						}
						else
						{
							if (compareOptions == CompareOptions.Ordinal)
							{
								if (c2 != c && !flag5)
								{
									break;
								}
							}
							else
							{
								string text = Conversions.ToString(c2);
								string text2 = Conversions.ToString(c);
								while (i + 1 < num)
								{
									if (UnicodeCategory.ModifierSymbol != char.GetUnicodeCategory(Pattern[i + 1]) && UnicodeCategory.NonSpacingMark != char.GetUnicodeCategory(Pattern[i + 1]))
									{
										break;
									}
									text += Conversions.ToString(Pattern[i + 1]);
									i++;
								}
								while (num3 + 1 < num2 && (UnicodeCategory.ModifierSymbol == char.GetUnicodeCategory(Source[num3 + 1]) || UnicodeCategory.NonSpacingMark == char.GetUnicodeCategory(Source[num3 + 1])))
								{
									text2 += Conversions.ToString(Source[num3 + 1]);
									num3++;
								}
								if (compareInfo.Compare(text, text2, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && !flag5)
								{
									break;
								}
							}
							flag5 = false;
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
							else if (num3 > num2)
							{
								return false;
							}
						}
					}
					IL_039F:
					i++;
				}
				if (!flag2)
				{
					return i == num && num3 == num2;
				}
				if (num2 == 0)
				{
					return false;
				}
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
			}
		}

		private static bool StrLikeCompareBinary(bool SeenNot, bool Match, char p, char s)
		{
			if (SeenNot && Match)
			{
				return p != s;
			}
			if (!SeenNot && !Match)
			{
				return p == s;
			}
			return Match;
		}

		private static bool StrLikeCompare(CompareInfo ci, bool SeenNot, bool Match, char p, char s, CompareOptions Options)
		{
			if (SeenNot && Match)
			{
				if (Options == CompareOptions.Ordinal)
				{
					return p != s;
				}
				return ci.Compare(Conversions.ToString(p), Conversions.ToString(s), Options) != 0;
			}
			else
			{
				if (SeenNot || Match)
				{
					return Match;
				}
				if (Options == CompareOptions.Ordinal)
				{
					return p == s;
				}
				return ci.Compare(Conversions.ToString(p), Conversions.ToString(s), Options) == 0;
			}
		}

		private static int AsteriskSkip(string Pattern, string Source, int SourceEndIndex, CompareMethod CompareOption, CompareInfo ci)
		{
			int num = Strings.Len(Pattern);
			checked
			{
				int i;
				int num2;
				while (i < num)
				{
					char c = Pattern[i];
					char c2 = c;
					bool flag3;
					if (c2 == '*')
					{
						if (num2 > 0)
						{
							bool flag;
							if (flag)
							{
								num2 = StringType.MultipleAsteriskSkip(Pattern, Source, num2, CompareOption);
								return SourceEndIndex - num2;
							}
							string text = Pattern.Substring(0, i);
							CompareOptions compareOptions;
							if (CompareOption == CompareMethod.Binary)
							{
								compareOptions = CompareOptions.Ordinal;
							}
							else
							{
								compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
							}
							num2 = ci.LastIndexOf(Source, text, compareOptions);
							return num2;
						}
					}
					else if (c2 == '-')
					{
						if (Pattern[i + 1] == ']')
						{
							bool flag2 = true;
						}
					}
					else if (c2 == '!')
					{
						if (Pattern[i + 1] == ']')
						{
							bool flag2 = true;
						}
						else
						{
							bool flag = true;
						}
					}
					else if (c2 == '[')
					{
						if (flag3)
						{
							bool flag2 = true;
						}
						else
						{
							flag3 = true;
						}
					}
					else if (c2 == ']')
					{
						bool flag2;
						if (flag2 || !flag3)
						{
							num2++;
							bool flag = true;
						}
						flag2 = false;
						flag3 = false;
					}
					else if (c2 == '?' || c2 == '#')
					{
						if (flag3)
						{
							bool flag2 = true;
						}
						else
						{
							num2++;
							bool flag = true;
						}
					}
					else if (flag3)
					{
						bool flag2 = true;
					}
					else
					{
						num2++;
					}
					i++;
				}
				return SourceEndIndex - num2;
			}
		}

		private static int MultipleAsteriskSkip(string Pattern, string Source, int Count, CompareMethod CompareOption)
		{
			int num = Strings.Len(Source);
			checked
			{
				while (Count < num)
				{
					string text = Source.Substring(num - Count);
					bool flag;
					try
					{
						flag = StringType.StrLike(text, Pattern, CompareOption);
					}
					catch (StackOverflowException ex)
					{
						throw ex;
					}
					catch (OutOfMemoryException ex2)
					{
						throw ex2;
					}
					catch (ThreadAbortException ex3)
					{
						throw ex3;
					}
					catch (Exception)
					{
						flag = false;
					}
					if (flag)
					{
						break;
					}
					Count++;
				}
				return Count;
			}
		}

		public static void MidStmtStr(ref string sDest, int StartPosition, int MaxInsertLength, string sInsert)
		{
			int length;
			if (sDest != null)
			{
				length = sDest.Length;
			}
			int num;
			if (sInsert != null)
			{
				num = sInsert.Length;
			}
			checked
			{
				StartPosition--;
				if (StartPosition < 0 || StartPosition >= length)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
				}
				if (MaxInsertLength < 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Length" }));
				}
				if (num > MaxInsertLength)
				{
					num = MaxInsertLength;
				}
				if (num > length - StartPosition)
				{
					num = length - StartPosition;
				}
				if (num == 0)
				{
					return;
				}
				StringBuilder stringBuilder = new StringBuilder(length);
				if (StartPosition > 0)
				{
					stringBuilder.Append(sDest, 0, StartPosition);
				}
				stringBuilder.Append(sInsert, 0, num);
				int num2 = length - (StartPosition + num);
				if (num2 > 0)
				{
					stringBuilder.Append(sDest, StartPosition + num, num2);
				}
				sDest = stringBuilder.ToString();
			}
		}

		private const string GENERAL_FORMAT = "G";
	}
}
