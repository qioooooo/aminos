using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200028E RID: 654
	public class ColumnWidthChangingEventArgs : CancelEventArgs
	{
		// Token: 0x060022E4 RID: 8932 RVA: 0x0004CF70 File Offset: 0x0004BF70
		public ColumnWidthChangingEventArgs(int columnIndex, int newWidth, bool cancel)
			: base(cancel)
		{
			this.columnIndex = columnIndex;
			this.newWidth = newWidth;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0004CF87 File Offset: 0x0004BF87
		public ColumnWidthChangingEventArgs(int columnIndex, int newWidth)
		{
			this.columnIndex = columnIndex;
			this.newWidth = newWidth;
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0004CF9D File Offset: 0x0004BF9D
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0004CFA5 File Offset: 0x0004BFA5
		// (set) Token: 0x060022E8 RID: 8936 RVA: 0x0004CFAD File Offset: 0x0004BFAD
		public int NewWidth
		{
			get
			{
				return this.newWidth;
			}
			set
			{
				this.newWidth = value;
			}
		}

		// Token: 0x0400153B RID: 5435
		private int columnIndex;

		// Token: 0x0400153C RID: 5436
		private int newWidth;
	}
}
