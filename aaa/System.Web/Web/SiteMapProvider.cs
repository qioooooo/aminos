using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000D2 RID: 210
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class SiteMapProvider : ProviderBase
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00029D40 File Offset: 0x00028D40
		public virtual SiteMapNode CurrentNode
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				SiteMapNode siteMapNode = this.ResolveSiteMapNode(httpContext);
				if (siteMapNode == null)
				{
					siteMapNode = this.FindSiteMapNode(httpContext);
				}
				return this.ReturnNodeIfAccessible(siteMapNode);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00029D6F File Offset: 0x00028D6F
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00029D77 File Offset: 0x00028D77
		public bool EnableLocalization
		{
			get
			{
				return this._enableLocalization;
			}
			set
			{
				this._enableLocalization = value;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00029D80 File Offset: 0x00028D80
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x00029D88 File Offset: 0x00028D88
		public virtual SiteMapProvider ParentProvider
		{
			get
			{
				return this._parentProvider;
			}
			set
			{
				this._parentProvider = value;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00029D91 File Offset: 0x00028D91
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x00029D99 File Offset: 0x00028D99
		public string ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				this._resourceKey = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00029DA4 File Offset: 0x00028DA4
		public virtual SiteMapProvider RootProvider
		{
			get
			{
				if (this._rootProvider == null)
				{
					lock (this._lock)
					{
						if (this._rootProvider == null)
						{
							Hashtable hashtable = new Hashtable();
							SiteMapProvider siteMapProvider = this;
							hashtable.Add(siteMapProvider, null);
							while (siteMapProvider.ParentProvider != null)
							{
								if (hashtable.Contains(siteMapProvider.ParentProvider))
								{
									throw new ProviderException(SR.GetString("SiteMapProvider_Circular_Provider"));
								}
								siteMapProvider = siteMapProvider.ParentProvider;
								hashtable.Add(siteMapProvider, null);
							}
							this._rootProvider = siteMapProvider;
						}
					}
				}
				return this._rootProvider;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00029E3C File Offset: 0x00028E3C
		public virtual SiteMapNode RootNode
		{
			get
			{
				SiteMapNode rootNodeCore = this.GetRootNodeCore();
				return this.ReturnNodeIfAccessible(rootNodeCore);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00029E57 File Offset: 0x00028E57
		public bool SecurityTrimmingEnabled
		{
			get
			{
				return this._securityTrimmingEnabled;
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060009A5 RID: 2469 RVA: 0x00029E5F File Offset: 0x00028E5F
		// (remove) Token: 0x060009A6 RID: 2470 RVA: 0x00029E78 File Offset: 0x00028E78
		public event SiteMapResolveEventHandler SiteMapResolve;

		// Token: 0x060009A7 RID: 2471 RVA: 0x00029E91 File Offset: 0x00028E91
		protected virtual void AddNode(SiteMapNode node)
		{
			this.AddNode(node, null);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00029E9B File Offset: 0x00028E9B
		protected internal virtual void AddNode(SiteMapNode node, SiteMapNode parentNode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00029EA4 File Offset: 0x00028EA4
		public virtual SiteMapNode FindSiteMapNode(HttpContext context)
		{
			if (context == null)
			{
				return null;
			}
			string rawUrl = context.Request.RawUrl;
			SiteMapNode siteMapNode = this.FindSiteMapNode(rawUrl);
			if (siteMapNode == null)
			{
				int num = rawUrl.IndexOf("?", StringComparison.Ordinal);
				if (num != -1)
				{
					siteMapNode = this.FindSiteMapNode(rawUrl.Substring(0, num));
				}
				if (siteMapNode == null)
				{
					Page page = context.CurrentHandler as Page;
					if (page != null)
					{
						string clientQueryString = page.ClientQueryString;
						if (clientQueryString.Length > 0)
						{
							siteMapNode = this.FindSiteMapNode(context.Request.Path + "?" + clientQueryString);
						}
					}
					if (siteMapNode == null)
					{
						siteMapNode = this.FindSiteMapNode(context.Request.Path);
					}
				}
			}
			return siteMapNode;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00029F47 File Offset: 0x00028F47
		public virtual SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			return this.FindSiteMapNode(key);
		}

		// Token: 0x060009AB RID: 2475
		public abstract SiteMapNode FindSiteMapNode(string rawUrl);

		// Token: 0x060009AC RID: 2476
		public abstract SiteMapNodeCollection GetChildNodes(SiteMapNode node);

		// Token: 0x060009AD RID: 2477 RVA: 0x00029F50 File Offset: 0x00028F50
		public virtual SiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
		{
			if (upLevel < -1)
			{
				throw new ArgumentOutOfRangeException("upLevel");
			}
			return this.CurrentNode;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00029F67 File Offset: 0x00028F67
		public virtual SiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
		{
			if (upLevel < -1)
			{
				throw new ArgumentOutOfRangeException("upLevel");
			}
			if (downLevel < -1)
			{
				throw new ArgumentOutOfRangeException("downLevel");
			}
			return this.CurrentNode;
		}

		// Token: 0x060009AF RID: 2479
		public abstract SiteMapNode GetParentNode(SiteMapNode node);

		// Token: 0x060009B0 RID: 2480 RVA: 0x00029F90 File Offset: 0x00028F90
		public virtual SiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
		{
			if (walkupLevels < 0)
			{
				throw new ArgumentOutOfRangeException("walkupLevels");
			}
			if (relativeDepthFromWalkup < 0)
			{
				throw new ArgumentOutOfRangeException("relativeDepthFromWalkup");
			}
			SiteMapNode currentNodeAndHintAncestorNodes = this.GetCurrentNodeAndHintAncestorNodes(walkupLevels);
			if (currentNodeAndHintAncestorNodes == null)
			{
				return null;
			}
			SiteMapNode parentNodesInternal = this.GetParentNodesInternal(currentNodeAndHintAncestorNodes, walkupLevels);
			if (parentNodesInternal == null)
			{
				return null;
			}
			this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
			return parentNodesInternal;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00029FE0 File Offset: 0x00028FE0
		public virtual SiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(SiteMapNode node, int walkupLevels, int relativeDepthFromWalkup)
		{
			if (walkupLevels < 0)
			{
				throw new ArgumentOutOfRangeException("walkupLevels");
			}
			if (relativeDepthFromWalkup < 0)
			{
				throw new ArgumentOutOfRangeException("relativeDepthFromWalkup");
			}
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.HintAncestorNodes(node, walkupLevels);
			SiteMapNode parentNodesInternal = this.GetParentNodesInternal(node, walkupLevels);
			if (parentNodesInternal == null)
			{
				return null;
			}
			this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
			return parentNodesInternal;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002A039 File Offset: 0x00029039
		private SiteMapNode GetParentNodesInternal(SiteMapNode node, int walkupLevels)
		{
			if (walkupLevels <= 0)
			{
				return node;
			}
			do
			{
				node = node.ParentNode;
				walkupLevels--;
			}
			while (node != null && walkupLevels != 0);
			return node;
		}

		// Token: 0x060009B3 RID: 2483
		protected internal abstract SiteMapNode GetRootNodeCore();

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002A055 File Offset: 0x00029055
		protected static SiteMapNode GetRootNodeCoreFromProvider(SiteMapProvider provider)
		{
			return provider.GetRootNodeCore();
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002A05D File Offset: 0x0002905D
		public virtual void HintAncestorNodes(SiteMapNode node, int upLevel)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (upLevel < -1)
			{
				throw new ArgumentOutOfRangeException("upLevel");
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002A07C File Offset: 0x0002907C
		public virtual void HintNeighborhoodNodes(SiteMapNode node, int upLevel, int downLevel)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (upLevel < -1)
			{
				throw new ArgumentOutOfRangeException("upLevel");
			}
			if (downLevel < -1)
			{
				throw new ArgumentOutOfRangeException("downLevel");
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002A0AC File Offset: 0x000290AC
		public override void Initialize(string name, NameValueCollection attributes)
		{
			if (attributes != null)
			{
				if (string.IsNullOrEmpty(attributes["description"]))
				{
					attributes.Remove("description");
					attributes.Add("description", base.GetType().Name);
				}
				ProviderUtil.GetAndRemoveBooleanAttribute(attributes, "securityTrimmingEnabled", this.Name, ref this._securityTrimmingEnabled);
			}
			base.Initialize(name, attributes);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002A110 File Offset: 0x00029110
		public virtual bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (!this.SecurityTrimmingEnabled)
			{
				return true;
			}
			if (node.Roles != null)
			{
				foreach (object obj in node.Roles)
				{
					string text = (string)obj;
					if (text == "*" || (context.User != null && context.User.IsInRole(text)))
					{
						return true;
					}
				}
			}
			VirtualPath virtualPath = node.VirtualPath;
			return !(virtualPath == null) && virtualPath.IsWithinAppRoot && Util.IsUserAllowedToPath(context, virtualPath);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002A1E0 File Offset: 0x000291E0
		protected internal virtual void RemoveNode(SiteMapNode node)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002A1E8 File Offset: 0x000291E8
		protected SiteMapNode ResolveSiteMapNode(HttpContext context)
		{
			SiteMapResolveEventHandler siteMapResolve = this.SiteMapResolve;
			if (siteMapResolve == null)
			{
				return null;
			}
			if (!context.Items.Contains(this._resolutionTicket))
			{
				context.Items.Add(this._resolutionTicket, true);
				try
				{
					Delegate[] invocationList = siteMapResolve.GetInvocationList();
					int num = invocationList.Length;
					for (int i = 0; i < num; i++)
					{
						SiteMapNode siteMapNode = ((SiteMapResolveEventHandler)invocationList[i])(this, new SiteMapResolveEventArgs(context, this));
						if (siteMapNode != null)
						{
							return siteMapNode;
						}
					}
				}
				finally
				{
					context.Items.Remove(this._resolutionTicket);
				}
			}
			return null;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002A28C File Offset: 0x0002928C
		internal SiteMapNode ReturnNodeIfAccessible(SiteMapNode node)
		{
			if (node != null && node.IsAccessibleToUser(HttpContext.Current))
			{
				return node;
			}
			return null;
		}

		// Token: 0x04001248 RID: 4680
		internal const string _securityTrimmingEnabledAttrName = "securityTrimmingEnabled";

		// Token: 0x04001249 RID: 4681
		private const string _allRoles = "*";

		// Token: 0x0400124A RID: 4682
		private bool _securityTrimmingEnabled;

		// Token: 0x0400124B RID: 4683
		private bool _enableLocalization;

		// Token: 0x0400124C RID: 4684
		private string _resourceKey;

		// Token: 0x0400124D RID: 4685
		private SiteMapProvider _rootProvider;

		// Token: 0x0400124E RID: 4686
		private SiteMapProvider _parentProvider;

		// Token: 0x0400124F RID: 4687
		private object _resolutionTicket = new object();

		// Token: 0x04001250 RID: 4688
		internal readonly object _lock = new object();
	}
}
