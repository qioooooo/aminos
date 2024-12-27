using System;

namespace System.Windows.Forms
{
	// Token: 0x02000315 RID: 789
	public class DataGridViewCellErrorTextNeededEventArgs : DataGridViewCellEventArgs
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x000B3D92 File Offset: 0x000B2D92
		internal DataGridViewCellErrorTextNeededEventArgs(int columnIndex, int rowIndex, string errorText)
			: base(columnIndex, rowIndex)
		{
			this.errorText = errorText;
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x000B3DA3 File Offset: 0x000B2DA3
		// (set) Token: 0x0600333D RID: 13117 RVA: 0x000B3DAB File Offset: 0x000B2DAB
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

		// Token: 0x04001AAA RID: 6826
		private string errorText;
	}
}
