using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006AC RID: 1708
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class CatalogZoneBase : ToolZone, IPostBackDataHandler
	{
		// Token: 0x06005398 RID: 21400 RVA: 0x00153547 File Offset: 0x00152547
		protected CatalogZoneBase()
			: base(WebPartManager.CatalogDisplayMode)
		{
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x00153554 File Offset: 0x00152554
		[DefaultValue(null)]
		[WebSysDescription("CatalogZoneBase_AddVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb AddVerb
		{
			get
			{
				if (this._addVerb == null)
				{
					this._addVerb = new WebPartCatalogAddVerb();
					this._addVerb.EventArgument = "add";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._addVerb).TrackViewState();
					}
				}
				return this._addVerb;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600539A RID: 21402 RVA: 0x00153592 File Offset: 0x00152592
		internal string CheckBoxName
		{
			get
			{
				return this.UniqueID + '$' + "_checkbox";
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600539B RID: 21403 RVA: 0x001535AB File Offset: 0x001525AB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CatalogPartChrome CatalogPartChrome
		{
			get
			{
				if (this._catalogPartChrome == null)
				{
					this._catalogPartChrome = this.CreateCatalogPartChrome();
				}
				return this._catalogPartChrome;
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x001535C8 File Offset: 0x001525C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CatalogPartCollection CatalogParts
		{
			get
			{
				if (this._catalogParts == null)
				{
					CatalogPartCollection catalogPartCollection = this.CreateCatalogParts();
					if (!base.DesignMode)
					{
						foreach (object obj in catalogPartCollection)
						{
							CatalogPart catalogPart = (CatalogPart)obj;
							if (string.IsNullOrEmpty(catalogPart.ID))
							{
								throw new InvalidOperationException(SR.GetString("CatalogZoneBase_NoCatalogPartID"));
							}
						}
					}
					this._catalogParts = catalogPartCollection;
					this.EnsureChildControls();
				}
				return this._catalogParts;
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x0015365C File Offset: 0x0015265C
		[WebCategory("Verbs")]
		[WebSysDescription("CatalogZoneBase_CloseVerb")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual WebPartVerb CloseVerb
		{
			get
			{
				if (this._closeVerb == null)
				{
					this._closeVerb = new WebPartCatalogCloseVerb();
					this._closeVerb.EventArgument = "close";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._closeVerb).TrackViewState();
					}
				}
				return this._closeVerb;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x0600539E RID: 21406 RVA: 0x0015369C File Offset: 0x0015269C
		// (set) Token: 0x0600539F RID: 21407 RVA: 0x001536CE File Offset: 0x001526CE
		[WebSysDefaultValue("CatalogZoneBase_DefaultEmptyZoneText")]
		public override string EmptyZoneText
		{
			get
			{
				string text = (string)this.ViewState["EmptyZoneText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("CatalogZoneBase_DefaultEmptyZoneText");
			}
			set
			{
				this.ViewState["EmptyZoneText"] = value;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x001536E4 File Offset: 0x001526E4
		// (set) Token: 0x060053A1 RID: 21409 RVA: 0x00153716 File Offset: 0x00152716
		[WebSysDefaultValue("CatalogZoneBase_HeaderText")]
		public override string HeaderText
		{
			get
			{
				string text = (string)this.ViewState["HeaderText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("CatalogZoneBase_HeaderText");
			}
			set
			{
				this.ViewState["HeaderText"] = value;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x0015372C File Offset: 0x0015272C
		// (set) Token: 0x060053A3 RID: 21411 RVA: 0x0015375E File Offset: 0x0015275E
		[WebSysDefaultValue("CatalogZoneBase_InstructionText")]
		public override string InstructionText
		{
			get
			{
				string text = (string)this.ViewState["InstructionText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("CatalogZoneBase_InstructionText");
			}
			set
			{
				this.ViewState["InstructionText"] = value;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x00153771 File Offset: 0x00152771
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("CatalogZoneBase_PartLinkStyle")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style PartLinkStyle
		{
			get
			{
				if (this._partLinkStyle == null)
				{
					this._partLinkStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._partLinkStyle).TrackViewState();
					}
				}
				return this._partLinkStyle;
			}
		}

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x060053A5 RID: 21413 RVA: 0x001537A0 File Offset: 0x001527A0
		// (set) Token: 0x060053A6 RID: 21414 RVA: 0x001537F4 File Offset: 0x001527F4
		[Themeable(false)]
		[WebSysDescription("CatalogZoneBase_SelectedCatalogPartID")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public string SelectedCatalogPartID
		{
			get
			{
				if (!string.IsNullOrEmpty(this._selectedCatalogPartID))
				{
					return this._selectedCatalogPartID;
				}
				if (base.DesignMode)
				{
					return string.Empty;
				}
				CatalogPartCollection catalogParts = this.CatalogParts;
				if (catalogParts != null && catalogParts.Count > 0)
				{
					return catalogParts[0].ID;
				}
				return string.Empty;
			}
			set
			{
				this._selectedCatalogPartID = value;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x060053A7 RID: 21415 RVA: 0x00153800 File Offset: 0x00152800
		private CatalogPart SelectedCatalogPart
		{
			get
			{
				CatalogPartCollection catalogParts = this.CatalogParts;
				if (catalogParts == null || catalogParts.Count <= 0)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this._selectedCatalogPartID))
				{
					return catalogParts[0];
				}
				return catalogParts[this._selectedCatalogPartID];
			}
		}

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x00153844 File Offset: 0x00152844
		// (set) Token: 0x060053A9 RID: 21417 RVA: 0x00153876 File Offset: 0x00152876
		[WebSysDescription("CatalogZoneBase_SelectTargetZoneText")]
		[WebSysDefaultValue("CatalogZoneBase_DefaultSelectTargetZoneText")]
		[WebCategory("Behavior")]
		[Localizable(true)]
		public virtual string SelectTargetZoneText
		{
			get
			{
				string text = (string)this.ViewState["SelectTargetZoneText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("CatalogZoneBase_DefaultSelectTargetZoneText");
			}
			set
			{
				this.ViewState["SelectTargetZoneText"] = value;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x00153889 File Offset: 0x00152889
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebSysDescription("CatalogZoneBase_SelectedPartLinkStyle")]
		[WebCategory("Styles")]
		public Style SelectedPartLinkStyle
		{
			get
			{
				if (this._selectedPartLinkStyle == null)
				{
					this._selectedPartLinkStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._selectedPartLinkStyle).TrackViewState();
					}
				}
				return this._selectedPartLinkStyle;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001538B8 File Offset: 0x001528B8
		// (set) Token: 0x060053AC RID: 21420 RVA: 0x001538E1 File Offset: 0x001528E1
		[WebSysDescription("CatalogZoneBase_ShowCatalogIcons")]
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		public virtual bool ShowCatalogIcons
		{
			get
			{
				object obj = this.ViewState["ShowCatalogIcons"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowCatalogIcons"] = value;
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x001538F9 File Offset: 0x001528F9
		private string ZonesID
		{
			get
			{
				return this.UniqueID + '$' + "_zones";
			}
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x00153914 File Offset: 0x00152914
		private void AddSelectedWebParts()
		{
			WebPartZoneBase webPartZoneBase = null;
			if (base.WebPartManager != null)
			{
				webPartZoneBase = base.WebPartManager.Zones[this._selectedZoneID];
			}
			CatalogPart selectedCatalogPart = this.SelectedCatalogPart;
			WebPartDescriptionCollection webPartDescriptionCollection = null;
			if (selectedCatalogPart != null)
			{
				webPartDescriptionCollection = selectedCatalogPart.GetAvailableWebPartDescriptions();
			}
			if (webPartZoneBase != null && webPartZoneBase.AllowLayoutChange && this._selectedCheckBoxValues != null && webPartDescriptionCollection != null)
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < this._selectedCheckBoxValues.Length; i++)
				{
					string text = this._selectedCheckBoxValues[i];
					WebPartDescription webPartDescription = webPartDescriptionCollection[text];
					if (webPartDescription != null)
					{
						WebPart webPart = selectedCatalogPart.GetWebPart(webPartDescription);
						if (webPart != null)
						{
							arrayList.Add(webPart);
						}
					}
				}
				this.AddWebParts(arrayList, webPartZoneBase);
			}
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x001539C0 File Offset: 0x001529C0
		private void AddWebParts(ArrayList webParts, WebPartZoneBase zone)
		{
			webParts.Reverse();
			foreach (object obj in webParts)
			{
				WebPart webPart = (WebPart)obj;
				WebPartZoneBase webPartZoneBase = zone;
				if (!webPart.AllowZoneChange && webPart.Zone != null)
				{
					webPartZoneBase = webPart.Zone;
				}
				base.WebPartManager.AddWebPart(webPart, webPartZoneBase, 0);
			}
		}

		// Token: 0x060053B0 RID: 21424 RVA: 0x00153A3C File Offset: 0x00152A3C
		protected override void Close()
		{
			if (base.WebPartManager != null)
			{
				base.WebPartManager.DisplayMode = WebPartManager.BrowseDisplayMode;
			}
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x00153A56 File Offset: 0x00152A56
		protected virtual CatalogPartChrome CreateCatalogPartChrome()
		{
			return new CatalogPartChrome(this);
		}

		// Token: 0x060053B2 RID: 21426
		protected abstract CatalogPartCollection CreateCatalogParts();

		// Token: 0x060053B3 RID: 21427 RVA: 0x00153A60 File Offset: 0x00152A60
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			foreach (object obj in this.CatalogParts)
			{
				CatalogPart catalogPart = (CatalogPart)obj;
				catalogPart.SetWebPartManager(base.WebPartManager);
				catalogPart.SetZone(this);
				this.Controls.Add(catalogPart);
			}
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x00153ADC File Offset: 0x00152ADC
		internal string GetCheckBoxID(string value)
		{
			return string.Concat(new object[] { this.ClientID, base.ClientIDSeparator, "_checkbox", base.ClientIDSeparator, value });
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x00153B27 File Offset: 0x00152B27
		protected void InvalidateCatalogParts()
		{
			this._catalogParts = null;
			base.ChildControlsCreated = false;
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x00153B38 File Offset: 0x00152B38
		protected internal override void LoadControlState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadControlState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 2)
			{
				throw new ArgumentException(SR.GetString("Invalid_ControlState"));
			}
			base.LoadControlState(array[0]);
			if (array[1] != null)
			{
				this._selectedCatalogPartID = (string)array[1];
			}
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x00153B8C File Offset: 0x00152B8C
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string text = postCollection[this.CheckBoxName];
			if (!string.IsNullOrEmpty(text))
			{
				base.ValidateEvent(this.CheckBoxName);
				this._selectedCheckBoxValues = text.Split(new char[] { ',' });
			}
			this._selectedZoneID = postCollection[this.ZonesID];
			return false;
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x00153BE8 File Offset: 0x00152BE8
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 5)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.AddVerb).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.CloseVerb).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.PartLinkStyle).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.SelectedPartLinkStyle).LoadViewState(array[4]);
			}
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x00153C74 File Offset: 0x00152C74
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page page = this.Page;
			if (page != null)
			{
				page.RegisterRequiresControlState(this);
			}
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x00153C99 File Offset: 0x00152C99
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.CatalogPartChrome.PerformPreRender();
			this.Page.RegisterRequiresPostBack(this);
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x00153CBC File Offset: 0x00152CBC
		protected override void RaisePostBackEvent(string eventArgument)
		{
			string[] array = eventArgument.Split(new char[] { '$' });
			if (array.Length == 2 && array[0] == "select")
			{
				this.SelectedCatalogPartID = array[1];
				return;
			}
			if (string.Equals(eventArgument, "add", StringComparison.OrdinalIgnoreCase))
			{
				if (this.AddVerb.Visible && this.AddVerb.Enabled)
				{
					this.AddSelectedWebParts();
					return;
				}
			}
			else if (string.Equals(eventArgument, "close", StringComparison.OrdinalIgnoreCase))
			{
				if (this.CloseVerb.Visible && this.CloseVerb.Enabled)
				{
					this.Close();
					return;
				}
			}
			else
			{
				base.RaisePostBackEvent(eventArgument);
			}
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x00153D61 File Offset: 0x00152D61
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			base.Render(writer);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x00153D80 File Offset: 0x00152D80
		protected override void RenderBody(HtmlTextWriter writer)
		{
			base.RenderBodyTableBeginTag(writer);
			if (base.DesignMode)
			{
				base.RenderDesignerRegionBeginTag(writer, Orientation.Vertical);
			}
			CatalogPartCollection catalogParts = this.CatalogParts;
			if (catalogParts != null && catalogParts.Count > 0)
			{
				bool flag = true;
				if (catalogParts.Count > 1)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					flag = false;
					this.RenderCatalogPartLinks(writer);
					writer.RenderEndTag();
					writer.RenderEndTag();
				}
				CatalogPartChrome catalogPartChrome = this.CatalogPartChrome;
				if (base.DesignMode)
				{
					using (IEnumerator enumerator = catalogParts.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							CatalogPart catalogPart = (CatalogPart)obj;
							this.RenderCatalogPart(writer, catalogPart, catalogPartChrome, ref flag);
						}
						goto IL_00C9;
					}
				}
				CatalogPart selectedCatalogPart = this.SelectedCatalogPart;
				if (selectedCatalogPart != null)
				{
					this.RenderCatalogPart(writer, selectedCatalogPart, catalogPartChrome, ref flag);
				}
				IL_00C9:
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			else
			{
				this.RenderEmptyZoneText(writer);
			}
			if (base.DesignMode)
			{
				WebZone.RenderDesignerRegionEndTag(writer);
			}
			WebZone.RenderBodyTableEndTag(writer);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x00153EBC File Offset: 0x00152EBC
		private void RenderCatalogPart(HtmlTextWriter writer, CatalogPart catalogPart, CatalogPartChrome chrome, ref bool firstCell)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (!firstCell)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "0");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			firstCell = false;
			chrome.RenderCatalogPart(writer, catalogPart);
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x00153EF8 File Offset: 0x00152EF8
		protected virtual void RenderCatalogPartLinks(HtmlTextWriter writer)
		{
			this.RenderInstructionText(writer);
			CatalogPart selectedCatalogPart = this.SelectedCatalogPart;
			foreach (object obj in this.CatalogParts)
			{
				CatalogPart catalogPart = (CatalogPart)obj;
				WebPartDescriptionCollection availableWebPartDescriptions = catalogPart.GetAvailableWebPartDescriptions();
				int num = ((availableWebPartDescriptions != null) ? availableWebPartDescriptions.Count : 0);
				string displayTitle = catalogPart.DisplayTitle;
				string text = displayTitle + " (" + num.ToString(CultureInfo.CurrentCulture) + ")";
				if (catalogPart == selectedCatalogPart)
				{
					Label label = new Label();
					label.Text = text;
					label.Page = this.Page;
					label.ApplyStyle(this.SelectedPartLinkStyle);
					label.RenderControl(writer);
				}
				else
				{
					string text2 = "select" + '$' + catalogPart.ID;
					ZoneLinkButton zoneLinkButton = new ZoneLinkButton(this, text2);
					zoneLinkButton.Text = text;
					zoneLinkButton.ToolTip = SR.GetString("CatalogZoneBase_SelectCatalogPart", new object[] { displayTitle });
					zoneLinkButton.Page = this.Page;
					zoneLinkButton.ApplyStyle(this.PartLinkStyle);
					zoneLinkButton.RenderControl(writer);
				}
				writer.WriteBreak();
			}
			writer.WriteBreak();
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x00154060 File Offset: 0x00153060
		private void RenderEmptyZoneText(HtmlTextWriter writer)
		{
			string emptyZoneText = this.EmptyZoneText;
			if (!string.IsNullOrEmpty(emptyZoneText))
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
				Style emptyZoneTextStyle = base.EmptyZoneTextStyle;
				if (!emptyZoneTextStyle.IsEmpty)
				{
					emptyZoneTextStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write(emptyZoneText);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x001540C4 File Offset: 0x001530C4
		protected override void RenderFooter(HtmlTextWriter writer)
		{
			writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "4px");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			DropDownList dropDownList = new DropDownList();
			dropDownList.ID = this.ZonesID;
			if (base.DesignMode)
			{
				dropDownList.Items.Add(SR.GetString("Zone_SampleHeaderText"));
			}
			else if (base.WebPartManager != null && base.WebPartManager.Zones != null)
			{
				foreach (object obj in base.WebPartManager.Zones)
				{
					WebPartZoneBase webPartZoneBase = (WebPartZoneBase)obj;
					if (webPartZoneBase.AllowLayoutChange)
					{
						ListItem listItem = new ListItem(webPartZoneBase.DisplayTitle, webPartZoneBase.ID);
						if (string.Equals(webPartZoneBase.ID, this._selectedZoneID, StringComparison.OrdinalIgnoreCase))
						{
							listItem.Selected = true;
						}
						dropDownList.Items.Add(listItem);
					}
				}
			}
			base.LabelStyle.AddAttributesToRender(writer, this);
			if (dropDownList.Items.Count > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.For, dropDownList.ClientID);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(this.SelectTargetZoneText);
			writer.RenderEndTag();
			writer.Write("&nbsp;");
			dropDownList.ApplyStyle(base.EditUIStyle);
			if (dropDownList.Items.Count > 0)
			{
				dropDownList.RenderControl(writer);
			}
			writer.Write("&nbsp;");
			this.RenderVerbs(writer);
			writer.RenderEndTag();
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x00154248 File Offset: 0x00153248
		private void RenderInstructionText(HtmlTextWriter writer)
		{
			string instructionText = this.InstructionText;
			if (!string.IsNullOrEmpty(instructionText))
			{
				Label label = new Label();
				label.Text = instructionText;
				label.Page = this.Page;
				label.ApplyStyle(base.InstructionTextStyle);
				label.RenderControl(writer);
				writer.WriteBreak();
				writer.WriteBreak();
			}
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x0015429C File Offset: 0x0015329C
		protected override void RenderVerbs(HtmlTextWriter writer)
		{
			int num = 0;
			bool flag = false;
			CatalogPart selectedCatalogPart = this.SelectedCatalogPart;
			if (selectedCatalogPart != null)
			{
				WebPartDescriptionCollection availableWebPartDescriptions = selectedCatalogPart.GetAvailableWebPartDescriptions();
				num = ((availableWebPartDescriptions != null) ? availableWebPartDescriptions.Count : 0);
			}
			if (num == 0)
			{
				flag = this.AddVerb.Enabled;
				this.AddVerb.Enabled = false;
			}
			try
			{
				base.RenderVerbsInternal(writer, new WebPartVerb[] { this.AddVerb, this.CloseVerb });
			}
			finally
			{
				if (num == 0)
				{
					this.AddVerb.Enabled = flag;
				}
			}
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x00154330 File Offset: 0x00153330
		protected internal override object SaveControlState()
		{
			object[] array = new object[2];
			array[0] = base.SaveControlState();
			if (!string.IsNullOrEmpty(this._selectedCatalogPartID))
			{
				array[1] = this._selectedCatalogPartID;
			}
			for (int i = 0; i < 2; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x00154378 File Offset: 0x00153378
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._addVerb != null) ? ((IStateManager)this._addVerb).SaveViewState() : null,
				(this._closeVerb != null) ? ((IStateManager)this._closeVerb).SaveViewState() : null,
				(this._partLinkStyle != null) ? ((IStateManager)this._partLinkStyle).SaveViewState() : null,
				(this._selectedPartLinkStyle != null) ? ((IStateManager)this._selectedPartLinkStyle).SaveViewState() : null
			};
			for (int i = 0; i < 5; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x00154410 File Offset: 0x00153410
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._addVerb != null)
			{
				((IStateManager)this._addVerb).TrackViewState();
			}
			if (this._closeVerb != null)
			{
				((IStateManager)this._closeVerb).TrackViewState();
			}
			if (this._partLinkStyle != null)
			{
				((IStateManager)this._partLinkStyle).TrackViewState();
			}
			if (this._selectedPartLinkStyle != null)
			{
				((IStateManager)this._selectedPartLinkStyle).TrackViewState();
			}
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x0015446F File Offset: 0x0015346F
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x00154479 File Offset: 0x00153479
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
		}

		// Token: 0x04002E7D RID: 11901
		private const int baseIndex = 0;

		// Token: 0x04002E7E RID: 11902
		private const int addVerbIndex = 1;

		// Token: 0x04002E7F RID: 11903
		private const int closeVerbIndex = 2;

		// Token: 0x04002E80 RID: 11904
		private const int partLinkStyleIndex = 3;

		// Token: 0x04002E81 RID: 11905
		private const int selectedPartLinkStyleIndex = 4;

		// Token: 0x04002E82 RID: 11906
		private const int viewStateArrayLength = 5;

		// Token: 0x04002E83 RID: 11907
		private const int selectedCatalogPartIDIndex = 1;

		// Token: 0x04002E84 RID: 11908
		private const int controlStateArrayLength = 2;

		// Token: 0x04002E85 RID: 11909
		private const string addEventArgument = "add";

		// Token: 0x04002E86 RID: 11910
		private const string closeEventArgument = "close";

		// Token: 0x04002E87 RID: 11911
		private const string selectEventArgument = "select";

		// Token: 0x04002E88 RID: 11912
		private CatalogPartCollection _catalogParts;

		// Token: 0x04002E89 RID: 11913
		private string[] _selectedCheckBoxValues;

		// Token: 0x04002E8A RID: 11914
		private string _selectedZoneID;

		// Token: 0x04002E8B RID: 11915
		private string _selectedCatalogPartID;

		// Token: 0x04002E8C RID: 11916
		private WebPartVerb _addVerb;

		// Token: 0x04002E8D RID: 11917
		private WebPartVerb _closeVerb;

		// Token: 0x04002E8E RID: 11918
		private Style _partLinkStyle;

		// Token: 0x04002E8F RID: 11919
		private Style _selectedPartLinkStyle;

		// Token: 0x04002E90 RID: 11920
		private CatalogPartChrome _catalogPartChrome;
	}
}
