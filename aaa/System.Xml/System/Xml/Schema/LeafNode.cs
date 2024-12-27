using System;

namespace System.Xml.Schema
{
	// Token: 0x02000196 RID: 406
	internal class LeafNode : SyntaxTreeNode
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x0005EE0F File Offset: 0x0005DE0F
		public LeafNode(int pos)
		{
			this.pos = pos;
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0005EE1E File Offset: 0x0005DE1E
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x0005EE26 File Offset: 0x0005DE26
		public int Pos
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0005EE2F File Offset: 0x0005DE2F
		public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
		{
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0005EE31 File Offset: 0x0005DE31
		public override SyntaxTreeNode Clone(Positions positions)
		{
			return new LeafNode(positions.Add(positions[this.pos].symbol, positions[this.pos].particle));
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0005EE60 File Offset: 0x0005DE60
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			firstpos.Set(this.pos);
			lastpos.Set(this.pos);
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x0005EE7A File Offset: 0x0005DE7A
		public override bool IsNullable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000CC4 RID: 3268
		private int pos;
	}
}
