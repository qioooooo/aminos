using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x0200049C RID: 1180
	[DefaultEvent("ServerClick")]
	[SupportsEventValidation]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputButton : HtmlInputControl, IPostBackEventHandler
	{
		// Token: 0x06003711 RID: 14097 RVA: 0x000ED30F File Offset: 0x000EC30F
		public HtmlInputButton()
			: base("button")
		{
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000ED31C File Offset: 0x000EC31C
		public HtmlInputButton(string type)
			: base(type)
		{
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x000ED328 File Offset: 0x000EC328
		// (set) Token: 0x06003714 RID: 14100 RVA: 0x000ED351 File Offset: 0x000EC351
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

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003715 RID: 14101 RVA: 0x000ED36C File Offset: 0x000EC36C
		// (set) Token: 0x06003716 RID: 14102 RVA: 0x000ED399 File Offset: 0x000EC399
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

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06003717 RID: 14103 RVA: 0x000ED3AC File Offset: 0x000EC3AC
		// (remove) Token: 0x06003718 RID: 14104 RVA: 0x000ED3BF File Offset: 0x000EC3BF
		[WebSysDescription("HtmlControl_OnServerClick")]
		[WebCategory("Action")]
		public event EventHandler ServerClick
		{
			add
			{
				base.Events.AddHandler(HtmlInputButton.EventServerClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlInputButton.EventServerClick, value);
			}
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000ED3D2 File Offset: 0x000EC3D2
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && base.Events[HtmlInputButton.EventServerClick] != null)
			{
				this.Page.RegisterPostBackScript();
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000ED400 File Offset: 0x000EC400
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			this.RenderAttributesInternal(writer);
			base.RenderAttributes(writer);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000ED410 File Offset: 0x000EC410
		internal virtual void RenderAttributesInternal(HtmlTextWriter writer)
		{
			bool flag = base.Events[HtmlInputButton.EventServerClick] != null;
			if (this.Page != null)
			{
				if (flag)
				{
					Util.WriteOnClickAttribute(writer, this, false, flag, this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0, this.ValidationGroup);
					return;
				}
				this.Page.ClientScript.RegisterForEventValidation(this.UniqueID);
			}
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000ED48C File Offset: 0x000EC48C
		protected virtual void OnServerClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlInputButton.EventServerClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000ED4BA File Offset: 0x000EC4BA
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000ED4C3 File Offset: 0x000EC4C3
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (this.CausesValidation)
			{
				this.Page.Validate(this.ValidationGroup);
			}
			this.OnServerClick(EventArgs.Empty);
		}

		// Token: 0x040025C3 RID: 9667
		private static readonly object EventServerClick = new object();
	}
}
