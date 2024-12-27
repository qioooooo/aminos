using System;
using System.Collections.Generic;
using System.Xml.XmlConfiguration;
using System.Xml.XPath;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000DD RID: 221
	internal class XPathParser<Node>
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x00030B0C File Offset: 0x0002FB0C
		public Node Parse(XPathScanner scanner, IXPathBuilder<Node> builder, LexKind endLex)
		{
			Node node = default(Node);
			this.scanner = scanner;
			this.builder = builder;
			this.posInfo.Clear();
			try
			{
				builder.StartBuild();
				node = this.ParseExpr();
				scanner.CheckToken(endLex);
			}
			catch (XPathCompileException ex)
			{
				if (ex.queryString == null)
				{
					ex.queryString = scanner.Source;
					this.PopPosInfo(out ex.startChar, out ex.endChar);
				}
				throw;
			}
			finally
			{
				node = builder.EndBuild(node);
			}
			return node;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00030BA4 File Offset: 0x0002FBA4
		internal static bool IsStep(LexKind lexKind)
		{
			return lexKind == LexKind.Dot || lexKind == LexKind.DotDot || lexKind == LexKind.At || lexKind == LexKind.Axis || lexKind == LexKind.Star || lexKind == LexKind.Name;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00030BC8 File Offset: 0x0002FBC8
		private Node ParseLocationPath()
		{
			if (this.scanner.Kind == LexKind.Slash)
			{
				this.scanner.NextLex();
				Node node = this.builder.Axis(XPathAxis.Root, XPathNodeType.All, null, null);
				if (XPathParser<Node>.IsStep(this.scanner.Kind))
				{
					node = this.builder.JoinStep(node, this.ParseRelativeLocationPath());
				}
				return node;
			}
			if (this.scanner.Kind == LexKind.SlashSlash)
			{
				this.scanner.NextLex();
				return this.builder.JoinStep(this.builder.Axis(XPathAxis.Root, XPathNodeType.All, null, null), this.builder.JoinStep(this.builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativeLocationPath()));
			}
			return this.ParseRelativeLocationPath();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00030C88 File Offset: 0x0002FC88
		private Node ParseRelativeLocationPath()
		{
			if (++this.parseRelativePath > 512 && XsltConfigSection.LimitXPathComplexity)
			{
				throw this.scanner.CreateException("Xslt_CompileError2", new string[0]);
			}
			Node node = this.ParseStep();
			if (this.scanner.Kind == LexKind.Slash)
			{
				this.scanner.NextLex();
				node = this.builder.JoinStep(node, this.ParseRelativeLocationPath());
			}
			else if (this.scanner.Kind == LexKind.SlashSlash)
			{
				this.scanner.NextLex();
				node = this.builder.JoinStep(node, this.builder.JoinStep(this.builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativeLocationPath()));
			}
			this.parseRelativePath--;
			return node;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00030D5C File Offset: 0x0002FD5C
		internal static XPathAxis GetAxis(string axisName, XPathScanner scanner)
		{
			switch (axisName)
			{
			case "ancestor":
				return XPathAxis.Ancestor;
			case "ancestor-or-self":
				return XPathAxis.AncestorOrSelf;
			case "attribute":
				return XPathAxis.Attribute;
			case "child":
				return XPathAxis.Child;
			case "descendant":
				return XPathAxis.Descendant;
			case "descendant-or-self":
				return XPathAxis.DescendantOrSelf;
			case "following":
				return XPathAxis.Following;
			case "following-sibling":
				return XPathAxis.FollowingSibling;
			case "namespace":
				return XPathAxis.Namespace;
			case "parent":
				return XPathAxis.Parent;
			case "preceding":
				return XPathAxis.Preceding;
			case "preceding-sibling":
				return XPathAxis.PrecedingSibling;
			case "self":
				return XPathAxis.Self;
			}
			throw scanner.CreateException("XPath_UnknownAxis", new string[] { axisName });
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00030EB0 File Offset: 0x0002FEB0
		private Node ParseStep()
		{
			Node node;
			if (LexKind.Dot == this.scanner.Kind)
			{
				this.scanner.NextLex();
				node = this.builder.Axis(XPathAxis.Self, XPathNodeType.All, null, null);
				if (LexKind.LBracket == this.scanner.Kind)
				{
					throw this.scanner.CreateException("XPath_PredicateAfterDot", new string[0]);
				}
			}
			else if (LexKind.DotDot == this.scanner.Kind)
			{
				this.scanner.NextLex();
				node = this.builder.Axis(XPathAxis.Parent, XPathNodeType.All, null, null);
				if (LexKind.LBracket == this.scanner.Kind)
				{
					throw this.scanner.CreateException("XPath_PredicateAfterDotDot", new string[0]);
				}
			}
			else
			{
				LexKind kind = this.scanner.Kind;
				XPathAxis xpathAxis;
				if (kind <= LexKind.At)
				{
					if (kind != LexKind.Star)
					{
						if (kind != LexKind.At)
						{
							goto IL_010E;
						}
						xpathAxis = XPathAxis.Attribute;
						this.scanner.NextLex();
						goto IL_0135;
					}
				}
				else
				{
					if (kind == LexKind.Axis)
					{
						xpathAxis = XPathParser<Node>.GetAxis(this.scanner.Name, this.scanner);
						this.scanner.NextLex();
						goto IL_0135;
					}
					if (kind != LexKind.Name)
					{
						goto IL_010E;
					}
				}
				xpathAxis = XPathAxis.Child;
				goto IL_0135;
				IL_010E:
				throw this.scanner.CreateException("XPath_UnexpectedToken", new string[] { this.scanner.RawValue });
				IL_0135:
				node = this.ParseNodeTest(xpathAxis);
				while (LexKind.LBracket == this.scanner.Kind)
				{
					node = this.builder.Predicate(node, this.ParsePredicate(), XPathParser<Node>.IsReverseAxis(xpathAxis));
				}
			}
			return node;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00031025 File Offset: 0x00030025
		private static bool IsReverseAxis(XPathAxis axis)
		{
			return axis == XPathAxis.Ancestor || axis == XPathAxis.Preceding || axis == XPathAxis.AncestorOrSelf || axis == XPathAxis.PrecedingSibling;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0003103C File Offset: 0x0003003C
		private Node ParseNodeTest(XPathAxis axis)
		{
			int lexStart = this.scanner.LexStart;
			XPathNodeType xpathNodeType;
			string text;
			string text2;
			XPathParser<Node>.InternalParseNodeTest(this.scanner, axis, out xpathNodeType, out text, out text2);
			this.PushPosInfo(lexStart, this.scanner.PrevLexEnd);
			Node node = this.builder.Axis(axis, xpathNodeType, text, text2);
			this.PopPosInfo();
			return node;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00031094 File Offset: 0x00030094
		private static bool IsNodeType(XPathScanner scanner)
		{
			return scanner.Prefix.Length == 0 && (scanner.Name == "node" || scanner.Name == "text" || scanner.Name == "processing-instruction" || scanner.Name == "comment");
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x000310F8 File Offset: 0x000300F8
		private static XPathNodeType PrincipalNodeType(XPathAxis axis)
		{
			if (axis == XPathAxis.Attribute)
			{
				return XPathNodeType.Attribute;
			}
			if (axis != XPathAxis.Namespace)
			{
				return XPathNodeType.Element;
			}
			return XPathNodeType.Namespace;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00031108 File Offset: 0x00030108
		internal static void InternalParseNodeTest(XPathScanner scanner, XPathAxis axis, out XPathNodeType nodeType, out string nodePrefix, out string nodeName)
		{
			LexKind kind = scanner.Kind;
			if (kind == LexKind.Star)
			{
				nodePrefix = null;
				nodeName = null;
				nodeType = XPathParser<Node>.PrincipalNodeType(axis);
				scanner.NextLex();
				return;
			}
			if (kind != LexKind.Name)
			{
				throw scanner.CreateException("XPath_NodeTestExpected", new string[] { scanner.RawValue });
			}
			if (scanner.CanBeFunction && XPathParser<Node>.IsNodeType(scanner))
			{
				nodePrefix = null;
				nodeName = null;
				string name;
				if ((name = scanner.Name) != null)
				{
					if (name == "comment")
					{
						nodeType = XPathNodeType.Comment;
						goto IL_007A;
					}
					if (name == "text")
					{
						nodeType = XPathNodeType.Text;
						goto IL_007A;
					}
					if (name == "node")
					{
						nodeType = XPathNodeType.All;
						goto IL_007A;
					}
				}
				nodeType = XPathNodeType.ProcessingInstruction;
				IL_007A:
				scanner.NextLex();
				scanner.PassToken(LexKind.LParens);
				if (nodeType == XPathNodeType.ProcessingInstruction && scanner.Kind != LexKind.RParens)
				{
					scanner.CheckToken(LexKind.String);
					nodePrefix = string.Empty;
					nodeName = scanner.StringValue;
					scanner.NextLex();
				}
				scanner.PassToken(LexKind.RParens);
				return;
			}
			nodePrefix = scanner.Prefix;
			nodeName = scanner.Name;
			nodeType = XPathParser<Node>.PrincipalNodeType(axis);
			scanner.NextLex();
			if (nodeName == "*")
			{
				nodeName = null;
				return;
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00031240 File Offset: 0x00030240
		private Node ParsePredicate()
		{
			this.scanner.PassToken(LexKind.LBracket);
			Node node = this.ParseExpr();
			this.scanner.PassToken(LexKind.RBracket);
			return node;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00031270 File Offset: 0x00030270
		private Node ParseExpr()
		{
			if (++this.parseSubExprDepth > 1024 && XsltConfigSection.LimitXPathComplexity)
			{
				throw this.scanner.CreateException("Xslt_CompileError2", new string[0]);
			}
			Node node = this.ParseAndExpr();
			while (this.scanner.IsKeyword("or"))
			{
				this.scanner.NextLex();
				node = this.builder.Operator(XPathOperator.Or, node, this.ParseAndExpr());
			}
			this.parseSubExprDepth--;
			return node;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00031300 File Offset: 0x00030300
		private Node ParseAndExpr()
		{
			Node node = this.ParseEqualityExpr();
			while (this.scanner.IsKeyword("and"))
			{
				this.scanner.NextLex();
				node = this.builder.Operator(XPathOperator.And, node, this.ParseEqualityExpr());
			}
			return node;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0003134C File Offset: 0x0003034C
		private Node ParseEqualityExpr()
		{
			Node node = this.ParseRelationalExpr();
			bool flag;
			while ((flag = this.scanner.Kind == LexKind.Eq) || this.scanner.Kind == LexKind.Ne)
			{
				XPathOperator xpathOperator = (flag ? XPathOperator.Eq : XPathOperator.Ne);
				this.scanner.NextLex();
				node = this.builder.Operator(xpathOperator, node, this.ParseRelationalExpr());
			}
			return node;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000313B0 File Offset: 0x000303B0
		private Node ParseRelationalExpr()
		{
			Node node = this.ParseAdditiveExpr();
			for (;;)
			{
				LexKind kind = this.scanner.Kind;
				XPathOperator xpathOperator;
				switch (kind)
				{
				case LexKind.Lt:
					xpathOperator = XPathOperator.Lt;
					break;
				case LexKind.Eq:
					return node;
				case LexKind.Gt:
					xpathOperator = XPathOperator.Gt;
					break;
				default:
					if (kind != LexKind.Ge)
					{
						if (kind != LexKind.Le)
						{
							return node;
						}
						xpathOperator = XPathOperator.Le;
					}
					else
					{
						xpathOperator = XPathOperator.Ge;
					}
					break;
				}
				this.scanner.NextLex();
				node = this.builder.Operator(xpathOperator, node, this.ParseAdditiveExpr());
			}
			return node;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00031424 File Offset: 0x00030424
		private Node ParseAdditiveExpr()
		{
			Node node = this.ParseMultiplicativeExpr();
			bool flag;
			while ((flag = this.scanner.Kind == LexKind.Plus) || this.scanner.Kind == LexKind.Minus)
			{
				XPathOperator xpathOperator = (flag ? XPathOperator.Plus : XPathOperator.Minus);
				this.scanner.NextLex();
				node = this.builder.Operator(xpathOperator, node, this.ParseMultiplicativeExpr());
			}
			return node;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00031488 File Offset: 0x00030488
		private Node ParseMultiplicativeExpr()
		{
			Node node = this.ParseUnaryExpr();
			for (;;)
			{
				XPathOperator xpathOperator;
				if (this.scanner.Kind == LexKind.Star)
				{
					xpathOperator = XPathOperator.Multiply;
				}
				else if (this.scanner.IsKeyword("div"))
				{
					xpathOperator = XPathOperator.Divide;
				}
				else
				{
					if (!this.scanner.IsKeyword("mod"))
					{
						break;
					}
					xpathOperator = XPathOperator.Modulo;
				}
				this.scanner.NextLex();
				node = this.builder.Operator(xpathOperator, node, this.ParseUnaryExpr());
			}
			return node;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00031504 File Offset: 0x00030504
		private Node ParseUnaryExpr()
		{
			if (this.scanner.Kind == LexKind.Minus)
			{
				this.scanner.NextLex();
				return this.builder.Operator(XPathOperator.UnaryMinus, this.ParseUnaryExpr(), default(Node));
			}
			return this.ParseUnionExpr();
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00031550 File Offset: 0x00030550
		private Node ParseUnionExpr()
		{
			int num = this.scanner.LexStart;
			Node node = this.ParsePathExpr();
			if (this.scanner.Kind == LexKind.Union)
			{
				this.PushPosInfo(num, this.scanner.PrevLexEnd);
				node = this.builder.Operator(XPathOperator.Union, default(Node), node);
				this.PopPosInfo();
				while (this.scanner.Kind == LexKind.Union)
				{
					this.scanner.NextLex();
					num = this.scanner.LexStart;
					Node node2 = this.ParsePathExpr();
					this.PushPosInfo(num, this.scanner.PrevLexEnd);
					node = this.builder.Operator(XPathOperator.Union, node, node2);
					this.PopPosInfo();
				}
			}
			return node;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003160C File Offset: 0x0003060C
		private Node ParsePathExpr()
		{
			if (this.IsPrimaryExpr())
			{
				int lexStart = this.scanner.LexStart;
				Node node = this.ParseFilterExpr();
				int prevLexEnd = this.scanner.PrevLexEnd;
				if (this.scanner.Kind == LexKind.Slash)
				{
					this.scanner.NextLex();
					this.PushPosInfo(lexStart, prevLexEnd);
					node = this.builder.JoinStep(node, this.ParseRelativeLocationPath());
					this.PopPosInfo();
				}
				else if (this.scanner.Kind == LexKind.SlashSlash)
				{
					this.scanner.NextLex();
					this.PushPosInfo(lexStart, prevLexEnd);
					node = this.builder.JoinStep(node, this.builder.JoinStep(this.builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), this.ParseRelativeLocationPath()));
					this.PopPosInfo();
				}
				return node;
			}
			return this.ParseLocationPath();
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000316E0 File Offset: 0x000306E0
		private Node ParseFilterExpr()
		{
			int lexStart = this.scanner.LexStart;
			Node node = this.ParsePrimaryExpr();
			int prevLexEnd = this.scanner.PrevLexEnd;
			while (this.scanner.Kind == LexKind.LBracket)
			{
				this.PushPosInfo(lexStart, prevLexEnd);
				node = this.builder.Predicate(node, this.ParsePredicate(), false);
				this.PopPosInfo();
			}
			return node;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00031740 File Offset: 0x00030740
		private bool IsPrimaryExpr()
		{
			return this.scanner.Kind == LexKind.String || this.scanner.Kind == LexKind.Number || this.scanner.Kind == LexKind.Dollar || this.scanner.Kind == LexKind.LParens || (this.scanner.Kind == LexKind.Name && this.scanner.CanBeFunction && !XPathParser<Node>.IsNodeType(this.scanner));
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000317B8 File Offset: 0x000307B8
		private Node ParsePrimaryExpr()
		{
			LexKind kind = this.scanner.Kind;
			Node node;
			if (kind <= LexKind.LParens)
			{
				if (kind == LexKind.Dollar)
				{
					int lexStart = this.scanner.LexStart;
					this.scanner.NextLex();
					this.scanner.CheckToken(LexKind.Name);
					this.PushPosInfo(lexStart, this.scanner.LexStart + this.scanner.LexSize);
					node = this.builder.Variable(this.scanner.Prefix, this.scanner.Name);
					this.PopPosInfo();
					this.scanner.NextLex();
					return node;
				}
				if (kind == LexKind.LParens)
				{
					this.scanner.NextLex();
					node = this.ParseExpr();
					this.scanner.PassToken(LexKind.RParens);
					return node;
				}
			}
			else
			{
				if (kind == LexKind.Number)
				{
					node = this.builder.Number(this.scanner.NumberValue);
					this.scanner.NextLex();
					return node;
				}
				if (kind == LexKind.String)
				{
					node = this.builder.String(this.scanner.StringValue);
					this.scanner.NextLex();
					return node;
				}
			}
			node = this.ParseFunctionCall();
			return node;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000318E8 File Offset: 0x000308E8
		private Node ParseFunctionCall()
		{
			List<Node> list = new List<Node>();
			string name = this.scanner.Name;
			string prefix = this.scanner.Prefix;
			int lexStart = this.scanner.LexStart;
			this.scanner.PassToken(LexKind.Name);
			this.scanner.PassToken(LexKind.LParens);
			if (this.scanner.Kind != LexKind.RParens)
			{
				for (;;)
				{
					list.Add(this.ParseExpr());
					if (this.scanner.Kind != LexKind.Comma)
					{
						break;
					}
					this.scanner.NextLex();
				}
				this.scanner.CheckToken(LexKind.RParens);
			}
			this.scanner.NextLex();
			this.PushPosInfo(lexStart, this.scanner.PrevLexEnd);
			Node node = this.builder.Function(prefix, name, list);
			this.PopPosInfo();
			return node;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x000319B6 File Offset: 0x000309B6
		private void PushPosInfo(int startChar, int endChar)
		{
			this.posInfo.Push(startChar);
			this.posInfo.Push(endChar);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000319D0 File Offset: 0x000309D0
		private void PopPosInfo()
		{
			this.posInfo.Pop();
			this.posInfo.Pop();
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x000319EA File Offset: 0x000309EA
		private void PopPosInfo(out int startChar, out int endChar)
		{
			endChar = this.posInfo.Pop();
			startChar = this.posInfo.Pop();
		}

		// Token: 0x040006B3 RID: 1715
		private const int MaxParseRelativePathDepth = 512;

		// Token: 0x040006B4 RID: 1716
		private const int MaxParseSubExprDepth = 1024;

		// Token: 0x040006B5 RID: 1717
		private XPathScanner scanner;

		// Token: 0x040006B6 RID: 1718
		private IXPathBuilder<Node> builder;

		// Token: 0x040006B7 RID: 1719
		private Stack<int> posInfo = new Stack<int>();

		// Token: 0x040006B8 RID: 1720
		private int parseRelativePath;

		// Token: 0x040006B9 RID: 1721
		private int parseSubExprDepth;
	}
}
