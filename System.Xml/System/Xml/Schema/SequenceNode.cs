using System;

namespace System.Xml.Schema
{
	internal sealed class SequenceNode : InteriorNode
	{
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			BitSet bitSet = new BitSet(lastpos.Count);
			base.LeftChild.ConstructPos(firstpos, bitSet, followpos);
			BitSet bitSet2 = new BitSet(firstpos.Count);
			base.RightChild.ConstructPos(bitSet2, lastpos, followpos);
			if (base.LeftChild.IsNullable && !base.RightChild.IsRangeNode)
			{
				firstpos.Or(bitSet2);
			}
			if (base.RightChild.IsNullable)
			{
				lastpos.Or(bitSet);
			}
			for (int num = bitSet.NextSet(-1); num != -1; num = bitSet.NextSet(num))
			{
				followpos[num].Or(bitSet2);
			}
			if (base.RightChild.IsRangeNode)
			{
				((LeafRangeNode)base.RightChild).NextIteration = firstpos.Clone();
			}
		}

		public override bool IsNullable
		{
			get
			{
				return (base.LeftChild.IsNullable && (base.RightChild.IsNullable || base.RightChild.IsRangeNode)) || (base.RightChild.IsRangeNode && ((LeafRangeNode)base.RightChild).Min == 0m);
			}
		}
	}
}
