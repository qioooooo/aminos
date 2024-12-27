using System;
using System.Collections.Generic;
using System.Xml.XmlConfiguration;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F0 RID: 240
	internal class XPathPatternParser
	{
		// Token: 0x06000ACD RID: 2765 RVA: 0x00033E30 File Offset: 0x00032E30
		public QilNode Parse(XPathScanner scanner, XPathPatternParser.IPatternBuilder ptrnBuilder)
		{
			QilNode qilNode = null;
			ptrnBuilder.StartBuild();
			try
			{
				this.scanner = scanner;
				this.ptrnBuilder = ptrnBuilder;
				qilNode = this.ParsePattern();
				this.scanner.CheckToken(LexKind.Eof);
			}
			finally
			{
				qilNode = ptrnBuilder.EndBuild(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00033E84 File Offset: 0x00032E84
		private QilNode ParsePattern()
		{
			QilNode qilNode = this.ParseLocationPathPattern();
			while (this.scanner.Kind == LexKind.Union)
			{
				this.scanner.NextLex();
				qilNode = this.ptrnBuilder.Operator(XPathOperator.Union, qilNode, this.ParseLocationPathPattern());
			}
			return qilNode;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00033ECC File Offset: 0x00032ECC
		private QilNode ParseLocationPathPattern()
		{
			LexKind kind = this.scanner.Kind;
			if (kind == LexKind.Slash)
			{
				this.scanner.NextLex();
				QilNode qilNode = this.ptrnBuilder.Axis(XPathAxis.Root, XPathNodeType.All, null, null);
				if (XPathParser<QilNode>.IsStep(this.scanner.Kind))
				{
					qilNode = this.ptrnBuilder.JoinStep(qilNode, this.ParseRelativePathPattern());
				}
				return qilNode;
			}
			if (kind != LexKind.SlashSlash)
			{
				if (kind == LexKind.Name)
				{
					if (this.scanner.CanBeFunction && this.scanner.Prefix.Length == 0 && (this.scanner.Name == "id" || this.scanner.Name == "key"))
					{
						QilNode qilNode = this.ParseIdKeyPattern();
						LexKind kind2 = this.scanner.Kind;
						if (kind2 != LexKind.Slash)
						{
							if (kind2 == LexKind.SlashSlash)
							{
								this.scanner.NextLex();
								qilNode = this.ptrnBuilder.JoinStep(qilNode, this.ptrnBuilder.JoinStep(this.ptrnBuilder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativePathPattern()));
							}
						}
						else
						{
							this.scanner.NextLex();
							qilNode = this.ptrnBuilder.JoinStep(qilNode, this.ParseRelativePathPattern());
						}
						return qilNode;
					}
				}
				return this.ParseRelativePathPattern();
			}
			this.scanner.NextLex();
			return this.ptrnBuilder.JoinStep(this.ptrnBuilder.Axis(XPathAxis.Root, XPathNodeType.All, null, null), this.ptrnBuilder.JoinStep(this.ptrnBuilder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativePathPattern()));
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00034064 File Offset: 0x00033064
		private QilNode ParseIdKeyPattern()
		{
			List<QilNode> list = new List<QilNode>(2);
			if (this.scanner.Name == "id")
			{
				this.scanner.NextLex();
				this.scanner.PassToken(LexKind.LParens);
				this.scanner.CheckToken(LexKind.String);
				list.Add(this.ptrnBuilder.String(this.scanner.StringValue));
				this.scanner.NextLex();
				this.scanner.PassToken(LexKind.RParens);
				return this.ptrnBuilder.Function("", "id", list);
			}
			this.scanner.NextLex();
			this.scanner.PassToken(LexKind.LParens);
			this.scanner.CheckToken(LexKind.String);
			list.Add(this.ptrnBuilder.String(this.scanner.StringValue));
			this.scanner.NextLex();
			this.scanner.PassToken(LexKind.Comma);
			this.scanner.CheckToken(LexKind.String);
			list.Add(this.ptrnBuilder.String(this.scanner.StringValue));
			this.scanner.NextLex();
			this.scanner.PassToken(LexKind.RParens);
			return this.ptrnBuilder.Function("", "key", list);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000341B4 File Offset: 0x000331B4
		private QilNode ParseRelativePathPattern()
		{
			if (++this.parseRelativePath > 512 && XsltConfigSection.LimitXPathComplexity)
			{
				throw this.scanner.CreateException("Xslt_CompileError2", new string[0]);
			}
			QilNode qilNode = this.ParseStepPattern();
			if (this.scanner.Kind == LexKind.Slash)
			{
				this.scanner.NextLex();
				qilNode = this.ptrnBuilder.JoinStep(qilNode, this.ParseRelativePathPattern());
			}
			else if (this.scanner.Kind == LexKind.SlashSlash)
			{
				this.scanner.NextLex();
				qilNode = this.ptrnBuilder.JoinStep(qilNode, this.ptrnBuilder.JoinStep(this.ptrnBuilder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativePathPattern()));
			}
			this.parseRelativePath--;
			return qilNode;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00034288 File Offset: 0x00033288
		private QilNode ParseStepPattern()
		{
			LexKind kind = this.scanner.Kind;
			XPathAxis xpathAxis;
			if (kind <= LexKind.At)
			{
				if (kind == LexKind.Star)
				{
					goto IL_00AC;
				}
				if (kind != LexKind.Dot)
				{
					if (kind != LexKind.At)
					{
						goto IL_00B0;
					}
					xpathAxis = XPathAxis.Attribute;
					this.scanner.NextLex();
					goto IL_00DA;
				}
			}
			else if (kind != LexKind.DotDot)
			{
				if (kind != LexKind.Axis)
				{
					if (kind != LexKind.Name)
					{
						goto IL_00B0;
					}
					goto IL_00AC;
				}
				else
				{
					xpathAxis = XPathParser<QilNode>.GetAxis(this.scanner.Name, this.scanner);
					if (xpathAxis != XPathAxis.Child && xpathAxis != XPathAxis.Attribute)
					{
						throw this.scanner.CreateException("XPath_InvalidAxisInPattern", new string[0]);
					}
					this.scanner.NextLex();
					goto IL_00DA;
				}
			}
			throw this.scanner.CreateException("XPath_InvalidAxisInPattern", new string[0]);
			IL_00AC:
			xpathAxis = XPathAxis.Child;
			goto IL_00DA;
			IL_00B0:
			throw this.scanner.CreateException("XPath_UnexpectedToken", new string[] { this.scanner.RawValue });
			IL_00DA:
			XPathNodeType xpathNodeType;
			string text;
			string text2;
			XPathParser<QilNode>.InternalParseNodeTest(this.scanner, xpathAxis, out xpathNodeType, out text, out text2);
			QilNode qilNode = this.ptrnBuilder.Axis(xpathAxis, xpathNodeType, text, text2);
			while (this.scanner.Kind == LexKind.LBracket)
			{
				qilNode = this.ptrnBuilder.Predicate(qilNode, this.ParsePredicate(qilNode), false);
			}
			return qilNode;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x000343BC File Offset: 0x000333BC
		private QilNode ParsePredicate(QilNode context)
		{
			this.scanner.NextLex();
			QilNode qilNode = this.predicateParser.Parse(this.scanner, this.ptrnBuilder.GetPredicateBuilder(context), LexKind.RBracket);
			this.scanner.NextLex();
			return qilNode;
		}

		// Token: 0x04000738 RID: 1848
		private const int MaxParseRelativePathDepth = 512;

		// Token: 0x04000739 RID: 1849
		private XPathScanner scanner;

		// Token: 0x0400073A RID: 1850
		private XPathPatternParser.IPatternBuilder ptrnBuilder;

		// Token: 0x0400073B RID: 1851
		private XPathParser<QilNode> predicateParser = new XPathParser<QilNode>();

		// Token: 0x0400073C RID: 1852
		private int parseRelativePath;

		// Token: 0x020000F1 RID: 241
		public interface IPatternBuilder : IXPathBuilder<QilNode>
		{
			// Token: 0x06000AD5 RID: 2773
			IXPathBuilder<QilNode> GetPredicateBuilder(QilNode context);
		}
	}
}
