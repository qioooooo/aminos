using System;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000E0 RID: 224
	internal sealed class XPathScanner
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x000323C4 File Offset: 0x000313C4
		public XPathScanner(string xpathExpr)
			: this(xpathExpr, 0)
		{
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x000323CE File Offset: 0x000313CE
		public XPathScanner(string xpathExpr, int startFrom)
		{
			this.xpathExpr = xpathExpr;
			this.SetSourceIndex(startFrom);
			this.NextLex();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00032405 File Offset: 0x00031405
		public string Source
		{
			get
			{
				return this.xpathExpr;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0003240D File Offset: 0x0003140D
		public LexKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00032415 File Offset: 0x00031415
		public int LexStart
		{
			get
			{
				return this.lexStart;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0003241D File Offset: 0x0003141D
		public int LexSize
		{
			get
			{
				return this.curIndex - this.lexStart;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0003242C File Offset: 0x0003142C
		public int PrevLexEnd
		{
			get
			{
				return this.prevLexEnd;
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00032434 File Offset: 0x00031434
		private void SetSourceIndex(int index)
		{
			this.curIndex = index - 1;
			this.NextChar();
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00032448 File Offset: 0x00031448
		private bool NextChar()
		{
			this.curIndex++;
			if (this.curIndex < this.xpathExpr.Length)
			{
				this.curChar = this.xpathExpr[this.curIndex];
				return true;
			}
			this.curChar = '\0';
			return false;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00032497 File Offset: 0x00031497
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0003249F File Offset: 0x0003149F
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x000324A7 File Offset: 0x000314A7
		public bool IsKeyword(string keyword)
		{
			return this.kind == LexKind.Name && this.prefix.Length == 0 && this.name.Equals(keyword);
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x000324CE File Offset: 0x000314CE
		public string RawValue
		{
			get
			{
				if (this.kind == LexKind.Eof)
				{
					return this.LexKindToString(this.kind);
				}
				return this.xpathExpr.Substring(this.lexStart, this.curIndex - this.lexStart);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00032505 File Offset: 0x00031505
		public string StringValue
		{
			get
			{
				return this.stringValue;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0003250D File Offset: 0x0003150D
		public double NumberValue
		{
			get
			{
				return this.numberValue;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00032515 File Offset: 0x00031515
		public bool CanBeFunction
		{
			get
			{
				return this.canBeFunction;
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0003251D File Offset: 0x0003151D
		private void SkipSpace()
		{
			while (this.xmlCharType.IsWhiteSpace(this.curChar) && this.NextChar())
			{
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0003253C File Offset: 0x0003153C
		public bool NextLex()
		{
			this.prevLexEnd = this.curIndex;
			this.SkipSpace();
			this.lexStart = this.curIndex;
			char c = this.curChar;
			if (c <= '@')
			{
				if (c == '\0')
				{
					this.kind = LexKind.Eof;
					return false;
				}
				switch (c)
				{
				case '!':
					this.kind = LexKind.Bang;
					this.NextChar();
					if (this.curChar == '=')
					{
						this.kind = LexKind.Ne;
						this.NextChar();
						return true;
					}
					return true;
				case '"':
				case '\'':
					this.ScanString();
					return true;
				case '#':
				case '$':
				case '(':
				case ')':
				case '*':
				case '+':
				case ',':
				case '-':
				case '=':
				case '@':
					break;
				case '%':
				case '&':
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
				case ':':
				case ';':
				case '?':
					goto IL_022C;
				case '.':
					this.kind = LexKind.Dot;
					this.NextChar();
					if (this.curChar == '.')
					{
						this.kind = LexKind.DotDot;
						this.NextChar();
						return true;
					}
					if (this.xmlCharType.IsDigit(this.curChar))
					{
						this.ScanFraction();
						return true;
					}
					return true;
				case '/':
					this.kind = LexKind.Slash;
					this.NextChar();
					if (this.curChar == '/')
					{
						this.kind = LexKind.SlashSlash;
						this.NextChar();
						return true;
					}
					return true;
				case '<':
					this.kind = LexKind.Lt;
					this.NextChar();
					if (this.curChar == '=')
					{
						this.kind = LexKind.Le;
						this.NextChar();
						return true;
					}
					return true;
				case '>':
					this.kind = LexKind.Gt;
					this.NextChar();
					if (this.curChar == '=')
					{
						this.kind = LexKind.Ge;
						this.NextChar();
						return true;
					}
					return true;
				default:
					goto IL_022C;
				}
			}
			else
			{
				switch (c)
				{
				case '[':
				case ']':
					break;
				case '\\':
					goto IL_022C;
				default:
					switch (c)
					{
					case '{':
					case '|':
					case '}':
						break;
					default:
						goto IL_022C;
					}
					break;
				}
			}
			this.kind = (LexKind)this.curChar;
			this.NextChar();
			return true;
			IL_022C:
			if (this.xmlCharType.IsDigit(this.curChar))
			{
				this.ScanNumber();
			}
			else if (this.xmlCharType.IsStartNCNameChar(this.curChar))
			{
				this.kind = LexKind.Name;
				this.name = this.ScanNCName();
				this.prefix = string.Empty;
				int num = this.curIndex;
				if (this.curChar == ':')
				{
					this.NextChar();
					if (this.curChar == ':')
					{
						this.NextChar();
						this.kind = LexKind.Axis;
					}
					else if (this.curChar == '*')
					{
						this.NextChar();
						this.prefix = this.name;
						this.name = "*";
					}
					else if (this.xmlCharType.IsStartNCNameChar(this.curChar))
					{
						this.prefix = this.name;
						this.name = this.ScanNCName();
					}
					else
					{
						this.SetSourceIndex(num);
					}
				}
				else
				{
					this.SkipSpace();
					if (this.curChar == ':')
					{
						this.NextChar();
						if (this.curChar == ':')
						{
							this.NextChar();
							this.kind = LexKind.Axis;
						}
						else
						{
							this.SetSourceIndex(num);
						}
					}
				}
				num = this.curIndex;
				this.SkipSpace();
				this.canBeFunction = this.curChar == '(';
				this.SetSourceIndex(num);
			}
			else
			{
				this.kind = LexKind.Unknown;
				this.NextChar();
			}
			return true;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000328D0 File Offset: 0x000318D0
		private void ScanNumber()
		{
			int num = this.curIndex;
			while (this.xmlCharType.IsDigit(this.curChar))
			{
				this.NextChar();
			}
			if (this.curChar == '.')
			{
				this.NextChar();
				while (this.xmlCharType.IsDigit(this.curChar))
				{
					this.NextChar();
				}
			}
			if (((int)this.curChar & -33) == 69)
			{
				this.NextChar();
				if (this.curChar == '+' || this.curChar == '-')
				{
					this.NextChar();
				}
				while (this.xmlCharType.IsDigit(this.curChar))
				{
					this.NextChar();
				}
				throw this.CreateException("XPath_ScientificNotation", new string[0]);
			}
			this.kind = LexKind.Number;
			this.numberValue = XPathConvert.StringToDouble(this.xpathExpr.Substring(num, this.curIndex - num));
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000329B4 File Offset: 0x000319B4
		private void ScanFraction()
		{
			int num = this.curIndex - 1;
			while (this.xmlCharType.IsDigit(this.curChar))
			{
				this.NextChar();
			}
			this.kind = LexKind.Number;
			this.numberValue = XPathConvert.StringToDouble(this.xpathExpr.Substring(num, this.curIndex - num));
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00032A10 File Offset: 0x00031A10
		private void ScanString()
		{
			char c = this.curChar;
			int num = this.curIndex + 1;
			while (this.NextChar())
			{
				if (this.curChar == c)
				{
					this.kind = LexKind.String;
					this.stringValue = this.xpathExpr.Substring(num, this.curIndex - num);
					this.NextChar();
					return;
				}
			}
			throw this.CreateException("XPath_UnclosedString", new string[0]);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00032A7C File Offset: 0x00031A7C
		private string ScanNCName()
		{
			int num = this.curIndex;
			while (this.xmlCharType.IsNCNameChar(this.curChar))
			{
				this.NextChar();
			}
			return this.xpathExpr.Substring(num, this.curIndex - num);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00032AC0 File Offset: 0x00031AC0
		public void PassToken(LexKind t)
		{
			this.CheckToken(t);
			this.NextLex();
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00032AD0 File Offset: 0x00031AD0
		public void CheckToken(LexKind t)
		{
			if (this.kind == t)
			{
				return;
			}
			if (t == LexKind.Eof)
			{
				throw this.CreateException("XPath_EofExpected", new string[] { this.RawValue });
			}
			throw this.CreateException("XPath_TokenExpected", new string[]
			{
				this.LexKindToString(t),
				this.RawValue
			});
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00032B30 File Offset: 0x00031B30
		public string LexKindToString(LexKind t)
		{
			if (",/@.()[]{}*+-=<>!$|".IndexOf((char)t) >= 0)
			{
				return ((char)t).ToString();
			}
			if (t <= LexKind.Unknown)
			{
				switch (t)
				{
				case LexKind.DotDot:
					return "..";
				case LexKind.Eof:
					return "<eof>";
				case (LexKind)70:
					break;
				case LexKind.Ge:
					return ">=";
				default:
					switch (t)
					{
					case LexKind.Le:
						return "<=";
					case (LexKind)77:
						break;
					case LexKind.Ne:
						return "!=";
					default:
						switch (t)
						{
						case LexKind.SlashSlash:
							return "//";
						case LexKind.Unknown:
							return "<unknown>";
						}
						break;
					}
					break;
				}
			}
			else if (t <= LexKind.Number)
			{
				if (t == LexKind.Axis)
				{
					return "<axis>";
				}
				if (t == LexKind.Number)
				{
					return "<number literal>";
				}
			}
			else
			{
				if (t == LexKind.Name)
				{
					return "<name>";
				}
				if (t == LexKind.String)
				{
					return "<string literal>";
				}
			}
			return string.Empty;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00032C07 File Offset: 0x00031C07
		public XPathCompileException CreateException(string resId, params string[] args)
		{
			return new XPathCompileException(this.xpathExpr, this.lexStart, this.curIndex, resId, args);
		}

		// Token: 0x040006D9 RID: 1753
		private string xpathExpr;

		// Token: 0x040006DA RID: 1754
		private int curIndex;

		// Token: 0x040006DB RID: 1755
		private char curChar;

		// Token: 0x040006DC RID: 1756
		private LexKind kind;

		// Token: 0x040006DD RID: 1757
		private string name;

		// Token: 0x040006DE RID: 1758
		private string prefix;

		// Token: 0x040006DF RID: 1759
		private string stringValue;

		// Token: 0x040006E0 RID: 1760
		private double numberValue = double.NaN;

		// Token: 0x040006E1 RID: 1761
		private bool canBeFunction;

		// Token: 0x040006E2 RID: 1762
		private int lexStart;

		// Token: 0x040006E3 RID: 1763
		private int prevLexEnd;

		// Token: 0x040006E4 RID: 1764
		private XmlCharType xmlCharType = XmlCharType.Instance;
	}
}
