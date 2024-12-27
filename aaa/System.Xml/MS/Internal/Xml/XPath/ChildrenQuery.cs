using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012C RID: 300
	internal class ChildrenQuery : BaseAxisQuery
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x0004E166 File Offset: 0x0004D166
		public ChildrenQuery(Query qyInput, string name, string prefix, XPathNodeType type)
			: base(qyInput, name, prefix, type)
		{
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004E17E File Offset: 0x0004D17E
		protected ChildrenQuery(ChildrenQuery other)
			: base(other)
		{
			this.iterator = Query.Clone(other.iterator);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004E1A3 File Offset: 0x0004D1A3
		public override void Reset()
		{
			this.iterator = XPathEmptyIterator.Instance;
			base.Reset();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004E1B8 File Offset: 0x0004D1B8
		public override XPathNavigator Advance()
		{
			while (!this.iterator.MoveNext())
			{
				XPathNavigator xpathNavigator = this.qyInput.Advance();
				if (xpathNavigator == null)
				{
					return null;
				}
				if (base.NameTest)
				{
					if (base.TypeTest == XPathNodeType.ProcessingInstruction)
					{
						this.iterator = new IteratorFilter(xpathNavigator.SelectChildren(base.TypeTest), base.Name);
					}
					else
					{
						this.iterator = xpathNavigator.SelectChildren(base.Name, base.Namespace);
					}
				}
				else
				{
					this.iterator = xpathNavigator.SelectChildren(base.TypeTest);
				}
				this.position = 0;
			}
			this.position++;
			this.currentNode = this.iterator.Current;
			return this.currentNode;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004E270 File Offset: 0x0004D270
		public sealed override XPathNavigator MatchNode(XPathNavigator context)
		{
			if (context == null || !this.matches(context))
			{
				return null;
			}
			XPathNavigator xpathNavigator = context.Clone();
			if (xpathNavigator.NodeType != XPathNodeType.Attribute && xpathNavigator.MoveToParent())
			{
				return this.qyInput.MatchNode(xpathNavigator);
			}
			return null;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004E2B1 File Offset: 0x0004D2B1
		public override XPathNodeIterator Clone()
		{
			return new ChildrenQuery(this);
		}

		// Token: 0x04000B43 RID: 2883
		private XPathNodeIterator iterator = XPathEmptyIterator.Instance;
	}
}
