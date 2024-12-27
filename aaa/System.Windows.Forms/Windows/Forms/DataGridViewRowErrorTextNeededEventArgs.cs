using System;

namespace System.Windows.Forms
{
	// Token: 0x02000389 RID: 905
	public class DataGridViewRowErrorTextNeededEventArgs : EventArgs
	{
		// Token: 0x0600378A RID: 14218 RVA: 0x000CA4F1 File Offset: 0x000C94F1
		internal DataGridViewRowErrorTextNeededEventArgs(int rowIndex, string errorText)
		{
			this.rowIndex = rowIndex;
			this.errorText = errorText;
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x000CA507 File Offset: 0x000C9507
		// (set) Token: 0x0600378C RID: 14220 RVA: 0x000CA50F File Offset: 0x000C950F
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x0600378D RID: 14221 RVA: 0x000CA518 File Offset: 0x000C9518
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001C1A RID: 7194
		private int rowIndex;

		// Token: 0x04001C1B RID: 7195
		private string errorText;
	}
}
