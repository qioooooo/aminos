using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200002E RID: 46
	internal abstract class QilVisitor
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000DC3B File Offset: 0x0000CC3B
		protected virtual QilNode VisitAssumeReference(QilNode expr)
		{
			if (expr is QilReference)
			{
				return this.VisitReference(expr);
			}
			return this.Visit(expr);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000DC54 File Offset: 0x0000CC54
		protected virtual QilNode VisitChildren(QilNode parent)
		{
			for (int i = 0; i < parent.Count; i++)
			{
				if (this.IsReference(parent, i))
				{
					this.VisitReference(parent[i]);
				}
				else
				{
					this.Visit(parent[i]);
				}
			}
			return parent;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000DC9C File Offset: 0x0000CC9C
		protected virtual bool IsReference(QilNode parent, int childNum)
		{
			QilNode qilNode = parent[childNum];
			if (qilNode != null)
			{
				QilNodeType nodeType = qilNode.NodeType;
				switch (nodeType)
				{
				case QilNodeType.For:
				case QilNodeType.Let:
				case QilNodeType.Parameter:
				{
					QilNodeType nodeType2 = parent.NodeType;
					switch (nodeType2)
					{
					case QilNodeType.GlobalVariableList:
					case QilNodeType.GlobalParameterList:
					case QilNodeType.FormalParameterList:
						return false;
					case QilNodeType.ActualParameterList:
						break;
					default:
						switch (nodeType2)
						{
						case QilNodeType.Loop:
						case QilNodeType.Filter:
						case QilNodeType.Sort:
							return childNum == 1;
						}
						break;
					}
					return true;
				}
				default:
					if (nodeType == QilNodeType.Function)
					{
						return parent.NodeType == QilNodeType.Invoke;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000DD24 File Offset: 0x0000CD24
		protected virtual QilNode Visit(QilNode n)
		{
			if (n == null)
			{
				return this.VisitNull();
			}
			switch (n.NodeType)
			{
			case QilNodeType.QilExpression:
				return this.VisitQilExpression((QilExpression)n);
			case QilNodeType.FunctionList:
				return this.VisitFunctionList((QilList)n);
			case QilNodeType.GlobalVariableList:
				return this.VisitGlobalVariableList((QilList)n);
			case QilNodeType.GlobalParameterList:
				return this.VisitGlobalParameterList((QilList)n);
			case QilNodeType.ActualParameterList:
				return this.VisitActualParameterList((QilList)n);
			case QilNodeType.FormalParameterList:
				return this.VisitFormalParameterList((QilList)n);
			case QilNodeType.SortKeyList:
				return this.VisitSortKeyList((QilList)n);
			case QilNodeType.BranchList:
				return this.VisitBranchList((QilList)n);
			case QilNodeType.OptimizeBarrier:
				return this.VisitOptimizeBarrier((QilUnary)n);
			case QilNodeType.Unknown:
				return this.VisitUnknown(n);
			case QilNodeType.DataSource:
				return this.VisitDataSource((QilDataSource)n);
			case QilNodeType.Nop:
				return this.VisitNop((QilUnary)n);
			case QilNodeType.Error:
				return this.VisitError((QilUnary)n);
			case QilNodeType.Warning:
				return this.VisitWarning((QilUnary)n);
			case QilNodeType.For:
				return this.VisitFor((QilIterator)n);
			case QilNodeType.Let:
				return this.VisitLet((QilIterator)n);
			case QilNodeType.Parameter:
				return this.VisitParameter((QilParameter)n);
			case QilNodeType.PositionOf:
				return this.VisitPositionOf((QilUnary)n);
			case QilNodeType.True:
				return this.VisitTrue(n);
			case QilNodeType.False:
				return this.VisitFalse(n);
			case QilNodeType.LiteralString:
				return this.VisitLiteralString((QilLiteral)n);
			case QilNodeType.LiteralInt32:
				return this.VisitLiteralInt32((QilLiteral)n);
			case QilNodeType.LiteralInt64:
				return this.VisitLiteralInt64((QilLiteral)n);
			case QilNodeType.LiteralDouble:
				return this.VisitLiteralDouble((QilLiteral)n);
			case QilNodeType.LiteralDecimal:
				return this.VisitLiteralDecimal((QilLiteral)n);
			case QilNodeType.LiteralQName:
				return this.VisitLiteralQName((QilName)n);
			case QilNodeType.LiteralType:
				return this.VisitLiteralType((QilLiteral)n);
			case QilNodeType.LiteralObject:
				return this.VisitLiteralObject((QilLiteral)n);
			case QilNodeType.And:
				return this.VisitAnd((QilBinary)n);
			case QilNodeType.Or:
				return this.VisitOr((QilBinary)n);
			case QilNodeType.Not:
				return this.VisitNot((QilUnary)n);
			case QilNodeType.Conditional:
				return this.VisitConditional((QilTernary)n);
			case QilNodeType.Choice:
				return this.VisitChoice((QilChoice)n);
			case QilNodeType.Length:
				return this.VisitLength((QilUnary)n);
			case QilNodeType.Sequence:
				return this.VisitSequence((QilList)n);
			case QilNodeType.Union:
				return this.VisitUnion((QilBinary)n);
			case QilNodeType.Intersection:
				return this.VisitIntersection((QilBinary)n);
			case QilNodeType.Difference:
				return this.VisitDifference((QilBinary)n);
			case QilNodeType.Average:
				return this.VisitAverage((QilUnary)n);
			case QilNodeType.Sum:
				return this.VisitSum((QilUnary)n);
			case QilNodeType.Minimum:
				return this.VisitMinimum((QilUnary)n);
			case QilNodeType.Maximum:
				return this.VisitMaximum((QilUnary)n);
			case QilNodeType.Negate:
				return this.VisitNegate((QilUnary)n);
			case QilNodeType.Add:
				return this.VisitAdd((QilBinary)n);
			case QilNodeType.Subtract:
				return this.VisitSubtract((QilBinary)n);
			case QilNodeType.Multiply:
				return this.VisitMultiply((QilBinary)n);
			case QilNodeType.Divide:
				return this.VisitDivide((QilBinary)n);
			case QilNodeType.Modulo:
				return this.VisitModulo((QilBinary)n);
			case QilNodeType.StrLength:
				return this.VisitStrLength((QilUnary)n);
			case QilNodeType.StrConcat:
				return this.VisitStrConcat((QilStrConcat)n);
			case QilNodeType.StrParseQName:
				return this.VisitStrParseQName((QilBinary)n);
			case QilNodeType.Ne:
				return this.VisitNe((QilBinary)n);
			case QilNodeType.Eq:
				return this.VisitEq((QilBinary)n);
			case QilNodeType.Gt:
				return this.VisitGt((QilBinary)n);
			case QilNodeType.Ge:
				return this.VisitGe((QilBinary)n);
			case QilNodeType.Lt:
				return this.VisitLt((QilBinary)n);
			case QilNodeType.Le:
				return this.VisitLe((QilBinary)n);
			case QilNodeType.Is:
				return this.VisitIs((QilBinary)n);
			case QilNodeType.After:
				return this.VisitAfter((QilBinary)n);
			case QilNodeType.Before:
				return this.VisitBefore((QilBinary)n);
			case QilNodeType.Loop:
				return this.VisitLoop((QilLoop)n);
			case QilNodeType.Filter:
				return this.VisitFilter((QilLoop)n);
			case QilNodeType.Sort:
				return this.VisitSort((QilLoop)n);
			case QilNodeType.SortKey:
				return this.VisitSortKey((QilSortKey)n);
			case QilNodeType.DocOrderDistinct:
				return this.VisitDocOrderDistinct((QilUnary)n);
			case QilNodeType.Function:
				return this.VisitFunction((QilFunction)n);
			case QilNodeType.Invoke:
				return this.VisitInvoke((QilInvoke)n);
			case QilNodeType.Content:
				return this.VisitContent((QilUnary)n);
			case QilNodeType.Attribute:
				return this.VisitAttribute((QilBinary)n);
			case QilNodeType.Parent:
				return this.VisitParent((QilUnary)n);
			case QilNodeType.Root:
				return this.VisitRoot((QilUnary)n);
			case QilNodeType.XmlContext:
				return this.VisitXmlContext(n);
			case QilNodeType.Descendant:
				return this.VisitDescendant((QilUnary)n);
			case QilNodeType.DescendantOrSelf:
				return this.VisitDescendantOrSelf((QilUnary)n);
			case QilNodeType.Ancestor:
				return this.VisitAncestor((QilUnary)n);
			case QilNodeType.AncestorOrSelf:
				return this.VisitAncestorOrSelf((QilUnary)n);
			case QilNodeType.Preceding:
				return this.VisitPreceding((QilUnary)n);
			case QilNodeType.FollowingSibling:
				return this.VisitFollowingSibling((QilUnary)n);
			case QilNodeType.PrecedingSibling:
				return this.VisitPrecedingSibling((QilUnary)n);
			case QilNodeType.NodeRange:
				return this.VisitNodeRange((QilBinary)n);
			case QilNodeType.Deref:
				return this.VisitDeref((QilBinary)n);
			case QilNodeType.ElementCtor:
				return this.VisitElementCtor((QilBinary)n);
			case QilNodeType.AttributeCtor:
				return this.VisitAttributeCtor((QilBinary)n);
			case QilNodeType.CommentCtor:
				return this.VisitCommentCtor((QilUnary)n);
			case QilNodeType.PICtor:
				return this.VisitPICtor((QilBinary)n);
			case QilNodeType.TextCtor:
				return this.VisitTextCtor((QilUnary)n);
			case QilNodeType.RawTextCtor:
				return this.VisitRawTextCtor((QilUnary)n);
			case QilNodeType.DocumentCtor:
				return this.VisitDocumentCtor((QilUnary)n);
			case QilNodeType.NamespaceDecl:
				return this.VisitNamespaceDecl((QilBinary)n);
			case QilNodeType.RtfCtor:
				return this.VisitRtfCtor((QilBinary)n);
			case QilNodeType.NameOf:
				return this.VisitNameOf((QilUnary)n);
			case QilNodeType.LocalNameOf:
				return this.VisitLocalNameOf((QilUnary)n);
			case QilNodeType.NamespaceUriOf:
				return this.VisitNamespaceUriOf((QilUnary)n);
			case QilNodeType.PrefixOf:
				return this.VisitPrefixOf((QilUnary)n);
			case QilNodeType.TypeAssert:
				return this.VisitTypeAssert((QilTargetType)n);
			case QilNodeType.IsType:
				return this.VisitIsType((QilTargetType)n);
			case QilNodeType.IsEmpty:
				return this.VisitIsEmpty((QilUnary)n);
			case QilNodeType.XPathNodeValue:
				return this.VisitXPathNodeValue((QilUnary)n);
			case QilNodeType.XPathFollowing:
				return this.VisitXPathFollowing((QilUnary)n);
			case QilNodeType.XPathPreceding:
				return this.VisitXPathPreceding((QilUnary)n);
			case QilNodeType.XPathNamespace:
				return this.VisitXPathNamespace((QilUnary)n);
			case QilNodeType.XsltGenerateId:
				return this.VisitXsltGenerateId((QilUnary)n);
			case QilNodeType.XsltInvokeLateBound:
				return this.VisitXsltInvokeLateBound((QilInvokeLateBound)n);
			case QilNodeType.XsltInvokeEarlyBound:
				return this.VisitXsltInvokeEarlyBound((QilInvokeEarlyBound)n);
			case QilNodeType.XsltCopy:
				return this.VisitXsltCopy((QilBinary)n);
			case QilNodeType.XsltCopyOf:
				return this.VisitXsltCopyOf((QilUnary)n);
			case QilNodeType.XsltConvert:
				return this.VisitXsltConvert((QilTargetType)n);
			default:
				return this.VisitUnknown(n);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000E45C File Offset: 0x0000D45C
		protected virtual QilNode VisitReference(QilNode n)
		{
			if (n == null)
			{
				return this.VisitNull();
			}
			QilNodeType nodeType = n.NodeType;
			switch (nodeType)
			{
			case QilNodeType.For:
				return this.VisitForReference((QilIterator)n);
			case QilNodeType.Let:
				return this.VisitLetReference((QilIterator)n);
			case QilNodeType.Parameter:
				return this.VisitParameterReference((QilParameter)n);
			default:
				if (nodeType != QilNodeType.Function)
				{
					return this.VisitUnknown(n);
				}
				return this.VisitFunctionReference((QilFunction)n);
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000E4D1 File Offset: 0x0000D4D1
		protected virtual QilNode VisitNull()
		{
			return null;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000E4D4 File Offset: 0x0000D4D4
		protected virtual QilNode VisitQilExpression(QilExpression n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000E4DD File Offset: 0x0000D4DD
		protected virtual QilNode VisitFunctionList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000E4E6 File Offset: 0x0000D4E6
		protected virtual QilNode VisitGlobalVariableList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000E4EF File Offset: 0x0000D4EF
		protected virtual QilNode VisitGlobalParameterList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E4F8 File Offset: 0x0000D4F8
		protected virtual QilNode VisitActualParameterList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000E501 File Offset: 0x0000D501
		protected virtual QilNode VisitFormalParameterList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E50A File Offset: 0x0000D50A
		protected virtual QilNode VisitSortKeyList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E513 File Offset: 0x0000D513
		protected virtual QilNode VisitBranchList(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000E51C File Offset: 0x0000D51C
		protected virtual QilNode VisitOptimizeBarrier(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E525 File Offset: 0x0000D525
		protected virtual QilNode VisitUnknown(QilNode n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E52E File Offset: 0x0000D52E
		protected virtual QilNode VisitDataSource(QilDataSource n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000E537 File Offset: 0x0000D537
		protected virtual QilNode VisitNop(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000E540 File Offset: 0x0000D540
		protected virtual QilNode VisitError(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000E549 File Offset: 0x0000D549
		protected virtual QilNode VisitWarning(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000E552 File Offset: 0x0000D552
		protected virtual QilNode VisitFor(QilIterator n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000E55B File Offset: 0x0000D55B
		protected virtual QilNode VisitForReference(QilIterator n)
		{
			return n;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000E55E File Offset: 0x0000D55E
		protected virtual QilNode VisitLet(QilIterator n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000E567 File Offset: 0x0000D567
		protected virtual QilNode VisitLetReference(QilIterator n)
		{
			return n;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E56A File Offset: 0x0000D56A
		protected virtual QilNode VisitParameter(QilParameter n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000E573 File Offset: 0x0000D573
		protected virtual QilNode VisitParameterReference(QilParameter n)
		{
			return n;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000E576 File Offset: 0x0000D576
		protected virtual QilNode VisitPositionOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000E57F File Offset: 0x0000D57F
		protected virtual QilNode VisitTrue(QilNode n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000E588 File Offset: 0x0000D588
		protected virtual QilNode VisitFalse(QilNode n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000E591 File Offset: 0x0000D591
		protected virtual QilNode VisitLiteralString(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000E59A File Offset: 0x0000D59A
		protected virtual QilNode VisitLiteralInt32(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000E5A3 File Offset: 0x0000D5A3
		protected virtual QilNode VisitLiteralInt64(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000E5AC File Offset: 0x0000D5AC
		protected virtual QilNode VisitLiteralDouble(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000E5B5 File Offset: 0x0000D5B5
		protected virtual QilNode VisitLiteralDecimal(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000E5BE File Offset: 0x0000D5BE
		protected virtual QilNode VisitLiteralQName(QilName n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000E5C7 File Offset: 0x0000D5C7
		protected virtual QilNode VisitLiteralType(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000E5D0 File Offset: 0x0000D5D0
		protected virtual QilNode VisitLiteralObject(QilLiteral n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000E5D9 File Offset: 0x0000D5D9
		protected virtual QilNode VisitAnd(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000E5E2 File Offset: 0x0000D5E2
		protected virtual QilNode VisitOr(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000E5EB File Offset: 0x0000D5EB
		protected virtual QilNode VisitNot(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000E5F4 File Offset: 0x0000D5F4
		protected virtual QilNode VisitConditional(QilTernary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000E5FD File Offset: 0x0000D5FD
		protected virtual QilNode VisitChoice(QilChoice n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000E606 File Offset: 0x0000D606
		protected virtual QilNode VisitLength(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000E60F File Offset: 0x0000D60F
		protected virtual QilNode VisitSequence(QilList n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000E618 File Offset: 0x0000D618
		protected virtual QilNode VisitUnion(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000E621 File Offset: 0x0000D621
		protected virtual QilNode VisitIntersection(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000E62A File Offset: 0x0000D62A
		protected virtual QilNode VisitDifference(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000E633 File Offset: 0x0000D633
		protected virtual QilNode VisitAverage(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000E63C File Offset: 0x0000D63C
		protected virtual QilNode VisitSum(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000E645 File Offset: 0x0000D645
		protected virtual QilNode VisitMinimum(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000E64E File Offset: 0x0000D64E
		protected virtual QilNode VisitMaximum(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000E657 File Offset: 0x0000D657
		protected virtual QilNode VisitNegate(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000E660 File Offset: 0x0000D660
		protected virtual QilNode VisitAdd(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000E669 File Offset: 0x0000D669
		protected virtual QilNode VisitSubtract(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E672 File Offset: 0x0000D672
		protected virtual QilNode VisitMultiply(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E67B File Offset: 0x0000D67B
		protected virtual QilNode VisitDivide(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000E684 File Offset: 0x0000D684
		protected virtual QilNode VisitModulo(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E68D File Offset: 0x0000D68D
		protected virtual QilNode VisitStrLength(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000E696 File Offset: 0x0000D696
		protected virtual QilNode VisitStrConcat(QilStrConcat n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E69F File Offset: 0x0000D69F
		protected virtual QilNode VisitStrParseQName(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000E6A8 File Offset: 0x0000D6A8
		protected virtual QilNode VisitNe(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000E6B1 File Offset: 0x0000D6B1
		protected virtual QilNode VisitEq(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E6BA File Offset: 0x0000D6BA
		protected virtual QilNode VisitGt(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E6C3 File Offset: 0x0000D6C3
		protected virtual QilNode VisitGe(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000E6CC File Offset: 0x0000D6CC
		protected virtual QilNode VisitLt(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E6D5 File Offset: 0x0000D6D5
		protected virtual QilNode VisitLe(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E6DE File Offset: 0x0000D6DE
		protected virtual QilNode VisitIs(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E6E7 File Offset: 0x0000D6E7
		protected virtual QilNode VisitAfter(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000E6F0 File Offset: 0x0000D6F0
		protected virtual QilNode VisitBefore(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E6F9 File Offset: 0x0000D6F9
		protected virtual QilNode VisitLoop(QilLoop n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000E702 File Offset: 0x0000D702
		protected virtual QilNode VisitFilter(QilLoop n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E70B File Offset: 0x0000D70B
		protected virtual QilNode VisitSort(QilLoop n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E714 File Offset: 0x0000D714
		protected virtual QilNode VisitSortKey(QilSortKey n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E71D File Offset: 0x0000D71D
		protected virtual QilNode VisitDocOrderDistinct(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E726 File Offset: 0x0000D726
		protected virtual QilNode VisitFunction(QilFunction n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E72F File Offset: 0x0000D72F
		protected virtual QilNode VisitFunctionReference(QilFunction n)
		{
			return n;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E732 File Offset: 0x0000D732
		protected virtual QilNode VisitInvoke(QilInvoke n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E73B File Offset: 0x0000D73B
		protected virtual QilNode VisitContent(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E744 File Offset: 0x0000D744
		protected virtual QilNode VisitAttribute(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000E74D File Offset: 0x0000D74D
		protected virtual QilNode VisitParent(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000E756 File Offset: 0x0000D756
		protected virtual QilNode VisitRoot(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E75F File Offset: 0x0000D75F
		protected virtual QilNode VisitXmlContext(QilNode n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E768 File Offset: 0x0000D768
		protected virtual QilNode VisitDescendant(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000E771 File Offset: 0x0000D771
		protected virtual QilNode VisitDescendantOrSelf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000E77A File Offset: 0x0000D77A
		protected virtual QilNode VisitAncestor(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000E783 File Offset: 0x0000D783
		protected virtual QilNode VisitAncestorOrSelf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E78C File Offset: 0x0000D78C
		protected virtual QilNode VisitPreceding(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000E795 File Offset: 0x0000D795
		protected virtual QilNode VisitFollowingSibling(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000E79E File Offset: 0x0000D79E
		protected virtual QilNode VisitPrecedingSibling(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000E7A7 File Offset: 0x0000D7A7
		protected virtual QilNode VisitNodeRange(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E7B0 File Offset: 0x0000D7B0
		protected virtual QilNode VisitDeref(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E7B9 File Offset: 0x0000D7B9
		protected virtual QilNode VisitElementCtor(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E7C2 File Offset: 0x0000D7C2
		protected virtual QilNode VisitAttributeCtor(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E7CB File Offset: 0x0000D7CB
		protected virtual QilNode VisitCommentCtor(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000E7D4 File Offset: 0x0000D7D4
		protected virtual QilNode VisitPICtor(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E7DD File Offset: 0x0000D7DD
		protected virtual QilNode VisitTextCtor(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E7E6 File Offset: 0x0000D7E6
		protected virtual QilNode VisitRawTextCtor(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000E7EF File Offset: 0x0000D7EF
		protected virtual QilNode VisitDocumentCtor(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E7F8 File Offset: 0x0000D7F8
		protected virtual QilNode VisitNamespaceDecl(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E801 File Offset: 0x0000D801
		protected virtual QilNode VisitRtfCtor(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E80A File Offset: 0x0000D80A
		protected virtual QilNode VisitNameOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E813 File Offset: 0x0000D813
		protected virtual QilNode VisitLocalNameOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E81C File Offset: 0x0000D81C
		protected virtual QilNode VisitNamespaceUriOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000E825 File Offset: 0x0000D825
		protected virtual QilNode VisitPrefixOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E82E File Offset: 0x0000D82E
		protected virtual QilNode VisitTypeAssert(QilTargetType n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E837 File Offset: 0x0000D837
		protected virtual QilNode VisitIsType(QilTargetType n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E840 File Offset: 0x0000D840
		protected virtual QilNode VisitIsEmpty(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E849 File Offset: 0x0000D849
		protected virtual QilNode VisitXPathNodeValue(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E852 File Offset: 0x0000D852
		protected virtual QilNode VisitXPathFollowing(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000E85B File Offset: 0x0000D85B
		protected virtual QilNode VisitXPathPreceding(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000E864 File Offset: 0x0000D864
		protected virtual QilNode VisitXPathNamespace(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000E86D File Offset: 0x0000D86D
		protected virtual QilNode VisitXsltGenerateId(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000E876 File Offset: 0x0000D876
		protected virtual QilNode VisitXsltInvokeLateBound(QilInvokeLateBound n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000E87F File Offset: 0x0000D87F
		protected virtual QilNode VisitXsltInvokeEarlyBound(QilInvokeEarlyBound n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E888 File Offset: 0x0000D888
		protected virtual QilNode VisitXsltCopy(QilBinary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000E891 File Offset: 0x0000D891
		protected virtual QilNode VisitXsltCopyOf(QilUnary n)
		{
			return this.VisitChildren(n);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000E89A File Offset: 0x0000D89A
		protected virtual QilNode VisitXsltConvert(QilTargetType n)
		{
			return this.VisitChildren(n);
		}
	}
}
