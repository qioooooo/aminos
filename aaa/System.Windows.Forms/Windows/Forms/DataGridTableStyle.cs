using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	// Token: 0x020002E1 RID: 737
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public class DataGridTableStyle : Component, IDataGridEditingService
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x00072E2A File Offset: 0x00071E2A
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x00072E34 File Offset: 0x00071E34
		[SRDescription("DataGridAllowSortingDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowSorting
		{
			get
			{
				return this.allowSorting;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "AllowSorting" }));
				}
				if (this.allowSorting != value)
				{
					this.allowSorting = value;
					this.OnAllowSortingChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000130 RID: 304
		// (add) Token: 0x06002AC3 RID: 10947 RVA: 0x00072E84 File Offset: 0x00071E84
		// (remove) Token: 0x06002AC4 RID: 10948 RVA: 0x00072E97 File Offset: 0x00071E97
		public event EventHandler AllowSortingChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventAllowSorting, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventAllowSorting, value);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x00072EAA File Offset: 0x00071EAA
		// (set) Token: 0x06002AC6 RID: 10950 RVA: 0x00072EB8 File Offset: 0x00071EB8
		[SRCategory("CatColors")]
		[SRDescription("DataGridAlternatingBackColorDescr")]
		public Color AlternatingBackColor
		{
			get
			{
				return this.alternatingBackBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "AlternatingBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentAlternatingBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "AlternatingBackColor" }));
				}
				if (!this.alternatingBackBrush.Color.Equals(value))
				{
					this.alternatingBackBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnAlternatingBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000131 RID: 305
		// (add) Token: 0x06002AC7 RID: 10951 RVA: 0x00072F6C File Offset: 0x00071F6C
		// (remove) Token: 0x06002AC8 RID: 10952 RVA: 0x00072F7F File Offset: 0x00071F7F
		public event EventHandler AlternatingBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventAlternatingBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventAlternatingBackColor, value);
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00072F92 File Offset: 0x00071F92
		public void ResetAlternatingBackColor()
		{
			if (this.ShouldSerializeAlternatingBackColor())
			{
				this.AlternatingBackColor = DataGridTableStyle.DefaultAlternatingBackBrush.Color;
				this.InvalidateInside();
			}
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00072FB2 File Offset: 0x00071FB2
		protected virtual bool ShouldSerializeAlternatingBackColor()
		{
			return !this.AlternatingBackBrush.Equals(DataGridTableStyle.DefaultAlternatingBackBrush);
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00072FC7 File Offset: 0x00071FC7
		internal SolidBrush AlternatingBackBrush
		{
			get
			{
				return this.alternatingBackBrush;
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x00072FCF File Offset: 0x00071FCF
		protected bool ShouldSerializeBackColor()
		{
			return !DataGridTableStyle.DefaultBackBrush.Equals(this.backBrush);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00072FE4 File Offset: 0x00071FE4
		protected bool ShouldSerializeForeColor()
		{
			return !DataGridTableStyle.DefaultForeBrush.Equals(this.foreBrush);
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x00072FF9 File Offset: 0x00071FF9
		internal SolidBrush BackBrush
		{
			get
			{
				return this.backBrush;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002ACF RID: 10959 RVA: 0x00073001 File Offset: 0x00072001
		// (set) Token: 0x06002AD0 RID: 10960 RVA: 0x00073010 File Offset: 0x00072010
		[SRCategory("CatColors")]
		[SRDescription("ControlBackColorDescr")]
		public Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "BackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "BackColor" }));
				}
				if (!this.backBrush.Color.Equals(value))
				{
					this.backBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x06002AD1 RID: 10961 RVA: 0x000730C4 File Offset: 0x000720C4
		// (remove) Token: 0x06002AD2 RID: 10962 RVA: 0x000730D7 File Offset: 0x000720D7
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventBackColor, value);
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000730EA File Offset: 0x000720EA
		public void ResetBackColor()
		{
			if (!this.backBrush.Equals(DataGridTableStyle.DefaultBackBrush))
			{
				this.BackColor = DataGridTableStyle.DefaultBackBrush.Color;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x00073110 File Offset: 0x00072110
		internal int BorderWidth
		{
			get
			{
				if (this.DataGrid == null)
				{
					return 0;
				}
				DataGridLineStyle dataGridLineStyle;
				int num;
				if (this.IsDefault)
				{
					dataGridLineStyle = this.DataGrid.GridLineStyle;
					num = this.DataGrid.GridLineWidth;
				}
				else
				{
					dataGridLineStyle = this.GridLineStyle;
					num = this.GridLineWidth;
				}
				if (dataGridLineStyle == DataGridLineStyle.None)
				{
					return 0;
				}
				return num;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x0007315F File Offset: 0x0007215F
		internal static SolidBrush DefaultAlternatingBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002AD6 RID: 10966 RVA: 0x0007316B File Offset: 0x0007216B
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Window;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x00073177 File Offset: 0x00072177
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.WindowText;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x00073183 File Offset: 0x00072183
		private static SolidBrush DefaultGridLineBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x0007318F File Offset: 0x0007218F
		private static SolidBrush DefaultHeaderBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.Control;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002ADA RID: 10970 RVA: 0x0007319B File Offset: 0x0007219B
		private static SolidBrush DefaultHeaderForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ControlText;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000731A7 File Offset: 0x000721A7
		private static Pen DefaultHeaderForePen
		{
			get
			{
				return new Pen(SystemColors.ControlText);
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000731B3 File Offset: 0x000721B3
		private static SolidBrush DefaultLinkBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.HotTrack;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000731BF File Offset: 0x000721BF
		private static SolidBrush DefaultSelectionBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002ADE RID: 10974 RVA: 0x000731CB File Offset: 0x000721CB
		private static SolidBrush DefaultSelectionForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000731D7 File Offset: 0x000721D7
		// (set) Token: 0x06002AE0 RID: 10976 RVA: 0x000731E0 File Offset: 0x000721E0
		internal int FocusedRelation
		{
			get
			{
				return this.focusedRelation;
			}
			set
			{
				if (this.focusedRelation != value)
				{
					this.focusedRelation = value;
					if (this.focusedRelation == -1)
					{
						this.focusedTextWidth = 0;
						return;
					}
					Graphics graphics = this.DataGrid.CreateGraphicsInternal();
					this.focusedTextWidth = (int)Math.Ceiling((double)graphics.MeasureString((string)this.RelationsList[this.focusedRelation], this.DataGrid.LinkFont).Width);
					graphics.Dispose();
				}
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x0007325C File Offset: 0x0007225C
		internal int FocusedTextWidth
		{
			get
			{
				return this.focusedTextWidth;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x00073264 File Offset: 0x00072264
		// (set) Token: 0x06002AE3 RID: 10979 RVA: 0x00073274 File Offset: 0x00072274
		[SRDescription("ControlForeColorDescr")]
		[SRCategory("CatColors")]
		public Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "ForeColor" }));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "BackColor" }));
				}
				if (!this.foreBrush.Color.Equals(value))
				{
					this.foreBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x06002AE4 RID: 10980 RVA: 0x00073310 File Offset: 0x00072310
		// (remove) Token: 0x06002AE5 RID: 10981 RVA: 0x00073323 File Offset: 0x00072323
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventForeColor, value);
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x00073336 File Offset: 0x00072336
		internal SolidBrush ForeBrush
		{
			get
			{
				return this.foreBrush;
			}
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x0007333E File Offset: 0x0007233E
		public void ResetForeColor()
		{
			if (!this.foreBrush.Equals(DataGridTableStyle.DefaultForeBrush))
			{
				this.ForeColor = DataGridTableStyle.DefaultForeBrush.Color;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x00073362 File Offset: 0x00072362
		// (set) Token: 0x06002AE9 RID: 10985 RVA: 0x00073370 File Offset: 0x00072370
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "GridLineColor" }));
				}
				if (this.gridLineBrush.Color != value)
				{
					if (value.IsEmpty)
					{
						throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "GridLineColor" }));
					}
					this.gridLineBrush = new SolidBrush(value);
					this.OnGridLineColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x06002AEA RID: 10986 RVA: 0x000733F8 File Offset: 0x000723F8
		// (remove) Token: 0x06002AEB RID: 10987 RVA: 0x0007340B File Offset: 0x0007240B
		public event EventHandler GridLineColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventGridLineColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventGridLineColor, value);
			}
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x0007341E File Offset: 0x0007241E
		protected virtual bool ShouldSerializeGridLineColor()
		{
			return !this.GridLineBrush.Equals(DataGridTableStyle.DefaultGridLineBrush);
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x00073433 File Offset: 0x00072433
		public void ResetGridLineColor()
		{
			if (this.ShouldSerializeGridLineColor())
			{
				this.GridLineColor = DataGridTableStyle.DefaultGridLineBrush.Color;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x0007344D File Offset: 0x0007244D
		internal SolidBrush GridLineBrush
		{
			get
			{
				return this.gridLineBrush;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x00073455 File Offset: 0x00072455
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

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x00073463 File Offset: 0x00072463
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x0007346C File Offset: 0x0007246C
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "GridLineStyle" }));
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.gridLineStyle != value)
				{
					this.gridLineStyle = value;
					this.OnGridLineStyleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x06002AF2 RID: 10994 RVA: 0x000734E2 File Offset: 0x000724E2
		// (remove) Token: 0x06002AF3 RID: 10995 RVA: 0x000734F5 File Offset: 0x000724F5
		public event EventHandler GridLineStyleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventGridLineStyle, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventGridLineStyle, value);
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x00073508 File Offset: 0x00072508
		// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x00073518 File Offset: 0x00072518
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "HeaderBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentHeaderBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "HeaderBackColor" }));
				}
				if (!value.Equals(this.headerBackBrush.Color))
				{
					this.headerBackBrush = new SolidBrush(value);
					this.OnHeaderBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x06002AF6 RID: 10998 RVA: 0x000735C4 File Offset: 0x000725C4
		// (remove) Token: 0x06002AF7 RID: 10999 RVA: 0x000735D7 File Offset: 0x000725D7
		public event EventHandler HeaderBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderBackColor, value);
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000735EA File Offset: 0x000725EA
		internal SolidBrush HeaderBackBrush
		{
			get
			{
				return this.headerBackBrush;
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000735F2 File Offset: 0x000725F2
		protected virtual bool ShouldSerializeHeaderBackColor()
		{
			return !this.HeaderBackBrush.Equals(DataGridTableStyle.DefaultHeaderBackBrush);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x00073607 File Offset: 0x00072607
		public void ResetHeaderBackColor()
		{
			if (this.ShouldSerializeHeaderBackColor())
			{
				this.HeaderBackColor = DataGridTableStyle.DefaultHeaderBackBrush.Color;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x00073621 File Offset: 0x00072621
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x0007364C File Offset: 0x0007264C
		[SRDescription("DataGridHeaderFontDescr")]
		[AmbientValue(null)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		public Font HeaderFont
		{
			get
			{
				if (this.headerFont != null)
				{
					return this.headerFont;
				}
				if (this.DataGrid != null)
				{
					return this.DataGrid.Font;
				}
				return Control.DefaultFont;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "HeaderFont" }));
				}
				if ((value == null && this.headerFont != null) || (value != null && !value.Equals(this.headerFont)))
				{
					this.headerFont = value;
					this.OnHeaderFontChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x06002AFD RID: 11005 RVA: 0x000736AF File Offset: 0x000726AF
		// (remove) Token: 0x06002AFE RID: 11006 RVA: 0x000736C2 File Offset: 0x000726C2
		public event EventHandler HeaderFontChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderFont, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderFont, value);
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000736D5 File Offset: 0x000726D5
		private bool ShouldSerializeHeaderFont()
		{
			return this.headerFont != null;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000736E3 File Offset: 0x000726E3
		public void ResetHeaderFont()
		{
			if (this.headerFont != null)
			{
				this.headerFont = null;
				this.OnHeaderFontChanged(EventArgs.Empty);
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x000736FF File Offset: 0x000726FF
		// (set) Token: 0x06002B02 RID: 11010 RVA: 0x0007370C File Offset: 0x0007270C
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
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "HeaderForeColor" }));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "HeaderForeColor" }));
				}
				if (!value.Equals(this.headerForePen.Color))
				{
					this.headerForePen = new Pen(value);
					this.headerForeBrush = new SolidBrush(value);
					this.OnHeaderForeColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x06002B03 RID: 11011 RVA: 0x000737AC File Offset: 0x000727AC
		// (remove) Token: 0x06002B04 RID: 11012 RVA: 0x000737BF File Offset: 0x000727BF
		public event EventHandler HeaderForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventHeaderForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventHeaderForeColor, value);
			}
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000737D2 File Offset: 0x000727D2
		protected virtual bool ShouldSerializeHeaderForeColor()
		{
			return !this.HeaderForePen.Equals(DataGridTableStyle.DefaultHeaderForePen);
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000737E7 File Offset: 0x000727E7
		public void ResetHeaderForeColor()
		{
			if (this.ShouldSerializeHeaderForeColor())
			{
				this.HeaderForeColor = DataGridTableStyle.DefaultHeaderForeBrush.Color;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x00073801 File Offset: 0x00072801
		internal SolidBrush HeaderForeBrush
		{
			get
			{
				return this.headerForeBrush;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x00073809 File Offset: 0x00072809
		internal Pen HeaderForePen
		{
			get
			{
				return this.headerForePen;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x00073811 File Offset: 0x00072811
		// (set) Token: 0x06002B0A RID: 11018 RVA: 0x00073820 File Offset: 0x00072820
		[SRCategory("CatColors")]
		[SRDescription("DataGridLinkColorDescr")]
		public Color LinkColor
		{
			get
			{
				return this.linkBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "LinkColor" }));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "LinkColor" }));
				}
				if (!this.linkBrush.Color.Equals(value))
				{
					this.linkBrush = new SolidBrush(value);
					this.OnLinkColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06002B0B RID: 11019 RVA: 0x000738B6 File Offset: 0x000728B6
		// (remove) Token: 0x06002B0C RID: 11020 RVA: 0x000738C9 File Offset: 0x000728C9
		public event EventHandler LinkColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventLinkColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventLinkColor, value);
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000738DC File Offset: 0x000728DC
		protected virtual bool ShouldSerializeLinkColor()
		{
			return !this.LinkBrush.Equals(DataGridTableStyle.DefaultLinkBrush);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000738F1 File Offset: 0x000728F1
		public void ResetLinkColor()
		{
			if (this.ShouldSerializeLinkColor())
			{
				this.LinkColor = DataGridTableStyle.DefaultLinkBrush.Color;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x0007390B File Offset: 0x0007290B
		internal Brush LinkBrush
		{
			get
			{
				return this.linkBrush;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x00073913 File Offset: 0x00072913
		// (set) Token: 0x06002B11 RID: 11025 RVA: 0x0007391B File Offset: 0x0007291B
		[SRCategory("CatColors")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRDescription("DataGridLinkHoverColorDescr")]
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

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06002B12 RID: 11026 RVA: 0x0007391D File Offset: 0x0007291D
		// (remove) Token: 0x06002B13 RID: 11027 RVA: 0x00073930 File Offset: 0x00072930
		public event EventHandler LinkHoverColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventLinkHoverColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventLinkHoverColor, value);
			}
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x00073943 File Offset: 0x00072943
		protected virtual bool ShouldSerializeLinkHoverColor()
		{
			return false;
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x00073946 File Offset: 0x00072946
		internal Rectangle RelationshipRect
		{
			get
			{
				if (this.relationshipRect.IsEmpty)
				{
					this.ComputeRelationshipRect();
				}
				return this.relationshipRect;
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x00073964 File Offset: 0x00072964
		private Rectangle ComputeRelationshipRect()
		{
			if (this.relationshipRect.IsEmpty && this.DataGrid.AllowNavigation)
			{
				Graphics graphics = this.DataGrid.CreateGraphicsInternal();
				this.relationshipRect = default(Rectangle);
				this.relationshipRect.X = 0;
				int num = 0;
				for (int i = 0; i < this.RelationsList.Count; i++)
				{
					int num2 = (int)Math.Ceiling((double)graphics.MeasureString((string)this.RelationsList[i], this.DataGrid.LinkFont).Width);
					if (num2 > num)
					{
						num = num2;
					}
				}
				graphics.Dispose();
				this.relationshipRect.Width = num + 5;
				this.relationshipRect.Width = this.relationshipRect.Width + 2;
				this.relationshipRect.Height = this.BorderWidth + this.relationshipHeight * this.RelationsList.Count;
				this.relationshipRect.Height = this.relationshipRect.Height + 2;
				if (this.RelationsList.Count > 0)
				{
					this.relationshipRect.Height = this.relationshipRect.Height + 2;
				}
			}
			return this.relationshipRect;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x00073A8B File Offset: 0x00072A8B
		internal void ResetRelationsUI()
		{
			this.relationshipRect = Rectangle.Empty;
			this.focusedRelation = -1;
			this.relationshipHeight = this.dataGrid.LinkFontHeight + 1;
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x00073AB2 File Offset: 0x00072AB2
		internal int RelationshipHeight
		{
			get
			{
				return this.relationshipHeight;
			}
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x00073ABA File Offset: 0x00072ABA
		public void ResetLinkHoverColor()
		{
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x00073ABC File Offset: 0x00072ABC
		// (set) Token: 0x06002B1B RID: 11035 RVA: 0x00073AC4 File Offset: 0x00072AC4
		[TypeConverter(typeof(DataGridPreferredColumnWidthTypeConverter))]
		[DefaultValue(75)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("DataGridPreferredColumnWidthDescr")]
		public int PreferredColumnWidth
		{
			get
			{
				return this.preferredColumnWidth;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "PreferredColumnWidth" }));
				}
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridColumnWidth"), "PreferredColumnWidth");
				}
				if (this.preferredColumnWidth != value)
				{
					this.preferredColumnWidth = value;
					this.OnPreferredColumnWidthChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x06002B1C RID: 11036 RVA: 0x00073B2D File Offset: 0x00072B2D
		// (remove) Token: 0x06002B1D RID: 11037 RVA: 0x00073B40 File Offset: 0x00072B40
		public event EventHandler PreferredColumnWidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventPreferredColumnWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventPreferredColumnWidth, value);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x00073B53 File Offset: 0x00072B53
		// (set) Token: 0x06002B1F RID: 11039 RVA: 0x00073B5C File Offset: 0x00072B5C
		[SRDescription("DataGridPreferredRowHeightDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		public int PreferredRowHeight
		{
			get
			{
				return this.prefferedRowHeight;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "PrefferedRowHeight" }));
				}
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("DataGridRowRowHeight"));
				}
				this.prefferedRowHeight = value;
				this.OnPreferredRowHeightChanged(EventArgs.Empty);
			}
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x06002B20 RID: 11040 RVA: 0x00073BB7 File Offset: 0x00072BB7
		// (remove) Token: 0x06002B21 RID: 11041 RVA: 0x00073BCA File Offset: 0x00072BCA
		public event EventHandler PreferredRowHeightChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventPreferredRowHeight, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventPreferredRowHeight, value);
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00073BDD File Offset: 0x00072BDD
		private void ResetPreferredRowHeight()
		{
			this.PreferredRowHeight = DataGridTableStyle.defaultFontHeight + 3;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x00073BEC File Offset: 0x00072BEC
		protected bool ShouldSerializePreferredRowHeight()
		{
			return this.prefferedRowHeight != DataGridTableStyle.defaultFontHeight + 3;
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002B24 RID: 11044 RVA: 0x00073C00 File Offset: 0x00072C00
		// (set) Token: 0x06002B25 RID: 11045 RVA: 0x00073C08 File Offset: 0x00072C08
		[SRDescription("DataGridColumnHeadersVisibleDescr")]
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		public bool ColumnHeadersVisible
		{
			get
			{
				return this.columnHeadersVisible;
			}
			set
			{
				if (this.columnHeadersVisible != value)
				{
					this.columnHeadersVisible = value;
					this.OnColumnHeadersVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x06002B26 RID: 11046 RVA: 0x00073C25 File Offset: 0x00072C25
		// (remove) Token: 0x06002B27 RID: 11047 RVA: 0x00073C38 File Offset: 0x00072C38
		public event EventHandler ColumnHeadersVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventColumnHeadersVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventColumnHeadersVisible, value);
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x00073C4B File Offset: 0x00072C4B
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x00073C53 File Offset: 0x00072C53
		[SRDescription("DataGridRowHeadersVisibleDescr")]
		[SRCategory("CatDisplay")]
		[DefaultValue(true)]
		public bool RowHeadersVisible
		{
			get
			{
				return this.rowHeadersVisible;
			}
			set
			{
				if (this.rowHeadersVisible != value)
				{
					this.rowHeadersVisible = value;
					this.OnRowHeadersVisibleChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x06002B2A RID: 11050 RVA: 0x00073C70 File Offset: 0x00072C70
		// (remove) Token: 0x06002B2B RID: 11051 RVA: 0x00073C83 File Offset: 0x00072C83
		public event EventHandler RowHeadersVisibleChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventRowHeadersVisible, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventRowHeadersVisible, value);
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x00073C96 File Offset: 0x00072C96
		// (set) Token: 0x06002B2D RID: 11053 RVA: 0x00073C9E File Offset: 0x00072C9E
		[DefaultValue(35)]
		[Localizable(true)]
		[SRDescription("DataGridRowHeaderWidthDescr")]
		[SRCategory("CatLayout")]
		public int RowHeaderWidth
		{
			get
			{
				return this.rowHeaderWidth;
			}
			set
			{
				if (this.DataGrid != null)
				{
					value = Math.Max(this.DataGrid.MinimumRowHeaderWidth(), value);
				}
				if (this.rowHeaderWidth != value)
				{
					this.rowHeaderWidth = value;
					this.OnRowHeaderWidthChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x06002B2E RID: 11054 RVA: 0x00073CD6 File Offset: 0x00072CD6
		// (remove) Token: 0x06002B2F RID: 11055 RVA: 0x00073CE9 File Offset: 0x00072CE9
		public event EventHandler RowHeaderWidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventRowHeaderWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventRowHeaderWidth, value);
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x00073CFC File Offset: 0x00072CFC
		// (set) Token: 0x06002B31 RID: 11057 RVA: 0x00073D0C File Offset: 0x00072D0C
		[SRCategory("CatColors")]
		[SRDescription("DataGridSelectionBackColorDescr")]
		public Color SelectionBackColor
		{
			get
			{
				return this.selectionBackBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "SelectionBackColor" }));
				}
				if (DataGrid.IsTransparentColor(value))
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleTransparentSelectionBackColorNotAllowed"));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "SelectionBackColor" }));
				}
				if (!value.Equals(this.selectionBackBrush.Color))
				{
					this.selectionBackBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnSelectionBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x06002B32 RID: 11058 RVA: 0x00073DBE File Offset: 0x00072DBE
		// (remove) Token: 0x06002B33 RID: 11059 RVA: 0x00073DD1 File Offset: 0x00072DD1
		public event EventHandler SelectionBackColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventSelectionBackColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventSelectionBackColor, value);
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002B34 RID: 11060 RVA: 0x00073DE4 File Offset: 0x00072DE4
		internal SolidBrush SelectionBackBrush
		{
			get
			{
				return this.selectionBackBrush;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002B35 RID: 11061 RVA: 0x00073DEC File Offset: 0x00072DEC
		internal SolidBrush SelectionForeBrush
		{
			get
			{
				return this.selectionForeBrush;
			}
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x00073DF4 File Offset: 0x00072DF4
		protected bool ShouldSerializeSelectionBackColor()
		{
			return !DataGridTableStyle.DefaultSelectionBackBrush.Equals(this.selectionBackBrush);
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x00073E09 File Offset: 0x00072E09
		public void ResetSelectionBackColor()
		{
			if (this.ShouldSerializeSelectionBackColor())
			{
				this.SelectionBackColor = DataGridTableStyle.DefaultSelectionBackBrush.Color;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x00073E23 File Offset: 0x00072E23
		// (set) Token: 0x06002B39 RID: 11065 RVA: 0x00073E30 File Offset: 0x00072E30
		[SRCategory("CatColors")]
		[Description("The foreground color for the current data grid row")]
		[SRDescription("DataGridSelectionForeColorDescr")]
		public Color SelectionForeColor
		{
			get
			{
				return this.selectionForeBrush.Color;
			}
			set
			{
				if (this.isDefaultTableStyle)
				{
					throw new ArgumentException(SR.GetString("DataGridDefaultTableSet", new object[] { "SelectionForeColor" }));
				}
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "SelectionForeColor" }));
				}
				if (!value.Equals(this.selectionForeBrush.Color))
				{
					this.selectionForeBrush = new SolidBrush(value);
					this.InvalidateInside();
					this.OnSelectionForeColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x06002B3A RID: 11066 RVA: 0x00073ECA File Offset: 0x00072ECA
		// (remove) Token: 0x06002B3B RID: 11067 RVA: 0x00073EDD File Offset: 0x00072EDD
		public event EventHandler SelectionForeColorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventSelectionForeColor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventSelectionForeColor, value);
			}
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x00073EF0 File Offset: 0x00072EF0
		protected virtual bool ShouldSerializeSelectionForeColor()
		{
			return !this.SelectionForeBrush.Equals(DataGridTableStyle.DefaultSelectionForeBrush);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x00073F05 File Offset: 0x00072F05
		public void ResetSelectionForeColor()
		{
			if (this.ShouldSerializeSelectionForeColor())
			{
				this.SelectionForeColor = DataGridTableStyle.DefaultSelectionForeBrush.Color;
			}
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x00073F1F File Offset: 0x00072F1F
		private void InvalidateInside()
		{
			if (this.DataGrid != null)
			{
				this.DataGrid.InvalidateInside();
			}
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x00073F34 File Offset: 0x00072F34
		public DataGridTableStyle(bool isDefaultTableStyle)
		{
			this.gridColumns = new GridColumnStylesCollection(this, isDefaultTableStyle);
			this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
			this.isDefaultTableStyle = isDefaultTableStyle;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x00074042 File Offset: 0x00073042
		public DataGridTableStyle()
			: this(false)
		{
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x0007404B File Offset: 0x0007304B
		public DataGridTableStyle(CurrencyManager listManager)
			: this()
		{
			this.mappingName = listManager.GetListName();
			this.SetGridColumnStylesCollection(listManager);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x00074068 File Offset: 0x00073068
		internal void SetRelationsList(CurrencyManager listManager)
		{
			PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
			int count = itemProperties.Count;
			if (this.relationsList.Count > 0)
			{
				this.relationsList.Clear();
			}
			for (int i = 0; i < count; i++)
			{
				PropertyDescriptor propertyDescriptor = itemProperties[i];
				if (DataGridTableStyle.PropertyDescriptorIsARelation(propertyDescriptor))
				{
					this.relationsList.Add(propertyDescriptor.Name);
				}
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000740CC File Offset: 0x000730CC
		internal void SetGridColumnStylesCollection(CurrencyManager listManager)
		{
			this.gridColumns.CollectionChanged -= this.OnColumnCollectionChanged;
			PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
			if (this.relationsList.Count > 0)
			{
				this.relationsList.Clear();
			}
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				PropertyDescriptor propertyDescriptor = itemProperties[i];
				if (propertyDescriptor.IsBrowsable)
				{
					if (DataGridTableStyle.PropertyDescriptorIsARelation(propertyDescriptor))
					{
						this.relationsList.Add(propertyDescriptor.Name);
					}
					else
					{
						DataGridColumnStyle dataGridColumnStyle = this.CreateGridColumn(propertyDescriptor, this.isDefaultTableStyle);
						if (this.isDefaultTableStyle)
						{
							this.gridColumns.AddDefaultColumn(dataGridColumnStyle);
						}
						else
						{
							dataGridColumnStyle.MappingName = propertyDescriptor.Name;
							dataGridColumnStyle.HeaderText = propertyDescriptor.Name;
							this.gridColumns.Add(dataGridColumnStyle);
						}
					}
				}
			}
			this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000741B7 File Offset: 0x000731B7
		private static bool PropertyDescriptorIsARelation(PropertyDescriptor prop)
		{
			return typeof(IList).IsAssignableFrom(prop.PropertyType) && !typeof(Array).IsAssignableFrom(prop.PropertyType);
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000741EA File Offset: 0x000731EA
		protected internal virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop)
		{
			return this.CreateGridColumn(prop, false);
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000741F4 File Offset: 0x000731F4
		protected internal virtual DataGridColumnStyle CreateGridColumn(PropertyDescriptor prop, bool isDefault)
		{
			Type propertyType = prop.PropertyType;
			DataGridColumnStyle dataGridColumnStyle;
			if (propertyType.Equals(typeof(bool)))
			{
				dataGridColumnStyle = new DataGridBoolColumn(prop, isDefault);
			}
			else if (propertyType.Equals(typeof(string)))
			{
				dataGridColumnStyle = new DataGridTextBoxColumn(prop, isDefault);
			}
			else if (propertyType.Equals(typeof(DateTime)))
			{
				dataGridColumnStyle = new DataGridTextBoxColumn(prop, "d", isDefault);
			}
			else if (propertyType.Equals(typeof(short)) || propertyType.Equals(typeof(int)) || propertyType.Equals(typeof(long)) || propertyType.Equals(typeof(ushort)) || propertyType.Equals(typeof(uint)) || propertyType.Equals(typeof(ulong)) || propertyType.Equals(typeof(decimal)) || propertyType.Equals(typeof(double)) || propertyType.Equals(typeof(float)) || propertyType.Equals(typeof(byte)) || propertyType.Equals(typeof(sbyte)))
			{
				dataGridColumnStyle = new DataGridTextBoxColumn(prop, "G", isDefault);
			}
			else
			{
				dataGridColumnStyle = new DataGridTextBoxColumn(prop, isDefault);
			}
			return dataGridColumnStyle;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x00074353 File Offset: 0x00073353
		internal void ResetRelationsList()
		{
			if (this.isDefaultTableStyle)
			{
				this.relationsList.Clear();
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002B48 RID: 11080 RVA: 0x00074368 File Offset: 0x00073368
		// (set) Token: 0x06002B49 RID: 11081 RVA: 0x00074370 File Offset: 0x00073370
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataGridTableStyleMappingNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string MappingName
		{
			get
			{
				return this.mappingName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Equals(this.mappingName))
				{
					return;
				}
				string text = this.MappingName;
				this.mappingName = value;
				try
				{
					if (this.DataGrid != null)
					{
						this.DataGrid.TableStyles.CheckForMappingNameDuplicates(this);
					}
				}
				catch
				{
					this.mappingName = text;
					throw;
				}
				this.OnMappingNameChanged(EventArgs.Empty);
			}
		}

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x06002B4A RID: 11082 RVA: 0x000743E4 File Offset: 0x000733E4
		// (remove) Token: 0x06002B4B RID: 11083 RVA: 0x000743F7 File Offset: 0x000733F7
		public event EventHandler MappingNameChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventMappingName, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventMappingName, value);
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x0007440A File Offset: 0x0007340A
		internal ArrayList RelationsList
		{
			get
			{
				return this.relationsList;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x00074412 File Offset: 0x00073412
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		public virtual GridColumnStylesCollection GridColumnStyles
		{
			get
			{
				return this.gridColumns;
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0007441C File Offset: 0x0007341C
		internal void SetInternalDataGrid(DataGrid dG, bool force)
		{
			if (this.dataGrid != null && this.dataGrid.Equals(dG) && !force)
			{
				return;
			}
			this.dataGrid = dG;
			if (dG != null && dG.Initializing)
			{
				return;
			}
			int count = this.gridColumns.Count;
			for (int i = 0; i < count; i++)
			{
				this.gridColumns[i].SetDataGridInternalInColumn(dG);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x00074480 File Offset: 0x00073480
		// (set) Token: 0x06002B50 RID: 11088 RVA: 0x00074488 File Offset: 0x00073488
		[Browsable(false)]
		public virtual DataGrid DataGrid
		{
			get
			{
				return this.dataGrid;
			}
			set
			{
				this.SetInternalDataGrid(value, true);
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x00074492 File Offset: 0x00073492
		// (set) Token: 0x06002B52 RID: 11090 RVA: 0x0007449A File Offset: 0x0007349A
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				if (this.readOnly != value)
				{
					this.readOnly = value;
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x06002B53 RID: 11091 RVA: 0x000744B7 File Offset: 0x000734B7
		// (remove) Token: 0x06002B54 RID: 11092 RVA: 0x000744CA File Offset: 0x000734CA
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGridTableStyle.EventReadOnly, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridTableStyle.EventReadOnly, value);
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000744E0 File Offset: 0x000734E0
		public bool BeginEdit(DataGridColumnStyle gridColumn, int rowNumber)
		{
			DataGrid dataGrid = this.DataGrid;
			return dataGrid != null && dataGrid.BeginEdit(gridColumn, rowNumber);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x00074504 File Offset: 0x00073504
		public bool EndEdit(DataGridColumnStyle gridColumn, int rowNumber, bool shouldAbort)
		{
			DataGrid dataGrid = this.DataGrid;
			return dataGrid != null && dataGrid.EndEdit(gridColumn, rowNumber, shouldAbort);
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x00074528 File Offset: 0x00073528
		internal void InvalidateColumn(DataGridColumnStyle column)
		{
			int num = this.GridColumnStyles.IndexOf(column);
			if (num >= 0 && this.DataGrid != null)
			{
				this.DataGrid.InvalidateColumn(num);
			}
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x0007455C File Offset: 0x0007355C
		private void OnColumnCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			this.gridColumns.CollectionChanged -= this.OnColumnCollectionChanged;
			try
			{
				DataGrid dataGrid = this.DataGrid;
				DataGridColumnStyle dataGridColumnStyle = e.Element as DataGridColumnStyle;
				if (e.Action == CollectionChangeAction.Add)
				{
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.SetDataGridInternalInColumn(dataGrid);
					}
				}
				else if (e.Action == CollectionChangeAction.Remove)
				{
					if (dataGridColumnStyle != null)
					{
						dataGridColumnStyle.SetDataGridInternalInColumn(null);
					}
				}
				else if (e.Element != null)
				{
					for (int i = 0; i < this.gridColumns.Count; i++)
					{
						this.gridColumns[i].SetDataGridInternalInColumn(null);
					}
				}
				if (dataGrid != null)
				{
					dataGrid.OnColumnCollectionChanged(this, e);
				}
			}
			finally
			{
				this.gridColumns.CollectionChanged += this.OnColumnCollectionChanged;
			}
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x00074624 File Offset: 0x00073624
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventReadOnly] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x00074654 File Offset: 0x00073654
		protected virtual void OnMappingNameChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventMappingName] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x00074684 File Offset: 0x00073684
		protected virtual void OnAlternatingBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventAlternatingBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000746B4 File Offset: 0x000736B4
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000746E4 File Offset: 0x000736E4
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x00074714 File Offset: 0x00073714
		protected virtual void OnAllowSortingChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventAllowSorting] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x00074744 File Offset: 0x00073744
		protected virtual void OnGridLineColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventGridLineColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00074774 File Offset: 0x00073774
		protected virtual void OnGridLineStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventGridLineStyle] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000747A4 File Offset: 0x000737A4
		protected virtual void OnHeaderBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000747D4 File Offset: 0x000737D4
		protected virtual void OnHeaderFontChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderFont] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x00074804 File Offset: 0x00073804
		protected virtual void OnHeaderForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventHeaderForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x00074834 File Offset: 0x00073834
		protected virtual void OnLinkColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventLinkColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x00074864 File Offset: 0x00073864
		protected virtual void OnLinkHoverColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventLinkHoverColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x00074894 File Offset: 0x00073894
		protected virtual void OnPreferredRowHeightChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventPreferredRowHeight] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000748C4 File Offset: 0x000738C4
		protected virtual void OnPreferredColumnWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventPreferredColumnWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000748F4 File Offset: 0x000738F4
		protected virtual void OnColumnHeadersVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventColumnHeadersVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x00074924 File Offset: 0x00073924
		protected virtual void OnRowHeadersVisibleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventRowHeadersVisible] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x00074954 File Offset: 0x00073954
		protected virtual void OnRowHeaderWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventRowHeaderWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00074984 File Offset: 0x00073984
		protected virtual void OnSelectionForeColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventSelectionForeColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000749B4 File Offset: 0x000739B4
		protected virtual void OnSelectionBackColorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridTableStyle.EventSelectionBackColor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000749E4 File Offset: 0x000739E4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GridColumnStylesCollection gridColumnStyles = this.GridColumnStyles;
				if (gridColumnStyles != null)
				{
					for (int i = 0; i < gridColumnStyles.Count; i++)
					{
						gridColumnStyles[i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002B6E RID: 11118 RVA: 0x00074A22 File Offset: 0x00073A22
		internal bool IsDefault
		{
			get
			{
				return this.isDefaultTableStyle;
			}
		}

		// Token: 0x040017CF RID: 6095
		internal const int relationshipSpacing = 1;

		// Token: 0x040017D0 RID: 6096
		private const bool defaultAllowSorting = true;

		// Token: 0x040017D1 RID: 6097
		private const DataGridLineStyle defaultGridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040017D2 RID: 6098
		private const int defaultPreferredColumnWidth = 75;

		// Token: 0x040017D3 RID: 6099
		private const int defaultRowHeaderWidth = 35;

		// Token: 0x040017D4 RID: 6100
		internal DataGrid dataGrid;

		// Token: 0x040017D5 RID: 6101
		private int relationshipHeight;

		// Token: 0x040017D6 RID: 6102
		private Rectangle relationshipRect = Rectangle.Empty;

		// Token: 0x040017D7 RID: 6103
		private int focusedRelation = -1;

		// Token: 0x040017D8 RID: 6104
		private int focusedTextWidth;

		// Token: 0x040017D9 RID: 6105
		private ArrayList relationsList = new ArrayList(2);

		// Token: 0x040017DA RID: 6106
		private string mappingName = "";

		// Token: 0x040017DB RID: 6107
		private GridColumnStylesCollection gridColumns;

		// Token: 0x040017DC RID: 6108
		private bool readOnly;

		// Token: 0x040017DD RID: 6109
		private bool isDefaultTableStyle;

		// Token: 0x040017DE RID: 6110
		private static readonly object EventAllowSorting = new object();

		// Token: 0x040017DF RID: 6111
		private static readonly object EventGridLineColor = new object();

		// Token: 0x040017E0 RID: 6112
		private static readonly object EventGridLineStyle = new object();

		// Token: 0x040017E1 RID: 6113
		private static readonly object EventHeaderBackColor = new object();

		// Token: 0x040017E2 RID: 6114
		private static readonly object EventHeaderForeColor = new object();

		// Token: 0x040017E3 RID: 6115
		private static readonly object EventHeaderFont = new object();

		// Token: 0x040017E4 RID: 6116
		private static readonly object EventLinkColor = new object();

		// Token: 0x040017E5 RID: 6117
		private static readonly object EventLinkHoverColor = new object();

		// Token: 0x040017E6 RID: 6118
		private static readonly object EventPreferredColumnWidth = new object();

		// Token: 0x040017E7 RID: 6119
		private static readonly object EventPreferredRowHeight = new object();

		// Token: 0x040017E8 RID: 6120
		private static readonly object EventColumnHeadersVisible = new object();

		// Token: 0x040017E9 RID: 6121
		private static readonly object EventRowHeaderWidth = new object();

		// Token: 0x040017EA RID: 6122
		private static readonly object EventSelectionBackColor = new object();

		// Token: 0x040017EB RID: 6123
		private static readonly object EventSelectionForeColor = new object();

		// Token: 0x040017EC RID: 6124
		private static readonly object EventMappingName = new object();

		// Token: 0x040017ED RID: 6125
		private static readonly object EventAlternatingBackColor = new object();

		// Token: 0x040017EE RID: 6126
		private static readonly object EventBackColor = new object();

		// Token: 0x040017EF RID: 6127
		private static readonly object EventForeColor = new object();

		// Token: 0x040017F0 RID: 6128
		private static readonly object EventReadOnly = new object();

		// Token: 0x040017F1 RID: 6129
		private static readonly object EventRowHeadersVisible = new object();

		// Token: 0x040017F2 RID: 6130
		internal static readonly Font defaultFont = Control.DefaultFont;

		// Token: 0x040017F3 RID: 6131
		internal static readonly int defaultFontHeight = DataGridTableStyle.defaultFont.Height;

		// Token: 0x040017F4 RID: 6132
		private bool allowSorting = true;

		// Token: 0x040017F5 RID: 6133
		private SolidBrush alternatingBackBrush = DataGridTableStyle.DefaultAlternatingBackBrush;

		// Token: 0x040017F6 RID: 6134
		private SolidBrush backBrush = DataGridTableStyle.DefaultBackBrush;

		// Token: 0x040017F7 RID: 6135
		private SolidBrush foreBrush = DataGridTableStyle.DefaultForeBrush;

		// Token: 0x040017F8 RID: 6136
		private SolidBrush gridLineBrush = DataGridTableStyle.DefaultGridLineBrush;

		// Token: 0x040017F9 RID: 6137
		private DataGridLineStyle gridLineStyle = DataGridLineStyle.Solid;

		// Token: 0x040017FA RID: 6138
		internal SolidBrush headerBackBrush = DataGridTableStyle.DefaultHeaderBackBrush;

		// Token: 0x040017FB RID: 6139
		internal Font headerFont;

		// Token: 0x040017FC RID: 6140
		internal SolidBrush headerForeBrush = DataGridTableStyle.DefaultHeaderForeBrush;

		// Token: 0x040017FD RID: 6141
		internal Pen headerForePen = DataGridTableStyle.DefaultHeaderForePen;

		// Token: 0x040017FE RID: 6142
		private SolidBrush linkBrush = DataGridTableStyle.DefaultLinkBrush;

		// Token: 0x040017FF RID: 6143
		internal int preferredColumnWidth = 75;

		// Token: 0x04001800 RID: 6144
		private int prefferedRowHeight = DataGridTableStyle.defaultFontHeight + 3;

		// Token: 0x04001801 RID: 6145
		private SolidBrush selectionBackBrush = DataGridTableStyle.DefaultSelectionBackBrush;

		// Token: 0x04001802 RID: 6146
		private SolidBrush selectionForeBrush = DataGridTableStyle.DefaultSelectionForeBrush;

		// Token: 0x04001803 RID: 6147
		private int rowHeaderWidth = 35;

		// Token: 0x04001804 RID: 6148
		private bool rowHeadersVisible = true;

		// Token: 0x04001805 RID: 6149
		private bool columnHeadersVisible = true;

		// Token: 0x04001806 RID: 6150
		public static readonly DataGridTableStyle DefaultTableStyle = new DataGridTableStyle(true);
	}
}
