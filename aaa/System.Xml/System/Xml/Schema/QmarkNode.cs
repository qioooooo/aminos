using System;

namespace System.Xml.Schema
{
	// Token: 0x0200019C RID: 412
	internal sealed class QmarkNode : InteriorNode
	{
		// Token: 0x0600156A RID: 5482 RVA: 0x0005F1EB File Offset: 0x0005E1EB
		public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
		{
			base.LeftChild.ConstructPos(firstpos, lastpos, followpos);
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x0005F1FB File Offset: 0x0005E1FB
		public override bool IsNullable
		{
			get
			{
				return true;
			}
		}
	}
}
