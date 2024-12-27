using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000446 RID: 1094
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class DesignTimeSiteMapProvider : DesignTimeSiteMapProviderBase
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x000D8958 File Offset: 0x000D7958
		internal DesignTimeSiteMapProvider(IDesignerHost host)
			: base(host)
		{
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002796 RID: 10134 RVA: 0x000D8964 File Offset: 0x000D7964
		public override SiteMapNode CurrentNode
		{
			get
			{
				SiteMapNode siteMapNode;
				SiteMapNode currentNodeFromLiveData = this.GetCurrentNodeFromLiveData(out siteMapNode);
				if (currentNodeFromLiveData != null)
				{
					return currentNodeFromLiveData;
				}
				return base.CurrentNode;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x000D8988 File Offset: 0x000D7988
		public override SiteMapNode RootNode
		{
			get
			{
				SiteMapNode siteMapNode;
				this.GetCurrentNodeFromLiveData(out siteMapNode);
				if (siteMapNode != null)
				{
					return siteMapNode;
				}
				return base.RootNode;
			}
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000D89AC File Offset: 0x000D79AC
		private Stream GetSiteMapFileStream(out string physicalPath)
		{
			physicalPath = string.Empty;
			if (this._host != null)
			{
				IWebApplication webApplication = (IWebApplication)this._host.GetService(typeof(IWebApplication));
				if (webApplication != null)
				{
					IProjectItem projectItemFromUrl = webApplication.GetProjectItemFromUrl("~/web.sitemap");
					if (projectItemFromUrl != null)
					{
						physicalPath = projectItemFromUrl.PhysicalPath;
						IDocumentProjectItem documentProjectItem = projectItemFromUrl as IDocumentProjectItem;
						if (documentProjectItem != null)
						{
							return documentProjectItem.GetContents();
						}
					}
				}
			}
			return null;
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x000D8A10 File Offset: 0x000D7A10
		internal IDictionary UrlTable
		{
			get
			{
				if (this._urlTable == null)
				{
					lock (this)
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

		// Token: 0x0600279A RID: 10138 RVA: 0x000D8A64 File Offset: 0x000D7A64
		public override SiteMapNode BuildSiteMap()
		{
			if (this._rootNode != null)
			{
				return this._rootNode;
			}
			string text = null;
			Stream siteMapFileStream = this.GetSiteMapFileStream(out text);
			XmlDocument xmlDocument = new XmlDocument();
			if (siteMapFileStream == null)
			{
				if (text.Length == 0)
				{
					this._rootNode = base.BuildSiteMap();
					return this._rootNode;
				}
				xmlDocument.Load(text);
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(siteMapFileStream))
				{
					xmlDocument.LoadXml(streamReader.ReadToEnd());
				}
			}
			XmlNode xmlNode = null;
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (string.Equals(xmlNode2.Name, "siteMap", StringComparison.Ordinal))
				{
					xmlNode = xmlNode2;
					break;
				}
			}
			if (xmlNode == null)
			{
				this._rootNode = base.BuildSiteMap();
				return this._rootNode;
			}
			try
			{
				this._rootNode = this.ConvertFromXmlNode(xmlNode.FirstChild);
			}
			catch (Exception)
			{
				this.Clear();
				this._rootNode = base.BuildSiteMap();
			}
			return this._rootNode;
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000D8BA0 File Offset: 0x000D7BA0
		private string GetAttributeFromXmlNode(XmlNode xmlNode, string attributeName)
		{
			XmlNode namedItem = xmlNode.Attributes.GetNamedItem(attributeName);
			if (namedItem != null)
			{
				return namedItem.Value;
			}
			return null;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000D8BC8 File Offset: 0x000D7BC8
		private SiteMapNode ConvertFromXmlNode(XmlNode xmlNode)
		{
			if (xmlNode.Attributes.GetNamedItem("provider") != null || xmlNode.Attributes.GetNamedItem("siteMapFile") != null)
			{
				return null;
			}
			string text = null;
			string text2 = this.GetAttributeFromXmlNode(xmlNode, "title");
			string text3 = this.GetAttributeFromXmlNode(xmlNode, "description");
			text = this.GetAttributeFromXmlNode(xmlNode, "url");
			string attributeFromXmlNode = this.GetAttributeFromXmlNode(xmlNode, "roles");
			text2 = this.HandleResourceAttribute(text2);
			text3 = this.HandleResourceAttribute(text3);
			ArrayList arrayList = new ArrayList();
			if (attributeFromXmlNode != null)
			{
				foreach (string text4 in attributeFromXmlNode.Split(DesignTimeSiteMapProvider._seperators))
				{
					string text5 = text4.Trim();
					if (text5.Length > 0)
					{
						arrayList.Add(text5);
					}
				}
			}
			arrayList = ArrayList.ReadOnly(arrayList);
			if (text == null)
			{
				text = string.Empty;
			}
			if (text.Length != 0 && !DesignTimeSiteMapProvider.IsAppRelativePath(text))
			{
				text = "~/" + text;
			}
			string text6 = text;
			if (text6.Length == 0)
			{
				text6 = Guid.NewGuid().ToString();
			}
			SiteMapNode siteMapNode = new SiteMapNode(this, text6, text, text2, text3, arrayList, null, null, null);
			SiteMapNodeCollection siteMapNodeCollection = new SiteMapNodeCollection();
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType == XmlNodeType.Element)
				{
					SiteMapNode siteMapNode2 = this.ConvertFromXmlNode(xmlNode2);
					if (siteMapNode2 != null)
					{
						siteMapNodeCollection.Add(siteMapNode2);
						this.AddNode(siteMapNode2, siteMapNode);
					}
				}
			}
			if (text.Length != 0)
			{
				if (this.UrlTable.Contains(text))
				{
					throw new InvalidOperationException(SR.GetString("DesignTimeSiteMapProvider_Duplicate_Url", new object[] { text }));
				}
				this.UrlTable[text] = siteMapNode;
			}
			return siteMapNode;
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000D8DBC File Offset: 0x000D7DBC
		private SiteMapNode GetCurrentNodeFromLiveData(out SiteMapNode rootNode)
		{
			rootNode = this.BuildSiteMap();
			if (rootNode != null && base.DocumentAppRelativeUrl != null)
			{
				return (SiteMapNode)this.UrlTable[base.DocumentAppRelativeUrl];
			}
			return null;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000D8DEC File Offset: 0x000D7DEC
		private string HandleResourceAttribute(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = text.TrimStart(new char[] { ' ' });
				if (text2.Length > 10 && text2.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
				{
					int num = text2.IndexOf(',');
					if (num != -1)
					{
						num = text2.IndexOf(',', num + 1);
						if (num != -1)
						{
							return text2.Substring(num + 1);
						}
					}
					return string.Empty;
				}
			}
			return text;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000D8E65 File Offset: 0x000D7E65
		private static bool IsAppRelativePath(string path)
		{
			return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000D8E95 File Offset: 0x000D7E95
		public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			return true;
		}

		// Token: 0x04001B58 RID: 7000
		private const string _providerAttribute = "provider";

		// Token: 0x04001B59 RID: 7001
		private const string _siteMapFileAttribute = "siteMapFile";

		// Token: 0x04001B5A RID: 7002
		private const string _siteMapNodeName = "siteMapNode";

		// Token: 0x04001B5B RID: 7003
		private const string _resourcePrefix = "$resources:";

		// Token: 0x04001B5C RID: 7004
		private const char _appRelativeCharacter = '~';

		// Token: 0x04001B5D RID: 7005
		private const int _resourcePrefixLength = 10;

		// Token: 0x04001B5E RID: 7006
		private static readonly char[] _seperators = new char[] { ';', ',' };

		// Token: 0x04001B5F RID: 7007
		private SiteMapNode _rootNode;

		// Token: 0x04001B60 RID: 7008
		private Hashtable _urlTable;
	}
}
