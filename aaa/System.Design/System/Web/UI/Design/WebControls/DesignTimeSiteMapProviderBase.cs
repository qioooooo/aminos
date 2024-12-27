using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000445 RID: 1093
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DesignTimeSiteMapProviderBase : StaticSiteMapProvider
	{
		// Token: 0x0600278D RID: 10125 RVA: 0x000D868D File Offset: 0x000D768D
		internal DesignTimeSiteMapProviderBase(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			this._host = host;
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x000D86AA File Offset: 0x000D76AA
		public override SiteMapNode CurrentNode
		{
			get
			{
				this.BuildDesignTimeSiteMapInternal();
				return this._currentNode;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000D86BC File Offset: 0x000D76BC
		internal string DocumentAppRelativeUrl
		{
			get
			{
				if (this._host != null)
				{
					IComponent rootComponent = this._host.RootComponent;
					if (rootComponent != null)
					{
						WebFormsRootDesigner webFormsRootDesigner = this._host.GetDesigner(rootComponent) as WebFormsRootDesigner;
						if (webFormsRootDesigner != null)
						{
							return webFormsRootDesigner.DocumentUrl;
						}
					}
				}
				return string.Empty;
			}
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000D8701 File Offset: 0x000D7701
		protected override SiteMapNode GetRootNodeCore()
		{
			this.BuildDesignTimeSiteMapInternal();
			return this._rootNode;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000D8710 File Offset: 0x000D7710
		private SiteMapNode BuildDesignTimeSiteMapInternal()
		{
			if (this._rootNode != null)
			{
				return this._rootNode;
			}
			this._rootNode = new SiteMapNode(this, DesignTimeSiteMapProviderBase._rootNodeText + " url", DesignTimeSiteMapProviderBase._rootNodeText + " url", DesignTimeSiteMapProviderBase._rootNodeText, DesignTimeSiteMapProviderBase._rootNodeText);
			this._currentNode = new SiteMapNode(this, DesignTimeSiteMapProviderBase._currentNodeText + " url", DesignTimeSiteMapProviderBase._currentNodeText + " url", DesignTimeSiteMapProviderBase._currentNodeText, DesignTimeSiteMapProviderBase._currentNodeText);
			SiteMapNode siteMapNode = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._parentNodeText);
			SiteMapNode siteMapNode2 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._siblingNodeText1);
			SiteMapNode siteMapNode3 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._siblingNodeText2);
			SiteMapNode siteMapNode4 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._siblingNodeText3);
			SiteMapNode siteMapNode5 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._childNodeText1);
			SiteMapNode siteMapNode6 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._childNodeText2);
			SiteMapNode siteMapNode7 = this.CreateNewSiteMapNode(DesignTimeSiteMapProviderBase._childNodeText3);
			this.AddNode(this._rootNode);
			this.AddNode(siteMapNode, this._rootNode);
			this.AddNode(siteMapNode2, siteMapNode);
			this.AddNode(this._currentNode, siteMapNode);
			this.AddNode(siteMapNode3, siteMapNode);
			this.AddNode(siteMapNode4, siteMapNode);
			this.AddNode(siteMapNode5, this._currentNode);
			this.AddNode(siteMapNode6, this._currentNode);
			this.AddNode(siteMapNode7, this._currentNode);
			return this._rootNode;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000D8859 File Offset: 0x000D7859
		public override SiteMapNode BuildSiteMap()
		{
			return this.BuildDesignTimeSiteMapInternal();
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000D8864 File Offset: 0x000D7864
		private SiteMapNode CreateNewSiteMapNode(string text)
		{
			string text2 = text + "url";
			return new SiteMapNode(this, text2, text2, text, text);
		}

		// Token: 0x04001B4A RID: 6986
		private SiteMapNode _rootNode;

		// Token: 0x04001B4B RID: 6987
		private SiteMapNode _currentNode;

		// Token: 0x04001B4C RID: 6988
		private static readonly string _rootNodeText = SR.GetString("DesignTimeSiteMapProvider_RootNodeText");

		// Token: 0x04001B4D RID: 6989
		private static readonly string _parentNodeText = SR.GetString("DesignTimeSiteMapProvider_ParentNodeText");

		// Token: 0x04001B4E RID: 6990
		private static readonly string _siblingNodeText = SR.GetString("DesignTimeSiteMapProvider_SiblingNodeText");

		// Token: 0x04001B4F RID: 6991
		private static readonly string _currentNodeText = SR.GetString("DesignTimeSiteMapProvider_CurrentNodeText");

		// Token: 0x04001B50 RID: 6992
		private static readonly string _childNodeText = SR.GetString("DesignTimeSiteMapProvider_ChildNodeText");

		// Token: 0x04001B51 RID: 6993
		private static readonly string _siblingNodeText1 = DesignTimeSiteMapProviderBase._siblingNodeText + " 1";

		// Token: 0x04001B52 RID: 6994
		private static readonly string _siblingNodeText2 = DesignTimeSiteMapProviderBase._siblingNodeText + " 2";

		// Token: 0x04001B53 RID: 6995
		private static readonly string _siblingNodeText3 = DesignTimeSiteMapProviderBase._siblingNodeText + " 3";

		// Token: 0x04001B54 RID: 6996
		private static readonly string _childNodeText1 = DesignTimeSiteMapProviderBase._childNodeText + " 1";

		// Token: 0x04001B55 RID: 6997
		private static readonly string _childNodeText2 = DesignTimeSiteMapProviderBase._childNodeText + " 2";

		// Token: 0x04001B56 RID: 6998
		private static readonly string _childNodeText3 = DesignTimeSiteMapProviderBase._childNodeText + " 3";

		// Token: 0x04001B57 RID: 6999
		protected IDesignerHost _host;
	}
}
