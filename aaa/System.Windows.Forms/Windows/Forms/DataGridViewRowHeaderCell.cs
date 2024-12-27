using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200038B RID: 907
	public class DataGridViewRowHeaderCell : DataGridViewHeaderCell
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x000CA54D File Offset: 0x000C954D
		private static Bitmap LeftArrowBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.leftArrowBmp == null)
				{
					DataGridViewRowHeaderCell.leftArrowBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.left.bmp");
				}
				return DataGridViewRowHeaderCell.leftArrowBmp;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x000CA56A File Offset: 0x000C956A
		private static Bitmap LeftArrowStarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.leftArrowStarBmp == null)
				{
					DataGridViewRowHeaderCell.leftArrowStarBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.leftstar.bmp");
				}
				return DataGridViewRowHeaderCell.leftArrowStarBmp;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000CA587 File Offset: 0x000C9587
		private static Bitmap PencilLTRBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.pencilLTRBmp == null)
				{
					DataGridViewRowHeaderCell.pencilLTRBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.pencil_ltr.bmp");
				}
				return DataGridViewRowHeaderCell.pencilLTRBmp;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003794 RID: 14228 RVA: 0x000CA5A4 File Offset: 0x000C95A4
		private static Bitmap PencilRTLBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.pencilRTLBmp == null)
				{
					DataGridViewRowHeaderCell.pencilRTLBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.pencil_rtl.bmp");
				}
				return DataGridViewRowHeaderCell.pencilRTLBmp;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x000CA5C1 File Offset: 0x000C95C1
		private static Bitmap RightArrowBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.rightArrowBmp == null)
				{
					DataGridViewRowHeaderCell.rightArrowBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.right.bmp");
				}
				return DataGridViewRowHeaderCell.rightArrowBmp;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x000CA5DE File Offset: 0x000C95DE
		private static Bitmap RightArrowStarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.rightArrowStarBmp == null)
				{
					DataGridViewRowHeaderCell.rightArrowStarBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.rightstar.bmp");
				}
				return DataGridViewRowHeaderCell.rightArrowStarBmp;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000CA5FB File Offset: 0x000C95FB
		private static Bitmap StarBitmap
		{
			get
			{
				if (DataGridViewRowHeaderCell.starBmp == null)
				{
					DataGridViewRowHeaderCell.starBmp = DataGridViewRowHeaderCell.GetBitmap("DataGridViewRow.star.bmp");
				}
				return DataGridViewRowHeaderCell.starBmp;
			}
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000CA618 File Offset: 0x000C9618
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewRowHeaderCell dataGridViewRowHeaderCell;
			if (type == DataGridViewRowHeaderCell.cellType)
			{
				dataGridViewRowHeaderCell = new DataGridViewRowHeaderCell();
			}
			else
			{
				dataGridViewRowHeaderCell = (DataGridViewRowHeaderCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewRowHeaderCell);
			dataGridViewRowHeaderCell.Value = base.Value;
			return dataGridViewRowHeaderCell;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000CA65C File Offset: 0x000C965C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewRowHeaderCell.DataGridViewRowHeaderCellAccessibleObject(this);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x000CA664 File Offset: 0x000C9664
		private static Bitmap GetArrowBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.RightArrowBitmap;
			}
			return DataGridViewRowHeaderCell.LeftArrowBitmap;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000CA674 File Offset: 0x000C9674
		private static Bitmap GetArrowStarBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.RightArrowStarBitmap;
			}
			return DataGridViewRowHeaderCell.LeftArrowStarBitmap;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000CA684 File Offset: 0x000C9684
		private static Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = new Bitmap(typeof(DataGridViewRowHeaderCell), bitmapName);
			bitmap.MakeTransparent();
			return bitmap;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x000CA6AC File Offset: 0x000C96AC
		protected override object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			object value = this.GetValue(rowIndex);
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (inFirstRow)
				{
					stringBuilder.Append("<TABLE>");
				}
				stringBuilder.Append("<TR>");
				stringBuilder.Append("<TD ALIGN=\"center\">");
				if (value != null)
				{
					stringBuilder.Append("<B>");
					DataGridViewCell.FormatPlainTextAsHtml(value.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
					stringBuilder.Append("</B>");
				}
				else
				{
					stringBuilder.Append("&nbsp;");
				}
				stringBuilder.Append("</TD>");
				if (lastCell)
				{
					stringBuilder.Append("</TR>");
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

		// Token: 0x0600379E RID: 14238 RVA: 0x000CA838 File Offset: 0x000C9838
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || base.OwningRow == null)
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

		// Token: 0x0600379F RID: 14239 RVA: 0x000CA898 File Offset: 0x000C9898
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || !base.DataGridView.ShowRowErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, formattedValue, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000CA922 File Offset: 0x000C9922
		protected internal override string GetErrorText(int rowIndex)
		{
			if (base.OwningRow == null)
			{
				return base.GetErrorText(rowIndex);
			}
			return base.OwningRow.GetErrorText(rowIndex);
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000CA940 File Offset: 0x000C9940
		public override ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
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

		// Token: 0x060037A2 RID: 14242 RVA: 0x000CA99C File Offset: 0x000C999C
		public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			DataGridViewCellStyle dataGridViewCellStyle = ((inheritedCellStyle == null) ? new DataGridViewCellStyle() : inheritedCellStyle);
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (base.HasStyle)
			{
				dataGridViewCellStyle2 = base.Style;
			}
			DataGridViewCellStyle rowHeadersDefaultCellStyle = base.DataGridView.RowHeadersDefaultCellStyle;
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (!rowHeadersDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = rowHeadersDefaultCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (!rowHeadersDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = rowHeadersDefaultCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (!rowHeadersDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = rowHeadersDefaultCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (!rowHeadersDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = rowHeadersDefaultCellStyle.SelectionForeColor;
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
			else if (rowHeadersDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = rowHeadersDefaultCellStyle.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = rowHeadersDefaultCellStyle.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = rowHeadersDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (rowHeadersDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = rowHeadersDefaultCellStyle.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (!rowHeadersDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = rowHeadersDefaultCellStyle.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (rowHeadersDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = rowHeadersDefaultCellStyle.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (rowHeadersDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = rowHeadersDefaultCellStyle.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (rowHeadersDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = rowHeadersDefaultCellStyle.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (rowHeadersDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = rowHeadersDefaultCellStyle.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000CAD4A File Offset: 0x000C9D4A
		private static Bitmap GetPencilBitmap(bool rightToLeft)
		{
			if (!rightToLeft)
			{
				return DataGridViewRowHeaderCell.PencilLTRBitmap;
			}
			return DataGridViewRowHeaderCell.PencilRTLBitmap;
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000CAD5C File Offset: 0x000C9D5C
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
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = base.OwningRow.AdjustRowHeaderBorderStyle(base.DataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStyle, false, false, false, false);
			Rectangle rectangle = this.BorderWidths(dataGridViewAdvancedBorderStyle2);
			int num = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int num2 = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			if (base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
				num += themeMargins.Y;
				num += themeMargins.Height;
				num2 += themeMargins.X;
				num2 += themeMargins.Width;
			}
			object obj = this.GetValue(rowIndex);
			if (!(obj is string))
			{
				obj = null;
			}
			return DataGridViewUtilities.GetPreferredRowHeaderSize(graphics, (string)obj, cellStyle, num, num2, base.DataGridView.ShowRowErrors, true, constraintSize, textFormatFlags);
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000CAE87 File Offset: 0x000C9E87
		protected override object GetValue(int rowIndex)
		{
			if (base.DataGridView != null && (rowIndex < -1 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			return base.Properties.GetObject(DataGridViewCell.PropCellValue);
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000CAEC4 File Offset: 0x000C9EC4
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000CAEFC File Offset: 0x000C9EFC
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle rectangle = Rectangle.Empty;
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
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
				if (rectangle4.Width > 0 && rectangle4.Height > 0)
				{
					if (paint && DataGridViewCell.PaintBackground(paintParts))
					{
						int num = 1;
						if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
						{
							if (base.ButtonState != ButtonState.Normal)
							{
								num = 3;
							}
							else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == -1)
							{
								num = 2;
							}
							else if (flag)
							{
								num = 3;
							}
						}
						using (Bitmap bitmap = new Bitmap(rectangle4.Height, rectangle4.Width))
						{
							using (Graphics graphics2 = Graphics.FromImage(bitmap))
							{
								DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.DrawHeader(graphics2, new Rectangle(0, 0, rectangle4.Height, rectangle4.Width), num);
								bitmap.RotateFlip(base.DataGridView.RightToLeftInternal ? RotateFlipType.Rotate90FlipNone : RotateFlipType.Rotate90FlipX);
								graphics.DrawImage(bitmap, rectangle4, new Rectangle(0, 0, rectangle4.Width, rectangle4.Height), GraphicsUnit.Pixel);
							}
						}
					}
					Rectangle themeMargins = DataGridViewHeaderCell.GetThemeMargins(graphics);
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle2.X += themeMargins.Height;
					}
					else
					{
						rectangle2.X += themeMargins.Y;
					}
					rectangle2.Width -= themeMargins.Y + themeMargins.Height;
					rectangle2.Height -= themeMargins.X + themeMargins.Width;
					rectangle2.Y += themeMargins.X;
				}
			}
			else
			{
				if (rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
					{
						graphics.FillRectangle(cachedBrush, rectangle2);
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
			Bitmap bitmap2 = null;
			if (rectangle2.Width > 0 && rectangle2.Height > 0)
			{
				Rectangle rectangle5 = rectangle2;
				string text = formattedValue as string;
				if (!string.IsNullOrEmpty(text))
				{
					if (rectangle2.Width >= 18 && rectangle2.Height >= 15)
					{
						if (paint && DataGridViewCell.PaintContentBackground(paintParts))
						{
							if (base.DataGridView.CurrentCellAddress.Y == rowIndex)
							{
								if (base.DataGridView.VirtualMode)
								{
									if (base.DataGridView.IsCurrentRowDirty && base.DataGridView.ShowEditingIcon)
									{
										bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
									}
									else if (base.DataGridView.NewRowIndex == rowIndex)
									{
										bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
									}
									else
									{
										bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
									}
								}
								else if (base.DataGridView.IsCurrentCellDirty && base.DataGridView.ShowEditingIcon)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
								}
								else if (base.DataGridView.NewRowIndex == rowIndex)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
								}
								else
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
								}
							}
							else if (base.DataGridView.NewRowIndex == rowIndex)
							{
								bitmap2 = DataGridViewRowHeaderCell.StarBitmap;
							}
							if (bitmap2 != null)
							{
								Color color;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									color = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									color = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								lock (bitmap2)
								{
									this.PaintIcon(graphics, bitmap2, rectangle2, color);
								}
							}
						}
						if (!base.DataGridView.RightToLeftInternal)
						{
							rectangle2.X += 18;
						}
						rectangle2.Width -= 18;
					}
					rectangle2.Offset(4, 1);
					rectangle2.Width -= 9;
					rectangle2.Height -= 2;
					if (rectangle2.Width > 0 && rectangle2.Height > 0)
					{
						TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						if (base.DataGridView.ShowRowErrors && rectangle2.Width > 18)
						{
							Size size = new Size(rectangle2.Width - 12 - 6, rectangle2.Height);
							if (DataGridViewCell.TextFitsInBounds(graphics, text, cellStyle.Font, size, textFormatFlags))
							{
								if (base.DataGridView.RightToLeftInternal)
								{
									rectangle2.X += 18;
								}
								rectangle2.Width -= 18;
							}
						}
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if (paint)
							{
								Color color2;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									color2 = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									color2 = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
								{
									textFormatFlags |= TextFormatFlags.EndEllipsis;
								}
								TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle2, color2, textFormatFlags);
							}
							else if (computeContentBounds)
							{
								rectangle = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle);
							}
						}
					}
					if (rectangle5.Width >= 33)
					{
						if (paint && base.DataGridView.ShowRowErrors && DataGridViewCell.PaintErrorIcon(paintParts))
						{
							this.PaintErrorIcon(graphics, clipBounds, rectangle5, errorText);
						}
						else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
						{
							rectangle = base.ComputeErrorIconBounds(rectangle5);
						}
					}
				}
				else
				{
					if (rectangle2.Width >= 18 && rectangle2.Height >= 15 && paint && DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (base.DataGridView.CurrentCellAddress.Y == rowIndex)
						{
							if (base.DataGridView.VirtualMode)
							{
								if (base.DataGridView.IsCurrentRowDirty && base.DataGridView.ShowEditingIcon)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
								}
								else if (base.DataGridView.NewRowIndex == rowIndex)
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
								}
								else
								{
									bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
								}
							}
							else if (base.DataGridView.IsCurrentCellDirty && base.DataGridView.ShowEditingIcon)
							{
								bitmap2 = DataGridViewRowHeaderCell.GetPencilBitmap(base.DataGridView.RightToLeftInternal);
							}
							else if (base.DataGridView.NewRowIndex == rowIndex)
							{
								bitmap2 = DataGridViewRowHeaderCell.GetArrowStarBitmap(base.DataGridView.RightToLeftInternal);
							}
							else
							{
								bitmap2 = DataGridViewRowHeaderCell.GetArrowBitmap(base.DataGridView.RightToLeftInternal);
							}
						}
						else if (base.DataGridView.NewRowIndex == rowIndex)
						{
							bitmap2 = DataGridViewRowHeaderCell.StarBitmap;
						}
						if (bitmap2 != null)
						{
							lock (bitmap2)
							{
								Color color3;
								if (base.DataGridView.ApplyVisualStylesToHeaderCells)
								{
									color3 = DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									color3 = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								this.PaintIcon(graphics, bitmap2, rectangle2, color3);
							}
						}
					}
					if (rectangle5.Width >= 33)
					{
						if (paint && base.DataGridView.ShowRowErrors && DataGridViewCell.PaintErrorIcon(paintParts))
						{
							base.PaintErrorIcon(graphics, cellStyle, rowIndex, cellBounds, rectangle5, errorText);
						}
						else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
						{
							rectangle = base.ComputeErrorIconBounds(rectangle5);
						}
					}
				}
			}
			return rectangle;
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000CB8D0 File Offset: 0x000CA8D0
		private void PaintIcon(Graphics g, Bitmap bmp, Rectangle bounds, Color foreColor)
		{
			Rectangle rectangle = new Rectangle(base.DataGridView.RightToLeftInternal ? (bounds.Right - 3 - 12) : (bounds.Left + 3), bounds.Y + (bounds.Height - 11) / 2, 12, 11);
			DataGridViewRowHeaderCell.colorMap[0].NewColor = foreColor;
			DataGridViewRowHeaderCell.colorMap[0].OldColor = Color.Black;
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetRemapTable(DataGridViewRowHeaderCell.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(bmp, rectangle, 0, 0, 12, 11, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000CB968 File Offset: 0x000CA968
		protected override bool SetValue(int rowIndex, object value)
		{
			object value2 = this.GetValue(rowIndex);
			if (value != null || base.Properties.ContainsObject(DataGridViewCell.PropCellValue))
			{
				base.Properties.SetObject(DataGridViewCell.PropCellValue, value);
			}
			if (base.DataGridView != null && value2 != value)
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(-1, rowIndex));
			}
			return true;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000CB9C0 File Offset: 0x000CA9C0
		public override string ToString()
		{
			return "DataGridViewRowHeaderCell { RowIndex=" + base.RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
		}

		// Token: 0x04001C1D RID: 7197
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginWidth = 3;

		// Token: 0x04001C1E RID: 7198
		private const byte DATAGRIDVIEWROWHEADERCELL_iconMarginHeight = 2;

		// Token: 0x04001C1F RID: 7199
		private const byte DATAGRIDVIEWROWHEADERCELL_contentMarginWidth = 3;

		// Token: 0x04001C20 RID: 7200
		private const byte DATAGRIDVIEWROWHEADERCELL_contentMarginHeight = 3;

		// Token: 0x04001C21 RID: 7201
		private const byte DATAGRIDVIEWROWHEADERCELL_iconsWidth = 12;

		// Token: 0x04001C22 RID: 7202
		private const byte DATAGRIDVIEWROWHEADERCELL_iconsHeight = 11;

		// Token: 0x04001C23 RID: 7203
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04001C24 RID: 7204
		private const byte DATAGRIDVIEWROWHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04001C25 RID: 7205
		private const byte DATAGRIDVIEWROWHEADERCELL_verticalTextMargin = 1;

		// Token: 0x04001C26 RID: 7206
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x04001C27 RID: 7207
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04001C28 RID: 7208
		private static Bitmap rightArrowBmp = null;

		// Token: 0x04001C29 RID: 7209
		private static Bitmap leftArrowBmp = null;

		// Token: 0x04001C2A RID: 7210
		private static Bitmap rightArrowStarBmp;

		// Token: 0x04001C2B RID: 7211
		private static Bitmap leftArrowStarBmp;

		// Token: 0x04001C2C RID: 7212
		private static Bitmap pencilLTRBmp = null;

		// Token: 0x04001C2D RID: 7213
		private static Bitmap pencilRTLBmp = null;

		// Token: 0x04001C2E RID: 7214
		private static Bitmap starBmp = null;

		// Token: 0x04001C2F RID: 7215
		private static Type cellType = typeof(DataGridViewRowHeaderCell);

		// Token: 0x0200038C RID: 908
		private class DataGridViewRowHeaderCellRenderer
		{
			// Token: 0x060037AC RID: 14252 RVA: 0x000CBA49 File Offset: 0x000CAA49
			private DataGridViewRowHeaderCellRenderer()
			{
			}

			// Token: 0x17000A48 RID: 2632
			// (get) Token: 0x060037AD RID: 14253 RVA: 0x000CBA51 File Offset: 0x000CAA51
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewRowHeaderCell.HeaderElement);
					}
					return DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x060037AE RID: 14254 RVA: 0x000CBA6E File Offset: 0x000CAA6E
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewRowHeaderCell.HeaderElement.ClassName, DataGridViewRowHeaderCell.HeaderElement.Part, headerState);
				DataGridViewRowHeaderCell.DataGridViewRowHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04001C30 RID: 7216
			private static VisualStyleRenderer visualStyleRenderer;
		}

		// Token: 0x0200038D RID: 909
		protected class DataGridViewRowHeaderCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x060037AF RID: 14255 RVA: 0x000CBAA6 File Offset: 0x000CAAA6
			public DataGridViewRowHeaderCellAccessibleObject(DataGridViewRowHeaderCell owner)
				: base(owner)
			{
			}

			// Token: 0x17000A49 RID: 2633
			// (get) Token: 0x060037B0 RID: 14256 RVA: 0x000CBAB0 File Offset: 0x000CAAB0
			public override Rectangle Bounds
			{
				get
				{
					if (base.Owner.OwningRow == null)
					{
						return Rectangle.Empty;
					}
					Rectangle bounds = this.ParentPrivate.Bounds;
					bounds.Width = base.Owner.DataGridView.RowHeadersWidth;
					return bounds;
				}
			}

			// Token: 0x17000A4A RID: 2634
			// (get) Token: 0x060037B1 RID: 14257 RVA: 0x000CBAF4 File Offset: 0x000CAAF4
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
					{
						return SR.GetString("DataGridView_RowHeaderCellAccDefaultAction");
					}
					return string.Empty;
				}
			}

			// Token: 0x17000A4B RID: 2635
			// (get) Token: 0x060037B2 RID: 14258 RVA: 0x000CBB2C File Offset: 0x000CAB2C
			public override string Name
			{
				get
				{
					if (this.ParentPrivate != null)
					{
						return this.ParentPrivate.Name;
					}
					return string.Empty;
				}
			}

			// Token: 0x17000A4C RID: 2636
			// (get) Token: 0x060037B3 RID: 14259 RVA: 0x000CBB47 File Offset: 0x000CAB47
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x17000A4D RID: 2637
			// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000CBB4F File Offset: 0x000CAB4F
			private AccessibleObject ParentPrivate
			{
				get
				{
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					return base.Owner.OwningRow.AccessibilityObject;
				}
			}

			// Token: 0x17000A4E RID: 2638
			// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000CBB70 File Offset: 0x000CAB70
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.RowHeader;
				}
			}

			// Token: 0x17000A4F RID: 2639
			// (get) Token: 0x060037B6 RID: 14262 RVA: 0x000CBB74 File Offset: 0x000CAB74
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
					if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect) && base.Owner.OwningRow != null && base.Owner.OwningRow.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000A50 RID: 2640
			// (get) Token: 0x060037B7 RID: 14263 RVA: 0x000CBBEE File Offset: 0x000CABEE
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x060037B8 RID: 14264 RVA: 0x000CBBF8 File Offset: 0x000CABF8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if ((base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.Owner.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect) && base.Owner.OwningRow != null)
				{
					base.Owner.OwningRow.Selected = true;
				}
			}

			// Token: 0x060037B9 RID: 14265 RVA: 0x000CBC4C File Offset: 0x000CAC4C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				switch (navigationDirection)
				{
				case AccessibleNavigation.Up:
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					if (base.Owner.OwningRow.Index == base.Owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible))
					{
						if (base.Owner.DataGridView.ColumnHeadersVisible)
						{
							return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(0);
						}
						return null;
					}
					else
					{
						int previousRow = base.Owner.DataGridView.Rows.GetPreviousRow(base.Owner.OwningRow.Index, DataGridViewElementStates.Visible);
						int rowCount = base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, previousRow);
						if (base.Owner.DataGridView.ColumnHeadersVisible)
						{
							return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount + 1).GetChild(0);
						}
						return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount).GetChild(0);
					}
					break;
				case AccessibleNavigation.Down:
				{
					if (base.Owner.OwningRow == null)
					{
						return null;
					}
					if (base.Owner.OwningRow.Index == base.Owner.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible))
					{
						return null;
					}
					int nextRow = base.Owner.DataGridView.Rows.GetNextRow(base.Owner.OwningRow.Index, DataGridViewElementStates.Visible);
					int rowCount2 = base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, nextRow);
					if (base.Owner.DataGridView.ColumnHeadersVisible)
					{
						return base.Owner.DataGridView.AccessibilityObject.GetChild(1 + rowCount2).GetChild(0);
					}
					return base.Owner.DataGridView.AccessibilityObject.GetChild(rowCount2).GetChild(0);
				}
				case AccessibleNavigation.Next:
					if (base.Owner.OwningRow != null && base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
					{
						return this.ParentPrivate.GetChild(1);
					}
					return null;
				case AccessibleNavigation.Previous:
					return null;
				}
				return null;
			}

			// Token: 0x060037BA RID: 14266 RVA: 0x000CBE80 File Offset: 0x000CAE80
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewRowHeaderCell dataGridViewRowHeaderCell = (DataGridViewRowHeaderCell)base.Owner;
				DataGridView dataGridView = dataGridViewRowHeaderCell.DataGridView;
				if (dataGridView == null)
				{
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					dataGridView.FocusInternal();
				}
				if (dataGridViewRowHeaderCell.OwningRow != null && (dataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect))
				{
					if ((flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) != AccessibleSelection.None)
					{
						dataGridViewRowHeaderCell.OwningRow.Selected = true;
						return;
					}
					if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection)
					{
						dataGridViewRowHeaderCell.OwningRow.Selected = false;
					}
				}
			}
		}
	}
}
