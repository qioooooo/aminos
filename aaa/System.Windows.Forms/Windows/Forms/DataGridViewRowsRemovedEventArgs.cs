using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000393 RID: 915
	public class DataGridViewRowsRemovedEventArgs : EventArgs
	{
		// Token: 0x060037F1 RID: 14321 RVA: 0x000CC8BC File Offset: 0x000CB8BC
		public DataGridViewRowsRemovedEventArgs(int rowIndex, int rowCount)
		{
			if (rowIndex < 0)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"rowIndex",
					rowIndex.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (rowCount < 1)
			{
				throw new ArgumentOutOfRangeException("rowCount", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"rowCount",
					rowCount.ToString(CultureInfo.CurrentCulture),
					1.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.rowIndex = rowIndex;
			this.rowCount = rowCount;
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x000CC96F File Offset: 0x000CB96F
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060037F3 RID: 14323 RVA: 0x000CC977 File Offset: 0x000CB977
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
		}

		// Token: 0x04001C4E RID: 7246
		private int rowIndex;

		// Token: 0x04001C4F RID: 7247
		private int rowCount;
	}
}
