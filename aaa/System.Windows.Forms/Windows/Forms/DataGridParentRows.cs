using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020002D8 RID: 728
	internal class DataGridParentRows
	{
		// Token: 0x06002A2C RID: 10796 RVA: 0x0006F91C File Offset: 0x0006E91C
		internal DataGridParentRows(DataGrid dataGrid)
		{
			this.colorMap[0].OldColor = Color.Black;
			this.dataGrid = dataGrid;
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x0006F9B7 File Offset: 0x0006E9B7
		public AccessibleObject AccessibleObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = new DataGridParentRows.DataGridParentRowsAccessibleObject(this);
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x0006F9D3 File Offset: 0x0006E9D3
		// (set) Token: 0x06002A2F RID: 10799 RVA: 0x0006F9E0 File Offset: 0x0006E9E0
		internal Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "Parent Rows BackColor" }));
				}
				if (value != this.backBrush.Color)
				{
					this.backBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x0006FA3B File Offset: 0x0006EA3B
		// (set) Token: 0x06002A31 RID: 10801 RVA: 0x0006FA43 File Offset: 0x0006EA43
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
			set
			{
				if (value != this.backBrush)
				{
					this.CheckNull(value, "BackBrush");
					this.backBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x0006FA67 File Offset: 0x0006EA67
		// (set) Token: 0x06002A33 RID: 10803 RVA: 0x0006FA6F File Offset: 0x0006EA6F
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
			set
			{
				if (value != this.foreBrush)
				{
					this.CheckNull(value, "BackBrush");
					this.foreBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x0006FA94 File Offset: 0x0006EA94
		internal Rectangle GetBoundsForDataGridStateAccesibility(DataGridState dgs)
		{
			Rectangle empty = Rectangle.Empty;
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				int num2 = (int)this.rowHeights[i];
				if (this.parents[i] == dgs)
				{
					empty.X = (this.layout.leftArrow.IsEmpty ? this.layout.data.X : this.layout.leftArrow.Right);
					empty.Height = num2;
					empty.Y = num;
					empty.Width = this.layout.data.Width;
					return empty;
				}
				num += num2;
			}
			return empty;
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x0006FB4A File Offset: 0x0006EB4A
		// (set) Token: 0x06002A36 RID: 10806 RVA: 0x0006FB52 File Offset: 0x0006EB52
		internal Brush BorderBrush
		{
			get
			{
				return this.borderBrush;
			}
			set
			{
				if (value != this.borderBrush)
				{
					this.borderBrush = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002A37 RID: 10807 RVA: 0x0006FB6A File Offset: 0x0006EB6A
		internal int Height
		{
			get
			{
				return this.totalHeight;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x0006FB72 File Offset: 0x0006EB72
		// (set) Token: 0x06002A39 RID: 10809 RVA: 0x0006FB80 File Offset: 0x0006EB80
		internal Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "Parent Rows ForeColor" }));
				}
				if (value != this.foreBrush.Color)
				{
					this.foreBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x0006FBDB File Offset: 0x0006EBDB
		// (set) Token: 0x06002A3B RID: 10811 RVA: 0x0006FBE8 File Offset: 0x0006EBE8
		internal bool Visible
		{
			get
			{
				return this.dataGrid.ParentRowsVisible;
			}
			set
			{
				this.dataGrid.ParentRowsVisible = value;
			}
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x0006FBF6 File Offset: 0x0006EBF6
		internal void AddParent(DataGridState dgs)
		{
			CurrencyManager currencyManager = (CurrencyManager)this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember];
			this.parents.Add(dgs);
			this.SetParentCount(this.parentsCount + 1);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x0006FC38 File Offset: 0x0006EC38
		internal void Clear()
		{
			for (int i = 0; i < this.parents.Count; i++)
			{
				DataGridState dataGridState = this.parents[i] as DataGridState;
				dataGridState.RemoveChangeNotification();
			}
			this.parents.Clear();
			this.rowHeights.Clear();
			this.totalHeight = 0;
			this.SetParentCount(0);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x0006FC97 File Offset: 0x0006EC97
		internal void SetParentCount(int count)
		{
			this.parentsCount = count;
			this.dataGrid.Caption.BackButtonVisible = this.parentsCount > 0 && this.dataGrid.AllowNavigation;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x0006FCC7 File Offset: 0x0006ECC7
		internal void CheckNull(object value, string propName)
		{
			if (value == null)
			{
				throw new ArgumentNullException("propName");
			}
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0006FCD7 File Offset: 0x0006ECD7
		internal void Dispose()
		{
			this.gridLinePen.Dispose();
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0006FCE4 File Offset: 0x0006ECE4
		internal DataGridState GetTopParent()
		{
			if (this.parentsCount < 1)
			{
				return null;
			}
			return (DataGridState)((ICloneable)this.parents[this.parentsCount - 1]).Clone();
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x0006FD13 File Offset: 0x0006ED13
		internal bool IsEmpty()
		{
			return this.parentsCount == 0;
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0006FD20 File Offset: 0x0006ED20
		internal DataGridState PopTop()
		{
			if (this.parentsCount < 1)
			{
				return null;
			}
			this.SetParentCount(this.parentsCount - 1);
			DataGridState dataGridState = (DataGridState)this.parents[this.parentsCount];
			dataGridState.RemoveChangeNotification();
			this.parents.RemoveAt(this.parentsCount);
			return dataGridState;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x0006FD75 File Offset: 0x0006ED75
		internal void Invalidate()
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateParentRows();
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0006FD8C File Offset: 0x0006ED8C
		internal void InvalidateRect(Rectangle rect)
		{
			if (this.dataGrid != null)
			{
				Rectangle rectangle = new Rectangle(rect.X, rect.Y, rect.Width + this.borderWidth, rect.Height + this.borderWidth);
				this.dataGrid.InvalidateParentRowsRect(rectangle);
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0006FDE0 File Offset: 0x0006EDE0
		internal void OnLayout()
		{
			if (this.parentsCount == this.rowHeights.Count)
			{
				return;
			}
			if (this.totalHeight == 0)
			{
				this.totalHeight += 2 * this.borderWidth;
			}
			this.textRegionHeight = this.dataGrid.Font.Height + 2;
			if (this.parentsCount > this.rowHeights.Count)
			{
				int count = this.rowHeights.Count;
				for (int i = count; i < this.parentsCount; i++)
				{
					DataGridState dataGridState = (DataGridState)this.parents[i];
					GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
					int num = 0;
					for (int j = 0; j < gridColumnStyles.Count; j++)
					{
						num = Math.Max(num, gridColumnStyles[j].GetMinimumHeight());
					}
					int num2 = Math.Max(num, this.textRegionHeight);
					num2++;
					this.rowHeights.Add(num2);
					this.totalHeight += num2;
				}
				return;
			}
			if (this.parentsCount == 0)
			{
				this.totalHeight = 0;
			}
			else
			{
				this.totalHeight -= (int)this.rowHeights[this.rowHeights.Count - 1];
			}
			this.rowHeights.RemoveAt(this.rowHeights.Count - 1);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x0006FF44 File Offset: 0x0006EF44
		private int CellCount()
		{
			int num = this.ColsCount();
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x0006FF7B File Offset: 0x0006EF7B
		private void ResetMouseInfo()
		{
			this.downLeftArrow = false;
			this.downRightArrow = false;
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x0006FF8B File Offset: 0x0006EF8B
		private void LeftArrowClick(int cellCount)
		{
			if (this.horizOffset > 0)
			{
				this.ResetMouseInfo();
				this.horizOffset--;
				this.Invalidate();
				return;
			}
			this.ResetMouseInfo();
			this.InvalidateRect(this.layout.leftArrow);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0006FFC8 File Offset: 0x0006EFC8
		private void RightArrowClick(int cellCount)
		{
			if (this.horizOffset < cellCount - 1)
			{
				this.ResetMouseInfo();
				this.horizOffset++;
				this.Invalidate();
				return;
			}
			this.ResetMouseInfo();
			this.InvalidateRect(this.layout.rightArrow);
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x00070008 File Offset: 0x0006F008
		internal void OnMouseDown(int x, int y, bool alignToRight)
		{
			if (this.layout.rightArrow.IsEmpty)
			{
				return;
			}
			int num = this.CellCount();
			if (this.layout.rightArrow.Contains(x, y))
			{
				this.downRightArrow = true;
				if (alignToRight)
				{
					this.LeftArrowClick(num);
					return;
				}
				this.RightArrowClick(num);
				return;
			}
			else
			{
				if (!this.layout.leftArrow.Contains(x, y))
				{
					if (this.downLeftArrow)
					{
						this.downLeftArrow = false;
						this.InvalidateRect(this.layout.leftArrow);
					}
					if (this.downRightArrow)
					{
						this.downRightArrow = false;
						this.InvalidateRect(this.layout.rightArrow);
					}
					return;
				}
				this.downLeftArrow = true;
				if (alignToRight)
				{
					this.RightArrowClick(num);
					return;
				}
				this.LeftArrowClick(num);
				return;
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000700CC File Offset: 0x0006F0CC
		internal void OnMouseLeave()
		{
			if (this.downLeftArrow)
			{
				this.downLeftArrow = false;
				this.InvalidateRect(this.layout.leftArrow);
			}
			if (this.downRightArrow)
			{
				this.downRightArrow = false;
				this.InvalidateRect(this.layout.rightArrow);
			}
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x0007011C File Offset: 0x0006F11C
		internal void OnMouseMove(int x, int y)
		{
			if (this.downLeftArrow)
			{
				this.downLeftArrow = false;
				this.InvalidateRect(this.layout.leftArrow);
			}
			if (this.downRightArrow)
			{
				this.downRightArrow = false;
				this.InvalidateRect(this.layout.rightArrow);
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x0007016C File Offset: 0x0006F16C
		internal void OnMouseUp(int x, int y)
		{
			this.ResetMouseInfo();
			if (!this.layout.rightArrow.IsEmpty && this.layout.rightArrow.Contains(x, y))
			{
				this.InvalidateRect(this.layout.rightArrow);
				return;
			}
			if (!this.layout.leftArrow.IsEmpty && this.layout.leftArrow.Contains(x, y))
			{
				this.InvalidateRect(this.layout.leftArrow);
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000701EE File Offset: 0x0006F1EE
		internal void OnResize(Rectangle oldBounds)
		{
			this.Invalidate();
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000701F8 File Offset: 0x0006F1F8
		internal void Paint(Graphics g, Rectangle visualbounds, bool alignRight)
		{
			Rectangle rectangle = visualbounds;
			if (this.borderWidth > 0)
			{
				this.PaintBorder(g, rectangle);
				rectangle.Inflate(-this.borderWidth, -this.borderWidth);
			}
			this.PaintParentRows(g, rectangle, alignRight);
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x00070238 File Offset: 0x0006F238
		private void PaintBorder(Graphics g, Rectangle bounds)
		{
			Rectangle rectangle = bounds;
			rectangle.Height = this.borderWidth;
			g.FillRectangle(this.borderBrush, rectangle);
			rectangle.Y = bounds.Bottom - this.borderWidth;
			g.FillRectangle(this.borderBrush, rectangle);
			rectangle = new Rectangle(bounds.X, bounds.Y + this.borderWidth, this.borderWidth, bounds.Height - 2 * this.borderWidth);
			g.FillRectangle(this.borderBrush, rectangle);
			rectangle.X = bounds.Right - this.borderWidth;
			g.FillRectangle(this.borderBrush, rectangle);
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000702E4 File Offset: 0x0006F2E4
		private int GetTableBoxWidth(Graphics g, Font font)
		{
			Font font2 = font;
			try
			{
				font2 = new Font(font, FontStyle.Bold);
			}
			catch
			{
			}
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				string text = dataGridState.ListManager.GetListName() + " :";
				int num2 = (int)g.MeasureString(text, font2).Width;
				num = Math.Max(num2, num);
			}
			return num;
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x0007036C File Offset: 0x0006F36C
		private int GetColBoxWidth(Graphics g, Font font, int colNum)
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
				if (colNum < gridColumnStyles.Count)
				{
					string text = gridColumnStyles[colNum].HeaderText + " :";
					int num2 = (int)g.MeasureString(text, font).Width;
					num = Math.Max(num2, num);
				}
			}
			return num;
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000703E8 File Offset: 0x0006F3E8
		private int GetColDataBoxWidth(Graphics g, int colNum)
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				GridColumnStylesCollection gridColumnStyles = dataGridState.GridColumnStyles;
				if (colNum < gridColumnStyles.Count)
				{
					object columnValueAtRow = gridColumnStyles[colNum].GetColumnValueAtRow((CurrencyManager)this.dataGrid.BindingContext[dataGridState.DataSource, dataGridState.DataMember], dataGridState.LinkingRow.RowNumber);
					int width = gridColumnStyles[colNum].GetPreferredSize(g, columnValueAtRow).Width;
					num = Math.Max(width, num);
				}
			}
			return num;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x00070490 File Offset: 0x0006F490
		private int ColsCount()
		{
			int num = 0;
			for (int i = 0; i < this.parentsCount; i++)
			{
				DataGridState dataGridState = (DataGridState)this.parents[i];
				num = Math.Max(num, dataGridState.GridColumnStyles.Count);
			}
			return num;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000704D8 File Offset: 0x0006F4D8
		private int TotalWidth(int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			int num = 0;
			num += tableNameBoxWidth;
			for (int i = 0; i < colsNameWidths.Length; i++)
			{
				num += colsNameWidths[i];
				num += colsDataWidths[i];
			}
			return num + 3 * (colsNameWidths.Length - 1);
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x00070510 File Offset: 0x0006F510
		private void ComputeLayout(Rectangle bounds, int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			int num = this.TotalWidth(tableNameBoxWidth, colsNameWidths, colsDataWidths);
			if (num > bounds.Width)
			{
				this.layout.leftArrow = new Rectangle(bounds.X, bounds.Y, 15, bounds.Height);
				this.layout.data = new Rectangle(this.layout.leftArrow.Right, bounds.Y, bounds.Width - 30, bounds.Height);
				this.layout.rightArrow = new Rectangle(this.layout.data.Right, bounds.Y, 15, bounds.Height);
				return;
			}
			this.layout.data = bounds;
			this.layout.leftArrow = Rectangle.Empty;
			this.layout.rightArrow = Rectangle.Empty;
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000705F4 File Offset: 0x0006F5F4
		private void PaintParentRows(Graphics g, Rectangle bounds, bool alignToRight)
		{
			int num = 0;
			int num2 = this.ColsCount();
			int[] array = new int[num2];
			int[] array2 = new int[num2];
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				num = this.GetTableBoxWidth(g, this.dataGrid.Font);
			}
			for (int i = 0; i < num2; i++)
			{
				if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.ColumnName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
				{
					array[i] = this.GetColBoxWidth(g, this.dataGrid.Font, i);
				}
				else
				{
					array[i] = 0;
				}
				array2[i] = this.GetColDataBoxWidth(g, i);
			}
			this.ComputeLayout(bounds, num, array, array2);
			if (!this.layout.leftArrow.IsEmpty)
			{
				g.FillRectangle(this.BackBrush, this.layout.leftArrow);
				this.PaintLeftArrow(g, this.layout.leftArrow, alignToRight);
			}
			Rectangle data = this.layout.data;
			for (int j = 0; j < this.parentsCount; j++)
			{
				data.Height = (int)this.rowHeights[j];
				if (data.Y > bounds.Bottom)
				{
					break;
				}
				int num3 = this.PaintRow(g, data, j, this.dataGrid.Font, alignToRight, num, array, array2);
				if (j == this.parentsCount - 1)
				{
					break;
				}
				g.DrawLine(this.gridLinePen, data.X, data.Bottom, data.X + num3, data.Bottom);
				data.Y += data.Height;
			}
			if (!this.layout.rightArrow.IsEmpty)
			{
				g.FillRectangle(this.BackBrush, this.layout.rightArrow);
				this.PaintRightArrow(g, this.layout.rightArrow, alignToRight);
			}
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000707DC File Offset: 0x0006F7DC
		private Bitmap GetBitmap(string bitmapName, Color transparentColor)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridParentRows), bitmapName);
				bitmap.MakeTransparent(transparentColor);
			}
			catch (Exception)
			{
			}
			return bitmap;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x0007081C File Offset: 0x0006F81C
		private Bitmap GetRightArrowBitmap()
		{
			if (DataGridParentRows.rightArrow == null)
			{
				DataGridParentRows.rightArrow = this.GetBitmap("DataGridParentRows.RightArrow.bmp", Color.White);
			}
			return DataGridParentRows.rightArrow;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x0007083F File Offset: 0x0006F83F
		private Bitmap GetLeftArrowBitmap()
		{
			if (DataGridParentRows.leftArrow == null)
			{
				DataGridParentRows.leftArrow = this.GetBitmap("DataGridParentRows.LeftArrow.bmp", Color.White);
			}
			return DataGridParentRows.leftArrow;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x00070864 File Offset: 0x0006F864
		private void PaintBitmap(Graphics g, Bitmap b, Rectangle bounds)
		{
			int num = bounds.X + (bounds.Width - b.Width) / 2;
			int num2 = bounds.Y + (bounds.Height - b.Height) / 2;
			Rectangle rectangle = new Rectangle(num, num2, b.Width, b.Height);
			g.FillRectangle(this.BackBrush, rectangle);
			ImageAttributes imageAttributes = new ImageAttributes();
			this.colorMap[0].NewColor = this.ForeColor;
			imageAttributes.SetRemapTable(this.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(b, rectangle, 0, 0, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x0007090C File Offset: 0x0006F90C
		private void PaintDownButton(Graphics g, Rectangle bounds)
		{
			g.DrawLine(Pens.Black, bounds.X, bounds.Y, bounds.X + bounds.Width, bounds.Y);
			g.DrawLine(Pens.White, bounds.X + bounds.Width, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height);
			g.DrawLine(Pens.White, bounds.X + bounds.Width, bounds.Y + bounds.Height, bounds.X, bounds.Y + bounds.Height);
			g.DrawLine(Pens.Black, bounds.X, bounds.Y + bounds.Height, bounds.X, bounds.Y);
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000709F8 File Offset: 0x0006F9F8
		private void PaintLeftArrow(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Bitmap leftArrowBitmap = this.GetLeftArrowBitmap();
			if (this.downLeftArrow)
			{
				this.PaintDownButton(g, bounds);
				this.layout.leftArrow.Inflate(-1, -1);
				lock (leftArrowBitmap)
				{
					this.PaintBitmap(g, leftArrowBitmap, bounds);
				}
				this.layout.leftArrow.Inflate(1, 1);
				return;
			}
			lock (leftArrowBitmap)
			{
				this.PaintBitmap(g, leftArrowBitmap, bounds);
			}
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x00070A94 File Offset: 0x0006FA94
		private void PaintRightArrow(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Bitmap rightArrowBitmap = this.GetRightArrowBitmap();
			if (this.downRightArrow)
			{
				this.PaintDownButton(g, bounds);
				this.layout.rightArrow.Inflate(-1, -1);
				lock (rightArrowBitmap)
				{
					this.PaintBitmap(g, rightArrowBitmap, bounds);
				}
				this.layout.rightArrow.Inflate(1, 1);
				return;
			}
			lock (rightArrowBitmap)
			{
				this.PaintBitmap(g, rightArrowBitmap, bounds);
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x00070B30 File Offset: 0x0006FB30
		private int PaintRow(Graphics g, Rectangle bounds, int row, Font font, bool alignToRight, int tableNameBoxWidth, int[] colsNameWidths, int[] colsDataWidths)
		{
			DataGridState dataGridState = (DataGridState)this.parents[row];
			Rectangle rectangle = bounds;
			Rectangle rectangle2 = bounds;
			rectangle.Height = (int)this.rowHeights[row];
			rectangle2.Height = (int)this.rowHeights[row];
			int num = 0;
			int num2 = 0;
			if (this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.TableName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both)
			{
				if (num2 < this.horizOffset)
				{
					num2++;
				}
				else
				{
					rectangle.Width = Math.Min(rectangle.Width, tableNameBoxWidth);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					string text = dataGridState.ListManager.GetListName() + ": ";
					this.PaintText(g, rectangle, text, font, true, alignToRight);
					num += rectangle.Width;
				}
			}
			if (num >= bounds.Width)
			{
				return bounds.Width;
			}
			rectangle2.Width -= num;
			rectangle2.X += (alignToRight ? 0 : num);
			num += this.PaintColumns(g, rectangle2, dataGridState, font, alignToRight, colsNameWidths, colsDataWidths, num2);
			if (num < bounds.Width)
			{
				rectangle.X = bounds.X + num;
				rectangle.Width = bounds.Width - num;
				rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
				g.FillRectangle(this.BackBrush, rectangle);
			}
			return num;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x00070CA4 File Offset: 0x0006FCA4
		private int PaintColumns(Graphics g, Rectangle bounds, DataGridState dgs, Font font, bool alignToRight, int[] colsNameWidths, int[] colsDataWidths, int skippedCells)
		{
			Rectangle rectangle = bounds;
			GridColumnStylesCollection gridColumnStyles = dgs.GridColumnStyles;
			int num = 0;
			int num2 = 0;
			while (num2 < gridColumnStyles.Count && num < bounds.Width)
			{
				if ((this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.ColumnName || this.dataGrid.ParentRowsLabelStyle == DataGridParentRowsLabelStyle.Both) && skippedCells >= this.horizOffset)
				{
					rectangle.X = bounds.X + num;
					rectangle.Width = Math.Min(bounds.Width - num, colsNameWidths[num2]);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					string text = gridColumnStyles[num2].HeaderText + ": ";
					this.PaintText(g, rectangle, text, font, false, alignToRight);
					num += rectangle.Width;
				}
				if (num >= bounds.Width)
				{
					break;
				}
				if (skippedCells < this.horizOffset)
				{
					skippedCells++;
				}
				else
				{
					rectangle.X = bounds.X + num;
					rectangle.Width = Math.Min(bounds.Width - num, colsDataWidths[num2]);
					rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
					gridColumnStyles[num2].Paint(g, rectangle, (CurrencyManager)this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember], this.dataGrid.BindingContext[dgs.DataSource, dgs.DataMember].Position, this.BackBrush, this.ForeBrush, alignToRight);
					num += rectangle.Width;
					g.DrawLine(new Pen(SystemColors.ControlDark), alignToRight ? rectangle.X : rectangle.Right, rectangle.Y, alignToRight ? rectangle.X : rectangle.Right, rectangle.Bottom);
					num++;
					if (num2 < gridColumnStyles.Count - 1)
					{
						rectangle.X = bounds.X + num;
						rectangle.Width = Math.Min(bounds.Width - num, 3);
						rectangle.X = this.MirrorRect(bounds, rectangle, alignToRight);
						g.FillRectangle(this.BackBrush, rectangle);
						num += 3;
					}
				}
				num2++;
			}
			return num;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x00070ED4 File Offset: 0x0006FED4
		private int PaintText(Graphics g, Rectangle textBounds, string text, Font font, bool bold, bool alignToRight)
		{
			Font font2 = font;
			if (bold)
			{
				try
				{
					font2 = new Font(font, FontStyle.Bold);
					goto IL_0018;
				}
				catch
				{
					goto IL_0018;
				}
			}
			font2 = font;
			IL_0018:
			g.FillRectangle(this.BackBrush, textBounds);
			StringFormat stringFormat = new StringFormat();
			if (alignToRight)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
				stringFormat.Alignment = StringAlignment.Far;
			}
			stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
			textBounds.Offset(0, 2);
			textBounds.Height -= 2;
			g.DrawString(text, font2, this.ForeBrush, textBounds, stringFormat);
			stringFormat.Dispose();
			return textBounds.Width;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00070F84 File Offset: 0x0006FF84
		private int MirrorRect(Rectangle surroundingRect, Rectangle containedRect, bool alignToRight)
		{
			if (alignToRight)
			{
				return surroundingRect.Right - containedRect.Right + surroundingRect.X;
			}
			return containedRect.X;
		}

		// Token: 0x0400179E RID: 6046
		private DataGrid dataGrid;

		// Token: 0x0400179F RID: 6047
		private SolidBrush backBrush = DataGrid.DefaultParentRowsBackBrush;

		// Token: 0x040017A0 RID: 6048
		private SolidBrush foreBrush = DataGrid.DefaultParentRowsForeBrush;

		// Token: 0x040017A1 RID: 6049
		private int borderWidth = 1;

		// Token: 0x040017A2 RID: 6050
		private Brush borderBrush = new SolidBrush(SystemColors.WindowFrame);

		// Token: 0x040017A3 RID: 6051
		private static Bitmap rightArrow;

		// Token: 0x040017A4 RID: 6052
		private static Bitmap leftArrow;

		// Token: 0x040017A5 RID: 6053
		private ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x040017A6 RID: 6054
		private Pen gridLinePen = SystemPens.Control;

		// Token: 0x040017A7 RID: 6055
		private int totalHeight;

		// Token: 0x040017A8 RID: 6056
		private int textRegionHeight;

		// Token: 0x040017A9 RID: 6057
		private DataGridParentRows.Layout layout = new DataGridParentRows.Layout();

		// Token: 0x040017AA RID: 6058
		private bool downLeftArrow;

		// Token: 0x040017AB RID: 6059
		private bool downRightArrow;

		// Token: 0x040017AC RID: 6060
		private int horizOffset;

		// Token: 0x040017AD RID: 6061
		private ArrayList parents = new ArrayList();

		// Token: 0x040017AE RID: 6062
		private int parentsCount;

		// Token: 0x040017AF RID: 6063
		private ArrayList rowHeights = new ArrayList();

		// Token: 0x040017B0 RID: 6064
		private AccessibleObject accessibleObject;

		// Token: 0x020002D9 RID: 729
		private class Layout
		{
			// Token: 0x06002A64 RID: 10852 RVA: 0x00070FA8 File Offset: 0x0006FFA8
			public Layout()
			{
				this.data = Rectangle.Empty;
				this.leftArrow = Rectangle.Empty;
				this.rightArrow = Rectangle.Empty;
			}

			// Token: 0x06002A65 RID: 10853 RVA: 0x00070FD4 File Offset: 0x0006FFD4
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				stringBuilder.Append("ParentRows Layout: \n");
				stringBuilder.Append("data = ");
				stringBuilder.Append(this.data.ToString());
				stringBuilder.Append("\n leftArrow = ");
				stringBuilder.Append(this.leftArrow.ToString());
				stringBuilder.Append("\n rightArrow = ");
				stringBuilder.Append(this.rightArrow.ToString());
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}

			// Token: 0x040017B1 RID: 6065
			public Rectangle data;

			// Token: 0x040017B2 RID: 6066
			public Rectangle leftArrow;

			// Token: 0x040017B3 RID: 6067
			public Rectangle rightArrow;
		}

		// Token: 0x020002DA RID: 730
		[ComVisible(true)]
		protected internal class DataGridParentRowsAccessibleObject : AccessibleObject
		{
			// Token: 0x06002A66 RID: 10854 RVA: 0x00071076 File Offset: 0x00070076
			public DataGridParentRowsAccessibleObject(DataGridParentRows owner)
			{
				this.owner = owner;
			}

			// Token: 0x170006FC RID: 1788
			// (get) Token: 0x06002A67 RID: 10855 RVA: 0x00071085 File Offset: 0x00070085
			internal DataGridParentRows Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x170006FD RID: 1789
			// (get) Token: 0x06002A68 RID: 10856 RVA: 0x0007108D File Offset: 0x0007008D
			public override Rectangle Bounds
			{
				get
				{
					return this.owner.dataGrid.RectangleToScreen(this.owner.dataGrid.ParentRowsBounds);
				}
			}

			// Token: 0x170006FE RID: 1790
			// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000710AF File Offset: 0x000700AF
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccDGNavigateBack");
				}
			}

			// Token: 0x170006FF RID: 1791
			// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000710BB File Offset: 0x000700BB
			public override string Name
			{
				get
				{
					return SR.GetString("AccDGParentRows");
				}
			}

			// Token: 0x17000700 RID: 1792
			// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000710C7 File Offset: 0x000700C7
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.dataGrid.AccessibilityObject;
				}
			}

			// Token: 0x17000701 RID: 1793
			// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000710D9 File Offset: 0x000700D9
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.List;
				}
			}

			// Token: 0x17000702 RID: 1794
			// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000710E0 File Offset: 0x000700E0
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.ReadOnly;
					if (this.owner.parentsCount == 0)
					{
						accessibleStates |= AccessibleStates.Invisible;
					}
					if (this.owner.dataGrid.ParentRowsVisible)
					{
						accessibleStates |= AccessibleStates.Expanded;
					}
					else
					{
						accessibleStates |= AccessibleStates.Collapsed;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000703 RID: 1795
			// (get) Token: 0x06002A6E RID: 10862 RVA: 0x0007112A File Offset: 0x0007012A
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return null;
				}
			}

			// Token: 0x06002A6F RID: 10863 RVA: 0x0007112D File Offset: 0x0007012D
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.dataGrid.NavigateBack();
			}

			// Token: 0x06002A70 RID: 10864 RVA: 0x0007113F File Offset: 0x0007013F
			public override AccessibleObject GetChild(int index)
			{
				return ((DataGridState)this.owner.parents[index]).ParentRowAccessibleObject;
			}

			// Token: 0x06002A71 RID: 10865 RVA: 0x0007115C File Offset: 0x0007015C
			public override int GetChildCount()
			{
				return this.owner.parentsCount;
			}

			// Token: 0x06002A72 RID: 10866 RVA: 0x00071169 File Offset: 0x00070169
			public override AccessibleObject GetFocused()
			{
				return null;
			}

			// Token: 0x06002A73 RID: 10867 RVA: 0x0007116C File Offset: 0x0007016C
			internal AccessibleObject GetNext(AccessibleObject child)
			{
				int childCount = this.GetChildCount();
				bool flag = false;
				for (int i = 0; i < childCount; i++)
				{
					if (flag)
					{
						return this.GetChild(i);
					}
					if (this.GetChild(i) == child)
					{
						flag = true;
					}
				}
				return null;
			}

			// Token: 0x06002A74 RID: 10868 RVA: 0x000711A8 File Offset: 0x000701A8
			internal AccessibleObject GetPrev(AccessibleObject child)
			{
				int childCount = this.GetChildCount();
				bool flag = false;
				for (int i = childCount - 1; i >= 0; i--)
				{
					if (flag)
					{
						return this.GetChild(i);
					}
					if (this.GetChild(i) == child)
					{
						flag = true;
					}
				}
				return null;
			}

			// Token: 0x06002A75 RID: 10869 RVA: 0x000711E4 File Offset: 0x000701E4
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.Parent.GetChild(this.GetChildCount() - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.Parent.GetChild(1);
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

			// Token: 0x06002A76 RID: 10870 RVA: 0x00071268 File Offset: 0x00070268
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
			}

			// Token: 0x040017B4 RID: 6068
			private DataGridParentRows owner;
		}
	}
}
