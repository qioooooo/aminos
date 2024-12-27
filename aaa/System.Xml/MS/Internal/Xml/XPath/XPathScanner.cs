using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200016C RID: 364
	internal sealed class XPathScanner
	{
		// Token: 0x0600138C RID: 5004 RVA: 0x00054E10 File Offset: 0x00053E10
		public XPathScanner(string xpathExpr)
		{
			if (xpathExpr == null)
			{
				throw XPathException.Create("Xp_ExprExpected", string.Empty);
			}
			this.xpathExpr = xpathExpr;
			this.NextChar();
			this.NextLex();
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00054E65 File Offset: 0x00053E65
		public string SourceText
		{
			get
			{
				return this.xpathExpr;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x00054E6D File Offset: 0x00053E6D
		private char CurerntChar
		{
			get
			{
				return this.currentChar;
			}
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00054E78 File Offset: 0x00053E78
		private bool NextChar()
		{
			if (this.xpathExprIndex < this.xpathExpr.Length)
			{
				this.currentChar = this.xpathExpr[this.xpathExprIndex++];
				return true;
			}
			this.currentChar = '\0';
			return false;
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x00054EC4 File Offset: 0x00053EC4
		public XPathScanner.LexKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00054ECC File Offset: 0x00053ECC
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00054ED4 File Offset: 0x00053ED4
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x00054EDC File Offset: 0x00053EDC
		public string StringValue
		{
			get
			{
				return this.stringValue;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00054EE4 File Offset: 0x00053EE4
		public double NumberValue
		{
			get
			{
				return this.numberValue;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x00054EEC File Offset: 0x00053EEC
		public bool CanBeFunction
		{
			get
			{
				return this.canBeFunction;
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00054EF4 File Offset: 0x00053EF4
		private void SkipSpace()
		{
			while (this.xmlCharType.IsWhiteSpace(this.CurerntChar) && this.NextChar())
			{
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00054F14 File Offset: 0x00053F14
		public bool NextLex()
		{
			this.SkipSpace();
			char curerntChar = this.CurerntChar;
			if (curerntChar <= '@')
			{
				if (curerntChar == '\0')
				{
					this.kind = XPathScanner.LexKind.Eof;
					return false;
				}
				switch (curerntChar)
				{
				case '!':
					this.kind = XPathScanner.LexKind.Bang;
					this.NextChar();
					if (this.CurerntChar == '=')
					{
						this.kind = XPathScanner.LexKind.Ne;
						this.NextChar();
						return true;
					}
					return true;
				case '"':
				case '\'':
					this.kind = XPathScanner.LexKind.String;
					this.stringValue = this.ScanString();
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
					goto IL_022F;
				case '.':
					this.kind = XPathScanner.LexKind.Dot;
					this.NextChar();
					if (this.CurerntChar == '.')
					{
						this.kind = XPathScanner.LexKind.DotDot;
						this.NextChar();
						return true;
					}
					if (this.xmlCharType.IsDigit(this.CurerntChar))
					{
						this.kind = XPathScanner.LexKind.Number;
						this.numberValue = this.ScanFraction();
						return true;
					}
					return true;
				case '/':
					this.kind = XPathScanner.LexKind.Slash;
					this.NextChar();
					if (this.CurerntChar == '/')
					{
						this.kind = XPathScanner.LexKind.SlashSlash;
						this.NextChar();
						return true;
					}
					return true;
				case '<':
					this.kind = XPathScanner.LexKind.Lt;
					this.NextChar();
					if (this.CurerntChar == '=')
					{
						this.kind = XPathScanner.LexKind.Le;
						this.NextChar();
						return true;
					}
					return true;
				case '>':
					this.kind = XPathScanner.LexKind.Gt;
					this.NextChar();
					if (this.CurerntChar == '=')
					{
						this.kind = XPathScanner.LexKind.Ge;
						this.NextChar();
						return true;
					}
					return true;
				default:
					goto IL_022F;
				}
			}
			else
			{
				switch (curerntChar)
				{
				case '[':
				case ']':
					break;
				case '\\':
					goto IL_022F;
				default:
					if (curerntChar != '|')
					{
						goto IL_022F;
					}
					break;
				}
			}
			this.kind = (XPathScanner.LexKind)Convert.ToInt32(this.CurerntChar, CultureInfo.InvariantCulture);
			this.NextChar();
			return true;
			IL_022F:
			if (this.xmlCharType.IsDigit(this.CurerntChar))
			{
				this.kind = XPathScanner.LexKind.Number;
				this.numberValue = this.ScanNumber();
			}
			else
			{
				if (!this.xmlCharType.IsStartNCNameChar(this.CurerntChar))
				{
					throw XPathException.Create("Xp_InvalidToken", this.SourceText);
				}
				this.kind = XPathScanner.LexKind.Name;
				this.name = this.ScanName();
				this.prefix = string.Empty;
				if (this.CurerntChar == ':')
				{
					this.NextChar();
					if (this.CurerntChar == ':')
					{
						this.NextChar();
						this.kind = XPathScanner.LexKind.Axe;
					}
					else
					{
						this.prefix = this.name;
						if (this.CurerntChar == '*')
						{
							this.NextChar();
							this.name = "*";
						}
						else
						{
							if (!this.xmlCharType.IsStartNCNameChar(this.CurerntChar))
							{
								throw XPathException.Create("Xp_InvalidName", this.SourceText);
							}
							this.name = this.ScanName();
						}
					}
				}
				else
				{
					this.SkipSpace();
					if (this.CurerntChar == ':')
					{
						this.NextChar();
						if (this.CurerntChar != ':')
						{
							throw XPathException.Create("Xp_InvalidName", this.SourceText);
						}
						this.NextChar();
						this.kind = XPathScanner.LexKind.Axe;
					}
				}
				this.SkipSpace();
				this.canBeFunction = this.CurerntChar == '(';
			}
			return true;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x000552AC File Offset: 0x000542AC
		private double ScanNumber()
		{
			int num = this.xpathExprIndex - 1;
			int num2 = 0;
			while (this.xmlCharType.IsDigit(this.CurerntChar))
			{
				this.NextChar();
				num2++;
			}
			if (this.CurerntChar == '.')
			{
				this.NextChar();
				num2++;
				while (this.xmlCharType.IsDigit(this.CurerntChar))
				{
					this.NextChar();
					num2++;
				}
			}
			return XmlConvert.ToXPathDouble(this.xpathExpr.Substring(num, num2));
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0005532C File Offset: 0x0005432C
		private double ScanFraction()
		{
			int num = this.xpathExprIndex - 2;
			int num2 = 1;
			while (this.xmlCharType.IsDigit(this.CurerntChar))
			{
				this.NextChar();
				num2++;
			}
			return XmlConvert.ToXPathDouble(this.xpathExpr.Substring(num, num2));
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00055378 File Offset: 0x00054378
		private string ScanString()
		{
			char curerntChar = this.CurerntChar;
			this.NextChar();
			int num = this.xpathExprIndex - 1;
			int num2 = 0;
			while (this.CurerntChar != curerntChar)
			{
				if (!this.NextChar())
				{
					throw XPathException.Create("Xp_UnclosedString");
				}
				num2++;
			}
			this.NextChar();
			return this.xpathExpr.Substring(num, num2);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000553D4 File Offset: 0x000543D4
		private string ScanName()
		{
			int num = this.xpathExprIndex - 1;
			int num2 = 0;
			while (this.xmlCharType.IsNCNameChar(this.CurerntChar))
			{
				this.NextChar();
				num2++;
			}
			return this.xpathExpr.Substring(num, num2);
		}

		// Token: 0x04000C03 RID: 3075
		private string xpathExpr;

		// Token: 0x04000C04 RID: 3076
		private int xpathExprIndex;

		// Token: 0x04000C05 RID: 3077
		private XPathScanner.LexKind kind;

		// Token: 0x04000C06 RID: 3078
		private char currentChar;

		// Token: 0x04000C07 RID: 3079
		private string name;

		// Token: 0x04000C08 RID: 3080
		private string prefix;

		// Token: 0x04000C09 RID: 3081
		private string stringValue;

		// Token: 0x04000C0A RID: 3082
		private double numberValue = double.NaN;

		// Token: 0x04000C0B RID: 3083
		private bool canBeFunction;

		// Token: 0x04000C0C RID: 3084
		private XmlCharType xmlCharType = XmlCharType.Instance;

		// Token: 0x0200016D RID: 365
		public enum LexKind
		{
			// Token: 0x04000C0E RID: 3086
			Comma = 44,
			// Token: 0x04000C0F RID: 3087
			Slash = 47,
			// Token: 0x04000C10 RID: 3088
			At = 64,
			// Token: 0x04000C11 RID: 3089
			Dot = 46,
			// Token: 0x04000C12 RID: 3090
			LParens = 40,
			// Token: 0x04000C13 RID: 3091
			RParens,
			// Token: 0x04000C14 RID: 3092
			LBracket = 91,
			// Token: 0x04000C15 RID: 3093
			RBracket = 93,
			// Token: 0x04000C16 RID: 3094
			Star = 42,
			// Token: 0x04000C17 RID: 3095
			Plus,
			// Token: 0x04000C18 RID: 3096
			Minus = 45,
			// Token: 0x04000C19 RID: 3097
			Eq = 61,
			// Token: 0x04000C1A RID: 3098
			Lt = 60,
			// Token: 0x04000C1B RID: 3099
			Gt = 62,
			// Token: 0x04000C1C RID: 3100
			Bang = 33,
			// Token: 0x04000C1D RID: 3101
			Dollar = 36,
			// Token: 0x04000C1E RID: 3102
			Apos = 39,
			// Token: 0x04000C1F RID: 3103
			Quote = 34,
			// Token: 0x04000C20 RID: 3104
			Union = 124,
			// Token: 0x04000C21 RID: 3105
			Ne = 78,
			// Token: 0x04000C22 RID: 3106
			Le = 76,
			// Token: 0x04000C23 RID: 3107
			Ge = 71,
			// Token: 0x04000C24 RID: 3108
			And = 65,
			// Token: 0x04000C25 RID: 3109
			Or = 79,
			// Token: 0x04000C26 RID: 3110
			DotDot = 68,
			// Token: 0x04000C27 RID: 3111
			SlashSlash = 83,
			// Token: 0x04000C28 RID: 3112
			Name = 110,
			// Token: 0x04000C29 RID: 3113
			String = 115,
			// Token: 0x04000C2A RID: 3114
			Number = 100,
			// Token: 0x04000C2B RID: 3115
			Axe = 97,
			// Token: 0x04000C2C RID: 3116
			Eof = 69
		}
	}
}
