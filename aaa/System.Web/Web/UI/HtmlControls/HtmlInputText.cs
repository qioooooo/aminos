using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A1 RID: 1185
	[ValidationProperty("Value")]
	[SupportsEventValidation]
	[DefaultEvent("ServerChange")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputText : HtmlInputControl, IPostBackDataHandler
	{
		// Token: 0x06003760 RID: 14176 RVA: 0x000EDD0F File Offset: 0x000ECD0F
		public HtmlInputText()
			: base("text")
		{
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000EDD1C File Offset: 0x000ECD1C
		public HtmlInputText(string type)
			: base(type)
		{
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x000EDD28 File Offset: 0x000ECD28
		// (set) Token: 0x06003763 RID: 14179 RVA: 0x000EDD5B File Offset: 0x000ECD5B
		[WebCategory("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		public int MaxLength
		{
			get
			{
				string text = (string)this.ViewState["maxlength"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["maxlength"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x000EDD74 File Offset: 0x000ECD74
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x000EDDA2 File Offset: 0x000ECDA2
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(-1)]
		public int Size
		{
			get
			{
				string text = base.Attributes["size"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["size"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x000EDDBC File Offset: 0x000ECDBC
		// (set) Token: 0x06003767 RID: 14183 RVA: 0x000EDDE4 File Offset: 0x000ECDE4
		public override string Value
		{
			get
			{
				string text = base.Attributes["value"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["value"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06003768 RID: 14184 RVA: 0x000EDDFC File Offset: 0x000ECDFC
		// (remove) Token: 0x06003769 RID: 14185 RVA: 0x000EDE0F File Offset: 0x000ECE0F
		[WebSysDescription("HtmlInputText_ServerChange")]
		[WebCategory("Action")]
		public event EventHandler ServerChange
		{
			add
			{
				base.Events.AddHandler(HtmlInputText.EventServerChange, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlInputText.EventServerChange, value);
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000EDE24 File Offset: 0x000ECE24
		protected virtual void OnServerChange(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[HtmlInputText.EventServerChange];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000EDE54 File Offset: 0x000ECE54
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			bool disabled = base.Disabled;
			if (!disabled && this.Page != null)
			{
				this.Page.RegisterEnabledControl(this);
			}
			if ((!disabled && base.Events[HtmlInputText.EventServerChange] == null) || base.Type.Equals("password", StringComparison.OrdinalIgnoreCase))
			{
				this.ViewState.SetItemDirty("value", false);
			}
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000EDEBF File Offset: 0x000ECEBF
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			base.RenderAttributes(writer);
			if (this.Page != null)
			{
				this.Page.ClientScript.RegisterForEventValidation(this.RenderedNameAttribute);
			}
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000EDEE6 File Offset: 0x000ECEE6
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000EDEF0 File Offset: 0x000ECEF0
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

		// Token: 0x0600376F RID: 14191 RVA: 0x000EDF28 File Offset: 0x000ECF28
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x000EDF30 File Offset: 0x000ECF30
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnServerChange(EventArgs.Empty);
		}

		// Token: 0x040025C9 RID: 9673
		private static readonly object EventServerChange = new object();
	}
}
