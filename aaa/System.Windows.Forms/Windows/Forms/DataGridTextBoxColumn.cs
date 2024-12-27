using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020002E7 RID: 743
	public class DataGridTextBoxColumn : DataGridColumnStyle
	{
		// Token: 0x06002C53 RID: 11347 RVA: 0x00077DC2 File Offset: 0x00076DC2
		public DataGridTextBoxColumn()
			: this(null, null)
		{
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x00077DCC File Offset: 0x00076DCC
		public DataGridTextBoxColumn(PropertyDescriptor prop)
			: this(prop, null, false)
		{
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00077DD7 File Offset: 0x00076DD7
		public DataGridTextBoxColumn(PropertyDescriptor prop, string format)
			: this(prop, format, false)
		{
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00077DE4 File Offset: 0x00076DE4
		public DataGridTextBoxColumn(PropertyDescriptor prop, string format, bool isDefault)
			: base(prop, isDefault)
		{
			this.edit = new DataGridTextBox();
			this.edit.BorderStyle = BorderStyle.None;
			this.edit.Multiline = true;
			this.edit.AcceptsReturn = true;
			this.edit.Visible = false;
			this.Format = format;
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00077E50 File Offset: 0x00076E50
		public DataGridTextBoxColumn(PropertyDescriptor prop, bool isDefault)
			: this(prop, null, isDefault)
		{
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x00077E5B File Offset: 0x00076E5B
		[Browsable(false)]
		public virtual TextBox TextBox
		{
			get
			{
				return this.edit;
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x00077E63 File Offset: 0x00076E63
		internal override bool KeyPress(int rowNum, Keys keyData)
		{
			return this.edit.IsInEditOrNavigateMode && base.KeyPress(rowNum, keyData);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00077E7C File Offset: 0x00076E7C
		protected override void SetDataGridInColumn(DataGrid value)
		{
			base.SetDataGridInColumn(value);
			if (this.edit.ParentInternal != null)
			{
				this.edit.ParentInternal.Controls.Remove(this.edit);
			}
			if (value != null)
			{
				value.Controls.Add(this.edit);
			}
			this.edit.SetDataGrid(value);
		}

		// Token: 0x17000785 RID: 1925
		// (set) Token: 0x06002C5B RID: 11355 RVA: 0x00077ED8 File Offset: 0x00076ED8
		[SRDescription("FormatControlFormatDescr")]
		[DefaultValue(null)]
		public override PropertyDescriptor PropertyDescriptor
		{
			set
			{
				base.PropertyDescriptor = value;
				if (this.PropertyDescriptor != null && this.PropertyDescriptor.PropertyType != typeof(object))
				{
					this.typeConverter = TypeDescriptor.GetConverter(this.PropertyDescriptor.PropertyType);
					this.parseMethod = this.PropertyDescriptor.PropertyType.GetMethod("Parse", new Type[]
					{
						typeof(string),
						typeof(IFormatProvider)
					});
				}
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x00077F5E File Offset: 0x00076F5E
		// (set) Token: 0x06002C5D RID: 11357 RVA: 0x00077F68 File Offset: 0x00076F68
		[Editor("System.Windows.Forms.Design.DataGridColumnStyleFormatEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue(null)]
		public string Format
		{
			get
			{
				return this.format;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.format == null || !this.format.Equals(value))
				{
					this.format = value;
					if (this.format.Length == 0 && this.typeConverter != null && !this.typeConverter.CanConvertFrom(typeof(string)))
					{
						this.ReadOnly = true;
					}
					this.Invalidate();
				}
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x00077FD5 File Offset: 0x00076FD5
		// (set) Token: 0x06002C5F RID: 11359 RVA: 0x00077FDD File Offset: 0x00076FDD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IFormatProvider FormatInfo
		{
			get
			{
				return this.formatInfo;
			}
			set
			{
				if (this.formatInfo == null || !this.formatInfo.Equals(value))
				{
					this.formatInfo = value;
				}
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x00077FFC File Offset: 0x00076FFC
		// (set) Token: 0x06002C61 RID: 11361 RVA: 0x00078004 File Offset: 0x00077004
		public override bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				if (!value && (this.format == null || this.format.Length == 0) && this.typeConverter != null && !this.typeConverter.CanConvertFrom(typeof(string)))
				{
					return;
				}
				base.ReadOnly = value;
			}
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x00078050 File Offset: 0x00077050
		private void DebugOut(string s)
		{
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x00078052 File Offset: 0x00077052
		protected internal override void ConcedeFocus()
		{
			this.edit.Bounds = Rectangle.Empty;
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x00078064 File Offset: 0x00077064
		protected void HideEditBox()
		{
			bool focused = this.edit.Focused;
			this.edit.Visible = false;
			if (focused && this.DataGridTableStyle != null && this.DataGridTableStyle.DataGrid != null && this.DataGridTableStyle.DataGrid.CanFocus)
			{
				this.DataGridTableStyle.DataGrid.FocusInternal();
			}
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000780C4 File Offset: 0x000770C4
		protected internal override void UpdateUI(CurrencyManager source, int rowNum, string displayText)
		{
			this.edit.Text = this.GetText(this.GetColumnValueAtRow(source, rowNum));
			if (!this.edit.ReadOnly && displayText != null)
			{
				this.edit.Text = displayText;
			}
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000780FB File Offset: 0x000770FB
		protected void EndEdit()
		{
			this.edit.IsInEditOrNavigateMode = true;
			this.DebugOut("Ending Edit");
			this.Invalidate();
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x0007811C File Offset: 0x0007711C
		protected internal override Size GetPreferredSize(Graphics g, object value)
		{
			Size size = Size.Ceiling(g.MeasureString(this.GetText(value), this.DataGridTableStyle.DataGrid.Font));
			size.Width += this.xMargin * 2 + this.DataGridTableStyle.GridLineWidth;
			size.Height += this.yMargin;
			return size;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x00078183 File Offset: 0x00077183
		protected internal override int GetMinimumHeight()
		{
			return base.FontHeight + this.yMargin + 3;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x00078194 File Offset: 0x00077194
		protected internal override int GetPreferredHeight(Graphics g, object value)
		{
			int num = 0;
			int num2 = 0;
			string text = this.GetText(value);
			while (num != -1 && num < text.Length)
			{
				num = text.IndexOf("\r\n", num + 1);
				num2++;
			}
			return base.FontHeight * num2 + this.yMargin;
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000781DE File Offset: 0x000771DE
		protected internal override void Abort(int rowNum)
		{
			this.RollBack();
			this.HideEditBox();
			this.EndEdit();
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000781F4 File Offset: 0x000771F4
		protected internal override void EnterNullValue()
		{
			if (this.ReadOnly)
			{
				return;
			}
			if (!this.edit.Visible)
			{
				return;
			}
			if (!this.edit.IsInEditOrNavigateMode)
			{
				return;
			}
			this.edit.Text = this.NullText;
			this.edit.IsInEditOrNavigateMode = false;
			if (this.DataGridTableStyle != null && this.DataGridTableStyle.DataGrid != null)
			{
				this.DataGridTableStyle.DataGrid.ColumnStartedEditing(this.edit.Bounds);
			}
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x00078274 File Offset: 0x00077274
		protected internal override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			this.edit.Bounds = Rectangle.Empty;
			if (this.edit.IsInEditOrNavigateMode)
			{
				return true;
			}
			try
			{
				object obj = this.edit.Text;
				if (this.NullText.Equals(obj))
				{
					obj = Convert.DBNull;
					this.edit.Text = this.NullText;
				}
				else if (this.format != null && this.format.Length != 0 && this.parseMethod != null && this.FormatInfo != null)
				{
					obj = this.parseMethod.Invoke(null, new object[]
					{
						this.edit.Text,
						this.FormatInfo
					});
					if (obj is IFormattable)
					{
						this.edit.Text = ((IFormattable)obj).ToString(this.format, this.formatInfo);
					}
					else
					{
						this.edit.Text = obj.ToString();
					}
				}
				else if (this.typeConverter != null && this.typeConverter.CanConvertFrom(typeof(string)))
				{
					obj = this.typeConverter.ConvertFromString(this.edit.Text);
					this.edit.Text = this.typeConverter.ConvertToString(obj);
				}
				this.SetColumnValueAtRow(dataSource, rowNum, obj);
			}
			catch
			{
				this.RollBack();
				return false;
			}
			this.DebugOut("OnCommit completed without Exception.");
			this.EndEdit();
			return true;
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x000783FC File Offset: 0x000773FC
		protected internal override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText, bool cellIsVisible)
		{
			this.DebugOut("Begining Edit, rowNum :" + rowNum.ToString(CultureInfo.InvariantCulture));
			Rectangle rectangle = bounds;
			this.edit.ReadOnly = readOnly || this.ReadOnly || this.DataGridTableStyle.ReadOnly;
			this.edit.Text = this.GetText(this.GetColumnValueAtRow(source, rowNum));
			if (!this.edit.ReadOnly && displayText != null)
			{
				this.DataGridTableStyle.DataGrid.ColumnStartedEditing(bounds);
				this.edit.IsInEditOrNavigateMode = false;
				this.edit.Text = displayText;
			}
			if (cellIsVisible)
			{
				bounds.Offset(this.xMargin, 2 * this.yMargin);
				bounds.Width -= this.xMargin;
				bounds.Height -= 2 * this.yMargin;
				this.DebugOut("edit bounds: " + bounds.ToString());
				this.edit.Bounds = bounds;
				this.edit.Visible = true;
				this.edit.TextAlign = this.Alignment;
			}
			else
			{
				this.edit.Bounds = Rectangle.Empty;
			}
			this.edit.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
			this.edit.FocusInternal();
			this.editRow = rowNum;
			if (!this.edit.ReadOnly)
			{
				this.oldValue = this.edit.Text;
			}
			if (displayText == null)
			{
				this.edit.SelectAll();
			}
			else
			{
				int length = this.edit.Text.Length;
				this.edit.Select(length, 0);
			}
			if (this.edit.Visible)
			{
				this.DataGridTableStyle.DataGrid.Invalidate(rectangle);
			}
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000785D7 File Offset: 0x000775D7
		internal override string GetDisplayText(object value)
		{
			return this.GetText(value);
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000785E0 File Offset: 0x000775E0
		private string GetText(object value)
		{
			if (value is DBNull)
			{
				return this.NullText;
			}
			if (this.format != null && this.format.Length != 0 && value is IFormattable)
			{
				try
				{
					return ((IFormattable)value).ToString(this.format, this.formatInfo);
				}
				catch
				{
					goto IL_0084;
				}
			}
			if (this.typeConverter != null && this.typeConverter.CanConvertTo(typeof(string)))
			{
				return (string)this.typeConverter.ConvertTo(value, typeof(string));
			}
			IL_0084:
			if (value == null)
			{
				return "";
			}
			return value.ToString();
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x00078694 File Offset: 0x00077694
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			this.Paint(g, bounds, source, rowNum, false);
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x000786A4 File Offset: 0x000776A4
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			string text = this.GetText(this.GetColumnValueAtRow(source, rowNum));
			this.PaintText(g, bounds, text, alignToRight);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000786CC File Offset: 0x000776CC
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			string text = this.GetText(this.GetColumnValueAtRow(source, rowNum));
			this.PaintText(g, bounds, text, backBrush, foreBrush, alignToRight);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x000786F8 File Offset: 0x000776F8
		protected void PaintText(Graphics g, Rectangle bounds, string text, bool alignToRight)
		{
			this.PaintText(g, bounds, text, this.DataGridTableStyle.BackBrush, this.DataGridTableStyle.ForeBrush, alignToRight);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0007871C File Offset: 0x0007771C
		protected void PaintText(Graphics g, Rectangle textBounds, string text, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			Rectangle rectangle = textBounds;
			StringFormat stringFormat = new StringFormat();
			if (alignToRight)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			}
			stringFormat.Alignment = ((this.Alignment == HorizontalAlignment.Left) ? StringAlignment.Near : ((this.Alignment == HorizontalAlignment.Center) ? StringAlignment.Center : StringAlignment.Far));
			stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
			g.FillRectangle(backBrush, rectangle);
			rectangle.Offset(0, 2 * this.yMargin);
			rectangle.Height -= 2 * this.yMargin;
			g.DrawString(text, this.DataGridTableStyle.DataGrid.Font, foreBrush, rectangle, stringFormat);
			stringFormat.Dispose();
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000787C8 File Offset: 0x000777C8
		private void RollBack()
		{
			this.edit.Text = this.oldValue;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000787DB File Offset: 0x000777DB
		protected internal override void ReleaseHostedControl()
		{
			if (this.edit.ParentInternal != null)
			{
				this.edit.ParentInternal.Controls.Remove(this.edit);
			}
		}

		// Token: 0x04001834 RID: 6196
		private int xMargin = 2;

		// Token: 0x04001835 RID: 6197
		private int yMargin = 1;

		// Token: 0x04001836 RID: 6198
		private string format;

		// Token: 0x04001837 RID: 6199
		private TypeConverter typeConverter;

		// Token: 0x04001838 RID: 6200
		private IFormatProvider formatInfo;

		// Token: 0x04001839 RID: 6201
		private MethodInfo parseMethod;

		// Token: 0x0400183A RID: 6202
		private DataGridTextBox edit;

		// Token: 0x0400183B RID: 6203
		private string oldValue;

		// Token: 0x0400183C RID: 6204
		private int editRow = -1;
	}
}
