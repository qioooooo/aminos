using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200038F RID: 911
	public class DataGridViewRowHeightInfoPushedEventArgs : HandledEventArgs
	{
		// Token: 0x060037C2 RID: 14274 RVA: 0x000CC02B File Offset: 0x000CB02B
		internal DataGridViewRowHeightInfoPushedEventArgs(int rowIndex, int height, int minimumHeight)
			: base(false)
		{
			this.rowIndex = rowIndex;
			this.height = height;
			this.minimumHeight = minimumHeight;
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000CC049 File Offset: 0x000CB049
		public int Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000CC051 File Offset: 0x000CB051
		public int MinimumHeight
		{
			get
			{
				return this.minimumHeight;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x000CC059 File Offset: 0x000CB059
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001C34 RID: 7220
		private int rowIndex;

		// Token: 0x04001C35 RID: 7221
		private int height;

		// Token: 0x04001C36 RID: 7222
		private int minimumHeight;
	}
}
