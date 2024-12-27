using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x0200031C RID: 796
	public class DataGridViewCellPaintingEventArgs : HandledEventArgs
	{
		// Token: 0x06003361 RID: 13153 RVA: 0x000B41E8 File Offset: 0x000B31E8
		public DataGridViewCellPaintingEventArgs(DataGridView dataGridView, Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, int columnIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if ((paintParts & ~DataGridViewPaintParts.All) != DataGridViewPaintParts.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewPaintPartsCombination", new object[] { "paintParts" }));
			}
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.cellBounds = cellBounds;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.cellState = cellState;
			this.value = value;
			this.formattedValue = formattedValue;
			this.errorText = errorText;
			this.cellStyle = cellStyle;
			this.advancedBorderStyle = advancedBorderStyle;
			this.paintParts = paintParts;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000B42AB File Offset: 0x000B32AB
		internal DataGridViewCellPaintingEventArgs(DataGridView dataGridView)
		{
			this.dataGridView = dataGridView;
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003363 RID: 13155 RVA: 0x000B42BA File Offset: 0x000B32BA
		public DataGridViewAdvancedBorderStyle AdvancedBorderStyle
		{
			get
			{
				return this.advancedBorderStyle;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x000B42C2 File Offset: 0x000B32C2
		public Rectangle CellBounds
		{
			get
			{
				return this.cellBounds;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003365 RID: 13157 RVA: 0x000B42CA File Offset: 0x000B32CA
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003366 RID: 13158 RVA: 0x000B42D2 File Offset: 0x000B32D2
		public Rectangle ClipBounds
		{
			get
			{
				return this.clipBounds;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003367 RID: 13159 RVA: 0x000B42DA File Offset: 0x000B32DA
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x000B42E2 File Offset: 0x000B32E2
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x000B42EA File Offset: 0x000B32EA
		public object FormattedValue
		{
			get
			{
				return this.formattedValue;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x000B42F2 File Offset: 0x000B32F2
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000B42FA File Offset: 0x000B32FA
		public DataGridViewPaintParts PaintParts
		{
			get
			{
				return this.paintParts;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x000B4302 File Offset: 0x000B3302
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000B430A File Offset: 0x000B330A
		public DataGridViewElementStates State
		{
			get
			{
				return this.cellState;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x000B4312 File Offset: 0x000B3312
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000B431C File Offset: 0x000B331C
		public void Paint(Rectangle clipBounds, DataGridViewPaintParts paintParts)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, paintParts);
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000B43E0 File Offset: 0x000B33E0
		public void PaintBackground(Rectangle clipBounds, bool cellsPaintSelectionBackground)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			DataGridViewPaintParts dataGridViewPaintParts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border;
			if (cellsPaintSelectionBackground)
			{
				dataGridViewPaintParts |= DataGridViewPaintParts.SelectionBackground;
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, dataGridViewPaintParts);
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000B44B0 File Offset: 0x000B34B0
		public void PaintContent(Rectangle clipBounds)
		{
			if (this.rowIndex < -1 || this.rowIndex >= this.dataGridView.Rows.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_RowIndexOutOfRange"));
			}
			if (this.columnIndex < -1 || this.columnIndex >= this.dataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange"));
			}
			this.dataGridView.GetCellInternal(this.columnIndex, this.rowIndex).PaintInternal(this.graphics, clipBounds, this.cellBounds, this.rowIndex, this.cellState, this.value, this.formattedValue, this.errorText, this.cellStyle, this.advancedBorderStyle, DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ErrorIcon);
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000B4574 File Offset: 0x000B3574
		internal void SetProperties(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, int columnIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			this.graphics = graphics;
			this.clipBounds = clipBounds;
			this.cellBounds = cellBounds;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.cellState = cellState;
			this.value = value;
			this.formattedValue = formattedValue;
			this.errorText = errorText;
			this.cellStyle = cellStyle;
			this.advancedBorderStyle = advancedBorderStyle;
			this.paintParts = paintParts;
			base.Handled = false;
		}

		// Token: 0x04001ABF RID: 6847
		private DataGridView dataGridView;

		// Token: 0x04001AC0 RID: 6848
		private Graphics graphics;

		// Token: 0x04001AC1 RID: 6849
		private Rectangle clipBounds;

		// Token: 0x04001AC2 RID: 6850
		private Rectangle cellBounds;

		// Token: 0x04001AC3 RID: 6851
		private int rowIndex;

		// Token: 0x04001AC4 RID: 6852
		private int columnIndex;

		// Token: 0x04001AC5 RID: 6853
		private DataGridViewElementStates cellState;

		// Token: 0x04001AC6 RID: 6854
		private object value;

		// Token: 0x04001AC7 RID: 6855
		private object formattedValue;

		// Token: 0x04001AC8 RID: 6856
		private string errorText;

		// Token: 0x04001AC9 RID: 6857
		private DataGridViewCellStyle cellStyle;

		// Token: 0x04001ACA RID: 6858
		private DataGridViewAdvancedBorderStyle advancedBorderStyle;

		// Token: 0x04001ACB RID: 6859
		private DataGridViewPaintParts paintParts;
	}
}
