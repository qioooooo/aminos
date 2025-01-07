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
	public abstract class WebFormsRootDesigner : IRootDesigner, IDesigner, IDisposable, IDesignerFilter
	{
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

		~WebFormsRootDesigner()
		{
			this.Dispose(false);
		}

		public CultureInfo CurrentCulture
		{
			get
			{
				return CultureInfo.CurrentCulture;
			}
		}

		public abstract string DocumentUrl { get; }

		public abstract bool IsDesignerViewLocked { get; }

		public abstract bool IsLoading { get; }

		public abstract WebFormsReferenceManager ReferenceManager { get; }

		protected ViewTechnology[] SupportedTechnologies
		{
			get
			{
				return new ViewTechnology[] { ViewTechnology.Default };
			}
		}

		protected DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection();
			}
		}

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

		protected object GetView(ViewTechnology viewTechnology)
		{
			return null;
		}

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

		public abstract void AddClientScriptToDocument(ClientScriptItem scriptItem);

		public abstract string AddControlToDocument(Control newControl, Control referenceControl, ControlLocation location);

		protected virtual DesignerActionService CreateDesignerActionService(IServiceProvider serviceProvider)
		{
			return new WebFormsDesignerActionService(serviceProvider);
		}

		protected virtual IUrlResolutionService CreateUrlResolutionService()
		{
			return new WebFormsRootDesigner.UrlResolutionService(this);
		}

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

		public virtual string GenerateEmptyDesignTimeHtml(Control control)
		{
			return this.GenerateErrorDesignTimeHtml(control, null, string.Empty);
		}

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

		public abstract ClientScriptItemCollection GetClientScriptsInDocument();

		protected internal abstract void GetControlViewAndTag(Control control, out IControlDesignerView view, out IControlDesignerTag tag);

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

		protected virtual void OnLoadComplete(EventArgs e)
		{
			if (this._loadCompleteHandler != null)
			{
				this._loadCompleteHandler(this, e);
			}
		}

		protected virtual void PostFilterAttributes(IDictionary attributes)
		{
		}

		protected virtual void PostFilterEvents(IDictionary events)
		{
		}

		protected virtual void PostFilterProperties(IDictionary properties)
		{
		}

		protected virtual void PreFilterAttributes(IDictionary attributes)
		{
		}

		protected virtual void PreFilterEvents(IDictionary events)
		{
		}

		protected virtual void PreFilterProperties(IDictionary properties)
		{
		}

		public abstract void RemoveClientScriptFromDocument(string clientScriptId);

		public abstract void RemoveControlFromDocument(Control control);

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

		public virtual void SetControlID(Control control, string id)
		{
			ISite site = control.Site;
			site.Name = id;
			control.ID = id.Trim();
		}

		private static bool IsRooted(string basepath)
		{
			return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\';
		}

		private static bool IsAppRelativePath(string path)
		{
			return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
		}

		DesignerVerbCollection IDesigner.Verbs
		{
			get
			{
				return this.Verbs;
			}
		}

		void IDesigner.DoDefaultAction()
		{
		}

		void IDesignerFilter.PostFilterAttributes(IDictionary attributes)
		{
			this.PostFilterAttributes(attributes);
		}

		void IDesignerFilter.PostFilterEvents(IDictionary events)
		{
			this.PostFilterEvents(events);
		}

		void IDesignerFilter.PostFilterProperties(IDictionary properties)
		{
			this.PostFilterProperties(properties);
		}

		void IDesignerFilter.PreFilterAttributes(IDictionary attributes)
		{
			this.PreFilterAttributes(attributes);
		}

		void IDesignerFilter.PreFilterEvents(IDictionary events)
		{
			this.PreFilterEvents(events);
		}

		void IDesignerFilter.PreFilterProperties(IDictionary properties)
		{
			this.PreFilterProperties(properties);
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		ViewTechnology[] IRootDesigner.SupportedTechnologies
		{
			get
			{
				return this.SupportedTechnologies;
			}
		}

		object IRootDesigner.GetView(ViewTechnology viewTechnology)
		{
			return this.GetView(viewTechnology);
		}

		private const string dummyProtocolAndServer = "file://foo";

		private const char appRelativeCharacter = '~';

		private IComponent _component;

		private EventHandler _loadCompleteHandler;

		private IUrlResolutionService _urlResolutionService;

		private DesignerActionService _designerActionService;

		private DesignerActionUIService _designerActionUIService;

		private IImplicitResourceProvider _implicitResourceProvider;

		private sealed class DataBindingUIItem : PropertyValueUIItem
		{
			public DataBindingUIItem()
				: base(WebFormsRootDesigner.DataBindingUIItem.DataBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.DataBindingUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.DataBindingUIItem.DataBindingToolTip)
			{
			}

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

			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			private static Bitmap _dataBindingBitmap;

			private static string _dataBindingToolTip;
		}

		private sealed class ExpressionBindingUIItem : PropertyValueUIItem
		{
			public ExpressionBindingUIItem()
				: base(WebFormsRootDesigner.ExpressionBindingUIItem.ExpressionBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.ExpressionBindingUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.ExpressionBindingUIItem.ExpressionBindingToolTip)
			{
			}

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

			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			private static Bitmap _expressionBindingBitmap;

			private static string _expressionBindingToolTip;
		}

		private sealed class ImplicitExpressionUIItem : PropertyValueUIItem
		{
			public ImplicitExpressionUIItem()
				: base(WebFormsRootDesigner.ImplicitExpressionUIItem.ImplicitExpressionBindingBitmap, new PropertyValueUIItemInvokeHandler(WebFormsRootDesigner.ImplicitExpressionUIItem.OnValueUIItemInvoke), WebFormsRootDesigner.ImplicitExpressionUIItem.ImplicitExpressionBindingToolTip)
			{
			}

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

			private static void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
			{
			}

			private static Bitmap _expressionBindingBitmap;

			private static string _expressionBindingToolTip;
		}

		private sealed class UrlResolutionService : IUrlResolutionService
		{
			public UrlResolutionService(WebFormsRootDesigner owner)
			{
				this._owner = owner;
			}

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

			private WebFormsRootDesigner _owner;
		}

		private sealed class ImplicitResourceProvider : IImplicitResourceProvider
		{
			public ImplicitResourceProvider(WebFormsRootDesigner owner)
			{
				this._owner = owner;
			}

			object IImplicitResourceProvider.GetObject(ImplicitResourceKey key, CultureInfo culture)
			{
				throw new NotSupportedException();
			}

			ICollection IImplicitResourceProvider.GetImplicitResourceKeys(string keyPrefix)
			{
				IDictionary pageResources = this.GetPageResources();
				return pageResources[keyPrefix] as ICollection;
			}

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

			private WebFormsRootDesigner _owner;
		}
	}
}
