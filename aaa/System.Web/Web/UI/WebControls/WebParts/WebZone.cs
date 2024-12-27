using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006AA RID: 1706
	[Bindable(false)]
	[Designer("System.Web.UI.Design.WebControls.WebParts.WebZoneDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WebZone : CompositeControl
	{
		// Token: 0x06005352 RID: 21330 RVA: 0x001522F8 File Offset: 0x001512F8
		internal WebZone()
		{
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06005353 RID: 21331 RVA: 0x00152300 File Offset: 0x00151300
		// (set) Token: 0x06005354 RID: 21332 RVA: 0x0015232D File Offset: 0x0015132D
		[UrlProperty]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("WebControl_BackImageUrl")]
		[WebCategory("Appearance")]
		public virtual string BackImageUrl
		{
			get
			{
				string text = (string)this.ViewState["BackImageUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["BackImageUrl"] = value;
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06005355 RID: 21333 RVA: 0x00152340 File Offset: 0x00151340
		// (set) Token: 0x06005356 RID: 21334 RVA: 0x0015236D File Offset: 0x0015136D
		[Localizable(true)]
		[WebSysDescription("Zone_EmptyZoneText")]
		[WebSysDefaultValue("")]
		[WebCategory("Behavior")]
		public virtual string EmptyZoneText
		{
			get
			{
				string text = (string)this.ViewState["EmptyZoneText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["EmptyZoneText"] = value;
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x06005357 RID: 21335 RVA: 0x00152380 File Offset: 0x00151380
		[NotifyParentProperty(true)]
		[WebSysDescription("Zone_EmptyZoneTextStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style EmptyZoneTextStyle
		{
			get
			{
				if (this._emptyZoneTextStyle == null)
				{
					this._emptyZoneTextStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._emptyZoneTextStyle).TrackViewState();
					}
				}
				return this._emptyZoneTextStyle;
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06005358 RID: 21336 RVA: 0x001523AE File Offset: 0x001513AE
		[NotifyParentProperty(true)]
		[WebCategory("Styles")]
		[WebSysDescription("Zone_ErrorStyle")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		public Style ErrorStyle
		{
			get
			{
				if (this._errorStyle == null)
				{
					this._errorStyle = new ErrorStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._errorStyle).TrackViewState();
					}
				}
				return this._errorStyle;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06005359 RID: 21337 RVA: 0x001523DC File Offset: 0x001513DC
		[WebCategory("Styles")]
		[WebSysDescription("Zone_FooterStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TitleStyle FooterStyle
		{
			get
			{
				if (this._footerStyle == null)
				{
					this._footerStyle = new TitleStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._footerStyle).TrackViewState();
					}
				}
				return this._footerStyle;
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x0600535A RID: 21338 RVA: 0x0015240A File Offset: 0x0015140A
		protected virtual bool HasFooter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600535B RID: 21339 RVA: 0x0015240D File Offset: 0x0015140D
		protected virtual bool HasHeader
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x0600535C RID: 21340 RVA: 0x00152410 File Offset: 0x00151410
		// (set) Token: 0x0600535D RID: 21341 RVA: 0x0015243D File Offset: 0x0015143D
		[WebSysDefaultValue("")]
		[WebSysDescription("Zone_HeaderText")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public virtual string HeaderText
		{
			get
			{
				string text = (string)this.ViewState["HeaderText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["HeaderText"] = value;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x00152450 File Offset: 0x00151450
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("Zone_HeaderStyle")]
		public TitleStyle HeaderStyle
		{
			get
			{
				if (this._headerStyle == null)
				{
					this._headerStyle = new TitleStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._headerStyle).TrackViewState();
					}
				}
				return this._headerStyle;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x00152480 File Offset: 0x00151480
		// (set) Token: 0x06005360 RID: 21344 RVA: 0x001524AE File Offset: 0x001514AE
		[WebCategory("WebPart")]
		[WebSysDescription("Zone_PartChromePadding")]
		[DefaultValue(typeof(Unit), "5px")]
		public Unit PartChromePadding
		{
			get
			{
				object obj = this.ViewState["PartChromePadding"];
				if (obj != null)
				{
					return (Unit)obj;
				}
				return Unit.Pixel(5);
			}
			set
			{
				if (value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["PartChromePadding"] = value;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x001524E3 File Offset: 0x001514E3
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("WebPart")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Zone_PartChromeStyle")]
		public Style PartChromeStyle
		{
			get
			{
				if (this._partChromeStyle == null)
				{
					this._partChromeStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._partChromeStyle).TrackViewState();
					}
				}
				return this._partChromeStyle;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06005362 RID: 21346 RVA: 0x00152514 File Offset: 0x00151514
		// (set) Token: 0x06005363 RID: 21347 RVA: 0x0015253D File Offset: 0x0015153D
		[WebSysDescription("Zone_PartChromeType")]
		[DefaultValue(PartChromeType.Default)]
		[WebCategory("WebPart")]
		public virtual PartChromeType PartChromeType
		{
			get
			{
				object obj = this.ViewState["PartChromeType"];
				if (obj == null)
				{
					return PartChromeType.Default;
				}
				return (PartChromeType)((int)obj);
			}
			set
			{
				if (value < PartChromeType.Default || value > PartChromeType.BorderOnly)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["PartChromeType"] = (int)value;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06005364 RID: 21348 RVA: 0x00152568 File Offset: 0x00151568
		[WebCategory("WebPart")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Zone_PartStyle")]
		public TableStyle PartStyle
		{
			get
			{
				if (this._partStyle == null)
				{
					this._partStyle = new TableStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._partStyle).TrackViewState();
					}
				}
				return this._partStyle;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06005365 RID: 21349 RVA: 0x00152596 File Offset: 0x00151596
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("WebPart")]
		[WebSysDescription("Zone_PartTitleStyle")]
		public TitleStyle PartTitleStyle
		{
			get
			{
				if (this._partTitleStyle == null)
				{
					this._partTitleStyle = new TitleStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._partTitleStyle).TrackViewState();
					}
				}
				return this._partTitleStyle;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x001525C4 File Offset: 0x001515C4
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x001525C8 File Offset: 0x001515C8
		// (set) Token: 0x06005368 RID: 21352 RVA: 0x001525F1 File Offset: 0x001515F1
		[DefaultValue(2)]
		[WebCategory("Layout")]
		[WebSysDescription("Zone_Padding")]
		public virtual int Padding
		{
			get
			{
				object obj = this.ViewState["Padding"];
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
				this.ViewState["Padding"] = value;
			}
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x00152618 File Offset: 0x00151618
		internal void RenderBodyTableBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			int padding = this.Padding;
			if (padding >= 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, padding.ToString(CultureInfo.InvariantCulture));
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			string backImageUrl = this.BackImageUrl;
			if (backImageUrl.Trim().Length > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + base.ResolveClientUrl(backImageUrl) + ")");
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x001526AF File Offset: 0x001516AF
		internal static void RenderBodyTableEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001526B8 File Offset: 0x001516B8
		internal void RenderDesignerRegionBeginTag(HtmlTextWriter writer, Orientation orientation)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (orientation == Orientation.Horizontal)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.DesignerRegion, "0");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, this.Padding.ToString(CultureInfo.InvariantCulture));
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (orientation == Orientation.Vertical)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x00152759 File Offset: 0x00151759
		internal static void RenderDesignerRegionEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600536D RID: 21357 RVA: 0x00152770 File Offset: 0x00151770
		protected internal bool RenderClientScript
		{
			get
			{
				bool flag = false;
				if (base.DesignMode)
				{
					flag = true;
				}
				else if (this.WebPartManager != null)
				{
					flag = this.WebPartManager.RenderClientScript;
				}
				return flag;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x001527A0 File Offset: 0x001517A0
		// (set) Token: 0x0600536F RID: 21359 RVA: 0x001527C9 File Offset: 0x001517C9
		[DefaultValue(ButtonType.Button)]
		[WebSysDescription("Zone_VerbButtonType")]
		[WebCategory("Appearance")]
		public virtual ButtonType VerbButtonType
		{
			get
			{
				object obj = this.ViewState["VerbButtonType"];
				if (obj != null)
				{
					return (ButtonType)obj;
				}
				return ButtonType.Button;
			}
			set
			{
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["VerbButtonType"] = value;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06005370 RID: 21360 RVA: 0x001527F4 File Offset: 0x001517F4
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("Zone_VerbStyle")]
		public Style VerbStyle
		{
			get
			{
				if (this._verbStyle == null)
				{
					this._verbStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._verbStyle).TrackViewState();
					}
				}
				return this._verbStyle;
			}
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x00152824 File Offset: 0x00151824
		public virtual PartChromeType GetEffectiveChromeType(Part part)
		{
			if (part == null)
			{
				throw new ArgumentNullException("part");
			}
			PartChromeType partChromeType = part.ChromeType;
			if (partChromeType == PartChromeType.Default)
			{
				PartChromeType partChromeType2 = this.PartChromeType;
				if (partChromeType2 == PartChromeType.Default)
				{
					partChromeType = PartChromeType.TitleAndBorder;
				}
				else
				{
					partChromeType = partChromeType2;
				}
			}
			return partChromeType;
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x0015285C File Offset: 0x0015185C
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 9)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.EmptyZoneTextStyle).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.FooterStyle).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.PartStyle).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.PartChromeStyle).LoadViewState(array[4]);
			}
			if (array[5] != null)
			{
				((IStateManager)this.PartTitleStyle).LoadViewState(array[5]);
			}
			if (array[6] != null)
			{
				((IStateManager)this.HeaderStyle).LoadViewState(array[6]);
			}
			if (array[7] != null)
			{
				((IStateManager)this.VerbStyle).LoadViewState(array[7]);
			}
			if (array[8] != null)
			{
				((IStateManager)this.ErrorStyle).LoadViewState(array[8]);
			}
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x00152934 File Offset: 0x00151934
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page page = this.Page;
			if (page != null)
			{
				if (page.ControlState >= ControlState.Initialized && !base.DesignMode)
				{
					throw new InvalidOperationException(SR.GetString("Zone_AddedTooLate"));
				}
				if (!base.DesignMode)
				{
					this._webPartManager = WebPartManager.GetCurrentWebPartManager(page);
					if (this._webPartManager == null)
					{
						throw new InvalidOperationException(SR.GetString("WebPartManagerRequired"));
					}
					this._webPartManager.RegisterZone(this);
				}
			}
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x001529AC File Offset: 0x001519AC
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			Control parent = this.Parent;
			if (parent != null && (parent is WebZone || parent is Part))
			{
				throw new InvalidOperationException(SR.GetString("Zone_InvalidParent"));
			}
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x001529EC File Offset: 0x001519EC
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (!base.DesignMode && this.Page != null && this.Page.Request.Browser.Type == "IE5" && this.Page.Request.Browser.Platform == "MacPPC" && (!base.ControlStyleCreated || base.ControlStyle.Height == Unit.Empty))
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "1px");
			}
			base.RenderBeginTag(writer);
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x00152AA4 File Offset: 0x00151AA4
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.HasHeader)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				TitleStyle headerStyle = this.HeaderStyle;
				if (!headerStyle.IsEmpty)
				{
					headerStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderHeader(writer);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this.RenderBody(writer);
			writer.RenderEndTag();
			writer.RenderEndTag();
			if (this.HasFooter)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				TitleStyle footerStyle = this.FooterStyle;
				if (!footerStyle.IsEmpty)
				{
					footerStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderFooter(writer);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x00152B65 File Offset: 0x00151B65
		protected virtual void RenderHeader(HtmlTextWriter writer)
		{
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x00152B67 File Offset: 0x00151B67
		protected virtual void RenderBody(HtmlTextWriter writer)
		{
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x00152B69 File Offset: 0x00151B69
		protected virtual void RenderFooter(HtmlTextWriter writer)
		{
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x00152B6C File Offset: 0x00151B6C
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._emptyZoneTextStyle != null) ? ((IStateManager)this._emptyZoneTextStyle).SaveViewState() : null,
				(this._footerStyle != null) ? ((IStateManager)this._footerStyle).SaveViewState() : null,
				(this._partStyle != null) ? ((IStateManager)this._partStyle).SaveViewState() : null,
				(this._partChromeStyle != null) ? ((IStateManager)this._partChromeStyle).SaveViewState() : null,
				(this._partTitleStyle != null) ? ((IStateManager)this._partTitleStyle).SaveViewState() : null,
				(this._headerStyle != null) ? ((IStateManager)this._headerStyle).SaveViewState() : null,
				(this._verbStyle != null) ? ((IStateManager)this._verbStyle).SaveViewState() : null,
				(this._errorStyle != null) ? ((IStateManager)this._errorStyle).SaveViewState() : null
			};
			for (int i = 0; i < 9; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x00152C68 File Offset: 0x00151C68
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._emptyZoneTextStyle != null)
			{
				((IStateManager)this._emptyZoneTextStyle).TrackViewState();
			}
			if (this._footerStyle != null)
			{
				((IStateManager)this._footerStyle).TrackViewState();
			}
			if (this._partStyle != null)
			{
				((IStateManager)this._partStyle).TrackViewState();
			}
			if (this._partChromeStyle != null)
			{
				((IStateManager)this._partChromeStyle).TrackViewState();
			}
			if (this._partTitleStyle != null)
			{
				((IStateManager)this._partTitleStyle).TrackViewState();
			}
			if (this._headerStyle != null)
			{
				((IStateManager)this._headerStyle).TrackViewState();
			}
			if (this._verbStyle != null)
			{
				((IStateManager)this._verbStyle).TrackViewState();
			}
			if (this._errorStyle != null)
			{
				((IStateManager)this._errorStyle).TrackViewState();
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600537C RID: 21372 RVA: 0x00152D13 File Offset: 0x00151D13
		protected WebPartManager WebPartManager
		{
			get
			{
				return this._webPartManager;
			}
		}

		// Token: 0x04002E5C RID: 11868
		private const int baseIndex = 0;

		// Token: 0x04002E5D RID: 11869
		private const int emptyZoneTextStyleIndex = 1;

		// Token: 0x04002E5E RID: 11870
		private const int footerStyleIndex = 2;

		// Token: 0x04002E5F RID: 11871
		private const int partStyleIndex = 3;

		// Token: 0x04002E60 RID: 11872
		private const int partChromeStyleIndex = 4;

		// Token: 0x04002E61 RID: 11873
		private const int partTitleStyleIndex = 5;

		// Token: 0x04002E62 RID: 11874
		private const int headerStyleIndex = 6;

		// Token: 0x04002E63 RID: 11875
		private const int verbStyleIndex = 7;

		// Token: 0x04002E64 RID: 11876
		private const int errorStyleIndex = 8;

		// Token: 0x04002E65 RID: 11877
		private const int viewStateArrayLength = 9;

		// Token: 0x04002E66 RID: 11878
		private WebPartManager _webPartManager;

		// Token: 0x04002E67 RID: 11879
		private Style _emptyZoneTextStyle;

		// Token: 0x04002E68 RID: 11880
		private TitleStyle _footerStyle;

		// Token: 0x04002E69 RID: 11881
		private TableStyle _partStyle;

		// Token: 0x04002E6A RID: 11882
		private Style _partChromeStyle;

		// Token: 0x04002E6B RID: 11883
		private TitleStyle _partTitleStyle;

		// Token: 0x04002E6C RID: 11884
		private TitleStyle _headerStyle;

		// Token: 0x04002E6D RID: 11885
		private Style _verbStyle;

		// Token: 0x04002E6E RID: 11886
		private Style _errorStyle;
	}
}
