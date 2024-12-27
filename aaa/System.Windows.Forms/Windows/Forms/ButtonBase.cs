using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000257 RID: 599
	[Designer("System.Windows.Forms.Design.ButtonBaseDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public abstract class ButtonBase : Control
	{
		// Token: 0x06001F3A RID: 7994 RVA: 0x00041A08 File Offset: 0x00040A08
		protected ButtonBase()
		{
			base.SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.StandardClick | ControlStyles.SupportsTransparentBackColor | ControlStyles.CacheText | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.UserMouse, this.OwnerDraw);
			this.SetFlag(128, true);
			this.SetFlag(256, false);
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x00041A85 File Offset: 0x00040A85
		// (set) Token: 0x06001F3C RID: 7996 RVA: 0x00041A8F File Offset: 0x00040A8F
		[SRDescription("ButtonAutoEllipsisDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(false)]
		public bool AutoEllipsis
		{
			get
			{
				return this.GetFlag(32);
			}
			set
			{
				if (this.AutoEllipsis != value)
				{
					this.SetFlag(32, value);
					if (value && this.textToolTip == null)
					{
						this.textToolTip = new ToolTip();
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x00041ABF File Offset: 0x00040ABF
		// (set) Token: 0x06001F3E RID: 7998 RVA: 0x00041AC7 File Offset: 0x00040AC7
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (value)
				{
					this.AutoEllipsis = false;
				}
			}
		}

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001F3F RID: 7999 RVA: 0x00041ADA File Offset: 0x00040ADA
		// (remove) Token: 0x06001F40 RID: 8000 RVA: 0x00041AE3 File Offset: 0x00040AE3
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
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

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x00041AEC File Offset: 0x00040AEC
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x00041AF4 File Offset: 0x00040AF4
		[SRDescription("ControlBackColorDescr")]
		[SRCategory("CatAppearance")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (base.DesignMode)
				{
					if (value != Color.Empty)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["UseVisualStyleBackColor"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(this, false);
						}
					}
				}
				else
				{
					this.UseVisualStyleBackColor = false;
				}
				base.BackColor = value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x00041B47 File Offset: 0x00040B47
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x00041B54 File Offset: 0x00040B54
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!this.OwnerDraw)
				{
					createParams.ExStyle &= -4097;
					createParams.Style |= 8192;
					if (this.IsDefault)
					{
						createParams.Style |= 1;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.TextAlign);
					if ((contentAlignment & WindowsFormsUtils.AnyLeftAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 256;
					}
					else if ((contentAlignment & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 512;
					}
					else
					{
						createParams.Style |= 768;
					}
					if ((contentAlignment & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 1024;
					}
					else if ((contentAlignment & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 2048;
					}
					else
					{
						createParams.Style |= 3072;
					}
				}
				return createParams;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x00041C53 File Offset: 0x00040C53
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x00041C56 File Offset: 0x00040C56
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x00041C60 File Offset: 0x00040C60
		protected internal bool IsDefault
		{
			get
			{
				return this.GetFlag(64);
			}
			set
			{
				if (this.GetFlag(64) != value)
				{
					this.SetFlag(64, value);
					if (base.IsHandleCreated)
					{
						if (this.OwnerDraw)
						{
							base.Invalidate();
							return;
						}
						base.UpdateStyles();
					}
				}
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x00041C93 File Offset: 0x00040C93
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x00041C9C File Offset: 0x00040C9C
		[DefaultValue(FlatStyle.Standard)]
		[Localizable(true)]
		[SRDescription("ButtonFlatStyleDescr")]
		[SRCategory("CatAppearance")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.flatStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				this.flatStyle = value;
				LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.FlatStyle);
				base.Invalidate();
				this.UpdateOwnerDraw();
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x00041CF9 File Offset: 0x00040CF9
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonFlatAppearance")]
		public FlatButtonAppearance FlatAppearance
		{
			get
			{
				if (this.flatAppearance == null)
				{
					this.flatAppearance = new FlatButtonAppearance(this);
				}
				return this.flatAppearance;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x00041D18 File Offset: 0x00040D18
		// (set) Token: 0x06001F4C RID: 8012 RVA: 0x00041D84 File Offset: 0x00040D84
		[SRDescription("ButtonImageDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public Image Image
		{
			get
			{
				if (this.image == null && this.imageList != null)
				{
					int num = this.imageIndex.ActualIndex;
					if (num >= this.imageList.Images.Count)
					{
						num = this.imageList.Images.Count - 1;
					}
					if (num >= 0)
					{
						return this.imageList.Images[num];
					}
				}
				return this.image;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					this.image = value;
					if (this.image != null)
					{
						this.ImageIndex = -1;
						this.ImageList = null;
					}
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Image);
					this.Animate();
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001F4D RID: 8013 RVA: 0x00041DE0 File Offset: 0x00040DE0
		// (set) Token: 0x06001F4E RID: 8014 RVA: 0x00041DE8 File Offset: 0x00040DE8
		[SRDescription("ButtonImageAlignDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		public ContentAlignment ImageAlign
		{
			get
			{
				return this.imageAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.imageAlign)
				{
					this.imageAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ImageAlign);
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x00041E40 File Offset: 0x00040E40
		// (set) Token: 0x06001F50 RID: 8016 RVA: 0x00041EA0 File Offset: 0x00040EA0
		[SRCategory("CatAppearance")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		public int ImageIndex
		{
			get
			{
				if (this.imageIndex.Index != -1 && this.imageList != null && this.imageIndex.Index >= this.imageList.Images.Count)
				{
					return this.imageList.Images.Count - 1;
				}
				return this.imageIndex.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						(-1).ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.imageIndex.Index != value)
				{
					if (value != -1)
					{
						this.image = null;
					}
					this.imageIndex.Index = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x00041F21 File Offset: 0x00040F21
		// (set) Token: 0x06001F52 RID: 8018 RVA: 0x00041F2E File Offset: 0x00040F2E
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string ImageKey
		{
			get
			{
				return this.imageIndex.Key;
			}
			set
			{
				if (this.imageIndex.Key != value)
				{
					if (value != null)
					{
						this.image = null;
					}
					this.imageIndex.Key = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x00041F5F File Offset: 0x00040F5F
		// (set) Token: 0x06001F54 RID: 8020 RVA: 0x00041F68 File Offset: 0x00040F68
		[SRDescription("ButtonImageListDescr")]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (this.imageList != value)
				{
					EventHandler eventHandler = new EventHandler(this.ImageListRecreateHandle);
					EventHandler eventHandler2 = new EventHandler(this.DetachImageList);
					if (this.imageList != null)
					{
						this.imageList.RecreateHandle -= eventHandler;
						this.imageList.Disposed -= eventHandler2;
					}
					if (value != null)
					{
						this.image = null;
					}
					this.imageList = value;
					this.imageIndex.ImageList = value;
					if (value != null)
					{
						value.RecreateHandle += eventHandler;
						value.Disposed += eventHandler2;
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x00041FEC File Offset: 0x00040FEC
		// (set) Token: 0x06001F56 RID: 8022 RVA: 0x00041FF4 File Offset: 0x00040FF4
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

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001F57 RID: 8023 RVA: 0x00041FFD File Offset: 0x00040FFD
		// (remove) Token: 0x06001F58 RID: 8024 RVA: 0x00042006 File Offset: 0x00041006
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x0004200F File Offset: 0x0004100F
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x00042012 File Offset: 0x00041012
		internal virtual Rectangle OverChangeRectangle
		{
			get
			{
				if (this.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.ClientRectangle;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x0004202D File Offset: 0x0004102D
		internal bool OwnerDraw
		{
			get
			{
				return this.FlatStyle != FlatStyle.System;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x0004203B File Offset: 0x0004103B
		internal virtual Rectangle DownChangeRectangle
		{
			get
			{
				return base.ClientRectangle;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x00042043 File Offset: 0x00041043
		internal bool MouseIsPressed
		{
			get
			{
				return this.GetFlag(4);
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x0004204C File Offset: 0x0004104C
		internal bool MouseIsDown
		{
			get
			{
				return this.GetFlag(2);
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x00042055 File Offset: 0x00041055
		internal bool MouseIsOver
		{
			get
			{
				return this.GetFlag(1);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x0004205E File Offset: 0x0004105E
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x0004206B File Offset: 0x0004106B
		internal bool ShowToolTip
		{
			get
			{
				return this.GetFlag(256);
			}
			set
			{
				this.SetFlag(256, value);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x00042079 File Offset: 0x00041079
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x00042081 File Offset: 0x00041081
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SettingsBindable(true)]
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

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x0004208A File Offset: 0x0004108A
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x00042094 File Offset: 0x00041094
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ButtonTextAlignDescr")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public virtual ContentAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.textAlign)
				{
					this.textAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.TextAlign);
					if (this.OwnerDraw)
					{
						base.Invalidate();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000420FB File Offset: 0x000410FB
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x00042104 File Offset: 0x00041104
		[SRDescription("ButtonTextImageRelationDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(TextImageRelation.Overlay)]
		[Localizable(true)]
		public TextImageRelation TextImageRelation
		{
			get
			{
				return this.textImageRelation;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidTextImageRelation(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TextImageRelation));
				}
				if (value != this.TextImageRelation)
				{
					this.textImageRelation = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.TextImageRelation);
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x0004215C File Offset: 0x0004115C
		// (set) Token: 0x06001F69 RID: 8041 RVA: 0x00042169 File Offset: 0x00041169
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("ButtonUseMnemonicDescr")]
		public bool UseMnemonic
		{
			get
			{
				return this.GetFlag(128);
			}
			set
			{
				this.SetFlag(128, value);
				LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text);
				base.Invalidate();
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x00042194 File Offset: 0x00041194
		private void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000421C3 File Offset: 0x000411C3
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000421CC File Offset: 0x000411CC
		private void Animate(bool animate)
		{
			if (animate != this.GetFlag(16))
			{
				if (animate)
				{
					if (this.image != null)
					{
						ImageAnimator.Animate(this.image, new EventHandler(this.OnFrameChanged));
						this.SetFlag(16, animate);
						return;
					}
				}
				else if (this.image != null)
				{
					ImageAnimator.StopAnimate(this.image, new EventHandler(this.OnFrameChanged));
					this.SetFlag(16, animate);
				}
			}
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00042238 File Offset: 0x00041238
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ButtonBase.ButtonBaseAccessibleObject(this);
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00042240 File Offset: 0x00041240
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0004224C File Offset: 0x0004124C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
				if (this.imageList != null)
				{
					this.imageList.Disposed -= this.DetachImageList;
				}
				if (this.textToolTip != null)
				{
					this.textToolTip.Dispose();
					this.textToolTip = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000422A2 File Offset: 0x000412A2
		private bool GetFlag(int flag)
		{
			return (this.state & flag) == flag;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000422AF File Offset: 0x000412AF
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.Invalidate();
			}
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000422BF File Offset: 0x000412BF
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000422CE File Offset: 0x000412CE
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.SetFlag(2, false);
			base.CaptureInternal = false;
			base.Invalidate();
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000422EC File Offset: 0x000412EC
		protected override void OnMouseEnter(EventArgs eventargs)
		{
			this.SetFlag(1, true);
			base.Invalidate();
			if (!base.DesignMode && this.AutoEllipsis && this.ShowToolTip && this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.textToolTip.Show(WindowsFormsUtils.TextWithoutMnemonics(this.Text), this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			base.OnMouseEnter(eventargs);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00042368 File Offset: 0x00041368
		protected override void OnMouseLeave(EventArgs eventargs)
		{
			this.SetFlag(1, false);
			if (this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.textToolTip.Hide(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			base.Invalidate();
			base.OnMouseLeave(eventargs);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000423C0 File Offset: 0x000413C0
		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			if (mevent.Button != MouseButtons.None && this.GetFlag(4))
			{
				if (!base.ClientRectangle.Contains(mevent.X, mevent.Y))
				{
					if (this.GetFlag(2))
					{
						this.SetFlag(2, false);
						base.Invalidate(this.DownChangeRectangle);
					}
				}
				else if (!this.GetFlag(2))
				{
					this.SetFlag(2, true);
					base.Invalidate(this.DownChangeRectangle);
				}
			}
			base.OnMouseMove(mevent);
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0004243D File Offset: 0x0004143D
		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left)
			{
				this.SetFlag(2, true);
				this.SetFlag(4, true);
				base.Invalidate(this.DownChangeRectangle);
			}
			base.OnMouseDown(mevent);
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0004246F File Offset: 0x0004146F
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00042478 File Offset: 0x00041478
		protected void ResetFlagsandPaint()
		{
			this.SetFlag(4, false);
			this.SetFlag(2, false);
			base.Invalidate(this.DownChangeRectangle);
			base.Update();
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0004249C File Offset: 0x0004149C
		private void PaintControl(PaintEventArgs pevent)
		{
			this.Adapter.Paint(pevent);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000424AA File Offset: 0x000414AA
		public override Size GetPreferredSize(Size proposedSize)
		{
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = 0;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = 0;
			}
			return base.GetPreferredSize(proposedSize);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000424D8 File Offset: 0x000414D8
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size preferredSizeCore = this.Adapter.GetPreferredSizeCore(proposedConstraints);
			return LayoutUtils.UnionSizes(preferredSizeCore + base.Padding.Size, this.MinimumSize);
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x00042514 File Offset: 0x00041514
		internal ButtonBaseAdapter Adapter
		{
			get
			{
				if (this._adapter == null || this.FlatStyle != this._cachedAdapterType)
				{
					switch (this.FlatStyle)
					{
					case FlatStyle.Flat:
						this._adapter = this.CreateFlatAdapter();
						break;
					case FlatStyle.Popup:
						this._adapter = this.CreatePopupAdapter();
						break;
					case FlatStyle.Standard:
						this._adapter = this.CreateStandardAdapter();
						break;
					}
					this._cachedAdapterType = this.FlatStyle;
				}
				return this._adapter;
			}
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0004258C File Offset: 0x0004158C
		internal virtual ButtonBaseAdapter CreateFlatAdapter()
		{
			return null;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0004258F File Offset: 0x0004158F
		internal virtual ButtonBaseAdapter CreatePopupAdapter()
		{
			return null;
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00042592 File Offset: 0x00041592
		internal virtual ButtonBaseAdapter CreateStandardAdapter()
		{
			return null;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x00042595 File Offset: 0x00041595
		internal virtual StringFormat CreateStringFormat()
		{
			if (this.Adapter == null)
			{
				return new StringFormat();
			}
			return this.Adapter.CreateStringFormat();
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000425B0 File Offset: 0x000415B0
		internal virtual TextFormatFlags CreateTextFormatFlags()
		{
			if (this.Adapter == null)
			{
				return TextFormatFlags.Default;
			}
			return this.Adapter.CreateTextFormatFlags();
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000425C8 File Offset: 0x000415C8
		private void OnFrameChanged(object o, EventArgs e)
		{
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (base.IsHandleCreated && base.InvokeRequired)
			{
				base.BeginInvoke(new EventHandler(this.OnFrameChanged), new object[] { o, e });
				return;
			}
			if (base.IsWindowObscured)
			{
				this.StopAnimate();
				return;
			}
			base.Invalidate();
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0004262F File Offset: 0x0004162F
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
			if (!base.Enabled)
			{
				this.SetFlag(2, false);
				this.SetFlag(1, false);
				base.Invalidate();
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0004265C File Offset: 0x0004165C
		protected override void OnTextChanged(EventArgs e)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
			{
				base.OnTextChanged(e);
				base.Invalidate();
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000426AC File Offset: 0x000416AC
		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if (kevent.KeyData == Keys.Space)
			{
				if (!this.GetFlag(2))
				{
					this.SetFlag(2, true);
					if (!this.OwnerDraw)
					{
						base.SendMessage(243, 1, 0);
					}
					base.Invalidate(this.DownChangeRectangle);
				}
				kevent.Handled = true;
			}
			base.OnKeyDown(kevent);
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x00042704 File Offset: 0x00041704
		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			if (this.GetFlag(2) && !base.ValidationCancelled)
			{
				if (this.OwnerDraw)
				{
					this.ResetFlagsandPaint();
				}
				else
				{
					this.SetFlag(4, false);
					this.SetFlag(2, false);
					base.SendMessage(243, 0, 0);
				}
				if (kevent.KeyCode == Keys.Return || kevent.KeyCode == Keys.Space)
				{
					this.OnClick(EventArgs.Empty);
				}
				kevent.Handled = true;
			}
			base.OnKeyUp(kevent);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x00042780 File Offset: 0x00041780
		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (this.AutoEllipsis)
			{
				Size preferredSize = base.PreferredSize;
				this.ShowToolTip = base.ClientRectangle.Width < preferredSize.Width || base.ClientRectangle.Height < preferredSize.Height;
			}
			else
			{
				this.ShowToolTip = false;
			}
			if (base.GetStyle(ControlStyles.UserPaint))
			{
				this.Animate();
				ImageAnimator.UpdateFrames(this.Image);
				this.PaintControl(pevent);
			}
			base.OnPaint(pevent);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x00042804 File Offset: 0x00041804
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Animate();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00042813 File Offset: 0x00041813
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00042822 File Offset: 0x00041822
		private void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0004282C File Offset: 0x0004182C
		private void SetFlag(int flag, bool value)
		{
			bool flag2 = (this.state & flag) != 0;
			if (value)
			{
				this.state |= flag;
			}
			else
			{
				this.state &= ~flag;
			}
			if (this.OwnerDraw && (flag & 2) != 0 && value != flag2)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00042887 File Offset: 0x00041887
		private bool ShouldSerializeImage()
		{
			return this.image != null;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x00042895 File Offset: 0x00041895
		private void UpdateOwnerDraw()
		{
			if (this.OwnerDraw != base.GetStyle(ControlStyles.UserPaint))
			{
				base.SetStyle(ControlStyles.UserPaint | ControlStyles.UserMouse, this.OwnerDraw);
				base.RecreateHandle();
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000428BD File Offset: 0x000418BD
		// (set) Token: 0x06001F90 RID: 8080 RVA: 0x000428C5 File Offset: 0x000418C5
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000428CE File Offset: 0x000418CE
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x000428D4 File Offset: 0x000418D4
		// (set) Token: 0x06001F93 RID: 8083 RVA: 0x00042913 File Offset: 0x00041913
		[SRDescription("ButtonUseVisualStyleBackColorDescr")]
		[SRCategory("CatAppearance")]
		public bool UseVisualStyleBackColor
		{
			get
			{
				return (this.isEnableVisualStyleBackgroundSet || (base.RawBackColor.IsEmpty && this.BackColor == SystemColors.Control)) && this.enableVisualStyleBackground;
			}
			set
			{
				this.isEnableVisualStyleBackgroundSet = true;
				this.enableVisualStyleBackground = value;
				base.Invalidate();
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00042929 File Offset: 0x00041929
		private void ResetUseVisualStyleBackColor()
		{
			this.isEnableVisualStyleBackgroundSet = false;
			this.enableVisualStyleBackground = true;
			base.Invalidate();
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0004293F File Offset: 0x0004193F
		private bool ShouldSerializeUseVisualStyleBackColor()
		{
			return this.isEnableVisualStyleBackgroundSet;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00042948 File Offset: 0x00041948
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 245)
			{
				if (this.OwnerDraw)
				{
					int msg2 = m.Msg;
					if (msg2 > 243)
					{
						if (msg2 <= 517)
						{
							if (msg2 != 514 && msg2 != 517)
							{
								goto IL_00E5;
							}
						}
						else if (msg2 != 520)
						{
							if (msg2 == 533)
							{
								goto IL_008C;
							}
							goto IL_00E5;
						}
						try
						{
							this.SetFlag(8, true);
							base.WndProc(ref m);
							return;
						}
						finally
						{
							this.SetFlag(8, false);
						}
						goto IL_00E5;
					}
					if (msg2 != 8 && msg2 != 31)
					{
						if (msg2 != 243)
						{
							goto IL_00E5;
						}
						return;
					}
					IL_008C:
					if (!this.GetFlag(8) && this.GetFlag(4))
					{
						this.SetFlag(4, false);
						if (this.GetFlag(2))
						{
							this.SetFlag(2, false);
							base.Invalidate(this.DownChangeRectangle);
						}
					}
					base.WndProc(ref m);
					return;
					IL_00E5:
					base.WndProc(ref m);
					return;
				}
				int msg3 = m.Msg;
				if (msg3 == 8465)
				{
					if (NativeMethods.Util.HIWORD(m.WParam) == 0 && !base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
				}
				return;
			}
			if (this is IButtonControl)
			{
				((IButtonControl)this).PerformClick();
				return;
			}
			this.OnClick(EventArgs.Empty);
		}

		// Token: 0x0400143C RID: 5180
		private const int FlagMouseOver = 1;

		// Token: 0x0400143D RID: 5181
		private const int FlagMouseDown = 2;

		// Token: 0x0400143E RID: 5182
		private const int FlagMousePressed = 4;

		// Token: 0x0400143F RID: 5183
		private const int FlagInButtonUp = 8;

		// Token: 0x04001440 RID: 5184
		private const int FlagCurrentlyAnimating = 16;

		// Token: 0x04001441 RID: 5185
		private const int FlagAutoEllipsis = 32;

		// Token: 0x04001442 RID: 5186
		private const int FlagIsDefault = 64;

		// Token: 0x04001443 RID: 5187
		private const int FlagUseMnemonic = 128;

		// Token: 0x04001444 RID: 5188
		private const int FlagShowToolTip = 256;

		// Token: 0x04001445 RID: 5189
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x04001446 RID: 5190
		private ContentAlignment imageAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04001447 RID: 5191
		private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04001448 RID: 5192
		private TextImageRelation textImageRelation;

		// Token: 0x04001449 RID: 5193
		private ImageList.Indexer imageIndex = new ImageList.Indexer();

		// Token: 0x0400144A RID: 5194
		private FlatButtonAppearance flatAppearance;

		// Token: 0x0400144B RID: 5195
		private ImageList imageList;

		// Token: 0x0400144C RID: 5196
		private Image image;

		// Token: 0x0400144D RID: 5197
		private int state;

		// Token: 0x0400144E RID: 5198
		private ToolTip textToolTip;

		// Token: 0x0400144F RID: 5199
		private bool enableVisualStyleBackground = true;

		// Token: 0x04001450 RID: 5200
		private bool isEnableVisualStyleBackgroundSet;

		// Token: 0x04001451 RID: 5201
		private ButtonBaseAdapter _adapter;

		// Token: 0x04001452 RID: 5202
		private FlatStyle _cachedAdapterType;

		// Token: 0x02000258 RID: 600
		[ComVisible(true)]
		public class ButtonBaseAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06001F97 RID: 8087 RVA: 0x00042A8C File Offset: 0x00041A8C
			public ButtonBaseAccessibleObject(Control owner)
				: base(owner)
			{
			}

			// Token: 0x06001F98 RID: 8088 RVA: 0x00042A95 File Offset: 0x00041A95
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				((ButtonBase)base.Owner).OnClick(EventArgs.Empty);
			}

			// Token: 0x17000481 RID: 1153
			// (get) Token: 0x06001F99 RID: 8089 RVA: 0x00042AAC File Offset: 0x00041AAC
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					ButtonBase buttonBase = (ButtonBase)base.Owner;
					if (buttonBase.OwnerDraw && buttonBase.MouseIsDown)
					{
						accessibleStates |= AccessibleStates.Pressed;
					}
					return accessibleStates;
				}
			}
		}
	}
}
