using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000372 RID: 882
	public class DataGridViewImageCell : DataGridViewCell
	{
		// Token: 0x06003618 RID: 13848 RVA: 0x000C0B7F File Offset: 0x000BFB7F
		public DataGridViewImageCell()
			: this(false)
		{
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000C0B88 File Offset: 0x000BFB88
		public DataGridViewImageCell(bool valueIsIcon)
		{
			if (valueIsIcon)
			{
				this.flags = 1;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x000C0B9A File Offset: 0x000BFB9A
		public override object DefaultNewRowValue
		{
			get
			{
				if (DataGridViewImageCell.defaultTypeImage.IsAssignableFrom(this.ValueType))
				{
					return DataGridViewImageCell.ErrorBitmap;
				}
				if (DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(this.ValueType))
				{
					return DataGridViewImageCell.ErrorIcon;
				}
				return null;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000C0BD0 File Offset: 0x000BFBD0
		// (set) Token: 0x0600361C RID: 13852 RVA: 0x000C0BFD File Offset: 0x000BFBFD
		[DefaultValue("")]
		public string Description
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewImageCell.PropImageCellDescription);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) || base.Properties.ContainsObject(DataGridViewImageCell.PropImageCellDescription))
				{
					base.Properties.SetObject(DataGridViewImageCell.PropImageCellDescription, value);
				}
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x000C0C2A File Offset: 0x000BFC2A
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600361E RID: 13854 RVA: 0x000C0C2D File Offset: 0x000BFC2D
		internal static Bitmap ErrorBitmap
		{
			get
			{
				if (DataGridViewImageCell.errorBmp == null)
				{
					DataGridViewImageCell.errorBmp = new Bitmap(typeof(DataGridView), "ImageInError.bmp");
				}
				return DataGridViewImageCell.errorBmp;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600361F RID: 13855 RVA: 0x000C0C54 File Offset: 0x000BFC54
		internal static Icon ErrorIcon
		{
			get
			{
				if (DataGridViewImageCell.errorIco == null)
				{
					DataGridViewImageCell.errorIco = new Icon(typeof(DataGridView), "IconInError.ico");
				}
				return DataGridViewImageCell.errorIco;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003620 RID: 13856 RVA: 0x000C0C7B File Offset: 0x000BFC7B
		public override Type FormattedValueType
		{
			get
			{
				if (this.ValueIsIcon)
				{
					return DataGridViewImageCell.defaultTypeIcon;
				}
				return DataGridViewImageCell.defaultTypeImage;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x000C0C90 File Offset: 0x000BFC90
		// (set) Token: 0x06003622 RID: 13858 RVA: 0x000C0CB8 File Offset: 0x000BFCB8
		[DefaultValue(DataGridViewImageCellLayout.NotSet)]
		public DataGridViewImageCellLayout ImageLayout
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewImageCell.PropImageCellLayout, out flag);
				if (flag)
				{
					return (DataGridViewImageCellLayout)integer;
				}
				return DataGridViewImageCellLayout.Normal;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewImageCellLayout));
				}
				if (this.ImageLayout != value)
				{
					base.Properties.SetInteger(DataGridViewImageCell.PropImageCellLayout, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170009DB RID: 2523
		// (set) Token: 0x06003623 RID: 13859 RVA: 0x000C0D0B File Offset: 0x000BFD0B
		internal DataGridViewImageCellLayout ImageLayoutInternal
		{
			set
			{
				if (this.ImageLayout != value)
				{
					base.Properties.SetInteger(DataGridViewImageCell.PropImageCellLayout, (int)value);
				}
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x000C0D27 File Offset: 0x000BFD27
		// (set) Token: 0x06003625 RID: 13861 RVA: 0x000C0D38 File Offset: 0x000BFD38
		[DefaultValue(false)]
		public bool ValueIsIcon
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (this.ValueIsIcon != value)
				{
					this.ValueIsIconInternal = value;
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x170009DD RID: 2525
		// (set) Token: 0x06003626 RID: 13862 RVA: 0x000C0D84 File Offset: 0x000BFD84
		internal bool ValueIsIconInternal
		{
			set
			{
				if (this.ValueIsIcon != value)
				{
					if (value)
					{
						this.flags |= 1;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -2);
					}
					if (base.DataGridView != null && base.RowIndex != -1 && base.DataGridView.NewRowIndex == base.RowIndex && !base.DataGridView.VirtualMode && ((value && base.Value == DataGridViewImageCell.ErrorBitmap) || (!value && base.Value == DataGridViewImageCell.ErrorIcon)))
					{
						base.Value = this.DefaultNewRowValue;
					}
				}
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003627 RID: 13863 RVA: 0x000C0E20 File Offset: 0x000BFE20
		// (set) Token: 0x06003628 RID: 13864 RVA: 0x000C0E4C File Offset: 0x000BFE4C
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				if (this.ValueIsIcon)
				{
					return DataGridViewImageCell.defaultTypeIcon;
				}
				return DataGridViewImageCell.defaultTypeImage;
			}
			set
			{
				base.ValueType = value;
				this.ValueIsIcon = value != null && DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(value);
			}
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000C0E6C File Offset: 0x000BFE6C
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewImageCell dataGridViewImageCell;
			if (type == DataGridViewImageCell.cellType)
			{
				dataGridViewImageCell = new DataGridViewImageCell();
			}
			else
			{
				dataGridViewImageCell = (DataGridViewImageCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewImageCell);
			dataGridViewImageCell.ValueIsIconInternal = this.ValueIsIcon;
			dataGridViewImageCell.Description = this.Description;
			dataGridViewImageCell.ImageLayoutInternal = this.ImageLayout;
			return dataGridViewImageCell;
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000C0EC8 File Offset: 0x000BFEC8
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewImageCell.DataGridViewImageCellAccessibleObject(this);
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000C0ED0 File Offset: 0x000BFED0
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

		// Token: 0x0600362C RID: 13868 RVA: 0x000C0F44 File Offset: 0x000BFF44
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
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, formattedValue, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000C0FD8 File Offset: 0x000BFFD8
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
			{
				return this.Description;
			}
			object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			if (formattedValue == null && cellStyle.NullValue == null)
			{
				return null;
			}
			if (this.ValueIsIcon)
			{
				Icon icon = formattedValue as Icon;
				if (icon == null)
				{
					icon = DataGridViewImageCell.ErrorIcon;
				}
				return icon;
			}
			Image image = formattedValue as Image;
			if (image == null)
			{
				image = DataGridViewImageCell.ErrorBitmap;
			}
			return image;
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000C1040 File Offset: 0x000C0040
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
			Image image = formattedValue as Image;
			Icon icon = null;
			if (image == null)
			{
				icon = formattedValue as Icon;
			}
			Size size;
			if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height && this.ImageLayout == DataGridViewImageCellLayout.Zoom)
			{
				if (image != null || icon != null)
				{
					if (image != null)
					{
						int num3 = constraintSize.Width - num;
						if (num3 <= 0 || image.Width == 0)
						{
							size = Size.Empty;
						}
						else
						{
							size = new Size(0, Math.Min(image.Height, decimal.ToInt32(image.Height * num3 / image.Width)));
						}
					}
					else
					{
						int num4 = constraintSize.Width - num;
						if (num4 <= 0 || icon.Width == 0)
						{
							size = Size.Empty;
						}
						else
						{
							size = new Size(0, Math.Min(icon.Height, decimal.ToInt32(icon.Height * num4 / icon.Width)));
						}
					}
				}
				else
				{
					size = new Size(0, 1);
				}
			}
			else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width && this.ImageLayout == DataGridViewImageCellLayout.Zoom)
			{
				if (image != null || icon != null)
				{
					if (image != null)
					{
						int num5 = constraintSize.Height - num2;
						if (num5 <= 0 || image.Height == 0)
						{
							size = Size.Empty;
						}
						else
						{
							size = new Size(Math.Min(image.Width, decimal.ToInt32(image.Width * num5 / image.Height)), 0);
						}
					}
					else
					{
						int num6 = constraintSize.Height - num2;
						if (num6 <= 0 || icon.Height == 0)
						{
							size = Size.Empty;
						}
						else
						{
							size = new Size(Math.Min(icon.Width, decimal.ToInt32(icon.Width * num6 / icon.Height)), 0);
						}
					}
				}
				else
				{
					size = new Size(1, 0);
				}
			}
			else
			{
				if (image != null)
				{
					size = new Size(image.Width, image.Height);
				}
				else if (icon != null)
				{
					size = new Size(icon.Width, icon.Height);
				}
				else
				{
					size = new Size(1, 1);
				}
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height)
				{
					size.Width = 0;
				}
				else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					size.Height = 0;
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
				size.Height += num2;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000C13B8 File Offset: 0x000C03B8
		protected override object GetValue(int rowIndex)
		{
			object value = base.GetValue(rowIndex);
			if (value == null)
			{
				DataGridViewImageColumn dataGridViewImageColumn = base.OwningColumn as DataGridViewImageColumn;
				if (dataGridViewImageColumn != null)
				{
					if (DataGridViewImageCell.defaultTypeImage.IsAssignableFrom(this.ValueType))
					{
						Image image = dataGridViewImageColumn.Image;
						if (image != null)
						{
							return image;
						}
					}
					else if (DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(this.ValueType))
					{
						Icon icon = dataGridViewImageColumn.Icon;
						if (icon != null)
						{
							return icon;
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000C141C File Offset: 0x000C041C
		private Rectangle ImgBounds(Rectangle bounds, int imgWidth, int imgHeight, DataGridViewImageCellLayout imageLayout, DataGridViewCellStyle cellStyle)
		{
			Rectangle empty = Rectangle.Empty;
			switch (imageLayout)
			{
			case DataGridViewImageCellLayout.NotSet:
			case DataGridViewImageCellLayout.Normal:
				empty = new Rectangle(bounds.X, bounds.Y, imgWidth, imgHeight);
				break;
			case DataGridViewImageCellLayout.Zoom:
				if (imgWidth * bounds.Height < imgHeight * bounds.Width)
				{
					empty = new Rectangle(bounds.X, bounds.Y, decimal.ToInt32(imgWidth * bounds.Height / imgHeight), bounds.Height);
				}
				else
				{
					empty = new Rectangle(bounds.X, bounds.Y, bounds.Width, decimal.ToInt32(imgHeight * bounds.Width / imgWidth));
				}
				break;
			}
			if (base.DataGridView.RightToLeftInternal)
			{
				DataGridViewContentAlignment alignment = cellStyle.Alignment;
				if (alignment <= DataGridViewContentAlignment.MiddleLeft)
				{
					if (alignment != DataGridViewContentAlignment.TopLeft)
					{
						if (alignment != DataGridViewContentAlignment.TopRight)
						{
							if (alignment == DataGridViewContentAlignment.MiddleLeft)
							{
								empty.X = bounds.Right - empty.Width;
							}
						}
						else
						{
							empty.X = bounds.X;
						}
					}
					else
					{
						empty.X = bounds.Right - empty.Width;
					}
				}
				else if (alignment != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment == DataGridViewContentAlignment.BottomRight)
						{
							empty.X = bounds.X;
						}
					}
					else
					{
						empty.X = bounds.Right - empty.Width;
					}
				}
				else
				{
					empty.X = bounds.X;
				}
			}
			else
			{
				DataGridViewContentAlignment alignment2 = cellStyle.Alignment;
				if (alignment2 <= DataGridViewContentAlignment.MiddleLeft)
				{
					if (alignment2 != DataGridViewContentAlignment.TopLeft)
					{
						if (alignment2 != DataGridViewContentAlignment.TopRight)
						{
							if (alignment2 == DataGridViewContentAlignment.MiddleLeft)
							{
								empty.X = bounds.X;
							}
						}
						else
						{
							empty.X = bounds.Right - empty.Width;
						}
					}
					else
					{
						empty.X = bounds.X;
					}
				}
				else if (alignment2 != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment2 != DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment2 == DataGridViewContentAlignment.BottomRight)
						{
							empty.X = bounds.Right - empty.Width;
						}
					}
					else
					{
						empty.X = bounds.X;
					}
				}
				else
				{
					empty.X = bounds.Right - empty.Width;
				}
			}
			DataGridViewContentAlignment alignment3 = cellStyle.Alignment;
			if (alignment3 == DataGridViewContentAlignment.TopCenter || alignment3 == DataGridViewContentAlignment.MiddleCenter || alignment3 == DataGridViewContentAlignment.BottomCenter)
			{
				empty.X = bounds.X + (bounds.Width - empty.Width) / 2;
			}
			DataGridViewContentAlignment alignment4 = cellStyle.Alignment;
			if (alignment4 > DataGridViewContentAlignment.MiddleCenter)
			{
				if (alignment4 <= DataGridViewContentAlignment.BottomLeft)
				{
					if (alignment4 == DataGridViewContentAlignment.MiddleRight)
					{
						goto IL_030C;
					}
					if (alignment4 != DataGridViewContentAlignment.BottomLeft)
					{
						return empty;
					}
				}
				else if (alignment4 != DataGridViewContentAlignment.BottomCenter && alignment4 != DataGridViewContentAlignment.BottomRight)
				{
					return empty;
				}
				empty.Y = bounds.Bottom - empty.Height;
				return empty;
			}
			switch (alignment4)
			{
			case DataGridViewContentAlignment.TopLeft:
			case DataGridViewContentAlignment.TopCenter:
			case DataGridViewContentAlignment.TopRight:
				empty.Y = bounds.Y;
				return empty;
			case (DataGridViewContentAlignment)3:
				return empty;
			default:
				if (alignment4 != DataGridViewContentAlignment.MiddleLeft && alignment4 != DataGridViewContentAlignment.MiddleCenter)
				{
					return empty;
				}
				break;
			}
			IL_030C:
			empty.Y = bounds.Y + (bounds.Height - empty.Height) / 2;
			return empty;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000C1770 File Offset: 0x000C0770
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000C17A8 File Offset: 0x000C07A8
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
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
			if (rectangle.Width > 0 && rectangle.Height > 0 && (paint || computeContentBounds))
			{
				Rectangle rectangle3 = rectangle;
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
				bool flag = (elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
				SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
				if (rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					Image image = formattedValue as Image;
					Icon icon = null;
					if (image == null)
					{
						icon = formattedValue as Icon;
					}
					if (icon != null || image != null)
					{
						DataGridViewImageCellLayout dataGridViewImageCellLayout = this.ImageLayout;
						if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.NotSet)
						{
							if (base.OwningColumn is DataGridViewImageColumn)
							{
								dataGridViewImageCellLayout = ((DataGridViewImageColumn)base.OwningColumn).ImageLayout;
							}
							else
							{
								dataGridViewImageCellLayout = DataGridViewImageCellLayout.Normal;
							}
						}
						if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.Stretch)
						{
							if (paint)
							{
								if (DataGridViewCell.PaintBackground(paintParts))
								{
									DataGridViewCell.PaintPadding(g, rectangle, cellStyle, cachedBrush, base.DataGridView.RightToLeftInternal);
								}
								if (DataGridViewCell.PaintContentForeground(paintParts))
								{
									if (image != null)
									{
										ImageAttributes imageAttributes = new ImageAttributes();
										imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
										g.DrawImage(image, rectangle3, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
										imageAttributes.Dispose();
									}
									else
									{
										g.DrawIcon(icon, rectangle3);
									}
								}
							}
							rectangle4 = rectangle3;
						}
						else
						{
							Rectangle rectangle5 = this.ImgBounds(rectangle3, (image == null) ? icon.Width : image.Width, (image == null) ? icon.Height : image.Height, dataGridViewImageCellLayout, cellStyle);
							rectangle4 = rectangle5;
							if (paint)
							{
								if (DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
								{
									g.FillRectangle(cachedBrush, rectangle);
								}
								if (DataGridViewCell.PaintContentForeground(paintParts))
								{
									Region clip = g.Clip;
									g.SetClip(Rectangle.Intersect(Rectangle.Intersect(rectangle5, rectangle3), Rectangle.Truncate(g.VisibleClipBounds)));
									if (image != null)
									{
										g.DrawImage(image, rectangle5);
									}
									else
									{
										g.DrawIconUnstretched(icon, rectangle5);
									}
									g.Clip = clip;
								}
							}
						}
					}
					else
					{
						if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
						{
							g.FillRectangle(cachedBrush, rectangle);
						}
						rectangle4 = Rectangle.Empty;
					}
				}
				else
				{
					if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
					{
						g.FillRectangle(cachedBrush, rectangle);
					}
					rectangle4 = Rectangle.Empty;
				}
				Point currentCellAddress = base.DataGridView.CurrentCellAddress;
				if (paint && DataGridViewCell.PaintFocus(paintParts) && currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.ShowFocusCues && base.DataGridView.Focused)
				{
					ControlPaint.DrawFocusRectangle(g, rectangle, Color.Empty, cachedBrush.Color);
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle, errorText);
				}
			}
			else if (computeErrorIconBounds)
			{
				if (!string.IsNullOrEmpty(errorText))
				{
					rectangle4 = base.ComputeErrorIconBounds(rectangle);
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
			return rectangle4;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000C1BE8 File Offset: 0x000C0BE8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewImageCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04001BBE RID: 7102
		private const byte DATAGRIDVIEWIMAGECELL_valueIsIcon = 1;

		// Token: 0x04001BBF RID: 7103
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04001BC0 RID: 7104
		private static readonly int PropImageCellDescription = PropertyStore.CreateKey();

		// Token: 0x04001BC1 RID: 7105
		private static readonly int PropImageCellLayout = PropertyStore.CreateKey();

		// Token: 0x04001BC2 RID: 7106
		private static Type defaultTypeImage = typeof(Image);

		// Token: 0x04001BC3 RID: 7107
		private static Type defaultTypeIcon = typeof(Icon);

		// Token: 0x04001BC4 RID: 7108
		private static Type cellType = typeof(DataGridViewImageCell);

		// Token: 0x04001BC5 RID: 7109
		private static Bitmap errorBmp = null;

		// Token: 0x04001BC6 RID: 7110
		private static Icon errorIco = null;

		// Token: 0x04001BC7 RID: 7111
		private byte flags;

		// Token: 0x02000373 RID: 883
		protected class DataGridViewImageCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x06003635 RID: 13877 RVA: 0x000C1CB7 File Offset: 0x000C0CB7
			public DataGridViewImageCellAccessibleObject(DataGridViewCell owner)
				: base(owner)
			{
			}

			// Token: 0x170009DF RID: 2527
			// (get) Token: 0x06003636 RID: 13878 RVA: 0x000C1CC0 File Offset: 0x000C0CC0
			public override string DefaultAction
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x170009E0 RID: 2528
			// (get) Token: 0x06003637 RID: 13879 RVA: 0x000C1CC8 File Offset: 0x000C0CC8
			public override string Description
			{
				get
				{
					DataGridViewImageCell dataGridViewImageCell = base.Owner as DataGridViewImageCell;
					if (dataGridViewImageCell != null)
					{
						return dataGridViewImageCell.Description;
					}
					return null;
				}
			}

			// Token: 0x170009E1 RID: 2529
			// (get) Token: 0x06003638 RID: 13880 RVA: 0x000C1CEC File Offset: 0x000C0CEC
			// (set) Token: 0x06003639 RID: 13881 RVA: 0x000C1CF4 File Offset: 0x000C0CF4
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return base.Value;
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
				}
			}

			// Token: 0x0600363A RID: 13882 RVA: 0x000C1CF6 File Offset: 0x000C0CF6
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
			}

			// Token: 0x0600363B RID: 13883 RVA: 0x000C1CF8 File Offset: 0x000C0CF8
			public override int GetChildCount()
			{
				return 0;
			}
		}
	}
}
