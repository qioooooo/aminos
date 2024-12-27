using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x02000492 RID: 1170
	[SupportsEventValidation]
	[DefaultEvent("ServerClick")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlButton : HtmlContainerControl, IPostBackEventHandler
	{
		// Token: 0x060036D0 RID: 14032 RVA: 0x000EC8FA File Offset: 0x000EB8FA
		public HtmlButton()
			: base("button")
		{
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060036D1 RID: 14033 RVA: 0x000EC908 File Offset: 0x000EB908
		// (set) Token: 0x060036D2 RID: 14034 RVA: 0x000EC931 File Offset: 0x000EB931
		[WebCategory("Behavior")]
		[DefaultValue(true)]
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

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060036D3 RID: 14035 RVA: 0x000EC94C File Offset: 0x000EB94C
		// (set) Token: 0x060036D4 RID: 14036 RVA: 0x000EC979 File Offset: 0x000EB979
		[DefaultValue("")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
		[WebCategory("Behavior")]
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

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060036D5 RID: 14037 RVA: 0x000EC98C File Offset: 0x000EB98C
		// (remove) Token: 0x060036D6 RID: 14038 RVA: 0x000EC99F File Offset: 0x000EB99F
		[WebSysDescription("HtmlControl_OnServerClick")]
		[WebCategory("Action")]
		public event EventHandler ServerClick
		{
			add
			{
				base.Events.AddHandler(HtmlButton.EventServerClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlButton.EventServerClick, value);
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000EC9B2 File Offset: 0x000EB9B2
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && base.Events[HtmlButton.EventServerClick] != null)
			{
				this.Page.RegisterPostBackScript();
			}
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000EC9E0 File Offset: 0x000EB9E0
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			bool flag = base.Events[HtmlButton.EventServerClick] != null;
			if (this.Page != null && flag)
			{
				Util.WriteOnClickAttribute(writer, this, false, true, this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0, this.ValidationGroup);
			}
			base.RenderAttributes(writer);
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000ECA4C File Offset: 0x000EBA4C
		protected virtual void OnServerClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlButton.EventServerClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000ECA7A File Offset: 0x000EBA7A
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000ECA83 File Offset: 0x000EBA83
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (this.CausesValidation)
			{
				this.Page.Validate(this.ValidationGroup);
			}
			this.OnServerClick(EventArgs.Empty);
		}

		// Token: 0x040025B4 RID: 9652
		private static readonly object EventServerClick = new object();
	}
}
