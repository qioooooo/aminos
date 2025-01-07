using System;

namespace System.Xml.Schema
{
	internal sealed class LeafRangeNode : LeafNode
	{
		public LeafRangeNode(decimal min, decimal max)
			: this(-1, min, max)
		{
		}

		public LeafRangeNode(int pos, decimal min, decimal max)
			: base(pos)
		{
			this.min = min;
			this.max = max;
		}

		public decimal Max
		{
			get
			{
				return this.max;
			}
		}

		public decimal Min
		{
			get
			{
				return this.min;
			}
		}

		public BitSet NextIteration
		{
			get
			{
				return this.nextIteration;
			}
			set
			{
				this.nextIteration = value;
			}
		}

		public override SyntaxTreeNode Clone(Positions positions)
		{
			return new LeafRangeNode(base.Pos, this.min, this.max);
		}

		public override bool IsRangeNode
		{
			get
			{
				return true;
			}
		}

		private decimal min;

		private decimal max;

		private BitSet nextIteration;
	}
}
