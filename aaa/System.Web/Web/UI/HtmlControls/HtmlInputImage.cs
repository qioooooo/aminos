using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A0 RID: 1184
	[SupportsEventValidation]
	[DefaultEvent("ServerClick")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputImage : HtmlInputControl, IPostBackDataHandler, IPostBackEventHandler
	{
		// Token: 0x06003747 RID: 14151 RVA: 0x000ED9A0 File Offset: 0x000EC9A0
		public HtmlInputImage()
			: base("image")
		{
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003748 RID: 14152 RVA: 0x000ED9B0 File Offset: 0x000EC9B0
		// (set) Token: 0x06003749 RID: 14153 RVA: 0x000ED9D8 File Offset: 0x000EC9D8
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Align
		{
			get
			{
				string text = base.Attributes["align"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["align"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x000ED9F0 File Offset: 0x000EC9F0
		// (set) Token: 0x0600374B RID: 14155 RVA: 0x000EDA18 File Offset: 0x000ECA18
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		[Localizable(true)]
		[DefaultValue("")]
		public string Alt
		{
			get
			{
				string text = base.Attributes["alt"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["alt"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x000EDA30 File Offset: 0x000ECA30
		// (set) Token: 0x0600374D RID: 14157 RVA: 0x000EDA5E File Offset: 0x000ECA5E
		[WebCategory("Appearance")]
		[DefaultValue(-1)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Border
		{
			get
			{
				string text = base.Attributes["border"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["border"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000EDA78 File Offset: 0x000ECA78
		// (set) Token: 0x0600374F RID: 14159 RVA: 0x000EDAA0 File Offset: 0x000ECAA0
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[UrlProperty]
		public string Src
		{
			get
			{
				string text = base.Attributes["src"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["src"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x000EDAB8 File Offset: 0x000ECAB8
		// (set) Token: 0x06003751 RID: 14161 RVA: 0x000EDAE1 File Offset: 0x000ECAE1
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

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x000EDAFC File Offset: 0x000ECAFC
		// (set) Token: 0x06003753 RID: 14163 RVA: 0x000EDB29 File Offset: 0x000ECB29
		[WebCategory("Behavior")]
		[WebSysDescription("PostBackControl_ValidationGroup")]
		[DefaultValue("")]
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

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06003754 RID: 14164 RVA: 0x000EDB3C File Offset: 0x000ECB3C
		// (remove) Token: 0x06003755 RID: 14165 RVA: 0x000EDB4F File Offset: 0x000ECB4F
		[WebCategory("Action")]
		[WebSysDescription("HtmlInputImage_OnServerClick")]
		public event ImageClickEventHandler ServerClick
		{
			add
			{
				base.Events.AddHandler(HtmlInputImage.EventServerClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(HtmlInputImage.EventServerClick, value);
			}
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000EDB62 File Offset: 0x000ECB62
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null)
			{
				if (!base.Disabled)
				{
					this.Page.RegisterRequiresPostBack(this);
				}
				if (this.CausesValidation)
				{
					this.Page.RegisterPostBackScript();
				}
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000EDB9C File Offset: 0x000ECB9C
		protected virtual void OnServerClick(ImageClickEventArgs e)
		{
			ImageClickEventHandler imageClickEventHandler = (ImageClickEventHandler)base.Events[HtmlInputImage.EventServerClick];
			if (imageClickEventHandler != null)
			{
				imageClickEventHandler(this, e);
			}
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000EDBCA File Offset: 0x000ECBCA
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000EDBD3 File Offset: 0x000ECBD3
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			if (this.CausesValidation)
			{
				this.Page.Validate(this.ValidationGroup);
			}
			this.OnServerClick(new ImageClickEventArgs(this._x, this._y));
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000EDC05 File Offset: 0x000ECC05
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000EDC10 File Offset: 0x000ECC10
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string text = postCollection[this.RenderedNameAttribute + ".x"];
			string text2 = postCollection[this.RenderedNameAttribute + ".y"];
			if (text != null && text2 != null && text.Length > 0 && text2.Length > 0)
			{
				base.ValidateEvent(this.UniqueID);
				this._x = int.Parse(text, CultureInfo.InvariantCulture);
				this._y = int.Parse(text2, CultureInfo.InvariantCulture);
				this.Page.RegisterRequiresRaiseEvent(this);
			}
			return false;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000EDC9E File Offset: 0x000ECC9E
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000EDCA6 File Offset: 0x000ECCA6
		protected virtual void RaisePostDataChangedEvent()
		{
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000EDCA8 File Offset: 0x000ECCA8
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			base.PreProcessRelativeReferenceAttribute(writer, "src");
			if (this.Page != null)
			{
				Util.WriteOnClickAttribute(writer, this, true, false, this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0, this.ValidationGroup);
			}
			base.RenderAttributes(writer);
		}

		// Token: 0x040025C6 RID: 9670
		private static readonly object EventServerClick = new object();

		// Token: 0x040025C7 RID: 9671
		private int _x;

		// Token: 0x040025C8 RID: 9672
		private int _y;
	}
}
