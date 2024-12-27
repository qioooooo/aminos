using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000248 RID: 584
	[Designer("System.Windows.Forms.Design.ToolStripItemDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	[DefaultEvent("Click")]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public abstract class ToolStripItem : Component, IDropTarget, ISupportOleDropSource, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x0003AE34 File Offset: 0x00039E34
		protected ToolStripItem()
		{
			this.state[ToolStripItem.stateEnabled | ToolStripItem.stateAutoSize | ToolStripItem.stateVisible | ToolStripItem.stateContstructing | ToolStripItem.stateSupportsItemClick | ToolStripItem.stateInvalidMirroredImage | ToolStripItem.stateMouseDownAndUpMustBeInSameItem | ToolStripItem.stateUseAmbientMargin] = true;
			this.state[ToolStripItem.stateAllowDrop | ToolStripItem.stateMouseDownAndNoDrag | ToolStripItem.stateSupportsRightClick | ToolStripItem.statePressed | ToolStripItem.stateSelected | ToolStripItem.stateDisposed | ToolStripItem.stateDoubleClickEnabled | ToolStripItem.stateRightToLeftAutoMirrorImage | ToolStripItem.stateSupportsSpaceKey] = false;
			this.SetAmbientMargin();
			this.Size = this.DefaultSize;
			this.DisplayStyle = this.DefaultDisplayStyle;
			CommonProperties.SetAutoSize(this, true);
			this.state[ToolStripItem.stateContstructing] = false;
			this.AutoToolTip = this.DefaultAutoToolTip;
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0003AF65 File Offset: 0x00039F65
		protected ToolStripItem(string text, Image image, EventHandler onClick)
			: this(text, image, onClick, null)
		{
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0003AF71 File Offset: 0x00039F71
		protected ToolStripItem(string text, Image image, EventHandler onClick, string name)
			: this()
		{
			this.Text = text;
			this.Image = image;
			if (onClick != null)
			{
				this.Click += onClick;
			}
			this.Name = name;
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x0003AF9C File Offset: 0x00039F9C
		[SRDescription("ToolStripItemAccessibilityObjectDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(ToolStripItem.PropAccessibility);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					this.Properties.SetObject(ToolStripItem.PropAccessibility, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0003AFDB File Offset: 0x00039FDB
		// (set) Token: 0x06001CEF RID: 7407 RVA: 0x0003AFF2 File Offset: 0x00039FF2
		[SRCategory("CatAccessibility")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolStripItemAccessibleDefaultActionDescr")]
		[Browsable(false)]
		public string AccessibleDefaultActionDescription
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleDefaultActionDescription);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleDefaultActionDescription, value);
				this.OnAccessibleDefaultActionDescriptionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0003B010 File Offset: 0x0003A010
		// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x0003B027 File Offset: 0x0003A027
		[SRDescription("ToolStripItemAccessibleDescriptionDescr")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRCategory("CatAccessibility")]
		public string AccessibleDescription
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleDescription);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleDescription, value);
				this.OnAccessibleDescriptionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0003B045 File Offset: 0x0003A045
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x0003B05C File Offset: 0x0003A05C
		[SRDescription("ToolStripItemAccessibleNameDescr")]
		[SRCategory("CatAccessibility")]
		[DefaultValue(null)]
		[Localizable(true)]
		public string AccessibleName
		{
			get
			{
				return (string)this.Properties.GetObject(ToolStripItem.PropAccessibleName);
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropAccessibleName, value);
				this.OnAccessibleNameChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x0003B07C File Offset: 0x0003A07C
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0003B0A4 File Offset: 0x0003A0A4
		[SRCategory("CatAccessibility")]
		[DefaultValue(AccessibleRole.Default)]
		[SRDescription("ToolStripItemAccessibleRoleDescr")]
		public AccessibleRole AccessibleRole
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropAccessibleRole, out flag);
				if (flag)
				{
					return (AccessibleRole)integer;
				}
				return AccessibleRole.Default;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 64))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AccessibleRole));
				}
				this.Properties.SetInteger(ToolStripItem.PropAccessibleRole, (int)value);
				this.OnAccessibleRoleChanged(EventArgs.Empty);
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0003B0F4 File Offset: 0x0003A0F4
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x0003B0FC File Offset: 0x0003A0FC
		[SRDescription("ToolStripItemAlignmentDescr")]
		[DefaultValue(ToolStripItemAlignment.Left)]
		[SRCategory("CatLayout")]
		public ToolStripItemAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemAlignment));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					if (this.ParentInternal != null && this.ParentInternal.IsHandleCreated)
					{
						this.ParentInternal.PerformLayout();
					}
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0003B15F File Offset: 0x0003A15F
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0003B171 File Offset: 0x0003A171
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ToolStripItemAllowDropDescr")]
		[DefaultValue(false)]
		[SRCategory("CatDragDrop")]
		[Browsable(false)]
		public virtual bool AllowDrop
		{
			get
			{
				return this.state[ToolStripItem.stateAllowDrop];
			}
			set
			{
				if (value != this.state[ToolStripItem.stateAllowDrop])
				{
					this.EnsureParentDropTargetRegistered();
					this.state[ToolStripItem.stateAllowDrop] = value;
				}
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0003B19D File Offset: 0x0003A19D
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x0003B1AF File Offset: 0x0003A1AF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[SRDescription("ToolStripItemAutoSizeDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool AutoSize
		{
			get
			{
				return this.state[ToolStripItem.stateAutoSize];
			}
			set
			{
				if (this.state[ToolStripItem.stateAutoSize] != value)
				{
					this.state[ToolStripItem.stateAutoSize] = value;
					CommonProperties.SetAutoSize(this, value);
					this.InvalidateItemLayout(PropertyNames.AutoSize);
				}
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0003B1E7 File Offset: 0x0003A1E7
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0003B1F9 File Offset: 0x0003A1F9
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ToolStripItemAutoToolTipDescr")]
		public bool AutoToolTip
		{
			get
			{
				return this.state[ToolStripItem.stateAutoToolTip];
			}
			set
			{
				this.state[ToolStripItem.stateAutoToolTip] = value;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0003B20C File Offset: 0x0003A20C
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x0003B21E File Offset: 0x0003A21E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ToolStripItemAvailableDescr")]
		public bool Available
		{
			get
			{
				return this.state[ToolStripItem.stateVisible];
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06001D00 RID: 7424 RVA: 0x0003B227 File Offset: 0x0003A227
		// (remove) Token: 0x06001D01 RID: 7425 RVA: 0x0003B23A File Offset: 0x0003A23A
		[Browsable(false)]
		[SRDescription("ToolStripItemOnAvailableChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler AvailableChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventAvailableChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventAvailableChanged, value);
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0003B24D File Offset: 0x0003A24D
		// (set) Token: 0x06001D03 RID: 7427 RVA: 0x0003B264 File Offset: 0x0003A264
		[DefaultValue(null)]
		[SRDescription("ToolStripItemImageDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		public virtual Image BackgroundImage
		{
			get
			{
				return this.Properties.GetObject(ToolStripItem.PropBackgroundImage) as Image;
			}
			set
			{
				if (this.BackgroundImage != value)
				{
					this.Properties.SetObject(ToolStripItem.PropBackgroundImage, value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0003B288 File Offset: 0x0003A288
		// (set) Token: 0x06001D05 RID: 7429 RVA: 0x0003B2C0 File Offset: 0x0003A2C0
		[DefaultValue(ImageLayout.Tile)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		public virtual ImageLayout BackgroundImageLayout
		{
			get
			{
				if (!this.Properties.ContainsObject(ToolStripItem.PropBackgroundImageLayout))
				{
					return ImageLayout.Tile;
				}
				return (ImageLayout)this.Properties.GetObject(ToolStripItem.PropBackgroundImageLayout);
			}
			set
			{
				if (this.BackgroundImageLayout != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ImageLayout));
					}
					this.Properties.SetObject(ToolStripItem.PropBackgroundImageLayout, value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0003B318 File Offset: 0x0003A318
		// (set) Token: 0x06001D07 RID: 7431 RVA: 0x0003B350 File Offset: 0x0003A350
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemBackColorDescr")]
		public virtual Color BackColor
		{
			get
			{
				Color rawBackColor = this.RawBackColor;
				if (!rawBackColor.IsEmpty)
				{
					return rawBackColor;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.BackColor;
				}
				return Control.DefaultBackColor;
			}
			set
			{
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(ToolStripItem.PropBackColor))
				{
					this.Properties.SetColor(ToolStripItem.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnBackColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06001D08 RID: 7432 RVA: 0x0003B3B5 File Offset: 0x0003A3B5
		// (remove) Token: 0x06001D09 RID: 7433 RVA: 0x0003B3C8 File Offset: 0x0003A3C8
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnBackColorChangedDescr")]
		public event EventHandler BackColorChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventBackColorChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventBackColorChanged, value);
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x0003B3DB File Offset: 0x0003A3DB
		[Browsable(false)]
		public virtual Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x0003B3E4 File Offset: 0x0003A3E4
		internal Rectangle ClientBounds
		{
			get
			{
				Rectangle rectangle = this.bounds;
				rectangle.Location = Point.Empty;
				return rectangle;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x0003B408 File Offset: 0x0003A408
		[Browsable(false)]
		public Rectangle ContentRectangle
		{
			get
			{
				Rectangle rectangle = LayoutUtils.InflateRect(this.InternalLayout.ContentRectangle, this.Padding);
				rectangle.Size = LayoutUtils.UnionSizes(Size.Empty, rectangle.Size);
				return rectangle;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0003B445 File Offset: 0x0003A445
		[Browsable(false)]
		public virtual bool CanSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x0003B448 File Offset: 0x0003A448
		internal virtual bool CanKeyboardSelect
		{
			get
			{
				return this.CanSelect;
			}
		}

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06001D0F RID: 7439 RVA: 0x0003B450 File Offset: 0x0003A450
		// (remove) Token: 0x06001D10 RID: 7440 RVA: 0x0003B463 File Offset: 0x0003A463
		[SRDescription("ToolStripItemOnClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventClick, value);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0003B476 File Offset: 0x0003A476
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x0003B47E File Offset: 0x0003A47E
		[DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AnchorStyles Anchor
		{
			get
			{
				return CommonProperties.xGetAnchor(this);
			}
			set
			{
				if (value != this.Anchor)
				{
					CommonProperties.xSetAnchor(this, value);
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(this, this.ParentInternal, PropertyNames.Anchor);
					}
				}
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x0003B4A9 File Offset: 0x0003A4A9
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x0003B4B4 File Offset: 0x0003A4B4
		[Browsable(false)]
		[DefaultValue(DockStyle.None)]
		public DockStyle Dock
		{
			get
			{
				return CommonProperties.xGetDock(this);
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DockStyle));
				}
				if (value != this.Dock)
				{
					CommonProperties.xSetDock(this, value);
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(this, this.ParentInternal, PropertyNames.Dock);
					}
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0003B510 File Offset: 0x0003A510
		protected virtual bool DefaultAutoToolTip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0003B513 File Offset: 0x0003A513
		protected internal virtual Padding DefaultMargin
		{
			get
			{
				if (this.Owner != null && this.Owner is StatusStrip)
				{
					return new Padding(0, 2, 0, 0);
				}
				return new Padding(0, 1, 0, 2);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0003B53D File Offset: 0x0003A53D
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0003B544 File Offset: 0x0003A544
		protected virtual Size DefaultSize
		{
			get
			{
				return new Size(23, 23);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0003B54F File Offset: 0x0003A54F
		protected virtual ToolStripItemDisplayStyle DefaultDisplayStyle
		{
			get
			{
				return ToolStripItemDisplayStyle.ImageAndText;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x0003B552 File Offset: 0x0003A552
		protected internal virtual bool DismissWhenClicked
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x0003B555 File Offset: 0x0003A555
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x0003B560 File Offset: 0x0003A560
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemDisplayStyleDescr")]
		public virtual ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return this.displayStyle;
			}
			set
			{
				if (this.displayStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemDisplayStyle));
					}
					this.displayStyle = value;
					if (!this.state[ToolStripItem.stateContstructing])
					{
						this.InvalidateItemLayout(PropertyNames.DisplayStyle);
						this.OnDisplayStyleChanged(new EventArgs());
					}
				}
			}
		}

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06001D1D RID: 7453 RVA: 0x0003B5CB File Offset: 0x0003A5CB
		// (remove) Token: 0x06001D1E RID: 7454 RVA: 0x0003B5DE File Offset: 0x0003A5DE
		public event EventHandler DisplayStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x0003B5F1 File Offset: 0x0003A5F1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		private RightToLeft DefaultRightToLeft
		{
			get
			{
				return RightToLeft.Inherit;
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06001D20 RID: 7456 RVA: 0x0003B5F4 File Offset: 0x0003A5F4
		// (remove) Token: 0x06001D21 RID: 7457 RVA: 0x0003B607 File Offset: 0x0003A607
		[SRDescription("ControlOnDoubleClickDescr")]
		[SRCategory("CatAction")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDoubleClick, value);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001D22 RID: 7458 RVA: 0x0003B61A File Offset: 0x0003A61A
		// (set) Token: 0x06001D23 RID: 7459 RVA: 0x0003B62C File Offset: 0x0003A62C
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripItemDoubleClickedEnabledDescr")]
		[DefaultValue(false)]
		public bool DoubleClickEnabled
		{
			get
			{
				return this.state[ToolStripItem.stateDoubleClickEnabled];
			}
			set
			{
				this.state[ToolStripItem.stateDoubleClickEnabled] = value;
			}
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06001D24 RID: 7460 RVA: 0x0003B63F File Offset: 0x0003A63F
		// (remove) Token: 0x06001D25 RID: 7461 RVA: 0x0003B652 File Offset: 0x0003A652
		[Browsable(false)]
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragDropDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event DragEventHandler DragDrop
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragDrop, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragDrop, value);
			}
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06001D26 RID: 7462 RVA: 0x0003B665 File Offset: 0x0003A665
		// (remove) Token: 0x06001D27 RID: 7463 RVA: 0x0003B678 File Offset: 0x0003A678
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragEnterDescr")]
		public event DragEventHandler DragEnter
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragEnter, value);
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06001D28 RID: 7464 RVA: 0x0003B68B File Offset: 0x0003A68B
		// (remove) Token: 0x06001D29 RID: 7465 RVA: 0x0003B69E File Offset: 0x0003A69E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ToolStripItemOnDragOverDescr")]
		[SRCategory("CatDragDrop")]
		[Browsable(false)]
		public event DragEventHandler DragOver
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragOver, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragOver, value);
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06001D2A RID: 7466 RVA: 0x0003B6B1 File Offset: 0x0003A6B1
		// (remove) Token: 0x06001D2B RID: 7467 RVA: 0x0003B6C4 File Offset: 0x0003A6C4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatDragDrop")]
		[SRDescription("ToolStripItemOnDragLeaveDescr")]
		public event EventHandler DragLeave
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDragLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDragLeave, value);
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x0003B6D7 File Offset: 0x0003A6D7
		private DropSource DropSource
		{
			get
			{
				if (this.ParentInternal != null && this.ParentInternal.AllowItemReorder && this.ParentInternal.ItemReorderDropSource != null)
				{
					return new DropSource(this.ParentInternal.ItemReorderDropSource);
				}
				return new DropSource(this);
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x0003B714 File Offset: 0x0003A714
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x0003B74C File Offset: 0x0003A74C
		[SRDescription("ToolStripItemEnabledDescr")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public virtual bool Enabled
		{
			get
			{
				bool flag = true;
				if (this.Owner != null)
				{
					flag = this.Owner.Enabled;
				}
				return this.state[ToolStripItem.stateEnabled] && flag;
			}
			set
			{
				if (this.state[ToolStripItem.stateEnabled] != value)
				{
					this.state[ToolStripItem.stateEnabled] = value;
					if (!this.state[ToolStripItem.stateEnabled])
					{
						this.state[ToolStripItem.stateSelected | ToolStripItem.statePressed] = false;
					}
					this.OnEnabledChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06001D2F RID: 7471 RVA: 0x0003B7B7 File Offset: 0x0003A7B7
		// (remove) Token: 0x06001D30 RID: 7472 RVA: 0x0003B7CA File Offset: 0x0003A7CA
		[SRDescription("ToolStripItemEnabledChangedDescr")]
		public event EventHandler EnabledChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventEnabledChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventEnabledChanged, value);
			}
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0003B7DD File Offset: 0x0003A7DD
		private void EnsureParentDropTargetRegistered()
		{
			if (this.ParentInternal != null)
			{
				IntSecurity.ClipboardRead.Demand();
				this.ParentInternal.DropTargetManager.EnsureRegistered(this);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x0003B804 File Offset: 0x0003A804
		// (set) Token: 0x06001D33 RID: 7475 RVA: 0x0003B844 File Offset: 0x0003A844
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemForeColorDescr")]
		public virtual Color ForeColor
		{
			get
			{
				Color color = this.Properties.GetColor(ToolStripItem.PropForeColor);
				if (!color.IsEmpty)
				{
					return color;
				}
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					return parentInternal.ForeColor;
				}
				return Control.DefaultForeColor;
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(ToolStripItem.PropForeColor))
				{
					this.Properties.SetColor(ToolStripItem.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnForeColorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06001D34 RID: 7476 RVA: 0x0003B8A9 File Offset: 0x0003A8A9
		// (remove) Token: 0x06001D35 RID: 7477 RVA: 0x0003B8BC File Offset: 0x0003A8BC
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnForeColorChangedDescr")]
		public event EventHandler ForeColorChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventForeColorChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventForeColorChanged, value);
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x0003B8D0 File Offset: 0x0003A8D0
		// (set) Token: 0x06001D37 RID: 7479 RVA: 0x0003B90C File Offset: 0x0003A90C
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemFontDescr")]
		public virtual Font Font
		{
			get
			{
				Font font = (Font)this.Properties.GetObject(ToolStripItem.PropFont);
				if (font != null)
				{
					return font;
				}
				Font ownerFont = this.GetOwnerFont();
				if (ownerFont != null)
				{
					return ownerFont;
				}
				return ToolStripManager.DefaultFont;
			}
			set
			{
				Font font = (Font)this.Properties.GetObject(ToolStripItem.PropFont);
				if (font != value)
				{
					this.Properties.SetObject(ToolStripItem.PropFont, value);
					this.OnFontChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06001D38 RID: 7480 RVA: 0x0003B94F File Offset: 0x0003A94F
		// (remove) Token: 0x06001D39 RID: 7481 RVA: 0x0003B962 File Offset: 0x0003A962
		[SRDescription("ToolStripItemOnGiveFeedbackDescr")]
		[SRCategory("CatDragDrop")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventGiveFeedback, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventGiveFeedback, value);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0003B978 File Offset: 0x0003A978
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0003B994 File Offset: 0x0003A994
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public int Height
		{
			get
			{
				return this.Bounds.Height;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				this.SetBounds(rectangle.X, rectangle.Y, rectangle.Width, value);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x0003B9C4 File Offset: 0x0003A9C4
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return ToolStripItem.EmptyChildCollection;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x0003B9CB File Offset: 0x0003A9CB
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				if (this.ParentInternal == null)
				{
					return this.Owner;
				}
				return this.ParentInternal;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0003B9E2 File Offset: 0x0003A9E2
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001D3F RID: 7487 RVA: 0x0003B9EA File Offset: 0x0003A9EA
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.state[ToolStripItem.stateVisible];
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x0003B9FC File Offset: 0x0003A9FC
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0003BA04 File Offset: 0x0003AA04
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBounds(bounds);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0003BA0D File Offset: 0x0003AA0D
		void IArrangedElement.PerformLayout(IArrangedElement container, string propertyName)
		{
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001D43 RID: 7491 RVA: 0x0003BA0F File Offset: 0x0003AA0F
		// (set) Token: 0x06001D44 RID: 7492 RVA: 0x0003BA17 File Offset: 0x0003AA17
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageAlignDescr")]
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
				if (this.imageAlign != value)
				{
					this.imageAlign = value;
					this.InvalidateItemLayout(PropertyNames.ImageAlign);
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x0003BA54 File Offset: 0x0003AA54
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x0003BB10 File Offset: 0x0003AB10
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolStripItemImageDescr")]
		public virtual Image Image
		{
			get
			{
				Image image = (Image)this.Properties.GetObject(ToolStripItem.PropImage);
				if (image != null || this.Owner == null || this.Owner.ImageList == null || this.ImageIndexer.ActualIndex < 0)
				{
					return image;
				}
				if (this.ImageIndexer.ActualIndex < this.Owner.ImageList.Images.Count)
				{
					image = this.Owner.ImageList.Images[this.ImageIndexer.ActualIndex];
					this.state[ToolStripItem.stateInvalidMirroredImage] = true;
					this.Properties.SetObject(ToolStripItem.PropImage, image);
					return image;
				}
				return null;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					Bitmap bitmap = value as Bitmap;
					if (bitmap != null && this.ImageTransparentColor != Color.Empty)
					{
						if (bitmap.RawFormat.Guid != ImageFormat.Icon.Guid && !ImageAnimator.CanAnimate(bitmap))
						{
							bitmap.MakeTransparent(this.ImageTransparentColor);
						}
						value = bitmap;
					}
					if (value != null)
					{
						this.ImageIndex = -1;
					}
					this.Properties.SetObject(ToolStripItem.PropImage, value);
					this.state[ToolStripItem.stateInvalidMirroredImage] = true;
					this.Animate();
					this.InvalidateItemLayout(PropertyNames.Image);
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x0003BBBB File Offset: 0x0003ABBB
		// (set) Token: 0x06001D48 RID: 7496 RVA: 0x0003BBC4 File Offset: 0x0003ABC4
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageTransparentColorDescr")]
		[Localizable(true)]
		public Color ImageTransparentColor
		{
			get
			{
				return this.imageTransparentColor;
			}
			set
			{
				if (this.imageTransparentColor != value)
				{
					this.imageTransparentColor = value;
					Bitmap bitmap = this.Image as Bitmap;
					if (bitmap != null && value != Color.Empty && bitmap.RawFormat.Guid != ImageFormat.Icon.Guid && !ImageAnimator.CanAnimate(bitmap))
					{
						bitmap.MakeTransparent(this.imageTransparentColor);
					}
					this.Invalidate();
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x0003BC38 File Offset: 0x0003AC38
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x0003BCB0 File Offset: 0x0003ACB0
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("Owner.ImageList")]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripItemImageIndexDescr")]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ToolStripImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				if (this.Owner != null && this.ImageIndexer.Index != -1 && this.Owner.ImageList != null && this.ImageIndexer.Index >= this.Owner.ImageList.Images.Count)
				{
					return this.Owner.ImageList.Images.Count - 1;
				}
				return this.ImageIndexer.Index;
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
				this.ImageIndexer.Index = value;
				this.state[ToolStripItem.stateInvalidMirroredImage] = true;
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.ImageIndex);
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x0003BD3F File Offset: 0x0003AD3F
		internal ToolStripItemImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ToolStripItemImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x0003BD5B File Offset: 0x0003AD5B
		// (set) Token: 0x06001D4D RID: 7501 RVA: 0x0003BD68 File Offset: 0x0003AD68
		[TypeConverter(typeof(ImageKeyConverter))]
		[SRDescription("ToolStripItemImageKeyDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Browsable(false)]
		[RelatedImageList("Owner.ImageList")]
		[Editor("System.Windows.Forms.Design.ToolStripImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				this.state[ToolStripItem.stateInvalidMirroredImage] = true;
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.ImageKey);
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x0003BDA3 File Offset: 0x0003ADA3
		// (set) Token: 0x06001D4F RID: 7503 RVA: 0x0003BDAC File Offset: 0x0003ADAC
		[Localizable(true)]
		[DefaultValue(ToolStripItemImageScaling.SizeToFit)]
		[SRDescription("ToolStripItemImageScalingDescr")]
		[SRCategory("CatAppearance")]
		public ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return this.imageScaling;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripItemImageScaling));
				}
				if (this.imageScaling != value)
				{
					this.imageScaling = value;
					this.InvalidateItemLayout(PropertyNames.ImageScaling);
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x0003BDFA File Offset: 0x0003ADFA
		internal ToolStripItemInternalLayout InternalLayout
		{
			get
			{
				if (this.toolStripItemInternalLayout == null)
				{
					this.toolStripItemInternalLayout = this.CreateInternalLayout();
				}
				return this.toolStripItemInternalLayout;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x0003BE18 File Offset: 0x0003AE18
		internal bool IsForeColorSet
		{
			get
			{
				if (!this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty)
				{
					return true;
				}
				Control parentInternal = this.ParentInternal;
				return parentInternal != null && parentInternal.ShouldSerializeForeColor();
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x0003BE53 File Offset: 0x0003AE53
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x0003BE5B File Offset: 0x0003AE5B
		[Browsable(false)]
		public bool IsDisposed
		{
			get
			{
				return this.state[ToolStripItem.stateDisposed];
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001D54 RID: 7508 RVA: 0x0003BE6D File Offset: 0x0003AE6D
		[Browsable(false)]
		public bool IsOnDropDown
		{
			get
			{
				if (this.ParentInternal != null)
				{
					return this.ParentInternal.IsDropDown;
				}
				return this.Owner != null && this.Owner.IsDropDown;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x0003BE9B File Offset: 0x0003AE9B
		[Browsable(false)]
		public bool IsOnOverflow
		{
			get
			{
				return this.Placement == ToolStripItemPlacement.Overflow;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x0003BEA6 File Offset: 0x0003AEA6
		internal virtual bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06001D57 RID: 7511 RVA: 0x0003BEA9 File Offset: 0x0003AEA9
		// (remove) Token: 0x06001D58 RID: 7512 RVA: 0x0003BEBC File Offset: 0x0003AEBC
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripItemOnLocationChangedDescr")]
		public event EventHandler LocationChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventLocationChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventLocationChanged, value);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x0003BECF File Offset: 0x0003AECF
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x0003BED7 File Offset: 0x0003AED7
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripItemMarginDescr")]
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				if (this.Margin != value)
				{
					this.state[ToolStripItem.stateUseAmbientMargin] = false;
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x0003BF00 File Offset: 0x0003AF00
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x0003BF26 File Offset: 0x0003AF26
		[SRDescription("ToolStripMergeActionDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(MergeAction.Append)]
		public MergeAction MergeAction
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropMergeAction, out flag);
				if (flag)
				{
					return (MergeAction)integer;
				}
				return MergeAction.Append;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MergeAction));
				}
				this.Properties.SetInteger(ToolStripItem.PropMergeAction, (int)value);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x0003BF60 File Offset: 0x0003AF60
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x0003BF86 File Offset: 0x0003AF86
		[DefaultValue(-1)]
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripMergeIndexDescr")]
		public int MergeIndex
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(ToolStripItem.PropMergeIndex, out flag);
				if (flag)
				{
					return integer;
				}
				return -1;
			}
			set
			{
				this.Properties.SetInteger(ToolStripItem.PropMergeIndex, value);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x0003BF99 File Offset: 0x0003AF99
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x0003BFAB File Offset: 0x0003AFAB
		internal bool MouseDownAndUpMustBeInSameItem
		{
			get
			{
				return this.state[ToolStripItem.stateMouseDownAndUpMustBeInSameItem];
			}
			set
			{
				this.state[ToolStripItem.stateMouseDownAndUpMustBeInSameItem] = value;
			}
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06001D61 RID: 7521 RVA: 0x0003BFBE File Offset: 0x0003AFBE
		// (remove) Token: 0x06001D62 RID: 7522 RVA: 0x0003BFD1 File Offset: 0x0003AFD1
		[SRDescription("ToolStripItemOnMouseDownDescr")]
		[SRCategory("CatMouse")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseDown, value);
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06001D63 RID: 7523 RVA: 0x0003BFE4 File Offset: 0x0003AFE4
		// (remove) Token: 0x06001D64 RID: 7524 RVA: 0x0003BFF7 File Offset: 0x0003AFF7
		[SRDescription("ToolStripItemOnMouseEnterDescr")]
		[SRCategory("CatMouse")]
		public event EventHandler MouseEnter
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseEnter, value);
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06001D65 RID: 7525 RVA: 0x0003C00A File Offset: 0x0003B00A
		// (remove) Token: 0x06001D66 RID: 7526 RVA: 0x0003C01D File Offset: 0x0003B01D
		[SRCategory("CatMouse")]
		[SRDescription("ToolStripItemOnMouseLeaveDescr")]
		public event EventHandler MouseLeave
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseLeave, value);
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06001D67 RID: 7527 RVA: 0x0003C030 File Offset: 0x0003B030
		// (remove) Token: 0x06001D68 RID: 7528 RVA: 0x0003C043 File Offset: 0x0003B043
		[SRDescription("ToolStripItemOnMouseHoverDescr")]
		[SRCategory("CatMouse")]
		public event EventHandler MouseHover
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseHover, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseHover, value);
			}
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06001D69 RID: 7529 RVA: 0x0003C056 File Offset: 0x0003B056
		// (remove) Token: 0x06001D6A RID: 7530 RVA: 0x0003C069 File Offset: 0x0003B069
		[SRDescription("ToolStripItemOnMouseMoveDescr")]
		[SRCategory("CatMouse")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseMove, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseMove, value);
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06001D6B RID: 7531 RVA: 0x0003C07C File Offset: 0x0003B07C
		// (remove) Token: 0x06001D6C RID: 7532 RVA: 0x0003C08F File Offset: 0x0003B08F
		[SRDescription("ToolStripItemOnMouseUpDescr")]
		[SRCategory("CatMouse")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventMouseUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventMouseUp, value);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0003C0A2 File Offset: 0x0003B0A2
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x0003C0BF File Offset: 0x0003B0BF
		[DefaultValue(null)]
		[Browsable(false)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, (string)this.Properties.GetObject(ToolStripItem.PropName));
			}
			set
			{
				if (base.DesignMode)
				{
					return;
				}
				this.Properties.SetObject(ToolStripItem.PropName, value);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0003C0DB File Offset: 0x0003B0DB
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x0003C0E3 File Offset: 0x0003B0E3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStrip Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				if (this.owner != value)
				{
					if (this.owner != null)
					{
						this.owner.Items.Remove(this);
					}
					if (value != null)
					{
						value.Items.Add(this);
					}
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x0003C118 File Offset: 0x0003B118
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripItem OwnerItem
		{
			get
			{
				ToolStripDropDown toolStripDropDown = null;
				if (this.ParentInternal != null)
				{
					toolStripDropDown = this.ParentInternal as ToolStripDropDown;
				}
				else if (this.Owner != null)
				{
					toolStripDropDown = this.Owner as ToolStripDropDown;
				}
				if (toolStripDropDown != null)
				{
					return toolStripDropDown.OwnerItem;
				}
				return null;
			}
		}

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06001D72 RID: 7538 RVA: 0x0003C15C File Offset: 0x0003B15C
		// (remove) Token: 0x06001D73 RID: 7539 RVA: 0x0003C16F File Offset: 0x0003B16F
		[SRDescription("ToolStripItemOwnerChangedDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler OwnerChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventOwnerChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventOwnerChanged, value);
			}
		}

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06001D74 RID: 7540 RVA: 0x0003C182 File Offset: 0x0003B182
		// (remove) Token: 0x06001D75 RID: 7541 RVA: 0x0003C195 File Offset: 0x0003B195
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemOnPaintDescr")]
		public event PaintEventHandler Paint
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventPaint, value);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x0003C1A8 File Offset: 0x0003B1A8
		// (set) Token: 0x06001D77 RID: 7543 RVA: 0x0003C1B0 File Offset: 0x0003B1B0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		protected internal ToolStrip Parent
		{
			get
			{
				return this.ParentInternal;
			}
			set
			{
				this.ParentInternal = value;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x0003C1B9 File Offset: 0x0003B1B9
		// (set) Token: 0x06001D79 RID: 7545 RVA: 0x0003C1C4 File Offset: 0x0003B1C4
		[SRCategory("CatLayout")]
		[DefaultValue(ToolStripItemOverflow.AsNeeded)]
		[SRDescription("ToolStripItemOverflowDescr")]
		public ToolStripItemOverflow Overflow
		{
			get
			{
				return this.overflow;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripGripStyle));
				}
				if (this.overflow != value)
				{
					this.overflow = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, "Overflow");
					}
				}
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0003C225 File Offset: 0x0003B225
		// (set) Token: 0x06001D7B RID: 7547 RVA: 0x0003C233 File Offset: 0x0003B233
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripItemPaddingDescr")]
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
					this.InvalidateItemLayout(PropertyNames.Padding);
				}
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x0003C255 File Offset: 0x0003B255
		// (set) Token: 0x06001D7D RID: 7549 RVA: 0x0003C260 File Offset: 0x0003B260
		internal ToolStrip ParentInternal
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					ToolStrip toolStrip = this.parent;
					this.parent = value;
					this.OnParentChanged(toolStrip, value);
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x0003C28C File Offset: 0x0003B28C
		[Browsable(false)]
		public ToolStripItemPlacement Placement
		{
			get
			{
				return this.placement;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x0003C294 File Offset: 0x0003B294
		internal Size PreferredImageSize
		{
			get
			{
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) != ToolStripItemDisplayStyle.Image)
				{
					return Size.Empty;
				}
				Image image = (Image)this.Properties.GetObject(ToolStripItem.PropImage);
				bool flag = this.Owner != null && this.Owner.ImageList != null && this.ImageIndexer.ActualIndex >= 0;
				if (this.ImageScaling == ToolStripItemImageScaling.SizeToFit)
				{
					ToolStrip toolStrip = this.Owner;
					if (toolStrip != null && (image != null || flag))
					{
						return toolStrip.ImageScalingSize;
					}
				}
				Size size = Size.Empty;
				if (flag)
				{
					size = this.Owner.ImageList.ImageSize;
				}
				else
				{
					size = ((image == null) ? Size.Empty : image.Size);
				}
				return size;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x0003C340 File Offset: 0x0003B340
		internal PropertyStore Properties
		{
			get
			{
				if (this.propertyStore == null)
				{
					this.propertyStore = new PropertyStore();
				}
				return this.propertyStore;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0003C35B File Offset: 0x0003B35B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool Pressed
		{
			get
			{
				return this.CanSelect && this.state[ToolStripItem.statePressed];
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06001D82 RID: 7554 RVA: 0x0003C377 File Offset: 0x0003B377
		// (remove) Token: 0x06001D83 RID: 7555 RVA: 0x0003C38A File Offset: 0x0003B38A
		[Browsable(false)]
		[SRDescription("ToolStripItemOnQueryContinueDragDescr")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatDragDrop")]
		public event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventQueryContinueDrag, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventQueryContinueDrag, value);
			}
		}

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06001D84 RID: 7556 RVA: 0x0003C39D File Offset: 0x0003B39D
		// (remove) Token: 0x06001D85 RID: 7557 RVA: 0x0003C3B0 File Offset: 0x0003B3B0
		[SRDescription("ToolStripItemOnQueryAccessibilityHelpDescr")]
		[SRCategory("CatBehavior")]
		public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventQueryAccessibilityHelp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventQueryAccessibilityHelp, value);
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x0003C3C3 File Offset: 0x0003B3C3
		internal Color RawBackColor
		{
			get
			{
				return this.Properties.GetColor(ToolStripItem.PropBackColor);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x0003C3D5 File Offset: 0x0003B3D5
		internal ToolStripRenderer Renderer
		{
			get
			{
				if (this.Owner != null)
				{
					return this.Owner.Renderer;
				}
				if (this.ParentInternal == null)
				{
					return null;
				}
				return this.ParentInternal.Renderer;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0003C400 File Offset: 0x0003B400
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x0003C460 File Offset: 0x0003B460
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemRightToLeftDescr")]
		[Localizable(true)]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				bool flag;
				int num = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2)
				{
					if (this.Owner != null)
					{
						num = (int)this.Owner.RightToLeft;
					}
					else if (this.ParentInternal != null)
					{
						num = (int)this.ParentInternal.RightToLeft;
					}
					else
					{
						num = (int)this.DefaultRightToLeft;
					}
				}
				return (RightToLeft)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				RightToLeft rightToLeft = this.RightToLeft;
				if (this.Properties.ContainsInteger(ToolStripItem.PropRightToLeft) || value != RightToLeft.Inherit)
				{
					this.Properties.SetInteger(ToolStripItem.PropRightToLeft, (int)value);
				}
				if (rightToLeft != this.RightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0003C4D5 File Offset: 0x0003B4D5
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x0003C4E7 File Offset: 0x0003B4E7
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemRightToLeftAutoMirrorImageDescr")]
		[DefaultValue(false)]
		public bool RightToLeftAutoMirrorImage
		{
			get
			{
				return this.state[ToolStripItem.stateRightToLeftAutoMirrorImage];
			}
			set
			{
				if (this.state[ToolStripItem.stateRightToLeftAutoMirrorImage] != value)
				{
					this.state[ToolStripItem.stateRightToLeftAutoMirrorImage] = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0003C514 File Offset: 0x0003B514
		internal Image MirroredImage
		{
			get
			{
				if (!this.state[ToolStripItem.stateInvalidMirroredImage])
				{
					return this.Properties.GetObject(ToolStripItem.PropMirroredImage) as Image;
				}
				Image image = this.Image;
				if (image != null)
				{
					Image image2 = image.Clone() as Image;
					image2.RotateFlip(RotateFlipType.RotateNoneFlipX);
					this.Properties.SetObject(ToolStripItem.PropMirroredImage, image2);
					this.state[ToolStripItem.stateInvalidMirroredImage] = false;
					return image2;
				}
				return null;
			}
		}

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06001D8D RID: 7565 RVA: 0x0003C58B File Offset: 0x0003B58B
		// (remove) Token: 0x06001D8E RID: 7566 RVA: 0x0003C59E File Offset: 0x0003B59E
		[SRDescription("ToolStripItemOnRightToLeftChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventRightToLeft, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventRightToLeft, value);
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x0003C5B4 File Offset: 0x0003B5B4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return this.CanSelect && !base.DesignMode && (this.state[ToolStripItem.stateSelected] || (this.ParentInternal != null && this.ParentInternal.IsSelectionSuspended && this.ParentInternal.LastMouseDownedItem == this));
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0003C60C File Offset: 0x0003B60C
		protected internal virtual bool ShowKeyboardCues
		{
			get
			{
				return base.DesignMode || ToolStripManager.ShowMenuFocusCues;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0003C620 File Offset: 0x0003B620
		// (set) Token: 0x06001D92 RID: 7570 RVA: 0x0003C63C File Offset: 0x0003B63C
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("ToolStripItemSizeDescr")]
		public virtual Size Size
		{
			get
			{
				return this.Bounds.Size;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				rectangle.Size = value;
				this.SetBounds(rectangle);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0003C65F File Offset: 0x0003B65F
		// (set) Token: 0x06001D94 RID: 7572 RVA: 0x0003C671 File Offset: 0x0003B671
		internal bool SupportsRightClick
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsRightClick];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsRightClick] = value;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x0003C684 File Offset: 0x0003B684
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x0003C696 File Offset: 0x0003B696
		internal bool SupportsItemClick
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsItemClick];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsItemClick] = value;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x0003C6A9 File Offset: 0x0003B6A9
		// (set) Token: 0x06001D98 RID: 7576 RVA: 0x0003C6BB File Offset: 0x0003B6BB
		internal bool SupportsSpaceKey
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsSpaceKey];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsSpaceKey] = value;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x0003C6CE File Offset: 0x0003B6CE
		// (set) Token: 0x06001D9A RID: 7578 RVA: 0x0003C6E0 File Offset: 0x0003B6E0
		internal bool SupportsDisabledHotTracking
		{
			get
			{
				return this.state[ToolStripItem.stateSupportsDisabledHotTracking];
			}
			set
			{
				this.state[ToolStripItem.stateSupportsDisabledHotTracking] = value;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001D9B RID: 7579 RVA: 0x0003C6F3 File Offset: 0x0003B6F3
		// (set) Token: 0x06001D9C RID: 7580 RVA: 0x0003C719 File Offset: 0x0003B719
		[SRCategory("CatData")]
		[Localizable(false)]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[Bindable(true)]
		[SRDescription("ToolStripItemTagDescr")]
		public object Tag
		{
			get
			{
				if (this.Properties.ContainsObject(ToolStripItem.PropTag))
				{
					return this.propertyStore.GetObject(ToolStripItem.PropTag);
				}
				return null;
			}
			set
			{
				this.Properties.SetObject(ToolStripItem.PropTag, value);
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0003C72C File Offset: 0x0003B72C
		// (set) Token: 0x06001D9E RID: 7582 RVA: 0x0003C75B File Offset: 0x0003B75B
		[SRDescription("ToolStripItemTextDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		public virtual string Text
		{
			get
			{
				if (this.Properties.ContainsObject(ToolStripItem.PropText))
				{
					return (string)this.Properties.GetObject(ToolStripItem.PropText);
				}
				return "";
			}
			set
			{
				if (value != this.Text)
				{
					this.Properties.SetObject(ToolStripItem.PropText, value);
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0003C787 File Offset: 0x0003B787
		// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x0003C78F File Offset: 0x0003B78F
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemTextAlignDescr")]
		[Localizable(true)]
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
				if (this.textAlign != value)
				{
					this.textAlign = value;
					this.InvalidateItemLayout(PropertyNames.TextAlign);
				}
			}
		}

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06001DA1 RID: 7585 RVA: 0x0003C7CA File Offset: 0x0003B7CA
		// (remove) Token: 0x06001DA2 RID: 7586 RVA: 0x0003C7DD File Offset: 0x0003B7DD
		[SRDescription("ToolStripItemOnTextChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler TextChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventText, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventText, value);
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0003C7F0 File Offset: 0x0003B7F0
		// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x0003C858 File Offset: 0x0003B858
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripTextDirectionDescr")]
		public virtual ToolStripTextDirection TextDirection
		{
			get
			{
				ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
				if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
				{
					toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
				}
				if (toolStripTextDirection == ToolStripTextDirection.Inherit)
				{
					if (this.ParentInternal != null)
					{
						toolStripTextDirection = this.ParentInternal.TextDirection;
					}
					else
					{
						toolStripTextDirection = ((this.Owner == null) ? ToolStripTextDirection.Horizontal : this.Owner.TextDirection);
					}
				}
				return toolStripTextDirection;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripTextDirection));
				}
				this.Properties.SetObject(ToolStripItem.PropTextDirection, value);
				this.InvalidateItemLayout("TextDirection");
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0003C8AC File Offset: 0x0003B8AC
		// (set) Token: 0x06001DA6 RID: 7590 RVA: 0x0003C8B4 File Offset: 0x0003B8B4
		[SRDescription("ToolStripItemTextImageRelationDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(TextImageRelation.ImageBeforeText)]
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
					this.InvalidateItemLayout(PropertyNames.TextImageRelation);
				}
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0003C8F0 File Offset: 0x0003B8F0
		// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x0003C947 File Offset: 0x0003B947
		[Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ToolStripItemToolTipTextDescr")]
		[SRCategory("CatBehavior")]
		public string ToolTipText
		{
			get
			{
				if (this.AutoToolTip && string.IsNullOrEmpty(this.toolTipText))
				{
					string text = this.Text;
					if (WindowsFormsUtils.ContainsMnemonic(text))
					{
						text = string.Join("", text.Split(new char[] { '&' }));
					}
					return text;
				}
				return this.toolTipText;
			}
			set
			{
				this.toolTipText = value;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x0003C950 File Offset: 0x0003B950
		// (set) Token: 0x06001DAA RID: 7594 RVA: 0x0003C96F File Offset: 0x0003B96F
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ToolStripItemVisibleDescr")]
		public bool Visible
		{
			get
			{
				return this.ParentInternal != null && this.ParentInternal.Visible && this.Available;
			}
			set
			{
				this.SetVisibleCore(value);
			}
		}

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06001DAB RID: 7595 RVA: 0x0003C978 File Offset: 0x0003B978
		// (remove) Token: 0x06001DAC RID: 7596 RVA: 0x0003C98B File Offset: 0x0003B98B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripItemOnVisibleChangedDescr")]
		public event EventHandler VisibleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventVisibleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventVisibleChanged, value);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0003C9A0 File Offset: 0x0003B9A0
		// (set) Token: 0x06001DAE RID: 7598 RVA: 0x0003C9BC File Offset: 0x0003B9BC
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width
		{
			get
			{
				return this.Bounds.Width;
			}
			set
			{
				Rectangle rectangle = this.Bounds;
				this.SetBounds(rectangle.X, rectangle.Y, value, rectangle.Height);
			}
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0003C9EC File Offset: 0x0003B9EC
		internal void AccessibilityNotifyClients(AccessibleEvents accEvent)
		{
			if (this.ParentInternal != null)
			{
				int num = this.ParentInternal.DisplayedItems.IndexOf(this);
				this.ParentInternal.AccessibilityNotifyClients(accEvent, num);
			}
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0003CA20 File Offset: 0x0003BA20
		private void Animate()
		{
			this.Animate(!base.DesignMode && this.Visible && this.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0003CA4F File Offset: 0x0003BA4F
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0003CA58 File Offset: 0x0003BA58
		private void Animate(bool animate)
		{
			if (animate != this.state[ToolStripItem.stateCurrentlyAnimatingImage])
			{
				if (animate)
				{
					if (this.Image != null)
					{
						ImageAnimator.Animate(this.Image, new EventHandler(this.OnAnimationFrameChanged));
						this.state[ToolStripItem.stateCurrentlyAnimatingImage] = animate;
						return;
					}
				}
				else if (this.Image != null)
				{
					ImageAnimator.StopAnimate(this.Image, new EventHandler(this.OnAnimationFrameChanged));
					this.state[ToolStripItem.stateCurrentlyAnimatingImage] = animate;
				}
			}
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0003CADC File Offset: 0x0003BADC
		internal bool BeginDragForItemReorder()
		{
			if (Control.ModifierKeys == Keys.Alt && this.ParentInternal.Items.Contains(this) && this.ParentInternal.AllowItemReorder)
			{
				this.DoDragDrop(this, DragDropEffects.Move);
				return true;
			}
			return false;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0003CB23 File Offset: 0x0003BB23
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripItem.ToolStripItemAccessibleObject(this);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0003CB2B File Offset: 0x0003BB2B
		internal virtual ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripItemInternalLayout(this);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0003CB34 File Offset: 0x0003BB34
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.state[ToolStripItem.stateDisposing] = true;
				if (this.Owner != null)
				{
					this.StopAnimate();
					this.Owner.Items.Remove(this);
					this.toolStripItemInternalLayout = null;
					this.state[ToolStripItem.stateDisposed] = true;
				}
			}
			base.Dispose(disposing);
			if (disposing)
			{
				this.Properties.SetObject(ToolStripItem.PropMirroredImage, null);
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.state[ToolStripItem.stateDisposing] = false;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0003CBC9 File Offset: 0x0003BBC9
		internal static long DoubleClickTicks
		{
			get
			{
				return (long)(SystemInformation.DoubleClickTime * 10000);
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0003CBD8 File Offset: 0x0003BBD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
		{
			int[] array = new int[1];
			int[] array2 = array;
			UnsafeNativeMethods.IOleDropSource dropSource = this.DropSource;
			IDataObject dataObject = data as IDataObject;
			if (dataObject == null)
			{
				IDataObject dataObject2 = data as IDataObject;
				DataObject dataObject3;
				if (dataObject2 != null)
				{
					dataObject3 = new DataObject(dataObject2);
				}
				else if (data is ToolStripItem)
				{
					dataObject3 = new DataObject();
					dataObject3.SetData(typeof(ToolStripItem).ToString(), data);
				}
				else
				{
					dataObject3 = new DataObject();
					dataObject3.SetData(data);
				}
				dataObject = dataObject3;
			}
			try
			{
				SafeNativeMethods.DoDragDrop(dataObject, dropSource, (int)allowedEffects, array2);
			}
			catch
			{
			}
			return (DragDropEffects)array2[0];
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0003CC74 File Offset: 0x0003BC74
		internal void FireEvent(ToolStripItemEventType met)
		{
			this.FireEvent(new EventArgs(), met);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0003CC84 File Offset: 0x0003BC84
		internal void FireEvent(EventArgs e, ToolStripItemEventType met)
		{
			switch (met)
			{
			case ToolStripItemEventType.Paint:
				this.HandlePaint(e as PaintEventArgs);
				return;
			case ToolStripItemEventType.LocationChanged:
				this.OnLocationChanged(e);
				return;
			case ToolStripItemEventType.MouseMove:
				if (!this.Enabled && this.ParentInternal != null)
				{
					this.BeginDragForItemReorder();
					return;
				}
				this.FireEventInteractive(e, met);
				return;
			case ToolStripItemEventType.MouseEnter:
				this.HandleMouseEnter(e);
				return;
			case ToolStripItemEventType.MouseLeave:
				if (!this.Enabled && this.ParentInternal != null)
				{
					this.ParentInternal.UpdateToolTip(null);
					return;
				}
				this.HandleMouseLeave(e);
				return;
			case ToolStripItemEventType.MouseHover:
				if (!this.Enabled && this.ParentInternal != null && !string.IsNullOrEmpty(this.ToolTipText))
				{
					this.ParentInternal.UpdateToolTip(this);
					return;
				}
				this.FireEventInteractive(e, met);
				return;
			}
			this.FireEventInteractive(e, met);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0003CD5C File Offset: 0x0003BD5C
		internal void FireEventInteractive(EventArgs e, ToolStripItemEventType met)
		{
			if (this.Enabled)
			{
				switch (met)
				{
				case ToolStripItemEventType.MouseUp:
					this.HandleMouseUp(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseDown:
					this.HandleMouseDown(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseMove:
					this.HandleMouseMove(e as MouseEventArgs);
					return;
				case ToolStripItemEventType.MouseEnter:
				case ToolStripItemEventType.MouseLeave:
					break;
				case ToolStripItemEventType.MouseHover:
					this.HandleMouseHover(e);
					return;
				case ToolStripItemEventType.Click:
					this.HandleClick(e);
					return;
				case ToolStripItemEventType.DoubleClick:
					this.HandleDoubleClick(e);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0003CDDA File Offset: 0x0003BDDA
		private Font GetOwnerFont()
		{
			if (this.Owner != null)
			{
				return this.Owner.Font;
			}
			return null;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0003CDF1 File Offset: 0x0003BDF1
		public ToolStrip GetCurrentParent()
		{
			return this.Parent;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0003CDF9 File Offset: 0x0003BDF9
		internal ToolStripDropDown GetCurrentParentDropDown()
		{
			if (this.ParentInternal != null)
			{
				return this.ParentInternal as ToolStripDropDown;
			}
			return this.Owner as ToolStripDropDown;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0003CE1C File Offset: 0x0003BE1C
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			constrainingSize = LayoutUtils.ConvertZeroToUnbounded(constrainingSize);
			return this.InternalLayout.GetPreferredSize(constrainingSize - this.Padding.Size) + this.Padding.Size;
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0003CE64 File Offset: 0x0003BE64
		internal Size GetTextSize()
		{
			if (string.IsNullOrEmpty(this.Text))
			{
				return Size.Empty;
			}
			if (this.cachedTextSize == Size.Empty)
			{
				this.cachedTextSize = TextRenderer.MeasureText(this.Text, this.Font);
			}
			return this.cachedTextSize;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0003CEB3 File Offset: 0x0003BEB3
		public void Invalidate()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Invalidate(this.Bounds, true);
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0003CED0 File Offset: 0x0003BED0
		public void Invalidate(Rectangle r)
		{
			Point point = this.TranslatePoint(r.Location, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ToolStripCoords);
			if (this.ParentInternal != null)
			{
				this.ParentInternal.Invalidate(new Rectangle(point, r.Size), true);
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0003CF0E File Offset: 0x0003BF0E
		internal void InvalidateItemLayout(string affectedProperty, bool invalidatePainting)
		{
			this.toolStripItemInternalLayout = null;
			if (this.Owner != null)
			{
				LayoutTransaction.DoLayout(this.Owner, this, affectedProperty);
			}
			if (invalidatePainting && this.Owner != null)
			{
				this.Owner.Invalidate();
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0003CF42 File Offset: 0x0003BF42
		internal void InvalidateItemLayout(string affectedProperty)
		{
			this.InvalidateItemLayout(affectedProperty, true);
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0003CF4C File Offset: 0x0003BF4C
		internal void InvalidateImageListImage()
		{
			if (this.ImageIndexer.ActualIndex >= 0)
			{
				this.Properties.SetObject(ToolStripItem.PropImage, null);
				this.InvalidateItemLayout(PropertyNames.Image);
			}
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0003CF78 File Offset: 0x0003BF78
		internal void InvokePaint()
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.InvokePaintItem(this);
			}
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0003CF8E File Offset: 0x0003BF8E
		protected internal virtual bool IsInputKey(Keys keyData)
		{
			return false;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0003CF91 File Offset: 0x0003BF91
		protected internal virtual bool IsInputChar(char charCode)
		{
			return false;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0003CF94 File Offset: 0x0003BF94
		private void HandleClick(EventArgs e)
		{
			try
			{
				if (!base.DesignMode)
				{
					this.state[ToolStripItem.statePressed] = true;
				}
				this.InvokePaint();
				if (this.SupportsItemClick && this.Owner != null)
				{
					this.Owner.HandleItemClick(this);
				}
				this.OnClick(e);
				if (this.SupportsItemClick && this.Owner != null)
				{
					this.Owner.HandleItemClicked(this);
				}
			}
			finally
			{
				this.state[ToolStripItem.statePressed] = false;
			}
			this.Invalidate();
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0003D02C File Offset: 0x0003C02C
		private void HandleDoubleClick(EventArgs e)
		{
			this.OnDoubleClick(e);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0003D035 File Offset: 0x0003C035
		private void HandlePaint(PaintEventArgs e)
		{
			this.Animate();
			ImageAnimator.UpdateFrames(this.Image);
			this.OnPaint(e);
			this.RaisePaintEvent(ToolStripItem.EventPaint, e);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0003D05C File Offset: 0x0003C05C
		private void HandleMouseEnter(EventArgs e)
		{
			if (!base.DesignMode && this.ParentInternal != null && this.ParentInternal.CanHotTrack && this.ParentInternal.ShouldSelectItem())
			{
				if (this.Enabled)
				{
					bool menuAutoExpand = this.ParentInternal.MenuAutoExpand;
					if (this.ParentInternal.LastMouseDownedItem == this && UnsafeNativeMethods.GetKeyState(1) < 0)
					{
						this.Push(true);
					}
					this.Select();
					this.ParentInternal.MenuAutoExpand = menuAutoExpand;
				}
				else if (this.SupportsDisabledHotTracking)
				{
					this.Select();
				}
			}
			if (this.Enabled)
			{
				this.OnMouseEnter(e);
				this.RaiseEvent(ToolStripItem.EventMouseEnter, e);
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0003D104 File Offset: 0x0003C104
		private void HandleMouseMove(MouseEventArgs mea)
		{
			if (this.Enabled && this.CanSelect && !this.Selected && this.ParentInternal != null && this.ParentInternal.CanHotTrack && this.ParentInternal.ShouldSelectItem())
			{
				this.Select();
			}
			this.OnMouseMove(mea);
			this.RaiseMouseEvent(ToolStripItem.EventMouseMove, mea);
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0003D164 File Offset: 0x0003C164
		private void HandleMouseHover(EventArgs e)
		{
			this.OnMouseHover(e);
			this.RaiseEvent(ToolStripItem.EventMouseHover, e);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0003D17C File Offset: 0x0003C17C
		private void HandleLeave()
		{
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag] || this.state[ToolStripItem.statePressed] || this.state[ToolStripItem.stateSelected])
			{
				this.state[ToolStripItem.stateMouseDownAndNoDrag | ToolStripItem.statePressed | ToolStripItem.stateSelected] = false;
				this.Invalidate();
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0003D1E2 File Offset: 0x0003C1E2
		private void HandleMouseLeave(EventArgs e)
		{
			this.HandleLeave();
			if (this.Enabled)
			{
				this.OnMouseLeave(e);
				this.RaiseEvent(ToolStripItem.EventMouseLeave, e);
			}
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0003D208 File Offset: 0x0003C208
		private void HandleMouseDown(MouseEventArgs e)
		{
			this.state[ToolStripItem.stateMouseDownAndNoDrag] = !this.BeginDragForItemReorder();
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag])
			{
				if (e.Button == MouseButtons.Left)
				{
					this.Push(true);
				}
				this.OnMouseDown(e);
				this.RaiseMouseEvent(ToolStripItem.EventMouseDown, e);
			}
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0003D268 File Offset: 0x0003C268
		private void HandleMouseUp(MouseEventArgs e)
		{
			bool flag = this.ParentInternal.LastMouseDownedItem == this;
			if (!flag && !this.MouseDownAndUpMustBeInSameItem)
			{
				flag = this.ParentInternal.ShouldSelectItem();
			}
			if (this.state[ToolStripItem.stateMouseDownAndNoDrag] || flag)
			{
				this.Push(false);
				if (e.Button == MouseButtons.Left || (e.Button == MouseButtons.Right && this.state[ToolStripItem.stateSupportsRightClick]))
				{
					bool flag2 = false;
					if (this.DoubleClickEnabled)
					{
						long ticks = DateTime.Now.Ticks;
						long num = ticks - this.lastClickTime;
						this.lastClickTime = ticks;
						if (num >= 0L && num < ToolStripItem.DoubleClickTicks)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						this.HandleDoubleClick(new EventArgs());
						this.lastClickTime = 0L;
					}
					else
					{
						this.HandleClick(new EventArgs());
					}
				}
				this.OnMouseUp(e);
				this.RaiseMouseEvent(ToolStripItem.EventMouseUp, e);
			}
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0003D354 File Offset: 0x0003C354
		internal virtual void OnAccessibleDescriptionChanged(EventArgs e)
		{
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0003D356 File Offset: 0x0003C356
		internal virtual void OnAccessibleNameChanged(EventArgs e)
		{
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0003D358 File Offset: 0x0003C358
		internal virtual void OnAccessibleDefaultActionDescriptionChanged(EventArgs e)
		{
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x0003D35A File Offset: 0x0003C35A
		internal virtual void OnAccessibleRoleChanged(EventArgs e)
		{
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0003D35C File Offset: 0x0003C35C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnBackColorChanged(EventArgs e)
		{
			this.Invalidate();
			this.RaiseEvent(ToolStripItem.EventBackColorChanged, e);
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0003D370 File Offset: 0x0003C370
		protected virtual void OnBoundsChanged()
		{
			LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.Bounds);
			this.InternalLayout.PerformLayout();
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0003D38E File Offset: 0x0003C38E
		protected virtual void OnClick(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventClick, e);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0003D39C File Offset: 0x0003C39C
		protected internal virtual void OnLayout(LayoutEventArgs e)
		{
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0003D39E File Offset: 0x0003C39E
		void IDropTarget.OnDragEnter(DragEventArgs dragEvent)
		{
			this.OnDragEnter(dragEvent);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0003D3A7 File Offset: 0x0003C3A7
		void IDropTarget.OnDragOver(DragEventArgs dragEvent)
		{
			this.OnDragOver(dragEvent);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0003D3B0 File Offset: 0x0003C3B0
		void IDropTarget.OnDragLeave(EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0003D3B9 File Offset: 0x0003C3B9
		void IDropTarget.OnDragDrop(DragEventArgs dragEvent)
		{
			this.OnDragDrop(dragEvent);
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0003D3C2 File Offset: 0x0003C3C2
		void ISupportOleDropSource.OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEventArgs)
		{
			this.OnGiveFeedback(giveFeedbackEventArgs);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0003D3CB File Offset: 0x0003C3CB
		void ISupportOleDropSource.OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEventArgs)
		{
			this.OnQueryContinueDrag(queryContinueDragEventArgs);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0003D3D4 File Offset: 0x0003C3D4
		private void OnAnimationFrameChanged(object o, EventArgs e)
		{
			ToolStrip parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				if (parentInternal.Disposing || parentInternal.IsDisposed)
				{
					return;
				}
				if (parentInternal.IsHandleCreated && parentInternal.InvokeRequired)
				{
					parentInternal.BeginInvoke(new EventHandler(this.OnAnimationFrameChanged), new object[] { o, e });
					return;
				}
				this.Invalidate();
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0003D436 File Offset: 0x0003C436
		protected virtual void OnAvailableChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventAvailableChanged, e);
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0003D444 File Offset: 0x0003C444
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragEnter(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragEnter, dragEvent);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0003D452 File Offset: 0x0003C452
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragOver(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragOver, dragEvent);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0003D460 File Offset: 0x0003C460
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragLeave(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDragLeave, e);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0003D46E File Offset: 0x0003C46E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDragDrop(DragEventArgs dragEvent)
		{
			this.RaiseDragEvent(ToolStripItem.EventDragDrop, dragEvent);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0003D47C File Offset: 0x0003C47C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnDisplayStyleChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDisplayStyleChanged, e);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0003D48C File Offset: 0x0003C48C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnGiveFeedback(GiveFeedbackEventArgs giveFeedbackEvent)
		{
			GiveFeedbackEventHandler giveFeedbackEventHandler = (GiveFeedbackEventHandler)base.Events[ToolStripItem.EventGiveFeedback];
			if (giveFeedbackEventHandler != null)
			{
				giveFeedbackEventHandler(this, giveFeedbackEvent);
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0003D4BA File Offset: 0x0003C4BA
		internal virtual void OnImageScalingSizeChanged(EventArgs e)
		{
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0003D4BC File Offset: 0x0003C4BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnQueryContinueDrag(QueryContinueDragEventArgs queryContinueDragEvent)
		{
			this.RaiseQueryContinueDragEvent(ToolStripItem.EventQueryContinueDrag, queryContinueDragEvent);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0003D4CA File Offset: 0x0003C4CA
		protected virtual void OnDoubleClick(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventDoubleClick, e);
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0003D4D8 File Offset: 0x0003C4D8
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventEnabledChanged, e);
			this.Animate();
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0003D4EC File Offset: 0x0003C4EC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnForeColorChanged(EventArgs e)
		{
			this.Invalidate();
			this.RaiseEvent(ToolStripItem.EventForeColorChanged, e);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0003D500 File Offset: 0x0003C500
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnFontChanged(EventArgs e)
		{
			this.cachedTextSize = Size.Empty;
			if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
			{
				this.InvalidateItemLayout(PropertyNames.Font);
			}
			else
			{
				this.toolStripItemInternalLayout = null;
			}
			this.RaiseEvent(ToolStripItem.EventFontChanged, e);
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0003D538 File Offset: 0x0003C538
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventLocationChanged, e);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0003D546 File Offset: 0x0003C546
		protected virtual void OnMouseEnter(EventArgs e)
		{
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0003D548 File Offset: 0x0003C548
		protected virtual void OnMouseMove(MouseEventArgs mea)
		{
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0003D54A File Offset: 0x0003C54A
		protected virtual void OnMouseHover(EventArgs e)
		{
			if (this.ParentInternal != null && !string.IsNullOrEmpty(this.ToolTipText))
			{
				this.ParentInternal.UpdateToolTip(this);
			}
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0003D56D File Offset: 0x0003C56D
		protected virtual void OnMouseLeave(EventArgs e)
		{
			if (this.ParentInternal != null)
			{
				this.ParentInternal.UpdateToolTip(null);
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0003D583 File Offset: 0x0003C583
		protected virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0003D585 File Offset: 0x0003C585
		protected virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0003D587 File Offset: 0x0003C587
		protected virtual void OnPaint(PaintEventArgs e)
		{
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0003D58C File Offset: 0x0003C58C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentBackColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(ToolStripItem.PropBackColor).IsEmpty)
			{
				this.OnBackColorChanged(e);
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0003D5BA File Offset: 0x0003C5BA
		protected virtual void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
		{
			this.SetAmbientMargin();
			if (oldParent != null && oldParent.DropTargetManager != null)
			{
				oldParent.DropTargetManager.EnsureUnRegistered(this);
			}
			if (this.AllowDrop && newParent != null)
			{
				this.EnsureParentDropTargetRegistered();
			}
			this.Animate();
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0003D5F0 File Offset: 0x0003C5F0
		protected internal virtual void OnParentEnabledChanged(EventArgs e)
		{
			this.OnEnabledChanged(EventArgs.Empty);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0003D5FD File Offset: 0x0003C5FD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void OnOwnerFontChanged(EventArgs e)
		{
			if (this.Properties.GetObject(ToolStripItem.PropFont) == null)
			{
				this.OnFontChanged(e);
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0003D618 File Offset: 0x0003C618
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnParentForeColorChanged(EventArgs e)
		{
			if (this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty)
			{
				this.OnForeColorChanged(e);
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0003D646 File Offset: 0x0003C646
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void OnParentRightToLeftChanged(EventArgs e)
		{
			if (!this.Properties.ContainsInteger(ToolStripItem.PropRightToLeft) || this.Properties.GetInteger(ToolStripItem.PropRightToLeft) == 2)
			{
				this.OnRightToLeftChanged(e);
			}
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0003D674 File Offset: 0x0003C674
		protected virtual void OnOwnerChanged(EventArgs e)
		{
			this.RaiseEvent(ToolStripItem.EventOwnerChanged, e);
			this.SetAmbientMargin();
			if (this.Owner != null)
			{
				bool flag = false;
				int num = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
				if (!flag)
				{
					num = 2;
				}
				if (num == 2 && this.RightToLeft != this.DefaultRightToLeft)
				{
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0003D6D4 File Offset: 0x0003C6D4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal void OnOwnerTextDirectionChanged()
		{
			ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
			if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
			{
				toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
			}
			if (toolStripTextDirection == ToolStripTextDirection.Inherit)
			{
				this.InvalidateItemLayout("TextDirection");
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0003D719 File Offset: 0x0003C719
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			this.InvalidateItemLayout(PropertyNames.RightToLeft);
			this.RaiseEvent(ToolStripItem.EventRightToLeft, e);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0003D732 File Offset: 0x0003C732
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnTextChanged(EventArgs e)
		{
			this.cachedTextSize = Size.Empty;
			this.InvalidateItemLayout(PropertyNames.Text);
			this.RaiseEvent(ToolStripItem.EventText, e);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0003D758 File Offset: 0x0003C758
		protected virtual void OnVisibleChanged(EventArgs e)
		{
			if (this.Owner != null && !this.Owner.IsDisposed && !this.Owner.Disposing)
			{
				this.Owner.OnItemVisibleChanged(new ToolStripItemEventArgs(this), true);
			}
			this.RaiseEvent(ToolStripItem.EventVisibleChanged, e);
			this.Animate();
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0003D7AB File Offset: 0x0003C7AB
		public void PerformClick()
		{
			if (this.Enabled && this.Available)
			{
				this.FireEvent(ToolStripItemEventType.Click);
			}
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0003D7C4 File Offset: 0x0003C7C4
		internal void Push(bool push)
		{
			if (!this.CanSelect || !this.Enabled || base.DesignMode)
			{
				return;
			}
			if (this.state[ToolStripItem.statePressed] != push)
			{
				this.state[ToolStripItem.statePressed] = push;
				if (this.Available)
				{
					this.Invalidate();
				}
			}
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0003D81C File Offset: 0x0003C81C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Return || (this.state[ToolStripItem.stateSupportsSpaceKey] && keyData == Keys.Space))
			{
				this.FireEvent(ToolStripItemEventType.Click);
				if (this.ParentInternal != null && !this.ParentInternal.IsDropDown)
				{
					this.ParentInternal.RestoreFocusInternal();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0003D86F File Offset: 0x0003C86F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			return false;
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0003D872 File Offset: 0x0003C872
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal virtual bool ProcessMnemonic(char charCode)
		{
			this.FireEvent(ToolStripItemEventType.Click);
			return true;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x0003D87C File Offset: 0x0003C87C
		internal void RaiseCancelEvent(object key, CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[key];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0003D8A8 File Offset: 0x0003C8A8
		internal void RaiseDragEvent(object key, DragEventArgs e)
		{
			DragEventHandler dragEventHandler = (DragEventHandler)base.Events[key];
			if (dragEventHandler != null)
			{
				dragEventHandler(this, e);
			}
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0003D8D4 File Offset: 0x0003C8D4
		internal void RaiseEvent(object key, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[key];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0003D900 File Offset: 0x0003C900
		internal void RaiseKeyEvent(object key, KeyEventArgs e)
		{
			KeyEventHandler keyEventHandler = (KeyEventHandler)base.Events[key];
			if (keyEventHandler != null)
			{
				keyEventHandler(this, e);
			}
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x0003D92C File Offset: 0x0003C92C
		internal void RaiseKeyPressEvent(object key, KeyPressEventArgs e)
		{
			KeyPressEventHandler keyPressEventHandler = (KeyPressEventHandler)base.Events[key];
			if (keyPressEventHandler != null)
			{
				keyPressEventHandler(this, e);
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0003D958 File Offset: 0x0003C958
		internal void RaiseMouseEvent(object key, MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[key];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0003D984 File Offset: 0x0003C984
		internal void RaisePaintEvent(object key, PaintEventArgs e)
		{
			PaintEventHandler paintEventHandler = (PaintEventHandler)base.Events[key];
			if (paintEventHandler != null)
			{
				paintEventHandler(this, e);
			}
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0003D9B0 File Offset: 0x0003C9B0
		internal void RaiseQueryContinueDragEvent(object key, QueryContinueDragEventArgs e)
		{
			QueryContinueDragEventHandler queryContinueDragEventHandler = (QueryContinueDragEventHandler)base.Events[key];
			if (queryContinueDragEventHandler != null)
			{
				queryContinueDragEventHandler(this, e);
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0003D9DA File Offset: 0x0003C9DA
		private void ResetToolTipText()
		{
			this.toolTipText = null;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0003D9E4 File Offset: 0x0003C9E4
		public void Select()
		{
			if (!this.CanSelect)
			{
				return;
			}
			if (this.Owner != null && this.Owner.IsCurrentlyDragging)
			{
				return;
			}
			if (this.ParentInternal != null && this.ParentInternal.IsSelectionSuspended)
			{
				return;
			}
			if (!this.Selected)
			{
				this.state[ToolStripItem.stateSelected] = true;
				if (this.ParentInternal != null)
				{
					this.ParentInternal.NotifySelectionChange(this);
				}
				if (this.IsOnDropDown && this.OwnerItem != null && this.OwnerItem.IsOnDropDown)
				{
					this.OwnerItem.Select();
				}
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0003DA7C File Offset: 0x0003CA7C
		internal void SetOwner(ToolStrip newOwner)
		{
			if (this.owner != newOwner)
			{
				Font font = this.Font;
				this.owner = newOwner;
				if (newOwner == null)
				{
					this.ParentInternal = null;
				}
				if (!this.state[ToolStripItem.stateDisposing] && !this.IsDisposed)
				{
					this.OnOwnerChanged(EventArgs.Empty);
					if (font != this.Font)
					{
						this.OnFontChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0003DAE4 File Offset: 0x0003CAE4
		protected virtual void SetVisibleCore(bool visible)
		{
			if (this.state[ToolStripItem.stateVisible] != visible)
			{
				this.state[ToolStripItem.stateVisible] = visible;
				this.Unselect();
				this.Push(false);
				this.OnAvailableChanged(EventArgs.Empty);
				this.OnVisibleChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0003DB38 File Offset: 0x0003CB38
		protected internal virtual void SetBounds(Rectangle bounds)
		{
			Rectangle rectangle = this.bounds;
			this.bounds = bounds;
			if (!this.state[ToolStripItem.stateContstructing])
			{
				if (this.bounds != rectangle)
				{
					this.OnBoundsChanged();
				}
				if (this.bounds.Location != rectangle.Location)
				{
					this.OnLocationChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0003DB9D File Offset: 0x0003CB9D
		internal void SetBounds(int x, int y, int width, int height)
		{
			this.SetBounds(new Rectangle(x, y, width, height));
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0003DBAF File Offset: 0x0003CBAF
		internal void SetPlacement(ToolStripItemPlacement placement)
		{
			this.placement = placement;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0003DBB8 File Offset: 0x0003CBB8
		internal void SetAmbientMargin()
		{
			if (this.state[ToolStripItem.stateUseAmbientMargin] && this.Margin != this.DefaultMargin)
			{
				CommonProperties.SetMargin(this, this.DefaultMargin);
			}
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0003DBEB File Offset: 0x0003CBEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageTransparentColor()
		{
			return this.ImageTransparentColor != Color.Empty;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0003DC00 File Offset: 0x0003CC00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeBackColor()
		{
			return !this.Properties.GetColor(ToolStripItem.PropBackColor).IsEmpty;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0003DC28 File Offset: 0x0003CC28
		private bool ShouldSerializeDisplayStyle()
		{
			return this.DisplayStyle != this.DefaultDisplayStyle;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0003DC3B File Offset: 0x0003CC3B
		private bool ShouldSerializeToolTipText()
		{
			return !string.IsNullOrEmpty(this.toolTipText);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0003DC4C File Offset: 0x0003CC4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeForeColor()
		{
			return !this.Properties.GetColor(ToolStripItem.PropForeColor).IsEmpty;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0003DC74 File Offset: 0x0003CC74
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeFont()
		{
			bool flag;
			object @object = this.Properties.GetObject(ToolStripItem.PropFont, out flag);
			return flag && @object != null;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0003DCA0 File Offset: 0x0003CCA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializePadding()
		{
			return this.Padding != this.DefaultPadding;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0003DCB3 File Offset: 0x0003CCB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeMargin()
		{
			return this.Margin != this.DefaultMargin;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0003DCC6 File Offset: 0x0003CCC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisible()
		{
			return !this.state[ToolStripItem.stateVisible];
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0003DCDB File Offset: 0x0003CCDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImage()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex < 0;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0003DCF5 File Offset: 0x0003CCF5
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageKey()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex >= 0 && this.ImageIndexer.Key != null && this.ImageIndexer.Key.Length != 0;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0003DD34 File Offset: 0x0003CD34
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeImageIndex()
		{
			return this.Image != null && this.ImageIndexer.ActualIndex >= 0 && this.ImageIndexer.Index != -1;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0003DD60 File Offset: 0x0003CD60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeRightToLeft()
		{
			bool flag = false;
			int integer = this.Properties.GetInteger(ToolStripItem.PropRightToLeft, out flag);
			return flag && integer != (int)this.DefaultRightToLeft;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0003DD94 File Offset: 0x0003CD94
		private bool ShouldSerializeTextDirection()
		{
			ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Inherit;
			if (this.Properties.ContainsObject(ToolStripItem.PropTextDirection))
			{
				toolStripTextDirection = (ToolStripTextDirection)this.Properties.GetObject(ToolStripItem.PropTextDirection);
			}
			return toolStripTextDirection != ToolStripTextDirection.Inherit;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0003DDD2 File Offset: 0x0003CDD2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetBackColor()
		{
			this.BackColor = Color.Empty;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0003DDDF File Offset: 0x0003CDDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetDisplayStyle()
		{
			this.DisplayStyle = this.DefaultDisplayStyle;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0003DDED File Offset: 0x0003CDED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetForeColor()
		{
			this.ForeColor = Color.Empty;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0003DDFA File Offset: 0x0003CDFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetFont()
		{
			this.Font = null;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0003DE03 File Offset: 0x0003CE03
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0003DE0C File Offset: 0x0003CE0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetImageTransparentColor()
		{
			this.ImageTransparentColor = Color.Empty;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0003DE19 File Offset: 0x0003CE19
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetMargin()
		{
			this.state[ToolStripItem.stateUseAmbientMargin] = true;
			this.SetAmbientMargin();
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0003DE32 File Offset: 0x0003CE32
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetPadding()
		{
			CommonProperties.ResetPadding(this);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0003DE3A File Offset: 0x0003CE3A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.Inherit;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0003DE43 File Offset: 0x0003CE43
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetTextDirection()
		{
			this.TextDirection = ToolStripTextDirection.Inherit;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0003DE4C File Offset: 0x0003CE4C
		internal Point TranslatePoint(Point fromPoint, ToolStripPointType fromPointType, ToolStripPointType toPointType)
		{
			ToolStrip toolStrip = this.ParentInternal;
			if (toolStrip == null)
			{
				toolStrip = ((this.IsOnOverflow && this.Owner != null) ? this.Owner.OverflowButton.DropDown : this.Owner);
			}
			if (toolStrip == null)
			{
				return fromPoint;
			}
			if (fromPointType == toPointType)
			{
				return fromPoint;
			}
			Point point = Point.Empty;
			Point location = this.Bounds.Location;
			if (fromPointType == ToolStripPointType.ScreenCoords)
			{
				point = toolStrip.PointToClient(fromPoint);
				if (toPointType == ToolStripPointType.ToolStripItemCoords)
				{
					point.X += location.X;
					point.Y += location.Y;
				}
			}
			else
			{
				if (fromPointType == ToolStripPointType.ToolStripItemCoords)
				{
					fromPoint.X += location.X;
					fromPoint.Y += location.Y;
				}
				if (toPointType == ToolStripPointType.ScreenCoords)
				{
					point = toolStrip.PointToScreen(fromPoint);
				}
				else if (toPointType == ToolStripPointType.ToolStripItemCoords)
				{
					fromPoint.X -= location.X;
					fromPoint.Y -= location.Y;
					point = fromPoint;
				}
				else
				{
					point = fromPoint;
				}
			}
			return point;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0003DF5C File Offset: 0x0003CF5C
		public override string ToString()
		{
			if (this.Text != null && this.Text.Length != 0)
			{
				return this.Text;
			}
			return base.ToString();
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0003DF80 File Offset: 0x0003CF80
		internal void Unselect()
		{
			if (this.state[ToolStripItem.stateSelected])
			{
				this.state[ToolStripItem.stateSelected] = false;
				if (this.Available)
				{
					this.Invalidate();
					if (this.ParentInternal != null)
					{
						this.ParentInternal.NotifySelectionChange(this);
					}
				}
			}
		}

		// Token: 0x0400137C RID: 4988
		internal static readonly TraceSwitch MouseDebugging;

		// Token: 0x0400137D RID: 4989
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x0400137E RID: 4990
		private PropertyStore propertyStore;

		// Token: 0x0400137F RID: 4991
		private ToolStripItemAlignment alignment;

		// Token: 0x04001380 RID: 4992
		private ToolStrip parent;

		// Token: 0x04001381 RID: 4993
		private ToolStrip owner;

		// Token: 0x04001382 RID: 4994
		private ToolStripItemOverflow overflow = ToolStripItemOverflow.AsNeeded;

		// Token: 0x04001383 RID: 4995
		private ToolStripItemPlacement placement = ToolStripItemPlacement.None;

		// Token: 0x04001384 RID: 4996
		private ContentAlignment imageAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04001385 RID: 4997
		private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04001386 RID: 4998
		private TextImageRelation textImageRelation = TextImageRelation.ImageBeforeText;

		// Token: 0x04001387 RID: 4999
		private ToolStripItemImageIndexer imageIndexer;

		// Token: 0x04001388 RID: 5000
		private ToolStripItemInternalLayout toolStripItemInternalLayout;

		// Token: 0x04001389 RID: 5001
		private BitVector32 state = default(BitVector32);

		// Token: 0x0400138A RID: 5002
		private string toolTipText;

		// Token: 0x0400138B RID: 5003
		private Color imageTransparentColor = Color.Empty;

		// Token: 0x0400138C RID: 5004
		private ToolStripItemImageScaling imageScaling = ToolStripItemImageScaling.SizeToFit;

		// Token: 0x0400138D RID: 5005
		private Size cachedTextSize = Size.Empty;

		// Token: 0x0400138E RID: 5006
		private ToolStripItemDisplayStyle displayStyle = ToolStripItemDisplayStyle.ImageAndText;

		// Token: 0x0400138F RID: 5007
		private static readonly ArrangedElementCollection EmptyChildCollection = new ArrangedElementCollection();

		// Token: 0x04001390 RID: 5008
		internal static readonly object EventMouseDown = new object();

		// Token: 0x04001391 RID: 5009
		internal static readonly object EventMouseEnter = new object();

		// Token: 0x04001392 RID: 5010
		internal static readonly object EventMouseLeave = new object();

		// Token: 0x04001393 RID: 5011
		internal static readonly object EventMouseHover = new object();

		// Token: 0x04001394 RID: 5012
		internal static readonly object EventMouseMove = new object();

		// Token: 0x04001395 RID: 5013
		internal static readonly object EventMouseUp = new object();

		// Token: 0x04001396 RID: 5014
		internal static readonly object EventMouseWheel = new object();

		// Token: 0x04001397 RID: 5015
		internal static readonly object EventClick = new object();

		// Token: 0x04001398 RID: 5016
		internal static readonly object EventDoubleClick = new object();

		// Token: 0x04001399 RID: 5017
		internal static readonly object EventDragDrop = new object();

		// Token: 0x0400139A RID: 5018
		internal static readonly object EventDragEnter = new object();

		// Token: 0x0400139B RID: 5019
		internal static readonly object EventDragLeave = new object();

		// Token: 0x0400139C RID: 5020
		internal static readonly object EventDragOver = new object();

		// Token: 0x0400139D RID: 5021
		internal static readonly object EventDisplayStyleChanged = new object();

		// Token: 0x0400139E RID: 5022
		internal static readonly object EventEnabledChanged = new object();

		// Token: 0x0400139F RID: 5023
		internal static readonly object EventFontChanged = new object();

		// Token: 0x040013A0 RID: 5024
		internal static readonly object EventForeColorChanged = new object();

		// Token: 0x040013A1 RID: 5025
		internal static readonly object EventBackColorChanged = new object();

		// Token: 0x040013A2 RID: 5026
		internal static readonly object EventGiveFeedback = new object();

		// Token: 0x040013A3 RID: 5027
		internal static readonly object EventQueryContinueDrag = new object();

		// Token: 0x040013A4 RID: 5028
		internal static readonly object EventQueryAccessibilityHelp = new object();

		// Token: 0x040013A5 RID: 5029
		internal static readonly object EventMove = new object();

		// Token: 0x040013A6 RID: 5030
		internal static readonly object EventResize = new object();

		// Token: 0x040013A7 RID: 5031
		internal static readonly object EventLayout = new object();

		// Token: 0x040013A8 RID: 5032
		internal static readonly object EventLocationChanged = new object();

		// Token: 0x040013A9 RID: 5033
		internal static readonly object EventRightToLeft = new object();

		// Token: 0x040013AA RID: 5034
		internal static readonly object EventVisibleChanged = new object();

		// Token: 0x040013AB RID: 5035
		internal static readonly object EventAvailableChanged = new object();

		// Token: 0x040013AC RID: 5036
		internal static readonly object EventOwnerChanged = new object();

		// Token: 0x040013AD RID: 5037
		internal static readonly object EventPaint = new object();

		// Token: 0x040013AE RID: 5038
		internal static readonly object EventText = new object();

		// Token: 0x040013AF RID: 5039
		internal static readonly object EventSelectedChanged = new object();

		// Token: 0x040013B0 RID: 5040
		private static readonly int PropName = PropertyStore.CreateKey();

		// Token: 0x040013B1 RID: 5041
		private static readonly int PropText = PropertyStore.CreateKey();

		// Token: 0x040013B2 RID: 5042
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x040013B3 RID: 5043
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x040013B4 RID: 5044
		private static readonly int PropImage = PropertyStore.CreateKey();

		// Token: 0x040013B5 RID: 5045
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x040013B6 RID: 5046
		private static readonly int PropRightToLeft = PropertyStore.CreateKey();

		// Token: 0x040013B7 RID: 5047
		private static readonly int PropTag = PropertyStore.CreateKey();

		// Token: 0x040013B8 RID: 5048
		private static readonly int PropAccessibility = PropertyStore.CreateKey();

		// Token: 0x040013B9 RID: 5049
		private static readonly int PropAccessibleName = PropertyStore.CreateKey();

		// Token: 0x040013BA RID: 5050
		private static readonly int PropAccessibleRole = PropertyStore.CreateKey();

		// Token: 0x040013BB RID: 5051
		private static readonly int PropAccessibleHelpProvider = PropertyStore.CreateKey();

		// Token: 0x040013BC RID: 5052
		private static readonly int PropAccessibleDefaultActionDescription = PropertyStore.CreateKey();

		// Token: 0x040013BD RID: 5053
		private static readonly int PropAccessibleDescription = PropertyStore.CreateKey();

		// Token: 0x040013BE RID: 5054
		private static readonly int PropTextDirection = PropertyStore.CreateKey();

		// Token: 0x040013BF RID: 5055
		private static readonly int PropMirroredImage = PropertyStore.CreateKey();

		// Token: 0x040013C0 RID: 5056
		private static readonly int PropBackgroundImage = PropertyStore.CreateKey();

		// Token: 0x040013C1 RID: 5057
		private static readonly int PropBackgroundImageLayout = PropertyStore.CreateKey();

		// Token: 0x040013C2 RID: 5058
		private static readonly int PropMergeAction = PropertyStore.CreateKey();

		// Token: 0x040013C3 RID: 5059
		private static readonly int PropMergeIndex = PropertyStore.CreateKey();

		// Token: 0x040013C4 RID: 5060
		private static readonly int stateAllowDrop = BitVector32.CreateMask();

		// Token: 0x040013C5 RID: 5061
		private static readonly int stateVisible = BitVector32.CreateMask(ToolStripItem.stateAllowDrop);

		// Token: 0x040013C6 RID: 5062
		private static readonly int stateEnabled = BitVector32.CreateMask(ToolStripItem.stateVisible);

		// Token: 0x040013C7 RID: 5063
		private static readonly int stateMouseDownAndNoDrag = BitVector32.CreateMask(ToolStripItem.stateEnabled);

		// Token: 0x040013C8 RID: 5064
		private static readonly int stateAutoSize = BitVector32.CreateMask(ToolStripItem.stateMouseDownAndNoDrag);

		// Token: 0x040013C9 RID: 5065
		private static readonly int statePressed = BitVector32.CreateMask(ToolStripItem.stateAutoSize);

		// Token: 0x040013CA RID: 5066
		private static readonly int stateSelected = BitVector32.CreateMask(ToolStripItem.statePressed);

		// Token: 0x040013CB RID: 5067
		private static readonly int stateContstructing = BitVector32.CreateMask(ToolStripItem.stateSelected);

		// Token: 0x040013CC RID: 5068
		private static readonly int stateDisposed = BitVector32.CreateMask(ToolStripItem.stateContstructing);

		// Token: 0x040013CD RID: 5069
		private static readonly int stateCurrentlyAnimatingImage = BitVector32.CreateMask(ToolStripItem.stateDisposed);

		// Token: 0x040013CE RID: 5070
		private static readonly int stateDoubleClickEnabled = BitVector32.CreateMask(ToolStripItem.stateCurrentlyAnimatingImage);

		// Token: 0x040013CF RID: 5071
		private static readonly int stateAutoToolTip = BitVector32.CreateMask(ToolStripItem.stateDoubleClickEnabled);

		// Token: 0x040013D0 RID: 5072
		private static readonly int stateSupportsRightClick = BitVector32.CreateMask(ToolStripItem.stateAutoToolTip);

		// Token: 0x040013D1 RID: 5073
		private static readonly int stateSupportsItemClick = BitVector32.CreateMask(ToolStripItem.stateSupportsRightClick);

		// Token: 0x040013D2 RID: 5074
		private static readonly int stateRightToLeftAutoMirrorImage = BitVector32.CreateMask(ToolStripItem.stateSupportsItemClick);

		// Token: 0x040013D3 RID: 5075
		private static readonly int stateInvalidMirroredImage = BitVector32.CreateMask(ToolStripItem.stateRightToLeftAutoMirrorImage);

		// Token: 0x040013D4 RID: 5076
		private static readonly int stateSupportsSpaceKey = BitVector32.CreateMask(ToolStripItem.stateInvalidMirroredImage);

		// Token: 0x040013D5 RID: 5077
		private static readonly int stateMouseDownAndUpMustBeInSameItem = BitVector32.CreateMask(ToolStripItem.stateSupportsSpaceKey);

		// Token: 0x040013D6 RID: 5078
		private static readonly int stateSupportsDisabledHotTracking = BitVector32.CreateMask(ToolStripItem.stateMouseDownAndUpMustBeInSameItem);

		// Token: 0x040013D7 RID: 5079
		private static readonly int stateUseAmbientMargin = BitVector32.CreateMask(ToolStripItem.stateSupportsDisabledHotTracking);

		// Token: 0x040013D8 RID: 5080
		private static readonly int stateDisposing = BitVector32.CreateMask(ToolStripItem.stateUseAmbientMargin);

		// Token: 0x040013D9 RID: 5081
		private long lastClickTime;

		// Token: 0x02000249 RID: 585
		[ComVisible(true)]
		public class ToolStripItemAccessibleObject : AccessibleObject
		{
			// Token: 0x06001E33 RID: 7731 RVA: 0x0003E329 File Offset: 0x0003D329
			public ToolStripItemAccessibleObject(ToolStripItem ownerItem)
			{
				if (ownerItem == null)
				{
					throw new ArgumentNullException("ownerItem");
				}
				this.ownerItem = ownerItem;
			}

			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0003E348 File Offset: 0x0003D348
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = this.ownerItem.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					return SR.GetString("AccessibleActionPress");
				}
			}

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0003E370 File Offset: 0x0003D370
			public override string Description
			{
				get
				{
					string accessibleDescription = this.ownerItem.AccessibleDescription;
					if (accessibleDescription != null)
					{
						return accessibleDescription;
					}
					return base.Description;
				}
			}

			// Token: 0x17000425 RID: 1061
			// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0003E394 File Offset: 0x0003D394
			public override string Help
			{
				get
				{
					QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[ToolStripItem.EventQueryAccessibilityHelp];
					if (queryAccessibilityHelpEventHandler != null)
					{
						QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
						queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
						return queryAccessibilityHelpEventArgs.HelpString;
					}
					return base.Help;
				}
			}

			// Token: 0x17000426 RID: 1062
			// (get) Token: 0x06001E37 RID: 7735 RVA: 0x0003E3E0 File Offset: 0x0003D3E0
			public override string KeyboardShortcut
			{
				get
				{
					char mnemonic = WindowsFormsUtils.GetMnemonic(this.ownerItem.Text, false);
					if (this.ownerItem.IsOnDropDown)
					{
						if (mnemonic != '\0')
						{
							return mnemonic.ToString();
						}
						return string.Empty;
					}
					else
					{
						if (mnemonic != '\0')
						{
							return "Alt+" + mnemonic;
						}
						return string.Empty;
					}
				}
			}

			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0003E438 File Offset: 0x0003D438
			// (set) Token: 0x06001E39 RID: 7737 RVA: 0x0003E47A File Offset: 0x0003D47A
			public override string Name
			{
				get
				{
					string accessibleName = this.ownerItem.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					string name = base.Name;
					if (name == null || name.Length == 0)
					{
						return WindowsFormsUtils.TextWithoutMnemonics(this.ownerItem.Text);
					}
					return name;
				}
				set
				{
					this.ownerItem.AccessibleName = value;
				}
			}

			// Token: 0x17000428 RID: 1064
			// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0003E488 File Offset: 0x0003D488
			internal ToolStripItem Owner
			{
				get
				{
					return this.ownerItem;
				}
			}

			// Token: 0x17000429 RID: 1065
			// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0003E490 File Offset: 0x0003D490
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerItem.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.PushButton;
				}
			}

			// Token: 0x1700042A RID: 1066
			// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0003E4B4 File Offset: 0x0003D4B4
			public override AccessibleStates State
			{
				get
				{
					if (!this.ownerItem.CanSelect)
					{
						return base.State | this.additionalState;
					}
					if (!this.ownerItem.Enabled)
					{
						return AccessibleStates.Unavailable | this.additionalState;
					}
					AccessibleStates accessibleStates = AccessibleStates.Focusable | this.additionalState;
					if (this.ownerItem.Selected || this.ownerItem.Pressed)
					{
						accessibleStates |= AccessibleStates.Focused | AccessibleStates.HotTracked;
					}
					if (this.ownerItem.Pressed)
					{
						accessibleStates |= AccessibleStates.Pressed;
					}
					return accessibleStates;
				}
			}

			// Token: 0x06001E3D RID: 7741 RVA: 0x0003E533 File Offset: 0x0003D533
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.Owner != null)
				{
					this.Owner.PerformClick();
				}
			}

			// Token: 0x06001E3E RID: 7742 RVA: 0x0003E548 File Offset: 0x0003D548
			public override int GetHelpTopic(out string fileName)
			{
				int num = 0;
				QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)this.Owner.Events[ToolStripItem.EventQueryAccessibilityHelp];
				if (queryAccessibilityHelpEventHandler != null)
				{
					QueryAccessibilityHelpEventArgs queryAccessibilityHelpEventArgs = new QueryAccessibilityHelpEventArgs();
					queryAccessibilityHelpEventHandler(this.Owner, queryAccessibilityHelpEventArgs);
					fileName = queryAccessibilityHelpEventArgs.HelpNamespace;
					if (fileName != null && fileName.Length > 0)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.PathDiscovery, fileName);
					}
					try
					{
						num = int.Parse(queryAccessibilityHelpEventArgs.HelpKeyword, CultureInfo.InvariantCulture);
					}
					catch
					{
					}
					return num;
				}
				return base.GetHelpTopic(out fileName);
			}

			// Token: 0x06001E3F RID: 7743 RVA: 0x0003E5D8 File Offset: 0x0003D5D8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				ToolStripItem toolStripItem = null;
				if (this.Owner != null)
				{
					ToolStrip parentInternal = this.Owner.ParentInternal;
					if (parentInternal == null)
					{
						return null;
					}
					RightToLeft rightToLeft = parentInternal.RightToLeft;
					switch (navigationDirection)
					{
					case AccessibleNavigation.Up:
						toolStripItem = (this.Owner.IsOnDropDown ? parentInternal.GetNextItem(this.Owner, ArrowDirection.Up) : parentInternal.GetNextItem(this.Owner, ArrowDirection.Left, true));
						break;
					case AccessibleNavigation.Down:
						toolStripItem = (this.Owner.IsOnDropDown ? parentInternal.GetNextItem(this.Owner, ArrowDirection.Down) : parentInternal.GetNextItem(this.Owner, ArrowDirection.Right, true));
						break;
					case AccessibleNavigation.Left:
					case AccessibleNavigation.Previous:
						toolStripItem = parentInternal.GetNextItem(this.Owner, ArrowDirection.Left, true);
						break;
					case AccessibleNavigation.Right:
					case AccessibleNavigation.Next:
						toolStripItem = parentInternal.GetNextItem(this.Owner, ArrowDirection.Right, true);
						break;
					case AccessibleNavigation.FirstChild:
						toolStripItem = parentInternal.GetNextItem(null, ArrowDirection.Right, true);
						break;
					case AccessibleNavigation.LastChild:
						toolStripItem = parentInternal.GetNextItem(null, ArrowDirection.Left, true);
						break;
					}
				}
				if (toolStripItem != null)
				{
					return toolStripItem.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06001E40 RID: 7744 RVA: 0x0003E6DD File Offset: 0x0003D6DD
			public void AddState(AccessibleStates state)
			{
				if (state == AccessibleStates.None)
				{
					this.additionalState = state;
					return;
				}
				this.additionalState |= state;
			}

			// Token: 0x06001E41 RID: 7745 RVA: 0x0003E6F8 File Offset: 0x0003D6F8
			public override string ToString()
			{
				if (this.Owner != null)
				{
					return "ToolStripItemAccessibleObject: Owner = " + this.Owner.ToString();
				}
				return "ToolStripItemAccessibleObject: Owner = null";
			}

			// Token: 0x1700042B RID: 1067
			// (get) Token: 0x06001E42 RID: 7746 RVA: 0x0003E720 File Offset: 0x0003D720
			public override Rectangle Bounds
			{
				get
				{
					Rectangle bounds = this.Owner.Bounds;
					if (this.Owner.ParentInternal != null)
					{
						return new Rectangle(this.Owner.ParentInternal.PointToScreen(bounds.Location), bounds.Size);
					}
					return Rectangle.Empty;
				}
			}

			// Token: 0x1700042C RID: 1068
			// (get) Token: 0x06001E43 RID: 7747 RVA: 0x0003E770 File Offset: 0x0003D770
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.Owner.IsOnDropDown)
					{
						ToolStripDropDown currentParentDropDown = this.Owner.GetCurrentParentDropDown();
						if (currentParentDropDown.OwnerItem != null)
						{
							return currentParentDropDown.OwnerItem.AccessibilityObject;
						}
						return currentParentDropDown.AccessibilityObject;
					}
					else
					{
						if (this.Owner.Parent == null)
						{
							return base.Parent;
						}
						return this.Owner.Parent.AccessibilityObject;
					}
				}
			}

			// Token: 0x040013DA RID: 5082
			private ToolStripItem ownerItem;

			// Token: 0x040013DB RID: 5083
			private AccessibleStates additionalState;
		}
	}
}
