using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000197 RID: 407
	internal class NamespaceListNode : SyntaxTreeNode
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x0005EE7D File Offset: 0x0005DE7D
		public NamespaceListNode(NamespaceList namespaceList, object particle)
		{
			this.namespaceList = namespaceList;
			this.particle = particle;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0005EE93 File Offset: 0x0005DE93
		public override SyntaxTreeNode Clone(Positions positions)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0005EE9A File Offset: 0x0005DE9A
		public virtual ICollection GetResolvedSymbols(SymbolsDictionary symbols)
		{
			return symbols.GetNamespaceListSymbols(this.namespaceList);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0005EEA8 File Offset: 0x0005DEA8
		public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
		{
			SyntaxTreeNode syntaxTreeNode = null;
			foreach (object obj in this.GetResolvedSymbols(symbols))
			{
				int num = (int)obj;
				if (symbols.GetParticle(num) != this.particle)
				{
					symbols.IsUpaEnforced = false;
				}
				LeafNode leafNode = new LeafNode(positions.Add(num, this.particle));
				if (syntaxTreeNode == null)
				{
					syntaxTreeNode = leafNode;
				}
				else
				{
					syntaxTreeNode = new ChoiceNode
					{
						LeftChild = syntaxTreeNode,
						RightChild = leafNode
					};
				}
			}
			if (parent.LeftChild == this)
			{
				parent.LeftChild = syntaxTreeNode;
				return;
			}
			parent.RightChild = syntaxTreeNode;
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0005EF64 File Offset: 0x0005DF64
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0005EF6B File Offset: 0x0005DF6B
		public override bool IsNullable
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04000CC5 RID: 3269
		protected NamespaceList namespaceList;

		// Token: 0x04000CC6 RID: 3270
		protected object particle;
	}
}
