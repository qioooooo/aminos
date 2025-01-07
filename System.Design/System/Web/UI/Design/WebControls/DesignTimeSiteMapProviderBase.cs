using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class DesignTimeSiteMapProviderBase : StaticSiteMapProvider
	{
		internal DesignTimeSiteMapProviderBase(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			this._host = host;
		}

		public override SiteMapNode CurrentNode
		{
			get
			{
				this.BuildDesignTimeSiteMapInternal();
				return this._currentNode;
			}
		}

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

		protected override SiteMapNode GetRootNodeCore()
		{
			this.BuildDesignTimeSiteMapInternal();
			return this._rootNode;
		}

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

		public override SiteMapNode BuildSiteMap()
		{
			return this.BuildDesignTimeSiteMapInternal();
		}

		private SiteMapNode CreateNewSiteMapNode(string text)
		{
			string text2 = text + "url";
			return new SiteMapNode(this, text2, text2, text, text);
		}

		private SiteMapNode _rootNode;

		private SiteMapNode _currentNode;

		private static readonly string _rootNodeText = SR.GetString("DesignTimeSiteMapProvider_RootNodeText");

		private static readonly string _parentNodeText = SR.GetString("DesignTimeSiteMapProvider_ParentNodeText");

		private static readonly string _siblingNodeText = SR.GetString("DesignTimeSiteMapProvider_SiblingNodeText");

		private static readonly string _currentNodeText = SR.GetString("DesignTimeSiteMapProvider_CurrentNodeText");

		private static readonly string _childNodeText = SR.GetString("DesignTimeSiteMapProvider_ChildNodeText");

		private static readonly string _siblingNodeText1 = DesignTimeSiteMapProviderBase._siblingNodeText + " 1";

		private static readonly string _siblingNodeText2 = DesignTimeSiteMapProviderBase._siblingNodeText + " 2";

		private static readonly string _siblingNodeText3 = DesignTimeSiteMapProviderBase._siblingNodeText + " 3";

		private static readonly string _childNodeText1 = DesignTimeSiteMapProviderBase._childNodeText + " 1";

		private static readonly string _childNodeText2 = DesignTimeSiteMapProviderBase._childNodeText + " 2";

		private static readonly string _childNodeText3 = DesignTimeSiteMapProviderBase._childNodeText + " 3";

		protected IDesignerHost _host;
	}
}
