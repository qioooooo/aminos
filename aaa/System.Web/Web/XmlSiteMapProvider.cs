using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

namespace System.Web
{
	// Token: 0x020000F1 RID: 241
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class XmlSiteMapProvider : StaticSiteMapProvider, IDisposable
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002CEF8 File Offset: 0x0002BEF8
		private ArrayList ChildProviderList
		{
			get
			{
				if (this._childProviderList == null)
				{
					lock (this._lock)
					{
						if (this._childProviderList == null)
						{
							this._childProviderList = ArrayList.ReadOnly(new ArrayList(this.ChildProviderTable.Keys));
						}
					}
				}
				return this._childProviderList;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0002CF5C File Offset: 0x0002BF5C
		private Hashtable ChildProviderTable
		{
			get
			{
				if (this._childProviderTable == null)
				{
					lock (this._lock)
					{
						if (this._childProviderTable == null)
						{
							this._childProviderTable = new Hashtable();
						}
					}
				}
				return this._childProviderTable;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0002CFB0 File Offset: 0x0002BFB0
		public override SiteMapNode RootNode
		{
			get
			{
				this.BuildSiteMap();
				return base.ReturnNodeIfAccessible(this._siteMapNode);
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002CFC8 File Offset: 0x0002BFC8
		protected internal override void AddNode(SiteMapNode node, SiteMapNode parentNode)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (parentNode == null)
			{
				throw new ArgumentNullException("parentNode");
			}
			SiteMapProvider provider = node.Provider;
			SiteMapProvider provider2 = parentNode.Provider;
			if (provider != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { node.ToString() }), "node");
			}
			if (provider2 != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { parentNode.ToString() }), "parentNode");
			}
			lock (this._lock)
			{
				this.RemoveNode(node);
				this.AddNodeInternal(node, parentNode, null);
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002D08C File Offset: 0x0002C08C
		private void AddNodeInternal(SiteMapNode node, SiteMapNode parentNode, XmlNode xmlNode)
		{
			lock (this._lock)
			{
				string url = node.Url;
				string key = node.Key;
				bool flag = false;
				if (!string.IsNullOrEmpty(url))
				{
					if (base.UrlTable[url] != null)
					{
						if (xmlNode != null)
						{
							throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { url }), xmlNode);
						}
						throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url", new object[] { url }));
					}
					else
					{
						flag = true;
					}
				}
				if (base.KeyTable.Contains(key))
				{
					if (xmlNode != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }), xmlNode);
					}
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key", new object[] { key }));
				}
				else
				{
					if (flag)
					{
						base.UrlTable[url] = node;
					}
					base.KeyTable[key] = node;
					if (parentNode != null)
					{
						base.ParentNodeTable[node] = parentNode;
						if (base.ChildNodeCollectionTable[parentNode] == null)
						{
							base.ChildNodeCollectionTable[parentNode] = new SiteMapNodeCollection();
						}
						((SiteMapNodeCollection)base.ChildNodeCollectionTable[parentNode]).Add(node);
					}
				}
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002D1EC File Offset: 0x0002C1EC
		protected virtual void AddProvider(string providerName, SiteMapNode parentNode)
		{
			if (parentNode == null)
			{
				throw new ArgumentNullException("parentNode");
			}
			if (parentNode.Provider != this)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_cannot_add_node", new object[] { parentNode.ToString() }), "parentNode");
			}
			SiteMapNode nodeFromProvider = this.GetNodeFromProvider(providerName);
			this.AddNodeInternal(nodeFromProvider, parentNode, null);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002D248 File Offset: 0x0002C248
		public override SiteMapNode BuildSiteMap()
		{
			SiteMapNode siteMapNode = this._siteMapNode;
			if (siteMapNode != null)
			{
				return siteMapNode;
			}
			XmlDocument configDocument = this.GetConfigDocument();
			SiteMapNode siteMapNode2;
			lock (this._lock)
			{
				if (this._siteMapNode != null)
				{
					siteMapNode2 = this._siteMapNode;
				}
				else
				{
					this.Clear();
					this.CheckSiteMapFileExists();
					try
					{
						using (Stream stream = this._normalizedVirtualPath.OpenFile())
						{
							XmlReader xmlReader = new XmlTextReader(stream);
							configDocument.Load(xmlReader);
						}
					}
					catch (XmlException ex)
					{
						string text = this._virtualPath.VirtualPathString;
						string text2 = this._normalizedVirtualPath.MapPathInternal();
						if (text2 != null && HttpRuntime.HasPathDiscoveryPermission(text2))
						{
							text = text2;
						}
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Error_loading_Config_file", new object[] { this._virtualPath, ex.Message }), ex, text, ex.LineNumber);
					}
					catch (Exception ex2)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Error_loading_Config_file", new object[] { this._virtualPath, ex2.Message }), ex2);
					}
					XmlNode xmlNode = null;
					foreach (object obj in configDocument.ChildNodes)
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
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Top_Element_Must_Be_SiteMap"), configDocument);
					}
					bool flag = false;
					HandlerBase.GetAndRemoveBooleanAttribute(xmlNode, "enableLocalization", ref flag);
					base.EnableLocalization = flag;
					XmlNode xmlNode3 = null;
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode4 = (XmlNode)obj2;
						if (xmlNode4.NodeType == XmlNodeType.Element)
						{
							if (!"siteMapNode".Equals(xmlNode4.Name))
							{
								throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_SiteMapNode_Allowed"), xmlNode4);
							}
							if (xmlNode3 != null)
							{
								throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top"), xmlNode4);
							}
							xmlNode3 = xmlNode4;
						}
					}
					if (xmlNode3 == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top"), xmlNode);
					}
					Queue queue = new Queue(50);
					queue.Enqueue(null);
					queue.Enqueue(xmlNode3);
					this._siteMapNode = this.ConvertFromXmlNode(queue);
					siteMapNode2 = this._siteMapNode;
				}
			}
			return siteMapNode2;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002D558 File Offset: 0x0002C558
		private void CheckSiteMapFileExists()
		{
			if (!Util.VirtualFileExistsWithAssert(this._normalizedVirtualPath))
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_does_not_exist", new object[] { this._virtualPath }));
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002D594 File Offset: 0x0002C594
		protected override void Clear()
		{
			lock (this._lock)
			{
				this.ChildProviderTable.Clear();
				this._siteMapNode = null;
				this._childProviderList = null;
				base.Clear();
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002D5E8 File Offset: 0x0002C5E8
		private SiteMapNode ConvertFromXmlNode(Queue queue)
		{
			SiteMapNode siteMapNode = null;
			while (queue.Count != 0)
			{
				SiteMapNode siteMapNode2 = (SiteMapNode)queue.Dequeue();
				XmlNode xmlNode = (XmlNode)queue.Dequeue();
				if (!"siteMapNode".Equals(xmlNode.Name))
				{
					throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_Only_SiteMapNode_Allowed"), xmlNode);
				}
				string text = null;
				HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "provider", ref text);
				SiteMapNode siteMapNode3;
				if (text != null)
				{
					siteMapNode3 = this.GetNodeFromProvider(text);
					HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
					HandlerBase.CheckForNonCommentChildNodes(xmlNode);
				}
				else
				{
					string text2 = null;
					HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "siteMapFile", ref text2);
					if (text2 != null)
					{
						siteMapNode3 = this.GetNodeFromSiteMapFile(xmlNode, VirtualPath.Create(text2));
					}
					else
					{
						siteMapNode3 = this.GetNodeFromXmlNode(xmlNode, queue);
					}
				}
				this.AddNodeInternal(siteMapNode3, siteMapNode2, xmlNode);
				if (siteMapNode == null)
				{
					siteMapNode = siteMapNode3;
				}
			}
			return siteMapNode;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002D6AE File Offset: 0x0002C6AE
		protected virtual void Dispose(bool disposing)
		{
			if (this._handler != null)
			{
				HttpRuntime.FileChangesMonitor.StopMonitoringFile(this._filename, this._handler);
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002D6CE File Offset: 0x0002C6CE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002D6E0 File Offset: 0x0002C6E0
		private void EnsureChildSiteMapProviderUpToDate(SiteMapProvider childProvider)
		{
			SiteMapNode siteMapNode = (SiteMapNode)this.ChildProviderTable[childProvider];
			SiteMapNode siteMapNode2 = childProvider.GetRootNodeCore();
			if (siteMapNode2 == null)
			{
				throw new ProviderException(SR.GetString("XmlSiteMapProvider_invalid_sitemapnode_returned", new object[] { childProvider.Name }));
			}
			if (!siteMapNode.Equals(siteMapNode2))
			{
				if (siteMapNode == null)
				{
					return;
				}
				lock (this._lock)
				{
					siteMapNode = (SiteMapNode)this.ChildProviderTable[childProvider];
					if (siteMapNode != null)
					{
						siteMapNode2 = childProvider.GetRootNodeCore();
						if (siteMapNode2 == null)
						{
							throw new ProviderException(SR.GetString("XmlSiteMapProvider_invalid_sitemapnode_returned", new object[] { childProvider.Name }));
						}
						if (!siteMapNode.Equals(siteMapNode2))
						{
							if (this._siteMapNode.Equals(siteMapNode))
							{
								base.UrlTable.Remove(siteMapNode.Url);
								base.KeyTable.Remove(siteMapNode.Key);
								base.UrlTable.Add(siteMapNode2.Url, siteMapNode2);
								base.KeyTable.Add(siteMapNode2.Key, siteMapNode2);
								this._siteMapNode = siteMapNode2;
							}
							SiteMapNode siteMapNode3 = (SiteMapNode)base.ParentNodeTable[siteMapNode];
							if (siteMapNode3 != null)
							{
								SiteMapNodeCollection siteMapNodeCollection = (SiteMapNodeCollection)base.ChildNodeCollectionTable[siteMapNode3];
								int num = siteMapNodeCollection.IndexOf(siteMapNode);
								if (num != -1)
								{
									siteMapNodeCollection.Remove(siteMapNode);
									siteMapNodeCollection.Insert(num, siteMapNode2);
								}
								else
								{
									siteMapNodeCollection.Add(siteMapNode2);
								}
								base.ParentNodeTable[siteMapNode2] = siteMapNode3;
								base.ParentNodeTable.Remove(siteMapNode);
								base.UrlTable.Remove(siteMapNode.Url);
								base.KeyTable.Remove(siteMapNode.Key);
								base.UrlTable.Add(siteMapNode2.Url, siteMapNode2);
								base.KeyTable.Add(siteMapNode2.Key, siteMapNode2);
							}
							else
							{
								XmlSiteMapProvider xmlSiteMapProvider = this.ParentProvider as XmlSiteMapProvider;
								if (xmlSiteMapProvider != null)
								{
									xmlSiteMapProvider.EnsureChildSiteMapProviderUpToDate(this);
								}
							}
							this.ChildProviderTable[childProvider] = siteMapNode2;
							this._childProviderList = null;
						}
					}
				}
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002D900 File Offset: 0x0002C900
		public override SiteMapNode FindSiteMapNode(string rawUrl)
		{
			SiteMapNode siteMapNode = base.FindSiteMapNode(rawUrl);
			if (siteMapNode == null)
			{
				foreach (object obj in this.ChildProviderList)
				{
					SiteMapProvider siteMapProvider = (SiteMapProvider)obj;
					this.EnsureChildSiteMapProviderUpToDate(siteMapProvider);
					siteMapNode = siteMapProvider.FindSiteMapNode(rawUrl);
					if (siteMapNode != null)
					{
						return siteMapNode;
					}
				}
				return siteMapNode;
			}
			return siteMapNode;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002D97C File Offset: 0x0002C97C
		public override SiteMapNode FindSiteMapNodeFromKey(string key)
		{
			SiteMapNode siteMapNode = base.FindSiteMapNodeFromKey(key);
			if (siteMapNode == null)
			{
				foreach (object obj in this.ChildProviderList)
				{
					SiteMapProvider siteMapProvider = (SiteMapProvider)obj;
					this.EnsureChildSiteMapProviderUpToDate(siteMapProvider);
					siteMapNode = siteMapProvider.FindSiteMapNodeFromKey(key);
					if (siteMapNode != null)
					{
						return siteMapNode;
					}
				}
				return siteMapNode;
			}
			return siteMapNode;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002D9F8 File Offset: 0x0002C9F8
		private XmlDocument GetConfigDocument()
		{
			if (this._document != null)
			{
				return this._document;
			}
			if (!this._initialized)
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Not_Initialized"));
			}
			if (this._virtualPath == null)
			{
				throw new ArgumentException(SR.GetString("XmlSiteMapProvider_missing_siteMapFile", new object[] { "siteMapFile" }));
			}
			if (!this._virtualPath.Extension.Equals(".sitemap", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Invalid_Extension", new object[] { this._virtualPath }));
			}
			this._normalizedVirtualPath = this._virtualPath.CombineWithAppRoot();
			this._normalizedVirtualPath.FailIfNotWithinAppRoot();
			this.CheckSiteMapFileExists();
			this._parentSiteMapFileCollection = new StringCollection();
			XmlSiteMapProvider xmlSiteMapProvider = this.ParentProvider as XmlSiteMapProvider;
			if (xmlSiteMapProvider != null && xmlSiteMapProvider._parentSiteMapFileCollection != null)
			{
				if (xmlSiteMapProvider._parentSiteMapFileCollection.Contains(this._normalizedVirtualPath.VirtualPathString))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_already_in_use", new object[] { this._virtualPath }));
				}
				foreach (string text in xmlSiteMapProvider._parentSiteMapFileCollection)
				{
					this._parentSiteMapFileCollection.Add(text);
				}
			}
			this._parentSiteMapFileCollection.Add(this._normalizedVirtualPath.VirtualPathString);
			this._filename = HostingEnvironment.MapPathInternal(this._normalizedVirtualPath);
			if (!string.IsNullOrEmpty(this._filename))
			{
				this._handler = new FileChangeEventHandler(this.OnConfigFileChange);
				HttpRuntime.FileChangesMonitor.StartMonitoringFile(this._filename, this._handler);
				base.ResourceKey = new FileInfo(this._filename).Name;
			}
			this._document = new ConfigXmlDocument();
			return this._document;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002DBF4 File Offset: 0x0002CBF4
		private SiteMapNode GetNodeFromProvider(string providerName)
		{
			SiteMapProvider providerFromName = this.GetProviderFromName(providerName);
			if (providerFromName is XmlSiteMapProvider)
			{
				XmlSiteMapProvider xmlSiteMapProvider = (XmlSiteMapProvider)providerFromName;
				StringCollection stringCollection = new StringCollection();
				if (this._parentSiteMapFileCollection != null)
				{
					foreach (string text in this._parentSiteMapFileCollection)
					{
						stringCollection.Add(text);
					}
				}
				xmlSiteMapProvider.BuildSiteMap();
				stringCollection.Add(this._normalizedVirtualPath.VirtualPathString);
				if (stringCollection.Contains(VirtualPath.GetVirtualPathString(xmlSiteMapProvider._normalizedVirtualPath)))
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_FileName_already_in_use", new object[] { xmlSiteMapProvider._virtualPath }));
				}
				xmlSiteMapProvider._parentSiteMapFileCollection = stringCollection;
			}
			SiteMapNode rootNodeCore = providerFromName.GetRootNodeCore();
			if (rootNodeCore == null)
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_invalid_GetRootNodeCore", new object[] { providerFromName.Name }));
			}
			this.ChildProviderTable.Add(providerFromName, rootNodeCore);
			this._childProviderList = null;
			providerFromName.ParentProvider = this;
			return rootNodeCore;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002DD20 File Offset: 0x0002CD20
		private SiteMapNode GetNodeFromSiteMapFile(XmlNode xmlNode, VirtualPath siteMapFile)
		{
			bool securityTrimmingEnabled = base.SecurityTrimmingEnabled;
			HandlerBase.GetAndRemoveBooleanAttribute(xmlNode, "securityTrimmingEnabled", ref securityTrimmingEnabled);
			HandlerBase.CheckForUnrecognizedAttributes(xmlNode);
			HandlerBase.CheckForNonCommentChildNodes(xmlNode);
			XmlSiteMapProvider xmlSiteMapProvider = new XmlSiteMapProvider();
			siteMapFile = this._normalizedVirtualPath.Parent.Combine(siteMapFile);
			xmlSiteMapProvider.ParentProvider = this;
			xmlSiteMapProvider.Initialize(siteMapFile, securityTrimmingEnabled);
			xmlSiteMapProvider.BuildSiteMap();
			SiteMapNode siteMapNode = xmlSiteMapProvider._siteMapNode;
			this.ChildProviderTable.Add(xmlSiteMapProvider, siteMapNode);
			this._childProviderList = null;
			return siteMapNode;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002DD9C File Offset: 0x0002CD9C
		private void HandleResourceAttribute(XmlNode xmlNode, ref NameValueCollection collection, string attrName, ref string text, bool allowImplicitResource)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string text2 = text.TrimStart(new char[] { ' ' });
			if (text2 != null && text2.Length > 10 && text2.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
			{
				if (!allowImplicitResource)
				{
					throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_multiple_resource_definition", new object[] { attrName }), xmlNode);
				}
				string text3 = text2.Substring(11);
				if (text3.Length == 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_resourceKey_cannot_be_empty"), xmlNode);
				}
				int num = text3.IndexOf(',');
				if (num == -1)
				{
					throw new ConfigurationErrorsException(SR.GetString("XmlSiteMapProvider_invalid_resource_key", new object[] { text3 }), xmlNode);
				}
				string text4 = text3.Substring(0, num);
				string text5 = text3.Substring(num + 1);
				int num2 = text5.IndexOf(',');
				if (num2 != -1)
				{
					text = text5.Substring(num2 + 1);
					text5 = text5.Substring(0, num2);
				}
				else
				{
					text = null;
				}
				if (collection == null)
				{
					collection = new NameValueCollection();
				}
				collection.Add(attrName, text4.Trim());
				collection.Add(attrName, text5.Trim());
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002DEE0 File Offset: 0x0002CEE0
		private SiteMapNode GetNodeFromXmlNode(XmlNode xmlNode, Queue queue)
		{
			SiteMapNode siteMapNode = null;
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			HandlerBase.GetAndRemoveStringAttribute(xmlNode, "url", ref text2);
			HandlerBase.GetAndRemoveStringAttribute(xmlNode, "title", ref text);
			HandlerBase.GetAndRemoveStringAttribute(xmlNode, "description", ref text3);
			HandlerBase.GetAndRemoveStringAttribute(xmlNode, "roles", ref text4);
			HandlerBase.GetAndRemoveStringAttribute(xmlNode, "resourceKey", ref text5);
			if (!string.IsNullOrEmpty(text5) && !this.ValidateResource(base.ResourceKey, text5 + ".title"))
			{
				text5 = null;
			}
			HandlerBase.CheckForbiddenAttribute(xmlNode, "securityTrimmingEnabled");
			NameValueCollection nameValueCollection = null;
			bool flag = string.IsNullOrEmpty(text5);
			this.HandleResourceAttribute(xmlNode, ref nameValueCollection, "title", ref text, flag);
			this.HandleResourceAttribute(xmlNode, ref nameValueCollection, "description", ref text3, flag);
			ArrayList arrayList = new ArrayList();
			if (text4 != null)
			{
				int num = text4.IndexOf('?');
				if (num != -1)
				{
					throw new ConfigurationErrorsException(SR.GetString("Auth_rule_names_cant_contain_char", new object[] { text4[num].ToString(CultureInfo.InvariantCulture) }), xmlNode);
				}
				foreach (string text6 in text4.Split(XmlSiteMapProvider._seperators))
				{
					string text7 = text6.Trim();
					if (text7.Length > 0)
					{
						arrayList.Add(text7);
					}
				}
			}
			arrayList = ArrayList.ReadOnly(arrayList);
			string text8 = null;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = text2.Trim();
				if (!UrlPath.IsAbsolutePhysicalPath(text2) && UrlPath.IsRelativeUrl(text2))
				{
					text2 = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, text2);
				}
				string text9 = HttpUtility.UrlDecode(text2);
				if (!string.Equals(text2, text9, StringComparison.Ordinal))
				{
					throw new ConfigurationErrorsException(SR.GetString("Property_Had_Malformed_Url", new object[] { "url", text2 }), xmlNode);
				}
				text8 = text2.ToLowerInvariant();
			}
			else
			{
				text8 = Guid.NewGuid().ToString();
			}
			XmlSiteMapProvider.ReadOnlyNameValueCollection readOnlyNameValueCollection = new XmlSiteMapProvider.ReadOnlyNameValueCollection();
			readOnlyNameValueCollection.SetReadOnly(false);
			foreach (object obj in xmlNode.Attributes)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)obj;
				string value = xmlAttribute.Value;
				this.HandleResourceAttribute(xmlNode, ref nameValueCollection, xmlAttribute.Name, ref value, flag);
				readOnlyNameValueCollection[xmlAttribute.Name] = value;
			}
			readOnlyNameValueCollection.SetReadOnly(true);
			siteMapNode = new SiteMapNode(this, text8, text2, text, text3, arrayList, readOnlyNameValueCollection, nameValueCollection, text5);
			siteMapNode.ReadOnly = true;
			foreach (object obj2 in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				if (xmlNode2.NodeType == XmlNodeType.Element)
				{
					queue.Enqueue(siteMapNode);
					queue.Enqueue(xmlNode2);
				}
			}
			return siteMapNode;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002E1E0 File Offset: 0x0002D1E0
		private SiteMapProvider GetProviderFromName(string providerName)
		{
			SiteMapProvider siteMapProvider = SiteMap.Providers[providerName];
			if (siteMapProvider == null)
			{
				throw new ProviderException(SR.GetString("Provider_Not_Found", new object[] { providerName }));
			}
			return siteMapProvider;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002E219 File Offset: 0x0002D219
		protected internal override SiteMapNode GetRootNodeCore()
		{
			this.BuildSiteMap();
			return this._siteMapNode;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002E228 File Offset: 0x0002D228
		public override void Initialize(string name, NameValueCollection attributes)
		{
			if (this._initialized)
			{
				throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_Cannot_Be_Inited_Twice"));
			}
			if (attributes != null)
			{
				if (string.IsNullOrEmpty(attributes["description"]))
				{
					attributes.Remove("description");
					attributes.Add("description", SR.GetString("XmlSiteMapProvider_Description"));
				}
				string text = null;
				ProviderUtil.GetAndRemoveStringAttribute(attributes, "siteMapFile", name, ref text);
				this._virtualPath = VirtualPath.CreateAllowNull(text);
			}
			base.Initialize(name, attributes);
			if (attributes != null)
			{
				ProviderUtil.CheckUnrecognizedAttributes(attributes, name);
			}
			this._initialized = true;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002E2B8 File Offset: 0x0002D2B8
		private void Initialize(VirtualPath virtualPath, bool secuityTrimmingEnabled)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("siteMapFile", virtualPath.VirtualPathString);
			nameValueCollection.Add("securityTrimmingEnabled", Util.GetStringFromBool(secuityTrimmingEnabled));
			this.Initialize(virtualPath.VirtualPathString, nameValueCollection);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002E2FC File Offset: 0x0002D2FC
		private void OnConfigFileChange(object sender, FileChangeEvent e)
		{
			XmlSiteMapProvider xmlSiteMapProvider = this.ParentProvider as XmlSiteMapProvider;
			if (xmlSiteMapProvider != null)
			{
				xmlSiteMapProvider.OnConfigFileChange(sender, e);
			}
			this.Clear();
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002E328 File Offset: 0x0002D328
		protected internal override void RemoveNode(SiteMapNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			SiteMapProvider provider = node.Provider;
			if (provider != this)
			{
				for (SiteMapProvider siteMapProvider = provider.ParentProvider; siteMapProvider != this; siteMapProvider = siteMapProvider.ParentProvider)
				{
					if (siteMapProvider == null)
					{
						throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_cannot_remove_node", new object[]
						{
							node.ToString(),
							this.Name,
							provider.Name
						}));
					}
				}
			}
			if (node.Equals(provider.GetRootNodeCore()))
			{
				throw new InvalidOperationException(SR.GetString("SiteMapProvider_cannot_remove_root_node"));
			}
			if (provider != this)
			{
				provider.RemoveNode(node);
			}
			base.RemoveNode(node);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002E3C8 File Offset: 0x0002D3C8
		protected virtual void RemoveProvider(string providerName)
		{
			if (providerName == null)
			{
				throw new ArgumentNullException("providerName");
			}
			lock (this._lock)
			{
				SiteMapProvider providerFromName = this.GetProviderFromName(providerName);
				SiteMapNode siteMapNode = (SiteMapNode)this.ChildProviderTable[providerFromName];
				if (siteMapNode == null)
				{
					throw new InvalidOperationException(SR.GetString("XmlSiteMapProvider_cannot_find_provider", new object[] { providerFromName.Name, this.Name }));
				}
				providerFromName.ParentProvider = null;
				this.ChildProviderTable.Remove(providerFromName);
				this._childProviderList = null;
				base.RemoveNode(siteMapNode);
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002E474 File Offset: 0x0002D474
		private bool ValidateResource(string classKey, string resourceKey)
		{
			try
			{
				HttpContext.GetGlobalResourceObject(classKey, resourceKey);
			}
			catch (MissingManifestResourceException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x04001385 RID: 4997
		private const string _providerAttribute = "provider";

		// Token: 0x04001386 RID: 4998
		private const string _siteMapFileAttribute = "siteMapFile";

		// Token: 0x04001387 RID: 4999
		private const string _siteMapNodeName = "siteMapNode";

		// Token: 0x04001388 RID: 5000
		private const string _xmlSiteMapFileExtension = ".sitemap";

		// Token: 0x04001389 RID: 5001
		private const string _resourcePrefix = "$resources:";

		// Token: 0x0400138A RID: 5002
		private const int _resourcePrefixLength = 10;

		// Token: 0x0400138B RID: 5003
		private const char _resourceKeySeparator = ',';

		// Token: 0x0400138C RID: 5004
		private string _filename;

		// Token: 0x0400138D RID: 5005
		private VirtualPath _virtualPath;

		// Token: 0x0400138E RID: 5006
		private VirtualPath _normalizedVirtualPath;

		// Token: 0x0400138F RID: 5007
		private SiteMapNode _siteMapNode;

		// Token: 0x04001390 RID: 5008
		private XmlDocument _document;

		// Token: 0x04001391 RID: 5009
		private bool _initialized;

		// Token: 0x04001392 RID: 5010
		private FileChangeEventHandler _handler;

		// Token: 0x04001393 RID: 5011
		private StringCollection _parentSiteMapFileCollection;

		// Token: 0x04001394 RID: 5012
		private static readonly char[] _seperators = new char[] { ';', ',' };

		// Token: 0x04001395 RID: 5013
		private ArrayList _childProviderList;

		// Token: 0x04001396 RID: 5014
		private Hashtable _childProviderTable;

		// Token: 0x020000F2 RID: 242
		private class ReadOnlyNameValueCollection : NameValueCollection
		{
			// Token: 0x06000B9D RID: 2973 RVA: 0x0002E4C8 File Offset: 0x0002D4C8
			public ReadOnlyNameValueCollection()
			{
				base.IsReadOnly = true;
			}

			// Token: 0x06000B9E RID: 2974 RVA: 0x0002E4D7 File Offset: 0x0002D4D7
			internal void SetReadOnly(bool isReadonly)
			{
				base.IsReadOnly = isReadonly;
			}
		}
	}
}
