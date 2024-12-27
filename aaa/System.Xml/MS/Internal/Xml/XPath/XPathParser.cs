using System;
using System.Collections;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200016A RID: 362
	internal class XPathParser
	{
		// Token: 0x06001361 RID: 4961 RVA: 0x00053A3B File Offset: 0x00052A3B
		private XPathParser(XPathScanner scanner)
		{
			this.scanner = scanner;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00053A4C File Offset: 0x00052A4C
		public static AstNode ParseXPathExpresion(string xpathExpresion)
		{
			XPathScanner xpathScanner = new XPathScanner(xpathExpresion);
			XPathParser xpathParser = new XPathParser(xpathScanner);
			AstNode astNode = xpathParser.ParseExpresion(null);
			if (xpathScanner.Kind != XPathScanner.LexKind.Eof)
			{
				throw XPathException.Create("Xp_InvalidToken", xpathScanner.SourceText);
			}
			return astNode;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00053A8C File Offset: 0x00052A8C
		public static AstNode ParseXPathPattern(string xpathPattern)
		{
			XPathScanner xpathScanner = new XPathScanner(xpathPattern);
			XPathParser xpathParser = new XPathParser(xpathScanner);
			AstNode astNode = xpathParser.ParsePattern(null);
			if (xpathScanner.Kind != XPathScanner.LexKind.Eof)
			{
				throw XPathException.Create("Xp_InvalidToken", xpathScanner.SourceText);
			}
			return astNode;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00053ACB File Offset: 0x00052ACB
		private AstNode ParseExpresion(AstNode qyInput)
		{
			return this.ParseOrExpr(qyInput);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00053AD4 File Offset: 0x00052AD4
		private AstNode ParseOrExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseAndExpr(qyInput);
			while (this.TestOp("or"))
			{
				this.NextLex();
				astNode = new Operator(Operator.Op.OR, astNode, this.ParseAndExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00053B10 File Offset: 0x00052B10
		private AstNode ParseAndExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseEqualityExpr(qyInput);
			while (this.TestOp("and"))
			{
				this.NextLex();
				astNode = new Operator(Operator.Op.AND, astNode, this.ParseEqualityExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00053B4C File Offset: 0x00052B4C
		private AstNode ParseEqualityExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseRelationalExpr(qyInput);
			for (;;)
			{
				Operator.Op op = ((this.scanner.Kind == XPathScanner.LexKind.Eq) ? Operator.Op.EQ : ((this.scanner.Kind == XPathScanner.LexKind.Ne) ? Operator.Op.NE : Operator.Op.INVALID));
				if (op == Operator.Op.INVALID)
				{
					break;
				}
				this.NextLex();
				astNode = new Operator(op, astNode, this.ParseRelationalExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00053BA8 File Offset: 0x00052BA8
		private AstNode ParseRelationalExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseAdditiveExpr(qyInput);
			for (;;)
			{
				Operator.Op op = ((this.scanner.Kind == XPathScanner.LexKind.Lt) ? Operator.Op.LT : ((this.scanner.Kind == XPathScanner.LexKind.Le) ? Operator.Op.LE : ((this.scanner.Kind == XPathScanner.LexKind.Gt) ? Operator.Op.GT : ((this.scanner.Kind == XPathScanner.LexKind.Ge) ? Operator.Op.GE : Operator.Op.INVALID))));
				if (op == Operator.Op.INVALID)
				{
					break;
				}
				this.NextLex();
				astNode = new Operator(op, astNode, this.ParseAdditiveExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00053C28 File Offset: 0x00052C28
		private AstNode ParseAdditiveExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseMultiplicativeExpr(qyInput);
			for (;;)
			{
				Operator.Op op = ((this.scanner.Kind == XPathScanner.LexKind.Plus) ? Operator.Op.PLUS : ((this.scanner.Kind == XPathScanner.LexKind.Minus) ? Operator.Op.MINUS : Operator.Op.INVALID));
				if (op == Operator.Op.INVALID)
				{
					break;
				}
				this.NextLex();
				astNode = new Operator(op, astNode, this.ParseMultiplicativeExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00053C84 File Offset: 0x00052C84
		private AstNode ParseMultiplicativeExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParseUnaryExpr(qyInput);
			for (;;)
			{
				Operator.Op op = ((this.scanner.Kind == XPathScanner.LexKind.Star) ? Operator.Op.MUL : (this.TestOp("div") ? Operator.Op.DIV : (this.TestOp("mod") ? Operator.Op.MOD : Operator.Op.INVALID)));
				if (op == Operator.Op.INVALID)
				{
					break;
				}
				this.NextLex();
				astNode = new Operator(op, astNode, this.ParseUnaryExpr(qyInput));
			}
			return astNode;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00053CEE File Offset: 0x00052CEE
		private AstNode ParseUnaryExpr(AstNode qyInput)
		{
			if (this.scanner.Kind == XPathScanner.LexKind.Minus)
			{
				this.NextLex();
				return new Operator(Operator.Op.MUL, this.ParseUnaryExpr(qyInput), new Operand(-1.0));
			}
			return this.ParseUnionExpr(qyInput);
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00053D2C File Offset: 0x00052D2C
		private AstNode ParseUnionExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParsePathExpr(qyInput);
			while (this.scanner.Kind == XPathScanner.LexKind.Union)
			{
				this.NextLex();
				AstNode astNode2 = this.ParsePathExpr(qyInput);
				this.CheckNodeSet(astNode.ReturnType);
				this.CheckNodeSet(astNode2.ReturnType);
				astNode = new Operator(Operator.Op.UNION, astNode, astNode2);
			}
			return astNode;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00053D84 File Offset: 0x00052D84
		private static bool IsNodeType(XPathScanner scaner)
		{
			return scaner.Prefix.Length == 0 && (scaner.Name == "node" || scaner.Name == "text" || scaner.Name == "processing-instruction" || scaner.Name == "comment");
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00053DE8 File Offset: 0x00052DE8
		private AstNode ParsePathExpr(AstNode qyInput)
		{
			AstNode astNode;
			if (XPathParser.IsPrimaryExpr(this.scanner))
			{
				astNode = this.ParseFilterExpr(qyInput);
				if (this.scanner.Kind == XPathScanner.LexKind.Slash)
				{
					this.NextLex();
					astNode = this.ParseRelativeLocationPath(astNode);
				}
				else if (this.scanner.Kind == XPathScanner.LexKind.SlashSlash)
				{
					this.NextLex();
					astNode = this.ParseRelativeLocationPath(new Axis(Axis.AxisType.DescendantOrSelf, astNode));
				}
			}
			else
			{
				astNode = this.ParseLocationPath(null);
			}
			return astNode;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00053E58 File Offset: 0x00052E58
		private AstNode ParseFilterExpr(AstNode qyInput)
		{
			AstNode astNode = this.ParsePrimaryExpr(qyInput);
			while (this.scanner.Kind == XPathScanner.LexKind.LBracket)
			{
				astNode = new Filter(astNode, this.ParsePredicate(astNode));
			}
			return astNode;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00053E90 File Offset: 0x00052E90
		private AstNode ParsePredicate(AstNode qyInput)
		{
			this.CheckNodeSet(qyInput.ReturnType);
			this.PassToken(XPathScanner.LexKind.LBracket);
			AstNode astNode = this.ParseExpresion(qyInput);
			this.PassToken(XPathScanner.LexKind.RBracket);
			return astNode;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00053EC4 File Offset: 0x00052EC4
		private AstNode ParseLocationPath(AstNode qyInput)
		{
			if (this.scanner.Kind == XPathScanner.LexKind.Slash)
			{
				this.NextLex();
				AstNode astNode = new Root();
				if (XPathParser.IsStep(this.scanner.Kind))
				{
					astNode = this.ParseRelativeLocationPath(astNode);
				}
				return astNode;
			}
			if (this.scanner.Kind == XPathScanner.LexKind.SlashSlash)
			{
				this.NextLex();
				return this.ParseRelativeLocationPath(new Axis(Axis.AxisType.DescendantOrSelf, new Root()));
			}
			return this.ParseRelativeLocationPath(qyInput);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00053F38 File Offset: 0x00052F38
		private AstNode ParseRelativeLocationPath(AstNode qyInput)
		{
			AstNode astNode = this.ParseStep(qyInput);
			if (XPathScanner.LexKind.SlashSlash == this.scanner.Kind)
			{
				this.NextLex();
				astNode = this.ParseRelativeLocationPath(new Axis(Axis.AxisType.DescendantOrSelf, astNode));
			}
			else if (XPathScanner.LexKind.Slash == this.scanner.Kind)
			{
				this.NextLex();
				astNode = this.ParseRelativeLocationPath(astNode);
			}
			return astNode;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00053F90 File Offset: 0x00052F90
		private static bool IsStep(XPathScanner.LexKind lexKind)
		{
			return lexKind == XPathScanner.LexKind.Dot || lexKind == XPathScanner.LexKind.DotDot || lexKind == XPathScanner.LexKind.At || lexKind == XPathScanner.LexKind.Axe || lexKind == XPathScanner.LexKind.Star || lexKind == XPathScanner.LexKind.Name;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00053FB4 File Offset: 0x00052FB4
		private AstNode ParseStep(AstNode qyInput)
		{
			AstNode astNode;
			if (XPathScanner.LexKind.Dot == this.scanner.Kind)
			{
				this.NextLex();
				astNode = new Axis(Axis.AxisType.Self, qyInput);
			}
			else if (XPathScanner.LexKind.DotDot == this.scanner.Kind)
			{
				this.NextLex();
				astNode = new Axis(Axis.AxisType.Parent, qyInput);
			}
			else
			{
				Axis.AxisType axisType = Axis.AxisType.Child;
				XPathScanner.LexKind kind = this.scanner.Kind;
				if (kind != XPathScanner.LexKind.At)
				{
					if (kind == XPathScanner.LexKind.Axe)
					{
						axisType = this.GetAxis(this.scanner);
						this.NextLex();
					}
				}
				else
				{
					axisType = Axis.AxisType.Attribute;
					this.NextLex();
				}
				XPathNodeType xpathNodeType = ((axisType == Axis.AxisType.Attribute) ? XPathNodeType.Attribute : XPathNodeType.Element);
				astNode = this.ParseNodeTest(qyInput, axisType, xpathNodeType);
				while (XPathScanner.LexKind.LBracket == this.scanner.Kind)
				{
					astNode = new Filter(astNode, this.ParsePredicate(astNode));
				}
			}
			return astNode;
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00054070 File Offset: 0x00053070
		private AstNode ParseNodeTest(AstNode qyInput, Axis.AxisType axisType, XPathNodeType nodeType)
		{
			XPathScanner.LexKind kind = this.scanner.Kind;
			string text;
			string text2;
			if (kind != XPathScanner.LexKind.Star)
			{
				if (kind != XPathScanner.LexKind.Name)
				{
					throw XPathException.Create("Xp_NodeSetExpected", this.scanner.SourceText);
				}
				if (this.scanner.CanBeFunction && XPathParser.IsNodeType(this.scanner))
				{
					text = string.Empty;
					text2 = string.Empty;
					nodeType = ((this.scanner.Name == "comment") ? XPathNodeType.Comment : ((this.scanner.Name == "text") ? XPathNodeType.Text : ((this.scanner.Name == "node") ? XPathNodeType.All : ((this.scanner.Name == "processing-instruction") ? XPathNodeType.ProcessingInstruction : XPathNodeType.Root))));
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.LParens);
					if (nodeType == XPathNodeType.ProcessingInstruction && this.scanner.Kind != XPathScanner.LexKind.RParens)
					{
						this.CheckToken(XPathScanner.LexKind.String);
						text2 = this.scanner.StringValue;
						this.NextLex();
					}
					this.PassToken(XPathScanner.LexKind.RParens);
				}
				else
				{
					text = this.scanner.Prefix;
					text2 = this.scanner.Name;
					this.NextLex();
					if (text2 == "*")
					{
						text2 = string.Empty;
					}
				}
			}
			else
			{
				text = string.Empty;
				text2 = string.Empty;
				this.NextLex();
			}
			return new Axis(axisType, qyInput, text, text2, nodeType);
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000541E0 File Offset: 0x000531E0
		private static bool IsPrimaryExpr(XPathScanner scanner)
		{
			return scanner.Kind == XPathScanner.LexKind.String || scanner.Kind == XPathScanner.LexKind.Number || scanner.Kind == XPathScanner.LexKind.Dollar || scanner.Kind == XPathScanner.LexKind.LParens || (scanner.Kind == XPathScanner.LexKind.Name && scanner.CanBeFunction && !XPathParser.IsNodeType(scanner));
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00054234 File Offset: 0x00053234
		private AstNode ParsePrimaryExpr(AstNode qyInput)
		{
			AstNode astNode = null;
			XPathScanner.LexKind kind = this.scanner.Kind;
			if (kind <= XPathScanner.LexKind.LParens)
			{
				if (kind != XPathScanner.LexKind.Dollar)
				{
					if (kind == XPathScanner.LexKind.LParens)
					{
						this.NextLex();
						astNode = this.ParseExpresion(qyInput);
						if (astNode.Type != AstNode.AstType.ConstantOperand)
						{
							astNode = new Group(astNode);
						}
						this.PassToken(XPathScanner.LexKind.RParens);
					}
				}
				else
				{
					this.NextLex();
					this.CheckToken(XPathScanner.LexKind.Name);
					astNode = new Variable(this.scanner.Name, this.scanner.Prefix);
					this.NextLex();
				}
			}
			else if (kind != XPathScanner.LexKind.Number)
			{
				if (kind != XPathScanner.LexKind.Name)
				{
					if (kind == XPathScanner.LexKind.String)
					{
						astNode = new Operand(this.scanner.StringValue);
						this.NextLex();
					}
				}
				else if (this.scanner.CanBeFunction && !XPathParser.IsNodeType(this.scanner))
				{
					astNode = this.ParseMethod(null);
				}
			}
			else
			{
				astNode = new Operand(this.scanner.NumberValue);
				this.NextLex();
			}
			return astNode;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00054330 File Offset: 0x00053330
		private AstNode ParseMethod(AstNode qyInput)
		{
			ArrayList arrayList = new ArrayList();
			string name = this.scanner.Name;
			string prefix = this.scanner.Prefix;
			this.PassToken(XPathScanner.LexKind.Name);
			this.PassToken(XPathScanner.LexKind.LParens);
			if (this.scanner.Kind != XPathScanner.LexKind.RParens)
			{
				for (;;)
				{
					arrayList.Add(this.ParseExpresion(qyInput));
					if (this.scanner.Kind == XPathScanner.LexKind.RParens)
					{
						break;
					}
					this.PassToken(XPathScanner.LexKind.Comma);
				}
			}
			this.PassToken(XPathScanner.LexKind.RParens);
			if (prefix.Length == 0)
			{
				XPathParser.ParamInfo paramInfo = (XPathParser.ParamInfo)XPathParser.functionTable[name];
				if (paramInfo != null)
				{
					int num = arrayList.Count;
					if (num < paramInfo.Minargs)
					{
						throw XPathException.Create("Xp_InvalidNumArgs", name, this.scanner.SourceText);
					}
					if (paramInfo.FType == Function.FunctionType.FuncConcat)
					{
						for (int i = 0; i < num; i++)
						{
							AstNode astNode = (AstNode)arrayList[i];
							if (astNode.ReturnType != XPathResultType.String)
							{
								astNode = new Function(Function.FunctionType.FuncString, astNode);
							}
							arrayList[i] = astNode;
						}
					}
					else
					{
						if (paramInfo.Maxargs < num)
						{
							throw XPathException.Create("Xp_InvalidNumArgs", name, this.scanner.SourceText);
						}
						if (paramInfo.ArgTypes.Length < num)
						{
							num = paramInfo.ArgTypes.Length;
						}
						for (int j = 0; j < num; j++)
						{
							AstNode astNode2 = (AstNode)arrayList[j];
							if (paramInfo.ArgTypes[j] != XPathResultType.Any && paramInfo.ArgTypes[j] != astNode2.ReturnType)
							{
								switch (paramInfo.ArgTypes[j])
								{
								case XPathResultType.Number:
									astNode2 = new Function(Function.FunctionType.FuncNumber, astNode2);
									break;
								case XPathResultType.String:
									astNode2 = new Function(Function.FunctionType.FuncString, astNode2);
									break;
								case XPathResultType.Boolean:
									astNode2 = new Function(Function.FunctionType.FuncBoolean, astNode2);
									break;
								case XPathResultType.NodeSet:
									if (!(astNode2 is Variable) && (!(astNode2 is Function) || astNode2.ReturnType != XPathResultType.Any))
									{
										throw XPathException.Create("Xp_InvalidArgumentType", name, this.scanner.SourceText);
									}
									break;
								}
								arrayList[j] = astNode2;
							}
						}
					}
					return new Function(paramInfo.FType, arrayList);
				}
			}
			return new Function(prefix, name, arrayList);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0005455C File Offset: 0x0005355C
		private AstNode ParsePattern(AstNode qyInput)
		{
			AstNode astNode = this.ParseLocationPathPattern(qyInput);
			while (this.scanner.Kind == XPathScanner.LexKind.Union)
			{
				this.NextLex();
				astNode = new Operator(Operator.Op.UNION, astNode, this.ParseLocationPathPattern(qyInput));
			}
			return astNode;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0005459C File Offset: 0x0005359C
		private AstNode ParseLocationPathPattern(AstNode qyInput)
		{
			AstNode astNode = null;
			XPathScanner.LexKind kind = this.scanner.Kind;
			if (kind != XPathScanner.LexKind.Slash)
			{
				if (kind != XPathScanner.LexKind.SlashSlash)
				{
					if (kind == XPathScanner.LexKind.Name)
					{
						if (this.scanner.CanBeFunction)
						{
							astNode = this.ParseIdKeyPattern(qyInput);
							if (astNode != null)
							{
								XPathScanner.LexKind kind2 = this.scanner.Kind;
								if (kind2 != XPathScanner.LexKind.Slash)
								{
									if (kind2 != XPathScanner.LexKind.SlashSlash)
									{
										return astNode;
									}
									this.NextLex();
									astNode = new Axis(Axis.AxisType.DescendantOrSelf, astNode);
								}
								else
								{
									this.NextLex();
								}
							}
						}
					}
				}
				else
				{
					this.NextLex();
					astNode = new Axis(Axis.AxisType.DescendantOrSelf, new Root());
				}
			}
			else
			{
				this.NextLex();
				astNode = new Root();
				if (this.scanner.Kind == XPathScanner.LexKind.Eof || this.scanner.Kind == XPathScanner.LexKind.Union)
				{
					return astNode;
				}
			}
			return this.ParseRelativePathPattern(astNode);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0005465C File Offset: 0x0005365C
		private AstNode ParseIdKeyPattern(AstNode qyInput)
		{
			ArrayList arrayList = new ArrayList();
			if (this.scanner.Prefix.Length == 0)
			{
				if (this.scanner.Name == "id")
				{
					XPathParser.ParamInfo paramInfo = (XPathParser.ParamInfo)XPathParser.functionTable["id"];
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.LParens);
					this.CheckToken(XPathScanner.LexKind.String);
					arrayList.Add(new Operand(this.scanner.StringValue));
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.RParens);
					return new Function(paramInfo.FType, arrayList);
				}
				if (this.scanner.Name == "key")
				{
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.LParens);
					this.CheckToken(XPathScanner.LexKind.String);
					arrayList.Add(new Operand(this.scanner.StringValue));
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.Comma);
					this.CheckToken(XPathScanner.LexKind.String);
					arrayList.Add(new Operand(this.scanner.StringValue));
					this.NextLex();
					this.PassToken(XPathScanner.LexKind.RParens);
					return new Function("", "key", arrayList);
				}
			}
			return null;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0005478C File Offset: 0x0005378C
		private AstNode ParseRelativePathPattern(AstNode qyInput)
		{
			AstNode astNode = this.ParseStepPattern(qyInput);
			if (XPathScanner.LexKind.SlashSlash == this.scanner.Kind)
			{
				this.NextLex();
				astNode = this.ParseRelativePathPattern(new Axis(Axis.AxisType.DescendantOrSelf, astNode));
			}
			else if (XPathScanner.LexKind.Slash == this.scanner.Kind)
			{
				this.NextLex();
				astNode = this.ParseRelativePathPattern(astNode);
			}
			return astNode;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000547E4 File Offset: 0x000537E4
		private AstNode ParseStepPattern(AstNode qyInput)
		{
			Axis.AxisType axisType = Axis.AxisType.Child;
			XPathScanner.LexKind kind = this.scanner.Kind;
			if (kind != XPathScanner.LexKind.At)
			{
				if (kind == XPathScanner.LexKind.Axe)
				{
					axisType = this.GetAxis(this.scanner);
					if (axisType != Axis.AxisType.Child && axisType != Axis.AxisType.Attribute)
					{
						throw XPathException.Create("Xp_InvalidToken", this.scanner.SourceText);
					}
					this.NextLex();
				}
			}
			else
			{
				axisType = Axis.AxisType.Attribute;
				this.NextLex();
			}
			XPathNodeType xpathNodeType = ((axisType == Axis.AxisType.Attribute) ? XPathNodeType.Attribute : XPathNodeType.Element);
			AstNode astNode = this.ParseNodeTest(qyInput, axisType, xpathNodeType);
			while (XPathScanner.LexKind.LBracket == this.scanner.Kind)
			{
				astNode = new Filter(astNode, this.ParsePredicate(astNode));
			}
			return astNode;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00054879 File Offset: 0x00053879
		private void CheckToken(XPathScanner.LexKind t)
		{
			if (this.scanner.Kind != t)
			{
				throw XPathException.Create("Xp_InvalidToken", this.scanner.SourceText);
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0005489F File Offset: 0x0005389F
		private void PassToken(XPathScanner.LexKind t)
		{
			this.CheckToken(t);
			this.NextLex();
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000548AE File Offset: 0x000538AE
		private void NextLex()
		{
			this.scanner.NextLex();
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000548BC File Offset: 0x000538BC
		private bool TestOp(string op)
		{
			return this.scanner.Kind == XPathScanner.LexKind.Name && this.scanner.Prefix.Length == 0 && this.scanner.Name.Equals(op);
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000548F2 File Offset: 0x000538F2
		private void CheckNodeSet(XPathResultType t)
		{
			if (t != XPathResultType.NodeSet && t != XPathResultType.Any)
			{
				throw XPathException.Create("Xp_NodeSetExpected", this.scanner.SourceText);
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00054914 File Offset: 0x00053914
		private static Hashtable CreateFunctionTable()
		{
			return new Hashtable(36)
			{
				{
					"last",
					new XPathParser.ParamInfo(Function.FunctionType.FuncLast, 0, 0, XPathParser.temparray1)
				},
				{
					"position",
					new XPathParser.ParamInfo(Function.FunctionType.FuncPosition, 0, 0, XPathParser.temparray1)
				},
				{
					"name",
					new XPathParser.ParamInfo(Function.FunctionType.FuncName, 0, 1, XPathParser.temparray2)
				},
				{
					"namespace-uri",
					new XPathParser.ParamInfo(Function.FunctionType.FuncNameSpaceUri, 0, 1, XPathParser.temparray2)
				},
				{
					"local-name",
					new XPathParser.ParamInfo(Function.FunctionType.FuncLocalName, 0, 1, XPathParser.temparray2)
				},
				{
					"count",
					new XPathParser.ParamInfo(Function.FunctionType.FuncCount, 1, 1, XPathParser.temparray2)
				},
				{
					"id",
					new XPathParser.ParamInfo(Function.FunctionType.FuncID, 1, 1, XPathParser.temparray3)
				},
				{
					"string",
					new XPathParser.ParamInfo(Function.FunctionType.FuncString, 0, 1, XPathParser.temparray3)
				},
				{
					"concat",
					new XPathParser.ParamInfo(Function.FunctionType.FuncConcat, 2, 100, XPathParser.temparray4)
				},
				{
					"starts-with",
					new XPathParser.ParamInfo(Function.FunctionType.FuncStartsWith, 2, 2, XPathParser.temparray5)
				},
				{
					"contains",
					new XPathParser.ParamInfo(Function.FunctionType.FuncContains, 2, 2, XPathParser.temparray5)
				},
				{
					"substring-before",
					new XPathParser.ParamInfo(Function.FunctionType.FuncSubstringBefore, 2, 2, XPathParser.temparray5)
				},
				{
					"substring-after",
					new XPathParser.ParamInfo(Function.FunctionType.FuncSubstringAfter, 2, 2, XPathParser.temparray5)
				},
				{
					"substring",
					new XPathParser.ParamInfo(Function.FunctionType.FuncSubstring, 2, 3, XPathParser.temparray6)
				},
				{
					"string-length",
					new XPathParser.ParamInfo(Function.FunctionType.FuncStringLength, 0, 1, XPathParser.temparray4)
				},
				{
					"normalize-space",
					new XPathParser.ParamInfo(Function.FunctionType.FuncNormalize, 0, 1, XPathParser.temparray4)
				},
				{
					"translate",
					new XPathParser.ParamInfo(Function.FunctionType.FuncTranslate, 3, 3, XPathParser.temparray7)
				},
				{
					"boolean",
					new XPathParser.ParamInfo(Function.FunctionType.FuncBoolean, 1, 1, XPathParser.temparray3)
				},
				{
					"not",
					new XPathParser.ParamInfo(Function.FunctionType.FuncNot, 1, 1, XPathParser.temparray8)
				},
				{
					"true",
					new XPathParser.ParamInfo(Function.FunctionType.FuncTrue, 0, 0, XPathParser.temparray8)
				},
				{
					"false",
					new XPathParser.ParamInfo(Function.FunctionType.FuncFalse, 0, 0, XPathParser.temparray8)
				},
				{
					"lang",
					new XPathParser.ParamInfo(Function.FunctionType.FuncLang, 1, 1, XPathParser.temparray4)
				},
				{
					"number",
					new XPathParser.ParamInfo(Function.FunctionType.FuncNumber, 0, 1, XPathParser.temparray3)
				},
				{
					"sum",
					new XPathParser.ParamInfo(Function.FunctionType.FuncSum, 1, 1, XPathParser.temparray2)
				},
				{
					"floor",
					new XPathParser.ParamInfo(Function.FunctionType.FuncFloor, 1, 1, XPathParser.temparray9)
				},
				{
					"ceiling",
					new XPathParser.ParamInfo(Function.FunctionType.FuncCeiling, 1, 1, XPathParser.temparray9)
				},
				{
					"round",
					new XPathParser.ParamInfo(Function.FunctionType.FuncRound, 1, 1, XPathParser.temparray9)
				}
			};
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00054BC8 File Offset: 0x00053BC8
		private static Hashtable CreateAxesTable()
		{
			return new Hashtable(13)
			{
				{
					"ancestor",
					Axis.AxisType.Ancestor
				},
				{
					"ancestor-or-self",
					Axis.AxisType.AncestorOrSelf
				},
				{
					"attribute",
					Axis.AxisType.Attribute
				},
				{
					"child",
					Axis.AxisType.Child
				},
				{
					"descendant",
					Axis.AxisType.Descendant
				},
				{
					"descendant-or-self",
					Axis.AxisType.DescendantOrSelf
				},
				{
					"following",
					Axis.AxisType.Following
				},
				{
					"following-sibling",
					Axis.AxisType.FollowingSibling
				},
				{
					"namespace",
					Axis.AxisType.Namespace
				},
				{
					"parent",
					Axis.AxisType.Parent
				},
				{
					"preceding",
					Axis.AxisType.Preceding
				},
				{
					"preceding-sibling",
					Axis.AxisType.PrecedingSibling
				},
				{
					"self",
					Axis.AxisType.Self
				}
			};
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00054CC0 File Offset: 0x00053CC0
		private Axis.AxisType GetAxis(XPathScanner scaner)
		{
			object obj = XPathParser.AxesTable[scaner.Name];
			if (obj == null)
			{
				throw XPathException.Create("Xp_InvalidToken", this.scanner.SourceText);
			}
			return (Axis.AxisType)obj;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00054D00 File Offset: 0x00053D00
		// Note: this type is marked as 'beforefieldinit'.
		static XPathParser()
		{
			XPathResultType[] array = new XPathResultType[3];
			array[0] = XPathResultType.String;
			XPathParser.temparray6 = array;
			XPathParser.temparray7 = new XPathResultType[]
			{
				XPathResultType.String,
				XPathResultType.String,
				XPathResultType.String
			};
			XPathParser.temparray8 = new XPathResultType[] { XPathResultType.Boolean };
			XPathResultType[] array2 = new XPathResultType[1];
			XPathParser.temparray9 = array2;
			XPathParser.functionTable = XPathParser.CreateFunctionTable();
			XPathParser.AxesTable = XPathParser.CreateAxesTable();
		}

		// Token: 0x04000BF3 RID: 3059
		private XPathScanner scanner;

		// Token: 0x04000BF4 RID: 3060
		private static readonly XPathResultType[] temparray1 = new XPathResultType[0];

		// Token: 0x04000BF5 RID: 3061
		private static readonly XPathResultType[] temparray2 = new XPathResultType[] { XPathResultType.NodeSet };

		// Token: 0x04000BF6 RID: 3062
		private static readonly XPathResultType[] temparray3 = new XPathResultType[] { XPathResultType.Any };

		// Token: 0x04000BF7 RID: 3063
		private static readonly XPathResultType[] temparray4 = new XPathResultType[] { XPathResultType.String };

		// Token: 0x04000BF8 RID: 3064
		private static readonly XPathResultType[] temparray5 = new XPathResultType[]
		{
			XPathResultType.String,
			XPathResultType.String
		};

		// Token: 0x04000BF9 RID: 3065
		private static readonly XPathResultType[] temparray6;

		// Token: 0x04000BFA RID: 3066
		private static readonly XPathResultType[] temparray7;

		// Token: 0x04000BFB RID: 3067
		private static readonly XPathResultType[] temparray8;

		// Token: 0x04000BFC RID: 3068
		private static readonly XPathResultType[] temparray9;

		// Token: 0x04000BFD RID: 3069
		private static Hashtable functionTable;

		// Token: 0x04000BFE RID: 3070
		private static Hashtable AxesTable;

		// Token: 0x0200016B RID: 363
		private class ParamInfo
		{
			// Token: 0x170004B7 RID: 1207
			// (get) Token: 0x06001387 RID: 4999 RVA: 0x00054DC9 File Offset: 0x00053DC9
			public Function.FunctionType FType
			{
				get
				{
					return this.ftype;
				}
			}

			// Token: 0x170004B8 RID: 1208
			// (get) Token: 0x06001388 RID: 5000 RVA: 0x00054DD1 File Offset: 0x00053DD1
			public int Minargs
			{
				get
				{
					return this.minargs;
				}
			}

			// Token: 0x170004B9 RID: 1209
			// (get) Token: 0x06001389 RID: 5001 RVA: 0x00054DD9 File Offset: 0x00053DD9
			public int Maxargs
			{
				get
				{
					return this.maxargs;
				}
			}

			// Token: 0x170004BA RID: 1210
			// (get) Token: 0x0600138A RID: 5002 RVA: 0x00054DE1 File Offset: 0x00053DE1
			public XPathResultType[] ArgTypes
			{
				get
				{
					return this.argTypes;
				}
			}

			// Token: 0x0600138B RID: 5003 RVA: 0x00054DE9 File Offset: 0x00053DE9
			internal ParamInfo(Function.FunctionType ftype, int minargs, int maxargs, XPathResultType[] argTypes)
			{
				this.ftype = ftype;
				this.minargs = minargs;
				this.maxargs = maxargs;
				this.argTypes = argTypes;
			}

			// Token: 0x04000BFF RID: 3071
			private Function.FunctionType ftype;

			// Token: 0x04000C00 RID: 3072
			private int minargs;

			// Token: 0x04000C01 RID: 3073
			private int maxargs;

			// Token: 0x04000C02 RID: 3074
			private XPathResultType[] argTypes;
		}
	}
}
