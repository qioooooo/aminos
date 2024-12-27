using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200014B RID: 331
	internal sealed class NamespaceQuery : BaseAxisQuery
	{
		// Token: 0x06001280 RID: 4736 RVA: 0x00050B08 File Offset: 0x0004FB08
		public NamespaceQuery(Query qyParent, string Name, string Prefix, XPathNodeType Type)
			: base(qyParent, Name, Prefix, Type)
		{
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00050B15 File Offset: 0x0004FB15
		private NamespaceQuery(NamespaceQuery other)
			: base(other)
		{
			this.onNamespace = other.onNamespace;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00050B2A File Offset: 0x0004FB2A
		public override void Reset()
		{
			this.onNamespace = false;
			base.Reset();
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00050B3C File Offset: 0x0004FB3C
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				if (!this.onNamespace)
				{
					this.currentNode = this.qyInput.Advance();
					if (this.currentNode == null)
					{
						break;
					}
					this.position = 0;
					this.currentNode = this.currentNode.Clone();
					this.onNamespace = this.currentNode.MoveToFirstNamespace();
				}
				else
				{
					this.onNamespace = this.currentNode.MoveToNextNamespace();
				}
				if (this.onNamespace && this.matches(this.currentNode))
				{
					goto Block_3;
				}
			}
			return null;
			Block_3:
			this.position++;
			return this.currentNode;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00050BD2 File Offset: 0x0004FBD2
		public override bool matches(XPathNavigator e)
		{
			return e.Value.Length != 0 && (!base.NameTest || base.Name.Equals(e.LocalName));
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00050BFE File Offset: 0x0004FBFE
		public override XPathNodeIterator Clone()
		{
			return new NamespaceQuery(this);
		}

		// Token: 0x04000B9B RID: 2971
		private bool onNamespace;
	}
}
