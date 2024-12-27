using System;

namespace System.Xml.Schema
{
	// Token: 0x0200019D RID: 413
	internal sealed class StarNode : InteriorNode
	{
		// Token: 0x0600156D RID: 5485 RVA: 0x0005F208 File Offset: 0x0005E208
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			base.LeftChild.ConstructPos(firstpos, lastpos, followpos);
			for (int num = lastpos.NextSet(-1); num != -1; num = lastpos.NextSet(num))
			{
				followpos[num].Or(firstpos);
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x0005F242 File Offset: 0x0005E242
		public override bool IsNullable
		{
			get
			{
				return true;
			}
		}
	}
}
