using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x02000491 RID: 1169
	[SupportsEventValidation]
	[DefaultEvent("ServerClick")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlAnchor : HtmlContainerControl, IPostBackEventHandler
	{
		// Token: 0x060036BA RID: 14010 RVA: 0x000EC595 File Offset: 0x000EB595
		public HtmlAnchor()
			: base("a")
		{
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060036BB RID: 14011 RVA: 0x000EC5A4 File Offset: 0x000EB5A4
		// (set) Token: 0x060036BC RID: 14012 RVA: 0x000EC5CD File Offset: 0x000EB5CD
		[DefaultValue(true)]
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

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x000EC5E8 File Offset: 0x000EB5E8
		// (set) Token: 0x060036BE RID: 14014 RVA: 0x000EC610 File Offset: 0x000EB610
		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Navigation")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HRef
		{
			get
			{
				string text = base.Attributes["href"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["href"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x000EC628 File Offset: 0x000EB628
		// (set) Token: 0x060036C0 RID: 14016 RVA: 0x000EC650 File Offset: 0x000EB650
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Navigation")]
		public string Name
		{
			get
			{
				string text = base.Attributes["name"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["name"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x000EC668 File Offset: 0x000EB668
		// (set) Token: 0x060036C2 RID: 14018 RVA: 0x000EC690 File Offset: 0x000EB690
		[WebCategory("Navigation")]
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Target
		{
			get
			{
				string text = base.Attributes["target"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["target"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060036C3 RID: 14019 RVA: 0x000EC6A8 File Offset: 0x000EB6A8
		// (set) Token: 0x060036C4 RID: 14020 RVA: 0x000EC6D0 File Offset: 0x000EB6D0
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(true)]
		[DefaultValue("")]
		public string Title
		{
			get
			{
				string text = base.Attributes["title"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["title"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060036C5 RID: 14021 RVA: 0x000EC6E8 File Offset: 0x000EB6E8
		// (set) Token: 0x060036C6 RID: 14022 RVA: 0x000EC715 File Offset: 0x000EB715
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
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

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060036C7 RID: 14023 RVA: 0x000EC728 File Offset: 0x000EB728
		// (remove) Token: 0x060036C8 RID: 14024 RVA: 0x000EC73B File Offset: 0x000EB73B
		[WebCategory("Action")]
		[WebSysDescription("HtmlControl_OnServerClick")]
		public event EventHandler ServerClick
		{
			add
			{
				base.Events.AddHandler(HtmlAnchor.EventServerClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlAnchor.EventServerClick, value);
			}
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000EC750 File Offset: 0x000EB750
		private PostBackOptions GetPostBackOptions()
		{
			PostBackOptions postBackOptions = new PostBackOptions(this, string.Empty);
			postBackOptions.RequiresJavaScriptProtocol = true;
			if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
			{
				postBackOptions.PerformValidation = true;
				postBackOptions.ValidationGroup = this.ValidationGroup;
			}
			return postBackOptions;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000EC7A8 File Offset: 0x000EB7A8
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && base.Events[HtmlAnchor.EventServerClick] != null)
			{
				this.Page.RegisterPostBackScript();
				if (this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0)
				{
					this.Page.RegisterWebFormsScript();
				}
			}
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000EC810 File Offset: 0x000EB810
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			if (base.Events[HtmlAnchor.EventServerClick] != null)
			{
				base.Attributes.Remove("href");
				base.RenderAttributes(writer);
				PostBackOptions postBackOptions = this.GetPostBackOptions();
				string postBackEventReference = this.Page.ClientScript.GetPostBackEventReference(postBackOptions, true);
				writer.WriteAttribute("href", postBackEventReference, true);
				return;
			}
			base.PreProcessRelativeReferenceAttribute(writer, "href");
			base.RenderAttributes(writer);
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000EC884 File Offset: 0x000EB884
		protected virtual void OnServerClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlAnchor.EventServerClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000EC8B2 File Offset: 0x000EB8B2
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000EC8BB File Offset: 0x000EB8BB
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (this.CausesValidation)
			{
				this.Page.Validate(this.ValidationGroup);
			}
			this.OnServerClick(EventArgs.Empty);
		}

		// Token: 0x040025B3 RID: 9651
		private static readonly object EventServerClick = new object();
	}
}
