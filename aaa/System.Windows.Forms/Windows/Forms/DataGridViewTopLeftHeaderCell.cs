using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200039D RID: 925
	public class DataGridViewTopLeftHeaderCell : DataGridViewColumnHeaderCell
	{
		// Token: 0x06003880 RID: 14464 RVA: 0x000CE2F5 File Offset: 0x000CD2F5
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject(this);
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x000CE300 File Offset: 0x000CD300
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, value, null, cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000CE364 File Offset: 0x000CD364
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, null, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000CE3C4 File Offset: 0x000CD3C4
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle rectangle = this.BorderWidths(base.DataGridView.AdjustedTopLeftHeaderBorderStyle);
			int num = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int num2 = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			object obj = this.GetValue(rowIndex);
			if (!(obj is string))
			{
				obj = null;
			}
			return DataGridViewUtilities.GetPreferredRowHeaderSize(graphics, (string)obj, cellStyle, num, num2, base.DataGridView.ShowCellErrors, false, constraintSize, textFormatFlags);
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000CE4A4 File Offset: 0x000CD4A4
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000CE4DC File Offset: 0x000CD4DC
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = cellBounds;
			Rectangle rectangle3 = this.BorderWidths(advancedBorderStyle);
			rectangle2.Offset(rectangle3.X, rectangle3.Y);
			rectangle2.Width -= rectangle3.Right;
			rectangle2.Height -= rectangle3.Bottom;
			bool flag = (cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			if (paint && DataGridViewCell.PaintBackground(paintParts))
			{
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					int num = 1;
					if (base.ButtonState != ButtonState.Normal)
					{
						num = 3;
					}
					else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex)
					{
						num = 2;
					}
					rectangle2.Inflate(16, 16);
					DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.DrawHeader(graphics, rectangle2, num);
					rectangle2.Inflate(-16, -16);
				}
				else
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (cachedBrush.Color.A == 255)
					{
						graphics.FillRectangle(cachedBrush, rectangle2);
					}
				}
			}
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle2.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle2.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle2.Width -= cellStyle.Padding.Horizontal;
				rectangle2.Height -= cellStyle.Padding.Vertical;
			}
			Rectangle rectangle4 = rectangle2;
			string text = formattedValue as string;
			rectangle2.Offset(1, 1);
			rectangle2.Width -= 3;
			rectangle2.Height -= 2;
			if (rectangle2.Width > 0 && rectangle2.Height > 0 && !string.IsNullOrEmpty(text) && (paint || computeContentBounds))
			{
				Color color;
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					color = DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
				}
				else
				{
					color = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
				}
				TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
				if (paint)
				{
					if (DataGridViewCell.PaintContentForeground(paintParts))
					{
						if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
						{
							textFormatFlags |= TextFormatFlags.EndEllipsis;
						}
						TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle2, color, textFormatFlags);
					}
				}
				else
				{
					rectangle = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle);
				}
			}
			else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
			{
				rectangle = base.ComputeErrorIconBounds(rectangle4);
			}
			if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
			{
				base.PaintErrorIcon(graphics, cellStyle, rowIndex, cellBounds, rectangle4, errorText);
			}
			return rectangle;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000CE81C File Offset: 0x000CD81C
		protected override void PaintBorder(Graphics graphics, Rectangle clipBounds, Rectangle bounds, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			base.PaintBorder(graphics, clipBounds, bounds, cellStyle, advancedBorderStyle);
			if (!base.DataGridView.RightToLeftInternal && base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Inset)
				{
					Pen pen = null;
					Pen pen2 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
					graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
					graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					return;
				}
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					Pen pen3 = null;
					Pen pen4 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen3, ref pen4);
					graphics.DrawLine(pen4, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
					graphics.DrawLine(pen4, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					return;
				}
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					Pen pen5 = null;
					Pen pen6 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen5, ref pen6);
					graphics.DrawLine(pen5, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Bottom - 1);
					graphics.DrawLine(pen5, bounds.X + 1, bounds.Y + 1, bounds.Right - 1, bounds.Y + 1);
				}
			}
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000CE9C7 File Offset: 0x000CD9C7
		public override string ToString()
		{
			return "DataGridViewTopLeftHeaderCell";
		}

		// Token: 0x04001C7A RID: 7290
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04001C7B RID: 7291
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04001C7C RID: 7292
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_verticalTextMargin = 1;

		// Token: 0x04001C7D RID: 7293
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x0200039E RID: 926
		private class DataGridViewTopLeftHeaderCellRenderer
		{
			// Token: 0x06003889 RID: 14473 RVA: 0x000CE9DA File Offset: 0x000CD9DA
			private DataGridViewTopLeftHeaderCellRenderer()
			{
			}

			// Token: 0x17000A9C RID: 2716
			// (get) Token: 0x0600388A RID: 14474 RVA: 0x000CE9E2 File Offset: 0x000CD9E2
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewTopLeftHeaderCell.HeaderElement);
					}
					return DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x0600388B RID: 14475 RVA: 0x000CE9FF File Offset: 0x000CD9FF
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewTopLeftHeaderCell.HeaderElement.ClassName, DataGridViewTopLeftHeaderCell.HeaderElement.Part, headerState);
				DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04001C7E RID: 7294
			private static VisualStyleRenderer visualStyleRenderer;
		}

		// Token: 0x0200039F RID: 927
		protected class DataGridViewTopLeftHeaderCellAccessibleObject : DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject
		{
			// Token: 0x0600388C RID: 14476 RVA: 0x000CEA37 File Offset: 0x000CDA37
			public DataGridViewTopLeftHeaderCellAccessibleObject(DataGridViewTopLeftHeaderCell owner)
				: base(owner)
			{
			}

			// Token: 0x17000A9D RID: 2717
			// (get) Token: 0x0600388D RID: 14477 RVA: 0x000CEA40 File Offset: 0x000CDA40
			public override Rectangle Bounds
			{
				get
				{
					Rectangle cellDisplayRectangle = base.Owner.DataGridView.GetCellDisplayRectangle(-1, -1, false);
					return base.Owner.DataGridView.RectangleToScreen(cellDisplayRectangle);
				}
			}

			// Token: 0x17000A9E RID: 2718
			// (get) Token: 0x0600388E RID: 14478 RVA: 0x000CEA72 File Offset: 0x000CDA72
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.DataGridView.MultiSelect)
					{
						return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellDefaultAction");
					}
					return string.Empty;
				}
			}

			// Token: 0x17000A9F RID: 2719
			// (get) Token: 0x0600388F RID: 14479 RVA: 0x000CEA98 File Offset: 0x000CDA98
			public override string Name
			{
				get
				{
					object value = base.Owner.Value;
					if (value != null && !(value is string))
					{
						return string.Empty;
					}
					string text = value as string;
					if (!string.IsNullOrEmpty(text))
					{
						return string.Empty;
					}
					if (base.Owner.DataGridView == null)
					{
						return string.Empty;
					}
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellName");
					}
					return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellNameRTL");
				}
			}

			// Token: 0x17000AA0 RID: 2720
			// (get) Token: 0x06003890 RID: 14480 RVA: 0x000CEB14 File Offset: 0x000CDB14
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Selectable;
					AccessibleStates state = base.State;
					if ((state & AccessibleStates.Offscreen) == AccessibleStates.Offscreen)
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					if (base.Owner.DataGridView.AreAllCellsSelected(false))
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000AA1 RID: 2721
			// (get) Token: 0x06003891 RID: 14481 RVA: 0x000CEB5C File Offset: 0x000CDB5C
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x06003892 RID: 14482 RVA: 0x000CEB63 File Offset: 0x000CDB63
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				base.Owner.DataGridView.SelectAll();
			}

			// Token: 0x06003893 RID: 14483 RVA: 0x000CEB78 File Offset: 0x000CDB78
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				switch (navigationDirection)
				{
				case AccessibleNavigation.Left:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return null;
					}
					return this.NavigateForward();
				case AccessibleNavigation.Right:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward();
					}
					return null;
				case AccessibleNavigation.Next:
					return this.NavigateForward();
				case AccessibleNavigation.Previous:
					return null;
				default:
					return null;
				}
			}

			// Token: 0x06003894 RID: 14484 RVA: 0x000CEBE1 File Offset: 0x000CDBE1
			private AccessibleObject NavigateForward()
			{
				if (base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
				{
					return null;
				}
				return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(1);
			}

			// Token: 0x06003895 RID: 14485 RVA: 0x000CEC1C File Offset: 0x000CDC1C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					base.Owner.DataGridView.FocusInternal();
					if (base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0 && base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
					{
						DataGridViewRow dataGridViewRow = base.Owner.DataGridView.Rows[base.Owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible)];
						DataGridViewColumn firstColumn = base.Owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
						base.Owner.DataGridView.SetCurrentCellAddressCoreInternal(firstColumn.Index, dataGridViewRow.Index, false, true, false);
					}
				}
				if ((flags & AccessibleSelection.AddSelection) == AccessibleSelection.AddSelection && base.Owner.DataGridView.MultiSelect)
				{
					base.Owner.DataGridView.SelectAll();
				}
				if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection && (flags & AccessibleSelection.AddSelection) == AccessibleSelection.None)
				{
					base.Owner.DataGridView.ClearSelection();
				}
			}
		}
	}
}
