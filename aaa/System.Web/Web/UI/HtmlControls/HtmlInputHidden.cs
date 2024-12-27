using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x0200049F RID: 1183
	[SupportsEventValidation]
	[DefaultEvent("ServerChange")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputHidden : HtmlInputControl, IPostBackDataHandler
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x000ED85B File Offset: 0x000EC85B
		public HtmlInputHidden()
			: base("hidden")
		{
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x0600373D RID: 14141 RVA: 0x000ED868 File Offset: 0x000EC868
		// (remove) Token: 0x0600373E RID: 14142 RVA: 0x000ED87B File Offset: 0x000EC87B
		[WebCategory("Action")]
		[WebSysDescription("HtmlInputHidden_OnServerChange")]
		public event EventHandler ServerChange
		{
			add
			{
				base.Events.AddHandler(HtmlInputHidden.EventServerChange, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlInputHidden.EventServerChange, value);
			}
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000ED890 File Offset: 0x000EC890
		protected virtual void OnServerChange(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlInputHidden.EventServerChange];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000ED8C0 File Offset: 0x000EC8C0
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!base.Disabled)
			{
				if (base.Events[HtmlInputHidden.EventServerChange] == null)
				{
					this.ViewState.SetItemDirty("value", false);
				}
				if (this.Page != null)
				{
					this.Page.RegisterEnabledControl(this);
				}
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000ED913 File Offset: 0x000EC913
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000ED920 File Offset: 0x000EC920
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string value = this.Value;
			string text = postCollection.GetValues(postDataKey)[0];
			if (!value.Equals(text))
			{
				base.ValidateEvent(postDataKey);
				this.Value = text;
				return true;
			}
			return false;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000ED958 File Offset: 0x000EC958
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			base.RenderAttributes(writer);
			if (this.Page != null)
			{
				this.Page.ClientScript.RegisterForEventValidation(this.RenderedNameAttribute);
			}
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000ED97F File Offset: 0x000EC97F
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000ED987 File Offset: 0x000EC987
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnServerChange(EventArgs.Empty);
		}

		// Token: 0x040025C5 RID: 9669
		private static readonly object EventServerChange = new object();
	}
}
