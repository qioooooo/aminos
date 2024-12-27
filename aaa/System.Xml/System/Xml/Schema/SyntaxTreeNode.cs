using System;

namespace System.Xml.Schema
{
	// Token: 0x02000195 RID: 405
	internal abstract class SyntaxTreeNode
	{
		// Token: 0x06001547 RID: 5447
		public abstract void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions);

		// Token: 0x06001548 RID: 5448
		public abstract SyntaxTreeNode Clone(Positions positions);

		// Token: 0x06001549 RID: 5449
		public abstract void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos);

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600154A RID: 5450
		public abstract bool IsNullable { get; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0005EE04 File Offset: 0x0005DE04
		public virtual bool IsRangeNode
		{
			get
			{
				return false;
			}
		}
	}
}
