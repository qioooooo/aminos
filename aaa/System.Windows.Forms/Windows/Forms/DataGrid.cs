using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020002C2 RID: 706
	[DefaultProperty("DataSource")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[DefaultEvent("Navigate")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[Designer("System.Windows.Forms.Design.DataGridDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataGrid : Control, ISupportInitialize, IDataGridEditingService
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x00060284 File Offset: 0x0005F284
		public DataGrid()
		{
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.Opaque, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			base.SetStyle(ControlStyles.UserMouse, true);
			this.gridState = new BitVector32(272423);
			this.dataGridTables = new GridTableStylesCollection(this);
			this.layout = this.CreateInitialLayoutState();
			this.parentRows = new DataGridParentRows(this);
			this.horizScrollBar.Top = base.ClientRectangle.Height - this.horizScrollBar.Height;
			this.horizScrollBar.Left = 0;
			this.horizScrollBar.Visible = false;
			this.horizScrollBar.Scroll += this.GridHScrolled;
			base.Controls.Add(this.horizScrollBar);
			this.vertScrollBar.Top = 0;
			this.vertScrollBar.Left = base.ClientRectangle.Width - this.vertScrollBar.Width;
			this.vertScrollBar.Visible = false;
			this.vertScrollBar.Scroll += this.GridVScrolled;
			base.Controls.Add(this.vertScrollBar);
			this.BackColor = DataGrid.DefaultBackBrush.Color;
			this.ForeColor = DataGrid.DefaultForeBrush.Color;
			this.borderStyle = BorderStyle.Fixed3D;
			this.currentChangedHandler = new EventHandler(this.DataSource_RowChanged);
			this.positionChangedHandler = new EventHandler(this.DataSource_PositionChanged);
			this.itemChangedHandler = new ItemChangedEventHandler(this.DataSource_ItemChanged);
			this.metaDataChangedHandler = new EventHandler(this.DataSource_MetaDataChanged);
			this.dataGridTableStylesCollectionChanged = new CollectionChangeEventHandler(this.TableStylesCollectionChanged);
			this.dataGridTables.CollectionChanged += this.dataGridTableStylesCollectionChanged;
			this.SetDataGridTable(this.defaultTableStyle, true);
			this.backButtonHandler = new EventHandler(this.OnBackButtonClicked);
			this.downButtonHandler = new EventHandler(this.OnShowParentDetailsButtonClicked);
			this.caption = new DataGridCaption(this);
			this.caption.BackwardClicked += this.backButtonHandler;
			this.caption.DownClicked += this.downButtonHandler;
			this.RecalculateFonts();
			base.Size = new Size(130, 80);
			base.Invalidate();
			base.PerformLayout();
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x00060606 File Offset: 0x0005F606
		// (set) Token: 0x06002705 RID: 9989 RVA: 0x00060614 File Offset: 0x0005F614
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("DataGridAllowSortingDescr")]
		public bool AllowSorting
		{
			get
			{
				return this.gridState[1];
			}
			set
			{
				if (this.AllowSorting != value)
				{
					this.gridState[1] = value;
					if (!value && this.listManager != null)
					{
						IList list = this.listManager.List;
						if (list is IBindingList)
						{
							((IBindingList)list).RemoveSort();
						}
					}
				}
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002706 RID: 9990 RVA: 0x00060661 File Offset: 0x0005F661
		// (set) Token: 0x06002707 RID: 9991 RVA: 0x00060670 File Offset: 0x0005F670
		[SRDescription("DataGridAlternatingBackColorDescr")]
		[SRCategory("CatColors")]
		public Color AlternatingBackColor
		{
			get
			{
				return this.alternatingBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "AlternatingBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentAlternatingBackColorNotAllowed"));
				}
				if (!this.alternatingBackBrush.Color.Equals(value))
				{
					this.alternatingBackBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000606F1 File Offset: 0x0005F6F1
		public void ResetAlternatingBackColor()
		{
			if (this.ShouldSerializeAlternatingBackColor())
			{
				this.AlternatingBackColor = DataGrid.DefaultAlternatingBackBrush.Color;
				this.InvalidateInside();
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x00060711 File Offset: 0x0005F711
		protected virtual bool ShouldSerializeAlternatingBackColor()
		{
			return !this.AlternatingBackBrush.Equals(DataGrid.DefaultAlternatingBackBrush);
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x0600270A RID: 9994 RVA: 0x00060726 File Offset: 0x0005F726
		internal Brush AlternatingBackBrush
		{
			get
			{
				return this.alternatingBackBrush;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x0006072E File Offset: 0x0005F72E
		// (set) Token: 0x0600270C RID: 9996 RVA: 0x00060736 File Offset: 0x0005F736
		[SRDescription("ControlBackColorDescr")]
		[SRCategory("CatColors")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentBackColorNotAllowed"));
				}
				base.BackColor = value;
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x00060758 File Offset: 0x0005F758
		public override void ResetBackColor()
		{
			if (!this.BackColor.Equals(DataGrid.DefaultBackBrush.Color))
			{
				this.BackColor = DataGrid.DefaultBackBrush.Color;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x0006079A File Offset: 0x0005F79A
		// (set) Token: 0x0600270F RID: 9999 RVA: 0x000607A2 File Offset: 0x0005F7A2
		[SRDescription("ControlForeColorDescr")]
		[SRCategory("CatColors")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000607AC File Offset: 0x0005F7AC
		public override void ResetForeColor()
		{
			if (!this.ForeColor.Equals(DataGrid.DefaultForeBrush.Color))
			{
				this.ForeColor = DataGrid.DefaultForeBrush.Color;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x000607EE File Offset: 0x0005F7EE
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000607F6 File Offset: 0x0005F7F6
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x000607FE File Offset: 0x0005F7FE
		// (set) Token: 0x06002714 RID: 10004 RVA: 0x00060808 File Offset: 0x0005F808
		[SRDescription("DataGridBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.PerformLayout();
					base.Invalidate();
					this.OnBorderStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06002715 RID: 10005 RVA: 0x00060862 File Offset: 0x0005F862
		// (remove) Token: 0x06002716 RID: 10006 RVA: 0x00060875 File Offset: 0x0005F875
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BORDERSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BORDERSTYLECHANGED, value);
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x00060888 File Offset: 0x0005F888
		private int BorderWidth
		{
			get
			{
				if (this.BorderStyle == BorderStyle.Fixed3D)
				{
					return SystemInformation.Border3DSize.Width;
				}
				if (this.BorderStyle == BorderStyle.FixedSingle)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x000608B8 File Offset: 0x0005F8B8
		protected override Size DefaultSize
		{
			get
			{
				return new Size(130, 80);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x000608C6 File Offset: 0x0005F8C6
		private static SolidBrush DefaultSelectionBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000608D2 File Offset: 0x0005F8D2
		private static SolidBrush DefaultSelectionForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600271B RID: 10011 RVA: 0x000608DE File Offset: 0x0005F8DE
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x000608EA File Offset: 0x0005F8EA
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x000608F6 File Offset: 0x0005F8F6
		private static SolidBrush DefaultBackgroundBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.AppWorkspace;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x00060902 File Offset: 0x0005F902
		internal static SolidBrush DefaultParentRowsForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x0006090E File Offset: 0x0005F90E
		internal static SolidBrush DefaultParentRowsBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x0006091A File Offset: 0x0005F91A
		internal static SolidBrush DefaultAlternatingBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x00060926 File Offset: 0x0005F926
		private static SolidBrush DefaultGridLineBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x00060932 File Offset: 0x0005F932
		private static SolidBrush DefaultHeaderBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x0006093E File Offset: 0x0005F93E
		private static SolidBrush DefaultHeaderForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ControlText;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x0006094A File Offset: 0x0005F94A
		private static Pen DefaultHeaderForePen
		{
			get
			{
				return new Pen(SystemColors.ControlText);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x00060956 File Offset: 0x0005F956
		private static SolidBrush DefaultLinkBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.HotTrack;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x00060962 File Offset: 0x0005F962
		// (set) Token: 0x06002727 RID: 10023 RVA: 0x00060974 File Offset: 0x0005F974
		private bool ListHasErrors
		{
			get
			{
				return this.gridState[128];
			}
			set
			{
				if (this.ListHasErrors != value)
				{
					this.gridState[128] = value;
					this.ComputeMinimumRowHeaderWidth();
					if (!this.layout.RowHeadersVisible)
					{
						return;
					}
					if (value)
					{
						if (this.myGridTable.IsDefault)
						{
							this.RowHeaderWidth += 15;
							return;
						}
						this.myGridTable.RowHeaderWidth += 15;
						return;
					}
					else
					{
						if (this.myGridTable.IsDefault)
						{
							this.RowHeaderWidth -= 15;
							return;
						}
						this.myGridTable.RowHeaderWidth -= 15;
					}
				}
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x00060A18 File Offset: 0x0005FA18
		private bool Bound
		{
			get
			{
				return this.listManager != null && this.myGridTable != null;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002729 RID: 10025 RVA: 0x00060A30 File Offset: 0x0005FA30
		internal DataGridCaption Caption
		{
			get
			{
				return this.caption;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x00060A38 File Offset: 0x0005FA38
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x00060A45 File Offset: 0x0005FA45
		[SRDescription("DataGridCaptionBackColorDescr")]
		[SRCategory("CatColors")]
		public Color CaptionBackColor
		{
			get
			{
				return this.Caption.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentCaptionBackColorNotAllowed"));
				}
				this.Caption.BackColor = value;
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x00060A6B File Offset: 0x0005FA6B
		private void ResetCaptionBackColor()
		{
			this.Caption.ResetBackColor();
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x00060A78 File Offset: 0x0005FA78
		protected virtual bool ShouldSerializeCaptionBackColor()
		{
			return this.Caption.ShouldSerializeBackColor();
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x00060A85 File Offset: 0x0005FA85
		// (set) Token: 0x0600272F RID: 10031 RVA: 0x00060A92 File Offset: 0x0005FA92
		[SRCategory("CatColors")]
		[SRDescription("DataGridCaptionForeColorDescr")]
		public Color CaptionForeColor
		{
			get
			{
				return this.Caption.ForeColor;
			}
			set
			{
				this.Caption.ForeColor = value;
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x00060AA0 File Offset: 0x0005FAA0
		private void ResetCaptionForeColor()
		{
			this.Caption.ResetForeColor();
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x00060AAD File Offset: 0x0005FAAD
		protected virtual bool ShouldSerializeCaptionForeColor()
		{
			return this.Caption.ShouldSerializeForeColor();
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x00060ABA File Offset: 0x0005FABA
		// (set) Token: 0x06002733 RID: 10035 RVA: 0x00060AC7 File Offset: 0x0005FAC7
		[Localizable(true)]
		[SRDescription("DataGridCaptionFontDescr")]
		[AmbientValue(null)]
		[SRCategory("CatAppearance")]
		public Font CaptionFont
		{
			get
			{
				return this.Caption.Font;
			}
			set
			{
				this.Caption.Font = value;
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00060AD5 File Offset: 0x0005FAD5
		private bool ShouldSerializeCaptionFont()
		{
			return this.Caption.ShouldSerializeFont();
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x00060AE2 File Offset: 0x0005FAE2
		private void ResetCaptionFont()
		{
			this.Caption.ResetFont();
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x00060AEF File Offset: 0x0005FAEF
		// (set) Token: 0x06002737 RID: 10039 RVA: 0x00060AFC File Offset: 0x0005FAFC
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("DataGridCaptionTextDescr")]
		public string CaptionText
		{
			get
			{
				return this.Caption.Text;
			}
			set
			{
				this.Caption.Text = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00060B0A File Offset: 0x0005FB0A
		// (set) Token: 0x06002739 RID: 10041 RVA: 0x00060B17 File Offset: 0x0005FB17
		[SRDescription("DataGridCaptionVisibleDescr")]
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		public bool CaptionVisible
		{
			get
			{
				return this.layout.CaptionVisible;
			}
			set
			{
				if (this.layout.CaptionVisible != value)
				{
					this.layout.CaptionVisible = value;
					base.PerformLayout();
					base.Invalidate();
					this.OnCaptionVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x0600273A RID: 10042 RVA: 0x00060B4A File Offset: 0x0005FB4A
		// (remove) Token: 0x0600273B RID: 10043 RVA: 0x00060B5D File Offset: 0x0005FB5D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnCaptionVisibleChangedDescr")]
		public event EventHandler CaptionVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_CAPTIONVISIBLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_CAPTIONVISIBLECHANGED, value);
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x00060B70 File Offset: 0x0005FB70
		// (set) Token: 0x0600273D RID: 10045 RVA: 0x00060B84 File Offset: 0x0005FB84
		[SRDescription("DataGridCurrentCellDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataGridCell CurrentCell
		{
			get
			{
				return new DataGridCell(this.currentRow, this.currentCol);
			}
			set
			{
				if (this.layout.dirty)
				{
					throw new ArgumentException(SR.GetString("DataGridSettingCurrentCellNotGood"));
				}
				if (value.RowNumber == this.currentRow && value.ColumnNumber == this.currentCol)
				{
					return;
				}
				if (this.DataGridRowsLength == 0 || this.myGridTable.GridColumnStyles == null || this.myGridTable.GridColumnStyles.Count == 0)
				{
					return;
				}
				this.EnsureBound();
				int num = this.currentRow;
				int num2 = this.currentCol;
				bool flag = this.gridState[32768];
				bool flag2 = false;
				bool flag3 = false;
				int num3 = value.ColumnNumber;
				int num4 = value.RowNumber;
				string text = null;
				try
				{
					int count = this.myGridTable.GridColumnStyles.Count;
					if (num3 < 0)
					{
						num3 = 0;
					}
					if (num3 >= count)
					{
						num3 = count - 1;
					}
					int num5 = this.DataGridRowsLength;
					DataGridRow[] array = this.DataGridRows;
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num4 >= num5)
					{
						num4 = num5 - 1;
					}
					if (this.currentCol != num3)
					{
						flag2 = true;
						int position = this.ListManager.Position;
						int count2 = this.ListManager.List.Count;
						this.EndEdit();
						if (this.ListManager.Position != position || count2 != this.ListManager.List.Count)
						{
							this.RecreateDataGridRows();
							if (this.ListManager.List.Count > 0)
							{
								this.currentRow = this.ListManager.Position;
								this.Edit();
							}
							else
							{
								this.currentRow = -1;
							}
							return;
						}
						this.currentCol = num3;
						this.InvalidateRow(this.currentRow);
					}
					if (this.currentRow != num4)
					{
						flag2 = true;
						int position2 = this.ListManager.Position;
						int count3 = this.ListManager.List.Count;
						this.EndEdit();
						if (this.ListManager.Position != position2 || count3 != this.ListManager.List.Count)
						{
							this.RecreateDataGridRows();
							if (this.ListManager.List.Count > 0)
							{
								this.currentRow = this.ListManager.Position;
								this.Edit();
							}
							else
							{
								this.currentRow = -1;
							}
							return;
						}
						if (this.currentRow < num5)
						{
							array[this.currentRow].OnRowLeave();
						}
						array[num4].OnRowEnter();
						this.currentRow = num4;
						if (num < num5)
						{
							this.InvalidateRow(num);
						}
						this.InvalidateRow(this.currentRow);
						if (num != this.listManager.Position)
						{
							flag3 = true;
							if (this.gridState[32768])
							{
								this.AbortEdit();
							}
						}
						else if (this.gridState[1048576])
						{
							this.ListManager.PositionChanged -= this.positionChangedHandler;
							this.ListManager.CancelCurrentEdit();
							this.ListManager.Position = this.currentRow;
							this.ListManager.PositionChanged += this.positionChangedHandler;
							array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
							this.SetDataGridRows(array, this.DataGridRowsLength);
							this.gridState[1048576] = false;
						}
						else
						{
							this.ListManager.EndCurrentEdit();
							if (num5 != this.DataGridRowsLength)
							{
								this.currentRow = ((this.currentRow == num5 - 1) ? (this.DataGridRowsLength - 1) : this.currentRow);
							}
							if (this.currentRow == this.dataGridRowsLength - 1 && this.policy.AllowAdd)
							{
								this.AddNewRow();
							}
							else
							{
								this.ListManager.Position = this.currentRow;
							}
						}
					}
				}
				catch (Exception ex)
				{
					text = ex.Message;
				}
				if (text != null)
				{
					DialogResult dialogResult = RTLAwareMessageBox.Show(null, SR.GetString("DataGridPushedIncorrectValueIntoColumn", new object[] { text }), SR.GetString("DataGridErrorMessageBoxCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (dialogResult == DialogResult.Yes)
					{
						this.currentRow = num;
						this.currentCol = num2;
						this.InvalidateRowHeader(num4);
						this.InvalidateRowHeader(this.currentRow);
						if (flag)
						{
							this.Edit();
						}
					}
					else
					{
						if (this.currentRow == this.DataGridRowsLength - 1 && num == this.DataGridRowsLength - 2 && this.DataGridRows[this.currentRow] is DataGridAddNewRow)
						{
							num4 = num;
						}
						this.currentRow = num4;
						this.listManager.PositionChanged -= this.positionChangedHandler;
						this.listManager.CancelCurrentEdit();
						this.listManager.Position = num4;
						this.listManager.PositionChanged += this.positionChangedHandler;
						this.currentRow = num4;
						this.currentCol = num3;
						if (flag)
						{
							this.Edit();
						}
					}
				}
				if (flag2)
				{
					this.EnsureVisible(this.currentRow, this.currentCol);
					this.OnCurrentCellChanged(EventArgs.Empty);
					if (!flag3)
					{
						this.Edit();
					}
					base.AccessibilityNotifyClients(AccessibleEvents.Focus, this.CurrentCellAccIndex);
					base.AccessibilityNotifyClients(AccessibleEvents.Selection, this.CurrentCellAccIndex);
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600273E RID: 10046 RVA: 0x000610A0 File Offset: 0x000600A0
		internal int CurrentCellAccIndex
		{
			get
			{
				int num = 0;
				num++;
				num += this.myGridTable.GridColumnStyles.Count;
				num += this.DataGridRows.Length;
				if (this.horizScrollBar.Visible)
				{
					num++;
				}
				if (this.vertScrollBar.Visible)
				{
					num++;
				}
				return num + (this.currentRow * this.myGridTable.GridColumnStyles.Count + this.currentCol);
			}
		}

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x0600273F RID: 10047 RVA: 0x00061115 File Offset: 0x00060115
		// (remove) Token: 0x06002740 RID: 10048 RVA: 0x00061128 File Offset: 0x00060128
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnCurrentCellChangedDescr")]
		public event EventHandler CurrentCellChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_CURRENTCELLCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_CURRENTCELLCHANGED, value);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x0006113C File Offset: 0x0006013C
		// (set) Token: 0x06002742 RID: 10050 RVA: 0x00061157 File Offset: 0x00060157
		private int CurrentColumn
		{
			get
			{
				return this.CurrentCell.ColumnNumber;
			}
			set
			{
				this.CurrentCell = new DataGridCell(this.currentRow, value);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x0006116C File Offset: 0x0006016C
		// (set) Token: 0x06002744 RID: 10052 RVA: 0x00061187 File Offset: 0x00060187
		private int CurrentRow
		{
			get
			{
				return this.CurrentCell.RowNumber;
			}
			set
			{
				this.CurrentCell = new DataGridCell(value, this.currentCol);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x0006119B File Offset: 0x0006019B
		// (set) Token: 0x06002746 RID: 10054 RVA: 0x000611A8 File Offset: 0x000601A8
		[SRDescription("DataGridSelectionBackColorDescr")]
		[SRCategory("CatColors")]
		public Color SelectionBackColor
		{
			get
			{
				return this.selectionBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "SelectionBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentSelectionBackColorNotAllowed"));
				}
				if (!value.Equals(this.selectionBackBrush.Color))
				{
					this.selectionBackBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x00061227 File Offset: 0x00060227
		internal SolidBrush SelectionBackBrush
		{
			get
			{
				return this.selectionBackBrush;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x0006122F File Offset: 0x0006022F
		internal SolidBrush SelectionForeBrush
		{
			get
			{
				return this.selectionForeBrush;
			}
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00061237 File Offset: 0x00060237
		protected bool ShouldSerializeSelectionBackColor()
		{
			return !DataGrid.DefaultSelectionBackBrush.Equals(this.selectionBackBrush);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x0006124C File Offset: 0x0006024C
		public void ResetSelectionBackColor()
		{
			if (this.ShouldSerializeSelectionBackColor())
			{
				this.SelectionBackColor = DataGrid.DefaultSelectionBackBrush.Color;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x00061266 File Offset: 0x00060266
		// (set) Token: 0x0600274C RID: 10060 RVA: 0x00061274 File Offset: 0x00060274
		[SRCategory("CatColors")]
		[SRDescription("DataGridSelectionForeColorDescr")]
		public Color SelectionForeColor
		{
			get
			{
				return this.selectionForeBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "SelectionForeColor" }));
				}
				if (!value.Equals(this.selectionForeBrush.Color))
				{
					this.selectionForeBrush = new SolidBrush(value);
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000612DB File Offset: 0x000602DB
		protected virtual bool ShouldSerializeSelectionForeColor()
		{
			return !this.SelectionForeBrush.Equals(DataGrid.DefaultSelectionForeBrush);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000612F0 File Offset: 0x000602F0
		public void ResetSelectionForeColor()
		{
			if (this.ShouldSerializeSelectionForeColor())
			{
				this.SelectionForeColor = DataGrid.DefaultSelectionForeBrush.Color;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0006130C File Offset: 0x0006030C
		internal override bool ShouldSerializeForeColor()
		{
			return !DataGrid.DefaultForeBrush.Color.Equals(this.ForeColor);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x00061340 File Offset: 0x00060340
		internal override bool ShouldSerializeBackColor()
		{
			return !DataGrid.DefaultBackBrush.Color.Equals(this.BackColor);
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x00061373 File Offset: 0x00060373
		internal DataGridRow[] DataGridRows
		{
			get
			{
				if (this.dataGridRows == null)
				{
					this.CreateDataGridRows();
				}
				return this.dataGridRows;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x00061389 File Offset: 0x00060389
		internal DataGridToolTip ToolTipProvider
		{
			get
			{
				return this.toolTipProvider;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x00061391 File Offset: 0x00060391
		// (set) Token: 0x06002754 RID: 10068 RVA: 0x00061399 File Offset: 0x00060399
		internal int ToolTipId
		{
			get
			{
				return this.toolTipId;
			}
			set
			{
				this.toolTipId = value;
			}
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000613A4 File Offset: 0x000603A4
		private void ResetToolTip()
		{
			for (int i = 0; i < this.ToolTipId; i++)
			{
				this.ToolTipProvider.RemoveToolTip(new IntPtr(i));
			}
			if (!this.parentRows.IsEmpty())
			{
				bool flag = this.isRightToLeft();
				int detailsButtonWidth = this.Caption.GetDetailsButtonWidth();
				Rectangle backButtonRect = this.Caption.GetBackButtonRect(this.layout.Caption, flag, detailsButtonWidth);
				Rectangle detailsButtonRect = this.Caption.GetDetailsButtonRect(this.layout.Caption, flag);
				backButtonRect.X = this.MirrorRectangle(backButtonRect, this.layout.Inside, this.isRightToLeft());
				detailsButtonRect.X = this.MirrorRectangle(detailsButtonRect, this.layout.Inside, this.isRightToLeft());
				this.ToolTipProvider.AddToolTip(SR.GetString("DataGridCaptionBackButtonToolTip"), new IntPtr(0), backButtonRect);
				this.ToolTipProvider.AddToolTip(SR.GetString("DataGridCaptionDetailsButtonToolTip"), new IntPtr(1), detailsButtonRect);
				this.ToolTipId = 2;
				return;
			}
			this.ToolTipId = 0;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000614B0 File Offset: 0x000604B0
		private void CreateDataGridRows()
		{
			CurrencyManager currencyManager = this.ListManager;
			DataGridTableStyle dataGridTableStyle = this.myGridTable;
			this.InitializeColumnWidths();
			if (currencyManager == null)
			{
				this.SetDataGridRows(new DataGridRow[0], 0);
				return;
			}
			int num = currencyManager.Count;
			if (this.policy.AllowAdd)
			{
				num++;
			}
			DataGridRow[] array = new DataGridRow[num];
			for (int i = 0; i < currencyManager.Count; i++)
			{
				array[i] = new DataGridRelationshipRow(this, dataGridTableStyle, i);
			}
			if (this.policy.AllowAdd)
			{
				this.addNewRow = new DataGridAddNewRow(this, dataGridTableStyle, num - 1);
				array[num - 1] = this.addNewRow;
			}
			else
			{
				this.addNewRow = null;
			}
			this.SetDataGridRows(array, num);
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x0006155C File Offset: 0x0006055C
		private void RecreateDataGridRows()
		{
			int num = 0;
			CurrencyManager currencyManager = this.ListManager;
			if (currencyManager != null)
			{
				num = currencyManager.Count;
				if (this.policy.AllowAdd)
				{
					num++;
				}
			}
			this.SetDataGridRows(null, num);
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x00061598 File Offset: 0x00060598
		internal void SetDataGridRows(DataGridRow[] newRows, int newRowsLength)
		{
			this.dataGridRows = newRows;
			this.dataGridRowsLength = newRowsLength;
			this.vertScrollBar.Maximum = Math.Max(0, this.DataGridRowsLength - 1);
			if (this.firstVisibleRow > newRowsLength)
			{
				this.vertScrollBar.Value = 0;
				this.firstVisibleRow = 0;
			}
			this.ResetUIState();
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x000615EE File Offset: 0x000605EE
		internal int DataGridRowsLength
		{
			get
			{
				return this.dataGridRowsLength;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000615F6 File Offset: 0x000605F6
		// (set) Token: 0x0600275B RID: 10075 RVA: 0x00061600 File Offset: 0x00060600
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("DataGridDataSourceDescr")]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.dataSource != null && this.dataSource.Equals(value))
				{
					return;
				}
				if ((value == null || value == Convert.DBNull) && this.DataMember != null && this.DataMember.Length != 0)
				{
					this.dataSource = null;
					this.DataMember = "";
					return;
				}
				if (value != null)
				{
					this.EnforceValidDataMember(value);
				}
				this.ResetParentRows();
				this.Set_ListManager(value, this.DataMember, false);
			}
		}

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x0600275C RID: 10076 RVA: 0x00061698 File Offset: 0x00060698
		// (remove) Token: 0x0600275D RID: 10077 RVA: 0x000616AB File Offset: 0x000606AB
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnDataSourceChangedDescr")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_DATASOURCECHANGED, value);
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000616BE File Offset: 0x000606BE
		// (set) Token: 0x0600275F RID: 10079 RVA: 0x000616C6 File Offset: 0x000606C6
		[DefaultValue(null)]
		[SRDescription("DataGridDataMemberDescr")]
		[SRCategory("CatData")]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (this.dataMember != null && this.dataMember.Equals(value))
				{
					return;
				}
				this.ResetParentRows();
				this.Set_ListManager(this.DataSource, value, false);
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000616F4 File Offset: 0x000606F4
		public void SetDataBinding(object dataSource, string dataMember)
		{
			this.parentRows.Clear();
			this.originalState = null;
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			this.Set_ListManager(dataSource, dataMember, false);
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x0006175D File Offset: 0x0006075D
		// (set) Token: 0x06002762 RID: 10082 RVA: 0x0006179A File Offset: 0x0006079A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("DataGridListManagerDescr")]
		protected internal CurrencyManager ListManager
		{
			get
			{
				if (this.listManager == null && this.BindingContext != null && this.DataSource != null)
				{
					return (CurrencyManager)this.BindingContext[this.DataSource, this.DataMember];
				}
				return this.listManager;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("DataGridSetListManager"));
			}
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000617AB File Offset: 0x000607AB
		internal void Set_ListManager(object newDataSource, string newDataMember, bool force)
		{
			this.Set_ListManager(newDataSource, newDataMember, force, true);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000617B8 File Offset: 0x000607B8
		internal void Set_ListManager(object newDataSource, string newDataMember, bool force, bool forceColumnCreation)
		{
			bool flag = this.DataSource != newDataSource;
			bool flag2 = this.DataMember != newDataMember;
			if (!force && !flag && !flag2 && this.gridState[2097152])
			{
				return;
			}
			this.gridState[2097152] = true;
			if (this.toBeDisposedEditingControl != null)
			{
				base.Controls.Remove(this.toBeDisposedEditingControl);
				this.toBeDisposedEditingControl = null;
			}
			bool flag3 = true;
			try
			{
				this.UpdateListManager();
				if (this.listManager != null)
				{
					this.UnWireDataSource();
				}
				CurrencyManager currencyManager = this.listManager;
				if (newDataSource != null && this.BindingContext != null && newDataSource != Convert.DBNull)
				{
					this.listManager = (CurrencyManager)this.BindingContext[newDataSource, newDataMember];
				}
				else
				{
					this.listManager = null;
				}
				this.dataSource = newDataSource;
				this.dataMember = ((newDataMember == null) ? "" : newDataMember);
				bool flag4 = this.listManager != currencyManager;
				if (this.listManager != null)
				{
					this.WireDataSource();
					this.policy.UpdatePolicy(this.listManager, this.ReadOnly);
				}
				if (!this.Initializing && this.listManager == null)
				{
					if (base.ContainsFocus && this.ParentInternal == null)
					{
						for (int i = 0; i < base.Controls.Count; i++)
						{
							if (base.Controls[i].Focused)
							{
								this.toBeDisposedEditingControl = base.Controls[i];
								break;
							}
						}
						if (this.toBeDisposedEditingControl == this.horizScrollBar || this.toBeDisposedEditingControl == this.vertScrollBar)
						{
							this.toBeDisposedEditingControl = null;
						}
					}
					this.SetDataGridRows(null, 0);
					this.defaultTableStyle.GridColumnStyles.Clear();
					this.SetDataGridTable(this.defaultTableStyle, forceColumnCreation);
					if (this.toBeDisposedEditingControl != null)
					{
						base.Controls.Add(this.toBeDisposedEditingControl);
					}
				}
				if (flag4 || this.gridState[4194304])
				{
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					if (this.listManager != null)
					{
						this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
						DataGridTableStyle dataGridTableStyle = this.dataGridTables[this.listManager.GetListName()];
						if (dataGridTableStyle == null)
						{
							this.SetDataGridTable(this.defaultTableStyle, forceColumnCreation);
						}
						else
						{
							this.SetDataGridTable(dataGridTableStyle, forceColumnCreation);
						}
						this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
					}
					this.RecreateDataGridRows();
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
					flag3 = false;
					this.ComputeMinimumRowHeaderWidth();
					if (this.myGridTable.IsDefault)
					{
						this.RowHeaderWidth = Math.Max(this.minRowHeaderWidth, this.RowHeaderWidth);
					}
					else
					{
						this.myGridTable.RowHeaderWidth = Math.Max(this.minRowHeaderWidth, this.RowHeaderWidth);
					}
					this.ListHasErrors = this.DataGridSourceHasErrors();
					this.ResetUIState();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
			finally
			{
				this.gridState[2097152] = false;
				if (flag3 && base.Visible)
				{
					base.EndUpdateInternal();
				}
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x00061AEC File Offset: 0x00060AEC
		// (set) Token: 0x06002766 RID: 10086 RVA: 0x00061B50 File Offset: 0x00060B50
		[SRDescription("DataGridSelectedIndexDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentRowIndex
		{
			get
			{
				if (this.originalState == null)
				{
					if (this.listManager != null)
					{
						return this.listManager.Position;
					}
					return -1;
				}
				else
				{
					if (this.BindingContext == null)
					{
						return -1;
					}
					CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.originalState.DataSource, this.originalState.DataMember];
					return currencyManager.Position;
				}
			}
			set
			{
				if (this.listManager == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridSetSelectIndex"));
				}
				if (this.originalState == null)
				{
					this.listManager.Position = value;
					this.currentRow = value;
					return;
				}
				CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.originalState.DataSource, this.originalState.DataMember];
				currencyManager.Position = value;
				this.originalState.LinkingRow = this.originalState.DataGridRows[value];
				base.Invalidate();
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x00061BDD File Offset: 0x00060BDD
		[SRDescription("DataGridGridTablesDescr")]
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		public GridTableStylesCollection TableStyles
		{
			get
			{
				return this.dataGridTables;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x00061BE5 File Offset: 0x00060BE5
		internal new int FontHeight
		{
			get
			{
				return this.fontHeight;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x00061BED File Offset: 0x00060BED
		internal AccessibleObject ParentRowsAccessibleObject
		{
			get
			{
				return this.parentRows.AccessibleObject;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x00061BFA File Offset: 0x00060BFA
		internal Rectangle ParentRowsBounds
		{
			get
			{
				return this.layout.ParentRows;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x00061C07 File Offset: 0x00060C07
		// (set) Token: 0x0600276C RID: 10092 RVA: 0x00061C14 File Offset: 0x00060C14
		[SRCategory("CatColors")]
		[SRDescription("DataGridGridLineColorDescr")]
		public Color GridLineColor
		{
			get
			{
				return this.gridLineBrush.Color;
			}
			set
			{
				if (this.gridLineBrush.Color != value)
				{
					if (value.IsEmpty)
					{
						throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "GridLineColor" }));
					}
					this.gridLineBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Data);
				}
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x00061C7A File Offset: 0x00060C7A
		protected virtual bool ShouldSerializeGridLineColor()
		{
			return !this.GridLineBrush.Equals(DataGrid.DefaultGridLineBrush);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00061C8F File Offset: 0x00060C8F
		public void ResetGridLineColor()
		{
			if (this.ShouldSerializeGridLineColor())
			{
				this.GridLineColor = DataGrid.DefaultGridLineBrush.Color;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x00061CA9 File Offset: 0x00060CA9
		internal SolidBrush GridLineBrush
		{
			get
			{
				return this.gridLineBrush;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x00061CB1 File Offset: 0x00060CB1
		// (set) Token: 0x06002771 RID: 10097 RVA: 0x00061CBC File Offset: 0x00060CBC
		[DefaultValue(DataGridLineStyle.Solid)]
		[SRDescription("DataGridGridLineStyleDescr")]
		[SRCategory("CatAppearance")]
		public DataGridLineStyle GridLineStyle
		{
			get
			{
				return this.gridLineStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.gridLineStyle != value)
				{
					this.gridLineStyle = value;
					this.myGridTable.ResetRelationsUI();
					base.Invalidate(this.layout.Data);
				}
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x00061D1B File Offset: 0x00060D1B
		internal int GridLineWidth
		{
			get
			{
				if (this.GridLineStyle != DataGridLineStyle.Solid)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x00061D29 File Offset: 0x00060D29
		// (set) Token: 0x06002774 RID: 10100 RVA: 0x00061D34 File Offset: 0x00060D34
		[SRCategory("CatDisplay")]
		[SRDescription("DataGridParentRowsLabelStyleDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(DataGridParentRowsLabelStyle.Both)]
		public DataGridParentRowsLabelStyle ParentRowsLabelStyle
		{
			get
			{
				return this.parentRowsLabels;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridParentRowsLabelStyle));
				}
				if (this.parentRowsLabels != value)
				{
					this.parentRowsLabels = value;
					base.Invalidate(this.layout.ParentRows);
					this.OnParentRowsLabelStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x06002775 RID: 10101 RVA: 0x00061D93 File Offset: 0x00060D93
		// (remove) Token: 0x06002776 RID: 10102 RVA: 0x00061DA6 File Offset: 0x00060DA6
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnParentRowsLabelStyleChangedDescr")]
		public event EventHandler ParentRowsLabelStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED, value);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x00061DB9 File Offset: 0x00060DB9
		internal bool Initializing
		{
			get
			{
				return this.inInit;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x00061DC1 File Offset: 0x00060DC1
		[SRDescription("DataGridFirstVisibleColumnDescr")]
		[Browsable(false)]
		public int FirstVisibleColumn
		{
			get
			{
				return this.firstVisibleCol;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x00061DC9 File Offset: 0x00060DC9
		// (set) Token: 0x0600277A RID: 10106 RVA: 0x00061DD8 File Offset: 0x00060DD8
		[SRDescription("DataGridFlatModeDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool FlatMode
		{
			get
			{
				return this.gridState[64];
			}
			set
			{
				if (value != this.FlatMode)
				{
					this.gridState[64] = value;
					base.Invalidate(this.layout.Inside);
					this.OnFlatModeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x0600277B RID: 10107 RVA: 0x00061E0D File Offset: 0x00060E0D
		// (remove) Token: 0x0600277C RID: 10108 RVA: 0x00061E20 File Offset: 0x00060E20
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnFlatModeChangedDescr")]
		public event EventHandler FlatModeChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_FLATMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_FLATMODECHANGED, value);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x00061E33 File Offset: 0x00060E33
		// (set) Token: 0x0600277E RID: 10110 RVA: 0x00061E40 File Offset: 0x00060E40
		[SRCategory("CatColors")]
		[SRDescription("DataGridHeaderBackColorDescr")]
		public Color HeaderBackColor
		{
			get
			{
				return this.headerBackBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "HeaderBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentHeaderBackColorNotAllowed"));
				}
				if (!value.Equals(this.headerBackBrush.Color))
				{
					this.headerBackBrush = new SolidBrush(value);
					if (this.layout.RowHeadersVisible)
					{
						base.Invalidate(this.layout.RowHeaders);
					}
					if (this.layout.ColumnHeadersVisible)
					{
						base.Invalidate(this.layout.ColumnHeaders);
					}
					base.Invalidate(this.layout.TopLeftHeader);
				}
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x00061F06 File Offset: 0x00060F06
		internal SolidBrush HeaderBackBrush
		{
			get
			{
				return this.headerBackBrush;
			}
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x00061F0E File Offset: 0x00060F0E
		protected virtual bool ShouldSerializeHeaderBackColor()
		{
			return !this.HeaderBackBrush.Equals(DataGrid.DefaultHeaderBackBrush);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x00061F23 File Offset: 0x00060F23
		public void ResetHeaderBackColor()
		{
			if (this.ShouldSerializeHeaderBackColor())
			{
				this.HeaderBackColor = DataGrid.DefaultHeaderBackBrush.Color;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x00061F3D File Offset: 0x00060F3D
		internal SolidBrush BackgroundBrush
		{
			get
			{
				return this.backgroundBrush;
			}
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x00061F45 File Offset: 0x00060F45
		private void ResetBackgroundColor()
		{
			if (this.backgroundBrush != null && this.BackgroundBrush != DataGrid.DefaultBackgroundBrush)
			{
				this.backgroundBrush.Dispose();
				this.backgroundBrush = null;
			}
			this.backgroundBrush = DataGrid.DefaultBackgroundBrush;
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x00061F79 File Offset: 0x00060F79
		protected virtual bool ShouldSerializeBackgroundColor()
		{
			return !this.BackgroundBrush.Equals(DataGrid.DefaultBackgroundBrush);
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x00061F8E File Offset: 0x00060F8E
		// (set) Token: 0x06002786 RID: 10118 RVA: 0x00061F9C File Offset: 0x00060F9C
		[SRCategory("CatColors")]
		[SRDescription("DataGridBackgroundColorDescr")]
		public Color BackgroundColor
		{
			get
			{
				return this.backgroundBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "BackgroundColor" }));
				}
				if (!value.Equals(this.backgroundBrush.Color))
				{
					if (this.backgroundBrush != null && this.BackgroundBrush != DataGrid.DefaultBackgroundBrush)
					{
						this.backgroundBrush.Dispose();
						this.backgroundBrush = null;
					}
					this.backgroundBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Inside);
					this.OnBackgroundColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x06002787 RID: 10119 RVA: 0x00062040 File Offset: 0x00061040
		// (remove) Token: 0x06002788 RID: 10120 RVA: 0x00062053 File Offset: 0x00061053
		[SRCategory("CatPropertyChanged")]
		[SRDescription("DataGridOnBackgroundColorChangedDescr")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BACKGROUNDCOLORCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BACKGROUNDCOLORCHANGED, value);
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x00062066 File Offset: 0x00061066
		// (set) Token: 0x0600278A RID: 10122 RVA: 0x00062080 File Offset: 0x00061080
		[SRDescription("DataGridHeaderFontDescr")]
		[SRCategory("CatAppearance")]
		public Font HeaderFont
		{
			get
			{
				if (this.headerFont != null)
				{
					return this.headerFont;
				}
				return this.Font;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HeaderFont");
				}
				if (!value.Equals(this.headerFont))
				{
					this.headerFont = value;
					this.RecalculateFonts();
					base.PerformLayout();
					base.Invalidate(this.layout.Inside);
				}
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000620CD File Offset: 0x000610CD
		protected bool ShouldSerializeHeaderFont()
		{
			return this.headerFont != null;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000620DB File Offset: 0x000610DB
		public void ResetHeaderFont()
		{
			if (this.headerFont != null)
			{
				this.headerFont = null;
				this.RecalculateFonts();
				base.PerformLayout();
				base.Invalidate(this.layout.Inside);
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x00062109 File Offset: 0x00061109
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x00062118 File Offset: 0x00061118
		[SRCategory("CatColors")]
		[SRDescription("DataGridHeaderForeColorDescr")]
		public Color HeaderForeColor
		{
			get
			{
				return this.headerForePen.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "HeaderForeColor" }));
				}
				if (!value.Equals(this.headerForePen.Color))
				{
					this.headerForePen = new Pen(value);
					this.headerForeBrush = new SolidBrush(value);
					if (this.layout.RowHeadersVisible)
					{
						base.Invalidate(this.layout.RowHeaders);
					}
					if (this.layout.ColumnHeadersVisible)
					{
						base.Invalidate(this.layout.ColumnHeaders);
					}
					base.Invalidate(this.layout.TopLeftHeader);
				}
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000621D2 File Offset: 0x000611D2
		protected virtual bool ShouldSerializeHeaderForeColor()
		{
			return !this.HeaderForePen.Equals(DataGrid.DefaultHeaderForePen);
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000621E7 File Offset: 0x000611E7
		public void ResetHeaderForeColor()
		{
			if (this.ShouldSerializeHeaderForeColor())
			{
				this.HeaderForeColor = DataGrid.DefaultHeaderForeBrush.Color;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x00062201 File Offset: 0x00061201
		internal SolidBrush HeaderForeBrush
		{
			get
			{
				return this.headerForeBrush;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x00062209 File Offset: 0x00061209
		internal Pen HeaderForePen
		{
			get
			{
				return this.headerForePen;
			}
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x00062211 File Offset: 0x00061211
		private void ResetHorizontalOffset()
		{
			this.horizontalOffset = 0;
			this.negOffset = 0;
			this.firstVisibleCol = 0;
			this.numVisibleCols = 0;
			this.lastTotallyVisibleCol = -1;
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x00062236 File Offset: 0x00061236
		// (set) Token: 0x06002795 RID: 10133 RVA: 0x00062240 File Offset: 0x00061240
		internal int HorizontalOffset
		{
			get
			{
				return this.horizontalOffset;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				int columnWidthSum = this.GetColumnWidthSum();
				int num = columnWidthSum - this.layout.Data.Width;
				if (value > num && num > 0)
				{
					value = num;
				}
				if (value == this.horizontalOffset)
				{
					return;
				}
				int num2 = this.horizontalOffset - value;
				this.horizScrollBar.Value = value;
				Rectangle rectangle = this.layout.Data;
				if (this.layout.ColumnHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
				}
				this.horizontalOffset = value;
				this.firstVisibleCol = this.ComputeFirstVisibleColumn();
				this.ComputeVisibleColumns();
				if (this.gridState[131072])
				{
					if (this.currentCol >= this.firstVisibleCol && this.currentCol < this.firstVisibleCol + this.numVisibleCols - 1 && (this.gridState[32768] || this.gridState[16384]))
					{
						this.Edit();
					}
					else
					{
						this.EndEdit();
					}
					this.gridState[131072] = false;
				}
				else
				{
					this.EndEdit();
				}
				NativeMethods.RECT[] array = this.CreateScrollableRegion(rectangle);
				this.ScrollRectangles(array, num2);
				this.OnScroll(EventArgs.Empty);
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x0006237C File Offset: 0x0006137C
		private void ScrollRectangles(NativeMethods.RECT[] rects, int change)
		{
			if (rects != null)
			{
				if (this.isRightToLeft())
				{
					change = -change;
				}
				foreach (NativeMethods.RECT rect in rects)
				{
					SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), change, 0, ref rect, ref rect);
				}
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x000623CB File Offset: 0x000613CB
		[SRDescription("DataGridHorizScrollBarDescr")]
		protected ScrollBar HorizScrollBar
		{
			get
			{
				return this.horizScrollBar;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000623D3 File Offset: 0x000613D3
		internal bool LedgerStyle
		{
			get
			{
				return this.gridState[32];
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x000623E2 File Offset: 0x000613E2
		// (set) Token: 0x0600279A RID: 10138 RVA: 0x000623F0 File Offset: 0x000613F0
		[SRDescription("DataGridLinkColorDescr")]
		[SRCategory("CatColors")]
		public Color LinkColor
		{
			get
			{
				return this.linkBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "LinkColor" }));
				}
				if (!this.linkBrush.Color.Equals(value))
				{
					this.linkBrush = new SolidBrush(value);
					base.Invalidate(this.layout.Data);
				}
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x00062464 File Offset: 0x00061464
		internal virtual bool ShouldSerializeLinkColor()
		{
			return !this.LinkBrush.Equals(DataGrid.DefaultLinkBrush);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x00062479 File Offset: 0x00061479
		public void ResetLinkColor()
		{
			if (this.ShouldSerializeLinkColor())
			{
				this.LinkColor = DataGrid.DefaultLinkBrush.Color;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x00062493 File Offset: 0x00061493
		internal Brush LinkBrush
		{
			get
			{
				return this.linkBrush;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x0006249B File Offset: 0x0006149B
		// (set) Token: 0x0600279F RID: 10143 RVA: 0x000624A3 File Offset: 0x000614A3
		[SRDescription("DataGridLinkHoverColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatColors")]
		[Browsable(false)]
		public Color LinkHoverColor
		{
			get
			{
				return this.LinkColor;
			}
			set
			{
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000624A5 File Offset: 0x000614A5
		protected virtual bool ShouldSerializeLinkHoverColor()
		{
			return false;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000624A8 File Offset: 0x000614A8
		public void ResetLinkHoverColor()
		{
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060027A2 RID: 10146 RVA: 0x000624AA File Offset: 0x000614AA
		internal Font LinkFont
		{
			get
			{
				return this.linkFont;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x000624B2 File Offset: 0x000614B2
		internal int LinkFontHeight
		{
			get
			{
				return this.linkFontHeight;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x000624BA File Offset: 0x000614BA
		// (set) Token: 0x060027A5 RID: 10149 RVA: 0x000624CC File Offset: 0x000614CC
		[DefaultValue(true)]
		[SRDescription("DataGridNavigationModeDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowNavigation
		{
			get
			{
				return this.gridState[8192];
			}
			set
			{
				if (this.AllowNavigation != value)
				{
					this.gridState[8192] = value;
					this.Caption.BackButtonActive = !this.parentRows.IsEmpty() && value;
					this.Caption.BackButtonVisible = this.Caption.BackButtonActive;
					this.RecreateDataGridRows();
					this.OnAllowNavigationChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x060027A6 RID: 10150 RVA: 0x00062536 File Offset: 0x00061536
		// (remove) Token: 0x060027A7 RID: 10151 RVA: 0x00062549 File Offset: 0x00061549
		[SRDescription("DataGridOnNavigationModeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler AllowNavigationChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_ALLOWNAVIGATIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_ALLOWNAVIGATIONCHANGED, value);
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x0006255C File Offset: 0x0006155C
		// (set) Token: 0x060027A9 RID: 10153 RVA: 0x00062564 File Offset: 0x00061564
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x060027AA RID: 10154 RVA: 0x0006256D File Offset: 0x0006156D
		// (remove) Token: 0x060027AB RID: 10155 RVA: 0x00062576 File Offset: 0x00061576
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x0006257F File Offset: 0x0006157F
		// (set) Token: 0x060027AD RID: 10157 RVA: 0x00062587 File Offset: 0x00061587
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00062590 File Offset: 0x00061590
		// (set) Token: 0x060027AF RID: 10159 RVA: 0x00062598 File Offset: 0x00061598
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x060027B0 RID: 10160 RVA: 0x000625A1 File Offset: 0x000615A1
		// (remove) Token: 0x060027B1 RID: 10161 RVA: 0x000625AA File Offset: 0x000615AA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x060027B2 RID: 10162 RVA: 0x000625B3 File Offset: 0x000615B3
		// (remove) Token: 0x060027B3 RID: 10163 RVA: 0x000625BC File Offset: 0x000615BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x000625C5 File Offset: 0x000615C5
		// (set) Token: 0x060027B5 RID: 10165 RVA: 0x000625D2 File Offset: 0x000615D2
		[SRCategory("CatColors")]
		[SRDescription("DataGridParentRowsBackColorDescr")]
		public Color ParentRowsBackColor
		{
			get
			{
				return this.parentRows.BackColor;
			}
			set
			{
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTransparentParentRowsBackColorNotAllowed"));
				}
				this.parentRows.BackColor = value;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x000625F8 File Offset: 0x000615F8
		internal SolidBrush ParentRowsBackBrush
		{
			get
			{
				return this.parentRows.BackBrush;
			}
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x00062605 File Offset: 0x00061605
		protected virtual bool ShouldSerializeParentRowsBackColor()
		{
			return !this.ParentRowsBackBrush.Equals(DataGrid.DefaultParentRowsBackBrush);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x0006261A File Offset: 0x0006161A
		private void ResetParentRowsBackColor()
		{
			if (this.ShouldSerializeParentRowsBackColor())
			{
				this.parentRows.BackBrush = DataGrid.DefaultParentRowsBackBrush;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x00062634 File Offset: 0x00061634
		// (set) Token: 0x060027BA RID: 10170 RVA: 0x00062641 File Offset: 0x00061641
		[SRDescription("DataGridParentRowsForeColorDescr")]
		[SRCategory("CatColors")]
		public Color ParentRowsForeColor
		{
			get
			{
				return this.parentRows.ForeColor;
			}
			set
			{
				this.parentRows.ForeColor = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x0006264F File Offset: 0x0006164F
		internal SolidBrush ParentRowsForeBrush
		{
			get
			{
				return this.parentRows.ForeBrush;
			}
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x0006265C File Offset: 0x0006165C
		protected virtual bool ShouldSerializeParentRowsForeColor()
		{
			return !this.ParentRowsForeBrush.Equals(DataGrid.DefaultParentRowsForeBrush);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x00062671 File Offset: 0x00061671
		private void ResetParentRowsForeColor()
		{
			if (this.ShouldSerializeParentRowsForeColor())
			{
				this.parentRows.ForeBrush = DataGrid.DefaultParentRowsForeBrush;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x0006268B File Offset: 0x0006168B
		// (set) Token: 0x060027BF RID: 10175 RVA: 0x00062693 File Offset: 0x00061693
		[DefaultValue(75)]
		[TypeConverter(typeof(DataGridPreferredColumnWidthTypeConverter))]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridPreferredColumnWidthDescr")]
		public int PreferredColumnWidth
		{
			get
			{
				return this.preferredColumnWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridColumnWidth"), "PreferredColumnWidth");
				}
				if (this.preferredColumnWidth != value)
				{
					this.preferredColumnWidth = value;
				}
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000626BE File Offset: 0x000616BE
		// (set) Token: 0x060027C1 RID: 10177 RVA: 0x000626C6 File Offset: 0x000616C6
		[SRDescription("DataGridPreferredRowHeightDescr")]
		[SRCategory("CatLayout")]
		public int PreferredRowHeight
		{
			get
			{
				return this.prefferedRowHeight;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridRowRowHeight"));
				}
				this.prefferedRowHeight = value;
			}
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000626E3 File Offset: 0x000616E3
		private void ResetPreferredRowHeight()
		{
			this.prefferedRowHeight = DataGrid.defaultFontHeight + 3;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000626F2 File Offset: 0x000616F2
		protected bool ShouldSerializePreferredRowHeight()
		{
			return this.prefferedRowHeight != DataGrid.defaultFontHeight + 3;
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x00062706 File Offset: 0x00061706
		// (set) Token: 0x060027C5 RID: 10181 RVA: 0x00062718 File Offset: 0x00061718
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("DataGridReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.gridState[4096];
			}
			set
			{
				if (this.ReadOnly != value)
				{
					bool flag = false;
					if (value)
					{
						flag = this.policy.AllowAdd;
						this.policy.AllowRemove = false;
						this.policy.AllowEdit = false;
						this.policy.AllowAdd = false;
					}
					else
					{
						flag |= this.policy.UpdatePolicy(this.listManager, value);
					}
					this.gridState[4096] = value;
					DataGridRow[] array = this.DataGridRows;
					if (flag)
					{
						this.RecreateDataGridRows();
						DataGridRow[] array2 = this.DataGridRows;
						int num = Math.Min(array2.Length, array.Length);
						for (int i = 0; i < num; i++)
						{
							if (array[i].Selected)
							{
								array2[i].Selected = true;
							}
						}
					}
					base.PerformLayout();
					this.InvalidateInside();
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x060027C6 RID: 10182 RVA: 0x000627ED File Offset: 0x000617ED
		// (remove) Token: 0x060027C7 RID: 10183 RVA: 0x00062800 File Offset: 0x00061800
		[SRDescription("DataGridOnReadOnlyChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_READONLYCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_READONLYCHANGED, value);
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x00062813 File Offset: 0x00061813
		// (set) Token: 0x060027C9 RID: 10185 RVA: 0x00062821 File Offset: 0x00061821
		[SRDescription("DataGridColumnHeadersVisibleDescr")]
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		public bool ColumnHeadersVisible
		{
			get
			{
				return this.gridState[2];
			}
			set
			{
				if (this.ColumnHeadersVisible != value)
				{
					this.gridState[2] = value;
					this.layout.ColumnHeadersVisible = value;
					base.PerformLayout();
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x00062851 File Offset: 0x00061851
		// (set) Token: 0x060027CB RID: 10187 RVA: 0x0006285E File Offset: 0x0006185E
		[SRDescription("DataGridParentRowsVisibleDescr")]
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		public bool ParentRowsVisible
		{
			get
			{
				return this.layout.ParentRowsVisible;
			}
			set
			{
				if (this.layout.ParentRowsVisible != value)
				{
					this.SetParentRowsVisibility(value);
					this.caption.SetDownButtonDirection(!value);
					this.OnParentRowsVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000119 RID: 281
		// (add) Token: 0x060027CC RID: 10188 RVA: 0x0006288F File Offset: 0x0006188F
		// (remove) Token: 0x060027CD RID: 10189 RVA: 0x000628A2 File Offset: 0x000618A2
		[SRDescription("DataGridOnParentRowsVisibleChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ParentRowsVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_PARENTROWSVISIBLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_PARENTROWSVISIBLECHANGED, value);
			}
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000628B5 File Offset: 0x000618B5
		internal bool ParentRowsIsEmpty()
		{
			return this.parentRows.IsEmpty();
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x000628C2 File Offset: 0x000618C2
		// (set) Token: 0x060027D0 RID: 10192 RVA: 0x000628D0 File Offset: 0x000618D0
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		[SRDescription("DataGridRowHeadersVisibleDescr")]
		public bool RowHeadersVisible
		{
			get
			{
				return this.gridState[4];
			}
			set
			{
				if (this.RowHeadersVisible != value)
				{
					this.gridState[4] = value;
					base.PerformLayout();
					this.InvalidateInside();
				}
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000628F4 File Offset: 0x000618F4
		// (set) Token: 0x060027D2 RID: 10194 RVA: 0x000628FC File Offset: 0x000618FC
		[SRCategory("CatLayout")]
		[DefaultValue(35)]
		[SRDescription("DataGridRowHeaderWidthDescr")]
		public int RowHeaderWidth
		{
			get
			{
				return this.rowHeaderWidth;
			}
			set
			{
				value = Math.Max(this.minRowHeaderWidth, value);
				if (this.rowHeaderWidth != value)
				{
					this.rowHeaderWidth = value;
					if (this.layout.RowHeadersVisible)
					{
						base.PerformLayout();
						this.InvalidateInside();
					}
				}
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x00062935 File Offset: 0x00061935
		// (set) Token: 0x060027D4 RID: 10196 RVA: 0x0006293D File Offset: 0x0006193D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x1400011A RID: 282
		// (add) Token: 0x060027D5 RID: 10197 RVA: 0x00062946 File Offset: 0x00061946
		// (remove) Token: 0x060027D6 RID: 10198 RVA: 0x0006294F File Offset: 0x0006194F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x00062958 File Offset: 0x00061958
		[Browsable(false)]
		[SRDescription("DataGridVertScrollBarDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected ScrollBar VertScrollBar
		{
			get
			{
				return this.vertScrollBar;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060027D8 RID: 10200 RVA: 0x00062960 File Offset: 0x00061960
		[Browsable(false)]
		[SRDescription("DataGridVisibleColumnCountDescr")]
		public int VisibleColumnCount
		{
			get
			{
				return Math.Min(this.numVisibleCols, (this.myGridTable == null) ? 0 : this.myGridTable.GridColumnStyles.Count);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x00062988 File Offset: 0x00061988
		[SRDescription("DataGridVisibleRowCountDescr")]
		[Browsable(false)]
		public int VisibleRowCount
		{
			get
			{
				return this.numVisibleRows;
			}
		}

		// Token: 0x1700069D RID: 1693
		public object this[int rowIndex, int columnIndex]
		{
			get
			{
				this.EnsureBound();
				if (rowIndex < 0 || rowIndex >= this.DataGridRowsLength)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (columnIndex < 0 || columnIndex >= this.myGridTable.GridColumnStyles.Count)
				{
					throw new ArgumentOutOfRangeException("columnIndex");
				}
				CurrencyManager currencyManager = this.listManager;
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[columnIndex];
				return dataGridColumnStyle.GetColumnValueAtRow(currencyManager, rowIndex);
			}
			set
			{
				this.EnsureBound();
				if (rowIndex < 0 || rowIndex >= this.DataGridRowsLength)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (columnIndex < 0 || columnIndex >= this.myGridTable.GridColumnStyles.Count)
				{
					throw new ArgumentOutOfRangeException("columnIndex");
				}
				CurrencyManager currencyManager = this.listManager;
				if (currencyManager.Position != rowIndex)
				{
					currencyManager.Position = rowIndex;
				}
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[columnIndex];
				dataGridColumnStyle.SetColumnValueAtRow(currencyManager, rowIndex, value);
				if (columnIndex >= this.firstVisibleCol && columnIndex <= this.firstVisibleCol + this.numVisibleCols - 1 && rowIndex >= this.firstVisibleRow && rowIndex <= this.firstVisibleRow + this.numVisibleRows)
				{
					Rectangle cellBounds = this.GetCellBounds(rowIndex, columnIndex);
					base.Invalidate(cellBounds);
				}
			}
		}

		// Token: 0x1700069E RID: 1694
		public object this[DataGridCell cell]
		{
			get
			{
				return this[cell.RowNumber, cell.ColumnNumber];
			}
			set
			{
				this[cell.RowNumber, cell.ColumnNumber] = value;
			}
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x00062AF0 File Offset: 0x00061AF0
		private void WireTableStylePropChanged(DataGridTableStyle gridTable)
		{
			gridTable.GridLineColorChanged += this.GridLineColorChanged;
			gridTable.GridLineStyleChanged += this.GridLineStyleChanged;
			gridTable.HeaderBackColorChanged += this.HeaderBackColorChanged;
			gridTable.HeaderFontChanged += this.HeaderFontChanged;
			gridTable.HeaderForeColorChanged += this.HeaderForeColorChanged;
			gridTable.LinkColorChanged += this.LinkColorChanged;
			gridTable.LinkHoverColorChanged += this.LinkHoverColorChanged;
			gridTable.PreferredColumnWidthChanged += this.PreferredColumnWidthChanged;
			gridTable.RowHeadersVisibleChanged += this.RowHeadersVisibleChanged;
			gridTable.ColumnHeadersVisibleChanged += this.ColumnHeadersVisibleChanged;
			gridTable.RowHeaderWidthChanged += this.RowHeaderWidthChanged;
			gridTable.AllowSortingChanged += this.AllowSortingChanged;
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x00062BD8 File Offset: 0x00061BD8
		private void UnWireTableStylePropChanged(DataGridTableStyle gridTable)
		{
			gridTable.GridLineColorChanged -= this.GridLineColorChanged;
			gridTable.GridLineStyleChanged -= this.GridLineStyleChanged;
			gridTable.HeaderBackColorChanged -= this.HeaderBackColorChanged;
			gridTable.HeaderFontChanged -= this.HeaderFontChanged;
			gridTable.HeaderForeColorChanged -= this.HeaderForeColorChanged;
			gridTable.LinkColorChanged -= this.LinkColorChanged;
			gridTable.LinkHoverColorChanged -= this.LinkHoverColorChanged;
			gridTable.PreferredColumnWidthChanged -= this.PreferredColumnWidthChanged;
			gridTable.RowHeadersVisibleChanged -= this.RowHeadersVisibleChanged;
			gridTable.ColumnHeadersVisibleChanged -= this.ColumnHeadersVisibleChanged;
			gridTable.RowHeaderWidthChanged -= this.RowHeaderWidthChanged;
			gridTable.AllowSortingChanged -= this.AllowSortingChanged;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00062CC0 File Offset: 0x00061CC0
		private void WireDataSource()
		{
			this.listManager.CurrentChanged += this.currentChangedHandler;
			this.listManager.PositionChanged += this.positionChangedHandler;
			this.listManager.ItemChanged += this.itemChangedHandler;
			this.listManager.MetaDataChanged += this.metaDataChangedHandler;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00062D14 File Offset: 0x00061D14
		private void UnWireDataSource()
		{
			this.listManager.CurrentChanged -= this.currentChangedHandler;
			this.listManager.PositionChanged -= this.positionChangedHandler;
			this.listManager.ItemChanged -= this.itemChangedHandler;
			this.listManager.MetaDataChanged -= this.metaDataChangedHandler;
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x00062D68 File Offset: 0x00061D68
		private void DataSource_Changed(object sender, EventArgs ea)
		{
			this.policy.UpdatePolicy(this.ListManager, this.ReadOnly);
			if (this.gridState[512])
			{
				DataGridRow[] array = this.DataGridRows;
				int num = this.DataGridRowsLength;
				array[num - 1] = new DataGridRelationshipRow(this, this.myGridTable, num - 1);
				this.SetDataGridRows(array, num);
			}
			else if (this.gridState[1048576] && !this.gridState[1024])
			{
				this.listManager.CancelCurrentEdit();
				this.gridState[1048576] = false;
				this.RecreateDataGridRows();
			}
			else if (!this.gridState[1024])
			{
				this.RecreateDataGridRows();
				this.currentRow = Math.Min(this.currentRow, this.listManager.Count);
			}
			bool listHasErrors = this.ListHasErrors;
			this.ListHasErrors = this.DataGridSourceHasErrors();
			if (listHasErrors == this.ListHasErrors)
			{
				this.InvalidateInside();
			}
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x00062E69 File Offset: 0x00061E69
		private void GridLineColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x00062E7C File Offset: 0x00061E7C
		private void GridLineStyleChanged(object sender, EventArgs e)
		{
			this.myGridTable.ResetRelationsUI();
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x00062E9C File Offset: 0x00061E9C
		private void HeaderBackColorChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.Invalidate(this.layout.RowHeaders);
			}
			if (this.layout.ColumnHeadersVisible)
			{
				base.Invalidate(this.layout.ColumnHeaders);
			}
			base.Invalidate(this.layout.TopLeftHeader);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00062EF6 File Offset: 0x00061EF6
		private void HeaderFontChanged(object sender, EventArgs e)
		{
			this.RecalculateFonts();
			base.PerformLayout();
			base.Invalidate(this.layout.Inside);
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00062F18 File Offset: 0x00061F18
		private void HeaderForeColorChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.Invalidate(this.layout.RowHeaders);
			}
			if (this.layout.ColumnHeadersVisible)
			{
				base.Invalidate(this.layout.ColumnHeaders);
			}
			base.Invalidate(this.layout.TopLeftHeader);
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x00062F72 File Offset: 0x00061F72
		private void LinkColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x00062F85 File Offset: 0x00061F85
		private void LinkHoverColorChanged(object sender, EventArgs e)
		{
			base.Invalidate(this.layout.Data);
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x00062F98 File Offset: 0x00061F98
		private void PreferredColumnWidthChanged(object sender, EventArgs e)
		{
			this.SetDataGridRows(null, this.DataGridRowsLength);
			base.PerformLayout();
			base.Invalidate();
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x00062FB3 File Offset: 0x00061FB3
		private void RowHeadersVisibleChanged(object sender, EventArgs e)
		{
			this.layout.RowHeadersVisible = this.myGridTable != null && this.myGridTable.RowHeadersVisible;
			base.PerformLayout();
			this.InvalidateInside();
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00062FE2 File Offset: 0x00061FE2
		private void ColumnHeadersVisibleChanged(object sender, EventArgs e)
		{
			this.layout.ColumnHeadersVisible = this.myGridTable != null && this.myGridTable.ColumnHeadersVisible;
			base.PerformLayout();
			this.InvalidateInside();
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x00063011 File Offset: 0x00062011
		private void RowHeaderWidthChanged(object sender, EventArgs e)
		{
			if (this.layout.RowHeadersVisible)
			{
				base.PerformLayout();
				this.InvalidateInside();
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x0006302C File Offset: 0x0006202C
		private void AllowSortingChanged(object sender, EventArgs e)
		{
			if (!this.myGridTable.AllowSorting && this.listManager != null)
			{
				IList list = this.listManager.List;
				if (list is IBindingList)
				{
					((IBindingList)list).RemoveSort();
				}
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x0006306D File Offset: 0x0006206D
		private void DataSource_RowChanged(object sender, EventArgs ea)
		{
			DataGridRow[] array = this.DataGridRows;
			if (this.currentRow < this.DataGridRowsLength)
			{
				this.InvalidateRow(this.currentRow);
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x00063090 File Offset: 0x00062090
		private void DataSource_PositionChanged(object sender, EventArgs ea)
		{
			if (this.DataGridRowsLength > this.listManager.Count + (this.policy.AllowAdd ? 1 : 0) && !this.gridState[1024])
			{
				this.RecreateDataGridRows();
			}
			if (this.ListManager.Position != this.currentRow)
			{
				this.CurrentCell = new DataGridCell(this.listManager.Position, this.currentCol);
			}
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x00063109 File Offset: 0x00062109
		internal void DataSource_MetaDataChanged(object sender, EventArgs e)
		{
			this.MetaDataChanged();
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x00063114 File Offset: 0x00062114
		private bool DataGridSourceHasErrors()
		{
			if (this.listManager == null)
			{
				return false;
			}
			for (int i = 0; i < this.listManager.Count; i++)
			{
				object obj = this.listManager[i];
				if (obj is IDataErrorInfo)
				{
					string error = ((IDataErrorInfo)obj).Error;
					if (error != null && error.Length != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x00063170 File Offset: 0x00062170
		private void TableStylesCollectionChanged(object sender, CollectionChangeEventArgs ccea)
		{
			if (sender != this.dataGridTables)
			{
				return;
			}
			if (this.listManager == null)
			{
				return;
			}
			if (ccea.Action == CollectionChangeAction.Add)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)ccea.Element;
				if (this.listManager.GetListName().Equals(dataGridTableStyle.MappingName))
				{
					this.SetDataGridTable(dataGridTableStyle, true);
					this.SetDataGridRows(null, 0);
					return;
				}
			}
			else if (ccea.Action == CollectionChangeAction.Remove)
			{
				DataGridTableStyle dataGridTableStyle2 = (DataGridTableStyle)ccea.Element;
				if (this.myGridTable.MappingName.Equals(dataGridTableStyle2.MappingName))
				{
					this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
					this.SetDataGridTable(this.defaultTableStyle, true);
					this.SetDataGridRows(null, 0);
					return;
				}
			}
			else
			{
				DataGridTableStyle dataGridTableStyle3 = this.dataGridTables[this.listManager.GetListName()];
				if (dataGridTableStyle3 == null)
				{
					if (!this.myGridTable.IsDefault)
					{
						this.defaultTableStyle.GridColumnStyles.ResetDefaultColumnCollection();
						this.SetDataGridTable(this.defaultTableStyle, true);
						this.SetDataGridRows(null, 0);
						return;
					}
				}
				else
				{
					this.SetDataGridTable(dataGridTableStyle3, true);
					this.SetDataGridRows(null, 0);
				}
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x00063284 File Offset: 0x00062284
		private void DataSource_ItemChanged(object sender, ItemChangedEventArgs ea)
		{
			if (ea.Index == -1)
			{
				this.DataSource_Changed(sender, EventArgs.Empty);
				return;
			}
			object obj = this.listManager[ea.Index];
			bool listHasErrors = this.ListHasErrors;
			if (obj is IDataErrorInfo)
			{
				if (((IDataErrorInfo)obj).Error.Length != 0)
				{
					this.ListHasErrors = true;
				}
				else if (this.ListHasErrors)
				{
					this.ListHasErrors = this.DataGridSourceHasErrors();
				}
			}
			if (listHasErrors == this.ListHasErrors)
			{
				this.InvalidateRow(ea.Index);
			}
			if (this.editColumn != null && ea.Index == this.currentRow)
			{
				this.editColumn.UpdateUI(this.ListManager, ea.Index, null);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x0006333C File Offset: 0x0006233C
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_BORDERSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x0006336C File Offset: 0x0006236C
		protected virtual void OnCaptionVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_CAPTIONVISIBLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x0006339C File Offset: 0x0006239C
		protected virtual void OnCurrentCellChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_CURRENTCELLCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000633CC File Offset: 0x000623CC
		protected virtual void OnFlatModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_FLATMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000633FC File Offset: 0x000623FC
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_BACKGROUNDCOLORCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x0006342C File Offset: 0x0006242C
		protected virtual void OnAllowNavigationChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_ALLOWNAVIGATIONCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x0006345C File Offset: 0x0006245C
		protected virtual void OnParentRowsVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_PARENTROWSVISIBLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x0006348C File Offset: 0x0006248C
		protected virtual void OnParentRowsLabelStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_PARENTROWSLABELSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000634BC File Offset: 0x000624BC
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_READONLYCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000634EA File Offset: 0x000624EA
		protected void OnNavigate(NavigateEventArgs e)
		{
			if (this.onNavigate != null)
			{
				this.onNavigate(this, e);
			}
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00063504 File Offset: 0x00062504
		internal void OnNodeClick(EventArgs e)
		{
			base.PerformLayout();
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (this.firstVisibleCol > -1 && this.firstVisibleCol < gridColumnStyles.Count && gridColumnStyles[this.firstVisibleCol] == this.editColumn)
			{
				this.Edit();
			}
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_NODECLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00063575 File Offset: 0x00062575
		protected void OnRowHeaderClick(EventArgs e)
		{
			if (this.onRowHeaderClick != null)
			{
				this.onRowHeaderClick(this, e);
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x0006358C File Offset: 0x0006258C
		protected void OnScroll(EventArgs e)
		{
			if (this.ToolTipProvider != null)
			{
				this.ResetToolTip();
			}
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_SCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000635C8 File Offset: 0x000625C8
		protected virtual void GridHScrolled(object sender, ScrollEventArgs se)
		{
			if (!base.Enabled)
			{
				return;
			}
			if (this.DataSource == null)
			{
				return;
			}
			this.gridState[131072] = true;
			if (se.Type == ScrollEventType.SmallIncrement || se.Type == ScrollEventType.SmallDecrement)
			{
				int num = ((se.Type == ScrollEventType.SmallIncrement) ? 1 : (-1));
				if (se.Type == ScrollEventType.SmallDecrement && this.negOffset == 0)
				{
					GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
					int num2 = this.firstVisibleCol - 1;
					while (num2 >= 0 && gridColumnStyles[num2].Width == 0)
					{
						num--;
						num2--;
					}
				}
				if (se.Type == ScrollEventType.SmallIncrement && this.negOffset == 0)
				{
					GridColumnStylesCollection gridColumnStyles2 = this.myGridTable.GridColumnStyles;
					int num3 = this.firstVisibleCol;
					while (num3 > -1 && num3 < gridColumnStyles2.Count && gridColumnStyles2[num3].Width == 0)
					{
						num++;
						num3++;
					}
				}
				this.ScrollRight(num);
				se.NewValue = this.HorizontalOffset;
			}
			else if (se.Type != ScrollEventType.EndScroll)
			{
				this.HorizontalOffset = se.NewValue;
			}
			this.gridState[131072] = false;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000636E8 File Offset: 0x000626E8
		protected virtual void GridVScrolled(object sender, ScrollEventArgs se)
		{
			if (!base.Enabled)
			{
				return;
			}
			if (this.DataSource == null)
			{
				return;
			}
			this.gridState[131072] = true;
			try
			{
				se.NewValue = Math.Min(se.NewValue, this.DataGridRowsLength - this.numTotallyVisibleRows);
				int num = se.NewValue - this.firstVisibleRow;
				this.ScrollDown(num);
			}
			finally
			{
				this.gridState[131072] = false;
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x00063770 File Offset: 0x00062770
		private void HandleEndCurrentEdit()
		{
			int num = this.currentRow;
			int num2 = this.currentCol;
			string text = null;
			try
			{
				this.listManager.EndCurrentEdit();
			}
			catch (Exception ex)
			{
				text = ex.Message;
			}
			if (text != null)
			{
				DialogResult dialogResult = RTLAwareMessageBox.Show(null, SR.GetString("DataGridPushedIncorrectValueIntoColumn", new object[] { text }), SR.GetString("DataGridErrorMessageBoxCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
				if (dialogResult == DialogResult.Yes)
				{
					this.currentRow = num;
					this.currentCol = num2;
					this.InvalidateRowHeader(this.currentRow);
					this.Edit();
					return;
				}
				this.listManager.PositionChanged -= this.positionChangedHandler;
				this.listManager.CancelCurrentEdit();
				this.listManager.Position = this.currentRow;
				this.listManager.PositionChanged += this.positionChangedHandler;
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x00063850 File Offset: 0x00062850
		protected void OnBackButtonClicked(object sender, EventArgs e)
		{
			this.NavigateBack();
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_BACKBUTTONCLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x00063884 File Offset: 0x00062884
		protected override void OnBackColorChanged(EventArgs e)
		{
			this.backBrush = new SolidBrush(this.BackColor);
			base.Invalidate();
			base.OnBackColorChanged(e);
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000638A4 File Offset: 0x000628A4
		protected override void OnBindingContextChanged(EventArgs e)
		{
			if (this.DataSource != null && !this.gridState[2097152])
			{
				try
				{
					this.Set_ListManager(this.DataSource, this.DataMember, true, false);
				}
				catch
				{
					if (this.Site == null || !this.Site.DesignMode)
					{
						throw;
					}
					RTLAwareMessageBox.Show(null, SR.GetString("DataGridExceptionInPaint"), null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					this.ResetParentRows();
					this.Set_ListManager(null, string.Empty, true);
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
				}
			}
			base.OnBindingContextChanged(e);
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x0006395C File Offset: 0x0006295C
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGrid.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x0006398C File Offset: 0x0006298C
		protected void OnShowParentDetailsButtonClicked(object sender, EventArgs e)
		{
			this.ParentRowsVisible = !this.caption.ToggleDownButtonDirection();
			EventHandler eventHandler = (EventHandler)base.Events[DataGrid.EVENT_DOWNBUTTONCLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000639CE File Offset: 0x000629CE
		protected override void OnForeColorChanged(EventArgs e)
		{
			this.foreBrush = new SolidBrush(this.ForeColor);
			base.Invalidate();
			base.OnForeColorChanged(e);
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000639F0 File Offset: 0x000629F0
		protected override void OnFontChanged(EventArgs e)
		{
			this.Caption.OnGridFontChanged();
			this.RecalculateFonts();
			this.RecreateDataGridRows();
			if (this.originalState != null)
			{
				Stack stack = new Stack();
				while (!this.parentRows.IsEmpty())
				{
					DataGridState dataGridState = this.parentRows.PopTop();
					int num = dataGridState.DataGridRowsLength;
					for (int i = 0; i < num; i++)
					{
						dataGridState.DataGridRows[i].Height = dataGridState.DataGridRows[i].MinimumRowHeight(dataGridState.GridColumnStyles);
					}
					stack.Push(dataGridState);
				}
				while (stack.Count != 0)
				{
					this.parentRows.AddParent((DataGridState)stack.Pop());
				}
			}
			base.OnFontChanged(e);
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00063A9E File Offset: 0x00062A9E
		protected override void OnPaintBackground(PaintEventArgs ebe)
		{
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x00063AA0 File Offset: 0x00062AA0
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.gridState[65536])
			{
				return;
			}
			base.OnLayout(levent);
			if (this.gridState[16777216])
			{
				return;
			}
			this.gridState[2048] = false;
			try
			{
				if (base.IsHandleCreated)
				{
					if (this.layout.ParentRowsVisible)
					{
						this.parentRows.OnLayout();
					}
					if (this.ToolTipProvider != null)
					{
						this.ResetToolTip();
					}
					this.ComputeLayout();
				}
			}
			finally
			{
				this.gridState[2048] = true;
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x00063B44 File Offset: 0x00062B44
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.toolTipProvider = new DataGridToolTip(this);
			this.toolTipProvider.CreateToolTipHandle();
			this.toolTipId = 0;
			base.PerformLayout();
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00063B71 File Offset: 0x00062B71
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			this.toolTipProvider.Destroy();
			this.toolTipProvider = null;
			this.toolTipId = 0;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00063B93 File Offset: 0x00062B93
		protected override void OnEnter(EventArgs e)
		{
			if (this.gridState[2048] && !this.gridState[65536])
			{
				if (this.Bound)
				{
					this.Edit();
				}
				base.OnEnter(e);
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x00063BCE File Offset: 0x00062BCE
		protected override void OnLeave(EventArgs e)
		{
			this.OnLeave_Grid();
			base.OnLeave(e);
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x00063BE0 File Offset: 0x00062BE0
		private void OnLeave_Grid()
		{
			this.gridState[2048] = false;
			try
			{
				this.EndEdit();
				if (this.listManager != null && !this.gridState[65536])
				{
					if (this.gridState[1048576])
					{
						this.listManager.CancelCurrentEdit();
						DataGridRow[] array = this.DataGridRows;
						array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
						this.SetDataGridRows(array, this.DataGridRowsLength);
					}
					else
					{
						this.HandleEndCurrentEdit();
					}
				}
			}
			finally
			{
				this.gridState[2048] = true;
				if (!this.gridState[65536])
				{
					this.gridState[1048576] = false;
				}
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x00063CBC File Offset: 0x00062CBC
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			base.OnKeyDown(ke);
			this.ProcessGridKey(ke);
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x00063CD0 File Offset: 0x00062CD0
		protected override void OnKeyPress(KeyPressEventArgs kpe)
		{
			base.OnKeyPress(kpe);
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (gridColumnStyles != null && this.currentCol > 0 && this.currentCol < gridColumnStyles.Count && !gridColumnStyles[this.currentCol].ReadOnly && kpe.KeyChar > ' ')
			{
				this.Edit(new string(new char[] { kpe.KeyChar }));
			}
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x00063D44 File Offset: 0x00062D44
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.gridState[524288] = false;
			this.gridState[256] = false;
			if (this.listManager == null)
			{
				return;
			}
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			Keys modifierKeys = Control.ModifierKeys;
			bool flag = (modifierKeys & Keys.Control) == Keys.Control && (modifierKeys & Keys.Alt) == Keys.None;
			bool flag2 = (modifierKeys & Keys.Shift) == Keys.Shift;
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ColumnResize)
			{
				if (e.Clicks > 1)
				{
					this.ColAutoResize(hitTestInfo.col);
					return;
				}
				this.ColResizeBegin(e, hitTestInfo.col);
				return;
			}
			else if (hitTestInfo.type == DataGrid.HitTestType.RowResize)
			{
				if (e.Clicks > 1)
				{
					this.RowAutoResize(hitTestInfo.row);
					return;
				}
				this.RowResizeBegin(e, hitTestInfo.row);
				return;
			}
			else
			{
				if (hitTestInfo.type == DataGrid.HitTestType.ColumnHeader)
				{
					this.trackColumnHeader = this.myGridTable.GridColumnStyles[hitTestInfo.col].PropertyDescriptor;
					return;
				}
				if (hitTestInfo.type == DataGrid.HitTestType.Caption)
				{
					Rectangle rectangle = this.layout.Caption;
					this.caption.MouseDown(e.X - rectangle.X, e.Y - rectangle.Y);
					return;
				}
				if (this.layout.Data.Contains(e.X, e.Y) || this.layout.RowHeaders.Contains(e.X, e.Y))
				{
					int rowFromY = this.GetRowFromY(e.Y);
					if (rowFromY > -1)
					{
						Point point = this.NormalizeToRow(e.X, e.Y, rowFromY);
						DataGridRow[] array = this.DataGridRows;
						if (array[rowFromY].OnMouseDown(point.X, point.Y, this.layout.RowHeaders, this.isRightToLeft()))
						{
							this.CommitEdit();
							array = this.DataGridRows;
							if (rowFromY < this.DataGridRowsLength && array[rowFromY] is DataGridRelationshipRow && ((DataGridRelationshipRow)array[rowFromY]).Expanded)
							{
								this.EnsureVisible(rowFromY, 0);
							}
							this.Edit();
							return;
						}
					}
				}
				if (hitTestInfo.type == DataGrid.HitTestType.RowHeader)
				{
					this.EndEdit();
					if (!(this.DataGridRows[hitTestInfo.row] is DataGridAddNewRow))
					{
						int num = this.currentRow;
						this.CurrentCell = new DataGridCell(hitTestInfo.row, this.currentCol);
						if (hitTestInfo.row != num && this.currentRow != hitTestInfo.row && this.currentRow == num)
						{
							return;
						}
					}
					if (flag)
					{
						if (this.IsSelected(hitTestInfo.row))
						{
							this.UnSelect(hitTestInfo.row);
						}
						else
						{
							this.Select(hitTestInfo.row);
						}
					}
					else
					{
						if (this.lastRowSelected != -1 && flag2)
						{
							int num2 = Math.Min(this.lastRowSelected, hitTestInfo.row);
							int num3 = Math.Max(this.lastRowSelected, hitTestInfo.row);
							int num4 = this.lastRowSelected;
							this.ResetSelection();
							this.lastRowSelected = num4;
							DataGridRow[] array2 = this.DataGridRows;
							for (int i = num2; i <= num3; i++)
							{
								array2[i].Selected = true;
								this.numSelectedRows++;
							}
							this.EndEdit();
							return;
						}
						this.ResetSelection();
						this.Select(hitTestInfo.row);
					}
					this.lastRowSelected = hitTestInfo.row;
					return;
				}
				if (hitTestInfo.type == DataGrid.HitTestType.ParentRows)
				{
					this.EndEdit();
					this.parentRows.OnMouseDown(e.X, e.Y, this.isRightToLeft());
				}
				if (hitTestInfo.type == DataGrid.HitTestType.Cell)
				{
					if (this.myGridTable.GridColumnStyles[hitTestInfo.col].MouseDown(hitTestInfo.row, e.X, e.Y))
					{
						return;
					}
					DataGridCell dataGridCell = new DataGridCell(hitTestInfo.row, hitTestInfo.col);
					if (this.policy.AllowEdit && this.CurrentCell.Equals(dataGridCell))
					{
						this.ResetSelection();
						this.EnsureVisible(this.currentRow, this.currentCol);
						this.Edit();
						return;
					}
					this.ResetSelection();
					this.CurrentCell = dataGridCell;
				}
				return;
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x00064190 File Offset: 0x00063190
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.oldRow != -1)
			{
				DataGridRow[] array = this.DataGridRows;
				array[this.oldRow].OnMouseLeft(this.layout.RowHeaders, this.isRightToLeft());
			}
			if (this.gridState[262144])
			{
				this.caption.MouseLeft();
			}
			this.Cursor = null;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000641F6 File Offset: 0x000631F6
		internal void TextBoxOnMouseWheel(MouseEventArgs e)
		{
			this.OnMouseWheel(e);
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x00064200 File Offset: 0x00063200
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.listManager == null)
			{
				return;
			}
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			bool flag = this.isRightToLeft();
			if (this.gridState[8])
			{
				this.ColResizeMove(e);
			}
			if (this.gridState[16])
			{
				this.RowResizeMove(e);
			}
			if (this.gridState[8] || hitTestInfo.type == DataGrid.HitTestType.ColumnResize)
			{
				this.Cursor = Cursors.SizeWE;
				return;
			}
			if (this.gridState[16] || hitTestInfo.type == DataGrid.HitTestType.RowResize)
			{
				this.Cursor = Cursors.SizeNS;
				return;
			}
			this.Cursor = null;
			if (this.layout.Data.Contains(e.X, e.Y) || (this.layout.RowHeadersVisible && this.layout.RowHeaders.Contains(e.X, e.Y)))
			{
				DataGridRow[] array = this.DataGridRows;
				int rowFromY = this.GetRowFromY(e.Y);
				if (this.lastRowSelected != -1 && !this.gridState[256])
				{
					int rowTop = this.GetRowTop(this.lastRowSelected);
					int num = rowTop + array[this.lastRowSelected].Height;
					int height = SystemInformation.DragSize.Height;
					this.gridState[256] = (e.Y - rowTop < height && rowTop - e.Y < height) || (e.Y - num < height && num - e.Y < height);
				}
				if (rowFromY > -1)
				{
					Point point = this.NormalizeToRow(e.X, e.Y, rowFromY);
					if (!array[rowFromY].OnMouseMove(point.X, point.Y, this.layout.RowHeaders, flag) && this.gridState[256])
					{
						MouseButtons mouseButtons = Control.MouseButtons;
						if (this.lastRowSelected != -1 && (mouseButtons & MouseButtons.Left) == MouseButtons.Left && ((Control.ModifierKeys & Keys.Control) != Keys.Control || (Control.ModifierKeys & Keys.Alt) != Keys.None))
						{
							int num2 = this.lastRowSelected;
							this.ResetSelection();
							this.lastRowSelected = num2;
							int num3 = Math.Min(this.lastRowSelected, rowFromY);
							int num4 = Math.Max(this.lastRowSelected, rowFromY);
							DataGridRow[] array2 = this.DataGridRows;
							for (int i = num3; i <= num4; i++)
							{
								array2[i].Selected = true;
								this.numSelectedRows++;
							}
						}
					}
				}
				if (this.oldRow != rowFromY && this.oldRow != -1)
				{
					array[this.oldRow].OnMouseLeft(this.layout.RowHeaders, flag);
				}
				this.oldRow = rowFromY;
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ParentRows && this.parentRows != null)
			{
				this.parentRows.OnMouseMove(e.X, e.Y);
			}
			if (hitTestInfo.type == DataGrid.HitTestType.Caption)
			{
				this.gridState[262144] = true;
				Rectangle rectangle = this.layout.Caption;
				this.caption.MouseOver(e.X - rectangle.X, e.Y - rectangle.Y);
				return;
			}
			if (this.gridState[262144])
			{
				this.gridState[262144] = false;
				this.caption.MouseLeft();
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00064588 File Offset: 0x00063588
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.gridState[256] = false;
			if (this.listManager == null || this.myGridTable == null)
			{
				return;
			}
			if (this.gridState[8])
			{
				this.ColResizeEnd(e);
			}
			if (this.gridState[16])
			{
				this.RowResizeEnd(e);
			}
			this.gridState[8] = false;
			this.gridState[16] = false;
			DataGrid.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);
			if ((hitTestInfo.type & DataGrid.HitTestType.Caption) == DataGrid.HitTestType.Caption)
			{
				this.caption.MouseUp(e.X, e.Y);
			}
			if (hitTestInfo.type == DataGrid.HitTestType.ColumnHeader)
			{
				PropertyDescriptor propertyDescriptor = this.myGridTable.GridColumnStyles[hitTestInfo.col].PropertyDescriptor;
				if (propertyDescriptor == this.trackColumnHeader)
				{
					this.ColumnHeaderClicked(this.trackColumnHeader);
				}
			}
			this.trackColumnHeader = null;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x0006467C File Offset: 0x0006367C
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e is HandledMouseEventArgs)
			{
				if (((HandledMouseEventArgs)e).Handled)
				{
					return;
				}
				((HandledMouseEventArgs)e).Handled = true;
			}
			bool flag = true;
			if ((Control.ModifierKeys & Keys.Control) != Keys.None)
			{
				flag = false;
			}
			if (this.listManager == null || this.myGridTable == null)
			{
				return;
			}
			ScrollBar scrollBar = (flag ? this.vertScrollBar : this.horizScrollBar);
			if (!scrollBar.Visible)
			{
				return;
			}
			this.gridState[131072] = true;
			this.wheelDelta += e.Delta;
			float num = (float)this.wheelDelta / 120f;
			int num2 = (int)((float)SystemInformation.MouseWheelScrollLines * num);
			if (num2 != 0)
			{
				this.wheelDelta = 0;
				if (flag)
				{
					int num3 = this.firstVisibleRow - num2;
					num3 = Math.Max(0, Math.Min(num3, this.DataGridRowsLength - this.numTotallyVisibleRows));
					this.ScrollDown(num3 - this.firstVisibleRow);
				}
				else
				{
					int num4 = this.horizScrollBar.Value + ((num2 < 0) ? 1 : (-1)) * this.horizScrollBar.LargeChange;
					this.HorizontalOffset = num4;
				}
			}
			this.gridState[131072] = false;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000647AC File Offset: 0x000637AC
		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				this.CheckHierarchyState();
				if (this.layout.dirty)
				{
					this.ComputeLayout();
				}
				Graphics graphics = pe.Graphics;
				Region clip = graphics.Clip;
				if (this.layout.CaptionVisible)
				{
					this.caption.Paint(graphics, this.layout.Caption, this.isRightToLeft());
				}
				if (this.layout.ParentRowsVisible)
				{
					graphics.FillRectangle(SystemBrushes.AppWorkspace, this.layout.ParentRows);
					this.parentRows.Paint(graphics, this.layout.ParentRows, this.isRightToLeft());
				}
				Rectangle rectangle = this.layout.Data;
				if (this.layout.RowHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.RowHeaders);
				}
				if (this.layout.ColumnHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
				}
				graphics.SetClip(rectangle);
				this.PaintGrid(graphics, rectangle);
				graphics.Clip = clip;
				clip.Dispose();
				this.PaintBorder(graphics, this.layout.ClientRectangle);
				graphics.FillRectangle(DataGrid.DefaultHeaderBackBrush, this.layout.ResizeBoxRect);
				base.OnPaint(pe);
			}
			catch
			{
				if (this.Site == null || !this.Site.DesignMode)
				{
					throw;
				}
				this.gridState[8388608] = true;
				try
				{
					RTLAwareMessageBox.Show(null, SR.GetString("DataGridExceptionInPaint"), null, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					if (base.Visible)
					{
						base.BeginUpdateInternal();
					}
					this.ResetParentRows();
					this.Set_ListManager(null, string.Empty, true);
				}
				finally
				{
					this.gridState[8388608] = false;
					if (base.Visible)
					{
						base.EndUpdateInternal();
					}
				}
			}
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000649A0 File Offset: 0x000639A0
		protected override void OnResize(EventArgs e)
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(this.layout.Caption);
			}
			if (this.layout.ParentRowsVisible)
			{
				this.parentRows.OnResize(this.layout.ParentRows);
			}
			int borderWidth = this.BorderWidth;
			Rectangle clientRectangle = this.layout.ClientRectangle;
			Rectangle rectangle = new Rectangle(clientRectangle.X + clientRectangle.Width - borderWidth, clientRectangle.Y, borderWidth, clientRectangle.Height);
			Rectangle rectangle2 = new Rectangle(clientRectangle.X, clientRectangle.Y + clientRectangle.Height - borderWidth, clientRectangle.Width, borderWidth);
			Rectangle clientRectangle2 = base.ClientRectangle;
			if (clientRectangle2.Width != clientRectangle.Width)
			{
				base.Invalidate(rectangle);
				rectangle = new Rectangle(clientRectangle2.X + clientRectangle2.Width - borderWidth, clientRectangle2.Y, borderWidth, clientRectangle2.Height);
				base.Invalidate(rectangle);
			}
			if (clientRectangle2.Height != clientRectangle.Height)
			{
				base.Invalidate(rectangle2);
				rectangle2 = new Rectangle(clientRectangle2.X, clientRectangle2.Y + clientRectangle2.Height - borderWidth, clientRectangle2.Width, borderWidth);
				base.Invalidate(rectangle2);
			}
			if (!this.layout.ResizeBoxRect.IsEmpty)
			{
				base.Invalidate(this.layout.ResizeBoxRect);
			}
			this.layout.ClientRectangle = clientRectangle2;
			int num = this.firstVisibleRow;
			base.OnResize(e);
			if (this.isRightToLeft() || num != this.firstVisibleRow)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x00064B38 File Offset: 0x00063B38
		internal void OnRowHeightChanged(DataGridRow row)
		{
			this.ClearRegionCache();
			int rowTop = this.GetRowTop(row.RowNumber);
			if (rowTop > 0)
			{
				base.Invalidate(new Rectangle
				{
					Y = rowTop,
					X = this.layout.Inside.X,
					Width = this.layout.Inside.Width,
					Height = this.layout.Inside.Bottom - rowTop
				});
			}
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x00064BBC File Offset: 0x00063BBC
		internal void ParentRowsDataChanged()
		{
			this.parentRows.Clear();
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			object obj = this.originalState.DataSource;
			string text = this.originalState.DataMember;
			this.originalState = null;
			this.Set_ListManager(obj, text, true);
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00064C40 File Offset: 0x00063C40
		private void AbortEdit()
		{
			this.gridState[65536] = true;
			this.editColumn.Abort(this.editRow.RowNumber);
			this.gridState[65536] = false;
			this.gridState[32768] = false;
			this.editRow = null;
			this.editColumn = null;
		}

		// Token: 0x1400011B RID: 283
		// (add) Token: 0x06002820 RID: 10272 RVA: 0x00064CA4 File Offset: 0x00063CA4
		// (remove) Token: 0x06002821 RID: 10273 RVA: 0x00064CBD File Offset: 0x00063CBD
		[SRCategory("CatAction")]
		[SRDescription("DataGridNavigateEventDescr")]
		public event NavigateEventHandler Navigate
		{
			add
			{
				this.onNavigate = (NavigateEventHandler)Delegate.Combine(this.onNavigate, value);
			}
			remove
			{
				this.onNavigate = (NavigateEventHandler)Delegate.Remove(this.onNavigate, value);
			}
		}

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x06002822 RID: 10274 RVA: 0x00064CD6 File Offset: 0x00063CD6
		// (remove) Token: 0x06002823 RID: 10275 RVA: 0x00064CEF File Offset: 0x00063CEF
		protected event EventHandler RowHeaderClick
		{
			add
			{
				this.onRowHeaderClick = (EventHandler)Delegate.Combine(this.onRowHeaderClick, value);
			}
			remove
			{
				this.onRowHeaderClick = (EventHandler)Delegate.Remove(this.onRowHeaderClick, value);
			}
		}

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x06002824 RID: 10276 RVA: 0x00064D08 File Offset: 0x00063D08
		// (remove) Token: 0x06002825 RID: 10277 RVA: 0x00064D1B File Offset: 0x00063D1B
		[SRDescription("DataGridNodeClickEventDescr")]
		[SRCategory("CatAction")]
		internal event EventHandler NodeClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_NODECLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_NODECLICKED, value);
			}
		}

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x06002826 RID: 10278 RVA: 0x00064D2E File Offset: 0x00063D2E
		// (remove) Token: 0x06002827 RID: 10279 RVA: 0x00064D41 File Offset: 0x00063D41
		[SRDescription("DataGridScrollEventDescr")]
		[SRCategory("CatAction")]
		public event EventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_SCROLL, value);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x00064D54 File Offset: 0x00063D54
		// (set) Token: 0x06002829 RID: 10281 RVA: 0x00064D5C File Offset: 0x00063D5C
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				ISite site = this.Site;
				base.Site = value;
				if (value != site && !base.Disposing)
				{
					this.SubObjectsSiteChange(false);
					this.SubObjectsSiteChange(true);
				}
			}
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x00064D94 File Offset: 0x00063D94
		internal void AddNewRow()
		{
			this.EnsureBound();
			this.ResetSelection();
			this.UpdateListManager();
			this.gridState[512] = true;
			this.gridState[1048576] = true;
			try
			{
				this.ListManager.AddNew();
			}
			catch
			{
				this.gridState[512] = false;
				this.gridState[1048576] = false;
				base.PerformLayout();
				this.InvalidateInside();
				throw;
			}
			this.gridState[512] = false;
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x00064E34 File Offset: 0x00063E34
		public bool BeginEdit(DataGridColumnStyle gridColumn, int rowNumber)
		{
			if (this.DataSource == null || this.myGridTable == null)
			{
				return false;
			}
			if (this.gridState[32768])
			{
				return false;
			}
			int num;
			if ((num = this.myGridTable.GridColumnStyles.IndexOf(gridColumn)) < 0)
			{
				return false;
			}
			this.CurrentCell = new DataGridCell(rowNumber, num);
			this.ResetSelection();
			this.Edit();
			return true;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x00064E9B File Offset: 0x00063E9B
		public void BeginInit()
		{
			if (this.inInit)
			{
				throw new InvalidOperationException(SR.GetString("DataGridBeginInit"));
			}
			this.inInit = true;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x00064EBC File Offset: 0x00063EBC
		private Rectangle CalcRowResizeFeedbackRect(MouseEventArgs e)
		{
			Rectangle data = this.layout.Data;
			Rectangle rectangle = new Rectangle(data.X, e.Y, data.Width, 3);
			rectangle.Y = Math.Min(data.Bottom - 3, rectangle.Y);
			rectangle.Y = Math.Max(rectangle.Y, 0);
			return rectangle;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x00064F24 File Offset: 0x00063F24
		private Rectangle CalcColResizeFeedbackRect(MouseEventArgs e)
		{
			Rectangle data = this.layout.Data;
			Rectangle rectangle = new Rectangle(e.X, data.Y, 3, data.Height);
			rectangle.X = Math.Min(data.Right - 3, rectangle.X);
			rectangle.X = Math.Max(rectangle.X, 0);
			return rectangle;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x00064F8A File Offset: 0x00063F8A
		private void CancelCursorUpdate()
		{
			if (this.listManager != null)
			{
				this.EndEdit();
				this.listManager.CancelCurrentEdit();
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x00064FA8 File Offset: 0x00063FA8
		private void CheckHierarchyState()
		{
			if (this.checkHierarchy && this.listManager != null && this.myGridTable != null)
			{
				if (this.myGridTable == null)
				{
					return;
				}
				for (int i = 0; i < this.myGridTable.GridColumnStyles.Count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[i];
				}
				this.checkHierarchy = false;
			}
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x0006500A File Offset: 0x0006400A
		private void ClearRegionCache()
		{
			this.cachedScrollableRegion = null;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x00065014 File Offset: 0x00064014
		private void ColAutoResize(int col)
		{
			this.EndEdit();
			CurrencyManager currencyManager = this.listManager;
			if (currencyManager == null)
			{
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			try
			{
				DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[col];
				string headerText = dataGridColumnStyle.HeaderText;
				Font font;
				if (this.myGridTable.IsDefault)
				{
					font = this.HeaderFont;
				}
				else
				{
					font = this.myGridTable.HeaderFont;
				}
				int num = (int)graphics.MeasureString(headerText, font).Width + this.layout.ColumnHeaders.Height + 1;
				int count = currencyManager.Count;
				for (int i = 0; i < count; i++)
				{
					object columnValueAtRow = dataGridColumnStyle.GetColumnValueAtRow(currencyManager, i);
					int width = dataGridColumnStyle.GetPreferredSize(graphics, columnValueAtRow).Width;
					if (width > num)
					{
						num = width;
					}
				}
				if (dataGridColumnStyle.Width != num)
				{
					dataGridColumnStyle.width = num;
					this.ComputeVisibleColumns();
					bool flag = true;
					if (this.lastTotallyVisibleCol != -1)
					{
						for (int j = this.lastTotallyVisibleCol + 1; j < this.myGridTable.GridColumnStyles.Count; j++)
						{
							if (this.myGridTable.GridColumnStyles[j].PropertyDescriptor != null)
							{
								flag = false;
								break;
							}
						}
					}
					else
					{
						flag = false;
					}
					if (flag && (this.negOffset != 0 || this.horizontalOffset != 0))
					{
						dataGridColumnStyle.width = num;
						int num2 = 0;
						int count2 = this.myGridTable.GridColumnStyles.Count;
						int width2 = this.layout.Data.Width;
						GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
						this.negOffset = 0;
						this.horizontalOffset = 0;
						this.firstVisibleCol = 0;
						for (int k = count2 - 1; k >= 0; k--)
						{
							if (gridColumnStyles[k].PropertyDescriptor != null)
							{
								num2 += gridColumnStyles[k].Width;
								if (num2 > width2)
								{
									if (this.negOffset == 0)
									{
										this.firstVisibleCol = k;
										this.negOffset = num2 - width2;
										this.horizontalOffset = this.negOffset;
										this.numVisibleCols++;
									}
									else
									{
										this.horizontalOffset += gridColumnStyles[k].Width;
									}
								}
								else
								{
									this.numVisibleCols++;
								}
							}
						}
						base.PerformLayout();
						base.Invalidate(Rectangle.Union(this.layout.Data, this.layout.ColumnHeaders));
					}
					else
					{
						base.PerformLayout();
						Rectangle rectangle = this.layout.Data;
						if (this.layout.ColumnHeadersVisible)
						{
							rectangle = Rectangle.Union(rectangle, this.layout.ColumnHeaders);
						}
						int colBeg = this.GetColBeg(col);
						if (!this.isRightToLeft())
						{
							rectangle.Width -= colBeg - rectangle.X;
							rectangle.X = colBeg;
						}
						else
						{
							rectangle.Width -= colBeg;
						}
						base.Invalidate(rectangle);
					}
				}
			}
			finally
			{
				graphics.Dispose();
			}
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset;
			}
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x0006534C File Offset: 0x0006434C
		public void Collapse(int row)
		{
			this.SetRowExpansionState(row, false);
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x00065358 File Offset: 0x00064358
		private void ColResizeBegin(MouseEventArgs e, int col)
		{
			int x = e.X;
			this.EndEdit();
			Rectangle rectangle = Rectangle.Union(this.layout.ColumnHeaders, this.layout.Data);
			if (this.isRightToLeft())
			{
				rectangle.Width = this.GetColBeg(col) - this.layout.Data.X - 2;
			}
			else
			{
				int colBeg = this.GetColBeg(col);
				rectangle.X = colBeg + 3;
				rectangle.Width = this.layout.Data.X + this.layout.Data.Width - colBeg - 2;
			}
			base.CaptureInternal = true;
			Cursor.ClipInternal = base.RectangleToScreen(rectangle);
			this.gridState[8] = true;
			this.trackColAnchor = x;
			this.trackColumn = col;
			this.DrawColSplitBar(e);
			this.lastSplitBar = e;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x00065433 File Offset: 0x00064433
		private void ColResizeMove(MouseEventArgs e)
		{
			if (this.lastSplitBar != null)
			{
				this.DrawColSplitBar(this.lastSplitBar);
				this.lastSplitBar = e;
			}
			this.DrawColSplitBar(e);
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x00065458 File Offset: 0x00064458
		private void ColResizeEnd(MouseEventArgs e)
		{
			this.gridState[16777216] = true;
			try
			{
				if (this.lastSplitBar != null)
				{
					this.DrawColSplitBar(this.lastSplitBar);
					this.lastSplitBar = null;
				}
				bool flag = this.isRightToLeft();
				int num = (flag ? Math.Max(e.X, this.layout.Data.X) : Math.Min(e.X, this.layout.Data.Right + 1));
				int num2 = num - this.GetColEnd(this.trackColumn);
				if (flag)
				{
					num2 = -num2;
				}
				if (this.trackColAnchor != num && num2 != 0)
				{
					DataGridColumnStyle dataGridColumnStyle = this.myGridTable.GridColumnStyles[this.trackColumn];
					int num3 = dataGridColumnStyle.Width + num2;
					num3 = Math.Max(num3, 3);
					dataGridColumnStyle.Width = num3;
					this.ComputeVisibleColumns();
					bool flag2 = true;
					for (int i = this.lastTotallyVisibleCol + 1; i < this.myGridTable.GridColumnStyles.Count; i++)
					{
						if (this.myGridTable.GridColumnStyles[i].PropertyDescriptor != null)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2 && (this.negOffset != 0 || this.horizontalOffset != 0))
					{
						int num4 = 0;
						int count = this.myGridTable.GridColumnStyles.Count;
						int width = this.layout.Data.Width;
						GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
						this.negOffset = 0;
						this.horizontalOffset = 0;
						this.firstVisibleCol = 0;
						for (int j = count - 1; j > -1; j--)
						{
							if (gridColumnStyles[j].PropertyDescriptor != null)
							{
								num4 += gridColumnStyles[j].Width;
								if (num4 > width)
								{
									if (this.negOffset == 0)
									{
										this.negOffset = num4 - width;
										this.firstVisibleCol = j;
										this.horizontalOffset = this.negOffset;
										this.numVisibleCols++;
									}
									else
									{
										this.horizontalOffset += gridColumnStyles[j].Width;
									}
								}
								else
								{
									this.numVisibleCols++;
								}
							}
						}
						base.Invalidate(Rectangle.Union(this.layout.Data, this.layout.ColumnHeaders));
					}
					else
					{
						Rectangle rectangle = Rectangle.Union(this.layout.ColumnHeaders, this.layout.Data);
						int colBeg = this.GetColBeg(this.trackColumn);
						rectangle.Width -= (flag ? (rectangle.Right - colBeg) : (colBeg - rectangle.X));
						rectangle.X = (flag ? this.layout.Data.X : colBeg);
						base.Invalidate(rectangle);
					}
				}
			}
			finally
			{
				Cursor.ClipInternal = Rectangle.Empty;
				base.CaptureInternal = false;
				this.gridState[16777216] = false;
			}
			base.PerformLayout();
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset;
			}
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x00065784 File Offset: 0x00064784
		private void MetaDataChanged()
		{
			this.parentRows.Clear();
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
			this.gridState[4194304] = true;
			try
			{
				if (this.originalState != null)
				{
					this.Set_ListManager(this.originalState.DataSource, this.originalState.DataMember, true);
					this.originalState = null;
				}
				else
				{
					this.Set_ListManager(this.DataSource, this.DataMember, true);
				}
			}
			finally
			{
				this.gridState[4194304] = false;
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x00065854 File Offset: 0x00064854
		private void RowAutoResize(int row)
		{
			this.EndEdit();
			CurrencyManager currencyManager = this.ListManager;
			if (currencyManager == null)
			{
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			try
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				DataGridRow dataGridRow = this.DataGridRows[row];
				int count = currencyManager.Count;
				int num = 0;
				int count2 = gridColumnStyles.Count;
				for (int i = 0; i < count2; i++)
				{
					object columnValueAtRow = gridColumnStyles[i].GetColumnValueAtRow(currencyManager, row);
					num = Math.Max(num, gridColumnStyles[i].GetPreferredHeight(graphics, columnValueAtRow));
				}
				if (dataGridRow.Height != num)
				{
					dataGridRow.Height = num;
					base.PerformLayout();
					Rectangle rectangle = this.layout.Data;
					if (this.layout.RowHeadersVisible)
					{
						rectangle = Rectangle.Union(rectangle, this.layout.RowHeaders);
					}
					int rowTop = this.GetRowTop(row);
					rectangle.Height -= rectangle.Y - rowTop;
					rectangle.Y = rowTop;
					base.Invalidate(rectangle);
				}
			}
			finally
			{
				graphics.Dispose();
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x0006596C File Offset: 0x0006496C
		private void RowResizeBegin(MouseEventArgs e, int row)
		{
			int y = e.Y;
			this.EndEdit();
			Rectangle rectangle = Rectangle.Union(this.layout.RowHeaders, this.layout.Data);
			int rowTop = this.GetRowTop(row);
			rectangle.Y = rowTop + 3;
			rectangle.Height = this.layout.Data.Y + this.layout.Data.Height - rowTop - 2;
			base.CaptureInternal = true;
			Cursor.ClipInternal = base.RectangleToScreen(rectangle);
			this.gridState[16] = true;
			this.trackRowAnchor = y;
			this.trackRow = row;
			this.DrawRowSplitBar(e);
			this.lastSplitBar = e;
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x00065A1D File Offset: 0x00064A1D
		private void RowResizeMove(MouseEventArgs e)
		{
			if (this.lastSplitBar != null)
			{
				this.DrawRowSplitBar(this.lastSplitBar);
				this.lastSplitBar = e;
			}
			this.DrawRowSplitBar(e);
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x00065A44 File Offset: 0x00064A44
		private void RowResizeEnd(MouseEventArgs e)
		{
			try
			{
				if (this.lastSplitBar != null)
				{
					this.DrawRowSplitBar(this.lastSplitBar);
					this.lastSplitBar = null;
				}
				int num = Math.Min(e.Y, this.layout.Data.Y + this.layout.Data.Height + 1);
				int num2 = num - this.GetRowBottom(this.trackRow);
				if (this.trackRowAnchor != num && num2 != 0)
				{
					DataGridRow dataGridRow = this.DataGridRows[this.trackRow];
					int num3 = dataGridRow.Height + num2;
					num3 = Math.Max(num3, 3);
					dataGridRow.Height = num3;
					base.PerformLayout();
					Rectangle rectangle = Rectangle.Union(this.layout.RowHeaders, this.layout.Data);
					int rowTop = this.GetRowTop(this.trackRow);
					rectangle.Height -= rectangle.Y - rowTop;
					rectangle.Y = rowTop;
					base.Invalidate(rectangle);
				}
			}
			finally
			{
				Cursor.ClipInternal = Rectangle.Empty;
				base.CaptureInternal = false;
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x00065B60 File Offset: 0x00064B60
		private void ColumnHeaderClicked(PropertyDescriptor prop)
		{
			if (!this.CommitEdit())
			{
				return;
			}
			bool flag;
			if (this.myGridTable.IsDefault)
			{
				flag = this.AllowSorting;
			}
			else
			{
				flag = this.myGridTable.AllowSorting;
			}
			if (!flag)
			{
				return;
			}
			ListSortDirection listSortDirection = this.ListManager.GetSortDirection();
			PropertyDescriptor sortProperty = this.ListManager.GetSortProperty();
			if (sortProperty != null && sortProperty.Equals(prop))
			{
				listSortDirection = ((listSortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
			}
			else
			{
				listSortDirection = ListSortDirection.Ascending;
			}
			if (this.listManager.Count == 0)
			{
				return;
			}
			this.ListManager.SetSort(prop, listSortDirection);
			this.ResetSelection();
			this.InvalidateInside();
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00065BF4 File Offset: 0x00064BF4
		private bool CommitEdit()
		{
			if ((!this.gridState[32768] && !this.gridState[16384]) || (this.gridState[65536] && !this.gridState[131072]))
			{
				return true;
			}
			this.gridState[65536] = true;
			if (this.editColumn.ReadOnly || this.gridState[1048576])
			{
				bool flag = false;
				if (base.ContainsFocus)
				{
					flag = true;
				}
				if (flag && this.gridState[2048])
				{
					this.FocusInternal();
				}
				this.editColumn.ConcedeFocus();
				if (flag && this.gridState[2048] && base.CanFocus && !this.Focused)
				{
					this.FocusInternal();
				}
				this.gridState[65536] = false;
				return true;
			}
			bool flag2 = this.editColumn.Commit(this.ListManager, this.currentRow);
			this.gridState[65536] = false;
			if (flag2)
			{
				this.gridState[32768] = false;
			}
			return flag2;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00065D2C File Offset: 0x00064D2C
		private int ComputeDeltaRows(int targetRow)
		{
			if (this.firstVisibleRow == targetRow)
			{
				return 0;
			}
			int num = -1;
			int num2 = -1;
			int num3 = this.DataGridRowsLength;
			int num4 = 0;
			DataGridRow[] array = this.DataGridRows;
			for (int i = 0; i < num3; i++)
			{
				if (i == this.firstVisibleRow)
				{
					num = num4;
				}
				if (i == targetRow)
				{
					num2 = num4;
				}
				if (num2 != -1 && num != -1)
				{
					break;
				}
				num4 += array[i].Height;
			}
			int num5 = num2 + array[targetRow].Height;
			int num6 = this.layout.Data.Height + num;
			if (num5 > num6)
			{
				int num7 = num5 - num6;
				num += num7;
			}
			else
			{
				if (num < num2)
				{
					return 0;
				}
				int num8 = num - num2;
				num -= num8;
			}
			int num9 = this.ComputeFirstVisibleRow(num);
			return num9 - this.firstVisibleRow;
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x00065DF4 File Offset: 0x00064DF4
		private int ComputeFirstVisibleRow(int firstVisibleRowLogicalTop)
		{
			int num = this.DataGridRowsLength;
			int num2 = 0;
			DataGridRow[] array = this.DataGridRows;
			int num3 = 0;
			while (num3 < num && num2 < firstVisibleRowLogicalTop)
			{
				num2 += array[num3].Height;
				num3++;
			}
			return num3;
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x00065E30 File Offset: 0x00064E30
		private void ComputeLayout()
		{
			bool flag = !this.isRightToLeft();
			Rectangle resizeBoxRect = this.layout.ResizeBoxRect;
			this.EndEdit();
			this.ClearRegionCache();
			DataGrid.LayoutData layoutData = new DataGrid.LayoutData(this.layout);
			layoutData.Inside = base.ClientRectangle;
			Rectangle inside = layoutData.Inside;
			int borderWidth = this.BorderWidth;
			inside.Inflate(-borderWidth, -borderWidth);
			Rectangle rectangle = inside;
			if (this.layout.CaptionVisible)
			{
				int num = this.captionFontHeight + 6;
				Rectangle rectangle2 = layoutData.Caption;
				rectangle2 = rectangle;
				rectangle2.Height = num;
				rectangle.Y += num;
				rectangle.Height -= num;
				layoutData.Caption = rectangle2;
			}
			else
			{
				layoutData.Caption = Rectangle.Empty;
			}
			if (this.layout.ParentRowsVisible)
			{
				Rectangle rectangle3 = layoutData.ParentRows;
				int height = this.parentRows.Height;
				rectangle3 = rectangle;
				rectangle3.Height = height;
				rectangle.Y += height;
				rectangle.Height -= height;
				layoutData.ParentRows = rectangle3;
			}
			else
			{
				layoutData.ParentRows = Rectangle.Empty;
			}
			int num2 = this.headerFontHeight + 6;
			if (this.layout.ColumnHeadersVisible)
			{
				Rectangle rectangle4 = layoutData.ColumnHeaders;
				rectangle4 = rectangle;
				rectangle4.Height = num2;
				rectangle.Y += num2;
				rectangle.Height -= num2;
				layoutData.ColumnHeaders = rectangle4;
			}
			else
			{
				layoutData.ColumnHeaders = Rectangle.Empty;
			}
			bool flag2 = (this.myGridTable.IsDefault ? this.RowHeadersVisible : this.myGridTable.RowHeadersVisible);
			int num3 = (this.myGridTable.IsDefault ? this.RowHeaderWidth : this.myGridTable.RowHeaderWidth);
			layoutData.RowHeadersVisible = flag2;
			if (this.myGridTable != null && flag2)
			{
				Rectangle rectangle5 = layoutData.RowHeaders;
				if (flag)
				{
					rectangle5 = rectangle;
					rectangle5.Width = num3;
					rectangle.X += num3;
					rectangle.Width -= num3;
				}
				else
				{
					rectangle5 = rectangle;
					rectangle5.Width = num3;
					rectangle5.X = rectangle.Right - num3;
					rectangle.Width -= num3;
				}
				layoutData.RowHeaders = rectangle5;
				if (this.layout.ColumnHeadersVisible)
				{
					Rectangle rectangle6 = layoutData.TopLeftHeader;
					Rectangle columnHeaders = layoutData.ColumnHeaders;
					if (flag)
					{
						rectangle6 = columnHeaders;
						rectangle6.Width = num3;
						columnHeaders.Width -= num3;
						columnHeaders.X += num3;
					}
					else
					{
						rectangle6 = columnHeaders;
						rectangle6.Width = num3;
						rectangle6.X = columnHeaders.Right - num3;
						columnHeaders.Width -= num3;
					}
					layoutData.TopLeftHeader = rectangle6;
					layoutData.ColumnHeaders = columnHeaders;
				}
				else
				{
					layoutData.TopLeftHeader = Rectangle.Empty;
				}
			}
			else
			{
				layoutData.RowHeaders = Rectangle.Empty;
				layoutData.TopLeftHeader = Rectangle.Empty;
			}
			layoutData.Data = rectangle;
			layoutData.Inside = inside;
			this.layout = layoutData;
			this.LayoutScrollBars();
			if (!resizeBoxRect.Equals(this.layout.ResizeBoxRect) && !this.layout.ResizeBoxRect.IsEmpty)
			{
				base.Invalidate(this.layout.ResizeBoxRect);
			}
			this.layout.dirty = false;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000661AC File Offset: 0x000651AC
		private int ComputeRowDelta(int from, int to)
		{
			int num = from;
			int num2 = to;
			int num3 = -1;
			if (num > num2)
			{
				num = to;
				num2 = from;
				num3 = 1;
			}
			DataGridRow[] array = this.DataGridRows;
			int num4 = 0;
			for (int i = num; i < num2; i++)
			{
				num4 += array[i].Height;
			}
			return num3 * num4;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000661F5 File Offset: 0x000651F5
		internal int MinimumRowHeaderWidth()
		{
			return this.minRowHeaderWidth;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x00066200 File Offset: 0x00065200
		internal void ComputeMinimumRowHeaderWidth()
		{
			this.minRowHeaderWidth = 15;
			if (this.ListHasErrors)
			{
				this.minRowHeaderWidth += 15;
			}
			if (this.myGridTable != null && this.myGridTable.RelationsList.Count != 0)
			{
				this.minRowHeaderWidth += 15;
			}
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x00066258 File Offset: 0x00065258
		private void ComputeVisibleColumns()
		{
			this.EnsureBound();
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num = -this.negOffset;
			int num2 = 0;
			int width = this.layout.Data.Width;
			int num3 = this.firstVisibleCol;
			if (width < 0 || gridColumnStyles.Count == 0)
			{
				this.numVisibleCols = (this.firstVisibleCol = 0);
				this.lastTotallyVisibleCol = -1;
				return;
			}
			while (num < width && num3 < count)
			{
				if (gridColumnStyles[num3].PropertyDescriptor != null)
				{
					num += gridColumnStyles[num3].Width;
				}
				num3++;
				num2++;
			}
			this.numVisibleCols = num2;
			if (num < width)
			{
				int num4 = this.firstVisibleCol - 1;
				while (num4 > 0 && num + gridColumnStyles[num4].Width <= width)
				{
					if (gridColumnStyles[num4].PropertyDescriptor != null)
					{
						num += gridColumnStyles[num4].Width;
					}
					num2++;
					this.firstVisibleCol--;
					num4--;
				}
				if (this.numVisibleCols != num2)
				{
					base.Invalidate(this.layout.Data);
					base.Invalidate(this.layout.ColumnHeaders);
					this.numVisibleCols = num2;
				}
			}
			this.lastTotallyVisibleCol = this.firstVisibleCol + this.numVisibleCols - 1;
			if (num > width)
			{
				if (this.numVisibleCols <= 1 || (this.numVisibleCols == 2 && this.negOffset != 0))
				{
					this.lastTotallyVisibleCol = -1;
					return;
				}
				this.lastTotallyVisibleCol--;
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000663E4 File Offset: 0x000653E4
		private int ComputeFirstVisibleColumn()
		{
			int i = 0;
			if (this.horizontalOffset == 0)
			{
				this.negOffset = 0;
				return 0;
			}
			if (this.myGridTable != null && this.myGridTable.GridColumnStyles != null && this.myGridTable.GridColumnStyles.Count != 0)
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int num = 0;
				int count = gridColumnStyles.Count;
				if (gridColumnStyles[0].Width == -1)
				{
					this.negOffset = 0;
					return 0;
				}
				for (i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
					if (num > this.horizontalOffset)
					{
						break;
					}
				}
				if (i == count)
				{
					this.negOffset = 0;
					return 0;
				}
				this.negOffset = gridColumnStyles[i].Width - (num - this.horizontalOffset);
			}
			return i;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000664BC File Offset: 0x000654BC
		private void ComputeVisibleRows()
		{
			this.EnsureBound();
			Rectangle data = this.layout.Data;
			int height = data.Height;
			int num = 0;
			int num2 = 0;
			DataGridRow[] array = this.DataGridRows;
			int num3 = this.DataGridRowsLength;
			if (height < 0)
			{
				this.numVisibleRows = (this.numTotallyVisibleRows = 0);
				return;
			}
			int num4 = this.firstVisibleRow;
			while (num4 < num3 && num <= height)
			{
				num += array[num4].Height;
				num2++;
				num4++;
			}
			if (num < height)
			{
				for (int i = this.firstVisibleRow - 1; i >= 0; i--)
				{
					int height2 = array[i].Height;
					if (num + height2 > height)
					{
						break;
					}
					num += height2;
					this.firstVisibleRow--;
					num2++;
				}
			}
			this.numVisibleRows = (this.numTotallyVisibleRows = num2);
			if (num > height)
			{
				this.numTotallyVisibleRows--;
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000665A2 File Offset: 0x000655A2
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGrid.DataGridAccessibleObject(this);
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000665AC File Offset: 0x000655AC
		private DataGridState CreateChildState(string relationName, DataGridRow source)
		{
			DataGridState dataGridState = new DataGridState();
			string text;
			if (string.IsNullOrEmpty(this.DataMember))
			{
				text = relationName;
			}
			else
			{
				text = this.DataMember + "." + relationName;
			}
			CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.DataSource, text];
			dataGridState.DataSource = this.DataSource;
			dataGridState.DataMember = text;
			dataGridState.ListManager = currencyManager;
			dataGridState.DataGridRows = null;
			dataGridState.DataGridRowsLength = currencyManager.Count + (this.policy.AllowAdd ? 1 : 0);
			return dataGridState;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x0006663C File Offset: 0x0006563C
		private DataGrid.LayoutData CreateInitialLayoutState()
		{
			return new DataGrid.LayoutData
			{
				Inside = default(Rectangle),
				TopLeftHeader = default(Rectangle),
				ColumnHeaders = default(Rectangle),
				RowHeaders = default(Rectangle),
				Data = default(Rectangle),
				Caption = default(Rectangle),
				ParentRows = default(Rectangle),
				ResizeBoxRect = default(Rectangle),
				ColumnHeadersVisible = true,
				RowHeadersVisible = true,
				CaptionVisible = true,
				ParentRowsVisible = true,
				ClientRectangle = base.ClientRectangle
			};
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000666D8 File Offset: 0x000656D8
		private NativeMethods.RECT[] CreateScrollableRegion(Rectangle scroll)
		{
			if (this.cachedScrollableRegion != null)
			{
				return this.cachedScrollableRegion;
			}
			bool flag = this.isRightToLeft();
			using (Region region = new Region(scroll))
			{
				int num = this.numVisibleRows;
				int num2 = this.layout.Data.Y;
				int x = this.layout.Data.X;
				DataGridRow[] array = this.DataGridRows;
				for (int i = this.firstVisibleRow; i < num; i++)
				{
					int height = array[i].Height;
					Rectangle nonScrollableArea = array[i].GetNonScrollableArea();
					nonScrollableArea.X += x;
					nonScrollableArea.X = this.MirrorRectangle(nonScrollableArea, this.layout.Data, flag);
					if (!nonScrollableArea.IsEmpty)
					{
						region.Exclude(new Rectangle(nonScrollableArea.X, nonScrollableArea.Y + num2, nonScrollableArea.Width, nonScrollableArea.Height));
					}
					num2 += height;
				}
				using (Graphics graphics = base.CreateGraphicsInternal())
				{
					IntPtr hrgn = region.GetHrgn(graphics);
					if (hrgn != IntPtr.Zero)
					{
						this.cachedScrollableRegion = UnsafeNativeMethods.GetRectsFromRegion(hrgn);
						IntSecurity.ObjectFromWin32Handle.Assert();
						try
						{
							region.ReleaseHrgn(hrgn);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
			return this.cachedScrollableRegion;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x0006687C File Offset: 0x0006587C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.vertScrollBar != null)
				{
					this.vertScrollBar.Dispose();
				}
				if (this.horizScrollBar != null)
				{
					this.horizScrollBar.Dispose();
				}
				if (this.toBeDisposedEditingControl != null)
				{
					this.toBeDisposedEditingControl.Dispose();
					this.toBeDisposedEditingControl = null;
				}
				GridTableStylesCollection tableStyles = this.TableStyles;
				if (tableStyles != null)
				{
					for (int i = 0; i < tableStyles.Count; i++)
					{
						tableStyles[i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000668FC File Offset: 0x000658FC
		private void DrawColSplitBar(MouseEventArgs e)
		{
			Rectangle rectangle = this.CalcColResizeFeedbackRect(e);
			this.DrawSplitBar(rectangle);
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x00066918 File Offset: 0x00065918
		private void DrawRowSplitBar(MouseEventArgs e)
		{
			Rectangle rectangle = this.CalcRowResizeFeedbackRect(e);
			this.DrawSplitBar(rectangle);
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x00066934 File Offset: 0x00065934
		private void DrawSplitBar(Rectangle r)
		{
			IntPtr handle = base.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr intPtr = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, intPtr));
			SafeNativeMethods.PatBlt(new HandleRef(this, dcex), r.X, r.Y, r.Width, r.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, intPtr2));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this, handle), new HandleRef(this, dcex));
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000669DC File Offset: 0x000659DC
		private void Edit()
		{
			this.Edit(null);
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000669E8 File Offset: 0x000659E8
		private void Edit(string displayText)
		{
			this.EnsureBound();
			bool flag = true;
			this.EndEdit();
			DataGridRow[] array = this.DataGridRows;
			if (this.DataGridRowsLength == 0)
			{
				return;
			}
			array[this.currentRow].OnEdit();
			this.editRow = array[this.currentRow];
			if (this.myGridTable.GridColumnStyles.Count == 0)
			{
				return;
			}
			this.editColumn = this.myGridTable.GridColumnStyles[this.currentCol];
			if (this.editColumn.PropertyDescriptor == null)
			{
				return;
			}
			Rectangle rectangle = Rectangle.Empty;
			if (this.currentRow < this.firstVisibleRow || this.currentRow > this.firstVisibleRow + this.numVisibleRows || this.currentCol < this.firstVisibleCol || this.currentCol > this.firstVisibleCol + this.numVisibleCols - 1 || (this.currentCol == this.firstVisibleCol && this.negOffset != 0))
			{
				flag = false;
			}
			else
			{
				rectangle = this.GetCellBounds(this.currentRow, this.currentCol);
			}
			this.gridState[16384] = true;
			this.gridState[32768] = false;
			this.gridState[65536] = true;
			this.editColumn.Edit(this.ListManager, this.currentRow, rectangle, this.myGridTable.ReadOnly || this.ReadOnly || !this.policy.AllowEdit, displayText, flag);
			this.gridState[65536] = false;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00066B6C File Offset: 0x00065B6C
		public bool EndEdit(DataGridColumnStyle gridColumn, int rowNumber, bool shouldAbort)
		{
			bool flag = false;
			if (this.gridState[32768])
			{
				DataGridColumnStyle dataGridColumnStyle = this.editColumn;
				int rowNumber2 = this.editRow.RowNumber;
				if (shouldAbort)
				{
					this.AbortEdit();
					flag = true;
				}
				else
				{
					flag = this.CommitEdit();
				}
			}
			return flag;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00066BB9 File Offset: 0x00065BB9
		private void EndEdit()
		{
			if (!this.gridState[32768] && !this.gridState[16384])
			{
				return;
			}
			if (!this.CommitEdit())
			{
				this.AbortEdit();
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00066BF0 File Offset: 0x00065BF0
		private void EnforceValidDataMember(object value)
		{
			if (this.DataMember == null || this.DataMember.Length == 0)
			{
				return;
			}
			if (this.BindingContext == null)
			{
				return;
			}
			try
			{
				BindingManagerBase bindingManagerBase = this.BindingContext[value, this.dataMember];
			}
			catch
			{
				this.dataMember = "";
			}
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00066C50 File Offset: 0x00065C50
		protected internal virtual void ColumnStartedEditing(Rectangle bounds)
		{
			DataGridRow[] array = this.DataGridRows;
			if (bounds.IsEmpty && this.editColumn is DataGridTextBoxColumn && this.currentRow != -1 && this.currentCol != -1)
			{
				DataGridTextBoxColumn dataGridTextBoxColumn = this.editColumn as DataGridTextBoxColumn;
				Rectangle cellBounds = this.GetCellBounds(this.currentRow, this.currentCol);
				this.gridState[65536] = true;
				try
				{
					dataGridTextBoxColumn.TextBox.Bounds = cellBounds;
				}
				finally
				{
					this.gridState[65536] = false;
				}
			}
			if (this.gridState[1048576])
			{
				int num = this.DataGridRowsLength;
				DataGridRow[] array2 = new DataGridRow[num + 1];
				for (int i = 0; i < num; i++)
				{
					array2[i] = array[i];
				}
				array2[num] = new DataGridAddNewRow(this, this.myGridTable, num);
				this.SetDataGridRows(array2, num + 1);
				this.Edit();
				this.gridState[1048576] = false;
				this.gridState[32768] = true;
				this.gridState[16384] = false;
				return;
			}
			this.gridState[32768] = true;
			this.gridState[16384] = false;
			this.InvalidateRowHeader(this.currentRow);
			array[this.currentRow].LoseChildFocus(this.layout.RowHeaders, this.isRightToLeft());
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x00066DD0 File Offset: 0x00065DD0
		protected internal virtual void ColumnStartedEditing(Control editingControl)
		{
			this.ColumnStartedEditing(editingControl.Bounds);
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x00066DDE File Offset: 0x00065DDE
		public void Expand(int row)
		{
			this.SetRowExpansionState(row, true);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x00066DE8 File Offset: 0x00065DE8
		protected virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop, bool isDefault)
		{
			if (this.myGridTable != null)
			{
				return this.myGridTable.CreateGridColumn(prop, isDefault);
			}
			return null;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00066E01 File Offset: 0x00065E01
		protected virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop)
		{
			if (this.myGridTable != null)
			{
				return this.myGridTable.CreateGridColumn(prop);
			}
			return null;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00066E1C File Offset: 0x00065E1C
		public void EndInit()
		{
			this.inInit = false;
			if (this.myGridTable == null && this.ListManager != null)
			{
				this.SetDataGridTable(this.TableStyles[this.ListManager.GetListName()], true);
			}
			if (this.myGridTable != null)
			{
				this.myGridTable.DataGrid = this;
			}
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00066E74 File Offset: 0x00065E74
		private int GetColFromX(int x)
		{
			if (this.myGridTable == null)
			{
				return -1;
			}
			Rectangle data = this.layout.Data;
			x = this.MirrorPoint(x, data, this.isRightToLeft());
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num = data.X - this.negOffset;
			int num2 = this.firstVisibleCol;
			while (num < data.Width + data.X && num2 < count)
			{
				if (gridColumnStyles[num2].PropertyDescriptor != null)
				{
					num += gridColumnStyles[num2].Width;
				}
				if (num > x)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00066F18 File Offset: 0x00065F18
		internal int GetColBeg(int col)
		{
			int num = this.layout.Data.X - this.negOffset;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num2 = Math.Min(col, gridColumnStyles.Count);
			for (int i = this.firstVisibleCol; i < num2; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					num += gridColumnStyles[i].Width;
				}
			}
			return this.MirrorPoint(num, this.layout.Data, this.isRightToLeft());
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00066F9C File Offset: 0x00065F9C
		internal int GetColEnd(int col)
		{
			int colBeg = this.GetColBeg(col);
			int width = this.myGridTable.GridColumnStyles[col].Width;
			if (!this.isRightToLeft())
			{
				return colBeg + width;
			}
			return colBeg - width;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00066FD8 File Offset: 0x00065FD8
		private int GetColumnWidthSum()
		{
			int num = 0;
			if (this.myGridTable != null && this.myGridTable.GridColumnStyles != null)
			{
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int count = gridColumnStyles.Count;
				for (int i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num += gridColumnStyles[i].Width;
					}
				}
			}
			return num;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x0006703C File Offset: 0x0006603C
		private DataGridRelationshipRow[] GetExpandableRows()
		{
			int num = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			if (this.policy.AllowAdd)
			{
				num = Math.Max(num - 1, 0);
			}
			DataGridRelationshipRow[] array2 = new DataGridRelationshipRow[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (DataGridRelationshipRow)array[i];
			}
			return array2;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x00067090 File Offset: 0x00066090
		private int GetRowFromY(int y)
		{
			Rectangle data = this.layout.Data;
			int num = data.Y;
			int num2 = this.firstVisibleRow;
			int num3 = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			int bottom = data.Bottom;
			while (num < bottom && num2 < num3)
			{
				num += array[num2].Height;
				if (num > y)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000670F2 File Offset: 0x000660F2
		internal Rectangle GetRowHeaderRect()
		{
			return this.layout.RowHeaders;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000670FF File Offset: 0x000660FF
		internal Rectangle GetColumnHeadersRect()
		{
			return this.layout.ColumnHeaders;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x0006710C File Offset: 0x0006610C
		private Rectangle GetRowRect(int rowNumber)
		{
			Rectangle data = this.layout.Data;
			int num = data.Y;
			DataGridRow[] array = this.DataGridRows;
			int num2 = this.firstVisibleRow;
			while (num2 <= rowNumber && num <= data.Bottom)
			{
				if (num2 == rowNumber)
				{
					Rectangle rectangle = new Rectangle(data.X, num, data.Width, array[num2].Height);
					if (this.layout.RowHeadersVisible)
					{
						rectangle.Width += this.layout.RowHeaders.Width;
						rectangle.X -= (this.isRightToLeft() ? 0 : this.layout.RowHeaders.Width);
					}
					return rectangle;
				}
				num += array[num2].Height;
				num2++;
			}
			return Rectangle.Empty;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000671E4 File Offset: 0x000661E4
		private int GetRowTop(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			int num = this.layout.Data.Y;
			int num2 = Math.Min(row, this.DataGridRowsLength);
			for (int i = this.firstVisibleRow; i < num2; i++)
			{
				num += array[i].Height;
			}
			for (int j = this.firstVisibleRow; j > num2; j--)
			{
				num -= array[j].Height;
			}
			return num;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00067254 File Offset: 0x00066254
		private int GetRowBottom(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			return this.GetRowTop(row) + array[row].Height;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x00067278 File Offset: 0x00066278
		private void EnsureBound()
		{
			if (!this.Bound)
			{
				throw new InvalidOperationException(SR.GetString("DataGridUnbound"));
			}
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00067294 File Offset: 0x00066294
		private void EnsureVisible(int row, int col)
		{
			if (row < this.firstVisibleRow || row >= this.firstVisibleRow + this.numTotallyVisibleRows)
			{
				int num = this.ComputeDeltaRows(row);
				this.ScrollDown(num);
			}
			if (this.firstVisibleCol == 0 && this.numVisibleCols == 0 && this.lastTotallyVisibleCol == -1)
			{
				return;
			}
			int num2 = this.firstVisibleCol;
			int num3 = this.negOffset;
			int num4 = this.lastTotallyVisibleCol;
			while (col < this.firstVisibleCol || (col == this.firstVisibleCol && this.negOffset != 0) || (this.lastTotallyVisibleCol == -1 && col > this.firstVisibleCol) || (this.lastTotallyVisibleCol > -1 && col > this.lastTotallyVisibleCol))
			{
				this.ScrollToColumn(col);
				if (num2 == this.firstVisibleCol && num3 == this.negOffset && num4 == this.lastTotallyVisibleCol)
				{
					return;
				}
				num2 = this.firstVisibleCol;
				num3 = this.negOffset;
				num4 = this.lastTotallyVisibleCol;
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00067370 File Offset: 0x00066370
		public Rectangle GetCurrentCellBounds()
		{
			DataGridCell currentCell = this.CurrentCell;
			return this.GetCellBounds(currentCell.RowNumber, currentCell.ColumnNumber);
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x00067398 File Offset: 0x00066398
		public Rectangle GetCellBounds(int row, int col)
		{
			DataGridRow[] array = this.DataGridRows;
			Rectangle cellBounds = array[row].GetCellBounds(col);
			cellBounds.Y += this.GetRowTop(row);
			cellBounds.X += this.layout.Data.X - this.negOffset;
			cellBounds.X = this.MirrorRectangle(cellBounds, this.layout.Data, this.isRightToLeft());
			return cellBounds;
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x00067410 File Offset: 0x00066410
		public Rectangle GetCellBounds(DataGridCell dgc)
		{
			return this.GetCellBounds(dgc.RowNumber, dgc.ColumnNumber);
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x00067428 File Offset: 0x00066428
		internal Rectangle GetRowBounds(DataGridRow row)
		{
			return new Rectangle
			{
				Y = this.GetRowTop(row.RowNumber),
				X = this.layout.Data.X,
				Height = row.Height,
				Width = this.layout.Data.Width
			};
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x0006748C File Offset: 0x0006648C
		public DataGrid.HitTestInfo HitTest(int x, int y)
		{
			int y2 = this.layout.Data.Y;
			DataGrid.HitTestInfo hitTestInfo = new DataGrid.HitTestInfo();
			if (this.layout.CaptionVisible && this.layout.Caption.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.Caption;
				return hitTestInfo;
			}
			if (this.layout.ParentRowsVisible && this.layout.ParentRows.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.ParentRows;
				return hitTestInfo;
			}
			if (!this.layout.Inside.Contains(x, y))
			{
				return hitTestInfo;
			}
			if (this.layout.TopLeftHeader.Contains(x, y))
			{
				return hitTestInfo;
			}
			if (this.layout.ColumnHeaders.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.ColumnHeader;
				hitTestInfo.col = this.GetColFromX(x);
				if (hitTestInfo.col < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				int colBeg = this.GetColBeg(hitTestInfo.col + 1);
				bool flag = this.isRightToLeft();
				if ((flag && x - colBeg < 8) || (!flag && colBeg - x < 8))
				{
					hitTestInfo.type = DataGrid.HitTestType.ColumnResize;
				}
				if (!this.allowColumnResize)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
			else if (this.layout.RowHeaders.Contains(x, y))
			{
				hitTestInfo.type = DataGrid.HitTestType.RowHeader;
				hitTestInfo.row = this.GetRowFromY(y);
				if (hitTestInfo.row < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				DataGridRow[] array = this.DataGridRows;
				int num = this.GetRowTop(hitTestInfo.row) + array[hitTestInfo.row].Height;
				if (num - y - this.BorderWidth < 2 && !(array[hitTestInfo.row] is DataGridAddNewRow))
				{
					hitTestInfo.type = DataGrid.HitTestType.RowResize;
				}
				if (!this.allowRowResize)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
			else
			{
				if (!this.layout.Data.Contains(x, y))
				{
					return hitTestInfo;
				}
				hitTestInfo.type = DataGrid.HitTestType.Cell;
				hitTestInfo.col = this.GetColFromX(x);
				hitTestInfo.row = this.GetRowFromY(y);
				if (hitTestInfo.col < 0 || hitTestInfo.row < 0)
				{
					return DataGrid.HitTestInfo.Nowhere;
				}
				return hitTestInfo;
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x00067687 File Offset: 0x00066687
		public DataGrid.HitTestInfo HitTest(Point position)
		{
			return this.HitTest(position.X, position.Y);
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000676A0 File Offset: 0x000666A0
		private void InitializeColumnWidths()
		{
			if (this.myGridTable == null)
			{
				return;
			}
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int count = gridColumnStyles.Count;
			int num = (this.myGridTable.IsDefault ? this.PreferredColumnWidth : this.myGridTable.PreferredColumnWidth);
			for (int i = 0; i < count; i++)
			{
				if (gridColumnStyles[i].width == -1)
				{
					gridColumnStyles[i].width = num;
				}
			}
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x00067712 File Offset: 0x00066712
		internal void InvalidateInside()
		{
			base.Invalidate(this.layout.Inside);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x00067725 File Offset: 0x00066725
		internal void InvalidateCaption()
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(this.layout.Caption);
			}
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x00067745 File Offset: 0x00066745
		internal void InvalidateCaptionRect(Rectangle r)
		{
			if (this.layout.CaptionVisible)
			{
				base.Invalidate(r);
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x0006775C File Offset: 0x0006675C
		internal void InvalidateColumn(int column)
		{
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			if (column < 0 || gridColumnStyles == null || gridColumnStyles.Count <= column)
			{
				return;
			}
			if (column < this.firstVisibleCol || column > this.firstVisibleCol + this.numVisibleCols - 1)
			{
				return;
			}
			Rectangle rectangle = default(Rectangle);
			rectangle.Height = this.layout.Data.Height;
			rectangle.Width = gridColumnStyles[column].Width;
			rectangle.Y = this.layout.Data.Y;
			int num = this.layout.Data.X - this.negOffset;
			int count = gridColumnStyles.Count;
			int num2 = this.firstVisibleCol;
			while (num2 < count && num2 != column)
			{
				num += gridColumnStyles[num2].Width;
				num2++;
			}
			rectangle.X = num;
			rectangle.X = this.MirrorRectangle(rectangle, this.layout.Data, this.isRightToLeft());
			base.Invalidate(rectangle);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00067862 File Offset: 0x00066862
		internal void InvalidateParentRows()
		{
			if (this.layout.ParentRowsVisible)
			{
				base.Invalidate(this.layout.ParentRows);
			}
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00067884 File Offset: 0x00066884
		internal void InvalidateParentRowsRect(Rectangle r)
		{
			Rectangle rectangle = this.layout.ParentRows;
			base.Invalidate(r);
			bool isEmpty = rectangle.IsEmpty;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000678AC File Offset: 0x000668AC
		internal void InvalidateRow(int rowNumber)
		{
			Rectangle rowRect = this.GetRowRect(rowNumber);
			if (!rowRect.IsEmpty)
			{
				base.Invalidate(rowRect);
			}
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000678D4 File Offset: 0x000668D4
		private void InvalidateRowHeader(int rowNumber)
		{
			if (rowNumber >= this.firstVisibleRow && rowNumber < this.firstVisibleRow + this.numVisibleRows)
			{
				if (!this.layout.RowHeadersVisible)
				{
					return;
				}
				base.Invalidate(new Rectangle
				{
					Y = this.GetRowTop(rowNumber),
					X = this.layout.RowHeaders.X,
					Width = this.layout.RowHeaders.Width,
					Height = this.DataGridRows[rowNumber].Height
				});
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x00067968 File Offset: 0x00066968
		internal void InvalidateRowRect(int rowNumber, Rectangle r)
		{
			Rectangle rowRect = this.GetRowRect(rowNumber);
			if (!rowRect.IsEmpty)
			{
				Rectangle rectangle = new Rectangle(rowRect.X + r.X, rowRect.Y + r.Y, r.Width, r.Height);
				if (this.vertScrollBar.Visible && this.isRightToLeft())
				{
					rectangle.X -= this.vertScrollBar.Width;
				}
				base.Invalidate(rectangle);
			}
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000679F0 File Offset: 0x000669F0
		public bool IsExpanded(int rowNumber)
		{
			if (rowNumber < 0 || rowNumber > this.DataGridRowsLength)
			{
				throw new ArgumentOutOfRangeException("rowNumber");
			}
			DataGridRow[] array = this.DataGridRows;
			DataGridRow dataGridRow = array[rowNumber];
			if (dataGridRow is DataGridRelationshipRow)
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)dataGridRow;
				return dataGridRelationshipRow.Expanded;
			}
			return false;
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00067A38 File Offset: 0x00066A38
		public bool IsSelected(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			return array[row].Selected;
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x00067A54 File Offset: 0x00066A54
		internal static bool IsTransparentColor(Color color)
		{
			return color.A < byte.MaxValue;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x00067A64 File Offset: 0x00066A64
		private void LayoutScrollBars()
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				this.horizScrollBar.Visible = false;
				this.vertScrollBar.Visible = false;
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = this.isRightToLeft();
			int count = this.myGridTable.GridColumnStyles.Count;
			DataGridRow[] array = this.DataGridRows;
			int num = Math.Max(0, this.GetColumnWidthSum());
			if (num > this.layout.Data.Width && !flag)
			{
				int height = this.horizScrollBar.Height;
				DataGrid.LayoutData layoutData = this.layout;
				layoutData.Data.Height = layoutData.Data.Height - height;
				if (this.layout.RowHeadersVisible)
				{
					DataGrid.LayoutData layoutData2 = this.layout;
					layoutData2.RowHeaders.Height = layoutData2.RowHeaders.Height - height;
				}
				flag = true;
			}
			int num2 = this.firstVisibleRow;
			this.ComputeVisibleRows();
			if (this.numTotallyVisibleRows != this.DataGridRowsLength && !flag2)
			{
				int width = this.vertScrollBar.Width;
				DataGrid.LayoutData layoutData3 = this.layout;
				layoutData3.Data.Width = layoutData3.Data.Width - width;
				if (this.layout.ColumnHeadersVisible)
				{
					if (flag4)
					{
						DataGrid.LayoutData layoutData4 = this.layout;
						layoutData4.ColumnHeaders.X = layoutData4.ColumnHeaders.X + width;
					}
					DataGrid.LayoutData layoutData5 = this.layout;
					layoutData5.ColumnHeaders.Width = layoutData5.ColumnHeaders.Width - width;
				}
				flag2 = true;
			}
			this.firstVisibleCol = this.ComputeFirstVisibleColumn();
			this.ComputeVisibleColumns();
			if (flag2 && num > this.layout.Data.Width && !flag)
			{
				this.firstVisibleRow = num2;
				int height2 = this.horizScrollBar.Height;
				DataGrid.LayoutData layoutData6 = this.layout;
				layoutData6.Data.Height = layoutData6.Data.Height - height2;
				if (this.layout.RowHeadersVisible)
				{
					DataGrid.LayoutData layoutData7 = this.layout;
					layoutData7.RowHeaders.Height = layoutData7.RowHeaders.Height - height2;
				}
				flag = true;
				flag3 = true;
			}
			if (flag3)
			{
				this.ComputeVisibleRows();
				if (this.numTotallyVisibleRows != this.DataGridRowsLength && !flag2)
				{
					int width2 = this.vertScrollBar.Width;
					DataGrid.LayoutData layoutData8 = this.layout;
					layoutData8.Data.Width = layoutData8.Data.Width - width2;
					if (this.layout.ColumnHeadersVisible)
					{
						if (flag4)
						{
							DataGrid.LayoutData layoutData9 = this.layout;
							layoutData9.ColumnHeaders.X = layoutData9.ColumnHeaders.X + width2;
						}
						DataGrid.LayoutData layoutData10 = this.layout;
						layoutData10.ColumnHeaders.Width = layoutData10.ColumnHeaders.Width - width2;
					}
					flag2 = true;
				}
			}
			this.layout.ResizeBoxRect = default(Rectangle);
			if (flag2 && flag)
			{
				Rectangle data = this.layout.Data;
				this.layout.ResizeBoxRect = new Rectangle(flag4 ? data.X : data.Right, data.Bottom, this.vertScrollBar.Width, this.horizScrollBar.Height);
			}
			if (flag && count > 0)
			{
				int num3 = num - this.layout.Data.Width;
				this.horizScrollBar.Minimum = 0;
				this.horizScrollBar.Maximum = num;
				this.horizScrollBar.SmallChange = 1;
				this.horizScrollBar.LargeChange = Math.Max(num - num3, 0);
				this.horizScrollBar.Enabled = base.Enabled;
				this.horizScrollBar.RightToLeft = this.RightToLeft;
				this.horizScrollBar.Bounds = new Rectangle(flag4 ? (this.layout.Inside.X + this.layout.ResizeBoxRect.Width) : this.layout.Inside.X, this.layout.Data.Bottom, this.layout.Inside.Width - this.layout.ResizeBoxRect.Width, this.horizScrollBar.Height);
				this.horizScrollBar.Visible = true;
			}
			else
			{
				this.HorizontalOffset = 0;
				this.horizScrollBar.Visible = false;
			}
			if (flag2)
			{
				int num4 = this.layout.Data.Y;
				if (this.layout.ColumnHeadersVisible)
				{
					num4 = this.layout.ColumnHeaders.Y;
				}
				this.vertScrollBar.LargeChange = ((this.numTotallyVisibleRows != 0) ? this.numTotallyVisibleRows : 1);
				this.vertScrollBar.Bounds = new Rectangle(flag4 ? this.layout.Data.X : this.layout.Data.Right, num4, this.vertScrollBar.Width, this.layout.Data.Height + this.layout.ColumnHeaders.Height);
				this.vertScrollBar.Enabled = base.Enabled;
				this.vertScrollBar.Visible = true;
				if (flag4)
				{
					DataGrid.LayoutData layoutData11 = this.layout;
					layoutData11.Data.X = layoutData11.Data.X + this.vertScrollBar.Width;
					return;
				}
			}
			else
			{
				this.vertScrollBar.Visible = false;
			}
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x00067F54 File Offset: 0x00066F54
		public void NavigateBack()
		{
			if (!this.CommitEdit() || this.parentRows.IsEmpty())
			{
				return;
			}
			if (this.gridState[1048576])
			{
				this.gridState[1048576] = false;
				try
				{
					this.listManager.CancelCurrentEdit();
					goto IL_004F;
				}
				catch
				{
					goto IL_004F;
				}
			}
			this.UpdateListManager();
			IL_004F:
			DataGridState dataGridState = this.parentRows.PopTop();
			this.ResetMouseState();
			dataGridState.PullState(this, false);
			if (this.parentRows.GetTopParent() == null)
			{
				this.originalState = null;
			}
			DataGridRow[] array = this.DataGridRows;
			if ((this.ReadOnly || !this.policy.AllowAdd) == array[this.DataGridRowsLength - 1] is DataGridAddNewRow)
			{
				int num = ((this.ReadOnly || !this.policy.AllowAdd) ? (this.DataGridRowsLength - 1) : (this.DataGridRowsLength + 1));
				DataGridRow[] array2 = new DataGridRow[num];
				for (int i = 0; i < Math.Min(num, this.DataGridRowsLength); i++)
				{
					array2[i] = this.DataGridRows[i];
				}
				if (!this.ReadOnly && this.policy.AllowAdd)
				{
					array2[num - 1] = new DataGridAddNewRow(this, this.myGridTable, num - 1);
				}
				this.SetDataGridRows(array2, num);
			}
			array = this.DataGridRows;
			if (array != null && array.Length != 0)
			{
				DataGridTableStyle dataGridTableStyle = array[0].DataGridTableStyle;
				if (dataGridTableStyle != this.myGridTable)
				{
					for (int j = 0; j < array.Length; j++)
					{
						array[j].DataGridTableStyle = this.myGridTable;
					}
				}
			}
			if (this.myGridTable.GridColumnStyles.Count > 0 && this.myGridTable.GridColumnStyles[0].Width == -1)
			{
				this.InitializeColumnWidths();
			}
			this.currentRow = ((this.ListManager.Position == -1) ? 0 : this.ListManager.Position);
			if (!this.AllowNavigation)
			{
				this.RecreateDataGridRows();
			}
			this.caption.BackButtonActive = this.parentRows.GetTopParent() != null && this.AllowNavigation;
			this.caption.BackButtonVisible = this.caption.BackButtonActive;
			this.caption.DownButtonActive = this.parentRows.GetTopParent() != null;
			base.PerformLayout();
			base.Invalidate();
			if (this.vertScrollBar.Visible)
			{
				this.vertScrollBar.Value = this.firstVisibleRow;
			}
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Value = this.HorizontalOffset + this.negOffset;
			}
			this.Edit();
			this.OnNavigate(new NavigateEventArgs(false));
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x00068208 File Offset: 0x00067208
		public void NavigateTo(int rowNumber, string relationName)
		{
			if (!this.AllowNavigation)
			{
				return;
			}
			DataGridRow[] array = this.DataGridRows;
			if (rowNumber < 0 || rowNumber > this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1))
			{
				throw new ArgumentOutOfRangeException("rowNumber");
			}
			this.EnsureBound();
			DataGridRow dataGridRow = array[rowNumber];
			this.NavigateTo(relationName, dataGridRow, false);
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x00068264 File Offset: 0x00067264
		internal void NavigateTo(string relationName, DataGridRow source, bool fromRow)
		{
			if (!this.AllowNavigation)
			{
				return;
			}
			if (!this.CommitEdit())
			{
				return;
			}
			DataGridState dataGridState;
			try
			{
				dataGridState = this.CreateChildState(relationName, source);
			}
			catch
			{
				this.NavigateBack();
				return;
			}
			try
			{
				this.listManager.EndCurrentEdit();
			}
			catch
			{
				return;
			}
			DataGridState dataGridState2 = new DataGridState(this);
			dataGridState2.LinkingRow = source;
			if (source.RowNumber != this.CurrentRow)
			{
				this.listManager.Position = source.RowNumber;
			}
			if (this.parentRows.GetTopParent() == null)
			{
				this.originalState = dataGridState2;
			}
			this.parentRows.AddParent(dataGridState2);
			this.NavigateTo(dataGridState);
			this.OnNavigate(new NavigateEventArgs(true));
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x00068328 File Offset: 0x00067328
		private void NavigateTo(DataGridState childState)
		{
			this.EndEdit();
			this.gridState[16384] = false;
			this.ResetMouseState();
			childState.PullState(this, true);
			if (this.listManager.Position != this.currentRow)
			{
				this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
			}
			if (this.parentRows.GetTopParent() != null)
			{
				this.caption.BackButtonActive = this.AllowNavigation;
				this.caption.BackButtonVisible = this.caption.BackButtonActive;
				this.caption.DownButtonActive = true;
			}
			this.HorizontalOffset = 0;
			base.PerformLayout();
			base.Invalidate();
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000683E4 File Offset: 0x000673E4
		private Point NormalizeToRow(int x, int y, int row)
		{
			Point point = new Point(0, this.layout.Data.Y);
			DataGridRow[] array = this.DataGridRows;
			for (int i = this.firstVisibleRow; i < row; i++)
			{
				point.Y += array[i].Height;
			}
			return new Point(x, y - point.Y);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x00068448 File Offset: 0x00067448
		internal void OnColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)sender;
			if (dataGridTableStyle.Equals(this.myGridTable))
			{
				if (!this.myGridTable.IsDefault && (e.Action != CollectionChangeAction.Refresh || e.Element == null))
				{
					this.PairTableStylesAndGridColumns(this.listManager, this.myGridTable, false);
				}
				base.Invalidate();
				base.PerformLayout();
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000684A8 File Offset: 0x000674A8
		private void PaintColumnHeaders(Graphics g)
		{
			bool flag = this.isRightToLeft();
			Rectangle columnHeaders = this.layout.ColumnHeaders;
			if (!flag)
			{
				columnHeaders.X -= this.negOffset;
			}
			columnHeaders.Width += this.negOffset;
			int num = this.PaintColumnHeaderText(g, columnHeaders);
			if (flag)
			{
				columnHeaders.X = columnHeaders.Right - num;
			}
			columnHeaders.Width = num;
			if (!this.FlatMode)
			{
				ControlPaint.DrawBorder3D(g, columnHeaders, Border3DStyle.RaisedInner);
				columnHeaders.Inflate(-1, -1);
				columnHeaders.Width--;
				columnHeaders.Height--;
				g.DrawRectangle(SystemPens.Control, columnHeaders);
			}
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x0006855C File Offset: 0x0006755C
		private int PaintColumnHeaderText(Graphics g, Rectangle boundingRect)
		{
			int num = 0;
			Rectangle rectangle = boundingRect;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			bool flag = this.isRightToLeft();
			int count = gridColumnStyles.Count;
			PropertyDescriptor sortProperty = this.ListManager.GetSortProperty();
			for (int i = this.firstVisibleCol; i < count; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					if (num > boundingRect.Width)
					{
						break;
					}
					bool flag2 = sortProperty != null && sortProperty.Equals(gridColumnStyles[i].PropertyDescriptor);
					TriangleDirection triangleDirection = TriangleDirection.Up;
					if (flag2)
					{
						ListSortDirection sortDirection = this.ListManager.GetSortDirection();
						if (sortDirection == ListSortDirection.Descending)
						{
							triangleDirection = TriangleDirection.Down;
						}
					}
					if (flag)
					{
						rectangle.Width = gridColumnStyles[i].Width - (flag2 ? rectangle.Height : 0);
						rectangle.X = boundingRect.Right - num - rectangle.Width;
					}
					else
					{
						rectangle.X = boundingRect.X + num;
						rectangle.Width = gridColumnStyles[i].Width - (flag2 ? rectangle.Height : 0);
					}
					Brush brush;
					if (this.myGridTable.IsDefault)
					{
						brush = this.HeaderBackBrush;
					}
					else
					{
						brush = this.myGridTable.HeaderBackBrush;
					}
					g.FillRectangle(brush, rectangle);
					if (flag)
					{
						rectangle.X -= 2;
						rectangle.Y += 2;
					}
					else
					{
						rectangle.X += 2;
						rectangle.Y += 2;
					}
					StringFormat stringFormat = new StringFormat();
					HorizontalAlignment alignment = gridColumnStyles[i].Alignment;
					stringFormat.Alignment = ((alignment == HorizontalAlignment.Right) ? StringAlignment.Far : ((alignment == HorizontalAlignment.Center) ? StringAlignment.Center : StringAlignment.Near));
					stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
					if (flag)
					{
						stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
						stringFormat.Alignment = StringAlignment.Near;
					}
					g.DrawString(gridColumnStyles[i].HeaderText, this.myGridTable.IsDefault ? this.HeaderFont : this.myGridTable.HeaderFont, this.myGridTable.IsDefault ? this.HeaderForeBrush : this.myGridTable.HeaderForeBrush, rectangle, stringFormat);
					stringFormat.Dispose();
					if (flag)
					{
						rectangle.X += 2;
						rectangle.Y -= 2;
					}
					else
					{
						rectangle.X -= 2;
						rectangle.Y -= 2;
					}
					if (flag2)
					{
						Rectangle rectangle2 = new Rectangle(flag ? (rectangle.X - rectangle.Height) : rectangle.Right, rectangle.Y, rectangle.Height, rectangle.Height);
						g.FillRectangle(brush, rectangle2);
						int num2 = Math.Max(0, (rectangle.Height - 5) / 2);
						rectangle2.Inflate(-num2, -num2);
						Pen pen = new Pen(this.BackgroundBrush);
						Pen pen2 = new Pen(this.myGridTable.BackBrush);
						Triangle.Paint(g, rectangle2, triangleDirection, brush, pen, pen2, pen, true);
						pen.Dispose();
						pen2.Dispose();
					}
					int num3 = rectangle.Width + (flag2 ? rectangle.Height : 0);
					if (!this.FlatMode)
					{
						if (flag && flag2)
						{
							rectangle.X -= rectangle.Height;
						}
						rectangle.Width = num3;
						ControlPaint.DrawBorder3D(g, rectangle, Border3DStyle.RaisedInner);
					}
					num += num3;
				}
			}
			if (num < boundingRect.Width)
			{
				rectangle = boundingRect;
				if (!flag)
				{
					rectangle.X += num;
				}
				rectangle.Width -= num;
				g.FillRectangle(this.backgroundBrush, rectangle);
			}
			return num;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x00068920 File Offset: 0x00067920
		private void PaintBorder(Graphics g, Rectangle bounds)
		{
			if (this.BorderStyle == BorderStyle.None)
			{
				return;
			}
			if (this.BorderStyle == BorderStyle.Fixed3D)
			{
				Border3DStyle border3DStyle = Border3DStyle.Sunken;
				ControlPaint.DrawBorder3D(g, bounds, border3DStyle);
				return;
			}
			if (this.BorderStyle == BorderStyle.FixedSingle)
			{
				Brush brush;
				if (this.myGridTable.IsDefault)
				{
					brush = this.HeaderForeBrush;
				}
				else
				{
					brush = this.myGridTable.HeaderForeBrush;
				}
				g.FillRectangle(brush, bounds.X, bounds.Y, bounds.Width + 2, 2);
				g.FillRectangle(brush, bounds.Right - 2, bounds.Y, 2, bounds.Height + 2);
				g.FillRectangle(brush, bounds.X, bounds.Bottom - 2, bounds.Width + 2, 2);
				g.FillRectangle(brush, bounds.X, bounds.Y, 2, bounds.Height + 2);
				return;
			}
			Pen windowFrame = SystemPens.WindowFrame;
			bounds.Width--;
			bounds.Height--;
			g.DrawRectangle(windowFrame, bounds);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00068A28 File Offset: 0x00067A28
		private void PaintGrid(Graphics g, Rectangle gridBounds)
		{
			Rectangle rectangle = gridBounds;
			if (this.listManager != null)
			{
				if (this.layout.ColumnHeadersVisible)
				{
					Region clip = g.Clip;
					g.SetClip(this.layout.ColumnHeaders);
					this.PaintColumnHeaders(g);
					g.Clip = clip;
					clip.Dispose();
					int height = this.layout.ColumnHeaders.Height;
					rectangle.Y += height;
					rectangle.Height -= height;
				}
				if (this.layout.TopLeftHeader.Width > 0)
				{
					if (this.myGridTable.IsDefault)
					{
						g.FillRectangle(this.HeaderBackBrush, this.layout.TopLeftHeader);
					}
					else
					{
						g.FillRectangle(this.myGridTable.HeaderBackBrush, this.layout.TopLeftHeader);
					}
					if (!this.FlatMode)
					{
						ControlPaint.DrawBorder3D(g, this.layout.TopLeftHeader, Border3DStyle.RaisedInner);
					}
				}
				this.PaintRows(g, ref rectangle);
			}
			if (rectangle.Height > 0)
			{
				g.FillRectangle(this.backgroundBrush, rectangle);
			}
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x00068B3C File Offset: 0x00067B3C
		private void DeleteDataGridRows(int deletedRows)
		{
			if (deletedRows == 0)
			{
				return;
			}
			int num = this.DataGridRowsLength;
			int num2 = num - deletedRows + (this.gridState[1048576] ? 1 : 0);
			DataGridRow[] array = new DataGridRow[num2];
			DataGridRow[] array2 = this.DataGridRows;
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				if (array2[i].Selected)
				{
					num3++;
				}
				else
				{
					array[i - num3] = array2[i];
					array[i - num3].number = i - num3;
				}
			}
			if (this.gridState[1048576])
			{
				array[num - num3] = new DataGridAddNewRow(this, this.myGridTable, num - num3);
				this.gridState[1048576] = false;
			}
			this.SetDataGridRows(array, num2);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x00068C00 File Offset: 0x00067C00
		private void PaintRows(Graphics g, ref Rectangle boundingRect)
		{
			int num = 0;
			bool flag = this.isRightToLeft();
			Rectangle rectangle = boundingRect;
			Rectangle rectangle2 = Rectangle.Empty;
			bool rowHeadersVisible = this.layout.RowHeadersVisible;
			Rectangle rectangle3 = Rectangle.Empty;
			int num2 = this.DataGridRowsLength;
			DataGridRow[] array = this.DataGridRows;
			int num3 = this.myGridTable.GridColumnStyles.Count - this.firstVisibleCol;
			int num4 = this.firstVisibleRow;
			while (num4 < num2 && num <= boundingRect.Height)
			{
				rectangle = boundingRect;
				rectangle.Height = array[num4].Height;
				rectangle.Y = boundingRect.Y + num;
				if (rowHeadersVisible)
				{
					rectangle3 = rectangle;
					rectangle3.Width = this.layout.RowHeaders.Width;
					if (flag)
					{
						rectangle3.X = rectangle.Right - rectangle3.Width;
					}
					if (g.IsVisible(rectangle3))
					{
						array[num4].PaintHeader(g, rectangle3, flag, this.gridState[32768]);
						g.ExcludeClip(rectangle3);
					}
					if (!flag)
					{
						rectangle.X += rectangle3.Width;
					}
					rectangle.Width -= rectangle3.Width;
				}
				if (g.IsVisible(rectangle))
				{
					rectangle2 = rectangle;
					if (!flag)
					{
						rectangle2.X -= this.negOffset;
					}
					rectangle2.Width += this.negOffset;
					array[num4].Paint(g, rectangle2, rectangle, this.firstVisibleCol, num3, flag);
				}
				num += rectangle.Height;
				num4++;
			}
			boundingRect.Y += num;
			boundingRect.Height -= num;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00068DBC File Offset: 0x00067DBC
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			DataGridRow[] array = this.DataGridRows;
			if (this.listManager != null && this.DataGridRowsLength > 0 && array[this.currentRow].OnKeyPress(keyData))
			{
				return true;
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys <= Keys.Down)
			{
				if (keys != Keys.Tab && keys != Keys.Return)
				{
					switch (keys)
					{
					case Keys.Escape:
					case Keys.Space:
					case Keys.Prior:
					case Keys.Next:
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						break;
					case Keys.IMEConvert:
					case Keys.IMENonconvert:
					case Keys.IMEAccept:
					case Keys.IMEModeChange:
					case Keys.End:
					case Keys.Home:
						goto IL_0243;
					default:
						goto IL_0243;
					}
				}
			}
			else if (keys <= Keys.C)
			{
				if (keys != Keys.Delete)
				{
					switch (keys)
					{
					case Keys.A:
						break;
					case Keys.B:
						goto IL_0243;
					case Keys.C:
						if ((keyData & Keys.Control) == Keys.None || (keyData & Keys.Alt) != Keys.None || !this.Bound)
						{
							goto IL_0243;
						}
						if (this.numSelectedRows != 0)
						{
							int num = 0;
							string text = "";
							for (int i = 0; i < this.DataGridRowsLength; i++)
							{
								if (array[i].Selected)
								{
									GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
									int count = gridColumnStyles.Count;
									for (int j = 0; j < count; j++)
									{
										DataGridColumnStyle dataGridColumnStyle = gridColumnStyles[j];
										text += dataGridColumnStyle.GetDisplayText(dataGridColumnStyle.GetColumnValueAtRow(this.ListManager, i));
										if (j < count - 1)
										{
											text += this.GetOutputTextDelimiter();
										}
									}
									if (num < this.numSelectedRows - 1)
									{
										text += "\r\n";
									}
									num++;
								}
							}
							Clipboard.SetDataObject(text);
							return true;
						}
						if (this.currentRow < this.ListManager.Count)
						{
							GridColumnStylesCollection gridColumnStyles2 = this.myGridTable.GridColumnStyles;
							DataGridColumnStyle dataGridColumnStyle2 = gridColumnStyles2[this.currentCol];
							string displayText = dataGridColumnStyle2.GetDisplayText(dataGridColumnStyle2.GetColumnValueAtRow(this.ListManager, this.currentRow));
							Clipboard.SetDataObject(displayText);
							return true;
						}
						goto IL_0243;
					default:
						goto IL_0243;
					}
				}
			}
			else
			{
				switch (keys)
				{
				case Keys.Add:
				case Keys.Subtract:
					break;
				case Keys.Separator:
					goto IL_0243;
				default:
					switch (keys)
					{
					case Keys.Oemplus:
					case Keys.OemMinus:
						break;
					case Keys.Oemcomma:
						goto IL_0243;
					default:
						goto IL_0243;
					}
					break;
				}
			}
			KeyEventArgs keyEventArgs = new KeyEventArgs(keyData);
			if (this.ProcessGridKey(keyEventArgs))
			{
				return true;
			}
			IL_0243:
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x00069014 File Offset: 0x00068014
		private void DeleteRows(DataGridRow[] localGridRows)
		{
			int num = 0;
			int num2 = ((this.listManager == null) ? 0 : this.listManager.Count);
			if (base.Visible)
			{
				base.BeginUpdateInternal();
			}
			try
			{
				if (this.ListManager != null)
				{
					for (int i = 0; i < this.DataGridRowsLength; i++)
					{
						if (localGridRows[i].Selected)
						{
							if (localGridRows[i] is DataGridAddNewRow)
							{
								localGridRows[i].Selected = false;
							}
							else
							{
								this.ListManager.RemoveAt(i - num);
								num++;
							}
						}
					}
				}
			}
			catch
			{
				this.RecreateDataGridRows();
				this.gridState[1024] = false;
				if (base.Visible)
				{
					base.EndUpdateInternal();
				}
				throw;
			}
			if (this.listManager != null && num2 == this.listManager.Count + num)
			{
				this.DeleteDataGridRows(num);
			}
			else
			{
				this.RecreateDataGridRows();
			}
			this.gridState[1024] = false;
			if (base.Visible)
			{
				base.EndUpdateInternal();
			}
			if (this.listManager != null && num2 != this.listManager.Count + num)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x00069134 File Offset: 0x00068134
		private int MoveLeftRight(GridColumnStylesCollection cols, int startCol, bool goRight)
		{
			int i;
			if (goRight)
			{
				for (i = startCol + 1; i < cols.Count; i++)
				{
					if (cols[i].PropertyDescriptor != null)
					{
						return i;
					}
				}
				return i;
			}
			for (i = startCol - 1; i >= 0; i--)
			{
				if (cols[i].PropertyDescriptor != null)
				{
					return i;
				}
			}
			return i;
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x00069188 File Offset: 0x00068188
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected bool ProcessGridKey(KeyEventArgs ke)
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				return false;
			}
			DataGridRow[] array = this.DataGridRows;
			KeyEventArgs keyEventArgs = ke;
			if (this.isRightToLeft())
			{
				switch (ke.KeyCode)
				{
				case Keys.Left:
					keyEventArgs = new KeyEventArgs(Keys.Right | ke.Modifiers);
					break;
				case Keys.Right:
					keyEventArgs = new KeyEventArgs(Keys.Left | ke.Modifiers);
					break;
				}
			}
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num = 0;
			int num2 = gridColumnStyles.Count;
			for (int i = 0; i < gridColumnStyles.Count; i++)
			{
				if (gridColumnStyles[i].PropertyDescriptor != null)
				{
					num = i;
					break;
				}
			}
			for (int j = gridColumnStyles.Count - 1; j >= 0; j--)
			{
				if (gridColumnStyles[j].PropertyDescriptor != null)
				{
					num2 = j;
					break;
				}
			}
			Keys keyCode = keyEventArgs.KeyCode;
			if (keyCode <= Keys.Delete)
			{
				if (keyCode == Keys.Tab)
				{
					return this.ProcessTabKey(keyEventArgs.KeyData);
				}
				if (keyCode != Keys.Return)
				{
					switch (keyCode)
					{
					case Keys.Escape:
						this.gridState[524288] = false;
						this.ResetSelection();
						if (!this.gridState[32768])
						{
							this.CancelEditing();
							this.Edit();
							return false;
						}
						this.AbortEdit();
						if (this.layout.RowHeadersVisible && this.currentRow > -1)
						{
							Rectangle rowRect = this.GetRowRect(this.currentRow);
							rowRect.Width = this.layout.RowHeaders.Width;
							base.Invalidate(rowRect);
						}
						this.Edit();
						break;
					case Keys.Space:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Shift)
						{
							this.ResetSelection();
							this.EndEdit();
							DataGridRow[] array2 = this.DataGridRows;
							array2[this.currentRow].Selected = true;
							this.numSelectedRows = 1;
							return true;
						}
						return false;
					case Keys.Prior:
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.gridState[524288] = false;
						if (keyEventArgs.Shift)
						{
							int num3 = this.currentRow;
							this.CurrentRow = Math.Max(0, this.CurrentRow - this.numTotallyVisibleRows);
							DataGridRow[] array3 = this.DataGridRows;
							for (int k = num3; k >= this.currentRow; k--)
							{
								if (!array3[k].Selected)
								{
									array3[k].Selected = true;
									this.numSelectedRows++;
								}
							}
							this.EndEdit();
						}
						else if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							this.ParentRowsVisible = false;
						}
						else
						{
							this.ResetSelection();
							this.CurrentRow = Math.Max(0, this.CurrentRow - this.numTotallyVisibleRows);
						}
						break;
					case Keys.Next:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Shift)
						{
							int num4 = this.currentRow;
							this.CurrentRow = Math.Min(this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1), this.currentRow + this.numTotallyVisibleRows);
							DataGridRow[] array4 = this.DataGridRows;
							for (int l = num4; l <= this.currentRow; l++)
							{
								if (!array4[l].Selected)
								{
									array4[l].Selected = true;
									this.numSelectedRows++;
								}
							}
							this.EndEdit();
						}
						else if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							this.ParentRowsVisible = true;
						}
						else
						{
							this.ResetSelection();
							this.CurrentRow = Math.Min(this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1), this.CurrentRow + this.numTotallyVisibleRows);
						}
						break;
					case Keys.End:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.ResetSelection();
						this.CurrentColumn = num2;
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							int num5 = this.currentRow;
							this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array5 = this.DataGridRows;
								for (int m = num5; m <= this.currentRow; m++)
								{
									array5[m].Selected = true;
								}
								this.numSelectedRows = this.currentRow - num5 + 1;
								this.EndEdit();
							}
							return true;
						}
						break;
					case Keys.Home:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						this.ResetSelection();
						this.CurrentColumn = 0;
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							int num6 = this.currentRow;
							this.CurrentRow = 0;
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array6 = this.DataGridRows;
								for (int n = 0; n <= num6; n++)
								{
									array6[n].Selected = true;
									this.numSelectedRows++;
								}
								this.EndEdit();
							}
							return true;
						}
						break;
					case Keys.Left:
						this.gridState[524288] = false;
						this.ResetSelection();
						if ((keyEventArgs.Modifiers & Keys.Modifiers) == Keys.Alt)
						{
							if (this.Caption.BackButtonVisible)
							{
								this.NavigateBack();
							}
							return true;
						}
						if ((keyEventArgs.Modifiers & Keys.Control) == Keys.Control)
						{
							this.CurrentColumn = num;
						}
						else if (this.currentCol == num && this.currentRow != 0)
						{
							this.CurrentRow--;
							int num7 = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.myGridTable.GridColumnStyles.Count, false);
							this.CurrentColumn = num7;
						}
						else
						{
							int num8 = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.currentCol, false);
							if (num8 == -1)
							{
								if (this.currentRow == 0)
								{
									return true;
								}
								this.CurrentRow--;
								this.CurrentColumn = num2;
							}
							else
							{
								this.CurrentColumn = num8;
							}
						}
						break;
					case Keys.Up:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array7 = this.DataGridRows;
								int num9 = this.currentRow;
								this.CurrentRow = 0;
								this.ResetSelection();
								for (int num10 = 0; num10 <= num9; num10++)
								{
									array7[num10].Selected = true;
								}
								this.numSelectedRows = num9 + 1;
								this.EndEdit();
								return true;
							}
							this.ResetSelection();
							this.CurrentRow = 0;
							return true;
						}
						else
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array8 = this.DataGridRows;
								if (array8[this.currentRow].Selected)
								{
									if (this.currentRow >= 1)
									{
										if (array8[this.currentRow - 1].Selected)
										{
											if (this.currentRow >= this.DataGridRowsLength - 1 || !array8[this.currentRow + 1].Selected)
											{
												this.numSelectedRows--;
												array8[this.currentRow].Selected = false;
											}
										}
										else
										{
											this.numSelectedRows += (array8[this.currentRow - 1].Selected ? 0 : 1);
											array8[this.currentRow - 1].Selected = true;
										}
										this.CurrentRow--;
									}
								}
								else
								{
									this.numSelectedRows++;
									array8[this.currentRow].Selected = true;
									if (this.currentRow >= 1)
									{
										this.numSelectedRows += (array8[this.currentRow - 1].Selected ? 0 : 1);
										array8[this.currentRow - 1].Selected = true;
										this.CurrentRow--;
									}
								}
								this.EndEdit();
								return true;
							}
							if (keyEventArgs.Alt)
							{
								this.SetRowExpansionState(-1, false);
								return true;
							}
							this.ResetSelection();
							this.CurrentRow--;
							this.Edit();
						}
						break;
					case Keys.Right:
						this.gridState[524288] = false;
						this.ResetSelection();
						if ((keyEventArgs.Modifiers & Keys.Control) == Keys.Control && !keyEventArgs.Alt)
						{
							this.CurrentColumn = num2;
						}
						else if (this.currentCol == num2 && this.currentRow != this.DataGridRowsLength - 1)
						{
							this.CurrentRow++;
							this.CurrentColumn = num;
						}
						else
						{
							int num11 = this.MoveLeftRight(this.myGridTable.GridColumnStyles, this.currentCol, true);
							if (num11 == gridColumnStyles.Count + 1)
							{
								this.CurrentColumn = num;
								this.CurrentRow++;
							}
							else
							{
								this.CurrentColumn = num11;
							}
						}
						break;
					case Keys.Down:
						this.gridState[524288] = false;
						if (this.dataGridRowsLength == 0)
						{
							return true;
						}
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							if (keyEventArgs.Shift)
							{
								int num12 = this.currentRow;
								this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
								DataGridRow[] array9 = this.DataGridRows;
								this.ResetSelection();
								for (int num13 = num12; num13 <= this.currentRow; num13++)
								{
									array9[num13].Selected = true;
								}
								this.numSelectedRows = this.currentRow - num12 + 1;
								this.EndEdit();
								return true;
							}
							this.ResetSelection();
							this.CurrentRow = Math.Max(0, this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1));
							return true;
						}
						else
						{
							if (keyEventArgs.Shift)
							{
								DataGridRow[] array10 = this.DataGridRows;
								if (array10[this.currentRow].Selected)
								{
									if (this.currentRow < this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0) - 1)
									{
										if (array10[this.currentRow + 1].Selected)
										{
											if (this.currentRow == 0 || !array10[this.currentRow - 1].Selected)
											{
												this.numSelectedRows--;
												array10[this.currentRow].Selected = false;
											}
										}
										else
										{
											this.numSelectedRows += (array10[this.currentRow + 1].Selected ? 0 : 1);
											array10[this.currentRow + 1].Selected = true;
										}
										this.CurrentRow++;
									}
								}
								else
								{
									this.numSelectedRows++;
									array10[this.currentRow].Selected = true;
									if (this.currentRow < this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0) - 1)
									{
										this.CurrentRow++;
										this.numSelectedRows += (array10[this.currentRow].Selected ? 0 : 1);
										array10[this.currentRow].Selected = true;
									}
								}
								this.EndEdit();
								return true;
							}
							if (keyEventArgs.Alt)
							{
								this.SetRowExpansionState(-1, true);
								return true;
							}
							this.ResetSelection();
							this.Edit();
							this.CurrentRow++;
						}
						break;
					case Keys.Delete:
						this.gridState[524288] = false;
						if (!this.policy.AllowRemove || this.numSelectedRows <= 0)
						{
							return false;
						}
						this.gridState[1024] = true;
						this.DeleteRows(array);
						this.currentRow = ((this.listManager.Count == 0) ? 0 : this.listManager.Position);
						this.numSelectedRows = 0;
						break;
					}
				}
				else
				{
					this.gridState[524288] = false;
					this.ResetSelection();
					if (!this.gridState[32768])
					{
						return false;
					}
					if ((keyEventArgs.Modifiers & Keys.Control) != Keys.None && !keyEventArgs.Alt)
					{
						this.EndEdit();
						this.HandleEndCurrentEdit();
						this.Edit();
					}
					else
					{
						this.CurrentRow = this.currentRow + 1;
					}
				}
			}
			else
			{
				if (keyCode <= Keys.Subtract)
				{
					if (keyCode != Keys.A)
					{
						switch (keyCode)
						{
						case Keys.Add:
							goto IL_0638;
						case Keys.Separator:
							return true;
						case Keys.Subtract:
							break;
						default:
							return true;
						}
					}
					else
					{
						this.gridState[524288] = false;
						if (keyEventArgs.Control && !keyEventArgs.Alt)
						{
							DataGridRow[] array11 = this.DataGridRows;
							for (int num14 = 0; num14 < this.DataGridRowsLength; num14++)
							{
								if (array11[num14] is DataGridRelationshipRow)
								{
									array11[num14].Selected = true;
								}
							}
							this.numSelectedRows = this.DataGridRowsLength - (this.policy.AllowAdd ? 1 : 0);
							this.EndEdit();
							return true;
						}
						return false;
					}
				}
				else
				{
					if (keyCode == Keys.F2)
					{
						this.gridState[524288] = false;
						this.ResetSelection();
						this.Edit();
						return true;
					}
					switch (keyCode)
					{
					case Keys.Oemplus:
						goto IL_0638;
					case Keys.Oemcomma:
						return true;
					case Keys.OemMinus:
						break;
					default:
						return true;
					}
				}
				this.gridState[524288] = false;
				if (keyEventArgs.Control && !keyEventArgs.Alt)
				{
					this.SetRowExpansionState(-1, false);
					return true;
				}
				return false;
				IL_0638:
				this.gridState[524288] = false;
				if (keyEventArgs.Control)
				{
					this.SetRowExpansionState(-1, true);
					this.EndEdit();
					return true;
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x00069F24 File Offset: 0x00068F24
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (m.Msg == 256)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs((Keys)(long)m.WParam | Control.ModifierKeys);
				Keys keyCode = keyEventArgs.KeyCode;
				if (keyCode <= Keys.Delete)
				{
					if (keyCode != Keys.Tab && keyCode != Keys.Return)
					{
						switch (keyCode)
						{
						case Keys.Escape:
						case Keys.Space:
						case Keys.Prior:
						case Keys.Next:
						case Keys.End:
						case Keys.Home:
						case Keys.Left:
						case Keys.Up:
						case Keys.Right:
						case Keys.Down:
						case Keys.Delete:
							break;
						case Keys.IMEConvert:
						case Keys.IMENonconvert:
						case Keys.IMEAccept:
						case Keys.IMEModeChange:
						case Keys.Select:
						case Keys.Print:
						case Keys.Execute:
						case Keys.Snapshot:
						case Keys.Insert:
							goto IL_011E;
						default:
							goto IL_011E;
						}
					}
				}
				else if (keyCode <= Keys.Subtract)
				{
					if (keyCode != Keys.A)
					{
						switch (keyCode)
						{
						case Keys.Add:
						case Keys.Subtract:
							break;
						case Keys.Separator:
							goto IL_011E;
						default:
							goto IL_011E;
						}
					}
				}
				else if (keyCode != Keys.F2)
				{
					switch (keyCode)
					{
					case Keys.Oemplus:
					case Keys.OemMinus:
						break;
					case Keys.Oemcomma:
						goto IL_011E;
					default:
						goto IL_011E;
					}
				}
				return this.ProcessGridKey(keyEventArgs);
			}
			if (m.Msg == 257)
			{
				KeyEventArgs keyEventArgs2 = new KeyEventArgs((Keys)(long)m.WParam | Control.ModifierKeys);
				if (keyEventArgs2.KeyCode == Keys.Tab)
				{
					return this.ProcessGridKey(keyEventArgs2);
				}
			}
			IL_011E:
			return base.ProcessKeyPreview(ref m);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x0006A058 File Offset: 0x00069058
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected bool ProcessTabKey(Keys keyData)
		{
			if (this.listManager == null || this.myGridTable == null)
			{
				return false;
			}
			bool flag = false;
			int count = this.myGridTable.GridColumnStyles.Count;
			this.isRightToLeft();
			this.ResetSelection();
			if (this.gridState[32768])
			{
				flag = true;
				if (!this.CommitEdit())
				{
					this.Edit();
					return true;
				}
			}
			if ((keyData & Keys.Control) == Keys.Control)
			{
				if ((keyData & Keys.Alt) == Keys.Alt)
				{
					return true;
				}
				Keys keys = keyData & ~Keys.Control;
				this.EndEdit();
				this.gridState[65536] = true;
				try
				{
					this.FocusInternal();
				}
				finally
				{
					this.gridState[65536] = false;
				}
				bool flag2 = false;
				IntSecurity.ModifyFocus.Assert();
				try
				{
					flag2 = base.ProcessDialogKey(keys);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return flag2;
			}
			else
			{
				DataGridRow[] array = this.DataGridRows;
				GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
				int num = 0;
				int num2 = gridColumnStyles.Count - 1;
				if (array.Length == 0)
				{
					this.EndEdit();
					bool flag3 = false;
					IntSecurity.ModifyFocus.Assert();
					try
					{
						flag3 = base.ProcessDialogKey(keyData);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return flag3;
				}
				for (int i = 0; i < gridColumnStyles.Count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num2 = i;
						break;
					}
				}
				for (int j = gridColumnStyles.Count - 1; j >= 0; j--)
				{
					if (gridColumnStyles[j].PropertyDescriptor != null)
					{
						num = j;
						break;
					}
				}
				if (this.CurrentColumn == num)
				{
					if ((this.gridState[524288] || (!this.gridState[524288] && (keyData & Keys.Shift) != Keys.Shift)) && array[this.CurrentRow].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
					{
						if (gridColumnStyles.Count > 0)
						{
							gridColumnStyles[this.CurrentColumn].ConcedeFocus();
						}
						this.gridState[524288] = true;
						if (this.gridState[2048] && base.CanFocus && !this.Focused)
						{
							this.FocusInternal();
						}
						return true;
					}
					if (this.currentRow == this.DataGridRowsLength - 1 && (keyData & Keys.Shift) == Keys.None)
					{
						this.EndEdit();
						bool flag4 = false;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							flag4 = base.ProcessDialogKey(keyData);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						return flag4;
					}
				}
				if (this.CurrentColumn == num2)
				{
					if (!this.gridState[524288])
					{
						if (this.CurrentRow != 0 && (keyData & Keys.Shift) == Keys.Shift && array[this.CurrentRow - 1].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
						{
							this.CurrentRow--;
							if (gridColumnStyles.Count > 0)
							{
								gridColumnStyles[this.CurrentColumn].ConcedeFocus();
							}
							this.gridState[524288] = true;
							if (this.gridState[2048] && base.CanFocus && !this.Focused)
							{
								this.FocusInternal();
							}
							return true;
						}
						if (this.currentRow == 0 && (keyData & Keys.Shift) == Keys.Shift)
						{
							this.EndEdit();
							bool flag5 = false;
							IntSecurity.ModifyFocus.Assert();
							try
							{
								flag5 = base.ProcessDialogKey(keyData);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
							return flag5;
						}
					}
					else
					{
						if (array[this.CurrentRow].ProcessTabKey(keyData, this.layout.RowHeaders, this.isRightToLeft()))
						{
							return true;
						}
						this.gridState[524288] = false;
						this.CurrentColumn = num;
						return true;
					}
				}
				if ((keyData & Keys.Shift) != Keys.Shift)
				{
					if (this.CurrentColumn == num)
					{
						if (this.CurrentRow != this.DataGridRowsLength - 1)
						{
							this.CurrentColumn = num2;
						}
						this.CurrentRow++;
					}
					else
					{
						int num3 = this.MoveLeftRight(gridColumnStyles, this.currentCol, true);
						this.CurrentColumn = num3;
					}
				}
				else if (this.CurrentColumn == num2)
				{
					if (this.CurrentRow != 0)
					{
						this.CurrentColumn = num;
					}
					if (!this.gridState[524288])
					{
						this.CurrentRow--;
					}
				}
				else if (this.gridState[524288] && this.CurrentColumn == num)
				{
					this.InvalidateRow(this.currentRow);
					this.Edit();
				}
				else
				{
					int num4 = this.MoveLeftRight(gridColumnStyles, this.currentCol, false);
					this.CurrentColumn = num4;
				}
				this.gridState[524288] = false;
				if (flag)
				{
					this.ResetSelection();
					this.Edit();
				}
				return true;
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x0006A560 File Offset: 0x00069560
		protected virtual void CancelEditing()
		{
			this.CancelCursorUpdate();
			if (this.gridState[1048576])
			{
				this.gridState[1048576] = false;
				DataGridRow[] array = this.DataGridRows;
				array[this.DataGridRowsLength - 1] = new DataGridAddNewRow(this, this.myGridTable, this.DataGridRowsLength - 1);
				this.SetDataGridRows(array, this.DataGridRowsLength);
			}
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x0006A5C8 File Offset: 0x000695C8
		internal void RecalculateFonts()
		{
			try
			{
				this.linkFont = new Font(this.Font, FontStyle.Underline);
			}
			catch
			{
			}
			this.fontHeight = this.Font.Height;
			this.linkFontHeight = this.LinkFont.Height;
			this.captionFontHeight = this.CaptionFont.Height;
			if (this.myGridTable == null || this.myGridTable.IsDefault)
			{
				this.headerFontHeight = this.HeaderFont.Height;
				return;
			}
			this.headerFontHeight = this.myGridTable.HeaderFont.Height;
		}

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600288F RID: 10383 RVA: 0x0006A66C File Offset: 0x0006966C
		// (remove) Token: 0x06002890 RID: 10384 RVA: 0x0006A67F File Offset: 0x0006967F
		[SRCategory("CatAction")]
		[SRDescription("DataGridBackButtonClickDescr")]
		public event EventHandler BackButtonClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_BACKBUTTONCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_BACKBUTTONCLICK, value);
			}
		}

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x06002891 RID: 10385 RVA: 0x0006A692 File Offset: 0x00069692
		// (remove) Token: 0x06002892 RID: 10386 RVA: 0x0006A6A5 File Offset: 0x000696A5
		[SRDescription("DataGridDownButtonClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler ShowParentDetailsButtonClick
		{
			add
			{
				base.Events.AddHandler(DataGrid.EVENT_DOWNBUTTONCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGrid.EVENT_DOWNBUTTONCLICK, value);
			}
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x0006A6B8 File Offset: 0x000696B8
		private void ResetMouseState()
		{
			this.oldRow = -1;
			this.gridState[262144] = true;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x0006A6D4 File Offset: 0x000696D4
		protected void ResetSelection()
		{
			if (this.numSelectedRows > 0)
			{
				DataGridRow[] array = this.DataGridRows;
				for (int i = 0; i < this.DataGridRowsLength; i++)
				{
					if (array[i].Selected)
					{
						array[i].Selected = false;
					}
				}
			}
			this.numSelectedRows = 0;
			this.lastRowSelected = -1;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x0006A724 File Offset: 0x00069724
		private void ResetParentRows()
		{
			this.parentRows.Clear();
			this.originalState = null;
			this.caption.BackButtonActive = (this.caption.DownButtonActive = (this.caption.BackButtonVisible = false));
			this.caption.SetDownButtonDirection(!this.layout.ParentRowsVisible);
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x0006A784 File Offset: 0x00069784
		private void ResetUIState()
		{
			this.gridState[524288] = false;
			this.ResetSelection();
			this.ResetMouseState();
			base.PerformLayout();
			base.Invalidate();
			if (this.horizScrollBar.Visible)
			{
				this.horizScrollBar.Invalidate();
			}
			if (this.vertScrollBar.Visible)
			{
				this.vertScrollBar.Invalidate();
			}
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x0006A7EC File Offset: 0x000697EC
		private void ScrollDown(int rows)
		{
			if (rows != 0)
			{
				this.ClearRegionCache();
				int num = Math.Max(0, Math.Min(this.firstVisibleRow + rows, this.DataGridRowsLength - 1));
				int num2 = this.firstVisibleRow;
				this.firstVisibleRow = num;
				this.vertScrollBar.Value = num;
				bool flag = this.gridState[32768];
				this.ComputeVisibleRows();
				if (this.gridState[131072])
				{
					this.Edit();
					this.gridState[131072] = false;
				}
				else
				{
					this.EndEdit();
				}
				int num3 = this.ComputeRowDelta(num2, num);
				Rectangle rectangle = this.layout.Data;
				if (this.layout.RowHeadersVisible)
				{
					rectangle = Rectangle.Union(rectangle, this.layout.RowHeaders);
				}
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
				SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), 0, num3, ref rect, ref rect);
				this.OnScroll(EventArgs.Empty);
				if (flag)
				{
					this.InvalidateRowHeader(this.currentRow);
				}
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x0006A910 File Offset: 0x00069910
		private void ScrollRight(int columns)
		{
			int num = this.firstVisibleCol + columns;
			GridColumnStylesCollection gridColumnStyles = this.myGridTable.GridColumnStyles;
			int num2 = 0;
			int count = gridColumnStyles.Count;
			int num3 = 0;
			if (this.myGridTable.IsDefault)
			{
				num3 = count;
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					if (gridColumnStyles[i].PropertyDescriptor != null)
					{
						num3++;
					}
				}
			}
			if ((this.lastTotallyVisibleCol == num3 - 1 && columns > 0) || (this.firstVisibleCol == 0 && columns < 0 && this.negOffset == 0))
			{
				return;
			}
			num = Math.Min(num, count - 1);
			for (int j = 0; j < num; j++)
			{
				if (gridColumnStyles[j].PropertyDescriptor != null)
				{
					num2 += gridColumnStyles[j].Width;
				}
			}
			this.HorizontalOffset = num2;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0006A9DC File Offset: 0x000699DC
		private void ScrollToColumn(int targetCol)
		{
			int num = targetCol - this.firstVisibleCol;
			if (targetCol > this.lastTotallyVisibleCol && this.lastTotallyVisibleCol != -1)
			{
				num = targetCol - this.lastTotallyVisibleCol;
			}
			if (num != 0 || this.negOffset != 0)
			{
				this.ScrollRight(num);
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x0006AA20 File Offset: 0x00069A20
		public void Select(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			if (!array[row].Selected)
			{
				array[row].Selected = true;
				this.numSelectedRows++;
			}
			this.EndEdit();
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x0006AA5C File Offset: 0x00069A5C
		private void PairTableStylesAndGridColumns(CurrencyManager lm, DataGridTableStyle gridTable, bool forceColumnCreation)
		{
			PropertyDescriptorCollection itemProperties = lm.GetItemProperties();
			GridColumnStylesCollection gridColumnStyles = gridTable.GridColumnStyles;
			if (gridTable.IsDefault || string.Compare(lm.GetListName(), gridTable.MappingName, true, CultureInfo.InvariantCulture) != 0)
			{
				gridTable.SetGridColumnStylesCollection(lm);
				if (gridTable.GridColumnStyles.Count > 0 && gridTable.GridColumnStyles[0].Width == -1)
				{
					this.InitializeColumnWidths();
				}
				return;
			}
			if (gridTable.GridColumnStyles.Count != 0 || base.DesignMode)
			{
				for (int i = 0; i < gridColumnStyles.Count; i++)
				{
					gridColumnStyles[i].PropertyDescriptor = null;
				}
				for (int j = 0; j < itemProperties.Count; j++)
				{
					DataGridColumnStyle dataGridColumnStyle = gridColumnStyles.MapColumnStyleToPropertyName(itemProperties[j].Name);
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.PropertyDescriptor = itemProperties[j];
					}
				}
				gridTable.SetRelationsList(lm);
				return;
			}
			if (forceColumnCreation)
			{
				gridTable.SetGridColumnStylesCollection(lm);
				return;
			}
			gridTable.SetRelationsList(lm);
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x0006AB54 File Offset: 0x00069B54
		internal void SetDataGridTable(DataGridTableStyle newTable, bool forceColumnCreation)
		{
			if (this.myGridTable != null)
			{
				this.UnWireTableStylePropChanged(this.myGridTable);
				if (this.myGridTable.IsDefault)
				{
					this.myGridTable.GridColumnStyles.ResetPropertyDescriptors();
					this.myGridTable.ResetRelationsList();
				}
			}
			this.myGridTable = newTable;
			this.WireTableStylePropChanged(this.myGridTable);
			this.layout.RowHeadersVisible = (newTable.IsDefault ? this.RowHeadersVisible : newTable.RowHeadersVisible);
			if (newTable != null)
			{
				newTable.DataGrid = this;
			}
			if (this.listManager != null)
			{
				this.PairTableStylesAndGridColumns(this.listManager, this.myGridTable, forceColumnCreation);
			}
			if (newTable != null)
			{
				newTable.ResetRelationsUI();
			}
			this.gridState[16384] = false;
			this.horizScrollBar.Value = 0;
			this.firstVisibleRow = 0;
			this.currentCol = 0;
			if (this.listManager == null)
			{
				this.currentRow = 0;
			}
			else
			{
				this.currentRow = ((this.listManager.Position == -1) ? 0 : this.listManager.Position);
			}
			this.ResetHorizontalOffset();
			this.negOffset = 0;
			this.ResetUIState();
			this.checkHierarchy = true;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x0006AC78 File Offset: 0x00069C78
		internal void SetParentRowsVisibility(bool visible)
		{
			Rectangle rectangle = this.layout.ParentRows;
			Rectangle data = this.layout.Data;
			if (this.layout.RowHeadersVisible)
			{
				data.X -= (this.isRightToLeft() ? 0 : this.layout.RowHeaders.Width);
				data.Width += this.layout.RowHeaders.Width;
			}
			if (this.layout.ColumnHeadersVisible)
			{
				data.Y -= this.layout.ColumnHeaders.Height;
				data.Height += this.layout.ColumnHeaders.Height;
			}
			this.EndEdit();
			if (visible)
			{
				this.layout.ParentRowsVisible = true;
				base.PerformLayout();
				base.Invalidate();
				return;
			}
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(data.X, data.Y - this.layout.ParentRows.Height, data.Width, data.Height + this.layout.ParentRows.Height);
			SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), 0, -rectangle.Height, ref rect, ref rect);
			if (this.vertScrollBar.Visible)
			{
				Rectangle bounds = this.vertScrollBar.Bounds;
				bounds.Y -= rectangle.Height;
				bounds.Height += rectangle.Height;
				base.Invalidate(bounds);
			}
			this.layout.ParentRowsVisible = false;
			base.PerformLayout();
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x0006AE1C File Offset: 0x00069E1C
		private void SetRowExpansionState(int row, bool expanded)
		{
			if (row < -1 || row > this.DataGridRowsLength - (this.policy.AllowAdd ? 2 : 1))
			{
				throw new ArgumentOutOfRangeException("row");
			}
			DataGridRow[] array = this.DataGridRows;
			if (row == -1)
			{
				DataGridRelationshipRow[] expandableRows = this.GetExpandableRows();
				bool flag = false;
				for (int i = 0; i < expandableRows.Length; i++)
				{
					if (expandableRows[i].Expanded != expanded)
					{
						expandableRows[i].Expanded = expanded;
						flag = true;
					}
				}
				if (flag && (this.gridState[16384] || this.gridState[32768]))
				{
					this.ResetSelection();
					this.Edit();
					return;
				}
			}
			else if (array[row] is DataGridRelationshipRow)
			{
				DataGridRelationshipRow dataGridRelationshipRow = (DataGridRelationshipRow)array[row];
				if (dataGridRelationshipRow.Expanded != expanded)
				{
					if (this.gridState[16384] || this.gridState[32768])
					{
						this.ResetSelection();
						this.Edit();
					}
					dataGridRelationshipRow.Expanded = expanded;
				}
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x0006AF18 File Offset: 0x00069F18
		private void ObjectSiteChange(IContainer container, IComponent component, bool site)
		{
			if (site)
			{
				if (component.Site == null)
				{
					container.Add(component);
					return;
				}
			}
			else if (component.Site != null && component.Site.Container == container)
			{
				container.Remove(component);
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x0006AF4C File Offset: 0x00069F4C
		public void SubObjectsSiteChange(bool site)
		{
			if (this.DesignMode && this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					DesignerTransaction designerTransaction = designerHost.CreateTransaction();
					try
					{
						IContainer container = this.Site.Container;
						DataGridTableStyle[] array = new DataGridTableStyle[this.TableStyles.Count];
						this.TableStyles.CopyTo(array, 0);
						foreach (DataGridTableStyle dataGridTableStyle in array)
						{
							this.ObjectSiteChange(container, dataGridTableStyle, site);
							DataGridColumnStyle[] array2 = new DataGridColumnStyle[dataGridTableStyle.GridColumnStyles.Count];
							dataGridTableStyle.GridColumnStyles.CopyTo(array2, 0);
							foreach (DataGridColumnStyle dataGridColumnStyle in array2)
							{
								this.ObjectSiteChange(container, dataGridColumnStyle, site);
							}
						}
					}
					finally
					{
						designerTransaction.Commit();
					}
				}
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0006B048 File Offset: 0x0006A048
		public void UnSelect(int row)
		{
			DataGridRow[] array = this.DataGridRows;
			if (array[row].Selected)
			{
				array[row].Selected = false;
				this.numSelectedRows--;
			}
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0006B080 File Offset: 0x0006A080
		private void UpdateListManager()
		{
			try
			{
				if (this.listManager != null)
				{
					this.EndEdit();
					this.listManager.EndCurrentEdit();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0006B0BC File Offset: 0x0006A0BC
		protected virtual string GetOutputTextDelimiter()
		{
			return "\t";
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0006B0C3 File Offset: 0x0006A0C3
		private int MirrorRectangle(Rectangle R1, Rectangle rect, bool rightToLeft)
		{
			if (rightToLeft)
			{
				return rect.Right + rect.X - R1.Right;
			}
			return R1.X;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0006B0E7 File Offset: 0x0006A0E7
		private int MirrorPoint(int x, Rectangle rect, bool rightToLeft)
		{
			if (rightToLeft)
			{
				return rect.Right + rect.X - x;
			}
			return x;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0006B0FF File Offset: 0x0006A0FF
		private bool isRightToLeft()
		{
			return this.RightToLeft == RightToLeft.Yes;
		}

		// Token: 0x04001695 RID: 5781
		private const int GRIDSTATE_allowSorting = 1;

		// Token: 0x04001696 RID: 5782
		private const int GRIDSTATE_columnHeadersVisible = 2;

		// Token: 0x04001697 RID: 5783
		private const int GRIDSTATE_rowHeadersVisible = 4;

		// Token: 0x04001698 RID: 5784
		private const int GRIDSTATE_trackColResize = 8;

		// Token: 0x04001699 RID: 5785
		private const int GRIDSTATE_trackRowResize = 16;

		// Token: 0x0400169A RID: 5786
		private const int GRIDSTATE_isLedgerStyle = 32;

		// Token: 0x0400169B RID: 5787
		private const int GRIDSTATE_isFlatMode = 64;

		// Token: 0x0400169C RID: 5788
		private const int GRIDSTATE_listHasErrors = 128;

		// Token: 0x0400169D RID: 5789
		private const int GRIDSTATE_dragging = 256;

		// Token: 0x0400169E RID: 5790
		private const int GRIDSTATE_inListAddNew = 512;

		// Token: 0x0400169F RID: 5791
		private const int GRIDSTATE_inDeleteRow = 1024;

		// Token: 0x040016A0 RID: 5792
		private const int GRIDSTATE_canFocus = 2048;

		// Token: 0x040016A1 RID: 5793
		private const int GRIDSTATE_readOnlyMode = 4096;

		// Token: 0x040016A2 RID: 5794
		private const int GRIDSTATE_allowNavigation = 8192;

		// Token: 0x040016A3 RID: 5795
		private const int GRIDSTATE_isNavigating = 16384;

		// Token: 0x040016A4 RID: 5796
		private const int GRIDSTATE_isEditing = 32768;

		// Token: 0x040016A5 RID: 5797
		private const int GRIDSTATE_editControlChanging = 65536;

		// Token: 0x040016A6 RID: 5798
		private const int GRIDSTATE_isScrolling = 131072;

		// Token: 0x040016A7 RID: 5799
		private const int GRIDSTATE_overCaption = 262144;

		// Token: 0x040016A8 RID: 5800
		private const int GRIDSTATE_childLinkFocused = 524288;

		// Token: 0x040016A9 RID: 5801
		private const int GRIDSTATE_inAddNewRow = 1048576;

		// Token: 0x040016AA RID: 5802
		private const int GRIDSTATE_inSetListManager = 2097152;

		// Token: 0x040016AB RID: 5803
		private const int GRIDSTATE_metaDataChanged = 4194304;

		// Token: 0x040016AC RID: 5804
		private const int GRIDSTATE_exceptionInPaint = 8388608;

		// Token: 0x040016AD RID: 5805
		private const int GRIDSTATE_layoutSuspended = 16777216;

		// Token: 0x040016AE RID: 5806
		private const int NumRowsForAutoResize = 10;

		// Token: 0x040016AF RID: 5807
		private const int errorRowBitmapWidth = 15;

		// Token: 0x040016B0 RID: 5808
		private const DataGridParentRowsLabelStyle defaultParentRowsLabelStyle = DataGridParentRowsLabelStyle.Both;

		// Token: 0x040016B1 RID: 5809
		private const BorderStyle defaultBorderStyle = BorderStyle.Fixed3D;

		// Token: 0x040016B2 RID: 5810
		private const bool defaultCaptionVisible = true;

		// Token: 0x040016B3 RID: 5811
		private const bool defaultParentRowsVisible = true;

		// Token: 0x040016B4 RID: 5812
		private const DataGridLineStyle defaultGridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040016B5 RID: 5813
		private const int defaultPreferredColumnWidth = 75;

		// Token: 0x040016B6 RID: 5814
		private const int defaultRowHeaderWidth = 35;

		// Token: 0x040016B7 RID: 5815
		internal TraceSwitch DataGridAcc;

		// Token: 0x040016B8 RID: 5816
		private BitVector32 gridState;

		// Token: 0x040016B9 RID: 5817
		private DataGridTableStyle defaultTableStyle = new DataGridTableStyle(true);

		// Token: 0x040016BA RID: 5818
		private SolidBrush alternatingBackBrush = DataGrid.DefaultAlternatingBackBrush;

		// Token: 0x040016BB RID: 5819
		private SolidBrush gridLineBrush = DataGrid.DefaultGridLineBrush;

		// Token: 0x040016BC RID: 5820
		private DataGridLineStyle gridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040016BD RID: 5821
		private SolidBrush headerBackBrush = DataGrid.DefaultHeaderBackBrush;

		// Token: 0x040016BE RID: 5822
		private Font headerFont;

		// Token: 0x040016BF RID: 5823
		private SolidBrush headerForeBrush = DataGrid.DefaultHeaderForeBrush;

		// Token: 0x040016C0 RID: 5824
		private Pen headerForePen = DataGrid.DefaultHeaderForePen;

		// Token: 0x040016C1 RID: 5825
		private SolidBrush linkBrush = DataGrid.DefaultLinkBrush;

		// Token: 0x040016C2 RID: 5826
		private int preferredColumnWidth = 75;

		// Token: 0x040016C3 RID: 5827
		private static int defaultFontHeight = Control.DefaultFont.Height;

		// Token: 0x040016C4 RID: 5828
		private int prefferedRowHeight = DataGrid.defaultFontHeight + 3;

		// Token: 0x040016C5 RID: 5829
		private int rowHeaderWidth = 35;

		// Token: 0x040016C6 RID: 5830
		private int minRowHeaderWidth;

		// Token: 0x040016C7 RID: 5831
		private SolidBrush selectionBackBrush = DataGrid.DefaultSelectionBackBrush;

		// Token: 0x040016C8 RID: 5832
		private SolidBrush selectionForeBrush = DataGrid.DefaultSelectionForeBrush;

		// Token: 0x040016C9 RID: 5833
		private DataGridParentRows parentRows;

		// Token: 0x040016CA RID: 5834
		private DataGridState originalState;

		// Token: 0x040016CB RID: 5835
		private DataGridRow[] dataGridRows = new DataGridRow[0];

		// Token: 0x040016CC RID: 5836
		private int dataGridRowsLength;

		// Token: 0x040016CD RID: 5837
		private int toolTipId;

		// Token: 0x040016CE RID: 5838
		private DataGridToolTip toolTipProvider;

		// Token: 0x040016CF RID: 5839
		private DataGridAddNewRow addNewRow;

		// Token: 0x040016D0 RID: 5840
		private DataGrid.LayoutData layout = new DataGrid.LayoutData();

		// Token: 0x040016D1 RID: 5841
		private NativeMethods.RECT[] cachedScrollableRegion;

		// Token: 0x040016D2 RID: 5842
		internal bool allowColumnResize = true;

		// Token: 0x040016D3 RID: 5843
		internal bool allowRowResize = true;

		// Token: 0x040016D4 RID: 5844
		internal DataGridParentRowsLabelStyle parentRowsLabels = DataGridParentRowsLabelStyle.Both;

		// Token: 0x040016D5 RID: 5845
		private int trackColAnchor;

		// Token: 0x040016D6 RID: 5846
		private int trackColumn;

		// Token: 0x040016D7 RID: 5847
		private int trackRowAnchor;

		// Token: 0x040016D8 RID: 5848
		private int trackRow;

		// Token: 0x040016D9 RID: 5849
		private PropertyDescriptor trackColumnHeader;

		// Token: 0x040016DA RID: 5850
		private MouseEventArgs lastSplitBar;

		// Token: 0x040016DB RID: 5851
		private Font linkFont;

		// Token: 0x040016DC RID: 5852
		private SolidBrush backBrush = DataGrid.DefaultBackBrush;

		// Token: 0x040016DD RID: 5853
		private SolidBrush foreBrush = DataGrid.DefaultForeBrush;

		// Token: 0x040016DE RID: 5854
		private SolidBrush backgroundBrush = DataGrid.DefaultBackgroundBrush;

		// Token: 0x040016DF RID: 5855
		private int fontHeight = -1;

		// Token: 0x040016E0 RID: 5856
		private int linkFontHeight = -1;

		// Token: 0x040016E1 RID: 5857
		private int captionFontHeight = -1;

		// Token: 0x040016E2 RID: 5858
		private int headerFontHeight = -1;

		// Token: 0x040016E3 RID: 5859
		private DataGridCaption caption;

		// Token: 0x040016E4 RID: 5860
		private BorderStyle borderStyle;

		// Token: 0x040016E5 RID: 5861
		private object dataSource;

		// Token: 0x040016E6 RID: 5862
		private string dataMember = "";

		// Token: 0x040016E7 RID: 5863
		private CurrencyManager listManager;

		// Token: 0x040016E8 RID: 5864
		private Control toBeDisposedEditingControl;

		// Token: 0x040016E9 RID: 5865
		internal GridTableStylesCollection dataGridTables;

		// Token: 0x040016EA RID: 5866
		internal DataGridTableStyle myGridTable;

		// Token: 0x040016EB RID: 5867
		internal bool checkHierarchy = true;

		// Token: 0x040016EC RID: 5868
		internal bool inInit;

		// Token: 0x040016ED RID: 5869
		internal int currentRow;

		// Token: 0x040016EE RID: 5870
		internal int currentCol;

		// Token: 0x040016EF RID: 5871
		private int numSelectedRows;

		// Token: 0x040016F0 RID: 5872
		private int lastRowSelected = -1;

		// Token: 0x040016F1 RID: 5873
		private DataGrid.Policy policy = new DataGrid.Policy();

		// Token: 0x040016F2 RID: 5874
		private DataGridColumnStyle editColumn;

		// Token: 0x040016F3 RID: 5875
		private DataGridRow editRow;

		// Token: 0x040016F4 RID: 5876
		private ScrollBar horizScrollBar = new HScrollBar();

		// Token: 0x040016F5 RID: 5877
		private ScrollBar vertScrollBar = new VScrollBar();

		// Token: 0x040016F6 RID: 5878
		private int horizontalOffset;

		// Token: 0x040016F7 RID: 5879
		private int negOffset;

		// Token: 0x040016F8 RID: 5880
		private int wheelDelta;

		// Token: 0x040016F9 RID: 5881
		internal int firstVisibleRow;

		// Token: 0x040016FA RID: 5882
		internal int firstVisibleCol;

		// Token: 0x040016FB RID: 5883
		private int numVisibleRows;

		// Token: 0x040016FC RID: 5884
		private int numVisibleCols;

		// Token: 0x040016FD RID: 5885
		private int numTotallyVisibleRows;

		// Token: 0x040016FE RID: 5886
		private int lastTotallyVisibleCol;

		// Token: 0x040016FF RID: 5887
		private int oldRow = -1;

		// Token: 0x04001700 RID: 5888
		private static readonly object EVENT_CURRENTCELLCHANGED = new object();

		// Token: 0x04001701 RID: 5889
		private static readonly object EVENT_NODECLICKED = new object();

		// Token: 0x04001702 RID: 5890
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04001703 RID: 5891
		private static readonly object EVENT_BACKBUTTONCLICK = new object();

		// Token: 0x04001704 RID: 5892
		private static readonly object EVENT_DOWNBUTTONCLICK = new object();

		// Token: 0x04001705 RID: 5893
		private ItemChangedEventHandler itemChangedHandler;

		// Token: 0x04001706 RID: 5894
		private EventHandler positionChangedHandler;

		// Token: 0x04001707 RID: 5895
		private EventHandler currentChangedHandler;

		// Token: 0x04001708 RID: 5896
		private EventHandler metaDataChangedHandler;

		// Token: 0x04001709 RID: 5897
		private CollectionChangeEventHandler dataGridTableStylesCollectionChanged;

		// Token: 0x0400170A RID: 5898
		private EventHandler backButtonHandler;

		// Token: 0x0400170B RID: 5899
		private EventHandler downButtonHandler;

		// Token: 0x0400170C RID: 5900
		private NavigateEventHandler onNavigate;

		// Token: 0x0400170D RID: 5901
		private EventHandler onRowHeaderClick;

		// Token: 0x0400170E RID: 5902
		private static readonly object EVENT_BORDERSTYLECHANGED = new object();

		// Token: 0x0400170F RID: 5903
		private static readonly object EVENT_CAPTIONVISIBLECHANGED = new object();

		// Token: 0x04001710 RID: 5904
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x04001711 RID: 5905
		private static readonly object EVENT_PARENTROWSLABELSTYLECHANGED = new object();

		// Token: 0x04001712 RID: 5906
		private static readonly object EVENT_FLATMODECHANGED = new object();

		// Token: 0x04001713 RID: 5907
		private static readonly object EVENT_BACKGROUNDCOLORCHANGED = new object();

		// Token: 0x04001714 RID: 5908
		private static readonly object EVENT_ALLOWNAVIGATIONCHANGED = new object();

		// Token: 0x04001715 RID: 5909
		private static readonly object EVENT_READONLYCHANGED = new object();

		// Token: 0x04001716 RID: 5910
		private static readonly object EVENT_PARENTROWSVISIBLECHANGED = new object();

		// Token: 0x020002C3 RID: 707
		[ComVisible(true)]
		internal class DataGridAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x060028A8 RID: 10408 RVA: 0x0006B1B4 File Offset: 0x0006A1B4
			public DataGridAccessibleObject(DataGrid owner)
				: base(owner)
			{
			}

			// Token: 0x170006A0 RID: 1696
			// (get) Token: 0x060028A9 RID: 10409 RVA: 0x0006B1BD File Offset: 0x0006A1BD
			internal DataGrid DataGrid
			{
				get
				{
					return (DataGrid)base.Owner;
				}
			}

			// Token: 0x170006A1 RID: 1697
			// (get) Token: 0x060028AA RID: 10410 RVA: 0x0006B1CA File Offset: 0x0006A1CA
			private int ColumnCount
			{
				get
				{
					return ((DataGrid)base.Owner).myGridTable.GridColumnStyles.Count;
				}
			}

			// Token: 0x170006A2 RID: 1698
			// (get) Token: 0x060028AB RID: 10411 RVA: 0x0006B1E6 File Offset: 0x0006A1E6
			private int RowCount
			{
				get
				{
					return ((DataGrid)base.Owner).dataGridRows.Length;
				}
			}

			// Token: 0x170006A3 RID: 1699
			// (get) Token: 0x060028AC RID: 10412 RVA: 0x0006B1FC File Offset: 0x0006A1FC
			// (set) Token: 0x060028AD RID: 10413 RVA: 0x0006B21F File Offset: 0x0006A21F
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					return "DataGrid";
				}
				set
				{
					base.Owner.AccessibleName = value;
				}
			}

			// Token: 0x170006A4 RID: 1700
			// (get) Token: 0x060028AE RID: 10414 RVA: 0x0006B230 File Offset: 0x0006A230
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Table;
				}
			}

			// Token: 0x060028AF RID: 10415 RVA: 0x0006B254 File Offset: 0x0006A254
			public override AccessibleObject GetChild(int index)
			{
				DataGrid dataGrid = (DataGrid)base.Owner;
				int columnCount = this.ColumnCount;
				int rowCount = this.RowCount;
				if (dataGrid.dataGridRows == null)
				{
					dataGrid.CreateDataGridRows();
				}
				if (index < 1)
				{
					return dataGrid.ParentRowsAccessibleObject;
				}
				index--;
				if (index < columnCount)
				{
					return dataGrid.myGridTable.GridColumnStyles[index].HeaderAccessibleObject;
				}
				index -= columnCount;
				if (index < rowCount)
				{
					return dataGrid.dataGridRows[index].AccessibleObject;
				}
				index -= rowCount;
				if (dataGrid.horizScrollBar.Visible)
				{
					if (index == 0)
					{
						return dataGrid.horizScrollBar.AccessibilityObject;
					}
					index--;
				}
				if (dataGrid.vertScrollBar.Visible)
				{
					if (index == 0)
					{
						return dataGrid.vertScrollBar.AccessibilityObject;
					}
					index--;
				}
				int count = dataGrid.myGridTable.GridColumnStyles.Count;
				int num = dataGrid.dataGridRows.Length;
				int num2 = index / count;
				int num3 = index % count;
				if (num2 < dataGrid.dataGridRows.Length && num3 < dataGrid.myGridTable.GridColumnStyles.Count)
				{
					return dataGrid.dataGridRows[num2].AccessibleObject.GetChild(num3);
				}
				return null;
			}

			// Token: 0x060028B0 RID: 10416 RVA: 0x0006B370 File Offset: 0x0006A370
			public override int GetChildCount()
			{
				int num = 1 + this.ColumnCount + ((DataGrid)base.Owner).DataGridRowsLength;
				if (this.DataGrid.horizScrollBar.Visible)
				{
					num++;
				}
				if (this.DataGrid.vertScrollBar.Visible)
				{
					num++;
				}
				return num + this.DataGrid.DataGridRows.Length * this.DataGrid.myGridTable.GridColumnStyles.Count;
			}

			// Token: 0x060028B1 RID: 10417 RVA: 0x0006B3EA File Offset: 0x0006A3EA
			public override AccessibleObject GetFocused()
			{
				if (this.DataGrid.Focused)
				{
					return this.GetSelected();
				}
				return null;
			}

			// Token: 0x060028B2 RID: 10418 RVA: 0x0006B404 File Offset: 0x0006A404
			public override AccessibleObject GetSelected()
			{
				if (this.DataGrid.DataGridRows.Length == 0 || this.DataGrid.myGridTable.GridColumnStyles.Count == 0)
				{
					return null;
				}
				DataGridCell currentCell = this.DataGrid.CurrentCell;
				return this.GetChild(1 + this.ColumnCount + currentCell.RowNumber).GetChild(currentCell.ColumnNumber);
			}

			// Token: 0x060028B3 RID: 10419 RVA: 0x0006B468 File Offset: 0x0006A468
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = this.DataGrid.PointToClient(new Point(x, y));
				DataGrid.HitTestInfo hitTestInfo = this.DataGrid.HitTest(point.X, point.Y);
				DataGrid.HitTestType type = hitTestInfo.Type;
				if (type <= DataGrid.HitTestType.RowResize)
				{
					switch (type)
					{
					case DataGrid.HitTestType.None:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.ColumnHeader:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.ColumnHeader | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.Cell | DataGrid.HitTestType.ColumnHeader | DataGrid.HitTestType.RowHeader:
					case DataGrid.HitTestType.ColumnResize:
						break;
					case DataGrid.HitTestType.Cell:
						return this.GetChild(1 + this.ColumnCount + hitTestInfo.Row).GetChild(hitTestInfo.Column);
					case DataGrid.HitTestType.ColumnHeader:
						return this.GetChild(1 + hitTestInfo.Column);
					case DataGrid.HitTestType.RowHeader:
						return this.GetChild(1 + this.ColumnCount + hitTestInfo.Row);
					default:
						if (type != DataGrid.HitTestType.RowResize)
						{
						}
						break;
					}
				}
				else if (type != DataGrid.HitTestType.Caption)
				{
					if (type == DataGrid.HitTestType.ParentRows)
					{
						return this.DataGrid.ParentRowsAccessibleObject;
					}
				}
				return null;
			}

			// Token: 0x060028B4 RID: 10420 RVA: 0x0006B540 File Offset: 0x0006A540
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				if (this.GetChildCount() > 0)
				{
					switch (navdir)
					{
					case AccessibleNavigation.FirstChild:
						return this.GetChild(0);
					case AccessibleNavigation.LastChild:
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return null;
			}
		}

		// Token: 0x020002C4 RID: 708
		internal class LayoutData
		{
			// Token: 0x060028B5 RID: 10421 RVA: 0x0006B584 File Offset: 0x0006A584
			public LayoutData()
			{
			}

			// Token: 0x060028B6 RID: 10422 RVA: 0x0006B604 File Offset: 0x0006A604
			public LayoutData(DataGrid.LayoutData src)
			{
				this.GrabLayout(src);
			}

			// Token: 0x060028B7 RID: 10423 RVA: 0x0006B688 File Offset: 0x0006A688
			private void GrabLayout(DataGrid.LayoutData src)
			{
				this.Inside = src.Inside;
				this.TopLeftHeader = src.TopLeftHeader;
				this.ColumnHeaders = src.ColumnHeaders;
				this.RowHeaders = src.RowHeaders;
				this.Data = src.Data;
				this.Caption = src.Caption;
				this.ParentRows = src.ParentRows;
				this.ResizeBoxRect = src.ResizeBoxRect;
				this.ColumnHeadersVisible = src.ColumnHeadersVisible;
				this.RowHeadersVisible = src.RowHeadersVisible;
				this.CaptionVisible = src.CaptionVisible;
				this.ParentRowsVisible = src.ParentRowsVisible;
				this.ClientRectangle = src.ClientRectangle;
			}

			// Token: 0x060028B8 RID: 10424 RVA: 0x0006B734 File Offset: 0x0006A734
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				stringBuilder.Append(base.ToString());
				stringBuilder.Append(" { \n");
				stringBuilder.Append("Inside = ");
				stringBuilder.Append(this.Inside.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("TopLeftHeader = ");
				stringBuilder.Append(this.TopLeftHeader.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ColumnHeaders = ");
				stringBuilder.Append(this.ColumnHeaders.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("RowHeaders = ");
				stringBuilder.Append(this.RowHeaders.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("Data = ");
				stringBuilder.Append(this.Data.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("Caption = ");
				stringBuilder.Append(this.Caption.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ParentRows = ");
				stringBuilder.Append(this.ParentRows.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ResizeBoxRect = ");
				stringBuilder.Append(this.ResizeBoxRect.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ColumnHeadersVisible = ");
				stringBuilder.Append(this.ColumnHeadersVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("RowHeadersVisible = ");
				stringBuilder.Append(this.RowHeadersVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("CaptionVisible = ");
				stringBuilder.Append(this.CaptionVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ParentRowsVisible = ");
				stringBuilder.Append(this.ParentRowsVisible.ToString());
				stringBuilder.Append('\n');
				stringBuilder.Append("ClientRectangle = ");
				stringBuilder.Append(this.ClientRectangle.ToString());
				stringBuilder.Append(" } ");
				return stringBuilder.ToString();
			}

			// Token: 0x04001717 RID: 5911
			internal bool dirty = true;

			// Token: 0x04001718 RID: 5912
			public Rectangle Inside = Rectangle.Empty;

			// Token: 0x04001719 RID: 5913
			public Rectangle RowHeaders = Rectangle.Empty;

			// Token: 0x0400171A RID: 5914
			public Rectangle TopLeftHeader = Rectangle.Empty;

			// Token: 0x0400171B RID: 5915
			public Rectangle ColumnHeaders = Rectangle.Empty;

			// Token: 0x0400171C RID: 5916
			public Rectangle Data = Rectangle.Empty;

			// Token: 0x0400171D RID: 5917
			public Rectangle Caption = Rectangle.Empty;

			// Token: 0x0400171E RID: 5918
			public Rectangle ParentRows = Rectangle.Empty;

			// Token: 0x0400171F RID: 5919
			public Rectangle ResizeBoxRect = Rectangle.Empty;

			// Token: 0x04001720 RID: 5920
			public bool ColumnHeadersVisible;

			// Token: 0x04001721 RID: 5921
			public bool RowHeadersVisible;

			// Token: 0x04001722 RID: 5922
			public bool CaptionVisible;

			// Token: 0x04001723 RID: 5923
			public bool ParentRowsVisible;

			// Token: 0x04001724 RID: 5924
			public Rectangle ClientRectangle = Rectangle.Empty;
		}

		// Token: 0x020002C5 RID: 709
		public sealed class HitTestInfo
		{
			// Token: 0x060028B9 RID: 10425 RVA: 0x0006B9A0 File Offset: 0x0006A9A0
			internal HitTestInfo()
			{
				this.type = DataGrid.HitTestType.None;
				this.row = (this.col = -1);
			}

			// Token: 0x060028BA RID: 10426 RVA: 0x0006B9CC File Offset: 0x0006A9CC
			internal HitTestInfo(DataGrid.HitTestType type)
			{
				this.type = type;
				this.row = (this.col = -1);
			}

			// Token: 0x170006A5 RID: 1701
			// (get) Token: 0x060028BB RID: 10427 RVA: 0x0006B9F6 File Offset: 0x0006A9F6
			public int Column
			{
				get
				{
					return this.col;
				}
			}

			// Token: 0x170006A6 RID: 1702
			// (get) Token: 0x060028BC RID: 10428 RVA: 0x0006B9FE File Offset: 0x0006A9FE
			public int Row
			{
				get
				{
					return this.row;
				}
			}

			// Token: 0x170006A7 RID: 1703
			// (get) Token: 0x060028BD RID: 10429 RVA: 0x0006BA06 File Offset: 0x0006AA06
			public DataGrid.HitTestType Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x060028BE RID: 10430 RVA: 0x0006BA10 File Offset: 0x0006AA10
			public override bool Equals(object value)
			{
				if (value is DataGrid.HitTestInfo)
				{
					DataGrid.HitTestInfo hitTestInfo = (DataGrid.HitTestInfo)value;
					return this.type == hitTestInfo.type && this.row == hitTestInfo.row && this.col == hitTestInfo.col;
				}
				return false;
			}

			// Token: 0x060028BF RID: 10431 RVA: 0x0006BA5A File Offset: 0x0006AA5A
			public override int GetHashCode()
			{
				return (int)(this.type + (this.row << 8) + (this.col << 16));
			}

			// Token: 0x060028C0 RID: 10432 RVA: 0x0006BA78 File Offset: 0x0006AA78
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"{ ",
					this.type.ToString(),
					",",
					this.row.ToString(CultureInfo.InvariantCulture),
					",",
					this.col.ToString(CultureInfo.InvariantCulture),
					"}"
				});
			}

			// Token: 0x04001725 RID: 5925
			internal DataGrid.HitTestType type;

			// Token: 0x04001726 RID: 5926
			internal int row;

			// Token: 0x04001727 RID: 5927
			internal int col;

			// Token: 0x04001728 RID: 5928
			public static readonly DataGrid.HitTestInfo Nowhere = new DataGrid.HitTestInfo();
		}

		// Token: 0x020002C6 RID: 710
		[Flags]
		public enum HitTestType
		{
			// Token: 0x0400172A RID: 5930
			None = 0,
			// Token: 0x0400172B RID: 5931
			Cell = 1,
			// Token: 0x0400172C RID: 5932
			ColumnHeader = 2,
			// Token: 0x0400172D RID: 5933
			RowHeader = 4,
			// Token: 0x0400172E RID: 5934
			ColumnResize = 8,
			// Token: 0x0400172F RID: 5935
			RowResize = 16,
			// Token: 0x04001730 RID: 5936
			Caption = 32,
			// Token: 0x04001731 RID: 5937
			ParentRows = 64
		}

		// Token: 0x020002C7 RID: 711
		private class Policy
		{
			// Token: 0x170006A8 RID: 1704
			// (get) Token: 0x060028C3 RID: 10435 RVA: 0x0006BB14 File Offset: 0x0006AB14
			// (set) Token: 0x060028C4 RID: 10436 RVA: 0x0006BB1C File Offset: 0x0006AB1C
			public bool AllowAdd
			{
				get
				{
					return this.allowAdd;
				}
				set
				{
					if (this.allowAdd != value)
					{
						this.allowAdd = value;
					}
				}
			}

			// Token: 0x170006A9 RID: 1705
			// (get) Token: 0x060028C5 RID: 10437 RVA: 0x0006BB2E File Offset: 0x0006AB2E
			// (set) Token: 0x060028C6 RID: 10438 RVA: 0x0006BB36 File Offset: 0x0006AB36
			public bool AllowEdit
			{
				get
				{
					return this.allowEdit;
				}
				set
				{
					if (this.allowEdit != value)
					{
						this.allowEdit = value;
					}
				}
			}

			// Token: 0x170006AA RID: 1706
			// (get) Token: 0x060028C7 RID: 10439 RVA: 0x0006BB48 File Offset: 0x0006AB48
			// (set) Token: 0x060028C8 RID: 10440 RVA: 0x0006BB50 File Offset: 0x0006AB50
			public bool AllowRemove
			{
				get
				{
					return this.allowRemove;
				}
				set
				{
					if (this.allowRemove != value)
					{
						this.allowRemove = value;
					}
				}
			}

			// Token: 0x060028C9 RID: 10441 RVA: 0x0006BB64 File Offset: 0x0006AB64
			public bool UpdatePolicy(CurrencyManager listManager, bool gridReadOnly)
			{
				bool flag = false;
				IBindingList bindingList = ((listManager == null) ? null : (listManager.List as IBindingList));
				if (listManager == null)
				{
					if (!this.allowAdd)
					{
						flag = true;
					}
					this.allowAdd = (this.allowEdit = (this.allowRemove = true));
				}
				else
				{
					if (this.AllowAdd != listManager.AllowAdd && !gridReadOnly)
					{
						flag = true;
					}
					this.AllowAdd = listManager.AllowAdd && !gridReadOnly && bindingList != null && bindingList.SupportsChangeNotification;
					this.AllowEdit = listManager.AllowEdit && !gridReadOnly;
					this.AllowRemove = listManager.AllowRemove && !gridReadOnly && bindingList != null && bindingList.SupportsChangeNotification;
				}
				return flag;
			}

			// Token: 0x04001732 RID: 5938
			private bool allowAdd = true;

			// Token: 0x04001733 RID: 5939
			private bool allowEdit = true;

			// Token: 0x04001734 RID: 5940
			private bool allowRemove = true;
		}
	}
}
