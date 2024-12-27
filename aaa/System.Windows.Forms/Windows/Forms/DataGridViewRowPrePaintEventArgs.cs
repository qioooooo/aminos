using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000391 RID: 913
	public class DataGridViewRowPrePaintEventArgs : HandledEventArgs
	{
		// Token: 0x060037D9 RID: 14297 RVA: 0x000CC450 File Offset: 0x000CB450
		public DataGridViewRowPrePaintEventArgs(DataGridView dataGridView, Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, string errorText, DataGridViewCellStyle inheritedRowStyle, bool isFirstDisplayedRow, bool isLastVisibleRow)
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
			this.paintParts = DataGridViewPaintParts.All;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000CC4E3 File Offset: 0x000CB4E3
		internal DataGridViewRowPrePaintEventArgs(DataGridView dataGridView)
		{
			this.dataGridView = dataGridView;
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060037DB RID: 14299 RVA: 0x000CC4F2 File Offset: 0x000CB4F2
		// (set) Token: 0x060037DC RID: 14300 RVA: 0x000CC4FA File Offset: 0x000CB4FA
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

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060037DD RID: 14301 RVA: 0x000CC503 File Offset: 0x000CB503
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060037DE RID: 14302 RVA: 0x000CC50B File Offset: 0x000CB50B
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x000CC513 File Offset: 0x000CB513
		public DataGridViewCellStyle InheritedRowStyle
		{
			get
			{
				return this.inheritedRowStyle;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x000CC51B File Offset: 0x000CB51B
		public bool IsFirstDisplayedRow
		{
			get
			{
				return this.isFirstDisplayedRow;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060037E1 RID: 14305 RVA: 0x000CC523 File Offset: 0x000CB523
		public bool IsLastVisibleRow
		{
			get
			{
				return this.isLastVisibleRow;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x000CC52B File Offset: 0x000CB52B
		// (set) Token: 0x060037E3 RID: 14307 RVA: 0x000CC534 File Offset: 0x000CB534
		public DataGridViewPaintParts PaintParts
		{
			get
			{
				return this.paintParts;
			}
			set
			{
				if ((value & ~DataGridViewPaintParts.All) != DataGridViewPaintParts.None)
				{
					throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewPaintPartsCombination", new object[] { "value" }));
				}
				this.paintParts = value;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060037E4 RID: 14308 RVA: 0x000CC56E File Offset: 0x000CB56E
		public Rectangle RowBounds
		{
			get
			{
				return this.rowBounds;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x000CC576 File Offset: 0x000CB576
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000CC57E File Offset: 0x000CB57E
		public DataGridViewElementStates State
		{
			get
			{
				return this.rowState;
			}
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x000CC588 File Offset: 0x000CB588
		public void DrawFocus(Rectangle bounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).DrawFocus(this.graphics, this.clipBounds, bounds, this.rowIndex, this.rowState, this.inheritedRowStyle, cellsPaintSelectionBackground);
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000CC604 File Offset: 0x000CB604
		public void PaintCells(Rectangle clipBounds, DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x000CC684 File Offset: 0x000CB684
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

		// Token: 0x060037EA RID: 14314 RVA: 0x000CC710 File Offset: 0x000CB710
		public void PaintCellsContent(Rectangle clipBounds)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintCells(this.graphics, clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon);
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000CC790 File Offset: 0x000CB790
		public void PaintHeader(bool paintSelectionBackground)
		{
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon;
			if (paintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.PaintHeader(dataGridViewPaintParts);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x000CC7B0 File Offset: 0x000CB7B0
		public void PaintHeader(DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < 0 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			this.dataGridView.Rows.SharedRow(this.rowIndex).PaintHeader(this.graphics, this.clipBounds, this.rowBounds, this.rowIndex, this.rowState, this.isFirstDisplayedRow, this.isLastVisibleRow, paintParts);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000CC834 File Offset: 0x000CB834
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
			this.paintParts = DataGridViewPaintParts.All;
			base.Handled = false;
		}

		// Token: 0x04001C41 RID: 7233
		private DataGridView dataGridView;

		// Token: 0x04001C42 RID: 7234
		private Graphics graphics;

		// Token: 0x04001C43 RID: 7235
		private Rectangle clipBounds;

		// Token: 0x04001C44 RID: 7236
		private Rectangle rowBounds;

		// Token: 0x04001C45 RID: 7237
		private DataGridViewCellStyle inheritedRowStyle;

		// Token: 0x04001C46 RID: 7238
		private int rowIndex;

		// Token: 0x04001C47 RID: 7239
		private DataGridViewElementStates rowState;

		// Token: 0x04001C48 RID: 7240
		private string errorText;

		// Token: 0x04001C49 RID: 7241
		private bool isFirstDisplayedRow;

		// Token: 0x04001C4A RID: 7242
		private bool isLastVisibleRow;

		// Token: 0x04001C4B RID: 7243
		private DataGridViewPaintParts paintParts;
	}
}
