using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class LikeOperator
	{
		private LikeOperator()
		{
		}

		static LikeOperator()
		{
			LikeOperator.LigatureMap[25] = 1;
			LikeOperator.LigatureMap[25] = 2;
			LikeOperator.LigatureMap[0] = 3;
			LikeOperator.LigatureMap[32] = 4;
			LikeOperator.LigatureMap[24] = 5;
			LikeOperator.LigatureMap[56] = 6;
			LikeOperator.LigatureMap[140] = 7;
			LikeOperator.LigatureMap[141] = 8;
		}

		private static byte LigatureIndex(char ch)
		{
			if (Strings.Asc(ch) < 198 || Strings.Asc(ch) > 339)
			{
				return 0;
			}
			return LikeOperator.LigatureMap[checked(Strings.Asc(ch) - 198)];
		}

		private static int CanCharExpand(char ch, byte[] LocaleSpecificLigatureTable, CompareInfo Comparer, CompareOptions Options)
		{
			byte b = LikeOperator.LigatureIndex(ch);
			if (b == 0)
			{
				return 0;
			}
			if (LocaleSpecificLigatureTable[(int)b] == 0)
			{
				if (Comparer.Compare(Conversions.ToString(ch), LikeOperator.LigatureExpansions[(int)b]) == 0)
				{
					LocaleSpecificLigatureTable[(int)b] = 1;
				}
				else
				{
					LocaleSpecificLigatureTable[(int)b] = 2;
				}
			}
			if (LocaleSpecificLigatureTable[(int)b] == 1)
			{
				return (int)b;
			}
			int num;
			return num;
		}

		private static string GetCharExpansion(char ch, byte[] LocaleSpecificLigatureTable, CompareInfo Comparer, CompareOptions Options)
		{
			int num = LikeOperator.CanCharExpand(ch, LocaleSpecificLigatureTable, Comparer, Options);
			if (num == 0)
			{
				return Conversions.ToString(ch);
			}
			return LikeOperator.LigatureExpansions[num];
		}

		private static void ExpandString(ref string Input, ref int Length, ref LikeOperator.LigatureInfo[] InputLigatureInfo, byte[] LocaleSpecificLigatureTable, CompareInfo Comparer, CompareOptions Options, ref bool WidthChanged, bool UseFullWidth)
		{
			WidthChanged = false;
			if (Length == 0)
			{
				return;
			}
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			Encoding encoding = Encoding.GetEncoding(cultureInfo.TextInfo.ANSICodePage);
			int num = 256;
			bool flag = false;
			if (!encoding.IsSingleByte)
			{
				num = 4194560;
				if (Strings.IsValidCodePage(932))
				{
					if (UseFullWidth)
					{
						num = 10486016;
					}
					else
					{
						num = 6291712;
					}
					Input = Strings.vbLCMapString(cultureInfo, num, Input);
					flag = true;
					if (Input.Length != Length)
					{
						Length = Input.Length;
						WidthChanged = true;
					}
				}
			}
			if (!flag)
			{
				Input = Strings.vbLCMapString(cultureInfo, num, Input);
			}
			int num2 = 0;
			checked
			{
				int num3 = Length - 1;
				int num4;
				for (int i = num2; i <= num3; i++)
				{
					char c = Input[i];
					if (LikeOperator.CanCharExpand(c, LocaleSpecificLigatureTable, Comparer, Options) != 0)
					{
						num4++;
					}
				}
				if (num4 > 0)
				{
					InputLigatureInfo = new LikeOperator.LigatureInfo[Length + num4 - 1 + 1];
					StringBuilder stringBuilder = new StringBuilder(Length + num4 - 1);
					int num5 = 0;
					int num6 = 0;
					int num7 = Length - 1;
					for (int j = num6; j <= num7; j++)
					{
						char c2 = Input[j];
						if (LikeOperator.CanCharExpand(c2, LocaleSpecificLigatureTable, Comparer, Options) != 0)
						{
							string charExpansion = LikeOperator.GetCharExpansion(c2, LocaleSpecificLigatureTable, Comparer, Options);
							stringBuilder.Append(charExpansion);
							InputLigatureInfo[num5].Kind = LikeOperator.CharKind.ExpandedChar1;
							InputLigatureInfo[num5].CharBeforeExpansion = c2;
							num5++;
							InputLigatureInfo[num5].Kind = LikeOperator.CharKind.ExpandedChar2;
							InputLigatureInfo[num5].CharBeforeExpansion = c2;
						}
						else
						{
							stringBuilder.Append(c2);
						}
						num5++;
					}
					Input = stringBuilder.ToString();
					Length = stringBuilder.Length;
				}
			}
		}

		public static object LikeObject(object Source, object Pattern, CompareMethod CompareOption)
		{
			IConvertible convertible = Source as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Source == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Pattern as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Pattern == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object && Source is char[])
			{
				typeCode = TypeCode.String;
			}
			if (typeCode2 == TypeCode.Object && Pattern is char[])
			{
				typeCode2 = TypeCode.String;
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Like, new object[] { Source, Pattern });
			}
			return LikeOperator.LikeString(Conversions.ToString(Source), Conversions.ToString(Pattern), CompareOption);
		}

		public static bool LikeString(string Source, string Pattern, CompareMethod CompareOption)
		{
			LikeOperator.LigatureInfo[] array = null;
			LikeOperator.LigatureInfo[] array2 = null;
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
			checked
			{
				CompareOptions compareOptions;
				CompareInfo compareInfo;
				if (CompareOption == CompareMethod.Binary)
				{
					compareOptions = CompareOptions.Ordinal;
					compareInfo = null;
				}
				else
				{
					compareInfo = Utils.GetCultureInfo().CompareInfo;
					compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
					byte[] array3 = new byte[LikeOperator.LigatureExpansions.Length - 1 + 1];
					byte[] array4 = array3;
					CompareInfo compareInfo2 = compareInfo;
					CompareOptions compareOptions2 = compareOptions;
					bool flag = false;
					LikeOperator.ExpandString(ref Source, ref num2, ref array, array4, compareInfo2, compareOptions2, ref flag, false);
					byte[] array5 = array3;
					CompareInfo compareInfo3 = compareInfo;
					CompareOptions compareOptions3 = compareOptions;
					flag = false;
					LikeOperator.ExpandString(ref Pattern, ref num, ref array2, array5, compareInfo3, compareOptions3, ref flag, false);
				}
				int i;
				int num3;
				while (i < num)
				{
					if (num3 >= num2)
					{
						break;
					}
					char c = Pattern[i];
					char c2 = c;
					if (c2 == '?' || c2 == '？')
					{
						LikeOperator.SkipToEndOfExpandedChar(array, num2, ref num3);
					}
					else if (c2 == '#' || c2 == '＃')
					{
						if (!char.IsDigit(Source[num3]))
						{
							return false;
						}
					}
					else if (c2 == '[' || c2 == '［')
					{
						string text = Source;
						int num4 = num2;
						LikeOperator.LigatureInfo[] array6 = array;
						string text2 = Pattern;
						int num5 = num;
						LikeOperator.LigatureInfo[] array7 = array2;
						CompareInfo compareInfo4 = compareInfo;
						CompareOptions compareOptions4 = compareOptions;
						bool flag = false;
						bool flag2;
						bool flag3;
						bool flag4;
						LikeOperator.MatchRange(text, num4, ref num3, array6, text2, num5, ref i, array7, ref flag2, ref flag3, ref flag4, compareInfo4, compareOptions4, ref flag, null, false);
						if (flag4)
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
						}
						if (flag3)
						{
							return false;
						}
						if (flag2)
						{
							i++;
							continue;
						}
					}
					else if (c2 == '*' || c2 == '＊')
					{
						bool flag5;
						bool flag6;
						LikeOperator.MatchAsterisk(Source, num2, num3, array, Pattern, num, i, array2, ref flag5, ref flag6, compareInfo, compareOptions);
						if (flag6)
						{
							throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
						}
						return !flag5;
					}
					else if (LikeOperator.CompareChars(Source, num2, num3, ref num3, array, Pattern, num, i, ref i, array2, compareInfo, compareOptions, false, false) != 0)
					{
						return false;
					}
					i++;
					num3++;
				}
				while (i < num)
				{
					char c = Pattern[i];
					if (c == '*' || c == '＊')
					{
						i++;
					}
					else
					{
						if (i + 1 >= num)
						{
							break;
						}
						if (c != '[' || Pattern[i + 1] != ']')
						{
							if (c != '［' || Pattern[i + 1] != '］')
							{
								break;
							}
						}
						i += 2;
					}
				}
				return i >= num && num3 >= num2;
			}
		}

		private static void SkipToEndOfExpandedChar(LikeOperator.LigatureInfo[] InputLigatureInfo, int Length, ref int Current)
		{
			checked
			{
				if (InputLigatureInfo != null)
				{
					if (Current < Length && InputLigatureInfo[Current].Kind == LikeOperator.CharKind.ExpandedChar1)
					{
						Current++;
					}
				}
			}
		}

		private static int CompareChars(string Left, int LeftLength, int LeftStart, ref int LeftEnd, LikeOperator.LigatureInfo[] LeftLigatureInfo, string Right, int RightLength, int RightStart, ref int RightEnd, LikeOperator.LigatureInfo[] RightLigatureInfo, CompareInfo Comparer, CompareOptions Options, bool MatchBothCharsOfExpandedCharInRight = false, bool UseUnexpandedCharForRight = false)
		{
			LeftEnd = LeftStart;
			RightEnd = RightStart;
			checked
			{
				if (Options == CompareOptions.Ordinal)
				{
					return (int)(Left[LeftStart] - Right[RightStart]);
				}
				if (UseUnexpandedCharForRight)
				{
					if (RightLigatureInfo != null && RightLigatureInfo[RightEnd].Kind == LikeOperator.CharKind.ExpandedChar1)
					{
						Right = Right.Substring(RightStart, RightEnd - RightStart);
						Right += Conversions.ToString(RightLigatureInfo[RightEnd].CharBeforeExpansion);
						RightEnd++;
						return LikeOperator.CompareChars(Left.Substring(LeftStart, LeftEnd - LeftStart + 1), Right, Comparer, Options);
					}
				}
				else if (MatchBothCharsOfExpandedCharInRight)
				{
					int num = RightEnd;
					LikeOperator.SkipToEndOfExpandedChar(RightLigatureInfo, RightLength, ref RightEnd);
					if (num < RightEnd)
					{
						int num2 = 0;
						if (LeftEnd + 1 < LeftLength)
						{
							num2 = 1;
						}
						int num3 = LikeOperator.CompareChars(Left.Substring(LeftStart, LeftEnd - LeftStart + 1 + num2), Right.Substring(RightStart, RightEnd - RightStart + 1), Comparer, Options);
						if (num3 == 0)
						{
							LeftEnd += num2;
						}
						return num3;
					}
				}
				if (LeftEnd == LeftStart && RightEnd == RightStart)
				{
					return Comparer.Compare(Conversions.ToString(Left[LeftStart]), Conversions.ToString(Right[RightStart]), Options);
				}
				return LikeOperator.CompareChars(Left.Substring(LeftStart, LeftEnd - LeftStart + 1), Right.Substring(RightStart, RightEnd - RightStart + 1), Comparer, Options);
			}
		}

		private static int CompareChars(string Left, string Right, CompareInfo Comparer, CompareOptions Options)
		{
			if (Options == CompareOptions.Ordinal)
			{
				return (int)(checked(Left[0] - Right[0]));
			}
			return Comparer.Compare(Left, Right, Options);
		}

		private static int CompareChars(char Left, char Right, CompareInfo Comparer, CompareOptions Options)
		{
			if (Options == CompareOptions.Ordinal)
			{
				return (int)(checked(Left - Right));
			}
			return Comparer.Compare(Conversions.ToString(Left), Conversions.ToString(Right), Options);
		}

		private static void MatchRange(string Source, int SourceLength, ref int SourceIndex, LikeOperator.LigatureInfo[] SourceLigatureInfo, string Pattern, int PatternLength, ref int PatternIndex, LikeOperator.LigatureInfo[] PatternLigatureInfo, ref bool RangePatternEmpty, ref bool Mismatch, ref bool PatternError, CompareInfo Comparer, CompareOptions Options, ref bool SeenNot = false, List<LikeOperator.Range> RangeList = null, bool ValidatePatternWithoutMatching = false)
		{
			RangePatternEmpty = false;
			Mismatch = false;
			PatternError = false;
			SeenNot = false;
			checked
			{
				PatternIndex++;
				if (PatternIndex >= PatternLength)
				{
					PatternError = true;
					return;
				}
				char c = Pattern[PatternIndex];
				if (c == '!' || c == '！')
				{
					SeenNot = true;
					PatternIndex++;
					if (PatternIndex >= PatternLength)
					{
						Mismatch = true;
						return;
					}
					c = Pattern[PatternIndex];
				}
				LikeOperator.Range range;
				if (c != ']' && c != '］')
				{
					while (c != ']' && c != '］')
					{
						int num2;
						int num3;
						if (!ValidatePatternWithoutMatching && PatternLigatureInfo != null && PatternLigatureInfo[PatternIndex].Kind == LikeOperator.CharKind.ExpandedChar1)
						{
							int num = LikeOperator.CompareChars(Source, SourceLength, SourceIndex, ref num2, SourceLigatureInfo, Pattern, PatternLength, PatternIndex, ref num3, PatternLigatureInfo, Comparer, Options, true, false);
							if (num == 0)
							{
								SourceIndex = num2;
								PatternIndex = num3;
								goto IL_037F;
							}
						}
						else
						{
							num3 = PatternIndex;
							LikeOperator.SkipToEndOfExpandedChar(PatternLigatureInfo, PatternLength, ref num3);
						}
						range.Start = PatternIndex;
						range.StartLength = num3 - PatternIndex + 1;
						string text;
						if (Options == CompareOptions.Ordinal)
						{
							text = Conversions.ToString(Pattern[PatternIndex]);
						}
						else if (PatternLigatureInfo != null && PatternLigatureInfo[PatternIndex].Kind == LikeOperator.CharKind.ExpandedChar1)
						{
							text = Conversions.ToString(PatternLigatureInfo[PatternIndex].CharBeforeExpansion);
							PatternIndex = num3;
						}
						else
						{
							text = Pattern.Substring(PatternIndex, num3 - PatternIndex + 1);
							PatternIndex = num3;
						}
						if (num3 + 2 < PatternLength && (Pattern[num3 + 1] == '-' || Pattern[num3 + 1] == '－') && Pattern[num3 + 2] != ']' && Pattern[num3 + 2] != '］')
						{
							PatternIndex += 2;
							if (!ValidatePatternWithoutMatching && PatternLigatureInfo != null && PatternLigatureInfo[PatternIndex].Kind == LikeOperator.CharKind.ExpandedChar1)
							{
								int num = LikeOperator.CompareChars(Source, SourceLength, SourceIndex, ref num2, SourceLigatureInfo, Pattern, PatternLength, PatternIndex, ref num3, PatternLigatureInfo, Comparer, Options, true, false);
								if (num == 0)
								{
									PatternIndex = num3;
									goto IL_037F;
								}
							}
							else
							{
								num3 = PatternIndex;
								LikeOperator.SkipToEndOfExpandedChar(PatternLigatureInfo, PatternLength, ref num3);
							}
							range.End = PatternIndex;
							range.EndLength = num3 - PatternIndex + 1;
							string text2;
							if (Options == CompareOptions.Ordinal)
							{
								text2 = Conversions.ToString(Pattern[PatternIndex]);
							}
							else if (PatternLigatureInfo != null && PatternLigatureInfo[PatternIndex].Kind == LikeOperator.CharKind.ExpandedChar1)
							{
								text2 = Conversions.ToString(PatternLigatureInfo[PatternIndex].CharBeforeExpansion);
								PatternIndex = num3;
							}
							else
							{
								text2 = Pattern.Substring(PatternIndex, num3 - PatternIndex + 1);
								PatternIndex = num3;
							}
							if (LikeOperator.CompareChars(text, text2, Comparer, Options) > 0)
							{
								PatternError = true;
								return;
							}
							if (!ValidatePatternWithoutMatching)
							{
								int num4 = SourceIndex;
								int num5 = range.Start + range.StartLength;
								int start = range.Start;
								int num6 = 0;
								if (LikeOperator.CompareChars(Source, SourceLength, num4, ref num2, SourceLigatureInfo, Pattern, num5, start, ref num6, PatternLigatureInfo, Comparer, Options, false, true) >= 0)
								{
									int num7 = SourceIndex;
									int num8 = range.End + range.EndLength;
									int end = range.End;
									int num9 = 0;
									if (LikeOperator.CompareChars(Source, SourceLength, num7, ref num2, SourceLigatureInfo, Pattern, num8, end, ref num9, PatternLigatureInfo, Comparer, Options, false, true) <= 0)
									{
										goto IL_037F;
									}
								}
							}
						}
						else
						{
							if (!ValidatePatternWithoutMatching)
							{
								int num10 = SourceIndex;
								int num11 = range.Start + range.StartLength;
								int start2 = range.Start;
								int num9 = 0;
								if (LikeOperator.CompareChars(Source, SourceLength, num10, ref num2, SourceLigatureInfo, Pattern, num11, start2, ref num9, PatternLigatureInfo, Comparer, Options, false, true) == 0)
								{
									goto IL_037F;
								}
							}
							range.End = -1;
							range.EndLength = 0;
						}
						if (RangeList != null)
						{
							RangeList.Add(range);
						}
						PatternIndex++;
						if (PatternIndex >= PatternLength)
						{
							PatternError = true;
							return;
						}
						c = Pattern[PatternIndex];
						continue;
						IL_037F:
						if (SeenNot)
						{
							Mismatch = true;
							return;
						}
						for (;;)
						{
							PatternIndex++;
							if (PatternIndex >= PatternLength)
							{
								break;
							}
							if (Pattern[PatternIndex] == ']' || Pattern[PatternIndex] == '］')
							{
								goto IL_03BC;
							}
						}
						PatternError = true;
						return;
						IL_03BC:
						SourceIndex = num2;
						return;
					}
					Mismatch = !SeenNot;
					return;
				}
				if (!SeenNot)
				{
					RangePatternEmpty = true;
					return;
				}
				SeenNot = false;
				if (!ValidatePatternWithoutMatching)
				{
					Mismatch = LikeOperator.CompareChars(Source[SourceIndex], '!', Comparer, Options) != 0;
				}
				if (RangeList != null)
				{
					range.Start = PatternIndex - 1;
					range.StartLength = 1;
					range.End = -1;
					range.EndLength = 0;
					RangeList.Add(range);
					return;
				}
			}
		}

		private static bool ValidateRangePattern(string Pattern, int PatternLength, ref int PatternIndex, LikeOperator.LigatureInfo[] PatternLigatureInfo, CompareInfo Comparer, CompareOptions Options, ref bool SeenNot, ref List<LikeOperator.Range> RangeList)
		{
			string text = null;
			int num = -1;
			int num2 = -1;
			LikeOperator.LigatureInfo[] array = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3;
			LikeOperator.MatchRange(text, num, ref num2, array, Pattern, PatternLength, ref PatternIndex, PatternLigatureInfo, ref flag, ref flag2, ref flag3, Comparer, Options, ref SeenNot, RangeList, true);
			return !flag3;
		}

		private static void BuildPatternGroups(string Source, int SourceLength, ref int SourceIndex, LikeOperator.LigatureInfo[] SourceLigatureInfo, string Pattern, int PatternLength, ref int PatternIndex, LikeOperator.LigatureInfo[] PatternLigatureInfo, ref bool PatternError, ref int PGIndexForLastAsterisk, CompareInfo Comparer, CompareOptions Options, ref LikeOperator.PatternGroup[] PatternGroups)
		{
			PatternError = false;
			PGIndexForLastAsterisk = 0;
			PatternGroups = new LikeOperator.PatternGroup[16];
			int num = 15;
			LikeOperator.PatternType patternType = LikeOperator.PatternType.NONE;
			int i = 0;
			checked
			{
				for (;;)
				{
					if (i >= num)
					{
						LikeOperator.PatternGroup[] array = new LikeOperator.PatternGroup[num + 16 + 1];
						PatternGroups.CopyTo(array, 0);
						PatternGroups = array;
						num += 16;
					}
					char c = Pattern[PatternIndex];
					char c2 = c;
					if (c2 == '*' || c2 == '＊')
					{
						if (patternType != LikeOperator.PatternType.STAR)
						{
							patternType = LikeOperator.PatternType.STAR;
							PatternGroups[i].PatType = LikeOperator.PatternType.STAR;
							PGIndexForLastAsterisk = i;
							i++;
						}
					}
					else if (c2 == '[' || c2 == '［')
					{
						bool flag = false;
						List<LikeOperator.Range> list = new List<LikeOperator.Range>();
						if (!LikeOperator.ValidateRangePattern(Pattern, PatternLength, ref PatternIndex, PatternLigatureInfo, Comparer, Options, ref flag, ref list))
						{
							break;
						}
						if (list.Count != 0)
						{
							if (flag)
							{
								patternType = LikeOperator.PatternType.EXCLIST;
							}
							else
							{
								patternType = LikeOperator.PatternType.INCLIST;
							}
							PatternGroups[i].PatType = patternType;
							PatternGroups[i].CharCount = 1;
							PatternGroups[i].RangeList = list;
							i++;
						}
					}
					else if (c2 == '#' || c2 == '＃')
					{
						if (patternType == LikeOperator.PatternType.DIGIT)
						{
							LikeOperator.PatternGroup[] array2 = PatternGroups;
							int num2 = i - 1;
							array2[num2].CharCount = PatternGroups[num2].CharCount + 1;
						}
						else
						{
							PatternGroups[i].PatType = LikeOperator.PatternType.DIGIT;
							PatternGroups[i].CharCount = 1;
							i++;
							patternType = LikeOperator.PatternType.DIGIT;
						}
					}
					else if (c2 == '?' || c2 == '？')
					{
						if (patternType == LikeOperator.PatternType.ANYCHAR)
						{
							LikeOperator.PatternGroup[] array3 = PatternGroups;
							int num2 = i - 1;
							array3[num2].CharCount = PatternGroups[num2].CharCount + 1;
						}
						else
						{
							PatternGroups[i].PatType = LikeOperator.PatternType.ANYCHAR;
							PatternGroups[i].CharCount = 1;
							i++;
							patternType = LikeOperator.PatternType.ANYCHAR;
						}
					}
					else
					{
						int num3 = PatternIndex;
						int num4 = PatternIndex;
						if (num4 >= PatternLength)
						{
							num4 = PatternLength - 1;
						}
						if (patternType == LikeOperator.PatternType.STRING)
						{
							LikeOperator.PatternGroup[] array4 = PatternGroups;
							int num2 = i - 1;
							array4[num2].CharCount = PatternGroups[num2].CharCount + 1;
							PatternGroups[i - 1].StringPatternEnd = num4;
						}
						else
						{
							PatternGroups[i].PatType = LikeOperator.PatternType.STRING;
							PatternGroups[i].CharCount = 1;
							PatternGroups[i].StringPatternStart = num3;
							PatternGroups[i].StringPatternEnd = num4;
							i++;
							patternType = LikeOperator.PatternType.STRING;
						}
					}
					PatternIndex++;
					if (PatternIndex >= PatternLength)
					{
						goto Block_14;
					}
				}
				PatternError = true;
				return;
			}
			Block_14:
			PatternGroups[i].PatType = LikeOperator.PatternType.NONE;
			PatternGroups[i].MinSourceIndex = SourceLength;
			int num5 = SourceLength;
			checked
			{
				while (i > 0)
				{
					switch (PatternGroups[i].PatType)
					{
					case LikeOperator.PatternType.STRING:
						num5 -= PatternGroups[i].CharCount;
						break;
					case LikeOperator.PatternType.EXCLIST:
					case LikeOperator.PatternType.INCLIST:
						num5--;
						break;
					case LikeOperator.PatternType.DIGIT:
					case LikeOperator.PatternType.ANYCHAR:
						num5 -= PatternGroups[i].CharCount;
						break;
					}
					PatternGroups[i].MaxSourceIndex = num5;
					i--;
				}
			}
		}

		private static void MatchAsterisk(string Source, int SourceLength, int SourceIndex, LikeOperator.LigatureInfo[] SourceLigatureInfo, string Pattern, int PatternLength, int PatternIndex, LikeOperator.LigatureInfo[] PattternLigatureInfo, ref bool Mismatch, ref bool PatternError, CompareInfo Comparer, CompareOptions Options)
		{
			Mismatch = false;
			PatternError = false;
			if (PatternIndex >= PatternLength)
			{
				return;
			}
			LikeOperator.PatternGroup[] array = null;
			int num;
			LikeOperator.BuildPatternGroups(Source, SourceLength, ref SourceIndex, SourceLigatureInfo, Pattern, PatternLength, ref PatternIndex, PattternLigatureInfo, ref PatternError, ref num, Comparer, Options, ref array);
			if (PatternError)
			{
				return;
			}
			checked
			{
				if (array[num + 1].PatType != LikeOperator.PatternType.NONE)
				{
					int num2 = SourceIndex;
					int i = num + 1;
					int num3;
					do
					{
						num3 += array[i].CharCount;
						i++;
					}
					while (array[i].PatType != LikeOperator.PatternType.NONE);
					SourceIndex = SourceLength;
					LikeOperator.SubtractChars(Source, SourceLength, ref SourceIndex, num3, SourceLigatureInfo, Options);
					LikeOperator.MatchAsterisk(Source, SourceLength, SourceIndex, SourceLigatureInfo, Pattern, PattternLigatureInfo, array, num, ref Mismatch, ref PatternError, Comparer, Options);
					if (PatternError)
					{
						return;
					}
					if (Mismatch)
					{
						return;
					}
					SourceLength = array[num + 1].StartIndexOfPossibleMatch;
					if (SourceLength <= 0)
					{
						return;
					}
					array[i].MaxSourceIndex = SourceLength;
					array[i].MinSourceIndex = SourceLength;
					array[i].StartIndexOfPossibleMatch = 0;
					array[num + 1] = array[i];
					array[num].MinSourceIndex = 0;
					array[num].StartIndexOfPossibleMatch = 0;
					i = num + 1;
					int num4 = SourceLength;
					while (i > 0)
					{
						switch (array[i].PatType)
						{
						case LikeOperator.PatternType.STRING:
							num4 -= array[i].CharCount;
							break;
						case LikeOperator.PatternType.EXCLIST:
						case LikeOperator.PatternType.INCLIST:
							num4--;
							break;
						case LikeOperator.PatternType.DIGIT:
						case LikeOperator.PatternType.ANYCHAR:
							num4 -= array[i].CharCount;
							break;
						}
						array[i].MaxSourceIndex = num4;
						i--;
					}
					SourceIndex = num2;
				}
				LikeOperator.MatchAsterisk(Source, SourceLength, SourceIndex, SourceLigatureInfo, Pattern, PattternLigatureInfo, array, 0, ref Mismatch, ref PatternError, Comparer, Options);
			}
		}

		private static void MatchAsterisk(string Source, int SourceLength, int SourceIndex, LikeOperator.LigatureInfo[] SourceLigatureInfo, string Pattern, LikeOperator.LigatureInfo[] PatternLigatureInfo, LikeOperator.PatternGroup[] PatternGroups, int PGIndex, ref bool Mismatch, ref bool PatternError, CompareInfo Comparer, CompareOptions Options)
		{
			int num = PGIndex;
			int num2 = SourceIndex;
			int num3 = -1;
			int num4 = -1;
			PatternGroups[PGIndex].MinSourceIndex = SourceIndex;
			PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
			checked
			{
				PGIndex++;
				for (;;)
				{
					LikeOperator.PatternGroup patternGroup = PatternGroups[PGIndex];
					switch (patternGroup.PatType)
					{
					case LikeOperator.PatternType.STRING:
						while (SourceIndex <= patternGroup.MaxSourceIndex)
						{
							PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
							int num5 = patternGroup.StringPatternStart;
							int num6 = 0;
							int num7 = SourceIndex;
							bool flag = true;
							for (;;)
							{
								int num8 = LikeOperator.CompareChars(Source, SourceLength, num7, ref num7, SourceLigatureInfo, Pattern, patternGroup.StringPatternEnd + 1, num5, ref num5, PatternLigatureInfo, Comparer, Options, false, false);
								if (flag)
								{
									flag = false;
									num6 = num7 + 1;
								}
								if (num8 != 0)
								{
									break;
								}
								num5++;
								num7++;
								if (num5 > patternGroup.StringPatternEnd)
								{
									goto Block_5;
								}
								if (num7 >= SourceLength)
								{
									goto Block_6;
								}
							}
							SourceIndex = num6;
							num = PGIndex - 1;
							num2 = SourceIndex;
							continue;
							Block_5:
							SourceIndex = num7;
							goto IL_02F4;
						}
						goto Block_2;
					case LikeOperator.PatternType.EXCLIST:
					case LikeOperator.PatternType.INCLIST:
						while (SourceIndex <= patternGroup.MaxSourceIndex)
						{
							PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
							if (LikeOperator.MatchRangeAfterAsterisk(Source, SourceLength, ref SourceIndex, SourceLigatureInfo, Pattern, PatternLigatureInfo, patternGroup, Comparer, Options))
							{
								goto IL_02F4;
							}
							num = PGIndex - 1;
							num2 = SourceIndex;
						}
						goto Block_10;
					case LikeOperator.PatternType.DIGIT:
						IL_010A:
						while (SourceIndex <= patternGroup.MaxSourceIndex)
						{
							PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
							int num9 = 1;
							int charCount = patternGroup.CharCount;
							for (int i = num9; i <= charCount; i++)
							{
								char c = Source[SourceIndex];
								SourceIndex++;
								if (!char.IsDigit(c))
								{
									num = PGIndex - 1;
									num2 = SourceIndex;
									goto IL_010A;
								}
							}
							goto IL_02F4;
						}
						goto Block_7;
					case LikeOperator.PatternType.ANYCHAR:
					{
						if (SourceIndex > patternGroup.MaxSourceIndex)
						{
							goto Block_12;
						}
						PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
						int num10 = 1;
						int charCount2 = patternGroup.CharCount;
						for (int j = num10; j <= charCount2; j++)
						{
							if (SourceIndex >= SourceLength)
							{
								goto Block_13;
							}
							LikeOperator.SkipToEndOfExpandedChar(SourceLigatureInfo, SourceLength, ref SourceIndex);
							SourceIndex++;
						}
						goto IL_02F4;
					}
					case LikeOperator.PatternType.STAR:
						PatternGroups[PGIndex].StartIndexOfPossibleMatch = SourceIndex;
						patternGroup.MinSourceIndex = SourceIndex;
						if (PatternGroups[num].PatType != LikeOperator.PatternType.STAR)
						{
							if (SourceIndex > patternGroup.MaxSourceIndex)
							{
								goto Block_19;
							}
							goto IL_0285;
						}
						break;
					case LikeOperator.PatternType.NONE:
						PatternGroups[PGIndex].StartIndexOfPossibleMatch = patternGroup.MaxSourceIndex;
						if (SourceIndex < patternGroup.MaxSourceIndex)
						{
							num = PGIndex - 1;
							num2 = patternGroup.MaxSourceIndex;
						}
						if (PatternGroups[num].PatType != LikeOperator.PatternType.STAR && PatternGroups[num].PatType != LikeOperator.PatternType.NONE)
						{
							goto IL_0285;
						}
						return;
					default:
						goto IL_02F4;
					}
					IL_02E9:
					PGIndex++;
					continue;
					IL_02F4:
					if (PGIndex == num)
					{
						if (SourceIndex == num2)
						{
							SourceIndex = PatternGroups[num3].MinSourceIndex;
							PGIndex = num3;
							num = num3;
						}
						else if (SourceIndex < num2)
						{
							int num11 = num4;
							PatternGroups[num11].MinSourceIndex = PatternGroups[num11].MinSourceIndex + 1;
							SourceIndex = PatternGroups[num4].MinSourceIndex;
							PGIndex = num4 + 1;
						}
						else
						{
							PGIndex++;
							num = num4;
						}
					}
					else
					{
						PGIndex++;
					}
					continue;
					IL_0285:
					num3 = PGIndex;
					SourceIndex = num2;
					PGIndex = num;
					do
					{
						LikeOperator.SubtractChars(Source, SourceLength, ref SourceIndex, PatternGroups[PGIndex].CharCount, SourceLigatureInfo, Options);
						PGIndex--;
					}
					while (PatternGroups[PGIndex].PatType != LikeOperator.PatternType.STAR);
					SourceIndex = Math.Max(SourceIndex, PatternGroups[PGIndex].MinSourceIndex + 1);
					PatternGroups[PGIndex].MinSourceIndex = SourceIndex;
					num4 = PGIndex;
					goto IL_02E9;
				}
			}
			Block_2:
			Mismatch = true;
			return;
			Block_6:
			Mismatch = true;
			return;
			Block_7:
			Mismatch = true;
			return;
			Block_10:
			Mismatch = true;
			return;
			Block_12:
			Mismatch = true;
			return;
			Block_13:
			Mismatch = true;
			return;
			Block_19:
			Mismatch = true;
		}

		private static bool MatchRangeAfterAsterisk(string Source, int SourceLength, ref int SourceIndex, LikeOperator.LigatureInfo[] SourceLigatureInfo, string Pattern, LikeOperator.LigatureInfo[] PatternLigatureInfo, LikeOperator.PatternGroup PG, CompareInfo Comparer, CompareOptions Options)
		{
			List<LikeOperator.Range> rangeList = PG.RangeList;
			int num = SourceIndex;
			bool flag = false;
			checked
			{
				try
				{
					foreach (LikeOperator.Range range in rangeList)
					{
						int num2 = 1;
						int num5;
						int num6;
						if (PatternLigatureInfo != null && PatternLigatureInfo[range.Start].Kind == LikeOperator.CharKind.ExpandedChar1)
						{
							int num3 = SourceIndex;
							int num4 = range.Start + range.StartLength;
							int start = range.Start;
							num5 = 0;
							num6 = LikeOperator.CompareChars(Source, SourceLength, num3, ref num, SourceLigatureInfo, Pattern, num4, start, ref num5, PatternLigatureInfo, Comparer, Options, true, false);
							if (num6 == 0)
							{
								flag = true;
								break;
							}
						}
						int num7 = SourceIndex;
						int num8 = range.Start + range.StartLength;
						int start2 = range.Start;
						num5 = 0;
						num6 = LikeOperator.CompareChars(Source, SourceLength, num7, ref num, SourceLigatureInfo, Pattern, num8, start2, ref num5, PatternLigatureInfo, Comparer, Options, false, true);
						if (num6 > 0 && range.End >= 0)
						{
							int num9 = SourceIndex;
							int num10 = range.End + range.EndLength;
							int end = range.End;
							num5 = 0;
							num2 = LikeOperator.CompareChars(Source, SourceLength, num9, ref num, SourceLigatureInfo, Pattern, num10, end, ref num5, PatternLigatureInfo, Comparer, Options, false, true);
						}
						if (num6 == 0 || (num6 > 0 && num2 <= 0))
						{
							flag = true;
							break;
						}
					}
				}
				finally
				{
					List<LikeOperator.Range>.Enumerator enumerator;
					((IDisposable)enumerator).Dispose();
				}
				if (PG.PatType == LikeOperator.PatternType.EXCLIST)
				{
					flag = !flag;
				}
				SourceIndex = num + 1;
				return flag;
			}
		}

		private static void SubtractChars(string Input, int InputLength, ref int Current, int CharsToSubtract, LikeOperator.LigatureInfo[] InputLigatureInfo, CompareOptions Options)
		{
			checked
			{
				if (Options == CompareOptions.Ordinal)
				{
					Current -= CharsToSubtract;
					if (Current < 0)
					{
						Current = 0;
						return;
					}
				}
				else
				{
					for (int i = 1; i <= CharsToSubtract; i++)
					{
						LikeOperator.SubtractOneCharInTextCompareMode(Input, InputLength, ref Current, InputLigatureInfo, Options);
						if (Current < 0)
						{
							Current = 0;
							break;
						}
					}
				}
			}
		}

		private static void SubtractOneCharInTextCompareMode(string Input, int InputLength, ref int Current, LikeOperator.LigatureInfo[] InputLigatureInfo, CompareOptions Options)
		{
			checked
			{
				if (Current >= InputLength)
				{
					Current--;
					return;
				}
				if (InputLigatureInfo != null && InputLigatureInfo[Current].Kind == LikeOperator.CharKind.ExpandedChar2)
				{
					Current -= 2;
				}
				else
				{
					Current--;
				}
			}
		}

		private static string[] LigatureExpansions = new string[] { "", "ss", "sz", "AE", "ae", "TH", "th", "OE", "oe" };

		private static byte[] LigatureMap = new byte[142];

		private enum Ligatures
		{
			Invalid,
			Min = 198,
			ssBeta = 223,
			szBeta = 223,
			aeUpper = 198,
			ae = 230,
			thUpper = 222,
			th = 254,
			oeUpper = 338,
			oe,
			Max = 339
		}

		private enum CharKind
		{
			None,
			ExpandedChar1,
			ExpandedChar2
		}

		private struct LigatureInfo
		{
			internal LikeOperator.CharKind Kind;

			internal char CharBeforeExpansion;
		}

		private enum PatternType
		{
			STRING,
			EXCLIST,
			INCLIST,
			DIGIT,
			ANYCHAR,
			STAR,
			NONE
		}

		private struct PatternGroup
		{
			internal LikeOperator.PatternType PatType;

			internal int MaxSourceIndex;

			internal int CharCount;

			internal int StringPatternStart;

			internal int StringPatternEnd;

			internal int MinSourceIndex;

			internal List<LikeOperator.Range> RangeList;

			public int StartIndexOfPossibleMatch;
		}

		private struct Range
		{
			internal int Start;

			internal int StartLength;

			internal int End;

			internal int EndLength;
		}
	}
}
