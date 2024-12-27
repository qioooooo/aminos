using System;

namespace System.Xml.Schema
{
	// Token: 0x0200019B RID: 411
	internal sealed class PlusNode : InteriorNode
	{
		// Token: 0x06001567 RID: 5479 RVA: 0x0005F19C File Offset: 0x0005E19C
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			base.LeftChild.ConstructPos(firstpos, lastpos, followpos);
			for (int num = lastpos.NextSet(-1); num != -1; num = lastpos.NextSet(num))
			{
				followpos[num].Or(firstpos);
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x0005F1D6 File Offset: 0x0005E1D6
		public override bool IsNullable
		{
			get
			{
				return base.LeftChild.IsNullable;
			}
		}
	}
}
