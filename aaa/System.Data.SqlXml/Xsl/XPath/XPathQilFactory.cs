using System;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000DE RID: 222
	internal class XPathQilFactory : QilPatternFactory
	{
		// Token: 0x06000A3C RID: 2620 RVA: 0x00031A19 File Offset: 0x00030A19
		public XPathQilFactory(QilFactory f, bool debug)
			: base(f, debug)
		{
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00031A23 File Offset: 0x00030A23
		public QilNode Error(string res, QilNode args)
		{
			return base.Error(this.InvokeFormatMessage(base.String(res), args));
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00031A39 File Offset: 0x00030A39
		public QilNode Error(ISourceLineInfo lineInfo, string res, params string[] args)
		{
			return base.Error(base.String(XslLoadException.CreateMessage(lineInfo, res, args)));
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00031A50 File Offset: 0x00030A50
		public QilIterator FirstNode(QilNode n)
		{
			QilIterator qilIterator = base.For(base.DocOrderDistinct(n));
			return base.For(base.Filter(qilIterator, base.Eq(base.PositionOf(qilIterator), base.Int32(1))));
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00031A8C File Offset: 0x00030A8C
		public bool IsAnyType(QilNode n)
		{
			XmlQueryType xmlType = n.XmlType;
			return !xmlType.IsStrict && !xmlType.IsNode;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00031AB6 File Offset: 0x00030AB6
		[Conditional("DEBUG")]
		public void CheckAny(QilNode n)
		{
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00031AB8 File Offset: 0x00030AB8
		[Conditional("DEBUG")]
		public void CheckNode(QilNode n)
		{
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00031ABA File Offset: 0x00030ABA
		[Conditional("DEBUG")]
		public void CheckNodeSet(QilNode n)
		{
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00031ABC File Offset: 0x00030ABC
		[Conditional("DEBUG")]
		public void CheckNodeNotRtf(QilNode n)
		{
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00031ABE File Offset: 0x00030ABE
		[Conditional("DEBUG")]
		public void CheckString(QilNode n)
		{
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00031AC0 File Offset: 0x00030AC0
		[Conditional("DEBUG")]
		public void CheckStringS(QilNode n)
		{
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00031AC2 File Offset: 0x00030AC2
		[Conditional("DEBUG")]
		public void CheckDouble(QilNode n)
		{
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00031AC4 File Offset: 0x00030AC4
		[Conditional("DEBUG")]
		public void CheckBool(QilNode n)
		{
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00031AC8 File Offset: 0x00030AC8
		public bool CannotBeNodeSet(QilNode n)
		{
			XmlQueryType xmlType = n.XmlType;
			return xmlType.IsAtomicValue && !xmlType.IsEmpty && !(n is QilIterator);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00031AFC File Offset: 0x00030AFC
		public QilNode SafeDocOrderDistinct(QilNode n)
		{
			XmlQueryType xmlType = n.XmlType;
			if (xmlType.MaybeMany)
			{
				if (xmlType.IsNode && xmlType.IsNotRtf)
				{
					return base.DocOrderDistinct(n);
				}
				if (!xmlType.IsAtomicValue)
				{
					QilIterator qilIterator;
					return base.Loop(qilIterator = base.Let(n), base.Conditional(base.Gt(base.Length(qilIterator), base.Int32(1)), base.DocOrderDistinct(base.TypeAssert(qilIterator, XmlQueryTypeFactory.NodeNotRtfS)), qilIterator));
				}
			}
			return n;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00031B78 File Offset: 0x00030B78
		public QilNode InvokeFormatMessage(QilNode res, QilNode args)
		{
			return base.XsltInvokeEarlyBound(base.QName("format-message"), XsltMethods.FormatMessage, XmlQueryTypeFactory.StringX, new QilNode[] { res, args });
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00031BB0 File Offset: 0x00030BB0
		public QilNode InvokeEqualityOperator(QilNodeType op, QilNode left, QilNode right)
		{
			left = base.TypeAssert(left, XmlQueryTypeFactory.ItemS);
			right = base.TypeAssert(right, XmlQueryTypeFactory.ItemS);
			double num;
			if (op == QilNodeType.Eq)
			{
				num = 0.0;
			}
			else
			{
				num = 1.0;
			}
			return base.XsltInvokeEarlyBound(base.QName("EqualityOperator"), XsltMethods.EqualityOperator, XmlQueryTypeFactory.BooleanX, new QilNode[]
			{
				base.Double(num),
				left,
				right
			});
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00031C2C File Offset: 0x00030C2C
		public QilNode InvokeRelationalOperator(QilNodeType op, QilNode left, QilNode right)
		{
			left = base.TypeAssert(left, XmlQueryTypeFactory.ItemS);
			right = base.TypeAssert(right, XmlQueryTypeFactory.ItemS);
			double num;
			switch (op)
			{
			case QilNodeType.Gt:
				num = 4.0;
				goto IL_0067;
			case QilNodeType.Lt:
				num = 2.0;
				goto IL_0067;
			case QilNodeType.Le:
				num = 3.0;
				goto IL_0067;
			}
			num = 5.0;
			IL_0067:
			return base.XsltInvokeEarlyBound(base.QName("RelationalOperator"), XsltMethods.RelationalOperator, XmlQueryTypeFactory.BooleanX, new QilNode[]
			{
				base.Double(num),
				left,
				right
			});
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00031CD5 File Offset: 0x00030CD5
		[Conditional("DEBUG")]
		private void ExpectAny(QilNode n)
		{
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00031CD8 File Offset: 0x00030CD8
		public QilNode ConvertToType(XmlTypeCode requiredType, QilNode n)
		{
			switch (requiredType)
			{
			case XmlTypeCode.Item:
				return n;
			case XmlTypeCode.Node:
				return this.EnsureNodeSet(n);
			default:
				switch (requiredType)
				{
				case XmlTypeCode.String:
					return this.ConvertToString(n);
				case XmlTypeCode.Boolean:
					return this.ConvertToBoolean(n);
				case XmlTypeCode.Double:
					return this.ConvertToNumber(n);
				}
				return null;
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00031D3C File Offset: 0x00030D3C
		public QilNode ConvertToString(QilNode n)
		{
			switch (n.XmlType.TypeCode)
			{
			case XmlTypeCode.String:
				return n;
			case XmlTypeCode.Boolean:
				if (n.NodeType == QilNodeType.True)
				{
					return base.String("true");
				}
				if (n.NodeType != QilNodeType.False)
				{
					return base.Conditional(n, base.String("true"), base.String("false"));
				}
				return base.String("false");
			case XmlTypeCode.Double:
				if (n.NodeType != QilNodeType.LiteralDouble)
				{
					return base.XsltConvert(n, XmlQueryTypeFactory.StringX);
				}
				return base.String(XPathConvert.DoubleToString((QilLiteral)n));
			}
			if (n.XmlType.IsNode)
			{
				return base.XPathNodeValue(this.SafeDocOrderDistinct(n));
			}
			return base.XsltConvert(n, XmlQueryTypeFactory.StringX);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00031E18 File Offset: 0x00030E18
		public QilNode ConvertToBoolean(QilNode n)
		{
			switch (n.XmlType.TypeCode)
			{
			case XmlTypeCode.String:
				if (n.NodeType != QilNodeType.LiteralString)
				{
					return base.Ne(base.StrLength(n), base.Int32(0));
				}
				return base.Boolean(((QilLiteral)n).Length != 0);
			case XmlTypeCode.Boolean:
				return n;
			case XmlTypeCode.Double:
				if (n.NodeType != QilNodeType.LiteralDouble)
				{
					QilIterator qilIterator;
					return base.Loop(qilIterator = base.Let(n), base.Or(base.Lt(qilIterator, base.Double(0.0)), base.Lt(base.Double(0.0), qilIterator)));
				}
				return base.Boolean((QilLiteral)n < 0.0 || 0.0 < (QilLiteral)n);
			}
			if (n.XmlType.IsNode)
			{
				return base.Not(base.IsEmpty(n));
			}
			return base.XsltConvert(n, XmlQueryTypeFactory.BooleanX);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00031F3C File Offset: 0x00030F3C
		public QilNode ConvertToNumber(QilNode n)
		{
			switch (n.XmlType.TypeCode)
			{
			case XmlTypeCode.String:
				return base.XsltConvert(n, XmlQueryTypeFactory.DoubleX);
			case XmlTypeCode.Boolean:
				if (n.NodeType == QilNodeType.True)
				{
					return base.Double(1.0);
				}
				if (n.NodeType != QilNodeType.False)
				{
					return base.Conditional(n, base.Double(1.0), base.Double(0.0));
				}
				return base.Double(0.0);
			case XmlTypeCode.Double:
				return n;
			}
			if (n.XmlType.IsNode)
			{
				return base.XsltConvert(base.XPathNodeValue(this.SafeDocOrderDistinct(n)), XmlQueryTypeFactory.DoubleX);
			}
			return base.XsltConvert(n, XmlQueryTypeFactory.DoubleX);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003200F File Offset: 0x0003100F
		public QilNode ConvertToNode(QilNode n)
		{
			if (n.XmlType.IsNode && n.XmlType.IsNotRtf && n.XmlType.IsSingleton)
			{
				return n;
			}
			return base.XsltConvert(n, XmlQueryTypeFactory.NodeNotRtf);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00032046 File Offset: 0x00031046
		public QilNode ConvertToNodeSet(QilNode n)
		{
			if (n.XmlType.IsNode && n.XmlType.IsNotRtf)
			{
				return n;
			}
			return base.XsltConvert(n, XmlQueryTypeFactory.NodeNotRtfS);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00032070 File Offset: 0x00031070
		public QilNode EnsureNodeSet(QilNode n)
		{
			if (n.XmlType.IsNode && n.XmlType.IsNotRtf)
			{
				return n;
			}
			if (this.CannotBeNodeSet(n))
			{
				throw new XPathCompileException("XPath_NodeSetExpected", new string[0]);
			}
			return this.InvokeEnsureNodeSet(n);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000320B0 File Offset: 0x000310B0
		public QilNode InvokeEnsureNodeSet(QilNode n)
		{
			return base.XsltInvokeEarlyBound(base.QName("ensure-node-set"), XsltMethods.EnsureNodeSet, XmlQueryTypeFactory.NodeDodS, new QilNode[] { n });
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000320E4 File Offset: 0x000310E4
		public QilNode Id(QilNode context, QilNode id)
		{
			if (id.XmlType.IsSingleton)
			{
				return base.Deref(context, this.ConvertToString(id));
			}
			QilIterator qilIterator;
			return base.Loop(qilIterator = base.For(id), base.Deref(context, this.ConvertToString(qilIterator)));
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003212C File Offset: 0x0003112C
		public QilNode InvokeStartsWith(QilNode str1, QilNode str2)
		{
			return base.XsltInvokeEarlyBound(base.QName("starts-with"), XsltMethods.StartsWith, XmlQueryTypeFactory.BooleanX, new QilNode[] { str1, str2 });
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00032164 File Offset: 0x00031164
		public QilNode InvokeContains(QilNode str1, QilNode str2)
		{
			return base.XsltInvokeEarlyBound(base.QName("contains"), XsltMethods.Contains, XmlQueryTypeFactory.BooleanX, new QilNode[] { str1, str2 });
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0003219C File Offset: 0x0003119C
		public QilNode InvokeSubstringBefore(QilNode str1, QilNode str2)
		{
			return base.XsltInvokeEarlyBound(base.QName("substring-before"), XsltMethods.SubstringBefore, XmlQueryTypeFactory.StringX, new QilNode[] { str1, str2 });
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000321D4 File Offset: 0x000311D4
		public QilNode InvokeSubstringAfter(QilNode str1, QilNode str2)
		{
			return base.XsltInvokeEarlyBound(base.QName("substring-after"), XsltMethods.SubstringAfter, XmlQueryTypeFactory.StringX, new QilNode[] { str1, str2 });
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0003220C File Offset: 0x0003120C
		public QilNode InvokeSubstring(QilNode str, QilNode start)
		{
			return base.XsltInvokeEarlyBound(base.QName("substring"), XsltMethods.Substring2, XmlQueryTypeFactory.StringX, new QilNode[] { str, start });
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00032244 File Offset: 0x00031244
		public QilNode InvokeSubstring(QilNode str, QilNode start, QilNode length)
		{
			return base.XsltInvokeEarlyBound(base.QName("substring"), XsltMethods.Substring3, XmlQueryTypeFactory.StringX, new QilNode[] { str, start, length });
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00032280 File Offset: 0x00031280
		public QilNode InvokeNormalizeSpace(QilNode str)
		{
			return base.XsltInvokeEarlyBound(base.QName("normalize-space"), XsltMethods.NormalizeSpace, XmlQueryTypeFactory.StringX, new QilNode[] { str });
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000322B4 File Offset: 0x000312B4
		public QilNode InvokeTranslate(QilNode str1, QilNode str2, QilNode str3)
		{
			return base.XsltInvokeEarlyBound(base.QName("translate"), XsltMethods.Translate, XmlQueryTypeFactory.StringX, new QilNode[] { str1, str2, str3 });
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000322F0 File Offset: 0x000312F0
		public QilNode InvokeLang(QilNode lang, QilNode context)
		{
			return base.XsltInvokeEarlyBound(base.QName("lang"), XsltMethods.Lang, XmlQueryTypeFactory.BooleanX, new QilNode[] { lang, context });
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00032328 File Offset: 0x00031328
		public QilNode InvokeFloor(QilNode value)
		{
			return base.XsltInvokeEarlyBound(base.QName("floor"), XsltMethods.Floor, XmlQueryTypeFactory.DoubleX, new QilNode[] { value });
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0003235C File Offset: 0x0003135C
		public QilNode InvokeCeiling(QilNode value)
		{
			return base.XsltInvokeEarlyBound(base.QName("ceiling"), XsltMethods.Ceiling, XmlQueryTypeFactory.DoubleX, new QilNode[] { value });
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00032390 File Offset: 0x00031390
		public QilNode InvokeRound(QilNode value)
		{
			return base.XsltInvokeEarlyBound(base.QName("round"), XsltMethods.Round, XmlQueryTypeFactory.DoubleX, new QilNode[] { value });
		}
	}
}
