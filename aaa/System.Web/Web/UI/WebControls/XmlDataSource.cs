using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;
using System.Xml;
using System.Xml.Xsl;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200068E RID: 1678
	[WebSysDescription("XmlDataSource_Description")]
	[DefaultProperty("DataFile")]
	[PersistChildren(false)]
	[WebSysDisplayName("XmlDataSource_DisplayName")]
	[DefaultEvent("Transforming")]
	[ToolboxBitmap(typeof(XmlDataSource))]
	[Designer("System.Web.UI.Design.WebControls.XmlDataSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ParseChildren(true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class XmlDataSource : HierarchicalDataSourceControl, IDataSource, IListSource
	{
		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x0014C21E File Offset: 0x0014B21E
		private DataSourceCache Cache
		{
			get
			{
				if (this._cache == null)
				{
					this._cache = new DataSourceCache();
					this._cache.Enabled = true;
				}
				return this._cache;
			}
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x0014C245 File Offset: 0x0014B245
		// (set) Token: 0x06005235 RID: 21045 RVA: 0x0014C252 File Offset: 0x0014B252
		[WebCategory("Cache")]
		[DefaultValue(0)]
		[TypeConverter(typeof(DataSourceCacheDurationConverter))]
		[WebSysDescription("DataSourceCache_Duration")]
		public virtual int CacheDuration
		{
			get
			{
				return this.Cache.Duration;
			}
			set
			{
				this.Cache.Duration = value;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x0014C260 File Offset: 0x0014B260
		// (set) Token: 0x06005237 RID: 21047 RVA: 0x0014C26D File Offset: 0x0014B26D
		[DefaultValue(DataSourceCacheExpiry.Absolute)]
		[WebSysDescription("DataSourceCache_ExpirationPolicy")]
		[WebCategory("Cache")]
		public virtual DataSourceCacheExpiry CacheExpirationPolicy
		{
			get
			{
				return this.Cache.ExpirationPolicy;
			}
			set
			{
				this.Cache.ExpirationPolicy = value;
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06005238 RID: 21048 RVA: 0x0014C27B File Offset: 0x0014B27B
		// (set) Token: 0x06005239 RID: 21049 RVA: 0x0014C288 File Offset: 0x0014B288
		[WebSysDescription("DataSourceCache_KeyDependency")]
		[DefaultValue("")]
		[WebCategory("Cache")]
		public virtual string CacheKeyDependency
		{
			get
			{
				return this.Cache.KeyDependency;
			}
			set
			{
				this.Cache.KeyDependency = value;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x0600523A RID: 21050 RVA: 0x0014C296 File Offset: 0x0014B296
		// (set) Token: 0x0600523B RID: 21051 RVA: 0x0014C2AC File Offset: 0x0014B2AC
		[Editor("System.ComponentModel.Design.MultilineStringEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TypeConverter("System.ComponentModel.MultilineStringConverter,System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
		[WebCategory("Data")]
		[WebSysDescription("XmlDataSource_Data")]
		public virtual string Data
		{
			get
			{
				if (this._data == null)
				{
					return string.Empty;
				}
				return this._data;
			}
			set
			{
				if (value != null)
				{
					value = value.Trim();
				}
				if (this.Data != value)
				{
					if (this._disallowChanges)
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_CannotChangeWhileLoading", new object[] { "Data", this.ID }));
					}
					this._data = value;
					this._xmlDocument = null;
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x0014C31C File Offset: 0x0014B31C
		// (set) Token: 0x0600523D RID: 21053 RVA: 0x0014C334 File Offset: 0x0014B334
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.XmlDataFileEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebCategory("Data")]
		[WebSysDescription("XmlDataSource_DataFile")]
		public virtual string DataFile
		{
			get
			{
				if (this._dataFile == null)
				{
					return string.Empty;
				}
				return this._dataFile;
			}
			set
			{
				if (this.DataFile != value)
				{
					if (this._disallowChanges)
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_CannotChangeWhileLoading", new object[] { "DataFile", this.ID }));
					}
					this._dataFile = value;
					this._xmlDocument = null;
					this._writeableDataFile = null;
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x0600523E RID: 21054 RVA: 0x0014C3A0 File Offset: 0x0014B3A0
		// (set) Token: 0x0600523F RID: 21055 RVA: 0x0014C3AD File Offset: 0x0014B3AD
		[WebSysDescription("DataSourceCache_Enabled")]
		[DefaultValue(true)]
		[WebCategory("Cache")]
		public virtual bool EnableCaching
		{
			get
			{
				return this.Cache.Enabled;
			}
			set
			{
				this.Cache.Enabled = value;
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x0014C3BB File Offset: 0x0014B3BB
		internal bool IsModifiable
		{
			get
			{
				return string.IsNullOrEmpty(this.TransformFile) && string.IsNullOrEmpty(this.Transform) && !string.IsNullOrEmpty(this.WriteableDataFile);
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x06005241 RID: 21057 RVA: 0x0014C3E7 File Offset: 0x0014B3E7
		// (set) Token: 0x06005242 RID: 21058 RVA: 0x0014C400 File Offset: 0x0014B400
		[WebCategory("Data")]
		[DefaultValue("")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TypeConverter("System.ComponentModel.MultilineStringConverter,System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
		[WebSysDescription("XmlDataSource_Transform")]
		public virtual string Transform
		{
			get
			{
				if (this._transform == null)
				{
					return string.Empty;
				}
				return this._transform;
			}
			set
			{
				if (value != null)
				{
					value = value.Trim();
				}
				if (this.Transform != value)
				{
					if (this._disallowChanges)
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_CannotChangeWhileLoading", new object[] { "Transform", this.ID }));
					}
					this._transform = value;
					this._xmlDocument = null;
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x0014C470 File Offset: 0x0014B470
		// (set) Token: 0x06005244 RID: 21060 RVA: 0x0014C478 File Offset: 0x0014B478
		[Browsable(false)]
		public virtual XsltArgumentList TransformArgumentList
		{
			get
			{
				return this._transformArgumentList;
			}
			set
			{
				this._transformArgumentList = value;
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06005245 RID: 21061 RVA: 0x0014C481 File Offset: 0x0014B481
		// (set) Token: 0x06005246 RID: 21062 RVA: 0x0014C498 File Offset: 0x0014B498
		[WebCategory("Data")]
		[WebSysDescription("XmlDataSource_TransformFile")]
		[Editor("System.Web.UI.Design.XslTransformFileEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		public virtual string TransformFile
		{
			get
			{
				if (this._transformFile == null)
				{
					return string.Empty;
				}
				return this._transformFile;
			}
			set
			{
				if (this.TransformFile != value)
				{
					if (this._disallowChanges)
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_CannotChangeWhileLoading", new object[] { "TransformFile", this.ID }));
					}
					this._transformFile = value;
					this._xmlDocument = null;
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06005247 RID: 21063 RVA: 0x0014C4FD File Offset: 0x0014B4FD
		private string WriteableDataFile
		{
			get
			{
				if (this._writeableDataFile == null)
				{
					this._writeableDataFile = this.GetWriteableDataFile();
				}
				return this._writeableDataFile;
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06005248 RID: 21064 RVA: 0x0014C519 File Offset: 0x0014B519
		// (set) Token: 0x06005249 RID: 21065 RVA: 0x0014C530 File Offset: 0x0014B530
		[WebSysDescription("XmlDataSource_XPath")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public virtual string XPath
		{
			get
			{
				if (this._xPath == null)
				{
					return string.Empty;
				}
				return this._xPath;
			}
			set
			{
				if (this.XPath != value)
				{
					if (this._disallowChanges)
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_CannotChangeWhileLoading", new object[] { "XPath", this.ID }));
					}
					this._xPath = value;
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x0600524A RID: 21066 RVA: 0x0014C58E File Offset: 0x0014B58E
		// (remove) Token: 0x0600524B RID: 21067 RVA: 0x0014C5A1 File Offset: 0x0014B5A1
		[WebCategory("Data")]
		[WebSysDescription("XmlDataSource_Transforming")]
		public event EventHandler Transforming
		{
			add
			{
				base.Events.AddHandler(XmlDataSource.EventTransforming, value);
			}
			remove
			{
				base.Events.RemoveHandler(XmlDataSource.EventTransforming, value);
			}
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x0014C5B4 File Offset: 0x0014B5B4
		private string CreateCacheKey()
		{
			StringBuilder stringBuilder = new StringBuilder("u", 1024);
			stringBuilder.Append(base.GetType().GetHashCode().ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(this.CacheDuration.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(':');
			stringBuilder.Append(((int)this.CacheExpirationPolicy).ToString(CultureInfo.InvariantCulture));
			bool flag = false;
			if (this.DataFile.Length > 0)
			{
				stringBuilder.Append(':');
				stringBuilder.Append(this.DataFile);
			}
			else if (this.Data.Length > 0)
			{
				flag = true;
			}
			if (this.TransformFile.Length > 0)
			{
				stringBuilder.Append(':');
				stringBuilder.Append(this.TransformFile);
			}
			else if (this.Transform.Length > 0)
			{
				flag = true;
			}
			if (flag)
			{
				if (this.Page != null)
				{
					stringBuilder.Append(':');
					stringBuilder.Append(this.Page.GetType().AssemblyQualifiedName);
				}
				stringBuilder.Append(':');
				string uniqueID = this.UniqueID;
				if (string.IsNullOrEmpty(uniqueID))
				{
					throw new InvalidOperationException(SR.GetString("XmlDataSource_NeedUniqueIDForCache"));
				}
				stringBuilder.Append(uniqueID);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x0014C6FE File Offset: 0x0014B6FE
		protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
		{
			return new XmlHierarchicalDataSourceView(this, viewPath);
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x0014C708 File Offset: 0x0014B708
		private XmlReader GetReader(string path, string content, out CacheDependency cacheDependency)
		{
			if (path.Length != 0)
			{
				Uri uri;
				bool flag = Uri.TryCreate(path, UriKind.Absolute, out uri);
				if (flag && uri.Scheme == Uri.UriSchemeHttp)
				{
					if (!HttpRuntime.HasWebPermission(uri))
					{
						throw new InvalidOperationException(SR.GetString("XmlDataSource_NoWebPermission", new object[] { uri.PathAndQuery, this.ID }));
					}
					cacheDependency = null;
					if (AppSettings.RestrictXmlControls)
					{
						return new NoEntitiesXmlReader(path);
					}
					return new XmlTextReader(path);
				}
				else
				{
					VirtualPath virtualPath;
					string text;
					base.ResolvePhysicalOrVirtualPath(path, out virtualPath, out text);
					if (virtualPath != null && base.DesignMode)
					{
						throw new NotSupportedException(SR.GetString("XmlDataSource_DesignTimeRelativePathsNotSupported", new object[] { this.ID }));
					}
					Stream stream = base.OpenFileAndGetDependency(virtualPath, text, out cacheDependency);
					if (AppSettings.RestrictXmlControls)
					{
						return new NoEntitiesXmlReader(stream);
					}
					return new XmlTextReader(stream);
				}
			}
			else
			{
				cacheDependency = null;
				content = content.Trim();
				if (content.Length == 0)
				{
					return null;
				}
				if (AppSettings.RestrictXmlControls)
				{
					return new NoEntitiesXmlReader(new StringReader(content));
				}
				return new XmlTextReader(new StringReader(content));
			}
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x0014C824 File Offset: 0x0014B824
		private string GetWriteableDataFile()
		{
			if (this.DataFile.Length == 0)
			{
				return null;
			}
			Uri uri;
			bool flag = Uri.TryCreate(this.DataFile, UriKind.Absolute, out uri);
			if (flag && uri.Scheme == Uri.UriSchemeHttp)
			{
				return null;
			}
			if (HostingEnvironment.UsingMapPathBasedVirtualPathProvider)
			{
				VirtualPath virtualPath;
				string text;
				base.ResolvePhysicalOrVirtualPath(this.DataFile, out virtualPath, out text);
				if (text == null)
				{
					text = virtualPath.MapPathInternal(base.TemplateControlVirtualDirectory, true);
				}
				return text;
			}
			return null;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x0014C894 File Offset: 0x0014B894
		public XmlDocument GetXmlDocument()
		{
			string text = null;
			if (!this._cacheLookupDone && this.Cache.Enabled)
			{
				text = this.CreateCacheKey();
				this._xmlDocument = this.Cache.LoadDataFromCache(text) as XmlDocument;
				this._cacheLookupDone = true;
			}
			if (this._xmlDocument == null)
			{
				this._xmlDocument = new XmlDocument();
				CacheDependency cacheDependency;
				CacheDependency cacheDependency2;
				this.PopulateXmlDocument(this._xmlDocument, out cacheDependency, out cacheDependency2);
				if (text != null)
				{
					CacheDependency cacheDependency3;
					if (cacheDependency != null)
					{
						if (cacheDependency2 != null)
						{
							AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
							aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency, cacheDependency2 });
							cacheDependency3 = aggregateCacheDependency;
						}
						else
						{
							cacheDependency3 = cacheDependency;
						}
					}
					else
					{
						cacheDependency3 = cacheDependency2;
					}
					this.Cache.SaveDataToCache(text, this._xmlDocument, cacheDependency3);
				}
			}
			return this._xmlDocument;
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x0014C950 File Offset: 0x0014B950
		private void PopulateXmlDocument(XmlDocument document, out CacheDependency dataCacheDependency, out CacheDependency transformCacheDependency)
		{
			XmlReader xmlReader = null;
			XmlReader xmlReader2 = null;
			XmlReader xmlReader3 = null;
			try
			{
				this._disallowChanges = true;
				xmlReader = this.GetReader(this.TransformFile, this.Transform, out transformCacheDependency);
				if (xmlReader != null)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlReader3 = this.GetReader(this.DataFile, this.Data, out dataCacheDependency);
					xmlDocument.Load(xmlReader3);
					if (AppSettings.RestrictXmlControls)
					{
						((XmlTextReader)xmlReader).ProhibitDtd = false;
						XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
						xslCompiledTransform.Load(xmlReader, null, null);
						this.OnTransforming(EventArgs.Empty);
						using (MemoryStream memoryStream = new MemoryStream())
						{
							xslCompiledTransform.Transform(xmlDocument, this._transformArgumentList, memoryStream);
							document.Load(memoryStream);
							goto IL_00F4;
						}
					}
					XslTransform xslTransform = new XslTransform();
					xslTransform.Load(xmlReader, null, null);
					this.OnTransforming(EventArgs.Empty);
					xmlReader2 = xslTransform.Transform(xmlDocument, this._transformArgumentList, null);
					document.Load(xmlReader2);
				}
				else
				{
					xmlReader2 = this.GetReader(this.DataFile, this.Data, out dataCacheDependency);
					document.Load(xmlReader2);
				}
				IL_00F4:;
			}
			finally
			{
				this._disallowChanges = false;
				if (xmlReader2 != null)
				{
					xmlReader2.Close();
				}
				if (xmlReader3 != null)
				{
					xmlReader3.Close();
				}
				if (xmlReader != null)
				{
					xmlReader.Close();
				}
			}
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x0014CA94 File Offset: 0x0014BA94
		protected virtual void OnTransforming(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[XmlDataSource.EventTransforming];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x0014CAC4 File Offset: 0x0014BAC4
		public void Save()
		{
			if (!this.IsModifiable)
			{
				throw new InvalidOperationException(SR.GetString("XmlDataSource_SaveNotAllowed", new object[] { this.ID }));
			}
			string writeableDataFile = this.WriteableDataFile;
			HttpRuntime.CheckFilePermission(writeableDataFile, true);
			this.GetXmlDocument().Save(writeableDataFile);
		}

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x06005254 RID: 21076 RVA: 0x0014CB14 File Offset: 0x0014BB14
		// (remove) Token: 0x06005255 RID: 21077 RVA: 0x0014CB1D File Offset: 0x0014BB1D
		event EventHandler IDataSource.DataSourceChanged
		{
			add
			{
				((IHierarchicalDataSource)this).DataSourceChanged += value;
			}
			remove
			{
				((IHierarchicalDataSource)this).DataSourceChanged -= value;
			}
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x0014CB26 File Offset: 0x0014BB26
		DataSourceView IDataSource.GetView(string viewName)
		{
			if (viewName.Length == 0)
			{
				viewName = "DefaultView";
			}
			return new XmlDataSourceView(this, viewName);
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x0014CB40 File Offset: 0x0014BB40
		ICollection IDataSource.GetViewNames()
		{
			if (this._viewNames == null)
			{
				this._viewNames = new string[] { "DefaultView" };
			}
			return this._viewNames;
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06005258 RID: 21080 RVA: 0x0014CB71 File Offset: 0x0014BB71
		bool IListSource.ContainsListCollection
		{
			get
			{
				return !base.DesignMode && ListSourceHelper.ContainsListCollection(this);
			}
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x0014CB83 File Offset: 0x0014BB83
		IList IListSource.GetList()
		{
			if (base.DesignMode)
			{
				return null;
			}
			return ListSourceHelper.GetList(this);
		}

		// Token: 0x04002DE8 RID: 11752
		private const string DefaultViewName = "DefaultView";

		// Token: 0x04002DE9 RID: 11753
		private static readonly object EventTransforming = new object();

		// Token: 0x04002DEA RID: 11754
		private DataSourceCache _cache;

		// Token: 0x04002DEB RID: 11755
		private bool _cacheLookupDone;

		// Token: 0x04002DEC RID: 11756
		private bool _disallowChanges;

		// Token: 0x04002DED RID: 11757
		private XsltArgumentList _transformArgumentList;

		// Token: 0x04002DEE RID: 11758
		private ICollection _viewNames;

		// Token: 0x04002DEF RID: 11759
		private XmlDocument _xmlDocument;

		// Token: 0x04002DF0 RID: 11760
		private string _writeableDataFile;

		// Token: 0x04002DF1 RID: 11761
		private string _data;

		// Token: 0x04002DF2 RID: 11762
		private string _dataFile;

		// Token: 0x04002DF3 RID: 11763
		private string _transform;

		// Token: 0x04002DF4 RID: 11764
		private string _transformFile;

		// Token: 0x04002DF5 RID: 11765
		private string _xPath;
	}
}
