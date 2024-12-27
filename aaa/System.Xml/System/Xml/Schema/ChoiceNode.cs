using System;

namespace System.Xml.Schema
{
	// Token: 0x0200019A RID: 410
	internal sealed class ChoiceNode : InteriorNode
	{
		// Token: 0x06001564 RID: 5476 RVA: 0x0005F128 File Offset: 0x0005E128
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			base.LeftChild.ConstructPos(firstpos, lastpos, followpos);
			BitSet bitSet = new BitSet(firstpos.Count);
			BitSet bitSet2 = new BitSet(lastpos.Count);
			base.RightChild.ConstructPos(bitSet, bitSet2, followpos);
			firstpos.Or(bitSet);
			lastpos.Or(bitSet2);
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x0005F177 File Offset: 0x0005E177
		public override bool IsNullable
		{
			get
			{
				return base.LeftChild.IsNullable || base.RightChild.IsNullable;
			}
		}
	}
}
