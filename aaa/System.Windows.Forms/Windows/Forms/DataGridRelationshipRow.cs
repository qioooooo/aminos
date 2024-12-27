using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002DC RID: 732
	internal class DataGridRelationshipRow : DataGridRow
	{
		// Token: 0x06002A77 RID: 10871 RVA: 0x0007126A File Offset: 0x0007026A
		public DataGridRelationshipRow(DataGrid dataGrid, DataGridTableStyle dgTable, int rowNumber)
			: base(dataGrid, dgTable, rowNumber)
		{
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x00071278 File Offset: 0x00070278
		protected internal override int MinimumRowHeight(GridColumnStylesCollection cols)
		{
			return base.MinimumRowHeight(cols) + (this.expanded ? this.GetRelationshipRect().Height : 0);
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000712A8 File Offset: 0x000702A8
		protected internal override int MinimumRowHeight(DataGridTableStyle dgTable)
		{
			return base.MinimumRowHeight(dgTable) + (this.expanded ? this.GetRelationshipRect().Height : 0);
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000712D6 File Offset: 0x000702D6
		// (set) Token: 0x06002A7B RID: 10875 RVA: 0x000712DE File Offset: 0x000702DE
		public virtual bool Expanded
		{
			get
			{
				return this.expanded;
			}
			set
			{
				if (this.expanded == value)
				{
					return;
				}
				if (this.expanded)
				{
					this.Collapse();
					return;
				}
				this.Expand();
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x000712FF File Offset: 0x000702FF
		// (set) Token: 0x06002A7D RID: 10877 RVA: 0x0007130C File Offset: 0x0007030C
		private int FocusedRelation
		{
			get
			{
				return this.dgTable.FocusedRelation;
			}
			set
			{
				this.dgTable.FocusedRelation = value;
			}
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x0007131A File Offset: 0x0007031A
		private void Collapse()
		{
			if (this.expanded)
			{
				this.expanded = false;
				this.FocusedRelation = -1;
				base.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x0007133E File Offset: 0x0007033E
		protected override AccessibleObject CreateAccessibleObject()
		{
			return new DataGridRelationshipRow.DataGridRelationshipRowAccessibleObject(this);
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x00071348 File Offset: 0x00070348
		private void Expand()
		{
			if (!this.expanded && base.DataGrid != null && this.dgTable != null && this.dgTable.RelationsList.Count > 0)
			{
				this.expanded = true;
				this.FocusedRelation = -1;
				base.DataGrid.OnRowHeightChanged(this);
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x0007139C File Offset: 0x0007039C
		// (set) Token: 0x06002A82 RID: 10882 RVA: 0x000713CC File Offset: 0x000703CC
		public override int Height
		{
			get
			{
				int height = base.Height;
				if (this.expanded)
				{
					return height + this.GetRelationshipRect().Height;
				}
				return height;
			}
			set
			{
				if (this.expanded)
				{
					base.Height = value - this.GetRelationshipRect().Height;
					return;
				}
				base.Height = value;
			}
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00071400 File Offset: 0x00070400
		public override Rectangle GetCellBounds(int col)
		{
			Rectangle cellBounds = base.GetCellBounds(col);
			cellBounds.Height = base.Height - 1;
			return cellBounds;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x00071428 File Offset: 0x00070428
		private Rectangle GetOutlineRect(int xOrigin, int yOrigin)
		{
			Rectangle rectangle = new Rectangle(xOrigin + 2, yOrigin + 2, 9, 9);
			return rectangle;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00071447 File Offset: 0x00070447
		public override Rectangle GetNonScrollableArea()
		{
			if (this.expanded)
			{
				return this.GetRelationshipRect();
			}
			return Rectangle.Empty;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x00071460 File Offset: 0x00070460
		private Rectangle GetRelationshipRect()
		{
			Rectangle relationshipRect = this.dgTable.RelationshipRect;
			relationshipRect.Y = base.Height - this.dgTable.BorderWidth;
			return relationshipRect;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x00071494 File Offset: 0x00070494
		private Rectangle GetRelationshipRectWithMirroring()
		{
			Rectangle relationshipRect = this.GetRelationshipRect();
			bool flag = (this.dgTable.IsDefault ? base.DataGrid.RowHeadersVisible : this.dgTable.RowHeadersVisible);
			if (flag)
			{
				int num = (this.dgTable.IsDefault ? base.DataGrid.RowHeaderWidth : this.dgTable.RowHeaderWidth);
				relationshipRect.X += base.DataGrid.GetRowHeaderRect().X + num;
			}
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, base.DataGrid.GetRowHeaderRect(), base.DataGrid.RightToLeft == RightToLeft.Yes);
			return relationshipRect;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00071544 File Offset: 0x00070544
		private bool PointOverPlusMinusGlyph(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			if (this.dgTable == null || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
			{
				return false;
			}
			Rectangle rectangle = rowHeaders;
			if (!base.DataGrid.FlatMode)
			{
				rectangle.Inflate(-1, -1);
			}
			Rectangle outlineRect = this.GetOutlineRect(rectangle.Right - 14, 0);
			outlineRect.X = this.MirrorRectangle(outlineRect.X, outlineRect.Width, rectangle, alignToRight);
			return outlineRect.Contains(x, y);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000715CC File Offset: 0x000705CC
		public override bool OnMouseDown(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			bool flag = (this.dgTable.IsDefault ? base.DataGrid.RowHeadersVisible : this.dgTable.RowHeadersVisible);
			if (flag && this.PointOverPlusMinusGlyph(x, y, rowHeaders, alignToRight))
			{
				if (this.dgTable.RelationsList.Count == 0)
				{
					return false;
				}
				if (this.expanded)
				{
					this.Collapse();
				}
				else
				{
					this.Expand();
				}
				base.DataGrid.OnNodeClick(EventArgs.Empty);
				return true;
			}
			else
			{
				if (!this.expanded)
				{
					return base.OnMouseDown(x, y, rowHeaders, alignToRight);
				}
				if (this.GetRelationshipRectWithMirroring().Contains(x, y))
				{
					int num = this.RelationFromY(y);
					if (num != -1)
					{
						this.FocusedRelation = -1;
						base.DataGrid.NavigateTo((string)this.dgTable.RelationsList[num], this, true);
					}
					return true;
				}
				return base.OnMouseDown(x, y, rowHeaders, alignToRight);
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000716B4 File Offset: 0x000706B4
		public override bool OnMouseMove(int x, int y, Rectangle rowHeaders, bool alignToRight)
		{
			if (!this.expanded)
			{
				return false;
			}
			if (this.GetRelationshipRectWithMirroring().Contains(x, y))
			{
				base.DataGrid.Cursor = Cursors.Hand;
				return true;
			}
			base.DataGrid.Cursor = Cursors.Default;
			return base.OnMouseMove(x, y, rowHeaders, alignToRight);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x0007170C File Offset: 0x0007070C
		public override void OnMouseLeft(Rectangle rowHeaders, bool alignToRight)
		{
			if (!this.expanded)
			{
				return;
			}
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
			if (this.FocusedRelation != -1)
			{
				this.InvalidateRowRect(relationshipRect);
				this.FocusedRelation = -1;
			}
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x00071771 File Offset: 0x00070771
		public override void OnMouseLeft()
		{
			if (!this.expanded)
			{
				return;
			}
			if (this.FocusedRelation != -1)
			{
				this.InvalidateRow();
				this.FocusedRelation = -1;
			}
			base.OnMouseLeft();
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00071798 File Offset: 0x00070798
		public override bool OnKeyPress(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) == Keys.Shift && (keyData & Keys.KeyCode) != Keys.Tab)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Return)
			{
				if (keys == Keys.Tab)
				{
					return false;
				}
				if (keys == Keys.Return)
				{
					if (this.FocusedRelation != -1)
					{
						base.DataGrid.NavigateTo((string)this.dgTable.RelationsList[this.FocusedRelation], this, true);
						this.FocusedRelation = -1;
						return true;
					}
					return false;
				}
			}
			else if (keys != Keys.F5)
			{
				if (keys == Keys.NumLock)
				{
					return this.FocusedRelation == -1 && base.OnKeyPress(keyData);
				}
			}
			else
			{
				if (this.dgTable == null || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
				{
					return false;
				}
				if (this.expanded)
				{
					this.Collapse();
				}
				else
				{
					this.Expand();
				}
				this.FocusedRelation = -1;
				return true;
			}
			this.FocusedRelation = -1;
			return base.OnKeyPress(keyData);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x00071898 File Offset: 0x00070898
		internal override void LoseChildFocus(Rectangle rowHeaders, bool alignToRight)
		{
			if (this.FocusedRelation == -1 || !this.expanded)
			{
				return;
			}
			this.FocusedRelation = -1;
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
			relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
			this.InvalidateRowRect(relationshipRect);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x00071900 File Offset: 0x00070900
		internal override bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight)
		{
			if (this.dgTable.RelationsList.Count == 0 || this.dgTable.DataGrid == null || !this.dgTable.DataGrid.AllowNavigation)
			{
				return false;
			}
			if (!this.expanded)
			{
				this.Expand();
			}
			if ((keyData & Keys.Shift) == Keys.Shift)
			{
				if (this.FocusedRelation == 0)
				{
					this.FocusedRelation = -1;
					return false;
				}
				Rectangle relationshipRect = this.GetRelationshipRect();
				relationshipRect.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
				relationshipRect.X = this.MirrorRelationshipRectangle(relationshipRect, rowHeaders, alignToRight);
				this.InvalidateRowRect(relationshipRect);
				if (this.FocusedRelation == -1)
				{
					this.FocusedRelation = this.dgTable.RelationsList.Count - 1;
				}
				else
				{
					this.FocusedRelation--;
				}
				return true;
			}
			else
			{
				if (this.FocusedRelation == this.dgTable.RelationsList.Count - 1)
				{
					this.FocusedRelation = -1;
					return false;
				}
				Rectangle relationshipRect2 = this.GetRelationshipRect();
				relationshipRect2.X += rowHeaders.X + this.dgTable.RowHeaderWidth;
				relationshipRect2.X = this.MirrorRelationshipRectangle(relationshipRect2, rowHeaders, alignToRight);
				this.InvalidateRowRect(relationshipRect2);
				this.FocusedRelation++;
				return true;
			}
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x00071A53 File Offset: 0x00070A53
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int numVisibleColumns)
		{
			return this.Paint(g, bounds, trueRowBounds, firstVisibleColumn, numVisibleColumns, false);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00071A64 File Offset: 0x00070A64
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int numVisibleColumns, bool alignToRight)
		{
			bool traceVerbose = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			int borderWidth = this.dgTable.BorderWidth;
			Rectangle rectangle = bounds;
			rectangle.Height = base.Height - borderWidth;
			int num = this.PaintData(g, rectangle, firstVisibleColumn, numVisibleColumns, alignToRight);
			int num2 = num + bounds.X - trueRowBounds.X;
			rectangle.Offset(0, borderWidth);
			if (borderWidth > 0)
			{
				this.PaintBottomBorder(g, rectangle, num, borderWidth, alignToRight);
			}
			if (this.expanded && this.dgTable.RelationsList.Count > 0)
			{
				Rectangle rectangle2 = new Rectangle(trueRowBounds.X, rectangle.Bottom, trueRowBounds.Width, trueRowBounds.Height - rectangle.Height - 2 * borderWidth);
				this.PaintRelations(g, rectangle2, trueRowBounds, num2, firstVisibleColumn, numVisibleColumns, alignToRight);
				rectangle2.Height++;
				if (borderWidth > 0)
				{
					this.PaintBottomBorder(g, rectangle2, num2, borderWidth, alignToRight);
				}
			}
			return num;
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x00071B50 File Offset: 0x00070B50
		protected override void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			CurrencyManager listManager = base.DataGrid.ListManager;
			string text = string.Empty;
			Rectangle rectangle = cellBounds;
			object obj = base.DataGrid.ListManager[this.number];
			if (obj is IDataErrorInfo)
			{
				text = ((IDataErrorInfo)obj)[column.PropertyDescriptor.Name];
			}
			if (!string.IsNullOrEmpty(text))
			{
				Bitmap errorBitmap = base.GetErrorBitmap();
				Rectangle rectangle2;
				lock (errorBitmap)
				{
					rectangle2 = base.PaintIcon(g, rectangle, true, alignToRight, errorBitmap, backBr);
				}
				if (alignToRight)
				{
					rectangle.Width -= rectangle2.Width + 3;
				}
				else
				{
					rectangle.X += rectangle2.Width + 3;
				}
				base.DataGrid.ToolTipProvider.AddToolTip(text, (IntPtr)(base.DataGrid.ToolTipId++), rectangle2);
			}
			column.Paint(g, rectangle, listManager, base.RowNumber, backBr, foreBrush, alignToRight);
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00071C68 File Offset: 0x00070C68
		public override void PaintHeader(Graphics g, Rectangle bounds, bool alignToRight, bool isDirty)
		{
			DataGrid dataGrid = base.DataGrid;
			Rectangle rectangle = bounds;
			if (!dataGrid.FlatMode)
			{
				ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
				rectangle.Inflate(-1, -1);
			}
			if (this.dgTable.IsDefault)
			{
				this.PaintHeaderInside(g, rectangle, base.DataGrid.HeaderBackBrush, alignToRight, isDirty);
				return;
			}
			this.PaintHeaderInside(g, rectangle, this.dgTable.HeaderBackBrush, alignToRight, isDirty);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00071CD4 File Offset: 0x00070CD4
		public void PaintHeaderInside(Graphics g, Rectangle bounds, Brush backBr, bool alignToRight, bool isDirty)
		{
			bool flag = this.dgTable.RelationsList.Count > 0 && this.dgTable.DataGrid.AllowNavigation;
			int num = this.MirrorRectangle(bounds.X, bounds.Width - (flag ? 14 : 0), bounds, alignToRight);
			Rectangle rectangle = new Rectangle(num, bounds.Y, bounds.Width - (flag ? 14 : 0), bounds.Height);
			base.PaintHeader(g, rectangle, alignToRight, isDirty);
			int num2 = this.MirrorRectangle(bounds.X + rectangle.Width, 14, bounds, alignToRight);
			Rectangle rectangle2 = new Rectangle(num2, bounds.Y, 14, bounds.Height);
			if (flag)
			{
				this.PaintPlusMinusGlyph(g, rectangle2, backBr, alignToRight);
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x00071DA4 File Offset: 0x00070DA4
		private void PaintRelations(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int dataWidth, int firstCol, int nCols, bool alignToRight)
		{
			Rectangle relationshipRect = this.GetRelationshipRect();
			relationshipRect.X = (alignToRight ? (bounds.Right - relationshipRect.Width) : bounds.X);
			relationshipRect.Y = bounds.Y;
			int num = Math.Max(dataWidth, relationshipRect.Width);
			Region clip = g.Clip;
			g.ExcludeClip(relationshipRect);
			g.FillRectangle(base.GetBackBrush(), alignToRight ? (bounds.Right - dataWidth) : bounds.X, bounds.Y, dataWidth, bounds.Height);
			g.SetClip(bounds);
			relationshipRect.Height -= this.dgTable.BorderWidth;
			g.DrawRectangle(SystemPens.ControlText, relationshipRect.X, relationshipRect.Y, relationshipRect.Width - 1, relationshipRect.Height - 1);
			relationshipRect.Inflate(-1, -1);
			int num2 = this.PaintRelationText(g, relationshipRect, alignToRight);
			if (num2 < relationshipRect.Height)
			{
				g.FillRectangle(base.GetBackBrush(), relationshipRect.X, relationshipRect.Y + num2, relationshipRect.Width, relationshipRect.Height - num2);
			}
			g.Clip = clip;
			if (num < bounds.Width)
			{
				int num3;
				if (this.dgTable.IsDefault)
				{
					num3 = base.DataGrid.GridLineWidth;
				}
				else
				{
					num3 = this.dgTable.GridLineWidth;
				}
				g.FillRectangle(base.DataGrid.BackgroundBrush, alignToRight ? bounds.X : (bounds.X + num), bounds.Y, bounds.Width - num - num3 + 1, bounds.Height);
				if (num3 > 0)
				{
					Brush brush;
					if (this.dgTable.IsDefault)
					{
						brush = base.DataGrid.GridLineBrush;
					}
					else
					{
						brush = this.dgTable.GridLineBrush;
					}
					g.FillRectangle(brush, alignToRight ? (bounds.Right - num3 - num) : (bounds.X + num - num3), bounds.Y, num3, bounds.Height);
				}
			}
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x00071FB4 File Offset: 0x00070FB4
		private int PaintRelationText(Graphics g, Rectangle bounds, bool alignToRight)
		{
			g.FillRectangle(base.GetBackBrush(), bounds.X, bounds.Y, bounds.Width, 1);
			int relationshipHeight = this.dgTable.RelationshipHeight;
			Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, relationshipHeight);
			int num = 1;
			int num2 = 0;
			while (num2 < this.dgTable.RelationsList.Count && num <= bounds.Height)
			{
				Brush brush = (this.dgTable.IsDefault ? base.DataGrid.LinkBrush : this.dgTable.LinkBrush);
				Font font = base.DataGrid.Font;
				Brush brush2 = (this.dgTable.IsDefault ? base.DataGrid.LinkBrush : this.dgTable.LinkBrush);
				font = base.DataGrid.LinkFont;
				g.FillRectangle(base.GetBackBrush(), rectangle);
				StringFormat stringFormat = new StringFormat();
				if (alignToRight)
				{
					stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
					stringFormat.Alignment = StringAlignment.Far;
				}
				g.DrawString((string)this.dgTable.RelationsList[num2], font, brush2, rectangle, stringFormat);
				if (num2 == this.FocusedRelation && this.number == base.DataGrid.CurrentCell.RowNumber)
				{
					rectangle.Width = this.dgTable.FocusedTextWidth;
					ControlPaint.DrawFocusRectangle(g, rectangle, ((SolidBrush)brush2).Color, ((SolidBrush)base.GetBackBrush()).Color);
					rectangle.Width = bounds.Width;
				}
				stringFormat.Dispose();
				rectangle.Y += relationshipHeight;
				num += rectangle.Height;
				num2++;
			}
			return num;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x00072188 File Offset: 0x00071188
		private void PaintPlusMinusGlyph(Graphics g, Rectangle bounds, Brush backBr, bool alignToRight)
		{
			bool traceVerbose = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			Rectangle rectangle = this.GetOutlineRect(bounds.X, bounds.Y);
			rectangle = Rectangle.Intersect(bounds, rectangle);
			if (rectangle.IsEmpty)
			{
				return;
			}
			g.FillRectangle(backBr, bounds);
			bool traceVerbose2 = CompModSwitches.DGRelationShpRowPaint.TraceVerbose;
			Pen pen = (this.dgTable.IsDefault ? base.DataGrid.HeaderForePen : this.dgTable.HeaderForePen);
			g.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			int num = 2;
			g.DrawLine(pen, rectangle.X + num, rectangle.Y + rectangle.Width / 2, rectangle.Right - num - 1, rectangle.Y + rectangle.Width / 2);
			if (!this.expanded)
			{
				g.DrawLine(pen, rectangle.X + rectangle.Height / 2, rectangle.Y + num, rectangle.X + rectangle.Height / 2, rectangle.Bottom - num - 1);
				return;
			}
			Point[] array = new Point[3];
			array[0] = new Point(rectangle.X + rectangle.Height / 2, rectangle.Bottom);
			array[1] = new Point(array[0].X, bounds.Y + 2 * num + base.Height);
			array[2] = new Point(alignToRight ? bounds.X : bounds.Right, array[1].Y);
			g.DrawLines(pen, array);
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x00072344 File Offset: 0x00071344
		private int RelationFromY(int y)
		{
			int num = -1;
			int relationshipHeight = this.dgTable.RelationshipHeight;
			Rectangle relationshipRect = this.GetRelationshipRect();
			int num2 = base.Height - this.dgTable.BorderWidth + 1;
			while (num2 < relationshipRect.Bottom && num2 <= y)
			{
				num2 += relationshipHeight;
				num++;
			}
			if (num >= this.dgTable.RelationsList.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000723A9 File Offset: 0x000713A9
		private int MirrorRelationshipRectangle(Rectangle relRect, Rectangle rowHeader, bool alignToRight)
		{
			if (alignToRight)
			{
				return rowHeader.X - relRect.Width;
			}
			return relRect.X;
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000723C5 File Offset: 0x000713C5
		private int MirrorRectangle(int x, int width, Rectangle rect, bool alignToRight)
		{
			if (alignToRight)
			{
				return rect.Right + rect.X - width - x;
			}
			return x;
		}

		// Token: 0x040017BA RID: 6074
		private const bool defaultOpen = false;

		// Token: 0x040017BB RID: 6075
		private const int expandoBoxWidth = 14;

		// Token: 0x040017BC RID: 6076
		private const int indentWidth = 20;

		// Token: 0x040017BD RID: 6077
		private const int triangleSize = 5;

		// Token: 0x040017BE RID: 6078
		private bool expanded;

		// Token: 0x020002DD RID: 733
		[ComVisible(true)]
		protected class DataGridRelationshipRowAccessibleObject : DataGridRow.DataGridRowAccessibleObject
		{
			// Token: 0x06002A9B RID: 10907 RVA: 0x000723E0 File Offset: 0x000713E0
			public DataGridRelationshipRowAccessibleObject(DataGridRow owner)
				: base(owner)
			{
			}

			// Token: 0x06002A9C RID: 10908 RVA: 0x000723EC File Offset: 0x000713EC
			protected override void AddChildAccessibleObjects(IList children)
			{
				base.AddChildAccessibleObjects(children);
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)base.Owner;
				if (dataGridRelationshipRow.dgTable.RelationsList != null)
				{
					for (int i = 0; i < dataGridRelationshipRow.dgTable.RelationsList.Count; i++)
					{
						children.Add(new DataGridRelationshipRow.DataGridRelationshipAccessibleObject(dataGridRelationshipRow, i));
					}
				}
			}

			// Token: 0x17000707 RID: 1799
			// (get) Token: 0x06002A9D RID: 10909 RVA: 0x00072442 File Offset: 0x00071442
			private DataGridRelationshipRow Row
			{
				get
				{
					return (DataGridRelationshipRow)base.Owner;
				}
			}

			// Token: 0x17000708 RID: 1800
			// (get) Token: 0x06002A9E RID: 10910 RVA: 0x0007244F File Offset: 0x0007144F
			public override string DefaultAction
			{
				get
				{
					if (this.Row.dgTable.RelationsList.Count <= 0)
					{
						return null;
					}
					if (this.Row.Expanded)
					{
						return SR.GetString("AccDGCollapse");
					}
					return SR.GetString("AccDGExpand");
				}
			}

			// Token: 0x17000709 RID: 1801
			// (get) Token: 0x06002A9F RID: 10911 RVA: 0x00072490 File Offset: 0x00071490
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					if (this.Row.dgTable.RelationsList.Count > 0)
					{
						if (((DataGridRelationshipRow)base.Owner).Expanded)
						{
							accessibleStates |= AccessibleStates.Expanded;
						}
						else
						{
							accessibleStates |= AccessibleStates.Collapsed;
						}
					}
					return accessibleStates;
				}
			}

			// Token: 0x06002AA0 RID: 10912 RVA: 0x000724E1 File Offset: 0x000714E1
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.Row.dgTable.RelationsList.Count > 0)
				{
					((DataGridRelationshipRow)base.Owner).Expanded = !((DataGridRelationshipRow)base.Owner).Expanded;
				}
			}

			// Token: 0x06002AA1 RID: 10913 RVA: 0x00072520 File Offset: 0x00071520
			public override AccessibleObject GetFocused()
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)base.Owner;
				int focusedRelation = dataGridRelationshipRow.dgTable.FocusedRelation;
				if (focusedRelation == -1)
				{
					return base.GetFocused();
				}
				return this.GetChild(this.GetChildCount() - dataGridRelationshipRow.dgTable.RelationsList.Count + focusedRelation);
			}
		}

		// Token: 0x020002DE RID: 734
		[ComVisible(true)]
		protected class DataGridRelationshipAccessibleObject : AccessibleObject
		{
			// Token: 0x06002AA2 RID: 10914 RVA: 0x0007256F File Offset: 0x0007156F
			public DataGridRelationshipAccessibleObject(DataGridRelationshipRow owner, int relationship)
			{
				this.owner = owner;
				this.relationship = relationship;
			}

			// Token: 0x1700070A RID: 1802
			// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x00072588 File Offset: 0x00071588
			public override Rectangle Bounds
			{
				get
				{
					Rectangle rowBounds = this.DataGrid.GetRowBounds(this.owner);
					Rectangle rectangle = (this.owner.Expanded ? this.owner.GetRelationshipRectWithMirroring() : Rectangle.Empty);
					rectangle.Y += this.owner.dgTable.RelationshipHeight * this.relationship;
					rectangle.Height = (this.owner.Expanded ? this.owner.dgTable.RelationshipHeight : 0);
					if (!this.owner.Expanded)
					{
						rectangle.X += rowBounds.X;
					}
					rectangle.Y += rowBounds.Y;
					return this.owner.DataGrid.RectangleToScreen(rectangle);
				}
			}

			// Token: 0x1700070B RID: 1803
			// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x0007265B File Offset: 0x0007165B
			public override string Name
			{
				get
				{
					return (string)this.owner.dgTable.RelationsList[this.relationship];
				}
			}

			// Token: 0x1700070C RID: 1804
			// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x0007267D File Offset: 0x0007167D
			protected DataGridRelationshipRow Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x1700070D RID: 1805
			// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x00072685 File Offset: 0x00071685
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.AccessibleObject;
				}
			}

			// Token: 0x1700070E RID: 1806
			// (get) Token: 0x06002AA7 RID: 10919 RVA: 0x00072692 File Offset: 0x00071692
			protected DataGrid DataGrid
			{
				get
				{
					return this.owner.DataGrid;
				}
			}

			// Token: 0x1700070F RID: 1807
			// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x0007269F File Offset: 0x0007169F
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Link;
				}
			}

			// Token: 0x17000710 RID: 1808
			// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x000726A4 File Offset: 0x000716A4
			public override AccessibleStates State
			{
				get
				{
					DataGridRow[] dataGridRows = this.DataGrid.DataGridRows;
					if (Array.IndexOf(dataGridRows, this.owner) == -1)
					{
						return AccessibleStates.Unavailable;
					}
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable | AccessibleStates.Linked;
					if (!this.owner.Expanded)
					{
						accessibleStates |= AccessibleStates.Invisible;
					}
					if (this.DataGrid.Focused && this.Owner.dgTable.FocusedRelation == this.relationship)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000711 RID: 1809
			// (get) Token: 0x06002AAA RID: 10922 RVA: 0x00072714 File Offset: 0x00071714
			// (set) Token: 0x06002AAB RID: 10923 RVA: 0x0007275E File Offset: 0x0007175E
			public override string Value
			{
				get
				{
					DataGridRow[] dataGridRows = this.DataGrid.DataGridRows;
					if (Array.IndexOf(dataGridRows, this.owner) == -1)
					{
						return null;
					}
					return (string)this.owner.dgTable.RelationsList[this.relationship];
				}
				set
				{
				}
			}

			// Token: 0x17000712 RID: 1810
			// (get) Token: 0x06002AAC RID: 10924 RVA: 0x00072760 File Offset: 0x00071760
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGNavigate");
				}
			}

			// Token: 0x06002AAD RID: 10925 RVA: 0x0007276C File Offset: 0x0007176C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.Owner.Expanded = true;
				this.owner.FocusedRelation = -1;
				this.DataGrid.NavigateTo((string)this.owner.dgTable.RelationsList[this.relationship], this.owner, true);
				this.DataGrid.BeginInvoke(new MethodInvoker(this.ResetAccessibilityLayer));
			}

			// Token: 0x06002AAE RID: 10926 RVA: 0x000727DC File Offset: 0x000717DC
			private void ResetAccessibilityLayer()
			{
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Reorder, 0);
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Focus, this.DataGrid.CurrentCellAccIndex);
				((DataGrid.DataGridAccessibleObject)this.DataGrid.AccessibilityObject).NotifyClients(AccessibleEvents.Selection, this.DataGrid.CurrentCellAccIndex);
			}

			// Token: 0x06002AAF RID: 10927 RVA: 0x00072850 File Offset: 0x00071850
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					if (this.relationship > 0)
					{
						return this.Parent.GetChild(this.Parent.GetChildCount() - this.owner.dgTable.RelationsList.Count + this.relationship - 1);
					}
					break;
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					if (this.relationship + 1 < this.owner.dgTable.RelationsList.Count)
					{
						return this.Parent.GetChild(this.Parent.GetChildCount() - this.owner.dgTable.RelationsList.Count + this.relationship + 1);
					}
					break;
				}
				return null;
			}

			// Token: 0x06002AB0 RID: 10928 RVA: 0x00072919 File Offset: 0x00071919
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.DataGrid.Focus();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.Owner.FocusedRelation = this.relationship;
				}
			}

			// Token: 0x040017BF RID: 6079
			private DataGridRelationshipRow owner;

			// Token: 0x040017C0 RID: 6080
			private int relationship;
		}
	}
}
