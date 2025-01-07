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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class DesignTimeSiteMapProvider : DesignTimeSiteMapProviderBase
	{
		internal DesignTimeSiteMapProvider(IDesignerHost host)
			: base(host)
		{
		}

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

		private string GetAttributeFromXmlNode(XmlNode xmlNode, string attributeName)
		{
			XmlNode namedItem = xmlNode.Attributes.GetNamedItem(attributeName);
			if (namedItem != null)
			{
				return namedItem.Value;
			}
			return null;
		}

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

		private SiteMapNode GetCurrentNodeFromLiveData(out SiteMapNode rootNode)
		{
			rootNode = this.BuildSiteMap();
			if (rootNode != null && base.DocumentAppRelativeUrl != null)
			{
				return (SiteMapNode)this.UrlTable[base.DocumentAppRelativeUrl];
			}
			return null;
		}

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

		private static bool IsAppRelativePath(string path)
		{
			return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
		}

		public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			return true;
		}

		private const string _providerAttribute = "provider";

		private const string _siteMapFileAttribute = "siteMapFile";

		private const string _siteMapNodeName = "siteMapNode";

		private const string _resourcePrefix = "$resources:";

		private const char _appRelativeCharacter = '~';

		private const int _resourcePrefixLength = 10;

		private static readonly char[] _seperators = new char[] { ';', ',' };

		private SiteMapNode _rootNode;

		private Hashtable _urlTable;
	}
}
