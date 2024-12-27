using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020003AE RID: 942
	[DefaultEvent("ValueChanged")]
	[ComVisible(true)]
	[DefaultProperty("Value")]
	[DefaultBindingProperty("Value")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.DateTimePickerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionDateTimePicker")]
	public class DateTimePicker : Control
	{
		// Token: 0x06003933 RID: 14643 RVA: 0x000D17AC File Offset: 0x000D07AC
		public DateTimePicker()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.FixedHeight, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick, false);
			this.format = DateTimePickerFormat.Long;
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003934 RID: 14644 RVA: 0x000D1858 File Offset: 0x000D0858
		// (set) Token: 0x06003935 RID: 14645 RVA: 0x000D186E File Offset: 0x000D086E
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x140001D5 RID: 469
		// (add) Token: 0x06003936 RID: 14646 RVA: 0x000D1877 File Offset: 0x000D0877
		// (remove) Token: 0x06003937 RID: 14647 RVA: 0x000D1880 File Offset: 0x000D0880
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003938 RID: 14648 RVA: 0x000D1889 File Offset: 0x000D0889
		// (set) Token: 0x06003939 RID: 14649 RVA: 0x000D1891 File Offset: 0x000D0891
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140001D6 RID: 470
		// (add) Token: 0x0600393A RID: 14650 RVA: 0x000D189A File Offset: 0x000D089A
		// (remove) Token: 0x0600393B RID: 14651 RVA: 0x000D18A3 File Offset: 0x000D08A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x0600393C RID: 14652 RVA: 0x000D18AC File Offset: 0x000D08AC
		// (set) Token: 0x0600393D RID: 14653 RVA: 0x000D18B4 File Offset: 0x000D08B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140001D7 RID: 471
		// (add) Token: 0x0600393E RID: 14654 RVA: 0x000D18BD File Offset: 0x000D08BD
		// (remove) Token: 0x0600393F RID: 14655 RVA: 0x000D18C6 File Offset: 0x000D08C6
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

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003940 RID: 14656 RVA: 0x000D18CF File Offset: 0x000D08CF
		// (set) Token: 0x06003941 RID: 14657 RVA: 0x000D18D8 File Offset: 0x000D08D8
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarForeColorDescr")]
		public Color CalendarForeColor
		{
			get
			{
				return this.calendarForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "value" }));
				}
				if (!value.Equals(this.calendarForeColor))
				{
					this.calendarForeColor = value;
					this.SetControlColor(1, value);
				}
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003942 RID: 14658 RVA: 0x000D1937 File Offset: 0x000D0937
		// (set) Token: 0x06003943 RID: 14659 RVA: 0x000D194E File Offset: 0x000D094E
		[SRCategory("CatAppearance")]
		[AmbientValue(null)]
		[Localizable(true)]
		[SRDescription("DateTimePickerCalendarFontDescr")]
		public Font CalendarFont
		{
			get
			{
				if (this.calendarFont == null)
				{
					return this.Font;
				}
				return this.calendarFont;
			}
			set
			{
				if ((value == null && this.calendarFont != null) || (value != null && !value.Equals(this.calendarFont)))
				{
					this.calendarFont = value;
					this.calendarFontHandleWrapper = null;
					this.SetControlCalendarFont();
				}
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003944 RID: 14660 RVA: 0x000D1980 File Offset: 0x000D0980
		private IntPtr CalendarFontHandle
		{
			get
			{
				if (this.calendarFont == null)
				{
					return base.FontHandle;
				}
				if (this.calendarFontHandleWrapper == null)
				{
					this.calendarFontHandleWrapper = new Control.FontHandleWrapper(this.CalendarFont);
				}
				return this.calendarFontHandleWrapper.Handle;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x000D19B5 File Offset: 0x000D09B5
		// (set) Token: 0x06003946 RID: 14662 RVA: 0x000D19C0 File Offset: 0x000D09C0
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerCalendarTitleBackColorDescr")]
		public Color CalendarTitleBackColor
		{
			get
			{
				return this.calendarTitleBackColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "value" }));
				}
				if (!value.Equals(this.calendarTitleBackColor))
				{
					this.calendarTitleBackColor = value;
					this.SetControlColor(2, value);
				}
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x000D1A1F File Offset: 0x000D0A1F
		// (set) Token: 0x06003948 RID: 14664 RVA: 0x000D1A28 File Offset: 0x000D0A28
		[SRDescription("DateTimePickerCalendarTitleForeColorDescr")]
		[SRCategory("CatAppearance")]
		public Color CalendarTitleForeColor
		{
			get
			{
				return this.calendarTitleForeColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "value" }));
				}
				if (!value.Equals(this.calendarTitleForeColor))
				{
					this.calendarTitleForeColor = value;
					this.SetControlColor(3, value);
				}
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x000D1A87 File Offset: 0x000D0A87
		// (set) Token: 0x0600394A RID: 14666 RVA: 0x000D1A90 File Offset: 0x000D0A90
		[SRDescription("DateTimePickerCalendarTrailingForeColorDescr")]
		[SRCategory("CatAppearance")]
		public Color CalendarTrailingForeColor
		{
			get
			{
				return this.calendarTrailingText;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "value" }));
				}
				if (!value.Equals(this.calendarTrailingText))
				{
					this.calendarTrailingText = value;
					this.SetControlColor(5, value);
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x000D1AEF File Offset: 0x000D0AEF
		// (set) Token: 0x0600394C RID: 14668 RVA: 0x000D1AF8 File Offset: 0x000D0AF8
		[SRDescription("DateTimePickerCalendarMonthBackgroundDescr")]
		[SRCategory("CatAppearance")]
		public Color CalendarMonthBackground
		{
			get
			{
				return this.calendarMonthBackground;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidNullArgument", new object[] { "value" }));
				}
				if (!value.Equals(this.calendarMonthBackground))
				{
					this.calendarMonthBackground = value;
					this.SetControlColor(4, value);
				}
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x000D1B58 File Offset: 0x000D0B58
		// (set) Token: 0x0600394E RID: 14670 RVA: 0x000D1BA4 File Offset: 0x000D0BA4
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerCheckedDescr")]
		[DefaultValue(true)]
		[Bindable(true)]
		public bool Checked
		{
			get
			{
				if (this.ShowCheckBox && base.IsHandleCreated)
				{
					NativeMethods.SYSTEMTIME systemtime = new NativeMethods.SYSTEMTIME();
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4097, 0, systemtime);
					return num == 0;
				}
				return this.validTime;
			}
			set
			{
				if (this.Checked != value)
				{
					if (this.ShowCheckBox && base.IsHandleCreated)
					{
						if (value)
						{
							int num = 0;
							NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(this.Value);
							UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num, systemtime);
						}
						else
						{
							int num2 = 1;
							NativeMethods.SYSTEMTIME systemtime2 = null;
							UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num2, systemtime2);
						}
					}
					this.validTime = value;
				}
			}
		}

		// Token: 0x140001D8 RID: 472
		// (add) Token: 0x0600394F RID: 14671 RVA: 0x000D1C1A File Offset: 0x000D0C1A
		// (remove) Token: 0x06003950 RID: 14672 RVA: 0x000D1C23 File Offset: 0x000D0C23
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x000D1C2C File Offset: 0x000D0C2C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysDateTimePick32";
				createParams.Style |= this.style;
				DateTimePickerFormat dateTimePickerFormat = this.format;
				switch (dateTimePickerFormat)
				{
				case DateTimePickerFormat.Long:
					createParams.Style |= 4;
					break;
				case DateTimePickerFormat.Short:
				case (DateTimePickerFormat)3:
					break;
				case DateTimePickerFormat.Time:
					createParams.Style |= 8;
					break;
				default:
					if (dateTimePickerFormat != DateTimePickerFormat.Custom)
					{
					}
					break;
				}
				createParams.ExStyle |= 512;
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x000D1CE9 File Offset: 0x000D0CE9
		// (set) Token: 0x06003953 RID: 14675 RVA: 0x000D1CF4 File Offset: 0x000D0CF4
		[DefaultValue(null)]
		[SRDescription("DateTimePickerCustomFormatDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public string CustomFormat
		{
			get
			{
				return this.customFormat;
			}
			set
			{
				if ((value != null && !value.Equals(this.customFormat)) || (value == null && this.customFormat != null))
				{
					this.customFormat = value;
					if (base.IsHandleCreated && this.format == DateTimePickerFormat.Custom)
					{
						base.SendMessage(NativeMethods.DTM_SETFORMAT, 0, this.customFormat);
					}
				}
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x000D1D48 File Offset: 0x000D0D48
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, this.PreferredHeight);
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x000D1D5A File Offset: 0x000D0D5A
		// (set) Token: 0x06003956 RID: 14678 RVA: 0x000D1D62 File Offset: 0x000D0D62
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		// Token: 0x140001D9 RID: 473
		// (add) Token: 0x06003957 RID: 14679 RVA: 0x000D1D6B File Offset: 0x000D0D6B
		// (remove) Token: 0x06003958 RID: 14680 RVA: 0x000D1D74 File Offset: 0x000D0D74
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x000D1D7D File Offset: 0x000D0D7D
		// (set) Token: 0x0600395A RID: 14682 RVA: 0x000D1D8D File Offset: 0x000D0D8D
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("DateTimePickerDropDownAlignDescr")]
		[DefaultValue(LeftRightAlignment.Left)]
		public LeftRightAlignment DropDownAlign
		{
			get
			{
				if ((this.style & 32) == 0)
				{
					return LeftRightAlignment.Left;
				}
				return LeftRightAlignment.Right;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LeftRightAlignment));
				}
				this.SetStyleBit(value == LeftRightAlignment.Right, 32);
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x000D1DC1 File Offset: 0x000D0DC1
		// (set) Token: 0x0600395C RID: 14684 RVA: 0x000D1DD7 File Offset: 0x000D0DD7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x140001DA RID: 474
		// (add) Token: 0x0600395D RID: 14685 RVA: 0x000D1DE0 File Offset: 0x000D0DE0
		// (remove) Token: 0x0600395E RID: 14686 RVA: 0x000D1DE9 File Offset: 0x000D0DE9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x000D1DF2 File Offset: 0x000D0DF2
		// (set) Token: 0x06003960 RID: 14688 RVA: 0x000D1DFC File Offset: 0x000D0DFC
		[SRDescription("DateTimePickerFormatDescr")]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public DateTimePickerFormat Format
		{
			get
			{
				return this.format;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 8, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DateTimePickerFormat));
				}
				if (this.format != value)
				{
					this.format = value;
					base.RecreateHandle();
					this.OnFormatChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001DB RID: 475
		// (add) Token: 0x06003961 RID: 14689 RVA: 0x000D1E51 File Offset: 0x000D0E51
		// (remove) Token: 0x06003962 RID: 14690 RVA: 0x000D1E64 File Offset: 0x000D0E64
		[SRDescription("DateTimePickerOnFormatChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler FormatChanged
		{
			add
			{
				base.Events.AddHandler(DateTimePicker.EVENT_FORMATCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(DateTimePicker.EVENT_FORMATCHANGED, value);
			}
		}

		// Token: 0x140001DC RID: 476
		// (add) Token: 0x06003963 RID: 14691 RVA: 0x000D1E77 File Offset: 0x000D0E77
		// (remove) Token: 0x06003964 RID: 14692 RVA: 0x000D1E80 File Offset: 0x000D0E80
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x000D1E8C File Offset: 0x000D0E8C
		internal static DateTime EffectiveMinDate(DateTime minDate)
		{
			DateTime minimumDateTime = DateTimePicker.MinimumDateTime;
			if (minDate < minimumDateTime)
			{
				return minimumDateTime;
			}
			return minDate;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x000D1EAC File Offset: 0x000D0EAC
		internal static DateTime EffectiveMaxDate(DateTime maxDate)
		{
			DateTime maximumDateTime = DateTimePicker.MaximumDateTime;
			if (maxDate > maximumDateTime)
			{
				return maximumDateTime;
			}
			return maxDate;
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x000D1ECB File Offset: 0x000D0ECB
		// (set) Token: 0x06003968 RID: 14696 RVA: 0x000D1ED8 File Offset: 0x000D0ED8
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerMaxDateDescr")]
		public DateTime MaxDate
		{
			get
			{
				return DateTimePicker.EffectiveMaxDate(this.max);
			}
			set
			{
				if (value != this.max)
				{
					if (value < DateTimePicker.EffectiveMinDate(this.min))
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"MaxDate",
							DateTimePicker.FormatDateTime(value),
							"MinDate"
						}));
					}
					if (value > DateTimePicker.MaximumDateTime)
					{
						throw new ArgumentOutOfRangeException("MaxDate", SR.GetString("DateTimePickerMaxDate", new object[] { DateTimePicker.FormatDateTime(DateTimePicker.MaxDateTime) }));
					}
					this.max = value;
					this.SetRange();
					if (this.Value > this.max)
					{
						this.Value = this.max;
					}
				}
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x000D1FA4 File Offset: 0x000D0FA4
		public static DateTime MaximumDateTime
		{
			get
			{
				DateTime maxSupportedDateTime = CultureInfo.CurrentCulture.Calendar.MaxSupportedDateTime;
				if (maxSupportedDateTime.Year > DateTimePicker.MaxDateTime.Year)
				{
					return DateTimePicker.MaxDateTime;
				}
				return maxSupportedDateTime;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x000D1FDE File Offset: 0x000D0FDE
		// (set) Token: 0x0600396B RID: 14699 RVA: 0x000D1FEC File Offset: 0x000D0FEC
		[SRCategory("CatBehavior")]
		[SRDescription("DateTimePickerMinDateDescr")]
		public DateTime MinDate
		{
			get
			{
				return DateTimePicker.EffectiveMinDate(this.min);
			}
			set
			{
				if (value != this.min)
				{
					if (value > DateTimePicker.EffectiveMaxDate(this.max))
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("InvalidHighBoundArgument", new object[]
						{
							"MinDate",
							DateTimePicker.FormatDateTime(value),
							"MaxDate"
						}));
					}
					if (value < DateTimePicker.MinimumDateTime)
					{
						throw new ArgumentOutOfRangeException("MinDate", SR.GetString("DateTimePickerMinDate", new object[] { DateTimePicker.FormatDateTime(DateTimePicker.MinimumDateTime) }));
					}
					this.min = value;
					this.SetRange();
					if (this.Value < this.min)
					{
						this.Value = this.min;
					}
				}
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x000D20B8 File Offset: 0x000D10B8
		public static DateTime MinimumDateTime
		{
			get
			{
				DateTime minSupportedDateTime = CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
				if (minSupportedDateTime.Year < 1753)
				{
					return new DateTime(1753, 1, 1);
				}
				return minSupportedDateTime;
			}
		}

		// Token: 0x140001DD RID: 477
		// (add) Token: 0x0600396D RID: 14701 RVA: 0x000D20F1 File Offset: 0x000D10F1
		// (remove) Token: 0x0600396E RID: 14702 RVA: 0x000D20FA File Offset: 0x000D10FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		// Token: 0x140001DE RID: 478
		// (add) Token: 0x0600396F RID: 14703 RVA: 0x000D2103 File Offset: 0x000D1103
		// (remove) Token: 0x06003970 RID: 14704 RVA: 0x000D210C File Offset: 0x000D110C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x000D2115 File Offset: 0x000D1115
		// (set) Token: 0x06003972 RID: 14706 RVA: 0x000D211D File Offset: 0x000D111D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x140001DF RID: 479
		// (add) Token: 0x06003973 RID: 14707 RVA: 0x000D2126 File Offset: 0x000D1126
		// (remove) Token: 0x06003974 RID: 14708 RVA: 0x000D212F File Offset: 0x000D112F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x000D2138 File Offset: 0x000D1138
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				if (this.prefHeightCache > -1)
				{
					return (int)this.prefHeightCache;
				}
				int num = base.FontHeight;
				num += SystemInformation.BorderSize.Height * 4 + 3;
				this.prefHeightCache = (short)num;
				return num;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003976 RID: 14710 RVA: 0x000D2179 File Offset: 0x000D1179
		// (set) Token: 0x06003977 RID: 14711 RVA: 0x000D2184 File Offset: 0x000D1184
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003978 RID: 14712 RVA: 0x000D21D8 File Offset: 0x000D11D8
		// (set) Token: 0x06003979 RID: 14713 RVA: 0x000D21E8 File Offset: 0x000D11E8
		[SRCategory("CatAppearance")]
		[SRDescription("DateTimePickerShowNoneDescr")]
		[DefaultValue(false)]
		public bool ShowCheckBox
		{
			get
			{
				return (this.style & 2) != 0;
			}
			set
			{
				this.SetStyleBit(value, 2);
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600397A RID: 14714 RVA: 0x000D21F2 File Offset: 0x000D11F2
		// (set) Token: 0x0600397B RID: 14715 RVA: 0x000D2202 File Offset: 0x000D1202
		[SRDescription("DateTimePickerShowUpDownDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool ShowUpDown
		{
			get
			{
				return (this.style & 1) != 0;
			}
			set
			{
				if (this.ShowUpDown != value)
				{
					this.SetStyleBit(value, 1);
				}
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x000D2215 File Offset: 0x000D1215
		// (set) Token: 0x0600397D RID: 14717 RVA: 0x000D221D File Offset: 0x000D121D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.ResetValue();
					return;
				}
				this.Value = DateTime.Parse(value, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x140001E0 RID: 480
		// (add) Token: 0x0600397E RID: 14718 RVA: 0x000D2242 File Offset: 0x000D1242
		// (remove) Token: 0x0600397F RID: 14719 RVA: 0x000D224B File Offset: 0x000D124B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003980 RID: 14720 RVA: 0x000D2254 File Offset: 0x000D1254
		// (set) Token: 0x06003981 RID: 14721 RVA: 0x000D2274 File Offset: 0x000D1274
		[SRDescription("DateTimePickerValueDescr")]
		[Bindable(true)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.All)]
		public DateTime Value
		{
			get
			{
				if (!this.userHasSetValue && this.validTime)
				{
					return this.creationTime;
				}
				return this.value;
			}
			set
			{
				bool flag = !DateTime.Equals(this.Value, value);
				if (!this.userHasSetValue || flag)
				{
					if (value < this.MinDate || value > this.MaxDate)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							DateTimePicker.FormatDateTime(value),
							"'MinDate'",
							"'MaxDate'"
						}));
					}
					string text = this.Text;
					this.value = value;
					this.userHasSetValue = true;
					if (base.IsHandleCreated)
					{
						int num = 0;
						NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(value);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num, systemtime);
					}
					if (flag)
					{
						this.OnValueChanged(EventArgs.Empty);
					}
					if (!text.Equals(this.Text))
					{
						this.OnTextChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x140001E1 RID: 481
		// (add) Token: 0x06003982 RID: 14722 RVA: 0x000D2365 File Offset: 0x000D1365
		// (remove) Token: 0x06003983 RID: 14723 RVA: 0x000D237E File Offset: 0x000D137E
		[SRDescription("DateTimePickerOnCloseUpDescr")]
		[SRCategory("CatAction")]
		public event EventHandler CloseUp
		{
			add
			{
				this.onCloseUp = (EventHandler)Delegate.Combine(this.onCloseUp, value);
			}
			remove
			{
				this.onCloseUp = (EventHandler)Delegate.Remove(this.onCloseUp, value);
			}
		}

		// Token: 0x140001E2 RID: 482
		// (add) Token: 0x06003984 RID: 14724 RVA: 0x000D2397 File Offset: 0x000D1397
		// (remove) Token: 0x06003985 RID: 14725 RVA: 0x000D23B0 File Offset: 0x000D13B0
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		// Token: 0x140001E3 RID: 483
		// (add) Token: 0x06003986 RID: 14726 RVA: 0x000D23C9 File Offset: 0x000D13C9
		// (remove) Token: 0x06003987 RID: 14727 RVA: 0x000D23E2 File Offset: 0x000D13E2
		[SRCategory("CatAction")]
		[SRDescription("valueChangedEventDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.onValueChanged = (EventHandler)Delegate.Combine(this.onValueChanged, value);
			}
			remove
			{
				this.onValueChanged = (EventHandler)Delegate.Remove(this.onValueChanged, value);
			}
		}

		// Token: 0x140001E4 RID: 484
		// (add) Token: 0x06003988 RID: 14728 RVA: 0x000D23FB File Offset: 0x000D13FB
		// (remove) Token: 0x06003989 RID: 14729 RVA: 0x000D2414 File Offset: 0x000D1414
		[SRDescription("DateTimePickerOnDropDownDescr")]
		[SRCategory("CatAction")]
		public event EventHandler DropDown
		{
			add
			{
				this.onDropDown = (EventHandler)Delegate.Combine(this.onDropDown, value);
			}
			remove
			{
				this.onDropDown = (EventHandler)Delegate.Remove(this.onDropDown, value);
			}
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x000D242D File Offset: 0x000D142D
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DateTimePicker.DateTimePickerAccessibleObject(this);
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x000D2438 File Offset: 0x000D1438
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr intPtr = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 256
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(intPtr);
				}
			}
			this.creationTime = DateTime.Now;
			base.CreateHandle();
			if (this.userHasSetValue && this.validTime)
			{
				int num = 0;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(this.Value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num, systemtime);
			}
			else if (!this.validTime)
			{
				int num2 = 1;
				NativeMethods.SYSTEMTIME systemtime2 = null;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num2, systemtime2);
			}
			if (this.format == DateTimePickerFormat.Custom)
			{
				base.SendMessage(NativeMethods.DTM_SETFORMAT, 0, this.customFormat);
			}
			this.UpdateUpDown();
			this.SetAllControlColors();
			this.SetControlCalendarFont();
			this.SetRange();
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x000D252C File Offset: 0x000D152C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override void DestroyHandle()
		{
			this.value = this.Value;
			base.DestroyHandle();
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000D2540 File Offset: 0x000D1540
		private static string FormatDateTime(DateTime value)
		{
			return value.ToString("G", CultureInfo.CurrentCulture);
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x000D2553 File Offset: 0x000D1553
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, this.PreferredHeight);
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x000D2564 File Offset: 0x000D1564
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = this.PreferredHeight;
			int width = CommonProperties.GetSpecifiedBounds(this).Width;
			return new Size(width, preferredHeight);
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x000D2590 File Offset: 0x000D1590
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x000D25D9 File Offset: 0x000D15D9
		protected virtual void OnCloseUp(EventArgs eventargs)
		{
			if (this.onCloseUp != null)
			{
				this.onCloseUp(this, eventargs);
			}
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x000D25F0 File Offset: 0x000D15F0
		protected virtual void OnDropDown(EventArgs eventargs)
		{
			if (this.onDropDown != null)
			{
				this.onDropDown(this, eventargs);
			}
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x000D2608 File Offset: 0x000D1608
		protected virtual void OnFormatChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DateTimePicker.EVENT_FORMATCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x000D2636 File Offset: 0x000D1636
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.MarshaledUserPreferenceChanged;
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x000D2650 File Offset: 0x000D1650
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.MarshaledUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x000D266A File Offset: 0x000D166A
		protected virtual void OnValueChanged(EventArgs eventargs)
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged(this, eventargs);
			}
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x000D2681 File Offset: 0x000D1681
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x000D26B0 File Offset: 0x000D16B0
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.prefHeightCache = -1;
			base.Height = this.PreferredHeight;
			if (this.calendarFont == null)
			{
				this.calendarFontHandleWrapper = null;
				this.SetControlCalendarFont();
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x000D26E1 File Offset: 0x000D16E1
		private void ResetCalendarForeColor()
		{
			this.CalendarForeColor = Control.DefaultForeColor;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x000D26EE File Offset: 0x000D16EE
		private void ResetCalendarFont()
		{
			this.CalendarFont = null;
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x000D26F7 File Offset: 0x000D16F7
		private void ResetCalendarMonthBackground()
		{
			this.CalendarMonthBackground = DateTimePicker.DefaultMonthBackColor;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x000D2704 File Offset: 0x000D1704
		private void ResetCalendarTitleBackColor()
		{
			this.CalendarTitleBackColor = DateTimePicker.DefaultTitleBackColor;
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x000D2711 File Offset: 0x000D1711
		private void ResetCalendarTitleForeColor()
		{
			this.CalendarTitleBackColor = Control.DefaultForeColor;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000D271E File Offset: 0x000D171E
		private void ResetCalendarTrailingForeColor()
		{
			this.CalendarTrailingForeColor = DateTimePicker.DefaultTrailingForeColor;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000D272B File Offset: 0x000D172B
		private void ResetFormat()
		{
			this.Format = DateTimePickerFormat.Long;
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000D2734 File Offset: 0x000D1734
		private void ResetMaxDate()
		{
			this.MaxDate = DateTime.MaxValue;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000D2741 File Offset: 0x000D1741
		private void ResetMinDate()
		{
			this.MinDate = DateTime.MinValue;
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x000D2750 File Offset: 0x000D1750
		private void ResetValue()
		{
			this.value = DateTime.Now;
			this.userHasSetValue = false;
			if (base.IsHandleCreated)
			{
				int num = 0;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(this.value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4098, num, systemtime);
			}
			this.Checked = false;
			this.OnValueChanged(EventArgs.Empty);
			this.OnTextChanged(EventArgs.Empty);
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000D27BB File Offset: 0x000D17BB
		private void SetControlColor(int colorIndex, Color value)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4102, colorIndex, ColorTranslator.ToWin32(value));
			}
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x000D27D8 File Offset: 0x000D17D8
		private void SetControlCalendarFont()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4105, this.CalendarFontHandle, NativeMethods.InvalidIntPtr);
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x000D27FC File Offset: 0x000D17FC
		private void SetAllControlColors()
		{
			this.SetControlColor(4, this.calendarMonthBackground);
			this.SetControlColor(1, this.calendarForeColor);
			this.SetControlColor(2, this.calendarTitleBackColor);
			this.SetControlColor(3, this.calendarTitleForeColor);
			this.SetControlColor(5, this.calendarTrailingText);
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x000D284A File Offset: 0x000D184A
		private void SetRange()
		{
			this.SetRange(DateTimePicker.EffectiveMinDate(this.min), DateTimePicker.EffectiveMaxDate(this.max));
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000D2868 File Offset: 0x000D1868
		private void SetRange(DateTime min, DateTime max)
		{
			if (base.IsHandleCreated)
			{
				int num = 0;
				NativeMethods.SYSTEMTIMEARRAY systemtimearray = new NativeMethods.SYSTEMTIMEARRAY();
				num |= 3;
				NativeMethods.SYSTEMTIME systemtime = DateTimePicker.DateTimeToSysTime(min);
				systemtimearray.wYear1 = systemtime.wYear;
				systemtimearray.wMonth1 = systemtime.wMonth;
				systemtimearray.wDayOfWeek1 = systemtime.wDayOfWeek;
				systemtimearray.wDay1 = systemtime.wDay;
				systemtimearray.wHour1 = systemtime.wHour;
				systemtimearray.wMinute1 = systemtime.wMinute;
				systemtimearray.wSecond1 = systemtime.wSecond;
				systemtimearray.wMilliseconds1 = systemtime.wMilliseconds;
				systemtime = DateTimePicker.DateTimeToSysTime(max);
				systemtimearray.wYear2 = systemtime.wYear;
				systemtimearray.wMonth2 = systemtime.wMonth;
				systemtimearray.wDayOfWeek2 = systemtime.wDayOfWeek;
				systemtimearray.wDay2 = systemtime.wDay;
				systemtimearray.wHour2 = systemtime.wHour;
				systemtimearray.wMinute2 = systemtime.wMinute;
				systemtimearray.wSecond2 = systemtime.wSecond;
				systemtimearray.wMilliseconds2 = systemtime.wMilliseconds;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4100, num, systemtimearray);
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x000D2974 File Offset: 0x000D1974
		private void SetStyleBit(bool flag, int bit)
		{
			if ((this.style & bit) != 0 == flag)
			{
				return;
			}
			if (flag)
			{
				this.style |= bit;
			}
			else
			{
				this.style &= ~bit;
			}
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
				base.Invalidate();
				base.Update();
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x000D29D0 File Offset: 0x000D19D0
		private bool ShouldSerializeCalendarForeColor()
		{
			return !this.CalendarForeColor.Equals(Control.DefaultForeColor);
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x000D29FE File Offset: 0x000D19FE
		private bool ShouldSerializeCalendarFont()
		{
			return this.calendarFont != null;
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x000D2A0C File Offset: 0x000D1A0C
		private bool ShouldSerializeCalendarTitleBackColor()
		{
			return !this.calendarTitleBackColor.Equals(DateTimePicker.DefaultTitleBackColor);
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000D2A2C File Offset: 0x000D1A2C
		private bool ShouldSerializeCalendarTitleForeColor()
		{
			return !this.calendarTitleForeColor.Equals(DateTimePicker.DefaultTitleForeColor);
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x000D2A4C File Offset: 0x000D1A4C
		private bool ShouldSerializeCalendarTrailingForeColor()
		{
			return !this.calendarTrailingText.Equals(DateTimePicker.DefaultTrailingForeColor);
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x000D2A6C File Offset: 0x000D1A6C
		private bool ShouldSerializeCalendarMonthBackground()
		{
			return !this.calendarMonthBackground.Equals(DateTimePicker.DefaultMonthBackColor);
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x000D2A8C File Offset: 0x000D1A8C
		private bool ShouldSerializeMaxDate()
		{
			return this.max != DateTimePicker.MaximumDateTime && this.max != DateTime.MaxValue;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x000D2AB2 File Offset: 0x000D1AB2
		private bool ShouldSerializeMinDate()
		{
			return this.min != DateTimePicker.MinimumDateTime && this.min != DateTime.MinValue;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000D2AD8 File Offset: 0x000D1AD8
		private bool ShouldSerializeValue()
		{
			return this.userHasSetValue;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000D2AE0 File Offset: 0x000D1AE0
		private bool ShouldSerializeFormat()
		{
			return this.Format != DateTimePickerFormat.Long;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x000D2AF0 File Offset: 0x000D1AF0
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", Value: " + DateTimePicker.FormatDateTime(this.Value);
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x000D2B1C File Offset: 0x000D1B1C
		private void UpdateUpDown()
		{
			if (this.ShowUpDown)
			{
				DateTimePicker.EnumChildren enumChildren = new DateTimePicker.EnumChildren();
				NativeMethods.EnumChildrenCallback enumChildrenCallback = new NativeMethods.EnumChildrenCallback(enumChildren.enumChildren);
				UnsafeNativeMethods.EnumChildWindows(new HandleRef(this, base.Handle), enumChildrenCallback, NativeMethods.NullHandleRef);
				if (enumChildren.hwndFound != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(enumChildren, enumChildren.hwndFound), null, true);
					SafeNativeMethods.UpdateWindow(new HandleRef(enumChildren, enumChildren.hwndFound));
				}
			}
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x000D2B94 File Offset: 0x000D1B94
		private void MarshaledUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			try
			{
				base.BeginInvoke(new UserPreferenceChangedEventHandler(this.UserPreferenceChanged), new object[] { sender, pref });
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x000D2BDC File Offset: 0x000D1BDC
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x000D2BEE File Offset: 0x000D1BEE
		private void WmCloseUp(ref Message m)
		{
			this.OnCloseUp(EventArgs.Empty);
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x000D2BFC File Offset: 0x000D1BFC
		private void WmDateTimeChange(ref Message m)
		{
			NativeMethods.NMDATETIMECHANGE nmdatetimechange = (NativeMethods.NMDATETIMECHANGE)m.GetLParam(typeof(NativeMethods.NMDATETIMECHANGE));
			DateTime dateTime = this.value;
			bool flag = this.validTime;
			if (nmdatetimechange.dwFlags != 1)
			{
				this.validTime = true;
				this.value = DateTimePicker.SysTimeToDateTime(nmdatetimechange.st);
				this.userHasSetValue = true;
			}
			else
			{
				this.validTime = false;
			}
			if (this.value != dateTime || flag != this.validTime)
			{
				this.OnValueChanged(EventArgs.Empty);
				this.OnTextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x000D2C8C File Offset: 0x000D1C8C
		private void WmDropDown(ref Message m)
		{
			if (this.RightToLeftLayout && this.RightToLeft == RightToLeft.Yes)
			{
				IntPtr intPtr = base.SendMessage(4104, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					int num = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, intPtr), -20);
					num |= 5242880;
					num &= -12289;
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, intPtr), -20, new HandleRef(this, (IntPtr)num));
				}
			}
			this.OnDropDown(EventArgs.Empty);
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x000D2D10 File Offset: 0x000D1D10
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			this.SetAllControlColors();
			base.OnSystemColorsChanged(e);
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x000D2D20 File Offset: 0x000D1D20
		private void WmReflectCommand(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				int code = ((NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR))).code;
				if (code == -759)
				{
					this.WmDateTimeChange(ref m);
					return;
				}
				switch (code)
				{
				case -754:
					this.WmDropDown(ref m);
					break;
				case -753:
					this.WmCloseUp(ref m);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000D2D94 File Offset: 0x000D1D94
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 71)
			{
				if (msg != 513)
				{
					if (msg == 8270)
					{
						this.WmReflectCommand(ref m);
						base.WndProc(ref m);
						return;
					}
					base.WndProc(ref m);
				}
				else
				{
					this.FocusInternal();
					if (!base.ValidationCancelled)
					{
						base.WndProc(ref m);
						return;
					}
				}
				return;
			}
			base.WndProc(ref m);
			this.UpdateUpDown();
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x000D2DFC File Offset: 0x000D1DFC
		internal static NativeMethods.SYSTEMTIME DateTimeToSysTime(DateTime time)
		{
			return new NativeMethods.SYSTEMTIME
			{
				wYear = (short)time.Year,
				wMonth = (short)time.Month,
				wDayOfWeek = (short)time.DayOfWeek,
				wDay = (short)time.Day,
				wHour = (short)time.Hour,
				wMinute = (short)time.Minute,
				wSecond = (short)time.Second,
				wMilliseconds = 0
			};
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x000D2E79 File Offset: 0x000D1E79
		internal static DateTime SysTimeToDateTime(NativeMethods.SYSTEMTIME s)
		{
			return new DateTime((int)s.wYear, (int)s.wMonth, (int)s.wDay, (int)s.wHour, (int)s.wMinute, (int)s.wSecond);
		}

		// Token: 0x04001CAA RID: 7338
		private const int TIMEFORMAT_NOUPDOWN = 8;

		// Token: 0x04001CAB RID: 7339
		protected static readonly Color DefaultTitleBackColor = SystemColors.ActiveCaption;

		// Token: 0x04001CAC RID: 7340
		protected static readonly Color DefaultTitleForeColor = SystemColors.ActiveCaptionText;

		// Token: 0x04001CAD RID: 7341
		protected static readonly Color DefaultMonthBackColor = SystemColors.Window;

		// Token: 0x04001CAE RID: 7342
		protected static readonly Color DefaultTrailingForeColor = SystemColors.GrayText;

		// Token: 0x04001CAF RID: 7343
		private static readonly object EVENT_FORMATCHANGED = new object();

		// Token: 0x04001CB0 RID: 7344
		private EventHandler onCloseUp;

		// Token: 0x04001CB1 RID: 7345
		private EventHandler onDropDown;

		// Token: 0x04001CB2 RID: 7346
		private EventHandler onValueChanged;

		// Token: 0x04001CB3 RID: 7347
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x04001CB4 RID: 7348
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);

		// Token: 0x04001CB5 RID: 7349
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public static readonly DateTime MaxDateTime = new DateTime(9998, 12, 31);

		// Token: 0x04001CB6 RID: 7350
		private int style;

		// Token: 0x04001CB7 RID: 7351
		private short prefHeightCache = -1;

		// Token: 0x04001CB8 RID: 7352
		private bool validTime = true;

		// Token: 0x04001CB9 RID: 7353
		private bool userHasSetValue;

		// Token: 0x04001CBA RID: 7354
		private DateTime value = DateTime.Now;

		// Token: 0x04001CBB RID: 7355
		private DateTime creationTime = DateTime.Now;

		// Token: 0x04001CBC RID: 7356
		private DateTime max = DateTime.MaxValue;

		// Token: 0x04001CBD RID: 7357
		private DateTime min = DateTime.MinValue;

		// Token: 0x04001CBE RID: 7358
		private Color calendarForeColor = Control.DefaultForeColor;

		// Token: 0x04001CBF RID: 7359
		private Color calendarTitleBackColor = DateTimePicker.DefaultTitleBackColor;

		// Token: 0x04001CC0 RID: 7360
		private Color calendarTitleForeColor = DateTimePicker.DefaultTitleForeColor;

		// Token: 0x04001CC1 RID: 7361
		private Color calendarMonthBackground = DateTimePicker.DefaultMonthBackColor;

		// Token: 0x04001CC2 RID: 7362
		private Color calendarTrailingText = DateTimePicker.DefaultTrailingForeColor;

		// Token: 0x04001CC3 RID: 7363
		private Font calendarFont;

		// Token: 0x04001CC4 RID: 7364
		private Control.FontHandleWrapper calendarFontHandleWrapper;

		// Token: 0x04001CC5 RID: 7365
		private string customFormat;

		// Token: 0x04001CC6 RID: 7366
		private DateTimePickerFormat format;

		// Token: 0x04001CC7 RID: 7367
		private bool rightToLeftLayout;

		// Token: 0x020003AF RID: 943
		private sealed class EnumChildren
		{
			// Token: 0x060039C0 RID: 14784 RVA: 0x000D2F07 File Offset: 0x000D1F07
			public bool enumChildren(IntPtr hwnd, IntPtr lparam)
			{
				this.hwndFound = hwnd;
				return true;
			}

			// Token: 0x04001CC8 RID: 7368
			public IntPtr hwndFound = IntPtr.Zero;
		}

		// Token: 0x020003B0 RID: 944
		[ComVisible(true)]
		public class DateTimePickerAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x060039C2 RID: 14786 RVA: 0x000D2F24 File Offset: 0x000D1F24
			public DateTimePickerAccessibleObject(DateTimePicker owner)
				: base(owner)
			{
			}

			// Token: 0x17000ACB RID: 2763
			// (get) Token: 0x060039C3 RID: 14787 RVA: 0x000D2F30 File Offset: 0x000D1F30
			public override string KeyboardShortcut
			{
				get
				{
					Label previousLabel = base.PreviousLabel;
					if (previousLabel != null)
					{
						char mnemonic = WindowsFormsUtils.GetMnemonic(previousLabel.Text, false);
						if (mnemonic != '\0')
						{
							return "Alt+" + mnemonic;
						}
					}
					string keyboardShortcut = base.KeyboardShortcut;
					if (keyboardShortcut == null || keyboardShortcut.Length == 0)
					{
						char mnemonic2 = WindowsFormsUtils.GetMnemonic(base.Owner.Text, false);
						if (mnemonic2 != '\0')
						{
							return "Alt+" + mnemonic2;
						}
					}
					return keyboardShortcut;
				}
			}

			// Token: 0x17000ACC RID: 2764
			// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000D2FA4 File Offset: 0x000D1FA4
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					string value = base.Value;
					if (value == null || value.Length == 0)
					{
						return base.Owner.Text;
					}
					return value;
				}
			}

			// Token: 0x17000ACD RID: 2765
			// (get) Token: 0x060039C5 RID: 14789 RVA: 0x000D2FD0 File Offset: 0x000D1FD0
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					if (((DateTimePicker)base.Owner).ShowCheckBox && ((DateTimePicker)base.Owner).Checked)
					{
						accessibleStates |= AccessibleStates.Checked;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000ACE RID: 2766
			// (get) Token: 0x060039C6 RID: 14790 RVA: 0x000D3010 File Offset: 0x000D2010
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.DropList;
				}
			}
		}
	}
}
