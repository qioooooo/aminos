using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000125 RID: 293
	internal sealed class AttributeQuery : BaseAxisQuery
	{
		// Token: 0x06001164 RID: 4452 RVA: 0x0004DB28 File Offset: 0x0004CB28
		public AttributeQuery(Query qyParent, string Name, string Prefix, XPathNodeType Type)
			: base(qyParent, Name, Prefix, Type)
		{
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004DB35 File Offset: 0x0004CB35
		private AttributeQuery(AttributeQuery other)
			: base(other)
		{
			this.onAttribute = other.onAttribute;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0004DB4A File Offset: 0x0004CB4A
		public override void Reset()
		{
			this.onAttribute = false;
			base.Reset();
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0004DB5C File Offset: 0x0004CB5C
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				if (!this.onAttribute)
				{
					this.currentNode = this.qyInput.Advance();
					if (this.currentNode == null)
					{
						break;
					}
					this.position = 0;
					this.currentNode = this.currentNode.Clone();
					this.onAttribute = this.currentNode.MoveToFirstAttribute();
				}
				else
				{
					this.onAttribute = this.currentNode.MoveToNextAttribute();
				}
				if (this.onAttribute && this.matches(this.currentNode))
				{
					goto Block_3;
				}
			}
			return null;
			Block_3:
			this.position++;
			return this.currentNode;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004DBF4 File Offset: 0x0004CBF4
		public override XPathNavigator MatchNode(XPathNavigator context)
		{
			if (context != null && context.NodeType == XPathNodeType.Attribute && this.matches(context))
			{
				XPathNavigator xpathNavigator = context.Clone();
				if (xpathNavigator.MoveToParent())
				{
					return this.qyInput.MatchNode(xpathNavigator);
				}
			}
			return null;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004DC33 File Offset: 0x0004CC33
		public override XPathNodeIterator Clone()
		{
			return new AttributeQuery(this);
		}

		// Token: 0x04000B26 RID: 2854
		private bool onAttribute;
	}
}
