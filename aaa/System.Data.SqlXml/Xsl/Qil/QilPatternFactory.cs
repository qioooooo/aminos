using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000059 RID: 89
	internal class QilPatternFactory
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x000216B3 File Offset: 0x000206B3
		public QilPatternFactory(QilFactory f, bool debug)
		{
			this.f = f;
			this.debug = debug;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x000216C9 File Offset: 0x000206C9
		public QilFactory BaseFactory
		{
			get
			{
				return this.f;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x000216D1 File Offset: 0x000206D1
		public bool IsDebug
		{
			get
			{
				return this.debug;
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000216D9 File Offset: 0x000206D9
		public QilLiteral String(string val)
		{
			return this.f.LiteralString(val);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000216E7 File Offset: 0x000206E7
		public QilLiteral Int32(int val)
		{
			return this.f.LiteralInt32(val);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000216F5 File Offset: 0x000206F5
		public QilLiteral Double(double val)
		{
			return this.f.LiteralDouble(val);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00021703 File Offset: 0x00020703
		public QilName QName(string local, string uri, string prefix)
		{
			return this.f.LiteralQName(local, uri, prefix);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00021713 File Offset: 0x00020713
		public QilName QName(string local, string uri)
		{
			return this.f.LiteralQName(local, uri, string.Empty);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00021727 File Offset: 0x00020727
		public QilName QName(string local)
		{
			return this.f.LiteralQName(local, string.Empty, string.Empty);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0002173F File Offset: 0x0002073F
		public QilNode Unknown(XmlQueryType t)
		{
			return this.f.Unknown(t);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002174D File Offset: 0x0002074D
		public QilExpression QilExpression(QilNode root, QilFactory factory)
		{
			return this.f.QilExpression(root, factory);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002175C File Offset: 0x0002075C
		public QilList FunctionList()
		{
			return this.f.FunctionList();
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00021769 File Offset: 0x00020769
		public QilList GlobalVariableList()
		{
			return this.f.GlobalVariableList();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00021776 File Offset: 0x00020776
		public QilList GlobalParameterList()
		{
			return this.f.GlobalParameterList();
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00021783 File Offset: 0x00020783
		public QilList ActualParameterList()
		{
			return this.f.ActualParameterList();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00021790 File Offset: 0x00020790
		public QilList ActualParameterList(QilNode arg1)
		{
			QilList qilList = this.f.ActualParameterList();
			qilList.Add(arg1);
			return qilList;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000217B4 File Offset: 0x000207B4
		public QilList ActualParameterList(QilNode arg1, QilNode arg2)
		{
			QilList qilList = this.f.ActualParameterList();
			qilList.Add(arg1);
			qilList.Add(arg2);
			return qilList;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000217DC File Offset: 0x000207DC
		public QilList ActualParameterList(params QilNode[] args)
		{
			return this.f.ActualParameterList(args);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000217EA File Offset: 0x000207EA
		public QilList FormalParameterList()
		{
			return this.f.FormalParameterList();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000217F8 File Offset: 0x000207F8
		public QilList FormalParameterList(QilNode arg1)
		{
			QilList qilList = this.f.FormalParameterList();
			qilList.Add(arg1);
			return qilList;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0002181C File Offset: 0x0002081C
		public QilList FormalParameterList(QilNode arg1, QilNode arg2)
		{
			QilList qilList = this.f.FormalParameterList();
			qilList.Add(arg1);
			qilList.Add(arg2);
			return qilList;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00021844 File Offset: 0x00020844
		public QilList FormalParameterList(params QilNode[] args)
		{
			return this.f.FormalParameterList(args);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00021852 File Offset: 0x00020852
		public QilList SortKeyList()
		{
			return this.f.SortKeyList();
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00021860 File Offset: 0x00020860
		public QilList SortKeyList(QilSortKey key)
		{
			QilList qilList = this.f.SortKeyList();
			qilList.Add(key);
			return qilList;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00021881 File Offset: 0x00020881
		public QilList BranchList(params QilNode[] args)
		{
			return this.f.BranchList(args);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0002188F File Offset: 0x0002088F
		public QilNode OptimizeBarrier(QilNode child)
		{
			return this.f.OptimizeBarrier(child);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0002189D File Offset: 0x0002089D
		public QilNode DataSource(QilNode name, QilNode baseUri)
		{
			return this.f.DataSource(name, baseUri);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000218AC File Offset: 0x000208AC
		public QilNode Nop(QilNode child)
		{
			return this.f.Nop(child);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000218BA File Offset: 0x000208BA
		public QilNode Error(QilNode text)
		{
			return this.f.Error(text);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000218C8 File Offset: 0x000208C8
		public QilNode Warning(QilNode text)
		{
			return this.f.Warning(text);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000218D6 File Offset: 0x000208D6
		public QilIterator For(QilNode binding)
		{
			return this.f.For(binding);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000218E4 File Offset: 0x000208E4
		public QilIterator Let(QilNode binding)
		{
			return this.f.Let(binding);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000218F2 File Offset: 0x000208F2
		public QilParameter Parameter(XmlQueryType t)
		{
			return this.f.Parameter(t);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00021900 File Offset: 0x00020900
		public QilParameter Parameter(QilNode defaultValue, QilName name, XmlQueryType t)
		{
			return this.f.Parameter(defaultValue, name, t);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00021910 File Offset: 0x00020910
		public QilNode PositionOf(QilIterator expr)
		{
			return this.f.PositionOf(expr);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0002191E File Offset: 0x0002091E
		public QilNode True()
		{
			return this.f.True();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0002192B File Offset: 0x0002092B
		public QilNode False()
		{
			return this.f.False();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00021938 File Offset: 0x00020938
		public QilNode Boolean(bool b)
		{
			if (!b)
			{
				return this.False();
			}
			return this.True();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002194A File Offset: 0x0002094A
		private static void CheckLogicArg(QilNode arg)
		{
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0002194C File Offset: 0x0002094C
		public QilNode And(QilNode left, QilNode right)
		{
			QilPatternFactory.CheckLogicArg(left);
			QilPatternFactory.CheckLogicArg(right);
			if (!this.debug)
			{
				if (left.NodeType == QilNodeType.True || right.NodeType == QilNodeType.False)
				{
					return right;
				}
				if (left.NodeType == QilNodeType.False || right.NodeType == QilNodeType.True)
				{
					return left;
				}
			}
			return this.f.And(left, right);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000219A8 File Offset: 0x000209A8
		public QilNode Or(QilNode left, QilNode right)
		{
			QilPatternFactory.CheckLogicArg(left);
			QilPatternFactory.CheckLogicArg(right);
			if (!this.debug)
			{
				if (left.NodeType == QilNodeType.True || right.NodeType == QilNodeType.False)
				{
					return left;
				}
				if (left.NodeType == QilNodeType.False || right.NodeType == QilNodeType.True)
				{
					return right;
				}
			}
			return this.f.Or(left, right);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00021A04 File Offset: 0x00020A04
		public QilNode Not(QilNode child)
		{
			if (!this.debug)
			{
				QilNodeType nodeType = child.NodeType;
				switch (nodeType)
				{
				case QilNodeType.True:
					return this.f.False();
				case QilNodeType.False:
					return this.f.True();
				default:
					if (nodeType == QilNodeType.Not)
					{
						return ((QilUnary)child).Child;
					}
					break;
				}
			}
			return this.f.Not(child);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00021A68 File Offset: 0x00020A68
		public QilNode Conditional(QilNode condition, QilNode trueBranch, QilNode falseBranch)
		{
			if (!this.debug)
			{
				QilNodeType nodeType = condition.NodeType;
				switch (nodeType)
				{
				case QilNodeType.True:
					return trueBranch;
				case QilNodeType.False:
					return falseBranch;
				default:
					if (nodeType == QilNodeType.Not)
					{
						return this.Conditional(((QilUnary)condition).Child, falseBranch, trueBranch);
					}
					break;
				}
			}
			return this.f.Conditional(condition, trueBranch, falseBranch);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00021AC4 File Offset: 0x00020AC4
		public QilNode Choice(QilNode expr, QilList branches)
		{
			if (!this.debug)
			{
				switch (branches.Count)
				{
				case 1:
					return this.f.Loop(this.f.Let(expr), branches[0]);
				case 2:
					return this.f.Conditional(this.f.Eq(expr, this.f.LiteralInt32(0)), branches[0], branches[1]);
				}
			}
			return this.f.Choice(expr, branches);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00021B50 File Offset: 0x00020B50
		public QilNode Length(QilNode child)
		{
			return this.f.Length(child);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00021B5E File Offset: 0x00020B5E
		public QilNode Sequence()
		{
			return this.f.Sequence();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00021B6C File Offset: 0x00020B6C
		public QilNode Sequence(QilNode child)
		{
			if (!this.debug)
			{
				return child;
			}
			QilList qilList = this.f.Sequence();
			qilList.Add(child);
			return qilList;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00021B98 File Offset: 0x00020B98
		public QilNode Sequence(QilNode child1, QilNode child2)
		{
			QilList qilList = this.f.Sequence();
			qilList.Add(child1);
			qilList.Add(child2);
			return qilList;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00021BC0 File Offset: 0x00020BC0
		public QilNode Sequence(params QilNode[] args)
		{
			if (!this.debug)
			{
				switch (args.Length)
				{
				case 0:
					return this.f.Sequence();
				case 1:
					return args[0];
				}
			}
			QilList qilList = this.f.Sequence();
			foreach (QilNode qilNode in args)
			{
				qilList.Add(qilNode);
			}
			return qilList;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00021C26 File Offset: 0x00020C26
		public QilNode Union(QilNode left, QilNode right)
		{
			return this.f.Union(left, right);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00021C35 File Offset: 0x00020C35
		public QilNode Sum(QilNode collection)
		{
			return this.f.Sum(collection);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00021C43 File Offset: 0x00020C43
		public QilNode Negate(QilNode child)
		{
			return this.f.Negate(child);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00021C51 File Offset: 0x00020C51
		public QilNode Add(QilNode left, QilNode right)
		{
			return this.f.Add(left, right);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00021C60 File Offset: 0x00020C60
		public QilNode Subtract(QilNode left, QilNode right)
		{
			return this.f.Subtract(left, right);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00021C6F File Offset: 0x00020C6F
		public QilNode Multiply(QilNode left, QilNode right)
		{
			return this.f.Multiply(left, right);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00021C7E File Offset: 0x00020C7E
		public QilNode Divide(QilNode left, QilNode right)
		{
			return this.f.Divide(left, right);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00021C8D File Offset: 0x00020C8D
		public QilNode Modulo(QilNode left, QilNode right)
		{
			return this.f.Modulo(left, right);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00021C9C File Offset: 0x00020C9C
		public QilNode StrLength(QilNode str)
		{
			return this.f.StrLength(str);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00021CAA File Offset: 0x00020CAA
		public QilNode StrConcat(QilNode values)
		{
			if (!this.debug && values.XmlType.IsSingleton)
			{
				return values;
			}
			return this.f.StrConcat(values);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00021CCF File Offset: 0x00020CCF
		public QilNode StrConcat(params QilNode[] args)
		{
			return this.StrConcat(args);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00021CD8 File Offset: 0x00020CD8
		public QilNode StrConcat(IList<QilNode> args)
		{
			if (!this.debug)
			{
				switch (args.Count)
				{
				case 0:
					return this.f.LiteralString(string.Empty);
				case 1:
					return this.StrConcat(args[0]);
				}
			}
			return this.StrConcat(this.f.Sequence(args));
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00021D35 File Offset: 0x00020D35
		public QilNode StrParseQName(QilNode str, QilNode ns)
		{
			return this.f.StrParseQName(str, ns);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00021D44 File Offset: 0x00020D44
		public QilNode Ne(QilNode left, QilNode right)
		{
			return this.f.Ne(left, right);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00021D53 File Offset: 0x00020D53
		public QilNode Eq(QilNode left, QilNode right)
		{
			return this.f.Eq(left, right);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00021D62 File Offset: 0x00020D62
		public QilNode Gt(QilNode left, QilNode right)
		{
			return this.f.Gt(left, right);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021D71 File Offset: 0x00020D71
		public QilNode Ge(QilNode left, QilNode right)
		{
			return this.f.Ge(left, right);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00021D80 File Offset: 0x00020D80
		public QilNode Lt(QilNode left, QilNode right)
		{
			return this.f.Lt(left, right);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021D8F File Offset: 0x00020D8F
		public QilNode Le(QilNode left, QilNode right)
		{
			return this.f.Le(left, right);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00021D9E File Offset: 0x00020D9E
		public QilNode Is(QilNode left, QilNode right)
		{
			return this.f.Is(left, right);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021DAD File Offset: 0x00020DAD
		public QilNode After(QilNode left, QilNode right)
		{
			return this.f.After(left, right);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00021DBC File Offset: 0x00020DBC
		public QilNode Before(QilNode left, QilNode right)
		{
			return this.f.Before(left, right);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021DCB File Offset: 0x00020DCB
		public QilNode Loop(QilIterator variable, QilNode body)
		{
			if (!this.debug && body == variable.Binding)
			{
				return body;
			}
			return this.f.Loop(variable, body);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00021DED File Offset: 0x00020DED
		public QilNode Filter(QilIterator variable, QilNode expr)
		{
			if (!this.debug && expr.NodeType == QilNodeType.True)
			{
				return variable.Binding;
			}
			return this.f.Filter(variable, expr);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021E15 File Offset: 0x00020E15
		public QilNode Sort(QilIterator iter, QilNode keys)
		{
			return this.f.Sort(iter, keys);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021E24 File Offset: 0x00020E24
		public QilSortKey SortKey(QilNode key, QilNode collation)
		{
			return this.f.SortKey(key, collation);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00021E33 File Offset: 0x00020E33
		public QilNode DocOrderDistinct(QilNode collection)
		{
			if (collection.NodeType == QilNodeType.DocOrderDistinct)
			{
				return collection;
			}
			return this.f.DocOrderDistinct(collection);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00021E4D File Offset: 0x00020E4D
		public QilFunction Function(QilList args, QilNode sideEffects, XmlQueryType resultType)
		{
			return this.f.Function(args, sideEffects, resultType);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021E5D File Offset: 0x00020E5D
		public QilFunction Function(QilList args, QilNode defn, QilNode sideEffects)
		{
			return this.f.Function(args, defn, sideEffects, defn.XmlType);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00021E73 File Offset: 0x00020E73
		public QilNode Invoke(QilFunction func, QilList args)
		{
			return this.f.Invoke(func, args);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00021E82 File Offset: 0x00020E82
		public QilNode Content(QilNode context)
		{
			return this.f.Content(context);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00021E90 File Offset: 0x00020E90
		public QilNode Parent(QilNode context)
		{
			return this.f.Parent(context);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00021E9E File Offset: 0x00020E9E
		public QilNode Root(QilNode context)
		{
			return this.f.Root(context);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00021EAC File Offset: 0x00020EAC
		public QilNode XmlContext()
		{
			return this.f.XmlContext();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00021EB9 File Offset: 0x00020EB9
		public QilNode Descendant(QilNode expr)
		{
			return this.f.Descendant(expr);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00021EC7 File Offset: 0x00020EC7
		public QilNode DescendantOrSelf(QilNode context)
		{
			return this.f.DescendantOrSelf(context);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00021ED5 File Offset: 0x00020ED5
		public QilNode Ancestor(QilNode expr)
		{
			return this.f.Ancestor(expr);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00021EE3 File Offset: 0x00020EE3
		public QilNode AncestorOrSelf(QilNode expr)
		{
			return this.f.AncestorOrSelf(expr);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00021EF1 File Offset: 0x00020EF1
		public QilNode Preceding(QilNode expr)
		{
			return this.f.Preceding(expr);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00021EFF File Offset: 0x00020EFF
		public QilNode FollowingSibling(QilNode expr)
		{
			return this.f.FollowingSibling(expr);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00021F0D File Offset: 0x00020F0D
		public QilNode PrecedingSibling(QilNode expr)
		{
			return this.f.PrecedingSibling(expr);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00021F1B File Offset: 0x00020F1B
		public QilNode NodeRange(QilNode left, QilNode right)
		{
			return this.f.NodeRange(left, right);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00021F2A File Offset: 0x00020F2A
		public QilBinary Deref(QilNode context, QilNode id)
		{
			return this.f.Deref(context, id);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00021F39 File Offset: 0x00020F39
		public QilNode ElementCtor(QilNode name, QilNode content)
		{
			return this.f.ElementCtor(name, content);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00021F48 File Offset: 0x00020F48
		public QilNode AttributeCtor(QilNode name, QilNode val)
		{
			return this.f.AttributeCtor(name, val);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00021F57 File Offset: 0x00020F57
		public QilNode CommentCtor(QilNode content)
		{
			return this.f.CommentCtor(content);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00021F65 File Offset: 0x00020F65
		public QilNode PICtor(QilNode name, QilNode content)
		{
			return this.f.PICtor(name, content);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00021F74 File Offset: 0x00020F74
		public QilNode TextCtor(QilNode content)
		{
			return this.f.TextCtor(content);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00021F82 File Offset: 0x00020F82
		public QilNode RawTextCtor(QilNode content)
		{
			return this.f.RawTextCtor(content);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00021F90 File Offset: 0x00020F90
		public QilNode DocumentCtor(QilNode child)
		{
			return this.f.DocumentCtor(child);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00021F9E File Offset: 0x00020F9E
		public QilNode NamespaceDecl(QilNode prefix, QilNode uri)
		{
			return this.f.NamespaceDecl(prefix, uri);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00021FAD File Offset: 0x00020FAD
		public QilNode RtfCtor(QilNode content, QilNode baseUri)
		{
			return this.f.RtfCtor(content, baseUri);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00021FBC File Offset: 0x00020FBC
		public QilNode NameOf(QilNode expr)
		{
			return this.f.NameOf(expr);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00021FCA File Offset: 0x00020FCA
		public QilNode LocalNameOf(QilNode expr)
		{
			return this.f.LocalNameOf(expr);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00021FD8 File Offset: 0x00020FD8
		public QilNode NamespaceUriOf(QilNode expr)
		{
			return this.f.NamespaceUriOf(expr);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00021FE6 File Offset: 0x00020FE6
		public QilNode PrefixOf(QilNode expr)
		{
			return this.f.PrefixOf(expr);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00021FF4 File Offset: 0x00020FF4
		public QilNode TypeAssert(QilNode expr, XmlQueryType t)
		{
			return this.f.TypeAssert(expr, t);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00022003 File Offset: 0x00021003
		public QilNode IsType(QilNode expr, XmlQueryType t)
		{
			return this.f.IsType(expr, t);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00022012 File Offset: 0x00021012
		public QilNode IsEmpty(QilNode set)
		{
			return this.f.IsEmpty(set);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00022020 File Offset: 0x00021020
		public QilNode XPathNodeValue(QilNode expr)
		{
			return this.f.XPathNodeValue(expr);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002202E File Offset: 0x0002102E
		public QilNode XPathFollowing(QilNode expr)
		{
			return this.f.XPathFollowing(expr);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002203C File Offset: 0x0002103C
		public QilNode XPathNamespace(QilNode expr)
		{
			return this.f.XPathNamespace(expr);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002204A File Offset: 0x0002104A
		public QilNode XPathPreceding(QilNode expr)
		{
			return this.f.XPathPreceding(expr);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00022058 File Offset: 0x00021058
		public QilNode XsltGenerateId(QilNode expr)
		{
			return this.f.XsltGenerateId(expr);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00022068 File Offset: 0x00021068
		public QilNode XsltInvokeEarlyBound(QilNode name, MethodInfo d, XmlQueryType t, IList<QilNode> args)
		{
			QilList qilList = this.f.ActualParameterList();
			qilList.Add(args);
			return this.f.XsltInvokeEarlyBound(name, this.f.LiteralObject(d), qilList, t);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000220A4 File Offset: 0x000210A4
		public QilNode XsltInvokeLateBound(QilNode name, IList<QilNode> args)
		{
			QilList qilList = this.f.ActualParameterList();
			qilList.Add(args);
			return this.f.XsltInvokeLateBound(name, qilList);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000220D1 File Offset: 0x000210D1
		public QilNode XsltCopy(QilNode expr, QilNode content)
		{
			return this.f.XsltCopy(expr, content);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000220E0 File Offset: 0x000210E0
		public QilNode XsltCopyOf(QilNode expr)
		{
			return this.f.XsltCopyOf(expr);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000220EE File Offset: 0x000210EE
		public QilNode XsltConvert(QilNode expr, XmlQueryType t)
		{
			return this.f.XsltConvert(expr, t);
		}

		// Token: 0x04000409 RID: 1033
		private bool debug;

		// Token: 0x0400040A RID: 1034
		private QilFactory f;
	}
}
