using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000135 RID: 309
	internal class DescendantQuery : DescendantBaseQuery
	{
		// Token: 0x060011DA RID: 4570 RVA: 0x0004EB40 File Offset: 0x0004DB40
		internal DescendantQuery(Query qyParent, string Name, string Prefix, XPathNodeType Type, bool matchSelf, bool abbrAxis)
			: base(qyParent, Name, Prefix, Type, matchSelf, abbrAxis)
		{
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004EB51 File Offset: 0x0004DB51
		public DescendantQuery(DescendantQuery other)
			: base(other)
		{
			this.nodeIterator = Query.Clone(other.nodeIterator);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0004EB6B File Offset: 0x0004DB6B
		public override void Reset()
		{
			this.nodeIterator = null;
			base.Reset();
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0004EB7C File Offset: 0x0004DB7C
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				if (this.nodeIterator == null)
				{
					this.position = 0;
					XPathNavigator xpathNavigator = this.qyInput.Advance();
					if (xpathNavigator == null)
					{
						break;
					}
					if (base.NameTest)
					{
						if (base.TypeTest == XPathNodeType.ProcessingInstruction)
						{
							this.nodeIterator = new IteratorFilter(xpathNavigator.SelectDescendants(base.TypeTest, this.matchSelf), base.Name);
						}
						else
						{
							this.nodeIterator = xpathNavigator.SelectDescendants(base.Name, base.Namespace, this.matchSelf);
						}
					}
					else
					{
						this.nodeIterator = xpathNavigator.SelectDescendants(base.TypeTest, this.matchSelf);
					}
				}
				if (this.nodeIterator.MoveNext())
				{
					goto Block_4;
				}
				this.nodeIterator = null;
			}
			return null;
			Block_4:
			this.position++;
			this.currentNode = this.nodeIterator.Current;
			return this.currentNode;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004EC58 File Offset: 0x0004DC58
		public override XPathNodeIterator Clone()
		{
			return new DescendantQuery(this);
		}

		// Token: 0x04000B54 RID: 2900
		private XPathNodeIterator nodeIterator;
	}
}
