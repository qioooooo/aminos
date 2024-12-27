using System;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F2 RID: 242
	internal class KeyMatchBuilder : XPathBuilder, XPathPatternParser.IPatternBuilder, IXPathBuilder<QilNode>
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x00034415 File Offset: 0x00033415
		public KeyMatchBuilder(IXPathEnvironment env)
			: base(env)
		{
			this.convertor = new KeyMatchBuilder.PathConvertor(env.Factory);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0003442F File Offset: 0x0003342F
		public override void StartBuild()
		{
			if (this.depth == 0)
			{
				base.StartBuild();
			}
			this.depth++;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00034450 File Offset: 0x00033450
		public override QilNode EndBuild(QilNode result)
		{
			this.depth--;
			if (result == null)
			{
				return base.EndBuild(result);
			}
			if (this.depth == 0)
			{
				result = this.convertor.ConvertReletive2Absolute(result, this.fixupCurrent);
				result = base.EndBuild(result);
			}
			return result;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0003449C File Offset: 0x0003349C
		public virtual IXPathBuilder<QilNode> GetPredicateBuilder(QilNode ctx)
		{
			return this;
		}

		// Token: 0x0400073D RID: 1853
		private int depth;

		// Token: 0x0400073E RID: 1854
		private KeyMatchBuilder.PathConvertor convertor;

		// Token: 0x020000F3 RID: 243
		internal class PathConvertor : QilReplaceVisitor
		{
			// Token: 0x06000ADA RID: 2778 RVA: 0x0003449F File Offset: 0x0003349F
			public PathConvertor(XPathQilFactory f)
				: base(f.BaseFactory)
			{
				this.f = f;
			}

			// Token: 0x06000ADB RID: 2779 RVA: 0x000344B4 File Offset: 0x000334B4
			public QilNode ConvertReletive2Absolute(QilNode node, QilNode fixup)
			{
				QilDepthChecker.Check(node);
				this.fixup = fixup;
				return this.Visit(node);
			}

			// Token: 0x06000ADC RID: 2780 RVA: 0x000344CA File Offset: 0x000334CA
			protected override QilNode Visit(QilNode n)
			{
				if (n.NodeType == QilNodeType.Union || n.NodeType == QilNodeType.DocOrderDistinct || n.NodeType == QilNodeType.Filter || n.NodeType == QilNodeType.Loop)
				{
					return base.Visit(n);
				}
				return n;
			}

			// Token: 0x06000ADD RID: 2781 RVA: 0x00034500 File Offset: 0x00033500
			protected override QilNode VisitLoop(QilLoop n)
			{
				if (n.Variable.Binding.NodeType == QilNodeType.Root || n.Variable.Binding.NodeType == QilNodeType.Deref)
				{
					return n;
				}
				if (n.Variable.Binding.NodeType == QilNodeType.Content)
				{
					QilUnary qilUnary = (QilUnary)n.Variable.Binding;
					QilIterator qilIterator = this.f.For(this.f.DescendantOrSelf(this.f.Root(this.fixup)));
					qilUnary.Child = qilIterator;
					n.Variable.Binding = this.f.Loop(qilIterator, qilUnary);
					return n;
				}
				n.Variable.Binding = this.Visit(n.Variable.Binding);
				return n;
			}

			// Token: 0x06000ADE RID: 2782 RVA: 0x000345C2 File Offset: 0x000335C2
			protected override QilNode VisitFilter(QilLoop n)
			{
				return this.VisitLoop(n);
			}

			// Token: 0x0400073F RID: 1855
			private new XPathQilFactory f;

			// Token: 0x04000740 RID: 1856
			private QilNode fixup;
		}
	}
}
