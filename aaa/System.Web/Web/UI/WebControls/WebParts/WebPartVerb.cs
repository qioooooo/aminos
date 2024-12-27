using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000703 RID: 1795
	[TypeConverter(typeof(EmptyStringExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartVerb : IStateManager
	{
		// Token: 0x06005784 RID: 22404 RVA: 0x0016105A File Offset: 0x0016005A
		internal WebPartVerb()
		{
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x00161069 File Offset: 0x00160069
		private WebPartVerb(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("id");
			}
			this._id = id;
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x00161092 File Offset: 0x00160092
		public WebPartVerb(string id, WebPartEventHandler serverClickHandler)
			: this(id)
		{
			if (serverClickHandler == null)
			{
				throw new ArgumentNullException("serverClickHandler");
			}
			this._serverClickHandler = serverClickHandler;
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001610B0 File Offset: 0x001600B0
		public WebPartVerb(string id, string clientClickHandler)
			: this(id)
		{
			if (string.IsNullOrEmpty(clientClickHandler))
			{
				throw new ArgumentNullException("clientClickHandler");
			}
			this._clientClickHandler = clientClickHandler;
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001610D3 File Offset: 0x001600D3
		public WebPartVerb(string id, WebPartEventHandler serverClickHandler, string clientClickHandler)
			: this(id)
		{
			if (serverClickHandler == null)
			{
				throw new ArgumentNullException("serverClickHandler");
			}
			if (string.IsNullOrEmpty(clientClickHandler))
			{
				throw new ArgumentNullException("clientClickHandler");
			}
			this._serverClickHandler = serverClickHandler;
			this._clientClickHandler = clientClickHandler;
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06005789 RID: 22409 RVA: 0x0016110C File Offset: 0x0016010C
		// (set) Token: 0x0600578A RID: 22410 RVA: 0x00161135 File Offset: 0x00160135
		[DefaultValue(false)]
		[Themeable(false)]
		[WebSysDescription("WebPartVerb_Checked")]
		[NotifyParentProperty(true)]
		public virtual bool Checked
		{
			get
			{
				object obj = this.ViewState["Checked"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["Checked"] = value;
			}
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x0600578B RID: 22411 RVA: 0x0016114D File Offset: 0x0016014D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string ClientClickHandler
		{
			get
			{
				if (this._clientClickHandler != null)
				{
					return this._clientClickHandler;
				}
				return string.Empty;
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x0600578C RID: 22412 RVA: 0x00161164 File Offset: 0x00160164
		// (set) Token: 0x0600578D RID: 22413 RVA: 0x00161191 File Offset: 0x00160191
		[NotifyParentProperty(true)]
		[Localizable(true)]
		[WebSysDefaultValue("")]
		[WebSysDescription("WebPartVerb_Description")]
		public virtual string Description
		{
			get
			{
				object obj = this.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Description"] = value;
			}
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x0600578E RID: 22414 RVA: 0x001611A4 File Offset: 0x001601A4
		// (set) Token: 0x0600578F RID: 22415 RVA: 0x001611CD File Offset: 0x001601CD
		[DefaultValue(true)]
		[Themeable(false)]
		[WebSysDescription("WebPartVerb_Enabled")]
		[NotifyParentProperty(true)]
		public virtual bool Enabled
		{
			get
			{
				object obj = this.ViewState["Enabled"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["Enabled"] = value;
			}
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06005790 RID: 22416 RVA: 0x001611E5 File Offset: 0x001601E5
		// (set) Token: 0x06005791 RID: 22417 RVA: 0x001611FB File Offset: 0x001601FB
		internal string EventArgument
		{
			get
			{
				if (this._eventArgument == null)
				{
					return string.Empty;
				}
				return this._eventArgument;
			}
			set
			{
				this._eventArgument = value;
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x00161204 File Offset: 0x00160204
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string ID
		{
			get
			{
				if (this._id == null)
				{
					return string.Empty;
				}
				return this._id;
			}
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x06005793 RID: 22419 RVA: 0x0016121C File Offset: 0x0016021C
		// (set) Token: 0x06005794 RID: 22420 RVA: 0x00161249 File Offset: 0x00160249
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebSysDescription("WebPartVerb_ImageUrl")]
		public virtual string ImageUrl
		{
			get
			{
				object obj = this.ViewState["ImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ImageUrl"] = value;
			}
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x06005795 RID: 22421 RVA: 0x0016125C File Offset: 0x0016025C
		protected virtual bool IsTrackingViewState
		{
			get
			{
				return this._isTrackingViewState;
			}
		}

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x06005796 RID: 22422 RVA: 0x00161264 File Offset: 0x00160264
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WebPartEventHandler ServerClickHandler
		{
			get
			{
				return this._serverClickHandler;
			}
		}

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x06005797 RID: 22423 RVA: 0x0016126C File Offset: 0x0016026C
		// (set) Token: 0x06005798 RID: 22424 RVA: 0x00161299 File Offset: 0x00160299
		[NotifyParentProperty(true)]
		[WebSysDescription("WebPartVerb_Text")]
		[Localizable(true)]
		[WebSysDefaultValue("")]
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
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x06005799 RID: 22425 RVA: 0x001612AC File Offset: 0x001602AC
		// (set) Token: 0x0600579A RID: 22426 RVA: 0x001612B4 File Offset: 0x001602B4
		[DefaultValue(true)]
		[Themeable(false)]
		[WebSysDescription("WebPartVerb_Visible")]
		[NotifyParentProperty(true)]
		public virtual bool Visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				this._visible = value;
				this.ViewState["Visible"] = value;
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x0600579B RID: 22427 RVA: 0x001612D3 File Offset: 0x001602D3
		protected StateBag ViewState
		{
			get
			{
				if (this._viewState == null)
				{
					this._viewState = new StateBag(false);
					if (this._isTrackingViewState)
					{
						((IStateManager)this._viewState).TrackViewState();
					}
				}
				return this._viewState;
			}
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x00161304 File Offset: 0x00160304
		internal string GetEventArgument(string webPartID)
		{
			if (string.IsNullOrEmpty(this._eventArgumentPrefix))
			{
				return string.Empty;
			}
			if (this._id == null)
			{
				return this._eventArgumentPrefix + webPartID;
			}
			return this._eventArgumentPrefix + this._id + ":" + webPartID;
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x00161350 File Offset: 0x00160350
		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				((IStateManager)this.ViewState).LoadViewState(savedState);
				object obj = this.ViewState["Visible"];
				if (obj != null)
				{
					this._visible = (bool)obj;
				}
			}
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x0016138C File Offset: 0x0016038C
		protected virtual object SaveViewState()
		{
			if (this._viewState != null)
			{
				return ((IStateManager)this._viewState).SaveViewState();
			}
			return null;
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x001613A3 File Offset: 0x001603A3
		internal void SetEventArgumentPrefix(string eventArgumentPrefix)
		{
			this._eventArgumentPrefix = eventArgumentPrefix;
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x001613AC File Offset: 0x001603AC
		protected virtual void TrackViewState()
		{
			this._isTrackingViewState = true;
			if (this._viewState != null)
			{
				((IStateManager)this._viewState).TrackViewState();
			}
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x001613C8 File Offset: 0x001603C8
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.IsTrackingViewState;
			}
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x001613D0 File Offset: 0x001603D0
		void IStateManager.LoadViewState(object savedState)
		{
			this.LoadViewState(savedState);
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x001613D9 File Offset: 0x001603D9
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x001613E1 File Offset: 0x001603E1
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x04002FA5 RID: 12197
		private bool _isTrackingViewState;

		// Token: 0x04002FA6 RID: 12198
		private StateBag _viewState;

		// Token: 0x04002FA7 RID: 12199
		private bool _visible = true;

		// Token: 0x04002FA8 RID: 12200
		private string _id;

		// Token: 0x04002FA9 RID: 12201
		private string _clientClickHandler;

		// Token: 0x04002FAA RID: 12202
		private WebPartEventHandler _serverClickHandler;

		// Token: 0x04002FAB RID: 12203
		private string _eventArgument;

		// Token: 0x04002FAC RID: 12204
		private string _eventArgumentPrefix;
	}
}
