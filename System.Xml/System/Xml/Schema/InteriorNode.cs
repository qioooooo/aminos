using System;

namespace System.Xml.Schema
{
	internal abstract class InteriorNode : SyntaxTreeNode
	{
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

		public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
		{
			this.leftChild.ExpandTree(this, symbols, positions);
			if (this.rightChild != null)
			{
				this.rightChild.ExpandTree(this, symbols, positions);
			}
		}

		private SyntaxTreeNode leftChild;

		private SyntaxTreeNode rightChild;
	}
}
