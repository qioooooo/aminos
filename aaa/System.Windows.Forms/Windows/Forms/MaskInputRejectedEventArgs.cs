using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020004A1 RID: 1185
	public class MaskInputRejectedEventArgs : EventArgs
	{
		// Token: 0x06004725 RID: 18213 RVA: 0x001023DE File Offset: 0x001013DE
		public MaskInputRejectedEventArgs(int position, MaskedTextResultHint rejectionHint)
		{
			this.position = position;
			this.hint = rejectionHint;
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x001023F4 File Offset: 0x001013F4
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x001023FC File Offset: 0x001013FC
		public MaskedTextResultHint RejectionHint
		{
			get
			{
				return this.hint;
			}
		}

		// Token: 0x040021D3 RID: 8659
		private int position;

		// Token: 0x040021D4 RID: 8660
		private MaskedTextResultHint hint;
	}
}
