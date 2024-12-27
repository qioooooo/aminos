using System;

namespace System.Xml.Schema
{
	// Token: 0x02000198 RID: 408
	internal abstract class InteriorNode : SyntaxTreeNode
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0005EF72 File Offset: 0x0005DF72
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0005EF7A File Offset: 0x0005DF7A
		public SyntaxTreeNode LeftChild
		{
			get
			{
				return this.leftChild;
			}
			set
			{
				this.leftChild = value;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0005EF83 File Offset: 0x0005DF83
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x0005EF8B File Offset: 0x0005DF8B
		public SyntaxTreeNode RightChild
		{
			get
			{
				return this.rightChild;
			}
			set
			{
				this.rightChild = value;
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0005EF94 File Offset: 0x0005DF94
		public override SyntaxTreeNode Clone(Positions positions)
		{
			InteriorNode interiorNode = (InteriorNode)base.MemberwiseClone();
			interiorNode.LeftChild = this.leftChild.Clone(positions);
			if (this.rightChild != null)
			{
				interiorNode.RightChild = this.rightChild.Clone(positions);
			}
			return interiorNode;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0005EFDA File Offset: 0x0005DFDA
		public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
		{
			this.leftChild.ExpandTree(this, symbols, positions);
			if (this.rightChild != null)
			{
				this.rightChild.ExpandTree(this, symbols, positions);
			}
		}

		// Token: 0x04000CC7 RID: 3271
		private SyntaxTreeNode leftChild;

		// Token: 0x04000CC8 RID: 3272
		private SyntaxTreeNode rightChild;
	}
}
