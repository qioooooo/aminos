using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Resources;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000CE RID: 206
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SiteMapNode : ICloneable, IHierarchyData, INavigateUIData
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x00028E94 File Offset: 0x00027E94
		public SiteMapNode(SiteMapProvider provider, string key)
			: this(provider, key, null, null, null, null, null, null, null)
		{
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00028EB0 File Offset: 0x00027EB0
		public SiteMapNode(SiteMapProvider provider, string key, string url)
			: this(provider, key, url, null, null, null, null, null, null)
		{
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00028ECC File Offset: 0x00027ECC
		public SiteMapNode(SiteMapProvider provider, string key, string url, string title)
			: this(provider, key, url, title, null, null, null, null, null)
		{
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00028EEC File Offset: 0x00027EEC
		public SiteMapNode(SiteMapProvider provider, string key, string url, string title, string description)
			: this(provider, key, url, title, description, null, null, null, null)
		{
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00028F0C File Offset: 0x00027F0C
		public SiteMapNode(SiteMapProvider provider, string key, string url, string title, string description, IList roles, NameValueCollection attributes, NameValueCollection explicitResourceKeys, string implicitResourceKey)
		{
			this._provider = provider;
			this._title = title;
			this._description = description;
			this._roles = roles;
			this._attributes = attributes;
			this._key = key;
			this._resourceKeys = explicitResourceKeys;
			this._resourceKey = implicitResourceKey;
			if (url != null)
			{
				this._url = url.Trim();
			}
			this._virtualPath = this.CreateVirtualPathFromUrl(this._url);
			if (this._key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this._provider == null)
			{
				throw new ArgumentNullException("provider");
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00028FA4 File Offset: 0x00027FA4
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00028FAC File Offset: 0x00027FAC
		protected NameValueCollection Attributes
		{
			get
			{
				return this._attributes;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Attributes" }));
				}
				this._attributes = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		public virtual string this[string key]
		{
			get
			{
				string text = null;
				if (this._attributes != null)
				{
					text = this._attributes[key];
				}
				if (this._provider.EnableLocalization)
				{
					string text2 = this.GetImplicitResourceString(key);
					if (text2 != null)
					{
						return text2;
					}
					text2 = this.GetExplicitResourceString(key, text, true);
					if (text2 != null)
					{
						return text2;
					}
				}
				return text;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Item" }));
				}
				if (this._attributes == null)
				{
					this._attributes = new NameValueCollection();
				}
				this._attributes[key] = value;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0002908D File Offset: 0x0002808D
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x000290AC File Offset: 0x000280AC
		public virtual SiteMapNodeCollection ChildNodes
		{
			get
			{
				if (this._childNodesSet)
				{
					return this._childNodes;
				}
				return this._provider.GetChildNodes(this);
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "ChildNodes" }));
				}
				this._childNodes = value;
				this._childNodesSet = true;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x000290F0 File Offset: 0x000280F0
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x00029148 File Offset: 0x00028148
		[Localizable(true)]
		public virtual string Description
		{
			get
			{
				if (this._provider.EnableLocalization)
				{
					string text = this.GetImplicitResourceString("description");
					if (text != null)
					{
						return text;
					}
					text = this.GetExplicitResourceString("description", this._description, true);
					if (text != null)
					{
						return text;
					}
				}
				if (this._description != null)
				{
					return this._description;
				}
				return string.Empty;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Description" }));
				}
				this._description = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x00029184 File Offset: 0x00028184
		public string Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0002918C File Offset: 0x0002818C
		public virtual bool HasChildNodes
		{
			get
			{
				IList childNodes = this.ChildNodes;
				return childNodes != null && childNodes.Count > 0;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000291B0 File Offset: 0x000281B0
		public virtual SiteMapNode NextSibling
		{
			get
			{
				IList siblingNodes = this.SiblingNodes;
				if (siblingNodes == null)
				{
					return null;
				}
				int num = siblingNodes.IndexOf(this);
				if (num >= 0 && num < siblingNodes.Count - 1)
				{
					return (SiteMapNode)siblingNodes[num + 1];
				}
				return null;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000291F0 File Offset: 0x000281F0
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00029210 File Offset: 0x00028210
		public virtual SiteMapNode ParentNode
		{
			get
			{
				if (this._parentNodeSet)
				{
					return this._parentNode;
				}
				return this._provider.GetParentNode(this);
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "ParentNode" }));
				}
				this._parentNode = value;
				this._parentNodeSet = true;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00029254 File Offset: 0x00028254
		public virtual SiteMapNode PreviousSibling
		{
			get
			{
				IList siblingNodes = this.SiblingNodes;
				if (siblingNodes == null)
				{
					return null;
				}
				int num = siblingNodes.IndexOf(this);
				if (num > 0 && num <= siblingNodes.Count - 1)
				{
					return (SiteMapNode)siblingNodes[num - 1];
				}
				return null;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00029294 File Offset: 0x00028294
		public SiteMapProvider Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0002929C File Offset: 0x0002829C
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x000292A4 File Offset: 0x000282A4
		public bool ReadOnly
		{
			get
			{
				return this._readonly;
			}
			set
			{
				this._readonly = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x000292AD File Offset: 0x000282AD
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x000292B8 File Offset: 0x000282B8
		public string ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "ResourceKey" }));
				}
				this._resourceKey = value;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x000292F4 File Offset: 0x000282F4
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x000292FC File Offset: 0x000282FC
		public IList Roles
		{
			get
			{
				return this._roles;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Roles" }));
				}
				this._roles = value;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00029338 File Offset: 0x00028338
		public virtual SiteMapNode RootNode
		{
			get
			{
				SiteMapNode rootNode = this._provider.RootProvider.RootNode;
				if (rootNode == null)
				{
					string name = this._provider.RootProvider.Name;
					throw new InvalidOperationException(SR.GetString("SiteMapProvider_Invalid_RootNode", new object[] { name }));
				}
				return rootNode;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00029388 File Offset: 0x00028388
		private SiteMapNodeCollection SiblingNodes
		{
			get
			{
				SiteMapNode parentNode = this.ParentNode;
				if (parentNode != null)
				{
					return parentNode.ChildNodes;
				}
				return null;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x000293A8 File Offset: 0x000283A8
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00029400 File Offset: 0x00028400
		[Localizable(true)]
		public virtual string Title
		{
			get
			{
				if (this._provider.EnableLocalization)
				{
					string text = this.GetImplicitResourceString("title");
					if (text != null)
					{
						return text;
					}
					text = this.GetExplicitResourceString("title", this._title, true);
					if (text != null)
					{
						return text;
					}
				}
				if (this._title != null)
				{
					return this._title;
				}
				return string.Empty;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Title" }));
				}
				this._title = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0002943C File Offset: 0x0002843C
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00029454 File Offset: 0x00028454
		public virtual string Url
		{
			get
			{
				if (this._url != null)
				{
					return this._url;
				}
				return string.Empty;
			}
			set
			{
				if (this._readonly)
				{
					throw new InvalidOperationException(SR.GetString("SiteMapNode_readonly", new object[] { "Url" }));
				}
				if (value != null)
				{
					this._url = value.Trim();
				}
				this._virtualPath = this.CreateVirtualPathFromUrl(this._url);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x000294AA File Offset: 0x000284AA
		internal VirtualPath VirtualPath
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x000294B4 File Offset: 0x000284B4
		private VirtualPath CreateVirtualPathFromUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return null;
			}
			if (!UrlPath.IsValidVirtualPathWithoutProtocol(url))
			{
				return null;
			}
			if (UrlPath.IsAbsolutePhysicalPath(url))
			{
				return null;
			}
			if (HttpRuntime.AppDomainAppVirtualPath == null)
			{
				return null;
			}
			if (UrlPath.IsRelativeUrl(url) && !UrlPath.IsAppRelativePath(url))
			{
				url = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, url);
			}
			int num = url.IndexOf('?');
			if (num != -1)
			{
				url = url.Substring(0, num);
			}
			return VirtualPath.Create(url, VirtualPathOptions.AllowAbsolutePath | VirtualPathOptions.AllowAppRelativePath);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00029524 File Offset: 0x00028524
		public virtual SiteMapNode Clone()
		{
			ArrayList arrayList = null;
			NameValueCollection nameValueCollection = null;
			NameValueCollection nameValueCollection2 = null;
			if (this._roles != null)
			{
				arrayList = new ArrayList(this._roles);
			}
			if (this._attributes != null)
			{
				nameValueCollection = new NameValueCollection(this._attributes);
			}
			if (this._resourceKeys != null)
			{
				nameValueCollection2 = new NameValueCollection(this._resourceKeys);
			}
			return new SiteMapNode(this._provider, this.Key, this.Url, this.Title, this.Description, arrayList, nameValueCollection, nameValueCollection2, this._resourceKey);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000295A4 File Offset: 0x000285A4
		public virtual SiteMapNode Clone(bool cloneParentNodes)
		{
			SiteMapNode siteMapNode = this.Clone();
			if (cloneParentNodes)
			{
				SiteMapNode siteMapNode2 = siteMapNode;
				SiteMapNode siteMapNode3 = this.ParentNode;
				while (siteMapNode3 != null)
				{
					SiteMapNode siteMapNode4 = siteMapNode3.Clone();
					siteMapNode2.ParentNode = siteMapNode4;
					siteMapNode4.ChildNodes = new SiteMapNodeCollection(siteMapNode2);
					siteMapNode3 = siteMapNode3.ParentNode;
					siteMapNode2 = siteMapNode4;
				}
			}
			return siteMapNode;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000295F0 File Offset: 0x000285F0
		public override bool Equals(object obj)
		{
			SiteMapNode siteMapNode = obj as SiteMapNode;
			return siteMapNode != null && this._key == siteMapNode.Key && string.Equals(this._url, siteMapNode._url, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00029630 File Offset: 0x00028630
		public SiteMapNodeCollection GetAllNodes()
		{
			SiteMapNodeCollection siteMapNodeCollection = new SiteMapNodeCollection();
			this.GetAllNodesRecursive(siteMapNodeCollection);
			return SiteMapNodeCollection.ReadOnly(siteMapNodeCollection);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00029650 File Offset: 0x00028650
		private void GetAllNodesRecursive(SiteMapNodeCollection collection)
		{
			SiteMapNodeCollection childNodes = this.ChildNodes;
			if (childNodes != null && childNodes.Count > 0)
			{
				collection.AddRange(childNodes);
				foreach (object obj in childNodes)
				{
					SiteMapNode siteMapNode = (SiteMapNode)obj;
					siteMapNode.GetAllNodesRecursive(collection);
				}
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000296C0 File Offset: 0x000286C0
		public SiteMapDataSourceView GetDataSourceView(SiteMapDataSource owner, string viewName)
		{
			return new SiteMapDataSourceView(owner, viewName, this);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x000296CA File Offset: 0x000286CA
		public SiteMapHierarchicalDataSourceView GetHierarchicalDataSourceView()
		{
			return new SiteMapHierarchicalDataSourceView(this);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000296D4 File Offset: 0x000286D4
		protected string GetExplicitResourceString(string attributeName, string defaultValue, bool throwIfNotFound)
		{
			if (attributeName == null)
			{
				throw new ArgumentNullException("attributeName");
			}
			string text = null;
			if (this._resourceKeys != null)
			{
				string[] values = this._resourceKeys.GetValues(attributeName);
				if (values != null && values.Length > 1)
				{
					try
					{
						text = ResourceExpressionBuilder.GetGlobalResourceObject(values[0], values[1]) as string;
					}
					catch (MissingManifestResourceException)
					{
						if (defaultValue != null)
						{
							return defaultValue;
						}
					}
					if (text == null && throwIfNotFound)
					{
						throw new InvalidOperationException(SR.GetString("Res_not_found_with_class_and_key", new object[]
						{
							values[0],
							values[1]
						}));
					}
					return text;
				}
			}
			return text;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002976C File Offset: 0x0002876C
		public override int GetHashCode()
		{
			return this._key.GetHashCode();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002977C File Offset: 0x0002877C
		protected string GetImplicitResourceString(string attributeName)
		{
			if (attributeName == null)
			{
				throw new ArgumentNullException("attributeName");
			}
			string text = null;
			if (!string.IsNullOrEmpty(this._resourceKey))
			{
				try
				{
					text = ResourceExpressionBuilder.GetGlobalResourceObject(this.Provider.ResourceKey, this.ResourceKey + "." + attributeName) as string;
				}
				catch
				{
				}
			}
			return text;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000297E4 File Offset: 0x000287E4
		public virtual bool IsAccessibleToUser(HttpContext context)
		{
			return this._provider.IsAccessibleToUser(context, this);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000297F4 File Offset: 0x000287F4
		public virtual bool IsDescendantOf(SiteMapNode node)
		{
			for (SiteMapNode siteMapNode = this.ParentNode; siteMapNode != null; siteMapNode = siteMapNode.ParentNode)
			{
				if (siteMapNode.Equals(node))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00029820 File Offset: 0x00028820
		public override string ToString()
		{
			return this.Title;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00029828 File Offset: 0x00028828
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00029830 File Offset: 0x00028830
		bool IHierarchyData.HasChildren
		{
			get
			{
				return this.HasChildNodes;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x00029838 File Offset: 0x00028838
		object IHierarchyData.Item
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0002983B File Offset: 0x0002883B
		string IHierarchyData.Path
		{
			get
			{
				return this.Key;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00029843 File Offset: 0x00028843
		string IHierarchyData.Type
		{
			get
			{
				return SiteMapNode._siteMapNodeType;
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002984A File Offset: 0x0002884A
		IHierarchicalEnumerable IHierarchyData.GetChildren()
		{
			return this.ChildNodes;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00029854 File Offset: 0x00028854
		IHierarchyData IHierarchyData.GetParent()
		{
			SiteMapNode parentNode = this.ParentNode;
			if (parentNode == null)
			{
				return null;
			}
			return parentNode;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0002986E File Offset: 0x0002886E
		string INavigateUIData.Description
		{
			get
			{
				return this.Description;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00029876 File Offset: 0x00028876
		string INavigateUIData.Name
		{
			get
			{
				return this.Title;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0002987E File Offset: 0x0002887E
		string INavigateUIData.NavigateUrl
		{
			get
			{
				return this.Url;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00029886 File Offset: 0x00028886
		string INavigateUIData.Value
		{
			get
			{
				return this.Title;
			}
		}

		// Token: 0x04001234 RID: 4660
		private static readonly string _siteMapNodeType = typeof(SiteMapNode).Name;

		// Token: 0x04001235 RID: 4661
		private SiteMapProvider _provider;

		// Token: 0x04001236 RID: 4662
		private bool _readonly;

		// Token: 0x04001237 RID: 4663
		private bool _parentNodeSet;

		// Token: 0x04001238 RID: 4664
		private bool _childNodesSet;

		// Token: 0x04001239 RID: 4665
		private VirtualPath _virtualPath;

		// Token: 0x0400123A RID: 4666
		private string _title;

		// Token: 0x0400123B RID: 4667
		private string _description;

		// Token: 0x0400123C RID: 4668
		private string _url;

		// Token: 0x0400123D RID: 4669
		private string _key;

		// Token: 0x0400123E RID: 4670
		private string _resourceKey;

		// Token: 0x0400123F RID: 4671
		private IList _roles;

		// Token: 0x04001240 RID: 4672
		private NameValueCollection _attributes;

		// Token: 0x04001241 RID: 4673
		private NameValueCollection _resourceKeys;

		// Token: 0x04001242 RID: 4674
		private SiteMapNode _parentNode;

		// Token: 0x04001243 RID: 4675
		private SiteMapNodeCollection _childNodes;
	}
}
