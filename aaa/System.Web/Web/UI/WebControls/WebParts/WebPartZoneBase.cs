using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200074A RID: 1866
	[Designer("System.Web.UI.Design.WebControls.WebParts.WebPartZoneBaseDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WebPartZoneBase : WebZone, IPostBackEventHandler, IWebPartMenuUser
	{
		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x0016C804 File Offset: 0x0016B804
		// (set) Token: 0x06005A5F RID: 23135 RVA: 0x0016C82D File Offset: 0x0016B82D
		[WebCategory("Behavior")]
		[Themeable(false)]
		[DefaultValue(true)]
		[WebSysDescription("WebPartZoneBase_AllowLayoutChange")]
		public virtual bool AllowLayoutChange
		{
			get
			{
				object obj = this.ViewState["AllowLayoutChange"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["AllowLayoutChange"] = value;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06005A60 RID: 23136 RVA: 0x0016C845 File Offset: 0x0016B845
		// (set) Token: 0x06005A61 RID: 23137 RVA: 0x0016C85B File Offset: 0x0016B85B
		[DefaultValue(typeof(Color), "Gray")]
		public override Color BorderColor
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return Color.Gray;
				}
				return base.BorderColor;
			}
			set
			{
				base.BorderColor = value;
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06005A62 RID: 23138 RVA: 0x0016C864 File Offset: 0x0016B864
		// (set) Token: 0x06005A63 RID: 23139 RVA: 0x0016C876 File Offset: 0x0016B876
		[DefaultValue(BorderStyle.Solid)]
		public override BorderStyle BorderStyle
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return BorderStyle.Solid;
				}
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x0016C87F File Offset: 0x0016B87F
		// (set) Token: 0x06005A65 RID: 23141 RVA: 0x0016C896 File Offset: 0x0016B896
		[DefaultValue(typeof(Unit), "1")]
		public override Unit BorderWidth
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return 1;
				}
				return base.BorderWidth;
			}
			set
			{
				base.BorderWidth = value;
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x0016C89F File Offset: 0x0016B89F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[WebSysDescription("WebPartZoneBase_CloseVerb")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb CloseVerb
		{
			get
			{
				if (this._closeVerb == null)
				{
					this._closeVerb = new WebPartCloseVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._closeVerb).TrackViewState();
					}
				}
				return this._closeVerb;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x06005A67 RID: 23143 RVA: 0x0016C8CD File Offset: 0x0016B8CD
		[DefaultValue(null)]
		[WebSysDescription("WebPartZoneBase_ConnectVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb ConnectVerb
		{
			get
			{
				if (this._connectVerb == null)
				{
					this._connectVerb = new WebPartConnectVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._connectVerb).TrackViewState();
					}
				}
				return this._connectVerb;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x06005A68 RID: 23144 RVA: 0x0016C8FB File Offset: 0x0016B8FB
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		[WebSysDescription("WebPartZoneBase_DeleteVerb")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public virtual WebPartVerb DeleteVerb
		{
			get
			{
				if (this._deleteVerb == null)
				{
					this._deleteVerb = new WebPartDeleteVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._deleteVerb).TrackViewState();
					}
				}
				return this._deleteVerb;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06005A69 RID: 23145 RVA: 0x0016C92C File Offset: 0x0016B92C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string DisplayTitle
		{
			get
			{
				string headerText = this.HeaderText;
				if (!string.IsNullOrEmpty(headerText))
				{
					return headerText;
				}
				string id = this.ID;
				if (!string.IsNullOrEmpty(id))
				{
					return id;
				}
				int num = 1;
				if (base.WebPartManager != null)
				{
					num = base.WebPartManager.Zones.IndexOf(this) + 1;
				}
				return SR.GetString("WebPartZoneBase_DisplayTitleFallback", new object[] { num.ToString(CultureInfo.CurrentCulture) });
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x0016C99A File Offset: 0x0016B99A
		protected internal bool DragDropEnabled
		{
			get
			{
				return !base.DesignMode && base.RenderClientScript && this.AllowLayoutChange && base.WebPartManager != null && base.WebPartManager.DisplayMode.AllowPageDesign;
			}
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06005A6B RID: 23147 RVA: 0x0016C9D0 File Offset: 0x0016B9D0
		// (set) Token: 0x06005A6C RID: 23148 RVA: 0x0016CA08 File Offset: 0x0016BA08
		[TypeConverter(typeof(WebColorConverter))]
		[WebSysDescription("WebPartZoneBase_DragHighlightColor")]
		[WebCategory("Appearance")]
		[DefaultValue(typeof(Color), "Blue")]
		public virtual Color DragHighlightColor
		{
			get
			{
				object obj = this.ViewState["DragHighlightColor"];
				if (obj != null)
				{
					Color color = (Color)obj;
					if (!color.IsEmpty)
					{
						return color;
					}
				}
				return Color.Blue;
			}
			set
			{
				this.ViewState["DragHighlightColor"] = value;
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06005A6D RID: 23149 RVA: 0x0016CA20 File Offset: 0x0016BA20
		[DefaultValue(null)]
		[WebCategory("Verbs")]
		[NotifyParentProperty(true)]
		[WebSysDescription("WebPartZoneBase_EditVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual WebPartVerb EditVerb
		{
			get
			{
				if (this._editVerb == null)
				{
					this._editVerb = new WebPartEditVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._editVerb).TrackViewState();
					}
				}
				return this._editVerb;
			}
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x0016CA50 File Offset: 0x0016BA50
		// (set) Token: 0x06005A6F RID: 23151 RVA: 0x0016CA82 File Offset: 0x0016BA82
		[WebSysDefaultValue("WebPartZoneBase_DefaultEmptyZoneText")]
		public override string EmptyZoneText
		{
			get
			{
				string text = (string)this.ViewState["EmptyZoneText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("WebPartZoneBase_DefaultEmptyZoneText");
			}
			set
			{
				this.ViewState["EmptyZoneText"] = value;
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x0016CA95 File Offset: 0x0016BA95
		[DefaultValue(null)]
		[WebCategory("Verbs")]
		[WebSysDescription("WebPartZoneBase_ExportVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual WebPartVerb ExportVerb
		{
			get
			{
				if (this._exportVerb == null)
				{
					this._exportVerb = new WebPartExportVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._exportVerb).TrackViewState();
					}
				}
				return this._exportVerb;
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x06005A71 RID: 23153 RVA: 0x0016CAC3 File Offset: 0x0016BAC3
		protected override bool HasFooter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06005A72 RID: 23154 RVA: 0x0016CAC8 File Offset: 0x0016BAC8
		protected override bool HasHeader
		{
			get
			{
				bool flag = false;
				if (base.DesignMode)
				{
					flag = true;
				}
				else if (base.WebPartManager != null)
				{
					flag = base.WebPartManager.DisplayMode.AllowPageDesign;
				}
				return flag;
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06005A73 RID: 23155 RVA: 0x0016CAFD File Offset: 0x0016BAFD
		[DefaultValue(null)]
		[WebCategory("Verbs")]
		[WebSysDescription("WebPartZoneBase_HelpVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual WebPartVerb HelpVerb
		{
			get
			{
				if (this._helpVerb == null)
				{
					this._helpVerb = new WebPartHelpVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._helpVerb).TrackViewState();
					}
				}
				return this._helpVerb;
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06005A74 RID: 23156 RVA: 0x0016CB2B File Offset: 0x0016BB2B
		internal WebPartMenu Menu
		{
			get
			{
				if (this._menu == null)
				{
					this._menu = new WebPartMenu(this);
				}
				return this._menu;
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06005A75 RID: 23157 RVA: 0x0016CB47 File Offset: 0x0016BB47
		[WebSysDescription("WebPartZoneBase_MenuCheckImageStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style MenuCheckImageStyle
		{
			get
			{
				if (this._menuCheckImageStyle == null)
				{
					this._menuCheckImageStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuCheckImageStyle).TrackViewState();
					}
				}
				return this._menuCheckImageStyle;
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06005A76 RID: 23158 RVA: 0x0016CB78 File Offset: 0x0016BB78
		// (set) Token: 0x06005A77 RID: 23159 RVA: 0x0016CBA5 File Offset: 0x0016BBA5
		[DefaultValue("")]
		[WebSysDescription("WebPartZoneBase_MenuCheckImageUrl")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		public virtual string MenuCheckImageUrl
		{
			get
			{
				string text = (string)this.ViewState["MenuCheckImageUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["MenuCheckImageUrl"] = value;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06005A78 RID: 23160 RVA: 0x0016CBB8 File Offset: 0x0016BBB8
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("WebPartZoneBase_MenuLabelHoverStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style MenuLabelHoverStyle
		{
			get
			{
				if (this._menuLabelHoverStyle == null)
				{
					this._menuLabelHoverStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuLabelHoverStyle).TrackViewState();
					}
				}
				return this._menuLabelHoverStyle;
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06005A79 RID: 23161 RVA: 0x0016CBE6 File Offset: 0x0016BBE6
		[WebSysDescription("WebPartZoneBase_MenuLabelStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style MenuLabelStyle
		{
			get
			{
				if (this._menuLabelStyle == null)
				{
					this._menuLabelStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuLabelStyle).TrackViewState();
					}
				}
				return this._menuLabelStyle;
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06005A7A RID: 23162 RVA: 0x0016CC14 File Offset: 0x0016BC14
		// (set) Token: 0x06005A7B RID: 23163 RVA: 0x0016CC41 File Offset: 0x0016BC41
		[WebCategory("Appearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[WebSysDescription("WebPartZoneBase_MenuLabelText")]
		public virtual string MenuLabelText
		{
			get
			{
				string text = (string)this.ViewState["MenuLabelText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["MenuLabelText"] = value;
			}
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x06005A7C RID: 23164 RVA: 0x0016CC54 File Offset: 0x0016BC54
		// (set) Token: 0x06005A7D RID: 23165 RVA: 0x0016CC81 File Offset: 0x0016BC81
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("WebPartZoneBase_MenuPopupImageUrl")]
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Appearance")]
		public virtual string MenuPopupImageUrl
		{
			get
			{
				string text = (string)this.ViewState["MenuPopupImageUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["MenuPopupImageUrl"] = value;
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06005A7E RID: 23166 RVA: 0x0016CC94 File Offset: 0x0016BC94
		[WebSysDescription("WebPartZoneBase_MenuPopupStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public WebPartMenuStyle MenuPopupStyle
		{
			get
			{
				if (this._menuPopupStyle == null)
				{
					this._menuPopupStyle = new WebPartMenuStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuPopupStyle).TrackViewState();
					}
				}
				return this._menuPopupStyle;
			}
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06005A7F RID: 23167 RVA: 0x0016CCC2 File Offset: 0x0016BCC2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[WebSysDescription("WebPartZoneBase_MenuVerbHoverStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		public Style MenuVerbHoverStyle
		{
			get
			{
				if (this._menuVerbHoverStyle == null)
				{
					this._menuVerbHoverStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuVerbHoverStyle).TrackViewState();
					}
				}
				return this._menuVerbHoverStyle;
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06005A80 RID: 23168 RVA: 0x0016CCF0 File Offset: 0x0016BCF0
		[WebSysDescription("WebPartZoneBase_MenuVerbStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style MenuVerbStyle
		{
			get
			{
				if (this._menuVerbStyle == null)
				{
					this._menuVerbStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._menuVerbStyle).TrackViewState();
					}
				}
				return this._menuVerbStyle;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06005A81 RID: 23169 RVA: 0x0016CD1E File Offset: 0x0016BD1E
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("WebPartZoneBase_MinimizeVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb MinimizeVerb
		{
			get
			{
				if (this._minimizeVerb == null)
				{
					this._minimizeVerb = new WebPartMinimizeVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._minimizeVerb).TrackViewState();
					}
				}
				return this._minimizeVerb;
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06005A82 RID: 23170 RVA: 0x0016CD4C File Offset: 0x0016BD4C
		// (set) Token: 0x06005A83 RID: 23171 RVA: 0x0016CD75 File Offset: 0x0016BD75
		[WebCategory("Layout")]
		[DefaultValue(Orientation.Vertical)]
		[WebSysDescription("WebPartZoneBase_LayoutOrientation")]
		public virtual Orientation LayoutOrientation
		{
			get
			{
				object obj = this.ViewState["LayoutOrientation"];
				if (obj == null)
				{
					return Orientation.Vertical;
				}
				return (Orientation)((int)obj);
			}
			set
			{
				if (value < Orientation.Horizontal || value > Orientation.Vertical)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["LayoutOrientation"] = (int)value;
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06005A84 RID: 23172 RVA: 0x0016CDA0 File Offset: 0x0016BDA0
		[DefaultValue(null)]
		[WebSysDescription("WebPartZoneBase_RestoreVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb RestoreVerb
		{
			get
			{
				if (this._restoreVerb == null)
				{
					this._restoreVerb = new WebPartRestoreVerb();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._restoreVerb).TrackViewState();
					}
				}
				return this._restoreVerb;
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06005A85 RID: 23173 RVA: 0x0016CDCE File Offset: 0x0016BDCE
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("WebPartZoneBase_SelectedPartChromeStyle")]
		[DefaultValue(null)]
		[WebCategory("WebPart")]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Style SelectedPartChromeStyle
		{
			get
			{
				if (this._selectedPartChromeStyle == null)
				{
					this._selectedPartChromeStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._selectedPartChromeStyle).TrackViewState();
					}
				}
				return this._selectedPartChromeStyle;
			}
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x06005A86 RID: 23174 RVA: 0x0016CDFC File Offset: 0x0016BDFC
		// (set) Token: 0x06005A87 RID: 23175 RVA: 0x0016CE25 File Offset: 0x0016BE25
		[DefaultValue(true)]
		[WebSysDescription("WebPartZoneBase_ShowTitleIcons")]
		[WebCategory("WebPart")]
		public virtual bool ShowTitleIcons
		{
			get
			{
				object obj = this.ViewState["ShowTitleIcons"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowTitleIcons"] = value;
			}
		}

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x06005A88 RID: 23176 RVA: 0x0016CE40 File Offset: 0x0016BE40
		// (set) Token: 0x06005A89 RID: 23177 RVA: 0x0016CE69 File Offset: 0x0016BE69
		[WebSysDescription("WebPartZoneBase_TitleBarVerbButtonType")]
		[DefaultValue(ButtonType.Image)]
		[WebCategory("Appearance")]
		public virtual ButtonType TitleBarVerbButtonType
		{
			get
			{
				object obj = this.ViewState["TitleBarVerbButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Image;
			}
			set
			{
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["TitleBarVerbButtonType"] = value;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06005A8A RID: 23178 RVA: 0x0016CE94 File Offset: 0x0016BE94
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Styles")]
		[WebSysDescription("WebPartZoneBase_TitleBarVerbStyle")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public Style TitleBarVerbStyle
		{
			get
			{
				if (this._titleBarVerbStyle == null)
				{
					this._titleBarVerbStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._titleBarVerbStyle).TrackViewState();
					}
				}
				return this._titleBarVerbStyle;
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x0016CEC2 File Offset: 0x0016BEC2
		// (set) Token: 0x06005A8C RID: 23180 RVA: 0x0016CECA File Offset: 0x0016BECA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		[Browsable(false)]
		public override ButtonType VerbButtonType
		{
			get
			{
				return base.VerbButtonType;
			}
			set
			{
				base.VerbButtonType = value;
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x0016CED3 File Offset: 0x0016BED3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebPartChrome WebPartChrome
		{
			get
			{
				if (this._webPartChrome == null)
				{
					this._webPartChrome = this.CreateWebPartChrome();
				}
				return this._webPartChrome;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x0016CEF0 File Offset: 0x0016BEF0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebPartCollection WebParts
		{
			get
			{
				if (base.DesignMode)
				{
					WebPart[] array = new WebPart[this.Controls.Count];
					this.Controls.CopyTo(array, 0);
					return new WebPartCollection(array);
				}
				WebPartCollection webPartCollection;
				if (base.WebPartManager != null)
				{
					webPartCollection = base.WebPartManager.GetWebPartsForZone(this);
				}
				else
				{
					webPartCollection = new WebPartCollection();
				}
				return webPartCollection;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x0016CF48 File Offset: 0x0016BF48
		// (set) Token: 0x06005A90 RID: 23184 RVA: 0x0016CF71 File Offset: 0x0016BF71
		[WebCategory("WebPart")]
		[WebSysDescription("WebPartZoneBase_WebPartVerbRenderMode")]
		[DefaultValue(WebPartVerbRenderMode.Menu)]
		public virtual WebPartVerbRenderMode WebPartVerbRenderMode
		{
			get
			{
				object obj = this.ViewState["WebPartVerbRenderMode"];
				if (obj == null)
				{
					return WebPartVerbRenderMode.Menu;
				}
				return (WebPartVerbRenderMode)((int)obj);
			}
			set
			{
				if (value < WebPartVerbRenderMode.Menu || value > WebPartVerbRenderMode.TitleBar)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["WebPartVerbRenderMode"] = (int)value;
			}
		}

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x06005A91 RID: 23185 RVA: 0x0016CF9C File Offset: 0x0016BF9C
		// (remove) Token: 0x06005A92 RID: 23186 RVA: 0x0016CFAF File Offset: 0x0016BFAF
		[WebSysDescription("WebPartZoneBase_CreateVerbs")]
		[WebCategory("Action")]
		public event WebPartVerbsEventHandler CreateVerbs
		{
			add
			{
				base.Events.AddHandler(WebPartZoneBase.CreateVerbsEvent, value);
			}
			remove
			{
				base.Events.RemoveHandler(WebPartZoneBase.CreateVerbsEvent, value);
			}
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x0016CFC2 File Offset: 0x0016BFC2
		protected virtual void CloseWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (base.WebPartManager != null && webPart.AllowClose && this.AllowLayoutChange)
			{
				base.WebPartManager.CloseWebPart(webPart);
			}
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x0016CFF8 File Offset: 0x0016BFF8
		protected virtual void ConnectWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (base.WebPartManager != null && base.WebPartManager.DisplayMode == WebPartManager.ConnectDisplayMode && webPart != base.WebPartManager.SelectedWebPart && webPart.AllowConnect)
			{
				base.WebPartManager.BeginWebPartConnecting(webPart);
			}
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x0016D050 File Offset: 0x0016C050
		protected internal override void CreateChildControls()
		{
			if (base.DesignMode)
			{
				this.Controls.Clear();
				WebPartCollection initialWebParts = this.GetInitialWebParts();
				foreach (object obj in initialWebParts)
				{
					WebPart webPart = (WebPart)obj;
					this.Controls.Add(webPart);
				}
			}
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x0016D0C4 File Offset: 0x0016C0C4
		protected override ControlCollection CreateControlCollection()
		{
			if (base.DesignMode)
			{
				return new ControlCollection(this);
			}
			return new EmptyControlCollection(this);
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x0016D0DC File Offset: 0x0016C0DC
		protected override Style CreateControlStyle()
		{
			return new Style
			{
				BorderColor = Color.Gray,
				BorderStyle = BorderStyle.Solid,
				BorderWidth = 1
			};
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x0016D10E File Offset: 0x0016C10E
		protected virtual WebPartChrome CreateWebPartChrome()
		{
			return new WebPartChrome(this, base.WebPartManager);
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x0016D11C File Offset: 0x0016C11C
		protected virtual void DeleteWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (base.WebPartManager != null && this.AllowLayoutChange)
			{
				base.WebPartManager.DeleteWebPart(webPart);
			}
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x0016D148 File Offset: 0x0016C148
		protected virtual void EditWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (base.WebPartManager != null && base.WebPartManager.DisplayMode == WebPartManager.EditDisplayMode && webPart != base.WebPartManager.SelectedWebPart)
			{
				base.WebPartManager.BeginWebPartEditing(webPart);
			}
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x0016D198 File Offset: 0x0016C198
		public override PartChromeType GetEffectiveChromeType(Part part)
		{
			PartChromeType partChromeType = base.GetEffectiveChromeType(part);
			if (base.WebPartManager != null && base.WebPartManager.DisplayMode.AllowPageDesign)
			{
				if (partChromeType == PartChromeType.None)
				{
					partChromeType = PartChromeType.TitleOnly;
				}
				else if (partChromeType == PartChromeType.BorderOnly)
				{
					partChromeType = PartChromeType.TitleAndBorder;
				}
			}
			return partChromeType;
		}

		// Token: 0x06005A9C RID: 23196
		protected internal abstract WebPartCollection GetInitialWebParts();

		// Token: 0x06005A9D RID: 23197 RVA: 0x0016D1D8 File Offset: 0x0016C1D8
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 18)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.SelectedPartChromeStyle).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.CloseVerb).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.ConnectVerb).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.DeleteVerb).LoadViewState(array[4]);
			}
			if (array[5] != null)
			{
				((IStateManager)this.EditVerb).LoadViewState(array[5]);
			}
			if (array[6] != null)
			{
				((IStateManager)this.HelpVerb).LoadViewState(array[6]);
			}
			if (array[7] != null)
			{
				((IStateManager)this.MinimizeVerb).LoadViewState(array[7]);
			}
			if (array[8] != null)
			{
				((IStateManager)this.RestoreVerb).LoadViewState(array[8]);
			}
			if (array[9] != null)
			{
				((IStateManager)this.ExportVerb).LoadViewState(array[9]);
			}
			if (array[10] != null)
			{
				((IStateManager)this.MenuPopupStyle).LoadViewState(array[10]);
			}
			if (array[11] != null)
			{
				((IStateManager)this.MenuLabelStyle).LoadViewState(array[11]);
			}
			if (array[12] != null)
			{
				((IStateManager)this.MenuLabelHoverStyle).LoadViewState(array[12]);
			}
			if (array[13] != null)
			{
				((IStateManager)this.MenuCheckImageStyle).LoadViewState(array[13]);
			}
			if (array[14] != null)
			{
				((IStateManager)this.MenuVerbStyle).LoadViewState(array[14]);
			}
			if (array[15] != null)
			{
				((IStateManager)this.MenuVerbHoverStyle).LoadViewState(array[15]);
			}
			if (array[16] != null)
			{
				((IStateManager)base.ControlStyle).LoadViewState(array[16]);
			}
			if (array[17] != null)
			{
				((IStateManager)this.TitleBarVerbStyle).LoadViewState(array[17]);
			}
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x0016D36C File Offset: 0x0016C36C
		private void CreateZoneVerbs()
		{
			WebPartVerbsEventArgs webPartVerbsEventArgs = new WebPartVerbsEventArgs();
			this.OnCreateVerbs(webPartVerbsEventArgs);
			this._verbs = webPartVerbsEventArgs.Verbs;
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x0016D392 File Offset: 0x0016C392
		private bool IsDefaultVerbEvent(string[] eventArguments)
		{
			return eventArguments.Length == 2;
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x0016D39A File Offset: 0x0016C39A
		private bool IsDragEvent(string[] eventArguments)
		{
			return eventArguments.Length == 3 && string.Equals(eventArguments[0], "Drag", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005AA1 RID: 23201 RVA: 0x0016D3B2 File Offset: 0x0016C3B2
		private bool IsPartVerbEvent(string[] eventArguments)
		{
			return eventArguments.Length == 3 && string.Equals(eventArguments[0], "partverb", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x0016D3CA File Offset: 0x0016C3CA
		private bool IsZoneVerbEvent(string[] eventArguments)
		{
			return eventArguments.Length == 3 && string.Equals(eventArguments[0], "zoneverb", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x0016D3E2 File Offset: 0x0016C3E2
		protected virtual void MinimizeWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (webPart.ChromeState == PartChromeState.Normal && webPart.AllowMinimize && this.AllowLayoutChange)
			{
				webPart.ChromeState = PartChromeState.Minimized;
			}
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x0016D414 File Offset: 0x0016C414
		protected virtual void OnCreateVerbs(WebPartVerbsEventArgs e)
		{
			WebPartVerbsEventHandler webPartVerbsEventHandler = (WebPartVerbsEventHandler)base.Events[WebPartZoneBase.CreateVerbsEvent];
			if (webPartVerbsEventHandler != null)
			{
				webPartVerbsEventHandler(this, e);
			}
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x0016D442 File Offset: 0x0016C442
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.CreateZoneVerbs();
			this.WebPartChrome.PerformPreRender();
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x0016D45C File Offset: 0x0016C45C
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			if (string.IsNullOrEmpty(eventArgument))
			{
				return;
			}
			string[] array = eventArgument.Split(new char[] { ':' });
			if (!this.IsDragEvent(array))
			{
				base.ValidateEvent(this.UniqueID, eventArgument);
			}
			if (base.WebPartManager == null)
			{
				return;
			}
			WebPartCollection webParts = base.WebPartManager.WebParts;
			if (this.IsDefaultVerbEvent(array))
			{
				string text = array[0];
				string text2 = array[1];
				WebPart webPart = webParts[text2];
				if (webPart != null && !webPart.IsClosed)
				{
					if (string.Equals(text, "close", StringComparison.OrdinalIgnoreCase))
					{
						if (this.CloseVerb.Visible && this.CloseVerb.Enabled)
						{
							this.CloseWebPart(webPart);
							return;
						}
					}
					else if (string.Equals(text, "connect", StringComparison.OrdinalIgnoreCase))
					{
						if (this.ConnectVerb.Visible && this.ConnectVerb.Enabled)
						{
							this.ConnectWebPart(webPart);
							return;
						}
					}
					else if (string.Equals(text, "delete", StringComparison.OrdinalIgnoreCase))
					{
						if (this.DeleteVerb.Visible && this.DeleteVerb.Enabled)
						{
							this.DeleteWebPart(webPart);
							return;
						}
					}
					else if (string.Equals(text, "edit", StringComparison.OrdinalIgnoreCase))
					{
						if (this.EditVerb.Visible && this.EditVerb.Enabled)
						{
							this.EditWebPart(webPart);
							return;
						}
					}
					else if (string.Equals(text, "minimize", StringComparison.OrdinalIgnoreCase))
					{
						if (this.MinimizeVerb.Visible && this.MinimizeVerb.Enabled)
						{
							this.MinimizeWebPart(webPart);
							return;
						}
					}
					else if (string.Equals(text, "restore", StringComparison.OrdinalIgnoreCase) && this.RestoreVerb.Visible && this.RestoreVerb.Enabled)
					{
						this.RestoreWebPart(webPart);
						return;
					}
				}
			}
			else if (this.IsDragEvent(array))
			{
				string text3 = array[1];
				string text4 = null;
				if (text3.StartsWith("WebPart_", StringComparison.Ordinal))
				{
					text4 = text3.Substring("WebPart_".Length);
				}
				int num = int.Parse(array[2], CultureInfo.InvariantCulture);
				WebPart webPart2 = webParts[text4];
				if (webPart2 != null && !webPart2.IsClosed)
				{
					if (this.WebParts.Contains(webPart2) && webPart2.ZoneIndex < num)
					{
						num--;
					}
					WebPartZoneBase zone = webPart2.Zone;
					if (this.AllowLayoutChange && base.WebPartManager.DisplayMode.AllowPageDesign && zone != null && zone.AllowLayoutChange && (webPart2.AllowZoneChange || zone == this))
					{
						base.WebPartManager.MoveWebPart(webPart2, this, num);
						return;
					}
				}
			}
			else if (this.IsPartVerbEvent(array))
			{
				string text5 = array[1];
				string text6 = array[2];
				WebPart webPart3 = webParts[text6];
				if (webPart3 != null && !webPart3.IsClosed)
				{
					WebPartVerb webPartVerb = webPart3.Verbs[text5];
					if (webPartVerb != null && webPartVerb.Visible && webPartVerb.Enabled)
					{
						webPartVerb.ServerClickHandler(webPartVerb, new WebPartEventArgs(webPart3));
						return;
					}
				}
			}
			else if (this.IsZoneVerbEvent(array))
			{
				this.CreateZoneVerbs();
				string text7 = array[1];
				string text8 = array[2];
				WebPart webPart4 = webParts[text8];
				if (webPart4 != null && !webPart4.IsClosed)
				{
					WebPartVerb webPartVerb2 = this._verbs[text7];
					if (webPartVerb2 != null && webPartVerb2.Visible && webPartVerb2.Enabled)
					{
						webPartVerb2.ServerClickHandler(webPartVerb2, new WebPartEventArgs(webPart4));
					}
				}
			}
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x0016D7F0 File Offset: 0x0016C7F0
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this._borderColor = this.BorderColor;
			this._borderStyle = this.BorderStyle;
			this._borderWidth = this.BorderWidth;
			if (base.ControlStyleCreated)
			{
				this.BorderColor = Color.Empty;
				this.BorderStyle = BorderStyle.NotSet;
				this.BorderWidth = Unit.Empty;
			}
			base.Render(writer);
			if (base.ControlStyleCreated)
			{
				this.BorderColor = this._borderColor;
				this.BorderStyle = this._borderStyle;
				this.BorderWidth = this._borderWidth;
			}
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x0016D890 File Offset: 0x0016C890
		protected override void RenderBody(HtmlTextWriter writer)
		{
			Orientation layoutOrientation = this.LayoutOrientation;
			if ((base.DesignMode || (base.WebPartManager != null && base.WebPartManager.DisplayMode.AllowPageDesign)) && (this._borderColor != Color.Empty || this._borderStyle != BorderStyle.NotSet || this._borderWidth != Unit.Empty))
			{
				new Style
				{
					BorderColor = this._borderColor,
					BorderStyle = this._borderStyle,
					BorderWidth = this._borderWidth
				}.AddAttributesToRender(writer, this);
			}
			base.RenderBodyTableBeginTag(writer);
			if (base.DesignMode)
			{
				base.RenderDesignerRegionBeginTag(writer, layoutOrientation);
			}
			if (layoutOrientation == Orientation.Horizontal)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			}
			bool dragDropEnabled = this.DragDropEnabled;
			if (dragDropEnabled)
			{
				this.RenderDropCue(writer);
			}
			WebPartCollection webParts = this.WebParts;
			if (webParts == null || webParts.Count == 0)
			{
				this.RenderEmptyZoneBody(writer);
			}
			else
			{
				WebPartChrome webPartChrome = this.WebPartChrome;
				foreach (object obj in webParts)
				{
					WebPart webPart = (WebPart)obj;
					if (webPart.ChromeState == PartChromeState.Minimized)
					{
						PartChromeType effectiveChromeType = this.GetEffectiveChromeType(webPart);
						if (effectiveChromeType == PartChromeType.None || effectiveChromeType == PartChromeType.BorderOnly)
						{
							writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
						}
					}
					if (layoutOrientation == Orientation.Vertical)
					{
						writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					}
					else
					{
						writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
						writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					webPartChrome.RenderWebPart(writer, webPart);
					writer.RenderEndTag();
					if (layoutOrientation == Orientation.Vertical)
					{
						writer.RenderEndTag();
					}
					if (dragDropEnabled)
					{
						this.RenderDropCue(writer);
					}
				}
				if (layoutOrientation == Orientation.Vertical)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
					writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag();
					writer.RenderEndTag();
				}
				else
				{
					writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
					writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag();
				}
			}
			if (layoutOrientation == Orientation.Horizontal)
			{
				writer.RenderEndTag();
			}
			if (base.DesignMode)
			{
				WebZone.RenderDesignerRegionEndTag(writer);
			}
			WebZone.RenderBodyTableEndTag(writer);
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x0016DACC File Offset: 0x0016CACC
		protected virtual void RenderDropCue(HtmlTextWriter writer)
		{
			if (this.LayoutOrientation == Orientation.Vertical)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "1");
				writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingBottom, "1");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderDropCueIBar(writer, Orientation.Horizontal);
				writer.RenderEndTag();
				writer.RenderEndTag();
				return;
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "1");
			writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "1");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this.RenderDropCueIBar(writer, Orientation.Vertical);
			writer.RenderEndTag();
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x0016DB54 File Offset: 0x0016CB54
		private void RenderDropCueIBar(HtmlTextWriter writer, Orientation orientation)
		{
			string text = ColorTranslator.ToHtml(this.DragHighlightColor);
			string text2 = "solid 3px " + text;
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (orientation == Orientation.Horizontal)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
				writer.AddStyleAttribute("border-left", text2);
				writer.AddStyleAttribute("border-right", text2);
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
				writer.AddStyleAttribute("border-top", text2);
				writer.AddStyleAttribute("border-bottom", text2);
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (orientation == Orientation.Vertical)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "0px");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			if (orientation == Orientation.Horizontal)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "2px 0px 2px 0px");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "2px");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0px 2px 0px 2px");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "2px");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, text);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x0016DCA8 File Offset: 0x0016CCA8
		private void RenderEmptyZoneBody(HtmlTextWriter writer)
		{
			bool flag = this.LayoutOrientation == Orientation.Vertical;
			bool flag2 = !flag;
			string emptyZoneText = this.EmptyZoneText;
			bool flag3 = !base.DesignMode && this.AllowLayoutChange && base.WebPartManager != null && base.WebPartManager.DisplayMode.AllowPageDesign && !string.IsNullOrEmpty(emptyZoneText);
			if (flag)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			}
			if (flag3)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			}
			if (flag2)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			if (flag3)
			{
				Style emptyZoneTextStyle = base.EmptyZoneTextStyle;
				if (!emptyZoneTextStyle.IsEmpty)
				{
					emptyZoneTextStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.Write(emptyZoneText);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
			if (flag)
			{
				writer.RenderEndTag();
			}
			if (flag3 && this.DragDropEnabled)
			{
				this.RenderDropCue(writer);
			}
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x0016DD98 File Offset: 0x0016CD98
		protected override void RenderHeader(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			TitleStyle headerStyle = base.HeaderStyle;
			if (!headerStyle.IsEmpty)
			{
				Style style = new Style();
				if (!headerStyle.ForeColor.IsEmpty)
				{
					style.ForeColor = headerStyle.ForeColor;
				}
				style.Font.CopyFrom(headerStyle.Font);
				if (!headerStyle.Font.Size.IsEmpty)
				{
					style.Font.Size = new FontUnit(new Unit(100.0, UnitType.Percentage));
				}
				if (!style.IsEmpty)
				{
					style.AddAttributesToRender(writer, this);
				}
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			HorizontalAlign horizontalAlign = headerStyle.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(HorizontalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Align, converter.ConvertToString(horizontalAlign));
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write(this.DisplayTitle);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x0016DECE File Offset: 0x0016CECE
		protected virtual void RestoreWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (webPart.ChromeState == PartChromeState.Minimized && this.AllowLayoutChange)
			{
				webPart.ChromeState = PartChromeState.Normal;
			}
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x0016DEF8 File Offset: 0x0016CEF8
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._selectedPartChromeStyle != null) ? ((IStateManager)this._selectedPartChromeStyle).SaveViewState() : null,
				(this._closeVerb != null) ? ((IStateManager)this._closeVerb).SaveViewState() : null,
				(this._connectVerb != null) ? ((IStateManager)this._connectVerb).SaveViewState() : null,
				(this._deleteVerb != null) ? ((IStateManager)this._deleteVerb).SaveViewState() : null,
				(this._editVerb != null) ? ((IStateManager)this._editVerb).SaveViewState() : null,
				(this._helpVerb != null) ? ((IStateManager)this._helpVerb).SaveViewState() : null,
				(this._minimizeVerb != null) ? ((IStateManager)this._minimizeVerb).SaveViewState() : null,
				(this._restoreVerb != null) ? ((IStateManager)this._restoreVerb).SaveViewState() : null,
				(this._exportVerb != null) ? ((IStateManager)this._exportVerb).SaveViewState() : null,
				(this._menuPopupStyle != null) ? ((IStateManager)this._menuPopupStyle).SaveViewState() : null,
				(this._menuLabelStyle != null) ? ((IStateManager)this._menuLabelStyle).SaveViewState() : null,
				(this._menuLabelHoverStyle != null) ? ((IStateManager)this._menuLabelHoverStyle).SaveViewState() : null,
				(this._menuCheckImageStyle != null) ? ((IStateManager)this._menuCheckImageStyle).SaveViewState() : null,
				(this._menuVerbStyle != null) ? ((IStateManager)this._menuVerbStyle).SaveViewState() : null,
				(this._menuVerbHoverStyle != null) ? ((IStateManager)this._menuVerbHoverStyle).SaveViewState() : null,
				base.ControlStyleCreated ? ((IStateManager)base.ControlStyle).SaveViewState() : null,
				(this._titleBarVerbStyle != null) ? ((IStateManager)this._titleBarVerbStyle).SaveViewState() : null
			};
			for (int i = 0; i < 18; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x0016E0E0 File Offset: 0x0016D0E0
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._selectedPartChromeStyle != null)
			{
				((IStateManager)this._selectedPartChromeStyle).TrackViewState();
			}
			if (this._closeVerb != null)
			{
				((IStateManager)this._closeVerb).TrackViewState();
			}
			if (this._connectVerb != null)
			{
				((IStateManager)this._connectVerb).TrackViewState();
			}
			if (this._deleteVerb != null)
			{
				((IStateManager)this._deleteVerb).TrackViewState();
			}
			if (this._editVerb != null)
			{
				((IStateManager)this._editVerb).TrackViewState();
			}
			if (this._helpVerb != null)
			{
				((IStateManager)this._helpVerb).TrackViewState();
			}
			if (this._minimizeVerb != null)
			{
				((IStateManager)this._minimizeVerb).TrackViewState();
			}
			if (this._restoreVerb != null)
			{
				((IStateManager)this._restoreVerb).TrackViewState();
			}
			if (this._exportVerb != null)
			{
				((IStateManager)this._exportVerb).TrackViewState();
			}
			if (this._menuPopupStyle != null)
			{
				((IStateManager)this._menuPopupStyle).TrackViewState();
			}
			if (this._menuLabelStyle != null)
			{
				((IStateManager)this._menuLabelStyle).TrackViewState();
			}
			if (this._menuLabelHoverStyle != null)
			{
				((IStateManager)this._menuLabelHoverStyle).TrackViewState();
			}
			if (this._menuCheckImageStyle != null)
			{
				((IStateManager)this._menuCheckImageStyle).TrackViewState();
			}
			if (this._menuVerbStyle != null)
			{
				((IStateManager)this._menuVerbStyle).TrackViewState();
			}
			if (this._menuVerbHoverStyle != null)
			{
				((IStateManager)this._menuVerbHoverStyle).TrackViewState();
			}
			if (base.ControlStyleCreated)
			{
				((IStateManager)base.ControlStyle).TrackViewState();
			}
			if (this._titleBarVerbStyle != null)
			{
				((IStateManager)this._titleBarVerbStyle).TrackViewState();
			}
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x0016E238 File Offset: 0x0016D238
		internal WebPartVerbCollection VerbsForWebPart(WebPart webPart)
		{
			WebPartVerbCollection webPartVerbCollection = new WebPartVerbCollection();
			WebPartVerbCollection verbs = webPart.Verbs;
			if (verbs != null)
			{
				foreach (object obj in verbs)
				{
					WebPartVerb webPartVerb = (WebPartVerb)obj;
					if (webPartVerb.ServerClickHandler != null)
					{
						webPartVerb.SetEventArgumentPrefix("partverb:");
					}
					webPartVerbCollection.Add(webPartVerb);
				}
			}
			if (this._verbs != null)
			{
				foreach (object obj2 in this._verbs)
				{
					WebPartVerb webPartVerb2 = (WebPartVerb)obj2;
					if (webPartVerb2.ServerClickHandler != null)
					{
						webPartVerb2.SetEventArgumentPrefix("zoneverb:");
					}
					webPartVerbCollection.Add(webPartVerb2);
				}
			}
			WebPartVerb minimizeVerb = this.MinimizeVerb;
			minimizeVerb.SetEventArgumentPrefix("minimize:");
			webPartVerbCollection.Add(minimizeVerb);
			WebPartVerb restoreVerb = this.RestoreVerb;
			restoreVerb.SetEventArgumentPrefix("restore:");
			webPartVerbCollection.Add(restoreVerb);
			WebPartVerb closeVerb = this.CloseVerb;
			closeVerb.SetEventArgumentPrefix("close:");
			webPartVerbCollection.Add(closeVerb);
			WebPartVerb deleteVerb = this.DeleteVerb;
			deleteVerb.SetEventArgumentPrefix("delete:");
			webPartVerbCollection.Add(deleteVerb);
			WebPartVerb editVerb = this.EditVerb;
			editVerb.SetEventArgumentPrefix("edit:");
			webPartVerbCollection.Add(editVerb);
			WebPartVerb connectVerb = this.ConnectVerb;
			connectVerb.SetEventArgumentPrefix("connect:");
			webPartVerbCollection.Add(connectVerb);
			webPartVerbCollection.Add(this.ExportVerb);
			webPartVerbCollection.Add(this.HelpVerb);
			return webPartVerbCollection;
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x0016E3EC File Offset: 0x0016D3EC
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x0016E3F5 File Offset: 0x0016D3F5
		Style IWebPartMenuUser.CheckImageStyle
		{
			get
			{
				return this._menuCheckImageStyle;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x0016E400 File Offset: 0x0016D400
		string IWebPartMenuUser.CheckImageUrl
		{
			get
			{
				string text = this.MenuCheckImageUrl;
				if (!string.IsNullOrEmpty(text))
				{
					text = base.ResolveClientUrl(text);
				}
				return text;
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x0016E425 File Offset: 0x0016D425
		string IWebPartMenuUser.ClientID
		{
			get
			{
				return this.ClientID;
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x0016E430 File Offset: 0x0016D430
		string IWebPartMenuUser.PopupImageUrl
		{
			get
			{
				string text = this.MenuPopupImageUrl;
				if (!string.IsNullOrEmpty(text))
				{
					text = base.ResolveClientUrl(text);
				}
				return text;
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x0016E455 File Offset: 0x0016D455
		Style IWebPartMenuUser.ItemHoverStyle
		{
			get
			{
				return this._menuVerbHoverStyle;
			}
		}

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x06005AB7 RID: 23223 RVA: 0x0016E45D File Offset: 0x0016D45D
		Style IWebPartMenuUser.ItemStyle
		{
			get
			{
				return this._menuVerbStyle;
			}
		}

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x0016E465 File Offset: 0x0016D465
		Style IWebPartMenuUser.LabelHoverStyle
		{
			get
			{
				return this._menuLabelHoverStyle;
			}
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x06005AB9 RID: 23225 RVA: 0x0016E46D File Offset: 0x0016D46D
		string IWebPartMenuUser.LabelImageUrl
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x06005ABA RID: 23226 RVA: 0x0016E470 File Offset: 0x0016D470
		Style IWebPartMenuUser.LabelStyle
		{
			get
			{
				return this.MenuLabelStyle;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x06005ABB RID: 23227 RVA: 0x0016E478 File Offset: 0x0016D478
		string IWebPartMenuUser.LabelText
		{
			get
			{
				return this.MenuLabelText;
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x06005ABC RID: 23228 RVA: 0x0016E480 File Offset: 0x0016D480
		WebPartMenuStyle IWebPartMenuUser.MenuPopupStyle
		{
			get
			{
				return this._menuPopupStyle;
			}
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x06005ABD RID: 23229 RVA: 0x0016E488 File Offset: 0x0016D488
		Page IWebPartMenuUser.Page
		{
			get
			{
				return this.Page;
			}
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x06005ABE RID: 23230 RVA: 0x0016E490 File Offset: 0x0016D490
		string IWebPartMenuUser.PostBackTarget
		{
			get
			{
				return this.UniqueID;
			}
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x06005ABF RID: 23231 RVA: 0x0016E498 File Offset: 0x0016D498
		IUrlResolutionService IWebPartMenuUser.UrlResolver
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x0016E49B File Offset: 0x0016D49B
		void IWebPartMenuUser.OnBeginRender(HtmlTextWriter writer)
		{
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x0016E49D File Offset: 0x0016D49D
		void IWebPartMenuUser.OnEndRender(HtmlTextWriter writer)
		{
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x0016E49F File Offset: 0x0016D49F
		// Note: this type is marked as 'beforefieldinit'.
		static WebPartZoneBase()
		{
			WebPartZoneBase.CreateVerbsEvent = new object();
		}

		// Token: 0x0400308B RID: 12427
		internal const string EventArgumentSeparator = ":";

		// Token: 0x0400308C RID: 12428
		private const char eventArgumentSeparatorChar = ':';

		// Token: 0x0400308D RID: 12429
		private const string dragEventArgument = "Drag";

		// Token: 0x0400308E RID: 12430
		private const string partVerbEventArgument = "partverb";

		// Token: 0x0400308F RID: 12431
		private const string zoneVerbEventArgument = "zoneverb";

		// Token: 0x04003090 RID: 12432
		private const string closeEventArgument = "close";

		// Token: 0x04003091 RID: 12433
		private const string connectEventArgument = "connect";

		// Token: 0x04003092 RID: 12434
		private const string deleteEventArgument = "delete";

		// Token: 0x04003093 RID: 12435
		private const string editEventArgument = "edit";

		// Token: 0x04003094 RID: 12436
		private const string minimizeEventArgument = "minimize";

		// Token: 0x04003095 RID: 12437
		private const string restoreEventArgument = "restore";

		// Token: 0x04003096 RID: 12438
		private const string partVerbEventArgumentWithSeparator = "partverb:";

		// Token: 0x04003097 RID: 12439
		private const string zoneVerbEventArgumentWithSeparator = "zoneverb:";

		// Token: 0x04003098 RID: 12440
		private const string connectEventArgumentWithSeparator = "connect:";

		// Token: 0x04003099 RID: 12441
		private const string editEventArgumentWithSeparator = "edit:";

		// Token: 0x0400309A RID: 12442
		private const string minimizeEventArgumentWithSeparator = "minimize:";

		// Token: 0x0400309B RID: 12443
		private const string restoreEventArgumentWithSeparator = "restore:";

		// Token: 0x0400309C RID: 12444
		private const string closeEventArgumentWithSeparator = "close:";

		// Token: 0x0400309D RID: 12445
		private const string deleteEventArgumentWithSeparator = "delete:";

		// Token: 0x0400309E RID: 12446
		private const int baseIndex = 0;

		// Token: 0x0400309F RID: 12447
		private const int selectedPartChromeStyleIndex = 1;

		// Token: 0x040030A0 RID: 12448
		private const int closeVerbIndex = 2;

		// Token: 0x040030A1 RID: 12449
		private const int connectVerbIndex = 3;

		// Token: 0x040030A2 RID: 12450
		private const int deleteVerbIndex = 4;

		// Token: 0x040030A3 RID: 12451
		private const int editVerbIndex = 5;

		// Token: 0x040030A4 RID: 12452
		private const int helpVerbIndex = 6;

		// Token: 0x040030A5 RID: 12453
		private const int minimizeVerbIndex = 7;

		// Token: 0x040030A6 RID: 12454
		private const int restoreVerbIndex = 8;

		// Token: 0x040030A7 RID: 12455
		private const int exportVerbIndex = 9;

		// Token: 0x040030A8 RID: 12456
		private const int menuPopupStyleIndex = 10;

		// Token: 0x040030A9 RID: 12457
		private const int menuLabelStyleIndex = 11;

		// Token: 0x040030AA RID: 12458
		private const int menuLabelHoverStyleIndex = 12;

		// Token: 0x040030AB RID: 12459
		private const int menuCheckImageStyleIndex = 13;

		// Token: 0x040030AC RID: 12460
		private const int menuVerbStyleIndex = 14;

		// Token: 0x040030AD RID: 12461
		private const int menuVerbHoverStyleIndex = 15;

		// Token: 0x040030AE RID: 12462
		private const int controlStyleIndex = 16;

		// Token: 0x040030AF RID: 12463
		private const int titleBarVerbStyleIndex = 17;

		// Token: 0x040030B0 RID: 12464
		private const int viewStateArrayLength = 18;

		// Token: 0x040030B2 RID: 12466
		private Style _selectedPartChromeStyle;

		// Token: 0x040030B3 RID: 12467
		private WebPartVerb _closeVerb;

		// Token: 0x040030B4 RID: 12468
		private WebPartVerb _connectVerb;

		// Token: 0x040030B5 RID: 12469
		private WebPartVerb _deleteVerb;

		// Token: 0x040030B6 RID: 12470
		private WebPartVerb _editVerb;

		// Token: 0x040030B7 RID: 12471
		private WebPartVerb _exportVerb;

		// Token: 0x040030B8 RID: 12472
		private WebPartVerb _helpVerb;

		// Token: 0x040030B9 RID: 12473
		private WebPartVerb _minimizeVerb;

		// Token: 0x040030BA RID: 12474
		private WebPartVerb _restoreVerb;

		// Token: 0x040030BB RID: 12475
		private WebPartVerbCollection _verbs;

		// Token: 0x040030BC RID: 12476
		private WebPartMenuStyle _menuPopupStyle;

		// Token: 0x040030BD RID: 12477
		private Style _menuLabelStyle;

		// Token: 0x040030BE RID: 12478
		private Style _menuLabelHoverStyle;

		// Token: 0x040030BF RID: 12479
		private Style _menuCheckImageStyle;

		// Token: 0x040030C0 RID: 12480
		private Style _menuVerbHoverStyle;

		// Token: 0x040030C1 RID: 12481
		private Style _menuVerbStyle;

		// Token: 0x040030C2 RID: 12482
		private Style _titleBarVerbStyle;

		// Token: 0x040030C3 RID: 12483
		private Color _borderColor;

		// Token: 0x040030C4 RID: 12484
		private BorderStyle _borderStyle;

		// Token: 0x040030C5 RID: 12485
		private Unit _borderWidth;

		// Token: 0x040030C6 RID: 12486
		private WebPartChrome _webPartChrome;

		// Token: 0x040030C7 RID: 12487
		private WebPartMenu _menu;
	}
}
