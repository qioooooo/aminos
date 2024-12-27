using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000D6 RID: 214
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class StaticSiteMapProvider : SiteMapProvider
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0002ACA4 File Offset: 0x00029CA4
		internal IDictionary ChildNodeCollectionTable
		{
			get
			{
				if (this._childNodeCollectionTable == null)
				{
					lock (this._lock)
					{
						if (this._childNodeCollectionTable == null)
						{
							this._childNodeCollectionTable = new Hashtable();
						}
					}
				}
				return this._childNodeCollectionTable;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002ACF8 File Offset: 0x00029CF8
		internal IDictionary KeyTable
		{
			get
			{
				if (this._keyTable == null)
				{
					lock (this._lock)
					{
						if (this._keyTable == null)
						{
							this._keyTable = new Hashtable();
						}
					}
				}
				return this._keyTable;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002AD4C File Offset: 0x00029D4C
		internal IDictionary ParentNodeTable
		{
			get
			{
				if (this._parentNodeTable == null)
				{
					lock (this._lock)
					{
						if (this._parentNodeTable == null)
						{
							this._parentNodeTable = new Hashtable();
						}
					}
				}
				return this._parentNodeTable;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002ADA0 File Offset: 0x00029DA0
		internal IDictionary UrlTable
		{
			get
			{
				if (this._urlTable == null)
				{
					lock (this._lock)
					{
						if (this._urlTable == null)
						{
							this._urlTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
						}
					}
				}
				return this._urlTable;
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002ADFC File Offset: 0x00029DFC
		protected internal override void AddNode(SiteMapNode node, SiteMapNode parentNode)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			lock (this._lock)
			{
				bool flag = false;
				string text = node.Url;
				if (!string.IsNullOrEmpty(text))
				{
					if (HttpRuntime.AppDomainAppVirtualPath != null)
					{
						if (!UrlPath.IsAbsolutePhysicalPath(text))
						{
							text = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, text);
							text = UrlPath.MakeVirtualPathAppAbsolute(text);
						}
						if (this.UrlTable[text] != null)
						{
							throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { text }));
						}
					}
					flag = true;
				}
				string key = node.Key;
				if (this.KeyTable.Contains(key))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }));
				}
				this.KeyTable[key] = node;
				if (flag)
				{
					this.UrlTable[text] = node;
				}
				if (parentNode != null)
				{
					this.ParentNodeTable[node] = parentNode;
					if (this.ChildNodeCollectionTable[parentNode] == null)
					{
						this.ChildNodeCollectionTable[parentNode] = new SiteMapNodeCollection();
					}
					((SiteMapNodeCollection)this.ChildNodeCollectionTable[parentNode]).Add(node);
				}
			}
		}

		// Token: 0x060009D5 RID: 2517
		public abstract SiteMapNode BuildSiteMap();

		// Token: 0x060009D6 RID: 2518 RVA: 0x0002AF38 File Offset: 0x00029F38
		protected virtual void Clear()
		{
			lock (this._lock)
			{
				if (this._childNodeCollectionTable != null)
				{
					this._childNodeCollectionTable.Clear();
				}
				if (this._urlTable != null)
				{
					this._urlTable.Clear();
				}
				if (this._parentNodeTable != null)
				{
					this._parentNodeTable.Clear();
				}
				if (this._keyTable != null)
				{
					this._keyTable.Clear();
				}
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002AFB8 File Offset: 0x00029FB8
		public override SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			SiteMapNode siteMapNode = base.FindSiteMapNodeFromKey(key);
			if (siteMapNode == null)
			{
				siteMapNode = (SiteMapNode)this.KeyTable[key];
			}
			return base.ReturnNodeIfAccessible(siteMapNode);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002AFEC File Offset: 0x00029FEC
		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			if (rawUrl == null)
			{
				throw new ArgumentNullException("rawUrl");
			}
			rawUrl = rawUrl.Trim();
			if (rawUrl.Length == 0)
			{
				return null;
			}
			if (UrlPath.IsAppRelativePath(rawUrl))
			{
				rawUrl = UrlPath.MakeVirtualPathAppAbsolute(rawUrl);
			}
			this.BuildSiteMap();
			return base.ReturnNodeIfAccessible((SiteMapNode)this.UrlTable[rawUrl]);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002B048 File Offset: 0x0002A048
		public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.BuildSiteMap();
			SiteMapNodeCollection siteMapNodeCollection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node];
			if (siteMapNodeCollection == null)
			{
				SiteMapNode siteMapNode = (SiteMapNode)this.KeyTable[node.Key];
				if (siteMapNode != null)
				{
					siteMapNodeCollection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[siteMapNode];
				}
			}
			if (siteMapNodeCollection == null)
			{
				return SiteMapNodeCollection.Empty;
			}
			if (!base.SecurityTrimmingEnabled)
			{
				return SiteMapNodeCollection.ReadOnly(siteMapNodeCollection);
			}
			HttpContext httpContext = HttpContext.Current;
			SiteMapNodeCollection siteMapNodeCollection2 = new SiteMapNodeCollection(siteMapNodeCollection.Count);
			foreach (object obj in siteMapNodeCollection)
			{
				SiteMapNode siteMapNode2 = (SiteMapNode)obj;
				if (siteMapNode2.IsAccessibleToUser(httpContext))
				{
					siteMapNodeCollection2.Add(siteMapNode2);
				}
			}
			return SiteMapNodeCollection.ReadOnly(siteMapNodeCollection2);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0002B138 File Offset: 0x0002A138
		public override SiteMapNode GetParentNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.BuildSiteMap();
			SiteMapNode siteMapNode = (SiteMapNode)this.ParentNodeTable[node];
			if (siteMapNode == null)
			{
				SiteMapNode siteMapNode2 = (SiteMapNode)this.KeyTable[node.Key];
				if (siteMapNode2 != null)
				{
					siteMapNode = (SiteMapNode)this.ParentNodeTable[siteMapNode2];
				}
			}
			if (siteMapNode == null && this.ParentProvider != null)
			{
				siteMapNode = this.ParentProvider.GetParentNode(node);
			}
			return base.ReturnNodeIfAccessible(siteMapNode);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002B1BC File Offset: 0x0002A1BC
		protected internal override void RemoveNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			lock (this._lock)
			{
				SiteMapNode siteMapNode = (SiteMapNode)this.ParentNodeTable[node];
				if (this.ParentNodeTable.Contains(node))
				{
					this.ParentNodeTable.Remove(node);
				}
				if (siteMapNode != null)
				{
					SiteMapNodeCollection siteMapNodeCollection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[siteMapNode];
					if (siteMapNodeCollection != null && siteMapNodeCollection.Contains(node))
					{
						siteMapNodeCollection.Remove(node);
					}
				}
				string url = node.Url;
				if (url != null && url.Length > 0 && this.UrlTable.Contains(url))
				{
					this.UrlTable.Remove(url);
				}
				string key = node.Key;
				if (this.KeyTable.Contains(key))
				{
					this.KeyTable.Remove(key);
				}
			}
		}

		// Token: 0x0400125E RID: 4702
		private Hashtable _childNodeCollectionTable;

		// Token: 0x0400125F RID: 4703
		private Hashtable _keyTable;

		// Token: 0x04001260 RID: 4704
		private Hashtable _parentNodeTable;

		// Token: 0x04001261 RID: 4705
		private Hashtable _urlTable;
	}
}
