using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000338 RID: 824
	public class DataGridViewColumnHeaderCell : DataGridViewHeaderCell
	{
		// Token: 0x060034A3 RID: 13475 RVA: 0x000BA5AF File Offset: 0x000B95AF
		public DataGridViewColumnHeaderCell()
		{
			this.sortGlyphDirection = SortOrder.None;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060034A4 RID: 13476 RVA: 0x000BA5BE File Offset: 0x000B95BE
		internal bool ContainsLocalValue
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewCell.PropCellValue);
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060034A5 RID: 13477 RVA: 0x000BA5D0 File Offset: 0x000B95D0
		// (set) Token: 0x060034A6 RID: 13478 RVA: 0x000BA5D8 File Offset: 0x000B95D8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SortOrder SortGlyphDirection
		{
			get
			{
				return this.sortGlyphDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
				}
				if (base.OwningColumn == null || base.DataGridView == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_CellDoesNotYetBelongToDataGridView"));
				}
				if (value != this.sortGlyphDirection)
				{
					if (base.OwningColumn.SortMode == DataGridViewColumnSortMode.NotSortable && value != SortOrder.None)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash", new object[] { value.ToString() }));
					}
					this.sortGlyphDirection = value;
					base.DataGridView.OnSortGlyphDirectionChanged(this);
				}
			}
		}

		// Token: 0x17000983 RID: 2435
		// (set) Token: 0x060034A7 RID: 13479 RVA: 0x000BA67D File Offset: 0x000B967D
		internal SortOrder SortGlyphDirectionInternal
		{
			set
			{
				this.sortGlyphDirection = value;
			}
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000BA688 File Offset: 0x000B9688
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell;
			if (type == DataGridViewColumnHeaderCell.cellType)
			{
				dataGridViewColumnHeaderCell = new DataGridViewColumnHeaderCell();
			}
			else
			{
				dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewColumnHeaderCell);
			dataGridViewColumnHeaderCell.Value = base.Value;
			return dataGridViewColumnHeaderCell;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000BA6CC File Offset: 0x000B96CC
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject(this);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000BA6D4 File Offset: 0x000B96D4
		protected override object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return null;
			}
			object value = this.GetValue(rowIndex);
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (firstCell)
				{
					stringBuilder.Append("<TABLE>");
					stringBuilder.Append("<THEAD>");
				}
				stringBuilder.Append("<TH>");
				if (value != null)
				{
					DataGridViewCell.FormatPlainTextAsHtml(value.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
				}
				else
				{
					stringBuilder.Append("&nbsp;");
				}
				stringBuilder.Append("</TH>");
				if (lastCell)
				{
					stringBuilder.Append("</THEAD>");
					if (inLastRow)
					{
						stringBuilder.Append("</TABLE>");
					}
				}
				return stringBuilder.ToString();
			}
			bool flag = string.Equals(format, DataFormats.CommaSeparatedValue, StringComparison.OrdinalIgnoreCase);
			if (flag || string.Equals(format, DataFormats.Text, StringComparison.OrdinalIgnoreCase) || string.Equals(format, DataFormats.UnicodeText, StringComparison.OrdinalIgnoreCase))
			{
				if (value != null)
				{
					bool flag2 = false;
					int length = stringBuilder.Length;
					DataGridViewCell.FormatPlainText(value.ToString(), flag, new StringWriter(stringBuilder, CultureInfo.CurrentCulture), ref flag2);
					if (flag2)
					{
						stringBuilder.Insert(length, '"');
					}
				}
				if (lastCell)
				{
					if (!inLastRow)
					{
						stringBuilder.Append('\r');
						stringBuilder.Append('\n');
					}
				}
				else
				{
					stringBuilder.Append(flag ? ',' : '\t');
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000BA834 File Offset: 0x000B9834
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
			if (base.DataGridView == null || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, value, cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000BA8A0 File Offset: 0x000B98A0
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			ContextMenuStrip contextMenuStrip = base.GetContextMenuStrip(-1);
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

		// Token: 0x060034AD RID: 13485 RVA: 0x000BA8E0 File Offset: 0x000B98E0
		public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			DataGridViewCellStyle dataGridViewCellStyle = ((inheritedCellStyle == null) ? new DataGridViewCellStyle() : inheritedCellStyle);
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (base.HasStyle)
			{
				dataGridViewCellStyle2 = base.Style;
			}
			DataGridViewCellStyle columnHeadersDefaultCellStyle = base.DataGridView.ColumnHeadersDefaultCellStyle;
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (!columnHeadersDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = columnHeadersDefaultCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (!columnHeadersDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = columnHeadersDefaultCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (!columnHeadersDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = columnHeadersDefaultCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (!columnHeadersDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = columnHeadersDefaultCellStyle.SelectionForeColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionForeColor = defaultCellStyle.SelectionForeColor;
				}
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle2.Font;
			}
			else if (columnHeadersDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = columnHeadersDefaultCellStyle.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (!columnHeadersDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = columnHeadersDefaultCellStyle.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (!columnHeadersDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = columnHeadersDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (columnHeadersDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = columnHeadersDefaultCellStyle.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (!columnHeadersDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = columnHeadersDefaultCellStyle.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (columnHeadersDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = columnHeadersDefaultCellStyle.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (columnHeadersDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = columnHeadersDefaultCellStyle.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (columnHeadersDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = columnHeadersDefaultCellStyle.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (columnHeadersDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = columnHeadersDefaultCellStyle.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000BACA0 File Offset: 0x000B9CA0
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
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = base.DataGridView.AdjustColumnHeaderBorderStyle(base.DataGridView.AdvancedColumnHeadersBorderStyle, dataGridViewAdvancedBorderStyle, false, false);
			Rectangle rectangle = this.BorderWidths(dataGridViewAdvancedBorderStyle2);
			int num = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int num2 = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			string text = this.GetValue(rowIndex) as string;
			Size size;
			switch (freeDimensionFromConstraint)
			{
			case DataGridViewFreeDimension.Height:
			{
				int num3 = constraintSize.Width - num;
				size = new Size(0, 0);
				Size empty;
				if (num3 >= 17 && base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					empty = new Size(17, 7);
				}
				else
				{
					empty = Size.Empty;
				}
				if (num3 - 2 - 2 > 0 && !string.IsNullOrEmpty(text))
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True)
					{
						if (empty.Width > 0 && num3 - 2 - 2 - 2 - empty.Width > 0)
						{
							size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, num3 - 2 - 2 - 2 - empty.Width, textFormatFlags));
						}
						else
						{
							size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, num3 - 2 - 2, textFormatFlags));
						}
					}
					else
					{
						size = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Height);
					}
				}
				size.Height = Math.Max(size.Height, empty.Height);
				size.Height = Math.Max(size.Height, 1);
				break;
			}
			case DataGridViewFreeDimension.Width:
				size = new Size(0, 0);
				if (!string.IsNullOrEmpty(text))
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True)
					{
						size = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Height - num2 - 2), textFormatFlags), 0);
					}
					else
					{
						size = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags).Width, 0);
					}
				}
				if (constraintSize.Height - num2 - 2 > 7 && base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					size.Width += 17;
					if (!string.IsNullOrEmpty(text))
					{
						size.Width += 2;
					}
				}
				size.Width = Math.Max(size.Width, 1);
				break;
			default:
				if (!string.IsNullOrEmpty(text))
				{
					if (cellStyle.WrapMode == DataGridViewTriState.True)
					{
						size = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, textFormatFlags);
					}
					else
					{
						size = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags);
					}
				}
				else
				{
					size = new Size(0, 0);
				}
				if (base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					size.Width += 17;
					if (!string.IsNullOrEmpty(text))
					{
						size.Width += 2;
					}
					size.Height = Math.Max(size.Height, 7);
				}
				size.Width = Math.Max(size.Width, 1);
				size.Height = Math.Max(size.Height, 1);
				break;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (!string.IsNullOrEmpty(text))
				{
					size.Width += 4;
				}
				size.Width += num;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				size.Height += 2 + num2;
			}
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					size.Width += themeMargins.X + themeMargins.Width;
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
				{
					size.Height += themeMargins.Y + themeMargins.Height;
				}
			}
			return size;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000BB0D9 File Offset: 0x000BA0D9
		protected override object GetValue(int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (this.ContainsLocalValue)
			{
				return base.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			if (base.OwningColumn != null)
			{
				return base.OwningColumn.Name;
			}
			return null;
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000BB118 File Offset: 0x000BA118
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, formattedValue, cellStyle, advancedBorderStyle, paintParts, true);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000BB14C File Offset: 0x000BA14C
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object formattedValue, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool paint)
		{
			Rectangle rectangle = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle2 = cellBounds;
			Rectangle rectangle3 = this.BorderWidths(advancedBorderStyle);
			rectangle2.Offset(rectangle3.X, rectangle3.Y);
			rectangle2.Width -= rectangle3.Right;
			rectangle2.Height -= rectangle3.Bottom;
			Rectangle rectangle4 = rectangle2;
			bool flag = (dataGridViewElementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (cellStyle.Padding != Padding.Empty && cellStyle.Padding != Padding.Empty)
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
				if (paint && DataGridViewCell.PaintBackground(paintParts) && rectangle4.Width > 0 && rectangle4.Height > 0)
				{
					int num = 1;
					if ((base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable) || base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						if (base.ButtonState != ButtonState.Normal)
						{
							num = 3;
						}
						else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex)
						{
							num = 2;
						}
						else if (flag)
						{
							num = 3;
						}
					}
					if (base.DataGridView.RightToLeftInternal)
					{
						Bitmap bitmap = base.FlipXPThemesBitmap;
						if (bitmap == null || bitmap.Width < rectangle4.Width || bitmap.Width > 2 * rectangle4.Width || bitmap.Height < rectangle4.Height || bitmap.Height > 2 * rectangle4.Height)
						{
							bitmap = (base.FlipXPThemesBitmap = new Bitmap(rectangle4.Width, rectangle4.Height));
						}
						Graphics graphics = Graphics.FromImage(bitmap);
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.DrawHeader(graphics, new Rectangle(0, 0, rectangle4.Width, rectangle4.Height), num);
						bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
						g.DrawImage(bitmap, rectangle4, new Rectangle(bitmap.Width - rectangle4.Width, 0, rectangle4.Width, rectangle4.Height), GraphicsUnit.Pixel);
					}
					else
					{
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.DrawHeader(g, rectangle4, num);
					}
				}
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(g);
				rectangle2.Y += themeMargins.Y;
				rectangle2.Height -= themeMargins.Y + themeMargins.Height;
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle2.X += themeMargins.Width;
					rectangle2.Width -= themeMargins.X + themeMargins.Width;
				}
				else
				{
					rectangle2.X += themeMargins.X;
					rectangle2.Width -= themeMargins.X + themeMargins.Width;
				}
			}
			else
			{
				if (paint && DataGridViewCell.PaintBackground(paintParts) && rectangle4.Width > 0 && rectangle4.Height > 0)
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (cachedBrush.Color.A == 255)
					{
						g.FillRectangle(cachedBrush, rectangle4);
					}
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
			}
			bool flag2 = false;
			Point point = new Point(0, 0);
			string text = formattedValue as string;
			rectangle2.Y++;
			rectangle2.Height -= 2;
			if (rectangle2.Width - 2 - 2 > 0 && rectangle2.Height > 0 && !string.IsNullOrEmpty(text))
			{
				rectangle2.Offset(2, 0);
				rectangle2.Width -= 4;
				Color color;
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					color = DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
				}
				else
				{
					color = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
				}
				if (base.OwningColumn != null && base.OwningColumn.SortMode != DataGridViewColumnSortMode.NotSortable)
				{
					int num2 = rectangle2.Width - 2 - 9 - 8;
					if (num2 > 0)
					{
						bool flag3;
						int preferredTextHeight = DataGridViewCell.GetPreferredTextHeight(g, base.DataGridView.RightToLeftInternal, text, cellStyle, num2, out flag3);
						if (preferredTextHeight <= rectangle2.Height && !flag3)
						{
							flag2 = this.SortGlyphDirection != SortOrder.None;
							rectangle2.Width -= 19;
							if (base.DataGridView.RightToLeftInternal)
							{
								rectangle2.X += 19;
								point = new Point(rectangle2.Left - 2 - 2 - 4 - 9, rectangle2.Top + (rectangle2.Height - 7) / 2);
							}
							else
							{
								point = new Point(rectangle2.Right + 2 + 2 + 4, rectangle2.Top + (rectangle2.Height - 7) / 2);
							}
						}
					}
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
						TextRenderer.DrawText(g, text, cellStyle.Font, rectangle2, color, textFormatFlags);
					}
				}
				else
				{
					rectangle = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle);
				}
			}
			else if (paint && this.SortGlyphDirection != SortOrder.None && rectangle2.Width >= 17 && rectangle2.Height >= 7)
			{
				flag2 = true;
				point = new Point(rectangle2.Left + (rectangle2.Width - 9) / 2, rectangle2.Top + (rectangle2.Height - 7) / 2);
			}
			if (paint && flag2 && DataGridViewCell.PaintContentBackground(paintParts))
			{
				Pen pen = null;
				Pen pen2 = null;
				base.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
				if (this.SortGlyphDirection == SortOrder.Ascending)
				{
					switch (advancedBorderStyle.Right)
					{
					case DataGridViewAdvancedCellBorderStyle.Inset:
						g.DrawLine(pen2, point.X, point.Y + 7 - 2, point.X + 4 - 1, point.Y);
						g.DrawLine(pen2, point.X + 1, point.Y + 7 - 2, point.X + 4 - 1, point.Y);
						g.DrawLine(pen, point.X + 4, point.Y, point.X + 9 - 2, point.Y + 7 - 2);
						g.DrawLine(pen, point.X + 4, point.Y, point.X + 9 - 3, point.Y + 7 - 2);
						g.DrawLine(pen, point.X, point.Y + 7 - 1, point.X + 9 - 2, point.Y + 7 - 1);
						return rectangle;
					case DataGridViewAdvancedCellBorderStyle.Outset:
					case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
					case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
						g.DrawLine(pen, point.X, point.Y + 7 - 2, point.X + 4 - 1, point.Y);
						g.DrawLine(pen, point.X + 1, point.Y + 7 - 2, point.X + 4 - 1, point.Y);
						g.DrawLine(pen2, point.X + 4, point.Y, point.X + 9 - 2, point.Y + 7 - 2);
						g.DrawLine(pen2, point.X + 4, point.Y, point.X + 9 - 3, point.Y + 7 - 2);
						g.DrawLine(pen2, point.X, point.Y + 7 - 1, point.X + 9 - 2, point.Y + 7 - 1);
						return rectangle;
					}
					for (int i = 0; i < 4; i++)
					{
						g.DrawLine(pen, point.X + i, point.Y + 7 - i - 1, point.X + 9 - i - 1, point.Y + 7 - i - 1);
					}
					g.DrawLine(pen, point.X + 4, point.Y + 7 - 4 - 1, point.X + 4, point.Y + 7 - 4);
				}
				else
				{
					switch (advancedBorderStyle.Right)
					{
					case DataGridViewAdvancedCellBorderStyle.Inset:
						g.DrawLine(pen2, point.X, point.Y + 1, point.X + 4 - 1, point.Y + 7 - 1);
						g.DrawLine(pen2, point.X + 1, point.Y + 1, point.X + 4 - 1, point.Y + 7 - 1);
						g.DrawLine(pen, point.X + 4, point.Y + 7 - 1, point.X + 9 - 2, point.Y + 1);
						g.DrawLine(pen, point.X + 4, point.Y + 7 - 1, point.X + 9 - 3, point.Y + 1);
						g.DrawLine(pen, point.X, point.Y, point.X + 9 - 2, point.Y);
						return rectangle;
					case DataGridViewAdvancedCellBorderStyle.Outset:
					case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
					case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
						g.DrawLine(pen, point.X, point.Y + 1, point.X + 4 - 1, point.Y + 7 - 1);
						g.DrawLine(pen, point.X + 1, point.Y + 1, point.X + 4 - 1, point.Y + 7 - 1);
						g.DrawLine(pen2, point.X + 4, point.Y + 7 - 1, point.X + 9 - 2, point.Y + 1);
						g.DrawLine(pen2, point.X + 4, point.Y + 7 - 1, point.X + 9 - 3, point.Y + 1);
						g.DrawLine(pen2, point.X, point.Y, point.X + 9 - 2, point.Y);
						return rectangle;
					}
					for (int j = 0; j < 4; j++)
					{
						g.DrawLine(pen, point.X + j, point.Y + j + 2, point.X + 9 - j - 1, point.Y + j + 2);
					}
					g.DrawLine(pen, point.X + 4, point.Y + 4 + 1, point.X + 4, point.Y + 4 + 2);
				}
			}
			return rectangle;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000BBDA8 File Offset: 0x000BADA8
		protected override bool SetValue(int rowIndex, object value)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			object value2 = this.GetValue(rowIndex);
			base.Properties.SetObject(DataGridViewCell.PropCellValue, value);
			if (base.DataGridView != null && value2 != value)
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(base.ColumnIndex, -1));
			}
			return true;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000BBDFC File Offset: 0x000BADFC
		public override string ToString()
		{
			return "DataGridViewColumnHeaderCell { ColumnIndex=" + base.ColumnIndex.ToString(CultureInfo.CurrentCulture) + " }";
		}

		// Token: 0x04001B37 RID: 6967
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphSeparatorWidth = 2;

		// Token: 0x04001B38 RID: 6968
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphHorizontalMargin = 4;

		// Token: 0x04001B39 RID: 6969
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphWidth = 9;

		// Token: 0x04001B3A RID: 6970
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_sortGlyphHeight = 7;

		// Token: 0x04001B3B RID: 6971
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_horizontalTextMarginLeft = 2;

		// Token: 0x04001B3C RID: 6972
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04001B3D RID: 6973
		private const byte DATAGRIDVIEWCOLUMNHEADERCELL_verticalMargin = 1;

		// Token: 0x04001B3E RID: 6974
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x04001B3F RID: 6975
		private static Type cellType = typeof(DataGridViewColumnHeaderCell);

		// Token: 0x04001B40 RID: 6976
		private SortOrder sortGlyphDirection;

		// Token: 0x02000339 RID: 825
		private class DataGridViewColumnHeaderCellRenderer
		{
			// Token: 0x060034B5 RID: 13493 RVA: 0x000BBE46 File Offset: 0x000BAE46
			private DataGridViewColumnHeaderCellRenderer()
			{
			}

			// Token: 0x17000984 RID: 2436
			// (get) Token: 0x060034B6 RID: 13494 RVA: 0x000BBE4E File Offset: 0x000BAE4E
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewColumnHeaderCell.HeaderElement);
					}
					return DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x060034B7 RID: 13495 RVA: 0x000BBE6C File Offset: 0x000BAE6C
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				Rectangle rectangle = Rectangle.Truncate(g.ClipBounds);
				if (2 == headerState)
				{
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewColumnHeaderCell.HeaderElement);
					Rectangle rectangle2 = new Rectangle(bounds.Left, bounds.Bottom - 2, 2, 2);
					rectangle2.Intersect(rectangle);
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, rectangle2);
					rectangle2 = new Rectangle(bounds.Right - 2, bounds.Bottom - 2, 2, 2);
					rectangle2.Intersect(rectangle);
					DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, rectangle2);
				}
				DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewColumnHeaderCell.HeaderElement.ClassName, DataGridViewColumnHeaderCell.HeaderElement.Part, headerState);
				DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, rectangle);
			}

			// Token: 0x04001B41 RID: 6977
			private static VisualStyleRenderer visualStyleRenderer;
		}

		// Token: 0x0200033A RID: 826
		protected class DataGridViewColumnHeaderCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x060034B8 RID: 13496 RVA: 0x000BBF22 File Offset: 0x000BAF22
			public DataGridViewColumnHeaderCellAccessibleObject(DataGridViewColumnHeaderCell owner)
				: base(owner)
			{
			}

			// Token: 0x17000985 RID: 2437
			// (get) Token: 0x060034B9 RID: 13497 RVA: 0x000BBF2B File Offset: 0x000BAF2B
			public override Rectangle Bounds
			{
				get
				{
					return base.GetAccessibleObjectBounds(this.ParentPrivate);
				}
			}

			// Token: 0x17000986 RID: 2438
			// (get) Token: 0x060034BA RID: 13498 RVA: 0x000BBF3C File Offset: 0x000BAF3C
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.OwningColumn == null)
					{
						return string.Empty;
					}
					if (base.Owner.OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
					{
						return SR.GetString("DataGridView_AccColumnHeaderCellDefaultAction");
					}
					if (base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						return SR.GetString("DataGridView_AccColumnHeaderCellSelectDefaultAction");
					}
					return string.Empty;
				}
			}

			// Token: 0x17000987 RID: 2439
			// (get) Token: 0x060034BB RID: 13499 RVA: 0x000BBFB0 File Offset: 0x000BAFB0
			public override string Name
			{
				get
				{
					if (base.Owner.OwningColumn != null)
					{
						return base.Owner.OwningColumn.HeaderText;
					}
					return string.Empty;
				}
			}

			// Token: 0x17000988 RID: 2440
			// (get) Token: 0x060034BC RID: 13500 RVA: 0x000BBFD5 File Offset: 0x000BAFD5
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x17000989 RID: 2441
			// (get) Token: 0x060034BD RID: 13501 RVA: 0x000BBFDD File Offset: 0x000BAFDD
			private AccessibleObject ParentPrivate
			{
				get
				{
					return base.Owner.DataGridView.AccessibilityObject.GetChild(0);
				}
			}

			// Token: 0x1700098A RID: 2442
			// (get) Token: 0x060034BE RID: 13502 RVA: 0x000BBFF5 File Offset: 0x000BAFF5
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ColumnHeader;
				}
			}

			// Token: 0x1700098B RID: 2443
			// (get) Token: 0x060034BF RID: 13503 RVA: 0x000BBFFC File Offset: 0x000BAFFC
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
					if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect) && base.Owner.OwningColumn != null && base.Owner.OwningColumn.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x1700098C RID: 2444
			// (get) Token: 0x060034C0 RID: 13504 RVA: 0x000BC076 File Offset: 0x000BB076
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			// Token: 0x060034C1 RID: 13505 RVA: 0x000BC080 File Offset: 0x000BB080
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewColumnHeaderCell.DataGridView;
				if (dataGridViewColumnHeaderCell.OwningColumn != null)
				{
					if (dataGridViewColumnHeaderCell.OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
					{
						ListSortDirection listSortDirection = ListSortDirection.Ascending;
						if (dataGridView.SortedColumn == dataGridViewColumnHeaderCell.OwningColumn && dataGridView.SortOrder == SortOrder.Ascending)
						{
							listSortDirection = ListSortDirection.Descending;
						}
						dataGridView.Sort(dataGridViewColumnHeaderCell.OwningColumn, listSortDirection);
						return;
					}
					if (dataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = true;
					}
				}
			}

			// Token: 0x060034C2 RID: 13506 RVA: 0x000BC100 File Offset: 0x000BB100
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				if (base.Owner.OwningColumn == null)
				{
					return null;
				}
				switch (navigationDirection)
				{
				case AccessibleNavigation.Left:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateBackward();
					}
					return this.NavigateForward();
				case AccessibleNavigation.Right:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward();
					}
					return this.NavigateBackward();
				case AccessibleNavigation.Next:
					return this.NavigateForward();
				case AccessibleNavigation.Previous:
					return this.NavigateBackward();
				case AccessibleNavigation.FirstChild:
					return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(0);
				case AccessibleNavigation.LastChild:
				{
					AccessibleObject child = base.Owner.DataGridView.AccessibilityObject.GetChild(0);
					return child.GetChild(child.GetChildCount() - 1);
				}
				default:
					return null;
				}
			}

			// Token: 0x060034C3 RID: 13507 RVA: 0x000BC1D8 File Offset: 0x000BB1D8
			private AccessibleObject NavigateBackward()
			{
				if (base.Owner.OwningColumn == base.Owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible))
				{
					if (base.Owner.DataGridView.RowHeadersVisible)
					{
						return this.Parent.GetChild(0);
					}
					return null;
				}
				else
				{
					int index = base.Owner.DataGridView.Columns.GetPreviousColumn(base.Owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					int num = base.Owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(index, DataGridViewElementStates.Visible);
					if (base.Owner.DataGridView.RowHeadersVisible)
					{
						return this.Parent.GetChild(num + 1);
					}
					return this.Parent.GetChild(num);
				}
			}

			// Token: 0x060034C4 RID: 13508 RVA: 0x000BC29C File Offset: 0x000BB29C
			private AccessibleObject NavigateForward()
			{
				if (base.Owner.OwningColumn == base.Owner.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None))
				{
					return null;
				}
				int index = base.Owner.DataGridView.Columns.GetNextColumn(base.Owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
				int num = base.Owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(index, DataGridViewElementStates.Visible);
				if (base.Owner.DataGridView.RowHeadersVisible)
				{
					return this.Parent.GetChild(num + 1);
				}
				return this.Parent.GetChild(num);
			}

			// Token: 0x060034C5 RID: 13509 RVA: 0x000BC340 File Offset: 0x000BB340
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = (DataGridViewColumnHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewColumnHeaderCell.DataGridView;
				if (dataGridView == null)
				{
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					dataGridView.FocusInternal();
				}
				if (dataGridViewColumnHeaderCell.OwningColumn != null && (dataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect))
				{
					if ((flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) != AccessibleSelection.None)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = true;
						return;
					}
					if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection)
					{
						dataGridViewColumnHeaderCell.OwningColumn.Selected = false;
					}
				}
			}
		}
	}
}
