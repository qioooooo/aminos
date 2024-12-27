using System;

namespace System.Xml.Schema
{
	// Token: 0x0200018B RID: 395
	internal class LocatedActiveAxis : ActiveAxis
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0005E05A File Offset: 0x0005D05A
		internal int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0005E062 File Offset: 0x0005D062
		internal LocatedActiveAxis(Asttree astfield, KeySequence ks, int column)
			: base(astfield)
		{
			this.Ks = ks;
			this.column = column;
			this.isMatched = false;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0005E080 File Offset: 0x0005D080
		internal void Reactivate(KeySequence ks)
		{
			base.Reactivate();
			this.Ks = ks;
		}

		// Token: 0x04000CA3 RID: 3235
		private int column;

		// Token: 0x04000CA4 RID: 3236
		internal bool isMatched;

		// Token: 0x04000CA5 RID: 3237
		internal KeySequence Ks;
	}
}
