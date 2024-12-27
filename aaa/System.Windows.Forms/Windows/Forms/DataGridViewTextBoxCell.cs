using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200039A RID: 922
	public class DataGridViewTextBoxCell : DataGridViewCell
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x000CCE43 File Offset: 0x000CBE43
		// (set) Token: 0x0600384A RID: 14410 RVA: 0x000CCE5A File Offset: 0x000CBE5A
		private DataGridViewTextBoxEditingControl EditingTextBox
		{
			get
			{
				return (DataGridViewTextBoxEditingControl)base.Properties.GetObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox))
				{
					base.Properties.SetObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox, value);
				}
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x000CCE82 File Offset: 0x000CBE82
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewTextBoxCell.defaultFormattedValueType;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600384C RID: 14412 RVA: 0x000CCE8C File Offset: 0x000CBE8C
		// (set) Token: 0x0600384D RID: 14413 RVA: 0x000CCEB8 File Offset: 0x000CBEB8
		[DefaultValue(32767)]
		public virtual int MaxInputLength
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewTextBoxCell.PropTextBoxCellMaxInputLength, out flag);
				if (flag)
				{
					return integer;
				}
				return 32767;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaxInputLength", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxInputLength",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				base.Properties.SetInteger(DataGridViewTextBoxCell.PropTextBoxCellMaxInputLength, value);
				if (this.OwnsEditingTextBox(base.RowIndex))
				{
					this.EditingTextBox.MaxLength = value;
				}
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600384E RID: 14414 RVA: 0x000CCF3C File Offset: 0x000CBF3C
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewTextBoxCell.defaultValueType;
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000CCF5A File Offset: 0x000CBF5A
		internal override void CacheEditingControl()
		{
			this.EditingTextBox = base.DataGridView.EditingControl as DataGridViewTextBoxEditingControl;
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000CCF74 File Offset: 0x000CBF74
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewTextBoxCell dataGridViewTextBoxCell;
			if (type == DataGridViewTextBoxCell.cellType)
			{
				dataGridViewTextBoxCell = new DataGridViewTextBoxCell();
			}
			else
			{
				dataGridViewTextBoxCell = (DataGridViewTextBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewTextBoxCell);
			dataGridViewTextBoxCell.MaxInputLength = this.MaxInputLength;
			return dataGridViewTextBoxCell;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000CCFB8 File Offset: 0x000CBFB8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			TextBox textBox = dataGridView.EditingControl as TextBox;
			if (textBox != null)
			{
				textBox.ClearUndo();
			}
			this.EditingTextBox = null;
			base.DetachEditingControl();
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x000CD000 File Offset: 0x000CC000
		private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
		{
			TextBox textBox = base.DataGridView.EditingControl as TextBox;
			int width = editingControlBounds.Width;
			if (textBox != null)
			{
				DataGridViewContentAlignment alignment = cellStyle.Alignment;
				if (alignment <= DataGridViewContentAlignment.MiddleCenter)
				{
					switch (alignment)
					{
					case DataGridViewContentAlignment.TopLeft:
						break;
					case DataGridViewContentAlignment.TopCenter:
						goto IL_00F9;
					case (DataGridViewContentAlignment)3:
						goto IL_017B;
					case DataGridViewContentAlignment.TopRight:
						goto IL_0120;
					default:
						if (alignment != DataGridViewContentAlignment.MiddleLeft)
						{
							if (alignment != DataGridViewContentAlignment.MiddleCenter)
							{
								goto IL_017B;
							}
							goto IL_00F9;
						}
						break;
					}
				}
				else if (alignment <= DataGridViewContentAlignment.BottomLeft)
				{
					if (alignment == DataGridViewContentAlignment.MiddleRight)
					{
						goto IL_0120;
					}
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						goto IL_017B;
					}
				}
				else
				{
					if (alignment == DataGridViewContentAlignment.BottomCenter)
					{
						goto IL_00F9;
					}
					if (alignment != DataGridViewContentAlignment.BottomRight)
					{
						goto IL_017B;
					}
					goto IL_0120;
				}
				if (base.DataGridView.RightToLeftInternal)
				{
					editingControlBounds.X++;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3 - 2);
					goto IL_017B;
				}
				editingControlBounds.X += 3;
				editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3 - 1);
				goto IL_017B;
				IL_00F9:
				editingControlBounds.X++;
				editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 3);
				goto IL_017B;
				IL_0120:
				if (base.DataGridView.RightToLeftInternal)
				{
					editingControlBounds.X += 3;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 4);
				}
				else
				{
					editingControlBounds.X++;
					editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 4 - 1);
				}
				IL_017B:
				DataGridViewContentAlignment alignment2 = cellStyle.Alignment;
				if (alignment2 > DataGridViewContentAlignment.MiddleCenter)
				{
					if (alignment2 <= DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment2 == DataGridViewContentAlignment.MiddleRight)
						{
							goto IL_0205;
						}
						if (alignment2 != DataGridViewContentAlignment.BottomLeft)
						{
							goto IL_022C;
						}
					}
					else if (alignment2 != DataGridViewContentAlignment.BottomCenter && alignment2 != DataGridViewContentAlignment.BottomRight)
					{
						goto IL_022C;
					}
					editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 1);
					goto IL_022C;
				}
				switch (alignment2)
				{
				case DataGridViewContentAlignment.TopLeft:
				case DataGridViewContentAlignment.TopCenter:
				case DataGridViewContentAlignment.TopRight:
					editingControlBounds.Y += 2;
					editingControlBounds.Height = Math.Max(0, editingControlBounds.Height - 2);
					goto IL_022C;
				case (DataGridViewContentAlignment)3:
					goto IL_022C;
				default:
					if (alignment2 != DataGridViewContentAlignment.MiddleLeft && alignment2 != DataGridViewContentAlignment.MiddleCenter)
					{
						goto IL_022C;
					}
					break;
				}
				IL_0205:
				editingControlBounds.Height++;
				IL_022C:
				int num;
				if (cellStyle.WrapMode == DataGridViewTriState.False)
				{
					num = textBox.PreferredSize.Height;
				}
				else
				{
					string text = (string)((IDataGridViewEditingControl)textBox).GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
					if (string.IsNullOrEmpty(text))
					{
						text = " ";
					}
					TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
					using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
					{
						num = DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, width, textFormatFlags);
					}
				}
				if (num < editingControlBounds.Height)
				{
					DataGridViewContentAlignment alignment3 = cellStyle.Alignment;
					if (alignment3 > DataGridViewContentAlignment.MiddleCenter)
					{
						if (alignment3 <= DataGridViewContentAlignment.BottomLeft)
						{
							if (alignment3 == DataGridViewContentAlignment.MiddleRight)
							{
								goto IL_031C;
							}
							if (alignment3 != DataGridViewContentAlignment.BottomLeft)
							{
								return editingControlBounds;
							}
						}
						else if (alignment3 != DataGridViewContentAlignment.BottomCenter && alignment3 != DataGridViewContentAlignment.BottomRight)
						{
							return editingControlBounds;
						}
						editingControlBounds.Y += editingControlBounds.Height - num;
						return editingControlBounds;
					}
					switch (alignment3)
					{
					case DataGridViewContentAlignment.TopLeft:
					case DataGridViewContentAlignment.TopCenter:
					case (DataGridViewContentAlignment)3:
					case DataGridViewContentAlignment.TopRight:
						return editingControlBounds;
					default:
						if (alignment3 != DataGridViewContentAlignment.MiddleLeft && alignment3 != DataGridViewContentAlignment.MiddleCenter)
						{
							return editingControlBounds;
						}
						break;
					}
					IL_031C:
					editingControlBounds.Y += (editingControlBounds.Height - num) / 2;
				}
			}
			return editingControlBounds;
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000CD36C File Offset: 0x000CC36C
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
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, formattedValue, null, cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x000CD3E0 File Offset: 0x000CC3E0
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

		// Token: 0x06003855 RID: 14421 RVA: 0x000CD458 File Offset: 0x000CC458
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
			object formattedValue = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize);
			string text = formattedValue as string;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			Size size;
			if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1)
			{
				switch (freeDimensionFromConstraint)
				{
				case DataGridViewFreeDimension.Height:
					size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Width - num), textFormatFlags));
					break;
				case DataGridViewFreeDimension.Width:
					size = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Height - num2 - 1 - 1), textFormatFlags), 0);
					break;
				default:
					size = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, textFormatFlags);
					break;
				}
			}
			else
			{
				switch (freeDimensionFromConstraint)
				{
				case DataGridViewFreeDimension.Height:
					size = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Height);
					break;
				case DataGridViewFreeDimension.Width:
					size = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Width, 0);
					break;
				default:
					size = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags);
					break;
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				size.Width += num;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Width = Math.Max(size.Width, num + 8 + 12);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				int num3 = ((cellStyle.WrapMode == DataGridViewTriState.True) ? 1 : 2);
				size.Height += num3 + 1 + num2;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000CD6B4 File Offset: 0x000CC6B4
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			TextBox textBox = base.DataGridView.EditingControl as TextBox;
			if (textBox != null)
			{
				textBox.BorderStyle = BorderStyle.None;
				textBox.AcceptsReturn = (textBox.Multiline = dataGridViewCellStyle.WrapMode == DataGridViewTriState.True);
				textBox.MaxLength = this.MaxInputLength;
				string text = initialFormattedValue as string;
				if (text == null)
				{
					textBox.Text = string.Empty;
				}
				else
				{
					textBox.Text = text;
				}
				this.EditingTextBox = base.DataGridView.EditingControl as DataGridViewTextBoxEditingControl;
			}
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x000CD740 File Offset: 0x000CC740
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			return (((char.IsLetterOrDigit((char)e.KeyCode) && (e.KeyCode < Keys.F1 || e.KeyCode > Keys.F24)) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.Divide) || (e.KeyCode >= Keys.OemSemicolon && e.KeyCode <= Keys.OemBackslash) || (e.KeyCode == Keys.Space && !e.Shift)) && !e.Alt && !e.Control) || base.KeyEntersEditMode(e);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000CD7CB File Offset: 0x000CC7CB
		protected override void OnEnter(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (throughMouseClick)
			{
				this.flagsState |= 1;
			}
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x000CD7E8 File Offset: 0x000CC7E8
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			this.flagsState = (byte)((int)this.flagsState & -2);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000CD804 File Offset: 0x000CC804
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == e.ColumnIndex && currentCellAddress.Y == e.RowIndex && e.Button == MouseButtons.Left)
			{
				if ((this.flagsState & 1) != 0)
				{
					this.flagsState = (byte)((int)this.flagsState & -2);
					return;
				}
				if (base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
				{
					base.DataGridView.BeginEdit(true);
				}
			}
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000CD887 File Offset: 0x000CC887
		private bool OwnsEditingTextBox(int rowIndex)
		{
			return rowIndex != -1 && this.EditingTextBox != null && rowIndex == ((IDataGridViewEditingControl)this.EditingTextBox).EditingControlRowIndex;
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000CD8A8 File Offset: 0x000CC8A8
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000CD8E0 File Offset: 0x000CC8E0
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle rectangle = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle3 = cellBounds;
			rectangle3.Offset(rectangle2.X, rectangle2.Y);
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Height -= rectangle2.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag2 = flag && base.DataGridView.EditingControl != null;
			bool flag3 = (cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			SolidBrush solidBrush;
			if (DataGridViewCell.PaintSelectionBackground(paintParts) && flag3 && !flag2)
			{
				solidBrush = base.DataGridView.GetCachedBrush(cellStyle.SelectionBackColor);
			}
			else
			{
				solidBrush = base.DataGridView.GetCachedBrush(cellStyle.BackColor);
			}
			if (paint && DataGridViewCell.PaintBackground(paintParts) && solidBrush.Color.A == 255 && rectangle3.Width > 0 && rectangle3.Height > 0)
			{
				graphics.FillRectangle(solidBrush, rectangle3);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle3.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle3.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle3.Width -= cellStyle.Padding.Horizontal;
				rectangle3.Height -= cellStyle.Padding.Vertical;
			}
			if (paint && flag && !flag2 && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && rectangle3.Width > 0 && rectangle3.Height > 0)
			{
				ControlPaint.DrawFocusRectangle(graphics, rectangle3, Color.Empty, solidBrush.Color);
			}
			Rectangle rectangle4 = rectangle3;
			string text = formattedValue as string;
			if (text != null && ((paint && !flag2) || computeContentBounds))
			{
				int num = ((cellStyle.WrapMode == DataGridViewTriState.True) ? 1 : 2);
				rectangle3.Offset(0, num);
				rectangle3.Width = rectangle3.Width;
				rectangle3.Height -= num + 1;
				if (rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
					if (paint)
					{
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
							{
								textFormatFlags |= TextFormatFlags.EndEllipsis;
							}
							TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle3, flag3 ? cellStyle.SelectionForeColor : cellStyle.ForeColor, textFormatFlags);
						}
					}
					else
					{
						rectangle = DataGridViewUtilities.GetTextBounds(rectangle3, text, textFormatFlags, cellStyle);
					}
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

		// Token: 0x0600385E RID: 14430 RVA: 0x000CDC54 File Offset: 0x000CCC54
		public override void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			Rectangle rectangle = this.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			rectangle = this.GetAdjustedEditingControlBounds(rectangle, cellStyle);
			base.DataGridView.EditingControl.Location = new Point(rectangle.X, rectangle.Y);
			base.DataGridView.EditingControl.Size = new Size(rectangle.Width, rectangle.Height);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x000CDCC8 File Offset: 0x000CCCC8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewTextBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04001C61 RID: 7265
		private const byte DATAGRIDVIEWTEXTBOXCELL_ignoreNextMouseClick = 1;

		// Token: 0x04001C62 RID: 7266
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextOffsetLeft = 3;

		// Token: 0x04001C63 RID: 7267
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextOffsetRight = 4;

		// Token: 0x04001C64 RID: 7268
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextMarginLeft = 0;

		// Token: 0x04001C65 RID: 7269
		private const byte DATAGRIDVIEWTEXTBOXCELL_horizontalTextMarginRight = 0;

		// Token: 0x04001C66 RID: 7270
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextOffsetTop = 2;

		// Token: 0x04001C67 RID: 7271
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextOffsetBottom = 1;

		// Token: 0x04001C68 RID: 7272
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginTopWithWrapping = 1;

		// Token: 0x04001C69 RID: 7273
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginTopWithoutWrapping = 2;

		// Token: 0x04001C6A RID: 7274
		private const byte DATAGRIDVIEWTEXTBOXCELL_verticalTextMarginBottom = 1;

		// Token: 0x04001C6B RID: 7275
		private const int DATAGRIDVIEWTEXTBOXCELL_maxInputLength = 32767;

		// Token: 0x04001C6C RID: 7276
		private static readonly int PropTextBoxCellMaxInputLength = PropertyStore.CreateKey();

		// Token: 0x04001C6D RID: 7277
		private static readonly int PropTextBoxCellEditingTextBox = PropertyStore.CreateKey();

		// Token: 0x04001C6E RID: 7278
		private byte flagsState;

		// Token: 0x04001C6F RID: 7279
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04001C70 RID: 7280
		private static Type defaultValueType = typeof(object);

		// Token: 0x04001C71 RID: 7281
		private static Type cellType = typeof(DataGridViewTextBoxCell);
	}
}
