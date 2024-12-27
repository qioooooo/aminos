using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020002CB RID: 715
	internal class DataGridAddNewRow : DataGridRow
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x0006CE8A File Offset: 0x0006BE8A
		public DataGridAddNewRow(DataGrid dGrid, DataGridTableStyle gridTable, int rowNum)
			: base(dGrid, gridTable, rowNum)
		{
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x0006CE95 File Offset: 0x0006BE95
		// (set) Token: 0x06002921 RID: 10529 RVA: 0x0006CE9D File Offset: 0x0006BE9D
		public bool DataBound
		{
			get
			{
				return this.dataBound;
			}
			set
			{
				this.dataBound = value;
			}
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x0006CEA6 File Offset: 0x0006BEA6
		public override void OnEdit()
		{
			if (!this.DataBound)
			{
				base.DataGrid.AddNewRow();
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x0006CEBB File Offset: 0x0006BEBB
		public override void OnRowLeave()
		{
			if (this.DataBound)
			{
				this.DataBound = false;
			}
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x0006CECC File Offset: 0x0006BECC
		internal override void LoseChildFocus(Rectangle rowHeader, bool alignToRight)
		{
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x0006CECE File Offset: 0x0006BECE
		internal override bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight)
		{
			return false;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x0006CED1 File Offset: 0x0006BED1
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int columnCount)
		{
			return this.Paint(g, bounds, trueRowBounds, firstVisibleColumn, columnCount, false);
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x0006CEE4 File Offset: 0x0006BEE4
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int columnCount, bool alignToRight)
		{
			Rectangle rectangle = bounds;
			DataGridLineStyle dataGridLineStyle;
			if (this.dgTable.IsDefault)
			{
				dataGridLineStyle = base.DataGrid.GridLineStyle;
			}
			else
			{
				dataGridLineStyle = this.dgTable.GridLineStyle;
			}
			int num = ((base.DataGrid == null) ? 0 : ((dataGridLineStyle == DataGridLineStyle.Solid) ? 1 : 0));
			rectangle.Height -= num;
			int num2 = base.PaintData(g, rectangle, firstVisibleColumn, columnCount, alignToRight);
			if (num > 0)
			{
				this.PaintBottomBorder(g, bounds, num2, num, alignToRight);
			}
			return num2;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x0006CF60 File Offset: 0x0006BF60
		protected override void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			if (this.DataBound)
			{
				CurrencyManager listManager = base.DataGrid.ListManager;
				column.Paint(g, cellBounds, listManager, base.RowNumber, alignToRight);
				return;
			}
			base.PaintCellContents(g, cellBounds, column, backBr, foreBrush, alignToRight);
		}

		// Token: 0x04001748 RID: 5960
		private bool dataBound;
	}
}
