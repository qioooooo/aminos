using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Util;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004BD RID: 1213
	[DefaultProperty("AdvertisementFile")]
	[Designer("System.Web.UI.Design.WebControls.AdRotatorDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("AdCreated")]
	[ToolboxData("<{0}:AdRotator runat=\"server\"></{0}:AdRotator>")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AdRotator : DataBoundControl
	{
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x000F3CA9 File Offset: 0x000F2CA9
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x000F3CBF File Offset: 0x000F2CBF
		[Bindable(true)]
		[UrlProperty]
		[WebSysDescription("AdRotator_AdvertisementFile")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.XmlUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string AdvertisementFile
		{
			get
			{
				if (this._advertisementFile != null)
				{
					return this._advertisementFile;
				}
				return string.Empty;
			}
			set
			{
				this._advertisementFile = value;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x060039A7 RID: 14759 RVA: 0x000F3CC8 File Offset: 0x000F2CC8
		// (set) Token: 0x060039A8 RID: 14760 RVA: 0x000F3CF5 File Offset: 0x000F2CF5
		[WebCategory("Behavior")]
		[WebSysDescription("AdRotator_AlternateTextField")]
		[DefaultValue("AlternateText")]
		public string AlternateTextField
		{
			get
			{
				string text = (string)this.ViewState["AlternateTextField"];
				if (text == null)
				{
					return "AlternateText";
				}
				return text;
			}
			set
			{
				this.ViewState["AlternateTextField"] = value;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060039A9 RID: 14761 RVA: 0x000F3D08 File Offset: 0x000F2D08
		internal string BaseUrl
		{
			get
			{
				if (this._baseUrl == null)
				{
					string virtualPathString = base.TemplateControlVirtualDirectory.VirtualPathString;
					string text = null;
					if (!string.IsNullOrEmpty(this.AdvertisementFile))
					{
						string text2 = UrlPath.Combine(virtualPathString, this.AdvertisementFile);
						text = UrlPath.GetDirectory(text2);
					}
					this._baseUrl = string.Empty;
					if (text != null)
					{
						this._baseUrl = text;
					}
					if (this._baseUrl.Length == 0)
					{
						this._baseUrl = virtualPathString;
					}
				}
				return this._baseUrl;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x060039AA RID: 14762 RVA: 0x000F3D7D File Offset: 0x000F2D7D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override FontInfo Font
		{
			get
			{
				return base.Font;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x000F3D88 File Offset: 0x000F2D88
		// (set) Token: 0x060039AC RID: 14764 RVA: 0x000F3DB5 File Offset: 0x000F2DB5
		[WebSysDescription("AdRotator_ImageUrlField")]
		[WebCategory("Behavior")]
		[DefaultValue("ImageUrl")]
		public string ImageUrlField
		{
			get
			{
				string text = (string)this.ViewState["ImageUrlField"];
				if (text == null)
				{
					return "ImageUrl";
				}
				return text;
			}
			set
			{
				this.ViewState["ImageUrlField"] = value;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x000F3DC8 File Offset: 0x000F2DC8
		private bool IsTargetSet
		{
			get
			{
				return this.ViewState["Target"] != null;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x000F3DE0 File Offset: 0x000F2DE0
		// (set) Token: 0x060039AF RID: 14767 RVA: 0x000F3DE8 File Offset: 0x000F2DE8
		internal bool IsPostCacheAdHelper
		{
			get
			{
				return this._isPostCacheAdHelper;
			}
			set
			{
				this._isPostCacheAdHelper = value;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x000F3DF4 File Offset: 0x000F2DF4
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x000F3E21 File Offset: 0x000F2E21
		[Bindable(true)]
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[WebSysDescription("AdRotator_KeywordFilter")]
		public string KeywordFilter
		{
			get
			{
				string text = (string)this.ViewState["KeywordFilter"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ViewState.Remove("KeywordFilter");
					return;
				}
				this.ViewState["KeywordFilter"] = value.Trim();
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000F3E54 File Offset: 0x000F2E54
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000F3E81 File Offset: 0x000F2E81
		[WebSysDescription("AdRotator_NavigateUrlField")]
		[DefaultValue("NavigateUrl")]
		[WebCategory("Behavior")]
		public string NavigateUrlField
		{
			get
			{
				string text = (string)this.ViewState["NavigateUrlField"];
				if (text == null)
				{
					return "NavigateUrl";
				}
				return text;
			}
			set
			{
				this.ViewState["NavigateUrlField"] = value;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000F3E94 File Offset: 0x000F2E94
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x000F3E9C File Offset: 0x000F2E9C
		private AdCreatedEventArgs SelectedAdArgs
		{
			get
			{
				return this._adCreatedEventArgs;
			}
			set
			{
				this._adCreatedEventArgs = value;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x060039B6 RID: 14774 RVA: 0x000F3EA8 File Offset: 0x000F2EA8
		// (set) Token: 0x060039B7 RID: 14775 RVA: 0x000F3ED5 File Offset: 0x000F2ED5
		[WebCategory("Behavior")]
		[TypeConverter(typeof(TargetConverter))]
		[DefaultValue("_top")]
		[WebSysDescription("AdRotator_Target")]
		[Bindable(true)]
		public string Target
		{
			get
			{
				string text = (string)this.ViewState["Target"];
				if (text != null)
				{
					return text;
				}
				return "_top";
			}
			set
			{
				this.ViewState["Target"] = value;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x000F3EE8 File Offset: 0x000F2EE8
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.A;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x060039B9 RID: 14777 RVA: 0x000F3EEB File Offset: 0x000F2EEB
		public override string UniqueID
		{
			get
			{
				if (this._uniqueID == null)
				{
					this._uniqueID = base.UniqueID;
				}
				return this._uniqueID;
			}
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060039BA RID: 14778 RVA: 0x000F3F07 File Offset: 0x000F2F07
		// (remove) Token: 0x060039BB RID: 14779 RVA: 0x000F3F1A File Offset: 0x000F2F1A
		[WebSysDescription("AdRotator_OnAdCreated")]
		[WebCategory("Action")]
		public event AdCreatedEventHandler AdCreated
		{
			add
			{
				base.Events.AddHandler(AdRotator.EventAdCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(AdRotator.EventAdCreated, value);
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000F3F30 File Offset: 0x000F2F30
		private void CheckOnlyOneDataSource()
		{
			int num = ((this.AdvertisementFile.Length > 0) ? 1 : 0);
			num += ((this.DataSourceID.Length > 0) ? 1 : 0);
			num += ((this.DataSource != null) ? 1 : 0);
			if (num > 1)
			{
				throw new HttpException(SR.GetString("AdRotator_only_one_datasource", new object[] { this.ID }));
			}
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x000F3F9C File Offset: 0x000F2F9C
		internal void CopyFrom(AdRotator adRotator)
		{
			this._adRecs = adRotator._adRecs;
			this.AccessKey = adRotator.AccessKey;
			this.AlternateTextField = adRotator.AlternateTextField;
			this.Enabled = adRotator.Enabled;
			this.ImageUrlField = adRotator.ImageUrlField;
			this.NavigateUrlField = adRotator.NavigateUrlField;
			this.TabIndex = adRotator.TabIndex;
			this.Target = adRotator.Target;
			this.ToolTip = adRotator.ToolTip;
			string id = adRotator.ID;
			if (!string.IsNullOrEmpty(id))
			{
				this.ID = adRotator.ClientID;
			}
			this._uniqueID = adRotator.UniqueID;
			this._baseUrl = adRotator.BaseUrl;
			if (adRotator.HasAttributes)
			{
				foreach (object obj in adRotator.Attributes.Keys)
				{
					string text = (string)obj;
					base.Attributes[text] = adRotator.Attributes[text];
				}
			}
			if (adRotator.ControlStyleCreated)
			{
				base.ControlStyle.CopyFrom(adRotator.ControlStyle);
			}
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x000F40CC File Offset: 0x000F30CC
		private ArrayList CreateAutoGeneratedFields(IEnumerable dataSource)
		{
			if (dataSource == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			if (dataSource is ITypedList)
			{
				propertyDescriptorCollection = ((ITypedList)dataSource).GetItemProperties(new PropertyDescriptor[0]);
			}
			if (propertyDescriptorCollection == null)
			{
				IEnumerator enumerator = dataSource.GetEnumerator();
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (this.IsBindableType(obj.GetType()))
					{
						throw new HttpException(SR.GetString("AdRotator_expect_records_with_advertisement_properties", new object[]
						{
							this.ID,
							obj.GetType()
						}));
					}
					propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
				}
			}
			if (propertyDescriptorCollection != null && propertyDescriptorCollection.Count > 0)
			{
				foreach (object obj2 in propertyDescriptorCollection)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					if (this.IsBindableType(propertyDescriptor.PropertyType))
					{
						arrayList.Add(propertyDescriptor.Name);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x000F41D0 File Offset: 0x000F31D0
		internal bool DoPostCacheSubstitutionAsNeeded(HtmlTextWriter writer)
		{
			if (!this.IsPostCacheAdHelper && this.SelectedAdArgs == null && this.Page.Response.HasCachePolicy && this.Page.Response.Cache.GetCacheability() != (HttpCacheability)6)
			{
				AdPostCacheSubstitution adPostCacheSubstitution = new AdPostCacheSubstitution(this);
				adPostCacheSubstitution.RegisterPostCacheCallBack(this.Context, this.Page, writer);
				return true;
			}
			return false;
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x000F4234 File Offset: 0x000F3234
		private AdCreatedEventArgs GetAdCreatedEventArgs()
		{
			IDictionary dictionary = this.SelectAdFromRecords();
			return new AdCreatedEventArgs(dictionary, this.ImageUrlField, this.NavigateUrlField, this.AlternateTextField);
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x000F4264 File Offset: 0x000F3264
		private AdRotator.AdRec[] GetDataSourceData(IEnumerable dataSource)
		{
			ArrayList arrayList = this.CreateAutoGeneratedFields(dataSource);
			ArrayList arrayList2 = new ArrayList();
			IEnumerator enumerator = dataSource.GetEnumerator();
			while (enumerator.MoveNext())
			{
				IDictionary dictionary = null;
				foreach (object obj in arrayList)
				{
					string text = (string)obj;
					if (dictionary == null)
					{
						dictionary = new HybridDictionary();
					}
					dictionary.Add(text, DataBinder.GetPropertyValue(enumerator.Current, text));
				}
				if (dictionary != null)
				{
					arrayList2.Add(dictionary);
				}
			}
			return this.SetAdRecs(arrayList2);
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x000F430C File Offset: 0x000F330C
		private AdRotator.AdRec[] GetFileData(string fileName)
		{
			VirtualPath virtualPath;
			string text;
			base.ResolvePhysicalOrVirtualPath(fileName, out virtualPath, out text);
			string text2 = "n" + ((!string.IsNullOrEmpty(text)) ? text : virtualPath.VirtualPathString);
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			AdRotator.AdRec[] array = cacheInternal[text2] as AdRotator.AdRec[];
			if (array == null)
			{
				CacheDependency cacheDependency;
				try
				{
					using (Stream stream = base.OpenFileAndGetDependency(virtualPath, text, out cacheDependency))
					{
						array = this.LoadStream(stream);
					}
				}
				catch (Exception ex)
				{
					if (!string.IsNullOrEmpty(text) && HttpRuntime.HasPathDiscoveryPermission(text))
					{
						throw new HttpException(SR.GetString("AdRotator_cant_open_file", new object[] { this.ID, ex.Message }));
					}
					throw new HttpException(SR.GetString("AdRotator_cant_open_file_no_permission", new object[] { this.ID }));
				}
				if (cacheDependency != null)
				{
					using (cacheDependency)
					{
						cacheInternal.UtcInsert(text2, array, cacheDependency);
					}
				}
			}
			return array;
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x000F4434 File Offset: 0x000F3434
		private static int GetRandomNumber(int maxValue)
		{
			if (AdRotator._random == null)
			{
				AdRotator._random = new Random();
			}
			return AdRotator._random.Next(maxValue) + 1;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x000F4454 File Offset: 0x000F3454
		private AdRotator.AdRec[] GetXmlDataSourceData(XmlDataSource xmlDataSource)
		{
			XmlDocument xmlDocument = xmlDataSource.GetXmlDocument();
			if (xmlDocument == null)
			{
				return null;
			}
			return this.LoadXmlDocument(xmlDocument);
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x000F4474 File Offset: 0x000F3474
		private bool IsBindableType(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x000F44A8 File Offset: 0x000F34A8
		private bool IsOnAdCreatedOverridden()
		{
			bool flag = false;
			Type type = base.GetType();
			if (type != AdRotator._adrotatorType)
			{
				MethodInfo method = type.GetMethod("OnAdCreated", BindingFlags.Instance | BindingFlags.NonPublic, null, AdRotator._AdCreatedParameterTypes, null);
				if (method.DeclaringType != AdRotator._adrotatorType)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x000F44EC File Offset: 0x000F34EC
		private AdRotator.AdRec[] LoadFromXmlReader(XmlReader reader)
		{
			ArrayList arrayList = new ArrayList();
			while (reader.Read())
			{
				if (reader.Name == "Advertisements")
				{
					if (reader.Depth != 0)
					{
						return null;
					}
					IL_00A7:
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "Ad" && reader.Depth == 1)
						{
							IDictionary dictionary = null;
							reader.Read();
							while (reader.NodeType != XmlNodeType.EndElement)
							{
								if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
								{
									if (dictionary == null)
									{
										dictionary = new HybridDictionary();
									}
									dictionary.Add(reader.LocalName, reader.ReadString());
								}
								reader.Skip();
							}
							if (dictionary != null)
							{
								arrayList.Add(dictionary);
							}
						}
					}
					return this.SetAdRecs(arrayList);
				}
			}
			goto IL_00A7;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x000F45B4 File Offset: 0x000F35B4
		private AdRotator.AdRec[] LoadStream(Stream stream)
		{
			AdRotator.AdRec[] array = null;
			try
			{
				XmlReader xmlReader = new XmlTextReader(stream);
				array = this.LoadFromXmlReader(xmlReader);
			}
			catch (Exception ex)
			{
				throw new HttpException(SR.GetString("AdRotator_parse_error", new object[] { this.ID, ex.Message }), ex);
			}
			if (array == null)
			{
				throw new HttpException(SR.GetString("AdRotator_no_advertisements", new object[] { this.ID, this.AdvertisementFile }));
			}
			return array;
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x000F4644 File Offset: 0x000F3644
		private AdRotator.AdRec[] LoadXmlDocument(XmlDocument doc)
		{
			ArrayList arrayList = new ArrayList();
			if (doc.DocumentElement != null && doc.DocumentElement.LocalName == "Advertisements")
			{
				for (XmlNode xmlNode = doc.DocumentElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
				{
					IDictionary dictionary = null;
					if (xmlNode.LocalName.Equals("Ad"))
					{
						for (XmlNode xmlNode2 = xmlNode.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
						{
							if (xmlNode2.NodeType == XmlNodeType.Element)
							{
								if (dictionary == null)
								{
									dictionary = new HybridDictionary();
								}
								dictionary.Add(xmlNode2.LocalName, xmlNode2.InnerText);
							}
						}
					}
					if (dictionary != null)
					{
						arrayList.Add(dictionary);
					}
				}
			}
			return this.SetAdRecs(arrayList);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x000F46F2 File Offset: 0x000F36F2
		private bool MatchingAd(AdRotator.AdRec adRec, string keywordFilter)
		{
			return string.Equals(keywordFilter, adRec.keyword, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x000F4704 File Offset: 0x000F3704
		protected virtual void OnAdCreated(AdCreatedEventArgs e)
		{
			AdCreatedEventHandler adCreatedEventHandler = (AdCreatedEventHandler)base.Events[AdRotator.EventAdCreated];
			if (adCreatedEventHandler != null)
			{
				adCreatedEventHandler(this, e);
			}
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x000F4732 File Offset: 0x000F3732
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			base.RequiresDataBinding = true;
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x000F4744 File Offset: 0x000F3744
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this._adRecs == null && this.AdvertisementFile.Length > 0)
			{
				this.PerformAdFileBinding();
			}
			if (base.Events[AdRotator.EventAdCreated] != null || this.IsOnAdCreatedOverridden())
			{
				this.SelectedAdArgs = this.GetAdCreatedEventArgs();
				this.OnAdCreated(this.SelectedAdArgs);
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x000F47A6 File Offset: 0x000F37A6
		private void PerformAdFileBinding()
		{
			this.OnDataBinding(EventArgs.Empty);
			this._adRecs = this.GetFileData(this.AdvertisementFile);
			this.OnDataBound(EventArgs.Empty);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x000F47D0 File Offset: 0x000F37D0
		protected internal override void PerformDataBinding(IEnumerable data)
		{
			if (data != null)
			{
				object dataSource = this.DataSource;
				XmlDataSource xmlDataSource;
				if (dataSource != null)
				{
					xmlDataSource = dataSource as XmlDataSource;
				}
				else
				{
					xmlDataSource = this.GetDataSource() as XmlDataSource;
				}
				if (xmlDataSource != null)
				{
					this._adRecs = this.GetXmlDataSourceData(xmlDataSource);
					return;
				}
				this._adRecs = this.GetDataSourceData(data);
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x000F481F File Offset: 0x000F381F
		protected override void PerformSelect()
		{
			this.CheckOnlyOneDataSource();
			if (this.AdvertisementFile.Length > 0)
			{
				this.PerformAdFileBinding();
				return;
			}
			base.PerformSelect();
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x000F4844 File Offset: 0x000F3844
		internal AdCreatedEventArgs PickAd()
		{
			AdCreatedEventArgs adCreatedEventArgs = this.SelectedAdArgs;
			if (adCreatedEventArgs == null)
			{
				adCreatedEventArgs = this.GetAdCreatedEventArgs();
			}
			adCreatedEventArgs.ImageUrl = this.ResolveAdRotatorUrl(this.BaseUrl, adCreatedEventArgs.ImageUrl);
			adCreatedEventArgs.NavigateUrl = this.ResolveAdRotatorUrl(this.BaseUrl, adCreatedEventArgs.NavigateUrl);
			return adCreatedEventArgs;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000F4894 File Offset: 0x000F3894
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (!base.DesignMode && !this.IsPostCacheAdHelper && this.DoPostCacheSubstitutionAsNeeded(writer))
			{
				return;
			}
			AdCreatedEventArgs adCreatedEventArgs = this.PickAd();
			this.RenderLink(writer, adCreatedEventArgs);
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x000F48CC File Offset: 0x000F38CC
		private void RenderLink(HtmlTextWriter writer, AdCreatedEventArgs adArgs)
		{
			HyperLink hyperLink = new HyperLink();
			hyperLink.NavigateUrl = adArgs.NavigateUrl;
			hyperLink.Target = this.Target;
			if (base.HasAttributes)
			{
				foreach (object obj in base.Attributes.Keys)
				{
					string text = (string)obj;
					hyperLink.Attributes[text] = base.Attributes[text];
				}
			}
			string id = this.ID;
			if (!string.IsNullOrEmpty(id))
			{
				hyperLink.ID = this.ClientID;
			}
			if (!this.Enabled)
			{
				hyperLink.Enabled = false;
			}
			string text2 = (string)this.ViewState["AccessKey"];
			if (!string.IsNullOrEmpty(text2))
			{
				hyperLink.AccessKey = text2;
			}
			object obj2 = this.ViewState["TabIndex"];
			if (obj2 != null)
			{
				short num = (short)obj2;
				if (num != 0)
				{
					hyperLink.TabIndex = num;
				}
			}
			hyperLink.RenderBeginTag(writer);
			Image image = new Image();
			if (base.ControlStyleCreated)
			{
				image.ApplyStyle(base.ControlStyle);
			}
			string alternateText = adArgs.AlternateText;
			if (!string.IsNullOrEmpty(alternateText))
			{
				image.AlternateText = alternateText;
			}
			else
			{
				IDictionary adProperties = adArgs.AdProperties;
				string text3 = ((this.AlternateTextField.Length != 0) ? this.AlternateTextField : "AlternateText");
				string text4 = ((adProperties == null) ? null : ((string)adProperties[text3]));
				if (text4 != null && text4.Length == 0)
				{
					image.GenerateEmptyAlternateText = true;
				}
			}
			image.UrlResolved = true;
			string imageUrl = adArgs.ImageUrl;
			if (!string.IsNullOrEmpty(imageUrl))
			{
				image.ImageUrl = imageUrl;
			}
			if (adArgs.HasWidth)
			{
				image.ControlStyle.Width = adArgs.Width;
			}
			if (adArgs.HasHeight)
			{
				image.ControlStyle.Height = adArgs.Height;
			}
			string text5 = (string)this.ViewState["ToolTip"];
			if (!string.IsNullOrEmpty(text5))
			{
				image.ToolTip = text5;
			}
			image.RenderControl(writer);
			hyperLink.RenderEndTag(writer);
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000F4B00 File Offset: 0x000F3B00
		private string ResolveAdRotatorUrl(string baseUrl, string relativeUrl)
		{
			if (relativeUrl == null || relativeUrl.Length == 0 || !UrlPath.IsRelativeUrl(relativeUrl) || baseUrl == null || baseUrl.Length == 0)
			{
				return relativeUrl;
			}
			return UrlPath.Combine(baseUrl, relativeUrl);
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x000F4B2C File Offset: 0x000F3B2C
		private IDictionary SelectAdFromRecords()
		{
			if (this._adRecs == null || this._adRecs.Length == 0)
			{
				return null;
			}
			string text = this.KeywordFilter;
			bool flag = string.IsNullOrEmpty(text);
			if (!flag)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
			}
			int num = 0;
			for (int i = 0; i < this._adRecs.Length; i++)
			{
				if (flag || this.MatchingAd(this._adRecs[i], text))
				{
					num += this._adRecs[i].impressions;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int randomNumber = AdRotator.GetRandomNumber(num);
			int num2 = 0;
			int num3 = -1;
			for (int j = 0; j < this._adRecs.Length; j++)
			{
				if (flag || this.MatchingAd(this._adRecs[j], text))
				{
					num2 += this._adRecs[j].impressions;
					if (randomNumber <= num2)
					{
						num3 = j;
						break;
					}
				}
			}
			return this._adRecs[num3].adProperties;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000F4C30 File Offset: 0x000F3C30
		private AdRotator.AdRec[] SetAdRecs(ArrayList adDicts)
		{
			if (adDicts == null || adDicts.Count == 0)
			{
				return null;
			}
			AdRotator.AdRec[] array = new AdRotator.AdRec[adDicts.Count];
			int num = 0;
			for (int i = 0; i < adDicts.Count; i++)
			{
				if (adDicts[i] != null)
				{
					array[num].Initialize((IDictionary)adDicts[i]);
					num++;
				}
			}
			return array;
		}

		// Token: 0x04002637 RID: 9783
		private const string XmlDocumentTag = "Advertisements";

		// Token: 0x04002638 RID: 9784
		private const string XmlDocumentRootXPath = "/Advertisements";

		// Token: 0x04002639 RID: 9785
		private const string XmlAdTag = "Ad";

		// Token: 0x0400263A RID: 9786
		private const string KeywordProperty = "Keyword";

		// Token: 0x0400263B RID: 9787
		private const string ImpressionsProperty = "Impressions";

		// Token: 0x0400263C RID: 9788
		private static readonly object EventAdCreated = new object();

		// Token: 0x0400263D RID: 9789
		private static Random _random;

		// Token: 0x0400263E RID: 9790
		private string _baseUrl;

		// Token: 0x0400263F RID: 9791
		private string _advertisementFile;

		// Token: 0x04002640 RID: 9792
		private AdCreatedEventArgs _adCreatedEventArgs;

		// Token: 0x04002641 RID: 9793
		private AdRotator.AdRec[] _adRecs;

		// Token: 0x04002642 RID: 9794
		private bool _isPostCacheAdHelper;

		// Token: 0x04002643 RID: 9795
		private string _uniqueID;

		// Token: 0x04002644 RID: 9796
		private static readonly Type _adrotatorType = typeof(AdRotator);

		// Token: 0x04002645 RID: 9797
		private static readonly Type[] _AdCreatedParameterTypes = new Type[] { typeof(AdCreatedEventArgs) };

		// Token: 0x020004BE RID: 1214
		private struct AdRec
		{
			// Token: 0x060039D8 RID: 14808 RVA: 0x000F4CD0 File Offset: 0x000F3CD0
			public void Initialize(IDictionary adProperties)
			{
				this.adProperties = adProperties;
				object obj = adProperties["Keyword"];
				if (obj != null && obj is string)
				{
					this.keyword = ((string)obj).Trim();
				}
				else
				{
					this.keyword = string.Empty;
				}
				string text = adProperties["Impressions"] as string;
				if (string.IsNullOrEmpty(text) || !int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out this.impressions))
				{
					this.impressions = 1;
				}
				if (this.impressions < 0)
				{
					this.impressions = 1;
				}
			}

			// Token: 0x04002646 RID: 9798
			public string keyword;

			// Token: 0x04002647 RID: 9799
			public int impressions;

			// Token: 0x04002648 RID: 9800
			public IDictionary adProperties;
		}
	}
}
