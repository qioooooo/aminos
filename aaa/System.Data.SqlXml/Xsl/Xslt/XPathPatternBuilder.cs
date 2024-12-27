using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000105 RID: 261
	internal class XPathPatternBuilder : XPathPatternParser.IPatternBuilder, IXPathBuilder<QilNode>
	{
		// Token: 0x06000BAF RID: 2991 RVA: 0x0003CA38 File Offset: 0x0003BA38
		public XPathPatternBuilder(IXPathEnvironment environment)
		{
			this.environment = environment;
			this.f = environment.Factory;
			this.predicateEnvironment = new XPathPatternBuilder.XPathPredicateEnvironment(environment);
			this.predicateBuilder = new XPathBuilder(this.predicateEnvironment);
			this.fixupNode = this.f.Unknown(XmlQueryTypeFactory.NodeNotRtfS);
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0003CA91 File Offset: 0x0003BA91
		public QilNode FixupNode
		{
			get
			{
				return this.fixupNode;
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0003CA99 File Offset: 0x0003BA99
		public virtual void StartBuild()
		{
			this.inTheBuild = true;
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0003CAA2 File Offset: 0x0003BAA2
		[Conditional("DEBUG")]
		public void AssertFilter(QilLoop filter)
		{
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003CAA4 File Offset: 0x0003BAA4
		private void FixupFilterBinding(QilLoop filter, QilNode newBinding)
		{
			filter.Variable.Binding = newBinding;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0003CAB2 File Offset: 0x0003BAB2
		public virtual QilNode EndBuild(QilNode result)
		{
			this.inTheBuild = false;
			return result;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0003CABE File Offset: 0x0003BABE
		public QilNode Operator(XPathOperator op, QilNode left, QilNode right)
		{
			if (left.NodeType == QilNodeType.Sequence)
			{
				((QilList)left).Add(right);
				return left;
			}
			return this.f.Sequence(left, right);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0003CAE8 File Offset: 0x0003BAE8
		private static QilLoop BuildAxisFilter(QilPatternFactory f, QilIterator itr, XPathAxis xpathAxis, XPathNodeType nodeType, string name, string nsUri)
		{
			QilNode qilNode = ((name != null && nsUri != null) ? f.Eq(f.NameOf(itr), f.QName(name, nsUri)) : ((nsUri != null) ? f.Eq(f.NamespaceUriOf(itr), f.String(nsUri)) : ((name != null) ? f.Eq(f.LocalNameOf(itr), f.String(name)) : f.True())));
			XmlNodeKindFlags xmlNodeKindFlags = XPathBuilder.AxisTypeMask(itr.XmlType.NodeKinds, nodeType, xpathAxis);
			QilNode qilNode2 = ((xmlNodeKindFlags == XmlNodeKindFlags.None) ? f.False() : ((xmlNodeKindFlags == itr.XmlType.NodeKinds) ? f.True() : f.IsType(itr, XmlQueryTypeFactory.NodeChoice(xmlNodeKindFlags))));
			QilLoop qilLoop = f.BaseFactory.Filter(itr, f.And(qilNode2, qilNode));
			qilLoop.XmlType = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeChoice(xmlNodeKindFlags), qilLoop.XmlType.Cardinality);
			return qilLoop;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0003CBC8 File Offset: 0x0003BBC8
		public QilNode Axis(XPathAxis xpathAxis, XPathNodeType nodeType, string prefix, string name)
		{
			if (xpathAxis != XPathAxis.DescendantOrSelf)
			{
				QilLoop qilLoop;
				double num;
				if (xpathAxis != XPathAxis.Root)
				{
					string text = ((prefix == null) ? null : this.environment.ResolvePrefix(prefix));
					qilLoop = XPathPatternBuilder.BuildAxisFilter(this.f, this.f.For(this.fixupNode), xpathAxis, nodeType, name, text);
					switch (nodeType)
					{
					case XPathNodeType.Element:
					case XPathNodeType.Attribute:
						if (name != null)
						{
							num = 0.0;
						}
						else if (prefix != null)
						{
							num = -0.25;
						}
						else
						{
							num = -0.5;
						}
						break;
					default:
						if (nodeType != XPathNodeType.ProcessingInstruction)
						{
							num = -0.5;
						}
						else
						{
							num = ((name != null) ? 0.0 : (-0.5));
						}
						break;
					}
				}
				else
				{
					QilIterator qilIterator;
					qilLoop = this.f.BaseFactory.Filter(qilIterator = this.f.For(this.fixupNode), this.f.IsType(qilIterator, XmlQueryTypeFactory.Document));
					num = 0.5;
				}
				XPathPatternBuilder.SetPriority(qilLoop, num);
				XPathPatternBuilder.SetLastParent(qilLoop, qilLoop);
				return qilLoop;
			}
			return this.f.Nop(this.fixupNode);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0003CCEC File Offset: 0x0003BCEC
		public QilNode JoinStep(QilNode left, QilNode right)
		{
			if (left.NodeType == QilNodeType.Nop)
			{
				QilUnary qilUnary = (QilUnary)left;
				qilUnary.Child = right;
				return qilUnary;
			}
			XPathPatternBuilder.CleanAnnotation(left);
			QilLoop qilLoop = (QilLoop)left;
			bool flag = false;
			if (right.NodeType == QilNodeType.Nop)
			{
				flag = true;
				QilUnary qilUnary2 = (QilUnary)right;
				right = qilUnary2.Child;
			}
			QilLoop lastParent = XPathPatternBuilder.GetLastParent(right);
			this.FixupFilterBinding(qilLoop, flag ? this.f.Ancestor(lastParent.Variable) : this.f.Parent(lastParent.Variable));
			lastParent.Body = this.f.And(lastParent.Body, this.f.Not(this.f.IsEmpty(qilLoop)));
			XPathPatternBuilder.SetPriority(right, 0.5);
			XPathPatternBuilder.SetLastParent(right, qilLoop);
			return right;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0003CDBC File Offset: 0x0003BDBC
		public QilNode Predicate(QilNode node, QilNode condition, bool isReverseStep)
		{
			QilLoop qilLoop = (QilLoop)node;
			if (condition.XmlType.TypeCode == XmlTypeCode.Double)
			{
				this.predicateEnvironment.SetContext(qilLoop);
				condition = this.f.Eq(condition, this.predicateEnvironment.GetPosition());
			}
			else
			{
				condition = this.f.ConvertToBoolean(condition);
			}
			qilLoop.Body = this.f.And(qilLoop.Body, condition);
			XPathPatternBuilder.SetPriority(node, 0.5);
			return node;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003CE3C File Offset: 0x0003BE3C
		public QilNode Function(string prefix, string name, IList<QilNode> args)
		{
			QilIterator qilIterator = this.f.For(this.fixupNode);
			QilNode qilNode;
			if (name == "id")
			{
				qilNode = this.f.Id(qilIterator, args[0]);
			}
			else
			{
				qilNode = this.environment.ResolveFunction(prefix, name, args, new XPathPatternBuilder.XsltFunctionFocus(qilIterator));
			}
			QilIterator qilIterator2;
			QilLoop qilLoop = this.f.BaseFactory.Filter(qilIterator, this.f.Not(this.f.IsEmpty(this.f.Filter(qilIterator2 = this.f.For(qilNode), this.f.Is(qilIterator2, qilIterator)))));
			XPathPatternBuilder.SetPriority(qilLoop, 0.5);
			XPathPatternBuilder.SetLastParent(qilLoop, qilLoop);
			return qilLoop;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0003CEF8 File Offset: 0x0003BEF8
		public QilNode String(string value)
		{
			return this.f.String(value);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0003CF06 File Offset: 0x0003BF06
		public QilNode Number(double value)
		{
			return this.UnexpectedToken("Literal number");
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0003CF13 File Offset: 0x0003BF13
		public QilNode Variable(string prefix, string name)
		{
			return this.UnexpectedToken("Variable");
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0003CF20 File Offset: 0x0003BF20
		private QilNode UnexpectedToken(string tokenName)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "Internal Error: {0} is not allowed in XSLT pattern outside of predicate.", new object[] { tokenName });
			throw new Exception(text);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0003CF50 File Offset: 0x0003BF50
		public static void SetPriority(QilNode node, double priority)
		{
			XPathPatternBuilder.Annotation annotation = ((XPathPatternBuilder.Annotation)node.Annotation) ?? new XPathPatternBuilder.Annotation();
			annotation.Priority = priority;
			node.Annotation = annotation;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0003CF80 File Offset: 0x0003BF80
		public static double GetPriority(QilNode node)
		{
			return ((XPathPatternBuilder.Annotation)node.Annotation).Priority;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0003CF94 File Offset: 0x0003BF94
		private static void SetLastParent(QilNode node, QilLoop parent)
		{
			XPathPatternBuilder.Annotation annotation = ((XPathPatternBuilder.Annotation)node.Annotation) ?? new XPathPatternBuilder.Annotation();
			annotation.Parent = parent;
			node.Annotation = annotation;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0003CFC4 File Offset: 0x0003BFC4
		private static QilLoop GetLastParent(QilNode node)
		{
			return ((XPathPatternBuilder.Annotation)node.Annotation).Parent;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0003CFD6 File Offset: 0x0003BFD6
		public static void CleanAnnotation(QilNode node)
		{
			node.Annotation = null;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0003CFE0 File Offset: 0x0003BFE0
		public IXPathBuilder<QilNode> GetPredicateBuilder(QilNode ctx)
		{
			QilLoop qilLoop = (QilLoop)ctx;
			this.predicateEnvironment.SetContext(qilLoop);
			return this.predicateBuilder;
		}

		// Token: 0x0400081A RID: 2074
		private XPathPatternBuilder.XPathPredicateEnvironment predicateEnvironment;

		// Token: 0x0400081B RID: 2075
		private XPathBuilder predicateBuilder;

		// Token: 0x0400081C RID: 2076
		private bool inTheBuild;

		// Token: 0x0400081D RID: 2077
		private XPathQilFactory f;

		// Token: 0x0400081E RID: 2078
		private QilNode fixupNode;

		// Token: 0x0400081F RID: 2079
		private IXPathEnvironment environment;

		// Token: 0x02000106 RID: 262
		private class Annotation
		{
			// Token: 0x04000820 RID: 2080
			public double Priority;

			// Token: 0x04000821 RID: 2081
			public QilLoop Parent;
		}

		// Token: 0x02000107 RID: 263
		private class XPathPredicateEnvironment : IXPathEnvironment, IFocus
		{
			// Token: 0x06000BC6 RID: 3014 RVA: 0x0003D00E File Offset: 0x0003C00E
			public XPathPredicateEnvironment(IXPathEnvironment baseEnvironment)
			{
				this.baseEnvironment = baseEnvironment;
				this.f = baseEnvironment.Factory;
				this.cloner = new XPathPatternBuilder.XPathPredicateEnvironment.Cloner(this.f.BaseFactory);
			}

			// Token: 0x06000BC7 RID: 3015 RVA: 0x0003D03F File Offset: 0x0003C03F
			public void SetContext(QilLoop filter)
			{
				this.baseContext = filter;
			}

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003D048 File Offset: 0x0003C048
			public XPathQilFactory Factory
			{
				get
				{
					return this.f;
				}
			}

			// Token: 0x06000BC9 RID: 3017 RVA: 0x0003D050 File Offset: 0x0003C050
			public QilNode ResolveVariable(string prefix, string name)
			{
				return this.baseEnvironment.ResolveVariable(prefix, name);
			}

			// Token: 0x06000BCA RID: 3018 RVA: 0x0003D05F File Offset: 0x0003C05F
			public QilNode ResolveFunction(string prefix, string name, IList<QilNode> args, IFocus env)
			{
				return this.baseEnvironment.ResolveFunction(prefix, name, args, env);
			}

			// Token: 0x06000BCB RID: 3019 RVA: 0x0003D071 File Offset: 0x0003C071
			public string ResolvePrefix(string prefix)
			{
				return this.baseEnvironment.ResolvePrefix(prefix);
			}

			// Token: 0x06000BCC RID: 3020 RVA: 0x0003D07F File Offset: 0x0003C07F
			public QilNode GetCurrent()
			{
				return this.baseContext.Variable;
			}

			// Token: 0x06000BCD RID: 3021 RVA: 0x0003D08C File Offset: 0x0003C08C
			public QilNode GetPosition()
			{
				QilLoop qilLoop = (QilLoop)this.cloner.Clone(this.baseContext);
				XmlNodeKindFlags nodeKinds = this.baseContext.XmlType.NodeKinds;
				if (nodeKinds == XmlNodeKindFlags.Attribute)
				{
					QilIterator qilIterator = this.f.For(this.f.Parent(this.GetCurrent()));
					qilLoop.Variable.Binding = this.f.Content(qilIterator);
					qilLoop.Body = this.f.And(qilLoop.Body, this.f.Before(qilLoop.Variable, this.GetCurrent()));
					qilLoop = this.f.BaseFactory.Loop(qilIterator, qilLoop);
				}
				else
				{
					qilLoop.Variable.Binding = this.f.PrecedingSibling(this.GetCurrent());
				}
				return this.f.Add(this.f.Double(1.0), this.f.XsltConvert(this.f.Length(qilLoop), XmlQueryTypeFactory.DoubleX));
			}

			// Token: 0x06000BCE RID: 3022 RVA: 0x0003D194 File Offset: 0x0003C194
			public QilNode GetLast()
			{
				QilLoop qilLoop = (QilLoop)this.cloner.Clone(this.baseContext);
				QilIterator qilIterator = this.f.For(this.f.Parent(this.GetCurrent()));
				qilLoop.Variable.Binding = this.f.Content(qilIterator);
				return this.f.XsltConvert(this.f.Length(this.f.Loop(qilIterator, qilLoop)), XmlQueryTypeFactory.DoubleX);
			}

			// Token: 0x04000822 RID: 2082
			private IXPathEnvironment baseEnvironment;

			// Token: 0x04000823 RID: 2083
			private QilLoop baseContext;

			// Token: 0x04000824 RID: 2084
			private XPathQilFactory f;

			// Token: 0x04000825 RID: 2085
			private XPathPatternBuilder.XPathPredicateEnvironment.Cloner cloner;

			// Token: 0x02000108 RID: 264
			internal class Cloner : QilCloneVisitor
			{
				// Token: 0x06000BCF RID: 3023 RVA: 0x0003D214 File Offset: 0x0003C214
				public Cloner(QilFactory f)
					: base(f)
				{
				}

				// Token: 0x06000BD0 RID: 3024 RVA: 0x0003D21D File Offset: 0x0003C21D
				protected override QilNode VisitUnknown(QilNode n)
				{
					return n;
				}
			}
		}

		// Token: 0x02000109 RID: 265
		private class XsltFunctionFocus : IFocus
		{
			// Token: 0x06000BD1 RID: 3025 RVA: 0x0003D220 File Offset: 0x0003C220
			public XsltFunctionFocus(QilIterator current)
			{
				this.current = current;
			}

			// Token: 0x06000BD2 RID: 3026 RVA: 0x0003D22F File Offset: 0x0003C22F
			public QilNode GetCurrent()
			{
				return this.current;
			}

			// Token: 0x06000BD3 RID: 3027 RVA: 0x0003D237 File Offset: 0x0003C237
			public QilNode GetPosition()
			{
				return null;
			}

			// Token: 0x06000BD4 RID: 3028 RVA: 0x0003D23A File Offset: 0x0003C23A
			public QilNode GetLast()
			{
				return null;
			}

			// Token: 0x04000826 RID: 2086
			private QilIterator current;
		}
	}
}
