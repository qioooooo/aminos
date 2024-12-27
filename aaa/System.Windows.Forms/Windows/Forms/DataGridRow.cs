using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002C8 RID: 712
	internal abstract class DataGridRow : MarshalByRefObject
	{
		// Token: 0x060028CA RID: 10442 RVA: 0x0006BC10 File Offset: 0x0006AC10
		public DataGridRow(DataGrid dataGrid, DataGridTableStyle dgTable, int rowNumber)
		{
			if (dataGrid == null || dgTable.DataGrid == null)
			{
				throw new ArgumentNullException("dataGrid");
			}
			if (rowNumber < 0)
			{
				throw new ArgumentException(SR.GetString("DataGridRowRowNumber"), "rowNumber");
			}
			this.number = rowNumber;
			DataGridRow.colorMap[0].OldColor = Color.Black;
			DataGridRow.colorMap[0].NewColor = dgTable.HeaderForeColor;
			this.dgTable = dgTable;
			this.height = this.MinimumRowHeight(dgTable);
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0006BCA7 File Offset: 0x0006ACA7
		public AccessibleObject AccessibleObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = this.CreateAccessibleObject();
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x0006BCC3 File Offset: 0x0006ACC3
		protected virtual AccessibleObject CreateAccessibleObject()
		{
			return new DataGridRow.DataGridRowAccessibleObject(this);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x0006BCCB File Offset: 0x0006ACCB
		protected internal virtual int MinimumRowHeight(DataGridTableStyle dgTable)
		{
			return this.MinimumRowHeight(dgTable.GridColumnStyles);
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x0006BCDC File Offset: 0x0006ACDC
		protected internal virtual int MinimumRowHeight(GridColumnStylesCollection columns)
		{
			int num = (this.dgTable.IsDefault ? this.DataGrid.PreferredRowHeight : this.dgTable.PreferredRowHeight);
			try
			{
				if (this.dgTable.DataGrid.DataSource != null)
				{
					int count = columns.Count;
					for (int i = 0; i < count; i++)
					{
						if (columns[i].PropertyDescriptor != null)
						{
							num = Math.Max(num, columns[i].GetMinimumHeight());
						}
					}
				}
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0006BD6C File Offset: 0x0006AD6C
		public DataGrid DataGrid
		{
			get
			{
				return this.dgTable.DataGrid;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x0006BD79 File Offset: 0x0006AD79
		// (set) Token: 0x060028D1 RID: 10449 RVA: 0x0006BD81 File Offset: 0x0006AD81
		internal DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.dgTable;
			}
			set
			{
				this.dgTable = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0006BD8A File Offset: 0x0006AD8A
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x0006BD92 File Offset: 0x0006AD92
		public virtual int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = Math.Max(0, value);
				this.dgTable.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x0006BDB2 File Offset: 0x0006ADB2
		public int RowNumber
		{
			get
			{
				return this.number;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x0006BDBA File Offset: 0x0006ADBA
		// (set) Token: 0x060028D6 RID: 10454 RVA: 0x0006BDC2 File Offset: 0x0006ADC2
		public virtual bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
				this.InvalidateRow();
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x0006BDD4 File Offset: 0x0006ADD4
		protected Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridCaption), bitmapName);
				bitmap.MakeTransparent();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return bitmap;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x0006BE10 File Offset: 0x0006AE10
		public virtual Rectangle GetCellBounds(int col)
		{
			int firstVisibleColumn = this.dgTable.DataGrid.FirstVisibleColumn;
			int num = 0;
			Rectangle rectangle = default(Rectangle);
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			if (gridColumnStyles != null)
			{
				for (int i = firstVisibleColumn; i < col; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
				}
				int gridLineWidth = this.dgTable.GridLineWidth;
				rectangle = new Rectangle(num, 0, gridColumnStyles[col].Width - gridLineWidth, this.Height - gridLineWidth);
			}
			return rectangle;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x0006BEA5 File Offset: 0x0006AEA5
		public virtual Rectangle GetNonScrollableArea()
		{
			return Rectangle.Empty;
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x0006BEAC File Offset: 0x0006AEAC
		protected Bitmap GetStarBitmap()
		{
			if (DataGridRow.starBmp == null)
			{
				DataGridRow.starBmp = this.GetBitmap("DataGridRow.star.bmp");
			}
			return DataGridRow.starBmp;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x0006BECA File Offset: 0x0006AECA
		protected Bitmap GetPencilBitmap()
		{
			if (DataGridRow.pencilBmp == null)
			{
				DataGridRow.pencilBmp = this.GetBitmap("DataGridRow.pencil.bmp");
			}
			return DataGridRow.pencilBmp;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0006BEE8 File Offset: 0x0006AEE8
		protected Bitmap GetErrorBitmap()
		{
			if (DataGridRow.errorBmp == null)
			{
				DataGridRow.errorBmp = this.GetBitmap("DataGridRow.error.bmp");
			}
			DataGridRow.errorBmp.MakeTransparent();
			return DataGridRow.errorBmp;
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x0006BF10 File Offset: 0x0006AF10
		protected Bitmap GetLeftArrowBitmap()
		{
			if (DataGridRow.leftArrow == null)
			{
				DataGridRow.leftArrow = this.GetBitmap("DataGridRow.left.bmp");
			}
			return DataGridRow.leftArrow;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0006BF2E File Offset: 0x0006AF2E
		protected Bitmap GetRightArrowBitmap()
		{
			if (DataGridRow.rightArrow == null)
			{
				DataGridRow.rightArrow = this.GetBitmap("DataGridRow.right.bmp");
			}
			return DataGridRow.rightArrow;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x0006BF4C File Offset: 0x0006AF4C
		public virtual void InvalidateRow()
		{
			this.dgTable.DataGrid.InvalidateRow(this.number);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x0006BF64 File Offset: 0x0006AF64
		public virtual void InvalidateRowRect(Rectangle r)
		{
			this.dgTable.DataGrid.InvalidateRowRect(this.number, r);
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x0006BF7D File Offset: 0x0006AF7D
		public virtual void OnEdit()
		{
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x0006BF80 File Offset: 0x0006AF80
		public virtual bool OnKeyPress(Keys keyData)
		{
			int columnNumber = this.dgTable.DataGrid.CurrentCell.ColumnNumber;
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			if (gridColumnStyles != null && columnNumber >= 0 && columnNumber < gridColumnStyles.Count)
			{
				DataGridColumnStyle dataGridColumnStyle = gridColumnStyles[columnNumber];
				if (dataGridColumnStyle.KeyPress(this.RowNumber, keyData))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x0006BFDC File Offset: 0x0006AFDC
		public virtual bool OnMouseDown(int x, int y, Rectangle rowHeaders)
		{
			return this.OnMouseDown(x, y, rowHeaders, false);
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x0006BFE8 File Offset: 0x0006AFE8
		public virtual bool OnMouseDown(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			this.LoseChildFocus(rowHeaders, alignToRight);
			return false;
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x0006BFF4 File Offset: 0x0006AFF4
		public virtual bool OnMouseMove(int x, int y, Rectangle rowHeaders)
		{
			return false;
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x0006BFF7 File Offset: 0x0006AFF7
		public virtual bool OnMouseMove(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			return false;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x0006BFFA File Offset: 0x0006AFFA
		public virtual void OnMouseLeft(Rectangle rowHeaders, bool alignToRight)
		{
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x0006BFFC File Offset: 0x0006AFFC
		public virtual void OnMouseLeft()
		{
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x0006BFFE File Offset: 0x0006AFFE
		public virtual void OnRowEnter()
		{
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x0006C000 File Offset: 0x0006B000
		public virtual void OnRowLeave()
		{
		}

		// Token: 0x060028EB RID: 10475
		internal abstract bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight);

		// Token: 0x060028EC RID: 10476
		internal abstract void LoseChildFocus(Rectangle rowHeaders, bool alignToRight);

		// Token: 0x060028ED RID: 10477
		public abstract int Paint(Graphics g, Rectangle dataBounds, Rectangle rowBounds, int firstVisibleColumn, int numVisibleColumns);

		// Token: 0x060028EE RID: 10478
		public abstract int Paint(Graphics g, Rectangle dataBounds, Rectangle rowBounds, int firstVisibleColumn, int numVisibleColumns, bool alignToRight);

		// Token: 0x060028EF RID: 10479 RVA: 0x0006C002 File Offset: 0x0006B002
		protected virtual void PaintBottomBorder(Graphics g, Rectangle bounds, int dataWidth)
		{
			this.PaintBottomBorder(g, bounds, dataWidth, this.dgTable.GridLineWidth, false);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0006C01C File Offset: 0x0006B01C
		protected virtual void PaintBottomBorder(Graphics g, Rectangle bounds, int dataWidth, int borderWidth, bool alignToRight)
		{
			Rectangle rectangle = new Rectangle(alignToRight ? (bounds.Right - dataWidth) : bounds.X, bounds.Bottom - borderWidth, dataWidth, borderWidth);
			g.FillRectangle(this.dgTable.IsDefault ? this.DataGrid.GridLineBrush : this.dgTable.GridLineBrush, rectangle);
			if (dataWidth < bounds.Width)
			{
				g.FillRectangle(this.dgTable.DataGrid.BackgroundBrush, alignToRight ? bounds.X : rectangle.Right, rectangle.Y, bounds.Width - rectangle.Width, borderWidth);
			}
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0006C0CB File Offset: 0x0006B0CB
		public virtual int PaintData(Graphics g, Rectangle bounds, int firstVisibleColumn, int columnCount)
		{
			return this.PaintData(g, bounds, firstVisibleColumn, columnCount, false);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0006C0DC File Offset: 0x0006B0DC
		public virtual int PaintData(Graphics g, Rectangle bounds, int firstVisibleColumn, int columnCount, bool alignToRight)
		{
			Rectangle rectangle = bounds;
			int num = (this.dgTable.IsDefault ? this.DataGrid.GridLineWidth : this.dgTable.GridLineWidth);
			int num2 = 0;
			DataGridCell currentCell = this.dgTable.DataGrid.CurrentCell;
			GridColumnStylesCollection gridColumnStyles = this.dgTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num3 = firstVisibleColumn;
			while (num3 < count && num2 <= bounds.Width)
			{
				if (gridColumnStyles[num3].PropertyDescriptor != null && gridColumnStyles[num3].Width > 0)
				{
					rectangle.Width = gridColumnStyles[num3].Width - num;
					if (alignToRight)
					{
						rectangle.X = bounds.Right - num2 - rectangle.Width;
					}
					else
					{
						rectangle.X = bounds.X + num2;
					}
					Brush brush = this.BackBrushForDataPaint(ref currentCell, gridColumnStyles[num3], num3);
					Brush brush2 = this.ForeBrushForDataPaint(ref currentCell, gridColumnStyles[num3], num3);
					this.PaintCellContents(g, rectangle, gridColumnStyles[num3], brush, brush2, alignToRight);
					if (num > 0)
					{
						g.FillRectangle(this.dgTable.IsDefault ? this.DataGrid.GridLineBrush : this.dgTable.GridLineBrush, alignToRight ? (rectangle.X - num) : rectangle.Right, rectangle.Y, num, rectangle.Height);
					}
					num2 += rectangle.Width + num;
				}
				num3++;
			}
			if (num2 < bounds.Width)
			{
				g.FillRectangle(this.dgTable.DataGrid.BackgroundBrush, alignToRight ? bounds.X : (bounds.X + num2), bounds.Y, bounds.Width - num2, bounds.Height);
			}
			return num2;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0006C2B9 File Offset: 0x0006B2B9
		protected virtual void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush)
		{
			this.PaintCellContents(g, cellBounds, column, backBr, foreBrush, false);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x0006C2C9 File Offset: 0x0006B2C9
		protected virtual void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			g.FillRectangle(backBr, cellBounds);
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0006C2D4 File Offset: 0x0006B2D4
		protected Rectangle PaintIcon(Graphics g, Rectangle visualBounds, bool paintIcon, bool alignToRight, Bitmap bmp)
		{
			return this.PaintIcon(g, visualBounds, paintIcon, alignToRight, bmp, this.dgTable.IsDefault ? this.DataGrid.HeaderBackBrush : this.dgTable.HeaderBackBrush);
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x0006C308 File Offset: 0x0006B308
		protected Rectangle PaintIcon(Graphics g, Rectangle visualBounds, bool paintIcon, bool alignToRight, Bitmap bmp, Brush backBrush)
		{
			Size size = bmp.Size;
			Rectangle rectangle = new Rectangle(alignToRight ? (visualBounds.Right - 3 - size.Width) : (visualBounds.X + 3), visualBounds.Y + 2, size.Width, size.Height);
			g.FillRectangle(backBrush, visualBounds);
			if (paintIcon)
			{
				DataGridRow.colorMap[0].NewColor = (this.dgTable.IsDefault ? this.DataGrid.HeaderForeColor : this.dgTable.HeaderForeColor);
				DataGridRow.colorMap[0].OldColor = Color.Black;
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetRemapTable(DataGridRow.colorMap, ColorAdjustType.Bitmap);
				g.DrawImage(bmp, rectangle, 0, 0, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, imageAttributes);
				imageAttributes.Dispose();
			}
			return rectangle;
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x0006C3DD File Offset: 0x0006B3DD
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds)
		{
			this.PaintHeader(g, visualBounds, false);
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x0006C3E8 File Offset: 0x0006B3E8
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds, bool alignToRight)
		{
			this.PaintHeader(g, visualBounds, alignToRight, false);
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x0006C3F4 File Offset: 0x0006B3F4
		public virtual void PaintHeader(Graphics g, Rectangle visualBounds, bool alignToRight, bool rowIsDirty)
		{
			Rectangle rectangle = visualBounds;
			Bitmap bitmap;
			if (this is DataGridAddNewRow)
			{
				bitmap = this.GetStarBitmap();
				lock (bitmap)
				{
					rectangle.X += this.PaintIcon(g, rectangle, true, alignToRight, bitmap).Width + 3;
				}
				return;
			}
			if (rowIsDirty)
			{
				bitmap = this.GetPencilBitmap();
				lock (bitmap)
				{
					rectangle.X += this.PaintIcon(g, rectangle, this.RowNumber == this.DataGrid.CurrentCell.RowNumber, alignToRight, bitmap).Width + 3;
					goto IL_010A;
				}
			}
			bitmap = (alignToRight ? this.GetLeftArrowBitmap() : this.GetRightArrowBitmap());
			lock (bitmap)
			{
				rectangle.X += this.PaintIcon(g, rectangle, this.RowNumber == this.DataGrid.CurrentCell.RowNumber, alignToRight, bitmap).Width + 3;
			}
			IL_010A:
			object obj = this.DataGrid.ListManager[this.number];
			if (!(obj is IDataErrorInfo))
			{
				return;
			}
			string text = ((IDataErrorInfo)obj).Error;
			if (text == null)
			{
				text = string.Empty;
			}
			if (this.tooltip != text && !string.IsNullOrEmpty(this.tooltip))
			{
				this.DataGrid.ToolTipProvider.RemoveToolTip(this.tooltipID);
				this.tooltip = string.Empty;
				this.tooltipID = new IntPtr(-1);
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			bitmap = this.GetErrorBitmap();
			Rectangle rectangle2;
			lock (bitmap)
			{
				rectangle2 = this.PaintIcon(g, rectangle, true, alignToRight, bitmap);
			}
			rectangle.X += rectangle2.Width + 3;
			this.tooltip = text;
			this.tooltipID = (IntPtr)(this.DataGrid.ToolTipId++);
			this.DataGrid.ToolTipProvider.AddToolTip(this.tooltip, this.tooltipID, rectangle2);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x0006C64C File Offset: 0x0006B64C
		protected Brush GetBackBrush()
		{
			Brush brush = (this.dgTable.IsDefault ? this.DataGrid.BackBrush : this.dgTable.BackBrush);
			if (this.DataGrid.LedgerStyle && this.RowNumber % 2 == 1)
			{
				brush = (this.dgTable.IsDefault ? this.DataGrid.AlternatingBackBrush : this.dgTable.AlternatingBackBrush);
			}
			return brush;
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x0006C6C0 File Offset: 0x0006B6C0
		protected Brush BackBrushForDataPaint(ref DataGridCell current, DataGridColumnStyle gridColumn, int column)
		{
			Brush brush = this.GetBackBrush();
			if (this.Selected)
			{
				brush = (this.dgTable.IsDefault ? this.DataGrid.SelectionBackBrush : this.dgTable.SelectionBackBrush);
			}
			return brush;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0006C704 File Offset: 0x0006B704
		protected Brush ForeBrushForDataPaint(ref DataGridCell current, DataGridColumnStyle gridColumn, int column)
		{
			Brush brush = (this.dgTable.IsDefault ? this.DataGrid.ForeBrush : this.dgTable.ForeBrush);
			if (this.Selected)
			{
				brush = (this.dgTable.IsDefault ? this.DataGrid.SelectionForeBrush : this.dgTable.SelectionForeBrush);
			}
			return brush;
		}

		// Token: 0x04001735 RID: 5941
		protected const int xOffset = 3;

		// Token: 0x04001736 RID: 5942
		protected const int yOffset = 2;

		// Token: 0x04001737 RID: 5943
		protected internal int number;

		// Token: 0x04001738 RID: 5944
		private bool selected;

		// Token: 0x04001739 RID: 5945
		private int height;

		// Token: 0x0400173A RID: 5946
		private IntPtr tooltipID = new IntPtr(-1);

		// Token: 0x0400173B RID: 5947
		private string tooltip = string.Empty;

		// Token: 0x0400173C RID: 5948
		private AccessibleObject accessibleObject;

		// Token: 0x0400173D RID: 5949
		protected DataGridTableStyle dgTable;

		// Token: 0x0400173E RID: 5950
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x0400173F RID: 5951
		private static Bitmap rightArrow = null;

		// Token: 0x04001740 RID: 5952
		private static Bitmap leftArrow = null;

		// Token: 0x04001741 RID: 5953
		private static Bitmap errorBmp = null;

		// Token: 0x04001742 RID: 5954
		private static Bitmap pencilBmp = null;

		// Token: 0x04001743 RID: 5955
		private static Bitmap starBmp = null;

		// Token: 0x020002C9 RID: 713
		[ComVisible(true)]
		protected class DataGridRowAccessibleObject : AccessibleObject
		{
			// Token: 0x060028FE RID: 10494 RVA: 0x0006C7A8 File Offset: 0x0006B7A8
			internal static string CellToDisplayString(DataGrid grid, int row, int column)
			{
				if (column < grid.myGridTable.GridColumnStyles.Count)
				{
					return grid.myGridTable.GridColumnStyles[column].PropertyDescriptor.Converter.ConvertToString(grid[row, column]);
				}
				return "";
			}

			// Token: 0x060028FF RID: 10495 RVA: 0x0006C7F6 File Offset: 0x0006B7F6
			internal static object DisplayStringToCell(DataGrid grid, int row, int column, string value)
			{
				if (column < grid.myGridTable.GridColumnStyles.Count)
				{
					return grid.myGridTable.GridColumnStyles[column].PropertyDescriptor.Converter.ConvertFromString(value);
				}
				return null;
			}

			// Token: 0x06002900 RID: 10496 RVA: 0x0006C82E File Offset: 0x0006B82E
			public DataGridRowAccessibleObject(DataGridRow owner)
			{
				this.owner = owner;
				DataGrid dataGrid = this.DataGrid;
				this.EnsureChildren();
			}

			// Token: 0x06002901 RID: 10497 RVA: 0x0006C84A File Offset: 0x0006B84A
			private void EnsureChildren()
			{
				if (this.cells == null)
				{
					this.cells = new ArrayList(this.DataGrid.myGridTable.GridColumnStyles.Count + 2);
					this.AddChildAccessibleObjects(this.cells);
				}
			}

			// Token: 0x06002902 RID: 10498 RVA: 0x0006C884 File Offset: 0x0006B884
			protected virtual void AddChildAccessibleObjects(IList children)
			{
				GridColumnStylesCollection gridColumnStyles = this.DataGrid.myGridTable.GridColumnStyles;
				int count = gridColumnStyles.Count;
				for (int i = 0; i < count; i++)
				{
					children.Add(this.CreateCellAccessibleObject(i));
				}
			}

			// Token: 0x06002903 RID: 10499 RVA: 0x0006C8C3 File Offset: 0x0006B8C3
			protected virtual AccessibleObject CreateCellAccessibleObject(int column)
			{
				return new DataGridRow.DataGridCellAccessibleObject(this.owner, column);
			}

			// Token: 0x170006B1 RID: 1713
			// (get) Token: 0x06002904 RID: 10500 RVA: 0x0006C8D1 File Offset: 0x0006B8D1
			public override Rectangle Bounds
			{
				get
				{
					return this.DataGrid.RectangleToScreen(this.DataGrid.GetRowBounds(this.owner));
				}
			}

			// Token: 0x170006B2 RID: 1714
			// (get) Token: 0x06002905 RID: 10501 RVA: 0x0006C8EF File Offset: 0x0006B8EF
			public override string Name
			{
				get
				{
					if (this.owner is DataGridAddNewRow)
					{
						return SR.GetString("AccDGNewRow");
					}
					return DataGridRow.DataGridRowAccessibleObject.CellToDisplayString(this.DataGrid, this.owner.RowNumber, 0);
				}
			}

			// Token: 0x170006B3 RID: 1715
			// (get) Token: 0x06002906 RID: 10502 RVA: 0x0006C920 File Offset: 0x0006B920
			protected DataGridRow Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x170006B4 RID: 1716
			// (get) Token: 0x06002907 RID: 10503 RVA: 0x0006C928 File Offset: 0x0006B928
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.DataGrid.AccessibilityObject;
				}
			}

			// Token: 0x170006B5 RID: 1717
			// (get) Token: 0x06002908 RID: 10504 RVA: 0x0006C935 File Offset: 0x0006B935
			private DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x170006B6 RID: 1718
			// (get) Token: 0x06002909 RID: 10505 RVA: 0x0006C942 File Offset: 0x0006B942
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Row;
				}
			}

			// Token: 0x170006B7 RID: 1719
			// (get) Token: 0x0600290A RID: 10506 RVA: 0x0006C948 File Offset: 0x0006B948
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.DataGrid.CurrentCell.RowNumber == this.owner.RowNumber)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					if (this.DataGrid.CurrentRowIndex == this.owner.RowNumber)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x170006B8 RID: 1720
			// (get) Token: 0x0600290B RID: 10507 RVA: 0x0006C99C File Offset: 0x0006B99C
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			// Token: 0x0600290C RID: 10508 RVA: 0x0006C9A4 File Offset: 0x0006B9A4
			public override AccessibleObject GetChild(int index)
			{
				if (index < this.cells.Count)
				{
					return (AccessibleObject)this.cells[index];
				}
				return null;
			}

			// Token: 0x0600290D RID: 10509 RVA: 0x0006C9C7 File Offset: 0x0006B9C7
			public override int GetChildCount()
			{
				return this.cells.Count;
			}

			// Token: 0x0600290E RID: 10510 RVA: 0x0006C9D4 File Offset: 0x0006B9D4
			public override AccessibleObject GetFocused()
			{
				if (this.DataGrid.Focused)
				{
					DataGridCell currentCell = this.DataGrid.CurrentCell;
					if (currentCell.RowNumber == this.owner.RowNumber)
					{
						return (AccessibleObject)this.cells[currentCell.ColumnNumber];
					}
				}
				return null;
			}

			// Token: 0x0600290F RID: 10511 RVA: 0x0006CA28 File Offset: 0x0006BA28
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1);
				case AccessibleNavigation.FirstChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(0);
					}
					break;
				case AccessibleNavigation.LastChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
					break;
				}
				return null;
			}

			// Token: 0x06002910 RID: 10512 RVA: 0x0006CAFA File Offset: 0x0006BAFA
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.DataGrid.CurrentRowIndex = this.owner.RowNumber;
				}
			}

			// Token: 0x04001744 RID: 5956
			private ArrayList cells;

			// Token: 0x04001745 RID: 5957
			private DataGridRow owner;
		}

		// Token: 0x020002CA RID: 714
		[ComVisible(true)]
		protected class DataGridCellAccessibleObject : AccessibleObject
		{
			// Token: 0x06002911 RID: 10513 RVA: 0x0006CB2A File Offset: 0x0006BB2A
			public DataGridCellAccessibleObject(DataGridRow owner, int column)
			{
				this.owner = owner;
				this.column = column;
			}

			// Token: 0x170006B9 RID: 1721
			// (get) Token: 0x06002912 RID: 10514 RVA: 0x0006CB40 File Offset: 0x0006BB40
			public override Rectangle Bounds
			{
				get
				{
					return this.DataGrid.RectangleToScreen(this.DataGrid.GetCellBounds(new DataGridCell(this.owner.RowNumber, this.column)));
				}
			}

			// Token: 0x170006BA RID: 1722
			// (get) Token: 0x06002913 RID: 10515 RVA: 0x0006CB6E File Offset: 0x0006BB6E
			public override string Name
			{
				get
				{
					return this.DataGrid.myGridTable.GridColumnStyles[this.column].HeaderText;
				}
			}

			// Token: 0x170006BB RID: 1723
			// (get) Token: 0x06002914 RID: 10516 RVA: 0x0006CB90 File Offset: 0x0006BB90
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.AccessibleObject;
				}
			}

			// Token: 0x170006BC RID: 1724
			// (get) Token: 0x06002915 RID: 10517 RVA: 0x0006CB9D File Offset: 0x0006BB9D
			protected DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x170006BD RID: 1725
			// (get) Token: 0x06002916 RID: 10518 RVA: 0x0006CBAA File Offset: 0x0006BBAA
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGEdit");
				}
			}

			// Token: 0x170006BE RID: 1726
			// (get) Token: 0x06002917 RID: 10519 RVA: 0x0006CBB6 File Offset: 0x0006BBB6
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Cell;
				}
			}

			// Token: 0x170006BF RID: 1727
			// (get) Token: 0x06002918 RID: 10520 RVA: 0x0006CBBC File Offset: 0x0006BBBC
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.DataGrid.CurrentCell.RowNumber == this.owner.RowNumber && this.DataGrid.CurrentCell.ColumnNumber == this.column)
					{
						if (this.DataGrid.Focused)
						{
							accessibleStates |= AccessibleStates.Focused;
						}
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x170006C0 RID: 1728
			// (get) Token: 0x06002919 RID: 10521 RVA: 0x0006CC20 File Offset: 0x0006BC20
			// (set) Token: 0x0600291A RID: 10522 RVA: 0x0006CC50 File Offset: 0x0006BC50
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner is DataGridAddNewRow)
					{
						return null;
					}
					return DataGridRow.DataGridRowAccessibleObject.CellToDisplayString(this.DataGrid, this.owner.RowNumber, this.column);
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					if (!(this.owner is DataGridAddNewRow))
					{
						object obj = DataGridRow.DataGridRowAccessibleObject.DisplayStringToCell(this.DataGrid, this.owner.RowNumber, this.column, value);
						this.DataGrid[this.owner.RowNumber, this.column] = obj;
					}
				}
			}

			// Token: 0x0600291B RID: 10523 RVA: 0x0006CCA5 File Offset: 0x0006BCA5
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.Select(AccessibleSelection.TakeFocus | AccessibleSelection.TakeSelection);
			}

			// Token: 0x0600291C RID: 10524 RVA: 0x0006CCAE File Offset: 0x0006BCAE
			public override AccessibleObject GetFocused()
			{
				return this.DataGrid.AccessibilityObject.GetFocused();
			}

			// Token: 0x0600291D RID: 10525 RVA: 0x0006CCC0 File Offset: 0x0006BCC0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1).Navigate(AccessibleNavigation.FirstChild);
				case AccessibleNavigation.Down:
					return this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1).Navigate(AccessibleNavigation.FirstChild);
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
				{
					if (this.column > 0)
					{
						return this.owner.AccessibleObject.GetChild(this.column - 1);
					}
					AccessibleObject child = this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber - 1);
					if (child != null)
					{
						return child.Navigate(AccessibleNavigation.LastChild);
					}
					break;
				}
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
				{
					if (this.column < this.owner.AccessibleObject.GetChildCount() - 1)
					{
						return this.owner.AccessibleObject.GetChild(this.column + 1);
					}
					AccessibleObject child2 = this.DataGrid.AccessibilityObject.GetChild(1 + this.owner.dgTable.GridColumnStyles.Count + this.owner.RowNumber + 1);
					if (child2 != null)
					{
						return child2.Navigate(AccessibleNavigation.FirstChild);
					}
					break;
				}
				}
				return null;
			}

			// Token: 0x0600291E RID: 10526 RVA: 0x0006CE4F File Offset: 0x0006BE4F
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.DataGrid.CurrentCell = new DataGridCell(this.owner.RowNumber, this.column);
				}
			}

			// Token: 0x04001746 RID: 5958
			private DataGridRow owner;

			// Token: 0x04001747 RID: 5959
			private int column;
		}
	}
}
