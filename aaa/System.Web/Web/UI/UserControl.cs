using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.SessionState;

namespace System.Web.UI
{
	// Token: 0x02000429 RID: 1065
	[DefaultEvent("Load")]
	[Designer("System.Web.UI.Design.UserControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IDesigner))]
	[Designer("Microsoft.VisualStudio.Web.WebForms.WebFormDesigner, Microsoft.VisualStudio.Web, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerCategory("ASPXCodeBehind")]
	[DesignerSerializer("Microsoft.VisualStudio.Web.WebForms.WebFormCodeDomSerializer, Microsoft.VisualStudio.Web, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.TypeCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ParseChildren(true)]
	[ToolboxItem(false)]
	[ControlBuilder(typeof(UserControlControlBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class UserControl : TemplateControl, IAttributeAccessor, INonBindingContainer, INamingContainer, IUserControlDesignerAccessor
	{
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x000DDF1C File Offset: 0x000DCF1C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AttributeCollection Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					if (this.attributeStorage == null)
					{
						this.attributeStorage = new StateBag(true);
						if (base.IsTrackingViewState)
						{
							this.attributeStorage.TrackViewState();
						}
					}
					this.attributes = new AttributeCollection(this.attributeStorage);
				}
				return this.attributes;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x000DDF6F File Offset: 0x000DCF6F
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpApplicationState Application
		{
			get
			{
				return this.Page.Application;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x000DDF7C File Offset: 0x000DCF7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public TraceContext Trace
		{
			get
			{
				return this.Page.Trace;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x000DDF89 File Offset: 0x000DCF89
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpRequest Request
		{
			get
			{
				return this.Page.Request;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x000DDF96 File Offset: 0x000DCF96
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpResponse Response
		{
			get
			{
				return this.Page.Response;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x000DDFA3 File Offset: 0x000DCFA3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpServerUtility Server
		{
			get
			{
				return this.Page.Server;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600331A RID: 13082 RVA: 0x000DDFB0 File Offset: 0x000DCFB0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cache Cache
		{
			get
			{
				return this.Page.Cache;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x000DDFC0 File Offset: 0x000DCFC0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ControlCachePolicy CachePolicy
		{
			get
			{
				BasePartialCachingControl basePartialCachingControl = this.Parent as BasePartialCachingControl;
				if (basePartialCachingControl != null)
				{
					return basePartialCachingControl.CachePolicy;
				}
				return ControlCachePolicy.GetCachePolicyStub();
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x000DDFE8 File Offset: 0x000DCFE8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPostBack
		{
			get
			{
				return this.Page.IsPostBack;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x0600331D RID: 13085 RVA: 0x000DDFF5 File Offset: 0x000DCFF5
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpSessionState Session
		{
			get
			{
				return this.Page.Session;
			}
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000DE002 File Offset: 0x000DD002
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void DesignerInitialize()
		{
			this.InitRecursive(null);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000DE00C File Offset: 0x000DD00C
		protected internal override void OnInit(EventArgs e)
		{
			bool flag = base.DesignMode;
			if (!flag && this.Page != null && this.Page.Site != null)
			{
				flag = this.Page.Site.DesignMode;
			}
			if (!flag)
			{
				this.InitializeAsUserControlInternal();
			}
			base.OnInit(e);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000DE059 File Offset: 0x000DD059
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void InitializeAsUserControl(Page page)
		{
			this._page = page;
			this.InitializeAsUserControlInternal();
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000DE068 File Offset: 0x000DD068
		internal void InitializeAsUserControlInternal()
		{
			if (this._fUserControlInitialized)
			{
				return;
			}
			this._fUserControlInitialized = true;
			base.HookUpAutomaticHandlers();
			this.FrameworkInitialize();
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000DE088 File Offset: 0x000DD088
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				Pair pair = (Pair)savedState;
				base.LoadViewState(pair.First);
				if (pair.Second != null)
				{
					if (this.attributeStorage == null)
					{
						this.attributeStorage = new StateBag(true);
						this.attributeStorage.TrackViewState();
					}
					this.attributeStorage.LoadViewState(pair.Second);
				}
			}
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000DE0E4 File Offset: 0x000DD0E4
		protected override object SaveViewState()
		{
			Pair pair = null;
			object obj = base.SaveViewState();
			object obj2 = null;
			if (this.attributeStorage != null)
			{
				obj2 = this.attributeStorage.SaveViewState();
			}
			if (obj != null || obj2 != null)
			{
				pair = new Pair(obj, obj2);
			}
			return pair;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000DE11F File Offset: 0x000DD11F
		string IAttributeAccessor.GetAttribute(string name)
		{
			if (this.attributeStorage == null)
			{
				return null;
			}
			return (string)this.attributeStorage[name];
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000DE13C File Offset: 0x000DD13C
		void IAttributeAccessor.SetAttribute(string name, string value)
		{
			this.Attributes[name] = value;
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000DE14B File Offset: 0x000DD14B
		public string MapPath(string virtualPath)
		{
			return this.Request.MapPath(VirtualPath.CreateAllowNull(virtualPath), base.TemplateControlVirtualDirectory, true);
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x000DE168 File Offset: 0x000DD168
		// (set) Token: 0x06003328 RID: 13096 RVA: 0x000DE195 File Offset: 0x000DD195
		string IUserControlDesignerAccessor.TagName
		{
			get
			{
				string text = (string)this.ViewState["!DesignTimeTagName"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["!DesignTimeTagName"] = value;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003329 RID: 13097 RVA: 0x000DE1A8 File Offset: 0x000DD1A8
		// (set) Token: 0x0600332A RID: 13098 RVA: 0x000DE1D5 File Offset: 0x000DD1D5
		string IUserControlDesignerAccessor.InnerText
		{
			get
			{
				string text = (string)this.ViewState["!DesignTimeInnerText"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["!DesignTimeInnerText"] = value;
			}
		}

		// Token: 0x040023E7 RID: 9191
		private StateBag attributeStorage;

		// Token: 0x040023E8 RID: 9192
		private AttributeCollection attributes;

		// Token: 0x040023E9 RID: 9193
		private bool _fUserControlInitialized;
	}
}
