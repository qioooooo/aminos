using System;

namespace System.Windows.Forms
{
	// Token: 0x02000326 RID: 806
	public class DataGridViewCellToolTipTextNeededEventArgs : DataGridViewCellEventArgs
	{
		// Token: 0x060033BE RID: 13246 RVA: 0x000B5805 File Offset: 0x000B4805
		internal DataGridViewCellToolTipTextNeededEventArgs(int columnIndex, int rowIndex, string toolTipText)
			: base(columnIndex, rowIndex)
		{
			this.toolTipText = toolTipText;
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x000B5816 File Offset: 0x000B4816
		// (set) Token: 0x060033C0 RID: 13248 RVA: 0x000B581E File Offset: 0x000B481E
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		// Token: 0x04001AF9 RID: 6905
		private string toolTipText;
	}
}
