using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004DC RID: 1244
	[ControlValueProperty("SelectedDate", typeof(DateTime), "1/1/0001")]
	[SupportsEventValidation]
	[DataBindingHandler("System.Web.UI.Design.WebControls.CalendarDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("SelectionChanged")]
	[DefaultProperty("SelectedDate")]
	[Designer("System.Web.UI.Design.WebControls.CalendarDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Calendar : WebControl, IPostBackEventHandler
	{
		// Token: 0x06003BBF RID: 15295 RVA: 0x000FB202 File Offset: 0x000FA202
		public Calendar()
			: base(HtmlTextWriterTag.Table)
		{
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003BC0 RID: 15296 RVA: 0x000FB20C File Offset: 0x000FA20C
		// (set) Token: 0x06003BC1 RID: 15297 RVA: 0x000FB239 File Offset: 0x000FA239
		[Localizable(true)]
		[WebCategory("Accessibility")]
		[DefaultValue("")]
		[WebSysDescription("Calendar_Caption")]
		public virtual string Caption
		{
			get
			{
				string text = (string)this.ViewState["Caption"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Caption"] = value;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003BC2 RID: 15298 RVA: 0x000FB24C File Offset: 0x000FA24C
		// (set) Token: 0x06003BC3 RID: 15299 RVA: 0x000FB275 File Offset: 0x000FA275
		[WebSysDescription("WebControl_CaptionAlign")]
		[DefaultValue(TableCaptionAlign.NotSet)]
		[WebCategory("Accessibility")]
		public virtual TableCaptionAlign CaptionAlign
		{
			get
			{
				object obj = this.ViewState["CaptionAlign"];
				if (obj == null)
				{
					return TableCaptionAlign.NotSet;
				}
				return (TableCaptionAlign)obj;
			}
			set
			{
				if (value < TableCaptionAlign.NotSet || value > TableCaptionAlign.Right)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CaptionAlign"] = value;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x000FB2A0 File Offset: 0x000FA2A0
		// (set) Token: 0x06003BC5 RID: 15301 RVA: 0x000FB2C9 File Offset: 0x000FA2C9
		[WebCategory("Layout")]
		[WebSysDescription("Calendar_CellPadding")]
		[DefaultValue(2)]
		public int CellPadding
		{
			get
			{
				object obj = this.ViewState["CellPadding"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 2;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CellPadding"] = value;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x000FB2F0 File Offset: 0x000FA2F0
		// (set) Token: 0x06003BC7 RID: 15303 RVA: 0x000FB319 File Offset: 0x000FA319
		[WebSysDescription("Calendar_CellSpacing")]
		[WebCategory("Layout")]
		[DefaultValue(0)]
		public int CellSpacing
		{
			get
			{
				object obj = this.ViewState["CellSpacing"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CellSpacing"] = value;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x000FB340 File Offset: 0x000FA340
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[WebSysDescription("Calendar_DayHeaderStyle")]
		[NotifyParentProperty(true)]
		public TableItemStyle DayHeaderStyle
		{
			get
			{
				if (this.dayHeaderStyle == null)
				{
					this.dayHeaderStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.dayHeaderStyle).TrackViewState();
					}
				}
				return this.dayHeaderStyle;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x000FB370 File Offset: 0x000FA370
		// (set) Token: 0x06003BCA RID: 15306 RVA: 0x000FB399 File Offset: 0x000FA399
		[WebCategory("Appearance")]
		[WebSysDescription("Calendar_DayNameFormat")]
		[DefaultValue(DayNameFormat.Short)]
		public DayNameFormat DayNameFormat
		{
			get
			{
				object obj = this.ViewState["DayNameFormat"];
				if (obj != null)
				{
					return (DayNameFormat)obj;
				}
				return DayNameFormat.Short;
			}
			set
			{
				if (value < DayNameFormat.Full || value > DayNameFormat.Shortest)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["DayNameFormat"] = value;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003BCB RID: 15307 RVA: 0x000FB3C4 File Offset: 0x000FA3C4
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Calendar_DayStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public TableItemStyle DayStyle
		{
			get
			{
				if (this.dayStyle == null)
				{
					this.dayStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.dayStyle).TrackViewState();
					}
				}
				return this.dayStyle;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003BCC RID: 15308 RVA: 0x000FB3F4 File Offset: 0x000FA3F4
		// (set) Token: 0x06003BCD RID: 15309 RVA: 0x000FB41D File Offset: 0x000FA41D
		[WebCategory("Appearance")]
		[WebSysDescription("Calendar_FirstDayOfWeek")]
		[DefaultValue(FirstDayOfWeek.Default)]
		public FirstDayOfWeek FirstDayOfWeek
		{
			get
			{
				object obj = this.ViewState["FirstDayOfWeek"];
				if (obj != null)
				{
					return (FirstDayOfWeek)obj;
				}
				return FirstDayOfWeek.Default;
			}
			set
			{
				if (value < FirstDayOfWeek.Sunday || value > FirstDayOfWeek.Default)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["FirstDayOfWeek"] = value;
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003BCE RID: 15310 RVA: 0x000FB448 File Offset: 0x000FA448
		// (set) Token: 0x06003BCF RID: 15311 RVA: 0x000FB475 File Offset: 0x000FA475
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("&gt;")]
		[WebSysDescription("Calendar_NextMonthText")]
		public string NextMonthText
		{
			get
			{
				object obj = this.ViewState["NextMonthText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&gt;";
			}
			set
			{
				this.ViewState["NextMonthText"] = value;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x000FB488 File Offset: 0x000FA488
		// (set) Token: 0x06003BD1 RID: 15313 RVA: 0x000FB4B1 File Offset: 0x000FA4B1
		[WebSysDescription("Calendar_NextPrevFormat")]
		[WebCategory("Appearance")]
		[DefaultValue(NextPrevFormat.CustomText)]
		public NextPrevFormat NextPrevFormat
		{
			get
			{
				object obj = this.ViewState["NextPrevFormat"];
				if (obj != null)
				{
					return (NextPrevFormat)obj;
				}
				return NextPrevFormat.CustomText;
			}
			set
			{
				if (value < NextPrevFormat.CustomText || value > NextPrevFormat.FullMonth)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["NextPrevFormat"] = value;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x000FB4DC File Offset: 0x000FA4DC
		[WebCategory("Styles")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Calendar_NextPrevStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		public TableItemStyle NextPrevStyle
		{
			get
			{
				if (this.nextPrevStyle == null)
				{
					this.nextPrevStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.nextPrevStyle).TrackViewState();
					}
				}
				return this.nextPrevStyle;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x000FB50A File Offset: 0x000FA50A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[WebSysDescription("Calendar_OtherMonthDayStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		public TableItemStyle OtherMonthDayStyle
		{
			get
			{
				if (this.otherMonthDayStyle == null)
				{
					this.otherMonthDayStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.otherMonthDayStyle).TrackViewState();
					}
				}
				return this.otherMonthDayStyle;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003BD4 RID: 15316 RVA: 0x000FB538 File Offset: 0x000FA538
		// (set) Token: 0x06003BD5 RID: 15317 RVA: 0x000FB565 File Offset: 0x000FA565
		[DefaultValue("&lt;")]
		[WebSysDescription("Calendar_PrevMonthText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		public string PrevMonthText
		{
			get
			{
				object obj = this.ViewState["PrevMonthText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&lt;";
			}
			set
			{
				this.ViewState["PrevMonthText"] = value;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x000FB578 File Offset: 0x000FA578
		// (set) Token: 0x06003BD7 RID: 15319 RVA: 0x000FB599 File Offset: 0x000FA599
		[WebSysDescription("Calendar_SelectedDate")]
		[Bindable(true, BindingDirection.TwoWay)]
		[DefaultValue(typeof(DateTime), "1/1/0001")]
		public DateTime SelectedDate
		{
			get
			{
				if (this.SelectedDates.Count == 0)
				{
					return DateTime.MinValue;
				}
				return this.SelectedDates[0];
			}
			set
			{
				if (value == DateTime.MinValue)
				{
					this.SelectedDates.Clear();
					return;
				}
				this.SelectedDates.SelectRange(value, value);
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x000FB5C1 File Offset: 0x000FA5C1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[WebSysDescription("Calendar_SelectedDates")]
		public SelectedDatesCollection SelectedDates
		{
			get
			{
				if (this.selectedDates == null)
				{
					if (this.dateList == null)
					{
						this.dateList = new ArrayList();
					}
					this.selectedDates = new SelectedDatesCollection(this.dateList);
				}
				return this.selectedDates;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x000FB5F5 File Offset: 0x000FA5F5
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Calendar_SelectedDayStyle")]
		[WebCategory("Styles")]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableItemStyle SelectedDayStyle
		{
			get
			{
				if (this.selectedDayStyle == null)
				{
					this.selectedDayStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.selectedDayStyle).TrackViewState();
					}
				}
				return this.selectedDayStyle;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003BDA RID: 15322 RVA: 0x000FB624 File Offset: 0x000FA624
		// (set) Token: 0x06003BDB RID: 15323 RVA: 0x000FB64D File Offset: 0x000FA64D
		[WebSysDescription("Calendar_SelectionMode")]
		[DefaultValue(CalendarSelectionMode.Day)]
		[WebCategory("Behavior")]
		public CalendarSelectionMode SelectionMode
		{
			get
			{
				object obj = this.ViewState["SelectionMode"];
				if (obj != null)
				{
					return (CalendarSelectionMode)obj;
				}
				return CalendarSelectionMode.Day;
			}
			set
			{
				if (value < CalendarSelectionMode.None || value > CalendarSelectionMode.DayWeekMonth)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["SelectionMode"] = value;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x000FB678 File Offset: 0x000FA678
		// (set) Token: 0x06003BDD RID: 15325 RVA: 0x000FB6A5 File Offset: 0x000FA6A5
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("Calendar_SelectMonthText")]
		[DefaultValue("&gt;&gt;")]
		public string SelectMonthText
		{
			get
			{
				object obj = this.ViewState["SelectMonthText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&gt;&gt;";
			}
			set
			{
				this.ViewState["SelectMonthText"] = value;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x000FB6B8 File Offset: 0x000FA6B8
		[WebSysDescription("Calendar_SelectorStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableItemStyle SelectorStyle
		{
			get
			{
				if (this.selectorStyle == null)
				{
					this.selectorStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.selectorStyle).TrackViewState();
					}
				}
				return this.selectorStyle;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x000FB6E8 File Offset: 0x000FA6E8
		// (set) Token: 0x06003BE0 RID: 15328 RVA: 0x000FB715 File Offset: 0x000FA715
		[DefaultValue("&gt;")]
		[WebSysDescription("Calendar_SelectWeekText")]
		[WebCategory("Appearance")]
		[Localizable(true)]
		public string SelectWeekText
		{
			get
			{
				object obj = this.ViewState["SelectWeekText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&gt;";
			}
			set
			{
				this.ViewState["SelectWeekText"] = value;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x000FB728 File Offset: 0x000FA728
		// (set) Token: 0x06003BE2 RID: 15330 RVA: 0x000FB751 File Offset: 0x000FA751
		[WebCategory("Appearance")]
		[WebSysDescription("Calendar_ShowDayHeader")]
		[DefaultValue(true)]
		public bool ShowDayHeader
		{
			get
			{
				object obj = this.ViewState["ShowDayHeader"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowDayHeader"] = value;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x000FB76C File Offset: 0x000FA76C
		// (set) Token: 0x06003BE4 RID: 15332 RVA: 0x000FB795 File Offset: 0x000FA795
		[WebCategory("Appearance")]
		[DefaultValue(false)]
		[WebSysDescription("Calendar_ShowGridLines")]
		public bool ShowGridLines
		{
			get
			{
				object obj = this.ViewState["ShowGridLines"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ShowGridLines"] = value;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x000FB7B0 File Offset: 0x000FA7B0
		// (set) Token: 0x06003BE6 RID: 15334 RVA: 0x000FB7D9 File Offset: 0x000FA7D9
		[DefaultValue(true)]
		[WebSysDescription("Calendar_ShowNextPrevMonth")]
		[WebCategory("Appearance")]
		public bool ShowNextPrevMonth
		{
			get
			{
				object obj = this.ViewState["ShowNextPrevMonth"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowNextPrevMonth"] = value;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x000FB7F4 File Offset: 0x000FA7F4
		// (set) Token: 0x06003BE8 RID: 15336 RVA: 0x000FB81D File Offset: 0x000FA81D
		[WebSysDescription("Calendar_ShowTitle")]
		[DefaultValue(true)]
		[WebCategory("Appearance")]
		public bool ShowTitle
		{
			get
			{
				object obj = this.ViewState["ShowTitle"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowTitle"] = value;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x000FB838 File Offset: 0x000FA838
		// (set) Token: 0x06003BEA RID: 15338 RVA: 0x000FB861 File Offset: 0x000FA861
		[WebCategory("Appearance")]
		[WebSysDescription("Calendar_TitleFormat")]
		[DefaultValue(TitleFormat.MonthYear)]
		public TitleFormat TitleFormat
		{
			get
			{
				object obj = this.ViewState["TitleFormat"];
				if (obj != null)
				{
					return (TitleFormat)obj;
				}
				return TitleFormat.MonthYear;
			}
			set
			{
				if (value < TitleFormat.Month || value > TitleFormat.MonthYear)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["TitleFormat"] = value;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x000FB88C File Offset: 0x000FA88C
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("Calendar_TitleStyle")]
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		public TableItemStyle TitleStyle
		{
			get
			{
				if (this.titleStyle == null)
				{
					this.titleStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.titleStyle).TrackViewState();
					}
				}
				return this.titleStyle;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06003BEC RID: 15340 RVA: 0x000FB8BA File Offset: 0x000FA8BA
		[NotifyParentProperty(true)]
		[WebSysDescription("Calendar_TodayDayStyle")]
		[WebCategory("Styles")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle TodayDayStyle
		{
			get
			{
				if (this.todayDayStyle == null)
				{
					this.todayDayStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.todayDayStyle).TrackViewState();
					}
				}
				return this.todayDayStyle;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x000FB8E8 File Offset: 0x000FA8E8
		// (set) Token: 0x06003BEE RID: 15342 RVA: 0x000FB915 File Offset: 0x000FA915
		[Browsable(false)]
		[WebSysDescription("Calendar_TodaysDate")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime TodaysDate
		{
			get
			{
				object obj = this.ViewState["TodaysDate"];
				if (obj != null)
				{
					return (DateTime)obj;
				}
				return DateTime.Today;
			}
			set
			{
				this.ViewState["TodaysDate"] = value.Date;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06003BEF RID: 15343 RVA: 0x000FB934 File Offset: 0x000FA934
		// (set) Token: 0x06003BF0 RID: 15344 RVA: 0x000FB95D File Offset: 0x000FA95D
		[WebCategory("Accessibility")]
		[WebSysDescription("Table_UseAccessibleHeader")]
		[DefaultValue(true)]
		public virtual bool UseAccessibleHeader
		{
			get
			{
				object obj = this.ViewState["UseAccessibleHeader"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["UseAccessibleHeader"] = value;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x000FB978 File Offset: 0x000FA978
		// (set) Token: 0x06003BF2 RID: 15346 RVA: 0x000FB9A5 File Offset: 0x000FA9A5
		[WebSysDescription("Calendar_VisibleDate")]
		[DefaultValue(typeof(DateTime), "1/1/0001")]
		[Bindable(true)]
		public DateTime VisibleDate
		{
			get
			{
				object obj = this.ViewState["VisibleDate"];
				if (obj != null)
				{
					return (DateTime)obj;
				}
				return DateTime.MinValue;
			}
			set
			{
				this.ViewState["VisibleDate"] = value.Date;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x000FB9C3 File Offset: 0x000FA9C3
		[WebSysDescription("Calendar_WeekendDayStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public TableItemStyle WeekendDayStyle
		{
			get
			{
				if (this.weekendDayStyle == null)
				{
					this.weekendDayStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this.weekendDayStyle).TrackViewState();
					}
				}
				return this.weekendDayStyle;
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06003BF4 RID: 15348 RVA: 0x000FB9F1 File Offset: 0x000FA9F1
		// (remove) Token: 0x06003BF5 RID: 15349 RVA: 0x000FBA04 File Offset: 0x000FAA04
		[WebCategory("Action")]
		[WebSysDescription("Calendar_OnDayRender")]
		public event DayRenderEventHandler DayRender
		{
			add
			{
				base.Events.AddHandler(Calendar.EventDayRender, value);
			}
			remove
			{
				base.Events.RemoveHandler(Calendar.EventDayRender, value);
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06003BF6 RID: 15350 RVA: 0x000FBA17 File Offset: 0x000FAA17
		// (remove) Token: 0x06003BF7 RID: 15351 RVA: 0x000FBA2A File Offset: 0x000FAA2A
		[WebCategory("Action")]
		[WebSysDescription("Calendar_OnSelectionChanged")]
		public event EventHandler SelectionChanged
		{
			add
			{
				base.Events.AddHandler(Calendar.EventSelectionChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Calendar.EventSelectionChanged, value);
			}
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06003BF8 RID: 15352 RVA: 0x000FBA3D File Offset: 0x000FAA3D
		// (remove) Token: 0x06003BF9 RID: 15353 RVA: 0x000FBA50 File Offset: 0x000FAA50
		[WebCategory("Action")]
		[WebSysDescription("Calendar_OnVisibleMonthChanged")]
		public event MonthChangedEventHandler VisibleMonthChanged
		{
			add
			{
				base.Events.AddHandler(Calendar.EventVisibleMonthChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(Calendar.EventVisibleMonthChanged, value);
			}
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x000FBA64 File Offset: 0x000FAA64
		private void ApplyTitleStyle(TableCell titleCell, Table titleTable, TableItemStyle titleStyle)
		{
			if (titleStyle.BackColor != Color.Empty)
			{
				titleCell.BackColor = titleStyle.BackColor;
			}
			if (titleStyle.BorderColor != Color.Empty)
			{
				titleCell.BorderColor = titleStyle.BorderColor;
			}
			if (titleStyle.BorderWidth != Unit.Empty)
			{
				titleCell.BorderWidth = titleStyle.BorderWidth;
			}
			if (titleStyle.BorderStyle != BorderStyle.NotSet)
			{
				titleCell.BorderStyle = titleStyle.BorderStyle;
			}
			if (titleStyle.Height != Unit.Empty)
			{
				titleCell.Height = titleStyle.Height;
			}
			if (titleStyle.VerticalAlign != VerticalAlign.NotSet)
			{
				titleCell.VerticalAlign = titleStyle.VerticalAlign;
			}
			if (titleStyle.CssClass.Length > 0)
			{
				titleTable.CssClass = titleStyle.CssClass;
			}
			else if (this.CssClass.Length > 0)
			{
				titleTable.CssClass = this.CssClass;
			}
			if (titleStyle.ForeColor != Color.Empty)
			{
				titleTable.ForeColor = titleStyle.ForeColor;
			}
			else if (this.ForeColor != Color.Empty)
			{
				titleTable.ForeColor = this.ForeColor;
			}
			titleTable.Font.CopyFrom(titleStyle.Font);
			titleTable.Font.MergeWith(this.Font);
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x000FBBA7 File Offset: 0x000FABA7
		protected override ControlCollection CreateControlCollection()
		{
			return new InternalControlCollection(this);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x000FBBB0 File Offset: 0x000FABB0
		private DateTime EffectiveVisibleDate()
		{
			DateTime dateTime = this.VisibleDate;
			if (dateTime.Equals(DateTime.MinValue))
			{
				dateTime = this.TodaysDate;
			}
			if (this.IsMinSupportedYearMonth(dateTime))
			{
				return this.minSupportedDate;
			}
			return this.threadCalendar.AddDays(dateTime, -(this.threadCalendar.GetDayOfMonth(dateTime) - 1));
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x000FBC04 File Offset: 0x000FAC04
		private DateTime FirstCalendarDay(DateTime visibleDate)
		{
			if (this.IsMinSupportedYearMonth(visibleDate))
			{
				return visibleDate;
			}
			int num = this.threadCalendar.GetDayOfWeek(visibleDate) - (DayOfWeek)this.NumericFirstDayOfWeek();
			if (num <= 0)
			{
				num += 7;
			}
			return this.threadCalendar.AddDays(visibleDate, -num);
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x000FBC48 File Offset: 0x000FAC48
		private string GetCalendarButtonText(string eventArgument, string buttonText, string title, bool showLink, Color foreColor)
		{
			if (showLink)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<a href=\"");
				stringBuilder.Append(this.Page.ClientScript.GetPostBackClientHyperlink(this, eventArgument, true));
				stringBuilder.Append("\" style=\"color:");
				stringBuilder.Append(foreColor.IsEmpty ? this.defaultButtonColorText : ColorTranslator.ToHtml(foreColor));
				if (!string.IsNullOrEmpty(title))
				{
					stringBuilder.Append("\" title=\"");
					stringBuilder.Append(title);
				}
				stringBuilder.Append("\">");
				stringBuilder.Append(buttonText);
				stringBuilder.Append("</a>");
				return stringBuilder.ToString();
			}
			return buttonText;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x000FBCF8 File Offset: 0x000FACF8
		private int GetDefinedStyleMask()
		{
			int num = 8;
			if (this.dayStyle != null && !this.dayStyle.IsEmpty)
			{
				num |= 16;
			}
			if (this.todayDayStyle != null && !this.todayDayStyle.IsEmpty)
			{
				num |= 4;
			}
			if (this.otherMonthDayStyle != null && !this.otherMonthDayStyle.IsEmpty)
			{
				num |= 2;
			}
			if (this.weekendDayStyle != null && !this.weekendDayStyle.IsEmpty)
			{
				num |= 1;
			}
			return num;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x000FBD6D File Offset: 0x000FAD6D
		private string GetMonthName(int m, bool bFull)
		{
			if (bFull)
			{
				return DateTimeFormatInfo.CurrentInfo.GetMonthName(m);
			}
			return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(m);
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x000FBD89 File Offset: 0x000FAD89
		protected bool HasWeekSelectors(CalendarSelectionMode selectionMode)
		{
			return selectionMode == CalendarSelectionMode.DayWeek || selectionMode == CalendarSelectionMode.DayWeekMonth;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x000FBD98 File Offset: 0x000FAD98
		private bool IsTheSameYearMonth(DateTime date1, DateTime date2)
		{
			return this.threadCalendar.GetEra(date1) == this.threadCalendar.GetEra(date2) && this.threadCalendar.GetYear(date1) == this.threadCalendar.GetYear(date2) && this.threadCalendar.GetMonth(date1) == this.threadCalendar.GetMonth(date2);
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000FBDF5 File Offset: 0x000FADF5
		private bool IsMinSupportedYearMonth(DateTime date)
		{
			return this.IsTheSameYearMonth(this.minSupportedDate, date);
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x000FBE04 File Offset: 0x000FAE04
		private bool IsMaxSupportedYearMonth(DateTime date)
		{
			return this.IsTheSameYearMonth(this.maxSupportedDate, date);
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x000FBE14 File Offset: 0x000FAE14
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				if (array[0] != null)
				{
					base.LoadViewState(array[0]);
				}
				if (array[1] != null)
				{
					((IStateManager)this.TitleStyle).LoadViewState(array[1]);
				}
				if (array[2] != null)
				{
					((IStateManager)this.NextPrevStyle).LoadViewState(array[2]);
				}
				if (array[3] != null)
				{
					((IStateManager)this.DayStyle).LoadViewState(array[3]);
				}
				if (array[4] != null)
				{
					((IStateManager)this.DayHeaderStyle).LoadViewState(array[4]);
				}
				if (array[5] != null)
				{
					((IStateManager)this.TodayDayStyle).LoadViewState(array[5]);
				}
				if (array[6] != null)
				{
					((IStateManager)this.WeekendDayStyle).LoadViewState(array[6]);
				}
				if (array[7] != null)
				{
					((IStateManager)this.OtherMonthDayStyle).LoadViewState(array[7]);
				}
				if (array[8] != null)
				{
					((IStateManager)this.SelectedDayStyle).LoadViewState(array[8]);
				}
				if (array[9] != null)
				{
					((IStateManager)this.SelectorStyle).LoadViewState(array[9]);
				}
				ArrayList arrayList = (ArrayList)this.ViewState["SD"];
				if (arrayList != null)
				{
					this.dateList = arrayList;
					this.selectedDates = null;
				}
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000FBF10 File Offset: 0x000FAF10
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this.titleStyle != null)
			{
				((IStateManager)this.titleStyle).TrackViewState();
			}
			if (this.nextPrevStyle != null)
			{
				((IStateManager)this.nextPrevStyle).TrackViewState();
			}
			if (this.dayStyle != null)
			{
				((IStateManager)this.dayStyle).TrackViewState();
			}
			if (this.dayHeaderStyle != null)
			{
				((IStateManager)this.dayHeaderStyle).TrackViewState();
			}
			if (this.todayDayStyle != null)
			{
				((IStateManager)this.todayDayStyle).TrackViewState();
			}
			if (this.weekendDayStyle != null)
			{
				((IStateManager)this.weekendDayStyle).TrackViewState();
			}
			if (this.otherMonthDayStyle != null)
			{
				((IStateManager)this.otherMonthDayStyle).TrackViewState();
			}
			if (this.selectedDayStyle != null)
			{
				((IStateManager)this.selectedDayStyle).TrackViewState();
			}
			if (this.selectorStyle != null)
			{
				((IStateManager)this.selectorStyle).TrackViewState();
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x000FBFCE File Offset: 0x000FAFCE
		private int NumericFirstDayOfWeek()
		{
			if (this.FirstDayOfWeek != FirstDayOfWeek.Default)
			{
				return (int)this.FirstDayOfWeek;
			}
			return (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x000FBFEC File Offset: 0x000FAFEC
		protected virtual void OnDayRender(TableCell cell, CalendarDay day)
		{
			DayRenderEventHandler dayRenderEventHandler = (DayRenderEventHandler)base.Events[Calendar.EventDayRender];
			if (dayRenderEventHandler != null)
			{
				int days = day.Date.Subtract(Calendar.baseDate).Days;
				string text = null;
				Page page = this.Page;
				if (page != null)
				{
					string text2 = days.ToString(CultureInfo.InvariantCulture);
					text = this.Page.ClientScript.GetPostBackClientHyperlink(this, text2, true);
				}
				dayRenderEventHandler(this, new DayRenderEventArgs(cell, day, text));
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000FC070 File Offset: 0x000FB070
		protected virtual void OnSelectionChanged()
		{
			EventHandler eventHandler = (EventHandler)base.Events[Calendar.EventSelectionChanged];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x000FC0A4 File Offset: 0x000FB0A4
		protected virtual void OnVisibleMonthChanged(DateTime newDate, DateTime previousDate)
		{
			MonthChangedEventHandler monthChangedEventHandler = (MonthChangedEventHandler)base.Events[Calendar.EventVisibleMonthChanged];
			if (monthChangedEventHandler != null)
			{
				monthChangedEventHandler(this, new MonthChangedEventArgs(newDate, previousDate));
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x000FC0D8 File Offset: 0x000FB0D8
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (this._adapter != null)
			{
				IPostBackEventHandler postBackEventHandler = this._adapter as IPostBackEventHandler;
				if (postBackEventHandler != null)
				{
					postBackEventHandler.RaisePostBackEvent(eventArgument);
					return;
				}
			}
			else
			{
				if (string.Compare(eventArgument, 0, "V", 0, "V".Length, StringComparison.Ordinal) == 0)
				{
					DateTime dateTime = this.VisibleDate;
					if (dateTime.Equals(DateTime.MinValue))
					{
						dateTime = this.TodaysDate;
					}
					int num = int.Parse(eventArgument.Substring("V".Length), CultureInfo.InvariantCulture);
					this.VisibleDate = Calendar.baseDate.AddDays((double)num);
					if (this.VisibleDate == DateTime.MinValue)
					{
						this.VisibleDate = DateTimeFormatInfo.CurrentInfo.Calendar.AddDays(this.VisibleDate, 1);
					}
					this.OnVisibleMonthChanged(this.VisibleDate, dateTime);
					return;
				}
				if (string.Compare(eventArgument, 0, "R", 0, "R".Length, StringComparison.Ordinal) == 0)
				{
					int num2 = int.Parse(eventArgument.Substring("R".Length), CultureInfo.InvariantCulture);
					int num3 = num2 / 100;
					int num4 = num2 % 100;
					if (num4 < 1)
					{
						num4 = 100 + num4;
						num3--;
					}
					DateTime dateTime2 = Calendar.baseDate.AddDays((double)num3);
					this.SelectRange(dateTime2, dateTime2.AddDays((double)(num4 - 1)));
					return;
				}
				int num5 = int.Parse(eventArgument, CultureInfo.InvariantCulture);
				DateTime dateTime3 = Calendar.baseDate.AddDays((double)num5);
				this.SelectRange(dateTime3, dateTime3);
			}
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x000FC253 File Offset: 0x000FB253
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x000FC25C File Offset: 0x000FB25C
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null)
			{
				this.Page.RegisterPostBackScript();
			}
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x000FC278 File Offset: 0x000FB278
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.threadCalendar = DateTimeFormatInfo.CurrentInfo.Calendar;
			this.minSupportedDate = this.threadCalendar.MinSupportedDateTime;
			this.maxSupportedDate = this.threadCalendar.MaxSupportedDateTime;
			DateTime dateTime = this.EffectiveVisibleDate();
			DateTime dateTime2 = this.FirstCalendarDay(dateTime);
			CalendarSelectionMode selectionMode = this.SelectionMode;
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			Page page = this.Page;
			bool flag = page != null && !base.DesignMode && base.IsEnabled;
			this.defaultForeColor = this.ForeColor;
			if (this.defaultForeColor == Color.Empty)
			{
				this.defaultForeColor = Calendar.DefaultForeColor;
			}
			this.defaultButtonColorText = ColorTranslator.ToHtml(this.defaultForeColor);
			Table table = new Table();
			if (this.ID != null)
			{
				table.ID = this.ClientID;
			}
			table.CopyBaseAttributes(this);
			if (base.ControlStyleCreated)
			{
				table.ApplyStyle(base.ControlStyle);
			}
			table.Width = this.Width;
			table.Height = this.Height;
			table.CellPadding = this.CellPadding;
			table.CellSpacing = this.CellSpacing;
			if (!base.ControlStyleCreated || !base.ControlStyle.IsSet(32) || this.BorderWidth.Equals(Unit.Empty))
			{
				table.BorderWidth = Unit.Pixel(1);
			}
			if (this.ShowGridLines)
			{
				table.GridLines = GridLines.Both;
			}
			else
			{
				table.GridLines = GridLines.None;
			}
			bool useAccessibleHeader = this.UseAccessibleHeader;
			if (useAccessibleHeader && table.Attributes["title"] == null)
			{
				table.Attributes["title"] = SR.GetString("Calendar_TitleText");
			}
			string caption = this.Caption;
			if (caption.Length > 0)
			{
				table.Caption = caption;
				table.CaptionAlign = this.CaptionAlign;
			}
			table.RenderBeginTag(writer);
			if (this.ShowTitle)
			{
				this.RenderTitle(writer, dateTime, selectionMode, flag, useAccessibleHeader);
			}
			if (this.ShowDayHeader)
			{
				this.RenderDayHeader(writer, dateTime, selectionMode, flag, useAccessibleHeader);
			}
			this.RenderDays(writer, dateTime2, dateTime, selectionMode, flag, useAccessibleHeader);
			table.RenderEndTag(writer);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x000FC4B0 File Offset: 0x000FB4B0
		private void RenderCalendarCell(HtmlTextWriter writer, TableItemStyle style, string text, string title, bool hasButton, string eventArgument)
		{
			style.AddAttributesToRender(writer, this);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			if (hasButton)
			{
				Color foreColor = style.ForeColor;
				writer.Write("<a href=\"");
				writer.Write(this.Page.ClientScript.GetPostBackClientHyperlink(this, eventArgument, true));
				writer.Write("\" style=\"color:");
				writer.Write(foreColor.IsEmpty ? this.defaultButtonColorText : ColorTranslator.ToHtml(foreColor));
				if (!string.IsNullOrEmpty(title))
				{
					writer.Write("\" title=\"");
					writer.Write(title);
				}
				writer.Write("\">");
				writer.Write(text);
				writer.Write("</a>");
			}
			else
			{
				writer.Write(text);
			}
			writer.RenderEndTag();
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x000FC570 File Offset: 0x000FB570
		private void RenderCalendarHeaderCell(HtmlTextWriter writer, TableItemStyle style, string text, string abbrText)
		{
			style.AddAttributesToRender(writer, this);
			writer.AddAttribute("abbr", abbrText);
			writer.AddAttribute("scope", "col");
			writer.RenderBeginTag(HtmlTextWriterTag.Th);
			writer.Write(text);
			writer.RenderEndTag();
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x000FC5AC File Offset: 0x000FB5AC
		private void RenderDayHeader(HtmlTextWriter writer, DateTime visibleDate, CalendarSelectionMode selectionMode, bool buttonsActive, bool useAccessibleHeader)
		{
			writer.Write("<tr>");
			DateTimeFormatInfo currentInfo = DateTimeFormatInfo.CurrentInfo;
			if (this.HasWeekSelectors(selectionMode))
			{
				TableItemStyle tableItemStyle = new TableItemStyle();
				tableItemStyle.HorizontalAlign = HorizontalAlign.Center;
				if (selectionMode == CalendarSelectionMode.DayWeekMonth)
				{
					int days = visibleDate.Subtract(Calendar.baseDate).Days;
					int num = this.threadCalendar.GetDaysInMonth(this.threadCalendar.GetYear(visibleDate), this.threadCalendar.GetMonth(visibleDate), this.threadCalendar.GetEra(visibleDate));
					if (this.IsMinSupportedYearMonth(visibleDate))
					{
						num = num - this.threadCalendar.GetDayOfMonth(visibleDate) + 1;
					}
					else if (this.IsMaxSupportedYearMonth(visibleDate))
					{
						num = this.threadCalendar.GetDayOfMonth(this.maxSupportedDate);
					}
					string text = "R" + (days * 100 + num).ToString(CultureInfo.InvariantCulture);
					tableItemStyle.CopyFrom(this.SelectorStyle);
					string text2 = null;
					if (useAccessibleHeader)
					{
						text2 = SR.GetString("Calendar_SelectMonthTitle");
					}
					this.RenderCalendarCell(writer, tableItemStyle, this.SelectMonthText, text2, buttonsActive, text);
				}
				else
				{
					tableItemStyle.CopyFrom(this.DayHeaderStyle);
					this.RenderCalendarCell(writer, tableItemStyle, string.Empty, null, false, null);
				}
			}
			TableItemStyle tableItemStyle2 = new TableItemStyle();
			tableItemStyle2.HorizontalAlign = HorizontalAlign.Center;
			tableItemStyle2.CopyFrom(this.DayHeaderStyle);
			DayNameFormat dayNameFormat = this.DayNameFormat;
			int num2 = this.NumericFirstDayOfWeek();
			int i = num2;
			while (i < num2 + 7)
			{
				int num3 = i % 7;
				string text3;
				switch (dayNameFormat)
				{
				case DayNameFormat.Full:
					text3 = currentInfo.GetDayName((DayOfWeek)num3);
					break;
				case DayNameFormat.Short:
					goto IL_01AD;
				case DayNameFormat.FirstLetter:
					text3 = currentInfo.GetDayName((DayOfWeek)num3).Substring(0, 1);
					break;
				case DayNameFormat.FirstTwoLetters:
					text3 = currentInfo.GetDayName((DayOfWeek)num3).Substring(0, 2);
					break;
				case DayNameFormat.Shortest:
					text3 = currentInfo.GetShortestDayName((DayOfWeek)num3);
					break;
				default:
					goto IL_01AD;
				}
				IL_01C3:
				if (useAccessibleHeader)
				{
					string dayName = currentInfo.GetDayName((DayOfWeek)num3);
					this.RenderCalendarHeaderCell(writer, tableItemStyle2, text3, dayName);
				}
				else
				{
					this.RenderCalendarCell(writer, tableItemStyle2, text3, null, false, null);
				}
				i++;
				continue;
				IL_01AD:
				text3 = currentInfo.GetAbbreviatedDayName((DayOfWeek)num3);
				goto IL_01C3;
			}
			writer.Write("</tr>");
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x000FC7C4 File Offset: 0x000FB7C4
		private void RenderDays(HtmlTextWriter writer, DateTime firstDay, DateTime visibleDate, CalendarSelectionMode selectionMode, bool buttonsActive, bool useAccessibleHeader)
		{
			DateTime dateTime = firstDay;
			TableItemStyle tableItemStyle = null;
			bool flag = this.HasWeekSelectors(selectionMode);
			Unit unit;
			if (flag)
			{
				tableItemStyle = new TableItemStyle();
				tableItemStyle.Width = Unit.Percentage(12.0);
				tableItemStyle.HorizontalAlign = HorizontalAlign.Center;
				tableItemStyle.CopyFrom(this.SelectorStyle);
				unit = Unit.Percentage(12.0);
			}
			else
			{
				unit = Unit.Percentage(14.0);
			}
			bool flag2 = !(this.threadCalendar is HebrewCalendar);
			bool flag3 = base.GetType() != typeof(Calendar) || base.Events[Calendar.EventDayRender] != null;
			TableItemStyle[] array = new TableItemStyle[16];
			int definedStyleMask = this.GetDefinedStyleMask();
			DateTime todaysDate = this.TodaysDate;
			string selectWeekText = this.SelectWeekText;
			bool flag4 = buttonsActive && selectionMode != CalendarSelectionMode.None;
			int month = this.threadCalendar.GetMonth(visibleDate);
			int num = firstDay.Subtract(Calendar.baseDate).Days;
			bool flag5 = base.DesignMode && this.SelectionMode != CalendarSelectionMode.None;
			int i = 0;
			if (this.IsMinSupportedYearMonth(visibleDate))
			{
				i = this.threadCalendar.GetDayOfWeek(firstDay) - (DayOfWeek)this.NumericFirstDayOfWeek();
				if (i < 0)
				{
					i += 7;
				}
			}
			bool flag6 = false;
			DateTime dateTime2 = this.threadCalendar.AddMonths(this.maxSupportedDate, -1);
			bool flag7 = this.IsMaxSupportedYearMonth(visibleDate) || this.IsTheSameYearMonth(dateTime2, visibleDate);
			for (int j = 0; j < 6; j++)
			{
				if (flag6)
				{
					return;
				}
				writer.Write("<tr>");
				if (flag)
				{
					int num2 = num * 100 + 7;
					if (i > 0)
					{
						num2 -= i;
					}
					else if (flag7)
					{
						int days = this.maxSupportedDate.Subtract(dateTime).Days;
						if (days < 6)
						{
							num2 -= 6 - days;
						}
					}
					string text = "R" + num2.ToString(CultureInfo.InvariantCulture);
					string text2 = null;
					if (useAccessibleHeader)
					{
						text2 = SR.GetString("Calendar_SelectWeekTitle", new object[] { (j + 1).ToString(CultureInfo.InvariantCulture) });
					}
					this.RenderCalendarCell(writer, tableItemStyle, selectWeekText, text2, buttonsActive, text);
				}
				for (int k = 0; k < 7; k++)
				{
					if (i > 0)
					{
						k += i;
						while (i > 0)
						{
							writer.RenderBeginTag(HtmlTextWriterTag.Td);
							writer.RenderEndTag();
							i--;
						}
					}
					else if (flag6)
					{
						while (k < 7)
						{
							writer.RenderBeginTag(HtmlTextWriterTag.Td);
							writer.RenderEndTag();
							k++;
						}
						break;
					}
					int dayOfWeek = (int)this.threadCalendar.GetDayOfWeek(dateTime);
					int dayOfMonth = this.threadCalendar.GetDayOfMonth(dateTime);
					string text3;
					if (dayOfMonth <= 31 && flag2)
					{
						text3 = Calendar.cachedNumbers[dayOfMonth];
					}
					else
					{
						text3 = dateTime.ToString("dd", CultureInfo.CurrentCulture);
					}
					CalendarDay calendarDay = new CalendarDay(dateTime, dayOfWeek == 0 || dayOfWeek == 6, dateTime.Equals(todaysDate), this.selectedDates != null && this.selectedDates.Contains(dateTime), this.threadCalendar.GetMonth(dateTime) != month, text3);
					int num3 = 16;
					if (calendarDay.IsSelected)
					{
						num3 |= 8;
					}
					if (calendarDay.IsOtherMonth)
					{
						num3 |= 2;
					}
					if (calendarDay.IsToday)
					{
						num3 |= 4;
					}
					if (calendarDay.IsWeekend)
					{
						num3 |= 1;
					}
					int num4 = definedStyleMask & num3;
					int num5 = num4 & 15;
					TableItemStyle tableItemStyle2 = array[num5];
					if (tableItemStyle2 == null)
					{
						tableItemStyle2 = new TableItemStyle();
						this.SetDayStyles(tableItemStyle2, num4, unit);
						array[num5] = tableItemStyle2;
					}
					string text4 = null;
					if (useAccessibleHeader)
					{
						text4 = dateTime.ToString("m", CultureInfo.CurrentCulture);
					}
					if (flag3)
					{
						TableCell tableCell = new TableCell();
						tableCell.ApplyStyle(tableItemStyle2);
						LiteralControl literalControl = new LiteralControl(text3);
						tableCell.Controls.Add(literalControl);
						calendarDay.IsSelectable = flag4;
						this.OnDayRender(tableCell, calendarDay);
						literalControl.Text = this.GetCalendarButtonText(num.ToString(CultureInfo.InvariantCulture), text3, text4, buttonsActive && calendarDay.IsSelectable, tableCell.ForeColor);
						tableCell.RenderControl(writer);
					}
					else
					{
						if (flag5 && tableItemStyle2.ForeColor.IsEmpty)
						{
							tableItemStyle2.ForeColor = this.defaultForeColor;
						}
						this.RenderCalendarCell(writer, tableItemStyle2, text3, text4, flag4, num.ToString(CultureInfo.InvariantCulture));
					}
					if (flag7 && dateTime.Month == this.maxSupportedDate.Month && dateTime.Day == this.maxSupportedDate.Day)
					{
						flag6 = true;
					}
					else
					{
						dateTime = this.threadCalendar.AddDays(dateTime, 1);
						num++;
					}
				}
				writer.Write("</tr>");
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x000FCC84 File Offset: 0x000FBC84
		private void RenderTitle(HtmlTextWriter writer, DateTime visibleDate, CalendarSelectionMode selectionMode, bool buttonsActive, bool useAccessibleHeader)
		{
			writer.Write("<tr>");
			TableCell tableCell = new TableCell();
			Table table = new Table();
			tableCell.ColumnSpan = (this.HasWeekSelectors(selectionMode) ? 8 : 7);
			tableCell.BackColor = Color.Silver;
			table.GridLines = GridLines.None;
			table.Width = Unit.Percentage(100.0);
			table.CellSpacing = 0;
			TableItemStyle tableItemStyle = this.TitleStyle;
			this.ApplyTitleStyle(tableCell, table, tableItemStyle);
			tableCell.RenderBeginTag(writer);
			table.RenderBeginTag(writer);
			writer.Write("<tr>");
			NextPrevFormat nextPrevFormat = this.NextPrevFormat;
			TableItemStyle tableItemStyle2 = new TableItemStyle();
			tableItemStyle2.Width = Unit.Percentage(15.0);
			tableItemStyle2.CopyFrom(this.NextPrevStyle);
			if (this.ShowNextPrevMonth)
			{
				if (this.IsMinSupportedYearMonth(visibleDate))
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag();
				}
				else
				{
					string text;
					if (nextPrevFormat == NextPrevFormat.ShortMonth || nextPrevFormat == NextPrevFormat.FullMonth)
					{
						int month = this.threadCalendar.GetMonth(this.threadCalendar.AddMonths(visibleDate, -1));
						text = this.GetMonthName(month, nextPrevFormat == NextPrevFormat.FullMonth);
					}
					else
					{
						text = this.PrevMonthText;
					}
					DateTime dateTime = this.threadCalendar.AddMonths(this.minSupportedDate, 1);
					DateTime dateTime2;
					if (this.IsTheSameYearMonth(dateTime, visibleDate))
					{
						dateTime2 = this.minSupportedDate;
					}
					else
					{
						dateTime2 = this.threadCalendar.AddMonths(visibleDate, -1);
					}
					string text2 = "V" + dateTime2.Subtract(Calendar.baseDate).Days.ToString(CultureInfo.InvariantCulture);
					string text3 = null;
					if (useAccessibleHeader)
					{
						text3 = SR.GetString("Calendar_PreviousMonthTitle");
					}
					this.RenderCalendarCell(writer, tableItemStyle2, text, text3, buttonsActive, text2);
				}
			}
			TableItemStyle tableItemStyle3 = new TableItemStyle();
			if (tableItemStyle.HorizontalAlign != HorizontalAlign.NotSet)
			{
				tableItemStyle3.HorizontalAlign = tableItemStyle.HorizontalAlign;
			}
			else
			{
				tableItemStyle3.HorizontalAlign = HorizontalAlign.Center;
			}
			tableItemStyle3.Wrap = tableItemStyle.Wrap;
			tableItemStyle3.Width = Unit.Percentage(70.0);
			string text4;
			switch (this.TitleFormat)
			{
			case TitleFormat.Month:
				text4 = visibleDate.ToString("MMMM", CultureInfo.CurrentCulture);
				goto IL_0241;
			}
			string text5 = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
			if (text5.IndexOf(',') >= 0)
			{
				text5 = "MMMM yyyy";
			}
			text4 = visibleDate.ToString(text5, CultureInfo.CurrentCulture);
			IL_0241:
			this.RenderCalendarCell(writer, tableItemStyle3, text4, null, false, null);
			if (this.ShowNextPrevMonth)
			{
				if (this.IsMaxSupportedYearMonth(visibleDate))
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag();
				}
				else
				{
					tableItemStyle2.HorizontalAlign = HorizontalAlign.Right;
					string text6;
					if (nextPrevFormat == NextPrevFormat.ShortMonth || nextPrevFormat == NextPrevFormat.FullMonth)
					{
						int month2 = this.threadCalendar.GetMonth(this.threadCalendar.AddMonths(visibleDate, 1));
						text6 = this.GetMonthName(month2, nextPrevFormat == NextPrevFormat.FullMonth);
					}
					else
					{
						text6 = this.NextMonthText;
					}
					string text7 = "V" + this.threadCalendar.AddMonths(visibleDate, 1).Subtract(Calendar.baseDate).Days.ToString(CultureInfo.InvariantCulture);
					string text8 = null;
					if (useAccessibleHeader)
					{
						text8 = SR.GetString("Calendar_NextMonthTitle");
					}
					this.RenderCalendarCell(writer, tableItemStyle2, text6, text8, buttonsActive, text7);
				}
			}
			writer.Write("</tr>");
			table.RenderEndTag(writer);
			tableCell.RenderEndTag(writer);
			writer.Write("</tr>");
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x000FCFD0 File Offset: 0x000FBFD0
		protected override object SaveViewState()
		{
			if (this.SelectedDates.Count > 0)
			{
				this.ViewState["SD"] = this.dateList;
			}
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this.titleStyle != null) ? ((IStateManager)this.titleStyle).SaveViewState() : null,
				(this.nextPrevStyle != null) ? ((IStateManager)this.nextPrevStyle).SaveViewState() : null,
				(this.dayStyle != null) ? ((IStateManager)this.dayStyle).SaveViewState() : null,
				(this.dayHeaderStyle != null) ? ((IStateManager)this.dayHeaderStyle).SaveViewState() : null,
				(this.todayDayStyle != null) ? ((IStateManager)this.todayDayStyle).SaveViewState() : null,
				(this.weekendDayStyle != null) ? ((IStateManager)this.weekendDayStyle).SaveViewState() : null,
				(this.otherMonthDayStyle != null) ? ((IStateManager)this.otherMonthDayStyle).SaveViewState() : null,
				(this.selectedDayStyle != null) ? ((IStateManager)this.selectedDayStyle).SaveViewState() : null,
				(this.selectorStyle != null) ? ((IStateManager)this.selectorStyle).SaveViewState() : null
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x000FD10C File Offset: 0x000FC10C
		private void SelectRange(DateTime dateFrom, DateTime dateTo)
		{
			TimeSpan timeSpan = dateTo - dateFrom;
			if (this.SelectedDates.Count != timeSpan.Days + 1 || this.SelectedDates[0] != dateFrom || this.SelectedDates[this.SelectedDates.Count - 1] != dateTo)
			{
				this.SelectedDates.SelectRange(dateFrom, dateTo);
				this.OnSelectionChanged();
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x000FD180 File Offset: 0x000FC180
		private void SetDayStyles(TableItemStyle style, int styleMask, Unit defaultWidth)
		{
			style.Width = defaultWidth;
			style.HorizontalAlign = HorizontalAlign.Center;
			if ((styleMask & 16) != 0)
			{
				style.CopyFrom(this.DayStyle);
			}
			if ((styleMask & 1) != 0)
			{
				style.CopyFrom(this.WeekendDayStyle);
			}
			if ((styleMask & 2) != 0)
			{
				style.CopyFrom(this.OtherMonthDayStyle);
			}
			if ((styleMask & 4) != 0)
			{
				style.CopyFrom(this.TodayDayStyle);
			}
			if ((styleMask & 8) != 0)
			{
				style.ForeColor = Color.White;
				style.BackColor = Color.Silver;
				style.CopyFrom(this.SelectedDayStyle);
			}
		}

		// Token: 0x040026D4 RID: 9940
		private const string SELECT_RANGE_COMMAND = "R";

		// Token: 0x040026D5 RID: 9941
		private const string NAVIGATE_MONTH_COMMAND = "V";

		// Token: 0x040026D6 RID: 9942
		private const int STYLEMASK_DAY = 16;

		// Token: 0x040026D7 RID: 9943
		private const int STYLEMASK_UNIQUE = 15;

		// Token: 0x040026D8 RID: 9944
		private const int STYLEMASK_SELECTED = 8;

		// Token: 0x040026D9 RID: 9945
		private const int STYLEMASK_TODAY = 4;

		// Token: 0x040026DA RID: 9946
		private const int STYLEMASK_OTHERMONTH = 2;

		// Token: 0x040026DB RID: 9947
		private const int STYLEMASK_WEEKEND = 1;

		// Token: 0x040026DC RID: 9948
		private const string ROWBEGINTAG = "<tr>";

		// Token: 0x040026DD RID: 9949
		private const string ROWENDTAG = "</tr>";

		// Token: 0x040026DE RID: 9950
		private const int cachedNumberMax = 31;

		// Token: 0x040026DF RID: 9951
		private static readonly object EventDayRender = new object();

		// Token: 0x040026E0 RID: 9952
		private static readonly object EventSelectionChanged = new object();

		// Token: 0x040026E1 RID: 9953
		private static readonly object EventVisibleMonthChanged = new object();

		// Token: 0x040026E2 RID: 9954
		private TableItemStyle titleStyle;

		// Token: 0x040026E3 RID: 9955
		private TableItemStyle nextPrevStyle;

		// Token: 0x040026E4 RID: 9956
		private TableItemStyle dayHeaderStyle;

		// Token: 0x040026E5 RID: 9957
		private TableItemStyle selectorStyle;

		// Token: 0x040026E6 RID: 9958
		private TableItemStyle dayStyle;

		// Token: 0x040026E7 RID: 9959
		private TableItemStyle otherMonthDayStyle;

		// Token: 0x040026E8 RID: 9960
		private TableItemStyle todayDayStyle;

		// Token: 0x040026E9 RID: 9961
		private TableItemStyle selectedDayStyle;

		// Token: 0x040026EA RID: 9962
		private TableItemStyle weekendDayStyle;

		// Token: 0x040026EB RID: 9963
		private string defaultButtonColorText;

		// Token: 0x040026EC RID: 9964
		private static readonly Color DefaultForeColor = Color.Black;

		// Token: 0x040026ED RID: 9965
		private Color defaultForeColor;

		// Token: 0x040026EE RID: 9966
		private ArrayList dateList;

		// Token: 0x040026EF RID: 9967
		private SelectedDatesCollection selectedDates;

		// Token: 0x040026F0 RID: 9968
		private Calendar threadCalendar;

		// Token: 0x040026F1 RID: 9969
		private DateTime minSupportedDate;

		// Token: 0x040026F2 RID: 9970
		private DateTime maxSupportedDate;

		// Token: 0x040026F3 RID: 9971
		private static DateTime baseDate = new DateTime(2000, 1, 1);

		// Token: 0x040026F4 RID: 9972
		private static readonly string[] cachedNumbers = new string[]
		{
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31"
		};
	}
}
