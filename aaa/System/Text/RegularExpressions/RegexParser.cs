using System;
using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002B RID: 43
	internal sealed class RegexParser
	{
		// Token: 0x060001DA RID: 474 RVA: 0x0000E380 File Offset: 0x0000D380
		internal static RegexTree Parse(string re, RegexOptions op)
		{
			RegexParser regexParser = new RegexParser(((op & RegexOptions.CultureInvariant) != RegexOptions.None) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			regexParser._options = op;
			regexParser.SetPattern(re);
			regexParser.CountCaptures();
			regexParser.Reset(op);
			RegexNode regexNode = regexParser.ScanRegex();
			string[] array;
			if (regexParser._capnamelist == null)
			{
				array = null;
			}
			else
			{
				array = (string[])regexParser._capnamelist.ToArray(typeof(string));
			}
			return new RegexTree(regexNode, regexParser._caps, regexParser._capnumlist, regexParser._captop, regexParser._capnames, array, op);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000E414 File Offset: 0x0000D414
		internal static RegexReplacement ParseReplacement(string rep, Hashtable caps, int capsize, Hashtable capnames, RegexOptions op)
		{
			RegexParser regexParser = new RegexParser(((op & RegexOptions.CultureInvariant) != RegexOptions.None) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			regexParser._options = op;
			regexParser.NoteCaptures(caps, capsize, capnames);
			regexParser.SetPattern(rep);
			RegexNode regexNode = regexParser.ScanReplacement();
			return new RegexReplacement(rep, regexNode, caps);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000E464 File Offset: 0x0000D464
		internal static string Escape(string input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (RegexParser.IsMetachar(input[i]))
				{
					StringBuilder stringBuilder = new StringBuilder();
					char c = input[i];
					stringBuilder.Append(input, 0, i);
					do
					{
						stringBuilder.Append('\\');
						switch (c)
						{
						case '\t':
							c = 't';
							break;
						case '\n':
							c = 'n';
							break;
						case '\f':
							c = 'f';
							break;
						case '\r':
							c = 'r';
							break;
						}
						stringBuilder.Append(c);
						i++;
						int num = i;
						while (i < input.Length)
						{
							c = input[i];
							if (RegexParser.IsMetachar(c))
							{
								break;
							}
							i++;
						}
						stringBuilder.Append(input, num, i - num);
					}
					while (i < input.Length);
					return stringBuilder.ToString();
				}
			}
			return input;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000E53C File Offset: 0x0000D53C
		internal static string Unescape(string input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '\\')
				{
					StringBuilder stringBuilder = new StringBuilder();
					RegexParser regexParser = new RegexParser(CultureInfo.InvariantCulture);
					regexParser.SetPattern(input);
					stringBuilder.Append(input, 0, i);
					do
					{
						i++;
						regexParser.Textto(i);
						if (i < input.Length)
						{
							stringBuilder.Append(regexParser.ScanCharEscape());
						}
						i = regexParser.Textpos();
						int num = i;
						while (i < input.Length && input[i] != '\\')
						{
							i++;
						}
						stringBuilder.Append(input, num, i - num);
					}
					while (i < input.Length);
					return stringBuilder.ToString();
				}
			}
			return input;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000E5F1 File Offset: 0x0000D5F1
		private RegexParser(CultureInfo culture)
		{
			this._culture = culture;
			this._optionsStack = new ArrayList();
			this._caps = new Hashtable();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000E616 File Offset: 0x0000D616
		internal void SetPattern(string Re)
		{
			if (Re == null)
			{
				Re = string.Empty;
			}
			this._pattern = Re;
			this._currentPos = 0;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000E630 File Offset: 0x0000D630
		internal void Reset(RegexOptions topopts)
		{
			this._currentPos = 0;
			this._autocap = 1;
			this._ignoreNextParen = false;
			if (this._optionsStack.Count > 0)
			{
				this._optionsStack.RemoveRange(0, this._optionsStack.Count - 1);
			}
			this._options = topopts;
			this._stack = null;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000E688 File Offset: 0x0000D688
		internal RegexNode ScanRegex()
		{
			bool flag = false;
			this.StartGroup(new RegexNode(28, this._options, 0, -1));
			IL_043F:
			while (this.CharsRight() > 0)
			{
				bool flag2 = flag;
				flag = false;
				this.ScanBlank();
				int num = this.Textpos();
				char c;
				if (this.UseOptionX())
				{
					while (this.CharsRight() > 0)
					{
						if (RegexParser.IsStopperX(c = this.RightChar()))
						{
							if (c != '{')
							{
								break;
							}
							if (this.IsTrueQuantifier())
							{
								break;
							}
						}
						this.MoveRight();
					}
				}
				else
				{
					while (this.CharsRight() > 0 && (!RegexParser.IsSpecial(c = this.RightChar()) || (c == '{' && !this.IsTrueQuantifier())))
					{
						this.MoveRight();
					}
				}
				int num2 = this.Textpos();
				this.ScanBlank();
				if (this.CharsRight() == 0)
				{
					c = '!';
				}
				else if (RegexParser.IsSpecial(c = this.RightChar()))
				{
					flag = RegexParser.IsQuantifier(c);
					this.MoveRight();
				}
				else
				{
					c = ' ';
				}
				if (num < num2)
				{
					int num3 = num2 - num - (flag ? 1 : 0);
					flag2 = false;
					if (num3 > 0)
					{
						this.AddConcatenate(num, num3, false);
					}
					if (flag)
					{
						this.AddUnitOne(this.CharAt(num2 - 1));
					}
				}
				char c2 = c;
				if (c2 <= '?')
				{
					switch (c2)
					{
					case ' ':
						continue;
					case '!':
						goto IL_044B;
					case '"':
					case '#':
					case '%':
					case '&':
					case '\'':
					case ',':
					case '-':
						goto IL_02C8;
					case '$':
						this.AddUnitType(this.UseOptionM() ? 15 : 20);
						break;
					case '(':
					{
						this.PushOptions();
						RegexNode regexNode;
						if ((regexNode = this.ScanGroupOpen()) == null)
						{
							this.PopKeepOptions();
							continue;
						}
						this.PushGroup();
						this.StartGroup(regexNode);
						continue;
					}
					case ')':
						if (this.EmptyStack())
						{
							throw this.MakeException(SR.GetString("TooManyParens"));
						}
						this.AddGroup();
						this.PopGroup();
						this.PopOptions();
						if (this.Unit() == null)
						{
							continue;
						}
						break;
					case '*':
					case '+':
						goto IL_0283;
					case '.':
						if (this.UseOptionS())
						{
							this.AddUnitSet("\0\u0001\0\0");
						}
						else
						{
							this.AddUnitNotone('\n');
						}
						break;
					default:
						if (c2 != '?')
						{
							goto IL_02C8;
						}
						goto IL_0283;
					}
				}
				else
				{
					switch (c2)
					{
					case '[':
						this.AddUnitSet(this.ScanCharClass(this.UseOptionI()).ToStringClass());
						break;
					case '\\':
						this.AddUnitNode(this.ScanBackslash());
						break;
					case ']':
						goto IL_02C8;
					case '^':
						this.AddUnitType(this.UseOptionM() ? 14 : 18);
						break;
					default:
						switch (c2)
						{
						case '{':
							goto IL_0283;
						case '|':
							this.AddAlternate();
							continue;
						default:
							goto IL_02C8;
						}
						break;
					}
				}
				IL_02D9:
				this.ScanBlank();
				if (this.CharsRight() == 0 || !(flag = this.IsTrueQuantifier()))
				{
					this.AddConcatenate();
					continue;
				}
				c = this.MoveRightGetChar();
				while (this.Unit() != null)
				{
					char c3 = c;
					int num4;
					int num5;
					switch (c3)
					{
					case '*':
						num4 = 0;
						num5 = int.MaxValue;
						break;
					case '+':
						num4 = 1;
						num5 = int.MaxValue;
						break;
					default:
						if (c3 != '?')
						{
							if (c3 != '{')
							{
								throw this.MakeException(SR.GetString("InternalError"));
							}
							num = this.Textpos();
							num4 = (num5 = this.ScanDecimal());
							if (num < this.Textpos() && this.CharsRight() > 0 && this.RightChar() == ',')
							{
								this.MoveRight();
								if (this.CharsRight() == 0 || this.RightChar() == '}')
								{
									num5 = int.MaxValue;
								}
								else
								{
									num5 = this.ScanDecimal();
								}
							}
							if (num == this.Textpos() || this.CharsRight() == 0 || this.MoveRightGetChar() != '}')
							{
								this.AddConcatenate();
								this.Textto(num - 1);
								goto IL_043F;
							}
						}
						else
						{
							num4 = 0;
							num5 = 1;
						}
						break;
					}
					this.ScanBlank();
					bool flag3;
					if (this.CharsRight() == 0 || this.RightChar() != '?')
					{
						flag3 = false;
					}
					else
					{
						this.MoveRight();
						flag3 = true;
					}
					if (num4 > num5)
					{
						throw this.MakeException(SR.GetString("IllegalRange"));
					}
					this.AddConcatenate(flag3, num4, num5);
				}
				continue;
				IL_0283:
				if (this.Unit() == null)
				{
					throw this.MakeException(flag2 ? SR.GetString("NestedQuantify", new object[] { c.ToString() }) : SR.GetString("QuantifyAfterNothing"));
				}
				this.MoveLeft();
				goto IL_02D9;
				IL_02C8:
				throw this.MakeException(SR.GetString("InternalError"));
			}
			IL_044B:
			if (!this.EmptyStack())
			{
				throw this.MakeException(SR.GetString("NotEnoughParens"));
			}
			this.AddGroup();
			return this.Unit();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000EB08 File Offset: 0x0000DB08
		internal RegexNode ScanReplacement()
		{
			this._concatenation = new RegexNode(25, this._options);
			for (;;)
			{
				int num = this.CharsRight();
				if (num == 0)
				{
					break;
				}
				int num2 = this.Textpos();
				while (num > 0 && this.RightChar() != '$')
				{
					this.MoveRight();
					num--;
				}
				this.AddConcatenate(num2, this.Textpos() - num2, true);
				if (num > 0)
				{
					if (this.MoveRightGetChar() == '$')
					{
						this.AddUnitNode(this.ScanDollar());
					}
					this.AddConcatenate();
				}
			}
			return this._concatenation;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000EB8B File Offset: 0x0000DB8B
		internal RegexCharClass ScanCharClass(bool caseInsensitive)
		{
			return this.ScanCharClass(caseInsensitive, false);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000EB98 File Offset: 0x0000DB98
		internal RegexCharClass ScanCharClass(bool caseInsensitive, bool scanOnly)
		{
			char c = '\0';
			bool flag = false;
			bool flag2 = true;
			bool flag3 = false;
			RegexCharClass regexCharClass = (scanOnly ? null : new RegexCharClass());
			if (this.CharsRight() > 0 && this.RightChar() == '^')
			{
				this.MoveRight();
				if (!scanOnly)
				{
					regexCharClass.Negate = true;
				}
			}
			while (this.CharsRight() > 0)
			{
				bool flag4 = false;
				char c2 = this.MoveRightGetChar();
				if (c2 == ']')
				{
					if (!flag2)
					{
						flag3 = true;
						break;
					}
					goto IL_029F;
				}
				else
				{
					if (c2 == '\\' && this.CharsRight() > 0)
					{
						char c3;
						c2 = (c3 = this.MoveRightGetChar());
						if (c3 <= 'S')
						{
							if (c3 <= 'D')
							{
								if (c3 != '-')
								{
									if (c3 != 'D')
									{
										goto IL_0238;
									}
								}
								else
								{
									if (!scanOnly)
									{
										regexCharClass.AddRange(c2, c2);
										goto IL_03BE;
									}
									goto IL_03BE;
								}
							}
							else
							{
								if (c3 == 'P')
								{
									goto IL_01CB;
								}
								if (c3 != 'S')
								{
									goto IL_0238;
								}
								goto IL_013F;
							}
						}
						else
						{
							if (c3 <= 'd')
							{
								if (c3 != 'W')
								{
									if (c3 != 'd')
									{
										goto IL_0238;
									}
									goto IL_00F3;
								}
							}
							else
							{
								if (c3 == 'p')
								{
									goto IL_01CB;
								}
								if (c3 == 's')
								{
									goto IL_013F;
								}
								if (c3 != 'w')
								{
									goto IL_0238;
								}
							}
							if (scanOnly)
							{
								goto IL_03BE;
							}
							if (flag)
							{
								throw this.MakeException(SR.GetString("BadClassInCharRange", new object[] { c2.ToString() }));
							}
							regexCharClass.AddWord(this.UseOptionE(), c2 == 'W');
							goto IL_03BE;
						}
						IL_00F3:
						if (scanOnly)
						{
							goto IL_03BE;
						}
						if (flag)
						{
							throw this.MakeException(SR.GetString("BadClassInCharRange", new object[] { c2.ToString() }));
						}
						regexCharClass.AddDigit(this.UseOptionE(), c2 == 'D', this._pattern);
						goto IL_03BE;
						IL_013F:
						if (scanOnly)
						{
							goto IL_03BE;
						}
						if (flag)
						{
							throw this.MakeException(SR.GetString("BadClassInCharRange", new object[] { c2.ToString() }));
						}
						regexCharClass.AddSpace(this.UseOptionE(), c2 == 'S');
						goto IL_03BE;
						IL_01CB:
						if (scanOnly)
						{
							this.ParseProperty();
							goto IL_03BE;
						}
						if (flag)
						{
							throw this.MakeException(SR.GetString("BadClassInCharRange", new object[] { c2.ToString() }));
						}
						regexCharClass.AddCategoryFromName(this.ParseProperty(), c2 != 'p', caseInsensitive, this._pattern);
						goto IL_03BE;
						IL_0238:
						this.MoveLeft();
						c2 = this.ScanCharEscape();
						flag4 = true;
						goto IL_029F;
					}
					if (c2 != '[' || this.CharsRight() <= 0 || this.RightChar() != ':' || flag)
					{
						goto IL_029F;
					}
					int num = this.Textpos();
					this.MoveRight();
					this.ScanCapname();
					if (this.CharsRight() < 2 || this.MoveRightGetChar() != ':' || this.MoveRightGetChar() != ']')
					{
						this.Textto(num);
						goto IL_029F;
					}
					goto IL_029F;
				}
				IL_03BE:
				flag2 = false;
				continue;
				IL_029F:
				if (flag)
				{
					flag = false;
					if (scanOnly)
					{
						goto IL_03BE;
					}
					if (c2 == '[' && !flag4 && !flag2)
					{
						regexCharClass.AddChar(c);
						regexCharClass.AddSubtraction(this.ScanCharClass(caseInsensitive, false));
						if (this.CharsRight() > 0 && this.RightChar() != ']')
						{
							throw this.MakeException(SR.GetString("SubtractionMustBeLast"));
						}
						goto IL_03BE;
					}
					else
					{
						if (c > c2)
						{
							throw this.MakeException(SR.GetString("ReversedCharRange"));
						}
						regexCharClass.AddRange(c, c2);
						goto IL_03BE;
					}
				}
				else
				{
					if (this.CharsRight() >= 2 && this.RightChar() == '-' && this.RightChar(1) != ']')
					{
						c = c2;
						flag = true;
						this.MoveRight();
						goto IL_03BE;
					}
					if (this.CharsRight() >= 1 && c2 == '-' && !flag4 && this.RightChar() == '[' && !flag2)
					{
						if (scanOnly)
						{
							this.MoveRight(1);
							this.ScanCharClass(caseInsensitive, true);
							goto IL_03BE;
						}
						this.MoveRight(1);
						regexCharClass.AddSubtraction(this.ScanCharClass(caseInsensitive, false));
						if (this.CharsRight() > 0 && this.RightChar() != ']')
						{
							throw this.MakeException(SR.GetString("SubtractionMustBeLast"));
						}
						goto IL_03BE;
					}
					else
					{
						if (!scanOnly)
						{
							regexCharClass.AddRange(c2, c2);
							goto IL_03BE;
						}
						goto IL_03BE;
					}
				}
			}
			if (!flag3)
			{
				throw this.MakeException(SR.GetString("UnterminatedBracket"));
			}
			if (!scanOnly && caseInsensitive)
			{
				regexCharClass.AddLowercase(this._culture);
			}
			return regexCharClass;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000EF9C File Offset: 0x0000DF9C
		internal RegexNode ScanGroupOpen()
		{
			char c = '>';
			if (this.CharsRight() != 0 && this.RightChar() == '?' && (this.RightChar() != '?' || this.CharsRight() <= 1 || this.RightChar(1) != ')'))
			{
				this.MoveRight();
				if (this.CharsRight() != 0)
				{
					char c2 = this.MoveRightGetChar();
					int num3;
					if (c2 != '!')
					{
						char c3;
						switch (c2)
						{
						case '\'':
							c = '\'';
							break;
						case '(':
						{
							int num = this.Textpos();
							if (this.CharsRight() > 0)
							{
								c3 = this.RightChar();
								if (c3 >= '0' && c3 <= '9')
								{
									int num2 = this.ScanDecimal();
									if (this.CharsRight() <= 0 || this.MoveRightGetChar() != ')')
									{
										throw this.MakeException(SR.GetString("MalformedReference", new object[] { num2.ToString(CultureInfo.CurrentCulture) }));
									}
									if (this.IsCaptureSlot(num2))
									{
										return new RegexNode(33, this._options, num2);
									}
									throw this.MakeException(SR.GetString("UndefinedReference", new object[] { num2.ToString(CultureInfo.CurrentCulture) }));
								}
								else if (RegexCharClass.IsWordChar(c3))
								{
									string text = this.ScanCapname();
									if (this.IsCaptureName(text) && this.CharsRight() > 0 && this.MoveRightGetChar() == ')')
									{
										return new RegexNode(33, this._options, this.CaptureSlotFromName(text));
									}
								}
							}
							num3 = 34;
							this.Textto(num - 1);
							this._ignoreNextParen = true;
							int num4 = this.CharsRight();
							if (num4 < 3 || this.RightChar(1) != '?')
							{
								goto IL_0552;
							}
							char c4 = this.RightChar(2);
							if (c4 == '#')
							{
								throw this.MakeException(SR.GetString("AlternationCantHaveComment"));
							}
							if (c4 == '\'')
							{
								throw this.MakeException(SR.GetString("AlternationCantCapture"));
							}
							if (num4 >= 4 && c4 == '<' && this.RightChar(3) != '!' && this.RightChar(3) != '=')
							{
								throw this.MakeException(SR.GetString("AlternationCantCapture"));
							}
							goto IL_0552;
						}
						default:
							switch (c2)
							{
							case ':':
								num3 = 29;
								goto IL_0552;
							case '<':
								goto IL_0113;
							case '=':
								this._options &= ~RegexOptions.RightToLeft;
								num3 = 30;
								goto IL_0552;
							case '>':
								num3 = 32;
								goto IL_0552;
							}
							this.MoveLeft();
							num3 = 29;
							this.ScanOptions();
							if (this.CharsRight() == 0)
							{
								goto IL_055F;
							}
							if ((c3 = this.MoveRightGetChar()) == ')')
							{
								return null;
							}
							if (c3 == ':')
							{
								goto IL_0552;
							}
							goto IL_055F;
						}
						IL_0113:
						if (this.CharsRight() == 0)
						{
							goto IL_055F;
						}
						char c5;
						c3 = (c5 = this.MoveRightGetChar());
						if (c5 != '!')
						{
							if (c5 == '=')
							{
								if (c == '\'')
								{
									goto IL_055F;
								}
								this._options |= RegexOptions.RightToLeft;
								num3 = 30;
							}
							else
							{
								this.MoveLeft();
								int num5 = -1;
								int num6 = -1;
								bool flag = false;
								if (c3 >= '0' && c3 <= '9')
								{
									num5 = this.ScanDecimal();
									if (!this.IsCaptureSlot(num5))
									{
										num5 = -1;
									}
									if (this.CharsRight() > 0 && this.RightChar() != c && this.RightChar() != '-')
									{
										throw this.MakeException(SR.GetString("InvalidGroupName"));
									}
									if (num5 == 0)
									{
										throw this.MakeException(SR.GetString("CapnumNotZero"));
									}
								}
								else if (RegexCharClass.IsWordChar(c3))
								{
									string text2 = this.ScanCapname();
									if (this.IsCaptureName(text2))
									{
										num5 = this.CaptureSlotFromName(text2);
									}
									if (this.CharsRight() > 0 && this.RightChar() != c && this.RightChar() != '-')
									{
										throw this.MakeException(SR.GetString("InvalidGroupName"));
									}
								}
								else
								{
									if (c3 != '-')
									{
										throw this.MakeException(SR.GetString("InvalidGroupName"));
									}
									flag = true;
								}
								if ((num5 != -1 || flag) && this.CharsRight() > 0 && this.RightChar() == '-')
								{
									this.MoveRight();
									c3 = this.RightChar();
									if (c3 >= '0' && c3 <= '9')
									{
										num6 = this.ScanDecimal();
										if (!this.IsCaptureSlot(num6))
										{
											throw this.MakeException(SR.GetString("UndefinedBackref", new object[] { num6 }));
										}
										if (this.CharsRight() > 0 && this.RightChar() != c)
										{
											throw this.MakeException(SR.GetString("InvalidGroupName"));
										}
									}
									else
									{
										if (!RegexCharClass.IsWordChar(c3))
										{
											throw this.MakeException(SR.GetString("InvalidGroupName"));
										}
										string text3 = this.ScanCapname();
										if (!this.IsCaptureName(text3))
										{
											throw this.MakeException(SR.GetString("UndefinedNameRef", new object[] { text3 }));
										}
										num6 = this.CaptureSlotFromName(text3);
										if (this.CharsRight() > 0 && this.RightChar() != c)
										{
											throw this.MakeException(SR.GetString("InvalidGroupName"));
										}
									}
								}
								if ((num5 != -1 || num6 != -1) && this.CharsRight() > 0 && this.MoveRightGetChar() == c)
								{
									return new RegexNode(28, this._options, num5, num6);
								}
								goto IL_055F;
							}
						}
						else
						{
							if (c == '\'')
							{
								goto IL_055F;
							}
							this._options |= RegexOptions.RightToLeft;
							num3 = 31;
						}
					}
					else
					{
						this._options &= ~RegexOptions.RightToLeft;
						num3 = 31;
					}
					IL_0552:
					return new RegexNode(num3, this._options);
				}
				IL_055F:
				throw this.MakeException(SR.GetString("UnrecognizedGrouping"));
			}
			if (this.UseOptionN() || this._ignoreNextParen)
			{
				this._ignoreNextParen = false;
				return new RegexNode(29, this._options);
			}
			return new RegexNode(28, this._options, this._autocap++, -1);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000F518 File Offset: 0x0000E518
		internal void ScanBlank()
		{
			if (this.UseOptionX())
			{
				for (;;)
				{
					if (this.CharsRight() <= 0 || !RegexParser.IsSpace(this.RightChar()))
					{
						if (this.CharsRight() == 0)
						{
							break;
						}
						if (this.RightChar() == '#')
						{
							while (this.CharsRight() > 0)
							{
								if (this.RightChar() == '\n')
								{
									break;
								}
								this.MoveRight();
							}
						}
						else
						{
							if (this.CharsRight() < 3 || this.RightChar(2) != '#' || this.RightChar(1) != '?' || this.RightChar() != '(')
							{
								return;
							}
							while (this.CharsRight() > 0 && this.RightChar() != ')')
							{
								this.MoveRight();
							}
							if (this.CharsRight() == 0)
							{
								goto Block_12;
							}
							this.MoveRight();
						}
					}
					else
					{
						this.MoveRight();
					}
				}
				return;
				Block_12:
				throw this.MakeException(SR.GetString("UnterminatedComment"));
			}
			while (this.CharsRight() >= 3 && this.RightChar(2) == '#' && this.RightChar(1) == '?' && this.RightChar() == '(')
			{
				while (this.CharsRight() > 0 && this.RightChar() != ')')
				{
					this.MoveRight();
				}
				if (this.CharsRight() == 0)
				{
					throw this.MakeException(SR.GetString("UnterminatedComment"));
				}
				this.MoveRight();
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000F658 File Offset: 0x0000E658
		internal RegexNode ScanBackslash()
		{
			if (this.CharsRight() == 0)
			{
				throw this.MakeException(SR.GetString("IllegalEndEscape"));
			}
			char c2;
			char c = (c2 = this.RightChar());
			if (c2 <= 'Z')
			{
				if (c2 <= 'P')
				{
					switch (c2)
					{
					case 'A':
					case 'B':
					case 'G':
						break;
					case 'C':
					case 'E':
					case 'F':
						goto IL_0259;
					case 'D':
						this.MoveRight();
						if (this.UseOptionE())
						{
							return new RegexNode(11, this._options, "\u0001\u0002\00:");
						}
						return new RegexNode(11, this._options, RegexCharClass.NotDigitClass);
					default:
						if (c2 != 'P')
						{
							goto IL_0259;
						}
						goto IL_0205;
					}
				}
				else if (c2 != 'S')
				{
					if (c2 != 'W')
					{
						if (c2 != 'Z')
						{
							goto IL_0259;
						}
					}
					else
					{
						this.MoveRight();
						if (this.UseOptionE())
						{
							return new RegexNode(11, this._options, "\u0001\n\00:A[_`a{İı");
						}
						return new RegexNode(11, this._options, RegexCharClass.NotWordClass);
					}
				}
				else
				{
					this.MoveRight();
					if (this.UseOptionE())
					{
						return new RegexNode(11, this._options, "\u0001\u0004\0\t\u000e !");
					}
					return new RegexNode(11, this._options, RegexCharClass.NotSpaceClass);
				}
			}
			else if (c2 <= 'p')
			{
				switch (c2)
				{
				case 'b':
					break;
				case 'c':
					goto IL_0259;
				case 'd':
					this.MoveRight();
					if (this.UseOptionE())
					{
						return new RegexNode(11, this._options, "\0\u0002\00:");
					}
					return new RegexNode(11, this._options, RegexCharClass.DigitClass);
				default:
					if (c2 != 'p')
					{
						goto IL_0259;
					}
					goto IL_0205;
				}
			}
			else if (c2 != 's')
			{
				if (c2 != 'w')
				{
					if (c2 != 'z')
					{
						goto IL_0259;
					}
				}
				else
				{
					this.MoveRight();
					if (this.UseOptionE())
					{
						return new RegexNode(11, this._options, "\0\n\00:A[_`a{İı");
					}
					return new RegexNode(11, this._options, RegexCharClass.WordClass);
				}
			}
			else
			{
				this.MoveRight();
				if (this.UseOptionE())
				{
					return new RegexNode(11, this._options, "\0\u0004\0\t\u000e !");
				}
				return new RegexNode(11, this._options, RegexCharClass.SpaceClass);
			}
			this.MoveRight();
			return new RegexNode(this.TypeFromCode(c), this._options);
			IL_0205:
			this.MoveRight();
			RegexCharClass regexCharClass = new RegexCharClass();
			regexCharClass.AddCategoryFromName(this.ParseProperty(), c != 'p', this.UseOptionI(), this._pattern);
			if (this.UseOptionI())
			{
				regexCharClass.AddLowercase(this._culture);
			}
			return new RegexNode(11, this._options, regexCharClass.ToStringClass());
			IL_0259:
			return this.ScanBasicBackslash();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000F8C4 File Offset: 0x0000E8C4
		internal RegexNode ScanBasicBackslash()
		{
			if (this.CharsRight() == 0)
			{
				throw this.MakeException(SR.GetString("IllegalEndEscape"));
			}
			bool flag = false;
			char c = '\0';
			int num = this.Textpos();
			char c2 = this.RightChar();
			if (c2 == 'k')
			{
				if (this.CharsRight() >= 2)
				{
					this.MoveRight();
					c2 = this.MoveRightGetChar();
					if (c2 == '<' || c2 == '\'')
					{
						flag = true;
						c = ((c2 == '\'') ? '\'' : '>');
					}
				}
				if (!flag || this.CharsRight() <= 0)
				{
					throw this.MakeException(SR.GetString("MalformedNameRef"));
				}
				c2 = this.RightChar();
			}
			else if ((c2 == '<' || c2 == '\'') && this.CharsRight() > 1)
			{
				flag = true;
				c = ((c2 == '\'') ? '\'' : '>');
				this.MoveRight();
				c2 = this.RightChar();
			}
			if (flag && c2 >= '0' && c2 <= '9')
			{
				int num2 = this.ScanDecimal();
				if (this.CharsRight() > 0 && this.MoveRightGetChar() == c)
				{
					if (this.IsCaptureSlot(num2))
					{
						return new RegexNode(13, this._options, num2);
					}
					throw this.MakeException(SR.GetString("UndefinedBackref", new object[] { num2.ToString(CultureInfo.CurrentCulture) }));
				}
			}
			else if (!flag && c2 >= '1' && c2 <= '9')
			{
				if (this.UseOptionE())
				{
					int num3 = -1;
					int i = (int)(c2 - '0');
					int num4 = this.Textpos() - 1;
					while (i <= this._captop)
					{
						if (this.IsCaptureSlot(i) && (this._caps == null || (int)this._caps[i] < num4))
						{
							num3 = i;
						}
						this.MoveRight();
						if (this.CharsRight() == 0 || (c2 = this.RightChar()) < '0' || c2 > '9')
						{
							break;
						}
						i = i * 10 + (int)(c2 - '0');
					}
					if (num3 >= 0)
					{
						return new RegexNode(13, this._options, num3);
					}
				}
				else
				{
					int num5 = this.ScanDecimal();
					if (this.IsCaptureSlot(num5))
					{
						return new RegexNode(13, this._options, num5);
					}
					if (num5 <= 9)
					{
						throw this.MakeException(SR.GetString("UndefinedBackref", new object[] { num5.ToString(CultureInfo.CurrentCulture) }));
					}
				}
			}
			else if (flag && RegexCharClass.IsWordChar(c2))
			{
				string text = this.ScanCapname();
				if (this.CharsRight() > 0 && this.MoveRightGetChar() == c)
				{
					if (this.IsCaptureName(text))
					{
						return new RegexNode(13, this._options, this.CaptureSlotFromName(text));
					}
					throw this.MakeException(SR.GetString("UndefinedNameRef", new object[] { text }));
				}
			}
			this.Textto(num);
			c2 = this.ScanCharEscape();
			if (this.UseOptionI())
			{
				c2 = char.ToLower(c2, this._culture);
			}
			return new RegexNode(9, this._options, c2);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000FB98 File Offset: 0x0000EB98
		internal RegexNode ScanDollar()
		{
			/*
An exception occurred when decompiling this method (060001E9)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Text.RegularExpressions.RegexNode System.Text.RegularExpressions.RegexParser::ScanDollar()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.VariableSlot.CloneVariableState(VariableSlot[] state) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 78
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 407
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000FDF4 File Offset: 0x0000EDF4
		internal string ScanCapname()
		{
			int num = this.Textpos();
			while (this.CharsRight() > 0)
			{
				if (!RegexCharClass.IsWordChar(this.MoveRightGetChar()))
				{
					this.MoveLeft();
					break;
				}
			}
			return this._pattern.Substring(num, this.Textpos() - num);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000FE3C File Offset: 0x0000EE3C
		internal char ScanOctal()
		{
			int num = 3;
			if (num > this.CharsRight())
			{
				num = this.CharsRight();
			}
			int num2 = 0;
			int num3;
			while (num > 0 && (num3 = (int)(this.RightChar() - '0')) <= 7)
			{
				this.MoveRight();
				num2 *= 8;
				num2 += num3;
				if (this.UseOptionE() && num2 >= 32)
				{
					break;
				}
				num--;
			}
			num2 &= 255;
			return (char)num2;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000FE9C File Offset: 0x0000EE9C
		internal int ScanDecimal()
		{
			int num = 0;
			int num2;
			while (this.CharsRight() > 0 && (num2 = (int)((ushort)(this.RightChar() - '0'))) <= 9)
			{
				this.MoveRight();
				if (num > 214748364 || (num == 214748364 && num2 > 7))
				{
					throw this.MakeException(SR.GetString("CaptureGroupOutOfRange"));
				}
				num *= 10;
				num += num2;
			}
			return num;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000FEFC File Offset: 0x0000EEFC
		internal char ScanHex(int c)
		{
			int num = 0;
			if (this.CharsRight() >= c)
			{
				int num2;
				while (c > 0 && (num2 = RegexParser.HexDigit(this.MoveRightGetChar())) >= 0)
				{
					num *= 16;
					num += num2;
					c--;
				}
			}
			if (c > 0)
			{
				throw this.MakeException(SR.GetString("TooFewHex"));
			}
			return (char)num;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000FF50 File Offset: 0x0000EF50
		internal static int HexDigit(char ch)
		{
			int num;
			if ((num = (int)(ch - '0')) <= 9)
			{
				return num;
			}
			if ((num = (int)(ch - 'a')) <= 5)
			{
				return num + 10;
			}
			if ((num = (int)(ch - 'A')) <= 5)
			{
				return num + 10;
			}
			return -1;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000FF88 File Offset: 0x0000EF88
		internal char ScanControl()
		{
			if (this.CharsRight() <= 0)
			{
				throw this.MakeException(SR.GetString("MissingControl"));
			}
			char c = this.MoveRightGetChar();
			if (c >= 'a' && c <= 'z')
			{
				c -= ' ';
			}
			if ((c -= '@') < ' ')
			{
				return c;
			}
			throw this.MakeException(SR.GetString("UnrecognizedControl"));
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000FFE3 File Offset: 0x0000EFE3
		internal bool IsOnlyTopOption(RegexOptions option)
		{
			return option == RegexOptions.RightToLeft || option == RegexOptions.Compiled || option == RegexOptions.CultureInvariant || option == RegexOptions.ECMAScript;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00010000 File Offset: 0x0000F000
		internal void ScanOptions()
		{
			bool flag = false;
			while (this.CharsRight() > 0)
			{
				char c = this.RightChar();
				if (c == '-')
				{
					flag = true;
				}
				else if (c == '+')
				{
					flag = false;
				}
				else
				{
					RegexOptions regexOptions = RegexParser.OptionFromCode(c);
					if (regexOptions == RegexOptions.None || this.IsOnlyTopOption(regexOptions))
					{
						return;
					}
					if (flag)
					{
						this._options &= ~regexOptions;
					}
					else
					{
						this._options |= regexOptions;
					}
				}
				this.MoveRight();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00010070 File Offset: 0x0000F070
		internal char ScanCharEscape()
		{
			char c = this.MoveRightGetChar();
			if (c >= '0' && c <= '7')
			{
				this.MoveLeft();
				return this.ScanOctal();
			}
			char c2 = c;
			switch (c2)
			{
			case 'a':
				return '\a';
			case 'b':
				return '\b';
			case 'c':
				return this.ScanControl();
			case 'd':
				break;
			case 'e':
				return '\u001b';
			case 'f':
				return '\f';
			default:
				switch (c2)
				{
				case 'n':
					return '\n';
				case 'r':
					return '\r';
				case 't':
					return '\t';
				case 'u':
					return this.ScanHex(4);
				case 'v':
					return '\v';
				case 'x':
					return this.ScanHex(2);
				}
				break;
			}
			if (!this.UseOptionE() && RegexCharClass.IsWordChar(c))
			{
				throw this.MakeException(SR.GetString("UnrecognizedEscape", new object[] { c.ToString() }));
			}
			return c;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00010158 File Offset: 0x0000F158
		internal string ParseProperty()
		{
			if (this.CharsRight() < 3)
			{
				throw this.MakeException(SR.GetString("IncompleteSlashP"));
			}
			char c = this.MoveRightGetChar();
			if (c != '{')
			{
				throw this.MakeException(SR.GetString("MalformedSlashP"));
			}
			int num = this.Textpos();
			while (this.CharsRight() > 0)
			{
				c = this.MoveRightGetChar();
				if (!RegexCharClass.IsWordChar(c) && c != '-')
				{
					this.MoveLeft();
					break;
				}
			}
			string text = this._pattern.Substring(num, this.Textpos() - num);
			if (this.CharsRight() == 0 || this.MoveRightGetChar() != '}')
			{
				throw this.MakeException(SR.GetString("IncompleteSlashP"));
			}
			return text;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00010204 File Offset: 0x0000F204
		internal int TypeFromCode(char ch)
		{
			if (ch <= 'G')
			{
				switch (ch)
				{
				case 'A':
					return 18;
				case 'B':
					if (!this.UseOptionE())
					{
						return 17;
					}
					return 42;
				default:
					if (ch == 'G')
					{
						return 19;
					}
					break;
				}
			}
			else
			{
				if (ch == 'Z')
				{
					return 20;
				}
				if (ch != 'b')
				{
					if (ch == 'z')
					{
						return 21;
					}
				}
				else
				{
					if (!this.UseOptionE())
					{
						return 16;
					}
					return 41;
				}
			}
			return 22;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0001026C File Offset: 0x0000F26C
		internal static RegexOptions OptionFromCode(char ch)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				ch += ' ';
			}
			char c = ch;
			if (c <= 'i')
			{
				switch (c)
				{
				case 'c':
					return RegexOptions.Compiled;
				case 'd':
					break;
				case 'e':
					return RegexOptions.ECMAScript;
				default:
					if (c == 'i')
					{
						return RegexOptions.IgnoreCase;
					}
					break;
				}
			}
			else
			{
				switch (c)
				{
				case 'm':
					return RegexOptions.Multiline;
				case 'n':
					return RegexOptions.ExplicitCapture;
				case 'o':
				case 'p':
				case 'q':
					break;
				case 'r':
					return RegexOptions.RightToLeft;
				case 's':
					return RegexOptions.Singleline;
				default:
					if (c == 'x')
					{
						return RegexOptions.IgnorePatternWhitespace;
					}
					break;
				}
			}
			return RegexOptions.None;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000102F4 File Offset: 0x0000F2F4
		internal void CountCaptures()
		{
			this.NoteCaptureSlot(0, 0);
			this._autocap = 1;
			while (this.CharsRight() > 0)
			{
				int num = this.Textpos();
				char c = this.MoveRightGetChar();
				char c2 = c;
				if (c2 != '#')
				{
					switch (c2)
					{
					case '(':
						if (this.CharsRight() >= 2 && this.RightChar(1) == '#' && this.RightChar() == '?')
						{
							this.MoveLeft();
							this.ScanBlank();
						}
						else
						{
							this.PushOptions();
							if (this.CharsRight() > 0 && this.RightChar() == '?')
							{
								this.MoveRight();
								if (this.CharsRight() > 1 && (this.RightChar() == '<' || this.RightChar() == '\''))
								{
									this.MoveRight();
									c = this.RightChar();
									if (c != '0' && RegexCharClass.IsWordChar(c))
									{
										if (c >= '1' && c <= '9')
										{
											this.NoteCaptureSlot(this.ScanDecimal(), num);
										}
										else
										{
											this.NoteCaptureName(this.ScanCapname(), num);
										}
									}
								}
								else
								{
									this.ScanOptions();
									if (this.CharsRight() > 0)
									{
										if (this.RightChar() == ')')
										{
											this.MoveRight();
											this.PopKeepOptions();
										}
										else if (this.RightChar() == '(')
										{
											this._ignoreNextParen = true;
											break;
										}
									}
								}
							}
							else if (!this.UseOptionN() && !this._ignoreNextParen)
							{
								this.NoteCaptureSlot(this._autocap++, num);
							}
						}
						this._ignoreNextParen = false;
						break;
					case ')':
						if (!this.EmptyOptionsStack())
						{
							this.PopOptions();
						}
						break;
					default:
						switch (c2)
						{
						case '[':
							this.ScanCharClass(false, true);
							break;
						case '\\':
							if (this.CharsRight() > 0)
							{
								this.MoveRight();
							}
							break;
						}
						break;
					}
				}
				else if (this.UseOptionX())
				{
					this.MoveLeft();
					this.ScanBlank();
				}
			}
			this.AssignNameSlots();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000104DC File Offset: 0x0000F4DC
		internal void NoteCaptureSlot(int i, int pos)
		{
			if (!this._caps.ContainsKey(i))
			{
				this._caps.Add(i, pos);
				this._capcount++;
				if (this._captop <= i)
				{
					if (i == 2147483647)
					{
						this._captop = i;
						return;
					}
					this._captop = i + 1;
				}
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00010544 File Offset: 0x0000F544
		internal void NoteCaptureName(string name, int pos)
		{
			if (this._capnames == null)
			{
				this._capnames = new Hashtable();
				this._capnamelist = new ArrayList();
			}
			if (!this._capnames.ContainsKey(name))
			{
				this._capnames.Add(name, pos);
				this._capnamelist.Add(name);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0001059C File Offset: 0x0000F59C
		internal void NoteCaptures(Hashtable caps, int capsize, Hashtable capnames)
		{
			this._caps = caps;
			this._capsize = capsize;
			this._capnames = capnames;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000105B4 File Offset: 0x0000F5B4
		internal void AssignNameSlots()
		{
			if (this._capnames != null)
			{
				for (int i = 0; i < this._capnamelist.Count; i++)
				{
					while (this.IsCaptureSlot(this._autocap))
					{
						this._autocap++;
					}
					string text = (string)this._capnamelist[i];
					int num = (int)this._capnames[text];
					this._capnames[text] = this._autocap;
					this.NoteCaptureSlot(this._autocap, num);
					this._autocap++;
				}
			}
			if (this._capcount < this._captop)
			{
				this._capnumlist = new object[this._capcount];
				int num2 = 0;
				IDictionaryEnumerator enumerator = this._caps.GetEnumerator();
				while (enumerator.MoveNext())
				{
					this._capnumlist[num2++] = enumerator.Key;
				}
				Array.Sort(this._capnumlist, InvariantComparer.Default);
			}
			if (this._capnames != null || this._capnumlist != null)
			{
				int num3 = 0;
				ArrayList arrayList;
				int num4;
				if (this._capnames == null)
				{
					arrayList = null;
					this._capnames = new Hashtable();
					this._capnamelist = new ArrayList();
					num4 = -1;
				}
				else
				{
					arrayList = this._capnamelist;
					this._capnamelist = new ArrayList();
					num4 = (int)this._capnames[arrayList[0]];
				}
				for (int j = 0; j < this._capcount; j++)
				{
					int num5 = ((this._capnumlist == null) ? j : ((int)this._capnumlist[j]));
					if (num4 == num5)
					{
						this._capnamelist.Add((string)arrayList[num3++]);
						num4 = ((num3 == arrayList.Count) ? (-1) : ((int)this._capnames[arrayList[num3]]));
					}
					else
					{
						string text2 = Convert.ToString(num5, this._culture);
						this._capnamelist.Add(text2);
						this._capnames[text2] = num5;
					}
				}
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000107D1 File Offset: 0x0000F7D1
		internal int CaptureSlotFromName(string capname)
		{
			return (int)this._capnames[capname];
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000107E4 File Offset: 0x0000F7E4
		internal bool IsCaptureSlot(int i)
		{
			if (this._caps != null)
			{
				return this._caps.ContainsKey(i);
			}
			return i >= 0 && i < this._capsize;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0001080F File Offset: 0x0000F80F
		internal bool IsCaptureName(string capname)
		{
			return this._capnames != null && this._capnames.ContainsKey(capname);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00010827 File Offset: 0x0000F827
		internal bool UseOptionN()
		{
			return (this._options & RegexOptions.ExplicitCapture) != RegexOptions.None;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00010837 File Offset: 0x0000F837
		internal bool UseOptionI()
		{
			return (this._options & RegexOptions.IgnoreCase) != RegexOptions.None;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00010847 File Offset: 0x0000F847
		internal bool UseOptionM()
		{
			return (this._options & RegexOptions.Multiline) != RegexOptions.None;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00010857 File Offset: 0x0000F857
		internal bool UseOptionS()
		{
			return (this._options & RegexOptions.Singleline) != RegexOptions.None;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00010868 File Offset: 0x0000F868
		internal bool UseOptionX()
		{
			return (this._options & RegexOptions.IgnorePatternWhitespace) != RegexOptions.None;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00010879 File Offset: 0x0000F879
		internal bool UseOptionE()
		{
			return (this._options & RegexOptions.ECMAScript) != RegexOptions.None;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0001088D File Offset: 0x0000F88D
		internal static bool IsSpecial(char ch)
		{
			return ch <= '|' && RegexParser._category[(int)ch] >= 4;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000108A3 File Offset: 0x0000F8A3
		internal static bool IsStopperX(char ch)
		{
			return ch <= '|' && RegexParser._category[(int)ch] >= 2;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000108B9 File Offset: 0x0000F8B9
		internal static bool IsQuantifier(char ch)
		{
			return ch <= '{' && RegexParser._category[(int)ch] >= 5;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000108D0 File Offset: 0x0000F8D0
		internal bool IsTrueQuantifier()
		{
			int num = this.CharsRight();
			if (num == 0)
			{
				return false;
			}
			int num2 = this.Textpos();
			char c = this.CharAt(num2);
			if (c != '{')
			{
				return c <= '{' && RegexParser._category[(int)c] >= 5;
			}
			int num3 = num2;
			while (--num > 0 && (c = this.CharAt(++num3)) >= '0' && c <= '9')
			{
			}
			if (num == 0 || num3 - num2 == 1)
			{
				return false;
			}
			if (c == '}')
			{
				return true;
			}
			if (c != ',')
			{
				return false;
			}
			while (--num > 0 && (c = this.CharAt(++num3)) >= '0' && c <= '9')
			{
			}
			return num > 0 && c == '}';
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00010974 File Offset: 0x0000F974
		internal static bool IsSpace(char ch)
		{
			return ch <= ' ' && RegexParser._category[(int)ch] == 2;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00010987 File Offset: 0x0000F987
		internal static bool IsMetachar(char ch)
		{
			return ch <= '|' && RegexParser._category[(int)ch] >= 1;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000109A0 File Offset: 0x0000F9A0
		internal void AddConcatenate(int pos, int cch, bool isReplacement)
		{
			if (cch == 0)
			{
				return;
			}
			RegexNode regexNode;
			if (cch > 1)
			{
				string text = this._pattern.Substring(pos, cch);
				if (this.UseOptionI() && !isReplacement)
				{
					StringBuilder stringBuilder = new StringBuilder(text.Length);
					for (int i = 0; i < text.Length; i++)
					{
						stringBuilder.Append(char.ToLower(text[i], this._culture));
					}
					text = stringBuilder.ToString();
				}
				regexNode = new RegexNode(12, this._options, text);
			}
			else
			{
				char c = this._pattern[pos];
				if (this.UseOptionI() && !isReplacement)
				{
					c = char.ToLower(c, this._culture);
				}
				regexNode = new RegexNode(9, this._options, c);
			}
			this._concatenation.AddChild(regexNode);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00010A60 File Offset: 0x0000FA60
		internal void PushGroup()
		{
			this._group._next = this._stack;
			this._alternation._next = this._group;
			this._concatenation._next = this._alternation;
			this._stack = this._concatenation;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00010AAC File Offset: 0x0000FAAC
		internal void PopGroup()
		{
			this._concatenation = this._stack;
			this._alternation = this._concatenation._next;
			this._group = this._alternation._next;
			this._stack = this._group._next;
			if (this._group.Type() == 34 && this._group.ChildCount() == 0)
			{
				if (this._unit == null)
				{
					throw this.MakeException(SR.GetString("IllegalCondition"));
				}
				this._group.AddChild(this._unit);
				this._unit = null;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00010B45 File Offset: 0x0000FB45
		internal bool EmptyStack()
		{
			return this._stack == null;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00010B50 File Offset: 0x0000FB50
		internal void StartGroup(RegexNode openGroup)
		{
			this._group = openGroup;
			this._alternation = new RegexNode(24, this._options);
			this._concatenation = new RegexNode(25, this._options);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00010B80 File Offset: 0x0000FB80
		internal void AddAlternate()
		{
			if (this._group.Type() == 34 || this._group.Type() == 33)
			{
				this._group.AddChild(this._concatenation.ReverseLeft());
			}
			else
			{
				this._alternation.AddChild(this._concatenation.ReverseLeft());
			}
			this._concatenation = new RegexNode(25, this._options);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00010BEC File Offset: 0x0000FBEC
		internal void AddConcatenate()
		{
			this._concatenation.AddChild(this._unit);
			this._unit = null;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00010C06 File Offset: 0x0000FC06
		internal void AddConcatenate(bool lazy, int min, int max)
		{
			this._concatenation.AddChild(this._unit.MakeQuantifier(lazy, min, max));
			this._unit = null;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00010C28 File Offset: 0x0000FC28
		internal RegexNode Unit()
		{
			return this._unit;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00010C30 File Offset: 0x0000FC30
		internal void AddUnitOne(char ch)
		{
			if (this.UseOptionI())
			{
				ch = char.ToLower(ch, this._culture);
			}
			this._unit = new RegexNode(9, this._options, ch);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00010C5C File Offset: 0x0000FC5C
		internal void AddUnitNotone(char ch)
		{
			if (this.UseOptionI())
			{
				ch = char.ToLower(ch, this._culture);
			}
			this._unit = new RegexNode(10, this._options, ch);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00010C88 File Offset: 0x0000FC88
		internal void AddUnitSet(string cc)
		{
			this._unit = new RegexNode(11, this._options, cc);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00010C9E File Offset: 0x0000FC9E
		internal void AddUnitNode(RegexNode node)
		{
			this._unit = node;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00010CA7 File Offset: 0x0000FCA7
		internal void AddUnitType(int type)
		{
			this._unit = new RegexNode(type, this._options);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00010CBC File Offset: 0x0000FCBC
		internal void AddGroup()
		{
			if (this._group.Type() == 34 || this._group.Type() == 33)
			{
				this._group.AddChild(this._concatenation.ReverseLeft());
				if ((this._group.Type() == 33 && this._group.ChildCount() > 2) || this._group.ChildCount() > 3)
				{
					throw this.MakeException(SR.GetString("TooManyAlternates"));
				}
			}
			else
			{
				this._alternation.AddChild(this._concatenation.ReverseLeft());
				this._group.AddChild(this._alternation);
			}
			this._unit = this._group;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00010D6C File Offset: 0x0000FD6C
		internal void PushOptions()
		{
			this._optionsStack.Add(this._options);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00010D85 File Offset: 0x0000FD85
		internal void PopOptions()
		{
			this._options = (RegexOptions)this._optionsStack[this._optionsStack.Count - 1];
			this._optionsStack.RemoveAt(this._optionsStack.Count - 1);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00010DC2 File Offset: 0x0000FDC2
		internal bool EmptyOptionsStack()
		{
			return this._optionsStack.Count == 0;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00010DD2 File Offset: 0x0000FDD2
		internal void PopKeepOptions()
		{
			this._optionsStack.RemoveAt(this._optionsStack.Count - 1);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00010DEC File Offset: 0x0000FDEC
		internal ArgumentException MakeException(string message)
		{
			return new ArgumentException(SR.GetString("MakeException", new object[] { this._pattern, message }));
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00010E1D File Offset: 0x0000FE1D
		internal int Textpos()
		{
			return this._currentPos;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00010E25 File Offset: 0x0000FE25
		internal void Textto(int pos)
		{
			this._currentPos = pos;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00010E30 File Offset: 0x0000FE30
		internal char MoveRightGetChar()
		{
			return this._pattern[this._currentPos++];
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00010E59 File Offset: 0x0000FE59
		internal void MoveRight()
		{
			this.MoveRight(1);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00010E62 File Offset: 0x0000FE62
		internal void MoveRight(int i)
		{
			this._currentPos += i;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00010E72 File Offset: 0x0000FE72
		internal void MoveLeft()
		{
			this._currentPos--;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00010E82 File Offset: 0x0000FE82
		internal char CharAt(int i)
		{
			return this._pattern[i];
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00010E90 File Offset: 0x0000FE90
		internal char RightChar()
		{
			return this._pattern[this._currentPos];
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00010EA3 File Offset: 0x0000FEA3
		internal char RightChar(int i)
		{
			return this._pattern[this._currentPos + i];
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010EB8 File Offset: 0x0000FEB8
		internal int CharsRight()
		{
			return this._pattern.Length - this._currentPos;
		}

		// Token: 0x04000798 RID: 1944
		internal const int MaxValueDiv10 = 214748364;

		// Token: 0x04000799 RID: 1945
		internal const int MaxValueMod10 = 7;

		// Token: 0x0400079A RID: 1946
		internal const byte Q = 5;

		// Token: 0x0400079B RID: 1947
		internal const byte S = 4;

		// Token: 0x0400079C RID: 1948
		internal const byte Z = 3;

		// Token: 0x0400079D RID: 1949
		internal const byte X = 2;

		// Token: 0x0400079E RID: 1950
		internal const byte E = 1;

		// Token: 0x0400079F RID: 1951
		internal RegexNode _stack;

		// Token: 0x040007A0 RID: 1952
		internal RegexNode _group;

		// Token: 0x040007A1 RID: 1953
		internal RegexNode _alternation;

		// Token: 0x040007A2 RID: 1954
		internal RegexNode _concatenation;

		// Token: 0x040007A3 RID: 1955
		internal RegexNode _unit;

		// Token: 0x040007A4 RID: 1956
		internal string _pattern;

		// Token: 0x040007A5 RID: 1957
		internal int _currentPos;

		// Token: 0x040007A6 RID: 1958
		internal CultureInfo _culture;

		// Token: 0x040007A7 RID: 1959
		internal int _autocap;

		// Token: 0x040007A8 RID: 1960
		internal int _capcount;

		// Token: 0x040007A9 RID: 1961
		internal int _captop;

		// Token: 0x040007AA RID: 1962
		internal int _capsize;

		// Token: 0x040007AB RID: 1963
		internal Hashtable _caps;

		// Token: 0x040007AC RID: 1964
		internal Hashtable _capnames;

		// Token: 0x040007AD RID: 1965
		internal object[] _capnumlist;

		// Token: 0x040007AE RID: 1966
		internal ArrayList _capnamelist;

		// Token: 0x040007AF RID: 1967
		internal RegexOptions _options;

		// Token: 0x040007B0 RID: 1968
		internal ArrayList _optionsStack;

		// Token: 0x040007B1 RID: 1969
		internal bool _ignoreNextParen;

		// Token: 0x040007B2 RID: 1970
		internal static readonly byte[] _category = new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
			2, 0, 2, 2, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 2, 0, 0, 3, 4, 0, 0, 0,
			4, 4, 5, 5, 0, 0, 4, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 4, 4, 0, 4, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 5, 4, 0, 0, 0
		};
	}
}
