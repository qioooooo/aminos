using System;

namespace System.Windows.Forms
{
	// Token: 0x0200028C RID: 652
	public class ColumnWidthChangedEventArgs : EventArgs
	{
		// Token: 0x060022DE RID: 8926 RVA: 0x0004CF59 File Offset: 0x0004BF59
		public ColumnWidthChangedEventArgs(int columnIndex)
		{
			this.columnIndex = columnIndex;
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x0004CF68 File Offset: 0x0004BF68
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x0400153A RID: 5434
		private readonly int columnIndex;
	}
}
