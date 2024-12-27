using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000390 RID: 912
	public class DataGridViewRowPostPaintEventArgs : EventArgs
	{
		// Token: 0x060037C6 RID: 14278 RVA: 0x000CC064 File Offset: 0x000CB064
		public DataGridViewRowPostPaintEventArgs(DataGridView dataGridView, Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, string errorText, DataGridViewCellStyle inheritedRowStyle, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (inheritedRowStyle == null)
			{
				throw new ArgumentNullException("inheritedRowStyle");
			}
			this.dataGridView = dataGridView;
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.rowBounds = rowBounds;
			this.rowIndex = rowIndex;
			this.rowState = rowState;
			this.errorText = errorText;
			this.inheritedRowStyle = inheritedRowStyle;
			this.isFirstDisplayedRow = isFirstDisplayedRow;
			this.isLastVisibleRow = isLastVisibleRow;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000CC0EF File Offset: 0x000CB0EF
		internal DataGridViewRowPostPaintEventArgs(DataGridView dataGridView)
		{
			this.dataGridView = dataGridView;
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x000CC0FE File Offset: 0x000CB0FE
		// (set) Token: 0x060037C9 RID: 14281 RVA: 0x000CC106 File Offset: 0x000CB106
		public Rectangle ClipBounds
		{
			get
			{
				return this.clipBounds;
			}
			set
			{
				this.clipBounds = value;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060037CA RID: 14282 RVA: 0x000CC10F File Offset: 0x000CB10F
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060037CB RID: 14283 RVA: 0x000CC117 File Offset: 0x000CB117
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060037CC RID: 14284 RVA: 0x000CC11F File Offset: 0x000CB11F
		public DataGridViewCellStyle InheritedRowStyle
		{
			get
			{
				return this.inheritedRowStyle;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060037CD RID: 14285 RVA: 0x000CC127 File Offset: 0x000CB127
		public bool IsFirstDisplayedRow
		{
			get
			{
				return this.isFirstDisplayedRow;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060037CE RID: 14286 RVA: 0x000CC12F File Offset: 0x000CB12F
		public bool IsLastVisibleRow
		{
			get
			{
				return this.isLastVisibleRow;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060037CF RID: 14287 RVA: 0x000CC137 File Offset: 0x000CB137
		public Rectangle RowBounds
		{
			get
			{
				return this.rowBounds;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060037D0 RID: 14288 RVA: 0x000CC13F File Offset: 0x000CB13F
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060037D1 RID: 14289 RVA: 0x000CC147 File Offset: 0x000CB147
		public DataGridViewElementStates State
		{
			get
			{
				return this.rowState;
			}
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000CC150 File Offset: 0x000CB150
		public void DrawFocus(Rectangle bounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).DrawFocus(this.graphics, this.clipBounds, bounds, this.rowIndex, this.rowState, this.inheritedRowStyle, cellsPaintSelectionBackground);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000CC1CC File Offset: 0x000CB1CC
		public void PaintCells(Rectangle clipBounds, DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000CC24C File Offset: 0x000CB24C
		public void PaintCellsBackground(Rectangle clipBounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border;
			if (cellsPaintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, dataGridViewPaintParts);
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000CC2D8 File Offset: 0x000CB2D8
		public void PaintCellsContent(Rectangle clipBounds)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000CC358 File Offset: 0x000CB358
		public void PaintHeader(bool paintSelectionBackground)
		{
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon;
			if (paintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.PaintHeader(dataGridViewPaintParts);
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000CC378 File Offset: 0x000CB378
		public void PaintHeader(DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintHeader(this.graphics, this.clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000CC3FC File Offset: 0x000CB3FC
		internal void SetProperties(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, string errorText, DataGridViewCellStyle inheritedRowStyle, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.rowBounds = rowBounds;
			this.rowIndex = rowIndex;
			this.rowState = rowState;
			this.errorText = errorText;
			this.inheritedRowStyle = inheritedRowStyle;
			this.isFirstDisplayedRow = isFirstDisplayedRow;
			this.isLastVisibleRow = isLastVisibleRow;
		}

		// Token: 0x04001C37 RID: 7223
		private DataGridView dataGridView;

		// Token: 0x04001C38 RID: 7224
		private Graphics graphics;

		// Token: 0x04001C39 RID: 7225
		private Rectangle clipBounds;

		// Token: 0x04001C3A RID: 7226
		private Rectangle rowBounds;

		// Token: 0x04001C3B RID: 7227
		private DataGridViewCellStyle inheritedRowStyle;

		// Token: 0x04001C3C RID: 7228
		private int rowIndex;

		// Token: 0x04001C3D RID: 7229
		private DataGridViewElementStates rowState;

		// Token: 0x04001C3E RID: 7230
		private string errorText;

		// Token: 0x04001C3F RID: 7231
		private bool isFirstDisplayedRow;

		// Token: 0x04001C40 RID: 7232
		private bool isLastVisibleRow;
	}
}
