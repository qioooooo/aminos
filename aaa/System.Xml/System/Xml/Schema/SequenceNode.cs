using System;

namespace System.Xml.Schema
{
	// Token: 0x02000199 RID: 409
	internal sealed class SequenceNode : InteriorNode
	{
		// Token: 0x06001561 RID: 5473 RVA: 0x0005F008 File Offset: 0x0005E008
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

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x0005F0C0 File Offset: 0x0005E0C0
		public override bool IsNullable
		{
			get
			{
				return (base.LeftChild.IsNullable && (base.RightChild.IsNullable || base.RightChild.IsRangeNode)) || (base.RightChild.IsRangeNode && ((LeafRangeNode)base.RightChild).Min == 0m);
			}
		}
	}
}
