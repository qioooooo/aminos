using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Internal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200030A RID: 778
	public class DataGridViewButtonCell : DataGridViewCell
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003280 RID: 12928 RVA: 0x000B077C File Offset: 0x000AF77C
		// (set) Token: 0x06003281 RID: 12929 RVA: 0x000B07A2 File Offset: 0x000AF7A2
		private ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellState, out flag);
				if (flag)
				{
					return (ButtonState)integer;
				}
				return ButtonState.Normal;
			}
			set
			{
				if (this.ButtonState != value)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellState, (int)value);
				}
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x000B07BE File Offset: 0x000AF7BE
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x000B07C4 File Offset: 0x000AF7C4
		// (set) Token: 0x06003284 RID: 12932 RVA: 0x000B07EC File Offset: 0x000AF7EC
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, out flag);
				if (flag)
				{
					return (FlatStyle)integer;
				}
				return FlatStyle.Standard;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (set) Token: 0x06003285 RID: 12933 RVA: 0x000B083F File Offset: 0x000AF83F
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellFlatStyle, (int)value);
				}
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x000B085B File Offset: 0x000AF85B
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewButtonCell.defaultFormattedValueType;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x000B0864 File Offset: 0x000AF864
		// (set) Token: 0x06003288 RID: 12936 RVA: 0x000B088F File Offset: 0x000AF88F
		[DefaultValue(false)]
		public bool UseColumnTextForButtonValue
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.UseColumnTextForButtonValue)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, value ? 1 : 0);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170008CA RID: 2250
		// (set) Token: 0x06003289 RID: 12937 RVA: 0x000B08B7 File Offset: 0x000AF8B7
		internal bool UseColumnTextForButtonValueInternal
		{
			set
			{
				if (value != this.UseColumnTextForButtonValue)
				{
					base.Properties.SetInteger(DataGridViewButtonCell.PropButtonCellUseColumnTextForButtonValue, value ? 1 : 0);
				}
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x000B08DC File Offset: 0x000AF8DC
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewButtonCell.defaultValueType;
			}
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000B08FC File Offset: 0x000AF8FC
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewButtonCell dataGridViewButtonCell;
			if (type == DataGridViewButtonCell.cellType)
			{
				dataGridViewButtonCell = new DataGridViewButtonCell();
			}
			else
			{
				dataGridViewButtonCell = (DataGridViewButtonCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewButtonCell);
			dataGridViewButtonCell.FlatStyleInternal = this.FlatStyle;
			dataGridViewButtonCell.UseColumnTextForButtonValueInternal = this.UseColumnTextForButtonValue;
			return dataGridViewButtonCell;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000B094C File Offset: 0x000AF94C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewButtonCell.DataGridViewButtonCellAccessibleObject(this);
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000B0954 File Offset: 0x000AF954
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, null, null, cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000B09AC File Offset: 0x000AF9AC
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null || !base.DataGridView.ShowCellErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, null, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000B0A24 File Offset: 0x000AFA24
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			string text = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			int num3;
			int num4;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				Rectangle themeMargins = DataGridViewButtonCell.GetThemeMargins(graphics);
				num3 = themeMargins.X + themeMargins.Width;
				num4 = themeMargins.Y + themeMargins.Height;
			}
			else
			{
				num4 = (num3 = 5);
			}
			Size size;
			switch (freeDimensionFromConstraint)
			{
			case DataGridViewFreeDimension.Height:
				if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1 && constraintSize.Width - num - num3 - 4 > 0)
				{
					size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, constraintSize.Width - num - num3 - 4, textFormatFlags));
				}
				else
				{
					size = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Height);
				}
				break;
			case DataGridViewFreeDimension.Width:
				if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1 && constraintSize.Height - num2 - num4 - 2 > 0)
				{
					size = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, constraintSize.Height - num2 - num4 - 2, textFormatFlags), 0);
				}
				else
				{
					size = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Width, 0);
				}
				break;
			default:
				if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1)
				{
					size = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, textFormatFlags);
				}
				else
				{
					size = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags);
				}
				break;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				size.Width += num + num3 + 4;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Width = Math.Max(size.Width, num + 8 + 12);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				size.Height += num2 + num4 + 2;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000B0CD8 File Offset: 0x000AFCD8
		private static Rectangle GetThemeMargins(Graphics g)
		{
			if (DataGridViewButtonCell.rectThemeMargins.X == -1)
			{
				Rectangle rectangle = new Rectangle(0, 0, 100, 100);
				Rectangle backgroundContentRectangle = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetBackgroundContentRectangle(g, rectangle);
				DataGridViewButtonCell.rectThemeMargins.X = backgroundContentRectangle.X;
				DataGridViewButtonCell.rectThemeMargins.Y = backgroundContentRectangle.Y;
				DataGridViewButtonCell.rectThemeMargins.Width = 100 - backgroundContentRectangle.Right;
				DataGridViewButtonCell.rectThemeMargins.Height = 100 - backgroundContentRectangle.Bottom;
			}
			return DataGridViewButtonCell.rectThemeMargins;
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000B0D5C File Offset: 0x000AFD5C
		protected override object GetValue(int rowIndex)
		{
			if (this.UseColumnTextForButtonValue && base.DataGridView != null && base.DataGridView.NewRowIndex != rowIndex && base.OwningColumn != null && base.OwningColumn is DataGridViewButtonColumn)
			{
				return ((DataGridViewButtonColumn)base.OwningColumn).Text;
			}
			return base.GetValue(rowIndex);
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x000B0DB4 File Offset: 0x000AFDB4
		protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x000B0DDB File Offset: 0x000AFDDB
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000B0DE7 File Offset: 0x000AFDE7
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000B0DF8 File Offset: 0x000AFDF8
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000B0E38 File Offset: 0x000AFE38
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return (this.ButtonState & ButtonState.Pushed) != ButtonState.Normal;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000B0E4C File Offset: 0x000AFE4C
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x000B0E5C File Offset: 0x000AFE5C
		protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Checked, rowIndex);
				e.Handled = true;
			}
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000B0EB0 File Offset: 0x000AFEB0
		protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Checked, rowIndex);
				if (!e.Alt && !e.Control && !e.Shift)
				{
					base.RaiseCellClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					if (base.DataGridView != null && base.ColumnIndex < base.DataGridView.Columns.Count && rowIndex < base.DataGridView.Rows.Count)
					{
						base.RaiseCellContentClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000B0F5C File Offset: 0x000AFF5C
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.ButtonState != ButtonState.Normal)
			{
				this.UpdateButtonState(ButtonState.Normal, rowIndex);
			}
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000B0F77 File Offset: 0x000AFF77
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && DataGridViewButtonCell.mouseInContentBounds)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
			}
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000B0FB0 File Offset: 0x000AFFB0
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewButtonCell.mouseInContentBounds)
			{
				DataGridViewButtonCell.mouseInContentBounds = false;
				if (base.ColumnIndex >= 0 && rowIndex >= 0 && (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
				}
			}
			if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, rowIndex);
			}
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x000B1064 File Offset: 0x000B0064
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			bool flag = DataGridViewButtonCell.mouseInContentBounds;
			DataGridViewButtonCell.mouseInContentBounds = base.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (flag != DataGridViewButtonCell.mouseInContentBounds)
			{
				if (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (e.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && e.RowIndex == base.DataGridView.MouseDownCellAddress.Y && Control.MouseButtons == MouseButtons.Left)
				{
					if ((this.ButtonState & ButtonState.Pushed) == ButtonState.Normal && DataGridViewButtonCell.mouseInContentBounds && base.DataGridView.CellMouseDownInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
					}
					else if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && !DataGridViewButtonCell.mouseInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
					}
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000B119A File Offset: 0x000B019A
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left)
			{
				this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
			}
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x000B11CC File Offset: 0x000B01CC
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x000B1204 File Offset: 0x000B0204
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = (elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			bool flag2 = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			string text = formattedValue as string;
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
			SolidBrush cachedBrush2 = base.DataGridView.GetCachedBrush(flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			Rectangle rectangle4;
			if (rectangle.Height > 0 && rectangle.Width > 0)
			{
				if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
				{
					g.FillRectangle(cachedBrush, rectangle);
				}
				if (cellStyle.Padding != Padding.Empty)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
					}
					else
					{
						rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
					}
					rectangle.Width -= cellStyle.Padding.Horizontal;
					rectangle.Height -= cellStyle.Padding.Vertical;
				}
				Rectangle rectangle3 = rectangle;
				if (rectangle.Height > 0 && rectangle.Width > 0 && (paint || computeContentBounds))
				{
					if (this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System)
					{
						if (base.DataGridView.ApplyVisualStylesToInnerCells)
						{
							if (paint && DataGridViewCell.PaintContentBackground(paintParts))
							{
								PushButtonState pushButtonState = PushButtonState.Normal;
								if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
								{
									pushButtonState = PushButtonState.Pressed;
								}
								else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
								{
									pushButtonState = PushButtonState.Hot;
								}
								if (DataGridViewCell.PaintFocus(paintParts) && flag2 && base.DataGridView.ShowFocusCues && base.DataGridView.Focused)
								{
									pushButtonState |= PushButtonState.Default;
								}
								DataGridViewButtonCell.DataGridViewButtonCellRenderer.DrawButton(g, rectangle, (int)pushButtonState);
							}
							rectangle4 = rectangle;
							rectangle = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetBackgroundContentRectangle(g, rectangle);
						}
						else
						{
							if (paint && DataGridViewCell.PaintContentBackground(paintParts))
							{
								ControlPaint.DrawBorder(g, rectangle, SystemColors.Control, (this.ButtonState == ButtonState.Normal) ? ButtonBorderStyle.Outset : ButtonBorderStyle.Inset);
							}
							rectangle4 = rectangle;
							rectangle.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
						}
					}
					else if (this.FlatStyle == FlatStyle.Flat)
					{
						rectangle.Inflate(-1, -1);
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, cachedBrush2.Color, true);
							if (cachedBrush.Color.A == 255)
							{
								if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
								{
									ButtonBaseAdapter.ColorData colorData = ButtonBaseAdapter.PaintFlatRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
									IntPtr hdc = g.GetHdc();
									try
									{
										using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
										{
											WindowsBrush windowsBrush;
											if (colorData.options.highContrast)
											{
												windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, colorData.buttonShadow);
											}
											else
											{
												windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, colorData.lowHighlight);
											}
											try
											{
												ButtonBaseAdapter.PaintButtonBackground(windowsGraphics, rectangle, windowsBrush);
											}
											finally
											{
												windowsBrush.Dispose();
											}
										}
										goto IL_04CF;
									}
									finally
									{
										g.ReleaseHdc();
									}
								}
								if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
								{
									IntPtr hdc2 = g.GetHdc();
									try
									{
										using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromHdc(hdc2))
										{
											Color controlDark = SystemColors.ControlDark;
											using (WindowsBrush windowsBrush2 = new WindowsSolidBrush(windowsGraphics2.DeviceContext, controlDark))
											{
												ButtonBaseAdapter.PaintButtonBackground(windowsGraphics2, rectangle, windowsBrush2);
											}
										}
									}
									finally
									{
										g.ReleaseHdc();
									}
								}
							}
						}
						IL_04CF:
						rectangle4 = rectangle;
					}
					else
					{
						rectangle.Inflate(-1, -1);
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
							{
								ButtonBaseAdapter.ColorData colorData2 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData2.options.highContrast ? colorData2.windowText : colorData2.windowFrame, true);
								ControlPaint.DrawBorder(g, rectangle, colorData2.options.highContrast ? colorData2.windowText : colorData2.buttonShadow, ButtonBorderStyle.Solid);
							}
							else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewButtonCell.mouseInContentBounds)
							{
								ButtonBaseAdapter.ColorData colorData3 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData3.options.highContrast ? colorData3.windowText : colorData3.buttonShadow, false);
								ButtonBaseAdapter.Draw3DLiteBorder(g, rectangle, colorData3, true);
							}
							else
							{
								ButtonBaseAdapter.ColorData colorData4 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
								ButtonBaseAdapter.DrawDefaultBorder(g, rectangle, colorData4.options.highContrast ? colorData4.windowText : colorData4.buttonShadow, false);
								ButtonBaseAdapter.DrawFlatBorder(g, rectangle, colorData4.options.highContrast ? colorData4.windowText : colorData4.buttonShadow);
							}
						}
						rectangle4 = rectangle;
					}
				}
				else if (computeErrorIconBounds)
				{
					if (!string.IsNullOrEmpty(errorText))
					{
						rectangle4 = base.ComputeErrorIconBounds(rectangle3);
					}
					else
					{
						rectangle4 = Rectangle.Empty;
					}
				}
				else
				{
					rectangle4 = Rectangle.Empty;
				}
				if (paint && DataGridViewCell.PaintFocus(paintParts) && flag2 && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && rectangle.Width > 2 * SystemInformation.Border3DSize.Width + 1 && rectangle.Height > 2 * SystemInformation.Border3DSize.Height + 1)
				{
					if (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard)
					{
						ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(rectangle, -1, -1), Color.Empty, SystemColors.Control);
					}
					else if (this.FlatStyle == FlatStyle.Flat)
					{
						if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal || (base.DataGridView.CurrentCellAddress.Y == rowIndex && base.DataGridView.CurrentCellAddress.X == base.ColumnIndex))
						{
							ButtonBaseAdapter.ColorData colorData5 = ButtonBaseAdapter.PaintFlatRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							string text2 = ((text != null) ? text : string.Empty);
							ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonFlatAdapter.PaintFlatLayout(g, true, SystemInformation.HighContrast, 1, rectangle, Padding.Empty, false, cellStyle.Font, text2, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
							layoutOptions.everettButtonCompat = false;
							ButtonBaseAdapter.LayoutData layoutData = layoutOptions.Layout();
							ButtonBaseAdapter.DrawFlatFocus(g, layoutData.focus, colorData5.options.highContrast ? colorData5.windowText : colorData5.constrastButtonShadow);
						}
					}
					else if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal || (base.DataGridView.CurrentCellAddress.Y == rowIndex && base.DataGridView.CurrentCellAddress.X == base.ColumnIndex))
					{
						bool flag3 = this.ButtonState == ButtonState.Normal;
						string text3 = ((text != null) ? text : string.Empty);
						ButtonBaseAdapter.LayoutOptions layoutOptions2 = ButtonPopupAdapter.PaintPopupLayout(g, flag3, SystemInformation.HighContrast ? 2 : 1, rectangle, Padding.Empty, false, cellStyle.Font, text3, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
						layoutOptions2.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData2 = layoutOptions2.Layout();
						ControlPaint.DrawFocusRectangle(g, layoutData2.focus, cellStyle.ForeColor, cellStyle.BackColor);
					}
				}
				if (text != null && paint && DataGridViewCell.PaintContentForeground(paintParts))
				{
					rectangle.Offset(2, 1);
					rectangle.Width -= 4;
					rectangle.Height -= 2;
					if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal && this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup)
					{
						rectangle.Offset(1, 1);
						rectangle.Width--;
						rectangle.Height--;
					}
					if (rectangle.Width > 0 && rectangle.Height > 0)
					{
						Color color;
						if (base.DataGridView.ApplyVisualStylesToInnerCells && (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard))
						{
							color = DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.GetColor(ColorProperty.TextColor);
						}
						else
						{
							color = cachedBrush2.Color;
						}
						TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						TextRenderer.DrawText(g, text, cellStyle.Font, rectangle, color, textFormatFlags);
					}
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle3, errorText);
				}
			}
			else
			{
				rectangle4 = Rectangle.Empty;
			}
			return rectangle4;
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x000B1CD0 File Offset: 0x000B0CD0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewButtonCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x000B1D2E File Offset: 0x000B0D2E
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			if (this.ButtonState != newButtonState)
			{
				this.ButtonState = newButtonState;
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x04001A6F RID: 6767
		private const byte DATAGRIDVIEWBUTTONCELL_themeMargin = 100;

		// Token: 0x04001A70 RID: 6768
		private const byte DATAGRIDVIEWBUTTONCELL_horizontalTextMargin = 2;

		// Token: 0x04001A71 RID: 6769
		private const byte DATAGRIDVIEWBUTTONCELL_verticalTextMargin = 1;

		// Token: 0x04001A72 RID: 6770
		private const byte DATAGRIDVIEWBUTTONCELL_textPadding = 5;

		// Token: 0x04001A73 RID: 6771
		private static readonly int PropButtonCellFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04001A74 RID: 6772
		private static readonly int PropButtonCellState = PropertyStore.CreateKey();

		// Token: 0x04001A75 RID: 6773
		private static readonly int PropButtonCellUseColumnTextForButtonValue = PropertyStore.CreateKey();

		// Token: 0x04001A76 RID: 6774
		private static readonly VisualStyleElement ButtonElement = VisualStyleElement.Button.PushButton.Normal;

		// Token: 0x04001A77 RID: 6775
		private static Rectangle rectThemeMargins = new Rectangle(-1, -1, 0, 0);

		// Token: 0x04001A78 RID: 6776
		private static bool mouseInContentBounds = false;

		// Token: 0x04001A79 RID: 6777
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04001A7A RID: 6778
		private static Type defaultValueType = typeof(object);

		// Token: 0x04001A7B RID: 6779
		private static Type cellType = typeof(DataGridViewButtonCell);

		// Token: 0x0200030B RID: 779
		private class DataGridViewButtonCellRenderer
		{
			// Token: 0x060032A4 RID: 12964 RVA: 0x000B1DCA File Offset: 0x000B0DCA
			private DataGridViewButtonCellRenderer()
			{
			}

			// Token: 0x170008CC RID: 2252
			// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000B1DD2 File Offset: 0x000B0DD2
			public static VisualStyleRenderer DataGridViewButtonRenderer
			{
				get
				{
					if (DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewButtonCell.ButtonElement);
					}
					return DataGridViewButtonCell.DataGridViewButtonCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x060032A6 RID: 12966 RVA: 0x000B1DEF File Offset: 0x000B0DEF
			public static void DrawButton(Graphics g, Rectangle bounds, int buttonState)
			{
				DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.SetParameters(DataGridViewButtonCell.ButtonElement.ClassName, DataGridViewButtonCell.ButtonElement.Part, buttonState);
				DataGridViewButtonCell.DataGridViewButtonCellRenderer.DataGridViewButtonRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04001A7C RID: 6780
			private static VisualStyleRenderer visualStyleRenderer;
		}

		// Token: 0x0200030C RID: 780
		protected class DataGridViewButtonCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x060032A7 RID: 12967 RVA: 0x000B1E27 File Offset: 0x000B0E27
			public DataGridViewButtonCellAccessibleObject(DataGridViewCell owner)
				: base(owner)
			{
			}

			// Token: 0x170008CD RID: 2253
			// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000B1E30 File Offset: 0x000B0E30
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("DataGridView_AccButtonCellDefaultAction");
				}
			}

			// Token: 0x060032A9 RID: 12969 RVA: 0x000B1E3C File Offset: 0x000B0E3C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewButtonCell dataGridViewButtonCell = (DataGridViewButtonCell)base.Owner;
				DataGridView dataGridView = dataGridViewButtonCell.DataGridView;
				if (dataGridView != null && dataGridViewButtonCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (dataGridViewButtonCell.OwningColumn != null && dataGridViewButtonCell.OwningRow != null)
				{
					dataGridView.OnCellClickInternal(new DataGridViewCellEventArgs(dataGridViewButtonCell.ColumnIndex, dataGridViewButtonCell.RowIndex));
					dataGridView.OnCellContentClickInternal(new DataGridViewCellEventArgs(dataGridViewButtonCell.ColumnIndex, dataGridViewButtonCell.RowIndex));
				}
			}

			// Token: 0x060032AA RID: 12970 RVA: 0x000B1EB6 File Offset: 0x000B0EB6
			public override int GetChildCount()
			{
				return 0;
			}
		}
	}
}
