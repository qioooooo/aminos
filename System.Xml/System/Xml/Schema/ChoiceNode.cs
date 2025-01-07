using System;

namespace System.Xml.Schema
{
	internal sealed class ChoiceNode : InteriorNode
	{
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			base.LeftChild.ConstructPos(firstpos, lastpos, followpos);
			BitSet bitSet = new BitSet(firstpos.Count);
			BitSet bitSet2 = new BitSet(lastpos.Count);
			base.RightChild.ConstructPos(bitSet, bitSet2, followpos);
			firstpos.Or(bitSet);
			lastpos.Or(bitSet2);
		}

		public override bool IsNullable
		{
			get
			{
				return base.LeftChild.IsNullable || base.RightChild.IsNullable;
			}
		}
	}
}
