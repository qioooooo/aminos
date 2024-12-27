using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200032A RID: 810
	public class DataGridViewCheckBoxCell : DataGridViewCell, IDataGridViewEditingCell
	{
		// Token: 0x060033D2 RID: 13266 RVA: 0x000B58EB File Offset: 0x000B48EB
		public DataGridViewCheckBoxCell()
			: this(false)
		{
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000B58F4 File Offset: 0x000B48F4
		public DataGridViewCheckBoxCell(bool threeState)
		{
			if (threeState)
			{
				this.flags = 1;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x000B5906 File Offset: 0x000B4906
		// (set) Token: 0x060033D5 RID: 13269 RVA: 0x000B5910 File Offset: 0x000B4910
		public virtual object EditingCellFormattedValue
		{
			get
			{
				return this.GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				if (this.FormattedValueType == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
				}
				if (value == null || !this.FormattedValueType.IsAssignableFrom(value.GetType()))
				{
					throw new ArgumentException(SR.GetString("DataGridViewCheckBoxCell_InvalidValueType"));
				}
				if (value is CheckState)
				{
					if ((CheckState)value == CheckState.Checked)
					{
						this.flags |= 16;
						this.flags = (byte)((int)this.flags & -33);
						return;
					}
					if ((CheckState)value == CheckState.Indeterminate)
					{
						this.flags |= 32;
						this.flags = (byte)((int)this.flags & -17);
						return;
					}
					this.flags = (byte)((int)this.flags & -17);
					this.flags = (byte)((int)this.flags & -33);
					return;
				}
				else
				{
					if (value is bool)
					{
						if ((bool)value)
						{
							this.flags |= 16;
						}
						else
						{
							this.flags = (byte)((int)this.flags & -17);
						}
						this.flags = (byte)((int)this.flags & -33);
						return;
					}
					throw new ArgumentException(SR.GetString("DataGridViewCheckBoxCell_InvalidValueType"));
				}
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x000B5A2A File Offset: 0x000B4A2A
		// (set) Token: 0x060033D7 RID: 13271 RVA: 0x000B5A3A File Offset: 0x000B4A3A
		public virtual bool EditingCellValueChanged
		{
			get
			{
				return (this.flags & 2) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2;
					return;
				}
				this.flags = (byte)((int)this.flags & -3);
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000B5A60 File Offset: 0x000B4A60
		public virtual object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context)
		{
			if (this.FormattedValueType == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
			}
			if (this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
			{
				if ((this.flags & 16) != 0)
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardChecked");
					}
					return CheckState.Checked;
				}
				else if ((this.flags & 32) != 0)
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardIndeterminate");
					}
					return CheckState.Indeterminate;
				}
				else
				{
					if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
					{
						return SR.GetString("DataGridViewCheckBoxCell_ClipboardUnchecked");
					}
					return CheckState.Unchecked;
				}
			}
			else
			{
				if (!this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
				{
					return null;
				}
				bool flag = (this.flags & 16) != 0;
				if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
				{
					return SR.GetString(flag ? "DataGridViewCheckBoxCell_ClipboardTrue" : "DataGridViewCheckBoxCell_ClipboardFalse");
				}
				return flag;
			}
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000B5B46 File Offset: 0x000B4B46
		public virtual void PrepareEditingCellForEdit(bool selectAll)
		{
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x000B5B48 File Offset: 0x000B4B48
		// (set) Token: 0x060033DB RID: 13275 RVA: 0x000B5B6E File Offset: 0x000B4B6E
		private ButtonState ButtonState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewCheckBoxCell.PropButtonCellState, out flag);
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
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropButtonCellState, (int)value);
				}
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x000B5B8A File Offset: 0x000B4B8A
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x000B5B8D File Offset: 0x000B4B8D
		// (set) Token: 0x060033DE RID: 13278 RVA: 0x000B5BA0 File Offset: 0x000B4BA0
		[DefaultValue(null)]
		public object FalseValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropFalseValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropFalseValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropFalseValue, value);
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

		// Token: 0x17000950 RID: 2384
		// (set) Token: 0x060033DF RID: 13279 RVA: 0x000B5C02 File Offset: 0x000B4C02
		internal object FalseValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropFalseValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropFalseValue, value);
				}
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000B5C2C File Offset: 0x000B4C2C
		// (set) Token: 0x060033E1 RID: 13281 RVA: 0x000B5C54 File Offset: 0x000B4C54
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewCheckBoxCell.PropFlatStyle, out flag);
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
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x17000952 RID: 2386
		// (set) Token: 0x060033E2 RID: 13282 RVA: 0x000B5CA7 File Offset: 0x000B4CA7
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewCheckBoxCell.PropFlatStyle, (int)value);
				}
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x000B5CC3 File Offset: 0x000B4CC3
		public override Type FormattedValueType
		{
			get
			{
				if (this.ThreeState)
				{
					return DataGridViewCheckBoxCell.defaultCheckStateType;
				}
				return DataGridViewCheckBoxCell.defaultBooleanType;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x000B5CD8 File Offset: 0x000B4CD8
		// (set) Token: 0x060033E5 RID: 13285 RVA: 0x000B5CEC File Offset: 0x000B4CEC
		[DefaultValue(null)]
		public object IndeterminateValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropIndeterminateValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropIndeterminateValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropIndeterminateValue, value);
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

		// Token: 0x17000955 RID: 2389
		// (set) Token: 0x060033E6 RID: 13286 RVA: 0x000B5D4E File Offset: 0x000B4D4E
		internal object IndeterminateValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropIndeterminateValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropIndeterminateValue, value);
				}
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x000B5D76 File Offset: 0x000B4D76
		// (set) Token: 0x060033E8 RID: 13288 RVA: 0x000B5D88 File Offset: 0x000B4D88
		[DefaultValue(false)]
		public bool ThreeState
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (this.ThreeState != value)
				{
					this.ThreeStateInternal = value;
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

		// Token: 0x17000957 RID: 2391
		// (set) Token: 0x060033E9 RID: 13289 RVA: 0x000B5DD4 File Offset: 0x000B4DD4
		internal bool ThreeStateInternal
		{
			set
			{
				if (this.ThreeState != value)
				{
					if (value)
					{
						this.flags |= 1;
						return;
					}
					this.flags = (byte)((int)this.flags & -2);
				}
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x000B5E02 File Offset: 0x000B4E02
		// (set) Token: 0x060033EB RID: 13291 RVA: 0x000B5E14 File Offset: 0x000B4E14
		[DefaultValue(null)]
		public object TrueValue
		{
			get
			{
				return base.Properties.GetObject(DataGridViewCheckBoxCell.PropTrueValue);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropTrueValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropTrueValue, value);
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

		// Token: 0x17000959 RID: 2393
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x000B5E76 File Offset: 0x000B4E76
		internal object TrueValueInternal
		{
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewCheckBoxCell.PropTrueValue))
				{
					base.Properties.SetObject(DataGridViewCheckBoxCell.PropTrueValue, value);
				}
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000B5EA0 File Offset: 0x000B4EA0
		// (set) Token: 0x060033EE RID: 13294 RVA: 0x000B5ECC File Offset: 0x000B4ECC
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				if (this.ThreeState)
				{
					return DataGridViewCheckBoxCell.defaultCheckStateType;
				}
				return DataGridViewCheckBoxCell.defaultBooleanType;
			}
			set
			{
				base.ValueType = value;
				this.ThreeState = value != null && DataGridViewCheckBoxCell.defaultCheckStateType.IsAssignableFrom(value);
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000B5EEC File Offset: 0x000B4EEC
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewCheckBoxCell dataGridViewCheckBoxCell;
			if (type == DataGridViewCheckBoxCell.cellType)
			{
				dataGridViewCheckBoxCell = new DataGridViewCheckBoxCell();
			}
			else
			{
				dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewCheckBoxCell);
			dataGridViewCheckBoxCell.ThreeStateInternal = this.ThreeState;
			dataGridViewCheckBoxCell.TrueValueInternal = this.TrueValue;
			dataGridViewCheckBoxCell.FalseValueInternal = this.FalseValue;
			dataGridViewCheckBoxCell.IndeterminateValueInternal = this.IndeterminateValue;
			dataGridViewCheckBoxCell.FlatStyleInternal = this.FlatStyle;
			return dataGridViewCheckBoxCell;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000B5F60 File Offset: 0x000B4F60
		private bool CommonContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			return currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == e.RowIndex && base.DataGridView.IsCurrentCellInEditMode;
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000B5FA4 File Offset: 0x000B4FA4
		protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return this.CommonContentClickUnsharesRow(e);
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000B5FAD File Offset: 0x000B4FAD
		protected override bool ContentDoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return this.CommonContentClickUnsharesRow(e);
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000B5FB6 File Offset: 0x000B4FB6
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewCheckBoxCell.DataGridViewCheckBoxCellAccessibleObject(this);
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000B5FC0 File Offset: 0x000B4FC0
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

		// Token: 0x060033F5 RID: 13301 RVA: 0x000B6018 File Offset: 0x000B5018
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
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.IsCurrentCellInEditMode)
			{
				return Rectangle.Empty;
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, null, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000B60CC File Offset: 0x000B50CC
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (value != null)
			{
				if (this.ThreeState)
				{
					if (value.Equals(this.TrueValue) || (value is int && (int)value == 1))
					{
						value = CheckState.Checked;
					}
					else if (value.Equals(this.FalseValue) || (value is int && (int)value == 0))
					{
						value = CheckState.Unchecked;
					}
					else if (value.Equals(this.IndeterminateValue) || (value is int && (int)value == 2))
					{
						value = CheckState.Indeterminate;
					}
				}
				else if (value.Equals(this.TrueValue) || (value is int && (int)value != 0))
				{
					value = true;
				}
				else if (value.Equals(this.FalseValue) || (value is int && (int)value == 0))
				{
					value = false;
				}
			}
			object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			if (formattedValue != null && (context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
			{
				if (formattedValue is bool)
				{
					bool flag = (bool)formattedValue;
					if (flag)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardChecked" : "DataGridViewCheckBoxCell_ClipboardTrue");
					}
					return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardUnchecked" : "DataGridViewCheckBoxCell_ClipboardFalse");
				}
				else if (formattedValue is CheckState)
				{
					CheckState checkState = (CheckState)formattedValue;
					if (checkState == CheckState.Checked)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardChecked" : "DataGridViewCheckBoxCell_ClipboardTrue");
					}
					if (checkState == CheckState.Unchecked)
					{
						return SR.GetString(this.ThreeState ? "DataGridViewCheckBoxCell_ClipboardUnchecked" : "DataGridViewCheckBoxCell_ClipboardFalse");
					}
					return SR.GetString("DataGridViewCheckBoxCell_ClipboardIndeterminate");
				}
			}
			return formattedValue;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000B6274 File Offset: 0x000B5274
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
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			Size size;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				Size glyphSize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					glyphSize.Width -= 3;
					glyphSize.Height -= 3;
					break;
				case FlatStyle.Popup:
					glyphSize.Width -= 2;
					glyphSize.Height -= 2;
					break;
				}
				switch (freeDimensionFromConstraint)
				{
				case DataGridViewFreeDimension.Height:
					size = new Size(0, glyphSize.Height + num2 + 4);
					break;
				case DataGridViewFreeDimension.Width:
					size = new Size(glyphSize.Width + num + 4, 0);
					break;
				default:
					size = new Size(glyphSize.Width + num + 4, glyphSize.Height + num2 + 4);
					break;
				}
			}
			else
			{
				int num3;
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					num3 = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal).Width - 3;
					break;
				case FlatStyle.Popup:
					num3 = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal).Width - 2;
					break;
				default:
					num3 = SystemInformation.Border3DSize.Width * 2 + 9 + 4;
					break;
				}
				switch (freeDimensionFromConstraint)
				{
				case DataGridViewFreeDimension.Height:
					size = new Size(0, num3 + num2);
					break;
				case DataGridViewFreeDimension.Width:
					size = new Size(num3 + num, 0);
					break;
				default:
					size = new Size(num3 + num, num3 + num2);
					break;
				}
			}
			if (base.DataGridView.ShowCellErrors)
			{
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					size.Width = Math.Max(size.Width, num + 8 + 12);
				}
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000B64B4 File Offset: 0x000B54B4
		protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000B64DB File Offset: 0x000B54DB
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode == Keys.Space;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000B64E7 File Offset: 0x000B54E7
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000B64F8 File Offset: 0x000B54F8
		protected override bool MouseEnterUnsharesRow(int rowIndex)
		{
			return base.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && rowIndex == base.DataGridView.MouseDownCellAddress.Y;
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000B6538 File Offset: 0x000B5538
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return (this.ButtonState & ButtonState.Pushed) != ButtonState.Normal;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000B654C File Offset: 0x000B554C
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return e.Button == MouseButtons.Left;
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000B655B File Offset: 0x000B555B
		private void NotifyDataGridViewOfValueChange()
		{
			this.flags |= 2;
			base.DataGridView.NotifyCurrentCellDirty(true);
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000B6578 File Offset: 0x000B5578
		private void OnCommonContentClick(DataGridViewCellEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == e.RowIndex && base.DataGridView.IsCurrentCellInEditMode && this.SwitchFormattedValue())
			{
				this.NotifyDataGridViewOfValueChange();
			}
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000B65D3 File Offset: 0x000B55D3
		protected override void OnContentClick(DataGridViewCellEventArgs e)
		{
			this.OnCommonContentClick(e);
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000B65DC File Offset: 0x000B55DC
		protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
		{
			this.OnCommonContentClick(e);
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000B65E8 File Offset: 0x000B55E8
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

		// Token: 0x06003403 RID: 13315 RVA: 0x000B663C File Offset: 0x000B563C
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

		// Token: 0x06003404 RID: 13316 RVA: 0x000B66E8 File Offset: 0x000B56E8
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

		// Token: 0x06003405 RID: 13317 RVA: 0x000B6703 File Offset: 0x000B5703
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.Button == MouseButtons.Left && DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
			}
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000B673C File Offset: 0x000B573C
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				DataGridViewCheckBoxCell.mouseInContentBounds = false;
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

		// Token: 0x06003407 RID: 13319 RVA: 0x000B67F0 File Offset: 0x000B57F0
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			bool flag = DataGridViewCheckBoxCell.mouseInContentBounds;
			DataGridViewCheckBoxCell.mouseInContentBounds = base.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (flag != DataGridViewCheckBoxCell.mouseInContentBounds)
			{
				if (base.DataGridView.ApplyVisualStylesToInnerCells || this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (e.ColumnIndex == base.DataGridView.MouseDownCellAddress.X && e.RowIndex == base.DataGridView.MouseDownCellAddress.Y && Control.MouseButtons == MouseButtons.Left)
				{
					if ((this.ButtonState & ButtonState.Pushed) == ButtonState.Normal && DataGridViewCheckBoxCell.mouseInContentBounds && base.DataGridView.CellMouseDownInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState | ButtonState.Pushed, e.RowIndex);
					}
					else if ((this.ButtonState & ButtonState.Pushed) != ButtonState.Normal && !DataGridViewCheckBoxCell.mouseInContentBounds)
					{
						this.UpdateButtonState(this.ButtonState & ~ButtonState.Pushed, e.RowIndex);
					}
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000B6926 File Offset: 0x000B5926
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

		// Token: 0x06003409 RID: 13321 RVA: 0x000B6958 File Offset: 0x000B5958
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000B6990 File Offset: 0x000B5990
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
			bool flag = (elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			bool flag2 = false;
			bool flag3 = true;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.IsCurrentCellInEditMode)
			{
				flag3 = false;
			}
			CheckState checkState;
			ButtonState buttonState;
			if (formattedValue != null && formattedValue is CheckState)
			{
				checkState = (CheckState)formattedValue;
				buttonState = ((checkState == CheckState.Unchecked) ? ButtonState.Normal : ButtonState.Checked);
				flag2 = checkState == CheckState.Indeterminate;
			}
			else if (formattedValue != null && formattedValue is bool)
			{
				if ((bool)formattedValue)
				{
					checkState = CheckState.Checked;
					buttonState = ButtonState.Checked;
				}
				else
				{
					checkState = CheckState.Unchecked;
					buttonState = ButtonState.Normal;
				}
			}
			else
			{
				buttonState = ButtonState.Normal;
				checkState = CheckState.Unchecked;
			}
			if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
			{
				buttonState |= ButtonState.Pushed;
			}
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
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
			if (paint && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle, Color.Empty, cachedBrush.Color);
			}
			Rectangle rectangle3 = rectangle;
			rectangle.Inflate(-2, -2);
			CheckBoxState checkBoxState = CheckBoxState.UncheckedNormal;
			Size size;
			if (base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				checkBoxState = CheckBoxRenderer.ConvertFromButtonState(buttonState, flag2, base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds);
				size = CheckBoxRenderer.GetGlyphSize(g, checkBoxState);
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					size.Width -= 3;
					size.Height -= 3;
					break;
				case FlatStyle.Popup:
					size.Width -= 2;
					size.Height -= 2;
					break;
				}
			}
			else
			{
				switch (this.FlatStyle)
				{
				case FlatStyle.Flat:
					size = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
					size.Width -= 3;
					size.Height -= 3;
					break;
				case FlatStyle.Popup:
					size = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
					size.Width -= 2;
					size.Height -= 2;
					break;
				default:
					size = new Size(SystemInformation.Border3DSize.Width * 2 + 9, SystemInformation.Border3DSize.Width * 2 + 9);
					break;
				}
			}
			Rectangle rectangle4;
			if (rectangle.Width >= size.Width && rectangle.Height >= size.Height && (paint || computeContentBounds))
			{
				int num = 0;
				int num2 = 0;
				if ((!base.DataGridView.RightToLeftInternal && (cellStyle.Alignment & DataGridViewCheckBoxCell.anyRight) != DataGridViewContentAlignment.NotSet) || (base.DataGridView.RightToLeftInternal && (cellStyle.Alignment & DataGridViewCheckBoxCell.anyLeft) != DataGridViewContentAlignment.NotSet))
				{
					num = rectangle.Right - size.Width;
				}
				else if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyCenter) != DataGridViewContentAlignment.NotSet)
				{
					num = rectangle.Left + (rectangle.Width - size.Width) / 2;
				}
				else
				{
					num = rectangle.Left;
				}
				if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					num2 = rectangle.Bottom - size.Height;
				}
				else if ((cellStyle.Alignment & DataGridViewCheckBoxCell.anyMiddle) != DataGridViewContentAlignment.NotSet)
				{
					num2 = rectangle.Top + (rectangle.Height - size.Height) / 2;
				}
				else
				{
					num2 = rectangle.Top;
				}
				if (base.DataGridView.ApplyVisualStylesToInnerCells && this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup)
				{
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.DrawCheckBox(g, new Rectangle(num, num2, size.Width, size.Height), (int)checkBoxState);
					}
					rectangle4 = new Rectangle(num, num2, size.Width, size.Height);
				}
				else if (this.FlatStyle == FlatStyle.System || this.FlatStyle == FlatStyle.Standard)
				{
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						if (flag2)
						{
							ControlPaint.DrawMixedCheckBox(g, num, num2, size.Width, size.Height, buttonState);
						}
						else
						{
							ControlPaint.DrawCheckBox(g, num, num2, size.Width, size.Height, buttonState);
						}
					}
					rectangle4 = new Rectangle(num, num2, size.Width, size.Height);
				}
				else if (this.FlatStyle == FlatStyle.Flat)
				{
					Rectangle rectangle5 = new Rectangle(num, num2, size.Width, size.Height);
					SolidBrush solidBrush = null;
					SolidBrush solidBrush2 = null;
					Color color = Color.Empty;
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						solidBrush = base.DataGridView.GetCachedBrush(flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
						solidBrush2 = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
						color = ControlPaint.LightLight(solidBrush2.Color);
						if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds)
						{
							float num3 = 0.9f;
							if ((double)color.GetBrightness() < 0.5)
							{
								num3 = 1.2f;
							}
							color = Color.FromArgb(ButtonBaseAdapter.ColorOptions.Adjust255(num3, (int)color.R), ButtonBaseAdapter.ColorOptions.Adjust255(num3, (int)color.G), ButtonBaseAdapter.ColorOptions.Adjust255(num3, (int)color.B));
						}
						color = g.GetNearestColor(color);
						using (Pen pen = new Pen(solidBrush.Color))
						{
							g.DrawLine(pen, rectangle5.Left, rectangle5.Top, rectangle5.Right - 1, rectangle5.Top);
							g.DrawLine(pen, rectangle5.Left, rectangle5.Top, rectangle5.Left, rectangle5.Bottom - 1);
						}
					}
					rectangle5.Inflate(-1, -1);
					rectangle5.Width++;
					rectangle5.Height++;
					if (paint && DataGridViewCell.PaintContentForeground(paintParts))
					{
						if (checkState == CheckState.Indeterminate)
						{
							ButtonBaseAdapter.DrawDitheredFill(g, solidBrush2.Color, color, rectangle5);
						}
						else
						{
							using (SolidBrush solidBrush3 = new SolidBrush(color))
							{
								g.FillRectangle(solidBrush3, rectangle5);
							}
						}
						if (checkState != CheckState.Unchecked)
						{
							Rectangle rectangle6 = new Rectangle(num - 1, num2 - 1, size.Width + 3, size.Height + 3);
							rectangle6.Width++;
							rectangle6.Height++;
							if (DataGridViewCheckBoxCell.checkImage == null || DataGridViewCheckBoxCell.checkImage.Width != rectangle6.Width || DataGridViewCheckBoxCell.checkImage.Height != rectangle6.Height)
							{
								if (DataGridViewCheckBoxCell.checkImage != null)
								{
									DataGridViewCheckBoxCell.checkImage.Dispose();
									DataGridViewCheckBoxCell.checkImage = null;
								}
								NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, rectangle6.Width, rectangle6.Height);
								Bitmap bitmap = new Bitmap(rectangle6.Width, rectangle6.Height);
								using (Graphics graphics = Graphics.FromImage(bitmap))
								{
									graphics.Clear(Color.Transparent);
									IntPtr hdc = graphics.GetHdc();
									try
									{
										SafeNativeMethods.DrawFrameControl(new HandleRef(graphics, hdc), ref rect, 2, 1);
									}
									finally
									{
										graphics.ReleaseHdcInternal(hdc);
									}
								}
								bitmap.MakeTransparent();
								DataGridViewCheckBoxCell.checkImage = bitmap;
							}
							rectangle6.Y--;
							ControlPaint.DrawImageColorized(g, DataGridViewCheckBoxCell.checkImage, rectangle6, (checkState == CheckState.Indeterminate) ? ControlPaint.LightLight(solidBrush.Color) : solidBrush.Color);
						}
					}
					rectangle4 = rectangle5;
				}
				else
				{
					Rectangle rectangle7 = new Rectangle(num, num2, size.Width - 1, size.Height - 1);
					rectangle7.Y -= 3;
					if ((this.ButtonState & (ButtonState.Checked | ButtonState.Pushed)) != ButtonState.Normal)
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions = CheckBoxPopupAdapter.PaintPopupLayout(g, true, size.Width, rectangle7, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
						layoutOptions.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData = layoutOptions.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData.checkBounds, colorData.windowText, colorData.buttonFace, true, colorData);
							CheckBoxBaseAdapter.DrawPopupBorder(g, layoutData.checkBounds, colorData);
							CheckBoxBaseAdapter.DrawCheckOnly(size.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData, colorData, colorData.windowText, colorData.buttonFace, true);
						}
						rectangle4 = layoutData.checkBounds;
					}
					else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewCheckBoxCell.mouseInContentBounds)
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions2 = CheckBoxPopupAdapter.PaintPopupLayout(g, true, size.Width, rectangle7, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
						layoutOptions2.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData2 = layoutOptions2.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData2 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData2.checkBounds, colorData2.windowText, colorData2.options.highContrast ? colorData2.buttonFace : colorData2.highlight, true, colorData2);
							CheckBoxBaseAdapter.DrawPopupBorder(g, layoutData2.checkBounds, colorData2);
							CheckBoxBaseAdapter.DrawCheckOnly(size.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData2, colorData2, colorData2.windowText, colorData2.highlight, true);
						}
						rectangle4 = layoutData2.checkBounds;
					}
					else
					{
						ButtonBaseAdapter.LayoutOptions layoutOptions3 = CheckBoxPopupAdapter.PaintPopupLayout(g, false, size.Width, rectangle7, Padding.Empty, false, cellStyle.Font, string.Empty, base.DataGridView.Enabled, DataGridViewUtilities.ComputeDrawingContentAlignmentForCellStyleAlignment(cellStyle.Alignment), base.DataGridView.RightToLeft);
						layoutOptions3.everettButtonCompat = false;
						ButtonBaseAdapter.LayoutData layoutData3 = layoutOptions3.Layout();
						if (paint && DataGridViewCell.PaintContentForeground(paintParts))
						{
							ButtonBaseAdapter.ColorData colorData3 = ButtonBaseAdapter.PaintPopupRender(g, cellStyle.ForeColor, cellStyle.BackColor, base.DataGridView.Enabled).Calculate();
							CheckBoxBaseAdapter.DrawCheckBackground(base.DataGridView.Enabled, checkState, g, layoutData3.checkBounds, colorData3.windowText, colorData3.options.highContrast ? colorData3.buttonFace : colorData3.highlight, true, colorData3);
							ButtonBaseAdapter.DrawFlatBorder(g, layoutData3.checkBounds, colorData3.buttonShadow);
							CheckBoxBaseAdapter.DrawCheckOnly(size.Width, checkState == CheckState.Checked || checkState == CheckState.Indeterminate, base.DataGridView.Enabled, checkState, g, layoutData3, colorData3, colorData3.windowText, colorData3.highlight, true);
						}
						rectangle4 = layoutData3.checkBounds;
					}
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
			if (paint && DataGridViewCell.PaintErrorIcon(paintParts) && flag3 && base.DataGridView.ShowCellErrors)
			{
				base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle3, errorText);
			}
			return rectangle4;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000B7748 File Offset: 0x000B6748
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (formattedValue != null)
			{
				if (formattedValue is bool)
				{
					if ((bool)formattedValue)
					{
						if (this.TrueValue != null)
						{
							return this.TrueValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return true;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Checked;
						}
					}
					else
					{
						if (this.FalseValue != null)
						{
							return this.FalseValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return false;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Unchecked;
						}
					}
				}
				else if (formattedValue is CheckState)
				{
					switch ((CheckState)formattedValue)
					{
					case CheckState.Unchecked:
						if (this.FalseValue != null)
						{
							return this.FalseValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return false;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Unchecked;
						}
						break;
					case CheckState.Checked:
						if (this.TrueValue != null)
						{
							return this.TrueValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
						{
							return true;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Checked;
						}
						break;
					case CheckState.Indeterminate:
						if (this.IndeterminateValue != null)
						{
							return this.IndeterminateValue;
						}
						if (this.ValueType != null && this.ValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultCheckStateType))
						{
							return CheckState.Indeterminate;
						}
						break;
					}
				}
			}
			return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000B7928 File Offset: 0x000B6928
		private bool SwitchFormattedValue()
		{
			if (this.FormattedValueType == null)
			{
				return false;
			}
			if (this.FormattedValueType.IsAssignableFrom(typeof(CheckState)))
			{
				if ((this.flags & 16) != 0)
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Indeterminate;
				}
				else if ((this.flags & 32) != 0)
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Unchecked;
				}
				else
				{
					((IDataGridViewEditingCell)this).EditingCellFormattedValue = CheckState.Checked;
				}
			}
			else if (this.FormattedValueType.IsAssignableFrom(DataGridViewCheckBoxCell.defaultBooleanType))
			{
				((IDataGridViewEditingCell)this).EditingCellFormattedValue = !(bool)((IDataGridViewEditingCell)this).GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			return true;
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000B79C8 File Offset: 0x000B69C8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewCheckBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000B7A26 File Offset: 0x000B6A26
		private void UpdateButtonState(ButtonState newButtonState, int rowIndex)
		{
			this.ButtonState = newButtonState;
			base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
		}

		// Token: 0x04001B00 RID: 6912
		private const byte DATAGRIDVIEWCHECKBOXCELL_threeState = 1;

		// Token: 0x04001B01 RID: 6913
		private const byte DATAGRIDVIEWCHECKBOXCELL_valueChanged = 2;

		// Token: 0x04001B02 RID: 6914
		private const byte DATAGRIDVIEWCHECKBOXCELL_checked = 16;

		// Token: 0x04001B03 RID: 6915
		private const byte DATAGRIDVIEWCHECKBOXCELL_indeterminate = 32;

		// Token: 0x04001B04 RID: 6916
		private const byte DATAGRIDVIEWCHECKBOXCELL_margin = 2;

		// Token: 0x04001B05 RID: 6917
		private static readonly DataGridViewContentAlignment anyLeft = (DataGridViewContentAlignment)273;

		// Token: 0x04001B06 RID: 6918
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04001B07 RID: 6919
		private static readonly DataGridViewContentAlignment anyCenter = (DataGridViewContentAlignment)546;

		// Token: 0x04001B08 RID: 6920
		private static readonly DataGridViewContentAlignment anyBottom = (DataGridViewContentAlignment)1792;

		// Token: 0x04001B09 RID: 6921
		private static readonly DataGridViewContentAlignment anyMiddle = (DataGridViewContentAlignment)112;

		// Token: 0x04001B0A RID: 6922
		private static readonly VisualStyleElement CheckBoxElement = VisualStyleElement.Button.CheckBox.UncheckedNormal;

		// Token: 0x04001B0B RID: 6923
		private static readonly int PropButtonCellState = PropertyStore.CreateKey();

		// Token: 0x04001B0C RID: 6924
		private static readonly int PropTrueValue = PropertyStore.CreateKey();

		// Token: 0x04001B0D RID: 6925
		private static readonly int PropFalseValue = PropertyStore.CreateKey();

		// Token: 0x04001B0E RID: 6926
		private static readonly int PropFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04001B0F RID: 6927
		private static readonly int PropIndeterminateValue = PropertyStore.CreateKey();

		// Token: 0x04001B10 RID: 6928
		private static Bitmap checkImage = null;

		// Token: 0x04001B11 RID: 6929
		private byte flags;

		// Token: 0x04001B12 RID: 6930
		private static bool mouseInContentBounds = false;

		// Token: 0x04001B13 RID: 6931
		private static Type defaultCheckStateType = typeof(CheckState);

		// Token: 0x04001B14 RID: 6932
		private static Type defaultBooleanType = typeof(bool);

		// Token: 0x04001B15 RID: 6933
		private static Type cellType = typeof(DataGridViewCheckBoxCell);

		// Token: 0x0200032B RID: 811
		private class DataGridViewCheckBoxCellRenderer
		{
			// Token: 0x06003410 RID: 13328 RVA: 0x000B7AF5 File Offset: 0x000B6AF5
			private DataGridViewCheckBoxCellRenderer()
			{
			}

			// Token: 0x1700095B RID: 2395
			// (get) Token: 0x06003411 RID: 13329 RVA: 0x000B7AFD File Offset: 0x000B6AFD
			public static VisualStyleRenderer CheckBoxRenderer
			{
				get
				{
					if (DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewCheckBoxCell.CheckBoxElement);
					}
					return DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x06003412 RID: 13330 RVA: 0x000B7B1A File Offset: 0x000B6B1A
			public static void DrawCheckBox(Graphics g, Rectangle bounds, int state)
			{
				DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.CheckBoxRenderer.SetParameters(DataGridViewCheckBoxCell.CheckBoxElement.ClassName, DataGridViewCheckBoxCell.CheckBoxElement.Part, state);
				DataGridViewCheckBoxCell.DataGridViewCheckBoxCellRenderer.CheckBoxRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x04001B16 RID: 6934
			private static VisualStyleRenderer visualStyleRenderer;
		}

		// Token: 0x0200032C RID: 812
		protected class DataGridViewCheckBoxCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x06003413 RID: 13331 RVA: 0x000B7B52 File Offset: 0x000B6B52
			public DataGridViewCheckBoxCellAccessibleObject(DataGridViewCell owner)
				: base(owner)
			{
			}

			// Token: 0x1700095C RID: 2396
			// (get) Token: 0x06003414 RID: 13332 RVA: 0x000B7B5C File Offset: 0x000B6B5C
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.ReadOnly)
					{
						return string.Empty;
					}
					bool flag = true;
					object formattedValue = base.Owner.FormattedValue;
					if (formattedValue is CheckState)
					{
						flag = (CheckState)formattedValue == CheckState.Unchecked;
					}
					else if (formattedValue is bool)
					{
						flag = !(bool)formattedValue;
					}
					if (flag)
					{
						return SR.GetString("DataGridView_AccCheckBoxCellDefaultActionCheck");
					}
					return SR.GetString("DataGridView_AccCheckBoxCellDefaultActionUncheck");
				}
			}

			// Token: 0x06003415 RID: 13333 RVA: 0x000B7BC8 File Offset: 0x000B6BC8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)base.Owner;
				DataGridView dataGridView = dataGridViewCheckBoxCell.DataGridView;
				if (dataGridView != null && dataGridViewCheckBoxCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (!dataGridViewCheckBoxCell.ReadOnly && dataGridViewCheckBoxCell.OwningColumn != null && dataGridViewCheckBoxCell.OwningRow != null)
				{
					dataGridView.CurrentCell = dataGridViewCheckBoxCell;
					bool flag = false;
					if (!dataGridView.IsCurrentCellInEditMode)
					{
						flag = true;
						dataGridView.BeginEdit(false);
					}
					if (dataGridView.IsCurrentCellInEditMode)
					{
						if (dataGridViewCheckBoxCell.SwitchFormattedValue())
						{
							dataGridViewCheckBoxCell.NotifyDataGridViewOfValueChange();
							dataGridView.InvalidateCell(dataGridViewCheckBoxCell.ColumnIndex, dataGridViewCheckBoxCell.RowIndex);
							int rowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, dataGridViewCheckBoxCell.RowIndex);
							int num = dataGridView.Columns.ColumnIndexToActualDisplayIndex(dataGridViewCheckBoxCell.ColumnIndex, DataGridViewElementStates.Visible);
							bool columnHeadersVisible = dataGridView.ColumnHeadersVisible;
							bool rowHeadersVisible = dataGridView.RowHeadersVisible;
							int num2 = rowCount + (columnHeadersVisible ? 1 : 0) + 1;
							int num3 = num + (rowHeadersVisible ? 1 : 0);
							Control.ControlAccessibleObject controlAccessibleObject = (Control.ControlAccessibleObject)dataGridView.AccessibilityObject;
							controlAccessibleObject.NotifyClients(AccessibleEvents.DefaultActionChange, num2, num3);
						}
						if (flag)
						{
							dataGridView.EndEdit();
						}
					}
				}
			}

			// Token: 0x06003416 RID: 13334 RVA: 0x000B7CEA File Offset: 0x000B6CEA
			public override int GetChildCount()
			{
				return 0;
			}
		}
	}
}
