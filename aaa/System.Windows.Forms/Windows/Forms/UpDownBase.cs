using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020003B8 RID: 952
	[Designer("System.Windows.Forms.Design.UpDownBaseDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public abstract class UpDownBase : ContainerControl
	{
		// Token: 0x060039D8 RID: 14808 RVA: 0x000D320C File Offset: 0x000D220C
		public UpDownBase()
		{
			this.upDownButtons = new UpDownBase.UpDownButtons(this);
			this.upDownEdit = new UpDownBase.UpDownEdit(this);
			this.upDownEdit.BorderStyle = BorderStyle.None;
			this.upDownEdit.AutoSize = false;
			this.upDownEdit.KeyDown += this.OnTextBoxKeyDown;
			this.upDownEdit.KeyPress += this.OnTextBoxKeyPress;
			this.upDownEdit.TextChanged += this.OnTextBoxTextChanged;
			this.upDownEdit.LostFocus += this.OnTextBoxLostFocus;
			this.upDownEdit.Resize += this.OnTextBoxResize;
			this.upDownButtons.TabStop = false;
			this.upDownButtons.Size = new Size(16, this.PreferredHeight);
			this.upDownButtons.UpDown += this.OnUpDown;
			base.Controls.AddRange(new Control[] { this.upDownButtons, this.upDownEdit });
			base.SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.FixedHeight, true);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x000D3361 File Offset: 0x000D2361
		// (set) Token: 0x060039DA RID: 14810 RVA: 0x000D3364 File Offset: 0x000D2364
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x000D3366 File Offset: 0x000D2366
		// (set) Token: 0x060039DC RID: 14812 RVA: 0x000D336E File Offset: 0x000D236E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000D3377 File Offset: 0x000D2377
		// (set) Token: 0x060039DE RID: 14814 RVA: 0x000D337F File Offset: 0x000D237F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x000D3388 File Offset: 0x000D2388
		// (set) Token: 0x060039E0 RID: 14816 RVA: 0x000D3390 File Offset: 0x000D2390
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
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

		// Token: 0x140001E5 RID: 485
		// (add) Token: 0x060039E1 RID: 14817 RVA: 0x000D3399 File Offset: 0x000D2399
		// (remove) Token: 0x060039E2 RID: 14818 RVA: 0x000D33A2 File Offset: 0x000D23A2
		[SRCategory("CatPropertyChanged")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
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

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x000D33AB File Offset: 0x000D23AB
		// (set) Token: 0x060039E4 RID: 14820 RVA: 0x000D33B8 File Offset: 0x000D23B8
		public override Color BackColor
		{
			get
			{
				return this.upDownEdit.BackColor;
			}
			set
			{
				base.BackColor = value;
				this.upDownEdit.BackColor = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000D33D3 File Offset: 0x000D23D3
		// (set) Token: 0x060039E6 RID: 14822 RVA: 0x000D33DB File Offset: 0x000D23DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140001E6 RID: 486
		// (add) Token: 0x060039E7 RID: 14823 RVA: 0x000D33E4 File Offset: 0x000D23E4
		// (remove) Token: 0x060039E8 RID: 14824 RVA: 0x000D33ED File Offset: 0x000D23ED
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

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060039E9 RID: 14825 RVA: 0x000D33F6 File Offset: 0x000D23F6
		// (set) Token: 0x060039EA RID: 14826 RVA: 0x000D33FE File Offset: 0x000D23FE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140001E7 RID: 487
		// (add) Token: 0x060039EB RID: 14827 RVA: 0x000D3407 File Offset: 0x000D2407
		// (remove) Token: 0x060039EC RID: 14828 RVA: 0x000D3410 File Offset: 0x000D2410
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

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x000D3419 File Offset: 0x000D2419
		// (set) Token: 0x060039EE RID: 14830 RVA: 0x000D3421 File Offset: 0x000D2421
		[DispId(-504)]
		[SRDescription("UpDownBaseBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
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
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x000D345F File Offset: 0x000D245F
		// (set) Token: 0x060039F0 RID: 14832 RVA: 0x000D3467 File Offset: 0x000D2467
		protected bool ChangingText
		{
			get
			{
				return this.changingText;
			}
			set
			{
				this.changingText = value;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x000D3470 File Offset: 0x000D2470
		// (set) Token: 0x060039F2 RID: 14834 RVA: 0x000D3478 File Offset: 0x000D2478
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
				this.upDownEdit.ContextMenu = value;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060039F3 RID: 14835 RVA: 0x000D348D File Offset: 0x000D248D
		// (set) Token: 0x060039F4 RID: 14836 RVA: 0x000D3495 File Offset: 0x000D2495
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
				this.upDownEdit.ContextMenuStrip = value;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x000D34AC File Offset: 0x000D24AC
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -8388609;
				if (!Application.RenderWithVisualStyles)
				{
					switch (this.borderStyle)
					{
					case BorderStyle.FixedSingle:
						createParams.Style |= 8388608;
						break;
					case BorderStyle.Fixed3D:
						createParams.ExStyle |= 512;
						break;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x000D3519 File Offset: 0x000D2519
		protected override Size DefaultSize
		{
			get
			{
				return new Size(120, this.PreferredHeight);
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060039F7 RID: 14839 RVA: 0x000D3528 File Offset: 0x000D2528
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060039F8 RID: 14840 RVA: 0x000D3530 File Offset: 0x000D2530
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ControlFocusedDescr")]
		public override bool Focused
		{
			get
			{
				return this.upDownEdit.Focused;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x000D353D File Offset: 0x000D253D
		// (set) Token: 0x060039FA RID: 14842 RVA: 0x000D354A File Offset: 0x000D254A
		public override Color ForeColor
		{
			get
			{
				return this.upDownEdit.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				this.upDownEdit.ForeColor = value;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x060039FB RID: 14843 RVA: 0x000D355F File Offset: 0x000D255F
		// (set) Token: 0x060039FC RID: 14844 RVA: 0x000D3567 File Offset: 0x000D2567
		[SRDescription("UpDownBaseInterceptArrowKeysDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool InterceptArrowKeys
		{
			get
			{
				return this.interceptArrowKeys;
			}
			set
			{
				this.interceptArrowKeys = value;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x060039FD RID: 14845 RVA: 0x000D3570 File Offset: 0x000D2570
		// (set) Token: 0x060039FE RID: 14846 RVA: 0x000D3578 File Offset: 0x000D2578
		public override Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = new Size(value.Width, 0);
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x060039FF RID: 14847 RVA: 0x000D358D File Offset: 0x000D258D
		// (set) Token: 0x06003A00 RID: 14848 RVA: 0x000D3595 File Offset: 0x000D2595
		public override Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = new Size(value.Width, 0);
			}
		}

		// Token: 0x140001E8 RID: 488
		// (add) Token: 0x06003A01 RID: 14849 RVA: 0x000D35AA File Offset: 0x000D25AA
		// (remove) Token: 0x06003A02 RID: 14850 RVA: 0x000D35B3 File Offset: 0x000D25B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseEnter
		{
			add
			{
				base.MouseEnter += value;
			}
			remove
			{
				base.MouseEnter -= value;
			}
		}

		// Token: 0x140001E9 RID: 489
		// (add) Token: 0x06003A03 RID: 14851 RVA: 0x000D35BC File Offset: 0x000D25BC
		// (remove) Token: 0x06003A04 RID: 14852 RVA: 0x000D35C5 File Offset: 0x000D25C5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseLeave
		{
			add
			{
				base.MouseLeave += value;
			}
			remove
			{
				base.MouseLeave -= value;
			}
		}

		// Token: 0x140001EA RID: 490
		// (add) Token: 0x06003A05 RID: 14853 RVA: 0x000D35CE File Offset: 0x000D25CE
		// (remove) Token: 0x06003A06 RID: 14854 RVA: 0x000D35D7 File Offset: 0x000D25D7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseHover
		{
			add
			{
				base.MouseHover += value;
			}
			remove
			{
				base.MouseHover -= value;
			}
		}

		// Token: 0x140001EB RID: 491
		// (add) Token: 0x06003A07 RID: 14855 RVA: 0x000D35E0 File Offset: 0x000D25E0
		// (remove) Token: 0x06003A08 RID: 14856 RVA: 0x000D35E9 File Offset: 0x000D25E9
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

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000D35F4 File Offset: 0x000D25F4
		[SRDescription("UpDownBasePreferredHeightDescr")]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int PreferredHeight
		{
			get
			{
				int num = base.FontHeight;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				else
				{
					num += 3;
				}
				return num;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003A0A RID: 14858 RVA: 0x000D362B File Offset: 0x000D262B
		// (set) Token: 0x06003A0B RID: 14859 RVA: 0x000D3638 File Offset: 0x000D2638
		[SRDescription("UpDownBaseReadOnlyDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ReadOnly
		{
			get
			{
				return this.upDownEdit.ReadOnly;
			}
			set
			{
				this.upDownEdit.ReadOnly = value;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x000D3646 File Offset: 0x000D2646
		// (set) Token: 0x06003A0D RID: 14861 RVA: 0x000D3653 File Offset: 0x000D2653
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return this.upDownEdit.Text;
			}
			set
			{
				this.upDownEdit.Text = value;
				this.ChangingText = false;
				if (this.UserEdit)
				{
					this.ValidateEditText();
				}
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003A0E RID: 14862 RVA: 0x000D3676 File Offset: 0x000D2676
		// (set) Token: 0x06003A0F RID: 14863 RVA: 0x000D3683 File Offset: 0x000D2683
		[DefaultValue(HorizontalAlignment.Left)]
		[SRCategory("CatAppearance")]
		[SRDescription("UpDownBaseTextAlignDescr")]
		[Localizable(true)]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return this.upDownEdit.TextAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.upDownEdit.TextAlign = value;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003A10 RID: 14864 RVA: 0x000D36B7 File Offset: 0x000D26B7
		internal TextBox TextBox
		{
			get
			{
				return this.upDownEdit;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003A11 RID: 14865 RVA: 0x000D36BF File Offset: 0x000D26BF
		// (set) Token: 0x06003A12 RID: 14866 RVA: 0x000D36C8 File Offset: 0x000D26C8
		[SRDescription("UpDownBaseAlignmentDescr")]
		[DefaultValue(LeftRightAlignment.Right)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public LeftRightAlignment UpDownAlign
		{
			get
			{
				return this.upDownAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LeftRightAlignment));
				}
				if (this.upDownAlign != value)
				{
					this.upDownAlign = value;
					this.PositionControls();
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x000D3717 File Offset: 0x000D2717
		internal UpDownBase.UpDownButtons UpDownButtonsInternal
		{
			get
			{
				return this.upDownButtons;
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x000D371F File Offset: 0x000D271F
		// (set) Token: 0x06003A15 RID: 14869 RVA: 0x000D3727 File Offset: 0x000D2727
		protected bool UserEdit
		{
			get
			{
				return this.userEdit;
			}
			set
			{
				this.userEdit = value;
			}
		}

		// Token: 0x06003A16 RID: 14870
		public abstract void DownButton();

		// Token: 0x06003A17 RID: 14871 RVA: 0x000D3730 File Offset: 0x000D2730
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, this.PreferredHeight);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000D3741 File Offset: 0x000D2741
		protected virtual void OnChanged(object source, EventArgs e)
		{
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x000D3743 File Offset: 0x000D2743
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.PositionControls();
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChanged;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x000D3763 File Offset: 0x000D2763
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x000D3780 File Offset: 0x000D2780
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle bounds = this.upDownEdit.Bounds;
			if (Application.RenderWithVisualStyles)
			{
				if (this.borderStyle == BorderStyle.None)
				{
					goto IL_0211;
				}
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle clipRectangle = e.ClipRectangle;
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
				int num = 1;
				Rectangle rectangle = new Rectangle(clientRectangle.Left, clientRectangle.Top, num, clientRectangle.Height);
				Rectangle rectangle2 = new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, num);
				Rectangle rectangle3 = new Rectangle(clientRectangle.Right - num, clientRectangle.Top, num, clientRectangle.Height);
				Rectangle rectangle4 = new Rectangle(clientRectangle.Left, clientRectangle.Bottom - num, clientRectangle.Width, num);
				rectangle.Intersect(clipRectangle);
				rectangle2.Intersect(clipRectangle);
				rectangle3.Intersect(clipRectangle);
				rectangle4.Intersect(clipRectangle);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, rectangle);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, rectangle2);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, rectangle3);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, rectangle4);
				using (Pen pen = new Pen(this.BackColor))
				{
					Rectangle rectangle5 = bounds;
					rectangle5.X--;
					rectangle5.Y--;
					rectangle5.Width++;
					rectangle5.Height++;
					e.Graphics.DrawRectangle(pen, rectangle5);
					goto IL_0211;
				}
			}
			using (Pen pen2 = new Pen(this.BackColor, (float)(base.Enabled ? 2 : 1)))
			{
				Rectangle rectangle6 = bounds;
				rectangle6.Inflate(1, 1);
				if (!base.Enabled)
				{
					rectangle6.X--;
					rectangle6.Y--;
					rectangle6.Width++;
					rectangle6.Height++;
				}
				e.Graphics.DrawRectangle(pen2, rectangle6);
			}
			IL_0211:
			if (!base.Enabled && this.BorderStyle != BorderStyle.None && !this.upDownEdit.ShouldSerializeBackColor())
			{
				bounds.Inflate(1, 1);
				ControlPaint.DrawBorder(e.Graphics, bounds, SystemColors.Control, ButtonBorderStyle.Solid);
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000D39F4 File Offset: 0x000D29F4
		protected virtual void OnTextBoxKeyDown(object source, KeyEventArgs e)
		{
			this.OnKeyDown(e);
			if (this.interceptArrowKeys)
			{
				if (e.KeyData == Keys.Up)
				{
					this.UpButton();
					e.Handled = true;
				}
				else if (e.KeyData == Keys.Down)
				{
					this.DownButton();
					e.Handled = true;
				}
			}
			if (e.KeyCode == Keys.Return && this.UserEdit)
			{
				this.ValidateEditText();
			}
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000D3A58 File Offset: 0x000D2A58
		protected virtual void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x000D3A61 File Offset: 0x000D2A61
		protected virtual void OnTextBoxLostFocus(object source, EventArgs e)
		{
			if (this.UserEdit)
			{
				this.ValidateEditText();
			}
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x000D3A71 File Offset: 0x000D2A71
		protected virtual void OnTextBoxResize(object source, EventArgs e)
		{
			base.Height = this.PreferredHeight;
			this.PositionControls();
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x000D3A85 File Offset: 0x000D2A85
		protected virtual void OnTextBoxTextChanged(object source, EventArgs e)
		{
			if (this.changingText)
			{
				this.ChangingText = false;
			}
			else
			{
				this.UserEdit = true;
			}
			this.OnTextChanged(e);
			this.OnChanged(source, new EventArgs());
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x000D3AB2 File Offset: 0x000D2AB2
		internal virtual void OnStartTimer()
		{
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000D3AB4 File Offset: 0x000D2AB4
		internal virtual void OnStopTimer()
		{
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x000D3AB6 File Offset: 0x000D2AB6
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Clicks == 2 && e.Button == MouseButtons.Left)
			{
				this.doubleClickFired = true;
			}
			base.OnMouseDown(e);
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x000D3ADC File Offset: 0x000D2ADC
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle && !base.ValidationCancelled)
				{
					if (!this.doubleClickFired)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
					else
					{
						this.doubleClickFired = false;
						this.OnDoubleClick(mevent);
						this.OnMouseDoubleClick(mevent);
					}
				}
				this.doubleClickFired = false;
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x000D3B74 File Offset: 0x000D2B74
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			HandledMouseEventArgs handledMouseEventArgs = e as HandledMouseEventArgs;
			if (handledMouseEventArgs != null)
			{
				if (handledMouseEventArgs.Handled)
				{
					return;
				}
				handledMouseEventArgs.Handled = true;
			}
			if ((Control.ModifierKeys & (Keys.Shift | Keys.Alt)) != Keys.None || Control.MouseButtons != MouseButtons.None)
			{
				return;
			}
			int num = SystemInformation.MouseWheelScrollLines;
			if (num == 0)
			{
				return;
			}
			this.wheelDelta += e.Delta;
			float num2 = (float)this.wheelDelta / 120f;
			if (num == -1)
			{
				num = 1;
			}
			int num3 = (int)((float)num * num2);
			if (num3 != 0)
			{
				if (num3 > 0)
				{
					for (int i = num3; i > 0; i--)
					{
						this.UpButton();
					}
					this.wheelDelta -= (int)((float)num3 * (120f / (float)num));
					return;
				}
				for (int i = -num3; i > 0; i--)
				{
					this.DownButton();
				}
				this.wheelDelta -= (int)((float)num3 * (120f / (float)num));
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x000D3C55 File Offset: 0x000D2C55
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.PositionControls();
			base.OnLayout(e);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000D3C64 File Offset: 0x000D2C64
		protected override void OnFontChanged(EventArgs e)
		{
			base.FontHeight = -1;
			base.Height = this.PreferredHeight;
			this.PositionControls();
			base.OnFontChanged(e);
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x000D3C86 File Offset: 0x000D2C86
		private void OnUpDown(object source, UpDownEventArgs e)
		{
			if (e.ButtonID == 1)
			{
				this.UpButton();
				return;
			}
			if (e.ButtonID == 2)
			{
				this.DownButton();
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x000D3CA8 File Offset: 0x000D2CA8
		private void PositionControls()
		{
			Rectangle rectangle = Rectangle.Empty;
			Rectangle empty = Rectangle.Empty;
			Rectangle rectangle2 = new Rectangle(Point.Empty, base.ClientSize);
			int width = rectangle2.Width;
			bool renderWithVisualStyles = Application.RenderWithVisualStyles;
			BorderStyle borderStyle = this.BorderStyle;
			int num = ((borderStyle == BorderStyle.None) ? 0 : 2);
			rectangle2.Inflate(-num, -num);
			if (this.upDownEdit != null)
			{
				rectangle = rectangle2;
				rectangle.Size = new Size(rectangle2.Width - 16, rectangle2.Height);
			}
			if (this.upDownButtons != null)
			{
				int num2 = (renderWithVisualStyles ? 1 : 2);
				if (borderStyle == BorderStyle.None)
				{
					num2 = 0;
				}
				empty = new Rectangle(rectangle2.Right - 16 + num2, rectangle2.Top - num2, 16, rectangle2.Height + num2 * 2);
			}
			LeftRightAlignment leftRightAlignment = this.UpDownAlign;
			if (base.RtlTranslateLeftRight(leftRightAlignment) == LeftRightAlignment.Left)
			{
				empty.X = width - empty.Right;
				rectangle.X = width - rectangle.Right;
			}
			if (this.upDownEdit != null)
			{
				this.upDownEdit.Bounds = rectangle;
			}
			if (this.upDownButtons != null)
			{
				this.upDownButtons.Bounds = empty;
				this.upDownButtons.Invalidate();
			}
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x000D3DD6 File Offset: 0x000D2DD6
		public void Select(int start, int length)
		{
			this.upDownEdit.Select(start, length);
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x000D3DE8 File Offset: 0x000D2DE8
		private MouseEventArgs TranslateMouseEvent(Control child, MouseEventArgs e)
		{
			if (child != null && base.IsHandleCreated)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(e.X, e.Y);
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(child, child.Handle), new HandleRef(this, base.Handle), point, 1);
				return new MouseEventArgs(e.Button, e.Clicks, point.x, point.y, e.Delta);
			}
			return e;
		}

		// Token: 0x06003A2C RID: 14892
		public abstract void UpButton();

		// Token: 0x06003A2D RID: 14893
		protected abstract void UpdateEditText();

		// Token: 0x06003A2E RID: 14894 RVA: 0x000D3E57 File Offset: 0x000D2E57
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				this.UpdateEditText();
			}
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000D3E69 File Offset: 0x000D2E69
		protected virtual void ValidateEditText()
		{
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000D3E6C File Offset: 0x000D2E6C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
			case 7:
				if (base.HostedInWin32DialogManager)
				{
					if (this.TextBox.CanFocus)
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(this.TextBox, this.TextBox.Handle));
					}
					base.WndProc(ref m);
					return;
				}
				if (base.ActiveControl == null)
				{
					base.SetActiveControlInternal(this.TextBox);
					return;
				}
				base.FocusActiveControlInternal();
				return;
			case 8:
				this.DefWndProc(ref m);
				return;
			default:
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x000D3EF6 File Offset: 0x000D2EF6
		internal void SetToolTip(ToolTip toolTip, string caption)
		{
			toolTip.SetToolTip(this.upDownEdit, caption);
			toolTip.SetToolTip(this.upDownButtons, caption);
		}

		// Token: 0x04001CF8 RID: 7416
		private const int DefaultWheelScrollLinesPerPage = 1;

		// Token: 0x04001CF9 RID: 7417
		private const int DefaultButtonsWidth = 16;

		// Token: 0x04001CFA RID: 7418
		private const int DefaultControlWidth = 120;

		// Token: 0x04001CFB RID: 7419
		private const int ThemedBorderWidth = 1;

		// Token: 0x04001CFC RID: 7420
		private const BorderStyle DefaultBorderStyle = BorderStyle.Fixed3D;

		// Token: 0x04001CFD RID: 7421
		private const LeftRightAlignment DefaultUpDownAlign = LeftRightAlignment.Right;

		// Token: 0x04001CFE RID: 7422
		private const int DefaultTimerInterval = 500;

		// Token: 0x04001CFF RID: 7423
		private static readonly bool DefaultInterceptArrowKeys = true;

		// Token: 0x04001D00 RID: 7424
		internal UpDownBase.UpDownEdit upDownEdit;

		// Token: 0x04001D01 RID: 7425
		internal UpDownBase.UpDownButtons upDownButtons;

		// Token: 0x04001D02 RID: 7426
		private bool interceptArrowKeys = UpDownBase.DefaultInterceptArrowKeys;

		// Token: 0x04001D03 RID: 7427
		private LeftRightAlignment upDownAlign = LeftRightAlignment.Right;

		// Token: 0x04001D04 RID: 7428
		private bool userEdit;

		// Token: 0x04001D05 RID: 7429
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x04001D06 RID: 7430
		private int wheelDelta;

		// Token: 0x04001D07 RID: 7431
		private bool changingText;

		// Token: 0x04001D08 RID: 7432
		private bool doubleClickFired;

		// Token: 0x020003B9 RID: 953
		internal class UpDownEdit : TextBox
		{
			// Token: 0x06003A33 RID: 14899 RVA: 0x000D3F1A File Offset: 0x000D2F1A
			internal UpDownEdit(UpDownBase parent)
			{
				base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
				base.SetStyle(ControlStyles.Selectable, false);
				this.parent = parent;
			}

			// Token: 0x06003A34 RID: 14900 RVA: 0x000D3F3E File Offset: 0x000D2F3E
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new UpDownBase.UpDownEdit.UpDownEditAccessibleObject(this, this.parent);
			}

			// Token: 0x06003A35 RID: 14901 RVA: 0x000D3F4C File Offset: 0x000D2F4C
			protected override void OnMouseDown(MouseEventArgs e)
			{
				if (e.Clicks == 2 && e.Button == MouseButtons.Left)
				{
					this.doubleClickFired = true;
				}
				this.parent.OnMouseDown(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06003A36 RID: 14902 RVA: 0x000D3F84 File Offset: 0x000D2F84
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Point point = new Point(e.X, e.Y);
				point = base.PointToScreen(point);
				MouseEventArgs mouseEventArgs = this.parent.TranslateMouseEvent(this, e);
				if (e.Button == MouseButtons.Left)
				{
					if (!this.parent.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
					{
						if (!this.doubleClickFired)
						{
							this.parent.OnClick(mouseEventArgs);
							this.parent.OnMouseClick(mouseEventArgs);
						}
						else
						{
							this.doubleClickFired = false;
							this.parent.OnDoubleClick(mouseEventArgs);
							this.parent.OnMouseDoubleClick(mouseEventArgs);
						}
					}
					this.doubleClickFired = false;
				}
				this.parent.OnMouseUp(mouseEventArgs);
			}

			// Token: 0x06003A37 RID: 14903 RVA: 0x000D4048 File Offset: 0x000D3048
			internal override void WmContextMenu(ref Message m)
			{
				if (this.ContextMenu == null && this.ContextMenuStrip != null)
				{
					base.WmContextMenu(ref m, this.parent);
					return;
				}
				base.WmContextMenu(ref m, this);
			}

			// Token: 0x06003A38 RID: 14904 RVA: 0x000D4070 File Offset: 0x000D3070
			protected override void OnKeyUp(KeyEventArgs e)
			{
				this.parent.OnKeyUp(e);
			}

			// Token: 0x06003A39 RID: 14905 RVA: 0x000D407E File Offset: 0x000D307E
			protected override void OnGotFocus(EventArgs e)
			{
				this.parent.SetActiveControlInternal(this);
				this.parent.OnGotFocus(e);
			}

			// Token: 0x06003A3A RID: 14906 RVA: 0x000D4098 File Offset: 0x000D3098
			protected override void OnLostFocus(EventArgs e)
			{
				this.parent.OnLostFocus(e);
			}

			// Token: 0x04001D09 RID: 7433
			private UpDownBase parent;

			// Token: 0x04001D0A RID: 7434
			private bool doubleClickFired;

			// Token: 0x020003BA RID: 954
			internal class UpDownEditAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x06003A3B RID: 14907 RVA: 0x000D40A6 File Offset: 0x000D30A6
				public UpDownEditAccessibleObject(UpDownBase.UpDownEdit owner, UpDownBase parent)
					: base(owner)
				{
					this.parent = parent;
				}

				// Token: 0x17000AF1 RID: 2801
				// (get) Token: 0x06003A3C RID: 14908 RVA: 0x000D40B6 File Offset: 0x000D30B6
				// (set) Token: 0x06003A3D RID: 14909 RVA: 0x000D40C8 File Offset: 0x000D30C8
				public override string Name
				{
					get
					{
						return this.parent.AccessibilityObject.Name;
					}
					set
					{
						this.parent.AccessibilityObject.Name = value;
					}
				}

				// Token: 0x17000AF2 RID: 2802
				// (get) Token: 0x06003A3E RID: 14910 RVA: 0x000D40DB File Offset: 0x000D30DB
				public override string KeyboardShortcut
				{
					get
					{
						return this.parent.AccessibilityObject.KeyboardShortcut;
					}
				}

				// Token: 0x04001D0B RID: 7435
				private UpDownBase parent;
			}
		}

		// Token: 0x020003BB RID: 955
		internal class UpDownButtons : Control
		{
			// Token: 0x06003A3F RID: 14911 RVA: 0x000D40ED File Offset: 0x000D30ED
			internal UpDownButtons(UpDownBase parent)
			{
				base.SetStyle(ControlStyles.Opaque | ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
				base.SetStyle(ControlStyles.Selectable, false);
				this.parent = parent;
			}

			// Token: 0x140001EC RID: 492
			// (add) Token: 0x06003A40 RID: 14912 RVA: 0x000D4111 File Offset: 0x000D3111
			// (remove) Token: 0x06003A41 RID: 14913 RVA: 0x000D412A File Offset: 0x000D312A
			public event UpDownEventHandler UpDown
			{
				add
				{
					this.upDownEventHandler = (UpDownEventHandler)Delegate.Combine(this.upDownEventHandler, value);
				}
				remove
				{
					this.upDownEventHandler = (UpDownEventHandler)Delegate.Remove(this.upDownEventHandler, value);
				}
			}

			// Token: 0x06003A42 RID: 14914 RVA: 0x000D4144 File Offset: 0x000D3144
			private void BeginButtonPress(MouseEventArgs e)
			{
				int num = base.Size.Height / 2;
				if (e.Y < num)
				{
					this.pushed = (this.captured = UpDownBase.ButtonID.Up);
					base.Invalidate();
				}
				else
				{
					this.pushed = (this.captured = UpDownBase.ButtonID.Down);
					base.Invalidate();
				}
				base.CaptureInternal = true;
				this.OnUpDown(new UpDownEventArgs((int)this.pushed));
				this.StartTimer();
			}

			// Token: 0x06003A43 RID: 14915 RVA: 0x000D41B7 File Offset: 0x000D31B7
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject(this);
			}

			// Token: 0x06003A44 RID: 14916 RVA: 0x000D41BF File Offset: 0x000D31BF
			private void EndButtonPress()
			{
				this.pushed = UpDownBase.ButtonID.None;
				this.captured = UpDownBase.ButtonID.None;
				this.StopTimer();
				base.CaptureInternal = false;
				base.Invalidate();
			}

			// Token: 0x06003A45 RID: 14917 RVA: 0x000D41E4 File Offset: 0x000D31E4
			protected override void OnMouseDown(MouseEventArgs e)
			{
				this.parent.FocusInternal();
				if (!this.parent.ValidationCancelled && e.Button == MouseButtons.Left)
				{
					this.BeginButtonPress(e);
				}
				if (e.Clicks == 2 && e.Button == MouseButtons.Left)
				{
					this.doubleClickFired = true;
				}
				this.parent.OnMouseDown(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06003A46 RID: 14918 RVA: 0x000D4254 File Offset: 0x000D3254
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (base.Capture)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					clientRectangle.Height /= 2;
					if (this.captured == UpDownBase.ButtonID.Down)
					{
						clientRectangle.Y += clientRectangle.Height;
					}
					if (clientRectangle.Contains(e.X, e.Y))
					{
						if (this.pushed != this.captured)
						{
							this.StartTimer();
							this.pushed = this.captured;
							base.Invalidate();
						}
					}
					else if (this.pushed != UpDownBase.ButtonID.None)
					{
						this.StopTimer();
						this.pushed = UpDownBase.ButtonID.None;
						base.Invalidate();
					}
				}
				Rectangle clientRectangle2 = base.ClientRectangle;
				Rectangle clientRectangle3 = base.ClientRectangle;
				clientRectangle2.Height /= 2;
				clientRectangle3.Y += clientRectangle3.Height / 2;
				if (clientRectangle2.Contains(e.X, e.Y))
				{
					this.mouseOver = UpDownBase.ButtonID.Up;
					base.Invalidate();
				}
				else if (clientRectangle3.Contains(e.X, e.Y))
				{
					this.mouseOver = UpDownBase.ButtonID.Down;
					base.Invalidate();
				}
				this.parent.OnMouseMove(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06003A47 RID: 14919 RVA: 0x000D438C File Offset: 0x000D338C
			protected override void OnMouseUp(MouseEventArgs e)
			{
				if (!this.parent.ValidationCancelled && e.Button == MouseButtons.Left)
				{
					this.EndButtonPress();
				}
				Point point = new Point(e.X, e.Y);
				point = base.PointToScreen(point);
				MouseEventArgs mouseEventArgs = this.parent.TranslateMouseEvent(this, e);
				if (e.Button == MouseButtons.Left)
				{
					if (!this.parent.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
					{
						if (!this.doubleClickFired)
						{
							this.parent.OnClick(mouseEventArgs);
						}
						else
						{
							this.doubleClickFired = false;
							this.parent.OnDoubleClick(mouseEventArgs);
							this.parent.OnMouseDoubleClick(mouseEventArgs);
						}
					}
					this.doubleClickFired = false;
				}
				this.parent.OnMouseUp(mouseEventArgs);
			}

			// Token: 0x06003A48 RID: 14920 RVA: 0x000D4464 File Offset: 0x000D3464
			protected override void OnMouseLeave(EventArgs e)
			{
				this.mouseOver = UpDownBase.ButtonID.None;
				base.Invalidate();
				this.parent.OnMouseLeave(e);
			}

			// Token: 0x06003A49 RID: 14921 RVA: 0x000D4480 File Offset: 0x000D3480
			protected override void OnPaint(PaintEventArgs e)
			{
				int num = base.ClientSize.Height / 2;
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer((this.mouseOver == UpDownBase.ButtonID.Up) ? VisualStyleElement.Spin.Up.Hot : VisualStyleElement.Spin.Up.Normal);
					if (!base.Enabled)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Up.Disabled);
					}
					else if (this.pushed == UpDownBase.ButtonID.Up)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Up.Pressed);
					}
					visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, 0, 16, num));
					if (!base.Enabled)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Down.Disabled);
					}
					else if (this.pushed == UpDownBase.ButtonID.Down)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Down.Pressed);
					}
					else
					{
						visualStyleRenderer.SetParameters((this.mouseOver == UpDownBase.ButtonID.Up) ? VisualStyleElement.Spin.Down.Hot : VisualStyleElement.Spin.Down.Normal);
					}
					visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, num, 16, num));
				}
				else
				{
					ControlPaint.DrawScrollButton(e.Graphics, new Rectangle(0, 0, 16, num), ScrollButton.Up, (this.pushed == UpDownBase.ButtonID.Up) ? ButtonState.Pushed : (base.Enabled ? ButtonState.Normal : ButtonState.Inactive));
					ControlPaint.DrawScrollButton(e.Graphics, new Rectangle(0, num, 16, num), ScrollButton.Down, (this.pushed == UpDownBase.ButtonID.Down) ? ButtonState.Pushed : (base.Enabled ? ButtonState.Normal : ButtonState.Inactive));
				}
				if (num != (base.ClientSize.Height + 1) / 2)
				{
					using (Pen pen = new Pen(this.parent.BackColor))
					{
						Rectangle clientRectangle = base.ClientRectangle;
						e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom - 1, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
					}
				}
				base.OnPaint(e);
			}

			// Token: 0x06003A4A RID: 14922 RVA: 0x000D464C File Offset: 0x000D364C
			protected virtual void OnUpDown(UpDownEventArgs upevent)
			{
				if (this.upDownEventHandler != null)
				{
					this.upDownEventHandler(this, upevent);
				}
			}

			// Token: 0x06003A4B RID: 14923 RVA: 0x000D4664 File Offset: 0x000D3664
			protected void StartTimer()
			{
				this.parent.OnStartTimer();
				if (this.timer == null)
				{
					this.timer = new Timer();
					this.timer.Tick += this.TimerHandler;
				}
				this.timerInterval = 500;
				this.timer.Interval = this.timerInterval;
				this.timer.Start();
			}

			// Token: 0x06003A4C RID: 14924 RVA: 0x000D46CD File Offset: 0x000D36CD
			protected void StopTimer()
			{
				if (this.timer != null)
				{
					this.timer.Stop();
					this.timer.Dispose();
					this.timer = null;
				}
				this.parent.OnStopTimer();
			}

			// Token: 0x06003A4D RID: 14925 RVA: 0x000D4700 File Offset: 0x000D3700
			private void TimerHandler(object source, EventArgs args)
			{
				if (!base.Capture)
				{
					this.EndButtonPress();
					return;
				}
				this.OnUpDown(new UpDownEventArgs((int)this.pushed));
				this.timerInterval *= 7;
				this.timerInterval /= 10;
				if (this.timerInterval < 1)
				{
					this.timerInterval = 1;
				}
				this.timer.Interval = this.timerInterval;
			}

			// Token: 0x04001D0C RID: 7436
			private UpDownBase parent;

			// Token: 0x04001D0D RID: 7437
			private UpDownBase.ButtonID pushed;

			// Token: 0x04001D0E RID: 7438
			private UpDownBase.ButtonID captured;

			// Token: 0x04001D0F RID: 7439
			private UpDownBase.ButtonID mouseOver;

			// Token: 0x04001D10 RID: 7440
			private UpDownEventHandler upDownEventHandler;

			// Token: 0x04001D11 RID: 7441
			private Timer timer;

			// Token: 0x04001D12 RID: 7442
			private int timerInterval;

			// Token: 0x04001D13 RID: 7443
			private bool doubleClickFired;

			// Token: 0x020003BC RID: 956
			internal class UpDownButtonsAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x06003A4E RID: 14926 RVA: 0x000D476B File Offset: 0x000D376B
				public UpDownButtonsAccessibleObject(UpDownBase.UpDownButtons owner)
					: base(owner)
				{
				}

				// Token: 0x17000AF3 RID: 2803
				// (get) Token: 0x06003A4F RID: 14927 RVA: 0x000D4774 File Offset: 0x000D3774
				// (set) Token: 0x06003A50 RID: 14928 RVA: 0x000D479A File Offset: 0x000D379A
				public override string Name
				{
					get
					{
						string name = base.Name;
						if (name == null || name.Length == 0)
						{
							return "Spinner";
						}
						return name;
					}
					set
					{
						base.Name = value;
					}
				}

				// Token: 0x17000AF4 RID: 2804
				// (get) Token: 0x06003A51 RID: 14929 RVA: 0x000D47A4 File Offset: 0x000D37A4
				public override AccessibleRole Role
				{
					get
					{
						AccessibleRole accessibleRole = base.Owner.AccessibleRole;
						if (accessibleRole != AccessibleRole.Default)
						{
							return accessibleRole;
						}
						return AccessibleRole.SpinButton;
					}
				}

				// Token: 0x17000AF5 RID: 2805
				// (get) Token: 0x06003A52 RID: 14930 RVA: 0x000D47C5 File Offset: 0x000D37C5
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject UpButton
				{
					get
					{
						if (this.upButton == null)
						{
							this.upButton = new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject(this, true);
						}
						return this.upButton;
					}
				}

				// Token: 0x17000AF6 RID: 2806
				// (get) Token: 0x06003A53 RID: 14931 RVA: 0x000D47E2 File Offset: 0x000D37E2
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject DownButton
				{
					get
					{
						if (this.downButton == null)
						{
							this.downButton = new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject(this, false);
						}
						return this.downButton;
					}
				}

				// Token: 0x06003A54 RID: 14932 RVA: 0x000D47FF File Offset: 0x000D37FF
				public override AccessibleObject GetChild(int index)
				{
					if (index == 0)
					{
						return this.UpButton;
					}
					if (index == 1)
					{
						return this.DownButton;
					}
					return null;
				}

				// Token: 0x06003A55 RID: 14933 RVA: 0x000D4817 File Offset: 0x000D3817
				public override int GetChildCount()
				{
					return 2;
				}

				// Token: 0x04001D14 RID: 7444
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject upButton;

				// Token: 0x04001D15 RID: 7445
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject downButton;

				// Token: 0x020003BD RID: 957
				internal class DirectionButtonAccessibleObject : AccessibleObject
				{
					// Token: 0x06003A56 RID: 14934 RVA: 0x000D481A File Offset: 0x000D381A
					public DirectionButtonAccessibleObject(UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject parent, bool up)
					{
						this.parent = parent;
						this.up = up;
					}

					// Token: 0x17000AF7 RID: 2807
					// (get) Token: 0x06003A57 RID: 14935 RVA: 0x000D4830 File Offset: 0x000D3830
					public override Rectangle Bounds
					{
						get
						{
							Rectangle bounds = ((UpDownBase.UpDownButtons)this.parent.Owner).Bounds;
							bounds.Height /= 2;
							if (!this.up)
							{
								bounds.Y += bounds.Height;
							}
							return ((UpDownBase.UpDownButtons)this.parent.Owner).ParentInternal.RectangleToScreen(bounds);
						}
					}

					// Token: 0x17000AF8 RID: 2808
					// (get) Token: 0x06003A58 RID: 14936 RVA: 0x000D489A File Offset: 0x000D389A
					// (set) Token: 0x06003A59 RID: 14937 RVA: 0x000D48B9 File Offset: 0x000D38B9
					public override string Name
					{
						get
						{
							if (this.up)
							{
								return SR.GetString("UpDownBaseUpButtonAccName");
							}
							return SR.GetString("UpDownBaseDownButtonAccName");
						}
						set
						{
						}
					}

					// Token: 0x17000AF9 RID: 2809
					// (get) Token: 0x06003A5A RID: 14938 RVA: 0x000D48BB File Offset: 0x000D38BB
					public override AccessibleObject Parent
					{
						[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
						get
						{
							return this.parent;
						}
					}

					// Token: 0x17000AFA RID: 2810
					// (get) Token: 0x06003A5B RID: 14939 RVA: 0x000D48C3 File Offset: 0x000D38C3
					public override AccessibleRole Role
					{
						get
						{
							return AccessibleRole.PushButton;
						}
					}

					// Token: 0x04001D16 RID: 7446
					private bool up;

					// Token: 0x04001D17 RID: 7447
					private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject parent;
				}
			}
		}

		// Token: 0x020003BE RID: 958
		internal enum ButtonID
		{
			// Token: 0x04001D19 RID: 7449
			None,
			// Token: 0x04001D1A RID: 7450
			Up,
			// Token: 0x04001D1B RID: 7451
			Down
		}
	}
}
