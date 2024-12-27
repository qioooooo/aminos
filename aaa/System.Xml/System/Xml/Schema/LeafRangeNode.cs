using System;

namespace System.Xml.Schema
{
	// Token: 0x0200019E RID: 414
	internal sealed class LeafRangeNode : LeafNode
	{
		// Token: 0x06001570 RID: 5488 RVA: 0x0005F24D File Offset: 0x0005E24D
		public LeafRangeNode(decimal min, decimal max)
			: this(-1, min, max)
		{
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0005F258 File Offset: 0x0005E258
		public LeafRangeNode(int pos, decimal min, decimal max)
			: base(pos)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0005F26F File Offset: 0x0005E26F
		public decimal Max
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0005F277 File Offset: 0x0005E277
		public decimal Min
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0005F27F File Offset: 0x0005E27F
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x0005F287 File Offset: 0x0005E287
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

		// Token: 0x06001576 RID: 5494 RVA: 0x0005F290 File Offset: 0x0005E290
		public override SyntaxTreeNode Clone(Positions positions)
		{
			return new LeafRangeNode(base.Pos, this.min, this.max);
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x0005F2A9 File Offset: 0x0005E2A9
		public override bool IsRangeNode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000CC9 RID: 3273
		private decimal min;

		// Token: 0x04000CCA RID: 3274
		private decimal max;

		// Token: 0x04000CCB RID: 3275
		private BitSet nextIteration;
	}
}
