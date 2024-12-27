using System;

namespace System.Windows.Forms
{
	// Token: 0x02000322 RID: 802
	public class DataGridViewCellStyleContentChangedEventArgs : EventArgs
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x000B5754 File Offset: 0x000B4754
		internal DataGridViewCellStyleContentChangedEventArgs(DataGridViewCellStyle dataGridViewCellStyle, bool changeAffectsPreferredSize)
		{
			this.dataGridViewCellStyle = dataGridViewCellStyle;
			this.changeAffectsPreferredSize = changeAffectsPreferredSize;
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000B576A File Offset: 0x000B476A
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.dataGridViewCellStyle;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060033B9 RID: 13241 RVA: 0x000B5772 File Offset: 0x000B4772
		public DataGridViewCellStyleScopes CellStyleScope
		{
			get
			{
				return this.dataGridViewCellStyle.Scope;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000B577F File Offset: 0x000B477F
		internal bool ChangeAffectsPreferredSize
		{
			get
			{
				return this.changeAffectsPreferredSize;
			}
		}

		// Token: 0x04001AE9 RID: 6889
		private DataGridViewCellStyle dataGridViewCellStyle;

		// Token: 0x04001AEA RID: 6890
		private bool changeAffectsPreferredSize;
	}
}
