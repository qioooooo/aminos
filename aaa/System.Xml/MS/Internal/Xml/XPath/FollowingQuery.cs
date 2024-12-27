using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013D RID: 317
	internal sealed class FollowingQuery : BaseAxisQuery
	{
		// Token: 0x06001217 RID: 4631 RVA: 0x0004F7D0 File Offset: 0x0004E7D0
		public FollowingQuery(Query qyInput, string name, string prefix, XPathNodeType typeTest)
			: base(qyInput, name, prefix, typeTest)
		{
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0004F7DD File Offset: 0x0004E7DD
		private FollowingQuery(FollowingQuery other)
			: base(other)
		{
			this.input = Query.Clone(other.input);
			this.iterator = Query.Clone(other.iterator);
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0004F808 File Offset: 0x0004E808
		public override void Reset()
		{
			this.iterator = null;
			base.Reset();
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0004F818 File Offset: 0x0004E818
		public override XPathNavigator Advance()
		{
			if (this.iterator == null)
			{
				this.input = this.qyInput.Advance();
				if (this.input == null)
				{
					return null;
				}
				XPathNavigator xpathNavigator;
				do
				{
					xpathNavigator = this.input.Clone();
					this.input = this.qyInput.Advance();
				}
				while (xpathNavigator.IsDescendant(this.input));
				this.input = xpathNavigator;
				this.iterator = XPathEmptyIterator.Instance;
			}
			while (!this.iterator.MoveNext())
			{
				bool flag;
				if (this.input.NodeType == XPathNodeType.Attribute || this.input.NodeType == XPathNodeType.Namespace)
				{
					this.input.MoveToParent();
					flag = false;
				}
				else
				{
					while (!this.input.MoveToNext())
					{
						if (!this.input.MoveToParent())
						{
							return null;
						}
					}
					flag = true;
				}
				if (base.NameTest)
				{
					this.iterator = this.input.SelectDescendants(base.Name, base.Namespace, flag);
				}
				else
				{
					this.iterator = this.input.SelectDescendants(base.TypeTest, flag);
				}
			}
			this.position++;
			this.currentNode = this.iterator.Current;
			return this.currentNode;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0004F94C File Offset: 0x0004E94C
		public override XPathNodeIterator Clone()
		{
			return new FollowingQuery(this);
		}

		// Token: 0x04000B60 RID: 2912
		private XPathNavigator input;

		// Token: 0x04000B61 RID: 2913
		private XPathNodeIterator iterator;
	}
}
