using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000C4 RID: 196
	public sealed class JSScanner
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x00041E54 File Offset: 0x00040E54
		public JSScanner()
		{
			this.keywords = JSScanner.s_Keywords;
			this.strSourceCode = null;
			this.startPos = 0;
			this.endPos = 0;
			this.currentPos = 0;
			this.currentLine = 1;
			this.startLinePos = 0;
			this.currentToken = null;
			this.escapedString = null;
			this.identifier = new StringBuilder(128);
			this.idLastPosOnBuilder = 0;
			this.gotEndOfLine = false;
			this.IsAuthoring = false;
			this.peekModeOn = false;
			this.preProcessorOn = false;
			this.matchIf = 0;
			this.ppTable = null;
			this.currentDocument = null;
			this.globals = null;
			this.scanForDebugger = false;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00041F00 File Offset: 0x00040F00
		public JSScanner(Context sourceContext)
		{
			this.IsAuthoring = false;
			this.peekModeOn = false;
			this.keywords = JSScanner.s_Keywords;
			this.preProcessorOn = false;
			this.matchIf = 0;
			this.ppTable = null;
			this.SetSource(sourceContext);
			this.currentDocument = null;
			this.globals = sourceContext.document.engine.Globals;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00041F65 File Offset: 0x00040F65
		public void SetAuthoringMode(bool mode)
		{
			this.IsAuthoring = mode;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00041F70 File Offset: 0x00040F70
		public void SetSource(Context sourceContext)
		{
			this.strSourceCode = sourceContext.source_string;
			this.startPos = sourceContext.startPos;
			this.startLinePos = sourceContext.startLinePos;
			this.endPos = ((0 < sourceContext.endPos && sourceContext.endPos < this.strSourceCode.Length) ? sourceContext.endPos : this.strSourceCode.Length);
			this.currentToken = sourceContext;
			this.escapedString = null;
			this.identifier = new StringBuilder(128);
			this.idLastPosOnBuilder = 0;
			this.currentPos = this.startPos;
			this.currentLine = ((sourceContext.lineNumber > 0) ? sourceContext.lineNumber : 1);
			this.gotEndOfLine = false;
			this.scanForDebugger = sourceContext.document != null && sourceContext.document.engine != null && VsaEngine.executeForJSEE;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0004204C File Offset: 0x0004104C
		internal JSToken PeekToken()
		{
			int num = this.currentPos;
			int num2 = this.currentLine;
			int num3 = this.startLinePos;
			bool flag = this.gotEndOfLine;
			int num4 = this.idLastPosOnBuilder;
			this.peekModeOn = true;
			JSToken jstoken = JSToken.None;
			Context context = this.currentToken;
			this.currentToken = this.currentToken.Clone();
			try
			{
				this.GetNextToken();
				jstoken = this.currentToken.token;
			}
			finally
			{
				this.currentToken = context;
				this.currentPos = num;
				this.currentLine = num2;
				this.startLinePos = num3;
				this.gotEndOfLine = flag;
				this.identifier.Length = 0;
				this.idLastPosOnBuilder = num4;
				this.peekModeOn = false;
				this.escapedString = null;
			}
			return jstoken;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00042110 File Offset: 0x00041110
		public void GetNextToken()
		{
			JSToken jstoken = JSToken.None;
			this.gotEndOfLine = false;
			try
			{
				int num = this.currentLine;
				char c;
				int num2;
				for (;;)
				{
					this.SkipBlanks();
					this.currentToken.startPos = this.currentPos;
					this.currentToken.lineNumber = this.currentLine;
					this.currentToken.startLinePos = this.startLinePos;
					c = this.GetChar(this.currentPos++);
					char c2 = c;
					if (c2 <= '\r')
					{
						if (c2 != '\0')
						{
							if (c2 == '\n')
							{
								this.currentLine++;
								this.startLinePos = this.currentPos;
								continue;
							}
							if (c2 == '\r')
							{
								if (this.GetChar(this.currentPos) == '\n')
								{
									this.currentPos++;
								}
								this.currentLine++;
								this.startLinePos = this.currentPos;
								continue;
							}
						}
						else
						{
							if (this.currentPos >= this.endPos)
							{
								break;
							}
							continue;
						}
					}
					else if (c2 <= '_')
					{
						switch (c2)
						{
						case '!':
							goto IL_032D;
						case '"':
						case '\'':
							goto IL_09E2;
						case '#':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							goto IL_0E46;
						case '$':
							goto IL_0E3B;
						case '%':
							goto IL_097E;
						case '&':
							goto IL_041C;
						case '(':
							goto IL_09AA;
						case ')':
							goto IL_09B2;
						case '*':
							goto IL_0540;
						case '+':
							goto IL_04AE;
						case ',':
							goto IL_037D;
						case '-':
							goto IL_04F7;
						case '.':
							goto IL_03C1;
						case '/':
						{
							jstoken = JSToken.Divide;
							c = this.GetChar(this.currentPos);
							bool flag = false;
							char c3 = c;
							if (c3 != '*')
							{
								if (c3 != '/')
								{
									if (c3 == '=')
									{
										this.currentPos++;
										jstoken = JSToken.DivideAssign;
									}
								}
								else
								{
									if (this.GetChar(++this.currentPos) == '@' && !this.peekModeOn)
									{
										if (!this.preProcessorOn)
										{
											if ('c' == this.GetChar(++this.currentPos) && 'c' == this.GetChar(++this.currentPos) && '_' == this.GetChar(++this.currentPos) && 'o' == this.GetChar(++this.currentPos) && 'n' == this.GetChar(++this.currentPos))
											{
												char @char = this.GetChar(this.currentPos + 1);
												if (!JSScanner.IsDigit(@char) && !JSScanner.IsAsciiLetter(@char) && !JSScanner.IsUnicodeLetter(@char))
												{
													this.SetPreProcessorOn();
													this.currentPos++;
													continue;
												}
											}
										}
										else
										{
											if (!JSScanner.IsBlankSpace(this.GetChar(++this.currentPos)))
											{
												flag = true;
												goto IL_0946;
											}
											continue;
										}
									}
									this.SkipSingleLineComment();
									if (!this.IsAuthoring)
									{
										continue;
									}
									jstoken = JSToken.Comment;
								}
							}
							else
							{
								if (this.GetChar(++this.currentPos) == '@' && !this.peekModeOn)
								{
									if (!this.preProcessorOn)
									{
										if ('c' == this.GetChar(++this.currentPos) && 'c' == this.GetChar(++this.currentPos) && '_' == this.GetChar(++this.currentPos) && 'o' == this.GetChar(++this.currentPos) && 'n' == this.GetChar(++this.currentPos))
										{
											char char2 = this.GetChar(this.currentPos + 1);
											if (!JSScanner.IsDigit(char2) && !JSScanner.IsAsciiLetter(char2) && !JSScanner.IsUnicodeLetter(char2))
											{
												this.SetPreProcessorOn();
												this.currentPos++;
												continue;
											}
										}
									}
									else
									{
										if (!JSScanner.IsBlankSpace(this.GetChar(++this.currentPos)))
										{
											flag = true;
											goto IL_0946;
										}
										continue;
									}
								}
								this.SkipMultiLineComment();
								if (!this.IsAuthoring)
								{
									continue;
								}
								if (this.currentPos > this.endPos)
								{
									jstoken = JSToken.UnterminatedComment;
									this.currentPos = this.endPos;
								}
								else
								{
									jstoken = JSToken.Comment;
								}
							}
							IL_0946:
							if (!flag)
							{
								goto IL_0EB6;
							}
							break;
						}
						case ':':
							goto IL_0395;
						case ';':
							goto IL_09DA;
						case '<':
							goto IL_02D3;
						case '=':
							goto IL_01E5;
						case '>':
							goto IL_0235;
						case '?':
							goto IL_038D;
						case '@':
							break;
						default:
							switch (c2)
							{
							case '[':
								goto IL_09CA;
							case '\\':
								goto IL_056C;
							case ']':
								goto IL_09D2;
							case '^':
								goto IL_0952;
							case '_':
								goto IL_0E3B;
							default:
								goto IL_0E46;
							}
							break;
						}
						if (this.scanForDebugger)
						{
							this.HandleError(JSError.CcInvalidInDebugger);
						}
						if (this.peekModeOn)
						{
							goto Block_79;
						}
						num2 = this.currentPos;
						this.currentToken.startPos = num2;
						this.currentToken.lineNumber = this.currentLine;
						this.currentToken.startLinePos = this.startLinePos;
						this.ScanIdentifier();
						switch (this.currentPos - num2)
						{
						case 0:
							if (this.preProcessorOn && '*' == this.GetChar(this.currentPos) && '/' == this.GetChar(++this.currentPos))
							{
								this.currentPos++;
								continue;
							}
							this.HandleError(JSError.IllegalChar);
							continue;
						case 2:
							if ('i' == this.strSourceCode[num2] && 'f' == this.strSourceCode[num2 + 1])
							{
								if (!this.preProcessorOn)
								{
									this.SetPreProcessorOn();
								}
								this.matchIf++;
								if (!this.PPTestCond())
								{
									this.PPSkipToNextCondition(true);
									continue;
								}
								continue;
							}
							break;
						case 3:
							if ('s' == this.strSourceCode[num2] && 'e' == this.strSourceCode[num2 + 1] && 't' == this.strSourceCode[num2 + 2])
							{
								if (!this.preProcessorOn)
								{
									this.SetPreProcessorOn();
								}
								this.PPScanSet();
								continue;
							}
							if ('e' == this.strSourceCode[num2] && 'n' == this.strSourceCode[num2 + 1] && 'd' == this.strSourceCode[num2 + 2])
							{
								if (0 >= this.matchIf)
								{
									this.HandleError(JSError.CcInvalidEnd);
									continue;
								}
								this.matchIf--;
								continue;
							}
							break;
						case 4:
							if ('e' == this.strSourceCode[num2] && 'l' == this.strSourceCode[num2 + 1] && 's' == this.strSourceCode[num2 + 2] && 'e' == this.strSourceCode[num2 + 3])
							{
								if (0 >= this.matchIf)
								{
									this.HandleError(JSError.CcInvalidElse);
									continue;
								}
								this.PPSkipToNextCondition(false);
								continue;
							}
							else if ('e' == this.strSourceCode[num2] && 'l' == this.strSourceCode[num2 + 1] && 'i' == this.strSourceCode[num2 + 2] && 'f' == this.strSourceCode[num2 + 3])
							{
								if (0 >= this.matchIf)
								{
									this.HandleError(JSError.CcInvalidElif);
									continue;
								}
								this.PPSkipToNextCondition(false);
								continue;
							}
							break;
						case 5:
							if ('c' == this.GetChar(num2) && 'c' == this.GetChar(num2 + 1) && '_' == this.GetChar(num2 + 2) && 'o' == this.GetChar(num2 + 3) && 'n' == this.GetChar(num2 + 4))
							{
								if (!this.preProcessorOn)
								{
									this.SetPreProcessorOn();
									continue;
								}
								continue;
							}
							break;
						}
						if (!this.preProcessorOn)
						{
							this.HandleError(JSError.CcOff);
							continue;
						}
						goto IL_0DF1;
					}
					else
					{
						switch (c2)
						{
						case '{':
							goto IL_09BA;
						case '|':
							goto IL_0465;
						case '}':
							goto IL_09C2;
						case '~':
							goto IL_0385;
						default:
							switch (c2)
							{
							case '\u2028':
								this.currentLine++;
								this.startLinePos = this.currentPos;
								continue;
							case '\u2029':
								this.currentLine++;
								this.startLinePos = this.currentPos;
								continue;
							}
							break;
						}
					}
					IL_0E46:
					if ('a' <= c && c <= 'z')
					{
						goto Block_115;
					}
					if (JSScanner.IsDigit(c))
					{
						goto Block_117;
					}
					if (('A' <= c && c <= 'Z') || JSScanner.IsUnicodeLetter(c))
					{
						goto IL_0E9B;
					}
					this.HandleError(JSError.IllegalChar);
				}
				this.currentPos--;
				jstoken = JSToken.EndOfFile;
				if (this.matchIf > 0)
				{
					this.currentToken.endLineNumber = this.currentLine;
					this.currentToken.endLinePos = this.startLinePos;
					this.currentToken.endPos = this.currentPos;
					this.HandleError(JSError.NoCcEnd);
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_01E5:
				jstoken = JSToken.Assign;
				if ('=' != this.GetChar(this.currentPos))
				{
					goto IL_0EB6;
				}
				this.currentPos++;
				jstoken = JSToken.Equal;
				if ('=' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.StrictEqual;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_0235:
				jstoken = JSToken.GreaterThan;
				if ('>' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.RightShift;
					if ('>' == this.GetChar(this.currentPos))
					{
						this.currentPos++;
						jstoken = JSToken.UnsignedRightShift;
					}
				}
				if ('=' != this.GetChar(this.currentPos))
				{
					goto IL_0EB6;
				}
				this.currentPos++;
				JSToken jstoken2 = jstoken;
				if (jstoken2 == JSToken.GreaterThan)
				{
					jstoken = JSToken.GreaterThanEqual;
					goto IL_0EB6;
				}
				switch (jstoken2)
				{
				case JSToken.RightShift:
					jstoken = JSToken.RightShiftAssign;
					goto IL_0EB6;
				case JSToken.UnsignedRightShift:
					jstoken = JSToken.UnsignedRightShiftAssign;
					goto IL_0EB6;
				default:
					goto IL_0EB6;
				}
				IL_02D3:
				jstoken = JSToken.LessThan;
				if ('<' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.LeftShift;
				}
				if ('=' != this.GetChar(this.currentPos))
				{
					goto IL_0EB6;
				}
				this.currentPos++;
				if (jstoken == JSToken.LessThan)
				{
					jstoken = JSToken.LessThanEqual;
					goto IL_0EB6;
				}
				jstoken = JSToken.LeftShiftAssign;
				goto IL_0EB6;
				IL_032D:
				jstoken = JSToken.FirstOp;
				if ('=' != this.GetChar(this.currentPos))
				{
					goto IL_0EB6;
				}
				this.currentPos++;
				jstoken = JSToken.NotEqual;
				if ('=' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.StrictNotEqual;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_037D:
				jstoken = JSToken.Comma;
				goto IL_0EB6;
				IL_0385:
				jstoken = JSToken.BitwiseNot;
				goto IL_0EB6;
				IL_038D:
				jstoken = JSToken.ConditionalIf;
				goto IL_0EB6;
				IL_0395:
				jstoken = JSToken.Colon;
				if (':' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.DoubleColon;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_03C1:
				jstoken = JSToken.AccessField;
				c = this.GetChar(this.currentPos);
				if (JSScanner.IsDigit(c))
				{
					jstoken = this.ScanNumber('.');
					goto IL_0EB6;
				}
				if ('.' != c)
				{
					goto IL_0EB6;
				}
				c = this.GetChar(this.currentPos + 1);
				if ('.' == c)
				{
					this.currentPos += 2;
					jstoken = JSToken.ParamArray;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_041C:
				jstoken = JSToken.BitwiseAnd;
				c = this.GetChar(this.currentPos);
				if ('&' == c)
				{
					this.currentPos++;
					jstoken = JSToken.LogicalAnd;
					goto IL_0EB6;
				}
				if ('=' == c)
				{
					this.currentPos++;
					jstoken = JSToken.BitwiseAndAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_0465:
				jstoken = JSToken.BitwiseOr;
				c = this.GetChar(this.currentPos);
				if ('|' == c)
				{
					this.currentPos++;
					jstoken = JSToken.LogicalOr;
					goto IL_0EB6;
				}
				if ('=' == c)
				{
					this.currentPos++;
					jstoken = JSToken.BitwiseOrAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_04AE:
				jstoken = JSToken.FirstBinaryOp;
				c = this.GetChar(this.currentPos);
				if ('+' == c)
				{
					this.currentPos++;
					jstoken = JSToken.Increment;
					goto IL_0EB6;
				}
				if ('=' == c)
				{
					this.currentPos++;
					jstoken = JSToken.PlusAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_04F7:
				jstoken = JSToken.Minus;
				c = this.GetChar(this.currentPos);
				if ('-' == c)
				{
					this.currentPos++;
					jstoken = JSToken.Decrement;
					goto IL_0EB6;
				}
				if ('=' == c)
				{
					this.currentPos++;
					jstoken = JSToken.MinusAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_0540:
				jstoken = JSToken.Multiply;
				if ('=' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.MultiplyAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_056C:
				this.currentPos--;
				if (this.IsIdentifierStartChar(ref c))
				{
					this.currentPos++;
					this.ScanIdentifier();
					jstoken = JSToken.Identifier;
					goto IL_0EB6;
				}
				this.currentPos++;
				c = this.GetChar(this.currentPos);
				if ('a' <= c && c <= 'z')
				{
					JSKeyword jskeyword = this.keywords[(int)(c - 'a')];
					if (jskeyword != null)
					{
						this.currentToken.startPos++;
						jstoken = this.ScanKeyword(jskeyword);
						if (jstoken != JSToken.Identifier)
						{
							jstoken = JSToken.Identifier;
							goto IL_0EB6;
						}
						this.currentToken.startPos--;
					}
				}
				this.currentPos = this.currentToken.startPos + 1;
				this.HandleError(JSError.IllegalChar);
				goto IL_0EB6;
				IL_0952:
				jstoken = JSToken.BitwiseXor;
				if ('=' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.BitwiseXorAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_097E:
				jstoken = JSToken.Modulo;
				if ('=' == this.GetChar(this.currentPos))
				{
					this.currentPos++;
					jstoken = JSToken.ModuloAssign;
					goto IL_0EB6;
				}
				goto IL_0EB6;
				IL_09AA:
				jstoken = JSToken.LeftParen;
				goto IL_0EB6;
				IL_09B2:
				jstoken = JSToken.RightParen;
				goto IL_0EB6;
				IL_09BA:
				jstoken = JSToken.LeftCurly;
				goto IL_0EB6;
				IL_09C2:
				jstoken = JSToken.RightCurly;
				goto IL_0EB6;
				IL_09CA:
				jstoken = JSToken.LeftBracket;
				goto IL_0EB6;
				IL_09D2:
				jstoken = JSToken.RightBracket;
				goto IL_0EB6;
				IL_09DA:
				jstoken = JSToken.Semicolon;
				goto IL_0EB6;
				IL_09E2:
				jstoken = JSToken.StringLiteral;
				this.ScanString(c);
				goto IL_0EB6;
				Block_79:
				this.currentToken.token = JSToken.PreProcessDirective;
				goto IL_0EB6;
				IL_0DF1:
				object obj = this.ppTable[this.strSourceCode.Substring(num2, this.currentPos - num2)];
				if (obj == null)
				{
					this.preProcessorValue = double.NaN;
				}
				else
				{
					this.preProcessorValue = obj;
				}
				jstoken = JSToken.PreProcessorConstant;
				goto IL_0EB6;
				IL_0E3B:
				this.ScanIdentifier();
				jstoken = JSToken.Identifier;
				goto IL_0EB6;
				Block_115:
				JSKeyword jskeyword2 = this.keywords[(int)(c - 'a')];
				if (jskeyword2 != null)
				{
					jstoken = this.ScanKeyword(jskeyword2);
					goto IL_0EB6;
				}
				jstoken = JSToken.Identifier;
				this.ScanIdentifier();
				goto IL_0EB6;
				Block_117:
				jstoken = this.ScanNumber(c);
				goto IL_0EB6;
				IL_0E9B:
				jstoken = JSToken.Identifier;
				this.ScanIdentifier();
				IL_0EB6:
				this.currentToken.endLineNumber = this.currentLine;
				this.currentToken.endLinePos = this.startLinePos;
				this.currentToken.endPos = this.currentPos;
				this.gotEndOfLine = this.currentLine > num || jstoken == JSToken.EndOfFile;
				if (this.gotEndOfLine && jstoken == JSToken.StringLiteral && this.currentToken.lineNumber == num)
				{
					this.gotEndOfLine = false;
				}
			}
			catch (IndexOutOfRangeException)
			{
				jstoken = JSToken.None;
				this.currentToken.endPos = this.currentPos;
				this.currentToken.endLineNumber = this.currentLine;
				this.currentToken.endLinePos = this.startLinePos;
				throw new ScannerException(JSError.ErrEOF);
			}
			this.currentToken.token = jstoken;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000430AC File Offset: 0x000420AC
		private char GetChar(int index)
		{
			if (index < this.endPos)
			{
				return this.strSourceCode[index];
			}
			return '\0';
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000430C5 File Offset: 0x000420C5
		public int GetCurrentPosition(bool absolute)
		{
			return this.currentPos;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000430CD File Offset: 0x000420CD
		public int GetCurrentLine()
		{
			return this.currentLine;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000430D5 File Offset: 0x000420D5
		public int GetStartLinePosition()
		{
			return this.startLinePos;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000430DD File Offset: 0x000420DD
		public string GetStringLiteral()
		{
			return this.escapedString;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000430E5 File Offset: 0x000420E5
		public string GetSourceCode()
		{
			return this.strSourceCode;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000430ED File Offset: 0x000420ED
		public bool GotEndOfLine()
		{
			return this.gotEndOfLine;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x000430F8 File Offset: 0x000420F8
		internal string GetIdentifier()
		{
			string text;
			if (this.identifier.Length > 0)
			{
				text = this.identifier.ToString();
				this.identifier.Length = 0;
			}
			else
			{
				text = this.currentToken.GetCode();
			}
			if (text.Length > 500)
			{
				text = text.Substring(0, 500) + text.GetHashCode().ToString(CultureInfo.InvariantCulture);
			}
			return text;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00043170 File Offset: 0x00042170
		private void ScanIdentifier()
		{
			for (;;)
			{
				char @char = this.GetChar(this.currentPos);
				if (!this.IsIdentifierPartChar(@char))
				{
					break;
				}
				this.currentPos++;
			}
			if (this.idLastPosOnBuilder > 0)
			{
				this.identifier.Append(this.strSourceCode.Substring(this.idLastPosOnBuilder, this.currentPos - this.idLastPosOnBuilder));
				this.idLastPosOnBuilder = 0;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000431E0 File Offset: 0x000421E0
		private JSToken ScanKeyword(JSKeyword keyword)
		{
			char @char;
			for (;;)
			{
				@char = this.GetChar(this.currentPos);
				if ('a' > @char || @char > 'z')
				{
					break;
				}
				this.currentPos++;
			}
			if (this.IsIdentifierPartChar(@char))
			{
				this.ScanIdentifier();
				return JSToken.Identifier;
			}
			return keyword.GetKeyword(this.currentToken, this.currentPos - this.currentToken.startPos);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00043244 File Offset: 0x00042244
		private JSToken ScanNumber(char leadChar)
		{
			bool flag = '.' == leadChar;
			JSToken jstoken = (flag ? JSToken.NumericLiteral : JSToken.IntegerLiteral);
			bool flag2 = false;
			char c;
			if ('0' == leadChar)
			{
				c = this.GetChar(this.currentPos);
				if ('x' == c || 'X' == c)
				{
					if (!JSScanner.IsHexDigit(this.GetChar(this.currentPos + 1)))
					{
						this.HandleError(JSError.BadHexDigit);
					}
					while (JSScanner.IsHexDigit(this.GetChar(++this.currentPos)))
					{
					}
					return jstoken;
				}
			}
			for (;;)
			{
				c = this.GetChar(this.currentPos);
				if (!JSScanner.IsDigit(c))
				{
					if ('.' == c)
					{
						if (flag)
						{
							break;
						}
						flag = true;
						jstoken = JSToken.NumericLiteral;
					}
					else if ('e' == c || 'E' == c)
					{
						if (flag2)
						{
							break;
						}
						flag2 = true;
						jstoken = JSToken.NumericLiteral;
					}
					else
					{
						if ('+' != c && '-' != c)
						{
							break;
						}
						char @char = this.GetChar(this.currentPos - 1);
						if ('e' != @char && 'E' != @char)
						{
							break;
						}
					}
				}
				this.currentPos++;
			}
			c = this.GetChar(this.currentPos - 1);
			if ('+' == c || '-' == c)
			{
				this.currentPos--;
				c = this.GetChar(this.currentPos - 1);
			}
			if ('e' == c || 'E' == c)
			{
				this.currentPos--;
			}
			return jstoken;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00043380 File Offset: 0x00042380
		internal string ScanRegExp()
		{
			int num = this.currentPos;
			bool flag = false;
			char @char;
			while (!this.IsEndLineOrEOF(@char = this.GetChar(this.currentPos++), 0))
			{
				if (flag)
				{
					flag = false;
				}
				else if (@char == '/')
				{
					if (num == this.currentPos)
					{
						return null;
					}
					this.currentToken.endPos = this.currentPos;
					this.currentToken.endLinePos = this.startLinePos;
					this.currentToken.endLineNumber = this.currentLine;
					return this.strSourceCode.Substring(this.currentToken.startPos + 1, this.currentToken.endPos - this.currentToken.startPos - 2);
				}
				else if (@char == '\\')
				{
					flag = true;
				}
			}
			this.currentPos = num;
			return null;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0004344C File Offset: 0x0004244C
		internal string ScanRegExpFlags()
		{
			int num = this.currentPos;
			while (JSScanner.IsAsciiLetter(this.GetChar(this.currentPos)))
			{
				this.currentPos++;
			}
			if (num != this.currentPos)
			{
				this.currentToken.endPos = this.currentPos;
				this.currentToken.endLineNumber = this.currentLine;
				this.currentToken.endLinePos = this.startLinePos;
				return this.strSourceCode.Substring(num, this.currentToken.endPos - num);
			}
			return null;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000434DC File Offset: 0x000424DC
		private void ScanString(char cStringTerminator)
		{
			int num = this.currentPos;
			this.escapedString = null;
			StringBuilder stringBuilder = null;
			for (;;)
			{
				char c = this.GetChar(this.currentPos++);
				if (c == '\\')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(128);
					}
					if (this.currentPos - num - 1 > 0)
					{
						stringBuilder.Append(this.strSourceCode, num, this.currentPos - num - 1);
					}
					bool flag = false;
					int num2 = 0;
					c = this.GetChar(this.currentPos++);
					char c2 = c;
					if (c2 <= '7')
					{
						if (c2 <= '\r')
						{
							if (c2 == '\n')
							{
								goto IL_01E3;
							}
							if (c2 != '\r')
							{
								goto IL_0672;
							}
							if ('\n' == this.GetChar(this.currentPos))
							{
								this.currentPos++;
								goto IL_01E3;
							}
							goto IL_01E3;
						}
						else if (c2 != '"')
						{
							switch (c2)
							{
							case '\'':
								stringBuilder.Append('\'');
								c = '\0';
								goto IL_067A;
							case '(':
							case ')':
							case '*':
							case '+':
							case ',':
							case '-':
							case '.':
							case '/':
								goto IL_0672;
							case '0':
							case '1':
							case '2':
							case '3':
								flag = true;
								num2 = (int)((int)(c - '0') << 6);
								break;
							case '4':
							case '5':
							case '6':
							case '7':
								break;
							default:
								goto IL_0672;
							}
							if (!flag)
							{
								num2 = (int)((int)(c - '0') << 3);
							}
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\a')
							{
								if (flag)
								{
									num2 |= (int)((int)(c - '0') << 3);
									c = this.GetChar(this.currentPos++);
									if (c - '0' <= '\a')
									{
										num2 |= (int)(c - '0');
										stringBuilder.Append((char)num2);
									}
									else
									{
										stringBuilder.Append((char)(num2 >> 3));
										if (c != cStringTerminator)
										{
											this.currentPos--;
										}
									}
								}
								else
								{
									num2 |= (int)(c - '0');
									stringBuilder.Append((char)num2);
								}
							}
							else
							{
								if (flag)
								{
									stringBuilder.Append((char)(num2 >> 6));
								}
								else
								{
									stringBuilder.Append((char)(num2 >> 3));
								}
								if (c != cStringTerminator)
								{
									this.currentPos--;
								}
							}
						}
						else
						{
							stringBuilder.Append('"');
							c = '\0';
						}
					}
					else if (c2 <= 'b')
					{
						if (c2 != '\\')
						{
							if (c2 != 'b')
							{
								goto IL_0672;
							}
							stringBuilder.Append('\b');
						}
						else
						{
							stringBuilder.Append('\\');
						}
					}
					else if (c2 != 'f')
					{
						switch (c2)
						{
						case 'n':
							stringBuilder.Append('\n');
							break;
						case 'o':
						case 'p':
						case 'q':
						case 's':
						case 'w':
							goto IL_0672;
						case 'r':
							stringBuilder.Append('\r');
							break;
						case 't':
							stringBuilder.Append('\t');
							break;
						case 'u':
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 = (int)((int)(c - '0') << 12);
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 = (int)((int)(c + '\n' - 'A') << 12);
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 = (int)((int)(c + '\n' - 'a') << 12);
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 |= (int)((int)(c - '0') << 8);
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 |= (int)((int)(c + '\n' - 'A') << 8);
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 |= (int)((int)(c + '\n' - 'a') << 8);
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 |= (int)((int)(c - '0') << 4);
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 |= (int)((int)(c + '\n' - 'A') << 4);
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 |= (int)((int)(c + '\n' - 'a') << 4);
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 |= (int)(c - '0');
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 |= (int)(c + '\n' - 'A');
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 |= (int)(c + '\n' - 'a');
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							stringBuilder.Append((char)num2);
							break;
						case 'v':
							stringBuilder.Append('\v');
							break;
						case 'x':
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 = (int)((int)(c - '0') << 4);
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 = (int)((int)(c + '\n' - 'A') << 4);
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 = (int)((int)(c + '\n' - 'a') << 4);
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							c = this.GetChar(this.currentPos++);
							if (c - '0' <= '\t')
							{
								num2 |= (int)(c - '0');
							}
							else if (c - 'A' <= '\u0005')
							{
								num2 |= (int)(c + '\n' - 'A');
							}
							else if (c - 'a' <= '\u0005')
							{
								num2 |= (int)(c + '\n' - 'a');
							}
							else
							{
								this.HandleError(JSError.BadHexDigit);
								if (c != cStringTerminator)
								{
									this.currentPos--;
									break;
								}
								break;
							}
							stringBuilder.Append((char)num2);
							break;
						default:
							switch (c2)
							{
							case '\u2028':
							case '\u2029':
								goto IL_01E3;
							default:
								goto IL_0672;
							}
							break;
						}
					}
					else
					{
						stringBuilder.Append('\f');
					}
					IL_067A:
					num = this.currentPos;
					goto IL_0681;
					IL_01E3:
					this.currentLine++;
					this.startLinePos = this.currentPos;
					goto IL_067A;
					IL_0672:
					stringBuilder.Append(c);
					goto IL_067A;
				}
				if (this.IsLineTerminator(c, 0))
				{
					break;
				}
				if (c == '\0')
				{
					goto Block_3;
				}
				IL_0681:
				if (c == cStringTerminator)
				{
					goto IL_0688;
				}
			}
			this.HandleError(JSError.UnterminatedString);
			this.currentPos--;
			goto IL_0688;
			Block_3:
			this.currentPos--;
			this.HandleError(JSError.UnterminatedString);
			IL_0688:
			if (stringBuilder != null)
			{
				if (this.currentPos - num - 1 > 0)
				{
					stringBuilder.Append(this.strSourceCode, num, this.currentPos - num - 1);
				}
				this.escapedString = stringBuilder.ToString();
				return;
			}
			if (this.currentPos <= this.currentToken.startPos + 2)
			{
				this.escapedString = "";
				return;
			}
			this.escapedString = this.currentToken.source_string.Substring(this.currentToken.startPos + 1, this.currentPos - this.currentToken.startPos - 2);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00043C00 File Offset: 0x00042C00
		private void SkipSingleLineComment()
		{
			while (!this.IsEndLineOrEOF(this.GetChar(this.currentPos++), 0))
			{
			}
			if (this.IsAuthoring)
			{
				this.currentToken.endPos = this.currentPos;
				this.currentToken.endLineNumber = this.currentLine;
				this.currentToken.endLinePos = this.startLinePos;
				this.gotEndOfLine = true;
			}
			this.currentLine++;
			this.startLinePos = this.currentPos;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00043C8C File Offset: 0x00042C8C
		public int SkipMultiLineComment()
		{
			for (;;)
			{
				char c = this.GetChar(this.currentPos);
				while ('*' == c)
				{
					c = this.GetChar(++this.currentPos);
					if ('/' == c)
					{
						goto Block_0;
					}
					if (c == '\0')
					{
						break;
					}
					if (this.IsLineTerminator(c, 1))
					{
						c = this.GetChar(++this.currentPos);
						this.currentLine++;
						this.startLinePos = this.currentPos + 1;
					}
				}
				if (c == '\0' && this.currentPos >= this.endPos)
				{
					goto IL_00D1;
				}
				if (this.IsLineTerminator(c, 1))
				{
					this.currentLine++;
					this.startLinePos = this.currentPos + 1;
				}
				this.currentPos++;
			}
			Block_0:
			this.currentPos++;
			return this.currentPos;
			IL_00D1:
			if (!this.IsAuthoring)
			{
				this.currentToken.endPos = --this.currentPos;
				this.currentToken.endLinePos = this.startLinePos;
				this.currentToken.endLineNumber = this.currentLine;
				throw new ScannerException(JSError.NoCommentEnd);
			}
			return this.currentPos;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00043DC4 File Offset: 0x00042DC4
		private void SkipBlanks()
		{
			char c = this.GetChar(this.currentPos);
			while (JSScanner.IsBlankSpace(c))
			{
				c = this.GetChar(++this.currentPos);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00043E00 File Offset: 0x00042E00
		private static bool IsBlankSpace(char c)
		{
			switch (c)
			{
			case '\t':
			case '\v':
			case '\f':
				break;
			case '\n':
				goto IL_002A;
			default:
				if (c != ' ' && c != '\u00a0')
				{
					goto IL_002A;
				}
				break;
			}
			return true;
			IL_002A:
			return c >= '\u0080' && char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00043E4C File Offset: 0x00042E4C
		private bool IsLineTerminator(char c, int increment)
		{
			if (c == '\n')
			{
				return true;
			}
			if (c == '\r')
			{
				if ('\n' == this.GetChar(this.currentPos + increment))
				{
					this.currentPos++;
				}
				return true;
			}
			switch (c)
			{
			case '\u2028':
				return true;
			case '\u2029':
				return true;
			default:
				return false;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00043EA4 File Offset: 0x00042EA4
		private bool IsEndLineOrEOF(char c, int increment)
		{
			return this.IsLineTerminator(c, increment) || (c == '\0' && this.currentPos >= this.endPos);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00043EC8 File Offset: 0x00042EC8
		private int GetHexValue(char hex)
		{
			int num;
			if ('0' <= hex && hex <= '9')
			{
				num = (int)(hex - '0');
			}
			else if ('a' <= hex && hex <= 'f')
			{
				num = (int)(hex - 'a' + '\n');
			}
			else
			{
				num = (int)(hex - 'A' + '\n');
			}
			return num;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00043F04 File Offset: 0x00042F04
		internal bool IsIdentifierPartChar(char c)
		{
			if (this.IsIdentifierStartChar(ref c))
			{
				return true;
			}
			if ('0' <= c && c <= '9')
			{
				return true;
			}
			if (c < '\u0080')
			{
				return false;
			}
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			UnicodeCategory unicodeCategory2 = unicodeCategory;
			switch (unicodeCategory2)
			{
			case UnicodeCategory.NonSpacingMark:
			case UnicodeCategory.SpacingCombiningMark:
			case UnicodeCategory.DecimalDigitNumber:
				break;
			case UnicodeCategory.EnclosingMark:
				return false;
			default:
				if (unicodeCategory2 != UnicodeCategory.ConnectorPunctuation)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00043F5C File Offset: 0x00042F5C
		internal bool IsIdentifierStartChar(ref char c)
		{
			bool flag = false;
			if ('\\' == c && 'u' == this.GetChar(this.currentPos + 1))
			{
				char @char = this.GetChar(this.currentPos + 2);
				if (JSScanner.IsHexDigit(@char))
				{
					char char2 = this.GetChar(this.currentPos + 3);
					if (JSScanner.IsHexDigit(char2))
					{
						char char3 = this.GetChar(this.currentPos + 4);
						if (JSScanner.IsHexDigit(char3))
						{
							char char4 = this.GetChar(this.currentPos + 5);
							if (JSScanner.IsHexDigit(char4))
							{
								flag = true;
								c = (char)((this.GetHexValue(@char) << 12) | (this.GetHexValue(char2) << 8) | (this.GetHexValue(char3) << 4) | this.GetHexValue(char4));
							}
						}
					}
				}
			}
			if (('a' > c || c > 'z') && ('A' > c || c > 'Z') && '_' != c && '$' != c)
			{
				if (c < '\u0080')
				{
					return false;
				}
				switch (char.GetUnicodeCategory(c))
				{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.ModifierLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.LetterNumber:
					break;
				default:
					return false;
				}
			}
			if (flag)
			{
				int num = ((this.idLastPosOnBuilder > 0) ? this.idLastPosOnBuilder : this.currentToken.startPos);
				if (this.currentPos - num > 0)
				{
					this.identifier.Append(this.strSourceCode.Substring(num, this.currentPos - num));
				}
				this.identifier.Append(c);
				this.currentPos += 5;
				this.idLastPosOnBuilder = this.currentPos + 1;
			}
			return true;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000440FA File Offset: 0x000430FA
		internal static bool IsDigit(char c)
		{
			return '0' <= c && c <= '9';
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0004410B File Offset: 0x0004310B
		internal static bool IsHexDigit(char c)
		{
			return JSScanner.IsDigit(c) || ('A' <= c && c <= 'F') || ('a' <= c && c <= 'f');
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00044130 File Offset: 0x00043130
		internal static bool IsAsciiLetter(char c)
		{
			return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0004414D File Offset: 0x0004314D
		internal static bool IsUnicodeLetter(char c)
		{
			return c >= '\u0080' && char.IsLetter(c);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00044160 File Offset: 0x00043160
		private void SetPreProcessorOn()
		{
			this.preProcessorOn = true;
			this.ppTable = new SimpleHashtable(16U);
			this.ppTable["_debug"] = this.globals.engine.GenerateDebugInfo;
			this.ppTable["_fast"] = ((IActivationObject)this.globals.ScopeStack.Peek()).GetGlobalScope().fast;
			this.ppTable["_jscript"] = true;
			this.ppTable["_jscript_build"] = GlobalObject.ScriptEngineBuildVersion();
			this.ppTable["_jscript_version"] = Convert.ToNumber(GlobalObject.ScriptEngineMajorVersion() + "." + GlobalObject.ScriptEngineMinorVersion());
			this.ppTable["_microsoft"] = true;
			if (this.globals.engine.PEMachineArchitecture == ImageFileMachine.I386 && this.globals.engine.PEKindFlags == PortableExecutableKinds.ILOnly)
			{
				this.ppTable["_win32"] = Environment.OSVersion.Platform.ToString().StartsWith("Win32", StringComparison.Ordinal);
				this.ppTable["_x86"] = true;
			}
			Hashtable hashtable = (Hashtable)this.globals.engine.GetOption("defines");
			if (hashtable != null)
			{
				foreach (object obj in hashtable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					this.ppTable[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00044344 File Offset: 0x00043344
		private bool PPTestCond()
		{
			this.SkipBlanks();
			if ('(' != this.GetChar(this.currentPos))
			{
				this.currentToken.startPos = this.currentPos - 1;
				this.currentToken.lineNumber = this.currentLine;
				this.currentToken.startLinePos = this.startLinePos;
				this.HandleError(JSError.NoLeftParen);
			}
			else
			{
				this.currentPos++;
			}
			object obj = this.PPScanExpr();
			if (')' != this.GetChar(this.currentPos++))
			{
				this.currentToken.startPos = this.currentPos - 1;
				this.currentToken.lineNumber = this.currentLine;
				this.currentToken.startLinePos = this.startLinePos;
				this.HandleError(JSError.NoRightParen);
				this.currentPos--;
			}
			return Convert.ToBoolean(obj);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00044430 File Offset: 0x00043430
		private void PPSkipToNextCondition(bool checkCondition)
		{
			int num = 0;
			for (;;)
			{
				char @char = this.GetChar(this.currentPos++);
				char c = @char;
				if (c <= '\n')
				{
					if (c != '\0')
					{
						if (c == '\n')
						{
							this.currentLine++;
							this.startLinePos = this.currentPos;
						}
					}
					else if (this.currentPos >= this.endPos)
					{
						break;
					}
				}
				else if (c != '\r')
				{
					if (c != '@')
					{
						switch (c)
						{
						case '\u2028':
							this.currentLine++;
							this.startLinePos = this.currentPos;
							break;
						case '\u2029':
							this.currentLine++;
							this.startLinePos = this.currentPos;
							break;
						}
					}
					else
					{
						this.currentToken.startPos = this.currentPos;
						this.currentToken.lineNumber = this.currentLine;
						this.currentToken.startLinePos = this.startLinePos;
						this.ScanIdentifier();
						switch (this.currentPos - this.currentToken.startPos)
						{
						case 2:
							if ('i' == this.strSourceCode[this.currentToken.startPos] && 'f' == this.strSourceCode[this.currentToken.startPos + 1])
							{
								num++;
							}
							break;
						case 3:
							if ('e' == this.strSourceCode[this.currentToken.startPos] && 'n' == this.strSourceCode[this.currentToken.startPos + 1] && 'd' == this.strSourceCode[this.currentToken.startPos + 2])
							{
								if (num == 0)
								{
									goto Block_15;
								}
								num--;
							}
							break;
						case 4:
							if ('e' == this.strSourceCode[this.currentToken.startPos] && 'l' == this.strSourceCode[this.currentToken.startPos + 1] && 's' == this.strSourceCode[this.currentToken.startPos + 2] && 'e' == this.strSourceCode[this.currentToken.startPos + 3])
							{
								if (num == 0 && checkCondition)
								{
									return;
								}
							}
							else if ('e' == this.strSourceCode[this.currentToken.startPos] && 'l' == this.strSourceCode[this.currentToken.startPos + 1] && 'i' == this.strSourceCode[this.currentToken.startPos + 2] && 'f' == this.strSourceCode[this.currentToken.startPos + 3] && num == 0 && checkCondition && this.PPTestCond())
							{
								return;
							}
							break;
						}
					}
				}
				else
				{
					if (this.GetChar(this.currentPos) == '\n')
					{
						this.currentPos++;
					}
					this.currentLine++;
					this.startLinePos = this.currentPos;
				}
			}
			this.currentPos--;
			this.currentToken.endPos = this.currentPos;
			this.currentToken.endLineNumber = this.currentLine;
			this.currentToken.endLinePos = this.startLinePos;
			this.HandleError(JSError.NoCcEnd);
			throw new ScannerException(JSError.ErrEOF);
			Block_15:
			this.matchIf--;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x000447C0 File Offset: 0x000437C0
		private void PPScanSet()
		{
			this.SkipBlanks();
			if ('@' != this.GetChar(this.currentPos++))
			{
				this.HandleError(JSError.NoAt);
				this.currentPos--;
			}
			int num = this.currentPos;
			this.ScanIdentifier();
			int num2 = this.currentPos - num;
			string text;
			if (num2 == 0)
			{
				this.currentToken.startPos = this.currentPos - 1;
				this.currentToken.lineNumber = this.currentLine;
				this.currentToken.startLinePos = this.startLinePos;
				this.HandleError(JSError.NoIdentifier);
				text = "#_Missing CC Identifier_#";
			}
			else
			{
				text = this.strSourceCode.Substring(num, num2);
			}
			this.SkipBlanks();
			char @char = this.GetChar(this.currentPos++);
			if ('(' != @char)
			{
				if ('=' != @char)
				{
					this.currentToken.startPos = this.currentPos - 1;
					this.currentToken.lineNumber = this.currentLine;
					this.currentToken.startLinePos = this.startLinePos;
					this.HandleError(JSError.NoEqual);
					this.currentPos--;
				}
				object obj = this.PPScanConstant();
				this.ppTable[text] = obj;
				return;
			}
			if (text.Equals("position"))
			{
				this.PPRemapPositionInfo();
				return;
			}
			if (text.Equals("option"))
			{
				this.PPLanguageOption();
				return;
			}
			if (text.Equals("debug"))
			{
				this.PPDebugDirective();
				return;
			}
			this.currentToken.startPos = this.currentPos - 1;
			this.currentToken.lineNumber = this.currentLine;
			this.currentToken.startLinePos = this.startLinePos;
			this.HandleError(JSError.NoEqual);
			this.currentPos--;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00044998 File Offset: 0x00043998
		private object PPScanExpr()
		{
			OpListItem opListItem = new OpListItem(JSToken.None, OpPrec.precNone, null);
			ConstantListItem constantListItem = new ConstantListItem(this.PPScanConstant(), null);
			for (;;)
			{
				this.GetNextToken();
				if (!JSScanner.IsPPOperator(this.currentToken.token))
				{
					break;
				}
				OpPrec ppoperatorPrecedence = JSScanner.GetPPOperatorPrecedence(this.currentToken.token);
				while (ppoperatorPrecedence < opListItem._prec)
				{
					object obj = this.PPGetValue(opListItem._operator, constantListItem.prev.term, constantListItem.term);
					opListItem = opListItem._prev;
					constantListItem = constantListItem.prev.prev;
					constantListItem = new ConstantListItem(obj, constantListItem);
				}
				opListItem = new OpListItem(this.currentToken.token, ppoperatorPrecedence, opListItem);
				constantListItem = new ConstantListItem(this.PPScanConstant(), constantListItem);
			}
			while (opListItem._operator != JSToken.None)
			{
				object obj = this.PPGetValue(opListItem._operator, constantListItem.prev.term, constantListItem.term);
				opListItem = opListItem._prev;
				constantListItem = constantListItem.prev.prev;
				constantListItem = new ConstantListItem(obj, constantListItem);
			}
			this.currentPos = this.currentToken.startPos;
			return constantListItem.term;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00044AAC File Offset: 0x00043AAC
		private void PPRemapPositionInfo()
		{
			this.GetNextToken();
			string text = null;
			int num = 0;
			int num2 = -1;
			bool flag = false;
			while (JSToken.RightParen != this.currentToken.token)
			{
				if (JSToken.Identifier == this.currentToken.token)
				{
					if (this.currentToken.Equals("file"))
					{
						if (this.currentDocument != null)
						{
							this.HandleError(JSError.CannotNestPositionDirective);
							goto IL_0355;
						}
						if (text != null)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.Assign != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.StringLiteral != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						text = this.GetStringLiteral();
						if (text == this.currentToken.document.documentName)
						{
							text = null;
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
					}
					else if (this.currentToken.Equals("line"))
					{
						if (this.currentDocument != null)
						{
							this.HandleError(JSError.CannotNestPositionDirective);
							goto IL_0355;
						}
						if (num != 0)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.Assign != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.IntegerLiteral != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						string code = this.currentToken.GetCode();
						double num3 = Convert.ToNumber(code, true, true, Missing.Value);
						if ((double)((int)num3) != num3 || num3 <= 0.0)
						{
							num = 1;
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						num = (int)num3;
					}
					else if (this.currentToken.Equals("column"))
					{
						if (this.currentDocument != null)
						{
							this.HandleError(JSError.CannotNestPositionDirective);
							goto IL_0355;
						}
						if (num2 != -1)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.Assign != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.IntegerLiteral != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						string code2 = this.currentToken.GetCode();
						double num4 = Convert.ToNumber(code2, true, true, Missing.Value);
						if ((double)((int)num4) != num4 || num4 < 0.0)
						{
							num2 = 0;
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						num2 = (int)num4;
					}
					else
					{
						if (!this.currentToken.Equals("end"))
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						if (this.currentDocument == null)
						{
							this.HandleError(JSError.WrongDirective);
							goto IL_0355;
						}
						this.GetNextToken();
						if (JSToken.RightParen != this.currentToken.token)
						{
							this.HandleError(JSError.InvalidPositionDirective);
							goto IL_0355;
						}
						this.currentToken.document = this.currentDocument;
						this.currentDocument = null;
						flag = true;
						break;
					}
					this.GetNextToken();
					if (JSToken.RightParen == this.currentToken.token)
					{
						break;
					}
					if (JSToken.Semicolon == this.currentToken.token)
					{
						this.GetNextToken();
						continue;
					}
					continue;
				}
				else
				{
					this.HandleError(JSError.InvalidPositionDirective);
				}
				IL_0355:
				while (JSToken.RightParen != this.currentToken.token)
				{
					if (this.currentToken.token == JSToken.EndOfFile)
					{
						break;
					}
					this.GetNextToken();
				}
				break;
			}
			this.SkipBlanks();
			if (';' == this.GetChar(this.currentPos))
			{
				this.currentPos++;
				this.SkipBlanks();
			}
			if (this.currentPos < this.endPos && !this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
			{
				this.HandleError(JSError.MustBeEOL);
				while (this.currentPos < this.endPos && !this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
				{
				}
			}
			this.currentLine++;
			this.startLinePos = this.currentPos;
			if (!flag)
			{
				if (text == null && num == 0 && num2 == -1)
				{
					this.HandleError(JSError.InvalidPositionDirective);
					return;
				}
				if (text == null)
				{
					text = this.currentToken.document.documentName;
				}
				if (num == 0)
				{
					num = 1;
				}
				if (num2 == -1)
				{
					num2 = 0;
				}
				this.currentDocument = this.currentToken.document;
				this.currentToken.document = new DocumentContext(text, num, num2, this.currentLine, this.currentDocument.sourceItem);
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00044F5C File Offset: 0x00043F5C
		private void PPDebugDirective()
		{
			this.GetNextToken();
			if (JSToken.Identifier == this.currentToken.token)
			{
				bool flag;
				if (this.currentToken.Equals("off"))
				{
					flag = false;
				}
				else
				{
					if (!this.currentToken.Equals("on"))
					{
						this.HandleError(JSError.InvalidDebugDirective);
						goto IL_00C3;
					}
					flag = true;
				}
				this.GetNextToken();
				if (JSToken.RightParen != this.currentToken.token)
				{
					this.HandleError(JSError.InvalidDebugDirective);
				}
				else
				{
					this.currentToken.document.debugOn = flag && this.globals.engine.GenerateDebugInfo;
					this.ppTable["_debug"] = flag;
				}
			}
			else
			{
				this.HandleError(JSError.InvalidDebugDirective);
			}
			IL_00C3:
			while (JSToken.RightParen != this.currentToken.token)
			{
				this.GetNextToken();
			}
			this.SkipBlanks();
			if (';' == this.GetChar(this.currentPos))
			{
				this.currentPos++;
				this.SkipBlanks();
			}
			if (!this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
			{
				this.HandleError(JSError.MustBeEOL);
				while (!this.IsLineTerminator(this.GetChar(this.currentPos++), 0))
				{
				}
			}
			this.currentLine++;
			this.startLinePos = this.currentPos;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x000450CC File Offset: 0x000440CC
		private void PPLanguageOption()
		{
			this.GetNextToken();
			this.HandleError(JSError.InvalidLanguageOption);
			this.GetNextToken();
			Context context = null;
			while (JSToken.RightParen != this.currentToken.token)
			{
				if (context == null)
				{
					context = this.currentToken.Clone();
				}
				else
				{
					context.UpdateWith(this.currentToken);
				}
				this.GetNextToken();
			}
			if (context != null)
			{
				this.HandleError(JSError.NoRightParen);
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00045134 File Offset: 0x00044134
		private object PPScanConstant()
		{
			this.GetNextToken();
			JSToken token = this.currentToken.token;
			object obj;
			switch (token)
			{
			case JSToken.True:
				return true;
			case JSToken.False:
				return false;
			case JSToken.This:
			case JSToken.Identifier:
			case JSToken.StringLiteral:
			case JSToken.LeftBracket:
			case JSToken.AccessField:
				break;
			case JSToken.IntegerLiteral:
				return Convert.ToNumber(this.currentToken.GetCode(), true, true, Missing.Value);
			case JSToken.NumericLiteral:
				return Convert.ToNumber(this.currentToken.GetCode(), false, false, Missing.Value);
			case JSToken.LeftParen:
				obj = this.PPScanExpr();
				this.GetNextToken();
				if (JSToken.RightParen != this.currentToken.token)
				{
					this.currentToken.endPos = this.currentToken.startPos + 1;
					this.currentToken.endLineNumber = this.currentLine;
					this.currentToken.endLinePos = this.startLinePos;
					this.HandleError(JSError.NoRightParen);
					this.currentPos = this.currentToken.startPos;
					return obj;
				}
				return obj;
			case JSToken.FirstOp:
				return !Convert.ToBoolean(this.PPScanConstant());
			case JSToken.BitwiseNot:
				return ~Convert.ToInt32(this.PPScanConstant());
			default:
				switch (token)
				{
				case JSToken.FirstBinaryOp:
					return Convert.ToNumber(this.PPScanConstant());
				case JSToken.Minus:
					return -Convert.ToNumber(this.PPScanConstant());
				default:
					if (token == JSToken.PreProcessorConstant)
					{
						return this.preProcessorValue;
					}
					break;
				}
				break;
			}
			this.HandleError(JSError.NotConst);
			this.currentPos = this.currentToken.startPos;
			obj = true;
			return obj;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00045308 File Offset: 0x00044308
		private object PPGetValue(JSToken op, object op1, object op2)
		{
			switch (op)
			{
			case JSToken.FirstBinaryOp:
				return Convert.ToNumber(op1) + Convert.ToNumber(op2);
			case JSToken.Minus:
				return Convert.ToNumber(op1) - Convert.ToNumber(op2);
			case JSToken.LogicalOr:
				return Convert.ToBoolean(op1) || Convert.ToBoolean(op2);
			case JSToken.LogicalAnd:
				return Convert.ToBoolean(op1) && Convert.ToBoolean(op2);
			case JSToken.BitwiseOr:
				return Convert.ToInt32(op1) | Convert.ToInt32(op2);
			case JSToken.BitwiseXor:
				return Convert.ToInt32(op1) ^ Convert.ToInt32(op2);
			case JSToken.BitwiseAnd:
				return Convert.ToInt32(op1) & Convert.ToInt32(op2);
			case JSToken.Equal:
				return Convert.ToNumber(op1) == Convert.ToNumber(op2);
			case JSToken.NotEqual:
				return Convert.ToNumber(op1) != Convert.ToNumber(op2);
			case JSToken.StrictEqual:
				return op1 == op2;
			case JSToken.StrictNotEqual:
				return op1 != op2;
			case JSToken.GreaterThan:
				return Convert.ToNumber(op1) > Convert.ToNumber(op2);
			case JSToken.LessThan:
				return Convert.ToNumber(op1) < Convert.ToNumber(op2);
			case JSToken.LessThanEqual:
				return Convert.ToNumber(op1) <= Convert.ToNumber(op2);
			case JSToken.GreaterThanEqual:
				return Convert.ToNumber(op1) >= Convert.ToNumber(op2);
			case JSToken.LeftShift:
				return Convert.ToInt32(op1) << Convert.ToInt32(op2);
			case JSToken.RightShift:
				return Convert.ToInt32(op1) >> Convert.ToInt32(op2);
			case JSToken.UnsignedRightShift:
				return (uint)Convert.ToInt32(op1) >> Convert.ToInt32(op2);
			case JSToken.Multiply:
				return Convert.ToNumber(op1) * Convert.ToNumber(op2);
			case JSToken.Divide:
				return Convert.ToNumber(op1) / Convert.ToNumber(op2);
			case JSToken.Modulo:
				return Convert.ToInt32(op1) % Convert.ToInt32(op2);
			default:
				return null;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00045523 File Offset: 0x00044523
		internal object GetPreProcessorValue()
		{
			return this.preProcessorValue;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0004552B File Offset: 0x0004452B
		private void HandleError(JSError error)
		{
			if (!this.IsAuthoring)
			{
				this.currentToken.HandleError(error);
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00045541 File Offset: 0x00044541
		public static bool IsOperator(JSToken token)
		{
			return JSToken.FirstOp <= token && token <= JSToken.Comma;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00045552 File Offset: 0x00044552
		internal static bool IsAssignmentOperator(JSToken token)
		{
			return JSToken.Assign <= token && token <= JSToken.UnsignedRightShiftAssign;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00045563 File Offset: 0x00044563
		internal static bool CanStartStatement(JSToken token)
		{
			return JSToken.If <= token && token <= JSToken.Function;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00045573 File Offset: 0x00044573
		internal static bool CanParseAsExpression(JSToken token)
		{
			return (JSToken.FirstBinaryOp <= token && token <= JSToken.Comma) || (JSToken.LeftParen <= token && token <= JSToken.AccessField);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00045590 File Offset: 0x00044590
		internal static bool IsRightAssociativeOperator(JSToken token)
		{
			return JSToken.Assign <= token && token <= JSToken.ConditionalIf;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000455A4 File Offset: 0x000445A4
		public static bool IsKeyword(JSToken token)
		{
			switch (token)
			{
			case JSToken.If:
			case JSToken.For:
			case JSToken.Do:
			case JSToken.While:
			case JSToken.Continue:
			case JSToken.Break:
			case JSToken.Return:
			case JSToken.Import:
			case JSToken.With:
			case JSToken.Switch:
			case JSToken.Throw:
			case JSToken.Try:
			case JSToken.Package:
			case JSToken.Abstract:
			case JSToken.Public:
			case JSToken.Static:
			case JSToken.Private:
			case JSToken.Protected:
			case JSToken.Final:
			case JSToken.Var:
			case JSToken.Const:
			case JSToken.Class:
			case JSToken.Function:
			case JSToken.Null:
			case JSToken.True:
			case JSToken.False:
			case JSToken.This:
			case JSToken.Delete:
			case JSToken.Void:
			case JSToken.Typeof:
				break;
			case JSToken.Internal:
			case JSToken.Event:
			case JSToken.LeftCurly:
			case JSToken.Semicolon:
			case JSToken.Identifier:
			case JSToken.StringLiteral:
			case JSToken.IntegerLiteral:
			case JSToken.NumericLiteral:
			case JSToken.LeftParen:
			case JSToken.LeftBracket:
			case JSToken.AccessField:
			case JSToken.FirstOp:
			case JSToken.BitwiseNot:
				return false;
			default:
				switch (token)
				{
				case JSToken.Instanceof:
				case JSToken.In:
				case JSToken.Case:
				case JSToken.Catch:
				case JSToken.Debugger:
				case JSToken.Default:
				case JSToken.Else:
				case JSToken.Export:
				case JSToken.Extends:
				case JSToken.Finally:
				case JSToken.Get:
				case JSToken.Implements:
				case JSToken.Interface:
				case JSToken.New:
				case JSToken.Set:
				case JSToken.Super:
				case JSToken.Boolean:
				case JSToken.Byte:
				case JSToken.Char:
				case JSToken.Double:
				case JSToken.Enum:
				case JSToken.Float:
				case JSToken.Goto:
				case JSToken.Int:
				case JSToken.Long:
				case JSToken.Native:
				case JSToken.Short:
				case JSToken.Synchronized:
				case JSToken.Transient:
				case JSToken.Throws:
				case JSToken.Volatile:
					break;
				case JSToken.Assign:
				case JSToken.PlusAssign:
				case JSToken.MinusAssign:
				case JSToken.MultiplyAssign:
				case JSToken.DivideAssign:
				case JSToken.BitwiseAndAssign:
				case JSToken.BitwiseOrAssign:
				case JSToken.BitwiseXorAssign:
				case JSToken.ModuloAssign:
				case JSToken.LeftShiftAssign:
				case JSToken.RightShiftAssign:
				case JSToken.UnsignedRightShiftAssign:
				case JSToken.ConditionalIf:
				case JSToken.Colon:
				case JSToken.Comma:
				case JSToken.RightParen:
				case JSToken.RightCurly:
				case JSToken.RightBracket:
				case JSToken.PreProcessorConstant:
				case JSToken.Comment:
				case JSToken.UnterminatedComment:
				case JSToken.Assert:
				case JSToken.Decimal:
				case JSToken.DoubleColon:
				case JSToken.Ensure:
				case JSToken.Invariant:
				case JSToken.Namespace:
				case JSToken.Require:
				case JSToken.Sbyte:
				case JSToken.ParamArray:
					return false;
				default:
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00045769 File Offset: 0x00044769
		internal static bool IsProcessableOperator(JSToken token)
		{
			return JSToken.FirstBinaryOp <= token && token <= JSToken.ConditionalIf;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0004577A File Offset: 0x0004477A
		internal static bool IsPPOperator(JSToken token)
		{
			return JSToken.FirstBinaryOp <= token && token <= JSToken.Modulo;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0004578B File Offset: 0x0004478B
		internal static OpPrec GetOperatorPrecedence(JSToken token)
		{
			return JSScanner.s_OperatorsPrec[token - JSToken.FirstBinaryOp];
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00045797 File Offset: 0x00044797
		internal static OpPrec GetPPOperatorPrecedence(JSToken token)
		{
			return JSScanner.s_PPOperatorsPrec[token - JSToken.FirstBinaryOp];
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000457A4 File Offset: 0x000447A4
		private static OpPrec[] InitOperatorsPrec()
		{
			OpPrec[] array = new OpPrec[36];
			array[0] = OpPrec.precAdditive;
			array[1] = OpPrec.precAdditive;
			array[2] = OpPrec.precLogicalOr;
			array[3] = OpPrec.precLogicalAnd;
			array[4] = OpPrec.precBitwiseOr;
			array[5] = OpPrec.precBitwiseXor;
			array[6] = OpPrec.precBitwiseAnd;
			array[7] = OpPrec.precEquality;
			array[8] = OpPrec.precEquality;
			array[9] = OpPrec.precEquality;
			array[10] = OpPrec.precEquality;
			array[21] = OpPrec.precRelational;
			array[22] = OpPrec.precRelational;
			array[11] = OpPrec.precRelational;
			array[12] = OpPrec.precRelational;
			array[13] = OpPrec.precRelational;
			array[14] = OpPrec.precRelational;
			array[15] = OpPrec.precShift;
			array[16] = OpPrec.precShift;
			array[17] = OpPrec.precShift;
			array[18] = OpPrec.precMultiplicative;
			array[19] = OpPrec.precMultiplicative;
			array[20] = OpPrec.precMultiplicative;
			array[23] = OpPrec.precAssignment;
			array[24] = OpPrec.precAssignment;
			array[25] = OpPrec.precAssignment;
			array[26] = OpPrec.precAssignment;
			array[27] = OpPrec.precAssignment;
			array[28] = OpPrec.precAssignment;
			array[29] = OpPrec.precAssignment;
			array[30] = OpPrec.precAssignment;
			array[31] = OpPrec.precAssignment;
			array[32] = OpPrec.precAssignment;
			array[33] = OpPrec.precAssignment;
			array[34] = OpPrec.precAssignment;
			array[35] = OpPrec.precConditional;
			return array;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00045878 File Offset: 0x00044878
		private static OpPrec[] InitPPOperatorsPrec()
		{
			return new OpPrec[]
			{
				OpPrec.precAdditive,
				OpPrec.precAdditive,
				OpPrec.precLogicalOr,
				OpPrec.precLogicalAnd,
				OpPrec.precBitwiseOr,
				OpPrec.precBitwiseXor,
				OpPrec.precBitwiseAnd,
				OpPrec.precEquality,
				OpPrec.precEquality,
				OpPrec.precEquality,
				OpPrec.precEquality,
				OpPrec.precRelational,
				OpPrec.precRelational,
				OpPrec.precRelational,
				OpPrec.precRelational,
				OpPrec.precShift,
				OpPrec.precShift,
				OpPrec.precShift,
				OpPrec.precMultiplicative,
				OpPrec.precMultiplicative,
				OpPrec.precMultiplicative
			};
		}

		// Token: 0x040004B2 RID: 1202
		private string strSourceCode;

		// Token: 0x040004B3 RID: 1203
		private int startPos;

		// Token: 0x040004B4 RID: 1204
		private int endPos;

		// Token: 0x040004B5 RID: 1205
		private int currentPos;

		// Token: 0x040004B6 RID: 1206
		private int currentLine;

		// Token: 0x040004B7 RID: 1207
		private int startLinePos;

		// Token: 0x040004B8 RID: 1208
		private Context currentToken;

		// Token: 0x040004B9 RID: 1209
		private string escapedString;

		// Token: 0x040004BA RID: 1210
		private StringBuilder identifier;

		// Token: 0x040004BB RID: 1211
		private int idLastPosOnBuilder;

		// Token: 0x040004BC RID: 1212
		private bool gotEndOfLine;

		// Token: 0x040004BD RID: 1213
		private bool IsAuthoring;

		// Token: 0x040004BE RID: 1214
		private bool peekModeOn;

		// Token: 0x040004BF RID: 1215
		private bool scanForDebugger;

		// Token: 0x040004C0 RID: 1216
		private JSKeyword[] keywords;

		// Token: 0x040004C1 RID: 1217
		private static readonly JSKeyword[] s_Keywords = JSKeyword.InitKeywords();

		// Token: 0x040004C2 RID: 1218
		private bool preProcessorOn;

		// Token: 0x040004C3 RID: 1219
		private int matchIf;

		// Token: 0x040004C4 RID: 1220
		private object preProcessorValue;

		// Token: 0x040004C5 RID: 1221
		private SimpleHashtable ppTable;

		// Token: 0x040004C6 RID: 1222
		private DocumentContext currentDocument;

		// Token: 0x040004C7 RID: 1223
		private Globals globals;

		// Token: 0x040004C8 RID: 1224
		private static readonly OpPrec[] s_OperatorsPrec = JSScanner.InitOperatorsPrec();

		// Token: 0x040004C9 RID: 1225
		private static readonly OpPrec[] s_PPOperatorsPrec = JSScanner.InitPPOperatorsPrec();
	}
}
