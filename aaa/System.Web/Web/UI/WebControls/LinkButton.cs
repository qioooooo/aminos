using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000534 RID: 1332
	[ParseChildren(false)]
	[ToolboxData("<{0}:LinkButton runat=\"server\">LinkButton</{0}:LinkButton>")]
	[DefaultProperty("Text")]
	[Designer("System.Web.UI.Design.WebControls.LinkButtonDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SupportsEventValidation]
	[ControlBuilder(typeof(LinkButtonControlBuilder))]
	[DataBindingHandler("System.Web.UI.Design.TextDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("Click")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LinkButton : WebControl, IButtonControl, IPostBackEventHandler
	{
		// Token: 0x06004194 RID: 16788 RVA: 0x0010FA39 File Offset: 0x0010EA39
		public LinkButton()
			: base(HtmlTextWriterTag.A)
		{
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x0010FA44 File Offset: 0x0010EA44
		// (set) Token: 0x06004196 RID: 16790 RVA: 0x0010FA71 File Offset: 0x0010EA71
		[WebSysDescription("WebControl_CommandName")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[Themeable(false)]
		public string CommandName
		{
			get
			{
				string text = (string)this.ViewState["CommandName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CommandName"] = value;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x0010FA84 File Offset: 0x0010EA84
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x0010FAB1 File Offset: 0x0010EAB1
		[DefaultValue("")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("WebControl_CommandArgument")]
		[Bindable(true)]
		public string CommandArgument
		{
			get
			{
				string text = (string)this.ViewState["CommandArgument"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CommandArgument"] = value;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x0010FAC4 File Offset: 0x0010EAC4
		// (set) Token: 0x0600419A RID: 16794 RVA: 0x0010FAED File Offset: 0x0010EAED
		[DefaultValue(true)]
		[WebSysDescription("Button_CausesValidation")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = this.ViewState["CausesValidation"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["CausesValidation"] = value;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x0010FB08 File Offset: 0x0010EB08
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x0010FB35 File Offset: 0x0010EB35
		[DefaultValue("")]
		[Themeable(false)]
		[WebCategory("Behavior")]
		[WebSysDescription("Button_OnClientClick")]
		public virtual string OnClientClick
		{
			get
			{
				string text = (string)this.ViewState["OnClientClick"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["OnClientClick"] = value;
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x0010FB48 File Offset: 0x0010EB48
		internal override bool RequiresLegacyRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x0010FB4C File Offset: 0x0010EB4C
		// (set) Token: 0x0600419F RID: 16799 RVA: 0x0010FB79 File Offset: 0x0010EB79
		[Localizable(true)]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[Bindable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("LinkButton_Text")]
		public virtual string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (this.HasControls())
				{
					this.Controls.Clear();
				}
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x0010FBA0 File Offset: 0x0010EBA0
		// (set) Token: 0x060041A1 RID: 16801 RVA: 0x0010FBCD File Offset: 0x0010EBCD
		[DefaultValue("")]
		[UrlProperty("*.aspx")]
		[WebSysDescription("Button_PostBackUrl")]
		[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Themeable(false)]
		[WebCategory("Behavior")]
		public virtual string PostBackUrl
		{
			get
			{
				string text = (string)this.ViewState["PostBackUrl"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["PostBackUrl"] = value;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x0010FBE0 File Offset: 0x0010EBE0
		// (set) Token: 0x060041A3 RID: 16803 RVA: 0x0010FC0D File Offset: 0x0010EC0D
		[DefaultValue("")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
		[WebCategory("Behavior")]
		[Themeable(false)]
		public virtual string ValidationGroup
		{
			get
			{
				string text = (string)this.ViewState["ValidationGroup"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ValidationGroup"] = value;
			}
		}

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x060041A4 RID: 16804 RVA: 0x0010FC20 File Offset: 0x0010EC20
		// (remove) Token: 0x060041A5 RID: 16805 RVA: 0x0010FC33 File Offset: 0x0010EC33
		[WebCategory("Action")]
		[WebSysDescription("LinkButton_OnClick")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(LinkButton.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(LinkButton.EventClick, value);
			}
		}

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x060041A6 RID: 16806 RVA: 0x0010FC46 File Offset: 0x0010EC46
		// (remove) Token: 0x060041A7 RID: 16807 RVA: 0x0010FC59 File Offset: 0x0010EC59
		[WebCategory("Action")]
		[WebSysDescription("Button_OnCommand")]
		public event CommandEventHandler Command
		{
			add
			{
				base.Events.AddHandler(LinkButton.EventCommand, value);
			}
			remove
			{
				base.Events.RemoveHandler(LinkButton.EventCommand, value);
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0010FC6C File Offset: 0x0010EC6C
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			string text = Util.EnsureEndWithSemiColon(this.OnClientClick);
			if (base.HasAttributes)
			{
				string text2 = base.Attributes["onclick"];
				if (text2 != null)
				{
					text += Util.EnsureEndWithSemiColon(text2);
					base.Attributes.Remove("onclick");
				}
			}
			if (text.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, text);
			}
			bool isEnabled = base.IsEnabled;
			if (this.Enabled && !isEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			base.AddAttributesToRender(writer);
			if (isEnabled && this.Page != null)
			{
				PostBackOptions postBackOptions = this.GetPostBackOptions();
				string text3 = null;
				if (postBackOptions != null)
				{
					text3 = this.Page.ClientScript.GetPostBackEventReference(postBackOptions, true);
				}
				if (string.IsNullOrEmpty(text3))
				{
					text3 = "javascript:void(0)";
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Href, text3);
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0010FD50 File Offset: 0x0010ED50
		protected override void AddParsedSubObject(object obj)
		{
			if (this.HasControls())
			{
				base.AddParsedSubObject(obj);
				return;
			}
			if (!(obj is LiteralControl))
			{
				string text = this.Text;
				if (text.Length != 0)
				{
					this.Text = string.Empty;
					base.AddParsedSubObject(new LiteralControl(text));
				}
				base.AddParsedSubObject(obj);
				return;
			}
			if (base.DesignMode)
			{
				if (this._textSetByAddParsedSubObject)
				{
					this.Text += ((LiteralControl)obj).Text;
				}
				else
				{
					this.Text = ((LiteralControl)obj).Text;
				}
				this._textSetByAddParsedSubObject = true;
				return;
			}
			this.Text = ((LiteralControl)obj).Text;
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x0010FDFC File Offset: 0x0010EDFC
		protected virtual PostBackOptions GetPostBackOptions()
		{
			PostBackOptions postBackOptions = new PostBackOptions(this, string.Empty);
			postBackOptions.RequiresJavaScriptProtocol = true;
			if (!string.IsNullOrEmpty(this.PostBackUrl))
			{
				postBackOptions.ActionUrl = HttpUtility.UrlPathEncode(base.ResolveClientUrl(this.PostBackUrl));
				if (!base.DesignMode && this.Page != null && string.Equals(this.Page.Request.Browser.Browser, "IE", StringComparison.OrdinalIgnoreCase))
				{
					postBackOptions.ActionUrl = Util.QuoteJScriptString(postBackOptions.ActionUrl, true);
				}
			}
			if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
			{
				postBackOptions.PerformValidation = true;
				postBackOptions.ValidationGroup = this.ValidationGroup;
			}
			return postBackOptions;
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0010FEBC File Offset: 0x0010EEBC
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				base.LoadViewState(savedState);
				string text = (string)this.ViewState["Text"];
				if (text != null)
				{
					this.Text = text;
				}
			}
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0010FEF4 File Offset: 0x0010EEF4
		protected virtual void OnClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[LinkButton.EventClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0010FF24 File Offset: 0x0010EF24
		protected virtual void OnCommand(CommandEventArgs e)
		{
			CommandEventHandler commandEventHandler = (CommandEventHandler)base.Events[LinkButton.EventCommand];
			if (commandEventHandler != null)
			{
				commandEventHandler(this, e);
			}
			base.RaiseBubbleEvent(this, e);
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0010FF5A File Offset: 0x0010EF5A
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0010FF64 File Offset: 0x0010EF64
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (this.CausesValidation)
			{
				this.Page.Validate(this.ValidationGroup);
			}
			this.OnClick(EventArgs.Empty);
			this.OnCommand(new CommandEventArgs(this.CommandName, this.CommandArgument));
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0010FFBC File Offset: 0x0010EFBC
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && this.Enabled)
			{
				this.Page.RegisterPostBackScript();
				if ((this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0) || !string.IsNullOrEmpty(this.PostBackUrl))
				{
					this.Page.RegisterWebFormsScript();
				}
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x00110024 File Offset: 0x0010F024
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (base.HasRenderingData())
			{
				base.RenderContents(writer);
				return;
			}
			writer.Write(this.Text);
		}

		// Token: 0x040028B8 RID: 10424
		private bool _textSetByAddParsedSubObject;

		// Token: 0x040028B9 RID: 10425
		private static readonly object EventClick = new object();

		// Token: 0x040028BA RID: 10426
		private static readonly object EventCommand = new object();
	}
}
