using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000D5 RID: 213
	internal class XPathBuilder : IXPathBuilder<QilNode>, IXPathEnvironment, IFocus
	{
		// Token: 0x060009E6 RID: 2534 RVA: 0x0002EC59 File Offset: 0x0002DC59
		QilNode IFocus.GetCurrent()
		{
			return this.GetCurrentNode();
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0002EC61 File Offset: 0x0002DC61
		QilNode IFocus.GetPosition()
		{
			return this.GetCurrentPosition();
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0002EC69 File Offset: 0x0002DC69
		QilNode IFocus.GetLast()
		{
			return this.GetLastPosition();
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002EC71 File Offset: 0x0002DC71
		XPathQilFactory IXPathEnvironment.Factory
		{
			get
			{
				return this.f;
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002EC79 File Offset: 0x0002DC79
		QilNode IXPathEnvironment.ResolveVariable(string prefix, string name)
		{
			return this.Variable(prefix, name);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002EC83 File Offset: 0x0002DC83
		QilNode IXPathEnvironment.ResolveFunction(string prefix, string name, IList<QilNode> args, IFocus env)
		{
			return null;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002EC86 File Offset: 0x0002DC86
		string IXPathEnvironment.ResolvePrefix(string prefix)
		{
			return this.environment.ResolvePrefix(prefix);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002EC94 File Offset: 0x0002DC94
		public XPathBuilder(IXPathEnvironment environment)
		{
			this.environment = environment;
			this.f = this.environment.Factory;
			this.fixupCurrent = this.f.Unknown(XmlQueryTypeFactory.NodeNotRtf);
			this.fixupPosition = this.f.Unknown(XmlQueryTypeFactory.DoubleX);
			this.fixupLast = this.f.Unknown(XmlQueryTypeFactory.DoubleX);
			this.fixupVisitor = new XPathBuilder.FixupVisitor(this.f, this.fixupCurrent, this.fixupPosition, this.fixupLast);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002ED24 File Offset: 0x0002DD24
		public virtual void StartBuild()
		{
			this.inTheBuild = true;
			this.numFixupCurrent = (this.numFixupPosition = (this.numFixupLast = 0));
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002ED54 File Offset: 0x0002DD54
		public virtual QilNode EndBuild(QilNode result)
		{
			if (result == null)
			{
				this.inTheBuild = false;
				return result;
			}
			if (result.XmlType.MaybeMany && result.XmlType.IsNode && result.XmlType.IsNotRtf)
			{
				result = this.f.DocOrderDistinct(result);
			}
			result = this.fixupVisitor.Fixup(result, this.environment);
			this.numFixupCurrent -= this.fixupVisitor.numCurrent;
			this.numFixupPosition -= this.fixupVisitor.numPosition;
			this.numFixupLast -= this.fixupVisitor.numLast;
			this.inTheBuild = false;
			return result;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0002EE06 File Offset: 0x0002DE06
		private QilNode GetCurrentNode()
		{
			this.numFixupCurrent++;
			return this.fixupCurrent;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002EE1C File Offset: 0x0002DE1C
		private QilNode GetCurrentPosition()
		{
			this.numFixupPosition++;
			return this.fixupPosition;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002EE32 File Offset: 0x0002DE32
		private QilNode GetLastPosition()
		{
			this.numFixupLast++;
			return this.fixupLast;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002EE48 File Offset: 0x0002DE48
		public virtual QilNode String(string value)
		{
			return this.f.String(value);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002EE56 File Offset: 0x0002DE56
		public virtual QilNode Number(double value)
		{
			return this.f.Double(value);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002EE64 File Offset: 0x0002DE64
		public virtual QilNode Operator(XPathOperator op, QilNode left, QilNode right)
		{
			switch (XPathBuilder.OperatorGroup[(int)op])
			{
			case XPathBuilder.XPathOperatorGroup.Logical:
				return this.LogicalOperator(op, left, right);
			case XPathBuilder.XPathOperatorGroup.Equality:
				return this.EqualityOperator(op, left, right);
			case XPathBuilder.XPathOperatorGroup.Relational:
				return this.RelationalOperator(op, left, right);
			case XPathBuilder.XPathOperatorGroup.Arithmetic:
				return this.ArithmeticOperator(op, left, right);
			case XPathBuilder.XPathOperatorGroup.Negate:
				return this.NegateOperator(op, left, right);
			case XPathBuilder.XPathOperatorGroup.Union:
				return this.UnionOperator(op, left, right);
			default:
				return null;
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002EED8 File Offset: 0x0002DED8
		private QilNode LogicalOperator(XPathOperator op, QilNode left, QilNode right)
		{
			left = this.f.ConvertToBoolean(left);
			right = this.f.ConvertToBoolean(right);
			if (op != XPathOperator.Or)
			{
				return this.f.And(left, right);
			}
			return this.f.Or(left, right);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002EF18 File Offset: 0x0002DF18
		private QilNode CompareValues(XPathOperator op, QilNode left, QilNode right, XmlTypeCode compType)
		{
			left = this.f.ConvertToType(compType, left);
			right = this.f.ConvertToType(compType, right);
			switch (op)
			{
			case XPathOperator.Eq:
				return this.f.Eq(left, right);
			case XPathOperator.Ne:
				return this.f.Ne(left, right);
			case XPathOperator.Lt:
				return this.f.Lt(left, right);
			case XPathOperator.Le:
				return this.f.Le(left, right);
			case XPathOperator.Gt:
				return this.f.Gt(left, right);
			case XPathOperator.Ge:
				return this.f.Ge(left, right);
			default:
				return null;
			}
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002EFC0 File Offset: 0x0002DFC0
		private QilNode CompareNodeSetAndValue(XPathOperator op, QilNode nodeset, QilNode val, XmlTypeCode compType)
		{
			if (compType == XmlTypeCode.Boolean || nodeset.XmlType.IsSingleton)
			{
				return this.CompareValues(op, nodeset, val, compType);
			}
			QilIterator qilIterator = this.f.For(nodeset);
			return this.f.Not(this.f.IsEmpty(this.f.Filter(qilIterator, this.CompareValues(op, this.f.XPathNodeValue(qilIterator), val, compType))));
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002F031 File Offset: 0x0002E031
		private static XPathOperator InvertOp(XPathOperator op)
		{
			if (op == XPathOperator.Lt)
			{
				return XPathOperator.Gt;
			}
			if (op == XPathOperator.Le)
			{
				return XPathOperator.Ge;
			}
			if (op == XPathOperator.Gt)
			{
				return XPathOperator.Lt;
			}
			if (op != XPathOperator.Ge)
			{
				return op;
			}
			return XPathOperator.Le;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002F04C File Offset: 0x0002E04C
		private QilNode CompareNodeSetAndNodeSet(XPathOperator op, QilNode left, QilNode right, XmlTypeCode compType)
		{
			if (right.XmlType.IsSingleton)
			{
				return this.CompareNodeSetAndValue(op, left, right, compType);
			}
			if (left.XmlType.IsSingleton)
			{
				op = XPathBuilder.InvertOp(op);
				return this.CompareNodeSetAndValue(op, right, left, compType);
			}
			QilIterator qilIterator = this.f.For(left);
			QilIterator qilIterator2 = this.f.For(right);
			return this.f.Not(this.f.IsEmpty(this.f.Loop(qilIterator, this.f.Filter(qilIterator2, this.CompareValues(op, this.f.XPathNodeValue(qilIterator), this.f.XPathNodeValue(qilIterator2), compType)))));
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002F0FC File Offset: 0x0002E0FC
		private QilNode EqualityOperator(XPathOperator op, QilNode left, QilNode right)
		{
			XmlQueryType xmlType = left.XmlType;
			XmlQueryType xmlType2 = right.XmlType;
			if (this.f.IsAnyType(left) || this.f.IsAnyType(right))
			{
				return this.f.InvokeEqualityOperator(XPathBuilder.QilOperator[(int)op], left, right);
			}
			if (xmlType.IsNode && xmlType2.IsNode)
			{
				return this.CompareNodeSetAndNodeSet(op, left, right, XmlTypeCode.String);
			}
			if (xmlType.IsNode)
			{
				return this.CompareNodeSetAndValue(op, left, right, xmlType2.TypeCode);
			}
			if (xmlType2.IsNode)
			{
				return this.CompareNodeSetAndValue(op, right, left, xmlType.TypeCode);
			}
			XmlTypeCode xmlTypeCode = ((xmlType.TypeCode == XmlTypeCode.Boolean || xmlType2.TypeCode == XmlTypeCode.Boolean) ? XmlTypeCode.Boolean : ((xmlType.TypeCode == XmlTypeCode.Double || xmlType2.TypeCode == XmlTypeCode.Double) ? XmlTypeCode.Double : XmlTypeCode.String));
			return this.CompareValues(op, left, right, xmlTypeCode);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002F1D4 File Offset: 0x0002E1D4
		private QilNode RelationalOperator(XPathOperator op, QilNode left, QilNode right)
		{
			XmlQueryType xmlType = left.XmlType;
			XmlQueryType xmlType2 = right.XmlType;
			if (this.f.IsAnyType(left) || this.f.IsAnyType(right))
			{
				return this.f.InvokeRelationalOperator(XPathBuilder.QilOperator[(int)op], left, right);
			}
			if (xmlType.IsNode && xmlType2.IsNode)
			{
				return this.CompareNodeSetAndNodeSet(op, left, right, XmlTypeCode.Double);
			}
			if (xmlType.IsNode)
			{
				XmlTypeCode xmlTypeCode = ((xmlType2.TypeCode == XmlTypeCode.Boolean) ? XmlTypeCode.Boolean : XmlTypeCode.Double);
				return this.CompareNodeSetAndValue(op, left, right, xmlTypeCode);
			}
			if (xmlType2.IsNode)
			{
				XmlTypeCode xmlTypeCode2 = ((xmlType.TypeCode == XmlTypeCode.Boolean) ? XmlTypeCode.Boolean : XmlTypeCode.Double);
				op = XPathBuilder.InvertOp(op);
				return this.CompareNodeSetAndValue(op, right, left, xmlTypeCode2);
			}
			return this.CompareValues(op, left, right, XmlTypeCode.Double);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0002F297 File Offset: 0x0002E297
		private QilNode NegateOperator(XPathOperator op, QilNode left, QilNode right)
		{
			return this.f.Negate(this.f.ConvertToNumber(left));
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0002F2B0 File Offset: 0x0002E2B0
		private QilNode ArithmeticOperator(XPathOperator op, QilNode left, QilNode right)
		{
			left = this.f.ConvertToNumber(left);
			right = this.f.ConvertToNumber(right);
			switch (op)
			{
			case XPathOperator.Plus:
				return this.f.Add(left, right);
			case XPathOperator.Minus:
				return this.f.Subtract(left, right);
			case XPathOperator.Multiply:
				return this.f.Multiply(left, right);
			case XPathOperator.Divide:
				return this.f.Divide(left, right);
			case XPathOperator.Modulo:
				return this.f.Modulo(left, right);
			default:
				return null;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0002F344 File Offset: 0x0002E344
		private QilNode UnionOperator(XPathOperator op, QilNode left, QilNode right)
		{
			if (left == null)
			{
				return this.f.EnsureNodeSet(right);
			}
			left = this.f.EnsureNodeSet(left);
			right = this.f.EnsureNodeSet(right);
			if (left.NodeType == QilNodeType.Sequence)
			{
				((QilList)left).Add(right);
				return left;
			}
			return this.f.Union(left, right);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002F3A2 File Offset: 0x0002E3A2
		public static XmlNodeKindFlags AxisTypeMask(XmlNodeKindFlags inputTypeMask, XPathNodeType nodeType, XPathAxis xpathAxis)
		{
			return inputTypeMask & XPathBuilder.XPathNodeType2QilXmlNodeKind[(int)nodeType] & XPathBuilder.XPathAxisMask[(int)xpathAxis];
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002F3B8 File Offset: 0x0002E3B8
		private QilNode BuildAxisFilter(QilNode qilAxis, XPathAxis xpathAxis, XPathNodeType nodeType, string name, string nsUri)
		{
			XmlNodeKindFlags nodeKinds = qilAxis.XmlType.NodeKinds;
			XmlNodeKindFlags xmlNodeKindFlags = XPathBuilder.AxisTypeMask(nodeKinds, nodeType, xpathAxis);
			if (xmlNodeKindFlags == XmlNodeKindFlags.None)
			{
				return this.f.Sequence();
			}
			QilIterator qilIterator;
			if (xmlNodeKindFlags != nodeKinds)
			{
				qilAxis = this.f.Filter(qilIterator = this.f.For(qilAxis), this.f.IsType(qilIterator, XmlQueryTypeFactory.NodeChoice(xmlNodeKindFlags)));
				qilAxis.XmlType = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeChoice(xmlNodeKindFlags), qilAxis.XmlType.Cardinality);
				if (qilAxis.NodeType == QilNodeType.Filter)
				{
					QilLoop qilLoop = (QilLoop)qilAxis;
					qilLoop.Body = this.f.And(qilLoop.Body, (name != null && nsUri != null) ? this.f.Eq(this.f.NameOf(qilIterator), this.f.QName(name, nsUri)) : ((nsUri != null) ? this.f.Eq(this.f.NamespaceUriOf(qilIterator), this.f.String(nsUri)) : ((name != null) ? this.f.Eq(this.f.LocalNameOf(qilIterator), this.f.String(name)) : this.f.True())));
					return qilLoop;
				}
			}
			return this.f.Filter(qilIterator = this.f.For(qilAxis), (name != null && nsUri != null) ? this.f.Eq(this.f.NameOf(qilIterator), this.f.QName(name, nsUri)) : ((nsUri != null) ? this.f.Eq(this.f.NamespaceUriOf(qilIterator), this.f.String(nsUri)) : ((name != null) ? this.f.Eq(this.f.LocalNameOf(qilIterator), this.f.String(name)) : this.f.True())));
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0002F5A0 File Offset: 0x0002E5A0
		private QilNode BuildAxis(XPathAxis xpathAxis, XPathNodeType nodeType, string nsUri, string name)
		{
			QilNode currentNode = this.GetCurrentNode();
			QilNode qilNode;
			switch (xpathAxis)
			{
			case XPathAxis.Ancestor:
				qilNode = this.f.Ancestor(currentNode);
				break;
			case XPathAxis.AncestorOrSelf:
				qilNode = this.f.AncestorOrSelf(currentNode);
				break;
			case XPathAxis.Attribute:
				qilNode = this.f.Content(currentNode);
				break;
			case XPathAxis.Child:
				qilNode = this.f.Content(currentNode);
				break;
			case XPathAxis.Descendant:
				qilNode = this.f.Descendant(currentNode);
				break;
			case XPathAxis.DescendantOrSelf:
				qilNode = this.f.DescendantOrSelf(currentNode);
				break;
			case XPathAxis.Following:
				qilNode = this.f.XPathFollowing(currentNode);
				break;
			case XPathAxis.FollowingSibling:
				qilNode = this.f.FollowingSibling(currentNode);
				break;
			case XPathAxis.Namespace:
				qilNode = this.f.XPathNamespace(currentNode);
				break;
			case XPathAxis.Parent:
				qilNode = this.f.Parent(currentNode);
				break;
			case XPathAxis.Preceding:
				qilNode = this.f.XPathPreceding(currentNode);
				break;
			case XPathAxis.PrecedingSibling:
				qilNode = this.f.PrecedingSibling(currentNode);
				break;
			case XPathAxis.Self:
				qilNode = currentNode;
				break;
			case XPathAxis.Root:
				return this.f.Root(currentNode);
			default:
				qilNode = null;
				break;
			}
			QilNode qilNode2 = this.BuildAxisFilter(qilNode, xpathAxis, nodeType, name, nsUri);
			if (xpathAxis == XPathAxis.Ancestor || xpathAxis == XPathAxis.Preceding || xpathAxis == XPathAxis.AncestorOrSelf || xpathAxis == XPathAxis.PrecedingSibling)
			{
				qilNode2 = this.f.BaseFactory.DocOrderDistinct(qilNode2);
			}
			return qilNode2;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002F700 File Offset: 0x0002E700
		public virtual QilNode Axis(XPathAxis xpathAxis, XPathNodeType nodeType, string prefix, string name)
		{
			string text = ((prefix == null) ? null : this.environment.ResolvePrefix(prefix));
			return this.BuildAxis(xpathAxis, nodeType, text, name);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0002F72C File Offset: 0x0002E72C
		public virtual QilNode JoinStep(QilNode left, QilNode right)
		{
			QilIterator qilIterator = this.f.For(this.f.EnsureNodeSet(left));
			right = this.fixupVisitor.Fixup(right, qilIterator, null);
			this.numFixupCurrent -= this.fixupVisitor.numCurrent;
			this.numFixupPosition -= this.fixupVisitor.numPosition;
			this.numFixupLast -= this.fixupVisitor.numLast;
			return this.f.DocOrderDistinct(this.f.Loop(qilIterator, right));
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0002F7C4 File Offset: 0x0002E7C4
		public virtual QilNode Predicate(QilNode nodeset, QilNode predicate, bool isReverseStep)
		{
			if (isReverseStep)
			{
				nodeset = ((QilUnary)nodeset).Child;
			}
			nodeset = this.f.EnsureNodeSet(nodeset);
			if (!this.f.IsAnyType(predicate))
			{
				if (predicate.XmlType.TypeCode == XmlTypeCode.Double)
				{
					predicate = this.f.Eq(this.GetCurrentPosition(), predicate);
				}
				else
				{
					predicate = this.f.ConvertToBoolean(predicate);
				}
			}
			else
			{
				QilIterator qilIterator;
				predicate = this.f.Loop(qilIterator = this.f.Let(predicate), this.f.Conditional(this.f.IsType(qilIterator, XmlQueryTypeFactory.Double), this.f.Eq(this.GetCurrentPosition(), this.f.TypeAssert(qilIterator, XmlQueryTypeFactory.DoubleX)), this.f.ConvertToBoolean(qilIterator)));
			}
			QilNode qilNode;
			if (this.numFixupLast != 0 && this.fixupVisitor.CountUnfixedLast(predicate) != 0)
			{
				QilIterator qilIterator2 = this.f.Let(nodeset);
				QilIterator qilIterator3 = this.f.Let(this.f.XsltConvert(this.f.Length(qilIterator2), XmlQueryTypeFactory.DoubleX));
				QilIterator qilIterator4 = this.f.For(qilIterator2);
				predicate = this.fixupVisitor.Fixup(predicate, qilIterator4, qilIterator3);
				this.numFixupCurrent -= this.fixupVisitor.numCurrent;
				this.numFixupPosition -= this.fixupVisitor.numPosition;
				this.numFixupLast -= this.fixupVisitor.numLast;
				qilNode = this.f.Loop(qilIterator2, this.f.Loop(qilIterator3, this.f.Filter(qilIterator4, predicate)));
			}
			else
			{
				QilIterator qilIterator5 = this.f.For(nodeset);
				predicate = this.fixupVisitor.Fixup(predicate, qilIterator5, null);
				this.numFixupCurrent -= this.fixupVisitor.numCurrent;
				this.numFixupPosition -= this.fixupVisitor.numPosition;
				this.numFixupLast -= this.fixupVisitor.numLast;
				qilNode = this.f.Filter(qilIterator5, predicate);
			}
			if (isReverseStep)
			{
				qilNode = this.f.DocOrderDistinct(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0002F9FF File Offset: 0x0002E9FF
		public virtual QilNode Variable(string prefix, string name)
		{
			return this.environment.ResolveVariable(prefix, name);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0002FA10 File Offset: 0x0002EA10
		public virtual QilNode Function(string prefix, string name, IList<QilNode> args)
		{
			XPathBuilder.FunctionInfo<XPathBuilder.FuncId> functionInfo;
			if (prefix.Length != 0 || !XPathBuilder.FunctionTable.TryGetValue(name, out functionInfo))
			{
				return this.environment.ResolveFunction(prefix, name, args, this);
			}
			functionInfo.CastArguments(args, name, this.f);
			switch (functionInfo.id)
			{
			case XPathBuilder.FuncId.Last:
				return this.GetLastPosition();
			case XPathBuilder.FuncId.Position:
				return this.GetCurrentPosition();
			case XPathBuilder.FuncId.Count:
				return this.f.XsltConvert(this.f.Length(this.f.DocOrderDistinct(args[0])), XmlQueryTypeFactory.DoubleX);
			case XPathBuilder.FuncId.LocalName:
				if (args.Count != 0)
				{
					return this.LocalNameOfFirstNode(args[0]);
				}
				return this.f.LocalNameOf(this.GetCurrentNode());
			case XPathBuilder.FuncId.NamespaceUri:
				if (args.Count != 0)
				{
					return this.NamespaceOfFirstNode(args[0]);
				}
				return this.f.NamespaceUriOf(this.GetCurrentNode());
			case XPathBuilder.FuncId.Name:
				if (args.Count != 0)
				{
					return this.NameOfFirstNode(args[0]);
				}
				return this.NameOf(this.GetCurrentNode());
			case XPathBuilder.FuncId.String:
				if (args.Count != 0)
				{
					return this.f.ConvertToString(args[0]);
				}
				return this.f.XPathNodeValue(this.GetCurrentNode());
			case XPathBuilder.FuncId.Number:
				if (args.Count != 0)
				{
					return this.f.ConvertToNumber(args[0]);
				}
				return this.f.XsltConvert(this.f.XPathNodeValue(this.GetCurrentNode()), XmlQueryTypeFactory.DoubleX);
			case XPathBuilder.FuncId.Boolean:
				return this.f.ConvertToBoolean(args[0]);
			case XPathBuilder.FuncId.True:
				return this.f.True();
			case XPathBuilder.FuncId.False:
				return this.f.False();
			case XPathBuilder.FuncId.Not:
				return this.f.Not(args[0]);
			case XPathBuilder.FuncId.Id:
				return this.f.DocOrderDistinct(this.f.Id(this.GetCurrentNode(), args[0]));
			case XPathBuilder.FuncId.Concat:
				return this.f.StrConcat(args);
			case XPathBuilder.FuncId.StartsWith:
				return this.f.InvokeStartsWith(args[0], args[1]);
			case XPathBuilder.FuncId.Contains:
				return this.f.InvokeContains(args[0], args[1]);
			case XPathBuilder.FuncId.SubstringBefore:
				return this.f.InvokeSubstringBefore(args[0], args[1]);
			case XPathBuilder.FuncId.SubstringAfter:
				return this.f.InvokeSubstringAfter(args[0], args[1]);
			case XPathBuilder.FuncId.Substring:
				if (args.Count != 2)
				{
					return this.f.InvokeSubstring(args[0], args[1], args[2]);
				}
				return this.f.InvokeSubstring(args[0], args[1]);
			case XPathBuilder.FuncId.StringLength:
				return this.f.XsltConvert(this.f.StrLength((args.Count == 0) ? this.f.XPathNodeValue(this.GetCurrentNode()) : args[0]), XmlQueryTypeFactory.DoubleX);
			case XPathBuilder.FuncId.Normalize:
				return this.f.InvokeNormalizeSpace((args.Count == 0) ? this.f.XPathNodeValue(this.GetCurrentNode()) : args[0]);
			case XPathBuilder.FuncId.Translate:
				return this.f.InvokeTranslate(args[0], args[1], args[2]);
			case XPathBuilder.FuncId.Lang:
				return this.f.InvokeLang(args[0], this.GetCurrentNode());
			case XPathBuilder.FuncId.Sum:
				return this.Sum(this.f.DocOrderDistinct(args[0]));
			case XPathBuilder.FuncId.Floor:
				return this.f.InvokeFloor(args[0]);
			case XPathBuilder.FuncId.Ceiling:
				return this.f.InvokeCeiling(args[0]);
			case XPathBuilder.FuncId.Round:
				return this.f.InvokeRound(args[0]);
			default:
				return null;
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0002FE04 File Offset: 0x0002EE04
		private QilNode LocalNameOfFirstNode(QilNode arg)
		{
			if (arg.XmlType.IsSingleton)
			{
				return this.f.LocalNameOf(arg);
			}
			QilIterator qilIterator;
			return this.f.StrConcat(this.f.Loop(qilIterator = this.f.FirstNode(arg), this.f.LocalNameOf(qilIterator)));
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0002FE5C File Offset: 0x0002EE5C
		private QilNode NamespaceOfFirstNode(QilNode arg)
		{
			if (arg.XmlType.IsSingleton)
			{
				return this.f.NamespaceUriOf(arg);
			}
			QilIterator qilIterator;
			return this.f.StrConcat(this.f.Loop(qilIterator = this.f.FirstNode(arg), this.f.NamespaceUriOf(qilIterator)));
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0002FEB4 File Offset: 0x0002EEB4
		private QilNode NameOf(QilNode arg)
		{
			if (arg is QilIterator)
			{
				QilIterator qilIterator;
				QilIterator qilIterator2;
				return this.f.Loop(qilIterator = this.f.Let(this.f.PrefixOf(arg)), this.f.Loop(qilIterator2 = this.f.Let(this.f.LocalNameOf(arg)), this.f.Conditional(this.f.Eq(this.f.StrLength(qilIterator), this.f.Int32(0)), qilIterator2, this.f.StrConcat(new QilNode[]
				{
					qilIterator,
					this.f.String(":"),
					qilIterator2
				}))));
			}
			QilIterator qilIterator3 = this.f.Let(arg);
			return this.f.Loop(qilIterator3, this.NameOf(qilIterator3));
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0002FF94 File Offset: 0x0002EF94
		private QilNode NameOfFirstNode(QilNode arg)
		{
			if (arg.XmlType.IsSingleton)
			{
				return this.NameOf(arg);
			}
			QilIterator qilIterator;
			return this.f.StrConcat(this.f.Loop(qilIterator = this.f.FirstNode(arg), this.NameOf(qilIterator)));
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0002FFE4 File Offset: 0x0002EFE4
		private QilNode Sum(QilNode arg)
		{
			QilIterator qilIterator;
			return this.f.Sum(this.f.Sequence(this.f.Double(0.0), this.f.Loop(qilIterator = this.f.For(arg), this.f.ConvertToNumber(qilIterator))));
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00030040 File Offset: 0x0002F040
		private static Dictionary<string, XPathBuilder.FunctionInfo<XPathBuilder.FuncId>> CreateFunctionTable()
		{
			return new Dictionary<string, XPathBuilder.FunctionInfo<XPathBuilder.FuncId>>(36)
			{
				{
					"last",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Last, 0, 0, null)
				},
				{
					"position",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Position, 0, 0, null)
				},
				{
					"name",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Name, 0, 1, XPathBuilder.argNodeSet)
				},
				{
					"namespace-uri",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.NamespaceUri, 0, 1, XPathBuilder.argNodeSet)
				},
				{
					"local-name",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.LocalName, 0, 1, XPathBuilder.argNodeSet)
				},
				{
					"count",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Count, 1, 1, XPathBuilder.argNodeSet)
				},
				{
					"id",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Id, 1, 1, XPathBuilder.argAny)
				},
				{
					"string",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.String, 0, 1, XPathBuilder.argAny)
				},
				{
					"concat",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Concat, 2, int.MaxValue, null)
				},
				{
					"starts-with",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.StartsWith, 2, 2, XPathBuilder.argString2)
				},
				{
					"contains",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Contains, 2, 2, XPathBuilder.argString2)
				},
				{
					"substring-before",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.SubstringBefore, 2, 2, XPathBuilder.argString2)
				},
				{
					"substring-after",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.SubstringAfter, 2, 2, XPathBuilder.argString2)
				},
				{
					"substring",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Substring, 2, 3, XPathBuilder.argFnSubstr)
				},
				{
					"string-length",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.StringLength, 0, 1, XPathBuilder.argString)
				},
				{
					"normalize-space",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Normalize, 0, 1, XPathBuilder.argString)
				},
				{
					"translate",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Translate, 3, 3, XPathBuilder.argString3)
				},
				{
					"boolean",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Boolean, 1, 1, XPathBuilder.argAny)
				},
				{
					"not",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Not, 1, 1, XPathBuilder.argBoolean)
				},
				{
					"true",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.True, 0, 0, null)
				},
				{
					"false",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.False, 0, 0, null)
				},
				{
					"lang",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Lang, 1, 1, XPathBuilder.argString)
				},
				{
					"number",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Number, 0, 1, XPathBuilder.argAny)
				},
				{
					"sum",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Sum, 1, 1, XPathBuilder.argNodeSet)
				},
				{
					"floor",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Floor, 1, 1, XPathBuilder.argDouble)
				},
				{
					"ceiling",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Ceiling, 1, 1, XPathBuilder.argDouble)
				},
				{
					"round",
					new XPathBuilder.FunctionInfo<XPathBuilder.FuncId>(XPathBuilder.FuncId.Round, 1, 1, XPathBuilder.argDouble)
				}
			};
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000302E0 File Offset: 0x0002F2E0
		public static bool IsFunctionAvailable(string localName, string nsUri)
		{
			return nsUri.Length == 0 && XPathBuilder.FunctionTable.ContainsKey(localName);
		}

		// Token: 0x04000641 RID: 1601
		private XPathQilFactory f;

		// Token: 0x04000642 RID: 1602
		private IXPathEnvironment environment;

		// Token: 0x04000643 RID: 1603
		private bool inTheBuild;

		// Token: 0x04000644 RID: 1604
		protected QilNode fixupCurrent;

		// Token: 0x04000645 RID: 1605
		protected QilNode fixupPosition;

		// Token: 0x04000646 RID: 1606
		protected QilNode fixupLast;

		// Token: 0x04000647 RID: 1607
		protected int numFixupCurrent;

		// Token: 0x04000648 RID: 1608
		protected int numFixupPosition;

		// Token: 0x04000649 RID: 1609
		protected int numFixupLast;

		// Token: 0x0400064A RID: 1610
		private XPathBuilder.FixupVisitor fixupVisitor;

		// Token: 0x0400064B RID: 1611
		private static XmlNodeKindFlags[] XPathNodeType2QilXmlNodeKind = new XmlNodeKindFlags[]
		{
			XmlNodeKindFlags.Document,
			XmlNodeKindFlags.Element,
			XmlNodeKindFlags.Attribute,
			XmlNodeKindFlags.Namespace,
			XmlNodeKindFlags.Text,
			XmlNodeKindFlags.Text,
			XmlNodeKindFlags.Text,
			XmlNodeKindFlags.PI,
			XmlNodeKindFlags.Comment,
			XmlNodeKindFlags.Any
		};

		// Token: 0x0400064C RID: 1612
		private static XPathBuilder.XPathOperatorGroup[] OperatorGroup = new XPathBuilder.XPathOperatorGroup[]
		{
			XPathBuilder.XPathOperatorGroup.Unknown,
			XPathBuilder.XPathOperatorGroup.Logical,
			XPathBuilder.XPathOperatorGroup.Logical,
			XPathBuilder.XPathOperatorGroup.Equality,
			XPathBuilder.XPathOperatorGroup.Equality,
			XPathBuilder.XPathOperatorGroup.Relational,
			XPathBuilder.XPathOperatorGroup.Relational,
			XPathBuilder.XPathOperatorGroup.Relational,
			XPathBuilder.XPathOperatorGroup.Relational,
			XPathBuilder.XPathOperatorGroup.Arithmetic,
			XPathBuilder.XPathOperatorGroup.Arithmetic,
			XPathBuilder.XPathOperatorGroup.Arithmetic,
			XPathBuilder.XPathOperatorGroup.Arithmetic,
			XPathBuilder.XPathOperatorGroup.Arithmetic,
			XPathBuilder.XPathOperatorGroup.Negate,
			XPathBuilder.XPathOperatorGroup.Union
		};

		// Token: 0x0400064D RID: 1613
		private static QilNodeType[] QilOperator = new QilNodeType[]
		{
			QilNodeType.Unknown,
			QilNodeType.Or,
			QilNodeType.And,
			QilNodeType.Eq,
			QilNodeType.Ne,
			QilNodeType.Lt,
			QilNodeType.Le,
			QilNodeType.Gt,
			QilNodeType.Ge,
			QilNodeType.Add,
			QilNodeType.Subtract,
			QilNodeType.Multiply,
			QilNodeType.Divide,
			QilNodeType.Modulo,
			QilNodeType.Negate,
			QilNodeType.Sequence
		};

		// Token: 0x0400064E RID: 1614
		private static XmlNodeKindFlags[] XPathAxisMask = new XmlNodeKindFlags[]
		{
			XmlNodeKindFlags.None,
			XmlNodeKindFlags.Document | XmlNodeKindFlags.Element,
			XmlNodeKindFlags.Any,
			XmlNodeKindFlags.Attribute,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Any,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Namespace,
			XmlNodeKindFlags.Document | XmlNodeKindFlags.Element,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Content,
			XmlNodeKindFlags.Any,
			XmlNodeKindFlags.Document
		};

		// Token: 0x0400064F RID: 1615
		public static readonly XmlTypeCode[] argAny = new XmlTypeCode[] { XmlTypeCode.Item };

		// Token: 0x04000650 RID: 1616
		public static readonly XmlTypeCode[] argNodeSet = new XmlTypeCode[] { XmlTypeCode.Node };

		// Token: 0x04000651 RID: 1617
		public static readonly XmlTypeCode[] argBoolean = new XmlTypeCode[] { XmlTypeCode.Boolean };

		// Token: 0x04000652 RID: 1618
		public static readonly XmlTypeCode[] argDouble = new XmlTypeCode[] { XmlTypeCode.Double };

		// Token: 0x04000653 RID: 1619
		public static readonly XmlTypeCode[] argString = new XmlTypeCode[] { XmlTypeCode.String };

		// Token: 0x04000654 RID: 1620
		public static readonly XmlTypeCode[] argString2 = new XmlTypeCode[]
		{
			XmlTypeCode.String,
			XmlTypeCode.String
		};

		// Token: 0x04000655 RID: 1621
		public static readonly XmlTypeCode[] argString3 = new XmlTypeCode[]
		{
			XmlTypeCode.String,
			XmlTypeCode.String,
			XmlTypeCode.String
		};

		// Token: 0x04000656 RID: 1622
		public static readonly XmlTypeCode[] argFnSubstr = new XmlTypeCode[]
		{
			XmlTypeCode.String,
			XmlTypeCode.Double,
			XmlTypeCode.Double
		};

		// Token: 0x04000657 RID: 1623
		public static Dictionary<string, XPathBuilder.FunctionInfo<XPathBuilder.FuncId>> FunctionTable = XPathBuilder.CreateFunctionTable();

		// Token: 0x020000D6 RID: 214
		private enum XPathOperatorGroup
		{
			// Token: 0x04000659 RID: 1625
			Unknown,
			// Token: 0x0400065A RID: 1626
			Logical,
			// Token: 0x0400065B RID: 1627
			Equality,
			// Token: 0x0400065C RID: 1628
			Relational,
			// Token: 0x0400065D RID: 1629
			Arithmetic,
			// Token: 0x0400065E RID: 1630
			Negate,
			// Token: 0x0400065F RID: 1631
			Union
		}

		// Token: 0x020000D7 RID: 215
		internal enum FuncId
		{
			// Token: 0x04000661 RID: 1633
			Last,
			// Token: 0x04000662 RID: 1634
			Position,
			// Token: 0x04000663 RID: 1635
			Count,
			// Token: 0x04000664 RID: 1636
			LocalName,
			// Token: 0x04000665 RID: 1637
			NamespaceUri,
			// Token: 0x04000666 RID: 1638
			Name,
			// Token: 0x04000667 RID: 1639
			String,
			// Token: 0x04000668 RID: 1640
			Number,
			// Token: 0x04000669 RID: 1641
			Boolean,
			// Token: 0x0400066A RID: 1642
			True,
			// Token: 0x0400066B RID: 1643
			False,
			// Token: 0x0400066C RID: 1644
			Not,
			// Token: 0x0400066D RID: 1645
			Id,
			// Token: 0x0400066E RID: 1646
			Concat,
			// Token: 0x0400066F RID: 1647
			StartsWith,
			// Token: 0x04000670 RID: 1648
			Contains,
			// Token: 0x04000671 RID: 1649
			SubstringBefore,
			// Token: 0x04000672 RID: 1650
			SubstringAfter,
			// Token: 0x04000673 RID: 1651
			Substring,
			// Token: 0x04000674 RID: 1652
			StringLength,
			// Token: 0x04000675 RID: 1653
			Normalize,
			// Token: 0x04000676 RID: 1654
			Translate,
			// Token: 0x04000677 RID: 1655
			Lang,
			// Token: 0x04000678 RID: 1656
			Sum,
			// Token: 0x04000679 RID: 1657
			Floor,
			// Token: 0x0400067A RID: 1658
			Ceiling,
			// Token: 0x0400067B RID: 1659
			Round
		}

		// Token: 0x020000D8 RID: 216
		private class FixupVisitor : QilReplaceVisitor
		{
			// Token: 0x06000A10 RID: 2576 RVA: 0x0003051A File Offset: 0x0002F51A
			public FixupVisitor(QilPatternFactory f, QilNode fixupCurrent, QilNode fixupPosition, QilNode fixupLast)
				: base(f.BaseFactory)
			{
				this.f = f;
				this.fixupCurrent = fixupCurrent;
				this.fixupPosition = fixupPosition;
				this.fixupLast = fixupLast;
			}

			// Token: 0x06000A11 RID: 2577 RVA: 0x00030548 File Offset: 0x0002F548
			public QilNode Fixup(QilNode inExpr, QilIterator current, QilNode last)
			{
				QilDepthChecker.Check(inExpr);
				this.current = current;
				this.last = last;
				this.justCount = false;
				this.environment = null;
				this.numCurrent = (this.numPosition = (this.numLast = 0));
				inExpr = this.VisitAssumeReference(inExpr);
				return inExpr;
			}

			// Token: 0x06000A12 RID: 2578 RVA: 0x0003059C File Offset: 0x0002F59C
			public QilNode Fixup(QilNode inExpr, IXPathEnvironment environment)
			{
				QilDepthChecker.Check(inExpr);
				this.justCount = false;
				this.current = null;
				this.environment = environment;
				this.numCurrent = (this.numPosition = (this.numLast = 0));
				inExpr = this.VisitAssumeReference(inExpr);
				return inExpr;
			}

			// Token: 0x06000A13 RID: 2579 RVA: 0x000305E8 File Offset: 0x0002F5E8
			public int CountUnfixedLast(QilNode inExpr)
			{
				this.justCount = true;
				this.numCurrent = (this.numPosition = (this.numLast = 0));
				this.VisitAssumeReference(inExpr);
				return this.numLast;
			}

			// Token: 0x06000A14 RID: 2580 RVA: 0x00030624 File Offset: 0x0002F624
			protected override QilNode VisitUnknown(QilNode unknown)
			{
				if (unknown == this.fixupCurrent)
				{
					this.numCurrent++;
					if (!this.justCount)
					{
						if (this.environment != null)
						{
							unknown = this.environment.GetCurrent();
						}
						else if (this.current != null)
						{
							unknown = this.current;
						}
					}
				}
				else if (unknown == this.fixupPosition)
				{
					this.numPosition++;
					if (!this.justCount)
					{
						if (this.environment != null)
						{
							unknown = this.environment.GetPosition();
						}
						else if (this.current != null)
						{
							unknown = this.f.XsltConvert(this.f.PositionOf(this.current), XmlQueryTypeFactory.DoubleX);
						}
					}
				}
				else if (unknown == this.fixupLast)
				{
					this.numLast++;
					if (!this.justCount)
					{
						if (this.environment != null)
						{
							unknown = this.environment.GetLast();
						}
						else if (this.current != null)
						{
							unknown = this.last;
						}
					}
				}
				return unknown;
			}

			// Token: 0x0400067C RID: 1660
			private new QilPatternFactory f;

			// Token: 0x0400067D RID: 1661
			private QilNode fixupCurrent;

			// Token: 0x0400067E RID: 1662
			private QilNode fixupPosition;

			// Token: 0x0400067F RID: 1663
			private QilNode fixupLast;

			// Token: 0x04000680 RID: 1664
			private QilIterator current;

			// Token: 0x04000681 RID: 1665
			private QilNode last;

			// Token: 0x04000682 RID: 1666
			private bool justCount;

			// Token: 0x04000683 RID: 1667
			private IXPathEnvironment environment;

			// Token: 0x04000684 RID: 1668
			public int numCurrent;

			// Token: 0x04000685 RID: 1669
			public int numPosition;

			// Token: 0x04000686 RID: 1670
			public int numLast;
		}

		// Token: 0x020000D9 RID: 217
		internal class FunctionInfo<T>
		{
			// Token: 0x06000A15 RID: 2581 RVA: 0x00030732 File Offset: 0x0002F732
			public FunctionInfo(T id, int minArgs, int maxArgs, XmlTypeCode[] argTypes)
			{
				this.id = id;
				this.minArgs = minArgs;
				this.maxArgs = maxArgs;
				this.argTypes = argTypes;
			}

			// Token: 0x06000A16 RID: 2582 RVA: 0x00030758 File Offset: 0x0002F758
			public static void CheckArity(int minArgs, int maxArgs, string name, int numArgs)
			{
				if (minArgs <= numArgs && numArgs <= maxArgs)
				{
					return;
				}
				string text;
				if (minArgs == maxArgs)
				{
					text = "XPath_NArgsExpected";
				}
				else if (maxArgs == minArgs + 1)
				{
					text = "XPath_NOrMArgsExpected";
				}
				else if (numArgs < minArgs)
				{
					text = "XPath_AtLeastNArgsExpected";
				}
				else
				{
					text = "XPath_AtMostMArgsExpected";
				}
				throw new XPathCompileException(text, new string[]
				{
					name,
					minArgs.ToString(CultureInfo.InvariantCulture),
					maxArgs.ToString(CultureInfo.InvariantCulture)
				});
			}

			// Token: 0x06000A17 RID: 2583 RVA: 0x000307CC File Offset: 0x0002F7CC
			public void CastArguments(IList<QilNode> args, string name, XPathQilFactory f)
			{
				XPathBuilder.FunctionInfo<T>.CheckArity(this.minArgs, this.maxArgs, name, args.Count);
				if (this.maxArgs == 2147483647)
				{
					for (int i = 0; i < args.Count; i++)
					{
						args[i] = f.ConvertToType(XmlTypeCode.String, args[i]);
					}
					return;
				}
				for (int j = 0; j < args.Count; j++)
				{
					if (this.argTypes[j] == XmlTypeCode.Node && f.CannotBeNodeSet(args[j]))
					{
						throw new XPathCompileException("XPath_NodeSetArgumentExpected", new string[]
						{
							name,
							(j + 1).ToString(CultureInfo.InvariantCulture)
						});
					}
					args[j] = f.ConvertToType(this.argTypes[j], args[j]);
				}
			}

			// Token: 0x04000687 RID: 1671
			public const int Infinity = 2147483647;

			// Token: 0x04000688 RID: 1672
			public T id;

			// Token: 0x04000689 RID: 1673
			public int minArgs;

			// Token: 0x0400068A RID: 1674
			public int maxArgs;

			// Token: 0x0400068B RID: 1675
			public XmlTypeCode[] argTypes;
		}
	}
}
