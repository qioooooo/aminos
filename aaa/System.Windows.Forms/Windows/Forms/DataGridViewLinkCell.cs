using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000379 RID: 889
	public class DataGridViewLinkCell : DataGridViewCell
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x000C25BD File Offset: 0x000C15BD
		// (set) Token: 0x06003669 RID: 13929 RVA: 0x000C25EC File Offset: 0x000C15EC
		public Color ActiveLinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor);
				}
				return LinkUtilities.IEActiveLinkColor;
			}
			set
			{
				if (!value.Equals(this.ActiveLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor, value);
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

		// Token: 0x170009F1 RID: 2545
		// (set) Token: 0x0600366A RID: 13930 RVA: 0x000C2658 File Offset: 0x000C1658
		internal Color ActiveLinkColorInternal
		{
			set
			{
				if (!value.Equals(this.ActiveLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor, value);
				}
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000C268C File Offset: 0x000C168C
		private bool ShouldSerializeActiveLinkColor()
		{
			return !this.ActiveLinkColor.Equals(LinkUtilities.IEActiveLinkColor);
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x000C26BA File Offset: 0x000C16BA
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x000C26BD File Offset: 0x000C16BD
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewLinkCell.defaultFormattedValueType;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000C26C4 File Offset: 0x000C16C4
		// (set) Token: 0x0600366F RID: 13935 RVA: 0x000C26EC File Offset: 0x000C16EC
		[DefaultValue(LinkBehavior.SystemDefault)]
		public LinkBehavior LinkBehavior
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, out flag);
				if (flag)
				{
					return (LinkBehavior)integer;
				}
				return LinkBehavior.SystemDefault;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LinkBehavior));
				}
				if (value != this.LinkBehavior)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, (int)value);
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

		// Token: 0x170009F5 RID: 2549
		// (set) Token: 0x06003670 RID: 13936 RVA: 0x000C2768 File Offset: 0x000C1768
		internal LinkBehavior LinkBehaviorInternal
		{
			set
			{
				if (value != this.LinkBehavior)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, (int)value);
				}
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x000C2784 File Offset: 0x000C1784
		// (set) Token: 0x06003672 RID: 13938 RVA: 0x000C27B4 File Offset: 0x000C17B4
		public Color LinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellLinkColor);
				}
				return LinkUtilities.IELinkColor;
			}
			set
			{
				if (!value.Equals(this.LinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellLinkColor, value);
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

		// Token: 0x170009F7 RID: 2551
		// (set) Token: 0x06003673 RID: 13939 RVA: 0x000C2820 File Offset: 0x000C1820
		internal Color LinkColorInternal
		{
			set
			{
				if (!value.Equals(this.LinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellLinkColor, value);
				}
			}
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000C2854 File Offset: 0x000C1854
		private bool ShouldSerializeLinkColor()
		{
			return !this.LinkColor.Equals(LinkUtilities.IELinkColor);
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x000C2884 File Offset: 0x000C1884
		// (set) Token: 0x06003676 RID: 13942 RVA: 0x000C28AA File Offset: 0x000C18AA
		private LinkState LinkState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellLinkState, out flag);
				if (flag)
				{
					return (LinkState)integer;
				}
				return LinkState.Normal;
			}
			set
			{
				if (this.LinkState != value)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkState, (int)value);
				}
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x000C28C6 File Offset: 0x000C18C6
		// (set) Token: 0x06003678 RID: 13944 RVA: 0x000C28D8 File Offset: 0x000C18D8
		public bool LinkVisited
		{
			get
			{
				return this.linkVisitedSet && this.linkVisited;
			}
			set
			{
				this.linkVisitedSet = true;
				if (value != this.LinkVisited)
				{
					this.linkVisited = value;
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

		// Token: 0x06003679 RID: 13945 RVA: 0x000C292C File Offset: 0x000C192C
		private bool ShouldSerializeLinkVisited()
		{
			return this.linkVisitedSet = true;
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x000C2944 File Offset: 0x000C1944
		// (set) Token: 0x0600367B RID: 13947 RVA: 0x000C2970 File Offset: 0x000C1970
		[DefaultValue(true)]
		public bool TrackVisitedState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, out flag);
				return !flag || integer != 0;
			}
			set
			{
				if (value != this.TrackVisitedState)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, value ? 1 : 0);
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

		// Token: 0x170009FB RID: 2555
		// (set) Token: 0x0600367C RID: 13948 RVA: 0x000C29CC File Offset: 0x000C19CC
		internal bool TrackVisitedStateInternal
		{
			set
			{
				if (value != this.TrackVisitedState)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, value ? 1 : 0);
				}
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x000C29F0 File Offset: 0x000C19F0
		// (set) Token: 0x0600367E RID: 13950 RVA: 0x000C2A1B File Offset: 0x000C1A1B
		[DefaultValue(false)]
		public bool UseColumnTextForLinkValue
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.UseColumnTextForLinkValue)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, value ? 1 : 0);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x170009FD RID: 2557
		// (set) Token: 0x0600367F RID: 13951 RVA: 0x000C2A43 File Offset: 0x000C1A43
		internal bool UseColumnTextForLinkValueInternal
		{
			set
			{
				if (value != this.UseColumnTextForLinkValue)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, value ? 1 : 0);
				}
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x000C2A65 File Offset: 0x000C1A65
		// (set) Token: 0x06003681 RID: 13953 RVA: 0x000C2A94 File Offset: 0x000C1A94
		public Color VisitedLinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor);
				}
				return LinkUtilities.IEVisitedLinkColor;
			}
			set
			{
				if (!value.Equals(this.VisitedLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor, value);
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

		// Token: 0x170009FF RID: 2559
		// (set) Token: 0x06003682 RID: 13954 RVA: 0x000C2B00 File Offset: 0x000C1B00
		internal Color VisitedLinkColorInternal
		{
			set
			{
				if (!value.Equals(this.VisitedLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor, value);
				}
			}
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000C2B34 File Offset: 0x000C1B34
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.VisitedLinkColor.Equals(LinkUtilities.IEVisitedLinkColor);
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06003684 RID: 13956 RVA: 0x000C2B64 File Offset: 0x000C1B64
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewLinkCell.defaultValueType;
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000C2B84 File Offset: 0x000C1B84
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewLinkCell dataGridViewLinkCell;
			if (type == DataGridViewLinkCell.cellType)
			{
				dataGridViewLinkCell = new DataGridViewLinkCell();
			}
			else
			{
				dataGridViewLinkCell = (DataGridViewLinkCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewLinkCell);
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor))
			{
				dataGridViewLinkCell.ActiveLinkColorInternal = this.ActiveLinkColor;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue))
			{
				dataGridViewLinkCell.UseColumnTextForLinkValueInternal = this.UseColumnTextForLinkValue;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior))
			{
				dataGridViewLinkCell.LinkBehaviorInternal = this.LinkBehavior;
			}
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellLinkColor))
			{
				dataGridViewLinkCell.LinkColorInternal = this.LinkColor;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState))
			{
				dataGridViewLinkCell.TrackVisitedStateInternal = this.TrackVisitedState;
			}
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor))
			{
				dataGridViewLinkCell.VisitedLinkColorInternal = this.VisitedLinkColor;
			}
			if (this.linkVisitedSet)
			{
				dataGridViewLinkCell.LinkVisited = this.LinkVisited;
			}
			return dataGridViewLinkCell;
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000C2C84 File Offset: 0x000C1C84
		private bool LinkBoundsContainPoint(int x, int y, int rowIndex)
		{
			return base.GetContentBounds(rowIndex).Contains(x, y);
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x000C2CA2 File Offset: 0x000C1CA2
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject(this);
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x000C2CAC File Offset: 0x000C1CAC
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

		// Token: 0x06003689 RID: 13961 RVA: 0x000C2D20 File Offset: 0x000C1D20
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

		// Token: 0x0600368A RID: 13962 RVA: 0x000C2DB4 File Offset: 0x000C1DB4
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
					size = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Width - num - 1 - 2), textFormatFlags));
					break;
				case DataGridViewFreeDimension.Width:
				{
					int num3 = constraintSize.Height - num2 - 1 - 1;
					if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
					{
						num3--;
					}
					size = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, num3), textFormatFlags), 0);
					break;
				}
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
				size.Width += 3 + num;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Width = Math.Max(size.Width, num + 8 + 12);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				size.Height += 2 + num2;
				if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					size.Height++;
				}
				if (base.DataGridView.ShowCellErrors)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000C3038 File Offset: 0x000C2038
		protected override object GetValue(int rowIndex)
		{
			if (this.UseColumnTextForLinkValue && base.DataGridView != null && base.DataGridView.NewRowIndex != rowIndex && base.OwningColumn != null && base.OwningColumn is DataGridViewLinkColumn)
			{
				return ((DataGridViewLinkColumn)base.OwningColumn).Text;
			}
			return base.GetValue(rowIndex);
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000C3090 File Offset: 0x000C2090
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode != Keys.Space || e.Alt || e.Control || e.Shift || (this.TrackVisitedState && !this.LinkVisited);
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000C30C9 File Offset: 0x000C20C9
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex);
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x000C30E3 File Offset: 0x000C20E3
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return this.LinkState != LinkState.Normal;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000C30F1 File Offset: 0x000C20F1
		protected override bool MouseMoveUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				if ((this.LinkState & LinkState.Hover) == LinkState.Normal)
				{
					return true;
				}
			}
			else if ((this.LinkState & LinkState.Hover) != LinkState.Normal)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000C3126 File Offset: 0x000C2126
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return this.TrackVisitedState && this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex);
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000C314C File Offset: 0x000C214C
		protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift)
			{
				base.RaiseCellClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
				if (base.DataGridView != null && base.ColumnIndex < base.DataGridView.Columns.Count && rowIndex < base.DataGridView.Rows.Count)
				{
					base.RaiseCellContentClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					if (this.TrackVisitedState)
					{
						this.LinkVisited = true;
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000C31F4 File Offset: 0x000C21F4
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				this.LinkState |= LinkState.Active;
				base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
			}
			base.OnMouseDown(e);
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000C3250 File Offset: 0x000C2250
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewLinkCell.dataGridViewCursor != null)
			{
				base.DataGridView.Cursor = DataGridViewLinkCell.dataGridViewCursor;
				DataGridViewLinkCell.dataGridViewCursor = null;
			}
			if (this.LinkState != LinkState.Normal)
			{
				this.LinkState = LinkState.Normal;
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseLeave(rowIndex);
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000C32B4 File Offset: 0x000C22B4
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				if ((this.LinkState & LinkState.Hover) == LinkState.Normal)
				{
					this.LinkState |= LinkState.Hover;
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (DataGridViewLinkCell.dataGridViewCursor == null)
				{
					DataGridViewLinkCell.dataGridViewCursor = base.DataGridView.UserSetCursor;
				}
				if (base.DataGridView.Cursor != Cursors.Hand)
				{
					base.DataGridView.Cursor = Cursors.Hand;
				}
			}
			else if ((this.LinkState & LinkState.Hover) != LinkState.Normal)
			{
				this.LinkState &= (LinkState)(-2);
				base.DataGridView.Cursor = DataGridViewLinkCell.dataGridViewCursor;
				base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000C33A0 File Offset: 0x000C23A0
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex) && this.TrackVisitedState)
			{
				this.LinkVisited = true;
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000C33D4 File Offset: 0x000C23D4
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000C340C File Offset: 0x000C240C
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle3 = cellBounds;
			rectangle3.Offset(rectangle2.X, rectangle2.Y);
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Height -= rectangle2.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag2 = (cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag2) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
			if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
			{
				g.FillRectangle(cachedBrush, rectangle3);
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
			Rectangle rectangle4 = rectangle3;
			string text = formattedValue as string;
			if (text != null && (paint || computeContentBounds))
			{
				rectangle3.Offset(1, 1);
				rectangle3.Width -= 3;
				rectangle3.Height -= 2;
				if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					rectangle3.Height--;
				}
				Font font = null;
				Font font2 = null;
				LinkUtilities.EnsureLinkFonts(cellStyle.Font, this.LinkBehavior, ref font, ref font2);
				TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
				if (paint)
				{
					if (rectangle3.Width > 0 && rectangle3.Height > 0)
					{
						if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts))
						{
							Rectangle textBounds = DataGridViewUtilities.GetTextBounds(rectangle3, text, textFormatFlags, cellStyle, (this.LinkState == LinkState.Hover) ? font2 : font);
							if ((cellStyle.Alignment & DataGridViewLinkCell.anyLeft) != DataGridViewContentAlignment.NotSet)
							{
								textBounds.X--;
								textBounds.Width++;
							}
							else if ((cellStyle.Alignment & DataGridViewLinkCell.anyRight) != DataGridViewContentAlignment.NotSet)
							{
								textBounds.X++;
								textBounds.Width++;
							}
							textBounds.Height += 2;
							ControlPaint.DrawFocusRectangle(g, textBounds, Color.Empty, cachedBrush.Color);
						}
						Color color;
						if ((this.LinkState & LinkState.Active) == LinkState.Active)
						{
							color = this.ActiveLinkColor;
						}
						else if (this.LinkVisited)
						{
							color = this.VisitedLinkColor;
						}
						else
						{
							color = this.LinkColor;
						}
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
							{
								textFormatFlags |= TextFormatFlags.EndEllipsis;
							}
							TextRenderer.DrawText(g, text, (this.LinkState == LinkState.Hover) ? font2 : font, rectangle3, color, textFormatFlags);
						}
					}
					else if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts) && rectangle4.Width > 0 && rectangle4.Height > 0)
					{
						ControlPaint.DrawFocusRectangle(g, rectangle4, Color.Empty, cachedBrush.Color);
					}
				}
				else
				{
					rectangle = DataGridViewUtilities.GetTextBounds(rectangle3, text, textFormatFlags, cellStyle, (this.LinkState == LinkState.Hover) ? font2 : font);
				}
				font.Dispose();
				font2.Dispose();
			}
			else if (paint || computeContentBounds)
			{
				if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts) && paint && rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					ControlPaint.DrawFocusRectangle(g, rectangle3, Color.Empty, cachedBrush.Color);
				}
			}
			else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
			{
				rectangle = base.ComputeErrorIconBounds(rectangle4);
			}
			if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
			{
				base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle4, errorText);
			}
			return rectangle;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000C38D4 File Offset: 0x000C28D4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewLinkCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04001BD9 RID: 7129
		private const byte DATAGRIDVIEWLINKCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04001BDA RID: 7130
		private const byte DATAGRIDVIEWLINKCELL_horizontalTextMarginRight = 2;

		// Token: 0x04001BDB RID: 7131
		private const byte DATAGRIDVIEWLINKCELL_verticalTextMarginTop = 1;

		// Token: 0x04001BDC RID: 7132
		private const byte DATAGRIDVIEWLINKCELL_verticalTextMarginBottom = 1;

		// Token: 0x04001BDD RID: 7133
		private static readonly DataGridViewContentAlignment anyLeft = (DataGridViewContentAlignment)273;

		// Token: 0x04001BDE RID: 7134
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04001BDF RID: 7135
		private static readonly DataGridViewContentAlignment anyBottom = (DataGridViewContentAlignment)1792;

		// Token: 0x04001BE0 RID: 7136
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04001BE1 RID: 7137
		private static Type defaultValueType = typeof(object);

		// Token: 0x04001BE2 RID: 7138
		private static Type cellType = typeof(DataGridViewLinkCell);

		// Token: 0x04001BE3 RID: 7139
		private static readonly int PropLinkCellActiveLinkColor = PropertyStore.CreateKey();

		// Token: 0x04001BE4 RID: 7140
		private static readonly int PropLinkCellLinkBehavior = PropertyStore.CreateKey();

		// Token: 0x04001BE5 RID: 7141
		private static readonly int PropLinkCellLinkColor = PropertyStore.CreateKey();

		// Token: 0x04001BE6 RID: 7142
		private static readonly int PropLinkCellLinkState = PropertyStore.CreateKey();

		// Token: 0x04001BE7 RID: 7143
		private static readonly int PropLinkCellTrackVisitedState = PropertyStore.CreateKey();

		// Token: 0x04001BE8 RID: 7144
		private static readonly int PropLinkCellUseColumnTextForLinkValue = PropertyStore.CreateKey();

		// Token: 0x04001BE9 RID: 7145
		private static readonly int PropLinkCellVisitedLinkColor = PropertyStore.CreateKey();

		// Token: 0x04001BEA RID: 7146
		private bool linkVisited;

		// Token: 0x04001BEB RID: 7147
		private bool linkVisitedSet;

		// Token: 0x04001BEC RID: 7148
		private static Cursor dataGridViewCursor = null;

		// Token: 0x0200037A RID: 890
		protected class DataGridViewLinkCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			// Token: 0x0600369A RID: 13978 RVA: 0x000C39D8 File Offset: 0x000C29D8
			public DataGridViewLinkCellAccessibleObject(DataGridViewCell owner)
				: base(owner)
			{
			}

			// Token: 0x17000A01 RID: 2561
			// (get) Token: 0x0600369B RID: 13979 RVA: 0x000C39E1 File Offset: 0x000C29E1
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("DataGridView_AccLinkCellDefaultAction");
				}
			}

			// Token: 0x0600369C RID: 13980 RVA: 0x000C39F0 File Offset: 0x000C29F0
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewLinkCell dataGridViewLinkCell = (DataGridViewLinkCell)base.Owner;
				DataGridView dataGridView = dataGridViewLinkCell.DataGridView;
				if (dataGridView != null && dataGridViewLinkCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (dataGridViewLinkCell.OwningColumn != null && dataGridViewLinkCell.OwningRow != null)
				{
					dataGridView.OnCellContentClickInternal(new DataGridViewCellEventArgs(dataGridViewLinkCell.ColumnIndex, dataGridViewLinkCell.RowIndex));
				}
			}

			// Token: 0x0600369D RID: 13981 RVA: 0x000C3A53 File Offset: 0x000C2A53
			public override int GetChildCount()
			{
				return 0;
			}
		}
	}
}
