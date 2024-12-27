using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000461 RID: 1121
	[DefaultBindingProperty("Text")]
	[DefaultProperty("Text")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Designer("System.Windows.Forms.Design.LabelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionLabel")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class Label : Control
	{
		// Token: 0x060041E3 RID: 16867 RVA: 0x000EBA40 File Offset: 0x000EAA40
		public Label()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, this.IsOwnerDraw());
			base.SetStyle(ControlStyles.FixedHeight | ControlStyles.Selectable, false);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			CommonProperties.SetSelfAutoSizeInDefaultLayout(this, true);
			this.labelState[Label.StateFlatStyle] = 2;
			this.labelState[Label.StateUseMnemonic] = 1;
			this.labelState[Label.StateBorderStyle] = 0;
			this.TabStop = false;
			this.requestedHeight = base.Height;
			this.requestedWidth = base.Width;
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060041E4 RID: 16868 RVA: 0x000EBAEA File Offset: 0x000EAAEA
		// (set) Token: 0x060041E5 RID: 16869 RVA: 0x000EBAF2 File Offset: 0x000EAAF2
		[SRDescription("LabelAutoSizeDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				if (this.AutoSize != value)
				{
					base.AutoSize = value;
					this.AdjustSize();
				}
			}
		}

		// Token: 0x14000255 RID: 597
		// (add) Token: 0x060041E6 RID: 16870 RVA: 0x000EBB0A File Offset: 0x000EAB0A
		// (remove) Token: 0x060041E7 RID: 16871 RVA: 0x000EBB13 File Offset: 0x000EAB13
		[SRCategory("CatPropertyChanged")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
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

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x000EBB1C File Offset: 0x000EAB1C
		// (set) Token: 0x060041E9 RID: 16873 RVA: 0x000EBB34 File Offset: 0x000EAB34
		[SRDescription("LabelAutoEllipsisDescr")]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		public bool AutoEllipsis
		{
			get
			{
				return this.labelState[Label.StateAutoEllipsis] != 0;
			}
			set
			{
				if (this.AutoEllipsis != value)
				{
					this.labelState[Label.StateAutoEllipsis] = (value ? 1 : 0);
					this.MeasureTextCache.InvalidateCache();
					this.OnAutoEllipsisChanged();
					if (value && this.textToolTip == null)
					{
						this.textToolTip = new ToolTip();
					}
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.AutoEllipsis);
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x000EBBAD File Offset: 0x000EABAD
		// (set) Token: 0x060041EB RID: 16875 RVA: 0x000EBBB5 File Offset: 0x000EABB5
		[SRDescription("LabelBackgroundImageDescr")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SRCategory("CatAppearance")]
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

		// Token: 0x14000256 RID: 598
		// (add) Token: 0x060041EC RID: 16876 RVA: 0x000EBBBE File Offset: 0x000EABBE
		// (remove) Token: 0x060041ED RID: 16877 RVA: 0x000EBBC7 File Offset: 0x000EABC7
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

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060041EE RID: 16878 RVA: 0x000EBBD0 File Offset: 0x000EABD0
		// (set) Token: 0x060041EF RID: 16879 RVA: 0x000EBBD8 File Offset: 0x000EABD8
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

		// Token: 0x14000257 RID: 599
		// (add) Token: 0x060041F0 RID: 16880 RVA: 0x000EBBE1 File Offset: 0x000EABE1
		// (remove) Token: 0x060041F1 RID: 16881 RVA: 0x000EBBEA File Offset: 0x000EABEA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060041F2 RID: 16882 RVA: 0x000EBBF3 File Offset: 0x000EABF3
		// (set) Token: 0x060041F3 RID: 16883 RVA: 0x000EBC08 File Offset: 0x000EAC08
		[DispId(-504)]
		[DefaultValue(BorderStyle.None)]
		[SRDescription("LabelBorderDescr")]
		[SRCategory("CatAppearance")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				return (BorderStyle)this.labelState[Label.StateBorderStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.BorderStyle != value)
				{
					this.labelState[Label.StateBorderStyle] = (int)value;
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle);
					}
					if (this.AutoSize)
					{
						this.AdjustSize();
					}
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x000EBC88 File Offset: 0x000EAC88
		internal virtual bool CanUseTextRenderer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x000EBC8C File Offset: 0x000EAC8C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "STATIC";
				if (this.OwnerDraw)
				{
					createParams.Style |= 13;
					createParams.ExStyle &= -4097;
				}
				if (!this.OwnerDraw)
				{
					ContentAlignment textAlign = this.TextAlign;
					if (textAlign <= ContentAlignment.MiddleCenter)
					{
						switch (textAlign)
						{
						case ContentAlignment.TopLeft:
							break;
						case ContentAlignment.TopCenter:
							goto IL_00BD;
						case (ContentAlignment)3:
							goto IL_00D9;
						case ContentAlignment.TopRight:
							goto IL_00AD;
						default:
							if (textAlign != ContentAlignment.MiddleLeft)
							{
								if (textAlign != ContentAlignment.MiddleCenter)
								{
									goto IL_00D9;
								}
								goto IL_00BD;
							}
							break;
						}
					}
					else if (textAlign <= ContentAlignment.BottomLeft)
					{
						if (textAlign == ContentAlignment.MiddleRight)
						{
							goto IL_00AD;
						}
						if (textAlign != ContentAlignment.BottomLeft)
						{
							goto IL_00D9;
						}
					}
					else
					{
						if (textAlign == ContentAlignment.BottomCenter)
						{
							goto IL_00BD;
						}
						if (textAlign != ContentAlignment.BottomRight)
						{
							goto IL_00D9;
						}
						goto IL_00AD;
					}
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
					goto IL_00D9;
					IL_00AD:
					createParams.Style |= 2;
					goto IL_00D9;
					IL_00BD:
					createParams.Style |= 1;
				}
				else
				{
					CreateParams createParams3 = createParams;
					createParams3.Style = createParams3.Style;
				}
				IL_00D9:
				switch (this.BorderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.Style |= 4096;
					break;
				}
				if (!this.UseMnemonic)
				{
					createParams.Style |= 128;
				}
				return createParams;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x000EBDCC File Offset: 0x000EADCC
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x000EBDCF File Offset: 0x000EADCF
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(3, 0, 3, 0);
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x000EBDDA File Offset: 0x000EADDA
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, this.AutoSize ? this.PreferredHeight : 23);
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x000EBDF5 File Offset: 0x000EADF5
		// (set) Token: 0x060041FA RID: 16890 RVA: 0x000EBE08 File Offset: 0x000EAE08
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[SRDescription("ButtonFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return (FlatStyle)this.labelState[Label.StateFlatStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (this.labelState[Label.StateFlatStyle] != (int)value)
				{
					bool flag = this.labelState[Label.StateFlatStyle] == 3 || value == FlatStyle.System;
					this.labelState[Label.StateFlatStyle] = (int)value;
					base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, this.OwnerDraw);
					if (flag)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle);
						if (this.AutoSize)
						{
							this.AdjustSize();
						}
						base.RecreateHandle();
						return;
					}
					this.Refresh();
				}
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x000EBEC0 File Offset: 0x000EAEC0
		// (set) Token: 0x060041FC RID: 16892 RVA: 0x000EBF19 File Offset: 0x000EAF19
		[SRDescription("ButtonImageDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		public Image Image
		{
			get
			{
				Image image = (Image)base.Properties.GetObject(Label.PropImage);
				if (image == null && this.ImageList != null && this.ImageIndexer.ActualIndex >= 0)
				{
					return this.ImageList.Images[this.ImageIndexer.ActualIndex];
				}
				return image;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					base.Properties.SetObject(Label.PropImage, value);
					if (value != null)
					{
						this.ImageIndex = -1;
						this.ImageList = null;
					}
					this.Animate();
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x000EBF58 File Offset: 0x000EAF58
		// (set) Token: 0x060041FE RID: 16894 RVA: 0x000EBFAC File Offset: 0x000EAFAC
		[DefaultValue(-1)]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[SRCategory("CatAppearance")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(ImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				if (this.ImageIndexer == null)
				{
					return -1;
				}
				int index = this.ImageIndexer.Index;
				if (this.ImageList != null && index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return index;
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
				if (this.ImageIndex != value)
				{
					if (value != -1)
					{
						base.Properties.SetObject(Label.PropImage, null);
					}
					this.ImageIndexer.Index = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x000EC032 File Offset: 0x000EB032
		// (set) Token: 0x06004200 RID: 16896 RVA: 0x000EC049 File Offset: 0x000EB049
		[SRDescription("ButtonImageIndexDescr")]
		[DefaultValue("")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		public string ImageKey
		{
			get
			{
				if (this.ImageIndexer != null)
				{
					return this.ImageIndexer.Key;
				}
				return null;
			}
			set
			{
				if (this.ImageKey != value)
				{
					base.Properties.SetObject(Label.PropImage, null);
					this.ImageIndexer.Key = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x000EC07C File Offset: 0x000EB07C
		// (set) Token: 0x06004202 RID: 16898 RVA: 0x000EC0B6 File Offset: 0x000EB0B6
		internal LabelImageIndexer ImageIndexer
		{
			get
			{
				bool flag;
				LabelImageIndexer labelImageIndexer = base.Properties.GetObject(Label.PropImageIndex, out flag) as LabelImageIndexer;
				if (labelImageIndexer == null || !flag)
				{
					labelImageIndexer = new LabelImageIndexer(this);
					this.ImageIndexer = labelImageIndexer;
				}
				return labelImageIndexer;
			}
			set
			{
				base.Properties.SetObject(Label.PropImageIndex, value);
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x000EC0C9 File Offset: 0x000EB0C9
		// (set) Token: 0x06004204 RID: 16900 RVA: 0x000EC0E0 File Offset: 0x000EB0E0
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageListDescr")]
		public ImageList ImageList
		{
			get
			{
				return (ImageList)base.Properties.GetObject(Label.PropImageList);
			}
			set
			{
				if (this.ImageList != value)
				{
					EventHandler eventHandler = new EventHandler(this.ImageListRecreateHandle);
					EventHandler eventHandler2 = new EventHandler(this.DetachImageList);
					ImageList imageList = this.ImageList;
					if (imageList != null)
					{
						imageList.RecreateHandle -= eventHandler;
						imageList.Disposed -= eventHandler2;
					}
					if (value != null)
					{
						base.Properties.SetObject(Label.PropImage, null);
					}
					base.Properties.SetObject(Label.PropImageList, value);
					if (value != null)
					{
						value.RecreateHandle += eventHandler;
						value.Disposed += eventHandler2;
					}
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x000EC164 File Offset: 0x000EB164
		// (set) Token: 0x06004206 RID: 16902 RVA: 0x000EC18C File Offset: 0x000EB18C
		[SRDescription("ButtonImageAlignDescr")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		public ContentAlignment ImageAlign
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(Label.PropImageAlign, out flag);
				if (flag)
				{
					return (ContentAlignment)integer;
				}
				return ContentAlignment.MiddleCenter;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.ImageAlign)
				{
					base.Properties.SetInteger(Label.PropImageAlign, (int)value);
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ImageAlign);
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x000EC1EE File Offset: 0x000EB1EE
		// (set) Token: 0x06004208 RID: 16904 RVA: 0x000EC1F6 File Offset: 0x000EB1F6
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

		// Token: 0x14000258 RID: 600
		// (add) Token: 0x06004209 RID: 16905 RVA: 0x000EC1FF File Offset: 0x000EB1FF
		// (remove) Token: 0x0600420A RID: 16906 RVA: 0x000EC208 File Offset: 0x000EB208
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

		// Token: 0x14000259 RID: 601
		// (add) Token: 0x0600420B RID: 16907 RVA: 0x000EC211 File Offset: 0x000EB211
		// (remove) Token: 0x0600420C RID: 16908 RVA: 0x000EC21A File Offset: 0x000EB21A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x1400025A RID: 602
		// (add) Token: 0x0600420D RID: 16909 RVA: 0x000EC223 File Offset: 0x000EB223
		// (remove) Token: 0x0600420E RID: 16910 RVA: 0x000EC22C File Offset: 0x000EB22C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x1400025B RID: 603
		// (add) Token: 0x0600420F RID: 16911 RVA: 0x000EC235 File Offset: 0x000EB235
		// (remove) Token: 0x06004210 RID: 16912 RVA: 0x000EC23E File Offset: 0x000EB23E
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x000EC247 File Offset: 0x000EB247
		internal LayoutUtils.MeasureTextCache MeasureTextCache
		{
			get
			{
				if (this.textMeasurementCache == null)
				{
					this.textMeasurementCache = new LayoutUtils.MeasureTextCache();
				}
				return this.textMeasurementCache;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x000EC262 File Offset: 0x000EB262
		internal virtual bool OwnerDraw
		{
			get
			{
				return this.IsOwnerDraw();
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x000EC26C File Offset: 0x000EB26C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("LabelPreferredHeightDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatLayout")]
		public virtual int PreferredHeight
		{
			get
			{
				return base.PreferredSize.Height;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x000EC288 File Offset: 0x000EB288
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("LabelPreferredWidthDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatLayout")]
		public virtual int PreferredWidth
		{
			get
			{
				return base.PreferredSize.Width;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x000EC2A3 File Offset: 0x000EB2A3
		// (set) Token: 0x06004216 RID: 16918 RVA: 0x000EC2AB File Offset: 0x000EB2AB
		[Obsolete("This property has been deprecated. Use BackColor instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected new virtual bool RenderTransparent
		{
			get
			{
				return base.RenderTransparent;
			}
			set
			{
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x000EC2AD File Offset: 0x000EB2AD
		private bool SelfSizing
		{
			get
			{
				return CommonProperties.ShouldSelfSize(this);
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x000EC2B5 File Offset: 0x000EB2B5
		// (set) Token: 0x06004219 RID: 16921 RVA: 0x000EC2BD File Offset: 0x000EB2BD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1400025C RID: 604
		// (add) Token: 0x0600421A RID: 16922 RVA: 0x000EC2C6 File Offset: 0x000EB2C6
		// (remove) Token: 0x0600421B RID: 16923 RVA: 0x000EC2CF File Offset: 0x000EB2CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x000EC2D8 File Offset: 0x000EB2D8
		// (set) Token: 0x0600421D RID: 16925 RVA: 0x000EC300 File Offset: 0x000EB300
		[SRCategory("CatAppearance")]
		[SRDescription("LabelTextAlignDescr")]
		[Localizable(true)]
		[DefaultValue(ContentAlignment.TopLeft)]
		public virtual ContentAlignment TextAlign
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(Label.PropTextAlign, out flag);
				if (flag)
				{
					return (ContentAlignment)integer;
				}
				return ContentAlignment.TopLeft;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.TextAlign != value)
				{
					base.Properties.SetInteger(Label.PropTextAlign, (int)value);
					base.Invalidate();
					if (!this.OwnerDraw)
					{
						base.RecreateHandle();
					}
					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x000EC364 File Offset: 0x000EB364
		// (set) Token: 0x0600421F RID: 16927 RVA: 0x000EC36C File Offset: 0x000EB36C
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

		// Token: 0x1400025D RID: 605
		// (add) Token: 0x06004220 RID: 16928 RVA: 0x000EC375 File Offset: 0x000EB375
		// (remove) Token: 0x06004221 RID: 16929 RVA: 0x000EC388 File Offset: 0x000EB388
		[SRDescription("LabelOnTextAlignChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(Label.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Label.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x000EC39B File Offset: 0x000EB39B
		// (set) Token: 0x06004223 RID: 16931 RVA: 0x000EC3AD File Offset: 0x000EB3AD
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return !this.CanUseTextRenderer || base.UseCompatibleTextRenderingInt;
			}
			set
			{
				if (base.UseCompatibleTextRenderingInt != value)
				{
					base.UseCompatibleTextRenderingInt = value;
					this.AdjustSize();
				}
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x000EC3C5 File Offset: 0x000EB3C5
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004225 RID: 16933 RVA: 0x000EC3C8 File Offset: 0x000EB3C8
		// (set) Token: 0x06004226 RID: 16934 RVA: 0x000EC3E0 File Offset: 0x000EB3E0
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("LabelUseMnemonicDescr")]
		public bool UseMnemonic
		{
			get
			{
				return this.labelState[Label.StateUseMnemonic] != 0;
			}
			set
			{
				if (this.UseMnemonic != value)
				{
					this.labelState[Label.StateUseMnemonic] = (value ? 1 : 0);
					this.MeasureTextCache.InvalidateCache();
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
					{
						this.AdjustSize();
						base.Invalidate();
					}
					if (base.IsHandleCreated)
					{
						int num = base.WindowStyle;
						if (!this.UseMnemonic)
						{
							num |= 128;
						}
						else
						{
							num &= -129;
						}
						base.WindowStyle = num;
					}
				}
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x000EC48C File Offset: 0x000EB48C
		internal void AdjustSize()
		{
			if (!this.SelfSizing)
			{
				return;
			}
			if (!this.AutoSize && ((this.Anchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right) || (this.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom)))
			{
				return;
			}
			int num = this.requestedHeight;
			int num2 = this.requestedWidth;
			try
			{
				Size size = (this.AutoSize ? base.PreferredSize : new Size(num2, num));
				base.Size = size;
			}
			finally
			{
				this.requestedHeight = num;
				this.requestedWidth = num2;
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000EC514 File Offset: 0x000EB514
		internal void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x000EC543 File Offset: 0x000EB543
		internal void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x000EC54C File Offset: 0x000EB54C
		private void Animate(bool animate)
		{
			bool flag = this.labelState[Label.StateAnimating] != 0;
			if (animate != flag)
			{
				Image image = (Image)base.Properties.GetObject(Label.PropImage);
				if (animate)
				{
					if (image != null)
					{
						ImageAnimator.Animate(image, new EventHandler(this.OnFrameChanged));
						this.labelState[Label.StateAnimating] = (animate ? 1 : 0);
						return;
					}
				}
				else if (image != null)
				{
					ImageAnimator.StopAnimate(image, new EventHandler(this.OnFrameChanged));
					this.labelState[Label.StateAnimating] = (animate ? 1 : 0);
				}
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x000EC5E8 File Offset: 0x000EB5E8
		protected Rectangle CalcImageRenderBounds(Image image, Rectangle r, ContentAlignment align)
		{
			Size size = image.Size;
			int num = r.X + 2;
			int num2 = r.Y + 2;
			if ((align & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
			{
				num = r.X + r.Width - 4 - size.Width;
			}
			else if ((align & WindowsFormsUtils.AnyCenterAlign) != (ContentAlignment)0)
			{
				num = r.X + (r.Width - size.Width) / 2;
			}
			if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
			{
				num2 = r.Y + r.Height - 4 - size.Height;
			}
			else if ((align & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
			{
				num2 = r.Y + 2;
			}
			else
			{
				num2 = r.Y + (r.Height - size.Height) / 2;
			}
			return new Rectangle(num, num2, size.Width, size.Height);
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x000EC6C1 File Offset: 0x000EB6C1
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new Label.LabelAccessibleObject(this);
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x000EC6C9 File Offset: 0x000EB6C9
		internal virtual StringFormat CreateStringFormat()
		{
			return ControlPaint.CreateStringFormat(this, this.TextAlign, this.AutoEllipsis, this.UseMnemonic);
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x000EC6E3 File Offset: 0x000EB6E3
		private TextFormatFlags CreateTextFormatFlags()
		{
			return this.CreateTextFormatFlags(base.Size - this.GetBordersAndPadding());
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x000EC6FC File Offset: 0x000EB6FC
		internal virtual TextFormatFlags CreateTextFormatFlags(Size constrainingSize)
		{
			TextFormatFlags textFormatFlags = ControlPaint.CreateTextFormatFlags(this, this.TextAlign, this.AutoEllipsis, this.UseMnemonic);
			if (!this.MeasureTextCache.TextRequiresWordBreak(this.Text, this.Font, constrainingSize, textFormatFlags))
			{
				textFormatFlags &= ~(TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
			}
			return textFormatFlags;
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x000EC746 File Offset: 0x000EB746
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x000EC750 File Offset: 0x000EB750
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
				if (this.ImageList != null)
				{
					this.ImageList.Disposed -= this.DetachImageList;
					this.ImageList.RecreateHandle -= this.ImageListRecreateHandle;
					base.Properties.SetObject(Label.PropImageList, null);
				}
				if (this.Image != null)
				{
					base.Properties.SetObject(Label.PropImage, null);
				}
				if (this.textToolTip != null)
				{
					this.textToolTip.Dispose();
					this.textToolTip = null;
				}
				this.controlToolTip = false;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x000EC7F4 File Offset: 0x000EB7F4
		protected void DrawImage(Graphics g, Image image, Rectangle r, ContentAlignment align)
		{
			Rectangle rectangle = this.CalcImageRenderBounds(image, r, align);
			if (!base.Enabled)
			{
				ControlPaint.DrawImageDisabled(g, image, rectangle.X, rectangle.Y, this.BackColor);
				return;
			}
			g.DrawImage(image, rectangle.X, rectangle.Y, image.Width, image.Height);
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x000EC854 File Offset: 0x000EB854
		private Size GetBordersAndPadding()
		{
			Size size = base.Padding.Size;
			if (this.UseCompatibleTextRendering)
			{
				if (this.BorderStyle != BorderStyle.None)
				{
					size.Height += 6;
					size.Width += 2;
				}
				else
				{
					size.Height += 3;
				}
			}
			else
			{
				size += this.SizeFromClientSize(Size.Empty);
				if (this.BorderStyle == BorderStyle.Fixed3D)
				{
					size += new Size(2, 2);
				}
			}
			return size;
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x000EC8DB File Offset: 0x000EB8DB
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

		// Token: 0x06004235 RID: 16949 RVA: 0x000EC908 File Offset: 0x000EB908
		internal virtual bool UseGDIMeasuring()
		{
			return this.FlatStyle == FlatStyle.System || !this.UseCompatibleTextRendering;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x000EC920 File Offset: 0x000EB920
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size bordersAndPadding = this.GetBordersAndPadding();
			proposedConstraints -= bordersAndPadding;
			proposedConstraints = LayoutUtils.UnionSizes(proposedConstraints, Size.Empty);
			Size size;
			if (string.IsNullOrEmpty(this.Text))
			{
				using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
				{
					size = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent("0", windowsFont);
					size.Width = 0;
					goto IL_0113;
				}
			}
			if (this.UseGDIMeasuring())
			{
				TextFormatFlags textFormatFlags = ((this.FlatStyle == FlatStyle.System) ? TextFormatFlags.Default : this.CreateTextFormatFlags(proposedConstraints));
				size = this.MeasureTextCache.GetTextSize(this.Text, this.Font, proposedConstraints, textFormatFlags);
			}
			else
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					using (StringFormat stringFormat = this.CreateStringFormat())
					{
						SizeF sizeF = ((proposedConstraints.Width == 1) ? new SizeF(0f, (float)proposedConstraints.Height) : new SizeF((float)proposedConstraints.Width, (float)proposedConstraints.Height));
						size = Size.Ceiling(graphics.MeasureString(this.Text, this.Font, sizeF, stringFormat));
					}
				}
			}
			IL_0113:
			size += bordersAndPadding;
			return size;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x000ECA74 File Offset: 0x000EBA74
		private int GetLeadingTextPaddingFromTextFormatFlags()
		{
			if (!base.IsHandleCreated)
			{
				return 0;
			}
			if (this.UseCompatibleTextRendering && this.FlatStyle != FlatStyle.System)
			{
				return 0;
			}
			int iLeftMargin;
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHwnd(base.Handle))
			{
				TextFormatFlags textFormatFlags = this.CreateTextFormatFlags();
				if ((textFormatFlags & TextFormatFlags.NoPadding) == TextFormatFlags.NoPadding)
				{
					windowsGraphics.TextPadding = TextPaddingOptions.NoPadding;
				}
				else if ((textFormatFlags & TextFormatFlags.LeftAndRightPadding) == TextFormatFlags.LeftAndRightPadding)
				{
					windowsGraphics.TextPadding = TextPaddingOptions.LeftAndRightPadding;
				}
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
				{
					IntNativeMethods.DRAWTEXTPARAMS textMargins = windowsGraphics.GetTextMargins(windowsFont);
					iLeftMargin = textMargins.iLeftMargin;
				}
			}
			return iLeftMargin;
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x000ECB30 File Offset: 0x000EBB30
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.Invalidate();
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x000ECB40 File Offset: 0x000EBB40
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x000ECB43 File Offset: 0x000EBB43
		internal bool IsOwnerDraw()
		{
			return this.FlatStyle != FlatStyle.System;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x000ECB54 File Offset: 0x000EBB54
		protected override void OnMouseEnter(EventArgs e)
		{
			if (!this.controlToolTip && !base.DesignMode && this.AutoEllipsis && this.showToolTip && this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.controlToolTip = true;
					this.textToolTip.Show(WindowsFormsUtils.TextWithoutMnemonics(this.Text), this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					this.controlToolTip = false;
				}
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000ECBD8 File Offset: 0x000EBBD8
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.controlToolTip && this.textToolTip != null && this.textToolTip.GetHandleCreated())
			{
				this.textToolTip.RemoveAll();
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
			base.OnMouseLeave(e);
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x000ECC44 File Offset: 0x000EBC44
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
			base.Invalidate();
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x000ECC9C File Offset: 0x000EBC9C
		protected override void OnFontChanged(EventArgs e)
		{
			this.MeasureTextCache.InvalidateCache();
			base.OnFontChanged(e);
			this.AdjustSize();
			base.Invalidate();
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x000ECCBC File Offset: 0x000EBCBC
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (this.textToolTip != null && this.textToolTip.GetHandleCreated())
			{
				this.textToolTip.DestroyHandle();
			}
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x000ECCE8 File Offset: 0x000EBCE8
		protected override void OnTextChanged(EventArgs e)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
			{
				this.MeasureTextCache.InvalidateCache();
				base.OnTextChanged(e);
				this.AdjustSize();
				base.Invalidate();
			}
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x000ECD48 File Offset: 0x000EBD48
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Label.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x000ECD76 File Offset: 0x000EBD76
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.AdjustSize();
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x000ECD88 File Offset: 0x000EBD88
		protected override void OnPaint(PaintEventArgs e)
		{
			this.Animate();
			ImageAnimator.UpdateFrames(this.Image);
			Rectangle rectangle = LayoutUtils.DeflateRect(base.ClientRectangle, base.Padding);
			Image image = this.Image;
			if (image != null)
			{
				this.DrawImage(e.Graphics, image, rectangle, base.RtlTranslateAlignment(this.ImageAlign));
			}
			IntPtr hdc = e.Graphics.GetHdc();
			Color nearestColor;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					nearestColor = windowsGraphics.GetNearestColor(base.Enabled ? this.ForeColor : base.DisabledColor);
				}
			}
			finally
			{
				e.Graphics.ReleaseHdc();
			}
			if (this.AutoEllipsis)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				Size preferredSize = this.GetPreferredSize(new Size(clientRectangle.Width, clientRectangle.Height));
				this.showToolTip = clientRectangle.Width < preferredSize.Width || clientRectangle.Height < preferredSize.Height;
			}
			else
			{
				this.showToolTip = false;
			}
			if (this.UseCompatibleTextRendering)
			{
				using (StringFormat stringFormat = this.CreateStringFormat())
				{
					if (base.Enabled)
					{
						using (Brush brush = new SolidBrush(nearestColor))
						{
							e.Graphics.DrawString(this.Text, this.Font, brush, rectangle, stringFormat);
							goto IL_0161;
						}
					}
					ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, nearestColor, rectangle, stringFormat);
					IL_0161:
					goto IL_01C5;
				}
			}
			TextFormatFlags textFormatFlags = this.CreateTextFormatFlags();
			if (base.Enabled)
			{
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rectangle, nearestColor, textFormatFlags);
			}
			else
			{
				Color color = TextRenderer.DisabledTextColor(this.BackColor);
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rectangle, color, textFormatFlags);
			}
			IL_01C5:
			base.OnPaint(e);
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x000ECF98 File Offset: 0x000EBF98
		internal virtual void OnAutoEllipsisChanged()
		{
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x000ECF9A File Offset: 0x000EBF9A
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x000ECFA9 File Offset: 0x000EBFA9
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (this.SelfSizing)
			{
				this.AdjustSize();
			}
			this.Animate();
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x000ECFC6 File Offset: 0x000EBFC6
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			this.MeasureTextCache.InvalidateCache();
			base.OnRightToLeftChanged(e);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x000ECFDA File Offset: 0x000EBFDA
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x000ECFEC File Offset: 0x000EBFEC
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			base.PrintToMetaFileRecursive(hDC, lParam, bounds);
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
				{
					ControlPaint.PrintBorder(graphics, new Rectangle(Point.Empty, base.Size), this.BorderStyle, Border3DStyle.SunkenOuter);
				}
			}
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x000ED070 File Offset: 0x000EC070
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			/*
An exception occurred when decompiling this method (0600424A)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Windows.Forms.Label::ProcessMnemonic(System.Char)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 81
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 65
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x000ED0E8 File Offset: 0x000EC0E8
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				this.requestedHeight = height;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				this.requestedWidth = width;
			}
			if (this.AutoSize && this.SelfSizing)
			{
				Size preferredSize = base.PreferredSize;
				width = preferredSize.Width;
				height = preferredSize.Height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x000ED146 File Offset: 0x000EC146
		private void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x000ED14F File Offset: 0x000EC14F
		private bool ShouldSerializeImage()
		{
			return base.Properties.GetObject(Label.PropImage) != null;
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x000ED167 File Offset: 0x000EC167
		internal void SetToolTip(ToolTip toolTip)
		{
			if (toolTip != null && !this.controlToolTip)
			{
				this.controlToolTip = true;
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x000ED17C File Offset: 0x000EC17C
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", Text: " + this.Text;
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x000ED1A4 File Offset: 0x000EC1A4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 132)
			{
				Rectangle rectangle = base.RectangleToScreen(new Rectangle(0, 0, base.Width, base.Height));
				Point point = new Point((int)(long)m.LParam);
				m.Result = (IntPtr)(rectangle.Contains(point) ? 1 : 0);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04002083 RID: 8323
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x04002084 RID: 8324
		private static readonly BitVector32.Section StateUseMnemonic = BitVector32.CreateSection(1);

		// Token: 0x04002085 RID: 8325
		private static readonly BitVector32.Section StateAutoSize = BitVector32.CreateSection(1, Label.StateUseMnemonic);

		// Token: 0x04002086 RID: 8326
		private static readonly BitVector32.Section StateAnimating = BitVector32.CreateSection(1, Label.StateAutoSize);

		// Token: 0x04002087 RID: 8327
		private static readonly BitVector32.Section StateFlatStyle = BitVector32.CreateSection(3, Label.StateAnimating);

		// Token: 0x04002088 RID: 8328
		private static readonly BitVector32.Section StateBorderStyle = BitVector32.CreateSection(2, Label.StateFlatStyle);

		// Token: 0x04002089 RID: 8329
		private static readonly BitVector32.Section StateAutoEllipsis = BitVector32.CreateSection(1, Label.StateBorderStyle);

		// Token: 0x0400208A RID: 8330
		private static readonly int PropImageList = PropertyStore.CreateKey();

		// Token: 0x0400208B RID: 8331
		private static readonly int PropImage = PropertyStore.CreateKey();

		// Token: 0x0400208C RID: 8332
		private static readonly int PropTextAlign = PropertyStore.CreateKey();

		// Token: 0x0400208D RID: 8333
		private static readonly int PropImageAlign = PropertyStore.CreateKey();

		// Token: 0x0400208E RID: 8334
		private static readonly int PropImageIndex = PropertyStore.CreateKey();

		// Token: 0x0400208F RID: 8335
		private BitVector32 labelState = default(BitVector32);

		// Token: 0x04002090 RID: 8336
		private int requestedHeight;

		// Token: 0x04002091 RID: 8337
		private int requestedWidth;

		// Token: 0x04002092 RID: 8338
		private LayoutUtils.MeasureTextCache textMeasurementCache;

		// Token: 0x04002093 RID: 8339
		internal bool showToolTip;

		// Token: 0x04002094 RID: 8340
		private ToolTip textToolTip;

		// Token: 0x04002095 RID: 8341
		private bool controlToolTip;

		// Token: 0x02000462 RID: 1122
		[ComVisible(true)]
		internal class LabelAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06004252 RID: 16978 RVA: 0x000ED2B4 File Offset: 0x000EC2B4
			public LabelAccessibleObject(Label owner)
				: base(owner)
			{
			}

			// Token: 0x17000CEE RID: 3310
			// (get) Token: 0x06004253 RID: 16979 RVA: 0x000ED2C0 File Offset: 0x000EC2C0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.StaticText;
				}
			}
		}
	}
}
