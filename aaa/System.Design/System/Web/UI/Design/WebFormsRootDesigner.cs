using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Resources;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	// Token: 0x020003AB RID: 939
	public abstract class WebFormsRootDesigner : IRootDesigner, IDesigner, IDisposable, IDesignerFilter
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x000BD221 File Offset: 0x000BC221
		// (set) Token: 0x060022B3 RID: 8883 RVA: 0x000BD229 File Offset: 0x000BC229
		public virtual IComponent Component
		{
			get
			{
				return this._component;
			}
			set
			{
				this._component = value;
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000BD234 File Offset: 0x000BC234
		~WebFormsRootDesigner()
		{
			this.Dispose(false);
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000BD264 File Offset: 0x000BC264
		public CultureInfo CurrentCulture
		{
			get
			{
				return CultureInfo.CurrentCulture;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060022B6 RID: 8886
		public abstract string DocumentUrl { get; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060022B7 RID: 8887
		public abstract bool IsDesignerViewLocked { get; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060022B8 RID: 8888
		public abstract bool IsLoading { get; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060022B9 RID: 8889
		public abstract WebFormsReferenceManager ReferenceManager { get; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x000BD26C File Offset: 0x000BC26C
		protected ViewTechnology[] SupportedTechnologies
		{
			get
			{
				return new ViewTechnology[] { ViewTechnology.Default };
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x000BD285 File Offset: 0x000BC285
		protected DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection();
			}
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000BD28C File Offset: 0x000BC28C
		protected internal virtual object GetService(Type serviceType)
		{
			if (this._component != null)
			{
				ISite site = this._component.Site;
				if (site != null)
				{
					return site.GetService(serviceType);
				}
			}
			return null;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000BD2B9 File Offset: 0x000BC2B9
		protected object GetView(ViewTechnology viewTechnology)
		{
			return null;
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060022BE RID: 8894 RVA: 0x000BD2BC File Offset: 0x000BC2BC
		// (remove) Token: 0x060022BF RID: 8895 RVA: 0x000BD2D5 File Offset: 0x000BC2D5
		public event EventHandler LoadComplete
		{
			add
			{
				this._loadCompleteHandler = (EventHandler)Delegate.Combine(this._loadCompleteHandler, value);
			}
			remove
			{
				this._loadCompleteHandler = (EventHandler)Delegate.Remove(this._loadCompleteHandler, value);
			}
		}

		// Token: 0x060022C0 RID: 8896
		public abstract void AddClientScriptToDocument(ClientScriptItem scriptItem);

		// Token: 0x060022C1 RID: 8897
		public abstract string AddControlToDocument(Control newControl, Control referenceControl, ControlLocation location);

		// Token: 0x060022C2 RID: 8898 RVA: 0x000BD2EE File Offset: 0x000BC2EE
		protected virtual DesignerActionService CreateDesignerActionService(IServiceProvider serviceProvider)
		{
			return new WebFormsDesignerActionService(serviceProvider);
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000BD2F6 File Offset: 0x000BC2F6
		protected virtual IUrlResolutionService CreateUrlResolutionService()
		{
			return new WebFormsRootDesigner.UrlResolutionService(this);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000BD300 File Offset: 0x000BC300
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)this.GetService(typeof(IPropertyValueUIService));
				if (propertyValueUIService != null)
				{
					propertyValueUIService.RemovePropertyValueUIHandler(new PropertyValueUIHandler(this.OnGetUIValueItem));
				}
				IServiceContainer serviceContainer = (IServiceContainer)this.GetService(typeof(IServiceContainer));
				if (serviceContainer != null)
				{
					if (this._urlResolutionService != null)
					{
						serviceContainer.RemoveService(typeof(IUrlResolutionService));
					}
					serviceContainer.RemoveService(typeof(IImplicitResourceProvider));
					if (this._designerActionService != null)
					{
						this._designerActionService.Dispose();
					}
					this._designerActionUIService.Dispose();
				}
				this._urlResolutionService = null;
				this._component = null;
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000BD3AB File Offset: 0x000BC3AB
		public virtual string GenerateEmptyDesignTimeHtml(Control control)
		{
			return this.GenerateErrorDesignTimeHtml(control, null, string.Empty);
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000BD3BC File Offset: 0x000BC3BC
		public virtual string GenerateErrorDesignTimeHtml(Control control, Exception e, string errorMessage)
		{
			string name = control.Site.Name;
			if (errorMessage == null)
			{
				errorMessage = string.Empty;
			}
			else
			{
				errorMessage = HttpUtility.HtmlEncode(errorMessage);
			}
			if (e != null)
			{
				errorMessage = errorMessage + "<br />" + HttpUtility.HtmlEncode(e.Message);
			}
			return string.Format(CultureInfo.InvariantCulture, ControlDesigner.ErrorDesignTimeHtmlTemplate, new object[]
			{
				SR.GetString("ControlDesigner_DesignTimeHtmlError"),
				HttpUtility.HtmlEncode(name),
				errorMessage
			});
		}

		// Token: 0x060022C7 RID: 8903
		public abstract ClientScriptItemCollection GetClientScriptsInDocument();

		// Token: 0x060022C8 RID: 8904
		protected internal abstract void GetControlViewAndTag(Control control, out IControlDesignerView view, out IControlDesignerTag tag);

		// Token: 0x060022C9 RID: 8905 RVA: 0x000BD438 File Offset: 0x000BC438
		public virtual void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(TemplateControl));
			this._component = component;
			IServiceContainer serviceContainer = (IServiceContainer)this.GetService(typeof(IServiceContainer));
			if (serviceContainer != null)
			{
				this._urlResolutionService = this.CreateUrlResolutionService();
				if (this._urlResolutionService != null)
				{
					serviceContainer.AddService(typeof(IUrlResolutionService), this._urlResolutionService);
				}
				this._designerActionService = this.CreateDesignerActionService(this._component.Site);
				this._designerActionUIService = new DesignerActionUIService(this._component.Site);
				ServiceCreatorCallback serviceCreatorCallback = new ServiceCreatorCallback(this.OnCreateService);
				serviceContainer.AddService(typeof(IImplicitResourceProvider), serviceCreatorCallback);
			}
			IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)this.GetService(typeof(IPropertyValueUIService));
			if (propertyValueUIService != null)
			{
				propertyValueUIService.AddPropertyValueUIHandler(new PropertyValueUIHandler(this.OnGetUIValueItem));
			}
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000BD518 File Offset: 0x000BC518
		private object OnCreateService(IServiceContainer container, Type serviceType)
		{
			if (serviceType == typeof(IImplicitResourceProvider))
			{
				if (this._implicitResourceProvider == null)
				{
					DesignTimeResourceProviderFactory designTimeResourceProviderFactory = ControlDesigner.GetDesignTimeResourceProviderFactory(this.Component.Site);
					IResourceProvider resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeLocalResourceProvider(this.Component.Site);
					this._implicitResourceProvider = resourceProvider as IImplicitResourceProvider;
					if (this._implicitResourceProvider == null)
					{
						this._implicitResourceProvider = new WebFormsRootDesigner.ImplicitResourceProvider(this);
					}
				}
				return this._implicitResourceProvider;
			}
			return null;
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000BD588 File Offset: 0x000BC588
		private void OnGetUIValueItem(ITypeDescriptorContext context, PropertyDescriptor propDesc, ArrayList valueUIItemList)
		{
			Control control = context.Instance as Control;
			if (control != null)
			{
				IDataBindingsAccessor dataBindingsAccessor = control;
				if (dataBindingsAccessor.HasDataBindings)
				{
					DataBinding dataBinding = dataBindingsAccessor.DataBindings[propDesc.Name];
					if (dataBinding != null)
					{
						valueUIItemList.Add(new WebFormsRootDesigner.DataBindingUIItem());
					}
				}
				IExpressionsAccessor expressionsAccessor = control;
				if (expressionsAccessor.HasExpressions)
				{
					ExpressionBinding expressionBinding = expressionsAccessor.Expressions[propDesc.Name];
					if (expressionBinding != null)
					{
						if (expressionBinding.Generated)
						{
							valueUIItemList.Add(new WebFormsRootDesigner.ImplicitExpressionUIItem());
							return;
						}
						valueUIItemList.Add(new WebFormsRootDesigner.ExpressionBindingUIItem());
					}
				}
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000BD612 File Offset: 0x000BC612
		protected virtual void OnLoadComplete(EventArgs e)
		{
			if (this._loadCompleteHandler != null)
			{
				this._loadCompleteHandler(this, e);
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000BD629 File Offset: 0x000BC629
		protected virtual void PostFilterAttributes(IDictionary attributes)
		{
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000BD62B File Offset: 0x000BC62B
		protected virtual void PostFilterEvents(IDictionary events)
		{
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000BD62D File Offset: 0x000BC62D
		protected virtual void PostFilterProperties(IDictionary properties)
		{
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000BD62F File Offset: 0x000BC62F
		protected virtual void PreFilterAttributes(IDictionary attributes)
		{
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000BD631 File Offset: 0x000BC631
		protected virtual void PreFilterEvents(IDictionary events)
		{
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000BD633 File Offset: 0x000BC633
		protected virtual void PreFilterProperties(IDictionary properties)
		{
		}

		// Token: 0x060022D3 RID: 8915
		public abstract void RemoveClientScriptFromDocument(string clientScriptId);

		// Token: 0x060022D4 RID: 8916
		public abstract void RemoveControlFromDocument(Control control);

		// Token: 0x060022D5 RID: 8917 RVA: 0x000BD638 File Offset: 0x000BC638
		public string ResolveUrl(string relativeUrl)
		{
			if (relativeUrl == null)
			{
				throw new ArgumentNullException("relativeUrl");
			}
			string text = this.DocumentUrl;
			if (text == null || text.Length == 0 || WebFormsRootDesigner.IsAppRelativePath(relativeUrl) || WebFormsRootDesigner.IsRooted(relativeUrl) || !WebFormsRootDesigner.IsAppRelativePath(text))
			{
				return relativeUrl;
			}
			text = text.Replace("~", "file://foo");
			Uri uri = new Uri(text, true);
			Uri uri2 = new Uri(uri, relativeUrl);
			string text2 = uri2.ToString();
			return text2.Replace("file://foo", "~");
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000BD6BC File Offset: 0x000BC6BC
		public virtual void SetControlID(Control control, string id)
		{
			ISite site = control.Site;
			site.Name = id;
			control.ID = id.Trim();
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000BD6E3 File Offset: 0x000BC6E3
		private static bool IsRooted(string basepath)
		{
			return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\';
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000BD708 File Offset: 0x000BC708
		private static bool IsAppRelativePath(string path)
		{
			return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000BD738 File Offset: 0x000BC738
		DesignerVerbCollection IDesigner.Verbs
		{
			get
			{
				return this.Verbs;
			}
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000BD740 File Offset: 0x000BC740
		void IDesigner.DoDefaultAction()
		{
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000BD742 File Offset: 0x000BC742
		void IDesignerFilter.PostFilterAttributes(IDictionary attributes)
		{
			this.PostFilterAttributes(attributes);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000BD74B File Offset: 0x000BC74B
		void IDesignerFilter.PostFilterEvents(IDictionary events)
		{
			this.PostFilterEvents(events);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000BD754 File Offset: 0x000BC754
		void IDesignerFilter.PostFilterProperties(IDictionary properties)
		{
			this.PostFilterProperties(properties);
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000BD75D File Offset: 0x000BC75D
		void IDesignerFilter.PreFilterAttributes(IDictionary attributes)
		{
			this.PreFilterAttributes(attributes);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000BD766 File Offset: 0x000BC766
		void IDesignerFilter.PreFilterEvents(IDictionary events)
		{
			this.PreFilterEvents(events);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000BD76F File Offset: 0x000BC76F
		void IDesignerFilter.PreFilterProperties(IDictionary properties)
		{
			this.PreFilterProperties(properties);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000BD778 File Offset: 0x000BC778
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x000BD787 File Offset: 0x000BC787
		ViewTechnology[] IRootDesigner.SupportedTechnologies
		{
			get
			{
				return this.SupportedTechnologies;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000BD78F File Offset: 0x000BC78F
		object IRootDesigner.GetView(ViewTechnology viewTechnology)
		{
			return this.GetView(viewTechnology);
		}

		// Token: 0x04001867 RID: 6247
		private const string dummyProtocolAndServer = "file://foo";

		// Token: 0x04001868 RID: 6248
		private const char appRelativeCharacter = '~';

		// Token: 0x04001869 RID: 6249
		private IComponent _component;

		// Token: 0x0400186A RID: 6250
		private EventHandler _loadCompleteHandler;

		// Token: 0x0400186B RID: 6251
		private IUrlResolutionService _urlResolutionService;

		// Token: 0x0400186C RID: 6252
		private DesignerActionService _designerActionService;

		// Token: 0x0400186D RID: 6253
		private DesignerActionUIService _designerActionUIService;

		// Token: 0x0400186E RID: 6254
		private IImplicitResourceProvider _implicitResourceProvider;

		// Token: 0x020003AC RID: 940
		private sealed class DataBindingUIItem : PropertyValueUIItem
		{
			// Token: 0x060022E5 RID: 8933 RVA: 0x000BD7A0 File Offset: 0x000BC7A0
			public DataBindingUIItem()
				: base(WebFormsRootDesigner.DataBindingUIItem.DataBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.DataBindingUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.DataBindingUIItem.DataBindingToolTip)
			{
			}

			// Token: 0x17000670 RID: 1648
			// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000BD7BE File Offset: 0x000BC7BE
			private static Bitmap DataBindingBitmap
			{
				get
				{
					if (WebFormsRootDesigner.DataBindingUIItem._dataBindingBitmap == null)
					{
						WebFormsRootDesigner.DataBindingUIItem._dataBindingBitmap = new Bitmap(typeof(WebFormsRootDesigner), "DataBindingGlyph.bmp");
						WebFormsRootDesigner.DataBindingUIItem._dataBindingBitmap.MakeTransparent(Color.Fuchsia);
					}
					return WebFormsRootDesigner.DataBindingUIItem._dataBindingBitmap;
				}
			}

			// Token: 0x17000671 RID: 1649
			// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000BD7F4 File Offset: 0x000BC7F4
			private static string DataBindingToolTip
			{
				get
				{
					if (WebFormsRootDesigner.DataBindingUIItem._dataBindingToolTip == null)
					{
						WebFormsRootDesigner.DataBindingUIItem._dataBindingToolTip = SR.GetString("DataBindingGlyph_ToolTip");
					}
					return WebFormsRootDesigner.DataBindingUIItem._dataBindingToolTip;
				}
			}

			// Token: 0x060022E8 RID: 8936 RVA: 0x000BD811 File Offset: 0x000BC811
			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			// Token: 0x0400186F RID: 6255
			private static Bitmap _dataBindingBitmap;

			// Token: 0x04001870 RID: 6256
			private static string _dataBindingToolTip;
		}

		// Token: 0x020003AD RID: 941
		private sealed class ExpressionBindingUIItem : PropertyValueUIItem
		{
			// Token: 0x060022E9 RID: 8937 RVA: 0x000BD813 File Offset: 0x000BC813
			public ExpressionBindingUIItem()
				: base(WebFormsRootDesigner.ExpressionBindingUIItem.ExpressionBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.ExpressionBindingUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.ExpressionBindingUIItem.ExpressionBindingToolTip)
			{
			}

			// Token: 0x17000672 RID: 1650
			// (get) Token: 0x060022EA RID: 8938 RVA: 0x000BD831 File Offset: 0x000BC831
			private static Bitmap ExpressionBindingBitmap
			{
				get
				{
					if (WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingBitmap == null)
					{
						WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingBitmap = new Bitmap(typeof(WebFormsRootDesigner), "ExpressionBindingGlyph.bmp");
						WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingBitmap.MakeTransparent(Color.Fuchsia);
					}
					return WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingBitmap;
				}
			}

			// Token: 0x17000673 RID: 1651
			// (get) Token: 0x060022EB RID: 8939 RVA: 0x000BD867 File Offset: 0x000BC867
			private static string ExpressionBindingToolTip
			{
				get
				{
					if (WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingToolTip == null)
					{
						WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingToolTip = SR.GetString("ExpressionBindingGlyph_ToolTip");
					}
					return WebFormsRootDesigner.ExpressionBindingUIItem._expressionBindingToolTip;
				}
			}

			// Token: 0x060022EC RID: 8940 RVA: 0x000BD884 File Offset: 0x000BC884
			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			// Token: 0x04001871 RID: 6257
			private static Bitmap _expressionBindingBitmap;

			// Token: 0x04001872 RID: 6258
			private static string _expressionBindingToolTip;
		}

		// Token: 0x020003AE RID: 942
		private sealed class ImplicitExpressionUIItem : PropertyValueUIItem
		{
			// Token: 0x060022ED RID: 8941 RVA: 0x000BD886 File Offset: 0x000BC886
			public ImplicitExpressionUIItem()
				: base(WebFormsRootDesigner.ImplicitExpressionUIItem.ImplicitExpressionBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.ImplicitExpressionUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.ImplicitExpressionUIItem.ImplicitExpressionBindingToolTip)
			{
			}

			// Token: 0x17000674 RID: 1652
			// (get) Token: 0x060022EE RID: 8942 RVA: 0x000BD8A4 File Offset: 0x000BC8A4
			private static Bitmap ImplicitExpressionBindingBitmap
			{
				get
				{
					if (WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingBitmap == null)
					{
						WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingBitmap = new Bitmap(typeof(WebFormsRootDesigner), "ImplicitExpressionBindingGlyph.bmp");
						WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingBitmap.MakeTransparent(Color.Fuchsia);
					}
					return WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingBitmap;
				}
			}

			// Token: 0x17000675 RID: 1653
			// (get) Token: 0x060022EF RID: 8943 RVA: 0x000BD8DA File Offset: 0x000BC8DA
			private static string ImplicitExpressionBindingToolTip
			{
				get
				{
					if (WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingToolTip == null)
					{
						WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingToolTip = SR.GetString("ImplicitExpressionBindingGlyph_ToolTip");
					}
					return WebFormsRootDesigner.ImplicitExpressionUIItem._expressionBindingToolTip;
				}
			}

			// Token: 0x060022F0 RID: 8944 RVA: 0x000BD8F7 File Offset: 0x000BC8F7
			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			// Token: 0x04001873 RID: 6259
			private static Bitmap _expressionBindingBitmap;

			// Token: 0x04001874 RID: 6260
			private static string _expressionBindingToolTip;
		}

		// Token: 0x020003AF RID: 943
		private sealed class UrlResolutionService : IUrlResolutionService
		{
			// Token: 0x060022F1 RID: 8945 RVA: 0x000BD8F9 File Offset: 0x000BC8F9
			public UrlResolutionService(WebFormsRootDesigner owner)
			{
				this._owner = owner;
			}

			// Token: 0x060022F2 RID: 8946 RVA: 0x000BD908 File Offset: 0x000BC908
			string IUrlResolutionService.ResolveClientUrl(string relativeUrl)
			{
				if (relativeUrl == null)
				{
					throw new ArgumentNullException("relativeUrl");
				}
				if (!WebFormsRootDesigner.IsAppRelativePath(relativeUrl))
				{
					return relativeUrl;
				}
				string text = this._owner.DocumentUrl;
				if (text == null || text.Length == 0 || !WebFormsRootDesigner.IsAppRelativePath(text))
				{
					return relativeUrl.Substring(2);
				}
				text = text.Replace("~", "file://foo");
				Uri uri = new Uri(text, true);
				relativeUrl = relativeUrl.Replace("~", "file://foo");
				Uri uri2 = new Uri(relativeUrl, true);
				string text2 = uri.MakeRelativeUri(uri2).ToString();
				return text2.Replace("file://foo", string.Empty);
			}

			// Token: 0x04001875 RID: 6261
			private WebFormsRootDesigner _owner;
		}

		// Token: 0x020003B0 RID: 944
		private sealed class ImplicitResourceProvider : IImplicitResourceProvider
		{
			// Token: 0x060022F3 RID: 8947 RVA: 0x000BD9A4 File Offset: 0x000BC9A4
			public ImplicitResourceProvider(WebFormsRootDesigner owner)
			{
				this._owner = owner;
			}

			// Token: 0x060022F4 RID: 8948 RVA: 0x000BD9B3 File Offset: 0x000BC9B3
			object IImplicitResourceProvider.GetObject(ImplicitResourceKey key, CultureInfo culture)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022F5 RID: 8949 RVA: 0x000BD9BC File Offset: 0x000BC9BC
			ICollection IImplicitResourceProvider.GetImplicitResourceKeys(string keyPrefix)
			{
				IDictionary pageResources = this.GetPageResources();
				return pageResources[keyPrefix] as ICollection;
			}

			// Token: 0x060022F6 RID: 8950 RVA: 0x000BD9DC File Offset: 0x000BC9DC
			private IDictionary GetPageResources()
			{
				if (this._owner.Component == null)
				{
					return null;
				}
				IServiceProvider site = this._owner.Component.Site;
				if (site == null)
				{
					return null;
				}
				DesignTimeResourceProviderFactory designTimeResourceProviderFactory = ControlDesigner.GetDesignTimeResourceProviderFactory(site);
				if (designTimeResourceProviderFactory == null)
				{
					return null;
				}
				IResourceProvider resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeLocalResourceProvider(site);
				if (resourceProvider == null)
				{
					return null;
				}
				IResourceReader resourceReader = resourceProvider.ResourceReader;
				if (resourceReader == null)
				{
					return null;
				}
				IDictionary dictionary = new HybridDictionary(true);
				if (resourceReader != null)
				{
					foreach (object obj in resourceReader)
					{
						string text = (string)((DictionaryEntry)obj).Key;
						string text2 = string.Empty;
						if (text.IndexOf(':') > 0)
						{
							string[] array = text.Split(new char[] { ':' });
							if (array.Length > 2)
							{
								continue;
							}
							text2 = array[0];
							text = array[1];
						}
						int num = text.IndexOf('.');
						if (num > 0)
						{
							string text3 = text.Substring(0, num);
							string text4 = text.Substring(num + 1);
							ArrayList arrayList = (ArrayList)dictionary[text3];
							if (arrayList == null)
							{
								arrayList = new ArrayList();
								dictionary[text3] = arrayList;
							}
							arrayList.Add(new ImplicitResourceKey
							{
								Filter = text2,
								Property = text4,
								KeyPrefix = text3
							});
						}
					}
				}
				return dictionary;
			}

			// Token: 0x04001876 RID: 6262
			private WebFormsRootDesigner _owner;
		}
	}
}
