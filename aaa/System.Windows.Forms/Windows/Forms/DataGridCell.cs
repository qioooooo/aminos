using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002D4 RID: 724
	public struct DataGridCell
	{
		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x0006F12B File Offset: 0x0006E12B
		// (set) Token: 0x060029F7 RID: 10743 RVA: 0x0006F133 File Offset: 0x0006E133
		public int ColumnNumber
		{
			get
			{
				return this.columnNumber;
			}
			set
			{
				this.columnNumber = value;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x0006F13C File Offset: 0x0006E13C
		// (set) Token: 0x060029F9 RID: 10745 RVA: 0x0006F144 File Offset: 0x0006E144
		public int RowNumber
		{
			get
			{
				return this.rowNumber;
			}
			set
			{
				this.rowNumber = value;
			}
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0006F14D File Offset: 0x0006E14D
		public DataGridCell(int r, int c)
		{
			this.rowNumber = r;
			this.columnNumber = c;
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0006F160 File Offset: 0x0006E160
		public override bool Equals(object o)
		{
			if (o is DataGridCell)
			{
				DataGridCell dataGridCell = (DataGridCell)o;
				return dataGridCell.RowNumber == this.RowNumber && dataGridCell.ColumnNumber == this.ColumnNumber;
			}
			return false;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0006F19E File Offset: 0x0006E19E
		public override int GetHashCode()
		{
			return ((~this.rowNumber * (this.columnNumber + 1)) & 16776960) >> 8;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x0006F1B8 File Offset: 0x0006E1B8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridCell {RowNumber = ",
				this.RowNumber.ToString(CultureInfo.CurrentCulture),
				", ColumnNumber = ",
				this.ColumnNumber.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x04001795 RID: 6037
		private int rowNumber;

		// Token: 0x04001796 RID: 6038
		private int columnNumber;
	}
}
