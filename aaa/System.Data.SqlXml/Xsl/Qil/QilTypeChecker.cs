using System;
using System.Diagnostics;
using System.Xml.Schema;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200005D RID: 93
	internal class QilTypeChecker
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x000221A8 File Offset: 0x000211A8
		public XmlQueryType Check(QilNode n)
		{
			switch (n.NodeType)
			{
			case QilNodeType.QilExpression:
				return this.CheckQilExpression((QilExpression)n);
			case QilNodeType.FunctionList:
				return this.CheckFunctionList((QilList)n);
			case QilNodeType.GlobalVariableList:
				return this.CheckGlobalVariableList((QilList)n);
			case QilNodeType.GlobalParameterList:
				return this.CheckGlobalParameterList((QilList)n);
			case QilNodeType.ActualParameterList:
				return this.CheckActualParameterList((QilList)n);
			case QilNodeType.FormalParameterList:
				return this.CheckFormalParameterList((QilList)n);
			case QilNodeType.SortKeyList:
				return this.CheckSortKeyList((QilList)n);
			case QilNodeType.BranchList:
				return this.CheckBranchList((QilList)n);
			case QilNodeType.OptimizeBarrier:
				return this.CheckOptimizeBarrier((QilUnary)n);
			case QilNodeType.Unknown:
				return this.CheckUnknown(n);
			case QilNodeType.DataSource:
				return this.CheckDataSource((QilDataSource)n);
			case QilNodeType.Nop:
				return this.CheckNop((QilUnary)n);
			case QilNodeType.Error:
				return this.CheckError((QilUnary)n);
			case QilNodeType.Warning:
				return this.CheckWarning((QilUnary)n);
			case QilNodeType.For:
				return this.CheckFor((QilIterator)n);
			case QilNodeType.Let:
				return this.CheckLet((QilIterator)n);
			case QilNodeType.Parameter:
				return this.CheckParameter((QilParameter)n);
			case QilNodeType.PositionOf:
				return this.CheckPositionOf((QilUnary)n);
			case QilNodeType.True:
				return this.CheckTrue(n);
			case QilNodeType.False:
				return this.CheckFalse(n);
			case QilNodeType.LiteralString:
				return this.CheckLiteralString((QilLiteral)n);
			case QilNodeType.LiteralInt32:
				return this.CheckLiteralInt32((QilLiteral)n);
			case QilNodeType.LiteralInt64:
				return this.CheckLiteralInt64((QilLiteral)n);
			case QilNodeType.LiteralDouble:
				return this.CheckLiteralDouble((QilLiteral)n);
			case QilNodeType.LiteralDecimal:
				return this.CheckLiteralDecimal((QilLiteral)n);
			case QilNodeType.LiteralQName:
				return this.CheckLiteralQName((QilName)n);
			case QilNodeType.LiteralType:
				return this.CheckLiteralType((QilLiteral)n);
			case QilNodeType.LiteralObject:
				return this.CheckLiteralObject((QilLiteral)n);
			case QilNodeType.And:
				return this.CheckAnd((QilBinary)n);
			case QilNodeType.Or:
				return this.CheckOr((QilBinary)n);
			case QilNodeType.Not:
				return this.CheckNot((QilUnary)n);
			case QilNodeType.Conditional:
				return this.CheckConditional((QilTernary)n);
			case QilNodeType.Choice:
				return this.CheckChoice((QilChoice)n);
			case QilNodeType.Length:
				return this.CheckLength((QilUnary)n);
			case QilNodeType.Sequence:
				return this.CheckSequence((QilList)n);
			case QilNodeType.Union:
				return this.CheckUnion((QilBinary)n);
			case QilNodeType.Intersection:
				return this.CheckIntersection((QilBinary)n);
			case QilNodeType.Difference:
				return this.CheckDifference((QilBinary)n);
			case QilNodeType.Average:
				return this.CheckAverage((QilUnary)n);
			case QilNodeType.Sum:
				return this.CheckSum((QilUnary)n);
			case QilNodeType.Minimum:
				return this.CheckMinimum((QilUnary)n);
			case QilNodeType.Maximum:
				return this.CheckMaximum((QilUnary)n);
			case QilNodeType.Negate:
				return this.CheckNegate((QilUnary)n);
			case QilNodeType.Add:
				return this.CheckAdd((QilBinary)n);
			case QilNodeType.Subtract:
				return this.CheckSubtract((QilBinary)n);
			case QilNodeType.Multiply:
				return this.CheckMultiply((QilBinary)n);
			case QilNodeType.Divide:
				return this.CheckDivide((QilBinary)n);
			case QilNodeType.Modulo:
				return this.CheckModulo((QilBinary)n);
			case QilNodeType.StrLength:
				return this.CheckStrLength((QilUnary)n);
			case QilNodeType.StrConcat:
				return this.CheckStrConcat((QilStrConcat)n);
			case QilNodeType.StrParseQName:
				return this.CheckStrParseQName((QilBinary)n);
			case QilNodeType.Ne:
				return this.CheckNe((QilBinary)n);
			case QilNodeType.Eq:
				return this.CheckEq((QilBinary)n);
			case QilNodeType.Gt:
				return this.CheckGt((QilBinary)n);
			case QilNodeType.Ge:
				return this.CheckGe((QilBinary)n);
			case QilNodeType.Lt:
				return this.CheckLt((QilBinary)n);
			case QilNodeType.Le:
				return this.CheckLe((QilBinary)n);
			case QilNodeType.Is:
				return this.CheckIs((QilBinary)n);
			case QilNodeType.After:
				return this.CheckAfter((QilBinary)n);
			case QilNodeType.Before:
				return this.CheckBefore((QilBinary)n);
			case QilNodeType.Loop:
				return this.CheckLoop((QilLoop)n);
			case QilNodeType.Filter:
				return this.CheckFilter((QilLoop)n);
			case QilNodeType.Sort:
				return this.CheckSort((QilLoop)n);
			case QilNodeType.SortKey:
				return this.CheckSortKey((QilSortKey)n);
			case QilNodeType.DocOrderDistinct:
				return this.CheckDocOrderDistinct((QilUnary)n);
			case QilNodeType.Function:
				return this.CheckFunction((QilFunction)n);
			case QilNodeType.Invoke:
				return this.CheckInvoke((QilInvoke)n);
			case QilNodeType.Content:
				return this.CheckContent((QilUnary)n);
			case QilNodeType.Attribute:
				return this.CheckAttribute((QilBinary)n);
			case QilNodeType.Parent:
				return this.CheckParent((QilUnary)n);
			case QilNodeType.Root:
				return this.CheckRoot((QilUnary)n);
			case QilNodeType.XmlContext:
				return this.CheckXmlContext(n);
			case QilNodeType.Descendant:
				return this.CheckDescendant((QilUnary)n);
			case QilNodeType.DescendantOrSelf:
				return this.CheckDescendantOrSelf((QilUnary)n);
			case QilNodeType.Ancestor:
				return this.CheckAncestor((QilUnary)n);
			case QilNodeType.AncestorOrSelf:
				return this.CheckAncestorOrSelf((QilUnary)n);
			case QilNodeType.Preceding:
				return this.CheckPreceding((QilUnary)n);
			case QilNodeType.FollowingSibling:
				return this.CheckFollowingSibling((QilUnary)n);
			case QilNodeType.PrecedingSibling:
				return this.CheckPrecedingSibling((QilUnary)n);
			case QilNodeType.NodeRange:
				return this.CheckNodeRange((QilBinary)n);
			case QilNodeType.Deref:
				return this.CheckDeref((QilBinary)n);
			case QilNodeType.ElementCtor:
				return this.CheckElementCtor((QilBinary)n);
			case QilNodeType.AttributeCtor:
				return this.CheckAttributeCtor((QilBinary)n);
			case QilNodeType.CommentCtor:
				return this.CheckCommentCtor((QilUnary)n);
			case QilNodeType.PICtor:
				return this.CheckPICtor((QilBinary)n);
			case QilNodeType.TextCtor:
				return this.CheckTextCtor((QilUnary)n);
			case QilNodeType.RawTextCtor:
				return this.CheckRawTextCtor((QilUnary)n);
			case QilNodeType.DocumentCtor:
				return this.CheckDocumentCtor((QilUnary)n);
			case QilNodeType.NamespaceDecl:
				return this.CheckNamespaceDecl((QilBinary)n);
			case QilNodeType.RtfCtor:
				return this.CheckRtfCtor((QilBinary)n);
			case QilNodeType.NameOf:
				return this.CheckNameOf((QilUnary)n);
			case QilNodeType.LocalNameOf:
				return this.CheckLocalNameOf((QilUnary)n);
			case QilNodeType.NamespaceUriOf:
				return this.CheckNamespaceUriOf((QilUnary)n);
			case QilNodeType.PrefixOf:
				return this.CheckPrefixOf((QilUnary)n);
			case QilNodeType.TypeAssert:
				return this.CheckTypeAssert((QilTargetType)n);
			case QilNodeType.IsType:
				return this.CheckIsType((QilTargetType)n);
			case QilNodeType.IsEmpty:
				return this.CheckIsEmpty((QilUnary)n);
			case QilNodeType.XPathNodeValue:
				return this.CheckXPathNodeValue((QilUnary)n);
			case QilNodeType.XPathFollowing:
				return this.CheckXPathFollowing((QilUnary)n);
			case QilNodeType.XPathPreceding:
				return this.CheckXPathPreceding((QilUnary)n);
			case QilNodeType.XPathNamespace:
				return this.CheckXPathNamespace((QilUnary)n);
			case QilNodeType.XsltGenerateId:
				return this.CheckXsltGenerateId((QilUnary)n);
			case QilNodeType.XsltInvokeLateBound:
				return this.CheckXsltInvokeLateBound((QilInvokeLateBound)n);
			case QilNodeType.XsltInvokeEarlyBound:
				return this.CheckXsltInvokeEarlyBound((QilInvokeEarlyBound)n);
			case QilNodeType.XsltCopy:
				return this.CheckXsltCopy((QilBinary)n);
			case QilNodeType.XsltCopyOf:
				return this.CheckXsltCopyOf((QilUnary)n);
			case QilNodeType.XsltConvert:
				return this.CheckXsltConvert((QilTargetType)n);
			default:
				return this.CheckUnknown(n);
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000228D5 File Offset: 0x000218D5
		public XmlQueryType CheckQilExpression(QilExpression node)
		{
			return XmlQueryTypeFactory.ItemS;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000228DC File Offset: 0x000218DC
		public XmlQueryType CheckFunctionList(QilList node)
		{
			foreach (QilNode qilNode in node)
			{
			}
			return node.XmlType;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00022924 File Offset: 0x00021924
		public XmlQueryType CheckGlobalVariableList(QilList node)
		{
			foreach (QilNode qilNode in node)
			{
			}
			return node.XmlType;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002296C File Offset: 0x0002196C
		public XmlQueryType CheckGlobalParameterList(QilList node)
		{
			foreach (QilNode qilNode in node)
			{
			}
			return node.XmlType;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000229B4 File Offset: 0x000219B4
		public XmlQueryType CheckActualParameterList(QilList node)
		{
			return node.XmlType;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000229BC File Offset: 0x000219BC
		public XmlQueryType CheckFormalParameterList(QilList node)
		{
			foreach (QilNode qilNode in node)
			{
			}
			return node.XmlType;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00022A04 File Offset: 0x00021A04
		public XmlQueryType CheckSortKeyList(QilList node)
		{
			foreach (QilNode qilNode in node)
			{
			}
			return node.XmlType;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00022A4C File Offset: 0x00021A4C
		public XmlQueryType CheckBranchList(QilList node)
		{
			return node.XmlType;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00022A54 File Offset: 0x00021A54
		public XmlQueryType CheckOptimizeBarrier(QilUnary node)
		{
			return node.Child.XmlType;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00022A61 File Offset: 0x00021A61
		public XmlQueryType CheckNoDefaultValue(QilNode node)
		{
			return XmlQueryTypeFactory.None;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00022A68 File Offset: 0x00021A68
		public XmlQueryType CheckUnknown(QilNode node)
		{
			return node.XmlType;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00022A70 File Offset: 0x00021A70
		public XmlQueryType CheckDataSource(QilDataSource node)
		{
			return XmlQueryTypeFactory.NodeNotRtfQ;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00022A77 File Offset: 0x00021A77
		public XmlQueryType CheckNop(QilUnary node)
		{
			return node.Child.XmlType;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00022A84 File Offset: 0x00021A84
		public XmlQueryType CheckError(QilUnary node)
		{
			return XmlQueryTypeFactory.None;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00022A8B File Offset: 0x00021A8B
		public XmlQueryType CheckWarning(QilUnary node)
		{
			return XmlQueryTypeFactory.Empty;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00022A92 File Offset: 0x00021A92
		public XmlQueryType CheckFor(QilIterator node)
		{
			return node.Binding.XmlType.Prime;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00022AA4 File Offset: 0x00021AA4
		public XmlQueryType CheckLet(QilIterator node)
		{
			return node.Binding.XmlType;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00022AB1 File Offset: 0x00021AB1
		public XmlQueryType CheckParameter(QilParameter node)
		{
			return node.XmlType;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00022AB9 File Offset: 0x00021AB9
		public XmlQueryType CheckPositionOf(QilUnary node)
		{
			return XmlQueryTypeFactory.IntX;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00022AC0 File Offset: 0x00021AC0
		public XmlQueryType CheckTrue(QilNode node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00022AC7 File Offset: 0x00021AC7
		public XmlQueryType CheckFalse(QilNode node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00022ACE File Offset: 0x00021ACE
		public XmlQueryType CheckLiteralString(QilLiteral node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00022AD5 File Offset: 0x00021AD5
		public XmlQueryType CheckLiteralInt32(QilLiteral node)
		{
			return XmlQueryTypeFactory.IntX;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00022ADC File Offset: 0x00021ADC
		public XmlQueryType CheckLiteralInt64(QilLiteral node)
		{
			return XmlQueryTypeFactory.IntegerX;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00022AE3 File Offset: 0x00021AE3
		public XmlQueryType CheckLiteralDouble(QilLiteral node)
		{
			return XmlQueryTypeFactory.DoubleX;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00022AEA File Offset: 0x00021AEA
		public XmlQueryType CheckLiteralDecimal(QilLiteral node)
		{
			/*
An exception occurred when decompiling this method (0600062E)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Xml.Xsl.XmlQueryType System.Xml.Xsl.Qil.QilTypeChecker::CheckLiteralDecimal(System.Xml.Xsl.Qil.QilLiteral)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.FlowAnalysis.ControlFlowGraph.<>c.<ComputeDominanceFrontier>b__15_1(ControlFlowNode n) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\FlowAnalysis\ControlFlowGraph.cs:line 161
   at ICSharpCode.Decompiler.FlowAnalysis.ControlFlowNode.TraversePostOrder(Func`2 children, Action`1 visitAction) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\FlowAnalysis\ControlFlowNode.cs:line 247
   at ICSharpCode.Decompiler.FlowAnalysis.ControlFlowNode.TraversePostOrder(Func`2 children, Action`1 visitAction) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\FlowAnalysis\ControlFlowNode.cs:line 245
   at ICSharpCode.Decompiler.FlowAnalysis.ControlFlowGraph.ComputeDominanceFrontier() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\FlowAnalysis\ControlFlowGraph.cs:line 157
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 55
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00022AF1 File Offset: 0x00021AF1
		public XmlQueryType CheckLiteralQName(QilName node)
		{
			return XmlQueryTypeFactory.QNameX;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00022AF8 File Offset: 0x00021AF8
		public XmlQueryType CheckLiteralType(QilLiteral node)
		{
			return node;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00022B00 File Offset: 0x00021B00
		public XmlQueryType CheckLiteralObject(QilLiteral node)
		{
			return XmlQueryTypeFactory.ItemS;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00022B07 File Offset: 0x00021B07
		public XmlQueryType CheckAnd(QilBinary node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00022B0E File Offset: 0x00021B0E
		public XmlQueryType CheckOr(QilBinary node)
		{
			return this.CheckAnd(node);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00022B17 File Offset: 0x00021B17
		public XmlQueryType CheckNot(QilUnary node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00022B1E File Offset: 0x00021B1E
		public XmlQueryType CheckConditional(QilTernary node)
		{
			return XmlQueryTypeFactory.Choice(node.Center.XmlType, node.Right.XmlType);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00022B3B File Offset: 0x00021B3B
		public XmlQueryType CheckChoice(QilChoice node)
		{
			return node.Branches.XmlType;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00022B48 File Offset: 0x00021B48
		public XmlQueryType CheckLength(QilUnary node)
		{
			return XmlQueryTypeFactory.IntX;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00022B4F File Offset: 0x00021B4F
		public XmlQueryType CheckSequence(QilList node)
		{
			return node.XmlType;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00022B57 File Offset: 0x00021B57
		public XmlQueryType CheckUnion(QilBinary node)
		{
			return this.DistinctType(XmlQueryTypeFactory.Sequence(node.Left.XmlType, node.Right.XmlType));
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00022B7A File Offset: 0x00021B7A
		public XmlQueryType CheckIntersection(QilBinary node)
		{
			return this.CheckUnion(node);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00022B83 File Offset: 0x00021B83
		public XmlQueryType CheckDifference(QilBinary node)
		{
			return XmlQueryTypeFactory.AtMost(node.Left.XmlType, node.Left.XmlType.Cardinality);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00022BA8 File Offset: 0x00021BA8
		public XmlQueryType CheckAverage(QilUnary node)
		{
			XmlQueryType xmlType = node.Child.XmlType;
			return XmlQueryTypeFactory.PrimeProduct(xmlType, xmlType.MaybeEmpty ? XmlQueryCardinality.ZeroOrOne : XmlQueryCardinality.One);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00022BDB File Offset: 0x00021BDB
		public XmlQueryType CheckSum(QilUnary node)
		{
			return this.CheckAverage(node);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00022BE4 File Offset: 0x00021BE4
		public XmlQueryType CheckMinimum(QilUnary node)
		{
			return this.CheckAverage(node);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00022BED File Offset: 0x00021BED
		public XmlQueryType CheckMaximum(QilUnary node)
		{
			return this.CheckAverage(node);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00022BF6 File Offset: 0x00021BF6
		public XmlQueryType CheckNegate(QilUnary node)
		{
			return node.Child.XmlType;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00022C03 File Offset: 0x00021C03
		public XmlQueryType CheckAdd(QilBinary node)
		{
			if (node.Left.XmlType.TypeCode != XmlTypeCode.None)
			{
				return node.Left.XmlType;
			}
			return node.Right.XmlType;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00022C2E File Offset: 0x00021C2E
		public XmlQueryType CheckSubtract(QilBinary node)
		{
			return this.CheckAdd(node);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00022C37 File Offset: 0x00021C37
		public XmlQueryType CheckMultiply(QilBinary node)
		{
			return this.CheckAdd(node);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00022C40 File Offset: 0x00021C40
		public XmlQueryType CheckDivide(QilBinary node)
		{
			return this.CheckAdd(node);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00022C49 File Offset: 0x00021C49
		public XmlQueryType CheckModulo(QilBinary node)
		{
			return this.CheckAdd(node);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00022C52 File Offset: 0x00021C52
		public XmlQueryType CheckStrLength(QilUnary node)
		{
			return XmlQueryTypeFactory.IntX;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00022C59 File Offset: 0x00021C59
		public XmlQueryType CheckStrConcat(QilStrConcat node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00022C60 File Offset: 0x00021C60
		public XmlQueryType CheckStrParseQName(QilBinary node)
		{
			return XmlQueryTypeFactory.QNameX;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00022C67 File Offset: 0x00021C67
		public XmlQueryType CheckNe(QilBinary node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00022C6E File Offset: 0x00021C6E
		public XmlQueryType CheckEq(QilBinary node)
		{
			return this.CheckNe(node);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00022C77 File Offset: 0x00021C77
		public XmlQueryType CheckGt(QilBinary node)
		{
			return this.CheckNe(node);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00022C80 File Offset: 0x00021C80
		public XmlQueryType CheckGe(QilBinary node)
		{
			return this.CheckNe(node);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00022C89 File Offset: 0x00021C89
		public XmlQueryType CheckLt(QilBinary node)
		{
			return this.CheckNe(node);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00022C92 File Offset: 0x00021C92
		public XmlQueryType CheckLe(QilBinary node)
		{
			return this.CheckNe(node);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00022C9B File Offset: 0x00021C9B
		public XmlQueryType CheckIs(QilBinary node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00022CA2 File Offset: 0x00021CA2
		public XmlQueryType CheckAfter(QilBinary node)
		{
			return this.CheckIs(node);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00022CAB File Offset: 0x00021CAB
		public XmlQueryType CheckBefore(QilBinary node)
		{
			return this.CheckIs(node);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00022CB4 File Offset: 0x00021CB4
		public XmlQueryType CheckLoop(QilLoop node)
		{
			XmlQueryType xmlType = node.Body.XmlType;
			XmlQueryCardinality xmlQueryCardinality = ((node.Variable.NodeType == QilNodeType.Let) ? XmlQueryCardinality.One : node.Variable.Binding.XmlType.Cardinality);
			if (xmlType.IsDod)
			{
				return XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtfS, xmlQueryCardinality * xmlType.Cardinality);
			}
			return XmlQueryTypeFactory.PrimeProduct(xmlType, xmlQueryCardinality * xmlType.Cardinality);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00022D2C File Offset: 0x00021D2C
		public XmlQueryType CheckFilter(QilLoop node)
		{
			XmlQueryType xmlQueryType = this.FindFilterType(node.Variable, node.Body);
			if (xmlQueryType != null)
			{
				return xmlQueryType;
			}
			return XmlQueryTypeFactory.AtMost(node.Variable.Binding.XmlType, node.Variable.Binding.XmlType.Cardinality);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00022D84 File Offset: 0x00021D84
		public XmlQueryType CheckSort(QilLoop node)
		{
			XmlQueryType xmlType = node.Variable.Binding.XmlType;
			if (xmlType.IsDod)
			{
				return XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtfS, xmlType.Cardinality);
			}
			return node.Variable.Binding.XmlType;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00022DCB File Offset: 0x00021DCB
		public XmlQueryType CheckSortKey(QilSortKey node)
		{
			return node.Key.XmlType;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00022DD8 File Offset: 0x00021DD8
		public XmlQueryType CheckDocOrderDistinct(QilUnary node)
		{
			return this.DistinctType(node.Child.XmlType);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00022DEB File Offset: 0x00021DEB
		public XmlQueryType CheckFunction(QilFunction node)
		{
			return node.XmlType;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00022DF3 File Offset: 0x00021DF3
		public XmlQueryType CheckInvoke(QilInvoke node)
		{
			return node.Function.XmlType;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00022E00 File Offset: 0x00021E00
		public XmlQueryType CheckContent(QilUnary node)
		{
			return XmlQueryTypeFactory.AttributeOrContentS;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00022E07 File Offset: 0x00021E07
		public XmlQueryType CheckAttribute(QilBinary node)
		{
			return XmlQueryTypeFactory.AttributeQ;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00022E0E File Offset: 0x00021E0E
		public XmlQueryType CheckParent(QilUnary node)
		{
			return XmlQueryTypeFactory.DocumentOrElementQ;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022E15 File Offset: 0x00021E15
		public XmlQueryType CheckRoot(QilUnary node)
		{
			return XmlQueryTypeFactory.NodeNotRtf;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00022E1C File Offset: 0x00021E1C
		public XmlQueryType CheckXmlContext(QilNode node)
		{
			return XmlQueryTypeFactory.NodeNotRtf;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00022E23 File Offset: 0x00021E23
		public XmlQueryType CheckDescendant(QilUnary node)
		{
			return XmlQueryTypeFactory.ContentS;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00022E2A File Offset: 0x00021E2A
		public XmlQueryType CheckDescendantOrSelf(QilUnary node)
		{
			return XmlQueryTypeFactory.Choice(node.Child.XmlType, XmlQueryTypeFactory.ContentS);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00022E41 File Offset: 0x00021E41
		public XmlQueryType CheckAncestor(QilUnary node)
		{
			return XmlQueryTypeFactory.DocumentOrElementS;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00022E48 File Offset: 0x00021E48
		public XmlQueryType CheckAncestorOrSelf(QilUnary node)
		{
			return XmlQueryTypeFactory.Choice(node.Child.XmlType, XmlQueryTypeFactory.DocumentOrElementS);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00022E5F File Offset: 0x00021E5F
		public XmlQueryType CheckPreceding(QilUnary node)
		{
			return XmlQueryTypeFactory.DocumentOrContentS;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00022E66 File Offset: 0x00021E66
		public XmlQueryType CheckFollowingSibling(QilUnary node)
		{
			return XmlQueryTypeFactory.ContentS;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00022E6D File Offset: 0x00021E6D
		public XmlQueryType CheckPrecedingSibling(QilUnary node)
		{
			return XmlQueryTypeFactory.ContentS;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00022E74 File Offset: 0x00021E74
		public XmlQueryType CheckNodeRange(QilBinary node)
		{
			return XmlQueryTypeFactory.Choice(new XmlQueryType[]
			{
				node.Left.XmlType,
				XmlQueryTypeFactory.ContentS,
				node.Right.XmlType
			});
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00022EB2 File Offset: 0x00021EB2
		public XmlQueryType CheckDeref(QilBinary node)
		{
			return XmlQueryTypeFactory.ElementS;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00022EB9 File Offset: 0x00021EB9
		public XmlQueryType CheckElementCtor(QilBinary node)
		{
			return XmlQueryTypeFactory.UntypedElement;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00022EC0 File Offset: 0x00021EC0
		public XmlQueryType CheckAttributeCtor(QilBinary node)
		{
			return XmlQueryTypeFactory.UntypedAttribute;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022EC7 File Offset: 0x00021EC7
		public XmlQueryType CheckCommentCtor(QilUnary node)
		{
			return XmlQueryTypeFactory.Comment;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00022ECE File Offset: 0x00021ECE
		public XmlQueryType CheckPICtor(QilBinary node)
		{
			return XmlQueryTypeFactory.PI;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00022ED5 File Offset: 0x00021ED5
		public XmlQueryType CheckTextCtor(QilUnary node)
		{
			return XmlQueryTypeFactory.Text;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00022EDC File Offset: 0x00021EDC
		public XmlQueryType CheckRawTextCtor(QilUnary node)
		{
			return XmlQueryTypeFactory.Text;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00022EE3 File Offset: 0x00021EE3
		public XmlQueryType CheckDocumentCtor(QilUnary node)
		{
			return XmlQueryTypeFactory.UntypedDocument;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00022EEA File Offset: 0x00021EEA
		public XmlQueryType CheckNamespaceDecl(QilBinary node)
		{
			return XmlQueryTypeFactory.Namespace;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00022EF1 File Offset: 0x00021EF1
		public XmlQueryType CheckRtfCtor(QilBinary node)
		{
			return XmlQueryTypeFactory.Node;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00022EF8 File Offset: 0x00021EF8
		public XmlQueryType CheckNameOf(QilUnary node)
		{
			return XmlQueryTypeFactory.QNameX;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00022EFF File Offset: 0x00021EFF
		public XmlQueryType CheckLocalNameOf(QilUnary node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00022F06 File Offset: 0x00021F06
		public XmlQueryType CheckNamespaceUriOf(QilUnary node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00022F0D File Offset: 0x00021F0D
		public XmlQueryType CheckPrefixOf(QilUnary node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00022F14 File Offset: 0x00021F14
		public XmlQueryType CheckDeepCopy(QilUnary node)
		{
			return node.XmlType;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00022F1C File Offset: 0x00021F1C
		public XmlQueryType CheckTypeAssert(QilTargetType node)
		{
			return node.TargetType;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00022F24 File Offset: 0x00021F24
		public XmlQueryType CheckIsType(QilTargetType node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00022F2B File Offset: 0x00021F2B
		public XmlQueryType CheckIsEmpty(QilUnary node)
		{
			return XmlQueryTypeFactory.BooleanX;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00022F32 File Offset: 0x00021F32
		public XmlQueryType CheckXPathNodeValue(QilUnary node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00022F39 File Offset: 0x00021F39
		public XmlQueryType CheckXPathFollowing(QilUnary node)
		{
			return XmlQueryTypeFactory.ContentS;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00022F40 File Offset: 0x00021F40
		public XmlQueryType CheckXPathPreceding(QilUnary node)
		{
			return XmlQueryTypeFactory.ContentS;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00022F47 File Offset: 0x00021F47
		public XmlQueryType CheckXPathNamespace(QilUnary node)
		{
			return XmlQueryTypeFactory.NamespaceS;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00022F4E File Offset: 0x00021F4E
		public XmlQueryType CheckXsltGenerateId(QilUnary node)
		{
			return XmlQueryTypeFactory.StringX;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00022F55 File Offset: 0x00021F55
		public XmlQueryType CheckXsltInvokeLateBound(QilInvokeLateBound node)
		{
			return XmlQueryTypeFactory.ItemS;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00022F5C File Offset: 0x00021F5C
		public XmlQueryType CheckXsltInvokeEarlyBound(QilInvokeEarlyBound node)
		{
			return node.XmlType;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00022F64 File Offset: 0x00021F64
		public XmlQueryType CheckXsltCopy(QilBinary node)
		{
			return XmlQueryTypeFactory.Choice(node.Left.XmlType, node.Right.XmlType);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00022F81 File Offset: 0x00021F81
		public XmlQueryType CheckXsltCopyOf(QilUnary node)
		{
			if ((node.Child.XmlType.NodeKinds & XmlNodeKindFlags.Document) != XmlNodeKindFlags.None)
			{
				return XmlQueryTypeFactory.NodeNotRtfS;
			}
			return node.Child.XmlType;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00022FA8 File Offset: 0x00021FA8
		public XmlQueryType CheckXsltConvert(QilTargetType node)
		{
			return node.TargetType;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00022FB0 File Offset: 0x00021FB0
		[Conditional("DEBUG")]
		private void Check(bool value, QilNode node, string message)
		{
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00022FB4 File Offset: 0x00021FB4
		[Conditional("DEBUG")]
		private void CheckLiteralValue(QilNode node, Type clrTypeValue)
		{
			((QilLiteral)node).Value.GetType();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00022FC7 File Offset: 0x00021FC7
		[Conditional("DEBUG")]
		private void CheckClass(QilNode node, Type clrTypeClass)
		{
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00022FC9 File Offset: 0x00021FC9
		[Conditional("DEBUG")]
		private void CheckClassAndNodeType(QilNode node, Type clrTypeClass, QilNodeType nodeType)
		{
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00022FCB File Offset: 0x00021FCB
		[Conditional("DEBUG")]
		private void CheckXmlType(QilNode node, XmlQueryType xmlType)
		{
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00022FCD File Offset: 0x00021FCD
		[Conditional("DEBUG")]
		private void CheckNumericX(QilNode node)
		{
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00022FCF File Offset: 0x00021FCF
		[Conditional("DEBUG")]
		private void CheckNumericXS(QilNode node)
		{
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00022FD1 File Offset: 0x00021FD1
		[Conditional("DEBUG")]
		private void CheckAtomicX(QilNode node)
		{
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00022FD3 File Offset: 0x00021FD3
		[Conditional("DEBUG")]
		private void CheckNotDisjoint(QilBinary node)
		{
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00022FD5 File Offset: 0x00021FD5
		private XmlQueryType DistinctType(XmlQueryType type)
		{
			if (type.Cardinality == XmlQueryCardinality.More)
			{
				return XmlQueryTypeFactory.PrimeProduct(type, XmlQueryCardinality.OneOrMore);
			}
			if (type.Cardinality == XmlQueryCardinality.NotOne)
			{
				return XmlQueryTypeFactory.PrimeProduct(type, XmlQueryCardinality.ZeroOrMore);
			}
			return type;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00023014 File Offset: 0x00022014
		private XmlQueryType FindFilterType(QilIterator variable, QilNode body)
		{
			if (body.XmlType.TypeCode == XmlTypeCode.None)
			{
				return XmlQueryTypeFactory.None;
			}
			QilNodeType nodeType = body.NodeType;
			if (nodeType <= QilNodeType.And)
			{
				if (nodeType == QilNodeType.False)
				{
					return XmlQueryTypeFactory.Empty;
				}
				if (nodeType == QilNodeType.And)
				{
					XmlQueryType xmlQueryType = this.FindFilterType(variable, ((QilBinary)body).Left);
					if (xmlQueryType != null)
					{
						return xmlQueryType;
					}
					return this.FindFilterType(variable, ((QilBinary)body).Right);
				}
			}
			else if (nodeType != QilNodeType.Eq)
			{
				if (nodeType == QilNodeType.IsType)
				{
					if (object.Equals(((QilTargetType)body).Source, variable))
					{
						return XmlQueryTypeFactory.AtMost(((QilTargetType)body).TargetType, variable.Binding.XmlType.Cardinality);
					}
				}
			}
			else
			{
				QilBinary qilBinary = (QilBinary)body;
				if (qilBinary.Left.NodeType == QilNodeType.PositionOf && object.Equals(((QilUnary)qilBinary.Left).Child, variable))
				{
					return XmlQueryTypeFactory.AtMost(variable.Binding.XmlType, XmlQueryCardinality.ZeroOrOne);
				}
			}
			return null;
		}
	}
}
