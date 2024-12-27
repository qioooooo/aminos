using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000336 RID: 822
	public class DataGridViewHeaderCell : DataGridViewCell
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x000B996C File Offset: 0x000B896C
		protected ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewHeaderCell.PropButtonState, out flag);
				if (flag)
				{
					return (ButtonState)integer;
				}
				return ButtonState.Normal;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (set) Token: 0x0600347F RID: 13439 RVA: 0x000B9992 File Offset: 0x000B8992
		private ButtonState ButtonStatePrivate
		{
			set
			{
				if (this.ButtonState != value)
				{
					base.Properties.SetInteger(DataGridViewHeaderCell.PropButtonState, (int)value);
				}
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000B99AE File Offset: 0x000B89AE
		protected override void Dispose(bool disposing)
		{
			if (this.FlipXPThemesBitmap != null && disposing)
			{
				this.FlipXPThemesBitmap.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000B99D0 File Offset: 0x000B89D0
		[Browsable(false)]
		public override bool Displayed
		{
			get
			{
				if (base.DataGridView == null || !base.DataGridView.Visible)
				{
					return false;
				}
				if (base.OwningRow != null)
				{
					return base.DataGridView.RowHeadersVisible && base.OwningRow.Displayed;
				}
				if (base.OwningColumn != null)
				{
					return base.DataGridView.ColumnHeadersVisible && base.OwningColumn.Displayed;
				}
				return base.DataGridView.LayoutInfo.TopLeftHeader != Rectangle.Empty;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x000B9A54 File Offset: 0x000B8A54
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x000B9A6B File Offset: 0x000B8A6B
		internal Bitmap FlipXPThemesBitmap
		{
			get
			{
				return (Bitmap)base.Properties.GetObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap))
				{
					base.Properties.SetObject(DataGridViewHeaderCell.PropFlipXPThemesBitmap, value);
				}
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x000B9A93 File Offset: 0x000B8A93
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewHeaderCell.defaultFormattedValueType;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000B9A9A File Offset: 0x000B8A9A
		[Browsable(false)]
		public override bool Frozen
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Frozen;
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Frozen;
				}
				return base.DataGridView != null;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000B9ACF File Offset: 0x000B8ACF
		internal override bool HasValueType
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewHeaderCell.PropValueType) && base.Properties.GetObject(DataGridViewHeaderCell.PropValueType) != null;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000B9AFB File Offset: 0x000B8AFB
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x000B9B00 File Offset: 0x000B8B00
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override bool ReadOnly
		{
			get
			{
				return true;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_HeaderCellReadOnlyProperty", new object[] { "ReadOnly" }));
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x000B9B2C File Offset: 0x000B8B2C
		[Browsable(false)]
		public override bool Resizable
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing);
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing);
				}
				return base.DataGridView != null && (base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing || base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x000B9BC2 File Offset: 0x000B8BC2
		// (set) Token: 0x0600348B RID: 13451 RVA: 0x000B9BC8 File Offset: 0x000B8BC8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Selected
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_HeaderCellReadOnlyProperty", new object[] { "Selected" }));
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x000B9BF4 File Offset: 0x000B8BF4
		// (set) Token: 0x0600348D RID: 13453 RVA: 0x000B9C21 File Offset: 0x000B8C21
		public override Type ValueType
		{
			get
			{
				Type type = (Type)base.Properties.GetObject(DataGridViewHeaderCell.PropValueType);
				if (type != null)
				{
					return type;
				}
				return DataGridViewHeaderCell.defaultValueType;
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewHeaderCell.PropValueType))
				{
					base.Properties.SetObject(DataGridViewHeaderCell.PropValueType, value);
				}
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x000B9C4C File Offset: 0x000B8C4C
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				if (base.OwningRow != null)
				{
					return base.OwningRow.Visible && (base.DataGridView == null || base.DataGridView.RowHeadersVisible);
				}
				if (base.OwningColumn != null)
				{
					return base.OwningColumn.Visible && (base.DataGridView == null || base.DataGridView.ColumnHeadersVisible);
				}
				return base.DataGridView != null && base.DataGridView.RowHeadersVisible && base.DataGridView.ColumnHeadersVisible;
			}
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000B9CD8 File Offset: 0x000B8CD8
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewHeaderCell dataGridViewHeaderCell;
			if (type == DataGridViewHeaderCell.cellType)
			{
				dataGridViewHeaderCell = new DataGridViewHeaderCell();
			}
			else
			{
				dataGridViewHeaderCell = (DataGridViewHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewHeaderCell);
			dataGridViewHeaderCell.Value = base.Value;
			return dataGridViewHeaderCell;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000B9D1C File Offset: 0x000B8D1C
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			ContextMenuStrip contextMenuStrip = base.GetContextMenuStrip(rowIndex);
			if (contextMenuStrip != null)
			{
				return contextMenuStrip;
			}
			if (base.DataGridView != null)
			{
				return base.DataGridView.ContextMenuStrip;
			}
			return null;
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000B9D4C File Offset: 0x000B8D4C
		public override DataGridViewElementStates GetInheritedState(int rowIndex)
		{
			DataGridViewElementStates dataGridViewElementStates = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.ResizableSet;
			if (base.OwningRow != null)
			{
				if ((base.DataGridView == null && rowIndex != -1) || (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.DataGridView != null && base.DataGridView.Rows.SharedRow(rowIndex) != base.OwningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				dataGridViewElementStates |= base.OwningRow.GetState(rowIndex) & DataGridViewElementStates.Frozen;
				if (base.OwningRow.GetResizable(rowIndex) == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.OwningRow.GetVisible(rowIndex) && (base.DataGridView == null || base.DataGridView.RowHeadersVisible))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.OwningRow.GetDisplayed(rowIndex))
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			else if (base.OwningColumn != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				dataGridViewElementStates |= base.OwningColumn.State & DataGridViewElementStates.Frozen;
				if (base.OwningColumn.Resizable == DataGridViewTriState.True || (base.DataGridView != null && base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.OwningColumn.Visible && (base.DataGridView == null || base.DataGridView.ColumnHeadersVisible))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.OwningColumn.Displayed)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			else if (base.DataGridView != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				dataGridViewElementStates |= DataGridViewElementStates.Frozen;
				if (base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing || base.DataGridView.ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (base.DataGridView.RowHeadersVisible && base.DataGridView.ColumnHeadersVisible)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (base.DataGridView.LayoutInfo.TopLeftHeader != Rectangle.Empty)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
			}
			return dataGridViewElementStates;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000B9F9C File Offset: 0x000B8F9C
		protected override Size GetSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(-1, -1);
			}
			else if (base.OwningColumn != null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(base.OwningColumn.Thickness, base.DataGridView.ColumnHeadersHeight);
			}
			else if (base.OwningRow != null)
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (base.DataGridView.Rows.SharedRow(rowIndex) != base.OwningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				return new Size(base.DataGridView.RowHeadersWidth, base.OwningRow.GetHeight(rowIndex));
			}
			else
			{
				if (rowIndex != -1)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				return new Size(base.DataGridView.RowHeadersWidth, base.DataGridView.ColumnHeadersHeight);
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000BA0B8 File Offset: 0x000B90B8
		internal static Rectangle GetThemeMargins(Graphics g)
		{
			if (DataGridViewHeaderCell.rectThemeMargins.X == -1)
			{
				Rectangle rectangle = new Rectangle(0, 0, 100, 100);
				Rectangle backgroundContentRectangle = DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.VisualStyleRenderer.GetBackgroundContentRectangle(g, rectangle);
				DataGridViewHeaderCell.rectThemeMargins.X = backgroundContentRectangle.X;
				DataGridViewHeaderCell.rectThemeMargins.Y = backgroundContentRectangle.Y;
				DataGridViewHeaderCell.rectThemeMargins.Width = 100 - backgroundContentRectangle.Right;
				DataGridViewHeaderCell.rectThemeMargins.Height = 100 - backgroundContentRectangle.Bottom;
				if (DataGridViewHeaderCell.rectThemeMargins.X == 3 && DataGridViewHeaderCell.rectThemeMargins.Y + DataGridViewHeaderCell.rectThemeMargins.Width + DataGridViewHeaderCell.rectThemeMargins.Height == 0)
				{
					DataGridViewHeaderCell.rectThemeMargins = new Rectangle(0, 0, 2, 3);
				}
				else
				{
					try
					{
						string fileName = Path.GetFileName(VisualStyleInformation.ThemeFilename);
						if (string.Equals(fileName, "Aero.msstyles", StringComparison.OrdinalIgnoreCase))
						{
							DataGridViewHeaderCell.rectThemeMargins = new Rectangle(2, 1, 0, 2);
						}
					}
					catch
					{
					}
				}
			}
			return DataGridViewHeaderCell.rectThemeMargins;
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000BA1BC File Offset: 0x000B91BC
		protected override object GetValue(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			return base.Properties.GetObject(DataGridViewCell.PropCellValue);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000BA1DD File Offset: 0x000B91DD
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000BA1FC File Offset: 0x000B91FC
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000BA247 File Offset: 0x000B9247
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return this.ButtonState != ButtonState.Normal && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000BA25E File Offset: 0x000B925E
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells;
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000BA27C File Offset: 0x000B927C
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells && !base.DataGridView.ResizingOperationAboutToStart)
			{
				this.UpdateButtonState(ButtonState.Pushed, e.RowIndex);
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000BA2CC File Offset: 0x000B92CC
		protected override void OnMouseEnter(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y && this.ButtonState == ButtonState.Normal && Control.MouseButtons == MouseButtons.Left && !base.DataGridView.ResizingOperationAboutToStart)
				{
					this.UpdateButtonState(ButtonState.Pushed, rowIndex);
				}
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000BA35F File Offset: 0x000B935F
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (this.ButtonState != ButtonState.Normal)
				{
					this.UpdateButtonState(ButtonState.Normal, rowIndex);
				}
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000BA399 File Offset: 0x000B9399
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				this.UpdateButtonState(ButtonState.Normal, e.RowIndex);
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000BA3CC File Offset: 0x000B93CC
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			if (DataGridViewCell.PaintBackground(paintParts))
			{
				Rectangle rectangle = cellBounds;
				Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
				rectangle.Offset(rectangle2.X, rectangle2.Y);
				rectangle.Width -= rectangle2.Right;
				rectangle.Height -= rectangle2.Bottom;
				bool flag = (dataGridViewElementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
				SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
				if (cachedBrush.Color.A == 255)
				{
					graphics.FillRectangle(cachedBrush, rectangle);
				}
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000BA4A8 File Offset: 0x000B94A8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewHeaderCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000BA506 File Offset: 0x000B9506
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			this.ButtonStatePrivate = newButtonState;
			base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
		}

		// Token: 0x04001B2D RID: 6957
		private const byte DATAGRIDVIEWHEADERCELL_themeMargin = 100;

		// Token: 0x04001B2E RID: 6958
		private const string AEROTHEMEFILENAME = "Aero.msstyles";

		// Token: 0x04001B2F RID: 6959
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04001B30 RID: 6960
		private static Type defaultValueType = typeof(object);

		// Token: 0x04001B31 RID: 6961
		private static Type cellType = typeof(DataGridViewHeaderCell);

		// Token: 0x04001B32 RID: 6962
		private static Rectangle rectThemeMargins = new Rectangle(-1, -1, 0, 0);

		// Token: 0x04001B33 RID: 6963
		private static readonly int PropValueType = PropertyStore.CreateKey();

		// Token: 0x04001B34 RID: 6964
		private static readonly int PropButtonState = PropertyStore.CreateKey();

		// Token: 0x04001B35 RID: 6965
		private static readonly int PropFlipXPThemesBitmap = PropertyStore.CreateKey();

		// Token: 0x02000337 RID: 823
		private class DataGridViewHeaderCellRenderer
		{
			// Token: 0x060034A1 RID: 13473 RVA: 0x000BA58A File Offset: 0x000B958A
			private DataGridViewHeaderCellRenderer()
			{
			}

			// Token: 0x17000980 RID: 2432
			// (get) Token: 0x060034A2 RID: 13474 RVA: 0x000BA592 File Offset: 0x000B9592
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
					}
					return DataGridViewHeaderCell.DataGridViewHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x04001B36 RID: 6966
			private static VisualStyleRenderer visualStyleRenderer;
		}
	}
}
