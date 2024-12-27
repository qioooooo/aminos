using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000426 RID: 1062
	[DefaultProperty("Value")]
	[DefaultEvent("Scroll")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public abstract class ScrollBar : Control
	{
		// Token: 0x06003EF5 RID: 16117 RVA: 0x000E5268 File Offset: 0x000E4268
		public ScrollBar()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.TabStop = false;
			if ((this.CreateParams.Style & 1) != 0)
			{
				this.scrollOrientation = ScrollOrientation.VerticalScroll;
				return;
			}
			this.scrollOrientation = ScrollOrientation.HorizontalScroll;
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x000E52D7 File Offset: 0x000E42D7
		// (set) Token: 0x06003EF7 RID: 16119 RVA: 0x000E52DF File Offset: 0x000E42DF
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x1400021F RID: 543
		// (add) Token: 0x06003EF8 RID: 16120 RVA: 0x000E52E8 File Offset: 0x000E42E8
		// (remove) Token: 0x06003EF9 RID: 16121 RVA: 0x000E52F1 File Offset: 0x000E42F1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06003EFA RID: 16122 RVA: 0x000E52FA File Offset: 0x000E42FA
		// (set) Token: 0x06003EFB RID: 16123 RVA: 0x000E5302 File Offset: 0x000E4302
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x14000220 RID: 544
		// (add) Token: 0x06003EFC RID: 16124 RVA: 0x000E530B File Offset: 0x000E430B
		// (remove) Token: 0x06003EFD RID: 16125 RVA: 0x000E5314 File Offset: 0x000E4314
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

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x000E531D File Offset: 0x000E431D
		// (set) Token: 0x06003EFF RID: 16127 RVA: 0x000E5325 File Offset: 0x000E4325
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

		// Token: 0x14000221 RID: 545
		// (add) Token: 0x06003F00 RID: 16128 RVA: 0x000E532E File Offset: 0x000E432E
		// (remove) Token: 0x06003F01 RID: 16129 RVA: 0x000E5337 File Offset: 0x000E4337
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

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06003F02 RID: 16130 RVA: 0x000E5340 File Offset: 0x000E4340
		// (set) Token: 0x06003F03 RID: 16131 RVA: 0x000E5348 File Offset: 0x000E4348
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

		// Token: 0x14000222 RID: 546
		// (add) Token: 0x06003F04 RID: 16132 RVA: 0x000E5351 File Offset: 0x000E4351
		// (remove) Token: 0x06003F05 RID: 16133 RVA: 0x000E535A File Offset: 0x000E435A
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

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06003F06 RID: 16134 RVA: 0x000E5364 File Offset: 0x000E4364
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SCROLLBAR";
				createParams.Style &= -8388609;
				return createParams;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003F07 RID: 16135 RVA: 0x000E5396 File Offset: 0x000E4396
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06003F08 RID: 16136 RVA: 0x000E5399 File Offset: 0x000E4399
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06003F09 RID: 16137 RVA: 0x000E53A0 File Offset: 0x000E43A0
		// (set) Token: 0x06003F0A RID: 16138 RVA: 0x000E53A8 File Offset: 0x000E43A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000223 RID: 547
		// (add) Token: 0x06003F0B RID: 16139 RVA: 0x000E53B1 File Offset: 0x000E43B1
		// (remove) Token: 0x06003F0C RID: 16140 RVA: 0x000E53BA File Offset: 0x000E43BA
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

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x000E53C3 File Offset: 0x000E43C3
		// (set) Token: 0x06003F0E RID: 16142 RVA: 0x000E53CB File Offset: 0x000E43CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x14000224 RID: 548
		// (add) Token: 0x06003F0F RID: 16143 RVA: 0x000E53D4 File Offset: 0x000E43D4
		// (remove) Token: 0x06003F10 RID: 16144 RVA: 0x000E53DD File Offset: 0x000E43DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06003F11 RID: 16145 RVA: 0x000E53E6 File Offset: 0x000E43E6
		// (set) Token: 0x06003F12 RID: 16146 RVA: 0x000E53EE File Offset: 0x000E43EE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x14000225 RID: 549
		// (add) Token: 0x06003F13 RID: 16147 RVA: 0x000E53F7 File Offset: 0x000E43F7
		// (remove) Token: 0x06003F14 RID: 16148 RVA: 0x000E5400 File Offset: 0x000E4400
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06003F15 RID: 16149 RVA: 0x000E5409 File Offset: 0x000E4409
		// (set) Token: 0x06003F16 RID: 16150 RVA: 0x000E5428 File Offset: 0x000E4428
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(10)]
		[SRDescription("ScrollBarLargeChangeDescr")]
		[SRCategory("CatBehavior")]
		public int LargeChange
		{
			get
			{
				return Math.Min(this.largeChange, this.maximum - this.minimum + 1);
			}
			set
			{
				if (this.largeChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("LargeChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"LargeChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.largeChange = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06003F17 RID: 16151 RVA: 0x000E5494 File Offset: 0x000E4494
		// (set) Token: 0x06003F18 RID: 16152 RVA: 0x000E549C File Offset: 0x000E449C
		[SRDescription("ScrollBarMaximumDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(100)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.maximum != value)
				{
					if (this.minimum > value)
					{
						this.minimum = value;
					}
					if (value < this.value)
					{
						this.Value = value;
					}
					this.maximum = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x000E54D4 File Offset: 0x000E44D4
		// (set) Token: 0x06003F1A RID: 16154 RVA: 0x000E54DC File Offset: 0x000E44DC
		[SRCategory("CatBehavior")]
		[SRDescription("ScrollBarMinimumDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.minimum != value)
				{
					if (this.maximum < value)
					{
						this.maximum = value;
					}
					if (value > this.value)
					{
						this.value = value;
					}
					this.minimum = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06003F1B RID: 16155 RVA: 0x000E5514 File Offset: 0x000E4514
		// (set) Token: 0x06003F1C RID: 16156 RVA: 0x000E5528 File Offset: 0x000E4528
		[SRDescription("ScrollBarSmallChangeDescr")]
		[DefaultValue(1)]
		[SRCategory("CatBehavior")]
		public int SmallChange
		{
			get
			{
				return Math.Min(this.smallChange, this.LargeChange);
			}
			set
			{
				if (this.smallChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("SmallChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SmallChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.smallChange = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x000E5594 File Offset: 0x000E4594
		// (set) Token: 0x06003F1E RID: 16158 RVA: 0x000E559C File Offset: 0x000E459C
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06003F1F RID: 16159 RVA: 0x000E55A5 File Offset: 0x000E45A5
		// (set) Token: 0x06003F20 RID: 16160 RVA: 0x000E55AD File Offset: 0x000E45AD
		[Browsable(false)]
		[Bindable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x14000226 RID: 550
		// (add) Token: 0x06003F21 RID: 16161 RVA: 0x000E55B6 File Offset: 0x000E45B6
		// (remove) Token: 0x06003F22 RID: 16162 RVA: 0x000E55BF File Offset: 0x000E45BF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x000E55C8 File Offset: 0x000E45C8
		// (set) Token: 0x06003F24 RID: 16164 RVA: 0x000E55D0 File Offset: 0x000E45D0
		[Bindable(true)]
		[SRDescription("ScrollBarValueDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (value < this.minimum || value > this.maximum)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'minimum'",
							"'maximum'"
						}));
					}
					this.value = value;
					this.UpdateScrollInfo();
					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000227 RID: 551
		// (add) Token: 0x06003F25 RID: 16165 RVA: 0x000E5654 File Offset: 0x000E4654
		// (remove) Token: 0x06003F26 RID: 16166 RVA: 0x000E565D File Offset: 0x000E465D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000228 RID: 552
		// (add) Token: 0x06003F27 RID: 16167 RVA: 0x000E5666 File Offset: 0x000E4666
		// (remove) Token: 0x06003F28 RID: 16168 RVA: 0x000E566F File Offset: 0x000E466F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000229 RID: 553
		// (add) Token: 0x06003F29 RID: 16169 RVA: 0x000E5678 File Offset: 0x000E4678
		// (remove) Token: 0x06003F2A RID: 16170 RVA: 0x000E5681 File Offset: 0x000E4681
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1400022A RID: 554
		// (add) Token: 0x06003F2B RID: 16171 RVA: 0x000E568A File Offset: 0x000E468A
		// (remove) Token: 0x06003F2C RID: 16172 RVA: 0x000E5693 File Offset: 0x000E4693
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x1400022B RID: 555
		// (add) Token: 0x06003F2D RID: 16173 RVA: 0x000E569C File Offset: 0x000E469C
		// (remove) Token: 0x06003F2E RID: 16174 RVA: 0x000E56A5 File Offset: 0x000E46A5
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

		// Token: 0x1400022C RID: 556
		// (add) Token: 0x06003F2F RID: 16175 RVA: 0x000E56AE File Offset: 0x000E46AE
		// (remove) Token: 0x06003F30 RID: 16176 RVA: 0x000E56B7 File Offset: 0x000E46B7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				base.MouseDown += value;
			}
			remove
			{
				base.MouseDown -= value;
			}
		}

		// Token: 0x1400022D RID: 557
		// (add) Token: 0x06003F31 RID: 16177 RVA: 0x000E56C0 File Offset: 0x000E46C0
		// (remove) Token: 0x06003F32 RID: 16178 RVA: 0x000E56C9 File Offset: 0x000E46C9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				base.MouseUp += value;
			}
			remove
			{
				base.MouseUp -= value;
			}
		}

		// Token: 0x1400022E RID: 558
		// (add) Token: 0x06003F33 RID: 16179 RVA: 0x000E56D2 File Offset: 0x000E46D2
		// (remove) Token: 0x06003F34 RID: 16180 RVA: 0x000E56DB File Offset: 0x000E46DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				base.MouseMove += value;
			}
			remove
			{
				base.MouseMove -= value;
			}
		}

		// Token: 0x1400022F RID: 559
		// (add) Token: 0x06003F35 RID: 16181 RVA: 0x000E56E4 File Offset: 0x000E46E4
		// (remove) Token: 0x06003F36 RID: 16182 RVA: 0x000E56F7 File Offset: 0x000E46F7
		[SRCategory("CatAction")]
		[SRDescription("ScrollBarOnScrollDescr")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(ScrollBar.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollBar.EVENT_SCROLL, value);
			}
		}

		// Token: 0x14000230 RID: 560
		// (add) Token: 0x06003F37 RID: 16183 RVA: 0x000E570A File Offset: 0x000E470A
		// (remove) Token: 0x06003F38 RID: 16184 RVA: 0x000E571D File Offset: 0x000E471D
		[SRCategory("CatAction")]
		[SRDescription("valueChangedEventDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				base.Events.AddHandler(ScrollBar.EVENT_VALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollBar.EVENT_VALUECHANGED, value);
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000E5730 File Offset: 0x000E4730
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			if (this.scrollOrientation == ScrollOrientation.VerticalScroll)
			{
				specified &= ~BoundsSpecified.Width;
			}
			else
			{
				specified &= ~BoundsSpecified.Height;
			}
			return base.GetScaledBounds(bounds, factor, specified);
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x000E5752 File Offset: 0x000E4752
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			return IntPtr.Zero;
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x000E5759 File Offset: 0x000E4759
		protected override void OnEnabledChanged(EventArgs e)
		{
			if (base.Enabled)
			{
				this.UpdateScrollInfo();
			}
			base.OnEnabledChanged(e);
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x000E5770 File Offset: 0x000E4770
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateScrollInfo();
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x000E5780 File Offset: 0x000E4780
		protected virtual void OnScroll(ScrollEventArgs se)
		{
			ScrollEventHandler scrollEventHandler = (ScrollEventHandler)base.Events[ScrollBar.EVENT_SCROLL];
			if (scrollEventHandler != null)
			{
				scrollEventHandler(this, se);
			}
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x000E57B0 File Offset: 0x000E47B0
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.wheelDelta += e.Delta;
			bool flag = false;
			while (Math.Abs(this.wheelDelta) >= 120)
			{
				if (this.wheelDelta > 0)
				{
					this.wheelDelta -= 120;
					this.DoScroll(ScrollEventType.SmallDecrement);
					flag = true;
				}
				else
				{
					this.wheelDelta += 120;
					this.DoScroll(ScrollEventType.SmallIncrement);
					flag = true;
				}
			}
			if (flag)
			{
				this.DoScroll(ScrollEventType.EndScroll);
			}
			if (e is HandledMouseEventArgs)
			{
				((HandledMouseEventArgs)e).Handled = true;
			}
			base.OnMouseWheel(e);
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x000E5844 File Offset: 0x000E4844
		protected virtual void OnValueChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ScrollBar.EVENT_VALUECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x000E5872 File Offset: 0x000E4872
		private int ReflectPosition(int position)
		{
			if (this is HScrollBar)
			{
				return this.minimum + (this.maximum - this.LargeChange + 1) - position;
			}
			return position;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x000E5898 File Offset: 0x000E4898
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", Minimum: ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum: ",
				this.Maximum.ToString(CultureInfo.CurrentCulture),
				", Value: ",
				this.Value.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x000E5918 File Offset: 0x000E4918
		protected void UpdateScrollInfo()
		{
			if (base.IsHandleCreated && base.Enabled)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 23;
				scrollinfo.nMin = this.minimum;
				scrollinfo.nMax = this.maximum;
				scrollinfo.nPage = this.LargeChange;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					scrollinfo.nPos = this.ReflectPosition(this.value);
				}
				else
				{
					scrollinfo.nPos = this.value;
				}
				scrollinfo.nTrackPos = 0;
				UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 2, scrollinfo, true);
			}
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x000E59C8 File Offset: 0x000E49C8
		private void WmReflectScroll(ref Message m)
		{
			ScrollEventType scrollEventType = (ScrollEventType)NativeMethods.Util.LOWORD(m.WParam);
			this.DoScroll(scrollEventType);
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x000E59E8 File Offset: 0x000E49E8
		private void DoScroll(ScrollEventType type)
		{
			if (this.RightToLeft == RightToLeft.Yes)
			{
				switch (type)
				{
				case ScrollEventType.SmallDecrement:
					type = ScrollEventType.SmallIncrement;
					break;
				case ScrollEventType.SmallIncrement:
					type = ScrollEventType.SmallDecrement;
					break;
				case ScrollEventType.LargeDecrement:
					type = ScrollEventType.LargeIncrement;
					break;
				case ScrollEventType.LargeIncrement:
					type = ScrollEventType.LargeDecrement;
					break;
				case ScrollEventType.First:
					type = ScrollEventType.Last;
					break;
				case ScrollEventType.Last:
					type = ScrollEventType.First;
					break;
				}
			}
			int num = this.value;
			int num2 = this.value;
			switch (type)
			{
			case ScrollEventType.SmallDecrement:
				num = Math.Max(this.value - this.SmallChange, this.minimum);
				break;
			case ScrollEventType.SmallIncrement:
				num = Math.Min(this.value + this.SmallChange, this.maximum - this.LargeChange + 1);
				break;
			case ScrollEventType.LargeDecrement:
				num = Math.Max(this.value - this.LargeChange, this.minimum);
				break;
			case ScrollEventType.LargeIncrement:
				num = Math.Min(this.value + this.LargeChange, this.maximum - this.LargeChange + 1);
				break;
			case ScrollEventType.ThumbPosition:
			case ScrollEventType.ThumbTrack:
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.fMask = 16;
				SafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), 2, scrollinfo);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					num = this.ReflectPosition(scrollinfo.nTrackPos);
				}
				else
				{
					num = scrollinfo.nTrackPos;
				}
				break;
			}
			case ScrollEventType.First:
				num = this.minimum;
				break;
			case ScrollEventType.Last:
				num = this.maximum - this.LargeChange + 1;
				break;
			}
			ScrollEventArgs scrollEventArgs = new ScrollEventArgs(type, num2, num, this.scrollOrientation);
			this.OnScroll(scrollEventArgs);
			this.Value = scrollEventArgs.NewValue;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x000E5B8C File Offset: 0x000E4B8C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 5)
			{
				if (msg != 20)
				{
					switch (msg)
					{
					case 8468:
					case 8469:
						this.WmReflectScroll(ref m);
						return;
					default:
						base.WndProc(ref m);
						break;
					}
				}
			}
			else if (UnsafeNativeMethods.GetFocus() == base.Handle)
			{
				this.DefWndProc(ref m);
				base.SendMessage(8, 0, 0);
				base.SendMessage(7, 0, 0);
				return;
			}
		}

		// Token: 0x04001F10 RID: 7952
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04001F11 RID: 7953
		private static readonly object EVENT_VALUECHANGED = new object();

		// Token: 0x04001F12 RID: 7954
		private int minimum;

		// Token: 0x04001F13 RID: 7955
		private int maximum = 100;

		// Token: 0x04001F14 RID: 7956
		private int smallChange = 1;

		// Token: 0x04001F15 RID: 7957
		private int largeChange = 10;

		// Token: 0x04001F16 RID: 7958
		private int value;

		// Token: 0x04001F17 RID: 7959
		private ScrollOrientation scrollOrientation;

		// Token: 0x04001F18 RID: 7960
		private int wheelDelta;
	}
}
