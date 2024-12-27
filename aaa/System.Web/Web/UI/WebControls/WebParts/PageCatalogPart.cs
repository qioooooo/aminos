using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D5 RID: 1749
	[Designer("System.Web.UI.Design.WebControls.WebParts.PageCatalogPartDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PageCatalogPart : CatalogPart
	{
		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x060055CE RID: 21966 RVA: 0x0015B754 File Offset: 0x0015A754
		// (set) Token: 0x060055CF RID: 21967 RVA: 0x0015B786 File Offset: 0x0015A786
		[WebSysDefaultValue("PageCatalogPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("PageCatalogPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x0015B79C File Offset: 0x0015A79C
		public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
		{
			if (base.DesignMode)
			{
				return PageCatalogPart.DesignModeAvailableWebParts;
			}
			if (this._availableWebPartDescriptions == null)
			{
				WebPartCollection webPartCollection;
				if (base.WebPartManager != null)
				{
					WebPartCollection closedWebParts = this.GetClosedWebParts();
					if (closedWebParts != null)
					{
						webPartCollection = closedWebParts;
					}
					else
					{
						webPartCollection = new WebPartCollection();
					}
				}
				else
				{
					webPartCollection = new WebPartCollection();
				}
				ArrayList arrayList = new ArrayList();
				foreach (object obj in webPartCollection)
				{
					WebPart webPart = (WebPart)obj;
					if (!(webPart is UnauthorizedWebPart))
					{
						WebPartDescription webPartDescription = new WebPartDescription(webPart);
						arrayList.Add(webPartDescription);
					}
				}
				this._availableWebPartDescriptions = new WebPartDescriptionCollection(arrayList);
			}
			return this._availableWebPartDescriptions;
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x0015B860 File Offset: 0x0015A860
		private WebPartCollection GetClosedWebParts()
		{
			ArrayList arrayList = new ArrayList();
			WebPartCollection webParts = base.WebPartManager.WebParts;
			if (webParts != null)
			{
				foreach (object obj in webParts)
				{
					WebPart webPart = (WebPart)obj;
					if (webPart.IsClosed)
					{
						arrayList.Add(webPart);
					}
				}
			}
			return new WebPartCollection(arrayList);
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x0015B8DC File Offset: 0x0015A8DC
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

		// Token: 0x060055D3 RID: 21971 RVA: 0x0015B924 File Offset: 0x0015A924
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (base.WebPartManager != null)
			{
				base.WebPartManager.WebPartAdded += this.OnWebPartsChanged;
				base.WebPartManager.WebPartClosed += this.OnWebPartsChanged;
				base.WebPartManager.WebPartDeleted += this.OnWebPartsChanged;
			}
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x0015B985 File Offset: 0x0015A985
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this._availableWebPartDescriptions = null;
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x0015B995 File Offset: 0x0015A995
		private void OnWebPartsChanged(object sender, WebPartEventArgs e)
		{
			this._availableWebPartDescriptions = null;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x0015B99E File Offset: 0x0015A99E
		protected internal override void Render(HtmlTextWriter writer)
		{
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x060055D7 RID: 21975 RVA: 0x0015B9A0 File Offset: 0x0015A9A0
		// (set) Token: 0x060055D8 RID: 21976 RVA: 0x0015B9A8 File Offset: 0x0015A9A8
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x060055D9 RID: 21977 RVA: 0x0015B9B1 File Offset: 0x0015A9B1
		// (set) Token: 0x060055DA RID: 21978 RVA: 0x0015B9B9 File Offset: 0x0015A9B9
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x060055DB RID: 21979 RVA: 0x0015B9C2 File Offset: 0x0015A9C2
		// (set) Token: 0x060055DC RID: 21980 RVA: 0x0015B9CA File Offset: 0x0015A9CA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
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

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x060055DD RID: 21981 RVA: 0x0015B9D3 File Offset: 0x0015A9D3
		// (set) Token: 0x060055DE RID: 21982 RVA: 0x0015B9DB File Offset: 0x0015A9DB
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

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x060055DF RID: 21983 RVA: 0x0015B9E4 File Offset: 0x0015A9E4
		// (set) Token: 0x060055E0 RID: 21984 RVA: 0x0015B9EC File Offset: 0x0015A9EC
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x060055E1 RID: 21985 RVA: 0x0015B9F5 File Offset: 0x0015A9F5
		// (set) Token: 0x060055E2 RID: 21986 RVA: 0x0015B9FD File Offset: 0x0015A9FD
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

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x060055E3 RID: 21987 RVA: 0x0015BA06 File Offset: 0x0015AA06
		// (set) Token: 0x060055E4 RID: 21988 RVA: 0x0015BA0E File Offset: 0x0015AA0E
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

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x060055E5 RID: 21989 RVA: 0x0015BA17 File Offset: 0x0015AA17
		// (set) Token: 0x060055E6 RID: 21990 RVA: 0x0015BA1F File Offset: 0x0015AA1F
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

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x060055E7 RID: 21991 RVA: 0x0015BA28 File Offset: 0x0015AA28
		// (set) Token: 0x060055E8 RID: 21992 RVA: 0x0015BA30 File Offset: 0x0015AA30
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

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x060055E9 RID: 21993 RVA: 0x0015BA39 File Offset: 0x0015AA39
		// (set) Token: 0x060055EA RID: 21994 RVA: 0x0015BA41 File Offset: 0x0015AA41
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

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x060055EB RID: 21995 RVA: 0x0015BA4A File Offset: 0x0015AA4A
		// (set) Token: 0x060055EC RID: 21996 RVA: 0x0015BA50 File Offset: 0x0015AA50
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

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x060055ED RID: 21997 RVA: 0x0015BA82 File Offset: 0x0015AA82
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

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x060055EE RID: 21998 RVA: 0x0015BA8A File Offset: 0x0015AA8A
		// (set) Token: 0x060055EF RID: 21999 RVA: 0x0015BA92 File Offset: 0x0015AA92
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x060055F0 RID: 22000 RVA: 0x0015BA9B File Offset: 0x0015AA9B
		// (set) Token: 0x060055F1 RID: 22001 RVA: 0x0015BAA3 File Offset: 0x0015AAA3
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

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x060055F2 RID: 22002 RVA: 0x0015BAAC File Offset: 0x0015AAAC
		// (set) Token: 0x060055F3 RID: 22003 RVA: 0x0015BAB4 File Offset: 0x0015AAB4
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

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x060055F4 RID: 22004 RVA: 0x0015BABD File Offset: 0x0015AABD
		// (set) Token: 0x060055F5 RID: 22005 RVA: 0x0015BAC5 File Offset: 0x0015AAC5
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

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0015BACE File Offset: 0x0015AACE
		// (set) Token: 0x060055F7 RID: 22007 RVA: 0x0015BAD6 File Offset: 0x0015AAD6
		[Browsable(false)]
		[Themeable(false)]
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

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x060055F8 RID: 22008 RVA: 0x0015BADF File Offset: 0x0015AADF
		// (set) Token: 0x060055F9 RID: 22009 RVA: 0x0015BAE8 File Offset: 0x0015AAE8
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

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x0015BB1A File Offset: 0x0015AB1A
		// (set) Token: 0x060055FB RID: 22011 RVA: 0x0015BB22 File Offset: 0x0015AB22
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
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

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x060055FC RID: 22012 RVA: 0x0015BB2B File Offset: 0x0015AB2B
		// (set) Token: 0x060055FD RID: 22013 RVA: 0x0015BB33 File Offset: 0x0015AB33
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Themeable(false)]
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

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x060055FE RID: 22014 RVA: 0x0015BB3C File Offset: 0x0015AB3C
		// (set) Token: 0x060055FF RID: 22015 RVA: 0x0015BB44 File Offset: 0x0015AB44
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Themeable(false)]
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

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x0015BB4D File Offset: 0x0015AB4D
		// (set) Token: 0x06005601 RID: 22017 RVA: 0x0015BB55 File Offset: 0x0015AB55
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x06005602 RID: 22018 RVA: 0x0015BB5E File Offset: 0x0015AB5E
		// (set) Token: 0x06005603 RID: 22019 RVA: 0x0015BB66 File Offset: 0x0015AB66
		[Browsable(false)]
		[Themeable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x04002F34 RID: 12084
		private WebPartDescriptionCollection _availableWebPartDescriptions;

		// Token: 0x04002F35 RID: 12085
		private static readonly WebPartDescriptionCollection DesignModeAvailableWebParts = new WebPartDescriptionCollection(new WebPartDescription[]
		{
			new WebPartDescription("webpart1", string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogPart_SampleWebPartTitle"), new object[] { "1" }), null, null),
			new WebPartDescription("webpart2", string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogPart_SampleWebPartTitle"), new object[] { "2" }), null, null),
			new WebPartDescription("webpart3", string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogPart_SampleWebPartTitle"), new object[] { "3" }), null, null)
		});
	}
}
