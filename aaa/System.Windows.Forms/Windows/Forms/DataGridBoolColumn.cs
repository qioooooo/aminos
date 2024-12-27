using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	// Token: 0x020002D0 RID: 720
	public class DataGridBoolColumn : DataGridColumnStyle
	{
		// Token: 0x06002986 RID: 10630 RVA: 0x0006DA0C File Offset: 0x0006CA0C
		public DataGridBoolColumn()
		{
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x0006DA5C File Offset: 0x0006CA5C
		public DataGridBoolColumn(PropertyDescriptor prop)
			: base(prop)
		{
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x0006DAAC File Offset: 0x0006CAAC
		public DataGridBoolColumn(PropertyDescriptor prop, bool isDefault)
			: base(prop, isDefault)
		{
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x0006DAFD File Offset: 0x0006CAFD
		// (set) Token: 0x0600298A RID: 10634 RVA: 0x0006DB05 File Offset: 0x0006CB05
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(true)]
		public object TrueValue
		{
			get
			{
				return this.trueValue;
			}
			set
			{
				if (!this.trueValue.Equals(value))
				{
					this.trueValue = value;
					this.OnTrueValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x0600298B RID: 10635 RVA: 0x0006DB2D File Offset: 0x0006CB2D
		// (remove) Token: 0x0600298C RID: 10636 RVA: 0x0006DB40 File Offset: 0x0006CB40
		public event EventHandler TrueValueChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventTrueValue, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventTrueValue, value);
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x0006DB53 File Offset: 0x0006CB53
		// (set) Token: 0x0600298E RID: 10638 RVA: 0x0006DB5B File Offset: 0x0006CB5B
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(false)]
		public object FalseValue
		{
			get
			{
				return this.falseValue;
			}
			set
			{
				if (!this.falseValue.Equals(value))
				{
					this.falseValue = value;
					this.OnFalseValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600298F RID: 10639 RVA: 0x0006DB83 File Offset: 0x0006CB83
		// (remove) Token: 0x06002990 RID: 10640 RVA: 0x0006DB96 File Offset: 0x0006CB96
		public event EventHandler FalseValueChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventFalseValue, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventFalseValue, value);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002991 RID: 10641 RVA: 0x0006DBA9 File Offset: 0x0006CBA9
		// (set) Token: 0x06002992 RID: 10642 RVA: 0x0006DBB1 File Offset: 0x0006CBB1
		[TypeConverter(typeof(StringConverter))]
		public object NullValue
		{
			get
			{
				return this.nullValue;
			}
			set
			{
				if (!this.nullValue.Equals(value))
				{
					this.nullValue = value;
					this.OnFalseValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x0006DBD9 File Offset: 0x0006CBD9
		protected internal override void ConcedeFocus()
		{
			base.ConcedeFocus();
			this.isSelected = false;
			this.isEditing = false;
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x0006DBF0 File Offset: 0x0006CBF0
		private Rectangle GetCheckBoxBounds(Rectangle bounds, bool alignToRight)
		{
			if (alignToRight)
			{
				return new Rectangle(bounds.X + (bounds.Width - DataGridBoolColumn.idealCheckSize) / 2, bounds.Y + (bounds.Height - DataGridBoolColumn.idealCheckSize) / 2, (bounds.Width < DataGridBoolColumn.idealCheckSize) ? bounds.Width : DataGridBoolColumn.idealCheckSize, DataGridBoolColumn.idealCheckSize);
			}
			return new Rectangle(Math.Max(0, bounds.X + (bounds.Width - DataGridBoolColumn.idealCheckSize) / 2), Math.Max(0, bounds.Y + (bounds.Height - DataGridBoolColumn.idealCheckSize) / 2), (bounds.Width < DataGridBoolColumn.idealCheckSize) ? bounds.Width : DataGridBoolColumn.idealCheckSize, DataGridBoolColumn.idealCheckSize);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x0006DCB8 File Offset: 0x0006CCB8
		protected internal override object GetColumnValueAtRow(CurrencyManager lm, int row)
		{
			object columnValueAtRow = base.GetColumnValueAtRow(lm, row);
			object obj = Convert.DBNull;
			if (columnValueAtRow.Equals(this.trueValue))
			{
				obj = true;
			}
			else if (columnValueAtRow.Equals(this.falseValue))
			{
				obj = false;
			}
			return obj;
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x0006DD04 File Offset: 0x0006CD04
		private bool IsReadOnly()
		{
			bool flag = this.ReadOnly;
			if (this.DataGridTableStyle != null)
			{
				flag = flag || this.DataGridTableStyle.ReadOnly;
				if (this.DataGridTableStyle.DataGrid != null)
				{
					flag = flag || this.DataGridTableStyle.DataGrid.ReadOnly;
				}
			}
			return flag;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0006DD58 File Offset: 0x0006CD58
		protected internal override void SetColumnValueAtRow(CurrencyManager lm, int row, object value)
		{
			object obj = null;
			if (true.Equals(value))
			{
				obj = this.TrueValue;
			}
			else if (false.Equals(value))
			{
				obj = this.FalseValue;
			}
			else if (Convert.IsDBNull(value))
			{
				obj = this.NullValue;
			}
			this.currentValue = obj;
			base.SetColumnValueAtRow(lm, row, obj);
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0006DDB0 File Offset: 0x0006CDB0
		protected internal override Size GetPreferredSize(Graphics g, object value)
		{
			return new Size(DataGridBoolColumn.idealCheckSize + 2, DataGridBoolColumn.idealCheckSize + 2);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0006DDC5 File Offset: 0x0006CDC5
		protected internal override int GetMinimumHeight()
		{
			return DataGridBoolColumn.idealCheckSize + 2;
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x0006DDCE File Offset: 0x0006CDCE
		protected internal override int GetPreferredHeight(Graphics g, object value)
		{
			return DataGridBoolColumn.idealCheckSize + 2;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x0006DDD7 File Offset: 0x0006CDD7
		protected internal override void Abort(int rowNum)
		{
			this.isSelected = false;
			this.isEditing = false;
			this.Invalidate();
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x0006DDED File Offset: 0x0006CDED
		protected internal override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			this.isSelected = false;
			this.Invalidate();
			if (!this.isEditing)
			{
				return true;
			}
			this.SetColumnValueAtRow(dataSource, rowNum, this.currentValue);
			this.isEditing = false;
			return true;
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0006DE1C File Offset: 0x0006CE1C
		protected internal override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText, bool cellIsVisible)
		{
			this.isSelected = true;
			DataGrid dataGrid = this.DataGridTableStyle.DataGrid;
			if (!dataGrid.Focused)
			{
				dataGrid.FocusInternal();
			}
			if (!readOnly && !this.IsReadOnly())
			{
				this.editingRow = rowNum;
				this.currentValue = this.GetColumnValueAtRow(source, rowNum);
			}
			base.Invalidate();
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x0006DE72 File Offset: 0x0006CE72
		internal override bool KeyPress(int rowNum, Keys keyData)
		{
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly() && (keyData & Keys.KeyCode) == Keys.Space)
			{
				this.ToggleValue();
				this.Invalidate();
				return true;
			}
			return base.KeyPress(rowNum, keyData);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0006DEAE File Offset: 0x0006CEAE
		internal override bool MouseDown(int rowNum, int x, int y)
		{
			base.MouseDown(rowNum, x, y);
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly())
			{
				this.ToggleValue();
				this.Invalidate();
				return true;
			}
			return false;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x0006DEE4 File Offset: 0x0006CEE4
		private void OnTrueValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventTrueValue] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0006DF14 File Offset: 0x0006CF14
		private void OnFalseValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventFalseValue] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0006DF44 File Offset: 0x0006CF44
		private void OnAllowNullChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventAllowNull] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x0006DF72 File Offset: 0x0006CF72
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			this.Paint(g, bounds, source, rowNum, false);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x0006DF80 File Offset: 0x0006CF80
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			this.Paint(g, bounds, source, rowNum, this.DataGridTableStyle.BackBrush, this.DataGridTableStyle.ForeBrush, alignToRight);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0006DFA8 File Offset: 0x0006CFA8
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			object obj = ((this.isEditing && this.editingRow == rowNum) ? this.currentValue : this.GetColumnValueAtRow(source, rowNum));
			ButtonState buttonState = ButtonState.Inactive;
			if (!Convert.IsDBNull(obj))
			{
				buttonState = (((bool)obj) ? ButtonState.Checked : ButtonState.Normal);
			}
			Rectangle checkBoxBounds = this.GetCheckBoxBounds(bounds, alignToRight);
			Region clip = g.Clip;
			g.ExcludeClip(checkBoxBounds);
			Brush brush = (this.DataGridTableStyle.IsDefault ? this.DataGridTableStyle.DataGrid.SelectionBackBrush : this.DataGridTableStyle.SelectionBackBrush);
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly())
			{
				g.FillRectangle(brush, bounds);
			}
			else
			{
				g.FillRectangle(backBrush, bounds);
			}
			g.Clip = clip;
			if (buttonState == ButtonState.Inactive)
			{
				ControlPaint.DrawMixedCheckBox(g, checkBoxBounds, ButtonState.Checked);
			}
			else
			{
				ControlPaint.DrawCheckBox(g, checkBoxBounds, buttonState);
			}
			if (this.IsReadOnly() && this.isSelected && source.Position == rowNum)
			{
				bounds.Inflate(-1, -1);
				Pen pen = new Pen(brush);
				pen.DashStyle = DashStyle.Dash;
				g.DrawRectangle(pen, bounds);
				pen.Dispose();
				bounds.Inflate(1, 1);
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060029A6 RID: 10662 RVA: 0x0006E0DC File Offset: 0x0006D0DC
		// (set) Token: 0x060029A7 RID: 10663 RVA: 0x0006E0E4 File Offset: 0x0006D0E4
		[SRDescription("DataGridBoolColumnAllowNullValue")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				if (this.allowNull != value)
				{
					this.allowNull = value;
					if (!value && Convert.IsDBNull(this.currentValue))
					{
						this.currentValue = false;
						this.Invalidate();
					}
					this.OnAllowNullChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x060029A8 RID: 10664 RVA: 0x0006E123 File Offset: 0x0006D123
		// (remove) Token: 0x060029A9 RID: 10665 RVA: 0x0006E136 File Offset: 0x0006D136
		public event EventHandler AllowNullChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventAllowNull, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventAllowNull, value);
			}
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x0006E149 File Offset: 0x0006D149
		protected internal override void EnterNullValue()
		{
			if (!this.AllowNull || this.IsReadOnly())
			{
				return;
			}
			if (this.currentValue != Convert.DBNull)
			{
				this.currentValue = Convert.DBNull;
				this.Invalidate();
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x0006E17A File Offset: 0x0006D17A
		private void ResetNullValue()
		{
			this.NullValue = Convert.DBNull;
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x0006E187 File Offset: 0x0006D187
		private bool ShouldSerializeNullValue()
		{
			return this.nullValue != Convert.DBNull;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0006E19C File Offset: 0x0006D19C
		private void ToggleValue()
		{
			if (this.currentValue is bool && !(bool)this.currentValue)
			{
				this.currentValue = true;
			}
			else if (this.AllowNull)
			{
				if (Convert.IsDBNull(this.currentValue))
				{
					this.currentValue = false;
				}
				else
				{
					this.currentValue = Convert.DBNull;
				}
			}
			else
			{
				this.currentValue = false;
			}
			this.isEditing = true;
			this.DataGridTableStyle.DataGrid.ColumnStartedEditing(Rectangle.Empty);
		}

		// Token: 0x04001760 RID: 5984
		private static readonly int idealCheckSize = 14;

		// Token: 0x04001761 RID: 5985
		private bool isEditing;

		// Token: 0x04001762 RID: 5986
		private bool isSelected;

		// Token: 0x04001763 RID: 5987
		private bool allowNull = true;

		// Token: 0x04001764 RID: 5988
		private int editingRow = -1;

		// Token: 0x04001765 RID: 5989
		private object currentValue = Convert.DBNull;

		// Token: 0x04001766 RID: 5990
		private object trueValue = true;

		// Token: 0x04001767 RID: 5991
		private object falseValue = false;

		// Token: 0x04001768 RID: 5992
		private object nullValue = Convert.DBNull;

		// Token: 0x04001769 RID: 5993
		private static readonly object EventTrueValue = new object();

		// Token: 0x0400176A RID: 5994
		private static readonly object EventFalseValue = new object();

		// Token: 0x0400176B RID: 5995
		private static readonly object EventAllowNull = new object();
	}
}
