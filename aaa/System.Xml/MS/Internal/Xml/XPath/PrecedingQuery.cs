using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000154 RID: 340
	internal sealed class PrecedingQuery : BaseAxisQuery
	{
		// Token: 0x060012B8 RID: 4792 RVA: 0x00051385 File Offset: 0x00050385
		public PrecedingQuery(Query qyInput, string name, string prefix, XPathNodeType typeTest)
			: base(qyInput, name, prefix, typeTest)
		{
			this.ancestorStk = new ClonableStack<XPathNavigator>();
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0005139D File Offset: 0x0005039D
		private PrecedingQuery(PrecedingQuery other)
			: base(other)
		{
			this.workIterator = Query.Clone(other.workIterator);
			this.ancestorStk = other.ancestorStk.Clone();
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000513C8 File Offset: 0x000503C8
		public override void Reset()
		{
			this.workIterator = null;
			this.ancestorStk.Clear();
			base.Reset();
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000513E4 File Offset: 0x000503E4
		public override XPathNavigator Advance()
		{
			if (this.workIterator == null)
			{
				XPathNavigator xpathNavigator = this.qyInput.Advance();
				if (xpathNavigator == null)
				{
					return null;
				}
				XPathNavigator xpathNavigator2 = xpathNavigator.Clone();
				do
				{
					xpathNavigator2.MoveTo(xpathNavigator);
				}
				while ((xpathNavigator = this.qyInput.Advance()) != null);
				if (xpathNavigator2.NodeType == XPathNodeType.Attribute || xpathNavigator2.NodeType == XPathNodeType.Namespace)
				{
					xpathNavigator2.MoveToParent();
				}
				do
				{
					this.ancestorStk.Push(xpathNavigator2.Clone());
				}
				while (xpathNavigator2.MoveToParent());
				this.workIterator = xpathNavigator2.SelectDescendants(XPathNodeType.All, true);
			}
			while (this.workIterator.MoveNext())
			{
				this.currentNode = this.workIterator.Current;
				if (this.currentNode.IsSamePosition(this.ancestorStk.Peek()))
				{
					this.ancestorStk.Pop();
					if (this.ancestorStk.Count == 0)
					{
						this.currentNode = null;
						this.workIterator = null;
						return null;
					}
				}
				else if (this.matches(this.currentNode))
				{
					this.position++;
					return this.currentNode;
				}
			}
			return null;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000514F4 File Offset: 0x000504F4
		public override XPathNodeIterator Clone()
		{
			return new PrecedingQuery(this);
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x000514FC File Offset: 0x000504FC
		public override QueryProps Properties
		{
			get
			{
				return base.Properties | QueryProps.Reverse;
			}
		}

		// Token: 0x04000BBA RID: 3002
		private XPathNodeIterator workIterator;

		// Token: 0x04000BBB RID: 3003
		private ClonableStack<XPathNavigator> ancestorStk;
	}
}
