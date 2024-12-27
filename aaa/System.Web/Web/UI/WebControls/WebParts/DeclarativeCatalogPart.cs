using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006BA RID: 1722
	[Designer("System.Web.UI.Design.WebControls.WebParts.DeclarativeCatalogPartDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DeclarativeCatalogPart : CatalogPart
	{
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06005470 RID: 21616 RVA: 0x001581B0 File Offset: 0x001571B0
		// (set) Token: 0x06005471 RID: 21617 RVA: 0x001581E2 File Offset: 0x001571E2
		[WebSysDefaultValue("DeclarativeCatalogPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("DeclarativeCatalogPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06005472 RID: 21618 RVA: 0x001581F5 File Offset: 0x001571F5
		// (set) Token: 0x06005473 RID: 21619 RVA: 0x0015820B File Offset: 0x0015720B
		[Editor("System.Web.UI.Design.UserControlFileEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Behavior")]
		[WebSysDescription("DeclarativeCatlaogPart_WebPartsListUserControlPath")]
		[DefaultValue("")]
		[Themeable(false)]
		[UrlProperty]
		public string WebPartsListUserControlPath
		{
			get
			{
				if (this._webPartsListUserControlPath == null)
				{
					return string.Empty;
				}
				return this._webPartsListUserControlPath;
			}
			set
			{
				this._webPartsListUserControlPath = value;
				this._descriptions = null;
			}
		}

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06005474 RID: 21620 RVA: 0x0015821B File Offset: 0x0015721B
		// (set) Token: 0x06005475 RID: 21621 RVA: 0x00158223 File Offset: 0x00157223
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(DeclarativeCatalogPart))]
		[Browsable(false)]
		public ITemplate WebPartsTemplate
		{
			get
			{
				return this._webPartsTemplate;
			}
			set
			{
				this._webPartsTemplate = value;
				this._descriptions = null;
			}
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x00158234 File Offset: 0x00157234
		private void AddControlToDescriptions(Control control, ArrayList descriptions)
		{
			WebPart webPart = control as WebPart;
			if (webPart == null && !(control is LiteralControl))
			{
				if (base.WebPartManager != null)
				{
					webPart = base.WebPartManager.CreateWebPart(control);
				}
				else
				{
					webPart = WebPartManager.CreateWebPartStatic(control);
				}
			}
			if (webPart != null && (base.WebPartManager == null || base.WebPartManager.IsAuthorized(webPart)))
			{
				WebPartDescription webPartDescription = new WebPartDescription(webPart);
				descriptions.Add(webPartDescription);
			}
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x00158299 File Offset: 0x00157299
		public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
		{
			if (this._descriptions == null)
			{
				this.LoadAvailableWebParts();
			}
			return this._descriptions;
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x001582B0 File Offset: 0x001572B0
		public override WebPart GetWebPart(WebPartDescription description)
		{
			if (description == null)
			{
				throw new ArgumentNullException("description");
			}
			WebPartDescriptionCollection availableWebPartDescriptions = this.GetAvailableWebPartDescriptions();
			if (!availableWebPartDescriptions.Contains(description))
			{
				throw new ArgumentException(SR.GetString("CatalogPart_UnknownDescription"), "description");
			}
			return description.WebPart;
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x001582F8 File Offset: 0x001572F8
		private void LoadAvailableWebParts()
		{
			ArrayList arrayList = new ArrayList();
			if (this.WebPartsTemplate != null)
			{
				Control control = new NonParentingControl();
				this.WebPartsTemplate.InstantiateIn(control);
				if (control.HasControls())
				{
					Control[] array = new Control[control.Controls.Count];
					control.Controls.CopyTo(array, 0);
					foreach (Control control2 in array)
					{
						this.AddControlToDescriptions(control2, arrayList);
					}
				}
			}
			string webPartsListUserControlPath = this.WebPartsListUserControlPath;
			if (!string.IsNullOrEmpty(webPartsListUserControlPath) && !base.DesignMode)
			{
				Control control3 = this.Page.LoadControl(webPartsListUserControlPath);
				if (control3 != null && control3.HasControls())
				{
					Control[] array3 = new Control[control3.Controls.Count];
					control3.Controls.CopyTo(array3, 0);
					foreach (Control control4 in array3)
					{
						this.AddControlToDescriptions(control4, arrayList);
					}
				}
			}
			this._descriptions = new WebPartDescriptionCollection(arrayList);
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x001583F9 File Offset: 0x001573F9
		protected internal override void Render(HtmlTextWriter writer)
		{
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600547B RID: 21627 RVA: 0x001583FB File Offset: 0x001573FB
		// (set) Token: 0x0600547C RID: 21628 RVA: 0x00158403 File Offset: 0x00157403
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string AccessKey
		{
			get
			{
				return base.AccessKey;
			}
			set
			{
				base.AccessKey = value;
			}
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x0600547D RID: 21629 RVA: 0x0015840C File Offset: 0x0015740C
		// (set) Token: 0x0600547E RID: 21630 RVA: 0x00158414 File Offset: 0x00157414
		[Themeable(false)]
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

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x0015841D File Offset: 0x0015741D
		// (set) Token: 0x06005480 RID: 21632 RVA: 0x00158425 File Offset: 0x00157425
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string BackImageUrl
		{
			get
			{
				return base.BackImageUrl;
			}
			set
			{
				base.BackImageUrl = value;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x06005481 RID: 21633 RVA: 0x0015842E File Offset: 0x0015742E
		// (set) Token: 0x06005482 RID: 21634 RVA: 0x00158436 File Offset: 0x00157436
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Themeable(false)]
		public override Color BorderColor
		{
			get
			{
				return base.BorderColor;
			}
			set
			{
				base.BorderColor = value;
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x06005483 RID: 21635 RVA: 0x0015843F File Offset: 0x0015743F
		// (set) Token: 0x06005484 RID: 21636 RVA: 0x00158447 File Offset: 0x00157447
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06005485 RID: 21637 RVA: 0x00158450 File Offset: 0x00157450
		// (set) Token: 0x06005486 RID: 21638 RVA: 0x00158458 File Offset: 0x00157458
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		[Browsable(false)]
		public override Unit BorderWidth
		{
			get
			{
				return base.BorderWidth;
			}
			set
			{
				base.BorderWidth = value;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06005487 RID: 21639 RVA: 0x00158461 File Offset: 0x00157461
		// (set) Token: 0x06005488 RID: 21640 RVA: 0x00158469 File Offset: 0x00157469
		[Themeable(false)]
		[CssClassProperty]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string CssClass
		{
			get
			{
				return base.CssClass;
			}
			set
			{
				base.CssClass = value;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06005489 RID: 21641 RVA: 0x00158472 File Offset: 0x00157472
		// (set) Token: 0x0600548A RID: 21642 RVA: 0x0015847A File Offset: 0x0015747A
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string DefaultButton
		{
			get
			{
				return base.DefaultButton;
			}
			set
			{
				base.DefaultButton = value;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x0600548B RID: 21643 RVA: 0x00158483 File Offset: 0x00157483
		// (set) Token: 0x0600548C RID: 21644 RVA: 0x0015848B File Offset: 0x0015748B
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContentDirection Direction
		{
			get
			{
				return base.Direction;
			}
			set
			{
				base.Direction = value;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x0600548D RID: 21645 RVA: 0x00158494 File Offset: 0x00157494
		// (set) Token: 0x0600548E RID: 21646 RVA: 0x0015849C File Offset: 0x0015749C
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x001584A5 File Offset: 0x001574A5
		// (set) Token: 0x06005490 RID: 21648 RVA: 0x001584A8 File Offset: 0x001574A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
		[Browsable(false)]
		[Themeable(false)]
		public override bool EnableTheming
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06005491 RID: 21649 RVA: 0x001584DA File Offset: 0x001574DA
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override FontInfo Font
		{
			get
			{
				return base.Font;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x001584E2 File Offset: 0x001574E2
		// (set) Token: 0x06005493 RID: 21651 RVA: 0x001584EA File Offset: 0x001574EA
		[Themeable(false)]
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

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06005494 RID: 21652 RVA: 0x001584F3 File Offset: 0x001574F3
		// (set) Token: 0x06005495 RID: 21653 RVA: 0x001584FB File Offset: 0x001574FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		[Browsable(false)]
		public override string GroupingText
		{
			get
			{
				return base.GroupingText;
			}
			set
			{
				base.GroupingText = value;
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06005496 RID: 21654 RVA: 0x00158504 File Offset: 0x00157504
		// (set) Token: 0x06005497 RID: 21655 RVA: 0x0015850C File Offset: 0x0015750C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Themeable(false)]
		public override Unit Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06005498 RID: 21656 RVA: 0x00158515 File Offset: 0x00157515
		// (set) Token: 0x06005499 RID: 21657 RVA: 0x0015851D File Offset: 0x0015751D
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override HorizontalAlign HorizontalAlign
		{
			get
			{
				return base.HorizontalAlign;
			}
			set
			{
				base.HorizontalAlign = value;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x0600549A RID: 21658 RVA: 0x00158526 File Offset: 0x00157526
		// (set) Token: 0x0600549B RID: 21659 RVA: 0x0015852E File Offset: 0x0015752E
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ScrollBars ScrollBars
		{
			get
			{
				return base.ScrollBars;
			}
			set
			{
				base.ScrollBars = value;
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x0600549C RID: 21660 RVA: 0x00158537 File Offset: 0x00157537
		// (set) Token: 0x0600549D RID: 21661 RVA: 0x00158540 File Offset: 0x00157540
		[Browsable(false)]
		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		public override string SkinID
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600549E RID: 21662 RVA: 0x00158572 File Offset: 0x00157572
		// (set) Token: 0x0600549F RID: 21663 RVA: 0x0015857A File Offset: 0x0015757A
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override short TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x00158583 File Offset: 0x00157583
		// (set) Token: 0x060054A1 RID: 21665 RVA: 0x0015858B File Offset: 0x0015758B
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string ToolTip
		{
			get
			{
				return base.ToolTip;
			}
			set
			{
				base.ToolTip = value;
			}
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x060054A2 RID: 21666 RVA: 0x00158594 File Offset: 0x00157594
		// (set) Token: 0x060054A3 RID: 21667 RVA: 0x0015859C File Offset: 0x0015759C
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x060054A4 RID: 21668 RVA: 0x001585A5 File Offset: 0x001575A5
		// (set) Token: 0x060054A5 RID: 21669 RVA: 0x001585AD File Offset: 0x001575AD
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Unit Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x060054A6 RID: 21670 RVA: 0x001585B6 File Offset: 0x001575B6
		// (set) Token: 0x060054A7 RID: 21671 RVA: 0x001585BE File Offset: 0x001575BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		[Browsable(false)]
		public override bool Wrap
		{
			get
			{
				return base.Wrap;
			}
			set
			{
				base.Wrap = value;
			}
		}

		// Token: 0x04002EE2 RID: 12002
		private ITemplate _webPartsTemplate;

		// Token: 0x04002EE3 RID: 12003
		private WebPartDescriptionCollection _descriptions;

		// Token: 0x04002EE4 RID: 12004
		private string _webPartsListUserControlPath;
	}
}
